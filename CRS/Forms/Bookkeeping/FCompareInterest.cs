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
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using System.Collections;

namespace CRS.Forms.Bookkeeping
{
    public partial class FCompareInterest : DevExpress.XtraEditors.XtraForm
    {
        public FCompareInterest()
        {
            InitializeComponent();
        }
        public string fromdate, todate;

        int paymentID;

        private void FCompareInterest_Load(object sender, EventArgs e)
        {
            CompareGridView.ViewCaption = fromdate + " - " + todate + " tarix intervalına olan faiz gəlirlərinin müqayisəsi";
            LoadCompare();
        }

        private void LoadCompare()
        {
            string s = $@"WITH P
                                 AS (SELECT C.CONTRACT_CODE,
                                            CP.CONTRACT_ID,
                                            CP.ID,
                                            CP.PAYMENT_DATE,
                                            CP.PAYMENT_INTEREST_AMOUNT,
                                            CP.CURRENCY_RATE,
                                            ROUND (CP.CURRENCY_RATE * CP.PAYMENT_INTEREST_AMOUNT, 2)
                                               AMOUNT_AZN
                                       FROM CRS_USER.CUSTOMER_PAYMENTS CP,
                                            CRS_USER.V_CONTRACTS C                
                                      WHERE     CP.CONTRACT_ID = C.CONTRACT_ID                
                                            AND CP.PAYMENT_DATE >= TO_DATE ('1/1/2016', 'MM/DD/YYYY')),
                                 J
                                 AS (SELECT J.CONTRACT_ID,
                                            J.CUSTOMER_PAYMENT_ID,
                                            J.OPERATION_DATE,
                                            J.AMOUNT_CUR,
                                            J.CURRENCY_RATE,
                                            J.AMOUNT_AZN
                                       FROM CRS_USER.OPERATION_JOURNAL J
                                      WHERE CREDIT_ACCOUNT LIKE '631%')
                              SELECT P.CONTRACT_CODE,
                                     P.PAYMENT_DATE,
                                     P.PAYMENT_INTEREST_AMOUNT PAMOUNTCUR,
                                     P.CURRENCY_RATE PCURRATE,
                                     P.AMOUNT_AZN PAMOUNTAZN,
                                     J.AMOUNT_CUR JAMOUNTCUR,
                                     J.CURRENCY_RATE JCURRATE,
                                     J.AMOUNT_AZN JAMOUNTAZN,
                                     (CASE WHEN P.AMOUNT_AZN = J.AMOUNT_AZN THEN 1 ELSE 0 END) CONTROL,
                                     J.CUSTOMER_PAYMENT_ID
                                FROM P, J
                               WHERE     J.CONTRACT_ID = P.CONTRACT_ID
                                     AND P.ID = J.CUSTOMER_PAYMENT_ID
                                     AND J.OPERATION_DATE = P.PAYMENT_DATE
                                     AND P.PAYMENT_DATE BETWEEN TO_DATE ('{fromdate}', 'DD/MM/YYYY')
                                                            AND TO_DATE ('{todate}', 'DD/MM/YYYY')
                            ORDER BY P.PAYMENT_DATE";


            try
            {

                CompareGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Faiz gəlirlərinin müqayisəsi cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CompareGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CONTRACT_CODE", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAMOUNTAZN", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAMOUNTCUR", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("JAMOUNTAZN", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("JAMOUNTCUR", "Far", e);
        }

        private void CompareGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCompare();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(CompareGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CompareGridControl, "xls");
        }

        private void CompareGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CompareGridView.GetFocusedDataRow();
            if (row != null)
                paymentID = int.Parse(row["CUSTOMER_PAYMENT_ID"].ToString());
        }

        private void AgainBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ArrayList rows = new ArrayList();
            rows.Clear();

            if (CompareGridView.SelectedRowsCount <= 0)
            {
                XtraMessageBox.Show("Mühasibatlıqda yenidən hesablamaq isdədiyiniz sətri seçin", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < CompareGridView.SelectedRowsCount; i++)
            {
                rows.Add(CompareGridView.GetDataRow(CompareGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_ONE_PAYMENT_INS_JOURNAL", "P_PAYMENT_ID", int.Parse(row["CUSTOMER_PAYMENT_ID"].ToString()), "Seçilmiş müqavilə və tarix üçün lizinq ödənişi mühasibatlıqda təkrar hesablamadı");
            }

            
            LoadCompare();
        }

        private void CompareGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CompareGridView, PopupMenu, e);
        }

        private void FilterCompare()
        {
            ColumnView view = CompareGridView;

            if (YesBarCheck.Checked && !NoBarCheck.Checked)
                view.ActiveFilter.Add(view.Columns["CONTROL"],
                    new ColumnFilterInfo("[CONTROL] = 1", ""));
            else if (!YesBarCheck.Checked && NoBarCheck.Checked)
                view.ActiveFilter.Add(view.Columns["CONTROL"],
                    new ColumnFilterInfo("[CONTROL] = 0", ""));
            else
                view.ActiveFilter.Remove(view.Columns["CONTROL"]);
        }

        private void YesBarCheck_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FilterCompare();
        }

        private void CompareGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                int type = int.Parse(View.GetRowCellDisplayText(e.RowHandle, View.Columns["CONTROL"]));
                if (type == 0)
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.BackColor2 = Color.Red;
                }
                else
                {
                    if (e.Column.FieldName == "JAMOUNTCUR" || e.Column.FieldName == "JCURRATE" || e.Column.FieldName == "JAMOUNTAZN")
                        e.Appearance.BackColor = Color.Yellow;

                    if (e.Column.FieldName == "PAMOUNTCUR" || e.Column.FieldName == "PCURRATE" || e.Column.FieldName == "PAMOUNTAZN")
                        e.Appearance.BackColor = Color.Lime;
                }
            }
        }
    }
}