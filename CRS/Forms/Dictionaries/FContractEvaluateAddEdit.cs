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
    public partial class FContractEvaluateAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FContractEvaluateAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractEvaluateID;
        bool CurrentStatus = false, EvaluatetUsed = false;
        int EvaluateUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshContractEvaluateDataGridView;

        private void FContractEvaluateAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACT_EVALUATE", GlobalVariables.V_UserID, "WHERE ID = " + ContractEvaluateID + " AND USED_USER_ID = -1");
                LoadContractEvaluateDetails();
                EvaluatetUsed = (EvaluateUsedUserID > 0);

                if (EvaluatetUsed)
                {
                    if (GlobalVariables.V_UserID != EvaluateUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == EvaluateUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş qiymətləndirmə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş qiymətləndirmənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void LoadContractEvaluateDetails()
        {
            string s = $@"SELECT NAME,NOTE,USED_USER_ID FROM CRS_USER.CONTRACT_EVALUATE WHERE ID = {ContractEvaluateID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractEvaluateDetails");
            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["NAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                EvaluateUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private bool ControlContractEvaluate()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qiymətləndirmə dərəcəsinin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FContractEvaluateAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACT_EVALUATE", -1, "WHERE ID = " + ContractEvaluateID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshContractEvaluateDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlContractEvaluate())
            {
                if (TransactionName == "INSERT")
                    InsertContractEvaluate();
                else
                    UpdateContractEvaluate();
                this.Close();
            }
        }

        private void InsertContractEvaluate()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CONTRACT_EVALUATE") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CONTRACT_EVALUATE(NAME,NOTE,ORDER_ID,INSERT_USER) VALUES('{NameText.Text.Trim()}','{NoteText.Text.Trim()}',{order},{GlobalVariables.V_UserID})",
                                                "Qiymətləndirmə daxil edilmədi.",
                                            this.Name + "/InsertContractEvaluate");
        }

        private void UpdateContractEvaluate()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_EVALUATE SET NAME ='{NameText.Text.Trim()}', NOTE ='{NoteText.Text.Trim()}', UPDATE_DATE = SYSDATE, UPDATE_USER = {GlobalVariables.V_UserID} WHERE ID = {ContractEvaluateID}",
                                                "Qiymətləndirmə dəyişdirilmədi.",
                                                this.Name + "/UpdateContractEvaluate");
        }
    }
}