using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.AttractedFunds
{
    public partial class FAttractedFunds : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FAttractedFunds()
        {
            InitializeComponent();
        }
        string ContractID, currency_name;

        decimal calc_debt,
            calc_amount,
            calc_payment_amount,
            calc_basic_amount,
            calc_interest_amount,
            calc_payment_interest_amount,
            calc_payment_interest_debt,
            calc_total,
            rateAmount = 1;

        bool FormStatus = false;

        List<CurrencyRates> lstRate = CurrencyRatesDAL.SelectCurrencyRatesLastDate().ToList<CurrencyRates>();
        List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();

        private void FundsGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        int topindex, old_row_num;

        private void FAttractedFunds_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                ContractBarButton.Enabled = GlobalVariables.FundContract;
                FundsBarButton.Enabled = GlobalVariables.FundPayment;
            }

            CurBarStatic.Caption = GlobalVariables.V_LastRate;
            SearchDockPanel.Hide();
            FormStatus = true;
        }

        private void LoadFundsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 T.CONTRACT_ID,
                                 FC.CONTRACT_NUMBER,
                                 FC.REGISTRATION_NUMBER,
                                    FS.NAME
                                 || ': '
                                 || (CASE
                                        WHEN FS.ID = 6
                                        THEN
                                           (SELECT B.LONG_NAME
                                              FROM CRS_USER.BANKS B, CRS_USER.BANK_CONTRACTS BC
                                             WHERE B.ID = BC.BANK_ID AND BC.FUNDS_CONTRACT_ID = FC.ID)
                                        WHEN FS.ID = 10
                                        THEN
                                           (SELECT FULLNAME
                                              FROM CRS_USER.FOUNDERS F, CRS_USER.FOUNDER_CONTRACTS FCS
                                             WHERE     F.ID = FCS.FOUNDER_ID
                                                   AND FCS.FUNDS_CONTRACT_ID = FC.ID)
                                        ELSE
                                           (SELECT NAME
                                              FROM CRS_USER.FUNDS_SOURCES_NAME
                                             WHERE     ID = FC.FUNDS_SOURCE_NAME_ID
                                                   AND SOURCE_ID = FC.FUNDS_SOURCE_ID)
                                     END)
                                 || ', '
                                 || FC.AMOUNT
                                 || ' '
                                 || C.CODE
                                    SOURCE_NAME,
                                 FC.PERIOD || ' ay' PERIOD,
                                 FP.PERCENT_VALUE || ' %' INTEREST,
                                 FC.START_DATE,
                                 FC.END_DATE,
                                 T.BUY_AMOUNT,
                                 T.PAYMENT_AMOUNT,
                                 T.BASIC_AMOUNT,
                                 T.DEBT,
                                 T.DAY_COUNT,
                                 T.INTEREST_AMOUNT,
                                 T.PAYMENT_INTEREST_AMOUNT,
                                 T.PAYMENT_INTEREST_DEBT,
                                 T.TOTAL,
                                 S.STATUS_NAME,
                                 FC.USED_USER_ID,
                                 C.CODE CURRENCY_CODE,
                                 FC.STATUS_ID,
                                 FC.INTEREST INT_INTEREST,
                                 FC.PERIOD INT_PERIOD,
                                 ROW_NUMBER () OVER (ORDER BY FC.START_DATE) ROW_NUM
                            FROM CRS_USER.ATTRACTED_FUNDS_TOTAL T,
                                 CRS_USER.FUNDS_CONTRACTS FC,
                                 CRS_USER.FUNDS_SOURCES FS,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER.STATUS S,
                                 CRS_USER.V_LAST_FUNDS_PERCENT FP
                           WHERE     T.CONTRACT_ID = FC.ID
                                 AND FC.FUNDS_SOURCE_ID = FS.ID
                                 AND FC.CURRENCY_ID = C.ID
                                 AND FC.STATUS_ID = S.ID
                                 AND FC.ID = FP.FUNDS_CONTRACTS_ID
                        ORDER BY FC.START_DATE";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, "LoadFundsDataGridView");

            FundsGridControl.DataSource = dt;

            if (FundsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                    FundsBarButton.Enabled = GlobalVariables.FundPayment;
                else
                    FundsBarButton.Enabled = true;
            }
            else
                FundsBarButton.Enabled = false;
        }

        private void ContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = FundsGridView.TopRowIndex;
            old_row_num = FundsGridView.FocusedRowHandle;
            FFundContract ffc = new FFundContract();
            ffc.RefreshFundsDataGridView += new FFundContract.DoEvent(RefreshFunds);
            ffc.ShowDialog();
            FundsGridView.TopRowIndex = topindex;
            FundsGridView.FocusedRowHandle = old_row_num;
        }

        private void ContractsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(FundsGridView, PopupMenu, e);
        }

        void RefreshFunds()
        {
            LoadFundsDataGridView();
        }

        private void LoadFFunds(string ContractID)
        {
            topindex = FundsGridView.TopRowIndex;
            old_row_num = FundsGridView.FocusedRowHandle;
            FFunds ff = new FFunds();
            ff.ContractID = ContractID;
            ff.RefreshFundsDataGridView += new FFunds.DoEvent(RefreshFunds);
            ff.ShowDialog();
            FundsGridView.TopRowIndex = topindex;
            FundsGridView.FocusedRowHandle = old_row_num;
        }

        private void FundsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFFunds(ContractID);
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurBarStatic.Caption = GlobalVariables.V_LastRate;
            LoadFundsDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(FundsGridControl);
        }

        private void FAttractedFunds_Activated(object sender, EventArgs e)
        {
            LoadFundsDataGridView();

            GlobalProcedures.GridRestoreLayout(FundsGridView, AttractedFundsRibbonPage.Text);
        }

        private void FundsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CONTRACT_NUMBER", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("BUY_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("TOTAL", "Far", e);

            if (e.Column.FieldName == "DEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }

            if (e.Column.FieldName == "DAY_COUNT")
            {
                e.Handled = true;
                e.Appearance.FontStyleDelta = FontStyle.Regular;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void FundsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = FundsGridView.GetFocusedDataRow();
            if (row != null)
            {
                ContractID = row["CONTRACT_ID"].ToString();
            }
        }

        private void FundsGridView_DoubleClick(object sender, EventArgs e)
        {
            LoadFFunds(ContractID);
        }

        private void FundsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            GlobalProcedures.GridRowCellStyleForBlock(FundsGridView, e);
            GlobalProcedures.GridRowCellStyleForClose(14, currentView, e);
        }

        private void FundsGridView_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0} : {1}   (cəmi  {2} müqavilə)", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));
        }

        private void FundsGridView_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            //GridView currentView = sender as GridView;
            //if (currentView.RowCount == 0)
            //    return;

            //if (e.SummaryProcess == CustomSummaryProcess.Start)
            //{
            //    calc_debt =
            //        calc_amount =
            //        calc_basic_amount =
            //        calc_interest_amount =
            //        calc_payment_amount =
            //        calc_payment_interest_amount =
            //        calc_payment_interest_debt =
            //        calc_total = 0;
            //}


            //if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            //{
            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("BUY_AMOUNT") == 0) //gelir
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT") == 0) //odenilen
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_payment_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0) //esas mebleg
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_basic_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //gelir
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_debt += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0) //hesablanmis faiz
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_interest_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_AMOUNT") == 0) //odenilen faiz
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_payment_interest_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) //qaliq faiz
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_payment_interest_debt += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("TOTAL") == 0) //cemi
            //    {
            //        var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
            //        var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

            //        if (rate != null)
            //            rateAmount = rate.AMOUNT;
            //        else if (currency.CODE == "AZN")
            //            rateAmount = 1;
            //        else
            //            rateAmount = 0;
            //        calc_total += Convert.ToDecimal(e.FieldValue) * rateAmount;
            //    }
            //}

            //if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            //{
            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("BUY_AMOUNT") == 0) //celb olunan
            //        e.TotalValue = calc_amount;                

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT") == 0) //odenilen
            //        e.TotalValue = calc_payment_amount;

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0) //esas mebleg
            //        e.TotalValue = calc_basic_amount;

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaliq                
            //        e.TotalValue = calc_debt;

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0) //hesablanmis faiz
            //        e.TotalValue = calc_interest_amount;

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_AMOUNT") == 0) //odenilen faiz
            //        e.TotalValue = calc_payment_interest_amount;

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) //qaliq faiz
            //        e.TotalValue = calc_payment_interest_debt;

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("TOTAL") == 0) //cemi
            //        e.TotalValue = calc_total;

            //    if (((GridSummaryItem)e.Item).FieldName.CompareTo("SOURCE_NAME") == 0 && FundsGridView.RowCount > 0) //cemi
            //    {
            //        e.TotalValue = e.GetValue("CURRENCY_CODE").ToString() + " üzrə cəmi";
            //        if (e.IsTotalSummary)
            //            e.TotalValue = "YEKUN (AZN - ilə)";
            //    }
            //}
        }

        private void FundsGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "SOURCE_NAME")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
            if (e.Column.FieldName == "DEBT")
                e.Appearance.ForeColor = Color.Red;
        }

        private void ExcellBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(FundsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(FundsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(FundsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(FundsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(FundsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(FundsGridControl, "csv");
        }

        private void FundsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(FundsGridView, AttractedFundsRibbonPage.Text);
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(FundsGridControl, "mht");
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
            {
                SearchDockPanel.Show();
                GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
            }
            else
                SearchDockPanel.Hide();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void FilterTotals()
        {
            if (FormStatus)
            {
                ColumnView view = FundsGridView;

                //RegistrationNumber
                if (!String.IsNullOrEmpty(RegistrationNumberText.Text))
                    view.ActiveFilter.Add(view.Columns["REGISTRATION_NUMBER"],
                      new ColumnFilterInfo("[REGISTRATION_NUMBER] Like '%" + RegistrationNumberText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["REGISTRATION_NUMBER"]);

                //SourceName
                if (!String.IsNullOrEmpty(SourceNameText.Text))
                    view.ActiveFilter.Add(view.Columns["SOURCE_NAME"],
                  new ColumnFilterInfo("[SOURCE_NAME] Like '%" + SourceNameText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["SOURCE_NAME"]);

                //ContractNumber
                if (!String.IsNullOrEmpty(ContractNumberText.Text))
                    view.ActiveFilter.Add(view.Columns["CONTRACT_NUMBER"],
                        new ColumnFilterInfo("[CONTRACT_NUMBER] Like '%" + ContractNumberText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CONTRACT_NUMBER"]);

                //Interest
                if (InterestValue.Value > 0)
                    view.ActiveFilter.Add(view.Columns["INT_INTEREST"],
                        new ColumnFilterInfo("[INT_INTEREST] = " + InterestValue.Value, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["INT_INTEREST"]);

                //Period
                if (PeriodValue.Value > 0)
                    view.ActiveFilter.Add(view.Columns["INT_PERIOD"],
                        new ColumnFilterInfo("[INT_PERIOD] = " + PeriodValue.Value, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["INT_PERIOD"]);

                //Currency
                if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CURRENCY_CODE"],
                        new ColumnFilterInfo(currency_name, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CURRENCY_CODE"]);

                //StartDate
                if (!String.IsNullOrEmpty(FromDateValue.Text) && !String.IsNullOrEmpty(ToDateValue.Text))
                    view.ActiveFilter.Add(view.Columns["START_DATE"],
                  new ColumnFilterInfo("[START_DATE] >= '" + FromDateValue.DateTime.ToString("yyyyMMdd") + "' AND [START_DATE] <= '" + ToDateValue.DateTime.ToString("yyyyMMdd") + "'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["START_DATE"]);

                //Status
                if ((ActiveContractBarCheck.Checked) && (!ClosedContractBarCheck.Checked))
                    view.ActiveFilter.Add(view.Columns["STATUS_ID"],
                  new ColumnFilterInfo("[STATUS_ID] = 13", ""));
                else if ((!ActiveContractBarCheck.Checked) && (ClosedContractBarCheck.Checked))
                    view.ActiveFilter.Add(view.Columns["STATUS_ID"],
                  new ColumnFilterInfo("[STATUS_ID] = 14", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["STATUS_ID"]);
            }
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            currency_name = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            FilterTotals();
        }

        private void RegistrationNumberText_EditValueChanged(object sender, EventArgs e)
        {
            FilterTotals();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = GlobalFunctions.ChangeStringToDate(FromDateValue.Text, "ddmmyyyy");
            FilterTotals();
        }

        private void FilterClearBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            RegistrationNumberText.Text =
                SourceNameText.Text =
                ContractNumberText.Text =
                CurrencyComboBox.Text =
                FromDateValue.Text =
                ToDateValue.Text = null;
            InterestValue.Value = PeriodValue.Value = 0;
            FundsGridView.ClearColumnsFilter();
        }

        private void ActiveContractBarCheck_ItemClick(object sender, ItemClickEventArgs e)
        {
            FilterTotals();
        }

        private void FundsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (FundsGridView.RowCount > 0)
                FundsBarButton.Enabled = (GlobalVariables.V_UserID > 0) ? GlobalVariables.FundPayment : true;
            else
                FundsBarButton.Enabled = false;
        }
    }
}