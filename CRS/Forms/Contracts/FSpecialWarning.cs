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

namespace CRS.Forms.Contracts
{
    public partial class FSpecialWarning : DevExpress.XtraEditors.XtraForm
    {
        public FSpecialWarning()
        {
            InitializeComponent();
        }
        public string Description;

        private void FSpecialWarning_Load(object sender, EventArgs e)
        {
            labelControl2.Text = Description;
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }
    }
}