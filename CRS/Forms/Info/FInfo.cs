using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS.Forms.Info
{
    public partial class FInfo : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FInfo()
        {
            InitializeComponent();
        }
        int pay_alert = 3;

        private void FInfo_Load(object sender, EventArgs e)
        {
            CreateSlideLabel();
            SlideTimer.Interval = (100) * (1);              // Timer will tick evert second
            //SlideTimer.Enabled = true;                       // Enable the timer
            //SlideTimer.Start();

            if (pay_alert > 0)//xeberdarliq edilme gunleri yazilarsa            
                PaymentsGridView.ViewCaption = 
                    PaymentPrintBarButton.Caption = 
                    PaymentExportBarButton.Caption= "Qrafik üzrə bu gün və ya növbəti " + pay_alert.ToString() + " gün ərzində ödəniş etməli olan müştərilər";                            
        }

        private void CreateSlideLabel()
        {
            //GlobalProcedures.GenerateRateText();
            SlideLabel.Text = GlobalVariables.V_LastRate;
        }

        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            SlideLabel.Location = new Point(SlideLabel.Location.X + 5, SlideLabel.Location.Y);

            if (SlideLabel.Location.X + 5 > SlidePanel.Width)
            {
                SlideLabel.Location = new Point(50 - SlideLabel.Width, SlideLabel.Location.Y);
            }
        }

        void RefreshSlide()
        {
            GlobalProcedures.GenerateRateText();
            CreateSlideLabel();
        }

        private void CurrencyRatesBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FExchanges ex = new FExchanges();
            ex.RefreshSlideLabel += new FExchanges.DoEvent(RefreshSlide);
            ex.ShowDialog();
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshSlide();
            LoadPaymentsDataGridView();
            LoadDelyassDataGridView();
            LoadInsuranceDataGridView();
            LoadLawsDataGridView();
        }

        private void LoadPaymentsDataGridView()
        {
            string s = $@"SELECT C.FULLNAME CUSTOMERFULLNAME,
                                 CON.CONTRACT_CODE,
                                 PS.MONTH_DATE PDATE,
                                 ROUND (PS.MONTHLY_PAYMENT, 2) AMOUNT,
                                 CON.CURRENCY_CODE CUR_CODE,
                                 CON.CREDIT_TYPE_ID,
                                 CON.CONTRACT_ID 
                            FROM CRS_USER.PAYMENT_SCHEDULES PS,
                                 CRS_USER.V_CUSTOMERS C,                                 
                                 CRS_USER.V_CONTRACTS CON         
                           WHERE     PS.CONTRACT_ID = CON.CONTRACT_ID 
                                 AND CON.CUSTOMER_ID = C.ID
                                 AND CON.CUSTOMER_TYPE_ID = C.PERSON_TYPE_ID 
                                 AND CON.STATUS_ID = 5
                                 AND TO_CHAR (PS.MONTH_DATE, 'YYYYMMDD') BETWEEN TO_CHAR (SYSDATE,
                                                                                          'YYYYMMDD')
                                                                             AND TO_CHAR (SYSDATE + {pay_alert},
                                                                                          'YYYYMMDD')
                        ORDER BY PS.MONTH_DATE, C.FULLNAME";
            PaymentsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPaymentsDataGridView");
        }

        private void LoadDelyassDataGridView()
        {
            string s = @"SELECT CON.CUSTOMER_ID,
                                       CON.CONTRACT_ID,
                                       NVL(COM.COMMITMENT_NAME, CUS.FULLNAME) CUSTOMER_NAME,
                                       CON.CONTRACT_CODE,
                                       CON.AMOUNT,
                                       CON.CURRENCY_CODE,
                                       LT.DEBT,
                                       CON.MONTHLY_AMOUNT,
                                       LT.REQUIRED,
                                       LT.DELAYS,
                                       CON.CREDIT_TYPE_ID,
                                       LT.DELAYS_DAY_COUNT,
                                       LT.OVERDUE_PERCENT
                                  FROM CRS_USER.LEASING_TOTAL LT,
                                       CRS_USER.V_CUSTOMERS CUS,
                                       CRS_USER.V_CONTRACTS CON,
                                       CRS_USER.V_COMMITMENTS COM
                                 WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                                       AND CON.CUSTOMER_ID = CUS.ID
                                       AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                       AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                       AND LT.DELAYS > 0
                                    ORDER BY CON.CONTRACT_CODE";

            DelaysGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDelyassDataGridView");
        }

        private void LoadInsuranceDataGridView()
        {
            string s = @"SELECT 1 SS,
                                 NVL (COMMITMENT_NAME, CUS_NAME) COMMITMENT_OR_CUSTOMER,
                                 CONTRACT_CODE,
                                 BRAND_NAME || ' - ' || MODEL_NAME BRAND_AND_MODEL,
                                 AMOUNT,
                                 PERIOD,
                                 INTEREST,
                                 UNCONDITIONAL_AMOUNT,
                                 START_DATE,
                                 END_DATE,
                                 CONTROL,
                                 CREDIT_TYPE_ID
                            FROM (SELECT C.CONTRACT_CODE,                 
                                         CUS.CUSTOMER_NAME CUS_NAME,
                                         COM.COMMITMENT_NAME,
                                         I.INSURANCE_AMOUNT || ' ' || CUR.CODE AMOUNT,
                                         I.INSURANCE_PERIOD || ' ay' PERIOD,
                                         I.INSURANCE_INTEREST || ' %' INTEREST,
                                         I.UNCONDITIONAL_AMOUNT || ' ' || CUR.CODE UNCONDITIONAL_AMOUNT,
                                         I.START_DATE,
                                         I.END_DATE,
                                         (CASE
                                             WHEN I.END_DATE < SYSDATE
                                             THEN
                                                2
                                             WHEN I.END_DATE BETWEEN SYSDATE
                                                                 AND ADD_MONTHS (SYSDATE, 1)
                                             THEN
                                                1
                                             ELSE
                                                0
                                          END)
                                            CONTROL,
                                         C.CREDIT_TYPE_ID,
                                         CB.NAME BRAND_NAME,
                                         CM.NAME MODEL_NAME
                                    FROM CRS_USER.INSURANCES I,
                                         CRS_USER.V_CONTRACTS C,
                                         CRS_USER.V_CUSTOMERS CUS,
                                         CRS_USER.CURRENCY CUR,
                                         CRS_USER.HOSTAGE_CAR HC,
                                         CRS_USER.CAR_BRANDS CB,
                                         CRS_USER.CAR_MODELS CM,
                                         CRS_USER.V_COMMITMENTS COM
                                   WHERE     CUS.ID = C.CUSTOMER_ID
                                         AND CUS.PERSON_TYPE_ID = C.CUSTOMER_TYPE_ID
                                         AND I.CURRENCY_ID = CUR.ID
                                         AND I.CONTRACT_ID = C.CONTRACT_ID                 
                                         AND C.CONTRACT_ID = HC.CONTRACT_ID
                                         AND C.CONTRACT_ID = COM.CONTRACT_ID(+)
                                         AND HC.MODEL_ID = CM.ID
                                         AND HC.BRAND_ID = CB.ID
                                         AND C.STATUS_ID = 5
                                         AND I.INSURANCE_AMOUNT > 0
                                         AND I.ID = (SELECT MAX (ID)
                                                       FROM CRS_USER.INSURANCES
                                                      WHERE CONTRACT_ID = C.CONTRACT_ID))
                           WHERE CONTROL <> 0
                        ORDER BY CONTROL DESC, START_DATE";

            InsuranceGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceDataGridView");

        }

        private void LoadLawsDataGridView()
        {
            string s = @"SELECT 1 SS,
                             C.CONTRACT_CODE||' - '||CUS.CUSTOMER_NAME CONTRACT_CODE,
                             CL.APPLICANT,
                             CL.DEFANDANT_NAME,
                             L.NAME LAW_NAME,
                             CL.JUDGE_NAME,
                             CL.START_DATE,
                             LS.NAME STATUS_NAME,
                             (SELECT TO_CHAR (CLS.LAST_DATE, 'DD.MM.YYYY HH24:MI:SS') LAST_DATE
                                FROM CRS_USER.CONTRACT_LAWS CLS
                               WHERE     CLS.CONTRACT_ID = CL.CONTRACT_ID
                                     AND CLS.ID =
                                            (SELECT MAX (ID)
                                               FROM CRS_USER.CONTRACT_LAWS
                                              WHERE CONTRACT_ID = CLS.CONTRACT_ID AND ID < CL.ID))
                                LAST_DATE,
                             TO_CHAR (CL.NEXT_DATE, 'DD.MM.YYYY HH24:MI:SS') NEXT_DATE,
                             CL.NOTE,
                             CL.CREATED_USER_NAME
                        FROM CRS_USER.CONTRACT_LAWS CL,
                             CRS_USER.V_CONTRACTS C,
                             CRS_USER.V_CUSTOMERS CUS,
                             CRS_USER.LAWS L,
                             CRS_USER.LAW_STATUS LS
                       WHERE     CL.CONTRACT_ID = C.CONTRACT_ID
                             AND C.CUSTOMER_ID = CUS.ID
                             AND CL.LAW_ID = L.ID
                             AND CL.LAW_STATUS_ID = LS.ID
                             AND CL.IS_ACTIVE = 1
                             AND TO_CHAR (CL.NEXT_DATE, 'YYYYMMDD') BETWEEN TO_CHAR (SYSDATE,
                                                                                     'YYYYMMDD')
                                                                        AND TO_CHAR (SYSDATE + 7,
                                                                                     'YYYYMMDD')
                             AND CL.ID = (SELECT MAX (ID)
                                            FROM CRS_USER.CONTRACT_LAWS
                                           WHERE CONTRACT_ID = CL.CONTRACT_ID)
                    ORDER BY CL.NEXT_DATE";
            LawsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadLawsDataGridView");
        }

        private void DelaysGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            decimal monthly_amount, delya_value, res;
            GridView currentView = sender as GridView;

            if (e.Column.FieldName == "REQUIRED")
            {
                monthly_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "MONTHLY_AMOUNT"));
                delya_value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DELAYS"));
                res = Math.Round((delya_value / monthly_amount) * 100, 2);
                GlobalProcedures.FindFontDetails(res, e);
            }

            if (e.Column.FieldName == "DELAYS")
            {
                monthly_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "MONTHLY_AMOUNT"));
                delya_value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DELAYS"));
                res = Math.Round((delya_value / monthly_amount) * 100, 2);
                GlobalProcedures.FindFontDetails(res, e);
            }

            if (e.Column.FieldName == "CONTRACT")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }
        }

        private void FInfo_Activated(object sender, EventArgs e)
        {            
            LoadPaymentsDataGridView();
            LoadDelyassDataGridView();
            LoadInsuranceDataGridView();
            LoadLawsDataGridView();

            GlobalProcedures.GridRestoreLayout(PaymentsGridView, "Ödənişlər");
            GlobalProcedures.GridRestoreLayout(DelaysGridView, "Gecikmələr");
            GlobalProcedures.GridRestoreLayout(InsuranceGridView, "Sığotalar");
            GlobalProcedures.GridRestoreLayout(LawsGridView, "Məhkəmələr");
        }

        private void PaymentsGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }
        }

        private void InsuranceGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "START_DATE" || e.Column.FieldName == "END_DATE")
            {
                e.Column.AppearanceHeader.FontStyleDelta = FontStyle.Bold;
                int control = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CONTROL"));
                switch (control)
                {
                    case 2:
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                        break;
                    case 1:
                        e.Appearance.BackColor = Color.Yellow;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                        break;
                }
            }

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }
        }

        private void InsuranceGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void DelaysPrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(DelaysGridControl);
        }

        private void InsurancePrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InsuranceGridControl);
        }

        private void DelaysGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void LawsPrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(LawsGridControl);
        }

        private void PaymentPrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PaymentsGridControl);
        }

        private void PaymentExportBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PaymentsGridControl, "xls");
        }

        private void DelaysExportBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DelaysGridControl, "xls");
        }

        private void InsuranceExportBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InsuranceGridControl, "xls");
        }

        private void LawsExportBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(LawsGridControl, "xls");
        }

        private void PaymentsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(PaymentsGridView, "Ödənişlər");
        }

        private void InfoRibbon_Click(object sender, EventArgs e)
        {

        }

        private void DelaysGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(DelaysGridView, "Gecikmələr");
        }

        private void InsuranceGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(InsuranceGridView, "Sığotalar");
        }

        private void LawsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(LawsGridView, "Məhkəmələr");
        }
    }
}