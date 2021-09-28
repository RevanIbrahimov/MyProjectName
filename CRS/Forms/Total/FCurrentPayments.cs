using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.Data;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using CRS.Class.Tables;
using CRS.Class;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using CRS.Class.DataAccess;

namespace CRS.Forms.Total
{
    public partial class FCurrentPayments : DevExpress.XtraEditors.XtraForm
    {
        public FCurrentPayments()
        {
            InitializeComponent();
        }
        decimal
            calc_payment_amount,
            calc_payment_amount_azn,
            calc_basic_amount,
            calc_interest_amount,
            calc_payed_amount,
            rateAmount = 1;

        string currency_name;
        bool FormStatus = false;
        List<CurrencyRates> lstRate = CurrencyRatesDAL.SelectCurrencyRatesLastDate().ToList<CurrencyRates>();
        List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();

        private void FCurrentPayments_Load(object sender, EventArgs e)
        {
            FromDateValue.EditValue = DateTime.Today;
            ToDateValue.EditValue = DateTime.Today;
            GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
            LoadPaymentsDataGridView();
            FormStatus = true;
        }

        private void LoadPaymentsDataGridView()
        {
            string s = $@"SELECT *
                            FROM (SELECT NVL (COM.COMMITMENT_NAME, CUS.CUSTOMER_NAME) CUSTOMER_NAME,
                                         CON.CONTRACT_CODE,
                                         CP.PAYMENT_DATE,
                                         CP.PAYED_AMOUNT,
                                         CP.PAYED_AMOUNT_AZN,
                                         CP.INSURANCE_AMOUNT,
                                         CP.PAYMENT_AMOUNT,
                                         CP.CURRENCY_RATE,
                                         CP.PAYMENT_AMOUNT_AZN,
                                         ROUND (CP.BASIC_AMOUNT * CP.CURRENCY_RATE, 2) BASIC_AMOUNT,
                                         ROUND (CP.PAYMENT_INTEREST_AMOUNT * CP.CURRENCY_RATE, 2)
                                            INTEREST_AMOUNT,
                                         CON.CURRENCY_CODE,
                                         DECODE (
                                            CP.BANK_CASH,
                                            'C', 'Kassa',
                                               'Bank - '
                                            || (SELECT B.SHORT_NAME
                                                  FROM CRS_USER.BANKS B
                                                 WHERE B.ID IN (SELECT BANK_ID
                                                                  FROM CRS_USER.BANK_OPERATIONS
                                                                 WHERE CONTRACT_PAYMENT_ID = CP.ID)))
                                            PAYMENT_TYPE,
                                         CP.INSERT_DATE,
                                         CP.PAYED_PENALTY
                                    FROM CRS_USER.CUSTOMER_PAYMENTS CP,
                                         CRS_USER.V_CONTRACTS CON,
                                         CRS_USER.V_CUSTOMERS CUS,
                                         CRS_USER.V_CONTRACT_COMMITMENTS COM
                                   WHERE     CUS.ID = CON.CUSTOMER_ID
                                         AND CUS.PERSON_TYPE_ID = CON.CUSTOMER_TYPE_ID
                                         AND CON.CONTRACT_ID = CP.CONTRACT_ID
                                         AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                  UNION ALL
                                  SELECT NVL (COM.COMMITMENT_NAME, CUS.CUSTOMER_NAME) CUSTOMER_NAME,
                                         CON.CONTRACT_CODE,
                                         IP.PAY_DATE PAYMENT_DATE,
                                         0 PAYED_AMOUNT,
                                         0 PAYED_AMOUNT_AZN,
                                         IP.PAYED_AMOUNT INSURANCE_AMOUNT,
                                         0 PAYMENT_AMOUNT,
                                         1 CURRENCY_RATE,
                                         0 PAYMENT_AMOUNT_AZN,
                                         0 BASIC_AMOUNT,
                                         0 INTEREST_AMOUNT,
                                         CON.CURRENCY_CODE,
                                         NULL PAYMENT_TYPE,
                                         IP.INSERT_DATE,
                                         0 PAYED_PENALTY
                                    FROM CRS_USER.INSURANCE_PAYMENT IP,
                                         CRS_USER.INSURANCES I,
                                         CRS_USER.V_CONTRACTS CON,
                                         CRS_USER.V_CUSTOMERS CUS,
                                         CRS_USER.V_CONTRACT_COMMITMENTS COM
                                   WHERE     IP.INSURANCE_ID = I.ID
                                         AND I.CONTRACT_ID = CON.CONTRACT_ID
                                         AND CUS.ID = CON.CUSTOMER_ID
                                         AND IP.CUSTOMER_PAYMENT_ID = 0
                                         AND CUS.PERSON_TYPE_ID = CON.CUSTOMER_TYPE_ID
                                         AND CON.CONTRACT_ID = COM.CONTRACT_ID(+))
                           WHERE PAYMENT_DATE BETWEEN TO_DATE ('{FromDateValue.Text}', 'DD/MM/YYYY')
                                                  AND TO_DATE ('{ToDateValue.Text}', 'DD/MM/YYYY')
                        ORDER BY PAYMENT_DATE";

            PaymentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridView", "Ödənişlər cədvələ yüklənmədi.");
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                ToDateValue.Properties.MinValue = GlobalFunctions.ChangeStringToDate(FromDateValue.Text, "ddmmyyyy");
                LoadPaymentsDataGridView();
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
                LoadPaymentsDataGridView();
        }

