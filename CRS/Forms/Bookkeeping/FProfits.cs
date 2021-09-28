using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.Data;
using DevExpress.Utils;
using CRS.Class;

namespace CRS.Forms.Bookkeeping
{
    public partial class FProfits : DevExpress.XtraEditors.XtraForm
    {
        public FProfits()
        {
            InitializeComponent();
        }
        double g_amount = 0, x_amount = 0;

        private void LoadProfits()
        {
            string s = $@"SELECT T.TYPE,
                                         AP.SUB_ACCOUNT_NAME,
                                         AP.SUB_ACCOUNT,
                                         SUM (T.AMOUNT_AZN) AMOUNT,
                                         T.TYPE_ID
                                    FROM (SELECT 1 TYPE_ID,
                                                 'Gəlirlər' TYPE,
                                                 CREDIT_ACCOUNT SUB_ACCOUNT_NUMBER,
                                                 AMOUNT_AZN,
                                                 OPERATION_DATE
                                            FROM CRS_USER.OPERATION_JOURNAL
                                           WHERE SUBSTR (CREDIT_ACCOUNT, 0, 1) = 6
                                          UNION ALL
                                          SELECT 2 TYPE_ID,
                                                 'Xərclər' TYPE,
                                                 DEBIT_ACCOUNT SUB_ACCOUNT_NUMBER,
                                                 AMOUNT_AZN,
                                                 OPERATION_DATE
                                            FROM CRS_USER.OPERATION_JOURNAL
                                           WHERE SUBSTR (DEBIT_ACCOUNT, 0, 1) = 7) T,
                                         CRS_USER.ACCOUNTING_PLAN AP
                                   WHERE     T.SUB_ACCOUNT_NUMBER = AP.SUB_ACCOUNT
                                         AND T.OPERATION_DATE BETWEEN TO_DATE ('{FromDateValue.Text}', 'DD/MM/YYYY')
                                                                  AND TO_DATE ('{ToDateValue.Text}', 'DD/MM/YYYY')
                                GROUP BY T.TYPE_ID,
                                         T.TYPE,
                                         AP.SUB_ACCOUNT,
                                         AP.SUB_ACCOUNT_NAME
                                ORDER BY T.TYPE_ID, AP.SUB_ACCOUNT";

            try
            {                
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadProfits");


                ProfitsGridControl.DataSource = dt;   
                CompareBarButton.Enabled = !(ProfitsGridView.RowCount > 0);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Mənfəət və zərər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);               
            }
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void FProfits_Load(object sender, EventArgs e)
        {           
            FromDateValue.EditValue = DateTime.Today;           
            ToDateValue.EditValue = DateTime.Today;
            GlobalProcedures.FillComboBoxEdit(YearComboBox, "DIM_TIME", "DISTINCT YEAR_ID,YEAR_ID,YEAR_ID", "CALENDAR_DATE IN (SELECT DISTINCT OPERATION_DATE FROM CRS_USER.OPERATION_JOURNAL) ORDER BY 1 DESC");
            YearComboBox.SelectedIndex = 0;
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = GlobalFunctions.ChangeStringToDate(FromDateValue.Text, "ddmmyyyy");
            ToDateValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ProfitsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalına olan mənfəət";
            LoadProfits();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadProfits();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ProfitsGridControl);
        }

        private void ProfitsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ProfitsGridView, PopupMenu, e);
        }

        private void ProfitsGridView_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0)
                {
                    g_amount = 0;
                    x_amount = 0;
                }
            }

            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0)
                {
                    if (e.GetValue("TYPE_ID").ToString() == "1")
                        g_amount += Convert.ToDouble(e.FieldValue);
                    else if (e.GetValue("TYPE_ID").ToString() == "2")
                        x_amount += Convert.ToDouble(e.FieldValue);
                }
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0) //menfeet               
                    e.TotalValue = g_amount - x_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("SUB_ACCOUNT_NAME") == 0 && ProfitsGridView.RowCount > 0) //cemi
                {
                    if (e.GetValue("TYPE_ID").ToString() == "1")
                        e.TotalValue = "Gəlirlər üzrə cəmi";
                    else if (e.GetValue("TYPE_ID").ToString() == "2")
                        e.TotalValue = "Xərclər üzrə cəmi";
                    if (e.IsTotalSummary)
                        e.TotalValue = "Cari mənfəət";
                }
            }            
        }

        private void ProfitsGridView_CustomDrawRowFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "SUB_ACCOUNT_NAME")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;            
        }

        private void ProfitsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("AMOUNT", "Far", e);
            if (e.Column.FieldName == "AMOUNT")
                e.Appearance.ForeColor = Color.Red;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExcellBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ProfitsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ProfitsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ProfitsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ProfitsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ProfitsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ProfitsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ProfitsGridControl, "mht");
        }

        private void CompareBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FCompareInterest fci = new FCompareInterest();
            fci.fromdate = FromDateValue.Text;
            fci.todate = ToDateValue.Text;
            fci.ShowDialog();
        }

        private void ProfitsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ProfitsGridView.GetFocusedDataRow();
            if (row != null)
            {
                if (row["SUB_ACCOUNT"].ToString().Substring(0, 3) == "631")
                    CompareBarButton.Enabled = true;
                else
                    CompareBarButton.Enabled = false;
            }
        }

        private void YearComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FromDateValue.EditValue = new DateTime(int.Parse(YearComboBox.Text), 1, 1);
            ToDateValue.EditValue = new DateTime(int.Parse(YearComboBox.Text), 12, 31);
        }
    }
}