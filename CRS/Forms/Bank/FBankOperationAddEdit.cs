using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using CRS.Class;

namespace CRS.Forms.Bank
{
    public partial class FBankOperationAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FBankOperationAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, OperationID, PaymentDate, BankName, Date, BankID;
        public bool CloseWindow;
        public int OperationType;

        int appointment_id, fund_contract_id = 0;
        string customer_payment_id = null, contract_code, funds_payment_id;

        public delegate void DoEvent();
        public event DoEvent RefreshOperationsDataGridView;

        private void FBankOperationAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)            
                AppointmentLookUp.Properties.Buttons[1].Visible = GlobalVariables.BankOperation;            

            DateText.Text = Date;
            BankNameText.Text = BankName;
            if (OperationType == 1)
            {
                this.Icon = Properties.Resources.income;
                this.Text = "Mədaxilin əlavə/düzəliş edilməsi";
            }
            else
            {
                this.Icon = Properties.Resources.expenses;
                this.Text = "Məxaricin əlavə/düzəliş edilməsi";
            }
            RefreshDictionaries(12);
            GlobalProcedures.CalcEditFormat(AmountValue);
            if (TransactionName == "EDIT")
            {
                LoadOperationDetails();
                ContractGridControl.Enabled = ContractsBarButton.Enabled = false;
            }
        }

        private void LoadOperationDetails()
        {
            string s = $@"SELECT BA.NAME,
                                   BO.INCOME,
                                   BO.EXPENSES,
                                   BO.NOTE,
                                   FUNDS_CONTRACT_ID,
                                   BA.ID
                              FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO, CRS_USER.BANK_APPOINTMENTS BA
                             WHERE BO.APPOINTMENT_ID = BA.ID AND BO.ID = {OperationID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperationDetails");

            if(dt.Rows.Count > 0)
            {
                AppointmentLookUp.EditValue = AppointmentLookUp.Properties.GetKeyValueByDisplayText(dt.Rows[0]["NAME"].ToString());
                AppointmentLookUp.Enabled = false;
                AmountValue.Value = (OperationType == 1)? Convert.ToDecimal(dt.Rows[0]["INCOME"].ToString()): Convert.ToDecimal(dt.Rows[0]["EXPENSES"].ToString());
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                if (!String.IsNullOrEmpty(dt.Rows[0]["FUNDS_CONTRACT_ID"].ToString()))
                    fund_contract_id = Convert.ToInt32(dt.Rows[0]["FUNDS_CONTRACT_ID"].ToString());
                if (Convert.ToInt32(dt.Rows[0]["ID"].ToString()) == 15 || Convert.ToInt32(dt.Rows[0]["ID"].ToString()) == 18)
                    LoadContractsGridView();
            }

            //try
            //{
            //    if (OperationType == 1) //medaxil
            //        s = $@"SELECT BA.NAME,BO.INCOME,BO.NOTE,FUNDS_CONTRACT_ID,BA.ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO,CRS_USER.BANK_APPOINTMENTS BA WHERE BO.APPOINTMENT_ID = BA.ID AND BO.ID = {OperationID}";
            //    else //mexaric
            //        s = $@"SELECT BA.NAME,BO.EXPENSES,BO.NOTE,FUNDS_CONTRACT_ID,BA.ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO,CRS_USER.BANK_APPOINTMENTS BA WHERE BO.APPOINTMENT_ID = BA.ID AND BO.ID = {OperationID}";
            //    DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperationDetails");

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        AppointmentLookUp.EditValue = AppointmentLookUp.Properties.GetKeyValueByDisplayText(dr[0].ToString());
            //        AppointmentLookUp.Enabled = false;
            //        AmountValue.Value = Convert.ToDecimal(dr[1].ToString());
            //        NoteText.Text = dr[2].ToString();
            //        if (!String.IsNullOrEmpty(dr[3].ToString()))
            //            fund_contract_id = Convert.ToInt32(dr[3].ToString());
            //        if (Convert.ToInt32(dr[4].ToString()) == 15 || Convert.ToInt32(dr[4].ToString()) == 18)
            //            LoadContractsGridView();
            //    }
            //}
            //catch (Exception exx)
            //{
            //    GlobalProcedures.LogWrite("Bank əməliyyatının rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            //}
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FBankOperationAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshOperationsDataGridView();
        }

        private void LoadContractsGridView()
        {
            string s = null;

            if (TransactionName == "INSERT")
                s = $@"SELECT FC.ID,
                             FC.CONTRACT_NUMBER,
                             FC.START_DATE,
                             FC.INTEREST || ' %' INTEREST,
                             FC.PERIOD || ' ay' PERIOD
                        FROM CRS_USER.FUNDS_CONTRACTS FC, CRS_USER.BANK_CONTRACTS BC
                       WHERE     FC.ID = BC.FUNDS_CONTRACT_ID
                             AND FC.STATUS_ID = 13
                             AND BC.BANK_ID = {BankID}
                    ORDER BY FC.START_DATE, FC.ID";
            else
                s = $@"SELECT FC.ID,
                             FC.CONTRACT_NUMBER,
                             FC.START_DATE,
                             FC.INTEREST || ' %' INTEREST,
                             FC.PERIOD || ' ay' PERIOD
                        FROM CRS_USER.FUNDS_CONTRACTS FC, CRS_USER.BANK_CONTRACTS BC
                       WHERE     FC.ID = BC.FUNDS_CONTRACT_ID
                             AND BC.FUNDS_CONTRACT_ID = {fund_contract_id}
                    ORDER BY FC.START_DATE, FC.ID";

            ContractGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractsGridView");
        }

        private void InsertOption()
        {
            int cus_id = 0;
            if (!String.IsNullOrEmpty(customer_payment_id))
                cus_id = Convert.ToInt32(customer_payment_id);
            
            if (OperationType == 1)
                OperationID = GlobalFunctions.InsertQuery("INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,IS_CHANGE,USED_USER_ID,CONTRACT_PAYMENT_ID,CONTRACT_CODE) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + BankID + ",TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')," + appointment_id + "," + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,0,'" + NoteText.Text.Trim() + "',1," + GlobalVariables.V_UserID + "," + cus_id + ",'" + contract_code + "') RETURNING ID INTO :ID",
                                                    "Mədaxil daxil edilmədi.",
                                                    "ID",
                                                    this.Name + "/InsertOption").ToString();
            else
                OperationID = GlobalFunctions.InsertQuery("INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,IS_CHANGE,USED_USER_ID) VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL," + BankID + ",TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')," + appointment_id + ",0," + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,'" + NoteText.Text.Trim() + "',1," + GlobalVariables.V_UserID + ") RETURNING ID INTO :ID",
                                                    "Məxaric daxil edilmədi.",
                                                    "ID",
                                                    this.Name + "/InsertOption").ToString();
        }

        private void UpdateOption()
        {
            if (OperationType == 1)
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET IS_CHANGE = 1,APPOINTMENT_ID = {appointment_id},INCOME = {Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}' WHERE ID = {OperationID}",
                                                    "Mədaxil dəyişdirilmədi.",
                                                    this.Name + "/UpdateOption");
            else
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET IS_CHANGE = 1,APPOINTMENT_ID = {appointment_id},EXPENSES = {Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}' WHERE ID = {OperationID}",
                                                    "Məxaric dəyişdirilmədi.",
                                                    this.Name + "/UpdateOption");
        }

        private void UpdateBankOptionContract()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET FUNDS_CONTRACT_ID = {fund_contract_id} WHERE ID = {OperationID}",
                                                    "Mədaxil dəyişdirilmədi.");
        }

        private bool ControlOperationDetails()
        {
            bool b = false;

            if (AppointmentLookUp.EditValue == null)
            {
                AppointmentLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təyinat seçilməyib.");
                AppointmentLookUp.Focus();
                AppointmentLookUp.BackColor = GlobalFunctions.ElementColor();
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

            if (appointment_id == 15)
            {
                int p_funds_contract_id = GlobalFunctions.GetID($@"SELECT NVL(FUNDS_CONTRACT_ID,0) FROM CRS_USER.BANK_CONTRACTS WHERE BANK_ID = {BankID}");
                if (p_funds_contract_id == 0)
                {
                    XtraMessageBox.Show("Seçilmiş bank, filial və hesab üçün heç bir müqavilə olmadığı üçün bank əməliyyatlarına mədaxil etmək olmaz.");
                    return false;
                }
                else
                    b = true;
            }

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlOperationDetails())
            {
                if (TransactionName == "INSERT")
                {
                    InsertOption();
                    if (CloseWindow)
                        this.Close();
                    else
                    {
                        AmountValue.Value = 0;
                        NoteText.Text = null;
                        this.RefreshOperationsDataGridView();
                    }
                    if (appointment_id == 15)
                    {
                        InsertBuyAmount();
                        UpdateBankOptionContract();
                    }
                    if (appointment_id == 18)
                        UpdateFundPayment(funds_payment_id);
                }
                else
                {
                    UpdateOption();
                    if (appointment_id == 15)
                        UpdateBuyAmount();
                    this.Close();
                }
            }
        }

        private void InsertBuyAmount()
        {
            double currenct_payment_interest_debt = 0, total = 0, current_debt = 0, interest_amount = 0, diff_day = 0, Debt = 0, totaldebtamount = 0,
                payment_interest_debt = 0, payment_interest_amount = 0, basic_amount = 0, one_day_interest = 0, residual_percent = 0;
            int pay_count, interest = 0, payment_temp_count = 0;
            string lastdate, PaymentID;
            payment_temp_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = {fund_contract_id}");
            if (payment_temp_count == 0)
                GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                    "INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE," + GlobalVariables.V_UserID + ",MANUAL_INTEREST FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id,
                                                    "Ödənişlər temp cədvələ daxil edilmədi.");

            if (fund_contract_id > 0)
            {
                PaymentID = GlobalFunctions.GetOracleSequenceValue("FUNDS_PAYMENTS_SEQUENCE").ToString();
                interest = GlobalFunctions.GetID("SELECT INTEREST FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = " + fund_contract_id);
                lastdate = GlobalFunctions.GetMaxDate("SELECT NVL(MAX(CP.PAYMENT_DATE),TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID).ToString("d", GlobalVariables.V_CultureInfoAZ);
                current_debt = GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                Debt = current_debt;
                pay_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')");
                one_day_interest = Math.Round(((Debt * interest) / 100) / 360, 2);
                diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(DateText.Text.Trim(), "ddmmyyyy"));
                interest_amount = diff_day * one_day_interest;
                residual_percent = interest_amount + GlobalFunctions.GetAmount("SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                totaldebtamount = Math.Round((Debt + residual_percent), 2);
                basic_amount = GlobalFunctions.GetAmount("SELECT BASIC_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                payment_interest_debt = GlobalFunctions.GetAmount("SELECT PAYMENT_INTEREST_DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                payment_interest_amount = GlobalFunctions.GetAmount("SELECT PAYMENT_INTEREST_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                if (current_debt <= 0)
                {
                    interest_amount = 0;
                    diff_day = 0;
                    Debt = 0;
                    totaldebtamount = 0;
                }

                if ((pay_count != 0))
                {
                    string m_ldate = GlobalFunctions.GetMaxDate("SELECT MAX(CP.PAYMENT_DATE) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND PAYMENT_DATE < TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND CP.CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID).ToString("d", GlobalVariables.V_CultureInfoAZ);
                    Debt = GlobalFunctions.GetAmount("SELECT CP.DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + m_ldate + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                    interest_amount = GlobalFunctions.GetAmount("SELECT CP.INTEREST_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                }
                currenct_payment_interest_debt = payment_interest_debt + interest_amount - payment_interest_amount;
                current_debt = Debt + (double)AmountValue.Value - basic_amount;
                total = current_debt + currenct_payment_interest_debt;

                if (pay_count == 0)
                    GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,IS_CHANGE)VALUES(" + PaymentID + "," + fund_contract_id + ",TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')," + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0,0," + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + diff_day + "," + Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",0," + Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + "," + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",1,0," + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + NoteText.Text.Trim() + "'," + GlobalVariables.V_UserID + ",1)",
                                                    "Verilən məbləğ temp cədvələ daxil edilmədi.");
                else
                {
                    total = current_debt + payment_interest_debt;
                    GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET BUY_AMOUNT = " + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "',IS_CHANGE = 1 WHERE PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldə dəyişdirilmədi.");
                }
                GlobalProcedures.UpdateFundChange(DateText.Text.Trim(), fund_contract_id, 1);
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET FUNDS_PAYMENT_ID = " + PaymentID + " WHERE OPERATION_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID + " AND BANK_ID = " + BankID + " AND ID = " + OperationID,
                                                    "Mədaxil dəyişdirilmədi.");
            }
        }

        private void UpdateBuyAmount()
        {
            double total = 0, current_debt = 0, Debt = 0, basic_amount = 0, payment_interest_debt = 0, residual_percent = 0, totaldebtamount = 0, one_day_interest = 0, interest_amount = 0;
            string lastdate;
            int diff_day, interest, payment_id, payment_temp_count;
            payment_temp_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id);
            if (payment_temp_count == 0)
                GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                    "INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE," + GlobalVariables.V_UserID + ",MANUAL_INTEREST FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id,
                                                    "Ödənişlər temp cədvələ daxil edilmədi.");

            if (fund_contract_id > 0)
            {
                interest = GlobalFunctions.GetID("SELECT INTEREST FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = " + fund_contract_id);
                payment_id = GlobalFunctions.GetID("SELECT FUNDS_PAYMENT_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE ID = " + OperationID);
                current_debt = GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                payment_interest_debt = GlobalFunctions.GetAmount("SELECT PAYMENT_INTEREST_DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                lastdate = GlobalFunctions.GetMaxDate("SELECT NVL(MAX(PAYMENT_DATE),TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE < TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND CONTRACT_ID = " + fund_contract_id).ToString("d", GlobalVariables.V_CultureInfoAZ);
                Debt = GlobalFunctions.GetAmount("SELECT CP.DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY')");
                Debt = current_debt;
                if (current_debt <= 0)
                    Debt = 0;
                one_day_interest = Math.Round(((Debt * interest) / 100) / 360, 2);
                diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(DateText.Text.Trim(), "ddmmyyyy"));
                interest_amount = diff_day * one_day_interest;
                residual_percent = interest_amount + GlobalFunctions.GetAmount("SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
                basic_amount = GlobalFunctions.GetAmount("SELECT BASIC_AMOUNT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                current_debt = Debt + (double)AmountValue.Value - basic_amount;
                total = current_debt + payment_interest_debt;
                totaldebtamount = Math.Round((Debt + residual_percent), 2);

                if (AmountValue.Value > 0)
                    GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 1, BUY_AMOUNT = " + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "',REQUIRED_CLOSE_AMOUNT = " + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + payment_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldə dəyişdirilmədi.");
                else
                {
                    int pay_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_AMOUNT > 0 AND IS_CHANGE IN (0,1) AND PAYMENT_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')");
                    if (pay_count == 0)
                        GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 2 WHERE ID = " + payment_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldən silinmədi.");
                    else
                        GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 1, BUY_AMOUNT = " + Math.Round(AmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "',REQUIRED_CLOSE_AMOUNT = " + totaldebtamount.ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + payment_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                    "Verilən məbləğ temp cədvəldə dəyişdirilmədi.");
                }
                GlobalProcedures.UpdateFundChange(DateText.Text.Trim(), fund_contract_id, 1);
            }
        }

        private void UpdateFundPayment(string paymentid)
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET FUNDS_PAYMENT_ID = " + paymentid + " WHERE OPERATION_DATE = TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID + " AND BANK_ID = " + BankID + " AND ID = " + OperationID,
                                                       "Mədaxil dəyişdirilmədi.");
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(AppointmentLookUp, "BANK_APPOINTMENTS", "ID", "NAME", "IS_SHOW = 1 AND OPERATION_TYPE_ID = " + OperationType);
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        void RefreshPayment(decimal a, string p)
        {
            AmountValue.Value = a;
            customer_payment_id = p;
            contract_code = GlobalFunctions.GetName("SELECT CT.CODE||C.CODE CONTRACT_CODE FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP,CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE CP.CONTRACT_ID = C.ID AND CP.CUSTOMER_ID = C.CUSTOMER_ID AND C.CREDIT_TYPE_ID = CT.ID AND CP.ID = " + p);
        }

        private void LoadFPaymentBankAddEdit()
        {
            Total.FPaymentBankAddEdit fb = new Total.FPaymentBankAddEdit();
            fb.TransactionName = "BINSERT";
            fb.BankName = BankNameText.Text;
            fb.PDate = DateText.Text;
            fb.RefreshPaymentsDataGridView += new Total.FPaymentBankAddEdit.DoEvent(RefreshPayment);
            fb.ShowDialog();
        }

        private void AppointmentLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 12);
        }

        private void AppointmentLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AppointmentLookUp.EditValue == null)
                return;

            appointment_id = Convert.ToInt32(AppointmentLookUp.EditValue);
            
            if (appointment_id == 3 || appointment_id == 18)
            {
                if (TransactionName == "INSERT")
                {
                    BContract.Visible = true;
                    AmountValue.Enabled = false;
                }
                else
                    BContract.Visible = AmountValue.Enabled = BOK.Visible = false;
            }
            else
            {
                BContract.Visible = false;
                AmountValue.Enabled = BOK.Visible = true;
            }

            if (appointment_id == 15 || appointment_id == 18)
            {
                LoadContractsGridView();
                GroupControl.Visible = true;
                BContract.Text = "Bank müqaviləsi üzrə axtar";
                BContract.SuperTip.Items.Clear();
                BContract.SuperTip.Items.AddTitle("<color=255,0,0>Bank müqaviləsi üzrə axtar</color>");
                BContract.SuperTip.Items.Add("Bank ödənişinin hansı müqaviləyə aid olduğunu müəyyənləşdirmək üçün nəzərdə tutulub.");
            }
            else
            {
                GroupControl.Visible = false;
                BContract.Text = "Liziq müqaviləsi üzrə axtar";
                BContract.SuperTip.Items.Clear();
                BContract.SuperTip.Items.AddTitle("<color=255,0,0>Liziq müqaviləsi üzrə axtar</color>");
                BContract.SuperTip.Items.Add("Lizinq ödənişinin hansı müqaviləyə aid olduğunu müəyyənləşdirmək üçün nəzərdə tutulub.");
            }
        }

        void RefreshFundsPayment(decimal a, int p)
        {
            AmountValue.Value = a;
            funds_payment_id = p.ToString();
        }

        private void LoadFPaymentAmountAddEdit(string transaction)
        {
            double current_debt;
            int pay_count = 0, interest = 0, payment_temp_count = 0;
            string lastdate = null, contract_number = null, start_date = null, currency = null, s = null;

            s = $@"SELECT FC.ID,FC.CONTRACT_NUMBER,FC.INTEREST,TO_CHAR(FC.START_DATE,'DD/MM/YYYY') START_DATE,C.CODE FROM CRS_USER.FUNDS_CONTRACTS FC,CRS_USER.CURRENCY C WHERE FC.CURRENCY_ID = C.ID AND FC.ID = (SELECT FUNDS_CONTRACT_ID FROM CRS_USER.BANK_CONTRACTS WHERE BANK_ID = {BankID})";
            DataTable dt = GlobalFunctions.GenerateDataTable(s);

            if (dt.Rows.Count > 0)
            {
                contract_number = dt.Rows[0]["CONTRACT_NUMBER"].ToString();
                interest = Convert.ToInt32(dt.Rows[0]["INTEREST"].ToString());
                start_date = dt.Rows[0]["START_DATE"].ToString();
                currency = dt.Rows[0]["CODE"].ToString();
            }

            if (fund_contract_id > 0)
            {
                payment_temp_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id);
                if (payment_temp_count == 0)
                    GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                        "INSERT INTO CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP(ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE,USED_USER_ID,MANUAL_INTEREST)SELECT ID,CONTRACT_ID,PAYMENT_DATE,BUY_AMOUNT,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,NOTE," + GlobalVariables.V_UserID + ",MANUAL_INTEREST FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = " + fund_contract_id,
                                                        "Ödənişlər temp cədvələ daxil edilmədi.");

                pay_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE IS_CHANGE IN (0,1) AND CONTRACT_ID = " + fund_contract_id);
                if (pay_count == 0)
                {
                    lastdate = start_date;
                    current_debt = 0;
                }
                else
                {
                    lastdate = GlobalFunctions.GetMaxDate("SELECT NVL(MAX(CP.PAYMENT_DATE),TO_DATE('" + DateText.Text.Trim() + "','DD/MM/YYYY')) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND USED_USER_ID = " + GlobalVariables.V_UserID).ToString("d", GlobalVariables.V_CultureInfoAZ);
                    current_debt = GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.CONTRACT_ID = " + fund_contract_id + " AND CP.PAYMENT_DATE = TO_DATE('" + lastdate + "','DD/MM/YYYY') AND USED_USER_ID = " + GlobalVariables.V_UserID);
                }

                if (GlobalFunctions.ChangeStringToDate(lastdate, "ddmmyyyy") <= GlobalFunctions.ChangeStringToDate(DateText.Text.Trim(), "ddmmyyyy"))
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
                    fpa.SourceName = "Bank: " + BankNameText.Text;
                    fpa.PayDate = DateText.Text.Trim();
                    fpa.Currency = currency;
                    fpa.RefreshPaymentsDataGridView += new AttractedFunds.FPaymentAmountAddEdit.DoEvent(RefreshFundsPayment);
                    fpa.ShowDialog();
                }
                else
                    XtraMessageBox.Show("Cəlb olunmuş vəsaitlərdə ən son daxil olma tarixi " + lastdate + " olduğundan " + DateText.Text.Trim() + " tarixi üçün ödəniş etmək olmaz.");
            }
            else
                XtraMessageBox.Show("Seçilmiş bank, filial və hesaba görə heç bir müqavilə olmadığı üçün məxaric etmək olmaz.");
        }

        private void BContract_Click(object sender, EventArgs e)
        {
            if (appointment_id == 3)
                LoadFPaymentBankAddEdit();
            else
                LoadFPaymentAmountAddEdit("INSERT");
        }

        private void ContractGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ContractGridView.GetFocusedDataRow();
            if (row != null)
                fund_contract_id = Convert.ToInt32(row["ID"].ToString());
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractsGridView();
        }

        void RefreshConracts()
        {
            LoadContractsGridView();
        }

        private void ContractsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Forms.AttractedFunds.FFundContract ffc = new AttractedFunds.FFundContract();
            ffc.RefreshFundsDataGridView += new Forms.AttractedFunds.FFundContract.DoEvent(RefreshConracts);
            ffc.ShowDialog();
        }

        private void ContractGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractGridView, PopupMenu, e);
        }
    }
}