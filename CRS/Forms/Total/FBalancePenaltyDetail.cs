using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using System.Collections;
using CRS.Class;

namespace CRS.Forms.Total
{
    public partial class FBalancePenaltyDetail : DevExpress.XtraEditors.XtraForm
    {
        public FBalancePenaltyDetail()
        {
            InitializeComponent();
        }
        public string CustomerID, ContractID, Currency;

        private void FBalancePenaltyDetail_Load(object sender, EventArgs e)
        {
            GlobalProcedures.CalcEditFormat(PenaltyValue);
            CurrencyLabel.Text = Currency;
            LoadPenaltyDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadPenaltyDataGridView()
        {
            string s = "SELECT 1 SS,CP.ID," +
                               "TO_CHAR (CP.LAST_PAYMENT_DATE, 'DD.MM.YYYY') LPD," +
                               "TO_CHAR (CP.START_DATE, 'DD.MM.YYYY') SD," +
                               "TO_CHAR (CP.END_DATE, 'DD.MM.YYYY') ED," +
                               "CP.DAY_COUNT," +
                               "CP.DEBT," +
                               "IP.INTEREST," +
                               "CP.PENALTY_AMOUNT," +
                               "CP.IS_CALCULATED " +
                        "FROM CRS_USER.CONTRACT_CALCULATED_PENALTIES CP," +
                            "CRS_USER.CONTRACT_INTEREST_PENALTIES IP " +
                        "WHERE CP.IS_BALANCE = 1 AND CP.INTEREST_PENALTIES_ID = IP.ID AND CP.CUSTOMER_ID = " + CustomerID + " AND CP.CONTRACT_ID = " + ContractID;
            try
            {
                PenaltyGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPenaltyDataGridView");
                PenaltyGridView.PopulateColumns();
                PenaltyGridView.Columns[0].Caption = "S/s";
                PenaltyGridView.Columns[0].Visible = false;
                PenaltyGridView.Columns[1].Visible = false;
                PenaltyGridView.Columns[2].Caption = "Son ödənişin tarixi";
                PenaltyGridView.Columns[3].Caption = "Cərimənin başlama tarixi";
                PenaltyGridView.Columns[4].Caption = "Cərimənin bitmə tarixi";
                PenaltyGridView.Columns[5].Caption = "Günlərin sayı";
                PenaltyGridView.Columns[6].Caption = "Qalıq";
                PenaltyGridView.Columns[7].Caption = "Faiz";
                PenaltyGridView.Columns[8].Caption = "Cərimə";
                PenaltyGridView.Columns[9].Visible = false;
                
                PenaltyGridView.Columns[2].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                PenaltyGridView.Columns[2].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                PenaltyGridView.Columns[3].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                PenaltyGridView.Columns[3].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                PenaltyGridView.Columns[4].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                PenaltyGridView.Columns[4].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                PenaltyGridView.Columns[5].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                PenaltyGridView.Columns[5].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                PenaltyGridView.Columns[7].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                PenaltyGridView.Columns[7].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                PenaltyGridView.Columns[6].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
                PenaltyGridView.Columns[6].DisplayFormat.FormatType = FormatType.Custom;
                PenaltyGridView.Columns[8].DisplayFormat.FormatString = "### ### ### ### ### ### ##0.00";
                PenaltyGridView.Columns[8].DisplayFormat.FormatType = FormatType.Custom;

                if (PenaltyGridView.RowCount > 0)
                {   
                    PenaltyGridView.Columns[8].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "{0:### ### ### ### ### ### ##0.00}");
                }               

                PenaltyGridView.BestFitColumns();

                try
                {
                    PenaltyGridView.BeginUpdate();
                    for (int i = 0; i < PenaltyGridView.RowCount; i++)
                    {
                        DataRow row = PenaltyGridView.GetDataRow(PenaltyGridView.GetVisibleRowHandle(i));
                        if (Convert.ToInt32(row["IS_CALCULATED"].ToString()) == 1)
                            PenaltyGridView.SelectRow(i);
                    }
                }
                finally
                {
                    PenaltyGridView.EndUpdate();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müştəridən tutulan cərimələr cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void PenaltyGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("PENALTY_AMOUNT", "Far", e);
        }

        private void PenaltyGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            double PenaltySum = 0;
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PenaltyGridView.SelectedRowsCount; i++)
            {
                rows.Add(PenaltyGridView.GetDataRow(PenaltyGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                PenaltySum = PenaltySum + Convert.ToDouble(row["PENALTY_AMOUNT"].ToString());
            }
            PenaltyValue.Value = (decimal)PenaltySum;
            PenaltyValue.Value = (decimal)(PenaltySum);
            PenaltyGridView.UpdateTotalSummary();
        }      
    }
}