using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using CRS.Class;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;

namespace CRS.Forms.Total
{
    public partial class FPaymentSchedules : DevExpress.XtraEditors.XtraForm
    {
        public FPaymentSchedules()
        {
            InitializeComponent();
        }
        public string ContractCode, ContractID, Amount, OrderID, CurrencyCode;
        public int Version = 1;

        private void FPaymentSchedules_Load(object sender, EventArgs e)
        {
            //PaymentSchedulesGridView.ViewCaption = "Lizinq Məbləği: " + Amount;
            LoadPaymentScheduleDataGridView();
            this.Text = ContractCode + " saylı lizinq müqaviləsi üzrə ödəniş qrafiki";
        }

        private void LoadPaymentScheduleDataGridView()
        {
            string filter = null;

            if (ExtendBarCheck.Checked && !FirstBarCheck.Checked)
                filter = "AND P.VERSION > 0";
            else if (!ExtendBarCheck.Checked && FirstBarCheck.Checked)
                filter = "AND P.VERSION = 0";

            string s = $@"SELECT P.ORDER_ID SS,
                                 P.ID,
                                 P.MONTH_DATE,
                                 P.MONTHLY_PAYMENT,
                                 P.BASIC_AMOUNT,
                                 P.INTEREST_AMOUNT,
                                 P.DEBT,
                                 C.CODE CURRENCY_CODE,
                                 P.IS_CHANGE_DATE,
                                 P.ORDER_ID,
                                 P.VERSION,
                                 A.CONTRACT_AMOUNT,
                                 (CASE
                                     WHEN P.VERSION = 0
                                     THEN
                                        (P.VERSION + 1)||'. Əsas (Lizinq məbləği: '
                                        || A.CONTRACT_AMOUNT
                                        || ' ' || C.CODE ||', tarix intervalı: '|| TO_CHAR(A.START_DATE,'DD.MM.YYYY')||' - '||TO_CHAR(A.END_DATE,'DD.MM.YYYY')||')'
                                     WHEN P.VERSION > 0
                                     THEN
                                        (P.VERSION + 1)||'. Uzadılmış (Lizinq məbləği: '
                                        || A.CONTRACT_AMOUNT
                                        || ' ' || C.CODE ||', tarix intervalı: '|| TO_CHAR(A.START_DATE,'DD.MM.YYYY')||' - '||TO_CHAR(A.END_DATE,'DD.MM.YYYY')||')' 
                                  END)
                                    VERSION_DESCRIPTION
                            FROM CRS_USER.PAYMENT_SCHEDULES P,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER.V_CONTRACT_AMOUNT A
                           WHERE     P.CURRENCY_ID = C.ID
                                 AND P.CONTRACT_ID = A.CONTRACT_ID
                                 AND P.VERSION = A.VERSION
                                 AND P.CONTRACT_ID = {ContractID}{filter}
                        ORDER BY P.VERSION, P.ORDER_ID DESC";
            PaymentSchedulesGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentScheduleDataGridView", "Ödəniş qrafiki açılmadı.");            
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPaymentScheduleDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PaymentSchedulesGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentSchedulesGridControl, "xls");
        }

        private void PaymentSchedulesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentSchedulesGridView, PopupMenu, e);
        }

        private void PaymentSchedulesGridView_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            if (info.Column.Caption == "Qrafikin növü")
            {
                string rowValue = view.GetGroupRowValue(e.RowHandle, info.Column).ToString();
                string colorName = getColorName(rowValue);
                info.GroupText = info.Column.Caption + ": <color=" + colorName + ">" + info.GroupValueText + "</color> ";
                info.GroupText += "<color=LightSteelBlue>" + view.GetGroupSummaryText(e.RowHandle) + "</color> ";
            }
        }

        string getColorName(string value)
        {
            if (value.IndexOf("Uzadılmış") > 0)
                return "Red";
            else
                return "Blue";
        }

        private void PaymentSchedulesGridView_EndGrouping(object sender, EventArgs e)
        {
            if ((sender as GridView).DataRowCount > 0)
                (sender as GridView).SetRowExpanded(-1, true);
        }

        private void PaymentSchedulesGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "SS")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
        }

        private void PaymentSchedulesGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (e.Column == Schedules_VersionDescription)
            {
                if (Convert.ToInt16(PaymentSchedulesGridView.GetListSourceRowCellValue(rowIndex, "VERSION").ToString()) == 0)
                    e.Value = "Əsas (Lizinq məbləği: " + Convert.ToDecimal(PaymentSchedulesGridView.GetListSourceRowCellValue(rowIndex, "CONTRACT_AMOUNT")).ToString("N2") + " " + CurrencyCode + ")";
                else
                    e.Value = "Uzadılmış (Lizinq məbləği: " + Convert.ToDecimal(PaymentSchedulesGridView.GetListSourceRowCellValue(rowIndex, "CONTRACT_AMOUNT")).ToString("N2") + " " + CurrencyCode + ")";
            }
        }

        private void PaymentSchedulesGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("MONTHLY_PAYMENT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
        }

        private void PaymentSchedulesGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;            
            
            if (e.Column.FieldName == "DEBT")
            {
                //int version = Convert.ToInt16(currentView.GetRowCellValue(e.RowHandle, "VERSION"));
                //if (version != Version)
                //    return;

                decimal value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DEBT"));
                if (value == 0 || currentView.GetRowCellDisplayText(e.RowHandle, currentView.Columns["ORDER_ID"]) == OrderID)
                    e.Appearance.ForeColor = Color.Red;
            }
        }

        private void PaymentSchedulesGridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string payment_order = View.GetRowCellDisplayText(e.RowHandle, View.Columns["ORDER_ID"]);
                if (payment_order == OrderID)
                {
                    e.Appearance.BackColor = Color.FromArgb(0xCC, 0xCC, 0x00);
                    e.Appearance.FontStyleDelta = FontStyle.Bold;                    
                }
            }
        }
    }
}