        private void PaymentsGridView_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0} : {1}   (cəmi  {2} müqavilə)", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));
        }

        private void PaymentsGridView_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
                return;

            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                calc_basic_amount =
                   calc_interest_amount =
                   calc_payment_amount =
                   calc_payed_amount =
                   calc_payment_amount_azn = 0;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {      
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT_AZN") == 0) //gelir
                {
                    calc_payment_amount_azn += Convert.ToDecimal(e.FieldValue);
                }                

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0) //esas mebleg
                {
                    calc_basic_amount += Convert.ToDecimal(e.FieldValue);
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0) //hesablanmis faiz
                {
                    calc_interest_amount += Convert.ToDecimal(e.FieldValue);
                }
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT_AZN") == 0)
                    e.TotalValue = calc_payment_amount_azn;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0)
                    e.TotalValue = calc_basic_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0)
                    e.TotalValue = calc_interest_amount;


                if (((GridSummaryItem)e.Item).FieldName.CompareTo("CUSTOMER_NAME") == 0 && PaymentsGridView.RowCount > 0)
                {
                    e.TotalValue = e.GetValue("CURRENCY_CODE").ToString() + " üzrə cəmi";
                    if (e.IsTotalSummary)
                        e.TotalValue = "YEKUN (AZN - ilə)";
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("CURRENCY_RATE") == 0) //cemi
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);
                    if (rate != null)
                        e.TotalValue = rate.AMOUNT.ToString("N4");
                }
            }
        }

        private void PaymentsGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CONTRACT_CODE", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("CURRENCY_RATE", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT_AZN", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYED_AMOUNT_AZN", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INSURANCE_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYED_PENALTY", "Far", e);
        }

        private void PaymentsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e, 8f, 8f, 8f, 8f);           
        }

        private void ExcellBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "mht");
        }

        private void PaymentsGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CUSTOMER_NAME", "Near", e);
        }

        private void CompareBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Bookkeeping.FCompareInterest fci = new Bookkeeping.FCompareInterest();
            fci.fromdate = FromDateValue.Text;
            fci.todate = ToDateValue.Text;
            fci.ShowDialog();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PaymentsGridControl);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPaymentsDataGridView();
        }

        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            currency_name = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";

            if (FormStatus)
            {
                ColumnView view = PaymentsGridView;

                if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CURRENCY_CODE"],
                        new ColumnFilterInfo(currency_name, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CURRENCY_CODE"]);
            }
        }
    }
}
