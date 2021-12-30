using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using System.IO;
using DevExpress.XtraEditors;
using System.Reflection;
using System.Diagnostics;
using CRS.Class;
using System.Threading;
using DevExpress.XtraEditors.Controls;
using System.Net;

namespace CRS.Forms
{
    public partial class LRSSplashScreen : SplashScreen
    {
        public LRSSplashScreen()
        {
            Localizer.Active = new ComponentLocalization.StringLocalizer();
            InitializeComponent();
            XtraMessageBox.AllowHtmlText = true;
        }

        bool is_connected = false, start_service = false;
        string license = null, LicenseID, cpu;
        DateTime enddate = DateTime.Today;

        #region Overrides

        public override void ProcessCommand(System.Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void UpdateLRS()
        {
            try
            {
                string networkPath = GlobalFunctions.ReadSetting("UpdatePath"),
                   currentVersion = GlobalFunctions.ReadSetting("CurrentVersion");

                int isServer = int.Parse(GlobalFunctions.ReadSetting("IsServer"));

                if (isServer == 0)
                {
                    NetworkCredential credentials = new NetworkCredential(@GlobalFunctions.ReadSetting("ServerUserName"), GlobalFunctions.ReadSetting("ServerPassword"));

                    using (new ConnectToSharedFolder(networkPath, credentials))
                    {
                        ControlVersion(networkPath, currentVersion);
                    }
                }
                else
                    ControlVersion(networkPath, currentVersion);
            }
            catch (Exception exx)
            {

            }
        }

        private void ControlVersion(string networkPath, string currentVersion)
        {
            if (Directory.Exists(networkPath))
            {
                DirectoryInfo dir = new DirectoryInfo(networkPath);
                FileInfo[] files = dir.GetFiles();

                if (files.Length == 0)
                    return;

                foreach (FileInfo file in files)
                {
                    if (file.Name != "VERSION.txt")
                        continue;

                    string lastVersion = lastVersion = File.ReadAllText(file.FullName).Trim();
                    if (lastVersion != string.Empty && currentVersion != lastVersion)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show($@"LRS sisteminin yeni {lastVersion} versiyası mövcuddur. Sistemi yeniləmək istəyirsiniz?", "Yeni versiya", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Application.Exit();
                            Process.Start(GlobalVariables.V_ExecutingFolder + "\\UpdateLRS.exe");
                            break;
                        }
                    }
                }
            }
        }

        void RefreshLicense(string a, string b, string c, DateTime d)
        {
            license = a;
            LicenseID = b;
            cpu = c;
            enddate = d;
        }

        private void LRSSplashScreen_Load(object sender, EventArgs e)
        {
            VersionLabel.Text = "v" + GlobalVariables.V_Version;
            GlobalVariables.V_ExecutingFolder = new FileInfo((Assembly.GetExecutingAssembly().Location)).Directory.FullName;
            GlobalProcedures.SetSetting("CurrentVersion", GlobalVariables.V_Version);
            UpdateLRS();

            if (LRSBackgroundWorker.IsBusy != true)
                LRSBackgroundWorker.RunWorkerAsync();
        }

        void RefreshService(bool k)
        {
            start_service = k;
        }

        private void LoadFServiceMessage()
        {
            FServiceStartMessage fs = new FServiceStartMessage();
            fs.ListenerServiceName = GlobalFunctions.ReadSetting("OracleListenerServiceName");
            fs.DataBaseServiceName = GlobalFunctions.ReadSetting("OracleServiceName");
            fs.RefreshService += new FServiceStartMessage.DoEvent(RefreshService);
            fs.ShowDialog();
        }

