using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class.Tables;
using CRS.Class;
using Oracle.ManagedDataAccess.Client;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Configuration;
using CRS.Class.Views;
using CRS.Class.DataAccess;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WordToPDF;

namespace CRS.Forms.Total
{
    public partial class FPayment : DevExpress.XtraEditors.XtraForm
    {
        public FPayment()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string ContractID, CustomerID, CustomerName, ContractCode, CustomerCode, SDate, EDate, Lizinq, Currency, CommitmentName, DebtDate;
        public double Amount;
        public int Interest, Period, CommitmentID, CommitmentPersonTypeID, CustomerTypeID, IsSpecialAttention, CreditNameID, CurrencyID;
        public bool IsExtend;

        double debt,
            monthly_amount,
            payment_amount,
            total,
            basic_amount,
            interest_amount,
            payment_interest_amount,
            payment_interest_debt = 0,
            calc_debt = 0,
            interest_debt = 0,
            one_day_interest,
            debt_penalty = 0,
            startPercent = 0,
            diffInterestAmount = 0,
            sumPaymentAmount = 0;

        decimal a;
        int PaymentUsedUserID = -1, diff_day, topindex, old_row_num, insuranceID, extendInterest = 0, payDay;
        int? parentContractID = null;
        bool CurrentStatus = false, PaymentUsed = false, PaymentClosed = false, isActive = false, clearing = false;
        string ldate, lclearingdate, paymentid, bank_cash, paymentdate, clearingdate, rate, toolTip = null, ContractImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages";
        List<Contract> lstContract = null;

        public delegate void DoEvent();
        public event DoEvent RefreshTotalsDataGridView;

        private void FPayment_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            PeriodText.Text = Period + " ay";
            StartDateText.Text = SDate;
            EndDateText.Text = EDate;
            ObjectText.Text = Lizinq;
            CustomerCodeText.Text = CustomerCode;
            CustomerNameText.Text = CustomerName;
            CommitmentNameValue.Text = CommitmentName;
            
            payDay = Convert.ToInt16(SDate.Substring(0, 2));
            CommitmentNameLabel.Visible = CommitmentNameValue.Visible = !(CommitmentID == 0);
            LoadCustomerImage();

            GenerateViewCaption();

            InterestCurrencyLabel.Text = Currency;
            ResidualPercentCurrencyLabel.Text = Currency;
            TotalDebtCurrencyLabel.Text = Currency;
            InsertPaymentsTemp();
            InsertOperationJournalTemp();

