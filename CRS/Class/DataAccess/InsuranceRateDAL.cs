using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class InsuranceRateDAL
    {
        public static DataSet SelectInsuranceRate(int? rateID)
        {
            string sql = null;
            if (rateID == null)
                sql = "SELECT ID,RATE,UNCONDITIONAL_AMOUNT AMOUNT,NOTE,USED_USER_ID FROM CRS_USER.INSURANCE_RATE ORDER BY ORDER_ID";
            else
                sql = "SELECT ID,RATE,UNCONDITIONAL_AMOUNT AMOUNT,NOTE,USED_USER_ID FROM CRS_USER.INSURANCE_RATE WHERE ID = " + rateID;

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
                GlobalProcedures.LogWrite("Sığorta dərəcəsi açılmadı.", sql, GlobalVariables.V_UserName, "InsuranceRateDAL", "SelectInsuranceRate", exx);
            }
        }
    }
}
