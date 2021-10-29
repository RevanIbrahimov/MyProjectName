using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Globalization;
using Oracle.ManagedDataAccess.Client;
using CRS.Class.Tables;
using System.Diagnostics;

namespace CRS.Class
{
    class GlobalVariables
    {
        public static string V_CompanyPhone;
        public static string V_CompanyAddress;
        public static string V_CompanyDirector;
        public static string V_CompanyVoen;
        public static string V_CompanyName;
        public static string V_Version = "1.0.2.3";
        public static string SuperAdminPassword = "@q1LEAS!NGw22015e3LRSr4";
        public static string V_StyleName;
        public static string V_CurrentDate;
        public static string V_ConnectionString = null;
        public static string V_LastRate = null;
        public static int V_UserID = 0; 
        public static string SelectedLanguage;
        public static string V_ExecutingFolder;
        public static string V_UserName = "SuperAdmin";
        public static string V_Connect_User;        
        public static int V_DefaultMenu = 0;
        public static int V_DefaultLanguage = 0;
        public static int V_DefaultDateSort = 0;
        public static List<LrsUsers> lstUsers = null;        
        public static bool V_FConnect_BOK_Click = false;
        public static CultureInfo V_CultureInfoEN = new CultureInfo("en-US");
        public static CultureInfo V_CultureInfoAZ = new CultureInfo("az-Latn-AZ");
        public static int V_License;
        public static bool WordDocumentUsed = false;
        public static int V_CommitmentCount = 0;
        public static Process runSmsConsole = new Process();
        //color
        public static int V_BlockColor1, V_BlockColor2;
        public static int V_CloseColor1, V_CloseColor2;
        public static int V_ConnectColor1, V_ConnectColor2;
        public static int V_CommitColor1, V_CommitColor2;
        public static int V_CalculatedColumnColor1, V_CalculatedColumnColor2;
        public static int V_IncomeColor1, V_IncomeColor2;
        public static int V_ExpensesColor1, V_ExpensesColor2;
        public static int V_ContractNotExpensesColor1, V_ContractNotExpensesColor2;
        public static int V_ContractLawCloseColor1, V_ContractLawCloseColor2;
        public static int V_ExtendColor1, V_ExtendColor2;

        //Users
        public static bool User = false;
        public static bool AddUser = false;
        public static bool EditUser = false;
        public static bool DeleteUser = false;
        public static bool UnlockUser = false;
        
        public static bool UsersGroup = false;
        public static bool AddUserGroup = false;
        public static bool EditUserGroup = false;
        public static bool DeleteUserGroup = false;
        public static bool CopyUserGroup = false;

        //Info
        public static bool Info = true;

        //Customer
        public static bool Customer = false;
        public static bool AddCustomer = false;
        public static bool EditCustomer = false;
        public static bool DeleteCustomer = false;
        public static bool PrintCustomer = false;
        public static bool ExportCustomer = false;
        public static bool SendMailCustomer = false;
        public static bool SendSmsCustomer = false;
        public static bool EditCustomerCode = false;
        public static bool ShowVoenList = false;

        //Contract
        public static bool Contract = false;
        public static bool AddContract = false;
        public static bool AddAgreement = false;
        public static bool EditContract = false;
        public static bool DeleteContract = false;
        public static bool PrintContract = false;
        public static bool ExportContract = false;
        public static bool CommitContract = false;
        public static bool EditContractCode = false;
        public static bool ChangeCurrencyRate = false;
        public static bool AddCommitment = false;
        public static bool EditCommitment = false;
        public static bool DeleteCommitment = false;
        public static bool AddPower = false;
        public static bool EditPower = false;
        public static bool DeletePower = false;
        public static bool AddInsurance = false;
        public static bool EditInsurance = false;
        public static bool DeleteInsurance = false;
        public static bool CancelInsurance = false;
        public static bool AddInterestPenalties = false;
        public static bool EditInterestPenalties = false;
        public static bool DeleteInterestPenalties = false;
        public static bool AddLawDetail = false;
        public static bool EditLawDetail = false;
        public static bool DeleteLawDetail = false;
        public static bool AddProsecute = false;
        public static bool CloseProsecute = false;
        public static bool OpenContract = false;
        public static bool CloseContract = false;
        public static bool ContractCashPayment = false;
        public static bool ContractBankPayment = false;
        public static bool AddSpecialAttention = false;
        public static bool DeleteSpecialAttention = false;
        public static bool AddExtend = false;
        public static bool EditExtend = false;
        public static bool DeleteExtend = false;
        public static bool InsurancePayment = false;
        public static bool DeleteInsurancePayment = false;
        public static bool InsuranceCompensation = false;
        public static bool InsuranceTransfer = false;
        public static bool EditInsuranceTransfer = false;
        public static bool DeleteInsuranceTransfer = false;
        public static bool CancelContract = false;
        public static bool AddCommon = false;
        public static bool DeleteCommon = false;
        public static bool AddCommonOrder = false;
        public static bool EditCommonOrder = false;
        public static bool DeleteCommonOrder = false;

