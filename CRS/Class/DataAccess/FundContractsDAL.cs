using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class FundContractsDAL
    {
        public static DataSet SelectFundContractByID(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = $@"SELECT ID,
                                   FUNDS_SOURCE_ID,
                                   FUNDS_SOURCE_NAME_ID,
                                   CONTRACT_NUMBER,
                                   REGISTRATION_NUMBER,
                                   INTEREST,
                                   PERIOD,
                                   START_DATE,
                                   END_DATE,
                                   AMOUNT,
                                   CURRENCY_ID,
                                   STATUS_ID,
                                   NOTE,
                                   CHECK_END_DATE,
                                   CLOSED_DATE,
                                   USED_USER_ID
                              FROM CRS_USER.FUNDS_CONTRACTS
                         ORDER BY ID";
            else
                sql = $@"SELECT ID,
                                   FUNDS_SOURCE_ID,
                                   FUNDS_SOURCE_NAME_ID,
                                   CONTRACT_NUMBER,
                                   REGISTRATION_NUMBER,
                                   INTEREST,
                                   PERIOD,
                                   START_DATE,
                                   END_DATE,
                                   AMOUNT,
                                   CURRENCY_ID,
                                   STATUS_ID,
                                   NOTE,
                                   CHECK_END_DATE,
                                   CLOSED_DATE,
                                   USED_USER_ID
                              FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = {contractID}";

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
                GlobalProcedures.LogWrite("Müqavilə açılmadı.", sql, GlobalVariables.V_UserName, "FundContractsDAL", "SelectFundContractByID", exx);
            }
        }
    }
}
