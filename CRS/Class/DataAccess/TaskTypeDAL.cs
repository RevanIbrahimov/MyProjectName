using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class TaskTypeDAL
    {
        public static DataSet SelectTypeByID(int? typeID)
        {
            string sql = null;
            if (typeID == null)
                sql = "SELECT ID,TYPE_NAME,CODE,ORDER_ID,USED_USER_ID,IS_INTERNAL FROM CRS_USER.TASK_TYPE ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,TYPE_NAME,CODE,ORDER_ID,USED_USER_ID,IS_INTERNAL FROM CRS_USER.TASK_TYPE WHERE ID = {typeID}";

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
                GlobalProcedures.LogWrite("Tapşırığın növü açılmadı.", sql, GlobalVariables.V_UserName, "TaskTypeDAL", "SelectTypeByID", exx);
            }
        }

        public static DataSet SelectTypeByName(string typeName)
        {
            string sql = $@"SELECT ID,TYPE_NAME,CODE,ORDER_ID,USED_USER_ID,IS_INTERNAL FROM CRS_USER.TASK_TYPE WHERE TYPE_NAME = '{typeName}'"; ;

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
                GlobalProcedures.LogWrite("Tapşırığın növü açılmadı.", sql, GlobalVariables.V_UserName, "TaskTypeDAL", "SelectTypeByName", exx);
            }
        }
    }
}
