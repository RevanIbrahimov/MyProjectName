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
using DevExpress.XtraCharts;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Forms.Bookkeeping;

namespace CRS.Forms.Total
{
    public partial class FPortfelReports : DevExpress.XtraEditors.XtraForm
    {
        public FPortfelReports()
        {
            InitializeComponent();
        }
        DateTime date1From, date1To, date2From, date2To, date3From, date3To;
        int topindex, old_row_num;
        string contractID;

        private void FPortfelReports_Load(object sender, EventArgs e)
        {

        }

        private void LoadSelectedPage()
        {
            switch (ReportsBackstageViewControl.SelectedTabIndex)
            {
                case 0:
                    LoadDelyasDataGridView();
                    break;
                case 2:
                    {
                        LoadPortfelWirhInterest();
                        LoadPortfelWirhPeriod();
                    }
                    break;
                case 3:
                    LoadPortfelDataGridView();
                    break;
                case 4:
                    {
                        GlobalProcedures.FillComboBoxEdit(YearComboBox, "DIM_TIME", "DISTINCT YEAR_ID,YEAR_ID,YEAR_ID", "CALENDAR_DATE IN (SELECT DISTINCT START_DATE FROM CRS_USER.V_CONTRACTS) ORDER BY 1 DESC", 1, true);
                        int currentMonth = DateTime.Today.Month;
                        MonthComboBox.SelectedIndex = currentMonth;
                        GenerateDate();
                    }
                    break;
                case 5:
                    LoadContractExtend();
                    break;
                case 6:
                    LoadContractExtend2();
                    break;
                case 7:
                    DebtFromDate.DateTime = new DateTime(DateTime.Today.Year, 1, 1);
                    DebtToDate.DateTime = DateTime.Today;
                    LoadContractMonthDebt();
                    break;
            }
        }

        private void LoadDelyasDataGridView()
        {
            string s = @"SELECT CON.CUSTOMER_ID,
                                 CON.CONTRACT_ID,
                                 NVL (COM.COMMITMENT_NAME, CUS.FULLNAME) CUSTOMER_NAME,
                                 CON.CONTRACT_CODE,
                                 CON.AMOUNT,
                                 CON.CURRENCY_CODE,
                                 H.HOSTAGE_TYPE,
                                 H.HOSTAGE,
                                 LT.DEBT,
                                 CON.MONTHLY_AMOUNT,
                                 LT.FULL_MONTH_COUNT,
                                 LT.REQUIRED,
                                 LT.DELAYS,
                                 ROUND(LT.REQUIRED / CON.MONTHLY_AMOUNT, 2) DELAY_MONTH,
                                 CON.CREDIT_TYPE_ID,
                                 CON.CONTRACT_EVALUATE_NAME
                            FROM CRS_USER.LEASING_TOTAL LT,
                                 CRS_USER.V_CUSTOMERS CUS,
                                 CRS_USER.V_CONTRACTS CON,
                                 CRS_USER.V_COMMITMENTS COM,
                                 CRS_USER.V_HOSTAGE H
                           WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                                 AND CON.CUSTOMER_ID = CUS.ID
                                 AND CON.CONTRACT_ID = H.CONTRACT_ID
                                 AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                 AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                 AND LT.DELAYS > 0
                        ORDER BY CON.CONTRACT_CODE";

            DelaysGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDelyassDataGridView");
        }

