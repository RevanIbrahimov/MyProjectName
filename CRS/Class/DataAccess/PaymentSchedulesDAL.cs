using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    class PaymentSchedulesDAL
    {
        public static DataSet SelectPaymentSchedules(int contractID)
        {
            string sql = $@"SELECT PS.ID,
                                   PS.CONTRACT_ID,
                                   PS.MONTH_DATE,
                                   PS.REAL_DATE,
                                   PS.MONTHLY_PAYMENT,
                                   PS.BASIC_AMOUNT,
                                   PS.INTEREST_AMOUNT,
                                   PS.DEBT,
                                   C.CODE CURRENCY_CODE,
                                   PS.USED_USER_ID,
                                   PS.IS_CHANGE_DATE,
                                   PS.ORDER_ID
                              FROM CRS_USER.V_LAST_PAYMENT_SCHEDULES PS, CRS_USER.CURRENCY C
                             WHERE PS.CURRENCY_ID = C.ID AND PS.CONTRACT_ID = {contractID}
                            ORDER BY PS.ORDER_ID";

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
                GlobalProcedures.LogWrite("Ödənişlər açılmadı.", sql, GlobalVariables.V_UserName, "PaymentSchedulesDAL", "SelectPaymentSchedules", exx);
                return null;
            }
        }
    }
}
