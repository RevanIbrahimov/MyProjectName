using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class AcceptorBankDAL
    {
        public static DataSet SelectBankByID(int? bankID)
        {
            string sql = null;
            if (bankID == null)
                sql = "SELECT ID,NAME,CODE,SWIFT,VOEN,CBAR_ACCOUNT,USED_USER_ID,ACCEPTOR_ACCOUNT FROM CRS_USER.TASK_ACCEPTOR_BANKS ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,NAME,CODE,SWIFT,VOEN,CBAR_ACCOUNT,USED_USER_ID,ACCEPTOR_ACCOUNT FROM CRS_USER.TASK_ACCEPTOR_BANKS WHERE ID = {bankID}";

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
                GlobalProcedures.LogWrite("Bank açılmadı.", sql, GlobalVariables.V_UserName, "AcceptorBankDAL", "SelectBankByID", exx);
            }
        }
    }
}
