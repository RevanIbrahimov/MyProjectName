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
using CRS.Class;
using CRS.Class.DataAccess;
using System.Collections;
using CRS.Class.Views;
using System.IO;
using System.Diagnostics;

namespace CRS.Forms.Contracts
{
    public partial class FJuridicalCommitment : DevExpress.XtraEditors.XtraForm
    {
        public FJuridicalCommitment()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName,
            CommitmentID,
            Customer,
            CustomerName,
            CustomerCode,
            CustomerPhone,
            CustomerAddress,
            CustomerCard,
            ContractCode,
            Account,
            ContractStartDate,
            ContractEndDate,
            CustomerID,
            ContractID,
            Lizinq,
            Currency,
            CreditName,
            DebtCurrency,
            LetterHostage,
            CarNumber,
            Voen;

        public int Interest,
            Period,
            CreditNameID,
            Count,
            LiquidCurrencyID,
            DebtCurrencyID,
            ParentID,
            CustomerTypeID,
            ParentPersonTypeID;

        public decimal Debt,
            Amount,
            LiquidValue;



        public DateTime AgreementDateMinValue;

        public delegate void DoEvent();
        public event DoEvent RefreshCommitmentsDataGridView;

        decimal currecnt_debt, interest_debt = 0;
        string PhoneID, PhoneNumber, copy_file_name, letterNumber, referenceNumber, letterCustomerName, customerDrivingLicense, letterCommitmentName, commitmentDrivingLicense;

        private void AccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(AccountText, AccountLengthLabel);
        }

        private void CheckInterestDebt_CheckedChanged(object sender, EventArgs e)
        {
            TotalDebtValue.EditValue = DebtValue.Value + (CheckInterestDebt.Checked ? InterestDebtValue.Value : 0);
        }

        static int file_count = 0;
        bool letterDetailsOK = false, FormStatus = false;

        private void AgreementDate_EditValueChanged(object sender, EventArgs e)
        {
            RefreshTotals();
        }

