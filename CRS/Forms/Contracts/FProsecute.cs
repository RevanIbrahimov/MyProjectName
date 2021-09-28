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
    public partial class FProsecute : DevExpress.XtraEditors.XtraForm
    {
        public FProsecute()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID, CustomerName, DetailID, ContractStartDate;

        public delegate void DoEvent();
        public event DoEvent RefreshDetailsDataGridView;

        int law_id, law_status_id;

        private void FProsecute_Load(object sender, EventArgs e)
        {
            GlobalProcedures.DateEditFormat(StartDate);
            StartDate.EditValue = DateTime.Today;
            StartDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(ContractStartDate, "ddmmyyyy");
            GlobalProcedures.FillComboBoxEdit(LawComboBox, "LAWS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
            GlobalProcedures.FillComboBoxEdit(StatusComboBox, "LAW_STATUS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");

            if (TransactionName == "EDIT")
                LoadProsecuteDetails();
            else
            {
                DefandantNameText.Text = CustomerName;
                ApplicantText.Text = GlobalFunctions.ReadSetting("Company");
            }
        }

        private void LoadProsecuteDetails()
        {
            string s = "SELECT START_DATE,L.NAME LAW_NAME,CL.JUDGE_NAME,CL.REPRESENTATIVE,LS.NAME STATUS_NAME,CL.NOTE,CL.DEFANDANT_NAME,CL.APPLICANT FROM CRS_USER.CONTRACT_LAWS CL,CRS_USER.LAWS L,CRS_USER.LAW_STATUS LS WHERE CL.LAW_ID = L.ID AND CL.LAW_STATUS_ID = LS.ID AND CL.ID = " + DetailID;

            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadProsecuteDetails", "Məhkəmə iclasının məlumatları tapılmadı.").Rows)
            {
                StartDate.EditValue = dr["START_DATE"];
                LawComboBox.EditValue = dr["LAW_NAME"].ToString();
                JudgeNameText.EditValue = dr["JUDGE_NAME"];
                RepresentativeText.EditValue = dr["REPRESENTATIVE"];
                StatusComboBox.EditValue = dr["STATUS_NAME"];
                NoteText.EditValue = dr["NOTE"];
                DefandantNameText.EditValue = dr["DEFANDANT_NAME"];
                ApplicantText.EditValue = dr["APPLICANT"];
            }
        }

        private void FProsecute_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDetailsDataGridView();
        }

        void RefreshLaws()
        {
            GlobalProcedures.FillComboBoxEdit(LawComboBox, "LAWS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
        }

        void RefreshLawStatus()
        {
            GlobalProcedures.FillComboBoxEdit(StatusComboBox, "LAW_STATUS", "NAME,NAME,NAME", "1 = 1 ORDER BY 1");
        }

        private void LawComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FLaws fl = new FLaws();
                fl.RefreshLawsData += new FLaws.DoEvent(RefreshLaws);
                fl.ShowDialog();
            }
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

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            law_status_id = GlobalFunctions.FindComboBoxSelectedValue("LAW_STATUS", "NAME", "ID", StatusComboBox.Text);
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private void BRepeat_Click(object sender, EventArgs e)
        {
            string a = ApplicantText.Text, b = DefandantNameText.Text;

            ApplicantText.Text = b;
            DefandantNameText.Text = a;
        }

        private void LawComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            law_id = GlobalFunctions.FindComboBoxSelectedValue("LAWS", "NAME", "ID", LawComboBox.Text);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlProsecute())
            {
                if (TransactionName == "INSERT")
                    InsertProsecute();
                else
                    UpdateProsecute();
                this.Close();
            }
        }

        private bool ControlProsecute()
        {
            bool b = false;

            if (String.IsNullOrWhiteSpace(StartDate.Text))
            {
                StartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məhkəməyə verilmə tarixi daxil edilməyib.");
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (law_id <= 0)
            {
                LawComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məhkəmənin adı daxil edilməyib.");
                LawComboBox.Focus();
                LawComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

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

            if (String.IsNullOrWhiteSpace(ApplicantText.Text))
            {
                ApplicantText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İddiaçı daxil edilməyib.");
                ApplicantText.Focus();
                ApplicantText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrWhiteSpace(DefandantNameText.Text))
            {
                DefandantNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cavabdeh şəxs daxil edilməyib.");
                DefandantNameText.Focus();
                DefandantNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertProsecute()
        {
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CONTRACT_LAWS(CONTRACT_ID,DEFANDANT_NAME,LAW_ID,JUDGE_NAME,REPRESENTATIVE,LAW_STATUS_ID,START_DATE,NOTE,CREATED_USER_NAME,APPLICANT) VALUES(" + ContractID + ",'" + DefandantNameText.Text.Trim() + "'," + law_id + ",'" + JudgeNameText.Text.Trim() + "','" + RepresentativeText.Text.Trim() + "'," + law_status_id + ",TO_DATE('" + StartDate.Text + "','DD.MM.YYYY'),'" + NoteText.Text.Trim() + "','" + GlobalVariables.V_UserName + "','" + ApplicantText.Text.Trim() + "')",
                                             "Məhkəməyə verilmə daxil edilmədi.",
                                             this.Name + "/InsertProsecute");
        }

        private void UpdateProsecute()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CONTRACT_LAWS SET DEFANDANT_NAME = '" + DefandantNameText.Text.Trim() + "',LAW_ID = " + law_id + ",JUDGE_NAME = '" + JudgeNameText.Text.Trim() + "', REPRESENTATIVE = '" + RepresentativeText.Text.Trim() + "', LAW_STATUS_ID = " + law_status_id + ", START_DATE = TO_DATE('" + StartDate.Text + "','DD.MM.YYYY'),NOTE = '" + NoteText.Text.Trim() + "',APPLICANT = '" + ApplicantText.Text.Trim() + "' WHERE ID = " + DetailID,
                                                "Məhkəməyə verilmə dəyişdirilmədi.",
                                             this.Name + "/UpdateProsecute");
        }
    }
}