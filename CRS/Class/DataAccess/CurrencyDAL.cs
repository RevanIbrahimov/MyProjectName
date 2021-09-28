using CRS.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CurrencyDAL
    {
        public static DataSet SelectCurrencyByID(int? currencyID)
        {
            string sql = null;
            if (currencyID == null)
                sql = "SELECT ID,CODE,VALUE,NAME,SHORT_NAME,SMALL_NAME,NOTE FROM CRS_USER.CURRENCY ORDER BY ORDER_ID";
            else
                sql = "SELECT ID,CODE,VALUE,NAME,SHORT_NAME,SMALL_NAME,NOTE FROM CRS_USER.CURRENCY WHERE ID = " + currencyID;

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
                GlobalProcedures.LogWrite("Valyuta açılmadı.", sql, GlobalVariables.V_UserName, "CurrencyDAL", "SelectCurrency", exx);
            }
        }

        public static DataSet SelectCurrencyByCode(string currencyCode)
        {
            string sql = null;
            if (currencyCode != null)                
                sql = "SELECT ID,CODE,VALUE,NAME,SHORT_NAME,SMALL_NAME,NOTE FROM CRS_USER.CURRENCY WHERE CODE = '" + currencyCode + "'";
            try
            {
                using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
                {
                    DataSet dsAdapter = new DataSet();
                    adapter.Fill(dsAdapter);
                    return dsAdapter;
                }
            }
            catch(Exception exx)
            {
                return null;
                GlobalProcedures.LogWrite("Valyuta açılmadı.", sql, GlobalVariables.V_UserName, "CurrencyDAL", "SelectCurrency", exx);
            }
        }
    }
}
