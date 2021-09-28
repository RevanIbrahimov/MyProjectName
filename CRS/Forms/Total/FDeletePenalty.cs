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
    public partial class FDeletePenalty : DevExpress.XtraEditors.XtraForm
    {
        public FDeletePenalty()
        {
            InitializeComponent();
        }
        public string TransactionName, CustomerID, ContractID, PenaltyID, Currency;
        public decimal Debt;

        public delegate void DoEvent();
        public event DoEvent RefreshPenaltyDataGridView;

        private void FDeletePenalty_Load(object sender, EventArgs e)
        {
            GlobalProcedures.CalcEditFormat(DebtPanalty);            
            GlobalProcedures.CalcEditFormat(CurrentDebtPenalty);
            GlobalProcedures.CalcEditFormat(DeletedPenalty);
            Currency1Label.Text = Currency;
            Currency2Label.Text = Currency;
            Currency2Label.Text = Currency;

            if (TransactionName == "INSERT")
            {
                DebtPanalty.Value = Debt;
                DateText.Text = DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ);
                DeletedPenalty.Value = Debt;
                DeletedPenalty_EditValueChanged(sender, EventArgs.Empty);
            }
            else
            {
                DebtPanalty.Value = (decimal)GlobalFunctions.GetAmount("SELECT DEBT_PENALTY FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP B WHERE B.CUSTOMER_ID = " + CustomerID + " AND B.CONTRACT_ID = " + ContractID + " AND ID = (SELECT MAX(ID) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE CUSTOMER_ID = B.CUSTOMER_ID AND CONTRACT_ID = B.CONTRACT_ID AND ID < " + PenaltyID + ")");
                LoadPenaltyDetails();
            }
        }

        private void LoadPenaltyDetails()
        {
            string s = $@"SELECT TO_CHAR(BAL_DATE,'DD.MM.YYYY') BAL_DATE,PAYMENT_PENALTY FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE ID =  {PenaltyID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPenaltyDetails", "Silinən cərimə tapılmadı.");
            if(dt.Rows.Count > 0)
            {
                DateText.Text = dt.Rows[0]["BAL_DATE"].ToString();
                DeletedPenalty.Value = Convert.ToDecimal(dt.Rows[0]["PAYMENT_PENALTY"].ToString());
            }            
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeletedPenalty_EditValueChanged(object sender, EventArgs e)
        {
            CurrentDebtPenalty.Value = DebtPanalty.Value - DeletedPenalty.Value;
        }

        private void FDeletePenalty_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPenaltyDataGridView();
        }

        private bool ControlPenaltyDetails()
        {
            bool b = false;

            if (CurrentDebtPenalty.Value < 0)
            {
                DeletedPenalty.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Silinən cərimə qalıq cərimə faizindən böyük ola bilməz.");
                DeletedPenalty.Focus();
                DeletedPenalty.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE PENALTY_STATUS = 'Silinib' AND IS_CHANGE IN (0,1) AND BAL_DATE = TO_DATE('{DateText.Text}','DD/MM/YYYY')", this.Name + "/ControlPenaltyDetails") > 0)
            {
                XtraMessageBox.Show(DateText.Text + " tarixi üçün artıq cərimə müştərinin balansından silinib.");
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertPenalty()
        {
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.BALANCE_PENALTIES_TEMP(ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_CHANGE,PENALTY_STATUS,USED_USER_ID) VALUES(BALANCE_PENALTIES_SEQUENCE.NEXTVAL," + CustomerID + "," + ContractID + ",TO_DATE('" + DateText.Text + "','DD/MM/YYYY'),0,0," + CurrentDebtPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + DeletedPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",1,'Silinib'," + GlobalVariables.V_UserID + ")",
                                                "Cərimə balansdan silinmədi.",
                                                this.Name + "/InsertPenalty");
        }

        private void UpdatePenalty()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET IS_CHANGE = 1, DEBT_PENALTY = " + CurrentDebtPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + ", PAYMENT_PENALTY = " + DeletedPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + PenaltyID + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                "Silinən cərimə dəyişdirilmədi.",
                                                this.Name + "/UpdatePenalty");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlPenaltyDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPenalty();
                else
                    UpdatePenalty();
                this.Close();
            }
        }
    }
}