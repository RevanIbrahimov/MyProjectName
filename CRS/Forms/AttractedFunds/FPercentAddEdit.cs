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
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.AttractedFunds
{
    public partial class FPercentAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPercentAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int ContractID;
        public int? PercentID;
        public DateTime ContractStartDate, ContractEndDate;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FPercentAddEdit_Load(object sender, EventArgs e)
        {
            List<FundContractPercent> lstPercent = FundContractPercentDAL.SelectFundContractPercentTempByContractID(ContractID).ToList<FundContractPercent>();
            if (TransactionName == "EDIT")
            {
                if (lstPercent.Where(item => item.ID < PercentID).ToList<FundContractPercent>().Count > 0)
                    StartDate.Properties.MinValue = lstPercent.Where(item => item.ID < PercentID).Max(item => item.PDATE);
                else
                    StartDate.Properties.MinValue = ContractStartDate;
                if (lstPercent.Where(item => item.ID > PercentID).ToList<FundContractPercent>().Count > 0)
                    StartDate.Properties.MaxValue = lstPercent.Where(item => item.ID > PercentID).Min(item => item.PDATE);
                else
                    StartDate.Properties.MaxValue = ContractEndDate;
                LoadPercent();
            }
            else
            {
                PercentID = GlobalFunctions.GetOracleSequenceValue("FUNDS_PERCENTS_SEQUENCE");
                if (lstPercent.Count > 0)                
                    StartDate.Properties.MinValue = lstPercent.Max(item => item.PDATE);               
                else
                    StartDate.Properties.MinValue = ContractStartDate;
                StartDate.Properties.MaxValue = ContractEndDate;
            }
        }

        private void LoadPercent()
        {
            string sql = $@"SELECT PDATE,PERCENT_VALUE,NOTE FROM CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP WHERE ID = {PercentID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPercent", "İllik faiz açılmadı.");

            if (dt.Rows.Count > 0)
            {
                StartDate.EditValue = dt.Rows[0]["PDATE"];
                PercentValue.EditValue = dt.Rows[0]["PERCENT_VALUE"];
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
            }
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(StartDate.Text))
            {
                StartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix seçilməyib.");
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP WHERE IS_CHANGE != 2 AND ID != {PercentID} AND FUNDS_CONTRACTS_ID = {ContractID} AND PDATE = TO_DATE('{StartDate.Text}','DD.MM.YYYY')") > 0)
            {
                StartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(StartDate.Text + " tarixi üçün artıq illik faiz daxil edilib. Zəhmət olmasa başqa tarix seçin.");
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void Insert()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP(ID,FUNDS_CONTRACTS_ID,PDATE,PERCENT_VALUE,NOTE,USED_USER_ID,IS_CHANGE)
                            VALUES({PercentID},{ContractID},TO_DATE('{StartDate.Text}','DD.MM.YYYY'),{Math.Round(PercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},'{NoteText.Text.Trim()}',{GlobalVariables.V_UserID},1)";

            GlobalProcedures.ExecuteQuery(sql, "İllik faiz temp cədvələ daxil edilmədi.", this.Name + "/Insert");
        }

        private void Update()
        {
            string sql = $@"UPDATE CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP SET PDATE = TO_DATE('{StartDate.Text}','DD.MM.YYYY'),PERCENT_VALUE = {Math.Round(PercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},NOTE = '{NoteText.Text.Trim()}',IS_CHANGE = 1 WHERE ID = {PercentID} AND USED_USER_ID = {GlobalVariables.V_UserID}";
            GlobalProcedures.ExecuteQuery(sql, "İllik faiz temp cədvəldə dəyişdirilmədi.", this.Name + "/Update");
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

        private void FPercentAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }
    }
}