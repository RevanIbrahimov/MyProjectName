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

namespace CRS.Forms.Contracts
{
    public partial class FInterestAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInterestAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID, InterestID, StartDate;
        public int IsTemp = 1;

        public delegate void DoEvent(double value, double interest);
        public event DoEvent RefreshInterestDataGridView;

        private void FInterestAddEdit_Load(object sender, EventArgs e)
        {            
            CalcDate.EditValue = GlobalFunctions.ChangeStringToDate(StartDate, "ddmmyyyy");
            CalcDate.Properties.MinValue = GlobalFunctions.ChangeStringToDate(StartDate, "ddmmyyyy");                 
            if (TransactionName == "EDIT")
                LoadInterestDetails();
        }

        private void LoadInterestDetails()
        {
            string s = null;
            if(IsTemp == 1)
                s = $@"SELECT CALC_DATE,INTEREST,NOTE FROM CRS_USER_TEMP.INTEREST_PENALTIES_TEMP WHERE ID = {InterestID}";
            else
                s = $@"SELECT CALC_DATE,INTEREST,NOTE FROM CRS_USER.CONTRACT_INTEREST_PENALTIES WHERE ID = {InterestID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInterestDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    CalcDate.EditValue = DateTime.Parse(dr[0].ToString());                    
                    InterestValue.Value = Convert.ToDecimal(dr[1].ToString());
                    NoteText.Text = dr[2].ToString();                                      
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Cərimə faizi açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FInterestAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshInterestDataGridView(100, (double)InterestValue.Value);
        }

        private bool ControlInterestDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(CalcDate.Text))
            {
                CalcDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");                
                CalcDate.Focus();
                CalcDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.INTEREST_PENALTIES_TEMP WHERE CONTRACT_ID = {ContractID} AND CALC_DATE = TO_DATE('{CalcDate.Text}','DD/MM/YYYY')") > 0)
            {
                CalcDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(CalcDate.Text + " tarixi üçün artıq cərimə faizi daxil edilib.");                
                CalcDate.Focus();
                CalcDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (InterestValue.Value < 0)
            {
                InterestValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cərimə faiz dərəcəsi mənfi ədəd ola bilməz.");                
                InterestValue.Focus();
                InterestValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertInterest()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.INTEREST_PENALTIES_TEMP(CONTRACT_ID,CALC_DATE,INTEREST,NOTE,IS_DEFAULT,IS_CHANGE,USED_USER_ID)VALUES(" + ContractID + ",TO_DATE('" + CalcDate.Text + "','DD/MM/YYYY')," + InterestValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + NoteText.Text.Trim() + "',0,1," + GlobalVariables.V_UserID + ")",
                                                "Cərimə faiz daxil edilmədi.",
                                                this.Name + "/InsertInterest");
        }

        private void UpdateInterest()
        {
            if(IsTemp == 1)
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.INTEREST_PENALTIES_TEMP SET CALC_DATE = TO_DATE('" + CalcDate.Text + "','DD/MM/YYYY'),INTEREST = " + InterestValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "', IS_CHANGE = 1 WHERE ID = " + InterestID,
                                                "Cərimə faiz dəyişdirilmədi.",
                                                this.Name + "/UpdateInterest");
            else
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_INTEREST_PENALTIES SET CALC_DATE = TO_DATE('" + CalcDate.Text + "','DD/MM/YYYY'),INTEREST = " + InterestValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "' WHERE ID = " + InterestID,
                                                "Cərimə faiz dəyişdirilmədi.",
                                                this.Name + "/UpdateInterest");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlInterestDetails())
            {
                if (TransactionName == "INSERT")
                    InsertInterest();
                else
                    UpdateInterest();
                this.Close();
            }
        }
    }
}