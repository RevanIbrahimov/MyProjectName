using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Diagnostics;
using DevExpress.Utils;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS.Forms.Cash
{
    public partial class FExpensesAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FExpensesAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, OperationOwnerID, OperationID;
        public int Index;

        int bank_id, ContractID, CreditNameID, currency_id, other_card_series_id, service_card_series_id,
            other_card_issuing_id, service_card_issuing_id, FounderID, OtherID, ServiceID, founder_id, founder_card_id, fund_contract_id, old_founder_id, old_fund_contract_id,
            other_appointment_id, personnel_id, personnel_card_id, SalaryID, responsible_id = 0, responsible_card_id;
        double hostage_amount, hostage_amount_azn;
        decimal currency_rate;
        bool FormStatus = false, close_form = true;
        string operationdate, IDImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\IDCardImages",
            PersonnelImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images",
            funds_payment_id;

        ReportViewer rv_expenditure = new ReportViewer();

        public delegate void DoEvent(string a);
        public event DoEvent RefreshCashDataGridView;

        private void FExpensesAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                PersonnelComboBox.Properties.Buttons[1].Visible = GlobalVariables.Personnel;
                FounderComboBox.Properties.Buttons[1].Visible = GlobalVariables.Founders;
                BankLookUp.Properties.Buttons[1].Visible = GlobalVariables.Banks;
                OtherAppointmentComboBox.Properties.Buttons[1].Visible = GlobalVariables.CashOperations;
                OtherSeriesComboBox.Properties.Buttons[1].Visible = GlobalVariables.CardSeries;
                ServiceSeriesComboBox.Properties.Buttons[1].Visible = GlobalVariables.CardSeries;
                OtherIssuingComboBox.Properties.Buttons[1].Visible = GlobalVariables.CardIssuing;
                ServiceIssuingComboBox.Properties.Buttons[1].Visible = GlobalVariables.CardIssuing;
            }

            this.ActiveControl = ContractCodeText;
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_OPERATIONS", GlobalVariables.V_UserID, "WHERE ID = " + OperationID + " AND USED_USER_ID = -1");
                LoadExpensesDetails();
            }
            FormStatus = true;
        }

        private void LoadExpensesDetails()
        {
            int UsedUserID = -1;

            switch (Index)
            {
                case 7:
                    {
                        ExpensesBackstageViewControl.SelectedTabIndex = 0;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID IN (SELECT CONTRACT_ID FROM CRS_USER.CASH_EXPENSES_PAYMENT WHERE ID = " + OperationOwnerID + ") AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CONTRACTS WHERE ID IN (SELECT CONTRACT_ID FROM CRS_USER.CASH_EXPENSES_PAYMENT WHERE ID = " + OperationOwnerID + ")");
                        if (UsedUserID >= 0)
                        {

                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz lizinq müqaviləsi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş linq müqaviləsinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                PaymentTab.Enabled = BOK.Visible = false;
                            }
                            else
                            {
                                PaymentTab.Enabled = BOK.Visible = true;
                            }
                        }
                        else
                        {
                            PaymentTab.Enabled = BOK.Visible = true;
                        }
                        LoadPaymentDetails();
                        PaymentDate.Enabled =
                            FounderTab.Enabled =
                            AccountTab.Enabled =
                            OtherTab.Enabled =
                            ServiceTab.Enabled =
                            SalaryTab.Enabled = false;
                    }
                    break;
                case 8:
                    {
                        ExpensesBackstageViewControl.SelectedTabIndex = 2;
                        FounderID = int.Parse(OperationOwnerID);
                        LoadFounderDetails();
                        LoadFundContractsGridView(founder_id);
                        ContractGridControl.Enabled = false;
                        ContractsBarButton.Enabled = false;
                        FounderTab.Enabled = false;
                        BOK.Visible = false;
                        AccountTab.Enabled = false;
                        OtherTab.Enabled = false;
                        ServiceTab.Enabled = false;
                        PaymentTab.Enabled = false;
                        SalaryTab.Enabled = false;
                    }
                    break;
                case 9:
                    {
                        ExpensesBackstageViewControl.SelectedTabIndex = 4;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_BANK_ACCOUNT", GlobalVariables.V_UserID, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_BANK_ACCOUNT WHERE ID = " + OperationOwnerID);
                        if (UsedUserID >= 0)
                        {
                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz Bank hesabı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş bank hesabının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                AccountTab.Enabled = BOK.Visible = false;
                            }
                            else
                                AccountTab.Enabled = BOK.Visible = true;

                        }
                        else
                            AccountTab.Enabled = BOK.Visible = true;

                        LoadBankAccountDetails();
                        BankLookUp.Enabled =
                            PaymentTab.Enabled =
                            FounderTab.Enabled =
                            OtherTab.Enabled =
                            ServiceTab.Enabled =
                            SalaryTab.Enabled = false;
                    }
                    break;
                case 10:
                    {
                        ExpensesBackstageViewControl.SelectedTabIndex = 6;
                        OtherID = int.Parse(OperationOwnerID);
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_EXPENSES_OTHER_PAYMENT", GlobalVariables.V_UserID, "WHERE ID = " + OtherID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_EXPENSES_OTHER_PAYMENT WHERE ID = " + OtherID);
                        if (UsedUserID >= 0)
                        {
                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz Bank hesabı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş bank hesabının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                OtherTab.Enabled = BOK.Visible = false;
                            }
                            else
                                OtherTab.Enabled = BOK.Visible = true;
                        }
                        else
                            OtherTab.Enabled = BOK.Visible = true;

                        LoadOtherDetails();
                        PaymentTab.Enabled =
                            FounderTab.Enabled =
                            ServiceTab.Enabled =
                            AccountTab.Enabled =
                            SalaryTab.Enabled = false;
                    }
                    break;
                case 11:
                    {
                        ExpensesBackstageViewControl.SelectedTabIndex = 8;
                        ServiceID = int.Parse(OperationOwnerID);
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_EXPENSES_SERVICE_PRICE", GlobalVariables.V_UserID, "WHERE ID = " + ServiceID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_EXPENSES_SERVICE_PRICE WHERE ID = " + ServiceID);
                        if (UsedUserID >= 0)
                        {
                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz Bank hesabı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş bank hesabının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                ServiceTab.Enabled = BOK.Visible = false;
                            }
                            else
                                ServiceTab.Enabled = BOK.Visible = true;
                        }
                        else
                            ServiceTab.Enabled = BOK.Visible = true;

                        LoadServiceDetails();
                        PaymentTab.Enabled =
                            FounderTab.Enabled =
                            OtherTab.Enabled =
                            AccountTab.Enabled = false;
                    }
                    break;
                case 12:
                    {
                        ExpensesBackstageViewControl.SelectedTabIndex = 10;
                        GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CASH_SALARY", GlobalVariables.V_UserID, "WHERE ID = " + OperationOwnerID + " AND USED_USER_ID = -1");
                        UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.CASH_SALARY WHERE ID = " + OperationOwnerID);
                        if (UsedUserID >= 0)
                        {
                            if (GlobalVariables.V_UserID != UsedUserID)
                            {
                                string used_user_name = Class.GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                                XtraMessageBox.Show("Seçdiyiniz əmək haqqı hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş əmək haqqının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                SalaryTab.Enabled = BOK.Visible = false;
                            }
                            else
                                SalaryTab.Enabled = BOK.Visible = true;
                        }
                        else
                            SalaryTab.Enabled = BOK.Visible = true;

                        SalaryID = int.Parse(OperationOwnerID);
                        LoadSalaryDetails();
                        AccountTab.Enabled =
                            OtherTab.Enabled =
                            ServiceTab.Enabled =
                            PaymentTab.Enabled =
                            FounderTab.Enabled = false;
                    }
                    break;
            }
        }

        private void LoadPaymentDetails()
        {
            string //s = "SELECT CP.CONTRACT_CODE,CP.PAYMENT_DATE P_DATE,S.SURNAME||' '||S.NAME||' '||S.PATRONYMIC SELLERNAME,CS.NAME||':'||CS.SERIES||', №: '||S.CARD_NUMBER||', '||TO_CHAR(S.CARD_ISSUE_DATE,'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' SELLER_CARD,CP.AMOUNT,CP.AMOUNT_AZN,C.ID,C.CODE,CP.CURRENCY_RATE,CP.NOTE,CP.CONTRACT_ID,CP.SELLER_ID,CP.PERSONNEL_ID,P.SURNAME||' '||P.NAME||' '||P.PATRONYMIC PERSONNELFULLNAME,(SELECT CS.NAME||' : '||CS.SERIES||', №:'||FC.CARD_NUMBER||', '||TO_CHAR(FC.ISSUE_DATE,'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' CARD FROM CRS_USER.PERSONNEL_CARDS FC,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE FC.CARD_SERIES_ID = CS.ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.ID = CP.PERSONNEL_CARD_ID) PERSONNEL_CARD,CP.CREDIT_NAME_ID,CP.PERSONNEL_CARD_ID FROM CRS_USER.CASH_EXPENSES_PAYMENT CP,CRS_USER.SELLERS S,CRS_USER.CURRENCY C,CRS_USER.CARD_ISSUING CI,CRS_USER.CARD_SERIES CS,CRS_USER.PERSONNEL P WHERE CP.SELLER_ID = S.ID AND CP.CURRENCY_ID = C.ID AND S.CARD_SERIES_ID = CS.ID AND S.CARD_ISSUING_ID = CI.ID AND CP.PERSONNEL_ID = P.ID(+) AND CP.ID = " + OperationOwnerID;

            s = $@"SELECT CON.CONTRACT_CODE,
                           CP.PAYMENT_DATE,
                           S.FULLNAME SELLERNAME,
                           SC.SELLER_CARD,
                           CP.AMOUNT,
                           CP.AMOUNT_AZN,
                           C.ID CURRENCY_ID,
                           C.CODE CURRENCY_CODE,
                           CP.CURRENCY_RATE,
                           CP.NOTE,
                           CP.CONTRACT_ID,
                           P.SURNAME || ' ' || P.NAME || ' ' || P.PATRONYMIC PERSONNELFULLNAME,
                           CP.CREDIT_NAME_ID,
                           CP.PERSONNEL_ID
                      FROM CRS_USER.CASH_EXPENSES_PAYMENT CP,
                           CRS_USER.V_CONTRACTS CON,
                           CRS_USER.V_SELLERS S,
                           CRS_USER.V_SELLER_CARDS SC,
                           CRS_USER.CURRENCY C,
                           CRS_USER.PERSONNEL P
                     WHERE     CP.CONTRACT_ID = CON.CONTRACT_ID
                           AND CON.SELLER_ID = S.ID
                           AND CON.SELLER_TYPE_ID = S.PERSON_TYPE_ID
                           AND S.ID = SC.SELLER_ID
                           AND CP.CURRENCY_ID = C.ID
                           AND CP.PERSONNEL_ID = P.ID(+)
                           AND CP.ID = {OperationOwnerID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentDetails").Rows)
                {
                    ContractCodeText.Enabled = false;
                    ContractCodeText.Text = dr["CONTRACT_CODE"].ToString();
                    PaymentDate.EditValue = DateTime.Parse(dr["PAYMENT_DATE"].ToString());
                    SellerNameText.Text = dr["SELLERNAME"].ToString();
                    SellerCardText.Text = dr["SELLER_CARD"].ToString();
                    hostage_amount = Convert.ToDouble(dr["AMOUNT"]);
                    PaymentAmountText.Text = hostage_amount.ToString("N2");
                    CurrencyLabel.Text = dr["CURRENCY_CODE"].ToString();
                    currency_id = Convert.ToInt32(dr["CURRENCY_ID"]);
                    if (currency_id != 1)
                    {
                        PaymentAZNAmountText.Visible =
                            AZNLabel.Visible =
                            CurrencyRateLabel.Visible =
                            CurrencyRateValue.Visible = true;

                        PaymentAZNAmountText.BackColor = Color.PaleGreen;
                        PaymentAmountText.BackColor = GlobalFunctions.ElementColor();
                        CurrencyRateLabel.Text = "1 " + CurrencyLabel.Text + " = ";
                        CurrencyRateValue.Value = Convert.ToDecimal(dr["CURRENCY_RATE"]);
                        hostage_amount_azn = Convert.ToDouble(dr["AMOUNT_AZN"]);
                        PaymentAZNAmountText.Text = hostage_amount_azn.ToString("N2");
                    }
                    else
                    {
                        PaymentAZNAmountText.Visible =
                            AZNLabel.Visible =
                            CurrencyRateLabel.Visible =
                            CurrencyRateValue.Visible = false;

                        PaymentAmountText.BackColor = Color.PaleGreen;
                        PaymentAZNAmountText.BackColor = GlobalFunctions.ElementColor();
                    }
                    PaymentNoteText.Text = dr["NOTE"].ToString();

                    ContractID = Convert.ToInt32(dr["CONTRACT_ID"]);

                    responsible_id = Convert.ToInt32(dr["PERSONNEL_ID"]);

                    if (responsible_id > 0)
                    {
                        ResponsibleCheck.Checked = true;
                        ResponsibleComboBox.EditValue = dr["PERSONNELFULLNAME"].ToString();
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadFounderDetails()
        {
            string s = $@"SELECT F.FULLNAME,
                           CS.SERIES,
                           FC.CARD_NUMBER,
                           TO_CHAR (FC.ISSUE_DATE, 'DD.MM.YYYY'),
                           TO_CHAR (FC.RELIABLE_DATE, 'DD.MM.YYYY'),
                           CI.NAME,
                           FC.REGISTRATION_ADDRESS,
                           FC.ADDRESS,
                           CF.PAYMENT_DATE,
                           CF.APPOINTMENT,
                           CF.AMOUNT,
                           CF.NOTE,
                           FC.ID CARD_ID,
                           FC.FRONT_FACE_IMAGE,
                           FC.FRONT_FACE_IMAGE_FORMAT,
                           FC.REAR_FACE_IMAGE,
                           FC.REAR_FACE_IMAGE_FORMAT
                      FROM CRS_USER.CASH_FOUNDER CF,
                           CRS_USER.FOUNDERS F,
                           CRS_USER.FOUNDER_CARDS FC,
                           CRS_USER.CARD_SERIES CS,
                           CRS_USER.CARD_ISSUING CI
                     WHERE     CF.FOUNDER_ID = F.ID
                           AND CF.FOUNDER_CARD_ID = FC.ID
                           AND FC.FOUNDER_ID = F.ID
                           AND FC.CARD_SERIES_ID = CS.ID
                           AND FC.CARD_ISSUING_ID = CI.ID
                           AND CF.INC_EXP = 2
                           AND CF.ID = {FounderID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFounderDetails").Rows)
                {
                    FounderComboBox.EditValue = dr[0].ToString();
                    FounderSeriestText.Text = dr[1].ToString();
                    FounderNumberText.Text = dr[2].ToString();
                    FounderIssueDateText.Text = dr[3].ToString();
                    FounderReliableDateText.Text = dr[4].ToString();
                    FounderIssuingText.Text = dr[5].ToString();
                    FounderRegistrationAddressText.Text = dr[6].ToString();
                    FounderAddressText.Text = dr[7].ToString();
                    FounderDate.EditValue = DateTime.Parse(dr[8].ToString());
                    FounderAppointmentText.Text = dr[9].ToString();
                    FounderAmountValue.Value = Convert.ToDecimal(dr[10].ToString());
                    FounderNoteText.Text = dr[11].ToString();

                    founder_card_id = Convert.ToInt32(dr[12].ToString());

                    if (!DBNull.Value.Equals(dr["FRONT_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["FRONT_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Front_" + FounderComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"]);
                        FileStream front_fs = new FileStream(IDImagePath + "\\Expenses_Front_" + FounderComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(front_fs);
                        front_fs.Close();
                        stmBLOBData.Close();
                        FounderFrontFaceButtonEdit.Text = IDImagePath + "\\Expenses_Front_" + FounderComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"];
                    }

                    if (!DBNull.Value.Equals(dr["REAR_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["REAR_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Rear_" + FounderComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"]);
                        FileStream rear_fs = new FileStream(IDImagePath + "\\Expenses_Rear_" + FounderComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(rear_fs);
                        rear_fs.Close();
                        stmBLOBData.Close();
                        FounderRearFaceButtonEdit.Text = IDImagePath + "\\Expenses_Rear_" + FounderComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"];
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçi ilə hesablaşmanın məxaricinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadBankAccountDetails()
        {
            string s = $@"SELECT BA.ADATE,
                                   B.LONG_NAME BANK_NAME,       
                                   BA.APPOINTMENT,
                                   BA.AMOUNT,
                                   BA.NOTE
                              FROM CRS_USER.CASH_BANK_ACCOUNT BA,
                                   CRS_USER.BANKS B
                             WHERE     BA.INC_EXP = 2       
                                   AND BA.BANK_ID = B.ID      
                                   AND BA.ID = {OperationOwnerID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadBankAccountDetails").Rows)
                {
                    AccountDate.EditValue = DateTime.Parse(dr["ADATE"].ToString());
                    BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                    AccountAppointmentText.Text = dr["APPOINTMENT"].ToString();
                    AccountAmountValue.Value = Convert.ToDecimal(dr["AMOUNT"].ToString());
                    AccountNoteText.Text = dr["NOTE"].ToString();
                }

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bank hesabının məxaricinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadOtherDetails()
        {
            string s = $@"SELECT CF.PAYMENT_DATE,
                                   CF.FULLNAME,
                                   CA.NAME APPOINTMENT,
                                   CF.AMOUNT,
                                   CS.SERIES,
                                   CF.CARD_NUMBER,
                                   CF.CARD_ISSUE_DATE,
                                   CF.CARD_RELIABLE_DATE,
                                   CI.NAME,
                                   CF.ADRESS,
                                   CF.REGISTRATION_ADDRESS,
                                   CF.NOTE,
                                   CF.CARD_FRONT_FACE_IMAGE,
                                   CF.CARD_FRONT_FACE_IMAGE_FORMAT,
                                   CF.CARD_REAR_FACE_IMAGE,
                                   CF.CARD_REAR_FACE_IMAGE_FORMAT
                              FROM CRS_USER.CASH_EXPENSES_OTHER_PAYMENT CF,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.CARD_ISSUING CI,
                                   CRS_USER.CASH_APPOINTMENTS CA
                             WHERE     CF.CASH_APPOINTMENT_ID = CA.ID
                                   AND CA.TYPE = 1
                                   AND CF.CARD_SERIES_ID = CS.ID
                                   AND CF.CARD_ISSUING_ID = CI.ID
                                   AND CF.ID = {OtherID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOtherDetails").Rows)
                {
                    OtherDate.EditValue = DateTime.Parse(dr[0].ToString());
                    OtherFullNameText.Text = dr[1].ToString();
                    OtherAppointmentComboBox.EditValue = dr[2].ToString();
                    OtherAmountValue.Value = Convert.ToDecimal(dr[3].ToString());
                    OtherSeriesComboBox.EditValue = dr[4].ToString();
                    OtherNumberText.Text = dr[5].ToString();
                    OtherIssueDate.EditValue = DateTime.Parse(dr[6].ToString());
                    OtherReliableDate.EditValue = DateTime.Parse(dr[7].ToString());
                    OtherIssuingComboBox.EditValue = dr[8].ToString();
                    OtherAddressText.Text = dr[9].ToString();
                    OtherRegistrationAddressText.Text = dr[10].ToString();
                    OtherNoteText.Text = dr[11].ToString();

                    if (!DBNull.Value.Equals(dr["CARD_FRONT_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["CARD_FRONT_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Front_" + OtherFullNameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"]);
                        FileStream front_fs = new FileStream(IDImagePath + "\\Expenses_Front_" + OtherFullNameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(front_fs);
                        front_fs.Close();
                        stmBLOBData.Close();
                        OtherFrontFaceButtonEdit.Text = IDImagePath + "\\Expenses_Front_" + OtherFullNameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"];
                    }

                    if (!DBNull.Value.Equals(dr["CARD_REAR_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["CARD_REAR_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Rear_" + OtherFullNameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"]);
                        FileStream rear_fs = new FileStream(IDImagePath + "\\Expenses_Rear_" + OtherFullNameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(rear_fs);
                        rear_fs.Close();
                        stmBLOBData.Close();
                        OtherRearFaceButtonEdit.Text = IDImagePath + "\\Expenses_Rear_" + OtherFullNameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"];
                    }
                }

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Digər ödənişlərin məxaricinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadServiceDetails()
        {
            string s = $@"SELECT CF.PAYMENT_DATE,
                                   CF.FULLNAME,
                                   CF.APPOINTMENT,
                                   CF.AMOUNT,
                                   CS.SERIES,
                                   CF.CARD_NUMBER,
                                   CF.CARD_ISSUE_DATE,
                                   CF.CARD_RELIABLE_DATE,
                                   CI.NAME,
                                   CF.ADRESS,
                                   CF.REGISTRATION_ADDRESS,
                                   CF.NOTE,
                                   CF.CARD_FRONT_FACE_IMAGE,
                                   CF.CARD_FRONT_FACE_IMAGE_FORMAT,
                                   CF.CARD_REAR_FACE_IMAGE,
                                   CF.CARD_REAR_FACE_IMAGE_FORMAT
                              FROM CRS_USER.CASH_EXPENSES_SERVICE_PRICE CF,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.CARD_ISSUING CI
                             WHERE CF.CARD_SERIES_ID = CS.ID AND CF.CARD_ISSUING_ID = CI.ID AND CF.ID = {ServiceID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadServiceDetails").Rows)
                {
                    ServiceDate.EditValue = DateTime.Parse(dr[0].ToString());
                    ServiceFullNameText.Text = dr[1].ToString();
                    ServiceAppointmentText.Text = dr[2].ToString();
                    ServiceAmountValue.Value = Convert.ToDecimal(dr[3].ToString());
                    ServiceSeriesComboBox.EditValue = dr[4].ToString();
                    ServiceNumberText.Text = dr[5].ToString();
                    ServiceIssueDate.EditValue = DateTime.Parse(dr[6].ToString());
                    ServiceReliableDate.EditValue = DateTime.Parse(dr[7].ToString());
                    ServiceIssuingComboBox.EditValue = dr[8].ToString();
                    ServiceAddressText.Text = dr[9].ToString();
                    ServiceRegistrationAddressText.Text = dr[10].ToString();
                    ServiceNoteText.Text = dr[11].ToString();

                    if (!DBNull.Value.Equals(dr["CARD_FRONT_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["CARD_FRONT_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Front_" + ServiceFullNameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"]);
                        FileStream front_fs = new FileStream(IDImagePath + "\\Expenses_Front_" + ServiceFullNameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(front_fs);
                        front_fs.Close();
                        stmBLOBData.Close();
                        ServiceFrontFaceButtonEdit.Text = IDImagePath + "\\Expenses_Front_" + ServiceFullNameText.Text + dr["CARD_FRONT_FACE_IMAGE_FORMAT"];
                    }

                    if (!DBNull.Value.Equals(dr["CARD_REAR_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["CARD_REAR_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Rear_" + ServiceFullNameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"]);
                        FileStream rear_fs = new FileStream(IDImagePath + "\\Expenses_Rear_" + ServiceFullNameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(rear_fs);
                        rear_fs.Close();
                        stmBLOBData.Close();
                        ServiceRearFaceButtonEdit.Text = IDImagePath + "\\Expenses_Rear_" + ServiceFullNameText.Text + dr["CARD_REAR_FACE_IMAGE_FORMAT"];
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Xidmət haqqının məxaricinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadSalaryDetails()
        {
            string s = $@"SELECT F.SURNAME || ' ' || F.NAME || ' ' || F.PATRONYMIC FULLNAME,
                                       CS.SERIES,
                                       FC.CARD_NUMBER,
                                       TO_CHAR(FC.ISSUE_DATE,'DD.MM.YYYY'),
                                       TO_CHAR(FC.RELIABLE_DATE,'DD.MM.YYYY'),
                                       CI.NAME,
                                       FC.REGISTRATION_ADDRESS,
                                       FC.ADDRESS,
                                       CF.PAYMENT_DATE,
                                       CF.APPOINTMENT,
                                       CF.AMOUNT,
                                       CF.NOTE,
                                       FC.ID CARD_ID,
                                       FC.FRONT_FACE_IMAGE,
                                       FC.FRONT_FACE_IMAGE_FORMAT,
                                       FC.REAR_FACE_IMAGE,
                                       FC.REAR_FACE_IMAGE_FORMAT
                                  FROM CRS_USER.CASH_SALARY CF,
                                       CRS_USER.PERSONNEL F,
                                       CRS_USER.PERSONNEL_CARDS FC,
                                       CRS_USER.CARD_SERIES CS,
                                       CRS_USER.CARD_ISSUING CI
                                 WHERE     CF.PERSONNEL_ID = F.ID
                                       AND CF.PERSONNEL_CARD_ID = FC.ID
                                       AND FC.PERSONNEL_ID = F.ID
                                       AND FC.CARD_SERIES_ID = CS.ID
                                       AND FC.CARD_ISSUING_ID = CI.ID
                                       AND CF.ID = {SalaryID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadSalaryDetails").Rows)
                {
                    PersonnelComboBox.EditValue = dr[0].ToString();
                    SalarySeriesText.Text = dr[1].ToString();
                    SalaryNumberText.Text = dr[2].ToString();
                    SalaryIssueDateText.Text = dr[3].ToString();
                    SalaryReliableDateText.Text = dr[4].ToString();
                    SalaryIssuingText.Text = dr[5].ToString();
                    SalaryRegistrationAddressText.Text = dr[6].ToString();
                    SalaryAddressText.Text = dr[7].ToString();
                    SalaryDate.EditValue = DateTime.Parse(dr[8].ToString());
                    SalaryAppointmentText.Text = dr[9].ToString();
                    SalaryAmountValue.Value = Convert.ToDecimal(dr[10].ToString());
                    SalaryNoteText.Text = dr[11].ToString();
                    personnel_card_id = Convert.ToInt32(dr[12].ToString());

                    if (!DBNull.Value.Equals(dr["FRONT_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["FRONT_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Front_" + PersonnelComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"]);
                        FileStream front_fs = new FileStream(IDImagePath + "\\Expenses_Front_" + PersonnelComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(front_fs);
                        front_fs.Close();
                        stmBLOBData.Close();
                        SalaryFrontFaceButtonEdit.Text = IDImagePath + "\\Expenses_Front_" + PersonnelComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"];
                    }

                    if (!DBNull.Value.Equals(dr["REAR_FACE_IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["REAR_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        if (!Directory.Exists(IDImagePath))
                        {
                            Directory.CreateDirectory(IDImagePath);
                        }
                        GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Rear_" + PersonnelComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"]);
                        FileStream rear_fs = new FileStream(IDImagePath + "\\Expenses_Rear_" + PersonnelComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(rear_fs);
                        rear_fs.Close();
                        stmBLOBData.Close();
                        SalaryRearFaceButtonEdit.Text = IDImagePath + "\\Expenses_Rear_" + PersonnelComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"];
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İşçinin əmək haqqısının rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void ExpensesBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            switch (ExpensesBackstageViewControl.SelectedTabIndex)
            {
                case 0:
                    {
                        ContractCodeText.Focus();
                        PaymentDate.EditValue = DateTime.Today;
                        GlobalProcedures.FillComboBoxEditWithSqlText(ResponsibleComboBox, "SELECT SURNAME||' '||NAME||' '||PATRONYMIC,SURNAME||' '||NAME||' '||PATRONYMIC,SURNAME||' '||NAME||' '||PATRONYMIC FROM CRS_USER.PERSONNEL ORDER BY SURNAME");
                    }
                    break;
                case 2:
                    {
                        GlobalProcedures.FillComboBoxEditWithSqlText(FounderComboBox, "SELECT FULLNAME,FULLNAME,FULLNAME FROM CRS_USER.FOUNDERS ORDER BY ORDER_ID");
                        FounderDate.Focus();
                        FounderDate.EditValue = DateTime.Today;
                    }
                    break;
                case 4:
                    {
                        AccountDate.Focus();
                        AccountDate.EditValue = DateTime.Today;
                        GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY 1");
                    }
                    break;
                case 6:
                    {
                        OtherDate.Focus();
                        OtherDate.EditValue = DateTime.Today;
                        OtherIssueDate.EditValue = DateTime.Today;
                        OtherIssueDate.Properties.MaxValue = DateTime.Today;
                        OtherReliableDate.EditValue = DateTime.Today;
                        GlobalProcedures.FillComboBoxEditWithSqlText(OtherAppointmentComboBox, "SELECT NAME,NAME_EN,NAME_RU FROM CRS_USER.CASH_APPOINTMENTS ORDER BY ORDER_ID");
                        GlobalProcedures.FillComboBoxEdit(OtherSeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
                        GlobalProcedures.FillComboBoxEdit(OtherIssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                        if (TransactionName == "INSERT")
                            OtherID = GlobalFunctions.GetOracleSequenceValue("CASH_EXPENSES_OTHER_SEQUENCE");
                    }
                    break;
                case 8:
                    {
                        ServiceDate.Focus();
                        ServiceDate.EditValue = DateTime.Today;
                        ServiceIssueDate.EditValue = DateTime.Today;
                        ServiceIssueDate.Properties.MaxValue = DateTime.Today;
                        ServiceReliableDate.EditValue = DateTime.Today;
                        GlobalProcedures.FillComboBoxEdit(ServiceSeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
                        GlobalProcedures.FillComboBoxEdit(ServiceIssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                        if (TransactionName == "INSERT")
                            ServiceID = GlobalFunctions.GetOracleSequenceValue("CASH_EXPENSES_SERVICE_SEQUENCE");
                    }
                    break;
                case 10:
                    {
                        GlobalProcedures.FillComboBoxEditWithSqlText(PersonnelComboBox, "SELECT SURNAME||' '||NAME||' '||PATRONYMIC,SURNAME||' '||NAME||' '||PATRONYMIC,SURNAME||' '||NAME||' '||PATRONYMIC FROM CRS_USER.PERSONNEL ORDER BY SURNAME");
                        SalaryDate.Focus();
                        SalaryDate.EditValue = DateTime.Today;
                    }
                    break;
            }
        }

        private void PaymentNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (PaymentNoteText.Text.Length <= 400)
                PaymentDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - PaymentNoteText.Text.Length).ToString();
        }

        private void FounderNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (FounderNoteText.Text.Length <= 400)
                FounderDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - FounderNoteText.Text.Length).ToString();
        }

        private void AccountNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (AccountNoteText.Text.Length <= 400)
                AccountDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - AccountNoteText.Text.Length).ToString();
        }

        private void OtherNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (OtherNoteText.Text.Length <= 400)
                OtherDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - OtherNoteText.Text.Length).ToString();
        }

        private void ServiceNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (ServiceNoteText.Text.Length <= 400)
                ServiceDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - ServiceNoteText.Text.Length).ToString();
        }

        private void OtherNumberText_EditValueChanged(object sender, EventArgs e)
        {
            if (OtherNumberText.Text.Trim().Length == 0)
                OtherNumberLengthLabel.Visible = false;
            else
            {
                OtherNumberLengthLabel.Visible = true;
                OtherNumberLengthLabel.Text = OtherNumberText.Text.Trim().Length.ToString();
            }
        }

        private void ServiceNumberText_EditValueChanged(object sender, EventArgs e)
        {
            if (ServiceNumberText.Text.Trim().Length == 0)
                ServiceNumberLengthLabel.Visible = false;
            else
            {
                ServiceNumberLengthLabel.Visible = true;
                ServiceNumberLengthLabel.Text = ServiceNumberText.Text.Trim().Length.ToString();
            }
        }

        private void ContractCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                if (ContractCodeText.Text.Length < 4)
                    return;

                string sql = $@"SELECT CON.CONTRACT_ID,
                                       S.ID SELLER_ID,
                                       S.FULLNAME SELLER_NAME,
                                       SC.SELLER_CARD,
                                       CON.CREDIT_NAME_ID,
                                       TO_CHAR (CON.START_DATE, 'DD/MM/YYYY') START_DATE,
                                       CON.CUSTOMER_ID,
                                       CON.CURRENCY_RATE,
                                       H.LIQUID_AMOUNT,
                                       H.CURRENCY_CODE,
                                       H.CURRENCY_ID       
                                  FROM CRS_USER.V_CONTRACTS CON,
                                       CRS_USER.V_SELLERS S,
                                       CRS_USER.V_SELLER_CARDS SC,
                                       CRS_USER.V_HOSTAGE H
                                 WHERE     CON.USED_USER_ID = -1
                                       AND CON.SELLER_ID = S.ID
                                       AND CON.SELLER_TYPE_ID = S.PERSON_TYPE_ID
                                       AND S.ID = SC.SELLER_ID
                                       AND CON.IS_COMMIT = 1
                                       AND CON.LIQUID_TYPE = 0
                                       AND CON.CONTRACT_ID = H.CONTRACT_ID
                                       AND CON.CONTRACT_CODE = '{ContractCodeText.Text.Trim()}'";

                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/ContractCodeText_EditValueChanged");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ContractID = Convert.ToInt32(dr["CONTRACT_ID"]);
                        SellerNameText.Text = dr["SELLER_NAME"].ToString();
                        SellerCardText.Text = dr["SELLER_CARD"].ToString();
                        CreditNameID = Convert.ToInt32(dr["CREDIT_NAME_ID"]);
                        PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(dr["START_DATE"].ToString(), "ddmmyyyy");
                        currency_rate = Convert.ToDecimal(dr["CURRENCY_RATE"]);
                        hostage_amount = Convert.ToDouble(dr["LIQUID_AMOUNT"]);
                        CurrencyLabel.Text = dr["CURRENCY_CODE"].ToString();
                        currency_id = Convert.ToInt32(dr["CURRENCY_ID"]);
                    }

                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", GlobalVariables.V_UserID, "WHERE ID = " + ContractID + " AND USED_USER_ID = -1");

                    switch (CreditNameID)
                    {
                        case 1:
                            {
                                ResponsibleCheck.Checked =
                                ResponsibleCheck.Visible =
                                ResponsibleLabel.Visible =
                                ResponsibleComboBox.Visible =
                                ResponsibleCardText.Visible =
                                ResponsibleCardLabel.Visible =
                                labelControl55.Visible = false;
                            }
                            break;
                        case 5:
                            {
                                ResponsibleCheck.Checked = (GlobalFunctions.GetID($@"SELECT PERSONNEL_ID FROM CRS_USER.CASH_EXPENSES_PAYMENT WHERE CONTRACT_ID = {ContractID}") > 0);
                                ResponsibleCheck_CheckedChanged(sender, EventArgs.Empty);
                                ResponsibleComboBox_SelectedIndexChanged(sender, EventArgs.Empty);
                                ResponsibleCheck.Visible =
                                ResponsibleLabel.Visible =
                                ResponsibleComboBox.Visible =
                                ResponsibleCardText.Visible =
                                ResponsibleCardLabel.Visible =
                                labelControl55.Visible = true;
                            }
                            break;
                    }

                    PaymentAmountText.Text = hostage_amount.ToString("N2");
                    if (currency_id != 1)
                    {
                        PaymentAZNAmountText.Visible =
                        AZNLabel.Visible =
                        CurrencyRateLabel.Visible =
                        CurrencyRateValue.Visible = true;
                        PaymentAZNAmountText.BackColor = Color.PaleGreen;
                        PaymentAmountText.BackColor = GlobalFunctions.ElementColor();
                        CurrencyRateLabel.Text = "1 " + CurrencyLabel.Text + " = ";
                        CurrencyRateValue.Value = (decimal)GlobalFunctions.CurrencyLastRate(currency_id, PaymentDate.Text);
                    }
                    else
                    {
                        PaymentAZNAmountText.Visible =
                        AZNLabel.Visible =
                        CurrencyRateLabel.Visible =
                        CurrencyRateValue.Visible = false;
                        PaymentAmountText.BackColor = Color.PaleGreen;
                        PaymentAZNAmountText.BackColor = GlobalFunctions.ElementColor();
                    }
                }
                else
                {
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                    ContractID = 0;
                    SellerNameText.Text = SellerCardText.Text = PaymentAmountText.Text = null;
                    PaymentAmountText.BackColor = GlobalFunctions.ElementColor();
                    CreditNameID = 0;
                    PaymentAZNAmountText.Visible = AZNLabel.Visible = CurrencyRateLabel.Visible = CurrencyRateValue.Visible = false;
                    PaymentDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate("1.1.1990", "ddmmyyyy");
                }
            }
        }

        private void BCalculator_Click(object sender, EventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void BExchange_Click(object sender, EventArgs e)
        {
            switch (ExpensesBackstageViewControl.SelectedTabIndex)
            {
                case 0:
                    GlobalProcedures.ExchangeCalculator(PaymentDate.Text);
                    break;
                case 2:
                    GlobalProcedures.ExchangeCalculator(FounderDate.Text);
                    break;
                case 4:
                    GlobalProcedures.ExchangeCalculator(AccountDate.Text);
                    break;
                case 6:
                    GlobalProcedures.ExchangeCalculator(OtherDate.Text);
                    break;
                case 8:
                    GlobalProcedures.ExchangeCalculator(ServiceDate.Text);
                    break;
                case 10:
                    GlobalProcedures.ExchangeCalculator(SalaryDate.Text);
                    break;
            }
        }

        private void CurrencyRateValue_EditValueChanged(object sender, EventArgs e)
        {
            hostage_amount_azn = Math.Round(hostage_amount * (double)CurrencyRateValue.Value, 2);
            PaymentAZNAmountText.Text = hostage_amount_azn.ToString("N2");
        }

        private void InsertPayment()
        {
            if (!ResponsibleCheck.Checked)
            {
                responsible_id = 0;
                responsible_card_id = 0;
            }
            else
                responsible_card_id = GlobalFunctions.GetID($@"SELECT MAX(ID) FROM CRS_USER.PERSONNEL_CARDS WHERE PERSONNEL_ID = {responsible_id}");

            int PaymentID = GlobalFunctions.GetOracleSequenceValue("CASH_EXPENSES_PAYMENT_SEQUENCE");
            GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.CASH_EXPENSES_PAYMENT(ID,CONTRACT_ID,PAYMENT_DATE,AMOUNT,AMOUNT_AZN,CURRENCY_ID,CURRENCY_RATE,NOTE,PERSONNEL_ID,PERSONNEL_CARD_ID,CREDIT_NAME_ID)VALUES(" + PaymentID + "," + ContractID + ",TO_DATE('" + PaymentDate.Text + "','DD/MM/YYYY')," + hostage_amount.ToString(GlobalVariables.V_CultureInfoEN) + "," + hostage_amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + "," + currency_id + "," + CurrencyRateValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + PaymentNoteText.Text.Trim() + "'," + responsible_id + "," + responsible_card_id + "," + CreditNameID + ")",
                                             $@"UPDATE CRS_USER.CONTRACTS SET IS_EXPENSES = 1 WHERE ID = {ContractID}",
                                                "Lizinq müqaviləsi üzrə məxaric daxil olunmadı.");
            if (currency_id == 1)
                GlobalProcedures.InsertCashOperation(7, PaymentID, PaymentDate.Text, ContractCodeText.Text.Trim(), 0, hostage_amount, 1);
            else
                GlobalProcedures.InsertCashOperation(7, PaymentID, PaymentDate.Text, ContractCodeText.Text.Trim(), 0, hostage_amount_azn, 1);
        }

        private void UpdatePayment()
        {
            if (!ResponsibleCheck.Checked)
            {
                responsible_id = 0;
                responsible_card_id = 0;
            }
            else
            {
                responsible_id = GlobalFunctions.FindComboBoxSelectedValue("PERSONNEL", "SURNAME||' '||NAME||' '||PATRONYMIC", "ID", ResponsibleComboBox.Text);
                responsible_card_id = GlobalFunctions.GetID("SELECT MAX(ID) FROM CRS_USER.PERSONNEL_CARDS WHERE PERSONNEL_ID = " + responsible_id);
            }
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_EXPENSES_PAYMENT SET NOTE = '" + PaymentNoteText.Text.Trim() + "',PERSONNEL_ID = " + responsible_id + ",PERSONNEL_CARD_ID = " + responsible_card_id + " WHERE ID = " + OperationOwnerID + " AND CONTRACT_ID = " + ContractID,
                                                "Lizinq müqaviləsi üzrə məxaric dəyişdirilmədi.");
            if (currency_id == 1)
                GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 7, int.Parse(OperationOwnerID), PaymentDate.Text, 0, hostage_amount);
            else
                GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 7, int.Parse(OperationOwnerID), PaymentDate.Text, 0, hostage_amount_azn);
            if (ResponsibleCheck.Checked)
                LoadExpenditure();
        }

        private void InsertFounder()
        {
            FounderID = GlobalFunctions.GetOracleSequenceValue("CASH_FOUNDER_SEQUENCE");
            int foundermaxcode = GlobalFunctions.GetMax("SELECT NVL(MAX(SUBSTR(CODE,2,4)),0)+1 FROM CRS_USER.CASH_FOUNDER WHERE INC_EXP = 2");
            string founder_code = "T" + foundermaxcode.ToString().PadLeft(3, '0');
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_FOUNDER(ID,FOUNDER_ID,FOUNDER_CARD_ID,PAYMENT_DATE,APPOINTMENT,AMOUNT,INC_EXP,NOTE,CODE)VALUES(" + FounderID + "," + founder_id + "," + founder_card_id + ",TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY'),'" + FounderAppointmentText.Text.Trim() + "'," + FounderAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",2,'" + FounderNoteText.Text.Trim() + "','" + founder_code + "')",
                                                "Təsisçi ilə hesablaşmanın məxarici daxil olunmadı.");
            GlobalProcedures.InsertCashOperation(8, FounderID, FounderDate.Text, null, 0, (double)FounderAmountValue.Value, 1);
        }

        private void UpdateFounder()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_FOUNDER SET founder_id = " + founder_id + ",FOUNDER_CARD_ID = " + founder_card_id + ",PAYMENT_DATE = TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY'),APPOINTMENT = '" + FounderAppointmentText.Text.Trim() + "',AMOUNT = " + FounderAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + FounderNoteText.Text.Trim() + "' WHERE ID = " + OperationOwnerID,
                                                "Təsisçi ilə hesablaşmanın məxarici dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 8, FounderID, FounderDate.Text, 0, (double)FounderAmountValue.Value);
        }

        private void InsertBankAccount()
        {
            int AccountID = GlobalFunctions.InsertQuery($@"INSERT INTO CRS_USER.CASH_BANK_ACCOUNT(ADATE,BANK_ID,APPOINTMENT,AMOUNT,NOTE,INC_EXP)VALUES(TO_DATE('{AccountDate.Text}','DD/MM/YYYY'),{bank_id},'{AccountAppointmentText.Text.Trim()}',{AccountAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},'{AccountNoteText.Text.Trim()}',2) RETURNING ID INTO :ID",
                                                "Bank hesabının məxarici daxil olunmadı.",
                                                "ID",
                                                this.Name + "/InsertBankAccount");
            GlobalProcedures.InsertCashOperation(9, AccountID, AccountDate.Text, null, 0, (double)AccountAmountValue.Value, 1);
        }

        private void UpdateBankAccount()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CASH_BANK_ACCOUNT SET ADATE = TO_DATE('{AccountDate.Text}','DD/MM/YYYY'),BANK_ID = {bank_id},APPOINTMENT = '{AccountAppointmentText.Text.Trim()}',AMOUNT = {AccountAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{AccountNoteText.Text.Trim()}' WHERE INC_EXP = 2 AND ID = {OperationOwnerID}",
                                                "Bank hesabının məxarici dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 9, int.Parse(OperationOwnerID), AccountDate.Text, 0, (double)AccountAmountValue.Value);
        }

        private void InsertBankOperationAccount()
        {
            string s = $@"SELECT BANK_ID,TO_CHAR(ADATE,'DD/MM/YYYY') OPERATION_DATE,AMOUNT INCOME,(CASE WHEN NOTE IS NULL THEN 'Kassa əməliyyatlarının məxaricindən daxil olub' ELSE NOTE||' -'||'Kassa əməliyyatlarının məxaricindən daxil olub' END) NOTE,ID,BANK_OPERATION_ID FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 2 AND BANK_ID = {bank_id} AND ADATE = TO_DATE('{AccountDate.Text.Trim()}','DD/MM/YYYY')";
            try
            {                
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s).Rows)
                {
                    double income = Convert.ToDouble(dr["INCOME"].ToString());
                    if (TransactionName == "INSERT")
                    {
                        if (String.IsNullOrEmpty(dr["BANK_OPERATION_ID"].ToString()))
                        {                            
                            GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.BANK_OPERATIONS(ID,
                                                                                                        BANK_ID,
                                                                                                        OPERATION_DATE,
                                                                                                        APPOINTMENT_ID,
                                                                                                        INCOME,
                                                                                                        EXPENSES,
                                                                                                        DEBT,
                                                                                                        NOTE)
                                                            VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL,
                                                                        {dr["BANK_ID"]},
                                                                        TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'),
                                                                        4,
                                                                        {income.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                        0,0,
                                                                        '{dr["NOTE"]}')",
                                                            $@"UPDATE CRS_USER.CASH_BANK_ACCOUNT SET BANK_OPERATION_ID = {OperationID} WHERE ID = {dr["ID"]}",
                                                                "Bankın möhkəmləndirilməsi üçün olan məbləğ bank əməliyyatlarına daxil olmadı.");
                        }
                    }
                    else
                        GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.BANK_OPERATIONS SET BANK_ID = {dr["BANK_ID"]}, OPERATION_DATE = TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'), INCOME = {income.ToString(GlobalVariables.V_CultureInfoEN)},EXPENSES = 0, NOTE = '{dr["NOTE"]}' WHERE ID = {dr["BANK_OPERATION_ID"]}",
                                                            "Bankın möhkəmləndirilməsi üçün olan məbləğ bank əməliyyatlarında dəyişdirilmədi.");
                }
                GlobalProcedures.UpdateBankOperationDebtWithBank(AccountDate.Text, bank_id);
                GlobalProcedures.UpdateBankOperationDebt(AccountDate.Text);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Bankın möhkəmləndirilməsi üçün kassa əməliyyatlarının rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void InsertOther()
        {
            int maxcode = GlobalFunctions.GetMax("SELECT NVL(MAX(SUBSTR(CODE,2,4)),0)+1 FROM CRS_USER.CASH_EXPENSES_OTHER_PAYMENT");
            string code = "Q" + maxcode.ToString().PadLeft(3, '0');
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_EXPENSES_OTHER_PAYMENT(ID,FULLNAME,CASH_APPOINTMENT_ID,PAYMENT_DATE,AMOUNT,CARD_SERIES_ID,CARD_NUMBER,CARD_ISSUE_DATE,CARD_RELIABLE_DATE,CARD_ISSUING_ID,ADRESS,REGISTRATION_ADDRESS,NOTE,CODE)VALUES(" + OtherID + ",'" + OtherFullNameText.Text.Trim() + "','" + other_appointment_id + "',TO_DATE('" + OtherDate.Text + "','DD/MM/YYYY')," + OtherAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + other_card_series_id + ",'" + OtherNumberText.Text.Trim() + "',TO_DATE('" + OtherIssueDate.Text + "','DD/MM/YYYY'),TO_DATE('" + OtherReliableDate.Text + "','DD/MM/YYYY')," + other_card_issuing_id + ",'" + OtherAddressText.Text.Trim() + "','" + OtherRegistrationAddressText.Text.Trim() + "','" + OtherNoteText.Text.Trim() + "','" + code + "')",
                                                "Digər ödənişlərin məxarici daxil olunmadı.");
            GlobalProcedures.InsertCashOperation(10, OtherID, OtherDate.Text, null, 0, (double)OtherAmountValue.Value, 1);
        }

        private void UpdateOther()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_EXPENSES_OTHER_PAYMENT SET FULLNAME = '" + OtherFullNameText.Text.Trim() + "',CASH_APPOINTMENT_ID = '" + other_appointment_id + "',PAYMENT_DATE = TO_DATE('" + OtherDate.Text + "','DD/MM/YYYY'),AMOUNT = " + OtherAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",CARD_SERIES_ID = " + other_card_series_id + ",CARD_NUMBER = '" + OtherNumberText.Text.Trim() + "',CARD_ISSUE_DATE = TO_DATE('" + OtherIssueDate.Text + "','DD/MM/YYYY'),CARD_RELIABLE_DATE = TO_DATE('" + OtherReliableDate.Text + "','DD/MM/YYYY'),CARD_ISSUING_ID = " + other_card_issuing_id + ",ADRESS = '" + OtherAddressText.Text.Trim() + "',REGISTRATION_ADDRESS = '" + OtherRegistrationAddressText.Text.Trim() + "',NOTE = '" + OtherNoteText.Text.Trim() + "' WHERE ID = " + OtherID,
                                                "Təsisçi ilə hesablaşmanın məxarici dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 10, OtherID, OtherDate.Text, 0, (double)OtherAmountValue.Value);
        }

        private void InsertService()
        {
            int maxcode = GlobalFunctions.GetMax("SELECT NVL(MAX(SUBSTR(CODE,2,4)),0)+1 FROM CRS_USER.CASH_EXPENSES_SERVICE_PRICE");
            string code = "Q" + maxcode.ToString().PadLeft(3, '0');
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_EXPENSES_SERVICE_PRICE(ID,FULLNAME,APPOINTMENT,PAYMENT_DATE,AMOUNT,CARD_SERIES_ID,CARD_NUMBER,CARD_ISSUE_DATE,CARD_RELIABLE_DATE,CARD_ISSUING_ID,ADRESS,REGISTRATION_ADDRESS,NOTE,CODE)VALUES(" + ServiceID + ",'" + ServiceFullNameText.Text.Trim() + "','" + ServiceAppointmentText.Text.Trim() + "',TO_DATE('" + ServiceDate.Text + "','DD/MM/YYYY')," + ServiceAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + service_card_series_id + ",'" + ServiceNumberText.Text.Trim() + "',TO_DATE('" + ServiceIssueDate.Text + "','DD/MM/YYYY'),TO_DATE('" + ServiceReliableDate.Text + "','DD/MM/YYYY')," + service_card_issuing_id + ",'" + ServiceAddressText.Text.Trim() + "','" + ServiceRegistrationAddressText.Text.Trim() + "','" + ServiceNoteText.Text.Trim() + "','" + code + "')",
                                                "Xidmət haqqının məxarici daxil olunmadı.");
            GlobalProcedures.InsertCashOperation(11, ServiceID, ServiceDate.Text, null, 0, (double)ServiceAmountValue.Value, 1);
        }

        private void UpdateService()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_EXPENSES_SERVICE_PRICE SET FULLNAME = '" + ServiceFullNameText.Text.Trim() + "',APPOINTMENT = '" + ServiceAppointmentText.Text.Trim() + "',PAYMENT_DATE = TO_DATE('" + ServiceDate.Text + "','DD/MM/YYYY'),AMOUNT = " + ServiceAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",CARD_SERIES_ID = " + service_card_series_id + ",CARD_NUMBER = '" + ServiceNumberText.Text.Trim() + "',CARD_ISSUE_DATE = TO_DATE('" + ServiceIssueDate.Text + "','DD/MM/YYYY'),CARD_RELIABLE_DATE = TO_DATE('" + ServiceReliableDate.Text + "','DD/MM/YYYY'),CARD_ISSUING_ID = " + service_card_issuing_id + ",ADRESS = '" + ServiceAddressText.Text.Trim() + "',REGISTRATION_ADDRESS = '" + ServiceRegistrationAddressText.Text.Trim() + "',NOTE = '" + ServiceNoteText.Text.Trim() + "' WHERE ID = " + ServiceID,
                                                "Təsisçi ilə hesablaşmanın məxarici dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 11, ServiceID, ServiceDate.Text, 0, (double)ServiceAmountValue.Value);
        }

        private void InsertSalary()
        {
            SalaryID = GlobalFunctions.GetOracleSequenceValue("CASH_SALARY_SEQUENCE");
            int maxcode = GlobalFunctions.GetMax("SELECT NVL(MAX(SUBSTR(CODE,2,4)),0)+1 FROM CRS_USER.CASH_SALARY");
            string code = "S" + maxcode.ToString().PadLeft(3, '0');
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CASH_SALARY(ID,PERSONNEL_ID,PERSONNEL_CARD_ID,PAYMENT_DATE,APPOINTMENT,AMOUNT,NOTE,CODE)VALUES(" + SalaryID + "," + personnel_id + "," + personnel_card_id + ",TO_DATE('" + SalaryDate.Text + "','DD/MM/YYYY'),'" + SalaryAppointmentText.Text.Trim() + "'," + SalaryAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + SalaryNoteText.Text.Trim() + "','" + code + "')",
                                                "İşçinin əmək haqqısı məxaric olunmadı.");
            GlobalProcedures.InsertCashOperation(12, SalaryID, SalaryDate.Text, null, 0, (double)SalaryAmountValue.Value, 1);
        }

        private void UpdateSalary()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_SALARY SET PERSONNEL_ID = " + personnel_id + ",PERSONNEL_CARD_ID = " + personnel_card_id + ",PAYMENT_DATE = TO_DATE('" + SalaryDate.Text + "','DD/MM/YYYY'),APPOINTMENT = '" + SalaryAppointmentText.Text.Trim() + "',AMOUNT = " + SalaryAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + SalaryNoteText.Text.Trim() + "' WHERE ID = " + OperationOwnerID,
                                                "İşçinin əmək haqqısı dəyişdirilmədi.");
            GlobalProcedures.UpdateCashOperation(int.Parse(OperationID), 12, SalaryID, SalaryDate.Text, 0, (double)SalaryAmountValue.Value);
        }

        private void InsertExence(int selectedpageindex)
        {
            switch (selectedpageindex)
            {
                case 0: //lizinq odenisi
                    {
                        close_form = true;
                        PaymentProgressPanel.Show();
                        Application.DoEvents();
                        InsertPayment();
                        LoadExpenditure();
                        operationdate = PaymentDate.Text;
                    }
                    break;
                case 2: //tesisci ile hesablasma
                    {
                        double diff = 0, debt = GlobalFunctions.GetAmount("SELECT NVL(SUM(BUY_AMOUNT) - SUM(BASIC_AMOUNT),0) AMOUNT FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id);
                        diff = (double)FounderAmountValue.Value + debt;
                        if (diff < 0)
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("Əgər seçilmiş təsisçi üçün bu hesablaşma əməliyyatını etsəz, bu zaman təsisçinin cəlb olunmuş vəsaitlərdə qalığı mənfi olacaq. Buna razısınız?", "Təsisçinin cəl olunmuş vəsaitlərdəki qalığı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                FounderProgressPanel.Show();
                                Application.DoEvents();
                                InsertFounder();
                                InsertFundsPayments();
                                UpdateCashFounder(funds_payment_id);
                                close_form = true;
                            }
                            else
                                close_form = false;
                        }
                        else
                        {
                            close_form = true;
                            FounderProgressPanel.Show();
                            Application.DoEvents();
                            InsertFounder();
                            InsertFundsPayments();
                            UpdateCashFounder(funds_payment_id);
                        }
                        operationdate = FounderDate.Text;
                    }
                    break;
                case 4: //bank hesabi
                    {
                        close_form = true;
                        AccountProgressPanel.Show();
                        Application.DoEvents();
                        InsertBankAccount();
                        InsertBankOperationAccount();
                        operationdate = AccountDate.Text;
                    }
                    break;
                case 6: //diger odenisler
                    {
                        close_form = true;
                        OtherProgressPanel.Show();
                        Application.DoEvents();
                        InsertOther();
                        UpdateCardFrontFace(6, OtherID);
                        UpdateCardRearFace(6, OtherID);
                        operationdate = OtherDate.Text;
                    }
                    break;
                case 8: //xidmet haqqi
                    {
                        close_form = true;
                        ServiceProgressPanel.Show();
                        Application.DoEvents();
                        InsertService();
                        UpdateCardFrontFace(8, ServiceID);
                        UpdateCardRearFace(8, ServiceID);
                        operationdate = ServiceDate.Text;
                    }
                    break;
                case 10: //emek haqqi
                    {
                        close_form = true;
                        SalaryProgressPanel.Show();
                        Application.DoEvents();
                        InsertSalary();
                        operationdate = SalaryDate.Text;
                    }
                    break;
            }
        }

        private void UpdateExence(int selectedpageindex)
        {
            switch (selectedpageindex)
            {
                case 0: //lizinq odenisi
                    {
                        PaymentProgressPanel.Show();
                        Application.DoEvents();
                        UpdatePayment();
                        LoadExpenditure();
                        operationdate = PaymentDate.Text;
                    }
                    break;
                case 2: //tesisci ile hesablasma
                    {
                        FounderProgressPanel.Show();
                        Application.DoEvents();
                        UpdateFounder();
                        operationdate = FounderDate.Text;
                    }
                    break;
                case 4: //bank hesabi
                    {
                        AccountProgressPanel.Show();
                        Application.DoEvents();
                        UpdateBankAccount();
                        InsertBankOperationAccount();
                        operationdate = AccountDate.Text;
                    }
                    break;
                case 6: //diger odenisler
                    {
                        OtherProgressPanel.Show();
                        Application.DoEvents();
                        UpdateOther();
                        UpdateCardFrontFace(6, OtherID);
                        UpdateCardRearFace(6, OtherID);
                        operationdate = OtherDate.Text;
                    }
                    break;
                case 8: //xidmet haqqi
                    {
                        ServiceProgressPanel.Show();
                        Application.DoEvents();
                        UpdateService();
                        UpdateCardFrontFace(8, ServiceID);
                        UpdateCardRearFace(8, ServiceID);
                        operationdate = ServiceDate.Text;
                    }
                    break;
                case 10: //emek haqqi
                    {
                        SalaryProgressPanel.Show();
                        Application.DoEvents();
                        UpdateSalary();
                        operationdate = SalaryDate.Text;
                    }
                    break;
            }
        }

        private void LoadExpenditure()
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\RDLC\\Expenditure.rdlc"))
            {
                GlobalProcedures.ShowErrorMessage("Expenditure.rdlc çap faylı " + GlobalVariables.V_ExecutingFolder + "\\RDLC ünvanında yoxdur.");
                return;
            }

            PaymentProgressPanel.Visible = true;
            Application.DoEvents();
            string amount_with_word,
                day = String.Format("{0:dd}", GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy")),
                month = GlobalFunctions.FindMonth(GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy").Month),
                year = GlobalFunctions.FindYear(GlobalFunctions.ChangeStringToDate(PaymentDate.Text, "ddmmyyyy")),
                seller, card;
            double d;

            if (currency_id == 1)
                d = hostage_amount;
            else
                d = hostage_amount_azn;
            amount_with_word = GlobalFunctions.AmountInWritining(d, 1, false);

            if (ResponsibleCheck.Checked)
            {
                seller = ResponsibleComboBox.Text;
                card = ResponsibleCardText.Text.Trim();
            }
            else
            {
                seller = SellerNameText.Text.Trim();
                card = SellerCardText.Text.Trim();
            }

            rv_expenditure.LocalReport.ReportPath = GlobalVariables.V_ExecutingFolder + "\\RDLC\\Expenditure.rdlc";
            ReportParameter p1 = new ReportParameter("PContractCode", ContractCodeText.Text.Trim());
            ReportParameter p2 = new ReportParameter("PName", seller);
            ReportParameter p3 = new ReportParameter("PReason", ContractCodeText.Text.Trim() + " saylı müqavilə üzrə alqı satqı");
            ReportParameter p4 = new ReportParameter("PAmountWithString", amount_with_word);
            ReportParameter p5 = new ReportParameter("PCard", card);
            ReportParameter p6 = new ReportParameter("PMonth", month);
            ReportParameter p7 = new ReportParameter("PDay", "«" + day + "»");
            ReportParameter p8 = new ReportParameter("PYear", year);
            ReportParameter p9 = new ReportParameter("PAmount", d.ToString("N2"));
            rv_expenditure.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 });

            Warning[] warnings;
            try
            {
                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Reports\\Məxaric.doc"))
                    File.Delete(GlobalVariables.V_ExecutingFolder + "\\Reports\\Məxaric.doc");
                using (var stream = File.Create(GlobalVariables.V_ExecutingFolder + "\\Reports\\Məxaric.doc"))
                {
                    rv_expenditure.LocalReport.Render(
                        "WORD",
                        @"<DeviceInfo><ExpandContent>True</ExpandContent></DeviceInfo>",
                        (CreateStreamCallback)delegate { return stream; },
                        out warnings);
                }
                PaymentProgressPanel.Visible = false;
                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Reports\\Məxaric.doc"))
                {
                    GlobalVariables.WordDocumentUsed = true;
                    Process.Start(GlobalVariables.V_ExecutingFolder + "\\Reports\\Məxaric.doc");
                }
                else
                {
                    GlobalVariables.WordDocumentUsed = false;
                    XtraMessageBox.Show("Məxaricin çap faylı yaradılmayıb.");
                }
            }
            catch (Exception exx)
            {
                XtraMessageBox.Show("Məxaric.doc faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
        }

        private void UpdateCashFounder(string paymentid)
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CASH_FOUNDER SET FUND_PAYMENT_ID = " + paymentid + " WHERE INC_EXP = 2 AND PAYMENT_DATE = TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY') AND FOUNDER_ID = " + founder_id + " AND FOUNDER_CARD_ID = " + founder_card_id + " AND ID = " + FounderID,
                                                       "Məxaric dəyişdirilmədi.");
        }

        private void InsertFundsPayments()
        {
            GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER.FUNDS_PAYMENTS WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
                                                    "INSERT INTO CRS_USER.FUNDS_PAYMENTS(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,MANUAL_INTEREST FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE = 1 AND CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                    "Ödənişlər əsas cədvələ daxil edilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlExpensesDetails(ExpensesBackstageViewControl.SelectedTabIndex))
            {
                if (TransactionName == "INSERT")
                    InsertExence(ExpensesBackstageViewControl.SelectedTabIndex);
                else
                    UpdateExence(ExpensesBackstageViewControl.SelectedTabIndex);

                if (close_form)
                    Close();
                RefreshCashDataGridView(operationdate);
            }
        }

        private void FExpensesAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_UNLOCK_CASH_DATA", "P_OWNER_ID", int.Parse(OperationOwnerID), "P_OPERATION_ID", int.Parse(OperationID), "Kassaya aid olan cədvəllər blokdan çıxmadı");

            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_UNLOCK_FUND_DATA", "P_FOUNDER_ID", founder_id, "Təsisçiyə aid olan cədvəllə blokdan çıxmadı.");
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID} AND CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = {founder_id})",
                                                "Cəlb olunmuş vəsaitlər temp cədvəldən silinmədi.");
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACTS", -1, "WHERE ID = " + ContractID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.DeleteAllFilesInDirectory(IDImagePath);
            GlobalProcedures.DeleteAllFilesInDirectory(PersonnelImagePath);
            if (ExpensesBackstageViewControl.SelectedTabIndex == 0)
                rv_expenditure.Reset();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ControlExpensesDetails(int index)
        {
            bool b = false;

            switch (index)
            {
                case 0:
                    {
                        if (String.IsNullOrEmpty(ContractCodeText.Text))
                        {
                            ContractCodeText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Müqavilənin nömrəsi daxil edilməyib");
                            ContractCodeText.Focus();
                            ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(PaymentDate.Text))
                        {
                            PaymentDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Ödənişin tarixi daxil edilməyib");
                            PaymentDate.Focus();
                            PaymentDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (ContractID == 0)
                        {
                            ContractCodeText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage(ContractCodeText.Text + " saylı lizinq müqaviləsinə uyğun məlumatlar tapılmadı.");
                            ContractCodeText.Focus();
                            ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        //if (String.IsNullOrEmpty(SellerNameText.Text))
                        //{
                        //    SellerNameText.BackColor = Color.Red;
                        //    GlobalProcedures.ShowErrorMessage("Satıcının adı daxil edilməyib");                            
                        //    SellerNameText.Focus();
                        //    SellerNameText.BackColor = Class.GlobalFunctions.ElementColor();
                        //    return false;
                        //}
                        //else
                        //    b = true;

                        //if (String.IsNullOrEmpty(SellerCardText.Text))
                        //{
                        //    SellerCardText.BackColor = Color.Red;
                        //    GlobalProcedures.ShowErrorMessage("Satıcının şəxsiyyətini təsdiq edən sənəd daxil edilməyib");                            
                        //    SellerCardText.Focus();
                        //    SellerCardText.BackColor = GlobalFunctions.ElementColor();
                        //    return false;
                        //}
                        //else
                        //    b = true;

                        if (String.IsNullOrEmpty(PaymentAmountText.Text) || (currency_id != 1 && hostage_amount_azn == 0))
                        {
                            PaymentAmountText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Məbləğ daxil edilməyib və ya məzənnə daxil edilməyib.");
                            PaymentAmountText.Focus();
                            PaymentAmountText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (TransactionName == "INSERT" && GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CASH_EXPENSES_PAYMENT WHERE CONTRACT_ID = " + ContractID) > 0)
                        {
                            ContractCodeText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage(ContractCodeText.Text + " saylı lizinq müqaviləsi üzrə məxaric artıq bazaya daxil edilib.");
                            ContractCodeText.Focus();
                            ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                            return false;
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
                            GlobalProcedures.ShowErrorMessage("Ödənişin tarixi daxil edilməyib.");
                            FounderDate.Focus();
                            FounderDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(FounderComboBox.Text))
                        {
                            FounderComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Soyadı, adı və atasının adı daxil edilməyib.");
                            FounderComboBox.Focus();
                            FounderComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (fund_contract_id == 0)
                        {
                            FounderComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Seçilmiş təsisçinin heç bir müqaviləsi olmadığı üçün kassadan məxaric etmək olmaz.");
                            FounderComboBox.Focus();
                            FounderComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(FounderAppointmentText.Text))
                        {
                            FounderAppointmentText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Təyinat daxil edilməyib.");
                            FounderAppointmentText.Focus();
                            FounderAppointmentText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (FounderAmountValue.Value <= 0)
                        {
                            FounderAmountValue.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                            FounderAmountValue.Focus();
                            FounderAmountValue.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (founder_card_id == 0)
                        {
                            FounderComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Təsisçinin şəxsiyyətini təsdiq edən sənəd daxil edilməyib.");
                            FounderComboBox.Focus();
                            FounderComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (TransactionName == "INSERT" && GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.FUNDS_PAYMENTS WHERE PAYMENT_DATE = TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY') AND PAYMENT_AMOUNT > 0 AND CONTRACT_ID = " + fund_contract_id) > 0)
                        {
                            FounderDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage(FounderDate.Text + " tarixinə artıq ödəniş olunub.");
                            FounderDate.Focus();
                            FounderDate.BackColor = GlobalFunctions.ElementColor();
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
                            GlobalProcedures.ShowErrorMessage("Ödənişin tarixi daxil edilməyib");
                            AccountDate.Focus();
                            AccountDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (BankLookUp.EditValue == null)
                        {
                            BankLookUp.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Bankın adı daxil edilməyib");
                            BankLookUp.Focus();
                            BankLookUp.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(AccountAppointmentText.Text))
                        {
                            AccountAppointmentText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Təyinat daxil edilməyib");
                            AccountAppointmentText.Focus();
                            AccountAppointmentText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (AccountAmountValue.Value <= 0)
                        {
                            AccountAmountValue.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır");
                            AccountAmountValue.Focus();
                            AccountAmountValue.BackColor = GlobalFunctions.ElementColor();
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
                            GlobalProcedures.ShowErrorMessage("Ödənişin tarixi daxil edilməyib.");
                            OtherDate.Focus();
                            OtherDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherFullNameText.Text))
                        {
                            OtherFullNameText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Soyadı, adı və atasının adı daxil edilməyib.");
                            OtherFullNameText.Focus();
                            OtherFullNameText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherAppointmentComboBox.Text))
                        {
                            OtherAppointmentComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Təyinat daxil edilməyib.");
                            OtherAppointmentComboBox.Focus();
                            OtherAppointmentComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (OtherAmountValue.Value <= 0)
                        {
                            OtherAmountValue.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                            OtherAmountValue.Focus();
                            OtherAmountValue.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherSeriesComboBox.Text))
                        {
                            OtherSeriesComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriyası daxil edilməyib.");
                            OtherSeriesComboBox.Focus();
                            OtherSeriesComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherNumberText.Text))
                        {
                            OtherNumberText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi daxil edilməyib.");
                            OtherNumberText.Focus();
                            OtherNumberText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if ((other_card_series_id == 2) && (OtherNumberText.Text.Trim().Length != 8))
                        {
                            OtherNumberText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi 8 simvol olmalıdır.");
                            OtherNumberText.Focus();
                            OtherNumberText.BackColor = GlobalFunctions.ElementColor(); ;
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherIssueDate.Text))
                        {
                            OtherIssueDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi daxil edilməyib.");
                            OtherIssueDate.Focus();
                            OtherIssueDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else if (GlobalFunctions.ChangeStringToDate(OtherIssueDate.Text, "ddmmyyyy") == GlobalFunctions.ChangeStringToDate(OtherReliableDate.Text, "ddmmyyyy"))
                        {
                            OtherIssueDate.BackColor = Color.Red;
                            OtherReliableDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi ilə etibarlı olma tarixi eyni ola bilməz.");
                            OtherIssueDate.Focus();
                            OtherIssueDate.BackColor = GlobalFunctions.ElementColor();
                            OtherReliableDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else if (GlobalFunctions.ChangeStringToDate(OtherReliableDate.Text, "ddmmyyyy") < DateTime.Today)
                        {
                            OtherReliableDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Sənədin vaxtı bitib.");
                            OtherReliableDate.Focus();
                            OtherReliableDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherIssuingComboBox.Text))
                        {
                            OtherIssuingComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədi verən orqanın adı daxil edilməyib.");
                            OtherIssuingComboBox.Focus();
                            OtherIssuingComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(OtherRegistrationAddressText.Text.Trim()))
                        {
                            OtherRegistrationAddressText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Qeydiyyatda olduğu ünvan daxil edilməyib.");
                            OtherRegistrationAddressText.Focus();
                            OtherRegistrationAddressText.BackColor = GlobalFunctions.ElementColor();
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
                            GlobalProcedures.ShowErrorMessage("Ödənişin tarixi daxil edilməyib.");
                            ServiceDate.Focus();
                            ServiceDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceFullNameText.Text))
                        {
                            ServiceFullNameText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Soyadı, adı və atasının adı daxil edilməyib.");
                            ServiceFullNameText.Focus();
                            ServiceFullNameText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceAppointmentText.Text))
                        {
                            ServiceAppointmentText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Təyinat daxil edilməyib.");
                            ServiceAppointmentText.Focus();
                            ServiceAppointmentText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (ServiceAmountValue.Value <= 0)
                        {
                            ServiceAmountValue.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                            ServiceAmountValue.Focus();
                            ServiceAmountValue.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceSeriesComboBox.Text))
                        {
                            ServiceSeriesComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriyası daxil edilməyib.");
                            ServiceSeriesComboBox.Focus();
                            ServiceSeriesComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceNumberText.Text))
                        {
                            ServiceNumberText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi daxil edilməyib.");
                            ServiceNumberText.Focus();
                            ServiceNumberText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if ((service_card_series_id == 2) && (ServiceNumberText.Text.Trim().Length != 8))
                        {
                            ServiceNumberText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi 8 simvol olmalıdır.");
                            ServiceNumberText.Focus();
                            ServiceNumberText.BackColor = GlobalFunctions.ElementColor(); ;
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceIssueDate.Text))
                        {
                            ServiceIssueDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi daxil edilməyib.");
                            ServiceIssueDate.Focus();
                            ServiceIssueDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else if (ServiceIssueDate.DateTime == ServiceReliableDate.DateTime)
                        {
                            ServiceIssueDate.BackColor = Color.Red;
                            ServiceReliableDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi ilə etibarlı olma tarixi eyni ola bilməz.");
                            ServiceIssueDate.Focus();
                            ServiceIssueDate.BackColor = GlobalFunctions.ElementColor();
                            ServiceReliableDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else if (ServiceReliableDate.DateTime < DateTime.Today)
                        {
                            ServiceReliableDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Sənədin vaxtı bitib.");
                            ServiceReliableDate.Focus();
                            ServiceReliableDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceIssuingComboBox.Text))
                        {
                            ServiceIssuingComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədi verən orqanın adı daxil edilməyib.");
                            ServiceIssuingComboBox.Focus();
                            ServiceIssuingComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(ServiceRegistrationAddressText.Text.Trim()))
                        {
                            ServiceRegistrationAddressText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Qeydiyyatda olduğu ünvan daxil edilməyib.");
                            ServiceRegistrationAddressText.Focus();
                            ServiceRegistrationAddressText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;
                    }
                    break;
                case 10:
                    {
                        if (String.IsNullOrEmpty(SalaryDate.Text))
                        {
                            SalaryDate.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Ödənişin tarixi daxil edilməyib.");
                            SalaryDate.Focus();
                            SalaryDate.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(PersonnelComboBox.Text))
                        {
                            PersonnelComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Soyadı, adı və atasının adı daxil edilməyib.");
                            PersonnelComboBox.Focus();
                            PersonnelComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (String.IsNullOrEmpty(SalaryAppointmentText.Text))
                        {
                            SalaryAppointmentText.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Təyinat daxil edilməyib.");
                            SalaryAppointmentText.Focus();
                            SalaryAppointmentText.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (SalaryAmountValue.Value <= 0)
                        {
                            SalaryAmountValue.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                            SalaryAmountValue.Focus();
                            SalaryAmountValue.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;

                        if (personnel_card_id == 0)
                        {
                            PersonnelComboBox.BackColor = Color.Red;
                            GlobalProcedures.ShowErrorMessage("İşçinin şəxsiyyətini təsdiq edən sənəd daxil edilməyib.");
                            PersonnelComboBox.Focus();
                            PersonnelComboBox.BackColor = GlobalFunctions.ElementColor();
                            return false;
                        }
                        else
                            b = true;
                    }
                    break;
            }

            return b;
        }

        private void OtherSeriesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            other_card_series_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_SERIES", "SERIES", "ID", OtherSeriesComboBox.Text);
        }

        private void ServiceSeriesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            service_card_series_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_SERIES", "SERIES", "ID", ServiceSeriesComboBox.Text);
        }

        private void OtherIssueDate_EditValueChanged(object sender, EventArgs e)
        {
            OtherReliableDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(OtherIssueDate.Text, "ddmmyyyy");
        }

        private void ServiceIssueDate_EditValueChanged(object sender, EventArgs e)
        {
            ServiceReliableDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(ServiceIssueDate.Text, "ddmmyyyy");
        }

        private void UpdateCardFrontFace(int index, int id)//senedlerin skan formasinin daxil etmek
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
                    command = connection.CreateCommand();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    switch (index)
                    {
                        case 6:
                            {
                                if (!String.IsNullOrEmpty(OtherFrontFaceButtonEdit.Text))
                                {
                                    FileStream front_fls = new FileStream(OtherFrontFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                                    byte[] front_blob = new byte[front_fls.Length];
                                    front_fls.Read(front_blob, 0, System.Convert.ToInt32(front_fls.Length));
                                    front_format = Path.GetExtension(OtherFrontFaceButtonEdit.Text);
                                    front_fls.Close();
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_OTHER_PAYMENT SET CARD_FRONT_FACE_IMAGE = :BlobFrontImage, CARD_FRONT_FACE_IMAGE_FORMAT = '" + front_format + "' WHERE ID = " + id;
                                    OracleParameter front_blobParameter = new OracleParameter();
                                    front_blobParameter.OracleDbType = OracleDbType.Blob;
                                    front_blobParameter.ParameterName = "BlobFrontImage";
                                    front_blobParameter.Value = front_blob;
                                    command.Parameters.Add(front_blobParameter);
                                }
                                else
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_OTHER_PAYMENT SET CARD_FRONT_FACE_IMAGE = null WHERE ID = " + id;
                            }
                            break;
                        case 8:
                            {
                                if (!String.IsNullOrEmpty(ServiceFrontFaceButtonEdit.Text))
                                {
                                    FileStream front_fls = new FileStream(ServiceFrontFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                                    byte[] front_blob = new byte[front_fls.Length];
                                    front_fls.Read(front_blob, 0, System.Convert.ToInt32(front_fls.Length));
                                    front_format = Path.GetExtension(ServiceFrontFaceButtonEdit.Text);
                                    front_fls.Close();
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_SERVICE_PRICE SET CARD_FRONT_FACE_IMAGE = :BlobFrontImage, CARD_FRONT_FACE_IMAGE_FORMAT = '" + front_format + "' WHERE ID = " + id;
                                    OracleParameter front_blobParameter = new OracleParameter();
                                    front_blobParameter.OracleDbType = OracleDbType.Blob;
                                    front_blobParameter.ParameterName = "BlobFrontImage";
                                    front_blobParameter.Value = front_blob;
                                    command.Parameters.Add(front_blobParameter);
                                }
                                else
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_SERVICE_PRICE SET CARD_FRONT_FACE_IMAGE = null WHERE ID = " + id;
                            }
                            break;
                    }

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    switch (index)
                    {
                        case 6:                            
                                GlobalProcedures.LogWrite("Digər ödənişlərin məxaricində şəxsiyyəti təsdiq edən sənədin ön üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                                
                            break;
                        case 8:                            
                                GlobalProcedures.LogWrite("Xidmət haqqının məxaricində şəxsiyyəti təsdiq edən sənədin ön üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                             
                            break;
                    }
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void UpdateCardRearFace(int index, int id)//senedlerin skan formasinin daxil etmek
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
                    command = connection.CreateCommand();
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    switch (index)
                    {
                        case 6:
                            {
                                if (!String.IsNullOrEmpty(OtherRearFaceButtonEdit.Text))
                                {
                                    FileStream rear_fls = new FileStream(OtherRearFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                                    byte[] rear_blob = new byte[rear_fls.Length];
                                    rear_fls.Read(rear_blob, 0, System.Convert.ToInt32(rear_fls.Length));
                                    rear_format = Path.GetExtension(OtherRearFaceButtonEdit.Text);
                                    rear_fls.Close();
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_OTHER_PAYMENT SET CARD_REAR_FACE_IMAGE = :BlobRearImage, CARD_REAR_FACE_IMAGE_FORMAT = '" + rear_format + "' WHERE ID = " + id;
                                    OracleParameter rear_blobParameter = new OracleParameter();
                                    rear_blobParameter.OracleDbType = OracleDbType.Blob;
                                    rear_blobParameter.ParameterName = "BlobRearImage";
                                    rear_blobParameter.Value = rear_blob;
                                    command.Parameters.Add(rear_blobParameter);
                                }
                                else
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_OTHER_PAYMENT SET CARD_REAR_FACE_IMAGE = null WHERE ID = " + id;
                            }
                            break;
                        case 8:
                            {
                                if (!String.IsNullOrEmpty(ServiceRearFaceButtonEdit.Text))
                                {
                                    FileStream rear_fls = new FileStream(ServiceRearFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                                    byte[] rear_blob = new byte[rear_fls.Length];
                                    rear_fls.Read(rear_blob, 0, System.Convert.ToInt32(rear_fls.Length));
                                    rear_format = Path.GetExtension(ServiceRearFaceButtonEdit.Text);
                                    rear_fls.Close();
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_SERVICE_PRICE SET CARD_REAR_FACE_IMAGE = :BlobRearImage, CARD_REAR_FACE_IMAGE_FORMAT = '" + rear_format + "' WHERE ID = " + id;
                                    OracleParameter rear_blobParameter = new OracleParameter();
                                    rear_blobParameter.OracleDbType = OracleDbType.Blob;
                                    rear_blobParameter.ParameterName = "BlobRearImage";
                                    rear_blobParameter.Value = rear_blob;
                                    command.Parameters.Add(rear_blobParameter);
                                }
                                else
                                    command.CommandText = "UPDATE CRS_USER.CASH_EXPENSES_SERVICE_PRICE SET CARD_REAR_FACE_IMAGE = null WHERE ID = " + id;
                            }
                            break;
                    }

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    switch (index)
                    {
                        case 6:
                            GlobalProcedures.LogWrite("Digər ödənişlərin məxaricində şəxsiyyəti təsdiq edən sənədin arxa üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                            break;
                        case 8:
                            GlobalProcedures.LogWrite("Xidmət haqqının məxaricində şəxsiyyəti təsdiq edən sənədin arxa üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                            break;
                    }
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void FounderFrontFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if ((sender as ButtonEdit).Text.Length != 0)
                    Process.Start((sender as ButtonEdit).Text);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin ön üzünün skan formasının ünvanı düz deyil.", (sender as ButtonEdit).Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FounderRearFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if ((sender as ButtonEdit).Text.Length != 0)
                    Process.Start((sender as ButtonEdit).Text);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin arxa üzünün skan formasının ünvanı düz deyil.", (sender as ButtonEdit).Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void OtherIssuingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            other_card_issuing_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_ISSUING", "NAME", "ID", OtherIssuingComboBox.Text);
        }

        private void ServiceIssuingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            service_card_issuing_id = GlobalFunctions.FindComboBoxSelectedValue("CARD_ISSUING", "NAME", "ID", ServiceIssuingComboBox.Text);
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 2:
                    {
                        GlobalProcedures.FillComboBoxEdit(OtherSeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
                        GlobalProcedures.FillComboBoxEdit(ServiceSeriesComboBox, "CARD_SERIES", "SERIES,SERIES,SERIES", "1 = 1 ORDER BY ORDER_ID");
                    }
                    break;
                case 4:
                    {
                        GlobalProcedures.FillComboBoxEdit(OtherIssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                        GlobalProcedures.FillComboBoxEdit(ServiceIssuingComboBox, "CARD_ISSUING", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                    }
                    break;
                case 10:
                    {
                        GlobalProcedures.FillComboBoxEditWithSqlText(PersonnelComboBox, "SELECT SURNAME||' '||NAME||' '||PATRONYMIC,SURNAME||' '||NAME||' '||PATRONYMIC,SURNAME||' '||NAME||' '||PATRONYMIC FROM CRS_USER.PERSONNEL ORDER BY SURNAME");
                        FindPersonnelDetails();
                    }
                    break;
                case 11:
                    GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY 1");
                    break;
                case 15:
                    {
                        GlobalProcedures.FillComboBoxEditWithSqlText(FounderComboBox, "SELECT FULLNAME,FULLNAME,FULLNAME FROM CRS_USER.FOUNDERS ORDER BY ORDER_ID");
                        FindFounderCardID();
                    }
                    break;
            }
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);
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

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
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
            if (FormStatus)
                LoadFundContractsGridView(founder_id);
            if (fund_contract_id > 0)
            {
                if (founder_id != old_founder_id)
                {
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", -1, "WHERE ID = " + old_founder_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", -1, "WHERE ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_PAYMENTS", -1, "WHERE CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", GlobalVariables.V_UserID, "WHERE ID = " + founder_id + " AND USED_USER_ID = -1");
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", GlobalVariables.V_UserID, "WHERE ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + founder_id + ") AND USED_USER_ID = -1");
                    GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_PAYMENTS", GlobalVariables.V_UserID, "WHERE CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + founder_id + ") AND USED_USER_ID = -1");
                }
                old_founder_id = founder_id;
                old_fund_contract_id = fund_contract_id;
            }
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", -1, "WHERE ID = " + old_founder_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_CONTRACTS", -1, "WHERE ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FUNDS_PAYMENTS", -1, "WHERE CONTRACT_ID IN (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_ID = " + old_founder_id + ") AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            }
        }

        private void LoadFundContractsGridView(int founderid)
        {
            string s = null;
            if (TransactionName == "INSERT")
                s = "SELECT FC.ID,FC.CONTRACT_NUMBER,TO_CHAR(FC.START_DATE,'DD.MM.YYYY'),FC.INTEREST||' %',FC.PERIOD||' ay' FROM CRS_USER.FUNDS_CONTRACTS FC,CRS_USER.FOUNDER_CONTRACTS FCON WHERE FC.ID = FCON.FUNDS_CONTRACT_ID AND FCON.FOUNDER_ID = " + founderid + " AND FCON.FOUNDER_CARD_ID = " + founder_card_id + " ORDER BY FC.START_DATE,FC.ID";
            else
                s = "SELECT FC.ID,FC.CONTRACT_NUMBER,TO_CHAR(FC.START_DATE,'DD.MM.YYYY'),FC.INTEREST||' %',FC.PERIOD||' ay' FROM CRS_USER.FUNDS_CONTRACTS FC,CRS_USER.FOUNDER_CONTRACTS FCON WHERE FC.ID = FCON.FUNDS_CONTRACT_ID AND FCON.FOUNDER_ID = " + founderid + " AND FCON.FOUNDER_CARD_ID = " + founder_card_id + " ORDER BY FC.START_DATE,FC.ID";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFundContractsGridView");
                if (dt.Rows.Count > 0)
                {
                    ContractGridControl.DataSource = dt;
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
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçinin müqavilələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FindFounderCard(int founderid)
        {
            string s = $@"SELECT CS.SERIES,
                                   FC.CARD_NUMBER,
                                   TO_CHAR (FC.ISSUE_DATE, 'DD.MM.YYYY'),
                                   TO_CHAR (FC.RELIABLE_DATE, 'DD.MM.YYYY'),
                                   CI.NAME,
                                   FC.REGISTRATION_ADDRESS,
                                   FC.ADDRESS,
                                   FC.ID,
                                   FC.FRONT_FACE_IMAGE,
                                   FC.FRONT_FACE_IMAGE_FORMAT,
                                   FC.REAR_FACE_IMAGE,
                                   FC.REAR_FACE_IMAGE_FORMAT
                              FROM CRS_USER.FOUNDER_CARDS FC,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.CARD_ISSUING CI
                             WHERE     FC.CARD_SERIES_ID = CS.ID
                                   AND FC.CARD_ISSUING_ID = CI.ID
                                   AND FC.ID = (SELECT MAX (ID)
                                                  FROM CRS_USER.FOUNDER_CARDS
                                                 WHERE FOUNDER_ID = FC.FOUNDER_ID)
                                   AND FC.FOUNDER_ID = {founderid}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/FindFounderCard");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FounderSeriestText.Text = dr[0].ToString();
                        FounderNumberText.Text = dr[1].ToString();
                        FounderIssueDateText.Text = dr[2].ToString();
                        FounderReliableDateText.Text = dr[3].ToString();
                        FounderIssuingText.Text = dr[4].ToString();
                        FounderRegistrationAddressText.Text = dr[5].ToString();
                        FounderAddressText.Text = dr[6].ToString();
                        founder_card_id = Convert.ToInt32(dr[7].ToString());

                        if (!DBNull.Value.Equals(dr["FRONT_FACE_IMAGE"]))
                        {
                            Byte[] BLOBData = (byte[])dr["FRONT_FACE_IMAGE"];
                            MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                            if (!Directory.Exists(IDImagePath))
                            {
                                Directory.CreateDirectory(IDImagePath);
                            }
                            GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Front_" + FounderComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"]);
                            FileStream front_fs = new FileStream(IDImagePath + "\\Expenses_Front_" + FounderComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                            stmBLOBData.WriteTo(front_fs);
                            front_fs.Close();
                            stmBLOBData.Close();
                            FounderFrontFaceButtonEdit.Text = IDImagePath + "\\Expenses_Front_" + FounderComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"];
                        }

                        if (!DBNull.Value.Equals(dr["REAR_FACE_IMAGE"]))
                        {
                            Byte[] BLOBData = (byte[])dr["REAR_FACE_IMAGE"];
                            MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                            if (!Directory.Exists(IDImagePath))
                            {
                                Directory.CreateDirectory(IDImagePath);
                            }
                            GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Rear_" + FounderComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"]);
                            FileStream rear_fs = new FileStream(IDImagePath + "\\Expenses_Rear_" + FounderComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                            stmBLOBData.WriteTo(rear_fs);
                            rear_fs.Close();
                            stmBLOBData.Close();
                            FounderRearFaceButtonEdit.Text = IDImagePath + "\\Expenses_Rear_" + FounderComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"];
                        }
                    }
                }
                else
                {
                    FounderSeriestText.Text = null;
                    FounderNumberText.Text = null;
                    FounderIssueDateText.Text = null;
                    FounderReliableDateText.Text = null;
                    FounderIssuingText.Text = null;
                    FounderRegistrationAddressText.Text = null;
                    FounderAddressText.Text = null;
                    founder_card_id = 0;
                    FounderFrontFaceButtonEdit.Text = null;
                    FounderRearFaceButtonEdit.Text = null;
                }

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təsisçinin şəxsiyyətini təsdiq edən sənəd tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FounderComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 15);
        }

        void RefreshFundsPayment(decimal a, int p)
        {
            FounderAmountValue.Value = a;
            funds_payment_id = p.ToString();
        }

        private void LoadFPaymentAmountAddEdit(string transaction)
        {
            double current_debt;
            int pay_count = 0, interest = 0, payment_temp_count = 0;
            string lastdate = null, contract_number = null, start_date = null, currency = null, s = null;
            try
            {
                s = $@"SELECT FC.ID,FC.CONTRACT_NUMBER,FC.INTEREST,TO_CHAR(FC.START_DATE,'DD/MM/YYYY'),C.CODE FROM CRS_USER.FUNDS_CONTRACTS FC,CRS_USER.CURRENCY C WHERE FC.CURRENCY_ID = C.ID AND FC.ID = {fund_contract_id}";
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s).Rows)
                {
                    contract_number = dr[1].ToString();
                    interest = Convert.ToInt32(dr[2].ToString());
                    start_date = dr[3].ToString();
                    currency = dr[4].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            if (fund_contract_id > 0)
            {
                payment_temp_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id);
                if (payment_temp_count == 0)
                    GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                        "INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE," + Class.GlobalVariables.V_UserID + ",MANUAL_INTEREST FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id,
                                                        "Ödənişlər temp cədvələ daxil edilmədi.");

                if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE = TO_DATE('" + FounderDate.Text + "','DD/MM/YYYY') AND PAYMENT_AMOUNT > 0 AND CONTRACT_ID = " + fund_contract_id) == 0)
                {
                    pay_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND CONTRACT_ID = " + fund_contract_id);
                    if (pay_count == 0)
                    {
                        lastdate = start_date;
                        current_debt = 0;
                    }
                    else
                    {
                        lastdate = GlobalFunctions.GetMaxDate("SELECT NVL(MAX(CP.PAYMENT_DATE),TO_DATE('" + FounderDate.Text.Trim() + "','DD/MM/YYYY')) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID).ToString("d", Class.GlobalVariables.V_CultureInfoAZ);
                        current_debt = GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY') AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
                    }

                    if (GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy") <= GlobalFunctions.ChangeStringToDate(FounderDate.Text.Trim(), "ddmmyyyy"))
                    {
                        AttractedFunds.FPaymentAmountAddEdit fpa = new AttractedFunds.FPaymentAmountAddEdit();
                        fpa.TransactionName = transaction;
                        fpa.PaymentCount = pay_count;
                        fpa.LastDate = lastdate;
                        fpa.Debt = current_debt;
                        fpa.ContractID = fund_contract_id.ToString();
                        fpa.ContractCode = contract_number;
                        fpa.SourceID = 0;
                        fpa.ContractStartDate = start_date;
                        fpa.SourceName = "Təsisçi: " + FounderComboBox.Text;
                        fpa.PayDate = FounderDate.Text.Trim();
                        fpa.Currency = currency;
                        fpa.RefreshPaymentsDataGridView += new AttractedFunds.FPaymentAmountAddEdit.DoEvent(RefreshFundsPayment);
                        fpa.ShowDialog();
                    }
                    else
                        XtraMessageBox.Show("Cəlb olunmuş vəsaitlərdə ən son daxil olma tarixi " + lastdate + " olduğundan " + FounderDate.Text.Trim() + " tarixi üçün ödəniş etmək olmaz.");
                }
                else
                {
                    FounderDate.BackColor = Color.Red;
                    XtraMessageBox.Show(FounderDate.Text + " tarixinə artıq ödəniş olunub.");
                    FounderDate.Focus();
                    FounderDate.BackColor = GlobalFunctions.ElementColor();
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş təsisçinin heç bir müqaviləsi olmadığı üçün kassadan məxaric etmək olmaz.");
        }

        private void BContract_Click(object sender, EventArgs e)
        {
            LoadFPaymentAmountAddEdit("INSERT");
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

        private void OtherIssuingComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 3);
        }

        private void OtherSeriesComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 2);
        }

        private void PersonnelNoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (SalaryNoteText.Text.Length <= 400)
                SalaryDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - SalaryNoteText.Text.Length).ToString();
        }

        private void OtherFrontFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Sənədin ön üzünü seçin";
                    dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png;*.pdf)|*.jpeg;*.jpg;*.bmp;*.png;*.pdf|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Pdf files (*.pdf)|*.pdf";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        (sender as ButtonEdit).Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                (sender as ButtonEdit).Text = null;
            else
            {
                try
                {
                    if ((sender as ButtonEdit).Text.Length != 0)
                        Process.Start((sender as ButtonEdit).Text);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin ön üzünün skan formasının ünvanı düz deyil.", (sender as ButtonEdit).Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void OtherRearFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Sənədin arxa üzünü seçin";
                    dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png;*.pdf)|*.jpeg;*.jpg;*.bmp;*.png;*.pdf|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Pdf files (*.pdf)|*.pdf";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        (sender as ButtonEdit).Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                (sender as ButtonEdit).Text = null;
            else
            {
                try
                {
                    if ((sender as ButtonEdit).Text.Length != 0)
                        Process.Start((sender as ButtonEdit).Text);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin arxa üzünün skan formasının ünvanı düz deyil.", (sender as ButtonEdit).Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void FindPersonnelDetails()
        {
            personnel_id = Class.GlobalFunctions.FindComboBoxSelectedValue("PERSONNEL", "SURNAME||' '||NAME||' '||PATRONYMIC", "ID", PersonnelComboBox.Text);
            FindPersonnelImage(personnel_id);
            if (FormStatus)
                FindPersonnelCard(personnel_id);
        }

        private void PersonnelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindPersonnelDetails();
        }

        private void FindPersonnelImage(int personnelid)
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.IMAGE,T.ID FROM CRS_USER.PERSONNEL T WHERE T.ID = {personnelid}", this.Name + "/FindPersonnelImage");
            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["IMAGE"]))
                {

                    Byte[] BLOBData = (byte[])dr["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    PersonnelPictureBox.Image = Image.FromStream(stmBLOBData);

                    if (!Directory.Exists(PersonnelImagePath))
                    {
                        Directory.CreateDirectory(PersonnelImagePath);
                    }

                    GlobalProcedures.DeleteFile(PersonnelImagePath + "\\P_" + dr["ID"] + ".jpeg");
                    FileStream fs = new FileStream(PersonnelImagePath + "\\P_" + dr["ID"] + ".jpeg", FileMode.Create, FileAccess.Write);
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                }
                else
                {
                    PersonnelPictureBox.Image = null;
                }
            }
        }

        private void FindPersonnelCard(int personnelid)
        {
            string s = "SELECT CS.SERIES,FC.CARD_NUMBER,TO_CHAR(FC.ISSUE_DATE,'DD.MM.YYYY'),TO_CHAR(FC.RELIABLE_DATE,'DD.MM.YYYY'),CI.NAME,FC.REGISTRATION_ADDRESS,FC.ADDRESS,FC.ID,FC.FRONT_FACE_IMAGE,FC.REAR_FACE_IMAGE,FC.FRONT_FACE_IMAGE_FORMAT,FC.REAR_FACE_IMAGE_FORMAT FROM CRS_USER.PERSONNEL_CARDS FC,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE FC.CARD_SERIES_ID = CS.ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.ID = (SELECT MAX(ID) FROM CRS_USER.PERSONNEL_CARDS WHERE PERSONNEL_ID = FC.PERSONNEL_ID) AND FC.PERSONNEL_ID = " + personnelid;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/FindPersonnelCard");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SalarySeriesText.Text = dr[0].ToString();
                        SalaryNumberText.Text = dr[1].ToString();
                        SalaryIssueDateText.Text = dr[2].ToString();
                        SalaryReliableDateText.Text = dr[3].ToString();
                        SalaryIssuingText.Text = dr[4].ToString();
                        SalaryRegistrationAddressText.Text = dr[5].ToString();
                        if (!String.IsNullOrEmpty(dr[6].ToString()))
                            SalaryAddressText.Text = dr[6].ToString();
                        else
                            SalaryAddressText.Text = null;
                        personnel_card_id = Convert.ToInt32(dr[7].ToString());

                        if (!DBNull.Value.Equals(dr["FRONT_FACE_IMAGE"]))
                        {
                            Byte[] front_BLOBData = (byte[])dr["FRONT_FACE_IMAGE"];
                            MemoryStream stmBLOBData = new MemoryStream(front_BLOBData);

                            if (!Directory.Exists(IDImagePath))
                            {
                                Directory.CreateDirectory(IDImagePath);
                            }

                            GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Personnel_Rear_" + PersonnelComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"]);
                            FileStream front_fs = new FileStream(IDImagePath + "\\Expenses_Personnel_Front_" + PersonnelComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                            stmBLOBData.WriteTo(front_fs);
                            front_fs.Close();
                            stmBLOBData.Close();
                            SalaryFrontFaceButtonEdit.Text = IDImagePath + "\\Expenses_Personnel_Front_" + PersonnelComboBox.Text + dr["FRONT_FACE_IMAGE_FORMAT"];
                        }

                        if (!DBNull.Value.Equals(dr["REAR_FACE_IMAGE"]))
                        {
                            Byte[] rear_BLOBData = (byte[])dr["REAR_FACE_IMAGE"];
                            MemoryStream stmBLOBData = new MemoryStream(rear_BLOBData);

                            if (!Directory.Exists(IDImagePath))
                            {
                                Directory.CreateDirectory(IDImagePath);
                            }

                            GlobalProcedures.DeleteFile(IDImagePath + "\\Expenses_Personnel_Rear_" + PersonnelComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"]);
                            FileStream front_fs = new FileStream(IDImagePath + "\\Expenses_Personnel_Rear_" + PersonnelComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                            stmBLOBData.WriteTo(front_fs);
                            front_fs.Close();
                            stmBLOBData.Close();
                            SalaryRearFaceButtonEdit.Text = IDImagePath + "\\Expenses_Personnel_Rear_" + PersonnelComboBox.Text + dr["REAR_FACE_IMAGE_FORMAT"];
                        }
                    }
                }
                else
                {
                    SalarySeriesText.Text =
                        SalaryNumberText.Text =
                        SalaryIssueDateText.Text =
                        SalaryReliableDateText.Text =
                        SalaryIssuingText.Text =
                        SalaryRegistrationAddressText.Text =
                        SalaryAddressText.Text =
                        SalaryFrontFaceButtonEdit.Text =
                        SalaryRearFaceButtonEdit.Text = null;
                    personnel_card_id = 0;
                }

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İşçinin şəxsiyyətini təsdiq edən sənəd tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void PersonnelComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 10);
        }

        private void ContractGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractGridView.GetFocusedDataRow();
            if (row != null)
            {
                fund_contract_id = Convert.ToInt32(row["ID"].ToString());
                string lastdate = DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ);
                int pay_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = {fund_contract_id}");
                if (pay_count == 0)
                    lastdate = GlobalFunctions.GetMaxDate($@"SELECT MAX(START_DATE) FROM FUNDS_CONTRACTS WHERE ID = {fund_contract_id}").ToString("d", GlobalVariables.V_CultureInfoAZ);
                else
                    lastdate = GlobalFunctions.GetMaxDate($@"SELECT NVL(MAX(CP.PAYMENT_DATE),TO_DATE('{DateTime.Today.ToString("d")}','DD/MM/YYYY')) FROM CRS_USER.FUNDS_PAYMENTS CP WHERE CP.CONTRACT_ID = {fund_contract_id}").ToString("d", GlobalVariables.V_CultureInfoAZ);

                FounderDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy");
            }
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFundContractsGridView(founder_id);
        }

        private void ContractGridControl_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractGridView, PopupMenu, e);
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

        private void ResponsibleCheck_CheckedChanged(object sender, EventArgs e)
        {
            ResponsibleComboBox.Enabled = ResponsibleCheck.Checked;
        }

        private void ResponsibleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            responsible_id = GlobalFunctions.FindComboBoxSelectedValue("PERSONNEL", "SURNAME||' '||NAME||' '||PATRONYMIC", "ID", ResponsibleComboBox.Text);
            ResponsibleCardText.Text = GlobalFunctions.GetName("SELECT CS.NAME||' : '||CS.SERIES||', №:'||FC.CARD_NUMBER||', '||TO_CHAR(FC.ISSUE_DATE,'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' CARD FROM CRS_USER.PERSONNEL_CARDS FC,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE FC.CARD_SERIES_ID = CS.ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.ID = (SELECT MAX(ID) FROM CRS_USER.PERSONNEL_CARDS WHERE PERSONNEL_ID = FC.PERSONNEL_ID) AND FC.PERSONNEL_ID = " + responsible_id);
        }
    }
}