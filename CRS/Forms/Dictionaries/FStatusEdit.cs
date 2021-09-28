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
    public partial class FStatusEdit : DevExpress.XtraEditors.XtraForm
    {
        public FStatusEdit()
        {
            InitializeComponent();
        }

        public string StatusID;
        bool CurrentStatus = false, StatusUsed = false;
        int StatusUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshStatusDataGridView;

        private void FStatusEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.STATUS", GlobalVariables.V_UserID, "WHERE ID = " + StatusID + " AND USED_USER_ID = -1");
            LoadStatusDetails();
            CurrentStatus = (StatusUsedUserID >= 0);
            
            if (CurrentStatus)
            {
                if (GlobalVariables.V_UserID != StatusUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == StatusUsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş status hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş statusun hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;
            ComponentEnabled(CurrentStatus);            
        }

        private void ComponentEnabled(bool status)
        {
            AzStatusText.Enabled = 
                EnStatusText.Enabled = 
                RuStatusText.Enabled = 
                BOK.Visible = !status;            
        }

        private void LoadStatusDetails()
        {
            string s = $@"SELECT STATUS_NAME,OWNER_FORM,STATUS_DESCRIPTION,STATUS_NAME_EN,STATUS_NAME_RU,USED_USER_ID FROM CRS_USER.STATUS WHERE ID = {StatusID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadStatusDetails", "Statusun parametrləri açılmadı.");
            if(dt.Rows.Count > 0)
            {
                AzStatusText.Text = dt.Rows[0]["STATUS_NAME"].ToString();
                TypeText.Text = dt.Rows[0]["OWNER_FORM"].ToString();
                DescriptionText.Text = dt.Rows[0]["STATUS_DESCRIPTION"].ToString();
                EnStatusText.Text = dt.Rows[0]["STATUS_NAME_EN"].ToString();
                RuStatusText.Text = dt.Rows[0]["STATUS_NAME_RU"].ToString();
                StatusUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private void UpdateStatus()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.STATUS SET STATUS_NAME = '{AzStatusText.Text.Trim()}',STATUS_NAME_EN = '{EnStatusText.Text.Trim()}',STATUS_NAME_RU = '{RuStatusText.Text.Trim()}' WHERE ID = {StatusID}",
                                                "Statusun adı dəyişdirilmədi.",
                                                this.Name + "/UpdateStatus");
        }

        private bool ControlStatusDetails()
        {
            bool b = false;

            if (AzStatusText.Text.Length == 0)
            {
                AzStatusText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Statusun azərbaycan dilində adı daxil edilməyib.");
                AzStatusText.Focus();
                AzStatusText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (EnStatusText.Text.Length == 0)
            //{
            //    EnStatusText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Statusun ingilis dilində adı daxil edilməyib.");
            //    EnStatusText.Focus();
            //    EnStatusText.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (EnStatusText.Text.Length == 0)
            //{
            //    RuStatusText.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("Statusun rus dilində adı daxil edilməyib.");
            //    RuStatusText.Focus();
            //    RuStatusText.BackColor = GlobalFunctions.ElementColor();
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

        private void FStatusEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.STATUS", -1, "WHERE ID = " + StatusID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshStatusDataGridView();
        }
        
        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlStatusDetails())
            {
                UpdateStatus();
                this.Close();                
            }
        }
    }
}