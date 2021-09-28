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
using Bytescout.Document;
using System.Diagnostics;
using DevExpress.Utils;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;
using CRS.Forms.Bookkeeping;
using CRS.Class.DataAccess;
using CRS.Class.Tables;

namespace CRS.Forms.Cash
{
    public partial class FIncomeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FIncomeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, OperationOwnerID, OperationID;
        public int Index;
        int ContractID = 0, CustomerID = 0, currency_id = 0, diff_day = 0, bank_id, founder_id, founder_card_id, fund_contract_id = 0, old_founder_id,
            old_fund_contract_id, FounderID, other_appointment_id, is_penalty = 0;
        double normal_debt = 0, cur, debt = 0, payment_interest_debt = 0, one_day_interest = 0, interest_amount = 0, totaldebtamount, requiredamount, leasing_amount,
            sum_interest_amount = 0, payment_interest_amount = 0, residual_percent = 0, penalty_amount = 0, monthly_amount, DebtPenalty = 0;
        string currency = "AZN", PaymentID, operationdate, old_bank_account_date;
        bool FormStatus = false, close_form = true;


        public delegate void DoEvent(string a);
        public event DoEvent RefreshCashDataGridView;

        private void IncomeBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            switch (IncomeBackstageViewControl.SelectedTabIndex)
            {
                case 0:
                    {
                        PaymentDate.EditValue = DateTime.Today;
                        LeasingContractCodeText.Focus();
                        BOK.Visible = true;
                    }
                    break;
                case 2:
                    {
                        GlobalProcedures.FillComboBoxEditWithSqlText(FounderComboBox, "SELECT FULLNAME,FULLNAME,FULLNAME FROM CRS_USER.FOUNDERS WHERE USED_USER_ID = -1 ORDER BY ORDER_ID");
                        FounderDate.EditValue = DateTime.Today;
                        FounderDate.Focus();
                        BOK.Visible = true;
                    }
                    break;
                case 4:
                    {
                        //permission 
                        AccountDate.EditValue = DateTime.Today;
                        AccountDate.Focus();
                        GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                    }
                    break;
                case 6:
                    {
                        GlobalProcedures.FillComboBoxEditWithSqlText(OtherAppointmentComboBox, "SELECT NAME,NAME_EN,NAME_RU FROM CRS_USER.CASH_APPOINTMENTS ORDER BY ORDER_ID");
                        OtherDate.EditValue = DateTime.Today;
                        OtherDate.Focus();
                        BOK.Visible = true;
                    }
                    break;
                case 8:
                    {
                        ServiceDate.EditValue = DateTime.Today;
                        ServiceDate.Focus();
                        BOK.Visible = true;
                    }
                    break;
            }
        }

        private void FIncomeAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                FounderComboBox.Properties.Buttons[1].Visible = GlobalVariables.Founders;
                BankLookUp.Properties.Buttons[1].Visible = GlobalVariables.Banks;
                OtherAppointmentComboBox.Properties.Buttons[1].Visible = GlobalVariables.CashOperations;
            }

            this.ActiveControl = LeasingContractCodeText;
            if (TransactionName == "EDIT")
            {
                BOK.Visible = false;
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_OPERATIONS", GlobalVariables.V_UserID, "WHERE ID = " + OperationID + " AND USED_USER_ID = -1");
                LoadIncomeDetails();
            }
            FormStatus = true;
        }

        private void LoadIncomeDetails()
        {
            int UsedUserID = -1, status_id;
            bool Used = false;
            switch (Index)
            {
                case 1:
                    {

                        IncomeBackstageViewControl.SelectedTabIndex = 0;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID IN (SELECT CONTRACT_ID FROM CRS_USER.CUSTOMER_PAYMENTS WHERE ID = " + OperationOwnerID + ") AND USED_USER_ID = -1");
                        status_id = GlobalFunctions.GetCount("SELECT STATUS_ID FROM CRS_USER.CONTRACTS WHERE ID IN (SELECT CP.CONTRACT_ID FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.ID = " + OperationOwnerID + ")");

                        if (status_id == 5)
                        {
                            UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CONTRACTS WHERE (ID,CUSTOMER_ID) IN (SELECT CONTRACT_ID,CUSTOMER_ID FROM CRS_USER.CUSTOMER_PAYMENTS WHERE ID = " + OperationOwnerID + ")");
                            if (UsedUserID >= 0)
                            {
                                if (GlobalVariables.V_UserID != UsedUserID)
                                {
                                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                    XtraMessageBox.Show("Seçdiyiniz Lizinq ödənişinin müqaviləsi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş Lizinq ödənişinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    LeasingPaymentTab.Enabled = BOK.Visible = false;
                                }
                                else
                                    LeasingPaymentTab.Enabled = BOK.Visible = true;
                            }
                            else
                                LeasingPaymentTab.Enabled = BOK.Visible = true;
                        }
                        else
                        {
                            XtraMessageBox.Show("Seçdiyiniz Lizinq ödənişinin müqaviləsi bağlanılıb. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş Lizinq ödənişinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LeasingPaymentTab.Enabled = BOK.Visible = false;
                        }

                        LoadLeasingPaymentDetails();
                        PaymentDate.Enabled =
                            FounderTab.Enabled =
                            BankAccountTab.Enabled =
                            OtherPaymentsTab.Enabled =
                            ServicePriceTab.Enabled = false;
                    }
                    break;
                case 3:
                    {
                        IncomeBackstageViewControl.SelectedTabIndex = 2;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_FOUNDER", GlobalVariables.V_UserID, "WHERE INC_EXP = 1 AND ID = " + OperationOwnerID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_FOUNDER WHERE INC_EXP = 1 AND ID = " + OperationOwnerID);
                        if (UsedUserID >= 0)
                        {
                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz Təsisçi ilə hesablaşma hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş təsisçi ilə hesablaşmanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                FounderTab.Enabled = BOK.Visible = false;
                            }
                            else
                                FounderTab.Enabled = BOK.Visible = true;
                        }
                        else
                            FounderTab.Enabled = BOK.Visible = true;

                        LoadFounderDetails();
                        LoadFundContractsGridView(founder_id);
                        FounderComboBox.Enabled =
                            ContractGridControl.Enabled =
                            ContractsBarButton.Enabled =
                            FounderDate.Enabled =
                            LeasingPaymentTab.Enabled =
                            BankAccountTab.Enabled =
                            OtherPaymentsTab.Enabled =
                            ServicePriceTab.Enabled = false;
                    }
                    break;
                case 4:
                    {
                        IncomeBackstageViewControl.SelectedTabIndex = 4;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_BANK_ACCOUNT", GlobalVariables.V_UserID, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_BANK_ACCOUNT WHERE ID = " + OperationOwnerID);
                        if (UsedUserID >= 0)
                        {
                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz Bank hesabı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş bank hesabının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                BankAccountTab.Enabled = BOK.Visible = false;
                            }
                            else
                                BankAccountTab.Enabled = BOK.Visible = true;
                        }
                        else
                            BankAccountTab.Enabled = BOK.Visible = true;

                        LoadBankAccountDetails();
                        BankLookUp.Enabled =
                            LeasingPaymentTab.Enabled =
                            FounderTab.Enabled =
                            OtherPaymentsTab.Enabled =
                            ServicePriceTab.Enabled = false;
                    }
                    break;
                case 5:
                    {
                        IncomeBackstageViewControl.SelectedTabIndex = 6;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_OTHER_PAYMENT", GlobalVariables.V_UserID, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_OTHER_PAYMENT WHERE ID = " + OperationOwnerID);
                        if (UsedUserID >= 0)
                        {
                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz Digər ödənişin hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş digər ödənişin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                OtherPaymentsTab.Enabled = BOK.Visible = false;
                            }
                            else
                                OtherPaymentsTab.Enabled = BOK.Visible = true;
                        }
                        else
                            OtherPaymentsTab.Enabled = BOK.Visible = true;

                        LoadOtherPaymentDetails();
                        LeasingPaymentTab.Enabled =
                        BankAccountTab.Enabled =
                        FounderTab.Enabled =
                        ServicePriceTab.Enabled = false;
                    }
                    break;
                case 6:
                    {
                        IncomeBackstageViewControl.SelectedTabIndex = 8;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_SERVICE_PRICE", GlobalVariables.V_UserID, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_SERVICE_PRICE WHERE ID = " + OperationOwnerID);
                        if (UsedUserID >= 0)
                        {

                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz Xidmət haqqı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş xidmət haqqının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                ServicePriceTab.Enabled = BOK.Visible = false;
                            }
                            else
                                ServicePriceTab.Enabled = BOK.Visible = true;
                        }
                        else
                            ServicePriceTab.Enabled = BOK.Visible = true;

                        LoadServicePriceDetails();
                        LeasingPaymentTab.Enabled =
                            BankAccountTab.Enabled =
                            FounderTab.Enabled =
                            OtherPaymentsTab.Enabled = false;
                    }
                    break;
            }
        }

        private void LoadLeasingPaymentDetails()
        {
            string s = "SELECT CT.CODE||C.CODE,TO_CHAR(C.START_DATE,'DD.MM.YYYY') S_DATE,CUS.SURNAME||' '||CUS.NAME||' '||CUS.PATRONYMIC||' '||DECODE(CUS.SEX_ID,1,'oğlu','qızı') CUSTOMER_NAME,CP.PAYMENT_NAME,TO_CHAR(CP.PAYMENT_DATE,'DD.MM.YYYY') P_DATE,CP.DAY_COUNT,CT.INTEREST,CP.REQUIRED_CLOSE_AMOUNT,CP.ONE_DAY_INTEREST_AMOUNT,CP.PAYMENT_INTEREST_AMOUNT,C.MONTHLY_AMOUNT,CP.REQUIRED_AMOUNT,CP.PAYMENT_AMOUNT,CP.PAYMENT_AMOUNT_AZN,C.CURRENCY_ID,CUR.CODE,CP.NOTE,CP.CUSTOMER_ID,CP.CONTRACT_ID,C.AMOUNT,CP.CURRENCY_RATE,CP.PAYMENT_INTEREST_DEBT,CP.PENALTY_DEBT,CP.IS_PENALTY,CP.PENALTY_AMOUNT FROM CRS_USER.CUSTOMER_PAYMENTS CP,CRS_USER.CONTRACTS C,CRS_USER.CURRENCY CUR,CRS_USER.CUSTOMERS CUS,CRS_USER.CREDIT_TYPE CT WHERE C.CUSTOMER_ID = CUS.ID AND CP.CONTRACT_ID = C.ID AND CP.CUSTOMER_ID = C.CUSTOMER_ID AND C.CURRENCY_ID = CUR.ID AND C.CREDIT_TYPE_ID = CT.ID AND CP.ID = " + OperationOwnerID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    LeasingContractCodeText.Properties.ReadOnly = true;
                    LeasingContractCodeText.Text = dr[0].ToString();
                    ContractStartDateText.Text = dr[1].ToString();
                    CustomerNameText.Text = dr[2].ToString();
                    NameText.Text = dr[3].ToString();
                    PaymentDate.EditValue = Class.GlobalFunctions.ChangeStringToDate(dr[4].ToString(), "ddmmyyyy");
                    DayCountText.Text = dr[5].ToString();
                    diff_day = Convert.ToInt32(dr[5].ToString());
                    InterestText.Text = dr[6].ToString();
                    TotalDebtValue.Value = Convert.ToDecimal(dr[7].ToString());
                    OneDayInterestValue.Value = Convert.ToDecimal(dr[8].ToString());
                    interest_amount = Convert.ToDouble(dr[9].ToString());
                    MonthlyPaymentValue.Value = Convert.ToDecimal(dr[10].ToString());
                    RequiredValue.Value = Convert.ToDecimal(dr[11].ToString());
                    currency_id = Convert.ToInt32(dr[14].ToString());
                    if (currency_id != 1)
                    {
                        CurrencyRateLabel.Visible = true;
                        CurrencyRateValue.Visible = true;
                        RateAZNLabel.Visible = true;
                        PaymentAZNLabel.Visible = true;
                        PaymentAZNValue.Visible = true;
                        CurrencyRateLabel.Text = "1 " + dr[15].ToString() + " = ";
                        CurrencyRateValue.Value = Convert.ToDecimal(dr[20].ToString());
                        PaymentLabel.Text = "Ödənişin məbləği (" + dr[15].ToString() + " - ilə)";
                    }
                    else
                    {
                        PaymentAZNLabel.Visible = false;
                        PaymentAZNValue.Visible = false;
                    }
                    PaymentValue.Value = Convert.ToDecimal(dr[12].ToString());
                    PaymentAZNValue.Value = Convert.ToDecimal(dr[13].ToString());
                    CurrencyLabel.Text = dr[15].ToString();

                    if (!String.IsNullOrEmpty(dr[16].ToString()))
                        LeasingPaymentNoteText.Text = dr[16].ToString();
                    else
                        LeasingPaymentNoteText.Text = null;
                    CustomerID = Convert.ToInt32(dr[17].ToString());
                    ContractID = Convert.ToInt32(dr[18].ToString());
                    leasing_amount = Convert.ToDouble(dr[19].ToString());
                    payment_interest_debt = Convert.ToDouble(dr[21].ToString());
                    PaymentInterestDebtValue.Value = (decimal)payment_interest_debt;
                    PenaltyText.Text = Convert.ToDouble(dr[22].ToString()).ToString("N2");
                    if (Convert.ToInt32(dr[23].ToString()) == 1)
                        PenaltyCheck.Checked = true;
                    PenaltyValue.Value = Math.Abs(Convert.ToDecimal(dr[24].ToString()));
                }
                if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CUSTOMER_PAYMENTS WHERE PAYMENT_DATE < TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY') AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID) > 0)
                {
                    LastDateText.Visible = true;
                    LastDateLabel.Visible = true;
                    LastDateText.Text = Class.GlobalFunctions.GetMaxDate("SELECT MAX(PAYMENT_DATE) FROM CRS_USER.CUSTOMER_PAYMENTS WHERE PAYMENT_DATE < TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY') AND CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID).ToString("d", Class.GlobalVariables.V_CultureInfoAZ);
                    debt = Class.GlobalFunctions.GetAmount("SELECT CP.DEBT FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.CUSTOMER_ID = " + CustomerID + "AND CP.CONTRACT_ID = " + ContractID + " AND CP.PAYMENT_DATE = TO_DATE('" + LastDateText.Text + "','DD/MM/YYYY')");
                    DebtValue.Value = (decimal)debt;
                }
                else
                {
                    LastDateText.Visible = false;
                    LastDateText.Text = ContractStartDateText.Text;
                    LastDateLabel.Visible = false;
                    debt = leasing_amount;
                    DebtValue.Value = (decimal)debt;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadFounderDetails()
        {
            string s = "SELECT F.FULLNAME,CS.SERIES||': '||FC.CARD_NUMBER||', '||TO_CHAR(FC.ISSUE_DATE,'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib.' CARD,TO_CHAR(CF.PAYMENT_DATE,'DD.MM.YYYY'),CF.APPOINTMENT,CF.AMOUNT,CF.NOTE,FC.ID CARD_ID,FP.CONTRACT_ID FROM CRS_USER.CASH_FOUNDER CF,CRS_USER.FOUNDERS F,CRS_USER.FOUNDER_CARDS FC,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI,CRS_USER.FUNDS_PAYMENTS FP WHERE CF.FOUNDER_ID = F.ID AND CF.FOUNDER_CARD_ID = FC.ID AND FC.FOUNDER_ID = F.ID AND FC.CARD_SERIES_ID = CS.ID AND FC.CARD_ISSUING_ID = CI.ID AND CF.FUND_PAYMENT_ID = FP.ID AND CF.INC_EXP = 1 AND CF.ID = " + OperationOwnerID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    FounderComboBox.EditValue = dr[0].ToString();
                    FounderCardText.Text = dr[1].ToString();
                    FounderDate.EditValue = Class.GlobalFunctions.ChangeStringToDate(dr[2].ToString(), "ddmmyyyy");
                    FounderAppointmentText.Text = dr[3].ToString();
                    FounderAmountValue.Value = Convert.ToDecimal(dr[4].ToString());
                    FounderNoteText.Text = dr[5].ToString();
                    founder_card_id = Convert.ToInt32(dr[6].ToString());
                    fund_contract_id = Convert.ToInt32(dr[7].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçi ilə hesablaşmanın rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadBankAccountDetails()
        {
            string s = $@"SELECT TO_CHAR(BA.ADATE, 'DD.MM.YYYY') ADATE,
                                   B.LONG_NAME BANK_NAME,
                                   BA.APPOINTMENT,
                                   BA.AMOUNT,
                                   BA.NOTE
                              FROM CRS_USER.CASH_BANK_ACCOUNT BA, CRS_USER.BANKS B
                             WHERE BA.INC_EXP = 1 AND BA.BANK_ID = B.ID AND BA.ID = {OperationOwnerID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);
                foreach (DataRow dr in dt.Rows)
                {
                    old_bank_account_date = dr["ADATE"].ToString();
                    AccountDate.EditValue = GlobalFunctions.ChangeStringToDate(dr["ADATE"].ToString(), "ddmmyyyy");
                    BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                    AccountAppointmentText.Text = dr["APPOINTMENT"].ToString();
                    AccountAmountValue.Value = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    AccountNoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bank hesabının rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadOtherPaymentDetails()
        {
            string s = "SELECT AP.CUSTOMER_NAME,TO_CHAR(AP.PAYMENT_DATE,'DD.MM.YYYY'),CA.NAME APPOINTMENT,AP.AMOUNT,AP.NOTE FROM CRS_USER.CASH_OTHER_PAYMENT AP,CRS_USER.CASH_APPOINTMENTS CA WHERE CA.ID = AP.CASH_APPOINTMENT_ID AND AP.ID = " + OperationOwnerID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    OtherCustomerNameText.Text = dr[0].ToString();
                    OtherDate.EditValue = Class.GlobalFunctions.ChangeStringToDate(dr[1].ToString(), "ddmmyyyy");
                    OtherAppointmentComboBox.EditValue = dr[2].ToString();
                    OtherAmountValue.Value = Convert.ToDecimal(dr[3].ToString());
                    OtherNoteText.Text = dr[4].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Digər ödənişin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadServicePriceDetails()
        {
            string s = $@"SELECT AP.CUSTOMER_NAME,TO_CHAR(AP.PAYMENT_DATE,'DD.MM.YYYY'),AP.APPOINTMENT,AP.AMOUNT,AP.NOTE FROM CRS_USER.CASH_SERVICE_PRICE AP WHERE AP.ID = {OperationOwnerID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    ServiceCustomerNameText.Text = dr[0].ToString();
                    ServiceDate.EditValue = GlobalFunctions.ChangeStringToDate(dr[1].ToString(), "ddmmyyyy");
                    ServiceAppointmentText.Text = dr[2].ToString();
                    ServiceAmountValue.Value = Convert.ToDecimal(dr[3].ToString());
                    ServiceNoteText.Text = dr[4].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Xidmət haqqının rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LeasingDetailWithNull()
        {
            ContractStartDateText.Text =
                    CustomerNameText.Text =
                    LastDateText.Text =
                    NameText.Text =
                    InterestText.Text =
                    CurrencyLabel.Text =
                    DayCountText.Text =
                    PenaltyText.Text = null;

            DebtValue.Value =
                PaymentInterestDebtValue.Value =
                OneDayInterestValue.Value =
                MonthlyPaymentValue.Value =
                TotalDebtValue.Value = 0;
            interest_amount = 0;

            RequiredValue.Value =
                PaymentAZNValue.Value =
                PaymentValue.Value =
                PenaltyValue.Value = 0;

            CurrencyRateLabel.Visible =
                CurrencyRateValue.Visible =
                RateAZNLabel.Visible =
                CommitmentLabel.Visible =
                PenaltyCheck.Checked = 
                PaymentsLabel.Visible = false;

            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            CustomerID = ContractID = 0;
            PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate("01.01.1900", "ddmmyyyy");
        }

        private void LeasingContractCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (LeasingContractCodeText.Text.Length != 4)
            {
                LeasingDetailWithNull();
                return;
            }

            if (!FormStatus)
                return;

            double amount = 0;
            int interest = 24;

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
                                       AND C.CONTRACT_CODE = '{LeasingContractCodeText.Text.Trim()}'";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                PaymentsLabel.Visible = true;
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
                        currency = dr["CURRENCY_CODE"].ToString();
                        interest = Convert.ToInt32(dr["INTEREST"].ToString());
                        monthly_amount = Convert.ToDouble(dr["MONTHLY_AMOUNT"].ToString());
                        MonthlyPaymentValue.Value = (decimal)monthly_amount;
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
                        payment_interest_debt = 0;
                    }
                    else
                    {
                        LastDateText.Visible = LastDateLabel.Visible = true;
                        var lastPayments = lstPayments.Where(item => item.IS_CHANGE == 0 || item.IS_CHANGE == 1).LastOrDefault();

                        LastDateText.Text = lastPayments.PAYMENT_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
                        debt = Math.Round((double)lastPayments.DEBT, 2);
                        payment_interest_debt = Math.Round((double)lastPayments.PAYMENT_INTEREST_DEBT, 2);                        
                    }

                    if (currency_id == 1)
                    {
                        PaymentAZNLabel.Visible = PaymentAZNValue.Visible = false;
                        PaymentLabel.Text = "Ödənişin məbləği";
                    }
                    else
                    {
                        PaymentAZNLabel.Visible = PaymentAZNValue.Visible = true;
                        PaymentLabel.Text = "Ödənişin məbləği (" + currency + " - ilə)";
                    }

                    CurrencyLabel.Text = currency;
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
                    DebtPenalty = GlobalFunctions.GetAmount($@"SELECT DEBT_PENALTY FROM CRS_USER.CONTRACT_BALANCE_PENALTIES BP WHERE BAL_DATE = (SELECT MAX(BAL_DATE) FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE IS_COMMIT = 1 AND CUSTOMER_ID = BP.CUSTOMER_ID AND CONTRACT_ID = BP.CONTRACT_ID) AND BP.CONTRACT_ID = {ContractID}");
                    PenaltyText.Text = DebtPenalty.ToString("N2");
                    PenaltyCurrencyLabel.Text = currency;
                    PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
                    PaymentDate_EditValueChanged(sender, EventArgs.Empty);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Müqavilənin rekvizitləri tapılmadı.", sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
            else
                LeasingDetailWithNull();

            //    using (OracleConnection connection = new OracleConnection())
            //    {
            //        OracleCommand command = null;
            //        try
            //        {
            //            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            //            {
            //                connection.ConnectionString = GlobalFunctions.GetConnectionString();
            //                connection.Open();
            //            }
            //            command = connection.CreateCommand();
            //            command.CommandText = "SELECT C.ID CONTRACT_ID,C.CUSTOMER_ID,TO_CHAR(C.START_DATE,'DD.MM.YYYY') S_DATE,CUS.SURNAME||' '||CUS.NAME||' '||CUS.PATRONYMIC||' '||DECODE(CUS.SEX_ID,1,'oğlu','qızı') CUSTOMER_NAME,C.CURRENCY_ID,C.AMOUNT,CUR.CODE,(CASE WHEN C.CHECK_INTEREST > -1 THEN C.CHECK_INTEREST ELSE CT.INTEREST END) INTEREST,C.MONTHLY_AMOUNT FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT,CRS_USER.CUSTOMERS CUS,CRS_USER.CURRENCY CUR WHERE C.USED_USER_ID = -1 AND C.STATUS_ID = 5 AND CUR.ID = C.CURRENCY_ID AND C.CREDIT_TYPE_ID = CT.ID AND C.CUSTOMER_ID = CUS.ID AND CT.CODE||C.CODE = '" + LeasingContractCodeText.Text.Trim() + "'";
            //            Class.GlobalVariables.V_Reader = command.ExecuteReader();

                //            if (Class.GlobalVariables.V_Reader.HasRows)
                //            {
                //                while (Class.GlobalVariables.V_Reader.Read())
                //                {
                //                    ContractID = Class.GlobalVariables.V_Reader.GetInt32(0);
                //                    CustomerID = Class.GlobalVariables.V_Reader.GetInt32(1);
                //                    ContractStartDateText.Text = Class.GlobalVariables.V_Reader.GetString(2);
                //                    CustomerNameText.Text = Class.GlobalVariables.V_Reader.GetString(3);
                //                    currency_id = Class.GlobalVariables.V_Reader.GetInt32(4);
                //                    amount = Class.GlobalVariables.V_Reader.GetDouble(5);
                //                    currency = Class.GlobalVariables.V_Reader.GetString(6);
                //                    interest = Class.GlobalVariables.V_Reader.GetInt32(7);
                //                    MonthlyPaymentValue.Value = Class.GlobalVariables.V_Reader.GetDecimal(8);
                //                    monthly_amount = Class.GlobalVariables.V_Reader.GetDouble(8);
                //                }
                //                Class.GlobalVariables.V_Reader.Close();

                //                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");

                //                if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.CONTRACT_ID = " + ContractID) == 0)
                //                {
                //                    LastDateText.Visible = false;
                //                    LastDateText.Text = ContractStartDateText.Text;
                //                    LastDateLabel.Visible = false;
                //                    debt = amount;
                //                }
                //                else
                //                {
                //                    LastDateText.Visible = true;
                //                    LastDateLabel.Visible = true;
                //                    LastDateText.Text = GlobalFunctions.GetMaxDate("SELECT MAX(CP.PAYMENT_DATE) FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.CONTRACT_ID = " + ContractID).ToString("d", GlobalVariables.V_CultureInfoAZ);
                //                    debt = GlobalFunctions.GetAmount("SELECT CP.DEBT FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.CONTRACT_ID = " + ContractID + " AND CP.PAYMENT_DATE = TO_DATE('" + LastDateText.Text + "','DD/MM/YYYY')");
                //                }

                //                if (currency_id == 1)
                //                {
                //                    PaymentAZNLabel.Visible = PaymentAZNValue.Visible = false;
                //                    PaymentLabel.Text = "Ödənişin məbləği";
                //                }
                //                else
                //                {
                //                    PaymentAZNLabel.Visible = PaymentAZNValue.Visible = true;
                //                    PaymentLabel.Text = "Ödənişin məbləği (" + currency + " - ilə)";
                //                }

                //                CurrencyLabel.Text = currency;
                //                DebtValue.Value = (decimal)debt;
                //                InterestText.Text = interest.ToString();
                //                one_day_interest = Math.Round(((debt * interest) / 100) / 360, 2);
                //                OneDayInterestValue.Value = (decimal)one_day_interest;
                //                payment_interest_debt = Class.GlobalFunctions.GetAmount("SELECT CP.PAYMENT_INTEREST_DEBT FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.CUSTOMER_ID = " + CustomerID + "AND CP.CONTRACT_ID = " + ContractID + " AND CP.PAYMENT_DATE = TO_DATE('" + LastDateText.Text + "','DD/MM/YYYY')");
                //                string commitment_name = Class.GlobalFunctions.GetName($@"SELECT CC.COMMITMENT_NAME FROM CRS_USER.V_COMMITMENTS CC WHERE CC.ID = {ContractID}");
                //                if (String.IsNullOrEmpty(commitment_name))
                //                {
                //                    CommitmentLabel.Visible = false;
                //                    NameText.Text = CustomerNameText.Text;
                //                }
                //                else
                //                {
                //                    CommitmentLabel.Visible = true;
                //                    NameText.Text = commitment_name;
                //                }
                //                DebtPenalty = GlobalFunctions.GetAmount("SELECT DEBT_PENALTY FROM CRS_USER.CONTRACT_BALANCE_PENALTIES BP WHERE BAL_DATE = (SELECT MAX(BAL_DATE) FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE IS_COMMIT = 1 AND CUSTOMER_ID = BP.CUSTOMER_ID AND CONTRACT_ID = BP.CONTRACT_ID) AND BP.CUSTOMER_ID = " + CustomerID + " AND BP.CONTRACT_ID = " + ContractID);
                //                PenaltyText.Text = DebtPenalty.ToString("N2");
                //                PenaltyCurrencyLabel.Text = currency;
                //                PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy");
                //                PaymentDate_EditValueChanged(sender, EventArgs.Empty);
                //            }
                //            else
                //            {
                //                ContractStartDateText.Text = null;
                //                CustomerNameText.Text = null;
                //                LastDateText.Text = null;
                //                NameText.Text = null;
                //                InterestText.Text = null;
                //                CurrencyLabel.Text = null;
                //                DebtValue.Value = 0;
                //                CommitmentLabel.Visible = false;
                //                PaymentInterestDebtValue.Value = 0;
                //                OneDayInterestValue.Value = 0;
                //                MonthlyPaymentValue.Value = 0;
                //                TotalDebtValue.Value = 0;
                //                interest_amount = 0;
                //                DayCountText.Text = null;
                //                RequiredValue.Value = 0;
                //                PaymentAZNValue.Value = 0;
                //                PaymentValue.Value = 0;
                //                CurrencyRateLabel.Visible = false;
                //                CurrencyRateValue.Visible = false;
                //                RateAZNLabel.Visible = false;
                //                PenaltyText.Text = null;
                //                PenaltyCheck.Checked = false;
                //                PenaltyValue.Value = 0;
                //                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                //                CustomerID = 0;
                //                ContractID = 0;
                //                PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate("01.01.1900", "ddmmyyyy");
                //            }
                //        }
                //        catch (Exception exx)
                //        {
                //            GlobalProcedures.LogWrite("Müqavilənin rekvizitləri tapılmadı.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                //        }
                //        finally
                //        {
                //            command.Dispose();
                //            connection.Dispose();
                //        }
                //    }

        }

        private void PaymentDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            
            int order_id;
            if ((!String.IsNullOrEmpty(PaymentDate.Text)) && (!String.IsNullOrEmpty(LastDateText.Text)))
            {
                if (currency_id != 1)
                {
                    CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                    cur = GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                    CurrencyRateLabel.Text = "1 " + currency + " = ";
                    CurrencyRateValue.Value = (decimal)cur;
                }
                else if (currency_id == 1)
                    cur = 1;

                diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(LastDateText.Text, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy"));
                DayCountText.Text = diff_day.ToString();
                interest_amount = diff_day * one_day_interest;
                double diff = GlobalFunctions.GetAmount($@"SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}");
                //sum_interest_amount = interest_amount + GlobalFunctions.GetAmount("SELECT NVL(SUM(INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                //payment_interest_amount = GlobalFunctions.GetAmount("SELECT NVL(SUM(CP.PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP WHERE CP.CONTRACT_ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                residual_percent = Math.Round(interest_amount + diff, 2);
                PaymentInterestDebtValue.Value = (decimal)residual_percent;
                totaldebtamount = Math.Round((debt + residual_percent), 2);
                TotalDebtValue.Value = (decimal)totaldebtamount;
                List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(ContractID).ToList<PaymentSchedules>();
                var schedules = lstSchedules.Where(s => s.MONTH_DATE <= GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy")).ToList<PaymentSchedules>();
                if (schedules.Count == 0)
                    order_id = 0;
                else
                    order_id = schedules.Max(s => s.ORDER_ID);

                if (order_id == 0)
                    normal_debt = debt;
                else
                    normal_debt = Math.Round(lstSchedules.Find(s => s.ORDER_ID == order_id).DEBT, 2); //qrafik uzre odenis
                if ((debt - normal_debt) > 0)
                    requiredamount = (debt - normal_debt) + residual_percent;
                else
                    requiredamount = residual_percent;
                RequiredValue.Value = (decimal)requiredamount;
                PaymentValue_EditValueChanged(sender, EventArgs.Empty);
                PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
            }
        }

        private void PaymentValue_EditValueChanged(object sender, EventArgs e)
        {
            PaymentAZNValue.Value = (decimal)((double)PaymentValue.Value * (double)CurrencyRateValue.Value);
            CurrencyRateValue.BackColor = cur != (double)CurrencyRateValue.Value ? Color.GreenYellow : GlobalFunctions.ElementColor();
            //if (cur != (double)CurrencyRateValue.Value)
            //    CurrencyRateValue.BackColor = Color.GreenYellow;
            //else
            //    CurrencyRateValue.BackColor = GlobalFunctions.ElementColor();
        }

        private void PaymentAZNValue_EditValueChanged(object sender, EventArgs e)
        {
            PaymentValue.Value = CurrencyRateValue.Value != 0 ? (decimal)((double)PaymentAZNValue.Value / (double)CurrencyRateValue.Value) : 0;
            //if (CurrencyRateValue.Value != 0)
            //    PaymentValue.Value = (decimal)((double)PaymentAZNValue.Value / (double)CurrencyRateValue.Value);
            //else
            //    PaymentValue.Value = 0;
        }

        private void LeasingPaymentNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (LeasingPaymentNoteText.Text.Length <= 400)
                LeasingDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - LeasingPaymentNoteText.Text.Length).ToString();
        }

        private void InsertIncome(int selectedpageindex)
        {
            switch (selectedpageindex)
            {
                case 0: //lizinq odenisi
                    {
                        close_form = true;
                        InsertLeasingPayment();
                        InsertBalancePenalty();
                        if (currency_id == 1)
                            GlobalProcedures.InsertCashOperation(1, int.Parse(PaymentID), PaymentDate.Text, LeasingContractCodeText.Text, (double)PaymentValue.Value, 0, 1);
                        else
                            GlobalProcedures.InsertCashOperation(1, int.Parse(PaymentID), PaymentDate.Text, LeasingContractCodeText.Text, (double)PaymentAZNValue.Value, 0, 1);
                        UpdateLeasingPaymentStatus();
                        if (ReceiptCheck.Checked)
                            GlobalProcedures.LoadReceipt(PaymentDate.Text, (double)PaymentAZNValue.Value, currency_id, CurrencyRateValue.Value, CustomerNameText.Text.Trim(), ContractID, LeasingContractCodeText.Text.Trim(), "saylı lizinq müqaviləsi üzrə ödəniş");
                        operationdate = PaymentDate.Text;
                    }
                    break;
                case 2://tesisci ile hesablasma 
                    {
                        double diff = 0, debt = GlobalFunctions.GetAmount("SELECT NVL(SUM(BUY_AMOUNT) - SUM(BASIC_AMOUNT),0) AMOUNT FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id);
                        diff = (double)FounderAmountValue.Value + debt;
                        if (diff < 0)
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("Əgər seçilmiş təsisçi üçün bu hesablaşma əməliyyatını etsəz, bu zaman təsisçinin cəlb olunmuş vəsaitlərdə qalığı mənfi olacaq. Buna razısınız?", "Təsisçinin cəl olunmuş vəsaitlərdəki qalığı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                InsertCashFounderPayment();
                                InsertBuyAmount();
                                close_form = true;
                            }
                            else
                                close_form = false;
                        }
                        else
                        {
                            close_form = true;
                            InsertCashFounderPayment();
                            InsertBuyAmount();
                        }
                        operationdate = FounderDate.Text;
                    }
                    break;
                case 4: //Bank hesabi
                    {
                        close_form = true;
                        InsertBankAccount();
                        InsertBankOperationAccount();
                        operationdate = AccountDate.Text;
                    }
                    break;
                case 6: //diger odenisler 
                    {
                        close_form = true;
                        InsertCashOtherPayment();
                        operationdate = OtherDate.Text;
                    }
                    break;
                case 8://xidmet haqqi
                    {
                        close_form = true;
                        InsertCashServicePrice();
                        operationdate = ServiceDate.Text;
                    }
                    break;
            }
        }

        private void UpdateIncome(int selectedpageindex)
        {
            switch (selectedpageindex)
            {
                case 0: //lizinq odenisi
                    {
                        close_form = true;
                        GlobalProcedures.UpdateCustomerPayment(0,
                                                                PaymentDate.Text,
                                                                PaymentDate.Text,
                                                                ContractID,
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
                                                                Convert.ToInt32(OperationOwnerID),
                                                                (double)PenaltyValue.Value,
                                                                is_penalty,
                                                                0,
                                                                0,
                                                                null);
                        if (currency_id == 1)
                            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 1, int.Parse(OperationOwnerID), PaymentDate.Text, (double)PaymentValue.Value, 0);
                        else
                            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 1, int.Parse(OperationOwnerID), PaymentDate.Text, (double)PaymentAZNValue.Value, 0);
                        GlobalProcedures.LoadReceipt(PaymentDate.Text, (double)PaymentAZNValue.Value, currency_id, CurrencyRateValue.Value, CustomerNameText.Text.Trim(), ContractID, LeasingContractCodeText.Text.Trim(), "saylı lizinq müqaviləsi üzrə ödəniş");
                        operationdate = PaymentDate.Text;
                        GlobalProcedures.CalculatedLeasingTotal(ContractID);
                    }
                    break;
                case 2://tesisci ile hesablasma 
                    {
                        double diff = 0, debt = Class.GlobalFunctions.GetAmount("SELECT NVL(SUM(BUY_AMOUNT) - SUM(BASIC_AMOUNT),0) AMOUNT FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id);
                        diff = (double)FounderAmountValue.Value + debt;
                        if (diff < 0)
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("Əgər seçilmiş təsisçi üçün bu hesablaşma əməliyyatını etsəz, bu zaman təsisçinin cəlb olunmuş vəsaitlərdə qalığı mənfi olacaq. Buna razısınız?", "Təsisçinin cəl olunmuş vəsaitlərdəki qalığı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                UpdateCashFounderPayment();
                                UpdateBuyAmount();
                                close_form = true;
                            }
                            else
                                close_form = false;
                        }
                        else
                        {
                            close_form = true;
                            UpdateCashFounderPayment();
                            UpdateBuyAmount();
                        }
                        operationdate = FounderDate.Text;
                    }
                    break;
                case 4://Bank hesabi 
                    {
                        close_form = true;
                        UpdateBankAccount();
                        InsertBankOperationAccount();
                        operationdate = AccountDate.Text;
                    }
                    break;
                case 6://diger odenisler 
                    {
                        close_form = true;
                        UpdateCashOtherPayment();
                        operationdate = OtherDate.Text;
                    }
                    break;
                case 8://xidmet haqqi
                    {
                        close_form = true;
                        UpdateCashServicePrice();
                        operationdate = ServiceDate.Text;
                    }
                    break;
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlIncomeDetails(IncomeBackstageViewControl.SelectedTabIndex))
            {
                if (TransactionName == "INSERT")
                    InsertIncome(IncomeBackstageViewControl.SelectedTabIndex);
                else
                    UpdateIncome(IncomeBackstageViewControl.SelectedTabIndex);

                if (IncomeBackstageViewControl.SelectedTabIndex == 2)
                    InsertFundsPayments();

                if (close_form)
                    this.Close();
                RefreshCashDataGridView(operationdate);
            }
        }

        private void InsertFundsPayments()
        {
            Class.GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER.FUNDS_PAYMENTS WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
                                                    "INSERT INTO CRS_USER.FUNDS_PAYMENTS(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,MANUAL_INTEREST FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE = 1 AND CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "Ödənişlər əsas cədvələ daxil edilmədi.");
        }

        private bool ControlIncomeDetails(int index)
        {
            bool b = false;

            switch (index)
            {
                case 0:
                    {
                        double sumpenalty = 0, paymentpenalty = 0;
                        if (String.IsNullOrEmpty(LeasingContractCodeText.Text))
                        {
                            LeasingContractCodeText.BackColor = Color.Red;
                            XtraMessageBox.Show("Müqavilənin nömrəsi daxil edilməyib.");
                            LeasingContractCodeText.Focus();
                            LeasingContractCodeText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (CustomerID == 0 || ContractID == 0)
                        {
                            LeasingContractCodeText.BackColor = Color.Red;
                            XtraMessageBox.Show(LeasingContractCodeText.Text + " saylı lizinq müqaviləsinə uyğun məlumatlar tapılmadı.");
                            LeasingContractCodeText.Focus();
                            LeasingContractCodeText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(NameText.Text))
                        {
                            NameText.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödəyənin adı daxil edilməyib.");
                            NameText.Focus();
                            NameText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(PaymentDate.Text))
                        {
                            PaymentDate.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödənişin tarixi daxil edilməyib.");
                            PaymentDate.Focus();
                            PaymentDate.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (PaymentValue.Value <= 0)
                        {
                            PaymentValue.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödənişin məbləği daxil edilməyib.");
                            PaymentValue.Focus();
                            PaymentValue.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (PaymentAZNValue.Value <= 0 && currency_id != 1)
                        {
                            CurrencyRateValue.BackColor = Color.Red;
                            XtraMessageBox.Show(PaymentDate.Text + " tarixi üçün " + CurrencyLabel.Text + " valyutasının AZN qarşılığı daxil edilməyib.");
                            CurrencyRateValue.Focus();
                            CurrencyRateValue.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (PaymentValue.Value > TotalDebtValue.Value)
                        {
                            PaymentValue.BackColor = Color.Red;
                            TotalDebtValue.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödənişin məbləği 'Tam bağlamaq üçün tələb olunan məbləğ' - dən çox ola bilməz.");
                            PaymentValue.Focus();
                            PaymentValue.BackColor = Class.GlobalFunctions.ElementColor();
                            TotalDebtValue.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (PenaltyValue.Value > Convert.ToDecimal(PenaltyText.Text))
                        {
                            PenaltyValue.BackColor = Color.Red;
                            PenaltyText.BackColor = Color.Red;
                            XtraMessageBox.Show("Silinəcək cərimə faizi balansa daxil edilmiş cərimə fazinin qalığından (" + PenaltyText.Text + " " + PenaltyCurrencyLabel.Text + ") çox ola bilməz.");
                            PenaltyValue.Focus();
                            PenaltyValue.BackColor = Class.GlobalFunctions.ElementColor();
                            PenaltyText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else if (PenaltyValue.Value <= Convert.ToDecimal(PenaltyText.Text))
                        {
                            sumpenalty = Class.GlobalFunctions.GetAmount("SELECT NVL(SUM(PENALTY_AMOUNT),0) FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND IS_COMMIT = 1");
                            paymentpenalty = Class.GlobalFunctions.GetAmount("SELECT NVL(SUM(PAYMENT_PENALTY),0) FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE CUSTOMER_ID = " + CustomerID + " AND CONTRACT_ID = " + ContractID + " AND IS_COMMIT = 1 AND BAL_DATE > TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY')");
                            if ((sumpenalty - paymentpenalty) < (double)PenaltyValue.Value)
                            {
                                PenaltyValue.BackColor = Color.Red;
                                XtraMessageBox.Show(PaymentDate.Text + " tarixindən sonrakı tarixlərdə də cərimə faizləri silindiyi üçün silinən cərimə faizinin məbləği ən çoxu " + (sumpenalty - paymentpenalty).ToString(Class.GlobalVariables.V_CultureInfoAZ) + " ola bilər.");
                                PenaltyValue.Focus();
                                PenaltyValue.BackColor = Class.GlobalFunctions.ElementColor();
                                return false;
                            }
                            else
                                b = true;
                        }
                        else
                            b = true;
                    }
                    break;
                case 2:
                    {
                        if (String.IsNullOrEmpty(FounderDate.Text))
                        {
                            FounderDate.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödənişin tarixi daxil edilməyib.");
                            FounderDate.Focus();
                            FounderDate.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(FounderComboBox.Text))
                        {
                            FounderComboBox.BackColor = Color.Red;
                            XtraMessageBox.Show("Təsisçinin adı daxil edilməyib.");
                            FounderComboBox.Focus();
                            FounderComboBox.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (fund_contract_id == 0)
                        {
                            FounderComboBox.BackColor = Color.Red;
                            XtraMessageBox.Show("Seçilmiş təsisçinin heç bir müqaviləsi olmadığı üçün kassaya mədaxil etmək olmaz.");
                            FounderComboBox.Focus();
                            FounderComboBox.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(FounderAppointmentText.Text))
                        {
                            FounderAppointmentText.BackColor = Color.Red;
                            XtraMessageBox.Show("Təyinat daxil edilməyib.");
                            FounderAppointmentText.Focus();
                            FounderAppointmentText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (FounderAmountValue.Value <= 0)
                        {
                            FounderAmountValue.BackColor = Color.Red;
                            XtraMessageBox.Show("Məbləğ daxil edilməyib.");
                            FounderAmountValue.Focus();
                            FounderAmountValue.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (TransactionName == "INSERT" && Class.GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.FUNDS_PAYMENTS WHERE PAYMENT_DATE = TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY') AND BUY_AMOUNT > 0 AND CONTRACT_ID = " + fund_contract_id) > 0)
                        {
                            FounderDate.BackColor = Color.Red;
                            XtraMessageBox.Show(FounderDate.Text + " tarixinə artıq ödəniş olunub.");
                            FounderDate.Focus();
                            FounderDate.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;
                    }
                    break;
                case 4:
                    {
                        if (String.IsNullOrEmpty(AccountDate.Text))
                        {
                            AccountDate.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödənişin tarixi daxil edilməyib.");
                            AccountDate.Focus();
                            AccountDate.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (BankLookUp.EditValue == null)
                        {
                            BankLookUp.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Bankın adı daxil edilməyib.");
                            BankLookUp.Focus();
                            BankLookUp.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(AccountAppointmentText.Text))
                        {
                            AccountAppointmentText.BackColor = Color.Red;
                            XtraMessageBox.Show("Təyinat daxil edilməyib.");
                            AccountAppointmentText.Focus();
                            AccountAppointmentText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (AccountAmountValue.Value <= 0)
                        {
                            AccountAmountValue.BackColor = Color.Red;
                            XtraMessageBox.Show("Məbləğ daxil edilməyib.");
                            AccountAmountValue.Focus();
                            AccountAmountValue.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;
                    }
                    break;
                case 6:
                    {
                        if (String.IsNullOrEmpty(OtherDate.Text))
                        {
                            OtherDate.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödənişin tarixi daxil edilməyib.");
                            OtherDate.Focus();
                            OtherDate.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherCustomerNameText.Text))
                        {
                            OtherCustomerNameText.BackColor = Color.Red;
                            XtraMessageBox.Show("Soyadı, adı və atasının adı daxil edilməyib.");
                            OtherCustomerNameText.Focus();
                            OtherCustomerNameText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherAppointmentComboBox.Text))
                        {
                            OtherAppointmentComboBox.BackColor = Color.Red;
                            XtraMessageBox.Show("Təyinat daxil edilməyib.");
                            OtherAppointmentComboBox.Focus();
                            OtherAppointmentComboBox.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (OtherAmountValue.Value <= 0)
                        {
                            OtherAmountValue.BackColor = Color.Red;
                            XtraMessageBox.Show("Məbləğ sıfırdan böyük olmalıdır.");
                            OtherAmountValue.Focus();
                            OtherAmountValue.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;
                    }
                    break;
                case 8:
                    {
                        if (String.IsNullOrEmpty(ServiceDate.Text))
                        {
                            ServiceDate.BackColor = Color.Red;
                            XtraMessageBox.Show("Ödənişin tarixi daxil edilməyib.");
                            ServiceDate.Focus();
                            ServiceDate.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceCustomerNameText.Text))
                        {
                            ServiceCustomerNameText.BackColor = Color.Red;
                            XtraMessageBox.Show("Soyadı, adı və atasının adı daxil edilməyib.");
                            ServiceCustomerNameText.Focus();
                            ServiceCustomerNameText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceAppointmentText.Text))
                        {
                            ServiceAppointmentText.BackColor = Color.Red;
                            XtraMessageBox.Show("Təyinat daxil edilməyib.");
                            ServiceAppointmentText.Focus();
                            ServiceAppointmentText.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (ServiceAmountValue.Value <= 0)
                        {
                            ServiceAmountValue.BackColor = Color.Red;
                            XtraMessageBox.Show("Məbləğ sıfırdan böyük olmalıdır.");
                            ServiceAmountValue.Focus();
                            ServiceAmountValue.BackColor = Class.GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;
                    }
                    break;
            }

            return b;
        }

        private void InsertLeasingPayment()
        {
            double payment_interest_amount = 0, currenct_payment_interest_debt = 0, basic_amount = 0, current_debt = 0, total = 0, payment_value_AZN = 0, penalty = 0;
            PaymentID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_PAYMENT_SEQUENCE").ToString();
            if (PaymentValue.Value > 0 && BasicAmountCheck.Checked) // eger odenis sifirdan boyuk olarsa
            {
                if ((interest_amount + payment_interest_debt) > (double)PaymentValue.Value) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                    payment_interest_amount = (double)PaymentValue.Value;
                else
                    payment_interest_amount = interest_amount + payment_interest_debt; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
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

            currenct_payment_interest_debt = payment_interest_debt + interest_amount - payment_interest_amount;

            current_debt = debt - basic_amount;
            total = current_debt + currenct_payment_interest_debt;
            if (currency_id == 1)
                payment_value_AZN = (double)PaymentValue.Value;
            else
                payment_value_AZN = (double)PaymentAZNValue.Value;

            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CUSTOMER_PAYMENTS(ID,CUSTOMER_ID,CONTRACT_ID,PAYMENT_DATE,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,REQUIRED_AMOUNT,PAYMENT_NAME,NOTE,BANK_CASH,PENALTY_DEBT,IS_PENALTY,PENALTY_AMOUNT,IS_BASIC_AMOUNT)VALUES(" + PaymentID + "," + CustomerID + "," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY')," + Math.Round(PaymentValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(basic_amount, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(current_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + diff_day + "," + Math.Round(one_day_interest, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(interest_amount, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(payment_interest_amount, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(currenct_payment_interest_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(total, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(CurrencyRateValue.Value, 4).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(payment_value_AZN, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + totaldebtamount.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + requiredamount.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + NameText.Text.Trim() + "','" + LeasingPaymentNoteText.Text.Trim() + "','C'," + Math.Round(DebtPenalty, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + is_penalty + "," + Math.Round(penalty_amount, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + ((BasicAmountCheck.Checked) ? 0 : 1) + ")",
                                                "Ödəniş temp cədvələ daxil edilmədi.");
            GlobalProcedures.InsertOperationJournal(PaymentDate.Text, (double)CurrencyRateValue.Value, currency_id, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, basic_amount, payment_interest_amount, ContractID.ToString(), PaymentID, null, 0, PaymentDate.Text);
            GlobalProcedures.CalculatedLeasingTotal(ContractID);
        }

        private void InsertBalancePenalty()
        {
            if (PenaltyCheck.Checked)
            {
                string PenaltyID = Class.GlobalFunctions.GetOracleSequenceValue("BALANCE_PENALTIES_SEQUENCE").ToString();
                double currentdebtpenalty = DebtPenalty - penalty_amount;
                Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CONTRACT_BALANCE_PENALTIES(ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_COMMIT,PENALTY_STATUS,CUSTOMER_PAYMENT_ID) VALUES(" + PenaltyID + "," + CustomerID + "," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY'),0,0," + currentdebtpenalty.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + penalty_amount.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",1,'Ödənilib'," + PaymentID + ")",
                                                    "Cərimə cədvələ daxil edilmədi.");
            }
        }

        private void UpdateLeasingPaymentStatus()
        {
            double total = Class.GlobalFunctions.GetAmount("SELECT NVL(TOTAL,0) FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.CUSTOMER_ID = " + CustomerID + " AND CP.CONTRACT_ID = " + ContractID + " AND CP.PAYMENT_DATE = (SELECT MAX(PAYMENT_DATE) FROM CRS_USER.CUSTOMER_PAYMENTS WHERE CUSTOMER_ID = CP.CUSTOMER_ID AND CONTRACT_ID = CP.CONTRACT_ID)");
            int paymentcount = Class.GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CUSTOMER_PAYMENTS CP WHERE CP.CUSTOMER_ID = " + CustomerID + " AND CP.CONTRACT_ID = " + ContractID);
            if (total <= 0 && paymentcount > 0)
            {
                Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CONTRACTS SET STATUS_ID = 6 WHERE CUSTOMER_ID = " + CustomerID + " AND ID = " + ContractID,
                                                    "Müqavilənin statusu dəyişdirilmədi.");
                XtraMessageBox.Show(LeasingContractCodeText.Text + " saylı Lizinq Müqaviləsi ödənişlər tam başa çatdığı üçün sistem tərəfindən avtomatik olaraq bağlanıldı.");
            }
        }

        private void FIncomeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CUSTOMER_PAYMENTS", -1, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_ADVANCE_PAYMENTS", -1, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_BANK_ACCOUNT", -1, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_FOUNDER", -1, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_OTHER_PAYMENT", -1, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_SERVICE_PRICE", -1, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_OPERATIONS", -1, "WHERE ID = " + OperationID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            }

            Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", -1, "WHERE ID = " + founder_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", -1, "WHERE ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_PAYMENTS", -1, "WHERE CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            Class.GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE USED_USER_ID = " + Class.GlobalVariables.V_UserID + " AND CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + founder_id + ")",
                                                "Cəlb olunmuş vəsaitlər temp cədvəldən silinmədi.");
            Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND CUSTOMER_ID = " + CustomerID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);


        }

        private void FounderNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (FounderNoteText.Text.Length <= 400)
                FounderDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - FounderNoteText.Text.Length).ToString();
        }

        private void InsertCashFounderPayment()
        {
            FounderID = Class.GlobalFunctions.GetOracleSequenceValue("CASH_FOUNDER_SEQUENCE");
            Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_FOUNDER(ID,FOUNDER_ID,FOUNDER_CARD_ID,PAYMENT_DATE,APPOINTMENT,AMOUNT,INC_EXP,NOTE)VALUES(" + FounderID + "," + founder_id + "," + founder_card_id + ",TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY'),'" + FounderAppointmentText.Text.Trim() + "'," + FounderAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",1,'" + FounderNoteText.Text.Trim() + "')",
                                                "Təsisçi ilə hesablaşmanın mədaxili daxil olunmadı.");
            Class.GlobalProcedures.InsertCashOperation(3, FounderID, FounderDate.Text, null, (double)FounderAmountValue.Value, 0, 1);
        }

        private void UpdateCashFounderPayment()
        {
            Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_FOUNDER SET founder_id = " + founder_id + ",FOUNDER_CARD_ID = " + founder_card_id + ",PAYMENT_DATE = TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY'),APPOINTMENT = '" + FounderAppointmentText.Text.Trim() + "',AMOUNT = " + FounderAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + FounderNoteText.Text.Trim() + "' WHERE ID = " + OperationOwnerID,
                                                "Təsisçi ilə hesablaşmanın mədaxili dəyişdirilmədi.");
            Class.GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 3, int.Parse(OperationOwnerID), FounderDate.Text, (double)FounderAmountValue.Value, 0);
        }

        private void InsertBuyAmount()
        {
            double currenct_payment_interest_debt = 0, total = 0, current_debt = 0, interest_amount = 0, diff_day = 0, Debt = 0, totaldebtamount = 0,
                payment_interest_debt = 0, payment_interest_amount = 0, basic_amount = 0, one_day_interest = 0, residual_percent = 0;
            int pay_count, interest = 0, payment_temp_count = 0;
            string lastdate, PaymentID;
            payment_temp_count = Class.GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id);
            if (payment_temp_count == 0)
                Class.GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE," + Class.GlobalVariables.V_UserID + ",MANUAL_INTEREST FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id,
                                                    "Ödənişlər temp cədvələ daxil edilmədi.");

            if (fund_contract_id > 0)
            {
                PaymentID = Class.GlobalFunctions.GetOracleSequenceValue("FUNDS_PAYMENTS_SEQUENCE").ToString();
                interest = Class.GlobalFunctions.GetID("SELECT INTEREST FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = " + fund_contract_id);
                lastdate = Class.GlobalFunctions.GetMaxDate("SELECT NVL(MAX(CP.PAYMENT_DATE),TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY')) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID).ToString("d", Class.GlobalVariables.V_CultureInfoAZ);
                current_debt = Class.GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Debt = current_debt;
                pay_count = Class.GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY')");
                one_day_interest = Math.Round(((Debt * interest) / 100) / 360, 2);
                diff_day = Class.GlobalFunctions.Days360(Class.GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy"), Class.GlobalFunctions.ChangeStringToDate(FounderDate.Text.Trim(), "ddmmyyyy"));
                interest_amount = diff_day * one_day_interest;
                residual_percent = interest_amount + Class.GlobalFunctions.GetAmount("SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                totaldebtamount = Math.Round((Debt + residual_percent), 2);
                basic_amount = Class.GlobalFunctions.GetAmount("SELECT BASIC_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                payment_interest_debt = Class.GlobalFunctions.GetAmount("SELECT PAYMENT_INTEREST_DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                payment_interest_amount = Class.GlobalFunctions.GetAmount("SELECT PAYMENT_INTEREST_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                if (current_debt <= 0)
                {
                    interest_amount = 0;
                    diff_day = 0;
                    Debt = 0;
                    totaldebtamount = 0;
                }

                if ((pay_count != 0))
                {
                    string m_ldate = Class.GlobalFunctions.GetMaxDate("SELECT MAX(CP.PAYMENT_DATE) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND PAYMENT_DATE < TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND CP.CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID).ToString("d", Class.GlobalVariables.V_CultureInfoAZ);
                    Debt = Class.GlobalFunctions.GetAmount("SELECT CP.DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + m_ldate + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    interest_amount = Class.GlobalFunctions.GetAmount("SELECT CP.INTEREST_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                }
                currenct_payment_interest_debt = payment_interest_debt + interest_amount - payment_interest_amount;
                current_debt = Debt + (double)FounderAmountValue.Value - basic_amount;
                total = current_debt + currenct_payment_interest_debt;

                if (pay_count == 0)
                {
                    Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,IS_CHANGE)VALUES(" + PaymentID + "," + fund_contract_id + ",TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY')," + Math.Round(FounderAmountValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",0,0," + Math.Round(current_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + diff_day + "," + Math.Round(one_day_interest, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(interest_amount, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",0," + Math.Round(currenct_payment_interest_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(total, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",1,0," + totaldebtamount.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + FounderNoteText.Text.Trim() + "'," + Class.GlobalVariables.V_UserID + ",1)",
                                                    "Verilən məbləğ temp cədvələ daxil edilmədi.");
                }
                else
                {
                    total = current_debt + payment_interest_debt;
                    Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET BUY_AMOUNT = " + Math.Round(FounderAmountValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + FounderNoteText.Text.Trim() + "',IS_CHANGE = 1 WHERE PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldə dəyişdirilmədi.");
                    PaymentID = Class.GlobalFunctions.GetID("SELECT ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE = TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID).ToString();
                }
                Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_FOUNDER SET FUND_PAYMENT_ID = " + PaymentID + " WHERE founder_id = " + founder_id + "AND FOUNDER_CARD_ID = " + founder_card_id + " AND PAYMENT_DATE = TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY') AND ID = " + FounderID,
                                                "Təsisçi ilə hesablaşmanın mədaxili dəyişdirilmədi.");
                Class.GlobalProcedures.UpdateFundChange(FounderDate.Text.Trim(), fund_contract_id, 1);
            }
        }

        private void UpdateBuyAmount()
        {
            double total = 0, current_debt = 0, Debt = 0, basic_amount = 0, payment_interest_debt = 0, residual_percent = 0, totaldebtamount = 0, one_day_interest = 0, interest_amount = 0;
            string lastdate;
            int diff_day, interest, payment_id, payment_temp_count;
            payment_temp_count = Class.GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id);
            if (payment_temp_count == 0)
                Class.GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE," + Class.GlobalVariables.V_UserID + ",MANUAL_INTEREST FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id,
                                                    "Ödənişlər temp cədvələ daxil edilmədi.");

            if (fund_contract_id > 0)
            {
                interest = Class.GlobalFunctions.GetID("SELECT INTEREST FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = " + fund_contract_id);
                payment_id = Class.GlobalFunctions.GetID("SELECT FUND_PAYMENT_ID FROM CRS_USER.CASH_FOUNDER WHERE ID = " + OperationOwnerID);
                current_debt = Class.GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                payment_interest_debt = Class.GlobalFunctions.GetAmount("SELECT PAYMENT_INTEREST_DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                lastdate = Class.GlobalFunctions.GetMaxDate("SELECT NVL(MAX(PAYMENT_DATE),TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY')) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE < TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND CONTRACT_ID = " + fund_contract_id).ToString("d", Class.GlobalVariables.V_CultureInfoAZ);
                Debt = Class.GlobalFunctions.GetAmount("SELECT CP.DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY')");
                Debt = current_debt;
                if (current_debt <= 0)
                    Debt = 0;
                one_day_interest = Math.Round(((Debt * interest) / 100) / 360, 2);
                diff_day = Class.GlobalFunctions.Days360(Class.GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy"), Class.GlobalFunctions.ChangeStringToDate(FounderDate.Text.Trim(), "ddmmyyyy"));
                interest_amount = diff_day * one_day_interest;
                residual_percent = interest_amount + Class.GlobalFunctions.GetAmount("SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                basic_amount = Class.GlobalFunctions.GetAmount("SELECT BASIC_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                current_debt = Debt + (double)FounderAmountValue.Value - basic_amount;
                total = current_debt + payment_interest_debt;
                totaldebtamount = Math.Round((Debt + residual_percent), 2);

                if (FounderAmountValue.Value > 0)
                    GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 1, BUY_AMOUNT = " + Math.Round(FounderAmountValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + FounderNoteText.Text.Trim() + "',REQUIRED_CLOSE_AMOUNT = " + totaldebtamount.ToString(Class.GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + payment_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldə dəyişdirilmədi.");
                else
                {
                    int pay_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_AMOUNT > 0 AND IS_CHANGE IN (0,1) AND PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY')");
                    if (pay_count == 0)
                        GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 2 WHERE ID = " + payment_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldən silinmədi.");
                    else
                        GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 1, BUY_AMOUNT = " + Math.Round(FounderAmountValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + FounderNoteText.Text.Trim() + "',REQUIRED_CLOSE_AMOUNT = " + totaldebtamount.ToString(Class.GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + payment_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldə dəyişdirilmədi.");
                }
                GlobalProcedures.UpdateFundChange(FounderDate.Text.Trim(), fund_contract_id, 1);
            }
        }

        private void PaymentsLabel_Click(object sender, EventArgs e)
        {
            FShowPayments fsp = new FShowPayments();
            fsp.ContractID = ContractID.ToString();
            fsp.ShowDialog();
        }

        private void InsertBankAccount()
        {
            int AccountID = GlobalFunctions.GetOracleSequenceValue("CASH_BANK_ACCOUNT_SEQUENCE");
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CASH_BANK_ACCOUNT(ID,
                                                                                    ADATE,
                                                                                    BANK_ID,
                                                                                    APPOINTMENT,
                                                                                    AMOUNT,
                                                                                    NOTE,
                                                                                    INC_EXP)
                                                VALUES({AccountID},
                                                        TO_DATE('{AccountDate.Text}','DD/MM/YYYY'),
                                                        {bank_id},
                                                        '{AccountAppointmentText.Text.Trim()}',
                                                        {AccountAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                        '{AccountNoteText.Text.Trim()}',1)",
                                                "Bank hesabının mədaxili daxil olunmadı.");
            GlobalProcedures.InsertCashOperation(4, AccountID, AccountDate.Text, null, (double)AccountAmountValue.Value, 0, 1);
        }

        private void UpdateBankAccount()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_BANK_ACCOUNT SET ADATE = TO_DATE('" + AccountDate.Text + "','DD/MM/YYYY'),BANK_ID = " + bank_id + ",APPOINTMENT = '" + AccountAppointmentText.Text.Trim() + "',AMOUNT = " + AccountAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + AccountNoteText.Text.Trim() + "' WHERE INC_EXP = 1 AND ID = " + OperationOwnerID,
                                                "Bank hesabının mədaxili dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 4, int.Parse(OperationOwnerID), AccountDate.Text, (double)AccountAmountValue.Value, 0);
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

        private void InsertBankOperationAccount()
        {
            string s = $@"SELECT BANK_ID,
                                   TO_CHAR (ADATE, 'DD/MM/YYYY') OPERATION_DATE,
                                   AMOUNT INCOME,
                                   (CASE
                                       WHEN NOTE IS NULL
                                       THEN
                                          'Kassa əməliyyatlarının mədaxilindən daxil olub'
                                       ELSE
                                             NOTE
                                          || ' -'
                                          || 'Kassa əməliyyatlarının mədaxilindən daxil olub'
                                    END)
                                      NOTE,
                                   ID,
                                   BANK_OPERATION_ID
                              FROM CRS_USER.CASH_BANK_ACCOUNT
                             WHERE     INC_EXP = 1
                                   AND BANK_ID = {bank_id}
                                   AND ADATE = TO_DATE ('{AccountDate.Text}', 'DD/MM/YYYY')";

            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    double income = Convert.ToDouble(dr["INCOME"].ToString());
                    if (TransactionName == "INSERT")
                    {
                        if (String.IsNullOrEmpty(dr["BANK_OPERATION_ID"].ToString()))
                        {
                            int OperationID = GlobalFunctions.GetOracleSequenceValue("BANK_OPERATION_SEQUENCE");
                            GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.BANK_OPERATIONS(ID,
                                                                                                    BANK_ID,
                                                                                                    OPERATION_DATE,
                                                                                                    APPOINTMENT_ID,
                                                                                                    INCOME,
                                                                                                    EXPENSES,
                                                                                                    DEBT,
                                                                                                    NOTE)
                                                            VALUES({OperationID},
                                                                    {dr["BANK_ID"]},
                                                                    TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'),
                                                                    7,
                                                                    0,
                                                                    {income.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                    0,
                                                                    '{dr["NOTE"]}')",
                                                            $@"UPDATE CRS_USER.CASH_BANK_ACCOUNT SET BANK_OPERATION_ID = {OperationID} 
                                                                        WHERE ID = {dr["ID"]}",
                                                            "Kassanın möhkəmləndirilməsi üçün olan məbləğ bank əməliyyatlarına daxil olmadı.");
                        }
                    }
                    else
                        GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.BANK_OPERATIONS SET BANK_ID = {dr["BANK_ID"]}, 
                                                                                OPERATION_DATE = TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'), 
                                                                                INCOME = 0, 
                                                                                EXPENSES = {income.ToString(GlobalVariables.V_CultureInfoEN)}, 
                                                                                NOTE = '{dr["NOTE"]}' 
                                                                         WHERE ID = {dr["BANK_OPERATION_ID"]}",
                                                            "Kassanın möhkəmləndirilməsi üçün olan məbləğ bank əməliyyatlarında dəyişdirilmədi.");
                }
                GlobalProcedures.UpdateBankOperationDebtWithBank(AccountDate.Text, bank_id);
                GlobalProcedures.UpdateBankOperationDebt(AccountDate.Text);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Kassanın möhkəmləndirilməsi üçün kassa əməliyyatlarının rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void OtherNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (OtherNoteText.Text.Length <= 400)
                OtherDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - OtherNoteText.Text.Length).ToString();
        }

        private void InsertCashOtherPayment()
        {
            int OtherID = Class.GlobalFunctions.GetOracleSequenceValue("CASH_OTHER_PAYMENT_SEQUENCE");
            Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_OTHER_PAYMENT(ID,CUSTOMER_NAME,PAYMENT_DATE,CASH_APPOINTMENT_ID,AMOUNT,NOTE)VALUES(" + OtherID + ",'" + OtherCustomerNameText.Text.Trim() + "',TO_DATE('" + OtherDate.Text + "','DD/MM/YYYY')," + other_appointment_id + "," + OtherAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + OtherNoteText.Text.Trim() + "')",
                                                "Digər ödənişlərin mədaxili kassaya daxil olunmadı.");
            Class.GlobalProcedures.InsertCashOperation(5, OtherID, OtherDate.Text, null, (double)OtherAmountValue.Value, 0, 1);
        }

        private void UpdateCashOtherPayment()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_OTHER_PAYMENT SET CUSTOMER_NAME = '" + OtherCustomerNameText.Text.Trim() + "',PAYMENT_DATE = TO_DATE('" + OtherDate.Text + "','DD/MM/YYYY'),CASH_APPOINTMENT_ID = " + other_appointment_id + ",AMOUNT = " + OtherAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + OtherNoteText.Text.Trim() + "' WHERE ID = " + OperationOwnerID,
                                                "Digər ödənişlərin mədaxili kassada dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 5, int.Parse(OperationOwnerID), OtherDate.Text, (double)OtherAmountValue.Value, 0);
        }


        private void AccountNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (AccountNoteText.Text.Length <= 400)
                AccountDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - AccountNoteText.Text.Length).ToString();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 11:
                    GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                    break;
                case 15:
                    {
                        GlobalProcedures.FillComboBoxEditWithSqlText(FounderComboBox, "SELECT FULLNAME,FULLNAME,FULLNAME FROM CRS_USER.FOUNDERS ORDER BY ORDER_ID");
                        FindFounderCardID();
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

        private void BankComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            switch (IncomeBackstageViewControl.SelectedTabIndex)
            {
                case 0:
                    GlobalProcedures.ExchangeCalculator(PaymentDate.Text);
                    break;
                case 4:
                    GlobalProcedures.ExchangeCalculator(FounderDate.Text);
                    break;
                case 6:
                    GlobalProcedures.ExchangeCalculator(AccountDate.Text);
                    break;
                case 8:
                    GlobalProcedures.ExchangeCalculator(OtherDate.Text);
                    break;
                case 10:
                    GlobalProcedures.ExchangeCalculator(ServiceDate.Text);
                    break;
            }
            if (currency_id != 1)
            {
                CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                cur = GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                CurrencyRateLabel.Text = "1 " + currency + " = ";
                CurrencyRateValue.Value = (decimal)cur;
            }
            PaymentValue_EditValueChanged(sender, EventArgs.Empty);
            PaymentAZNValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void ServiceNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (ServiceNoteText.Text.Length <= 400)
                ServiceDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - ServiceNoteText.Text.Length).ToString();
        }

        private void InsertCashServicePrice()
        {
            int ServiceID = GlobalFunctions.GetOracleSequenceValue("CASH_SERVICE_PRICE_SEQUENCE");
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_SERVICE_PRICE(ID,CUSTOMER_NAME,PAYMENT_DATE,APPOINTMENT,AMOUNT,NOTE)VALUES(" + ServiceID + ",'" + ServiceCustomerNameText.Text.Trim() + "',TO_DATE('" + ServiceDate.Text + "','DD/MM/YYYY'),'" + ServiceAppointmentText.Text.Trim() + "'," + ServiceAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + ServiceNoteText.Text.Trim() + "')",
                                                "Xidmət haqqı kassaya daxil olunmadı.");
            GlobalProcedures.InsertCashOperation(6, ServiceID, ServiceDate.Text, null, (double)ServiceAmountValue.Value, 0, 1);
        }

        private void UpdateCashServicePrice()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_SERVICE_PRICE SET CUSTOMER_NAME = '" + ServiceCustomerNameText.Text.Trim() + "',PAYMENT_DATE = TO_DATE('" + ServiceDate.Text + "','DD/MM/YYYY'),APPOINTMENT = '" + ServiceAppointmentText.Text.Trim() + "',AMOUNT = " + ServiceAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + ServiceNoteText.Text.Trim() + "' WHERE ID = " + OperationOwnerID,
                                                "Xidmət haqqı kassada dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 6, int.Parse(OperationOwnerID), ServiceDate.Text, (double)ServiceAmountValue.Value, 0);
        }

        private void FindFounderCardID()
        {
            founder_id = GlobalFunctions.FindComboBoxSelectedValue("FOUNDERS", "FULLNAME", "ID", FounderComboBox.Text);
            if (FormStatus)
                FindFounderCard(founder_id);
        }

        private void FounderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindFounderCardID();
            LoadFundContractsGridView(founder_id);
            if (fund_contract_id > 0)
            {
                if (founder_id != old_founder_id)
                {
                    Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", -1, "WHERE ID = " + old_founder_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", -1, "WHERE ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_PAYMENTS", -1, "WHERE CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", Class.GlobalVariables.V_UserID, "WHERE ID = " + founder_id + " AND USED_USER_ID = -1");
                    Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", Class.GlobalVariables.V_UserID, "WHERE ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + founder_id + ") AND USED_USER_ID = -1");
                    Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_PAYMENTS", Class.GlobalVariables.V_UserID, "WHERE CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + founder_id + ") AND USED_USER_ID = -1");
                }
                old_founder_id = founder_id;
                old_fund_contract_id = fund_contract_id;
            }
            else
            {
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", -1, "WHERE ID = " + old_founder_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", -1, "WHERE ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_PAYMENTS", -1, "WHERE CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            }
        }

        private void LoadFundContractsGridView(int founderid)
        {
            string s = null;
            if (TransactionName == "INSERT")
                s = "SELECT FC.ID,FC.CONTRACT_NUMBER,TO_CHAR(FC.START_DATE,'DD.MM.YYYY'),FC.INTEREST||' %',FC.PERIOD||' ay' FROM CRS_USER.FUNDS_CONTRACTS FC,CRS_USER.FOUNDER_CONTRACTS FCON WHERE FC.ID = FCON.FUNDS_CONTRACT_ID AND FCON.FOUNDER_ID = " + founderid + " AND FCON.FOUNDER_CARD_ID = " + founder_card_id + " ORDER BY FC.START_DATE,FC.ID";
            else
                s = "SELECT FC.ID,FC.CONTRACT_NUMBER,TO_CHAR(FC.START_DATE,'DD.MM.YYYY'),FC.INTEREST||' %',FC.PERIOD||' ay' FROM CRS_USER.FUNDS_CONTRACTS FC,CRS_USER.FOUNDER_CONTRACTS FCON WHERE FC.ID = FCON.FUNDS_CONTRACT_ID AND FCON.FOUNDER_ID = " + founderid + " AND FCON.FOUNDER_CARD_ID = " + founder_card_id + " AND FCON.FUNDS_CONTRACT_ID = " + fund_contract_id + " ORDER BY FC.START_DATE,FC.ID";
            try
            {
                ContractGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFundContractsGridView");
                ContractGridView.PopulateColumns();
                ContractGridView.Columns[0].Visible = false;
                ContractGridView.Columns[1].Caption = "Nömrəsi";
                ContractGridView.Columns[2].Caption = "Başlama tarixi";
                ContractGridView.Columns[3].Caption = "İllik faiz";
                ContractGridView.Columns[4].Caption = "Müddəti";

                ContractGridView.Columns[2].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractGridView.Columns[2].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractGridView.Columns[3].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractGridView.Columns[3].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                ContractGridView.Columns[4].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                ContractGridView.Columns[4].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                //HeaderAligment
                for (int i = 0; i < ContractGridView.Columns.Count; i++)
                {
                    ContractGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    ContractGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                }

                ContractGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçinin müqavilələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FindFounderCard(int founderid)
        {
            string s = "SELECT CS.SERIES||': '||FC.CARD_NUMBER||', '||TO_CHAR(FC.ISSUE_DATE,'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib',FC.ID FROM CRS_USER.FOUNDER_CARDS FC,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE FC.CARD_SERIES_ID = CS.ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.ID = (SELECT MAX(ID) FROM CRS_USER.FOUNDER_CARDS WHERE FOUNDER_ID = FC.FOUNDER_ID) AND FC.FOUNDER_ID = " + founderid;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FounderCardText.Text = dr[0].ToString();
                        founder_card_id = Convert.ToInt32(dr[1].ToString());
                    }
                }
                else
                {
                    FounderCardText.Text = null;
                    founder_card_id = 0;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçinin şəxsiyyətini təsdiq edən sənəd tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FounderComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 15);
        }

        private void OtherAppointmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    other_appointment_id = GlobalFunctions.FindComboBoxSelectedValue("CASH_APPOINTMENTS", "NAME", "ID", OtherAppointmentComboBox.Text);
                    break;
                case "EN":
                    other_appointment_id = GlobalFunctions.FindComboBoxSelectedValue("CASH_APPOINTMENTS", "NAME_EN", "ID", OtherAppointmentComboBox.Text);
                    break;
                case "RU":
                    other_appointment_id = GlobalFunctions.FindComboBoxSelectedValue("CASH_APPOINTMENTS", "NAME_RU", "ID", OtherAppointmentComboBox.Text);
                    break;
            }
        }

        void RefreshAppointmentDictionaries(int index)
        {
            switch (index)
            {
                case 0:
                    GlobalProcedures.FillComboBoxEditWithSqlText(OtherAppointmentComboBox, "SELECT NAME,NAME_EN,NAME_RU FROM CRS_USER.CASH_APPOINTMENTS ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadAppointmentDictionaries(string transaction, int index, int hostage_index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.HostageSelectedTabIndex = hostage_index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshAppointmentDictionaries);
            fc.ShowDialog();
        }

        private void OtherAppointmentComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadAppointmentDictionaries("E", 9, 0);
        }

        private void ContractGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractGridView.GetFocusedDataRow();
            if (row != null)
            {
                fund_contract_id = Convert.ToInt32(row["ID"].ToString());
                int pay_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id);
                string lastdate = DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ);
                if (pay_count == 0)
                    lastdate = GlobalFunctions.GetMaxDate("SELECT MAX(START_DATE) FROM FUNDS_CONTRACTS WHERE ID = " + fund_contract_id).ToString("d", GlobalVariables.V_CultureInfoAZ);
                else
                    lastdate = GlobalFunctions.GetMaxDate("SELECT NVL(MAX(CP.PAYMENT_DATE),TO_DATE('" + DateTime.Today.ToString("d") + "','DD/MM/YYYY')) FROM CRS_USER.FUNDS_PAYMENTS CP WHERE CP.CONTRACT_ID = " + fund_contract_id).ToString("d", Class.GlobalVariables.V_CultureInfoAZ);

                FounderDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy");

            }
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFundContractsGridView(founder_id);
        }

        void RefreshConracts()
        {
            LoadFundContractsGridView(founder_id);
        }

        private void ContractsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AttractedFunds.FFundContract ffc = new AttractedFunds.FFundContract();
            ffc.RefreshFundsDataGridView += new AttractedFunds.FFundContract.DoEvent(RefreshConracts);
            ffc.ShowDialog();
        }

        private void ContractGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractGridView, PopupMenu, e);
        }

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