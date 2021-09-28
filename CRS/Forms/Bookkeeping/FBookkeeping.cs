using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class;
using DevExpress.XtraSplashScreen;

namespace CRS.Forms.Bookkeeping
{
    public partial class FBookkeeping : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FBookkeeping()
        {
            InitializeComponent();
        }
        int AccountNumber, old_row_num = 0;
        bool FormStatus = false;
        string account = null, AccountName;

        private void AccountingBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FAccountingPlan fap = new FAccountingPlan();
            fap.RefreshAccountList += new FAccountingPlan.DoEvent(RefreshOperations);
            fap.ShowDialog();
        }

        void RefreshOperations()
        {
            LoadOperations();
            GlobalProcedures.FillCheckedComboBoxWithSqlText(AccountComboBox, "SELECT DISTINCT ACCOUNT_NAME FROM (SELECT AT.SHORT_NAME||': '||AP.ACCOUNT_NUMBER||' - '||AP.ACCOUNT_NAME ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN AP,CRS_USER.ACCOUNT_TYPE AT WHERE AP.ACCOUNT_TYPE_ID = AT.ID ORDER BY 1) ORDER BY 1");            
        }

        private void FBookkeeping_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                if (GlobalVariables.AddJournal || GlobalVariables.EditJournal || GlobalVariables.DeleteJournal)
                    JournalBarButton.Enabled = true;
                else
                    JournalBarButton.Enabled = GlobalVariables.ShowJournal;

                DetailsBarButton.Enabled = GlobalVariables.ShowDetails;

                if (GlobalVariables.AddAccountPlan || GlobalVariables.EditAccountPlan || GlobalVariables.DeleteAccountPlan)
                    AccountingBarButton.Enabled = true;
                else
                    AccountingBarButton.Enabled = GlobalVariables.ShowAccountPlan;

                if (GlobalVariables.AddDebt || GlobalVariables.EditDebt || GlobalVariables.DeleteDebt)
                    AccountDebtsBarButton.Enabled = true;
                else
                    AccountDebtsBarButton.Enabled = GlobalVariables.ShowDebt;

                ProfitsBarButton.Enabled = GlobalVariables.ShowProfits;
            }
            
            FromDateValue.EditValue = DateTime.Today;
            OperationsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalına olan dövriyyə";
            GlobalProcedures.FillCheckedComboBoxWithSqlText(AccountComboBox, "SELECT DISTINCT ACCOUNT_NAME FROM (SELECT AT.SHORT_NAME||': '||AP.ACCOUNT_NUMBER||' - '||AP.ACCOUNT_NAME ACCOUNT_NAME FROM CRS_USER.ACCOUNTING_PLAN AP,CRS_USER.ACCOUNT_TYPE AT WHERE AP.ACCOUNT_TYPE_ID = AT.ID ORDER BY 1) ORDER BY 1");
            FormStatus = true;
        }

        private void LoadOperations()
        {
            string s = $@"SELECT 1 SS,
                                 AP.ACCOUNT_NUMBER,
                                 AP.ACCOUNT_NAME,
                                 NVL ( (CASE WHEN T.T1D - T.T1C < 0 THEN 0 ELSE T.T1D - T.T1C END), 0)
                                    T1D,
                                 NVL ( (CASE WHEN T.T1C - T.T1D < 0 THEN 0 ELSE T.T1C - T.T1D END), 0)
                                    T1C,
                                 NVL (T.T2D, 0) T2D,
                                 NVL (T.T2C, 0) T2C,
                                 (CASE
                                     WHEN   (NVL (T.T1D, 0) + NVL (T.T2D, 0))
                                          - (NVL (T.T1C, 0) + NVL (T.T2C, 0)) < 0
                                     THEN
                                        0
                                     ELSE
                                          (NVL (T.T1D, 0) + NVL (T.T2D, 0))
                                        - (NVL (T.T1C, 0) + NVL (T.T2C, 0))
                                  END)
                                    DDEBT,
                                 (CASE
                                     WHEN   (NVL (T.T1C, 0) + NVL (T.T2C, 0))
                                          - (NVL (T.T1D, 0) + NVL (T.T2D, 0)) < 0
                                     THEN
                                        0
                                     ELSE
                                          (NVL (T.T1C, 0) + NVL (T.T2C, 0))
                                        - (NVL (T.T1D, 0) + NVL (T.T2D, 0))
                                  END)
                                    CDEBT,
                                    DECODE (AP.ACCOUNT_TYPE_ID, 1, 'A', 'P')
                                 || ': '
                                 || AP.ACCOUNT_NUMBER
                                 || ' - '
                                 || AP.ACCOUNT_NAME
                                    ACCOUNTNAME
                            FROM (WITH T1
                                       AS (  SELECT SUBSTR (ACCOUNT, 0, 3) ACCOUNT_NUMBER,
                                                    NVL(SUM (DEBIT), 0) T1D,
                                                    NVL(SUM (CREDIT), 0) T1C
                                               FROM (SELECT ACCOUNT,
                                                            DEBT_DATE OPERATION_DATE,
                                                            DEBIT,
                                                            CREDIT
                                                       FROM CRS_USER.OPERATION_DEBT
                                                      WHERE YR_MNTH_DY = 20151231
                                                     UNION ALL
                                                     SELECT *
                                                       FROM (SELECT DEBIT_ACCOUNT ACCOUNT,
                                                                    AMOUNT_AZN,
                                                                    OPERATION_DATE,
                                                                    1 TYPE
                                                               FROM CRS_USER.OPERATION_JOURNAL
                                                             UNION ALL
                                                             SELECT CREDIT_ACCOUNT ACCOUNT,
                                                                    AMOUNT_AZN,
                                                                    OPERATION_DATE,
                                                                    2 TYPE
                                                               FROM CRS_USER.OPERATION_JOURNAL) PIVOT (SUM (
                                                                                                          AMOUNT_AZN)
                                                                                                FOR TYPE
                                                                                                IN  (1 AS DEBIT,
                                                                                                    2 AS CREDIT)))
                                              WHERE OPERATION_DATE < TO_DATE ('{FromDateValue.Text}', 'DD.MM.YYYY')
                                           GROUP BY SUBSTR (ACCOUNT, 0, 3)),
                                       T2
                                       AS (  SELECT ACCOUNT_NUMBER, NVL(SUM (DEBIT), 0) T2D, NVL(SUM (CREDIT), 0) T2C
                                               FROM (SELECT OJ.OPERATION_DATE,
                                                            SUBSTR (OJ.DEBIT_ACCOUNT, 0, 3)
                                                               ACCOUNT_NUMBER,
                                                            1 AMOUNT_TYPE,
                                                            OJ.AMOUNT_AZN AMOUNT
                                                       FROM CRS_USER.OPERATION_JOURNAL OJ
                                                     UNION ALL
                                                     SELECT OJ.OPERATION_DATE,
                                                            SUBSTR (OJ.CREDIT_ACCOUNT, 0, 3)
                                                               ACCOUNT_NUMBER,
                                                            2 AMOUNT_TYPE,
                                                            OJ.AMOUNT_AZN AMOUNT
                                                       FROM CRS_USER.OPERATION_JOURNAL OJ) PIVOT (SUM (
                                                                                                     AMOUNT)
                                                                                           FOR AMOUNT_TYPE
                                                                                           IN  (1 AS DEBIT,
                                                                                               2 AS CREDIT))
                                              WHERE OPERATION_DATE BETWEEN TO_DATE ('{FromDateValue.Text}',
                                                                                    'DD.MM.YYYY')
                                                                       AND TO_DATE ('{ToDateValue.Text}',
                                                                                    'DD.MM.YYYY')
                                           GROUP BY ACCOUNT_NUMBER)
                                  SELECT NVL (T2.ACCOUNT_NUMBER, T1.ACCOUNT_NUMBER) ACCOUNT_NUMBER,
                                         T1.T1D,
                                         T1C,
                                         T2D,
                                         T2C
                                    FROM T1 FULL JOIN T2 ON T1.ACCOUNT_NUMBER = T2.ACCOUNT_NUMBER) T
                                 RIGHT JOIN
                                 (SELECT DISTINCT ACCOUNT_NUMBER, ACCOUNT_NAME, ACCOUNT_TYPE_ID
                                    FROM CRS_USER.ACCOUNTING_PLAN) AP
                                    ON T.ACCOUNT_NUMBER = AP.ACCOUNT_NUMBER {(!(AgreementCheck.Checked)? " WHERE T.ACCOUNT_NUMBER != 222 " : null)}
                        ORDER BY AP.ACCOUNT_NUMBER";

            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperations");

                OperationsGridControl.DataSource = dt;

                if (OperationsGridView.RowCount > 0)
                {
                    if (GlobalVariables.V_UserID > 0)
                        DetailsBarButton.Enabled = GlobalVariables.ShowDetails;
                    else
                        DetailsBarButton.Enabled = true;
                }
                else
                    DetailsBarButton.Enabled = false;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Əməliyyatlar cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadOperations();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.EditValue = new DateTime(FromDateValue.DateTime.Year, 12, 31);
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            ToDateValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            if (FormStatus)
            {
                OperationsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalına olan dövriyyə";
                LoadOperations();
                FilterOperations();
            }
        }

        private void OperationsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void OperationsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = OperationsGridView.GetFocusedDataRow();
            if (row != null)
            {
                AccountNumber = Convert.ToInt32(row["ACCOUNT_NUMBER"].ToString());
                AccountName = row["ACCOUNT_NAME"].ToString();
            }
        }

        private void OperationsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("T1D", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("T1C", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("T2D", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("T2C", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DDEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("CDEBT", "Far", e);
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void OperationsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(OperationsGridView, PopupMenu, e);
        }

        private void AccountComboBox_EditValueChanged(object sender, EventArgs e)
        {
            account = " [ACCOUNTNAME] IN ('" + AccountComboBox.Text.Replace("; ", "','") + "')";
            FilterOperations();
        }

        private void FilterOperations()
        {
            if (FormStatus)
            {
                ColumnView view = OperationsGridView;

                if (!String.IsNullOrEmpty(AccountComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["ACCOUNTNAME"],
                        new ColumnFilterInfo(account, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["ACCOUNTNAME"]);
            }
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(OperationsGridControl);
        }

        private void ExcellBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "mht");
        }

        private void FilterClearBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            AccountComboBox.Text = null;
            FromDateValue.EditValue = DateTime.Today.AddDays(-30);
            ToDateValue.EditValue = DateTime.Today;
            OperationsGridView.ClearColumnsFilter();
        }

        private void LoadFOperationDetails()
        {
            int topindex = OperationsGridView.TopRowIndex;
            old_row_num = OperationsGridView.FocusedRowHandle;
            FOperationDetails fod = new FOperationDetails();
            fod.AccountNumber = AccountNumber.ToString();
            fod.AccountName = AccountNumber.ToString() + " - " + AccountName;
            fod.FromDate = FromDateValue.Text;
            fod.ToDate = ToDateValue.Text;
            fod.RefreshOperationsDataGridView += new FOperationDetails.DoEvent(RefreshOperations);
            fod.ShowDialog();
            OperationsGridView.FocusedRowHandle = old_row_num;
            OperationsGridView.TopRowIndex = topindex;
        }

        private void DetailsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFOperationDetails();
        }

        private void OperationsGridView_DoubleClick(object sender, EventArgs e)
        {
            LoadFOperationDetails();
        }

        private void FBookkeeping_Activated(object sender, EventArgs e)
        {            
            LoadOperations();

            GlobalProcedures.GridRestoreLayout(OperationsGridView, BookkeepingRibbonPage.Text);
        }

        private void JournalBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FJournalShowWait));            
            FJournal fj = new FJournal();
            fj.RefreshOperationsDataGridView += new FJournal.DoEvent(RefreshOperations);
            fj.ShowDialog();
            //GlobalProcedures.SplashScreenClose();
        }

        private void FromDateValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (FormStatus)
                {
                    OperationsGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " tarix intervalınaolan dövriyyə";
                    LoadOperations();
                    FilterOperations();
                }
            }
        }

        private void AccountDebtsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FAccountDebt fad = new FAccountDebt();
            fad.RefreshAccountList += new FAccountDebt.DoEvent(RefreshOperations);
            fad.ShowDialog();
        }

        private void OperationsGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void AgreementCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle(AgreementCheck);
            LoadOperations();
        }

        private void VacationDatesBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FPersonneıVacationDates vd = new FPersonneıVacationDates();
            vd.ShowDialog();
        }

        private void OperationsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (OperationsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                    DetailsBarButton.Enabled = GlobalVariables.ShowDetails;
                else
                    DetailsBarButton.Enabled = true;
            }
            else
                DetailsBarButton.Enabled = false;
        }

        private void CustomerPercentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FCustomerNormalPercent fc = new FCustomerNormalPercent();
            fc.ShowDialog();
        }

        private void OperationsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(OperationsGridView, BookkeepingRibbonPage.Text);
        }

        private void ProfitsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FProfits fp = new FProfits();
            fp.ShowDialog();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }
    }
}