        private void LoadPortfelWirhInterest()
        {
            string sql = $@"WITH T1
                                 AS (SELECT DISTINCT
                                            (CASE
                                                WHEN CON.INTEREST < 20 THEN '<20%'
                                                WHEN CON.INTEREST BETWEEN 20 AND 25 THEN '20% - 25%'
                                                WHEN CON.INTEREST BETWEEN 25.01 AND 30 THEN '25% - 30%'
                                                WHEN CON.INTEREST BETWEEN 30.01 AND 40 THEN '30% - 40%'
                                             END)
                                               INTEREST,
                                            (CASE
                                                WHEN CON.INTEREST < 20 THEN 1
                                                WHEN CON.INTEREST BETWEEN 20 AND 25 THEN 2
                                                WHEN CON.INTEREST BETWEEN 25.01 AND 30 THEN 3
                                                WHEN CON.INTEREST BETWEEN 30.01 AND 40 THEN 4
                                             END)
                                               INTEREST_ORDER
                                       FROM CRS_USER.V_CONTRACTS CON
                                     UNION ALL
                                     SELECT '>40%' INTEREST, 5 INTEREST_ORDER FROM DUAL),
                                 T2
                                 AS (  SELECT INTEREST_ORDER,
                                              SUM (DEBT) DEBT,
                                              ROUND (RATIO_TO_REPORT (SUM (DEBT)) OVER () * 100, 2) PERCENT
                                         FROM (SELECT (CASE
                                                          WHEN CON.INTEREST < 20 THEN 1
                                                          WHEN CON.INTEREST BETWEEN 20 AND 25 THEN 2
                                                          WHEN CON.INTEREST BETWEEN 25.01 AND 30 THEN 3
                                                          WHEN CON.INTEREST BETWEEN 30.01 AND 40 THEN 4
                                                          WHEN CON.INTEREST > 40 THEN 5
                                                       END)
                                                         INTEREST_ORDER,
                                                      ROUND (LT.DEBT * NVL (RT.AMOUNT, 1), 2) DEBT
                                                 FROM CRS_USER.LEASING_TOTAL LT,
                                                      CRS_USER.V_CONTRACTS CON,
                                                      CRS_USER.V_LAST_CURRENCY_RATE RT
                                                WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                                                      AND CON.CURRENCY_CODE = RT.CURRENCY_CODE(+)
                                                      AND CON.IS_COMMIT = 1
                                                      AND CON.STATUS_ID = 5)
                                     GROUP BY INTEREST_ORDER)
                              SELECT T1.INTEREST, NVL (ROUND (T2.DEBT, 2), 0) DEBT, NVL(T2.PERCENT, 0) PERCENT
                                FROM T1 LEFT JOIN T2 ON T1.INTEREST_ORDER = T2.INTEREST_ORDER
                            ORDER BY T1.INTEREST_ORDER";

            InterestGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPortfelWirhInterest", "Faizlər üzrə portfelin təsnifatı yüklənmədi.");

        }

        private void LoadPortfelWirhPeriod()
        {
            string sql = $@"WITH T1
                             AS (SELECT DISTINCT
                                        (CASE
                                            WHEN CON.PERIOD < 12 THEN '<12ay'
                                            WHEN CON.PERIOD BETWEEN 12 AND 24 THEN '12ay - 24ay'
                                            WHEN CON.PERIOD BETWEEN 25 AND 36 THEN '25ay - 36ay'
                                         END)
                                           PERIOD,
                                        (CASE
                                            WHEN CON.PERIOD < 12 THEN 1
                                            WHEN CON.PERIOD BETWEEN 12 AND 24 THEN 2
                                            WHEN CON.PERIOD BETWEEN 25 AND 36 THEN 3
                                         END)
                                           PERIOD_ORDER
                                   FROM CRS_USER.V_CONTRACTS CON
                                 UNION ALL
                                 SELECT '37ay - 48ay' PERIOD, 4 PERIOD_ORDER FROM DUAL  
                                 UNION ALL
                                 SELECT '>48ay' PERIOD, 5 PERIOD_ORDER FROM DUAL),
                             T2
                             AS (  SELECT PERIOD_ORDER,
                                          SUM (DEBT) DEBT,
                                          ROUND (RATIO_TO_REPORT (SUM (DEBT)) OVER () * 100, 2) PERCENT
                                     FROM (SELECT (CASE
                                                      WHEN CON.PERIOD < 12 THEN 1
                                                      WHEN CON.PERIOD BETWEEN 12 AND 24 THEN 2
                                                      WHEN CON.PERIOD BETWEEN 25 AND 36 THEN 3
                                                      WHEN CON.PERIOD BETWEEN 37 AND 48 THEN 4
                                                      WHEN CON.PERIOD > 48 THEN 5
                                                   END)
                                                     PERIOD_ORDER,
                                                  ROUND (LT.DEBT * NVL (RT.AMOUNT, 1), 2) DEBT
                                             FROM CRS_USER.LEASING_TOTAL LT,
                                                  CRS_USER.V_CONTRACTS CON,
                                                  CRS_USER.V_LAST_CURRENCY_RATE RT
                                            WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                                                  AND CON.CURRENCY_CODE = RT.CURRENCY_CODE(+)
                                                  AND CON.IS_COMMIT = 1
                                                  AND CON.STATUS_ID = 5 )
                                 GROUP BY PERIOD_ORDER)
                          SELECT T1.PERIOD, NVL (ROUND (T2.DEBT, 2), 0) DEBT, NVL(T2.PERCENT, 0) PERCENT
                            FROM T1 LEFT JOIN T2 ON T1.PERIOD_ORDER = T2.PERIOD_ORDER
                        ORDER BY T1.PERIOD_ORDER";

            PeriodGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPortfelWirhInterest", "Müddətlət üzrə portfelin təsnifatı yüklənmədi.");

        }

