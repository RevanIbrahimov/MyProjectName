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
    public partial class FCarTypeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCarTypeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, TypeID;
        bool CurrentStatus = false, TypeUsed = false;
        int TypeUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshCarTypeDataGridView;

        private void FCarTypeAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")               
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_TYPES", GlobalVariables.V_UserID, "WHERE ID = " + TypeID + " AND USED_USER_ID = -1");
                LoadCarTypeDetails();
                TypeUsed = (TypeUsedUserID >= 0);
                
                if (TypeUsed)
                {
                    if (GlobalVariables.V_UserID != TypeUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == TypeUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş tip hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş tipin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadCarTypeDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.CAR_TYPES WHERE ID = {TypeID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCarTypeDetails");
            if(dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                TypeUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private bool ControlCarTypeDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tip daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCarType()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CAR_TYPES") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CAR_TYPES(ID,NAME,NOTE,ORDER_ID) VALUES(CAR_TYPE_SEQUENCE.NEXTVAL,'{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order})",
                                                "Tip daxil edilmədi.",
                                                this.Name + "/InsertCarType");
        }

        private void UpdateCarType()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CAR_TYPES SET NAME ='{NameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}' WHERE ID = {TypeID}",
                                                "Tip dəyişdirilmədi.",
                                                this.Name + "/UpdateCarType");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCarTypeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CAR_TYPES", -1, "WHERE ID = " + TypeID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCarTypeDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCarTypeDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCarType();
                else
                    UpdateCarType();
                this.Close();
            }
        }
    }
}