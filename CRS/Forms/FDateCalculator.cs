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

namespace CRS.Forms
{
    public partial class FDateCalculator : DevExpress.XtraEditors.XtraForm
    {
        public FDateCalculator()
        {
            InitializeComponent();
        }

        private void FDateCalculator_Load(object sender, EventArgs e)
        {           
            EndDate.EditValue = StartDate.EditValue = DateTime.Today;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DateDiff()
        {
            if (TypeRadioGroup.SelectedIndex == 0)
                DayResLabel.Text = GlobalFunctions.Days360(StartDate.DateTime, EndDate.DateTime) + " gün";
            else
                DayResLabel.Text = GlobalFunctions.DifferenceTwoDateWithDay(StartDate.DateTime, EndDate.DateTime) + " gün";
            MonthResLabel.Text = GlobalFunctions.DifferenceTwoDateWithMonth(StartDate.DateTime, EndDate.DateTime) + " ay";
            YearResLabel.Text = GlobalFunctions.DifferenceTwoDateWithYear360(StartDate.DateTime, EndDate.DateTime) + " il";
        }

        private void EndDate_EditValueChanged(object sender, EventArgs e)
        {
            DateDiff();
        }

        private void TypeRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateDiff();
        }
    }
}