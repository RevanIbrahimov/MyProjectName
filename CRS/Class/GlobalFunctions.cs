using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using CColor = System.Drawing.Color;
using System.Net;
using System.Net.NetworkInformation;
using System.Management;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Globalization;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.Data;
using System.Deployment.Application;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Net.Sockets;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using System.ServiceProcess;
using System.Reflection;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;

namespace CRS.Class
{
    class GlobalFunctions
    {
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
        static string[] ones = new string[] { "", "bir", "iki", "üç", "dörd", "beş", "altı", "yeddi", "səkkiz", "doqquz" };
        static string[] teens = new string[] { "on", "on bir", "on iki", "on üç", "on dörd", "on beş", "on altı", "on yeddi", "on səkkiz", "on doqquz" };
        static string[] tens = new string[] { "iyirmi", "otuz", "qırx", "əlli", "altmış", "yetmiş", "səksən", "doxsan" };
        static string[] thousandsGroups = { "", " min", " milyon", " milyard" };
        static int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        static int year, month, day, increment;
        static DateTime fromDate, toDate;
        static string S = null;

        /// <summary>
        /// Musterinin kodu
        /// </summary>
        /// <returns></returns>
        public static string GenerateCustomerCode()
        {
            int max_code_number = 0, a = 0;
            string code = "00000";
            List<CustomerCodeTemp> lstCustomerCode = CustomerCodeTempDAL.SelectCustomerCode(null).ToList<CustomerCodeTemp>();
            if (lstCustomerCode.Count == 0)
                max_code_number = GetMax("SELECT NVL(MAX(TO_NUMBER(CODE)),0) FROM CRS_USER.V_CUSTOMERS");
            else
                max_code_number = lstCustomerCode.Max(c => c.CODE_NUMBER);

            a = max_code_number + 1;
            code = a.ToString().PadLeft(4, '0');
            if (ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.CUSTOMER_CODE_TEMP(USED_USER_ID,CODE_NUMBER,CODE)
                                        VALUES({GlobalVariables.V_UserID},{a},'{code}')",
                             "Müştərinin maksimum qeydiyyat nömrəsi temp cədvələ daxil edilmədi.") > 0)
                return code;
            else
                return null;
        }