        private void InsertPhonesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_JUR_COMMIT_PHONE_TEMP", "P_CONTRACT_ID", int.Parse(ContractID), "P_COMMITMENT_ID", int.Parse(CommitmentID), "Öhdəlik götürənin telefonları temp cədvələ daxil edilmədi.");
        }

        public void LoadFPhoneAddEdit(string transaction, string OwnerID, string OwnerType, string PhoneID)
        {
            FPhoneAddEdit fp = new FPhoneAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerID = OwnerID;
            fp.OwnerType = OwnerType;
            fp.PhoneID = PhoneID;
            fp.RefreshPhonesDataGridView += new FPhoneAddEdit.DoEvent(LoadPhoneDataGridView);
            fp.ShowDialog();
        }

        private void CustomerPaymentsLabel_Click(object sender, EventArgs e)
        {
            LoadFPayment(ContractID, ContractCode, Interest, Period, ContractStartDate, ContractEndDate, Lizinq, CustomerCode, CustomerName, CustomerID, (double)Amount, DebtCurrency);
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PopupMenu, e);
        }

        void RefreshTotals()
        {
            //currecnt_debt = (decimal)GlobalFunctions.GetAmount($@"SELECT TOTAL FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CONTRACT_ID = {ContractID} AND PAYMENT_DATE = (SELECT MAX(PAYMENT_DATE) FROM CRS_USER.CUSTOMER_PAYMENTS WHERE CONTRACT_ID = CP.CONTRACT_ID)");
            //if (currecnt_debt == 0)
            //    DebtValue.Value = Debt;
            //else
            //    DebtValue.Value = currecnt_debt;

            if (!FormStatus)
                return;

            string sql = $@"SELECT T.DEBT,
                                         TOTALS.PAYMENT_INTEREST_DEBT (T.P_STARTDATE,
                                                                       T.P_ENDDATE,
                                                                       T.DEBT,
                                                                       T.INTEREST,
                                                                       T.CONTRACT_ID,
                                                                       T.PAYMENT_INTEREST_AMOUNT)
                                            PAYMENT_INTEREST_DEBT
                                    FROM (WITH T1
                                               AS (SELECT CON.INTEREST,                          
                                                          CON.START_DATE,
                                                          CON.AMOUNT,  
                                                          CON.CONTRACT_ID,
                                                          CON.CUSTOMER_ID,
                                                          CON.CUSTOMER_TYPE_ID                          
                                                     FROM CRS_USER.V_CONTRACTS CON,                          
                                                          CRS_USER.V_CUSTOMERS CUS                          
                                                    WHERE     CON.IS_COMMIT = 1
                                                          AND CON.CUSTOMER_ID = CUS.ID
                                                          AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID                                                 
                                                          AND CON.START_DATE <=
                                                                 TO_DATE ('{AgreementDate.Text}', 'DD/MM/YYYY')),
                                               T2
                                               AS (  SELECT CP.CUSTOMER_ID,
                                                            CP.CONTRACT_ID,
                                                            CP.CUSTOMER_TYPE_ID,
                                                            SUM (CP.BASIC_AMOUNT) BASIC_AMOUNT,
                                                            SUM (CP.PAYMENT_INTEREST_AMOUNT)
                                                               PAYMENT_INTEREST_AMOUNT,
                                                            MAX (CP.PAYMENT_DATE) PAYMENT_DATE
                                                       FROM CRS_USER.CUSTOMER_PAYMENTS CP
                                                      WHERE CP.PAYMENT_DATE <=
                                                               TO_DATE ('{AgreementDate.Text}', 'DD/MM/YYYY')
                                                   GROUP BY CP.CUSTOMER_ID,
                                                            CP.CONTRACT_ID,
                                                            CP.CUSTOMER_TYPE_ID)
                                          SELECT T1.INTEREST,  
                                                 T1.AMOUNT - NVL (T2.BASIC_AMOUNT, 0) DEBT,
                                                 NVL (T2.PAYMENT_INTEREST_AMOUNT, 0) PAYMENT_INTEREST_AMOUNT,                 
                                                 T1.CONTRACT_ID,                 
                                                 (CASE
                                                     WHEN T2.PAYMENT_DATE IS NULL THEN T1.START_DATE
                                                     ELSE T2.PAYMENT_DATE
                                                  END)
                                                    P_STARTDATE,
                                                 TO_DATE ('{AgreementDate.Text}', 'DD/MM/YYYY') P_ENDDATE                 
                                            FROM T1
                                                 LEFT JOIN T2
                                                    ON     T1.CUSTOMER_ID = T2.CUSTOMER_ID
                                                       AND T1.CONTRACT_ID = T2.CONTRACT_ID
                                                       AND T1.CUSTOMER_TYPE_ID = T2.CUSTOMER_TYPE_ID) T
                                        WHERE CONTRACT_ID = {ContractID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/FExtendTime_Load", "Qalıq tapılmadı.");
            if (dt.Rows.Count > 0)
            {
                currecnt_debt = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                interest_debt = Convert.ToDecimal(dt.Rows[0]["PAYMENT_INTEREST_DEBT"]);
            }

            DebtValue.Value = (currecnt_debt == 0) ? Debt : currecnt_debt;
            InterestDebtValue.Value = interest_debt;
        }

        private void LoadFPayment(string contract_id, string contract_code, int interest, int period, string s_date, string e_date, string lizinq, string customer_code, string customer_name, string customer_id, double amount, string currency)
        {
            Total.FPayment fp = new Total.FPayment();
            fp.ContractID = contract_id;
            fp.ContractCode = contract_code;
            fp.Interest = interest;
            fp.Period = period;
            fp.SDate = s_date;
            fp.EDate = e_date;
            fp.Lizinq = lizinq;
            fp.CustomerID = customer_id;
            fp.CustomerCode = customer_code;
            fp.CustomerName = customer_name;
            fp.Amount = amount;
            fp.Currency = currency;
            fp.CustomerTypeID = CustomerTypeID;
            fp.RefreshTotalsDataGridView += new Total.FPayment.DoEvent(RefreshTotals);
            fp.ShowDialog();
        }

        private void FJuridicalCommitment_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshCommitmentsDataGridView();
        }

        private void VoenText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(VoenText, VoenLengthLabel);
        }

        private void NewPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("INSERT", CommitmentID, "CC", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", CommitmentID, "CC", PhoneID);
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled)
                LoadFPhoneAddEdit("EDIT", CommitmentID, "CC", PhoneID);
        }

        private void RefreshPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDataGridView();
        }

        private void DeletePhone()
        {
            DialogResult dialogResult = XtraMessageBox.Show(PhoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'CC' AND ID = {PhoneID}", "Telefon nömrəsi temp cədvəldən silinmədi.");
            }
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
            LoadPhoneDataGridView();
        }

        private void UpPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "up", out orderid);
            LoadPhoneDataGridView();
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "down", out orderid);
            LoadPhoneDataGridView();
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void PhoneGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PhoneGridView.GetFocusedDataRow();
            if (row != null)
            {
                PhoneID = row["ID"].ToString();
                PhoneNumber = row["PHONE_NUMBER"].ToString();
                UpPhoneBarButton.Enabled = !(PhoneGridView.FocusedRowHandle == 0);
                DownPhoneBarButton.Enabled = !(PhoneGridView.FocusedRowHandle == PhoneGridView.RowCount - 1);
            }
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(NameText.Text.Trim()))
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Öhdəlik götürənin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(LeadingPersonNameText.Text.Trim()))
            {
                LeadingPersonNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Rəhbər şəxsin adı daxil edilməyib.");
                LeadingPersonNameText.Focus();
                LeadingPersonNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(AddressText.Text.Trim()))
            {
                AddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin ünvanı daxil edilməyib.");
                AddressText.Focus();
                AddressText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin vöen-i daxil edilməyib.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length > 0 && !GlobalFunctions.Regexp("[0-9]{9}1", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>1</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (AccountText.Text.Length > 0 && AccountText.Text.Length != 28)
            {
                AccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin hesab nömrəsi 28 simvol olmalıdır.");
                AccountText.Focus();
                AccountText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(AddressText.Text))
            {
                AddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ünvan daxil edilməyib.");
                AddressText.Focus();
                AddressText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PhoneGridView.RowCount == 0)
            {
                PhoneGridControl.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Telefon nömrələri daxil edilməyib.");
                PhoneGridControl.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DebtValue.Value <= 0)
            {
                DebtValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qalıq borc sıfırdan böyük olmalıdır.");
                DebtValue.Focus();
                DebtValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(PeriodDate.Text))
            {
                PeriodDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinq müddəti daxil edilməyib.");
                PeriodDate.Focus();
                PeriodDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PrizeInterestValue.Value <= 0)
            {
                PrizeInterestValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinq verənin mükafatı daxil edilməyib.");
                PrizeInterestValue.Focus();
                PrizeInterestValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ServiceAmountValue.Value < 0)
            {
                ServiceAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Xidmət haqqı mənfi ədəd ola bilməz.");
                ServiceAmountValue.Focus();
                ServiceAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        void GetLetterDetails(string number, string number2, string customerName, string drivingLicense1, string commitmentName, string drivingLicense2, bool cancel)
        {
            letterNumber = number;
            referenceNumber = number2;
            letterCustomerName = customerName;
            customerDrivingLicense = drivingLicense1;
            letterCommitmentName = commitmentName;
            commitmentDrivingLicense = drivingLicense2;
            letterDetailsOK = cancel;
        }

        private void LoadLetterDetails()
        {
            FCommitmentLetterDetails fl = new FCommitmentLetterDetails();
            fl.CustomerName = CustomerNameText.Text.Trim();
            fl.CommitmentName = NameText.Text.Trim();
            fl.GetDetails += new FCommitmentLetterDetails.DoEvent(GetLetterDetails);
            fl.ShowDialog();
        }

        private void BCreateFile_Click(object sender, EventArgs e)
        {
            object fileName = null, saveAs;

            if (CreditNameID == 5)
            {
                fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Öhdəlik ötürmə.docx");
                letterDetailsOK = true;
            }
            else if (CreditNameID == 1 && ApplicantionCheck.Checked)
            {
                LoadLetterDetails();
                fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Öhdəlik ötürmə (məktub ilə).docx");
            }
            else
            if (CreditNameID == 1 && !ApplicantionCheck.Checked)
            {
                fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Öhdəlik ötürmə.docx");
                letterDetailsOK = true;
            }

            if (!letterDetailsOK)
                return;

            if (!File.Exists((string)fileName))
            {
                XtraMessageBox.Show("Öhdəlik ötürmənin şablon faylı " + GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında yoxdur. Zəhmət olmasa şablon faylı göstərilən ünvanda yaradandan sonra bu düyməni klikləyin.");
                return;
            }

            if (ControlDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCommitmentFileWait));
                List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();
                string amount_with_word,
                    amount_with_word2,
                    amount_with_word3,
                    qep = null,
                    qep2 = null,
                    qep3 = null,
                    object_desc = null,
                    phone = null,
                    s_bankaccount = null,
                    date = GlobalFunctions.DateWithDayMonthYear(AgreementDate.DateTime),
                    letterDate = GlobalFunctions.DateWithYear(AgreementDate.DateTime),
                    bank_account1 = " ",
                    bank_account2 = " ",
                    liquid_currency_name = lstCurrency.Find(c => c.ID == LiquidCurrencyID).NAME,
                    liquid_currency_small_name = lstCurrency.Find(c => c.ID == LiquidCurrencyID).SMALL_NAME,
                    currency_name = lstCurrency.Find(c => c.ID == DebtCurrencyID).NAME,
                    currency_small_name = lstCurrency.Find(c => c.ID == DebtCurrencyID).SMALL_NAME;


                if (CreditNameID == 1)
                    object_desc = "Marka, Model, Tip, Buraxılış ili, Rəngi, BAN";
                else if (CreditNameID == 5)
                    object_desc = "Ünvan, çıxarış, sahəsi";

                double d = (double)LiquidValue * 100;

                int div = (int)(d / 100), mod = (int)(d % 100);
                if (mod > 0)
                    qep = " " + mod.ToString() + " " + liquid_currency_small_name;


                amount_with_word = " (" + GlobalFunctions.IntegerToWritten(div) + ") " + liquid_currency_name + qep;

                double d2 = (double)Amount * 100;

                int div2 = (int)(d2 / 100), mod2 = (int)(d2 % 100);
                if (mod2 > 0)
                    qep2 = " " + mod2.ToString() + " " + currency_small_name;

                amount_with_word2 = " (" + GlobalFunctions.IntegerToWritten(div2) + ") " + currency_name + qep2;

                double d3 = (double)TotalDebtValue.Value * 100;

                int div3 = (int)(d3 / 100), mod3 = (int)(d3 % 100), i = 1;
                if (mod3 > 0)
                    qep3 = " " + mod3.ToString() + " " + currency_small_name;

                amount_with_word3 = " (" + GlobalFunctions.IntegerToWritten(div3) + " " + currency_name + qep3 + ") ";

                List<PhonesView> lstPhone = PhonesViewDAL.SelectPhone(int.Parse(CommitmentID), "CC").ToList<PhonesView>();
                phone = lstPhone.First().PHONE;

                string voen1 = (String.IsNullOrWhiteSpace(VoenText.Text)) ? "" : "Vöen : " + VoenText.Text.Trim(),
                       voen = (String.IsNullOrWhiteSpace(Voen)) ? "" : "Vöen : " + Voen;
                try
                {
                    s_bankaccount = $@"SELECT B.LONG_NAME || ', HESAB:' || B.ACCOUNT BANK_ACCOUNT
                                          FROM CRS_USER.BANKS B
                                         WHERE B.IS_USED = 1";

                    foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s_bankaccount).Rows)
                    {
                        if (i > 2)
                            continue;

                        if (i == 1)
                            bank_account1 = dr[0].ToString();
                        if (i == 2)
                            bank_account2 = dr[0].ToString();
                        i++;
                    }
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Bank hesabları tapılmadı.", s_bankaccount, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }

                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                try
                {

                    copy_file_name = ContractCode.Replace(@"/", null) + "_Öhdəlik ötürmə_H" + (file_count + 1).ToString().PadLeft(3, '0');
                    object readOnly = false;
                    object isVisible = false;
                    wordApp.Visible = false;

                    aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    saveAs = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + copy_file_name + ".docx");

                    aDoc.Activate();
                    GlobalProcedures.FindAndReplace(wordApp, "[$currentdate]", date);
                    GlobalProcedures.FindAndReplace(wordApp, "[$code]", ContractCode);
                    GlobalProcedures.FindAndReplace(wordApp, "[$persontype]", "hüquqi şəxs");
                    GlobalProcedures.FindAndReplace(wordApp, "[$commitment]", NameText.Text.Trim() + " (Vöen: " + VoenText.Text.Trim() + ")");
                    GlobalProcedures.FindAndReplace(wordApp, "[$amount]", LiquidValue.ToString("N2") + " " + liquid_currency_name);
                    GlobalProcedures.FindAndReplace(wordApp, "[$objecttype]", CreditName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$objectdescription]", object_desc);
                    GlobalProcedures.FindAndReplace(wordApp, "[$object]", Lizinq);
                    GlobalProcedures.FindAndReplace(wordApp, "[$objectcount]", Count.ToString());
                    GlobalProcedures.FindAndReplace(wordApp, "[$amountwithstring]", LiquidValue.ToString("N2") + amount_with_word);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$seller]", NameText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$advancepayment]", AdvancePaymentValue.Value.ToString("N2") + " " + currency_name);
                    GlobalProcedures.FindAndReplace(wordApp, "[$period]", PeriodDate.Text);
                    GlobalProcedures.FindAndReplace(wordApp, "[$prizeinterest]", "maliyyələşdirilən məbləğin illik " + PrizeInterestValue.Value.ToString() + " faiz");
                    GlobalProcedures.FindAndReplace(wordApp, "[$serviceamount]", ServiceAmountValue.Value.ToString("N2") + " " + currency_name);
                    GlobalProcedures.FindAndReplace(wordApp, "[$debt]", TotalDebtValue.Value.ToString("N2") + amount_with_word3);
                    GlobalProcedures.FindAndReplace(wordApp, "[$bankaccount1]", bank_account1);
                    GlobalProcedures.FindAndReplace(wordApp, "[$bankaccount2]", bank_account2);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer1]", CustomerName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerName);
                    if (ParentPersonTypeID == 1)
                    {
                        GlobalProcedures.FindAndReplace(wordApp, "[$sv]", CustomerCard);
                        GlobalProcedures.FindAndReplace(wordApp, "[$voen]", voen);
                    }
                    else
                    {
                        GlobalProcedures.FindAndReplace(wordApp, "[$sv]", voen);
                        GlobalProcedures.FindAndReplace(wordApp, "[$voen]", null);
                    }
                    GlobalProcedures.FindAndReplace(wordApp, "[$address]", "Ünvan:" + CustomerAddress);
                    GlobalProcedures.FindAndReplace(wordApp, "[$phone]", "Telefon:" + CustomerPhone);
                    GlobalProcedures.FindAndReplace(wordApp, "[$account]", Account);
                    GlobalProcedures.FindAndReplace(wordApp, "[$commitment1]", NameText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$sv1]", "Vöen: " + VoenText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$address1]", "Ünvan:" + AddressText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$phone1]", "Telefon:" + phone);

                    GlobalProcedures.FindAndReplace(wordApp, "[$voen1]", null);
                    GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", ContractStartDate);
                    GlobalProcedures.FindAndReplace(wordApp, "[$carnumber]", CarNumber);
                    GlobalProcedures.FindAndReplace(wordApp, "[$letterhostage]", LetterHostage);
                    GlobalProcedures.FindAndReplace(wordApp, "[$letterdate]", letterDate);
                    if (letterCommitmentName != null)
                        GlobalProcedures.FindAndReplace(wordApp, "[$lettercommitmentname2]", letterCommitmentName + ((letterCommitmentName.IndexOf("oğlu") > 0) ? "nun" : "nın"));
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercode]", letterNumber);
                    GlobalProcedures.FindAndReplace(wordApp, "[$referencecode]", referenceNumber);
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercustomername]", letterCustomerName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercommitmentname]", letterCommitmentName);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customerdrivinglicense]", customerDrivingLicense);
                    GlobalProcedures.FindAndReplace(wordApp, "[$sellerdrivinglicense]", commitmentDrivingLicense);
                    file_count++;

                    aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                    if (File.Exists((string)saveAs))
                        Process.Start((string)saveAs);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.SplashScreenClose();
                    GlobalProcedures.LogWrite(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + copy_file_name + ".docx ötürmə.docx faylı açılmadı", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    aDoc.Close(ref missing, ref missing, ref missing);
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }

        private void InsertCommitment()
        {
            string sql1 = $@"INSERT INTO CRS_USER_TEMP.JURIDICAL_COMMITMENTS_TEMP(ID,
                                                                                    CONTRACT_ID,                                                                                    
                                                                                    COMMITMENT_NAME,
                                                                                    VOEN,
                                                                                    LEADING_NAME,                                                                                    
                                                                                    ADDRESS,   
                                                                                    ACCOUNT_NUMBER,
                                                                                    USED_USER_ID,
                                                                                    IS_CHANGE) 
                                                                    VALUES({CommitmentID},                                        
                                                                                {ContractID},                                        
                                                                                '{NameText.Text.Trim()}',    
                                                                                '{VoenText.Text.Trim()}', 
                                                                                '{LeadingPersonNameText.Text.Trim()}', 
                                                                                '{AddressText.Text.Trim()}',  
                                                                                '{AccountText.Text.Trim()}',
                                                                                {GlobalVariables.V_UserID},
                                                                                1)",
                   sql2 = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP(ID,
                                                                                      PARENT_ID,
                                                                                      CONTRACT_ID,
                                                                                      PERSON_TYPE_ID,
                                                                                      AGREEMENTDATE,
                                                                                      COMMITMENT_NAME,                                                                                    
                                                                                      DEBT,
                                                                                      PAYMENT_INTEREST_DEBT,
                                                                                      TOTAL_DEBT,
                                                                                      IS_ADD_PAYMENT_DEBT,
                                                                                      CURRENCY_ID,
                                                                                      PERIOD_DATE,
                                                                                      INTEREST,
                                                                                      ADVANCE_PAYMENT,
                                                                                      SERVICE_AMOUNT,
                                                                                      VOEN,
                                                                                      ACCOUNT_NUMBER,
                                                                                      USED_USER_ID,
                                                                                      IS_CHANGE) 
                            VALUES({CommitmentID},
                                        {ParentID},
                                        {ContractID},
                                        2,
                                        TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                        '{NameText.Text.Trim()}',                                        
                                        {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {(CheckInterestDebt.Checked ? 1 : 0)},
                                        {DebtCurrencyID},
                                        TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                        {PrizeInterestValue.Value},
                                        {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},     
                                        '{VoenText.Text.Trim()}',
                                        '{AccountText.Text.Trim()}',
                                        {GlobalVariables.V_UserID},
                                        1)";

            GlobalProcedures.ExecuteTwoQuery(sql1, sql2, "Öhdəlik ötürmənin məlumatları temp cədvələ daxil edilmədi.", this.Name + "/InsertCommitment");
        }

        private void UpdateCommitment()
        {
            string sql1 = $@"UPDATE CRS_USER_TEMP.JURIDICAL_COMMITMENTS_TEMP SET                                     
                                    COMMITMENT_NAME = '{NameText.Text.Trim()}',                                   
                                    ADDRESS = '{AddressText.Text.Trim()}',  
                                    LEADING_NAME = '{LeadingPersonNameText.Text.Trim()}',
                                    IS_CHANGE = 1,
                                    VOEN = '{VoenText.Text.Trim()}',
                                    ACCOUNT_NUMBER = '{AccountText.Text.Trim()}'
                            WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {CommitmentID}",
                    sql2 = $@"UPDATE CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP SET 
                                    AGREEMENTDATE = TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                    COMMITMENT_NAME = '{NameText.Text.Trim()}',                                    
                                    DEBT = {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    PAYMENT_INTEREST_DEBT = {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    TOTAL_DEBT = {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    IS_ADD_PAYMENT_DEBT = {(CheckInterestDebt.Checked ? 1 : 0)},
                                    CURRENCY_ID = {DebtCurrencyID},
                                    PERIOD_DATE = TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                    INTEREST = {PrizeInterestValue.Value},
                                    ADVANCE_PAYMENT = {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    SERVICE_AMOUNT = {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    VOEN = '{VoenText.Text.Trim()}',
                                    ACCOUNT_NUMBER = '{AccountText.Text.Trim()}',
                                    IS_CHANGE = 1
                            WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {CommitmentID}";

            GlobalProcedures.ExecuteTwoQuery(sql1, sql2, "Öhdəlik ötürmənin məlumatları temp cədvəldə dəyişdirilmədi.", this.Name + "/UpdateCommitment");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCommitment();
                else
                    UpdateCommitment();
                UpdatePhoneSendSms();
                this.Close();
            }
        }

        private void FJuridicalCommitment_Load(object sender, EventArgs e)
        {
            AgreementDate.Properties.MinValue = AgreementDateMinValue;
            AgreementDate.Properties.MaxValue = GlobalFunctions.ChangeStringToDate(ContractEndDate, "ddmmyyyy");
            ApplicantionCheck.Visible = (CreditNameID == 1);
            CustomerNameText.Text = CustomerName;
            LizinqObjectText.Text = Lizinq;

            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                AgreementDate.EditValue = DateTime.Today;
                CommitmentID = GlobalFunctions.GetOracleSequenceValue("COMMITMENT_SEQUENCE").ToString();
                PeriodDate.EditValue = GlobalFunctions.ChangeStringToDate(ContractEndDate, "ddmmyyyy");
                PeriodDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(ContractStartDate, "ddmmyyyy");
                //currecnt_debt = (decimal)GlobalFunctions.GetAmount($@"SELECT DEBT FROM CRS_USER.V_CUSTOMER_LAST_PAYMENT WHERE CONTRACT_ID = {ContractID}", this.Name + "/FCommitmentTransmission_Load");
                DebtCurrencyLabel.Text = InterestDebtCurrencyLabel.Text = TotalDebtCurrencyLabel.Text = DebtCurrency;
                //if (currecnt_debt == 0)
                //    DebtValue.Value = Debt;
                //else
                //    DebtValue.Value = currecnt_debt;
                PrizeInterestValue.Value = Interest;
                if (DebtValue.Value == 0)
                    RefreshTotals();
            }
            else
            {
                InsertPhonesTemp();
                LoadCommitmentDetails();
                FormStatus = true;
            }
            LoadPhoneDataGridView();
        }

        private void LoadCommitmentDetails()
        {
            string s = $@"SELECT JC.COMMITMENT_NAME,
                               JC.LEADING_NAME,
                               JC.VOEN,
                               JC.ACCOUNT_NUMBER,
                               JC.ADDRESS,
                               CC.AGREEMENTDATE,
                               CC.DEBT,
                               CC.PAYMENT_INTEREST_DEBT,
                               CC.IS_ADD_PAYMENT_DEBT,
                               C.CODE CURRENCY_CODE,
                               CC.PERIOD_DATE,
                               CC.INTEREST,
                               CC.ADVANCE_PAYMENT,
                               CC.SERVICE_AMOUNT
                          FROM CRS_USER_TEMP.JURIDICAL_COMMITMENTS_TEMP JC,
                               CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP CC,
                               CRS_USER.CURRENCY C
                         WHERE CC.ID = JC.ID AND CC.CURRENCY_ID = C.ID AND JC.ID = {CommitmentID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCommitmentDetails", "Öhdəliyin parametrləri tapılmadı.");
            foreach (DataRow dr in dt.Rows)
            {
                AgreementDate.EditValue = DateTime.Parse(dr["AGREEMENTDATE"].ToString());
                NameText.Text = dr["COMMITMENT_NAME"].ToString();
                LeadingPersonNameText.Text = dr["LEADING_NAME"].ToString();
                VoenText.Text = dr["VOEN"].ToString();
                AccountText.Text = dr["ACCOUNT_NUMBER"].ToString();
                AddressText.Text = dr["ADDRESS"].ToString();
                DebtValue.Value = Convert.ToDecimal(dr["DEBT"].ToString());
                InterestDebtValue.Value = Convert.ToDecimal(dr["PAYMENT_INTEREST_DEBT"]);
                CheckInterestDebt.Checked = Convert.ToInt16(dr["IS_ADD_PAYMENT_DEBT"]) == 1;
                DebtCurrencyLabel.Text = InterestDebtCurrencyLabel.Text = TotalDebtCurrencyLabel.Text = dr["CURRENCY_CODE"].ToString();
                PeriodDate.EditValue = DateTime.Parse(dr["PERIOD_DATE"].ToString());
                PrizeInterestValue.EditValue = Convert.ToInt32(dr["INTEREST"].ToString());
                AdvancePaymentValue.Value = Convert.ToDecimal(dr["ADVANCE_PAYMENT"].ToString());
                ServiceAmountValue.Value = Convert.ToDecimal(dr["SERVICE_AMOUNT"].ToString());
            }
        }

        private void LoadPhoneDataGridView()
        {
            string s = $@"SELECT 1 SS,
                             P.ID,
                             PD.DESCRIPTION_AZ DESCRIPTION,
                             '+' || C.CODE || P.PHONE_NUMBER PHONE_NUMBER,
                             P.IS_SEND_SMS,
                             KR.NAME KINDSHIP_RATE_NAME
                        FROM CRS_USER_TEMP.PHONES_TEMP P,
                             CRS_USER.PHONE_DESCRIPTIONS PD,
                             CRS_USER.COUNTRIES C,
                             CRS_USER.KINDSHIP_RATE KR
                       WHERE     P.IS_CHANGE IN (0, 1)
                             AND P.PHONE_DESCRIPTION_ID = PD.ID
                             AND P.COUNTRY_ID = C.ID
                             AND P.OWNER_TYPE = 'CC'
                             AND P.KINDSHIP_RATE_ID = KR.ID(+)
                             AND P.OWNER_ID = {CommitmentID}
                    ORDER BY P.ORDER_ID";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");

                if (PhoneGridView.RowCount > 0)
                    EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = true;
                else
                    EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = UpPhoneBarButton.Enabled = DownPhoneBarButton.Enabled = false;

                try
                {
                    PhoneGridView.BeginUpdate();
                    for (int i = 0; i < PhoneGridView.RowCount; i++)
                    {
                        DataRow row = PhoneGridView.GetDataRow(PhoneGridView.GetVisibleRowHandle(i));
                        if (Convert.ToInt32(row["IS_SEND_SMS"].ToString()) == 1)
                            PhoneGridView.SelectRow(i);
                    }
                }
                finally
                {
                    PhoneGridView.EndUpdate();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Telefon nömrələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void UpdatePhoneSendSms()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0 WHERE IS_SEND_SMS = 1 AND OWNER_TYPE = 'CC' AND OWNER_ID IN (SELECT ID FROM CRS_USER_TEMP.JURIDICAL_COMMITMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                                    "Telefon nömrələri dəyişdirilmədi.",
                                                this.Name + "/UpdatePhoneSendSms");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PhoneGridView.SelectedRowsCount; i++)
            {
                rows.Add(PhoneGridView.GetDataRow(PhoneGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 1 WHERE ID = {row["ID"]}",
                                                    "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                                  this.Name + "/UpdatePhoneSendSms");
            }

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'CC' AND OWNER_ID IN (SELECT ID FROM CRS_USER_TEMP.JURIDICAL_COMMITMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                                "Telefon nömrələri dəyişdirilmədi.",
                                               this.Name + "/UpdatePhoneSendSms");
        }
    }
}