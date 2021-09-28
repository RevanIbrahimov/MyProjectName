using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Net;

namespace UpdateLRS
{
    public partial class FUpdate : DevExpress.XtraEditors.XtraForm
    {
        public FUpdate()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (UpdateBackgroundWorker.IsBusy != true)
                UpdateBackgroundWorker.RunWorkerAsync();
        }

        private void UpdateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            VersionLabel.Invoke((MethodInvoker)delegate
            {
                VersionLabel.Text = "LRS sisteminin versiyası yenilənir...";
            });

            MarqueeProgressBarControl.Invoke((MethodInvoker)delegate
            {
                MarqueeProgressBarControl.Text = "update...";
            });

            try
            {
                string executingFolder = new FileInfo((Assembly.GetExecutingAssembly().Location)).Directory.FullName;

                var lrsConfig = ConfigurationManager.OpenExeConfiguration(executingFolder + "\\LRS.exe");
                AppSettingsSection appSettings = (AppSettingsSection)lrsConfig.GetSection("appSettings");                

                string networkPath = appSettings.Settings["UpdatePath"].Value.Trim();
                int isServer = int.Parse(appSettings.Settings["IsServer"].Value.Trim());
                if (isServer == 0)
                {
                    NetworkCredential credentials = new NetworkCredential(@appSettings.Settings["ServerUserName"].Value.Trim(), appSettings.Settings["ServerPassword"].Value.Trim());

                    using (new ConnectToSharedFolder(networkPath, credentials))
                    {
                        ChangeVersion(networkPath, lrsConfig, executingFolder);
                    }
                }
                else
                    ChangeVersion(networkPath, lrsConfig, executingFolder);

            }
            catch (Exception exx)
            {
                XtraMessageBox.Show("LRS update olmadı.\r\n" + exx.Message, "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void ChangeVersion(string networkPath, Configuration lrsConfig, string executingFolder)
        {
            if (Directory.Exists(networkPath))
            {

                DirectoryInfo dir = new DirectoryInfo(networkPath);
                FileInfo[] files = dir.GetFiles();

                if (files.Length == 0)
                {
                    Application.Exit();
                    return;
                }

                string lastVersion = null;

                foreach (FileInfo file in files)
                {
                    if (file.Name == "VERSION.txt")
                    {
                        lastVersion = File.ReadAllText(file.FullName).Trim();

                        var entry = lrsConfig.AppSettings.Settings["CurrentVersion"];
                        if (entry == null)
                            lrsConfig.AppSettings.Settings.Add("CurrentVersion", lastVersion);
                        else
                            lrsConfig.AppSettings.Settings["CurrentVersion"].Value = lastVersion;

                        lrsConfig.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");

                        continue;
                    }

                    string sourcepath = Path.Combine(networkPath, file.Name),
                            temppath = Path.Combine(executingFolder, file.Name);

                    if (File.Exists(sourcepath))
                        File.Delete(temppath);

                    File.Copy(sourcepath, temppath);
                }

                Thread.Sleep(5000);

                VersionLabel.Invoke((MethodInvoker)delegate
                {
                    VersionLabel.Text = "LRS sisteminin versiyası yeniləndi...";
                });

                MarqueeProgressBarControl.Invoke((MethodInvoker)delegate
                {
                    MarqueeProgressBarControl.Text = "loading...";
                });

                Thread.Sleep(5000);

                Process.Start(executingFolder + "\\LRS.exe");
                Application.Exit();
            }
        }

        private void UpdateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
    }
}
