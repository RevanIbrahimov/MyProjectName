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
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using System.IO;
using System.Diagnostics;

namespace CRS.Forms.Commons
{
    public partial class FOrderAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FOrderAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CurrencyCode, ContractCode, CustomerName, CustomerVoen, CustomerAccount;
        public int CommonID, BankID, CurrencyID, OrderCount, ContractID;
        public int? ID;
        public decimal CommonAmount, TotalDebt;
        public DateTime CommonDate;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        decimal oldPayed = 0, Debt, sumPayed = 0;
        string paying_bank_name,
            paying_bank_voen,
            paying_bank_swift,
            paying_bank_code,
            paying_account,
            paying_bank_cbar_account,
            acceptor_bank_name,
            acceptor_bank_voen,
            acceptor_bank_swift,
            acceptor_bank_code,
            acceptor_account,
            acceptor_bank_cbar_account;

        private void DateValue_EditValueChanged(object sender, EventArgs e)
        {
            sumPayed = (decimal)GlobalFunctions.GetAmount($@"SELECT NVL(SUM(PAYMENT_AMOUNT),0) FROM CRS_USER.CUSTOMER_PAYMENTS WHERE CONTRACT_ID = {ContractID} AND PAYMENT_DATE < TO_DATE('{DateValue.Text}','DD.MM.YYYY')");
            Debt = TotalDebt - sumPayed;
        }

        int acceptor_bank_id = 0;

        List<Banks> lstBanks = null;

        private void FOrderAddEdit_Load(object sender, EventArgs e)
        {
            AmountCurrencyLabel.Text = CurrencyCode;
            DateValue.Properties.MinValue = CommonDate;
            
            lstBanks = BanksDAL.SelectBankByID(null).ToList<Banks>();
            RefreshDictionaries(11);
            LoadPayingBankDetails();
            if (TransactionName == "EDIT")
                LoadDetails();
            else
            {
                CodeText.Text = (OrderCount + 1).ToString().PadLeft(3, '0');
                DateValue.EditValue = DateTime.Today;
                AmountValue.Focus();
            }
        }

