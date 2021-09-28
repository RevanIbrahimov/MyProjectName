using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using DevExpress.Utils;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS.Forms.Dictionaries
{
    public partial class FPersonnelAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPersonnelAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, PersonnelID;
        string PersonnelImage, PersonnelImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images", PhoneID, PhoneNumber, MailID, CardID, OwnerType = "P";
        int PersonnelUsedUserID = -1, birthplace_id, sex_id, status_id, crop_image_count = 0, positionID, topindex, old_row_num;
        bool PersonnelUsed = false, CurrentStatus = false, PersonnelClosed = false;

        public delegate void DoEvent();
        public event DoEvent RefreshPersonnelDataGridView;

        private void FPersonnelAddEdit_Load(object sender, EventArgs e)
        {                   
            BirthdayDate.EditValue = DateTime.Today;
            BirthdayDate.Properties.MaxValue = DateTime.Today;

            GlobalProcedures.FillComboBoxEdit(SexComboBox, "SEX", "NAME,NAME_EN,NAME_RU", null);
            GlobalProcedures.FillComboBoxEdit(BirthplaceComboBox, "BIRTHPLACE", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PERSONNEL", GlobalVariables.V_UserID, "WHERE ID = " + PersonnelID + " AND USED_USER_ID = -1");
                PersonnelUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.PERSONNEL WHERE ID = {PersonnelID}");
                PersonnelUsed = (PersonnelUsedUserID >= 0);                

                if (GlobalFunctions.GetID($@"SELECT STATUS_ID FROM CRS_USER.PERSONNEL WHERE ID = {PersonnelID}") == 16)
                    PersonnelClosed = true;
                else
                    PersonnelClosed = false;

                if (((PersonnelClosed) && (PersonnelUsed)) || ((PersonnelClosed) && (!PersonnelUsed)))
                {
                    XtraMessageBox.Show("Seçilmiş işçinin statusu bağlı olduğu üçün onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş işçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else if ((!PersonnelClosed) && (PersonnelUsed))
                {
                    if (GlobalVariables.V_UserID != PersonnelUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == PersonnelUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş işçinin məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş işçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnable(CurrentStatus);
                StatusComboBox.Visible = true;
                StatusLabel.Visible = true;
                InsertPhonesTemp();
                InsertMailTemp();
                InsertPersonnelTemp();
                GlobalProcedures.FillComboBoxEdit(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (15,16) ORDER BY ID");
                LoadPersonnelDetails();
            }
            else
            {
                PersonnelID = GlobalFunctions.GetOracleSequenceValue("PERSONNEL_SEQUENCE").ToString();
                switch (GlobalVariables.SelectedLanguage)
                {                    
                    case "RU": PersonnelPictureBox.Properties.NullText = "Фотография работник";
                        break;
                    case "EN": PersonnelPictureBox.Properties.NullText = "Personnel picture";
                        break;
                }
            }
            LoadCardsDataGridView();
        }

        public void ComponentEnable(bool status)
        {
            SurnameText.Enabled = 
                NameText.Enabled = 
                PatronymicText.Enabled = 
                SexComboBox.Enabled = 
                NoteText.Enabled = 
                BirthdayDate.Enabled = 
                BirthplaceComboBox.Enabled = 
                BLoadPicture.Enabled = 
                BDeletePicture.Enabled = 
                PhoneStandaloneBarDockControl.Enabled = 
                MailStandaloneBarDockControl.Enabled = 
                PhoneStandaloneBarDockControl.Enabled = 
                MailStandaloneBarDockControl.Enabled = 
                CardStandaloneBarDockControl.Enabled = 
                BOK.Visible = 
                NoteText.Enabled = !status;
            if (status == false)
            {
                PhonePopupMenu.Manager = PhoneBarManager;
                MailPopupMenu.Manager = MailBarManager;
                CardsPopupMenu.Manager = CardsBarManager;
            }
            else            
                PhonePopupMenu.Manager = 
                    MailPopupMenu.Manager = 
                    CardsPopupMenu.Manager = null;            
        }

        private void LoadPersonnelDetails()
        {
            string s = $@"SELECT C.ID,
                                   C.SURNAME,
                                   C.NAME,
                                   C.PATRONYMIC,
                                   S.NAME,
                                   S.NAME_EN,
                                   S.NAME_RU,
                                   TO_CHAR (C.BIRTHDAY, 'DD.MM.YYYY') BIRTHDAY,
                                   B.NAME BIRTHPLACE_NAME,
                                   C.NOTE,
                                   ST.ID STATUS_ID,
                                   ST.STATUS_NAME,
                                   ST.STATUS_NAME_EN,
                                   ST.STATUS_NAME_RU,
                                   TO_CHAR (C.CLOSED_DATE, 'DD.MM.YYYY'),
                                   C.IMAGE
                              FROM CRS_USER.PERSONNEL C,
                                   CRS_USER.BIRTHPLACE B,
                                   CRS_USER.SEX S,
                                   CRS_USER.STATUS ST
                             WHERE     C.STATUS_ID = ST.ID
                                   AND C.BIRTHPLACE_ID = B.ID
                                   AND C.SEX_ID = S.ID
                                   AND C.ID = {PersonnelID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPersonnelDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    SurnameText.Text = dr[1].ToString();
                    NameText.Text = dr[2].ToString();
                    PatronymicText.Text = dr[3].ToString();

                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ": SexComboBox.EditValue = dr[4].ToString();
                            break;
                        case "EN": SexComboBox.EditValue = dr[5].ToString();
                            break;
                        case "RU": SexComboBox.EditValue = dr[6].ToString();
                            break;
                    }
                    BirthdayDate.EditValue = GlobalFunctions.ChangeStringToDate(dr[7].ToString(), "ddmmyyyy");
                    BirthplaceComboBox.EditValue = dr[8].ToString();

                    NoteText.Text = dr[9].ToString();                    
                    
                    status_id = Convert.ToInt32(dr[10].ToString());
                    if (status_id == 16)
                    {
                        StatusComboBox.Enabled = false;
                        CloseDateValue.Enabled = false;
                        BOK.Enabled = false;
                        NoteText.Enabled = false;
                        BLoadPicture.Enabled = false;
                        BDeletePicture.Enabled = false;
                    }
                    switch (GlobalVariables.SelectedLanguage)
                    {
                        case "AZ": StatusComboBox.EditValue = dr[11].ToString();
                            break;
                        case "EN": StatusComboBox.EditValue = dr[12].ToString();
                            break;
                        case "RU": StatusComboBox.EditValue = dr[13].ToString();
                            break;
                    }

                    if (String.IsNullOrEmpty(dr[14].ToString()))
                        CloseDateValue.EditValue = DateTime.Today;
                    else
                        CloseDateValue.EditValue = GlobalFunctions.ChangeStringToDate(dr[15].ToString(), "ddmmyyyy");

                    if (!DBNull.Value.Equals(dr["IMAGE"]))
                    {
                        Byte[] BLOBData = (byte[])dr["IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        PersonnelPictureBox.Image = Image.FromStream(stmBLOBData);

                        if (!Directory.Exists(PersonnelImagePath))
                        {
                            Directory.CreateDirectory(PersonnelImagePath);
                        }

                        GlobalProcedures.DeleteFile(PersonnelImagePath + "\\P_" + dr["ID"] + ".jpeg");
                        FileStream fs = new FileStream(PersonnelImagePath + "\\P_" + dr["ID"] + ".jpeg", FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(fs);
                        fs.Close();
                        stmBLOBData.Close();
                        PersonnelImage = PersonnelImagePath + "\\P_" + dr["ID"] + ".jpeg";
                        BLoadPicture.Text = "Dəyiş";
                        BDeletePicture.Enabled = true;
                    }
                    else
                    {
                        BLoadPicture.Text = "Yüklə";
                        BDeletePicture.Enabled = false;
                        switch (GlobalVariables.SelectedLanguage)
                        {                            
                            case "RU":
                                PersonnelPictureBox.Properties.NullText = "Фотография работник";
                                break;
                            case "EN":
                                PersonnelPictureBox.Properties.NullText = "Personnel picture";
                                break;
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("İşçinin parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }            
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BLoadPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "İşçinin şəkilini seçin";
                dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PersonnelPictureBox.Image = new Bitmap(dlg.FileName);
                    PersonnelImage = dlg.FileName;
                    BDeletePicture.Enabled = true;
                }
                dlg.Dispose();
            }
        }

        private void BDeletePicture_Click(object sender, EventArgs e)
        {
            PersonnelPictureBox.Image = null;
            PersonnelImage = null;            
            BLoadPicture.Text = "Yüklə";
            BDeletePicture.Enabled = false;
        }

        private void CardsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }        

        private void CardsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CardsGridView, CardsPopupMenu, e);
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PhonePopupMenu, e);
        }

        private void MailGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(MailGridView, MailPopupMenu, e);
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 1: GlobalProcedures.FillComboBoxEdit(StatusComboBox, "STATUS", "STATUS_NAME,STATUS_NAME_EN,STATUS_NAME_RU", "ID IN (15, 16)");
                    break;
                case 4:
                    GlobalProcedures.FillComboBoxEdit(BirthplaceComboBox, "BIRTHPLACE", "NAME,NAME,NAME", "1 = 1 ORDER BY ORDER_ID");
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

        private void BirthplaceComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 4, null);
        }

        private void PositionComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 16, null);
        }

        private void LoadPhoneDataGridView()
        {
            string s = $@"SELECT 1 SS,P.ID,PD.DESCRIPTION_AZ,'+'||C.CODE||P.PHONE_NUMBER PHONENUMBER,P.IS_SEND_SMS FROM CRS_USER_TEMP.PHONES_TEMP P,CRS_USER.PHONE_DESCRIPTIONS PD,CRS_USER.COUNTRIES C WHERE P.IS_CHANGE IN (0,1) AND P.PHONE_DESCRIPTION_ID = PD.ID AND P.COUNTRY_ID = C.ID AND P.OWNER_TYPE = 'P' AND P.OWNER_ID = {PersonnelID}";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");
                PhoneGridView.PopulateColumns();
                PhoneGridView.Columns[0].Caption = "S/s";
                PhoneGridView.Columns[1].Visible = false;
                PhoneGridView.Columns[2].Caption = "Təsvir";
                PhoneGridView.Columns[3].Caption = "Nömrə";
                PhoneGridView.Columns[4].Visible = false;

                for (int i = 0; i < PhoneGridView.Columns.Count; i++)
                {
                    PhoneGridView.Columns[i].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    PhoneGridView.Columns[i].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                    PhoneGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    PhoneGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                }

                PhoneGridView.BestFitColumns();

                if (PhoneGridView.RowCount > 0)
                {
                    EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = true;
                    PhoneGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");                  
                }
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
            string s = "SELECT 1 SS,ID,MAIL,NOTE,IS_SEND FROM CRS_USER_TEMP.MAILS_TEMP WHERE IS_CHANGE IN (0,1) AND OWNER_TYPE = 'P' AND OWNER_ID = " + PersonnelID;
            try
            {
                MailGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadMailDataGridView");
                MailGridView.PopulateColumns();
                MailGridView.Columns[0].Caption = "S/s";
                MailGridView.Columns[1].Visible = false;
                MailGridView.Columns[2].Caption = "Mail";
                MailGridView.Columns[3].Caption = "Qeyd";
                MailGridView.Columns[4].Visible = false;

                MailGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                MailGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                for (int i = 0; i < MailGridView.Columns.Count; i++)
                {
                    MailGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    MailGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                }

                MailGridView.BestFitColumns();

                if (MailGridView.RowCount > 0)
                {
                    EditMailBarButton.Enabled = true;
                    DeleteMailBarButton.Enabled = true;
                    MailGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                {
                    EditMailBarButton.Enabled = false;
                    DeleteMailBarButton.Enabled = false;
                }

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
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0 WHERE IS_SEND_SMS = 1 AND OWNER_TYPE = '{OwnerType}' AND OWNER_ID = {PersonnelID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Telefon nömrələri dəyişdirilmədi.",
                                             this.Name + "/UpdatePhoneSendSms");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PhoneGridView.SelectedRowsCount; i++)
            {
                rows.Add(PhoneGridView.GetDataRow(PhoneGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 1 WHERE ID = {row["ID"]} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                                this.Name + "/UpdatePhoneSendSms");
            }

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = '{OwnerType}' AND OWNER_ID = {PersonnelID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Telefon nömrələri dəyişdirilmədi.",
                                          this.Name + "/UpdatePhoneSendSms");
        }

        private void UpdateMailSend()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 0 WHERE IS_SEND = 1 AND OWNER_TYPE = '{OwnerType}' AND OWNER_ID = {PersonnelID} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
                                            "Maillər dəyişdirilmədi.",
                                          this.Name + "/UpdateMailSend");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < MailGridView.SelectedRowsCount; i++)
            {
                rows.Add(MailGridView.GetDataRow(MailGridView.GetSelectedRows()[i]));
            }

            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 1 WHERE ID = {row["ID"]} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
                                                        "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                               this.Name + "/UpdateMailSend");
            }

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = '{OwnerType}' AND OWNER_ID = {PersonnelID} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
                                            "Maillər dəyişdirilmədi.",
                                            this.Name + "/UpdateMailSend");
        }

        private void InsertPhonesTemp()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_INSERT_PHONE_TEMP", "P_OWNER_ID", int.Parse(PersonnelID), "P_OWNER_TYPE", OwnerType, "Telefonlar temp cədvələ daxil edilmədi.");            
        }

        private void InsertMailTemp()
        {
            GlobalProcedures.ExecuteProcedureWithTwoParametrAndUser("CRS_USER.PROC_INSERT_MAIL_TEMP", "P_OWNER_ID", int.Parse(PersonnelID), "P_OWNER_TYPE", OwnerType, "Maillər temp cədvələ daxil edilmədi.");           
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
            LoadFPhoneAddEdit("INSERT", PersonnelID, "P", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", PersonnelID, "P", PhoneID);
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
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = '{OwnerType}' AND OWNER_ID = {PersonnelID} AND ID = {PhoneID}", "Telefon nömrəsi temp cədvəldən silinmədi.");
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

        private void MailGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = MailGridView.GetFocusedDataRow();
            if (row != null)
                MailID = row["ID"].ToString();
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

        private void NewMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("INSERT", PersonnelID, "P", null);
        }

        private void EditMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("EDIT", PersonnelID, "P", MailID);
        }

        private void DeleteMail()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş elektron ünvanları silmək istəyirsiniz?", "Elektron ünvanların silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = '{OwnerType}' AND OWNER_ID = {PersonnelID} AND ID = {MailID}", "Elektron ünvanlar temp cədvəldən silinmədi.");
            }
        }

        private void MailGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditMailBarButton.Enabled) && (MailStandaloneBarDockControl.Enabled))
                LoadFMailAddEdit("EDIT", PersonnelID, "P", MailID);
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled && PhoneStandaloneBarDockControl.Enabled)
                LoadFPhoneAddEdit("EDIT", PersonnelID, "P", PhoneID);
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

        private void DeleteAllTemp()
        {
            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER_TEMP.PROC_PERSONNEL_DELETE_ALL_TEMP", "P_USED_USER_ID", GlobalVariables.V_UserID, "İşçinin məlumatları temp cədvəldən silinmədi.");            
        }

        private void FPersonnelAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PERSONNEL", -1, "WHERE ID = " + PersonnelID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID);
            DeleteAllTemp();
            GlobalProcedures.DeleteAllFilesInDirectory(PersonnelImagePath);
            this.RefreshPersonnelDataGridView();
        }

        private bool ControlPersonnelDetails()
        {
            bool b = false;

            if (SurnameText.Text.Length == 0)
            {
                SurnameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İşçinin soyadı daxil edilməyib.");
                SurnameText.Focus();
                SurnameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İşçinin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;
            if (PatronymicText.Text.Length == 0)
            {
                PatronymicText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İşçinin atasının adı daxil edilməyib.");
                PatronymicText.Focus();
                PatronymicText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BirthdayDate.DateTime >= DateTime.Today)
            {
                BirthdayDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İşçinin doğum tarixi cari tarixdən kiçik olmalıdır.");
                BirthdayDate.Focus();
                BirthdayDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(BirthplaceComboBox.Text))
            {
                BirthplaceComboBox.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İşçinin doğum yeri daxil edilməyib.");
                BirthplaceComboBox.Focus();
                BirthplaceComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;            

            //if (CardsGridView.RowCount == 0)
            //{
            //    OtherTabControl.SelectedTabPageIndex = 0;
            //    CardsGridControl.BackColor = Color.Red;
            //    GlobalProcedures.ShowErrorMessage("İşçinin şəxsiyyətini təsdiq edən sənəd daxil daxil edilməyib.");
            //    CardsGridControl.BackColor = GlobalFunctions.ElementColor();
            //    return false;
            //}
            //else
            //    b = true;

            return b;
        }

        private void InsertPersonnel()
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    if (PersonnelImage != null)
                    {
                        FileStream fls = new FileStream(PersonnelImage, FileMode.Open, FileAccess.Read);
                        byte[] blob = new byte[fls.Length];
                        fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                        fls.Close();
                        command.CommandText = "INSERT INTO CRS_USER.PERSONNEL(ID,SURNAME,NAME,PATRONYMIC,SEX_ID,IMAGE,NOTE,BIRTHDAY,"
                                                                             + "BIRTHPLACE_ID,STATUS_ID) VALUES("
                                                                            + PersonnelID + ",'" + GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim()) + "','" + GlobalFunctions.FirstCharToUpper(NameText.Text.Trim()) + "','"
                                                                            + GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim()) + "'," + sex_id + ","
                                                                            + ":BlobImage,'" + NoteText.Text.Trim() + "',TO_DATE('" + BirthdayDate.Text + "','DD/MM/YYYY')," + birthplace_id + ",15)";
                        OracleParameter blobParameter = new OracleParameter();
                        blobParameter.OracleDbType = OracleDbType.Blob;
                        blobParameter.ParameterName = "BlobImage";
                        blobParameter.Value = blob;
                        command.Parameters.Add(blobParameter);
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO CRS_USER.PERSONNEL(ID,SURNAME,NAME,PATRONYMIC,SEX_ID,NOTE,BIRTHDAY,"
                                                                             + "BIRTHPLACE_ID,STATUS_ID) VALUES("
                                                                            + PersonnelID + ",'" + GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim()) + "','" + GlobalFunctions.FirstCharToUpper(NameText.Text.Trim()) + "','"
                                                                            + GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim()) + "'," + sex_id + ",'"
                                                                            + NoteText.Text.Trim() + "',TO_DATE('" + BirthdayDate.Text + "','DD/MM/YYYY')," + birthplace_id + ",15)";
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("İşçinin məlumatları sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                    
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void UpdatePersonnel()
        {
            using (OracleConnection connection = new OracleConnection())
            {
                OracleTransaction transaction = null;
                OracleCommand command = null;             
                try
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.ConnectionString = GlobalFunctions.GetConnectionString();
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;
                    if (status_id == 15)
                    {
                        if (PersonnelImage != null)
                        {
                            FileStream fls = new FileStream(PersonnelImage, FileMode.Open, FileAccess.Read);
                            byte[] blob = new byte[fls.Length];
                            fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                            fls.Close();
                            command.CommandText = "UPDATE CRS_USER.PERSONNEL SET SURNAME = '" + GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim()) + "',NAME = '" + GlobalFunctions.FirstCharToUpper(NameText.Text.Trim()) + "'," +
                                                                            "PATRONYMIC = '" + GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim()) + "',SEX_ID = " + sex_id + ",IMAGE = :BlobImage,NOTE = '" + NoteText.Text.Trim() + "',BIRTHDAY = TO_DATE('" + BirthdayDate.Text + "','DD/MM/YYYY'),BIRTHPLACE_ID = " + birthplace_id + ", STATUS_ID = " + status_id + " WHERE ID = " + PersonnelID;
                            OracleParameter blobParameter = new OracleParameter();
                            blobParameter.OracleDbType = OracleDbType.Blob;
                            blobParameter.ParameterName = "BlobImage";
                            blobParameter.Value = blob;
                            command.Parameters.Add(blobParameter);
                        }
                        else
                        {
                            command.CommandText = "UPDATE CRS_USER.PERSONNEL SET SURNAME = '" + GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim()) + "',NAME = '" + GlobalFunctions.FirstCharToUpper(NameText.Text.Trim()) + "'," +
                                                                            "PATRONYMIC = '" + GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim()) + "',SEX_ID = " + sex_id + ",IMAGE = null,NOTE = '" + NoteText.Text.Trim() + "',BIRTHDAY = TO_DATE('" + BirthdayDate.Text + "','DD/MM/YYYY'),BIRTHPLACE_ID = " + birthplace_id + ",STATUS_ID = " + status_id + " WHERE ID = " + PersonnelID;
                        }
                    }
                    else
                        command.CommandText = "UPDATE CRS_USER.PERSONNEL SET CLOSED_DATE = TO_DATE('" + CloseDateValue.Text + "','DD/MM/YYYY'),STATUS_ID = " + status_id + " WHERE ID = " + PersonnelID;

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("İşçinin məlumatları sistemdə dəyişdirilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                    
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void BirthplaceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            birthplace_id = GlobalFunctions.FindComboBoxSelectedValue("BIRTHPLACE", "NAME", "ID", BirthplaceComboBox.Text);
        }

        private void BirthdayDate_EditValueChanged(object sender, EventArgs e)
        {
            AgeLabel.Text = GlobalFunctions.CalculationAgeWithYear(BirthdayDate.DateTime, DateTime.Today);
        }

        private void SexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ": sex_id = GlobalFunctions.FindComboBoxSelectedValue("SEX", "NAME", "ID", SexComboBox.Text);
                    break;
                case "EN": sex_id = GlobalFunctions.FindComboBoxSelectedValue("SEX", "NAME_EN", "ID", SexComboBox.Text);
                    break;
                case "RU": sex_id = GlobalFunctions.FindComboBoxSelectedValue("SEX", "NAME_RU", "ID", SexComboBox.Text);
                    break;
            }
        }

        private void LoadCardsDataGridView()
        {
            string s = $@"SELECT 1 SS,CC.ID,CS.NAME||': '||CS.SERIES||' - '||CC.CARD_NUMBER,TO_CHAR(CC.ISSUE_DATE,'DD.MM.YYYY'),CI.NAME,TO_CHAR(CC.RELIABLE_DATE,'DD.MM.YYYY'),CC.ADDRESS,CC.REGISTRATION_ADDRESS FROM CRS_USER_TEMP.PERSONNEL_CARDS_TEMP CC, CRS_USER.CARD_SERIES CS, CRS_USER.CARD_ISSUING CI WHERE CC.IS_CHANGE <> 2 AND CC.CARD_SERIES_ID = CS.ID AND CC.CARD_ISSUING_ID = CI.ID AND CC.PERSONNEL_ID = {PersonnelID} ORDER BY CC.ID";
            try
            {   
                CardsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCardsDataGridView");
                CardsGridView.PopulateColumns();

                CardsGridView.Columns[0].Caption = "S/s";
                CardsGridView.Columns[1].Visible = false;
                CardsGridView.Columns[2].Caption = "Seriya və nömrəsi";
                CardsGridView.Columns[3].Caption = "Verilmə tarixi";
                CardsGridView.Columns[4].Caption = "Veren orqanın adı";
                CardsGridView.Columns[5].Caption = "Etibarlıdır";
                CardsGridView.Columns[6].Caption = "Yaşadığı ünvan";
                CardsGridView.Columns[7].Caption = "Qeydiyyatda olduğu ünvan";

                CardsGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                CardsGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                CardsGridView.Columns[3].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                CardsGridView.Columns[3].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                CardsGridView.Columns[5].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                CardsGridView.Columns[5].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;

                for (int i = 0; i < CardsGridView.Columns.Count; i++)
                {
                    CardsGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    CardsGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                }
                if (CardsGridView.RowCount > 0)
                {
                    EditCardBarButton.Enabled = true;
                    DeleteCardBarButton.Enabled = true;
                    CardsGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                {
                    EditCardBarButton.Enabled = false;
                    DeleteCardBarButton.Enabled = false;
                }
                CardsGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədlər yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        void RefreshCards()
        {
            LoadCardsDataGridView();
        }

        public void LoadFCardAddEdit(string transaction, string PersonnelID, string CardID)
        {
            Customer.FCardAddEdit fp = new Customer.FCardAddEdit();
            fp.TransactionName = transaction;
            fp.CardID = CardID;
            fp.OwnerID = PersonnelID;
            fp.OwnerType = "P";
            fp.RefreshCardsDataGridView += new Customer.FCardAddEdit.DoEvent(RefreshCards);
            fp.ShowDialog();
        }

        private void NewCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardAddEdit("INSERT", PersonnelID, null);
        }

        private void EditCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardAddEdit("EDIT", PersonnelID, CardID);
        }

        private void CardsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CardsGridView.GetFocusedDataRow();
            if (row != null)
                CardID = row["ID"].ToString();
        }

        private void RefreshPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPositions();
        }

        private void PositionGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PositionGridView.GetFocusedDataRow();
            if (row != null)
                positionID = int.Parse(row["ID"].ToString());
        }

        private void EditPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPersonnelPositionAddEdit("EDIT", positionID);
        }

        private void PositionGridView_DoubleClick(object sender, EventArgs e)
        {
            if(EditPositionBarButton.Enabled)
                LoadFPersonnelPositionAddEdit("EDIT", positionID);
        }

        private void DeletePositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int a = 0;//GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CASH_SALARY WHERE PERSONNEL_CARD_ID = {CardID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş vəzifəni silmək istəyirsiniz?", "Vəzifənin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PERSONNEL_POSITIONS_TEMP SET IS_CHANGE = 2 WHERE ID = {positionID}", "Vəzifə silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş vəzifə məzuniyyətlərin hesablanmasında istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            LoadPositions();
        }

        private void PositionGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PositionGridView, PositionPopupMenu, e);
        }

        private void LoadFPersonnelPositionAddEdit(string transaction, int? id)
        {
            topindex = PositionGridView.TopRowIndex;
            old_row_num = PositionGridView.FocusedRowHandle;
            FPersonnelPositionAddEdit fpp = new FPersonnelPositionAddEdit();
            fpp.TransactionName = transaction;
            fpp.ID = id;
            fpp.PersonnelID = int.Parse(PersonnelID);
            fpp.RefreshDataGridView += new FPersonnelPositionAddEdit.DoEvent(LoadPositions);
            fpp.ShowDialog();
            PositionGridView.TopRowIndex = topindex;
            PositionGridView.FocusedRowHandle = old_row_num;
        }

        private void RefreshSalaryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadSalary();
        }

        private void PrintSalaryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(SalaryGridControl);
        }

        private void ExportSalaryBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(SalaryGridControl, "xls");
        }

        private void SalaryGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(SalaryGridView, SalaryPopupMenu, e);
        }

        private void NewPositionBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPersonnelPositionAddEdit("INSERT", null);
        }

        private void CardsGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditCardBarButton.Enabled) && (CardStandaloneBarDockControl.Enabled))
                LoadFCardAddEdit("EDIT", PersonnelID, CardID);
        }

        private void InsertPersonnelTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INS_PERSONNEL_DTL_TEMP", "P_PERSONNEL_ID", PersonnelID, "İşçinin məlumatları temp cədvəllərə daxil olmadı.");            
        }
        
        private void DeleteCard()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CASH_SALARY WHERE PERSONNEL_CARD_ID = {CardID}");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sənədi silmək istəyirsiniz?", "Şəxsiyyəti təsdiq edən sənədin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PERSONNEL_CARDS_TEMP SET IS_CHANGE = 2 WHERE ID = {CardID}", "Şəxsiyyəti təsdiq edən sənəd silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş sənəd kassada işçilərin əmək haqlarının ödənilməsində istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCard();
            LoadCardsDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlPersonnelDetails())
            {
                if (TransactionName == "INSERT")
                    InsertPersonnel();
                else
                    UpdatePersonnel();
                UpdatePhoneSendSms();
                UpdateMailSend();
                InserdPersonnelDetails();
                this.Close();
            }
        }

        private void InserdPersonnelDetails()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_PERSONNEL_DETAILS", "P_PERSONNEL_ID", PersonnelID, "İşçinin məlumatları əsas cədvəllərə daxil edilmədi.");
        }

        private void OtherTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            switch (OtherTabControl.SelectedTabPageIndex)
            {
                case 1:
                    {
                        LoadPhoneDataGridView();
                        LoadMailDataGridView();
                    }
                    break;
                case 2:
                    LoadPositions();
                    break;
                case 3:
                    LoadSalary();
                    break;
            }
        }

        private void LoadPositions()
        {
            string sql = $@"SELECT 1 SS,
                                   PP.ID,
                                   P.NAME POSITION_NAME,
                                   PP.START_DATE,
                                   PP.SALARY,
                                   PP.NOTE
                              FROM CRS_USER_TEMP.PERSONNEL_POSITIONS_TEMP PP, CRS_USER.POSITIONS P
                             WHERE PP.IS_CHANGE != 2 AND PP.POSITION_ID = P.ID AND PP.PERSONNEL_ID = {PersonnelID}
                            ORDER BY PP.ID";

            PositionGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadPositions", "İşçinin vəzifələrinin siyahısı açılmadı.");

            EditPositionBarButton.Enabled = DeletePositionBarButton.Enabled = (PositionGridView.RowCount > 0);
        }
        
        private void LoadSalary()
        {
            string sql = $@"SELECT 1 SS,
                                   PS.ID,
                                   P.NAME POSITION_NAME,
                                   PS.SDATE,
                                   PS.SALARY
                              FROM CRS_USER.PERSONNEL_SALARY PS,
                                   CRS_USER.PERSONNEL_POSITIONS PP,
                                   CRS_USER.POSITIONS P
                             WHERE     PS.PERSONNEL_POSITION_ID = PP.ID
                                   AND PP.POSITION_ID = P.ID
                                   AND PS.SDATE <= LAST_DAY(SYSDATE)
                                   AND PS.PERSONNEL_ID = {PersonnelID}
                            ORDER BY SDATE DESC";
            
            SalaryGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadSalary", "Əmək haqqıların siyahısı açılmadı.");
        }

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GlobalVariables.SelectedLanguage)
            {
                case "AZ": status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (15,16) AND STATUS_NAME", "ID", StatusComboBox.Text);
                    break;
                case "EN": status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (15,16) AND STATUS_NAME_EN", "ID", StatusComboBox.Text);
                    break;
                case "RU": status_id = GlobalFunctions.FindComboBoxSelectedValue("STATUS", "ID IN (15,16) AND STATUS_NAME_RU", "ID", StatusComboBox.Text);
                    break;
            }

            if (status_id == 16)
            {
                CloseDateLabel.Visible = 
                    CloseDateValue.Visible = true;
                SurnameText.Enabled =
                    NameText.Enabled =
                    PatronymicText.Enabled =
                    SexComboBox.Enabled =
                    BirthdayDate.Enabled =
                    BirthplaceComboBox.Enabled =
                    PhoneStandaloneBarDockControl.Enabled =
                    MailStandaloneBarDockControl.Enabled =
                    CardStandaloneBarDockControl.Enabled =
                    PositionStandaloneBarDockControl.Enabled = 
                    BLoadPicture.Enabled = 
                    BDeletePicture.Enabled = false;
                CardsPopupMenu.Manager = 
                    PhonePopupMenu.Manager = 
                    MailPopupMenu.Manager = 
                    PositionPopupMenu.Manager = null;


                CloseDateValue.Properties.MinValue = GlobalFunctions.GetMaxDate($@"SELECT NVL(MAX(START_DATE),TRUNC(SYSDATE)) FROM CRS_USER_TEMP.V_PERSONNEL_LAST_POSITION WHERE PERSONNEL_ID = {PersonnelID}");
                CloseDateValue.EditValue = DateTime.Today;
            }
            else
            {
                CloseDateLabel.Visible = 
                    CloseDateValue.Visible = false;
                SurnameText.Enabled = 
                    NameText.Enabled = 
                    PatronymicText.Enabled = 
                    SexComboBox.Enabled = 
                    BirthdayDate.Enabled = 
                    BirthplaceComboBox.Enabled = 
                    PhoneStandaloneBarDockControl.Enabled = 
                    MailStandaloneBarDockControl.Enabled = 
                    CardStandaloneBarDockControl.Enabled = 
                    PositionStandaloneBarDockControl.Enabled =
                    BLoadPicture.Enabled = true;
                CardsPopupMenu.Manager = CardsBarManager;
                PhonePopupMenu.Manager = PhoneBarManager;
                MailPopupMenu.Manager = MailBarManager;
                PositionPopupMenu.Manager = PositionBarManager;
            }
        }

        private void StatusComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 1, "WHERE ID IN (15, 16)");
        }

        void SelectionImage(string a, int count)
        {
            if (!String.IsNullOrEmpty(a) && File.Exists(a))
            {
                PersonnelPictureBox.Image = Image.FromFile(a);
                PersonnelImage = a;
                BDeletePicture.Enabled = true;
            }
            crop_image_count = count;
        }

        private void PersonnelPictureBox_DoubleClick(object sender, EventArgs e)
        {
            PersonnelPictureBox.Image = null;
            FImageCrop crop = new FImageCrop();
            crop.PictureOwner = "P" + PersonnelID.ToString();
            crop.count = crop_image_count;
            crop.SelectionImage += new FImageCrop.DoEvent(SelectionImage);
            crop.ShowDialog();
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
    }
}