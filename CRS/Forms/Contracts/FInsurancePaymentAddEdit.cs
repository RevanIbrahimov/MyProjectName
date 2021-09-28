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

namespace CRS.Forms.Contracts
{
    public partial class FInsurancePaymentAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsurancePaymentAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, StartDate;
        public int? ID;
        public int InsuranceID, TypeID, PayedCount;
        public decimal InsuranceAmount, CurrentDebt;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        decimal oldPayed = 0;
        int customerPaymentID = 0;
        bool CurrentStatus = false;

        private void FInsurancePaymentAddEdit_Load(object sender, EventArgs e)
        {
            PayedDateValue.Properties.MinValue = GlobalFunctions.ChangeStringToDate(StartDate, "ddmmyyyy");
            this.Text = TypeID == 1 ? "Ödənişin əlavə/düzəliş edilməsi" : "Əvəzləşmənin əlavə/düzəliş edilməsi";
            DateLabel.Text = TypeID == 1 ? "Ödənişin tarixi" : "Əvəzləşmənin tarixi";
            AmountLabel.Text = TypeID == 1 ? "Ödənilən" : "Əvəzləşdirilən";
            BOK.Text = TypeID == 1 ? "Ödəniş et" : "Əvəzləşdir";
            LegalCheck.Visible = TypeID == 1;
            if (TransactionName == "EDIT")
            {
                LoadDetails();

                if(customerPaymentID > 0)
                {
                    GlobalProcedures.ShowWarningMessage("Bu ödəniş lizinq ödənişindən avtomatik ödənildiyindən dəyişdirilə bilməz.");
                    CurrentStatus = true;
                }

                ComponentEnable(CurrentStatus);
            }
            else
            {
                PayedDateValue.DateTime = DateTime.Today;
                PayedAmountValue.EditValue = CurrentDebt;
            }
        }

        private void ComponentEnable(bool status)
        {
            PayedDateValue.Enabled =
                PayedAmountValue.Enabled =
                LegalCheck.Enabled =
                NoteText.Enabled = 
                BOK.Visible = !status;
        }

        private void LoadDetails()
        {
            string sql = null;
            if (TypeID == 1)
                sql = $@"SELECT PAY_DATE PDATE,
                                   PAYED_AMOUNT AMOUNT,
                                   NOTE,
                                   IS_LEGAL,
                                   CUSTOMER_PAYMENT_ID
                              FROM CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP
                             WHERE ID = {ID}";
            else
                sql = $@"SELECT TRANSFER_DATE PDATE,
                                   COMPENSATION AMOUNT,
                                   NOTE,
                                   0 IS_LEGAL,
                                   0 CUSTOMER_PAYMENT_ID
                              FROM CRS_USER_TEMP.INSURANCE_TRANSFER_TEMP
                             WHERE ID = {ID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", (TypeID == 1)? "Ödəniş açılmadı." : "Əvəzləşmə açılmadı.");

            if (dt.Rows.Count > 0)
            {
                PayedDateValue.EditValue = dt.Rows[0]["PDATE"];
                oldPayed = Convert.ToDecimal(dt.Rows[0]["AMOUNT"]);
                PayedAmountValue.EditValue = oldPayed;
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                LegalCheck.Checked = Convert.ToInt16(dt.Rows[0]["IS_LEGAL"]) == 1;
                customerPaymentID = Convert.ToInt32(dt.Rows[0]["CUSTOMER_PAYMENT_ID"]);
            }
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (PayedDateValue.Text.Length == 0)
            {
                PayedDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarixi seçilməyib.");
                PayedDateValue.Focus();
                PayedDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PayedAmountValue.Value <= 0)
            {
                PayedAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğ sıfırdan böyük olmalıdır.");
                PayedAmountValue.Focus();
                PayedAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TypeID == 1 && PayedAmountValue.Value > CurrentDebt)
            {
                PayedAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödənilən məbləğ maksimum " + CurrentDebt + " AZN olmalıdır.");
                PayedAmountValue.Focus();
                PayedAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
            if (TypeID == 2 && PayedAmountValue.Value > CurrentDebt)
            {
                PayedAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əvəzləşdiriləcək məbləğ maksimum " + CurrentDebt + " AZN olmalıdır.");
                PayedAmountValue.Focus();
                PayedAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void Insert()
        {
            string sql = null;
            if (TypeID == 1)
                sql = $@"INSERT INTO CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP (ID,
                                                   INSURANCE_ID,
                                                   PAY_DATE,
                                                   PAYED_AMOUNT,    
                                                   NOTE,
                                                   IS_CHANGE,
                                                   IS_LEGAL,
                                                   USED_USER_ID)
                            VALUES(CRS_USER.INSURANCE_PAYMENT_SEQUENCE.NEXTVAL,
                                    {InsuranceID},
                                    TO_DATE('{PayedDateValue.Text}','DD.MM.YYYY'),
                                    {Math.Round(PayedAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},  
                                    '{NoteText.Text.Trim()}',
                                    1,
                                    {(LegalCheck.Checked? 1 : 0)},
                                    {GlobalVariables.V_UserID})";
            else
                sql = $@"INSERT INTO CRS_USER_TEMP.INSURANCE_TRANSFER_TEMP (ID,
                                                   INSURANCE_ID,
                                                   TRANSFER_DATE,
                                                   COMPENSATION,    
                                                   NOTE,
                                                   IS_CHANGE,
                                                   USED_USER_ID)
                            VALUES(CRS_USER.INSURANCE_TRANSFER_SEQUENCE.NEXTVAL,
                                    {InsuranceID},
                                    TO_DATE('{PayedDateValue.Text}','DD.MM.YYYY'),
                                    {Math.Round(PayedAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},  
                                    '{NoteText.Text.Trim()}',
                                    1,
                                    {GlobalVariables.V_UserID})";
            GlobalProcedures.ExecuteQuery(sql, TypeID == 1? "Ödəniş temp cədvələ daxil edilmədi." : "Əvəzləşmə temp cədvələ daxil edilmədi.", this.Name + "/Insert");


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

        private void FInsurancePaymentAddEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void Update()
        {
            string sql = null;
            if (TypeID == 1)
                sql = $@"UPDATE CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP SET
                                                   PAY_DATE = TO_DATE('{PayedDateValue.Text}','DD.MM.YYYY'),
                                                   PAYED_AMOUNT = {Math.Round(PayedAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                                                    
                                                   NOTE = '{NoteText.Text.Trim()}',
                                                   IS_CHANGE = 1,
                                                   IS_LEGAL = {(LegalCheck.Checked ? 1 : 0)}
                            WHERE ID = {ID}";
            else
                sql = $@"UPDATE CRS_USER_TEMP.INSURANCE_TRANSFER_TEMP SET
                                                   TRANSFER_DATE = TO_DATE('{PayedDateValue.Text}','DD.MM.YYYY'),
                                                   COMPENSATION = {Math.Round(PayedAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                                                    
                                                   NOTE = '{NoteText.Text.Trim()}',
                                                   IS_CHANGE = 1
                            WHERE ID = {ID}";
            GlobalProcedures.ExecuteQuery(sql, "Əvəzləşmə temp cədvəldə dəyişdirilmədi.", this.Name + "/Update");
        }
    }
}