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

namespace CRS.Forms.Dictionaries
{
    public partial class FCashOtherAppointmentAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCashOtherAppointmentAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, Name, ID;
        int NameUsedUserID = -1;
        bool NameUsed = false, CurrentStatus = false;

        public delegate void DoEvent();
        public event DoEvent RefreshAppointmentDataGridView;

        private void FStaticValueEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")               
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_APPOINTMENTS", GlobalVariables.V_UserID, "WHERE ID = " + ID + " AND USED_USER_ID = -1");
                LoadAppointmentDetails();
                NameUsed = (NameUsedUserID >= 0);
                
                if (NameUsed)
                {
                    if (GlobalVariables.V_UserID != NameUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == NameUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş təyinat hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş statik məlumatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);                
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            NoteText.Enabled = !status;            
            BOK.Visible = !status;
        }

        private void LoadAppointmentDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.CASH_APPOINTMENTS WHERE TYPE = 1 AND ID = {ID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAppointmentDetails");
            if(dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                NameUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private void InsertAppointment()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CASH_APPOINTMENTS") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CASH_APPOINTMENTS(ID,NAME,NOTE,ORDER_ID,TYPE)VALUES(CASH_APPOINTMENT_SEQUENCE.NEXTVAL,'{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order},1)",
                                                "Təyinat daxil edilmədi.",
                                                this.Name + "/InsertAppointment");
        }

        private void UpdateAppointment()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CASH_APPOINTMENTS SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}' WHERE ID = {ID}",
                                                "Təyinat dəyişdirilmədi.",
                                                this.Name + "/UpdateAppointment");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private bool ControlAppointmentDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(NameText.Text))
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tətinatın adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlAppointmentDetails())
            {
                if (TransactionName == "INSERT")
                    InsertAppointment();
                else
                    UpdateAppointment();
                this.Close();
            }
        }

        private void FStaticValueEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_APPOINTMENTS", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshAppointmentDataGridView();
        }
    }
}