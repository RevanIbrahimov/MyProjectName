using CRS.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class AcceptorDAL
    {
        public static DataSet SelectAcceptorByID(int? acceptorID)
        {
            string sql = null;
            if (acceptorID == null)
                sql = "SELECT ID,ACCEPTOR_NAME,ACCEPTOR_VOEN,VAT_ACCOUNT,USED_USER_ID FROM CRS_USER.TASK_ACCEPTOR ORDER BY NAME";
            else
                sql = $@"SELECT ID,ACCEPTOR_NAME,ACCEPTOR_VOEN,VAT_ACCOUNT,USED_USER_ID FROM CRS_USER.TASK_ACCEPTOR WHERE ID = {acceptorID}";

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
                GlobalProcedures.LogWrite("Ödənişi alan tərəflər açılmadı.", sql, GlobalVariables.V_UserName, "AcceptorDAL", "SelectAcceptorByID", exx);
            }
        }

        public static DataSet SelectAcceptorByName(string acceptorName)
        {
            string sql = $@"SELECT ID,ACCEPTOR_NAME,ACCEPTOR_VOEN,VAT_ACCOUNT,USED_USER_ID FROM CRS_USER.TASK_ACCEPTOR WHERE NAME = '{acceptorName}'"; ;

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
                GlobalProcedures.LogWrite("Ödənişi alan tərəflər açılmadı.", sql, GlobalVariables.V_UserName, "AcceptorDAL", "SelectAcceptorByName", exx);
            }
        }
    }
}
