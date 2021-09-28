using CRS.Class.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRS.Class.DataAccess
{
    public class CashOperationsDAL
    {
        public static DataSet SelectCashOperation(int? operationID)
        {
            string sql = null;
            if (operationID == null)
                sql = @"SELECT ID,
                             DESTINATION_ID,
                             OPERATION_OWNER_ID,
                             OPERATION_DATE,
                             CONTRACT_CODE,
                             INCOME,
                             EXPENSES,
                             DEBT,
                             IS_COMMIT,
                             USED_USER_ID,
                             ETL_DT_TM
                        FROM CRS_USER.CASH_OPERATIONS
                    ORDER BY OPERATION_DATE, ID";
            else
                sql = $@"SELECT ID,
                             DESTINATION_ID,
                             OPERATION_OWNER_ID,
                             OPERATION_DATE,
                             CONTRACT_CODE,
                             INCOME,
                             EXPENSES,
                             DEBT,
                             IS_COMMIT,
                             USED_USER_ID,
                             ETL_DT_TM
                        FROM CRS_USER.CASH_OPERATIONS WHERE ID = {operationID}";

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
                GlobalProcedures.LogWrite("Kassa əməliyyatı açılmadı.", sql, GlobalVariables.V_UserName, "CashOperationsDAL", "SelectCashOperation", exx);
            }
        }

        public static DataSet SelectCashOperationForDebt(string operationDate)
        {
            string sql = $@"SELECT ID,
                             DESTINATION_ID,
                             OPERATION_OWNER_ID,
                             OPERATION_DATE,
                             CONTRACT_CODE,
                             INCOME,
                             EXPENSES,
                             DEBT,
                             IS_COMMIT,
                             USED_USER_ID,
                             ETL_DT_TM
                        FROM CRS_USER.CASH_OPERATIONS WHERE OPERATION_DATE >= TO_DATE('{operationDate}','DD.MM.YYYY')
                      ORDER BY OPERATION_DATE, ID";

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
                GlobalProcedures.LogWrite("Kassa əməliyyatları açılmadı.", sql, GlobalVariables.V_UserName, "CashOperationsDAL", "SelectCashOperationForDebt", exx);
            }
        }

        public static DataSet SelectCashOperationForDateAgain(string operationDate)
        {
            string sql = $@"SELECT ID,
                             DESTINATION_ID,
                             OPERATION_OWNER_ID,
                             OPERATION_DATE,
                             CONTRACT_CODE,
                             INCOME,
                             EXPENSES,
                             DEBT,
                             IS_COMMIT,
                             USED_USER_ID,
                             ETL_DT_TM
                        FROM CRS_USER.CASH_OPERATIONS WHERE OPERATION_DATE < TO_DATE('{operationDate}','DD.MM.YYYY')
                       ORDER BY OPERATION_DATE, ID";

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
                GlobalProcedures.LogWrite(operationDate + " tarixindən kiçik olan kassa əməliyyatları açılmadı.", sql, GlobalVariables.V_UserName, "CashOperationsDAL", "SelectCashOperationForDateAgain", exx);
                return null;
            }
        }

        public static decimal CashLastDebt(string operationDate)
        {
            decimal debt = 0;

            List<CashOperations> lstCashOperations = SelectCashOperationForDateAgain(operationDate).ToList<CashOperations>();
            if (lstCashOperations.Count == 0)
                return debt;

            var last_operation = lstCashOperations.LastOrDefault();
            if (last_operation != null)
                debt = last_operation.DEBT;

            return debt;
        }
    }
}
