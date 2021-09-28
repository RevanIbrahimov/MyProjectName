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

namespace CRS.Forms.Customer
{
    public partial class FVoenPaymentCalc : DevExpress.XtraEditors.XtraForm
    {
        public FVoenPaymentCalc()
        {
            InitializeComponent();
        }
        int customerID, typeID, topindex, old_row_num;
        string customerName;

        private void FVoenPaymentCalc_Load(object sender, EventArgs e)
        {
            SearchDockPanel.Hide();
            ToDateValue.Properties.MaxValue = DateTime.Today;
            ToDateValue.DateTime = DateTime.Today;
            FromDateValue.DateTime = DateTime.Today.AddYears(-1);
            LoadDetails();
        }

        private void LoadDetails()
        {
            if (FromDateValue.Text.Length == 0 || ToDateValue.Text.Length == 0)
                return;

            string sql = $@"SELECT 1 SS, T1.*
                                      FROM (WITH CN
                                                 AS (SELECT CUS.CUSTOMER_NAME,
                                                            H.FIRST_PAYMENT ADVANCE_PAYMENT,
                                                            CON.CONTRACT_ID,
                                                            CON.CUSTOMER_ID,
                                                            CUS.VOEN,
                                                            CON.START_DATE,
                                                            (CASE
                                                                WHEN CON.END_DATE > FC.AGREEMENTDATE
                                                                THEN
                                                                   FC.AGREEMENTDATE
                                                                ELSE
                                                                   CON.END_DATE
                                                             END)
                                                               END_DATE
                                                       FROM CRS_USER.V_CUSTOMERS CUS,
                                                            CRS_USER.V_CONTRACTS CON,
                                                            CRS_USER.V_HOSTAGE H,
                                                            CRS_USER.V_CONTRACT_FIRST_COMMITMENTS FC
                                                      WHERE     CON.CUSTOMER_ID = CUS.ID
                                                            AND CUS.VOEN IS NOT NULL
                                                            AND CUS.PERSON_TYPE_ID = 1
                                                            AND CON.CONTRACT_ID = H.CONTRACT_ID
                                                            AND CON.CONTRACT_ID = FC.CONTRACT_ID(+)
                                                            AND CON.START_DATE BETWEEN TO_DATE ('{FromDateValue.Text}',
                                                                                                'DD.MM.YYYY')
                                                                                   AND TO_DATE ('{ToDateValue.Text}',
                                                                                                'DD.MM.YYYY')),
                                                 PAY
                                                 AS (SELECT PAYMENT_AMOUNT_AZN, CONTRACT_ID, PAYMENT_DATE
                                                       FROM CRS_USER.CUSTOMER_PAYMENTS
                                                      WHERE PAYMENT_DATE BETWEEN TO_DATE ('{FromDateValue.Text}',
                                                                                          'DD.MM.YYYY')
                                                                             AND TO_DATE ('{ToDateValue.Text}',
                                                                                          'DD.MM.YYYY'))
                                              SELECT 1 CUS_TYPE,
                                                     CN.CUSTOMER_ID ID,
                                                     CN.CUSTOMER_NAME,
                                                     CN.VOEN,
                                                     SUM (DISTINCT CN.ADVANCE_PAYMENT) ADVANCE_PAYMENT,
                                                     SUM (PAY.PAYMENT_AMOUNT_AZN) PAYED_AMOUNT
                                                FROM CN, PAY
                                               WHERE     CN.CONTRACT_ID = PAY.CONTRACT_ID
                                                     AND PAY.PAYMENT_DATE BETWEEN CN.START_DATE AND CN.END_DATE
                                            GROUP BY CN.CUSTOMER_ID, CN.CUSTOMER_NAME, CN.VOEN) T1
                                    UNION ALL
                                    SELECT 1 SS, T2.*
                                      FROM (WITH COM
                                                 AS (SELECT CC.ID,
                                                            CC.COMMITMENT_NAME CUSTOMER_NAME,
                                                            VOEN,
                                                            CC.ADVANCE_PAYMENT,
                                                            CC.CONTRACT_ID,
                                                            CC.AGREEMENTDATE START_DATE,
                                                            NVL (
                                                               (SELECT AGREEMENTDATE
                                                                  FROM CRS_USER.CONTRACT_COMMITMENTS
                                                                 WHERE     CONTRACT_ID = CC.CONTRACT_ID
                                                                       AND PARENT_ID = CC.ID),
                                                               TRUNC (SYSDATE))
                                                               END_DATE
                                                       FROM CRS_USER.CONTRACT_COMMITMENTS CC
                                                      WHERE     CC.VOEN IS NOT NULL
                                                            AND CC.AGREEMENTDATE BETWEEN TO_DATE ('{FromDateValue.Text}',
                                                                                                  'DD.MM.YYYY')
                                                                                     AND TO_DATE ('{ToDateValue.Text}',
                                                                                                  'DD.MM.YYYY')),
                                                 PAY
                                                 AS (SELECT PAYMENT_AMOUNT_AZN, CONTRACT_ID, PAYMENT_DATE
                                                       FROM CRS_USER.CUSTOMER_PAYMENTS
                                                      WHERE PAYMENT_DATE BETWEEN TO_DATE ('{FromDateValue.Text}',
                                                                                          'DD.MM.YYYY')
                                                                             AND TO_DATE ('{ToDateValue.Text}',
                                                                                          'DD.MM.YYYY'))
                                              SELECT 2 CUS_TYPE,
                                                     COM.ID,
                                                     COM.CUSTOMER_NAME,
                                                     COM.VOEN,
                                                     SUM (DISTINCT COM.ADVANCE_PAYMENT) ADVANCE_PAYMENT,
                                                     SUM (PAY.PAYMENT_AMOUNT_AZN) PAYED_AMOUNT
                                                FROM COM, PAY
                                               WHERE     COM.CONTRACT_ID = PAY.CONTRACT_ID
                                                     AND PAY.PAYMENT_DATE BETWEEN COM.START_DATE AND COM.END_DATE
                                            GROUP BY COM.ID, COM.CUSTOMER_NAME, COM.VOEN) T2";

            CustomersGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Vöen-li müştərilərin dövriyyəsi açılmadı.");

            DetailsBarButton.Enabled = (CustomersGridView.RowCount > 0);
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

            if (e.Column == Customer_Type)
            {
                int cusType = Convert.ToInt16(CustomersGridView.GetListSourceRowCellValue(rowIndex, "CUS_TYPE"));
                if (cusType == 1)
                    e.Value = "İlk müştəri";
                else
                    e.Value = "Öhdəlik götürən";
            }
        }

