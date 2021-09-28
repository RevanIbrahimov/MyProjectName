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
using Microsoft.Reporting.WebForms;
using System.Collections;
using Bytescout.Document;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using CRS.Class;
using Oracle.ManagedDataAccess.Client;
using CRS.Class.Views;
using System.Reflection;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Text.RegularExpressions;

namespace CRS.Forms.Contracts
{
    public partial class FContractAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FContractAddEdit()
        {
            InitializeComponent();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName, ContractID, CustomerID, CustomerCode, SellerID;
        public int Commit = 0, IsSpecialAttention = 0;
        public bool IsExtend;

        string PhoneID,
            PhoneNumber,
            SellerImagePath,
            ContractImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages",
            IDImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\IDCardImages",
            ImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images",
            SellerImage,
            credit_type_code = null,
            currency_name,
            currency_small_name,
            credit_currency_name,
            credit_currency_code,
            contract_date,
            currency_short_name,
            card_series,
            card_number,
            seller_card_name,
            CommitmentID,
            CommitmentName,
            PowerID,
            InsuranceID,
            DocumentID,
            InterestID,
            liquid_account = null,
            first_payment_account = null,
            liquidbanktext = "",
            firstpaymentbanktext = "",
            insurance_startdate,
            insurance_enddate,
            seller_type = null,
            old_seller_id,
            person_description = null,
            new_individual_seller_id = null,
            new_juridical_seller_id = null,
            power_code,
            oldContractID = null,
            carOwnerName;

        int credit_type_id = 0,
            currency_id = 0,
            credit_currency_id = 0,
            credit_currency_value = 0,
            old_month_count = 0,
            old_interest = 0,
            old_currency_id = 0,
            credit_name_id = 0,
            brand_id = 0,
            type_id = 0,
            ban_type_id = 0,
            color_id = 0,
            model_id = 0,
            contract_images_count = 0,
            card_issuing_id = 0,
            power_card_issuing_id,
            card_series_id = 0,
            power_card_series_id = 0,
            check_end_date = 0,
            old_grace_period = 0,
            customer_card_id = 0,
            period = 0,
            ContractUsedUserID = -1,
            statusID,
            default_interest = 0,
            check_interest_value = -1,
            crop_image_count = 0,
            pay_count = 0,
            power_count = 0,
            old_payment_type = -1,
            penalty_count = 0,
            liquid_bank_id = 0,
            liquid_plan_id = 0,
            first_payment_bank_id = 0,
            first_payment_plan_id = 0,
            liquidtype_selectedindex = 0,
            first_payment_selectedindex = 0,
            contract_rate_currency_id = 0,
            insurance_period = 0,
            insurance_company_id = 0,
            customer_type_id = 0,
            seller_type_id = 0,
            old_seller_type_id = 0,
            old_seller_index = 0,
            CommitmentPersonTypeID = 1,
            sheduleVersion,
            topindex,
            old_row_num,
            code_number;

        int? parent_contract_id = null;

        double old_contract_amount,
            monthly_amount = 0,
            old_monthly_amount = 0,
            credit_currency_rate = 0,
            old_credit_currency_rate = 0,
            rate = 0,
            old_first_payment = 0,
            old_liquid_amount = 0,
            car_amount = 0,
            insurance_amount = 0,
            unconditional_amount = 0,
            insurance_interest,
            debt;

        bool FormStatus = false,
            contract_click = false,
            leasing_object_contract_click = false,
            document_click = false,
            ContractUsed = false,
            CurrentStatus = false,
            ContractClosed = false,
            changecode = false,
            sellerdetails = false,
            real_estate = false,
            is_contract_rate_change = false;

        DateTime old_start_date;
        List<string> image_list = new List<string>();

        ReportViewer rv_payment = new ReportViewer();
        ReportViewer rv_insurance = new ReportViewer();
        ReportViewer rv_document = new ReportViewer();
        List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();

        public delegate void DoEvent(string contract_id);
        public event DoEvent RefreshContractsDataGridView;

        private void FContractAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                SellerSeriesLookUp.Properties.Buttons[1].Visible = GlobalVariables.CardSeries;

                CreditNameLookUp.Properties.Buttons[1].Visible = GlobalVariables.CreditType;
                BrandComboBox.Properties.Buttons[1].Visible =
                    ModelComboBox.Properties.Buttons[1].Visible =
                    TypeComboBox.Properties.Buttons[1].Visible =
                    BanTypeComboBox.Properties.Buttons[1].Visible = GlobalVariables.Hostage;

                SellerIssuingLookUp.Properties.Buttons[1].Visible = GlobalVariables.CardIssuing;

                NewCommitmentBarSubItem.Enabled =
                    BJournal.Enabled = GlobalVariables.AddCommitment;

                NewPowerOfAttorneyBarButton.Enabled = GlobalVariables.AddPower;
                NewInsuranceBarButton.Enabled = GlobalVariables.AddInsurance;
                NewInterestPenaltiesBarButton.Enabled = GlobalVariables.AddInterestPenalties;

                LiquidTypeRadioGroup.Properties.Items[0].Enabled =
                    FirstPaymentTypeRadioGroup.Properties.Items[0].Enabled = GlobalVariables.ContractCashPayment;
                LiquidTypeRadioGroup.Properties.Items[1].Enabled =
                    FirstPaymentTypeRadioGroup.Properties.Items[1].Enabled =
                    LiquidBankComboBox.Enabled =
                    FirstPaymentBankComboBox.Enabled = GlobalVariables.ContractBankPayment;

                BExtendTime.Enabled = (GlobalVariables.AddExtend || GlobalVariables.EditExtend || GlobalVariables.DeleteExtend);
            }
            else
                GlobalVariables.CommitContract = true;

            GlobalProcedures.DeleteAllFilesInDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents");
            RefreshDictionaries(7);
            RefreshDictionaries(5);

            EditCustomerLabel.Visible = false;
            if (TransactionName == "INSERT")
            {
                if (!String.IsNullOrEmpty(CustomerCode))
                    RegistrationCodeText.Text = CustomerCode;

                ContractStartDate.EditValue = DateTime.Today;
                SellerTypeRadioGroup.SelectedIndex = 0;
                FormStatus = true;
                ContractID = GlobalFunctions.GetOracleSequenceValue("CONTRACT_SEQUENCE").ToString();
                LiquidTypeRadioGroup.SelectedIndex = FirstPaymentTypeRadioGroup.SelectedIndex = 1;
                BExtendTime.Visible = false;
                seller_type_id = 1;
                sellerdetails = true;
            }
            else if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");
                
                LoadContractDetails();
                ExtendTimeCaption(IsExtend);
                ContractUsed = (ContractUsedUserID >= 0);

