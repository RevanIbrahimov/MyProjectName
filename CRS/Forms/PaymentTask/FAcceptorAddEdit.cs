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
using CRS.Class.Tables;

namespace CRS.Forms.PaymentTask
{
    public partial class FAcceptorAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAcceptorAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, AcceptorID;

        bool CurrentStatus = false, AcceptorUsed = false;
        int UsedUserID = -1;
        string BankID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FAcceptorAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
                AcceptorID = GlobalFunctions.GetOracleSequenceValue("TASK_ACCEPTOR_SEQUENCE").ToString();
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TASK_ACCEPTOR", GlobalVariables.V_UserID, "WHERE ID = " + AcceptorID + " AND USED_USER_ID = -1");
                UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.TASK_ACCEPTOR WHERE ID = " + AcceptorID);
                AcceptorUsed = (UsedUserID >= 0);

                if (AcceptorUsed)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş alan tərəf hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş alan tərəf hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadAcceptorDetails();
                InsertBankTemp();                
            }
            LoadBanksGridView();
        }

        private void LoadAcceptorDetails()
        {
            string s = "SELECT ACCEPTOR_NAME,ACCEPTOR_VOEN,VAT_ACCOUNT FROM CRS_USER.TASK_ACCEPTOR WHERE ID = " + AcceptorID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAcceptorDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    AcceptorNameText.Text = dr["ACCEPTOR_NAME"].ToString();
                    AcceptorVoenText.Text = dr["ACCEPTOR_VOEN"].ToString();
                    VatAccountText.Text = dr["VAT_ACCOUNT"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ödənişi alan tərəf açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void ComponentEnabled(bool status)
        {
            AcceptorNameText.Enabled =
                AcceptorVoenText.Enabled =
                GroupControl.Enabled =
                BOK.Visible = !status;
        }

        private void LoadBanksGridView()
        {
            string sql = $@"SELECT 1 SS,
                                     ID,
                                     NAME,
                                     CODE,
                                     SWIFT,
                                     VOEN,
                                     CBAR_ACCOUNT,
                                     ACCEPTOR_ACCOUNT
                                FROM CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP
                               WHERE IS_CHANGE <> 2 AND ACCEPTOR_ID = {AcceptorID}
                            ORDER BY ORDER_ID";
            BankGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql);

            if (BankGridView.RowCount > 0)
                EditBarButton.Enabled = DeleteBarButton.Enabled = CopyBarButton.Enabled = true;
            else
                UpBarButton.Enabled =
                    EditBarButton.Enabled =
                    DeleteBarButton.Enabled =
                    DownBarButton.Enabled = 
                    CopyBarButton.Enabled = false;

        }

        private void InsertBankTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_ACCEPTOR_BANK_TEMP", "P_ACCEPTOR_ID", AcceptorID, "Alan tərəfin bankları temp cədvələ daxil edilmədi.");
        }

        private bool ControlAcceptorDetails()
        {
            bool b = false;

            if (AcceptorNameText.Text.Length == 0)
            {
                AcceptorNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Adı daxil edilməyib.");
                AcceptorNameText.Focus();
                AcceptorNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AcceptorVoenText.Text.Length == 0)
            {
                AcceptorVoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Vöen daxil edilməyib.");
                AcceptorVoenText.Focus();
                AcceptorVoenText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertAcceptor()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.TASK_ACCEPTOR(ID,
                                                                                ACCEPTOR_NAME,
                                                                                ACCEPTOR_VOEN,
                                                                                VAT_ACCOUNT) 
                                                VALUES({AcceptorID},
                                                        '{AcceptorNameText.Text.Trim()}',
                                                        '{AcceptorVoenText.Text.Trim()}',
                                                        '{VatAccountText.Text.Trim()}')",
                                                "Ödənişi alan tərəf daxil edilmədi.");
        }

        private void FAcceptorAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TASK_ACCEPTOR", -1, "WHERE ID = " + AcceptorID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP WHERE ACCEPTOR_ID = {AcceptorID} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Alan tətəfin bankları temp cədvəldən silinmədi.");
            this.RefreshDataGridView();
        }

        private void InsertBank()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_ACCEPTOR_BANK", "P_ACCEPTOR_ID", AcceptorID, "Alan tərəfin bankları əsas cədvələ daxil edilmədi.");
        }
        
        void RefreshBank()
        {
            LoadBanksGridView();
        }

        private void LoadFAcceptorBankAddEdit(string transaction, string acceptor_id, string bank_id)
        {
            FAcceptorBankAddEdit fa = new FAcceptorBankAddEdit();
            fa.TransactionName = transaction;
            fa.AcceptorID = acceptor_id;
            fa.BankID = bank_id;
            fa.RefreshBanksDataGridView += new FAcceptorBankAddEdit.DoEvent(RefreshBank);
            fa.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAcceptorBankAddEdit("INSERT", AcceptorID, null);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAcceptorBankAddEdit("EDIT", AcceptorID, BankID);
        }

        private void BankGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = BankGridView.GetFocusedDataRow();
            if (row != null)
            {
                BankID = row["ID"].ToString();
                UpBarButton.Enabled = !(BankGridView.FocusedRowHandle == 0);
                DownBarButton.Enabled = !(BankGridView.FocusedRowHandle == BankGridView.RowCount - 1);
            }
        }

        private void BankGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFAcceptorBankAddEdit("EDIT", AcceptorID, BankID);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadBanksGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(BankGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "xls");
        }

        private void BankGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(BankGridView, PopupMenu, e);
        }

        private void BankGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAYMENT_TASKS WHERE ACCEPTOR_ID = {AcceptorID} AND ACCEPTOR_BANK_ID = {BankID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş bankı silmək istəyirsiniz?", "Bankın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.TASK_ACCEPTOR_BANKS_TEMP SET IS_CHANGE = 2 WHERE ID = {BankID} AND ACCEPTOR_ID = {AcceptorID}", 
                                                    "Bank silinmədi.",
                                                    "FAcceptorAddEdit - DeleteBarButton_ItemClick");
            }
            else
                XtraMessageBox.Show("Seçilmiş bank bazada ödəniş tapşırıqlarında istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            LoadBanksGridView();
        }

        private void UpBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TASK_ACCEPTOR_BANKS", BankID, "up", out orderid);
            LoadBanksGridView();
            BankGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderID("TASK_ACCEPTOR_BANKS", BankID, "down", out orderid);
            LoadBanksGridView();
            BankGridView.FocusedRowHandle = orderid - 1;
        }

        private void CopyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAcceptorBankAddEdit("INSERT", AcceptorID, BankID);
        }

        private void VatAccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(VatAccountText, NumberLengthLabel);

        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlAcceptorDetails())
            {
                if (TransactionName == "INSERT")
                    InsertAcceptor();
                else
                    UpdateAcceptor();
                InsertBank();
                this.Close();
            }
        }

        private void UpdateAcceptor()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.TASK_ACCEPTOR SET ACCEPTOR_NAME ='{AcceptorNameText.Text.Trim()}',
                                                                                        ACCEPTOR_VOEN = '{AcceptorVoenText.Text.Trim()}',
                                                                               VAT_ACCOUNT = '{VatAccountText.Text.Trim()}'                                                                                          
                                                                WHERE ID = {AcceptorID}",
                                                "Ödənişi alan tərəf dəyişdirilmədi.");
        }
    }
}