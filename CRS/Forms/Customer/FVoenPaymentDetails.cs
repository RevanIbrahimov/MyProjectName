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

namespace CRS.Forms.Customer
{
    public partial class FVoenPaymentDetails : DevExpress.XtraEditors.XtraForm
    {
        public FVoenPaymentDetails()
        {
            InitializeComponent();
        }
        public string FromDate, ToDate, CustomerName;
        public int CustomerID, CustomerType;

        int topindex, old_row_num, contractID;

        private void FVoenPaymentDetails_Load(object sender, EventArgs e)
        {
            this.Text = FromDate + " - " + ToDate + " tarix intervalında " + CustomerName + "na məxsuz olan dövriyyə";
            LoadDetails();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadDetails();
        }

        private void CustomersGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void CustomersGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;

            if (e.Column == Customer_Total)
            {
                decimal advance = Convert.ToDecimal(CustomersGridView.GetListSourceRowCellValue(rowIndex, "ADVANCE_PAYMENT"));
                decimal payed = Convert.ToDecimal(CustomersGridView.GetListSourceRowCellValue(rowIndex, "PAYED_AMOUNT"));
                e.Value = advance + payed;
            }
        }

        private void CustomersGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "xls");
        }

        private void CustomersGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CustomersGridView.GetFocusedDataRow();
            if (row != null)
                contractID = int.Parse(row["CONTRACT_ID"].ToString());
        }

        private void CustomersGridView_DoubleClick(object sender, EventArgs e)
        {
            ShowPayment();
        }

        private void CustomersGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CustomersGridView, PopupMenu, e);
        }

        private void PaymentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPayment();
        }

        private void ShowPayment()
        {
            topindex = CustomersGridView.TopRowIndex;
            old_row_num = CustomersGridView.FocusedRowHandle;
            Bookkeeping.FShowPayments fsp = new Bookkeeping.FShowPayments();
            fsp.ContractID = contractID.ToString();
            fsp.ShowDialog();
            CustomersGridView.TopRowIndex = topindex;
            CustomersGridView.FocusedRowHandle = old_row_num;
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(CustomersGridControl);
        }

        private void LoadDetails()
        {
            string sql = null;

            if (CustomerType == 1)
                sql = $@"WITH CN
                                 AS (SELECT CUS.CUSTOMER_NAME,
                                            H.FIRST_PAYMENT ADVANCE_PAYMENT,
                                            CON.CONTRACT_ID,
                                            TRUNC (CON.CUSTOMER_ID) ID,
                                            CON.CONTRACT_CODE,
                                            CUS.VOEN,
                                            CON.START_DATE,
                                            (CASE
                                                WHEN CON.END_DATE > FC.AGREEMENTDATE
                                                THEN
                                                   FC.AGREEMENTDATE
                                                ELSE
                                                   CON.END_DATE
                                             END)
                                               END_DATE,
                                            H.HOSTAGE
                                       FROM CRS_USER.V_CUSTOMERS CUS,
                                            CRS_USER.V_CONTRACTS CON,
                                            CRS_USER.V_HOSTAGE H,
                                            CRS_USER.V_CONTRACT_FIRST_COMMITMENTS FC
                                      WHERE     CON.CUSTOMER_ID = CUS.ID
                                            AND CUS.VOEN IS NOT NULL
                                            AND CUS.PERSON_TYPE_ID = 1
                                            AND CON.CONTRACT_ID = H.CONTRACT_ID
                                            AND CON.CONTRACT_ID = FC.CONTRACT_ID(+)
                                            AND CON.START_DATE BETWEEN TO_DATE ('{FromDate}',
                                                                                'DD.MM.YYYY')
                                                                   AND TO_DATE ('{ToDate}',
                                                                                'DD.MM.YYYY')),
                                 PAY
                                 AS (SELECT PAYMENT_AMOUNT_AZN, CONTRACT_ID, PAYMENT_DATE
                                       FROM CRS_USER.CUSTOMER_PAYMENTS
                                      WHERE PAYMENT_DATE BETWEEN TO_DATE ('{FromDate}', 'DD.MM.YYYY')
                                                             AND TO_DATE ('{ToDate}', 'DD.MM.YYYY'))
                              SELECT 1 SS,
                                     CN.ID,
                                     CN.CUSTOMER_NAME,
                                     CN.VOEN,
                                     CN.CONTRACT_CODE,
                                     CN.CONTRACT_ID,
                                     CN.HOSTAGE,
                                     TO_CHAR (CN.START_DATE, 'DD.MM.YYYY')
                                     || ' - '
                                     || TO_CHAR (CN.END_DATE, 'DD.MM.YYYY')
                                        PERIOD,
                                     SUM (DISTINCT CN.ADVANCE_PAYMENT) ADVANCE_PAYMENT,
                                     SUM (PAY.PAYMENT_AMOUNT_AZN) PAYED_AMOUNT
                                FROM CN, PAY
                               WHERE     CN.CONTRACT_ID = PAY.CONTRACT_ID
                                     AND (PAY.PAYMENT_DATE BETWEEN CN.START_DATE AND CN.END_DATE)
                                     AND CN.ID = {CustomerID}
                            GROUP BY CN.ID,
                                     CN.CUSTOMER_NAME,
                                     CN.VOEN,
                                     CN.CONTRACT_ID,
                                     CN.CONTRACT_CODE,
                                     CN.HOSTAGE,
                                     CN.START_DATE,
                                     CN.END_DATE";
            else
                sql = $@"WITH COM
                             AS (SELECT CC.ID,
                                        CC.COMMITMENT_NAME CUSTOMER_NAME,
                                        VOEN,
                                        CC.ADVANCE_PAYMENT,
                                        CC.CONTRACT_ID,
                                        CON.CONTRACT_CODE,
                                        CC.AGREEMENTDATE START_DATE,
                                        NVL (
                                           (SELECT AGREEMENTDATE
                                              FROM CRS_USER.CONTRACT_COMMITMENTS
                                             WHERE CONTRACT_ID = CC.CONTRACT_ID AND PARENT_ID = CC.ID),
                                           TRUNC (SYSDATE))
                                           END_DATE,
                                        H.HOSTAGE
                                   FROM CRS_USER.CONTRACT_COMMITMENTS CC,
                                        CRS_USER.V_CONTRACTS CON,
                                        CRS_USER.V_HOSTAGE H
                                  WHERE     CC.VOEN IS NOT NULL
                                        AND CC.CONTRACT_ID = CON.CONTRACT_ID
                                        AND CON.CONTRACT_ID = H.CONTRACT_ID
                                        AND CC.AGREEMENTDATE BETWEEN TO_DATE ('{FromDate}',
                                                                              'DD.MM.YYYY')
                                                                 AND TO_DATE ('{ToDate}',
                                                                              'DD.MM.YYYY')),
                             PAY
                             AS (SELECT PAYMENT_AMOUNT_AZN, CONTRACT_ID, PAYMENT_DATE
                                   FROM CRS_USER.CUSTOMER_PAYMENTS
                                  WHERE PAYMENT_DATE BETWEEN TO_DATE ('{FromDate}', 'DD.MM.YYYY')
                                                         AND TO_DATE ('{ToDate}', 'DD.MM.YYYY'))
                          SELECT 1 SS,
                                 COM.ID,
                                 COM.CUSTOMER_NAME,
                                 COM.VOEN,
                                 COM.CONTRACT_ID,
                                 COM.CONTRACT_CODE,
                                 COM.HOSTAGE,
                                    TO_CHAR (COM.START_DATE, 'DD.MM.YYYY')
                                 || ' - '
                                 || TO_CHAR (COM.END_DATE, 'DD.MM.YYYY')
                                    PERIOD,
                                 SUM (DISTINCT COM.ADVANCE_PAYMENT) ADVANCE_PAYMENT,
                                 SUM (PAY.PAYMENT_AMOUNT_AZN) PAYED_AMOUNT
                            FROM COM, PAY
                           WHERE     COM.CONTRACT_ID = PAY.CONTRACT_ID
                                 AND PAY.PAYMENT_DATE BETWEEN COM.START_DATE AND COM.END_DATE
                                 AND COM.ID = {CustomerID}
                        GROUP BY COM.ID,
                                 COM.CUSTOMER_NAME,
                                 COM.VOEN,
                                 COM.CONTRACT_ID,
                                 COM.CONTRACT_CODE,
                                 COM.HOSTAGE,
                                 COM.START_DATE,
                                 COM.END_DATE";

            CustomersGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Vöen-li müştərilərin dövriyyəsi açılmadı.");
        }
    }
}