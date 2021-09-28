using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using Bytescout.Document;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using System.Text.RegularExpressions;
using System.Threading;

namespace CRS.Forms.Contracts
{
    public partial class FPowerOfAttorney : DevExpress.XtraEditors.XtraForm
    {
        public FPowerOfAttorney()
        {
            InitializeComponent();
        }
        public string TransactionName,
            PowerID,
            CustomerName,
            Object,
            CarNumber,
            CustomerID,
            ContractID,
            CardSeries,
            CardNumber,
            CardIssuing,
            CardIssuingDate,
            CardReliableDate,
            CarOwnerName,
            BrandAndModel,
            BrandModelAndTip;
        public int RowCount = 0;

        string card_name, CodeChar = "E";
        int code_number = 0;
        bool changecode = false;
        DateTime insert_date = DateTime.Today;

        public delegate void DoEvent();
        public event DoEvent RefreshPowerDataGridView;

        private void FPowerOfAttorney_Load(object sender, EventArgs e)
        {
            IssueDate.Properties.MaxValue = DateTime.Today;
            ReliableDate.Properties.MinValue = DateTime.Today;

            GlobalProcedures.FillLookUpEdit(CardSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
            GlobalProcedures.FillLookUpEdit(CardIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");

            if (TransactionName == "INSERT")
            {
                DateValue.Properties.MinValue = DateTime.Today;
                if (PowerID == null)
                {
                    DateValue.EditValue = DateTime.Today;
                    NameText.Text = CustomerName;
                    CardSeriesLookUp.EditValue = CardSeriesLookUp.Properties.GetKeyValueByDisplayText(CardSeries);
                    NumberText.Text = CardNumber;
                    IssueDate.EditValue = GlobalFunctions.ChangeStringToDate(CardIssuingDate, "ddmmyyyy");
                    ReliableDate.EditValue = GlobalFunctions.ChangeStringToDate(CardReliableDate, "ddmmyyyy");
                    CardIssuingLookUp.EditValue = CardIssuingLookUp.Properties.GetKeyValueByDisplayText(CardIssuing);
                    ResponsibleCheck.Checked = RowCount == 0;
                    ResponsibleCheck.ReadOnly = RowCount == 0;
                }
                else
                    LoadPowerDetails();
                PowerID = GlobalFunctions.GetOracleSequenceValue("POWER_SEQUENCE").ToString();
                CodeCheck_CheckedChanged(sender, EventArgs.Empty);
            }
            else
            {
                CodeCheck.Enabled = BChangeCode.Enabled = false;
                LoadPowerDetails();
            }
        }

        private void LoadPowerDetails()
        {
            string s = $@"SELECT PA.POWER_CODE,
                                   PA.FULLNAME,
                                   PA.FULLNAME_CHECK,
                                   PA.POWER_DATE,
                                   CS.SERIES CARD_SERIES,
                                   PA.CARD_NUMBER,
                                   PA.CARD_ISSUE_DATE,
                                   PA.CARD_RELIABLE_DATE,
                                   CI.NAME ISSUING_NAME,
                                   CI.ID CARD_ISSUING_ID,
                                   PA.IS_RESPONSIBLE
                              FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP PA,
                                   CRS_USER.CARD_SERIES CS,
                                   CRS_USER.CARD_ISSUING CI
                             WHERE     PA.CARD_ISSUING_ID = CI.ID
                                   AND PA.CARD_SERIES_ID = CS.ID
                                   AND PA.ID = {PowerID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPowerDetails", "Etibarnamənin parametrləri tapılmadı.");

            foreach (DataRow dr in dt.Rows)
            {
                if (TransactionName == "EDIT")
                {
                    CodeCheck.Checked = !(String.IsNullOrWhiteSpace(dr["POWER_CODE"].ToString()));
                    CodeText.Text = dr["POWER_CODE"].ToString();
                }
                NameText.Text = dr["FULLNAME"].ToString();
                SAACheck.Checked = (Convert.ToInt32(dr["FULLNAME_CHECK"].ToString()) == 1);
                DateValue.EditValue = (TransactionName == "INSERT") ? DateTime.Today : DateTime.Parse(dr["POWER_DATE"].ToString());
                CardSeriesLookUp.EditValue = CardSeriesLookUp.Properties.GetKeyValueByDisplayText(dr["CARD_SERIES"].ToString());
                NumberText.Text = dr["CARD_NUMBER"].ToString();
                IssueDate.EditValue = DateTime.Parse(dr["CARD_ISSUE_DATE"].ToString());
                ReliableDate.EditValue = DateTime.Parse(dr["CARD_RELIABLE_DATE"].ToString());
                CardIssuingLookUp.EditValue = CardIssuingLookUp.Properties.GetKeyValueByDisplayText(dr["ISSUING_NAME"].ToString());
                ResponsibleCheck.Checked = Convert.ToInt32(dr["IS_RESPONSIBLE"].ToString()) == 1;
            }
        }

        private string InsertCodeTemp()
        {
            if (PowerID == null)
                return null;

            int max_code_number = 0, power_number_count = 0;
            string code = "00000";

            List<PowerOfAttorneyCodeTemp> lstCode = PowerOfAttorneyCodeTempDAL.SelectPowerOfAttorneyCode().ToList<PowerOfAttorneyCodeTemp>();

            power_number_count = lstCode.Count(c => c.POWER_ID == int.Parse(PowerID));

            if (lstCode.Count == 0)
                max_code_number = GlobalFunctions.GetMax($@"SELECT NVL(MAX(POWER_NUMBER),0) FROM CRS_USER.POWER_OF_ATTORNEY");
            else
            {
                if (power_number_count == 0)
                    max_code_number = lstCode.Max(t => t.CODE_NUMBER);
                else
                    return lstCode.Find(c => c.POWER_ID == int.Parse(PowerID)).CODE;
            }

            code_number = max_code_number + 1;
            code = CodeChar + code_number.ToString().PadLeft(4, '0') + "/" + DateTime.Today.Year.ToString().Substring(2, 2);
            if (GlobalFunctions.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.POWER_OF_ATTORNEY_CODE_TEMP(USED_USER_ID,CONTRACT_ID,POWER_ID,CODE_NUMBER,CODE)VALUES({GlobalVariables.V_UserID},{ContractID},{PowerID},{code_number},'{code}')",
                                                            "Etibarnamənin maksimum nömrəsi temp cədvələ daxil edilmədi.") > 0)
                return code;
            else
                return null;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_CODE_TEMP WHERE CODE = '{CodeText.Text.Trim()}' AND USED_USER_ID = {GlobalVariables.V_UserID}", "Müqavilənin kodu temp cədvəldən silinmədi.");
            this.Close();
        }

        private void SAACheck_CheckedChanged(object sender, EventArgs e)
        {
            NameText.Properties.ReadOnly = !SAACheck.Checked;
            GlobalProcedures.ChangeCheckStyle(SAACheck);
            if (SAACheck.Checked)
                NameText.Focus();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPowerDetail())
            {
                if (GenerateFile())
                {
                    if (TransactionName == "INSERT")
                        InsertPower();
                    else
                        UpdatePower();
                    OpenFile();
                    this.Close();
                }
            }
        }

        void OpenFile()
        {
            string filePath = null;
            try
            {
                if (FileCheck.Checked)
                {
                    filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + code_number + ".docx";
                    Process.Start(filePath);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.ShowErrorMessage(exx.Message);
            }

            try
            {
                if (ApplicantionCheck.Checked)
                {
                    filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\EtibarnameErizesi_" + code_number + ".docx";
                    Process.Start(filePath);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.ShowErrorMessage(exx.Message);
            }

            try
            {
                if (ResponsibleFileCheck.Checked)
                {
                    filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\EsasEtibarnameErizesi_" + code_number + ".docx";
                    Process.Start(filePath);
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.ShowErrorMessage(exx.Message);
            }
        }

        void RefreshCode(string code, bool close)
        {
            changecode = close;
            if (close)
            {
                CodeText.Text = code;
                code_number = int.Parse(code.Substring(1, 4));
            }
        }

        private void FileCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle(FileCheck);
        }

        private void ApplicantionCheck_CheckedChanged(object sender, EventArgs e)
        {
            GlobalProcedures.ChangeCheckStyle(ApplicantionCheck);
        }

        private void ResponsibleCheck_CheckedChanged(object sender, EventArgs e)
        {
            ResponsibleFileCheck.Visible = ResponsibleCheck.Checked;
        }

        private void CodeCheck_CheckedChanged(object sender, EventArgs e)
        {
            BChangeCode.Visible = (CodeCheck.Checked);
            if (CodeCheck.Checked)
                CodeText.Text = InsertCodeTemp();
            GlobalProcedures.ChangeCheckStyle(CodeCheck);
        }

        private void DateValue_EditValueChanged(object sender, EventArgs e)
        {
            int day_count = GlobalFunctions.DifferenceTwoDateWithDay(DateTime.Today, DateValue.DateTime);
            if (day_count > 0)
                DayCountLabel.Text = $@"Bu gündən sonra {day_count} gün qüvvədədir.";
            else if (day_count == 0)
                DayCountLabel.Text = $@"Yalnız bu gün qüvvədədir.";
            else
                DayCountLabel.Text = $@"Bu etibarnamə qüvvədən düşüb.";
        }

        private void BChangeCode_Click(object sender, EventArgs e)
        {
            Customer.FChangeCode fcc = new Customer.FChangeCode();
            fcc.type = 3;
            fcc.RefreshCode += new Customer.FChangeCode.DoEvent(RefreshCode);
            fcc.ShowDialog();
        }

        private void CardSeriesLookUp_EditValueChanged(object sender, EventArgs e)
        {
            card_name = GlobalFunctions.GetName($@"SELECT NAME FROM CRS_USER.CARD_SERIES WHERE ID = {CardSeriesLookUp.EditValue}");
        }

        private void CardSeriesLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 2);
        }

        private void CardIssuingLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 3);
        }

