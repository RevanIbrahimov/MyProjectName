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
using System.Web.UI.WebControls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using System.Runtime.InteropServices;

namespace CRS.Forms.Contracts
{
    public partial class FInsurance : DevExpress.XtraEditors.XtraForm
    {
        public FInsurance()
        {
            InitializeComponent();
        }
        DateTime StartDate, EndDate;
        string ContractCode, Police;
        int InsuranceID, topindex, old_row_num;
        decimal selectedTransferCurrentDebtSum = 0;

        private void FInsurance_Load(object sender, EventArgs e)
        {
            if (GlobalVariables.V_UserID > 0)
            {
                //PaymentBarButton.Enabled = GlobalVariables.InsurancePayment;
                //DeletePaymentBarButton.Enabled = GlobalVariables.DeleteInsurancePayment;
                //CompensationBarButton.Enabled = GlobalVariables.InsuranceCompensation;
                TransferBarButton.Enabled = GlobalVariables.InsuranceTransfer;
            }


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
                   status = null;

            if (ClosedCheck.Checked && !ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 6";
            else if (!ClosedCheck.Checked && ActiveCheck.Checked)
                status = " AND C.STATUS_ID = 5";

            string s = $@"SELECT I.ID,
                                 ROW_NUMBER () OVER (PARTITION BY I.CONTRACT_ID ORDER BY I.ID DESC) RNUM,
                                 I.CONTRACT_ID,
                                 C.CONTRACT_CODE,
                                 IC.NAME COMPANY_NAME,
                                 I.INSURANCE_AMOUNT,
                                 I.INSURANCE_PERIOD ,
                                 I.INSURANCE_INTEREST,
                                 I.UNCONDITIONAL_AMOUNT,
                                 I.AMOUNT INSURANCE_COST,
                                 NVL (SP.LEGAL_PAYED_AMOUNT, 0) LEGAL_PAYED_AMOUNT,
                                 NVL (SP.INLEGAL_PAYED_AMOUNT, 0) INLEGAL_PAYED_AMOUNT,
                                 NVL (SP.PAYED_AMOUNT, 0) PAYED_AMOUNT,
                                 I.AMOUNT - NVL(SP.PAYED_AMOUNT, 0) CUSTOMER_DEBT,
                                 NVL(ST.TRANSFER_AMOUNT, 0) TRANSFER_AMOUNT,
                                 NVL(ST.COMPENSATION, 0) COMPENSATION,
                                 NVL(SP.PAYED_AMOUNT, 0) - NVL(ST.TRANSFER_AMOUNT, 0) - NVL(ST.COMPENSATION, 0) TRANSFER_CURRENT_DEBT,
                                 I.AMOUNT - NVL(ST.TRANSFER_AMOUNT, 0) - NVL(ST.COMPENSATION, 0) TRANSFER_DEBT,
                                 I.START_DATE,
                                 I.END_DATE,
                                 I.POLICE,
                                 I.NOTE,
                                 DECODE(I.IS_AGAIN,1,'Təkrar', null) AGAIN_DESCRIPTION,
                                 I.IS_CANCEL,
                                 (CASE
                                     WHEN I.INSURANCE_PERIOD = C.PERIOD AND I.END_DATE < TRUNC(SYSDATE) THEN 103
                                     WHEN I.INSURANCE_PERIOD = C.PERIOD AND I.END_DATE >= TRUNC(SYSDATE) THEN 100
                                     WHEN I.IS_CANCEL = 1 THEN 101
                                     WHEN I.END_DATE < TRUNC(SYSDATE) THEN 102 
                                     WHEN (I.END_DATE - TRUNC(SYSDATE)) < 31 THEN 1
                                     ELSE 0
                                  END)
                                    INSURANCE_DIFF_MONTH,
                                  H.HOSTAGE,
                                  S.STATUS_NAME,
                                  C.STATUS_ID,
                                  (SELECT COUNT(*) FROM CRS_USER.INSURANCE_TRANSFER IT,CRS_USER.PAYMENT_TASKS PT WHERE IT.PAYMENT_TASK_ID = PT.ID AND INSURANCE_ID = I.ID) IS_TASK
                            FROM CRS_USER.INSURANCES I,
                                 CRS_USER.V_CONTRACTS C,
                                  CRS_USER.STATUS S,
                                 CRS_USER.INSURANCE_COMPANY IC,
                                 CRS_USER.V_SUM_INSURANCE_PAYMENT SP,
                                 CRS_USER.V_SUM_INSURANCE_TRANSFER ST,
                                 CRS_USER.V_HOSTAGE H
                           WHERE     I.CONTRACT_ID = C.CONTRACT_ID
                                 AND I.COMPANY_ID = IC.ID
                                 AND C.CONTRACT_ID = H.CONTRACT_ID
                                 AND C.STATUS_ID = S.ID
                                 AND I.ID = SP.INSURANCE_ID(+)
                                 AND I.ID = ST.INSURANCE_ID(+){last}{again}{cancel}{status}
                        ORDER BY C.CONTRACT_CODE";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadInsuranceDataGridView", "Sığortaların siyahısı yüklənmədi.");
            InsuranceGridControl.DataSource = dt;

            ShowCancelFileBarButton.Enabled = (dt.Rows.Count > 0);

            if (dt.Rows.Count > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    PaymentBarButton.Enabled = GlobalVariables.InsurancePayment;
                    DeletePaymentBarButton.Enabled = GlobalVariables.DeleteInsurancePayment;
                    CompensationBarButton.Enabled = GlobalVariables.InsuranceCompensation;
                }
                else
                    PaymentBarButton.Enabled = CompensationBarButton.Enabled = DeletePaymentBarButton.Enabled = true;
            }
            else
                PaymentBarButton.Enabled = CompensationBarButton.Enabled = DeletePaymentBarButton.Enabled = false;

