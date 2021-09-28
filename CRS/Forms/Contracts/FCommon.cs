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
using CRS.Class;
using Bytescout.Document;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CRS.Forms.Contracts
{
    public partial class FCommon : DevExpress.XtraEditors.XtraForm
    {
        public FCommon()
        {
            InitializeComponent();
        }
        public int ContractID, CustomerType, PersonTypeID, CustomerID, CurrencyID;
        public DateTime ContractStartDate;
        public string CustomerName, Account, Voen, ContractCode, CurrencyCode;
        public decimal ContractAmount;

        int bankID = 0;
        double currencyRate = 1;

        private void PayedAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            DebtValue.EditValue = ContractAmountValue.Value - PayedAmountValue.Value;

            AmountValue.EditValue = Math.Round((double)DebtValue.Value * currencyRate, 2);
        }

        private void PaymentScheduleLabel_Click(object sender, EventArgs e)
        {
            Total.FPaymentSchedules fps = new Total.FPaymentSchedules();
            fps.ContractID = ContractID.ToString();
            fps.Amount = ContractAmount.ToString("N2") + " " + CurrencyCode;
            fps.ContractCode = ContractCode;
            fps.ShowDialog();
        }

        private void PayedLabel_Click(object sender, EventArgs e)
        {
            Bookkeeping.FShowPayments fsp = new Bookkeeping.FShowPayments();
            fsp.ContractID = ContractID.ToString();
            fsp.ShowDialog();
        }

        private void AmountValue_EditValueChanged(object sender, EventArgs e)
        {
            TotalAmountValue.EditValue = AmountValue.Value + TempAmountValue.Value;
        }

        private void DateValue_EditValueChanged(object sender, EventArgs e)
        {
            currencyRate = CurrencyID != 1? GlobalFunctions.CurrencyLastRate(CurrencyID, DateValue.Text) : 1;
            CurrencyRateValue.EditValue = currencyRate;

            PayedAmountValue.EditValue = GlobalFunctions.GetAmount($@"SELECT NVL(ROUND(SUM (PAYMENT_AMOUNT), 2), 0) PAYMENT_AMOUNT
                                                                         FROM CRS_USER.CUSTOMER_PAYMENTS
                                                                        WHERE     CONTRACT_ID = {ContractID}
                                                                              AND PAYMENT_DATE <= TO_DATE ('{DateValue.Text}', 'DD.MM.YYYY')");            
        }

        private void FCommon_Load(object sender, EventArgs e)
        {
            RefreshDictionaries(11);
            ContractAmountValue.EditValue = GlobalFunctions.GetAmount($@"SELECT NVL (ROUND (SUM (MONTHLY_PAYMENT), 2), 0) CONTRACT_AMOUNT                                                                              
                                                                         FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP PS
                                                                        WHERE CONTRACT_ID = {ContractID} AND VERSION = (SELECT MAX(VERSION) FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP
                                                                                                                                WHERE CONTRACT_ID = PS.CONTRACT_ID)", this.Name + "/FCommon_Load");
            DateValue.EditValue = DateTime.Today;
            DateValue.Properties.MinValue = ContractStartDate;
            CustomerNameText.Text = CustomerName;
            VoenText.Text = Voen;
            AccountText.Text = Account;
            CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = CurrencyID != 1;
            if (CurrencyID != 1)
                CurrencyRateLabel.Text = "1 " + CurrencyCode + " = ";
            ContractAmountCurrencyLabel.Text = PayedAmountCurrencyLabel.Text = DebtCurrencyLabel.Text = CurrencyCode;
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bankID = Convert.ToInt32(BankLookUp.EditValue);
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

                int code_number = int.Parse(Regex.Replace(ContractCode, "[^0-9]", ""));

                try
                {
                    Document document = new Document();
                    document.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\Sərəncam.docx");
                    document.ReplaceText("[$date]", GlobalFunctions.DateWithDayMonthYear(DateValue.DateTime));
                    document.ReplaceText("[$customername]", CustomerName);
                    document.ReplaceText("[$contractcode]", ContractCode);
                    document.ReplaceText("[$voen]", Voen);
                    document.ReplaceText("[$bankname]", BankLookUp.Text);
                    document.ReplaceText("[$account]", Account);
                    document.ReplaceText("[$amount]", TotalAmountValue.Value.ToString("N2"));

                    if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Sərəncam_" + code_number + ".docx"))
                        File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Sərəncam_" + code_number + ".docx");
                    document.Save(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Sərəncam_" + code_number + ".docx");
                    document.Dispose();

                    decimal totalDebt = Math.Round(ContractAmountValue.Value * (decimal)currencyRate, 2);
                    string sql1, sql2;
                    sql1 = $@"UPDATE CRS_USER.CONTRACT_COMMON CC SET STATUS_ID = 19 WHERE CONTRACT_ID = {ContractID} AND ID = (SELECT MAX(ID) FROM CRS_USER.CONTRACT_COMMON WHERE CONTRACT_ID = CC.CONTRACT_ID)";
                    sql2 = $@"INSERT INTO CRS_USER.CONTRACT_COMMON(CONTRACT_ID,BANK_ID,AMOUNT,TEMP_AMOUNT,COMMON_DATE,CUSTOMER_NAME,VOEN,ACCOUNT_NUMBER,TOTAL_DEBT,CUSTOMER_ID,CUSTOMER_TYPE,PERSON_TYPE_ID,COMMON_FILE,INSERT_USER)
                                  VALUES({ContractID},{bankID},{AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},{TempAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},TO_DATE('{DateValue.Text}','DD.MM.YYYY'),'{CustomerName}','{Voen}','{Account}',{totalDebt.ToString(GlobalVariables.V_CultureInfoEN)},{CustomerID},{CustomerType},{PersonTypeID},:BlobFile,{GlobalVariables.V_UserID})";

                    GlobalFunctions.ExecuteTwoQueryWithBlob(sql1, sql2, GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Sərəncam_" + code_number + ".docx", "Sərəncamın faylı bazaya daxil edilmədi.");

                    this.Close();
                    Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Sərəncam_" + code_number + ".docx");
                }
                catch
                {
                    GlobalProcedures.SplashScreenClose();
                    GlobalProcedures.ShowErrorMessage("Sərəncam_" + code_number + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
                }
                finally
                {
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(DateValue.Text))
            {
                DateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix seçilməyib.");
                DateValue.Focus();
                DateValue.BackColor = GlobalFunctions.ElementColor();
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

            if (bankID == 0)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank seçilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;            

            return b;
        }


    }
}