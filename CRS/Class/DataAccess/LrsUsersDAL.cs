﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class LrsUsersDAL
    {
        public static DataSet SelectUser(int? userID)
        {
            string sql = null;
            if (userID == null)
                sql = $@"SELECT U.ID,
                                 U.SURNAME,
                                 U.NAME,
                                 U.PATRONYMIC,
                                 U.SURNAME || ' ' || U.NAME || ' ' || U.PATRONYMIC FULLNAME,
                                 U.NIKNAME,
                                 U.PASSWORD,
                                 U.STATUS_ID,
                                 S.NAME SEX_NAME,
                                 SESSION_ID,
                                 U.GROUP_ID,
                                 BIRTHDAY,
                                 NOTE,
                                 USED_USER_ID
                            FROM CRS_USER.CRS_USERS U, CRS_USER.SEX S
                           WHERE U.SEX_ID = S.ID
                        ORDER BY SURNAME";
            else
                sql = $@"SELECT U.ID,
                                 U.SURNAME,
                                 U.NAME,
                                 U.PATRONYMIC,
                                 U.SURNAME || ' ' || U.NAME || ' ' || U.PATRONYMIC FULLNAME,
                                 U.NIKNAME,
                                 U.PASSWORD,
                                 U.STATUS_ID,
                                 S.NAME SEX_NAME,
                                 SESSION_ID,
                                 U.GROUP_ID,
                                 BIRTHDAY,
                                 NOTE,
                                 USED_USER_ID
                            FROM CRS_USER.CRS_USERS U, CRS_USER.SEX S
                           WHERE U.SEX_ID = S.ID
                                 AND U.ID = {userID}";

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
                GlobalProcedures.LogWrite("İstifadəçinin məlumatları açılmadı.", sql, GlobalVariables.V_UserName, "LrsUsersDAL", "SelectUser", exx);
            }
        }
    }
}
