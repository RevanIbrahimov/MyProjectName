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

namespace CRS.Forms.Dictionaries
{
    public partial class FInsuranceRateAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceRateAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, RateID;
        bool CurrentStatus = false, RateUsed = false;
        int RateUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshInsuranceRateDataGridView;

        private void FInsuranceRateAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.INSURANCE_RATE", GlobalVariables.V_UserID, "WHERE ID = " + RateID + " AND USED_USER_ID = -1");
                RateUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.INSURANCE_COMPANY WHERE ID = " + RateID);
                RateUsed = (RateUsedUserID > 0);

                if (RateUsed)
                {
                    if (GlobalVariables.V_UserID != RateUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == RateUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş sığorta dərəcəsi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş sığorta dərəcəsinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadInsuranceRateDetails();
            }
        }

        private void LoadInsuranceRateDetails()
        {
            string s = null;
            try
            {
                s = $@"SELECT RATE, UNCONDITIONAL_AMOUNT, NOTE
                                      FROM CRS_USER.INSURANCE_RATE
                                     WHERE ID = {RateID}";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceRateDetails");

                foreach (DataRow dr in dt.Rows)
                {                    
                    InsuranceInterestValue.Value = Convert.ToDecimal(dr["RATE"].ToString());
                    InsuranceUnconditionalValue.Value = Convert.ToDecimal(dr["UNCONDITIONAL_AMOUNT"].ToString());                    
                    NoteText.Text = dr["NOTE"].ToString();                    
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sığortanın dərəcəsi tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FInsuranceRateAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.INSURANCE_RATE", -1, "WHERE ID = " + RateID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshInsuranceRateDataGridView();
        }

        private void ComponentEnabled(bool status)
        {
            InsuranceInterestValue.Enabled = 
                InsuranceUnconditionalValue.Enabled = 
                NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private bool ControlInsureDetails()
        {
            bool b = false;
                                    
            if (InsuranceUnconditionalValue.Value <= 0)
            {
                InsuranceUnconditionalValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sığortanın şərtsiz azadolma məbləği daxil edilməyib.");
                InsuranceUnconditionalValue.Focus();
                InsuranceUnconditionalValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;
                        
            return b;
        }

        private void InsertRate()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.INSURANCE_RATE") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.INSURANCE_RATE(RATE,UNCONDITIONAL_AMOUNT,NOTE,ORDER_ID)VALUES({InsuranceInterestValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},{InsuranceUnconditionalValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},'{NoteText.Text.Trim()}',{order})",
                                                "Sığorta dərəcsi bazaya daxil edilmədi.",
                                          this.Name + "/InsertRate");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlInsureDetails())
            {
                if (TransactionName == "INSERT")
                    InsertRate();
                else
                    UpdateRate();
                this.Close();
            }
        }

        private void UpdateRate()
        {            
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.INSURANCE_RATE SET RATE = {InsuranceInterestValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},UNCONDITIONAL_AMOUNT = {InsuranceUnconditionalValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}' WHERE ID = {RateID}",
                                                "Sığorta dərəcsi bazada dəyişdirilmədi.",
                                          this.Name + "/UpdateRate");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}