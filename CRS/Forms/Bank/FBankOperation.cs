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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class;
using Oracle.ManagedDataAccess.Client;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Bank
{
    public partial class FBankOperation : DevExpress.XtraEditors.XtraForm
    {
        public FBankOperation()
        {
            InitializeComponent();
        }
        public string TransactionName, OperationID, BankID, BankName, Date;

        int UsedUserID = -1, bank_id = 0, operation_type_id, appointment_id, topindex, old_row_num;
        bool Used = false, CurrentStatus = false;

        public delegate void DoEvent(string odate);
        public event DoEvent RefreshBankDataGridView;

        private void FBankOperationAddEdit_Load(object sender, EventArgs e)
        {
            ProgressPanel.Hide();
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                IncomeBarButton.Enabled = GlobalVariables.AddBankIncome;
                ExpensesBarButton.Enabled = GlobalVariables.AddBankExpenses;
                PrintBarButton.Enabled = GlobalVariables.PrintBank;
                ExportBarButton.Enabled = GlobalVariables.ExportBank;
            }

            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");

            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.BANK_OPERATIONS", GlobalVariables.V_UserID, "WHERE BANK_ID = " + BankID + " AND OPERATION_DATE = TO_DATE('" + Date + "','DD/MM/YYYY') AND USED_USER_ID = -1");
                List<BankOperations> lstBankOperations = BankOperationsDAL.SelectBankOperation(int.Parse(BankID)).ToList<BankOperations>();
                var bankoperations = lstBankOperations.Find(bo => bo.OPERATION_DATE == GlobalFunctions.ChangeStringToDate(Date, "ddmmyyyy"));
                UsedUserID = bankoperations.USED_USER_ID;
                Used = (UsedUserID > 0);

                if (Used)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş " + Date + " tarixinə olan əməliyyatlar hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş əməliyyatların hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;

                ComponentEnabled(CurrentStatus);
                InsertOperationsTemp();
                PaymentDate.Enabled =
                    BankLookUp.Enabled = false;
                PaymentDate.EditValue = GlobalFunctions.ChangeStringToDate(Date, "ddmmyyyy");
                BankLookUp.EditValue = BankLookUp.Properties.GetKeyValueByDisplayText(BankName);
            }
            else
            {
                PaymentDate.EditValue = DateTime.Today;
            }
            LoadOperationsDataGridView();
        }

        private void ComponentEnabled(bool status)
        {
            if (status)
            {
                StandaloneBarDockControl.Enabled = false;
                PopupMenu.Manager = null;
            }
            else
            {
                StandaloneBarDockControl.Enabled = true;
                PopupMenu.Manager = BarManager;
            }
            BOK.Visible = !status;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OperationsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void OperationsGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("INCOME", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("EXPENSES", "Far", e);
            if (e.Column.FieldName == "DEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void OperationsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(OperationsGridView, PopupMenu, e);
        }

        private void LoadOperationsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 BO.ID,
                                 (CASE
                                     WHEN BO.CONTRACT_CODE IS NOT NULL
                                     THEN
                                        BA.NAME || ' - ' || BO.CONTRACT_CODE
                                     ELSE
                                        BA.NAME
                                  END) APPOINTMENT,
                                 BO.INCOME,
                                 BO.EXPENSES,
                                 BO.DEBT,
                                 BA.OPERATION_TYPE_ID,
                                 BA.ID APPOINTMENT_ID
                            FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO, CRS_USER.BANK_APPOINTMENTS BA
                           WHERE     BO.IS_CHANGE IN (0, 1)
                                 AND BO.APPOINTMENT_ID = BA.ID
                                 AND BO.BANK_ID = {bank_id}                                 
                                 AND OPERATION_DATE = TO_DATE('{PaymentDate.Text}', 'DD/MM/YYYY')
                        ORDER BY ID";
            OperationsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadOperationsDataGridView");
            if (OperationsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {                    
                    //DeleteBarButton.Enabled = GlobalVariables.DeleteBank;
                }
                else
                    EditBarButton.Enabled = DeleteBarButton.Enabled = true;
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }

        void RefreshOperatios()
        {
            LoadOperationsDataGridView();
        }

        private void LoadFBankOperationAddEdit(string transaction, string operationid, string date, string bankname, int operationtype, bool closewindow, string bankid)
        {
            topindex = OperationsGridView.TopRowIndex;
            old_row_num = OperationsGridView.FocusedRowHandle;
            FBankOperationAddEdit fcae = new FBankOperationAddEdit();
            fcae.TransactionName = transaction;
            fcae.Date = date;
            fcae.OperationID = operationid;
            fcae.BankName = bankname;
            fcae.OperationType = operationtype;
            fcae.CloseWindow = closewindow;
            fcae.BankID = bankid;
            fcae.RefreshOperationsDataGridView += new FBankOperationAddEdit.DoEvent(RefreshOperatios);
            fcae.ShowDialog();
            OperationsGridView.TopRowIndex = topindex;
            OperationsGridView.FocusedRowHandle = old_row_num;
        }

        private void IncomeBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ControlOperationDetails())
                LoadFBankOperationAddEdit("INSERT", null, PaymentDate.Text, BankLookUp.Text, 1, CloseWindowCheck.Checked, bank_id.ToString());
        }

        private void ExpensesBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ControlOperationDetails())
                LoadFBankOperationAddEdit("INSERT", null, PaymentDate.Text, BankLookUp.Text, 2, CloseWindowCheck.Checked, bank_id.ToString());
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadOperationsDataGridView();
        }

        private void FBankOperation_FormClosing(object sender, FormClosingEventArgs e)
        {
            BankOperationTempDelete();
            this.RefreshBankDataGridView(PaymentDate.Text);
        }

        private void BankOperationTempDelete()
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = "CRS_USER_TEMP.PROC_BOP_TEMP_DELETE";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_BANK_ID", "INTEGER").Value = bank_id;
                    command.Parameters.Add("P_DATE", "VARCHAR2").Value = PaymentDate.Text;
                    command.Parameters.Add("P_USED_USER_ID", "INTEGER").Value = GlobalVariables.V_UserID;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Bank əməliyyatları temp cədvəldən silinmədi.", "CRS_USER_TEMP.PROC_BOP_TEMP_DELETE", GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(OperationsGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(OperationsGridControl, "xls");
        }

        private void OperationsGridView_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                int type = int.Parse(View.GetRowCellDisplayText(e.RowHandle, View.Columns["OPERATION_TYPE_ID"]));
                GlobalProcedures.GridRowOperationTypeColor(type, e);
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFBankOperationAddEdit("EDIT", OperationID, PaymentDate.Text, BankLookUp.Text, operation_type_id, CloseWindowCheck.Checked, bank_id.ToString());
        }

        private void OperationsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = OperationsGridView.GetFocusedDataRow();
            if (row != null)
            {
                OperationID = row["ID"].ToString();
                operation_type_id = Convert.ToInt32(row["OPERATION_TYPE_ID"].ToString());
                appointment_id = Convert.ToInt32(row["APPOINTMENT_ID"].ToString());
                EditBarButton.Enabled = (!(appointment_id == 19 || appointment_id == 8 || appointment_id == 5 || appointment_id == 18 || appointment_id == 21 || appointment_id == 27 || appointment_id == 15) && GlobalVariables.EditBank);
                DeleteBarButton.Enabled = (!(appointment_id == 8 || appointment_id == 19) && GlobalVariables.DeleteBank);
            }
        }

        private void OperationsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled && StandaloneBarDockControl.Enabled)
                LoadFBankOperationAddEdit("EDIT", OperationID, PaymentDate.Text, BankLookUp.Text, operation_type_id, CloseWindowCheck.Checked, bank_id.ToString());
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(BankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
        }

        private void BankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void BankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BankLookUp.EditValue == null)
                return;

            bank_id = Convert.ToInt32(BankLookUp.EditValue);
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (appointment_id != 3)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş bank əməliyyatını silmək istəyirsiniz?", "Əməliyyatın silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.BANK_OPERATIONS_TEMP SET IS_CHANGE = 2 WHERE ID = " + OperationID, "Əməliyyat silinmədi", this.Name + "/DeleteBarButton_ItemClick");
                }
            }
            else
                XtraMessageBox.Show("Lizinq ödənişini yalnız müştərinin ödənişləri cədvəlindən silmək olar.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            LoadOperationsDataGridView();
        }

        private void InsertOperationsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_INSERT_BO_TEMP", "P_BANK_ID", int.Parse(BankID), "P_DATE", GlobalFunctions.ConvertDateToInteger(Date, "ddmmyyyy"), "Ödənişlər temp cədvələ daxil edilmədi.");
        }

        private void InsertOperations()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.BANK_OPERATIONS WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND BANK_ID = {bank_id} AND OPERATION_DATE = TO_DATE('{PaymentDate.Text.Trim()}','DD/MM/YYYY') AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                             string.Format("INSERT INTO CRS_USER.BANK_OPERATIONS(ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,CONTRACT_PAYMENT_ID,CONTRACT_CODE,FUNDS_PAYMENT_ID,FUNDS_CONTRACT_ID)SELECT ID,BANK_ID,OPERATION_DATE,APPOINTMENT_ID,INCOME,EXPENSES,DEBT,NOTE,CONTRACT_PAYMENT_ID,CONTRACT_CODE,FUNDS_PAYMENT_ID,FUNDS_CONTRACT_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE = 1 AND BANK_ID = {0} AND USED_USER_ID = {1} AND OPERATION_DATE = TO_DATE('{2}','DD/MM/YYYY')", bank_id, GlobalVariables.V_UserID, PaymentDate.Text.Trim()),
                                                    "Bankın əməliyyatları əsas cədvələ daxil olmadı.",
                                                    this.Name + "/InsertOperations");
        }

        private void InsertOperationJournal()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.OPERATION_JOURNAL WHERE (ACCOUNT_OPERATION_TYPE_ID,CONTRACT_ID,CUSTOMER_PAYMENT_ID) IN (SELECT ACCOUNT_OPERATION_TYPE_ID,CONTRACT_ID,CUSTOMER_PAYMENT_ID FROM CRS_USER_TEMP.OPERATION_JOURNAL_TEMP WHERE ACCOUNT_OPERATION_TYPE_ID = 1 AND IS_CHANGE <> 0 AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                             $@"INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,CREATED_USER_ID,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY)SELECT ID,OPERATION_DATE,CREATED_USER_ID,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY FROM CRS_USER_TEMP.OPERATION_JOURNAL_TEMP WHERE IS_CHANGE = 1 AND ACCOUNT_OPERATION_TYPE_ID = 1 AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                             "Əməliyyatlar jurnalın əsas cədvəlinə daxil edilmədi.",
                                             this.Name + "/InsertOperationJournal");
        }

        private void InsertCashBankAccount()
        {
            string s = $@"SELECT TO_CHAR (BO.OPERATION_DATE, 'DD/MM/YYYY') OPERATION_DATE,
                                   BO.BANK_ID,
                                   BA.NAME,
                                   BO.EXPENSES AMOUNT,
                                   1 INC_EXP,
                                   (CASE
                                       WHEN BO.NOTE IS NULL
                                       THEN
                                          'Bank əməliyyatlarının məxaricindən daxil olub'
                                       ELSE
                                             BO.NOTE
                                          || ' - '
                                          || 'Bank əməliyyatlarının məxaricindən daxil olub'
                                    END)
                                      NOTE,
                                   BA.ID APPOINTMENT_ID,
                                   BO.INCOME,
                                   BO.IS_CHANGE,
                                   BO.ID BANK_OPERATION_ID,
                                   BA.NAME APPOINTMENT
                              FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP BO, CRS_USER.BANK_APPOINTMENTS BA
                             WHERE     BO.APPOINTMENT_ID IN (4, 7)
                                   AND BO.APPOINTMENT_ID = BA.ID
                                   AND BO.IS_CHANGE != 0
                                   AND BO.BANK_ID = {bank_id}
                                   AND BO.USED_USER_ID = {GlobalVariables.V_UserID}
                                   AND BO.OPERATION_DATE = TO_DATE ('{PaymentDate.Text}', 'DD/MM/YYYY')";

            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/InsertCashBankAccount");
                if (dt.Rows.Count == 0)
                    return;

                foreach (DataRow dr in dt.Rows)
                {
                    int AccountID = 0,
                        ischange = Convert.ToInt32(dr["IS_CHANGE"].ToString());
                    double expenses = Convert.ToDouble(dr["AMOUNT"].ToString()),
                        income = Convert.ToDouble(dr["INCOME"].ToString());

                    if (Convert.ToInt32(dr["APPOINTMENT_ID"].ToString()) == 7)
                    {
                        GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE DESTINATION_ID = 4 AND OPERATION_OWNER_ID IN (SELECT ID FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP IN 1 AND BANK_ID = {dr["BANK_ID"]} AND BANK_OPERATION_ID = {dr["BANK_OPERATION_ID"]} AND ADATE = TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'))",
                                                        $@"DELETE FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 1 AND BANK_ID = {dr["BANK_ID"]} AND ADATE = TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY') AND BANK_OPERATION_ID = {dr["BANK_OPERATION_ID"]}",
                                                            "Bank hesabının mədaxili kassadan silinmədi.",
                                                        this.Name + "/InsertCashBankAccount");

                        if (ischange == 1)
                        {
                            AccountID = GlobalFunctions.InsertQuery($@"INSERT INTO CRS_USER.CASH_BANK_ACCOUNT(ADATE,BANK_ID,APPOINTMENT,AMOUNT,NOTE,INC_EXP,BANK_OPERATION_ID)VALUES(TO_DATE('{dr["OPERATION_DATE"]} ','DD/MM/YYYY'),{dr["BANK_ID"]},'{dr["APPOINTMENT"]}',{expenses.ToString(GlobalVariables.V_CultureInfoEN)},'{dr["NOTE"]}',1,{dr["BANK_OPERATION_ID"]}) RETURNING ID INTO :ID",
                                                                        "Kassanın möhkəmləndirilməsi üçün olan məbləğ kassanın bank hesabına daxil olmadı.",
                                                                        "ID",
                                                                        this.Name + "/InsertCashBankAccount");
                            GlobalProcedures.InsertCashOperation(4, AccountID, PaymentDate.Text, null, expenses, 0, 1);
                        }
                    }
                    else
                    {
                        GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE DESTINATION_ID = 9 AND OPERATION_OWNER_ID IN (SELECT ID FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP IN 2 AND BANK_ID = {dr["BANK_ID"]} AND BANK_OPERATION_ID = {dr["BANK_OPERATION_ID"]} AND ADATE = TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'))",
                                                      $@"DELETE FROM CRS_USER.CASH_BANK_ACCOUNT WHERE INC_EXP = 2 AND BANK_ID = {dr["BANK_ID"]} AND ADATE = TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY') AND BANK_OPERATION_ID = {dr["BANK_OPERATION_ID"]}",
                                           "Bank hesabının mədaxili kassadan silinmədi.",
                                           this.Name + "/InsertCashBankAccount");

                        if (ischange == 1)
                        {
                            AccountID = GlobalFunctions.InsertQuery($@"INSERT INTO CRS_USER.CASH_BANK_ACCOUNT(ADATE,BANK_ID,APPOINTMENT,AMOUNT,NOTE,INC_EXP,BANK_OPERATION_ID)VALUES(TO_DATE('{dr["OPERATION_DATE"]}','DD/MM/YYYY'),{dr["BANK_ID"]},'{dr["APPOINTMENT"]}',{income.ToString(GlobalVariables.V_CultureInfoEN)},'{dr["NOTE"]}',2,{dr["BANK_OPERATION_ID"]}) RETURNING ID INTO :ID",
                                                            "Kassanın möhkəmləndirilməsi üçün olan məbləğ kassanın bank hesabına daxil olmadı.",
                                                            "ID",
                                                            this.Name + "/InsertCashBankAccount");
                            GlobalProcedures.InsertCashOperation(9, AccountID, PaymentDate.Text, null, 0, income, 1);
                        }
                    }
                }
                GlobalProcedures.UpdateCashDebt(PaymentDate.Text);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Kassanın möhkəmləndirilməsi üçün bank əməliyyatlarının rekvizitləri tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void InsertContractPayments()
        {
            int a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_PAYMENT_ID <> 0 AND APPOINTMENT_ID = 3 AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            if (a > 0)
            {
                GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CUSTOMER_PAYMENTS WHERE ID IN (SELECT CONTRACT_PAYMENT_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_PAYMENT_ID <> 0 AND APPOINTMENT_ID = 3 AND USED_USER_ID = " + Class.GlobalVariables.V_UserID + ")",
                                                 $@"INSERT INTO CRS_USER.CUSTOMER_PAYMENTS(ID,CUSTOMER_ID,CONTRACT_ID,PAYMENT_DATE,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,REQUIRED_AMOUNT,PAYMENT_NAME,NOTE,BANK_CASH,PENALTY_DEBT,IS_PENALTY,PENALTY_AMOUNT,BANK_ID)SELECT ID,CUSTOMER_ID,CONTRACT_ID,PAYMENT_DATE,PAYMENT_AMOUNT,BASIC_AMOUNT,DEBT,DAY_COUNT,ONE_DAY_INTEREST_AMOUNT,INTEREST_AMOUNT,PAYMENT_INTEREST_AMOUNT,PAYMENT_INTEREST_DEBT,TOTAL,CURRENCY_RATE,PAYMENT_AMOUNT_AZN,REQUIRED_CLOSE_AMOUNT,REQUIRED_AMOUNT,PAYMENT_NAME,NOTE,BANK_CASH,PENALTY_DEBT,IS_PENALTY,PENALTY_AMOUNT,BANK_ID FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE = 1  AND ID IN (SELECT CONTRACT_PAYMENT_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_PAYMENT_ID <> 0 AND APPOINTMENT_ID = 3 AND USED_USER_ID = " + GlobalVariables.V_UserID + ") AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                           "Ödənişlər əsas cədvələ daxil edilmədi.",
                                                 this.Name + "/InsertContractPayments");

                GlobalProcedures.ExecuteThreeQuery($@"DELETE FROM CRS_USER.CONTRACT_BALANCE_PENALTIES
                                                                  WHERE ID IN
                                                                           (SELECT ID
                                                                              FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP
                                                                             WHERE     IS_CHANGE <> 0
                                                                                   AND CONTRACT_ID IN
                                                                                          (SELECT CONTRACT_ID
                                                                                             FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                                                                                            WHERE     IS_CHANGE = 1
                                                                                                  AND ID IN
                                                                                                         (SELECT CONTRACT_PAYMENT_ID
                                                                                                            FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP
                                                                                                           WHERE     IS_CHANGE <> 0
                                                                                                                 AND CONTRACT_PAYMENT_ID <>
                                                                                                                        0
                                                                                                                 AND APPOINTMENT_ID = 3
                                                                                                                 AND USED_USER_ID = {GlobalVariables.V_UserID})
                                                                                                  AND USED_USER_ID = {GlobalVariables.V_UserID}))",
                                                    $@"INSERT INTO CRS_USER.CONTRACT_BALANCE_PENALTIES (ID,
                                                                                                         CUSTOMER_ID,
                                                                                                         CONTRACT_ID,
                                                                                                         BAL_DATE,
                                                                                                         PENALTY_AMOUNT,
                                                                                                         DISCOUNT_PENALTY,
                                                                                                         DEBT_PENALTY,
                                                                                                         PAYMENT_PENALTY,
                                                                                                         IS_COMMIT,
                                                                                                         PENALTY_STATUS,
                                                                                                         CUSTOMER_PAYMENT_ID)
                                                           SELECT ID,
                                                                  CUSTOMER_ID,
                                                                  CONTRACT_ID,
                                                                  BAL_DATE,
                                                                  PENALTY_AMOUNT,
                                                                  DISCOUNT_PENALTY,
                                                                  DEBT_PENALTY,
                                                                  PAYMENT_PENALTY,
                                                                  IS_COMMIT,
                                                                  PENALTY_STATUS,
                                                                  CUSTOMER_PAYMENT_ID
                                                             FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP
                                                            WHERE     IS_CHANGE = 1
                                                                  AND CONTRACT_ID IN
                                                                         (SELECT CONTRACT_ID
                                                                            FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                                                                           WHERE     IS_CHANGE = 1
                                                                                 AND ID IN
                                                                                        (SELECT CONTRACT_PAYMENT_ID
                                                                                           FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP
                                                                                          WHERE     IS_CHANGE <> 0
                                                                                                AND CONTRACT_PAYMENT_ID <> 0
                                                                                                AND APPOINTMENT_ID = 3
                                                                                                AND USED_USER_ID ={GlobalVariables.V_UserID})
                                                                                 AND USED_USER_ID = {GlobalVariables.V_UserID})
                                                                  AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    $@"UPDATE CRS_USER.CONTRACT_CALCULATED_PENALTIES
                                                                   SET IS_BALANCE = 1, IS_COMMIT = 1
                                                                 WHERE     IS_BALANCE = 1
                                                                       AND IS_COMMIT = 0
                                                                       AND CONTRACT_ID IN
                                                                              (SELECT CONTRACT_ID
                                                                                 FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                                                                                WHERE     IS_CHANGE = 1
                                                                                      AND ID IN
                                                                                             (SELECT CONTRACT_PAYMENT_ID
                                                                                                FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP
                                                                                               WHERE     IS_CHANGE <> 0
                                                                                                     AND CONTRACT_PAYMENT_ID <> 0
                                                                                                     AND APPOINTMENT_ID = 3
                                                                                                     AND USED_USER_ID = {GlobalVariables.V_UserID})
                                                                                      AND USED_USER_ID ={GlobalVariables.V_UserID})
                                                                       AND EXISTS
                                                                              (SELECT ID
                                                                                 FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP
                                                                                WHERE     IS_CHANGE = 1
                                                                                      AND CONTRACT_ID IN
                                                                                             (SELECT CONTRACT_ID
                                                                                                FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                                                                                               WHERE     IS_CHANGE = 1
                                                                                                     AND ID IN
                                                                                                            (SELECT CONTRACT_PAYMENT_ID
                                                                                                               FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP
                                                                                                              WHERE     IS_CHANGE <> 0
                                                                                                                    AND CONTRACT_PAYMENT_ID <>
                                                                                                                           0
                                                                                                                    AND APPOINTMENT_ID = 3
                                                                                                                    AND USED_USER_ID = {GlobalVariables.V_UserID})
                                                                                                     AND USED_USER_ID ={GlobalVariables.V_UserID})
                                                                                      AND USED_USER_ID = {GlobalVariables.V_UserID})",
                                                    "Cərimələr əsas cədvələ daxil edilmədi.");
                CalculatedLeasingTotal();
            }
        }

        private void CalculatedLeasingTotal()
        {
            string s = $@"SELECT CONTRACT_ID FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP WHERE IS_CHANGE = 1  AND ID IN (SELECT CONTRACT_PAYMENT_ID FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP WHERE IS_CHANGE <> 0 AND CONTRACT_PAYMENT_ID <> 0 AND APPOINTMENT_ID = 3 AND USED_USER_ID = {GlobalVariables.V_UserID}) AND USED_USER_ID = {GlobalVariables.V_UserID}";
            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, this.Name + "/CalculatedLeasingTotal").Rows)
            {
                GlobalProcedures.CalculatedLeasingTotal(dr[0]);
            }

        }

        private bool ControlOperationDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(PaymentDate.Text))
            {
                PaymentDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                PaymentDate.Focus();
                PaymentDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BankLookUp.EditValue == null)
            {
                BankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bank daxil edilməyib.");
                BankLookUp.Focus();
                BankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertFundsPayments()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_FUNDS_PAYMENT", "P_BANK_ID", bank_id, "Cəlb olunmuş vəsaitlərin ödənişləri əsas cədvələ daxil edilmədi.");
            CalculatedFundsPaymentTotal();
        }

        private void CalculatedFundsPaymentTotal()
        {
            string s = $@"SELECT FUNDS_CONTRACT_ID
                                  FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP
                                 WHERE     IS_CHANGE <> 0
                                       AND APPOINTMENT_ID IN (SELECT ID
                                                                FROM CRS_USER.BANK_APPOINTMENTS
                                                               WHERE APPOINTMENT_TYPE_ID = 4)
                                       AND USED_USER_ID = {GlobalVariables.V_UserID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/CalculatedFundsPaymentTotal");
            int contractID = 0;
            if (dt.Rows.Count > 0)
            {
                contractID = Convert.ToInt32(dt.Rows[0]["FUNDS_CONTRACT_ID"]);
                GlobalProcedures.UpdateFundChange(PaymentDate.Text, contractID, 0);
                GlobalProcedures.CalculatedAttractedFundsTotal(contractID);
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlOperationDetails())
            {
                ProgressPanel.Show();
                Application.DoEvents();
                InsertOperations();
                InsertOperationJournal();
                InsertCashBankAccount();
                InsertContractPayments();
                InsertFundsPayments();
                GlobalProcedures.UpdateBankOperationDebtWithBank(PaymentDate.Text, bank_id);
                GlobalProcedures.UpdateBankOperationDebt(PaymentDate.Text);
                this.Close();
            }
        }

        private void OperationsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (OperationsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                {
                    EditBarButton.Enabled = GlobalVariables.EditBank;
                    DeleteBarButton.Enabled = GlobalVariables.DeleteBank;
                }
                else
                {
                    EditBarButton.Enabled = !(appointment_id == 19 || appointment_id == 8 || appointment_id == 5 || appointment_id == 18 || appointment_id == 21 || appointment_id == 27 || appointment_id == 15);
                    DeleteBarButton.Enabled = !(appointment_id == 19 || appointment_id == 8);
                }
            }
            else
                EditBarButton.Enabled = DeleteBarButton.Enabled = false;
        }
    }
}