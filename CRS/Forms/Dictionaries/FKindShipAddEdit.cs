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
    public partial class FKindShipAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FKindShipAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, KindShipID;
        bool CurrentStatus = false, KindShipUsed = false;
        int KindShipUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshKindShipDataGridView;

        private void LoadKindShipDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.KINDSHIP_RATE WHERE ID = {KindShipID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadKindShipDetails");
            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                KindShipUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private bool ControlKindShip()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qohumluq dərəcəsinin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void FKindShipAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.KINDSHIP_RATE", -1, "WHERE ID = " + KindShipID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshKindShipDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }        

        private void FKindShipAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.KINDSHIP_RATE", GlobalVariables.V_UserID, "WHERE ID = " + KindShipID + " AND USED_USER_ID = -1");
                LoadKindShipDetails();
                KindShipUsed = (KindShipUsedUserID > 0);

                if (KindShipUsed)
                {
                    if (GlobalVariables.V_UserID != KindShipUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == KindShipUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş qohumluq dərəcəsi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş qohumluq dərəcəsinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlKindShip())
            {
                if (TransactionName == "INSERT")
                    InsertKindShip();
                else
                    UpdateKindShip();
                this.Close();
            }
        }

        private void InsertKindShip()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.KINDSHIP_RATE") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.KINDSHIP_RATE(NAME,NOTE,ORDER_ID,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order},{GlobalVariables.V_UserID})",
                                                "Qiymətləndirmə daxil edilmədi.",
                                            this.Name + "/InsertContractEvaluate");
        }

        private void UpdateKindShip()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.KINDSHIP_RATE SET NAME ='{NameText.Text.Trim()}', NOTE ='{NoteText.Text.Trim()}', UPDATE_DATE = SYSDATE, UPDATE_USER = {GlobalVariables.V_UserID} WHERE ID = {KindShipID}",
                                                "Qiymətləndirmə dəyişdirilmədi.",
                                                this.Name + "/UpdateContractEvaluate");
        }
    }
}