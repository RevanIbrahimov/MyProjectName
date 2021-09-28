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
    public partial class FAccountingOperationAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAccountingOperationAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, OperationID, AccountID;
        int account_id = 0;
        string debit_account, credit_account;
        bool CurrentStatus = false, OperationUsed = false, FormStatus = false;
        int UsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshOperationsDataGridView;

        private void FAccountingOperationAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                DebitAccountLookUp.Properties.Buttons[1].Visible = 
                    CreditAccountLookUp.Properties.Buttons[1].Visible = 
                    (GlobalVariables.AddAccountPlan || GlobalVariables.EditAccountPlan || GlobalVariables.DeleteAccountPlan);
            }
            
            OperationDate.EditValue = DateTime.Today;
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OPERATION_JOURNAL", GlobalVariables.V_UserID, "WHERE ID = " + OperationID + " AND USED_USER_ID = -1");
                List<OperationJournal> lstJournal = OperationJournalDAL.SelectOperationJournal(int.Parse(OperationID)).ToList<OperationJournal>();
                var journal = lstJournal.First();
                if (journal.USED_USER_ID > 0)
                {
                    UsedUserID = journal.USED_USER_ID;
                    OperationUsed = true;
                }
                else
                    OperationUsed = false;

                if (OperationUsed)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş əməliyyat hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş əməliyyatın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadOperationDetails();
            }
            else if (TransactionName == "COPY")
                LoadOperationDetails();
        }

        private void ComponentEnabled(bool status)
        {
            DebitAccountLookUp.Enabled = 
                CreditAccountLookUp.Enabled =
                OperationDate.Enabled = 
                AmountValue.Enabled = 
                AppointmentText.Enabled = 
                BOK.Visible = !status;
        }

        private void LoadOperationDetails()
        {
            string s = $@"SELECT OPERATION_DATE,(SELECT SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE SUB_ACCOUNT = DEBIT_ACCOUNT) DEBIT_ACCOUNT,(SELECT SUB_ACCOUNT||' - '||SUB_ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN WHERE SUB_ACCOUNT = CREDIT_ACCOUNT) CREDIT_ACCOUNT,AMOUNT_AZN,APPOINTMENT FROM CRS_USER.OPERATION_JOURNAL WHERE ID = {OperationID}";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperationDetails").Rows)
                {
                    FormStatus = true;
                    OperationDate.EditValue = TransactionName == "EDIT" ? dr["OPERATION_DATE"] : DateTime.Today;
                    GlobalProcedures.LookUpEditValue(DebitAccountLookUp, dr[1].ToString());
                    GlobalProcedures.LookUpEditValue(CreditAccountLookUp, dr[2].ToString());
                    AmountValue.Value = Convert.ToDecimal(dr[3].ToString());
                    AppointmentText.Text = dr[4].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Əməliyyatın rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private bool ControlOperationDetails()
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

            if (DebitAccountLookUp.Text == null)
            {
                DebitAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Debit hesabı daxil edilməyib.");
                DebitAccountLookUp.Focus();
                DebitAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CreditAccountLookUp.Text == null)
            {
                CreditAccountLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kredit hesabı daxil edilməyib.");
                CreditAccountLookUp.Focus();
                CreditAccountLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(AppointmentText.Text))
            {
                AppointmentText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təyinat daxil edilməyib.");                
                AppointmentText.Focus();
                AppointmentText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "COPY" && GlobalFunctions.GetCount($@"SELECT COUNT(*)
                                                                          FROM CRS_USER.OPERATION_JOURNAL
                                                                         WHERE     OPERATION_DATE = TO_DATE ('{OperationDate.Text}', 'DD.MM.YYYY')
                                                                               AND DEBIT_ACCOUNT = '{debit_account}'
                                                                               AND CREDIT_ACCOUNT = '{credit_account}'
                                                                               AND AMOUNT_AZN = {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)}
                                                                               AND APPOINTMENT = '{AppointmentText.Text.Trim()}'") > 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Bu əməliyyat artıq bazada mövcuddur. Təkrar bazaya daxil etmək istəyirsiniz?", "Əməliyyatın təkrarlanması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    b = true;
                else
                    return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertOperation()
        {      
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.OPERATION_JOURNAL(OPERATION_DATE,
                                                                                    DEBIT_ACCOUNT,
                                                                                    CREDIT_ACCOUNT,
                                                                                    CURRENCY_RATE,
                                                                                    AMOUNT_CUR,
                                                                                    AMOUNT_AZN,
                                                                                    APPOINTMENT,
                                                                                    ACCOUNT_OPERATION_TYPE_ID,
                                                                                    CREATED_USER_ID,
                                                                                    IS_MANUAL,
                                                                                    YR_MNTH_DY)
                                            VALUES(TO_DATE('{OperationDate.Text}','DD/MM/YYYY'),
                                                    '{debit_account}',
                                                    '{credit_account}',
                                                    1,0,
                                                    {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                    '{AppointmentText.Text.Trim()}',
                                                    3,
                                                    {GlobalVariables.V_UserID},
                                                    1,
                                                    {GlobalFunctions.ConvertDateToInteger(OperationDate.Text, "ddmmyyyy")})",
                             debit_account + " hesabının debitinə və " + credit_account + " hesabının kreditinə məbləğ daxil edilmədi.");
        }

        private void UpdateOperation()
        {   
            GlobalProcedures.ExecuteTwoQuery($@"INSERT INTO CRS_USER.OPERATION_JOURNAL_LOG (ID,
                                                                                           OPERATION_DATE,
                                                                                           CREATED_USER_ID,
                                                                                           DEBIT_ACCOUNT,
                                                                                           CREDIT_ACCOUNT,
                                                                                           CURRENCY_RATE,
                                                                                           AMOUNT_CUR,
                                                                                           AMOUNT_AZN,
                                                                                           APPOINTMENT,
                                                                                           CONTRACT_ID,
                                                                                           CUSTOMER_PAYMENT_ID,
                                                                                           ACCOUNT_OPERATION_TYPE_ID,
                                                                                           IS_MANUAL,
                                                                                           USER_ID,
                                                                                           YR_MNTH_DY,
                                                                                           LOG_TYPE)
                                                          SELECT ID,
                                                                 OPERATION_DATE,
                                                                 CREATED_USER_ID,
                                                                 DEBIT_ACCOUNT,
                                                                 CREDIT_ACCOUNT,
                                                                 CURRENCY_RATE,
                                                                 AMOUNT_CUR,
                                                                 AMOUNT_AZN,
                                                                 APPOINTMENT,
                                                                 CONTRACT_ID,
                                                                 CUSTOMER_PAYMENT_ID,
                                                                 ACCOUNT_OPERATION_TYPE_ID,
                                                                 IS_MANUAL,
                                                                 {GlobalVariables.V_UserID} USER_ID,
                                                                 YR_MNTH_DY,
                                                                 2 LOG_TYPE
                                                            FROM CRS_USER.OPERATION_JOURNAL
                                                            WHERE ID = {OperationID}",
                                            $@"UPDATE CRS_USER.OPERATION_JOURNAL SET OPERATION_DATE = TO_DATE('{OperationDate.Text}','DD/MM/YYYY'),
                                                            DEBIT_ACCOUNT = '{debit_account}',
                                                            CREDIT_ACCOUNT = '{credit_account}',
                                                            AMOUNT_AZN = {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                            APPOINTMENT = '{AppointmentText.Text.Trim()}',
                                                            CREATED_USER_ID = {GlobalVariables.V_UserID},
                                                            YR_MNTH_DY = {GlobalFunctions.ConvertDateToInteger(OperationDate.Text, "ddmmyyyy")} 
                                                WHERE ID = {OperationID}",
                             debit_account + " hesabının debitinin və " + credit_account + " hesabının kreditinin məbləği dəyişdirilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlOperationDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
                if (TransactionName == "INSERT" || TransactionName == "COPY")
                    InsertOperation();
                else
                    UpdateOperation();
                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }

        void RefreshAccount()
        {
            string sql = $@"SELECT NVL (AP.SUB_ACCOUNT, AP.ACCOUNT_NUMBER) ID,
                                          NVL (AP.SUB_ACCOUNT, AP.ACCOUNT_NUMBER)
                                       || ' - '
                                       || AP.SUB_ACCOUNT_NAME
                                          ACCOUNT_NAME
                                  FROM CRS_USER.ACCOUNTING_PLAN AP
                                 WHERE AP.ACCOUNT_NUMBER NOT IN (174,
                                                                 543,
                                                                 216,
                                                                 243)
                                UNION ALL
                                SELECT SUB_ACCOUNT ID, ACCOUNT_NAME
                                  FROM (SELECT 174 || LEASING_ACCOUNT SUB_ACCOUNT,
                                                  174
                                               || LEASING_ACCOUNT
                                               || ' - '
                                               || (SELECT DISTINCT AP.ACCOUNT_NAME
                                                     FROM CRS_USER.ACCOUNTING_PLAN AP
                                                    WHERE AP.ACCOUNT_NUMBER = 174)
                                                  ACCOUNT_NAME,
                                               ID,
                                               CUSTOMER_ID
                                          FROM CRS_USER.CONTRACTS
                                        UNION ALL
                                        SELECT 543 || CUSTOMER_ACCOUNT SUB_ACCOUNT,
                                                  543
                                               || CUSTOMER_ACCOUNT
                                               || ' - '
                                               || (SELECT DISTINCT AP.ACCOUNT_NAME
                                                     FROM CRS_USER.ACCOUNTING_PLAN AP
                                                    WHERE AP.ACCOUNT_NUMBER = 543)
                                                  ACCOUNT_NAME,
                                               ID,
                                               CUSTOMER_ID
                                          FROM CRS_USER.CONTRACTS
                                        UNION ALL
                                        SELECT 216 || LEASING_INTEREST_ACCOUNT SUB_ACCOUNT,
                                                  216
                                               || LEASING_INTEREST_ACCOUNT
                                               || ' - '
                                               || (SELECT DISTINCT AP.ACCOUNT_NAME
                                                     FROM CRS_USER.ACCOUNTING_PLAN AP
                                                    WHERE AP.ACCOUNT_NUMBER = 216)
                                                  ACCOUNT_NAME,
                                               ID,
                                               CUSTOMER_ID
                                          FROM CRS_USER.CONTRACTS
                                        UNION ALL
                                        SELECT 243 || SELLER_ACCOUNT SUB_ACCOUNT,
                                                  243
                                               || SELLER_ACCOUNT
                                               || ' - '
                                               || (SELECT DISTINCT AP.ACCOUNT_NAME
                                                     FROM CRS_USER.ACCOUNTING_PLAN AP
                                                    WHERE AP.ACCOUNT_NUMBER = 243)
                                                  ACCOUNT_NAME,
                                               ID,
                                               CUSTOMER_ID
                                          FROM CRS_USER.CONTRACTS)
                                 WHERE (ID) IN (SELECT CONTRACT_ID
                                                  FROM CRS_USER.CUSTOMER_PAYMENTS
                                                 WHERE     DEBT > 0
                                                       AND PAYMENT_DATE BETWEEN TO_DATE ('12/31/2015',
                                                                                         'MM/DD/YYYY')
                                                                            AND TO_DATE ('{OperationDate.Text}',
                                                                                         'DD/MM/YYYY'))
                                ORDER BY ACCOUNT_NAME";

            if (TransactionName == "EDIT" && FormStatus)
            {
                GlobalProcedures.FillLookUpEditWithSqlText(DebitAccountLookUp, sql, "ID", "ACCOUNT_NAME");
                GlobalProcedures.FillLookUpEditWithSqlText(CreditAccountLookUp, sql, "ID", "ACCOUNT_NAME");
            }
            else if (TransactionName == "INSERT" || TransactionName == "COPY")
            {
                if (!String.IsNullOrEmpty(AccountID))
                {
                    GlobalProcedures.FillLookUpEditWithSqlText(DebitAccountLookUp, $@"SELECT NVL (AP.SUB_ACCOUNT, AP.ACCOUNT_NUMBER) ID,
                                                                                                NVL (AP.SUB_ACCOUNT, AP.ACCOUNT_NUMBER)
                                                                                             || ' - '
                                                                                             || AP.SUB_ACCOUNT_NAME
                                                                                                ACCOUNT_NAME
                                                                                        FROM CRS_USER.ACCOUNTING_PLAN AP
                                                                                        WHERE AP.ID = {AccountID}
                                                                                    ORDER BY AP.ACCOUNT_NUMBER", "ID", "ACCOUNT_NAME");
                    
                    GlobalProcedures.FillLookUpEditWithSqlText(CreditAccountLookUp, "SELECT NVL(AP.SUB_ACCOUNT, AP.ACCOUNT_NUMBER) ID, NVL(AP.SUB_ACCOUNT, AP.ACCOUNT_NUMBER)||' - '||AP.SUB_ACCOUNT_NAME ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN AP WHERE AP.ID = " + AccountID + " ORDER BY AP.ACCOUNT_NUMBER", "ID", "ACCOUNT_NAME");
                }
                else
                {
                    GlobalProcedures.FillLookUpEditWithSqlText(DebitAccountLookUp, sql, "ID", "ACCOUNT_NAME");
                    GlobalProcedures.FillLookUpEditWithSqlText(CreditAccountLookUp, sql, "ID", "ACCOUNT_NAME");
                }
            }
        }

        private void DebitAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            debit_account = (sender as LookUpEdit).EditValue.ToString();
        }

        private void CreditAccountLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFAccountingPlan();
        }

        private void CreditAccountLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            credit_account = (sender as LookUpEdit).EditValue.ToString();
        }

        private void LoadFAccountingPlan()
        {
            FAccountingPlan fap = new FAccountingPlan();
            fap.RefreshAccountList += new FAccountingPlan.DoEvent(RefreshAccount);
            fap.ShowDialog();
        }
        
        private void DebitAccountLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadFAccountingPlan();
        }

        private void FAccountingOperationAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.OPERATION_JOURNAL", -1, "WHERE ID = " + OperationID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshOperationsDataGridView();
        }
        
        private void OperationDate_EditValueChanged(object sender, EventArgs e)
        {
            RefreshAccount();
        }
    }
}