        private void LoadPortfelDataGridView()
        {
            string s = @"SELECT CON.CUSTOMER_ID,
                                 CON.CONTRACT_ID,
                                 NVL (COM.COMMITMENT_NAME, CUS.FULLNAME) CUSTOMER_NAME,
                                 CON.CONTRACT_CODE,
                                 CON.AMOUNT,
                                 CON.CURRENCY_CODE,
                                 H.HOSTAGE_TYPE,
                                 H.SHORT_HOSTAGE HOSTAGE,
                                 LT.DEBT,
                                 CON.MONTHLY_AMOUNT,
                                 LT.FULL_MONTH_COUNT,
                                 LT.REQUIRED,
                                 LT.DELAYS,
                                 ROUND (LT.REQUIRED / CON.MONTHLY_AMOUNT, 2) AT,
                                 ROUND ((LT.DEBT / CON.AMOUNT)*100, 2) QB,
                                 ROUND (((LT.DEBT * NVL (CR.AMOUNT, 1)) / H.LIQUID_AMOUNT)*100, 2) QD,
                                 CON.CREDIT_TYPE_ID,
                                 (CASE
                                     WHEN COM.COMMITMENT_NAME IS NULL THEN CUS.PHONE
                                     ELSE COM.PHONE
                                  END)
                                    PHONES,
                                 EXTRACT (DAY FROM CON.START_DATE)
                                        PAYMENT_DAY,
                                 (CASE WHEN NVL(CE.END_DATE, CON.END_DATE) < TRUNC(SYSDATE) THEN 1 ELSE 0 END) IS_OLD
                            FROM CRS_USER.LEASING_TOTAL LT,
                                 CRS_USER.V_CUSTOMERS CUS,
                                 CRS_USER.V_CONTRACTS CON,
                                 CRS_USER.V_COMMITMENTS COM,
                                 CRS_USER.V_HOSTAGE H,
                                 CRS_USER.V_LAST_CURRENCY_RATE CR,
                                 CRS_USER.V_LAST_CONTRACT_EXTEND CE
                           WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                                 AND CON.CUSTOMER_ID = CUS.ID
                                 AND CON.CONTRACT_ID = H.CONTRACT_ID
                                 AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                 AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                 AND CON.CURRENCY_ID = CR.CURRENCY_ID(+)
                                 AND CON.CONTRACT_ID = CE.CONTRACT_ID(+)
                                 AND LT.DELAYS != 0";

            PortfelGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPortfelDataGridView");
        }

