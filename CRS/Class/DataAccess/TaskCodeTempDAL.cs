using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class TaskCodeTempDAL
    {
        public static DataSet SelectTaskCode(int? typeID)
        {
            string sql = null;
            if (typeID == null)
                sql = $@"SELECT USED_USER_ID,
                               TASK_ID,
                               TASK_TYPE_ID,
                               TASK_NUMBER,
                               CODE
                          FROM CRS_USER_TEMP.TASK_CODE_TEMP
                    ORDER BY TASK_NUMBER";
            else
                sql = $@"SELECT USED_USER_ID,
                               TASK_ID,
                               TASK_TYPE_ID,
                               TASK_NUMBER,
                               CODE
                          FROM CRS_USER_TEMP.TASK_CODE_TEMP WHERE TASK_TYPE_ID = {typeID}";

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
                GlobalProcedures.LogWrite("Tapşırığın nömrəsi açılmadı.", sql, GlobalVariables.V_UserName, "TaskCodeTempDAL", "SelectTaskCode", exx);
            }
        }
    }
}
