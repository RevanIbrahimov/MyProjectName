using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;
using DevExpress.Utils;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.AttractedFunds
{
    public partial class FBuyAmount : DevExpress.XtraEditors.XtraForm
    {
        public FBuyAmount()
        {
            InitializeComponent();
        }
        public string TransactionName, PaymentID, LastDate, ContractID, Currency, StartDate, ContractNumber;
        public double Debt, PaymentInterestDebt;
        public int  SourceID, LastID, PaymentCount = 0, CurrencyID;

        int diff_day, pay_count, bank_id, appointment_id;
        double one_day_interest = 0,
            interest_amount = 0,
            current_debt = 0,
            basic_amount = 0,
            payment_interest_amount = 0,
            residual_percent = 0,
            totaldebtamount = 0, cur;

        decimal percent = 0;

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(PaymentDate.Text);
            if (CurrencyID != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(CurrencyID, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + Currency + " = ";
                CurrencyRateValue.Value = (decimal)cur;
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 11:
                    {
                        if (SourceID != 6)
                            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                        else
                        {
                            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", $@"STATUS_ID = 7 AND ID = (SELECT BANK_ID FROM CRS_USER.BANK_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}) ORDER BY ORDER_ID");
                            BankLookUp.ItemIndex = (TransactionName == "INSERT") ? 0 : -1;
                        }
                    }
                    break;
                case 12:
                    GlobalProcedures.FillLookUpEdit(AppointmentLookUp, "BANK_APPOINTMENTS", "ID", "NAME", "OPERATION_TYPE_ID = 1 AND APPOINTMENT_TYPE_ID = 4 ORDER BY NAME");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void AppointmentLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 12);
        }

        private void AppointmentLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AppointmentLookUp.EditValue == null)
                return;

            appointment_id = Convert.ToInt32(AppointmentLookUp.EditValue);
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
        }

        bool FormStatus = false;

        public delegate void DoEvent();
        public event DoEvent RefreshFundsDataGridView;

        private void FBuyAmount_Load(object sender, EventArgs e)
        {
            BankStarLabel.Visible = AppointmentStarLabel.Visible = (SourceID == 6);
            CurrencyRateValue.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            CurrencyRateValue.Properties.DisplayFormat.FormatString = "### ##0.0000";
            GlobalProcedures.CalcEditFormat(AmountValue);
            CurrencyLabel.Text = Currency;
            
            LastDateLabel.Visible = LastDateText.Visible = !(PaymentCount == 0);
            LastDateText.Text = LastDate;

            RefreshDictionaries(11);
            RefreshDictionaries(12);

            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                PaymentDate.EditValue = DateTime.Today;
            }
            else
            {
                LoadAmountDetails();
                ComponentEnable((TransactionName == "SHOW"));
            }

            PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");

            current_debt = Debt;

            FormStatus = true;
        }

        private void LoadAmountDetails()
        {
            string s = $@"SELECT TO_CHAR (FP.PAYMENT_DATE, 'DD/MM/YYYY') PAYMENT_DATE,
                                   FP.BUY_AMOUNT,
                                   LP.LAST_DATE,
                                   LP.LAST_DEBT,
                                   FP.CURRENCY_RATE,
                                   BA.NAME APPOINTMENT_NAME,
                                   B.LONG_NAME BANK_NAME,
                                   FP.NOTE,
                                   FP.PERCENT
                              FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP FP,
                                   CRS_USER_TEMP.V_FUND_LAST_PAYMENT_TEMP LP,
                                   CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO,
                                   CRS_USER.BANK_APPOINTMENTS BA,
                                   CRS_USER.BANKS B
                             WHERE     FP.ID = LP.ID
                                   AND FP.ID = BO.FUNDS_PAYMENT_ID(+)
                                   AND BO.APPOINTMENT_ID = BA.ID(+)
                                   AND BO.BANK_ID = B.ID(+)
                                   AND FP.ID = {PaymentID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadAmountDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    PaymentDate.Enabled = false;
                    PaymentDate.EditValue = GlobalFunctions.ChangeStringToDate(dr["PAYMENT_DATE"].ToString(), "ddmmyyyy");
                    AmountValue.Value = Convert.ToDecimal(dr["BUY_AMOUNT"].ToString());
                    percent = Convert.ToDecimal(dr["PERCENT"].ToString());
                    InterestText.Text = percent.ToString();
                    if (CurrencyID != 1)
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                        CurrencyRateLabel.Text = "1 " + Currency + " = ";
                        if (!String.IsNullOrWhiteSpace(dr["CURRENCY_RATE"].ToString()))
                            CurrencyRateValue.Value = Convert.ToDecimal(dr["CURRENCY_RATE"].ToString());
                    }

                    if (!String.IsNullOrWhiteSpace(dr["LAST_DATE"].ToString()))
                    {
                        LastDateText.Visible = LastDateLabel.Visible = true;
                        LastDateText.Text = dr["LAST_DATE"].ToString().Substring(0, 10);
                        Debt = Convert.ToDouble(dr["LAST_DEBT"].ToString());
                    }

                    BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                    AppointmentLookUp.EditValue = AppointmentLookUp.Properties.GetKeyValueByDisplayText(dr["APPOINTMENT_NAME"].ToString());
                    NoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Cəlb olunan məbləğ açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void ComponentEnable(bool status)
        {
            PaymentDate.Enabled =
                AmountValue.Enabled =
                BankLookUp.Enabled =
                CurrencyRateValue.Enabled =
                AppointmentLookUp.Enabled = !status;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FBuyAmount_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshFundsDataGridView();
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                PaymentCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private void InsertAmount()
        {
            double currenct_payment_interest_debt = 0, total = 0;

            if (current_debt == 0)
            {
                interest_amount = 0;
                diff_day = 0;
                Debt = 0;
                totaldebtamount = 0;
            }

            currenct_payment_interest_debt = PaymentInterestDebt + interest_amount - payment_interest_amount;
            current_debt = Debt + (double)AmountValue.Value - basic_amount;
            total = current_debt + currenct_payment_interest_debt;

            PaymentID = GlobalFunctions.InsertQuery($@"INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,IS_CHANGE,PERCENT)VALUES(FUNDS_PAYMENTS_SEQUENCE.NEXTVAL," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY')," + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,0," + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + diff_day + "," + Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0," + Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN) + ",0," + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + NoteText.Text.Trim() + "'," + GlobalVariables.V_UserID + ",1," + Math.Round(percent,2).ToString(GlobalVariables.V_CultureInfoEN) + ") RETURNING ID INTO :ID",
                                                "Verilən məbləğ temp cədvələ daxil edilmədi.",
                                                "ID",
                                             this.Name + "/InsertAmount").ToString();
        }

        private void UpdateAmount()
        {
            double total = 0, currenct_payment_interest_debt = 0;
            if (current_debt <= 0)
                Debt = 0;

            currenct_payment_interest_debt = PaymentInterestDebt + interest_amount - payment_interest_amount;
            current_debt = Debt + (double)AmountValue.Value - basic_amount;
            total = current_debt + currenct_payment_interest_debt;
            totaldebtamount = Math.Round((Debt + residual_percent), 2);

            if (AmountValue.Value > 0)
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 1, BUY_AMOUNT = " + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "',REQUIRED_CLOSE_AMOUNT = " + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + ",CURRENCY_RATE = " + Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN) + ",PERCENT = " + Math.Round(percent, 2).ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + PaymentID + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                "Verilən məbləğ temp cədvəldə dəyişdirilmədi.",
                                                this.Name + "/UpdateAmount");
            else
            {
                int pay_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_AMOUNT > 0 AND IS_CHANGE IN (0,1) AND PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY')");
                if (pay_count == 0)
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 2 WHERE ID = " + PaymentID + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                "Verilən məbləğ temp cədvəldən silinmədi.",
                                                this.Name + "/UpdateAmount");
                else
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 1, BUY_AMOUNT = " + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "',REQUIRED_CLOSE_AMOUNT = " + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + ",CURRENCY_RATE = " + Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN) + ",PERCENT = " + Math.Round(percent, 2).ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + PaymentID + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                "Verilən məbləğ temp cədvəldə dəyişdirilmədi.",
                                                this.Name + "/UpdateAmount");
            }
        }

        private bool ControlPaymentDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(PaymentDate.Text))
            {
                PaymentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                PaymentDate.Focus();
                PaymentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AmountValue.Value <= 0)
            {
                AmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                AmountValue.Focus();
                AmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CurrencyID > 1 && CurrencyRateValue.Value <= 0)
            {
                CurrencyRateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(PaymentDate.Text + " tarixinə məzənnə daxil edilməyib.");
                CurrencyRateValue.Focus();
                CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SourceID == 6 && BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank seçilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SourceID == 6 && AppointmentLookUp.EditValue == null)
            {
                AppointmentLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təyinat seçilməyib.");
                AppointmentLookUp.Focus();
                AppointmentLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
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
                    InsertAmount();
                    InsertBankOperationTemp();
                    if (SourceID == 10)
                        InsertCashFounderTemp();
                }
                else
                {
                    PaymentDate_EditValueChanged(sender, EventArgs.Empty);
                    UpdateAmount();
                    UpdateBankOperationTemp();
                    if (SourceID == 10)
                        UpdateCashFounderTemp();
                }

                GlobalProcedures.UpdateFundChange(PaymentDate.Text, int.Parse(ContractID), 1);
                this.Close();
            }
        }

        private void InsertBankOperationTemp()
        {
            if (SourceID != 6)
                return;

            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,IS_CHANGE,USED_USER_ID,FUNDS_PAYMENT_ID,FUNDS_CONTRACT_ID,CONTRACT_CODE) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + bank_id + ",TO_DATE('" + PaymentDate.Text.Trim() + "','DD/MM/YYYY')," + appointment_id + "," + Math.Round(AmountValue.Value * CurrencyRateValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,0,'" + NoteText.Text.Trim() + "',1," + GlobalVariables.V_UserID + "," + PaymentID + "," + ContractID + ",'" + ContractNumber + "')",
                                            "Bank əməliyyatlarının mədaxili daxil edilmədi.",
                                           this.Name + "/InsertBankOperationTemp");
        }

        private void UpdateBankOperationTemp()
        {
            if (SourceID != 6)
                return;

            if (AmountValue.Value > 0)
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET INCOME = {Math.Round(AmountValue.Value * CurrencyRateValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}',IS_CHANGE = 1,APPOINTMENT_ID = {appointment_id} WHERE BANK_ID = {bank_id} AND OPERATION_DATE = TO_DATE('{PaymentDate.Text.Trim()}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID} AND FUNDS_PAYMENT_ID = {PaymentID}",
                                                "Bank əməliyyatlarının mədaxili dəyişdirilmədi.",
                                                this.Name + "/UpdateBankOperationTemp");
            else
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET IS_CHANGE = 2, APPOINTMENT_ID = {appointment_id} WHERE BANK_ID = {bank_id} AND OPERATION_DATE = TO_DATE('{PaymentDate.Text.Trim()}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID} AND FUNDS_PAYMENT_ID = {PaymentID}",
                                                "Bank əməliyyatlarının mədaxili silinmədi.",
                                              this.Name + "/UpdateBankOperationTemp");
        }

        private void InsertCashFounderTemp()
        {
            string founder_id = null, founder_card_id = null, s = null;

            s = $@"SELECT FOUNDER_ID,FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/InsertCashFounderTemp");
            if (dt.Rows.Count > 0)
            {
                founder_id = dt.Rows[0]["FOUNDER_ID"].ToString();
                founder_card_id = dt.Rows[0]["FOUNDER_CARD_ID"].ToString();

                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.CASH_FOUNDER_TEMP(ID,FOUNDER_ID,FOUNDER_CARD_ID,PAYMENT_DATE,APPOINTMENT,AMOUNT,INC_EXP,NOTE,USED_USER_ID,IS_CHANGE,FUND_PAYMENT_ID)VALUES(CASH_FOUNDER_SEQUENCE.NEXTVAL," + founder_id + "," + founder_card_id + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY'),'Təsisçi tərəfindən mədaxil'," + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",1,'" + NoteText.Text.Trim() + "'," + GlobalVariables.V_UserID + ",1," + PaymentID + ")",
                                                        "Təsisçi ilə hesablaşmanın mədaxili temp cədvələ daxil olunmadı.",
                                                   this.Name + "/InsertCashFounderTemp");
            }
        }

        private void UpdateCashFounderTemp()
        {
            string founder_id = null, founder_card_id = null, s = null;

            s = $@"SELECT FOUNDER_ID,FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/UpdateCashFounderTemp");
            if (dt.Rows.Count > 0)
            {
                founder_id = dt.Rows[0]["FOUNDER_ID"].ToString();
                founder_card_id = dt.Rows[0]["FOUNDER_CARD_ID"].ToString();

                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CASH_FOUNDER_TEMP SET IS_CHANGE = 1, AMOUNT = {Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}' WHERE INC_EXP = 1 AND FOUNDER_ID = {founder_id} AND FOUNDER_CARD_ID = {founder_card_id} AND PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY') AND FUND_PAYMENT_ID = {PaymentID}",
                                                        "Təsisçi ilə hesablaşmanın mədaxili temp cədvəldə dəyişdirilmədi.",
                                                     this.Name + "/UpdateCashFounderTemp");
            }
        }

        private void PaymentDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                if ((!String.IsNullOrEmpty(PaymentDate.Text)) && (!String.IsNullOrEmpty(LastDateText.Text)))
                {
                    CurrencyRate();

                    List<FundContractPercent> lstPercent = FundContractPercentDAL.SelectFundContractPercentByContractID(int.Parse(ContractID)).ToList<FundContractPercent>();
                    if (lstPercent.Count > 0)
                    {
                        int lastPercentID = lstPercent.Where(item => item.PDATE <= PaymentDate.DateTime).Max(item => item.ID);
                        percent = lstPercent.Find(item => item.ID == lastPercentID).PERCENT_VALUE;
                    }
                    InterestText.Text = percent.ToString();
                    one_day_interest = Math.Round(((Debt * (double)percent) / 100) / 360, 2);
                    diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(LastDate, "ddmmyyyy"), PaymentDate.DateTime);
                    pay_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY')");
                    //hesablanmis faiz
                    interest_amount = diff_day * one_day_interest;
                    residual_percent = interest_amount + GlobalFunctions.GetAmount($@"SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}");
                    totaldebtamount = Math.Round((Debt + residual_percent), 2);
                    DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT BASIC_AMOUNT,PAYMENT_INTEREST_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND CP.PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID}", this.Name + "/PaymentDate_EditValueChanged");
                    if (dt.Rows.Count > 0)
                    {
                        //esas mebleg
                        basic_amount = Convert.ToDouble(dt.Rows[0]["BASIC_AMOUNT"]); //GlobalFunctions.GetAmount($@"SELECT BASIC_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND CP.PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID}");
                        //odenilen faiz
                        payment_interest_amount = Convert.ToDouble(dt.Rows[0]["PAYMENT_INTEREST_AMOUNT"]); //GlobalFunctions.GetAmount($@"SELECT PAYMENT_INTEREST_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND CP.PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID}");
                    }
                }
            }
        }

        private void CurrencyRate()
        {
            if (CurrencyID != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(CurrencyID, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + Currency + " = ";
            }
            else
                cur = 1;

            CurrencyRateValue.Value = (decimal)cur;
        }
    }
}