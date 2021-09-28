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

namespace CRS.Forms.Bookkeeping
{
    public partial class FCustomerNormalPercent : DevExpress.XtraEditors.XtraForm
    {
        public FCustomerNormalPercent()
        {
            InitializeComponent();
        }
        string contractID, currency, contractCode;
        decimal amount;
        int topindex, old_row_num;

        private void FCustomerNormalPercent_Load(object sender, EventArgs e)
        {
            FromDateValue.EditValue = ToDateValue.EditValue = DateTime.Today;
        }

        private void LoadData()
        {
            string sql = $@"SELECT C.CONTRACT_ID,
                                   C.CONTRACT_CODE,
                                   CUS.CUSTOMER_NAME,
                                   CUS.VOEN,
                                   ROUND(PS.INTEREST_AMOUNT, 3) INTEREST_AMOUNT,
                                   PS.MONTH_DATE,
                                   PS.REAL_DATE,
                                   C.AMOUNT,
                                   C.CURRENCY_CODE
                              FROM CRS_USER.PAYMENT_SCHEDULES PS,
                                   CRS_USER.V_CONTRACTS C,
                                   CRS_USER.V_CUSTOMERS CUS
                             WHERE     PS.CONTRACT_ID = C.CONTRACT_ID
                                   AND C.CUSTOMER_ID = CUS.ID
                                   AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND PS.MONTH_DATE BETWEEN TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}','DD.MM.YYYY')
                            ORDER BY PS.MONTH_DATE DESC";

            ListGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadData", "Müştərilərin qrafik üzrə faizləri açılmadı.");
        }

        private void DateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadData();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ListGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ListGridControl, "xls");
        }

        private void ListGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, List_SS, e);
        }

        private void ListGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ListGridView, PopupMenu, e);
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ListGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ListGridView.GetFocusedDataRow();
            if (row != null)
            {
                contractID = row["CONTRACT_ID"].ToString();
                currency = row["CURRENCY_CODE"].ToString();
                contractCode = row["CONTRACT_CODE"].ToString();
                amount = Convert.ToDecimal(row["AMOUNT"]);
            }
        }

        private void ListGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(List_SS, "Center", e);
        }

        private void PaymentSheduleBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = ListGridView.TopRowIndex;
            old_row_num = ListGridView.FocusedRowHandle;
            Total.FPaymentSchedules fps = new Total.FPaymentSchedules();
            fps.ContractID = contractID;
            fps.Amount = amount.ToString("N2") + " " + currency;
            fps.ContractCode = contractCode;
            fps.ShowDialog();
            ListGridView.TopRowIndex = topindex;
            ListGridView.FocusedRowHandle = old_row_num;
        }
    }
}