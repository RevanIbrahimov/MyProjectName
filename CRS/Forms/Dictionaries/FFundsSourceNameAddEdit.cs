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
    public partial class FFundsSourceNameAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FFundsSourceNameAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, SourceID;
        bool CurrentStatus = false, SourceUsed = false;
        int SourceUsedUserID = -1, source_id;

        public delegate void DoEvent();
        public event DoEvent RefreshSourceNameDataGridView;


        private void FFundsSourceNameAddEdit_Load(object sender, EventArgs e)
        {            
            GlobalProcedures.FillLookUpEdit(SourceLookUp, "FUNDS_SOURCES", "ID", "NAME", "ID NOT IN (6,10) ORDER BY ORDER_ID");
            if (TransactionName == "EDIT")                
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_SOURCES_NAME", GlobalVariables.V_UserID, "WHERE ID = " + SourceID + " AND USED_USER_ID = -1");
                LoadSourceDetails();
                SourceUsed = (SourceUsedUserID >= 0);
                
                if (SourceUsed)
                {
                    if (GlobalVariables.V_UserID != SourceUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == SourceUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş mənbənin adı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş mənbənin adının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            NameText.Enabled = 
                NoteText.Enabled = 
                SourceLookUp.Enabled = 
                BOK.Visible = !status;
        }

        private void LoadSourceDetails()
        {
            string s = $@"SELECT SN.NAME SOURCE_NAME,SN.NOTE,S.NAME SOURCE,SN.USED_USER_ID FROM CRS_USER.FUNDS_SOURCES_NAME SN,CRS_USER.FUNDS_SOURCES S WHERE SN.SOURCE_ID = S.ID AND SN.ID = {SourceID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadSourceDetails", "Mənbənin adı açılmadı.");
            if(dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["SOURCE_NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                SourceLookUp.EditValue = SourceLookUp.Properties.GetKeyValueByDisplayText(dt.Rows[0]["SOURCE"].ToString());                
                SourceUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }
        
        private bool ControlSourceDetails()
        {
            bool b = false;

            if (SourceLookUp.EditValue == null)
            {
                SourceLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Mənbə daxil edilməyib.");
                SourceLookUp.Focus();
                SourceLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;            

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Mənbəyin adı daxil edilməyib.");
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
            int order = GlobalFunctions.GetMax($@"SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.FUNDS_SOURCES_NAME", this.Name + "/InsertSource") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.FUNDS_SOURCES_NAME(ID,NAME,NOTE,ORDER_ID,SOURCE_ID) VALUES(FUNDS_SOURCES_NAME_SEQUENCE.NEXTVAL,'{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order},{source_id})",
                                                "Mənbənin adı daxil edilmədi.",
                                                this.Name + "/InsertSource");
        }

        private void SourceLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SourceLookUp.EditValue == null)
                return;

            source_id = Convert.ToInt32(SourceLookUp.EditValue);
        }

        private void UpdateSource()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.FUNDS_SOURCES_NAME SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}',SOURCE_ID = {source_id} WHERE ID = {SourceID}",
                                                "Mənbənin adı daxil edilmədi.",
                                                this.Name + "/UpdateSource");
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

        private void FFundsSourceNameAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_SOURCES_NAME", -1, "WHERE ID = " + SourceID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshSourceNameDataGridView();
        }
    }
}