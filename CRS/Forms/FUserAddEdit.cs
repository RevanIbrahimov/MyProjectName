using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Collections;
using DevExpress.Utils;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms
{
    public partial class FUserAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FUserAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, UserID, GroupID;
        string PhoneID,
            PhoneNumber,
            MailID,
            UserImage,
            usernikname,
            userniknametype,
            UserImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images",
            group_name;

        int status_id,
            sex_id,
            group_id,
            UsedUserID = -1,
            crop_image_count = 0,
            mail_selected_count = 0;

        bool CurrentStatus = false,
            UserClosed = false,
            UserConnected = false,
            UserUsed = false,
            maildetails = false,
            permissiondetails = false;

        public delegate void DoEvent();
        public event DoEvent RefreshUserDataGridView;

        private void FUserAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                GroupNameLookUp.Properties.Buttons[1].Visible = GlobalVariables.UsersGroup;
                StatusLookUp.Properties.Buttons[1].Visible = GlobalVariables.Status;
            }

            GlobalProcedures.FillLookUpEdit(SexLookUp, "SEX", "ID", "NAME", "1 = 1 ORDER BY ID");
            GlobalProcedures.FillLookUpEdit(GroupNameLookUp, "USER_GROUP", "ID", "GROUP_NAME", null);
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CRS_USERS", GlobalVariables.V_UserID, "WHERE ID = " + UserID + " AND USED_USER_ID = -1");
                GlobalProcedures.FillLookUpEdit(StatusLookUp, "STATUS", "ID", "STATUS_NAME", "ID IN (1, 2)");
                List<LrsUsers> lstLrsUser = LrsUsersDAL.SelectUser(int.Parse(UserID)).ToList<LrsUsers>();
                var user = lstLrsUser.First();

                if (user.STATUS_ID == 2)
                    UserClosed = true;
                else
                {
                    UserClosed = false;
                    if (user.SESSION_ID == 0)
                        UserConnected = false;
                    else
                        UserConnected = true;
                }

                UsedUserID = user.USED_USER_ID;
                UserUsed = (user.USED_USER_ID > 0);

                if (((UserClosed) && (UserUsed)) || ((UserClosed) && (!UserUsed)))
                {
                    XtraMessageBox.Show("İstifadəçi sistemdə bağlanılıb. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş istifadəçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else if ((!UserClosed) && (UserUsed))
                {
                    if (GlobalVariables.V_UserID != UsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                        XtraMessageBox.Show("İstifadəçinin məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş istifadəçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else if (UserConnected)
                    {
                        XtraMessageBox.Show("İstifadəçi sistemə daxil olub. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş istifadəçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;

                StatusLookUp.Visible = StatusLabel.Visible = true;
                ComponentEnable(CurrentStatus);
                InsertTemps();
                LoadUserDetails();
            }
            else
            {
                maildetails = permissiondetails = true;
                UserID = GlobalFunctions.GetOracleSequenceValue("USER_SEQUENCE").ToString();
                RegistrationIDText.Text = UserID;
                switch (GlobalVariables.SelectedLanguage)
                {
                    case "RU":
                        UserPictureBox.Properties.NullText = "Пользователь фото";
                        break;
                    case "EN":
                        UserPictureBox.Properties.NullText = "User picture";
                        break;
                }
            }
        }

        private void ComponentEnable(bool status)
        {
            StatusLookUp.Enabled =
                BLoadPicture.Enabled =
                PhoneStandaloneBarDockControl.Enabled =
                MailStandaloneBarDockControl.Enabled =
                GroupNameLookUp.Enabled =
                OutomaticUserNameCheckEdit.Enabled =
                OutomaticUserNameComboBoxEdit.Enabled =
                ManualUserNameText.Enabled =
                PasswordText.Enabled =
                BLoadPicture.Enabled =
                SurnameText.Enabled =
                NameText.Enabled =
                PatronymicText.Enabled =
                BirthdayDate.Enabled =
                AddressText.Enabled =
                SexLookUp.Enabled =
                NoteText.Enabled =
                ShowPasswordCheck.Enabled =
                BOK.Visible = !status;
        }

        private void LoadUserDetails()
        {
            string s = $@"SELECT U.ID,
                               U.NAME,
                               U.SURNAME,
                               U.PATRONYMIC,
                               U.NIKNAME_TYPE,
                               U.NIKNAME,
                               U.PASSWORD,
                               S.STATUS_NAME,                               
                               U.BIRTHDAY,
                               SE.NAME SEX_NAME,                               
                               ADDRESS,
                               G.GROUP_NAME,                               
                               U.CLOSED_DATE,
                               U.NOTE,
                               U.ETL_DT_TM,
                               UI.IMAGE,
                               U.GROUP_ID
                          FROM CRS_USER.CRS_USERS U,
                               CRS_USER.STATUS S,
                               CRS_USER.SEX SE,
                               CRS_USER.USER_GROUP G,
                               CRS_USER.USER_IMAGE UI
                         WHERE     U.GROUP_ID = G.ID(+)
                               AND U.SEX_ID = SE.ID
                               AND U.STATUS_ID = S.ID
                               AND U.ID = UI.USER_ID(+)
                               AND U.ID = {UserID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s);


                foreach (DataRow dr in dt.Rows)
                {
                    RegistrationIDText.Text = dr["ID"].ToString();
                    NameText.Text = dr["NAME"].ToString();
                    SurnameText.Text = dr["SURNAME"].ToString();
                    PatronymicText.Text = dr["PATRONYMIC"].ToString();
                    if (Convert.ToInt32(dr["NIKNAME_TYPE"].ToString()) == 1)
                    {
                        OutomaticUserNameCheckEdit.Checked = true;
                        OutomaticUserNameComboBoxEdit.EditValue = GlobalFunctions.Decrypt(dr["NIKNAME"].ToString());
                    }
                    else
                    {
                        OutomaticUserNameCheckEdit.Checked = false;
                        ManualUserNameText.Text = GlobalFunctions.Decrypt(dr["NIKNAME"].ToString());
                    }
                    PasswordText.Text = GlobalFunctions.Decrypt(dr["PASSWORD"].ToString());
                    StatusLookUp.EditValue = StatusLookUp.Properties.GetKeyValueByDisplayText(dr["STATUS_NAME"].ToString());
                    StatusLookUp.Enabled = !(status_id == 2);
                    CloseDateValue.Enabled = !(status_id == 2);
                    BOK.Enabled = !(status_id == 2);
                    NoteText.Enabled = !(status_id == 2);
                    BLoadPicture.Enabled = !(status_id == 2);
                    BDeletePicture.Enabled = !(status_id == 2);

                    BirthdayDate.EditValue = DateTime.Parse(dr["BIRTHDAY"].ToString());
                    SexLookUp.EditValue = SexLookUp.Properties.GetKeyValueByDisplayText(dr["SEX_NAME"].ToString());
                    AddressText.Text = dr["ADDRESS"].ToString();
                    group_name = dr["GROUP_NAME"].ToString();
                    group_id = Convert.ToInt32(dr["GROUP_ID"].ToString());
                    if (!String.IsNullOrEmpty(dr["CLOSED_DATE"].ToString()))
                        CloseDateValue.EditValue = DateTime.Parse(dr["CLOSED_DATE"].ToString());
                    else
                        CloseDateValue.EditValue = DateTime.Today;

                    CloseDateValue.Properties.MinValue = DateTime.Parse(dr["ETL_DT_TM"].ToString());

                    NoteText.Text = dr["NOTE"].ToString();

                    if (!DBNull.Value.Equals(dr["IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        UserPictureBox.Image = Image.FromStream(stmBLOBData);

                        UserImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images";

                        FileStream fs = new FileStream(UserImagePath + "\\U_" + RegistrationIDText.Text + ".jpeg", FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(fs);
                        fs.Close();
                        stmBLOBData.Close();
                        UserImage = UserImagePath + "\\U_" + RegistrationIDText.Text + ".jpeg";
                        BLoadPicture.Text = "Dəyiş";
                        if (status_id == 1)
                        {
                            BDeletePicture.Enabled = !CurrentStatus;
                            BLoadPicture.Enabled = !CurrentStatus;
                        }
                    }
                    else
                    {
                        BLoadPicture.Text = "Yüklə";
                        if (status_id == 1)
                            BLoadPicture.Enabled = !CurrentStatus;
                        BDeletePicture.Enabled = false;
                        UserImage = null;
                        switch (GlobalVariables.SelectedLanguage)
                        {
                            case "RU":
                                UserPictureBox.Properties.NullText = "Пользователь фото";
                                break;
                            case "EN":
                                UserPictureBox.Properties.NullText = "User picture";
                                break;
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İstifadəçinin parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BLoadPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "İstifadəçinin şəklini seçin";
                dlg.Filter = "All files (*.*)|*.*|Image files (*.jpeg)|*.jpeg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    UserPictureBox.Image = new Bitmap(dlg.FileName);
                    UserImage = dlg.FileName;
                    BDeletePicture.Enabled = !CurrentStatus;
                }
                dlg.Dispose();
            }
        }

        private void BDeletePicture_Click(object sender, EventArgs e)
        {
            UserPictureBox.Image = null;
            UserImage = null;
            BLoadPicture.Text = "Yüklə";
            BDeletePicture.Enabled = false;
        }

        private void ManualUserNameText_Enter(object sender, EventArgs e)
        {
            WarningUserNameLabel.Visible = true;
            WarningPasswordLabel.Visible = false;
        }

        private void ManualUserNameText_Leave(object sender, EventArgs e)
        {
            WarningUserNameLabel.Visible = false;
        }

        private void PasswordText_Enter(object sender, EventArgs e)
        {
            WarningUserNameLabel.Visible = false;
            WarningPasswordLabel.Visible = true;
        }

        private void PasswordText_Leave(object sender, EventArgs e)
        {
            WarningPasswordLabel.Visible = false;
        }

        private void OutomaticUserNameCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            ManualUserNameLabel.Enabled = !OutomaticUserNameCheckEdit.Checked;
            if (!OutomaticUserNameCheckEdit.Checked)
                ManualUserNameText.Enabled = !CurrentStatus;
            else
                ManualUserNameText.Enabled = false;
            ManualUserNameText.Focus();
            WarningOutomaticUserName.Visible = OutomaticUserNameCheckEdit.Checked;
            OutomaticUserNameComboBoxEdit.Enabled = OutomaticUserNameCheckEdit.Checked;
        }

        private void SurnameText_Leave(object sender, EventArgs e)
        {
            string surname = SurnameText.Text.ToLower();
            surname = surname.Replace("ə", "e");
            surname = surname.Replace("ö", "o");
            surname = surname.Replace("ğ", "g");
            surname = surname.Replace("ı", "i");
            surname = surname.Replace("ü", "u");
            surname = surname.Replace("ş", "s");
            surname = surname.Replace("ç", "c");

            string name = NameText.Text.ToLower();
            name = name.Replace("ə", "e");
            name = name.Replace("ö", "o");
            name = name.Replace("ğ", "g");
            name = name.Replace("ı", "i");
            name = name.Replace("ü", "u");
            name = name.Replace("ş", "s");
            name = name.Replace("ç", "c");

            OutomaticUserNameComboBoxEdit.Properties.Items.Clear();
            OutomaticUserNameComboBoxEdit.Properties.Items.Add(RegistrationIDText.Text + surname + name);
            OutomaticUserNameComboBoxEdit.Properties.Items.Add(surname + name + RegistrationIDText.Text);
            OutomaticUserNameComboBoxEdit.SelectedIndex = 0;
        }

        private void OutomaticUserNameComboBoxEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                OutomaticUserNameComboBoxEdit.Properties.Items.Clear();
                OutomaticUserNameComboBoxEdit.Properties.Items.Add(RegistrationIDText.Text + SurnameText.Text.ToLower() + NameText.Text.ToLower());
                OutomaticUserNameComboBoxEdit.Properties.Items.Add(SurnameText.Text.ToLower() + NameText.Text.ToLower() + RegistrationIDText.Text);
                OutomaticUserNameComboBoxEdit.SelectedIndex = 0;
            }
        }

        private bool ControlUserDetails()
        {
            bool b = false;

            if (SurnameText.Text.Length == 0)
            {
                SurnameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçinin soyadı daxil edilməyib.");
                SurnameText.Focus();
                SurnameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçinin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PatronymicText.Text.Length == 0)
            {
                PatronymicText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçinin atasının adı daxil edilməyib.");
                PatronymicText.Focus();
                PatronymicText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SexLookUp.EditValue == null)
            {
                SexLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçinin cinsi daxil edilməyib.");
                SexLookUp.Focus();
                SexLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(BirthdayDate.Text))
            {
                BirthdayDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçinin doğum günü daxil edilməyib.");
                BirthdayDate.Focus();
                BirthdayDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BirthdayDate.DateTime == DateTime.Today)
            {
                BirthdayDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçinin doğum günü cari tarix ola bilməz.");
                BirthdayDate.Focus();
                BirthdayDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (OutomaticUserNameCheckEdit.Checked)
            {
                if (String.IsNullOrEmpty(OutomaticUserNameComboBoxEdit.Text))
                {
                    UserTabControl.SelectedTabPageIndex = 0;
                    GlobalProcedures.ShowErrorMessage("Sistemə qoşulmaq üçün istifadəçi adı daxil edilməyib.");
                    return false;
                }
                else
                    b = true;
            }
            else
            {
                if (String.IsNullOrEmpty(ManualUserNameText.Text))
                {
                    UserTabControl.SelectedTabPageIndex = 0;
                    ManualUserNameText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("Sistemə qoşulmaq üçün istifadəçi adı daxil edilməyib.");
                    ManualUserNameText.Focus();
                    ManualUserNameText.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(ManualUserNameText.Text, "^[a-zA-Z0-9]+$"))
                {
                    UserTabControl.SelectedTabPageIndex = 0;
                    ManualUserNameText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage("İstifadəçi adı yalnız ingilis hərflərindən və ya rəqəmdən ibarət olmalıdır");
                    ManualUserNameText.Focus();
                    ManualUserNameText.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CRS_USERS WHERE NIKNAME = '{GlobalFunctions.Encrypt(ManualUserNameText.Text)}'") > 0)
                {
                    UserTabControl.SelectedTabPageIndex = 0;
                    ManualUserNameText.BackColor = Color.Red;
                    GlobalProcedures.ShowErrorMessage(ManualUserNameText.Text + " istifadəçi adı artıq bazaya daxil edilib. İstifadəçi adını təkrar olaraq daxil edilə bilməz.");
                    ManualUserNameText.Focus();
                    ManualUserNameText.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
                else
                    b = true;
            }

            if (String.IsNullOrEmpty(PasswordText.Text))
            {
                UserTabControl.SelectedTabPageIndex = 0;
                PasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sistemə qoşulmaq üçün şifrə daxil edilməyib.");
                PasswordText.Focus();
                PasswordText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else if (PasswordText.Text.Length < 6)
            {
                UserTabControl.SelectedTabPageIndex = 0;
                PasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şifrənin uzunluğu 6 simvoldan az ola bilməz.");
                PasswordText.Focus();
                PasswordText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!System.Text.RegularExpressions.Regex.IsMatch(PasswordText.Text, "^[a-zA-Z0-9]+$"))
            {
                UserTabControl.SelectedTabPageIndex = 0;
                PasswordText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şifrə yalnız ingilis hərflərindən və ya rəqəmdən ibarət olmalıdır");
                PasswordText.Focus();
                PasswordText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (permissiondetails && PermissionGridView.RowCount == 0)
            {
                UserTabControl.SelectedTabPageIndex = 1;
                GroupNameLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İstifadəçiyə heç bir hüquq verilməyib.");
                GroupNameLookUp.Focus();
                GroupNameLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!maildetails)
                LoadMailDataGridView();

            if (MailGridView.RowCount == 0)
            {
                UserTabControl.SelectedTabPageIndex = 2;
                GlobalProcedures.ShowErrorMessage("İstifadəçi üçün ən azı bir elektron ünvan daxil edilməli və mail göndərmək üçün ən azı bir elektron ünvan seçilməlidir.");
                return false;
            }
            else
                b = true;

            if (mail_selected_count == 0)
            {
                UserTabControl.SelectedTabPageIndex = 2;
                GlobalProcedures.ShowErrorMessage("Mail göndərmək üçün ən azı bir elektron ünvan seçilməlidir.");
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertUser()
        {
            status_id = 1;

            if (OutomaticUserNameCheckEdit.Checked)
            {
                usernikname = GlobalFunctions.Encrypt(OutomaticUserNameComboBoxEdit.Text.Trim());
                userniknametype = "1";
            }
            else
            {
                usernikname = GlobalFunctions.Encrypt(ManualUserNameText.Text.Trim());
                userniknametype = "2";
            }

            string sql = $@"INSERT INTO CRS_USER.CRS_USERS(
                                                            ID,
                                                            NAME,
                                                            SURNAME,
                                                            PATRONYMIC,
                                                            NIKNAME_TYPE,
                                                            NIKNAME,
                                                            PASSWORD,
                                                            STATUS_ID,
                                                            BIRTHDAY,
                                                            SEX_ID,
                                                            ADDRESS,
                                                            GROUP_ID,
                                                            NOTE
                                                          ) 
                            VALUES(
                                    {UserID},
                                    '{GlobalFunctions.FirstCharToUpper(NameText.Text.Trim())}',
                                    '{GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim())}',
                                    '{GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim())}',
                                    {userniknametype},
                                    '{usernikname}','{GlobalFunctions.Encrypt(PasswordText.Text.Trim())}',
                                    {status_id},
                                    TO_DATE('{BirthdayDate.Text}','DD/MM/YYYY'),
                                    {sex_id},
                                    '{AddressText.Text.Trim()}',
                                    {group_id},
                                    '{NoteText.Text.Trim()}')";
            GlobalFunctions.ExecuteQuery(sql, "İstifadəçinin məlumatları bazaya daxil edilmədi.");
        }

        private void InsertUserImage()
        {
            GlobalFunctions.ExecuteQueryWithBlob(@"INSERT INTO CRS_USER.USER_IMAGE(USER_ID,IMAGE)VALUES(" + UserID + ",:BlobFile)", UserImage, "İstifadəçinin şəkli sistemə daxil edilmədi.");
        }

        private void UpdateUserImage()
        {
            GlobalFunctions.ExecuteQueryWithBlob($@"UPDATE CRS_USER.USER_IMAGE SET IMAGE = :BlobFile WHERE USER_ID = {UserID}", UserImage, "İstifadəçinin şəkli dəyişdirilmədi.");
        }

        private void UppdateUser()
        {
            if (OutomaticUserNameCheckEdit.Checked)
            {
                usernikname = GlobalFunctions.Encrypt(OutomaticUserNameComboBoxEdit.Text.Trim());
                userniknametype = "1";
            }
            else
            {
                usernikname = GlobalFunctions.Encrypt(ManualUserNameText.Text.Trim());
                userniknametype = "2";
            }

            string sql = null;            

            if (status_id == 1)
            {
                sql = $@"UPDATE CRS_USER.CRS_USERS SET 
                                                NAME = '{GlobalFunctions.FirstCharToUpper(NameText.Text.Trim())}',
                                                SURNAME = '{GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim())}',
                                                PATRONYMIC = '{GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim())}',
                                                NIKNAME_TYPE = {userniknametype},
                                                NIKNAME = '{usernikname.Trim()}',
                                                PASSWORD = '{GlobalFunctions.Encrypt(PasswordText.Text.Trim())}',
                                                BIRTHDAY = TO_DATE('{BirthdayDate.Text.Trim()}','DD/MM/YYYY'),
                                                SEX_ID = {sex_id},
                                                NOTE = '{NoteText.Text}',
                                                ADDRESS = '{AddressText.Text}',
                                                GROUP_ID = {group_id}
                                        WHERE ID = {UserID}";
            }
            else
            {
                sql = $@"UPDATE CRS_USER.CRS_USERS SET 
                                                STATUS_ID = {status_id},
                                                CLOSED_DATE = TO_DATE('{CloseDateValue.Text}','DD/MM/YYYY'),
                                                NOTE = '{NoteText.Text}' 
                            WHERE ID = {UserID}";
            }

            GlobalProcedures.ExecuteQuery(sql, "İstifadəçinin məlumatları bazada dəyişdirilmədi.");
        }

        private void LoadUserGroupPermissionDataGridView()
        {
            string s = $@"SELECT R.ROLE_DESCRIPTION, RD.DETAIL_NAME_AZ DETAIL_NAME, RD.ID
                                  FROM CRS_USER.ALL_USER_GROUP_ROLE_DETAILS RDT,
                                       CRS_USER.ROLES R,
                                       CRS_USER.ALL_ROLE_DETAILS RD
                                 WHERE     RD.ID = RDT.ROLE_DETAIL_ID
                                       AND R.ROLE_ID = RD.ROLE_ID
                                       AND RDT.GROUP_ID = {group_id}";
            PermissionGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
        }

        private void DeleteAllTemp()
        {
            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER_TEMP.PROC_USER_DELETE_ALL_TEMP", "P_USED_USER_ID", GlobalVariables.V_UserID, "İstifadəçinin məlumatları temp cədvəldən silinmədi.");
        }

        private void FUserAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CRS_USERS", -1, "WHERE ID = " + UserID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.DeleteAllFilesInDirectory(UserImagePath);
            DeleteAllTemp();
            this.RefreshUserDataGridView();
        }

        private void LoadPhoneDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 P.ID,
                                 PD.DESCRIPTION_AZ DESCRIPTION,
                                 '+' || C.CODE || P.PHONE_NUMBER PHONENUMBER,
                                 P.IS_SEND_SMS
                            FROM CRS_USER_TEMP.PHONES_TEMP P,
                                 CRS_USER.PHONE_DESCRIPTIONS PD,
                                 CRS_USER.COUNTRIES C
                           WHERE     P.IS_CHANGE IN (0, 1)
                                 AND P.PHONE_DESCRIPTION_ID = PD.ID
                                 AND P.COUNTRY_ID = C.ID
                                 AND P.OWNER_TYPE = 'U'
                                 AND P.OWNER_ID = {UserID}
                        ORDER BY P.ORDER_ID";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
                if (PhoneGridView.RowCount > 0)
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled = true;
                else
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled =
                        UpPhoneBarButton.Enabled =
                        DownPhoneBarButton.Enabled = false;
                try
                {
                    PhoneGridView.BeginUpdate();
                    for (int i = 0; i < PhoneGridView.RowCount; i++)
                    {
                        DataRow row = PhoneGridView.GetDataRow(PhoneGridView.GetVisibleRowHandle(i));
                        if (Convert.ToInt32(row["IS_SEND_SMS"].ToString()) == 1)
                            PhoneGridView.SelectRow(i);
                    }
                }
                finally
                {
                    PhoneGridView.EndUpdate();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Telefon nömrələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void LoadMailDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                   ID,
                                   MAIL,
                                   NOTE,
                                   IS_SEND
                              FROM CRS_USER_TEMP.MAILS_TEMP
                             WHERE IS_CHANGE IN (0, 1) AND OWNER_TYPE = 'U' AND OWNER_ID = {UserID}";
            try
            {
                MailGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
                EditMailBarButton.Enabled = DeleteMailBarButton.Enabled = (MailGridView.RowCount > 0);
                try
                {
                    MailGridView.BeginUpdate();
                    for (int i = 0; i < MailGridView.RowCount; i++)
                    {
                        DataRow row = MailGridView.GetDataRow(MailGridView.GetVisibleRowHandle(i));
                        if (Convert.ToInt32(row["IS_SEND"].ToString()) == 1)
                            MailGridView.SelectRow(i);
                    }
                }
                finally
                {
                    MailGridView.EndUpdate();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Maillər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void UpdatePhoneSendSms()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0 WHERE IS_SEND_SMS = 1 AND OWNER_TYPE = 'U' AND OWNER_ID = {UserID} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Telefon nömrələri dəyişdirilmədi.");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PhoneGridView.SelectedRowsCount; i++)
            {
                rows.Add(PhoneGridView.GetDataRow(PhoneGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 1 WHERE ID = {row["ID"]} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.");
            }

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'U' AND OWNER_ID = {UserID} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Telefon nömrələri dəyişdirilmədi.");
        }

        private void UpdateMailSend()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 0 WHERE IS_SEND = 1 AND OWNER_TYPE = 'U' AND OWNER_ID = {UserID} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Maillər dəyişdirilmədi.");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < MailGridView.SelectedRowsCount; i++)
            {
                rows.Add(MailGridView.GetDataRow(MailGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 1 WHERE ID = {row["ID"]} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.");
                mail_selected_count++;
            }

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'U' AND OWNER_ID = {UserID} AND USED_USER_ID = {GlobalVariables.V_UserID}", "Maillər dəyişdirilmədi.");
        }

        private void InsertTemps()
        {
            if (TransactionName == "INSERT")
                return;

            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_USER_TEMP", "P_USER_ID", UserID, "İstifadəçinin məlumatları temp cədvələ daxil edilmədi.");
        }

        private void BirthdayDate_EditValueChanged(object sender, EventArgs e)
        {
            AgeLabel.Text = GlobalFunctions.CalculationAgeWithYear(BirthdayDate.DateTime, DateTime.Today);
        }

        private void InsertUserGroupPermission()
        {
            if (permissiondetails)
                GlobalProcedures.ExecuteTwoQuery("DELETE FROM CRS_USER.USER_GROUP_PERMISSION WHERE USER_ID = " + UserID,
                                                        "INSERT INTO CRS_USER.USER_GROUP_PERMISSION(ID,USER_ID,GROUP_ID) VALUES(USER_GROUP_ROLE_SEQUENCE.NEXTVAL," + UserID + "," + group_id + ")",
                                                    "Qrupun hüquqları istifadəçiyə verilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlUserDetails())
            {
                if (TransactionName == "INSERT")
                {
                    InsertUser();
                    InsertUserImage();
                }
                else
                {
                    UppdateUser();
                    UpdateUserImage();
                }
                InsertUserGroupPermission();
                UpdatePhoneSendSms();
                UpdateMailSend();
                InsertUserDetails();
                this.Close();
            }
        }

        private void InsertUserDetails()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_USER_DETAILS", "P_CUSTOMER_ID", UserID, "İstifadəçinin temp məlumatları əsas cədvələ daxil edilmədi.");
        }

        private void ShowPasswordCheck_CheckedChanged(object sender, EventArgs e)
        {
            PasswordText.Properties.UseSystemPasswordChar = !ShowPasswordCheck.Checked;
        }

        void RefreshPhone()
        {
            LoadPhoneDataGridView();
        }

        public void LoadFPhoneAddEdit(string transaction, string OwnerID, string OwnerType, string PhoneID)
        {
            FPhoneAddEdit fp = new FPhoneAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerID = OwnerID;
            fp.OwnerType = OwnerType;
            fp.PhoneID = PhoneID;
            fp.RefreshPhonesDataGridView += new FPhoneAddEdit.DoEvent(RefreshPhone);
            fp.ShowDialog();
        }

        private void NewPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("INSERT", UserID, "U", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", UserID, "U", PhoneID);
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled && PhoneStandaloneBarDockControl.Enabled)
                LoadFPhoneAddEdit("EDIT", UserID, "U", PhoneID);
        }

        private void PhoneGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PhoneGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PhoneGridView.GetFocusedDataRow();
            if (row != null)
            {
                PhoneID = row["ID"].ToString();
                PhoneNumber = row["PHONENUMBER"].ToString();
                UpPhoneBarButton.Enabled = !(PhoneGridView.FocusedRowHandle == 0);
                DownPhoneBarButton.Enabled = !(PhoneGridView.FocusedRowHandle == PhoneGridView.RowCount - 1);
            }
        }

        private void DeletePhone()
        {
            DialogResult dialogResult = XtraMessageBox.Show(PhoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'U' AND OWNER_ID = " + UserID + " AND ID = " + PhoneID, "Telefon nömrəsi temp cədvəldən silinmədi.");
            }
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
            LoadPhoneDataGridView();
        }

        private void RefreshPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDataGridView();
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PhonePopupMenu, e);
        }

        void RefreshMail()
        {
            LoadMailDataGridView();
        }

        public void LoadFMailAddEdit(string transaction, string OwnerID, string OwnerType, string MailID)
        {
            FMailAddEdit fp = new FMailAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerID = OwnerID;
            fp.OwnerType = OwnerType;
            fp.MailID = MailID;
            fp.RefreshEmailDataGridView += new FMailAddEdit.DoEvent(RefreshMail);
            fp.ShowDialog();
        }

        private void UpPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "up", out orderid);
            LoadPhoneDataGridView();
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void DownPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int orderid;
            GlobalProcedures.ChangeOrderIDforTEMP("PHONES_TEMP", PhoneID, "down", out orderid);
            LoadPhoneDataGridView();
            PhoneGridView.FocusedRowHandle = orderid - 1;
        }

        private void StatusLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 1, "WHERE ID IN (1, 2)");
        }

        private void StatusLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (StatusLookUp.EditValue == null)
                return;

            status_id = Convert.ToInt32(StatusLookUp.EditValue);

            if (status_id == 2)
            {
                CloseDateLabel.Visible = CloseDateValue.Visible = true;
                CloseDateValue.EditValue = DateTime.Today;
                SurnameText.Enabled =
                    NameText.Enabled =
                    PatronymicText.Enabled =
                    SexLookUp.Enabled =
                    BirthdayDate.Enabled =
                    AddressText.Enabled =
                    PhoneStandaloneBarDockControl.Enabled =
                    MailStandaloneBarDockControl.Enabled =
                    GroupNameLookUp.Enabled =
                    OutomaticUserNameCheckEdit.Enabled =
                    OutomaticUserNameComboBoxEdit.Enabled =
                    ManualUserNameText.Enabled =
                    PasswordText.Enabled =
                    BLoadPicture.Enabled =
                    BDeletePicture.Enabled =
                    ShowPasswordCheck.Enabled = false;
                PhonePopupMenu.Manager = MailPopupMenu.Manager = null;
            }
            else
            {
                CloseDateLabel.Visible = CloseDateValue.Visible = false;
                SurnameText.Enabled =
                    NameText.Enabled =
                    PatronymicText.Enabled =
                    SexLookUp.Enabled =
                    BirthdayDate.Enabled =
                    AddressText.Enabled =
                    PhoneStandaloneBarDockControl.Enabled =
                    MailStandaloneBarDockControl.Enabled =
                    GroupNameLookUp.Enabled =
                    OutomaticUserNameCheckEdit.Enabled =
                    OutomaticUserNameComboBoxEdit.Enabled =
                    ManualUserNameText.Enabled =
                    PasswordText.Enabled =
                    BLoadPicture.Enabled =
                    ShowPasswordCheck.Enabled = true;
                PhonePopupMenu.Manager = PhoneBarManager;
                MailPopupMenu.Manager = MailBarManager;
            }
        }

        private void GroupNameLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FUsersGroups fug = new FUsersGroups();
                fug.ShowDialog();
                GlobalProcedures.FillLookUpEdit(GroupNameLookUp, "USER_GROUP", "ID", "GROUP_NAME", null);
            }
        }

        private void GroupNameLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (GroupNameLookUp.EditValue == null)
                return;

            group_id = Convert.ToInt32(GroupNameLookUp.EditValue);
            LoadUserGroupPermissionDataGridView();
        }

        private void SexLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SexLookUp.EditValue == null)
                return;

            sex_id = Convert.ToInt32(SexLookUp.EditValue);
        }

        private void NewMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("INSERT", UserID, "U", null);
        }

        private void EditMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("EDIT", UserID, "U", MailID);
        }

        private void MailGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditMailBarButton.Enabled && MailStandaloneBarDockControl.Enabled)
                LoadFMailAddEdit("EDIT", UserID, "U", MailID);
        }

        private void DeleteMail()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş elektron ünvanları silmək istəyirsiniz?", "Elektron ünvanların silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'U' AND OWNER_ID = " + UserID + " AND ID = " + MailID, "Elektron ünvanlar temp cədvəldən silinmədi.");
            }
        }

        private void DeleteMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteMail();
            LoadMailDataGridView();
        }

        private void RefreshMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadMailDataGridView();
        }

        private void MailGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(MailGridView, MailPopupMenu, e);
        }

        private void MailGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = MailGridView.GetFocusedDataRow();
            if (row != null)
                MailID = row["ID"].ToString();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 1:
                    GlobalProcedures.FillLookUpEdit(StatusLookUp, "STATUS", "ID", "STATUS_NAME", "ID IN (1, 2)");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index, string where)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.StatusWhere = where;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }
                
        void SelectionImage(string a, int count)
        {
            if (!String.IsNullOrEmpty(a) && File.Exists(a))
            {
                UserPictureBox.Image = Image.FromFile(a);
                UserImage = a;
                BDeletePicture.Enabled = true;
            }
            crop_image_count = count;
        }

        private void UserPictureBox_DoubleClick(object sender, EventArgs e)
        {
            UserPictureBox.Image = null;
            FImageCrop crop = new FImageCrop();
            crop.PictureOwner = "C" + UserID.ToString();
            crop.count = crop_image_count;
            crop.SelectionImage += new FImageCrop.DoEvent(SelectionImage);
            crop.ShowDialog();
        }

        private void MailGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < MailGridView.SelectedRowsCount; i++)
            {
                rows.Add(MailGridView.GetDataRow(MailGridView.GetSelectedRows()[i]));
            }
            mail_selected_count = rows.Count;
        }

        private void UserTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            switch (UserTabControl.SelectedTabPageIndex)
            {
                case 1:
                    {
                        permissiondetails = true;
                        GroupNameLookUp.EditValue = GroupNameLookUp.Properties.GetKeyValueByDisplayText(group_name);
                    }
                    break;
                case 2:
                    {
                        LoadPhoneDataGridView();
                        LoadMailDataGridView();
                    }
                    break;
            }
        }

        private void MailGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditMailBarButton.Enabled = DeleteMailBarButton.Enabled = (MailGridView.RowCount > 0);
        }

        private void PhoneGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = (PhoneGridView.RowCount > 0);
        }
    }
}