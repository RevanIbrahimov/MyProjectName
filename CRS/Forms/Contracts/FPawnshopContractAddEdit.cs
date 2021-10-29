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
using CRS.Class.Views;
//using CRS.Class2;
//using CRS.Class2.Views;
using Oracle.ManagedDataAccess.Client;

using System.Reflection;
//using CRS.Class2.DataAccess;
//using CRS.Class2.Tables;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using DevExpress.XtraReports.UI;


//using Microsoft.Office.Interop.Word;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace CRS.Forms.Contracts
{
    public partial class FContractAddEdit : DevExpress.XtraEditors.XtraForm
    { 
        public FContractAddEdit()
        {
            InitializeComponent();

            System.Drawing.Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            WindowState = (Width > screen.Width || Height > screen.Height) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        public string TransactionName, ContractID, CustomerID, CustomerCode, GuaranteeID;
        public int Commit = 0, IsSpecialAttention;
        public bool IsExtend;

        string PhoneID,
            PhoneNumber,
            SellerImagePath,
            ContractImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages",
            IDImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\IDCardImages",
            ImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images",
            SellerImage,
            credit_currency_name,
            credit_currency_code,
            currency_short_name,
            card_series,
            card_number,
            seller_card_name,
            DocumentID,
            InterestID,
            seller_type = null,
            person_description = null,
            new_individual_seller_id = null,
            new_juridical_seller_id = null,
            oldContractID = null,
            commission_account = null,
            credit_amount_account = null,
            old_account = null;

        int credit_type_id = 0,
            appraiser_id = 0,
            credit_currency_id = 0,
            credit_currency_value = 0,
            old_month_count = 0,
            old_currency_id = 0,
            credit_name_id = 0,
            contract_images_count = 0,
            card_issuing_id = 0,
            card_series_id = 0,
            old_grace_period = 0,
            customer_card_id = 0,
            period = 0,
            ContractUsedUserID = -1,
            statusID,
            crop_image_count = 0,
            pay_count = 0,
            old_payment_type = -1,
            penalty_count = 0,
            customer_type_id = 0,
            seller_type_id = 0,
            old_seller_type_id = 0,
            old_seller_index = 0,
            customer_contract_number = 0,
            sheduleVersion,
            pawnID,
            loan_officer_id = 0,
            topindex,
            old_row_num;

        int? parent_contract_id = null;

        double old_contract_amount,
            monthly_amount = 0,
            old_monthly_amount = 0,
            credit_currency_rate = 0,
            old_credit_currency_rate = 0,
            rate = 0,
            debt,
            old_interest = 0;

        decimal sumPawnAmount = 0, sumWeight = 0, check_interest_value = -1, default_interest = 0, sumTotalAmount = 0;

        bool FormStatus = false,
            contract_click = false,
            pawn_contract_click = false,
            pawn_list_click = false,
            akt_click = false,
            consent_click = false,
            ContractUsed = false,
            CurrentStatus = false,
            ContractClosed = false,
            changecode = false,
            sellerdetails = false,
            real_estate = false;

        DateTime old_start_date;
        List<string> image_list = new List<string>();
        List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();

        public delegate void DoEvent(string contract_id);
        public event DoEvent RefreshContractsDataGridView;

        private void FContractAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                SellerSeriesLookUp.Properties.Buttons[1].Visible = GlobalVariables.CardSeries;
                SellerIssuingLookUp.Properties.Buttons[1].Visible = GlobalVariables.CardIssuing;

                BJournal.Enabled = GlobalVariables.AddCommitment;

                NewPawnBarButton.Enabled = GlobalVariables.AddPower;
                NewInterestPenaltiesBarButton.Enabled = GlobalVariables.AddInterestPenalties;
            }
            else
                GlobalVariables.CommitContract = true;
            ContractStartDate.Properties.MinValue = GlobalFunctions.LastClosedDay().AddDays(1);
            GlobalProcedures.DeleteAllFilesInDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents");
            RefreshDictionaries(7);
            RefreshDictionaries(12);
            RefreshDictionaries(14);
            GlobalProcedures.LookUpEditValue(CreditCurrencyLookUp, "AZN");
            GlobalProcedures.FillLookUpEditWithSqlText(CommissionAccountLookUp, "SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER IN (10010, 15010)", "ID", "NAME");
            GlobalProcedures.FillLookUpEditWithSqlText(CreditAmountAccountLookUp, "SELECT SUB_ACCOUNT ID, SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER IN (10010, 15010)", "ID", "NAME");
            EditCustomerLabel.Visible = false;
            if (TransactionName == "INSERT")
            {
                BChangeCode.Visible = GlobalVariables.EditContractCode;
                ContractID = GlobalFunctions.GetOracleSequenceValue("CONTRACT_SEQUENCE").ToString();
                ContractStartDate.EditValue = DateTime.Today;
                if (!String.IsNullOrEmpty(CustomerCode))
                    RegistrationCodeSearch.Text = CustomerCode;

                FormStatus = true;
            }
            else if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");

                LoadContractDetails();
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

                InsertPaymentSchedulesTemp();
                InsertPawnTemp();
                LoadCustomerDetails();
                CommitStatus();
                ComponentEnable(CurrentStatus);
                FormStatus = true;
                ControlJournal();
                oldContractID = ContractID;
            }

            old_contract_amount = (double)CreditAmountValue.Value;
        }

        private void CommitStatus()
        {
            if (Commit == 0 && !GlobalVariables.CommitContract)
                BOK.Visible = true;
            else if (Commit == 1 && !GlobalVariables.CommitContract)
            {
                BContract.Enabled =
                    BPawnContract.Enabled =
                    BPawnList.Enabled =
                    BPaymentSchedule.Enabled =
                    BConsent.Enabled =
                    BAct.Enabled =
                    BConsent.Enabled =
                    BDocumentPacket.Enabled =
                    BCashOrder.Enabled = !CurrentStatus;

                FilesTab.PageVisible = true;

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
                    BPawnContract.Enabled =
                    BPawnList.Enabled =
                    BPaymentSchedule.Enabled =
                    BConsent.Enabled =
                    BAct.Enabled =
                    BCashOrder.Enabled =
                    BDocumentPacket.Enabled =
                    BOK.Visible = !CurrentStatus;

                if (Commit == 1)
                    BConfirm.Visible = false;
                else if (TransactionName == "EDIT")
                    BConfirm.Visible = !CurrentStatus;
                DeleteFileBarButton.Enabled = !CurrentStatus;

                FilesTab.PageVisible = (Commit == 1);
            }
        }

        public void ComponentEnable(bool status)
        {
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
                    ContractStartDate.Enabled =
                    PeriodCheckEdit.Enabled =
                    CreditAmountValue.Enabled =
                    CreditCurrencyLookUp.Enabled =
                    InterestCheckEdit.Enabled =
                    CommissionValue.Enabled =
                    CommissionAccountLookUp.Enabled =
                    CreditAmountAccountLookUp.Enabled =
                    FifdValue.Enabled =
                    SellerTypeRadioGroup.Enabled = (pay_count == 0);
            }
            else
            {
                PhonePopupMenu.Manager = null;
                EditCustomerLabel.Visible = true;
                NewCustomerDropDownButton.Visible =
                    GracePeriodValue.Enabled =
                    PaymentTypeRadioGroup.Enabled =
                    ContractStartDate.Enabled =
                    PeriodCheckEdit.Enabled =
                    CreditAmountValue.Enabled =
                    CreditCurrencyLookUp.Enabled =
                    InterestCheckEdit.Enabled =
                    SellerTypeRadioGroup.Enabled = false;
            }
        }

        public void AgreementComponent()
        {
            ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths((int)PeriodValue.Value);
            ContractEndDate.Properties.MinValue = ContractStartDate.DateTime;

            PhonePopupMenu.Manager = PhoneBarManager;

            GracePeriodValue.Enabled =
                PaymentTypeRadioGroup.Enabled =
                ContractStartDate.Enabled =
                PeriodCheckEdit.Enabled =
                CreditAmountValue.Enabled =
                CreditCurrencyLookUp.Enabled =
                InterestCheckEdit.Enabled =
                SellerTypeRadioGroup.Enabled =
                BOK.Visible = true;

            PawnTab.PageVisible = false;
        }

        private void LoadContractDetails()
        {
            string s = $@"SELECT  C.CODE CONTRACT_CODE,
                               C.START_DATE,
                               C.END_DATE,
                               CT.TERM PERIOD,
                               CT.INTEREST,
                               C.GRACE_PERIOD,
                               C.AMOUNT,
                               C.FIFD,
                               C.COMMISSION,
                               CUR.CODE,                              
                               C.CHECK_END_DATE,
                               C.PAYMENT_TYPE,
                               C.MONTHLY_AMOUNT,
                               C.CHECK_INTEREST,
                               CT.ID CREDIT_TYPE_ID,                             
                               C.CHECK_PERIOD,
                               C.CURRENCY_RATE,
                               C.CUSTOMER_TYPE_ID,
                               CUS.CODE CUSTOMER_CODE,                               
                               C.USED_USER_ID,
                               C.STATUS_ID,
                               C.PARENT_ID,
                               (SELECT COUNT (*)
                                      FROM (SELECT CONTRACT_ID FROM CRS_USER.CUSTOMER_PAYMENTS
                                            UNION ALL
                                            SELECT CONTRACT_ID FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP)
                                     WHERE CONTRACT_ID = C.ID)
                                      PAY_COUNT,
                               A.NAME APPRAISER_NAME,
                               (SELECT SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE SUB_ACCOUNT = C.COMMISSION_ACCOUNT) COMMISSION_ACOUNT,
                               (SELECT SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE SUB_ACCOUNT = C.AMOUNT_ACCOUNT) AMOUNT_ACOUNT,
                               C.CUSTOMER_CONTRACT_NUMBER,
                               C.CONTRACT_ACCOUNT,
                               LO.NAME LOAN_OFFICER
                          FROM CRS_USER.CONTRACTS C,
                               CRS_USER.CREDIT_TYPE CT,
                               CRS_USER.CURRENCY CUR,
                               CRS_USER.V_CUSTOMERS CUS,
                               CRS_USER.APPRAISER A,
                               CRS_USER.LOAN_OFFICER LO
                         WHERE     C.CREDIT_TYPE_ID = CT.ID
                               AND C.CURRENCY_ID = CUR.ID
                               AND C.CUSTOMER_ID = CUS.ID
                               AND C.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                               AND C.APPRAISER_ID = A.ID
                               AND C.LOAN_OFFICER_ID = LO.ID
                               AND C.ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractDetails", "Müqavilənin məlumatları açılmadı.");

            if (dt.Rows.Count > 0)
            {
                ContractUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
                statusID = Convert.ToInt32(dt.Rows[0]["STATUS_ID"]);
                ContractClosed = (GlobalVariables.V_UserID > 0) ? (statusID == 6) : false;
                ContractCodeText.Text = dt.Rows[0]["CONTRACT_CODE"].ToString();
                ContractStartDate.EditValue = (TransactionName == "EDIT") ? dt.Rows[0]["START_DATE"] : DateTime.Today;
                ContractEndDate.EditValue = (TransactionName == "EDIT") ? dt.Rows[0]["END_DATE"] : DateTime.Today;
                period = Convert.ToInt32(dt.Rows[0]["PERIOD"].ToString());
                default_interest = Convert.ToInt32(dt.Rows[0]["INTEREST"].ToString());
                CreditAmountValue.EditValue = (TransactionName == "EDIT") ? dt.Rows[0]["AMOUNT"] : 0;
                FifdValue.EditValue = (TransactionName == "EDIT") ? dt.Rows[0]["FIFD"] : 0;
                CommissionValue.EditValue = (TransactionName == "EDIT") ? dt.Rows[0]["COMMISSION"] : 0;
                debt = Convert.ToDouble(dt.Rows[0]["AMOUNT"].ToString());
                credit_currency_code = dt.Rows[0]["CODE"].ToString();
                CreditCurrencyLookUp.EditValue = (TransactionName == "EDIT") ? CreditCurrencyLookUp.Properties.GetKeyValueByDisplayText(credit_currency_code) : null;

                if (Convert.ToInt32(dt.Rows[0]["CHECK_PERIOD"]) > 0)
                {
                    PeriodCheckEdit.Checked = true;
                    PeriodValue.EditValue = Convert.ToInt16(dt.Rows[0]["CHECK_PERIOD"]);
                }
                else
                {
                    PeriodValue.EditValue = Convert.ToInt16(dt.Rows[0]["PERIOD"]);
                    PeriodCheckEdit.Checked = false;
                    PeriodValue.Properties.ReadOnly = true;
                }

                PaymentTypeRadioGroup.SelectedIndex = (TransactionName == "EDIT") ? Convert.ToInt32(dt.Rows[0]["PAYMENT_TYPE"].ToString()) : 0;
                old_payment_type = Convert.ToInt32(dt.Rows[0]["PAYMENT_TYPE"].ToString());
                monthly_amount = (TransactionName == "EDIT") ? Convert.ToDouble(dt.Rows[0]["MONTHLY_AMOUNT"].ToString()) : 0;
                old_monthly_amount = monthly_amount;
                MonthlyPaymentValue.Value = (decimal)monthly_amount;
                if (Convert.ToDecimal(dt.Rows[0]["CHECK_INTEREST"]) > -1)
                {
                    InterestCheckEdit.Checked = true;
                    InterestValue.EditValue = Convert.ToDecimal(dt.Rows[0]["CHECK_INTEREST"]);
                    check_interest_value = Convert.ToInt32(dt.Rows[0]["CHECK_INTEREST"]);
                }
                else
                {
                    InterestValue.EditValue = Convert.ToDecimal(dt.Rows[0]["INTEREST"]);
                    InterestCheckEdit.Checked = false;
                    InterestValue.Properties.ReadOnly = true;
                }
                GracePeriodValue.Value = Convert.ToInt32(dt.Rows[0]["GRACE_PERIOD"]);
                credit_type_id = Convert.ToInt32(dt.Rows[0]["CREDIT_TYPE_ID"]);
                old_credit_currency_rate = Convert.ToDouble(dt.Rows[0]["CURRENCY_RATE"]);
                rate = (TransactionName == "EDIT") ? old_credit_currency_rate : 1;
                RegistrationCodeSearch.Text = dt.Rows[0]["CUSTOMER_CODE"].ToString();
                CreditCurrencyRateLabel.Visible = (credit_currency_id == 2);
                credit_currency_rate = rate;
                CreditCurrencyRateLabel.Text = credit_currency_value + " " + credit_currency_name + " = " + credit_currency_rate.ToString("N4") + " AZN";
                if (!String.IsNullOrWhiteSpace(dt.Rows[0]["PARENT_ID"].ToString()))
                    parent_contract_id = Convert.ToInt32(dt.Rows[0]["PARENT_ID"]);
                pay_count = Convert.ToInt32(dt.Rows[0]["PAY_COUNT"]);
                GlobalProcedures.LookUpEditValue(AppraiserLookUp, dt.Rows[0]["APPRAISER_NAME"].ToString());
                GlobalProcedures.LookUpEditValue(LoanOfficerLookUp, dt.Rows[0]["LOAN_OFFICER"].ToString());
                GlobalProcedures.LookUpEditValue(CommissionAccountLookUp, dt.Rows[0]["COMMISSION_ACOUNT"].ToString());
                GlobalProcedures.LookUpEditValue(CreditAmountAccountLookUp, dt.Rows[0]["AMOUNT_ACOUNT"].ToString());
                customer_contract_number = Convert.ToInt32(dt.Rows[0]["CUSTOMER_CONTRACT_NUMBER"]);
                old_account = dt.Rows[0]["CONTRACT_ACCOUNT"].ToString();
            }

            if (!PeriodCheckEdit.Checked)
                old_month_count = (int)PeriodValue.Value;
            else
                old_month_count = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);

            old_interest = (double)InterestValue.Value;

            old_start_date = GlobalFunctions.ChangeStringToDate(ContractStartDate.Text, "ddmmyyyy");
            old_grace_period = (int)GracePeriodValue.Value;

            RegistrationCodeSearch.Enabled = false;
        }

        private void LoadCustomerDetails()
        {
            if (RegistrationCodeSearch.Text.Length != 5)
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
                    BPawnContract.Enabled =
                    BConsent.Enabled =
                    BAct.Enabled =
                    BPaymentSchedule.Enabled =
                    BCashOrder.Enabled =
                    BDocumentPacket.Enabled =
                    BPawnList.Enabled = false;

                return;
            }

            string s = null;
            DataTable dt = null;

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
                               (SELECT NVL(MAX (CUSTOMER_CONTRACT_NUMBER), 0)
                                  FROM CRS_USER.CONTRACTS
                                 WHERE CUSTOMER_ID = C.ID)
                                  CUSTOMER_CONTRACT_NUMBER
                          FROM CRS_USER.V_CUSTOMERS C,
                               CRS_USER.V_CUSTOMER_CARDS_DETAILS CC,
                               CRS_USER.CUSTOMER_IMAGE CIM,
                               CRS_USER.PERSON_TYPE TP
                         WHERE     C.ID = CC.CUSTOMER_ID
                               AND C.PERSON_TYPE_ID = CC.PERSON_TYPE_ID
                               AND C.PERSON_TYPE_ID = TP.ID
                               AND C.ID = CIM.CUSTOMER_ID(+)
                               AND C.CODE ='{RegistrationCodeSearch.Text}'";
            dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCustomerDetails");

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

                    BContract.Enabled = BPawnContract.Enabled = BPawnList.Enabled = BPaymentSchedule.Enabled = BAct.Enabled = BConsent.Enabled = BCashOrder.Enabled = BDocumentPacket.Enabled = GlobalVariables.CommitContract;
                }

                RegistrationCodeSearch.Text = dt.Rows[0]["CUSTOMER_CODE"].ToString();
                CustomerFullNameText.Text = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                CardDescriptionText.Text = dt.Rows[0]["CARD_DESCRIPTION"].ToString();
                RegistrationAddressText.Text = dt.Rows[0]["ADDRESS"].ToString();
                IssuingDateText.Text = dt.Rows[0]["ISSUE_DATE"].ToString();
                ReliableDateText.Text = dt.Rows[0]["RELIABLE_DATE"].ToString();
                IssuingText.Text = dt.Rows[0]["CARD_ISSUING_NAME"].ToString();
                card_series = dt.Rows[0]["CARD_SERIES"].ToString();
                card_number = dt.Rows[0]["CARD_NUMBER"].ToString();
                CustomerID = dt.Rows[0]["CUSTOMER_ID"].ToString();
                customer_type_id = Convert.ToInt32(dt.Rows[0]["PERSON_TYPE_ID"]);
                customer_card_id = Convert.ToInt32(dt.Rows[0]["CARD_ID"]);
                CustomerPictureBox.Visible = (customer_type_id == 1);
                CustomerTypeLabel.Text = dt.Rows[0]["PERSON_TYPE_NAME"].ToString();
                person_description = dt.Rows[0]["PERSON_DESCRIPTION"].ToString();
                VoenText.Text = dt.Rows[0]["VOEN"].ToString();
                customer_contract_number = Convert.ToInt32(dt.Rows[0]["CUSTOMER_CONTRACT_NUMBER"]);
                if (!DBNull.Value.Equals(dt.Rows[0]["IMAGE"]))
                {
                    Byte[] BLOBData = (byte[])dt.Rows[0]["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    CustomerPictureBox.Image = Image.FromStream(stmBLOBData);
                    stmBLOBData.Close();
                }

                IssuingDateText.Visible =
                    DateOfIssueLabel.Visible =
                    ReliableDateText.Visible =
                    ReliableLabel.Visible =
                    IssuingAuthorityLabel.Visible =
                    IssuingText.Visible =
                    VoenLabel.Visible =
                    VoenText.Visible = (customer_type_id == 1);
            }
            else
            {
                RegistrationCodeSearch.Text =
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
                    BPawnContract.Enabled =
                    BConsent.Enabled =
                    BAct.Enabled =
                    BPaymentSchedule.Enabled =
                    BCashOrder.Enabled =
                    BDocumentPacket.Enabled =
                    BPawnList.Enabled = false;
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

        private void LoadSellerDetails()
        {
            if (TransactionName == "INSERT")
                return;

            FindSeller(int.Parse(GuaranteeID));
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
                               C.CARD_ISSUING_ID
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

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegistrationCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (TransactionName != "INSERT")
                return;

            LoadCustomerDetails();
            CreditTypeParametr();
            ContractCodeText.Text = InsertContractCodeTemp();
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
            if (RegistrationCodeSearch.Text.Length != 5 || !OtherInfoTabControl.Enabled)
                return;

            string s = $@"SELECT CT.ID,
                                   CT.TERM,
                                   CT.INTEREST
                              FROM CRS_USER.CREDIT_TYPE CT
                             WHERE CT.CALC_DATE =
                                      (SELECT MAX (CALC_DATE)
                                         FROM CRS_USER.CREDIT_TYPE
                                        WHERE     NAME_ID = CT.NAME_ID
                                              AND CALC_DATE <= TO_DATE ('{ContractStartDate.Text}', 'DD/MM/YYYY'))";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/CreditTypeParametr", "Lombardın rekvizitləri açılmadı.");

            if (dt.Rows.Count > 0)
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", -1, "WHERE USED_USER_ID = " + GlobalVariables.V_UserID);
                credit_type_id = Convert.ToInt32(dt.Rows[0]["ID"]);
                period = Convert.ToInt32(dt.Rows[0]["TERM"]);


                if (!InterestCheckEdit.Checked)
                {
                    InterestValue.EditValue = Convert.ToDecimal(dt.Rows[0]["INTEREST"]);
                    default_interest = Convert.ToInt32(dt.Rows[0]["INTEREST"]);
                }

                if (!PeriodCheckEdit.Checked)
                {
                    PeriodValue.EditValue = period;
                    ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths(period);
                    ContractEndDate.Properties.MinValue = ContractStartDate.DateTime;
                }

                CreditAmountValue.Enabled =
                       CreditCurrencyLookUp.Enabled = (pay_count == 0);

                SellerTabPage.PageEnabled =
                         PaymentScheduleTabPage.PageEnabled =
                         InterestPenaltiesTab.PageEnabled =
                         DescriptionTab.PageEnabled =
                         BOK.Visible = !CurrentStatus;
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", GlobalVariables.V_UserID, "WHERE ID = " + credit_type_id + " AND USED_USER_ID = -1");
            }
            else
            {
                XtraMessageBox.Show(ContractStartDate.Text + " tarixinə heç bir rekvizit təyin edilməyib.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                credit_type_id = 0;
                PeriodValue.EditValue =
                InterestValue.EditValue =
                ContractEndDate.Text = null;

                CreditAmountValue.Enabled =
                    CreditCurrencyLookUp.Enabled =
                    SellerTabPage.PageEnabled =
                    PaymentScheduleTabPage.PageEnabled =
                    InterestPenaltiesTab.PageEnabled =
                    DescriptionTab.PageEnabled =
                    BOK.Visible = false;

                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", -1, "WHERE USED_USER_ID = " + GlobalVariables.V_UserID);
            }
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 2:
                    GlobalProcedures.FillLookUpEdit(SellerSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 3:
                    GlobalProcedures.FillLookUpEdit(SellerIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 7:
                    GlobalProcedures.FillLookUpEdit(CreditCurrencyLookUp, "CURRENCY", "ID", "CODE", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 12:
                    GlobalProcedures.FillLookUpEdit(AppraiserLookUp, "APPRAISER", "ID", "NAME", "1 = 1 ORDER BY NAME");
                    break;
                case 14:
                    GlobalProcedures.FillLookUpEdit(LoanOfficerLookUp, "LOAN_OFFICER", "ID", "NAME", "1 = 1 ORDER BY NAME");
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

        private void InsertPaymentSchedulesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_SCHEDULES_TEMP", "P_CONTRACT_ID", ContractID, "Ödəniş qrafiki temp cədvələ daxil edilmədi.");
        }

        private void CalculatedPaymentScheduleTemp(bool isAgain)
        {
            if ((FormStatus) && (CreditAmountValue.Value > 0))
            {
                int month_count,
                    grace_period = 0,
                    is_change_date = 0;
                month_count = (int)PeriodValue.Value;

                double interest = (double)InterestValue.Value;

                DateTime d_start, realDate;
                DayOfWeek day_of_week;
                double interest_amount = 0,
                       basic_amount = 0,
                       beginningDebt = (double)CreditAmountValue.Value,
                       debt = (double)CreditAmountValue.Value,
                       monthly_amount = (double)MonthlyPaymentValue.Value;

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
                        interest_amount = Math.Round((((debt * (double)interest) / 100) / 360 * 30), 2);
                        monthly_amount = interest_amount;
                        basic_amount = 0;
                        //debt = debt;
                        GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                            MONTH_DATE,
                                                                                                            REAL_DATE,
                                                                                                            MONTHLY_PAYMENT,
                                                                                                            BASIC_AMOUNT,
                                                                                                            INTEREST_AMOUNT,
                                                                                                            BEGINNING_DEBT,
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
                                                                            {beginningDebt.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            {debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            {credit_currency_id},
                                                                            {GlobalVariables.V_UserID},
                                                                            1,
                                                                            {is_change_date},
                                                                            {j})",
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
                        interest_amount = Math.Round((((debt * (double)interest) / 100) / 360 * 30), 2);
                        if (i < month_count)
                            basic_amount = monthly_amount - interest_amount;
                        else
                        {
                            basic_amount = debt;
                            monthly_amount = debt + interest_amount;
                        }
                        beginningDebt = debt;
                        debt = debt - basic_amount;
                        GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                            MONTH_DATE,
                                                                                                            REAL_DATE,
                                                                                                            MONTHLY_PAYMENT,
                                                                                                            BASIC_AMOUNT,
                                                                                                            INTEREST_AMOUNT,
                                                                                                            BEGINNING_DEBT,
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
                                                                        {beginningDebt.ToString(GlobalVariables.V_CultureInfoEN)},
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

            DateTime d_start;
            DayOfWeek day_of_week;
            int is_change_date = 0, interest = extend.INTEREST, month_count = extend.MONTH_COUNT;
            double interest_amount = 0,
                basic_amount = 0,
                debt = (double)extend.DEBT,
                beginningDebt = (double)extend.DEBT,
                monthly_amount = (double)extend.MONTHLY_AMOUNT;

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
                beginningDebt = debt;
                debt = beginningDebt - basic_amount;

                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                  MONTH_DATE,
                                                                                                  MONTHLY_PAYMENT,
                                                                                                  BASIC_AMOUNT,
                                                                                                  INTEREST_AMOUNT,
                                                                                                  BEGINNING_DEBT,
                                                                                                  DEBT,
                                                                                                  CURRENCY_ID,
                                                                                                  USED_USER_ID,
                                                                                                  IS_CHANGE,
                                                                                                  IS_CHANGE_DATE,
                                                                                                  ORDER_ID,
                                                                                                  VERSION)
                                                          VALUES({ContractID},
                                                                  TO_DATE('{d_start.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                  {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {basic_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {interest_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {beginningDebt.ToString(GlobalVariables.V_CultureInfoEN)},
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
            if (OtherInfoTabControl.SelectedTabPageIndex == 3)
            {
                CalculatedPaymentScheduleTemp(false);
                LoadPaymentScheduleDataGridView();
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 2)
            {
                RefreshDictionaries(7);
                RefreshDictionaries(5);
                SellerScrollableControl.Enabled = BLoadSellerPicture.Enabled = !CurrentStatus;
                SellerType();
                SellerIssueDate.Properties.MaxValue = DateTime.Today;
                SellerSurnameText.Focus();
                LoadSellerDetails();
                if (!CurrentStatus)
                    InsertPhonesTemp();

                LoadPhoneDataGridView(GuaranteeID);
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 5)
                LoadFilesDataGridView();
            else if (OtherInfoTabControl.SelectedTabPageIndex == 1)
            {
                if (!CurrentStatus)
                    InsertPawnTemp();
                PawnStandaloneBarDockControl.Enabled = !CurrentStatus;
                LoadPawnDataGridView();
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
                ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkil";
            }
            else if (OtherInfoTabControl.SelectedTabPageIndex == 6)
            {
                if (!CurrentStatus && TransactionName == "EDIT")
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
                                     P.BEGINNING_DEBT,
                                     P.DEBT,
                                     C.CODE CURRENCY_CODE,
                                     P.IS_CHANGE_DATE,
                                     P.VERSION,
                                     NVL(A.CONTRACT_AMOUNT,{CreditAmountValue.Value}) CONTRACT_AMOUNT                                     
                                FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP P,
                                     CRS_USER.CURRENCY C,
                                     CRS_USER_TEMP.V_CONTRACT_AMOUNT_TEMP A
                               WHERE     P.CURRENCY_ID = C.ID
                                     AND P.CONTRACT_ID = A.CONTRACT_ID(+)
                                     AND P.VERSION = A.VERSION(+)
                                     AND P.IS_CHANGE != 2
                                     AND P.CONTRACT_ID = {ContractID}
                            ORDER BY P.VERSION, P.ORDER_ID";

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
            GlobalProcedures.GridCustomDrawFooterCell(Shedule_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("MONTHLY_PAYMENT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
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
            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Ödəniş qrafiki.docx");
            if (!File.Exists(fileName.ToString()))
            {
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Ödəniş qrafikinin şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_OdenisQrafiki.docx",
                   date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime);
            int payDay = ContractStartDate.DateTime.Day;
            try
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));

                OtherInfoTabControl.SelectedTabPageIndex = 6;

                object missing = Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();

                Microsoft.Office.Interop.Word.Table table = aDoc.Tables[2];

                string sql = $@"SELECT P.ORDER_ID SS,
                                         TO_CHAR(P.MONTH_DATE,'DD.MM.YYYY') MONTH_DATE,
                                         P.MONTHLY_PAYMENT,
                                         P.BASIC_AMOUNT,
                                         P.INTEREST_AMOUNT,
                                         P.BEGINNING_DEBT,
                                         P.DEBT,
                                         C.CODE CURRENCY_CODE
                                    FROM CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP P,
                                         CRS_USER.CURRENCY C,
                                         CRS_USER_TEMP.V_CONTRACT_AMOUNT_TEMP A
                                   WHERE     P.CURRENCY_ID = C.ID
                                         AND P.CONTRACT_ID = A.CONTRACT_ID(+)
                                         AND P.VERSION = A.VERSION(+)
                                         AND P.IS_CHANGE != 2
                                         AND P.CONTRACT_ID = {ContractID}
                                ORDER BY P.VERSION, P.ORDER_ID";
                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/BPaymentSchedule_Click", "Qrafik açılmadı.");
                int i = 5, r = 1;
                decimal totalMonthlyAmount = 0, sumPercent = 0, sumBasic = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    table.Rows.Add(ref missing);
                    table.Cell(i, 1).Range.Text = r.ToString();
                    table.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 2).Range.Text = dr["MONTH_DATE"].ToString();
                    table.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 3).Range.Text = Convert.ToDecimal(dr["BEGINNING_DEBT"].ToString()).ToString("n2");
                    table.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 4).Range.Text = Convert.ToDecimal(dr["MONTHLY_PAYMENT"].ToString()).ToString("n2");
                    table.Cell(i, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 5).Range.Text = Convert.ToDecimal(dr["INTEREST_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 6).Range.Text = Convert.ToDecimal(dr["BASIC_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 7).Range.Text = Convert.ToDecimal(dr["DEBT"].ToString()).ToString("n2");
                    table.Cell(i, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    totalMonthlyAmount = totalMonthlyAmount + Convert.ToDecimal(dr["MONTHLY_PAYMENT"].ToString());
                    sumPercent = sumPercent + Convert.ToDecimal(dr["INTEREST_AMOUNT"].ToString());
                    sumBasic = sumBasic + Convert.ToDecimal(dr["BASIC_AMOUNT"].ToString());
                    i++;
                    r++;

                }

                table.Cell(i, 1).Merge(table.Cell(i, 3));
                table.Cell(i, 1).Range.Text = "CƏMİ";
                table.Cell(i, 2).Range.Text = totalMonthlyAmount.ToString("n2");
                table.Cell(i, 3).Range.Text = sumPercent.ToString("n2");
                table.Cell(i, 4).Range.Text = sumBasic.ToString("n2");

                decimal penaltyPercent = 0;
                List<Penalty> lstPenaly = PenaltyDAL.SelectContractPenaltyByContractID(int.Parse(ContractID), true).ToList<Penalty>();
                if (lstPenaly.Count > 0)
                    penaltyPercent = lstPenaly.LastOrDefault().INTEREST;

                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", CreditAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + " " + CreditCurrencyLookUp.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$period]", PeriodValue.Value + " ay");
                GlobalProcedures.FindAndReplace(wordApp, "[$grace]", GracePeriodValue.Value + " ay");
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyvoen]", GlobalVariables.V_CompanyVoen);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyphone]", GlobalVariables.V_CompanyPhone);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyaddress]", GlobalVariables.V_CompanyAddress);
                GlobalProcedures.FindAndReplace(wordApp, "[$companydirector]", GlobalVariables.V_CompanyDirector);
                GlobalProcedures.FindAndReplace(wordApp, "[$payday1]", GlobalFunctions.DayWithSuffix1(payDay));
                GlobalProcedures.FindAndReplace(wordApp, "[$payday2]", GlobalFunctions.DayWithSuffix2(payDay));
                GlobalProcedures.FindAndReplace(wordApp, "[$penaltypercent]", penaltyPercent.ToString("n2", GlobalVariables.V_CultureInfoEN));

                if (customer_type_id == 1)
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib)");
                else
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ")");

                GlobalProcedures.FindAndReplace(wordApp, "[$customeraddress]", RegistrationAddressText.Text.Trim());


                table.AutoFitBehavior(Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow);
                table.AllowAutoFit = false;
                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Replace("/", "") + "_OdenisQrafiki.docx faylı yaradılmadı.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void CalcMonthlyAmount()//ayliq odenisin hesablanmasi
        {
            if (PeriodValue.EditValue == null)
            {
                monthly_amount = 0;
                return;
            }

            int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
            if (!PeriodCheckEdit.Checked)
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)PeriodValue.Value - (int)GracePeriodValue.Value, (double)InterestValue.Value);
            else
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, (double)InterestValue.Value);

            MonthlyPaymentValue.Value = (decimal)monthly_amount;
        }

        private void CreditAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            if (CreditAmountValue.Value > 0)
                CalcMonthlyAmount();
            else
            {
                monthly_amount = 0;
                MonthlyPaymentValue.Value = 0;
                DeletePaymentSchedulesTemp();
            }
        }

        private void RefreshPaymentSchedulesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPaymentScheduleDataGridView();
        }

        private void PrintPaymentSchedulesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BPaymentSchedule_Click(sender, EventArgs.Empty);
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

        private void ContractStartDate_EditValueChanged(object sender, EventArgs e)
        {
            ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths((int)PeriodValue.Value);
            CreditTypeParametr();
            if ((!String.IsNullOrEmpty(ContractStartDate.Text)) && (!String.IsNullOrEmpty(ContractEndDate.Text)))
            {
                int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
                if ((FormStatus) && (CreditAmountValue.Value > 0))
                {
                    if (!PeriodCheckEdit.Checked)
                    {
                        monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)(PeriodValue.Value - GracePeriodValue.Value), (double)InterestValue.Value);
                        MonthlyPaymentValue.Value = (decimal)monthly_amount;
                    }
                    else
                    {
                        monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, (double)InterestValue.Value);
                        MonthlyPaymentValue.Value = (decimal)monthly_amount;
                    }
                }
                CreditAmountValue_EditValueChanged(sender, EventArgs.Empty);
            }
        }

        private string InsertContractCodeTemp()
        {
            if (RegistrationCodeSearch.Text.Length != 5 || !OtherInfoTabControl.Enabled)
                return null;

            int max_code_number = 0, a = 0;
            string code = "00000";

            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.CONTRACT_CODE_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID}", "Müqavilənin kodu temp cədvəldən silinmədi.");

            List<ContractCodeTemp> lstContractCode = ContractCodeTempDAL.SelectContractCode(null).ToList<ContractCodeTemp>();

            if (lstContractCode.Count == 0)
                max_code_number = GlobalFunctions.GetMax($@"SELECT NVL(MAX(TO_NUMBER(SUBSTR(C.CODE,1,5))),0) FROM CRS_USER.CONTRACTS C WHERE C.IS_USED_FOR_CODE = 1");
            else
                max_code_number = lstContractCode.Max(c => c.CODE_NUMBER);

            a = max_code_number + 1;
            code = a.ToString().PadLeft(5, '0') + "/" + DateTime.Today.ToString("yy");
            if (GlobalFunctions.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.CONTRACT_CODE_TEMP(USED_USER_ID,CONTRACT_ID,CODE_NUMBER,CODE)VALUES({GlobalVariables.V_UserID},{ContractID},{a},'{code}')",
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
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.PHONES WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.PHONES_TEMP WHERE OWNER_TYPE = '{seller_type}' AND IS_CHANGE <> 0 AND OWNER_ID = {GuaranteeID})",
                                                    $@"INSERT INTO CRS_USER.PHONES(ID,PHONE_DESCRIPTION_ID,COUNTRY_ID,PHONE_NUMBER,OWNER_ID,OWNER_TYPE,IS_SEND_SMS,ORDER_ID,KINDSHIP_RATE_ID,KINDSHIP_NAME)SELECT ID,PHONE_DESCRIPTION_ID,COUNTRY_ID,PHONE_NUMBER,OWNER_ID,OWNER_TYPE,IS_SEND_SMS,ORDER_ID,KINDSHIP_RATE_ID,KINDSHIP_NAME FROM CRS_USER_TEMP.PHONES_TEMP WHERE OWNER_ID = {GuaranteeID} AND IS_CHANGE = 1 AND OWNER_TYPE = '{seller_type}' AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Satıcının telefonları əsas cədvələ daxil edilmədi.");
        }

        private void InsertPhonesTemp()
        {
            if (TransactionName == "INSERT")
                return;

            ExecutePhonesTempProcedure(int.Parse(GuaranteeID));
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
            LoadPhoneDataGridView(GuaranteeID);
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
            LoadFPhoneAddEdit("INSERT", GuaranteeID, seller_type, null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", GuaranteeID, seller_type, PhoneID);
        }

        private void DeletePhone()
        {
            DialogResult dialogResult = XtraMessageBox.Show(PhoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE IN ('S','JP') AND OWNER_ID = {GuaranteeID} AND ID = {PhoneID}", "Telefon nömrəsi temp cədvəldən silinmədi.");
            }
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
            LoadPhoneDataGridView(GuaranteeID);
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
            LoadPhoneDataGridView(GuaranteeID);
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

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled)
                LoadFPhoneAddEdit("EDIT", GuaranteeID, seller_type, PhoneID);
        }

        private int InsertContract()
        {
            int max_code_number = 0,
                check_period = 0;
            decimal check_interest = -1;
            string code = null, account = credit_currency_id + RegistrationCodeSearch.Text + (customer_contract_number + 1).ToString().PadLeft(3, '0');

            if (TransactionName == "INSERT")
            {
                if (changecode)
                    code = ContractCodeText.Text.Trim();
                else
                {
                    max_code_number = (TransactionName == "INSERT") ? GlobalFunctions.GetMax($@"SELECT NVL(MAX(TO_NUMBER(SUBSTR (C.CODE, 1, 5))),0) FROM CRS_USER.CONTRACTS C WHERE C.IS_USED_FOR_CODE = 1") + 1 : 0;
                    code = max_code_number.ToString().PadLeft(5, '0').Trim() + "/" + DateTime.Today.ToString("yy");
                }
            }
            else
            {
                code = ContractCodeText.Text.Substring(1, ContractCodeText.Text.Length - 1);
                seller_type_id = (seller_type_id == 0) ? old_seller_type_id : seller_type_id;
            }

            if (PeriodCheckEdit.Checked)
                check_period = (int)PeriodValue.Value;

            if (InterestCheckEdit.Checked)
                check_interest = InterestValue.Value;

            return GlobalFunctions.ExecuteFourQuery($@"INSERT INTO CRS_USER.CONTRACTS(ID,
                                                                                  CODE,
                                                                                  CUSTOMER_ID,
                                                                                  CREDIT_TYPE_ID,
                                                                                  START_DATE,
                                                                                  END_DATE,
                                                                                  GRACE_PERIOD,
                                                                                  AMOUNT,
                                                                                  FIFD,
                                                                                  COMMISSION,
                                                                                  CURRENCY_ID,   
                                                                                  CUSTOMER_CARDS_ID,
                                                                                  MONTHLY_AMOUNT,
                                                                                  CHECK_PERIOD,
                                                                                  PAYMENT_TYPE,
                                                                                  CHECK_INTEREST,  
                                                                                  CURRENCY_RATE,
                                                                                  CUSTOMER_TYPE_ID,
                                                                                  PARENT_ID,
                                                                                  APPRAISER_ID,
                                                                                  LOAN_OFFICER_ID,
                                                                                  CONTRACT_ACCOUNT,
                                                                                  CUSTOMER_CONTRACT_NUMBER,
                                                                                  COMMISSION_ACCOUNT,
                                                                                  AMOUNT_ACCOUNT,
                                                                                  INSERT_USER)
                                                    VALUES({ContractID},
                                                            '{code.Trim()}',
                                                            {CustomerID},
                                                            {credit_type_id},
                                                            TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'),
                                                            TO_DATE('{ContractEndDate.Text}','DD/MM/YYYY'),
                                                            {GracePeriodValue.Value},
                                                            {Math.Round(CreditAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                  
                                                            {Math.Round(FifdValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {Math.Round(CommissionValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {credit_currency_id},                                                                                                                      
                                                            {customer_card_id},
                                                            {Math.Round(MonthlyPaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                            {check_period},
                                                            {PaymentTypeRadioGroup.SelectedIndex},
                                                            {check_interest},                                                         
                                                            {credit_currency_rate.ToString(GlobalVariables.V_CultureInfoEN)},                                                            
                                                            {customer_type_id},
                                                            {(!string.IsNullOrWhiteSpace(oldContractID) ? oldContractID : "NULL")},
                                                            {appraiser_id},
                                                            {loan_officer_id},
                                                            '{account}',
                                                            {customer_contract_number + 1},
                                                            '{commission_account}',
                                                            '{credit_amount_account}',
                                                            {GlobalVariables.V_UserID})",
                                                    $@"INSERT INTO CRS_USER.ACCOUNTING_PLAN (ID,
                                                                                             ACCOUNT_NUMBER,
                                                                                             ACCOUNT_NAME,
                                                                                             SUB_ACCOUNT,
                                                                                             SUB_ACCOUNT_NAME)
                                                               VALUES (CRS_USER.ACCOUNTING_PLAN_SEQUENCE.NEXTVAL,
                                                                         21110,
                                                                         'Ssuda hesabı',
                                                                         21110 || {account},
                                                                            'Ssuda hesabı - '
                                                                         || '{CustomerFullNameText.Text.Trim()}'
                                                                         || ' ('
                                                                         || '{ContractCodeText.Text.Trim()}'
                                                                         || ')')",
                                                     $@"INSERT INTO CRS_USER.ACCOUNTING_PLAN (ID,
                                                                                             ACCOUNT_NUMBER,
                                                                                             ACCOUNT_NAME,
                                                                                             SUB_ACCOUNT,
                                                                                             SUB_ACCOUNT_NAME)
                                                               VALUES (CRS_USER.ACCOUNTING_PLAN_SEQUENCE.NEXTVAL,
                                                                         21112,
                                                                         'Faiz hesabı',
                                                                         21112 || {account},
                                                                            'Faiz hesabı - '
                                                                         || '{CustomerFullNameText.Text.Trim()}'
                                                                         || ' ('
                                                                         || '{ContractCodeText.Text.Trim()}'
                                                                         || ')')",
                                                    $@"INSERT INTO CRS_USER.ACCOUNTING_PLAN (ID,
                                                                                             ACCOUNT_NUMBER,
                                                                                             ACCOUNT_NAME,
                                                                                             SUB_ACCOUNT,
                                                                                             SUB_ACCOUNT_NAME)
                                                               VALUES (CRS_USER.ACCOUNTING_PLAN_SEQUENCE.NEXTVAL,
                                                                         41010,
                                                                         'Cari hesab',
                                                                         41010 || {account},
                                                                            'Cari hesab - '
                                                                         || '{CustomerFullNameText.Text.Trim()}'
                                                                         || ' ('
                                                                         || '{ContractCodeText.Text.Trim()}'
                                                                         || ')')",
                                     "Müqavilə bazaya daxil edilmədi.",
                                  this.Name + "/InsertContract");
        }

        private void UpdateContract()
        {
            string account = credit_currency_id + RegistrationCodeSearch.Text + (customer_contract_number).ToString().PadLeft(3, '0');

            int check_period = 0;
            decimal check_interest = -1;
            if (PeriodCheckEdit.Checked)
                check_period = (int)PeriodValue.Value;
            if (InterestCheckEdit.Checked)
                check_interest = InterestValue.Value;
            if (seller_type_id == 0)
                seller_type_id = old_seller_type_id;

            string s = $@"UPDATE CRS_USER.CONTRACTS SET 
                                        CREDIT_TYPE_ID = {credit_type_id},
                                        START_DATE = TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'), 
                                        END_DATE = TO_DATE('{ContractEndDate.Text}','DD/MM/YYYY'),
                                        GRACE_PERIOD = {GracePeriodValue.Value},
                                        AMOUNT = {Math.Round(CreditAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                        COMMISSION = {Math.Round(CommissionValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                        CURRENCY_ID = {credit_currency_id},                                                                                
                                        CUSTOMER_CARDS_ID = {customer_card_id},
                                        MONTHLY_AMOUNT = {Math.Round(MonthlyPaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                        CHECK_PERIOD = {check_period},
                                        PAYMENT_TYPE = {PaymentTypeRadioGroup.SelectedIndex},
                                        CHECK_INTEREST = {check_interest},                                        
                                        CURRENCY_RATE = {credit_currency_rate.ToString(GlobalVariables.V_CultureInfoEN)},                                       
                                        CUSTOMER_TYPE_ID = {customer_type_id},
                                        APPRAISER_ID = {appraiser_id},
                                        LOAN_OFFICER_ID = {loan_officer_id},
                                        CONTRACT_ACCOUNT = '{account}',
                                        CUSTOMER_CONTRACT_NUMBER = {customer_contract_number},
                                        COMMISSION_ACCOUNT = '{commission_account}',
                                        AMOUNT_ACCOUNT = '{credit_amount_account}',
                                        UPDATE_USER = {GlobalVariables.V_UserID},
                                        UPDATE_DATE = SYSDATE
                                   WHERE ID = {ContractID}";

            GlobalProcedures.ExecuteFourQuery(s,
                                              $@"UPDATE CRS_USER.ACCOUNTING_PLAN SET SUB_ACCOUNT = 21110 || {account},SUB_ACCOUNT_NAME = 'Ssuda hesabı - '|| '{CustomerFullNameText.Text.Trim()} ('||'{ContractCodeText.Text.Trim()})'  WHERE ACCOUNT_NUMBER = 21110 AND SUB_ACCOUNT = 21110 || {old_account}",
                                              $@"UPDATE CRS_USER.ACCOUNTING_PLAN SET SUB_ACCOUNT = 21112 || {account},SUB_ACCOUNT_NAME = 'Faiz hesabı - '|| '{CustomerFullNameText.Text.Trim()} ('||'{ContractCodeText.Text.Trim()})'  WHERE ACCOUNT_NUMBER = 21112 AND SUB_ACCOUNT = 21112 || {old_account}",
                                              $@"UPDATE CRS_USER.ACCOUNTING_PLAN SET SUB_ACCOUNT = 41010 || {account},SUB_ACCOUNT_NAME = 'Cari hesab - '|| '{CustomerFullNameText.Text.Trim()} ('||'{ContractCodeText.Text.Trim()})'  WHERE ACCOUNT_NUMBER = 41010 AND SUB_ACCOUNT = 41010 || {old_account}",
                                                 "Müqavilə bazada dəyişdirilmədi.", this.Name + "/UpdateContract");
        }

        private void InsertContractSubData()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_CONTRACT_SUB_DATA", "P_CONTRACT_ID", ContractID, "Müqavilənin alt məlumatları əsas cədvəllərə daxil olmadı.");
        }

        private bool ControlContractDetails()
        {
            bool b = false;

            if (CommissionValue.Value > 0 && commission_account == null)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CommissionAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Komissiyanın ödəniləcək hesabı seçilməyib.");
                CommissionAccountLookUp.Focus();
                CommissionAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CreditAmountValue.Value <= 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CreditAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kreditin məbləği sıfırdan böyük olmalıdır.");
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

            if (CreditAmountValue.Value > 0 && credit_amount_account == null)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CreditAmountAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kredit məbləğinin ödəniləcək hesabı seçilməyib.");
                CreditAmountAccountLookUp.Focus();
                CreditAmountAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (FifdValue.Value <= 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                FifdValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("FİFD sıfırdan böyük olmalıdır.");
                FifdValue.Focus();
                FifdValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (appraiser_id == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                AppraiserLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qiymətləndirici seçilməyib.");
                AppraiserLookUp.Focus();
                AppraiserLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (loan_officer_id == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                LoanOfficerLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kredit mütəxəssisi seçilməyib.");
                LoanOfficerLookUp.Focus();
                LoanOfficerLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS C WHERE C.CODE = '{ContractCodeText.Text.Trim()}'") > 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text + " saylı kredit müqaviləsi artıq bazaya daxil edilib.");
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PAWN_TEMP WHERE CONTRACT_ID = {ContractID}") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı kredit müqaviləsi üçün girovlar daxil edilməyib.");
                return false;
            }
            else
                b = true;

            if (CreditAmountValue.Value > sumTotalAmount)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                DialogResult dialogResult = XtraMessageBox.Show("Girovların dəyəri kreditin məbləğindən (" + CreditAmountValue.Value + " AZN) azdır. Bu halda kredit verməyinizə əminsiniz?", "Xəbərdarlıq", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    b = true;
                else
                    return false;
            }
            else
                b = true;

            if ((TransactionName == "INSERT" || TransactionName == "AGREEMENT") && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.INTEREST_PENALTIES_TEMP WHERE CONTRACT_ID = {ContractID}") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 8;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı kredit müqaviləsi üçün cərimə faizi təyin edilməyib.");
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ControlContractDetails())
                {
                    GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FContractSaveWait));
                    if (TransactionName == "INSERT")
                    {
                        //InsertSeller();
                        if (InsertContract() < 1)
                            return;

                        if (GlobalVariables.CommitContract)
                            GlobalProcedures.InsertContractOperationJournal(ContractStartDate.Text, (double)CreditAmountValue.Value, (double)CommissionValue.Value, ContractID, credit_amount_account, commission_account);
                    }
                    else if (TransactionName == "EDIT")
                    {
                        if (((Commit == 1 || Commit == 0) && GlobalVariables.CommitContract) || (Commit == 0 && !GlobalVariables.CommitContract && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.AGAIN_COMMITMENT WHERE CONTRACT_ID = {ContractID}") == 0))
                        {
                            UpdateContract();
                        }
                        else
                            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_COMMITMENT_CHANGE", "P_CONTRACT_ID", ContractID, "Müqavilə təkrar təsdiq cədvəlinə daxil olmadı.");


                        if (GlobalVariables.CommitContract)
                        {
                            if (Commit == 0)
                            {
                                GlobalProcedures.ExecuteTwoQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_COMMIT = 1 WHERE ID = {ContractID}",
                                                                   $@"INSERT INTO CRS_USER.CONTRACT_INTEREST_PENALTIES(ID,CONTRACT_ID,CALC_DATE)VALUES(INTEREST_PENALTIES_SEQUENCE.NEXTVAL,{ContractID},TO_DATE('{ContractStartDate.Text}','DD/MM/YYYY'))",
                                                                "Müqavilə təsdiqlənmədi.");
                            }

                            GlobalProcedures.InsertContractOperationJournal(ContractStartDate.Text, (double)CreditAmountValue.Value, (double)CommissionValue.Value, ContractID, credit_amount_account, commission_account);
                            GlobalProcedures.ContractClosedDay(ContractStartDate.DateTime, int.Parse(ContractID));
                        }
                    }

                    if (PaymentSchedulesGridView.RowCount == 0)
                        CalculatedPaymentScheduleTemp(false);
                    InsertContractImages();
                    InsertContractDescriptions();
                    InsertContractSubData();
                    //UpdatePhoneSendSms();
                    //InsertPhones();
                    //UpdateCardFrontFace();
                    //UpdateCardRearFace();
                    InsertContractDocument();
                    GlobalProcedures.CalculatedLeasingTotal(ContractID);
                    this.Close();
                }

            }
            catch (Exception exx)
            {

            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

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
                                                                                                            REGISTRATION_ADDRESS) 
                                                                            VALUES({GuaranteeID},                                                                                  D
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
                                                                                        '{SellerRegistrationAddressText.Text}')";
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
                                                                                                            CARD_SERIES_ID,
                                                                                                            CARD_NUMBER,
                                                                                                            CARD_PINCODE,
                                                                                                            CARD_ISSUE_DATE,
                                                                                                            CARD_ISSUING_ID,
                                                                                                            ADDRESS,
                                                                                                            REGISTRATION_ADDRESS) 
                                                                            VALUES({GuaranteeID}, 
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                        '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',                                                                                        
                                                                                        {card_series_id},
                                                                                        '{SellerNumberText.Text}',
                                                                                        '{SellerPinCodeText.Text.Trim()}',
                                                                                        TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                        {card_issuing_id},
                                                                                        '{SellerAddressText.Text.Trim()}',
                                                                                        '{SellerRegistrationAddressText.Text}')";
                        }
                    }
                    else
                    {
                        command.CommandText = $@"INSERT INTO CRS_USER.JURIDICAL_PERSONS(ID,
                                                                                                                NAME,
                                                                                                                VOEN,
                                                                                                                LEADING_NAME,
                                                                                                                ADDRESS,
                                                                                                                IS_BUYER) 
                                                               VALUES(    
                                                                        {GuaranteeID},                                                                  
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
                res = GlobalFunctions.ExecuteProcedureWithInParametrAndOutParametr("CRS_USER.PROC_DUPLICATE_SELLER", "P_SELLER_ID", int.Parse(GuaranteeID), "SELLER_ID", OracleDbType.Int32, "Satıcının məlumatları bazaya daxil edilmədi.").ToString();
            else
                res = GlobalFunctions.ExecuteProcedureWithInParametrAndOutParametr("CRS_USER.PROC_DUPLICATE_JURIDICAL", "P_SELLER_ID", int.Parse(GuaranteeID), "SELLER_ID", OracleDbType.Int32, "Satıcının məlumatları bazaya daxil edilmədi.").ToString();

            return res;
        }

        //private void ControlSeller()
        //{
        //    if (SellerTypeRadioGroup.SelectedIndex == 0 && old_seller_index == 1)
        //        GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.PHONES WHERE (OWNER_ID,OWNER_TYPE) IN (SELECT OWNER_ID,OWNER_TYPE FROM CRS_USER_TEMP.PHONES_TEMP
        //                                                                                                    WHERE OWNER_TYPE = 'JP' 
        //                                                                                                        AND USED_USER_ID = {GlobalVariables.V_UserID})",
        //                                                         $@"DELETE FROM CRS_USER.JURIDICAL_PERSONS 
        //                                                                                WHERE ID = {old_seller_id}",
        //                                                                        "Hüquqi şəxs silinmədi.",
        //                                             this.Name + "/ControlSeller");
        //    else if (SellerTypeRadioGroup.SelectedIndex == 1 && old_seller_index == 0)
        //        GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.PHONES WHERE (OWNER_ID,OWNER_TYPE) IN (SELECT OWNER_ID,OWNER_TYPE FROM CRS_USER_TEMP.PHONES_TEMP
        //                                                                                            WHERE OWNER_TYPE = 'S' 
        //                                                                                                AND USED_USER_ID = {GlobalVariables.V_UserID})",
        //                                                         $@"DELETE FROM CRS_USER.SELLERS 
        //                                                                                WHERE ID = {old_seller_id}",
        //                                                                        "Fiziki şəxs silinmədi.",
        //                                           this.Name + "/ControlSeller");
        //}

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
                                command.CommandText = $@"UPDATE CRS_USER.SELLERS SET 
                                                                                                SURNAME = '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                                NAME = '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                                PATRONYMIC = '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',
                                                                                                IMAGE = :BlobImage,
                                                                                                CARD_SERIES_ID = {card_series_id},
                                                                                                CARD_NUMBER = '{SellerNumberText.Text}',
                                                                                                CARD_PINCODE = '{SellerPinCodeText.Text.Trim()}',
                                                                                                CARD_ISSUE_DATE = TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                                CARD_ISSUING_ID = {card_issuing_id},
                                                                                                ADDRESS = '{SellerAddressText.Text.Trim()}',
                                                                                                REGISTRATION_ADDRESS = '{SellerRegistrationAddressText.Text}'
                                                                                                WHERE ID = {GuaranteeID}";
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
                                                                                                            IMAGE,
                                                                                                            CARD_SERIES_ID,
                                                                                                            CARD_NUMBER,
                                                                                                            CARD_PINCODE,
                                                                                                            CARD_ISSUE_DATE,
                                                                                                            CARD_ISSUING_ID,
                                                                                                            ADDRESS,
                                                                                                            REGISTRATION_ADDRESS
                                                                                                      ) 
                                                                            VALUES(
                                                                                    {GuaranteeID},
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
                                                                                    '{SellerRegistrationAddressText.Text}'
                                                                                )";
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
                                command.CommandText = $@"UPDATE CRS_USER.SELLERS SET 
                                                                                                SURNAME = '{GlobalFunctions.FirstCharToUpper(SellerSurnameText.Text.Trim())}',
                                                                                                NAME = '{GlobalFunctions.FirstCharToUpper(SellerNameText.Text.Trim())}',
                                                                                                PATRONYMIC = '{GlobalFunctions.FirstCharToUpper(SellerPatronymicText.Text.Trim())}',                                                                                                
                                                                                                CARD_SERIES_ID = {card_series_id},
                                                                                                CARD_NUMBER = '{SellerNumberText.Text}',
                                                                                                CARD_PINCODE = '{SellerPinCodeText.Text.Trim()}',
                                                                                                CARD_ISSUE_DATE = TO_DATE('{SellerIssueDate.Text}','DD/MM/YYYY'),
                                                                                                CARD_ISSUING_ID = {card_issuing_id},
                                                                                                ADDRESS = '{SellerAddressText.Text.Trim()}',
                                                                                                REGISTRATION_ADDRESS = '{SellerRegistrationAddressText.Text}'
                                                                                                WHERE ID = {GuaranteeID}";
                            else
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
                                                                                    {GuaranteeID},
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
                        if (old_seller_index == SellerTypeRadioGroup.SelectedIndex)
                            command.CommandText = $@"UPDATE CRS_USER.JURIDICAL_PERSONS SET 
                                                                                            NAME = '{JuridicalPersonNameText.Text.Trim()}',
                                                                                            VOEN = '{JuridicalPersonVoenText.Text.Trim()}',
                                                                                            LEADING_NAME = '{LeadingPersonNameText.Text.Trim()}',
                                                                                            ADDRESS = '{JuridicalPersonAddressText.Text.Trim()}' 
                                                                            WHERE ID = {GuaranteeID}";
                        else
                            command.CommandText = $@"INSERT INTO CRS_USER.JURIDICAL_PERSONS(
                                                                                                                ID,
                                                                                                                NAME,
                                                                                                                VOEN,
                                                                                                                LEADING_NAME,
                                                                                                                ADDRESS
                                                                                                                ) 
                                                                                                     VALUES(
                                                                                                                {GuaranteeID},
                                                                                                                '{JuridicalPersonNameText.Text.Trim()}',
                                                                                                                '{JuridicalPersonVoenText.Text.Trim()}',
                                                                                                                '{LeadingPersonNameText.Text.Trim()}',
                                                                                                                '{JuridicalPersonAddressText.Text.Trim()}'
                                                                                                           )";
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
            RegistrationCodeSearch.Text = code;
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
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_FRONT_FACE_IMAGE = :BlobFrontImage, CARD_FRONT_FACE_IMAGE_FORMAT = '{front_format}' WHERE ID = {GuaranteeID}";
                        OracleParameter front_blobParameter = new OracleParameter();
                        front_blobParameter.OracleDbType = OracleDbType.Blob;
                        front_blobParameter.ParameterName = "BlobFrontImage";
                        front_blobParameter.Value = front_blob;
                        command.Parameters.Add(front_blobParameter);
                    }
                    else
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_FRONT_FACE_IMAGE = null WHERE ID = {GuaranteeID}";
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
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_REAR_FACE_IMAGE = :BlobRearImage, CARD_REAR_FACE_IMAGE_FORMAT = '{rear_format}' WHERE ID = {GuaranteeID}";
                        OracleParameter rear_blobParameter = new OracleParameter();
                        rear_blobParameter.OracleDbType = OracleDbType.Blob;
                        rear_blobParameter.ParameterName = "BlobRearImage";
                        rear_blobParameter.Value = rear_blob;
                        command.Parameters.Add(rear_blobParameter);
                    }
                    else
                        command.CommandText = $@"UPDATE CRS_USER.SELLERS SET CARD_REAR_FACE_IMAGE = null WHERE ID = {GuaranteeID}";
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

        private void BChangeCreditPercent_Click(object sender, EventArgs e)
        {
            LoadDictionaries("E", 5);
            CreditTypeParametr();
        }

        private bool ControlContractFileDetails()
        {
            bool b = false;

            OtherInfoTabControl.SelectedTabPageIndex = 0;

            if (PeriodValue.EditValue == null)
            {
                PeriodValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müqavilənin müddəti təyin edilməyib.");
                PeriodValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;
            return b;
        }

        private void BContract_Click(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Müqavilə.docx");
            if (!File.Exists(fileName.ToString()))
            {
                contract_click = false;
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Müqavilənin faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string qep = null,
                amount_with_word,
                com_with_word,
                fifd_with_word,
                phone = null,
                period = null,
                date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_Müqavilə.docx";

            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            object saveAs = Path.Combine(filePath);
            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;

            aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            aDoc.Activate();
            double d = 0, div = 0;
            int mod = 0;
            d = (double)CreditAmountValue.Value * 100;

            div = (int)(d / 100);
            mod = (int)(d % 100);
            if (mod > 0)
            {
                if (credit_currency_id == 1)
                    qep = " " + mod.ToString() + " qəpik";
                else
                    qep = " " + mod.ToString();
            }

            amount_with_word = "(" + GlobalFunctions.IntegerToWritten(div) + ") " + credit_currency_name + qep;

            //Komissiya
            d = (double)CommissionValue.Value * 100;

            div = (int)(d / 100);
            mod = (int)(d % 100);
            if (mod > 0)
            {
                if (credit_currency_id == 1)
                    qep = " " + mod.ToString() + " qəpik";
                else
                    qep = " " + mod.ToString();
            }

            com_with_word = "(" + GlobalFunctions.IntegerToWritten(div) + ") " + credit_currency_name + qep;

            //FIFD
            decimal fifd = Math.Round(FifdValue.Value, 2);
            d = (double)fifd * 100;

            div = (int)(d / 100);
            mod = (int)(d % 100);
            if (mod > 0)
                qep = " tam yüzdə " + GlobalFunctions.IntegerToWritten(mod);

            fifd_with_word = "(" + GlobalFunctions.IntegerToWritten(div) + qep + ")";

            if (PeriodCheckEdit.Checked)
                period = ContractEndDate.Text + " tarixinə qədər";
            else
                period = PeriodValue.Value + " (" + GlobalFunctions.IntegerToWritten((int)PeriodValue.Value) + ") ay";

            phone = GlobalFunctions.GetName($@"SELECT PHONE FROM CRS_USER.V_PHONE WHERE OWNER_TYPE = '{person_description}' AND OWNER_ID = {CustomerID}");

            try
            {
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", date);
                if (customer_type_id == 1)
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib)");
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib");
                }
                else
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ")");
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", null);
                }
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyvoen]", GlobalVariables.V_CompanyVoen);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyphone]", GlobalVariables.V_CompanyPhone);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyaddress]", GlobalVariables.V_CompanyAddress);
                GlobalProcedures.FindAndReplace(wordApp, "[$companydirector]", GlobalVariables.V_CompanyDirector);
                GlobalProcedures.FindAndReplace(wordApp, "[$card]", CardDescriptionText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$cur]", credit_currency_name);
                GlobalProcedures.FindAndReplace(wordApp, "[$creditpurpose]", "biznes tələblərinin ödənilməsi");
                GlobalProcedures.FindAndReplace(wordApp, "[$percent]", InterestValue.Value);
                GlobalProcedures.FindAndReplace(wordApp, "[$percentwrite]", "(" + GlobalFunctions.IntegerToWritten((double)InterestValue.Value) + ")");
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", CreditAmountValue.Value.ToString("N2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$amountwrite]", amount_with_word);
                GlobalProcedures.FindAndReplace(wordApp, "[$grace]", GracePeriodValue.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$gracewrite]", "(" + GlobalFunctions.IntegerToWritten((int)GracePeriodValue.Value) + ")");
                GlobalProcedures.FindAndReplace(wordApp, "[$period]", PeriodValue.Value);
                GlobalProcedures.FindAndReplace(wordApp, "[$periodwrite]", "(" + GlobalFunctions.IntegerToWritten((double)PeriodValue.Value) + ")");
                GlobalProcedures.FindAndReplace(wordApp, "[$com]", CommissionValue.Value);
                GlobalProcedures.FindAndReplace(wordApp, "[$comwrite]", com_with_word);
                GlobalProcedures.FindAndReplace(wordApp, "[$fifd]", fifd);
                GlobalProcedures.FindAndReplace(wordApp, "[$fifdwrite]", fifd_with_word);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractenddate]", GlobalFunctions.DateWithDayMonthYear(ContractEndDate.DateTime));
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$address]", RegistrationAddressText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$phones]", phone);

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
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text + "_Müqavilə.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(ContractStartDate.Text);
            CreditAmountValue_EditValueChanged(sender, EventArgs.Empty);
        }


        private void ContractEndDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ContractStartDate.Text) && !String.IsNullOrEmpty(ContractEndDate.Text) && InterestValue.EditValue != null && OtherInfoTabControl.Enabled && (double)InterestValue.Value > 0)
            {
                int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
                //PeriodText.Text = diff_month.ToString();
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, (double)InterestValue.Value);
                MonthlyPaymentValue.Value = (decimal)monthly_amount;
            }
        }

        private void GracePeriodValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(ContractStartDate.DateTime, ContractEndDate.DateTime);
            if (!PeriodCheckEdit.Checked)
            {
                if (PaymentTypeRadioGroup.SelectedIndex == 0)
                {
                    monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)(PeriodValue.Value - GracePeriodValue.Value), (double)InterestValue.Value);
                    MonthlyPaymentValue.Value = (decimal)monthly_amount;
                }
                else
                    monthly_amount = (double)MonthlyPaymentValue.Value;
            }
            else
            {
                if (PaymentTypeRadioGroup.SelectedIndex == 0)
                {
                    monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, (double)InterestValue.Value);
                    MonthlyPaymentValue.Value = (decimal)monthly_amount;
                }
                else
                    monthly_amount = (double)MonthlyPaymentValue.Value;
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        static int file_count = 0;

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
                    ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkil";
                }
                dlg.Dispose();
            }
        }

        private void CreditCurrencyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CreditCurrencyLookUp.EditValue == null)
            {
                MonthlyPaymentCurrencyLabel.Text = null;
                return;
            }

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
            CreditAmountValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void CreditNameLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                LoadDictionaries("E", 5);
                if (PeriodValue.EditValue != null && InterestValue.EditValue != null)
                {
                    monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)PeriodValue.Value, (double)InterestValue.Value);
                    MonthlyPaymentValue.Value = (decimal)monthly_amount;
                }
            }
        }

        private void CreditCurrencyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 7);
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

        private void AppraiserLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            appraiser_id = Convert.ToInt32((sender as LookUpEdit).EditValue);
        }

        private void AppraiserLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 12);
        }

        private void PaymentSchedulesGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Shedule_SS, e);
        }

        private void CommissionAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            commission_account = (sender as LookUpEdit).EditValue.ToString();
        }

        private void BAct_Click(object sender, EventArgs e)
        {
            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PAWN_TEMP WHERE CONTRACT_ID = {ContractID}") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı kredit müqaviləsi üçün girovlar daxil edilməyib.");
                return;
            }

            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Qiymətləndirmə aktı.docx");
            if (!File.Exists(fileName.ToString()))
            {
                GlobalVariables.WordDocumentUsed = akt_click = false;
                XtraMessageBox.Show("Qiymətləndirmə aktının şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_QiymetlendirmeAkti.docx",
                   date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                   eyebrowsType = "Brilyant";

            try
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
                string phone = GlobalFunctions.GetName($@"SELECT PHONE FROM CRS_USER.V_PHONE WHERE OWNER_TYPE = '{person_description}' AND OWNER_ID = {CustomerID}");
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();

                Microsoft.Office.Interop.Word.Table table = aDoc.Tables[1];

                string sql = $@"SELECT PT.NAME PAWN_NAME,
                                       P.COUNT,
                                       GT.NAME GOLD_NAME,
                                       P.WEIGHT,
                                       P.CWEIGHT,
                                       P.GOLD_AMOUNT,
                                       P.EYEBROWS_WEIGHT,
                                       P.EYEBROWS_AMOUNT,
                                       P.TOTAL_AMOUNT,
                                       ET.NAME EYEBROWS_TYPE_NAME
                                  FROM CRS_USER_TEMP.PAWN_TEMP P, CRS_USER.PAWN_TYPE PT, CRS_USER.GOLD_TYPE GT, CRS_USER.EYEBROWS_TYPE ET
                                 WHERE P.IS_CHANGE != 2 AND P.PAWN_TYPE_ID = PT.ID AND P.GOLD_TYPE_ID = GT.ID AND P.EYEBROWS_TYPE_ID = ET.ID AND P.CONTRACT_ID = {ContractID}
                                 ORDER BY P.ID";
                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/BPawnList_Click", "Girovlar açılmadı.");
                int i = 4, r = 1;
                decimal totalAmount = 0, sumWeightt = 0, sumCweightt = 0, sumEyebrowsAmount = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    eyebrowsType = dr["EYEBROWS_TYPE_NAME"].ToString();
                    table.Rows.Add(ref missing);
                    table.Cell(i, 1).Range.Text = r.ToString();
                    table.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 2).Range.Text = dr["PAWN_NAME"].ToString();
                    table.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    table.Cell(i, 3).Range.Text = dr["COUNT"].ToString();
                    table.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 4).Range.Text = dr["GOLD_NAME"].ToString();
                    table.Cell(i, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 5).Range.Text = Convert.ToDecimal(dr["WEIGHT"].ToString()).ToString("n2");
                    table.Cell(i, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 6).Range.Text = Convert.ToDecimal(dr["CWEIGHT"].ToString()).ToString("n2");
                    table.Cell(i, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 7).Range.Text = Convert.ToDecimal(dr["GOLD_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 8).Range.Text = Convert.ToDecimal(dr["EYEBROWS_WEIGHT"].ToString()).ToString("n2");
                    table.Cell(i, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 9).Range.Text = Convert.ToDecimal(dr["EYEBROWS_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 10).Range.Text = Convert.ToDecimal(dr["TOTAL_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 10).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    totalAmount = totalAmount + Convert.ToDecimal(dr["TOTAL_AMOUNT"].ToString());
                    sumWeightt = sumWeightt + Convert.ToDecimal(dr["WEIGHT"].ToString());
                    sumCweightt = sumCweightt + Convert.ToDecimal(dr["CWEIGHT"].ToString());
                    sumEyebrowsAmount = sumEyebrowsAmount + Convert.ToDecimal(dr["EYEBROWS_AMOUNT"].ToString());
                    i++;
                    r++;

                }

                table.Cell(i, 1).Merge(table.Cell(i, 4));
                table.Cell(i, 1).Range.Text = "CƏMİ";
                table.Cell(i, 2).Range.Text = sumWeightt.ToString("n2");
                table.Cell(i, 3).Range.Text = sumCweightt.ToString("n2");
                table.Cell(i, 6).Range.Text = sumEyebrowsAmount.ToString("n2");
                table.Cell(i, 7).Range.Text = totalAmount.ToString("n2");

                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", date);
                if (customer_type_id == 1)
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib)");
                else
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ")");

                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$phones]", phone);
                GlobalProcedures.FindAndReplace(wordApp, "[$username]", LoanOfficerLookUp.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$appraiser]", AppraiserLookUp.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$eyebrows]", eyebrowsType);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyvoen]", GlobalVariables.V_CompanyVoen);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyphone]", GlobalVariables.V_CompanyPhone);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyaddress]", GlobalVariables.V_CompanyAddress);
                GlobalProcedures.FindAndReplace(wordApp, "[$companydirector]", GlobalVariables.V_CompanyDirector);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);
                GlobalVariables.WordDocumentUsed = akt_click = true;
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Replace("/", "") + "_GirovMuqavilesineElave.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
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

        private void BPawnList_Click(object sender, EventArgs e)
        {
            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PAWN_TEMP WHERE CONTRACT_ID = {ContractID}") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı kredit müqaviləsi üçün girovlar daxil edilməyib.");
                return;
            }

            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Girov müqaviləsinə əlavə.docx");
            if (!File.Exists(fileName.ToString()))
            {
                GlobalVariables.WordDocumentUsed = pawn_list_click = false;
                XtraMessageBox.Show("Girov müqaviləsinə əlavə şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_GirovMuqavilesineElave.docx",
                   date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                   eyebrowType = "Brilyant";

            try
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
                string phone = GlobalFunctions.GetName($@"SELECT PHONE FROM CRS_USER.V_PHONE WHERE OWNER_TYPE = '{person_description}' AND OWNER_ID = {CustomerID}");
                object missing = Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();

                Microsoft.Office.Interop.Word.Table table = aDoc.Tables[1];

                string sql = $@"SELECT PT.NAME PAWN_NAME,
                                       P.COUNT,
                                       GT.NAME GOLD_NAME,
                                       P.WEIGHT,
                                       P.CWEIGHT,
                                       P.GOLD_AMOUNT,
                                       P.EYEBROWS_WEIGHT,
                                       P.EYEBROWS_AMOUNT,
                                       P.TOTAL_AMOUNT,
                                       ET.NAME EYEBROWS_TYPE_NAME
                                  FROM CRS_USER_TEMP.PAWN_TEMP P, CRS_USER.PAWN_TYPE PT, CRS_USER.GOLD_TYPE GT, CRS_USER.EYEBROWS_TYPE ET
                                 WHERE P.IS_CHANGE != 2 AND P.PAWN_TYPE_ID = PT.ID AND P.GOLD_TYPE_ID = GT.ID AND P.EYEBROWS_TYPE_ID = ET.ID AND P.CONTRACT_ID = {ContractID}
                                 ORDER BY P.ID";
                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/BPawnList_Click", "Girovlar açılmadı.");
                int i = 4, r = 1;
                decimal totalAmount = 0, sumWeightt = 0, sumCweightt = 0, sumEyebrowsAmount = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    eyebrowType = dr["EYEBROWS_TYPE_NAME"].ToString();
                    table.Rows.Add(ref missing);
                    table.Cell(i, 1).Range.Text = r.ToString();
                    table.Cell(i, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 2).Range.Text = dr["PAWN_NAME"].ToString();
                    table.Cell(i, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    table.Cell(i, 3).Range.Text = dr["COUNT"].ToString();
                    table.Cell(i, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 4).Range.Text = dr["GOLD_NAME"].ToString();
                    table.Cell(i, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 5).Range.Text = Convert.ToDecimal(dr["WEIGHT"].ToString()).ToString("n2");
                    table.Cell(i, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 6).Range.Text = Convert.ToDecimal(dr["CWEIGHT"].ToString()).ToString("n2");
                    table.Cell(i, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 7).Range.Text = Convert.ToDecimal(dr["GOLD_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 8).Range.Text = Convert.ToDecimal(dr["EYEBROWS_WEIGHT"].ToString()).ToString("n2");
                    table.Cell(i, 8).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 9).Range.Text = Convert.ToDecimal(dr["EYEBROWS_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 9).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    table.Cell(i, 10).Range.Text = Convert.ToDecimal(dr["TOTAL_AMOUNT"].ToString()).ToString("n2");
                    table.Cell(i, 10).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    totalAmount = totalAmount + Convert.ToDecimal(dr["TOTAL_AMOUNT"].ToString());
                    sumWeightt = sumWeightt + Convert.ToDecimal(dr["WEIGHT"].ToString());
                    sumCweightt = sumCweightt + Convert.ToDecimal(dr["CWEIGHT"].ToString());
                    sumEyebrowsAmount = sumEyebrowsAmount + Convert.ToDecimal(dr["EYEBROWS_AMOUNT"].ToString());
                    i++;
                    r++;

                }

                table.Cell(i, 1).Merge(table.Cell(i, 4));
                table.Cell(i, 1).Range.Text = "CƏMİ";
                table.Cell(i, 2).Range.Text = sumWeightt.ToString("n2");
                table.Cell(i, 3).Range.Text = sumCweightt.ToString("n2");
                table.Cell(i, 6).Range.Text = sumEyebrowsAmount.ToString("n2");
                table.Cell(i, 7).Range.Text = totalAmount.ToString("n2");

                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", date);
                if (customer_type_id == 1)
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib");
                else
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", null);

                GlobalProcedures.FindAndReplace(wordApp, "[$card]", CardDescriptionText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$address]", RegistrationAddressText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$phones]", phone);
                GlobalProcedures.FindAndReplace(wordApp, "[$eyebrows]", eyebrowType);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyvoen]", GlobalVariables.V_CompanyVoen);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyphone]", GlobalVariables.V_CompanyPhone);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyaddress]", GlobalVariables.V_CompanyAddress);
                GlobalProcedures.FindAndReplace(wordApp, "[$companydirector]", GlobalVariables.V_CompanyDirector);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);
                GlobalVariables.WordDocumentUsed = pawn_list_click = true;
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Replace("/", "") + "_GirovMuqavilesineElave.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void CommissionValue_EditValueChanged(object sender, EventArgs e)
        {
            CommissionAccountStarLabel.Visible = CommissionValue.Value > 0;
            CalcFIFD();
        }

        private void MonthlyPaymentValue_EditValueChanged(object sender, EventArgs e)
        {
            CalcFIFD();
        }

        private void PeriodCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (PeriodCheckEdit.Checked)
            {
                if (pay_count == 0)
                    PeriodValue.Properties.ReadOnly = CurrentStatus;
                else
                    PeriodValue.Properties.ReadOnly = true;
                if (!CurrentStatus)
                    PeriodValue.Focus();
            }
            else
            {
                PeriodValue.EditValue = period;
                PeriodValue.ReadOnly = true;
            }
            PeriodValue.TabStop = !PeriodValue.Properties.ReadOnly;
            ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths((int)PeriodValue.Value);
        }

        private void PeriodValue_EditValueChanged(object sender, EventArgs e)
        {            
            ContractEndDate.EditValue = ContractStartDate.DateTime.AddMonths((int)PeriodValue.Value);
        }

        private void LoanOfficerLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 14);
        }

        private void LoanOfficerLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            loan_officer_id = Convert.ToInt32((sender as LookUpEdit).EditValue);
        }

        private void CalcFIFD()
        {
            if (MonthlyPaymentValue.Value < 1 || (CreditAmountValue.Value - CommissionValue.Value) < 0)
                return;

            try
            {
                int count = (int)PeriodValue.Value + 1;
                double[] tmpCashflows = new double[count];

                for (int i = 0; i < count; i++)
                {
                    if (i == 0)
                        tmpCashflows[i] = (double)(CreditAmountValue.Value - CommissionValue.Value);
                    else
                        tmpCashflows[i] = (double)MonthlyPaymentValue.Value * -1;
                }

                double tmpIrr = Microsoft.VisualBasic.Financial.IRR(ref tmpCashflows) * 12 * 100;

                FifdValue.EditValue = tmpIrr;
            }
            catch { }
        }

        //private void CalcGoldAmount()
        //{
        //    if (sumWeight == 0)
        //    {
        //        LoadPawnDataGridView();
        //        return;
        //    }

        //    decimal amount = Math.Round((CreditAmountValue.Value / (decimal)0.70) / sumWeight, 2);

        //    //string sql = $@"UPDATE CRS_USER_TEMP.PAWN_TEMP SET AMOUNT = {amount.ToString(GlobalVariables.V_CultureInfoEN)}, GOLD_AMOUNT = CWEIGHT * {amount.ToString(GlobalVariables.V_CultureInfoEN)}, TOTAL_AMOUNT = CWEIGHT * {amount.ToString(GlobalVariables.V_CultureInfoEN)} + EYEBROWS_AMOUNT";
        //    //GlobalProcedures.ExecuteQuery(sql, "Qızılın qiyməti hesablanmadı.", this.Name + "/CalcBarButton_ItemClick");
        //    //LoadPawnDataGridView();
        //}

        private void BCashOrder_Click(object sender, EventArgs e)
        {
            string qep = null,
                amount_with_word,
                amount_with_word2,
                number;

            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Kassa məxaric orderi.docx");
            if (!File.Exists(fileName.ToString()))
            {
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Kassa məxaric orderinin şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string sql = $@"SELECT CODE FROM CRS_USER.CASH_ORDER_NUMBER";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/BCashOrder_Click", "Sonuncu kassa məxaric orderin nömrəsi açılmadı.");
            if (dt.Rows.Count == 0)
            {
                GlobalProcedures.ShowErrorMessage("Kassa məxaric orderinin başlanğıc nömrəsi daxil edilməyib. Zəhmət olmasa Sazlamalar bölməsinən başlanğıc nömrəni daxil edin.");
                return;
            }
            else
            {
                object objCompute = dt.Compute("MAX(CODE)", null);
                number = (Convert.ToInt32(objCompute) + 1).ToString().PadLeft(4, '0');
                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CASH_ORDER_NUMBER(CODE,INSERT_USER)VALUES('{number}', {GlobalVariables.V_UserID})", "Kassa məxaric orderinin nömrəsi bazaya daxil edilmədi.", this.Name + "/BCashOrder_Click");
            }

            string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_KassaMexaricOrderi.docx",
                   date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime);

            try
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
                object missing = Missing.Value;
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;

                object saveAs = Path.Combine(filePath);

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();

                double d = 0, div = 0;
                int mod = 0;
                d = (double)CreditAmountValue.Value * 100;

                div = (int)(d / 100);
                mod = (int)(d % 100);

                if (credit_currency_id == 1)
                    qep = " " + mod.ToString("00.") + " qəpik";
                else
                    qep = " " + mod.ToString("00.");


                amount_with_word = GlobalFunctions.IntegerToWritten(div) + " " + credit_currency_name + qep;
                amount_with_word2 = div.ToString() + " AZN " + credit_currency_name + "ı " + mod.ToString("00.") + " qəpik";

                GlobalProcedures.FindAndReplace(wordApp, "[$number]", number);
                GlobalProcedures.FindAndReplace(wordApp, "[$amount]", CreditAmountValue.Value.ToString("n2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$amount1]", amount_with_word2);
                GlobalProcedures.FindAndReplace(wordApp, "[$amountwrite]", amount_with_word);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                if (customer_type_id == 1)
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", IssuingDateText.Text);
                    GlobalProcedures.FindAndReplace(wordApp, "[$cardissue]", IssuingText.Text);
                }
                else
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", null);
                    GlobalProcedures.FindAndReplace(wordApp, "[$cardissue]", null);
                }

                GlobalProcedures.FindAndReplace(wordApp, "[$seria]", card_series);
                GlobalProcedures.FindAndReplace(wordApp, "[$card]", card_number);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Replace("/", "") + "_KassaMexaricOrderi.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void InterestValue_EditValueChanged(object sender, EventArgs e)
        {
            if (InterestValue.EditorContainsFocus)
                CalcMonthlyAmount();
        }

        void SearchOrderCode(string code)
        {
            RegistrationCodeSearch.Text = code;
        }

        private void RegistrationCodeSearch_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Customer.FCustomers fc = new Customer.FCustomers();
                fc.TransactionName = "S";
                fc.RefreshDataGridView += new Customer.FCustomers.DoEvent(SearchOrderCode);
                fc.ShowDialog();
            }
        }

        private void BDocumentPacket_Click(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Sənəd paketi üzqabığı.docx");
            if (!File.Exists(fileName.ToString()))
            {
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Sənəd paketinin üzqabığının şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_Uzqabigi.docx";

            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            object saveAs = Path.Combine(filePath);
            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;

            aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            aDoc.Activate();

            try
            {
                GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text.ToUpper());
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                GlobalVariables.WordDocumentUsed = true;

                Process.Start(filePath);
            }
            catch
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Replace("/", "") + "_Uzqabigi.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void DeletePawnBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş girovu silmək istəyirsiniz?", "Girovun silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PAWN_TEMP SET IS_CHANGE = 2 WHERE ID = {pawnID}",
                                                    "Girov temp cədvəldən silinmədi.",
                                               this.Name + "/DeletePawnBarButton_ItemClick");
                LoadPawnDataGridView();
            }
        }

        private void CreditAmountAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            credit_amount_account = (sender as LookUpEdit).EditValue.ToString();
        }

        string getColorName(string value)
        {
            if (value.IndexOf("Uzadılmış") > 0)
                return "Red";
            else
                return "Blue";
        }

        private void PawnGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Pawn_SS, e);
        }

        private void RefreshPawnBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPawnDataGridView();
        }

        private void PrintPawnBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PawnGridControl);
        }

        private void ExportPawnBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PawnGridControl, "xls");
        }

        private void PawnGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Pawn_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell(Pawn_Count, "Center", e);
        }

        private void BConsent_Click(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Razılıq ərizəsi.docx");
            if (!File.Exists(fileName.ToString()))
            {
                GlobalVariables.WordDocumentUsed = consent_click = false;
                XtraMessageBox.Show("Razılıq ərizəsinin şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_Raziliq.docx";

            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            object saveAs = Path.Combine(filePath);
            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;

            aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            aDoc.Activate();

            try
            {
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", date);
                if (customer_type_id == 1)
                    GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib)");
                else
                    GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ")");

                GlobalProcedures.FindAndReplace(wordApp, "[$gdate]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$gcode]", ContractCodeText.Text + "/G");
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyvoen]", GlobalVariables.V_CompanyVoen);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyphone]", GlobalVariables.V_CompanyPhone);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyaddress]", GlobalVariables.V_CompanyAddress);
                GlobalProcedures.FindAndReplace(wordApp, "[$companydirector]", GlobalVariables.V_CompanyDirector);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                GlobalVariables.WordDocumentUsed = consent_click = true;
                Process.Start(filePath);
            }
            catch
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Replace("/", "") + "_Raziliq.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void BPawnContract_Click(object sender, EventArgs e)
        {
            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PAWN_TEMP WHERE CONTRACT_ID = {ContractID}") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Trim() + " saylı kredit müqaviləsi üçün girovlar daxil edilməyib.");
                return;
            }

            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Girov müqaviləsi.docx");
            if (!File.Exists(fileName.ToString()))
            {
                pawn_contract_click = false;
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Girov müqaviləsinin şablon faylı tapılmadı.");
                GlobalProcedures.SplashScreenClose();
                return;
            }

            string qep = null,
                amount_with_word,
                sum_pawn_amount_with_word,
                phone,
                date = GlobalFunctions.DateWithDayMonthYear(ContractStartDate.DateTime),
                filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_GirovMuqavilesi.docx";

            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            object saveAs = Path.Combine(filePath);
            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;

            aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            aDoc.Activate();

            double d = 0, div = 0;
            int mod = 0;
            d = (double)CreditAmountValue.Value * 100;

            div = (int)(d / 100);
            mod = (int)(d % 100);
            if (mod > 0)
            {
                if (credit_currency_id == 1)
                    qep = " " + mod.ToString() + " qəpik";
                else
                    qep = " " + mod.ToString();
            }

            amount_with_word = "(" + GlobalFunctions.IntegerToWritten(div) + " " + credit_currency_name + qep + ")";

            if (sumPawnAmount == 0)
                LoadPawnDataGridView();

            d = (double)sumPawnAmount * 100;

            div = (int)(d / 100);
            mod = (int)(d % 100);
            if (mod > 0)
            {
                if (credit_currency_id == 1)
                    qep = " " + mod.ToString() + " qəpik";
                else
                    qep = " " + mod.ToString();
            }

            sum_pawn_amount_with_word = "(" + GlobalFunctions.IntegerToWritten(div) + " " + credit_currency_name + qep + ")";
            phone = GlobalFunctions.GetName($@"SELECT PHONE FROM CRS_USER.V_PHONE WHERE OWNER_TYPE = '{person_description}' AND OWNER_ID = {CustomerID}");

            try
            {
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCodeText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractdate]", date);
                if (customer_type_id == 1)
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ", " + IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib)");
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", IssuingDateText.Text + " tarixində " + IssuingText.Text + " tərəfindən verilib");
                }
                else
                {
                    GlobalProcedures.FindAndReplace(wordApp, "[$customer]", CustomerFullNameText.Text + " (" + CardDescriptionText.Text + ")");
                    GlobalProcedures.FindAndReplace(wordApp, "[$carddate]", null);
                }
                GlobalProcedures.FindAndReplace(wordApp, "[$card]", CardDescriptionText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractamount]", CreditAmountValue.Value.ToString("N2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$contractamountwrite]", amount_with_word);
                GlobalProcedures.FindAndReplace(wordApp, "[$sumpawnamount]", sumPawnAmount.ToString("N2"));
                GlobalProcedures.FindAndReplace(wordApp, "[$sumpawnamountwrite]", sum_pawn_amount_with_word);
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerFullNameText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$address]", RegistrationAddressText.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$phones]", phone);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyname]", GlobalVariables.V_CompanyName);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyvoen]", GlobalVariables.V_CompanyVoen);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyphone]", GlobalVariables.V_CompanyPhone);
                GlobalProcedures.FindAndReplace(wordApp, "[$companyaddress]", GlobalVariables.V_CompanyAddress);
                GlobalProcedures.FindAndReplace(wordApp, "[$companydirector]", GlobalVariables.V_CompanyDirector);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                GlobalVariables.WordDocumentUsed = pawn_contract_click = true;
                Process.Start(filePath);
            }
            catch
            {
                GlobalProcedures.KillWord();
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage(ContractCodeText.Text.Replace("/", "") + "_Raziliq.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.");
            }
            finally
            {
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void PawnGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void PaymentSchedulesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PaymentSchedulesGridView.GetFocusedDataRow();
            if (row != null)
                sheduleVersion = Convert.ToInt32(row["VERSION"]);
        }

        private void NewPawnBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPawnAddEdit("INSERT", null);
        }

        void RefreshPawn()
        {
            LoadPawnDataGridView();
        }

        private void LoadFPawnAddEdit(string transaction, int? id)
        {
            if (CreditAmountValue.Value <= 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CreditAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kreditin məbləği sıfırdan böyük olmalıdır.");
                CreditAmountValue.Focus();
                CreditAmountValue.BackColor = GlobalFunctions.ElementColor();
                return;
            }

            topindex = PawnGridView.TopRowIndex;
            old_row_num = PawnGridView.FocusedRowHandle;
            FPawnAddEdit fp = new FPawnAddEdit();
            fp.TransactionName = transaction;
            fp.ID = id;
            fp.ContractID = ContractID;
            fp.CreditAmount = CreditAmountValue.Value;
            fp.Currency = CreditCurrencyLookUp.Text;
            fp.RefreshDataGridView += new FPawnAddEdit.DoEvent(RefreshPawn);
            fp.ShowDialog();
            PawnGridView.TopRowIndex = topindex;
            PawnGridView.FocusedRowHandle = old_row_num;
        }

        private void PawnGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditPawnBarButton.Enabled = DeletePawnBarButton.Enabled = (PawnGridView.RowCount > 0);
        }

        private void PawnGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PawnGridView, PawnPopupMenu, e);
        }

        private void EditPawnBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPawnAddEdit("EDIT", pawnID);
        }

        private void PawnGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPawnBarButton.Enabled)
                LoadFPawnAddEdit("EDIT", pawnID);
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
                fs.ShowDialog();
            }
        }

        private void PaymentSchedulesGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Shedule_SS")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
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
            LoadPhoneDataGridView(GuaranteeID);
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
                    command.Parameters.Add("P_NEW_SELLER_ID", OracleDbType.Int32).Value = GuaranteeID;
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

        private void SellerTypeRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                SellerType();
                LoadPhoneDataGridView(GuaranteeID);
            }
        }

        private void BDeleteContractPicture_Click(object sender, EventArgs e)
        {
            int currentindex = ContractImageSlider.CurrentImageIndex;
            if (currentindex == -1)
                currentindex = 0;
            ContractImageSlider.Images.Remove(ContractImageSlider.Images[currentindex]);
            image_list.Remove(image_list[currentindex]);
            ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkil";

            if (ContractImageSlider.Images.Count == 0)
                BDeleteContractPicture.Enabled = BChangeContractPicture.Enabled = false;
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
                    ImageCountLabel.Text = ContractImageSlider.Images.Count + " şəkildən";
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

                string commission = (CommissionValue.Value > 0) ? "\n<b>Komissiya = " + CommissionValue.Value.ToString("N2") + " " + CreditCurrencyLookUp.Text + "</b>, Hesab = " + commission_account : null;

                DialogResult dialogResult = XtraMessageBox.Show("<b>Aşağıda qeyd olunmuş dəyərlərlə " + ContractCodeText.Text + " saylı müqaviləni təsdiqləmək istəyirsiniz?</b>" +
                                                                    "\n\nİllik faiz = " + InterestValue.Value + " %" +
                                                                    "\nMüddəti = " + PeriodValue.Value + " ay" +
                                                                    commission +
                                                                    "\n<b>Məbləğ = " + CreditAmountValue.Value.ToString("N2") + " " + CreditCurrencyLookUp.Text + "</b>, Hesab = " + credit_amount_account +
                                                                    "\n<b>FİFD = " + FifdValue.Value.ToString("N2") + " %</b>" +
                                                                    ((extend != null) ? "\n\n" + extend : null), ContractCodeText.Text + " saylı müqavilənin təsdiqlənməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FContractSaveWait));
                    GlobalProcedures.InsertContractOperationJournal(ContractStartDate.Text, (double)CreditAmountValue.Value, (double)CommissionValue.Value, ContractID, credit_amount_account, commission_account);
                    GlobalProcedures.CalculatedLeasingTotal(ContractID);
                    GlobalProcedures.SplashScreenClose();
                    this.Close();
                }
            }
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

        private void LoadPawnDataGridView()
        {
            string s = $@"SELECT P.ID,
                                   PT.NAME PAWN_TYPE_NAME,
                                   P.COUNT,
                                   GT.NAME GOLD_TYPE_NAME,
                                   P.WEIGHT,
                                   P.CWEIGHT,
                                   P.AMOUNT,
                                   P.GOLD_AMOUNT,
                                   P.EYEBROWS_WEIGHT,
                                   P.EYEBROWS_AMOUNT,
                                   ET.NAME EYEBROWS_TYPE_NAME,     
                                   P.TOTAL_AMOUNT,
                                   P.NOTE
                              FROM CRS_USER_TEMP.PAWN_TEMP P,
                                   CRS_USER.PAWN_TYPE PT,
                                   CRS_USER.GOLD_TYPE GT,
                                   CRS_USER.EYEBROWS_TYPE ET
                             WHERE P.IS_CHANGE != 2 
                               AND P.GOLD_TYPE_ID = GT.ID 
                               AND P.PAWN_TYPE_ID = PT.ID 
                               AND P.EYEBROWS_TYPE_ID = ET.ID
                               AND P.CONTRACT_ID = {ContractID}
                          ORDER BY P.ID";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPawnDataGridView", "Girovların siyahısı yüklənmədi.");
            PawnGridControl.DataSource = dt;

            if (PawnGridView.RowCount > 0)
            {
                EditPawnBarButton.Enabled = GlobalVariables.EditPower;
                DeletePawnBarButton.Enabled = GlobalVariables.DeletePower;
                DataView dv = new DataView();
                dv = new DataView(dt);

                object sumObject = dt.Compute("Sum(GOLD_AMOUNT)", dv.RowFilter);
                object sumTotalObject = dt.Compute("Sum(TOTAL_AMOUNT)", dv.RowFilter);
                sumPawnAmount = Convert.ToDecimal(sumObject);
                sumWeight = Convert.ToDecimal(dt.Compute("Sum(CWEIGHT)", dv.RowFilter));
                sumTotalAmount = Convert.ToDecimal(sumTotalObject);
            }
            else
            {
                EditPawnBarButton.Enabled =
                    DeletePawnBarButton.Enabled = false;
                sumPawnAmount = sumWeight = 0;
            }
        }

        private void InsertPawnTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_PAWN_TEMP", "P_CONTRACT_ID", ContractID, "Girovlar temp cədvələ daxil edilmədi.");
        }

        private void InsertInterestPenaltiesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_PENELTIES_TEMP", "P_CONTRACT_ID", ContractID, "Cərimə faizləri temp cədvələ daxil edilmədi.");
        }

        private void PowerGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PawnGridView.GetFocusedDataRow();
            if (row != null)
                pawnID = int.Parse(row["ID"].ToString());
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
            string file_name = null, code = "01", sql = null, filePath = null;
            if (GlobalVariables.WordDocumentUsed)
            {
                GlobalProcedures.SplashScreenClose();
                XtraMessageBox.Show("Açıq olan bütün word fayllar avtomatik olaraq bağlanılacaq.", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GlobalProcedures.KillWord();
                GlobalVariables.WordDocumentUsed = false;
            }

            //ProgressPanel.Description = "Hazırlanmış fayllar bazaya yüklənir...";

            #region muqavile
            filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_Müqavilə.docx";
            if (contract_click && File.Exists(filePath))
            {

                file_name = Path.GetFileName(filePath);
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

                GlobalFunctions.ExecuteQueryWithBlob(sql, filePath,
                                                        "Müqavilən hazır çap faylı bazaya daxil edilmədi.");

            }
            #endregion

            #region girov müqaviləsi
            filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_GirovMuqavilesi.docx";
            if (pawn_contract_click && File.Exists(filePath))
            {
                file_name = Path.GetFileName(filePath);
                code = DocumentCode(2);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                                   VALUES({ContractID},2,:BlobFile,'{code}','{file_name}')";
                GlobalFunctions.ExecuteQueryWithBlob(sql, filePath,
                                                        "Girov müqaviləsinin hazır çap faylı bazaya daxil edilmədi.");

            }
            #endregion

            #region girov müqaviləsinə əlavə
            filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_GirovMuqavilesineElave.docx";
            if (pawn_list_click && File.Exists(filePath))
            {
                file_name = Path.GetFileName(filePath);
                code = DocumentCode(3);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                             VALUES({ContractID},3,:BlobFile,'{code}','{file_name}')";
                GlobalFunctions.ExecuteQueryWithBlob(sql, filePath,
                                                        "Girov müqaviləsinə əlavələrin hazır çap faylı bazaya daxil edilmədi.");
            }
            #endregion

            #region razılıq ərizəsi
            filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_Raziliq.docx";
            if (consent_click && File.Exists(filePath))
            {
                file_name = Path.GetFileName(filePath);
                code = DocumentCode(4);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                                      VALUES({ContractID},4,:BlobFile,'{code}','{file_name}')";
                GlobalFunctions.ExecuteQueryWithBlob(sql, filePath,
                                                        "Razılıq ərizəsinin hazır çap faylı bazaya daxil edilmədi.");
            }
            #endregion

            #region qiymətləndirmə aktı
            filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCodeText.Text.Replace("/", "") + "_QiymetlendirmeAkti.docx";
            if (akt_click && File.Exists(filePath))
            {
                file_name = Path.GetFileName(filePath);
                code = DocumentCode(5);
                sql = $@"INSERT INTO CRS_USER.CONTRACT_DOCUMENTS(CONTRACT_ID,
                                                                 DOCUMENT_TYPE_ID,
                                                                 DOCUMENT_FILE,
                                                                 CODE,
                                                                 FILE_NAME) 
                                      VALUES({ContractID},5,:BlobFile,'{code}','{file_name}')";
                GlobalFunctions.ExecuteQueryWithBlob(sql, filePath,
                                                        "Qiymətləndirmə aktının hazır çap faylı bazaya daxil edilmədi.");
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
                monthly_amount = GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, diff_month - (int)GracePeriodValue.Value, (double)InterestValue.Value);
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
            changecode = close;
            if (close)
                ContractCodeText.Text = (code + "/" + DateTime.Today.ToString("yy")).Trim();
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
                    InterestValue.Properties.ReadOnly = CurrentStatus;
                else
                    InterestValue.Properties.ReadOnly = true;
                if (!CurrentStatus)
                    InterestValue.Focus();
                if (check_interest_value > -1)
                    InterestValue.EditValue = check_interest_value;
            }
            else
            {
                InterestValue.EditValue = default_interest;
                InterestValue.Properties.ReadOnly = true;
            }
            InterestValue.TabStop = !InterestValue.Properties.ReadOnly;
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
            crop.PictureOwner = "S" + GuaranteeID;
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
            LoadPhoneDataGridView(GuaranteeID);
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "down", out orderid);
            LoadPhoneDataGridView(GuaranteeID);
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

        private void ControlJournal()
        {
            if (GlobalVariables.V_UserID > 0)
                BJournal.Visible = (Commit == 1 && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.OPERATION_JOURNAL WHERE ACCOUNT_OPERATION_TYPE_ID = 2 AND CONTRACT_ID = {ContractID}") == 0);
            else
                BJournal.Visible = true;
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
                labelControl20.Visible =
                labelControl24.Visible =
                labelControl28.Visible =
                labelControl29.Visible =
                labelControl30.Visible =
                SellerIssuingLookUp.Visible =
                SellerIssuingLabel.Visible =
                SellerFrontFaceLabel.Visible =
                SellerRearFaceLabel.Visible =
                ExcampleCardLabel.Visible = (SellerTypeRadioGroup.SelectedIndex == 0);

            BSearcSeller.Visible = (SellerTypeRadioGroup.Enabled || TransactionName == "INSERT");

            if (SellerTypeRadioGroup.SelectedIndex == 0)
            {
                GlobalProcedures.FillLookUpEdit(SellerSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                if (TransactionName == "INSERT")
                    SellerSeriesLookUp.EditValue = SellerSeriesLookUp.Properties.GetKeyValueByDisplayText("AZE");
                GlobalProcedures.FillLookUpEdit(SellerIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                if (new_individual_seller_id == null)
                    new_individual_seller_id = GlobalFunctions.GetOracleSequenceValue("SELLER_SEQUENCE").ToString();
                GuaranteeID = new_individual_seller_id;

                PhoneGroupControl.Location = new Point(155, 314);
                PhoneDescriptionLabel.Location = new Point(531, 500);
                SellerSurnameText.Focus();
                seller_type = "S";
                seller_type_id = 1;
                BLoadSellerPicture.Visible = BDeleteSellerPicture.Visible = SellerPictureBox.Enabled = true;
            }
            else
            {
                if (new_juridical_seller_id == null)
                    new_juridical_seller_id = GlobalFunctions.GetOracleSequenceValue("JURIDICAL_PERSONS_SEQUENCE").ToString();
                GuaranteeID = new_juridical_seller_id;
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
                PhoneDescriptionLabel.Location = new Point(531, 344);
                labelControl12.Location = new Point(143, 131);
            }
        }
    }
}