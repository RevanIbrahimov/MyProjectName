using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS.Forms.Total
{
    public partial class FBankToCash : DevExpress.XtraEditors.XtraForm
    {
        public FBankToCash()
        {
            InitializeComponent();
        }
        public string ContractCode, CustomerName, PaymentID;
        string contractid;
        int currency_id;
        double basic_amount, payment_interest_amount;

        public delegate void DoEvent(decimal a, string p);
        public event DoEvent RefreshPaymentsDataGridView;

        private void FBankToCash_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            CustomerNameText.Text = CustomerName;
            InsertBankOperationsTemp();
            LoadPaymentDetails();
        }

        private void LoadPaymentDetails()
        {            
            string s = null;
            try
            {
                s = $@"SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD.MM.YYYY') P_DATE,
                                       C.CURRENCY_ID,
                                       CUR.CODE,
                                       CP.CURRENCY_RATE,
                                       CP.PAYMENT_AMOUNT,
                                       CP.PAYMENT_AMOUNT_AZN,
                                       C.ID,
                                       CP.BASIC_AMOUNT,
                                       CP.PAYMENT_INTEREST_AMOUNT,
                                       B.LONG_NAME BANK_NAME,
                                       BO.BANK_ID,
                                       TO_CHAR (CP.OPERATION_DATE, 'DD.MM.YYYY') OPERATION_DATE
                                  FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP,
                                       CRS_USER.CURRENCY CUR,
                                       CRS_USER.CONTRACTS C,
                                       CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO,
                                       CRS_USER.BANKS B
                                 WHERE     C.ID = CP.CONTRACT_ID
                                       AND C.CUSTOMER_ID = CP.CUSTOMER_ID
                                       AND C.CURRENCY_ID = CUR.ID
                                       AND CP.ID = BO.CONTRACT_PAYMENT_ID
                                       AND BO.BANK_ID = B.ID
                                       AND CP.ID = {PaymentID}";
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
                    //bank rekvizitleri
                    BankNameText.Text = dr["BANK_NAME"].ToString();
                    OperationDateText.Text = dr["OPERATION_DATE"].ToString();
                }                
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Lizinq ödənişinin rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void InsertBankOperationsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_BO_PAYMENT_TEMP", "P_PAYMENT_ID", PaymentID, "Ödəniş temp cədvələ daxil edilmədi.");            
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_CHANGE_PAYMENT_BANKTOCASH", "P_PAYMENT_ID", PaymentID, "Ödəniş temp cədvəldə bankdan kassaya dəyişdirilmədi.");            
            GlobalProcedures.InsertOperationJournal(PaymentDateText.Text, (double)CurrencyRateValue.Value, currency_id, (double)PaymentValue.Value, (double)PaymentAZNValue.Value, basic_amount, payment_interest_amount, contractid, PaymentID, null, 1, OperationDateText.Text);
            this.RefreshPaymentsDataGridView(PaymentValue.Value, PaymentID);
            this.Close();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}