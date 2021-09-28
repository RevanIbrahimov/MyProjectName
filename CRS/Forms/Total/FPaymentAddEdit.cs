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
using System.IO;
using System.Diagnostics;
using Bytescout.Document;
using DevExpress.Utils;
using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;

namespace CRS.Forms.Total
{
    public partial class FPaymentAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPaymentAddEdit()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName,
            LastDate,
            Currency,
            CustomerID,
            ContractID,
            PaymentID,
            CustomerName,
            ContractCode,
            ContractStartDate,
            CommitmentName,
            PaymentType;

        public int PaymentCount = 0,
            Interest,
            CustomerTypeID;

        public double Amount,
            Debt,
            MonthlyAmount,
            PaymentAmount,
            DebtPenalty = 0,
            PaymentInterestDebt = 0;

        double one_day_interest = 0,
            interest_amount = 0,
            residual_percent = 0,
            normal_debt = 0,
            cur,
            totaldebtamount = 0,
            requiredamount = 0,
            penalty_amount = 0;

        private void OperationCheck_CheckedChanged(object sender, EventArgs e)
        {
            ClearingDate.ReadOnly = !ClearingCheck.Checked;
        }

        int diff_day,
            order_id,
            currency_id,
            is_penalty = 0;

        bool FormStatus = false;
        string ldate;

        public delegate void DoEvent(decimal a, string p);
        public event DoEvent RefreshPaymentsDataGridView;

        private void RequiredValue_EditValueChanged(object sender, EventArgs e)
        {
            TAValue.EditValue = RequiredValue.Value / MonthlyPaymentValue.Value;
        }        

        private void FPaymentAddEdit_Load(object sender, EventArgs e)
        {
            PaymentDate.Properties.MaxValue = DateTime.Today;

            if (PaymentType == "C")
                this.Text = "Kassa ödənişinin əlavə/düzəliş edilməsi";
            else
                this.Text = "Digər ödənişin əlavə/düzəliş edilməsi";
            CurrencyRateValue.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            CurrencyRateValue.Properties.DisplayFormat.FormatString = "### ##0.0000";

            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                currency_id = (int)GlobalFunctions.GetAmount("SELECT ID FROM CRS_USER.CURRENCY WHERE CODE = '" + Currency + "'");
                if (PaymentCount == 0)
                    LastDateLabel.Visible = LastDateText.Visible = false;
                else
                {
                    LastDateLabel.Visible = LastDateText.Visible = true;
                    LastDateLabel.Text = "Ən son ödənişin tarixi";
                }
                CalculatedPenalty.Value = (decimal)DebtPenalty;
                
                PaymentID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_PAYMENT_SEQUENCE").ToString();
                ContractCodeText.Text = ContractCode;
                ContractStartDateText.Text = ContractStartDate;
                CustomerNameText.Text = CustomerName;
                if (String.IsNullOrEmpty(CommitmentName))
                    NameText.Text = CustomerName;
                else
                    NameText.Text = CommitmentName;
                LastDateText.Text = LastDate;
                PaymentDate.EditValue = ClearingDate.EditValue = DateTime.Today;
                CurrencyLabel.Text = Currency;
                PenaltyCurrencyLabel.Text = Currency;
                DebtValue.Value = (decimal)Debt;
                InterestText.Text = Interest.ToString();
                one_day_interest = Math.Round(((Debt * Interest) / 100) / 360, 2);
                OneDayInterestValue.Value = (decimal)one_day_interest;
                MonthlyPaymentValue.Value = (decimal)MonthlyAmount;
            }
            else
                LoadPaymentDetails();

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

            PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT MIN(PAYMENT_DATE) PDATE, COUNT(*) RCOUNT FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND ID > {PaymentID}");
            if (Convert.ToInt16(dt.Rows[0]["RCOUNT"]) > 0)
                PaymentDate.Properties.MaxValue = Convert.ToDateTime(dt.Rows[0]["PDATE"]);
            PaymentDate_EditValueChanged(sender, EventArgs.Empty);
            FormStatus = true;
        }

        private void LoadPaymentDetails()
        {
            double leasing_amount = 0;
            string s = $@"SELECT C.CONTRACT_CODE,
                               TO_CHAR (C.START_DATE, 'DD.MM.YYYY') S_DATE,
                               CUS.CUSTOMER_NAME,
                               CP.PAYMENT_NAME,
                               CP.PAYMENT_DATE P_DATE,
                               CP.DAY_COUNT,
                               C.INTEREST,
                               CP.REQUIRED_CLOSE_AMOUNT,
                               CP.ONE_DAY_INTEREST_AMOUNT,
                               CP.INTEREST_AMOUNT,
                               C.MONTHLY_AMOUNT,
                               CP.REQUIRED_AMOUNT,
                               CP.PAYMENT_AMOUNT,
                               CP.PAYMENT_AMOUNT_AZN,
                               C.CURRENCY_ID,
                               C.CURRENCY_CODE,
                               CP.NOTE,
                               CP.CUSTOMER_ID,
                               CP.CONTRACT_ID,
                               C.AMOUNT,
                               CP.CURRENCY_RATE,
                               CP.PAYMENT_INTEREST_DEBT,
                               CP.PENALTY_DEBT,
                               CP.IS_PENALTY,
                               CP.PENALTY_AMOUNT,
                               LP.LAST_DATE,
                               LP.LAST_DEBT,
                               CP.IS_BASIC_AMOUNT,
                               CP.CLEARING_DATE,
                               CP.IS_CLEARING
                          FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP,
                               CRS_USER.V_CONTRACTS C,       
                               CRS_USER.V_CUSTOMERS CUS,
                               CRS_USER_TEMP.V_CONTRACT_LAST_PAYMENT_TEMP LP       
                         WHERE     C.CUSTOMER_ID = CUS.ID
                               AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                               AND CP.CONTRACT_ID = C.CONTRACT_ID  
                               AND CP.ID = LP.ID   
                               AND CP.IS_CHANGE != 2
                               AND CP.ID = {PaymentID}";
            try
            {
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
                    ClearingCheck.Checked = Convert.ToInt16(dr["IS_CLEARING"]) == 1 ? true : false;
                    ldate = PaymentDate.Text;
                    DayCountText.Text = dr["DAY_COUNT"].ToString();
                    diff_day = Convert.ToInt32(dr["DAY_COUNT"].ToString());
                    InterestText.Text = dr["INTEREST"].ToString();
                    Interest = Convert.ToInt32(dr["INTEREST"].ToString());
                    TotalDebtValue.Value = Convert.ToDecimal(dr["REQUIRED_CLOSE_AMOUNT"].ToString());
                    OneDayInterestValue.Value = Convert.ToDecimal(dr["ONE_DAY_INTEREST_AMOUNT"].ToString());
                    interest_amount = Convert.ToDouble(dr["INTEREST_AMOUNT"].ToString());
                    MonthlyPaymentValue.Value = Convert.ToDecimal(dr["MONTHLY_AMOUNT"].ToString());
                    RequiredValue.Value = Convert.ToDecimal(dr["REQUIRED_AMOUNT"].ToString());
                    if (dr["CURRENCY_CODE"].ToString() != "AZN")
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = true;
                        RateAZNLabel.Visible = true;
                        CurrencyRateLabel.Text = "1 " + dr["CURRENCY_CODE"] + " = ";
                        CurrencyRateValue.Value = Convert.ToDecimal(dr["CURRENCY_RATE"].ToString());
                    }
                    PaymentValue.Value = Convert.ToDecimal(dr["PAYMENT_AMOUNT"].ToString());
                    PaymentAZNValue.Value = Convert.ToDecimal(dr["PAYMENT_AMOUNT_AZN"].ToString());
                    CurrencyLabel.Text = dr["CURRENCY_CODE"].ToString();
                    currency_id = Convert.ToInt32(dr["CURRENCY_ID"].ToString());
                    NoteText.Text = dr["NOTE"].ToString();
                    CustomerID = dr["CUSTOMER_ID"].ToString();
                    ContractID = dr["CONTRACT_ID"].ToString();
                    leasing_amount = Convert.ToDouble(dr["AMOUNT"].ToString());
                    PaymentInterestDebtValue.Value = Convert.ToDecimal(dr["PAYMENT_INTEREST_DEBT"].ToString());
                    CalculatedPenalty.Value = Convert.ToDecimal(dr["PENALTY_DEBT"].ToString());
                    PenaltyCheck.Checked = (Convert.ToInt32(dr["IS_PENALTY"].ToString()) == 1);
                    PenaltyValue.Value = Math.Abs(Convert.ToDecimal(dr["PENALTY_AMOUNT"].ToString()));
                    if (String.IsNullOrWhiteSpace(dr["LAST_DATE"].ToString()))
                    {
                        LastDateText.Visible = LastDateLabel.Visible = false;
                        LastDateText.Text = ContractStartDateText.Text;
                        Debt = leasing_amount;
                        DebtValue.Value = (decimal)Debt;
                    }
                    else
                    {
                        LastDateText.Visible = LastDateLabel.Visible = true;
                        LastDateText.Text = dr["LAST_DATE"].ToString().Substring(0, 10);
                        Debt = Convert.ToDouble(dr["LAST_DEBT"].ToString());
                        DebtValue.Value = (decimal)Debt;
                    }
                    BasicAmountCheck.Checked = (Convert.ToInt32(dr["IS_BASIC_AMOUNT"].ToString()) == 0);
                }

                one_day_interest = Math.Round(((Debt * Interest) / 100) / 360, 2);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
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

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaymentDate_EditValueChanged(object sender, EventArgs e)
        {      
            if (!FormStatus)
                return;

            if ((String.IsNullOrEmpty(PaymentDate.Text)) || (String.IsNullOrEmpty(LastDateText.Text)))
                return;

            ClearingDate.Properties.MinValue = PaymentDate.DateTime;

            if (!ClearingCheck.Checked)
                ClearingDate.EditValue = PaymentDate.DateTime;

            if (currency_id != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + Currency + " = ";
                CurrencyRateValue.Value = (decimal)cur;
            }
            else if (currency_id == 1)
                cur = 1;

            double startPercent = GlobalFunctions.ContractStartPercent(int.Parse(ContractID));

            diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy"));
            DayCountText.Text = diff_day.ToString();
            interest_amount = (diff_day * one_day_interest) + startPercent;
            residual_percent = interest_amount + GlobalFunctions.GetAmount($@"SELECT NVL(SUM(INTEREST_AMOUNT) - SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}");
            PaymentInterestDebtValue.Value = (decimal)residual_percent;
           
            totaldebtamount = Math.Round((Debt + residual_percent + DebtPenalty), 2);
            TotalDebtValue.Value = (decimal)totaldebtamount;
            List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(int.Parse(ContractID)).ToList<PaymentSchedules>();
            var schedules = lstSchedules.Where(s => s.MONTH_DATE <= GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy")).ToList<PaymentSchedules>();
            if (schedules.Count == 0)
                order_id = 0;
            else
                order_id = schedules.Max(s => s.ORDER_ID);

            if (order_id == 0)
                normal_debt = Debt;
            else
                normal_debt = Math.Round(lstSchedules.Find(s => s.ORDER_ID == order_id).DEBT, 2);

            if ((Debt - normal_debt) > 0)
                requiredamount = (Debt - normal_debt) + residual_percent;
            else
                requiredamount = residual_percent;

            RequiredValue.Value = (decimal)requiredamount;
            PaymentValue_EditValueChanged(sender, EventArgs.Empty);
            PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void InsertPayment()
        {
            double payment_interest_amount = 0, currenct_payment_interest_debt = 0, basic_amount = 0, current_debt = 0, total = 0, payment_value_AZN = 0, penalty = 0;

            if (PaymentValue.Value > 0 && BasicAmountCheck.Checked) // eger odenis sifirdan boyuk olarsa
            {
                if ((interest_amount + PaymentInterestDebt) > (double)PaymentValue.Value) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                    payment_interest_amount = (double)PaymentValue.Value;
                else
                    payment_interest_amount = interest_amount + PaymentInterestDebt; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
            }

            if (PenaltyCheck.Checked)
            {
                if ((double)PaymentValue.Value < (double)PenaltyValue.Value)
                {
                    basic_amount = 0;
                    penalty_amount = (double)PaymentValue.Value;
                    payment_interest_amount = 0;
                }
                else
                {
                    penalty = (double)PaymentValue.Value - (double)PenaltyValue.Value;
                    if (penalty < payment_interest_amount)
                    {
                        payment_interest_amount = penalty;
                        basic_amount = 0;
                    }
                    else
                        basic_amount = penalty - payment_interest_amount;
                    penalty_amount = (double)PenaltyValue.Value;
                }
                is_penalty = 1;
            }
            else
                basic_amount = (double)PaymentValue.Value - payment_interest_amount;
            currenct_payment_interest_debt = PaymentInterestDebt + interest_amount - payment_interest_amount;
            current_debt = Debt - basic_amount;
            total = current_debt + currenct_payment_interest_debt;
            if (currency_id == 1)
                payment_value_AZN = (double)PaymentValue.Value;
            else
                payment_value_AZN = (double)PaymentAZNValue.Value;

            int clearingCalc = 1;

            if (ClearingCheck.Checked && ClearingDate.DateTime > DateTime.Today)
                clearingCalc = 0;

            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP(ID,
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
                                                                                                CUSTOMER_TYPE_ID,
                                                                                                IS_BASIC_AMOUNT,
                                                                                                CLEARING_DATE,
                                                                                                IS_CLEARING,
                                                                                                CLEARING_CALCULATED)
                                                VALUES({PaymentID},
                                                            {CustomerID},
                                                            {ContractID},
                                                            TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                            {Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
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
                                                            {totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {requiredamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            '{NameText.Text.Trim()}',
                                                            '{NoteText.Text.Trim()}',
                                                            {GlobalVariables.V_UserID},
                                                            1,
                                                            '{PaymentType}',
                                                            {Math.Round(DebtPenalty, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {is_penalty},
                                                            {Math.Round(penalty_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {CustomerTypeID},
                                                            {((BasicAmountCheck.Checked)? 0 : 1)},
                                                            TO_DATE('{ClearingDate.Text}','DD/MM/YYYY'),
                                                            {((ClearingCheck.Checked) ? 1 : 0)},
                                                            {clearingCalc})",
                                           "Ödəniş temp cədvələ daxil edilmədi.",
                                           this.Name + "/InsertPayment");

            if (PaymentType == "C")
                GlobalProcedures.InsertOperationJournal(PaymentDate.Text, (double)CurrencyRateValue.Value, currency_id, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, basic_amount, payment_interest_amount, ContractID, PaymentID, null, 1, ClearingDate.Text);
            else
                GlobalProcedures.InsertAgreementOperationJournal(PaymentDate.Text, (double)CurrencyRateValue.Value, currency_id, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, basic_amount, payment_interest_amount, ContractID, PaymentID, "222M00000000", 1);
        }

        private void InsertBalancePenalty()
        {
            if (PenaltyCheck.Checked)
            {
                double currentdebtpenalty = DebtPenalty - penalty_amount;
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.BALANCE_PENALTIES_TEMP(ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_CHANGE,IS_COMMIT,PENALTY_STATUS,CUSTOMER_PAYMENT_ID,USED_USER_ID) VALUES(BALANCE_PENALTIES_SEQUENCE.NEXTVAL," + CustomerID + "," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY'),0,0," + currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN) + "," + penalty_amount.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",1,1,'Ödənilib'," + PaymentID + "," + GlobalVariables.V_UserID + ")",
                                                    "Cərimə temp cədvələ daxil edilmədi.");
            }
        }

        private void FPaymentAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPaymentsDataGridView(PaymentValue.Value, PaymentID);
        }

        private bool ControlPaymentDetails()
        {
            bool b = false;
            double sumpenalty = 0, paymentpenalty = 0;

            if (String.IsNullOrEmpty(CustomerID) || String.IsNullOrEmpty(ContractID))
            {
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(ContractCodeText + " saylı lizinq müqaviləsinə uyğun məlumatlar tapılmadı. Zəhmət olmasa müqavilələrin siyahısına baxın.");
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
                GlobalProcedures.ShowErrorMessage("Ödəniş tarixi daxil edilməyib.");
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

            if (PaymentValue.Value > TotalDebtValue.Value)
            {
                PaymentValue.BackColor = TotalDebtValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödənişin məbləği 'Tam bağlamaq üçün tələb olunan məbləğ' - dən çox ola bilməz.");
                PaymentValue.Focus();
                PaymentValue.BackColor = TotalDebtValue.BackColor = GlobalFunctions.ElementColor();
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

            if (PenaltyValue.Value > CalculatedPenalty.Value)
            {
                PenaltyValue.BackColor = CalculatedPenalty.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Silinəcək cərimə faizi balansa daxil edilmiş cərimə fazinin qalığından (" + CalculatedPenalty.Value + " " + PenaltyCurrencyLabel.Text + ") çox ola bilməz.");
                PenaltyValue.Focus();
                PenaltyValue.BackColor = CalculatedPenalty.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (PenaltyValue.Value <= CalculatedPenalty.Value)
            {
                sumpenalty = GlobalFunctions.GetAmount($@"SELECT NVL(SUM(PENALTY_AMOUNT),0) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE CONTRACT_ID = {ContractID} AND IS_CHANGE <> 2");
                paymentpenalty = GlobalFunctions.GetAmount($@"SELECT NVL(SUM(PAYMENT_PENALTY),0) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE CONTRACT_ID = {ContractID} AND IS_CHANGE <> 2 AND BAL_DATE > TO_DATE('{PaymentDate.Text}','DD/MM/YYYY')");
                if ((sumpenalty - paymentpenalty) < (double)PenaltyValue.Value)
                {
                    PenaltyValue.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage(PaymentDate.Text + " tarixindən sonrakı tarixlərdə də cərimə faizləri silindiyi üçün silinən cərimə faizinin məbləği ən çoxu " + (sumpenalty - paymentpenalty).ToString(GlobalVariables.V_CultureInfoAZ) + " ola bilər.");
                    PenaltyValue.Focus();
                    PenaltyValue.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPaymentDetails())
            {
                if (TransactionName == "INSERT")
                {
                    InsertPayment();
                    InsertBalancePenalty();
                }
                else
                {
                    GlobalProcedures.ShowWarningMessage("Əgər " + ldate + " tarixinə olan ödənişi dəyişib yadda saxlasaz, bu zaman portfel və kassanın qalığı yenidən hesablanacaq.");                    
                    GlobalProcedures.UpdateCustomerPayment(1,
                                                                PaymentDate.Text,
                                                                ClearingDate.Text,
                                                                int.Parse(ContractID),
                                                                (double)Math.Round(PaymentValue.Value, 2),
                                                                (((BasicAmountCheck.Checked) ? 0 : 1)),
                                                                (double)Math.Round(PaymentAZNValue.Value, 2),
                                                                (double)CurrencyRateValue.Value,
                                                                NameText.Text.Trim(),
                                                                Convert.ToInt32(DayCountText.Text.Trim()),
                                                                (double)OneDayInterestValue.Value,
                                                                Math.Round(interest_amount, 2),
                                                                (double)TotalDebtValue.Value,
                                                                (double)RequiredValue.Value,
                                                                Convert.ToInt32(PaymentID),
                                                                (double)PenaltyValue.Value,
                                                                is_penalty,
                                                                0,
                                                                0,
                                                                null,
                                                                NoteText.Text.Trim());
                }
                //GlobalProcedures.LoadReceipt(PaymentDate.Text, (double)PaymentAZNValue.Value, currency_id, CurrencyRateValue.Value, CustomerNameText.Text.Trim(), int.Parse(ContractID), ContractCodeText.Text.Trim(), "saylı lizinq müqaviləsi üzrə ödəniş");
                if (ReceiptCheck.Checked)
                    GlobalProcedures.LoadBankReceipt(PaymentDate.Text, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, currency_id, (decimal)CurrencyRateValue.Value, Currency, NameText.Text.Trim(), int.Parse(ContractID), ContractCodeText.Text.Trim(), "saylı lizinq müqaviləsi üzrə ödəniş", NameText.Text.Trim());
                this.Close();
            }
        }

        private void PaymentValue_EditValueChanged(object sender, EventArgs e)
        {
            PaymentAZNValue.Value = (decimal)((double)PaymentValue.Value * (double)CurrencyRateValue.Value);
            if (cur != (double)CurrencyRateValue.Value)
                CurrencyRateValue.BackColor = Color.GreenYellow;
            else
                CurrencyRateValue.BackColor = Class.GlobalFunctions.ElementColor();
        }

        private void PaymentAZNValue_EditValueChanged(object sender, EventArgs e)
        {
            if (CurrencyRateValue.Value != 0)
                PaymentValue.Value = (decimal)((double)PaymentAZNValue.Value / (double)CurrencyRateValue.Value);
            else
                PaymentValue.Value = 0;
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                PaymentCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        //void RefreshInterest(double value, double interest)
        //{
        //    if ((double)PaymentValue.Value <= value && PaymentValue.Value > 0)
        //    {
        //        PenaltyText.Text = ((Debt * diff_day * interest) / 100).ToString("N2");
        //        penalty_amount = (Debt * diff_day * interest) / 100;
        //        if (penalty_interest_id == 0)
        //            penalty_interest_id = Class.GlobalFunctions.GetID("SELECT ID FROM CRS_USER.CONTRACT_INTEREST_PENALTIES P WHERE P.CONTRACT_ID = " + ContractID + " AND P.CALC_DATE = (SELECT MAX(CALC_DATE) FROM CRS_USER.CONTRACT_INTEREST_PENALTIES WHERE CONTRACT_ID = P.CONTRACT_ID AND CALC_DATE <= TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY'))");
        //    }
        //    else
        //    {
        //        PenaltyText.Text = "0.00";
        //        penalty_amount = 0;
        //        penalty_interest_id = 0;
        //    }
        //    PenaltyLabel.Text = "Cərimə (" + interest + " %, ödəniş <= " + value + ")";
        //}

        //private void LoadFInterestAddEdit(string transaction, string contractid, string interestid, string startdate, int istemp)
        //{
        //    Forms.Contracts.FInterestAddEdit fiae = new Contracts.FInterestAddEdit();
        //    fiae.TransactionName = transaction;
        //    fiae.ContractID = contractid;            
        //    fiae.InterestID = interestid;
        //    fiae.StartDate = startdate;
        //    fiae.IsTemp = istemp;
        //    fiae.RefreshInterestDataGridView += new Forms.Contracts.FInterestAddEdit.DoEvent(RefreshInterest);
        //    fiae.ShowDialog();
        //}

        //private void BChangePenalty_Click(object sender, EventArgs e)
        //{
        //    LoadFInterestAddEdit("EDIT", ContractID, penalty_interest_id.ToString(), ContractStartDate,0);
        //}

        private void PenaltyCheck_CheckedChanged(object sender, EventArgs e)
        {
            PenaltyValue.Properties.ReadOnly = !PenaltyCheck.Checked;
            if (PenaltyCheck.Checked)
            {
                PenaltyValue.Focus();
                is_penalty = 1;
                if (TransactionName == "INSERT")
                    PenaltyValue.Value = (decimal)DebtPenalty;
            }
            else
                is_penalty = 0;
        }
    }
}