using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRS.Class.DataAccess
{
    public class AccountingPlanDAL
    {
        public static DataSet SelectAccountingPlan(int? planID)
        {
            string sql = null;
            if (planID == null)
                sql = "SELECT ID,ACCOUNT_NUMBER,ACCOUNT_NAME,SUB_ACCOUNT,SUB_ACCOUNT_NAME,ACCOUNT_TYPE_ID,ACCOUNT_CATEGORY,BANK_ID,NOTE,USED_USER_ID,ETL_DT_TM,NVL(SUB_ACCOUNT,ACCOUNT_NUMBER) ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN ORDER BY ID";
            else
                sql = "SELECT ID,ACCOUNT_NUMBER,ACCOUNT_NAME,SUB_ACCOUNT,SUB_ACCOUNT_NAME,ACCOUNT_TYPE_ID,ACCOUNT_CATEGORY,BANK_ID,NOTE,USED_USER_ID,ETL_DT_TM,NVL(SUB_ACCOUNT,ACCOUNT_NUMBER) ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN WHERE ID = " + planID;

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
                GlobalProcedures.LogWrite("Hesablar planı açılmadı.", sql, GlobalVariables.V_UserName, "AccountingPlanDAL", "SelectAccountingPlan", exx);
            }
        }
    }
}
