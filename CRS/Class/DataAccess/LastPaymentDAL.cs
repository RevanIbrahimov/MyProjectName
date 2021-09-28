using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class LastPaymentDAL
    {
        public static DataSet SelectLastPayment(int contractID)
        {
            string sql = null;

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
                                   ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID
                              FROM CRS_USER.V_CUSTOMER_LAST_PAYMENT
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
                GlobalProcedures.LogWrite("Ödənişlər açılmadı.", sql, GlobalVariables.V_UserName, "LastPaymentDAL", "SelectLastPayment", exx);
                return null;
            }
        }
    }
}
