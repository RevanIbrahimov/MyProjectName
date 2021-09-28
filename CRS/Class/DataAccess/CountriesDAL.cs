using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRS.Class.DataAccess
{
    public class CountriesDAL
    {
        public static DataSet SelectCountryByID(int? countryID)
        {
            string sql = null;
            if (countryID == null)
                sql = "SELECT ID,NAME,NAME_EN,NAME_RU,CODE,ORDER_ID,NOTE,USED_USER_ID,ETL_DT_TM FROM CRS_USER.COUNTRIES ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,NAME,NAME_EN,NAME_RU,CODE,ORDER_ID,NOTE,USED_USER_ID,ETL_DT_TM FROM CRS_USER.COUNTRIES WHERE ID = {countryID}";

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
                GlobalProcedures.LogWrite("Ölkə açılmadı.", sql, GlobalVariables.V_UserName, "CountriesDAL", "SelectCountryByID", exx);
            }
        }

        public static DataSet SelectCountryByCode(int country_code)
        {
            string sql = $@"SELECT ID,NAME,NAME_EN,NAME_RU,CODE,ORDER_ID,NOTE,USED_USER_ID,ETL_DT_TM FROM CRS_USER.COUNTRIES WHERE CODE = {country_code}";

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
                GlobalProcedures.LogWrite("Ölkə açılmadı.", sql, GlobalVariables.V_UserName, "CountriesDAL", "SelectCountryByCode", exx);
            }
        }

        public static DataSet SelectCountryByName(string country_name)
        {
            string sql = $@"SELECT ID,NAME,NAME_EN,NAME_RU,CODE,ORDER_ID,NOTE,USED_USER_ID,ETL_DT_TM FROM CRS_USER.COUNTRIES WHERE NAME = '{country_name}'";

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
                GlobalProcedures.LogWrite("Ölkə açılmadı.", sql, GlobalVariables.V_UserName, "CountriesDAL", "SelectCountryByName", exx);
            }
        }
    }
}
