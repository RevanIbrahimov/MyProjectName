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

namespace CRS.Forms.Bookkeeping
{
    public partial class FPersonneıVacationDates : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FPersonneıVacationDates()
        {
            InitializeComponent();
        }
        int personnelID, dateID, dayCount, topindex, old_row_num;
        string personnel, position, period;
        DateTime startDate, endDate;

        private void DatesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = (sender as GridView).GetFocusedDataRow();
            if (row != null)
            {
                personnel = row["PERSONNEL_NAME"].ToString();
                position = row["POSITION_NAME"].ToString();
                period = row["PERIOD"].ToString();
                personnelID = int.Parse(row["PERSONNEL_ID"].ToString());
                dateID = int.Parse(row["ID"].ToString());
                dayCount = int.Parse(row["DAY_COUNT"].ToString());
                startDate = Convert.ToDateTime(row["START_DATE"]);
                endDate = Convert.ToDateTime(row["END_DATE"]);
            }
        }

        private void DatesGridView_DoubleClick(object sender, EventArgs e)
        {
            if (VacationsBarButton.Enabled)
                LoadVacations();
        }

        private void VacationsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadVacations();
        }

        private void LoadVacations()
        {
            topindex = DatesGridView.TopRowIndex;
            old_row_num = DatesGridView.FocusedRowHandle;
            FPersonnelVacation fv = new FPersonnelVacation();
            fv.PersonnelID = personnelID;
            fv.Personnel = personnel;
            fv.VacationDateID = dateID;
            fv.Position = position;
            fv.Period = period;
            fv.DayCount = dayCount;
            fv.StartDate = startDate;
            fv.EndDate = endDate;
            fv.RefreshDataGridView += new FPersonnelVacation.DoEvent(LoadDates);
            fv.ShowDialog();
            DatesGridView.TopRowIndex = topindex;
            DatesGridView.FocusedRowHandle = old_row_num;
        }

        private void FPersonneıVacationDates_Load(object sender, EventArgs e)
        {
            LoadDates();
        }

        private void LoadDates()
        {
            string sql = $@"SELECT 1 SS,
                                     VD.ID,
                                     VD.PERSONNEL_ID,
                                     P.SURNAME || ' ' || P.NAME || ' ' || P.PATRONYMIC PERSONNEL_NAME,
                                     LP.POSITION_NAME,
                                        TO_CHAR (VD.START_DATE, 'DD.MM.YYYY')
                                     || ' - '
                                     || TO_CHAR (VD.END_DATE, 'DD.MM.YYYY')
                                        PERIOD,
                                     VD.DAY_COUNT,
                                     NVL (PV.DAY_COUNT, 0) USED_DAY_COUNT,
                                     VD.DAY_COUNT - NVL (PV.DAY_COUNT, 0) DEBT_DAY_COUNT,
                                     P.STATUS_ID PERSONNEL_STATUS_ID,
                                     VD.START_DATE,
                                     VD.END_DATE
                                FROM CRS_USER.PERSONNEL_VACATION_DATES VD,
                                     CRS_USER.PERSONNEL P,
                                     CRS_USER.V_PERSONNEL_LAST_POSITION LP,
                                     CRS_USER.V_PERSONNEL_VACATION_DAY_COUNT PV
                               WHERE     VD.PERSONNEL_ID = P.ID
                                     AND P.ID = LP.PERSONNEL_ID
                                     AND VD.ID = PV.PERSONNEL_VACATION_DATE_ID(+)
                            ORDER BY P.SURNAME, VD.ID";

            DatesGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDates", "Məzuniyyət tarixləri açılmadı.");

            VacationsBarButton.Enabled = (DatesGridView.RowCount > 0); 
        }

        private void DatesGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void RefreshBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadDates();
        }
    }
}