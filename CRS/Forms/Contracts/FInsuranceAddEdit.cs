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
using CRS.Class.DataAccess;
using CRS.Class.Tables;

namespace CRS.Forms.Contracts
{
    public partial class FInsuranceAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID, InsuranceID, CurrencyID, CurrencyCode;
        public decimal CarAmount;
        public int IsAgain = 0;
        public DateTime ContractStartDate;
        int company_id;
        decimal insurancerate = 0;

        public delegate void DoEvent();
        public event DoEvent RefreshCompanyDataGridView;

        private void FInsuranceAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                RateLookUp.Properties.Buttons[1].Visible =
                    CompanyLookUp.Properties.Buttons[1].Visible = GlobalVariables.Insurance;
            }

            GlobalProcedures.FillLookUpEdit(RateLookUp, "INSURANCE_RATE", "ID", "RATE", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillLookUpEdit(CompanyLookUp, "INSURANCE_COMPANY", "ID", "NAME", null);
            CompanyLookUp.ItemIndex = 0;

            //CompanyComboBox.SelectedIndex = 0;
            InsuranceAmountCurrencyLabel.Text =
                InsuranceUnconditionalCurrencyLabel.Text =
                InsuranceCarAmountCurrencyLabel.Text = CurrencyCode;


            InsuranceStartDate.Properties.MinValue = ContractStartDate;

            if (TransactionName == "EDIT")
                LoadInsuranceDetails();
            else
            {
                InsuranceStartDate.EditValue = DateTime.Today;
                InsuranceCarAmountValue.EditValue = CarAmount;
            }
        }

        private void LoadInsuranceDetails()
        {
            string s = null;
            try
            {
                s = $@"SELECT C.CAR_AMOUNT,
                               C.INSURANCE_AMOUNT,
                               C.INSURANCE_PERIOD,
                               C.INSURANCE_INTEREST,
                               C.UNCONDITIONAL_AMOUNT,
                               C.START_DATE,
                               C.END_DATE,
                               IC.NAME COMPANY_NAME,
                               C.POLICE,
                               C.NOTE,
                               C.AMOUNT,
                               C.CHECK_AMOUNT,
                               NVL (IP.PAYED_AMOUNT, 0) PAYED_AMOUNT
                          FROM CRS_USER_TEMP.INSURANCES_TEMP C,
                               CRS_USER.INSURANCE_COMPANY IC,
                               CRS_USER.V_SUM_INSURANCE_PAYMENT IP
                         WHERE IC.ID = C.COMPANY_ID AND C.ID = IP.INSURANCE_ID(+) AND C.ID = {InsuranceID}";
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    InsuranceCarAmountValue.Value = Convert.ToDecimal(dr["CAR_AMOUNT"].ToString());
                    InsuranceAmountValue.Value = Convert.ToDecimal(dr["INSURANCE_AMOUNT"]);
                    InsurancePeriodValue.Value = Convert.ToInt32(dr["INSURANCE_PERIOD"]);
                    RateLookUp.EditValue = RateLookUp.Properties.GetKeyValueByDisplayText(dr["INSURANCE_INTEREST"].ToString());
                    InsuranceUnconditionalValue.Value = Convert.ToDecimal(dr["UNCONDITIONAL_AMOUNT"].ToString());
                    InsuranceStartDate.EditValue = DateTime.Parse(dr["START_DATE"].ToString());
                    InsuranceEndDate.EditValue = DateTime.Parse(dr["END_DATE"].ToString());
                    CompanyLookUp.EditValue = CompanyLookUp.Properties.GetKeyValueByDisplayText(dr["COMPANY_NAME"].ToString());
                    AmountCheck.ReadOnly = Convert.ToDecimal(dr["PAYED_AMOUNT"]) > 0;
                    InsuranceCostValue.EditValue = Convert.ToDecimal(dr["AMOUNT"]);
                    AmountCheck.Checked = Convert.ToInt32(dr["CHECK_AMOUNT"]) == 1;
                    PoliceText.Text = dr["POLICE"].ToString();
                    NoteText.Text = dr["NOTE"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sığortanın məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FInsuranceAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshCompanyDataGridView();
        }

        private void InsuranceStartDate_EditValueChanged(object sender, EventArgs e)
        {
            InsuranceEndDate.EditValue = GlobalFunctions.ChangeStringToDate(InsuranceStartDate.Text, "ddmmyyyy").AddMonths((int)InsurancePeriodValue.Value);
        }

        private bool ControlInsureDetails()
        {
            bool b = false;

            if (CompanyLookUp.EditValue == null)
            {
                CompanyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sığorta şirkəti daxil edilməyib.");
                CompanyLookUp.Focus();
                CompanyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (InsuranceAmountValue.Value <= 0)
            {
                InsuranceAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sığorta məbləği daxil edilməyib.");
                InsuranceAmountValue.Focus();
                InsuranceAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (InsuranceAmountValue.Value > InsuranceCarAmountValue.Value)
            {
                InsuranceAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sığorta məbləği avtomobilin ilkin dəyərindən böyük ola bilməz.");
                InsuranceAmountValue.Focus();
                InsuranceAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (RateLookUp.EditValue == null)
            {
                RateLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sığorta dərəcəsi daxil edilməyib.");
                RateLookUp.Focus();
                RateLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

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

            if (InsuranceUnconditionalValue.Value > InsuranceAmountValue.Value)
            {
                InsuranceUnconditionalValue.BackColor = Color.Red;
                InsuranceAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sığortanın şərtsiz azadolma məbləği Sığorta dəyərindən böyük ola bilməz.");
                InsuranceUnconditionalValue.Focus();
                InsuranceUnconditionalValue.BackColor = GlobalFunctions.ElementColor();
                InsuranceAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertInsurance()
        {
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.INSURANCES_TEMP(CONTRACT_ID,CAR_AMOUNT,INSURANCE_AMOUNT,CURRENCY_ID,INSURANCE_PERIOD,INSURANCE_INTEREST,UNCONDITIONAL_AMOUNT,START_DATE,END_DATE,COMPANY_ID,POLICE,IS_CHANGE,USED_USER_ID,IS_AGAIN,NOTE,AMOUNT,CHECK_AMOUNT)VALUES(" + ContractID + "," + InsuranceCarAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + InsuranceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + CurrencyID + "," + InsurancePeriodValue.Value + "," + insurancerate.ToString(GlobalVariables.V_CultureInfoEN) + "," + InsuranceUnconditionalValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",TO_DATE('" + InsuranceStartDate.Text + "','DD/MM/YYYY'),TO_DATE('" + InsuranceEndDate.Text + "','DD/MM/YYYY')," + company_id + ",'" + PoliceText.Text.Trim() + "',1," + Class.GlobalVariables.V_UserID + "," + IsAgain + ",'" + NoteText.Text.Trim() + "'," + InsuranceCostValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + "," + (AmountCheck.Checked ? 1 : 0) + ")",
                                                "Sığorta bazaya daxil edilmədi.",
                                          this.Name + "/InsertInsurance");
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 6:
                    GlobalProcedures.FillLookUpEdit(CompanyLookUp, "INSURANCE_COMPANY", "ID", "NAME", null);
                    break;
                case 7:
                    GlobalProcedures.FillLookUpEdit(RateLookUp, "INSURANCE_RATE", "ID", "RATE", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index, int hostage_index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.HostageSelectedTabIndex = hostage_index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void RateLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 8, 7);
        }

        private void CompanyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 8, 6);
        }

        private void CompanyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CompanyLookUp.EditValue == null)
                return;

            company_id = Convert.ToInt32(CompanyLookUp.EditValue);
        }

        private void RateLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (RateLookUp.EditValue == null)
                return;

            int rateID = Convert.ToInt32(RateLookUp.EditValue);
            List<InsuranceRate> lstInsuranceRate = InsuranceRateDAL.SelectInsuranceRate(rateID).ToList<InsuranceRate>();
            insurancerate = lstInsuranceRate.First().Rate;
            InsuranceUnconditionalValue.EditValue = lstInsuranceRate.First().Amount;
            if (RateLookUp.EditorContainsFocus)
                CalcInsuranceCost();
        }

        private void InsuranceAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            CalcInsuranceCost();
        }

        private void AmountCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!AmountCheck.ReadOnly)
                InsuranceCostValue.ReadOnly = !AmountCheck.Checked;
            else
                InsuranceCostValue.ReadOnly = true;

            if (AmountCheck.Checked)
                InsuranceCostValue.Focus();
        }

        private void CalcInsuranceCost()
        {
            InsuranceCostValue.EditValue = InsuranceAmountValue.Value * insurancerate / 100;
        }

        private void CompanyComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 8, 6);
        }

        private void UpdateInsurance()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.INSURANCES_TEMP SET CAR_AMOUNT = " + InsuranceCarAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ", INSURANCE_AMOUNT = " + InsuranceAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",CURRENCY_ID = " + CurrencyID + ",INSURANCE_PERIOD = " + InsurancePeriodValue.Value + ",INSURANCE_INTEREST = " + insurancerate.ToString(GlobalVariables.V_CultureInfoEN) + ",UNCONDITIONAL_AMOUNT = " + InsuranceUnconditionalValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",START_DATE = TO_DATE('" + InsuranceStartDate.Text + "','DD/MM/YYYY'),END_DATE = TO_DATE('" + InsuranceEndDate.Text + "','DD/MM/YYYY'),COMPANY_ID = " + company_id + ",POLICE = '" + PoliceText.Text.Trim() + "',IS_CHANGE = 1,NOTE = '" + NoteText.Text.Trim() + "',AMOUNT = " + InsuranceCostValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",CHECK_AMOUNT = " + (AmountCheck.Checked ? 1 : 0) + " WHERE ID = " + InsuranceID,
                                                "Sığorta bazada dəyişdirilmədi.",
                                                this.Name + "/UpdateInsurance");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlInsureDetails())
            {
                if (TransactionName == "INSERT")
                    InsertInsurance();
                else
                    UpdateInsurance();
                this.Close();
            }
        }
    }
}