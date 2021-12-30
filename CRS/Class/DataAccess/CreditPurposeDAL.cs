using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   CRS.Class.DataAccess
{
    public class CreditPurposeDAL
    {
        public static DataSet SelectCreditPurposeByID(int? typeID)
        {
            string sql = null;
            if (typeID == null)
                sql = "SELECT ID,NAME,CODE FROM CRS_USER.CREDIT_PURPOSE ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,NAME,CODE FROM CRS_USER.CREDIT_PURPOSE WHERE ID = {typeID}";

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
                GlobalProcedures.LogWrite("Əyyarlar açılmadı.", sql, GlobalVariables.V_UserName, "CreditPurposeDAL", "SelectCreditPurposeByID", exx);
            }
        }
    }
}
