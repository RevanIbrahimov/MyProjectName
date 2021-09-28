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

namespace CRS.Forms.Total
{
    public partial class FTemporaryPayments : DevExpress.XtraEditors.XtraForm
    {
        public FTemporaryPayments()
        {
            InitializeComponent();
        }
        string PaymentID;
        int old_row_num, topindex;

        private void FTemporaryPayments_Load(object sender, EventArgs e)
        {
            RefreshPaymets();
        }

        private void LoadPaymentDataGridView()
        {
            string sql = $@"SELECT 1 SS,
                                   P.ID,
                                   C.CONTRACT_CODE,
                                   P.PAY_DATE,
                                   P.PAYING,
                                   P.AMOUNT,
                                   P.AMOUNT_AZN,
                                   PT.TYPE_NAME,
                                   P.NOTE,
                                   U.USER_FULLNAME INSERT_USER,
                                   P.INSERT_DATE,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = P.UPDATE_USER)
                                      UPDATE_USER,
                                   P.UPDATE_DATE,
                                   P.IS_COMMIT,
                                   P.USED_USER_ID
                              FROM CRS_USER.CONTRACT_TOMORROW_PAYMENTS P,
                                   CRS_USER.V_CONTRACTS C,
                                   CRS_USER.PAYMENT_TYPE PT,
                                   CRS_USER.V_USERS U
                             WHERE     P.CONTRACT_ID = C.CONTRACT_ID
                                   AND P.PAYMENT_TYPE_ID = PT.ID
                                   AND P.INSERT_USER = U.ID
                                   AND P.IS_COMMIT = 0";
            PaymentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql);

            if(PaymentsGridView.RowCount > 0)
            {
                EditBarButton.Enabled = DeleteBarButton.Enabled = CommitBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = CommitBarButton.Enabled = false;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void RefreshPaymets()
        {
            LoadPaymentDataGridView();
        }

        private void LoadFTemporaryPaymentAddEdit(string transaction, string temporary_id)
        {
            old_row_num = PaymentsGridView.FocusedRowHandle;
            topindex = PaymentsGridView.TopRowIndex;
            FTemporaryPaymentAddEdit ft = new FTemporaryPaymentAddEdit();
            ft.TransactionName = transaction;
            ft.PaymentID = temporary_id;
            ft.RefreshPaymentsDataGridView += new FTemporaryPaymentAddEdit.DoEvent(RefreshPaymets);
            ft.ShowDialog();
            PaymentsGridView.FocusedRowHandle = old_row_num;
            PaymentsGridView.TopRowIndex = topindex;
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTemporaryPaymentAddEdit("INSERT", null);
        }

        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PaymentsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshPaymets();
        }

        private void PaymentsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PaymentsGridView.GetFocusedDataRow();
            if (row != null)
            {
                PaymentID = row["ID"].ToString();
                DeleteBarButton.Enabled = (Convert.ToInt32(row["IS_COMMIT"].ToString()) == 0);
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFTemporaryPaymentAddEdit("EDIT", PaymentID);
        }

        private void PaymentsGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditBarButton.Enabled)
                LoadFTemporaryPaymentAddEdit("EDIT", PaymentID);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            old_row_num = PaymentsGridView.FocusedRowHandle;
            topindex = PaymentsGridView.TopRowIndex;
            
            PaymentsGridView.FocusedRowHandle = old_row_num;
            PaymentsGridView.TopRowIndex = topindex;
        }
    }
}