        private void FPowerOfAttorney_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshPowerDataGridView();
        }

        private bool ControlPowerDetail()
        {
            bool b = false;

            if (CodeCheck.Checked && CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Nömrə daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT POWER_CODE FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP WHERE IS_CHANGE <> 2 UNION ALL SELECT POWER_CODE FROM CRS_USER.POWER_OF_ATTORNEY) WHERE POWER_CODE = '{CodeText.Text}'") > 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bu nömrə artıq daxil edilib. Zəhmət olmasa digər nömrə daxil edin.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sürücün adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(DateValue.Text))
            {
                DateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qüvvədə olma tarixi daxil edilməyib.");
                DateValue.Focus();
                DateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;


            if (String.IsNullOrEmpty(CardSeriesLookUp.Text))
            {
                CardSeriesLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təyin edən sənədin seriyası daxil edilməyib.");
                CardSeriesLookUp.Focus();
                CardSeriesLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NumberText.Text.Length == 0)
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi daxil edilməyib.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((Convert.ToInt32(CardSeriesLookUp.EditValue) == 2) && (NumberText.Text.Length != 8))
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi 8 simvol olmalıdır.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(IssueDate.Text))
            {
                IssueDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi daxil edilməyib.");
                IssueDate.Focus();
                IssueDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (GlobalFunctions.ChangeStringToDate(IssueDate.Text, "ddmmyyyy") == GlobalFunctions.ChangeStringToDate(ReliableDate.Text, "ddmmyyyy"))
            {
                IssueDate.BackColor = ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi ilə etibarlı olma tarixi eyni ola bilməz.");
                IssueDate.Focus();
                IssueDate.BackColor = ReliableDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (GlobalFunctions.ChangeStringToDate(ReliableDate.Text, "ddmmyyyy") < DateTime.Today)
            {
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin vaxtı bitib.");
                ReliableDate.Focus();
                ReliableDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(CardIssuingLookUp.Text))
            {
                CardIssuingLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədi verən orqanın adı daxil edilməyib.");
                CardIssuingLookUp.Focus();
                CardIssuingLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private bool GenerateFile()
        {
            bool result = false;
            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Etibarnamə.docx"))
            {
                GlobalVariables.WordDocumentUsed = false;
                GlobalProcedures.ShowErrorMessage(GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında etibarnamənin şablon faylı tapılmadı.");
                return false;
            }

            GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FPrintDocumentWait));
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Microsoft.Office.Interop.Word.Document aDoc = null;
            string curentdate = GlobalFunctions.DateWithDayMonthYear(DateTime.Today);

            code_number = (CodeCheck.Checked) ? code_number : 0;

            object missing = System.Reflection.Missing.Value;
            string filePath = null;
            object readOnly = false;
            object isVisible = false;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            #region Etibarname
            try
            {
                filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + code_number + ".docx";

                wordApp = new Microsoft.Office.Interop.Word.Application();
                aDoc = null;

                wordApp.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;

                object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Etibarnamə.docx"),
                   saveAs = Path.Combine(filePath);

                aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                aDoc.Activate();
                GlobalProcedures.FindAndReplace(wordApp, "[$code]", (CodeCheck.Checked) ? CodeText.Text.Trim() : "________");
                GlobalProcedures.FindAndReplace(wordApp, "[$currentdate]", curentdate);
                GlobalProcedures.FindAndReplace(wordApp, "[$date]", DateValue.Text);
                GlobalProcedures.FindAndReplace(wordApp, "[$object]", Object);
                GlobalProcedures.FindAndReplace(wordApp, "[$carnumber]", CarNumber + " ");
                GlobalProcedures.FindAndReplace(wordApp, "[$customername]", NameText.Text.Trim() + ((NameText.Text.Trim().IndexOf("oğlu") > 0 || NameText.Text.Trim().IndexOf("qızı") > 0) ? "na (" : " (") + CardSeriesLookUp.Text.Trim() + ": " + NumberText.Text.Trim() + ", " + CardIssuingLookUp.Text.Trim() + " tərəfindən " + IssueDate.Text + " tarixində verilmişdir) ");

                if (File.Exists(filePath))
                    File.Delete(filePath);

                aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                aDoc.Close(ref missing, ref missing, ref missing);

                result = true;
            }
            catch (Exception exx)
            {
                GlobalProcedures.SplashScreenClose();
                XtraMessageBox.Show("Etibarname_" + code_number + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.\r\n" + exx.Message);
                return false;
            }
            finally
            {
                wordApp.Quit();
            }
            #endregion

            #region Etibarnamə üçün razılıq ərizəsi
            if (ApplicantionCheck.Checked)
            {
                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Etibarnamə üçün razılıq ərizəsi.docx"))
                {
                    try
                    {
                        filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\EtibarnameErizesi_" + code_number + ".docx";

                        wordApp = new Microsoft.Office.Interop.Word.Application();
                        aDoc = null;
                       
                        wordApp.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                        object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Etibarnamə üçün razılıq ərizəsi.docx"),
                           saveAs = Path.Combine(filePath);                        

                        aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                        aDoc.Activate();
                        GlobalProcedures.FindAndReplace(wordApp, "[$customername2]", NameText.Text.Trim());
                        GlobalProcedures.FindAndReplace(wordApp, "[$ownername]", CarOwnerName);
                        GlobalProcedures.FindAndReplace(wordApp, "[$object2]", BrandAndModel);

                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                        aDoc.Close(ref missing, ref missing, ref missing);

                        result = true;
                    }
                    catch (Exception exx)
                    {
                        GlobalProcedures.SplashScreenClose();
                        XtraMessageBox.Show("EtibarnameErizesi_" + code_number + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.\r\n" + exx.Message);
                        return false;
                    }
                    finally
                    {
                        wordApp.Quit();
                    }
                }
                else
                {
                    GlobalProcedures.SplashScreenClose();
                    GlobalProcedures.ShowErrorMessage(GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında etibarnamə üçün razılıq ərizəsinin şablon faylı tapılmadı.");
                }
            }
            #endregion

            #region Əsas etibarnamənin razılıq ərizəsi
            if (ResponsibleFileCheck.Checked)
            {
                if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Əsas etibarnamənin razılıq ərizəsi.docx"))
                {
                    try
                    {
                        if (CarOwnerName.IndexOf("oğlu") > 0)
                            CarOwnerName = CarOwnerName + "nun";
                        else if (CarOwnerName.IndexOf("qızı") > 0)
                            CarOwnerName = CarOwnerName + "nın";

                        filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\EsasEtibarnameErizesi_" + code_number + ".docx";

                        wordApp = new Microsoft.Office.Interop.Word.Application();
                        aDoc = null;
                        
                        wordApp.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                        object fileName = Path.Combine(GlobalVariables.V_ExecutingFolder + "\\Documents\\Əsas etibarnamənin razılıq ərizəsi.docx"),
                           saveAs = Path.Combine(filePath);

                        aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

                        aDoc.Activate();
                        GlobalProcedures.FindAndReplace(wordApp, "[$fullname]", NameText.Text.Trim());
                        GlobalProcedures.FindAndReplace(wordApp, "[$ownername]", CarOwnerName);
                        GlobalProcedures.FindAndReplace(wordApp, "[$car]", BrandAndModel + ", " + CarNumber);

                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                        aDoc.Close(ref missing, ref missing, ref missing);

                        result = true;
                    }
                    catch (Exception exx)
                    {
                        GlobalProcedures.SplashScreenClose();
                        XtraMessageBox.Show("EsasEtibarnameErizesi_" + code_number + ".docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz. Yenidən yaratmaq üçün zəhmət olmasa bu faylı bağlayın.\r\n" + exx.Message);
                        return false;
                    }
                    finally
                    {
                        wordApp.Quit();
                    }
                }
                else
                {
                    GlobalProcedures.SplashScreenClose();
                    GlobalProcedures.ShowErrorMessage(GlobalVariables.V_ExecutingFolder + "\\Documents ünvanında əsas etibarnamənin razılıq ərizəsinin şablon faylı tapılmadı.");
                }
            }
            #endregion

            GlobalProcedures.SplashScreenClose();
            return result;
        }

        private bool InsertPower()
        {
            if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + code_number + ".docx"))
            {
                bool b = false;
                int a = (SAACheck.Checked) ? 1 : 0;
                string code = (CodeCheck.Checked) ? CodeText.Text.Trim() : null;
                string sql = null;
                if (String.IsNullOrEmpty(code))
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.POWER_OF_ATTORNEY_CODE_TEMP WHERE CODE = '{CodeText.Text.Trim()}' AND USED_USER_ID = {GlobalVariables.V_UserID}", "Müqavilənin kodu temp cədvəldən silinmədi.");

                sql = $@"INSERT INTO CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP(ID,
                                                                                CONTRACT_ID,
                                                                                FULLNAME,
                                                                                FULLNAME_CHECK,
                                                                                POWER_DATE,
                                                                                CARD_SERIES_ID,
                                                                                CARD_NUMBER,
                                                                                CARD_ISSUING_ID,
                                                                                CARD_ISSUE_DATE,
                                                                                CARD_RELIABLE_DATE,
                                                                                POWER_FILE,
                                                                                USED_USER_ID,
                                                                                IS_CHANGE,
                                                                                POWER_CODE,
                                                                                POWER_NUMBER,
                                                                                IS_RESPONSIBLE,
                                                                                INSERT_USER)
                            VALUES({PowerID},
                                   {ContractID},
                                    '{NameText.Text.Trim()}',
                                    {a},
                                    TO_DATE('{DateValue.Text}','DD/MM/YYYY'),
                                    {CardSeriesLookUp.EditValue},
                                    '{NumberText.Text.Trim()}',
                                    {CardIssuingLookUp.EditValue},
                                    TO_DATE('{IssueDate.Text}','DD/MM/YYYY'),
                                    TO_DATE('{ReliableDate.Text}','DD/MM/YYYY'),
                                    :BlobFile,
                                    {GlobalVariables.V_UserID},
                                    1,
                                    '{code}',
                                    {code_number},
                                    {(ResponsibleCheck.Checked ? 1 : 0)},
                                    {GlobalVariables.V_UserID})";

                b = GlobalFunctions.ExecuteQueryWithBlob(sql,
                                                            GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + code_number + ".docx",
                                                                "Etibarnamənin məlumatları temp cədvələ daxil edilmədi.");

                if (ResponsibleCheck.Checked)
                    GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_RESPON_POWERATTORNEY_TEMP", "P_ID", PowerID, "P_CONTRACT_ID", ContractID, "Köhnə etibarnamələrin əsas etibarnamə olması dəyişdirilmədi.");

                return b;
            }
            else
                return false;
        }

        private bool UpdatePower()
        {
            if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + code_number + ".docx"))
            {
                int a = (SAACheck.Checked) ? 1 : 0;
                string code = (CodeCheck.Checked) ? CodeText.Text.Trim() : null, sql = null;

                sql = $@"UPDATE CRS_USER_TEMP.POWER_OF_ATTORNEY_TEMP SET FULLNAME = '{NameText.Text.Trim()}',
                                                                            FULLNAME_CHECK = {a},
                                                                            POWER_DATE= TO_DATE('{DateValue.Text}','DD/MM/YYYY'),
                                                                            CARD_SERIES_ID = {CardSeriesLookUp.EditValue},
                                                                            CARD_NUMBER = '{NumberText.Text.Trim()}',
                                                                            CARD_ISSUING_ID = {CardIssuingLookUp.EditValue},
                                                                            CARD_ISSUE_DATE = TO_DATE('{IssueDate.Text}','DD/MM/YYYY'),
                                                                            CARD_RELIABLE_DATE = TO_DATE('{ReliableDate.Text}','DD/MM/YYYY'),
                                                                            POWER_FILE = :BlobFile,
                                                                            IS_CHANGE = 1,  
                                                                            IS_RESPONSIBLE = {(ResponsibleCheck.Checked ? 1 : 0)},
                                                                            UPDATE_USER = {GlobalVariables.V_UserID},
                                                                            UPDATE_DATE = SYSDATE 
                                            WHERE ID = {PowerID} AND USED_USER_ID = {GlobalVariables.V_UserID}";

                if (ResponsibleCheck.Checked)
                    GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER_TEMP.PROC_RESPON_POWERATTORNEY_TEMP", "P_ID", PowerID, "P_CONTRACT_ID", ContractID, "Köhnə etibarnamələrin əsas etibarnamə olması dəyişdirilmədi.");

                return GlobalFunctions.ExecuteQueryWithBlob(sql,
                                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\Etibarname_" + code_number + ".docx",
                                                                    "Etibarnamənin məlumatları temp cədvəldə dəyişdirilmədi.");
            }
            else
                return false;
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 2:
                    GlobalProcedures.FillLookUpEdit(CardSeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 3:
                    GlobalProcedures.FillLookUpEdit(CardIssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void NumberText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(NumberText, NumberLengthLabel);
        }
    }
}