            ExtendLabel.Visible = ExtendStartDateText.Visible = ExtendEndDateText.Visible = ExtendSimvolLabel.Visible = ExtendPercentLabel.Visible = ExtendIntrestText.Visible = IsExtend;
            if (IsExtend)
            {
                List<ContractExtend> lstContractExtend = ContractExtendDAL.SelectContractExtend(0, int.Parse(ContractID)).ToList<ContractExtend>();
                if (lstContractExtend.Count > 0)
                {
                    var extend = lstContractExtend.LastOrDefault();
                    ExtendStartDateText.Text = extend.START_DATE.ToString("dd.MM.yyyy");
                    ExtendEndDateText.Text = extend.END_DATE.ToString("dd.MM.yyyy");
                    extendInterest = extend.INTEREST;
                    ExtendIntrestText.Text = extendInterest.ToString() + " %";
                    monthly_amount = (double)extend.MONTHLY_AMOUNT;
                }
            }


            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
            lstContract = ContractDAL.SelectContract(int.Parse(ContractID)).ToList<Contract>();
            var contract = lstContract.First();
            PaymentUsedUserID = contract.USED_USER_ID;
            monthly_amount = (!IsExtend) ? (double)contract.MONTHLY_AMOUNT : monthly_amount;
            PaymentUsed = (PaymentUsedUserID >= 0);
            PaymentClosed = (contract.STATUS_ID == 6);
            InterestText.Text = contract.INTEREST + " %";
            parentContractID = contract.PARENT_ID;
            MonthlyAmountValue.EditValue = monthly_amount;
            if ((PaymentClosed && PaymentUsed) || (PaymentClosed && !PaymentUsed))
            {
                XtraMessageBox.Show(ContractCode + " saylı lizinq müqaviləsi bağlanılıb. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CurrentStatus = true;
            }
            else if (PaymentUsed && !PaymentClosed)
            {
                if (GlobalVariables.V_UserID != PaymentUsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PaymentUsedUserID).FULLNAME;
                    XtraMessageBox.Show(CustomerName + "na məxsus olan ödənişlər hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş ödənişlərin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;
            if (!CurrentStatus)
                CurrentStatus = !(String.IsNullOrWhiteSpace(DebtDate));

            ComponentEnable(CurrentStatus);
            LoadPaymentsDataGridView(Currency);
            CalculatedPercents();
            CalculatedPenalties();
            InsuranceCostCurrencyLabel.Visible = InsuranceLabel.Visible = InsuranceDebtValue.Visible = BInsurance.Visible = CreditNameID == 1;
            InsuranceDebt();
            DescriptionPicture.Visible = contract.CONTRACT_IMAGE_COUNT > 0;
        }

        private void InsuranceDebt()
        {
            if (CreditNameID != 1)
                return;

            string sql = $@"SELECT I.AMOUNT
                                   - NVL (IP.PAYED_AMOUNT, 0)
                                      DEBT,
                                  I.ID
                              FROM CRS_USER_TEMP.V_SUM_INSURANCE_PAYMENT_TEMP IP, CRS_USER.V_LAST_INSURANCES I
                             WHERE I.ID = IP.INSURANCE_ID(+) AND I.CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/InsuranceDebt", "Sığortanın borcu açılmadı.");
            if (dt.Rows.Count > 0)
            {
                InsuranceDebtValue.EditValue = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                insuranceID = Convert.ToInt32(dt.Rows[0]["ID"]);
            }
        }

        private void InsuranceWarningMessage()
        {
            if (InsuranceDebtValue.Visible && InsuranceDebtValue.Value > 0)
            {
                Contracts.FSpecialWarning fs = new Contracts.FSpecialWarning();
                fs.Description = "<color=red>DİQQƏT!!!</color>  Bu müqavilə üzrə " + InsuranceDebtValue.Value.ToString("n2") + " AZN sığorta borcu var.";
                fs.ShowDialog();
            }
        }

        private void GenerateViewCaption()
        {
            double percent = GlobalFunctions.GetAmount($@"SELECT START_PERCENT FROM CRS_USER.CONTRACT_START_PERCENT WHERE CONTRACT_ID = {ContractID}", this.Name + "/CalculatedPercents");

            if (Currency == "AZN")
                PaymentsGridView.ViewCaption = "Verilən : " + Amount.ToString("N2") + " " + Currency + ((percent > 0) ? " [ Başlanğıc qalıq faiz = " + percent.ToString("N2") + " ]" : null);
            else
            {
                rate = GlobalFunctions.GetName($@"SELECT CUR.VALUE||' '||CUR.CODE||' = '||C.CURRENCY_RATE||' AZN' FROM CRS_USER.CONTRACTS C,CRS_USER.CURRENCY CUR WHERE C.CURRENCY_ID = CUR.ID AND C.ID = {ContractID}");
                PaymentsGridView.ViewCaption = "Verilən : " + Amount.ToString("N2") + " " + Currency + "   (" + rate + ")" + ((percent > 0) ? " [ Başlanğıc qalıq faiz = " + percent.ToString("N2") + " ]" : null);
            }
        }

        private void FindLastDateAndDebt()
        {
            ldate = null;
            try
            {
                if (PaymentsGridView.RowCount == 0)
                {
                    ldate = StartDateText.Text;
                    lclearingdate = StartDateText.Text;
                    debt = Amount;
                    payment_interest_debt = 0;
                    debt_penalty = 0;
                }
                else
                {
                    List<Payments> lstPayments = PaymentsDAL.SelectPayments(1, int.Parse(ContractID)).ToList<Payments>();
                    if (lstPayments.Count == 0)
                        return;

                    var lastPayments = lstPayments.Where(item => item.IS_CHANGE != 2).LastOrDefault();

                    ldate = lastPayments.PAYMENT_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
                    lclearingdate = lastPayments.CLEARING_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
                    debt = Math.Round((double)lastPayments.DEBT, 2);
                    debt_penalty = (double)lastPayments.PENALTY_DEBT;
                    payment_interest_debt = Math.Round((double)lastPayments.PAYMENT_INTEREST_DEBT, 2);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sonuncu tarix və qaliq tapılmadı", ldate, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CalculatedPercents()
        {
            try
            {
                FindLastDateAndDebt();
                startPercent = GlobalFunctions.ContractStartPercent(int.Parse(ContractID));

                diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(lclearingdate, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ), "ddmmyyyy"));
                one_day_interest = Math.Round(((debt * Interest) / 100) / 360, 2);
                InterestValue.Value = (decimal)((diff_day * one_day_interest) + startPercent);
                InterestValue.Value = InterestValue.Value > 0 ? InterestValue.Value : 0;
                ResidualPercentValue.Value = (decimal)((double)InterestValue.Value + GlobalFunctions.GetAmount($@"SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND CONTRACT_ID = {ContractID}"));
                //TotalDebtValue.Value = (decimal)(Math.Round(((decimal)(debt_penalty + debt) + ResidualPercentValue.Value), 2));
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bu günə olan faiz hesablanmadı.", diff_day.ToString(), GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CalculatedPenalties()
        {
            DateTime lastPayDate = GlobalFunctions.ChangeStringToDate(lclearingdate, "ddmmyyyy");
            DateTime payDate = lastPayDate;
            int order_id = 0;

            DataTable dtPayment = GlobalFunctions.GenerateDataTable($@"SELECT NVL(INTEREST_AMOUNT, 0) INTEREST_AMOUNT, NVL(PAYMENT_INTEREST_AMOUNT, 0) PAYMENT_INTEREST_AMOUNT, NVL(PAYMENT_AMOUNT, 0) PAYMENT_AMOUNT FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}");
            if (dtPayment.Rows.Count > 0)
            {
                DataView dv = new DataView();
                dv = new DataView(dtPayment);
                object sumInterestAmountObject = dtPayment.Compute("Sum(INTEREST_AMOUNT)", dv.RowFilter),
                       sumPaymentInterestAmountObject = dtPayment.Compute("Sum(PAYMENT_INTEREST_AMOUNT)", dv.RowFilter),
                       sumPaymentAmountObject = dtPayment.Compute("Sum(PAYMENT_AMOUNT)", dv.RowFilter);

                diffInterestAmount = Math.Abs(Math.Round(Convert.ToDouble(sumInterestAmountObject), 2) - Math.Round(Convert.ToDouble(sumPaymentInterestAmountObject), 2));
                sumPaymentAmount = Convert.ToDouble(sumPaymentAmountObject);
            }

            if (sumPaymentAmount == 0)
            {
                int lastDateMonth = lastPayDate.Month + 1,
                        lastDateYear = lastPayDate.Year;

                if (lastDateMonth > 12)
                {
                    lastDateMonth = 1;
                    lastDateYear = lastDateYear + 1;
                }

                payDate = (payDay == 31 || payDay == 29) ? new DateTime(lastDateYear, lastDateMonth,
                                                                    DateTime.DaysInMonth(lastDateYear, lastDateMonth)) : new DateTime(lastDateYear, lastDateMonth, payDay);
            }
            List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(int.Parse(ContractID)).ToList<PaymentSchedules>();
            var schedulesPenalty = lstSchedules.Where(s => s.REAL_DATE < DateTime.Today).ToList<PaymentSchedules>();
            if (schedulesPenalty.Count == 0)
                order_id = 0;
            else
                order_id = schedulesPenalty.Max(s => s.ORDER_ID);

            int penaltyDayCount = GlobalFunctions.Days360(payDate, DateTime.Today);
            double interestPenalty = 0, DebtPenalty = 0;
            //cerime faizi
            double debtPaymentAmount = order_id * monthly_amount - sumPaymentAmount;

            if (debtPaymentAmount > 0 && penaltyDayCount > 0)
            {
                DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT * FROM CRS_USER.CONTRACT_INTEREST_PENALTIES WHERE CONTRACT_ID = {ContractID} ORDER BY ID");
                if (dt.Rows.Count == 1)
                {
                    DataView dv = new DataView();
                    dv = new DataView(dt);
                    interestPenalty = Convert.ToDouble(dt.Compute("Max(INTEREST)", dv.RowFilter));
                    DebtPenalty = Math.Round(debtPaymentAmount * interestPenalty / 100, 2) * penaltyDayCount;
                }
            }

            PenaltyValue.Value = (decimal)(DebtPenalty + debt_penalty);
            TotalDebtValue.Value = (decimal)debt + ResidualPercentValue.Value + PenaltyValue.Value;
        }

        public void ComponentEnable(bool status)
        {
            if (GlobalVariables.V_UserID > 0 && !status)
            {
                NewBarSub.Enabled = (GlobalVariables.AddPaymentToCash || GlobalVariables.AddPaymentToBank || GlobalVariables.AddPaymentToOther);
                NewCashBarButton.Enabled = (GlobalVariables.AddPaymentToCash);
                NewBankBarButton.Enabled = (GlobalVariables.AddPaymentToBank);
                OtherBarButton.Enabled = GlobalVariables.AddPaymentToOther;
            }
            else if (status)
                NewBarSub.Enabled = ChangeBarSubItem.Enabled = AttentionBarButton.Enabled = OtherBarButton.Enabled = false;

            BOK.Visible = !status;
        }

        private void InsertPenaltyTemp()
        {
            int penalty_temp_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID);
            if (penalty_temp_count == 0)
                GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                        "INSERT INTO CRS_USER_TEMP.BALANCE_PENALTIES_TEMP(ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_COMMIT,PENALTY_STATUS,CUSTOMER_PAYMENT_ID,USED_USER_ID) SELECT ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_COMMIT,PENALTY_STATUS,CUSTOMER_PAYMENT_ID," + Class.GlobalVariables.V_UserID + " FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID,
                                                        "Cərimə temp cədvələ daxil edilmədi.");
        }

        private void InsertOperationJournalTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_JOURNAL_TEMP", "P_CONTRACT_ID", ContractID, "Əməliyyatlar jurnalın temp cədvəlinə daxil edilmədi.");
        }

        private void InsertPaymentsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_PAYMENTS_TEMP", "P_CONTRACT_ID", ContractID, "Ödənişlər temp cədvələ daxil edilmədi.");
        }

        private void InsertPayments()
        {
            string s = null;
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CUSTOMER_PAYMENTS WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                             $@"INSERT INTO CRS_USER.CUSTOMER_PAYMENTS(ID,CUSTOMER_ID,CONTRACT_ID,PAYMENT_DATE,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,REQUIRED_AMOUNT,PAYMENT_NAME,NOTE,BANK_CASH,PENALTY_DEBT,IS_PENALTY,PENALTY_AMOUNT,PAYED_PENALTY,BANK_ID,CUSTOMER_TYPE_ID,PAYED_AMOUNT,PAYED_AMOUNT_AZN,INSURANCE_CHECK,INSURANCE_AMOUNT,CONTRACT_PERCENT,CLEARING_DATE,IS_CLEARING,CLEARING_CALCULATED,IS_CHANGED_INTEREST,CHANGED_PAY_INTEREST_AMOUNT,IS_PENALTY_DEBT,OVERDUE_PERCENT)SELECT ID,CUSTOMER_ID,CONTRACT_ID,PAYMENT_DATE,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,REQUIRED_AMOUNT,PAYMENT_NAME,NOTE,BANK_CASH,PENALTY_DEBT,IS_PENALTY,PENALTY_AMOUNT,PAYED_PENALTY,BANK_ID,CUSTOMER_TYPE_ID,PAYED_AMOUNT,PAYED_AMOUNT_AZN,INSURANCE_CHECK,INSURANCE_AMOUNT,CONTRACT_PERCENT,CLEARING_DATE,IS_CLEARING,CLEARING_CALCULATED,IS_CHANGED_INTEREST,CHANGED_PAY_INTEREST_AMOUNT,IS_PENALTY_DEBT,OVERDUE_PERCENT FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE = 1 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Ödənişlər əsas cədvələ daxil edilmədi.",
                                                    "InsertPayments");

            //GlobalProcedures.ExecuteThreeQuery("DELETE FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE <> 0 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
            //                                        "INSERT INTO CRS_USER.CONTRACT_BALANCE_PENALTIES(ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_COMMIT,PENALTY_STATUS,CUSTOMER_PAYMENT_ID)SELECT ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_COMMIT,PENALTY_STATUS,CUSTOMER_PAYMENT_ID FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE = 1 AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
            //                                        "UPDATE CRS_USER.CONTRACT_CALCULATED_PENALTIES SET IS_BALANCE = 1,IS_COMMIT = 1 WHERE IS_BALANCE = 1 AND IS_COMMIT = 0 AND CONTRACT_ID = " + ContractID + " AND EXISTS (SELECT ID FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE = 1 AND CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
            //                                        "Cərimələr əsas cədvələ daxil edilmədi.",
            //                                        "InsertPayments");

            if (insuranceID != 0)
                GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_INSERT_INSURANCE_PAYMENT", "P_INSURANCE_ID", insuranceID, "P_TYPE", 1, "Ödənişlər əsas cədvələ daxil edilmədi.");

            //kassa
            try
            {
                s = $@"SELECT CP.ID OWNER_ID,TO_CHAR(PAYMENT_DATE,'DD/MM/YYYY'),CT.CODE||C.CODE CON_CODE,(CASE C.CURRENCY_ID WHEN 1 THEN CP.PAYMENT_AMOUNT ELSE CP.PAYMENT_AMOUNT_AZN END) INCOME,CP.IS_CHANGE FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP,CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE CP.BANK_CASH = 'C' AND CP.CONTRACT_ID = C.ID AND C.CREDIT_TYPE_ID= CT.ID AND CP.IS_CHANGE <> 0 AND CP.CONTRACT_ID = {ContractID} AND CP.USED_USER_ID = {GlobalVariables.V_UserID} AND CP.CUSTOMER_TYPE_ID = {CustomerTypeID} ORDER BY CP.PAYMENT_DATE,CP.ID";

                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/InsertPayments").Rows)
                {
                    if (Convert.ToInt32(dr[4].ToString()) == 1)
                        GlobalProcedures.InsertCashOperation(1, int.Parse(dr[0].ToString()), dr[1].ToString(), dr[2].ToString(), Convert.ToDouble(dr[3].ToString()), 0, 1);
                    else
                        GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.CASH_OPERATIONS WHERE DESTINATION_ID = 1 AND OPERATION_OWNER_ID = " + dr[0],
                                                            "Kassadan lizinq ödənişləri silinmədi.",
                                                            "InsertPayments (kassa)");
                    GlobalProcedures.UpdateCashDebt(dr[1].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişləri kassaya daxil olmadı.", s, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            //bank
            try
            {
                s = $@"SELECT ID,BANK_ID,TO_CHAR(OPERATION_DATE,'DD/MM/YYYY') OPERATION_DATE,IS_CHANGE FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_PAYMENT_ID <> 0 AND USED_USER_ID = {GlobalVariables.V_UserID}";

                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/InsertPayments").Rows)
                {
                    if (Convert.ToInt32(dr["IS_CHANGE"].ToString()) == 1)
                        GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE APPOINTMENT_ID = 3 AND ID = {dr["ID"]}",
                                                         $@"INSERT INTO CRS_USER.BANK_OPERATIONS(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,CONTRACT_PAYMENT_ID,CONTRACT_CODE,ACCOUNTING_PLAN_ID)SELECT ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,CONTRACT_PAYMENT_ID,CONTRACT_CODE,ACCOUNTING_PLAN_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE ID = {dr["ID"]}",
                                                                "Bank əməliyyatları əsas cədvələ daxil olmadı.",
                                                                "InsertPayments (bank)");
                    else
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE APPOINTMENT_ID = 3 AND ID = {dr["ID"]}",
                                                                "Kassadan lizinq ödənişləri silinmədi.",
                                                                "InsertPayments (bank)");
                    GlobalProcedures.UpdateBankOperationDebtWithBank(dr["OPERATION_DATE"].ToString(), Convert.ToInt32(dr["BANK_ID"].ToString()));
                    GlobalProcedures.UpdateBankOperationDebt(dr["OPERATION_DATE"].ToString());
                }
                //eger bankın ödənişi digər banka keçibse bu zaman köhnə bankın qalığı dəyişilir
                GlobalProcedures.UpdateBankDebtWithChanges();
                GlobalProcedures.CalculatedLeasingTotal(ContractID);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişləri bank əməliyyatlarına daxil olmadı.", s, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void UpdateContractStatus()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET STATUS_ID = 6 WHERE ID = {ContractID}",
                                                    "Müqavilənin statusu dəyişdirilmədi.",
                                                    this.Name + "/UpdateContractStatus");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadCustomerImage()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.IMAGE FROM CRS_USER.CUSTOMER_IMAGE T WHERE T.CUSTOMER_ID = {CustomerID}", this.Name + "/LoadCustomerImage");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["IMAGE"]))
                {
                    Byte[] BLOBData = (byte[])dr["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    CustomerPictureBox.Image = Image.FromStream(stmBLOBData);
                    stmBLOBData.Close();
                }
                else
                {
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "RU":
                            CustomerPictureBox.Properties.NullText = "Фотография клиентов";
                            break;
                        case "EN":
                            CustomerPictureBox.Properties.NullText = "Customer picture";
                            break;
                    }
                }
            }
        }

