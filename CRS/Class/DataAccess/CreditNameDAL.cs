using CRS.Class.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CreditNameDAL
    {
        public static DataSet SelectCreditNameByID(int? nameID)
        {
            string sql = null;
            if (nameID == null)
                sql = "SELECT ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CREDIT_NAMES WHERE ID IN (1,5) ORDER BY ID";
            else
                sql = "SELECT ID,NAME,NOTE,USED_USER_ID FROM CRS_USER.CREDIT_NAMES WHERE ID = " + nameID;

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
                GlobalProcedures.LogWrite("Lizinqin növü açılmadı.", sql, GlobalVariables.V_UserName, "CreditNameDAL", "SelectCreditNameByID", exx);
            }
        }

        public static List<CreditName> lstCreditName = new List<CreditName>();

        public static void InsertCreditName(int creidtID, string name, string note)
        {
            lstCreditName.Add(new CreditName()
            {
                ID = creidtID,
                NAME = name,
                NOTE = note
            });
        }

        public static void RemoveAllCreditName()
        {
            lstCreditName.Clear();
        }
    }

}
