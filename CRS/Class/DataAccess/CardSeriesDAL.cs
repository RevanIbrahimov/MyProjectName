using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CardSeriesDAL
    {
        public static DataSet SelectSerieByID(int? serieID)
        {
            string sql = null;
            if (serieID == null)
                sql = "SELECT ID,NAME,SERIES,NOTE,ORDER_ID,USED_USER_ID,ETL_DT_TM FROM CRS_USER.CARD_SERIES ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,NAME,SERIES,NOTE,ORDER_ID,USED_USER_ID,ETL_DT_TM FROM CRS_USER.CARD_SERIES WHERE ID = {serieID}";

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
                GlobalProcedures.LogWrite("Seriya açılmadı.", sql, GlobalVariables.V_UserName, "CardSeriesDAL", "SelectSerieByID", exx);
            }
        }

        public static DataSet SelectSerieByName(string name)
        {
            string sql = $@"SELECT ID,NAME,SERIES,NOTE,ORDER_ID,USED_USER_ID,ETL_DT_TM FROM CRS_USER.CARD_SERIES WHERE NAME = '{name}'";

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
                GlobalProcedures.LogWrite("Seriya açılmadı.", sql, GlobalVariables.V_UserName, "CardSeriesDAL", "SelectSerieByName", exx);
            }
        }

        public static DataSet SelectSerieBySerie(string serieNAME)
        {
            string sql = $@"SELECT ID,NAME,SERIES,NOTE,ORDER_ID,USED_USER_ID,ETL_DT_TM FROM CRS_USER.CARD_SERIES WHERE SERIES = '{serieNAME}'";

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
                GlobalProcedures.LogWrite("Seriya açılmadı.", sql, GlobalVariables.V_UserName, "CardSeriesDAL", "SelectSerieBySerie", exx);
            }
        }
    }
}
