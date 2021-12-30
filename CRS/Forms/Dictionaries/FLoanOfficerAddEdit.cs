using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms.Dictionaries
{
    public partial class FLoanOfficerAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FLoanOfficerAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ID;
        bool CurrentStatus = false, Used = false;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FLoanOfficerAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.LOAN_OFFICER", GlobalVariables.V_UserID, "WHERE ID = " + ID + " AND USED_USER_ID = -1");
                LoadDetails();
                Used = (UsedUserID > 0);

                if (Used)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş mütəxəssiz hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş mütəxəssisin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.LOAN_OFFICER WHERE ID = {ID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDetails");
            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                UsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void Insert()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.LOAN_OFFICER(NAME,NOTE,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{GlobalVariables.V_UserID})",
                                                "Kredit mütəxəssisi daxil edilmədi.",
                                            this.Name + "/Insert");
        }

        private void Update()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.LOAN_OFFICER SET NAME ='{NameText.Text.Trim()}', NOTE ='{NoteText.Text.Trim()}', UPDATE_DATE = SYSDATE, UPDATE_USER = {GlobalVariables.V_UserID} WHERE ID = {ID}",
                                                "Kredit mütəxəssisi dəyişdirilmədi.",
                                                this.Name + "/Update");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void FLoanOfficerAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.LOAN_OFFICER", -1, "WHERE ID = " + ID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kredit mütəxəssisinin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }
    }
}