        private void LoadMonthlyDataGridView()
        {
            string status = null, delays = null;

            if (ActiveContractCheck.Checked && !PassiveContractCheck.Checked)
                status = $@" AND CON.STATUS_ID = 5";
            else if (!ActiveContractCheck.Checked && PassiveContractCheck.Checked)
                status = $@" AND CON.STATUS_ID = 6";

            if (DelaysCheck.Checked)
                delays = " AND LT.DELAYS > 0";

            string s = $@"SELECT CON.CUSTOMER_ID,
                               CON.CONTRACT_ID,
                               NVL (COM.COMMITMENT_NAME, CUS.FULLNAME) CUSTOMER_NAME,
                               CON.CONTRACT_CODE,
                               CON.AMOUNT,
                               CON.CURRENCY_CODE,
                               H.HOSTAGE_TYPE,
                               H.SHORT_HOSTAGE HOSTAGE,
                               LT.DEBT,
                               CON.MONTHLY_AMOUNT,
                               (CASE WHEN CON.END_DATE < TRUNC (SYSDATE) THEN 1 ELSE 0 END) IS_OLD,
                               NVL((SELECT SUM(PAYMENT_AMOUNT)
                                  FROM CRS_USER.CUSTOMER_PAYMENTS
                                 WHERE     CONTRACT_ID = CON.CONTRACT_ID
                                       AND PAYMENT_DATE BETWEEN TO_DATE ('{date1From.ToString("dd.MM.yyyy")}', 'DD.MM.YYYY') AND TO_DATE ('{date1To.ToString("dd.MM.yyyy")}', 'DD.MM.YYYY')), 0)
                                  AMOUNT1,
                               NVL((SELECT SUM(PAYMENT_AMOUNT)
                                  FROM CRS_USER.CUSTOMER_PAYMENTS
                                 WHERE     CONTRACT_ID = CON.CONTRACT_ID
                                       AND PAYMENT_DATE BETWEEN TO_DATE ('{date2From.ToString("dd.MM.yyyy")}', 'DD.MM.YYYY') AND TO_DATE ('{date2To.ToString("dd.MM.yyyy")}', 'DD.MM.YYYY')), 0)
                                  AMOUNT2,
                               NVL((SELECT SUM(PAYMENT_AMOUNT)
                                  FROM CRS_USER.CUSTOMER_PAYMENTS
                                 WHERE     CONTRACT_ID = CON.CONTRACT_ID
                                       AND PAYMENT_DATE BETWEEN TO_DATE ('{date3From.ToString("dd.MM.yyyy")}', 'DD.MM.YYYY') AND TO_DATE ('{date3To.ToString("dd.MM.yyyy")}', 'DD.MM.YYYY')), 0)
                                  AMOUNT3,
                               CON.CREDIT_TYPE_ID,
                               CON.STATUS_ID,
                               LT.DELAYS
                          FROM CRS_USER.LEASING_TOTAL LT,
                               CRS_USER.V_CUSTOMERS CUS,
                               CRS_USER.V_CONTRACTS CON,
                               CRS_USER.V_COMMITMENTS COM,
                               CRS_USER.V_HOSTAGE H,
                               CRS_USER.V_LAST_CURRENCY_RATE CR
                         WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                               AND CON.CUSTOMER_ID = CUS.ID
                               AND CON.CONTRACT_ID = H.CONTRACT_ID
                               AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                               AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                               AND CON.CURRENCY_ID = CR.CURRENCY_ID(+){status}{delays}
                         ORDER BY CON.CONTRACT_CODE DESC";

            MonthlyGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadMonthlyDataGridView");
        }

        private void LoadContractExtend()
        {
            string filter = null;

            if (ExtendFromDateValue.Text.Length > 0 && ExtendToDateValue.Text.Length > 0)
                filter = $@" AND CE.START_DATE BETWEEN TO_DATE('{ExtendFromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ExtendToDateValue.Text}','DD.MM.YYYY')";

            string sql = $@"SELECT CE.ID,
                                     C.CONTRACT_CODE,
                                     CE.START_DATE,
                                     CE.END_DATE,
                                     CE.MONTH_COUNT,
                                     CE.INTEREST,
                                     CE.DEBT,
                                     CE.CURRENT_DEBT,
                                     CE.INTEREST_DEBT,
                                     CE.CHECK_INTEREST_DEBT,
                                     CE.MONTHLY_AMOUNT,
                                     CE.VERSION,
                                     CE.NOTE,
                                     CE.IS_COMMIT,
                                     CE.CONTRACT_ID
                                FROM CRS_USER.CONTRACT_EXTEND CE, CRS_USER.V_CONTRACTS C
                               WHERE CE.CONTRACT_ID = C.CONTRACT_ID{filter}
                            ORDER BY CE.START_DATE, CE.CONTRACT_ID";

            ExtendsGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContractExtend", "Uzadılmış müqavilələr açılmadı.");
        }

