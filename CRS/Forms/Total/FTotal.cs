using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using CRS.Class;
using DevExpress.XtraReports.UI;

namespace CRS.Forms.Total
{
    public partial class FTotal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FTotal()
        {
            InitializeComponent();
        }
        string contract_id,
            contract_code,
            s_date,
            e_date,
            lizinq,
            customer_code,
            customer_name,
            commitment_name,
            customer_id,
            currency,
            toolTip = null,
            TitleText = null,
            currency_name,
            credit_id,
            person_type_id,
            seller_id,
            contract_evaluate,
            car_number,
            brandModel,
            hostageName,
            extend_start_date;

        string[] required_portfel_fonts = null;
        string[] delays_portfel_fonts = null;

        decimal amount,
            calc_debt = 0,
            calc_amount,
            calc_payment_amount,
            calc_basic_amount,
            calc_interest_amount,
            calc_payment_interest_amount,
            calc_payment_interest_debt,
            calc_total,
            rateAmount = 1,
            extendDebt = 0;

        int interest, period, topindex, old_row_num, status_id, commitment_id, customer_type_id, commitmentPersonTypeID = 0, delayedMonth, isSpecialAttention, credit_name_id, currency_id;
        bool FormStatus = false, isExtend = false;

        List<CurrencyRates> lstRate = null;
        List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(null).ToList<Currency>();
        Dictionary<string, Color> portfelFont = new Dictionary<string, Color>();


