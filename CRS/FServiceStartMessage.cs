using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.ServiceProcess;
using CRS.Class;

namespace CRS
{
    public partial class FServiceStartMessage : DevExpress.XtraEditors.XtraForm
    {
        public FServiceStartMessage()
        {
            InitializeComponent();
        }
        public string ListenerServiceName, DataBaseServiceName;

        ServiceController myService = new ServiceController();
        bool is_start = false;

        public delegate void DoEvent(bool is_start);
        public event DoEvent RefreshService;

        private void FServiceStartMessage_Load(object sender, EventArgs e)
        {
            if (ServiceBackgroundWorker.IsBusy != true)
                ServiceBackgroundWorker.RunWorkerAsync();
        }

        
        private void BCancel_Click(object sender, EventArgs e)
        {
            if (ServiceBackgroundWorker.WorkerSupportsCancellation == true)
                ServiceBackgroundWorker.CancelAsync();

            bool listener = GlobalFunctions.StopService(ListenerServiceName),
                database = GlobalFunctions.StopService(DataBaseServiceName);

            is_start = (listener && database);
            this.Close();
        }

        private void ServiceBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DescriptionLabel.Invoke((MethodInvoker)delegate
            {
                DescriptionLabel.Text = "Servislər işə salınır.";
            });

            bool listener = GlobalFunctions.StartService(ListenerServiceName),
                 database = GlobalFunctions.StartService(DataBaseServiceName);

            is_start = (listener && database);
        }

        private void FServiceStartMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshService(is_start);
        }

        private void ServiceBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                XtraMessageBox.Show("Canceled!");
            }
            else if (e.Error != null)
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