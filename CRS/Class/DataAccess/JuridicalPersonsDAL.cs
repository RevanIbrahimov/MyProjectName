using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRS.Class;

namespace CRS.Class.DataAccess
{
    public class JuridicalPersonsDAL
    {
        public static DataSet SelectJuridicalPerson(int? customerID)
        {
            string sql = null;
            if (customerID == null)
                sql = $@"SELECT ID,
                           CODE,                           
                           NAME,
                           LEADING_NAME,
                           ADDRESS,
                           VOEN,                          
                           NOTE,
                           USED_USER_ID
                      FROM CRS_USER.JURIDICAL_PERSONS
                    ORDER BY ID";
            else
                sql = $@"SELECT ID,
                           CODE,                           
                           NAME,
                           LEADING_NAME,
                           ADDRESS,
                           VOEN,                          
                           NOTE,
                           USED_USER_ID
                      FROM CRS_USER.JURIDICAL_PERSONS WHERE ID = {customerID}";

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
                GlobalProcedures.LogWrite("Hüquqi şəxs açılmadı.", sql, GlobalVariables.V_UserName, "JuridicalPersonsDAL", "SelectJuridicalPerson", exx);
            }
        }
    }
}
