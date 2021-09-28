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

namespace CRS.Forms.PaymentTask
{
    public partial class FTaskTypeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FTaskTypeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, TypeID;
        bool CurrentStatus = false, TypeUsed = false;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FTaskTypeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TASK_TYPE", -1, "WHERE ID = " + TypeID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void FTaskTypeAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TASK_TYPE", GlobalVariables.V_UserID, "WHERE ID = " + TypeID + " AND USED_USER_ID = -1");
                UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.TASK_TYPE WHERE ID = " + TypeID);
                TypeUsed = (UsedUserID >= 0);

                if (TypeUsed)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş tapşırıq növü hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş tapşırıq növünün hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadTypeDetails();
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            CodeText.Enabled = !status;
            BOK.Visible = !status;
        }

        private bool ControlTypeDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tapşırığın növünün adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tapşırığın kodu daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.TASK_TYPE WHERE CODE = '{CodeText.Text.Trim()}'") > 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bu kod artıq bazaya daxil edilib. Zəhmət olmasa digər kod daxil edin.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertType()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.TASK_TYPE") + 1,
                is_internal = IsInternalCheck.Checked ? 1 : 0;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.TASK_TYPE(TYPE_NAME,CODE,ORDER_ID,IS_INTERNAL) VALUES('{NameText.Text.Trim()}','{CodeText.Text.Trim()}',{order},{is_internal})",
                                                "Tapşırığın növü daxil edilmədi.");
        }

        private void UpdateType()
        {
            int is_internal = IsInternalCheck.Checked ? 1 : 0;
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.TASK_TYPE SET TYPE_NAME ='{NameText.Text.Trim()}',CODE = '{CodeText.Text.Trim()}', IS_INTERNAL = {is_internal} WHERE ID = " + TypeID,
                                                "Tapşırığın növü dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlTypeDetails())
            {
                if (TransactionName == "INSERT")
                    InsertType();
                else
                    UpdateType();
                this.Close();
            }
        }

        private void LoadTypeDetails()
        {
            string s = "SELECT TYPE_NAME,CODE,IS_INTERNAL FROM CRS_USER.TASK_TYPE WHERE ID = " + TypeID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadTypeDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr["TYPE_NAME"].ToString();
                    CodeText.Text = dr["CODE"].ToString();
                    IsInternalCheck.Checked = (Convert.ToInt32(dr["IS_INTERNAL"].ToString()) == 1);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Tapşırığın növü açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}