        private void ControlDirectories()
        {
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\Documents");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\RDLC"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\RDLC");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\Reports"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\Reports");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\WebCameraImages"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\WebCameraImages");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\IDCardImages"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\IDCardImages");
            if (!Directory.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images"))
                Directory.CreateDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images");
        }

        private void LRSBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "Versiya yoxlanılır";
            });

            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "Baza ilə əlaqə yoxlanılır";
            });

            string ListenerServiceName = GlobalFunctions.ReadSetting("OracleListenerServiceName"),
                   DataBaseServiceName = GlobalFunctions.ReadSetting("OracleServiceName");

            bool existsListenerService = !String.IsNullOrWhiteSpace(ListenerServiceName),
                 existsDataBaseService = !String.IsNullOrWhiteSpace(DataBaseServiceName);

            if (existsDataBaseService && existsListenerService)
            {
                string statusListenerService = GlobalFunctions.ControlServiceStatus(GlobalFunctions.ReadSetting("OracleListenerServiceName")),
                       statusDataBaseService = GlobalFunctions.ControlServiceStatus(GlobalFunctions.ReadSetting("OracleServiceName"));

                if (statusDataBaseService != "Running" || statusListenerService != "Running")
                {
                    DescriptionLabel.Invoke((MethodInvoker)delegate
                    {
                        DescriptionLabel.Text = "Servislər işə salınır";
                    });

                    bool listener = GlobalFunctions.StartService(ListenerServiceName),
                         database = GlobalFunctions.StartService(DataBaseServiceName);

                    start_service = (listener && database);

                    if (!start_service)
                    {
                        string message = null;
                        if (statusDataBaseService != "Running" && statusListenerService != "Running")
                            message = "Bazanın servisləri sönüb və avtomatik olaraq yadırıla bilmir. Zəhmət olmasa serverdə aşağıda adları göstərilən servisləri ardıcıl olaraq yandırın.\r\n\r\n<b>1. <color=red>" + ListenerServiceName + "</color>\r\n2. <color=red>" + DataBaseServiceName + "</color></b>";
                        else if (statusDataBaseService != "Running" && statusListenerService == "Running")
                            message = "Bazanın <color=red><b>" + DataBaseServiceName + "</color></b> servisi sönüb və avtomatik olaraq yandırıla bilmir. Zəhmət olmasa serverdə həmin servisi yandırın.";
                        else if (statusDataBaseService == "Running" && statusListenerService != "Running")
                            message = "Bazanın <color=red><b>" + ListenerServiceName + "</color></b> servisi sönüb və avtomatik olaraq yandırıla bilmir. Zəhmət olmasa serverdə həmin servisi yandırın.";

                        GlobalProcedures.ShowWarningMessage(message);
                        Process.Start("services.msc");
                        Application.Exit();
                    }
                }
            }

            if (!GlobalFunctions.ConnectDataBase())
                Application.Exit();

            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "Baza ilə əlaqə var";
            });

            ControlDirectories();

            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "Qovluqlar mövcuddur";
            });

            LicenseID = GlobalFunctions.Decrypt(GlobalFunctions.ReadSetting("LicenseID")).Trim();
            GlobalVariables.V_CurrentDate = DateTime.Today.ToString("d", GlobalVariables.V_CultureInfoAZ);
            GlobalVariables.V_StyleName = GlobalFunctions.ReadSetting("StyleName");

            bool blicense = false;
            string sql = null;

            sql = $@"SELECT LICENSE, 
                            CPU, 
                            END_DATE, 
                            DAY_COUNT 
                    FROM CRS_USER.CRS_LICENSE WHERE CPU = '{GlobalFunctions.Encrypt(LicenseID + GlobalFunctions.GetCPUID())}'";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql);

            if (dt == null)
                return;

            if (dt.Rows.Count == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Sizin lisenziyanız yoxdur. Yeni lisenziya almaq istəyirsiniz?.", "Lisenziya", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    FLicense fl = new FLicense();
                    fl.RefreshLicense += new FLicense.DoEvent(RefreshLicense);
                    fl.ShowDialog();
                }
                else
                {
                    Application.Exit();
                    return;
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                license = dr["LICENSE"].ToString();
                cpu = dr["CPU"].ToString();
                enddate = DateTime.Parse(dr["END_DATE"].ToString());
            }

            switch (license)
            {
                case "JNCQPcIIZypbYqwatzvXlwRgy5tpFW6y":
                    {
                        blicense = true;
                        GlobalVariables.V_License = 0;
                    }
                    break;
                case "Uvki+HBeAPgq2di6nfFJm8Uu3b3ldYbx":
                    {
                        blicense = true;
                        GlobalVariables.V_License = 1;
                    }
                    break;
            }

            if (!blicense)
            {
                XtraMessageBox.Show("Proqramın lisenziyası düzgün deyil. Zəhmət olmasa sistemin administratoru ilə əlaqə saxlayın.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            if (cpu != GlobalFunctions.Encrypt(LicenseID + GlobalFunctions.GetCPUID()))
            {
                XtraMessageBox.Show("Bu proqram qeydiyyatdan keçməyib. Zəhmət olmasa sistemin administratoru ilə əlaqə saxlayın.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            if (enddate == DateTime.Today)
                XtraMessageBox.Show("Proqramın lisenziyası bu gün bitir. Zəhmət olmasa sistemin administratoru ilə əlaqə saxlayın.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (enddate < DateTime.Today)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Proqramın lisenziyası bitib. Yeni lisenziya almaq istəyirsiniz?.", "Lisenziya", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    FLicense fl = new FLicense();
                    fl.RefreshLicense += new FLicense.DoEvent(RefreshLicense);
                    fl.ShowDialog();
                }
                else
                {
                    Application.Exit();
                    return;
                }
            }

            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "Məzənnələr yüklənir";
            });

            GlobalProcedures.GenerateRateText();

            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "Temp fayllar yüklənir";
            });

            DownloadTemplateFiles();

            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "RDLC fayllar yüklənir";
            });

            DownloadRDLCFiles();

            GlobalVariables.V_CommitmentCount = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.AGAIN_COMMITMENT");

            if (GlobalFunctions.ReadSetting("IsServer") == "1")
            {
                DescriptionLabel.Invoke((MethodInvoker)delegate
                {
                    DescriptionLabel.Text = "Ödənişlərin silinməsi gedir";
                });

                GlobalProcedures.ExecuteProcedure("CRS_USER.PROC_CLEARNING", "Ödənişlərin silinməsi olmadı.");
            }

            if (ControlFiles())
                is_connected = true;
            else
                Application.Exit();
        }

        private void DownloadTemplateFiles()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT NAME,TEMPLATE_FILE FROM CRS_USER.TEMPLATE_FILES WHERE TEMPLATE_FILE IS NOT NULL ORDER BY ID");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["TEMPLATE_FILE"]))
                {
                    string filePath = GlobalVariables.V_ExecutingFolder + "\\Documents\\" + dr["NAME"] + ".docx";

                    GlobalProcedures.DeleteFile(filePath);

                    Byte[] BLOBData = (byte[])dr["TEMPLATE_FILE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    GlobalProcedures.DeleteFile(filePath);
                    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                }
            }
        }

        private void DownloadRDLCFiles()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT FILENAME NAME,RDLC FROM CRS_USER.LRS_RDLC_FILES WHERE RDLC IS NOT NULL ORDER BY ID");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["RDLC"]))
                {
                    string filePath = GlobalVariables.V_ExecutingFolder + "\\RDLC\\" + dr["NAME"] + ".rdlc";

                    GlobalProcedures.DeleteFile(filePath);

                    Byte[] BLOBData = (byte[])dr["RDLC"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    GlobalProcedures.DeleteFile(filePath);
                    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                }
            }
        }

        private void LRSBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                XtraMessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                this.Close();
            }
        }

        private bool ControlFiles()
        {
            bool s = false;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\RDLC\\Commitment.rdlc"))
            {
                XtraMessageBox.Show("Commitment.rdlc faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\RDLC\\Expenditure.rdlc"))
            {
                XtraMessageBox.Show("Expenditure.rdlc faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\RDLC\\Insurance.rdlc"))
            {
                XtraMessageBox.Show("Insurance.rdlc faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Alqı-satqı.docx"))
            {
                XtraMessageBox.Show("Alqı-satqı.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Daşınmaz Əmlak.docx"))
            {
                XtraMessageBox.Show("Daşınmaz Əmlak.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Etibarnamə.docx"))
            {
                XtraMessageBox.Show("Etibarnamə.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mədaxil orderi.docx"))
            {
                XtraMessageBox.Show("Mədaxil orderi.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Müqavilə.docx"))
            {
                XtraMessageBox.Show("Müqavilə.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Daimi sənədlər.docx"))
            {
                XtraMessageBox.Show("Daimi sənədlər.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Ödəniş qrafiki.docx"))
            {
                XtraMessageBox.Show("Ödəniş qrafiki.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Öhdəlik ötürmə.docx"))
            {
                XtraMessageBox.Show("Öhdəlik ötürmə.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Polisə çıxarış.docx"))
            {
                XtraMessageBox.Show("Polisə çıxarış.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Qebz.docx"))
            {
                XtraMessageBox.Show("Qebz.docx faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Bytescout.Document.dll"))
            {
                XtraMessageBox.Show("Bytescout.Document.dll faylı tapılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                s = true;

            return s;
        }

        private void LRSSplashScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!is_connected)
                return;

            FConnect fc = new FConnect();
            fc.ShowDialog();
        }
    }
}