        //Portfel
        public static bool Portfel = false;
        public static bool AddPayment = false;
        public static bool AddPaymentToBank = false;
        public static bool AddPaymentToCash = false;
        public static bool AddPaymentToOther = false;
        public static bool EditPayment = false;
        public static bool DeletePayment = false;
        public static bool PrintPortfel = false;
        public static bool ExportPortfel = false;
        public static bool ChangeColor = false;
        public static bool BankToCash = false;
        public static bool CashToBank = false;
        public static bool DeleteWarningLetter = false;
        public static bool ViewReports = false;
        public static bool CheckPenaltyDebt = false;

        //cash
        public static bool Cash = false;
        public static bool AddCashIncome = false;
        public static bool AddCashExpenses = false;
        public static bool EditCash = false;
        public static bool DeleteCash = false;
        public static bool PrintCash = false;
        public static bool ExportCash = false;
        public static bool ReceiptCash = false;

        //bank
        public static bool Bank = false;
        public static bool AddBankIncome = false;
        public static bool AddBankExpenses = false;
        public static bool EditBank = false;
        public static bool DeleteBank = false;
        public static bool PrintBank = false;
        public static bool ExportBank = false;
        public static bool RepeatBank = false;
        public static bool BankAccount = false;
        public static bool AddAccount = false;
        public static bool EditAccount = false;
        public static bool DeleteAccount = false;

        //dictionaries
        public static bool Dictionaries = false;
        public static bool PhoneDescription = false;
        public static bool Status = false;
        public static bool CardSeries = false;
        public static bool CardIssuing = false;
        public static bool Birthplace = false;
        public static bool CreditType = false;
        public static bool CreditName = false;
        public static bool Currency = false;
        public static bool Hostage = false;
        public static bool Banks = false;
        public static bool BankOperation = false;
        public static bool Countries = false;
        public static bool CashOperations = false;
        public static bool Personnel = false;
        public static bool AttractedFunds = false;
        public static bool Founders = false;
        public static bool Positions = false;
        public static bool Insurance = false;

        //funds
        public static bool Funds = false;
        public static bool FundContract = false;        
        public static bool AddFundContract = false;
        public static bool EditFundContract = false;
        public static bool DeleteFundContract = false;
        public static bool FundPayment = false;
        public static bool AddBuyAmount = false;
        public static bool EditBuyAmount = false;
        public static bool AddFundPayment = false;
        public static bool EditFundPayment = false;
        public static bool DeleteFund = false;
        public static bool OpenFundContract = false;
        public static bool CloseFundContract = false;
        public static bool AddFundPercent = false;
        public static bool EditFundPercent = false;
        public static bool DeleteFundPercent = false;

        //bookkeeping
        public static bool Bookkeeping = false;
        public static bool ShowJournal = false;
        public static bool AddJournal = false;
        public static bool EditJournal = false;
        public static bool DeleteJournal = false;
        public static bool CopyJournal = false;
        public static bool ShowDetails = false;
        public static bool ShowAccountPlan = false;
        public static bool AddAccountPlan = false;
        public static bool EditAccountPlan = false;
        public static bool DeleteAccountPlan = false;
        public static bool ShowDebt = false;
        public static bool AddDebt = false;
        public static bool EditDebt = false;
        public static bool DeleteDebt = false;
        public static bool ShowProfits = false;
        public static bool AgainCalculated = false;
        public static bool OperationHistory = false;

        //payment task
        public static bool PaymentTask = false;
        public static bool AddTask = false;
        public static bool EditTask = false;
        public static bool DeleteTask = false;
        public static bool PaymentTaskTemplate = false;
        public static bool AddTemplate = false;
        public static bool EditTemplate = false;
        public static bool DeleteTemplate = false;
        public static bool AddTaskType = false;
        public static bool EditTaskType = false;
        public static bool DeleteTaskType = false;
        public static bool AddAcceptor = false;
        public static bool EditAcceptor = false;
        public static bool DeleteAcceptor = false;
        public static bool EditVatBank = false;

        //options
        public static bool EditTemplateFile = false;

        //sms
        public static bool Sms = false;
    }
}
