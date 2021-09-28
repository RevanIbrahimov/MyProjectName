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
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Dictionaries
{
    public partial class FCardIssuingAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCardIssuingAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, IssuingID;
        bool CurrentStatus = false, IssuingUsed = false;
        int IssuingUsedUserID = -1;
        string old_name = null;

        List<CardIssuing> lstIssuing = null;

        public delegate void DoEvent();
        public event DoEvent RefreshCardIssuingDataGridView;

        private void FCardIssuingAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CARD_ISSUING", GlobalVariables.V_UserID, "WHERE ID = " + IssuingID + " AND USED_USER_ID = -1");
                lstIssuing = CardIssuingDAL.SelectIssuingByID(int.Parse(IssuingID)).ToList<CardIssuing>();
                IssuingUsedUserID = lstIssuing.First().USED_USER_ID;
                IssuingUsed = (IssuingUsedUserID > 0);

                if (IssuingUsed)
                {
                    if (GlobalVariables.V_UserID != IssuingUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == IssuingUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş orqanın adı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş orqanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadSeriesDetails();
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadSeriesDetails()
        {
            var issuing = lstIssuing.First();
            NameText.Text = issuing.NAME;
            NoteText.Text = issuing.NOTE;
            old_name = issuing.NAME;
        }

        private bool ControlIssuingDetails()
        {
            bool b = false;

            if (TransactionName == "INSERT" || old_name != NameText.Text.Trim())
                lstIssuing = CardIssuingDAL.SelectIssuingByID(null).ToList<CardIssuing>();

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Orqanın adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((TransactionName == "INSERT" || old_name != NameText.Text.Trim()) && lstIssuing.Exists(i => i.NAME == NameText.Text.Trim()))
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bu orqan adı artıq bazaya daxil edilib. Zəhmət olmasa digər orqan adı daxil edin.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertIssuing()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CARD_ISSUING") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CARD_ISSUING(ID,
                                                                                NAME,
                                                                                NOTE,
                                                                                ORDER_ID) 
                                                VALUES(CARD_ISSUING_SEQUENCE.NEXTVAL,
                                                        '{NameText.Text.Trim()}',
                                                        '{NoteText.Text.Trim()}',
                                                        {order})",
                                                "Seriya daxil edilmədi.",
                                                this.Name + "/InsertIssuing");
        }

        private void UpdateIssuing()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CARD_ISSUING SET NAME ='{NameText.Text.Trim()}',
                                                                                NOTE ='{NoteText.Text.Trim()}' 
                                                        WHERE ID = {IssuingID}",
                                                "Seriya dəyişdirilmədi.",
                                                this.Name + "/UpdateIssuing");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCardIssuingAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CARD_ISSUING", -1, "WHERE ID = " + IssuingID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCardIssuingDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlIssuingDetails())
            {
                if (TransactionName == "INSERT")
                    InsertIssuing();
                else
                    UpdateIssuing();
                this.Close();
            }
        }
    }
}