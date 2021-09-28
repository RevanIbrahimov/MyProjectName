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
using CRS.Class;

namespace CRS.Forms.Contracts
{
    public partial class FCommitmentLetterDetails : DevExpress.XtraEditors.XtraForm
    {
        public FCommitmentLetterDetails()
        {
            InitializeComponent();
        }
        public string CustomerName, CommitmentName;

        public delegate void DoEvent(string number, string number2, string customerName, string drivingLicense1, string commitmentName, string drivingLicense2, bool cancel);
        public event DoEvent GetDetails;

        bool ok = false;

        private bool ControlDetail()
        {
            bool b = false;
            
            if (NumberText.Text.Length == 0)
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məktubun nömrəsi daxil edilməyib.");
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (Number2Text.Text.Length == 0)
            {
                Number2Text.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Arayışım nömrəsi daxil edilməyib.");
                Number2Text.Focus();
                Number2Text.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CustomerNameText.Text.Length == 0)
            {
                CustomerNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Öhdəlik ötürənin adı daxil edilməyib.");
                CustomerNameText.Focus();
                CustomerNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DrivingLicense1Text.Text.Length == 0)
            {
                DrivingLicense1Text.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Öhdəlik ötürənin sürücük vəsiqəsi daxil edilməyib.");
                DrivingLicense1Text.Focus();
                DrivingLicense1Text.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CommitmentNameText.Text.Length == 0)
            {
                CommitmentNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Öhdəlik götürənin adı daxil edilməyib.");
                CommitmentNameText.Focus();
                CommitmentNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (DrivingLicense2Text.Text.Length == 0)
            {
                DrivingLicense2Text.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Öhdəlik götürənin sürücük vəsiqəsi daxil edilməyib.");
                DrivingLicense2Text.Focus();
                DrivingLicense2Text.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlDetail())
            {
                ok = true;
                this.Close();
            }
        }

        private void FCommitmentLetterDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.GetDetails(NumberText.Text.Trim(), Number2Text.Text.Trim(), CustomerNameText.Text.Trim(), DrivingLicense1Text.Text.Trim(), CommitmentNameText.Text.Trim(), DrivingLicense2Text.Text.Trim(), ok);
        }

        private void FCommitmentLetterDetails_Load(object sender, EventArgs e)
        {
            
        }
    }
}