        public static string ReadSetting(string key)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSettings = (AppSettingsSection)cfg.GetSection("appSettings");
            return appSettings.Settings[key].Value.Trim();
        }

        public static string GetConnectionString()
        {
            string ConnectionStringName = "OracleMainDB";
            return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        }

        public static string GetAllMessages(Exception ex, string msg)
        {
            int i = 0;
            if (ex == null)
                throw new ArgumentNullException("ex");

            StringBuilder sb = new StringBuilder();

            if (msg != null)
            {
                if (sb.Length > 0)
                    sb.Append("\r\n");
                sb.Append("ShowMessage: " + msg);
            }

            while (ex != null)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    if (sb.Length > 0)
                        sb.Append("\r\n");

                    if (i == 0)
                        sb.Append("   Exception Message: " + ex.Message);
                    else if (i == 1)
                        sb.Append("   InnerException Message:  " + ex.Message);
                    else if (i > 1)
                        sb.Append("   " + ex.Message);
                }
                i++;
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        public static string GetSID()
        {
            object sid = null;
            string sql = "SELECT SYS_CONTEXT('USERENV','SID') FROM DUAL";
            using (OracleConnection connection = new OracleConnection())
            {
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    command = connection.CreateCommand();
                    command.CommandText = sql;
                    sid = command.ExecuteScalar();
                }
                catch (Exception exx)
                {
                    sid = null;
                    GlobalProcedures.LogWrite("SID tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
            return Convert.ToString(sid);
        }

        public static string GetSERIAL()
        {
            string s = GetSID(), sql = $@"SELECT SERIAL# FROM V$SESSION WHERE SID = {s}";
            object serial = null;
            using (OracleConnection connection = new OracleConnection())
            {
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    command = connection.CreateCommand();
                    command.CommandText = sql;
                    serial = command.ExecuteScalar();
                }
                catch (Exception exx)
                {
                    serial = null;
                    GlobalProcedures.LogWrite("Serial tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
            return Convert.ToString(serial);
        }

        public static Version AssemblyVersion
        {
            get
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
        }

        public static int ExecuteQuery(string sql_text, string message_text, string procedure_name = null)
        {
            int execute_row_count;
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand commond = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    commond = connection.CreateCommand();
                    commond.Transaction = transaction;
                    commond.CommandText = sql_text;
                    execute_row_count = commond.ExecuteNonQuery();
                    transaction.Commit();
                    return execute_row_count;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message_text, sql_text, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                    return -1;
                }
                finally
                {
                    commond.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static int ExecuteTwoQuery(string sql_text1, string sql_text2, string message_text)
        {
            int execute_row_count;
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand commond = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    commond = connection.CreateCommand();
                    commond.Transaction = transaction;
                    commond.CommandText = sql_text1;
                    execute_row_count = commond.ExecuteNonQuery();
                    commond.CommandText = sql_text2;
                    execute_row_count = commond.ExecuteNonQuery();
                    transaction.Commit();
                    return execute_row_count;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message_text, "sql_text1 = " + sql_text1 + ",\r\n sql_text2 = " + sql_text2, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                    return -1;
                }
                finally
                {
                    commond.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static bool ExecuteQueryWithBlob(string sql, string filePath, string message_text, string procedure_name = null)
        {
            if (String.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand commond = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    commond = connection.CreateCommand();
                    commond.Transaction = transaction;
                    FileStream fls = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    byte[] blob = new byte[fls.Length];
                    fls.Read(blob, 0, Convert.ToInt32(fls.Length));
                    fls.Close();
                    commond.CommandText = sql;
                    OracleParameter blobParameter = new OracleParameter();
                    blobParameter.OracleDbType = OracleDbType.Blob;
                    blobParameter.ParameterName = "BlobFile";
                    blobParameter.Value = blob;
                    commond.Parameters.Add(blobParameter);
                    commond.ExecuteNonQuery();
                    transaction.Commit();
                    return true;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message_text, "sql_text = " + sql, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    commond.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static bool ExecuteTwoQueryWithBlob(string sql1, string sql2, string filePath, string message_text, string procedure_name = null)
        {
            if (String.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand commond = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    commond = connection.CreateCommand();
                    commond.Transaction = transaction;
                    commond.CommandText = sql1;
                    commond.ExecuteNonQuery();
                    FileStream fls = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    byte[] blob = new byte[fls.Length];
                    fls.Read(blob, 0, Convert.ToInt32(fls.Length));
                    fls.Close();
                    commond.CommandText = sql2;
                    OracleParameter blobParameter = new OracleParameter();
                    blobParameter.OracleDbType = OracleDbType.Blob;
                    blobParameter.ParameterName = "BlobFile";
                    blobParameter.Value = blob;
                    commond.Parameters.Add(blobParameter);
                    commond.ExecuteNonQuery();
                    transaction.Commit();
                    return true;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message_text, "sql_text1 = " + sql1 + ", sql_text2 = " + sql2, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    commond.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static int GetOracleSequenceValue(string SequenceName)
        {
            int seqVal = -1;
            string sql = $@"SELECT CRS_USER.{SequenceName}.NEXTVAL FROM DUAL";
            using (OracleConnection connection = new OracleConnection())
            {
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    command = connection.CreateCommand();
                    command.CommandText = sql;
                    seqVal = Int32.Parse(command.ExecuteScalar().ToString());
                }
                catch (Exception exx)
                {
                    seqVal = -1;
                    GlobalProcedures.LogWrite("Sequence tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
            return seqVal;
        }

        public static int GetOracleTempSequenceValue(string SequenceName)
        {
            int seqVal = -1;
            string sql = $@"SELECT CRS_USER_TEMP.{SequenceName}.NEXTVAL FROM DUAL";
            using (OracleConnection connection = new OracleConnection())
            {
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    command = connection.CreateCommand();
                    command.CommandText = sql;
                    seqVal = Int32.Parse(command.ExecuteScalar().ToString());
                }
                catch (Exception exx)
                {
                    seqVal = -1;
                    GlobalProcedures.LogWrite("Temp Sequence tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
            return seqVal;
        }

        public static int FindDayOfWeekNumber(string dayofweekname)
        {
            int result = -1;
            try
            {
                switch (dayofweekname)
                {
                    //az
                    case "Bazar ertəsi":
                        result = 1;
                        break;
                    case "Çərsənbə axsamı":
                        result = 2;
                        break;
                    case "Çərsənbə":
                        result = 3;
                        break;
                    case "Cümə axsamı":
                        result = 4;
                        break;
                    case "Cümə":
                        result = 5;
                        break;
                    case "Şənbə":
                        result = 6;
                        break;
                    case "Bazar":
                        result = 7;
                        break;
                    //en
                    case "Monday":
                        result = 1;
                        break;
                    case "Tuesday":
                        result = 2;
                        break;
                    case "Wednesday":
                        result = 3;
                        break;
                    case "Thursday":
                        result = 4;
                        break;
                    case "Friday":
                        result = 5;
                        break;
                    case "Saturday":
                        result = 6;
                        break;
                    case "Sunday":
                        result = 7;
                        break;
                }
            }
            catch (Exception exx)
            {
                result = -1;
                GlobalProcedures.LogWrite("Həftənin günü rəqəm ilə ifadə olunmadı.", dayofweekname, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return result;
        }

        public static string FindMonth(int monthnum)
        {
            string result = null;
            try
            {
                switch (monthnum)
                {
                    //az
                    case 1:
                        result = "yanvar";
                        break;
                    case 2:
                        result = "fevral";
                        break;
                    case 3:
                        result = "mart";
                        break;
                    case 4:
                        result = "aprel";
                        break;
                    case 5:
                        result = "may";
                        break;
                    case 6:
                        result = "iyun";
                        break;
                    case 7:
                        result = "iyul";
                        break;
                    case 8:
                        result = "avqust";
                        break;
                    case 9:
                        result = "sentyabr";
                        break;
                    case 10:
                        result = "oktyabr";
                        break;
                    case 11:
                        result = "noyabr";
                        break;
                    case 12:
                        result = "dekabr";
                        break;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ay yazı ilə ifadə olunmadı.", monthnum.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return result;
        }

        public static string FindYear(DateTime d)
        {
            string result = null, s, a;
            try
            {
                s = d.Year.ToString().Substring(3, 1);
                a = d.Year.ToString().Substring(2, 1);

                if (s == "0")
                    s = a;

                switch (s)
                {
                    case "1":
                        result = d.Year.ToString() + "-ci il";
                        break;
                    case "2":
                        result = d.Year.ToString() + "-ci il";
                        break;
                    case "3":
                        result = d.Year.ToString() + "-cü il";
                        break;
                    case "4":
                        result = d.Year.ToString() + "-cü il";
                        break;
                    case "5":
                        result = d.Year.ToString() + "-ci il";
                        break;
                    case "6":
                        result = d.Year.ToString() + "-cı il";
                        break;
                    case "7":
                        result = d.Year.ToString() + "-ci il";
                        break;
                    case "8":
                        result = d.Year.ToString() + "-ci il";
                        break;
                    case "9":
                        result = d.Year.ToString() + "-cu il";
                        break;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İlin sonuna şəkilçi əlavə olunmadı.", d.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return result;
        }

        public static string FindDateSuffix(DateTime d)
        {
            string result = null, s;
            try
            {
                s = d.Year.ToString().Substring(3, 1);
                switch (s)
                {
                    case "1":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-ci il";
                        break;
                    case "2":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-ci il";
                        break;
                    case "3":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-cü il";
                        break;
                    case "4":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-cü il";
                        break;
                    case "5":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-ci il";
                        break;
                    case "6":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-cı il";
                        break;
                    case "7":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-ci il";
                        break;
                    case "8":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-ci il";
                        break;
                    case "9":
                        result = d.ToString("d", GlobalVariables.V_CultureInfoAZ) + "-cu il";
                        break;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Tarixin sonuna şəkilçi əlavə olunmadı.", d.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return result;
        }

        public static string Encrypt(string originalString)
        {
            try
            {
                if (String.IsNullOrEmpty(originalString))
                {
                    throw new ArgumentNullException
                           ("Boş parametri şifrələmək olmaz.");
                    return " ";
                }
                else
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream,
                        cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cryptoStream);
                    writer.Write(originalString);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();
                    writer.Flush();
                    return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
            catch (Exception exx)
            {
                return " ";
                GlobalProcedures.LogWrite("Şifrələmə zamanı xəta baş verdi.", originalString, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static string Decrypt(string cryptedString)
        {
            try
            {
                if (String.IsNullOrEmpty(cryptedString))
                {
                    throw new ArgumentNullException
                       ("Boş parametrin şifrəsini açmaq olmaz.");
                    return null;
                }
                else
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    MemoryStream memoryStream = new MemoryStream
                            (Convert.FromBase64String(cryptedString));
                    CryptoStream cryptoStream = new CryptoStream(memoryStream,
                        cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cryptoStream);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception exx)
            {
                return " ";
                GlobalProcedures.LogWrite("Şifrənin oxunması zamanı xəta baş verdi.", cryptedString, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static Color CreateColor(string A, string R, string G, string B, string ColorType, string ColorName)
        {
            Color c;
            try
            {
                if (ColorType == "Number")
                    c = Color.FromArgb(int.Parse(A), int.Parse(R), int.Parse(G), int.Parse(B));
                else
                {
                    CColor ctlColor = CColor.FromName(ColorName);
                    c = ctlColor;
                }
            }
            catch (Exception exx)
            {
                c = Color.Black;
                GlobalProcedures.LogWrite("Rəng yaradılmadı.", null, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return c;
        }

        public static Color CreateColor(int colorValue)
        {
            Color c;
            try
            {
                c = Color.FromArgb(colorValue);
            }
            catch (Exception exx)
            {
                c = Color.Black;
                GlobalProcedures.LogWrite("Rəng yaradılmadı.", null, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return c;
        }

        public static Color CreateColor(string hexCode)
        {
            Color c;
            try
            {
                c = ColorTranslator.FromHtml(hexCode);
            }
            catch (Exception exx)
            {
                c = Color.Black;
                GlobalProcedures.LogWrite("Rəng yaradılmadı.", null, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return c;
        }

        private static string FriendlyInteger(double n, string leftDigits, int thousands)
        {
            if (n == 0)
                return leftDigits;

            double number = Math.Truncate(n);

            string friendlyInt = leftDigits.Trim();

            if (friendlyInt.Length > 0)
                friendlyInt += " ";

            if (number < 10)
                friendlyInt += ones[(int)number];
            else if (number < 20)
                friendlyInt += teens[(int)number - 10];
            else if (number < 100)
                friendlyInt += FriendlyInteger(number % 10, tens[(int)number / 10 - 2], 0);
            else if (number < 1000)
                friendlyInt += FriendlyInteger(number % 100, (friendlyInt.Length > 0) ? (((int)number / 100 == 1 ? "" : ones[(int)number / 100]) + " yüz") : (ones[(int)number / 100] + " yüz"), 0);
            else
            {
                friendlyInt += FriendlyInteger(number % 1000, FriendlyInteger(number / 1000, "", thousands + 1), 0);
                if (number % 1000 == 0)
                    return friendlyInt;
            }

            return friendlyInt + thousandsGroups[thousands];
        }

        public static string IntegerToWritten(double n)
        {
            if (n == 0)
                return "sıfır";

            else if (n < 0)
                return "mənfi " + IntegerToWritten(-n);

            return FriendlyInteger(n, "", 0);
        }

        public static string CalculationAgeWithYear(DateTime d1, DateTime d2)
        {
            try
            {
                if (d1 > d2)
                {
                    fromDate = d2;
                    toDate = d1;
                }
                else
                {
                    fromDate = d1;
                    toDate = d2;
                }

                increment = 0;
                if (fromDate.Day > toDate.Day)
                {
                    increment = monthDay[fromDate.Month - 1];
                }

                if (increment == -1)
                {
                    if (DateTime.IsLeapYear(fromDate.Year))
                    {
                        increment = 29;
                    }
                    else
                    {
                        increment = 28;
                    }
                }

                if (increment != 0)
                {
                    day = (toDate.Day + increment) - fromDate.Day;
                    increment = 1;
                }
                else
                {
                    day = toDate.Day - fromDate.Day;
                }

                //month

                if ((fromDate.Month + increment) > toDate.Month)
                {
                    month = (toDate.Month + 12) - (fromDate.Month + increment);
                    increment = 1;
                }
                else
                {
                    month = (toDate.Month) - (fromDate.Month + increment);
                    increment = 0;
                }
                //year

                year = toDate.Year - (fromDate.Year + increment);

                if (year == 0)
                    S = null;
                else
                    S = year + " yaş";
            }
            catch (Exception exx)
            {
                S = null;
                GlobalProcedures.LogWrite("Yaş hesablanmadı.", "d1 = " + d1 + ", d2 = " + d2, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return S;
        }

        public static string GetIPAddress()
        {
            string hostName = null, myIP = null; // Retrive the Name of HOST
            try
            {
                hostName = Dns.GetHostName();
                myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            }
            catch (Exception exx)
            {
                myIP = null;
                GlobalProcedures.LogWrite("IP tapılmadı.", null, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return myIP;
        }

        public static string GetMACAddress()
        {
            string macAddresses = string.Empty;
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        macAddresses += nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
            }
            catch (Exception exx)
            {
                macAddresses = null;
                GlobalProcedures.LogWrite("MAC address tapılmadı.", null, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return macAddresses;
        }

        public static string GetCPUID()
        {
            String cpuid = "";
            try
            {
                ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select ProcessorID From Win32_processor");
                ManagementObjectCollection mbsList = mbs.Get();

                foreach (ManagementObject mo in mbsList)
                {
                    cpuid = mo["ProcessorID"].ToString();
                }
                return cpuid;
            }
            catch (Exception) { return cpuid; }
        }

        public static string GetComputerName()
        {
            string name = "";
            try
            {
                name = SystemInformation.ComputerName;
                return name;
            }
            catch (Exception) { return name; }
        }

        public static bool FindUserName(string UserNikeName)
        {
            bool result = false;
            string sql = null;
            try
            {
                sql = "SELECT ID,NIKNAME FROM CRS_USER.CRS_USERS WHERE STATUS_ID = 1 AND NIKNAME = '" + UserNikeName + "'";
                DataTable dt = GenerateDataTable(sql, "FindUserName");
                foreach (DataRow dr in dt.Rows)
                {
                    GlobalVariables.V_UserID = Convert.ToInt32(dr[0]);
                }

                return (dt.Rows.Count > 0);
            }
            catch (Exception exx)
            {
                result = false;
                GlobalProcedures.LogWrite(UserNikeName + " istifadəçi adı tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", MethodBase.GetCurrentMethod().Name, exx);
            }
            return result;
        }

        public static int FindComboBoxSelectedValue(string tablename, string displeynamecolumn, string valuemembercolumn, string comboboxtext)
        {
            int selectedvalue = 0;
            string sql = null;
            try
            {
                sql = "SELECT " + valuemembercolumn + " FROM CRS_USER." + tablename + " WHERE " + displeynamecolumn + " = '" + comboboxtext + "'";
                foreach (DataRow dr in GenerateDataTable(sql, "FindComboBoxSelectedValue").Rows)
                {
                    if (!String.IsNullOrEmpty(dr[0].ToString()))
                        selectedvalue = Convert.ToInt32(dr[0].ToString());
                    else
                        selectedvalue = 0;
                }
            }
            catch (Exception exx)
            {
                selectedvalue = 0;
                GlobalProcedures.LogWrite(comboboxtext + " siyahıda tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return selectedvalue;
        }

        public static int FindTempComboBoxSelectedValue(string tablename, string displeynamecolumn, string valuemembercolumn, string comboboxtext)
        {
            int selectedvalue = 0;
            string sql = null;
            try
            {
                sql = "SELECT " + valuemembercolumn + " FROM CRS_USER_TEMP." + tablename + " WHERE " + displeynamecolumn + " = '" + comboboxtext + "'";
                foreach (DataRow dr in GenerateDataTable(sql, "FindTempComboBoxSelectedValue").Rows)
                {
                    if (!String.IsNullOrEmpty(dr[0].ToString()))
                        selectedvalue = Convert.ToInt32(dr[0].ToString());
                    else
                        selectedvalue = 0;
                }
            }
            catch (Exception exx)
            {
                selectedvalue = 0;
                GlobalProcedures.LogWrite(comboboxtext + " siyahıda tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return selectedvalue;
        }

        public static int GetID(string sql, string procedure_name = null)
        {
            int id = 0;
            try
            {
                foreach (DataRow dr in GenerateDataTable(sql, "GetID").Rows)
                {
                    if (!String.IsNullOrEmpty(dr[0].ToString()))
                        id = Convert.ToInt32(dr[0].ToString());
                    else
                        id = 0;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("ID tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
            }

            return id;
        }

        public static int GetCount(string sql, string procedure_name = null)
        {
            int id = 0;
            try
            {
                foreach (DataRow dr in GenerateDataTable(sql, "GetCount").Rows)
                {
                    id = Convert.ToInt32(dr[0].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Say tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
            }

            return id;
        }

        public static string GetName(string sql, string procedure_name = null)
        {
            string name = null;
            try
            {
                foreach (DataRow dr in GenerateDataTable(sql, "GetName").Rows)
                {
                    if (!String.IsNullOrEmpty(dr[0].ToString()))
                        name = dr[0].ToString();
                    else
                        name = null;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ad tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
            }

            return name;
        }

        public static double GetAmount(string sql, string procedure_name = null)
        {
            double amount = 0;
            try
            {
                foreach (DataRow dr in GenerateDataTable(sql, "GetAmount").Rows)
                {
                    amount = Convert.ToDouble(dr[0].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Məbləğ tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
            }

            return amount;
        }

        public static int GetMax(string sql, string procedure_name = null)
        {
            int id = 0;
            try
            {
                foreach (DataRow dr in GenerateDataTable(sql, "GetMax").Rows)
                {
                    id = Convert.ToInt32(dr[0].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ən böyük ədəd tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
            }

            return id;
        }

        public static DateTime GetMaxDate(string sql, string procedure_name = null)
        {
            DateTime id = DateTime.Today;
            try
            {
                foreach (DataRow dr in GenerateDataTable(sql, "GetMaxDate").Rows)
                {
                    id = Convert.ToDateTime(dr[0].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Ən böyük tarix tapılmadı.", sql, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
            }

            return id;
        }

        public static DateTime ChangeStringToDate(string datestring, string dateformat)
        {
            DateTime result = DateTime.Today, dt;
            string format = "dd/MM/yyyy";
            try
            {
                if (dateformat == "ddmmyyyy")
                {
                    if (datestring.IndexOf('.') > -1)
                        format = "dd.MM.yyyy";
                    else if (datestring.IndexOf('-') > -1)
                        format = "dd-MM-yyyy";
                    if (DateTime.TryParseExact(datestring, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        result = dt;
                }
                else
                {
                    if (DateTime.TryParseExact(datestring, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        result = dt;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite(datestring + " tarixi " + dateformat + " formatına çevirilmədi.", datestring, GlobalVariables.V_UserName, "GlobalFunctions", MethodBase.GetCurrentMethod().Name, exx);
            }

            return result;
        }

        public static int ConvertDateToInteger(string datestring, string currentformat)
        {
            int result;
            DateTime dt = ChangeStringToDate(datestring, currentformat);
            result = Convert.ToInt32(dt.ToString("yyyyMMdd"));
            return result;
        }

        public static OracleDbType ConvertObjectToOracleDBType(object p)
        {
            OracleDbType res = OracleDbType.Int32;

            try
            {
                if (p is int)
                    res = OracleDbType.Int32;
                else if (p is string)
                    res = OracleDbType.Varchar2;
                else if (p is DateTime)
                    res = OracleDbType.Date;
                else if (p is double)
                    res = OracleDbType.Double;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Object tipi OracleDBType-a çevrilmədi.", p.GetType().ToString(), GlobalVariables.V_UserName, "GlobalFunctions", MethodBase.GetCurrentMethod().Name, exx);
            }

            return res;
        }

        public static string FirstCharToUpper(string input)
        {
            string res = null;
            try
            {
                if (String.IsNullOrEmpty(input))
                    //return String.Empty;
                    res = null;
                else
                    res = input.First().ToString().ToUpper() + input.Substring(1).ToLower();

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sözün birinci hərfi böyük hərfə çevrilmədi.", input, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return res;
        }

        public static double CalcPayment(double presentValue, double Period, double interestRatePerYear)
        {
            double a, b, x, monthlyPayment = 0;
            try
            {
                a = (1 + interestRatePerYear / 1200);
                b = Period;
                x = Math.Pow(a, b);
                x = 1 / x;
                x = 1 - x;
                if (interestRatePerYear != 0)
                    if (x != 0)
                        monthlyPayment = (presentValue) * (interestRatePerYear / 1200) / x;
                    else
                        monthlyPayment = 0;
                else
                    monthlyPayment = 0;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Aylıq ödəniş tapılmadı.", "presentValue = " + presentValue + ", Period = " + Period + ", interestRatePerYear= " + interestRatePerYear, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return (monthlyPayment);
        }

        public static int DifferenceTwoDateWithYear360(DateTime d1, DateTime d2)
        {
            double diff = 0;
            int div = 0;
            try
            {
                diff = (d2 - d1).TotalDays;
                div = (int)diff / 360;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Tarixlərin illərlə fərqi tapılmadı.", "d1 = " + d1 + ", d2 = " + d2, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return div;
        }

        public static int DifferenceTwoDateWithMonth(DateTime d1, DateTime d2)
        {
            double diff = 0;
            int div = 0;
            try
            {
                diff = (d2 - d1).TotalDays;
                div = (int)diff / 30;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Tarixlərin aylarla fərqi tapılmadı.", "d1 = " + d1 + ", d2 = " + d2, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return div;
        }

        public static int DifferenceTwoDateWithDay(DateTime d1, DateTime d2)
        {
            double diff = 0;
            int div = 0;
            try
            {
                diff = (d2 - d1).TotalDays;
                div = (int)diff;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Tarixlərin günlərlə fərqi tapılmadı.", "d1 = " + d1 + ", d2 = " + d2, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return div;
        }

        public static int Days360(DateTime a, DateTime b)
        {
            int res = 0;
            try
            {
                var dayA = a.Day;
                var dayB = b.Day;

                if (IsLastDayOfFebruary(a) && IsLastDayOfFebruary(b))
                    dayB = 30;

                if (dayA == 31 || IsLastDayOfFebruary(a))
                    dayA = 30;

                if (dayA == 30 && dayB == 31)
                    dayB = 30;

                res = ((b.Year - a.Year) * 12 + b.Month - a.Month) * 30 + dayB - dayA;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Tarixlərin 360 günə görə fərqi tapılmadı.", "a = " + a + ", b = " + b, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return res;
        }

        private static bool IsLastDayOfFebruary(DateTime date)
        {
            int lastDay = 0;
            try
            {
                if (date.Month != 2)
                    return false;

                lastDay = DateTime.DaysInMonth(date.Year, 2);
            }
            catch (Exception exx)
            {
                lastDay = 0;
                GlobalProcedures.LogWrite("Fevral ayının son günü tapılmadı.", date.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return date.Day == lastDay;
        }

        public static Color ElementColor()
        {
            Color c;
            Skin currentSkin = CommonSkins.GetSkin(UserLookAndFeel.Default);
            SkinElement element = currentSkin[CommonSkins.SkinLabel];
            c = element.Color.BackColor;

            return c;
        }

        public static double CurrencyLastRate(int currency_id, string date)
        {
            if (currency_id <= 1)
                return 0;

            double res = 0;
            string s = null, candition = null;
            if (!String.IsNullOrEmpty(date))
                candition = $@"AND RATE_DATE = TO_DATE('{date}','DD/MM/YYYY')";

            s = $@"SELECT AMOUNT FROM CRS_USER.CURRENCY_RATES C WHERE CURRENCY_ID = {currency_id} AND RATE_DATE = (SELECT MAX(RATE_DATE) FROM CRS_USER.CURRENCY_RATES WHERE CURRENCY_ID = C.CURRENCY_ID {candition})";
            DataTable dt = GenerateDataTable(s, "CurrencyLastRate");
            if (dt.Rows.Count == 0)
                return 0;

            res = Convert.ToDouble(dt.Rows[0]["AMOUNT"].ToString());

            return res;
        }

        public static double CalculatedExchange(double amount, int cur1, string d1, int cur2, string d2)
        {
            double a = 1, amount1, amount2, res = 0;
            try
            {
                amount1 = CurrencyLastRate(cur1, d1);
                amount2 = CurrencyLastRate(cur2, d2);
                if (cur1 == cur2)
                    a = 1;
                else if ((cur1 != cur2) && (cur1 == 1) && (amount2 != 0))
                    a = 1 / amount2;
                else if ((cur1 != cur2) && (cur2 == 1))
                    a = amount1;
                else if ((cur1 != cur2) && (cur2 != 1) && (amount2 != 0))
                    a = amount1 / amount2;
                else
                    a = 0;
                res = amount * a;
            }
            catch (Exception exx)
            {
                res = 0;
                GlobalProcedures.LogWrite("Məbləğ məzənnə ilə ifadə olunmadı.", amount.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return res;
        }

        public static double ChangeExchange(int cur1, string d1, int cur2, string d2)
        {
            double a = 1, amount1, amount2, res = 0;
            try
            {
                amount1 = CurrencyLastRate(cur1, d1);
                amount2 = CurrencyLastRate(cur2, d2);
                if (cur1 == cur2)
                    a = 1;
                else if ((cur1 != cur2) && (cur1 == 1) && (amount2 != 0))
                    a = 1 / amount2;
                else if ((cur1 != cur2) && (cur2 == 1))
                    a = amount1;
                else if ((cur1 != cur2) && (cur2 != 1) && (amount2 != 0))
                    a = amount1 / amount2;
                res = a;
            }
            catch (Exception exx)
            {
                res = 0;
                GlobalProcedures.LogWrite("Məbləğ məzənnə ilə ifadə olunmadı.", cur1.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return res;
        }

        public static string AmountInWritining(double amount, int currencyid, bool firstchar)
        {
            string res, qep = null, currency_name = null, currency_small_name = null;
            try
            {
                List<Currency> lstCurrency = CurrencyDAL.SelectCurrencyByID(currencyid).ToList<Currency>();
                currency_name = lstCurrency.First().NAME;
                currency_small_name = lstCurrency.First().SMALL_NAME;

                double div = (amount * 100) / 100;
                decimal mod = (decimal)(amount * 100) % 100;

                if (mod >= 0)
                    qep = ", " + mod.ToString().PadLeft(2, '0') + " " + currency_small_name;

                if (firstchar)
                    res = FirstCharToUpper(IntegerToWritten(div)) + " " + currency_name + qep;
                else
                    res = IntegerToWritten(div) + " " + currency_name + qep;
            }
            catch (Exception exx)
            {
                res = null;
                GlobalProcedures.LogWrite("Məbləğ yazı ilə ifadə olunmadı.", amount.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            return res;
        }

        public static string StringRight(string original, int numberCharacters)
        {
            return original.Substring(original.Length - numberCharacters);
        }

        public static Boolean IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public static DataTable GenerateDataTable(string s, string procedureName = null, string message = null)
        {
            try
            {
                using (OracleDataAdapter da = new OracleDataAdapter(s, GetConnectionString()))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                GlobalProcedures.LogWrite((message == null) ? "DataTable yaradılmadı." : message, s, GlobalVariables.V_UserName, "GlobalFunctions", (procedureName == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + " : " + procedureName, exx);
                return null;
            }
        }

        public static DataSet GenerateDataSet(string s, string procedureName = null, string message = null)
        {
            try
            {
                using (OracleDataAdapter da = new OracleDataAdapter(s, GetConnectionString()))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite((message == null) ? "DataSet yaradılmadı." : message, s, GlobalVariables.V_UserName, "GlobalFunctions", (procedureName == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + " : " + procedureName, exx);
                return null;
            }
        }

        public static Int32 InsertQuery(string sql_text, string message_text, string parametrID, string procedure_name = null)
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand commond = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    commond = connection.CreateCommand();
                    commond.Transaction = transaction;
                    commond.CommandText = sql_text;
                    commond.Parameters.Add(parametrID, OracleDbType.Int32);
                    commond.ExecuteNonQuery();
                    transaction.Commit();
                    return Convert.ToInt32(commond.Parameters[parametrID].Value.ToString());
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message_text, sql_text, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                    return 0;
                }
                finally
                {
                    commond.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static Int32 InsertTwoQuery(string sql1, string sql2, string message_text, string parametrID, string procedure_name = null)
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand commond = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    commond = connection.CreateCommand();
                    commond.Transaction = transaction;
                    commond.CommandText = sql1;
                    commond.ExecuteNonQuery();
                    commond.CommandText = sql2;
                    commond.Parameters.Add(parametrID, OracleDbType.Int32);
                    commond.ExecuteNonQuery();
                    transaction.Commit();
                    return Convert.ToInt32(commond.Parameters[parametrID].Value.ToString());
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message_text, "sql1 = " + sql1 + ",\r\n sql2 = " + sql2, GlobalVariables.V_UserName, "GlobalFunctions", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                    return 0;
                }
                finally
                {
                    commond.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static string DateWithDayMonthYear(DateTime date)
        {
            return String.Format("{0:dd}", date) + " " + FindMonth(date.Month) + " " + FindYear(date);
        }

        public static string DateWithYear(DateTime date)
        {
            string s = date.Year.ToString().Substring(3, 1),
                  result = null;

            switch (s)
            {
                case "1":
                    result = date.ToString("dd.MM.yyyy") + "-ci il";
                    break;
                case "2":
                    result = date.ToString("dd.MM.yyyy") + "-ci il";
                    break;
                case "3":
                    result = date.ToString("dd.MM.yyyy") + "-cü il";
                    break;
                case "4":
                    result = date.ToString("dd.MM.yyyy") + "-cü il";
                    break;
                case "5":
                    result = date.ToString("dd.MM.yyyy") + "-ci il";
                    break;
                case "6":
                    result = date.ToString("dd.MM.yyyy") + "-cı il";
                    break;
                case "7":
                    result = date.ToString("dd.MM.yyyy") + "-ci il";
                    break;
                case "8":
                    result = date.ToString("dd.MM.yyyy") + "-ci il";
                    break;
                case "9":
                    result = date.ToString("dd.MM.yyyy") + "-cu il";
                    break;
            }

            return result;
        }

        public static bool ConnectDataBase()
        {
            using (OracleConnection connection = new OracleConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    return true;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.ShowErrorMessage("Baza ilə əlaqə yoxdur...", exx);
                    return false;
                    throw;
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Servisin statusunun yoxlanilmasi
        /// </summary>
        /// <param name="service_name"></param>
        /// <returns></returns>
        public static string ControlServiceStatus(string service_name)
        {
            string svcStatus = null;
            ServiceController myService = new ServiceController();
            myService.ServiceName = service_name;
            svcStatus = myService.Status.ToString();

            return svcStatus;
        }

        public static bool StartService(string service_name)
        {
            ServiceController myService = new ServiceController();
            myService.ServiceName = service_name;
            string svcStatus = myService.Status.ToString();
            try
            {
                if (svcStatus == "Running")
                    myService.Refresh();
                else if (svcStatus == "Stopped")
                {
                    myService.Start();
                    while (svcStatus != "Running")
                    {
                        myService.Refresh();
                        svcStatus = myService.Status.ToString();
                    }
                }
                else
                {
                    myService.Stop();
                    while (svcStatus != "Stopped")
                    {
                        myService.Refresh();
                        svcStatus = myService.Status.ToString();
                    }
                }
            }
            catch
            {
                return false;
            }

            return (myService.Status.ToString() == "Running");
        }

        public static bool StopService(string service_name)
        {
            ServiceController myService = new ServiceController();
            myService.ServiceName = service_name;
            string svcStatus = myService.Status.ToString();
            bool stop = false;

            if (svcStatus == "Running" || svcStatus != "Stopped")
            {
                while (svcStatus != "Stopped")
                {
                    myService.Refresh();
                    svcStatus = myService.Status.ToString();
                    if (svcStatus == "Running")
                        break;
                }

                if (svcStatus == "Running")
                    myService.Stop();
                stop = true;
            }

            return stop;
        }

        public static object ExecuteProcedureWithInParametrAndOutParametr(string procedure_name, string in_parametr_name, object in_parametr_value, string out_parametr_name, OracleDbType out_parametr_type, string message)
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = procedure_name;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(in_parametr_name, ConvertObjectToOracleDBType(in_parametr_value)).Value = in_parametr_value;
                    OracleParameter par = new OracleParameter();
                    par = command.Parameters.Add(out_parametr_name, out_parametr_type, ParameterDirection.Output);
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    return par.Value;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalFucntions", MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                    return 0;
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static double FindContractResidualPercent(int contractID)
        {
            double debt = 0, one_day_interest, interestValue, residualPercentValue = 0;
            int diff_day = 0, interest;
            
            DateTime StartDate, LastDate;
            try
            {
                List<Contract> lstContract = ContractDAL.SelectContract(contractID).ToList<Contract>();
                if (lstContract.Count == 0)
                    return 0;

                var contract = lstContract.First();
                LastDate = contract.START_DATE;
                debt = (double)contract.AMOUNT;
                StartDate = contract.START_DATE;
                interest = (contract.CHECK_INTEREST > 0) ? contract.CHECK_INTEREST : contract.INTEREST;


                List<Payments> lstPayments = PaymentsDAL.SelectPayments(0, contractID).ToList<Payments>();
                if (lstPayments.Count > 0)                
                {
                    var lastPayments = lstPayments.LastOrDefault();

                    LastDate = lastPayments.PAYMENT_DATE;
                    debt = Math.Round((double)lastPayments.DEBT, 2);
                }

                diff_day = GlobalFunctions.Days360(LastDate, DateTime.Today);
                if (diff_day > 0)
                {
                    one_day_interest = Math.Round(((debt * interest) / 100) / 360, 2);
                    interestValue = diff_day * one_day_interest;
                    residualPercentValue = interestValue + GlobalFunctions.GetAmount($@"SELECT NVL(SUM(INTEREST_AMOUNT),0) - NVL(SUM(PAYMENT_INTEREST_AMOUNT),0) FROM CRS_USER.CUSTOMER_PAYMENTS WHERE CONTRACT_ID = {contractID}", "/FindContractResidualPercent");
                }

                return residualPercentValue;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müqavilənin qalıq faizi tapılmadı", null, GlobalVariables.V_UserName, "GlobalFunctions", MethodBase.GetCurrentMethod().Name, exx);
                return 0;
            }            
        }

        public static double FindContractLastDebt(int contractID)
        {
            double debt = 0;
           
            try
            {
                List<Contract> lstContract = ContractDAL.SelectContract(contractID).ToList<Contract>();
                if (lstContract.Count == 0)
                    return 0;

                var contract = lstContract.First();                
                debt = (double)contract.AMOUNT;   

                List<Payments> lstPayments = PaymentsDAL.SelectPayments(0, contractID).ToList<Payments>();
                if (lstPayments.Count > 0)
                {
                    var lastPayments = lstPayments.LastOrDefault();
                    
                    debt = Math.Round((double)lastPayments.DEBT, 2);
                }

                return debt;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müqavilənin qalığı tapılmadı", null, GlobalVariables.V_UserName, "GlobalFunctions", MethodBase.GetCurrentMethod().Name, exx);
                return 0;
            }
        }

        public static double ContractStartPercent(int contractID)
        {
            string value = ExecuteProcedureWithInParametrAndOutParametr("CRS_USER.PROC_CONTRACT_START_PERCENT", "P_CONTRACT_ID", contractID, "P_START_PERCENT", OracleDbType.Double, "Müqavilənin başlanğıc qalıq faizi tapılmadı.").ToString();

            return ConvertStringToDouble(value);            
        }

        public static double ConvertStringToDouble(string a)
        {
            try
            {
                return double.Parse(a, GlobalVariables.V_CultureInfoEN);
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("String number-yə çevrilmədi.", a, GlobalVariables.V_UserName, "GlobalFunctions", MethodBase.GetCurrentMethod().Name, exx);
                return 0;
            }
        }

        public static bool Regexp(string re, string text)
        {
            Regex regex = new Regex(re);
            return regex.IsMatch(text); ;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static int GetLookUpValueMember(LookUpEdit lookUp)
        {
            if (lookUp.EditValue == null)
                return 0;

            return Convert.ToInt32(lookUp.EditValue);
        }

        public static decimal SumSelectedRow(DevExpress.XtraGrid.Views.Grid.GridView view, string fieldName)
        {
            decimal sumAmount = 0;
            for (int i = 0; i < view.SelectedRowsCount; i++)
            {
                int rowHandle = view.GetSelectedRows()[i];
                if (!view.IsGroupRow(rowHandle))
                    sumAmount += Convert.ToDecimal(view.GetDataRow(rowHandle)[fieldName]);
            }

            return sumAmount;
        }

        public static int CountSelectedRow(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            int count = 0;
            for (int i = 0; i < view.SelectedRowsCount; i++)
            {
                int rowHandle = view.GetSelectedRows()[i];
                if (!view.IsGroupRow(rowHandle))
                    count++;
            }

            return count;
        }

        public static object GridGetRowCellValue(GridView view, string columnName)
        {
            return view.GetRowCellValue(view.FocusedRowHandle, columnName);
        }

        public static DateTime GetDate(string sql, string procedure_name = null)
        {
            DateTime date = DateTime.Today;
            DataTable dt = GenerateDataTable(sql, "GetDate", "Tarix tapılmadı.");
            if (dt.Rows[0][0] != DBNull.Value)
                date = Convert.ToDateTime(dt.Rows[0][0]);

            return date;
        }

        public static DateTime LastClosedDay()
        {
            return GetDate("SELECT MAX(CLOSED_DAY) FROM CRS_USER.CLOSED_DAYS");
        }

        public static string RightString(string input, int count)
        {
            return input.Substring(Math.Max(input.Length - count, 0), Math.Min(count, input.Length));
        }

        public static string LeftString(string input, int count)
        {
            return input.Substring(0, Math.Min(input.Length, count));
        }

        public static string DayWithSuffix1(int d)
        {
            string result = null, s, f = null;
            try
            {
                s = RightString(d.ToString(), 1);
                f = LeftString(d.ToString(), 1);
                if (s != "0")
                    switch (s)
                    {
                        case "1":
                            result = d.ToString() + "-nə";
                            break;
                        case "2":
                            result = d.ToString() + "-nə";
                            break;
                        case "3":
                            result = d.ToString() + "-nə";
                            break;
                        case "4":
                            result = d.ToString() + "-nə";
                            break;
                        case "5":
                            result = d.ToString() + "-nə";
                            break;
                        case "6":
                            result = d.ToString() + "-na";
                            break;
                        case "7":
                            result = d.ToString() + "-nə";
                            break;
                        case "8":
                            result = d.ToString() + "-nə";
                            break;
                        case "9":
                            result = d.ToString() + "-na";
                            break;
                    }
                else
                    switch (f)
                    {
                        case "1":
                            result = d.ToString() + "-na";
                            break;
                        case "2":
                            result = d.ToString() + "-nə";
                            break;
                        case "3":
                            result = d.ToString() + "-na";
                            break;
                        case "4":
                            result = d.ToString() + "-na";
                            break;
                        case "5":
                            result = d.ToString() + "-nə";
                            break;
                        case "6":
                            result = d.ToString() + "-na";
                            break;
                        case "7":
                            result = d.ToString() + "-nə";
                            break;
                        case "8":
                            result = d.ToString() + "-nə";
                            break;
                        case "9":
                            result = d.ToString() + "-na";
                            break;
                    }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Günün sonuna şəkilçi əlavə olunmadı.", d.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return result;
        }

        public static string DayWithSuffix2(int d)
        {
            string result = null, s, f = null;
            try
            {
                s = RightString(d.ToString(), 1);
                f = LeftString(d.ToString(), 1);
                if (s != "0")
                    switch (s)
                    {
                        case "1":
                            result = d.ToString() + "-i";
                            break;
                        case "2":
                            result = d.ToString() + "-i";
                            break;
                        case "3":
                            result = d.ToString() + "-ü";
                            break;
                        case "4":
                            result = d.ToString() + "-ü";
                            break;
                        case "5":
                            result = d.ToString() + "-i";
                            break;
                        case "6":
                            result = d.ToString() + "-ı";
                            break;
                        case "7":
                            result = d.ToString() + "-i";
                            break;
                        case "8":
                            result = d.ToString() + "-i";
                            break;
                        case "9":
                            result = d.ToString() + "-u";
                            break;
                    }
                else
                    switch (f)
                    {
                        case "1":
                            result = d.ToString() + "-nu";
                            break;
                        case "2":
                            result = d.ToString() + "-si";
                            break;
                        case "3":
                            result = d.ToString() + "-u";
                            break;
                        case "4":
                            result = d.ToString() + "-ı";
                            break;
                        case "5":
                            result = d.ToString() + "-i";
                            break;
                        case "6":
                            result = d.ToString() + "-ı";
                            break;
                        case "7":
                            result = d.ToString() + "-i";
                            break;
                        case "8":
                            result = d.ToString() + "-ni";
                            break;
                        case "9":
                            result = d.ToString() + "-nı";
                            break;
                    }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Günün sonuna şəkilçi əlavə olunmadı.", d.ToString(), GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }

            return result;
        }

        public static int ExecuteFourQuery(string sql_text1, string sql_text2, string sql_text3, string sql_text4, string message_text, string procedure_name = null)
        {
            int execute_row_count;
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand commond = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    commond = connection.CreateCommand();
                    commond.Transaction = transaction;
                    commond.CommandText = sql_text1;
                    execute_row_count = commond.ExecuteNonQuery();
                    commond.CommandText = sql_text2;
                    execute_row_count = commond.ExecuteNonQuery();
                    commond.CommandText = sql_text3;
                    execute_row_count = commond.ExecuteNonQuery();
                    commond.CommandText = sql_text4;
                    execute_row_count = commond.ExecuteNonQuery();
                    transaction.Commit();
                    return execute_row_count;
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite(message_text, "sql_text1 = " + sql_text1 + ",\r\n sql_text2 = " + sql_text2 + ",\r\n sql_text3 = " + sql_text3 + ",\r\n sql_text4 = " + sql_text4, GlobalVariables.V_UserName, "GlobalFunctions", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                    return -1;
                }
                finally
                {
                    commond.Dispose();
                    connection.Dispose();
                }
            }
        }



    }
}
