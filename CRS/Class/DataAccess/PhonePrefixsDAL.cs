using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRS.Class.DataAccess
{
    public class PhonePrefixsDAL
    {
        public static DataSet SelectPrefixByID(int? prefixID)
        {
            string sql = null;
            if (prefixID == null)
                sql = "SELECT ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE,ETL_DT_TM FROM CRS_USER.PHONE_PREFIXS ORDER BY ID";
            else
                sql = $@"SELECT ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE,ETL_DT_TM FROM CRS_USER.PHONE_PREFIXS WHERE ID = {prefixID}";

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
                GlobalProcedures.LogWrite("Telefonun təsviri açılmadı.", sql, GlobalVariables.V_UserName, "PhonePrefixsDAL", "SelectPrefixByID", exx);
            }
        }

        public static DataSet SelectPrefixByDescription(int descriptionID)
        {
            string sql = $@"SELECT ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE,ETL_DT_TM FROM CRS_USER.PHONE_PREFIXS WHERE PHONE_DESCRIPTION_ID = '{descriptionID}'";

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
                GlobalProcedures.LogWrite("Telefonun təsviri açılmadı.", sql, GlobalVariables.V_UserName, "PhonePrefixsDAL", "SelectPrefixByDescription", exx);
            }
        }

        public static DataSet SelectPrefix(string prefix)
        {
            string sql = $@"SELECT ID,PHONE_DESCRIPTION_ID,PREFIX,NOTE,ETL_DT_TM FROM CRS_USER.PHONE_PREFIXS WHERE PREFIX = '{prefix}'";

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
                GlobalProcedures.LogWrite("Telefonun təsviri açılmadı.", sql, GlobalVariables.V_UserName, "PhonePrefixsDAL", "SelectPrefix", exx);
            }
        }
    }
}
