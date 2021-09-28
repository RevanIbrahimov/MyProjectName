using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;
using CRS.Class.Views;
using CRS.Class.DataAccess;

namespace CRS.Forms.Total
{
    public partial class FTemporaryPaymentAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FTemporaryPaymentAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, PaymentID;

        int contractID = 0,
            paymentTypeID = 0,
            currencyID = 0,
            bankID = 0;

        public delegate void DoEvent();
        public event DoEvent RefreshPaymentsDataGridView;

        private void FTemporaryPaymentAddEdit_Load(object sender, EventArgs e)
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);
            PaymentDate.Properties.MinValue = tomorrow;
            PaymentDate.EditValue = tomorrow;
            GlobalProcedures.FillLookUpEdit(TypeLookUp, "PAYMENT_TYPE", "ID", "TYPE_NAME", "1 = 1 ORDER BY ID");
            TypeLookUp.ItemIndex = 1;
            RefreshDictionaries(11);
            BankLookUp.ItemIndex = 0;

            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACT_TOMORROW_PAYMENTS", GlobalVariables.V_UserID, "WHERE ID = " + PaymentID + " AND USED_USER_ID = -1");


                LoadPaymentDetails();
            }
        }

        private void LoadPaymentDetails()
        {
            string sql = $@"SELECT C.CONTRACT_CODE,
                                   P.PAY_DATE,
                                   P.PAYING,
                                   P.AMOUNT,
                                   P.AMOUNT_AZN,
                                   P.NOTE,
                                   PT.TYPE_NAME,
                                   P.PAYMENT_TYPE_ID,
                                   (SELECT LONG_NAME
                                      FROM CRS_USER.BANKS
                                     WHERE ID = P.BANK_ID)
                                      BANK_NAME,
                                   C.CONTRACT_ID
                              FROM CRS_USER.CONTRACT_TOMORROW_PAYMENTS P,
                                   CRS_USER.V_CONTRACTS C,
                                   CRS_USER.PAYMENT_TYPE PT
                             WHERE     P.CONTRACT_ID = C.CONTRACT_ID
                                   AND P.PAYMENT_TYPE_ID = PT.ID
                                   AND P.ID = {PaymentID}";
            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                TypeLookUp.EditValue = TypeLookUp.Properties.GetKeyValueByDisplayText(dr["TYPE_NAME"].ToString());
                BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_NAME"].ToString());
                PaymentDate.EditValue = DateTime.Parse(dr["PAY_DATE"].ToString());
                ContractCodeText.Enabled = false;
                ContractCodeText.Text = dr["CONTRACT_CODE"].ToString();
                PayingNameText.Text = dr["PAYING"].ToString();
                PaymentAZNValue.Value = Convert.ToDecimal(dr["AMOUNT_AZN"].ToString());
                PaymentUSDValue.Value = Convert.ToDecimal(dr["AMOUNT"].ToString());
                NoteText.Text = dr["NOTE"].ToString();
                contractID = Convert.ToInt32(dr["CONTRACT_ID"].ToString());
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
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

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void ContractCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
                return;

            FindContract();
        }

        private void FTemporaryPaymentAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CONTRACT_TOMORROW_PAYMENTS", -1, "WHERE ID = " + PaymentID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshPaymentsDataGridView();
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bankID = Convert.ToInt32(BankLookUp.EditValue);
        }

        private void TypeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (TypeLookUp.EditValue == null)
                return;

            paymentTypeID = Convert.ToInt32(TypeLookUp.EditValue);
            BankLookUp.Visible = BankStartLabel.Visible = BankNameLabel.Visible = (paymentTypeID == 2);
        }

        private bool ControlPaymentDetails()
        {
            bool b = false;

            if (TypeLookUp.EditValue == null)
            {
                TypeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödənişin növü daxil edilməyib.");
                TypeLookUp.Focus();
                TypeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (paymentTypeID == 2 && BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PaymentDate.Text.Length == 0)
            {
                PaymentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                PaymentDate.Focus();
                PaymentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (contractID == 0)
            {
                ContractCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müqavilənin kodu daxil edilməyib.");
                ContractCodeText.Focus();
                ContractCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PayingNameText.Text.Length == 0)
            {
                PayingNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödəyən şəxsin adı daxil edilməyib.");
                PayingNameText.Focus();
                PayingNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (currencyID == 1 && PaymentAZNValue.Value <= 0)
            {
                PaymentAZNValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                PaymentAZNValue.Focus();
                PaymentAZNValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (currencyID != 1 && PaymentAZNValue.Value <= 0 && PaymentUSDValue.Value <= 0)
            {
                PaymentAZNValue.BackColor = Color.Red;
                PaymentUSDValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğlərdən hər hansı biri sıfırdan böyük olmalıdır.");
                PaymentAZNValue.Focus();
                PaymentAZNValue.BackColor = GlobalFunctions.ElementColor();
                PaymentUSDValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (currencyID != 1 && PaymentAZNValue.Value > 0 && PaymentUSDValue.Value > 0)
            {
                PaymentAZNValue.BackColor = Color.Red;
                PaymentUSDValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məzənnə məlum olmadığından ödəniş yalnız bir valyutada qəbul olunur. Zəhmət olmasa məbləğlərdən hər hansı birini silin.");
                PaymentAZNValue.Focus();
                PaymentAZNValue.BackColor = GlobalFunctions.ElementColor();
                PaymentUSDValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertPayment()
        {
            int bank_id = (paymentTypeID == 2) ? bankID : 0;
            string sql = $@"INSERT INTO CRS_USER.CONTRACT_TOMORROW_PAYMENTS (CONTRACT_ID,
                                                                             PAYING,
                                                                             PAY_DATE,
                                                                             AMOUNT,
                                                                             AMOUNT_AZN,
                                                                             CURRENCY_ID,
                                                                             PAYMENT_TYPE_ID,
                                                                             BANK_ID,
                                                                             NOTE,
                                                                             INSERT_USER)
                                    VALUES ({contractID},
                                            '{PayingNameText.Text.Trim()}',
                                            TO_DATE('{PaymentDate.Text}','DD.MM.YYYY'),
                                            {PaymentUSDValue.Value}, 
                                            {PaymentAZNValue.Value}, 
                                            {currencyID},
                                            {paymentTypeID}, 
                                            {bank_id}, 
                                            '{NoteText.Text.Trim()}',
                                            {GlobalVariables.V_UserID})";
            GlobalProcedures.ExecuteQuery(sql, "Sabahkı ödəniş cədvələ daxil edilmədi.");
        }

        private void UpdatePayment()
        {
            int bank_id = (paymentTypeID == 2) ? bankID : 0;
            string sql = $@"UPDATE CRS_USER.CONTRACT_TOMORROW_PAYMENTS SET PAYING = '{PayingNameText.Text.Trim()}',
                                                                           PAY_DATE = TO_DATE('{PaymentDate.Text}','DD.MM.YYYY'),
                                                                           AMOUNT = {PaymentUSDValue.Value}, 
                                                                           AMOUNT_AZN = {PaymentAZNValue.Value},                                                                           
                                                                           PAYMENT_TYPE_ID = {paymentTypeID}, 
                                                                           BANK_ID = {bank_id}, 
                                                                           NOTE = '{NoteText.Text.Trim()}',
                                                                           UPDATE_USER = {GlobalVariables.V_UserID},
                                                                           UPDATE_DATE = SYSDATE
                            WHERE ID = {PaymentID}";
            GlobalProcedures.ExecuteQuery(sql, "Sabahkı ödəniş dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPaymentDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPayment();
                else
                    UpdatePayment();
                this.Close();
            }
        }

        private void FindContract()
        {
            if (ContractCodeText.Text.Length != 4)
            {
                contractID = currencyID = 0;
                PayingNameText.Text = null;
                PaymentUSDValue.Visible = USDLabel.Visible = false;
                return;
            }

            List<TomorrowPaymentDataView> lstTomorrowData = TomorrowPaymentDataViewDAL.SelectTomorrowPaymentData(ContractCodeText.Text.Trim()).ToList<TomorrowPaymentDataView>();
            if (lstTomorrowData.Count > 0)
            {
                var contract = lstTomorrowData.First();
                contractID = contract.ContractID;
                currencyID = contract.CurrencyID;
                PaymentUSDValue.Visible = USDLabel.Visible = (currencyID != 1);
                USDLabel.Text = contract.CurrencyCode;
                PayingNameText.Text = (String.IsNullOrWhiteSpace(contract.CommitmentName)) ? contract.CustomerName : contract.CommitmentName;
            }
        }
    }
}