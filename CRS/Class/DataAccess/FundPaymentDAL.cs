using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class FundPaymentDAL
    {
        public static DataSet SelectFundPayments(int istemp, int contractID)
        {
            string sql = null;

            if (istemp == 1)
                sql = $@"SELECT ID,
                                 CONTRACT_ID,
                                 PAYMENT_DATE,
                                 BUY_AMOUNT,
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
                                 IS_CHANGE,
                                 ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID
                            FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP
                           WHERE CONTRACT_ID = {contractID}
                              AND USED_USER_ID = {GlobalVariables.V_UserID}
                        ORDER BY PAYMENT_DATE, ID";
            else
                sql = $@"SELECT ID,
                                 CONTRACT_ID,
                                 PAYMENT_DATE,
                                 BUY_AMOUNT,
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
                                 ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID
                            FROM CRS_USER.FUNDS_PAYMENTS
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
                GlobalProcedures.LogWrite("Cəlb olunmuş vəsaitlərin ödənişləri açılmadı.", sql, GlobalVariables.V_UserName, "FundPaymentDAL", "SelectFundPayments", exx);
                return null;
            }
        }
    }
}
