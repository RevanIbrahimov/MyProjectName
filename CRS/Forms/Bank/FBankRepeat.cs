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
using Oracle.ManagedDataAccess.Client;
using System.Reflection;

namespace CRS.Forms.Bank
{
    public partial class FBankRepeat : DevExpress.XtraEditors.XtraForm
    {
        public FBankRepeat()
        {
            InitializeComponent();
        }
        public string BankName, PaymentDate, Appointment, OperationType, OperationID, OldBankID, ContractCode;
        public int AppointmentID;
        public decimal Amount;

        int bankID = 0, bank_id, plan_id;
        string account = null;

        public delegate void DoEvent(string operationDate);
        public event DoEvent RefreshDataGridView;

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bankID = Convert.ToInt32(BankLookUp.EditValue);

            string[] liquid_words = BankLookUp.Text.Split('/');
            account = liquid_words[1].Remove(0, 8).Trim();
            //bank_id = GlobalFunctions.FindComboBoxSelectedValue("BANKS", "LONG_NAME", "ID", liquid_words[0].Trim());
            //plan_id = GlobalFunctions.FindComboBoxSelectedValue("ACCOUNTING_PLAN", "BANK_ID = " + bank_id + " AND SUB_ACCOUNT", "ID", account);

        }

        private bool ControlDetails()
        {
            bool b = false;

            if (BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank seçilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                ProgressPanel.Show();
                Application.DoEvents();
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
                        command.CommandText = "CRS_USER.PROC_REPEAT_BANK_PAYMENT";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("P_BANK_ID", OracleDbType.Int32).Value = bankID;
                        command.Parameters.Add("P_OPERATION_ID", OracleDbType.Int32).Value = int.Parse(OperationID);
                        command.Parameters.Add("P_CONRACT_CODE", OracleDbType.Varchar2).Value = ContractCode;
                        command.Parameters.Add("P_APPOINTMENT_ID", OracleDbType.Int32).Value = AppointmentID;
                        command.Parameters.Add("P_ACCOUNT", OracleDbType.Varchar2).Value = account;
                        command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception exx)
                    {
                        GlobalProcedures.LogWrite("Alqı-satqı üzrə mühasibat əməliyyatları bazaya daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + MethodBase.GetCurrentMethod().Name, exx);
                        transaction.Rollback();
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Dispose();
                    }
                }
                GlobalProcedures.UpdateBankOperationDebtWithBank(PaymentDate, int.Parse(OldBankID));
                GlobalProcedures.UpdateBankOperationDebtWithBank(PaymentDate, bankID);
                GlobalProcedures.UpdateBankOperationDebt(PaymentDate);               
                this.RefreshDataGridView(PaymentDate);
                this.Close();
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FBankRepeat_Load(object sender, EventArgs e)
        {
            ProgressPanel.Hide();
            DateText.Text = PaymentDate;
            BankNameText.Text = BankName;
            AppointmentText.Text = Appointment;
            AmountText.Text = Amount.ToString("N2");
            AmountLabel.Text = OperationType;
            GlobalProcedures.FillLookUpEdit(BankLookUp, "V_PAYMENT_BANKS", "ID", "NAME", null);
        }
    }
}