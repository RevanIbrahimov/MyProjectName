using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Microsoft.Reporting.WebForms;
using DevExpress.Utils;
using Oracle.ManagedDataAccess.Client;
using CRS.Class.Tables;
using CRS.Class;
using CRS.Class.DataAccess;

namespace CRS.Forms.Total
{
    public partial class FPaymentBankAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPaymentBankAddEdit()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName,
            LastDate,
            LastClearingDate,
            Currency,
            PaymentID,
            CustomerName,
            ContractCode,
            ContractStartDate,
            BankName,
            BranchName,
            AccountName,
            PDate,
            CommitmentName;

        public int PaymentCount = 0,
            Interest,
            CustomerID = 0,
            ContractID = 0,
            CustomerTypeID,
            InsuranceID,
            PayDay,
            CurrencyID;

        public decimal InsuranceAmount = 0;
        public bool Clearing = true;
        public double Amount,
            Debt,
            MonthlyAmount,
            PaymentAmount,
            PaymentInterestDebt = 0,
            DebtPenalty = 0;

        double one_day_interest = 0,
            interest_amount = 0,
            debt = 0,
            residual_percent = 0,
            normal_debt = 0,
            cur,
            totaldebtamount = 0,
            requiredamount = 0,
            penalty_amount = 0,
            diff_interest_amount = 0,
            changed_interest_amount = 0,
            startPercent = 0,
            sumPaymentAmount = 0;

        int diff_day,
            order_id,
            currency_id,
            bank_id = 0,
            accounting_plan_id = 0,
            is_penalty = 0,
            is_changed_interest = 0;

        private void DebtPenaltyCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            if (DebtPenaltyCheck.Checked)
                DebtPenaltyValue.Value = CalcDebtPenalty();
            else
                DebtPenaltyValue.Value = 0;

