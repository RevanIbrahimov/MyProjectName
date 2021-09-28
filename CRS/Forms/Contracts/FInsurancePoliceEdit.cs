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
    public partial class FInsurancePoliceEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsurancePoliceEdit()
        {
            InitializeComponent();
        }
        public int InsuranceID;
        public string ContractCode, Police;
        public DateTime StartDate, EndDate;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FInsurancePoliceEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            string sql = $@"UPDATE CRS_USER.INSURANCES SET POLICE = '{PoliceText.Text.Trim()}' WHERE ID = {InsuranceID}";
            GlobalProcedures.ExecuteQuery(sql, "Sığortanın polisdəki qeydiyyatı dəyişdirilmədi.", this.Name + "/BOK_Click");
            this.Close();
        }

        private void FInsurancePoliceEdit_Load(object sender, EventArgs e)
        {
            ContractCodeText.Text = ContractCode;
            InsuranceStartDate.EditValue = StartDate;
            InsuranceEndDate.EditValue = EndDate;
            PoliceText.Text = Police;
        }
    }
}