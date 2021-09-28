using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net.Mail;
using System.Net.Configuration;
using System.Configuration;
using System.Net;
using CRS.Class;

namespace CRS.Forms
{
    public partial class FForgetPassword : DevExpress.XtraEditors.XtraForm
    {
        public FForgetPassword()
        {
            InitializeComponent();
        }
        string UserFullName, UserNikName, UserPassword;

        private void FForgetPassword_Load(object sender, EventArgs e)
        {
           
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (UserNameText.Text.Length == 0)
            {
                UserNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçi adı daxil edilməyib.");
                UserNameText.Focus();
                UserNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (MailText.Text.Length == 0)
            {
                MailText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şifrəni göndərmək üçün elektron ünvan daxil edilməyib.");
                MailText.Focus();
                MailText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!GlobalFunctions.FindUserName(GlobalFunctions.Encrypt(UserNameText.Text.Trim())))
            {
                UserNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçi adı ya düz deyil ya da bu istifadəçinin sistemə girişi bağlanılıb.");
                UserNameText.Focus();
                UserNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.MAILS WHERE OWNER_TYPE = 'U' AND OWNER_ID = " + GlobalVariables.V_UserID + " AND MAIL = '" + MailText.Text.Trim() + "'") == 0)
            {
                MailText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Daxil etdiyiniz elektron ünvan " + UserNameText.Text.Trim() + " adlı istifadəçinin bazada olan heç bir elektron ünvanına uyğun gəlmədi. Elektron ünvanı yenidən daxil edin.");
                MailText.Focus();
                MailText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void LoadUserDetails()
        {
            string s = $@"SELECT SURNAME||' '||NAME||' '||PATRONYMIC FULLNAME,NIKNAME,PASSWORD FROM CRS_USER.CRS_USERS WHERE STATUS_ID = 1 AND ID = {GlobalVariables.V_UserID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadUserDetails", "İstifadəçinin parametrləri açılmadı.");
            if(dt.Rows.Count > 0)
            {
                UserFullName = dt.Rows[0]["FULLNAME"].ToString();
                UserNikName = GlobalFunctions.Decrypt(dt.Rows[0]["NIKNAME"].ToString());
                UserPassword = GlobalFunctions.Decrypt(dt.Rows[0]["PASSWORD"].ToString());
            }            
        }

        private void SendMail()
        {
            BOK.Enabled = false;
            string host, username, password, fromaddress, fromname = "Faktor Lizinq", toaddress, toname = UserFullName;
            int port;
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("mailSettings");

            fromaddress = section.From;
            toaddress = MailText.Text.Trim();
            host = section.Network.Host;
            port = section.Network.Port;
            username = section.Network.UserName;
            password = section.Network.Password;

            try
            {
                using (var message = new MailMessage(new MailAddress(fromaddress, fromname), new MailAddress(toaddress, toname)))
                {
                    message.Subject = "LRS sisteminə daxil olmaq üçün lazım olan rekvizitlər";
                    message.Body = "<b>Hörmətli " + UserFullName + ",</b><br><br>Sizin LRS sisteminə daxil olmaq üçün istifadəçi adınız və şifrəniz aşağıdakı kimidir.<br><br><b>İstifadəçi adı:</b> " + UserNikName + "<br><b>Şifrə:</b> " + UserPassword + "<br><br><br><i>Bu mail sistem tərəfindən avtomatik olaraq göndərilib.</i>";
                    message.IsBodyHtml = true;

                    using (SmtpClient client = new SmtpClient
                    {
                        EnableSsl = true,
                        Host = host,
                        Port = port,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(username, password)
                    })
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };
                        client.Send(message);
                        XtraMessageBox.Show("Şifrə elektron ünvanınıza göndərildi.", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            catch (SmtpException exx)
            {
                if (exx.StatusCode == SmtpStatusCode.MailboxUnavailable ||
                    exx.StatusCode == SmtpStatusCode.MailboxBusy ||
                    exx.StatusCode == SmtpStatusCode.CommandUnrecognized ||
                    exx.StatusCode == (SmtpStatusCode)556)
                {
                    return;
                }

            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şifrə elektron ünvana göndərilmədi.", "toaddress = " + toaddress, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
            finally
            {
                BOK.Enabled = true;
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                LoadUserDetails();
                SendMail();
            }
        }
    }
}