﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   CRS.Class.DataAccess
{
    public class CreditClassDAL
    {
        public static DataSet SelectCreditClassByID(int? typeID)
        {
            string sql = null;
            if (typeID == null)
                sql = "SELECT ID,NAME,CODE FROM CRS_USER.CREDIT_CLASS ORDER BY ORDER_ID";
            else
                sql = $@"SELECT ID,NAME,CODE FROM CRS_USER.CREDIT_CLASS WHERE ID = {typeID}";

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
                GlobalProcedures.LogWrite("Əyyarlar açılmadı.", sql, GlobalVariables.V_UserName, "CreditClassDAL", "SelectCreditClassByID", exx);
            }
        }
    }
}