            GenerateSumSelectedRowsValue();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadInsuranceDataGridView();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(InsuranceGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(InsuranceGridControl, "xls");
        }

        private void InsuranceGridView_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void InsuranceGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(InsuranceGridView, PopupMenu, e);
        }

        private void InsuranceGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (int.Parse(InsuranceGridView.GetRowCellDisplayText(e.RowHandle, InsuranceGridView.Columns["STATUS_ID"])) == 5)
                {
                    if (Convert.ToInt16(InsuranceGridView.GetRowCellValue(e.RowHandle, "RNUM")) > 1)
                        e.Appearance.BackColor = e.Appearance.BackColor2 = Color.Coral;
                    else if (e.Column.FieldName == "END_DATE")
                    {
                        if (!String.IsNullOrWhiteSpace(InsuranceGridView.GetRowCellValue(e.RowHandle, "INSURANCE_DIFF_MONTH").ToString()))
                        {
                            int monthCount = Convert.ToInt32(InsuranceGridView.GetRowCellValue(e.RowHandle, "INSURANCE_DIFF_MONTH"));
                            if (monthCount == 1)
                                e.Appearance.BackColor = e.Appearance.BackColor2 = Color.Yellow;
                            else if (monthCount == 102)
                                e.Appearance.BackColor = e.Appearance.BackColor2 = Color.Red;
                            else if (monthCount == 100)
                                e.Appearance.BackColor = e.Appearance.BackColor2 = GlobalFunctions.CreateColor("#3CEEED");
                            else if (monthCount == 103)
                                e.Appearance.BackColor = e.Appearance.BackColor2 = GlobalFunctions.CreateColor("#D819F6");
                            else if (monthCount == 101)
                            {
                                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor1);
                                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor2);
                            }
                        }
                    }
                }
                else
                    GlobalProcedures.GridRowCellStyleForClose(6, InsuranceGridView, e);
            }
        }

        private void DebtCheck_CheckedChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            ColumnView view = InsuranceGridView;

            if (DebtCheck.Checked)
                view.ActiveFilter.Add(view.Columns["CUSTOMER_DEBT"],
              new ColumnFilterInfo("[CUSTOMER_DEBT] > 0", "Müştərinin borcu > 0"));
            else
                view.ActiveFilter.Remove(view.Columns["CUSTOMER_DEBT"]);

            if (InLegalPayedCheck.Checked)
                view.ActiveFilter.Add(view.Columns["INLEGAL_PAYED_AMOUNT"],
              new ColumnFilterInfo("[INLEGAL_PAYED_AMOUNT] > 0", "Müştərinin qeyri-rəsmi ödədiyi > 0"));
            else
                view.ActiveFilter.Remove(view.Columns["INLEGAL_PAYED_AMOUNT"]);

            if (TransferCheck.Checked)
                view.ActiveFilter.Add(view.Columns["TRANSFER_AMOUNT"],
              new ColumnFilterInfo("[INSURANCE_COST] > 0 AND [TRANSFER_AMOUNT] = 0", "Köçürülüb = 0"));
            else
                view.ActiveFilter.Remove(view.Columns["TRANSFER_AMOUNT"]);

            if (TransferDebt2Check.Checked)
                view.ActiveFilter.Add(view.Columns["TRANSFER_DEBT"],
              new ColumnFilterInfo("[TRANSFER_DEBT] > 0", "Sığorta şirkətinə olan borc > 0"));
            else
                view.ActiveFilter.Remove(view.Columns["TRANSFER_DEBT"]);

            if (TransferDebtCheck.Checked && !TransferCurrentDebtCheck.Checked)
                view.ActiveFilter.Add(view.Columns["TRANSFER_CURRENT_DEBT"],
              new ColumnFilterInfo("[TRANSFER_CURRENT_DEBT] < 0", "Hesab < 0"));
            else if (!TransferDebtCheck.Checked && TransferCurrentDebtCheck.Checked)
                view.ActiveFilter.Add(view.Columns["TRANSFER_CURRENT_DEBT"],
              new ColumnFilterInfo("[TRANSFER_CURRENT_DEBT] > 0", "Hesab > 0"));
            else
                view.ActiveFilter.Remove(view.Columns["TRANSFER_CURRENT_DEBT"]);            

            if (PoliceCheck.Checked)
                view.ActiveFilter.Add(view.Columns["POLICE"],
              new ColumnFilterInfo("[POLICE] IS NULL", "Polisdəki qeydiyyatı = null"));
            else
                view.ActiveFilter.Remove(view.Columns["POLICE"]);

            if (CurrentActiveCheck.Checked)
                view.ActiveFilter.Add(view.Columns["END_DATE"],
              new ColumnFilterInfo($@"[END_DATE] >= #{DateTime.Today}#", "Bitmə tarixi >= " + DateTime.Today.ToString("dd.MM.yyyy")));
            else
                view.ActiveFilter.Remove(view.Columns["END_DATE"]);
        }

        private void InsuranceGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell(Insurance_SS, "Center", e);
        }

        private void TransferBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string listID = null;
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < InsuranceGridView.SelectedRowsCount; i++)
            {
                int rowHandle = InsuranceGridView.GetSelectedRows()[i];
                if (!InsuranceGridView.IsGroupRow(rowHandle))
                    rows.Add(InsuranceGridView.GetDataRow(rowHandle));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;

                listID += row["ID"] + ",";
            }

            if (listID != null)
                listID = listID.TrimEnd(',');

            FInsuranceTransfer ft = new FInsuranceTransfer();
            ft.ListID = listID;
            ft.RefreshDataGridView += new FInsuranceTransfer.DoEvent(LoadInsuranceDataGridView);
            ft.ShowDialog();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void PaymentListBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FInsurancePaymentList fpl = new FInsurancePaymentList();
            fpl.ShowDialog();
        }

        private void TransferListBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string listID = null;
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < InsuranceGridView.SelectedRowsCount; i++)
            {
                int rowHandle = InsuranceGridView.GetSelectedRows()[i];
                if (!InsuranceGridView.IsGroupRow(rowHandle))
                    rows.Add(InsuranceGridView.GetDataRow(rowHandle));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;

                listID += row["ID"] + ",";
            }

            if (listID != null)
                listID = listID.TrimEnd(',');

            FInsuranceTransferList fl = new FInsuranceTransferList();
            fl.ListID = listID;
            fl.RefreshDataGridView += new FInsuranceTransferList.DoEvent(LoadInsuranceDataGridView);
            fl.ShowDialog();
        }

        private void CompensationBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = InsuranceGridView.TopRowIndex;
            old_row_num = InsuranceGridView.FocusedRowHandle;
            FInsurancePayment fp = new FInsurancePayment();
            fp.InsuranceID = InsuranceID;
            fp.TypeID = 2;
            fp.RefreshDataGridView += new FInsurancePayment.DoEvent(LoadInsuranceDataGridView);
            fp.ShowDialog();
            InsuranceGridView.TopRowIndex = topindex;
            InsuranceGridView.FocusedRowHandle = old_row_num;
        }

        private void DeletePaymentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string listID = null;
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < InsuranceGridView.SelectedRowsCount; i++)
            {
                int rowHandle = InsuranceGridView.GetSelectedRows()[i];
                if (!InsuranceGridView.IsGroupRow(rowHandle))
                    rows.Add(InsuranceGridView.GetDataRow(rowHandle));
            }

            if (rows.Count == 0)
            {
                GlobalProcedures.ShowWarningMessage("Ödənişlərini silmək istədiyiniz sığortaları seçin.");
                return;
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;

                listID += row["ID"] + ",";
            }

            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sığortaların ödənişlərini silmək istəyirsiniz? Silinmiş ödənişləri geri qaytarmaq olmayacaq", "Seçilmiş ödənişlərin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                listID = listID.TrimEnd(',');
                GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_DELETE_INSURANCE_PAYMENT", "P_ID_LIST", listID, "Seçilmiş sığortaların ödənişləri silinmədi.");
            }
            LoadInsuranceDataGridView();
        }

        private void PaymentBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFInsurancePayment();
        }

        private void InsuranceGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            ShowCancelFileBarButton.Enabled = (InsuranceGridView.RowCount > 0);

            if (InsuranceGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    PaymentBarButton.Enabled = GlobalVariables.InsurancePayment;
                    DeletePaymentBarButton.Enabled = GlobalVariables.DeleteInsurancePayment;
                    CompensationBarButton.Enabled = GlobalVariables.InsuranceCompensation;
                }
                else
                    PaymentBarButton.Enabled = CompensationBarButton.Enabled = DeletePaymentBarButton.Enabled = true;
            }
            else
                PaymentBarButton.Enabled = CompensationBarButton.Enabled = DeletePaymentBarButton.Enabled = false;
        }

        private void PoliceBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            topindex = InsuranceGridView.TopRowIndex;
            old_row_num = InsuranceGridView.FocusedRowHandle;
            FInsurancePoliceEdit fp = new FInsurancePoliceEdit();
            fp.InsuranceID = InsuranceID;
            fp.StartDate = StartDate;
            fp.EndDate = EndDate;
            fp.ContractCode = ContractCode;
            fp.Police = Police;
            fp.RefreshDataGridView += new FInsurancePoliceEdit.DoEvent(LoadInsuranceDataGridView);
            fp.ShowDialog();
            InsuranceGridView.TopRowIndex = topindex;
            InsuranceGridView.FocusedRowHandle = old_row_num;
        }

        private void LoadFInsurancePayment()
        {
            topindex = InsuranceGridView.TopRowIndex;
            old_row_num = InsuranceGridView.FocusedRowHandle;
            FInsurancePayment fp = new FInsurancePayment();
            fp.InsuranceID = InsuranceID;
            fp.TypeID = 1;
            fp.RefreshDataGridView += new FInsurancePayment.DoEvent(LoadInsuranceDataGridView);
            fp.ShowDialog();
            InsuranceGridView.TopRowIndex = topindex;
            InsuranceGridView.FocusedRowHandle = old_row_num;
        }

        private void SearchBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
                SearchDockPanel.Show();
            else
                SearchDockPanel.Hide();
        }

        private void DebtEqualBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string filePath = null;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Debitor faylı seçin";
                dlg.Filter = "All files (*.xls;*.xlsx)|*.xls;*.xlsx";

                if (dlg.ShowDialog() == DialogResult.OK)
                    filePath = dlg.FileName;
                dlg.Dispose();
            }

            if (filePath == null)
                return;
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FWait));


            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int errorCount = 0;
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            try
            {
                for (int i = 1; i <= rowCount; i++)
                {
                    if (xlRange.Cells[i, 1].Value2 == null || xlRange.Cells[i, 2].Value2 == null)
                    {
                        errorCount++;
                        continue;
                    }

                    string police = xlRange.Cells[i, 1].Value2.ToString();
                    decimal debt = Convert.ToDecimal(xlRange.Cells[i, 2].Value2);
                    if (GlobalFunctions.ExecuteTwoQuery($@"DELETE FROM CRS_USER_TEMP.INSURANCE_DEBITOR_TEMP WHERE POLICE = '{police}'", $@"INSERT INTO CRS_USER_TEMP.INSURANCE_DEBITOR_TEMP(POLICE,AMOUNT,USED_USER_ID)VALUES('{police}',{debt.ToString(GlobalVariables.V_CultureInfoEN)},{GlobalVariables.V_UserID})", police + " sığortasının borcu temp cədvələ daxil edilmədi.") == -1)
                        errorCount++;
                }

                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowWarningMessage(rowCount + " sətirdən " + (rowCount - errorCount) + " sətir temp cədvələ daxil edildi.");
                FInsuranceDebitor fid = new FInsuranceDebitor();
                fid.ShowDialog();
            }
            catch
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.ShowErrorMessage("Seçdiyiniz fayl temp cədvələ daxil edilmədi.");
            }
            finally
            {
                //cleanup  
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //release com objects to fully kill excel process from running in the background  
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                //close and release  
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                //quit and release  
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
        }

        private void CompensationListBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string listID = null;
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < InsuranceGridView.SelectedRowsCount; i++)
            {
                int rowHandle = InsuranceGridView.GetSelectedRows()[i];
                if (!InsuranceGridView.IsGroupRow(rowHandle))
                    rows.Add(InsuranceGridView.GetDataRow(rowHandle));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;

                listID += row["ID"] + ",";
            }

            if (listID != null)
                listID = listID.TrimEnd(',');

            FInsuranceCompensationList fl = new FInsuranceCompensationList();
            fl.ListID = listID;
            fl.RefreshDataGridView += new FInsuranceCompensationList.DoEvent(LoadInsuranceDataGridView);
            fl.ShowDialog();
        }

        private void InsuranceGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            //if (InsuranceGridView.RowCount == 0)
            //    return;
            
            //if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            //    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("TRANSFER_CURRENT_DEBT") == 0)
            //        e.TotalValue = selectedTransferCurrentDebtSum;
        }

        private void GenerateSumSelectedRowsValue()
        {
            TransferCurrentDebtSumBarStaticItem.Caption = "<color=104,0,0>Seçilmiş hesabda olanların cəmi:</color><b> " + GlobalFunctions.SumSelectedRow(InsuranceGridView, "TRANSFER_CURRENT_DEBT").ToString("n2") + "</b>";            
        }

        private void InsuranceGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            GenerateSumSelectedRowsValue();
        }

        private void LastCheck_CheckedChanged(object sender, EventArgs e)
        {
            LoadInsuranceDataGridView();
            GlobalProcedures.ChangeCheckStyle((sender as CheckEdit));
        }

        private void ShowCancelFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowWordFileWithExtensionFromDB($@"SELECT T.CANCEL_FILE, CANCEL_FILE_EXTENSION FROM CRS_USER.INSURANCES T WHERE T.ID = {InsuranceID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + ContractCode + "_SığortadanImtinaErizezi",
                                                "CANCEL_FILE", "CANCEL_FILE_EXTENSION");
        }

        private void InsuranceGridView_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, Insurance_SS, e);
        }

        private void InsuranceGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = InsuranceGridView.GetFocusedDataRow();
            if (row != null)
            {
                InsuranceID = int.Parse(row["ID"].ToString());
                ContractCode = row["CONTRACT_CODE"].ToString();
                ShowCancelFileBarButton.Enabled = (Convert.ToInt16(row["IS_CANCEL"]) == 1);
                StartDate = Convert.ToDateTime(row["START_DATE"]);
                EndDate = Convert.ToDateTime(row["END_DATE"]);
                Police = row["POLICE"].ToString();
            }
        }
    }
}