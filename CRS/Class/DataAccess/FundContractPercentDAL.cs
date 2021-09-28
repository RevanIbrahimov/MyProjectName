using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class FundContractPercentDAL
    {
        public static DataSet SelectFundContractPercentByContractID(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = $@"SELECT ID,
                                   FUNDS_CONTRACTS_ID,
                                   PDATE,
                                   PERCENT_VALUE,
                                   NOTE
                              FROM CRS_USER.FUNDS_CONTRACTS_PERCENTS
                         ORDER BY ID";
            else
                sql = $@"SELECT ID,
                                   FUNDS_CONTRACTS_ID,
                                   PDATE,
                                   PERCENT_VALUE,
                                   NOTE
                              FROM CRS_USER.FUNDS_CONTRACTS_PERCENTS WHERE FUNDS_CONTRACTS_ID = {contractID} ORDER BY ID";

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
                GlobalProcedures.LogWrite("Müqavilənin illik faizləri açılmadı.", sql, GlobalVariables.V_UserName, "FundContractPercentDAL", "SelectFundContractPercentByContractID", exx);
            }
        }

        public static DataSet SelectFundContractPercentTempByContractID(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = $@"SELECT ID,
                                   FUNDS_CONTRACTS_ID,
                                   PDATE,
                                   PERCENT_VALUE,
                                   NOTE
                              FROM CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP
                              WHERE IS_CHANGE != 2
                         ORDER BY ID";
            else
                sql = $@"SELECT ID,
                                   FUNDS_CONTRACTS_ID,
                                   PDATE,
                                   PERCENT_VALUE,
                                   NOTE
                              FROM CRS_USER_TEMP.FUNDS_CONTRACTS_PERCENTS_TEMP WHERE IS_CHANGE != 2 AND FUNDS_CONTRACTS_ID = {contractID}";

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
                GlobalProcedures.LogWrite("Müqavilənin illik faizləri açılmadı.", sql, GlobalVariables.V_UserName, "FundContractPercentDAL", "SelectFundContractPercentByContractID", exx);
            }
        }
    }
}
