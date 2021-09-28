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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraGrid;

namespace CRS.Forms.Bookkeeping
{
    public partial class FPersonnelVacation : DevExpress.XtraEditors.XtraForm
    {
        public FPersonnelVacation()
        {
            InitializeComponent();
        }
        public int PersonnelID, VacationDateID, DayCount;
        public string Personnel, Position, Period;
        public DateTime StartDate, EndDate;

        int topindex, old_row_num, calc_daycount = 0, vacationID;
        
        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadVacation();
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(VacationGridControl);
        }

        private void VacationGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void ExcelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(VacationGridControl, "xls");
        }

        private void VacationGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(VacationGridView, PopupMenu, e);
        }

        private void VacationGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditBarButton.Enabled = DeleteBarButton.Enabled = (VacationGridView.RowCount > 0);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPersonnelVacationAddEdit("EDIT", vacationID);
        }

        private void VacationGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFPersonnelVacationAddEdit("EDIT", vacationID);
        }

        private void VacationGridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (currentView.RowCount == 0)
            {
                calc_daycount = 0;
                return;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Start)
                calc_daycount = 0;

            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DAY_COUNT") == 0)
                    calc_daycount += Convert.ToInt32(e.FieldValue);
            }

            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((GridSummaryItem)e.Item).FieldName.CompareTo("DAY_COUNT") == 0)            
                    e.TotalValue = calc_daycount;
            }            
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş məzuniyyəti silmək istəyirsiniz?", "Məzuniyyətin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PERSONNEL_VACATIONS_TEMP SET IS_CHANGE = 2 WHERE ID = {vacationID}",
                                                 "Məzuniyyət silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
            }
            LoadVacation();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_PERSONNEL_VACATION", "P_VACATION_DATES_ID", VacationDateID, "Məzuniyyətlər əsas cədvələ daxil edilmədi.");
            this.RefreshDataGridView();
            this.Close();
        }

        private void VacationGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = (sender as GridView).GetFocusedDataRow();
            if (row != null)
                vacationID = Convert.ToInt32(row["ID"]);
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPersonnelVacationAddEdit("INSERT", null);
        }

        private void LoadFPersonnelVacationAddEdit(string transaction, int? id)
        {
            topindex = VacationGridView.TopRowIndex;
            old_row_num = VacationGridView.FocusedRowHandle;
            FPersonnelVacationAddEdit fv = new FPersonnelVacationAddEdit();
            fv.TransactionName = transaction;
            fv.ID = id;
            fv.StartDate = StartDate;
            fv.EndDate = EndDate;
            fv.PersonnelID = PersonnelID;
            fv.VacationDateID = VacationDateID;
            fv.DayCount = (int)DayCountValue.Value - calc_daycount;
            fv.RefreshDataGridView += new FPersonnelVacationAddEdit.DoEvent(LoadVacation);
            fv.ShowDialog();
            VacationGridView.TopRowIndex = topindex;
            VacationGridView.FocusedRowHandle = old_row_num;
        }

        private void VacationGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
            GlobalProcedures.GridCustomDrawFooterCell("DAY_COUNT", "Center", e);
        }

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FPersonnelVacation_Load(object sender, EventArgs e)
        {
            PersonnelNameText.Text = Personnel;
            PositionText.Text = Position;
            PeriodText.Text = Period;
            DayCountValue.EditValue = DayCount;
            InsertTemp();
            LoadVacation();
        }

        private void InsertTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_PERSONNEL_VACATION_TEMP", "P_VACATION_DATES_ID", VacationDateID, "Məzuniyyətlər temp cədvələ daxil olmadı.");
        }

        private void LoadVacation()
        {
            string sql = $@"SELECT 1 SS,
                                   ID,
                                   START_DATE,
                                   END_DATE,
                                   DAY_COUNT,
                                   SALARY_AVERAGE,
                                   ONE_DAY_VACATION_AMOUNT,
                                   TOTAL_AMOUNT
                              FROM CRS_USER_TEMP.PERSONNEL_VACATIONS_TEMP
                             WHERE IS_CHANGE != 2 AND PERSONNEL_VACATION_DATE_ID = {VacationDateID}";

            VacationGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadVacation", "Məzuniyyətlərin siyahısı açılmadı.");

            EditBarButton.Enabled = DeleteBarButton.Enabled = (VacationGridView.RowCount > 0);

            NewBarButton.Enabled = (((int)DayCountValue.Value - calc_daycount) > 0);

            DebtDayCountValue.Value = DayCountValue.Value - calc_daycount;
        }
    }
}