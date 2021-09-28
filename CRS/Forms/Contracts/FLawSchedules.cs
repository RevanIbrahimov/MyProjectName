using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraScheduler;
using DevExpress.XtraPrinting;
using DevExpress.XtraScheduler.Drawing;
using DevExpress.XtraScheduler.Printing;

namespace CRS.Forms.Contracts
{
    public partial class FLawSchedules : DevExpress.XtraEditors.XtraForm
    {
        public FLawSchedules()
        {
            InitializeComponent();
        }

        private void FLawSchedules_Load(object sender, EventArgs e)
        {
            LoadSchedule();
            SchedulerControl.ActiveViewType = SchedulerViewType.Month;
            SchedulerControl.GroupType = SchedulerGroupType.None;
            SchedulerControl.ActivePrintStyle.AppointmentFont = new Font("Arial", 6, FontStyle.Regular);

            SchedulerControl.BeginUpdate();
            SchedulerControl.WorkDays.Clear();
            SchedulerControl.WorkDays.Add(WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday | WeekDays.Thursday | WeekDays.Friday | WeekDays.Saturday | WeekDays.Sunday);
            SchedulerControl.EndUpdate();

            
        }

        private void LoadSchedule()
        {
            string selectAppointments = @"SELECT NEXT_DATE STARTDATE,
                                                   NULL ENDDATE,
                                                   D.DAY_NUM_OF_WEEK LABEL,
                                                   CL.DEFANDANT_NAME SUBJECT,
                                                   L.NAME||' - '||LS.NAME LOCATION,
                                                   CL.LAW_ID,
                                                   CL.DEFANDANT_NAME || ' (' || LS.NAME || ')' DESCRIPTION
                                              FROM CRS_USER.CONTRACT_LAWS CL,
                                                   CRS_USER.CONTRACTS C,
                                                   CRS_USER.CREDIT_TYPE CT,
                                                   CRS_USER.DIM_TIME D,
                                                   CRS_USER.LAWS L,
                                                   CRS_USER.LAW_STATUS LS
                                             WHERE CL.CONTRACT_ID = C.ID
                                                   AND C.CREDIT_TYPE_ID = CT.ID
                                                   AND TO_CHAR (CL.NEXT_DATE, 'YYYYMMDD') =
                                                          TO_CHAR(D.CALENDAR_DATE, 'YYYYMMDD')
                                                   AND CL.LAW_ID = L.ID
                                                   AND CL.IS_ACTIVE = 1
                                                   AND CL.LAW_STATUS_ID = LS.ID";
            string selectResources = "SELECT ID,NAME FROM CRS_USER.LAWS";
            try
            {
                

                DataTable generalTable = Class.GlobalFunctions.GenerateDataTable(selectAppointments);

                DataTable dt = Class.GlobalFunctions.GenerateDataTable(selectResources);

                SchedulerStorage.Appointments.DataSource = generalTable;
                SchedulerStorage.Appointments.Mappings.Description = generalTable.Columns[6].ToString();
                SchedulerStorage.Appointments.Mappings.End = generalTable.Columns[1].ToString();
                SchedulerStorage.Appointments.Mappings.Label = generalTable.Columns[2].ToString();
                SchedulerStorage.Appointments.Mappings.Location = generalTable.Columns[4].ToString();
                SchedulerStorage.Appointments.Mappings.Start = generalTable.Columns[0].ToString();
                SchedulerStorage.Appointments.Mappings.Subject = generalTable.Columns[3].ToString();
                SchedulerStorage.Appointments.Mappings.ResourceId = generalTable.Columns[5].ToString();

                SchedulerStorage.Resources.DataSource = dt;
                SchedulerStorage.Resources.Mappings.Id = dt.Columns[0].ToString();
                SchedulerStorage.Resources.Mappings.Caption = dt.Columns[1].ToString();

                SchedulerControl.GroupType = SchedulerGroupType.Resource;
                SchedulerControl.DayView.ResourcesPerPage = 3;
            }
            catch (Exception exx)
            {
                Class.GlobalProcedures.LogWrite("Məhkəmələrin vaxtı yüklənmədi.", "selectAppointments = " + selectAppointments + "\r\nselectResources = " + selectResources, Class.GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void printItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            link.Component = SchedulerControl;
            link.Landscape = true;
            link.ShowPreview();
        }
    }
}