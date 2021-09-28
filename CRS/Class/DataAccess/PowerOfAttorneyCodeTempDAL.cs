using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class PowerOfAttorneyCodeTempDAL
    {
        public static DataSet SelectPowerOfAttorneyCode()
        {
            string sql = $@"SELECT USED_USER_ID,
                               CONTRACT_ID,
                               POWER_ID,                               
                               CODE_NUMBER,
                               CODE
                          FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_CODE_TEMP
                    ORDER BY CODE_NUMBER";            

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
                GlobalProcedures.LogWrite("Etibarnamənin nömrəsi açılmadı.", sql, GlobalVariables.V_UserName, "TaskCodeTempDAL", "SelectTaskCode", exx);
            }
        }
    }
}
