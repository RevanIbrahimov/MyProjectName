using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Bookkeeping
{
    public partial class FAccountDebtAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAccountDebtAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, DebtID;
        bool CurrentStatus = false, DebtUsed = false, FormStatus = false;
        int UsedUserID = -1;
        List<OperationDebt> lstOperationDebt = null;

        public delegate void DoEvent();
        public event DoEvent RefreshDebtsDataGridView;

        private void FAccountDebtAddEdit_Load(object sender, EventArgs e)
        {  
            GlobalProcedures.DateEditFormat(OperationDate);
            OperationDate.EditValue = DateTime.Today;
            OperationDate.Properties.MinValue = Class.GlobalFunctions.ChangeStringToDate("31.12.2015","ddmmyyyy");
            GlobalProcedures.CalcEditFormat(DebitAmountValue);
            GlobalProcedures.CalcEditFormat(CreditAmountValue);
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OPERATION_DEBT", Class.GlobalVariables.V_UserID, "WHERE ID = " + DebtID + " AND USED_USER_ID = -1");
                lstOperationDebt = OperationDebtDAL.SelectOperationDebtByID(int.Parse(DebtID)).ToList<OperationDebt>();
                var operation = lstOperationDebt.First();
                if (operation.USED_USER_ID > 0)
                {
                    UsedUserID = operation.USED_USER_ID;
                    DebtUsed = true;
                }
                else
                    DebtUsed = false;

                if (DebtUsed)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = Class.GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş qalıq hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş qalığın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadDebtDetails();
            }
        }

        private void LoadDebtDetails()
        {
            string s = $@"SELECT TO_CHAR(DEBT_DATE,'DD/MM/YYYY') DEBT_DATE,ACCOUNT,CURRENT_DEBIT,CURRENT_CREDIT,NOTE FROM CRS_USER.OPERATION_DEBT WHERE ID = {DebtID}";
            try
            {
                
               
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s).Rows)
                {
                    FormStatus = true;
                    OperationDate.EditValue = GlobalFunctions.ChangeStringToDate(dr["DEBT_DATE"].ToString(), "ddmmyyyy");
                    AccountComboBox.EditValue = dr["ACCOUNT"].ToString();
                    DebitAmountValue.Value = Convert.ToDecimal(dr["CURRENT_DEBIT"].ToString());
                    CreditAmountValue.Value = Convert.ToDecimal(dr["CURRENT_CREDIT"].ToString());
                    NoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Qalıq tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void FAccountDebtAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OPERATION_DEBT", -1, "WHERE ID = " + DebtID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDebtsDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComponentEnabled(bool status)
        {
            AccountComboBox.Enabled = 
                OperationDate.Enabled = 
                DebitAmountValue.Enabled = 
                CreditAmountValue.Enabled = 
                BOK.Visible = !status;
        }

        private bool ControlDebtDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(OperationDate.Text))
            {
                OperationDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                OperationDate.Focus();
                OperationDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(AccountComboBox.Text))
            {
                AccountComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hesab daxil edilməyib.");
                AccountComboBox.Focus();
                AccountComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.OPERATION_DEBT WHERE DEBT_DATE = TO_DATE('" + OperationDate.Text + "','DD/MM/YYYY') AND ACCOUNT = '" + AccountComboBox.Text.Trim() + "'") > 0)
            {
                AccountComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(AccountComboBox.Text + " hesabının " + OperationDate.Text + " tarixinə qalığı artıq daxil edilib.");
                AccountComboBox.Focus();
                AccountComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.OPERATION_DEBT WHERE ACCOUNT = '" + AccountComboBox.Text.Trim() + "' AND DEBT_DATE < TO_DATE('" + OperationDate.Text + "','DD/MM/YYYY')");

            if (TransactionName == "INSERT" && a > 0)
            {
                XtraMessageBox.Show(AccountComboBox.Text.Trim() + " hesabının " + OperationDate.Text + " tarixindən kiçik tarixlər üçün qalıqları olduğundan bu hesabın " + OperationDate.Text + " tarixi üçün yeni qalığını daxil etmək olmaz.");
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertDebt()
        {
            decimal first_debit = 0, first_credit = 0, debt_debit = 0, debt_credit = 0;
            string account = AccountComboBox.Text.Trim();
            int parentid = 0;
            lstOperationDebt = OperationDebtDAL.SelectOperationDebtByAccount(account).ToList<OperationDebt>();
            var lastDebt = lstOperationDebt.LastOrDefault(d => d.DEBT_DATE < GlobalFunctions.ChangeStringToDate(OperationDate.Text, "ddmmyyyy"));

            if(lastDebt != null)
            {
                parentid = lastDebt.ID;
                first_debit = lastDebt.DEBIT;
                first_credit = lastDebt.CREDIT;
            }
            
            debt_credit = first_credit + CreditAmountValue.Value - DebitAmountValue.Value;
            debt_debit = first_debit + DebitAmountValue.Value - CreditAmountValue.Value;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.OPERATION_DEBT(                                                                                        
                                                                                        DEBT_DATE,
                                                                                        ACCOUNT,
                                                                                        DEBIT,
                                                                                        CREDIT,
                                                                                        CURRENT_DEBIT,
                                                                                        CURRENT_CREDIT,
                                                                                        PARENT_ID,
                                                                                        IS_MANUAL,
                                                                                        YR_MNTH_DY,
                                                                                        NOTE
                                                                                 ) 
                                                    VALUES(
                                                                TO_DATE('{OperationDate.Text}','DD/MM/YYYY'),
                                                                '{AccountComboBox.Text.Trim()}',
                                                                {debt_debit.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {debt_credit.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {DebitAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {CreditAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                {parentid},0,
                                                                {GlobalFunctions.ConvertDateToInteger(OperationDate.Text, "ddmmyyyy")},
                                                                '{NoteText.Text.Trim()}'
                                                            )",
                                                "Hesabın qalığı daxil edilmədi.",
                                                this.Name + "/InsertDebt");

            //GlobalProcedures.CalculatedOperationDebt(account, OperationDate.Text);
        }

        private void UpdateDebt()
        {
            decimal first_debit = 0, first_credit = 0, debt_debit = 0, debt_credit = 0;
            string account = AccountComboBox.Text.Trim();

            lstOperationDebt = OperationDebtDAL.SelectOperationDebtByAccount(account).ToList<OperationDebt>();
            var lastDebt = lstOperationDebt.LastOrDefault(d => d.DEBT_DATE < GlobalFunctions.ChangeStringToDate(OperationDate.Text, "ddmmyyyy"));

            if (lastDebt != null)
            {                
                first_debit = lastDebt.DEBIT;
                first_credit = lastDebt.CREDIT;
            }

            debt_credit = first_credit + CreditAmountValue.Value - DebitAmountValue.Value;
            debt_debit = first_debit + DebitAmountValue.Value - CreditAmountValue.Value;
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.OPERATION_DEBT SET 
                                                                DEBT_DATE = TO_DATE('{OperationDate.Text}','DD/MM/YYYY'),
                                                                ACCOUNT = '{account}',
                                                                DEBIT = {debt_debit.ToString(Class.GlobalVariables.V_CultureInfoEN)},
                                                                CREDIT = {debt_credit.ToString(Class.GlobalVariables.V_CultureInfoEN)},
                                                                CURRENT_DEBIT = {DebitAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN)},
                                                                CURRENT_CREDIT = {CreditAmountValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN)},
                                                                YR_MNTH_DY = {GlobalFunctions.ConvertDateToInteger(OperationDate.Text, "ddmmyyyy")},
                                                                NOTE = '{NoteText.Text.Trim()}' 
                                                    WHERE ID = {DebtID}",
                                                   "Hesabın qalığı dəyişdirilmədi.", this.Name + "/UpdateDebt");
            //GlobalProcedures.CalculatedOperationDebt(account, OperationDate.Text);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDebtDetails())
            {
                if (TransactionName == "INSERT")
                    InsertDebt();
                else
                    UpdateDebt();
                this.Close();
            }
        }

        private void OperationDate_EditValueChanged(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT" && FormStatus)
                GlobalProcedures.FillComboBoxEditWithSqlText(AccountComboBox, "SELECT NVL(AP.SUB_ACCOUNT,AP.ACCOUNT_NUMBER) ACCOUNT_NAME " +
                                                                                                                        "FROM CRS_USER.ACCOUNTING_PLAN AP WHERE AP.ACCOUNT_NUMBER NOT IN (174) " +
                                                                                                                    "UNION ALL " +
                                                                                                                    "SELECT 174||LEASING_ACCOUNT ACCOUNT_NAME " +
                                                                                                                        "FROM CRS_USER.CONTRACTS " +
                                                                                                                    "WHERE (ID,CUSTOMER_ID) IN (SELECT CONTRACT_ID, CUSTOMER_ID " +
                                                                                                                                                    "FROM CRS_USER.CUSTOMER_PAYMENTS " +
                                                                                                                                                "WHERE DEBT > 0 AND PAYMENT_DATE BETWEEN TO_DATE('12/31/2015','MM/DD/YYYY') AND TO_DATE ('" + OperationDate.Text + "','DD/MM/YYYY')) " +
                                                                                            "ORDER BY ACCOUNT_NAME");
            else if (TransactionName == "INSERT")
                GlobalProcedures.FillComboBoxEditWithSqlText(AccountComboBox, "SELECT NVL(AP.SUB_ACCOUNT,AP.ACCOUNT_NUMBER) ACCOUNT_NAME " +
                                                                                                                        "FROM CRS_USER.ACCOUNTING_PLAN AP WHERE AP.ACCOUNT_NUMBER NOT IN (174) " +
                                                                                                                    "UNION ALL " +
                                                                                                                    "SELECT 174||LEASING_ACCOUNT ACCOUNT_NAME " +
                                                                                                                        "FROM CRS_USER.CONTRACTS " +
                                                                                                                    "WHERE (ID,CUSTOMER_ID) IN (SELECT CONTRACT_ID, CUSTOMER_ID " +
                                                                                                                                                    "FROM CRS_USER.CUSTOMER_PAYMENTS " +
                                                                                                                                                "WHERE DEBT > 0 AND PAYMENT_DATE BETWEEN TO_DATE('12/31/2015','MM/DD/YYYY') AND TO_DATE ('" + OperationDate.Text + "','DD/MM/YYYY')) " +
                                                                                            "ORDER BY ACCOUNT_NAME");
        }
    }
}