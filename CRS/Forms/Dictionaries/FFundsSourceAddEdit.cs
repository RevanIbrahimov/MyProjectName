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
    public partial class FFundsSourceAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FFundsSourceAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, SourceID;
        bool CurrentStatus = false, SourceUsed = false;
        int SourceUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshSourceDataGridView;

        private void FFundsSourceAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
                SourceID = GlobalFunctions.GetOracleSequenceValue("FUNDS_SOURCES_SEQUENCE").ToString();
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_SOURCES", GlobalVariables.V_UserID, "WHERE ID = " + SourceID + " AND USED_USER_ID = -1");
                LoadSourceDetails();
                SourceUsed = (SourceUsedUserID >= 0);
                
                if (SourceUsed)
                {
                    if (GlobalVariables.V_UserID != SourceUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == SourceUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş mənbə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş mənbənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private void LoadSourceDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.FUNDS_SOURCES WHERE ID = {SourceID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadSourceDetails", "Mənbə açılmadı.");
            if(dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                SourceUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private bool ControlSourceDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Mənbə daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertSource()
        {
            int order = GlobalFunctions.GetMax($@"SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.FUNDS_SOURCES", this.Name + "/InsertSource") + 1;
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.FUNDS_SOURCES(ID,NAME,NOTE,ORDER_ID) VALUES(" + SourceID + ",'" + NameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + order + ")",
                                                "Mənbə daxil edilmədi.",
                                              this.Name + "/InsertSource");
        }

        private void UpdateSource()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.FUNDS_SOURCES SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}' WHERE ID = {SourceID}",
                                                "Mənbə dəyişdirilmədi.",
                                                this.Name + "/UpdateSource");
        }

        private void FFundsSourceAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_SOURCES", -1, "WHERE ID = " + SourceID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshSourceDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlSourceDetails())
            {
                if (TransactionName == "INSERT")
                    InsertSource();
                else
                    UpdateSource();
                this.Close();
            }
        }
    }
}