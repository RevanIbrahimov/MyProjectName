using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class EyebrowDAL
    {
        public static DataSet SelectEyebrowByID(int? ID)
        {
            string sql = null;
            if (ID == null)
                sql = "SELECT ID,NAME,IS_DEDUCTION,NOTE FROM CRS_USER.EYEBROWS_TYPE ORDER BY NAME";
            else
                sql = $@"SELECT ID,NAME,IS_DEDUCTION,NOTE FROM CRS_USER.EYEBROWS_TYPE WHERE ID = {ID}";

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
                GlobalProcedures.LogWrite("Qaşın növü açılmadı.", sql, GlobalVariables.V_UserName, "EyebrowDAL", "SelectEyebrowByID", exx);
            }
        }
    }
}
