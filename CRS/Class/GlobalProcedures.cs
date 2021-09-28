using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraPivotGrid;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using DevExpress.Utils;
using System.Xml;
using System.Drawing;
using Bytescout.Document;
using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using Tulpep.NotificationWindow;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using System.Net;

namespace CRS.Class
{
    class GlobalProcedures
    {
        private static ReportViewer rv_expenditure = new ReportViewer();

        public static void SetSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var entry = config.AppSettings.Settings[key];
            if (entry == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static void InsertErrorMessage(string Sql, string UserName, string FormName, string ProcedureName, Exception ex)
        {
            if (ex.Message == null)
                return;

            using (OracleConnection connection = new OracleConnection())
            {
                string message = ("Message:" + ex.Message + ((ex.InnerException != null) ? "\r\nInnerException:\r\n" + ex.InnerException : null) + ((ex.StackTrace != null) ? "\r\nStack Trace:\r\n" + ex.StackTrace : null)).Trim(),
                            error_text = (message.Length > 4000) ? message.ToString().Substring(0, 3999) : message,
                sql = $@"INSERT INTO CRS_USER.LRS_ERRORS(USER_NAME,FORM_NAME,PROCEDURE_NAME,ERROR_TEXT,SQL)
                                    VALUES('{UserName}','{FormName}','{ProcedureName}','{error_text}',:SQL)";

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
                    command.CommandText = sql;
                    command.Parameters.Add("SQL", OracleDbType.Clob, Sql, ParameterDirection.Input);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.ShowErrorMessage(exx.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private static void CreateErrorMessage(string logMessage, TextWriter txtWriter, string Sql, string UserName, string FormName, string ProcedureName, Exception ex)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  ");
                if (UserName != null)
                    txtWriter.WriteLine("   {0} {1}", "UserName = ", UserName);
                txtWriter.WriteLine("   {0} {1}", "FormName = ", FormName);
                txtWriter.WriteLine("   {0} {1}", "ProcedureName = ", ProcedureName);
                txtWriter.WriteLine("  ");
                txtWriter.WriteLine("   {0}", GlobalFunctions.GetAllMessages(ex, logMessage));
                if (Sql != null)
                    txtWriter.WriteLine("   {0} {1}", "SQL Text      : ", Sql);
                txtWriter.WriteLine("-------------------------------------------------");
            }
            catch (Exception exx)
            {
                LogWrite("Səhv log fayla yazılmadı.", null, GlobalVariables.V_UserName, "GlobalProcedures", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void AddToFile(string logMessage)
        {
            string filename = "\\Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string path = GlobalVariables.V_ExecutingFolder + "\\Logs";
            string fullfilepath = path + filename;

            if (!Directory.Exists(path)) //No Folder? Create 
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(fullfilepath)) //No File? Create
            {
                StreamWriter sw = File.CreateText(fullfilepath);
                sw.Close();
            }
            using (StreamWriter w = File.AppendText(fullfilepath))
            {
                w.WriteLine(logMessage);
            }
        }

        public static void LogWrite(string logMessage, string Sql, string UserName, string FormName, string ProcedureName, Exception exception)
        {
            string filename = "\\ErrorLog_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string path = GlobalVariables.V_ExecutingFolder + "\\Logs";
            string fullfilepath = path + filename;

            if (!Directory.Exists(path)) //No Folder? Create 
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(fullfilepath)) //No File? Create
            {
                StreamWriter sw = File.CreateText(fullfilepath);
                sw.Close();
            }
            using (StreamWriter w = File.AppendText(fullfilepath))
            {
                CreateErrorMessage(logMessage, w, Sql, UserName, FormName, ProcedureName, exception);
                InsertErrorMessage(Sql, UserName, FormName, ProcedureName, exception);
                XtraMessageBox.Show(logMessage, "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void GridCustomColumnDisplayText(DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption == "S/s")
                e.DisplayText = (e.ListSourceRowIndex + 1).ToString();
        }

        public static void ShowErrorMessage(string message, Exception exx = null)
        {
            XtraMessageBox.Show(message + (exx != null ? "\r\n\r\nErrorMessage: " + exx.Message : null), "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningMessage(string message)
        {
            XtraMessageBox.Show(message, "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowNotification(string titleText, string text)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.info_32;
            popup.TitleText = titleText;
            popup.ContentText = text;
            popup.TitleColor = Color.Red;
            popup.TitleFont = new Font(popup.TitleFont, FontStyle.Bold);
            popup.AnimationDuration = 2000;
            popup.ShowCloseButton = true;
            //FontFamily fontFamily = new FontFamily("Arial");
            //Font font = new Font(
            //   fontFamily,
            //   16,
            //   FontStyle.Bold,
            //   GraphicsUnit.Pixel);
            //popup.TitleFont = font;
            popup.Popup();
        }

        public static void ExecuteQuery(string sql_text, string message_text, string procedure_name = null)
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
                    command.CommandText = sql_text;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message_text, sql_text, GlobalVariables.V_UserName, "GlobalProcedures", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteTwoQuery(string sql_text1, string sql_text2, string message_text, string procedure_name = null)
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
                    command.CommandText = sql_text1;
                    command.ExecuteNonQuery();
                    command.CommandText = sql_text2;
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message_text, "sql_text1 = " + sql_text1 + ",\r\n sql_text2 = " + sql_text2, GlobalVariables.V_UserName, "GlobalProcedures", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteThreeQuery(string sql_text1, string sql_text2, string sql_text3, string message_text, string procedure_name = null)
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
                    command.CommandText = sql_text1;
                    command.ExecuteNonQuery();
                    command.CommandText = sql_text2;
                    command.ExecuteNonQuery();
                    command.CommandText = sql_text3;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message_text + " { " + exx.Message + " }", "sql_text1 = " + sql_text1 + ", sql_text2 = " + sql_text2 + ", sql_text3 = " + sql_text3, GlobalVariables.V_UserName, "GlobalProcedures", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteFourQuery(string sql_text1, string sql_text2, string sql_text3, string sql_text4, string message_text, string procedure_name = null)
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
                    command.CommandText = sql_text1;
                    command.ExecuteNonQuery();
                    command.CommandText = sql_text2;
                    command.ExecuteNonQuery();
                    command.CommandText = sql_text3;
                    command.ExecuteNonQuery();
                    command.CommandText = sql_text4;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message_text + " { " + exx.Message + " }", "sql_text1 = " + sql_text1 + ", sql_text2 = " + sql_text2 + ", sql_text3 = " + sql_text3 + ", sql_text4 = " + sql_text4, GlobalVariables.V_UserName, "GlobalProcedures", (procedure_name == null) ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteProcedure(string procedure_name, string message)
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
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteProcedureWithParametr(string procedure_name, string parametr_name, object parametr_value, string message)
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
                    command.Parameters.Add(parametr_name, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value)).Value = parametr_value;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteProcedureWithTwoParametr(string procedure_name, string parametr1_name, object parametr1_value, string parametr2_name, object parametr2_value, string message)
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
                    command.Parameters.Add(parametr1_name, GlobalFunctions.ConvertObjectToOracleDBType(parametr1_value)).Value = parametr1_value;
                    command.Parameters.Add(parametr2_name, GlobalFunctions.ConvertObjectToOracleDBType(parametr2_value)).Value = parametr2_value;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteProcedureWithUser(string procedure_name, string parametr_name, object parametr_value, string message)
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
                    command.Parameters.Add(parametr_name, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value)).Value = parametr_value;
                    command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteProcedureWithTwoParametrAndUser(string procedure_name, string parametr_name1, object parametr_value1, string parametr_name2, object parametr_value2, string message)
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
                    command.Parameters.Add(parametr_name1, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value1)).Value = parametr_value1;
                    command.Parameters.Add(parametr_name2, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value2)).Value = parametr_value2;
                    command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteProcedureWithThreeParametrs(string procedure_name, string parametr_name1, object parametr_value1, string parametr_name2, object parametr_value2, string parametr_name3, object parametr_value3, string message)
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
                    command.Parameters.Add(parametr_name1, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value1)).Value = parametr_value1;
                    command.Parameters.Add(parametr_name2, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value2)).Value = parametr_value2;
                    command.Parameters.Add(parametr_name3, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value3)).Value = parametr_value3;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void ExecuteProcedureWithThreeParametrAndUser(string procedure_name, string parametr_name1, object parametr_value1, string parametr_name2, object parametr_value2, string parametr_name3, object parametr_value3, string message)
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
                    command.Parameters.Add(parametr_name1, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value1)).Value = parametr_value1;
                    command.Parameters.Add(parametr_name2, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value2)).Value = parametr_value2;
                    command.Parameters.Add(parametr_name3, GlobalFunctions.ConvertObjectToOracleDBType(parametr_value3)).Value = parametr_value3;
                    command.Parameters.Add("P_USED_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite(message, command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name + "/" + procedure_name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void CalculatedLeasingTotal(object contract_id)
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
                    command.CommandText = "CRS_USER.PROC_LEASING_TOTAL";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_CONTRACT_ID", OracleDbType.Int32).Value = contract_id;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite("Lizinq Portfeli hesablanmadı.", "PROC_LEASING_TOTAL", GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void CalculatedAttractedFundsTotal(object contract_id)
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
                    command.CommandText = "CRS_USER.PROC_ATTRACTED_FUNDS_TOTAL";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_CONTRACT_ID", OracleDbType.Int32).Value = contract_id;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite("Сəlb olunmuş vəsaitlərin portfeli hesablanmadı.", "PROC_ATTRACTED_FUNDS_TOTAL", GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void GridMouseUpForPopupMenu(GridView View, PopupMenu Menu, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridHitInfo hi = View.CalcHitInfo(e.Location);
                GridFooterCellInfoArgs footerInfo = hi.FooterCell;
                if (footerInfo == null)
                {
                    if (!hi.InColumn)
                        Menu.ShowPopup(Control.MousePosition);
                }
            }
        }

        public static void GridMouseUpForRadialMenu(GridView View, RadialMenu Menu, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridHitInfo hi = View.CalcHitInfo(e.Location);
                GridFooterCellInfoArgs footerInfo = hi.FooterCell;
                if (footerInfo == null)
                {
                    if (!hi.InColumn)
                        Menu.ShowPopup(Control.MousePosition);
                }
            }
        }

        public static void GridExportToFile(GridControl gridControl, string fileExtenstion, bool showPrintable = true)
        {
            bool selectedColumn = true;
            if (gridControl == null)
                return;

            var view = (gridControl.MainView as GridView);

            if (showPrintable)
            {
                List<ColumnPrintable> lstColumnName = new List<ColumnPrintable>();
                for (int i = 0; i < view.Columns.Count; i++)
                {
                    if (view.Columns[i].Visible == false)
                        continue;
                    lstColumnName.Add(new ColumnPrintable(i, view.Columns[i].Caption));
                }

                FPrintableCheck fp = new FPrintableCheck();
                fp.ListColumnName = lstColumnName;
                fp.RefreshDataGridView += new FPrintableCheck.DoEvent(RefreshPrintable);
                fp.ShowDialog();

                void RefreshPrintable(List<int> lstColumnIndex)
                {
                    if (lstColumnIndex.Count == 0)
                    {
                        selectedColumn = false;
                        return;
                    }

                    for (int i = 0; i < view.Columns.Count; i++)
                    {
                        if (view.Columns[i].Visible == false)
                            continue;

                        if (!lstColumnIndex.Contains(i))
                            view.Columns[i].OptionsColumn.Printable = DefaultBoolean.False;
                    }
                }
            }

            if (!selectedColumn)
                return;

            string filter = null;
            switch (fileExtenstion)
            {
                case "xls":
                    filter = "Excel (2003)(.xls)|*.xls";
                    break;
                case "xlsx":
                    filter = "Excel (2010) (.xlsx)|*.xlsx";
                    break;
                case "rtf":
                    filter = "RichText faylı (.rtf)|*.rtf";
                    break;
                case "pdf":
                    filter = "Pdf faylı (.pdf)|*.pdf";
                    break;
                case "html":
                    filter = "Html faylı (.html)|*.html";
                    break;
                case "mht":
                    filter = "Mht faylı (.mht)|*.mht";
                    break;
                case "txt":
                    filter = "Text faylı (.txt)|*.txt";
                    break;
                case "csv":
                    filter = "Csv faylı (.csv)|*.csv";
                    break;
                default: break;
            }

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    if (!String.IsNullOrWhiteSpace(view.ViewCaption))
                        saveDialog.FileName = view.ViewCaption.Replace(":", "") + "." + fileExtenstion;

                    saveDialog.Filter = filter;
                    if (saveDialog.ShowDialog() != DialogResult.Cancel)
                    {
                        string exportFilePath = saveDialog.FileName;
                        switch (fileExtenstion)
                        {
                            case "xls":
                                gridControl.ExportToXls(exportFilePath);
                                break;
                            case "xlsx":
                                gridControl.ExportToXlsx(exportFilePath);
                                break;
                            case "rtf":
                                gridControl.ExportToRtf(exportFilePath);
                                break;
                            case "pdf":
                                gridControl.ExportToPdf(exportFilePath);
                                break;
                            case "html":
                                gridControl.ExportToHtml(exportFilePath);
                                break;
                            case "mht":
                                gridControl.ExportToMht(exportFilePath);
                                break;
                            case "txt":
                                gridControl.ExportToTextOld(exportFilePath);
                                break;
                            case "csv":
                                gridControl.ExportToCsv(exportFilePath);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                LogWrite(gridControl.Views[0].ViewCaption + "." + fileExtenstion + " faylı yaradılmadı.", gridControl.Name, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
            finally
            {
                for (int i = 0; i < view.Columns.Count; i++)
                {
                    if (view.Columns[i].Visible == false)
                        continue;

                    view.Columns[i].OptionsColumn.Printable = DefaultBoolean.Default;
                }
            }
        }

        public static void TreeListExportToFile(TreeList treeList, string fileExtenstion)
        {
            if (treeList == null)
                return;

            string filter = null;
            switch (fileExtenstion)
            {
                case "xls":
                    filter = "Excel (2003)(.xls)|*.xls";
                    break;
                case "xlsx":
                    filter = "Excel (2010) (.xlsx)|*.xlsx";
                    break;
                case "rtf":
                    filter = "RichText faylı (.rtf)|*.rtf";
                    break;
                case "pdf":
                    filter = "Pdf faylı (.pdf)|*.pdf";
                    break;
                case "html":
                    filter = "Html faylı (.html)|*.html";
                    break;
                case "mht":
                    filter = "Mht faylı (.mht)|*.mht";
                    break;
                case "txt":
                    filter = "Text faylı (.txt)|*.txt";
                    break;
                case "csv":
                    filter = "Csv faylı (.csv)|*.csv";
                    break;
                default: break;
            }

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = filter;
                    if (saveDialog.ShowDialog() != DialogResult.Cancel)
                    {
                        string exportFilePath = saveDialog.FileName;
                        switch (fileExtenstion)
                        {
                            case "xls":
                                treeList.ExportToXls(exportFilePath);
                                break;
                            case "xlsx":
                                treeList.ExportToXlsx(exportFilePath);
                                break;
                            case "rtf":
                                treeList.ExportToRtf(exportFilePath);
                                break;
                            case "pdf":
                                treeList.ExportToPdf(exportFilePath);
                                break;
                            case "html":
                                treeList.ExportToHtml(exportFilePath);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                LogWrite("TreeList üçün " + fileExtenstion + " faylı yaradılmadı.", treeList.Name, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void GridRowCellStyleForBlock(GridView View, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                int used_user_id = int.Parse(View.GetRowCellDisplayText(e.RowHandle, View.Columns["USED_USER_ID"]));
                if (used_user_id >= 0)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_BlockColor1);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_BlockColor1);
                }
            }
        }

        public static void GridRowCellStyleForNotCommit(GridView View, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                int is_commit = int.Parse(View.GetRowCellDisplayText(e.RowHandle, View.Columns["IS_COMMIT"]));
                if (is_commit == 0)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor1);  //GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor_A, GlobalVariables.V_CommitColor_R, GlobalVariables.V_CommitColor_G, GlobalVariables.V_CommitColor_B, GlobalVariables.V_CommitColor_Type, GlobalVariables.V_CommitColor_Name);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor2); //GlobalFunctions.CreateColor(GlobalVariables.V_CommitColor2_A, GlobalVariables.V_CommitColor2_R, GlobalVariables.V_CommitColor2_G, GlobalVariables.V_CommitColor2_B, GlobalVariables.V_CommitColor2_Type, GlobalVariables.V_CommitColor2_Name);
                }
            }
        }

        public static void GridRowCellStyleForConnect(GridView View, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                int connect_id = int.Parse(View.GetRowCellDisplayText(e.RowHandle, View.Columns["SESSION_ID"]));
                if (connect_id > 0)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_ConnectColor1); //GlobalFunctions.CreateColor(GlobalVariables.V_ConnectColor_A, GlobalVariables.V_ConnectColor_R, GlobalVariables.V_ConnectColor_G, GlobalVariables.V_ConnectColor_B, GlobalVariables.V_ConnectColor_Type, GlobalVariables.V_ConnectColor_Name);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_ConnectColor2); //GlobalFunctions.CreateColor(GlobalVariables.V_ConnectColor2_A, GlobalVariables.V_ConnectColor2_R, GlobalVariables.V_ConnectColor2_G, GlobalVariables.V_ConnectColor2_B, GlobalVariables.V_ConnectColor2_Type, GlobalVariables.V_ConnectColor2_Name);
                }
            }
        }

        public static void GridRowCellStyleForClose(int statusid, GridView View, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                int status_id = int.Parse(View.GetRowCellDisplayText(e.RowHandle, View.Columns["STATUS_ID"]));
                if (status_id == statusid)
                {
                    e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor1);
                    e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_CloseColor2);
                }
            }
        }

        public static void GridRowOperationTypeColor(int operationTypeID, RowStyleEventArgs e)
        {
            if (operationTypeID == 1)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_IncomeColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_IncomeColor2);
            }
            else
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_ExpensesColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_ExpensesColor2);
            }
        }

        public static void GridRowOperationTypeColor(int operationTypeID, RowCellStyleEventArgs e)
        {
            if (operationTypeID == 1)
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_IncomeColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_IncomeColor2);
            }
            else
            {
                e.Appearance.BackColor = GlobalFunctions.CreateColor(GlobalVariables.V_ExpensesColor1);
                e.Appearance.BackColor2 = GlobalFunctions.CreateColor(GlobalVariables.V_ExpensesColor2);
            }
        }

        public static void GridCustomDrawFooterCell(string fieldname, string horzalignment, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == fieldname)
            {
                switch (horzalignment)
                {
                    case "Center":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Center; //merkez
                        break;
                    case "Default":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Default;
                        break;
                    case "Far":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Far; //sag
                        break;
                    case "Near":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;//sol
                        break;
                }
                e.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            }
        }

        public static void GridCustomDrawFooterCell(DevExpress.XtraGrid.Columns.GridColumn column, string horzalignment, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column == column)
            {
                switch (horzalignment)
                {
                    case "Center":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Center; //merkez
                        break;
                    case "Default":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Default;
                        break;
                    case "Far":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Far; //sag
                        break;
                    case "Near":
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Near;//sol
                        break;
                }
                e.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            }
        }

        public static void ShowGridPreview(GridControl grid, bool showPrintable = true)
        {
            if (!grid.IsPrintingAvailable)
            {
                XtraMessageBox.Show("'DevExpress.XtraPrinting' kitabxanası tapılmadı", "Xəta");
                return;
            }

            var view = (grid.MainView as GridView);
            bool selectedColumn = true;

            if (showPrintable)
            {
                List<ColumnPrintable> lstColumnName = new List<ColumnPrintable>();
                for (int i = 0; i < view.Columns.Count; i++)
                {
                    if (view.Columns[i].Visible == false)
                        continue;
                    lstColumnName.Add(new ColumnPrintable(i, view.Columns[i].Caption));
                }

                FPrintableCheck fp = new FPrintableCheck();
                fp.ListColumnName = lstColumnName;
                fp.RefreshDataGridView += new FPrintableCheck.DoEvent(RefreshPrintable);
                fp.ShowDialog();

                void RefreshPrintable(List<int> lstColumnIndex)
                {
                    if (lstColumnIndex.Count == 0)
                    {
                        selectedColumn = false;
                        return;
                    }

                    for (int i = 0; i < view.Columns.Count; i++)
                    {
                        if (view.Columns[i].Visible == false)
                            continue;

                        if (!lstColumnIndex.Contains(i))
                            view.Columns[i].OptionsColumn.Printable = DefaultBoolean.False;
                    }
                }
            }

            if (!selectedColumn)
                return;

            grid.ShowPrintPreview();

            if (showPrintable)
                for (int i = 0; i < view.Columns.Count; i++)
                {
                    if (view.Columns[i].Visible == false)
                        continue;

                    view.Columns[i].OptionsColumn.Printable = DefaultBoolean.Default;
                }
        }

        public static void ShowTreeListPreview(TreeList treeList)
        {
            // Check whether the Tree List can be previewed. 
            if (!treeList.IsPrintingAvailable)
            {
                MessageBox.Show("The Printing Library is not found", "Error");
                return;
            }

            // Open the Preview window. 
            treeList.ShowRibbonPrintPreview();
        }

        public static void InsertUserConnection()
        {
            ExecuteQuery($@"INSERT INTO CRS_USER.USER_CONNECTIONS(USER_ID,
                                                                    IPADDRESS,
                                                                    MACADDRESS,
                                                                    CONNECT_DATE,
                                                                    COMPUTER_NAME) 
                                VALUES({GlobalVariables.V_UserID},
                                        '{GlobalFunctions.GetIPAddress()}',
                                        '{GlobalFunctions.GetMACAddress()}',
                                        SYSDATE,
                                        '{GlobalFunctions.GetComputerName()}')",
                          "İstifadəçinin sistemə qoşulma vaxtı cədvələ daxil edilmədi.",
                          "InsertUserConnection");
        }

        public static void UpdateUserConnected()
        {
            ExecuteQuery($@"UPDATE CRS_USER.CRS_USERS SET SESSION_ID = SYS_CONTEXT ('userenv', 'sessionid') WHERE ID = {GlobalVariables.V_UserID}",
                             "İstifadəçinin sistemə qoşulması istifadəçilər cədvəlində qeyd olunmadı.",
                             "UpdateUserConnected");
        }

        public static void UpdateUserDisconnected()
        {
            ExecuteQuery($@"UPDATE CRS_USER.CRS_USERS SET SESSION_ID = 0 WHERE ID = {GlobalVariables.V_UserID}",
                            "İstifadəçinin sistemdən çıxması istifadəçilər cədvəlində qeyd olunmadı.",
                            "UpdateUserDisconnected");
        }

        public static void UpdateUserCloseConnection()
        {
            if (GlobalVariables.V_FConnect_BOK_Click)
                ExecuteQuery($@"UPDATE CRS_USER.USER_CONNECTIONS SET DISCONNECT_DATE = SYSDATE WHERE USER_ID = {GlobalVariables.V_UserID} AND DISCONNECT_DATE IS NULL",
                    "İstifadəçinin sistemdən çıxma vaxtı cədvəldə dəyişdirilmədi.",
                    "UpdateUserCloseConnection");
        }

        public static void Lock_or_UnLock_UserID(string tablename, int userID, string where)
        {
            ExecuteQuery($@"UPDATE {tablename} SET USED_USER_ID = {userID} {where}",
                            tablename + " cədvəli bloka düşmədi.",
                            "Lock_or_UnLock_UserID");
        }

        public static void SortTableByName(string tablename)
        {
            ExecuteQuery($@"UPDATE {tablename} T
                                   SET ORDER_ID =
                                          (SELECT ORDERID
                                             FROM (  SELECT ID,                                                                                                                     
                                                            ROW_NUMBER () OVER (ORDER BY NAME) ORDERID
                                                       FROM {tablename}
                                                   ORDER BY NAME)
                                            WHERE ID = T.ID)",
                          tablename + " cədvəli əlifba sırası ilə düzülmədi.",
                          "SortTableByName");
        }

        public static void FillCheckedComboBox(CheckedComboBoxEdit cb, string TableName, string DisplayMember, string Where, string selectedText = null)
        {
            string s = null;
            try
            {
                if (Where == null)
                    s = "SELECT " + DisplayMember + " FROM CRS_USER." + TableName;
                else
                    s = "SELECT " + DisplayMember + " FROM CRS_USER." + TableName + " WHERE " + Where;

                cb.Properties.Items.Clear();
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, "FillCheckedComboBox").Rows)
                {
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            {
                                if (!String.IsNullOrEmpty(dr[0].ToString()))
                                {
                                    cb.Properties.Items.Add(dr[0].ToString(), CheckState.Unchecked, true);
                                    cb.Properties.SeparatorChar = ';';
                                }
                            }
                            break;
                        case "EN":
                            {
                                if (!String.IsNullOrEmpty(dr[1].ToString()))
                                {
                                    cb.Properties.Items.Add(dr[1].ToString(), CheckState.Unchecked, true);
                                    cb.Properties.SeparatorChar = ';';
                                }
                            }
                            break;
                        case "RU":
                            {
                                if (!String.IsNullOrEmpty(dr[2].ToString()))
                                {
                                    cb.Properties.Items.Add(dr[2].ToString(), CheckState.Unchecked, true);
                                    cb.Properties.SeparatorChar = ';';
                                }
                            }
                            break;
                    }

                    if (!String.IsNullOrWhiteSpace(selectedText))
                        cb.SetEditValue(selectedText);
                }
            }
            catch (Exception exx)
            {
                LogWrite(TableName + " " + cb.Name + " siyahısına yüklənə bilmədi.", s, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void FillCheckedComboBoxWithSqlText(CheckedComboBoxEdit cb, string sqltext)
        {
            try
            {
                cb.Properties.Items.Clear();
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sqltext, "FillCheckedComboBoxWithSqlText").Rows)
                {
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            {
                                if (!String.IsNullOrEmpty(dr[0].ToString()))
                                {
                                    cb.Properties.Items.Add(dr[0].ToString(), CheckState.Unchecked, true);
                                    cb.Properties.SeparatorChar = ';';
                                }
                            }
                            break;
                        case "EN":
                            {
                                if (!String.IsNullOrEmpty(dr[1].ToString()))
                                {
                                    cb.Properties.Items.Add(dr[1].ToString(), CheckState.Unchecked, true);
                                    cb.Properties.SeparatorChar = ';';
                                }
                            }
                            break;
                        case "RU":
                            {
                                if (!String.IsNullOrEmpty(dr[2].ToString()))
                                {
                                    cb.Properties.Items.Add(dr[2].ToString(), CheckState.Unchecked, true);
                                    cb.Properties.SeparatorChar = ';';
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception exx)
            {
                LogWrite("SQL " + cb.Name + " siyahısına yüklənə bilmədi.", sqltext, GlobalVariables.V_UserName, "GlobalProcedures", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void FillComboBoxEdit(ComboBoxEdit cb, string TableName, string DisplayMember, string Where, int? SelectedIndex = null, bool AddFirstItemIsNull = false)
        {
            string s = null;
            try
            {
                if (Where == null)
                    s = "SELECT " + DisplayMember + " FROM CRS_USER." + TableName;
                else
                    s = "SELECT " + DisplayMember + " FROM CRS_USER." + TableName + " WHERE " + Where;

                cb.Properties.Items.Clear();

                if (AddFirstItemIsNull)
                    cb.Properties.Items.Add(String.Empty);

                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, "FillComboBoxEdit").Rows)
                {
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            {
                                if (!String.IsNullOrEmpty(dr[0].ToString()))
                                    cb.Properties.Items.Add(dr[0].ToString());

                            }
                            break;
                        case "EN":
                            {
                                if (!String.IsNullOrEmpty(dr[1].ToString()))
                                    cb.Properties.Items.Add(dr[1].ToString());
                            }
                            break;
                        case "RU":
                            {
                                if (!String.IsNullOrEmpty(dr[2].ToString()))
                                    cb.Properties.Items.Add(dr[2].ToString());
                            }
                            break;
                    }
                }

                if (SelectedIndex != null)
                    cb.SelectedIndex = (int)SelectedIndex;
            }
            catch (Exception exx)
            {
                LogWrite(TableName + " " + cb.Name + " siyahısına yüklənə bilmədi.", s, GlobalVariables.V_UserName, "GlobalProcedures", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void FillComboBoxEditWithSqlText(ComboBoxEdit cb, string sqltext)
        {
            try
            {
                cb.Properties.Items.Clear();
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sqltext, "FillComboBoxEditWithSqlText").Rows)
                {
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ":
                            {
                                if (!String.IsNullOrEmpty(dr[0].ToString()))
                                    cb.Properties.Items.Add(dr[0].ToString());

                            }
                            break;
                        case "EN":
                            {
                                if (!String.IsNullOrEmpty(dr[1].ToString()))
                                    cb.Properties.Items.Add(dr[1].ToString());
                            }
                            break;
                        case "RU":
                            {
                                if (!String.IsNullOrEmpty(dr[2].ToString()))
                                    cb.Properties.Items.Add(dr[2].ToString());
                            }
                            break;
                    }
                }
                //cb.SelectedIndex = 0;
            }
            catch (Exception exx)
            {
                LogWrite("SQL " + cb.Name + " siyahısına yüklənə bilmədi.", sqltext, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void FillLookUpEdit(LookUpEdit luk, string TableName, string DisplayID, string DisplayMember, string Where)
        {
            string s = null;
            try
            {
                if (Where == null)
                    s = $@"SELECT {DisplayID},{DisplayMember} FROM CRS_USER.{TableName}";
                else
                    s = $@"SELECT {DisplayID},{DisplayMember} FROM CRS_USER.{TableName} WHERE {Where}";
                luk.Properties.DataSource = null;
                luk.Properties.DataSource = GlobalFunctions.GenerateDataTable(s, "FillLookUpEdit");
                luk.Properties.DisplayMember = DisplayMember;
                luk.Properties.ValueMember = DisplayID;
            }
            catch (Exception exx)
            {
                LogWrite(TableName + " " + luk.Name + " siyahısına yüklənə bilmədi.", s, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void FillLookUpEditWithSqlText(LookUpEdit luk, string sqltext, string DisplayID, string DisplayMember)
        {
            try
            {
                luk.Properties.DataSource = GlobalFunctions.GenerateDataTable(sqltext, "FillLookUpEditWithSqlText");
                luk.Properties.DisplayMember = DisplayMember;
                luk.Properties.ValueMember = DisplayID;
                //luk.Properties.PopulateColumns();
                //luk.Properties.Columns[luk.Properties.ValueMember].Visible = false;
            }
            catch (Exception exx)
            {
                LogWrite("SQL " + luk.Name + " siyahısına yüklənə bilmədi.", sqltext, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void GenerateRateText()
        {
            List<CurrencyRates> lstRate = CurrencyRatesDAL.SelectCurrencyRatesLastDate().ToList<CurrencyRates>();
            if (lstRate.Count == 0)
                return;

            decimal cur_USD = 0, cur_EUR = 0, cur_RUB = 0;
            if (lstRate.Find(r => r.CURRENCY_CODE == "USD") != null)
                cur_USD = lstRate.Find(r => r.CURRENCY_CODE == "USD").AMOUNT;//usd-in mezennesi
            //if (lstRate.Find(r => r.CURRENCY_CODE == "EUR") != null)
            //    cur_EUR = lstRate.Find(r => r.CURRENCY_CODE == "EUR").AMOUNT;//eur-in mezennesi
            //if (lstRate.Find(r => r.CURRENCY_CODE == "RUB") != null)
            //    cur_RUB = lstRate.Find(r => r.CURRENCY_CODE == "RUB").AMOUNT;//eur-in mezennesi

            GlobalVariables.V_LastRate = lstRate.LastOrDefault().RATE_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ) + " tarixinə 1 USD = " + cur_USD + " AZN";
        }

        public static void DeleteAllFilesInDirectory(string directorypath)
        {
            if (String.IsNullOrWhiteSpace(directorypath))
                return;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(directorypath);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(directorypath, file.Name);
                    if (File.Exists(temppath))
                    {
                        if (!GlobalFunctions.IsFileLocked(file))
                            File.Delete(temppath);
                    }
                }
            }
            catch (Exception exx)
            {
                LogWrite(directorypath + " ünvanında olan fayllar silinmədi.", directorypath, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void DeleteFile(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    FileInfo fileInfo = new FileInfo(filepath);
                    if (GlobalFunctions.IsFileLocked(fileInfo))
                        GlobalProcedures.KillWord();

                    File.Delete(filepath);
                }
            }
            catch (Exception exx)
            {
                LogWrite(filepath + " faylı silinmədi.", filepath, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void KillWord()
        {
            foreach (Process p in Process.GetProcessesByName("winword"))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit();
                }
                catch
                {
                    // process was terminating or can't be terminated - deal with it
                }
            }
        }

        public static void ExchangeCalculator(string date)
        {
            Forms.FExchangeCalculator fec = new Forms.FExchangeCalculator();
            fec.RateDate = date;
            fec.ShowDialog();
        }

        public static void CreditCalculator()
        {
            Forms.FCreditCalculator fcc = new Forms.FCreditCalculator();
            fcc.ShowDialog();
        }

        public static void Calculator()
        {
            Process.Start("Calc");
        }

        public static void CalcEditFormat(CalcEdit c_edit)
        {
            c_edit.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            c_edit.Properties.DisplayFormat.FormatString = "### ### ### ### ##0.00";
        }

        public static void DateEditFormat(DateEdit d_edit)
        {
            d_edit.Properties.Mask.EditMask = "dd.MM.yyyy";
            d_edit.Properties.Mask.UseMaskAsDisplayFormat = true;
        }

        public static void ShowUserControl(SplitContainerControl scc, XtraUserControl module)
        {
            if (scc.Panel2.Controls.Count > 0)
            {
                scc.Panel2.Controls.RemoveAt(0);
            }
            module.Dock = DockStyle.Fill;
            module.Bounds = scc.Panel2.DisplayRectangle;

            scc.Panel2.Controls.Add(module);
        }

        public static void LoadCurrencyRateFromCBAR(string date)
        {
            int i = 0;
            string filePath = @"http://cbar.az/currencies/" + date + ".xml", s = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(filePath);
                try
                {
                    s = "SELECT ID,CODE FROM CRS_USER.CURRENCY ORDER BY ORDER_ID";

                    foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, "LoadCurrencyRateFromCBAR").Rows)
                    {
                        i++;
                        XmlNodeList xnList = xml.SelectNodes($@"/ValCurs[@*]/ValType/Valute[@Code='{dr[1]}']");
                        foreach (XmlNode xn in xnList)
                        {
                            ExecuteTwoQuery($@"DELETE FROM CRS_USER.CURRENCY_RATES WHERE CURRENCY_ID = {dr[0]} AND RATE_DATE = TO_DATE('{date}','DD/MM/YYYY')",
                                            $@"INSERT INTO CRS_USER.CURRENCY_RATES(ID,CURRENCY_ID,RATE_DATE,AMOUNT) VALUES(CURRENCY_RATE_SEQUENCE.NEXTVAL,{dr[0]},TO_DATE('{date}','DD/MM/YYYY'),{xn["Value"].InnerText.Trim()})",
                                                                "Məzənnə internetdən bazaya daxil olmadı.",
                                            "LoadCurrencyRateFromCBAR");
                        }
                    }
                }
                catch (Exception exx)
                {
                    LogWrite("Valyutalar tapılmadı.", s, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                }
            }
            catch (Exception exx)
            {
                LogWrite(filePath + " faylı Mərkəzi Bankın bazasında tapılmadı.", null, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void InsertCashOperation(int destination_id, int operation_owner_id, string operation_date, string contract_code, double income, double expenses, int commit)
        {
            double debt = 0;
            ExecuteTwoQuery($@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE DESTINATION_ID = {destination_id} AND OPERATION_OWNER_ID = {operation_owner_id}",
                            "INSERT INTO CRS_USER.CASH_OPERATIONS(ID,DESTINATION_ID,OPERATION_OWNER_ID,OPERATION_DATE,CONTRACT_CODE,INCOME,EXPENSES,DEBT,IS_COMMIT)VALUES(CASH_OPERATION_SEQUENCE.NEXTVAL," + destination_id + "," + operation_owner_id + ",TO_DATE('" + operation_date + "','DD/MM/YYYY'),'" + contract_code + "'," + income.ToString(GlobalVariables.V_CultureInfoEN) + "," + expenses.ToString(GlobalVariables.V_CultureInfoEN) + "," + debt.ToString(GlobalVariables.V_CultureInfoEN) + "," + commit + ")",
                                "Əməliyyat kassaya daxil olunmadı.",
                                "InsertCashOperation");
        }

        public static void UpdateCashOperation(int id, int destination_id, int operation_owner_id, string operation_date, double income, double expenses)
        {
            ExecuteQuery("UPDATE CRS_USER.CASH_OPERATIONS SET INCOME = " + income.ToString(GlobalVariables.V_CultureInfoEN) + ",EXPENSES = " + expenses.ToString(GlobalVariables.V_CultureInfoEN) + ",OPERATION_DATE = TO_DATE('" + operation_date + "','DD/MM/YYYY') WHERE ID = " + id + " AND DESTINATION_ID = " + destination_id + " AND OPERATION_OWNER_ID = " + operation_owner_id,
                         "Əməliyyat kassada dəyişdirilmədi.",
                         "UpdateCashOperation");
        }

        public static void InsertCashAdvancePayment(int contractid, int customerid, string contractstartdate, string paymentdate, string appointment, double amount, string note, string contractcode)
        {
            int AdvanceID = GlobalFunctions.GetOracleSequenceValue("CASH_ADVANCE_SEQUENCE");
            ExecuteThreeQuery($@"DELETE FROM CRS_USER.CASH_OPERATIONS WHERE DESTINATION_ID = 2 AND OPERATION_OWNER_ID IN (SELECT ID FROM CRS_USER.CASH_ADVANCE_PAYMENTS WHERE CONTRACT_ID = {contractid})",
                               $@"DELETE FROM CRS_USER.CASH_ADVANCE_PAYMENTS WHERE CONTRACT_ID = {contractid}",
                               $@"INSERT INTO CRS_USER.CASH_ADVANCE_PAYMENTS(ID,CONTRACT_ID,CUSTOMER_ID,CONTRACT_START_DATE,PAYMENT_DATE,APPOINTMENT,AMOUNT,NOTE)VALUES(" + AdvanceID + "," + contractid + "," + customerid + ",TO_DATE('" + contractstartdate + "','DD/MM/YYYY'),TO_DATE('" + paymentdate + "','DD/MM/YYYY'),'" + appointment + "'," + amount.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + note + "')",
                                                "Avans ödənişi daxil olunmadı.",
                                                "InsertCashAdvancePayment");

            InsertCashOperation(2, AdvanceID, paymentdate, contractcode, amount, 0, 1);
            UpdateCashDebt(paymentdate);
        }

        public static void UpdateCashAdvancePayment(int contractid, int customerid, string contractstartdate, string paymentdate, double amount)
        {
            ExecuteTwoQuery($@"UPDATE CRS_USER.CASH_ADVANCE_PAYMENTS SET AMOUNT = " + amount.ToString(GlobalVariables.V_CultureInfoEN) + ",CONTRACT_START_DATE = TO_DATE('" + contractstartdate + "','DD/MM/YYYY'),PAYMENT_DATE = TO_DATE('" + paymentdate + "','DD/MM/YYYY') WHERE CONTRACT_ID = " + contractid + " AND CUSTOMER_ID = " + customerid,
                                "UPDATE CRS_USER.CASH_OPERATIONS SET INCOME = " + amount.ToString(GlobalVariables.V_CultureInfoEN) + ",OPERATION_DATE = TO_DATE('" + paymentdate + "','DD/MM/YYYY') WHERE DESTINATION_ID = 2 AND OPERATION_OWNER_ID IN (SELECT ID FROM CRS_USER.CASH_ADVANCE_PAYMENTS WHERE CONTRACT_ID = " + contractid + " AND CUSTOMER_ID = " + customerid + ")",
                                    "Avans ödənişi dəyişdirilmədi",
                                    "UpdateCashAdvancePayment");
            UpdateCashDebt(paymentdate);
        }

        public static void UpdateCashDebt(string operationdate)
        {
            decimal prev_debt = 0;

            prev_debt = CashOperationsDAL.CashLastDebt(operationdate);

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
                    command.CommandText = "CRS_USER.PROC_UPDATE_CO_DEBT";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_PREV_DEBT", OracleDbType.Decimal).Value = prev_debt.ToString(GlobalVariables.V_CultureInfoEN);
                    command.Parameters.Add("P_OPERATION_DATE", OracleDbType.Varchar2).Value = operationdate;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite("Kassanın qalığı dəyişdirilmədi.", "CRS_USER.PROC_UPDATE_CO_DEBT", GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void UpdateCustomerPaymentChange(int istemp, int contractid, int paymentid)
        {
            string s2 = null,
                s3 = null,
                first_date = null,
                s_date = null,
                contract_start_date = null;

            double first_debt = 0,
                first_payment_interest_debt = 0,
                currency_rate = 1,
                contract_amount = 0,
                one_day_interest = 0,
                interest_amount = 0,
                totaldebtamount = 0,
                debt = 0,
                payment_interest_amount = 0,
                current_payment_interest_debt = 0,
                basic_amount = 0,
                current_debt = 0,
                total = 0,
                //payment_value_AZN = 0,
                paymentamount = 0,
                paymentamountazn = 0,
                payment_interest_debt = 0,
                normal_debt = 0,
                requiredamount = 0,
                penalty_amount = 0,
                penalty = 0,
                debtpenalty = 0,
                currentdebtpenalty = 0,
                payedAmount = 0,
                payedAmountAZN = 0;

            int interest = 0,
                currencyid = 1,
                diff_day,
                i = 0,
                order_id = 1,
                orderID = 1,
                is_penalty = 0,
                insuranceCheck = 0;

            decimal insuranceAmount = 0;

            try
            {
                s2 = $@"SELECT TO_CHAR (START_DATE, 'DD/MM/YYYY') START_DATE,
                               AMOUNT,
                               CURRENCY_ID,
                               INTEREST
                          FROM CRS_USER.V_CONTRACTS
                         WHERE CONTRACT_ID = {contractid}";
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s2, "UpdateCustomerPaymentChange").Rows)
                {
                    contract_start_date = dr["START_DATE"].ToString();
                    contract_amount = Convert.ToDouble(dr["AMOUNT"].ToString());
                    currencyid = Convert.ToInt32(dr["CURRENCY_ID"].ToString());
                    interest = Convert.ToInt32(dr["INTEREST"].ToString());
                }

                List<Payments> lstPayments = PaymentsDAL.SelectAllPayments(istemp, contractid).ToList<Payments>();
                if (lstPayments.Count > 0)
                {
                    orderID = lstPayments.Find(p => p.ID == paymentid).ORDER_ID;

                    var payments = lstPayments.Where(p => p.ORDER_ID < orderID).ToList<Payments>();
                    if (payments.Count > 0)
                    {
                        int last_payment_id = payments.LastOrDefault().ID;//  Max(p => p.ID);
                        first_date = lstPayments.Find(p => p.ID == last_payment_id).PAYMENT_DATE.ToString("d", GlobalVariables.V_CultureInfoAZ);
                        first_debt = Math.Round((double)lstPayments.Find(p => p.ID == last_payment_id).DEBT, 2);
                        first_payment_interest_debt = Math.Round((double)lstPayments.Find(p => p.ID == last_payment_id).PAYMENT_INTEREST_DEBT, 2);
                    }
                    else
                    {
                        first_date = contract_start_date;
                        first_debt = contract_amount;
                        first_payment_interest_debt = 0;
                    }
                }
                else
                {
                    first_date = contract_start_date;
                    first_debt = contract_amount;
                    first_payment_interest_debt = 0;
                }

                if (istemp == 0)
                    s3 = $@"SELECT *
                                FROM (SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD/MM/YYYY') PAYMENT_DATE,
                                             CP.ID,
                                             CP.CUSTOMER_ID,
                                             CP.CONTRACT_ID,
                                             CP.PAYMENT_AMOUNT,
                                             CP.PAYMENT_AMOUNT_AZN,
                                             CP.PAYMENT_INTEREST_DEBT,
                                             CP.IS_PENALTY,
                                             CP.PENALTY_AMOUNT,
                                             CP.PENALTY_DEBT,
                                             CP.BANK_CASH,
                                             CP.CURRENCY_RATE,
                                             ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                             INSURANCE_CHECK,
                                             INSURANCE_AMOUNT,
                                             CP.PAYED_AMOUNT,
                                             CP.PAYED_AMOUNT_AZN
                                        FROM CRS_USER.CUSTOMER_PAYMENTS CP
                                       WHERE CONTRACT_ID = {contractid})
                               WHERE ORDER_ID >= {orderID}
                            ORDER BY ORDER_ID";
                else
                    s3 = $@"SELECT *
                                FROM (SELECT TO_CHAR (CP.PAYMENT_DATE, 'DD/MM/YYYY') PAYMENT_DATE,
                                             CP.ID,
                                             CP.CUSTOMER_ID,
                                             CP.CONTRACT_ID,
                                             CP.PAYMENT_AMOUNT,
                                             CP.PAYMENT_AMOUNT_AZN,
                                             CP.PAYMENT_INTEREST_DEBT,
                                             CP.IS_PENALTY,
                                             CP.PENALTY_AMOUNT,
                                             CP.PENALTY_DEBT,
                                             CP.BANK_CASH,
                                             CP.CURRENCY_RATE,
                                             ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                             INSURANCE_CHECK,
                                             INSURANCE_AMOUNT,
                                             CP.PAYED_AMOUNT,
                                             CP.PAYED_AMOUNT_AZN
                                        FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP CP
                                       WHERE     CONTRACT_ID = {contractid}
                                             AND CP.IS_CHANGE IN (0, 1)
                                             AND CP.USED_USER_ID = {GlobalVariables.V_UserID})
                               WHERE ORDER_ID >= {orderID}
                            ORDER BY ORDER_ID";

                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s3, "UpdateCustomerPaymentChange").Rows)
                {
                    debtpenalty = Convert.ToDouble(dr["PENALTY_DEBT"].ToString());
                    if (i == 0)
                    {
                        debt = first_debt;
                        payment_interest_debt = first_payment_interest_debt;
                        s_date = first_date;
                    }
                    else
                        debt = current_debt;

                    insuranceCheck = Convert.ToInt16(dr["INSURANCE_CHECK"]);
                    insuranceAmount = Convert.ToDecimal(dr["INSURANCE_AMOUNT"]);
                    currency_rate = Convert.ToDouble(dr["CURRENCY_RATE"]);
                    one_day_interest = Math.Round(((debt * interest) / 100) / 360, 2);
                    diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(s_date, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(dr["PAYMENT_DATE"].ToString(), "ddmmyyyy"));
                    interest_amount = diff_day * one_day_interest;

                    paymentamount = Convert.ToDouble(dr["PAYED_AMOUNT"].ToString());
                    paymentamountazn = Convert.ToDouble(dr["PAYED_AMOUNT_AZN"].ToString());
                    penalty_amount = Convert.ToDouble(dr["PENALTY_AMOUNT"].ToString());
                    is_penalty = Convert.ToInt32(dr["IS_PENALTY"].ToString());

                    payedAmountAZN = paymentamountazn - (insuranceCheck == 1 ? (double)insuranceAmount : 0);
                    payedAmount = Math.Round(currency_rate > 0 ? payedAmountAZN / currency_rate : 0, 2);

                    if (payedAmount > 0) // eger odenis sifirdan boyuk olarsa
                    {
                        if ((interest_amount + payment_interest_debt) > payedAmount) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                            payment_interest_amount = payedAmount;
                        else
                            payment_interest_amount = interest_amount + payment_interest_debt; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
                    }

                    if (is_penalty == 1)
                    {
                        if (payedAmount < penalty_amount)
                        {
                            basic_amount = 0;
                            penalty_amount = payedAmount;
                            payment_interest_amount = 0;
                        }
                        else
                        {
                            penalty = payedAmount - penalty_amount;
                            if (penalty < payment_interest_amount)
                            {
                                payment_interest_amount = penalty;
                                basic_amount = 0;
                            }
                            else
                                basic_amount = Math.Round(penalty - payment_interest_amount, 2);
                            //penalty_amount = penalty_amount;
                        }
                    }
                    else
                    {
                        basic_amount = Math.Round(payedAmount - payment_interest_amount, 2);
                        penalty_amount = 0;
                    }

                    current_payment_interest_debt = payment_interest_debt + interest_amount - payment_interest_amount;
                    //basic_amount = paymentamount - payment_interest_amount;
                    current_debt = debt - basic_amount;
                    total = current_debt + current_payment_interest_debt;
                    totaldebtamount = Math.Round((debt + current_payment_interest_debt + interest_amount), 2);
                    //if (currencyid == 1)
                    //    payment_value_AZN = paymentamount;
                    //else
                    //    payment_value_AZN = paymentamountazn;

                    List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(contractid).ToList<PaymentSchedules>();
                    var schedules = lstSchedules.Where(s => s.MONTH_DATE <= GlobalFunctions.ChangeStringToDate(dr["PAYMENT_DATE"].ToString(), "ddmmyyyy")).ToList<PaymentSchedules>();
                    if (schedules.Count == 0)
                        order_id = 0;
                    else
                        order_id = schedules.Max(s => s.ORDER_ID);

                    if (order_id == 0)
                        normal_debt = current_debt;
                    else
                        normal_debt = Math.Round(lstSchedules.Find(s => s.ORDER_ID == order_id).DEBT, 2); //qrafik uzre odenis
                    if ((current_debt - normal_debt) > 0)
                        requiredamount = (current_debt - normal_debt) + payment_interest_debt;
                    else
                        requiredamount = payment_interest_debt;

                    i++;
                    s_date = dr["PAYMENT_DATE"].ToString();

                    if (istemp == 0)
                    {
                        ExecuteQuery($@"UPDATE CRS_USER.CUSTOMER_PAYMENTS SET BASIC_AMOUNT = {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            DEBT = {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            DAY_COUNT = {diff_day},
                                                                            ONE_DAY_INTEREST_AMOUNT = {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            INTEREST_AMOUNT = {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            PAYMENT_INTEREST_AMOUNT = {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            PAYMENT_INTEREST_DEBT = {Math.Round(current_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            TOTAL = {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            REQUIRED_CLOSE_AMOUNT = {Math.Round(totaldebtamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            REQUIRED_AMOUNT = {Math.Round(requiredamount, 2).ToString(GlobalVariables.V_CultureInfoEN)} 
                                                                            WHERE CONTRACT_ID = {contractid} AND ID = {dr["ID"]}",
                                                        "Lizinq ödənişi dəyişdirilmədi",
                                                        "UpdateCustomerPaymentChange");

                        if (penalty_amount > 0)
                        {
                            currentdebtpenalty = debtpenalty - penalty_amount;
                            ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_BALANCE_PENALTIES SET 
                                                                DEBT_PENALTY = {currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                PAYMENT_PENALTY = {penalty_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                IS_COMMIT = 1 
                                                                WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr["ID"]}",
                                           "Cərimə cədvəldə dəyişdirilmədi.",
                                           "UpdateCustomerPaymentChange");
                        }
                        else
                            ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_BALANCE_PENALTIES SET 
                                                    DEBT_PENALTY = {debtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                                                    PAYMENT_PENALTY = 0,
                                                    IS_COMMIT = 1 
                                                    WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr["ID"]}",
                                        "Cərimə cədvəldə dəyişdirilmədi.",
                                        "UpdateCustomerPaymentChange");
                    }
                    else
                    {
                        ExecuteQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP SET IS_CHANGE = 1, 
                                                                            BASIC_AMOUNT = {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            DEBT = {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            DAY_COUNT = {diff_day},
                                                                            ONE_DAY_INTEREST_AMOUNT = {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            INTEREST_AMOUNT = {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            PAYMENT_INTEREST_AMOUNT = {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            PAYMENT_INTEREST_DEBT = {Math.Round(current_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            TOTAL = {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            REQUIRED_CLOSE_AMOUNT = {Math.Round(totaldebtamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                            REQUIRED_AMOUNT = {Math.Round(requiredamount, 2).ToString(GlobalVariables.V_CultureInfoEN)} 
                                                                            WHERE CONTRACT_ID = {contractid} AND ID = {dr["ID"]} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                        "Lizinq ödənişi dəyişdirilmədi",
                                                        "UpdateCustomerPaymentChange");

                        if (penalty_amount > 0)
                        {
                            currentdebtpenalty = debtpenalty - penalty_amount;
                            ExecuteQuery($@"UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET DEBT_PENALTY = {currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                PAYMENT_PENALTY = {penalty_amount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                IS_CHANGE = 1 
                                                                WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr["ID"]}",
                                                        "Cərimə temp cədvəldə dəyişdirilmədi.",
                                                        "UpdateCustomerPaymentChange");
                        }
                        else
                            ExecuteQuery($@"UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET DEBT_PENALTY = {debtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                                                               PAYMENT_PENALTY = 0,
                                                               IS_CHANGE = 2 
                                                               WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr["ID"]}",
                                                        "Cərimə temp cədvəldə dəyişdirilmədi.",
                                                        "UpdateCustomerPaymentChange");
                    }

                    payment_interest_debt = current_payment_interest_debt;
                }
            }
            catch (Exception exx)
            {
                LogWrite("Lizinq ödənişinin parametrləri tapılmadı.", s3, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void UpdateCustomerPayment(int istemp, string operationdate, string clearingdate, int contractid, double paymentamount, double changedpayinterestamount, double paymentamountazn, double currencyrate, string paymentname, int diff_day, double one_day_interest, double interest_amount, double totaldebtamount, double requiredamount, int paymentid, double payedpenalty, int ispenalty, double penalty_debt, double penalty_amount, string operationaccount, string paymentNote = null, int insuranceCheck = 0, decimal insuranceAmount = 0, int is_changed_interest = 0, int is_penalty_debt = 0)
        {
            string s2 = null,
                s3 = null,
                s_date = null,
                o_date = null,
                p_date = null,
                contract_start_date = null,
                note = null;

            double first_debt = 0,
                first_payment_interest_debt = 0,
                contract_amount = 0,
                debt = 0,
                payment_interest_amount = 0,
                currenct_payment_interest_debt = 0,
                basic_amount = 0,
                current_debt = 0,
                total = 0,
                payment_interest_debt = 0,
                normal_debt = 0,
                currency_rate = 1,
                payedAmount = 0,
                payedAmountAZN = 0,
                totalpayedinterest = 0,
                payed_penalty = 0,
                penalty,
                penaltyamount = 0,
                debtpenalty = 0;

            int interest = 0,
                currencyid = 1,
                i = 0,
                order_id = 1,
                is_penalty = 0,
                orderID = 1,
                clearing_calculated = 1,
                is_clearing = 0,
                ispenaltydebt = 0;

            try
            {
                s2 = $@"SELECT TO_CHAR (START_DATE, 'DD/MM/YYYY') START_DATE,
                               AMOUNT,
                               CURRENCY_ID,
                               INTEREST
                          FROM CRS_USER.V_CONTRACTS
                         WHERE CONTRACT_ID = {contractid}";
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s2, "UpdateCustomerPayment (s2)").Rows)
                {
                    contract_start_date = dr["START_DATE"].ToString();
                    contract_amount = Convert.ToDouble(dr["AMOUNT"].ToString());
                    currencyid = Convert.ToInt32(dr["CURRENCY_ID"].ToString());
                }

                List<Payments> lstPayments = PaymentsDAL.SelectPayments(istemp, contractid).ToList<Payments>();
                if (lstPayments.Count > 0)
                {
                    orderID = lstPayments.Find(p => p.ID == paymentid).ORDER_ID;

                    var payments = lstPayments.Where(p => p.ORDER_ID < orderID).ToList<Payments>();

                    if (payments.Count > 0)
                    {
                        int last_payment_id = payments.LastOrDefault().ID;
                        first_debt = Math.Round((double)lstPayments.Find(p => p.ID == last_payment_id).DEBT, 2);
                        first_payment_interest_debt = Math.Round((double)lstPayments.Find(p => p.ID == last_payment_id).PAYMENT_INTEREST_DEBT, 2);
                    }
                    else
                    {
                        first_debt = contract_amount;
                        first_payment_interest_debt = 0;
                    }
                }
                else
                {
                    first_debt = contract_amount;
                    first_payment_interest_debt = 0;
                }

                if (istemp == 0)
                    s3 = $@"SELECT *
                                FROM (SELECT TO_CHAR (PAYMENT_DATE, 'DD/MM/YYYY') PAYMENT_DATE,
                                             ID,
                                             CUSTOMER_ID,
                                             CONTRACT_ID,
                                             PAYED_AMOUNT,
                                             PAYED_AMOUNT_AZN,
                                             PAYMENT_INTEREST_DEBT,
                                             IS_PENALTY,
                                             PAYED_PENALTY,
                                             PENALTY_DEBT,
                                             PENALTY_AMOUNT,
                                             CURRENCY_RATE,
                                             BANK_CASH,
                                             CHANGED_PAY_INTEREST_AMOUNT,
                                             IS_CHANGED_INTEREST,
                                             ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                             NOTE,
                                             INSURANCE_CHECK,
                                             INSURANCE_AMOUNT,
                                             CONTRACT_PERCENT,
                                             TO_CHAR (CLEARING_DATE, 'DD/MM/YYYY') OPERATION_DATE,
                                             IS_PENALTY_DEBT
                                        FROM CRS_USER.CUSTOMER_PAYMENTS
                                       WHERE CONTRACT_ID = {contractid})
                               WHERE ORDER_ID >= {orderID}
                            ORDER BY ORDER_ID";
                else
                    s3 = $@"SELECT *
                                    FROM (SELECT TO_CHAR (PAYMENT_DATE, 'DD/MM/YYYY') PAYMENT_DATE,
                                                 ID,
                                                 CUSTOMER_ID,
                                                 CONTRACT_ID,
                                                 PAYED_AMOUNT,
                                                 PAYED_AMOUNT_AZN,
                                                 PAYMENT_INTEREST_DEBT,
                                                 IS_PENALTY,
                                                 PAYED_PENALTY,
                                                 PENALTY_DEBT,  
                                                 PENALTY_AMOUNT,
                                                 CURRENCY_RATE,
                                                 BANK_CASH,
                                                 CHANGED_PAY_INTEREST_AMOUNT,
                                                 IS_CHANGED_INTEREST,
                                                 ROW_NUMBER () OVER (ORDER BY PAYMENT_DATE, ID) ORDER_ID,
                                                 NOTE,
                                                 INSURANCE_CHECK,
                                                 INSURANCE_AMOUNT,
                                                 CONTRACT_PERCENT,
                                                 TO_CHAR (CLEARING_DATE, 'DD/MM/YYYY') OPERATION_DATE,
                                                 IS_PENALTY_DEBT
                                            FROM CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP
                                           WHERE     CONTRACT_ID = {contractid}
                                                 AND USED_USER_ID = {GlobalVariables.V_UserID}
                                                 AND IS_CHANGE IN (0, 1))
                                   WHERE ORDER_ID >= {orderID}
                                ORDER BY ORDER_ID";

                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s3, "UpdateCustomerPayment (s3)").Rows)
                {
                    interest = Convert.ToInt32(dr["CONTRACT_PERCENT"].ToString());

                    if (i == 0)
                    {
                        debt = first_debt;
                        payment_interest_debt = first_payment_interest_debt;
                        s_date = p_date = operationdate;
                        o_date = clearingdate;
                        payed_penalty = payedpenalty;
                        is_penalty = ispenalty;
                        penaltyamount = penalty_amount;
                        debtpenalty = penalty_debt;
                        currency_rate = currencyrate;
                        note = paymentNote;
                        ispenaltydebt = is_penalty_debt;
                    }
                    else
                    {
                        o_date = dr["OPERATION_DATE"].ToString();
                        insuranceCheck = Convert.ToInt16(dr["INSURANCE_CHECK"]);
                        insuranceAmount = Convert.ToDecimal(dr["INSURANCE_AMOUNT"]);
                        debt = current_debt;
                        paymentamount = Convert.ToDouble(dr["PAYED_AMOUNT"]);
                        is_changed_interest = Convert.ToInt32(dr["IS_CHANGED_INTEREST"]);
                        changedpayinterestamount = Convert.ToDouble(dr["CHANGED_PAY_INTEREST_AMOUNT"]);
                        paymentamountazn = Convert.ToDouble(dr["PAYED_AMOUNT_AZN"]);
                        payed_penalty = Convert.ToDouble(dr["PAYED_PENALTY"]);
                        penaltyamount = Convert.ToDouble(dr["PENALTY_AMOUNT"]);
                        debtpenalty = Convert.ToDouble(dr["PENALTY_DEBT"]);
                        is_penalty = Convert.ToInt32(dr["IS_PENALTY"]);
                        ispenaltydebt = Convert.ToInt32(dr["IS_PENALTY_DEBT"]);
                        one_day_interest = Math.Round(((debt * interest) / 100) / 360, 2);
                        diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(s_date, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(dr["OPERATION_DATE"].ToString(), "ddmmyyyy"));
                        interest_amount = diff_day * one_day_interest;
                        s_date = o_date;
                        p_date = dr["PAYMENT_DATE"].ToString();
                        currency_rate = Convert.ToDouble(dr["CURRENCY_RATE"]);
                        note = dr["NOTE"].ToString();
                    }
                    clearing_calculated = GlobalFunctions.ChangeStringToDate(o_date, "ddmmyyyy") > DateTime.Today ? 0 : 1;
                    is_clearing = GlobalFunctions.ChangeStringToDate(o_date, "ddmmyyyy") > GlobalFunctions.ChangeStringToDate(p_date, "ddmmyyyy") ? 1 : 0;
                    payedAmountAZN = paymentamountazn - (insuranceCheck == 1 ? (double)insuranceAmount : 0);
                    payedAmount = Math.Round(currency_rate > 0 ? payedAmountAZN / currency_rate : 0, 2);

                    if (payedAmount > 0) // eger odenis sifirdan boyuk olarsa
                    {
                        totalpayedinterest = is_changed_interest == 0 ? interest_amount + payment_interest_debt : changedpayinterestamount;

                        if (totalpayedinterest > paymentamount) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                            payment_interest_amount = paymentamount;
                        else
                            payment_interest_amount = totalpayedinterest; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
                    }
                    else
                        payment_interest_amount = 0;

                    if (payedAmount < payed_penalty)
                    {
                        basic_amount = 0;
                        payed_penalty = payedAmount;
                        payment_interest_amount = 0;
                    }
                    else
                    {
                        penalty = payedAmount - payed_penalty;
                        if (penalty < payment_interest_amount)
                        {
                            payment_interest_amount = penalty;
                            basic_amount = 0;
                        }
                        else
                            basic_amount = Math.Round(penalty - payment_interest_amount, 2);
                    }


                    currenct_payment_interest_debt = payment_interest_debt + interest_amount - payment_interest_amount;
                    current_debt = debt - basic_amount;
                    total = current_debt + currenct_payment_interest_debt;
                    //totaldebtamount = Math.Round((debt + currenct_payment_interest_debt + interest_amount + payed_penalty + debtpenalty), 2);
                    totaldebtamount = Math.Round((debt + interest_amount + payed_penalty + debtpenalty), 2);

                    //teleb olunan mebleg
                    List<PaymentSchedules> lstSchedules = PaymentSchedulesDAL.SelectPaymentSchedules(contractid).ToList<PaymentSchedules>();

                    var schedules = lstSchedules.Where(s => s.MONTH_DATE <= GlobalFunctions.ChangeStringToDate(s_date, "ddmmyyyy")).ToList<PaymentSchedules>();
                    if (schedules.Count == 0)
                        order_id = 0;
                    else
                        order_id = schedules.Max(s => s.ORDER_ID);

                    if (order_id == 0)
                        normal_debt = current_debt;
                    else
                        normal_debt = Math.Round(lstSchedules.Find(s => s.ORDER_ID == order_id).DEBT, 2); //qrafik uzre odenis
                    if (current_debt - normal_debt > 0)
                        requiredamount = (current_debt - normal_debt) + currenct_payment_interest_debt;
                    else
                        requiredamount = currenct_payment_interest_debt;


                    if (istemp == 0)
                    {
                        ExecuteQuery($@"UPDATE CRS_USER.CUSTOMER_PAYMENTS SET 
                                                PAYED_AMOUNT = {Math.Round(paymentamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYED_AMOUNT_AZN = {Math.Round(paymentamountazn, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYMENT_AMOUNT = {payedAmount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYMENT_AMOUNT_AZN = {payedAmountAZN.ToString(GlobalVariables.V_CultureInfoEN)},
                                                BASIC_AMOUNT = {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                DEBT = {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                DAY_COUNT = {diff_day},
                                                ONE_DAY_INTEREST_AMOUNT = {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                INTEREST_AMOUNT = {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYMENT_INTEREST_AMOUNT = {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYMENT_INTEREST_DEBT = {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                TOTAL = {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                REQUIRED_CLOSE_AMOUNT = {Math.Round(totaldebtamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                REQUIRED_AMOUNT = {Math.Round(requiredamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYMENT_NAME = '{paymentname}',
                                                CURRENCY_RATE = {Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYMENT_DATE = TO_DATE('{p_date}','DD/MM/YYYY'),
                                                CLEARING_DATE = TO_DATE('{o_date}','DD/MM/YYYY'),
                                                CLEARING_CALCULATED = {clearing_calculated},
                                                IS_CLEARING = {is_clearing},
                                                IS_PENALTY = {is_penalty},
                                                IS_PENALTY_DEBT = {ispenaltydebt},
                                                CHANGE_DATE = SYSDATE,
                                                IS_CHANGED_INTEREST = {is_changed_interest},
                                                CHANGED_PAY_INTEREST_AMOUNT = {changedpayinterestamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                PAYED_PENALTY = {Math.Round(payed_penalty, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PENALTY_AMOUNT = {Math.Round(penaltyamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                PENALTY_DEBT = {Math.Round(debtpenalty, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                NOTE = '{note}',
                                                INSURANCE_CHECK = {insuranceCheck},
                                                INSURANCE_AMOUNT = {insuranceAmount.ToString(GlobalVariables.V_CultureInfoEN)}
                                                WHERE CONTRACT_ID = {contractid} AND ID = {dr[1]}",
                                     "Lizinq ödənişi dəyişdirilmədi",
                                     "UpdateCustomerPayment");

                        //if (payed_penalty > 0)
                        //{
                        //    currentdebtpenalty = debtpenalty - payed_penalty;
                        //    ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_BALANCE_PENALTIES SET 
                        //                                        DEBT_PENALTY = {currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                        //                                        PAYMENT_PENALTY = {payed_penalty.ToString(GlobalVariables.V_CultureInfoEN)},
                        //                                        IS_COMMIT = 1 
                        //                                        WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr[1]}",
                        //                   "Cərimə cədvəldə dəyişdirilmədi.",
                        //                   "UpdateCustomerPayment");
                        //}
                        //else
                        //    ExecuteQuery($@"UPDATE CRS_USER.CONTRACT_BALANCE_PENALTIES SET 
                        //                            DEBT_PENALTY = {debtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                        //                            PAYMENT_PENALTY = 0,
                        //                            IS_COMMIT = 1 
                        //                            WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr[1]}",
                        //                "Cərimə cədvəldə dəyişdirilmədi.",
                        //                "UpdateCustomerPayment");
                    }
                    else
                    {
                        ExecuteQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_PAYMENTS_TEMP SET IS_CHANGE = 1, 
                                                                                        PAYED_AMOUNT = {Math.Round(paymentamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYED_AMOUNT_AZN = {Math.Round(paymentamountazn, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYMENT_AMOUNT = {payedAmount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYMENT_AMOUNT_AZN = {payedAmountAZN.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        BASIC_AMOUNT = {Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        DEBT = {Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        DAY_COUNT = {diff_day},
                                                                                        ONE_DAY_INTEREST_AMOUNT = {Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        INTEREST_AMOUNT = {Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYMENT_INTEREST_AMOUNT = {Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYMENT_INTEREST_DEBT = {Math.Round(currenct_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        TOTAL = {Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        REQUIRED_CLOSE_AMOUNT = {Math.Round(totaldebtamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        REQUIRED_AMOUNT = {Math.Round(requiredamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYMENT_NAME = '{paymentname}',
                                                                                        CURRENCY_RATE = {Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYMENT_DATE = TO_DATE('{p_date}','DD/MM/YYYY'),
                                                                                        CLEARING_DATE = TO_DATE('{o_date}','DD/MM/YYYY'),
                                                                                        CLEARING_CALCULATED = {clearing_calculated},
                                                                                        IS_CLEARING = {is_clearing},
                                                                                        IS_PENALTY = {is_penalty},
                                                                                        IS_PENALTY_DEBT = {ispenaltydebt},
                                                                                        IS_CHANGED_INTEREST = {is_changed_interest},
                                                                                        CHANGED_PAY_INTEREST_AMOUNT = {changedpayinterestamount.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PAYED_PENALTY = {Math.Round(payed_penalty, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PENALTY_AMOUNT = {Math.Round(penaltyamount, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        PENALTY_DEBT = {Math.Round(debtpenalty, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                        NOTE = '{note}',
                                                                                        INSURANCE_CHECK = {insuranceCheck},
                                                                                        INSURANCE_AMOUNT = {insuranceAmount.ToString(GlobalVariables.V_CultureInfoEN)}
                                                                                        WHERE CONTRACT_ID = {contractid} AND ID = {dr[1]}",
                                                    "Lizinq ödənişi dəyişdirilmədi",
                                                    "UpdateCustomerPayment");

                        //if (payed_penalty > 0)
                        //{
                        //    currentdebtpenalty = debtpenalty - payed_penalty;
                        //    ExecuteQuery($@"UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET DEBT_PENALTY = {currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                        //                                        PAYMENT_PENALTY = {payed_penalty.ToString(GlobalVariables.V_CultureInfoEN)},
                        //                                        IS_CHANGE = 1 
                        //                                        WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr[1]}",
                        //                                "Cərimə temp cədvəldə dəyişdirilmədi.",
                        //                                "UpdateCustomerPayment");
                        //}
                        //else
                        //    ExecuteQuery($@"UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET DEBT_PENALTY = {debtpenalty.ToString(GlobalVariables.V_CultureInfoEN)},
                        //                                       PAYMENT_PENALTY = 0,
                        //                                       IS_CHANGE = 2 
                        //                                       WHERE CONTRACT_ID = {contractid} AND CUSTOMER_PAYMENT_ID = {dr[1]}",
                        //                                "Cərimə temp cədvəldə dəyişdirilmədi.",
                        //                                "UpdateCustomerPayment");
                    }

                    if (insuranceCheck == 1)
                        ExecuteQuery($@"UPDATE CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP SET PAYED_AMOUNT = {insuranceAmount.ToString(GlobalVariables.V_CultureInfoEN)}, IS_CHANGE = 1 WHERE CUSTOMER_PAYMENT_ID = {dr["ID"]}", "Sığorta ödənişi dəyişdirilmədi.", "UpdateCustomerPayment");
                    else
                        ExecuteQuery($@"UPDATE CRS_USER_TEMP.INSURANCE_PAYMENT_TEMP SET IS_CHANGE = 2 WHERE CUSTOMER_PAYMENT_ID = {dr["ID"]}", "Sığorta ödənişi silinmədi.", "UpdateCustomerPayment");

                    UpdateBalancePenalty(istemp, contractid.ToString(), operationdate);
                    payment_interest_debt = currenct_payment_interest_debt;

                    if (i > 0)
                    {
                        if (dr["BANK_CASH"].ToString() == "B")
                            operationaccount = GlobalFunctions.GetName($@"SELECT SUB_ACCOUNT FROM CRS_USER.ACCOUNTING_PLAN AP WHERE (AP.BANK_ID,AP.ID) IN (SELECT BANK_ID,ACCOUNTING_PLAN_ID FROM CRS_USER.BANK_OPERATIONS WHERE CONTRACT_PAYMENT_ID = {dr["ID"]})");
                        else
                            operationaccount = null;
                    }
                    i++;
                    InsertOperationJournal(s_date, Math.Round(currency_rate, 4), currencyid, Math.Round(payedAmount, 2), Math.Round(payedAmountAZN, 2), Math.Round(basic_amount, 2), payment_interest_amount, contractid.ToString(), dr[1].ToString(), operationaccount, istemp, o_date, (decimal)payed_penalty);
                }
            }
            catch (Exception exx)
            {
                LogWrite("Lizinq ödənişinin parametrləri tapılmadı.", s3, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void UpdateBalancePenalty(int istemp, string contractid, string baldate)
        {
            string s = null;
            double penalty_amount = 0, discount_penalty = 0, payment_penalty = 0, currentdebtpenalty = 0,
                debt = GlobalFunctions.GetAmount("SELECT NVL(SUM(DEBT_PENALTY),0) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP B WHERE ID = (SELECT MAX(ID) FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE <> 2 AND CUSTOMER_ID = B.CUSTOMER_ID AND CONTRACT_ID = B.CONTRACT_ID AND BAL_DATE <= TO_DATE('" + baldate + "','DD/MM/YYYY')) AND B.CONTRACT_ID = " + contractid);
            if (istemp == 1)
                s = "SELECT PENALTY_AMOUNT,DISCOUNT_PENALTY,PAYMENT_PENALTY,ID FROM CRS_USER_TEMP.BALANCE_PENALTIES_TEMP WHERE IS_CHANGE <> 2 AND CONTRACT_ID = " + contractid + " AND BAL_DATE > TO_DATE('" + baldate + "','DD/MM/YYYY') ORDER BY ID,BAL_DATE";
            else
                s = "SELECT PENALTY_AMOUNT,DISCOUNT_PENALTY,PAYMENT_PENALTY,ID FROM CRS_USER.CONTRACT_BALANCE_PENALTIES WHERE IS_COMMIT = 1 AND CONTRACT_ID = " + contractid + " AND BAL_DATE > TO_DATE('" + baldate + "','DD/MM/YYYY') ORDER BY ID,BAL_DATE";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, "UpdateBalancePenalty");
                foreach (DataRow dr in dt.Rows)
                {
                    penalty_amount = Convert.ToDouble(dr[0].ToString());
                    discount_penalty = Convert.ToDouble(dr[1].ToString());
                    payment_penalty = Convert.ToDouble(dr[2].ToString());
                    currentdebtpenalty = debt + penalty_amount - (discount_penalty + payment_penalty);
                    if (istemp == 1)
                        ExecuteQuery("UPDATE CRS_USER_TEMP.BALANCE_PENALTIES_TEMP SET DEBT_PENALTY = " + currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN) + ",IS_CHANGE = 1 WHERE CONTRACT_ID = " + contractid + " AND ID = " + dr[3],
                                                        "Cərimənin qalıqları temp cədvəldə dəyişdirilmədi.",
                                                        "UpdateBalancePenalty");
                    else
                        ExecuteQuery("UPDATE CRS_USER.CONTRACT_BALANCE_PENALTIES SET DEBT_PENALTY = " + currentdebtpenalty.ToString(GlobalVariables.V_CultureInfoEN) + ",IS_COMMIT = 1 WHERE CONTRACT_ID = " + contractid + " AND ID = " + dr[3],
                                                        "Cərimənin qalıqları əsas cədvəldə dəyişdirilmədi.",
                                                        "UpdateBalancePenalty");
                }
            }
            catch (Exception exx)
            {
                LogWrite("Cərimə faizinin parametrləri tapılmadı.", s, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void UpdateBankOperationDebt(string operationdate)
        {
            decimal prev_debt = 0;

            prev_debt = BankOperationsDAL.GeneralLastDebt(operationdate);

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
                    command.CommandText = "CRS_USER.PROC_UPDATE_BO_DEBT";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_PREV_DEBT", OracleDbType.Decimal).Value = prev_debt.ToString(GlobalVariables.V_CultureInfoEN);
                    command.Parameters.Add("P_OPERATION_DATE", OracleDbType.Varchar2).Value = operationdate;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite("Bankın qalığı dəyişdirilmədi.", "CRS_USER.PROC_UPDATE_BO_DEBT", GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void UpdateBankOperationDebtWithBank(string operationdate, int bank_id)
        {
            decimal prev_debt = 0;

            prev_debt = BankOperationsDAL.BankLastDebt(operationdate, bank_id);

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
                    command.CommandText = "CRS_USER.PROC_UPDATE_BO_DEBT_WITH_BANK";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_BANK_ID", OracleDbType.Int32).Value = bank_id;
                    command.Parameters.Add("P_PREV_DEBT", OracleDbType.Decimal).Value = prev_debt.ToString(GlobalVariables.V_CultureInfoEN);
                    command.Parameters.Add("P_OPERATION_DATE", OracleDbType.Varchar2).Value = operationdate;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite("Bankın qalığı dəyişdirilmədi.", "CRS_USER.PROC_UPDATE_BO_DEBT_WITH_BANK", GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void UpdateBankDebtWithChanges()
        {
            string s = null;
            try
            {
                s = $@"SELECT BANK_ID,TO_CHAR(OPERATION_DATE,'DD/MM/YYYY') OPERATION_DATE FROM CRS_USER_TEMP.BANK_OPERATIONS_TEMP_CHANGES WHERE USED_USER_ID = {GlobalVariables.V_UserID} ORDER BY BANK_ID";
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s).Rows)
                {
                    UpdateBankOperationDebtWithBank(dr["OPERATION_DATE"].ToString(), Convert.ToInt32(dr["BANK_ID"].ToString()));
                    UpdateBankOperationDebt(dr["OPERATION_DATE"].ToString());
                }
            }
            catch (Exception exx)
            {
                LogWrite("Məlumatlarının yeri dəyişən bankların siyahısı açılmadı.", s, GlobalVariables.V_UserName, "GlobalProcedures", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void ChangeOrderID(string tablename, string id, string type, out int orderid)
        {
            //int res = 0;
            //using (OracleConnection connection = new OracleConnection())
            //{
            //    OracleTransaction transaction = null;
            //    OracleCommand command = null;
            //    try
            //    {
            //        if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            //        {
            //            connection.ConnectionString = GlobalFunctions.GetConnectionString();
            //            connection.Open();
            //        }
            //        transaction = connection.BeginTransaction();
            //        command = connection.CreateCommand();
            //        command.Transaction = transaction;
            //        command.CommandText = "CRS_USER.PROC_CHANGE_ORDER_ID";
            //        command.CommandType = CommandType.StoredProcedure;
            //        command.Parameters.Add("P_TABLE_NAME", "VARCHAR2").Value = tablename;
            //        command.Parameters.Add("P_ID", "INTEGER").Value = id;
            //        command.Parameters.Add("P_CHANGE_TYPE", "VARCHAR2").Value = type;
            //        command.Parameters.Add("P_ORDER_ID", "INTEGER").Direction = ParameterDirection.Output;
            //        command.ExecuteNonQuery();
            //        res = Convert.ToInt32(command.Parameters.Add("P_ORDER_ID", "INTEGER").Value);
            //        transaction.Commit();
            //    }
            //    catch (Exception exx)
            //    {
            //        LogWrite(tablename + " cədvəlində sıra nömrəsi dəyişdirilmədi.", "CRS_USER.PROC_CHANGE_ORDER_ID", GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            //        transaction.Rollback();
            //    }
            //    finally
            //    {
            //        command.Dispose();
            //        connection.Dispose();
            //    }
            //}

            //orderid = res;

            int selected_order_id, previous_order_id, next_order_id;
            selected_order_id = GlobalFunctions.GetID("SELECT ORDER_ID FROM CRS_USER." + tablename + " WHERE ID = " + id);
            previous_order_id = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER." + tablename + " WHERE ORDER_ID < " + selected_order_id);
            next_order_id = GlobalFunctions.GetMax("SELECT NVL(MIN(ORDER_ID),0) FROM CRS_USER." + tablename + " WHERE ORDER_ID > " + selected_order_id);
            if (type == "up")
            {
                if (previous_order_id > 0)
                    ExecuteTwoQuery("UPDATE CRS_USER." + tablename + " SET ORDER_ID = " + selected_order_id + " WHERE ORDER_ID = " + previous_order_id,
                                    "UPDATE CRS_USER." + tablename + " SET ORDER_ID = " + previous_order_id + " WHERE ID = " + id,
                                                        tablename + " cədvəlində sıralama dəyişmədi.",
                                                        "ChangeOrderID");
                orderid = GlobalFunctions.GetID("SELECT R FROM (SELECT ORDER_ID,ROW_NUMBER() OVER (ORDER BY ORDER_ID) R FROM CRS_USER." + tablename + " ORDER BY ORDER_ID) WHERE ORDER_ID = " + previous_order_id);
            }
            else
            {
                if (next_order_id > 0)
                    GlobalProcedures.ExecuteTwoQuery("UPDATE CRS_USER." + tablename + " SET ORDER_ID = " + selected_order_id + " WHERE ORDER_ID = " + next_order_id,
                                                     "UPDATE CRS_USER." + tablename + " SET ORDER_ID = " + next_order_id + " WHERE ID = " + id,
                                                        tablename + " cədvəlində sıralama dəyişmədi.",
                                                        "ChangeOrderID");
                orderid = GlobalFunctions.GetID("SELECT R FROM (SELECT ORDER_ID,ROW_NUMBER() OVER (ORDER BY ORDER_ID) R FROM CRS_USER." + tablename + " ORDER BY ORDER_ID) WHERE ORDER_ID = " + next_order_id);
            }
        }

        public static void ChangeOrderIDforTEMP(string tablename, string id, string type, out int orderid)
        {
            int selected_order_id, previous_order_id, next_order_id;
            selected_order_id = GlobalFunctions.GetID("SELECT ORDER_ID FROM CRS_USER_TEMP." + tablename + " WHERE ID = " + id);
            previous_order_id = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER_TEMP." + tablename + " WHERE ORDER_ID < " + selected_order_id);
            next_order_id = GlobalFunctions.GetMax("SELECT NVL(MIN(ORDER_ID),0) FROM CRS_USER_TEMP." + tablename + " WHERE ORDER_ID > " + selected_order_id);
            if (type == "up")
            {
                if (previous_order_id > 0)
                    ExecuteTwoQuery("UPDATE CRS_USER_TEMP." + tablename + " SET ORDER_ID = " + selected_order_id + ",IS_CHANGE = 1 WHERE ORDER_ID = " + previous_order_id,
                                                     "UPDATE CRS_USER_TEMP." + tablename + " SET ORDER_ID = " + previous_order_id + ",IS_CHANGE = 1 WHERE ID = " + id,
                                                        tablename + " cədvəlində sıralama dəyişmədi.",
                                                        "ChangeOrderIDforTEMP");
                orderid = GlobalFunctions.GetID("SELECT R FROM (SELECT ORDER_ID,ROW_NUMBER() OVER (ORDER BY ORDER_ID) R FROM CRS_USER_TEMP." + tablename + " ORDER BY ORDER_ID) WHERE ORDER_ID = " + previous_order_id);
            }
            else
            {
                if (next_order_id > 0)
                    ExecuteTwoQuery("UPDATE CRS_USER_TEMP." + tablename + " SET ORDER_ID = " + selected_order_id + ",IS_CHANGE = 1 WHERE ORDER_ID = " + next_order_id,
                                                     "UPDATE CRS_USER_TEMP." + tablename + " SET ORDER_ID = " + next_order_id + ",IS_CHANGE = 1 WHERE ID = " + id,
                                                        tablename + " cədvəlində sıralama dəyişmədi.",
                                                        "ChangeOrderIDforTEMP");
                orderid = GlobalFunctions.GetID("SELECT R FROM (SELECT ORDER_ID,ROW_NUMBER() OVER (ORDER BY ORDER_ID) R FROM CRS_USER_TEMP." + tablename + " ORDER BY ORDER_ID) WHERE ORDER_ID = " + next_order_id);
            }
        }

        public static void LoadFontStyleComboBox(ComboBoxEdit cb)
        {
            cb.Properties.Items.Add(FontStyle.Regular.ToString());
            cb.Properties.Items.Add(FontStyle.Bold.ToString());
            cb.Properties.Items.Add(FontStyle.Italic.ToString());
        }

        public static void FindFontDetails(decimal a, RowCellStyleEventArgs e)
        {
            string s = "SELECT FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_R,BACKCOLOR_G,BACKCOLOR_B,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_R,FONTCOLOR_G,FONTCOLOR_B,FONTCOLOR_TYPE,FONTCOLOR_NAME FROM CRS_USER.PORTFEL_FONTS WHERE " + a.ToString(GlobalVariables.V_CultureInfoEN) + " BETWEEN START_VALUE AND END_VALUE";
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s, "GlobalProcedures/FindFontDetails", "Portfelin rəngi açılmadı.").Rows)
                {
                    switch (dr[2].ToString())
                    {
                        case "Regular":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Regular);
                            break;
                        case "Bold":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Bold);
                            break;
                        case "Italic":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Italic);
                            break;
                        case "Underline":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Underline);
                            break;
                        case "Strikeout":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Strikeout);
                            break;
                    }

                    e.Appearance.BackColor = GlobalFunctions.CreateColor(dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                    e.Appearance.ForeColor = GlobalFunctions.CreateColor(dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), dr[13].ToString(), dr[14].ToString());
                }
            }
            catch (Exception exx)
            {
                LogWrite("Portfelin fontları açılmadı.", s, GlobalVariables.V_UserName, "GlobalProcedures", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void FindFontDetailsforCreditType(int a, RowCellStyleEventArgs e)
        {
            string s = "SELECT FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_R,BACKCOLOR_G,BACKCOLOR_B,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_R,FONTCOLOR_G,FONTCOLOR_B,FONTCOLOR_TYPE,FONTCOLOR_NAME FROM CRS_USER.CREDIT_TYPE_FONTS WHERE CREDIT_TYPE_ID = " + a;
            try
            {
                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s).Rows)
                {
                    switch (dr[2].ToString())
                    {
                        case "Regular":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Regular);
                            break;
                        case "Bold":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Bold);
                            break;
                        case "Italic":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Italic);
                            break;
                        case "Underline":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Underline);
                            break;
                        case "Strikeout":
                            e.Appearance.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Strikeout);
                            break;
                    }
                    if (Convert.ToInt32(dr[3].ToString()) != 0 || Convert.ToInt32(dr[4].ToString()) != 0 || Convert.ToInt32(dr[5].ToString()) != 0 || Convert.ToInt32(dr[6].ToString()) != 0)
                        e.Appearance.BackColor = GlobalFunctions.CreateColor(dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                    else
                        e.Appearance.BackColor = GlobalFunctions.ElementColor();
                    if (Convert.ToInt32(dr[9].ToString()) != 0 || Convert.ToInt32(dr[10].ToString()) != 0 || Convert.ToInt32(dr[11].ToString()) != 0 || Convert.ToInt32(dr[12].ToString()) != 0)
                        e.Appearance.ForeColor = GlobalFunctions.CreateColor(dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), dr[13].ToString(), dr[14].ToString());
                    else
                        e.Appearance.ForeColor = GlobalFunctions.ElementColor();
                }
            }
            catch (Exception exx)
            {
                LogWrite("Kodun fontları açılmadı.", s, GlobalVariables.V_UserName, "GlobalProcedures", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void UpdateFundChange(string operationdate, int contractid, int istemp)
        {
            string s1 = null,
                s2 = null,
                s3 = null,
                first_date = null,
                s_date = null,
                contract_start_date = null;

            double first_debt = 0,
                first_payment_interest_debt = 0,
                one_day_interest = 0,
                interest_amount = 0,
                totaldebtamount = 0,
                debt = 0,
                payment_interest_amount = 0,
                current_payment_interest_debt = 0,
                basic_amount = 0,
                current_debt = 0,
                total = 0,
                paymentamount = 0,
                payment_interest_debt = 0,
                buyamount = 0,
                contract_amount = 0;

            int diff_day, i = 0, currencyid;

            decimal interest = 0;

            try
            {

                s2 = $@"SELECT CURRENCY_ID,INTEREST,AMOUNT,TO_CHAR(START_DATE,'DD.MM.YYYY') S_DATE FROM CRS_USER.FUNDS_CONTRACTS WHERE ID = {contractid}";
                DataTable dt1 = GlobalFunctions.GenerateDataTable(s2, "UpdateFundChange");
                if (dt1.Rows.Count > 0)
                {
                    currencyid = Convert.ToInt32(dt1.Rows[0]["CURRENCY_ID"].ToString());
                    contract_amount = Convert.ToDouble(dt1.Rows[0]["AMOUNT"].ToString());
                    contract_start_date = dt1.Rows[0]["S_DATE"].ToString();
                }

                //if (interest == 0)
                //{
                //    List<FundContractPercent> lstPercent = FundContractPercentDAL.SelectFundContractPercentByContractID(contractid).ToList<FundContractPercent>();
                //    if (lstPercent.Count > 0)
                //    {
                //        int lastPercentID = lstPercent.Where(item => item.PDATE <= GlobalFunctions.ChangeStringToDate(operationdate, "ddmmyyyy")).Max(item => item.ID);
                //        interest = lstPercent.Find(item => item.ID == lastPercentID).PERCENT_VALUE;
                //    }
                //}

                //ozunden qabaqki odenisi tap
                if (istemp == 1)
                    s1 = $@"SELECT TO_CHAR(CP.PAYMENT_DATE,'DD/MM/YYYY') PAYMENT_DATE,CP.DEBT,CP.PAYMENT_INTEREST_DEBT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.ID = (SELECT MAX(ID) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE = (SELECT MAX(PAYMENT_DATE) FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP WHERE PAYMENT_DATE < TO_DATE('{operationdate}','DD/MM/YYYY') AND CONTRACT_ID = {contractid} AND USED_USER_ID = {GlobalVariables.V_UserID}))";
                else
                    s1 = $@"SELECT TO_CHAR(CP.PAYMENT_DATE,'DD/MM/YYYY') PAYMENT_DATE,CP.DEBT,CP.PAYMENT_INTEREST_DEBT FROM CRS_USER.FUNDS_PAYMENTS CP WHERE CP.ID = (SELECT MAX(ID) FROM CRS_USER.FUNDS_PAYMENTS WHERE PAYMENT_DATE = (SELECT MAX(PAYMENT_DATE) FROM CRS_USER.FUNDS_PAYMENTS WHERE PAYMENT_DATE < TO_DATE('{operationdate}','DD/MM/YYYY') AND CONTRACT_ID = {contractid}))";
                DataTable dt = GlobalFunctions.GenerateDataTable(s1, "UpdateFundChange");
                if (dt.Rows.Count > 0)
                {
                    first_date = dt.Rows[0]["PAYMENT_DATE"].ToString();
                    first_debt = Convert.ToDouble(dt.Rows[0]["DEBT"].ToString());
                    first_payment_interest_debt = Convert.ToDouble(dt.Rows[0]["PAYMENT_INTEREST_DEBT"].ToString());
                }

                if (istemp == 1)
                    s3 = $@"SELECT TO_CHAR(CP.PAYMENT_DATE,'DD/MM/YYYY') P_DATE,CP.ID,CP.CONTRACT_ID,CP.PAYMENT_AMOUNT,CP.BUY_AMOUNT,CP.MANUAL_INTEREST,CP.PAYMENT_INTEREST_AMOUNT,CP.PERCENT_TYPE,PERCENT FROM CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP CP WHERE CP.IS_CHANGE IN (0,1) AND CP.PAYMENT_DATE >= TO_DATE('" + operationdate + "','DD/MM/YYYY') AND CONTRACT_ID = " + contractid + " AND CP.USED_USER_ID = " + GlobalVariables.V_UserID + " ORDER BY PAYMENT_DATE,ID";
                else
                    s3 = $@"SELECT TO_CHAR(CP.PAYMENT_DATE,'DD/MM/YYYY') P_DATE,CP.ID,CP.CONTRACT_ID,CP.PAYMENT_AMOUNT,CP.BUY_AMOUNT,CP.MANUAL_INTEREST,CP.PAYMENT_INTEREST_AMOUNT,CP.PERCENT_TYPE,PERCENT FROM CRS_USER.FUNDS_PAYMENTS CP WHERE CP.PAYMENT_DATE >= TO_DATE('" + operationdate + "','DD/MM/YYYY') AND CONTRACT_ID = " + contractid + " ORDER BY PAYMENT_DATE,ID";

                foreach (DataRow dr in GlobalFunctions.GenerateDataTable(s3, "UpdateFundChange").Rows)
                {
                    if (i == 0)
                    {
                        debt = first_debt;
                        payment_interest_debt = first_payment_interest_debt;
                        if (!String.IsNullOrEmpty(first_date))
                            s_date = first_date;
                        else
                            s_date = operationdate;
                    }
                    else
                        debt = current_debt;
                    interest = Convert.ToDecimal(dr["PERCENT"]);
                    one_day_interest = Math.Round(((debt * (double)interest) / 100) / 360, 2);
                    diff_day = GlobalFunctions.Days360(GlobalFunctions.ChangeStringToDate(s_date, "ddmmyyyy"), GlobalFunctions.ChangeStringToDate(dr[0].ToString(), "ddmmyyyy"));
                    interest_amount = diff_day * one_day_interest;
                    paymentamount = Convert.ToDouble(dr[3].ToString());
                    buyamount = Convert.ToDouble(dr[4].ToString());
                    if (Convert.ToInt32(dr["MANUAL_INTEREST"].ToString()) == 0)
                    {
                        if (paymentamount > 0 && Convert.ToInt32(dr["PERCENT_TYPE"].ToString()) == 0) // eger odenis sifirdan boyuk olarsa
                        {
                            if ((interest_amount + payment_interest_debt) > paymentamount) // eger hesablanan faizle qaliq faizin cemi edenisden boyuk olarsa onda odenilen faiz ele odenisin meblegi olur
                                payment_interest_amount = paymentamount;
                            else
                                payment_interest_amount = interest_amount + payment_interest_debt; // eks halda odenilen faiz hesablanan faizle qaliq faizin cemi olur
                        }
                        else if (paymentamount == 0 && Convert.ToInt32(dr["PERCENT_TYPE"].ToString()) == 0)
                            payment_interest_amount = interest_amount;
                        else
                            payment_interest_amount = 0;
                    }
                    else
                        payment_interest_amount = Convert.ToDouble(dr[6].ToString());

                    current_payment_interest_debt = payment_interest_debt + interest_amount - payment_interest_amount;
                    basic_amount = paymentamount - payment_interest_amount;
                    basic_amount = (basic_amount < 0) ? 0 : basic_amount;
                    current_debt = buyamount + debt - basic_amount;
                    total = current_debt + current_payment_interest_debt;
                    totaldebtamount = Math.Round((debt + current_payment_interest_debt + interest_amount), 2);

                    i++;
                    s_date = dr["P_DATE"].ToString();
                    payment_interest_debt = current_payment_interest_debt;
                    if (istemp == 1)
                        ExecuteQuery($@"UPDATE CRS_USER_TEMP.FUNDS_PAYMENTS_TEMP SET IS_CHANGE = 1, BASIC_AMOUNT = " + Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DAY_COUNT = " + diff_day + ",ONE_DAY_INTEREST_AMOUNT = " + Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",INTEREST_AMOUNT = " + Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",PAYMENT_INTEREST_AMOUNT = " + Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",PAYMENT_INTEREST_DEBT = " + Math.Round(current_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",REQUIRED_CLOSE_AMOUNT = " + Math.Round(totaldebtamount, 2).ToString(GlobalVariables.V_CultureInfoEN) + " WHERE CONTRACT_ID = " + contractid + " AND ID = " + dr[1] + " AND USED_USER_ID = " + GlobalVariables.V_UserID,
                                                        "Cəlb olunmuş vəsaitlərin qalığı temp cədvəldə dəyişdirilmədi.",
                                                        "UpdateFundChange");
                    else
                    {
                        if (buyamount == 0 && paymentamount == 0)
                        {
                            ExecuteQuery($@"DELETE FROM CRS_USER.FUNDS_PAYMENTS WHERE CONTRACT_ID = {contractid} AND ID = {dr[1]}",
                                                        "Cəlb olunmuş vəsaitlərin qalığı əsas cədvəldən silinmədi.",
                                                "UpdateFundChange");
                            UpdateFundChange(operationdate, contractid, istemp);
                        }
                        else
                            ExecuteQuery($@"UPDATE CRS_USER.FUNDS_PAYMENTS SET BASIC_AMOUNT = " + Math.Round(basic_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DEBT = " + Math.Round(current_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",DAY_COUNT = " + diff_day + ",ONE_DAY_INTEREST_AMOUNT = " + Math.Round(one_day_interest, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",INTEREST_AMOUNT = " + Math.Round(interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",PAYMENT_INTEREST_AMOUNT = " + Math.Round(payment_interest_amount, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",PAYMENT_INTEREST_DEBT = " + Math.Round(current_payment_interest_debt, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",TOTAL = " + Math.Round(total, 2).ToString(GlobalVariables.V_CultureInfoEN) + ",REQUIRED_CLOSE_AMOUNT = " + Math.Round(totaldebtamount, 2).ToString(GlobalVariables.V_CultureInfoEN) + " WHERE CONTRACT_ID = " + contractid + " AND ID = " + dr[1],
                                                        "Cəlb olunmuş vəsaitlərin qalığı əsas cədvəldə dəyişdirilmədi.",
                                                        "UpdateFundChange");
                    }
                }
            }
            catch (Exception exx)
            {
                LogWrite("Cəlb olunmuş vəsaitlərin parametrləri tapılmadı.", s3, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void LoadReceipt(string date, double amount, int currency, decimal currency_rate, string customername, int contractid, string contractcode, string description)
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Qebz.docx"))
            {
                GlobalVariables.WordDocumentUsed = false;
                XtraMessageBox.Show("Qəbzin şablon faylı " + GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında yoxdur. Zəhmət olmasa şablon faylı yaradıb göstərilən ünvana yerləşdirin.");
                return;
            }

            if (currency <= 0)
            {
                XtraMessageBox.Show("Likvid dəyərin valyutası daxil edilməyib.");
                return;
            }

            string amount_with_word,
                day = String.Format("{0:dd}", GlobalFunctions.ChangeStringToDate(date, "ddmmyyyy")),
                month = GlobalFunctions.FindMonth(GlobalFunctions.ChangeStringToDate(date, "ddmmyyyy").Month),
                year = GlobalFunctions.ChangeStringToDate(date, "ddmmyyyy").Year.ToString(),
                rate = "",
                paymentname,
                commitment_name = null;
            double payment_value = Math.Round(amount, 2);
            if (currency != 1)
                rate = "Kurs: " + currency_rate;

            amount_with_word = GlobalFunctions.AmountInWritining(payment_value, currency, true);

            if (contractid > 0)
            {
                commitment_name = GlobalFunctions.GetName("SELECT CC.COMMITMENT_NAME FROM CRS_USER.V_COMMITMENTS CC WHERE CC.CONTRACT_ID = " + contractid);
                if (String.IsNullOrEmpty(commitment_name))
                    paymentname = customername;
                else
                    paymentname = commitment_name;
                description = contractcode + " " + description;
            }
            else
                paymentname = customername;

            try
            {
                Document document = new Document();
                document.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\Qebz.docx");

                document.ReplaceText("[$day]", day);
                document.ReplaceText("[$month]", month);
                document.ReplaceText("[$year]", year);
                document.ReplaceText("[$customer]", paymentname);
                document.ReplaceText("[$amount]", payment_value.ToString("N2"));
                document.ReplaceText("[$amountwithword]", amount_with_word);
                document.ReplaceText("[$details]", description);
                document.ReplaceText("[$rate]", rate);
                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + contractcode + "_Qebz.docx"))
                    File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + contractcode + "_Qebz.docx");
                document.Save(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + contractcode + "_Qebz.docx");
                //GlobalVariables.WordDocumentUsed = true;
                Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + contractcode + "_Qebz.docx");
            }
            catch
            {
                XtraMessageBox.Show(contractcode + "_Qebz.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
        }

        public static void LoadBankReceipt(string date, double amount, double azn_amount, int currency, decimal currency_rate, string currency_code, string customername, int contractid, string contractcode, string description, string payer)
        {
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mədaxil orderi.docx"))
            {
                XtraMessageBox.Show("Mədaxil orderinin şablon faylı " + GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında yoxdur. Zəhmət olmasa şablon faylı yaradıb göstərilən ünvana yerləşdirin.");
                return;
            }

            string amount_with_word = null,
                rate = "",
                account = "",
                payment_value = "";

            double pay_amount = 0;

            if (currency == 1)
            {
                payment_value = amount.ToString("N2") + " AZN";
                pay_amount = amount;
            }
            else
            {
                payment_value = azn_amount.ToString("N2") + " AZN (" + amount.ToString("N2") + " " + currency_code + ")";
                pay_amount = azn_amount;
                rate = "      [ Kurs: " + currency_rate.ToString("N4") + " ]";
            }
            account = GlobalFunctions.GetName("SELECT CUSTOMER_ACCOUNT FROM CRS_USER.CONTRACTS WHERE ID = " + contractid);
            amount_with_word = GlobalFunctions.AmountInWritining(Math.Round(pay_amount, 2), 1, true);

            description = contractcode + " " + description;

            try
            {
                Document document = new Document();
                document.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mədaxil orderi.docx");

                document.ReplaceText("[$date]", date);
                document.ReplaceText("[$customer]", customername);
                document.ReplaceText("[$account]", account);
                document.ReplaceText("[$amount]", payment_value);
                document.ReplaceText("[$amountwithword]", amount_with_word);
                document.ReplaceText("[$details]", description);
                document.ReplaceText("[$rate]", rate.Trim());
                document.ReplaceText("[$payer]", payer);
                string path = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + contractcode.Replace("/", string.Empty) + "_MədaxilOrderi.docx";
                if (File.Exists(path))
                    File.Delete(path);
                document.Save(path);

                Process.Start(path);
            }
            catch
            {
                XtraMessageBox.Show(contractcode + "_Mədaxil orderi.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            }
        }

        public static void LoadExpensesReceipt(string paymentdate, double amount, string seller, string card, string contractcode, string reason)
        {


            //string amount_with_word,
            //    day = String.Format("{0:dd}", GlobalFunctions.ChangeStringToDate(paymentdate, "ddmmyyyy")),
            //    month = GlobalFunctions.FindMonth(GlobalFunctions.ChangeStringToDate(paymentdate, "ddmmyyyy").Month),
            //    year = GlobalFunctions.FindYear(GlobalFunctions.ChangeStringToDate(paymentdate, "ddmmyyyy"));

            //amount_with_word = GlobalFunctions.AmountInWritining(amount, 1, false);

            //rv_expenditure.LocalReport.ReportPath = GlobalVariables.V_ExecutingFolder + "\\RDLC\\Expenditure.rdlc";
            //ReportParameter p1 = new ReportParameter("PContractCode", contractcode);
            //ReportParameter p2 = new ReportParameter("PName", seller);
            //ReportParameter p3 = new ReportParameter("PReason", reason);
            //ReportParameter p4 = new ReportParameter("PAmountWithString", amount_with_word);
            //ReportParameter p5 = new ReportParameter("PCard", card);
            //ReportParameter p6 = new ReportParameter("PMonth", month);
            //ReportParameter p7 = new ReportParameter("PDay", "«" + day + "»");
            //ReportParameter p8 = new ReportParameter("PYear", year);
            //ReportParameter p9 = new ReportParameter("PAmount", amount.ToString("N2"));
            //rv_expenditure.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 });

            //Warning[] warnings;
            //try
            //{
            //    if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Reports\\" + contractcode + "_Məxaric.doc"))
            //        File.Delete(GlobalVariables.V_ExecutingFolder + "\\Reports\\" + contractcode + "_Məxaric.doc");
            //    using (var stream = File.Create(GlobalVariables.V_ExecutingFolder + "\\Reports\\" + contractcode + "_Məxaric.doc"))
            //    {
            //        rv_expenditure.LocalReport.Render(
            //            "WORD",
            //            @"<DeviceInfo><ExpandContent>True</ExpandContent></DeviceInfo>",
            //            (CreateStreamCallback)delegate { return stream; },
            //            out warnings);
            //    }

            //    if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Reports\\" + contractcode + "_Məxaric.doc"))
            //        Process.Start(GlobalVariables.V_ExecutingFolder + "\\Reports\\" + contractcode + "_Məxaric.doc");
            //    else
            //        XtraMessageBox.Show("Məxaricin çap faylı yaradılmayıb.");
            //}
            //catch
            //{
            //    XtraMessageBox.Show(contractcode + "_Məxaric.doc faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.");
            //}
            //finally
            //{
            //    rv_expenditure.Reset();
            //}
        }

        public static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                XtraMessageBox.Show("Excel faylı bağlanmadı. " + "\nError - " + ex.Message, "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void InsertOperationJournalForSeller(string date, double amount, double first_payment, string contractid, string liquid_account, string first_payment_account)
        {
            if (GlobalFunctions.ChangeStringToDate(date, "ddmmyyyy") >= GlobalFunctions.ChangeStringToDate("31/12/2015", "ddmmyyyy"))
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
                        command.CommandText = "CRS_USER.PROC_SELLER_OPERATION_JOURNAL";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("P_DATE", OracleDbType.Varchar2).Value = date;
                        command.Parameters.Add("P_AMOUNT", OracleDbType.Double).Value = amount;
                        command.Parameters.Add("P_FIRST_PAYMENT", OracleDbType.Double).Value = first_payment;
                        command.Parameters.Add("P_CONTRACT_ID", OracleDbType.Int32).Value = int.Parse(contractid);
                        command.Parameters.Add("P_LACCOUNT", OracleDbType.Varchar2).Value = liquid_account;
                        command.Parameters.Add("P_FIRST_PAYMENT_ACCOUNT", OracleDbType.Varchar2).Value = first_payment_account;
                        command.Parameters.Add("P_USED_USER_ID", OracleDbType.Varchar2).Value = GlobalVariables.V_UserID;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception exx)
                    {
                        LogWrite("Alqı-satqı üzrə mühasibat əməliyyatları bazaya daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                        transaction.Rollback();
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Dispose();
                    }
                }
            }
            else
                XtraMessageBox.Show("31.12.2015 tarixindən qabaqkı tarixlər üçün satıcının əməliyyatları Mühasibatlıqda jurnala daxil ola bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void InsertOperationJournal(string date, double currency_rate, int currency_id, double amount, double amount_azn, double basic_amount, double interest_amount, string contractid, string paymentid, string owner_account, int istemp, string operationDate, decimal penalty = 0)
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
                    command.CommandText = "CRS_USER.PROC_INSERT_OPERATION_JOURNAL";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_DATE", OracleDbType.Varchar2).Value = date;
                    command.Parameters.Add("P_CURRENCY_RATE", OracleDbType.Double).Value = currency_rate;
                    command.Parameters.Add("P_CURRENCY_ID", OracleDbType.Int32).Value = currency_id;
                    command.Parameters.Add("P_AMOUNT", OracleDbType.Double).Value = amount;
                    command.Parameters.Add("P_AMOUNT_AZN", OracleDbType.Double).Value = amount_azn;
                    command.Parameters.Add("P_BASIC_AMOUNT", OracleDbType.Double).Value = basic_amount;
                    command.Parameters.Add("P_INTEREST_AMOUNT", OracleDbType.Double).Value = interest_amount;
                    command.Parameters.Add("P_CONTRACT_ID", OracleDbType.Int32).Value = int.Parse(contractid);
                    command.Parameters.Add("P_PAYMENT_ID", OracleDbType.Int32).Value = int.Parse(paymentid);
                    command.Parameters.Add("P_OWNER_ACCOUNT", OracleDbType.Varchar2).Value = owner_account;
                    command.Parameters.Add("P_IS_TEMP", OracleDbType.Int32).Value = istemp;
                    command.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                    command.Parameters.Add("P_CLEARING_DATE", OracleDbType.Varchar2).Value = operationDate;
                    command.Parameters.Add("P_PENALTY", OracleDbType.Double).Value = penalty;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite("Əməliyatlar jurnala daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }

            //string appointment,
            //    debit_account,
            //    credit_account,
            //    customer_account,
            //    leasing_account,
            //    leasing_interest_account,
            //    account,
            //    account612,
            //    account712,
            //    account631,
            //    contract_code;

            //double amount_cur = 0;

            //try
            //{
            //    List<Contract> lstContract = ContractDAL.SelectContract(int.Parse(contractid)).ToList<Contract>();
            //    if (lstContract.Count == 0)
            //        return;

            //    var contract = lstContract.First();
            //    contract_code = contract.CODE;
            //    customer_account = contract.CUSTOMER_ACCOUNT;
            //    leasing_account = contract.LEASING_ACCOUNT;
            //    leasing_interest_account = contract.LEASING_INTEREST_ACCOUNT;

            //    List<AccountingPlan> lstAccountPlan = AccountingPlanDAL.SelectAccountingPlan(null).ToList<AccountingPlan>();
            //    if (String.IsNullOrEmpty(owner_account))
            //        account = lstAccountPlan.Find(p => p.ACCOUNT_NUMBER == 221).ACCOUNT;
            //    else
            //        account = owner_account;
            //    account612 = lstAccountPlan.Find(p => p.ACCOUNT_NUMBER == 612).ACCOUNT;
            //    account712 = lstAccountPlan.Find(p => p.ACCOUNT_NUMBER == 712).ACCOUNT;
            //    account631 = lstAccountPlan.Find(p => p.ACCOUNT_NUMBER == 631).ACCOUNT;

            //    if (istemp == 1)
            //        ExecuteQuery("DELETE FROM CRS_USER_TEMP.OPERATION_JOURNAL_TEMP WHERE OPERATION_DATE = TO_DATE('" + date + "','DD/MM/YYYY') AND CONTRACT_ID = " + contractid + " AND CUSTOMER_PAYMENT_ID = " + paymentid,
            //                  "Əməliyyatlar jurnaldan silinmədi.");
            //    else
            //        ExecuteQuery("DELETE FROM CRS_USER.OPERATION_JOURNAL WHERE OPERATION_DATE = TO_DATE('" + date + "','DD/MM/YYYY') AND CONTRACT_ID = " + contractid + " AND CUSTOMER_PAYMENT_ID = " + paymentid,
            //                  "Əməliyyatlar jurnaldan silinmədi.");

            //    //221 - 543
            //    appointment = contract_code + " saylı müqavilə üzrə lizinq ödənişi";
            //    debit_account = account;
            //    credit_account = 543 + customer_account;
            //    //amount_azn = Math.Round(amount * currency_rate, 2);
            //    if (currency_id != 1)
            //        amount_cur = Math.Round(amount, 2);
            //    if (istemp == 1)
            //        ExecuteQuery("INSERT INTO CRS_USER_TEMP.OPERATION_JOURNAL_TEMP(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,IS_CHANGE,USED_USER_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1,1," + GlobalVariables.V_UserID + "," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 account + " hesabının debitinə və 543 hesabının kreditinə lizinq ödənişi daxil edilmədi.");
            //    else
            //        ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 account + " hesabının debitinə və 543 hesabının kreditinə lizinq ödənişi daxil edilmədi.");

            //    //216 - 631
            //    appointment = contract_code + " saylı müqavilə üzrə hesablanmış faiz";
            //    debit_account = 216 + leasing_interest_account;
            //    credit_account = account631;
            //    amount_azn = Math.Round(interest_amount * currency_rate, 2);
            //    if (currency_id != 1)
            //        amount_cur = Math.Round(interest_amount, 2);
            //    if (istemp == 1)
            //        ExecuteQuery("INSERT INTO CRS_USER_TEMP.OPERATION_JOURNAL_TEMP(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,IS_CHANGE,USED_USER_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1,1," + GlobalVariables.V_UserID + "," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 "216 hesabının debitinə və 631 hesabının kreditinə hesablanmış faiz daxil edilmədi.");
            //    else
            //        ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 "216 hesabının debitinə və 631 hesabının kreditinə hesablanmış faiz daxil edilmədi.");

            //    //543 - 216
            //    appointment = contract_code + " saylı müqavilə üzrə silinən faiz";
            //    debit_account = 543 + customer_account;
            //    credit_account = 216 + leasing_interest_account;
            //    amount_azn = Math.Round(interest_amount * currency_rate, 2);
            //    if (currency_id != 1)
            //        amount_cur = Math.Round(interest_amount, 2);
            //    if (istemp == 1)
            //        ExecuteQuery("INSERT INTO CRS_USER_TEMP.OPERATION_JOURNAL_TEMP(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,IS_CHANGE,USED_USER_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1,1," + GlobalVariables.V_UserID + "," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 "543 hesabının debitinə və 216 hesabının kreditinə silinən faiz daxil edilmədi.");
            //    else
            //        ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 "543 hesabının debitinə və 216 hesabının kreditinə silinən faiz daxil edilmədi.");

            //    //543 - 174
            //    appointment = contract_code + " saylı müqavilə üzrə silinən əsas məbləğ";
            //    debit_account = 543 + customer_account;
            //    credit_account = 174 + leasing_account;
            //    amount_azn = Math.Round(basic_amount * currency_rate, 2);
            //    if (currency_id != 1)
            //        amount_cur = Math.Round(basic_amount, 2);
            //    if (istemp == 1)
            //        ExecuteQuery("INSERT INTO CRS_USER_TEMP.OPERATION_JOURNAL_TEMP(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,IS_CHANGE,USED_USER_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1,1," + GlobalVariables.V_UserID + "," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 "543 hesabının debitinə və 174 hesabının kreditinə silinən əsas məbləğ daxil edilmədi.");
            //    else
            //        ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(currency_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                 "543 hesabının debitinə və 174 hesabının kreditinə silinən əsas məbləğ daxil edilmədi.");


            //    if (currency_id != 1)
            //    {
            //        double contract_rate = GlobalFunctions.GetAmount("SELECT CURRENCY_RATE FROM CRS_USER.CONTRACTS WHERE ID = " + contractid), diff_rate = 0;
            //        diff_rate = currency_rate - contract_rate;

            //        if (diff_rate > 0)
            //        {
            //            //174 - 612                        
            //            appointment = contract_code + " saylı müqavilə üzrə məzənnə fərqindən gəlir";
            //            debit_account = 174 + leasing_account;
            //            credit_account = account612;
            //            amount_azn = Math.Round(basic_amount * diff_rate, 2);
            //            if (currency_id != 1)
            //                amount_cur = Math.Round(basic_amount, 2);
            //            if (istemp == 1)
            //                ExecuteQuery("INSERT INTO CRS_USER_TEMP.OPERATION_JOURNAL_TEMP(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,IS_CHANGE,USED_USER_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(diff_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1,1," + GlobalVariables.V_UserID + "," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                         "174 hesabının debitinə və 612 hesabının kreditinə məzənnə fərqindən gəlir daxil edilmədi.");
            //            else
            //                ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Round(diff_rate, 4).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                         "174 hesabının debitinə və 612 hesabının kreditinə məzənnə fərqindən gəlir daxil edilmədi.");
            //        }
            //        else
            //        {
            //            //712 - 174
            //            appointment = contract_code + " saylı müqavilə üzrə məzənnə fərqindən xərc";
            //            debit_account = account712;
            //            credit_account = 174 + leasing_account;
            //            amount_azn = Math.Abs(Math.Round(basic_amount * diff_rate, 2));
            //            if (currency_id != 1)
            //                amount_cur = Math.Round(basic_amount, 2);
            //            if (istemp == 1)
            //                ExecuteQuery("INSERT INTO CRS_USER_TEMP.OPERATION_JOURNAL_TEMP(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,IS_CHANGE,USED_USER_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Abs(Math.Round(diff_rate, 4)).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1,1," + GlobalVariables.V_UserID + "," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                         "712 hesabının debitinə və 174 hesabının kreditinə məzənnə fərqindən xərc daxil edilmədi.");
            //            else
            //                ExecuteQuery("INSERT INTO CRS_USER.OPERATION_JOURNAL(OPERATION_DATE,DEBIT_ACCOUNT,CREDIT_ACCOUNT,CURRENCY_RATE,AMOUNT_CUR,AMOUNT_AZN,APPOINTMENT,CONTRACT_ID,CUSTOMER_PAYMENT_ID,ACCOUNT_OPERATION_TYPE_ID,YR_MNTH_DY)VALUES(TO_DATE('" + date + "','DD/MM/YYYY'),'" + debit_account + "','" + credit_account + "'," + Math.Abs(Math.Round(diff_rate, 4)).ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_cur.ToString(GlobalVariables.V_CultureInfoEN) + "," + amount_azn.ToString(GlobalVariables.V_CultureInfoEN) + ",'" + appointment + "'," + contractid + "," + paymentid + ",1," + GlobalFunctions.ConvertDateToInteger(date, "ddmmyyyy") + ")",
            //                         "712 hesabının debitinə və 174 hesabının kreditinə məzənnə fərqindən xərc daxil edilmədi.");
            //        }
            //    }

            //}
            //catch (Exception exx)
            //{
            //    LogWrite("Əməliyatlar jurnalın temp cədvəlinə daxil edilmədi.", null, GlobalVariables.V_UserName, "GlobalProcedures", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            //}
        }

        public static void InsertAgreementOperationJournal(string date, double currency_rate, int currency_id, double amount, double amount_azn, double basic_amount, double interest_amount, string contractid, string paymentid, string owner_account, int istemp)
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
                    command.CommandText = "CRS_USER.PROC_AGREEMENT_JOURNAL";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_DATE", OracleDbType.Varchar2).Value = date;
                    command.Parameters.Add("P_CURRENCY_RATE", OracleDbType.Double).Value = currency_rate;
                    command.Parameters.Add("P_CURRENCY_ID", OracleDbType.Int32).Value = currency_id;
                    command.Parameters.Add("P_AMOUNT", OracleDbType.Double).Value = amount;
                    command.Parameters.Add("P_AMOUNT_AZN", OracleDbType.Double).Value = amount_azn;
                    command.Parameters.Add("P_BASIC_AMOUNT", OracleDbType.Double).Value = basic_amount;
                    command.Parameters.Add("P_INTEREST_AMOUNT", OracleDbType.Double).Value = interest_amount;
                    command.Parameters.Add("P_CONTRACT_ID", OracleDbType.Int32).Value = int.Parse(contractid);
                    command.Parameters.Add("P_PAYMENT_ID", OracleDbType.Int32).Value = int.Parse(paymentid);
                    command.Parameters.Add("P_OWNER_ACCOUNT", OracleDbType.Varchar2).Value = owner_account;
                    command.Parameters.Add("P_IS_TEMP", OracleDbType.Int32).Value = istemp;
                    command.Parameters.Add("P_USER_ID", OracleDbType.Int32).Value = GlobalVariables.V_UserID;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    LogWrite("Əməliyatlar jurnala daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
                    transaction.Rollback();
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public static void FindAndReplace(Microsoft.Office.Interop.Word.Application doc, object findText, object replaceWithText)
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            //execute find and replace
            doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }

        public static void GridViewPrintInitializeByLandscape(DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e, float? top = null, float? bottom = null, float? right = null, float? left = null)
        {
            DevExpress.XtraPrinting.PrintingSystemBase pb = e.PrintingSystem as DevExpress.XtraPrinting.PrintingSystemBase;
            pb.PageSettings.Landscape = true;
            pb.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;

            if (top != null)
                (e.PrintingSystem as DevExpress.XtraPrinting.PrintingSystemBase).PageSettings.TopMarginF = (float)top;
            if (bottom != null)
                (e.PrintingSystem as DevExpress.XtraPrinting.PrintingSystemBase).PageSettings.BottomMarginF = (float)bottom;
            if (right != null)
                (e.PrintingSystem as DevExpress.XtraPrinting.PrintingSystemBase).PageSettings.RightMarginF = (float)right;
            if (left != null)
                (e.PrintingSystem as DevExpress.XtraPrinting.PrintingSystemBase).PageSettings.LeftMarginF = (float)left;
        }

        public static void ShowWordFileFromDB(string sql, string filePath, string blobColumn)
        {
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, "ShowWordFileFromDB");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr[blobColumn]))
                {
                    Byte[] BLOBData = (byte[])dr[blobColumn];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    DeleteFile(filePath);
                    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                    Process.Start(filePath);
                }
            }
        }

        public static void ShowWordFileWithExtensionFromDB(string sql, string filePath, string blobColumn, string extensionColumn)
        {
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, "ShowWordFileFromDB");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                filePath = filePath + dr[extensionColumn];

                if (File.Exists(filePath))
                {
                    Process.Start(filePath);
                    break;
                }

                if (!DBNull.Value.Equals(dr[blobColumn]))
                {

                    Byte[] BLOBData = (byte[])dr[blobColumn];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    DeleteFile(filePath);
                    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                    Process.Start(filePath);
                }
            }
        }

        //public static void LoadColorXML()
        //{
        //    string filepath = GlobalVariables.V_ExecutingFolder + "\\XMLs\\Color.xml";
        //    try
        //    {
        //        GlobalVariables.colorxmldoc = new XmlDataDocument();
        //        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        //        GlobalVariables.colorxmldoc.Load(fs);

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("BlockColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_BlockColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_BlockColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_BlockColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_BlockColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_BlockColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_BlockColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("CloseColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_CloseColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_CloseColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_CloseColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_CloseColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_CloseColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_CloseColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("CommitColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_CommitColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_CommitColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_CommitColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_CommitColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_CommitColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_CommitColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ConnectColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ConnectColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("CalculatedColumnColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_CalculatedColumnColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("IncomeColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_IncomeColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ExpensesColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ExpensesColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ContractNotExpensesColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ContractNotExpensesColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ContractLawCloseColor");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ContractLawCloseColor_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("BlockColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_BlockColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_BlockColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_BlockColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_BlockColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_BlockColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_BlockColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("CloseColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_CloseColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_CloseColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_CloseColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_CloseColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_CloseColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_CloseColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("CommitColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_CommitColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_CommitColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_CommitColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_CommitColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_CommitColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_CommitColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ConnectColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ConnectColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ConnectColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("CalculatedColumnColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_CalculatedColumnColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_CalculatedColumnColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("IncomeColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_IncomeColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_IncomeColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ExpensesColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ExpensesColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ExpensesColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ContractNotExpensesColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ContractNotExpensesColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ContractNotExpensesColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }

        //        GlobalVariables.xmlnode = GlobalVariables.colorxmldoc.GetElementsByTagName("ContractLawCloseColor2");
        //        if (GlobalVariables.xmlnode.Count > 0)
        //        {
        //            GlobalVariables.V_ContractLawCloseColor2_A = GlobalVariables.xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor2_B = GlobalVariables.xmlnode[0].ChildNodes.Item(1).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor2_G = GlobalVariables.xmlnode[0].ChildNodes.Item(2).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor2_R = GlobalVariables.xmlnode[0].ChildNodes.Item(3).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor2_Type = GlobalVariables.xmlnode[0].ChildNodes.Item(4).InnerText.Trim();
        //            GlobalVariables.V_ContractLawCloseColor2_Name = GlobalVariables.xmlnode[0].ChildNodes.Item(5).InnerText.Trim();
        //        }
        //        fs.Close();
        //    }
        //    catch (Exception exx)
        //    {
        //        LogWrite(filepath + " faylı açılmadı.", null, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
        //    }
        //}

        public static void TextEditCharCount(TextEdit txt, LabelControl lbl)
        {
            if (txt.Text.Trim().Length == 0)
                lbl.Visible = false;
            else
            {
                lbl.Visible = true;
                lbl.Text = txt.Text.Trim().Length.ToString();
            }
        }

        public static void LoadLrsColor()
        {
            List<LrsColors> lstColors = LrsColorsDAL.SelectColor(GlobalVariables.V_UserID).ToList<LrsColors>();
            if (lstColors.Count == 0)
                return;

            GlobalVariables.V_BlockColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 1).COLOR_VALUE_1;
            GlobalVariables.V_BlockColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 1).COLOR_VALUE_2;

            GlobalVariables.V_CloseColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 2).COLOR_VALUE_1;
            GlobalVariables.V_CloseColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 2).COLOR_VALUE_2;

            GlobalVariables.V_CommitColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 3).COLOR_VALUE_1;
            GlobalVariables.V_CommitColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 3).COLOR_VALUE_2;

            GlobalVariables.V_ConnectColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 4).COLOR_VALUE_1;
            GlobalVariables.V_ConnectColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 4).COLOR_VALUE_2;

            GlobalVariables.V_CalculatedColumnColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 5).COLOR_VALUE_1;
            GlobalVariables.V_CalculatedColumnColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 5).COLOR_VALUE_2;

            GlobalVariables.V_IncomeColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 6).COLOR_VALUE_1;
            GlobalVariables.V_IncomeColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 6).COLOR_VALUE_2;

            GlobalVariables.V_ExpensesColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 7).COLOR_VALUE_1;
            GlobalVariables.V_ExpensesColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 7).COLOR_VALUE_2;

            GlobalVariables.V_ContractNotExpensesColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 8).COLOR_VALUE_1;
            GlobalVariables.V_ContractNotExpensesColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 8).COLOR_VALUE_2;

            GlobalVariables.V_ContractLawCloseColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 9).COLOR_VALUE_1;
            GlobalVariables.V_ContractLawCloseColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 9).COLOR_VALUE_2;

            GlobalVariables.V_ExtendColor1 = lstColors.Find(item => item.COLOR_TYPE_ID == 10).COLOR_VALUE_1;
            GlobalVariables.V_ExtendColor2 = lstColors.Find(item => item.COLOR_TYPE_ID == 10).COLOR_VALUE_2;
        }

        public static void ChangeCheckStyle(CheckEdit ce)
        {
            if (ce.Checked)
                ce.Font = new Font(ce.Font.FontFamily, ce.Font.Size, FontStyle.Bold);
            else
                ce.Font = new Font(ce.Font.FontFamily, ce.Font.Size, FontStyle.Regular);
        }

        public static void SplashScreenShow(Form form, Type t)
        {
            SplashScreenClose();
            SplashScreenManager.ShowForm(form, t, true, true, ParentFormState.Locked);
        }

        public static void SplashScreenClose()
        {
            if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                SplashScreenManager.CloseForm();
        }

        public static void DeleteLookUpEditSelectedValue(LookUpEdit lookup)
        {
            if (lookup != null)
                lookup.EditValue = null;
        }

        public static void LookUpEditValue(LookUpEdit lookup, string value)
        {
            lookup.EditValue = lookup.Properties.GetKeyValueByDisplayText(value);
        }

        public static void GenerateAutoRowNumber(object sender, DevExpress.XtraGrid.Columns.GridColumn column, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column == column && e.IsGetData)
                e.Value = view.GetRowHandle(e.ListSourceRowIndex) + 1;
        }

        public static void ShowPivotGridPreview(PivotGridControl pivotGrid)
        {
            if (!pivotGrid.IsPrintingAvailable)
            {
                XtraMessageBox.Show("'DevExpress.XtraPrinting' kitabxanası tapılmadı", "Xəta");
                return;
            }
            pivotGrid.ShowPrintPreview();
        }

        public static void PivotGridExportToFile(PivotGridControl gridControl, string fileExtenstion)
        {
            if (gridControl == null)
                return;

            string filter = null;
            switch (fileExtenstion)
            {
                case "xls":
                    filter = "Excel (2003)(.xls)|*.xls";
                    break;
                case "xlsx":
                    filter = "Excel (2010) (.xlsx)|*.xlsx";
                    break;
                case "rtf":
                    filter = "RichText faylı (.rtf)|*.rtf";
                    break;
                case "pdf":
                    filter = "Pdf faylı (.pdf)|*.pdf";
                    break;
                case "html":
                    filter = "Html faylı (.html)|*.html";
                    break;
                case "mht":
                    filter = "Mht faylı (.mht)|*.mht";
                    break;
                case "txt":
                    filter = "Text faylı (.txt)|*.txt";
                    break;
                case "csv":
                    filter = "Csv faylı (.csv)|*.csv";
                    break;
                default: break;
            }

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = filter;
                    if (saveDialog.ShowDialog() != DialogResult.Cancel)
                    {
                        string exportFilePath = saveDialog.FileName;
                        switch (fileExtenstion)
                        {
                            case "xls":
                                gridControl.ExportToXls(exportFilePath);
                                break;
                            case "xlsx":
                                {
                                    var pivotExportOptions = new PivotXlsxExportOptions();
                                    pivotExportOptions.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                                    gridControl.ExportToXlsx(exportFilePath, pivotExportOptions);
                                }
                                break;
                            case "rtf":
                                gridControl.ExportToRtf(exportFilePath);
                                break;
                            case "pdf":
                                gridControl.ExportToPdf(exportFilePath);
                                break;
                            case "html":
                                gridControl.ExportToHtml(exportFilePath);
                                break;
                            case "mht":
                                gridControl.ExportToMht(exportFilePath);
                                break;
                            case "txt":
                                gridControl.ExportToText(exportFilePath);
                                break;
                            case "csv":
                                gridControl.ExportToCsv(exportFilePath);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                LogWrite(fileExtenstion + " faylı yaradılmadı.", gridControl.Name, GlobalVariables.V_UserName, "GlobalProcedures", MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        public static void GridSaveLayout(GridView view, string ModulName)
        {
            string path = GlobalVariables.V_ExecutingFolder + "\\Layouts\\" + ModulName;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = path + "\\" + view.Name + ".xml";
            view.SaveLayoutToXml(fileName);
        }

        public static void GridRestoreLayout(GridView view, string ModulName)
        {
            string path = GlobalVariables.V_ExecutingFolder + "\\Layouts\\" + ModulName;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = path + "\\" + view.Name + ".xml";
            if (File.Exists(fileName))
                view.RestoreLayoutFromXml(fileName);
        }
    }
}
