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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;

namespace CRS.Forms.Contracts
{
    public partial class FInsuranceDebitor : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceDebitor()
        {
            InitializeComponent();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadInsuranceDataGridView();
        }

        private void FInsuranceDebitor_Load(object sender, EventArgs e)
        {
            SearchDockPanel.Hide();
            LoadInsuranceDataGridView();
        }

        private void LoadInsuranceDataGridView()
        {
            string last = (LastCheck.Checked) ? $@" AND I.ID = (SELECT MAX (ID)
                                                       FROM CRS_USER.INSURANCES
                                                      WHERE CONTRACT_ID = I.CONTRACT_ID)" : null,
                   again = (AgainCheck.Checked) ? $@" AND I.IS_AGAIN = 1" : null,
                   cancel = (CancelCheck.Checked) ? $@" AND I.IS_CANCEL = 1" : null,
                   status = null,
                   police = null;

            if (ClosedCheck.Checked && !ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 6";
            else if (!ClosedCheck.Checked && ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 5";

            if (NotPoliceEqualCheck.Checked && !PoliceEqualCheck.Checked)
                police = "WHERE T1.POLICE IS NULL OR T2.POLICE IS NULL";
            else if (!NotPoliceEqualCheck.Checked && PoliceEqualCheck.Checked)
                police = "WHERE T1.POLICE IS NOT NULL AND T2.POLICE IS NOT NULL";

            string s = $@"WITH T1
                             AS (SELECT C.CONTRACT_CODE,
                                          ROUND (I.INSURANCE_AMOUNT * I.INSURANCE_INTEREST / 100, 2)
                                        - NVL (ST.TRANSFER_AMOUNT, 0)
                                        - NVL (ST.COMPENSATION, 0)
                                           TRANSFER_DEBT,
                                        I.START_DATE,
                                        I.END_DATE,
                                        I.POLICE,
                                        H.SHORT_HOSTAGE HOSTAGE
                                   FROM CRS_USER.INSURANCES I,
                                        CRS_USER.V_CONTRACTS C,
                                        CRS_USER.V_SUM_INSURANCE_TRANSFER ST,
                                        CRS_USER.V_HOSTAGE H
                                  WHERE     I.CONTRACT_ID = C.CONTRACT_ID
                                        AND I.ID = ST.INSURANCE_ID(+){last}{again}{cancel}{status}
                                        AND I.CONTRACT_ID = H.CONTRACT_ID),
                             T2 AS (SELECT * FROM CRS_USER_TEMP.INSURANCE_DEBITOR_TEMP)
                        SELECT T1.CONTRACT_CODE,T1.TRANSFER_DEBT,T1.START_DATE,T1.END_DATE,T1.POLICE,T1.HOSTAGE,T2.POLICE POLICE2,T2.AMOUNT, NVL(T1.TRANSFER_DEBT, 0) - NVL(T2.AMOUNT, 0) DIFF
                          FROM T1 FULL JOIN T2 ON T1.POLICE = T2.POLICE {police}
                        ORDER BY T1.CONTRACT_CODE";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceDataGridView", "Sığorta borclarının siyahısı yüklənmədi.");
            InsuranceGridControl.DataSource = dt;
        }

        private void FInsuranceDebitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.INSURANCE_DEBITOR_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID}", "Sğorta borclarının siyahısı temp cədvəldən silinmədi.", this.Name + "/FInsuranceDebitor_FormClosing");
        }

        private void InsuranceGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Insurance_SS, e);
        }

        private void InsuranceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InsuranceGridView, PopupMenu, e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InsuranceGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InsuranceGridControl, "xls");
        }

        private void InsuranceGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            decimal debt = 0, amount = 0;
            GridView currentView = sender as GridView;

            if (currentView.GetRowCellValue(e.RowHandle, "TRANSFER_DEBT") != DBNull.Value)
                debt = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "TRANSFER_DEBT"));
            if (currentView.GetRowCellValue(e.RowHandle, "AMOUNT") != DBNull.Value)
                amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "AMOUNT"));

            if ((debt != 0 || amount != 0) && debt != amount)
                e.Appearance.BackColor = e.Appearance.BackColor2 = Color.Khaki;
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

        private void LastCheck_CheckedChanged(object sender, EventArgs e)
        {
            LoadInsuranceDataGridView();
            GlobalProcedures.ChangeCheckStyle((sender as CheckEdit));
        }

        private void DebtCheck_CheckedChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            ColumnView view = InsuranceGridView;

            if (DebtCheck.Checked)
                view.ActiveFilter.Add(view.Columns["TRANSFER_DEBT"],
              new ColumnFilterInfo("[TRANSFER_DEBT] > 0", "Sistemdə olan borc> 0"));
            else
                view.ActiveFilter.Remove(view.Columns["TRANSFER_DEBT"]);            

            if (PoliceCheck.Checked)
                view.ActiveFilter.Add(view.Columns["POLICE"],
              new ColumnFilterInfo("[POLICE] IS NULL", "Polisdəki qeydiyyatı = null"));
            else
                view.ActiveFilter.Remove(view.Columns["POLICE"]);

            if (DiffCheck.Checked)
                view.ActiveFilter.Add(view.Columns["DIFF"],
              new ColumnFilterInfo("[DIFF] != 0", "Borcu fərqli olanlar> 0"));
            else
                view.ActiveFilter.Remove(view.Columns["DIFF"]);
        }

        private void InsuranceGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Insurance_SS, "Center", e);
        }
    }
}