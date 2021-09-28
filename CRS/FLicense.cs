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
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS
{
    public partial class FLicense : DevExpress.XtraEditors.XtraForm
    {
        public FLicense()
        {
            InitializeComponent();
        }
        string LicenseID, license, cpu;
        bool exists_license = false, is_trial = false;
        DateTime startdate, end_date;

        public delegate void DoEvent(string license, string id, string cpu, DateTime end_date);
        public event DoEvent RefreshLicense;

        private void FLicense_Load(object sender, EventArgs e)
        {
            LicenseID = GlobalFunctions.Decrypt(GlobalFunctions.ReadSetting("LicenseID")).Trim();
            string sql = $@"SELECT ID,LICENSE,START_DATE,TO_CHAR(END_DATE,'DD.MM.YYYY') END_DATE,CPU FROM CRS_USER.CRS_LICENSE WHERE CPU = '{GlobalFunctions.Encrypt(LicenseID + GlobalFunctions.GetCPUID())}'";
            DataTable dt = GlobalFunctions.GenerateDataTable(sql, "FLicense_Load");
            if (dt.Rows.Count == 0)
            {
                exists_license = false;
                LicenseTypeLabel.Text = "Sizin heç bir lisenziyanız yoxdur.";
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr["LICENSE"].ToString())
                    {
                        case "JNCQPcIIZypbYqwatzvXlwRgy5tpFW6y":
                            is_trial = true;//trial
                            LicenseTypeLabel.Text = "Sizin 30 günlük lisenziyanız var. Son tarix " + dr["END_DATE"];
                            break;
                        case "Uvki+HBeAPgq2di6nfFJm8Uu3b3ldYbx":
                            is_trial = false;
                            LicenseTypeLabel.Text = "Sizin daimi lisenziyanız var.";
                            break;
                    }
                }
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ControlLicenseDetail()
        {
            bool b = false, a = false;

            if (LicenseCodeText.Text.Length == 0)
            {
                LicenseCodeText.BackColor = Color.Red;
                XtraMessageBox.Show("Lisenziya daxil edilməyib.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LicenseCodeText.Focus();
                LicenseCodeText.BackColor = Color.White;
                return false;
            }
            else
                b = true;

            switch (GlobalFunctions.Encrypt(LicenseCodeText.Text.Trim()))
            {
                case "JNCQPcIIZypbYqwatzvXlwRgy5tpFW6y":
                    a = true;//trial
                    break;
                case "Uvki+HBeAPgq2di6nfFJm8Uu3b3ldYbx":
                    a = true;
                    break;
            }

            if (!a)
            {
                LicenseCodeText.BackColor = Color.Red;
                XtraMessageBox.Show("Lisenziya düzgün deyil.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LicenseCodeText.Focus();
                LicenseCodeText.BackColor = Color.White;
                return false;
            }
            else
                b = true;

            if (is_trial && GlobalFunctions.Encrypt(LicenseCodeText.Text.Trim()) == "JNCQPcIIZypbYqwatzvXlwRgy5tpFW6y")
            {
                XtraMessageBox.Show("Siz artıq bir dəfə 30 günlük lisenziyadan istifadə etmisiniz. Təkrar olaraq 30 günlük lisenziya almaq olmaz.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                b = true;

            //if (!is_trial && GlobalFunctions.Encrypt(LicenseCodeText.Text.Trim()) == "Uvki+HBeAPgq2di6nfFJm8Uu3b3ldYbx")
            //{
            //    XtraMessageBox.Show("Siz artıq bir dəfə daimi lisenziyadan istifadə etmisiniz. Təkrar olaraq daimi lisenziya almaq olmaz.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
            //else
            //    b = true;

            if (PasswordText.Text != "@q1LEAS!NGw22015e3LRSr4")
            {
                PasswordText.BackColor = Color.Red;
                XtraMessageBox.Show("Şifrə düzgün deyil.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PasswordText.Focus();
                PasswordText.BackColor = Color.White;
                return false;
            }
            else
                b = true;

            return b;
        }

        private bool InsertLicense()
        {
            string sql = null;
            int day_count = 0;            

            LicenseID = GlobalFunctions.GetOracleSequenceValue("LICENSE_SEQUENCE").ToString();
            license = GlobalFunctions.Encrypt(LicenseCodeText.Text.Trim());
            cpu = GlobalFunctions.Encrypt(LicenseID + GlobalFunctions.GetCPUID());
            startdate = DateTime.Today;
            switch (license)
            {
                case "JNCQPcIIZypbYqwatzvXlwRgy5tpFW6y":
                    day_count = 30;  //1 ay
                    break;
                case "Uvki+HBeAPgq2di6nfFJm8Uu3b3ldYbx":
                    day_count = 9000; //25 il
                    break;
            }
            end_date = startdate.AddDays(day_count);
            sql = $@"INSERT INTO CRS_USER.CRS_LICENSE(
                                                        ID,
                                                        DAY_COUNT,
                                                        LICENSE,
                                                        START_DATE,
                                                        END_DATE,
                                                        CPU
                                                     )
                     VALUES(
                                {LicenseID},
                                {day_count},
                                '{license}',
                                TO_DATE('{startdate.ToString("d", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                TO_DATE('{end_date.ToString("d", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                '{cpu}'
                            )";
            return (GlobalFunctions.ExecuteQuery(sql, "Lisenziya daxil edilmədi.") > 0);
        }

        private bool UpdateLicense()
        {
            string sql = null;
            int day_count = 0;
            DateTime startdate, endate;
            LicenseID = GlobalFunctions.Decrypt(GlobalFunctions.ReadSetting("LicenseID")).Trim();
            license = GlobalFunctions.Encrypt(LicenseCodeText.Text.Trim());
            cpu = GlobalFunctions.Encrypt(LicenseID + GlobalFunctions.GetCPUID());
            startdate = DateTime.Today;
            switch (license)
            {
                case "JNCQPcIIZypbYqwatzvXlwRgy5tpFW6y":
                    day_count = 30;  //1 ay
                    break;
                case "Uvki+HBeAPgq2di6nfFJm8Uu3b3ldYbx":
                    day_count = 9000; //25 il
                    break;
            }
            endate = startdate.AddDays(day_count);


            sql = $@"UPDATE CRS_USER.CRS_LICENSE SET 
                                            DAY_COUNT = {day_count},
                                            LICENSE = '{license}',
                                            START_DATE = TO_DATE('{startdate.ToString("d", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY'),
                                            END_DATE = TO_DATE('{endate.ToString("d", GlobalVariables.V_CultureInfoEN.DateTimeFormat)}','MM/DD/YYYY') 
                        WHERE CPU = '{cpu}'";

            return (GlobalFunctions.ExecuteQuery(sql, "Lisenziya dəyişdirilmədi.") > 0);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            bool close = true;
            if (ControlLicenseDetail())
            {
                if (!exists_license)
                {
                    close = InsertLicense();
                    if (close)
                        GlobalProcedures.SetSetting("LicenseID", GlobalFunctions.Encrypt(LicenseID));
                }
                else
                    close = UpdateLicense();

                if (close)
                {
                    this.RefreshLicense(license, LicenseID, cpu, end_date);
                    this.Close();
                }
            }
        }
    }
}