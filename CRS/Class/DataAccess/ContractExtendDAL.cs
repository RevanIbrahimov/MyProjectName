using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class ContractExtendDAL
    {
        public static DataSet SelectContractExtend(int istemp, int contractID)
        {
            string sql = null;

            if (istemp == 1)
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   START_DATE,
                                   END_DATE,
                                   MONTH_COUNT,
                                   INTEREST,
                                   DEBT,
                                   CURRENT_DEBT,
                                   INTEREST_DEBT,
                                   CHECK_INTEREST_DEBT,
                                   MONTHLY_AMOUNT,                                                                      
                                   VERSION,
                                   PAYMENT_TYPE,
                                   NOTE,
                                   IS_CHANGE
                              FROM CRS_USER_TEMP.CONTRACT_EXTEND_TEMP
                             WHERE IS_CHANGE != 2 AND CONTRACT_ID = {contractID} ORDER BY ID";
            else
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   START_DATE,
                                   END_DATE,
                                   MONTH_COUNT,
                                   INTEREST,
                                   DEBT,
                                   CURRENT_DEBT,
                                   INTEREST_DEBT,
                                   CHECK_INTEREST_DEBT,
                                   MONTHLY_AMOUNT,                                   
                                   VERSION,
                                   PAYMENT_TYPE,
                                   NOTE,
                                   0 IS_CHANGE
                              FROM CRS_USER.CONTRACT_EXTEND
                             WHERE CONTRACT_ID = {contractID} ORDER BY ID";

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
                GlobalProcedures.LogWrite("Müqavilənin uzadılma şərtləri açılmadı.", sql, GlobalVariables.V_UserName, "ContractExtendDAL", "SelectContractExtend", exx);
                return null;
            }
        }
    }
}
