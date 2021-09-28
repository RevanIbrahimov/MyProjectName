using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using CRS.Class;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using DevExpress.XtraEditors;

namespace CRS.Forms.Bookkeeping
{
    public partial class FJournalLog : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FJournalLog()
        {
            InitializeComponent();
        }
        int old_row_num, topindex;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FJournalLog_Load(object sender, EventArgs e)
        {
            LoadOperations();
        }

        private void LoadOperations()
        {
            string s = $@"SELECT OJ.OPERATION_DATE ODATE,
                                 DECODE (OJ.CREATED_USER_ID,
                                         -1, 'Avtomatik',
                                         (SELECT NAME
                                            FROM CRS_USER.V_USERS
                                           WHERE ID = OJ.CREATED_USER_ID))
                                    USERNAME,
                                 OJ.APPOINTMENT,
                                 OJ.DEBIT_ACCOUNT,
                                 OJ.CREDIT_ACCOUNT,
                                 OJ.CURRENCY_RATE,
                                 OJ.AMOUNT_CUR,
                                 OJ.AMOUNT_AZN,
                                 OJ.ID,
                                 OJ.USER_ID,
                                 OJ.YR_MNTH_DY,
                                 OJ.IS_MANUAL,
                                 OJ.ACCOUNT_OPERATION_TYPE_ID,
                                 OJ.CONTRACT_ID,
                                 DECODE (OJ.LOG_TYPE,
                                         1, 'Daxil edilib',
                                         2, 'Dəyişdirilib',
                                         'Silinib')
                                    LOG_TYPE_NAME,
                                 OJ.LOG_TYPE,
                                 CU.NAME LOG_USER_NAME,
                                 OJ.LOG_DATE,
                                 OJ.USED_USER_ID,
                                 OJ.LOG_ID
                            FROM CRS_USER.OPERATION_JOURNAL_LOG OJ, CRS_USER.CRS_USERS CU
                           WHERE OJ.USER_ID = CU.ID
                        ORDER BY OJ.LOG_DATE, OJ.ID";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperations", "Jurnalın tarixçəsi yüklənmədi.");

            OperationsGridControl.DataSource = dt;
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadOperations();
        }

        private void OperationsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            int usedUserID = int.Parse(currentView.GetRowCellDisplayText(e.RowHandle, currentView.Columns["USED_USER_ID"]));
            if (usedUserID >= 0 && usedUserID != GlobalVariables.V_UserID)
                GlobalProcedures.GridRowCellStyleForBlock(currentView, e);
            else if (int.Parse(OperationsGridView.GetRowCellDisplayText(e.RowHandle, OperationsGridView.Columns["LOG_TYPE"])) == 2)
                e.Appearance.BackColor = e.Appearance.BackColor2 = GlobalFunctions.CreateColor("#FEF5E7");
            else if (int.Parse(OperationsGridView.GetRowCellDisplayText(e.RowHandle, OperationsGridView.Columns["LOG_TYPE"])) == 3)
                e.Appearance.BackColor = e.Appearance.BackColor2 = GlobalFunctions.CreateColor("#D6DBDF");
        }

        private void BackBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < OperationsGridView.SelectedRowsCount; i++)
            {
                int rowHandle = OperationsGridView.GetSelectedRows()[i];
                if (!OperationsGridView.IsGroupRow(rowHandle))
                    rows.Add(OperationsGridView.GetDataRow(rowHandle));
            }

            if (rows.Count == 0)
            {
                GlobalProcedures.ShowWarningMessage("Manual əməliyyatlardan geri qaytarmaq istədiyinizi seçin.");
                return;
            }

            bool allManual = true;
            string deleteListID = null, updateListID = null, listID = null;
            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;

                if (Convert.ToDecimal(row["IS_MANUAL"]) == 0)
                {
                    allManual = false;
                    break;
                }

                if (Convert.ToDecimal(row["LOG_TYPE"]) == 3)
                    deleteListID += row["LOG_ID"] + ",";
                else if (Convert.ToDecimal(row["LOG_TYPE"]) == 2)               
                    updateListID += row["LOG_ID"] + ",";

                listID += row["LOG_ID"] + ",";
            }

            if (!allManual)
            {
                GlobalProcedures.ShowWarningMessage("Avtomatik yaradılmış əməliyyatları seçmək olmaz.");
                return;
            }

            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş əməliyyatları geri qaytarmaq istəyirsiniz?", "Əməliyyatların geri qaytarılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (deleteListID != null)
                    deleteListID = deleteListID.TrimEnd(',');
                if (updateListID != null)
                    updateListID = updateListID.TrimEnd(',');

                listID = listID.TrimEnd(',');

                GlobalProcedures.ExecuteProcedureWithThreeParametrs("CRS_USER.PROC_BACK_TO_JOURNAL", "P_DELETE_LIST_ID", deleteListID, "P_UPDATE_LIST_ID", updateListID, "P_LIST_ID", listID, "Əməliyyatlar geri qaytarılmadı.");
            }
            LoadOperations();
        }

        private void OperationsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(OperationsGridView, PopupMenu, e);
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(OperationsGridControl);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "xls");
        }

        private void PdfBarButtonI_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "mht");
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < OperationsGridView.SelectedRowsCount; i++)
            {
                int rowHandle = OperationsGridView.GetSelectedRows()[i];
                if (!OperationsGridView.IsGroupRow(rowHandle))
                    rows.Add(OperationsGridView.GetDataRow(rowHandle));
            }

            if (rows.Count == 0)
            {
                GlobalProcedures.ShowWarningMessage("Silmək istədiyiniz əməliyyatları seçin.");
                return;
            }

            string listID = null;
            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;

                listID += row["LOG_ID"] + ",";
            }

            listID = listID.TrimEnd(',');

            old_row_num = OperationsGridView.FocusedRowHandle;
            topindex = OperationsGridView.TopRowIndex;
            int UsedUserID = GlobalFunctions.GetMax($@"SELECT MAX(USED_USER_ID) FROM CRS_USER.OPERATION_JOURNAL_LOG WHERE LOG_ID IN ({listID})");
            if (UsedUserID >= 0)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş əməliyyatlar hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş əməliyyatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    DeleteOperation(listID);
            }
            else
                DeleteOperation(listID);
            LoadOperations();
            OperationsGridView.FocusedRowHandle = old_row_num - 1;
            OperationsGridView.TopRowIndex = topindex;
        }

        private void OperationsGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            GridView view = sender as GridView;

            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OPERATION_JOURNAL_LOG", -1, "WHERE USED_USER_ID = " + GlobalVariables.V_UserID);

            for (int i = 0; i < view.SelectedRowsCount; i++)
            {
                int rowHandle = view.GetSelectedRows()[i];
                if (!view.IsGroupRow(rowHandle))
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OPERATION_JOURNAL_LOG", GlobalVariables.V_UserID, "WHERE USED_USER_ID = -1 AND LOG_ID = " + view.GetDataRow(rowHandle)["LOG_ID"]);
            }
        }

        private void FJournalLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OPERATION_JOURNAL_LOG", -1, "WHERE USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void DeleteOperation(string listID)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş əməliyyatları silmək istəyirsiniz?", "Əməliyyatların silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                string sql = $@"DELETE FROM CRS_USER.OPERATION_JOURNAL_LOG WHERE LOG_ID IN ({listID})";

                GlobalProcedures.ExecuteQuery(sql, "Əməliyyatlar tarixçədən silinmədi.", this.Name + "/DeleteOperation");
            }
        }
    }
}