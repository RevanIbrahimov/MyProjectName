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
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.AttractedFunds
{
    public partial class FFunds : DevExpress.XtraEditors.XtraForm
    {
        public FFunds()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string ContractID;
        double contract_amount, debt = 0, calc_debt = 0, interest_amount = 0, payment_interest_amount = 0, interest_debt = 0, payment_interest_debt = 0, one_day_interest;
        string PaymentID, ldate, Currency;
        int PaymentUsedUserID = -1, source_id = 0, row_num, last_id, old_row_num, currency_id, diff_day;
        bool CurrentStatus = false, PaymentUsed = false, PaymentClosed = false, IsInsert = false;
        decimal interest;

        public delegate void DoEvent();
        public event DoEvent RefreshFundsDataGridView;

        private void FFunds_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                BuyAmountBarButton.Enabled = GlobalVariables.AddBuyAmount;
                ShowBuyAmountBarButton.Visibility = (GlobalVariables.EditBuyAmount) ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
                ShowPaymentAmountBarButton.Visibility = (GlobalVariables.EditFundPayment) ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
                ShowBarSubItem.Visibility = (GlobalVariables.EditBuyAmount && GlobalVariables.EditFundPayment) ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
            }

            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT USED_USER_ID,STATUS_ID FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = {ContractID}", this.Name + "/FFunds_Load");
            PaymentUsedUserID = int.Parse(dt.Rows[0]["USED_USER_ID"].ToString());
            PaymentUsed = (PaymentUsedUserID >= 0);

            PaymentClosed = (int.Parse(dt.Rows[0]["STATUS_ID"].ToString()) == 14);

            LoadContractDetails();

            if ((PaymentClosed && PaymentUsed) || (PaymentClosed && !PaymentUsed))
            {
                XtraMessageBox.Show(ContractNumberText.Text.Trim() + " saylı müqavilə bağlanılıb. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CurrentStatus = true;
            }
            else if (PaymentUsed && !PaymentClosed)
            {
                if (GlobalVariables.V_UserID != PaymentUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PaymentUsedUserID).FULLNAME;
                    XtraMessageBox.Show(ContractNumberText.Text.Trim() + " saylı müqavilə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Buna görə də həmin müqavilənin ödənişləri dəyişdirilə bilməz. Siz yalnız ödənişlərə baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;
            ComponentEnable(CurrentStatus);

            InsertPaymentsTemp();
            InsertBankOperationsTemp();
            if (source_id == 10)
                InsertCashFounderTemp();
            LoadPaymentsDataGridView();

            if (source_id == 10)
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", GlobalVariables.V_UserID, "WHERE ID = (SELECT FOUNDER_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = " + ContractID + ") AND USED_USER_ID = -1");
            PaymentsGridView.FocusedRowHandle = PaymentsGridView.RowCount - 1;
            CalculatedPercents();
        }

        private void CalculatedPercents()
        {
            try
            {
                FindLastDateAndDebt();
                diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(ldate, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ), "ddmmyyyy"));
                one_day_interest = Math.Round(((debt * (double)interest) / 100) / 360, 2);
                InterestValue.Value = (decimal)(diff_day * one_day_interest);
                ResidualPercentValue.Value = (decimal)((double)InterestValue.Value + GlobalFunctions.GetAmount($@"SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}"));
                TotalDebtValue.Value = (decimal)(Math.Round(((decimal)(debt) + ResidualPercentValue.Value), 2));
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bu günə olan faiz hesablanmadı.", diff_day.ToString(), GlobalVariables.V_UserName, "Class", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public void ComponentEnable(bool status)
        {
            StandaloneBarDockControl.Enabled = BOK.Visible = !status;
            if (status == false)
                PopupMenu.Manager = BarManager;
            else
                PopupMenu.Manager = null;
        }

        private void LoadContractDetails()
        {
            string s = $@"SELECT FC.CONTRACT_NUMBER,
                                   FC.REGISTRATION_NUMBER,
                                   FP.PERCENT_VALUE INTEREST,
                                   FC.PERIOD,
                                   TO_CHAR (FC.START_DATE, 'DD.MM.YYYY') S_DATE,
                                   TO_CHAR (FC.END_DATE, 'DD.MM.YYYY') E_DATE,
                                   FC.AMOUNT,
                                   C.ID CURRENCY_ID,
                                   C.CODE CURRENCY_CODE,
                                      FS.NAME
                                   || ': '
                                   || (CASE
                                          WHEN FS.ID = 6
                                          THEN
                                             (SELECT B.LONG_NAME
                                                FROM CRS_USER.BANKS B
                                               WHERE (B.ID) IN (SELECT BANK_ID
                                                                  FROM CRS_USER.BANK_CONTRACTS
                                                                 WHERE FUNDS_CONTRACT_ID = FC.ID))
                                          WHEN FS.ID = 10
                                          THEN
                                             (SELECT FULLNAME
                                                FROM CRS_USER.FOUNDERS
                                               WHERE ID = (SELECT FOUNDER_ID
                                                             FROM CRS_USER.FOUNDER_CONTRACTS
                                                            WHERE FUNDS_CONTRACT_ID = FC.ID))
                                          ELSE
                                             (SELECT NAME
                                                FROM CRS_USER.FUNDS_SOURCES_NAME
                                               WHERE     ID = FC.FUNDS_SOURCE_NAME_ID
                                                     AND SOURCE_ID = FC.FUNDS_SOURCE_ID)
                                       END)
                                      SOURCE_NAME,
                                   FC.FUNDS_SOURCE_ID,
                                   FC.CURRENCY_RATE
                              FROM CRS_USER.FUNDS_CONTRACTS FC,
                                   CRS_USER.FUNDS_SOURCES FS,
                                   CRS_USER.CURRENCY C,
                                   CRS_USER.V_LAST_FUNDS_PERCENT FP
                             WHERE FC.FUNDS_SOURCE_ID = FS.ID AND FC.CURRENCY_ID = C.ID AND FC.ID = FP.FUNDS_CONTRACTS_ID AND FC.ID = {ContractID}",
                     rate = null;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    ContractNumberText.Text = dr["CONTRACT_NUMBER"].ToString();
                    RegistrationNumberText.Text = dr["REGISTRATION_NUMBER"].ToString();
                    interest = Convert.ToDecimal(dr["INTEREST"].ToString());
                    InterestText.Text = interest.ToString() + " %";
                    PeriodText.Text = dr["PERIOD"].ToString() + " ay";
                    StartDateText.Text = dr["S_DATE"].ToString();
                    EndDateText.Text = dr["E_DATE"].ToString();
                    contract_amount = Convert.ToDouble(dr["AMOUNT"].ToString());
                    currency_id = Convert.ToInt32(dr["CURRENCY_ID"].ToString());
                    Currency = dr["CURRENCY_CODE"].ToString();
                    FundSourceText.Text = dr["SOURCE_NAME"].ToString();
                    source_id = Convert.ToInt32(dr["FUNDS_SOURCE_ID"].ToString());
                    if (currency_id > 1)
                        rate = " (1 " + Currency + " = " + Convert.ToDouble(dr["CURRENCY_RATE"].ToString()).ToString("N4") + " AZN)";
                    PaymentsGridView.ViewCaption = ("Məbləğ : " + contract_amount.ToString("N2") + " " + Currency + rate).Trim();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müqavilənin rekvizitləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadPaymentsDataGridView()
        {
            string s = null;
            try
            {
                s = $@"SELECT 1 SS,
                             CP.CONTRACT_ID,
                             CP.PAYMENT_DATE,
                             CP.BUY_AMOUNT,
                             CP.CURRENCY_RATE,
                             CP.PAYMENT_AMOUNT_AZN,
                             CP.PAYMENT_AMOUNT,         
                             CP.BASIC_AMOUNT,
                             CP.DEBT,
                             CP.DAY_COUNT,
                             CP.INTEREST_AMOUNT,
                             CP.PAYMENT_INTEREST_AMOUNT,
                             CP.PAYMENT_INTEREST_DEBT,
                             CP.TOTAL,
                             CP.ID,
                             ROW_NUMBER () OVER (ORDER BY CP.PAYMENT_DATE, CP.ID) ROW_NUM,
                             CP.PERCENT||' %' PERCENT
                        FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP
                       WHERE CP.IS_CHANGE IN (0, 1) AND CP.CONTRACT_ID = {ContractID}
                    ORDER BY CP.PAYMENT_DATE, CP.ID";


                PaymentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridViewForAZN");
                Payment_Rate.Visible = Payment_AmountAZN.Visible = !(Currency == "AZN");

                EnabledButton();

                PaymentsGridView.TopRowIndex = PaymentsGridView.FocusedRowHandle = old_row_num;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ödənişlər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void EnabledButton()
        {
            if (PaymentsGridView.RowCount > 0)
            {
                EditBarButton.Enabled = true;
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBuyAmountBarButton.Enabled = GlobalVariables.EditBuyAmount;
                    EditPaymentAmountBarButton.Enabled = GlobalVariables.EditFundPayment;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteFund;
                    PaymentBarButton.Enabled = GlobalVariables.AddFundPayment;
                }
                else
                    EditBuyAmountBarButton.Enabled = EditPaymentAmountBarButton.Enabled = DeleteBarButton.Enabled = PaymentBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = PaymentBarButton.Enabled = false;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
        }

        private void FFunds_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_DELETE_INVOLVED_FUNDS", "P_CONTRACT_ID", int.Parse(ContractID), "P_SOURCE_ID", source_id, "Vəsaitin uçotu temp cədvəldən silinmədi.");
            this.RefreshFundsDataGridView();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPaymentsDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PaymentsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PaymentsGridView.ViewCaption = ContractNumberText.Text + " müqaviləsi üzrə vəsait uçotu";
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");
        }

        private void PaymentsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PaymentsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("DAY_COUNT", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("BUY_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("TOTAL", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT_AZN", "Far", e);

            if (e.Column.FieldName == "TOTAL")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }

            if (e.Column.FieldName == "DEBT")
            {
                if (calc_debt < 0)
                    e.Appearance.ForeColor = Color.Red;
            }

            if (e.Column.FieldName == "INTEREST_AMOUNT")
            {
                if (interest_amount < 0)
                    e.Appearance.ForeColor = Color.Red;
            }

            if (e.Column.FieldName == "PAYMENT_INTEREST_AMOUNT")
            {
                if (payment_interest_amount < 0)
                    e.Appearance.ForeColor = Color.Red;
            }

            if (e.Column.FieldName == "PAYMENT_INTEREST_DEBT")
            {
                if (interest_debt < 0)
                    e.Appearance.ForeColor = Color.Red;
            }
        }

        void RefreshFunds()
        {
            if (IsInsert)
                old_row_num = GlobalFunctions.GetID("SELECT ROW_NUM-1 FROM (SELECT ROW_NUMBER() OVER (ORDER BY CP.PAYMENT_DATE,CP.ID) ROW_NUM,CP.ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP ORDER BY CP.PAYMENT_DATE,CP.ID) WHERE ID = (SELECT MAX(ID) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP)");
            else
                old_row_num = row_num - 1;

            LoadPaymentsDataGridView();
        }

        private void LoadFBuyAmount(string transaction, string paymentid, string lastdate, double debt, string contractid, int sourceid)
        {
            FBuyAmount fba = new FBuyAmount();
            fba.TransactionName = transaction;
            fba.PaymentID = paymentid;
            fba.PaymentCount = PaymentsGridView.RowCount;
            fba.LastDate = lastdate;
            fba.Debt = debt;
            fba.ContractID = contractid;
            fba.Currency = Currency;
            fba.CurrencyID = currency_id;
            fba.SourceID = sourceid;
            fba.LastID = last_id;
            fba.ContractNumber = ContractNumberText.Text;
            fba.PaymentInterestDebt = payment_interest_debt;
            fba.RefreshFundsDataGridView += new FBuyAmount.DoEvent(RefreshFunds);
            fba.ShowDialog();
        }

        private void BuyAmountBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsInsert = true;
            FindLastDateAndDebt();
            LoadFBuyAmount("INSERT", null, ldate, debt, ContractID, source_id);
        }

        private void BDateCalculator_Click(object sender, EventArgs e)
        {
            FDateCalculator fdc = new FDateCalculator();
            fdc.ShowDialog();
        }

        private void PaymentsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ShowBuyAmountBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsInsert = false;
            last_id = 0;
            LoadFBuyAmount("SHOW", PaymentID, ldate, debt, ContractID, source_id);
        }

        private void ShowPaymentAmountBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsInsert = false;
            LoadFPaymentAmountAddEdit("SHOW", PaymentsGridView.RowCount, ldate, debt, Currency, currency_id, ContractID, FundSourceText.Text.Trim(), ContractNumberText.Text, StartDateText.Text.Trim(), PaymentID, source_id);
        }

        private void PaymentsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            double basic_amount, amount = 0;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaligi hesabliyir
                {
                    amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("BUY_AMOUNT").SummaryItem.SummaryValue);
                    basic_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("BASIC_AMOUNT").SummaryItem.SummaryValue);
                    calc_debt = amount - basic_amount;
                    e.TotalValue = calc_debt;
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) // qaliq faizi hesabliyir
                {
                    interest_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("INTEREST_AMOUNT").SummaryItem.SummaryValue);
                    payment_interest_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("PAYMENT_INTEREST_AMOUNT").SummaryItem.SummaryValue);
                    interest_debt = interest_amount - payment_interest_amount;
                    e.TotalValue = interest_debt;
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("TOTAL") == 0) // cemi hesabliyir
                {
                    e.TotalValue = interest_debt + calc_debt;
                }
            }
        }

        private void PercentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPaymentPercentAddEdit("INSERT", Currency, currency_id, ContractID, FundSourceText.Text.Trim(), ContractNumberText.Text, StartDateText.Text.Trim(), PaymentID, source_id);
        }

        private void LoadFPaymentPercentAddEdit(string transaction, string currency, int currency_id, string contract_id, string sourcename, string contractcode, string startdate, string paymentid, int sourceid)
        {
            FPaymentPercentAddEdit fpa = new FPaymentPercentAddEdit();
            fpa.TransactionName = transaction;
            fpa.Currency = currency;
            fpa.CurrencyID = currency_id;
            fpa.ContractID = contract_id;
            fpa.SourceName = sourcename;
            fpa.ContractCode = contractcode;
            fpa.ContractStartDate = startdate;
            fpa.PayDate = ldate;
            fpa.PaymentID = int.Parse(paymentid);
            fpa.RefreshPaymentsDataGridView += new FPaymentPercentAddEdit.DoEvent(RefreshFundsPayment);
            fpa.ShowDialog();
        }

        private void PaymentsGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            decimal buy_amount = 0, payment_amount = 0;
            GridView currentView = sender as GridView;

            buy_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "BUY_AMOUNT"));
            payment_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "PAYMENT_AMOUNT"));
            if (buy_amount == 0 && payment_amount == 0)
                e.Appearance.BackColor = e.Appearance.BackColor2 = Color.GreenYellow;

        }

        private void EditPercentAmountBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPaymentPercentAddEdit("EDIT", Currency, currency_id, ContractID, FundSourceText.Text.Trim(), ContractNumberText.Text, StartDateText.Text.Trim(), PaymentID, source_id);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            old_row_num = row_num - 1;
            DialogResult dialogResult = XtraMessageBox.Show(ldate + " tarixinə olan ödənişi silmək istəyirsiniz? Əgər bu ödənişi silib yadda saxlasaz, bu zaman cəlb olunmuş vəsaitlərin qalığı yenidən hesablanacaq.", "Ödənişin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithThreeParametrAndUser("CRS_USER_TEMP.PROC_DELETE_FUNDS_TEMP", "P_CONTRACT_ID", int.Parse(ContractID), "P_PAYMENT_ID", int.Parse(PaymentID), "P_LAST_DATE", ldate, "Cəlb olunmuş vəsaitlərin ödənişləri silinmədi.");
                GlobalProcedures.UpdateFundChange(ldate, int.Parse(ContractID), 1);
            }
            RefreshFunds();
        }

        private void EditBuyAmountBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsInsert = false;
            last_id = 0;
            LoadFBuyAmount("EDIT", PaymentID, ldate, debt, ContractID, source_id);
        }

        private void PaymentsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            ldate = null;
            DataRow row = PaymentsGridView.GetFocusedDataRow();
            if (row != null)
            {
                PaymentID = row["ID"].ToString();
                ldate = row["PAYMENT_DATE"].ToString().Substring(0, 10);
                debt = Convert.ToDouble(row["DEBT"].ToString());
                EditBuyAmountBarButton.Enabled = (GlobalVariables.EditBuyAmount && Convert.ToDouble(row["BUY_AMOUNT"].ToString()) > 0);
                EditPaymentAmountBarButton.Enabled = (GlobalVariables.EditFundPayment && Convert.ToDouble(row["PAYMENT_AMOUNT"].ToString()) > 0);
                EditPercentAmountBarButton.Enabled = (Convert.ToDouble(row["PAYMENT_AMOUNT"].ToString()) == 0 && Convert.ToDouble(row["BUY_AMOUNT"].ToString()) == 0);
                ShowBuyAmountBarButton.Enabled = (Convert.ToDouble(row["BUY_AMOUNT"].ToString()) > 0);
                ShowPaymentAmountBarButton.Enabled = (Convert.ToDouble(row["PAYMENT_AMOUNT"].ToString()) >= 0);
                row_num = Convert.ToInt32(row["ROW_NUM"].ToString());
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));            
            InsertPayments();
            InsertBankOperations();
            if (source_id == 10)
                InsertCashFounderPayment();            
            GlobalProcedures.SplashScreenClose();
            this.Close();
        }

        private void InsertPayments()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.FUNDS_PAYMENTS WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                             $@"INSERT INTO CRS_USER.FUNDS_PAYMENTS(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,MANUAL_INTEREST,PERCENT_TYPE,PERCENT)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,MANUAL_INTEREST,PERCENT_TYPE,PERCENT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE = 1 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Ödənişlər əsas cədvələ daxil edilmədi.",
                                             this.Name + "/InsertPayments");
            GlobalProcedures.CalculatedAttractedFundsTotal(int.Parse(ContractID));
        }

        private void InsertPaymentsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_FUND_PAYMENTS_TEMP", "P_CONTRACT_ID", int.Parse(ContractID), "Cəlb edilmiş vəsaitlərin ödənişləri temp cədvələ daxil edilmədi.");
        }

        private void InsertBankOperations()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND APPOINTMENT_ID IN (SELECT ID FROM CRS_USER.BANK_APPOINTMENTS WHERE APPOINTMENT_TYPE_ID = 4) AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                             $@"INSERT INTO CRS_USER.BANK_OPERATIONS(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,CONTRACT_PAYMENT_ID,CONTRACT_CODE,FUNDS_PAYMENT_ID,FUNDS_CONTRACT_ID)SELECT ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,CONTRACT_PAYMENT_ID,CONTRACT_CODE,FUNDS_PAYMENT_ID,FUNDS_CONTRACT_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE = 1 AND APPOINTMENT_ID IN (SELECT ID FROM CRS_USER.BANK_APPOINTMENTS WHERE APPOINTMENT_TYPE_ID = 4) AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Bankın əməliyyatları əsas cədvələ daxil olmadı.",
                                                    this.Name + "/InsertBankOperations");
            string s = $@"SELECT BANK_ID,TO_CHAR(OPERATION_DATE,'DD/MM/YYYY') OPERATION_DATE FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND APPOINTMENT_ID IN (SELECT ID FROM CRS_USER.BANK_APPOINTMENTS WHERE APPOINTMENT_TYPE_ID = 4) AND USED_USER_ID = {GlobalVariables.V_UserID}";
            try
            {

                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/InsertBankOperations");

                foreach (DataRow dr in dt.Rows)
                {
                    GlobalProcedures.UpdateBankOperationDebtWithBank(dr["OPERATION_DATE"].ToString(), Convert.ToInt32(dr["BANK_ID"].ToString()));
                    GlobalProcedures.UpdateBankOperationDebt(dr["OPERATION_DATE"].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Mədaxil bank əməliyyatlarına daxil edilmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void InsertBankOperationsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_FUND_PAYMENTS_BO_TEMP", "P_CONTRACT_ID", int.Parse(ContractID), "Bank əməliyyatları temp cədvələ daxil edilmədi.");
        }

        private void InsertCashFounderPayment()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CASH_OPERATIONS WHERE DESTINATION_ID IN (3,8) AND OPERATION_OWNER_ID IN (SELECT ID FROM CRS_USER_TEMP.CASH_FOUNDER_TEMP WHERE IS_CHANGE <> 0 AND (FOUNDER_ID,FOUNDER_CARD_ID) IN (SELECT FOUNDER_ID,FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}) AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                             $@"DELETE FROM CRS_USER.CASH_FOUNDER WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.CASH_FOUNDER_TEMP WHERE IS_CHANGE <> 0 AND (FOUNDER_ID,FOUNDER_CARD_ID) IN (SELECT FOUNDER_ID,FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}) AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                                 "Təsisçi ilə hesablaşmanın mədaxili kassadan silinmədi",
                                              this.Name + "/InsertCashFounderPayment");
            string s = $@"SELECT ID,FOUNDER_ID,FOUNDER_CARD_ID,TO_CHAR(PAYMENT_DATE,'DD/MM/YYYY'),APPOINTMENT,AMOUNT,INC_EXP,NOTE,FUND_PAYMENT_ID FROM CRS_USER_TEMP.CASH_FOUNDER_TEMP WHERE IS_CHANGE = 1 AND (FOUNDER_ID,FOUNDER_CARD_ID) IN (SELECT FOUNDER_ID,FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}) AND USED_USER_ID = {GlobalVariables.V_UserID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/InsertCashFounderPayment");

                foreach (DataRow dr in dt.Rows)
                {

                    GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_FOUNDER(ID,FOUNDER_ID,FOUNDER_CARD_ID,PAYMENT_DATE,APPOINTMENT,AMOUNT,INC_EXP,NOTE,FUND_PAYMENT_ID)VALUES(" + dr[0] + "," + dr[1] + "," + dr[2] + ",TO_DATE('" + dr[3] + "','DD/MM/YYYY'),'" + dr[4] + "'," + dr[5] + "," + dr[6] + ",'" + dr[7] + "'," + dr[8] + ")",
                                                    "Təsisçi ilə hesablaşmanın mədaxili daxil olunmadı.");
                    if (Convert.ToInt32(dr[6].ToString()) == 1)
                        GlobalProcedures.InsertCashOperation(3, Convert.ToInt32(dr[0].ToString()), dr[3].ToString(), null, Convert.ToDouble(dr[5].ToString()), 0, 1);
                    else
                        GlobalProcedures.InsertCashOperation(8, Convert.ToInt32(dr[0].ToString()), dr[3].ToString(), null, 0, Convert.ToDouble(dr[5].ToString()), 1);
                    GlobalProcedures.UpdateCashDebt(dr[3].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçi ilə hesablaşmanın parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void InsertCashFounderTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_CASH_FOUNDER_TEMP", "P_CONTRACT_ID", int.Parse(ContractID), "Təsisçi ilə hesablaşmalar temp cədvələ daxil olunmadı.");
        }

        void RefreshFundsPayment(decimal a, int p)
        {
            if (IsInsert)
                old_row_num = GlobalFunctions.GetID("SELECT ROW_NUM-1 FROM (SELECT ROW_NUMBER() OVER (ORDER BY CP.PAYMENT_DATE,CP.ID) ROW_NUM,CP.ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP ORDER BY CP.PAYMENT_DATE,CP.ID) WHERE ID = (SELECT MAX(ID) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP)");
            else
                old_row_num = row_num - 1;

            LoadPaymentsDataGridView();
        }

        private void LoadFPaymentAmountAddEdit(string transaction, int paymentcount, string lastdate, double debt, string currency, int currency_id, string contract_id, string sourcename, string contractcode, string startdate, string paymentid, int sourceid)
        {
            FPaymentAmountAddEdit fpa = new FPaymentAmountAddEdit();
            fpa.TransactionName = transaction;
            fpa.PaymentCount = paymentcount;
            fpa.LastDate = lastdate;
            fpa.Debt = debt;
            fpa.Currency = currency;
            fpa.CurrencyID = currency_id;
            fpa.ContractID = contract_id;
            fpa.SourceName = sourcename;
            fpa.ContractCode = contractcode;
            fpa.ContractStartDate = startdate;
            fpa.PaymentID = Convert.ToInt32(paymentid);
            fpa.SourceID = sourceid;
            fpa.PaymentInterestDebt = payment_interest_debt;
            fpa.RefreshPaymentsDataGridView += new FPaymentAmountAddEdit.DoEvent(RefreshFundsPayment);
            fpa.ShowDialog();
        }

        private void PaymentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsInsert = true;
            FindLastDateAndDebt();
            LoadFPaymentAmountAddEdit("INSERT", PaymentsGridView.RowCount, ldate, debt, Currency, currency_id, ContractID, FundSourceText.Text.Trim(), ContractNumberText.Text, StartDateText.Text.Trim(), PaymentID, source_id);
        }

        private void FindLastDateAndDebt()
        {
            ldate = null;
            try
            {
                if (PaymentsGridView.RowCount == 0)
                {
                    ldate = StartDateText.Text;
                    debt = 0;
                }
                else
                {
                    List<FundPayment> lstPayments = FundPaymentDAL.SelectFundPayments(1, int.Parse(ContractID)).ToList<FundPayment>();
                    if (lstPayments.Count == 0)
                        return;

                    var lastPayments = lstPayments.Where(item => item.IS_CHANGE == 0 || item.IS_CHANGE == 1).LastOrDefault();
                    ldate = lastPayments.PAYMENT_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
                    debt = Math.Round((double)lastPayments.DEBT, 2);
                    payment_interest_debt = Math.Round((double)lastPayments.PAYMENT_INTEREST_DEBT, 2);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sonuncu tarix və qaliq tapılmadı.", ldate, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void EditPaymentAmountBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsInsert = false;
            LoadFPaymentAmountAddEdit("EDIT", PaymentsGridView.RowCount, ldate, debt, Currency, currency_id, ContractID, FundSourceText.Text.Trim(), ContractNumberText.Text, StartDateText.Text.Trim(), PaymentID, source_id);
        }

        private void PaymentsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(DateTime.Today.ToString("d", Class.GlobalVariables.V_CultureInfoAZ));
        }
    }
}