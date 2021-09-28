using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRS.Class.DataAccess
{
    public class OperationDebtDAL
    {
        public static DataSet SelectOperationDebtByID(int? debtID)
        {
            string sql = null;
            if (debtID == null)
                sql = @"SELECT ID,
                               DEBT_DATE,
                               YR_MNTH_DY,                              
                               ACCOUNT,
                               DEBIT,
                               CREDIT,
                               CURRENT_DEBIT,
                               CURRENT_CREDIT, 
                               NOTE,
                               PARENT_ID,                               
                               IS_MANUAL,
                               NOTE,
                               USED_USER_ID,
                               ETL_DT_TM
                          FROM CRS_USER.OPERATION_DEBT ORDER BY DEBT_DATE, ID";
            else
                sql = $@"SELECT ID,
                               DEBT_DATE,
                               YR_MNTH_DY,                              
                               ACCOUNT,
                               DEBIT,
                               CREDIT,
                               CURRENT_DEBIT,
                               CURRENT_CREDIT, 
                               NOTE,
                               PARENT_ID,                               
                               IS_MANUAL,
                               NOTE,
                               USED_USER_ID,
                               ETL_DT_TM
                          FROM CRS_USER.OPERATION_DEBT WHERE ID = {debtID}";

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
                GlobalProcedures.LogWrite("Qalıq açılmadı.", sql, GlobalVariables.V_UserName, "OperationDebtDAL", "SelectOperationDebt", exx);
            }
        }

        public static DataSet SelectOperationDebt(string account, int yr_mnth_dy)
        {
            string sql = null;
            sql = $@"SELECT ID,
                               DEBT_DATE,
                               YR_MNTH_DY,                              
                               ACCOUNT,
                               DEBIT,
                               CREDIT,
                               CURRENT_DEBIT,
                               CURRENT_CREDIT, 
                               NOTE,
                               PARENT_ID,                               
                               IS_MANUAL,
                               NOTE,
                               USED_USER_ID,
                               ETL_DT_TM
                          FROM CRS_USER.OPERATION_DEBT WHERE ACCOUNT = {account} AND YR_MNTH_DY = {yr_mnth_dy}
                    ORDER BY DEBT_DATE, ID";

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
                GlobalProcedures.LogWrite("Qalıq açılmadı.", sql, GlobalVariables.V_UserName, "OperationDebtDAL", "SelectOperationDebt", exx);
            }
        }

        public static DataSet SelectOperationDebtByAccount(string account)
        {
            string sql = null;
            sql = $@"SELECT ID,
                               DEBT_DATE,
                               YR_MNTH_DY,                              
                               ACCOUNT,
                               DEBIT,
                               CREDIT,
                               CURRENT_DEBIT,
                               CURRENT_CREDIT, 
                               NOTE,
                               PARENT_ID,                               
                               IS_MANUAL,
                               NOTE,
                               USED_USER_ID,
                               ETL_DT_TM
                          FROM CRS_USER.OPERATION_DEBT WHERE ACCOUNT = '{account}'
                    ORDER BY DEBT_DATE, ID";

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
                GlobalProcedures.LogWrite("Qalıq açılmadı.", sql, GlobalVariables.V_UserName, "OperationDebtDAL", "SelectOperationDebt", exx);
            }
        }
    }
}
