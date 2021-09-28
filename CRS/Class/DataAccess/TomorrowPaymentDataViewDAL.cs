using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class TomorrowPaymentDataViewDAL
    {
        public static DataSet SelectTomorrowPaymentData(string contractCode)
        {
            string sql = $@"SELECT CON.CONTRACT_ID CONTRACTID,
                                   CUS.CUSTOMER_NAME CUSTOMERNAME,
                                   COM.COMMITMENT_NAME COMMITMENTNAME,
                                   CON.CURRENCY_ID CURRENCYID,
                                   CON.CURRENCY_CODE CURRENCYCODE,
                                   CON.INTEREST,
                                   CON.AMOUNT,
                                   CON.USED_USER_ID
                              FROM CRS_USER.V_CONTRACTS CON,
                                   CRS_USER.V_COMMITMENTS COM,
                                   CRS_USER.V_CUSTOMERS CUS
                             WHERE     CON.CONTRACT_ID = COM.CONTRACT_ID(+)
                                   AND CON.CUSTOMER_ID = CUS.ID
                                   AND CON.CUSTOMER_TYPE_ID = CUS.PERSON_TYPE_ID
                                   AND CON.STATUS_ID = 5
                                   AND CON.CONTRACT_CODE = '{contractCode}'";

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
                GlobalProcedures.LogWrite("Sabahkı ödənişlərin məlumatları açılmadı.", sql, GlobalVariables.V_UserName, "TomorrowPaymentDataViewDAL", "SelectTomorrowPaymentData", exx);
            }
        }
    }
}