        private void LoadContractExtend2()
        {
            string filter = null, status = $@" STATUS_ID IN (5, 6)";

            if (ExtendActiveCheck.Checked && !ExtendCloseCheck.Checked)
                status = $@" STATUS_ID = 5";
            else if (!ActiveContractCheck.Checked && PassiveContractCheck.Checked)
                status = $@" STATUS_ID = 6";

            if (ExtendDate.Text.Length > 0)
                filter = $@" WHERE START_DATE <= TO_DATE('{ExtendDate.Text}','DD.MM.YYYY')";

            string sql = $@"WITH T1
                                 AS (SELECT *
                                       FROM CRS_USER.V_CONTRACTS
                                      {filter}),
                                 T2
                                 AS (SELECT *
                                       FROM CRS_USER.CONTRACT_EXTEND
                                      {filter})
                            SELECT T1.CONTRACT_CODE,
                                   T1.START_DATE,
                                   T1.END_DATE,
                                   T2.START_DATE EXTEND_START,
                                   T2.END_DATE EXTEND_END,
                                   T1.STATUS_ID,
                                   T1.CONTRACT_ID,
                                   T1.CREDIT_TYPE_ID
                              FROM T1 LEFT JOIN T2 ON T1.CONTRACT_ID = T2.CONTRACT_ID WHERE {status}
                            ORDER BY T1.START_DATE, T1.CONTRACT_ID";

            ExtendGridControl2.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContractExtend2", "Uzadılmış müqavilələr açılmadı.");
        }

        private void LoadContractMonthDebt()
        {
            string sql = $@"SELECT C.CONTRACT_ID,
                                   C.CONTRACT_CODE,
                                   C.START_DATE,
                                   C.END_DATE,
                                   C.AMOUNT,
                                   C.INTEREST,
                                   C.MONTHLY_AMOUNT,
                                   C.CURRENCY_CODE,
                                   D.DEBT,
                                   D.MONTH_END_DATE
                              FROM CRS_USER.V_CONTRACTS C,
                                   CRS_USER.CONTRACT_DEBT_BY_MONTH D
                             WHERE     C.CONTRACT_ID = D.CONTRACT_ID
                                   AND C.STATUS_ID = 5
                                   AND C.START_DATE BETWEEN TO_DATE ('{DebtFromDate.Text}', 'DD.MM.YYYY') AND TO_DATE ('{DebtToDate.Text}', 'DD.MM.YYYY')";

            ContractDebtPivotGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadContractMonthDebt", "Müqavilələrin aylar üzrə faktiki qalıqları yüklənmədi.");
        }

