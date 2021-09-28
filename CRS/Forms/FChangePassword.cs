using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FChangePassword : DevExpress.XtraEditors.XtraForm
    {
        public FChangePassword()
        {
            InitializeComponent();
        }

        private void FChangePassword_Load(object sender, EventArgs e)
        {

        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ControlPasswordDetails()
        {
            bool b = false;
            var user = GlobalVariables.lstUsers.Find(u => u.NIKNAME == GlobalFunctions.Encrypt(GlobalVariables.V_UserName));
            if (user == null)
            {
                GlobalProcedures.ShowErrorMessage("İstifadəçi adı ya düz deyil ya da bu istifadəçinin sistemə girişi bağlanılıb.");
                return false;
            }

            if (String.IsNullOrEmpty(OldPasswordText.Text))
            {
                OldPasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Köhnə şifrə daxil edilməyib.");                
                OldPasswordText.Focus();
                OldPasswordText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (user.PASSWORD == GlobalFunctions.Encrypt(OldPasswordText.Text.Trim()))
                b = true;
            else
            {
                OldPasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Köhnə şifrə düz deyil.");                
                OldPasswordText.Focus();
                OldPasswordText.BackColor = Color.White;
                return false;
            }

            if (String.IsNullOrEmpty(NewPasswordText.Text))
            {
                NewPasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Yeni şifrə daxil edilməyib.");                
                NewPasswordText.Focus();
                NewPasswordText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else if (NewPasswordText.Text.Length < 7)
            {
                NewPasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şifrənin uzunluğu 6 simvoldan az ola bilməz.");
                NewPasswordText.Focus();
                NewPasswordText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!System.Text.RegularExpressions.Regex.IsMatch(NewPasswordText.Text, "^[a-zA-Z0-9]+$"))
            {
                NewPasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şifrə yalnız ingilis hərflərindən və ya rəqəmdən ibarət olmalıdır");
                NewPasswordText.Focus();
                NewPasswordText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NewPasswordText.Text == AgainPasswordText.Text)
                b = true;
            else
            {
                AgainPasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Yeni şifrənin təkrarı düz deyil.");                
                AgainPasswordText.Focus();
                AgainPasswordText.BackColor = Color.White;
                return false;
            }

            return b;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlPasswordDetails())
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CRS_USERS SET PASSWORD = '{GlobalFunctions.Encrypt(NewPasswordText.Text)}' WHERE ID = {GlobalVariables.V_UserID}", 
                                                    "İstifadəçinin şifrəsi dəyişdirilmədi.");
                XtraMessageBox.Show(GlobalVariables.V_UserName + " adlı istifadəçinin şifrəsi dəyişdirildi.");
                this.Close();
            }
        }

    }
}