                if ((ContractClosed && ContractUsed) || (ContractClosed && !ContractUsed))
                {
                    XtraMessageBox.Show("Seçdiyiniz müqavilə bağlanılıb. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else if ((ContractUsed) && (!ContractClosed))
                {
                    if (GlobalVariables.V_UserID != ContractUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == ContractUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçdiyiniz müqavilə hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müqavilənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;

                LoadContractImages();
                LoadContractDescriptions();
                if (credit_name_id == 1)
                    LoadHostageCarDetails();
                else if (credit_name_id == 5)
                    LoadHostageObjectDetails();

                InsertPaymentSchedulesTemp();
                LoadCustomerDetails();
                LoadContractPaidOut();
                CommitStatus();
                ComponentEnable(CurrentStatus);
                FormStatus = true;
                ControlJournal();
                LiquidTypeRadioGroup.Visible = FirstPaymentTypeRadioGroup.Visible = (parent_contract_id == null);
                oldContractID = ContractID;
            }
            else if (TransactionName == "AGREEMENT")
            {
                LoadContractDetails();
                LoadContractImages();
                LoadContractDescriptions();
                if (credit_name_id == 1)
                    LoadHostageCarDetails();
                else if (credit_name_id == 5)
                    LoadHostageObjectDetails();
                LoadCustomerDetails();
                CommitStatus();
                AgreementComponent();
                FormStatus = true;

                oldContractID = ContractID;

                List<Payments> lstPayments = PaymentsDAL.SelectPayments(0, int.Parse(oldContractID)).ToList<Payments>();

                var pay = lstPayments.LastOrDefault();
                if (pay != null)
                {
                    if (pay.BANK_CASH == "D")
                    {
                        CreditCurrencyLookUp.EditValue = credit_currency_code == "USD" ? CreditCurrencyLookUp.Properties.GetKeyValueByDisplayText("AZN") : CreditCurrencyLookUp.Properties.GetKeyValueByDisplayText("USD");
                        LiquidValue.Value = lstPayments.LastOrDefault().PAYMENT_AMOUNT_AZN;
                        ContractCodeText.Text = ContractCodeText.Text + "/" + CalcAgreementNumber(int.Parse(oldContractID));
                        ContractID = GlobalFunctions.GetOracleSequenceValue("CONTRACT_SEQUENCE").ToString();
                        CreateAccounts();
                    }
                    else
                    {
                        GlobalProcedures.ShowWarningMessage("Razılaşma üçün köhnə müqavilənin xalis qalıq borcu digər ödəniş ilə bağlanmalıdır.");
                        BOK.Visible = false;
                    }
                }
                else
                {
                    GlobalProcedures.ShowWarningMessage("Razılaşma üçün köhnə müqavilənin xalis qalıq borcu digər ödəniş ilə bağlanmalıdır.");
                    BOK.Visible = false;
                }
            }

            old_contract_amount = (double)CreditAmountValue.Value;
        }

        private void ExtendTimeCaption(bool c)
        {
            BExtendTime.Text = (c) ? "Müddət uzadılıb" : "Müddəti uzat";
        }

        private int? CalcAgreementNumber(int contractID)
        {
            List<Contract> lstContract = ContractDAL.SelectContractWithParentID(contractID).ToList<Contract>();
            if (lstContract.Count == 0)
                return 1;
            else
                return lstContract.Count + 1;
        }

        private void CommitStatus()
        {
            if (Commit == 0 && !GlobalVariables.CommitContract)
                BOK.Visible = true;
            else if (Commit == 1 && !GlobalVariables.CommitContract)
            {
                BContract.Enabled =
                    BLeasingObjectContract.Enabled =
                    BDocument.Enabled =
                    BPaymentSchedule.Enabled =
                    BReceipt.Enabled = !CurrentStatus;

                if (CurrentStatus)
                    BCommon.Enabled = false;
                else
                    BCommon.Enabled = GlobalVariables.AddCommon;

                PowerOfAttorneyTab.PageVisible = (credit_name_id == 1);
                FilesTab.PageVisible = CommitmentTab.PageVisible = true;

                BOK.Visible = (!CurrentStatus && (GlobalVariables.AddCommitment ||
                                                    GlobalVariables.EditCommitment ||
                                                    GlobalVariables.DeleteCommitment ||
                                                    GlobalVariables.AddPower ||
                                                    GlobalVariables.EditPower ||
                                                    GlobalVariables.DeletePower ||
                                                    GlobalVariables.AddInterestPenalties ||
                                                    GlobalVariables.EditInterestPenalties ||
                                                    GlobalVariables.DeleteInterestPenalties));
            }
            else
            {
                BContract.Enabled =
                    BLeasingObjectContract.Enabled =
                    BDocument.Enabled =
                    BPaymentSchedule.Enabled =
                    BReceipt.Enabled =
                    BOK.Visible = !CurrentStatus;

                if (CurrentStatus)
                    BCommon.Enabled = false;
                else
                    BCommon.Enabled = GlobalVariables.AddCommon;

                if (Commit == 1)
                    BConfirm.Visible = false;
                else if (TransactionName == "EDIT")
                    BConfirm.Visible = !CurrentStatus;
                DeleteFileBarButton.Enabled = !CurrentStatus;

                PowerOfAttorneyTab.PageVisible = (credit_name_id == 1 && Commit == 1);
                FilesTab.PageVisible = CommitmentTab.PageVisible = (Commit == 1);
            }
        }

        public void ComponentEnable(bool status)
        {
            CreditNameLookUp.Enabled = false;
            if (!status)
                status = (Commit == 1 && !GlobalVariables.CommitContract);//tesdiq etmek huququ yoxdursa muqavilenin predmetlerini deyise bilmesin

            if (status == false)
            {
                PhonePopupMenu.Manager = PhoneBarManager;
                if (GlobalVariables.V_UserID > 0)
                {
                    EditCustomerLabel.Visible = GlobalVariables.EditCustomer;
                    if (TransactionName == "INSERT")
                        NewCustomerDropDownButton.Visible = GlobalVariables.AddCustomer;
                    else
                        NewCustomerDropDownButton.Visible = false;
                }
                else
                {
                    EditCustomerLabel.Visible = true;
                    NewCustomerDropDownButton.Visible = (TransactionName == "INSERT");
                }

                GracePeriodValue.Enabled =
                    PaymentTypeRadioGroup.Enabled =
                    LiquidTypeRadioGroup.Enabled =
                    FirstPaymentTypeRadioGroup.Enabled =
                    LiquidBankComboBox.Enabled =
                    FirstPaymentBankComboBox.Enabled =
                    ContractStartDate.Enabled =
                    EndDateCheckEdit.Enabled =
                    CreditAmountValue.Enabled =
                    LiquidCurrencyLookUp.Enabled =
                    CreditCurrencyLookUp.Enabled =
                    LiquidValue.Enabled =
                    FirstPaymentValue.Enabled =
                    InterestCheckEdit.Enabled =
                    SellerTypeRadioGroup.Enabled = (pay_count == 0);
            }
            else
            {
                if (credit_name_id == 5)
                    HostageScrollableControl.Enabled = false;
                else
                {
                    BrandComboBox.Enabled =
                    TypeComboBox.Enabled =
                    BanTypeComboBox.Enabled =
                    BanText.Enabled =
                    EngineNumberText.Enabled =
                    EngineValue.Enabled =
                    MilageValue.Enabled =
                    ChassisText.Enabled =
                    ModelComboBox.Enabled =
                    YearValue.Enabled =
                    ColorComboBox.Enabled =
                    LiquidCurrencyLookUp.Enabled =
                    LiquidBankComboBox.Enabled =
                    LiquidValue.Enabled =
                    FirstPaymentValue.Enabled =
                    FirstPaymentBankComboBox.Enabled = false;

                    CarNumberText.Enabled = (power_count == 0);
                }

                PhonePopupMenu.Manager = null;
                EditCustomerLabel.Visible = true;
                NewCustomerDropDownButton.Visible =
                    GracePeriodValue.Enabled =
                    PaymentTypeRadioGroup.Enabled =
                    CreditNameLookUp.Enabled =
                    ContractStartDate.Enabled =
                    EndDateCheckEdit.Enabled =
                    CreditAmountValue.Enabled =
                    CreditCurrencyLookUp.Enabled =
                    InterestCheckEdit.Enabled =
                    SellerTypeRadioGroup.Enabled = false;
            }
        }

        public void AgreementComponent()
        {
            CreditNameLookUp.Enabled = LiquidTypeRadioGroup.Visible = FirstPaymentTypeRadioGroup.Visible = false;

            ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths(Convert.ToInt32(PeriodText.Text));
            ContractEndDate.Properties.MinValue = ContractStartDate.DateTime;

            PhonePopupMenu.Manager = PhoneBarManager;

            GracePeriodValue.Enabled =
                PaymentTypeRadioGroup.Enabled =
                LiquidTypeRadioGroup.Enabled =
                FirstPaymentTypeRadioGroup.Enabled =
                LiquidBankComboBox.Enabled =
                FirstPaymentBankComboBox.Enabled =
                ContractStartDate.Enabled =
                EndDateCheckEdit.Enabled =
                CreditAmountValue.Enabled =
                LiquidCurrencyLookUp.Enabled =
                CreditCurrencyLookUp.Enabled =
                LiquidValue.Enabled =
                FirstPaymentValue.Enabled =
                InterestCheckEdit.Enabled =
                SellerTypeRadioGroup.Enabled =
                BOK.Visible = true;

            PowerOfAttorneyTab.PageVisible = CommitmentTab.PageVisible = false;
        }

        private void CreateAccounts()
        {
            if (String.IsNullOrWhiteSpace(ContractCodeText.Text) ||
                String.IsNullOrWhiteSpace(RegistrationCodeText.Text) ||
                String.IsNullOrWhiteSpace(currency_short_name))
            {
                CustomerAccountText.Text = LeasingAccountText.Text = LeasingInterestAccountText.Text = SellerAccountText.Text = null;
                return;
            }

            CustomerAccountText.Text = "33" + ContractCodeText.Text.Trim() + currency_short_name + RegistrationCodeText.Text.Trim();
            LeasingAccountText.Text = "11" + ContractCodeText.Text.Trim() + currency_short_name + RegistrationCodeText.Text.Trim();
            LeasingInterestAccountText.Text = "22" + ContractCodeText.Text.Trim() + currency_short_name + RegistrationCodeText.Text.Trim();
            SellerAccountText.Text = "44" + ContractCodeText.Text.Trim() + currency_short_name + SellerID;
        }

        private void LoadContractDetails()
        {
            string s = null;
            try
            {
                s = $@"SELECT CT.CODE || C.CODE CONTRACT_CODE,
                               CN.NAME CREDIT_NAME,
                               C.START_DATE,
                               C.END_DATE,
                               CT.TERM PERIOD,
                               CT.INTEREST,
                               C.GRACE_PERIOD,
                               C.AMOUNT,
                               CUR.CODE,
                               C.CUSTOMER_ACCOUNT,
                               C.LEASING_ACCOUNT,
                               C.LEASING_INTEREST_ACCOUNT,
                               C.CHECK_END_DATE,
                               C.PAYMENT_TYPE,
                               C.MONTHLY_AMOUNT,
                               C.CHECK_INTEREST,
                               CT.ID CREDIT_TYPE_ID,
                               CT.NAME_ID CREDIT_NAME_ID,
                               C.SELLER_ACCOUNT,
                               C.CHECK_PERIOD,
                               C.CURRENCY_RATE,
                               DECODE(C.SELLER_TYPE_ID,1,0,2,1) SELLER_INDEX,
                               C.CUSTOMER_TYPE_ID,
                               CUS.CODE CUSTOMER_CODE,
                               C.SELLER_ID,
                               C.SELLER_TYPE_ID,
                               C.USED_USER_ID,
                               C.STATUS_ID,
                               C.PARENT_ID,
                               (SELECT COUNT (*)
                                      FROM (SELECT CONTRACT_ID FROM CRS_USER.CUSTOMER_PAYMENTS
                                            UNION ALL
                                            SELECT CONTRACT_ID FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP)
                                     WHERE CONTRACT_ID = C.ID)
                                      PAY_COUNT,
                               (SELECT COUNT(*) FROM CRS_USER.POWER_OF_ATTORNEY WHERE CONTRACT_ID = C.ID) POWER_COUNT
                          FROM CRS_USER.CONTRACTS C,
                               CRS_USER.CREDIT_TYPE CT,
                               CRS_USER.CREDIT_NAMES CN,
                               CRS_USER.CURRENCY CUR,
                               CRS_USER.V_CUSTOMERS CUS
                         WHERE     C.CREDIT_TYPE_ID = CT.ID
                               AND CT.NAME_ID = CN.ID
                               AND C.CURRENCY_ID = CUR.ID
                               AND C.CUSTOMER_ID = CUS.ID
                               AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                               AND C.ID = {ContractID}";

                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractDetails").Rows)
                {
                    ContractUsedUserID = Convert.ToInt32(dr["USED_USER_ID"]);
                    statusID = Convert.ToInt32(dr["STATUS_ID"]);
                    ContractClosed = (GlobalVariables.V_UserID > 0) ? (statusID == 6) : false;
                    ContractCodeText.Text = dr["CONTRACT_CODE"].ToString();
                    ContractStartDate.EditValue = (TransactionName == "EDIT") ? DateTime.Parse(dr["START_DATE"].ToString()) : DateTime.Today;
                    ContractEndDate.EditValue = (TransactionName == "EDIT") ? DateTime.Parse(dr["END_DATE"].ToString()) : DateTime.Today;
                    CreditNameLookUp.EditValue = CreditNameLookUp.Properties.GetKeyValueByDisplayText(dr["CREDIT_NAME"].ToString());
                    period = Convert.ToInt32(dr["PERIOD"].ToString());
                    default_interest = Convert.ToInt32(dr["INTEREST"].ToString());
                    CreditAmountValue.Value = (TransactionName == "EDIT") ? Convert.ToDecimal(dr["AMOUNT"].ToString()) : 0;
                    debt = Convert.ToDouble(dr["AMOUNT"].ToString());
                    credit_currency_code = dr["CODE"].ToString();
                    CreditCurrencyLookUp.EditValue = (TransactionName == "EDIT") ? CreditCurrencyLookUp.Properties.GetKeyValueByDisplayText(credit_currency_code) : null;

                    if (Convert.ToInt32(dr[12].ToString()) == 1)
                    {
                        EndDateCheckEdit.Checked = true;
                        PeriodText.Text = dr[19].ToString();
                    }
                    else
                    {
                        PeriodText.Text = dr[4].ToString();
                        EndDateCheckEdit.Checked = false;
                    }
                    PaymentTypeRadioGroup.SelectedIndex = (TransactionName == "EDIT") ? Convert.ToInt32(dr[13].ToString()) : 0;
                    old_payment_type = Convert.ToInt32(dr[13].ToString());
                    monthly_amount = (TransactionName == "EDIT") ? Convert.ToDouble(dr[14].ToString()) : 0;
                    old_monthly_amount = monthly_amount;
                    MonthlyPaymentValue.Value = (decimal)monthly_amount;
                    pay_count = TransactionName == "AGREEMENT" ? 0 : Convert.ToInt32(dr["PAY_COUNT"]);
                    power_count = TransactionName == "AGREEMENT" ? 0 : Convert.ToInt32(dr["POWER_COUNT"]);
                    if (Convert.ToInt32(dr[15].ToString()) > -1)
                    {
                        InterestCheckEdit.Checked = true;
                        InterestText.Text = dr[15].ToString();
                        check_interest_value = Convert.ToInt32(dr[15].ToString());
                    }
                    else
                    {
                        InterestText.Text = dr[5].ToString();
                        InterestCheckEdit.Checked = false;
                    }
                    GracePeriodValue.Value = Convert.ToInt32(dr["GRACE_PERIOD"].ToString());
                    credit_type_id = Convert.ToInt32(dr[16].ToString());
                    credit_name_id = Convert.ToInt32(dr[17].ToString());
                    SellerAccountText.Text = dr[18].ToString();
                    old_credit_currency_rate = Convert.ToDouble(dr["CURRENCY_RATE"].ToString());
                    rate = (TransactionName == "EDIT") ? old_credit_currency_rate : 1;
                    old_seller_index = Convert.ToInt32(dr["SELLER_INDEX"]);
                    SellerTypeRadioGroup.SelectedIndex = old_seller_index;
                    RegistrationCodeText.Text = dr["CUSTOMER_CODE"].ToString();
                    old_seller_id = dr["SELLER_ID"].ToString();
                    old_seller_type_id = Convert.ToInt32(dr["SELLER_TYPE_ID"]);
                    CreditCurrencyRateLabel.Visible = (credit_currency_id == 2);
                    credit_currency_rate = rate;
                    CreditCurrencyRateLabel.Text = credit_currency_value + " " + credit_currency_name + " = " + credit_currency_rate.ToString("N4") + " " + currency_name;
                    if (!String.IsNullOrWhiteSpace(dr["PARENT_ID"].ToString()))
                        parent_contract_id = Convert.ToInt32(dr["PARENT_ID"].ToString());
                }

                if (!EndDateCheckEdit.Checked)
                    old_month_count = Convert.ToInt32(PeriodText.Text);
                else
                    old_month_count = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);

                old_interest = Convert.ToInt32(InterestText.Text);

                old_start_date = GlobalFunctions.ChangeStringToDate(ContractStartDate.Text, "ddmmyyyy");
                old_grace_period = (int)GracePeriodValue.Value;
                if (old_seller_type_id == 1)
                    new_individual_seller_id = old_seller_id;
                else
                    new_juridical_seller_id = old_seller_id;
                RegistrationCodeText.Enabled = false;
                VisibleHostageDetails(credit_name_id);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müqavilənin məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadCustomerDetails()
        {
            if (RegistrationCodeText.Text.Length != 4)
            {
                CustomerFullNameText.Text =
                    card_series =
                    card_number =
                    CardDescriptionText.Text =
                    CustomerID =
                    RegistrationAddressText.Text =
                    IssuingDateText.Text =
                    ReliableDateText.Text =
                    IssuingText.Text =
                    CustomerTypeLabel.Text = null;

                CustomerPictureBox.Image = null;

                OtherInfoTabControl.Enabled =
                    BOK.Visible =
                    BContract.Enabled =
                    EditCustomerLabel.Visible =
                    BLeasingObjectContract.Enabled =
                    BCommon.Enabled =
                    BReceipt.Enabled =
                    BPaymentSchedule.Enabled =
                    BDocument.Enabled = false;

                return;
            }

            string s = null;
            DataTable dt = null;
            if (RegistrationCodeText.Text.Length >= 4)
            {
                s = $@"SELECT C.CODE CUSTOMER_CODE,
                               C.CUSTOMER_NAME,
                               CC.CARD_DESCRIPTION,
                               DECODE(C.PERSON_TYPE_ID,1,CC.REGISTRATION_ADDRESS,2,CC.ADDRESS) ADDRESS,
                               TO_CHAR (CC.ISSUE_DATE, 'DD.MM.YYYY') ISSUE_DATE,
                               TO_CHAR (CC.RELIABLE_DATE, 'DD.MM.YYYY') RELIABLE_DATE,
                               CC.CARD_ISSUING_NAME,
                               C.ID CUSTOMER_ID,
                               CC.CARD_ID,
                               CIM.IMAGE,
                               C.PERSON_TYPE_ID,
                               CC.CARD_SERIES,
                               CC.CARD_NUMBER,
                               TP.NAME PERSON_TYPE_NAME,
                               DECODE(C.PERSON_TYPE_ID,1,'C',2,'JP') PERSON_DESCRIPTION,
                               C.VOEN,
                               C.ACCOUNT_NUMBER
                          FROM CRS_USER.V_CUSTOMERS C,
                               CRS_USER.V_CUSTOMER_CARDS_DETAILS CC,
                               CRS_USER.CUSTOMER_IMAGE CIM,
                               CRS_USER.PERSON_TYPE TP
                         WHERE     C.ID = CC.CUSTOMER_ID
                               AND C.PERSON_TYPE_ID = CC.PERSON_TYPE_ID
                               AND C.PERSON_TYPE_ID = TP.ID
                               AND C.ID = CIM.CUSTOMER_ID(+)
                               AND C.CODE ='{RegistrationCodeText.Text}'";
                dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCustomerDetails");
            }

            if (dt.Rows.Count > 0)
            {
                OtherInfoTabControl.Enabled = true;
                if (TransactionName == "INSERT")
                {
                    BOK.Visible = true;
                    if (GlobalVariables.V_UserID > 0)
                    {
                        EditCustomerLabel.Visible = GlobalVariables.EditCustomer;
                        NewCustomerDropDownButton.Visible = GlobalVariables.AddCustomer;
                    }
                    else
                        EditCustomerLabel.Visible = NewCustomerDropDownButton.Visible = true;

                    BContract.Enabled = BLeasingObjectContract.Enabled = BDocument.Enabled = BPaymentSchedule.Enabled = BCommon.Enabled = BReceipt.Enabled = GlobalVariables.CommitContract;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    RegistrationCodeText.Text = dr["CUSTOMER_CODE"].ToString();
                    CustomerFullNameText.Text = dr["CUSTOMER_NAME"].ToString();
                    CardDescriptionText.Text = dr["CARD_DESCRIPTION"].ToString();
                    RegistrationAddressText.Text = dr["ADDRESS"].ToString();
                    IssuingDateText.Text = dr["ISSUE_DATE"].ToString();
                    ReliableDateText.Text = dr["RELIABLE_DATE"].ToString();
                    IssuingText.Text = dr["CARD_ISSUING_NAME"].ToString();
                    card_series = dr["CARD_SERIES"].ToString();
                    card_number = dr["CARD_NUMBER"].ToString();
                    CustomerID = dr["CUSTOMER_ID"].ToString();
                    customer_type_id = Convert.ToInt32(dr["PERSON_TYPE_ID"].ToString());
                    customer_card_id = Convert.ToInt32(dr["CARD_ID"].ToString());
                    CustomerPictureBox.Visible = (customer_type_id == 1);
                    CustomerTypeLabel.Text = dr["PERSON_TYPE_NAME"].ToString();
                    person_description = dr["PERSON_DESCRIPTION"].ToString();
                    VoenText.Text = dr["VOEN"].ToString();
                    AccountText.Text = dr["ACCOUNT_NUMBER"].ToString();
                    if (!DBNull.Value.Equals(dr["IMAGE"]))
                    {

                        Byte[] BLOBData = (byte[])dr["IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        CustomerPictureBox.Image = Image.FromStream(stmBLOBData);
                        stmBLOBData.Close();
                    }
                    else
                    {
                        switch (GlobalVariables.SelectedLanguage)
                        {
                            case "RU":
                                CustomerPictureBox.Properties.NullText = "Фотография клиентов";
                                break;
                            case "EN":
                                CustomerPictureBox.Properties.NullText = "Customer picture";
                                break;
                        }
                    }
                }

                IssuingDateText.Visible =
                    DateOfIssueLabel.Visible =
                    ReliableDateText.Visible =
                    ReliableLabel.Visible =
                    IssuingAuthorityLabel.Visible =
                    IssuingText.Visible =
                    VoenLabel.Visible =
                    VoenText.Visible = (customer_type_id == 1);

                CreateAccounts();
            }
            else
            {
                RegistrationCodeText.Text =
                    CustomerFullNameText.Text =
                    card_series =
                    card_number =
                    CardDescriptionText.Text =
                    CustomerID =
                    RegistrationAddressText.Text =
                    IssuingDateText.Text =
                    ReliableDateText.Text =
                    IssuingText.Text =
                    CustomerTypeLabel.Text = null;

                CustomerPictureBox.Image = null;

                OtherInfoTabControl.Enabled =
                    BOK.Visible =
                    BContract.Enabled =
                    EditCustomerLabel.Visible =
                    BLeasingObjectContract.Enabled =
                    BCommon.Enabled =
                    BReceipt.Enabled =
                    BPaymentSchedule.Enabled =
                    BDocument.Enabled = false;
            }
        }

        private void LoadContractImages()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.IMAGE FROM CRS_USER.CONTRACT_IMAGES T WHERE T.CONTRACT_ID = {ContractID}", this.Name + "/LoadContractImages");

            if (dt == null)
            {
                BDeleteContractPicture.Enabled = false;
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    if (!DBNull.Value.Equals(dr["IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        ContractImageSlider.Images.Add(Image.FromStream(stmBLOBData));

                        if (!Directory.Exists(ContractImagePath))
                        {
                            Directory.CreateDirectory(ContractImagePath);
                        }

                        string imagePath = ContractImagePath + "\\" + ContractImageSlider.Images.Count + "_" + ContractCodeText.Text.Replace($@"/", "") + ".jpeg";
                        GlobalProcedures.DeleteFile(imagePath);
                        FileStream fs = new FileStream(imagePath, FileMode.Create, FileAccess.Write);
                        image_list.Add(imagePath);
                        stmBLOBData.WriteTo(fs);
                        fs.Close();
                        stmBLOBData.Close();
                        BDeleteContractPicture.Enabled = !CurrentStatus;
                    }
                }
                catch
                {

                }
            }
        }

        private void LoadContractDescriptions()
        {
            string s = $@"SELECT T.NOTE FROM CRS_USER.CONTRACT_DESCRIPTIONS T WHERE T.CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractDescriptions");
            if (dt.Rows.Count > 0)
                ContractNoteText.Text = dt.Rows[0]["NOTE"].ToString();
        }

        private void LoadHostageCarDetails()
        {
            string s = null;
            try
            {
                s = $@"SELECT CB.NAME BRAND_NAME,
                               CT.NAME,
                               CT.NAME_EN,
                               CT.NAME_RU,
                               CC.NAME,
                               CC.NAME_EN,
                               CC.NAME_RU,
                               HC.YEAR,
                               HC.BAN,
                               HC.ENGINE,
                               HC.CAR_NUMBER,
                               HC.LIQUID_AMOUNT,
                               HC.FIRST_PAYMENT,
                               C.CODE,
                               HC.ENGINE_NUMBER,
                               HC.MILAGE,
                               M.NAME,
                               HC.CHASSIS_NUMBER,
                               CBT.NAME,
                               CBT.NAME_EN,
                               CBT.NAME_RU,
                               HC.IS_INSURANCE
                          FROM CRS_USER.HOSTAGE_CAR HC,
                               CRS_USER.CAR_BRANDS CB,
                               CRS_USER.CAR_TYPES CT,
                               CRS_USER.CAR_COLORS CC,
                               CRS_USER.CURRENCY C,
                               CRS_USER.CAR_MODELS M,
                               CRS_USER.CAR_BAN_TYPES CBT
                         WHERE     HC.BAN_TYPE_ID = CBT.ID
                               AND HC.CURRENCY_ID = C.ID
                               AND HC.COLOR_ID = CC.ID
                               AND HC.BRAND_ID = CB.ID
                               AND HC.TYPE_ID = CT.ID
                               AND HC.MODEL_ID = M.ID
                               AND HC.CONTRACT_ID = {ContractID}";

                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadHostageCarDetails");
                foreach (DataRow dr in dt.Rows)
                {
                    BrandComboBox.EditValue = dr[0].ToString();
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            {
                                TypeComboBox.EditValue = dr[1].ToString();
                                ColorComboBox.EditValue = dr[4].ToString();
                                BanTypeComboBox.EditValue = dr[18].ToString();
                            }
                            break;
                        case "EN":
                            {
                                TypeComboBox.EditValue = dr[2].ToString();
                                ColorComboBox.EditValue = dr[5].ToString();
                                BanTypeComboBox.EditValue = dr[19].ToString();
                            }
                            break;
                        case "RU":
                            {
                                TypeComboBox.EditValue = dr[3].ToString();
                                ColorComboBox.EditValue = dr[6].ToString();
                                BanTypeComboBox.EditValue = dr[20].ToString();
                            }
                            break;
                    }
                    YearValue.Value = Convert.ToInt32(dr[7].ToString());
                    BanText.Text = dr[8].ToString();
                    EngineValue.Value = Convert.ToInt32(dr[9].ToString());
                    CarNumberText.Text = dr[10].ToString();
                    LiquidValue.Value = (TransactionName == "EDIT") ? Convert.ToDecimal(dr[11].ToString()) : 0;
                    old_liquid_amount = (double)LiquidValue.Value;
                    FirstPaymentValue.Value = (TransactionName == "EDIT") ? Convert.ToDecimal(dr[12].ToString()) : 0;
                    old_first_payment = (double)FirstPaymentValue.Value;
                    LiquidCurrencyLookUp.EditValue = LiquidCurrencyLookUp.Properties.GetKeyValueByDisplayText(dr["CODE"].ToString());
                    EngineNumberText.Text = dr[14].ToString();
                    MilageValue.Value = Convert.ToInt32(dr[15].ToString());
                    ModelComboBox.EditValue = dr[16].ToString();
                    ChassisText.Text = dr[17].ToString();
                    if (LiquidValue.Value != 0)
                        FirstPaymentPercentLabel.Text = Math.Round((FirstPaymentValue.Value / LiquidValue.Value * 100), 2).ToString() + " % - i ödənilib";
                    else
                        FirstPaymentPercentLabel.Visible = false;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Avtomobilin məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadHostageObjectDetails()
        {
            string s = null;
            try
            {
                s = $@"SELECT HC.ADDRESS,
                               HC.EXCERPT,
                               HC.AREA,
                               HC.LIQUID_AMOUNT,
                               HC.FIRST_PAYMENT,
                               C.CODE
                          FROM CRS_USER.HOSTAGE_OBJECT HC, CRS_USER.CURRENCY C
                         WHERE HC.CURRENCY_ID = C.ID AND HC.CONTRACT_ID = {ContractID}";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadHostageObjectDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    ObjectAddressText.Text = dr[0].ToString();
                    ObjectExcerptText.Text = dr[1].ToString();
                    ObjectAreaValue.Value = Convert.ToDecimal(dr[2].ToString());
                    LiquidValue.Value = (TransactionName == "EDIT") ? Convert.ToDecimal(dr[3].ToString()) : 0;
                    old_liquid_amount = (double)LiquidValue.Value;
                    FirstPaymentValue.Value = (TransactionName == "EDIT") ? Convert.ToDecimal(dr[4].ToString()) : 0;
                    old_first_payment = (double)FirstPaymentValue.Value;
                    LiquidCurrencyLookUp.EditValue = LiquidCurrencyLookUp.Properties.GetKeyValueByDisplayText(dr["CODE"].ToString());
                    if (LiquidValue.Value != 0)
                        FirstPaymentPercentLabel.Text = Math.Round((FirstPaymentValue.Value / LiquidValue.Value * 100), 2).ToString() + " % - i ödənilib";
                    else
                        FirstPaymentPercentLabel.Visible = false;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Obyektin məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadSellerDetails()
        {
            if (TransactionName == "INSERT")
                return;

            FindSeller(int.Parse(SellerID));
        }

        private void FindSeller(int sellerID)
        {
            string s = null;
            if (SellerTypeRadioGroup.SelectedIndex == 0)
            {
                s = $@"SELECT C.SURNAME,
                               C.NAME,
                               C.PATRONYMIC,
                               CS.SERIES,
                               C.CARD_NUMBER,
                               C.CARD_PINCODE,
                               C.CARD_ISSUE_DATE,
                               CI.NAME ISSUE_NAME,
                               C.ADDRESS,
                               C.REGISTRATION_ADDRESS,
                               C.IMAGE,
                               C.CARD_FRONT_FACE_IMAGE,
                               C.CARD_FRONT_FACE_IMAGE_FORMAT,
                               C.CARD_REAR_FACE_IMAGE,
                               C.CARD_REAR_FACE_IMAGE_FORMAT,
                               C.CARD_SERIES_ID,
                               C.CARD_ISSUING_ID,
                               C.WITH_POWER,
                               C.POWER_NUMBER,
                               C.POWER_NAME,
                               (SELECT SERIES
                                  FROM CRS_USER.CARD_SERIES
                                 WHERE ID = C.POWER_CARD_SERIES_ID)
                                  POWER_SERIES,
                               C.POWER_CARD_NUMBER,
                               C.POWER_CARD_PINCODE,
                               C.POWER_CARD_ISSUE_DATE,
                               (SELECT NAME
                                  FROM CRS_USER.CARD_ISSUING
                                 WHERE ID = C.POWER_CARD_ISSUING_ID)
                                  POWER_ISSUE_NAME
                          FROM CRS_USER.SELLERS C, CRS_USER.CARD_SERIES CS, CRS_USER.CARD_ISSUING CI
                         WHERE C.CARD_ISSUING_ID = CI.ID AND C.CARD_SERIES_ID = CS.ID AND C.ID = {sellerID}";
                try
                {
                    DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/FindSeller");

                    foreach (DataRow dr in dt.Rows)
                    {
                        SellerSurnameText.Text = dr["SURNAME"].ToString();
                        SellerNameText.Text = dr["NAME"].ToString();
                        SellerPatronymicText.Text = dr["PATRONYMIC"].ToString();

                        SellerSeriesLookUp.EditValue = SellerSeriesLookUp.Properties.GetKeyValueByDisplayText(dr["SERIES"].ToString());
                        SellerNumberText.Text = dr["CARD_NUMBER"].ToString();
                        SellerPinCodeText.Text = dr["CARD_PINCODE"].ToString();
                        SellerIssueDate.EditValue = DateTime.Parse(dr["CARD_ISSUE_DATE"].ToString());
                        PowerOfAttorneyCheck.Checked = Convert.ToInt32(dr["WITH_POWER"]) == 1 ? true : false;
                        PowerOfAttorneyNumberText.EditValue = dr["POWER_NUMBER"];
                        SellerPowerOfAttorneyNameText.EditValue = dr["POWER_NAME"];
                        SellerPowerSeriesLookUp.EditValue = SellerPowerSeriesLookUp.Properties.GetKeyValueByDisplayText(dr["POWER_SERIES"].ToString());
                        SellerPowerNumberText.Text = dr["POWER_CARD_NUMBER"].ToString();
                        SellerPowerPinCodeText.Text = dr["POWER_CARD_PINCODE"].ToString();
                        if (!String.IsNullOrEmpty(dr["POWER_CARD_ISSUE_DATE"].ToString()))
                            SellerPowerIssueDate.EditValue = DateTime.Parse(dr["POWER_CARD_ISSUE_DATE"].ToString());
                        SellerIssuingLookUp.EditValue = SellerIssuingLookUp.Properties.GetKeyValueByDisplayText(dr["ISSUE_NAME"].ToString());
                        SellerPowerIssuingLookUp.EditValue = SellerPowerIssuingLookUp.Properties.GetKeyValueByDisplayText(dr["POWER_ISSUE_NAME"].ToString());
                        SellerAddressText.Text = dr["ADDRESS"].ToString();
                        SellerRegistrationAddressText.Text = dr["REGISTRATION_ADDRESS"].ToString();

                        if (!DBNull.Value.Equals(dr["IMAGE"]))
                        {
                            Byte[] BLOBData = (byte[])dr["IMAGE"];
                            MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                            SellerPictureBox.Image = Image.FromStream(stmBLOBData);

                            SellerImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images";
                            if (!Directory.Exists(SellerImagePath))
                            {
                                Directory.CreateDirectory(SellerImagePath);
                            }
                            GlobalProcedures.DeleteFile(SellerImagePath + "\\S_" + SellerSurnameText.Text + ".jpeg");
                            FileStream fs = new FileStream(SellerImagePath + "\\S_" + SellerSurnameText.Text + ".jpeg", FileMode.Create, FileAccess.Write);
                            stmBLOBData.WriteTo(fs);
                            fs.Close();
                            stmBLOBData.Close();
                            SellerImage = SellerImagePath + "\\S_" + SellerSurnameText.Text + ".jpeg";
                            BLoadSellerPicture.Text = "Dəyiş";
                            BDeleteSellerPicture.Enabled = true;
                        }
                        else
                        {
                            BLoadSellerPicture.Text = "Yüklə";
                            BDeleteSellerPicture.Enabled = false;
                            switch (GlobalVariables.SelectedLanguage)
                            {
                                case "RU":
                                    SellerPictureBox.Properties.NullText = "Фотография продавца";
                                    break;
                                case "EN":
                                    SellerPictureBox.Properties.NullText = "Seller picture";
                                    break;
                            }
                        }

                        if (!DBNull.Value.Equals(dr["CARD_FRONT_FACE_IMAGE"]))
                        {
                            Byte[] BLOBData = (byte[])dr["CARD_FRONT_FACE_IMAGE"];
                            MemoryStream stmBLOBData = new MemoryStream(BLOBData);

                            if (!Directory.Exists(IDImagePath))
                            {
                                Directory.CreateDirectory(IDImagePath);
                            }
                            GlobalProcedures.DeleteFile(IDImagePath + "\\S_Front_" + SellerSurnameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"]);
                            FileStream front_fs = new FileStream(IDImagePath + "\\S_Front_" + SellerSurnameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                            stmBLOBData.WriteTo(front_fs);
                            front_fs.Close();
                            stmBLOBData.Close();
                            SellerFrontFaceButtonEdit.Text = IDImagePath + "\\S_Front_" + SellerSurnameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"];
                        }

                        if (!DBNull.Value.Equals(dr["CARD_REAR_FACE_IMAGE"]))
                        {
                            Byte[] BLOBData = (byte[])dr["CARD_REAR_FACE_IMAGE"];
                            MemoryStream stmBLOBData = new MemoryStream(BLOBData);

                            if (!Directory.Exists(IDImagePath))
                            {
                                Directory.CreateDirectory(IDImagePath);
                            }
                            GlobalProcedures.DeleteFile(IDImagePath + "\\S_Rear_" + SellerSurnameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"]);
                            FileStream rear_fs = new FileStream(IDImagePath + "\\S_Rear_" + SellerSurnameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                            stmBLOBData.WriteTo(rear_fs);
                            rear_fs.Close();
                            stmBLOBData.Close();
                            SellerRearFaceButtonEdit.Text = IDImagePath + "\\S_Rear_" + SellerSurnameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"];
                        }
                        sellerdetails = true;
                        seller_type_id = 1;
                    }
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Fiziki şəxsin parametrləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                }
            }
            else
            {
                s = $@"SELECT NAME,VOEN,LEADING_NAME,ADDRESS FROM CRS_USER.JURIDICAL_PERSONS WHERE ID = {sellerID}";
                try
                {
                    DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/FindSeller");

                    foreach (DataRow dr in dt.Rows)
                    {
                        JuridicalPersonNameText.Text = dr["NAME"].ToString();
                        JuridicalPersonVoenText.Text = dr["VOEN"].ToString();
                        LeadingPersonNameText.Text = dr["LEADING_NAME"].ToString();
                        JuridicalPersonAddressText.Text = dr["ADDRESS"].ToString();
                    }
                    sellerdetails = true;
                    seller_type_id = 2;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Hüquqi şəxsin parametrləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void LoadContractPaidOut()
        {
            string s = $@"SELECT C.LIQUID_BANK_ID,
                                   (SELECT B.LONG_NAME || ' / Daxili:' || AP.SUB_ACCOUNT
                                      FROM CRS_USER.BANKS B, CRS_USER.ACCOUNTING_PLAN AP
                                     WHERE     B.ID = AP.BANK_ID
                                           AND B.ID = C.LIQUID_BANK_ID
                                           AND AP.ID = C.LIQUID_PLAN_ID)
                                      LIQUID_BANK_NAME,
                                   C.FIRST_PAYMENT_BANK_ID,
                                   (SELECT B.LONG_NAME || ' / Daxili:' || AP.SUB_ACCOUNT
                                      FROM CRS_USER.BANKS B, CRS_USER.ACCOUNTING_PLAN AP
                                     WHERE     B.ID = AP.BANK_ID
                                           AND B.ID = C.FIRST_PAYMENT_BANK_ID
                                           AND AP.ID = C.FIRST_PAYMENT_PLAN_ID)
                                      FIRST_PAYMENT_BANK_NAME
                              FROM CRS_USER.CONTRACT_PAID_OUT C
                             WHERE CONTRACT_ID = {ContractID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractPaidOut");

                foreach (DataRow dr in dt.Rows)
                {
                    liquid_bank_id = int.Parse(dr["LIQUID_BANK_ID"].ToString());
                    if (liquid_bank_id != 0)
                    {
                        LiquidTypeRadioGroup.SelectedIndex = liquidtype_selectedindex = 1;
                        liquidbanktext = dr["LIQUID_BANK_NAME"].ToString();

                        LiquidBankComboBox.EditValue = liquidbanktext;
                    }
                    else
                        LiquidTypeRadioGroup.SelectedIndex = liquidtype_selectedindex = 0;

                    first_payment_bank_id = int.Parse(dr["FIRST_PAYMENT_BANK_ID"].ToString());
                    if (first_payment_bank_id != 0)
                    {
                        FirstPaymentTypeRadioGroup.SelectedIndex = first_payment_selectedindex = 1;
                        firstpaymentbanktext = dr["FIRST_PAYMENT_BANK_NAME"].ToString();
                        FirstPaymentBankComboBox.EditValue = firstpaymentbanktext;
                    }
                    else
                        FirstPaymentTypeRadioGroup.SelectedIndex = first_payment_selectedindex = 0;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Satıcıya ödənilən pulun mənbəyi tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegistrationCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
                LoadCustomerDetails();
        }

        private void DeleteAllTemp()
        {
            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER_TEMP.PROC_CONTRACT_DELETE_ALL_TEMP", "P_USED_USER_ID", GlobalVariables.V_UserID, "Müqavilənin temp məlumatları cədvəldən silinmədi.");
        }

        private void FContractAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_CONTRACT_UNLOCK", "P_CONTRACT_ID", int.Parse(ContractID), "P_CREDIT_TYPE_ID", credit_type_id, "Müqavilə blokdan çıxarılmadı.");
            GlobalProcedures.DeleteAllFilesInDirectory(IDImagePath);
            GlobalProcedures.DeleteAllFilesInDirectory(ImagePath);
            GlobalProcedures.DeleteAllFilesInDirectory(ContractImagePath);
            DeletePaymentSchedulesTemp();
            DeleteAllTemp();
            rv_payment.Reset();
            rv_insurance.Reset();
            rv_document.Reset();
            this.RefreshContractsDataGridView(ContractID);
        }

        private void DeletePaymentSchedulesTemp()
        {
            if (!String.IsNullOrEmpty(CustomerID))
                GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP WHERE CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Ödəniş qrafiki temp cədvəldən silinmədi.",
                                               this.Name + "/DeletePaymentSchedulesTemp");
        }

        private void CreditTypeParametr()
        {
            string s = null;
            if (FormStatus)
            {
                try
                {
                    if (CreditNameLookUp.EditValue != null)
                    {
                        s = $@"SELECT CT.ID,CT.CODE,CT.TERM,CT.INTEREST FROM CRS_USER.CREDIT_TYPE CT WHERE CT.NAME_ID = {credit_name_id} AND CT.CALC_DATE = (SELECT MAX(CALC_DATE) FROM CRS_USER.CREDIT_TYPE WHERE NAME_ID = CT.NAME_ID AND CALC_DATE <= TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'))";

                        DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/CreditTypeParametr");

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", -1, "WHERE USED_USER_ID = " + GlobalVariables.V_UserID);
                                credit_type_id = Convert.ToInt32(dr[0].ToString());
                                //credit_type_code = dr[1].ToString();
                                PeriodText.Text = dr[2].ToString();

                                if (!InterestCheckEdit.Checked)
                                {
                                    InterestText.Text = dr[3].ToString();
                                    default_interest = Convert.ToInt32(dr[3].ToString());
                                }

                                if (!EndDateCheckEdit.Checked)
                                {
                                    ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths(Convert.ToInt32(dr[2].ToString()));
                                    ContractEndDate.Properties.MinValue = ContractStartDate.DateTime;
                                }

                                CreditAmountValue.Enabled =
                                    LiquidValue.Enabled =
                                    FirstPaymentValue.Enabled =
                                    LiquidCurrencyLookUp.Enabled =
                                    CreditCurrencyLookUp.Enabled = (pay_count == 0);

                                SellerTabPage.PageEnabled =
                                    InsuranceTab.PageEnabled =
                                    PaymentScheduleTabPage.PageEnabled =
                                    InterestPenaltiesTab.PageEnabled =
                                    DescriptionTab.PageEnabled =
                                    BOK.Visible = !CurrentStatus;
                                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", GlobalVariables.V_UserID, "WHERE ID = " + credit_type_id + " AND USED_USER_ID = -1");
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show(CreditNameLookUp.Text + " lizinq növünün " + ContractStartDate.Text + " tarixinə heç bir rekviziti təyin edilməyib.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            credit_type_id = 0;
                            credit_type_code =
                                PeriodText.Text =
                                InterestText.Text =
                                ContractEndDate.Text = null;

                            CreditAmountValue.Enabled =
                                LiquidValue.Enabled =
                                FirstPaymentValue.Enabled =
                                LiquidCurrencyLookUp.Enabled =
                                CreditCurrencyLookUp.Enabled =
                                SellerTabPage.PageEnabled =
                                InsuranceTab.PageEnabled =
                                PaymentScheduleTabPage.PageEnabled =
                                InterestPenaltiesTab.PageEnabled =
                                DescriptionTab.PageEnabled =
                                BOK.Visible = false;

                            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", -1, "WHERE USED_USER_ID = " + GlobalVariables.V_UserID);
                        }
                    }
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Kredit növünün parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void EndDateCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (EndDateCheckEdit.Checked)
            {
                check_end_date = 1;
                ContractEndDate.Enabled = EndDateCheckEdit.Enabled;
                DifferenceDateLabel.Text = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime).ToString() + " ay";
            }
            else
            {
                PeriodText.Text = period.ToString();
                check_end_date = 0;
                ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths(Convert.ToInt32(PeriodText.Text));
                ContractEndDate.Enabled = false;
            }

            DifferenceDateLabel.Visible = EndDateCheckEdit.Checked;
        }

        void RefreshHostageDictionaries(int index)
        {
            switch (index)
            {
                case 0:
                    GlobalProcedures.FillComboBoxEdit(BrandComboBox, "CAR_BRANDS", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 1:
                    GlobalProcedures.FillComboBoxEdit(TypeComboBox, "CAR_TYPES", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 2:
                    GlobalProcedures.FillComboBoxEdit(ColorComboBox, "CAR_COLORS", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 3:
                    GlobalProcedures.FillComboBoxEditWithSqlText(ModelComboBox, "SELECT NAME,NAME,NAME FROM CRS_USER.CAR_MODELS WHERE BRAND_ID = " + brand_id + " ORDER BY ORDER_ID");
                    break;
                case 4:
                    GlobalProcedures.FillComboBoxEdit(BanTypeComboBox, "CAR_BAN_TYPES", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadHostageDictionaries(string transaction, int index, int hostage_index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.HostageSelectedTabIndex = hostage_index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshHostageDictionaries);
            fc.ShowDialog();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 2:
                    GlobalProcedures.FillLookUpEdit(SellerSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                    GlobalProcedures.FillLookUpEdit(SellerPowerSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 3:
                    GlobalProcedures.FillLookUpEdit(SellerIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    GlobalProcedures.FillLookUpEdit(SellerPowerIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 5:
                    GlobalProcedures.FillLookUpEdit(CreditNameLookUp, "CREDIT_NAMES", "ID", "NAME", "ID IN (1,5) ORDER BY ID");
                    break;
                case 7:
                    {
                        GlobalProcedures.FillLookUpEdit(LiquidCurrencyLookUp, "CURRENCY", "ID", "CODE", "1 = 1 ORDER BY ORDER_ID");
                        GlobalProcedures.FillLookUpEdit(CreditCurrencyLookUp, "CURRENCY", "ID", "CODE", "1 = 1 ORDER BY ORDER_ID");
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

        private void CalcFirstPaymentPercent()
        {
            if (LiquidValue.Value != 0)
            {
                FirstPaymentPercentLabel.Visible = true;
                FirstPaymentPercentLabel.Text = Math.Round((FirstPaymentValue.Value / LiquidValue.Value * 100), 2).ToString() + " % - i ödənilib";
            }
            else
                FirstPaymentPercentLabel.Visible = false;
        }

        private void InitialPaymentValue_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FirstPaymentValue.Value < 0)
                    FirstPaymentValue.BackColor = Color.Red;
                else
                    FirstPaymentValue.BackColor = GlobalFunctions.ElementColor();

                if (FormStatus)
                {
                    if ((FirstPaymentValue.Value < 0) && (LiquidValue.Value > 0) && (CreditAmountValue.Value > 0))
                    {
                        if ((credit_currency_id != currency_id) && (credit_currency_rate != 0))
                            FirstPaymentValue.Value = LiquidValue.Value - (decimal)Math.Round((double)CreditAmountValue.Value * credit_currency_rate, 2);
                        else
                            FirstPaymentValue.Value = LiquidValue.Value - CreditAmountValue.Value;
                    }

                    FormStatus = false;
                    if ((credit_currency_id != currency_id) && (credit_currency_rate != 0))
                        CreditAmountValue.Value = (decimal)Math.Round((double)(LiquidValue.Value - FirstPaymentValue.Value) / credit_currency_rate, 2);
                    else
                        CreditAmountValue.Value = LiquidValue.Value - FirstPaymentValue.Value;

                    CalcMonthlyAmount();
                    FormStatus = true;
                    if (CreditAmountValue.Value == 0)
                        return;
                    CalcFirstPaymentPercent();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Məbləğ hesablanan zaman xəta baş verdi.", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void InsertPaymentSchedulesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_SCHEDULES_TEMP", "P_CONTRACT_ID", ContractID, "Ödəniş qrafiki temp cədvələ daxil edilmədi.");
        }

        private void CalculatedPaymentScheduleTemp(bool isAgain)
        {
            if ((FormStatus) && (CreditAmountValue.Value > 0))
            {
                int month_count, grace_period = 0, interest = Convert.ToInt32(InterestText.Text), is_change_date = 0;
                month_count = Convert.ToInt32(PeriodText.Text);

                DateTime d_start, realDate;
                DayOfWeek day_of_week;
                double interest_amount = 0, basic_amount = 0, debt = (double)CreditAmountValue.Value, monthly_amount = (double)MonthlyPaymentValue.Value;

                grace_period = (int)GracePeriodValue.Value;

                if (isAgain ||
                    old_contract_amount != debt ||
                    old_interest != interest ||
                    old_month_count != month_count ||
                    old_monthly_amount != monthly_amount ||
                    old_start_date != ContractStartDate.DateTime ||
                    old_grace_period != grace_period ||
                    old_payment_type != PaymentTypeRadioGroup.SelectedIndex)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP 
                                                            WHERE VERSION = 0 
                                                                AND CONTRACT_ID = {ContractID} 
                                                                AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                        "Ödəniş qrafiki cədvəldən silinmədi.",
                                                 this.Name + "/CalculatedPaymentScheduleTemp");

                    for (int j = 1; j <= grace_period; j++)
                    {
                        d_start = ContractStartDate.DateTime;
                        d_start = d_start.AddMonths(j);
                        day_of_week = d_start.DayOfWeek;//heftenin gunu     
                        realDate = d_start;
                        if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 6) //tarix hefdenin sonuna duserse onda bu tarixi 5-ci gun gotursun
                        {
                            d_start = d_start.AddDays(-1);
                            is_change_date = 1;
                        }
                        else if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 7)
                        {
                            d_start = d_start.AddDays(-2);
                            is_change_date = 1;
                        }
                        interest_amount = ((debt * interest) / 100) / 360 * 30;
                        monthly_amount = interest_amount;
                        basic_amount = 0;
                        //debt = debt;
                        GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                            MONTH_DATE,
                                                                                                            REAL_DATE,
                                                                                                            MONTHLY_PAYMENT,
                                                                                                            BASIC_AMOUNT,
                                                                                                            INTEREST_AMOUNT,
                                                                                                            DEBT,
                                                                                                            CURRENCY_ID,
                                                                                                            USED_USER_ID,
                                                                                                            IS_CHANGE,
                                                                                                            IS_CHANGE_DATE,
                                                                                                            ORDER_ID)
                                                                VALUES(                                                                            
                                                                            {ContractID},
                                                                            TO_DATE('{d_start.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                            TO_DATE('{realDate.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                            {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            {basic_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            {interest_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            {debt.ToString(GlobalVariables.V_CultureInfoEN)},{credit_currency_id},{GlobalVariables.V_UserID},1,{is_change_date},{j}
                                                                      )",
                            "Ödəniş qrafiki cədvələ daxil edilmədi.", this.Name + "/CalculatedPaymentScheduleTemp");
                        is_change_date = 0;
                    }

                    if (PaymentTypeRadioGroup.SelectedIndex == 0)
                        monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, month_count - grace_period, interest);
                    else
                        monthly_amount = (double)MonthlyPaymentValue.Value;

                    for (int i = grace_period + 1; i <= month_count; i++)
                    {
                        d_start = ContractStartDate.DateTime;
                        d_start = d_start.AddMonths(i);


                        if (d_start > ContractEndDate.DateTime)
                            d_start = ContractEndDate.DateTime;
                        day_of_week = d_start.DayOfWeek;//heftenin gunu
                        is_change_date = 0;
                        realDate = d_start;
                        if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 6)
                        {
                            d_start = d_start.AddDays(-1);
                            is_change_date = 1;
                        }
                        else if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 7)
                        {
                            d_start = d_start.AddDays(-2);
                            is_change_date = 1;
                        }
                        interest_amount = ((debt * interest) / 100) / 360 * 30;
                        if (i < month_count)
                            basic_amount = monthly_amount - interest_amount;
                        else
                        {
                            basic_amount = debt;
                            monthly_amount = debt + interest_amount;
                        }
                        debt = debt - basic_amount;
                        GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                            MONTH_DATE,
                                                                                                            REAL_DATE,
                                                                                                            MONTHLY_PAYMENT,
                                                                                                            BASIC_AMOUNT,
                                                                                                            INTEREST_AMOUNT,
                                                                                                            DEBT,
                                                                                                            CURRENCY_ID,
                                                                                                            USED_USER_ID,
                                                                                                            IS_CHANGE,
                                                                                                            IS_CHANGE_DATE,
                                                                                                            ORDER_ID)
                                                          VALUES({ContractID},
                                                                        TO_DATE('{d_start.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                        TO_DATE('{realDate.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                        {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {basic_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {interest_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        {credit_currency_id},
                                                                        {GlobalVariables.V_UserID},1,
                                                                        {is_change_date},
                                                                        {i})",
                            "Ödəniş qrafiki cədvələ daxil edilmədi.", this.Name + "/CalculatedPaymentScheduleTemp");
                        is_change_date = 0;
                    }
                }
                old_month_count = month_count;
                old_interest = interest;
                old_contract_amount = (double)CreditAmountValue.Value;
                old_start_date = GlobalFunctions.ChangeStringToDate(ContractStartDate.Text, "ddmmyyyy");
                old_grace_period = grace_period;
                old_payment_type = PaymentTypeRadioGroup.SelectedIndex;
            }
        }

        private void CalcExtendPaymentShedules()
        {
            ContractExtend extend = new ContractExtend();
            List<ContractExtend> lstContractExtend = ContractExtendDAL.SelectContractExtend(1, int.Parse(ContractID)).ToList<ContractExtend>();
            if (lstContractExtend.Count > 0)
                extend = lstContractExtend.LastOrDefault();
            else
                return;

            DateTime d_start, realDate;
            DayOfWeek day_of_week;
            int is_change_date = 0, interest = extend.INTEREST, month_count = extend.MONTH_COUNT;
            double interest_amount = 0, basic_amount = 0, debt = (double)extend.DEBT, monthly_amount = (double)extend.MONTHLY_AMOUNT;

            GlobalProcedures.ExecuteQuery($@"DELETE CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP
                                                            WHERE VERSION = 1 
                                                                AND CONTRACT_ID = {ContractID} 
                                                                AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                        "Ödəniş qrafiki cədvəldən silinmədi.",
                                                 this.Name + "/CalcPaymentShedules");

            for (int i = 1; i <= month_count; i++)
            {
                d_start = extend.START_DATE;
                d_start = d_start.AddMonths(i);


                if (d_start > extend.END_DATE)
                    d_start = extend.END_DATE;
                day_of_week = d_start.DayOfWeek;//heftenin gunu
                is_change_date = 0;
                realDate = d_start;
                if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 6)
                {
                    d_start = d_start.AddDays(-1);
                    is_change_date = 1;
                }
                else if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 7)
                {
                    d_start = d_start.AddDays(-2);
                    is_change_date = 1;
                }
                interest_amount = ((debt * interest) / 100) / 360 * 30;
                if (i < month_count)
                    basic_amount = monthly_amount - interest_amount;
                else
                {
                    basic_amount = debt;
                    monthly_amount = debt + interest_amount;
                }
                debt = debt - basic_amount;

                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                  MONTH_DATE,
                                                                                                  REAL_DATE,
                                                                                                  MONTHLY_PAYMENT,
                                                                                                  BASIC_AMOUNT,
                                                                                                  INTEREST_AMOUNT,
                                                                                                  DEBT,
                                                                                                  CURRENCY_ID,
                                                                                                  USED_USER_ID,
                                                                                                  IS_CHANGE,
                                                                                                  IS_CHANGE_DATE,
                                                                                                  ORDER_ID,
                                                                                                  VERSION)
                                                          VALUES({ContractID},
                                                                  TO_DATE('{d_start.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                  TO_DATE('{realDate.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                  {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {basic_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {interest_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {credit_currency_id},
                                                                  {GlobalVariables.V_UserID},1,
                                                                  {is_change_date},
                                                                  {i},1)",
                    "Ödəniş qrafiki cədvələ daxil edilmədi.", this.Name + "/CalcExtendPaymentShedules");
                is_change_date = 0;
            }

        }

        private void UpdateCurrencyPaymentScheduleTemp()
        {
            if (credit_currency_id == 0)
                return;

            if ((old_currency_id != credit_currency_id) && (FormStatus))
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP SET 
                                                                IS_CHANGE = 1, 
                                                                CURRENCY_ID = {credit_currency_id} 
                                                         WHERE CONTRACT_ID = {ContractID} 
                                                                AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                        "Ödəniş qrafikinin valyutası temp cədvəldə dəyişdirilmədi.", this.Name + "/UpdateCurrencyPaymentScheduleTemp");
            old_currency_id = credit_currency_id;
        }

        private void OtherInfoTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (OtherInfoTabControl.SelectedTabPageIndex == 2)
            {
                CalculatedPaymentScheduleTemp(false);
                LoadPaymentScheduleDataGridView();
                //PaymentSchedulesGridView.ViewCaption = "Lizinq Məbləği: " + CreditAmountValue.Value.ToString("N2") + " " + CreditCurrencyLookUp.Text;
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 1)
            {
                SellerScrollableControl.Enabled = BLoadSellerPicture.Enabled = !CurrentStatus;
                SellerType();
                SellerIssueDate.Properties.MaxValue = DateTime.Today;
                SellerSurnameText.Focus();
                LoadSellerDetails();
                if (!CurrentStatus)
                    InsertPhonesTemp();

                LoadPhoneDataGridView(SellerID);
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 3)
            {
                InsuranceBrandText.Text = BrandComboBox.Text;
                InsuranceModelText.Text = ModelComboBox.Text;
                InsuranceTypeText.Text = TypeComboBox.Text;
                InsuranceYearText.Text = YearValue.Text;
                InsuranceColorText.Text = ColorComboBox.Text;
                InsuranceBanText.Text = BanText.Text;
                InsuranceCarAmountValue.EditValue = LiquidValue.Value;
                InsuranceCarAmountCurrencyLabel.Text = LiquidCurrencyLookUp.Text;

                if (!CurrentStatus && TransactionName == "EDIT")
                    InsertInsuranceTemp();
                InsuranceStandaloneBarDockControl.Enabled = !CurrentStatus;
                LoadInsuranceDataGridView();
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 5)
                LoadFilesDataGridView();
            else if (OtherInfoTabControl.SelectedTabPageIndex == 6)
            {                
                NewCommitmentBarSubItem.Enabled = !CurrentStatus;
                InsertCommitmensTemp();
                LoadCommitmentDataGridView();
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 7)
            {                
                InsertPowerTemp();
                PowerOfAttorneyStandaloneBarDockControl.Enabled = !CurrentStatus;
                LoadPowerDataGridView();
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 0)
                PaymentSchedulesGridControl.DataSource = null;
            else if (OtherInfoTabControl.SelectedTabPageIndex == 4)
            {
                BLoadContractPicture.Enabled = !CurrentStatus;
                BDeleteContractPicture.Enabled = !CurrentStatus;
                ContractNoteText.Enabled = !CurrentStatus;
                contract_images_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACT_IMAGES WHERE CONTRACT_ID = {ContractID}");
                if (contract_images_count == 0)
                    BChangeContractPicture.Enabled = false;
                else
                    BChangeContractPicture.Enabled = !CurrentStatus;
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkil";
                        break;
                    case "EN":
                        ImageCountLabel.Text = ContractImageSlider.Images.Count + " picture";
                        break;
                    case "RU":
                        ImageCountLabel.Text = ContractImageSlider.Images.Count + " картина";
                        break;
                }
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 8)
            {
                //if (!CurrentStatus && TransactionName == "EDIT")
                InsertInterestPenaltiesTemp();
                InterestPenaltiesStandaloneBarDockControl.Enabled = !CurrentStatus;
                LoadInterestPenaltiesDataGridView();
            }
        }

        private void LoadPaymentScheduleDataGridView()
        {
            string s = $@"SELECT P.ORDER_ID SS,
                                     P.ID,
                                     P.MONTH_DATE,
                                     P.MONTHLY_PAYMENT,
                                     P.BASIC_AMOUNT,
                                     P.INTEREST_AMOUNT,
                                     P.DEBT,
                                     C.CODE CURRENCY_CODE,
                                     P.IS_CHANGE_DATE,
                                     P.VERSION,
                                     NVL(A.CONTRACT_AMOUNT,{CreditAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)}) CONTRACT_AMOUNT,
                                     (CASE
                                         WHEN P.VERSION = 0
                                         THEN
                                            (P.VERSION + 1)||'. Əsas (Lizinq məbləği: '
                                            || NVL (A.CONTRACT_AMOUNT, {CreditAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)})
                                            || ' {CreditCurrencyLookUp.Text}' ||(CASE WHEN A.START_DATE IS NULL OR A.END_DATE IS NULL THEN NULL ELSE ', tarix intervalı: '|| TO_CHAR(A.START_DATE,'DD.MM.YYYY')||' - '||TO_CHAR(A.END_DATE,'DD.MM.YYYY') END)||')'
                                         WHEN P.VERSION > 0
                                         THEN
                                            (P.VERSION + 1)||'. Uzadılmış (Lizinq məbləği: '
                                            || NVL (A.CONTRACT_AMOUNT, {CreditAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)})
                                            || ' {CreditCurrencyLookUp.Text}' ||(CASE WHEN A.START_DATE IS NULL OR A.END_DATE IS NULL THEN NULL ELSE ', tarix intervalı: '|| TO_CHAR(A.START_DATE,'DD.MM.YYYY')||' - '||TO_CHAR(A.END_DATE,'DD.MM.YYYY') END)||')'                                             
                                      END)
                                        VERSION_DESCRIPTION
                                FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP P,
                                     CRS_USER.CURRENCY C,
                                     CRS_USER_TEMP.V_CONTRACT_AMOUNT_TEMP A
                               WHERE     P.CURRENCY_ID = C.ID
                                     AND P.CONTRACT_ID = A.CONTRACT_ID(+)
                                     AND P.VERSION = A.VERSION(+)
                                     AND P.IS_CHANGE != 2
                                     AND P.CONTRACT_ID = {ContractID}
                            ORDER BY P.VERSION, P.ORDER_ID DESC";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentScheduleDataGridView", "Ödəniş qrafiki açılmadı.");

            PaymentSchedulesGridControl.DataSource = dt;

            if (PaymentSchedulesGridView.RowCount > 0)
                PrintPaymentSchedulesBarButton.Enabled = !CurrentStatus;
            else
                PrintPaymentSchedulesBarButton.Enabled = false;
        }

        private void PaymentSchedulesGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PaymentSchedulesGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("MONTHLY_PAYMENT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
        }

        private bool ControlPaymentSchedulesDetails()
        {
            bool b = false;

            if (FirstPaymentValue.Value < 0)
            {
                FirstPaymentValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İlkin ödəniş mənfi ədəd ola bilməz.");
                FirstPaymentValue.Focus();
                FirstPaymentValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CreditCurrencyLookUp.EditValue == null)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CreditCurrencyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta seçilməyib.");
                CreditCurrencyLookUp.Focus();
                CreditCurrencyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LiquidCurrencyLookUp.EditValue == null)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LiquidCurrencyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta seçilməyib.");
                LiquidCurrencyLookUp.Focus();
                LiquidCurrencyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void LoadFPaymentSchedulesReportViewer()
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Ödəniş qrafiki.docx"))
            {
                XtraMessageBox.Show("Ödəniş qrafikinin şablon faylı tapılmadı.");
                return;
            }

            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            OtherInfoTabControl.SelectedTabPageIndex = 2;

            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
            string pobject = null,
                note = null;                       

            if (credit_name_id == 1)
                pobject = BrandComboBox.Text + " " + ModelComboBox.Text + " - " + TypeComboBox.Text + ", Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text;
            else
                pobject = "Ünvanı: " + ObjectAddressText.Text.Trim() + ", Çıxarışı: " + ObjectExcerptText.Text.Trim() + ", Sahəsi: " + ObjectAreaValue.Value.ToString() + " m²";

            if (credit_currency_id != 1)
                note = $@"Qeyd: Lizinq müqaviləsi üzrə uçot {credit_currency_name} ilə aparılır. Ödəniş həmin günə AR Mərkəzi Bankının müəyyən etdiyi rəsmi məzənnə ilə Azərbaycan manatı ilə aparılır. Ödəniş {(PaymentTypeRadioGroup.SelectedIndex == 0 ? "annuitet" : "fərdi")} ödəniş qrafiki əsasında aparılır.";
            else
                note = null;

            try
            {
                string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Qrafik.docx";
                object missing = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Ödəniş qrafiki.docx"),
                   saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();
                GlobalProcedures.FindAndReplace(wordApp, "[$code]", ContractCodeText.Text);

                GlobalProcedures.FindAndReplace(wordApp, "[$object]", pobject);
                GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$lamount]", LiquidValue.Value.ToString("n2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$famount]", FirstPaymentValue.Value.ToString("n2") + " " + LiquidCurrencyLookUp.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", CreditAmountValue.Value.ToString("n2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$mamount]", MonthlyPaymentValue.Value.ToString("n2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$lcur]", LiquidCurrencyLookUp.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$acur]", CreditCurrencyLookUp.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$mcur]", CreditCurrencyLookUp.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$paymenttype]", PaymentTypeRadioGroup.SelectedIndex == 0 ? "annuitet" : "fərdi");
                GlobalProcedures.FindAndReplace(wordApp, "[$note]", note);
                GlobalProcedures.FindAndReplace(wordApp, "[$period]", PeriodText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$percent]", InterestText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$gperiod]", GracePeriodValue.Value);

                Microsoft.Office.Interop.Word.Table table = aDoc.Tables[2];

                string sql = $@"SELECT P.ORDER_ID SS,                                     
                                     TO_CHAR(P.MONTH_DATE,'DD.MM.YYYY') MDATE,
                                     P.MONTHLY_PAYMENT,
                                     P.BASIC_AMOUNT,
                                     P.INTEREST_AMOUNT,
                                     P.DEBT
                                FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP P
                               WHERE     P.IS_CHANGE != 2
                                     AND P.CONTRACT_ID = {ContractID}
                            ORDER BY P.VERSION, P.ORDER_ID";
                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadFPaymentSchedulesReportViewer", "Ödəniş qrafiki açılmadı.");
                int i = 2;
                decimal sumPayment = 0, sumBasic = 0, sumInterest = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    table.Rows.Add(table.Rows[i]);

                    table.Cell(i, 1).Range.Text = dr["SS"].ToString();
                    //table.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 2).Range.Text = dr["MDATE"].ToString();
                    //table.Cell(i, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;                    
                    sumPayment += Convert.ToDecimal(dr["MONTHLY_PAYMENT"]);
                    table.Cell(i, 3).Range.Text = Convert.ToDecimal(dr["MONTHLY_PAYMENT"]).ToString("n2");
                    //table.Cell(i, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    sumBasic += Convert.ToDecimal(dr["BASIC_AMOUNT"]);
                    table.Cell(i, 4).Range.Text = Convert.ToDecimal(dr["BASIC_AMOUNT"]).ToString("n2");
                    //table.Cell(i, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    sumInterest += Convert.ToDecimal(dr["INTEREST_AMOUNT"]);
                    table.Cell(i, 5).Range.Text = Convert.ToDecimal(dr["INTEREST_AMOUNT"]).ToString("n2");
                    //table.Cell(i, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 6).Range.Text = Convert.ToDecimal(dr["DEBT"]).ToString("n2");
                    //table.Cell(i, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    
                    i++;
                }

                table.Rows[i].Delete();

                GlobalProcedures.FindAndReplace(wordApp, "[$sumpayment]", sumPayment.ToString("n2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$sumbasic]", sumBasic.ToString("n2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$suminterest]", sumInterest.ToString("n2"));

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);
                
                Process.Start(filePath);
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                XtraMessageBox.Show("Ödəniş qrafiki.doc faylı açıq olduğu üçün həmin faylı yenidən yaratmaq olmaz.\r\n\r\nError: " + exx.Message);
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void LoadFExtendPaymentSchedulesReportViewer()
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\RDLC\\ExtendPaymentSchedulesReport.rdlc"))
            {
                XtraMessageBox.Show("Ödəniş qrafikinin çap faylı yaradılmayıb.");
                return;
            }

            if (PaymentSchedulesGridView.RowCount == 0)
                CalculatedPaymentScheduleTemp(false);

            string mounthlyAmount = null, period = null;

            List<ContractExtend> lstContractEntend = ContractExtendDAL.SelectContractExtend(1, int.Parse(ContractID)).ToList<ContractExtend>();
            if (lstContractEntend.Count > 0)
            {
                var contractEntend = lstContractEntend.LastOrDefault();
                mounthlyAmount = contractEntend.MONTHLY_AMOUNT.ToString("### ### ### ### ### ### ##0.00");
                period = contractEntend.MONTH_COUNT.ToString();
            }

            string hostage_info = null, s,
                    bank_account = GlobalFunctions.GetName($@"SELECT NVL (LISTAGG (T.BANK_ACCOUNT, '; ')
                                                                             WITHIN GROUP (ORDER BY T.BANK_ACCOUNT),
                                                                          'Heç bir hesab seçilməyib')
                                                                          BANK_ACCOUNT
                                                                  FROM (SELECT B.LONG_NAME || ', HESAB:' || B.ACCOUNT BANK_ACCOUNT
                                                                          FROM CRS_USER.BANKS B
                                                                         WHERE B.IS_USED = 1 AND B.STATUS_ID = 7) T");
            if (credit_currency_id != 1)
                s = "Lizinq müqaviləsi üzrə uçot " + credit_currency_name + " ilə aparılır.                                                                                                                                                        Ödəniş həmin günə AR Mərkəzi Bankının məzənnəsi ilə Azərbaycan manatı ilə aparılır.";
            else
                s = " ";

            if (credit_name_id == 1)
                hostage_info = BrandComboBox.Text + ", Tip - " + BanTypeComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", Rəngi - " + ColorComboBox.Text + ", Ban - " + BanText.Text.Trim();
            else if ((credit_name_id == 5))
                hostage_info = "Ünvanı: " + ObjectAddressText.Text.Trim() + ", Çıxarış: " + ObjectExcerptText.Text.Trim() + ", Sahəsi: " + ObjectAreaValue.Value.ToString() + " m²";
            contract_date = GlobalFunctions.GetName("SELECT DAY_MONTH_YEAR FROM CRS_USER.DIM_TIME WHERE CALENDAR_DATE = TO_DATE('" + ContractStartDate.Text + "','DD/MM/YYYY')");

            rv_payment.LocalReport.ReportPath = GlobalVariables.V_ExecutingFolder + "\\RDLC\\ExtendPaymentSchedulesReport.rdlc";
            ReportDataSource rds = new ReportDataSource("PaymentSchedulesDataSet", getData(1));
            rv_payment.LocalReport.DataSources.Clear();
            rv_payment.LocalReport.DataSources.Add(rds);

            ReportParameter p1 = new ReportParameter("PHostageInfo", hostage_info);
            ReportParameter p2 = new ReportParameter("PCustomerName", CustomerFullNameText.Text.Trim());
            ReportParameter p3 = new ReportParameter("PCreditAmount", CreditAmountValue.Value.ToString("### ### ### ### ### ### ##0.00"));
            ReportParameter p4 = new ReportParameter("PInterest", InterestText.Text.Trim());
            ReportParameter p5 = new ReportParameter("PTerm", period);
            ReportParameter p6 = new ReportParameter("PMonthlyAmount", mounthlyAmount);
            ReportParameter p7 = new ReportParameter("PDate", contract_date);
            ReportParameter p8 = new ReportParameter("PGracePeriod", GracePeriodValue.Value.ToString());
            ReportParameter p9 = new ReportParameter("PContractCodeDescription", ContractCodeText.Text.Trim() + " saylı müqaviləyə ƏLAVƏ 2");
            ReportParameter p10 = new ReportParameter("PDescription", s);
            ReportParameter p11 = new ReportParameter("PPurpose", "Ödənişin təyinatı: " + ContractCodeText.Text.Trim() + " - " + CustomerFullNameText.Text.Trim() + " - " + CustomerAccountText.Text.Trim());
            ReportParameter p12 = new ReportParameter("PCredirCurrency", credit_currency_name);
            rv_payment.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 });
            Warning[] warnings;
            try
            {
                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Reports\\Ödəniş qrafiki.doc"))
                    File.Delete(GlobalVariables.V_ExecutingFolder + "\\Reports\\Ödəniş qrafiki.doc");

                using (var stream = File.Create(GlobalVariables.V_ExecutingFolder + "\\Reports\\Ödəniş qrafiki.doc"))
                {
                    rv_payment.LocalReport.Render(
                        "WORD",
                        @"<DeviceInfo><ExpandContent>True</ExpandContent></DeviceInfo>",
                        (CreateStreamCallback)delegate { return stream; },
                        out warnings);
                }

                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Reports\\Ödəniş qrafiki.doc"))
                    Process.Start(GlobalVariables.V_ExecutingFolder + "\\Reports\\Ödəniş qrafiki.doc");
                else
                    XtraMessageBox.Show("Ödəniş qrafikinin çap faylı yaradılmayıb.");

            }
            catch
            {
                XtraMessageBox.Show("Ödəniş qrafiki.doc faylı açıq olduğu üçün həmin faylı yenidən yaratmaq olmaz.");
            }
        }

        private DataTable getData(int version)
        {
            string s = $@"SELECT ID,
                                   MONTH_DATE,
                                   MONTHLY_PAYMENT,
                                   BASIC_AMOUNT,
                                   INTEREST_AMOUNT,
                                   DEBT
                              FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP P
                             WHERE VERSION = { version} AND USED_USER_ID = {GlobalVariables.V_UserID} AND CONTRACT_ID = {ContractID}
                            ORDER BY ORDER_ID";

            return GlobalFunctions.GenerateDataTable(s, this.Name + "/getData", "Ödəniş qrafiki açılmadı.");
        }

        private void BPaymentSchedule_Click(object sender, EventArgs e)
        {
            if (ControlPaymentSchedulesDetails())
                LoadFPaymentSchedulesReportViewer();
        }

        private void CalcMonthlyAmount()//ayliq odenisin hesablanmasi
        {
            if (String.IsNullOrWhiteSpace(PeriodText.Text))
            {
                monthly_amount = 0;
                return;
            }

            int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
            if (!EndDateCheckEdit.Checked)
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, Convert.ToDouble(PeriodText.Text) - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));
            else
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));

            MonthlyPaymentValue.Value = (decimal)monthly_amount;
        }

        private void CreditAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                try
                {
                    if (CreditAmountValue.Value > 0)
                    {
                        FormStatus = false;
                        if (TransactionName != "AGREEMENT")
                            if ((credit_currency_id != currency_id) && (credit_currency_rate != 0))
                                FirstPaymentValue.Value = LiquidValue.Value - (decimal)((double)CreditAmountValue.Value * credit_currency_rate);
                            else
                                FirstPaymentValue.Value = LiquidValue.Value - CreditAmountValue.Value;
                        FormStatus = true;
                        CalcMonthlyAmount();
                        CalcFirstPaymentPercent();
                    }
                    else if (CreditAmountValue.Value == 0)
                    {
                        FirstPaymentValue.Value = 0;
                        monthly_amount = 0;
                        MonthlyPaymentValue.Value = 0;
                        DeletePaymentSchedulesTemp();
                    }
                    else
                    {
                        monthly_amount = 0;
                        MonthlyPaymentValue.Value = 0;
                        DeletePaymentSchedulesTemp();
                        LiquidValue.BackColor = Color.Red;
                        FirstPaymentValue.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("İlkin ödəniş likvid dəyərdən çox ola bilməz.");
                        FirstPaymentValue.Focus();
                        LiquidValue.BackColor = Color.White;
                        FirstPaymentValue.BackColor = Color.White;
                    }
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("İlkin ödəniş hesablanan zaman xəta baş verdi.", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void RefreshPaymentSchedulesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPaymentScheduleDataGridView();
        }

        private void PrintPaymentSchedulesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sheduleVersion == 0)            
                LoadFPaymentSchedulesReportViewer();
        }

        private void PaymentSchedulesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PaymentSchedulesGridView, PaymentSchedulesPopupMenu, e);
        }

        private void BCreditCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.CreditCalculator();
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PhonePopupMenu, e);
        }

        private void VisibleHostageDetails(int creditnameid)
        {
            if (creditnameid == 5)
            {
                ObjectAddressLabel.Visible = true;
                ObjectAddressText.Visible = true;
                ObjectExcerptLabel.Visible = true;
                ObjectExcerptText.Visible = true;
                ObjectAreaLabel.Visible = true;
                ObjectAreaValue.Visible = true;
                ObjectAreaDescriptionLabel.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
                ObjectAddressLabel.Location = new Point(0, 14);
                ObjectAddressText.Location = new Point(112, 11);
                label9.Location = new Point(99, 14);
                ObjectExcerptLabel.Location = new Point(0, 40);
                ObjectExcerptText.Location = new Point(112, 37);
                label10.Location = new Point(99, 40);
                ObjectAreaLabel.Location = new Point(0, 66);
                ObjectAreaValue.Location = new Point(112, 63);
                ObjectAreaDescriptionLabel.Location = new Point(190, 66);
                label11.Location = new Point(99, 66);
                BrandLabel.Visible =
                BrandComboBox.Visible =
                ModelComboBox.Visible =
                ModelLabel.Visible =
                TypeLabel.Visible =
                TypeComboBox.Visible =
                YearLabel.Visible =
                YearValue.Visible =
                EngineLabel.Visible =
                EngineValue.Visible =
                EngineNumberText.Visible =
                EngineNumberLabel.Visible =
                ChassisText.Visible =
                ChassisLabel.Visible =
                EngineValueDescriptionLabel.Visible =
                MilageLabel.Visible =
                MilageValue.Visible =
                MilageValueDescription.Visible =
                BanLabel.Visible =
                BanText.Visible =
                ColorComboBox.Visible =
                ColorLabel.Visible =
                CarNumberLabel.Visible =
                CarNumberText.Visible =
                InsuranceTab.PageVisible =
                PowerOfAttorneyTab.PageVisible =
                label1.Visible =
                label2.Visible =
                label3.Visible =
                label4.Visible =
                label5.Visible =
                label6.Visible =
                label8.Visible =
                BanTypeComboBox.Visible =
                BanTypeLabel.Visible = false;
                BRealEstate.Visible = ContractClosed;
            }
            else if (creditnameid == 1)
            {
                GlobalProcedures.FillComboBoxEdit(BrandComboBox, "CAR_BRANDS", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                GlobalProcedures.FillComboBoxEdit(TypeComboBox, "CAR_TYPES", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                TypeComboBox.SelectedIndex = 0;
                GlobalProcedures.FillComboBoxEdit(BanTypeComboBox, "CAR_BAN_TYPES", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                GlobalProcedures.FillComboBoxEdit(ColorComboBox, "CAR_COLORS", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                BrandLabel.Visible =
                    BrandComboBox.Visible =
                    ModelComboBox.Visible =
                    ModelLabel.Visible =
                    TypeLabel.Visible =
                    TypeComboBox.Visible =
                    YearLabel.Visible =
                    YearValue.Visible =
                    EngineLabel.Visible =
                    EngineValue.Visible =
                    EngineNumberText.Visible =
                    EngineNumberLabel.Visible =
                    ChassisText.Visible =
                    ChassisLabel.Visible =
                    EngineValueDescriptionLabel.Visible =
                    MilageLabel.Visible =
                    MilageValue.Visible =
                    MilageValueDescription.Visible =
                    BanLabel.Visible =
                    BanText.Visible =
                    ColorComboBox.Visible =
                    ColorLabel.Visible =
                    CarNumberLabel.Visible =
                    CarNumberText.Visible =
                    PaymentTypeLabel.Visible =
                    BanTypeComboBox.Visible =
                    BanTypeLabel.Visible =
                    label1.Visible =
                    label2.Visible =
                    label3.Visible =
                    label4.Visible =
                    label5.Visible =
                    label6.Visible =
                    label8.Visible =
                    PowerOfAttorneyTab.PageVisible =
                    InsuranceTab.PageVisible = true;
                ObjectAddressLabel.Visible =
                    ObjectAddressText.Visible =
                    ObjectExcerptLabel.Visible =
                    ObjectExcerptText.Visible =
                    ObjectAreaLabel.Visible =
                    ObjectAreaValue.Visible =
                    ObjectAreaDescriptionLabel.Visible =
                    label9.Visible =
                    label10.Visible =
                    label11.Visible = false;

                EngineNumberText.Location = new Point(112, 366);
                EngineNumberLabel.Location = new Point(0, 369);
                ChassisLabel.Location = new Point(0, 395);
                ChassisText.Location = new Point(112, 392);
                BPolice.Visible = ContractClosed;
            }
        }

        private void BrandComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            brand_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_BRANDS", "NAME", "ID", BrandComboBox.Text);
            GlobalProcedures.FillComboBoxEditWithSqlText(ModelComboBox, "SELECT NAME,NAME,NAME FROM CRS_USER.CAR_MODELS WHERE BRAND_ID = " + brand_id + " ORDER BY 1");
        }

        private void ContractStartDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
                CreditTypeParametr();

            if ((!String.IsNullOrEmpty(ContractStartDate.Text)) && (!String.IsNullOrEmpty(ContractEndDate.Text)))
            {
                int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
                if ((FormStatus) && (CreditAmountValue.Value > 0))
                {
                    if (!EndDateCheckEdit.Checked)
                    {
                        monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, Convert.ToDouble(PeriodText.Text) - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));
                        MonthlyPaymentValue.Value = (decimal)monthly_amount;
                    }
                    else
                    {
                        monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));
                        MonthlyPaymentValue.Value = (decimal)monthly_amount;
                    }
                }
                DifferenceDateLabel.Text = diff_month + " ay";
                GenerateCurrencyRateLabel();
                CreditAmountValue_EditValueChanged(sender, EventArgs.Empty);
            }
        }

        private string InsertContractCodeTemp()
        {
            int max_code_number = 0, a = 0;
            string code = "00000";

            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.CONTRACT_CODE_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID}", "Müqavilənin kodu temp cədvəldən silinmədi.");

            List<ContractCodeTemp> lstContractCode = ContractCodeTempDAL.SelectContractCode(null).ToList<ContractCodeTemp>();

            if (lstContractCode.Count == 0)
                max_code_number = GlobalFunctions.GetMax($@"SELECT NVL(MAX(TO_NUMBER(SUBSTR(C.CODE,1,3))),0) FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.IS_USED_FOR_CODE = 1 AND C.CREDIT_TYPE_ID = CT.ID AND CT.NAME_ID = {credit_name_id}");
            else
                max_code_number = lstContractCode.Max(c => c.CODE_NUMBER);

            a = max_code_number + 1;
            code = credit_type_code + a.ToString().PadLeft(3, '0');
            if (GlobalFunctions.ExecuteQuery("INSERT INTO CRS_USER_TEMP.CONTRACT_CODE_TEMP(USED_USER_ID,CONTRACT_ID,CODE_NUMBER,CODE)VALUES(" + GlobalVariables.V_UserID + "," + ContractID + "," + a + ",'" + code + "')",
                                                    "Müqavilənin maksimum nömrəsi temp cədvələ daxil edilmədi.") > 0)
                return code;
            else
                return null;
        }