        private void TotalsGridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            GlobalProcedures.GridViewPrintInitializeByLandscape(e);
        }

        void RefreshTotals()
        {
            LoadTotalsDataGridView();
        }

        private void LoadFPayment(string contract_id, string contract_code, int interest, int period, string s_date, string e_date, string lizinq, string customer_code, string customer_name, string customer_id, double amount, string currency, string commitment_name, int commitment_id)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            FPayment fp = new FPayment();
            fp.ContractID = contract_id;
            fp.ContractCode = contract_code;
            fp.Interest = interest;
            fp.Period = period;
            fp.SDate = s_date;
            fp.EDate = e_date;
            fp.Lizinq = lizinq;
            fp.CustomerID = customer_id;
            fp.CustomerCode = customer_code;
            fp.CustomerName = customer_name;
            fp.CommitmentName = commitment_name;
            fp.CommitmentID = commitment_id;
            fp.CommitmentPersonTypeID = commitmentPersonTypeID;
            fp.Amount = amount;
            fp.Currency = currency;
            fp.CurrencyID = currency_id;
            fp.CustomerTypeID = customer_type_id;
            fp.DebtDate = DebtDateValue.Text;
            fp.IsExtend = isExtend;
            fp.IsSpecialAttention = isSpecialAttention;
            fp.CreditNameID = credit_name_id;
            fp.RefreshTotalsDataGridView += new FPayment.DoEvent(RefreshTotals);
            fp.ShowDialog();
            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void FTotal_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                PaymentBarButton.Enabled = GlobalVariables.AddPayment;
                PrintBarButton.Enabled = GlobalVariables.PrintPortfel;
                ExportBarSubItem.Enabled = GlobalVariables.ExportPortfel;
                if (GlobalVariables.CommitContract)
                    NewContractBarButton.Visibility = BarItemVisibility.Always;//eger commit etmek huququ varsa gosterilsin
                else
                    NewContractBarButton.Visibility = BarItemVisibility.Never;//eger commit etmek huququ yoxdursa gosterilmesin
                NewContractBarButton.Enabled = GlobalVariables.AddContract;
                EditContractBarButton.Enabled = GlobalVariables.EditContract;
                ColorBarButton.Enabled = GlobalVariables.ChangeColor;
                EditCustomerBarButton.Enabled = GlobalVariables.EditCustomer;
                ReportsBarButton.Enabled = GlobalVariables.ViewReports;
            }

            lstRate = CurrencyRatesDAL.SelectCurrencyRatesLastDate().ToList<CurrencyRates>();
            CurBarStatic.Caption = GlobalVariables.V_LastRate;
            SearchDockPanel.Hide();
            FormStatus = true;
        }

        static bool IsNullOrEmpty(string[] myStringArray)
        {
            return myStringArray == null || myStringArray.Length < 1;
        }

        private void LoadTotalsDataGridView()
        {
            string s = null, requiredFilter = null, delaysFilter = null;

            if (!String.IsNullOrEmpty(DebtDateValue.Text))
            {
                if (!IsNullOrEmpty(required_portfel_fonts))
                    requiredFilter = $@" AND (LT.REQUIRED / MONTHLY_AMOUNT)*100 BETWEEN {required_portfel_fonts[0].Trim()} AND {required_portfel_fonts[1].Trim()}";

                if (!IsNullOrEmpty(delays_portfel_fonts))
                    delaysFilter = $@" AND (LT.DELAYS / MONTHLY_AMOUNT)*100 BETWEEN {delays_portfel_fonts[0].Trim()} AND {delays_portfel_fonts[1].Trim()}";

                s = $@"SELECT * FROM 
                        (SELECT T.CUSTOMER_CODE,
                               T.FULL_CUSTOMER_NAME,
                               T.CONTRACT_CODE,
                               T.INTEREST,
                               T.PERIOD,
                               T.START_DATE,
                               T.END_DATE,
                               T.AMOUNT,
                               T.CURRENCY_CODE,
                               T.CURRENCY_ORDER,
                               T.PAYMENT_AMOUNT,
                               T.BASIC_AMOUNT,
                               T.DEBT,
                               (CASE
                                   WHEN T.STATUS_ID = 5 THEN TOTALS.DAYS360 (T.START_DATE, TO_DATE ('{DebtDateValue.Text}', 'DD/MM/YYYY'))
                                   ELSE TOTALS.DAYS360 (T.START_DATE, T.P_STARTDATE)
                                END)
                                  DAY_COUNT,
                               TOTALS.INTEREST_AMOUNT (T.P_STARTDATE,
                                                       T.P_ENDDATE,
                                                       T.DEBT,
                                                       T.INTEREST,
                                                       T.CONTRACT_ID)
                                  INTEREST_AMOUNT,
                               T.PAYMENT_INTEREST_AMOUNT,
                               TOTALS.PAYMENT_INTEREST_DEBT (T.P_STARTDATE,
                                                             T.P_ENDDATE,
                                                             T.DEBT,
                                                             T.INTEREST,
                                                             T.CONTRACT_ID,
                                                             T.PAYMENT_INTEREST_AMOUNT)
                                  PAYMENT_INTEREST_DEBT,
                               TOTALS.TOTAL (T.P_STARTDATE,
                                             T.P_ENDDATE,
                                             T.DEBT,
                                             T.INTEREST,
                                             T.CONTRACT_ID,
                                             T.PAYMENT_INTEREST_AMOUNT)
                                  TOTAL,
                               T.HOSTAGE,
                               T.LIQUID_AMOUNT,
                               T.SELLER_NAME,
                               T.COMMITMENT_NAME,
                               T.PHONES,
                               TOTALS.REQUIRED (T.P_STARTDATE,
                                                T.P_ENDDATE,
                                                T.DEBT,
                                                T.INTEREST,
                                                T.CONTRACT_ID,                                                
                                                T.PAYMENT_TYPE,
                                                T.PAYMENT_INTEREST_AMOUNT)
                                  REQUIRED,
                               TOTALS.DELAYS (T.P_STARTDATE,
                                              T.P_ENDDATE,
                                              T.DEBT,
                                              T.INTEREST,
                                              T.CONTRACT_ID,                                              
                                              T.PAYMENT_TYPE,
                                              T.PAYMENT_INTEREST_AMOUNT)
                                  DELAYS,
                               TOTALS.ORDERID (T.CONTRACT_ID) FULL_MONTH_COUNT,
                               T.MONTHLY_AMOUNT,
                               TOTALS.NORM_DEBT (T.CONTRACT_ID) NORM_DEBT,
                               T.MAX_PAYMENT_DATE,
                               T.CONTRACT_NOTE,
                               T.NOTE_CHANGE_USER,
                               T.NOTE_CHANGE_DATE,
                               T.USED_USER_ID,
                               T.CONTRACT_ID,
                               T.CUSTOMER_ID,
                               T.CUSTOMER_TYPE_ID,
                               T.SELLER_ID,
                               T.SELLER_TYPE_ID,
                               T.STATUS_ID,
                               T.CREDIT_TYPE_ID,
                               T.CREDIT_NAME_ID,
                               T.COMMITMENT_ID,
                               T.CUSTOMER_NAME,
                               T.COMMITMENT_PERSON_TYPE_ID,
                               T.CONTRACT_EVALUATE_NAME,
                               T.PAYMENT_DAY,
                               T.EXTEND_MONTHLY_AMOUNT,
                               T.IS_OLD,
                               T.INT_EXTEND_MONTH_COUNT,
                               T.INSURANCE_END_DATE,
                               T.INSURANCE_DIFF_MONTH,
                               T.IS_SPECIAL_ATTENTION,
                               T.CONTRACT_EVALUATE_ID,
                               T.HOSTAGE_NAME,
                               T.EXTEND_START_DATE,
                               T.EXTEND_DEBT,
                               TOTALS.DELAYS_DAY_COUNT (T.CONTRACT_ID) DELAYS_DAY_COUNT,
                               TOTALS.OVERDUE_PERCENT(T.CONTRACT_ID) OVERDUE_PERCENT,
                               T.CURRENCY_ID
                          FROM (WITH T1
                                     AS (SELECT CUS.CODE CUSTOMER_CODE,
                                                CUS.FULLNAME FULL_CUSTOMER_NAME,
                                                CUS.CUSTOMER_NAME,
                                                CON.CONTRACT_CODE,
                                                NVL2 (CE.INTEREST, CE.INTEREST, CON.INTEREST) INTEREST,
                                                NVL2 (CE.MONTH_COUNT, CE.MONTH_COUNT, CON.PERIOD) PERIOD,
                                                NVL2 (CE.START_DATE, CE.START_DATE, CON.START_DATE)
                                                        START_DATE,
                                                NVL(CE.END_DATE, CON.END_DATE) END_DATE,
                                                CON.AMOUNT,
                                                CON.CURRENCY_CODE,
                                                CON.CURRENCY_ORDER,
                                                H.HOSTAGE,
                                                H.LIQUID_AMOUNT,
                                                S.FULLNAME SELLER_NAME,
                                                COM.COMMITMENT_NAME,
                                                (CASE
                                                    WHEN COM.COMMITMENT_NAME IS NULL THEN CUS.PHONE
                                                    ELSE COM.PHONE
                                                 END)
                                                   PHONES,
                                                NVL2 (CE.MONTHLY_AMOUNT, ROUND(CE.MONTHLY_AMOUNT,2), CON.MONTHLY_AMOUNT)
                                                        MONTHLY_AMOUNT,
                                                CON.CONTRACT_NOTE,
                                                CON.NOTE_CHANGE_USER,
                                                CON.NOTE_CHANGE_DATE,
                                                CON.USED_USER_ID,
                                                CON.STATUS_ID,
                                                CON.CONTRACT_ID,
                                                CON.CUSTOMER_ID,
                                                CON.CUSTOMER_TYPE_ID,
                                                CON.SELLER_ID,
                                                CON.SELLER_TYPE_ID,
                                                CON.PAYMENT_TYPE,
                                                CON.CREDIT_TYPE_ID,
                                                CON.CREDIT_NAME_ID,
                                                NVL (COM.ID, 0) COMMITMENT_ID,
                                                CON.PARENT_ID,
                                                COM.PERSON_TYPE_ID COMMITMENT_PERSON_TYPE_ID,
                                                CON.CONTRACT_EVALUATE_NAME,
                                                CE.MONTHLY_AMOUNT EXTEND_MONTHLY_AMOUNT,
                                                (CASE WHEN CON.END_DATE < TRUNC(SYSDATE) THEN 1 ELSE 0 END) IS_OLD,
                                                CE.START_DATE EXTEND_START_DATE,
                                                NVL2(CE.MONTH_COUNT, CE.MONTH_COUNT,  0) INT_EXTEND_MONTH_COUNT,
                                                INS.END_DATE INSURANCE_END_DATE,
                                                  (CASE
                                                      WHEN INS.INSURANCE_PERIOD = CON.PERIOD AND INS.END_DATE < TRUNC(SYSDATE) THEN 103
                                                      WHEN INS.INSURANCE_PERIOD = CON.PERIOD AND INS.END_DATE >= TRUNC(SYSDATE) THEN 100
                                                      WHEN INS.IS_CANCEL = 1
                                                      THEN
                                                         101
                                                      WHEN INS.END_DATE < TRUNC(SYSDATE) THEN 102 
                                                         WHEN (INS.END_DATE - TRUNC(SYSDATE)) < 31  THEN 1
                                                         ELSE 0
                                                   END)
                                                     INSURANCE_DIFF_MONTH,
                                                  CON.IS_SPECIAL_ATTENTION,
                                                  CON.CONTRACT_EVALUATE_ID,
                                                  H.HOSTAGE_NAME,
                                                  NVL(CE.DEBT, 0) EXTEND_DEBT,
                                                  CON.CURRENCY_ID
                                           FROM CRS_USER.V_CONTRACTS CON,
                                                (SELECT *
                                                      FROM CRS_USER.V_LAST_CONTRACT_EXTEND
                                                     WHERE    START_DATE <= TO_DATE ('{DebtDateValue.Text}', 'DD/MM/YYYY')
                                                           OR START_DATE IS NULL) CE,
                                                CRS_USER.V_CUSTOMERS CUS,
                                                CRS_USER.V_HOSTAGE H,
                                                CRS_USER.V_SELLERS S,
                                                CRS_USER.V_CONTRACT_COMMITMENTS COM,
                                                CRS_USER.V_LAST_INSURANCES INS
                                          WHERE     CON.IS_COMMIT = 1
                                                AND CON.CUSTOMER_ID = CUS.ID
                                                AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                                AND CON.CONTRACT_ID = H.CONTRACT_ID
                                                AND CON.SELLER_ID = S.ID
                                                AND CON.SELLER_TYPE_ID = S.PERSON_TYPE_ID
                                                AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                                AND CON.CONTRACT_ID = CE.CONTRACT_ID(+)
                                                AND CON.CONTRACT_ID = INS.CONTRACT_ID(+)
                                                AND CON.STATUS_ID IN (5, 6)
                                                AND CON.START_DATE <=
                                                       TO_DATE ('{DebtDateValue.Text}', 'DD/MM/YYYY')),
                                     T2
                                     AS (  SELECT CP.CUSTOMER_ID,
                                                  CP.CONTRACT_ID,
                                                  CP.CUSTOMER_TYPE_ID,
                                                  SUM (CP.PAYMENT_AMOUNT) PAYMENT_AMOUNT,
                                                  SUM (CP.BASIC_AMOUNT) BASIC_AMOUNT,
                                                  SUM (CP.PAYMENT_INTEREST_AMOUNT)
                                                     PAYMENT_INTEREST_AMOUNT,
                                                  MAX (CP.PAYMENT_DATE) PAYMENT_DATE
                                             FROM CRS_USER.CUSTOMER_PAYMENTS CP
                                            WHERE CP.PAYMENT_DATE <=
                                                     TO_DATE ('{DebtDateValue.Text}', 'DD/MM/YYYY')
                                         GROUP BY CP.CUSTOMER_ID,
                                                  CP.CONTRACT_ID,
                                                  CP.CUSTOMER_TYPE_ID,
                                                  CP.USED_USER_ID)
                                SELECT T1.CUSTOMER_CODE,
                                       T1.FULL_CUSTOMER_NAME,
                                       T1.CUSTOMER_NAME,
                                       T1.CONTRACT_CODE,
                                       T1.INTEREST,
                                       T1.PERIOD,
                                       T1.START_DATE,
                                       T1.END_DATE,
                                       T1.AMOUNT,
                                       T1.CURRENCY_CODE,
                                       T1.CURRENCY_ORDER,
                                       NVL (T2.PAYMENT_AMOUNT, 0) PAYMENT_AMOUNT,
                                       NVL (T2.BASIC_AMOUNT, 0) BASIC_AMOUNT,
                                       T1.AMOUNT - NVL (T2.BASIC_AMOUNT, 0) DEBT,
                                       NVL (T2.PAYMENT_INTEREST_AMOUNT, 0) PAYMENT_INTEREST_AMOUNT,
                                       T2.PAYMENT_DATE MAX_PAYMENT_DATE,
                                       T1.HOSTAGE,
                                       T1.LIQUID_AMOUNT,
                                       T1.SELLER_NAME,
                                       T1.COMMITMENT_NAME,
                                       T1.MONTHLY_AMOUNT,
                                       T1.PHONES,
                                       T1.CONTRACT_NOTE,
                                       T1.NOTE_CHANGE_USER,
                                       T1.NOTE_CHANGE_DATE,
                                       T1.USED_USER_ID,
                                       T1.CONTRACT_ID,
                                       T1.CUSTOMER_ID,
                                       T1.CUSTOMER_TYPE_ID,
                                       T1.SELLER_ID,
                                       T1.SELLER_TYPE_ID,
                                       T1.STATUS_ID,
                                       T1.CREDIT_TYPE_ID,
                                       T1.CREDIT_NAME_ID,
                                       T1.COMMITMENT_ID,
                                       T1.PAYMENT_TYPE,
                                       (CASE
                                           WHEN T2.PAYMENT_DATE IS NULL THEN T1.START_DATE
                                           ELSE T2.PAYMENT_DATE
                                        END)
                                          P_STARTDATE,
                                       (CASE
                                           WHEN T1.STATUS_ID = 5
                                           THEN
                                              TO_DATE ('{DebtDateValue.Text}', 'DD/MM/YYYY')
                                           ELSE
                                              (CASE
                                                  WHEN T2.PAYMENT_DATE IS NULL THEN T1.START_DATE
                                                  ELSE T2.PAYMENT_DATE
                                               END)
                                        END)
                                          P_ENDDATE,
                                        T1.PARENT_ID,
                                        T1.COMMITMENT_PERSON_TYPE_ID,
                                        T1.CONTRACT_EVALUATE_NAME,
                                        EXTRACT (DAY FROM NVL2 (T1.EXTEND_START_DATE, T1.EXTEND_START_DATE, T1.START_DATE))
                                                        PAYMENT_DAY,
                                        T1.EXTEND_MONTHLY_AMOUNT,
                                        T1.IS_OLD,
                                        T1.INT_EXTEND_MONTH_COUNT,
                                        T1.INSURANCE_END_DATE,
                                        T1.INSURANCE_DIFF_MONTH,
                                        T1.IS_SPECIAL_ATTENTION,
                                        T1.CONTRACT_EVALUATE_ID,
                                        T1.HOSTAGE_NAME,
                                        T1.EXTEND_START_DATE,
                                        T1.EXTEND_DEBT,
                                        T1.CURRENCY_ID
                                  FROM T1
                                       LEFT JOIN
                                       T2
                                          ON     T1.CUSTOMER_ID = T2.CUSTOMER_ID
                                             AND T1.CONTRACT_ID = T2.CONTRACT_ID
                                             AND T1.CUSTOMER_TYPE_ID = T2.CUSTOMER_TYPE_ID) T
                                ORDER BY T.CONTRACT_CODE DESC) LT WHERE 1 = 1{requiredFilter}{delaysFilter}";
            }
            else
            {
                if (!IsNullOrEmpty(required_portfel_fonts))
                    requiredFilter = $@" AND (LT.REQUIRED / NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, CON.MONTHLY_AMOUNT))*100 BETWEEN {required_portfel_fonts[0].Trim()} AND {required_portfel_fonts[1].Trim()}";

                if (!IsNullOrEmpty(delays_portfel_fonts))
                    delaysFilter = $@" AND (LT.DELAYS / NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, CON.MONTHLY_AMOUNT))*100 BETWEEN {delays_portfel_fonts[0].Trim()} AND {delays_portfel_fonts[1].Trim()}";

                s = $@"SELECT CUS.CODE CUSTOMER_CODE,
                               CUS.FULLNAME FULL_CUSTOMER_NAME,
                               CON.CONTRACT_CODE,
                               NVL2 (CE.INTEREST, CE.INTEREST, CON.INTEREST) INTEREST,
                               NVL2 (CE.MONTH_COUNT, CE.MONTH_COUNT, CON.PERIOD) PERIOD,
                               NVL(CE.START_DATE, CON.START_DATE) START_DATE,
                               NVL(CE.END_DATE, CON.END_DATE) END_DATE,
                               CON.AMOUNT,
                               CON.CURRENCY_CODE,
                               CON.CURRENCY_ORDER,
                               LT.PAYMENT_AMOUNT,
                               LT.BASIC_AMOUNT,
                               LT.DEBT,
                               LT.DAY_COUNT,
                               LT.INTEREST_AMOUNT,
                               LT.PAYMENT_INTEREST_AMOUNT,
                               LT.PAYMENT_INTEREST_DEBT,
                               LT.TOTAL,
                               H.HOSTAGE,
                               H.LIQUID_AMOUNT,
                               S.FULLNAME SELLER_NAME,
                               COM.COMMITMENT_NAME,
                               (CASE
                                   WHEN COM.COMMITMENT_NAME IS NULL THEN CUS.PHONE
                                   ELSE COM.PHONE
                                END)
                                  PHONES,
                               LT.REQUIRED,
                               LT.DELAYS,
                               LT.FULL_MONTH_COUNT,
                               NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, CON.MONTHLY_AMOUNT)
                                        MONTHLY_AMOUNT,
                               LT.NORM_DEBT,
                               LT.MAX_PAYMENT_DATE,
                               LT.ROW_NUM,
                               CON.CONTRACT_NOTE,
                               CON.NOTE_CHANGE_USER,
                               CON.NOTE_CHANGE_DATE,
                               CON.USED_USER_ID,
                               LT.CONTRACT_ID,
                               CON.CUSTOMER_ID,
                               CON.CUSTOMER_TYPE_ID,
                               CON.SELLER_ID,
                               CON.SELLER_TYPE_ID,
                               CON.STATUS_ID,
                               CON.CREDIT_TYPE_ID,
                               CON.CREDIT_NAME_ID,
                               NVL (COM.ID, 0) COMMITMENT_ID,
                               CUS.CUSTOMER_NAME CUSTOMER_NAME,
                               CON.PARENT_ID,
                               COM.PERSON_TYPE_ID COMMITMENT_PERSON_TYPE_ID,
                               CON.CONTRACT_EVALUATE_NAME,
                               EXTRACT (DAY FROM NVL2 (CE.START_DATE, CE.START_DATE, CON.START_DATE))
                                        PAYMENT_DAY,
                               CE.MONTHLY_AMOUNT EXTEND_MONTHLY_AMOUNT,
                               (CASE WHEN CON.END_DATE < TRUNC(SYSDATE) THEN 1 ELSE 0 END) IS_OLD,
                               NVL2(CE.MONTH_COUNT, CE.MONTH_COUNT,  0) INT_EXTEND_MONTH_COUNT,
                               INS.END_DATE INSURANCE_END_DATE,
                               (CASE
                                     WHEN INS.INSURANCE_PERIOD = CON.PERIOD AND INS.END_DATE < TRUNC(SYSDATE) THEN 103
                                     WHEN INS.INSURANCE_PERIOD = CON.PERIOD AND INS.END_DATE >= TRUNC(SYSDATE) THEN 100
                                     WHEN INS.IS_CANCEL = 1 THEN 101
                                     WHEN INS.END_DATE < TRUNC(SYSDATE) THEN 102 
                                     WHEN (INS.END_DATE - TRUNC(SYSDATE)) < 31  THEN 1
                                     ELSE 0
                                  END)
                                    INSURANCE_DIFF_MONTH,
                               CON.IS_SPECIAL_ATTENTION,
                               CON.CONTRACT_EVALUATE_ID,
                               H.HOSTAGE_NAME,
                               CE.START_DATE EXTEND_START_DATE,
                               NVL(CE.DEBT, 0) EXTEND_DEBT,
                               LT.DELAYS_DAY_COUNT,
                               CON.CURRENCY_ID,
                               LT.OVERDUE_PERCENT
                          FROM CRS_USER.LEASING_TOTAL LT,
                               CRS_USER.V_CUSTOMERS CUS,
                               CRS_USER.V_HOSTAGE H,
                               CRS_USER.V_SELLERS S,
                               CRS_USER.V_CONTRACT_COMMITMENTS COM,
                               CRS_USER.V_CONTRACTS CON,
                               CRS_USER.V_LAST_CONTRACT_EXTEND CE,
                               CRS_USER.V_LAST_INSURANCES INS
                         WHERE     LT.CONTRACT_ID = CON.CONTRACT_ID
                               AND CON.CUSTOMER_ID = CUS.ID
                               AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                               AND CON.CONTRACT_ID = H.CONTRACT_ID(+)
                               AND CON.SELLER_ID = S.ID(+)
                               AND CON.SELLER_TYPE_ID = S.PERSON_TYPE_ID(+)
                               AND CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                               AND CON.CONTRACT_ID = CE.CONTRACT_ID(+)
                               AND CON.CONTRACT_ID = INS.CONTRACT_ID(+)
                               AND CON.STATUS_ID IN (5, 6)
                               AND CON.IS_COMMIT = 1{requiredFilter}{delaysFilter}                              
                        ORDER BY CON.CONTRACT_CODE DESC";
            }
            TotalsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadTotalsDataGridView", "Portfel yüklənmədi.");
            if (TotalsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                    PaymentBarButton.Enabled = GlobalVariables.AddPayment;
                else
                    PaymentBarButton.Enabled = true;
                PaymentScheduleBarButton.Enabled = AttentionBarButton.Enabled = EditContractBarButton.Enabled = EditCustomerBarButton.Enabled = ContractStatusEditBarButton.Enabled = true;
            }
            else
                PaymentBarButton.Enabled =
                    PaymentScheduleBarButton.Enabled =
                    AddSpeacialAttentionBarButton.Enabled =
                    DeleteSpecialAttentionBarButton.Enabled =
                    AttentionBarButton.Enabled =
                    EditContractBarButton.Enabled =
                    EditCustomerBarButton.Enabled =
                    ContractStatusEditBarButton.Enabled = false;

            if ((ActiveContractCheck.Checked) && (!ClosedContractCheck.Checked))
                TotalsGridView.ActiveFilter.Add(TotalsGridView.Columns["STATUS_ID"],
              new ColumnFilterInfo("[STATUS_ID] = 5", ""));
        }

        private void TotalsGridView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("CONTRACT_CODE", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("BASIC_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_AMOUNT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("PAYMENT_INTEREST_DEBT", "Far", e);
            GlobalProcedures.GridCustomDrawFooterCell("TOTAL", "Far", e);

            if (e.Column.FieldName == "DEBT")
            {
                e.Handled = true;
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }

            if (e.Column.FieldName == "DAY_COUNT")
            {
                e.Handled = true;
                e.Appearance.FontStyleDelta = FontStyle.Regular;
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, e.Bounds);
            }
        }

        private void TotalsGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = TotalsGridView.GetFocusedDataRow();
            if (row != null)
            {
                customer_id = row["CUSTOMER_ID"].ToString();
                contract_id = row["CONTRACT_ID"].ToString();
                seller_id = row["SELLER_ID"].ToString();
                customer_name = row["CUSTOMER_NAME"].ToString();
                commitment_name = row["COMMITMENT_NAME"].ToString();
                customer_code = row["CUSTOMER_CODE"].ToString();
                contract_code = row["CONTRACT_CODE"].ToString();
                period = Convert.ToInt32(row["PERIOD"].ToString());
                lizinq = row["HOSTAGE"].ToString();
                s_date = row["START_DATE"].ToString().Substring(0, 10);
                e_date = row["END_DATE"].ToString().Substring(0, 10);
                if (row["EXTEND_START_DATE"].ToString() != string.Empty)
                    extend_start_date = row["EXTEND_START_DATE"].ToString().Substring(0, 10);
                currency = row["CURRENCY_CODE"].ToString();
                amount = Convert.ToDecimal(row["AMOUNT"].ToString());
                extendDebt = Convert.ToDecimal(row["EXTEND_DEBT"].ToString());
                interest = Convert.ToInt32(row["INTEREST"].ToString());
                commitment_id = Convert.ToInt32(row["COMMITMENT_ID"].ToString());
                if (!String.IsNullOrWhiteSpace(row["COMMITMENT_PERSON_TYPE_ID"].ToString()))
                    commitmentPersonTypeID = Convert.ToInt32(row["COMMITMENT_PERSON_TYPE_ID"].ToString());
                else
                    commitmentPersonTypeID = 0;
                customer_type_id = Convert.ToInt32(row["CUSTOMER_TYPE_ID"]);
                currency_id = Convert.ToInt32(row["CURRENCY_ID"]);
                status_id = Convert.ToInt32(row["STATUS_ID"].ToString());
                credit_name_id = Convert.ToInt32(row["CREDIT_NAME_ID"]);

                car_number = lizinq.Substring(0, 7);
                //brandModel = hostage.Substring(hostage.IndexOf('=') + 1, hostage.IndexOf('-') - hostage.IndexOf('=') - 1);
                decimal required = 0;
                if (row["REQUIRED"].ToString() != string.Empty)
                    required = Convert.ToDecimal(row["REQUIRED"]);
                decimal monthlyAmount = Convert.ToDecimal(row["MONTHLY_AMOUNT"]);
                delayedMonth = (int)(required / ((monthlyAmount != 0) ? monthlyAmount : 1));
                hostageName = row["HOSTAGE_NAME"].ToString();
                isExtend = !(String.IsNullOrWhiteSpace(row["EXTEND_MONTHLY_AMOUNT"].ToString()));
                isSpecialAttention = Convert.ToInt32(row["IS_SPECIAL_ATTENTION"].ToString());
                AddSpeacialAttentionBarButton.Enabled = (isSpecialAttention == 0) ? GlobalVariables.AddSpecialAttention : false;
                DeleteSpecialAttentionBarButton.Enabled = (isSpecialAttention == 1) ? GlobalVariables.DeleteSpecialAttention : false;
            }
        }

        private void TotalsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (PaymentBarButton.Enabled)
                LoadFPayment(contract_id, contract_code, interest, period, s_date, e_date, lizinq, customer_code, customer_name, customer_id, (double)amount, currency, commitment_name, commitment_id);
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurBarStatic.Caption = CurrencyRatesDAL.LastRateString();
            LoadTotalsDataGridView();
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            FilterTotals();
        }

        private void TemporaryPaymentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FTemporaryPayments ft = new FTemporaryPayments();
            ft.ShowDialog();
        }

        private void LoadDelaysToList()
        {
            string sql = $@"SELECT ROW_NUMBER () OVER (ORDER BY C.CONTRACT_CODE DESC) ROW_NUM,
                                 C.CONTRACT_CODE,
                                 NVL (
                                    COM.COMMITMENT_NAME,
                                    DECODE (CUS.PERSON_TYPE_ID,
                                            1, CUS.CUSTOMER_NAME,
                                            SUBSTR (CUS.CUSTOMER_NAME, 0, 26)))
                                    CUSTOMER_NAME,
                                 NVL(SUBSTR (NVL (COM.PHONE, CUS.PHONE), 0, 55),'-') PHONE,
                                 NVL(H.CAR_NUMBER,'-') CAR_NUMBER,
                                 C.CURRENCY_CODE,
                                 LT.DELAYS,
                                 NVL2 (CE.MONTHLY_AMOUNT, CE.MONTHLY_AMOUNT, C.MONTHLY_AMOUNT)
                                        MONTHLY_AMOUNT
                            FROM CRS_USER.LEASING_TOTAL LT,
                                 CRS_USER.V_CONTRACTS C,
                                 CRS_USER.V_CUSTOMERS CUS,
                                 CRS_USER.V_COMMITMENTS COM,
                                 CRS_USER.HOSTAGE_CAR H,
                                 CRS_USER.CONTRACT_EXTEND CE
                           WHERE     LT.DELAYS > 0
                                 AND LT.CONTRACT_ID = C.CONTRACT_ID
                                 AND C.STATUS_ID = 5
                                 AND CUS.ID = C.CUSTOMER_ID
                                 AND C.CONTRACT_ID = COM.CONTRACT_ID(+)
                                 AND C.CONTRACT_ID = H.CONTRACT_ID(+)
                                 AND C.CONTRACT_ID = CE.CONTRACT_ID(+)
                        ORDER BY C.CONTRACT_CODE DESC";

            DelaysListDAL.RemoveAllDelays();

            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                DelaysListDAL.InsertDelays(int.Parse(dr["ROW_NUM"].ToString()),
                                           dr["CONTRACT_CODE"].ToString(),
                                           dr["CURRENCY_CODE"].ToString(),
                                           dr["CUSTOMER_NAME"].ToString(),
                                           dr["CAR_NUMBER"].ToString(),
                                           dr["PHONE"].ToString(),
                                           double.Parse(dr["DELAYS"].ToString()),
                                           double.Parse(dr["MONTHLY_AMOUNT"].ToString()));
            }
        }

        private void PrintDelaysBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            LoadDelaysToList();

            Reports.DelaysList report = new Reports.DelaysList();
            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
            report.ShowPrintMarginsWarning = false;
            report.RequestParameters = false;

            report.DataSource = DelaysListDAL.lstDelays;
            GlobalProcedures.SplashScreenClose();
            new ReportPrintTool(report).ShowPreview();
            report.PrintingSystem.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.Parameters, new object[] { true });
        }

        private void AttentionBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FAttentions fa = new FAttentions();
            fa.ContractCode = contract_code;
            fa.CustomerName = (commitment_name.Length != 0) ? commitment_name : customer_name;
            fa.ContractID = contract_id;
            fa.ShowDialog();
        }

        private void PersonTypeComboBox_EditValueChanged(object sender, EventArgs e)
        {
            string s = null, person_type = null;
            if (!String.IsNullOrEmpty(PersonTypeComboBox.Text))
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        person_type = " WHERE NAME IN ('" + PersonTypeComboBox.Text.Replace("; ", "','") + "')";
                        break;
                    case "EN":
                        person_type = " WHERE NAME_EN IN ('" + PersonTypeComboBox.Text.Replace("; ", "','") + "')";
                        break;
                    case "RU":
                        person_type = " WHERE NAME_RU IN ('" + PersonTypeComboBox.Text.Replace("; ", "','") + "')";
                        break;
                }
            }

            if (String.IsNullOrEmpty(person_type))
                person_type_id = null;
            else
            {
                s = $@"SELECT * FROM (SELECT '[CUSTOMER_TYPE_ID] IN ('||LTRIM(SYS_CONNECT_BY_PATH(ID,','),',')||')' FROM (SELECT ID,LAG(ID) OVER (ORDER BY ID) AS PREV_ID FROM CRS_USER.PERSON_TYPE {person_type}) START WITH PREV_ID IS NULL CONNECT BY PREV_ID = PRIOR ID ORDER BY 1 DESC) WHERE ROWNUM = 1";
                person_type_id = GlobalFunctions.GetName(s, this.Name + "/PersonTypeComboBox_EditValueChanged");
                if (String.IsNullOrEmpty(person_type_id))
                    person_type_id = "[CUSTOMER_TYPE_ID] IS NULL";
            }
            FilterTotals();
        }

        void RefreshContracts(string contract_id)
        {
            LoadTotalsDataGridView();
        }

        private void GroupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FContractGroups fcg = new FContractGroups();
            fcg.ShowDialog();
        }

        private void LoadFContractAddEdit(string transaction, string contractid, string customerid, string sellerid)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            Contracts.FCarOrObjectContractAddEdit fcae = new Contracts.FCarOrObjectContractAddEdit();
            fcae.TransactionName = transaction;
            fcae.ContractID = contractid;
            fcae.CustomerID = customerid;
            fcae.SellerID = sellerid;
            fcae.Commit = 1;
            fcae.IsExtend = isExtend;
            fcae.RefreshContractsDataGridView += new Contracts.FCarOrObjectContractAddEdit.DoEvent(RefreshContracts);
            fcae.ShowDialog();
            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void NewContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFContractAddEdit("INSERT", null, null, null);
        }

        private void LoadFContractStatusEdit()
        {
            Contracts.FContractStatusEdit fs = new Contracts.FContractStatusEdit();
            fs.ContractID = contract_id;
            fs.ContractCode = contract_code;
            fs.CustomerName = customer_name;
            fs.CommitmentName = commitment_name;
            fs.CommitmentID = commitment_id;
            fs.CommitmentPersonTypeID = commitmentPersonTypeID;
            fs.RefreshTotalsDataGridView += new Contracts.FContractStatusEdit.DoEvent(RefreshTotals);
            fs.ShowDialog();
        }

        private void LettersBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FContractWarningLetters fl = new FContractWarningLetters();
            fl.ShowDialog();
        }

        private void NewLetterBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (delayedMonth == 0)
            {
                GlobalProcedures.ShowWarningMessage(contract_code + " saylı lizinq müqaviləsi üzrə gecikmə ayı sıfır olduğu üçün xəbərdarlıq məktubu yaradıla bilməz.");
                return;
            }

            string letterAmount = null;

            List<ContractCommitment> lstCommitment = CommitmentsDAL.SelectAllCommitmentByContractID(int.Parse(contract_id)).ToList<ContractCommitment>();
            if (lstCommitment.Count > 0)
            {
                var commitment = lstCommitment.LastOrDefault();
                var builderCommitment = new StringBuilder();
                builderCommitment.Append('"');
                builderCommitment.Append(commitment.AGREEMENTDATE.Day);
                builderCommitment.Append('"');
                letterAmount = builderCommitment.ToString() + " " + GlobalFunctions.FindMonth(commitment.AGREEMENTDATE.Month) + " " + GlobalFunctions.FindYear(commitment.AGREEMENTDATE) + " tarixli Əlavə Razılaşmaya əsasən tərəfinizdən " + commitment.DEBT.ToString("n2") + " " + commitment.CURRENCY_CODE;
            }
            else
                letterAmount = "əsasən tərəfinizdən " + (extendDebt > 0 ? extendDebt : amount).ToString("n2") + " " + currency;

            DateTime contractDate = GlobalFunctions.ChangeStringToDate(extend_start_date == null ? s_date : extend_start_date, "ddmmyyyy");
            var builder = new StringBuilder();
            builder.Append('"');
            builder.Append(contractDate.Day);
            builder.Append('"');

            FWarningLetter fw = new FWarningLetter();
            fw.ContractNumber = contract_code;
            fw.ContractDate = builder.ToString() + " " + GlobalFunctions.FindMonth(contractDate.Month) + " " + GlobalFunctions.FindYear(contractDate);
            fw.CustomerName = (commitment_name == String.Empty) ? customer_name : commitment_name;
            fw.ContractAmount = letterAmount;
            fw.PaymentDay = contractDate.Day.ToString();
            fw.CarNumber = car_number;
            fw.ContractID = contract_id;
            fw.CreditNameID = credit_name_id;
            fw.HostageName = hostageName;
            fw.Delayes = delayedMonth.ToString();
            fw.BrandAndModel = brandModel;
            fw.ShowDialog();
        }

        private void Ribbon_Click(object sender, EventArgs e)
        {

        }

        private void EditCustomerBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (customer_type_id == 1)
                LoadFCustomerAddEdit("EDIT", customer_id, customer_name);
            else
                LoadFJuridicalPersonAddEdit("EDIT", customer_id);
        }

        void RefreshCustomers(string code)
        {
            LoadTotalsDataGridView();
        }

        private void LoadFCustomerAddEdit(string transaction, string customer_id, string fullname)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            Customer.L1FCustomerAddEdit fcae = new Customer.L1FCustomerAddEdit();
            fcae.TransactionName = transaction;
            fcae.CustomerID = customer_id;
            fcae.CustomerFullName = fullname;
            fcae.RefreshCustomersDataGridView += new Customer.L1FCustomerAddEdit.DoEvent(RefreshCustomers);
            fcae.ShowDialog();

            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void AddSpeacialAttentionBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş " + contract_code + " saylı Lizinq Müqaviləsinə xüsusi nəzarət əlavə etmək istəyirsiniz?", "Xüsusi nəzarət", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_SPECIAL_ATTENTION = 1 WHERE ID = {contract_id}", "Xüsusi nəzarət əlavə edilmədi.", this.Name + "/AddSpeacialAttentionBarButton_ItemClick");
            }
            LoadTotalsDataGridView();
            TotalsGridView.TopRowIndex = topindex;
            TotalsGridView.FocusedRowHandle = old_row_num;
        }

        private void DeleteSpecialAttentionBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş " + contract_code + " saylı Lizinq Müqaviləsindən xüsusi nəzarəti silmək istəyirsiniz?", "Xüsusi nəzarət", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CONTRACTS SET IS_SPECIAL_ATTENTION = 0 WHERE ID = {contract_id}", "Xüsusi nəzarət əlavə edilmədi.", this.Name + "/AddSpeacialAttentionBarButton_ItemClick");
            }
            LoadTotalsDataGridView();
            TotalsGridView.TopRowIndex = topindex;
            TotalsGridView.FocusedRowHandle = old_row_num;
        }

        private void FontsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            required_portfel_fonts = null;
            if (FontsComboBox.Text.Length > 0)
                required_portfel_fonts = FontsComboBox.Text.Split('-');
            LoadTotalsDataGridView();
        }

        private void TotalsGridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            GlobalProcedures.GridSaveLayout(TotalsGridView, TotalRibbonPage.Text);
        }

        private void FontsComboBox_Properties_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            e.AllowDrawSkinBackground = false;
            Color value;
            if (portfelFont.TryGetValue(e.Item.ToString(), out value))
                e.Appearance.BackColor = value;
        }

        private void DelaysFontsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            delays_portfel_fonts = null;
            if (DelaysFontsComboBox.Text.Length > 0)
                delays_portfel_fonts = DelaysFontsComboBox.Text.Split('-');
            LoadTotalsDataGridView();
        }

        private void LoadFJuridicalPersonAddEdit(string transaction, string person_id)
        {
            topindex = TotalsGridView.TopRowIndex;
            old_row_num = TotalsGridView.FocusedRowHandle;

            Customer.FJuridicalPersonAddEdit fj = new Customer.FJuridicalPersonAddEdit();
            fj.TransactionName = transaction;
            fj.CustomerID = person_id;
            fj.RefreshCustomersDataGridView += new Customer.FJuridicalPersonAddEdit.DoEvent(RefreshCustomers);
            fj.ShowDialog();

            TotalsGridView.FocusedRowHandle = old_row_num;
            TotalsGridView.TopRowIndex = topindex;
        }

        private void ReportsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FPortfelReports fpr = new FPortfelReports();
            fpr.ShowDialog();
        }

        private void EvaluateComboBox_EditValueChanged(object sender, EventArgs e)
        {
            contract_evaluate = " [CONTRACT_EVALUATE_NAME] IN ('" + EvaluateComboBox.Text.Replace("; ", "','") + "')";
            FilterTotals();
        }

        private void FromPaymentDay_EditValueChanged(object sender, EventArgs e)
        {
            ToPaymentDay.Properties.MinValue = FromPaymentDay.Value;
        }

        private void ToPaymentDay_EditValueChanged(object sender, EventArgs e)
        {
            FilterTotals();
        }

        private void WarningLetterBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ContractStatusEditBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFContractStatusEdit();
        }

        private void TotalToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            GridHitInfo hitInfo = TotalsGridView.CalcHitInfo(e.ControlMousePosition);
            DataRow drCurrentRow = TotalsGridView.GetDataRow(hitInfo.RowHandle);

            if (drCurrentRow == null)
                return;

            if (String.IsNullOrEmpty(drCurrentRow["CONTRACT_NOTE"].ToString()))
                return;

            if (e.SelectedControl != TotalsGridControl)
                return;

            if (hitInfo.InRow == false)
                return;

            if (hitInfo.Column == null)
                return;

            if (hitInfo.Column != Totals_CustomerName && hitInfo.Column != Totals_Note)
                return;

            //address = null; mail = null;

            SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
            toolTipArgs.AllowHtmlText = DefaultBoolean.True;

            toolTip = null;
            TitleText = drCurrentRow["CUSTOMER_NAME"].ToString();
            toolTipArgs.Title.Text = "<color=255,0,0>" + TitleText + "</color>";
            toolTipArgs.Contents.Text = drCurrentRow["CONTRACT_NOTE"].ToString();
            toolTipArgs.Contents.Image = Properties.Resources.notes_32;
            toolTip = "<i>Ən son dəyişiklik etmiş istifadəçi</i> : <b><color=104,6,6>" + drCurrentRow["NOTE_CHANGE_USER"].ToString() + "</color></b>\n" +
                            "<i>Dəyişilmə vaxtı</i> : <b><color=104,6,6>" + drCurrentRow["NOTE_CHANGE_DATE"].ToString() + "</color></b>";
            toolTipArgs.Footer.Text = toolTip;
            toolTipArgs.ShowFooterSeparator = true;
            e.Info = new ToolTipControlInfo();
            e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
            e.Info.ToolTipType = ToolTipType.SuperTip;
            e.Info.SuperTip = new SuperToolTip();
            e.Info.SuperTip.Setup(toolTipArgs);
        }

        private void TotalsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(TotalsGridView, PopupMenu, e);
        }

        private void ExcelBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "xls");
        }

        private void PdfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "pdf");
        }

        private void RtfBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "rtf");
        }

        private void HtmlBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "html");
        }

        private void TxtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "txt");
        }

        private void CsvBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "csv");
        }

        private void MhtBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(TotalsGridControl, "mht");
        }

        private void PrintBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(TotalsGridControl);
        }

        private void TotalsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            decimal monthly_amount = 0, delya_value = 0, res = 0;
            //GlobalProcedures.GridRowCellStyleForBlock(TotalsGridView, e);
            GridView currentView = sender as GridView;

            if (e.Column.FieldName == "DELAYS")
            {
                monthly_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "MONTHLY_AMOUNT"));
                delya_value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DELAYS"));
                if (monthly_amount != 0)
                    res = Math.Round((delya_value / monthly_amount) * 100, 2);
                GlobalProcedures.FindFontDetails(res, e);
            }

            if (e.Column.FieldName == "REQUIRED")
            {
                monthly_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "MONTHLY_AMOUNT"));
                delya_value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DELAYS"));
                if (monthly_amount != 0)
                    res = Math.Round((delya_value / monthly_amount) * 100, 2);
                GlobalProcedures.FindFontDetails(res, e);
            }

            //if (e.Column.FieldName == "REQUIRED")
            //{
            //    monthly_amount = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "MONTHLY_AMOUNT"));
            //    delya_value = Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "REQUIRED"));
            //    if (monthly_amount != 0)
            //        res = Math.Round((delya_value / monthly_amount) * 100, 2);
            //    GlobalProcedures.FindFontDetails(res, e);
            //}

            if (e.Column.FieldName == "CONTRACT_CODE")
            {
                int credit_type_id = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CREDIT_TYPE_ID"));
                GlobalProcedures.FindFontDetailsforCreditType(credit_type_id, e);
            }

            if (e.Column.FieldName == "FULL_CUSTOMER_NAME")
            {
                int evaluateID = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "CONTRACT_EVALUATE_ID"));
                if (evaluateID == 4)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.DarkOrange;
            }

            if (e.Column.FieldName == "START_DATE")
            {
                int isOld = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "IS_OLD"));
                if (isOld == 1)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.OrangeRed;
            }

            if (e.Column.FieldName == "END_DATE")
            {
                int isOld = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "IS_OLD"));
                if (isOld == 1)
                    e.Appearance.BackColor = e.Appearance.BackColor2 = Color.OrangeRed;
                else
                {
                    bool extend = (Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "INT_EXTEND_MONTH_COUNT")) > 0);
                    if (extend)
                    {
                        e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_ExtendColor1);
                        e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_ExtendColor2);
                    }
                }
            }

            if (e.Column.FieldName == "FULL_MONTH_COUNT" || e.Column.FieldName == "MONTHLY_AMOUNT" || e.Column.FieldName == "PAYMENT_DAY")
            {
                bool extend = (Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "INT_EXTEND_MONTH_COUNT")) > 0);
                if (extend)
                    e.Appearance.FontStyleDelta = FontStyle.Italic;
            }

            if (e.Column.FieldName == "INSURANCE_END_DATE")
            {
                if (!String.IsNullOrWhiteSpace(currentView.GetRowCellValue(e.RowHandle, "INSURANCE_DIFF_MONTH").ToString()))
                {
                    int monthCount = Convert.ToInt32(currentView.GetRowCellValue(e.RowHandle, "INSURANCE_DIFF_MONTH"));
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

            GlobalProcedures.GridRowCellStyleForBlock(TotalsGridView, e);
            //if (Math.Round(Convert.ToDecimal(currentView.GetRowCellValue(e.RowHandle, "DEBT")), 2) <= 0)
            GlobalProcedures.GridRowCellStyleForClose(6, TotalsGridView, e);
        }

        private void SearchBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SearchBarButton.Down)
            {
                SearchDockPanel.Show();
                GlobalProcedures.FillCheckedComboBox(CurrencyComboBox, "CURRENCY", "CODE,CODE,CODE", null);
                GlobalProcedures.FillCheckedComboBox(CreditNameComboBox, "CREDIT_NAMES", "NAME,NAME_EN,NAME_RU", "ID != 6 ORDER BY NAME");
                GlobalProcedures.FillCheckedComboBox(PersonTypeComboBox, "PERSON_TYPE", "NAME,NAME_EN,NAME_RU", null);
                GlobalProcedures.FillCheckedComboBox(EvaluateComboBox, "CONTRACT_EVALUATE", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
                GlobalProcedures.FillComboBoxEditWithSqlText(FontsComboBox, "SELECT START_VALUE||' - '||END_VALUE VALUE FROM CRS_USER.PORTFEL_FONTS ORDER BY ID");
                GlobalProcedures.FillComboBoxEditWithSqlText(DelaysFontsComboBox, "SELECT START_VALUE||' - '||END_VALUE VALUE FROM CRS_USER.PORTFEL_FONTS ORDER BY ID");

                string sql = $@"SELECT START_VALUE||' - '||END_VALUE VALUE,BACKCOLOR_A,BACKCOLOR_R,BACKCOLOR_G,BACKCOLOR_B,BACKCOLOR_TYPE,BACKCOLOR_NAME FROM CRS_USER.PORTFEL_FONTS";
                DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SearchBarButton_ItemClick", "Portfelin rengleri açılmadı.");
                if (dt.Rows.Count > 0)
                    portfelFont.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    Color c = GlobalFunctions.CreateColor(dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                    portfelFont.Add(dr[0].ToString(), c);
                }
            }
            else
                SearchDockPanel.Hide();
        }

        private void SearchDockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            SearchBarButton.Down = false;
        }

        private void FilterTotals()
        {
            if (FormStatus)
            {
                ColumnView view = TotalsGridView;

                //Currency
                if (!String.IsNullOrEmpty(CurrencyComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CURRENCY_CODE"],
                        new ColumnFilterInfo(currency_name, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CURRENCY_CODE"]);

                //CreditName
                if (!String.IsNullOrEmpty(CreditNameComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CREDIT_NAME_ID"],
                        new ColumnFilterInfo(credit_id, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CREDIT_NAME_ID"]);

                //PersonType
                if (!String.IsNullOrEmpty(PersonTypeComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CUSTOMER_TYPE_ID"],
                        new ColumnFilterInfo(person_type_id, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CUSTOMER_TYPE_ID"]);

                //Evaluate
                if (!String.IsNullOrEmpty(EvaluateComboBox.Text))
                    view.ActiveFilter.Add(view.Columns["CONTRACT_EVALUATE_NAME"],
                        new ColumnFilterInfo(contract_evaluate, ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CONTRACT_EVALUATE_NAME"]);

                //Delays
                if ((DelaysCheck.Checked) && (!MoreThanCheck.Checked))
                    view.ActiveFilter.Add(view.Columns["DELAYS"],
                  new ColumnFilterInfo("[DELAYS] > 0.00", ""));
                else if ((!DelaysCheck.Checked) && (MoreThanCheck.Checked))
                    view.ActiveFilter.Add(view.Columns["DELAYS"],
                  new ColumnFilterInfo("[DELAYS] <= 0.00", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["DELAYS"]);

                //Status
                if ((ActiveContractCheck.Checked) && (!ClosedContractCheck.Checked))
                    view.ActiveFilter.Add(view.Columns["STATUS_ID"],
                  new ColumnFilterInfo("[STATUS_ID] = 5", ""));
                else if ((!ActiveContractCheck.Checked) && (ClosedContractCheck.Checked))
                    view.ActiveFilter.Add(view.Columns["STATUS_ID"],
                  new ColumnFilterInfo("[STATUS_ID] = 6", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["STATUS_ID"]);

                //Note
                if (!String.IsNullOrEmpty(NoteText.Text))
                    view.ActiveFilter.Add(view.Columns["CONTRACT_NOTE"],
                  new ColumnFilterInfo("[CONTRACT_NOTE] Like '%" + NoteText.Text + "%'", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CONTRACT_NOTE"]);

                //Razılaşdırılmış
                if (AgreementContractCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["PARENT_ID"],
                  new ColumnFilterInfo("[PARENT_ID] IS NOT NULL", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["PARENT_ID"]);

                //Ohdelik
                if (IsCommitmentContractCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["COMMITMENT_NAME"],
                  new ColumnFilterInfo("[COMMITMENT_NAME] IS NOT NULL", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["COMMITMENT_NAME"]);

                // Axtaris
                if (SearchCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["CONTRACT_EVALUATE_ID"],
                  new ColumnFilterInfo("[CONTRACT_EVALUATE_ID] = 4", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["CONTRACT_EVALUATE_ID"]);

                //Vaxti bitmis
                if (IsOldCheck1.Checked && !IsOldCheck0.Checked)
                    view.ActiveFilter.Add(view.Columns["IS_OLD"],
                  new ColumnFilterInfo("[IS_OLD] = 1", ""));
                else if (!IsOldCheck1.Checked && IsOldCheck0.Checked)
                    view.ActiveFilter.Add(view.Columns["IS_OLD"],
                  new ColumnFilterInfo("[IS_OLD] = 0", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["IS_OLD"]);

                //Sigorta
                if (OldInsuranceCheck.Checked && !OneMonthInsuranceCheck.Checked && !ActiveInsuranceCheck.Checked && !CancelInsuranceCheck.Checked && !EqualInsuranceCheck.Checked && !OldEqualInsuranceCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["INSURANCE_DIFF_MONTH"],
                  new ColumnFilterInfo("[INSURANCE_DIFF_MONTH] = 102", ""));
                else if (!OldInsuranceCheck.Checked && !OneMonthInsuranceCheck.Checked && !ActiveInsuranceCheck.Checked && !CancelInsuranceCheck.Checked && !EqualInsuranceCheck.Checked && OldEqualInsuranceCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["INSURANCE_DIFF_MONTH"],
                  new ColumnFilterInfo("[INSURANCE_DIFF_MONTH] = 103", ""));
                else if (!OldInsuranceCheck.Checked && OneMonthInsuranceCheck.Checked && !ActiveInsuranceCheck.Checked && !CancelInsuranceCheck.Checked && !EqualInsuranceCheck.Checked && !OldEqualInsuranceCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["INSURANCE_DIFF_MONTH"],
                  new ColumnFilterInfo("[INSURANCE_DIFF_MONTH] = 1", ""));
                else if (!OldInsuranceCheck.Checked && !OneMonthInsuranceCheck.Checked && !ActiveInsuranceCheck.Checked && CancelInsuranceCheck.Checked && !EqualInsuranceCheck.Checked && !OldEqualInsuranceCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["INSURANCE_DIFF_MONTH"],
                  new ColumnFilterInfo("[INSURANCE_DIFF_MONTH] = 101", ""));
                else if (!OldInsuranceCheck.Checked && !OneMonthInsuranceCheck.Checked && ActiveInsuranceCheck.Checked && !CancelInsuranceCheck.Checked && !EqualInsuranceCheck.Checked && !OldEqualInsuranceCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["INSURANCE_DIFF_MONTH"],
                  new ColumnFilterInfo("[INSURANCE_DIFF_MONTH] = 0 AND [INSURANCE_END_DATE] IS NOT NULL", ""));
                else if (!OldInsuranceCheck.Checked && !OneMonthInsuranceCheck.Checked && !ActiveInsuranceCheck.Checked && !CancelInsuranceCheck.Checked && EqualInsuranceCheck.Checked && !OldEqualInsuranceCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["INSURANCE_DIFF_MONTH"],
                  new ColumnFilterInfo("[INSURANCE_DIFF_MONTH] = 100", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["INSURANCE_DIFF_MONTH"]);

                //Nezaret
                if (SpecialAttentionCheck.Checked && !NotSpecialAttentionCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["IS_SPECIAL_ATTENTION"],
                  new ColumnFilterInfo("[IS_SPECIAL_ATTENTION] = 1", ""));
                else if (!SpecialAttentionCheck.Checked && NotSpecialAttentionCheck.Checked)
                    view.ActiveFilter.Add(view.Columns["IS_SPECIAL_ATTENTION"],
                  new ColumnFilterInfo("[IS_SPECIAL_ATTENTION] = 0", ""));
                else
                    view.ActiveFilter.Remove(view.Columns["IS_SPECIAL_ATTENTION"]);
            }
        }

        private void FilterClearBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ClearFilter();
        }

        void ClearFilter()
        {
            CurrencyComboBox.Text =
                CreditNameComboBox.Text =
                EvaluateComboBox.Text =
                NoteText.Text =
                DebtDateValue.Text =
                FontsComboBox.Text =
                DelaysFontsComboBox.Text = null;
            DelaysCheck.Checked =
                MoreThanCheck.Checked =
                ActiveContractCheck.Checked =
                ClosedContractCheck.Checked =
                IsOldCheck0.Checked =
                IsOldCheck1.Checked =
                OldInsuranceCheck.Checked =
                OneMonthInsuranceCheck.Checked =
                ActiveInsuranceCheck.Checked =
                CancelInsuranceCheck.Checked =
                EqualInsuranceCheck.Checked = false;

            TotalsGridView.ClearColumnsFilter();
        }

        private void CurrencyComboBox_EditValueChanged(object sender, EventArgs e)
        {
            currency_name = " [CURRENCY_CODE] IN ('" + CurrencyComboBox.Text.Replace("; ", "','") + "')";
            FilterTotals();
        }

        private void CreditNameComboBox_EditValueChanged(object sender, EventArgs e)
        {
            string s = null, credit_name = null;
            if (!String.IsNullOrEmpty(CreditNameComboBox.Text))
            {
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "AZ":
                        credit_name = " WHERE NAME IN ('" + CreditNameComboBox.Text.Replace("; ", "','") + "')";
                        break;
                    case "EN":
                        credit_name = " WHERE NAME_EN IN ('" + CreditNameComboBox.Text.Replace("; ", "','") + "')";
                        break;
                    case "RU":
                        credit_name = " WHERE NAME_RU IN ('" + CreditNameComboBox.Text.Replace("; ", "','") + "')";
                        break;
                }
            }

            if (String.IsNullOrEmpty(credit_name))
                credit_id = null;
            else
            {
                s = "SELECT * FROM (SELECT '[CREDIT_NAME_ID] IN ('||LTRIM(SYS_CONNECT_BY_PATH(ID,','),',')||')' FROM (SELECT ID,LAG(ID) OVER (ORDER BY ID) AS PREV_ID FROM CRS_USER.CREDIT_NAMES " + credit_name + ") START WITH PREV_ID IS NULL CONNECT BY PREV_ID = PRIOR ID ORDER BY 1 DESC) WHERE ROWNUM = 1";
                credit_id = GlobalFunctions.GetName(s);
                if (String.IsNullOrEmpty(credit_id))
                    credit_id = "[CREDIT_NAME_ID] IS NULL";
            }
            FilterTotals();
        }

        private void PaymentScheduleBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string cellValue = TotalsGridView.GetRowCellValue(TotalsGridView.FocusedRowHandle, Totals_FullMonthCount).ToString();
                topindex = TotalsGridView.TopRowIndex;
                old_row_num = TotalsGridView.FocusedRowHandle;
                FPaymentSchedules fps = new FPaymentSchedules();
                fps.ContractID = contract_id;
                fps.Amount = amount.ToString("N2") + " " + currency;
                fps.ContractCode = contract_code;
                fps.OrderID = cellValue.Remove(cellValue.IndexOf(' ')).Trim();
                fps.CurrencyCode = currency;
                fps.ShowDialog();
                TotalsGridView.FocusedRowHandle = old_row_num;
                TotalsGridView.TopRowIndex = topindex;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müştərinin ödəniş qrafikini açmaq üçün həmin müştərini seçin.", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void TotalsGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
                return;

            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                calc_debt =
                    calc_amount =
                    calc_basic_amount =
                    calc_interest_amount =
                    calc_payment_amount =
                    calc_payment_interest_amount =
                    calc_payment_interest_debt =
                    calc_total = 0;
            }


            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0) //gelir
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT") == 0) //odenilen
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_payment_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0) //esas mebleg
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_basic_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //gelir
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_debt += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0) //hesablanmis faiz
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_interest_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_AMOUNT") == 0) //odenilen faiz
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_payment_interest_amount += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) //qaliq faiz
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_payment_interest_debt += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("TOTAL") == 0) //cemi
                {
                    var currency = lstCurrency.Find(c => c.CODE == e.GetValue("CURRENCY_CODE").ToString());
                    var rate = lstRate.Find(r => r.CURRENCY_ID == currency.ID);

                    if (rate != null)
                        rateAmount = rate.AMOUNT;
                    else if (currency.CODE == "AZN")
                        rateAmount = 1;
                    else
                        rateAmount = 0;
                    calc_total += Convert.ToDecimal(e.FieldValue) * rateAmount;
                }
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("AMOUNT") == 0) //verilen              
                    e.TotalValue = calc_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_AMOUNT") == 0) //odenilen
                    e.TotalValue = calc_payment_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("BASIC_AMOUNT") == 0) //esas mebleg
                    e.TotalValue = calc_basic_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEBT") == 0) //qaliq
                {
                    e.TotalValue = calc_debt;
                    TotalDebtBarStatic.Caption = "YEKUN PORTFEL = " + calc_debt.ToString("N2") + " AZN";
                }

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("INTEREST_AMOUNT") == 0) //hesablanmis faiz
                    e.TotalValue = calc_interest_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_AMOUNT") == 0) //odenilen faiz
                    e.TotalValue = calc_payment_interest_amount;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("PAYMENT_INTEREST_DEBT") == 0) //qaliq faiz
                    e.TotalValue = calc_payment_interest_debt;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("TOTAL") == 0) //cemi
                    e.TotalValue = calc_total;

                if (((GridSummaryItem)e.Item).FieldName.CompareTo("FULL_CUSTOMER_NAME") == 0 && TotalsGridView.RowCount > 0) //cemi
                {
                    e.TotalValue = e.GetValue("CURRENCY_CODE").ToString() + " üzrə cəmi";
                    if (e.IsTotalSummary)
                        e.TotalValue = "YEKUN (AZN - ilə)";
                }
            }
        }

        private void TotalsGridView_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0} : {1}   (cəmi  {2} müqavilə)", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));
        }

        private void TotalsGridView_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "FULL_CUSTOMER_NAME")
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
            if (e.Column.FieldName == "DEBT")
                e.Appearance.ForeColor = Color.Red;
        }

        private void FTotal_Activated(object sender, EventArgs e)
        {
            LoadTotalsDataGridView();
            if (SearchBarButton.Down)
                FilterTotals();

            GlobalProcedures.GridRestoreLayout(TotalsGridView, TotalRibbonPage.Text);
        }

        void RefreshData()
        {
            GlobalProcedures.FillComboBoxEditWithSqlText(FontsComboBox, "SELECT START_VALUE||' - '||END_VALUE VALUE FROM CRS_USER.PORTFEL_FONTS ORDER BY ID");
            GlobalProcedures.FillComboBoxEditWithSqlText(DelaysFontsComboBox, "SELECT START_VALUE||' - '||END_VALUE VALUE FROM CRS_USER.PORTFEL_FONTS ORDER BY ID");

            string sql = $@"SELECT START_VALUE||' - '||END_VALUE VALUE,BACKCOLOR_A,BACKCOLOR_R,BACKCOLOR_G,BACKCOLOR_B,BACKCOLOR_TYPE,BACKCOLOR_NAME FROM CRS_USER.PORTFEL_FONTS";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/SearchBarButton_ItemClick", "Portfelin rengleri açılmadı.");
            if (dt.Rows.Count > 0)
                portfelFont.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Color c = GlobalFunctions.CreateColor(dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                portfelFont.Add(dr[0].ToString(), c);
            }

            LoadTotalsDataGridView();
        }

        private void ColorBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FConditionalFormatting fc = new FConditionalFormatting();
            fc.RefreshDataGridView += new FConditionalFormatting.DoEvent(RefreshData);
            fc.ShowDialog();
        }

        private void PaymentBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFPayment(contract_id, contract_code, interest, period, s_date, e_date, lizinq, customer_code, customer_name, customer_id, (double)amount, currency, commitment_name, commitment_id);
        }

        private void DelaysCheck_CheckedChanged(object sender, EventArgs e)
        {
            FilterTotals();
            GlobalProcedures.ChangeCheckStyle((sender as CheckEdit));
        }

        private void TotalsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (TotalsGridView.RowCount > 0)
            {
                if (GlobalVariables.V_UserID > 0)
                    PaymentBarButton.Enabled = GlobalVariables.AddPayment;
                else
                    PaymentBarButton.Enabled = true;
                PaymentScheduleBarButton.Enabled = AttentionBarButton.Enabled = EditContractBarButton.Enabled = EditCustomerBarButton.Enabled = ContractStatusEditBarButton.Enabled = true;
            }
            else
                PaymentBarButton.Enabled =
                    PaymentScheduleBarButton.Enabled =
                    AddSpeacialAttentionBarButton.Enabled =
                    DeleteSpecialAttentionBarButton.Enabled =
                    AttentionBarButton.Enabled =
                    EditContractBarButton.Enabled =
                    EditCustomerBarButton.Enabled =
                    ContractStatusEditBarButton.Enabled = false;

            GridView view = sender as GridView;
            if (view.ActiveFilterString == "")
                ClearFilter();
        }

        private void CurrentPaymentsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            FCurrentPayments fcp = new FCurrentPayments();
            fcp.ShowDialog();
        }

        private void DebtDateValue_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCalculatedWait));
            if (!String.IsNullOrEmpty(DebtDateValue.Text))
            {
                TotalsGridView.OptionsView.ShowViewCaption = true;
                TotalsGridView.ViewCaption = DebtDateValue.Text + " tarixinə olan lizinq portfeli";
            }
            else
            {
                TotalsGridView.OptionsView.ShowViewCaption = false;
            }

            LoadTotalsDataGridView();
            GlobalProcedures.SplashScreenClose();
        }

        private void TotalsGridView_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            int rowIndex = e.ListSourceRowIndex;
            if (e.Column == Totals_Note)
            {
                if (TotalsGridView.GetListSourceRowCellValue(rowIndex, "CONTRACT_NOTE").ToString().Length > 0)
                    e.Value = Properties.Resources.notes_16;
                else
                    e.Value = null;
            }

            if (e.Column == Totals_SpecialAttention)
            {
                if (int.Parse(TotalsGridView.GetListSourceRowCellValue(rowIndex, "IS_SPECIAL_ATTENTION").ToString()) == 1)
                    e.Value = Properties.Resources.attention_161;
                else
                    e.Value = null;
            }

            if (e.Column == Totals_Interest)
            {
                object interest = TotalsGridView.GetListSourceRowCellValue(rowIndex, "INTEREST");
                e.Value = interest.ToString() + " %";
            }

            if (e.Column == Totals_Period)
            {
                object period = TotalsGridView.GetListSourceRowCellValue(rowIndex, "PERIOD");
                e.Value = period.ToString() + " ay";
            }

            if (e.Column == Totals_InsuranceStatus)
            {
                int insuranceDiffMonth = Convert.ToInt16(TotalsGridView.GetListSourceRowCellValue(rowIndex, "INSURANCE_DIFF_MONTH"));
                if (insuranceDiffMonth == 100)
                    e.Value = "Eyni müddətli";
                else if (insuranceDiffMonth == 102 || insuranceDiffMonth == 103)
                    e.Value = "Vaxtı bitib";
                else if (insuranceDiffMonth == 101)
                    e.Value = "İmtina edilib";
                else if (insuranceDiffMonth == 1)
                    e.Value = "Az qalıb";
                else
                    e.Value = null;
            }

            //if (e.Column == Totals_PaymentDay)
            //{
            //    DateTime startDate = DateTime.Parse(TotalsGridView.GetListSourceRowCellValue(rowIndex, "START_DATE").ToString());                
            //    e.Value = startDate.Day;
            //}
        }

        private void NoteBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (status_id == 5)
            {
                topindex = TotalsGridView.TopRowIndex;
                old_row_num = TotalsGridView.FocusedRowHandle;
                Contracts.FNote fn = new Contracts.FNote();
                fn.CustomerName = customer_name;
                fn.ContractCode = contract_code;
                fn.CustomerID = customer_id;
                fn.ContractID = contract_id;
                fn.RefreshContractDataGridView += new Contracts.FNote.DoEvent(RefreshContracts);
                fn.ShowDialog();
                TotalsGridView.FocusedRowHandle = old_row_num;
                TotalsGridView.TopRowIndex = topindex;
                GlobalProcedures.CalculatedLeasingTotal(contract_id);
            }
            else
                XtraMessageBox.Show(contract_code + " saylı lizinq müqaviləsi bağlanıldığı üçün bu müqaviləyə qeyd yazmaq olmaz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void EditContractBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadFContractAddEdit("EDIT", contract_id, customer_id, seller_id);
        }
    }
}