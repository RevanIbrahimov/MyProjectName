using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CustomerCodeTempDAL
    {
        public static DataSet SelectCustomerCode(int? customerID)
        {
            string sql = null;
            if (customerID == null)
                sql = $@"SELECT USED_USER_ID,
                               CUSTOMER_ID,
                               CODE_NUMBER,
                               CODE
                          FROM CRS_USER_TEMP.CUSTOMER_CODE_TEMP
                    ORDER BY CODE_NUMBER";
            else
                sql = $@"SELECT USED_USER_ID,
                               CUSTOMER_ID,
                               CODE_NUMBER,
                               CODE
                          FROM CRS_USER_TEMP.CUSTOMER_CODE_TEMP WHERE CUSTOMER_ID = {customerID}";

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
                GlobalProcedures.LogWrite("Müştərinin nömrəsi açılmadı.", sql, GlobalVariables.V_UserName, "CustomerCodeTempDAL", "SelectCustomerCode", exx);
            }
        }        
    }
}