        private void SellerFrontFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Sənədin ön üzünü seçin";
                    dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png;*.pdf)|*.jpeg;*.jpg;*.bmp;*.png;*.pdf|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Pdf files (*.pdf)|*.pdf";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        SellerFrontFaceButtonEdit.Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                SellerFrontFaceButtonEdit.Text = null;
            else
            {
                if (SellerFrontFaceButtonEdit.Text.Length != 0)
                    Process.Start(SellerFrontFaceButtonEdit.Text);
            }
        }

        private void SellerRearFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Sənədin arxa üzünü seçin";
                    dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png;*.pdf)|*.jpeg;*.jpg;*.bmp;*.png;*.pdf|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Pdf files (*.pdf)|*.pdf";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        SellerRearFaceButtonEdit.Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                SellerRearFaceButtonEdit.Text = null;
            else
            {
                if (SellerRearFaceButtonEdit.Text.Length != 0)
                    Process.Start(SellerRearFaceButtonEdit.Text);
            }
        }

        private void LoadPhoneDataGridView(string sellerID)
        {
            string s = null;
            try
            {
                s = $@"SELECT 1 SS,
                                 P.ID,
                                 PD.DESCRIPTION_AZ DESCRIPTION,
                                 '+' || C.CODE || P.PHONE_NUMBER PHONENUMBER,
                                 P.IS_SEND_SMS,
                                 KR.NAME KINDSHIP_RATE_NAME
                            FROM CRS_USER_TEMP.PHONES_TEMP P,
                                 CRS_USER.PHONE_DESCRIPTIONS PD,
                                 CRS_USER.COUNTRIES C,
                                 CRS_USER.KINDSHIP_RATE KR
                           WHERE     P.IS_CHANGE IN (0, 1)
                                 AND P.PHONE_DESCRIPTION_ID = PD.ID
                                 AND P.COUNTRY_ID = C.ID
                                 AND P.KINDSHIP_RATE_ID = KR.ID (+)
                                 AND P.OWNER_TYPE = '{seller_type}'
                                 AND P.OWNER_ID = {sellerID}
                        ORDER BY P.ORDER_ID";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");

                PhoneGridControl.DataSource = dt;

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
                GlobalProcedures.LogWrite("Telefon nömrələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void UpdatePhoneSendSms()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0 WHERE IS_SEND_SMS = 1 AND OWNER_TYPE IN ('S','JP') AND USED_USER_ID = {GlobalVariables.V_UserID}",
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
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 1 WHERE OWNER_TYPE = '{seller_type}' AND ID = {row["ID"]} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                                this.Name + "/UpdatePhoneSendSms");
            }

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE IN ('S','JP') AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Telefon nömrələri dəyişdirilmədi.",
                                                this.Name + "/UpdatePhoneSendSms");
        }

        private void InsertPhones()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.PHONES WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.PHONES_TEMP WHERE OWNER_TYPE = '{seller_type}' AND IS_CHANGE <> 0 AND OWNER_ID = {SellerID})",
                                                    $@"INSERT INTO CRS_USER.PHONES(ID,PHONE_DESCRIPTION_ID,COUNTRY_ID,PHONE_NUMBER,OWNER_ID,OWNER_TYPE,IS_SEND_SMS,ORDER_ID,KINDSHIP_RATE_ID,KINDSHIP_NAME)SELECT ID,PHONE_DESCRIPTION_ID,COUNTRY_ID,PHONE_NUMBER,OWNER_ID,OWNER_TYPE,IS_SEND_SMS,ORDER_ID,KINDSHIP_RATE_ID,KINDSHIP_NAME FROM CRS_USER_TEMP.PHONES_TEMP WHERE OWNER_ID = {SellerID} AND IS_CHANGE = 1 AND OWNER_TYPE = '{seller_type}' AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Satıcının telefonları əsas cədvələ daxil edilmədi.");
        }

        private void InsertPhonesTemp()
        {
            if (TransactionName == "INSERT")
                return;

            ExecutePhonesTempProcedure(int.Parse(SellerID));
        }

        private void ExecutePhonesTempProcedure(int sellerID)
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "CRS_USER_TEMP.PROC_INSERT_SELLER_PHONE_TEMP";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_SELLER_ID", OracleDbType.Int32).Value = sellerID;
                    command.Parameters.Add("P_SELLER_TYPE", OracleDbType.Varchar2).Value = seller_type;
                    command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Telefonlar temp cədvələ daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, MethodBase.GetCurrentMethod().Name + "/CRS_USER_TEMP.PROC_INSERT_SELLER_PHONE_TEMP", exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        void RefreshPhone()
        {
            LoadPhoneDataGridView(SellerID);
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
            PhoneGridControl.Focus();
        }

        private void NewPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("INSERT", SellerID, seller_type, null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", SellerID, seller_type, PhoneID);
        }

        private void DeletePhone()
        {
            DialogResult dialogResult = XtraMessageBox.Show(PhoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE IN ('S','JP') AND OWNER_ID = {SellerID} AND ID = {PhoneID}", "Telefon nömrəsi temp cədvəldən silinmədi.");
            }
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
            LoadPhoneDataGridView(SellerID);
        }

        private void PhoneGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PhoneGridView.GetFocusedDataRow();
            if (row != null)
            {
                PhoneID = row["ID"].ToString();
                PhoneNumber = row["PHONENUMBER"].ToString();
                UpPhoneBarButton.Enabled = !(PhoneGridView.FocusedRowHandle == 0);
                DownPhoneBarButton.Enabled = !(PhoneGridView.FocusedRowHandle == PhoneGridView.RowCount - 1);
            }
        }

        private void RefreshPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDataGridView(SellerID);
        }

        private void BLoadSellerPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Satıcının şəkilini seçin";
                dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SellerPictureBox.Image = new Bitmap(dlg.FileName);
                    SellerImage = dlg.FileName;
                    BDeleteSellerPicture.Enabled = true;
                    BLoadSellerPicture.Text = "Dəyiş";
                }
                dlg.Dispose();
            }
        }

        private void BDeleteSellerPicture_Click(object sender, EventArgs e)
        {
            SellerPictureBox.Image = null;
            SellerImage = null;
            BLoadSellerPicture.Text = "Yüklə";
            BDeleteSellerPicture.Enabled = false;
        }

        private void BrandComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadHostageDictionaries("E", 8, 0);
        }

        private void TypeComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadHostageDictionaries("E", 8, 1);
        }

        private void ColorComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadHostageDictionaries("E", 8, 2);
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled)
                LoadFPhoneAddEdit("EDIT", SellerID, seller_type, PhoneID);
        }

        private int InsertContract()
        {
            int max_code_number = (TransactionName == "INSERT") ? GlobalFunctions.GetMax($@"SELECT NVL(MAX(TO_NUMBER(SUBSTR (C.CODE, 1, 3))),0) FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.IS_USED_FOR_CODE = 1 AND C.CREDIT_TYPE_ID = CT.ID AND CT.NAME_ID = {credit_name_id}") + 1 : 0,
                check_period = 0,
                check_interest = -1,
                isExpenses = 0;

            string code = null;

            if (TransactionName == "INSERT")
            {
                if (changecode)
                    code = GlobalFunctions.StringRight(ContractCodeText.Text, 3);
                else
                    code = max_code_number.ToString().PadLeft(3, '0').Trim();
            }
            else
            {
                code = ContractCodeText.Text.Substring(1, ContractCodeText.Text.Length - 1);
                isExpenses = 1;
                seller_type_id = (seller_type_id == 0) ? old_seller_type_id : seller_type_id;
            }

            if (EndDateCheckEdit.Checked)
                check_period = Convert.ToInt32(PeriodText.Text);

            if (InterestCheckEdit.Checked)
                check_interest = Convert.ToInt32(InterestText.Text.Trim());

            if (!String.IsNullOrWhiteSpace(oldContractID))
                return GlobalFunctions.ExecuteQuery($@"INSERT INTO CRS_USER.CONTRACTS(
                                                                                ID,
                                                                                CODE,
                                                                                CUSTOMER_ID,
                                                                                SELLER_ID,
                                                                                CREDIT_TYPE_ID,
                                                                                START_DATE,
                                                                                END_DATE,
                                                                                GRACE_PERIOD,
                                                                                AMOUNT,
                                                                                CURRENCY_ID,
                                                                                CUSTOMER_ACCOUNT,
                                                                                LEASING_ACCOUNT,
                                                                                LEASING_INTEREST_ACCOUNT,
                                                                                CHECK_END_DATE,
                                                                                CUSTOMER_CARDS_ID,
                                                                                MONTHLY_AMOUNT,
                                                                                CHECK_PERIOD,
                                                                                PAYMENT_TYPE,
                                                                                CHECK_INTEREST,
                                                                                SELLER_ACCOUNT,
                                                                                LIQUID_TYPE,
                                                                                CURRENCY_RATE,
                                                                                SELLER_TYPE_ID,
                                                                                CUSTOMER_TYPE_ID,
                                                                                PARENT_ID,
                                                                                IS_EXPENSES
                                                                            )
                                                VALUES(
                                                            {ContractID},
                                                            '{code.Trim()}',
                                                            {CustomerID},
                                                            {SellerID},
                                                            {credit_type_id},
                                                            TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'),
                                                            TO_DATE('{ContractEndDate.Text}','DD/MM/YYYY'),
                                                            {GracePeriodValue.Value},
                                                            {Math.Round(CreditAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                  
                                                            {credit_currency_id},
                                                            '{CustomerAccountText.Text.Trim()}',
                                                            '{ LeasingAccountText.Text.Trim()}',
                                                            '{LeasingInterestAccountText.Text.Trim()}',
                                                            {check_end_date},
                                                            {customer_card_id},
                                                            {Math.Round(MonthlyPaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {check_period},
                                                            {PaymentTypeRadioGroup.SelectedIndex},
                                                            {check_interest},
                                                            '{SellerAccountText.Text.Trim()}',
                                                            {LiquidTypeRadioGroup.SelectedIndex},
                                                            {credit_currency_rate.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {seller_type_id},
                                                            {customer_type_id},
                                                            {oldContractID},
                                                            {isExpenses}
                                                        )",
                                     "Müqavilə bazaya daxil edilmədi.",
                                  this.Name + "/InsertContract");
            else
                return GlobalFunctions.ExecuteQuery($@"INSERT INTO CRS_USER.CONTRACTS(
                                                                                ID,
                                                                                CODE,
                                                                                CUSTOMER_ID,
                                                                                SELLER_ID,
                                                                                CREDIT_TYPE_ID,
                                                                                START_DATE,
                                                                                END_DATE,
                                                                                GRACE_PERIOD,
                                                                                AMOUNT,
                                                                                CURRENCY_ID,
                                                                                CUSTOMER_ACCOUNT,
                                                                                LEASING_ACCOUNT,
                                                                                LEASING_INTEREST_ACCOUNT,
                                                                                CHECK_END_DATE,
                                                                                CUSTOMER_CARDS_ID,
                                                                                MONTHLY_AMOUNT,
                                                                                CHECK_PERIOD,
                                                                                PAYMENT_TYPE,
                                                                                CHECK_INTEREST,
                                                                                SELLER_ACCOUNT,
                                                                                LIQUID_TYPE,
                                                                                CURRENCY_RATE,
                                                                                SELLER_TYPE_ID,
                                                                                CUSTOMER_TYPE_ID,
                                                                                IS_EXPENSES
                                                                            )
                                                VALUES(
                                                            {ContractID},
                                                            '{code.Trim()}',
                                                            {CustomerID},
                                                            {SellerID},
                                                            {credit_type_id},
                                                            TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'),
                                                            TO_DATE('{ContractEndDate.Text}','DD/MM/YYYY'),
                                                            {GracePeriodValue.Value},
                                                            {Math.Round(CreditAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                  
                                                            {credit_currency_id},
                                                            '{CustomerAccountText.Text.Trim()}',
                                                            '{ LeasingAccountText.Text.Trim()}',
                                                            '{LeasingInterestAccountText.Text.Trim()}',
                                                            {check_end_date},
                                                            {customer_card_id},
                                                            {Math.Round(MonthlyPaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {check_period},
                                                            {PaymentTypeRadioGroup.SelectedIndex},
                                                            {check_interest},
                                                            '{SellerAccountText.Text.Trim()}',
                                                            {LiquidTypeRadioGroup.SelectedIndex},
                                                            {credit_currency_rate.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {seller_type_id},
                                                            {customer_type_id},
                                                            {isExpenses}
                                                        )",
                                     "Müqavilə bazaya daxil edilmədi.",
                                  this.Name + "/InsertContract");
        }

        private void UpdateContract()
        {
            int check_period = 0, check_interest = -1;
            if (EndDateCheckEdit.Checked)
                check_period = Convert.ToInt32(PeriodText.Text);
            if (InterestCheckEdit.Checked)
                check_interest = Convert.ToInt32(InterestText.Text.Trim());
            if (seller_type_id == 0)
                seller_type_id = old_seller_type_id;
            string s = $@"UPDATE CRS_USER.CONTRACTS SET 
                                        CREDIT_TYPE_ID = {credit_type_id},
                                        START_DATE = TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'), 
                                        END_DATE = TO_DATE('{ContractEndDate.Text}','DD/MM/YYYY'),
                                        GRACE_PERIOD = {GracePeriodValue.Value},
                                        AMOUNT = {Math.Round(CreditAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                        CURRENCY_ID = {credit_currency_id},
                                        CUSTOMER_ACCOUNT = '{CustomerAccountText.Text.Trim()}',
                                        LEASING_ACCOUNT = '{LeasingAccountText.Text.Trim()}',
                                        LEASING_INTEREST_ACCOUNT = '{LeasingInterestAccountText.Text.Trim()}',
                                        CHECK_END_DATE = {check_end_date},
                                        CUSTOMER_CARDS_ID = {customer_card_id},
                                        MONTHLY_AMOUNT = {Math.Round(MonthlyPaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                        CHECK_PERIOD = {check_period},
                                        PAYMENT_TYPE = {PaymentTypeRadioGroup.SelectedIndex},
                                        CHECK_INTEREST = {check_interest},
                                        SELLER_ACCOUNT = '{SellerAccountText.Text.Trim()}',
                                        LIQUID_TYPE = {LiquidTypeRadioGroup.SelectedIndex},
                                        CURRENCY_RATE = {credit_currency_rate.ToString(GlobalVariables.V_CultureInfoEN)},
                                        SELLER_ID = {SellerID},
                                        SELLER_TYPE_ID = {seller_type_id}, 
                                        CUSTOMER_TYPE_ID = {customer_type_id}    
                                        WHERE ID = {ContractID}";
            GlobalProcedures.ExecuteQuery(s, "Müqavilə bazada dəyişdirilmədi.", this.Name + "/UpdateContract");
        }

        private void InsertHostageCar()
        {
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.HOSTAGE_CAR(CONTRACT_ID,BRAND_ID,MODEL_ID,TYPE_ID,COLOR_ID,YEAR,MILAGE,BAN,ENGINE,ENGINE_NUMBER,CAR_NUMBER,LIQUID_AMOUNT,FIRST_PAYMENT,CURRENCY_ID,CHASSIS_NUMBER,BAN_TYPE_ID)VALUES(" + ContractID + "," + brand_id + "," + model_id + "," + type_id + "," + color_id + "," + YearValue.Value + "," + MilageValue.Value + ",'" + BanText.Text.Trim() + "'," + EngineValue.Value + ",'" + EngineNumberText.Text.Trim() + "','" + CarNumberText.Text.ToUpper().Trim() + "'," + LiquidValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + FirstPaymentValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + currency_id + ",'" + ChassisText.Text.Trim() + "'," + ban_type_id + ")",
                                                "Avtomobilin göstəriciləri bazaya daxil edilmədi.",
                                           this.Name + "/InsertHostageCar");
        }

        private void UpdateHostageCar()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.HOSTAGE_CAR SET BRAND_ID = " + brand_id + ",MODEL_ID = " + model_id + ",TYPE_ID = " + type_id + ",COLOR_ID = " + color_id + ",YEAR = " + YearValue.Value + ",MILAGE = " + MilageValue.Value + ",BAN = '" + BanText.Text.Trim() + "',ENGINE = " + EngineValue.Value + ",ENGINE_NUMBER = '" + EngineNumberText.Text.Trim() + "',CAR_NUMBER = '" + CarNumberText.Text.ToUpper().Trim() + "',LIQUID_AMOUNT = " + LiquidValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",FIRST_PAYMENT = " + FirstPaymentValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",CURRENCY_ID = " + currency_id + ",CHASSIS_NUMBER = '" + ChassisText.Text.Trim() + "',BAN_TYPE_ID = " + ban_type_id + " WHERE CONTRACT_ID = " + ContractID,
                                                "Avtomobilin göstəriciləri bazada dəyişdirilmədi.",
                                           this.Name + "/UpdateHostageCar");
        }

        private void InsertHostageObject()
        {
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.HOSTAGE_OBJECT(CONTRACT_ID,ADDRESS,EXCERPT,AREA,LIQUID_AMOUNT,FIRST_PAYMENT,CURRENCY_ID)VALUES(" + ContractID + ",'" + ObjectAddressText.Text.Trim() + "','" + ObjectExcerptText.Text.Trim() + "'," + ObjectAreaValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(LiquidValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + Math.Round(FirstPaymentValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + currency_id + ")",
                                                "Daşınmaz əmlakın göstəriciləri bazaya daxil edilmədi.",
                                          this.Name + "/InsertHostageObject");
        }

        private void UpdateHostageObject()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.HOSTAGE_OBJECT SET ADDRESS = '" + ObjectAddressText.Text.Trim() + "',EXCERPT = '" + ObjectExcerptText.Text.Trim() + "',AREA = " + ObjectAreaValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",LIQUID_AMOUNT = " + Math.Round(LiquidValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",FIRST_PAYMENT = " + Math.Round(FirstPaymentValue.Value, 2).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",CURRENCY_ID = " + currency_id + " WHERE CONTRACT_ID = " + ContractID,
                                                "Daşınmaz əmlakın göstəriciləri bazada dəyişdirilmədi.",
                                          this.Name + "/UpdateHostageObject");
        }

        private void InsertContractSubData()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_CONTRACT_SUB_DATA", "P_CONTRACT_ID", ContractID, "Müqavilənin alt məlumatları əsas cədvəllərə daxil olmadı.");
        }

        private bool ControlContractDetails()
        {
            bool b = false;

            if (CreditNameLookUp.EditValue == null)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CreditNameLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinqin növü daxil edilməyib.");
                CreditNameLookUp.Focus();
                CreditNameLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;


            if (credit_name_id == 1)
            {
                if (String.IsNullOrEmpty(BrandComboBox.Text))
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    BrandComboBox.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Marka daxil edilməyib.");
                    BrandComboBox.Focus();
                    BrandComboBox.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (String.IsNullOrEmpty(ModelComboBox.Text))
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    BrandComboBox.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Model daxil edilməyib.");
                    BrandComboBox.Focus();
                    BrandComboBox.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (String.IsNullOrEmpty(TypeComboBox.Text))
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    TypeComboBox.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Tip daxil edilməyib.");
                    TypeComboBox.Focus();
                    TypeComboBox.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (String.IsNullOrEmpty(BanTypeComboBox.Text))
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    BanTypeComboBox.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Ban tipi daxil edilməyib.");
                    BanTypeComboBox.Focus();
                    BanTypeComboBox.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (YearValue.Value < 1950 || YearValue.Value > DateTime.Today.Year)
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    YearValue.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage($@"İl 1950-ci ildən kiçik və {GlobalFunctions.FindYear(DateTime.Today)}dən böyük ola bilməz.");
                    YearValue.Focus();
                    YearValue.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (String.IsNullOrEmpty(ColorComboBox.Text))
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    ColorComboBox.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Rəng daxil edilməyib.");
                    ColorComboBox.Focus();
                    ColorComboBox.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (BanText.Text.Length == 0)
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    BanText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Ban daxil edilməyib.");
                    BanText.Focus();
                    BanText.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                //if (CarNumberText.Text.Length == 0)
                //{
                //    OtherInfoTabControl.SelectedTabPageIndex = 0;
                //    CarNumberText.BackColor = Color.Red;
                //    GlobalProcedures.ShowErrorMessage("Qeydiyyat nişanı daxil edilməyib.");
                //    CarNumberText.Focus();
                //    CarNumberText.BackColor = GlobalFunctions.ElementColor();
                //    return false;
                //}
                //else
                //    b = true;
            }

            if (credit_name_id == 5)
            {
                if (String.IsNullOrEmpty(ObjectAddressText.Text))
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    ObjectAddressText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Ünvan daxil edilməyib.");
                    ObjectAddressText.Focus();
                    ObjectAddressText.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (String.IsNullOrEmpty(ObjectExcerptText.Text))
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    ObjectExcerptText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Çıxarış daxil edilməyib.");
                    ObjectExcerptText.Focus();
                    ObjectExcerptText.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;

                if (ObjectAreaValue.Value <= 0)
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 0;
                    ObjectAreaValue.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Sahə sıfırdan böyük olmalıdır.");
                    ObjectAreaValue.Focus();
                    ObjectAreaValue.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;
            }

            if (LiquidValue.Value <= 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LiquidValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Likvid dəyəri sıfırdan böyük olmalıdır.");
                LiquidValue.Focus();
                LiquidValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LiquidCurrencyLookUp.EditValue == null)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LiquidCurrencyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta seçilməyib.");
                LiquidCurrencyLookUp.Focus();
                LiquidCurrencyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CreditAmountValue.Value <= 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CreditAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                CreditAmountValue.Focus();
                CreditAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CreditCurrencyLookUp.EditValue == null)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CreditCurrencyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta seçilməyib.");
                CreditCurrencyLookUp.Focus();
                CreditCurrencyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LiquidTypeRadioGroup.SelectedIndex == 1 && LiquidBankComboBox.Text.Length == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LiquidBankComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                LiquidBankComboBox.Focus();
                LiquidBankComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (FirstPaymentTypeRadioGroup.SelectedIndex == 1 && FirstPaymentBankComboBox.Text.Length == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                FirstPaymentBankComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                FirstPaymentBankComboBox.Focus();
                FirstPaymentBankComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;


            if (sellerdetails)
            {
                if (SellerTypeRadioGroup.SelectedIndex == 0)
                {
                    if (SellerSurnameText.Text.Length == 0)
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerSurnameText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının soyadı daxil edilməyib.");
                        SellerSurnameText.Focus();
                        SellerSurnameText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (SellerNameText.Text.Length == 0)
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerNameText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının adı daxil edilməyib.");
                        SellerNameText.Focus();
                        SellerNameText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (SellerPatronymicText.Text.Length == 0)
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerPatronymicText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının atasının adı daxil edilməyib.");
                        SellerPatronymicText.Focus();
                        SellerPatronymicText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (SellerSeriesLookUp.EditValue == null)
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerSeriesLookUp.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının şəxsiyyəti təsdiq edən sənədinin seriyası daxil edilməyib.");
                        SellerSeriesLookUp.Focus();
                        SellerSeriesLookUp.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (SellerNumberText.Text.Length == 0)
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerNumberText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının şəxsiyyəti təsdiq edən sənədinin seriya nömrəsi daxil edilməyib.");
                        SellerNumberText.Focus();
                        SellerNumberText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if ((card_series_id == 2) && (SellerNumberText.Text.Trim().Length != 8))
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerNumberText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının şəxsiyyəti təsdiq edən sənədinin seriya nömrəsi 8 simvol olmalıdır.");
                        SellerNumberText.Focus();
                        SellerNumberText.BackColor = GlobalFunctions.ElementColor(); ;
                        return false;
                    }
                    else
                        b = true;

                    if ((card_series_id == 2 || card_series_id == 30) && SellerPinCodeText.Text.Length != 7)
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerPinCodeText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının şəxsiyyəti təsdiq edən sənədinin fin kodu 7 simvol olmalıdır.");
                        SellerPinCodeText.Focus();
                        SellerPinCodeText.BackColor = GlobalFunctions.ElementColor(); ;
                        return false;
                    }
                    else
                        b = true;

                    if (String.IsNullOrEmpty(SellerIssueDate.Text))
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerIssueDate.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının sənədinin verilmə tarixi daxil edilməyib.");
                        SellerIssueDate.Focus();
                        SellerIssueDate.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (SellerIssuingLookUp.EditValue == null)
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerIssuingLookUp.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının şəxsiyyətini təsdiq edən sənədini verən orqanın adı daxil edilməyib.");
                        SellerIssuingLookUp.Focus();
                        SellerIssuingLookUp.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (String.IsNullOrEmpty(SellerRegistrationAddressText.Text.Trim()))
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        SellerRegistrationAddressText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Satıcının qeydiyyatda olduğu ünvan daxil edilməyib.");
                        SellerRegistrationAddressText.Focus();
                        SellerRegistrationAddressText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;
                }
                else if (SellerTypeRadioGroup.SelectedIndex == 1)
                {
                    if (String.IsNullOrEmpty(JuridicalPersonNameText.Text.Trim()))
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        JuridicalPersonNameText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin adı daxil edilməyib.");
                        JuridicalPersonNameText.Focus();
                        JuridicalPersonNameText.BackColor = Class.GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (String.IsNullOrEmpty(JuridicalPersonVoenText.Text.Trim()))
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        JuridicalPersonVoenText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin vöen-i daxil edilməyib.");
                        JuridicalPersonVoenText.Focus();
                        JuridicalPersonVoenText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (String.IsNullOrEmpty(LeadingPersonNameText.Text.Trim()))
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        LeadingPersonNameText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Rəhbər şəxs daxil edilməyib.");
                        LeadingPersonNameText.Focus();
                        LeadingPersonNameText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;

                    if (String.IsNullOrEmpty(JuridicalPersonAddressText.Text.Trim()))
                    {
                        OtherInfoTabControl.SelectedTabPageIndex = 1;
                        JuridicalPersonAddressText.BackColor = Color.Red;
                        GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin ünvanı daxil edilməyib.");
                        JuridicalPersonAddressText.Focus();
                        JuridicalPersonAddressText.BackColor = GlobalFunctions.ElementColor();
                        return false;
                    }
                    else
                        b = true;
                }
            }

            if (String.IsNullOrEmpty(CustomerAccountText.Text.Trim()))
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CustomerAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştəri hesabı təyin edilməyib.");
                CustomerAccountText.Focus();
                CustomerAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(LeasingAccountText.Text.Trim()))
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LeasingAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinq hesabı təyin edilməyib.");
                LeasingAccountText.Focus();
                LeasingAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(LeasingInterestAccountText.Text.Trim()))
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LeasingInterestAccountText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Lizinq faiz hesabı təyin edilməyib.");
                LeasingInterestAccountText.Focus();
                LeasingInterestAccountText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND CT.NAME_ID = {credit_name_id} AND C.CODE = '{GlobalFunctions.StringRight(ContractCodeText.Text.Trim(), 3)}'") > 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text + " saylı lizinq müqaviləsi artıq bazaya daxil edilib.");
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //if (TransactionName == "INSERT" && credit_name_id == 1 && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID}") == 0)
            //{
            //    OtherInfoTabControl.SelectedTabPageIndex = 7;
            //    GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı lizinq müqaviləsi üçün ən azı 1 etibarnamə daxil edilməlidir.");
            //    return false;
            //}
            //else
            //    b = true;

            if (PowerGridView.RowCount > 0 && credit_name_id == 1 && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID} AND IS_RESPONSIBLE = 1") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 7;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı lizinq müqaviləsi üçün olan etibarnamələrin ən azı bir dənəsi əsas etibarnamə olmalıdır.");
                return false;
            }
            else
                b = true;

            if ((TransactionName == "INSERT" || TransactionName == "AGREEMENT") && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.INTEREST_PENALTIES_TEMP WHERE IS_CHANGE != 2 AND CONTRACT_ID = {ContractID}") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 8;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı lizinq müqaviləsi üçün cərimə faizi təyin edilməyib.");
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertBankOperation(int bank_id, int plan_id, int appointment_id, decimal income, decimal expenses)
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE APPOINTMENT_ID = {appointment_id} AND OPERATION_DATE = TO_DATE('{ContractStartDate.Text}','DD.MM.YYYY') AND CONTRACT_CODE = '" + ContractCodeText.Text.Trim() + "'",
                                                    "INSERT INTO CRS_USER.BANK_OPERATIONS(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,CONTRACT_CODE,ACCOUNTING_PLAN_ID) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + bank_id + ",TO_DATE('" + ContractStartDate.Text.Trim() + "','DD/MM/YYYY')," + appointment_id + "," + income.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + expenses.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",0,'" + ContractCodeText.Text.Trim() + "'," + plan_id + ")",
                                                    "Ödəniş bank əməliyyatlarına daxil olunmadı.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlContractDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FContractSaveWait));
                if (TransactionName == "INSERT")
                {
                    InsertSeller();
                    if (InsertContract() < 1)
                        return;
                    if (credit_name_id == 1)
                        InsertHostageCar();
                    else if (credit_name_id == 5)
                        InsertHostageObject();

                    if (GlobalVariables.CommitContract)
                    {
                        GlobalProcedures.ExecuteTwoQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_COMMIT = 1 WHERE ID = {ContractID}",
                                                         $@"UPDATE CRS_USER.CONTRACT_EXTEND SET IS_COMMIT = 1 WHERE IS_COMMIT = 0 AND CONTRACT_ID = {ContractID}",
                                                            "Müqavilə təsdiqlənmədi.");
                        InsertContractAmountToOperation();
                    }
                    InsertPaidOut();
                }
                else if (TransactionName == "EDIT")
                {
                    if (((Commit == 1 || Commit == 0) && GlobalVariables.CommitContract) || (Commit == 0 && !GlobalVariables.CommitContract && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.AGAIN_COMMITMENT WHERE CONTRACT_ID = {ContractID}") == 0))
                    {
                        ControlSeller();
                        UpdateSeller();
                        if (credit_name_id == 1)
                            UpdateHostageCar();
                        else if (credit_name_id == 5)
                            UpdateHostageObject();
                        UpdateContract();
                    }
                    else
                    {
                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_COMMITMENT_CHANGE", "P_CONTRACT_ID", ContractID, "Müqavilə təkrar təsdiq cədvəlinə daxil olmadı.");
                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_CONTRACT_EXTEND_CHANGE", "P_CONTRACT_ID", ContractID, "Müqavilənin uzadılması üçün təsdiq dəyişdirilmədi.");
                    }

                    if (GlobalVariables.CommitContract)
                    {
                        if (Commit == 0)
                        {
                            GlobalProcedures.ExecuteThreeQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_COMMIT = 1 WHERE ID = {ContractID}",
                                                               $@"DELETE FROM CRS_USER.AGAIN_COMMITMENT WHERE CONTRACT_ID = {ContractID}",
                                                               $@"INSERT INTO CRS_USER.CONTRACT_INTEREST_PENALTIES(ID,CONTRACT_ID,CALC_DATE)VALUES(INTEREST_PENALTIES_SEQUENCE.NEXTVAL,{ContractID},TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'))",
                                                            "Müqavilə təsdiqlənmədi.");
                            GlobalProcedures.InsertCashAdvancePayment(int.Parse(ContractID), int.Parse(CustomerID), ContractStartDate.Text, ContractStartDate.Text, ContractCodeText.Text.Trim() + " saylı müqavilə üzrə avans məbləği", Math.Round(Class.GlobalFunctions.CalculatedExchange((double)FirstPaymentValue.Value, currency_id, ContractStartDate.Text, 1, ContractStartDate.Text), 2), null, ContractCodeText.Text.Trim());
                        }

                        if (parent_contract_id == null)
                        {
                            if (old_first_payment != (double)FirstPaymentValue.Value || old_liquid_amount != (double)LiquidValue.Value || liquidtype_selectedindex != LiquidTypeRadioGroup.SelectedIndex || first_payment_selectedindex != FirstPaymentTypeRadioGroup.SelectedIndex || liquidbanktext != LiquidBankComboBox.Text || firstpaymentbanktext != FirstPaymentBankComboBox.Text)
                            {
                                if (FirstPaymentTypeRadioGroup.SelectedIndex == 0)
                                {
                                    if (first_payment_selectedindex != 0)
                                    {
                                        GlobalProcedures.InsertCashAdvancePayment(int.Parse(ContractID), int.Parse(CustomerID), ContractStartDate.Text, ContractStartDate.Text, ContractCodeText.Text.Trim() + " saylı müqavilə üzrə avans məbləği", Math.Round(GlobalFunctions.CalculatedExchange((double)FirstPaymentValue.Value, currency_id, ContractStartDate.Text, 1, ContractStartDate.Text), 2), null, ContractCodeText.Text.Trim());
                                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE BANK_ID = {first_payment_bank_id} AND ACCOUNTING_PLAN_ID = {first_payment_plan_id} AND APPOINTMENT_ID = 19 AND OPERATION_DATE = TO_DATE('{ContractStartDate.Text}','DD.MM.YYYY')",
                                                         "Ödəniş bank əməliyyatlarından silinmədi.");
                                        GlobalProcedures.UpdateBankOperationDebtWithBank(ContractStartDate.Text, first_payment_bank_id);
                                        GlobalProcedures.UpdateBankOperationDebt(ContractStartDate.Text);
                                    }
                                    else
                                        GlobalProcedures.UpdateCashAdvancePayment(int.Parse(ContractID), int.Parse(CustomerID), ContractStartDate.Text, ContractStartDate.Text, Math.Round(Class.GlobalFunctions.CalculatedExchange((double)FirstPaymentValue.Value, currency_id, ContractStartDate.Text, 1, ContractStartDate.Text), 2));
                                }
                                else
                                {
                                    //kassadan sil
                                    if (first_payment_selectedindex != 1)
                                    {
                                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_CASH_ADVANCE_DELETE", "P_CONTRACT_ID", int.Parse(ContractID), "Avans məbləği kassadan silinmədi.");
                                    }
                                    //banka yaz
                                    InsertBankOperation(first_payment_bank_id, first_payment_plan_id, 19, FirstPaymentValue.Value, 0);
                                    GlobalProcedures.UpdateBankOperationDebtWithBank(ContractStartDate.Text, first_payment_bank_id);
                                    GlobalProcedures.UpdateBankOperationDebt(ContractStartDate.Text);
                                }

                                if (LiquidTypeRadioGroup.SelectedIndex == 1)
                                {
                                    //kassadan sil
                                    if (liquidtype_selectedindex != 1)
                                    {
                                        GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_CASH_EXPENSES_DELETE", "P_CONTRACT_ID", int.Parse(ContractID), "Lizin müqaviləsi üzrə məxaric kassadan silinmədi.");
                                    }
                                    //banka yaz
                                    InsertBankOperation(liquid_bank_id, liquid_plan_id, 8, 0, LiquidValue.Value);
                                    GlobalProcedures.UpdateBankOperationDebtWithBank(ContractStartDate.Text, liquid_bank_id);
                                    GlobalProcedures.UpdateBankOperationDebt(ContractStartDate.Text);
                                }
                                else
                                {
                                    if (liquidtype_selectedindex != 0)
                                    {
                                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE BANK_ID = {liquid_bank_id} AND ACCOUNTING_PLAN_ID = {liquid_plan_id} AND APPOINTMENT_ID = 8 AND OPERATION_DATE = TO_DATE('{ContractStartDate.Text}','DD.MM.YYYY')",
                                                        "Ödəniş bank əməliyyatlarından silinmədi.");
                                        GlobalProcedures.UpdateBankOperationDebtWithBank(ContractStartDate.Text, liquid_bank_id);
                                        GlobalProcedures.UpdateBankOperationDebt(ContractStartDate.Text);
                                    }
                                }

                                GlobalProcedures.InsertOperationJournalForSeller(ContractStartDate.Text, (double)LiquidValue.Value, (double)FirstPaymentValue.Value, ContractID, liquid_account, first_payment_account);
                            }
                        }
                        else
                            InsertAgreementAmountToOperation();
                    }
                    UpdatePaidOut();
                }
                else if (TransactionName == "AGREEMENT")
                {
                    SellerID = InsertSellerForAgreement();
                    if (InsertContract() < 1)
                        return;
                    if (credit_name_id == 1)
                        InsertHostageCar();
                    else if (credit_name_id == 5)
                        InsertHostageObject();

                    if (GlobalVariables.CommitContract)
                    {
                        GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_COMMIT = 1 WHERE ID = {ContractID}",
                                                            "Müqavilə təsdiqlənmədi.");
                        InsertAgreementAmountToOperation();
                    }
                    InsertPaidOut();
                }


                if (PaymentSchedulesGridView.RowCount == 0)
                    CalculatedPaymentScheduleTemp(false);
                InsertContractImages();
                InsertContractDescriptions();
                InsertContractSubData();
                //UpdatePhoneSendSms();
                InsertPhones();
                UpdateCardFrontFace();
                UpdateCardRearFace();
                InsertContractDocument();
                GlobalProcedures.CalculatedLeasingTotal(ContractID);
                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }

        //private void InsertAgreementPayment()
        //{
        //    double payment_interest_amount = 0, currenct_payment_interest_debt = 0, basic_amount = 0, current_debt = 0, total = 0, payment_value_AZN = 0, penalty = 0;



        //    //if (PaymentValue.Value > 0) // eger odenis sifirdan boyuk olarsa
        //    //{
        //    //    if ((interest_amount + PaymentInterestDebt) > (double)PaymentValue.Value) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
        //    //        payment_interest_amount = (double)PaymentValue.Value;
        //    //    else
        //    //        payment_interest_amount = interest_amount + PaymentInterestDebt; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
        //    //}

        //    //if (PenaltyCheck.Checked)
        //    //{
        //    //    if ((double)PaymentValue.Value < (double)PenaltyValue.Value)
        //    //    {
        //    //        basic_amount = 0;
        //    //        penalty_amount = (double)PaymentValue.Value;
        //    //        payment_interest_amount = 0;
        //    //    }
        //    //    else
        //    //    {
        //    //        penalty = (double)PaymentValue.Value - (double)PenaltyValue.Value;
        //    //        if (penalty < payment_interest_amount)
        //    //        {
        //    //            payment_interest_amount = penalty;
        //    //            basic_amount = 0;
        //    //        }
        //    //        else
        //    //            basic_amount = penalty - payment_interest_amount;
        //    //        penalty_amount = (double)PenaltyValue.Value;
        //    //    }
        //    //    is_penalty = 1;
        //    //}
        //    //else
        //    //    basic_amount = (double)PaymentValue.Value - payment_interest_amount;
        //    //currenct_payment_interest_debt = PaymentInterestDebt + interest_amount - payment_interest_amount;
        //    //current_debt = Debt - basic_amount;
        //    //total = current_debt + currenct_payment_interest_debt;
        //    //if (currency_id == 1)
        //    //    payment_value_AZN = (double)PaymentValue.Value;
        //    //else
        //    //    payment_value_AZN = (double)PaymentAZNValue.Value;

        //    basic_amount = (double)CreditAmountValue.Value / old_credit_currency_rate;

        //    GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CUSTOMER_PAYMENTS(ID,
        //                                                                                        CUSTOMER_ID,
        //                                                                                        CONTRACT_ID,
        //                                                                                        PAYMENT_DATE,
        //                                                                                        PAYMENT_AMOUNT,
        //                                                                                        BASIC_AMOUNT,
        //                                                                                        DEBT,
        //                                                                                        DAY_COUNT,
        //                                                                                        ONE_DAY_INTEREST_AMOUNT,
        //                                                                                        INTEREST_AMOUNT,
        //                                                                                        PAYMENT_INTEREST_AMOUNT,
        //                                                                                        PAYMENT_INTEREST_DEBT,
        //                                                                                        TOTAL,
        //                                                                                        CURRENCY_RATE,
        //                                                                                        PAYMENT_AMOUNT_AZN,
        //                                                                                        REQUIRED_CLOSE_AMOUNT,
        //                                                                                        REQUIRED_AMOUNT,
        //                                                                                        PAYMENT_NAME,                                                                                                
        //                                                                                        BANK_CASH,
        //                                                                                        CUSTOMER_TYPE_ID)
        //                                        VALUES(CUSTOMER_PAYMENT_SEQUENCE.NEXTVAL,
        //                                                    {CustomerID},
        //                                                    {oldContractID},
        //                                                    TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'),
        //                                                    {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
        //                                                    {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
        //                                                    0,
        //                                                    0,
        //                                                    0,
        //                                                    0,
        //                                                    0,
        //                                                    0,
        //                                                    0,
        //                                                    {Math.Round(old_credit_currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN)},
        //                                                    {Math.Round(CreditAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
        //                                                    0,
        //                                                    0,
        //                                                    '{CustomerFullNameText.Text.Trim()}',                                                            
        //                                                    'D',
        //                                                    {customer_type_id})",
        //                                   "Ödəniş əsas cədvələ daxil edilmədi.",
        //                                   this.Name + "/InsertPayment");
        //    //GlobalProcedures.InsertOperationJournal(PaymentDate.Text, (double)CurrencyRateValue.Value, currency_id, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, basic_amount, payment_interest_amount, ContractID, PaymentID, null, 1);
        //}

        private void InsertContractImages()
        {
            if (contract_images_count != image_list.Count)
            {
                GlobalProcedures.ExecuteQuery($@"DELETE CRS_USER.CONTRACT_IMAGES WHERE CONTRACT_ID = {ContractID}", "Müqavilənin şəkilləri bazadan silinmədi.");
                for (int i = 0; i < image_list.Count; i++)
                {
                    GlobalFunctions.ExecuteQueryWithBlob($@"INSERT INTO CRS_USER.CONTRACT_IMAGES(CONTRACT_ID,IMAGE) 
                                                                             VALUES({ContractID},:BlobFile)", image_list[i], "Müqavilənin şəkilləri bazaya daxil edilmədi.");
                }
            }
        }

        private void InsertContractDescriptions()
        {
            if (String.IsNullOrEmpty(ContractNoteText.Text))
                return;

            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CONTRACT_DESCRIPTIONS WHERE CONTRACT_ID = {ContractID}",
                                              $@"INSERT INTO CRS_USER.CONTRACT_DESCRIPTIONS(CONTRACT_ID,NOTE) VALUES({ContractID},'{ContractNoteText.Text.Trim()}')",
                                                  "Müqavilənin təsvirləri bazaya daxil edilmədi.");

        }

        private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    type_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_TYPES", "NAME", "ID", TypeComboBox.Text);
                    break;
                case "EN":
                    type_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_TYPES", "NAME_EN", "ID", TypeComboBox.Text);
                    break;
                case "RU":
                    type_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_TYPES", "NAME_RU", "ID", TypeComboBox.Text);
                    break;
            }
        }

        private void ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    color_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_COLORS", "NAME", "ID", ColorComboBox.Text);
                    break;
                case "EN":
                    color_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_COLORS", "NAME_EN", "ID", ColorComboBox.Text);
                    break;
                case "RU":
                    color_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_COLORS", "NAME_RU", "ID", ColorComboBox.Text);
                    break;
            }
        }

        private void InsertSeller()
        {
            if (SellerTypeRadioGroup.SelectedIndex < 0)
                return;

            OracleTransaction transaction = null;
            OracleCommand command = null;
            using (OracleConnection connection = new OracleConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    command = connection.CreateCommand();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    if (SellerTypeRadioGroup.SelectedIndex == 0)
                    {
                        if (SellerImage != null)
                        {
                            FileStream fls = new FileStream(SellerImage, FileMode.Open, FileAccess.Read);
                            byte[] blob = new byte[fls.Length];
                            fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                            fls.Close();
                            command.CommandText = $@"INSERT INTO CRS_USER.SELLERS(ID,                                                                                                          
                                                                                   SURNAME,
                                                                                   NAME,
                                                                                   PATRONYMIC,
                                                                                   IMAGE,
                                                                                   CARD_SERIES_ID,
                                                                                   CARD_NUMBER,
                                                                                   CARD_PINCODE,
                                                                                   CARD_ISSUE_DATE,
                                                                                   CARD_ISSUING_ID,
                                                                                   ADDRESS,
                                                                                   REGISTRATION_ADDRESS,
                                                                                   WITH_POWER,
                                                                                   POWER_NUMBER,
                                                                                   POWER_NAME,
                                                                                   POWER_CARD_SERIES_ID,
                                                                                   POWER_CARD_NUMBER,
                                                                                   POWER_CARD_PINCODE,
                                                                                   POWER_CARD_ISSUE_DATE,
                                                                                   POWER_CARD_ISSUING_ID) 
                                                                            VALUES({SellerID},                                                                                  D
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',
                                                                                        :BlobImage,
                                                                                        {card_series_id},
                                                                                        '{SellerNumberText.Text}',
                                                                                        '{SellerPinCodeText.Text.Trim()}',
                                                                                        TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                        {card_issuing_id},
                                                                                        '{SellerAddressText.Text.Trim()}',
                                                                                        '{SellerRegistrationAddressText.Text}',
                                                                                        {(PowerOfAttorneyCheck.Checked ? 1 : 0)},
                                                                                        '{PowerOfAttorneyNumberText.Text.Trim()}',
                                                                                        '{SellerPowerOfAttorneyNameText.Text.Trim()}',
                                                                                        {power_card_series_id},
                                                                                        '{SellerPowerNumberText.Text}',
                                                                                        '{SellerPowerPinCodeText.Text.Trim()}',
                                                                                        TO_DATE('{SellerPowerIssueDate.Text}','DD/MM/YYYY'),
                                                                                        {power_card_issuing_id})";
                            OracleParameter blobParameter = new OracleParameter();
                            blobParameter.OracleDbType = OracleDbType.Blob;
                            blobParameter.ParameterName = "BlobImage";
                            blobParameter.Value = blob;
                            command.Parameters.Add(blobParameter);
                        }
                        else
                        {
                            command.CommandText = $@"INSERT INTO CRS_USER.SELLERS(  
                                                                                                            ID,                                                                                                          
                                                                                                            SURNAME,
                                                                                                            NAME,
                                                                                                            PATRONYMIC,                                                                                                            
                                                                                                            CARD_SERIES_ID,
                                                                                                            CARD_NUMBER,
                                                                                                            CARD_PINCODE,
                                                                                                            CARD_ISSUE_DATE,
                                                                                                            CARD_ISSUING_ID,
                                                                                                            ADDRESS,
                                                                                                            REGISTRATION_ADDRESS
                                                                                                    ) 
                                                                            VALUES(
                                                                                        {SellerID}, 
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',                                                                                        
                                                                                        {card_series_id},
                                                                                        '{SellerNumberText.Text}',
                                                                                        '{SellerPinCodeText.Text.Trim()}',
                                                                                        TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                        {card_issuing_id},
                                                                                        '{SellerAddressText.Text.Trim()}',
                                                                                        '{SellerRegistrationAddressText.Text}'
                                                                                  )";
                        }
                    }
                    else
                    {
                        command.CommandText = $@"INSERT INTO CRS_USER.JURIDICAL_PERSONS(
                                                                                                                ID,
                                                                                                                NAME,
                                                                                                                VOEN,
                                                                                                                LEADING_NAME,
                                                                                                                ADDRESS,
                                                                                                                IS_BUYER
                                                                                                          ) 
                                                               VALUES(    
                                                                        {SellerID},                                                                  
                                                                        '{JuridicalPersonNameText.Text.Trim()}',
                                                                        '{JuridicalPersonVoenText.Text.Trim()}',
                                                                        '{LeadingPersonNameText.Text.Trim()}',
                                                                        '{JuridicalPersonAddressText.Text.Trim()}',
                                                                        {0}
                                                                     )";

                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Satıcının məlumatları bazaya daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private string InsertSellerForAgreement()
        {
            string res = null;
            if (old_seller_type_id == 1)
                res = GlobalFunctions.ExecuteProcedureWithInParametrAndOutParametr("CRS_USER.PROC_DUPLICATE_SELLER", "P_SELLER_ID", int.Parse(SellerID), "SELLER_ID", OracleDbType.Int32, "Satıcının məlumatları bazaya daxil edilmədi.").ToString();
            else
                res = GlobalFunctions.ExecuteProcedureWithInParametrAndOutParametr("CRS_USER.PROC_DUPLICATE_JURIDICAL", "P_SELLER_ID", int.Parse(SellerID), "SELLER_ID", OracleDbType.Int32, "Satıcının məlumatları bazaya daxil edilmədi.").ToString();

            return res;
        }

        private void InsertPaidOut()
        {
            if (LiquidTypeRadioGroup.SelectedIndex != 1)
                liquid_bank_id = liquid_plan_id = 0;

            if (FirstPaymentTypeRadioGroup.SelectedIndex != 1)
                first_payment_bank_id = first_payment_plan_id = 0;

            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CONTRACT_PAID_OUT(
                                                                                    CONTRACT_ID,                                                                                    
                                                                                    LIQUID_BANK_ID,                                                                                   
                                                                                    LIQUID_PLAN_ID,
                                                                                    FIRST_PAYMENT_BANK_ID,                                                                                   
                                                                                    FIRST_PAYMENT_PLAN_ID
                                                                                  )
                                            VALUES(
                                                    {ContractID},
                                                    {liquid_bank_id},                                                  
                                                    {liquid_plan_id},
                                                    {first_payment_bank_id},                                                   
                                                    {first_payment_plan_id}
                                                  )",
                                                    "Satıcıya ödənilən pulun mənbəyi cədvələ daxil edilmədi.",
                                            this.Name + "/InsertPaidOut");
        }

        private void UpdatePaidOut()
        {
            if (LiquidTypeRadioGroup.SelectedIndex == 1)
            {
                string[] liquid_words = LiquidBankComboBox.Text.Split('/');
                liquid_account = liquid_words[1].Remove(0, 8).Trim();
                liquid_bank_id = GlobalFunctions.FindComboBoxSelectedValue("BANKS", "LONG_NAME", "ID", liquid_words[0].Trim());
                liquid_plan_id = GlobalFunctions.FindComboBoxSelectedValue("ACCOUNTING_PLAN", "BANK_ID = " + liquid_bank_id + " AND SUB_ACCOUNT", "ID", liquid_account);
            }
            else
                liquid_bank_id = liquid_plan_id = 0;

            if (FirstPaymentTypeRadioGroup.SelectedIndex == 1)
            {
                string[] first_payment_words = FirstPaymentBankComboBox.Text.Split('/');
                first_payment_account = first_payment_words[1].Remove(0, 8).Trim();
                first_payment_bank_id = GlobalFunctions.FindComboBoxSelectedValue("BANKS", "LONG_NAME", "ID", first_payment_words[0].Trim());
                first_payment_plan_id = GlobalFunctions.FindComboBoxSelectedValue("ACCOUNTING_PLAN", "BANK_ID = " + first_payment_bank_id + " AND SUB_ACCOUNT", "ID", first_payment_account);
            }
            else
                first_payment_bank_id = first_payment_plan_id = 0;

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_PAID_OUT SET 
                                                            LIQUID_BANK_ID = {liquid_bank_id},                                                            
                                                            LIQUID_PLAN_ID = {liquid_plan_id},
                                                            FIRST_PAYMENT_BANK_ID = {first_payment_bank_id},                                                            
                                                            FIRST_PAYMENT_PLAN_ID = {first_payment_plan_id} 
                                                            WHERE CONTRACT_ID = {ContractID}",
                                                    "Satıcıya ödənilən pulun mənbəyi cədvələ dəyişdirilmədi.",
                                          this.Name + "/UpdatePaidOut");
        }

        private void ControlSeller()
        {
            if (SellerTypeRadioGroup.SelectedIndex == 0 && old_seller_index == 1)
                GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.PHONES WHERE (OWNER_ID,OWNER_TYPE) IN (SELECT OWNER_ID,OWNER_TYPE FROM CRS_USER_TEMP.PHONES_TEMP
                                                                                                            WHERE OWNER_TYPE = 'JP' 
                                                                                                                AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                                                 $@"DELETE FROM CRS_USER.JURIDICAL_PERSONS 
                                                                                        WHERE ID = {old_seller_id}",
                                                                                "Hüquqi şəxs silinmədi.",
                                                     this.Name + "/ControlSeller");
            else if (SellerTypeRadioGroup.SelectedIndex == 1 && old_seller_index == 0)
                GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.PHONES WHERE (OWNER_ID,OWNER_TYPE) IN (SELECT OWNER_ID,OWNER_TYPE FROM CRS_USER_TEMP.PHONES_TEMP
                                                                                                    WHERE OWNER_TYPE = 'S' 
                                                                                                        AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                                                 $@"DELETE FROM CRS_USER.SELLERS 
                                                                                        WHERE ID = {old_seller_id}",
                                                                                "Fiziki şəxs silinmədi.",
                                                   this.Name + "/ControlSeller");
        }

        private void UpdateSeller()
        {
            if (!sellerdetails)
                return;

            OracleTransaction transaction = null;
            OracleCommand command = null;
            using (OracleConnection connection = new OracleConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }

                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    if (SellerTypeRadioGroup.SelectedIndex == 0)
                    {
                        if (SellerImage != null)
                        {
                            FileStream fls = new FileStream(SellerImage, FileMode.Open, FileAccess.Read);
                            byte[] blob = new byte[fls.Length];
                            fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                            fls.Close();
                            if (old_seller_index == SellerTypeRadioGroup.SelectedIndex)
                            {
                                command.CommandText = $@"UPDATE CRS_USER.SELLERS SET SURNAME = '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                     NAME = '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                     PATRONYMIC = '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',
                                                                                     IMAGE = :BlobImage,
                                                                                     CARD_SERIES_ID = {card_series_id},
                                                                                     CARD_NUMBER = '{SellerNumberText.Text}',
                                                                                     CARD_PINCODE = '{SellerPinCodeText.Text.Trim()}',
                                                                                     CARD_ISSUE_DATE = TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                     CARD_ISSUING_ID = {card_issuing_id},
                                                                                     ADDRESS = '{SellerAddressText.Text.Trim()}',
                                                                                     REGISTRATION_ADDRESS = '{SellerRegistrationAddressText.Text}',
                                                                                     WITH_POWER = {(PowerOfAttorneyCheck.Checked ? 1 : 0)},                                                                                        
                                                                                     POWER_NUMBER = '{PowerOfAttorneyNumberText.Text.Trim()}',
                                                                                     POWER_NAME = '{SellerPowerOfAttorneyNameText.Text.Trim()}',
                                                                                     POWER_CARD_SERIES_ID = {power_card_series_id},
                                                                                     POWER_CARD_NUMBER = '{SellerPowerNumberText.Text}',
                                                                                     POWER_CARD_PINCODE = '{SellerPowerPinCodeText.Text.Trim()}',
                                                                                     POWER_CARD_ISSUE_DATE = TO_DATE('{SellerPowerIssueDate.Text}','DD/MM/YYYY'),
                                                                                     POWER_CARD_ISSUING_ID = {power_card_issuing_id}
                                                          WHERE ID = {SellerID}";
                                OracleParameter blobParameter = new OracleParameter();
                                blobParameter.OracleDbType = OracleDbType.Blob;
                                blobParameter.ParameterName = "BlobImage";
                                blobParameter.Value = blob;
                                command.Parameters.Add(blobParameter);
                            }
                            else
                            {
                                command.CommandText = $@"INSERT INTO CRS_USER.SELLERS(ID,
                                                                                      SURNAME,
                                                                                      NAME,
                                                                                      PATRONYMIC,
                                                                                      IMAGE,
                                                                                      CARD_SERIES_ID,
                                                                                      CARD_NUMBER,
                                                                                      CARD_PINCODE,
                                                                                      CARD_ISSUE_DATE,
                                                                                      CARD_ISSUING_ID,
                                                                                      ADDRESS,
                                                                                      REGISTRATION_ADDRESS,
                                                                                      WITH_POWER,
                                                                                      POWER_NUMBER,
                                                                                      POWER_NAME,
                                                                                      POWER_CARD_SERIES_ID,
                                                                                      POWER_CARD_NUMBER,
                                                                                      POWER_CARD_PINCODE,
                                                                                      POWER_CARD_ISSUE_DATE,
                                                                                      POWER_CARD_ISSUING_ID) 
                                                                            VALUES(
                                                                                    {SellerID},
                                                                                    '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                    '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                    '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',
                                                                                    :BlobImage,
                                                                                    {card_series_id},
                                                                                    '{SellerNumberText.Text}',
                                                                                    '{SellerPinCodeText.Text.Trim()}',
                                                                                    TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                    {card_issuing_id},
                                                                                    '{SellerAddressText.Text.Trim()}',
                                                                                    '{SellerRegistrationAddressText.Text}',
                                                                                    {(PowerOfAttorneyCheck.Checked ? 1 : 0)},
                                                                                    '{PowerOfAttorneyNumberText.Text.Trim()}',
                                                                                    '{SellerPowerOfAttorneyNameText.Text.Trim()}',
                                                                                    {power_card_series_id},
                                                                                    '{SellerPowerNumberText.Text}',
                                                                                    '{SellerPowerPinCodeText.Text.Trim()}',
                                                                                    TO_DATE('{SellerPowerIssueDate.Text}','DD/MM/YYYY'),
                                                                                    {power_card_issuing_id})";
                                OracleParameter blobParameter = new OracleParameter();
                                blobParameter.OracleDbType = OracleDbType.Blob;
                                blobParameter.ParameterName = "BlobImage";
                                blobParameter.Value = blob;
                                command.Parameters.Add(blobParameter);
                            }
                        }
                        else
                        {
                            if (old_seller_index == SellerTypeRadioGroup.SelectedIndex)
                                command.CommandText = $@"UPDATE CRS_USER.SELLERS SET SURNAME = '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                     NAME = '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                     PATRONYMIC = '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',                                                                                                
                                                                                     CARD_SERIES_ID = {card_series_id},
                                                                                     CARD_NUMBER = '{SellerNumberText.Text}',
                                                                                     CARD_PINCODE = '{SellerPinCodeText.Text.Trim()}',
                                                                                     CARD_ISSUE_DATE = TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                     CARD_ISSUING_ID = {card_issuing_id},
                                                                                     ADDRESS = '{SellerAddressText.Text.Trim()}',
                                                                                     REGISTRATION_ADDRESS = '{SellerRegistrationAddressText.Text}',
                                                                                     WITH_POWER = {(PowerOfAttorneyCheck.Checked ? 1 : 0)},
                                                                                     POWER_NUMBER = '{PowerOfAttorneyNumberText.Text.Trim()}',
                                                                                     POWER_NAME = '{SellerPowerOfAttorneyNameText.Text.Trim()}',
                                                                                     POWER_CARD_SERIES_ID = {power_card_series_id},
                                                                                     POWER_CARD_NUMBER = '{SellerPowerNumberText.Text}',
                                                                                     POWER_CARD_PINCODE = '{SellerPowerPinCodeText.Text.Trim()}',
                                                                                     POWER_CARD_ISSUE_DATE = TO_DATE('{SellerPowerIssueDate.Text}','DD/MM/YYYY'),
                                                                                     POWER_CARD_ISSUING_ID = {power_card_issuing_id}
                                                                   WHERE ID = {SellerID}";
                            else
                                command.CommandText = $@"INSERT INTO CRS_USER.SELLERS(ID,
                                                                                      SURNAME,
                                                                                      NAME,
                                                                                      PATRONYMIC,                                                                                                            
                                                                                      CARD_SERIES_ID,
                                                                                      CARD_NUMBER,
                                                                                      CARD_PINCODE,
                                                                                      CARD_ISSUE_DATE,
                                                                                      CARD_ISSUING_ID,
                                                                                      ADDRESS,
                                                                                      REGISTRATION_ADDRESS,
                                                                                      WITH_POWER,
                                                                                      POWER_NUMBER,
                                                                                      POWER_NAME,
                                                                                      POWER_CARD_SERIES_ID,
                                                                                      POWER_CARD_NUMBER,
                                                                                      POWER_CARD_PINCODE,
                                                                                      POWER_CARD_ISSUE_DATE,
                                                                                      POWER_CARD_ISSUING_ID) 
                                                                            VALUES({SellerID},
                                                                                    '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                    '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                    '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',                                                                                    
                                                                                    {card_series_id},
                                                                                    '{SellerNumberText.Text}',
                                                                                    '{SellerPinCodeText.Text.Trim()}',
                                                                                    TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                    {card_issuing_id},
                                                                                    '{SellerAddressText.Text.Trim()}',
                                                                                    '{SellerRegistrationAddressText.Text}',
                                                                                    {(PowerOfAttorneyCheck.Checked ? 1 : 0)},
                                                                                    '{PowerOfAttorneyNumberText.Text.Trim()}',
                                                                                    '{SellerPowerOfAttorneyNameText.Text.Trim()}',
                                                                                    {power_card_series_id},
                                                                                    '{SellerPowerNumberText.Text}',
                                                                                    '{SellerPowerPinCodeText.Text.Trim()}',
                                                                                    TO_DATE('{SellerPowerIssueDate.Text}','DD/MM/YYYY'),
                                                                                    {power_card_issuing_id})";
                        }
                    }
                    else
                    {
                        if (old_seller_index == SellerTypeRadioGroup.SelectedIndex)
                            command.CommandText = $@"UPDATE CRS_USER.JURIDICAL_PERSONS SET 
                                                                                            NAME = '{JuridicalPersonNameText.Text.Trim()}',
                                                                                            VOEN = '{JuridicalPersonVoenText.Text.Trim()}',
                                                                                            LEADING_NAME = '{LeadingPersonNameText.Text.Trim()}',
                                                                                            ADDRESS = '{JuridicalPersonAddressText.Text.Trim()}' 
                                                                            WHERE ID = {SellerID}";
                        else
                            command.CommandText = $@"INSERT INTO CRS_USER.JURIDICAL_PERSONS(ID,
                                                                                            NAME,
                                                                                            VOEN,
                                                                                            LEADING_NAME,
                                                                                            ADDRESS) 
                                                           VALUES({SellerID},
                                                                   '{JuridicalPersonNameText.Text.Trim()}',
                                                                   '{JuridicalPersonVoenText.Text.Trim()}',
                                                                   '{LeadingPersonNameText.Text.Trim()}',
                                                                   '{JuridicalPersonAddressText.Text.Trim()}')";
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Satıcının məlumatları bazada dəyişdirilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        void RefreshCustomers(string code)
        {
            RegistrationCodeText.Text = code;
            LoadCustomerDetails();
        }

        private void LoadFCustomerAddEdit(string transaction, string customer_id, string fullname)
        {
            Customer.FCustomerAddEdit fcae = new Customer.FCustomerAddEdit();
            fcae.TransactionName = transaction;
            fcae.CustomerID = customer_id;
            fcae.CustomerFullName = fullname;
            fcae.RefreshCustomersDataGridView += new Customer.FCustomerAddEdit.DoEvent(RefreshCustomers);
            fcae.ShowDialog();
        }

        private void LoadFJuridicalAddEdit(string transaction, string customer_id)
        {
            Customer.FJuridicalPersonAddEdit fcae = new Customer.FJuridicalPersonAddEdit();
            fcae.TransactionName = transaction;
            fcae.CustomerID = customer_id;
            fcae.RefreshCustomersDataGridView += new Customer.FJuridicalPersonAddEdit.DoEvent(RefreshCustomers);
            fcae.ShowDialog();
        }

        private void EditCustomerLabel_Click(object sender, EventArgs e)
        {
            if (customer_type_id == 1)
                LoadFCustomerAddEdit("EDIT", CustomerID, CustomerFullNameText.Text);
            else if (customer_type_id == 2)
                LoadFJuridicalAddEdit("EDIT", CustomerID);
        }

        private void UpdateCardFrontFace()//senedlerin skan formasinin daxil etmek
        {
            string front_format = "jpeg";

            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }

                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    if (!String.IsNullOrEmpty(SellerFrontFaceButtonEdit.Text))
                    {
                        FileStream front_fls = new FileStream(SellerFrontFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                        byte[] front_blob = new byte[front_fls.Length];
                        front_fls.Read(front_blob, 0, System.Convert.ToInt32(front_fls.Length));
                        front_format = Path.GetExtension(SellerFrontFaceButtonEdit.Text);
                        front_fls.Close();
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_FRONT_FACE_IMAGE = :BlobFrontImage, CARD_FRONT_FACE_IMAGE_FORMAT = '{front_format}' WHERE ID = {SellerID}";
                        OracleParameter front_blobParameter = new OracleParameter();
                        front_blobParameter.OracleDbType = OracleDbType.Blob;
                        front_blobParameter.ParameterName = "BlobFrontImage";
                        front_blobParameter.Value = front_blob;
                        command.Parameters.Add(front_blobParameter);
                    }
                    else
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_FRONT_FACE_IMAGE = null WHERE ID = {SellerID}";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Satıcının şəxsiyyətini təsdiq edən sənədin ön üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void UpdateCardRearFace()//senedlerin skan formasinin daxil etmek
        {
            string rear_format = "jpeg";
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }

                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    if (!String.IsNullOrEmpty(SellerRearFaceButtonEdit.Text))
                    {
                        FileStream rear_fls = new FileStream(SellerRearFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                        byte[] rear_blob = new byte[rear_fls.Length];
                        rear_fls.Read(rear_blob, 0, System.Convert.ToInt32(rear_fls.Length));
                        rear_format = Path.GetExtension(SellerRearFaceButtonEdit.Text);
                        rear_fls.Close();
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_REAR_FACE_IMAGE = :BlobRearImage, CARD_REAR_FACE_IMAGE_FORMAT = '{rear_format}' WHERE ID = {SellerID}";
                        OracleParameter rear_blobParameter = new OracleParameter();
                        rear_blobParameter.OracleDbType = OracleDbType.Blob;
                        rear_blobParameter.ParameterName = "BlobRearImage";
                        rear_blobParameter.Value = rear_blob;
                        command.Parameters.Add(rear_blobParameter);
                    }
                    else
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_REAR_FACE_IMAGE = null WHERE ID = {SellerID}";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Satıcının şəxsiyyətini təsdiq edən sənədin arxa üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private bool ControlContractFileDetails()
        {
            bool b = false;

            OtherInfoTabControl.SelectedTabPageIndex = 0;

            if (String.IsNullOrEmpty(PeriodText.Text))
            {
                PeriodText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müqavilənin müddəti təyin edilməyib.");
                PeriodText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;
            return b;
        }

        private void BContract_Click(object sender, EventArgs e)
        {
            if (ControlContractFileDetails())
            {
                if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Müqavilə.docx"))
                {
                    contract_click = false;
                    GlobalVariables.WordDocumentUsed = false;
                    XtraMessageBox.Show("Müqavilənin faylı tapılmadı.");
                    return;
                }
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
                code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
                string qep = null,
                    amount_with_word,
                    phone = null,
                    pobject = null,
                    period = null,
                    //s_bankaccount = null,
                    date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                    //bank_account1 = "",
                    //bank_account2 = "";
                    penaltypercent;

                double d = (double)CreditAmountValue.Value * 100;

                double div = (int)(d / 100);
                int mod = (int)(d % 100), i = 1;
                if (mod > 0)
                {
                    if (currency_id == 1)
                        qep = " " + mod.ToString() + " qəpik";
                    else
                        qep = " " + mod.ToString();
                }

                amount_with_word = " (" + GlobalFunctions.IntegerToWritten(div) + ") " + credit_currency_name + qep;

                if (credit_name_id == 1)
                    pobject = BrandComboBox.Text + " " + ModelComboBox.Text + " - " + TypeComboBox.Text + ", Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text + " avtomobilini";
                else
                    pobject = "Ünvanı: " + ObjectAddressText.Text.Trim() + ", Çıxarışı: " + ObjectExcerptText.Text.Trim() + ", Sahəsi: " + ObjectAreaValue.Value.ToString() + " m² olan daşınmaz əmlakı";

                if (EndDateCheckEdit.Checked)
                    period = ContractEndDate.Text + " tarixinə qədər";
                else
                    period = PeriodText.Text + " (" + GlobalFunctions.IntegerToWritten(Convert.ToInt32(PeriodText.Text)) + ") ay";

                phone = GlobalFunctions.GetName($@"SELECT PHONE FROM CRS_USER.V_PHONE WHERE OWNER_TYPE = '{person_description}' AND OWNER_ID = {CustomerID}");

                penaltypercent = GlobalFunctions.GetAmount($@"SELECT INTEREST FROM CRS_USER.CONTRACT_INTEREST_PENALTIES WHERE CONTRACT_ID = {ContractID} AND ROWNUM = 1").ToString("n2");

                //try
                //{
                //    s_bankaccount = $@"SELECT B.LONG_NAME || ', HESAB:' || B.ACCOUNT BANK_ACCOUNT
                //                          FROM CRS_USER.BANKS B
                //                         WHERE B.IS_USED = 1";

                //    foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s_bankaccount, this.Name + "/BContract_Click").Rows)
                //    {
                //        if (i > 2)
                //            continue;

                //        if (i == 1)
                //            bank_account1 = dr[0].ToString();
                //        if (i == 2)
                //            bank_account2 = dr[0].ToString();
                //        i++;
                //    }
                //}
                //catch (Exception exx)
                //{
                //    GlobalProcedures.LogWrite("Bank hesabları tapılmadı.", s_bankaccount, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                //}

                try
                {
                    string voen = "",
                           filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Müqavilə.docx";
                    object missing = System.Reflection.Missing.Value;

                    Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                    Microsoft.Office.Interop.Word.Document aDoc = null;

                    object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Müqavilə.docx"),
                       saveAs = Path.Combine(filePath);

                    object readOnly = false;
                    object isVisible = false;
                    wordApp.Visible = false;

                    aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                    aDoc.Activate();
                    GlobalProcedures.FindAndReplace(wordApp, "[$code]", ContractCodeText.Text);
                    GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                    GlobalProcedures.FindAndReplace(wordApp, "[$object]", pobject);
                    if (customer_type_id == 1)
                    {
                        GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixdə " + IssuingText.Text + " tərəfindən verilib)");
                        GlobalProcedures.FindAndReplace(wordApp, "[$sv]", CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixdə " + IssuingText.Text + " tərəfindən verilib");
                        voen = (String.IsNullOrWhiteSpace(VoenText.Text)) ? "" : "Vöen : " + VoenText.Text.Trim();
                    }
                    else
                    {
                        GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ")");
                        GlobalProcedures.FindAndReplace(wordApp, "[$sv]", CardDescriptionText.Text);
                    }

                    GlobalProcedures.FindAndReplace(wordApp, "[$customer1]", CustomerFullNameText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$address]", "Ünvan:" + RegistrationAddressText.Text.Trim());
                    GlobalProcedures.FindAndReplace(wordApp, "[$payment]", CreditAmountValue.Value.ToString("N2") + amount_with_word);
                    GlobalProcedures.FindAndReplace(wordApp, "[$phone]", "Telefon:" + phone);
                    GlobalProcedures.FindAndReplace(wordApp, "[$period]", period);
                    GlobalProcedures.FindAndReplace(wordApp, "[$penaltypercent]", penaltypercent);
                    GlobalProcedures.FindAndReplace(wordApp, "[$voen]", voen);

                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Close(ref missing, ref missing, ref missing);

                    GlobalVariables.WordDocumentUsed = true;
                    contract_click = true;
                    Process.Start(filePath);
                }
                catch
                {
                    GlobalProcedures.SplashScreenClose();
                    XtraMessageBox.Show(code_number + "_Müqavilə.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
                }
                finally
                {
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_MODELS", "NAME", "ID", ModelComboBox.Text);
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(ContractStartDate.Text);
            GenerateCurrencyRateLabel();
            CreditAmountValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void ModelComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadHostageDictionaries("E", 8, 3);
        }

        private void ContractEndDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ContractStartDate.Text) && !String.IsNullOrEmpty(ContractEndDate.Text) && !String.IsNullOrEmpty(InterestText.Text) && OtherInfoTabControl.Enabled && Convert.ToDouble(InterestText.Text) > 0)
            {
                int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
                DifferenceDateLabel.Text = diff_month.ToString() + " ay";
                PeriodText.Text = diff_month.ToString();
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));
                MonthlyPaymentValue.Value = (decimal)monthly_amount;
            }
        }

        private void GracePeriodValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
            if (!EndDateCheckEdit.Checked)
            {
                if (PaymentTypeRadioGroup.SelectedIndex == 0)
                {
                    monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, Convert.ToDouble(PeriodText.Text) - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));
                    MonthlyPaymentValue.Value = (decimal)monthly_amount;
                }
                else
                    monthly_amount = (double)MonthlyPaymentValue.Value;
            }
            else
            {
                if (PaymentTypeRadioGroup.SelectedIndex == 0)
                {
                    monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));
                    MonthlyPaymentValue.Value = (decimal)monthly_amount;
                }
                else
                    monthly_amount = (double)MonthlyPaymentValue.Value;
            }
        }

        private bool ControlInsureDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(BanText.Text))
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                BanText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ban daxil edilməyib.");
                BanText.Focus();
                BanText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(CarNumberText.Text))
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CarNumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qeydiyyat nişanı daxil edilməyib.");
                CarNumberText.Focus();
                CarNumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (currency_id == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LiquidCurrencyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta seçilməyib.");
                LiquidCurrencyLookUp.Focus();
                LiquidCurrencyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private bool ControlPowerOfAttorneyParametrs()
        {
            bool b = false;
            if (BanText.Text.Length == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                BanText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ban daxil edilməyib.");
                BanText.Focus();
                BanText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CarNumberText.Text.Length == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CarNumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qeydiyyat nişanı daxil edilməyib.");
                CarNumberText.Focus();
                CarNumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BLeasingObjectContract_Click(object sender, EventArgs e)
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Alqı-satqı.docx"))
            {
                leasing_object_contract_click = false;
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Alqı-satqı müqaviləsinin faylı tapılmadı.");
                return;
            }

            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
            OtherInfoTabControl.SelectedTabPageIndex = 1;

            if (!sellerdetails)
            {
                SellerType();
                LoadSellerDetails();
                InsertPhonesTemp();
            }

            string qep = null,
                amount_with_word,
                phone = null,
                pobject = null,
                card = null,
                seller_name = null,
                customer = null,
                customer1 = null,
                address = null,
                sv = null,
                date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime);

            bool control = false;

            if (SellerTypeRadioGroup.SelectedIndex == 0 && (!String.IsNullOrEmpty(SellerSurnameText.Text.Trim())) && (!String.IsNullOrEmpty(SellerNameText.Text.Trim())) && (!String.IsNullOrEmpty(SellerPatronymicText.Text.Trim())))
            {
                seller_name = SellerSurnameText.Text.Trim() + " " + SellerNameText.Text.Trim() + " " + SellerPatronymicText.Text.Trim();
                
                if (!PowerOfAttorneyCheck.Checked)
                    card = card_series_id != 0 && SellerNumberText.Text != string.Empty && SellerIssueDate.Text != string.Empty && card_issuing_id != 0 ? " " + SellerSeriesLookUp.Text + ":" + SellerNumberText.Text + (!String.IsNullOrEmpty(SellerPinCodeText.Text) ? ", FİN:" + SellerPinCodeText.Text.Trim() : null) + ", " + GlobalFunctions.DateWithYear(SellerIssueDate.DateTime) + " tarixində " + SellerIssuingLookUp.Text + " tərəfindən verilib" : null;
                else
                    card = "Etibarnamə №: " + PowerOfAttorneyNumberText.Text.Trim() +
                                                (SellerPowerOfAttorneyNameText.Text != string.Empty ? " əsasında çıxış edir: " + SellerPowerOfAttorneyNameText.Text.Trim() +
                                                (power_card_series_id != 0 && SellerPowerNumberText.Text != string.Empty && SellerPowerIssueDate.Text != string.Empty && power_card_issuing_id != 0 ? " " + SellerPowerSeriesLookUp.Text + ":" + SellerPowerNumberText.Text.Trim() + ", " + GlobalFunctions.DateWithYear(SellerPowerIssueDate.DateTime) + " tarixində " + SellerPowerIssuingLookUp.Text + " tərəfindən verilib" : null) : null);

                customer = seller_name + " (" + card + ")";
                customer1 = seller_name;
                address = "Ünvan:" + SellerRegistrationAddressText.Text.Trim();
                sv = card.Trim();
                control = true;
            }
            else if (SellerTypeRadioGroup.SelectedIndex == 1 && !String.IsNullOrEmpty(JuridicalPersonNameText.Text) && !String.IsNullOrEmpty(JuridicalPersonVoenText.Text) && !String.IsNullOrEmpty(JuridicalPersonAddressText.Text) && !String.IsNullOrEmpty(LeadingPersonNameText.Text))
            {
                seller_name = JuridicalPersonNameText.Text;
                customer = seller_name + " (VÖEN : " + JuridicalPersonVoenText.Text.Trim() + ")";
                customer1 = seller_name;
                address = "Ünvan:" + JuridicalPersonAddressText.Text.Trim();
                sv = "VÖEN:" + JuridicalPersonVoenText.Text.Trim();
                control = true;
            }
            else
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                XtraMessageBox.Show("Satıcının bütün məlumatları daxil edilmədiyi üçün Alqı-satqı faylını yaratmaq olmaz.");
                control = false;
            }

            if (control)
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
                double d = (double)LiquidValue.Value * 100;

                int div = (int)(d / 100), mod = (int)(d % 100), i = 1;
                if (mod > 0)
                    qep = " " + mod.ToString() + " " + currency_small_name;


                amount_with_word = " (" + GlobalFunctions.IntegerToWritten(div) + ") " + currency_name + qep;

                if (credit_name_id == 1)
                    pobject = BrandComboBox.Text + " " + ModelComboBox.Text + " - " + TypeComboBox.Text + ", Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text;
                else
                    pobject = "Ünvanı: " + ObjectAddressText.Text.Trim() + ", Çıxarışı: " + ObjectExcerptText.Text.Trim() + ", Sahəsi: " + ObjectAreaValue.Value.ToString() + " m²";

                phone = GlobalFunctions.GetName($@"SELECT PHONE FROM CRS_USER_TEMP.V_PHONE_TEMP WHERE OWNER_TYPE = '{seller_type}' AND OWNER_ID = {SellerID}");
                               

                try
                {
                    string voen = "",
                           filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Alqı-satqı.docx";
                    object missing = System.Reflection.Missing.Value;

                    Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                    Microsoft.Office.Interop.Word.Document aDoc = null;

                    object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Alqı-satqı.docx"),
                       saveAs = Path.Combine(filePath);

                    object readOnly = false;
                    object isVisible = false;
                    wordApp.Visible = false;

                    aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                    aDoc.Activate();
                    GlobalProcedures.FindAndReplace(wordApp, "[$code]", ContractCodeText.Text);
                    GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                    GlobalProcedures.FindAndReplace(wordApp, "[$object]", pobject);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", customer);
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer1]", customer1);
                    GlobalProcedures.FindAndReplace(wordApp, "[$address]", address);
                    GlobalProcedures.FindAndReplace(wordApp, "[$sv]", sv);
                    GlobalProcedures.FindAndReplace(wordApp, "[$amount]", LiquidValue.Value.ToString("N2") + amount_with_word);
                    GlobalProcedures.FindAndReplace(wordApp, "[$phone]", "Telefon:" + phone);
                    GlobalProcedures.FindAndReplace(wordApp, "[$period]", period);
                    GlobalProcedures.FindAndReplace(wordApp, "[$voen]", voen);

                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Close(ref missing, ref missing, ref missing);

                    GlobalVariables.WordDocumentUsed = true;
                    leasing_object_contract_click = true;
                    Process.Start(filePath);
                }
                catch
                {
                    GlobalProcedures.SplashScreenClose();
                    XtraMessageBox.Show(code_number + "_Alqı-satqı.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
                }
                finally
                {
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }

        void RefreshCommitments()
        {
            LoadCommitmentDataGridView();
        }

        private void LoadFCommitment(string transaction, string commitmentid)
        {
            string phone = null, pobject, letterHostage = null, customername = null, customeraddress = null, customercard = null, voen = null, ownerType = (customer_type_id == 1) ? "C" : "JP", endDate = ContractEndDate.Text;
            int commitment_max_id = 0, com_parent_id, parentPersonTypeID = 1;
            DateTime AgreementDateMinValue = DateTime.Today;
            List<PhonesView> lstCommitmentPhones = PhonesViewDAL.SelectPhone(null, "CC").ToList<PhonesView>();
            List<PhonesView> lstCustomerPhones = PhonesViewDAL.SelectPhone(int.Parse(CustomerID), ownerType).ToList<PhonesView>();
            List<ContractCommitment> lstContractCommitment = CommitmentsDAL.SelectAllCommitmentTempByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();
            if (lstContractCommitment.Count > 0)
            {
                AgreementDateMinValue = lstContractCommitment.Max(c => c.AGREEMENTDATE);
                if (transaction == "INSERT")
                {
                    commitment_max_id = lstContractCommitment.Max(c => c.ID);
                    var last_commitment = lstContractCommitment.LastOrDefault();
                    customername = last_commitment.COMMITMENT_NAME;
                    customeraddress = last_commitment.ADDRESS;
                    customercard = last_commitment.CARD;
                    parentPersonTypeID = last_commitment.PERSON_TYPE_ID;
                    voen = last_commitment.VOEN;
                    var commitment_phone = lstCommitmentPhones.Find(p => p.OWNER_ID == last_commitment.ID);
                    if (commitment_phone != null)
                        phone = commitment_phone.PHONE;
                }
                else
                {
                    var parent_commitments = lstContractCommitment.Find(c => c.ID == int.Parse(commitmentid));
                    com_parent_id = parent_commitments.PARENT_ID;
                    if (com_parent_id > 0)
                    {
                        var last_commitment = lstContractCommitment.Find(c => c.ID == com_parent_id);
                        customername = last_commitment.COMMITMENT_NAME;
                        customeraddress = last_commitment.ADDRESS;
                        parentPersonTypeID = last_commitment.PERSON_TYPE_ID;
                        voen = last_commitment.VOEN;
                        var commitment_phone = lstCommitmentPhones.Find(p => p.OWNER_ID == com_parent_id);
                        if (commitment_phone != null)
                            phone = commitment_phone.PHONE;
                        customercard = last_commitment.CARD;
                    }
                    else
                    {
                        customername = CustomerFullNameText.Text.Trim();
                        if (lstCustomerPhones.Count > 0)
                            phone = lstCustomerPhones.First().PHONE;
                        customeraddress = RegistrationAddressText.Text.Trim();
                        customercard = CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixdə " + IssuingText.Text + " tərəfindən verilib";
                        parentPersonTypeID = customer_type_id;
                        voen = VoenText.Text.Trim();
                    }
                }
            }
            else
            {
                customername = CustomerFullNameText.Text.Trim();
                AgreementDateMinValue = ContractStartDate.DateTime;
                if (lstCustomerPhones.Count > 0)
                    phone = lstCustomerPhones.First().PHONE;
                customeraddress = RegistrationAddressText.Text.Trim();
                customercard = CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixdə " + IssuingText.Text + " tərəfindən verilib";
                parentPersonTypeID = customer_type_id;
                voen = VoenText.Text.Trim();
            }

            if (credit_name_id == 1)
            {
                pobject = BrandComboBox.Text + " " + ModelComboBox.Text + " - " + TypeComboBox.Text + ", Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text;
                letterHostage = BrandComboBox.Text + " " + ModelComboBox.Text + " markalı (Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text + ")";
            }
            else
                pobject = "Ünvanı: " + ObjectAddressText.Text.Trim() + ", Çıxarışı: " + ObjectExcerptText.Text.Trim() + ", Sahəsi: " + ObjectAreaValue.Value.ToString() + " m²";

            if (IsExtend)
            {
                List<ContractExtend> lstContractEntend = ContractExtendDAL.SelectContractExtend(0, int.Parse(ContractID)).ToList<ContractExtend>();
                if (lstContractEntend.Count > 0)
                    endDate = lstContractEntend.LastOrDefault().END_DATE.ToString("dd.MM.yyyy");
            }

            topindex = CommitmentsGridView.TopRowIndex;
            old_row_num = CommitmentsGridView.FocusedRowHandle;
            FCommitmentTransmission fct = new FCommitmentTransmission();
            fct.TransactionName = transaction;
            fct.CommitmentID = commitmentid;
            fct.ContractCode = ContractCodeText.Text.Trim();
            fct.Customer = customername + " (" + customercard + ")";
            fct.CustomerAddress = customeraddress;
            fct.CustomerCard = customercard;
            fct.CustomerCode = RegistrationCodeText.Text.Trim();
            fct.Account = CustomerAccountText.Text.Trim();
            fct.CustomerPhone = phone;
            fct.CustomerName = customername;
            fct.ContractEndDate = endDate;
            fct.ContractStartDate = ContractStartDate.Text;
            fct.Interest = Convert.ToInt32(InterestText.Text);
            fct.Period = Convert.ToInt32(PeriodText.Text);
            fct.Lizinq = pobject;
            fct.Amount = CreditAmountValue.Value;
            fct.Currency = credit_currency_name;
            fct.DebtCurrency = CreditCurrencyLookUp.Text;
            fct.DebtCurrencyID = credit_currency_id;
            fct.CustomerID = CustomerID;
            fct.ContractID = ContractID;
            fct.Debt = CreditAmountValue.Value;
            fct.CreditNameID = credit_name_id;
            fct.CreditName = CreditNameLookUp.Text;
            fct.LiquidValue = LiquidValue.Value;
            fct.LiquidCurrencyID = currency_id;
            fct.AgreementDateMinValue = AgreementDateMinValue;
            fct.Count = 1;
            fct.ParentID = commitment_max_id;
            fct.ParentPersonTypeID = parentPersonTypeID;
            fct.CustomerTypeID = customer_type_id;
            fct.Voen = voen;
            fct.CarNumber = CarNumberText.Text.Trim();
            fct.LetterHostage = letterHostage;
            fct.RefreshCommitmentsDataGridView += new FCommitmentTransmission.DoEvent(RefreshCommitments);
            fct.ShowDialog();
            CommitmentsGridView.TopRowIndex = topindex;
            CommitmentsGridView.FocusedRowHandle = old_row_num;
        }

        private void LoadFJuridicalCommitment(string transaction, string commitmentid)
        {
            string phone = null, pobject, customername = null, letterHostage = null, customeraddress = null, customercard = null, voen = null, ownerType = (customer_type_id == 1) ? "C" : "JP", endDate = ContractEndDate.Text;
            int commitment_max_id = 0, com_parent_id, parentPersonTypeID = 2;
            DateTime AgreementDateMinValue = DateTime.Today;
            List<PhonesView> lstCommitmentPhones = PhonesViewDAL.SelectPhone(null, "CC").ToList<PhonesView>();
            List<PhonesView> lstCustomerPhones = PhonesViewDAL.SelectPhone(int.Parse(CustomerID), ownerType).ToList<PhonesView>();
            List<ContractCommitment> lstContractCommitment = CommitmentsDAL.SelectAllCommitmentTempByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();

            if (lstContractCommitment.Count > 0)
            {
                AgreementDateMinValue = lstContractCommitment.Max(c => c.AGREEMENTDATE);
                if (transaction == "INSERT")
                {
                    var last_commitment = lstContractCommitment.LastOrDefault();
                    commitment_max_id = last_commitment.ID;
                    customername = last_commitment.COMMITMENT_NAME;
                    customeraddress = last_commitment.ADDRESS;
                    customercard = last_commitment.CARD;
                    parentPersonTypeID = last_commitment.PERSON_TYPE_ID;
                    voen = last_commitment.VOEN;
                    var commitment_phone = lstCommitmentPhones.Find(p => p.OWNER_ID == last_commitment.ID);
                    if (commitment_phone != null)
                        phone = commitment_phone.PHONE;
                }
                else
                {
                    var parent_commitments = lstContractCommitment.Find(c => c.ID == int.Parse(commitmentid));
                    com_parent_id = parent_commitments.PARENT_ID;
                    if (com_parent_id > 0)
                    {
                        var last_commitment = lstContractCommitment.Find(c => c.ID == com_parent_id);
                        customername = last_commitment.COMMITMENT_NAME;
                        customeraddress = last_commitment.ADDRESS;
                        parentPersonTypeID = last_commitment.PERSON_TYPE_ID;
                        voen = last_commitment.VOEN;
                        var commitment_phone = lstCommitmentPhones.Find(p => p.OWNER_ID == com_parent_id);
                        if (commitment_phone != null)
                            phone = commitment_phone.PHONE;
                        customercard = last_commitment.CARD;
                    }
                    else
                    {
                        customername = CustomerFullNameText.Text.Trim();
                        if (lstCustomerPhones.Count > 0)
                            phone = lstCustomerPhones.First().PHONE;
                        customeraddress = RegistrationAddressText.Text.Trim();
                        customercard = CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixdə " + IssuingText.Text + " tərəfindən verilib";
                        parentPersonTypeID = customer_type_id;
                        voen = VoenText.Text.Trim();
                    }
                }
            }
            else
            {
                customername = CustomerFullNameText.Text.Trim();
                AgreementDateMinValue = ContractStartDate.DateTime;
                if (lstCustomerPhones.Count > 0)
                    phone = lstCustomerPhones.First().PHONE;
                customeraddress = RegistrationAddressText.Text.Trim();
                customercard = CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixdə " + IssuingText.Text + " tərəfindən verilib";
                parentPersonTypeID = customer_type_id;
                voen = VoenText.Text.Trim();
            }

            if (credit_name_id == 1)
            {
                pobject = BrandComboBox.Text + " " + ModelComboBox.Text + " - " + TypeComboBox.Text + ", Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text;
                letterHostage = BrandComboBox.Text + " " + ModelComboBox.Text + " markalı (Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text + ")";
            }
            else
                pobject = "Ünvanı: " + ObjectAddressText.Text.Trim() + ", Çıxarışı: " + ObjectExcerptText.Text.Trim() + ", Sahəsi: " + ObjectAreaValue.Value.ToString() + " m²";

            if (IsExtend)
            {
                List<ContractExtend> lstContractEntend = ContractExtendDAL.SelectContractExtend(0, int.Parse(ContractID)).ToList<ContractExtend>();
                if (lstContractEntend.Count > 0)
                    endDate = lstContractEntend.LastOrDefault().END_DATE.ToString("dd.MM.yyyy");
            }

            topindex = CommitmentsGridView.TopRowIndex;
            old_row_num = CommitmentsGridView.FocusedRowHandle;
            FJuridicalCommitment fct = new FJuridicalCommitment();
            fct.TransactionName = transaction;
            fct.CommitmentID = commitmentid;
            fct.ContractCode = ContractCodeText.Text.Trim();
            fct.Customer = customername + " (" + customercard + ")";
            fct.CustomerAddress = customeraddress;
            fct.CustomerCard = customercard;
            fct.CustomerCode = RegistrationCodeText.Text.Trim();
            fct.Account = CustomerAccountText.Text.Trim();
            fct.CustomerPhone = phone;
            fct.CustomerName = customername;
            fct.ContractEndDate = endDate;
            fct.ContractStartDate = ContractStartDate.Text;
            fct.AgreementDateMinValue = AgreementDateMinValue;
            fct.Interest = Convert.ToInt32(InterestText.Text);
            fct.Period = Convert.ToInt32(PeriodText.Text);
            fct.Lizinq = pobject;
            fct.Amount = CreditAmountValue.Value;
            fct.Currency = credit_currency_name;
            fct.DebtCurrency = CreditCurrencyLookUp.Text;
            fct.DebtCurrencyID = credit_currency_id;
            fct.CustomerID = CustomerID;
            fct.ContractID = ContractID;
            fct.Debt = CreditAmountValue.Value;
            fct.CreditNameID = credit_name_id;
            fct.CreditName = CreditNameLookUp.Text;
            fct.LiquidValue = LiquidValue.Value;
            fct.LiquidCurrencyID = currency_id;
            fct.Count = 1;
            fct.ParentID = commitment_max_id;
            fct.CustomerTypeID = customer_type_id;
            fct.ParentPersonTypeID = parentPersonTypeID;
            fct.Voen = voen;
            fct.CarNumber = CarNumberText.Text.Trim();
            fct.LetterHostage = letterHostage;
            fct.RefreshCommitmentsDataGridView += new FJuridicalCommitment.DoEvent(RefreshCommitments);
            fct.ShowDialog();
            CommitmentsGridView.TopRowIndex = topindex;
            CommitmentsGridView.FocusedRowHandle = old_row_num;
        }

        private void BDocument_Click(object sender, EventArgs e)
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Daimi sənədlər.docx"))
            {
                leasing_object_contract_click = false;
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Daimi sənədlərin şablon faylı tapılmadı.");
                return;
            }
            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
            OtherInfoTabControl.SelectedTabPageIndex = 1;

            if (!sellerdetails)
            {
                SellerType();
                LoadSellerDetails();
                InsertPhonesTemp();
            }

            if (seller_type_id == 1 && (String.IsNullOrEmpty(SellerSurnameText.Text.Trim()) || String.IsNullOrEmpty(SellerNameText.Text.Trim()) || String.IsNullOrEmpty(SellerPatronymicText.Text.Trim())))
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                XtraMessageBox.Show("Satıcının bütün məlumatları daxil edilmədiyi üçün Daimi Sənədlər faylını yaratmaq olmaz.");
                return;
            }
            else if (seller_type_id == 2 && (String.IsNullOrEmpty(JuridicalPersonNameText.Text.Trim()) || String.IsNullOrEmpty(JuridicalPersonAddressText.Text.Trim()) || String.IsNullOrEmpty(JuridicalPersonVoenText.Text.Trim()) || String.IsNullOrEmpty(LeadingPersonNameText.Text.Trim())))
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                XtraMessageBox.Show("Satıcının bütün məlumatları daxil edilmədiyi üçün Daimi Sənədlər faylını yaratmaq olmaz.");
                return;
            }


            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

            string amount_with_word,
                amount_with_word2,
                qep = null,
                qep2 = null,
                object_desc = null,
                pobject = null,
                date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                seller_name = null,
                sellerwithcard = null,
                customer_name = null,
                card = null,
                personType = (customer_type_id == 1) ? "fiziki şəxs" : "hüquqi şəxs";

            if (seller_type_id == 1)
            {
                if (!PowerOfAttorneyCheck.Checked)
                    card = card_series_id != 0 && SellerNumberText.Text != string.Empty && SellerIssueDate.Text != string.Empty && card_issuing_id != 0 ? " " + SellerSeriesLookUp.Text + ":" + SellerNumberText.Text + (!String.IsNullOrEmpty(SellerPinCodeText.Text) ? ", FİN:" + SellerPinCodeText.Text.Trim() : null) + ", " + GlobalFunctions.DateWithYear(SellerIssueDate.DateTime) + " tarixində " + SellerIssuingLookUp.Text + " tərəfindən verilib" : null;
                else
                    card = "Etibarnamə №: " + PowerOfAttorneyNumberText.Text.Trim() +
                                                (SellerPowerOfAttorneyNameText.Text != string.Empty ? " əsasında çıxış edir: " + SellerPowerOfAttorneyNameText.Text.Trim() +
                                                (power_card_series_id != 0 && SellerPowerNumberText.Text != string.Empty && SellerPowerIssueDate.Text != string.Empty && power_card_issuing_id != 0 ? " " + SellerPowerSeriesLookUp.Text + ":" + SellerPowerNumberText.Text.Trim() + ", " + GlobalFunctions.DateWithYear(SellerPowerIssueDate.DateTime) + " tarixində " + SellerPowerIssuingLookUp.Text + " tərəfindən verilib" : null) : null);

                seller_name = SellerSurnameText.Text.Trim() + " " + SellerNameText.Text.Trim() + " " + SellerPatronymicText.Text.Trim();
                sellerwithcard = seller_name + " (" + card + ")";
            }
            else
            {
                seller_name = JuridicalPersonNameText.Text.Trim();
                sellerwithcard = seller_name + " (VÖEN : " + JuridicalPersonVoenText.Text.Trim() + ")";
            }

            if (credit_name_id == 1)
                object_desc = "Marka, Model, Tip, Buraxılış ili, Rəngi, BAN";
            else if (credit_name_id == 5)
                object_desc = "Ünvan, çıxarış, sahəsi";

            double d = (double)LiquidValue.Value * 100;

            int div = (int)(d / 100), mod = (int)(d % 100);
            if (mod > 0)
                qep = " " + mod.ToString() + " " + currency_small_name;

            amount_with_word = " (" + GlobalFunctions.IntegerToWritten(div) + ") " + currency_name + qep;

            if (credit_name_id == 1)
                pobject = BrandComboBox.Text + " " + ModelComboBox.Text + " - " + TypeComboBox.Text + ", Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text;
            else
                pobject = "Ünvanı: " + ObjectAddressText.Text.Trim() + ", Çıxarışı: " + ObjectExcerptText.Text.Trim() + ", Sahəsi: " + ObjectAreaValue.Value.ToString() + " m²";
            double d2 = (double)CreditAmountValue.Value * 100;

            int div2 = (int)(d2 / 100), mod2 = (int)(d2 % 100);
            if (mod2 > 0)
                qep2 = " " + mod2.ToString() + " " + currency_small_name;

            amount_with_word2 = " (" + GlobalFunctions.IntegerToWritten(div2) + ") " + currency_name + qep2;

            if (customer_type_id == 1)
                customer_name = CustomerFullNameText.Text.Trim() + " (" + CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixdə " + IssuingText.Text + ((String.IsNullOrWhiteSpace(VoenText.Text)) ? " tərəfindən verilib)" : " tərəfindən verilib, VÖEN: " + VoenText.Text.Trim() + ")");
            else
                customer_name = CustomerFullNameText.Text.Trim() + " (" + CardDescriptionText.Text + ")";

            try
            {
                string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daimi_Sənədlər.docx";
                object missing = System.Reflection.Missing.Value;

                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Daimi sənədlər.docx"),
                   saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();
                GlobalProcedures.FindAndReplace(wordApp, "[$code]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$seller]", sellerwithcard);
                GlobalProcedures.FindAndReplace(wordApp, "[$object]", pobject);
                GlobalProcedures.FindAndReplace(wordApp, "[$objectdesc]", object_desc);
                GlobalProcedures.FindAndReplace(wordApp, "[$customer]", customer_name);
                GlobalProcedures.FindAndReplace(wordApp, "[$customer1]", CustomerFullNameText.Text.Trim());
                GlobalProcedures.FindAndReplace(wordApp, "[$seller1]", seller_name);
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", LiquidValue.Value.ToString("N2") + " " + currency_name);
                GlobalProcedures.FindAndReplace(wordApp, "[$amountwithwrite]", LiquidValue.Value.ToString("N2") + amount_with_word);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                GlobalVariables.WordDocumentUsed = true;
                leasing_object_contract_click = true;
                Process.Start(filePath);
            }
            catch
            {
                GlobalProcedures.SplashScreenClose();
                XtraMessageBox.Show(code_number + "_Daimi_Sənədlər.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void BLoadContractPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Şəkili seçin";
                dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ContractImageSlider.Images.Add(Image.FromFile(dlg.FileName));
                    image_list.Add(dlg.FileName);
                    BDeleteContractPicture.Enabled = !CurrentStatus;
                    BChangeContractPicture.Enabled = !CurrentStatus;
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkil";
                            break;
                        case "EN":
                            ImageCountLabel.Text = ContractImageSlider.Images.Count + " picture";
                            break;
                        case "RU":
                            ImageCountLabel.Text = ContractImageSlider.Images.Count + " картина";
                            break;
                    }
                }
                dlg.Dispose();
            }
        }

        void RefreshInsurance()
        {
            LoadInsuranceDataGridView();
        }

        private void LoadFInsuranceAddEdit(string transaction, string contractid, string insurancetid, string currencyid, string currencycode)
        {
            topindex = InsuranceGridView.TopRowIndex;
            old_row_num = InsuranceGridView.FocusedRowHandle;
            FInsuranceAddEdit fiae = new FInsuranceAddEdit();
            fiae.TransactionName = transaction;
            fiae.ContractID = contractid;
            fiae.InsuranceID = insurancetid;
            fiae.CurrencyID = currencyid;
            fiae.CurrencyCode = currencycode;
            fiae.CarAmount = LiquidValue.Value;
            fiae.ContractStartDate = ContractStartDate.DateTime;
            fiae.IsAgain = InsuranceGridView.RowCount;
            fiae.RefreshCompanyDataGridView += new FInsuranceAddEdit.DoEvent(RefreshInsurance);
            fiae.ShowDialog();
            InsuranceGridView.TopRowIndex = topindex;
            InsuranceGridView.FocusedRowHandle = old_row_num;
        }

        private void NewInsuranceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ControlInsureDetails())
                LoadFInsuranceAddEdit("INSERT", ContractID, null, currency_id.ToString(), LiquidCurrencyLookUp.Text);
        }

        private void PrintInsuranceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InsuranceGridControl);
        }

        private void CreditCurrencyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CreditCurrencyLookUp.EditValue == null)
                return;

            credit_currency_id = Convert.ToInt32(CreditCurrencyLookUp.EditValue);

            var currency = lstCurrency.Find(c => c.ID == credit_currency_id);
            if (currency == null)
                return;

            credit_currency_name = currency.NAME;
            currency_short_name = currency.SHORT_NAME;
            credit_currency_value = currency.VALUE;
            rate = 0;
            MonthlyPaymentCurrencyLabel.Text = CreditCurrencyLookUp.Text;
            UpdateCurrencyPaymentScheduleTemp();
            CreateAccounts();
            GenerateCurrencyRateLabel();
            CreditAmountValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void LiquidCurrencyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (LiquidCurrencyLookUp.EditValue == null)
                return;

            currency_id = Convert.ToInt32(LiquidCurrencyLookUp.EditValue);
            var currency = lstCurrency.Find(c => c.ID == currency_id);
            currency_name = currency.NAME;
            currency_small_name = currency.SMALL_NAME;
            InitialPaymentCurrencyLabel.Text = LiquidCurrencyLookUp.Text;
            GenerateCurrencyRateLabel();
            if (FormStatus)
                InitialPaymentValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void CreditNameLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                LoadDictionaries("E", 5);
                CreditTypeParametr();
                if (!String.IsNullOrEmpty(PeriodText.Text) && !String.IsNullOrEmpty(InterestText.Text))
                {
                    monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, Convert.ToDouble(PeriodText.Text), Convert.ToDouble(InterestText.Text));
                    MonthlyPaymentValue.Value = (decimal)monthly_amount;
                }
            }
        }

        private void CreditNameLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CreditNameLookUp.EditValue == null)
                return;

            credit_name_id = Convert.ToInt32(CreditNameLookUp.EditValue);

            if (TransactionName == "INSERT")
                BChangeCode.Visible = GlobalVariables.EditContractCode;

            HostageScrollableControl.Visible = (credit_name_id != 0);
            CreditTypeParametr();

            if (TransactionName == "INSERT")
                VisibleHostageDetails(credit_name_id);

            if ((FormStatus) && (CreditAmountValue.Value > 0) && (credit_type_id > 0))
            {
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, Convert.ToDouble(PeriodText.Text), Convert.ToDouble(InterestText.Text));
                MonthlyPaymentValue.Value = (decimal)monthly_amount;
            }
            else
            {
                MonthlyPaymentValue.Value = 0;
                monthly_amount = 0;
            }

            ContractEndDate_EditValueChanged(sender, EventArgs.Empty);

            if (FormStatus)
            {
                ContractCodeText.Text = InsertContractCodeTemp();
                CreateAccounts();
            }
        }

        private void CreditCurrencyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 7);
        }

        private void PowerGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            int isRevoke = Convert.ToInt32(PowerGridView.GetListSourceRowCellValue(rowIndex, "IS_REVOKE"));
            if (e.Column == Power_Status && isRevoke == 0)
                e.Value = Properties.Resources.ok_16;
            else if (e.Column == Power_Status && isRevoke == 1)
                e.Value = Properties.Resources.cancel_16;

            int isResponsible = Convert.ToInt32(PowerGridView.GetListSourceRowCellValue(rowIndex, "IS_RESPONSIBLE"));
            if (e.Column == Power_Responsibile && isResponsible == 1)
                e.Value = Properties.Resources.status_blue_16;
            else if (e.Column == Power_Responsibile && isResponsible == 2)
                e.Value = Properties.Resources.status_offline_16;
        }

        private void AgainPowerOfAttorneyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPowerOfAttorney("INSERT", PowerID);
        }

        private void PrintPowerOfAttorneyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PowerGridControl);
        }

        private void PowerGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ExportPowerOfAttorneyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PowerGridControl, "xls");
        }

        private void ExportInsuranceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InsuranceGridControl, "xls");
        }

        private void InsuranceGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditInsuranceBarButton.Enabled = DeleteInsuranceBarButton.Enabled = ViewInsuranceFileBarButton.Enabled = (InsuranceGridView.RowCount > 0);
        }

        private void InsuranceGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            object currency = InsuranceGridView.GetListSourceRowCellValue(rowIndex, "CURRENCY_CODE");
            if (e.Column == Insurance_AmountText)
            {
                object interest = InsuranceGridView.GetListSourceRowCellValue(rowIndex, "INSURANCE_AMOUNT");
                e.Value = interest.ToString() + " " + currency.ToString();
            }

            if (e.Column == Insurance_UnconditionalText)
            {
                object interest = InsuranceGridView.GetListSourceRowCellValue(rowIndex, "UNCONDITIONAL_AMOUNT");
                e.Value = interest.ToString() + " " + currency.ToString();
            }

            if (e.Column == Insurance_PeriodText)
            {
                object period = InsuranceGridView.GetListSourceRowCellValue(rowIndex, "INSURANCE_PERIOD");
                e.Value = period.ToString() + " ay";
            }

            if (e.Column == Insurance_InterestText)
            {
                object period = InsuranceGridView.GetListSourceRowCellValue(rowIndex, "INSURANCE_INTEREST");
                e.Value = period.ToString() + " %";
            }
        }

        private void NewIndividualPersonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCustomerAddEdit("INSERT", null, null);
        }

        private void SellerIssuingLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerIssuingLookUp.EditValue == null)
                return;

            card_issuing_id = Convert.ToInt32(SellerIssuingLookUp.EditValue);
        }

        private void SellerIssuingLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 3);
        }

        private void SellerSeriesLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 2);
        }

        private void BExtendTime_Click(object sender, EventArgs e)
        {
            FContractExtend fce = new FContractExtend();
            fce.ContractID = int.Parse(ContractID);
            fce.ContractCode = ContractCodeText.Text;
            fce.Percent = int.Parse(InterestText.Text);
            fce.CustomerName = CustomerFullNameText.Text;
            fce.CurrencyCode = CreditCurrencyLookUp.Text;
            fce.ContractStartDate = ContractStartDate.DateTime;
            fce.CurrencyID = credit_currency_id;
            fce.Debt = CreditAmountValue.Value;
            fce.PersonType = CustomerTypeLabel.Text;
            fce.RefreshDataGridView += new FContractExtend.DoEvent(LoadPaymentScheduleDataGridView);
            fce.ShowDialog();

            ExtendTimeCaption(true);
        }

        private void PaymentSchedulesGridView_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            if (info.Column.Caption == "Qrafikin növü")
            {
                string rowValue = view.GetGroupRowValue(e.RowHandle, info.Column).ToString();
                string colorName = getColorName(rowValue);
                info.GroupText = info.Column.Caption + ": <color=" + colorName + ">" + info.GroupValueText + "</color> ";
                info.GroupText += "<color=LightSteelBlue>" + view.GetGroupSummaryText(e.RowHandle) + "</color> ";
            }
        }

        string getColorName(string value)
        {
            if (value.IndexOf("Uzadılmış") > 0)
                return "Red";
            else
                return "Blue";
        }

        private void PaymentSchedulesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PaymentSchedulesGridView.GetFocusedDataRow();
            if (row != null)
            {
                sheduleVersion = Convert.ToInt32(row["VERSION"]);
                CalcPaymentSchedulesBarButton.Visibility = (sheduleVersion == 0) ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                CalcPaymentSchedulesBarButton.Caption = "Əsas qrafiki yenidən hesabla";
                PrintPaymentSchedulesBarButton.Caption = (sheduleVersion == 0) ? "Əsas qrafikin çap faylını hazırla" : "Uzadılmış qrafikin çap faylını hazırla";
            }
        }

        private void PaymentSchedulesGridView_EndGrouping(object sender, EventArgs e)
        {
            if ((sender as GridView).DataRowCount > 0)
                (sender as GridView).SetRowExpanded(-1, true);
        }

        private void FContractAddEdit_Shown(object sender, EventArgs e)
        {
            if (IsSpecialAttention == 1)
            {
                FSpecialWarning fs = new FSpecialWarning();
                fs.Description = "<color=red>DİQQƏT!!!</color>  Bu müqaviləyə xüsusi nəzarət var";
                fs.ShowDialog();
            }
        }

        private void CancelInsuranceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sığortadan imtina etmək istəyirsiniz?", "Sığortadan imtina etmək", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.INSURANCES_TEMP SET IS_CHANGE = 1, IS_CANCEL = 1 WHERE ID = {InsuranceID} AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Sığorta temp cədvəldə imtina edilmədi.",
                                                this.Name + "/CancelInsuranceBarButton_ItemClick");
                GenerateFile();
            }
            LoadInsuranceDataGridView();
        }

        private void SellerPowerIssuingLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerPowerIssuingLookUp.EditValue == null)
                return;

            power_card_issuing_id = Convert.ToInt32(SellerPowerIssuingLookUp.EditValue);
        }

        private void SellerPowerSeriesLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerPowerSeriesLookUp.EditValue == null)
                return;

            power_card_series_id = Convert.ToInt32(SellerPowerSeriesLookUp.EditValue);
        }

        private void SellerPowerNumberText_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerPowerNumberText.Text.Trim().Length == 0)
                PowerNumberLengthLabel.Visible = false;
            else
            {
                PowerNumberLengthLabel.Visible = true;
                PowerNumberLengthLabel.Text = SellerPowerNumberText.Text.Trim().Length.ToString();
            }
        }

        private void SellerPowerPinCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerPowerPinCodeText.Text.Trim().Length == 0)
                PowerPinCodeLengthLabel.Visible = false;
            else
            {
                PowerPinCodeLengthLabel.Visible = true;
                PowerPinCodeLengthLabel.Text = SellerPowerPinCodeText.Text.Trim().Length.ToString();
            }
        }

        private void CancelInsuranceFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Sığortadan imtina ərizəsinin skan olunmuş faylını seçin";
                dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png;*.pdf)|*.jpeg;*.jpg;*.bmp;*.png;*.pdf|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Pdf files (*.pdf)|*.pdf";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(dlg.FileName);
                    string sql = $@"UPDATE CRS_USER_TEMP.INSURANCES_TEMP SET IS_CHANGE = 1, CANCEL_FILE = :BlobFile, CANCEL_FILE_EXTENSION = '{extension}' WHERE ID = {InsuranceID} AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}";
                    GlobalFunctions.ExecuteQueryWithBlob(sql, dlg.FileName, "Sığortadan imtina ərizəsinin skan olunmuş faylı bazada saxlanılmadı.", this.Name + "/CancelInsuranceFileBarButton_ItemClick");
                }
                dlg.Dispose();
            }
        }

        private void PowerOfAttorneyCheck_CheckedChanged(object sender, EventArgs e)
        {
            PowerOfAttorneyNumberText.Visible =
                PowerOfAttorneyNumberLabel.Visible =
                PowerSeparatorControl.Visible =
                SellerPowerSeriesLabel.Visible =
                SellerPowerSeriesLookUp.Visible =
                SellerPowerNumberLabel.Visible =
                SellerPowerNumberText.Visible =
                SellerPowerPinCodeLabel.Visible =
                SellerPowerPinCodeText.Visible =
                SellerPowerIssueDate.Visible =
                SellerPowerIssuingDateLabel.Visible =
                SellerPowerIssuingLabel.Visible =
                SellerPowerIssuingLookUp.Visible =
                SellerPowerOfAttorneyNameLabel.Visible =
                SellerPowerOfAttorneyNameText.Visible = PowerOfAttorneyCheck.Checked;

            SellerIssuingStarLabel.Visible =
            SellerIssueStarLabel.Visible =
            SellerSeriesStarLabel.Visible =
            SellerNumberStarLabel.Visible =
            SellerRegistrationAddressStarLabel.Visible = !PowerOfAttorneyCheck.Checked;

            if (PowerOfAttorneyCheck.Checked)
                SellerTypeRadioGroup.SelectedIndex = 0;
        }

        private void ShowCancelInsuranceFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowWordFileWithExtensionFromDB($@"SELECT T.CANCEL_FILE, CANCEL_FILE_EXTENSION FROM CRS_USER_TEMP.INSURANCES_TEMP T WHERE T.ID = {InsuranceID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Trim() + "_SığortadanImtinaErizezi",
                                                "CANCEL_FILE", "CANCEL_FILE_EXTENSION");
        }

        private void CommitmentPersonInfoBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CommitmentPersonTypeID == 1)
            {
                Total.FCommitmentInfo fci = new Total.FCommitmentInfo();
                fci.CommitmentID = int.Parse(CommitmentID);
                fci.ShowDialog();
            }
            else
            {
                Total.FJuridicalCommitmentInfo fjc = new Total.FJuridicalCommitmentInfo();
                fjc.CommitmentID = int.Parse(CommitmentID);
                fjc.ShowDialog();
            }
        }

        private void BCommon_Click(object sender, EventArgs e)
        {
            InsertCommitmensTemp();
            string customerName, voen, account;
            int customerType, personTypeID, customerID;

            string sql = $@"SELECT COMMITMENT_NAME, VOEN, ACCOUNT_NUMBER, PERSON_TYPE_ID, ID
                                  FROM CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP CA
                                 WHERE     CONTRACT_ID = {ContractID}
                                       AND ID =
                                              (SELECT MAX (ID)
                                                 FROM CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP WHERE
                                                  CONTRACT_ID = CA.CONTRACT_ID)";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/BCommon_Click", "Öhdəliyin məlumatları açılmadı.");

            if (dt.Rows.Count > 0)
            {
                customerName = dt.Rows[0]["COMMITMENT_NAME"].ToString();
                voen = dt.Rows[0]["VOEN"].ToString();
                account = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                personTypeID = Convert.ToInt16(dt.Rows[0]["PERSON_TYPE_ID"]);
                customerID = Convert.ToInt16(dt.Rows[0]["ID"]);
                customerType = 2;

                if (account.Length == 0)
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 6;
                    GlobalProcedures.ShowErrorMessage($@"Sərəncam vermək üçün sonuncu öhdəlik götürən şəxsin ({customerName}) bankdakı hesabı daxil edilməlidir.");
                    return;
                }

                if (voen.Length == 0)
                {
                    OtherInfoTabControl.SelectedTabPageIndex = 6;
                    GlobalProcedures.ShowErrorMessage($@"Sərəncam vermək üçün sonuncu öhdəlik götürən şəxsin ({customerName}) VÖEN-ni daxil edilməlidir.");
                    return;
                }
            }
            else
            {
                customerName = CustomerFullNameText.Text;
                voen = VoenText.Text;
                account = AccountText.Text;
                personTypeID = customer_type_id;
                customerID = int.Parse(CustomerID);
                customerType = 1;

                if (AccountText.Text.Length == 0)
                {
                    AccountText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Sərəncam vermək üçün müştərinin bankdakı hesabı daxil edilməlidir.");
                    AccountText.BackColor = GlobalFunctions.ElementColor();
                    return;
                }

                if (VoenText.Text.Length == 0)
                {
                    VoenText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Sərəncam vermək üçün müştərinin VÖEN-ni daxil edilməlidir.");
                    VoenText.BackColor = GlobalFunctions.ElementColor();
                    return;
                }
            }

            FCommon fc = new FCommon();
            fc.ContractID = int.Parse(ContractID);
            fc.CustomerName = customerName;
            fc.Voen = voen;
            fc.Account = account;
            fc.CustomerID = customerID;
            fc.CustomerType = customerType;
            fc.PersonTypeID = personTypeID;
            fc.CurrencyID = credit_currency_id;
            fc.ContractCode = ContractCodeText.Text;
            fc.CurrencyCode = CreditCurrencyLookUp.Text;
            fc.ContractAmount = CreditAmountValue.Value;
            fc.ContractStartDate = ContractStartDate.DateTime;
            fc.ShowDialog();
        }

        private void GenerateFile()
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Sığorta məsuliyyəti.docx"))
            {
                GlobalVariables.WordDocumentUsed = false;
                GlobalProcedures.ShowErrorMessage(GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında sığorta məsuliyyətinin şablon faylı tapılmadı.");
                return;
            }

            List<ContractCommitment> lstTempCommitments = CommitmentsDAL.SelectAllCommitmentTempByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();
            if (lstTempCommitments.Count > 0)
                carOwnerName = lstTempCommitments.LastOrDefault().COMMITMENT_NAME;
            else
            {
                List<ContractCommitment> lstCommitments = CommitmentsDAL.SelectAllCommitmentByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();
                if (lstCommitments.Count > 0)
                    carOwnerName = lstCommitments.LastOrDefault().COMMITMENT_NAME;
                else
                    carOwnerName = CustomerFullNameText.Text;
            }

            try
            {
                Document document = new Document();
                document.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\Sığorta məsuliyyəti.docx");
                document.ReplaceText("[$customername]", carOwnerName);
                document.ReplaceText("[$contractcode]", ContractCodeText.Text);
                document.ReplaceText("[$car]", BrandComboBox.Text + " " + ModelComboBox.Text);

                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\SığortaMəsuliyyəti_" + ContractCodeText.Text + ".docx"))
                    File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\SığortaMəsuliyyəti_" + ContractCodeText.Text + ".docx");
                document.Save(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\SığortaMəsuliyyəti_" + ContractCodeText.Text + ".docx");
                document.Dispose();

                Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\SığortaMəsuliyyəti_" + ContractCodeText.Text + ".docx");
            }
            catch
            {
                GlobalProcedures.ShowErrorMessage("SığortaMəsuliyyəti_" + ContractCodeText.Text + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
        }

        private void PaymentSchedulesGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "SS")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
        }

        private void NewIndividualCommitmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCommitment("INSERT", null);
        }

        private void InsuranceGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            int isCancel = int.Parse(currentView.GetRowCellDisplayText(e.RowHandle, currentView.Columns["IS_CANCEL"]));
            if (isCancel == 1)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor2);
            }
        }

        private void CommitmentsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;

            if (e.Column == Commitment_PersonTypeName)
            {
                int typeID = int.Parse(CommitmentsGridView.GetListSourceRowCellValue(rowIndex, "PERSON_TYPE_ID").ToString());
                e.Value = (typeID == 1) ? "Fiziki şəxs" : "Hüquqi şəxs";
            }
        }

        private void NewJuridicalCommitmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFJuridicalCommitment("INSERT", null);
        }

        void SelectedPerson(int sellerID, int categoryID, string sellerType)
        {
            string sql = null;

            switch (categoryID)
            {
                case 1:
                    {
                        FindSeller(sellerID);
                    }
                    break;
                case 2:
                    {
                        sql = $@"SELECT COMMITMENT_NAME,
                                           REGEXP_SUBSTR (COMMITMENT_NAME,
                                                          '[^ ]+',
                                                          1,
                                                          1)
                                              SURNAME,
                                           REGEXP_SUBSTR (COMMITMENT_NAME,
                                                          '[^ ]+',
                                                          1,
                                                          2)
                                              NAME,
                                              REGEXP_SUBSTR (COMMITMENT_NAME,
                                                             '[^ ]+',
                                                             1,
                                                             3)
                                           || ' '
                                           || REGEXP_SUBSTR (COMMITMENT_NAME,
                                                             '[^ ]+',
                                                             1,
                                                             4)
                                              PATRONYMIC,
                                           CS.SERIES,
                                           C.CARD_NUMBER,
                                           C.CARD_PINCODE PINCODE,
                                           C.ISSUEDATE ISSUE_DATE,
                                           CI.NAME ISSUE_NAME,
                                           C.ADDRESS,
                                           C.CARD_SERIES_ID,
                                           C.CARD_ISSUING_ID
                                      FROM CRS_USER.CUSTOMER_COMMITMENTS C,
                                           CRS_USER.V_COMMITMENT_CARDS CC,
                                           CRS_USER.CARD_SERIES CS,
                                           CRS_USER.CARD_ISSUING CI
                                     WHERE     C.ID = CC.COMMITMENT_ID
                                           AND C.CARD_SERIES_ID = CS.ID
                                           AND C.CARD_ISSUING_ID = CI.ID
                                           AND C.ID = {sellerID}";
                        try
                        {
                            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SelectedPerson");

                            foreach (DataRow dr in dt.Rows)
                            {
                                SellerSurnameText.Text = dr["SURNAME"].ToString();
                                SellerNameText.Text = dr["NAME"].ToString();
                                SellerPatronymicText.Text = dr["PATRONYMIC"].ToString();

                                SellerSeriesLookUp.EditValue = SellerSeriesLookUp.Properties.GetKeyValueByDisplayText(dr["SERIES"].ToString());
                                SellerNumberText.Text = dr["CARD_NUMBER"].ToString();
                                SellerPinCodeText.Text = dr["PINCODE"].ToString();
                                SellerIssueDate.EditValue = DateTime.Parse(dr["ISSUE_DATE"].ToString());

                                SellerIssuingLookUp.EditValue = SellerIssuingLookUp.Properties.GetKeyValueByDisplayText(dr["ISSUE_NAME"].ToString());
                                SellerRegistrationAddressText.Text = dr["ADDRESS"].ToString();

                                BLoadSellerPicture.Text = "Yüklə";
                                BDeleteSellerPicture.Enabled = false;
                                switch (GlobalVariables.SelectedLanguage)
                                {
                                    case "RU":
                                        SellerPictureBox.Properties.NullText = "Фотография продавца";
                                        break;
                                    case "EN":
                                        SellerPictureBox.Properties.NullText = "Seller picture";
                                        break;
                                }

                                sellerdetails = true;
                                seller_type_id = 1;
                            }
                        }
                        catch (Exception exx)
                        {
                            GlobalProcedures.LogWrite("Fiziki şəxsin parametrləri tapılmadı.", sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                        }
                    }
                    break;
                case 3:
                    {
                        if (SellerTypeRadioGroup.SelectedIndex == 0)
                        {
                            SellerPictureBox.Image = null;
                            sql = $@"SELECT C.SURNAME,
                                       C.NAME,
                                       C.PATRONYMIC,
                                       CS.SERIES,
                                       CC.CARD_NUMBER,
                                       CC.PINCODE,
                                       CC.ISSUE_DATE,
                                       CI.NAME ISSUE_NAME,
                                       CC.ADDRESS,
                                       CC.REGISTRATION_ADDRESS,
                                       CIM.IMAGE,
                                       CC.FRONT_FACE_IMAGE,
                                       CC.FRONT_FACE_IMAGE_FORMAT,
                                       CC.REAR_FACE_IMAGE,       
                                       CC.REAR_FACE_IMAGE_FORMAT,
                                       CC.CARD_SERIES_ID,
                                       CC.CARD_ISSUING_ID
                                  FROM CRS_USER.CUSTOMERS C,
                                       CRS_USER.CUSTOMER_IMAGE CIM,
                                       CRS_USER.CUSTOMER_CARDS CC,
                                       CRS_USER.CARD_SERIES CS,
                                       CRS_USER.CARD_ISSUING CI
                                 WHERE     CC.CARD_ISSUING_ID = CI.ID
                                       AND CC.CARD_SERIES_ID = CS.ID
                                       AND CC.CUSTOMER_ID = C.ID
                                       AND CIM.CUSTOMER_ID(+) = C.ID
                                       AND C.ID = {sellerID}";
                            try
                            {
                                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SelectedPerson");

                                foreach (DataRow dr in dt.Rows)
                                {
                                    SellerSurnameText.Text = dr["SURNAME"].ToString();
                                    SellerNameText.Text = dr["NAME"].ToString();
                                    SellerPatronymicText.Text = dr["PATRONYMIC"].ToString();

                                    SellerSeriesLookUp.EditValue = SellerSeriesLookUp.Properties.GetKeyValueByDisplayText(dr["SERIES"].ToString());
                                    SellerNumberText.Text = dr["CARD_NUMBER"].ToString();
                                    SellerPinCodeText.Text = dr["PINCODE"].ToString();
                                    SellerIssueDate.EditValue = DateTime.Parse(dr["ISSUE_DATE"].ToString());

                                    SellerIssuingLookUp.EditValue = SellerIssuingLookUp.Properties.GetKeyValueByDisplayText(dr["ISSUE_NAME"].ToString());
                                    SellerAddressText.Text = dr["ADDRESS"].ToString();
                                    SellerRegistrationAddressText.Text = dr["REGISTRATION_ADDRESS"].ToString();

                                    if (!DBNull.Value.Equals(dr["IMAGE"]))
                                    {
                                        Byte[] BLOBData = (byte[])dr["IMAGE"];
                                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                                        SellerPictureBox.Image = Image.FromStream(stmBLOBData);

                                        SellerImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images";
                                        if (!Directory.Exists(SellerImagePath))
                                            Directory.CreateDirectory(SellerImagePath);

                                        GlobalProcedures.DeleteFile(SellerImagePath + "\\S_" + SellerSurnameText.Text + ".jpeg");
                                        FileStream fs = new FileStream(SellerImagePath + "\\S_" + SellerSurnameText.Text + ".jpeg", FileMode.Create, FileAccess.Write);
                                        stmBLOBData.WriteTo(fs);
                                        fs.Close();
                                        stmBLOBData.Close();
                                        SellerImage = SellerImagePath + "\\S_" + SellerSurnameText.Text + ".jpeg";
                                        BLoadSellerPicture.Text = "Dəyiş";
                                        BDeleteSellerPicture.Enabled = true;
                                    }
                                    else
                                    {
                                        BLoadSellerPicture.Text = "Yüklə";
                                        BDeleteSellerPicture.Enabled = false;
                                        switch (GlobalVariables.SelectedLanguage)
                                        {
                                            case "RU":
                                                SellerPictureBox.Properties.NullText = "Фотография продавца";
                                                break;
                                            case "EN":
                                                SellerPictureBox.Properties.NullText = "Seller picture";
                                                break;
                                        }
                                    }

                                    if (!DBNull.Value.Equals(dr["FRONT_FACE_IMAGE"]))
                                    {
                                        Byte[] BLOBData = (byte[])dr["FRONT_FACE_IMAGE"];
                                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);

                                        if (!Directory.Exists(IDImagePath))
                                            Directory.CreateDirectory(IDImagePath);

                                        GlobalProcedures.DeleteFile(IDImagePath + "\\S_Front_" + SellerSurnameText.Text + dr["FRONT_FACE_IMAGE_FORMAT"]);
                                        FileStream front_fs = new FileStream(IDImagePath + "\\S_Front_" + SellerSurnameText.Text + dr["FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                                        stmBLOBData.WriteTo(front_fs);
                                        front_fs.Close();
                                        stmBLOBData.Close();
                                        SellerFrontFaceButtonEdit.Text = IDImagePath + "\\S_Front_" + SellerSurnameText.Text + dr["FRONT_FACE_IMAGE_FORMAT"];
                                    }

                                    if (!DBNull.Value.Equals(dr["REAR_FACE_IMAGE"]))
                                    {
                                        Byte[] BLOBData = (byte[])dr["REAR_FACE_IMAGE"];
                                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);

                                        if (!Directory.Exists(IDImagePath))
                                            Directory.CreateDirectory(IDImagePath);

                                        GlobalProcedures.DeleteFile(IDImagePath + "\\S_Rear_" + SellerSurnameText.Text + dr["REAR_FACE_IMAGE_FORMAT"]);
                                        FileStream rear_fs = new FileStream(IDImagePath + "\\S_Rear_" + SellerSurnameText.Text + dr["REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                                        stmBLOBData.WriteTo(rear_fs);
                                        rear_fs.Close();
                                        stmBLOBData.Close();
                                        SellerRearFaceButtonEdit.Text = IDImagePath + "\\S_Rear_" + SellerSurnameText.Text + dr["REAR_FACE_IMAGE_FORMAT"];
                                    }
                                    sellerdetails = true;
                                    seller_type_id = 1;
                                }
                            }
                            catch (Exception exx)
                            {
                                GlobalProcedures.LogWrite("Fiziki şəxsin parametrləri tapılmadı.", sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                            }
                        }
                        else
                        {
                            sql = $@"SELECT NAME,VOEN,LEADING_NAME,ADDRESS FROM CRS_USER.JURIDICAL_PERSONS WHERE IS_BUYER = 1 AND ID = {sellerID}";
                            try
                            {
                                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SelectedPerson");

                                foreach (DataRow dr in dt.Rows)
                                {
                                    JuridicalPersonNameText.Text = dr["NAME"].ToString();
                                    JuridicalPersonVoenText.Text = dr["VOEN"].ToString();
                                    LeadingPersonNameText.Text = dr["LEADING_NAME"].ToString();
                                    JuridicalPersonAddressText.Text = dr["ADDRESS"].ToString();
                                }
                                sellerdetails = true;
                                seller_type_id = 2;
                            }
                            catch (Exception exx)
                            {
                                GlobalProcedures.LogWrite("Hüquqi şəxsin parametrləri tapılmadı.", sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                            }
                        }
                    }
                    break;
            }

            ExecuteAgainSellerPhonesTempProcedure(sellerID, sellerType);
            LoadPhoneDataGridView(SellerID);
        }

        private void ExecuteAgainSellerPhonesTempProcedure(int sellerID, string sellerType)
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "CRS_USER_TEMP.PROC_INS_AGAINSELLER_PHONETEMP";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_OLD_SELLER_ID", OracleDbType.Int32).Value = sellerID;
                    command.Parameters.Add("P_NEW_SELLER_ID", OracleDbType.Int32).Value = SellerID;
                    command.Parameters.Add("P_SELLER_TYPE", OracleDbType.Varchar2).Value = sellerType;
                    command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Telefonlar temp cədvələ daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, MethodBase.GetCurrentMethod().Name + "/CRS_USER_TEMP.PROC_INSERT_SELLER_PHONE_TEMP", exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void BSearcSeller_Click(object sender, EventArgs e)
        {
            int sellerTupeID = (SellerTypeRadioGroup.SelectedIndex + 1);

            FAgainPerson fa = new FAgainPerson();
            fa.PersonTypeID = sellerTupeID;
            fa.SelectedPerson += new FAgainPerson.DoEvent(SelectedPerson);
            fa.ShowDialog();
        }

        private void SellerSeriesLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerSeriesLookUp.EditValue == null)
                return;

            card_series_id = Convert.ToInt32(SellerSeriesLookUp.EditValue);
            seller_card_name = GlobalFunctions.GetName($@"SELECT NAME FROM CRS_USER.CARD_SERIES WHERE ID = {card_series_id}");
        }

        private void NewJuridicalPersonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFJuridicalAddEdit("INSERT", null);
        }

        private void EditInsuranceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsuranceAddEdit("EDIT", ContractID, InsuranceID, currency_id.ToString(), LiquidCurrencyLookUp.Text);
        }

        private void SellerTypeRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                SellerType();
                LoadPhoneDataGridView(SellerID);
            }
        }

        private void InsuranceGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditInsuranceBarButton.Enabled && InsuranceStandaloneBarDockControl.Enabled)
                LoadFInsuranceAddEdit("EDIT", ContractID, InsuranceID, currency_id.ToString(), LiquidCurrencyLookUp.Text);
        }

        private void RefreshInsuranceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadInsuranceDataGridView();
        }

        private void InsuranceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (!CurrentStatus)
                GlobalProcedures.GridMouseUpForPopupMenu(InsuranceGridView, InsurancePopupMenu, e);
        }

        private void DeleteInsurance()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sığortanı silmək istəyirsiniz?", "Sığortanın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.INSURANCES_TEMP SET IS_CHANGE = 2 WHERE ID = {InsuranceID} AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Sığorta temp cədvəldən silinmədi.",
                                                this.Name + "/DeleteInsurance");
            }
        }

        private void DeleteInsuranceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteInsurance();
            LoadInsuranceDataGridView();
        }

        private void ViewInsuranceFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\RDLC\\Insurance.rdlc"))
            {
                XtraMessageBox.Show(GlobalVariables.V_ExecutingFolder + "\\RDLC\\Insurance.rdlc şablon faylı yoxdur.");
                return;
            }
            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
            string path = null, company = null, address = null, address_description = null, phone = null, fax = null, sql = null;
            if (ControlInsureDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
                string ins, chassis = null, engine = null;

                if (String.IsNullOrEmpty(ChassisText.Text))
                    chassis = " ";
                else
                    chassis = ChassisText.Text;
                if (String.IsNullOrEmpty(EngineNumberText.Text))
                    engine = " ";
                else
                    engine = EngineNumberText.Text;
                ins = ((insurance_amount * insurance_interest) / 100).ToString("N2");
                rv_insurance.LocalReport.ReportPath = GlobalVariables.V_ExecutingFolder + "\\RDLC\\Insurance.rdlc";
                ReportParameter p1 = new ReportParameter("PBrand", BrandComboBox.Text + ", " + ModelComboBox.Text + ", " + TypeComboBox.Text);
                ReportParameter p2 = new ReportParameter("PColor", ColorComboBox.Text);
                ReportParameter p3 = new ReportParameter("PYear", YearValue.Value.ToString());
                ReportParameter p4 = new ReportParameter("PCarNumber", CarNumberText.Text.Trim());
                ReportParameter p5 = new ReportParameter("PBan", BanText.Text.Trim());
                ReportParameter p6 = new ReportParameter("PChassis", chassis);
                ReportParameter p7 = new ReportParameter("PEngine", engine);
                ReportParameter p8 = new ReportParameter("PCarAmount", car_amount.ToString("N2"));
                ReportParameter p9 = new ReportParameter("PInsuranceAmount", insurance_amount.ToString("N2"));
                ReportParameter p10 = new ReportParameter("PUnAmount", unconditional_amount.ToString("N2"));
                ReportParameter p11 = new ReportParameter("PIns", ins);
                ReportParameter p12 = new ReportParameter("PPeriod", insurance_period.ToString() + " ay");
                ReportParameter p13 = new ReportParameter("PStartDate", insurance_startdate);
                ReportParameter p14 = new ReportParameter("PEndDate", insurance_enddate);

                sql = $@"SELECT T.LOGO,
                               T.NAME,
                               T.ADDRESS,
                               T.ADDRESS_DESCRIPTION,
                               T.PHONE_NUMBER,
                               T.FAX
                          FROM CRS_USER.INSURANCE_COMPANY T
                         WHERE T.ID = {insurance_company_id}";
                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/ViewInsuranceFileBarButton_ItemClick");

                foreach (DataRow dr in dt.Rows)
                {
                    if (!DBNull.Value.Equals(dr["LOGO"]))
                    {
                        Byte[] BLOBData = (byte[])dr["LOGO"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(ImagePath))
                        {
                            Directory.CreateDirectory(ImagePath);
                        }
                        GlobalProcedures.DeleteFile(ImagePath + "\\Insurance_" + insurance_company_id + ".jpg");
                        FileStream fs = new FileStream(ImagePath + "\\Insurance_" + insurance_company_id + ".jpg", FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(fs);
                        fs.Close();
                        stmBLOBData.Close();
                        path = "File://" + ImagePath + "\\Insurance_" + insurance_company_id + ".jpg";
                    }
                    company = dr["NAME"].ToString();
                    address = dr["ADDRESS"].ToString();
                    address_description = dr["ADDRESS_DESCRIPTION"].ToString();
                    phone = dr["PHONE_NUMBER"].ToString();
                    if (!String.IsNullOrWhiteSpace(dr["FAX"].ToString()))
                        fax = dr["FAX"].ToString();
                    else
                        fax = " ";
                }

                ReportParameter p15 = new ReportParameter("PCompany", company);
                ReportParameter p16 = new ReportParameter("PCompanyAddress", address);
                ReportParameter p17 = new ReportParameter("PCompanyAddressDescription", address_description);
                ReportParameter p18 = new ReportParameter("PCompanyPhone", "Tel: " + phone);
                ReportParameter p19 = new ReportParameter("PCompanyFax", fax);
                ReportParameter p20 = new ReportParameter("PImage", path, true);
                rv_insurance.LocalReport.EnableExternalImages = true;
                rv_insurance.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20 });

                Warning[] warnings;
                try
                {
                    if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Sığorta.doc"))
                        File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Sığorta.doc");
                    using (var stream = File.Create(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Sığorta.doc"))
                    {
                        rv_insurance.LocalReport.Render(
                            "WORD",
                            @"<DeviceInfo><ExpandContent>True</ExpandContent></DeviceInfo>",
                            (CreateStreamCallback)delegate { return stream; },
                            out warnings);
                    }

                    if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Sığorta.doc"))
                        Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Sığorta.doc");
                    else
                        XtraMessageBox.Show("Sığortanın çap faylı yaradılmayıb.");
                }
                catch
                {
                    XtraMessageBox.Show(code_number + "_Sığorta.doc faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
                }
                finally
                {
                    GlobalProcedures.SplashScreenClose();
                }
            }
        }

        private void InsuranceGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = InsuranceGridView.GetFocusedDataRow();
            if (row != null)
            {
                InsuranceID = row["ID"].ToString();
                car_amount = Convert.ToDouble(row["CAR_AMOUNT"].ToString());
                insurance_amount = Convert.ToDouble(row["INSURANCE_AMOUNT"].ToString());
                unconditional_amount = Convert.ToDouble(row["UNCONDITIONAL_AMOUNT"].ToString());
                insurance_period = Convert.ToInt32(row["INSURANCE_PERIOD"].ToString());
                insurance_interest = Convert.ToDouble(row["INSURANCE_INTEREST"].ToString());
                insurance_startdate = row["START_DATE"].ToString().Substring(0, 10);
                insurance_enddate = row["END_DATE"].ToString().Substring(0, 10);
                insurance_company_id = Convert.ToInt32(row["COMPANY_ID"].ToString());
                CancelInsuranceBarButton.Enabled = (Convert.ToInt32(row["IS_CANCEL"].ToString()) == 0);
                CancelInsuranceFileBarButton.Enabled = ShowCancelInsuranceFileBarButton.Enabled = (Convert.ToInt32(row["IS_CANCEL"].ToString()) == 1);
            }
        }

        private void BDeleteContractPicture_Click(object sender, EventArgs e)
        {
            int currentindex = ContractImageSlider.CurrentImageIndex;
            if (currentindex == -1)
                currentindex = 0;
            ContractImageSlider.Images.Remove(ContractImageSlider.Images[currentindex]);
            image_list.Remove(image_list[currentindex]);
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkil";
                    break;
                case "EN":
                    ImageCountLabel.Text = ContractImageSlider.Images.Count + " picture";
                    break;
                case "RU":
                    ImageCountLabel.Text = ContractImageSlider.Images.Count + " картина";
                    break;
            }

            if (ContractImageSlider.Images.Count == 0)
                BDeleteContractPicture.Enabled = BChangeContractPicture.Enabled = false;
        }

        private void BanTypeComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadHostageDictionaries("E", 8, 4);
        }

        private void BanTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    ban_type_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_BAN_TYPES", "NAME", "ID", BanTypeComboBox.Text);
                    break;
                case "EN":
                    ban_type_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_BAN_TYPES", "NAME_EN", "ID", BanTypeComboBox.Text);
                    break;
                case "RU":
                    ban_type_id = GlobalFunctions.FindComboBoxSelectedValue("CAR_BAN_TYPES", "NAME_RU", "ID", BanTypeComboBox.Text);
                    break;
            }
        }

        private void GenerateCurrencyRateLabel()
        {
            if (credit_currency_id == 0 || currency_id == 0)
                return;

            if (FormStatus)
            {
                if (credit_currency_id != currency_id || credit_currency_id != 1 || currency_id != 1)
                {
                    CreditCurrencyRateLabel.Visible = true;
                    if (rate == 0 || credit_currency_id == 1)
                    {
                        if (((GlobalFunctions.CurrencyLastRate(credit_currency_id, ContractStartDate.Text) != 0 && credit_currency_id != 1) || (GlobalFunctions.CurrencyLastRate(currency_id, ContractStartDate.Text) != 0 && currency_id != 1)))
                            credit_currency_rate = GlobalFunctions.CalculatedExchange(1, credit_currency_id, ContractStartDate.Text, currency_id, ContractStartDate.Text);
                        else
                        {
                            credit_currency_rate = 0;
                            CreditAmountValue.Enabled = LiquidValue.Enabled = FirstPaymentValue.Enabled = BOK.Visible = false;

                            if (credit_currency_id != 1)
                                XtraMessageBox.Show(CreditCurrencyLookUp.Text + " valyutasının " + ContractStartDate.Text + " tarixinə AZN ilə qarşılığı daxil edilməyib.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            else if (currency_id != 1)
                                XtraMessageBox.Show(LiquidCurrencyLookUp.Text + " valyutasının " + ContractStartDate.Text + " tarixinə AZN ilə qarşılığı daxil edilməyib.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        credit_currency_rate = rate;

                    if (credit_currency_rate > 0)
                    {
                        BOK.Visible = !CurrentStatus;
                        LiquidValue.Enabled = (pay_count == 0 && !CurrentStatus);
                        FirstPaymentValue.Enabled = (pay_count == 0 && !CurrentStatus);
                        CreditAmountValue.Enabled = LiquidValue.Enabled;
                    }
                    CreditCurrencyRateLabel.Text = credit_currency_value + " " + credit_currency_name + " = " + credit_currency_rate.ToString("N4") + " " + currency_name;
                }
                else
                {
                    CreditAmountValue.Enabled = LiquidValue.Enabled = FirstPaymentValue.Enabled = true;
                    credit_currency_rate = 1;
                    CreditCurrencyRateLabel.Visible = false;
                    BOK.Visible = !CurrentStatus;
                }
            }
        }

        private void BChangeContractPicture_Click(object sender, EventArgs e)
        {
            int currentindex = ContractImageSlider.CurrentImageIndex;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Şəkili seçin";
                dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ContractImageSlider.Images.Remove(ContractImageSlider.Images[currentindex]);
                    ContractImageSlider.Images.Add(Image.FromFile(dlg.FileName));
                    image_list.Remove(image_list[currentindex]);
                    image_list.Add(dlg.FileName);
                    contract_images_count = contract_images_count - 1;
                    BDeleteContractPicture.Enabled = !CurrentStatus;
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkildən";
                            break;
                        case "EN":
                            ImageCountLabel.Text = ContractImageSlider.Images.Count + " picture";
                            break;
                        case "RU":
                            ImageCountLabel.Text = ContractImageSlider.Images.Count + " картина";
                            break;
                    }
                }
                dlg.Dispose();
            }
        }

        private void ContractNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (ContractNoteText.Text.Length <= 1400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (1400 - ContractNoteText.Text.Length).ToString();
        }

        private void FilesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(FilesGridView, FilesPopupMenu, e);
        }

        private void BConfirm_Click(object sender, EventArgs e)
        {
            if (ControlContractDetails())
            {
                string commitment = GlobalFunctions.GetName($@"SELECT    CC.DEBT
                                                                       || ' '
                                                                       || C.CODE
                                                                       || ' qalıq borc '
                                                                       || TO_CHAR (CC.AGREEMENTDATE, 'DD.MM.YYYY')
                                                                       || ' tarixindən etibarən '
                                                                       || CC.INTEREST
                                                                       || ' %-lə '
                                                                       || CC.COMMITMENT_NAME
                                                                       || 'na ötürülüb'
                                                                          a
                                                                  FROM CRS_USER.V_COMMITMENTS CC, CRS_USER.CURRENCY C
                                                                 WHERE     C.ID = CC.CURRENCY_ID
                                                                       AND CC.CONTRACT_ID = {ContractID}"),
                    liquid_bank_cash = null, first_bank_cash = null, insurance = null;

                if (LiquidTypeRadioGroup.SelectedIndex == 0 && parent_contract_id == null)
                    liquid_bank_cash = " <i>(kassa üzrə)</i>";
                else if (LiquidTypeRadioGroup.SelectedIndex == 0 && parent_contract_id != null)
                    liquid_bank_cash = " <i>(razılaşma)</i>";
                else
                    liquid_bank_cash = " <i>(" + LiquidBankComboBox.Text + ")</i>";

                if (FirstPaymentTypeRadioGroup.SelectedIndex == 0 && parent_contract_id == null)
                    first_bank_cash = " <i>(kassa üzrə)</i>";
                else if (FirstPaymentTypeRadioGroup.SelectedIndex == 0 && parent_contract_id != null)
                    first_bank_cash = " <i>(razılaşma)</i>";
                else
                    first_bank_cash = " <i>(" + FirstPaymentBankComboBox.Text + ")</i>";

                if (InsuranceGridView.RowCount > 0 && credit_name_id == 1)
                    insurance = "\n\n<color=255,0,0><b>Avtomobil sığortalanıb</b></color>";
                else if (InsuranceGridView.RowCount == 0 && credit_name_id == 1)
                    insurance = "\n\n<color=255,0,0><b>Avtomobil sığortalanmayıb</b></color>";

                string extend = GlobalFunctions.GetName($@"SELECT    'Müqavilə '
                                                                       || CE.DEBT
                                                                       || ' '
                                                                       || C.CURRENCY_CODE
                                                                       || ' qalıq ilə '
                                                                       || TO_CHAR (CE.START_DATE, 'DD.MM.YYYY')
                                                                       || ' - '
                                                                       || TO_CHAR (CE.END_DATE, 'DD.MM.YYYY')
                                                                       || ' tarix intervalında '
                                                                       || MONTH_COUNT
                                                                       || ' aylığına uzadılıb.'
                                                                  FROM CRS_USER_TEMP.V_LAST_CONTRACT_EXTEND_TEMP CE, CRS_USER.V_CONTRACTS C
                                                                 WHERE CE.CONTRACT_ID = C.CONTRACT_ID AND CE.CONTRACT_ID = {ContractID}");

                DialogResult dialogResult = XtraMessageBox.Show("<b>Aşağıda qeyd olunmuş dəyərlərlə " + ContractCodeText.Text + " saylı müqaviləni təsdiqləmək istəyirsiniz?</b>" +
                                                                    "\n\nİllik faiz = " + InterestText.Text + " %" +
                                                                    "\nMüddəti = " + PeriodText.Text + " ay" +
                                                                    "\n\nLikvid dəyəri = " + LiquidValue.Value.ToString("N2") + " " + LiquidCurrencyLookUp.Text + liquid_bank_cash +
                                                                    "\nİlkin ödəniş = " + FirstPaymentValue.Value.ToString("N2") + " " + LiquidCurrencyLookUp.Text + first_bank_cash +
                                                                    "\n<b>Məbləğ = " + CreditAmountValue.Value.ToString("N2") + " " + CreditCurrencyLookUp.Text + "</b>" +
                                                                    insurance +
                                                                    ((commitment != null) ? "\n\n" + commitment : null) +
                                                                    ((extend != null) ? "\n\n" + extend : null), ContractCodeText.Text + " saylı müqavilənin təsdiqlənməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FContractSaveWait));
                    GlobalProcedures.ExecuteTwoQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_COMMIT = 1 WHERE ID = {ContractID}",
                                                     $@"UPDATE CRS_USER.CONTRACT_EXTEND SET IS_COMMIT = 1 WHERE IS_COMMIT = 0 AND CONTRACT_ID = {ContractID}",
                                                        "Müqavilə təsdiqlənmədi.", this.Name + "/BConfirm_Click");
                    if (String.IsNullOrEmpty(commitment))
                    {
                        if (parent_contract_id == null)
                            InsertContractAmountToOperation();
                        else
                            InsertAgreementAmountToOperation();
                    }
                    GlobalProcedures.CalculatedLeasingTotal(ContractID);
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.AGAIN_COMMITMENT WHERE CONTRACT_ID = {ContractID}", "Müqavilə təkrar təsdiq olunma cədvəlindən silinmədi.", this.Name + "/BConfirm_Click");
                    GlobalProcedures.SplashScreenClose();
                    this.Close();
                }
            }
        }

        private void InsertContractAmountToOperation()
        {
            if (FirstPaymentTypeRadioGroup.SelectedIndex == 0)
            {
                GlobalProcedures.InsertCashAdvancePayment(int.Parse(ContractID), int.Parse(CustomerID), ContractStartDate.Text, ContractStartDate.Text, ContractCodeText.Text.Trim() + " saylı müqavilə üzrə avans məbləği", Math.Round(Class.GlobalFunctions.CalculatedExchange((double)FirstPaymentValue.Value, currency_id, ContractStartDate.Text, 1, ContractStartDate.Text), 2), null, ContractCodeText.Text.Trim());
                GlobalProcedures.LoadReceipt(ContractStartDate.Text, (double)FirstPaymentValue.Value, currency_id, 0, CustomerFullNameText.Text.Trim(), int.Parse(ContractID), ContractCodeText.Text.Trim(), "saylı müqavilə üzrə avans ödənişi");
            }
            else
            {
                InsertBankOperation(first_payment_bank_id, first_payment_plan_id, 19, FirstPaymentValue.Value, 0);
                GlobalProcedures.UpdateBankOperationDebtWithBank(ContractStartDate.Text, first_payment_bank_id);
                GlobalProcedures.UpdateBankOperationDebt(ContractStartDate.Text);
            }

            if (LiquidTypeRadioGroup.SelectedIndex == 1)
            {
                InsertBankOperation(liquid_bank_id, liquid_plan_id, 8, 0, LiquidValue.Value);
                GlobalProcedures.UpdateBankOperationDebtWithBank(ContractStartDate.Text, liquid_bank_id);
                GlobalProcedures.UpdateBankOperationDebt(ContractStartDate.Text);
            }

            GlobalProcedures.InsertOperationJournalForSeller(ContractStartDate.Text, (double)LiquidValue.Value, (double)FirstPaymentValue.Value, ContractID, liquid_account, first_payment_account);
        }

        private void InsertAgreementAmountToOperation()
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "CRS_USER.PROC_AGREEMENT_OPERATION";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_DATE", OracleDbType.Varchar2).Value = ContractStartDate.Text;
                    command.Parameters.Add("P_AMOUNT", OracleDbType.Decimal).Value = CreditAmountValue.Value;
                    command.Parameters.Add("P_OLD_CONTRACT_ID", OracleDbType.Int32).Value = int.Parse(oldContractID);
                    command.Parameters.Add("P_NEW_CONTRACT_ID", OracleDbType.Int32).Value = int.Parse(ContractID);
                    command.Parameters.Add("P_NEW_LEASING_ACCOUNT", OracleDbType.Varchar2).Value = LeasingAccountText.Text;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Razılaşma üzrə mühasibat əməliyyatları bazaya daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void CommitmentsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CommitmentsGridView, CommitmentPopupMenu, e);
        }

        private void NewCommitmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCommitment("INSERT", null);
        }

        private void PowerGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (!CurrentStatus)
                GlobalProcedures.GridMouseUpForPopupMenu(PowerGridView, PowerPopupMenu, e);
        }

        void RefreshPower()
        {
            LoadPowerDataGridView();
        }

        private void LoadFPowerOfAttorney(string transaction, string powerid)
        {
            List<ContractCommitment> lstTempCommitments = CommitmentsDAL.SelectAllCommitmentTempByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();
            if (lstTempCommitments.Count > 0)
                carOwnerName = lstTempCommitments.LastOrDefault().COMMITMENT_NAME;
            else
            {
                List<ContractCommitment> lstCommitments = CommitmentsDAL.SelectAllCommitmentByContractID(int.Parse(ContractID)).ToList<ContractCommitment>();
                if (lstCommitments.Count > 0)
                    carOwnerName = lstCommitments.LastOrDefault().COMMITMENT_NAME;
                else
                    carOwnerName = CustomerFullNameText.Text;
            }

            if (ControlPowerOfAttorneyParametrs())
            {
                topindex = PowerGridView.TopRowIndex;
                old_row_num = PowerGridView.FocusedRowHandle;
                FPowerOfAttorney fp = new FPowerOfAttorney();
                fp.TransactionName = transaction;
                fp.PowerID = powerid;
                fp.CustomerName = CustomerFullNameText.Text.Trim();
                fp.CardSeries = card_series;
                fp.CardNumber = card_number;
                fp.CardIssuing = IssuingText.Text.Trim();
                fp.CarNumber = CarNumberText.Text.Trim();
                fp.CardIssuingDate = IssuingDateText.Text.Trim();
                fp.CardReliableDate = ReliableDateText.Text.Trim();
                fp.Object = BrandComboBox.Text + " - " + ModelComboBox.Text + ", Tip - " + TypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN - " + BanText.Text;
                fp.BrandAndModel = BrandComboBox.Text + " " + ModelComboBox.Text;
                fp.BrandModelAndTip = BrandComboBox.Text + " " + ModelComboBox.Text + " - " + TypeComboBox.Text;
                fp.CarOwnerName = carOwnerName;
                fp.CustomerID = CustomerID;
                fp.ContractID = ContractID;
                fp.RowCount = PowerGridView.RowCount;
                fp.RefreshPowerDataGridView += new FPowerOfAttorney.DoEvent(RefreshPower);
                fp.ShowDialog();
                PowerGridView.TopRowIndex = topindex;
                PowerGridView.FocusedRowHandle = old_row_num;
            }
        }

        private void NewPowerOfAttorneyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPowerOfAttorney("INSERT", null);
        }

        private void LoadCommitmentDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 CC.ID,
                                 CC.COMMITMENT_NAME,
                                 CC.AGREEMENTDATE,
                                 CC.TOTAL_DEBT || ' ' || C.CODE DEBT,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST || ' %' C_INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 CC.INSERT_DATE,
                                 CC.PERSON_TYPE_ID
                            FROM CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP CC, CRS_USER.CURRENCY C
                           WHERE CC.IS_CHANGE <> 2 AND CC.CURRENCY_ID = C.ID AND CC.CONTRACT_ID = {ContractID}
                        ORDER BY CC.ID";
            CommitmentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCommitmentDataGridView");
            if (CommitmentsGridView.RowCount > 0)
            {
                EditCommitmentBarButton.Enabled = (GlobalVariables.EditCommitment && !CurrentStatus);
                DeleteCommitmentBarButton.Enabled = (GlobalVariables.DeleteCommitment && !CurrentStatus);
                ViewCommitmentFileBarButton.Enabled = true;
            }
            else
                EditCommitmentBarButton.Enabled = DeleteCommitmentBarButton.Enabled = ViewCommitmentFileBarButton.Enabled = CommitmentPersonInfoBarButton.Enabled = false;
        }

        private void LoadPowerDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                   PA.ID,
                                   PA.POWER_CODE,
                                   PA.FULLNAME||' ('||CS.SERIES || ' ' || PA.CARD_NUMBER||')' FULLNAME,
                                   PA.INSERT_DATE,
                                   PA.POWER_DATE, 
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = PA.INSERT_USER)
                                      INSERT_USER,
                                   PA.FULLNAME_CHECK,
                                   PA.IS_REVOKE,
                                   PA.IS_RESPONSIBLE
                              FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP PA, CRS_USER.CARD_SERIES CS
                             WHERE     PA.IS_CHANGE <> 2
                                   AND PA.CARD_SERIES_ID = CS.ID
                                   AND CONTRACT_ID = {ContractID}
                            ORDER BY PA.ID";

            PowerGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPowerDataGridView");
            power_count = PowerGridView.RowCount;
            if (PowerGridView.RowCount > 0)
            {
                EditPowerOfAttorneyBarButton.Enabled = GlobalVariables.EditPower;
                DeletePowerOfAttorneyBarButton.Enabled = GlobalVariables.DeletePower;
                ViewPowerFileBarButton.Enabled = AgainPowerOfAttorneyBarButton.Enabled = true;                
            }
            else
                EditPowerOfAttorneyBarButton.Enabled =
                    DeletePowerOfAttorneyBarButton.Enabled =
                    ViewPowerFileBarButton.Enabled =
                    AgainPowerOfAttorneyBarButton.Enabled = false;
        }

        private void LoadInsuranceDataGridView()
        {
            string s = null;

            if (!CurrentStatus)
                s = $@"SELECT 1 SS,
                                     I.ID,
                                     IC.NAME COMPANY_NAME,
                                     I.START_DATE,
                                     I.END_DATE,
                                     CUR.CODE CURRENCY_CODE,
                                     I.INSURANCE_AMOUNT,
                                     I.INSURANCE_PERIOD,
                                     I.INSURANCE_INTEREST,
                                     I.UNCONDITIONAL_AMOUNT,
                                     I.COMPANY_ID,
                                     (CASE WHEN I.IS_AGAIN > 0 THEN 'Təkrar' ELSE null END) AGAIN_DESCRIPTION,
                                     I.IS_CANCEL,
                                     I.POLICE,
                                     I.CAR_AMOUNT
                                FROM CRS_USER_TEMP.INSURANCES_TEMP I,
                                     CRS_USER.CURRENCY CUR,
                                     CRS_USER.INSURANCE_COMPANY IC
                               WHERE     IC.ID = I.COMPANY_ID
                                     AND I.CURRENCY_ID = CUR.ID
                                     AND I.INSURANCE_AMOUNT > 0
                                     AND I.IS_CHANGE <> 2
                                     AND I.CONTRACT_ID = {ContractID}
                            ORDER BY I.ID";
            else
                s = $@"SELECT 1 SS,
                                 I.ID,
                                 IC.NAME COMPANY_NAME,
                                 I.START_DATE,
                                 I.END_DATE,
                                 CUR.CODE CURRENCY_CODE,
                                 I.INSURANCE_AMOUNT,
                                 I.INSURANCE_PERIOD,
                                 I.INSURANCE_INTEREST,
                                 I.UNCONDITIONAL_AMOUNT,         
                                 I.COMPANY_ID,
                                 (CASE WHEN I.IS_AGAIN > 0 THEN 'Təkrar' ELSE null END) AGAIN_DESCRIPTION,
                                 I.IS_CANCEL,
                                 I.POLICE,
                                 I.CAR_AMOUNT
                            FROM CRS_USER.INSURANCES I,         
                                 CRS_USER.CURRENCY CUR,
                                 CRS_USER.INSURANCE_COMPANY IC
                           WHERE     IC.ID = I.COMPANY_ID         
                                 AND I.CURRENCY_ID = CUR.ID
                                 AND I.INSURANCE_AMOUNT > 0
                                 AND I.CONTRACT_ID = {ContractID}
                        ORDER BY I.ID";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceDataGridView");

            InsuranceGridControl.DataSource = dt;

            if (InsuranceGridView.RowCount > 0)
            {
                EditInsuranceBarButton.Enabled = GlobalVariables.EditInsurance;
                DeleteInsuranceBarButton.Enabled = GlobalVariables.DeleteInsurance;
                CancelInsuranceBarButton.Enabled = GlobalVariables.CancelInsurance;
                ViewInsuranceFileBarButton.Enabled =
                    CancelBarSubItem.Enabled = true;
            }
            else
                EditInsuranceBarButton.Enabled =
                    DeleteInsuranceBarButton.Enabled =
                    ViewInsuranceFileBarButton.Enabled =
                    CancelInsuranceBarButton.Enabled =
                    CancelBarSubItem.Enabled = false;

            DataView dv = new DataView();

            dv = new DataView(dt);
            dv.RowFilter = $@"IS_CANCEL = {1}";
            CancelInsuranceBarButton.Enabled = !(dv.Count > 0 && dt.Rows.Count == 1);
        }

        private void InsertCommitmensTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_COMMITMENTS_TEMP", "P_CONTRACT_ID", ContractID, "Öhdəliklər temp cədvələ daxil edilmədi.");
        }

        private void InsertInsuranceTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_INSURANCES_TEMP", "P_CONTRACT_ID", ContractID, "Sığortalar temp cədvələ daxil edilmədi.");
        }

        private void InsertPowerTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_POW_ATTORNEY_TEMP", "P_CONTRACT_ID", ContractID, "Etibarnamələr temp cədvələ daxil edilmədi.");
        }

        private void InsertInterestPenaltiesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_PENELTIES_TEMP", "P_CONTRACT_ID", ContractID, "Cərimə faizləri temp cədvələ daxil edilmədi.");
        }

        private void RefreshCommitmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCommitmentDataGridView();
        }

        private void CommitmentsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CommitmentsGridView.GetFocusedDataRow();
            if (row != null)
            {
                CommitmentID = row["ID"].ToString();
                CommitmentName = row["COMMITMENT_NAME"].ToString();
                CommitmentPersonTypeID = Convert.ToInt32(row["PERSON_TYPE_ID"].ToString());
            }
        }

        private void EditCommitmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CommitmentPersonTypeID == 1)
                LoadFCommitment("EDIT", CommitmentID);
            else
                LoadFJuridicalCommitment("EDIT", CommitmentID);
        }

        private void CommitmentsGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditCommitmentBarButton.Enabled) && (CommitmentStandaloneBarDockControl.Enabled))
            {
                if (CommitmentPersonTypeID == 1)
                    LoadFCommitment("EDIT", CommitmentID);
                else
                    LoadFJuridicalCommitment("EDIT", CommitmentID);
            }
        }

        private void LoadCommitmentFile()
        {
            GlobalProcedures.ShowWordFileFromDB($@"SELECT T.COMMITMENT_FILE FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP T WHERE T.ID = {CommitmentID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\CT_" + CommitmentName + ".doc",
                                                "COMMITMENT_FILE");
        }

        private void LoadPowerFile()
        {
            GlobalProcedures.ShowWordFileFromDB($@"SELECT T.POWER_FILE FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP T WHERE T.ID = {PowerID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text + "_Etibarname.docx",
                                                "POWER_FILE");
        }

        private void ViewCommitmentFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCommitmentFile();
        }

        private void DeleteCommitment()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş öhdəliyi silmək istəyirsiniz?", "Öhdəliyin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER_TEMP.PROC_COMMITMENT_TEMP_DELETE", "P_COMMITMENT_ID", CommitmentID, "Öhdəlik götürənin məlumatları temp cədvəllərdən silinmədi");
            }
        }

        private void DeleteCommitmentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCommitment();
            LoadCommitmentDataGridView();
        }

        private void PowerGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PowerGridView.GetFocusedDataRow();
            if (row != null)
            {
                PowerID = row["ID"].ToString();
                power_code = row["POWER_CODE"].ToString();
            }
        }

        private void EditPowerOfAttorneyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPowerOfAttorney("EDIT", PowerID);
        }

        private void PowerGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPowerOfAttorneyBarButton.Enabled && PowerOfAttorneyStandaloneBarDockControl.Enabled)
                LoadFPowerOfAttorney("EDIT", PowerID);
        }

        private void RefreshPowerOfAttorneyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPowerDataGridView();
        }

        private void ViewPowerFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPowerFile();
        }

        private void DeletePower()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş etibarnaməni silmək istəyirsiniz?", "Etibarnamənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteTwoQuery($@"UPDATE CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP SET IS_CHANGE = 2 WHERE ID = {PowerID} AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                 $@"DELETE CRS_USER_TEMP.POWER_OF_ATTORNEY_CODE_TEMP WHERE CODE = '{power_code}' AND CONTRACT_ID = {ContractID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                  "Etibarnamə temp cədvəldən silinmədi.",
                                                 this.Name + "/DeletePower");
            }
        }

        private void DeletePowerOfAttorneyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePower();
            LoadPowerDataGridView();
        }

        private string DocumentCode(int type_id)
        {
            string s = "01";
            int max_code = GlobalFunctions.GetMax($@"SELECT NVL(MAX(LTRIM(CODE,'0')),0) + 1 MAXCODE FROM CRS_USER.CONTRACT_DOCUMENTS WHERE DOCUMENT_TYPE_ID = {type_id} AND CONTRACT_ID = {ContractID}");
            s = max_code.ToString().PadLeft(2, '0');
            return s;
        }

        private void InsertContractDocument()
        {
            string file_name = null, code = "01", sql = null;
            if (GlobalVariables.WordDocumentUsed)
            {
                GlobalProcedures.SplashScreenClose();
                XtraMessageBox.Show("Açıq olan bütün word fayllar avtomatik olaraq bağlanılacaq.", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GlobalProcedures.KillWord();
                GlobalVariables.WordDocumentUsed = false;
            }

            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));

            #region muqavile
            if (contract_click && File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Müqavilə.docx"))
            {

                file_name = Path.GetFileName(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Müqavilə.docx");
                code = DocumentCode(1);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                                        VALUES({ContractID},
                                                1,
                                                :BlobFile,
                                                '{code}',
                                                '{file_name}')";

                GlobalFunctions.ExecuteQueryWithBlob(sql, GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Müqavilə.docx",
                                                        "Müqavilən hazır çap faylı bazaya daxil edilmədi.");

            }
            #endregion

            #region alqi-satqi
            if (leasing_object_contract_click && File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Alqı-satqı.docx"))
            {
                file_name = Path.GetFileName(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Alqı-satqı.docx");
                code = DocumentCode(2);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                                   VALUES({ContractID},2,:BlobFile,'{code}','{file_name}')";
                GlobalFunctions.ExecuteQueryWithBlob(sql, GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Alqı-satqı.docx",
                                                        "Alqı-satqının hazır çap faylı bazaya daxil edilmədi.");

            }
            #endregion

            #region daimi senedler
            if (document_click && File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daimi_Sənədlər.doc"))
            {
                file_name = Path.GetFileName(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daimi_Sənədlər.doc");
                code = DocumentCode(2);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                             VALUES({ContractID},3,:BlobFile,'{code}','{file_name}')";
                GlobalFunctions.ExecuteQueryWithBlob(sql, GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daimi_Sənədlər.doc",
                                                        "Daimi sənədlərin hazır çap faylı bazaya daxil edilmədi.");
            }
            #endregion

            #region dasinmaz emlak
            if (real_estate && File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daşınmaz Əmlak.docx"))
            {
                file_name = Path.GetFileName(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daşınmaz Əmlak.docx");
                code = DocumentCode(6);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                                      VALUES({ContractID},6,:BlobFile,'{code}','{file_name}')";
                GlobalFunctions.ExecuteQueryWithBlob(sql, GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daşınmaz Əmlak.docx",
                                                        "Daşınmaz Əmlakın hazır çap faylı bazaya daxil edilmədi.");
            }
            #endregion
        }

        private void LoadFilesDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 CD.ID,
                                 DT.TYPE_NAME,
                                 CD.CODE || ' - ' || CD.FILE_NAME FILE_NAME,
                                 CD.ETL_DT_TM ETL_DATE
                            FROM CRS_USER.CONTRACT_DOCUMENTS CD, CRS_USER.DOCUMENT_TYPE DT
                           WHERE CD.DOCUMENT_TYPE_ID = DT.ID AND CD.CONTRACT_ID = {ContractID}
                        ORDER BY DT.ID, CD.CODE";
            FilesGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFilesDataGridView");

            if (FilesGridView.RowCount > 0)
            {
                ViewFileBarButton.Enabled = true;
                DeleteFileBarButton.Enabled = !CurrentStatus;
            }
            else
                ViewFileBarButton.Enabled = DeleteFileBarButton.Enabled = false;
        }

        private void LoadFile()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.DOCUMENT_FILE,T.FILE_NAME,T.CODE FROM CRS_USER.CONTRACT_DOCUMENTS T WHERE T.ID = {DocumentID}", this.Name + "/LoadFile");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["DOCUMENT_FILE"]))
                {
                    Byte[] BLOBData = (byte[])dr["DOCUMENT_FILE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    GlobalProcedures.DeleteFile(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + dr["CODE"] + " - " + dr["FILE_NAME"]);
                    FileStream fs = new FileStream(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + dr["CODE"] + " - " + dr["FILE_NAME"], FileMode.Create, FileAccess.Write);
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                    Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + dr["CODE"] + " - " + dr["FILE_NAME"]);
                }
            }
        }

        private void FilesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = FilesGridView.GetFocusedDataRow();
            if (row != null)
                DocumentID = row["ID"].ToString();
        }

        private void ViewFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFile();
        }

        private void DeleteFile()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş faylı silmək istəyirsiniz? Seçilmiş faylları sildikdən sonra onları geri qaytarmaq olmayacaq.", "Faylların silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                ArrayList rows = new ArrayList();
                rows.Clear();
                for (int i = 0; i < FilesGridView.SelectedRowsCount; i++)
                {
                    rows.Add(FilesGridView.GetDataRow(FilesGridView.GetSelectedRows()[i]));
                }

                if (rows.Count > 0)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataRow row = rows[i] as DataRow;
                        GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CONTRACT_DOCUMENTS WHERE CONTRACT_ID = {ContractID} AND ID = {row["ID"]}", "Fayl cədvəldən silinmədi.", this.Name + "/DeleteFile");
                    }
                }
                else
                    XtraMessageBox.Show("Silmək istədiyiniz faylları seçin.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteFile();
            LoadFilesDataGridView();
        }

        private void FilesGridView_DoubleClick(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void PaymentTypeRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PaymentTypeRadioGroup.SelectedIndex == 1)
                MonthlyPaymentValue.Properties.ReadOnly = !(pay_count == 0);
            else
            {
                int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, Convert.ToDouble(InterestText.Text));
                MonthlyPaymentValue.Value = (decimal)monthly_amount;
                MonthlyPaymentValue.Properties.ReadOnly = true;
            }
            MonthlyPaymentValue.TabStop = !MonthlyPaymentValue.Properties.ReadOnly;
        }

        private void SellerNumberText_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerNumberText.Text.Trim().Length == 0)
                NumberLengthLabel.Visible = false;
            else
            {
                NumberLengthLabel.Visible = true;
                NumberLengthLabel.Text = SellerNumberText.Text.Trim().Length.ToString();
            }
        }

        private void SellerPinCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (SellerPinCodeText.Text.Trim().Length == 0)
                PinCodeLengthLabel.Visible = false;
            else
            {
                PinCodeLengthLabel.Visible = true;
                PinCodeLengthLabel.Text = SellerPinCodeText.Text.Trim().Length.ToString();
            }
        }

        void RefreshCode(string code, bool close)
        {
            string s = null; //GlobalFunctions.GetName($@"SELECT CODE FROM CRS_USER.CREDIT_TYPE WHERE ID = {credit_type_id}").Trim();
            changecode = close;
            if (close)
                ContractCodeText.Text = (s + code).Trim();
            CreateAccounts();
        }

        private void BChangeCode_Click(object sender, EventArgs e)
        {
            Customer.FChangeCode fcc = new Customer.FChangeCode();
            fcc.type = 2;
            fcc.name_id = credit_name_id;
            fcc.RefreshCode += new Customer.FChangeCode.DoEvent(RefreshCode);
            fcc.ShowDialog();
        }

        private void InterestCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (InterestCheckEdit.Checked)
            {
                if (pay_count == 0)
                    InterestText.Properties.ReadOnly = CurrentStatus;
                else
                    InterestText.Properties.ReadOnly = true;
                if (!CurrentStatus)
                    InterestText.Focus();
                if (check_interest_value > -1)
                    InterestText.Text = check_interest_value.ToString();
            }
            else
            {
                InterestText.Text = default_interest.ToString();
                InterestText.Properties.ReadOnly = true;
            }
            InterestText.TabStop = !InterestText.Properties.ReadOnly;
        }

        void SelectionImage(string a, int count)
        {
            if (!String.IsNullOrEmpty(a) && File.Exists(a))
            {
                SellerPictureBox.Image = Image.FromFile(a);
                SellerImage = a;
                BDeleteSellerPicture.Enabled = true;
            }
            crop_image_count = count;
        }

        private void SellerPictureBox_DoubleClick(object sender, EventArgs e)
        {
            SellerPictureBox.Image = null;
            FImageCrop crop = new FImageCrop();
            crop.PictureOwner = "S" + SellerID;
            crop.count = crop_image_count;
            crop.SelectionImage += new FImageCrop.DoEvent(SelectionImage);
            crop.ShowDialog();
        }

        private void FilesGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (FilesGridView.RowCount > 0)
            {
                ViewFileBarButton.Enabled = true;
                DeleteFileBarButton.Enabled = !CurrentStatus;
            }
            else
                ViewFileBarButton.Enabled = DeleteFileBarButton.Enabled = false;
        }

        private void PowerGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditPowerOfAttorneyBarButton.Enabled = DeletePowerOfAttorneyBarButton.Enabled = ViewPowerFileBarButton.Enabled = (PowerGridView.RowCount > 0);
        }

        private void CommitmentsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditCommitmentBarButton.Enabled =
                DeleteCommitmentBarButton.Enabled =
                ViewCommitmentFileBarButton.Enabled =
                CommitmentPersonInfoBarButton.Enabled = (CommitmentsGridView.RowCount > 0 && !CurrentStatus);
        }

        private void BReceipt_Click(object sender, EventArgs e)
        {
            GlobalProcedures.LoadReceipt(ContractStartDate.Text, (double)FirstPaymentValue.Value, currency_id, 1, CustomerFullNameText.Text.Trim(), int.Parse(ContractID), ContractCodeText.Text.Trim(), "saylı linzq müqaviləsi üzrə avans ödənişi");
        }

        private void BRealEstate_Click(object sender, EventArgs e)
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Daşınmaz Əmlak.docx"))
            {
                real_estate = GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Daşınmaz Əmlak faylı tapılmadı.");
                return;
            }
            code_number = int.Parse(Regex.Replace(ContractCodeText.Text, "[^0-9]", ""));
            string day = String.Format("{0:dd}", GlobalFunctions.ChangeStringToDate(DateTime.Today.Day.ToString(), "ddmmyyyy")),
                month = GlobalFunctions.FindMonth(DateTime.Today.Month),
                year = GlobalFunctions.FindYear(DateTime.Today),
                customername, commitment_name = null;
            commitment_name = GlobalFunctions.GetName("SELECT CC.COMMITMENT_NAME FROM CRS_USER.V_COMMITMENTS CC WHERE CC.CONTRACT_ID = " + ContractID);
            if (String.IsNullOrEmpty(commitment_name))
                customername = CustomerFullNameText.Text.Trim();
            else
                customername = commitment_name;

            try
            {
                Document document = new Document();
                document.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\Daşınmaz Əmlak.docx");
                document.ReplaceText("[$currentdate]", day + " " + month + " " + year);
                document.ReplaceText("[$date_and_contractnumber]", GlobalFunctions.FindDateSuffix(ContractStartDate.DateTime) + " tarixli " + ContractCodeText.Text.Trim());
                document.ReplaceText("[$address]", ObjectAddressText.Text.Trim());
                document.ReplaceText("[$objectexcerpt]", ObjectExcerptText.Text.Trim());
                document.ReplaceText("[$contractnumber] ", ContractCodeText.Text.Trim() + " ");
                document.ReplaceText("[$customer]", customername + " ");

                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daşınmaz Əmlak.docx"))
                    File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daşınmaz Əmlak.docx");
                document.Save(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daşınmaz Əmlak.docx");
                GlobalVariables.WordDocumentUsed = true;
                real_estate = true;
                Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Daşınmaz Əmlak.docx");

            }
            catch
            {
                real_estate = false;
                XtraMessageBox.Show(code_number + "_Daşınmaz Əmlak.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
        }

        private void BPolice_Click(object sender, EventArgs e)
        {
            string customername = null, card = null, commitment_name = null;

            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT CC.COMMITMENT_NAME NAME,
                                                                       CS.SERIES
                                                                       || ' '
                                                                       || CC.CARD_NUMBER
                                                                       || ' '
                                                                       || CI.NAME
                                                                       || ' tərəfindən '
                                                                       || TO_CHAR(CC.CARD_ISSUE_DATE, 'DD.MM.YYYY')
                                                                       || ' tarixində verilib.'
                                                                          CARD
                                                                  FROM CRS_USER.V_COMMITMENTS CC,
                                                                       CRS_USER.CARD_SERIES CS,
                                                                       CRS_USER.CARD_ISSUING CI
                                                                 WHERE     CC.CARD_SERIES_ID = CS.ID
                                                                       AND CC.CARD_ISSUING_ID = CI.ID
                                                                       AND CC.CONTRACT_ID = {ContractID}", this.Name + "/BPolice_Click");
            if (dt.Rows.Count > 0)
                commitment_name = dt.Rows[0]["NAME"].ToString();

            if (String.IsNullOrEmpty(commitment_name))
            {
                customername = CustomerFullNameText.Text.Trim();
                card = CardDescriptionText.Text.Trim() + " " + IssuingText.Text.Trim() + " tərəfindən " + IssuingDateText.Text + " tarixində verilib.";
            }
            else
            {
                customername = commitment_name;
                card = dt.Rows[0]["CARD"].ToString();
            }

            FPolice fp = new FPolice();
            fp.ContractCode = ContractCodeText.Text.Trim();
            fp.Customer = customername;
            fp.CarNumber = CarNumberText.Text.Trim();
            fp.Car = BrandComboBox.Text + " " + ModelComboBox.Text;
            fp.Card = card;
            fp.DateAndContract = ContractStartDate.Text + " il tarixli " + ContractCodeText.Text.Trim();
            fp.CarWihtoutBan = BrandComboBox.Text;
            fp.PObject = "Tip - " + BanTypeComboBox.Text + ", Rəngi - " + ColorComboBox.Text + ", Buraxılış ili - " + YearValue.Value.ToString() + ", BAN " + BanText.Text.Trim();
            fp.ObjectCount = "1";
            fp.Type = TypeComboBox.Text;
            fp.BanType = BanTypeComboBox.Text;
            fp.Year = YearValue.Value.ToString();
            fp.Ban = BanText.Text.Trim();
            fp.PColor = ColorComboBox.Text;
            fp.Brand = BrandComboBox.Text;
            fp.EngineNumber = EngineNumberText.Text.Trim();
            fp.ChassisNumber = ChassisText.Text.Trim();
            fp.ShowDialog();
        }

        void RefreshRate(decimal a, bool c, int currencyid)
        {
            credit_currency_rate = (double)a;
            rate = credit_currency_rate;
            is_contract_rate_change = c;
            contract_rate_currency_id = currencyid;
            CreditAmountValue.Enabled = (pay_count == 0 && !CurrentStatus);
            LiquidValue.Enabled = (pay_count == 0 && !CurrentStatus);
            FirstPaymentValue.Enabled = (pay_count == 0 && !CurrentStatus);
            BOK.Visible = !CurrentStatus;
        }

        private void CreditCurrencyRateLabel_DoubleClick(object sender, EventArgs e)
        {
            if (GlobalVariables.ChangeCurrencyRate)
            {
                if (pay_count == 0)
                {
                    FChangeContractExchange fcce = new FChangeContractExchange();
                    fcce.CurrencyID = currency_id.ToString();
                    fcce.CreditCurrencyID = credit_currency_id.ToString();
                    fcce.Rate = (decimal)credit_currency_rate;
                    fcce.ContractID = ContractID;
                    fcce.TransactionName = TransactionName;
                    fcce.RefreshRate += new FChangeContractExchange.DoEvent(RefreshRate);
                    fcce.ShowDialog();
                    GenerateCurrencyRateLabel();
                    CreditAmountValue_EditValueChanged(sender, EventArgs.Empty);
                }
            }
            else
                XtraMessageBox.Show(GlobalVariables.V_UserName + " adlı istifadəçinin müqavilənin məzənnəsini dəyişdirmək üçün hüququ yoxdur.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CalcPaymentSchedulesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sheduleVersion == 0)
                CalculatedPaymentScheduleTemp(true);
            else
                CalcExtendPaymentShedules();
            LoadPaymentScheduleDataGridView();
        }

        private void UpPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "up", out orderid);
            LoadPhoneDataGridView(SellerID);
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "down", out orderid);
            LoadPhoneDataGridView(SellerID);
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void InterestPenaltiesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (!CurrentStatus)
                GlobalProcedures.GridMouseUpForPopupMenu(InterestPenaltiesGridView, InterestPenaltiesPopupMenu, e);
        }

        private void LoadInterestPenaltiesDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                     ID,
                                     CALC_DATE,
                                     INTEREST,
                                     NOTE,
                                     IS_DEFAULT
                                FROM CRS_USER_TEMP.INTEREST_PENALTIES_TEMP
                               WHERE IS_CHANGE IN (0, 1) AND CONTRACT_ID = {ContractID}
                            ORDER BY CALC_DATE";

            InterestPenaltiesGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInterestPenaltiesDataGridView");

            if (InterestPenaltiesGridView.RowCount > 0)
            {
                EditInterestPenaltiesBarButton.Enabled = GlobalVariables.EditInterestPenalties;
                DeleteInterestPenaltiesBarButton.Enabled = GlobalVariables.DeleteInterestPenalties;
            }
            else
                EditInterestPenaltiesBarButton.Enabled = DeleteInterestPenaltiesBarButton.Enabled = false;
        }

        private void RefreshInterestPenaltiesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadInterestPenaltiesDataGridView();
        }

        void RefreshInterest(double a, double b)
        {
            LoadInterestPenaltiesDataGridView();
        }

        private void LoadFInterestAddEdit(string transaction, string contractid, string interestid, string startdate)
        {
            topindex = InterestPenaltiesGridView.TopRowIndex;
            old_row_num = InterestPenaltiesGridView.FocusedRowHandle;
            FInterestAddEdit fiae = new FInterestAddEdit();
            fiae.TransactionName = transaction;
            fiae.ContractID = contractid;
            fiae.InterestID = interestid;
            fiae.StartDate = startdate;
            fiae.RefreshInterestDataGridView += new FInterestAddEdit.DoEvent(RefreshInterest);
            fiae.ShowDialog();
            InterestPenaltiesGridView.TopRowIndex = topindex;
            InterestPenaltiesGridView.FocusedRowHandle = old_row_num;
        }

        private void NewInterestPenaltiesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInterestAddEdit("INSERT", ContractID, null, ContractStartDate.Text);
        }

        private void InterestPenaltiesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = InterestPenaltiesGridView.GetFocusedDataRow();
            if (row != null)
                InterestID = row["ID"].ToString();
        }

        private void EditInterestPenaltiesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            penalty_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACT_CALCULATED_PENALTIES WHERE INTEREST_PENALTIES_ID = " + InterestID);
            if (penalty_count == 0)
                LoadFInterestAddEdit("EDIT", ContractID, InterestID, ContractStartDate.Text);
            else
                XtraMessageBox.Show("Seçdiyiniz cərimə faizinə görə lizinq portfelində hesablınmış faizlər var. Buna görə də bu cərimə faizini lizinq portfelindən dəyişmək olar.");
        }

        private void InterestPenaltiesGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditInterestPenaltiesBarButton.Enabled && InterestPenaltiesStandaloneBarDockControl.Enabled)
            {
                penalty_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACT_CALCULATED_PENALTIES WHERE INTEREST_PENALTIES_ID = " + InterestID);
                if (penalty_count == 0)
                    LoadFInterestAddEdit("EDIT", ContractID, InterestID, ContractStartDate.Text);
                else
                    XtraMessageBox.Show("Seçdiyiniz cərimə faizinə görə lizinq portfelində hesablınmış faizlər var. Buna görə də bu cərimə faizini lizinq portfelindən dəyişmək olar.");
            }
        }

        private void PrintInterestPenaltiesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InterestPenaltiesGridControl);
        }

        private void ExportInterestPenaltiesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InterestPenaltiesGridControl, "xls");
        }

        private void DeleteInterestPenaltiesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACT_CALCULATED_PENALTIES WHERE INTEREST_PENALTIES_ID = " + InterestID);
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş cərimə faizini silmək istəyirsiniz?", "Faizin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.INTEREST_PENALTIES_TEMP SET IS_CHANGE = 2 WHERE ID = " + InterestID,
                                                        "Müqavilənin cərimə faizi temp cədvəldən silinmədi.",
                                                   this.Name + "/DeleteInterestPenaltiesBarButton_ItemClick");
                    LoadInterestPenaltiesDataGridView();
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş cərimə faizinə görə lizinq portfelində hesablınmış faizlər var. Buna görə də bu faizi silmək olmaz.");
        }

        private void BJournal_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalProcedures.InsertOperationJournalForSeller(ContractStartDate.Text, (double)LiquidValue.Value, (double)FirstPaymentValue.Value, ContractID, liquid_account, first_payment_account);
                XtraMessageBox.Show(ContractCodeText.Text + " saylı lizinq müqaviləsinin ödənişləri mühasibatlığın jurnalına daxil edildi. ");
            }
            catch (Exception exx)
            {
                XtraMessageBox.Show(exx.Message);
            }
            finally
            {
                ControlJournal();
            }
        }

        private void ControlJournal()
        {
            if (GlobalVariables.V_UserID > 0)
                BJournal.Visible = (Commit == 1 && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.OPERATION_JOURNAL WHERE ACCOUNT_OPERATION_TYPE_ID = 2 AND CONTRACT_ID = {ContractID}") == 0);
            else
                BJournal.Visible = true;
        }

        private void LiquidTypeRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LiquidTypeRadioGroup.SelectedIndex == 0)
            {
                LiquidBankComboBox.Visible = false;
                liquid_account = "";
                //liquid_bank_id = liquid_plan_id = 0;
            }
            else
            {
                LiquidBankComboBox.Visible = true;
                GlobalProcedures.FillComboBoxEdit(LiquidBankComboBox, "V_PAYMENT_BANKS", "NAME,NAME,NAME", null);
                LiquidBankComboBox.SelectedIndex = 0;
            }
        }

        private void FirstPaymentTypeRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FirstPaymentTypeRadioGroup.SelectedIndex == 0)
            {
                FirstPaymentBankComboBox.Visible = false;
                first_payment_account = "";
                //first_payment_bank_id = first_payment_plan_id = 0;
            }
            else
            {
                FirstPaymentBankComboBox.Visible = true;
                GlobalProcedures.FillComboBoxEdit(FirstPaymentBankComboBox, "V_PAYMENT_BANKS", "NAME,NAME,NAME", null);
                FirstPaymentBankComboBox.SelectedIndex = 0;
            }
        }

        private void LiquidBankComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = LiquidBankComboBox.Text;
            string[] words = s.Split('/');
            liquid_account = words[1].Remove(0, 8).Trim();
            liquid_bank_id = GlobalFunctions.FindComboBoxSelectedValue("BANKS", "LONG_NAME", "ID", words[0].Trim());
            liquid_plan_id = GlobalFunctions.FindComboBoxSelectedValue("ACCOUNTING_PLAN", "BANK_ID = " + liquid_bank_id + " AND SUB_ACCOUNT", "ID", liquid_account);
        }

        private void FirstPaymentBankComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = FirstPaymentBankComboBox.Text;
            string[] words = s.Split('/');
            first_payment_account = words[1].Remove(0, 8).Trim();
            first_payment_bank_id = GlobalFunctions.FindComboBoxSelectedValue("BANKS", "LONG_NAME", "ID", words[0].Trim());
            first_payment_plan_id = GlobalFunctions.FindComboBoxSelectedValue("ACCOUNTING_PLAN", "BANK_ID = " + first_payment_bank_id + " AND SUB_ACCOUNT", "ID", first_payment_account);
        }

        private void SellerType()
        {
            JuridicalPersonVoenLabel.Visible =
                JuridicalPersonVoenText.Visible =
                JuridicalPersonNameLabel.Visible =
                JuridicalPersonNameText.Visible =
                JuridicalPersonAddressLabel.Visible =
                JuridicalPersonAddressText.Visible =
                LeadingPersonNameLabel.Visible =
                LeadingPersonNameText.Visible =
                labelControl4.Visible =
                labelControl5.Visible =
                labelControl6.Visible =
                labelControl12.Visible = !(SellerTypeRadioGroup.SelectedIndex == 0);

            SellerSurnameLabel.Visible =
                SellerSurnameText.Visible =
                SellerNameLabel.Visible =
                SellerNameText.Visible =
                SellerPatronymicLabel.Visible =
                SellerPatronymicText.Visible =
                SellerSeriesLookUp.Visible =
                SellerSeriesLabel.Visible =
                SellerNumberLabel.Visible =
                SellerNumberText.Visible =
                SellerPinCodeLabel.Visible =
                SellerPinCodeText.Visible =
                NumberLengthLabel.Visible =
                PinCodeLengthLabel.Visible =
                SellerIssuingDateLabel.Visible =
                SellerIssueDate.Visible =
                SellerFrontFaceButtonEdit.Visible =
                SellerRearFaceButtonEdit.Visible =
                SellerAddressLabel.Visible =
                SellerAddressText.Visible =
                SellerRegistrationAddressLabel.Visible =
                SellerRegistrationAddressText.Visible =
                labelControl1.Visible =
                labelControl2.Visible =
                labelControl18.Visible =
                SellerSeriesStarLabel.Visible =
                SellerNumberStarLabel.Visible =
                SellerIssueStarLabel.Visible =
                SellerIssuingStarLabel.Visible =
                SellerRegistrationAddressStarLabel.Visible =
                SellerIssuingLookUp.Visible =
                SellerIssuingLabel.Visible =
                SellerFrontFaceLabel.Visible =
                SellerRearFaceLabel.Visible =
                ExcampleCardLabel.Visible = (SellerTypeRadioGroup.SelectedIndex == 0);

            BSearcSeller.Visible = (SellerTypeRadioGroup.Enabled || TransactionName == "INSERT");

            if (SellerTypeRadioGroup.SelectedIndex == 0)
            {
                GlobalProcedures.FillLookUpEdit(SellerSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                GlobalProcedures.FillLookUpEdit(SellerPowerSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                if (TransactionName == "INSERT")
                    SellerSeriesLookUp.EditValue = SellerSeriesLookUp.Properties.GetKeyValueByDisplayText("AZE");
                GlobalProcedures.FillLookUpEdit(SellerIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                GlobalProcedures.FillLookUpEdit(SellerPowerIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                if (new_individual_seller_id == null)
                    new_individual_seller_id = GlobalFunctions.GetOracleSequenceValue("SELLER_SEQUENCE").ToString();
                SellerID = new_individual_seller_id;

                PhoneGroupControl.Location = new Point(155, 314);
                SellerSurnameText.Focus();
                seller_type = "S";
                seller_type_id = 1;
                BLoadSellerPicture.Visible = BDeleteSellerPicture.Visible = SellerPictureBox.Enabled = true;
            }
            else
            {
                if (new_juridical_seller_id == null)
                    new_juridical_seller_id = GlobalFunctions.GetOracleSequenceValue("JURIDICAL_PERSONS_SEQUENCE").ToString();
                SellerID = new_juridical_seller_id;
                seller_type = "JP";
                seller_type_id = 2;
                JuridicalPersonNameText.Focus();
                BLoadSellerPicture.Visible = BDeleteSellerPicture.Visible = SellerPictureBox.Enabled = false;
                JuridicalPersonNameText.Location = new Point(155, 50);
                JuridicalPersonNameLabel.Location = new Point(12, 53);
                labelControl4.Location = new Point(143, 53);
                JuridicalPersonVoenText.Location = new Point(155, 76);
                JuridicalPersonVoenLabel.Location = new Point(12, 79);
                labelControl5.Location = new Point(143, 80);
                LeadingPersonNameText.Location = new Point(155, 102);
                LeadingPersonNameLabel.Location = new Point(12, 105);
                labelControl6.Location = new Point(143, 106);
                JuridicalPersonAddressText.Location = new Point(155, 128);
                JuridicalPersonAddressLabel.Location = new Point(12, 131);
                PhoneGroupControl.Location = new Point(155, 155);
                labelControl12.Location = new Point(143, 131);
            }

            CreateAccounts();
        }
    }
}