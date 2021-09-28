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
using DevExpress.XtraEditors;

namespace CRS.Forms.PaymentTask
{
    public partial class FPaymentTask : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FPaymentTask()
        {
            InitializeComponent();
        }
        string TaskID;
        int old_row_num = 0, topindex;

        private void NewBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFTaskAddEdit("INSERT", null);
        }

        void RefreshTask(int taskID, DateTime taskDate, bool clickOK)
        {
            LoadTaskGridView();
        }

        private void LoadFTaskAddEdit(string transaction, string task_id)
        {
            if (transaction == "INSERT")
                old_row_num = topindex = TaskGridView.RowCount;
            else
            {
                old_row_num = TaskGridView.FocusedRowHandle;
                topindex = TaskGridView.TopRowIndex;
            }

            FTaskAddEdit ft = new FTaskAddEdit();
            ft.TransactionName = transaction;
            ft.TaskID = task_id;
            ft.RefreshDataGridView += new FTaskAddEdit.DoEvent(RefreshTask);
            ft.ShowDialog();

            TaskGridView.FocusedRowHandle = old_row_num;
            TaskGridView.TopRowIndex = topindex;
        }

        private void FPaymentTask_Load(object sender, EventArgs e)
        {
            if (GlobalVariables.V_UserID > 0)
            {
                NewBarButton.Enabled = GlobalVariables.AddTask;
                VatBarButton.Enabled = GlobalVariables.EditVatBank;
            }            
        }

        private void LoadTaskGridView()
        {
            string sql = $@"SELECT 1 SS,
                                     PT.ID TASK_ID,
                                     PT.CODE,
                                     TT.TYPE_NAME,
                                     PT.TDATE,
                                     PT.AMOUNT,
                                     C.CODE CURRENCY_CODE,
                                     B.LONG_NAME PAYING_BANK,
                                     NVL(A.ACCEPTOR_NAME,'{GlobalFunctions.ReadSetting("Company")}') ACCEPTOR_NAME,
                                     (SELECT USER_FULLNAME
                                        FROM CRS_USER.V_USERS
                                       WHERE ID = PT.INSERT_USER)
                                        INSERT_USER,
                                     PT.INSERT_DATE,
                                     (SELECT USER_FULLNAME
                                        FROM CRS_USER.V_USERS
                                       WHERE ID = PT.UPDATE_USER)
                                        UPDATE_USER,
                                     PT.UPDATE_DATE,
                                     PT.USED_USER_ID
                                FROM CRS_USER.PAYMENT_TASKS PT,
                                     CRS_USER.TASK_TYPE TT,
                                     CRS_USER.CURRENCY C,
                                     CRS_USER.TASK_ACCEPTOR A,
                                     CRS_USER.BANKS B
                               WHERE     PT.CURRENCY_ID = C.ID
                                     AND PT.TYPE_ID = TT.ID
                                     AND PT.ACCEPTOR_ID = A.ID(+)
                                     AND PT.PAYING_BANK_ID = B.ID
                            ORDER BY PT.TDATE, TT.ORDER_ID, PT.TASK_NUMBER ";

            TaskGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadTaskGridView", "Ödəniş tapşırıqları yüklənmədi.");

            if (TaskGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditTask;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteTask;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void FPaymentTask_Activated(object sender, EventArgs e)
        {
            LoadTaskGridView();
            TaskGridView.FocusedRowHandle = TaskGridView.RowCount - 1;
            TaskGridView.TopRowIndex = TaskGridView.RowCount - 1;
        }

        private void TaskGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = TaskGridView.GetFocusedDataRow();
            if (row != null)
                TaskID = row["TASK_ID"].ToString();
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadTaskGridView();            
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(TaskGridControl);
        }

        private void TaskGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void EditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFTaskAddEdit("EDIT", TaskID);
        }

        private void TaskGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadFTaskAddEdit("EDIT", TaskID);
        }

        private void TaskGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(TaskGridView, e);
        }

        private void TaskGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(TaskGridView, PopupMenu, e);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TaskGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TaskGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TaskGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TaskGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TaskGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TaskGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TaskGridControl, "mht");
        }

        void RefreshData()
        {
            LoadTaskGridView();
        }

        private void TemplatesBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FTemplates ft = new FTemplates();
            ft.RefreshData += new FTemplates.DoEvent(RefreshData);
            ft.ShowDialog();
        }

        private void TaskTypeBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FTaskType ftt = new FTaskType();
            ftt.RefreshTypeDataGridView += new FTaskType.DoEvent(RefreshData);
            ftt.ShowDialog();
        }

        private void AcceptorBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FAcceptors fa = new FAcceptors();
            fa.RefreshAcceptorDataGridView += new FAcceptors.DoEvent(RefreshData);
            fa.ShowDialog();
        }

        private void VatBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FVat fv = new FVat();
            fv.ShowDialog();
        }

        private void TaskGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Task_SS, e);
        }

        private void TaskGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (TaskGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditTask;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteTask;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            old_row_num = TaskGridView.FocusedRowHandle;
            topindex = TaskGridView.TopRowIndex;
            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.PAYMENT_TASKS WHERE ID = {TaskID}");
            if ((UsedUserID == -1) || (GlobalVariables.V_UserID == UsedUserID))
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş ödəniş tapşırığının məlumatlarını silmək istəyirsiniz?", "Ödəniş tapşırığının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)                
                    GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.INSURANCE_TRANSFER WHERE PAYMENT_TASK_ID = {TaskID}",
                                                     $@"DELETE FROM CRS_USER.PAYMENT_TASKS WHERE ID = {TaskID}", 
                                                     "Seçilmiş ödəniş tapşırığının məlumatları silinmədi.");                                    
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçilmiş tapşırığın məlumatları " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatlarını silmək olmaz.", "Seçilmiş ödəniş tapşırığının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadTaskGridView();
            TaskGridView.FocusedRowHandle = old_row_num;
            TaskGridView.TopRowIndex = topindex;
        }
    }
}