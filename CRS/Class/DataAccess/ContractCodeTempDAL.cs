using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class ContractCodeTempDAL
    {
        public static DataSet SelectContractCode(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = $@"SELECT USED_USER_ID,
                               CONTRACT_ID,
                               CODE_NUMBER,
                               CODE
                          FROM CRS_USER_TEMP.CONTRACT_CODE_TEMP
                    ORDER BY CODE_NUMBER";
            else
                sql = $@"SELECT USED_USER_ID,
                               CONTRACT_ID,
                               CODE_NUMBER,
                               CODE
                          FROM CRS_USER_TEMP.CONTRACT_CODE_TEMP WHERE CONTRACT_ID = {contractID}";

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
                GlobalProcedures.LogWrite("Müqavilənin nömrəsi açılmadı.", sql, GlobalVariables.V_UserName, "ContractCodeTempDAL", "SelectContractCode", exx);
            }
        }
    }
}
