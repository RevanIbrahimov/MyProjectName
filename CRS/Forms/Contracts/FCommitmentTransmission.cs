using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Bytescout.Document;
using System.IO;
using System.Diagnostics;
using System.Collections;
using Oracle.ManagedDataAccess.Client;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using CRS.Class;
using CRS.Class.Views;
using Microsoft.Reporting.WebForms;

namespace CRS.Forms.Contracts
{
    public partial class FCommitmentTransmission : DevExpress.XtraEditors.XtraForm
    {
        public FCommitmentTransmission()
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

        decimal currecnt_debt, interest_debt = 0;
        string PhoneID, PhoneNumber, copy_file_name, letterNumber, referenceNumber, letterCustomerName, customerDrivingLicense, letterCommitmentName, commitmentDrivingLicense;
        int card_series_id, card_issuing_id;
        static int file_count = 0;
        bool letterDetailsOK = false, FormStatus = false;

        ReportViewer rv_commitment = new ReportViewer();

        public delegate void DoEvent();
        public event DoEvent RefreshCommitmentsDataGridView;

        private void FCommitmentTransmission_Load(object sender, EventArgs e)
        {
            List<ContractCommitment> lstCommitments = CommitmentsDAL.SelectAllCommitmentTempByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();

            AgreementDate.Properties.MinValue = AgreementDateMinValue;
            AgreementDate.Properties.MaxValue = GlobalFunctions.ChangeStringToDate(ContractEndDate, "ddmmyyyy");

            IssueDate.Properties.MaxValue = ReliableDate.Properties.MinValue = DateTime.Today;
            CustomerNameText.Text = CustomerName;
            LizinqObjectText.Text = Lizinq;
            ApplicantionCheck.Visible = (CreditNameID == 1);
            GlobalProcedures.FillComboBoxEdit(SeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
            SeriesComboBox.SelectedIndex = 0;
            GlobalProcedures.FillComboBoxEdit(IssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");

            if (TransactionName == "INSERT")
            {
                FormStatus = true;
                AgreementDate.EditValue = DateTime.Today;
                CommitmentID = GlobalFunctions.GetOracleSequenceValue("COMMITMENT_SEQUENCE").ToString();
                PeriodDate.EditValue = GlobalFunctions.ChangeStringToDate(ContractEndDate, "ddmmyyyy");
                PeriodDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(ContractStartDate, "ddmmyyyy");
                //currecnt_debt = (decimal)GlobalFunctions.GetAmount($@"SELECT DEBT FROM CRS_USER.V_CUSTOMER_LAST_PAYMENT WHERE CONTRACT_ID = {ContractID}", this.Name + "/FCommitmentTransmission_Load");
                DebtCurrencyLabel.Text = InterestDebtCurrencyLabel.Text = TotalDebtCurrencyLabel.Text = DebtCurrency;
                //DebtValue.Value = (currecnt_debt == 0) ? Debt : currecnt_debt;
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
            string s = $@"SELECT CC.AGREEMENTDATE,
                               CC.COMMITMENT_NAME,
                               CS.SERIES,
                               CC.CARD_NUMBER,
                               CC.CARD_PINCODE,
                               CC.CARD_ISSUE_DATE,
                               CC.CARD_RELIABLE_DATE,
                               CI.NAME,
                               CC.ADDRESS,
                               CC.DEBT,
                               CC.PAYMENT_INTEREST_DEBT,
                               CC.IS_ADD_PAYMENT_DEBT,
                               C.CODE,
                               CC.PERIOD_DATE,
                               CC.INTEREST,
                               CC.ADVANCE_PAYMENT,
                               CC.SERVICE_AMOUNT,
                               CC.VOEN,
                               CC.ACCOUNT_NUMBER
                          FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP CC,
                               CRS_USER.CARD_SERIES CS,
                               CRS_USER.CARD_ISSUING CI,
                               CRS_USER.CURRENCY C
                         WHERE CC.CARD_SERIES_ID = CS.ID
                               AND CC.CARD_ISSUING_ID = CI.ID
                               AND CC.CURRENCY_ID = C.ID
                               AND CC.ID = {CommitmentID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCommitmentDetails").Rows)
                {
                    AgreementDate.EditValue = DateTime.Parse(dr["AGREEMENTDATE"].ToString());
                    NameText.Text = dr["COMMITMENT_NAME"].ToString();
                    SeriesComboBox.EditValue = dr["SERIES"].ToString();
                    NumberText.Text = dr["CARD_NUMBER"].ToString();
                    PinCodeText.Text = dr["CARD_PINCODE"].ToString();
                    IssueDate.EditValue = DateTime.Parse(dr["CARD_ISSUE_DATE"].ToString());
                    ReliableDate.EditValue = DateTime.Parse(dr["CARD_RELIABLE_DATE"].ToString());
                    IssuingComboBox.EditValue = dr["NAME"].ToString();
                    AddressText.Text = dr["ADDRESS"].ToString();
                    DebtValue.Value = Convert.ToDecimal(dr["DEBT"]);
                    InterestDebtValue.Value = Convert.ToDecimal(dr["PAYMENT_INTEREST_DEBT"]);
                    CheckInterestDebt.Checked = Convert.ToInt16(dr["IS_ADD_PAYMENT_DEBT"]) == 1;
                    DebtCurrencyLabel.Text = InterestDebtCurrencyLabel.Text = TotalDebtCurrencyLabel.Text = dr["CODE"].ToString();
                    PeriodDate.EditValue = DateTime.Parse(dr["PERIOD_DATE"].ToString());
                    PrizeInterestValue.EditValue = Convert.ToInt32(dr["INTEREST"]);
                    AdvancePaymentValue.Value = Convert.ToDecimal(dr["ADVANCE_PAYMENT"]);
                    ServiceAmountValue.Value = Convert.ToDecimal(dr["SERVICE_AMOUNT"]);
                    VoenText.Text = dr["VOEN"].ToString();
                    AccountText.Text = dr["ACCOUNT_NUMBER"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Öhdəliyin parametrləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
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

            if (AccountText.Text.Length > 0 && AccountText.Text.Length != 28)
            {
                AccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fiziki şəxsin hesab nömrəsi 28 simvol olmalıdır.");
                AccountText.Focus();
                AccountText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(SeriesComboBox.Text))
            {
                SeriesComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təyin edən sənədin seriyası daxil edilməyib.");
                SeriesComboBox.Focus();
                SeriesComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(NumberText.Text.Trim()))
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin nömrəsi daxil edilməyib.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((card_series_id == 2) && (NumberText.Text.Length != 8))
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi 8 simvol olmalıdır.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((card_series_id == 2) && (PinCodeText.Text.Length != 7) && (!String.IsNullOrEmpty(PinCodeText.Text.Trim())))
            {
                PinCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin pin kodu 7 simvol olmalıdır.");
                PinCodeText.Focus();
                PinCodeText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length > 0 && !GlobalFunctions.Regexp("[0-9]{9}2", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fiziki şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>2</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(IssueDate.Text))
            {
                IssueDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin verilmə tarixi daxil edilməyib.");
                IssueDate.Focus();
                IssueDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (String.IsNullOrEmpty(ReliableDate.Text))
            {
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin etibarlılıq tarixi daxil edilməyib.");
                ReliableDate.Focus();
                ReliableDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (IssueDate.DateTime == ReliableDate.DateTime)
            {
                IssueDate.BackColor = Color.Red;
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi ilə etibarlı olma tarixi eyni ola bilməz.");
                IssueDate.Focus();
                IssueDate.BackColor = GlobalFunctions.ElementColor();
                ReliableDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(IssuingComboBox.Text))
            {
                IssuingComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təyin edən sənədin seriyası daxil edilməyib.");
                IssuingComboBox.Focus();
                IssuingComboBox.BackColor = GlobalFunctions.ElementColor();
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

        private void CheckInterestDebt_CheckedChanged(object sender, EventArgs e)
        {
            TotalDebtValue.EditValue = DebtValue.Value + (CheckInterestDebt.Checked ? InterestDebtValue.Value : 0);
        }

        private void AccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(AccountText, AccountLengthLabel);
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 2:
                    GlobalProcedures.FillComboBoxEdit(SeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 3:
                    GlobalProcedures.FillComboBoxEdit(IssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void AgreementDate_EditValueChanged(object sender, EventArgs e)
        {
            RefreshTotals();
        }

        private void ApplicantionCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle(ApplicantionCheck);
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void SeriesComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 2);
        }

        private void VoenText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(VoenText, VoenLengthLabel);
        }

        private void IssuingComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 3);
        }

        void RefreshTotals()
        {
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

            //currecnt_debt = (decimal)GlobalFunctions.GetAmount($@"SELECT TOTAL FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CONTRACT_ID = {ContractID} AND PAYMENT_DATE = (SELECT MAX(PAYMENT_DATE) FROM CRS_USER.CUSTOMER_PAYMENTS WHERE CONTRACT_ID = CP.CONTRACT_ID)");
            //if (currecnt_debt == 0)
            //    DebtValue.Value = Debt;
            //else
            //    DebtValue.Value = currecnt_debt;
        }

        private void LoadFPayment(string contract_id, string contract_code, int interest, int period, string s_date, string e_date, string lizinq, string customer_code, string customer_name, string customer_id, double amount, string currency)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FWait));
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
            GlobalProcedures.SplashScreenClose();
            fp.ShowDialog();            
        }

        private void CustomerPaymentsLabel_Click(object sender, EventArgs e)
        {
            LoadFPayment(ContractID, ContractCode, Interest, Period, ContractStartDate, ContractEndDate, Lizinq, CustomerCode, CustomerName, CustomerID, (double)Amount, DebtCurrency);
        }

        private void PhoneGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PhoneGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
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
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0 WHERE IS_SEND_SMS = 1 AND OWNER_TYPE = 'CC' AND OWNER_ID IN (SELECT ID FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})",
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

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'CC' AND OWNER_ID IN (SELECT ID FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                                "Telefon nömrələri dəyişdirilmədi.",
                                               this.Name + "/UpdatePhoneSendSms");
        }

        private void InsertPhonesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_INSERT_COMMIT_PHONE_TEMP", "P_CONTRACT_ID", int.Parse(ContractID), "P_COMMITMENT_ID", int.Parse(CommitmentID), "Öhdəlik götürənin telefonları temp cədvələ daxil edilmədi.");
        }

        void RefreshPhone()
        {
            LoadPhoneDataGridView();
        }

        public void LoadFPhoneAddEdit(string transaction, string OwnerID, string OwnerType, string PhoneID)
        {
            FPhoneAddEdit fp = new FPhoneAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerID = OwnerID;
            fp.OwnerType = OwnerType;
            fp.PhoneID = PhoneID;
            fp.RefreshPhonesDataGridView += new FPhoneAddEdit.DoEvent(RefreshPhone);
            fp.ShowDialog();
        }

        private void NewPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("INSERT", CommitmentID, "CC", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", CommitmentID, "CC", PhoneID);
        }

        private void DeletePhone()
        {
            DialogResult dialogResult = XtraMessageBox.Show(PhoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'CC' AND ID = " + PhoneID, "Telefon nömrəsi temp cədvəldən silinmədi.");
            }
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
            LoadPhoneDataGridView();
        }

        private void RefreshPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDataGridView();
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

        private void FCommitmentTransmission_FormClosing(object sender, FormClosingEventArgs e)
        {
            rv_commitment.Reset();
            this.RefreshCommitmentsDataGridView();
        }

        private void InsertCommitment()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP(ID,
                                                                                    PARENT_ID,
                                                                                    CONTRACT_ID,
                                                                                    AGREEMENTDATE,
                                                                                    COMMITMENT_NAME,
                                                                                    CARD_SERIES_ID,
                                                                                    CARD_NUMBER,
                                                                                    CARD_PINCODE,
                                                                                    CARD_ISSUE_DATE,
                                                                                    CARD_RELIABLE_DATE,
                                                                                    CARD_ISSUING_ID,
                                                                                    ADDRESS,
                                                                                    DEBT,
                                                                                    PAYMENT_INTEREST_DEBT,
                                                                                    TOTAL_DEBT,
                                                                                    IS_ADD_PAYMENT_DEBT,
                                                                                    CURRENCY_ID,
                                                                                    PERIOD_DATE,
                                                                                    INTEREST,
                                                                                    ADVANCE_PAYMENT,
                                                                                    SERVICE_AMOUNT,
                                                                                    USED_USER_ID,
                                                                                    IS_CHANGE,
                                                                                    VOEN,
                                                                                    ACCOUNT_NUMBER) 
                            VALUES({CommitmentID},
                                        {ParentID},
                                        {ContractID},
                                        TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                        '{NameText.Text.Trim()}',
                                        {card_series_id},
                                        '{NumberText.Text.Trim()}',
                                        '{PinCodeText.Text.Trim()}',
                                        TO_DATE('{IssueDate.Text}','DD/MM/YYYY'),
                                        TO_DATE('{ReliableDate.Text}','DD/MM/YYYY'),
                                        {card_issuing_id},
                                        '{AddressText.Text.Trim()}',
                                        {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {(CheckInterestDebt.Checked? 1 : 0)},
                                        {DebtCurrencyID},
                                        TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                        {PrizeInterestValue.Value},
                                        {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {GlobalVariables.V_UserID},
                                        1,
                                        '{VoenText.Text.Trim()}',
                                        '{AccountText.Text.Trim()}')",
                                sql2 = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP(ID,
                                                                                                    PARENT_ID,
                                                                                                    CONTRACT_ID,
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

            GlobalProcedures.ExecuteTwoQuery(sql, sql2, "Öhdəlik ötürmənin məlumatları temp cədvələ daxil edilmədi.", this.Name + "/InsertCommitment");
        }

        private void UpdateCommitment()
        {
            string sql1 = $@"UPDATE CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP SET 
                                    AGREEMENTDATE = TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                    COMMITMENT_NAME = '{NameText.Text.Trim()}',
                                    CARD_SERIES_ID = {card_series_id},
                                    CARD_NUMBER = '{NumberText.Text.Trim()}',
                                    CARD_PINCODE = '{PinCodeText.Text.Trim()}',
                                    CARD_ISSUE_DATE = TO_DATE('{IssueDate.Text}','DD/MM/YYYY'),
                                    CARD_RELIABLE_DATE = TO_DATE('{ReliableDate.Text}','DD/MM/YYYY'),
                                    CARD_ISSUING_ID = {card_issuing_id},
                                    ADDRESS = '{AddressText.Text.Trim()}',
                                    DEBT = {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    PAYMENT_INTEREST_DEBT = {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    TOTAL_DEBT = {TotalDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    IS_ADD_PAYMENT_DEBT = {(CheckInterestDebt.Checked ? 1 : 0)},
                                    CURRENCY_ID = {DebtCurrencyID},
                                    PERIOD_DATE = TO_DATE('{PeriodDate.Text}','DD/MM/YYYY'),
                                    INTEREST = {PrizeInterestValue.Value},
                                    ADVANCE_PAYMENT = {AdvancePaymentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    SERVICE_AMOUNT = {ServiceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                    IS_CHANGE = 1,
                                    VOEN = '{VoenText.Text.Trim()}',
                                    ACCOUNT_NUMBER = '{AccountText.Text.Trim()}',
                                    ETL_DT_TM = SYSDATE
                            WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {CommitmentID}",
                    sql2 = $@"UPDATE CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP SET 
                                    AGREEMENTDATE = TO_DATE('{AgreementDate.Text}','DD/MM/YYYY'),
                                    COMMITMENT_NAME = '{NameText.Text.Trim()}',                                    
                                    DEBT = {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
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

        private void SeriesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            card_series_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_SERIES", "SERIES", "ID", SeriesComboBox.Text);
        }

        private void IssuingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            card_issuing_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_ISSUING", "NAME", "ID", IssuingComboBox.Text);
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PopupMenu, e);
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled)
                LoadFPhoneAddEdit("EDIT", CommitmentID, "CC", PhoneID);
        }

        private void NumberText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(NumberText, NumberLengthLabel);
            if (TransactionName == "INSERT")
                LoadOldData(NumberText.Text.Trim());
        }

        private void LoadOldData(string cardNumber)
        {
            if (cardNumber.Length < 7)
            {
                //NullData();
                LoadPhoneDataGridView();
                return;
            }

            string sql = $@"SELECT CC.COMMITMENT_NAME,
                                   CC.CARD_PINCODE,
                                   CC.CARD_ISSUE_DATE,
                                   CC.CARD_RELIABLE_DATE,
                                   CC.ADDRESS,
                                   CS.SERIES,
                                   CI.NAME,
                                   CC.VOEN,
                                   CC.ID
                              FROM CRS_USER.CONTRACT_COMMITMENTS CC,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.CARD_ISSUING CI
                             WHERE     CC.CARD_SERIES_ID = CS.ID
                                   AND CC.CARD_ISSUING_ID = CI.ID
                                   AND CC.ID = (SELECT MAX(ID) FROM CRS_USER.CONTRACT_COMMITMENTS WHERE CARD_NUMBER LIKE '%{cardNumber}')";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/", "Sənəd nömrəsinə görə öhdəlik götürən tapılmadı.");
            //try
            //{
            if (dt.Rows.Count > 0)
            {
                NameText.Text = dt.Rows[0]["COMMITMENT_NAME"].ToString();
                SeriesComboBox.EditValue = dt.Rows[0]["SERIES"];
                PinCodeText.Text = dt.Rows[0]["CARD_PINCODE"].ToString();
                IssueDate.EditValue = dt.Rows[0]["CARD_ISSUE_DATE"];
                ReliableDate.EditValue = dt.Rows[0]["CARD_RELIABLE_DATE"];
                IssuingComboBox.EditValue = dt.Rows[0]["NAME"];
                AddressText.Text = dt.Rows[0]["ADDRESS"].ToString();
                VoenText.Text = dt.Rows[0]["VOEN"].ToString();
                InsertOldPhone(int.Parse(dt.Rows[0]["ID"].ToString()));
            }
            //    else
            //        NullData();
            //}
            //catch
            //{
            //    NullData();
            //}
            LoadPhoneDataGridView();
        }

        private void InsertOldPhone(int oldCommitmentID)
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.PHONES_TEMP (ID,
                                             PHONE_DESCRIPTION_ID,
                                             COUNTRY_ID,
                                             PHONE_NUMBER,
                                             KINDSHIP_RATE_ID,
                                             KINDSHIP_NAME,
                                             IS_SEND_SMS,
                                             OWNER_ID,
                                             OWNER_TYPE,
                                             ORDER_ID,
                                             USED_USER_ID)
                             SELECT PHONES_SEQUENCE.NEXTVAL ID,
                                    PHONE_DESCRIPTION_ID,
                                    COUNTRY_ID,
                                    PHONE_NUMBER,
                                    KINDSHIP_RATE_ID,
                                    KINDSHIP_NAME,
                                    IS_SEND_SMS,
                                    {CommitmentID} OWNER_ID,
                                    OWNER_TYPE,
                                    ORDER_ID,
                                    {GlobalVariables.V_UserID} USED_USER_ID
                               FROM CRS_USER.PHONES
                              WHERE OWNER_TYPE = 'CC' AND OWNER_ID ={oldCommitmentID}",
                   sql_delete = $@"DELETE FROM CRS_USER_TEMP.PHONES_TEMP WHERE OWNER_TYPE = 'CC' AND OWNER_ID = {CommitmentID} AND USED_USER_ID = {GlobalVariables.V_UserID}";

            GlobalProcedures.ExecuteTwoQuery(sql_delete, sql, "Telefon nömrələri temp cədvələ daxil edilmədi.", this.Text + "/InsertOldPhone");
        }

        private void NullData()
        {
            NameText.Text =
                   PinCodeText.Text =
                   AddressText.Text =
                   VoenText.Text = null;
            IssueDate.EditValue =
            ReliableDate.EditValue =
            IssuingComboBox.EditValue = null;
            SeriesComboBox.EditValue = null;
            GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER_TEMP.PROC_COMMITMENT_PHONE_DELETE", "P_COMMITMENT_ID", int.Parse(CommitmentID), "P_USED_USER_ID", GlobalVariables.V_UserID, "Öhdəlik götürənin telefonları temp cədvəldən silinmədi.");
        }

        private void PinCodeText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(PinCodeText, PinCodeLengthLabel);
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
                if (lstPhone.Count > 0)
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
                    copy_file_name = ContractCode.Replace(@"/", null) + "_Öhdəlik ötürmə_F" + (file_count + 1).ToString().PadLeft(3, '0');
                    object readOnly = false;
                    object isVisible = false;
                    wordApp.Visible = false;

                    aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    saveAs = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + copy_file_name + ".docx");
                    aDoc.Activate();
                    GlobalProcedures.FindAndReplace(wordApp, "[$currentdate]", date);
                    GlobalProcedures.FindAndReplace(wordApp, "[$code]", ContractCode);
                    GlobalProcedures.FindAndReplace(wordApp, "[$persontype]", "fiziki şəxs");
                    GlobalProcedures.FindAndReplace(wordApp, "[$commitment]", NameText.Text.Trim() + " (" + SeriesComboBox.Text + ", №: " + NumberText.Text + ", " + IssueDate.Text + " tarixdə " + IssuingComboBox.Text + " tərəfindən verilib)");
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
                    GlobalProcedures.FindAndReplace(wordApp, "[$sv1]", SeriesComboBox.Text + ", №: " + NumberText.Text + ", " + IssueDate.Text + " tarixdə " + IssuingComboBox.Text + " tərəfindən verilib");
                    GlobalProcedures.FindAndReplace(wordApp, "[$address1]", "Ünvan:" + AddressText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$phone1]", "Telefon:" + phone);
                    GlobalProcedures.FindAndReplace(wordApp, "[$voen1]", voen1);
                    GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", ContractStartDate);
                    GlobalProcedures.FindAndReplace(wordApp, "[$carnumber]", CarNumber);
                    GlobalProcedures.FindAndReplace(wordApp, "[$letterhostage]", LetterHostage);
                    GlobalProcedures.FindAndReplace(wordApp, "[$letterdate]", letterDate);
                    GlobalProcedures.FindAndReplace(wordApp, "[$lettercommitmentname2]", letterCommitmentName + ((!String.IsNullOrWhiteSpace(letterCommitmentName) && letterCommitmentName.IndexOf("oğlu") > 0) ? "nun" : "nın"));
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
                    GlobalProcedures.LogWrite(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + copy_file_name + ".docx faylı açılmadı", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    aDoc.Close(ref missing, ref missing, ref missing);
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }
    }
}