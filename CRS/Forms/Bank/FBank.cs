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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;

namespace CRS.Forms.Bank
{
    public partial class FBank : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FBank()
        {
            InitializeComponent();
        }
        string OperationID, BankID, BankName, Date, operation_name, appointmentName, operationTypeName, contractCode;
        int appointment_id, topindex, old_row_num;
        decimal Amount;
        bool FormStatus = false;

        private void FBank_Load(object sender, EventArgs e)
        {
            ProgressPanel.Hide();
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                OperationsBarButton.Enabled = (GlobalVariables.AddBankIncome || GlobalVariables.AddBankExpenses);
                EditBarButton.Enabled = GlobalVariables.EditBank;
                DeleteBarButton.Enabled = GlobalVariables.DeleteBank;
                PrintBarButton.Enabled = GlobalVariables.PrintBank;
                ExportBarSubItem.Enabled = GlobalVariables.ExportBank;
                BankAccountBarButton.Enabled = GlobalVariables.BankAccount;                
            }

            GlobalProcedures.FillCheckedComboBox(OperationTypeComboBox, "OPERATION_TYPES", "TYPE_AZ,TYPE_EN,TYPE_RU", "1 = 1 ORDER BY ID");
            CurBarStatic.Caption = GlobalVariables.V_LastRate;
            SearchDockPanel.Hide();
            FormStatus = true;
        }

        private void LoadBankDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 BO.ID,
                                 B.LONG_NAME BANK_NAME,
                                 B.ACCOUNT ACCOUNT_NAME,
                                 BO.OPERATION_DATE,
                                 (CASE
                                     WHEN BO.CONTRACT_CODE IS NOT NULL
                                     THEN
                                        BA.NAME || ' - ' || BO.CONTRACT_CODE
                                     ELSE
                                        BA.NAME
                                  END)
                                    APPOINTMENT_NAME,
                                 BO.INCOME,
                                 BO.EXPENSES,
                                 BO.DEBT,
                                 BO.DEBT_BANK,
                                 BA.OPERATION_TYPE_ID,
                                 BO.USED_USER_ID,
                                 BO.BANK_ID,
                                 BA.ID APPOINTMENT_ID,
                                 OT.TYPE_AZ OPERATION_NAME,
                                 BO.CONTRACT_CODE
                            FROM CRS_USER.BANK_OPERATIONS BO,
                                 CRS_USER.BANK_APPOINTMENTS BA,
                                 CRS_USER.BANKS B,
                                 CRS_USER.OPERATION_TYPES OT
                           WHERE     BO.APPOINTMENT_ID = BA.ID
                                 AND BO.BANK_ID = B.ID
                                 AND BA.OPERATION_TYPE_ID = OT.ID
                        ORDER BY BO.OPERATION_DATE, BO.ID";
            BankGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadBankDataGridView", "Bank əməliyyatları yüklənmədi.");
            if (BankGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditBank;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteBank;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
                CalcDebtBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = CalcDebtBarButton.Enabled = false;
        }

        private void LoadFAccounts()
        {
            FBankAccounts fba = new FBankAccounts();
            fba.ShowDialog();
        }

        private void BankAccountBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFAccounts();
        }

        void RefreshBank(string operationdate)
        {
            LoadBankDataGridView();
        }

        private void LoadFBankOperationAddEdit(string transaction, string operationid, string bankid, string date, string bankname)
        {
            topindex = BankGridView.TopRowIndex;
            old_row_num = BankGridView.FocusedRowHandle;
            FBankOperation fbae = new FBankOperation();
            fbae.TransactionName = transaction;
            fbae.OperationID = operationid;
            fbae.BankID = bankid;
            fbae.Date = date;
            fbae.BankName = bankname;
            fbae.RefreshBankDataGridView += new FBankOperation.DoEvent(RefreshBank);
            fbae.ShowDialog();
            BankGridView.TopRowIndex = topindex;
            BankGridView.FocusedRowHandle = old_row_num;
        }

        private void OperationsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFBankOperationAddEdit("INSERT", null, null, null, null);
        }

        private void FBank_Activated(object sender, EventArgs e)
        {
            LoadBankDataGridView();
            BankGridView.FocusedRowHandle = BankGridView.RowCount - 1;
            BankGridView.TopRowIndex = BankGridView.RowCount - 1;

            GlobalProcedures.GridRestoreLayout(BankGridView, BankRibbonPage.Text);
        }

        private void BankGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("INCOME", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("EXPENSES", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            if (e.Column.FieldName == "DEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ProgressPanel.Show();
            Application.DoEvents();
            ControlContractInBankOperations();
            ControlPaymentsInBankOperations();
            LoadBankDataGridView();
            ProgressPanel.Hide();
            BankGridView.FocusedRowHandle = BankGridView.RowCount - 1;
            BankGridView.TopRowIndex = BankGridView.RowCount - 1;
        }

        private void ControlPaymentsInBankOperations()
        {
            string sql = $@"SELECT CP.BANK_ID,
                                   AP.ID ACCOUNTING_PLAN_ID,
                                   TO_CHAR(CP.PAYMENT_DATE,'DD/MM/YYYY') OPERATION_DATE,
                                   3 APPOINTMENT_ID,
                                   CP.PAYMENT_AMOUNT_AZN INCOME,
                                   0 EXPENSES,
                                   0 DEBT,
                                   CP.ID CONTRACT_PAYMENT_ID,
                                   C.CONTRACT_CODE
                              FROM CRS_USER.CUSTOMER_PAYMENTS CP,
                                   CRS_USER.V_CONTRACTS C,
                                   CRS_USER.ACCOUNTING_PLAN AP
                             WHERE     BANK_CASH = 'B'
                                   AND CP.CONTRACT_ID = C.CONTRACT_ID
                                   AND CP.BANK_ID = AP.BANK_ID
                                   AND CP.ID NOT IN
                                          (SELECT CONTRACT_PAYMENT_ID FROM CRS_USER.BANK_OPERATIONS)
                            ORDER BY CP.PAYMENT_DATE,CP.ID";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/ControlPaymentsInBankOperations");

            if (dt.Rows.Count == 0)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                string osql = $@"INSERT INTO CRS_USER.BANK_OPERATIONS(ID,
                                                                        BANK_ID,
                                                                        ACCOUNTING_PLAN_ID,
                                                                        OPERATION_DATE,
                                                                        APPOINTMENT_ID,
                                                                        INCOME,
                                                                        EXPENSES,
                                                                        DEBT,
                                                                        CONTRACT_PAYMENT_ID,
                                                                        CONTRACT_CODE) 
                                VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL,
                                        {dr["BANK_ID"]},
                                        {dr["ACCOUNTING_PLAN_ID"]},
                                        TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'),
                                        3,
                                        {Convert.ToDecimal(dr["INCOME"]).ToString(GlobalVariables.V_CultureInfoEN)},
                                        0,
                                        0,
                                        {dr["CONTRACT_PAYMENT_ID"]},
                                        '{dr["CONTRACT_CODE"]}')";

                GlobalProcedures.ExecuteQuery(osql, "Linq ödənişi bank əməliyyatlarının mədaxilinə daxil olunmadı.", this.Name + "/ControlPaymentsInBankOperations");
                GlobalProcedures.UpdateBankOperationDebtWithBank(dr["OPERATION_DATE"].ToString(), int.Parse(dr["BANK_ID"].ToString()));
                GlobalProcedures.UpdateBankOperationDebt(dr["OPERATION_DATE"].ToString());
            }
        }

        private void ControlContractInBankOperations()
        {
            string sql = $@"SELECT PO.LIQUID_BANK_ID,
                                 (SELECT ID
                                    FROM CRS_USER.ACCOUNTING_PLAN
                                   WHERE BANK_ID = PO.LIQUID_BANK_ID)
                                    LIQUID_ACCOUTING_PLAN_ID,
                                 PO.FIRST_PAYMENT_BANK_ID,
                                 (SELECT ID
                                    FROM CRS_USER.ACCOUNTING_PLAN
                                   WHERE BANK_ID = PO.LIQUID_BANK_ID)
                                    FIRST_ACCOUTING_PLAN_ID,
                                 TO_CHAR (C.START_DATE, 'DD/MM/YYYY') OPERATION_DATE,
                                 H.LIQUID_AMOUNT EXPENSES,
                                 H.FIRST_PAYMENT INCOME,
                                 C.CONTRACT_CODE
                            FROM CRS_USER.V_CONTRACTS C,
                                 CRS_USER.V_HOSTAGE H,
                                 CRS_USER.CONTRACT_PAID_OUT PO
                           WHERE     C.CONTRACT_ID = H.CONTRACT_ID
                                 AND C.CONTRACT_ID = PO.CONTRACT_ID
                                 AND C.IS_COMMIT = 1
                                 AND (PO.LIQUID_BANK_ID != 0 OR PO.FIRST_PAYMENT_BANK_ID != 0)
                                 AND NOT EXISTS
                                            (SELECT ID
                                               FROM CRS_USER.BANK_OPERATIONS
                                              WHERE     CONTRACT_CODE = C.CONTRACT_CODE
                                                    AND APPOINTMENT_ID IN (8, 19))
                        ORDER BY C.START_DATE, C.CONTRACT_CODE";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/ControlContractInBankOperations");

            if (dt.Rows.Count == 0)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                string sql19 = $@"INSERT INTO CRS_USER.BANK_OPERATIONS(ID,
                                                                        BANK_ID,
                                                                        ACCOUNTING_PLAN_ID,
                                                                        OPERATION_DATE,
                                                                        APPOINTMENT_ID,
                                                                        INCOME,
                                                                        EXPENSES,
                                                                        DEBT,                                                                        
                                                                        CONTRACT_CODE) 
                                VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL,
                                        {dr["FIRST_PAYMENT_BANK_ID"]},
                                        {dr["FIRST_ACCOUTING_PLAN_ID"]},
                                        TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'),
                                        19,
                                        {Convert.ToDecimal(dr["INCOME"]).ToString(GlobalVariables.V_CultureInfoEN)},
                                        0,
                                        0,
                                        '{dr["CONTRACT_CODE"]}')",
                       sql8 = $@"INSERT INTO CRS_USER.BANK_OPERATIONS(ID,
                                                                        BANK_ID,
                                                                        ACCOUNTING_PLAN_ID,
                                                                        OPERATION_DATE,
                                                                        APPOINTMENT_ID,
                                                                        INCOME,
                                                                        EXPENSES,
                                                                        DEBT,                                                                        
                                                                        CONTRACT_CODE) 
                                VALUES(BANK_OPERATION_SEQUENCE.NEXTVAL,
                                        {dr["LIQUID_BANK_ID"]},
                                        {dr["LIQUID_ACCOUTING_PLAN_ID"]},
                                        TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'),
                                        8,                                        
                                        0,
                                        {Convert.ToDecimal(dr["EXPENSES"]).ToString(GlobalVariables.V_CultureInfoEN)},
                                        0,
                                        '{dr["CONTRACT_CODE"]}')",
                        sql_delete = $@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE CONTRACT_CODE = '{dr["CONTRACT_CODE"]}' AND APPOINTMENT_ID IN (8,19) AND OPERATION_DATE = TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY')";


                GlobalProcedures.ExecuteThreeQuery(sql_delete, sql19, sql8, "Müqavilə bank əməliyyatlarına daxil olunmadı.", this.Name + "/ControlContractInBankOperations");
                GlobalProcedures.UpdateBankOperationDebtWithBank(dr["OPERATION_DATE"].ToString(), int.Parse(dr["LIQUID_BANK_ID"].ToString()));
                if (int.Parse(dr["LIQUID_BANK_ID"].ToString()) != int.Parse(dr["FIRST_PAYMENT_BANK_ID"].ToString()))
                    GlobalProcedures.UpdateBankOperationDebtWithBank(dr["OPERATION_DATE"].ToString(), int.Parse(dr["FIRST_PAYMENT_BANK_ID"].ToString()));
                GlobalProcedures.UpdateBankOperationDebt(dr["OPERATION_DATE"].ToString());
            }
        }

        private void BankGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;

            int type = int.Parse(currentView.GetRowCellDisplayText(e.RowHandle, currentView.Columns["OPERATION_TYPE_ID"]));
            GlobalProcedures.GridRowOperationTypeColor(type, e);
            GlobalProcedures.GridRowCellStyleForBlock(currentView, e);
        }

        private void BankGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = BankGridView.GetFocusedDataRow();
            if (row != null)
            {
                OperationID = row["ID"].ToString();
                BankID = row["BANK_ID"].ToString();
                BankName = row["BANK_NAME"].ToString();
                contractCode = row["CONTRACT_CODE"].ToString();
                appointmentName = row["APPOINTMENT_NAME"].ToString();
                appointment_id = Convert.ToInt32(row["APPOINTMENT_ID"].ToString());
                Date = row["OPERATION_DATE"].ToString().Substring(0, 10);
                operationTypeName = (Convert.ToInt32(row["OPERATION_TYPE_ID"].ToString()) == 1) ? "Mədaxil" : "Məxaric";
                Amount = (Convert.ToInt32(row["OPERATION_TYPE_ID"].ToString()) == 1) ? Convert.ToDecimal(row["INCOME"].ToString()) : Convert.ToDecimal(row["EXPENSES"].ToString());
                RepeatBarButton.Enabled = (GlobalVariables.RepeatBank && (appointment_id == 3 || appointment_id == 8 || appointment_id == 19));
            }
        }

        private void EditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFBankOperationAddEdit("EDIT", OperationID, BankID, Date, BankName);
        }

        private void BankGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFBankOperationAddEdit("EDIT", OperationID, BankID, Date, BankName);
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(BankGridControl);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "xls");
        }

        private void OperationTypeComboBox_EditValueChanged(object sender, EventArgs e)
        {
            operation_name = " [OPERATION_NAME] IN ('" + OperationTypeComboBox.Text.Replace("; ", "','") + "')";
            FilterBanks();
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(BankGridControl, "mht");
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
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

        private void BankGridView_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            GlobalProcedures.GenerateAutoRowNumber(sender, BankOperations_SS, e);
        }

        private void BankGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(BankGridView, BankRibbonPage.Text);
        }

        private void BankGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(BankGridView, PopupMenu, e);
        }

        private void RepeatBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = BankGridView.TopRowIndex;
            old_row_num = BankGridView.FocusedRowHandle;
            FBankRepeat fbr = new FBankRepeat();
            fbr.BankName = BankName;
            fbr.PaymentDate = Date;
            fbr.Appointment = appointmentName;
            fbr.OperationType = operationTypeName;
            fbr.Amount = Amount;
            fbr.AppointmentID = appointment_id;
            fbr.OperationID = OperationID;
            fbr.OldBankID = BankID;
            fbr.ContractCode = contractCode;
            fbr.RefreshDataGridView += new FBankRepeat.DoEvent(RefreshBank);
            fbr.ShowDialog();
            BankGridView.TopRowIndex = topindex;
            BankGridView.FocusedRowHandle = old_row_num;
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = BankGridView.TopRowIndex;
            old_row_num = BankGridView.FocusedRowHandle;
            if (appointment_id != 3)
            {
                DialogResult dialogResult = XtraMessageBox.Show(Date + " tarixinə olan bütün əməliyyatları silmək istəyirsiniz? Əgər bu əməliyyatları silsəz, onları geri qaytarmaq mümkün olmayacaq.", "Əməliyyatın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteProcedureWithTwoParametr("CRS_USER.PROC_DELETE_BANK_OPERATION", "P_BANK_ID", BankID, "P_DATE", GlobalFunctions.ConvertDateToInteger(Date, "ddmmyyyy"), "Bank əməliyyatı silinmədi.");
                    GlobalProcedures.UpdateCashDebt(Date);
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE BANK_ID = {BankID} AND OPERATION_DATE = TO_DATE('{Date}','DD/MM/YYYY')", "Əməliyyat silinmədi", this.Name +  "/DeleteBarButton_ItemClick");
                    GlobalProcedures.UpdateBankOperationDebtWithBank(Date, int.Parse(BankID));
                    GlobalProcedures.UpdateBankOperationDebt(Date);
                }
            }
            else
                XtraMessageBox.Show(Date + " tarixində Lizinq ödənişləri olduğu üçün bu tarixdəki əməliyyatların hamısını birdən silmək olmaz. Lizinq ödənişini yalnız müştərinin ödənişləri cədvəlindən silmək olar.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            LoadBankDataGridView();
            BankGridView.TopRowIndex = topindex;
            BankGridView.FocusedRowHandle = old_row_num;
        }

        private void BankGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void BankGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaligi hesabliyir 
                {
                    List<BankOperations> lstOperations = BankOperationsDAL.SelectBankOperation(null).ToList<BankOperations>();
                    if (lstOperations.Count > 0)
                        e.TotalValue = lstOperations.LastOrDefault().DEBT;
                    else
                        e.TotalValue = 0;
                }
            }
        }

        private void FilterBanks()
        {
            if (FormStatus)
            {
                ColumnView view = BankGridView;

                //operation
                if (!String.IsNullOrEmpty(OperationTypeComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["OPERATION_NAME"],
                        new ColumnFilterInfo(operation_name, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["OPERATION_NAME"]);
            }
        }

        private void ClearFilterBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            OperationTypeComboBox.Text = null;
            BankGridView.ClearColumnsFilter();
        }

        private void BankGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (BankGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditBank;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteBank;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
                CalcDebtBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = CalcDebtBarButton.Enabled = false;
        }

        private void CalcDebtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));            
            topindex = BankGridView.TopRowIndex;
            old_row_num = BankGridView.FocusedRowHandle;
            DataSet ds = BankOperationsDAL.SelectBankOperationDistinctBank(Date);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GlobalProcedures.UpdateBankOperationDebtWithBank(Date, int.Parse(dr["BANK_ID"].ToString()));
            }
            GlobalProcedures.UpdateBankOperationDebt(Date);
            XtraMessageBox.Show(Date + " tarixindən sonrakı tarixlər üçün hər bir bankın qalığı yenidən hesablandı.");
            LoadBankDataGridView();
            BankGridView.TopRowIndex = topindex;
            BankGridView.FocusedRowHandle = old_row_num;
            GlobalProcedures.SplashScreenClose();
        }
    }
}