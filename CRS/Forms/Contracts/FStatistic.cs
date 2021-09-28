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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;

namespace CRS.Forms.Contracts
{
    public partial class FStatistic : DevExpress.XtraEditors.XtraForm
    {
        public FStatistic()
        {
            InitializeComponent();
        }
        bool FormStatus = false;

        private void FStatistic_Load(object sender, EventArgs e)
        {
            MonthComboBox.SelectedIndex = DateTime.Today.Month;
            GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
            GlobalProcedures.FillComboBoxEdit(YearComboBox, "DIM_TIME", "DISTINCT YEAR_ID,YEAR_ID,YEAR_ID", "CALENDAR_DATE IN (SELECT DISTINCT START_DATE FROM CRS_USER.V_CONTRACTS) ORDER BY 1 DESC", 1, true);
            //YearComboBox.SelectedIndex = 0; 
        }

        private void LoadContractDataGridView()
        {
            string status = null, sql = null;

            if (ClosedCheck.Checked && !ActiveCheck.Checked)
                status = " AND STATUS_ID = 2 ";
            else if (ActiveCheck.Checked && !ClosedCheck.Checked)
                status = " AND STATUS_ID = 1 ";


            sql = $@"SELECT *
                          FROM (SELECT CON.CONTRACT_ID,
                                       CON.CONTRACT_CODE,
                                       CUS.FULLNAME CUSTOMER_NAME,
                                       CON.START_DATE CDATE,
                                       CON.END_DATE,
                                       CON.AMOUNT,
                                       CON.CURRENCY_CODE,
                                       CON.CURRENCY_RATE,
                                       CON.AMOUNT * CON.CURRENCY_RATE AMOUNT_AZN,
                                       HOS.HOSTAGE,
                                       'Verilmiş müqavilələr' CONTRACT_STATUS,
                                       1 STATUS_ID
                                  FROM CRS_USER.V_CONTRACTS CON,
                                       CRS_USER.V_CUSTOMERS CUS,
                                       CRS_USER.V_HOSTAGE HOS
                                 WHERE     CON.CUSTOMER_ID = CUS.ID
                                       AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                       AND CON.CONTRACT_ID = HOS.CONTRACT_ID
                                UNION ALL
                                SELECT CON.CONTRACT_ID,
                                       CON.CONTRACT_CODE,
                                       NVL (COM.COMMITMENT_NAME, CUS.FULLNAME) CUSTOMER_NAME,
                                       PAY.PAYMENT_DATE CDATE,
                                       CON.END_DATE,
                                       PAY.BASIC_AMOUNT AMOUNT,
                                       CON.CURRENCY_CODE,
                                       PAY.CURRENCY_RATE CURRENCY_RATE,
                                       PAY.BASIC_AMOUNT * PAY.CURRENCY_RATE AMOUNT_AZN,
                                       HOS.HOSTAGE,
                                       'Bağlanılmış müqavilələr' CONTRACT_STATUS,
                                       2 STATUS_ID
                                  FROM CRS_USER.V_CONTRACTS CON,
                                       CRS_USER.V_CUSTOMERS CUS,
                                       CRS_USER.V_HOSTAGE HOS,
                                       CRS_USER.V_CUSTOMER_LAST_PAYMENT PAY,
                                       CRS_USER.V_COMMITMENTS COM
                                 WHERE     CON.CUSTOMER_ID = CUS.ID
                                       AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                       AND CON.STATUS_ID = 6
                                       AND CON.CONTRACT_ID = HOS.CONTRACT_ID
                                       AND CON.CONTRACT_ID = PAY.CONTRACT_ID(+)
                                       AND CON.CONTRACT_ID = COM.CONTRACT_ID(+))
                         WHERE CDATE BETWEEN TO_DATE ('{FromDateValue.Text}', 'DD/MM/YYYY')
                                         AND TO_DATE ('{ToDateValue.Text}', 'DD/MM/YYYY')
                            {status}
                        ORDER BY CONTRACT_CODE DESC,CDATE DESC,CURRENCY_CODE";

            ContractGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContractDataGridView", "Müqavilələrin statistikası yüklənmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            ToDateValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void YearComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (YearComboBox.SelectedIndex < 0)
                return;

            int month = 0, monthLastDay = 0, year = YearComboBox.SelectedIndex, fromYear, toYear;

            if (year > 0 && MonthComboBox.SelectedIndex > 0)
            {
                month = MonthComboBox.SelectedIndex;
                monthLastDay = DateTime.DaysInMonth(int.Parse(YearComboBox.Text), month);
            }

            MonthComboBox.Enabled = (year > 0);

            fromYear = (year == 0) ? 2014 : int.Parse(YearComboBox.Text);
            toYear = (year == 0) ? DateTime.Today.Year : int.Parse(YearComboBox.Text);

            FromDateValue.EditValue = FromDateValue.Properties.MinValue = ToDateValue.Properties.MinValue = new DateTime(fromYear, (month == 0) ? 1 : month, 1);
            ToDateValue.EditValue = FromDateValue.Properties.MaxValue = ToDateValue.Properties.MaxValue = (toYear == DateTime.Today.Year)? DateTime.Today : new DateTime(toYear, (month == 0) ? 12 : month, (monthLastDay == 0) ? 31 : monthLastDay);
        }

        private void MonthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (YearComboBox.SelectedIndex < 0)
                return;

            if (MonthComboBox.SelectedIndex < 0)
                return;

            if (MonthComboBox.SelectedIndex == 0)
            {
                YearComboBox_SelectedIndexChanged(sender, EventArgs.Empty);
                return;
            }

            int month = MonthComboBox.SelectedIndex,
            monthLastDay = DateTime.DaysInMonth(int.Parse(YearComboBox.Text), month);

            FromDateValue.EditValue = FromDateValue.Properties.MinValue = ToDateValue.Properties.MinValue = new DateTime(int.Parse(YearComboBox.Text), month, 1);
            ToDateValue.EditValue = FromDateValue.Properties.MaxValue = ToDateValue.Properties.MaxValue = new DateTime(int.Parse(YearComboBox.Text), month, monthLastDay);
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ContractGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalına olan statistika";
            LoadContractDataGridView();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractDataGridView();
        }

        private void ContractGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ContractGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("AMOUNT_AZN", "Far", e);
        }

        private void ActiveCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle((sender as CheckEdit));
            LoadContractDataGridView();
        }

        private void ContractGridView_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0} : {1}   (cəmi  {2} müqavilə)", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ContractGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ContractGridControl, "xls");
        }

        private void ContractGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ContractGridView, PopupMenu, e);
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            string currency_name = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                ContractGridView.ActiveFilter.Add(ContractGridView.Columns["CURRENCY_CODE"],
                    new ColumnFilterInfo(currency_name, ""));
            else
                ContractGridView.ActiveFilter.Remove(ContractGridView.Columns["CURRENCY_CODE"]);
        }

        private void ContractGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "CONTRACT_CODE")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
        }

        private void ContractGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.Column.FieldName == "CDATE" || e.Column.FieldName == "END_DATE")
            {
                DateTime cdate = Convert.ToDateTime(currentView.GetRowCellValue(e.RowHandle, "CDATE")),
                         edate = Convert.ToDateTime(currentView.GetRowCellValue(e.RowHandle, "END_DATE"));

                if (cdate > edate)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(-1048576);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(-1048576);
                }
            }
        }
    }
}