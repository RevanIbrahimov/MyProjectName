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
using CRS.Forms.Total;

namespace CRS.Forms.Contracts
{
    public partial class FContractStatusEdit : DevExpress.XtraEditors.XtraForm
    {
        public FContractStatusEdit()
        {
            InitializeComponent();
        }
        public string ContractID, ContractCode, CustomerName, CommitmentName;
        public int CommitmentID, CommitmentPersonTypeID;

        int evaluateID = 0;

        private void FContractStatusEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshTotalsDataGridView();
        }

        private void CommitmentNameValue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                if (CommitmentPersonTypeID == 1)
                {
                    FCommitmentInfo fci = new FCommitmentInfo();
                    fci.CommitmentID = CommitmentID;
                    fci.ShowDialog();
                }
                else if (CommitmentPersonTypeID == 2)
                {
                    FJuridicalCommitmentInfo fjc = new FJuridicalCommitmentInfo();
                    fjc.CommitmentID = CommitmentID;
                    fjc.ShowDialog();
                }
            }
            else
            {
                Customer.FCommitments fc = new Customer.FCommitments();
                fc.ContractCode = ContractCodeText.Text;
                fc.ContractID = ContractID;
                fc.ShowDialog();
            }
        }

        private bool ControlDetailss()
        {
            bool b = false;

            if (StatusLookUp.EditValue == null)
            {
                StatusLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qiymətləndirmə dərəcəsi seçilməyib.");
                StatusLookUp.Focus();
                StatusLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetailss())
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET CONTRACT_EVALUATE_ID = {evaluateID} WHERE ID = {ContractID}", "Müqavilənin statusu dəyişdirilmədi.", this.Name + "/BOK_Click");
                this.Close();
            }
        }

        private void StatusLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (StatusLookUp.EditValue == null)
                return;

            evaluateID = Convert.ToInt32(StatusLookUp.EditValue);
        }

        private void StatusLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)            
                LoadDictionaries("E", 18);                
        }

        public delegate void DoEvent();
        public event DoEvent RefreshTotalsDataGridView;

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FContractStatusEdit_Load(object sender, EventArgs e)
        {
            RefreshDictionaries(18);
            ContractCodeText.Text = ContractCode;
            CustomerNameText.Text = CustomerName;
            CommitmentNameValue.Text = CommitmentName;
            CommitmentNameLabel.Visible = CommitmentNameValue.Visible = !(CommitmentID == 0);
            LoadContractEvaluate();
        }

        private void LoadContractEvaluate()
        {
            string s = $@"SELECT CONTRACT_EVALUATE_NAME FROM CRS_USER.V_CONTRACTS WHERE CONTRACT_ID = {ContractID}";
            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadContractEvaluate", "Müqavilənin qiymətləndirilməsi açılmadı.").Rows)
            {
                StatusLookUp.EditValue = StatusLookUp.Properties.GetKeyValueByDisplayText(dr["CONTRACT_EVALUATE_NAME"].ToString());
            }
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(StatusLookUp, "CONTRACT_EVALUATE", "ID", "NAME", "1=1 ORDER BY ORDER_ID");
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