using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;
using DevExpress.XtraEditors;
using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;
using System.Configuration;
using System.Diagnostics;

namespace CRS
{
    public partial class FConnect : DevExpress.XtraEditors.XtraForm
    {
        public FConnect()
        {
            GlobalVariables.SelectedLanguage = "AZ";
            InitializeComponent();
        }
        int saved = 0;
        bool FormStatus = false;

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalVariables.lstUsers = LrsUsersDAL.SelectUser(null).ToList<LrsUsers>();
            if (ControlLoginParametr())
            {
                if (SaveCheck.Checked)
                {
                    GlobalProcedures.SetSetting("SavedLoginName", UserNameText.Text.Trim());
                    GlobalProcedures.SetSetting("SavedLoginPassword", GlobalFunctions.Encrypt(PasswordText.Text.Trim()));
                    GlobalProcedures.SetSetting("SaveLogin", "1");
                }
                else
                {
                    GlobalProcedures.SetSetting("SavedLoginName", null);
                    GlobalProcedures.SetSetting("SavedLoginPassword", null);
                    GlobalProcedures.SetSetting("SaveLogin", "0");
                }
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER_TEMP.PROC_DELETE_ALL_TEMP", "P_USED_USER_ID", GlobalVariables.V_UserID, "Temp cədvəllə silinmədi.");
                GenerateUserPermisions();
                GlobalProcedures.LoadLrsColor();
                GlobalProcedures.InsertUserConnection();
                //session_id-ni cedvele yaz
                GlobalProcedures.UpdateUserConnected();
                if (AzCheck.Checked)
                    GlobalVariables.SelectedLanguage = "AZ";
                else if (EnCheck.Checked)
                    GlobalVariables.SelectedLanguage = "EN";
                else if (RuCheck.Checked)
                    GlobalVariables.SelectedLanguage = "RU";
                GlobalVariables.V_FConnect_BOK_Click = true;
                this.Hide();


                Forms.FMain fm = new Forms.FMain();
                fm.ShowDialog();

                
            }
        }

