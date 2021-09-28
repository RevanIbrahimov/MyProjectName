using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class DimTime
    {
        public int DayID { get; set; }
        public DateTime CalendarDate { get; set; }
        public int YearID { get; set; }
        public DateTime YearBeginDate { get; set; }
        public DateTime YearEndDate { get; set; }
        public int QuarterNumOfYear { get; set; }
        public string QuarterNameEn { get; set; }
        public string QuarterNameAz { get; set; }        
        public DateTime QuarterBeginDate { get; set; }
        public DateTime QuarterEndDate { get; set; }        
        public int MonthNumOfYear { get; set; }
        public string MonthNameEn { get; set; }
        public string MonthNameAz { get; set; }
        public DateTime MonthBeginDate { get; set; }
        public DateTime MonthEndDate { get; set; }
        public int DayNumOfMonth { get; set; }
        public string DayNameEn { get; set; }
        public string DayNameAz { get; set; }
        public string IsLastDayOfYear { get; set; }
        public string IsLastDayOfQuarter { get; set; }
        public string IsLastDayOfMonth { get; set; }
    }
}
