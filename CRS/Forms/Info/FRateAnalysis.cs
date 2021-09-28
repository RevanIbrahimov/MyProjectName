using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;

namespace CRS.Forms.Info
{
    public partial class FRateAnalysis : DevExpress.XtraEditors.XtraForm
    {
        public FRateAnalysis()
        {
            InitializeComponent();
        }
        string currency_name;
        bool FormStatus = false;

        private void FRateAnalysis_Load(object sender, EventArgs e)
        {
            Class.GlobalProcedures.DateEditFormat(FromDateValue);
            Class.GlobalProcedures.DateEditFormat(ToDateValue);
            FromDateValue.EditValue = DateTime.Today.AddDays(-10);            
            ToDateValue.EditValue = DateTime.Today;
            Class.GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
            LoadRateChart();
            FormStatus = true;
        }

        private void LoadRateChart()
        {
            string s = null;
            try
            {
                s = "SELECT C.CODE,CR.RATE_DATE R_DATE,CR.AMOUNT FROM CRS_USER.CURRENCY_RATES CR,CRS_USER.CURRENCY C WHERE CR.CURRENCY_ID = C.ID AND CR.RATE_DATE BETWEEN TO_DATE('" + FromDateValue.Text + "','DD/MM/YYYY') AND TO_DATE('" + ToDateValue.Text + "','DD/MM/YYYY')" + currency_name;
                               
                chart.DataSource = Class.GlobalFunctions.GenerateDataTable(s);               
                chart.SeriesDataMember = "CODE";
                
                chart.SeriesTemplate.ArgumentDataMember = "R_DATE";
                chart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "AMOUNT" });
                chart.SeriesTemplate.SeriesPointsSorting = SortingMode.Ascending;
                chart.SeriesTemplate.SeriesPointsSortingKey = SeriesPointKey.Argument;
                chart.SeriesTemplate.View = new LineSeriesView();
                              
                if (ShowLabelCheck.Checked)
                    chart.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;              
                else
                    chart.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;              
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
                chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
                chart.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
                chart.Legend.Direction = LegendDirection.LeftToRight;
                //chart.Legend.Antialiasing = false;
                //chart.Legend.Font = new Font("Arial", 9, FontStyle.Bold);                
            }
            catch (Exception exx)
            { }
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                ToDateValue.Properties.MinValue = Class.GlobalFunctions.ChangeStringToDate(FromDateValue.Text, "ddmmyyyy");
                LoadRateChart();
            }
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            if(FormStatus)
                LoadRateChart();
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                currency_name = " AND C.CODE IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            else
                currency_name = null;
            LoadRateChart();
        }
    }
}