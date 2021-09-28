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
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.AttractedFunds
{
    public partial class FPaymentAmountAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPaymentAmountAddEdit()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName, LastDate, Currency, ContractID, SourceName, ContractCode, ContractStartDate, PayDate;
        public int PaymentCount = 0, SourceID = 0, CurrencyID, PaymentID;
        public double Amount, Debt, PaymentAmount, PaymentInterestDebt;

        double one_day_interest = 0,
            interest_amount = 0,
            residual_percent = 0,
            cur,
            totaldebtamount = 0,
            buy_amount = 0;

        decimal percent = 0;

        int diff_day,
            pay_count = 0,
            bank_id,
            appointment_id;
        bool FormStatus = false;

        public delegate void DoEvent(decimal a, int p);
        public event DoEvent RefreshPaymentsDataGridView;

        private void FPaymentAmountAddEdit_Load(object sender, EventArgs e)
        {
            BankStarLabel.Visible = AppointmentStarLabel.Visible = (SourceID == 6);            
            RefreshDictionaries(11);
            RefreshDictionaries(12);

            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                LastDateLabel.Visible = LastDateText.Visible = !(PaymentCount == 0);
                LastDateText.Text = LastDate;
                ContractCodeText.Text = ContractCode;
                ContractStartDateText.Text = ContractStartDate;
                FundSourceNameText.Text = SourceName;
                CurrencyLabel.Text = Currency;
                DebtValue.EditValue = Debt;
               
                if (String.IsNullOrEmpty(PayDate))
                {
                    PaymentDate.EditValue = DateTime.Today;
                    this.ActiveControl = PaymentDate;
                }
                else
                {
                    PaymentDate.EditValue = GlobalFunctions.ChangeStringToDate(PayDate, "ddmmyyyy");
                    PaymentDate.Enabled = false;
                    this.ActiveControl = PaymentValue;
                }
            }
            else
            {
                LoadPaymentDetails();
                ComponentEnable((TransactionName == "SHOW"));
            }

            if (CurrencyID == 1)
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

            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT MIN(PAYMENT_DATE) PDATE,COUNT(*) RCOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE > TO_DATE('{PaymentDate.Text}','DD.MM.YYYY')");
            if (Convert.ToInt16(dt.Rows[0]["RCOUNT"]) > 0)
                PaymentDate.Properties.MaxValue = Convert.ToDateTime(dt.Rows[0]["PDATE"]);
            FormStatus = true;
            ManualInterestCheckEdit_CheckedChanged(sender, EventArgs.Empty);
        }

        private void LoadPaymentDetails()
        {
            double fund_amount = 0;
            string s = $@"SELECT C.CONTRACT_NUMBER,
                                   TO_CHAR (C.START_DATE, 'DD.MM.YYYY') S_DATE,
                                      FS.NAME
                                   || ': '
                                   || (CASE
                                          WHEN FS.ID = 6
                                          THEN
                                             (SELECT B.LONG_NAME || ', ' || BB.NAME
                                                FROM CRS_USER.BANKS B,
                                                     CRS_USER.BANK_BRANCHES BB,
                                                     CRS_USER.BANK_CONTRACTS BC
                                               WHERE     B.ID = BB.BANK_ID
                                                     AND B.ID = BC.BANK_ID
                                                     AND BC.FUNDS_CONTRACT_ID = C.ID)
                                          WHEN FS.ID = 10
                                          THEN
                                             (SELECT FULLNAME
                                                FROM CRS_USER.FOUNDERS
                                               WHERE ID = (SELECT FOUNDER_ID
                                                             FROM CRS_USER.FOUNDER_CONTRACTS
                                                            WHERE FUNDS_CONTRACT_ID = C.ID))
                                          ELSE
                                             (SELECT NAME
                                                FROM CRS_USER.FUNDS_SOURCES_NAME
                                               WHERE     ID = C.FUNDS_SOURCE_NAME_ID
                                                     AND SOURCE_ID = C.FUNDS_SOURCE_ID)
                                       END)
                                   || ', '
                                   || C.AMOUNT
                                   || ' '
                                   || CUR.CODE
                                      SOURCE_NAME,
                                   TO_CHAR (CP.PAYMENT_DATE, 'DD.MM.YYYY') P_DATE,
                                   CP.DAY_COUNT,
                                   CP.PERCENT INTEREST,
                                   CP.REQUIRED_CLOSE_AMOUNT,
                                   CP.ONE_DAY_INTEREST_AMOUNT,
                                   CP.PAYMENT_AMOUNT,
                                   CP.PAYMENT_AMOUNT_AZN,
                                   C.CURRENCY_ID,
                                   CUR.CODE,
                                   CP.NOTE,
                                   CP.CONTRACT_ID,
                                   CP.CURRENCY_RATE,
                                   CP.PAYMENT_INTEREST_DEBT,
                                   CP.PAYMENT_INTEREST_AMOUNT,
                                   CP.BASIC_AMOUNT,
                                   CP.MANUAL_INTEREST,
                                   CP.INTEREST_AMOUNT,
                                   C.AMOUNT,
                                   CP.PERCENT_TYPE,
                                   LP.LAST_DATE,
                                   LP.LAST_DEBT,
                                   B.LONG_NAME BANK_NAME,
                                   BA.NAME APPOINTMENT_NAME
                              FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP,
                                   CRS_USER.FUNDS_CONTRACTS C,
                                   CRS_USER.CURRENCY CUR,
                                   CRS_USER.FUNDS_SOURCES FS,
                                   CRS_USER_TEMP.V_FUND_LAST_PAYMENT_TEMP LP,
                                   CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO,
                                   CRS_USER.BANK_APPOINTMENTS BA,
                                   CRS_USER.BANKS B
                             WHERE     CP.CONTRACT_ID = C.ID
                                   AND C.CURRENCY_ID = CUR.ID
                                   AND C.FUNDS_SOURCE_ID = FS.ID
                                   AND CP.ID = LP.ID
                                   AND CP.ID = BO.FUNDS_PAYMENT_ID(+)
                                   AND BO.APPOINTMENT_ID = BA.ID(+)
                                   AND BO.BANK_ID = B.ID(+)
                                   AND CP.ID = {PaymentID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    ContractCodeText.Text = dr["CONTRACT_NUMBER"].ToString();
                    ContractStartDateText.Text = dr["S_DATE"].ToString();
                    FundSourceNameText.Text = dr[2].ToString();
                    PaymentDate.EditValue = GlobalFunctions.ChangeStringToDate(dr[3].ToString(), "ddmmyyyy");
                    PaymentDate.Enabled = false;
                    DayCountText.Text = dr[4].ToString();
                    InterestText.Text = dr[5].ToString();
                    percent = Convert.ToDecimal(dr[5].ToString());
                    TotalDebtValue.EditValue = Convert.ToDouble(dr[6].ToString());
                    OneDayInterestValue.EditValue = Convert.ToDouble(dr[7].ToString());

                    if (CurrencyID != 1)
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                        CurrencyRateLabel.Text = "1 " + dr[11].ToString() + " = ";
                        CurrencyRateValue.Value = Convert.ToDecimal(dr[14].ToString());
                    }
                    PaymentValue.Value = Convert.ToDecimal(dr[8].ToString());
                    PaymentAZNValue.Value = Convert.ToDecimal(dr[9].ToString());

                    CurrencyLabel.Text = dr[11].ToString();
                    NoteText.Text = dr[12].ToString();

                    ContractID = dr[13].ToString();
                    PaymentInterestDebtValue.EditValue = Convert.ToDouble(dr[15].ToString());
                    PaymentInterestAmountValue.Value = Convert.ToDecimal(dr[16].ToString());
                    BasicAmountValue.Value = Convert.ToDecimal(dr[17].ToString());
                    ManualInterestCheckEdit.Checked = (Convert.ToInt32(dr["MANUAL_INTEREST"].ToString()) == 1);
                    interest_amount = Convert.ToDouble(dr[19].ToString());
                    fund_amount = Convert.ToDouble(dr["AMOUNT"].ToString());
                    CalcPercentTypeRadioGroup.SelectedIndex = Convert.ToInt32(dr["PERCENT_TYPE"].ToString());

                    if (!String.IsNullOrWhiteSpace(dr["LAST_DATE"].ToString()))
                    {
                        LastDateText.Visible = LastDateLabel.Visible = true;
                        LastDateText.Text = dr["LAST_DATE"].ToString().Substring(0, 10);
                        Debt = Convert.ToDouble(dr["LAST_DEBT"].ToString());
                        DebtValue.EditValue = Debt;
                    }
                    else
                    {
                        LastDateText.Visible = LastDateLabel.Visible = false;
                        LastDateText.Text = ContractStartDateText.Text;
                        Debt = fund_amount;
                        DebtValue.EditValue = Debt;
                    }

                    BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                    AppointmentLookUp.EditValue = AppointmentLookUp.Properties.GetKeyValueByDisplayText(dr["APPOINTMENT_NAME"].ToString());
                }

                one_day_interest = Math.Round(((Debt * (double)percent) / 100) / 360, 2);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void ComponentEnable(bool status)
        {
            PaymentDate.Enabled =
                CalcPercentTypeRadioGroup.Enabled =
                BankLookUp.Enabled =
                AppointmentLookUp.Enabled =
                PaymentValue.Enabled =
                PaymentAZNValue.Enabled =
                CurrencyRateValue.Enabled =
                ManualInterestCheckEdit.Enabled =
                PaymentInterestAmountValue.Enabled =
                BasicAmountValue.Enabled = !status;
        }

        private void PaymentValue_EditValueChanged(object sender, EventArgs e)
        {
            PaymentAZNValue.Value = (decimal)((double)PaymentValue.Value * (double)CurrencyRateValue.Value);
            if (cur != (double)CurrencyRateValue.Value)
                CurrencyRateValue.BackColor = Color.GreenYellow;
            else
                CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();
            BasicAmountValue.Enabled = (ManualInterestCheckEdit.Checked && PaymentValue.Value != 0);
            PaymentInterestAmountValue_EditValueChanged(sender, EventArgs.Empty);
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
                    GlobalProcedures.FillLookUpEdit(AppointmentLookUp, "BANK_APPOINTMENTS", "ID", "NAME", "OPERATION_TYPE_ID = 2 AND APPOINTMENT_TYPE_ID = 4 ORDER BY NAME");
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

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void AppointmentLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AppointmentLookUp.EditValue == null)
                return;

            appointment_id = Convert.ToInt32(AppointmentLookUp.EditValue);
        }

        private void AppointmentLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 12);
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);
        }

        private void PaymentDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                if ((!String.IsNullOrEmpty(PaymentDate.Text)) && (!String.IsNullOrEmpty(LastDateText.Text)))
                {
                    if (CurrencyID != 1)
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = true;
                        cur = GlobalFunctions.CurrencyLastRate(CurrencyID, PaymentDate.Text);
                        CurrencyRateLabel.Text = "1 " + Currency + " = ";
                        CurrencyRateValue.Value = (decimal)cur;
                    }
                    else if (CurrencyID == 1)
                        cur = 1;

                    List<FundContractPercent> lstPercent = FundContractPercentDAL.SelectFundContractPercentByContractID(int.Parse(ContractID)).ToList<FundContractPercent>();
                    if(lstPercent.Count > 0)
                    {
                        int lastPercentID = lstPercent.Where(item => item.PDATE <= PaymentDate.DateTime).Max(item => item.ID);
                        percent = lstPercent.Find(item => item.ID == lastPercentID).PERCENT_VALUE;                         
                    }

                    InterestText.Text = percent.ToString();
                    one_day_interest = Math.Round(((Debt * (double)percent) / 100) / 360, 2);
                    OneDayInterestValue.EditValue = one_day_interest;

                    diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy"), PaymentDate.DateTime);
                    DayCountText.Text = diff_day.ToString();
                    interest_amount = diff_day * one_day_interest;
                    residual_percent = interest_amount + GlobalFunctions.GetAmount($@"SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}");
                    buy_amount = GlobalFunctions.GetAmount($@"SELECT BUY_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND CP.PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID}");
                    PaymentInterestDebtValue.EditValue = residual_percent;
                    totaldebtamount = Math.Round((Debt + residual_percent), 2);
                    if (totaldebtamount < (double)DebtValue.Value)
                        totaldebtamount = (double)DebtValue.Value;
                    TotalDebtValue.EditValue = totaldebtamount;
                    PaymentInterestAmountValue.Value = (decimal)(interest_amount + PaymentInterestDebt);
                    PaymentValue_EditValueChanged(sender, EventArgs.Empty);
                    PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
                }
            }
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

            if (PaymentValue.Value <= 0 && !ManualInterestCheckEdit.Checked)
            {
                PaymentValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödənişin məbləği sıfırdan böyük olmalıdır.");
                PaymentValue.Focus();
                PaymentValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PaymentAZNValue.Value < 0)
            {
                CurrencyRateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(PaymentDate.Text + " tarixi üçün " + CurrencyLabel.Text + " valyutasının AZN qarşılığı daxil edilməyib.");
                CurrencyRateValue.Focus();
                CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertPayment()
        {
            double payment_interest_amount = 0, currenct_payment_interest_debt = 0, basic_amount = 0, current_debt = 0, total = 0, payment_value_AZN = 0;
            int manual_interest = 0;
            if (!ManualInterestCheckEdit.Checked)
            {
                if (PaymentValue.Value > 0 && CalcPercentTypeRadioGroup.SelectedIndex == 0) // eger odenis sifirdan boyuk olarsa ve faiz silinmesi secilerse
                {
                    if ((interest_amount + PaymentInterestDebt) > (double)PaymentValue.Value) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                        payment_interest_amount = (double)PaymentValue.Value;
                    else
                        payment_interest_amount = interest_amount + PaymentInterestDebt; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
                }
                else
                    payment_interest_amount = 0;
            }
            else
                payment_interest_amount = (double)PaymentInterestAmountValue.Value;

            currenct_payment_interest_debt = PaymentInterestDebt + interest_amount - payment_interest_amount;
            basic_amount = (double)PaymentValue.Value - payment_interest_amount;
            basic_amount = (basic_amount < 0)? 0 : basic_amount;
            current_debt = buy_amount + Debt - basic_amount;
            total = current_debt + currenct_payment_interest_debt;
            if (CurrencyID == 1)
                payment_value_AZN = (double)PaymentValue.Value;
            else
                payment_value_AZN = (double)PaymentAZNValue.Value;

            if (ManualInterestCheckEdit.Checked)
                manual_interest = 1;

            PaymentID = GlobalFunctions.InsertQuery($@"INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,IS_CHANGE,MANUAL_INTEREST,PERCENT_TYPE,PERCENT)VALUES(FUNDS_PAYMENTS_SEQUENCE.NEXTVAL," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY'),0," + Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + diff_day + "," + Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + NoteText.Text.Trim() + "'," + GlobalVariables.V_UserID + ",1," + manual_interest + "," + CalcPercentTypeRadioGroup.SelectedIndex + "," + Math.Round(Convert.ToDecimal(InterestText.Text),2).ToString(GlobalVariables.V_CultureInfoEN) + ") RETURNING ID INTO :ID",
                                            "Ödəniş temp cədvələ daxil edilmədi.",
                                            "ID",
                                            this.Name + "/InsertPayment");
        }

        private void UpdatePayment()
        {
            double payment_interest_amount = 0, currenct_payment_interest_debt = 0, basic_amount = 0, current_debt = 0, total = 0, payment_value_AZN = 0;
            int manual_interest = 0;
            if (!ManualInterestCheckEdit.Checked)
            {
                if (PaymentValue.Value > 0 && CalcPercentTypeRadioGroup.SelectedIndex == 0) // eger odenis sifirdan boyuk olarsa
                {
                    if ((interest_amount + PaymentInterestDebt) > (double)PaymentValue.Value) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                        payment_interest_amount = (double)PaymentValue.Value;
                    else
                        payment_interest_amount = interest_amount + PaymentInterestDebt; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
                }
                else
                    payment_interest_amount = 0;
            }
            else
                payment_interest_amount = (double)PaymentInterestAmountValue.Value;

            currenct_payment_interest_debt = PaymentInterestDebt + interest_amount - payment_interest_amount;
            basic_amount = (double)PaymentValue.Value - payment_interest_amount;
            basic_amount = (basic_amount < 0) ? 0 : basic_amount;
            current_debt = buy_amount + Debt - basic_amount;
            total = current_debt + currenct_payment_interest_debt;
            if (CurrencyID == 1)
                payment_value_AZN = (double)PaymentValue.Value;
            else
                payment_value_AZN = (double)PaymentAZNValue.Value;

            if (ManualInterestCheckEdit.Checked)
                manual_interest = 1;

            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET PAYMENT_AMOUNT = " + Math.Round(PaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",BASIC_AMOUNT = " + Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",PAYMENT_INTEREST_AMOUNT = " + Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",PAYMENT_INTEREST_DEBT = " + Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",CURRENCY_RATE = " + Math.Round(CurrencyRateValue.Value, 4).ToString(GlobalVariables.V_CultureInfoEN) + ",PAYMENT_AMOUNT_AZN = " + Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",REQUIRED_CLOSE_AMOUNT = " + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "',IS_CHANGE = 1,MANUAL_INTEREST = " + manual_interest + ",PERCENT_TYPE = " + CalcPercentTypeRadioGroup.SelectedIndex + ",PERCENT = " + Math.Round(Convert.ToDecimal(InterestText.Text), 2).ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + PaymentID + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                             "Ödəniş temp cədvəldə dəyişdirilmədi.",
                                          this.Name + "/UpdatePayment");
        }

        private void InsertBankOperationTemp()
        {
            if (PaymentID <= 0 || SourceID != 6)
                return;

            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,IS_CHANGE,USED_USER_ID,FUNDS_PAYMENT_ID,FUNDS_CONTRACT_ID,CONTRACT_CODE) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + bank_id + ",TO_DATE('" + PaymentDate.Text.Trim() + "','DD/MM/YYYY')," + appointment_id + ",0," + Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,'" + NoteText.Text.Trim() + "',1," + GlobalVariables.V_UserID + "," + PaymentID + "," + ContractID + ",'" + ContractCode + "')",
                                                "Bank əməliyyatlarının məxarici daxil edilmədi.",
                                           this.Name + "/InsertBankOperationTemp");
        }

        private void UpdateBankOperationTemp()
        {
            if (SourceID != 6)
                return;

            if (PaymentValue.Value > 0)
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET EXPENSES = {Math.Round(PaymentAZNValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}',IS_CHANGE = 1,APPOINTMENT_ID = {appointment_id},BANK_ID = {bank_id} WHERE OPERATION_DATE = TO_DATE('{PaymentDate.Text.Trim()}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID} AND FUNDS_PAYMENT_ID = {PaymentID}",
                                                "Bank əməliyyatlarının məxarici dəyişdirilmədi.",
                                               this.Name + "/UpdateBankOperationTemp");
            else
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET IS_CHANGE = 2, APPOINTMENT_ID = {appointment_id}, BANK_ID = {bank_id} WHERE OPERATION_DATE = TO_DATE('{PaymentDate.Text.Trim()}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID} AND FUNDS_PAYMENT_ID = {PaymentID}",
                                                "Bank əməliyyatlarının məxarici silinmədi.",
                                              this.Name + "/UpdateBankOperationTemp");
        }

        private void InsertCashFounderTemp()
        {
            string founder_id = null, founder_card_id = null, s = null;
            double b = 0;

            s = "SELECT FOUNDER_ID,FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = " + ContractID;
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/InsertCashFounderTemp");
            if (dt.Rows.Count > 0)
            {
                founder_id = dt.Rows[0]["FOUNDER_ID"].ToString();
                founder_card_id = dt.Rows[0]["FOUNDER_CARD_ID"].ToString();
                b = (CurrencyID == 1) ? (double)PaymentValue.Value : (double)PaymentAZNValue.Value;

                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.CASH_FOUNDER_TEMP(ID,FOUNDER_ID,FOUNDER_CARD_ID,PAYMENT_DATE,APPOINTMENT,AMOUNT,INC_EXP,NOTE,USED_USER_ID,IS_CHANGE,FUND_PAYMENT_ID)VALUES(CASH_FOUNDER_SEQUENCE.NEXTVAL," + founder_id + "," + founder_card_id + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY'),'Təsisçi tərəfindən mədaxil'," + Math.Round(b, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",2,'" + NoteText.Text.Trim() + "'," + Class.GlobalVariables.V_UserID + ",1," + PaymentID + ")",
                                                            "Təsisçi ilə hesablaşmanın mədaxili temp cədvələ daxil olunmadı.",
                                              this.Name + "/InsertCashFounderTemp");
            }
        }

        private void UpdateCashFounderTemp()
        {
            string founder_id = null, founder_card_id = null, s = null;
            double b = 0;

            s = $@"SELECT FOUNDER_ID,FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FUNDS_CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/UpdateCashFounderTemp");
            if (dt.Rows.Count > 0)
            {
                founder_id = dt.Rows[0]["FOUNDER_ID"].ToString();
                founder_card_id = dt.Rows[0]["FOUNDER_CARD_ID"].ToString();

                if (CurrencyID == 1)
                    b = (double)PaymentValue.Value;
                else
                    b = (double)PaymentAZNValue.Value;

                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CASH_FOUNDER_TEMP SET IS_CHANGE = 1, AMOUNT = {Math.Round(b, 2).ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}' WHERE INC_EXP = 2 AND FOUNDER_ID = {founder_id} AND FOUNDER_CARD_ID = {founder_card_id} AND PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY')",
                                                            "Təsisçi ilə hesablaşmanın mədaxili temp cədvəldə dəyişdirilmədi.",
                                                         this.Name + "/UpdateCashFounderTemp");
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPaymentDetails())
            {
                if (TransactionName == "INSERT")
                {
                    InsertPayment();
                    InsertBankOperationTemp();
                    if (SourceID == 10)
                        InsertCashFounderTemp();
                }
                else
                {
                    UpdatePayment();
                    UpdateBankOperationTemp();
                    if (SourceID == 10)
                        UpdateCashFounderTemp();
                }
                GlobalProcedures.UpdateFundChange(PaymentDate.Text, int.Parse(ContractID), 1);
                this.Close();
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FPaymentAmountAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pay_count > 0)
                PaymentID = GlobalFunctions.GetID($@"SELECT ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE = TO_DATE('{PaymentDate.Text}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID}");
            this.RefreshPaymentsDataGridView(PaymentValue.Value, PaymentID);
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(PaymentDate.Text);
            if (CurrencyID != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(CurrencyID, PaymentDate.Text);
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

        private void ManualInterestCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            PaymentInterestAmountValue.Enabled = ManualInterestCheckEdit.Checked;
            BasicAmountValue.Enabled = (ManualInterestCheckEdit.Checked && PaymentValue.Value != 0);
            if (ManualInterestCheckEdit.Checked)
                PaymentInterestAmountValue.Focus();
            PaymentInterestAmountValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void PaymentInterestAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            if (ManualInterestCheckEdit.Checked)
            {
                decimal diff = PaymentValue.Value - PaymentInterestAmountValue.Value;
                BasicAmountValue.Value = (PaymentValue.Value == 0) ? 0 : diff;
            }
        }

        private void BasicAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            if (ManualInterestCheckEdit.Checked && PaymentValue.Value != 0)
                PaymentInterestAmountValue.Value = PaymentValue.Value - BasicAmountValue.Value;

            if (BasicAmountValue.Value < 0)
                BasicAmountValue.BackColor = Color.Red;
            else
                BasicAmountValue.BackColor = GlobalFunctions.ElementColor();
        }

        private void DebtValue_EditValueChanged(object sender, EventArgs e)
        {
            if (DebtValue.Value < 0)
                DebtValue.ForeColor = Color.Red;
            else
                DebtValue.ForeColor = GlobalFunctions.ElementColor();
        }

        private void PaymentInterestDebtValue_EditValueChanged(object sender, EventArgs e)
        {
            if (PaymentInterestDebtValue.Value < 0)
                PaymentInterestDebtValue.ForeColor = Color.Red;
            else
                PaymentInterestDebtValue.ForeColor = GlobalFunctions.ElementColor();
        }

        private void OneDayInterestValue_EditValueChanged(object sender, EventArgs e)
        {
            if (OneDayInterestValue.Value < 0)
                OneDayInterestValue.ForeColor = Color.Red;
            else
                OneDayInterestValue.ForeColor = GlobalFunctions.ElementColor();
        }

        private void TotalDebtValue_EditValueChanged(object sender, EventArgs e)
        {
            if (TotalDebtValue.Value < 0)
                TotalDebtValue.ForeColor = Color.Red;
            else
                TotalDebtValue.ForeColor = GlobalFunctions.ElementColor();
        }
    }
}