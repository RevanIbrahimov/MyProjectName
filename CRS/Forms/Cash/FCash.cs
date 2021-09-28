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
using System.Globalization;
using CRS.Class;

namespace CRS.Forms.Cash
{
    public partial class FCash : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FCash()
        {
            InitializeComponent();
        }
        string OperationOwnerID, OperationID, OperationDate, OperationName, destination_name = null;
        int DestinationID, IncExp, topindex, old_row_num, UsedUserID;
        bool FormStatus = false;

        private void IncomeBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFIncomeAddEdit("INSERT", null, null, 0);
        }

        void RefreshCash(string operationdate)
        {
            GlobalProcedures.UpdateCashDebt(operationdate);
            LoadCashDataGridView();
            LoadTotalDataGridView();
            TodayDebt();
        }

        private void LoadFIncomeAddEdit(string transaction, string operation_id, string owner_id, int index)
        {
            topindex = CashGridView.TopRowIndex;
            old_row_num = CashGridView.FocusedRowHandle;
            FIncomeAddEdit fiae = new FIncomeAddEdit();
            fiae.TransactionName = transaction;
            fiae.OperationOwnerID = owner_id;
            fiae.OperationID = operation_id;
            fiae.Index = index;
            fiae.RefreshCashDataGridView += new FIncomeAddEdit.DoEvent(RefreshCash);
            fiae.ShowDialog();
            CashGridView.TopRowIndex = topindex;
            CashGridView.FocusedRowHandle = old_row_num;
        }

        private void FCash_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                IncomeBarButton.Enabled = GlobalVariables.AddCashIncome;
                ExpensesBarButton.Enabled = GlobalVariables.AddCashExpenses;
                OperationBarButton.Enabled = GlobalVariables.EditCash;
                DeleteBarButton.Enabled = GlobalVariables.DeleteCash;
                PrintBarButton.Enabled = GlobalVariables.PrintCash;
                ExportBarSubItem.Enabled = GlobalVariables.ExportCash;
                ReceiptBarSubItem.Enabled = GlobalVariables.ReceiptCash;
            }
            ProgressPanel.Hide();
            GlobalProcedures.FillCheckedComboBox(DestinationComboBox, "CASH_DESTINATION", "NAME,NAME_EN,NAME_RU", "1 = 1 ORDER BY ID");
            TodayDebtGroupControl.Text = "Bu günə (" + DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ) + ") olan qalıq";
            SearchDockPanel.Hide();
            FormStatus = true;
        }

        private void LoadCashDataGridView()
        {
            string s = null;

            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        s = "SELECT 1 SS,CO.ID,D.YEAR_ID YEAR,D.AZ_QUARTER_OF_YEAR_NAME,D.AZ_MONTH_NAME,D.CALENDAR_DATE O_DATE,CD.NAME DESTINATION,CO.CONTRACT_CODE,CO.INCOME,CO.EXPENSES,CO.DEBT,CO.OPERATION_OWNER_ID,CO.USED_USER_ID,CO.DESTINATION_ID,CD.INC_EXP,TO_CHAR(D.CALENDAR_DATE,'YYYYMMDD') OPERATION_DATE,ROW_NUMBER() OVER (ORDER BY D.CALENDAR_DATE,CO.ID) ROW_NUM FROM CRS_USER.CASH_OPERATIONS CO, CRS_USER.CASH_DESTINATION CD,CRS_USER.DIM_TIME D WHERE CO.IS_COMMIT = 1 AND CO.DESTINATION_ID = CD.ID AND CO.OPERATION_DATE = D.CALENDAR_DATE ORDER BY D.DATE_ORDER DESC, CO.ID";
                        break;
                    case "EN":
                        s = "SELECT 1 SS,CO.ID,D.YEAR_ID YEAR,D.QUARTER_OF_YEAR_NAME,D.MONTH_NAME,D.CALENDAR_DATE O_DATE,CD.NAME_EN DESTINATION,CO.CONTRACT_CODE,CO.INCOME,CO.EXPENSES,CO.DEBT,CO.OPERATION_OWNER_ID,CO.USED_USER_ID,CO.DESTINATION_ID,CD.INC_EXP,TO_CHAR(D.CALENDAR_DATE,'YYYYMMDD') OPERATION_DATE,ROW_NUMBER() OVER (ORDER BY D.CALENDAR_DATE,CO.ID) ROW_NUM FROM CRS_USER.CASH_OPERATIONS CO, CRS_USER.CASH_DESTINATION CD,CRS_USER.DIM_TIME D WHERE CO.IS_COMMIT = 1 AND CO.DESTINATION_ID = CD.ID AND CO.OPERATION_DATE = D.CALENDAR_DATE ORDER BY D.DATE_ORDER DESC, CO.ID";
                        break;
                    case "RU":
                        s = "SELECT 1 SS,CO.ID,D.YEAR_ID YEAR,D.QUARTER_OF_YEAR_NAME,D.RU_MONTH_NAME,D.CALENDAR_DATE O_DATE,CD.NAME_RU DESTINATION,CO.CONTRACT_CODE,CO.INCOME,CO.EXPENSES,CO.DEBT,CO.OPERATION_OWNER_ID,CO.USED_USER_ID,CO.DESTINATION_ID,CD.INC_EXP,TO_CHAR(D.CALENDAR_DATE,'YYYYMMDD') OPERATION_DATE,ROW_NUMBER() OVER (ORDER BY D.CALENDAR_DATE,CO.ID) ROW_NUM FROM CRS_USER.CASH_OPERATIONS CO, CRS_USER.CASH_DESTINATION CD,CRS_USER.DIM_TIME D WHERE CO.IS_COMMIT = 1 AND CO.DESTINATION_ID = CD.ID AND CO.OPERATION_DATE = D.CALENDAR_DATE ORDER BY D.DATE_ORDER DESC, CO.ID";
                        break;
                }

                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCashDataGridView");

                CashGridControl.DataSource = dt;

                if (YearGroupBarButton.Down)
                    CashGridView.Columns[2].GroupIndex = 0;
                else
                    CashGridView.Columns[2].GroupIndex = -1;

                if (QuarterGroupBarButton.Down)
                    CashGridView.Columns[3].GroupIndex = 1;
                else
                    CashGridView.Columns[3].GroupIndex = -1;

                if (MonthGroupBarButton.Down)
                    CashGridView.Columns[4].GroupIndex = 2;
                else
                    CashGridView.Columns[4].GroupIndex = -1;

                if (ExpandBarButton.Down)
                    CashGridView.CollapseAllGroups();
                else if (CollapseBarButton.Down)
                    CashGridView.ExpandAllGroups();

                if (CashGridView.RowCount > 0)
                {
                    if (GlobalVariables.V_UserID > 0)
                    {
                        OperationBarButton.Enabled = GlobalVariables.EditCash;
                        DeleteBarButton.Enabled = GlobalVariables.DeleteCash;
                    }
                    else
                        OperationBarButton.Enabled = DeleteBarButton.Enabled = true;

                    YearGroupBarButton.Enabled = QuarterGroupBarButton.Enabled = MonthGroupBarButton.Enabled = true;
                    ShowReceiptBarButton(DestinationID);
                }
                else
                    OperationBarButton.Enabled = DeleteBarButton.Enabled = YearGroupBarButton.Enabled = QuarterGroupBarButton.Enabled = MonthGroupBarButton.Enabled = false;

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Kassa əməliyyatlarının siyahısı cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadTotalDataGridView()
        {
            string s = null;
            try
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        s = "SELECT CD.NAME NAME,SUM(CO.INCOME) INCOME,SUM(CO.EXPENSES) EXPENSE FROM CRS_USER.CASH_OPERATIONS CO,CRS_USER.CASH_DESTINATION CD WHERE CO.DESTINATION_ID = CD.ID GROUP BY CD.NAME";
                        break;
                    case "EN":
                        s = "SELECT CD.NAME_EN NAME,SUM(CO.INCOME) INCOME,SUM(CO.EXPENSES) EXPENSE FROM CRS_USER.CASH_OPERATIONS CO,CRS_USER.CASH_DESTINATION CD WHERE CO.DESTINATION_ID = CD.ID GROUP BY CD.NAME";
                        break;
                    case "RU":
                        s = "SELECT CD.NAME_RU NAME,SUM(CO.INCOME) INCOME,SUM(CO.EXPENSES) EXPENSE FROM CRS_USER.CASH_OPERATIONS CO,CRS_USER.CASH_DESTINATION CD WHERE CO.DESTINATION_ID = CD.ID GROUP BY CD.NAME";
                        break;
                }
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name);

                TotalGridControl.DataSource = dt;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Təyinata görə cəmlər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FCash_Activated(object sender, EventArgs e)
        {
            LoadCashDataGridView();
            LoadTotalDataGridView();
            TodayDebt();
            CurrencyRateBarStatic.Caption = GlobalVariables.V_LastRate;
            CashGridView.FocusedRowHandle = CashGridView.RowCount - 1;

            GlobalProcedures.GridRestoreLayout(CashGridView, CashRibbonPage.Text);
        }

        private void CashGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "DEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void CashGridView_CustomDrawRowFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "O_DATE")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
        }

        private void CashGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaligi hesabliyir                     
                    e.TotalValue = GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER.CASH_OPERATIONS WHERE ID = (SELECT MAX(ID) FROM CRS_USER.CASH_OPERATIONS WHERE OPERATION_DATE = (SELECT MAX(OPERATION_DATE) FROM CRS_USER.CASH_OPERATIONS WHERE IS_COMMIT = 1))");                
            }
        }

        private void YearGroupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadCashDataGridView();
        }

        private void CashGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "DEBT")
            {
                e.Column.AppearanceHeader.FontStyleDelta = FontStyle.Bold;
                e.Column.AppearanceHeader.ForeColor = Color.Red;
                e.Column.AppearanceCell.FontStyleDelta = FontStyle.Bold;
                decimal value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DEBT"));
                if (value < 0)
                    e.Appearance.ForeColor = Color.Red;
            }

            GlobalProcedures.GridRowCellStyleForBlock(CashGridView, e);
        }

        private void TodayDebt()
        {
            double today_income = 0, 
                today_expenses = 0, 
                yesteday_debt = GlobalFunctions.GetAmount("SELECT DEBT FROM CRS_USER.CASH_OPERATIONS WHERE ID = (SELECT MAX(ID) FROM CRS_USER.CASH_OPERATIONS WHERE OPERATION_DATE = (SELECT MAX(OPERATION_DATE) FROM CRS_USER.CASH_OPERATIONS WHERE IS_COMMIT = 1 AND TO_CHAR(OPERATION_DATE,'YYYYMMDD') < TO_CHAR(SYSDATE,'YYYYMMDD')))");

            string sql = $@"SELECT NVL (SUM (INCOME), 0) INCOME,NVL (SUM (EXPENSES), 0) EXPENSES
                                  FROM CRS_USER.CASH_OPERATIONS CO
                                 WHERE     CO.IS_COMMIT = 1
                                       AND TO_CHAR (CO.OPERATION_DATE, 'YYYYMMDD') =
                                              TO_CHAR (SYSDATE, 'YYYYMMDD')";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/TodayDebt");
            
            today_income = Convert.ToDouble(dt.Rows[0]["INCOME"].ToString());
            today_expenses = Convert.ToDouble(dt.Rows[0]["EXPENSES"].ToString());

            YestedayDebtValue.EditValue = yesteday_debt;
            IncomeValue.EditValue = today_income;
            ExpensesValue.EditValue = today_expenses;
            TodayDebtValue.EditValue = yesteday_debt + today_income - today_expenses;
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadCashDataGridView();
            LoadTotalDataGridView();
            TodayDebt();
        }

        private void CashGridControl_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CashGridView, CashPopupMenu, e);
        }

        private void CashGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CashGridView.GetFocusedDataRow();
            if (row != null)
            {
                OperationID = row["ID"].ToString();
                OperationOwnerID = row["OPERATION_OWNER_ID"].ToString();
                OperationDate = row["O_DATE"].ToString().Substring(0, 10).Trim();
                OperationName = row["DESTINATION"].ToString();
                DestinationID = Convert.ToInt32(row["DESTINATION_ID"].ToString());
                IncExp = Convert.ToInt32(row["INC_EXP"].ToString());
                UsedUserID = Convert.ToInt32(row["USED_USER_ID"].ToString());
                ShowReceiptBarButton(DestinationID);
            }
        }

        private void OperationBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (UsedUserID != GlobalVariables.V_UserID)
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;                
                XtraMessageBox.Show("Seçdiyiniz kassa əməliyyatı " + used_user_name + " tərəfindən bloklandığı üçün seçdiyiniz " + OperationDate + " tarixinə olan " + OperationName + " əməliyyatını dəyişmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (IncExp == 1)
            {
                if (DestinationID != 2)
                    LoadFIncomeAddEdit("EDIT", OperationID, OperationOwnerID, DestinationID);
                else
                    XtraMessageBox.Show(OperationName + " əməliyyatı müqavilədn avtomatik olaraq kassaya daxil olunduğu üçün bu əməliyyatı dəyişmək olmaz. Bu əməliyyatı dəyişmək üçün əvvəlcə uyğun müqavilə dəyişdirilməlidir.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                LoadFExpensesAddEdit("EDIT", OperationID, OperationOwnerID, DestinationID);
        }

        private void CashGridView_DoubleClick(object sender, EventArgs e)
        {
            if (OperationBarButton.Enabled)
            {
                if (IncExp == 1)
                {
                    if (DestinationID != 2)
                        LoadFIncomeAddEdit("EDIT", OperationID, OperationOwnerID, DestinationID);
                    else
                        XtraMessageBox.Show(OperationName + " əməliyyatı müqavilədn avtomatik olaraq kassaya daxil olunduğu üçün bu əməliyyatı dəyişmək olmaz. Bu əməliyyatı dəyişmək üçün əvvəlcə uyğun müqavilə dəyişdirilməlidir.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    LoadFExpensesAddEdit("EDIT", OperationID, OperationOwnerID, DestinationID);
            }
        }

        private void DeleteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string message = null;
            
            topindex = CashGridView.TopRowIndex;
            old_row_num = CashGridView.FocusedRowHandle;
            if (UsedUserID == -1 || UsedUserID == GlobalVariables.V_UserID)
            {
                if (DestinationID != 2)
                {
                    if (DestinationID == 1)
                    {
                        XtraMessageBox.Show("Lizinq ödənişini yalnız müştərinin ödənişləri cədvəlindən silmək olar.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;                        
                    }
                    else
                        message = OperationDate + " tarixinə olan " + OperationName + " əməliyyatını silmək istəyirsiniz?";

                    DialogResult dialogResult = XtraMessageBox.Show(message, "Əməliyyatın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ProgressPanel.Location = new Point(Bounds.X + Bounds.Width / 2 - ProgressPanel.Width / 2, Bounds.Y + Bounds.Height / 2 - ProgressPanel.Height / 2);
                        ProgressPanel.Description = "Kassanın qalığı hesablanır...";
                        ProgressPanel.Show();
                        Application.DoEvents();                           
                        
                        if (DestinationID == 3)
                        {
                            int funds_contract_id = GlobalFunctions.GetID($@"SELECT NVL(CONTRACT_ID,0) FROM CRS_USER.FUNDS_PAYMENTS WHERE ID = (SELECT FUND_PAYMENT_ID FROM CRS_USER.CASH_FOUNDER WHERE ID = {OperationOwnerID})");
                            GlobalProcedures.ExecuteThreeQuery($@"UPDATE CRS_USER.FUNDS_PAYMENTS SET BUY_AMOUNT = 0 WHERE ID = (SELECT FUND_PAYMENT_ID FROM CRS_USER.CASH_FOUNDER WHERE ID = {OperationOwnerID})",
                                                               $@"DELETE FROM CRS_USER.CASH_FOUNDER WHERE ID = {OperationOwnerID}",
                                                               $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                                    "Təsisçi ilə hesablaşma silinmədi.",
                                                                    this.Name + "/DeleteBarButton_ItemClick");
                            GlobalProcedures.UpdateFundChange(OperationDate, funds_contract_id, 0);                            
                        }
                        else if (DestinationID == 8)
                        {
                            int funds_contract_id = GlobalFunctions.GetID($@"SELECT NVL(CONTRACT_ID,0) FROM CRS_USER.FUNDS_PAYMENTS WHERE ID = (SELECT FUND_PAYMENT_ID FROM CRS_USER.CASH_FOUNDER WHERE ID = {OperationOwnerID})");
                            GlobalProcedures.ExecuteThreeQuery($@"UPDATE CRS_USER.FUNDS_PAYMENTS SET PAYMENT_AMOUNT = 0 WHERE ID = (SELECT FUND_PAYMENT_ID FROM CRS_USER.CASH_FOUNDER WHERE ID = {OperationOwnerID})",
                                                               $@"DELETE FROM CRS_USER.CASH_FOUNDER WHERE ID = {OperationOwnerID}",
                                                               $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                                    "Təsisçi ilə hesablaşma silinmədi.",
                                                                    this.Name + "/DeleteBarButton_ItemClick");
                            GlobalProcedures.UpdateFundChange(OperationDate, funds_contract_id, 0);                            
                        }
                        else if (DestinationID == 4)
                        {
                            string s_delete = $@"SELECT BANK_ID,TO_CHAR(ADATE,'DD.MM.YYYY') ADATE,BANK_OPERATION_ID FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 1 AND ID = {OperationOwnerID}";
                            try
                            {
                                DataTable dt = GlobalFunctions.GenerateDataTable(s_delete, this.Name + "/DeleteBarButton_ItemClick");

                                foreach (DataRow dr in dt.Rows)
                                {
                                    GlobalProcedures.ExecuteThreeQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE APPOINTMENT_ID = 7 AND BANK_ID = {dr["BANK_ID"]} AND OPERATION_DATE = TO_DATE('{dr["ADATE"]}','DD/MM/YYYY') AND ID = {dr["BANK_OPERATION_ID"]}",
                                                                       $@"DELETE FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 1 AND ID = {OperationOwnerID}",
                                                                       $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                                            "Bank hesabının mədaxili bank əməliyyatlarından silinmədi.",
                                                                       this.Name + "/DeleteBarButton_ItemClick");
                                    GlobalProcedures.UpdateBankOperationDebtWithBank(dr["ADATE"].ToString(), Convert.ToInt32(dr["BANK_ID"].ToString()));
                                    GlobalProcedures.UpdateBankOperationDebt(dr["ADATE"].ToString());
                                }
                            }
                            catch (Exception exx)
                            {
                                GlobalProcedures.LogWrite("Bank hesabının mədaxili tapılmadı.", s_delete, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                            }
                            //GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 1 AND ID = {OperationOwnerID}",
                            //                                 $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                            //                                "Bank hesabının mədaxili silinmədi.");
                        }
                        else if (DestinationID == 5)
                            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_OTHER_PAYMENT WHERE ID = {OperationOwnerID}",
                                                             $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                                "Digər ödənişlərin mədaxili silinmədi.",
                                                                this.Name + "/DeleteBarButton_ItemClick");
                        else if (DestinationID == 6)
                            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_SERVICE_PRICE WHERE ID = {OperationOwnerID}",
                                                             $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                                    "Xidmət haqqının mədaxili silinmədi.",
                                                                    this.Name + "/DeleteBarButton_ItemClick");
                        else if (DestinationID == 7)
                            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_EXPENSES_PAYMENT WHERE ID = {OperationOwnerID}",
                                                             $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                                "Lizinq müqaviləsi üzrə məxaric silinmədi.",
                                                             this.Name + "/DeleteBarButton_ItemClick");
                        else if (DestinationID == 9)
                        {
                            string s_delete = $@"SELECT BANK_ID,TO_CHAR(ADATE,'DD.MM.YYYY') ADATE,BANK_OPERATION_ID FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 2 AND ID = {OperationOwnerID}";
                            try
                            {
                                DataTable dt = GlobalFunctions.GenerateDataTable(s_delete, this.Name + "/DeleteBarButton_ItemClick");

                                foreach (DataRow dr in dt.Rows)
                                {
                                    GlobalProcedures.ExecuteThreeQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE APPOINTMENT_ID = 4 AND BANK_ID = {dr[0]} AND OPERATION_DATE = TO_DATE('{dr["ADATE"]}','DD/MM/YYYY') AND ID = {dr["BANK_OPERATION_ID"]}",
                                                                       $@"DELETE FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 2 AND ID = {OperationOwnerID}",
                                                                       $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                                            "Bank hesabının məxarici bank əməliyyatlarından silinmədi.",
                                                                       this.Name + "/DeleteBarButton_ItemClick");
                                    GlobalProcedures.UpdateBankOperationDebtWithBank(dr["ADATE"].ToString(), Convert.ToInt32(dr["BANK_ID"].ToString()));
                                    GlobalProcedures.UpdateBankOperationDebt(dr["ADATE"].ToString());
                                }
                            }
                            catch (Exception exx)
                            {
                                GlobalProcedures.LogWrite("Bank hesabının məxarici tapılmadı.", s_delete, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                            }
                            //GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 2 AND ID = {OperationOwnerID}",
                            //                                $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                            //                                    "Bank hesabının məxarici silinmədi.");
                        }
                        else if (DestinationID == 10)
                            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_EXPENSES_OTHER_PAYMENT WHERE ID = {OperationOwnerID}",
                                                            $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                            "Digər ödənişlərin məxarici silinmədi.",
                                                            this.Name + "/DeleteBarButton_ItemClick");
                        else if (DestinationID == 11)
                            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_EXPENSES_SERVICE_PRICE WHERE ID = {OperationOwnerID}",
                                                            $@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE ID = {OperationID}",
                                                            "Xidmət haqqının məxarici silinmədi.",
                                                            this.Name + "/DeleteBarButton_ItemClick");
                        
                        GlobalProcedures.UpdateCashDebt(OperationDate);
                        ProgressPanel.Hide();
                    }
                }
                else
                    XtraMessageBox.Show(OperationName + " əməliyyatı müqavilədən avtomatik olaraq kassaya daxil olunduğu üçün bu əməliyyatı silmək olmaz. Bu əməliyyatı silmək üçün əvvəlcə uyğun lizinq müqaviləsi bazadan silinməlidir.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                LoadCashDataGridView();
                LoadTotalDataGridView();
                TodayDebt();
            }
            else
            {
                string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                XtraMessageBox.Show("Seçdiyiniz kassa əməliyyatı " + used_user_name + " tərəfindən bloklandığından seçdiyiniz " + OperationDate + " tarixinə olan " + OperationName + " əməliyyatını silmək olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            CashGridView.TopRowIndex = topindex;
            CashGridView.FocusedRowHandle = old_row_num;
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(CashGridControl);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CashGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CashGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CashGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CashGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CashGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CashGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CashGridControl, "mht");
        }

        private void LoadFExpensesAddEdit(string transaction, string operation_id, string owner_id, int index)
        {
            topindex = CashGridView.TopRowIndex;
            old_row_num = CashGridView.FocusedRowHandle;
            FExpensesAddEdit fiae = new FExpensesAddEdit();
            fiae.TransactionName = transaction;
            fiae.OperationOwnerID = owner_id;
            fiae.OperationID = operation_id;
            fiae.Index = index;
            fiae.RefreshCashDataGridView += new FExpensesAddEdit.DoEvent(RefreshCash);
            fiae.ShowDialog();
            CashGridView.TopRowIndex = topindex;
            CashGridView.FocusedRowHandle = old_row_num;
        }

        private void ExpensesBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFExpensesAddEdit("INSERT", null, null, 0);
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

        private void DestinationComboBox_EditValueChanged(object sender, EventArgs e)
        {
            destination_name = " [DESTINATION] IN ('" + DestinationComboBox.Text.Replace("; ", "','") + "')";
            FilterTotals();
        }

        private void FilterTotals()
        {
            if (FormStatus)
            {
                ColumnView view = CashGridView;
                //destination
                if (!String.IsNullOrEmpty(DestinationComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["DESTINATION"],
                        new ColumnFilterInfo(destination_name, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["DESTINATION"]);

                //ContractCode
                if (!String.IsNullOrEmpty(ContractCodeText.Text))
                    view.ActiveFilter.Add(view.Columns["CONTRACT_CODE"],
                        new ColumnFilterInfo("[CONTRACT_CODE] Like '%" + ContractCodeText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CONTRACT_CODE"]);
            }
        }

        private void ContractCodeText_EditValueChanged(object sender, EventArgs e)
        {
            FilterTotals();
        }

        private void ClearFilterBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ContractCodeText.Text = null;
            DestinationComboBox.Text = null;
            CashGridView.ClearColumnsFilter();
        }

        private void CashGridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                int type = int.Parse(View.GetRowCellDisplayText(e.RowHandle, View.Columns["INC_EXP"]));
                GlobalProcedures.GridRowOperationTypeColor(type, e);               
            }
        }

        private void YestedayDebtValue_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as CalcEdit).Value < 0)
                (sender as CalcEdit).ForeColor = Color.Red;
            else
                (sender as CalcEdit).ForeColor = Class.GlobalFunctions.ElementColor();
        }

        private void CashGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (CashGridView.RowCount > 0)
            {
                if (Class.GlobalVariables.V_UserID > 0)
                {
                    OperationBarButton.Enabled = Class.GlobalVariables.EditCash;
                    DeleteBarButton.Enabled = Class.GlobalVariables.DeleteCash;
                }
                else
                    OperationBarButton.Enabled = DeleteBarButton.Enabled = true;

                YearGroupBarButton.Enabled = QuarterGroupBarButton.Enabled = MonthGroupBarButton.Enabled = true;
            }
            else
                OperationBarButton.Enabled = DeleteBarButton.Enabled = YearGroupBarButton.Enabled = QuarterGroupBarButton.Enabled = MonthGroupBarButton.Enabled = false;

        }

        private void GenerateIncomeReceipt(int destination)
        {
            string sql = null, error = null, description = null;
            switch (destination)
            {
                case 1:
                    {
                        sql = "SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD.MM.YYYY'),CP.PAYMENT_AMOUNT_AZN,C.CURRENCY_ID,CP.CURRENCY_RATE,CUS.SURNAME||' '||CUS.NAME||' '||CUS.PATRONYMIC CUSTOMERFULLNAME,CP.CONTRACT_ID,CT.CODE || C.CODE CONTRACT_CODE FROM CRS_USER.CUSTOMER_PAYMENTS CP,CRS_USER.CONTRACTS C,CRS_USER.CUSTOMERS CUS,CRS_USER.CREDIT_TYPE CT WHERE CP.CONTRACT_ID = C.ID AND CP.CUSTOMER_ID = CUS.ID AND C.CUSTOMER_ID = CUS.ID AND C.CREDIT_TYPE_ID = CT.ID AND CP.ID = " + OperationOwnerID;
                        error = "Lizinq ödənişinin rekvizitləri tapılmadı.";
                        description = "saylı linzq müqaviləsi üzrə lizinq ödənişi";
                    }
                    break;
                case 2:
                    {
                        sql = "SELECT TO_CHAR(C.START_DATE,'DD.MM.YYYY'),CAP.AMOUNT,1,1,CUS.SURNAME||' '||CUS.NAME||' '||CUS.PATRONYMIC CUSTOMERFULLNAME,CAP.CONTRACT_ID,CT.CODE||C.CODE CONTRACT_CODE FROM CRS_USER.CASH_ADVANCE_PAYMENTS CAP,CRS_USER.CONTRACTS C,CRS_USER.CUSTOMERS CUS,CRS_USER.CREDIT_TYPE CT WHERE CAP.CONTRACT_ID = C.ID AND CAP.CUSTOMER_ID = CUS.ID AND C.CUSTOMER_ID = CUS.ID AND C.CREDIT_TYPE_ID = CT.ID AND CAP.ID = " + OperationOwnerID;
                        error = "Avans ödənişinin rekvizitləri tapılmadı.";
                        description = "saylı linzq müqaviləsi üzrə avans ödənişi";
                    }
                    break;
                case 3:
                    {
                        sql = "SELECT TO_CHAR(CF.PAYMENT_DATE,'DD.MM.YYYY'),CF.AMOUNT,1 CURRENCY_ID,FP.CURRENCY_RATE,F.FULLNAME,0 CONTRACT_ID,NULL CONTRACT_CODE FROM CRS_USER.CASH_FOUNDER CF,CRS_USER.FUNDS_PAYMENTS FP,CRS_USER.FOUNDERS F WHERE CF.INC_EXP = 1 AND CF.FOUNDER_ID = F.ID AND CF.FUND_PAYMENT_ID = FP.ID AND CF.ID = " + OperationOwnerID;
                        error = "Təsisçi ilə hesablaşmanın mədaxilinin rekvizitləri tapılmadı.";
                        description = "Təsisçi tərəfindən mədaxil";
                    }
                    break;
                case 4:
                    {
                        sql = $@"SELECT TO_CHAR (CB.ADATE, 'DD.MM.YYYY'),
                                       CB.AMOUNT,
                                       1 CURRENCY_ID,
                                       1 CURRENCY_RATE,
                                       B.SHORT_NAME || ', ' || B.ACCOUNT FULLNAME,
                                       0 CONTRACT_ID,
                                       NULL CONTRACT_CODE
                                  FROM CRS_USER.CASH_BANK_ACCOUNT CB, CRS_USER.BANKS B
                                 WHERE CB.INC_EXP = 1 AND CB.BANK_ID = B.ID AND CB.ID = {OperationOwnerID}";
                        error = "Bank hesabının mədaxilinin rekvizitləri tapılmadı.";
                        description = "Bank hesabından mədaxil";
                    }
                    break;
                case 5:
                    {
                        sql = "SELECT TO_CHAR(CO.PAYMENT_DATE,'DD.MM.YYYY'),CO.AMOUNT,1 CURRENCY_ID,1 CURRENCY_RATE,CO.CUSTOMER_NAME,0 CONTRACT_ID, NULL CONTRACT_CODE,CA.NAME FROM CRS_USER.CASH_OTHER_PAYMENT CO,CRS_USER.CASH_APPOINTMENTS CA WHERE  CO.CASH_APPOINTMENT_ID = CA.ID AND CO.ID = " + OperationOwnerID;
                        error = "Digər ödənişlərin mədaxilinin rekvizitləri tapılmadı.";
                        description = null;
                    }
                    break;
                case 6:
                    {
                        sql = "SELECT TO_CHAR(CO.PAYMENT_DATE,'DD.MM.YYYY'),CO.AMOUNT,1 CURRENCY_ID,1 CURRENCY_RATE,CO.CUSTOMER_NAME,0 CONTRACT_ID,NULL CONTRACT_CODE,CO.APPOINTMENT FROM CRS_USER.CASH_SERVICE_PRICE CO WHERE CO.ID = " + OperationOwnerID;
                        error = "Xidmət haqqının mədaxilinin rekvizitləri tapılmadı.";
                        description = null;
                    }
                    break;
            }

            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql, this.Name + "/GenerateIncomeReceipt").Rows)
                {
                    if (String.IsNullOrEmpty(description))
                        description = dr[7].ToString();
                    GlobalProcedures.LoadReceipt(dr[0].ToString(), Convert.ToDouble(dr[1].ToString()), Convert.ToInt32(dr[2].ToString()), Convert.ToDecimal(dr[3].ToString()), dr[4].ToString(), int.Parse(dr[5].ToString()), dr[6].ToString(), description);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite(error, sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void GenerateExpensesReceipt(int destination)
        {
            string sql = null, error = null, reason = null;
            switch (destination)
            {
                case 7:
                    {
                        sql = "SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD/MM/YYYY') P_DATE," +
                                    "(CASE WHEN CP.CURRENCY_ID = 1 THEN CP.AMOUNT ELSE CP.AMOUNT_AZN END) AMOUNT," +
                                    "(CASE WHEN CP.PERSONNEL_ID = 0 THEN S.SURNAME|| ' ' || S.NAME || ' ' || S.PATRONYMIC " +
                                        "ELSE P.SURNAME || ' ' || P.NAME || ' ' || P.PATRONYMIC END) SELLERNAME," +
                                    "(CASE WHEN CP.PERSONNEL_ID = 0 THEN CS.NAME||':'||CS.SERIES||', №: '||S.CARD_NUMBER||', '|| TO_CHAR (S.CARD_ISSUE_DATE, 'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' " +
                                        "ELSE (SELECT CS.NAME||' : '||CS.SERIES||', №:'||FC.CARD_NUMBER||', '||TO_CHAR (FC.ISSUE_DATE, 'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' FROM CRS_USER.PERSONNEL_CARDS FC,CRS_USER.CARD_SERIES CS,CRS_USER.CARD_ISSUING CI WHERE FC.CARD_SERIES_ID = CS.ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.ID = CP.PERSONNEL_CARD_ID) END) SELLER_CARD," +
                                    "CP.CONTRACT_CODE " +
                            "FROM CRS_USER.CASH_EXPENSES_PAYMENT CP,CRS_USER.SELLERS S,CRS_USER.CURRENCY C,CRS_USER.CARD_ISSUING CI,CRS_USER.CARD_SERIES CS,CRS_USER.PERSONNEL P " +
                            "WHERE CP.SELLER_ID = S.ID AND CP.CURRENCY_ID = C.ID AND S.CARD_SERIES_ID = CS.ID AND S.CARD_ISSUING_ID = CI.ID AND CP.PERSONNEL_ID = P.ID(+) AND CP.ID = " + OperationOwnerID;
                        error = "Lizinq üzrə məxaricin rekvizitləri tapılmadı.";
                    }
                    break;
                case 8:
                    {
                        sql = "SELECT TO_CHAR (CF.PAYMENT_DATE, 'DD/MM/YYYY')," +
                                    "CF.AMOUNT," +
                                    "F.FULLNAME," +
                                    "CS.NAME||' : '||CS.SERIES||', №:'||FC.CARD_NUMBER||', '||TO_CHAR(FC.ISSUE_DATE, 'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' FC_CARD," +
                                    "CF.CODE " +
                                "FROM CRS_USER.CASH_FOUNDER CF,CRS_USER.FOUNDERS F,CRS_USER.FOUNDER_CARDS FC,CRS_USER.CARD_ISSUING CI,CRS_USER.CARD_SERIES CS " +
                                "WHERE CF.FOUNDER_ID = F.ID AND CF.FOUNDER_CARD_ID = FC.ID AND F.ID = FC.FOUNDER_ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.CARD_SERIES_ID = CS.ID " +
                                        "AND CF.ID = " + OperationOwnerID;
                        error = "Təsisçi ilə hesablaşmanın məxaricinin rekvizitləri tapılmadı.";
                    }
                    break;
                case 9:
                    {
                        sql = "SELECT TO_CHAR (CF.PAYMENT_DATE, 'DD/MM/YYYY')," +
                                    "CF.AMOUNT," +
                                    "F.FULLNAME," +
                                    "CS.NAME||' : '||CS.SERIES||', №:'||FC.CARD_NUMBER||', '||TO_CHAR(FC.ISSUE_DATE, 'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' FC_CARD," +
                                    "CF.CODE " +
                                "FROM CRS_USER.CASH_FOUNDER CF,CRS_USER.FOUNDERS F,CRS_USER.FOUNDER_CARDS FC,CRS_USER.CARD_ISSUING CI,CRS_USER.CARD_SERIES CS " +
                                "WHERE CF.FOUNDER_ID = F.ID AND CF.FOUNDER_CARD_ID = FC.ID AND F.ID = FC.FOUNDER_ID AND FC.CARD_ISSUING_ID = CI.ID AND FC.CARD_SERIES_ID = CS.ID " +
                                        "AND CF.ID = " + OperationOwnerID;
                        error = "Bank hesabının məxaricinin rekvizitləri tapılmadı.";
                    }
                    break;
                case 10:
                    {
                        sql = "SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD/MM/YYYY')," +
                                    "CP.AMOUNT," +
                                    "CP.FULLNAME," +
                                    "CS.NAME||' : '||CS.SERIES||', №:'||CP.CARD_NUMBER||', '||TO_CHAR(CP.CARD_ISSUE_DATE, 'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' CP_CARD," +
                                    "CP.CODE " +
                               "FROM CRS_USER.CASH_EXPENSES_OTHER_PAYMENT CP,CRS_USER.CARD_ISSUING CI,CRS_USER.CARD_SERIES CS " +
                               "WHERE CP.CARD_ISSUING_ID = CI.ID AND CP.CARD_SERIES_ID = CS.ID AND CP.ID = " + OperationOwnerID;
                        error = "Digər ödənişlərin məxaricinin rekvizitləri tapılmadı.";
                    }
                    break;
                case 11:
                    {
                        sql = "SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD/MM/YYYY')," +
                                    "CP.AMOUNT," +
                                    "CP.FULLNAME," +
                                    "CS.NAME||' : '||CS.SERIES||', №:'||CP.CARD_NUMBER||', '||TO_CHAR(CP.CARD_ISSUE_DATE, 'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' CP_CARD," +
                                    "CP.CODE " +
                               "FROM CRS_USER.CASH_EXPENSES_SERVICE_PRICE CP,CRS_USER.CARD_ISSUING CI,CRS_USER.CARD_SERIES CS " +
                               "WHERE CP.CARD_ISSUING_ID = CI.ID AND CP.CARD_SERIES_ID = CS.ID AND CP.ID = " + OperationOwnerID;
                        error = "Digər ödənişlərin məxaricinin rekvizitləri tapılmadı.";
                    }
                    break;
                case 12:
                    {
                        sql = "SELECT TO_CHAR (PS.PAYMENT_DATE, 'DD/MM/YYYY')," +
                                    "PS.AMOUNT," +
                                    "P.SURNAME||' '||P.NAME||' '||P.PATRONYMIC FULLNAME," +
                                    "CS.NAME||' : '||CS.SERIES||', №:'||PC.CARD_NUMBER||', '||TO_CHAR(PC.ISSUE_DATE, 'DD.MM.YYYY')||' tarixində '||CI.NAME||' tərəfindən verilib' PC_CARD," +
                                    "PS.CODE " +
                                "FROM CRS_USER.CASH_SALARY PS,CRS_USER.PERSONNEL P,CRS_USER.PERSONNEL_CARDS PC,CRS_USER.CARD_ISSUING CI,CRS_USER.CARD_SERIES CS " +
                                "WHERE PS.PERSONNEL_ID = P.ID AND PS.PERSONNEL_CARD_ID = PC.ID AND P.ID = PC.PERSONNEL_ID AND PC.CARD_ISSUING_ID = CI.ID AND PC.CARD_SERIES_ID = CS.ID AND PS.ID = " + OperationOwnerID;
                        error = "Əmək haqqının rekvizitləri tapılmadı.";
                    }
                    break;
            }

            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql, this.Name + "/GenerateExpensesReceipt").Rows)
                {
                    switch (destination)
                    {
                        case 7:
                            reason = dr[4].ToString() + " saylı müqavilə üzrə alqı satqı";
                            break;
                        case 8:
                            reason = "Təsisçi ilə hesablaşma";
                            break;
                        case 10:
                            reason = "Digər ödənişlər";
                            break;
                        case 11:
                            reason = "Xidmət haqqı";
                            break;
                        case 12:
                            reason = "Əmək haqqı";
                            break;
                    }
                    GlobalProcedures.LoadExpensesReceipt(dr[0].ToString(), Convert.ToDouble(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), reason);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite(error, sql, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void ShowReceiptBarButton(int destinaton)
        {
            switch (destinaton)
            {
                case 1:
                    {
                        ReceiptPaymentBarButton.Enabled = true;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 2:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = true;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 3:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = true;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 4:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = true;
                    }
                    break;
                case 5:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = true;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 6:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = true;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 7:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = true;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 8:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = true;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 10:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = true;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 11:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = false;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = true;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
                case 12:
                    {
                        ReceiptPaymentBarButton.Enabled = false;
                        ReceiptAdvanceBarButton.Enabled = false;
                        ReceiptIncOtherPaymentBarButton.Enabled = false;
                        ReceiptExpensesContractBarButton.Enabled = false;
                        ReceiptSalaryBarButton.Enabled = true;
                        ReceiptIncFounderBarButton.Enabled = false;
                        ReceiptExpFounderBarButton.Enabled = false;
                        ReceiptExpOtherPaymentBarButton.Enabled = false;
                        ReceiptIncServiceBarButton.Enabled = false;
                        ReceiptExpServiceBarButton.Enabled = false;
                        ReceiptBankIncBarButton.Enabled = false;
                    }
                    break;
            }
        }

        private void ReceiptPaymentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateIncomeReceipt(1);
        }

        private void ReceiptAdvanceBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateIncomeReceipt(2);
        }

        private void ReceiptIncFounderBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateIncomeReceipt(3);
        }

        private void ReceiptIncOtherPaymentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateIncomeReceipt(5);
        }

        private void ReceiptIncServiceBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateIncomeReceipt(6);
        }

        private void ReceiptExpensesContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateExpensesReceipt(7);
        }

        private void ReceiptExpFounderBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateExpensesReceipt(8);
        }

        private void CashGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(CashGridView, CashRibbonPage.Text);
        }

        private void CashGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            Class.GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        private void ReceiptExpOtherPaymentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateExpensesReceipt(10);
        }

        private void ReceiptExpServiceBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateExpensesReceipt(11);
        }

        private void ReceiptSalaryBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateExpensesReceipt(12);
        }

        private void ReceiptBankIncBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GenerateIncomeReceipt(4);
        }

        private void CalcDebtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = CashGridView.TopRowIndex;
            old_row_num = CashGridView.FocusedRowHandle;
            GlobalProcedures.UpdateCashDebt(OperationDate);
            XtraMessageBox.Show(OperationDate + " tarixindən sonrakı tarixlər üçün kassanın qalığı yenidən hesablandı.");
            LoadCashDataGridView();
            CashGridView.TopRowIndex = topindex;
            CashGridView.FocusedRowHandle = old_row_num;
        }

        private void ExpandBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            CollapseBarButton.Down = !ExpandBarButton.Down;
            LoadCashDataGridView();
        }

        private void CollapseBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ExpandBarButton.Down = !CollapseBarButton.Down;
            LoadCashDataGridView();
        }
    }
}