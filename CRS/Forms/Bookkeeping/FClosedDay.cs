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
    public partial class FClosedDay : DevExpress.XtraEditors.XtraForm
    {
        public FClosedDay()
        {
            InitializeComponent();
        }

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FClosedDay_Load(object sender, EventArgs e)
        {
            string sql = $@"SELECT MAX(CLOSED_DAY) CLOSED_DAY FROM CRS_USER.CLOSED_DAYS";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/FClosedDay_Load", "Günün bağlanılması açılmadı.");
            DateTime lastClosedDay = DateTime.Today;
            int month, day;
            if(dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CLOSED_DAY"].ToString() != "")
                    lastClosedDay = Convert.ToDateTime(dt.Rows[0]["CLOSED_DAY"]);

                month = lastClosedDay.AddDays(1).Month;
                day = lastClosedDay.AddDays(1).Day;

                if(month != 2 && day == 31)
                    LastDayValue.EditValue = lastClosedDay.AddDays(2);
                else
                    LastDayValue.EditValue = lastClosedDay.AddDays(1);
            }

            ClosedDayValue.Properties.MinValue = LastDayValue.DateTime.AddDays(1);
            ClosedDayValue.EditValue = ClosedDayValue.Properties.MinValue < DateTime.Today? DateTime.Today : ClosedDayValue.Properties.MinValue;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(LastDayValue.DateTime == null)
            {                
                return;
            }
            GlobalProcedures.ClosedDay(LastDayValue.DateTime, ClosedDayValue.DateTime);
            this.Close();
        }

        private void FClosedDay_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }
    }
}