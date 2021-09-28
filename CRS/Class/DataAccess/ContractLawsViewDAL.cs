using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class ContractLawsViewDAL
    {
        public static DataSet SelectContractLaw(int? parentID)
        {
            string sql = null;

            if (parentID != null)
                sql = $@"SELECT CL.ID,
                               CL.PARENT_ID PARENTID,
                               CL.CONTRACT_ID CONTRACTID,
                               CL.DEFANDANT_NAME DEFANDANTNAME,
                               CL.LAW_ID LAWID,
                               L.NAME LAWNAME,
                               CL.JUDGE_NAME JUDGENAME,
                               CL.LAW_STATUS_ID LAWSTATUSID,
                               CL.START_DATE STARTDATE,
                               CL.LAST_DATE LASTDATE,
                               CL.NEXT_DATE NEXTDATE,
                               CL.NOTE,
                               CL.CREATED_USER_NAME CREATEDUSERNAME,
                               CL.IS_ACTIVE ISACTIVE,
                               CL.USED_USER_ID USEDUSERID
                          FROM CRS_USER.CONTRACT_LAWS CL, CRS_USER.LAWS L
                         WHERE CL.LAW_ID = L.ID AND CL.ID = {parentID}";
            else
                sql = $@"SELECT CL.ID,
                               CL.PARENT_ID PARENTID,
                               CL.CONTRACT_ID CONTRACTID,
                               CL.DEFANDANT_NAME DEFANDANTNAME,
                               CL.LAW_ID LAWID,
                               L.NAME LAWNAME,
                               CL.JUDGE_NAME JUDGENAME,
                               CL.LAW_STATUS_ID LAWSTATUSID,
                               CL.START_DATE STARTDATE,
                               CL.LAST_DATE LASTDATE,
                               CL.NEXT_DATE NEXTDATE,
                               CL.NOTE,
                               CL.CREATED_USER_NAME CREATEDUSERNAME,
                               CL.IS_ACTIVE ISACTIVE,
                               CL.USED_USER_ID USEDUSERID
                          FROM CRS_USER.CONTRACT_LAWS CL, CRS_USER.LAWS L
                         WHERE CL.LAW_ID = L.ID
                       ORDER BY CONTRACT_ID,ID";

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
                GlobalProcedures.LogWrite("Məhkəmə prosesləri açılmadı.", sql, GlobalVariables.V_UserName, "ContractLawsViewDAL", "SelectContractLaw", exx);
                return null;
            }
        }
    }
}
