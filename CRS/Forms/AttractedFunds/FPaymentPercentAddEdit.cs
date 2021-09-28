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
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using CRS.Class;

namespace CRS.Forms.AttractedFunds
{
    public partial class FPaymentPercentAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPaymentPercentAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, LastDate, Currency, ContractID, SourceName, ContractCode, ContractStartDate, PayDate;
        public int CurrencyID, PaymentID;

        decimal percent = 0, debt = 0, one_day_interest = 0, currenct_payment_interest_debt = 0, interest_amount = 0;

        int diff_day = 0;
        DateTime lastPaymentDate = DateTime.Today;
        bool FormStatus = false;

        List<FundPayment> lstPayment = null;
        List<FundContractPercent> lstPercent = null;

        public delegate void DoEvent(decimal a, int p);
        public event DoEvent RefreshPaymentsDataGridView;

        private void PaymentDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                if (!String.IsNullOrEmpty(PaymentDate.Text))
                {
                    GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));

                    if (CurrencyID != 1)
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = true;
                        CurrencyRateLabel.Text = "1 " + Currency + " = ";
                        CurrencyRateValue.Value = (decimal)GlobalFunctions.CurrencyLastRate(CurrencyID, PaymentDate.Text);
                    }

                    if (lstPercent.Count > 0)
                    {
                        int lastPercentID = lstPercent.Where(item => item.PDATE <= PaymentDate.DateTime.AddDays(-1)).Max(item => item.ID);
                        percent = lstPercent.Find(item => item.ID == lastPercentID).PERCENT_VALUE;
                    }

                    InterestText.Text = percent.ToString();

                    if (lstPayment.Count > 0)
                    {
                        DateTime lastDate = lstPayment.Where(item => item.PAYMENT_DATE <= PaymentDate.DateTime).Max(item => item.PAYMENT_DATE);
                        int lastPaymentID = lstPayment.Where(item => item.PAYMENT_DATE == lastDate).Max(item => item.ID);
                        var payment = lstPayment.Find(item => item.ID == lastPaymentID);
                        lastPaymentDate = payment.PAYMENT_DATE;
                        debt = payment.DEBT;
                        currenct_payment_interest_debt = payment.PAYMENT_INTEREST_DEBT;
                    }

                    LastDateText.Text = lastPaymentDate.ToString("dd.MM.yyyy");
                    diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy"), PaymentDate.DateTime);
                    DayCountText.Text = diff_day.ToString();
                    DebtValue.Value = debt;
                    one_day_interest = Math.Round(((debt * percent) / 100) / 360, 2);
                    OneDayInterestValue.EditValue = one_day_interest;
                    interest_amount = diff_day * one_day_interest;
                    PaymentInterestValue.Value = interest_amount;
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }

        private void FPaymentPercentAddEdit_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            ContractStartDateText.Text = ContractStartDate;
            FundSourceNameText.Text = SourceName;
            CurrencyLabel.Text = PaymentCurrencyLabel.Text = DebtCurrencyLabel.Text = Currency;

            GlobalProcedures.CalcEditFormat(DebtValue);
            GlobalProcedures.CalcEditFormat(PaymentInterestValue);
            GlobalProcedures.CalcEditFormat(OneDayInterestValue);

            lstPercent = FundContractPercentDAL.SelectFundContractPercentByContractID(int.Parse(ContractID)).ToList<FundContractPercent>();
            lstPayment = FundPaymentDAL.SelectFundPayments(1, int.Parse(ContractID)).ToList<FundPayment>();

            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                lastPaymentDate = GlobalFunctions.ChangeStringToDate(ContractStartDate, "ddmmyyyy");
                PaymentDate.EditValue = DateTime.Today;
            }
            else
            {
                LoadData();
                FormStatus = true;
            }
        }

        private void LoadData()
        {
            string sql = $@"SELECT * FROM CRS_USER_TEMP.V_FUND_LAST_PAYMENT_TEMP WHERE ID = {PaymentID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadData", "Faizin məlumatları açılmadı.");

            if(dt.Rows.Count > 0)
            {
                PaymentDate.EditValue = dt.Rows[0]["PAYMENT_DATE"];
                LastDateText.Text = dt.Rows[0]["LAST_DATE"].ToString().Substring(0,10);
                percent = Convert.ToDecimal(dt.Rows[0]["PERCENT"]);
                InterestText.Text = percent.ToString();
                debt = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                DebtValue.EditValue = debt;
                diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy"), PaymentDate.DateTime);
                DayCountText.Text = diff_day.ToString();
                one_day_interest = Math.Round(((debt * percent) / 100) / 360, 2);
                OneDayInterestValue.EditValue = one_day_interest;
                interest_amount = diff_day * one_day_interest;
                PaymentInterestValue.Value = interest_amount;
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
            }
        }

        private void FPaymentPercentAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPaymentsDataGridView(PaymentInterestValue.Value, PaymentID);
        }

        private bool ControlPaymentDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(ContractID))
            {
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(ContractCodeText + " nömrəli müqaviləyə uyğun məlumatlar tapılmadı.");
                ContractCodeText.Focus();
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
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

            if (TransactionName == "INSERT" && lstPayment.Where(item => item.PAYMENT_DATE == PaymentDate.DateTime).ToList<FundPayment>().Count > 0)
            {
                PaymentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(PaymentDate.Text + " tarixi üçün faiz daxil edilib. Zəhmət olmasa başqa tarix seçin.");
                PaymentDate.Focus();
                PaymentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertPayment()
        {
            decimal current_debt = 0, total = 0;

            current_debt = debt;
            total = current_debt + currenct_payment_interest_debt;

            PaymentID = GlobalFunctions.InsertQuery($@"INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,
                                                                                                        CONTRACT_ID,
                                                                                                        PAYMENT_DATE,
                                                                                                        BUY_AMOUNT,
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
                                                                                                        NOTE,
                                                                                                        USED_USER_ID,
                                                                                                        IS_CHANGE,
                                                                                                        MANUAL_INTEREST,
                                                                                                        PERCENT_TYPE,
                                                                                                        PERCENT)
                                                    VALUES(FUNDS_PAYMENTS_SEQUENCE.NEXTVAL,
                                                        {ContractID},
                                                        TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                        0,
                                                        0,
                                                        0,
                                                        {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {diff_day},
                                                        {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        {Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                        0,
                                                        0,
                                                        '{NoteText.Text.Trim()}',
                                                        {GlobalVariables.V_UserID},                                                        
                                                        1,
                                                        0,
                                                        0,
                                                        {Math.Round(Convert.ToDecimal(InterestText.Text), 2).ToString(GlobalVariables.V_CultureInfoEN)}) RETURNING ID INTO :ID",
                                            "Ödəniş temp cədvələ daxil edilmədi.",
                                            "ID",
                                            this.Name + "/InsertPayment");
        }

        private void UpdatePayment()
        {
            decimal current_debt = 0, total = 0;

            current_debt = debt;
            total = current_debt + currenct_payment_interest_debt;

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY'),
                                                                                            DEBT = {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                            DAY_COUNT = {diff_day},
                                                                                            ONE_DAY_INTEREST_AMOUNT = {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                            INTEREST_AMOUNT = {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                            PAYMENT_INTEREST_AMOUNT = {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                            PAYMENT_INTEREST_DEBT = {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                            TOTAL = {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                            CURRENCY_RATE = {Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                            PAYMENT_AMOUNT_AZN = 0,
                                                                                            REQUIRED_CLOSE_AMOUNT = 0,
                                                                                            NOTE = '{NoteText.Text.Trim()}',
                                                                                            USED_USER_ID = {GlobalVariables.V_UserID},
                                                                                            IS_CHANGE = 1,
                                                                                            MANUAL_INTEREST = 0,
                                                                                            PERCENT_TYPE = 0,
                                                                                            PERCENT = {Math.Round(Convert.ToDecimal(InterestText.Text), 2).ToString(GlobalVariables.V_CultureInfoEN)} 
                                                    WHERE ID = {PaymentID}",
                                            "Ödəniş temp cədvəldə dəyişdirilmədi.",                                            
                                            this.Name + "/InsertPayment");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPaymentDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPayment();
                else
                    UpdatePayment();
                GlobalProcedures.UpdateFundChange(PaymentDate.Text, int.Parse(ContractID), 1);
                this.Close();
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                PaymentCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }
    }
}