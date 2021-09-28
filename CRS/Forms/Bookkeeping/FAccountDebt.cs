using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using CRS.Class;

namespace CRS.Forms.Bookkeeping
{
    public partial class FAccountDebt : DevExpress.XtraEditors.XtraForm
    {
        public FAccountDebt()
        {
            InitializeComponent();
        }
        string DebtID, debtdate, account;
        int ismanual;

        public delegate void DoEvent();
        public event DoEvent RefreshAccountList;

        private void FAccountDebt_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                NewBarButton.Enabled = GlobalVariables.AddDebt;
                EditBarButton.Enabled = GlobalVariables.EditDebt;
                DeleteBarButton.Enabled = GlobalVariables.DeleteDebt;
            }

            if (GlobalVariables.V_UserID == 0)
                barButtonItem1.Visibility =
                    barButtonItem2.Visibility =
                    barButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            else
                barButtonItem1.Visibility =
                    barButtonItem2.Visibility =
                    barButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;                        
            
            LoadDebtsDataGridView();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadDebtsDataGridView()
        {
            string s = @"SELECT ID,
                                     DEBT_DATE,
                                     ACCOUNT,
                                     DEBIT,
                                     CREDIT,
                                     USED_USER_ID,
                                     IS_MANUAL
                                FROM CRS_USER.OPERATION_DEBT
                              WHERE IS_MANUAL = 0
                            ORDER BY DEBT_DATE, ACCOUNT,ID";

            try
            {                
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadDebtsDataGridView");

                DebtsGridControl.DataSource = dt;

                if (DebtsGridView.RowCount > 0)
                {
                    if (ismanual == 0)
                    {
                        if (GlobalVariables.V_UserID > 0)
                        {
                            EditBarButton.Enabled = GlobalVariables.EditDebt;
                            DeleteBarButton.Enabled = GlobalVariables.DeleteDebt;
                        }
                        else
                            EditBarButton.Enabled = DeleteBarButton.Enabled = true;
                    }
                    else
                        EditBarButton.Enabled = DeleteBarButton.Enabled = false;

                }
                else
                    DeleteBarButton.Enabled = EditBarButton.Enabled = false;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Hesabların qalıqları cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void StandaloneBarDockControl_Click(object sender, EventArgs e)
        {
            LoadDebtsDataGridView();
        }

        void RefreshDebt()
        {
            LoadDebtsDataGridView();
        }

        private void LoadFAccountDebtAddEdit(string transaction, string debtid)
        {
            FAccountDebtAddEdit fadae = new FAccountDebtAddEdit();
            fadae.TransactionName = transaction;
            fadae.DebtID = debtid;
            fadae.RefreshDebtsDataGridView += new FAccountDebtAddEdit.DoEvent(RefreshDebt);
            fadae.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFAccountDebtAddEdit("INSERT", null);
        }

        private void DebtsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private bool ControlDebt()
        {
            bool b = false;
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.OPERATION_DEBT WHERE ACCOUNT = '{account}' AND DEBT_DATE < TO_DATE('{debtdate}','DD/MM/YYYY')");

            if (a > 0)
            {
                XtraMessageBox.Show(account + " hesabının " + debtdate + " tarixindən kiçik tarixlər üçün qalıqları olduğundan bu hesabın seçdiyiniz qalığını dəyişmək və ya silmək olmaz.");
                return false;
            }
            else
                b = true;
            return b;
        }

        private void DebtsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = DebtsGridView.GetFocusedDataRow();
            if (row != null)
            {
                DebtID = row["ID"].ToString();
                debtdate = Convert.ToDateTime(row["DEBT_DATE"]).ToString("d");
                account = row["ACCOUNT"].ToString();
                ismanual = Convert.ToInt32(row["IS_MANUAL"].ToString());
                EditBarButton.Enabled = DeleteBarButton.Enabled = (ismanual == 0);
                CalcDebtAgainBarButton.Enabled = (ismanual == 1);
            }
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ControlDebt())
                LoadFAccountDebtAddEdit("EDIT", DebtID);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(DebtsGridControl);
        }

