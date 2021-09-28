using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Localization;
using System.Globalization;
using System.IO;
using System.Xml;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Localization;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraTreeList.Localization;
using DevExpress.XtraBars.Alerter;
using System.Diagnostics;
using DevExpress.XtraScheduler.Localization;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;
using System.Configuration;
using System.Net;

namespace CRS.Forms
{
    public partial class FMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FMain()
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    {
                        Localizer.Active = new ComponentLocalization.StringLocalizer();
                        GridLocalizer.Active = new ComponentLocalization.CustomGridLocalizer();
                        BarLocalizer.Active = new ComponentLocalization.CustomBarLocalizer();
                        DockManagerResXLocalizer.Active = new ComponentLocalization.CustomDockManagerLocalizer();
                        TreeListLocalizer.Active = new ComponentLocalization.CustomTreeListLocalizer();
                        SchedulerLocalizer.Active = new ComponentLocalization.CustomerSchedulerLocalizer();
                        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("az-Latn-AZ");
                    }
                    break;
                case "EN":
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    break;
                case "RU":
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
                    break;
            }
            InitializeComponent();
        }

        DateTime startTime;

        private void FMain_Load(object sender, EventArgs e)
        {
            GenerateMenuPermisions();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalVariables.V_StyleName);
            startTime = DateTime.Now;
            LoadDefaultMenu();
            MainTimer.Start();
            VersionTimer.Start();
            CurrentDayBarStatic.Caption = "Bu gün : " + DateTime.Now.ToString("dddd,d MMMM yyyy");
            VersionBarStatic.Caption = GlobalFunctions.ReadSetting("Company")  + " - v" + GlobalVariables.V_Version;
            Bitmap bm = new Bitmap(Properties.Resources.yellow_leasing);
            CRSNotifyIcon.Icon = Icon.FromHandle(bm.GetHicon());
            CRSNotifyIcon.Text = "Leasing Registry System (Versiya " + GlobalVariables.V_Version + ")";
            CRSNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            CRSNotifyIcon.BalloonTipTitle = "Leasing Registry System";
            CRSNotifyIcon.BalloonTipText = "Versiya " + GlobalVariables.V_Version;
            CRSNotifyIcon.ShowBalloonTip(1000);           
            SmsBackstageViewButton.Visible = (File.Exists(GlobalVariables.V_ExecutingFolder + "\\SmsConsoleApp\\Sms_ConsoleApp.exe"));
            if (GlobalVariables.V_CommitmentCount > 0 && GlobalVariables.CommitContract)
            {
                //AlertInfo info = new AlertInfo("Xəbərdarlıq", GlobalVariables.V_CommitmentCount.ToString() + " müqavilə üçün yeni öhdəliklər təyin edilib.");
                //AlertControl.Show(this, info);
                GlobalProcedures.ShowNotification("Məlumat", GlobalVariables.V_CommitmentCount.ToString() + " müqavilə üçün yeni öhdəliklər təyin edilib.");
            }
        }

        private void LoadDefaultMenu()
        {
            switch (GlobalVariables.V_DefaultMenu)
            {
                case 0:
                    {
                        if (GlobalVariables.Info)
                            LoadInfoRibbonPage();
                    }
                    break;
                case 1:
                    {
                        if (GlobalVariables.Customer)
                            LoadCustomerRibbonPage();
                    }
                    break;
                case 2:
                    {
                        if (GlobalVariables.Contract)
                            LoadContractRibbonPage();
                    }
                    break;
                case 3:
                    {
                        if (GlobalVariables.Portfel)
                            LoadTotalRibbonPage();
                    }
                    break;
                case 4:
                    {
                        if (GlobalVariables.Cash)
                            LoadCashRibbonPage();
                    }
                    break;
                case 5:
                    {
                        if (GlobalVariables.Bank)
                            LoadBankRibbonPage();
                    }
                    break;
                case 6:
                    {
                        if (GlobalVariables.AttractedFunds)
                            LoadAttractedFundsRibbonPage();
                    }
                    break;
                case 7:
                    {
                        if (GlobalVariables.Bookkeeping)
                            LoadBookkeepingRibbonPage();
                    }
                    break;
                case 8:
                    {
                        if (GlobalVariables.PaymentTask)
                            LoadPaymentTaskRibbonPage();
                    }
                    break;
                case 9:
                    {
                        if (GlobalVariables.PaymentTask)
                            LoadSmsRibbonPage();
                    }
                    break;
            }
            MainRibbon.SelectedPage = MainRibbon.Pages[GlobalVariables.V_DefaultMenu];
        }

        private void CascadeWindowBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void VerticalWindowBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void HorizontalWindowBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void GalleryDropDown_GalleryItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(e.Item.Tag.ToString());
            SkinRibbonGallery.Caption = e.Item.Tag.ToString();
            GlobalVariables.V_StyleName = e.Item.Tag.ToString();
        }

        private void CloseBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            this.Close();
        }

        private void CalculatorBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            GlobalProcedures.Calculator();
        }

        private void UsersBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            FUsers fu = new FUsers();
            fu.ShowDialog();
        }

        int a = 0;
        private void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (a == 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("Proqramı bağlamaq istəyirsiniz?", "Proqramın bağlanılması", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        GlobalProcedures.SetSetting("StyleName", GlobalVariables.V_StyleName);
                        DirectoryInfo docDirectory = new DirectoryInfo(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents");
                        int fileCount = docDirectory.GetFiles().Length;

                        if (fileCount > 0 && GlobalVariables.WordDocumentUsed)
                        {
                            XtraMessageBox.Show("Açıq olan bütün word fayllar avtomatik olaraq bağlanılacaq.", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GlobalProcedures.KillWord();
                        }

                        GlobalProcedures.DeleteAllFilesInDirectory(GlobalVariables.V_ExecutingFolder + "\\WebCameraImages");
                        GlobalProcedures.DeleteAllFilesInDirectory(GlobalVariables.V_ExecutingFolder + "\\Reports");
                        GlobalProcedures.DeleteAllFilesInDirectory(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents");
                        GlobalProcedures.SetSetting("DefaultMenu", GlobalVariables.V_DefaultMenu.ToString());
                        GlobalProcedures.SetSetting("DefaultLanguage", GlobalVariables.V_DefaultLanguage.ToString());
                        GlobalProcedures.SetSetting("DefaultDateSort", GlobalVariables.V_DefaultDateSort.ToString());
                        CRSNotifyIcon.Visible = false;
                        GlobalProcedures.UpdateUserCloseConnection();
                        GlobalProcedures.UpdateUserDisconnected();

                        //try
                        //{
                        //    if (ConfigurationManager.AppSettings["SmsService"].ToString().Trim() == "true")
                        //    {
                        //        int IDstring = Convert.ToInt32(GlobalVariables.runSmsConsole.Id.ToString());
                        //        Process tempProc = Process.GetProcessById(IDstring);
                        //        tempProc.CloseMainWindow();
                        //        tempProc.WaitForExit();
                        //    }
                        //}
                        //catch
                        //{ }
                        a++;
                        Application.Exit();
                    }
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Sistem bağlanan zaman xəta baş verdi.", null, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void FMain_Activated(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ":
                    {
                        UserNameBarStatic.Caption = GlobalVariables.V_UserName;
                        if (GlobalVariables.V_License == 0)
                            LicenseBarStatic.Caption = "Statusu : Sınaq";
                        else
                            LicenseBarStatic.Caption = "Statusu : Lisenziyalı";
                    }
                    break;
                case "EN":
                    {
                        UserNameBarStatic.Caption = "Connected user : " + GlobalVariables.V_UserName;
                        if (GlobalVariables.V_License == 0)
                            LicenseBarStatic.Caption = "Status : Trial";
                        else
                            LicenseBarStatic.Caption = "Status : Licensed";
                    }
                    break;
                case "RU":
                    {
                        UserNameBarStatic.Caption = "Connected user : " + GlobalVariables.V_UserName;
                        if (GlobalVariables.V_License == 0)
                            LicenseBarStatic.Caption = "Статус : Пробный";
                        else
                            LicenseBarStatic.Caption = "Статус : Лицензионный";
                    }
                    break;
            }
            SkinRibbonGallery.Caption = GlobalVariables.V_StyleName;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);
            ConnectTimeBarStatic.Caption = null;
            ConnectTimeBarStatic.Caption = "Qoşulmanın müddəti - " + span.Hours + ":" + span.Minutes + ":" + span.Seconds;
        }

        private void DictionariesBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            FDictionaries fd = new FDictionaries();
            fd.ShowDialog();
        }

        Customer.FCustomers fc = null;
        private void LoadCustomerRibbonPage()
        {
            if (fc == null || fc.IsDisposed)
            {
                fc = new Customer.FCustomers();
                fc.MdiParent = this;
            }
            fc.WindowState = FormWindowState.Maximized;
            fc.Show();
        }

        Info.FInfo fi = null;
        private void LoadInfoRibbonPage()
        {
            if (fi == null || fi.IsDisposed)
            {
                fi = new Info.FInfo();
                fi.MdiParent = this;
            }
            fi.WindowState = FormWindowState.Maximized;
            fi.Show();
        }

        Contracts.FContracts fcon = null;
        private void LoadContractRibbonPage()
        {
            if (fcon == null || fcon.IsDisposed)
            {
                fcon = new Contracts.FContracts();
                fcon.MdiParent = this;
            }
            fcon.WindowState = FormWindowState.Maximized;
            fcon.Show();
        }

        Total.FTotal ft = null;
        private void LoadTotalRibbonPage()
        {
            if (ft == null || ft.IsDisposed)
            {
                ft = new Total.FTotal();
                ft.MdiParent = this;
            }
            ft.WindowState = FormWindowState.Maximized;
            ft.Show();
        }

        Forms.Cash.FCash fcsh = null;
        private void LoadCashRibbonPage()
        {
            if (fcsh == null || fcsh.IsDisposed)
            {
                fcsh = new Forms.Cash.FCash();
                fcsh.MdiParent = this;
            }
            fcsh.WindowState = FormWindowState.Maximized;
            fcsh.Show();
        }

        Bank.FBank fb = null;
        private void LoadBankRibbonPage()
        {
            if (fb == null || fb.IsDisposed)
            {
                fb = new Bank.FBank();
                fb.MdiParent = this;
            }
            fb.WindowState = FormWindowState.Maximized;
            fb.Show();
        }

        AttractedFunds.FAttractedFunds faf = null;
        private void LoadAttractedFundsRibbonPage()
        {
            if (faf == null || faf.IsDisposed)
            {
                faf = new AttractedFunds.FAttractedFunds();
                faf.MdiParent = this;
            }
            faf.WindowState = FormWindowState.Maximized;
            faf.Show();
        }

        Bookkeeping.FBookkeeping fbkk = null;
        private void LoadBookkeepingRibbonPage()
        {
            if (fbkk == null || fbkk.IsDisposed)
            {
                fbkk = new Bookkeeping.FBookkeeping();
                fbkk.MdiParent = this;
            }
            fbkk.WindowState = FormWindowState.Maximized;
            fbkk.Show();
        }

        PaymentTask.FPaymentTask fpt = null;
        private void LoadPaymentTaskRibbonPage()
        {
            if (fpt == null || fpt.IsDisposed)
            {
                fpt = new PaymentTask.FPaymentTask();
                fpt.MdiParent = this;
            }
            fpt.WindowState = FormWindowState.Maximized;
            fpt.Show();
        }

        Sms.FSms fsms = null;
        private void LoadSmsRibbonPage()
        {
            if (fsms == null || fsms.IsDisposed)
            {
                fsms = new Sms.FSms();
                fsms.MdiParent = this;
            }
            fsms.WindowState = FormWindowState.Maximized;
            fsms.Show();
        }

        Commons.FCommons fcm = null;
        private void LoadCommonsRibbonPage()
        {
            if (fcm == null || fcm.IsDisposed)
            {
                fcm = new Commons.FCommons();
                fcm.MdiParent = this;
            }
            fcm.WindowState = FormWindowState.Maximized;
            fcm.Show();
        }

        private void MainRibbon_SelectedPageChanged(object sender, EventArgs e)
        {
            switch (MainRibbon.SelectedPage.Name)
            {
                case "InfoRibbonPage":
                    {
                        LoadInfoRibbonPage();
                    }
                    break;
                case "CustomerRibbonPage":
                    {
                        LoadCustomerRibbonPage();
                    }
                    break;
                case "ContractRibbonPage":
                    {
                        LoadContractRibbonPage();
                    }
                    break;
                case "TotalRibbonPage":
                    {
                        LoadTotalRibbonPage();
                    }
                    break;
                case "CashRibbonPage":
                    {
                        LoadCashRibbonPage();
                    }
                    break;
                case "BankRibbonPage":
                    {
                        LoadBankRibbonPage();
                    }
                    break;
                case "AttractedFundsRibbonPage":
                    {
                        LoadAttractedFundsRibbonPage();
                    }
                    break;
                case "BookkeepingRibbonPage":
                    {
                        LoadBookkeepingRibbonPage();
                    }
                    break;
                case "PaymentTaskRibbonPage":
                    {
                        LoadPaymentTaskRibbonPage();
                    }
                    break;
                case "SmsRibbonPage":
                    {
                        LoadSmsRibbonPage();
                    }
                    break;
                case "CommonRibbonPage":
                    {
                        LoadCommonsRibbonPage();
                    }
                    break;
            }
        }

        private void CreditCalculatorBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            GlobalProcedures.CreditCalculator();
        }

        private void ExchangeBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            GlobalProcedures.ExchangeCalculator(null);
            GlobalProcedures.GenerateRateText();
        }

        private void GenerateMenuPermisions()
        {
            if (GlobalVariables.V_UserID > 0)
            {
                UsersBackstageViewButton.Visible = GlobalVariables.User;
                DictionariesBackstageViewButton.Visible = GlobalVariables.Dictionaries;
                CustomerRibbonPage.Visible = GlobalVariables.Customer;
                ContractRibbonPage.Visible = GlobalVariables.Contract;
                TotalRibbonPage.Visible = GlobalVariables.Portfel;
                CashRibbonPage.Visible = GlobalVariables.Cash;
                BankRibbonPage.Visible = GlobalVariables.Bank;
                AttractedFundsRibbonPage.Visible = GlobalVariables.Funds;
                BookkeepingRibbonPage.Visible = GlobalVariables.Bookkeeping;
                PaymentTaskRibbonPage.Visible = GlobalVariables.PaymentTask;
                SmsRibbonPage.Visible = GlobalVariables.Sms;
            }
        }

        private void OptionsBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            Options.FOptions fo = new Options.FOptions();
            fo.ShowDialog();
        }

        private void DateCalculatorBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            FDateCalculator fdc = new FDateCalculator();
            fdc.ShowDialog();
        }

        private void AboutBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            FAbout fab = new FAbout();
            fab.ShowDialog();
        }

        private void MyProfileBackstageViewButtonItem_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            FMyAccount fma = new FMyAccount();
            fma.ShowDialog();
        }

        void RefreshLicense(string license, string id, string cpu, DateTime edate)
        {
            switch (license)
            {
                case "JNCQPcIIZypbYqwatzvXlwRgy5tpFW6y":
                    {
                        GlobalVariables.V_License = 0;
                    }
                    break;
                case "Uvki+HBeAPgq2di6nfFJm8Uu3b3ldYbx":
                    {
                        GlobalVariables.V_License = 1;
                    }
                    break;
            }
        }

        private void LicenseBarStatic_ItemClick(object sender, ItemClickEventArgs e)
        {
            FLicense fl = new FLicense();
            fl.RefreshLicense += new FLicense.DoEvent(RefreshLicense);
            fl.ShowDialog();
        }

        private void BackupViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            string backupFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_Backup.dmp",
                logFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_Backup.log",
                    oracleUser = "CRS_USER",
                    oraclePassword = "L!z!nq2o2!",
                    oracleSID = GlobalFunctions.ReadSetting("OracleSID"),
                    oracleDIR = GlobalFunctions.ReadSetting("OracleDIR"),
                    dumpdir = GlobalFunctions.GetName("SELECT DIRECTORY_PATH FROM SYS.ALL_DIRECTORIES WHERE DIRECTORY_NAME = 'LRS_DUMP_DIR'");
            XtraMessageBox.AllowHtmlText = true;
            XtraMessageBox.Show("Backup alınan zaman açılan qara pəncərəni bağlamayın. <b><color=red>Əks halda backup tam olmayacaq.</color></b><br><br><b>Qeyd:</b> <i>Açılan pəncərə backup başa çatdıqdan sonra avtomatik olaraq bağlanacaq.<i>", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();

                psi.FileName = Path.Combine(oracleDIR, "bin", "expdp");
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(oracleUser + "/" + oraclePassword + "@" + oracleSID + " schemas=" + oracleUser + " directory =lrs_dump_dir dumpfile=" + backupFileName + " logfile=" + logFileName);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);
                process.WaitForExit();
                process.Close();
            }
            catch (Exception exx)
            {
                XtraMessageBox.Show("Bazanın backup-ı alınmadı. \r\nError : " + exx.Message, "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SmsBackstageViewButton_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            Process.Start(GlobalVariables.V_ExecutingFolder + "\\SmsConsoleApp\\Sms_ConsoleApp.exe");
        }

        private void VersionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string networkPath = GlobalFunctions.ReadSetting("UpdatePath"),
                   currentVersion = GlobalVariables.V_Version;

                int isServer = int.Parse(GlobalFunctions.ReadSetting("IsServer"));

                if (isServer == 0)
                {
                    NetworkCredential credentials = new NetworkCredential(@GlobalFunctions.ReadSetting("ServerUserName"), GlobalFunctions.ReadSetting("ServerPassword"));

                    using (new ConnectToSharedFolder(networkPath, credentials))
                    {
                        VersionNotify(networkPath, currentVersion);
                    }
                }
                else
                    VersionNotify(networkPath, currentVersion);
            }
            catch (Exception exx)
            {

            }
        }

        private void VersionNotify(string networkPath, string currentVersion)
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
                        Bitmap bm = new Bitmap(Properties.Resources.yellow_leasing);
                        CRSNotifyIcon.Icon = Icon.FromHandle(bm.GetHicon());
                        CRSNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                        CRSNotifyIcon.BalloonTipTitle = "Leasing Registry System";
                        CRSNotifyIcon.BalloonTipText = $@"Yeni {lastVersion} versiya mövcuddur. Versiyanı dəyişmək üçün sistemi bağlayıb yenidən açın.";
                        CRSNotifyIcon.ShowBalloonTip(30000);
                    }
                }
            }
        }
    }
}