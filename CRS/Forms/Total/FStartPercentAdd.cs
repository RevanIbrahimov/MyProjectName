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
    public partial class FStartPercentAdd : DevExpress.XtraEditors.XtraForm
    {
        public FStartPercentAdd()
        {
            InitializeComponent();
        }
        public int ContractID;

        public delegate void DoEvent(decimal a);
        public event DoEvent RefreshPaymentsDataGridView;

        private void FStartPercentAdd_Load(object sender, EventArgs e)
        {
            string sql = $@"SELECT START_PERCENT FROM CRS_USER.CONTRACT_START_PERCENT WHERE CONTRACT_ID = {ContractID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/FStartPercentAdd_Load", "Başlanğıc qalıq faiz açılmadı.");
            if(dt.Rows.Count > 0)
            {
                PercentValue.Value = (decimal)dt.Rows[0]["START_PERCENT"];
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPercent())
            {
                GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CONTRACT_START_PERCENT WHERE CONTRACT_ID = {ContractID}",
                                                 $@"INSERT INTO CRS_USER.CONTRACT_START_PERCENT(CONTRACT_ID,START_PERCENT)VALUES({ContractID},{Math.Round(PercentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)})", 
                                                 "Müqavilənin qalıq faizi daxil edilmədi.", 
                                                 this.Name + "/BOK_Click");
                this.RefreshPaymentsDataGridView(PercentValue.Value);
                this.Close();
            }
        }

        private bool ControlPercent()
        {
            bool b = false;

            if (PercentValue.Value <= 0)
            {
                PercentValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Başlanğıc qalıq faiz sıfırdan böyük olmalıdır.");
                PercentValue.Focus();
                PercentValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}