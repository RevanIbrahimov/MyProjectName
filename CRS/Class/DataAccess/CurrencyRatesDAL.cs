using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRS.Class;
using CRS.Class.Tables;

namespace CRS.Class.DataAccess
{
    public class CurrencyRatesDAL
    {
        public static DataSet SelectCurrencyRatesByID(int? rateID)
        {
            string sql = null;
            if (rateID == null)
                sql = "SELECT ID,CURRENCY_ID,RATE_DATE,AMOUNT,NOTE FROM CRS_USER.CURRENCY_RATES ORDER BY ID";
            else
                sql = "SELECT ID,CURRENCY_ID,RATE_DATE,AMOUNT,NOTE FROM CRS_USER.CURRENCY_RATES WHERE RATE_ID = " + rateID;

            using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
            {
                DataSet dsAdapter = new DataSet();
                adapter.Fill(dsAdapter);
                return dsAdapter;
            }
        }

        public static DataSet SelectCurrencyRates(int currencyID, string rateDate)
        {
            string sql = null;
            if (rateDate == null)
                sql = "SELECT ID,CURRENCY_ID,RATE_DATE,AMOUNT,NOTE FROM CRS_USER.CURRENCY_RATES WHERE CURRENCY_ID = " + currencyID + " ORDER BY ID";
            else if (rateDate != null)
                sql = "SELECT ID,CURRENCY_ID,RATE_DATE,AMOUNT,NOTE FROM CRS_USER.CURRENCY_RATES WHERE CURRENCY_ID = " + currencyID + " AND RATE_DATE = TO_DATE('" + rateDate + "','DD/MM/YYYY') ORDER BY ID";

            using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
            {
                DataSet dsAdapter = new DataSet();
                adapter.Fill(dsAdapter);
                return dsAdapter;
            }
        }

        public static DataSet SelectCurrencyRatesByDate(string rateDate)
        {
            string sql = null;
            if (rateDate != null)
                sql = "SELECT ID,CURRENCY_ID,RATE_DATE,AMOUNT,NOTE FROM CRS_USER.CURRENCY_RATES WHERE RATE_DATE = TO_DATE('" + rateDate + "','DD/MM/YYYY') ORDER BY ID";

            using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
            {
                DataSet dsAdapter = new DataSet();
                adapter.Fill(dsAdapter);
                return dsAdapter;
            }
        }

        public static DataSet SelectCurrencyRatesLastDate()
        {
            string sql = null;
            sql = $@"SELECT C.ID CURRENCY_ID, C.CODE CURRENCY_CODE, CR.AMOUNT, CR.RATE_DATE, C.VALUE
                                  FROM CRS_USER.CURRENCY_RATES CR, CRS_USER.CURRENCY C
                                 WHERE     CR.CURRENCY_ID = C.ID
                                       AND RATE_DATE = (SELECT MAX (RATE_DATE)
                                                          FROM CRS_USER.CURRENCY_RATES
                                                         WHERE CURRENCY_ID = CR.CURRENCY_ID)
                                       AND C.CODE IN ('USD', 'EUR', 'RUB') 
                        ORDER BY C.ORDER_ID";

            using (OracleDataAdapter adapter = new OracleDataAdapter(sql, GlobalFunctions.GetConnectionString()))
            {
                DataSet dsAdapter = new DataSet();
                adapter.Fill(dsAdapter);
                return dsAdapter;
            }
        }

        public static string LastRateString()
        {
            List<CurrencyRates> lstRate = SelectCurrencyRatesLastDate().ToList<CurrencyRates>();
            if (lstRate.Count > 0)
            {
                decimal cur_USD = lstRate.Find(r => r.CURRENCY_CODE == "USD").AMOUNT;
                //cur_EUR = lstRate.Find(r => r.CURRENCY_CODE == "EUR").AMOUNT,
                //cur_RUB = lstRate.Find(r => r.CURRENCY_CODE == "RUB").AMOUNT;
                GlobalVariables.V_LastRate = lstRate.LastOrDefault().RATE_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ) + " tarixinə 1 USD = " + cur_USD + " AZN";
            }
            return GlobalVariables.V_LastRate;
        }
    }
}
