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
using CRS.Class.DataAccess;
using CRS.Class.Views;
using CRS.Class.Tables;
using System.IO;
using System.Diagnostics;

namespace CRS.Forms.Contracts
{
    public partial class FExtendTime : DevExpress.XtraEditors.XtraForm
    {
        public FExtendTime()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractCode, CurrencyCode, CustomerName, PersonType;
        public decimal Debt, Interest;
        public int ContractID, CurrencyID;
        public int? ExtendID;
        public DateTime ContractStartDate;
        public bool Commit;

        decimal currecnt_debt = 0, interest_debt = 0;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        bool insert = false, CurrenctStatus = false, FormStatus = false;
        int version = 1;

        private void DebtValue_EditValueChanged(object sender, EventArgs e)
        {

        }

        private bool ControlDetails()
        {
            bool b = false;

            if (StartDate.Text == null)
            {
                StartDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qrafikin tarixi seçilməyib.");
                StartDate.Focus();
                StartDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            //List<ContractExtend> lstExtend = ContractExtendDAL.SelectContractExtend(1, ContractID).ToList<ContractExtend>();
            //if(lstExtend.Count > 0)
            //{
            //    if(lstExtend.Where(item => item.START_DATE == StartDate.DateTime).ToList<ContractExtend>().Count > 0)
            //    {
            //        StartDate.BackColor = Color.Red;
            //        GlobalProcedures.ShowErrorMessage(StartDate.Text + " tarixi üçün artıq uzadılma olub. Zəhmət olmasa başqa tarix seçin.");
            //        StartDate.Focus();
            //        StartDate.BackColor = GlobalFunctions.ElementColor();
            //        return false;
            //    }
            //    else if (lstExtend.Where(item => item.START_DATE > StartDate.DateTime).ToList<ContractExtend>().Count > 0)
            //    {
            //        StartDate.BackColor = Color.Red;
            //        GlobalProcedures.ShowErrorMessage("Qrafikin açılma tarixi ən son müddət uzadılmanın tarixindən (" + lstExtend.Max(item => item.START_DATE).ToString("dd.MM.yyyy") + ") boyük olmalıdır.");
            //        StartDate.Focus();
            //        StartDate.BackColor = GlobalFunctions.ElementColor();
            //        return false;
            //    }
            //    else
            //        b = true;
            //}
            //else
            //    b = true;            

            if (MonthCountValue.Value <= 0)
            {
                MonthCountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əlavə ayların sayı daxil edilməyib.");
                MonthCountValue.Focus();
                MonthCountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                int a = 0;
                if (TransactionName == "INSERT")
                    a = Insert();
                else
                    a = Update();
                if (a > 0)
                    CalcExtendPaymentShedules();

                if (FileCheck.Checked)
                    GenerateFile();
                this.Close();
            }
        }

        private int Insert()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.CONTRACT_EXTEND_TEMP(ID,
                                                                           CONTRACT_ID,
                                                                           START_DATE,
                                                                           END_DATE,
                                                                           MONTH_COUNT,
                                                                           INTEREST,
                                                                           DEBT,
                                                                           CURRENT_DEBT,
                                                                           INTEREST_DEBT,
                                                                           CHECK_INTEREST_DEBT,
                                                                           MONTHLY_AMOUNT,
                                                                           VERSION,
                                                                           NOTE,
                                                                           PAYMENT_TYPE,
                                                                           IS_CHANGE,
                                                                           USED_USER_ID)
                            VALUES(CRS_USER.CONTRACT_EXTEND_SEQUENCE.NEXTVAL,
                                   {ContractID},
                                   TO_DATE('{StartDate.Text}','DD.MM.YYYY'),
                                   TO_DATE('{EndDate.Text}','DD.MM.YYYY'),
                                   {MonthCountValue.Value},
                                   {InterestValue.Value},
                                   {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                   {currecnt_debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                   {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                   {((CheckInterestDebt.Checked) ? 1 : 0)},
                                   {Math.Round(MonthlyPaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                   {version},
                                   '{NoteText.Text.Trim()}',
                                   {PaymentTypeRadioGroup.SelectedIndex},
                                   1,{GlobalVariables.V_UserID}) RETURNING ID INTO :ID";

            return GlobalFunctions.InsertQuery(sql, "Müqavilənin müddəti uzadılmadı.", "ID", this.Name + "/Insert");
        }

        private int Update()
        {
            string sql = $@"UPDATE CRS_USER_TEMP.CONTRACT_EXTEND_TEMP SET  START_DATE = TO_DATE('{StartDate.Text}','DD.MM.YYYY'),
                                                                           END_DATE = TO_DATE('{EndDate.Text}','DD.MM.YYYY'),
                                                                           MONTH_COUNT = {MonthCountValue.Value},
                                                                           INTEREST = {InterestValue.Value},
                                                                           DEBT = {DebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                           CURRENT_DEBT = {currecnt_debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                           INTEREST_DEBT = {InterestDebtValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                           CHECK_INTEREST_DEBT = {((CheckInterestDebt.Checked) ? 1 : 0)},
                                                                           MONTHLY_AMOUNT = {Math.Round(MonthlyPaymentValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                           PAYMENT_TYPE = {PaymentTypeRadioGroup.SelectedIndex},
                                                                           IS_CHANGE = 1,
                                                                           NOTE = '{NoteText.Text.Trim()}'
                            WHERE ID = {ExtendID} RETURNING ID INTO :ID";

            return GlobalFunctions.InsertQuery(sql, "Müqavilənin müddətinin uzadılması dəyişilmədi.", "ID", this.Name + "/Insert");
        }

        private void GenerateFile()
        {
            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            object fileName = null, saveAs;
            string currency_name;

            fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Müddət uzadılma.docx");
            saveAs = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Uzadılma_" + ContractCode.Replace(@"/", null) + ".docx");

            List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(CurrencyID).ToList<Currency>();
            currency_name = lstCurrency.First().NAME;

            string date = GlobalFunctions.DateWithDayMonthYear(StartDate.DateTime);
            string month = MonthCountValue.Value + " (" + GlobalFunctions.IntegerToWritten((double)MonthCountValue.Value) + ")";
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;

            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;
            try
            {
                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                aDoc.Activate();
                
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", date);
                GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCode);                
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", CustomerName);
                GlobalProcedures.FindAndReplace(wordApp, "[$persontype]", PersonType.ToLower());
                GlobalProcedures.FindAndReplace(wordApp, "[$enddate]", EndDate.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$month]", month);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                if (File.Exists((string)saveAs))
                    Process.Start((string)saveAs);
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.LogWrite(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Uzadılma_" + ContractCode.Replace(@"/", null) + ".docx faylı açılmadı", null, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            finally
            {
                aDoc.Close(ref missing, ref missing, ref missing);
                wordApp.Quit();
                GlobalProcedures.SplashScreenClose();
            }
        }

        private void PaymentTypeRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PaymentTypeRadioGroup.SelectedIndex == 1)
                MonthlyPaymentValue.Properties.ReadOnly = CurrenctStatus;
            else
            {
                CalcMonthlyAmount();
                MonthlyPaymentValue.Properties.ReadOnly = true;
            }
        }

        private void InterestValue_EditValueChanged(object sender, EventArgs e)
        {
            if (InterestValue.EditorContainsFocus)
                CalcMonthlyAmount();
        }

        private void FExtendTime_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void CalcExtendPaymentShedules()
        {
            DateTime d_start, realDate;
            DayOfWeek day_of_week;
            int is_change_date = 0, interest = (int)Interest, month_count = (int)MonthCountValue.Value;
            double interest_amount = 0, basic_amount = 0, debt = (double)DebtValue.Value, monthly_amount = (double)MonthlyPaymentValue.Value;

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP SET IS_CHANGE = 2 
                                                            WHERE VERSION = {version} 
                                                                AND CONTRACT_ID = {ContractID} 
                                                                AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                        "Ödəniş qrafiki cədvəldən silinmədi.",
                                                 this.Name + "/CalcPaymentShedules");            

            for (int i = 1; i <= month_count; i++)
            {
                d_start = StartDate.DateTime;
                d_start = d_start.AddMonths(i);


                if (d_start > EndDate.DateTime)
                    d_start = EndDate.DateTime;
                day_of_week = d_start.DayOfWeek;//heftenin gunu
                is_change_date = 0;
                realDate = d_start;
                if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 6)
                {
                    d_start = d_start.AddDays(-1);
                    is_change_date = 1;
                }
                else if (GlobalFunctions.FindDayOfWeekNumber(day_of_week.ToString()) == 7)
                {
                    d_start = d_start.AddDays(-2);
                    is_change_date = 1;
                }
                interest_amount = ((debt * interest) / 100) / 360 * 30;
                if (i < month_count)
                    basic_amount = monthly_amount - interest_amount;
                else
                {
                    basic_amount = debt;
                    monthly_amount = debt + interest_amount;
                }
                debt = debt - basic_amount;

                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.PAYMENT_SCHEDULES_TEMP(CONTRACT_ID,
                                                                                                  MONTH_DATE,
                                                                                                  REAL_DATE,
                                                                                                  MONTHLY_PAYMENT,
                                                                                                  BASIC_AMOUNT,
                                                                                                  INTEREST_AMOUNT,
                                                                                                  DEBT,
                                                                                                  CURRENCY_ID,
                                                                                                  USED_USER_ID,
                                                                                                  IS_CHANGE,
                                                                                                  IS_CHANGE_DATE,
                                                                                                  ORDER_ID,
                                                                                                  VERSION)
                                                          VALUES({ContractID},
                                                                  TO_DATE('{d_start.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                  TO_DATE('{realDate.ToString("MM/dd/yyyy", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                                                  {monthly_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {basic_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {interest_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {debt.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                  {CurrencyID},
                                                                  {GlobalVariables.V_UserID},1,
                                                                  {is_change_date},
                                                                  {i},
                                                                  {version})",
                    "Ödəniş qrafiki cədvələ daxil edilmədi.", this.Name + "/CalcExtendPaymentShedules");
                is_change_date = 0;
            }

        }

        private void PaymentsLabel_Click(object sender, EventArgs e)
        {
            Bookkeeping.FShowPayments fsp = new Bookkeeping.FShowPayments();
            fsp.ContractID = ContractID.ToString();
            fsp.ShowDialog();
        }

        private void StartDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            string sql = $@"SELECT T.DEBT,
                                         TOTALS.PAYMENT_INTEREST_DEBT (T.P_STARTDATE,
                                                                       T.P_ENDDATE,
                                                                       T.DEBT,
                                                                       T.INTEREST,
                                                                       T.CONTRACT_ID,
                                                                       T.PAYMENT_INTEREST_AMOUNT)
                                            PAYMENT_INTEREST_DEBT
                                    FROM (WITH T1
                                               AS (SELECT CON.INTEREST,                          
                                                          CON.START_DATE,
                                                          CON.AMOUNT,  
                                                          CON.CONTRACT_ID,
                                                          CON.CUSTOMER_ID,
                                                          CON.CUSTOMER_TYPE_ID                          
                                                     FROM CRS_USER.V_CONTRACTS CON,                          
                                                          CRS_USER.V_CUSTOMERS CUS                          
                                                    WHERE     CON.IS_COMMIT = 1
                                                          AND CON.CUSTOMER_ID = CUS.ID
                                                          AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID                                                 
                                                          AND CON.START_DATE <=
                                                                 TO_DATE ('{StartDate.Text}', 'DD/MM/YYYY')),
                                               T2
                                               AS (  SELECT CP.CUSTOMER_ID,
                                                            CP.CONTRACT_ID,
                                                            CP.CUSTOMER_TYPE_ID,
                                                            SUM (CP.BASIC_AMOUNT) BASIC_AMOUNT,
                                                            SUM (CP.PAYMENT_INTEREST_AMOUNT)
                                                               PAYMENT_INTEREST_AMOUNT,
                                                            MAX (CP.PAYMENT_DATE) PAYMENT_DATE
                                                       FROM CRS_USER.CUSTOMER_PAYMENTS CP
                                                      WHERE CP.PAYMENT_DATE <=
                                                               TO_DATE ('{StartDate.Text}', 'DD/MM/YYYY')
                                                   GROUP BY CP.CUSTOMER_ID,
                                                            CP.CONTRACT_ID,
                                                            CP.CUSTOMER_TYPE_ID)
                                          SELECT T1.INTEREST,  
                                                 T1.AMOUNT - NVL (T2.BASIC_AMOUNT, 0) DEBT,
                                                 NVL (T2.PAYMENT_INTEREST_AMOUNT, 0) PAYMENT_INTEREST_AMOUNT,                 
                                                 T1.CONTRACT_ID,                 
                                                 (CASE
                                                     WHEN T2.PAYMENT_DATE IS NULL THEN T1.START_DATE
                                                     ELSE T2.PAYMENT_DATE
                                                  END)
                                                    P_STARTDATE,
                                                 TO_DATE ('{StartDate.Text}', 'DD/MM/YYYY') P_ENDDATE                 
                                            FROM T1
                                                 LEFT JOIN T2
                                                    ON     T1.CUSTOMER_ID = T2.CUSTOMER_ID
                                                       AND T1.CONTRACT_ID = T2.CONTRACT_ID
                                                       AND T1.CUSTOMER_TYPE_ID = T2.CUSTOMER_TYPE_ID) T
                                        WHERE CONTRACT_ID = {ContractID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/FExtendTime_Load", "Qalıq tapılmadı.");
            if (dt.Rows.Count > 0)
            {
                currecnt_debt = Convert.ToDecimal(dt.Rows[0]["DEBT"]);
                interest_debt = Convert.ToDecimal(dt.Rows[0]["PAYMENT_INTEREST_DEBT"]);
            }

            CurrentDebtValue.Value = (currecnt_debt == 0) ? Debt : currecnt_debt;
            InterestDebtValue.Value = interest_debt;

            MonthCountValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void CheckInterestDebt_CheckedChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            if (CheckInterestDebt.Checked)
                DebtValue.Value = CurrentDebtValue.Value + InterestDebtValue.Value;
            else
                DebtValue.Value = (currecnt_debt == 0) ? Debt : currecnt_debt;

            CalcMonthlyAmount();
        }

        private void MonthCountValue_EditValueChanged(object sender, EventArgs e)
        {
            if (!FormStatus)
                return;

            if (StartDate.Text.Length > 0)
                EndDate.EditValue = StartDate.DateTime.AddMonths((int)MonthCountValue.Value);
            else
                EndDate.Text = null;

            CalcMonthlyAmount();
        }

        private void FExtendTime_Load(object sender, EventArgs e)
        {
            this.Text = ContractCode + " saylı lizinq müqaviləsinin müddətinin uzadılması";
            CurrentDebtCurrencyLabel.Text = DebtCurrencyLabel.Text = InterestCurrencyLabel.Text = MonthlyPaymentCurrencyLabel.Text = CurrencyCode;
            List<Payments> lstPayments = PaymentsDAL.SelectPayments(0, ContractID).ToList<Payments>();
            List<ContractExtend> lstContractExtend = ContractExtendDAL.SelectContractExtend(1, ContractID).ToList<ContractExtend>();
            if (lstContractExtend.Count > 0)
            {
                if (TransactionName == "EDIT")
                {                    
                    var extend = lstContractExtend.Where(item => item.ID == ExtendID).LastOrDefault();
                    StartDate.EditValue = extend.START_DATE;
                    EndDate.EditValue = extend.END_DATE;
                    InterestValue.Value = extend.INTEREST;
                    currecnt_debt = extend.CURRENT_DEBT;
                    CurrentDebtValue.Value = currecnt_debt;
                    DebtValue.Value = extend.DEBT;
                    MonthCountValue.EditValue = extend.MONTH_COUNT;
                    InterestDebtValue.Value = extend.INTEREST_DEBT;
                    CheckInterestDebt.Checked = (extend.CHECK_INTEREST_DEBT == 1);
                    NoteText.Text = extend.NOTE;
                    MonthlyPaymentValue.EditValue = extend.MONTHLY_AMOUNT;
                    version = extend.VERSION;
                    

                    int pay_count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT CONTRACT_ID,PAYMENT_DATE FROM CRS_USER.CUSTOMER_PAYMENTS UNION ALL SELECT CONTRACT_ID,PAYMENT_DATE FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP) WHERE PAYMENT_DATE >= TO_DATE('{StartDate.Text}','DD.MM.YYYY') AND CONTRACT_ID = {ContractID}");
                    if (pay_count > 0)
                    {
                        GlobalProcedures.ShowWarningMessage("Seçilmiş müddət uzatma ilə " + StartDate.Text + " tarixindən sonra ödəniş olduğu üçün bu müddət uzatmanı dəyişmək olmaz. Siz yalnız məlumatlara baxa bilərsiz.");
                        CurrenctStatus = true;
                    }
                    else
                    {
                        if (extend.IS_CHANGE == 1)
                            CurrenctStatus = false;
                        else if (Commit)
                        {
                            GlobalProcedures.ShowWarningMessage("Seçilmiş müddət uzatma təsdiq edildiyi üçün onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.");
                            CurrenctStatus = true;
                        }
                        else
                        {
                            var lastExtend = lstContractExtend.Where(item => item.ID > ExtendID).ToList<ContractExtend>();
                            if (lastExtend.Count > 0)
                            {
                                GlobalProcedures.ShowWarningMessage("Seçilmiş müddət uzatmadan sonra başqa müddət uzatmalar olduğu üçün onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.");
                                CurrenctStatus = true;
                            }
                            else
                                CurrenctStatus = false;
                        }
                    }
                    PaymentTypeRadioGroup.SelectedIndex = extend.PAYMENT_TYPE;
                    ComponentEnable(CurrenctStatus);
                }
                else
                {
                    if (lstPayments.Count > 0 && lstPayments.LastOrDefault().PAYMENT_DATE > lstContractExtend.Max(item => item.START_DATE))
                        StartDate.Properties.MinValue = lstPayments.LastOrDefault().PAYMENT_DATE;
                    else
                        StartDate.Properties.MinValue = lstContractExtend.Max(item => item.START_DATE).AddDays(1);
                    version = lstContractExtend.LastOrDefault().VERSION + 1;
                }
            }
            else
            {
                if (lstPayments.Count > 0)
                    StartDate.Properties.MinValue = lstPayments.LastOrDefault().PAYMENT_DATE;
                else
                    StartDate.Properties.MinValue = ContractStartDate;
            }

            FormStatus = true;

            if (TransactionName == "INSERT")
            {
                StartDate.EditValue = DateTime.Today;
                InterestValue.Value = Interest;
            }
        }

        private void ComponentEnable(bool status)
        {
            StartDate.Enabled =
                EndDate.Enabled =
                MonthCountValue.Enabled =
                CheckInterestDebt.Enabled =
                NoteText.Enabled =
                PaymentTypeRadioGroup.Enabled =
                BOK.Visible = !status;
        }

        private void CalcMonthlyAmount()//ayliq odenisin hesablanmasi
        {
            double monthly_amount = 0;
            if (MonthCountValue.Value == 0)
            {
                monthly_amount = 0;
                return;
            }

            int diff_month = GlobalFunctions.DifferenceTwoDateWithMonth(StartDate.DateTime, EndDate.DateTime);
            monthly_amount = GlobalFunctions.CalcPayment((double)DebtValue.Value, diff_month, (double)InterestValue.Value);

            MonthlyPaymentValue.Value = (decimal)monthly_amount;
        }
    }
}