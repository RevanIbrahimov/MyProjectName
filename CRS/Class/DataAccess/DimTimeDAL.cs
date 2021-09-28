using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class DimTimeDAL
    {
        public static DataSet SelectDimTime(int? year, int? month, int? dayID)
        {
            string sql = null, where = null;

            if (year != null)
                where = $@" AND YEAR_ID = {year}";

            if(month != null)
                where = where + $@" AND MONTH_NUM_OF_YEAR = {month}";

            if (dayID != null)
                where = where + $@" AND DAY_ID = {dayID}";

            sql = $@"SELECT DAY_ID DayID,
                           CALENDAR_DATE CalendarDate,
                           YEAR_ID YearID,
                           YEAR_BEGIN_DATE YearBeginDate,
                           YEAR_END_DATE YearEndDate,
                           QUARTER_NUM_OF_YEAR QuarterNumOfYear,
                           QUARTER_OF_YEAR_NAME QuarterNameEn,
                           AZ_QUARTER_OF_YEAR_NAME QuarterNameAz,
                           QUARTER_BEGIN_DATE QuarterBeginDate,
                           QUARTER_END_DATE QuarterEndDate,
                           MONTH_NUM_OF_YEAR MonthNumOfYear,
                           MONTH_NAME MonthNameEn,
                           AZ_MONTH_NAME MonthNameAz,
                           MONTH_BEGIN_DATE MonthBeginDate,
                           MONTH_END_DATE MonthEndDate,
                           DAY_NUM_OF_MONTH DayNumOfMonth,
                           DAY_NAME DayNameEn,
                           DAY_NAME_AZ DayNameAz,
                           IS_LAST_DAY_OF_YEAR IsLastDayOfYear,
                           IS_LAST_DAY_OF_QUARTER IsLastDayOfQuarter,
                           IS_LAST_DAY_OF_MONTH IsLastDayOfMonth
                      FROM CRS_USER.DIM_TIME
                           WHERE YEAR_ID > 2000" + where;

            try
            {
                using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
                {
                    DataSet dsAdapter = new DataSet();
                    adapter.Fill(dsAdapter);
                    return dsAdapter;
                }
            }
            catch (Exception exx)
            {
                return null;
                GlobalProcedures.LogWrite("Tarixlər açılmadı.", sql, GlobalVariables.V_UserName, "DimTimeDAL", "SelectDimTime", exx);
            }
        }
    }
}
