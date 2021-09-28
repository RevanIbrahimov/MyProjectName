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
    public partial class FBalancePenaltyAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FBalancePenaltyAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CustomerID, ContractID, PenaltyID, Currency;
        public decimal Debt, CalculatedPenalty;

        bool button = false;

        public delegate void DoEvent(bool button);
        public event DoEvent RefreshPenaltyDataGridView;

        private void FBalancePenaltyAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.CalcEditFormat(DebtPanalty);
            GlobalProcedures.CalcEditFormat(CalcPenalty);
            GlobalProcedures.CalcEditFormat(CurrentDebtPenalty);
            GlobalProcedures.CalcEditFormat(DiscountPenalty);
            Currency1Label.Text = Currency;
            Currency2Label.Text = Currency;
            Currency3Label.Text = Currency;
            Currency4Label.Text = Currency;

            if (TransactionName == "INSERT")
            {
                DebtPanalty.Value = Debt;
                DateText.Text = DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ);
                DiscountPenalty.Value = 0;
                CalcPenalty.Value = CalculatedPenalty;
                DiscountPenalty_EditValueChanged(sender, EventArgs.Empty);
            }
            else
            {
                DebtPanalty.Value = (decimal)GlobalFunctions.GetAmount($@"SELECT DEBT_PENALTY FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP B WHERE B.CONTRACT_ID = {ContractID} AND ID = (SELECT MAX(ID) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE CUSTOMER_ID = B.CUSTOMER_ID AND CONTRACT_ID = B.CONTRACT_ID AND ID < {PenaltyID})", this.Name + "/FBalancePenaltyAddEdit_Load");
                LoadPenaltyDetails();
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            button = false;
            this.Close();
        }

        private void DiscountPenalty_EditValueChanged(object sender, EventArgs e)
        {
            CurrentDebtPenalty.Value = CalcPenalty.Value - DiscountPenalty.Value;
        }

        private void LoadPenaltyDetails()
        {
            string s = $@"SELECT TO_CHAR(BAL_DATE,'DD.MM.YYYY') BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE ID =  {PenaltyID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPenaltyDetails", "Cərimə tapılmadı.");
            if(dt.Rows.Count > 0)
            {
                DateText.Text = dt.Rows[0]["BAL_DATE"].ToString();
                CalcPenalty.Value = Convert.ToDecimal(dt.Rows[0]["PENALTY_AMOUNT"].ToString());
                DiscountPenalty.Value = Convert.ToDecimal(dt.Rows[0]["DISCOUNT_PENALTY"].ToString());
                CurrentDebtPenalty.Value = Convert.ToDecimal(dt.Rows[0]["DEBT_PENALTY"].ToString());
            }            
        }

        private bool ControlPenaltyDetails()
        {
            bool b = false;

            if (CurrentDebtPenalty.Value < 0)
            {
                DiscountPenalty.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Güzəşt olunan cərimə müştəridən tutulacaq cərimə faizindən böyük ola bilməz.");                            
                DiscountPenalty.Focus();
                DiscountPenalty.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;            

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE PENALTY_STATUS = 'Yeni' AND IS_CHANGE IN (0,1) AND BAL_DATE = TO_DATE('" + DateText.Text + "','DD/MM/YYYY')") > 0)
            {
                GlobalProcedures.ShowErrorMessage(DateText.Text + " tarixi üçün artıq cərimə balansa daxil edilib.");                    
                return false;
            }
            else
                b = true;

            return b;
        }

        private void FBalancePenaltyAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPenaltyDataGridView(button);
        }

        private void InsertPenalty()
        {            
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.BALANCE_PENALTIES_TEMP(ID,CUSTOMER_ID,CONTRACT_ID,BAL_DATE,PENALTY_AMOUNT,DISCOUNT_PENALTY,DEBT_PENALTY,PAYMENT_PENALTY,IS_CHANGE,PENALTY_STATUS,USED_USER_ID) VALUES(BALANCE_PENALTIES_SEQUENCE.NEXTVAL," + CustomerID + "," + ContractID + ",TO_DATE('" + DateText.Text + "','DD/MM/YYYY')," + CalcPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + DiscountPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + CurrentDebtPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",0,1,'Yeni'," + GlobalVariables.V_UserID + ")",
                                                "Cərimə daxil edilmədi.",
                                                this.Name + "/InsertPenalty");
        }

        private void UpdatePenalty()
        {            
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET IS_CHANGE = 1, DISCOUNT_PENALTY = " + DiscountPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT_PENALTY = " + CurrentDebtPenalty.Value.ToString(GlobalVariables.V_CultureInfoEN) + " WHERE ID = " + PenaltyID + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                "Cərimə dəyişdirilmədi.",
                                                this.Name + "/UpdatePenalty");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlPenaltyDetails())
            {
                button = true;
                if (TransactionName == "INSERT")
                    InsertPenalty();
                else
                    UpdatePenalty();
                this.Close();
            }
        }
    }
}