using CRS.Class.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class BankOperationsDAL
    {
        public static DataSet SelectBankOperation(int? operationID)
        {
            string sql = null;
            if (operationID == null)
                sql = @"SELECT ID,
                               BANK_ID,                               
                               ACCOUNTING_PLAN_ID,
                               OPERATION_DATE,
                               APPOINTMENT_ID,
                               INCOME,
                               EXPENSES,
                               DEBT,
                               DEBT_BANK,
                               NOTE,
                               CONTRACT_PAYMENT_ID,
                               CONTRACT_CODE,
                               FUNDS_PAYMENT_ID,
                               FUNDS_CONTRACT_ID,
                               USED_USER_ID
                          FROM CRS_USER.BANK_OPERATIONS
                    ORDER BY OPERATION_DATE, ID";
            else
                sql = $@"SELECT ID,
                               BANK_ID,                               
                               ACCOUNTING_PLAN_ID,
                               OPERATION_DATE,
                               APPOINTMENT_ID,
                               INCOME,
                               EXPENSES,
                               DEBT,
                               DEBT_BANK,
                               NOTE,
                               CONTRACT_PAYMENT_ID,
                               CONTRACT_CODE,
                               FUNDS_PAYMENT_ID,
                               FUNDS_CONTRACT_ID,
                               USED_USER_ID
                          FROM CRS_USER.BANK_OPERATIONS WHERE ID = {operationID}";

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
                GlobalProcedures.LogWrite("Bank əməliyyatı açılmadı.", sql, GlobalVariables.V_UserName, "BankOperationsDAL", "SelectBankOperation", exx);
            }
        }

        public static DataSet SelectBankOperation(int bankID)
        {
            string sql = $@"SELECT ID,
                               BANK_ID,                               
                               ACCOUNTING_PLAN_ID,
                               OPERATION_DATE,
                               APPOINTMENT_ID,
                               INCOME,
                               EXPENSES,
                               DEBT,
                               DEBT_BANK,
                               NOTE,
                               CONTRACT_PAYMENT_ID,
                               CONTRACT_CODE,
                               FUNDS_PAYMENT_ID,
                               FUNDS_CONTRACT_ID,
                               USED_USER_ID
                          FROM CRS_USER.BANK_OPERATIONS WHERE BANK_ID = {bankID}
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
                GlobalProcedures.LogWrite("Bank əməliyyatı açılmadı.", sql, GlobalVariables.V_UserName, "BankOperationsDAL", "SelectBankOperation", exx);
            }
        }

        public static DataSet SelectBankOperationDistinctBank(string date)
        {
            string sql = $@"SELECT DISTINCT BANK_ID    
                                FROM CRS_USER.BANK_OPERATIONS
                                WHERE OPERATION_DATE >= TO_DATE('{date}','DD/MM/YYYY')";

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
                GlobalProcedures.LogWrite("Bank əməliyyatı açılmadı.", sql, GlobalVariables.V_UserName, "BankOperationsDAL", "SelectBankOperationDistinctBank", exx);
            }
        }

        public static DataSet SelectBankOperationForDebt(string operationDate)
        {
            string sql = $@"SELECT ID,
                               BANK_ID,                               
                               ACCOUNTING_PLAN_ID,
                               OPERATION_DATE,
                               APPOINTMENT_ID,
                               INCOME,
                               EXPENSES,
                               DEBT,
                               DEBT_BANK,
                               NOTE,
                               CONTRACT_PAYMENT_ID,
                               CONTRACT_CODE,
                               FUNDS_PAYMENT_ID,
                               FUNDS_CONTRACT_ID,
                               USED_USER_ID
                          FROM CRS_USER.BANK_OPERATIONS WHERE OPERATION_DATE >= TO_DATE('{operationDate}','DD.MM.YYYY')
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
                GlobalProcedures.LogWrite("Bank əməliyyatları açılmadı.", sql, GlobalVariables.V_UserName, "BankOperationsDAL", "SelectBankOperationForDebt", exx);
            }
        }

        public static DataSet SelectBankOperationForDateAgain(string operationDate)
        {
            string sql = $@"SELECT ID,
                               BANK_ID,                               
                               ACCOUNTING_PLAN_ID,
                               OPERATION_DATE,
                               APPOINTMENT_ID,
                               INCOME,
                               EXPENSES,
                               DEBT,
                               DEBT_BANK,
                               NOTE,
                               CONTRACT_PAYMENT_ID,
                               CONTRACT_CODE,
                               FUNDS_PAYMENT_ID,
                               FUNDS_CONTRACT_ID,
                               USED_USER_ID
                          FROM CRS_USER.BANK_OPERATIONS WHERE OPERATION_DATE < TO_DATE('{operationDate}','DD.MM.YYYY')
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
                GlobalProcedures.LogWrite("Bank əməliyyatları açılmadı.", sql, GlobalVariables.V_UserName, "BankOperationsDAL", "SelectBankOperationForDateAgain", exx);
            }
        }

        public static decimal GeneralLastDebt(string operationDate)
        {
            decimal debt = 0;

            List<BankOperations> lstBankOperations = SelectBankOperationForDateAgain(operationDate).ToList<BankOperations>();
            if (lstBankOperations.Count == 0)
                return debt;

            var last_operation = lstBankOperations.LastOrDefault();
            if (last_operation != null)
                debt = last_operation.DEBT;

            return debt;
        }

        public static decimal BankLastDebt(string operationDate, int bank_id)
        {
            decimal debt = 0;

            List<BankOperations> lstBankOperations = SelectBankOperationForDateAgain(operationDate).ToList<BankOperations>();
            if (lstBankOperations.Count == 0)
                return debt;

            var last_operation = lstBankOperations.Where(item => item.BANK_ID == bank_id).LastOrDefault();
            if (last_operation != null)
                debt = last_operation.DEBT_BANK;

            return debt;
        }
    }
}
