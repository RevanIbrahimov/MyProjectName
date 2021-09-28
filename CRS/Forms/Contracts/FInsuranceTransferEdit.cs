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

namespace CRS.Forms.Contracts
{
    public partial class FInsuranceTransferEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceTransferEdit()
        {
            InitializeComponent();
        }
        public string ContractCode, TransferDate, Note;
        public int TransferID, InsuranceID;
        public decimal TransferAmount, InsuranceCost, Debt, Compensation;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FInsuranceTransferEdit_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            TransferDateText.Text = TransferDate;
            TransferAmountValue.EditValue = TransferAmount;
            CompensationValue.EditValue = Compensation;
            NoteText.Text = Note;
            InsuranceCostValue.EditValue = InsuranceCost;
        }
        
        private void FInsuranceTransferEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void Update()
        {
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
                    command.CommandText = "CRS_USER.PROC_UPDATE_INSURANCE_TRANSFER";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_ID", GlobalFunctions.ConvertObjectToOracleDBType(TransferID)).Value = TransferID;
                    command.Parameters.Add("P_AMOUNT", GlobalFunctions.ConvertObjectToOracleDBType(TransferAmountValue.Value)).Value = TransferAmountValue.Value;
                    command.Parameters.Add("P_NOTE", GlobalFunctions.ConvertObjectToOracleDBType(NoteText.Text)).Value = NoteText.Text.Trim();
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Sığortanın köçürməsi dəyişilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetails())
            {
                Update();
                this.Close();
            }
        }

        private bool ControlDetails()
        {
            bool b = false;           

            if (TransferAmountValue.Value <= 0)
            {
                TransferAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                TransferAmountValue.Focus();
                TransferAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransferAmountValue.Value > Debt)
            {
                TransferAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Köçürüləcək məbləğ maksimum " + Debt + " AZN olmalıdır.");
                TransferAmountValue.Focus();
                TransferAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

    }
}