        private void PaymentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentsGridView, PopupMenu, e);
        }

        private void FPayment_Shown(object sender, EventArgs e)
        {
            if (CurrentStatus)
                return;

            //InsuranceWarningMessage();

            if (IsSpecialAttention == 1)
            {
                Contracts.FSpecialWarning fs = new Contracts.FSpecialWarning();
                fs.Description = "<color=red>DİQQƏT!!!</color>  Bu müqaviləyə xüsusi nəzarət var";
                fs.ShowDialog();
            }

            string sql = $@"SELECT U.USER_FULLNAME,PN.NOTE,TO_CHAR(PN.INSERT_DATE, 'DD.MM.YYYY HH24:MI:SS') INSERT_DATE
                                      FROM CRS_USER.PAYMENT_NOTES PN, CRS_USER.V_USERS U
                                     WHERE     PN.INSERT_USER_ID = U.ID
                                           AND PN.CONTRACT_ID = {ContractID}
                                           AND PN.ID = (SELECT MAX (ID)
                                                          FROM CRS_USER.PAYMENT_NOTES
                                                         WHERE CONTRACT_ID = PN.CONTRACT_ID)";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/FPayment_Shown");
            if (dt.Rows.Count == 0)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                XtraMessageBox.Show("<b><color=104,0,0>" + dr["NOTE"] + "</color></b>\n\n\n<b>Qeyd yazan istifadəçi : </b>" + dr["USER_FULLNAME"] + "\n<b>Tarix : </b>" + dr["INSERT_DATE"], "Diqqət ediləcək qeyd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PaymentsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void EnabledButton()
        {
            if (PaymentsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = (GlobalVariables.EditPayment && !CurrentStatus);
                    DeleteBarButton.Enabled = (GlobalVariables.DeletePayment && !CurrentStatus);
                    CashToBankBarButton.Enabled = (GlobalVariables.CashToBank && !CurrentStatus);
                    BankToCashBarButton.Enabled = (GlobalVariables.BankToCash && !CurrentStatus);
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = ChangeBarSubItem.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = ChangeBarSubItem.Enabled = false;
        }

        private void OtherBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindLastDateAndDebt();
            LoadFPaymentAddEdit("INSERT", PaymentsGridView.RowCount, ldate, Amount, debt, Currency, Interest, monthly_amount, CustomerID, ContractID, 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, "D");
        }

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void PaymentsGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            int calculated = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CLEARING_CALCULATED"));
            if (calculated == 0)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor2);
            }
        }

        private void BInsurance_Click(object sender, EventArgs e)
        {
            Contracts.FInsurancePayment fi = new Contracts.FInsurancePayment();
            fi.InsuranceID = insuranceID;
            fi.TypeID = 1;
            fi.FromCustomerPayment = 0;
            fi.RefreshDataGridView += new Contracts.FInsurancePayment.DoEvent(InsuranceDebt);
            fi.ShowDialog();
        }

        private void PrintPdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Print();
        }

        private void PrintWordBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Print(false);
        }

        private void FPayment_Activated(object sender, EventArgs e)
        {
            if (isActive)
                return;

            isActive = true;
            PaymentsGridView.FocusedRowHandle = PaymentsGridView.RowCount - 1;
        }

        private void DescriptionPicture_Click(object sender, EventArgs e)
        {
            //FContractImages fci = new FContractImages();
            //fci.ContractID = int.Parse(ContractID);
            //fci.ContractCode = ContractCodeText.Text.Trim();
            //fci.ShowDialog();

            LoadContractImages();
        }

        private void LoadContractImages()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.IMAGE FROM CRS_USER.CONTRACT_IMAGES T WHERE T.CONTRACT_ID = {ContractID}", this.Name + "/LoadContractImages");

            int iCount = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["IMAGE"]))
                {
                    Byte[] BLOBData = (byte[])dr["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);

                    if (!Directory.Exists(ContractImagePath))
                    {
                        Directory.CreateDirectory(ContractImagePath);
                    }
                    string filePath = ContractImagePath + "\\" + iCount + "_" + ContractCode + ".jpeg";
                    GlobalProcedures.DeleteFile(filePath);
                    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                    iCount++;
                    Process.Start(filePath);
                }
            }
        }

        private void PaymentsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (PaymentsGridView.GetListSourceRowCellValue(rowIndex, "NOTE").ToString().Length > 0)
                e.Value = Properties.Resources.notes_16;
            else
                e.Value = null;
        }

        private void NoteToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            GridHitInfo hitInfo = PaymentsGridView.CalcHitInfo(e.ControlMousePosition);
            DataRow drCurrentRow = PaymentsGridView.GetDataRow(hitInfo.RowHandle);
            if (drCurrentRow == null)
                return;

            if (String.IsNullOrEmpty(drCurrentRow["NOTE"].ToString()))
                return;

            if (hitInfo.InRow == false)
                return;

            if (hitInfo.Column == null)
                return;

            if (hitInfo.Column != Payments_Notes)
                return;

            SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
            toolTipArgs.AllowHtmlText = DefaultBoolean.True;

            toolTip = null;
            toolTipArgs.Title.Text = "<color=255,0,0>Qeyd</color>";
            toolTipArgs.Contents.Text = drCurrentRow["NOTE"].ToString();
            toolTipArgs.Contents.Image = Properties.Resources.notes_32;


            e.Info = new ToolTipControlInfo();
            e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
            e.Info.ToolTipType = ToolTipType.SuperTip;
            e.Info.SuperTip = new SuperToolTip();
            e.Info.SuperTip.Setup(toolTipArgs);
        }

        void RefresfGridCaption(decimal a)
        {
            GenerateViewCaption();
        }

        private void StartPercentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FStartPercentAdd fs = new FStartPercentAdd();
            fs.ContractID = int.Parse(ContractID);
            fs.RefreshPaymentsDataGridView += new FStartPercentAdd.DoEvent(RefresfGridCaption);
            fs.ShowDialog();
        }

        private void AttentionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FAttentions fa = new FAttentions();
            fa.ContractCode = ContractCode;
            fa.CustomerName = (CommitmentName.Length != 0) ? CommitmentName : CustomerName;
            fa.ContractID = ContractID;
            fa.ShowDialog();
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

        private void CommitmentNameValue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                if (CommitmentPersonTypeID == 1)
                {
                    FCommitmentInfo fci = new FCommitmentInfo();
                    fci.CommitmentID = CommitmentID;
                    fci.ShowDialog();
                }
                else if (CommitmentPersonTypeID == 2)
                {
                    FJuridicalCommitmentInfo fjc = new FJuridicalCommitmentInfo();
                    fjc.CommitmentID = CommitmentID;
                    fjc.ShowDialog();
                }
            }
            else
            {
                Customer.FCommitments fc = new Customer.FCommitments();
                fc.ContractCode = ContractCodeText.Text;
                fc.ContractID = ContractID;
                fc.ShowDialog();
            }
        }

        private void PaymentsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Payments_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("DAY_COUNT", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("TOTAL", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PENALTY_AMOUNT", "Far", e);
            //if (Currency != "AZN")
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT_AZN", "Far", e);
            if (e.Column.FieldName == "TOTAL")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void LoadPaymentsDataGridView(string currency)
        {
            string s = null, debtDate = (String.IsNullOrWhiteSpace(DebtDate)) ? "" : $@" AND CP.PAYMENT_DATE <= TO_DATE('{DebtDate}','DD.MM.YYYY')",
                   sortType = (GlobalVariables.V_DefaultDateSort == 1) ? "DESC" : "ASC";

            s = $@"SELECT 1 SS,
                             CP.CUSTOMER_ID,
                             CP.CONTRACT_ID,
                             CP.PAYMENT_DATE,
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
                             CP.PENALTY_AMOUNT,
                             (CASE WHEN CP.BANK_CASH = 'C' THEN 'Kassa' 
                                    WHEN CP.BANK_CASH = 'B' THEN 'Bank, ' || B.SHORT_NAME 
                                    WHEN CP.BANK_CASH = 'D' THEN 'Digər' 
                             ELSE NULL END) PAYMENT_TYPE,                             
                             BANK_CASH,
                             ROW_NUMBER () OVER (ORDER BY CP.PAYMENT_DATE, CP.ID) ROW_NUM,
                             CP.NOTE,
                             CP.INSURANCE_CHECK,
                             CP.INSURANCE_AMOUNT,
                             CP.CONTRACT_PERCENT,
                             CP.CLEARING_DATE,
                             LEAD (CP.PAYMENT_AMOUNT, 1, 0) OVER (ORDER BY CP.CLEARING_DATE, CP.ID)
                                    CLEARING,
                             CP.CLEARING_CALCULATED,
                             CP.PAYED_PENALTY,
                             CP.PENALTY_DEBT
                        FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP, CRS_USER.BANKS B
                       WHERE     CP.IS_CHANGE IN (0, 1)
                             AND CP.BANK_ID = B.ID(+)
                             AND CP.CONTRACT_ID = {ContractID}{debtDate}
                    ORDER BY CP.PAYMENT_DATE {sortType}, CP.ID {sortType}";

            PaymentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridView", "Ödənişlər cədvələ yüklənmədi.");

            Payments_CurrencyRate.Visible = Payments_AmountAZN.Visible = !(currency == "AZN");

            StartPercentBarButton.Enabled = (PaymentsGridView.RowCount == 0);

            EnabledButton();
        }

        private void PaymentsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaligi hesabliyir
                {
                    basic_amount = Convert.ToDouble(PaymentsGridView.Columns.ColumnByFieldName("BASIC_AMOUNT").SummaryItem.SummaryValue);
                    calc_debt = Amount - basic_amount;
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
                    total = interest_debt + calc_debt;
                }
            }
        }

        void RefreshPayments(decimal a, string p)
        {
            LoadPaymentsDataGridView(Currency);
            CalculatedPercents();
            CalculatedPenalties();
            InsuranceDebt();
        }

        private void LoadFPaymentAddEdit(string transaction, int paymentcount, string lastdate, double amount, double debt, string currency, int interest, double monthly_amount, string customer_id, string contract_id, double payment_amount, string customername, string contractcode, string startdate, string paymentid, double debtpenalty, string paymentType)
        {
            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FPaymentAddEdit fpae = new FPaymentAddEdit();
            fpae.TransactionName = transaction;
            fpae.PaymentCount = paymentcount;
            fpae.LastDate = lastdate;
            fpae.Amount = amount;
            fpae.Debt = debt;
            fpae.Currency = currency;
            fpae.Interest = interest;
            fpae.MonthlyAmount = monthly_amount;
            fpae.CustomerID = customer_id;
            fpae.ContractID = contract_id;
            fpae.PaymentAmount = payment_amount;
            fpae.PaymentInterestDebt = payment_interest_debt;
            fpae.CustomerName = customername;
            fpae.CommitmentName = CommitmentName;
            fpae.ContractCode = contractcode;
            fpae.ContractStartDate = startdate;
            fpae.PaymentID = paymentid;
            fpae.DebtPenalty = debtpenalty;
            fpae.CustomerTypeID = CustomerTypeID;
            fpae.PaymentType = paymentType;
            fpae.RefreshPaymentsDataGridView += new FPaymentAddEdit.DoEvent(RefreshPayments);
            fpae.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void LoadFPaymentBankAddEdit(string transaction, int paymentcount, string lastdate, double amount, double debt, string currency, int interest, double monthly_amount, int customer_id, int contract_id, double payment_amount, string customername, string contractcode, string startdate, string paymentid, double debtpenalty, string lclearingdate)
        {
            decimal insuranceDebt = InsuranceDebtValue.Value;

            if (transaction == "EDIT")
                insuranceDebt = (decimal)GlobalFunctions.GetAmount($@"SELECT ROUND (I.INSURANCE_AMOUNT * I.INSURANCE_INTEREST / 100, 2)
                                                                               - NVL(IP.PAYED_AMOUNT, 0)
                                                                                  DEBT
                                                                          FROM (  SELECT INSURANCE_ID, SUM (PAYED_AMOUNT) PAYED_AMOUNT
                                                                                    FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                                                                                   WHERE CUSTOMER_PAYMENT_ID != {paymentid}
                                                                                GROUP BY INSURANCE_ID) IP,
                                                                               CRS_USER.INSURANCES I
                                                                         WHERE I.ID = IP.INSURANCE_ID(+) AND I.CONTRACT_ID = {contract_id}");

            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FPaymentBankAddEdit fpae = new FPaymentBankAddEdit();
            fpae.TransactionName = transaction;
            fpae.PaymentCount = paymentcount;
            fpae.LastDate = lastdate;
            fpae.LastClearingDate = lclearingdate;
            fpae.Amount = amount;
            fpae.Debt = debt;
            fpae.Currency = currency;
            fpae.CurrencyID = CurrencyID;
            fpae.Interest = interest;
            fpae.MonthlyAmount = monthly_amount;
            fpae.CustomerID = customer_id;
            fpae.ContractID = contract_id;
            fpae.PaymentAmount = payment_amount;
            fpae.PaymentInterestDebt = payment_interest_debt;
            fpae.CustomerName = customername;
            fpae.ContractCode = contractcode;
            fpae.ContractStartDate = startdate;
            fpae.PaymentID = paymentid;
            fpae.DebtPenalty = debtpenalty;
            fpae.CommitmentName = CommitmentName;
            fpae.CustomerTypeID = CustomerTypeID;
            fpae.InsuranceAmount = insuranceDebt;
            fpae.InsuranceID = insuranceID;
            fpae.Clearing = clearing;
            fpae.PayDay = payDay;
            fpae.RefreshPaymentsDataGridView += new FPaymentBankAddEdit.DoEvent(RefreshPayments);
            fpae.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void PaymentsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PaymentsGridView.GetFocusedDataRow();
            if (row != null)
            {
                paymentid = row["ID"].ToString();
                paymentdate = row["PAYMENT_DATE"].ToString().Substring(0, 10);
                clearingdate = row["CLEARING_DATE"].ToString().Substring(0, 10);
                bank_cash = row["BANK_CASH"].ToString();
                debt = Convert.ToDouble(row["DEBT"].ToString());
                payment_amount = Convert.ToDouble(row["PAYMENT_AMOUNT"]);
                if (!String.IsNullOrEmpty(row["CLEARING"].ToString()))
                    clearing = Convert.ToDouble(row["CLEARING"]) > 0 ? false : true;
                else
                    clearing = false;

                if (bank_cash == "B")
                {
                    BankToCashBarButton.Enabled = (GlobalVariables.BankToCash && !CurrentStatus);
                    CashToBankBarButton.Enabled = false;
                }
                else
                {
                    BankToCashBarButton.Enabled = false;
                    CashToBankBarButton.Enabled = (GlobalVariables.CashToBank && !CurrentStatus);
                }
            }
        }

        private void FPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            TruncatePaymentsTemp();
            GlobalProcedures.DeleteAllFilesInDirectory(ContractImagePath);
        }

        private void TruncatePaymentsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_PAYMENTS_TEMP_TRUNCATE", "P_CONTRACT_ID", ContractID, "Ödənişə aid olan məlumatlar temp cədvəllərdən silinmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
            InsertPayments();
            InsertOperation();
            if (Math.Round(total, 2) <= 0 && PaymentsGridView.RowCount > 0)
            {
                UpdateContractStatus();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowWarningMessage(ContractCode + " saylı Lizinq Müqaviləsi üzrə ödənişlər tam başa çatdığı üçün bu müqavilə sistem tərəfindən avtomatik olaraq bağlanıldı.");
            }

            Close();
            RefreshTotalsDataGridView();
            GlobalProcedures.SplashScreenClose();
        }

        private void InsertOperation()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_PAY_TO_OPERATION", "P_CONTRACT_ID", int.Parse(ContractID), "Əməliyyatlar jurnalın əsas cədvəlinə daxil edilmədi.");
        }

        private void LoadPaymentsToList()
        {
            string sql = $@"SELECT TO_CHAR(CP.PAYMENT_DATE, 'DD.MM.YYYY') PAYMENT_DATE,
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
                                     ROW_NUMBER () OVER (ORDER BY CP.PAYMENT_DATE, CP.ID) ROW_NUM
                                FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP
                               WHERE CP.IS_CHANGE IN (0, 1) AND CP.CONTRACT_ID = {ContractID}
                            ORDER BY CP.PAYMENT_DATE, CP.ID";

            PaymentsViewDAL.RemoveAllPayments();

            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                PaymentsViewDAL.InsertPayment(int.Parse(dr["ROW_NUM"].ToString()),
                                                 dr["PAYMENT_DATE"].ToString(),
                                                 double.Parse(dr["CURRENCY_RATE"].ToString()),
                                                 double.Parse(dr["PAYMENT_AMOUNT_AZN"].ToString()),
                                                 double.Parse(dr["PAYMENT_AMOUNT"].ToString()),
                                                 double.Parse(dr["BASIC_AMOUNT"].ToString()),
                                                 double.Parse(dr["DEBT"].ToString()),
                                                 int.Parse(dr["DAY_COUNT"].ToString()),
                                                 double.Parse(dr["INTEREST_AMOUNT"].ToString()),
                                                 double.Parse(dr["PAYMENT_INTEREST_AMOUNT"].ToString()),
                                                 double.Parse(dr["PAYMENT_INTEREST_DEBT"].ToString()),
                                                 double.Parse(dr["TOTAL"].ToString()));
            }
        }

        void Print(bool isPdf = true)
        {
            //GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            //LoadPaymentsToList();
            //double sum_basic_amount = PaymentsViewDAL.lstPayments.Sum(p => p.BasicAmount),
            //        sum_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.InterestAmount),
            //        sum_payment_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentInterestAmount);

            //Reports.PaymentsReportUSD report = new Reports.PaymentsReportUSD();
            //report.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //report.ShowPrintMarginsWarning = false;
            //report.RequestParameters = false;

            //report.DataSource = PaymentsViewDAL.lstPayments;
            //report.Parameters["PContractCode"].Value = ContractCodeText.Text;
            //report.Parameters["PPeriod"].Value = PeriodText.Text;
            //report.Parameters["PInterest"].Value = InterestText.Text;
            //report.Parameters["PStartDate"].Value = StartDateText.Text;
            //report.Parameters["PEndDate"].Value = EndDateText.Text;
            //report.Parameters["PCustomer"].Value = CustomerNameText.Text;
            //report.Parameters["PCommitment"].Value = CommitmentNameValue.Text;
            //report.Parameters["PCommitmentLabel"].Value = !(String.IsNullOrEmpty(CommitmentNameValue.Text)) ? "Öhdəlik götürən:" : null;
            //report.Parameters["PCommitmentLabel"].Visible = !(String.IsNullOrEmpty(CommitmentNameValue.Text));
            //report.Parameters["PObject"].Value = ObjectText.Text;
            //report.Parameters["PContractAmountWithText"].Value = Amount.ToString("N2") + " " + Currency + ((Currency == "AZN") ? null : "   (" + rate + ")");
            //report.Parameters["PDebt"].Value = Amount - sum_basic_amount;
            //report.Parameters["PPaymentInterestDebt"].Value = sum_interest_amount - sum_payment_interest_amount;
            //report.Parameters["PTotal"].Value = (Amount - sum_basic_amount) + (sum_interest_amount - sum_payment_interest_amount);
            //GlobalProcedures.SplashScreenClose();
            //new ReportPrintTool(report).ShowPreview();
            //report.PrintingSystem.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.Parameters, new object[] { true });

            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

            LoadPaymentsToList();
            if (PaymentsViewDAL.lstPayments.Count == 0)
                return;

            double sum_basic_amount = PaymentsViewDAL.lstPayments.Sum(p => p.BasicAmount),
                    sum_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.InterestAmount),
                    sum_payment_interest_amount = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentInterestAmount),
                    sum_payment_amount = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentAmount),
                    sum_payment_amount_azn = PaymentsViewDAL.lstPayments.Sum(p => p.PaymentAmountAZN),
                    debt = PaymentsViewDAL.lstPayments.LastOrDefault().Debt,
                    lastInterestDebt = PaymentsViewDAL.lstPayments.LastOrDefault().PaymentInterestDebt,
                    one_day_interest = 0,
                    interestAmount = 0,
                    sum_insurance_amount = 0;

            DateTime lastPayDate = GlobalFunctions.ChangeStringToDate(PaymentsViewDAL.lstPayments.LastOrDefault().PaymentDate, "ddmmyyyy");
            int dayCount = 0;

            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Ödənişlərin siyahısı.docx");
            if (!File.Exists(fileName.ToString()))
            {
                XtraMessageBox.Show("Ödənişlərin siyahısının şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }
            int code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Ödənişlər.docx",
                   pdfFilePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Ödənişlər.pdf";


            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            object saveAs = Path.Combine(filePath);
            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;

            aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            aDoc.Activate();

            try
            {
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$startdate]", StartDateText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$enddate]", EndDateText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$period]", PeriodText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$percent]", InterestText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$object]", ObjectText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$commitmentlabel]", !(String.IsNullOrEmpty(CommitmentNameValue.Text)) ? "Öhdəlik götürən:" : null);
                GlobalProcedures.FindAndReplace(wordApp, "[$commitment]", !(String.IsNullOrEmpty(CommitmentNameValue.Text)) ? CommitmentNameValue.Text : null);
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", Amount.ToString("N2") + " " + Currency + ((Currency == "AZN") ? null : "   (" + rate + ")"));

                Microsoft.Office.Interop.Word.Table table1 = aDoc.Tables[2];
                int i = 2;
                foreach (var item in PaymentsViewDAL.lstPayments)
                {
                    table1.Rows.Add(table1.Rows[i]);

                    table1.Cell(i, 1).Range.Text = item.SS.ToString();
                    table1.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 2).Range.Text = item.PaymentDate;
                    table1.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 3).Range.Text = item.CurrencyRate.ToString("#.###0");
                    table1.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 4).Range.Text = item.PaymentAmountAZN.ToString("N2");
                    table1.Cell(i, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 5).Range.Text = item.PaymentAmount.ToString("N2");
                    table1.Cell(i, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 6).Range.Text = item.BasicAmount.ToString("N2");
                    table1.Cell(i, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 7).Range.Text = item.Debt.ToString("N2");
                    table1.Cell(i, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 8).Range.Text = item.DayCount.ToString();
                    table1.Cell(i, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table1.Cell(i, 9).Range.Text = item.InterestAmount.ToString("N2");
                    table1.Cell(i, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 10).Range.Text = item.PaymentInterestAmount.ToString("N2");
                    table1.Cell(i, 10).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 11).Range.Text = item.PaymentInterestDebt.ToString("N2");
                    table1.Cell(i, 11).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    table1.Cell(i, 12).Range.Text = item.Total.ToString("N2");
                    table1.Cell(i, 12).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    i++;
                }
                dayCount = GlobalFunctions.Days360(lastPayDate, DateTime.Today);
                one_day_interest = Math.Round((debt * Interest / 100) / 360, 2);
                interestAmount = one_day_interest * dayCount;

                table1.Cell(i, 1).Range.Text = (i - 1).ToString();
                table1.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                table1.Cell(i, 2).Range.Text = DateTime.Today.ToString("dd.MM.yyyy");
                table1.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                table1.Cell(i, 7).Range.Text = debt.ToString("N2");
                table1.Cell(i, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i, 8).Range.Text = dayCount.ToString();
                table1.Cell(i, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                table1.Cell(i, 9).Range.Text = interestAmount.ToString("N2");
                table1.Cell(i, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i, 11).Range.Text = (lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i, 11).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i, 12).Range.Text = (debt + lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i, 12).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 4).Range.Text = sum_payment_amount_azn.ToString("N2");
                table1.Cell(i + 1, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 5).Range.Text = sum_payment_amount.ToString("N2");
                table1.Cell(i + 1, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 6).Range.Text = sum_basic_amount.ToString("N2");
                table1.Cell(i + 1, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 7).Range.Text = debt.ToString("N2");
                table1.Cell(i + 1, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 9).Range.Text = (interestAmount + sum_interest_amount).ToString("N2");
                table1.Cell(i + 1, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 10).Range.Text = sum_payment_interest_amount.ToString("N2");
                table1.Cell(i + 1, 10).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 11).Range.Text = (lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i + 1, 11).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                table1.Cell(i + 1, 12).Range.Text = (debt + lastInterestDebt + interestAmount).ToString("N2");
                table1.Cell(i + 1, 12).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                Microsoft.Office.Interop.Word.Table table2 = aDoc.Tables[3];

                string sql = $@"SELECT ID,
                                   TO_CHAR(PAY_DATE,'DD.MM.YYYY') PAY_DATE,
                                   PAYED_AMOUNT,                                   
                                   NOTE,
                                   IS_LEGAL,
                                   CUSTOMER_PAYMENT_ID
                              FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                             WHERE IS_CHANGE != 2 AND INSURANCE_ID = {insuranceID}
                            ORDER BY PAY_DATE, ID";

                DataTable dtInsurance = GlobalFunctions.GenerateDataTable(sql, this.Name + "/PdfBarButton_ItemClick", "Sığortanın ödənişləri açılmadı.");

                if (dtInsurance.Rows.Count == 0)
                {
                    table2.Delete();
                    GlobalProcedures.FindAndReplace(wordApp, "[$insurancelabel]", null);
                }
                else
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$insurancelabel]", "Sığorta ödənişləri");
                    i = 2;
                    for (int j = 0; j < dtInsurance.Rows.Count; j++)
                    {
                        table2.Rows.Add(table2.Rows[i]);

                        table2.Cell(i, 1).Range.Text = (i - 1).ToString();
                        table2.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        table2.Cell(i, 2).Range.Text = dtInsurance.Rows[j]["PAY_DATE"].ToString();
                        table2.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        sum_insurance_amount = sum_insurance_amount + Convert.ToDouble(dtInsurance.Rows[j]["PAYED_AMOUNT"]);

                        table2.Cell(i, 3).Range.Text = Convert.ToDecimal(dtInsurance.Rows[j]["PAYED_AMOUNT"]).ToString("N2");
                        table2.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                        i++;
                    }

                    table2.Rows[i].Delete();

                    table2.Cell(i, 3).Range.Text = sum_insurance_amount.ToString("N2");
                    table2.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                }

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                if (isPdf)
                {
                    Word2Pdf objWorPdf = new Word2Pdf();
                    object FromLocation = filePath;
                    object ToLocation = pdfFilePath;
                    objWorPdf.InputLocation = FromLocation;
                    objWorPdf.OutputLocation = ToLocation;
                    objWorPdf.Word2PdfCOnversion();

                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    GlobalProcedures.SplashScreenClose();
                    Process.Start(pdfFilePath);
                }
                else
                {
                    GlobalProcedures.SplashScreenClose();
                    Process.Start(filePath);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(code_number + "_Ödənişlər.pdf faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.", exx);
            }
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bank_cash == "B")
                LoadFPaymentBankAddEdit("EDIT", PaymentsGridView.RowCount, paymentdate, Amount, debt, Currency, (extendInterest != 0 ? extendInterest : Interest), monthly_amount, int.Parse(CustomerID), int.Parse(ContractID), 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, clearingdate);
            else
                LoadFPaymentAddEdit("EDIT", PaymentsGridView.RowCount, paymentdate, Amount, debt, Currency, (extendInterest != 0 ? extendInterest : Interest), monthly_amount, CustomerID, ContractID, payment_amount, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, bank_cash);
        }

        private void PaymentsGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditBarButton.Enabled) && (StandaloneBarDockControl.Enabled))
            {
                if (bank_cash == "B")
                    LoadFPaymentBankAddEdit("EDIT", PaymentsGridView.RowCount, paymentdate, Amount, debt, Currency, Interest, monthly_amount, int.Parse(CustomerID), int.Parse(ContractID), 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, clearingdate);
                else
                    LoadFPaymentAddEdit("EDIT", PaymentsGridView.RowCount, paymentdate, Amount, debt, Currency, Interest, monthly_amount, CustomerID, ContractID, payment_amount, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, bank_cash);
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int insuranceCheck = Convert.ToInt16(GlobalFunctions.GridGetRowCellValue(PaymentsGridView, "INSURANCE_CHECK"));
            decimal insuranceAmount = Convert.ToDecimal(GlobalFunctions.GridGetRowCellValue(PaymentsGridView, "INSURANCE_AMOUNT"));

            string message = null, p = null;
            if (bank_cash == "C")
            {
                if (insuranceCheck == 0)
                    message = paymentdate + " tarixinə olan ödənişi silmək istəyirsiniz? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və kassa qalığı yenidən hesablanacaq.";
                else
                    message = paymentdate + $@" tarixinə olan ödənişin içində <b><color=104,0,0>{insuranceAmount.ToString("N2")} AZN sığorta ödənişi</color></b> mövcuddur.Bu ödənişi silsəz sığorta ödənişidə silinəcək. Buna razısınız? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və kassa qalığı yenidən hesablanacaq.";
            }
            else
            {
                if (insuranceCheck == 0)
                    message = paymentdate + " tarixinə olan ödənişi silmək istəyirsiniz? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və bank əməliyyatlarının qalığı yenidən hesablanacaq.";
                else
                    message = paymentdate + $@" tarixinə olan ödənişin içində <b><color=104,0,0>{insuranceAmount.ToString("N2")} AZN sığorta ödənişi</color></b> mövcuddur.Bu ödənişi silsəz sığorta ödənişidə silinəcək. Buna razısınız? Əgər bu ödənişi silib yadda saxlasaz, bu zaman portfel və bank əməliyyatlarının qalığı yenidən hesablanacaq.";
            }

            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show(message, "Ödənişin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
                PaymentDelete();
                GlobalProcedures.UpdateCustomerPaymentChange(1, int.Parse(ContractID), Convert.ToInt32(paymentid));
                GlobalProcedures.UpdateBalancePenalty(1, ContractID, paymentdate);
                GlobalProcedures.SplashScreenClose();
            }
            RefreshPayments(a, p);
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void PaymentDelete()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_PAYMENTS_TEMP_DELETE", "P_CONTRACT_ID", int.Parse(ContractID), "P_ID", int.Parse(paymentid), "Ödənişə aid olan məlumatlar temp cədvəllərdən silinmədi.");
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string p = null;
            RefreshPayments(a, p);
        }

        private void PaymentScheduleLabel_Click(object sender, EventArgs e)
        {
            FPaymentSchedules fps = new FPaymentSchedules();
            fps.ContractID = ContractID;
            fps.Amount = Amount.ToString("N2") + " " + Currency;
            fps.ContractCode = ContractCodeText.Text;
            fps.ShowDialog();
        }

        private void NewCashBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindLastDateAndDebt();
            LoadFPaymentAddEdit("INSERT", PaymentsGridView.RowCount, ldate, Amount, debt, Currency, (extendInterest != 0 ? extendInterest : Interest), monthly_amount, CustomerID, ContractID, 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, "C");
        }

        private void NewBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FindLastDateAndDebt();
            LoadFPaymentBankAddEdit("INSERT", PaymentsGridView.RowCount, ldate, Amount, debt, Currency, (extendInterest != 0 ? extendInterest : Interest), monthly_amount, int.Parse(CustomerID), int.Parse(ContractID), 0, CustomerNameText.Text.Trim(), ContractCode, StartDateText.Text.Trim(), paymentid, debt_penalty, lclearingdate);
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(DateTime.Today.ToString("d", Class.GlobalVariables.V_CultureInfoAZ));
        }

        void RefreshPenaltiesAndPercents()
        {
            //CalculatedPenalties();
            CalculatedPercents();
        }

        private void LoadFPenalty()
        {
            FPenalty fp = new FPenalty();
            fp.ContractCode = ContractCodeText.Text;
            fp.ContractID = ContractID;
            fp.CustomerID = CustomerID;
            fp.Currency = Currency;
            fp.RefreshPenaltyValue += new FPenalty.DoEvent(RefreshPenaltiesAndPercents);
            fp.ShowDialog();
        }

        private void BPenalty_Click(object sender, EventArgs e)
        {
            LoadFPenalty();
        }

        private void BankToCashBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FBankToCash fbc = new FBankToCash();
            fbc.ContractCode = ContractCode;
            fbc.CustomerName = CustomerName;
            fbc.PaymentID = paymentid;
            fbc.RefreshPaymentsDataGridView += new FBankToCash.DoEvent(RefreshPayments);
            fbc.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        private void CashToBankBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = PaymentsGridView.TopRowIndex;
            old_row_num = PaymentsGridView.FocusedRowHandle;
            FCashToBank fbc = new FCashToBank();
            fbc.ContractCode = ContractCode;
            fbc.CustomerName = CustomerName;
            fbc.PaymentID = paymentid;
            fbc.RefreshPaymentsDataGridView += new FCashToBank.DoEvent(RefreshPayments);
            fbc.ShowDialog();
            PaymentsGridView.TopRowIndex = topindex;
            PaymentsGridView.FocusedRowHandle = old_row_num;
        }

        public static decimal OverduePercent(int conractID, string clearingDate, int id)
        {
            decimal overdue = 0;

            using (OracleConnection connection = new OracleConnection())
            {                
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    
                    command = connection.CreateCommand();
                    command.CommandText = "CRS_USER.PROC_OVERDUE_PERCENT_BY_ID";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_CONTRACT_ID", GlobalFunctions.ConvertObjectToOracleDBType(conractID)).Value = conractID;
                    command.Parameters.Add("P_CLEARING_DATE", GlobalFunctions.ConvertObjectToOracleDBType(clearingDate)).Value = clearingDate;
                    command.Parameters.Add("P_LAST_PAYMENT_ID", GlobalFunctions.ConvertObjectToOracleDBType(id)).Value = id;
                    command.Parameters.Add("OVERDUE_PERCENT", OracleDbType.Decimal, ParameterDirection.Output);
                    
                    command.ExecuteNonQuery();

                    overdue = Convert.ToDecimal(command.Parameters["OVERDUE_PERCENT"].Value.ToString(), GlobalVariables.V_CultureInfoEN);
                }
                catch (Exception exx)
                {
                    //LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }

            return overdue;
        }
    }
}