        private void DebtsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled && ControlDebt())
                LoadFAccountDebtAddEdit("EDIT", DebtID);
        }

        private void DebtsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(DebtsGridView, PopupMenu, e);
        }

        private void ExcellBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DebtsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DebtsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DebtsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DebtsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DebtsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DebtsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(DebtsGridControl, "mht");
        }

        private void DeleteDebt()
        {
            int journalcount = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT ID FROM CRS_USER.OPERATION_JOURNAL WHERE DEBIT_ACCOUNT = '" + account + "' AND OPERATION_DATE >= TO_DATE('" + debtdate + "','DD/MM/YYYY') UNION SELECT ID FROM CRS_USER.OPERATION_JOURNAL WHERE CREDIT_ACCOUNT = '" + account + "' AND OPERATION_DATE >= TO_DATE('" + debtdate + "','DD/MM/YYYY'))"),
                debtcount = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.OPERATION_DEBT WHERE ACCOUNT = '{account}' AND DEBT_DATE > TO_DATE('{debtdate}','DD/MM/YYYY')");
            string message = "Seçilmiş hesabın qalığıı silmək istəyirsiniz?";

            if (journalcount == 0 && debtcount > 0)
            {
                message = account + "hesabını silsəz bu hesabının jurnalda " + debtdate + " tarixindən sonraya heç bir əməliyyatı yoxdur. Buna görə də bu hesabın " + debtdate + " tarixindən sonrakı bütün qalıqları silinəcək.Buna razısınız?";
                DialogResult dialogResult = XtraMessageBox.Show(message, "Hesabın qalığının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.OPERATION_DEBT WHERE ID = {DebtID}", "Hesabın qalığı silinmədi.", this.Name + "/DeleteDebt");
                    //Class.GlobalProcedures.CalculatedOperationDebt(account, debtdate);
                }
            }
            else if (journalcount >= 0 && debtcount >= 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show(message, "Hesabın qalığının silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.OPERATION_DEBT WHERE ID = {DebtID}", "Hesabın qalığı silinmədi.", this.Name + "/DeleteDebt");
                   // Class.GlobalProcedures.CalculatedOperationDebt(account, debtdate);
                }
            }
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ControlDebt())
            {
                int UsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.OPERATION_DEBT WHERE ID = " + DebtID);
                if (UsedUserID >= 0)
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş hesabın qalığı hal-hazırda " + used_user_name + " tərəfindən istifadə olunduğu üçün onu silmək olmaz.", "Seçilmiş hesabın qalığının hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        DeleteDebt();
                }
                else
                    DeleteDebt();
                LoadDebtsDataGridView();
            }
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadDebtsDataGridView();
        }

        private void FAccountDebt_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshAccountList();
        }

        //private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    DebtProgressPanel.Show();
        //    Application.DoEvents();
        //    string s, s2, s3, bank_sub_account, OperationID;
        //    try
        //    {
        //        s = "SELECT C.ID, LIQUID_AMOUNT ,TO_CHAR(C.START_DATE,'DD/MM/YYYY'),H.FIRST_PAYMENT " +
        //                "FROM CRS_USER.CONTRACTS C,(SELECT CONTRACT_ID,LIQUID_AMOUNT, FIRST_PAYMENT FROM CRS_USER.HOSTAGE_CAR " +
        //                                            "UNION ALL " +
        //                                            "SELECT CONTRACT_ID,LIQUID_AMOUNT, FIRST_PAYMENT FROM CRS_USER.HOSTAGE_OBJECT) H " +
        //                "WHERE C.ID = H.CONTRACT_ID AND C.START_DATE BETWEEN TO_DATE('1/1/2016','MM/DD/YYYY') AND TO_DATE('12/31/2016','MM/DD/YYYY')";
        //        DataTable dt = Class.GlobalFunctions.GenerateDataTable(s);

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            Class.GlobalProcedures.InsertOperationJournalForSeller(dr[2].ToString(), Convert.ToDouble(dr[1].ToString()), Convert.ToDouble(dr[3].ToString()), dr[0].ToString(), null, null);
        //        }

        //    }
        //    catch (Exception exx)
        //    {
        //        XtraMessageBox.Show("Satışın məlumatları jurnala daxil olmadı" + "\nError - " + exx.Message);
        //    }

        //    try
        //    {
        //        s2 = "SELECT TO_CHAR(PAYMENT_DATE,'DD/MM/YYYY'),CURRENCY_RATE,DECODE(CURRENCY_RATE,1,1,2) CURRENCY_ID,PAYMENT_AMOUNT,BASIC_AMOUNT,PAYMENT_INTEREST_AMOUNT,ID PAYMENT_ID,CONTRACT_ID,BANK_CASH,PAYMENT_AMOUNT_AZN FROM CRS_USER.CUSTOMER_PAYMENTS WHERE PAYMENT_DATE >= TO_DATE('1/1/2016','MM/DD/YYYY') AND (CONTRACT_ID,CUSTOMER_ID) IN (SELECT ID,CUSTOMER_ID FROM CRS_USER.CONTRACTS WHERE 174||LEASING_ACCOUNT IN (SELECT ACCOUNT FROM CRS_USER.OPERATION_DEBT)) ORDER BY PAYMENT_DATE";
        //        DataTable dt = Class.GlobalFunctions.GenerateDataTable(s2);

        //        foreach (DataRow dr2 in dt.Rows)
        //        {
        //            if (dr2[7].ToString() == "C")
        //                InsertOJournal(dr2[0].ToString(), Convert.ToDouble(dr2[1].ToString()), Convert.ToInt32(dr2[2].ToString()), Convert.ToDouble(dr2[3].ToString()), Convert.ToDouble(dr2[9].ToString()), Convert.ToDouble(dr2[4].ToString()), Convert.ToDouble(dr2[5].ToString()), dr2[7].ToString(), dr2[6].ToString(), null);
        //            else
        //            {
        //                bank_sub_account = Class.GlobalFunctions.GetName("SELECT SUB_ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN WHERE (BANK_ID,BRANCH_ID) IN (SELECT BANK_ID,BRANCH_ID FROM CRS_USER.BANK_OPERATIONS WHERE CUSTOMER_PAYMENT_ID = " + dr2[6] + ") AND ROWNUM = 1");
        //                InsertOJournal(dr2[0].ToString(), Convert.ToDouble(dr2[1].ToString()), Convert.ToInt32(dr2[2].ToString()), Convert.ToDouble(dr2[3].ToString()), Convert.ToDouble(dr2[9].ToString()), Convert.ToDouble(dr2[4].ToString()), Convert.ToDouble(dr2[5].ToString()), dr2[7].ToString(), dr2[6].ToString(), bank_sub_account);
        //            }
        //        }
        //    }
        //    catch (Exception exx)
        //    {
        //        XtraMessageBox.Show("Ödənişlər jurnala daxil olmadı" + "\nError - " + exx.Message);
        //    }

        //    try
        //    {
        //        s3 = "SELECT TO_CHAR(OPERATION_DATE,'DD/MM/YYYY'),CREATED_USER_ID,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,ACCOUNT_OPERATION_TYPE_ID,IS_MANUAL FROM CRS_USER.OPERATION_JOURNAL_COPY";
        //        DataTable dt = Class.GlobalFunctions.GenerateDataTable(s3);

        //        foreach (DataRow dr3 in dt.Rows)
        //        {
        //            OperationID = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE").ToString();
        //            Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,CREATED_USER_ID,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,ACCOUNT_OPERATION_TYPE_ID,IS_MANUAL)VALUES(" + OperationID + ",TO_DATE('" + dr3[0] + "','DD/MM/YYYY')," + dr3[1] + ",'" + dr3[2] + "','" + dr3[3] + "'," + dr3[4] + "," + dr3[5] + "," + Convert.ToDouble(dr3[6].ToString()).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + dr3[7] + "'," + dr3[8] + ",1)",
        //                             dr3[2] + " hesabının debitinə və " + dr3[3] + " hesabının kreditinə məbləğ daxil edilmədi.");
        //            //Class.GlobalProcedures.CalculatedOperationDebt(dr3[2].ToString(), dr3[0].ToString());
        //            //Class.GlobalProcedures.CalculatedOperationDebt(dr3[3].ToString(), dr3[0].ToString());
        //        }

        //    }
        //    catch (Exception exx)
        //    {
        //        XtraMessageBox.Show("Digər ödənişlər jurnala daxil olmadı" + "\nError - " + exx.Message);
        //    }
        //    DebtProgressPanel.Hide();
        //}

        //silinecek
        //private void InsertOJournal(string date, double currency_rate, int currency_id, double amount, double amount_azn, double basic_amount, double interest_amount, string contractid, string paymentid, string owner_account)
        //{
        //    int operationid;
        //    string appointment, debit_account, credit_account, customer_account, leasing_account, leasing_interest_account, account, account612, account712, account631;
        //    double amount_cur = 0;
        //    try
        //    {
        //        customer_account = Class.GlobalFunctions.GetName("SELECT CUSTOMER_ACCOUNT FROM CRS_USER.CONTRACTS WHERE ID = " + contractid);
        //        leasing_account = Class.GlobalFunctions.GetName("SELECT LEASING_ACCOUNT FROM CRS_USER.CONTRACTS WHERE ID = " + contractid);
        //        leasing_interest_account = Class.GlobalFunctions.GetName("SELECT LEASING_INTEREST_ACCOUNT FROM CRS_USER.CONTRACTS WHERE ID = " + contractid);
        //        if (String.IsNullOrEmpty(owner_account))
        //            account = Class.GlobalFunctions.GetName("SELECT NVL(SUB_ACCOUNT,ACCOUNT_NUMBER) ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = " + 221);
        //        else
        //            account = owner_account;
        //        account612 = Class.GlobalFunctions.GetName("SELECT NVL(SUB_ACCOUNT,ACCOUNT_NUMBER) ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = " + 612);
        //        account712 = Class.GlobalFunctions.GetName("SELECT NVL(SUB_ACCOUNT,ACCOUNT_NUMBER) ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = " + 712);
        //        account631 = Class.GlobalFunctions.GetName("SELECT NVL(SUB_ACCOUNT,ACCOUNT_NUMBER) ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN WHERE ACCOUNT_NUMBER = " + 631);
        //        Class.GlobalProcedures.ExecuteQuery("DELETE FROM CRS_USER.OPERATION_JOURNAL WHERE OPERATION_DATE = TO_DATE('" + date + "','DD/MM/YYYY') AND CONTRACT_ID = " + contractid + " AND CUSTOMER_PAYMENT_ID = " + paymentid,
        //                      "Əməliyyatlar jurnaldan silinmədi.");

        //        //221 - 543
        //        operationid = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE");
        //        appointment = Class.GlobalFunctions.GetName("SELECT CT.CODE||C.CODE FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND C.ID = " + contractid) + " saylı müqavilə üzrə lizinq ödənişi";
        //        debit_account = account;
        //        credit_account = 543 + customer_account;
        //        //amount_azn = Math.Round(amount * currency_rate, 2);
        //        if (currency_id != 1)
        //            amount_cur = Math.Round(amount, 2);
        //        Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID)VALUES(" + operationid + ",TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1)",
        //                     "221 hesabının debitinə və 543 hesabının kreditinə lizinq ödənişi daxil edilmədi.");
        //        //Class.GlobalProcedures.CalculatedOperationDebt(debit_account, date);
        //        //Class.GlobalProcedures.CalculatedOperationDebt(credit_account, date);

        //        //216 - 631
        //        operationid = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE");
        //        appointment = Class.GlobalFunctions.GetName("SELECT CT.CODE||C.CODE FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND C.ID = " + contractid) + " saylı müqavilə üzrə hesablanmış faiz";
        //        debit_account = 216 + leasing_interest_account;
        //        credit_account = account631;
        //        amount_azn = Math.Round(interest_amount * currency_rate, 2);
        //        if (currency_id != 1)
        //            amount_cur = Math.Round(interest_amount, 2);
        //        Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID)VALUES(" + operationid + ",TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1)",
        //                     "216 hesabının debitinə və 631 hesabının kreditinə hesablanmış faiz daxil edilmədi.");
        //        //Class.GlobalProcedures.CalculatedOperationDebt(debit_account, date);
        //        //Class.GlobalProcedures.CalculatedOperationDebt(credit_account, date);

        //        //543 - 216
        //        operationid = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE");
        //        appointment = Class.GlobalFunctions.GetName("SELECT CT.CODE||C.CODE FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND C.ID = " + contractid) + " saylı müqavilə üzrə silinən faiz";
        //        debit_account = 543 + customer_account;
        //        credit_account = 216 + leasing_interest_account;
        //        amount_azn = Math.Round(interest_amount * currency_rate, 2);
        //        if (currency_id != 1)
        //            amount_cur = Math.Round(interest_amount, 2);
        //        Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID)VALUES(" + operationid + ",TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1)",
        //                     "543 hesabının debitinə və 216 hesabının kreditinə silinən faiz daxil edilmədi.");
        //        //Class.GlobalProcedures.CalculatedOperationDebt(debit_account, date);
        //        //Class.GlobalProcedures.CalculatedOperationDebt(credit_account, date);

        //        //543 - 174
        //        operationid = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE");
        //        appointment = Class.GlobalFunctions.GetName("SELECT CT.CODE||C.CODE FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND C.ID = " + contractid) + " saylı müqavilə üzrə silinən əsas məbləğ";
        //        debit_account = 543 + customer_account;
        //        credit_account = 174 + leasing_account;
        //        amount_azn = Math.Round(basic_amount * currency_rate, 2);
        //        if (currency_id != 1)
        //            amount_cur = Math.Round(basic_amount, 2);
        //        Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID)VALUES(" + operationid + ",TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1)",
        //                     "543 hesabının debitinə və 174 hesabının kreditinə silinən əsas məbləğ daxil edilmədi.");
        //        //Class.GlobalProcedures.CalculatedOperationDebt(debit_account, date);
        //        //Class.GlobalProcedures.CalculatedOperationDebt(credit_account, date);


        //        if (currency_id != 1)
        //        {
        //            double contract_rate = Class.GlobalFunctions.GetAmount("SELECT RATE FROM CRS_USER.CONTRACTS_RATES WHERE CONTRACT_ID = " + contractid), diff_rate = 0;
        //            diff_rate = currency_rate - contract_rate;

        //            if (diff_rate > 0)
        //            {
        //                //174 - 612
        //                operationid = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE");
        //                appointment = Class.GlobalFunctions.GetName("SELECT CT.CODE||C.CODE FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND C.ID = " + contractid) + " saylı müqavilə üzrə məzənnə fərqindən gəlir";
        //                debit_account = 174 + leasing_account;
        //                credit_account = account612;
        //                amount_azn = Math.Round(basic_amount * diff_rate, 2);
        //                if (currency_id != 1)
        //                    amount_cur = Math.Round(basic_amount, 2);
        //                Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID)VALUES(" + operationid + ",TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(diff_rate, 4).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1)",
        //                             "174 hesabının debitinə və 612 hesabının kreditinə məzənnə fərqindən gəlir daxil edilmədi.");
        //                //Class.GlobalProcedures.CalculatedOperationDebt(debit_account, date);
        //                //Class.GlobalProcedures.CalculatedOperationDebt(credit_account, date);
        //            }
        //            else
        //            {
        //                //712 - 174
        //                operationid = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE");
        //                appointment = Class.GlobalFunctions.GetName("SELECT CT.CODE||C.CODE FROM CRS_USER.CONTRACTS C,CRS_USER.CREDIT_TYPE CT WHERE C.CREDIT_TYPE_ID = CT.ID AND C.ID = " + contractid) + " saylı müqavilə üzrə məzənnə fərqindən xərc";
        //                debit_account = account712;
        //                credit_account = 174 + leasing_account;
        //                amount_azn = Math.Abs(Math.Round(basic_amount * diff_rate, 2));
        //                if (currency_id != 1)
        //                    amount_cur = Math.Round(basic_amount, 2);
        //                Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID)VALUES(" + operationid + ",TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Abs(Math.Round(diff_rate, 4)).ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1)",
        //                             "712 hesabının debitinə və 174 hesabının kreditinə məzənnə fərqindən xərc daxil edilmədi.");
        //                //Class.GlobalProcedures.CalculatedOperationDebt(debit_account, date);
        //                //Class.GlobalProcedures.CalculatedOperationDebt(credit_account, date);
        //            }
        //        }
        //    }
        //    catch (Exception exx)
        //    {
        //        GlobalProcedures.LogWrite("Əməliyatlar jurnalın temp cədvəlinə daxil edilmədi.", Class.GlobalVariables.V_Commond.CommandText, Class.GlobalVariables.V_UserName, "Class", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
        //    }
        //}

        private void DebtsGridView_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    int ismanual = Convert.ToInt32(View.GetRowCellDisplayText(e.RowHandle, View.Columns["IS_MANUAL"]));
            //    if (ismanual == 0)
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(0x99, 0x00, 0x00);
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //    }
            //}
        }

        private void CalcDebtAgainBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DebtProgressPanel.Visible = true;
            //Class.GlobalProcedures.CalculatedOperationDebt(account, debtdate);
            LoadDebtsDataGridView();
            XtraMessageBox.Show(account + " hesabı üçün " + debtdate + " tarixindən etibarən qalıq yenidən hesablandı.");
            DebtProgressPanel.Visible = false;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string s = @"SELECT DISTINCT TO_CHAR (MIN(DEBT_DATE), 'DD.MM.YYYY') DEBT_DATE, ACCOUNT
            //                              FROM CRS_USER.OPERATION_DEBT
            //                             WHERE YR_MNTH_DY > 20151231
            //                            GROUP BY ACCOUNT ";
            //DataTable dt = GlobalFunctions.GenerateDataTable(s);
            //foreach(DataRow dr in dt.Rows)
            //{
            //    GlobalProcedures.CalculatedOperationDebt(dr[1].ToString(),dr[0].ToString());
            //}
        }

        private void DebtsGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string s3, OperationID;
            //try
            //{
            //    s3 = "SELECT TO_CHAR(OPERATION_DATE,'DD/MM/YYYY'),CREATED_USER_ID,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,ACCOUNT_OPERATION_TYPE_ID,IS_MANUAL FROM CRS_USER.OPERATION_JOURNAL_COPY WHERE CREATED_USER_ID = 25";
            //    DataTable dt = Class.GlobalFunctions.GenerateDataTable(s3);

            //    foreach (DataRow dr3 in dt.Rows)
            //    {
            //        OperationID = Class.GlobalFunctions.GetOracleSequenceValue("OPERATION_JOURNAL_SEQUENCE").ToString();
            //        Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(ID,OPERATION_DATE,CREATED_USER_ID,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,ACCOUNT_OPERATION_TYPE_ID,IS_MANUAL)VALUES(" + OperationID + ",TO_DATE('" + dr3[0] + "','DD/MM/YYYY')," + dr3[1] + ",'" + dr3[2] + "','" + dr3[3] + "'," + dr3[4] + "," + dr3[5] + "," + Convert.ToDouble(dr3[6].ToString()).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + dr3[7] + "'," + dr3[8] + ",1)",
            //                         dr3[2] + " hesabının debitinə və " + dr3[3] + " hesabının kreditinə məbləğ daxil edilmədi.");
            //        //Class.GlobalProcedures.CalculatedOperationDebt(dr3[2].ToString(), dr3[0].ToString());
            //        //Class.GlobalProcedures.CalculatedOperationDebt(dr3[3].ToString(), dr3[0].ToString());
            //    }

            //}
            //catch (Exception exx)
            //{
            //    XtraMessageBox.Show("Digər ödənişlər jurnala daxil olmadı" + "\nError - " + exx.Message);
            //}
        }
    }
}