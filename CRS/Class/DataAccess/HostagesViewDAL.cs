using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class HostagesViewDAL
    {
        public static DataSet SelectHostage(int? contractID)
        {
            string sql = null;

            if (contractID != null)
                sql = $@"SELECT * FROM CRS_USER.V_HOSTAGE WHERE CONTRACT_ID = {contractID}";
            else
                sql = $@"SELECT * FROM CRS_USER.V_HOSTAGE";

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
                GlobalProcedures.LogWrite("Lizinqin predmeti açılmadı.", sql, GlobalVariables.V_UserName, "HostagesViewDAL", "SelectHostage", exx);
                return null;
            }
        }
    }
}
