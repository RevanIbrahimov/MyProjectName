using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRS.Class.DataAccess
{
    public class ContractDAL
    {
        public static DataSet SelectContract(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = @"SELECT C.ID,
                                 CT.CODE||C.CODE CODE,
                                 C.CUSTOMER_ID,
                                 C.SELLER_ID,
                                 C.CREDIT_TYPE_ID,
                                 C.START_DATE,
                                 C.END_DATE,
                                 C.GRACE_PERIOD,
                                 C.AMOUNT,
                                 C.CURRENCY_ID,
                                 C.CUSTOMER_ACCOUNT,
                                 C.LEASING_ACCOUNT,
                                 C.LEASING_INTEREST_ACCOUNT,
                                 C.IS_COMMIT,
                                 C.COMMITED_USER_ID,
                                 C.USED_USER_ID,
                                 C.CHECK_END_DATE,
                                 C.CUSTOMER_CARDS_ID,
                                 C.MONTHLY_AMOUNT,
                                 C.CHECK_PERIOD,
                                 C.PAYMENT_TYPE,
                                 C.STATUS_ID,
                                 C.COUNT,
                                 C.NOTE,
                                 C.NOTE_CHANGE_USER,
                                 C.NOTE_CHANGE_DATE,
                                 C.CHECK_INTEREST,
                                 C.ETL_DT_TM,
                                 C.SELLER_ACCOUNT,
                                 C.BANK_CASH,
                                 C.IS_EXPENSES,
                                 C.LIQUID_TYPE,
                                 C.CURRENCY_RATE,
                                 C.SELLER_TYPE_ID,
                                 C.CUSTOMER_TYPE_ID,
                                 C.PARENT_ID,
                                 CT.INTEREST,
                                 CT.TERM PERIOD,
                                 C.IS_SPECIAL_ATTENTION,
                                 (SELECT COUNT (*)
                                    FROM CRS_USER.CONTRACT_IMAGES
                                   WHERE CONTRACT_ID = C.ID)
                                    CONTRACT_IMAGE_COUNT
                            FROM CRS_USER.CONTRACTS C,
                                 CRS_USER.CREDIT_TYPE CT
                           WHERE C.CREDIT_TYPE_ID = CT.ID
                        ORDER BY ID";
            else
                sql = $@"SELECT C.ID,
                                 CT.CODE||C.CODE CODE,
                                 C.CUSTOMER_ID,
                                 C.SELLER_ID,
                                 C.CREDIT_TYPE_ID,
                                 C.START_DATE,
                                 C.END_DATE,
                                 C.GRACE_PERIOD,
                                 C.AMOUNT,
                                 C.CURRENCY_ID,
                                 C.CUSTOMER_ACCOUNT,
                                 C.LEASING_ACCOUNT,
                                 C.LEASING_INTEREST_ACCOUNT,
                                 C.IS_COMMIT,
                                 C.COMMITED_USER_ID,
                                 C.USED_USER_ID,
                                 C.CHECK_END_DATE,
                                 C.CUSTOMER_CARDS_ID,
                                 C.MONTHLY_AMOUNT,
                                 C.CHECK_PERIOD,
                                 C.PAYMENT_TYPE,
                                 C.STATUS_ID,
                                 C.COUNT,
                                 C.NOTE,
                                 C.NOTE_CHANGE_USER,
                                 C.NOTE_CHANGE_DATE,
                                 C.CHECK_INTEREST,
                                 C.ETL_DT_TM,
                                 C.SELLER_ACCOUNT,
                                 C.BANK_CASH,
                                 C.IS_EXPENSES,
                                 C.LIQUID_TYPE,
                                 C.CURRENCY_RATE,
                                 C.SELLER_TYPE_ID,
                                 C.CUSTOMER_TYPE_ID,
                                 C.PARENT_ID,
                                 CT.INTEREST,
                                 CT.TERM PERIOD,
                                 C.IS_SPECIAL_ATTENTION,
                                 (SELECT COUNT (*)
                                    FROM CRS_USER.CONTRACT_IMAGES
                                   WHERE CONTRACT_ID = C.ID)
                                    CONTRACT_IMAGE_COUNT
                            FROM CRS_USER.CONTRACTS C,
                                 CRS_USER.CREDIT_TYPE CT
                           WHERE C.CREDIT_TYPE_ID = CT.ID AND C.ID = {contractID}
                    ORDER BY ID";

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
                GlobalProcedures.LogWrite("Müqavilə açılmadı.", sql, GlobalVariables.V_UserName, "ContractsDAL", "SelectContract", exx);
                return null;
            }
        }

        public static DataSet SelectContractWithParentID(int parentID)
        {
            string sql = $@"SELECT C.ID,
                                 CT.CODE||C.CODE CODE,
                                 C.CUSTOMER_ID,
                                 C.SELLER_ID,
                                 C.CREDIT_TYPE_ID,
                                 C.START_DATE,
                                 C.END_DATE,
                                 C.GRACE_PERIOD,
                                 C.AMOUNT,
                                 C.CURRENCY_ID,
                                 C.CUSTOMER_ACCOUNT,
                                 C.LEASING_ACCOUNT,
                                 C.LEASING_INTEREST_ACCOUNT,
                                 C.IS_COMMIT,
                                 C.COMMITED_USER_ID,
                                 C.USED_USER_ID,
                                 C.CHECK_END_DATE,
                                 C.CUSTOMER_CARDS_ID,
                                 C.MONTHLY_AMOUNT,
                                 C.CHECK_PERIOD,
                                 C.PAYMENT_TYPE,
                                 C.STATUS_ID,
                                 C.COUNT,
                                 C.NOTE,
                                 C.NOTE_CHANGE_USER,
                                 C.NOTE_CHANGE_DATE,
                                 C.CHECK_INTEREST,
                                 C.ETL_DT_TM,
                                 C.SELLER_ACCOUNT,
                                 C.BANK_CASH,
                                 C.IS_EXPENSES,
                                 C.LIQUID_TYPE,
                                 C.CURRENCY_RATE,
                                 C.SELLER_TYPE_ID,
                                 C.CUSTOMER_TYPE_ID,
                                 C.PARENT_ID,
                                 C.IS_SPECIAL_ATTENTION
                            FROM CRS_USER.CONTRACTS C,
                                 CRS_USER.CREDIT_TYPE CT,
                                 CRS_USER.CREDIT_NAMES CN
                           WHERE C.CREDIT_TYPE_ID = CT.ID AND CT.NAME_ID = CN.ID AND C.PARENT_ID = {parentID}
                    ORDER BY ID";

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
                GlobalProcedures.LogWrite("Müqavilənin razılaşmaları açılmadı.", sql, GlobalVariables.V_UserName, "ContractsDAL", "SelectContractWithParentID", exx);
                return null;
            }
        }
    }
}
