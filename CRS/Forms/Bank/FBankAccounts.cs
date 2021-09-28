using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraEditors.Repository;
using System.Collections;
using CRS.Class;

namespace CRS.Forms.Bank
{
    public partial class FBankAccounts : DevExpress.XtraEditors.XtraForm
    {
        public FBankAccounts()
        {
            InitializeComponent();
        }
        public string TransactionName = null;
        string AccountID;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshBankAccountGridView;

        private void FBankAccounts_Load(object sender, EventArgs e)
        {
            //permission
            if (Class.GlobalVariables.V_UserID > 0)
            {
                NewBarButton.Enabled = Class.GlobalVariables.AddAccount;
                EditBarButton.Enabled = Class.GlobalVariables.EditAccount;
                DeleteBarButton.Enabled = Class.GlobalVariables.DeleteAccount;
            }

            LoadAccountsDataGridView();
        }

        private void LoadAccountsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 BA.ID,
                                 B.LONG_NAME||', '||BB.NAME BANK_FULLNAME,
                                 BA.ADATE,
                                 BA.ACCOUNT,
                                 C.CODE CURRENCY_CODE,
                                 BA.NOTE,         
                                 BA.STATUS_ID,
                                 BA.IS_PAYMENT,
                                 BA.USED_USER_ID
                            FROM CRS_USER.BANK_ACCOUNTS BA,
                                 CRS_USER.BANKS B,
                                 CRS_USER.BANK_BRANCHES BB,
                                 CRS_USER.CURRENCY C
                           WHERE     BA.BANK_ID = B.ID
                                 AND BA.BANK_BRANCH_ID = BB.ID
                                 AND B.ID = BB.BANK_ID         
                                 AND BA.CURRENCY_ID = C.ID
                        ORDER BY BA.ADATE,BA.ID";
            AccountsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAccountsDataGridView");

            if (AccountsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    DeleteBarButton.Enabled = GlobalVariables.DeleteAccount;
                    EditBarButton.Enabled = GlobalVariables.EditAccount;
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = true;
            }
            else
                DeleteBarButton.Enabled = EditBarButton.Enabled = false;

            try
            {
                AccountsGridView.BeginUpdate();
                for (int i = 0; i < AccountsGridView.RowCount; i++)
                {
                    DataRow row = AccountsGridView.GetDataRow(AccountsGridView.GetVisibleRowHandle(i));
                    if (Convert.ToInt32(row["IS_PAYMENT"].ToString()) == 1)
                        AccountsGridView.SelectRow(i);
                }
            }
            finally
            {
                AccountsGridView.EndUpdate();
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AccountsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void AccountsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(AccountsGridView, PopupMenu, e);
        }

        void RefreshAccounts()
        {
            LoadAccountsDataGridView();
        }

        private void LoadFBankAccountAddEdit(string transaction, string account_id)
        {
            FBankAccountAddEdit fcae = new FBankAccountAddEdit();
            fcae.TransactionName = transaction;
            fcae.AccountID = account_id;
            fcae.RefreshAccountsDataGridView += new FBankAccountAddEdit.DoEvent(RefreshAccounts);
            fcae.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFBankAccountAddEdit("INSERT", null);
        }

        private void AccountsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = AccountsGridView.GetFocusedDataRow();
            if (row != null)
            {
                AccountID = row["ID"].ToString();
                UsedUserID = Convert.ToInt32(row["USED_USER_ID"]);
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFBankAccountAddEdit("EDIT", AccountID);
        }

        private void AccountsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(AccountsGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(12, AccountsGridView, e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadAccountsDataGridView();
        }

        private void AccountsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFBankAccountAddEdit("EDIT", AccountID);
        }

        private void UpdateIsPayment()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.BANK_ACCOUNTS SET IS_PAYMENT = 0 WHERE IS_PAYMENT = 1 AND STATUS_ID = 11", "Hesabların sənədlərdə istifadə olunub olunmaması dəyişdirilmədi.");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < AccountsGridView.SelectedRowsCount; i++)
            {
                rows.Add(AccountsGridView.GetDataRow(AccountsGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.BANK_ACCOUNTS SET IS_PAYMENT = 1 WHERE ID = {row["ID"]}", "Hesabların sənədlərdə istifadə olunub olunmaması bazada saxlanılmadı.");
            }
        }

        private void FBankAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateIsPayment();
            if (TransactionName == "E")
                this.RefreshBankAccountGridView();
        }

        private void DeleteAccount()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT ACCOUNT_ID FROM CRS_USER.CASH_BANK_ACCOUNT UNION ALL SELECT ACCOUNT_ID FROM CRS_USER.BANK_OPERATIONS UNION ALL SELECT ACCOUNT_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP) WHERE ACCOUNT_ID = " + AccountID);

            if (UsedUserID >= 0)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş bank hesabı hal-hazırda " + used_user_name + " tərəfindən istifadə edildiyi üçün onu bazadan silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş bank hesabını silmək istəyirsiniz?", "Hesabın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.BANK_ACCOUNTS WHERE ID = {AccountID}", "Bank hesabı silinmədi");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş bank hesabı bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteAccount();
            LoadAccountsDataGridView();
        }

        private void AccountsGridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void AccountsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (AccountsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    DeleteBarButton.Enabled = GlobalVariables.DeleteAccount;
                    EditBarButton.Enabled = GlobalVariables.EditAccount;
                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = true;
            }
            else
                DeleteBarButton.Enabled = EditBarButton.Enabled = false;
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(AccountsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(AccountsGridControl, "xls");
        }

    }
}