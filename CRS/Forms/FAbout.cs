using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CRS.Forms
{
    public partial class FAbout : DevExpress.XtraEditors.XtraForm
    {
        public FAbout()
        {
            InitializeComponent();
        }

        private void FAbout_Load(object sender, EventArgs e)
        {
            VersionLabel.Text = "Version " + Class.GlobalVariables.V_Version;
            if (Class.GlobalVariables.V_License == 0)
            {
                LicenseLabel.Text = "Sınaq";
                DescriptionLabel.Visible = true;
            }
            else
            {
                LicenseLabel.Text = "Lisenziyalı";
                DescriptionLabel.Visible = false;
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void WebHyperlinkLabel_HyperlinkClick(object sender, DevExpress.Utils.HyperlinkClickEventArgs e)
        //{
        //    WebHyperlinkLabel.LinkVisited = true;
        //    System.Diagnostics.Process.Start(WebHyperlinkLabel.Text);
        //}
    }
}