        private void CustomersGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(CustomersGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CustomersGridControl, "xls");
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

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            CustomersGridView.ViewCaption = FromDateValue.Text + " - " + ToDateValue.Text + " intervalında VÖEN-li fiziki müştərilərin dövriyyəsi.";
            LoadDetails();
        }

        private void CustomersGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            if (e.Column == Customer_Total)
            {
                decimal total = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, Customer_Total));

                if (total > 200000)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.Red;
                else if (total >= 170000 && total <= 200000)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.Aqua;
            }
        }

        private void CustomersGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CustomersGridView, PopupMenu, e);
        }

        private void CustomersGridView_DoubleClick(object sender, EventArgs e)
        {
            if(DetailsBarButton.Enabled)            
                ShowCustomerDetails();            
        }

        private void DetailsBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCustomerDetails();
        }

        private void ShowCustomerDetails()
        {
            topindex = CustomersGridView.TopRowIndex;
            old_row_num = CustomersGridView.FocusedRowHandle;
            FVoenPaymentDetails fw = new FVoenPaymentDetails();
            fw.CustomerID = customerID;
            fw.FromDate = FromDateValue.Text;
            fw.ToDate = ToDateValue.Text;
            fw.CustomerName = customerName;
            fw.CustomerType = typeID;
            fw.ShowDialog();
            CustomersGridView.TopRowIndex = topindex;
            CustomersGridView.FocusedRowHandle = old_row_num;
        }

        private void CustomersGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CustomersGridView.GetFocusedDataRow();
            if (row != null)
            {
                customerID = int.Parse(row["ID"].ToString());
                typeID = int.Parse(row["CUS_TYPE"].ToString());
                customerName = row["CUSTOMER_NAME"].ToString();
            }
        }
    }
}