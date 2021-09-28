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

namespace CRS.Forms.Total
{
    public partial class FCashToBank : DevExpress.XtraEditors.XtraForm
    {
        public FCashToBank()
        {
            InitializeComponent();
        }
        public string ContractCode, CustomerName, PaymentID;
        int bank_id, currency_id;
        string contractid;
        double basic_amount, payment_interest_amount, payment_value_AZN = 0;

        public delegate void DoEvent(decimal a, string p);
        public event DoEvent RefreshPaymentsDataGridView;

        private void FCashToBank_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            CustomerNameText.Text = CustomerName;
            RefreshDictionaries(11);
            LoadPaymentDetails();
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);

            GlobalProcedures.FillLookUpEdit(OperationAccountLookUp, "ACCOUNTING_PLAN", "ID", "SUB_ACCOUNT", "BANK_ID = " + bank_id + " ORDER BY  ID,ACCOUNT_NUMBER");
            OperationAccountLookUp.ItemIndex = 0;
        }

        void RefreshOperationsAccount()
        {
            GlobalProcedures.FillLookUpEdit(OperationAccountLookUp, "ACCOUNTING_PLAN", "ID", "SUB_ACCOUNT", "BANK_ID = " + bank_id + " ORDER BY  ID,ACCOUNT_NUMBER");
        }

        private void OperationAccountLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Bookkeeping.FAccountingPlan fap = new Bookkeeping.FAccountingPlan();
                fap.RefreshAccountList += new Bookkeeping.FAccountingPlan.DoEvent(RefreshOperationsAccount);
                fap.ShowDialog();
            }
        }

        private void LoadPaymentDetails()
        {
            string s = $@"SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD.MM.YYYY') P_DATE,
                               C.CURRENCY_ID,
                               CUR.CODE,
                               CP.CURRENCY_RATE,
                               CP.PAYMENT_AMOUNT,
                               CP.PAYMENT_AMOUNT_AZN,
                               C.ID,
                               CP.BASIC_AMOUNT,
                               CP.PAYMENT_INTEREST_AMOUNT,
                               TO_CHAR (CP.OPERATION_DATE, 'DD.MM.YYYY') OPERATION_DATE
                          FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP,
                               CRS_USER.CURRENCY CUR,
                               CRS_USER.CONTRACTS C
                         WHERE     C.ID = CP.CONTRACT_ID       
                               AND C.CURRENCY_ID = CUR.ID
                               AND CP.ID = {PaymentID}";
            try
            {
                
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    PaymentDateText.Text = dr[0].ToString();
                    currency_id = Convert.ToInt32(dr[1].ToString());
                    if (currency_id != 1)
                    {
                        CurrencyRateLabel.Visible = CurrencyRateValue.Visible = RateAZNLabel.Visible = true;
                        CurrencyRateLabel.Text = "1 " + dr[2] + " = ";
                        CurrencyRateValue.Value = Convert.ToDecimal(dr[3].ToString());
                    }
                    else                    
                        PaymentAZNValue.Visible = PaymentAZNLabel.Visible = AZNLabel.Visible = false;
                    PaymentValue.Value = Convert.ToDecimal(dr[4].ToString());
                    PaymentAZNValue.Value = Convert.ToDecimal(dr[5].ToString());
                    CurrencyLabel.Text = dr[2].ToString();

                    contractid = dr[6].ToString();
                    basic_amount = Convert.ToDouble(dr[7].ToString());
                    payment_interest_amount = Convert.ToDouble(dr[8].ToString());
                    OperationDateText.Text = dr["OPERATION_DATE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            BOK.Enabled = false;
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP SET IS_CHANGE = 1,BANK_CASH = 'B',BANK_ID = {bank_id} WHERE BANK_CASH = 'C' AND PAYMENT_DATE = TO_DATE('{PaymentDateText.Text}','DD/MM/YYYY') AND ID = {PaymentID} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Ödənişin temp cədvəlində kassa bank ilə əvəz olunmadı.");
                
            if (currency_id == 1)
                payment_value_AZN = (double)PaymentValue.Value;
            else
                payment_value_AZN = (double)PaymentAZNValue.Value;

            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.BANK_OPERATIONS_TEMP(ID,
                                                                                            BANK_ID,
                                                                                            OPERATION_DATE,
                                                                                            APPOINTMENT_ID,
                                                                                            INCOME,
                                                                                            EXPENSES,
                                                                                            DEBT,
                                                                                            IS_CHANGE,
                                                                                            USED_USER_ID,
                                                                                            CONTRACT_PAYMENT_ID,
                                                                                            CONTRACT_CODE) 
                                            VALUES(CRS_USER.BANK_OPERATION_SEQUENCE.NEXTVAL,
                                                    {bank_id},
                                                    TO_DATE('{PaymentDateText.Text.Trim()}','DD/MM/YYYY'),
                                                    3,
                                                    {Math.Round(payment_value_AZN, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                    0,
                                                    0,
                                                    1,
                                                    {GlobalVariables.V_UserID},
                                                    {PaymentID},
                                                    '{ContractCodeText.Text.Trim()}')",
                                          "Linq ödənişi bank əməliyyatlarının mədaxilinə daxil olunmadı.");

            GlobalProcedures.InsertOperationJournal(PaymentDateText.Text, (double)CurrencyRateValue.Value, currency_id, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, basic_amount, payment_interest_amount, contractid, PaymentID, OperationAccountLookUp.Text, 1, OperationDateText.Text);
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE DESTINATION_ID = 1 AND OPERATION_OWNER_ID = {PaymentID}", "Ödəniş kassa əməliyyatlarından silinmədi.");
            GlobalProcedures.UpdateCashDebt(PaymentDateText.Text.Trim());
            this.RefreshPaymentsDataGridView(PaymentValue.Value, PaymentID);
            this.Close();
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }
        
    }
}