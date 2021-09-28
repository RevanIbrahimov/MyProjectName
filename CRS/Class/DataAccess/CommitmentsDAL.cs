using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.DataAccess
{
    public class CommitmentsDAL
    {
        public static DataSet SelectCommitmentTempByContractID(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CS.SERIES CARD_SERIES,
                                 CC.CARD_NUMBER,
                                 CC.CARD_PINCODE,
                                 CC.CARD_ISSUE_DATE,
                                 CC.CARD_RELIABLE_DATE,
                                 CI.NAME CARD_ISSUING_NAME,
                                 CC.ADDRESS,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 CC.IS_CHANGE,
                                    CS.SERIES
                                 || ', №: '
                                 || CC.CARD_NUMBER
                                 || ', '
                                 || TO_CHAR (CC.CARD_ISSUE_DATE, 'DD.MM.YYYY')
                                 || ' tarixdə '
                                 || CI.NAME
                                 || ' tərəfindən verilib'
                                    CARD
                            FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP CC,
                                 CRS_USER.CARD_SERIES CS,
                                 CRS_USER.CARD_ISSUING CI,
                                 CRS_USER.CURRENCY C
                           WHERE     CC.CARD_SERIES_ID = CS.ID
                                 AND CC.CARD_ISSUING_ID = CI.ID
                                 AND CC.CURRENCY_ID = C.ID                                 
                                 AND CC.IS_CHANGE <> 2
                        ORDER BY CC.ID";
            else
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CS.SERIES CARD_SERIES,
                                 CC.CARD_NUMBER,
                                 CC.CARD_PINCODE,
                                 CC.CARD_ISSUE_DATE,
                                 CC.CARD_RELIABLE_DATE,
                                 CI.NAME CARD_ISSUING_NAME,
                                 CC.ADDRESS,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 CC.IS_CHANGE,
                                    CS.SERIES
                                 || ', №: '
                                 || CC.CARD_NUMBER
                                 || ', '
                                 || TO_CHAR (CC.CARD_ISSUE_DATE, 'DD.MM.YYYY')
                                 || ' tarixdə '
                                 || CI.NAME
                                 || ' tərəfindən verilib'
                                    CARD
                            FROM CRS_USER_TEMP.CONTRACT_COMMITMENTS_TEMP CC,
                                 CRS_USER.CARD_SERIES CS,
                                 CRS_USER.CARD_ISSUING CI,
                                 CRS_USER.CURRENCY C
                           WHERE     CC.CARD_SERIES_ID = CS.ID
                                 AND CC.CARD_ISSUING_ID = CI.ID
                                 AND CC.CURRENCY_ID = C.ID                                 
                                 AND CC.IS_CHANGE <> 2
                                 AND CC.CONTRACT_ID = {contractID}
                        ORDER BY CC.ID";

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
                GlobalProcedures.LogWrite("Öhdəlik açılmadı.", sql, GlobalVariables.V_UserName, "CommitmentsDAL", "SelectCommitmentTempByContractID", exx);
                return null;
            }
        }

        public static DataSet SelectAllCommitmentTempByContractID(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 CC.IS_CHANGE,
                                 CC.PERSON_TYPE_ID,
                                 CD.CARD,
                                 CD.REGISTRATION_ADDRESS ADDRESS,
                                 CD.VOEN
                            FROM CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP CC,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER_TEMP.V_TEMP_COMMITMENT_CARDS CD
                           WHERE     CC.CURRENCY_ID = C.ID
                                 AND CC.IS_CHANGE <> 2
                                 AND CC.ID = CD.COMMITMENT_ID
                        ORDER BY CC.ID";
            else
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 CC.IS_CHANGE,
                                 CC.PERSON_TYPE_ID,
                                 CD.CARD,
                                 CD.REGISTRATION_ADDRESS ADDRESS,
                                 CD.VOEN
                            FROM CRS_USER_TEMP.CONTRACT_ALL_COMMITMENTS_TEMP CC,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER_TEMP.V_TEMP_COMMITMENT_CARDS CD
                           WHERE     CC.CURRENCY_ID = C.ID
                                 AND CC.IS_CHANGE <> 2
                                 AND CC.ID = CD.COMMITMENT_ID
                                 AND CC.CONTRACT_ID = {contractID}
                        ORDER BY CC.ID";

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
                GlobalProcedures.LogWrite("Öhdəlik açılmadı.", sql, GlobalVariables.V_UserName, "CommitmentsDAL", "SelectAllCommitmentTempByContractID", exx);
                return null;
            }
        }

        public static DataSet SelectAllCommitmentByContractID(int? contractID)
        {
            string sql = null;
            if (contractID == null)
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 CC.PERSON_TYPE_ID,
                                 CD.CARD,
                                 CD.REGISTRATION_ADDRESS ADDRESS,
                                 CD.VOEN
                            FROM CRS_USER.CONTRACT_ALL_COMMITMENTS CC,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER.V_COMMITMENT_CARDS CD
                           WHERE CC.CURRENCY_ID = C.ID AND CC.ID = CD.COMMITMENT_ID
                        ORDER BY CC.ID";
            else
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 CC.PERSON_TYPE_ID,
                                 CD.CARD,
                                 CD.REGISTRATION_ADDRESS ADDRESS,
                                 CD.VOEN
                            FROM CRS_USER.CONTRACT_ALL_COMMITMENTS CC,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER.V_COMMITMENT_CARDS CD
                           WHERE CC.CURRENCY_ID = C.ID AND CC.ID = CD.COMMITMENT_ID AND CC.CONTRACT_ID = {contractID}
                        ORDER BY CC.ID";

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
                GlobalProcedures.LogWrite("Öhdəlik açılmadı.", sql, GlobalVariables.V_UserName, "CommitmentsDAL", "SelectAllCommitmentByContractID", exx);
                return null;
            }
        }

        public static DataSet SelectAllCommitmentByID(int? commitmentID)
        {
            string sql = null;
            if (commitmentID == null)
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,    
                                 0 IS_CHANGE,
                                 CC.PERSON_TYPE_ID,
                                 CD.CARD,
                                 CD.REGISTRATION_ADDRESS ADDRESS
                            FROM CRS_USER.CONTRACT_ALL_COMMITMENTS CC,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER.V_COMMITMENT_CARDS CD
                           WHERE     CC.CURRENCY_ID = C.ID                                 
                                 AND CC.ID = CD.COMMITMENT_ID
                        ORDER BY CC.ID";
            else
                sql = $@"SELECT CC.ID,
                                 CC.PARENT_ID,
                                 CC.CONTRACT_ID,
                                 CC.AGREEMENTDATE,
                                 CC.COMMITMENT_NAME,
                                 CC.DEBT,
                                 CC.CURRENCY_ID,
                                 C.CODE CURRENCY_CODE,
                                 CC.PERIOD_DATE,
                                 CC.INTEREST,
                                 CC.ADVANCE_PAYMENT,
                                 CC.SERVICE_AMOUNT,
                                 0 IS_CHANGE,
                                 CC.PERSON_TYPE_ID,
                                 CD.CARD,
                                 CD.REGISTRATION_ADDRESS ADDRESS
                            FROM CRS_USER.CONTRACT_ALL_COMMITMENTS CC,
                                 CRS_USER.CURRENCY C,
                                 CRS_USER.V_COMMITMENT_CARDS CD
                           WHERE     CC.CURRENCY_ID = C.ID                                 
                                 AND CC.ID = CD.COMMITMENT_ID
                                 AND CC.ID = {commitmentID}
                        ORDER BY CC.ID";

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
                GlobalProcedures.LogWrite("Öhdəlik açılmadı.", sql, GlobalVariables.V_UserName, "CommitmentsDAL", "SelectAllCommitmentByContractID", exx);
                return null;
            }
        }
    }
}
