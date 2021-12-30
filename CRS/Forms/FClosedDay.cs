using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using CRS.Class;
using DevExpress.XtraEditors;

namespace CRS.Forms
{
    public partial class FClosedDay : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FClosedDay()
        {
            InitializeComponent();
        }
        int id;

        private void FClosedDay_Load(object sender, EventArgs e)
        {
            LoadDays();
        }

        private void LoadDays()
        {
            string sql = $@"SELECT ID,
                                   CLOSED_DAY,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = INSERT_USER)
                                      INSERT_USER,
                                   INSERT_DATE
                              FROM CRS_USER.CLOSED_DAYS
                            ORDER BY CLOSED_DAY DESC";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDays", "Bağlanılmış günlərin siyahısı yüklənmədi.");
            DaysGridControl.DataSource = dt;

            DeleteBarButton.Enabled = DaysGridView.RowCount > 0 ? GlobalVariables.DeleteClosedDay : false;

            if (dt.Rows.Count > 0)
            {
                object objLastClosedDay = dt.Compute("MAX(CLOSED_DAY)", null);
                GlobalVariables.V_LastClosedDay = Convert.ToDateTime(objLastClosedDay);
            }
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadDays();
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(DaysGridControl);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DaysGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DaysGridControl, "pdf");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DaysGridControl, "txt");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DaysGridControl, "html");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DaysGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DaysGridControl, "mht");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DaysGridControl, "rtf");
        }

        private void DaysGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Days_SS, e);
        }

        private void DaysGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(DaysGridView, PopupMenu, e);
        }

        private void ClosedDayBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Bookkeeping.FClosedDay fc = new Bookkeeping.FClosedDay();
            fc.RefreshDataGridView += new Bookkeeping.FClosedDay.DoEvent(LoadDays);
            fc.ShowDialog();
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş bağlanılmış günü silmək istəyirsiniz?", "Bağlanılmış günün silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_CLOSED_DAY", "P_ID", id, "Bağlanılmış gün bazadan silinmədi.");
            LoadDays();
        }

        private void DaysGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = DaysGridView.GetFocusedDataRow();
            if (row != null)
                id = Convert.ToInt32(row["ID"].ToString());
        }

        private void DaysGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            DeleteBarButton.Enabled = DaysGridView.RowCount > 0 ? GlobalVariables.DeleteClosedDay : false;
        }
    }
}