        private void DelaysGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Delays_SS, e);
        }

        private void RefreshDelaysBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadDelyasDataGridView();
        }

        private void ReportsBackstageViewControl_SelectedTabChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            LoadSelectedPage();
        }

        private void PrintDelaysBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(DelaysGridControl);
        }

        private void ExportDelaysBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DelaysGridControl, "xls");
        }

        private void DelaysGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(DelaysGridView, DelaysPopupMenu, e);
        }

        private void InterestGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Interest_SS, e);
        }

        private void InterestGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Interest_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell(Interest_Value, "Center", e);
        }

        private void PeriodGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Period_SS, e);
        }

        private void PeriodGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Period_SS, "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell(Period_Value, "Center", e);
        }

        private void InterestGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
                return;

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST") == 0 && currentView.RowCount > 0)
                    e.TotalValue = "YEKUN";
        }

        private void PeriodGridView_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
                return;

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PERIOD") == 0 && currentView.RowCount > 0)
                    e.TotalValue = "YEKUN";
        }

        private void RefreshInterestBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPortfelWirhInterest();
        }

        private void PrintInterestBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InterestGridControl);
        }

        private void ExportInterestBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InterestGridControl, "xls");
        }

        private void RefreshPeriodBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPortfelWirhPeriod();
        }

        private void PrintPeriodBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PeriodGridControl);
        }

        private void ExportPeriodBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PeriodGridControl, "xls");
        }

        private void InterestGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InterestGridView, InterestPopupMenu, e);
        }

        private void PeriodGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PeriodGridView, PeriodPopupMenu, e);
        }

        private void PortfelGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Portfel_SS, e);
        }

        private void PortfelGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }

            if (e.Column.FieldName == "DELAYS" || e.Column.FieldName == "REQUIRED")
            {
                int isOld = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "IS_OLD"));
                if (isOld == 1)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.OrangeRed;
            }
        }

        private void PortfelGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PortfelGridView, PortfelPopupMenu, e);
        }

        private void PrintPortfelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PortfelGridControl);
        }

        private void ExcelPortfelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PortfelGridControl, "xls");
        }

        private void backstageViewTabItem3_SelectedChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {

        }

        private void MonthlyGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Monthly_SS, e);
        }

        private void RefreshMonthlyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadMonthlyDataGridView();
        }

        private void PrintMonthlyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(MonthlyGridControl);
        }

        private void MonthlyGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = MonthlyGridView.GetFocusedDataRow();
            if (row != null)
            {
                contractID = row["CONTRACT_ID"].ToString();
            }
        }

        private void ExportMonthlyBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(MonthlyGridControl, "xls");
        }

        private void ActiveContractCheck_CheckedChanged(object sender, EventArgs e)
        {
            LoadMonthlyDataGridView();
        }

        private void ExtendsGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (e.Column == Extend_CheckInterestDebt)
            {
                if (Convert.ToInt32(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "CHECK_INTEREST_DEBT").ToString()) == 1)
                    e.Value = "Bəli";
                else
                    e.Value = "Xeyr";
            }

            if (e.Column == Extend_Debt)
            {
                decimal current = Convert.ToDecimal(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "CURRENT_DEBT").ToString());
                decimal interest = Convert.ToDecimal(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "INTEREST_DEBT").ToString());
                int check = Convert.ToInt32(ExtendsGridView.GetListSourceRowCellValue(rowIndex, "CHECK_INTEREST_DEBT").ToString());
                e.Value = (check == 1) ? current + interest : current;
            }

            GlobalProcedures.GenerateAutoRowNumber(sender, Extend_SS, e);
        }

        private void RefreshContractExtendBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractExtend();
        }

        private void PrintContractExtendBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ExtendsGridControl);
        }

        private void ExportContractExtendBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ExtendsGridControl, "xls");
        }

        private void ExtendsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ExtendsGridView, ContractExtendPopupMenu, e);
        }

        private void ExtendFromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ExtendToDateValue.Properties.MinValue = ExtendFromDateValue.DateTime;
            LoadContractExtend();
        }

        private void ExtendDate_EditValueChanged(object sender, EventArgs e)
        {
            LoadContractExtend2();
        }

        private void ExtendGridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Extend2_SS, e);
        }

        private void RefreshExtendBarButton2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractExtend2();
        }

        private void PrintExtendBarButton2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(ExtendGridControl2);
        }

        private void ExportExtendBarButton2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(ExtendGridControl2, "xls");
        }

        private void ExtendGridView2_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(ExtendGridView2, Extend2PopupMenu, e);
        }

        private void ExtendGridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }

            int statusID = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "STATUS_ID"));
            if (statusID == 6)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor2);
            }

        }

        private void RefreshContractDebtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadContractMonthDebt();
        }

        private void PrintContractDebtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowPivotGridPreview(ContractDebtPivotGridControl);
        }

        private void ExportContractDebtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.PivotGridExportToFile(ContractDebtPivotGridControl, "xls");
        }

        private void DebtFromDate_EditValueChanged(object sender, EventArgs e)
        {
            DebtToDate.Properties.MinValue = DebtFromDate.DateTime;
            LoadContractMonthDebt();
        }

        private void ShowPaymentListBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = PortfelGridView.TopRowIndex;
            old_row_num = PortfelGridView.FocusedRowHandle;
            FShowPayments fsp = new FShowPayments();
            fsp.ContractID = contractID;
            fsp.ShowDialog();
            PortfelGridView.TopRowIndex = topindex;
            PortfelGridView.FocusedRowHandle = old_row_num;
        }

        private void PortfelGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PortfelGridView.GetFocusedDataRow();
            if (row != null)
                contractID = row["CONTRACT_ID"].ToString();
        }

        private void ShowPaymentsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = MonthlyGridView.TopRowIndex;
            old_row_num = MonthlyGridView.FocusedRowHandle;
            FShowPayments fsp = new FShowPayments();
            fsp.ContractID = contractID;
            fsp.ShowDialog();
            MonthlyGridView.TopRowIndex = topindex;
            MonthlyGridView.FocusedRowHandle = old_row_num;
        }

        private void MonthlyGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(MonthlyGridView, MonthlyPopupMenu, e);
        }

        private void MonthlyGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            decimal monthlyAmount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "MONTHLY_AMOUNT")), amount = 0;

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }

            if (e.Column.FieldName == "AMOUNT1")
            {
                amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "AMOUNT1"));
                if (amount >= monthlyAmount)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.LightGoldenrodYellow;
                else
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.MistyRose;
            }

            if (e.Column.FieldName == "AMOUNT2")
            {
                amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "AMOUNT2"));
                if (amount >= monthlyAmount)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.LightGoldenrodYellow;
                else
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.MistyRose;
            }

            if (e.Column.FieldName == "AMOUNT3")
            {
                amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "AMOUNT3"));
                if (amount >= monthlyAmount)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.LightGoldenrodYellow;
                else
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.MistyRose;
            }

            if (e.Column == Monthly_SS || e.Column == Monthly_CustomerName || e.Column == Monthly_ContractCode || e.Column == Monthly_Amount || e.Column == Monthly_Debt || e.Column == Monthly_Hostage || e.Column == Monthly_CurrencyCode)
            {
                int statusID = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "STATUS_ID"));
                if (statusID == 6)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor1);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor2);
                }
            }
        }

        private void GenerateDate()
        {
            if (YearComboBox.SelectedIndex < 0)
                return;

            if (MonthComboBox.SelectedIndex < 0)
                return;

            int month = 0, monthLastDay = 0, year = Convert.ToInt16(YearComboBox.SelectedItem);

            if (YearComboBox.SelectedIndex > 0 && MonthComboBox.SelectedIndex > 0)
            {
                month = MonthComboBox.SelectedIndex;
                monthLastDay = DateTime.DaysInMonth(Convert.ToInt16(YearComboBox.SelectedItem), month);
            }

            MonthComboBox.Enabled = (year > 0);

            date1From = new DateTime(year, month, 1);
            date1To = new DateTime(year, month, monthLastDay);

            date2From = date1From.AddMonths(-1);
            date2To = date1To.AddMonths(-1);

            date3From = date1From.AddMonths(-2);
            date3To = date1To.AddMonths(-2);

            LoadMonthlyDataGridView();

            Monthly_Amount1.Caption = GlobalFunctions.FindMonth(date1From.Month).ToUpper() + " / " + date1From.Year;
            Monthly_Amount2.Caption = GlobalFunctions.FindMonth(date2From.Month).ToUpper() + " / " + date2From.Year;
            Monthly_Amount3.Caption = GlobalFunctions.FindMonth(date3From.Month).ToUpper() + " / " + date3From.Year;
        }

        private void YearComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateDate();
        }

        private void MonthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateDate();
        }
    }
}