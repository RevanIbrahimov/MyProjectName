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
    public partial class FCreditNameAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCreditNameAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, NameID;
        bool CurrentStatus = false, NameUsed = false;
        int NameUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshCreditNameDataGridView;

        private void FCreditNameAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_NAMES", GlobalVariables.V_UserID, "WHERE ID = " + NameID + " AND USED_USER_ID = -1");
                LoadNameDetails();
                NameUsed = (NameUsedUserID >= 0);

                if (NameUsed)
                {
                    if (GlobalVariables.V_UserID != NameUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == NameUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş lizinqin adı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş lizinqin adının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            AzNameText.Enabled =
                EnNameText.Enabled =
                RuNameText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadNameDetails()
        {
            string s = $@"SELECT NAME,NAME_EN,NAME_RU,NOTE,USED_USER_ID FROM CRS_USER.CREDIT_NAMES WHERE ID = {NameID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadNameDetails", "Kreditin adı açılmadı.");
            if (dt.Rows.Count > 0)
            {
                AzNameText.Text = dt.Rows[0]["NAME"].ToString();
                EnNameText.Text = dt.Rows[0]["NAME_EN"].ToString();
                RuNameText.Text = dt.Rows[0]["NAME_RU"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                NameUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private bool ControlNameDetails()
        {
            bool b = false;

            if (AzNameText.Text.Length == 0)
            {
                AzNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kreditin azərbaycan dilində adı daxil edilməyib.");
                AzNameText.Focus();
                AzNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (EnNameText.Text.Length == 0)
            //{
            //    EnNameText.BackColor = Color.Red;
            //   GlobalProcedures.ShowErrorMessage("Kreditin ingilis dilində adı daxil edilməyib.");                
            //    EnNameText.Focus();
            //    EnNameText.BackColor =GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            //if (RuNameText.Text.Length == 0)
            //{
            //    RuNameText.BackColor = Color.Red;
            //   GlobalProcedures.ShowErrorMessage("Kreditin rus dilində adı daxil edilməyib.");               
            //    RuNameText.Focus();
            //    RuNameText.BackColor =GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            return b;
        }

        private void InsertName()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CREDIT_NAMES(ID,NAME,NAME_EN,NAME_RU,NOTE) VALUES(CREDIT_NAME_SEQUENCE.NEXTVAL,'" + AzNameText.Text.Trim() + "','" + EnNameText.Text.Trim() + "','" + RuNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "')",
                                                 "Kreditin adı daxil edilmədi.",
                                                 this.Name + "/InsertName");
        }

        private void UpdateName()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CREDIT_NAMES SET NAME ='{AzNameText.Text.Trim()}',NAME_EN = '{EnNameText.Text.Trim()}',NAME_RU = '{RuNameText.Text.Trim()}',NOTE ='{NoteText.Text.Trim()}' WHERE ID = {NameID}",
                                                 "Kreditin adı dəyişdirilmədi.",
                                                 this.Name + "/UpdateName");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlNameDetails())
            {
                if (TransactionName == "INSERT")
                    InsertName();
                else
                    UpdateName();
                this.Close();
            }
        }

        private void FCreditNameAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_NAMES", -1, "WHERE ID = " + NameID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCreditNameDataGridView();
        }
    }
}