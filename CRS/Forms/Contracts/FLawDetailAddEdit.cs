using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms.Contracts
{
    public partial class FLawDetailAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FLawDetailAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID, DetailID, DefandantName, StartDate, ParentID, JudgeName, CustomerName, Applicant;
        public int LawID;
        int law_status_id, max_detail_id;

        public delegate void DoEvent();
        public event DoEvent RefreshDetailsDataGridView;

        private void FLawDetailAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.DateEditFormat(NextDate);
            DefandantNameText.Text = DefandantName;
            ApplicantText.Text = Applicant;
            CustomerNameText.Text = CustomerName;
            StartDateText.Text = StartDate;
            DateTime max_next_date = GlobalFunctions.GetMaxDate($@"SELECT MAX(NVL(NEXT_DATE,START_DATE)) FROM CRS_USER.CONTRACT_LAWS CL WHERE NVL(CL.PARENT_ID,CL.ID) = {ParentID}");
            NextDate.Properties.MinValue = max_next_date;
            GlobalProcedures.FillComboBoxEdit(StatusComboBox, "LAW_STATUS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
            max_detail_id = GlobalFunctions.GetID($@"SELECT MAX(ID) FROM CRS_USER.CONTRACT_LAWS WHERE CONTRACT_ID = {ContractID}");
            if (TransactionName == "EDIT")
                LoadLawDetails();
            else
            {
                if (max_next_date > DateTime.Today)
                    NextDate.EditValue = max_next_date;
                else
                    NextDate.EditValue = DateTime.Today;

                JudgeNameText.Text = JudgeName;
                LawNameText.Text = GlobalFunctions.GetName($@"SELECT NAME FROM CRS_USER.LAWS WHERE ID = {LawID}");
            }
        }

        private void LoadLawDetails()
        {
            string s = $@"SELECT L.NAME LAW_NAME,CL.JUDGE_NAME,LS.NAME STATUS_NAME,NEXT_DATE,TO_CHAR(NEXT_DATE,'HH24') NEXT_TIME_HOUR,TO_CHAR(NEXT_DATE,'MI') NEXT_TIME_MINUTE,TO_CHAR(NEXT_DATE,'SS') NEXT_TIME_SECOND,CL.NOTE,CL.REPRESENTATIVE FROM CRS_USER.CONTRACT_LAWS CL,CRS_USER.LAWS L,CRS_USER.LAW_STATUS LS WHERE CL.LAW_ID = L.ID AND CL.LAW_STATUS_ID = LS.ID AND CL.ID = {DetailID} ORDER BY CL.ID";
            int next_time_hour, next_time_minute, next_time_second;

            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadLawDetails", "Məhkəmə iclasının məlumatları tapılmadı.").Rows)
            {
                LawNameText.EditValue = dr["LAW_NAME"];
                JudgeNameText.EditValue = dr["JUDGE_NAME"];
                StatusComboBox.EditValue = dr["STATUS_NAME"];
                NextDate.EditValue = dr["NEXT_DATE"];

                next_time_hour = Convert.ToInt32(dr["NEXT_TIME_HOUR"]);
                next_time_minute = Convert.ToInt32(dr["NEXT_TIME_MINUTE"]);
                next_time_second = Convert.ToInt32(dr["NEXT_TIME_SECOND"]);
                NoteText.Text = dr["NOTE"].ToString();
                NextTime.EditValue = new TimeSpan(next_time_hour, next_time_minute, next_time_second);
                RepresentativeText.EditValue = dr["REPRESENTATIVE"];
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void RefreshLawStatus()
        {
            GlobalProcedures.FillComboBoxEdit(StatusComboBox, "LAW_STATUS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
        }

        private void StatusComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FLawStatus fls = new FLawStatus();
                fls.RefreshLawStatusData += new FLawStatus.DoEvent(RefreshLawStatus);
                fls.ShowDialog();
            }
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            law_status_id = GlobalFunctions.FindComboBoxSelectedValue("LAW_STATUS", "NAME", "ID", StatusComboBox.Text);
        }

        private bool ControlLawDetails()
        {
            bool b = false;

            if (law_status_id <= 0)
            {
                StatusComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İclasın statusu daxil edilməyib.");
                StatusComboBox.Focus();
                StatusComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrWhiteSpace(NextDate.Text))
            {
                NextDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İclasın növbəti tarixi daxil edilməyib.");
                NextDate.Focus();
                NextDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.ChangeStringToDate(NextDate.Text, "ddmmyyyy") < GlobalFunctions.ChangeStringToDate(StartDateText.Text, "ddmmyyyy"))
            {
                NextDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İclasın növbəti tarixi məhkəməyə verilmə tarixindən (" + StartDateText.Text + ") kiçik ola bilməz.");
                NextDate.Focus();
                NextDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrWhiteSpace(NextTime.Text) || NextTime.Text == "00:00:00")
            {
                NextTime.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İclasın növbəti tarixi daxil edilməyib.");
                NextTime.Focus();
                NextTime.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlLawDetails())
            {
                if (TransactionName == "INSERT")
                    InsertLawDetail();
                else
                    UpdateLawDetail();
                this.Close();
            }
        }

        private void InsertLawDetail()
        {
            string nextdate_and_time = NextDate.Text + " " + NextTime.Text;
            GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.CONTRACT_LAWS(PARENT_ID,CONTRACT_ID,DEFANDANT_NAME,APPLICANT,LAW_ID,JUDGE_NAME,REPRESENTATIVE,LAW_STATUS_ID,START_DATE,NEXT_DATE,NOTE,CREATED_USER_NAME) VALUES(" + ParentID + "," + ContractID + ",'" + DefandantName.Trim() + "','" + Applicant.Trim() + "'," + LawID + ",'" + JudgeNameText.Text.Trim() + "','" + RepresentativeText.Text.Trim() + "'," + law_status_id + ",TO_DATE('" + StartDate + "','DD.MM.YYYY'),TO_DATE('" + nextdate_and_time.Trim() + "','DD.MM.YYYY HH24:MI:SS'),'" + NoteText.Text.Trim() + "','" + GlobalVariables.V_UserName + "')",
                                             $@"UPDATE CRS_USER.CONTRACT_LAWS CL SET CL.LAST_DATE = (SELECT NEXT_DATE FROM CRS_USER.CONTRACT_LAWS WHERE ID = {max_detail_id}) WHERE CL.ID = {max_detail_id}",
                                                "Məhkəmə iclasının detalları daxil edilmədi.",
                                             this.Name + "/InsertLawDetail");
        }

        private void UpdateLawDetail()
        {
            string nextdate_and_time = NextDate.Text + " " + NextTime.Text;
            GlobalProcedures.ExecuteTwoQuery($@"UPDATE CRS_USER.CONTRACT_LAWS SET JUDGE_NAME = '{JudgeNameText.Text.Trim()}',REPRESENTATIVE = '{RepresentativeText.Text.Trim()}',LAW_STATUS_ID = {law_status_id},NEXT_DATE = TO_DATE('{nextdate_and_time.Trim()}','DD.MM.YYYY HH24:MI:SS'),NOTE = '{NoteText.Text.Trim()}',CREATED_USER_NAME = '{GlobalVariables.V_UserName}' WHERE ID = {DetailID}",
                                             $@"UPDATE CRS_USER.CONTRACT_LAWS CL SET CL.LAST_DATE = TO_DATE('{nextdate_and_time.Trim()}','DD.MM.YYYY HH24:MI:SS') WHERE CL.ID = {DetailID} AND CL.LAST_DATE IS NOT NULL AND CONTRACT_ID = {ContractID}",
                                                "Məhkəmə iclasının detalları dəyişdirilmədi.",
                                             this.Name + "/UpdateLawDetail");
        }

        private void FLawDetailAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDetailsDataGridView();
        }
    }
}