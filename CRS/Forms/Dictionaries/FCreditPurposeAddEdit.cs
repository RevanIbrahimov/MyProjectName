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
    public partial class FCreditPurposeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCreditPurposeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CreditPurposeID;
        bool CurrentStatus = false, NameUsed = false;
        int NameUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FCreditPurposeAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_PURPOSE", GlobalVariables.V_UserID, "WHERE ID = " + CreditPurposeID + " AND USED_USER_ID = -1");
                LoadDetails();
                NameUsed = (NameUsedUserID >= 0);

                if (NameUsed)
                {
                    if (GlobalVariables.V_UserID != NameUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == NameUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş əyyar hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş əyyarın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            NameText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadDetails()
        {
            string s = $@"SELECT NAME,CODE,USED_USER_ID FROM CRS_USER.CREDIT_PURPOSE WHERE ID = {CreditPurposeID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDetails", "Əyyarın adı açılmadı.");
            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["CODE"].ToString();
                NameUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private bool ControlNameDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əyyarın adı daxil edilməyib.");
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
            if (ControlNameDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void FCreditPurposeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_PURPOSE", -1, "WHERE ID = " + CreditPurposeID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void Insert()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CREDIT_PURPOSE(NAME,CODE,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{GlobalVariables.V_UserID})",
                                                 "Əyyar daxil edilmədi.",
                                                 this.Name + "/Insert");
        }

        private void Update()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CREDIT_PURPOSE SET NAME ='{NameText.Text.Trim()}',CODE ='{NoteText.Text.Trim()}', UPDATE_USER = {GlobalVariables.V_UserID}, UPDATE_DATE = SYSDATE WHERE ID = {CreditPurposeID}",
                                                 "Əyyar dəyişdirilmədi.",
                                                 this.Name + "/Update");
        }
    }
}