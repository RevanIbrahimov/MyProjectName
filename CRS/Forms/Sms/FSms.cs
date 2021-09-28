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
using DevExpress.XtraGrid.Views.Grid;
using CRS.Forms.Bookkeeping;
using CRS.Forms.Total;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;

namespace CRS.Forms.Sms
{
    public partial class FSms : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FSms()
        {
            InitializeComponent();
        }
        int topindex, old_row_num = 0, isAutomatic = 0;
        string contract_id, currency, contract_code, sms_status = null;
        decimal amount;
        bool FormStatus = false;

        private void FSms_Load(object sender, EventArgs e)
        {
            SearchDockPanel.Hide();
            FormStatus = true;
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadSmsDetails();
        }

        private void LoadSmsDetails()
        {
            string sendDate = null, insertDate = null;

            if (FromDateValue.Text.Length > 0 && ToDateValue.Text.Length > 0)
                sendDate = $@"AND TRUNC(SD.SEND_DATE) BETWEEN TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToDateValue.Text}','DD.MM.YYYY')";

            if (FromInsertDateValue.Text.Length > 0 && ToInsertDateValue.Text.Length > 0)
                insertDate = $@"AND TRUNC(SD.INSERT_DATE) BETWEEN TO_DATE('{FromInsertDateValue.Text}','DD.MM.YYYY') AND TO_DATE('{ToInsertDateValue.Text}','DD.MM.YYYY')";

            string s = $@"SELECT 1 SS,
                                     SD.ID,
                                     SD.OWNER_NAME,
                                     DECODE (SD.OWNER_TYPE, 'C', 'Müştəri', 'Öhdəlik götürən')
                                        OWNER_TYPE,
                                     PT.NAME PERSON_TYPE_NAME,
                                     CON.CONTRACT_CODE,
                                     SD.PAYMENT_DATE,
                                     CLP.PAYMENT_DATE LAST_PAYMENT_DATE,
                                     SD.DEBT,
                                     SD.NORMAL_DEBT,
                                     SD.FULL_MONTH_COUNT,
                                     SD.PHONE_NUMBER,
                                     SD.MESSAGE_BODY,
                                     SD.IS_AUTOMATIC,
                                     SD.IS_SEND,
                                     SD.INSERT_DATE,
                                     SD.SEND_DATE,
                                     SS.NAME STATUS_NAME,
                                     ST.NAME SMS_TYPE_NAME,
                                     CON.CREDIT_TYPE_ID,
                                     SD.CONTRACT_ID,
                                     CON.AMOUNT,
                                     CON.CURRENCY_CODE
                                FROM CRS_USER.SMS_DETAILS SD,
                                     CRS_USER.V_CONTRACTS CON,
                                     CRS_USER.SMS_TYPE ST,
                                     CRS_USER.PERSON_TYPE PT,
                                     CRS_USER.SMS_STATUS SS,
                                     CRS_USER.V_CUSTOMER_LAST_PAYMENT CLP
                               WHERE     SD.CONTRACT_ID = CON.CONTRACT_ID(+)
                                     AND SD.SMS_TYPE_ID = ST.ID
                                     AND SD.PERSON_TYPE_ID = PT.ID
                                     AND SD.SMS_STATUS_ID = SS.ID
                                     AND SD.CONTRACT_ID = CLP.CONTRACT_ID(+) {sendDate} {insertDate}
                            ORDER BY SD.INSERT_DATE DESC, CON.CONTRACT_CODE";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadSmsDetails", "Sms-lər yüklənmədi.");

            SmsGridControl.DataSource = dt;

            PaymentsBarButton.Enabled = PaymentScheduleBarButton.Enabled = (SmsGridView.RowCount > 0);
        }

        private void FSms_Activated(object sender, EventArgs e)
        {
            LoadSmsDetails();

            GlobalProcedures.GridRestoreLayout(SmsGridView, SmsRibbonPage.Text);
        }

        private void SmsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(SmsGridControl);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SmsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SmsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SmsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SmsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SmsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SmsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SmsGridControl, "mht");
        }

        private void SmsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(SmsGridView, PopupMenu, e);
        }

        private void SmsGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void SmsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                GridView currentView = sender as GridView;

                if (e.Column.FieldName == "CONTRACT_CODE")
                {
                    int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                    GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
                }
            }
            catch { }
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
            {
                GlobalProcedures.FillCheckedComboBox(SmsStatusComboBox, "SMS_STATUS", "NAME,NAME,NAME", null);
                SearchDockPanel.Show();
            }
            else
                SearchDockPanel.Hide();
        }

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            LoadSmsDetails();
        }

        private void SmsStatusComboBox_EditValueChanged(object sender, EventArgs e)
        {
            sms_status = " [STATUS_NAME] IN ('" + SmsStatusComboBox.Text.Replace("; ", "','") + "')";
            FilterSms();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void SmsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(SmsGridView, SmsRibbonPage.Text);
        }

        private void FromInsertDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToInsertDateValue.Properties.MinValue = FromInsertDateValue.DateTime;
        }

        private void FilterSms()
        {
            if (FormStatus)
            {
                ColumnView view = SmsGridView;

                //Currency
                if (!String.IsNullOrEmpty(SmsStatusComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["STATUS_NAME"],
                        new ColumnFilterInfo(sms_status, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["STATUS_NAME"]);

            }
        }

        private void SmsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = SmsGridView.GetFocusedDataRow();
            if (row != null)
            {
                isAutomatic = Convert.ToInt32(row["IS_AUTOMATIC"]);
                if (isAutomatic == 0)
                    return;

                contract_id = row["CONTRACT_ID"].ToString();
                if (row["AMOUNT"] != DBNull.Value)
                    amount = Convert.ToDecimal(row["AMOUNT"].ToString());
                currency = row["CURRENCY_CODE"].ToString();
                contract_code = row["CONTRACT_CODE"].ToString();
            }
        }

        private void PaymentScheduleBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string cellValue = SmsGridView.GetRowCellValue(SmsGridView.FocusedRowHandle, Sms_FullMonthCount).ToString();
                topindex = SmsGridView.TopRowIndex;
                old_row_num = SmsGridView.FocusedRowHandle;
                FPaymentSchedules fps = new FPaymentSchedules();
                fps.ContractID = contract_id;
                fps.Amount = amount.ToString("N2") + " " + currency;
                fps.ContractCode = contract_code;
                fps.OrderID = cellValue.Remove(cellValue.IndexOf(' ')).Trim();
                fps.ShowDialog();
                SmsGridView.FocusedRowHandle = old_row_num;
                SmsGridView.TopRowIndex = topindex;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müştərinin ödəniş qrafikini açmaq üçün həmin müştərini seçin.", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void PaymentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = SmsGridView.TopRowIndex;
            old_row_num = SmsGridView.FocusedRowHandle;
            FShowPayments fsp = new FShowPayments();
            fsp.ContractID = contract_id;
            fsp.ShowDialog();
            SmsGridView.TopRowIndex = topindex;
            SmsGridView.FocusedRowHandle = old_row_num;
        }
    }
}