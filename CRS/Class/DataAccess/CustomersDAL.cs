using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CustomersDAL
    {
        public static DataSet SelectCustomer(int? customerID)
        {
            string sql = null;
            if (customerID == null)
                sql = $@"SELECT ID,
                           CODE,
                           TO_NUMBER(CODE) CODE_VALUE,
                           SURNAME,
                           NAME,
                           PATRONYMIC,
                           SEX_ID,
                           BIRTHPLACE_ID,
                           BIRTHDAY,
                           NOTE,
                           USED_USER_ID
                      FROM CRS_USER.CUSTOMERS
                    ORDER BY ID";
            else
                sql = $@"SELECT ID,
                           CODE,
                           TO_NUMBER(CODE) CODE_VALUE,
                           SURNAME,
                           NAME,
                           PATRONYMIC,
                           SEX_ID,
                           BIRTHPLACE_ID,
                           BIRTHDAY,
                           NOTE,
                           USED_USER_ID
                      FROM CRS_USER.CUSTOMERS WHERE ID = {customerID}";

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
                GlobalProcedures.LogWrite("Fiziki şəxs açılmadı.", sql, GlobalVariables.V_UserName, "CustomersDAL", "SelectCustomer", exx);
            }
        }
    }
}
