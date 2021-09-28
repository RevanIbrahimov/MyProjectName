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

namespace CRS.Forms.Bookkeeping
{
    public partial class FAccountingPlan : DevExpress.XtraEditors.XtraForm
    {
        public FAccountingPlan()
        {
            InitializeComponent();
        }       

        string AccountID, SubAccount;
        int AccountNumber, topindex, old_row_num;

        public delegate void DoEvent();
        public event DoEvent RefreshAccountList;

        private void FAccountingPlan_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)            
                NewBarButton.Enabled = GlobalVariables.AddAccountPlan;               
            
            LoadAccountDataGridView();
        }

        private void LoadAccountDataGridView()
        {
            string s = $@"SELECT 1 SS,AP.ID,AP.ACCOUNT_NUMBER,AP.ACCOUNT_NAME,AP.SUB_ACCOUNT,AP.SUB_ACCOUNT_NAME,DECODE(AP.ACCOUNT_CATEGORY,0,'Digər','Bank') Category,AP.NOTE,AT.NAME,AP.USED_USER_ID,AP.IS_IMPORTANT FROM CRS_USER.ACCOUNTING_PLAN AP,CRS_USER.ACCOUNT_TYPE AT WHERE AP.ACCOUNT_TYPE_ID = AT.ID ORDER BY ACCOUNT_NUMBER";
            try
            {                
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAccountDataGridView");
                
                AccountsGridControl.DataSource = dt;
                AccountsGridView.PopulateColumns();

                AccountsGridView.Columns[0].Caption = "S/s";
                AccountsGridView.Columns[0].Visible = false;
                AccountsGridView.Columns[1].Visible = false;
                AccountsGridView.Columns[2].Caption = "Nömrəsi";
                AccountsGridView.Columns[3].Caption = "Adı";
                AccountsGridView.Columns[4].Caption = "Sub hesabı";
                AccountsGridView.Columns[5].Caption = "Sub hesabın adı";
                AccountsGridView.Columns[6].Caption = "Kateqoriyası";
                AccountsGridView.Columns[7].Caption = "Qeyd";
                AccountsGridView.Columns[8].Caption = "Tipi";
                AccountsGridView.Columns[9].Visible = false;
                AccountsGridView.Columns[10].Visible = false;

                AccountsGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                AccountsGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                AccountsGridView.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                AccountsGridView.Columns[2].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                AccountsGridView.Columns[4].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                AccountsGridView.Columns[4].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                AccountsGridView.Columns[6].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                AccountsGridView.Columns[6].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                AccountsGridView.Columns[8].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                AccountsGridView.Columns[8].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                
                if (AccountsGridView.RowCount > 0)
                {
                    if (GlobalVariables.V_UserID > 0)
                    {
                        EditBarButton.Enabled = GlobalVariables.EditAccountPlan;
                        DeleteBarButton.Enabled = GlobalVariables.DeleteAccountPlan;
                    }
                    else                    
                        DeleteBarButton.Enabled = EditBarButton.Enabled = true;
                    
                    AccountsGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else                
                    DeleteBarButton.Enabled = EditBarButton.Enabled = false;
                
                AccountsGridView.Columns[8].GroupIndex = 0;
                AccountsGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Hesablar planı cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
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

        private void AccountsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = AccountsGridView.GetFocusedDataRow();
            if (row != null)
            {
                AccountID = row["ID"].ToString();
                SubAccount = row["SUB_ACCOUNT"].ToString();
                AccountNumber = Convert.ToInt32(row["ACCOUNT_NUMBER"]);                
            }
        }

        void RefreshAccount()
        {
            LoadAccountDataGridView();
        }

        private void LoadFAccountingAddEdit(string transaction, string accountid)
        {
            topindex = AccountsGridView.TopRowIndex;
            old_row_num = AccountsGridView.FocusedRowHandle;
            FAccountingAddEdit fa = new FAccountingAddEdit();
            fa.TransactionName = transaction;
            fa.AccountID = accountid;
            fa.RefreshAccountDataGridView += new FAccountingAddEdit.DoEvent(RefreshAccount);
            fa.ShowDialog();
            AccountsGridView.TopRowIndex = topindex;
            AccountsGridView.FocusedRowHandle = old_row_num;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAccountingAddEdit("INSERT", null);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAccountingAddEdit("EDIT", AccountID);
        }

        private void AccountsGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadFAccountingAddEdit("EDIT", AccountID);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadAccountDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(AccountsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(AccountsGridControl, "xls");
        }

        private void AccountsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(AccountsGridView, PopupMenu, e);
        }

        private void DeleteAccount()
        {
            int isImportant = Convert.ToInt32(AccountsGridView.GetRowCellValue(AccountsGridView.FocusedRowHandle, "IS_IMPORTANT"));

            if(isImportant == 1)
            {
                GlobalProcedures.ShowWarningMessage("Seçdiyiniz hesab avtomatik hesablamalarda istifadə olunduğu üçün bu hesabı silmək olmaz.");
                return;
            }

            //if (AccountNumber != 543 && AccountNumber != 216 && AccountNumber != 174 && AccountNumber != 612 && AccountNumber != 712 && AccountNumber != 243)
            //{
                int a = GlobalFunctions.GetCount("SELECT COUNT (*) " +
                                                        "FROM (SELECT DEBIT_ACCOUNT ACCOUNT FROM CRS_USER.OPERATION_JOURNAL " +
                                                                "UNION " +
                                                              "SELECT CREDIT_ACCOUNT ACCOUNT FROM CRS_USER.OPERATION_JOURNAL " +
                                                                "UNION " +
                                                              "SELECT DEBIT_ACCOUNT ACCOUNT FROM CRS_USER_TEMP.OPERATION_JOURNAL_TEMP " +
                                                                "UNION " +
                                                              "SELECT CREDIT_ACCOUNT ACCOUNT FROM CRS_USER_TEMP.OPERATION_JOURNAL_TEMP) " +
                                                        "WHERE ACCOUNT = '" + SubAccount + "'");
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş hesabı silmək istəyirsiniz?", "Hesabın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.ACCOUNTING_PLAN WHERE ID = {AccountID}", "Hesab silinmədi.");
                }
                else
                    XtraMessageBox.Show("Seçilmiş hesab bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //    XtraMessageBox.Show("Seçdiyiniz hesab avtomatik hesablamalarda istifadə olunduğu üçün bu hesabı silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.ACCOUNTING_PLAN WHERE ID = {AccountID}");
            if (UsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş hesab hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş hesabın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteAccount();
            }
            else
                DeleteAccount();
            LoadAccountDataGridView();
        }

        private void AccountsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void FAccountingPlan_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshAccountList();
        }
    }
}