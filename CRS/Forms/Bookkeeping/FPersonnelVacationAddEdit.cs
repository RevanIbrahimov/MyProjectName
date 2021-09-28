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

namespace CRS.Forms.Bookkeeping
{
    public partial class FPersonnelVacationAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPersonnelVacationAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public DateTime StartDate, EndDate;
        public int PersonnelID, VacationDateID, DayCount;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        decimal[] salaryArray = new decimal[12];

        private void FromDateValue_EditValueChanged(object sender, EventArgs e)
        {
            ToDateValue.Properties.MinValue = FromDateValue.DateTime;
            ToDateValue_EditValueChanged(sender, EventArgs.Empty);
        }

        private void FPersonnelVacationAddEdit_Load(object sender, EventArgs e)
        {
            FromDateValue.Properties.MinValue = StartDate;
            FromDateValue.Properties.MaxValue = ToDateValue.Properties.MaxValue = EndDate;

            if (TransactionName == "EDIT")
                LoadDetails();
        }

        private void LoadDetails()
        {
            string sql = $@"SELECT START_DATE,END_DATE FROM CRS_USER_TEMP.PERSONNEL_VACATIONS_TEMP WHERE ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Məzuniyyətin detalları açılmadı.");

            if(dt.Rows.Count > 0)
            {
                FromDateValue.EditValue = dt.Rows[0]["START_DATE"];
                ToDateValue.EditValue = dt.Rows[0]["END_DATE"];
            }
        }

        private void SalaryGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void FPersonnelVacationAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (FromDateValue.Text.Length == 0)
            {
                FromDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məzuniyyətin başlama tarixi seçilməyib.");
                FromDateValue.Focus();
                FromDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if(GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PERSONNEL_VACATIONS_TEMP WHERE TO_DATE('{FromDateValue.Text}','DD.MM.YYYY') BETWEEN START_DATE AND END_DATE") > 0)
            {
                FromDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Artıq " + FromDateValue.Text + " tarixi üçün məzuniyyət götürülüb. Zəhmət olmasa başqa tarix seçin.");
                FromDateValue.Focus();
                FromDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (ToDateValue.Text.Length == 0)
            {
                ToDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məzuniyyətin bitmə tarixi seçilməyib.");
                ToDateValue.Focus();
                ToDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PERSONNEL_VACATIONS_TEMP WHERE TO_DATE('{ToDateValue.Text}','DD.MM.YYYY') BETWEEN START_DATE AND END_DATE") > 0)
            {
                ToDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Artıq " + ToDateValue.Text + " tarixi üçün məzuniyyət götürülüb. Zəhmət olmasa başqa tarix seçin.");
                ToDateValue.Focus();
                ToDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (int.Parse(DayCountText.Text) > DayCount)
            {
                FromDateValue.BackColor = Color.Red;
                ToDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məzuniyyət günlərinin sayı cari il üçün qalan məzuniyyət günlərinin sayından (" + DayCount + " gün) çox ola bilməz.");
                FromDateValue.Focus();
                FromDateValue.BackColor = GlobalFunctions.ElementColor();
                ToDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void Insert()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.PERSONNEL_VACATIONS_TEMP (ID,
                                                                                   PERSONNEL_VACATION_DATE_ID,
                                                                                   START_DATE,
                                                                                   END_DATE,
                                                                                   DAY_COUNT,
                                                                                   SALARY_AVERAGE,
                                                                                   ONE_DAY_VACATION_AMOUNT,
                                                                                   TOTAL_AMOUNT,
                                                                                   IS_CHANGE,
                                                                                   USED_USER_ID)
                           VALUES(CRS_USER.PERSONNEL_VACATION_SEQUENCE.NEXTVAL,
                                  {VacationDateID},
                                  TO_DATE('{FromDateValue.Text}','DD.MM.YYYY'),
                                  TO_DATE('{ToDateValue.Text}','DD.MM.YYYY'),
                                  {DayCountText.Text},
                                  {SalaryAverageValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                  {OneDayVacationAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                  {TotalAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                  1,
                                  {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Məzuniyyət temp cədvələ daxil olmadı.", this.Name + "/Insert");
        }

        public void Update()
        {
            string sql = $@"UPDATE CRS_USER_TEMP.PERSONNEL_VACATIONS_TEMP SET START_DATE = TO_DATE('{FromDateValue.Text}','DD.MM.YYYY'),
                                                                                   END_DATE = TO_DATE('{ToDateValue.Text}','DD.MM.YYYY'),
                                                                                   DAY_COUNT = {DayCountText.Text},
                                                                                   SALARY_AVERAGE = {SalaryAverageValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                   ONE_DAY_VACATION_AMOUNT = {OneDayVacationAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                   TOTAL_AMOUNT = {TotalAmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                   IS_CHANGE = 1
                            WHERE ID = {ID}";

            GlobalProcedures.ExecuteQuery(sql, "Məzuniyyət temp cədvəldə dəyişdirilmədi.", this.Name + "/Update");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void ToDateValue_EditValueChanged(object sender, EventArgs e)
        {
            if (FromDateValue.Text.Length > 0 && ToDateValue.Text.Length > 0)
            {
                DayCountText.Text = GlobalFunctions.Days360(FromDateValue.DateTime, ToDateValue.DateTime).ToString();
                LoadSalary();
            }
        }

        private void LoadSalary()
        {
            string sql = $@"SELECT 1 SS,
                                     PS.ID,
                                     PS.SDATE,
                                     PS.SALARY
                                FROM CRS_USER.PERSONNEL_SALARY PS
                               WHERE     PS.SDATE BETWEEN ADD_MONTHS (
                                                             LAST_DAY (TO_DATE ('{FromDateValue.Text}', 'DD/MM/YYYY')),
                                                             -12)
                                                      AND ADD_MONTHS (
                                                             LAST_DAY (TO_DATE ('{FromDateValue.Text}', 'DD/MM/YYYY')),
                                                             -1)
                                     AND PS.PERSONNEL_ID = {PersonnelID}
                            ORDER BY SDATE";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadSalary", "Əmək haqqıların siyahısı açılmadı.");
            SalaryGridControl.DataSource = dt;

            List<decimal> salaryList = new List<decimal>();
            Dictionary<decimal, int> salaryDict = new Dictionary<decimal, int>();
            decimal salaryAverage = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    salaryList.Add(Convert.ToDecimal(dr["SALARY"]));
                }

                salaryArray = salaryList.ToArray();

                for (int i = 0; i < salaryArray.Length; i++)
                {
                    if (salaryDict.ContainsKey(salaryArray[i]))
                        salaryDict[salaryArray[i]] = salaryDict[salaryArray[i]] + 1;
                    else
                        salaryDict.Add(salaryArray[i], 1);
                }
            }

            decimal firstSalary = 0, calc = 0;
            int n = 1, sumValue = 0, firstValue = 0;
            string averageToolTip = null;
            foreach (var item in salaryDict)
            {
                if (n == 1)
                {
                    firstSalary = item.Key;
                    firstValue = item.Value;
                }

                if (n > 1)
                {
                    if (firstSalary > 0)
                    {
                        calc += firstSalary * Math.Round(item.Key / firstSalary, 2, MidpointRounding.AwayFromZero) * firstValue;
                        averageToolTip += firstSalary.ToString() + " * " + Math.Round(item.Key / firstSalary, 2, MidpointRounding.AwayFromZero).ToString() + " * " + firstValue.ToString() + " + ";
                    }
                    firstSalary = item.Key;
                    firstValue = item.Value;
                }

                if (n == salaryDict.Count)
                {
                    calc += firstSalary * firstValue;
                    averageToolTip += firstSalary.ToString() + " * " + firstValue.ToString();
                }

                n++;

                sumValue += +item.Value;
            }

            if (sumValue > 0)
                salaryAverage = Math.Round(calc / sumValue, 2, MidpointRounding.AwayFromZero);

            SalaryAverageValue.EditValue = salaryAverage;
            OneDayVacationAmountValue.EditValue = Math.Round(SalaryAverageValue.Value / (decimal)30.4, 2, MidpointRounding.AwayFromZero);
            TotalAmountValue.EditValue = Math.Round(OneDayVacationAmountValue.Value * int.Parse(DayCountText.Text), 2, MidpointRounding.AwayFromZero);

            SalaryAverageValue.ToolTip = averageToolTip + " = " + SalaryAverageValue.Value.ToString();
            OneDayVacationAmountValue.ToolTip = SalaryAverageValue.Value.ToString() + " / 30.4 = " + OneDayVacationAmountValue.Value.ToString();
            TotalAmountValue.ToolTip = OneDayVacationAmountValue.Value.ToString() + " * " + DayCountText.Text + " = " + TotalAmountValue.Value.ToString();
        }
    }
}