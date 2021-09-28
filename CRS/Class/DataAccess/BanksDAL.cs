using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class BanksDAL
    {
        public static DataSet SelectBankByID(int? bankID)
        {
            string sql = null;
            if (bankID == null)
                sql = "SELECT ID,LONG_NAME,SHORT_NAME,CODE,SWIFT,VOEN,ACCOUNT,CBAR_ACCOUNT,USED_USER_ID,IS_USED FROM CRS_USER.BANKS ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,LONG_NAME,SHORT_NAME,CODE,SWIFT,VOEN,ACCOUNT,CBAR_ACCOUNT,USED_USER_ID,IS_USED FROM CRS_USER.BANKS WHERE ID = {bankID}";

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
                GlobalProcedures.LogWrite("Bank açılmadı.", sql, GlobalVariables.V_UserName, "BanksDAL", "SelectBankByID", exx);
            }
        }

        public static DataSet SelectBankByName(string bankName)
        {
            string sql = $@"SELECT ID,LONG_NAME,SHORT_NAME,CODE,SWIFT,VOEN,ACCOUNT,CBAR_ACCOUNT,USED_USER_ID,IS_USED FROM CRS_USER.BANKS WHERE LONG_NAME = '{bankName}'";

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
                GlobalProcedures.LogWrite("Bank açılmadı.", sql, GlobalVariables.V_UserName, "BanksDAL", "SelectBankByName", exx);
            }
        }
    }
}
