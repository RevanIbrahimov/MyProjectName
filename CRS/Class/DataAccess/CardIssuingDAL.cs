using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CardIssuingDAL
    {
        public static DataSet SelectIssuingByID(int? issuingID)
        {
            string sql = null;
            if (issuingID == null)
                sql = "SELECT ID,NAME,NOTE,ORDER_ID,USED_USER_ID,ETL_DT_TM FROM CRS_USER.CARD_ISSUING ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,NAME,NOTE,ORDER_ID,USED_USER_ID,ETL_DT_TM FROM CRS_USER.CARD_ISSUING WHERE ID = {issuingID}";

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
                GlobalProcedures.LogWrite("Sənədi verən orqanlar açılmadı.", sql, GlobalVariables.V_UserName, "CardIssuingDAL", "SelectIssuingByID", exx);
            }
        }
    }
}
