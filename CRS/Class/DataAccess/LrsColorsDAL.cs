using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class LrsColorsDAL
    {
        public static DataSet SelectColor(int? userID)
        {
            string sql = null;
            if (userID == null)
                sql = $@"SELECT ID,
                                 USER_ID,
                                 COLOR_TYPE_ID,
                                 COLOR_VALUE_1,
                                 COLOR_VALUE_2,
                                 USED_USER_ID
                            FROM CRS_USER.LRS_COLORS
                        ORDER BY COLOR_TYPE_ID";
            else
                sql = $@"SELECT ID,
                                 USER_ID,
                                 COLOR_TYPE_ID,
                                 COLOR_VALUE_1,
                                 COLOR_VALUE_2,
                                 USED_USER_ID
                            FROM CRS_USER.LRS_COLORS
                           WHERE USER_ID = {userID}
                        ORDER BY COLOR_TYPE_ID";

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
                GlobalProcedures.LogWrite("Rənglər açılmadı.", sql, GlobalVariables.V_UserName, "LrsColorsDAL", "SelectColor", exx);
            }
        }
    }
}
