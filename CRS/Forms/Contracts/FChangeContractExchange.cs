using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CRS.Forms.Contracts
{
    public partial class FChangeContractExchange : DevExpress.XtraEditors.XtraForm
    {
        public FChangeContractExchange()
        {
            InitializeComponent();
        }
        public string CurrencyID, CreditCurrencyID, ContractID, TransactionName;
        public decimal Rate;

        public delegate void DoEvent(decimal a, bool v, int currencyid);
        public event DoEvent RefreshRate;

        private void FChangeContractExchange_Load(object sender, EventArgs e)
        {
            CurrencyLabel.Text = Class.GlobalFunctions.GetName("SELECT VALUE||' '||CODE FROM CRS_USER.CURRENCY WHERE ID = " + CreditCurrencyID);
            CurrencyLabel2.Text = Class.GlobalFunctions.GetName("SELECT CODE FROM CRS_USER.CURRENCY WHERE ID = " + CurrencyID);
            CurrencyRateValue.Value = Rate;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (CurrencyRateValue.Value <= 0)
            {
                CurrencyRateValue.BackColor = Color.Red;
                XtraMessageBox.Show("Məzənnə sıfırdan böyük olmalıdır.");
                CurrencyRateValue.Focus();
                CurrencyRateValue.BackColor = Class.GlobalFunctions.ElementColor();
            }
            else
            {
                this.RefreshRate(CurrencyRateValue.Value, true, int.Parse(CreditCurrencyID));
                this.Close();
            }
        }
    }
}