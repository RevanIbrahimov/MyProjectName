using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class PenaltyDAL
    {
        public static DataSet SelectContractPenaltyByContractID(int contractID, bool temp)
        {
            string sql = null;
            if (temp)
                sql = $@"SELECT ID,
                                CONTRACT_ID,
                                CALC_DATE,
                                INTEREST,                                
                                IS_DEFAULT,
                                NOTE
                          FROM CRS_USER_TEMP.INTEREST_PENALTIES_TEMP
                               WHERE IS_CHANGE IN(0, 1) AND CONTRACT_ID = {contractID}
                            ORDER BY ID, CALC_DATE";
            else
                sql = $@"SELECT ID,
                                CONTRACT_ID,
                                CALC_DATE,
                                INTEREST,                                
                                IS_DEFAULT,
                                NOTE
                          FROM CRS_USER.INTEREST_PENALTIES
                               WHERE CONTRACT_ID = {contractID}
                            ORDER BY ID, CALC_DATE";

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
                GlobalProcedures.LogWrite("Müqavilənin cərimə faizləri açılmadı.", sql, GlobalVariables.V_UserName, "PenaltyDAL", "SelectContractPenaltyByContractID", exx);
            }
        }
    }
}
