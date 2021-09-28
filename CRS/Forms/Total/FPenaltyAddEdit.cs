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

namespace CRS.Forms.Total
{
    public partial class FPenaltyAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPenaltyAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        decimal debt = 0, penaltyInterest = 0, penaltyAmount = 0;
        DateTime startDate;
        int penaltyInterestID = 0;

        private void FPenaltyAddEdit_Load(object sender, EventArgs e)
        {
            LoadContractDetail();
        }

        private void LoadContractDetail()
        {
            string sql = $@"SELECT CON.CONTRACT_CODE,
                                   CON.START_DATE,
                                   CON.INTEREST,
                                   CUS.CUSTOMER_NAME,
                                   CON.CURRENCY_CODE,
                                   CON.AMOUNT,
                                   TO_CHAR(CLP.PAYMENT_DATE,'DD.MM.YYYY') PAYMENT_DATE,
                                   NVL (CLP.DEBT, 0) DEBT
                              FROM CRS_USER.V_CONTRACTS CON,
                                   CRS_USER.V_CUSTOMERS CUS,
                                   CRS_USER.V_CUSTOMER_LAST_PAYMENT CLP
                             WHERE     CON.CUSTOMER_ID = CUS.ID
                                   AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND CON.CONTRACT_ID = CLP.CONTRACT_ID(+)
                                   AND CON.CONTRACT_ID = {ContractID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/", "Müqavilənin parametrləri açılmadı.");

            if (dt.Rows.Count > 0)
            {
                ContractCodeText.EditValue = dt.Rows[0]["CONTRACT_CODE"];
                startDate = Convert.ToDateTime(dt.Rows[0]["START_DATE"]);
                ContractStartDateText.EditValue = startDate.ToString("dd.MM.yyyy");
                FromDateValue.Properties.MinValue = startDate;
                CustomerNameText.EditValue = dt.Rows[0]["CUSTOMER_NAME"];
                InterestText.EditValue = dt.Rows[0]["INTEREST"];
                LastDateText.EditValue = dt.Rows[0]["PAYMENT_DATE"];
                debt = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                DebtValueText.EditValue = debt.ToString("n2");
                AmountCurrencyLabel.Text = DebtCurrencyLabel.Text = dt.Rows[0]["CURRENCY_CODE"].ToString();
            }
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            DaysCountText.Text = GlobalFunctions.Days360(FromDateValue.DateTime, ToDateValue.Text.Length > 0 ? ToDateValue.DateTime : DateTime.Today).ToString();
            if (TransactionName == "INSERT")
            {
                DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT ID,INTEREST
                                                                                      FROM CRS_USER.CONTRACT_INTEREST_PENALTIES P
                                                                                     WHERE ID =
                                                                                              (SELECT MAX (ID)
                                                                                                 FROM CRS_USER.CONTRACT_INTEREST_PENALTIES
                                                                                                WHERE     CONTRACT_ID = P.CONTRACT_ID
                                                                                                      AND CALC_DATE <= TO_DATE ('{FromDateValue.Text}', 'DD.MM.YYYY'))
                                                                                        AND P.CONTRACT_ID = {ContractID}", this.Name + "/FromDateValue_EditValueChanged", "Cərimə faizi açılmadı.");

                if (dt.Rows.Count > 0)
                {
                    penaltyInterest = Convert.ToDecimal(dt.Rows[0]["INTEREST"]);
                    penaltyInterestID = Convert.ToInt16(dt.Rows[0]["ID"]);
                }

                PenaltyInterestValueText.EditValue = penaltyInterest.ToString("n2");
                PenaltyAmount();
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void Insert()
        {
            //string sql = $@"INSERT INTO CRS_USER.CONTRACT_CALCULATED_PENALTIES(CONTRACT_ID,LAST_PAYMENT_DATE,START_DATE,END_DATE,DEBT,PENALTY_AMOUNT,INTEREST_PENALTIES_ID)
            //                VALUE({ContractID},TO_DATE('{LastDateText.Text}','DD.MM.YYYY'),TO_DATE('{FromDateValue.Text}','DD.MM.YYYY'),TO_DATE('{ToDateValue.Text}','DD.MM.YYYY'),{debt.ToString(GlobalVariables.V_CultureInfoEN)},{penaltyAmount.ToString(GlobalVariables.V_CultureInfoEN)},{penaltyInterestID})";

            //GlobalProcedures.ExecuteQuery(sql, "Hesablanmış cərimə faizi bazaya daxil edilmədi.", this.Name + "/Insert");

            GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_CALC_CONTRACT_PENALTY", "P_CONTRACT_ID", int.Parse(ContractID), "P_DATE", FromDateValue.Text, "Cərimə hesablanmadı.");
        }

        private void FPenaltyAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void Update()
        {

        }

        private void PenaltyAmount()
        {
            int dayCount = Convert.ToInt16(DaysCountText.Text); //GlobalFunctions.Days360(FromDateValue.DateTime, ToDateValue.Text.Length > 0 ? ToDateValue.DateTime : DateTime.Today);
            penaltyAmount = Math.Round(debt * penaltyInterest / 100, 2) * dayCount;
            PenaltyAmountText.EditValue = penaltyAmount.ToString("n2");
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(FromDateValue.Text))
            {
                FromDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cərimənin başlama tarixi seçilməyib.");
                FromDateValue.Focus();
                FromDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }
    }
}