            LoadRequiredAmount();
        }

        private void PayedPenaltyValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            if (!PayedPenaltyValue.EditorContainsFocus)
                return;

            DebtPenaltyValue.Value = CalcDebtPenalty();

            LoadRequiredAmount();
        }

        private decimal CalcDebtPenalty()
        {
            decimal debtPenaty = DebtPenaltyCheck.Checked ? (PenaltyValue.Value + PaymentPenaltyDebtValue.Value) - PayedPenaltyValue.Value : 0;
            return debtPenaty < 0 ? 0 : debtPenaty;
        }

        private void PenaltyCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (PenaltyCheck.Checked)
            {
                PayedPenaltyValue.TabStop = true;
                PayedPenaltyValue.Focus();
                is_penalty = 1;
                PayedPenaltyValue.ReadOnly = false;
            }
            else
            {
                PayedPenaltyValue.TabStop = false;
                is_penalty = 0;
                PayedPenaltyValue.EditValue = PenaltyValue.Value + PaymentPenaltyDebtValue.Value;
                PayedPenaltyValue.ReadOnly = true;
                if (FormStatus)
                {
                    DebtPenaltyValue.Value = CalcDebtPenalty();
                    LoadRequiredAmount();
                }
            }
        }

        private void LoadRequiredAmount()
        {
            if (!FormStatus)
                return;

            //teleb olunan mebleg
            //residual_percent = Math.Round(interest_amount + diff_interest_amount, 2);
            residual_percent = Math.Round(interest_amount + diff_interest_amount, 2);
            PaymentInterestDebtValue.Value = (decimal)Math.Round(diff_interest_amount, 2);

            List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(ContractID).ToList<PaymentSchedules>();
            var schedules = lstSchedules.Where(s => s.MONTH_DATE <= ClearingDate.DateTime).ToList<PaymentSchedules>();
            if (schedules.Count == 0)
                order_id = 0;
            else
                order_id = schedules.Max(s => s.ORDER_ID);

            if (order_id == 0)
            {
                if (TransactionName != "BINSERT")
                    normal_debt = Debt;
                else
                    normal_debt = debt;
            }
            else
                normal_debt = Math.Round(lstSchedules.Find(s => s.ORDER_ID == order_id).DEBT, 2); //qrafik uzre odenis   

            if (TransactionName == "BINSERT")
                totaldebtamount = Math.Round((debt + residual_percent + (double)PayedPenaltyValue.Value + (double)DebtPenaltyValue.Value), 2);
            else
                totaldebtamount = Math.Round((Debt + residual_percent + (double)PayedPenaltyValue.Value + (double)DebtPenaltyValue.Value), 2);

            //qaliq borc
            TotalDebtValue.Value = (decimal)totaldebtamount;

            //tam baglamaq ucun lazim olan mebleg
            if (TransactionName == "BINSERT")
            {
                if ((debt - normal_debt) > 0)
                    //requiredamount = (debt - normal_debt) + residual_percent;
                    requiredamount = (debt - normal_debt) + (double)OverduePercentValue.Value;
                else
                    //requiredamount = residual_percent;
                    requiredamount = (double)OverduePercentValue.Value;
            }
            else
            {
                if ((Debt - normal_debt) > 0)
                    //requiredamount = (Debt - normal_debt) + residual_percent;
                    requiredamount = (Debt - normal_debt) + (double)OverduePercentValue.Value;
                else
                    //requiredamount = residual_percent;
                    requiredamount = (double)OverduePercentValue.Value;
            }

            RequiredValue.Value = (decimal)requiredamount; // + PayedPenaltyValue.Value + DebtPenaltyValue.Value;
        }

        private void ChangedInterestCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ChangedInterestCheck.Checked)
            {
                is_changed_interest = 1;
                PayedPercentValue.Focus();
                changed_interest_amount = (double)PayedPercentValue.Value;
                PayedPercentValue.Properties.ReadOnly = false;
                PayedPercentValue.TabStop = true;
            }
            else
            {
                is_changed_interest = 0;
                PayedPercentValue.EditValue = TransactionName == "INSERT" ? PaymentInterestDebtValue.Value : (decimal)interest_amount + PaymentInterestDebtValue.Value;
                changed_interest_amount = 0;
                PayedPercentValue.Properties.ReadOnly = true;
                PayedPercentValue.TabStop = false;
            }
        }

        DateTime lastClearingDate;

        private void ClearingCheck_CheckedChanged(object sender, EventArgs e)
        {
            ClearingDate.ReadOnly = !ClearingCheck.Checked;

            if (FormStatus)
                ClearingDate.EditValue = ClearingDate.Properties.MinValue = PaymentDate.DateTime < lastClearingDate ? lastClearingDate.AddDays(1) :
                                                                                            ClearingCheck.Checked ? PaymentDate.DateTime.AddDays(1) : PaymentDate.DateTime;

            ClearingDate.TabStop = ClearingCheck.Checked;
        }

        private void ClearingDate_EditValueChanged(object sender, EventArgs e)
        {
            if (LastDateText.Text.Length == 0)
                return;

            OverduePercentValue.EditValue = FPayment.OverduePercent(ContractID, ClearingDate.Text, int.Parse(PaymentID));

            DateTime lastPayDate = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
            diff_day = GlobalFunctions.Days360(lastPayDate, ClearingDate.DateTime);
            DayCountText.Text = diff_day.ToString();
            interest_amount = (diff_day * one_day_interest) + startPercent;

            //teleb olunan mebleg
            residual_percent = Math.Round(interest_amount + diff_interest_amount, 2);
            PaymentInterestDebtValue.Value = (decimal)Math.Round(diff_interest_amount, 2);
            PayedPercentValue.EditValue = ChangedInterestCheck.Checked? PayedPercentValue.EditValue : (decimal)residual_percent;

            List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(ContractID).ToList<PaymentSchedules>();
            var schedules = lstSchedules.Where(s => s.MONTH_DATE <= ClearingDate.DateTime).ToList<PaymentSchedules>();
            if (schedules.Count == 0)
                order_id = 0;
            else
                order_id = schedules.Max(s => s.ORDER_ID);

            if (order_id == 0)
            {
                if (TransactionName != "BINSERT")
                    normal_debt = Debt;
                else
                    normal_debt = debt;
            }
            else
                normal_debt = Math.Round(lstSchedules.Find(s => s.ORDER_ID == order_id).DEBT, 2); //qrafik uzre odenis


            DateTime payDate = lastPayDate;
            if (sumPaymentAmount == 0)
            {
                int lastDateMonth = lastPayDate.Month + 1,
                        lastDateYear = lastPayDate.Year;

                if (lastDateMonth > 12)
                {
                    lastDateMonth = 1;
                    lastDateYear = lastDateYear + 1;
                }

                payDate = (PayDay == 31 || PayDay == 29) ? new DateTime(lastDateYear, lastDateMonth,
                                                                    DateTime.DaysInMonth(lastDateYear, lastDateMonth)) : new DateTime(lastDateYear, lastDateMonth, PayDay);
            }

            var schedulesPenalty = lstSchedules.Where(s => s.REAL_DATE < ClearingDate.DateTime).ToList<PaymentSchedules>();
            if (schedulesPenalty.Count == 0)
                order_id = 0;
            else
                order_id = schedulesPenalty.Max(s => s.ORDER_ID);

            int penaltyDayCount = GlobalFunctions.Days360(payDate, ClearingDate.DateTime);
            double interestPenalty = 0;
            //cerime faizi
            double debtPaymentAmount = order_id * MonthlyAmount - sumPaymentAmount,
                   penaltyAmount = 0;
            if (debtPaymentAmount > 0 && penaltyDayCount > 0)
            {
                DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT * FROM CRS_USER.CONTRACT_INTEREST_PENALTIES WHERE CONTRACT_ID = {ContractID} ORDER BY ID");
                if (dt.Rows.Count == 1)
                {
                    DataView dv = new DataView();
                    dv = new DataView(dt);
                    interestPenalty = Convert.ToDouble(dt.Compute("Max(INTEREST)", dv.RowFilter));
                    penaltyAmount = Math.Round(debtPaymentAmount * interestPenalty / 100, 2) * penaltyDayCount;
                }
            }
            PenaltyValue.EditValue = penaltyAmount;
            PayedPenaltyValue.EditValue = PenaltyCheck.Checked ? PayedPenaltyValue.Value : (decimal)(penaltyAmount + DebtPenalty);
            DebtPenaltyValue.Value = CalcDebtPenalty();

            //tam baglamaq ucun lazim olan mebleg
            if (TransactionName == "BINSERT")
            {
                if ((debt - normal_debt) > 0)
                    //requiredamount = (debt - normal_debt) + residual_percent;
                    requiredamount = (debt - normal_debt) + (double)OverduePercentValue.Value;
                else
                    //requiredamount = residual_percent;
                    requiredamount = (double)OverduePercentValue.Value;
            }
            else
            {
                if ((Debt - normal_debt) > 0)
                    //requiredamount = (Debt - normal_debt) + residual_percent;
                    requiredamount = (Debt - normal_debt) + (double)OverduePercentValue.Value;
                else
                    //requiredamount = residual_percent;
                    requiredamount = (double)OverduePercentValue.Value; ;
            }

            if (TransactionName == "BINSERT")
                totaldebtamount = Math.Round((debt + residual_percent + (double)(PayedPenaltyValue.Value + DebtPenaltyValue.Value)), 2);
            else
                totaldebtamount = Math.Round((Debt + residual_percent + (double)(PayedPenaltyValue.Value + DebtPenaltyValue.Value)), 2);

            TotalDebtValue.Value = (decimal)totaldebtamount;

            RequiredValue.Value = (decimal)requiredamount;
            PaymentValue_EditValueChanged(sender, EventArgs.Empty);
            PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void InsurancePayedAmount_EditValueChanged(object sender, EventArgs e)
        {
            if (InsurancePayedAmount.EditorContainsFocus)
                CalcLeasingPayAmount();
        }

        private void InsurancePayedCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (InsurancePayedCheck.Checked)
            {
                if (FormStatus)
                    InsurancePayedAmount.EditValue = InsuranceAmount;
                InsurancePayedAmount.Focus();
                InsurancePayedAmount.ReadOnly = false;
            }
            else
            {
                InsurancePayedAmount.ReadOnly = true;
                if (InsuranceAmount == 0)
                    InsurancePayedAmount.EditValue = InsuranceAmount;
            }

            CalcLeasingPayAmount();
        }

        private void RequiredValue_EditValueChanged(object sender, EventArgs e)
        {
            if (MonthlyPaymentValue.Value != 0)
                TAValue.EditValue = RequiredValue.Value / MonthlyPaymentValue.Value;
        }

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);

            GlobalProcedures.FillLookUpEdit(OperationAccountLookUp, "ACCOUNTING_PLAN", "ID", "SUB_ACCOUNT", "BANK_ID = " + bank_id + " ORDER BY  ID,ACCOUNT_NUMBER");
            OperationAccountLookUp.ItemIndex = 0;
        }

        private void OperationAccountLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Bookkeeping.FAccountingPlan fap = new Bookkeeping.FAccountingPlan();
                fap.RefreshAccountList += new Bookkeeping.FAccountingPlan.DoEvent(RefreshOperationsAccount);
                fap.ShowDialog();
            }
        }

        private void OperationAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (OperationAccountLookUp.EditValue == null)
                return;

            accounting_plan_id = Convert.ToInt32(OperationAccountLookUp.EditValue);
        }

        bool FormStatus = false;
        string ldate;


        public delegate void DoEvent(decimal a, string p);
        public event DoEvent RefreshPaymentsDataGridView;

        private void FPaymentBank_Load(object sender, EventArgs e)
        {
            CurrencyRateValue.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            CurrencyRateValue.Properties.DisplayFormat.FormatString = "### ##0.0000";
            lastClearingDate = GlobalFunctions.ChangeStringToDate(LastClearingDate, "ddmmyyyy");
            RefreshDictionaries(11);
            BankLookUp.ItemIndex = 0;
            InsurancePayedCheck.ReadOnly = InsuranceAmount == 0;
            InsurancePayedCheck.Checked = InsuranceAmount > 0;
            startPercent = GlobalFunctions.ContractStartPercent(ContractID);
            //diff_interest_amount = GlobalFunctions.GetAmount($@"SELECT ABS(NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0)) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}");
            CurrencyLabel.Text = PayedPenaltyCurrencyLabel.Text = DebtPenaltyCurrencyLabel.Text = PayedPercentCurrencyLabel.Text = Currency;


            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                currency_id = CurrencyID;
                if (PaymentCount == 0)
                    LastDateLabel.Visible = LastDateText.Visible = false;
                else
                {
                    LastDateLabel.Visible = LastDateText.Visible = true;
                    LastDateLabel.Text = "Ən son silinmə tarixi";
                }
                PenaltyValue.Text = DebtPenalty.ToString("N2");

                PaymentID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_PAYMENT_SEQUENCE").ToString();
                ContractCodeText.Text = ContractCode;
                ContractStartDateText.Text = ContractStartDate;
                CustomerNameText.Text = CustomerName;

                if (String.IsNullOrEmpty(CommitmentName))
                    NameText.Text = CustomerName;
                else
                    NameText.Text = CommitmentName;

                DataTable dtPayment = GlobalFunctions.GenerateDataTable($@"SELECT NVL(INTEREST_AMOUNT, 0) INTEREST_AMOUNT, NVL(PAYMENT_INTEREST_AMOUNT, 0) PAYMENT_INTEREST_AMOUNT, NVL(PAYMENT_AMOUNT, 0) PAYMENT_AMOUNT, TO_CHAR(PAYMENT_DATE,'DD.MM.YYYY') PAYMENT_DATE FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}");
                if (dtPayment.Rows.Count > 0)
                {
                    DataView dv = new DataView();
                    dv = new DataView(dtPayment);
                    object sumInterestAmountObject = dtPayment.Compute("Sum(INTEREST_AMOUNT)", dv.RowFilter),
                           sumPaymentInterestAmountObject = dtPayment.Compute("Sum(PAYMENT_INTEREST_AMOUNT)", dv.RowFilter),
                           sumPaymentAmountObject = dtPayment.Compute("Sum(PAYMENT_AMOUNT)", dv.RowFilter);

                    diff_interest_amount = Math.Abs(Convert.ToDouble(sumInterestAmountObject) - Convert.ToDouble(sumPaymentInterestAmountObject));
                    sumPaymentAmount = Convert.ToDouble(sumPaymentAmountObject);
                }

                LastDateText.Text = LastClearingDate;

                DebtValue.Value = (decimal)Debt;
                InterestText.Text = Interest.ToString();
                one_day_interest = Math.Round(((Debt * Interest) / 100) / 360, 2);
                OneDayInterestValue.Value = (decimal)one_day_interest;
                PaymentDate.EditValue = DateTime.Today;
                MonthlyPaymentValue.Value = (decimal)MonthlyAmount;
                PaymentPenaltyDebtValue.EditValue = DebtPenalty;
                DebtPenaltyCheck.ReadOnly = !GlobalVariables.CheckPenaltyDebt;
            }
            else if (TransactionName == "EDIT")
            {
                ClearingCheck.ReadOnly = PenaltyCheck.ReadOnly = !Clearing;
                if (GlobalVariables.CheckPenaltyDebt)
                    DebtPenaltyCheck.ReadOnly = !Clearing;
                else
                    DebtPenaltyCheck.ReadOnly = false;
                ClearingDate.Enabled = PayedPenaltyValue.Enabled = Clearing;
                InsertBankOperationsTemp();
                LoadPaymentDetails();
            }
            else
            {
                ContractCodeText.TabIndex = 0;
                ContractCodeText.TabStop = true;
                PaymentID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_PAYMENT_SEQUENCE").ToString();
                BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(BankName);
                BankLookUp.Enabled = false;
                PaymentDate.Enabled = false;
                ContractCodeText.Properties.ReadOnly = false;
            }

            if (currency_id == 1)
            {
                PaymentAZNLabel.Visible = PaymentAZNValue.Visible = false;
                PaymentLabel.Text = "Ödənişin məbləği";
            }
            else
            {
                PaymentAZNLabel.Visible = PaymentAZNValue.Visible = true;
                PaymentLabel.Text = "Ödənişin məbləği (" + Currency + " - ilə)";
            }

            if (TransactionName != "BINSERT")
            {
                PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDate, "ddmmyyyy");
                DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT MIN(PAYMENT_DATE) PDATE, COUNT(*) RCOUNT FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND ID > {PaymentID}");
                if (Convert.ToInt16(dt.Rows[0]["RCOUNT"]) > 0)
                    PaymentDate.Properties.MaxValue = Convert.ToDateTime(dt.Rows[0]["PDATE"]);
                PaymentDate_EditValueChanged(sender, EventArgs.Empty);
            }
            InsurancePayedCheck_CheckedChanged(sender, EventArgs.Empty);
            FormStatus = true;
        }

        private void InsertBankOperationsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_BO_PAYMENT_TEMP", "P_PAYMENT_ID", PaymentID, "Ödəniş temp cədvələ daxil edilmədi.");
        }

        private void LoadPaymentDetails()
        {
            double leasing_amount = 0;
            string s = null;
            try
            {
                s = $@"SELECT C.CONTRACT_CODE,
                                   TO_CHAR (C.START_DATE, 'DD.MM.YYYY') S_DATE,
                                   CUS.CUSTOMER_NAME,
                                   CP.PAYMENT_NAME,
                                   CP.PAYMENT_DATE P_DATE,
                                   CP.DAY_COUNT,
                                   C.INTEREST,
                                   CP.REQUIRED_CLOSE_AMOUNT,
                                   CP.ONE_DAY_INTEREST_AMOUNT,
                                   CP.INTEREST_AMOUNT,
                                   NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, C.MONTHLY_AMOUNT)
                                            MONTHLY_AMOUNT,
                                   CP.REQUIRED_AMOUNT,
                                   CP.PAYED_AMOUNT,
                                   CP.PAYED_AMOUNT_AZN,
                                   C.CURRENCY_ID,
                                   C.CURRENCY_CODE,
                                   CP.NOTE,
                                   CP.CUSTOMER_ID,
                                   CP.CONTRACT_ID,
                                   C.AMOUNT,
                                   CP.CURRENCY_RATE,
                                   CP.PAYMENT_INTEREST_DEBT,
                                   NVL(LP.LAST_PAYMENT_INTEREST_DEBT, 0) LAST_PAYMENT_INTEREST_DEBT,
                                   CP.PENALTY_DEBT,
                                   NVL(LP.LAST_PENALTY_DEBT, 0) LAST_PENALTY_DEBT,
                                   CP.IS_PENALTY,
                                   CP.PENALTY_AMOUNT,
                                   CP.PAYED_PENALTY,
                                   BO.BANK_NAME,                                   
                                   LP.LAST_DATE,
                                   LP.LAST_CLEARING_DATE,
                                   LP.LAST_DEBT,
                                   CP.INSURANCE_CHECK,
                                   CP.INSURANCE_AMOUNT,
                                   CP.CONTRACT_PERCENT,
                                   CP.CLEARING_DATE,
                                   CP.IS_CLEARING,
                                   CP.IS_CHANGED_INTEREST,
                                   CP.CHANGED_PAY_INTEREST_AMOUNT,
                                   CP.IS_PENALTY_DEBT,
                                   CP.OVERDUE_PERCENT
                              FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP,
                                   CRS_USER.V_CONTRACTS C,       
                                   CRS_USER.V_CUSTOMERS CUS,
                                   CRS_USER_TEMP.V_BANK_OPERATION_TEMP BO,
                                   CRS_USER_TEMP.V_CONTRACT_LAST_PAYMENT_TEMP LP,
                                   CRS_USER.V_LAST_CONTRACT_EXTEND CE
                             WHERE     C.CUSTOMER_ID = CUS.ID
                                   AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND CP.CONTRACT_ID = C.CONTRACT_ID    
                                   AND CP.CONTRACT_ID = CE.CONTRACT_ID(+)
                                   AND CP.ID = BO.CONTRACT_PAYMENT_ID  
                                   AND CP.ID = LP.ID    
                                   AND CP.IS_CHANGE != 2
                                   AND BO.USED_USER_ID = {GlobalVariables.V_UserID}    
                                   AND CP.ID = {PaymentID}";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    ContractCodeText.Text = dr["CONTRACT_CODE"].ToString();
                    ContractStartDateText.Text = dr["S_DATE"].ToString();
                    CustomerNameText.Text = dr["CUSTOMER_NAME"].ToString();
                    NameText.Text = dr["PAYMENT_NAME"].ToString();
                    PaymentDate.EditValue = DateTime.Parse(dr["P_DATE"].ToString());
                    PaymentDate.Enabled = false;
                    ClearingDate.EditValue = DateTime.Parse(dr["CLEARING_DATE"].ToString());
                    ldate = ClearingDate.Text;
                    ClearingCheck.Checked = Convert.ToInt16(dr["IS_CLEARING"]) == 1 ? true : false;
                    OneDayInterestValue.Value = Convert.ToDecimal(dr["ONE_DAY_INTEREST_AMOUNT"].ToString());
                    DayCountText.Text = dr["DAY_COUNT"].ToString();
                    diff_day = Convert.ToInt32(dr["DAY_COUNT"].ToString());
                    Interest = Convert.ToInt32(dr["CONTRACT_PERCENT"]);
                    InterestText.Text = Interest.ToString();
                    TotalDebtValue.Value = Convert.ToDecimal(dr["REQUIRED_CLOSE_AMOUNT"].ToString());
                    interest_amount = Convert.ToDouble(dr["INTEREST_AMOUNT"].ToString());
                    MonthlyPaymentValue.Value = Convert.ToDecimal(dr["MONTHLY_AMOUNT"].ToString());
                    RequiredValue.Value = Convert.ToDecimal(dr["REQUIRED_AMOUNT"].ToString());
                    if (dr["CURRENCY_CODE"].ToString() != "AZN")
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                        CurrencyRateLabel.Text = "1 " + dr["CURRENCY_CODE"] + " = ";
                        CurrencyRateValue.Value = Convert.ToDecimal(dr["CURRENCY_RATE"]);
                    }
                    PaymentValue.Value = Convert.ToDecimal(dr["PAYED_AMOUNT"]);
                    PaymentAZNValue.Value = Convert.ToDecimal(dr["PAYED_AMOUNT_AZN"]);
                    InsurancePayedCheck.Checked = Convert.ToInt16(dr["INSURANCE_CHECK"]) == 1 ? true : false;
                    InsurancePayedAmount.Value = Convert.ToDecimal(dr["INSURANCE_AMOUNT"]);
                    CurrencyLabel.Text = dr["CURRENCY_CODE"].ToString();
                    currency_id = Convert.ToInt32(dr["CURRENCY_ID"].ToString());
                    NoteText.Text = dr["NOTE"].ToString();
                    CustomerID = Convert.ToInt32(dr["CUSTOMER_ID"].ToString());
                    ContractID = Convert.ToInt32(dr["CONTRACT_ID"].ToString());
                    leasing_amount = Convert.ToDouble(dr["AMOUNT"].ToString());
                    PaymentInterestDebtValue.EditValue = Convert.ToDecimal(dr["LAST_PAYMENT_INTEREST_DEBT"]);
                    PaymentPenaltyDebtValue.EditValue = Convert.ToDecimal(dr["LAST_PENALTY_DEBT"]);
                    PenaltyValue.EditValue = Convert.ToDecimal(dr["PENALTY_AMOUNT"]);
                    PenaltyCheck.Checked = (Convert.ToInt32(dr["IS_PENALTY"].ToString()) == 1);
                    PayedPenaltyValue.EditValue = Math.Abs(Convert.ToDecimal(dr["PAYED_PENALTY"].ToString()));
                    DebtPenaltyCheck.Checked = (Convert.ToInt32(dr["IS_PENALTY_DEBT"].ToString()) == 1);
                    DebtPenaltyValue.EditValue = Math.Abs(Convert.ToDecimal(dr["PENALTY_DEBT"].ToString()));
                    BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                    is_changed_interest = Convert.ToInt32(dr["IS_CHANGED_INTEREST"]);
                    ChangedInterestCheck.Checked = is_changed_interest == 1 ? true : false;
                    PayedPercentValue.EditValue = is_changed_interest == 1 ? Convert.ToDecimal(dr["CHANGED_PAY_INTEREST_AMOUNT"]) : (decimal)interest_amount + PaymentInterestDebtValue.Value;
                    if (String.IsNullOrWhiteSpace(dr["LAST_CLEARING_DATE"].ToString()))
                    {
                        LastDateText.Visible = LastDateLabel.Visible = false;
                        LastDateText.Text = ContractStartDateText.Text;
                        Debt = leasing_amount;
                        DebtValue.Value = (decimal)Debt;
                    }
                    else
                    {
                        LastDateText.Visible = LastDateLabel.Visible = true;
                        LastDateText.Text = dr["LAST_CLEARING_DATE"].ToString().Substring(0, 10);
                        Debt = Convert.ToDouble(dr["LAST_DEBT"]);
                        DebtValue.Value = (decimal)Debt;
                    }
                    lastClearingDate = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
                    ClearingDate.Properties.MinValue = PaymentDate.DateTime < lastClearingDate ? lastClearingDate.AddDays(1) :
                                                                                            ClearingCheck.Checked ? PaymentDate.DateTime.AddDays(1) : PaymentDate.DateTime;
                    OverduePercentValue.EditValue = Convert.ToDecimal(dr["OVERDUE_PERCENT"]);
                }


                one_day_interest = Math.Round(((Debt * Interest) / 100) / 360, 2);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CalcLeasingPayAmount()
        {
            LeasingPayedAmount.EditValue = PaymentAZNValue.Value - (InsurancePayedCheck.Checked ? InsurancePayedAmount.Value : 0);
        }

        private void PaymentValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!PaymentValue.EditorContainsFocus)
                return;

            PaymentAZNValue.Value = (decimal)((double)PaymentValue.Value * (double)CurrencyRateValue.Value);
            if (cur != (double)CurrencyRateValue.Value)
                CurrencyRateValue.BackColor = Color.GreenYellow;
            else
                CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();

            CalcLeasingPayAmount();
        }

        private void PaymentAZNValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!PaymentAZNValue.EditorContainsFocus)
                return;

            if (CurrencyRateValue.Value != 0)
                PaymentValue.Value = (decimal)((double)PaymentAZNValue.Value / (double)CurrencyRateValue.Value);
            else
                PaymentValue.Value = 0;

            CalcLeasingPayAmount();
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                PaymentCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private bool ControlPaymentDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(ContractCodeText.Text))
            {
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müqavilənin nömrəsi daxil edilməyib");
                ContractCodeText.Focus();
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CustomerID == 0 || ContractID == 0)
            {
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text + " saylı lizinq müqaviləsinə uyğun məlumatlar tapılmadı. Zəhmət olmasa müqavilələrin siyahısına baxın.");
                ContractCodeText.Focus();
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(NameText.Text))
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödəyənin tam adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(PaymentDate.Text))
            {
                PaymentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödəniş tarixi seçilməyib.");
                PaymentDate.Focus();
                PaymentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(ClearingDate.Text))
            {
                ClearingDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Silinmə tarixi seçilməyib.");
                ClearingDate.Focus();
                ClearingDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank seçilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (OperationAccountLookUp.EditValue == null)
            {
                OperationAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Daxili mühasibatlıq hesab daxil edilməyib.");
                OperationAccountLookUp.Focus();
                OperationAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PaymentValue.Value <= 0)
            {
                PaymentValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödənişin məbləği sıfırdan böyük olmalıdır.");
                PaymentValue.Focus();
                PaymentValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            decimal calcpercent = (decimal)interest_amount + PaymentInterestDebtValue.Value;
            if (TransactionName == "INSERT" && PayedPercentValue.Value > calcpercent)
            {
                PayedPercentValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödəniləcək faiz " + ClearingDate.Text + " tarixinə hesablanmış faizdən (" + calcpercent.ToString(GlobalVariables.V_CultureInfoEN) + ") böyük ola bilməz.");
                PaymentValue.Focus();
                PayedPercentValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (Math.Round(PaymentValue.Value, 2) > TotalDebtValue.Value)
            {
                PaymentValue.BackColor = Color.Red;
                TotalDebtValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödənişin məbləği 'Tam bağlamaq üçün tələb olunan məbləğ' - dən çox ola bilməz.");
                PaymentValue.Focus();
                PaymentValue.BackColor = GlobalFunctions.ElementColor();
                TotalDebtValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (Math.Round(PaymentValue.Value, 2) == TotalDebtValue.Value && PayedPercentValue.Value < calcpercent)
            {
                PayedPercentValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Borcu tam ödəyən zaman ödəniləcək faiz " + ClearingDate.Text + " tarixinə hesablanmış faizdən (" + calcpercent.ToString(GlobalVariables.V_CultureInfoEN) + ") kiçik ola bilməz.");
                PayedPercentValue.Focus();
                PayedPercentValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PaymentAZNValue.Value <= 0)
            {
                CurrencyRateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(PaymentDate.Text + " tarixi üçün " + CurrencyLabel.Text + " valyutasının AZN qarşılığı daxil edilməyib.");
                CurrencyRateValue.Focus();
                CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (InsurancePayedCheck.Checked && InsurancePayedAmount.Value > InsuranceAmount)
            {
                InsurancePayedAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage($@"Sığorta ödənişi maksimum {InsuranceAmount.ToString("N2")} AZN ola bilər.");
                InsurancePayedAmount.Focus();
                InsurancePayedAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LeasingPayedAmount.Value <= 0)
            {
                LeasingPayedAmount.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinqə ödəniləcək məbləğ sıfırdan böyük olmalıdır.");
                PaymentValue.Focus();
                LeasingPayedAmount.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertPayment()
        {
            double payment_interest_amount = 0, currenct_payment_interest_debt = 0, basic_amount = 0, current_debt = 0, total = 0, payment_value_AZN = 0, cdebt, penalty = 0, totalpayedinterest = 0;
            int is_penalty_debt = DebtPenaltyCheck.Checked ? 1 : 0;
            if (TransactionName == "INSERT")
                cdebt = Debt;
            else
                cdebt = debt;

            decimal paymentValue = Math.Round(CurrencyRateValue.Value > 0 ? LeasingPayedAmount.Value / CurrencyRateValue.Value : 0, 2);

            if (paymentValue > 0) // eger odenis sifirdan boyuk olarsa
            {
                totalpayedinterest = is_changed_interest == 0 ? interest_amount + PaymentInterestDebt : changed_interest_amount = (double)PayedPercentValue.Value;

                if (totalpayedinterest > (double)paymentValue) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                    payment_interest_amount = (double)paymentValue;
                else
                    payment_interest_amount = totalpayedinterest; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
            }

            if ((double)paymentValue < (double)PayedPenaltyValue.Value)
            {
                basic_amount = 0;
                penalty_amount = (double)paymentValue;
                payment_interest_amount = 0;
            }
            else
            {
                penalty_amount = (double)PayedPenaltyValue.Value;
                penalty = (double)paymentValue - penalty_amount;
                if (penalty < payment_interest_amount)
                {
                    payment_interest_amount = penalty;
                    basic_amount = 0;
                }
                else
                    basic_amount = penalty - payment_interest_amount;
            }

            currenct_payment_interest_debt = PaymentInterestDebt + interest_amount - payment_interest_amount;
            current_debt = cdebt - basic_amount;
            total = current_debt + currenct_payment_interest_debt;

            payment_value_AZN = (double)LeasingPayedAmount.Value;

            int clearingCalc = 1;

            if (ClearingCheck.Checked && ClearingDate.DateTime > DateTime.Today)
                clearingCalc = 0;

            if (TransactionName == "INSERT" && InsurancePayedCheck.Checked && InsurancePayedAmount.Value > 0)
                GlobalProcedures.ExecuteThreeQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,
                                                                                                            CUSTOMER_ID,
                                                                                                            CONTRACT_ID,
                                                                                                            PAYMENT_DATE,
                                                                                                            PAYMENT_AMOUNT,
                                                                                                            BASIC_AMOUNT,
                                                                                                            DEBT,
                                                                                                            DAY_COUNT,
                                                                                                            ONE_DAY_INTEREST_AMOUNT,
                                                                                                            INTEREST_AMOUNT,
                                                                                                            PAYMENT_INTEREST_AMOUNT,
                                                                                                            PAYMENT_INTEREST_DEBT,
                                                                                                            TOTAL,
                                                                                                            CURRENCY_RATE,
                                                                                                            PAYMENT_AMOUNT_AZN,
                                                                                                            REQUIRED_CLOSE_AMOUNT,
                                                                                                            REQUIRED_AMOUNT,
                                                                                                            PAYMENT_NAME,
                                                                                                            NOTE,
                                                                                                            USED_USER_ID,
                                                                                                            IS_CHANGE,
                                                                                                            BANK_CASH,
                                                                                                            PENALTY_DEBT,
                                                                                                            IS_PENALTY,
                                                                                                            PENALTY_AMOUNT,
                                                                                                            PAYED_PENALTY,
                                                                                                            BANK_ID,
                                                                                                            CUSTOMER_TYPE_ID,
                                                                                                            PAYED_AMOUNT,
                                                                                                            PAYED_AMOUNT_AZN,
                                                                                                            INSURANCE_CHECK,
                                                                                                            INSURANCE_AMOUNT,
                                                                                                            CONTRACT_PERCENT,
                                                                                                            CLEARING_DATE,
                                                                                                            IS_CLEARING,
                                                                                                            CLEARING_CALCULATED,
                                                                                                            CLEARING_CALCULATED,
                                                                                                            IS_CHANGED_INTEREST,
                                                                                                            CHANGED_PAY_INTEREST_AMOUNT,
                                                                                                            IS_PENALTY_DEBT,
                                                                                                            OVERDUE_PERCENT)
                                                    VALUES({PaymentID},
                                                            {CustomerID},
                                                            {ContractID},
                                                            TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                            {paymentValue.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {diff_day},
                                                            {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {requiredamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            '{NameText.Text.Trim()}',
                                                            '{NoteText.Text.Trim()}',
                                                            {GlobalVariables.V_UserID},
                                                            1,
                                                            'B',
                                                            {Math.Round(DebtPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {is_penalty},
                                                            {Math.Round(PenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(PayedPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {bank_id},
                                                            {CustomerTypeID},
                                                            {Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {(InsurancePayedCheck.Checked ? 1 : 0)},
                                                            {(InsurancePayedCheck.Checked ? InsurancePayedAmount.Value.ToString(GlobalVariables.V_CultureInfoEN) : "0")},
                                                            {Interest.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            TO_DATE('{ClearingDate.Text}','DD/MM/YYYY'),
                                                            {(ClearingCheck.Checked ? 1 : 0)},
                                                            {clearingCalc},
                                                            {is_changed_interest},
                                                            {Math.Round(changed_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {is_penalty_debt},
                                                            {Math.Round(OverduePercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)})",
                                                 "INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,IS_CHANGE,USED_USER_ID,CONTRACT_PAYMENT_ID,CONTRACT_CODE,ACCOUNTING_PLAN_ID) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + bank_id + ",TO_DATE('" + PaymentDate.Text.Trim() + "','DD/MM/YYYY'),3," + Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,0,'" + NoteText.Text.Trim() + "',1," + GlobalVariables.V_UserID + "," + PaymentID + ",'" + ContractCodeText.Text.Trim() + "'," + accounting_plan_id + ")",
                                                 $@"INSERT INTO CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP (ID,
                                                                                                       INSURANCE_ID,
                                                                                                       PAY_DATE,
                                                                                                       PAYED_AMOUNT, 
                                                                                                       IS_LEGAL,
                                                                                                       CUSTOMER_PAYMENT_ID,
                                                                                                       IS_CHANGE,
                                                                                                       USED_USER_ID)
                                                                        VALUES(CRS_USER.INSURANCE_PAYMENT_SEQUENCE.NEXTVAL,
                                                                                {InsuranceID},
                                                                                TO_DATE('{PaymentDate.Text}','DD.MM.YYYY'),
                                                                                {Math.Round(InsurancePayedAmount.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                                                          
                                                                                1,
                                                                                {PaymentID},
                                                                                1,
                                                                                {GlobalVariables.V_UserID})",
                                                    "Ödəniş temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertPayment");
            else if (TransactionName == "INSERT" && ((InsurancePayedCheck.Checked && InsurancePayedAmount.Value == 0) || !InsurancePayedCheck.Checked))
                GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,
                                                                                                    CUSTOMER_ID,
                                                                                                    CONTRACT_ID,
                                                                                                    PAYMENT_DATE,
                                                                                                    PAYMENT_AMOUNT,
                                                                                                    BASIC_AMOUNT,
                                                                                                    DEBT,
                                                                                                    DAY_COUNT,
                                                                                                    ONE_DAY_INTEREST_AMOUNT,
                                                                                                    INTEREST_AMOUNT,
                                                                                                    PAYMENT_INTEREST_AMOUNT,
                                                                                                    PAYMENT_INTEREST_DEBT,
                                                                                                    TOTAL,
                                                                                                    CURRENCY_RATE,
                                                                                                    PAYMENT_AMOUNT_AZN,
                                                                                                    REQUIRED_CLOSE_AMOUNT,
                                                                                                    REQUIRED_AMOUNT,
                                                                                                    PAYMENT_NAME,
                                                                                                    NOTE,
                                                                                                    USED_USER_ID,
                                                                                                    IS_CHANGE,
                                                                                                    BANK_CASH,
                                                                                                    PENALTY_DEBT,
                                                                                                    IS_PENALTY,
                                                                                                    PENALTY_AMOUNT,
                                                                                                    PAYED_PENALTY,
                                                                                                    BANK_ID,
                                                                                                    CUSTOMER_TYPE_ID,
                                                                                                    PAYED_AMOUNT,
                                                                                                    PAYED_AMOUNT_AZN,
                                                                                                    INSURANCE_CHECK,
                                                                                                    INSURANCE_AMOUNT,
                                                                                                    CONTRACT_PERCENT,
                                                                                                    CLEARING_DATE,
                                                                                                    IS_CLEARING,
                                                                                                    CLEARING_CALCULATED,
                                                                                                    IS_CHANGED_INTEREST,
                                                                                                    CHANGED_PAY_INTEREST_AMOUNT,
                                                                                                    IS_PENALTY_DEBT,
                                                                                                    OVERDUE_PERCENT)
                                                        VALUES({PaymentID},
                                                        {CustomerID},
                                                        {ContractID},
                                                        TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                        {paymentValue.ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {diff_day},
                                                        {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {requiredamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                        '{NameText.Text.Trim()}',
                                                        '{NoteText.Text.Trim()}',
                                                        {GlobalVariables.V_UserID},
                                                        1,
                                                        'B',
                                                        {Math.Round(DebtPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {is_penalty},
                                                        {Math.Round(PenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(PayedPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {bank_id},
                                                        {CustomerTypeID},
                                                        {Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {(InsurancePayedCheck.Checked ? 1 : 0)},
                                                        {(InsurancePayedCheck.Checked ? InsurancePayedAmount.Value.ToString(GlobalVariables.V_CultureInfoEN) : "0")},
                                                        {Interest.ToString(GlobalVariables.V_CultureInfoEN)},
                                                        TO_DATE('{ClearingDate.Text}','DD/MM/YYYY'),
                                                        {(ClearingCheck.Checked ? 1 : 0)},
                                                        {clearingCalc},
                                                        {is_changed_interest},
                                                        {Math.Round(changed_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {is_penalty_debt},
                                                        {Math.Round(OverduePercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)})",
                                                 "INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,IS_CHANGE,USED_USER_ID,CONTRACT_PAYMENT_ID,CONTRACT_CODE,ACCOUNTING_PLAN_ID) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + bank_id + ",TO_DATE('" + PaymentDate.Text.Trim() + "','DD/MM/YYYY'),3," + Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,0,'" + NoteText.Text.Trim() + "',1," + GlobalVariables.V_UserID + "," + PaymentID + ",'" + ContractCodeText.Text.Trim() + "'," + accounting_plan_id + ")",
                                                    "Ödəniş temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertPayment");
            else
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,CUSTOMER_ID,CONTRACT_ID,PAYMENT_DATE,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,REQUIRED_AMOUNT,PAYMENT_NAME,NOTE,USED_USER_ID,IS_CHANGE,BANK_CASH,PENALTY_DEBT,IS_PENALTY,PENALTY_AMOUNT,PAYED_PENALTY,BANK_ID,CUSTOMER_TYPE_ID,PAYED_AMOUNT,PAYED_AMOUNT_AZN,INSURANCE_CHECK,INSURANCE_AMOUNT,CONTRACT_PERCENT,CLEARING_DATE,IS_CLEARING,CLEARING_CALCULATED,IS_CHANGED_INTEREST,CHANGED_PAY_INTEREST_AMOUNT,IS_PENALTY_DEBT)VALUES(" + PaymentID + "," + CustomerID + "," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY')," + paymentValue.ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(current_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + diff_day + "," + Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + requiredamount.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + NameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + Class.GlobalVariables.V_UserID + ",1,'B'," + Math.Round(DebtPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + is_penalty + "," + Math.Round(PenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(PayedPenaltyValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + bank_id + "," + CustomerTypeID + "," + Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + (InsurancePayedCheck.Checked ? 1 : 0) + "," + (InsurancePayedCheck.Checked ? InsurancePayedAmount.Value.ToString(GlobalVariables.V_CultureInfoEN) : "0") + "," + Interest.ToString(GlobalVariables.V_CultureInfoEN) + ",TO_DATE('" + ClearingDate.Text + "','DD/MM/YYYY')," + (ClearingCheck.Checked ? 1 : 0) + "," + clearingCalc + "," + is_changed_interest + "," + Math.Round(changed_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + is_penalty_debt + ")",
                                                    "Ödəniş temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertPayment");
            GlobalProcedures.InsertOperationJournal(PaymentDate.Text, (double)CurrencyRateValue.Value, currency_id, (double)paymentValue, payment_value_AZN, basic_amount, payment_interest_amount, ContractID.ToString(), PaymentID, OperationAccountLookUp.Text, 1, ClearingDate.Text, PayedPenaltyValue.Value);
        }

        private void InsertBalancePenalty()
        {
            if (PenaltyCheck.Checked)
            {
                double currentdebtpenalty = DebtPenalty - penalty_amount;
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.BALANCE_PENALTIES_TEMP(ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_CHANGE,IS_COMMIT,PENALTY_STATUS,CUSTOMER_PAYMENT_ID,USED_USER_ID) VALUES(BALANCE_PENALTIES_SEQUENCE.NEXTVAL," + CustomerID + "," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY'),0,0," + currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN) + "," + penalty_amount.ToString(GlobalVariables.V_CultureInfoEN) + ",1,1,'Ödənilib'," + PaymentID + "," + GlobalVariables.V_UserID + ")",
                                                    "Cərimə temp cədvələ daxil edilmədi.",
                                            this.Name + "/InsertBalancePenalty");
            }
        }

        private void PaymentDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            if ((String.IsNullOrEmpty(PaymentDate.Text)) || (String.IsNullOrEmpty(LastDateText.Text)))
                return;

            if (!ClearingCheck.Checked && PaymentDate.DateTime <= lastClearingDate)
                ClearingCheck.Checked = ClearingCheck.ReadOnly = true;
            else if (!ClearingCheck.Checked)
            {
                ClearingDate.EditValue = PaymentDate.DateTime;
                ClearingCheck.ReadOnly = false;
            }
            else if (ClearingCheck.Checked)
            {
                ClearingCheck.ReadOnly = PaymentDate.DateTime <= lastClearingDate;
                ClearingCheck_CheckedChanged(sender, EventArgs.Empty);
            }

            if (currency_id != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + Currency + " = ";
                CurrencyRateValue.Value = (decimal)cur;
            }
            else if (currency_id == 1)
                cur = 1;
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(PaymentDate.Text);
            if (currency_id != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + Currency + " = ";
                CurrencyRateValue.Value = (decimal)cur;
                PaymentValue_EditValueChanged(sender, EventArgs.Empty);
                PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FPaymentBankAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshPaymentsDataGridView(PaymentAZNValue.Value, PaymentID);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPaymentDetails())
            {
                if (TransactionName == "INSERT" || TransactionName == "BINSERT")
                {
                    InsertPayment();
                }
                else
                {
                    GlobalProcedures.ShowWarningMessage("Əgər " + ldate + " tarixinə olan ödənişi dəyişib yadda saxlasaz, bu zaman portfel və bank əməliyyatlarının qalığı yenidən hesablanacaq.");
                    changed_interest_amount = is_changed_interest == 1 ? (double)PayedPercentValue.Value : 0;
                    GlobalProcedures.UpdateCustomerPayment(1,
                                                                PaymentDate.Text,
                                                                ClearingDate.Text,
                                                                ContractID,
                                                                (double)Math.Round(PaymentValue.Value, 2),
                                                                changed_interest_amount,
                                                                (double)Math.Round(PaymentAZNValue.Value, 2),
                                                                (double)CurrencyRateValue.Value,
                                                                NameText.Text.Trim(),
                                                                Convert.ToInt32(DayCountText.Text.Trim()),
                                                                (double)OneDayInterestValue.Value,
                                                                Math.Round(interest_amount, 2),
                                                                (double)TotalDebtValue.Value,
                                                                (double)RequiredValue.Value,
                                                                Convert.ToInt32(PaymentID),
                                                                (double)PayedPenaltyValue.Value,
                                                                is_penalty,
                                                                (double)DebtPenaltyValue.Value,
                                                                (double)PenaltyValue.Value,
                                                                OperationAccountLookUp.Text,
                                                                NoteText.Text.Trim(),
                                                                (InsurancePayedCheck.Checked ? 1 : 0),
                                                                (InsurancePayedCheck.Checked ? InsurancePayedAmount.Value : 0),
                                                                is_changed_interest);

                    GlobalProcedures.ExecuteTwoQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP SET BANK_ID = {bank_id} WHERE ID = {PaymentID}",
                                                     $@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET IS_CHANGE = 1,BANK_ID = {bank_id},ACCOUNTING_PLAN_ID = {accounting_plan_id},INCOME = {PaymentAZNValue.Value.ToString(GlobalVariables.V_CultureInfoEN)} WHERE CONTRACT_PAYMENT_ID = {PaymentID} AND CONTRACT_CODE = '{ContractCodeText.Text.Trim()}'",
                                                            "Bank əməliyyatı dəyişdirilmədi.",
                                                       this.Name + "/BOK_Click");

                }
                if (ReceiptCheck.Checked)
                    GlobalProcedures.LoadBankReceipt(PaymentDate.Text, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, currency_id, (decimal)CurrencyRateValue.Value, Currency, NameText.Text.Trim(), ContractID, ContractCodeText.Text.Trim(), "saylı lizinq müqaviləsi üzrə ödəniş", NameText.Text.Trim());
                this.Close();
            }
        }

        //private void InsertBankOperation()
        //{
        //    double payment_value_AZN = 0;
        //    if (currency_id == 1)
        //        payment_value_AZN = (double)PaymentValue.Value;
        //    else
        //        payment_value_AZN = (double)PaymentAZNValue.Value;
        //    GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,IS_CHANGE,USED_USER_ID,CONTRACT_PAYMENT_ID,CONTRACT_CODE,ACCOUNTING_PLAN_ID) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + bank_id + ",TO_DATE('" + PaymentDate.Text.Trim() + "','DD/MM/YYYY'),3," + Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,0,'" + NoteText.Text.Trim() + "',1," + GlobalVariables.V_UserID + "," + PaymentID + ",'" + ContractCodeText.Text.Trim() + "'," + accounting_plan_id + ")",
        //                                  "Linq ödənişi bank əməliyyatlarının mədaxilinə daxil olunmadı.");
        //}

        private void ContractCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus && TransactionName == "BINSERT")
            {
                double amount = 0;
                int interest = 24;

                if (ContractCodeText.Text.Length != 4)
                    return;

                string sql = $@"SELECT C.CONTRACT_ID,
                                       C.CUSTOMER_ID,
                                       TO_CHAR (C.START_DATE, 'DD.MM.YYYY') START_DATE,
                                       CUS.CUSTOMER_NAME,
                                       C.CURRENCY_ID,
                                       C.AMOUNT,
                                       CUR.CODE CURRENCY_CODE,
                                       C.INTEREST,
                                       C.MONTHLY_AMOUNT
                                  FROM CRS_USER.V_CONTRACTS C,       
                                       CRS_USER.V_CUSTOMERS CUS,
                                       CRS_USER.CURRENCY CUR
                                 WHERE     C.USED_USER_ID = -1
                                       AND C.STATUS_ID = 5
                                       AND CUR.ID = C.CURRENCY_ID       
                                       AND C.CUSTOMER_ID = CUS.ID
                                       AND C.CONTRACT_CODE = '{ContractCodeText.Text.Trim()}'";
                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/ContractCodeText_EditValueChanged");

                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ContractID = Convert.ToInt32(dr["CONTRACT_ID"].ToString());
                            CustomerID = Convert.ToInt32(dr["CUSTOMER_ID"].ToString());
                            ContractStartDateText.Text = dr["START_DATE"].ToString();
                            CustomerNameText.Text = dr["CUSTOMER_NAME"].ToString();
                            currency_id = Convert.ToInt32(dr["CURRENCY_ID"].ToString());
                            amount = Convert.ToDouble(dr["AMOUNT"].ToString());
                            Currency = dr["CURRENCY_CODE"].ToString();
                            interest = Convert.ToInt32(dr["INTEREST"].ToString());
                            MonthlyPaymentValue.Value = Convert.ToDecimal(dr["MONTHLY_AMOUNT"].ToString());
                        }

                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");

                        int isTemp = 0;

                        if (PaymentsDAL.PaymentTempIsEmpty(ContractID).HasValue && PaymentsDAL.PaymentTempIsEmpty(ContractID).Value == true)
                            isTemp = 1;


                        List<Payments> lstPayments = PaymentsDAL.SelectPayments(isTemp, ContractID).ToList<Payments>();

                        if (lstPayments.Count == 0)
                        {
                            LastDateText.Visible = LastDateLabel.Visible = false;
                            LastDateText.Text = ContractStartDateText.Text;
                            debt = amount;
                            PaymentInterestDebt = 0;
                        }
                        else
                        {
                            LastDateText.Visible = LastDateLabel.Visible = true;
                            var lastPayments = lstPayments.Where(item => item.IS_CHANGE == 0 || item.IS_CHANGE == 1).LastOrDefault();

                            LastDateText.Text = lastPayments.PAYMENT_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
                            debt = Math.Round((double)lastPayments.DEBT, 2);
                            PaymentInterestDebt = Math.Round((double)lastPayments.PAYMENT_INTEREST_DEBT, 2);
                        }

                        PaymentInterestDebtValue.Value = (decimal)PaymentInterestDebt;
                        if (currency_id == 1)
                        {
                            PaymentAZNLabel.Visible = PaymentAZNValue.Visible = false;
                            this.MinimumSize = new Size(682, 707);
                            this.MaximumSize = new Size(682, 707);
                            PaymentLabel.Text = "Ödənişin məbləği";
                            PenaltyValue.Location = new Point(252, 514);
                            PayedPenaltyCurrencyLabel.Location = new Point(369, 517);
                            PenaltyCheck.Location = new Point(409, 514);
                            PayedPenaltyValue.Location = new Point(485, 514);
                            NoteText.Location = new Point(252, 540);
                            NoteLabel.Location = new Point(12, 543);
                            PaymentCharCountLabel.Location = new Point(252, 600);
                        }
                        else
                        {
                            PaymentAZNLabel.Visible = true;
                            PaymentAZNValue.Visible = true;
                            this.MinimumSize = new Size(682, 732);
                            this.MaximumSize = new Size(682, 732);
                            PenaltyValue.Location = new Point(252, 540);
                            PayedPenaltyCurrencyLabel.Location = new Point(369, 543);
                            NoteText.Location = new Point(252, 568);
                            NoteLabel.Location = new Point(12, 566);
                            PaymentCharCountLabel.Location = new Point(252, 622);
                            PaymentLabel.Text = "Ödənişin məbləği (" + Currency + " - ilə)";
                        }
                        CurrencyLabel.Text = Currency;
                        DebtValue.Value = (decimal)debt;
                        InterestText.Text = interest.ToString();
                        one_day_interest = Math.Round(((debt * interest) / 100) / 360, 2);
                        OneDayInterestValue.Value = (decimal)one_day_interest;

                        string commitment_name = GlobalFunctions.GetName($@"SELECT CC.COMMITMENT_NAME FROM CRS_USER.V_COMMITMENTS CC WHERE CC.CONTRACT_ID = {ContractID}");
                        if (String.IsNullOrEmpty(commitment_name))
                        {
                            CommitmentLabel.Visible = false;
                            NameText.Text = CustomerNameText.Text;
                        }
                        else
                        {
                            CommitmentLabel.Visible = true;
                            NameText.Text = commitment_name;
                        }
                        //DebtPenalty = GlobalFunctions.GetAmount("SELECT DEBT_PENALTY FROM CRS_USER.CONTRACT_BALANCE_PENALTIES BP WHERE BAL_DATE = (SELECT MAX(BAL_DATE) FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE IS_COMMIT = 1 AND CUSTOMER_ID = BP.CUSTOMER_ID AND CONTRACT_ID = BP.CONTRACT_ID) AND BP.CONTRACT_ID = " + ContractID);
                        //PenaltyText.Text = DebtPenalty.ToString("N2");
                        PayedPenaltyCurrencyLabel.Text = DebtPenaltyCurrencyLabel.Text = PayedPercentCurrencyLabel.Text = Currency;
                        PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
                        PaymentDate.EditValue = GlobalFunctions.ChangeStringToDate(PDate, "ddmmyyyy");
                    }
                    catch (Exception exx)
                    {
                        GlobalProcedures.LogWrite("Müqavilənin rekvizitləri tapılmadı.", sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                    }
                }
                else
                {
                    ContractStartDateText.Text =
                        CustomerNameText.Text =
                        LastDateText.Text =
                        NameText.Text =
                        InterestText.Text =
                        CurrencyLabel.Text =
                        DayCountText.Text =
                        PenaltyValue.Text = null;

                    DebtValue.Value =
                        PaymentInterestDebtValue.Value =
                        OneDayInterestValue.Value =
                        MonthlyPaymentValue.Value =
                        TotalDebtValue.Value =
                        RequiredValue.Value =
                        PaymentAZNValue.Value =
                        PaymentValue.Value =
                        PayedPenaltyValue.Value = 0;

                    CurrencyRateLabel.Visible =
                        CurrencyRateValue.Visible =
                        RateAZNLabel.Visible =
                        PenaltyCheck.Checked = false;

                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                    CustomerID = 0;
                    ContractID = 0;
                    PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate("01.01.1900", "ddmmyyyy");
                }
            }
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        void RefreshOperationsAccount()
        {
            GlobalProcedures.FillLookUpEdit(OperationAccountLookUp, "ACCOUNTING_PLAN", "ID", "SUB_ACCOUNT", "BANK_ID = " + bank_id + " ORDER BY  ID,ACCOUNT_NUMBER");
        }
    }
}