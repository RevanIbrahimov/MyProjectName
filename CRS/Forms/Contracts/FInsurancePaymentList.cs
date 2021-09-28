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

namespace CRS.Forms.Contracts
{
    public partial class FInsurancePaymentList : DevExpress.XtraEditors.XtraForm
    {
        public FInsurancePaymentList()
        {
            InitializeComponent();
        }

        private void FInsurancePaymentList_Load(object sender, EventArgs e)
        {
            LoadPayment();
        }

        private void LoadPayment()
        {
            string filterDate = null, status = null;

            if (ClosedCheck.Checked && !ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 6";
            else if (!ClosedCheck.Checked && ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 5";

            if (FromDateValue.Text.Length > 0 && ToDateValue.Text.Length > 0)
                filterDate = $@" AND IP.PAY_DATE BETWEEN TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}','DD.MM.YYYY')";

            string sql = $@"SELECT IP.ID,
                                     C.CONTRACT_CODE,
                                     IC.NAME COMPANY_NAME,
                                     I.INSURANCE_AMOUNT,
                                     I.INSURANCE_PERIOD,
                                     I.INSURANCE_INTEREST,
                                     IP.PAY_DATE,
                                     IP.PAYED_AMOUNT,
                                     IP.NOTE,
                                     H.HOSTAGE,
                                     C.STATUS_ID,
                                     I.POLICE,
                                     I.AMOUNT
                                FROM CRS_USER.INSURANCE_PAYMENT IP,
                                     CRS_USER.INSURANCES I,
                                     CRS_USER.V_CONTRACTS C,
                                     CRS_USER.INSURANCE_COMPANY IC,
                                     CRS_USER.V_HOSTAGE H
                               WHERE     IP.INSURANCE_ID = I.ID
                                     AND I.CONTRACT_ID = C.CONTRACT_ID
                                     AND C.CONTRACT_ID = H.CONTRACT_ID
                                     AND I.COMPANY_ID = IC.ID{filterDate}{status}
                            ORDER BY ID";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPayment", "Sığortaların ödənişlərinin siyahısı yüklənmədi.");
            InsuranceGridControl.DataSource = dt;
        }

        private void InsuranceGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Insurance_SS, e);
        }

        private void InsuranceGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Insurance_SS, "Center", e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPayment();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InsuranceGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InsuranceGridControl, "xls");
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            LoadPayment();
        }

        private void InsuranceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InsuranceGridView, PopupMenu, e);
        }

        private void InsuranceGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void InsuranceGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForClose(6, InsuranceGridView, e);
        }
    }
}