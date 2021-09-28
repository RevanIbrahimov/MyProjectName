using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class BranchesDAL
    {
        public static DataSet SelectBranchByID(int? branchID)
        {
            string sql = null;
            if (branchID == null)
                sql = "SELECT ID,BANK_ID,NAME,CODE,NOTE,STATUS_ID FROM CRS_USER.BANK_BRANCHES ORDER BY NAME";
            else
                sql = $@"SELECT ID,BANK_ID,NAME,CODE,NOTE,STATUS_ID FROM CRS_USER.BANK_BRANCHES WHERE ID = {branchID}";

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
                GlobalProcedures.LogWrite("Filial açılmadı.", sql, GlobalVariables.V_UserName, "BranchesDAL", "SelectBranchByID", exx);
            }
        }

        public static DataSet SelectBranchByName(string branchName)
        {
            string sql = $@"SELECT ID,BANK_ID,NAME,CODE,NOTE,STATUS_ID FROM CRS_USER.BANK_BRANCHES WHERE NAME = '{branchName}'";

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
                GlobalProcedures.LogWrite("Filial açılmadı.", sql, GlobalVariables.V_UserName, "BranchesDAL", "SelectBranchByName", exx);
            }
        }

        public static DataSet SelectBranchByBankID(int bankID)
        {
            string sql = $@"SELECT ID,BANK_ID,NAME,CODE,NOTE,STATUS_ID FROM CRS_USER.BANK_BRANCHES WHERE BANK_ID = {bankID}";

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
                GlobalProcedures.LogWrite("Filial açılmadı.", sql, GlobalVariables.V_UserName, "BranchesDAL", "SelectBranchByBankID", exx);
            }
        }
    }
}
