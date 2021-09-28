using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CRS.Forms
{
    public partial class FCreditCalculator : DevExpress.XtraEditors.XtraForm
    {
        public FCreditCalculator()
        {
            InitializeComponent();
        }

        private void FCreditCalculator_Load(object sender, EventArgs e)
        {

        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreditAmountValue_EditValueChanged(object sender, EventArgs e)
        {
            MonthlyPaymentText.Text = Class.GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)PeriodValue.Value, (double)InterestValue.Value).ToString("N2");            
            SumPaymentText.Text = (Class.GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)PeriodValue.Value, (double)InterestValue.Value) * (double)PeriodValue.Value).ToString("N2");
            InterestAmountText.Text = ((Class.GlobalFunctions.CalcPayment((double)CreditAmountValue.Value, (double)PeriodValue.Value, (double)InterestValue.Value) * (double)PeriodValue.Value) - (double)CreditAmountValue.Value).ToString("N2");
        }
    }
}