using CRS.Class;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class PaymentsDAL
    {
        public static DataSet SelectPayments(int istemp, int contractID)
        {
            string sql = null;
                
            if(istemp == 1)                
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   PAYMENT_DATE,
                                   PAYMENT_AMOUNT,
                                   PAYMENT_AMOUNT_AZN,
                                   BASIC_AMOUNT,
                                   DEBT,
                                   DAY_COUNT,
                                   ONE_DAY_INTEREST_AMOUNT,
                                   INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_DEBT,
                                   TOTAL,                           
                                   CURRENCY_RATE,
                                   PAYMENT_NAME,
                                   BANK_CASH,
                                   IS_CHANGE,
                                   ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                   IS_CHANGED_INTEREST,
                                   CHANGED_PAY_INTEREST_AMOUNT,
                                   CLEARING_DATE,
                                   IS_CLEARING,
                                   CLEARING_CALCULATED,
                                   PENALTY_DEBT,
                                   IS_PENALTY,
                                   PENALTY_AMOUNT,
                                   PAYED_PENALTY
                              FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                             WHERE IS_CHANGE != 2 AND CONTRACT_ID = {contractID}
                          ORDER BY PAYMENT_DATE, ID";       
            else
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   PAYMENT_DATE,
                                   PAYMENT_AMOUNT,
                                   PAYMENT_AMOUNT_AZN,
                                   BASIC_AMOUNT,
                                   DEBT,
                                   DAY_COUNT,
                                   ONE_DAY_INTEREST_AMOUNT,
                                   INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_DEBT,
                                   TOTAL,                           
                                   CURRENCY_RATE,
                                   PAYMENT_NAME,
                                   BANK_CASH,
                                   0 IS_CHANGE,
                                   ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                   IS_CHANGED_INTEREST,
                                   CHANGED_PAY_INTEREST_AMOUNT,
                                   CLEARING_DATE,
                                   IS_CLEARING,
                                   CLEARING_CALCULATED,
                                   PENALTY_DEBT,
                                   IS_PENALTY,
                                   PENALTY_AMOUNT,
                                   PAYED_PENALTY
                              FROM CRS_USER.CUSTOMER_PAYMENTS
                             WHERE CONTRACT_ID = {contractID}
                        ORDER BY PAYMENT_DATE, ID";

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
                GlobalProcedures.LogWrite("Ödənişlər açılmadı.", sql, GlobalVariables.V_UserName, "PaymentsDAL", "SelectPayments", exx);
                return null;
            }
        }

        public static DataSet SelectAllPayments(int istemp, int contractID)
        {
            string sql = null;

            if (istemp == 1)
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   PAYMENT_DATE,
                                   PAYMENT_AMOUNT,
                                   PAYMENT_AMOUNT_AZN,
                                   BASIC_AMOUNT,
                                   DEBT,
                                   DAY_COUNT,
                                   ONE_DAY_INTEREST_AMOUNT,
                                   INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_DEBT,
                                   TOTAL,                           
                                   CURRENCY_RATE,
                                   PAYMENT_NAME,
                                   BANK_CASH,
                                   IS_CHANGE,
                                   ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                   IS_CHANGED_INTEREST,
                                   CHANGED_PAY_INTEREST_AMOUNT,
                                   CLEARING_DATE,
                                   IS_CLEARING,
                                   CLEARING_CALCULATED,
                                   PENALTY_DEBT,
                                   IS_PENALTY,
                                   PENALTY_AMOUNT,
                                   PAYED_PENALTY
                              FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                             WHERE CONTRACT_ID = {contractID}
                          ORDER BY PAYMENT_DATE, ID";
            else
                sql = $@"SELECT ID,
                                   CONTRACT_ID,
                                   PAYMENT_DATE,
                                   PAYMENT_AMOUNT,
                                   PAYMENT_AMOUNT_AZN,
                                   BASIC_AMOUNT,
                                   DEBT,
                                   DAY_COUNT,
                                   ONE_DAY_INTEREST_AMOUNT,
                                   INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_AMOUNT,
                                   PAYMENT_INTEREST_DEBT,
                                   TOTAL,                           
                                   CURRENCY_RATE,
                                   PAYMENT_NAME,
                                   BANK_CASH,
                                   0 IS_CHANGE,
                                   ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                   IS_CHANGED_INTEREST,
                                   CHANGED_PAY_INTEREST_AMOUNT,
                                   CLEARING_DATE,
                                   IS_CLEARING,
                                   CLEARING_CALCULATED,
                                   PENALTY_DEBT,
                                   IS_PENALTY,
                                   PENALTY_AMOUNT,
                                   PAYED_PENALTY
                              FROM CRS_USER.CUSTOMER_PAYMENTS
                             WHERE CONTRACT_ID = {contractID}
                        ORDER BY PAYMENT_DATE, ID";

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
                GlobalProcedures.LogWrite("Ödənişlər açılmadı.", sql, GlobalVariables.V_UserName, "PaymentsDAL", "SelectPayments", exx);
                return null;
            }
        }

        public static DataSet SelectPaymentsForAgain(int istemp, int contractID, int orderID)
        {
            string sql = null;

            if (istemp == 1)
                sql = $@"SELECT *
                                FROM (SELECT ID,
                                             CONTRACT_ID,
                                             PAYMENT_DATE,
                                             PAYMENT_AMOUNT,
                                             PAYMENT_AMOUNT_AZN,
                                             BASIC_AMOUNT,
                                             DEBT,
                                             DAY_COUNT,
                                             ONE_DAY_INTEREST_AMOUNT,
                                             INTEREST_AMOUNT,
                                             PAYMENT_INTEREST_AMOUNT,
                                             PAYMENT_INTEREST_DEBT,
                                             TOTAL,
                                             CURRENCY_RATE,
                                             PAYMENT_NAME,
                                             BANK_CASH,
                                             IS_CHANGE,
                                             ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                             CLEARING_DATE,
                                             IS_CLEARING,
                                             CLEARING_CALCULATED,
                                             PENALTY_DEBT,
                                             IS_PENALTY,
                                             PENALTY_AMOUNT,
                                             PAYED_PENALTY
                                        FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                                       WHERE CONTRACT_ID = {contractID})
                               WHERE ORDER_ID < {orderID}
                            ORDER BY PAYMENT_DATE, ID";
            else
                sql = $@"SELECT *
                                FROM (SELECT ID,
                                             CONTRACT_ID,
                                             PAYMENT_DATE,
                                             PAYMENT_AMOUNT,
                                             PAYMENT_AMOUNT_AZN,
                                             BASIC_AMOUNT,
                                             DEBT,
                                             DAY_COUNT,
                                             ONE_DAY_INTEREST_AMOUNT,
                                             INTEREST_AMOUNT,
                                             PAYMENT_INTEREST_AMOUNT,
                                             PAYMENT_INTEREST_DEBT,
                                             TOTAL,
                                             CURRENCY_RATE,
                                             PAYMENT_NAME,
                                             BANK_CASH,
                                             0 IS_CHANGE,
                                             ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                             CLEARING_DATE,
                                             IS_CLEARING,
                                             CLEARING_CALCULATED,
                                             PENALTY_DEBT,
                                             IS_PENALTY,
                                             PENALTY_AMOUNT,
                                             PAYED_PENALTY
                                        FROM CRS_USER.CUSTOMER_PAYMENTS
                                       WHERE CONTRACT_ID = {contractID})
                               WHERE ORDER_ID < {orderID}
                            ORDER BY PAYMENT_DATE, ID";

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
                GlobalProcedures.LogWrite("Ödənişlər açılmadı.", sql, GlobalVariables.V_UserName, "PaymentsDAL", "SelectPaymentsTemp", exx);
                return null;
            }
        }

        public static bool? PaymentTempIsEmpty(int contractID)
        {
            string sql = $@"SELECT *
                            FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                                WHERE CONTRACT_ID = {contractID}";            

            try
            {
                using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
                {
                    DataSet dsAdapter = new DataSet();
                    adapter.Fill(dsAdapter);

                    if (dsAdapter.IsEmpty())
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Temp ödənişlər tapılmadı.", sql, GlobalVariables.V_UserName, "PaymentsDAL", "PaymentTempIsEmpty", exx);
                return null;
            }
        }
    }
}