        private void LoadDetails()
        {
            string sql = $@"SELECT CO.ORDER_DATE PDATE,
                                   CO.ORDER_NUMBER,
                                   CO.AMOUNT,
                                   CO.PAYING_BANK_ID,
                                   CO.ACCEPT_BANK_ID,
                                   CO.NOTE,
                                   B.LONG_NAME BANK_NAME
                              FROM CRS_USER_TEMP.CONTRACT_ORDER_TEMP CO, CRS_USER.BANKS B
                             WHERE CO.ACCEPT_BANK_ID = B.ID AND CO.ID = {ID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Tələbnamə açılmadı.");

            if (dt.Rows.Count > 0)
            {
                DateValue.EditValue = dt.Rows[0]["PDATE"];
                CodeText.Text = dt.Rows[0]["ORDER_NUMBER"].ToString();
                oldPayed = Convert.ToDecimal(dt.Rows[0]["AMOUNT"]);
                AmountValue.EditValue = oldPayed;
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                GlobalProcedures.LookUpEditValue(AcceptorBankLookUp, dt.Rows[0]["BANK_NAME"].ToString());
            }
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (DateValue.Text.Length == 0)
            {
                DateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix seçilməyib.");
                DateValue.Focus();
                DateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tələbnamənin nömrəsi daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
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

            if (AmountValue.Value > Debt)
            {
                AmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğ maksimum " + Debt + $@" {CurrencyCode} olmalıdır.");
                AmountValue.Focus();
                AmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (acceptor_bank_id == 0)
            {
                AcceptorBankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Alan tərəfin bankı seçilməyib.");
                AcceptorBankLookUp.Focus();
                AcceptorBankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void FOrderAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void Insert()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_ORDER_TEMP (ID,
                                                   CONTRACT_COMMON_ID,
                                                   ORDER_DATE,
                                                   ORDER_NUMBER,
                                                   AMOUNT,
                                                   PAYING_BANK_ID,
                                                   ACCEPT_BANK_ID,
                                                   NOTE,
                                                   IS_CHANGE,
                                                   USED_USER_ID,
                                                   INSERT_USER)
                            VALUES(CRS_USER.CONTRACT_ORDER_SEQUENCE.NEXTVAL,
                                    {CommonID},
                                    TO_DATE('{DateValue.Text}','DD.MM.YYYY'),
                                    '{CodeText.Text.Trim()}',
                                    {Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                
                                    {BankID},
                                    {acceptor_bank_id},
                                    '{NoteText.Text.Trim()}',
                                    1,
                                    {GlobalVariables.V_UserID},
                                    {GlobalVariables.V_UserID})";
            GlobalProcedures.ExecuteQuery(sql, "Tələbnamə temp cədvələ daxil edilmədi.", this.Name + "/Insert");
        }

        private void Update()
        {
            string sql = $@"UPDATE CRS_USER_TEMP.CONTRACT_ORDER_TEMP SET
                                                   ORDER_DATE = TO_DATE('{DateValue.Text}','DD.MM.YYYY'),
                                                   ORDER_NUMBER = '{CodeText.Text.Trim()}',
                                                   AMOUNT = {Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                   PAYING_BANK_ID = {BankID},
                                                   ACCEPT_BANK_ID = {acceptor_bank_id},
                                                   NOTE = '{NoteText.Text.Trim()}',
                                                   IS_CHANGE = 1,
                                                   UPDATE_USER = {GlobalVariables.V_UserID},
                                                   UPDATE_DATE = SYSDATE
                            WHERE ID = {ID}";
            GlobalProcedures.ExecuteQuery(sql, "Tələbnamə temp cədvəldə dəyişdirilmədi.", this.Name + "/Update");
        }

        private void AcceptorBankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 11:
                    {
                        GlobalProcedures.FillLookUpEdit(AcceptorBankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                    }
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

        private void AcceptorBankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AcceptorBankLookUp.EditValue == null)
                return;

            acceptor_bank_id = Convert.ToInt32(AcceptorBankLookUp.EditValue);
            LoadAcceptorBankDetails();
        }

        private void LoadAcceptorBankDetails()
        {
            if (acceptor_bank_id == 0)
                return;

            var acceptor_bank = lstBanks.Find(b => b.ID == acceptor_bank_id);
            acceptor_bank_name = acceptor_bank.LONG_NAME;
            acceptor_bank_code = acceptor_bank.CODE;
            acceptor_bank_swift = acceptor_bank.SWIFT;
            acceptor_bank_voen = acceptor_bank.VOEN;
            acceptor_bank_cbar_account = acceptor_bank.CBAR_ACCOUNT;
            acceptor_account = acceptor_bank.ACCOUNT;
        }

        private void LoadPayingBankDetails()
        {
            if (BankID == 0)
                return;

            var bank = lstBanks.Find(b => b.ID == BankID);
            paying_bank_name = bank.LONG_NAME;
            paying_bank_swift = bank.SWIFT;
            paying_bank_voen = bank.VOEN;
            paying_bank_cbar_account = bank.CBAR_ACCOUNT;
            paying_account = bank.ACCOUNT;
            paying_bank_code = bank.CODE;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                if (FileCheck.Checked)
                    GenerateFile();
                this.Close();
            }
        }

        private void GenerateFile()
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            object fileName = null, saveAs;
            string currency_name;

            fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Tələbnamə.docx");
            saveAs = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Order_" + ContractCode.Replace(@"/", null) + ".docx");

            //List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(1).ToList<Currency>();
            //currency_name = lstCurrency.First().NAME;

            string date = GlobalFunctions.DateWithDayMonthYear(DateValue.DateTime);
            string amountWithWord = GlobalFunctions.AmountInWritining((double)AmountValue.Value, 1, true);
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            decimal debt = Debt - AmountValue.Value;
            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;
            try
            {
                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                aDoc.Activate();

                GlobalProcedures.FindAndReplace(wordApp, "[$code]", CodeText.Text.Trim());
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCode);
                GlobalProcedures.FindAndReplace(wordApp, "[$currency]", "AZN");
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerName);
                GlobalProcedures.FindAndReplace(wordApp, "[$customervoen]", CustomerVoen);
                GlobalProcedures.FindAndReplace(wordApp, "[$customerbankaccount]", CustomerAccount);
                GlobalProcedures.FindAndReplace(wordApp, "[$bankaccount]", paying_account);
                GlobalProcedures.FindAndReplace(wordApp, "[$payingbankname]", paying_bank_name);
                GlobalProcedures.FindAndReplace(wordApp, "[$payingbankcode]", paying_bank_code);
                GlobalProcedures.FindAndReplace(wordApp, "[$payingbankvoen]", paying_bank_voen);
                GlobalProcedures.FindAndReplace(wordApp, "[$payingbankmbaccount]", paying_bank_cbar_account);
                GlobalProcedures.FindAndReplace(wordApp, "[$payingbankswift]", paying_bank_swift);
                GlobalProcedures.FindAndReplace(wordApp, "[$acceptbankname]", acceptor_bank_name);
                GlobalProcedures.FindAndReplace(wordApp, "[$acceptbankcode]", acceptor_bank_code);
                GlobalProcedures.FindAndReplace(wordApp, "[$acceptbankvoen]", acceptor_bank_voen);
                GlobalProcedures.FindAndReplace(wordApp, "[$acceptbankswift]", acceptor_bank_swift);
                GlobalProcedures.FindAndReplace(wordApp, "[$acceptbankmbaccount]", acceptor_bank_cbar_account);
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", AmountValue.Value.ToString("n2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$amountbywrite]", amountWithWord);
                GlobalProcedures.FindAndReplace(wordApp, "[$commondate]", CommonDate.ToString("dd.MM.yyyy"));
                GlobalProcedures.FindAndReplace(wordApp, "[$commonamount]", CommonAmount.ToString("n2") + " AZN");
                GlobalProcedures.FindAndReplace(wordApp, "[$debt]", debt.ToString("n2") + " AZN");

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                if (File.Exists((string)saveAs))
                    Process.Start((string)saveAs);
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.LogWrite(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Order_" + ContractCode.Replace(@"/", null) + ".docx faylı açılmadı", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            finally
            {
                aDoc.Close(ref missing, ref missing, ref missing);
                wordApp.Quit();
                GlobalProcedures.SplashScreenClose();
            }
        }
    }
}