        private bool ControlLoginParametr()
        {
            bool b = false;

            if (UserNameText.Text.Length == 0)
            {
                UserNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçi adı daxil edilməyib.");
                UserNameText.Focus();
                UserNameText.BackColor = Color.White;
                return false;
            }
            else if (UserNameText.Text.Trim() != "SuperAdmin")               
            {
                var user = GlobalVariables.lstUsers.Find(u => u.STATUS_ID != 2 && u.NIKNAME == GlobalFunctions.Encrypt(UserNameText.Text.Trim()));
                if (user == null)
                {
                    GlobalProcedures.ShowErrorMessage("İstifadəçi adı ya düz deyil ya da bu istifadəçinin sistemə girişi bağlanılıb.");
                    return false;
                }

                if (user.SESSION_ID != 0)
                {
                    UserNameText.BackColor = Color.Red;
                    XtraMessageBox.Show(UserNameText.Text + " adlı istifadəçi artıq sistemə daxil olub. Zəhmət olmasa başqa istifadəçi adı ilə sistemə daxil olun", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UserNameText.Focus();
                    UserNameText.BackColor = Color.White;
                    return false;
                }

                if (PasswordText.Text.Length == 0)
                {
                    PasswordText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Şifrə daxil edilməyib.");
                    PasswordText.Focus();
                    PasswordText.BackColor = Color.White;
                    return false;
                }               
                else if (user.PASSWORD == GlobalFunctions.Encrypt(PasswordText.Text.Trim()))
                    b = true;
                else
                {
                    PasswordText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Şifrə düz deyil.");
                    PasswordText.Focus();
                    PasswordText.BackColor = Color.White;
                    return false;
                }
               

                GlobalVariables.V_UserName = UserNameText.Text.Trim();
                GlobalVariables.V_UserID = user.ID;
            }
            else if (PasswordText.Text.Trim() == GlobalVariables.SuperAdminPassword)
                b = true;
            else
            {
                PasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şifrə düz deyil.");
                PasswordText.Focus();
                PasswordText.BackColor = Color.White;
                return false;
            }


            return b;
        }

        private void GenerateUserPermisions() // istifadecinin huquqlarinin teyin edilmesi
        {
            string s = $@"SELECT RD.ROLE_ID, RD.DETAIL_NAME
                              FROM CRS_USER.USER_GROUP_PERMISSION UGP,
                                   CRS_USER.ALL_USER_GROUP_ROLE_DETAILS GRP,
                                   CRS_USER.ALL_ROLE_DETAILS RD
                             WHERE     UGP.GROUP_ID = GRP.GROUP_ID
                                   AND GRP.ROLE_DETAIL_ID = RD.ID
                                   AND UGP.USER_ID = {GlobalVariables.V_UserID}";

            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/GenerateUserPermisions").Rows)
                {
                    switch (int.Parse(dr[0].ToString()))
                    {
                        case 1:
                            {
                                GlobalVariables.User = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddUser":
                                        GlobalVariables.AddUser = true;
                                        break;
                                    case "EditUser":
                                        GlobalVariables.EditUser = true;
                                        break;
                                    case "DeleteUser":
                                        GlobalVariables.DeleteUser = true;
                                        break;
                                    case "UnlockUser":
                                        GlobalVariables.UnlockUser = true;
                                        break;
                                }
                            }
                            break;
                        case 2:
                            {
                                GlobalVariables.UsersGroup = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddUserGroup":
                                        GlobalVariables.AddUserGroup = true;
                                        break;
                                    case "EditUserGroup":
                                        GlobalVariables.EditUserGroup = true;
                                        break;
                                    case "DeleteUserGroup":
                                        GlobalVariables.DeleteUserGroup = true;
                                        break;
                                    case "CopyUserGroup":
                                        GlobalVariables.CopyUserGroup = true;
                                        break;
                                }
                            }
                            break;
                        case 3:
                            {
                                GlobalVariables.Customer = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddCustomer":
                                        GlobalVariables.AddCustomer = true;
                                        break;
                                    case "EditCustomer":
                                        GlobalVariables.EditCustomer = true;
                                        break;
                                    case "DeleteCustomer":
                                        GlobalVariables.DeleteCustomer = true;
                                        break;
                                    case "PrintCustomer":
                                        GlobalVariables.PrintCustomer = true;
                                        break;
                                    case "ExportCustomer":
                                        GlobalVariables.ExportCustomer = true;
                                        break;
                                    case "SendMailCustomer":
                                        GlobalVariables.SendMailCustomer = true;
                                        break;
                                    case "SendSmsCustomer":
                                        GlobalVariables.SendSmsCustomer = true;
                                        break;
                                    case "EditCustomerCode":
                                        GlobalVariables.EditCustomerCode = true;
                                        break;
                                    case "ShowVoenList":
                                        GlobalVariables.ShowVoenList = true;
                                        break;
                                }
                            }
                            break;
                        case 4:
                            {
                                GlobalVariables.Contract = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddContract":
                                        GlobalVariables.AddContract = true;
                                        break;
                                    case "AddAgreement":
                                        GlobalVariables.AddAgreement = true;
                                        break;
                                    case "EditContract":
                                        GlobalVariables.EditContract = true;
                                        break;
                                    case "DeleteContract":
                                        GlobalVariables.DeleteContract = true;
                                        break;
                                    case "PrintContract":
                                        GlobalVariables.PrintContract = true;
                                        break;
                                    case "ExportContract":
                                        GlobalVariables.ExportContract = true;
                                        break;
                                    case "CommitContract":
                                        GlobalVariables.CommitContract = true;
                                        break;
                                    case "EditContractCode":
                                        GlobalVariables.EditContractCode = true;
                                        break;
                                    case "ChangeCurrencyRate":
                                        GlobalVariables.ChangeCurrencyRate = true;
                                        break;
                                    case "AddCommitment":
                                        GlobalVariables.AddCommitment = true;
                                        break;
                                    case "EditCommitment":
                                        GlobalVariables.EditCommitment = true;
                                        break;
                                    case "DeleteCommitment":
                                        GlobalVariables.DeleteCommitment = true;
                                        break;
                                    case "AddPower":
                                        GlobalVariables.AddPower = true;
                                        break;
                                    case "EditPower":
                                        GlobalVariables.EditPower = true;
                                        break;
                                    case "DeletePower":
                                        GlobalVariables.DeletePower = true;
                                        break;
                                    case "AddInterestPenalties":
                                        GlobalVariables.AddInterestPenalties = true;
                                        break;
                                    case "EditInterestPenalties":
                                        GlobalVariables.EditInterestPenalties = true;
                                        break;
                                    case "DeleteInterestPenalties":
                                        GlobalVariables.DeleteInterestPenalties = true;
                                        break;
                                    case "AddInsurance":
                                        GlobalVariables.AddInsurance = true;
                                        break;
                                    case "EditInsurance":
                                        GlobalVariables.EditInsurance = true;
                                        break;
                                    case "DeleteInsurance":
                                        GlobalVariables.DeleteInsurance = true;
                                        break;
                                    case "CancelInsurance":
                                        GlobalVariables.CancelInsurance = true;
                                        break;
                                    case "AddLawDetail":
                                        GlobalVariables.AddLawDetail = true;
                                        break;
                                    case "EditLawDetail":
                                        GlobalVariables.EditLawDetail = true;
                                        break;
                                    case "DeleteLawDetail":
                                        GlobalVariables.DeleteLawDetail = true;
                                        break;
                                    case "AddProsecute":
                                        GlobalVariables.AddProsecute = true;
                                        break;
                                    case "CloseProsecute":
                                        GlobalVariables.CloseProsecute = true;
                                        break;
                                    case "OpenContract":
                                        GlobalVariables.OpenContract = true;
                                        break;
                                    case "CloseContract":
                                        GlobalVariables.CloseContract = true;
                                        break;
                                    case "ContractCashPayment":
                                        GlobalVariables.ContractCashPayment = true;
                                        break;
                                    case "ContractBankPayment":
                                        GlobalVariables.ContractBankPayment = true;
                                        break;
                                    case "AddSpecialAttention":
                                        GlobalVariables.AddSpecialAttention = true;
                                        break;
                                    case "DeleteSpecialAttention":
                                        GlobalVariables.DeleteSpecialAttention = true;
                                        break;
                                    case "AddExtend":
                                        GlobalVariables.AddExtend = true;
                                        break;
                                    case "EditExtend":
                                        GlobalVariables.EditExtend = true;
                                        break;
                                    case "DeleteExtend":
                                        GlobalVariables.DeleteExtend = true;
                                        break;
                                    case "InsurancePayment":
                                        GlobalVariables.InsurancePayment = true;
                                        break;
                                    case "DeleteInsurancePayment":
                                        GlobalVariables.DeleteInsurancePayment = true;
                                        break;
                                    case "InsuranceCompensation":
                                        GlobalVariables.InsuranceCompensation = true;
                                        break;
                                    case "InsuranceTransfer":
                                        GlobalVariables.InsuranceTransfer = true;
                                        break;
                                    case "EditInsuranceTransfer":
                                        GlobalVariables.EditInsuranceTransfer = true;
                                        break;
                                    case "DeleteInsuranceTransfer":
                                        GlobalVariables.DeleteInsuranceTransfer = true;
                                        break;
                                    case "CancelContract":
                                        GlobalVariables.CancelContract = true;
                                        break;
                                    case "AddCommon":
                                        GlobalVariables.AddCommon = true;
                                        break;
                                    case "DeleteCommon":
                                        GlobalVariables.DeleteCommon = true;
                                        break;
                                    case "AddCommonOrder":
                                        GlobalVariables.AddCommonOrder = true;
                                        break;
                                    case "EditCommonOrder":
                                        GlobalVariables.EditCommonOrder = true;
                                        break;
                                    case "DeleteCommonOrder":
                                        GlobalVariables.DeleteCommonOrder = true;
                                        break;
                                }
                            }
                            break;
                        case 5:
                            {
                                GlobalVariables.Portfel = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddPayment":
                                        GlobalVariables.AddPayment = true;
                                        break;
                                    case "AddPaymentToBank":
                                        GlobalVariables.AddPaymentToBank = true;
                                        break;
                                    case "AddPaymentToCash":
                                        GlobalVariables.AddPaymentToCash = true;
                                        break;
                                    case "AddPaymentToOther":
                                        GlobalVariables.AddPaymentToOther = true;
                                        break;
                                    case "EditPayment":
                                        GlobalVariables.EditPayment = true;
                                        break;
                                    case "DeletePayment":
                                        GlobalVariables.DeletePayment = true;
                                        break;
                                    case "PrintPortfel":
                                        GlobalVariables.PrintPortfel = true;
                                        break;
                                    case "ExportPortfel":
                                        GlobalVariables.ExportPortfel = true;
                                        break;
                                    case "ChangeColor":
                                        GlobalVariables.ChangeColor = true;
                                        break;
                                    case "BankToCash":
                                        GlobalVariables.BankToCash = true;
                                        break;
                                    case "CashToBank":
                                        GlobalVariables.CashToBank = true;
                                        break;
                                    case "DeleteWarningLetter":
                                        GlobalVariables.DeleteWarningLetter = true;
                                        break;
                                    case "ViewReports":
                                        GlobalVariables.ViewReports = true;
                                        break;
                                    case "CheckPenaltyDebt":
                                        GlobalVariables.CheckPenaltyDebt = true;
                                        break;
                                }
                            }
                            break;
                        case 6:
                            {
                                GlobalVariables.Cash = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddCashIncome":
                                        GlobalVariables.AddCashIncome = true;
                                        break;
                                    case "AddCashExpenses":
                                        GlobalVariables.AddCashExpenses = true;
                                        break;
                                    case "EditCash":
                                        GlobalVariables.EditCash = true;
                                        break;
                                    case "DeleteCash":
                                        GlobalVariables.DeleteCash = true;
                                        break;
                                    case "PrintCash":
                                        GlobalVariables.PrintCash = true;
                                        break;
                                    case "ExportCash":
                                        GlobalVariables.ExportCash = true;
                                        break;
                                    case "ReceiptCash":
                                        GlobalVariables.ReceiptCash = true;
                                        break;
                                }
                            }
                            break;
                        case 7:
                            {
                                GlobalVariables.Bank = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddBankIncome":
                                        GlobalVariables.AddBankIncome = true;
                                        break;
                                    case "AddBankExpenses":
                                        GlobalVariables.AddBankExpenses = true;
                                        break;
                                    case "EditBank":
                                        GlobalVariables.EditBank = true;
                                        break;
                                    case "DeleteBank":
                                        GlobalVariables.DeleteBank = true;
                                        break;
                                    case "PrintBank":
                                        GlobalVariables.PrintBank = true;
                                        break;
                                    case "ExportBank":
                                        GlobalVariables.ExportBank = true;
                                        break;
                                    case "AddAccount":
                                        {
                                            GlobalVariables.AddAccount = true;
                                            GlobalVariables.BankAccount = true;
                                        }
                                        break;
                                    case "EditAccount":
                                        {
                                            GlobalVariables.EditAccount = true;
                                            GlobalVariables.BankAccount = true;
                                        }
                                        break;
                                    case "DeleteAccount":
                                        {
                                            GlobalVariables.DeleteAccount = true;
                                            GlobalVariables.BankAccount = true;
                                        }
                                        break;
                                    case "RepeatBank":
                                        GlobalVariables.RepeatBank = true;
                                        break;
                                }
                            }
                            break;
                        case 8:
                            {
                                GlobalVariables.Dictionaries = true;
                                switch (dr[1].ToString())
                                {
                                    case "PhoneDescription":
                                        GlobalVariables.PhoneDescription = true;
                                        break;
                                    case "Status":
                                        GlobalVariables.Status = true;
                                        break;
                                    case "CardSeries":
                                        GlobalVariables.CardSeries = true;
                                        break;
                                    case "CardIssuing":
                                        GlobalVariables.CardIssuing = true;
                                        break;
                                    case "Birthplace":
                                        GlobalVariables.Birthplace = true;
                                        break;
                                    case "CreditType":
                                        GlobalVariables.CreditType = true;
                                        break;
                                    case "CreditName":
                                        GlobalVariables.CreditName = true;
                                        break;
                                    case "Currency":
                                        GlobalVariables.Currency = true;
                                        break;
                                    case "Hostage":
                                        GlobalVariables.Hostage = true;
                                        break;
                                    case "Banks":
                                        GlobalVariables.Banks = true;
                                        break;
                                    case "BankOperation":
                                        GlobalVariables.BankOperation = true;
                                        break;
                                    case "Countries":
                                        GlobalVariables.Countries = true;
                                        break;
                                    case "CashOperations":
                                        GlobalVariables.CashOperations = true;
                                        break;
                                    case "Personnel":
                                        GlobalVariables.Personnel = true;
                                        break;
                                    case "AttractedFunds":
                                        GlobalVariables.AttractedFunds = true;
                                        break;
                                    case "Founders":
                                        GlobalVariables.Founders = true;
                                        break;
                                    case "Positions":
                                        GlobalVariables.Positions = true;
                                        break;
                                    case "Insurance":
                                        GlobalVariables.Insurance = true;
                                        break;
                                }
                            }
                            break;
                        case 9:
                            {
                                GlobalVariables.Funds = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddFundContract":
                                        GlobalVariables.AddFundContract = true;
                                        break;
                                    case "EditFundContract":
                                        GlobalVariables.EditFundContract = true;
                                        break;
                                    case "DeleteFundContract":
                                        GlobalVariables.DeleteFundContract = true;
                                        break;
                                    case "AddBuyAmount":
                                        GlobalVariables.AddBuyAmount = true;
                                        break;
                                    case "EditBuyAmount":
                                        GlobalVariables.EditBuyAmount = true;
                                        break;
                                    case "AddFundPayment":
                                        GlobalVariables.AddFundPayment = true;
                                        break;
                                    case "EditFundPayment":
                                        GlobalVariables.EditFundPayment = true;
                                        break;
                                    case "DeleteFund":
                                        GlobalVariables.DeleteFund = true;
                                        break;
                                    case "OpenFundContract":
                                        GlobalVariables.OpenFundContract = true;
                                        break;
                                    case "CloseFundContract":
                                        GlobalVariables.CloseFundContract = true;
                                        break;
                                    case "AddFundPercent":
                                        GlobalVariables.AddFundPercent = true;
                                        break;
                                    case "EditFundPercent":
                                        GlobalVariables.EditFundPercent = true;
                                        break;
                                    case "DeleteFundPercent":
                                        GlobalVariables.DeleteFundPercent = true;
                                        break;
                                }
                                if (GlobalVariables.AddFundContract || GlobalVariables.EditFundContract || GlobalVariables.DeleteFundContract)
                                    GlobalVariables.FundContract = true;
                                if (GlobalVariables.AddBuyAmount || GlobalVariables.EditBuyAmount || GlobalVariables.AddFundPayment || GlobalVariables.EditFundPayment || GlobalVariables.DeleteFund)
                                    GlobalVariables.FundPayment = true;
                            }
                            break;
                        case 10:
                            {
                                GlobalVariables.Bookkeeping = true;
                                switch (dr[1].ToString())
                                {
                                    case "ShowJournal":
                                        GlobalVariables.ShowJournal = true;
                                        break;
                                    case "AddJournal":
                                        GlobalVariables.AddJournal = true;
                                        break;
                                    case "EditJournal":
                                        GlobalVariables.EditJournal = true;
                                        break;
                                    case "DeleteJournal":
                                        GlobalVariables.DeleteJournal = true;
                                        break;
                                    case "CopyJournal":
                                        GlobalVariables.CopyJournal = true;
                                        break;
                                    case "ShowDetails":
                                        GlobalVariables.ShowDetails = true;
                                        break;
                                    case "ShowAccountPlan":
                                        GlobalVariables.ShowAccountPlan = true;
                                        break;
                                    case "AddAccountPlan":
                                        GlobalVariables.AddAccountPlan = true;
                                        break;
                                    case "EditAccountPlan":
                                        GlobalVariables.EditAccountPlan = true;
                                        break;
                                    case "DeleteAccountPlan":
                                        GlobalVariables.DeleteAccountPlan = true;
                                        break;
                                    case "ShowDebt":
                                        GlobalVariables.ShowDebt = true;
                                        break;
                                    case "AddDebt":
                                        GlobalVariables.AddDebt = true;
                                        break;
                                    case "EditDebt":
                                        GlobalVariables.EditDebt = true;
                                        break;
                                    case "DeleteDebt":
                                        GlobalVariables.DeleteDebt = true;
                                        break;
                                    case "ShowProfits":
                                        GlobalVariables.ShowProfits = true;
                                        break;
                                    case "AgainCalculated":
                                        GlobalVariables.AgainCalculated = true;
                                        break;
                                    case "OperationHistory":
                                        GlobalVariables.OperationHistory = true;
                                        break;
                                }
                            }
                            break;
                        case 11:
                            {
                                GlobalVariables.PaymentTask = true;
                                switch (dr[1].ToString())
                                {
                                    case "AddTask":
                                        GlobalVariables.AddTask = true;
                                        break;
                                    case "EditTask":
                                        GlobalVariables.EditTask = true;
                                        break;
                                    case "DeleteTask":
                                        GlobalVariables.DeleteTask = true;
                                        break;
                                    case "AddTemplate":
                                        GlobalVariables.AddTemplate = true;
                                        break;
                                    case "EditTemplate":
                                        GlobalVariables.EditTemplate = true;
                                        break;
                                    case "DeleteTemplate":
                                        GlobalVariables.DeleteTemplate = true;
                                        break;
                                    case "AddTaskType":
                                        GlobalVariables.AddTaskType = true;
                                        break;
                                    case "EditTaskType":
                                        GlobalVariables.EditTaskType = true;
                                        break;
                                    case "DeleteTaskType":
                                        GlobalVariables.DeleteTaskType = true;
                                        break;
                                    case "AddAcceptor":
                                        GlobalVariables.AddAcceptor = true;
                                        break;
                                    case "EditAcceptor":
                                        GlobalVariables.EditAcceptor = true;
                                        break;
                                    case "DeleteAcceptor":
                                        GlobalVariables.DeleteAcceptor = true;
                                        break;
                                    case "EditVatBank":
                                        GlobalVariables.EditVatBank = true;
                                        break;
                                }
                            }
                            break;
                        case 12:
                            {                                
                                switch (dr[1].ToString())
                                {
                                    case "EditTemplateFile":
                                        GlobalVariables.EditTemplateFile = true;
                                        break;                                    
                                }
                            }
                            break;
                        case 13:
                            {
                                switch (dr[1].ToString())
                                {
                                    case "Sms":
                                        GlobalVariables.Sms = true;
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İstifadəçinin hüquqları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, MethodBase.GetCurrentMethod().Name, exx);

            }
        }        

        private void FConnect_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " (versiya " + GlobalVariables.V_Version + ")";
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("London Liquid Sky");
            saved = Convert.ToInt32(GlobalFunctions.ReadSetting("SaveLogin"));
            GlobalVariables.V_DefaultMenu = int.Parse(GlobalFunctions.ReadSetting("DefaultMenu"));
            GlobalVariables.V_DefaultLanguage = int.Parse(GlobalFunctions.ReadSetting("DefaultLanguage"));
            GlobalVariables.V_DefaultDateSort = int.Parse(GlobalFunctions.ReadSetting("DefaultDateSort"));
            SaveCheck.Visible = !(saved == 1);

            if (saved == 1)
            {
                GlobalVariables.V_Connect_User = GlobalFunctions.ReadSetting("SavedLoginName");
                UserNameText.Text = GlobalVariables.V_Connect_User;
                PasswordText.Text = GlobalFunctions.Decrypt(GlobalFunctions.ReadSetting("SavedLoginPassword"));
                SaveCheck.Checked = true;
            }
            FormStatus = true;
                        
            GlobalProcedures.ExecuteProcedure("CRS_USER.PROC_ALTER_SESSION", "İstifadəçilərin bazaya qoşulması dəyişdirilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AzCheck_Click(object sender, EventArgs e)
        {
            AzCheck.Checked = true;
            EnCheck.Checked = false;
            RuCheck.Checked = false;
        }

        private void EnCheck_Click(object sender, EventArgs e)
        {
            AzCheck.Checked = false;
            EnCheck.Checked = true;
            RuCheck.Checked = false;
        }

        private void RuCheck_Click(object sender, EventArgs e)
        {
            AzCheck.Checked = false;
            EnCheck.Checked = false;
            RuCheck.Checked = true;
        }

        private void UserNameText_TextChanged(object sender, EventArgs e)
        {
            SaveCheck.Visible = FormStatus;
            SaveCheck.Checked = false;
        }

        private void ForgetPasswordLabel_Click(object sender, EventArgs e)
        {
            Forms.FForgetPassword ffp = new Forms.FForgetPassword();
            ffp.ShowDialog();
        }
    }
}
