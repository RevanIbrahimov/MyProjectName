using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class LeasingTotalDAL
    {
        public static DataSet SelectLeasingTotal(int contractID)
        {
            string sql = null;

            sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   PAYMENT_AMOUNT,
                                   BASIC_AMOUNT,
                                   DEBT,
                                   DAY_COUNT,
                                   INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_DEBT,
                                   TOTAL,
                                   REQUIRED,
                                   DELAYS,
                                   NORM_DEBT
                              FROM CRS_USER.LEASING_TOTAL
                             WHERE CONTRACT_ID = {contractID}";

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
                GlobalProcedures.LogWrite("Ödənişlər açılmadı.", sql, GlobalVariables.V_UserName, "LeasingTotalDAL", "SelectLeasingTotal", exx);
                return null;
            }
        }
    }
}
