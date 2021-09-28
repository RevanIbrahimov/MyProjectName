using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class PhonesViewDAL
    {
        public static DataSet SelectPhone(int? ownerID, string ownerTYPE)
        {
            string sql = null;

            if (ownerID != null)
                sql = $@"SELECT DISTINCT OWNER_ID, OWNER_TYPE, PHONE
                              FROM (SELECT * FROM CRS_USER.V_PHONE
                                    UNION ALL
                                    SELECT * FROM CRS_USER_TEMP.V_PHONE_TEMP)
                             WHERE OWNER_TYPE = '{ownerTYPE}' AND OWNER_ID = {ownerID}";
            else
                sql = $@"SELECT DISTINCT OWNER_ID, OWNER_TYPE, PHONE
                              FROM (SELECT * FROM CRS_USER.V_PHONE
                                    UNION ALL
                                    SELECT * FROM CRS_USER_TEMP.V_PHONE_TEMP)
                             WHERE OWNER_TYPE = '{ownerTYPE}'";

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
                GlobalProcedures.LogWrite("Telefonun nömrələri açılmadı.", sql, GlobalVariables.V_UserName, "PhonesViewDAL", "SelectPhone", exx);
                return null;
            }
        }

    }
}
