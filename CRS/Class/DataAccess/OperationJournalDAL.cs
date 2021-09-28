using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRS.Class.DataAccess
{
    public class OperationJournalDAL
    {
        public static DataSet SelectOperationJournal(int? operationID)
        {
            string sql = null;
            if (operationID == null)
                sql = @"SELECT ID,
                               OPERATION_DATE,
                               YR_MNTH_DY,
                               CREATED_USER_ID,
                               DEBIT_ACCOUNT,
                               CREDIT_ACCOUNT,
                               CURRENCY_RATE,
                               AMOUNT_CUR,
                               AMOUNT_AZN,
                               APPOINTMENT,
                               CONTRACT_ID,
                               CUSTOMER_PAYMENT_ID,
                               ACCOUNT_OPERATION_TYPE_ID,
                               IS_MANUAL,
                               USED_USER_ID,
                               ETL_DT_TM
                          FROM CRS_USER.OPERATION_JOURNAL ORDER BY OPERATION_DATE, ID";
            else
                sql = $@"SELECT ID,
                               OPERATION_DATE,
                               YR_MNTH_DY,
                               CREATED_USER_ID,
                               DEBIT_ACCOUNT,
                               CREDIT_ACCOUNT,
                               CURRENCY_RATE,
                               AMOUNT_CUR,
                               AMOUNT_AZN,
                               APPOINTMENT,
                               CONTRACT_ID,
                               CUSTOMER_PAYMENT_ID,
                               ACCOUNT_OPERATION_TYPE_ID,
                               IS_MANUAL,
                               USED_USER_ID,
                               ETL_DT_TM
                          FROM CRS_USER.OPERATION_JOURNAL WHERE ID = {operationID}";

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
                GlobalProcedures.LogWrite("Jurnal açılmadı.", sql, GlobalVariables.V_UserName, "OperationJournalDAL", "SelectOperationJournal", exx);
            }
        }

        public static DataSet SelectOperationJournal(string debit_account, string credit_account)
        {
            string sql = null;
            sql = $@"SELECT ID,
                               OPERATION_DATE,
                               YR_MNTH_DY,
                               CREATED_USER_ID,
                               DEBIT_ACCOUNT,
                               CREDIT_ACCOUNT,
                               CURRENCY_RATE,
                               AMOUNT_CUR,
                               AMOUNT_AZN,
                               APPOINTMENT,
                               CONTRACT_ID,
                               CUSTOMER_PAYMENT_ID,
                               ACCOUNT_OPERATION_TYPE_ID,
                               IS_MANUAL,
                               USED_USER_ID,
                               ETL_DT_TM
                          FROM CRS_USER.OPERATION_JOURNAL WHERE DEBIT_ACCOUNT = '{debit_account}' OR CREDIT_ACCOUNT = '{credit_account}'";

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
                GlobalProcedures.LogWrite("Jurnal açılmadı.", sql, GlobalVariables.V_UserName, "OperationJournalDAL", "SelectOperationJournal", exx);
            }
        }
    }
}
