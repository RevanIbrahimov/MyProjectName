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
    public partial class FAppointmentAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAppointmentAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, AppointmentID , ID;
        bool CurrentStatus = false, AppointmentUsed = false;

        int AppointmentUsedUserID = -1, operation_type_id = 0, appointment_type_id = 0;

        public delegate void DoEvent();
        public event DoEvent RefreshAppointmentsDataGridView;
        public event DoEvent RefreshDataGridView;

        private void FAppointmentAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillComboBoxEdit(OperationTypeComboBox, "OPERATION_TYPES", "TYPE_AZ,TYPE_EN,TYPE_RU", null);
            GlobalProcedures.FillLookUpEdit(AppointmentTypeLookUp, "APPOINTMENT_TYPE", "ID", "NAME", "1 = 1 ORDER BY ID");
            if (TransactionName == "EDIT")              
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BANK_APPOINTMENTS", GlobalVariables.V_UserID, "WHERE ID = " + AppointmentID + " AND USED_USER_ID = -1");
                LoadAppointmentDetails();
                AppointmentUsed = (AppointmentUsedUserID >= 0);
                //AppointmentUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.BANK_APPOINTMENTS WHERE ID = {AppointmentID}");
                
                if (AppointmentUsed)
                {
                    if (GlobalVariables.V_UserID != AppointmentUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == AppointmentUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş təyinat hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş təyinatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            OperationTypeComboBox.Enabled = 
                AppointmentTypeLookUp.Enabled =
                AzNameText.Enabled = 
                EnNameText.Enabled = 
                RuNameText.Enabled = 
                NoteText.Enabled = 
                BOK.Visible = !status;
        }

        private void LoadAppointmentDetails()
        {
            string s = $@"SELECT BA.NAME,
                                   BA.NAME_EN,
                                   BA.NAME_RU,
                                   BA.NOTE,
                                   OT.TYPE_AZ TYPE,
                                   BA.USED_USER_ID,
                                   AT.NAME APPOINTMENT_TYPE_NAME
                              FROM CRS_USER.BANK_APPOINTMENTS BA,
                                   CRS_USER.OPERATION_TYPES OT,
                                   CRS_USER.APPOINTMENT_TYPE AT
                             WHERE     BA.OPERATION_TYPE_ID = OT.ID
                                   AND BA.APPOINTMENT_TYPE_ID = AT.ID
                                   AND BA.ID = {AppointmentID}";
            try
            {                
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAppointmentDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    AzNameText.Text = dr["NAME"].ToString();
                    EnNameText.Text = dr["NAME_EN"].ToString();
                    RuNameText.Text = dr["NAME_RU"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();                   
                    OperationTypeComboBox.Enabled = false;
                    OperationTypeComboBox.EditValue = dr["TYPE"].ToString();
                    AppointmentUsedUserID = Convert.ToInt32(dr["USED_USER_ID"]);
                    AppointmentTypeLookUp.EditValue = AppointmentTypeLookUp.Properties.GetKeyValueByDisplayText(dr["APPOINTMENT_TYPE_NAME"].ToString());
                }                
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təyinat açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlNameDetails()
        {
            bool b = false;

            if(operation_type_id <= 0)
            {
                OperationTypeComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əməliyyatın növü seçilməyib.");
                OperationTypeComboBox.Focus();
                OperationTypeComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AppointmentTypeLookUp.EditValue == null)
            {
                AppointmentTypeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təyinatın növü seçilməyib.");
                AppointmentTypeLookUp.Focus();
                AppointmentTypeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AzNameText.Text.Length == 0)
            {
                AzNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təyinatın azərbaycan dilində adı daxil edilməyib.");
                AzNameText.Focus();
                AzNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (EnNameText.Text.Length == 0)
            //{
            //    EnNameText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Təyinatın ingilis dilində adı daxil edilməyib.");
            //    EnNameText.Focus();
            //    EnNameText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (RuNameText.Text.Length == 0)
            //{
            //    RuNameText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Təyinatın rus dilində adı daxil edilməyib.");
            //    RuNameText.Focus();
            //    RuNameText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            return b;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FAppointmentAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BANK_APPOINTMENTS", -1, "WHERE ID = " + AppointmentID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshAppointmentsDataGridView();
        }

        private void InsertName()
        {
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.BANK_APPOINTMENTS(NAME,NAME_EN,NAME_RU,NOTE,OPERATION_TYPE_ID, APPOINTMENT_TYPE_ID) VALUES('" + AzNameText.Text.Trim() + "','" + EnNameText.Text.Trim() + "','" + RuNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + operation_type_id + "," + appointment_type_id + ")",
                                                "Təyinat daxil edilmədi.");
        }

        private void AppointmentTypeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AppointmentTypeLookUp.EditValue == null)
                return;

            appointment_type_id = Convert.ToInt32(AppointmentTypeLookUp.EditValue);
        }

        private void UpdateName()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.BANK_APPOINTMENTS SET OPERATION_TYPE_ID = " + operation_type_id + ",NAME ='" + AzNameText.Text.Trim() + "',NAME_EN = '" + EnNameText.Text.Trim() + "',NAME_RU = '" + RuNameText.Text.Trim() + "',NOTE ='" + NoteText.Text.Trim() + "',APPOINTMENT_TYPE_ID = " + appointment_type_id + " WHERE ID = " + AppointmentID,
                                          "Təyinat dəyişdirilmədi.");
        }

        private void OperationTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ": operation_type_id = GlobalFunctions.FindComboBoxSelectedValue("OPERATION_TYPES", "TYPE_AZ", "ID", OperationTypeComboBox.Text);                    
                    break;
                case "en": operation_type_id = GlobalFunctions.FindComboBoxSelectedValue("OPERATION_TYPES", "TYPE_EN", "ID", OperationTypeComboBox.Text);
                    break;
                case "RU": operation_type_id = GlobalFunctions.FindComboBoxSelectedValue("OPERATION_TYPES", "TYPE_RU", "ID", OperationTypeComboBox.Text);
                    break;
            }
            
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlNameDetails())
            {
                if (TransactionName == "INSERT")
                    InsertName();
                else
                    UpdateName();
                this.Close();
            }
        }
    }
}