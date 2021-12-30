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
using Oracle.ManagedDataAccess.Client;
using CRS.Class.Tables;
using CRS.Class.DataAccess;
using CRS.Class;

namespace CRS.Forms.Customer
{
    public partial class L2FCustomerAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public L2FCustomerAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName,
            CustomerID = "0",
            CustomerFullName;
        public bool ShowCustomer = false;

        string CustomerImage,
            CustomerImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images",
            PhoneID,
            PhoneNumber,
            MailID,
            WorkID,
            CardID;

        int CustomerUsedUserID = -1,
            birthplace_id,
            sex_id,
            crop_image_count = 0;

        bool CurrentStatus = false,
            CustomerUsed = false,
            changecode = false,
            existsImage = false;

        public delegate void DoEvent(string a);
        public event DoEvent RefreshCustomersDataGridView;

        private void FCustomerAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (GlobalVariables.V_UserID > 0)
            {
                if (TransactionName == "INSERT")
                    BChangeCode.Visible = GlobalVariables.EditCustomerCode;
                BirthplaceLookUp.Properties.Buttons[1].Visible = GlobalVariables.Birthplace;
            }

            BirthdayDate.Properties.MaxValue = DateTime.Today;

            GlobalProcedures.FillLookUpEdit(SexLookUp, "SEX", "ID", "NAME", "1 = 1 ORDER BY ID");
            GlobalProcedures.FillLookUpEdit(BirthplaceLookUp, "BIRTHPLACE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");

            if (TransactionName == "EDIT")
            {
                CodeDescriptionLabel.Visible = false;
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CUSTOMERS", GlobalVariables.V_UserID, "WHERE ID = " + CustomerID + " AND USED_USER_ID = -1");
                LoadCustomerDetails();
                CustomerUsed = (CustomerUsedUserID >= 0);

                if (!ShowCustomer)
                {
                    if (CustomerUsed)
                    {
                        if (GlobalVariables.V_UserID != CustomerUsedUserID)
                        {
                            string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CustomerUsedUserID).FULLNAME;
                            XtraMessageBox.Show("Seçilmiş şəxsin məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş müştərinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CurrentStatus = true;
                        }
                        else
                            CurrentStatus = false;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = true;

                ComponentEnable(CurrentStatus);
                InsertTemps();
            }
            else
            {
                CustomerID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_SEQUENCE").ToString();
                CodeDescriptionLabel.Visible = !(GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.CUSTOMER_CODE_TEMP") == 0);
                RegistrationCodeText.Text = GlobalFunctions.GenerateCustomerCode();
            }
            LoadCardsDataGridView();
        }

        public void ComponentEnable(bool status)
        {
            PersonalDetailsGroupBox.Enabled =
                PhoneStandaloneBarDockControl.Enabled =
                MailStandaloneBarDockControl.Enabled =
                WorkStandaloneBarDockControl.Enabled =
                CardStandaloneBarDockControl.Enabled =
                BOK.Visible =
                NoteText.Enabled = !status;

            if (status == false)
            {
                PhonePopupMenu.Manager = PhoneBarManager;
                MailPopupMenu.Manager = MailBarManager;
                WorkPopupMenu.Manager = WorkBarManager;
                CardPopupMenu.Manager = CardBarManager;
            }
            else
            {
                PhonePopupMenu.Manager =
                MailPopupMenu.Manager =
                WorkPopupMenu.Manager =
                CardPopupMenu.Manager = null;
            }
        }

        private void BLoadPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Müştərinin şəkilini seçin";
                dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    CustomerPictureBox.Image = new Bitmap(dlg.FileName);
                    CustomerImage = dlg.FileName;
                    BDeletePicture.Enabled = true;
                }
                dlg.Dispose();
            }
        }

        private void BDeletePicture_Click(object sender, EventArgs e)
        {
            CustomerPictureBox.Image = null;
            CustomerImage = null;
            BLoadPicture.Text = "Yüklə";
            BDeletePicture.Enabled = false;
        }

        private void PhoneGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PhonePopupMenu, e);
        }

        private void MailGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(MailGridView, MailPopupMenu, e);
        }

        private void WorkGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(WorkGridView, WorkPopupMenu, e);
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(BirthplaceLookUp, "BIRTHPLACE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void LoadPhoneDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 P.ID,
                                 PD.DESCRIPTION_AZ DESCRIPTION,
                                 '+' || C.CODE || P.PHONE_NUMBER PHONENUMBER,
                                 P.IS_SEND_SMS,
                                 KR.NAME KINDSHIP_RATE_NAME
                            FROM CRS_USER_TEMP.PHONES_TEMP P,
                                 CRS_USER.PHONE_DESCRIPTIONS PD,
                                 CRS_USER.COUNTRIES C,
                                 CRS_USER.KINDSHIP_RATE KR
                           WHERE     P.IS_CHANGE IN (0, 1)
                                 AND P.PHONE_DESCRIPTION_ID = PD.ID
                                 AND P.COUNTRY_ID = C.ID
                                 AND P.OWNER_TYPE = 'C'
                                 AND P.KINDSHIP_RATE_ID = KR.ID (+)
                                 AND P.OWNER_ID = {CustomerID}
                        ORDER BY P.ORDER_ID";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");
                if (PhoneGridView.RowCount > 0)
                {
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled = true;

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
                else
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled =
                        UpPhoneBarButton.Enabled =
                        DownPhoneBarButton.Enabled = false;
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Telefon nömrələri cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
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
                             WHERE IS_CHANGE IN (0, 1) AND OWNER_TYPE = 'C' AND OWNER_ID = {CustomerID}";
            try
            {
                MailGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadMailDataGridView", "Maillər cədvələ yüklənmədi.");
                EditMailBarButton.Enabled = DeleteMailBarButton.Enabled = (MailGridView.RowCount > 0);

                if (MailGridView.RowCount > 0)
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
                GlobalProcedures.LogWrite("Maillər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void UpdatePhoneSendSms()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0, IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'C' AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                        "Telefon nömrələri dəyişdirilmədi.",
                                                this.Name + "/UpdatePhoneSendSms");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < PhoneGridView.SelectedRowsCount; i++)
            {
                rows.Add(PhoneGridView.GetDataRow(PhoneGridView.GetSelectedRows()[i]));
            }

            if (rows.Count == 0)
                return;

            string listID = null;
            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                listID += row["ID"] + ",";                
            }

            listID = listID.TrimEnd(',');
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 1 WHERE ID IN ({listID}) AND OWNER_TYPE = 'C' AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                                this.Name + "/UpdatePhoneSendSms");
        }

        private void UpdateMailSend()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 0, IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'C' AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Maillər dəyişdirilmədi.",
                                           this.Name + "/UpdateMailSend");

            ArrayList rows = new ArrayList();
            rows.Clear();
            for (int i = 0; i < MailGridView.SelectedRowsCount; i++)
            {
                rows.Add(MailGridView.GetDataRow(MailGridView.GetSelectedRows()[i]));
            }

            if (rows.Count == 0)
                return;

            string listID = null;   
            for (int i = 0; i < rows.Count; i++)
            {
                DataRow row = rows[i] as DataRow;
                listID += row["ID"] + ",";
            }

            listID = listID.TrimEnd(',');
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 1 WHERE ID IN ({listID}) AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Mail göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                              this.Name + "/UpdateMailSend");
        }

        private void InsertTemps()
        {
            if (TransactionName == "INSERT")
                return;
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_CUSTOMER_TEMP", "P_CUSTOMER_ID", CustomerID, "Müştərinin məlumatları temp cədvələ daxil edilmədi.");
        }

        private void InsertCustomerDetails()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_CUSTOMER_DETAILS", "P_CUSTOMER_ID", CustomerID, "Müştərinin temp məlumatları əsas cədvələ daxil edilmədi.");
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
            LoadFPhoneAddEdit("INSERT", CustomerID, "C", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", CustomerID, "C", PhoneID);
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
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'C' AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {PhoneID}",
                                                "Telefon nömrəsi temp cədvəldən silinmədi.",
                                                this.Name + "/DeletePhone");
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
            LoadFMailAddEdit("INSERT", CustomerID, "C", null);
        }

        private void EditMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("EDIT", CustomerID, "C", MailID);
        }

        private void MailGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditMailBarButton.Enabled) && (MailStandaloneBarDockControl.Enabled))
                LoadFMailAddEdit("EDIT", CustomerID, "C", MailID);
        }

        private void PhoneGridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            GlobalProcedures.GridCustomDrawFooterCell("SS", "Center", e);
        }

        private void DeleteMail()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş elektron ünvanları silmək istəyirsiniz?", "Elektron ünvanların silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'C' AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = {MailID}", "Elektron ünvanlar temp cədvəldən silinmədi.");
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

        private void FCustomerAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CUSTOMERS", -1, "WHERE ID = " + CustomerID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);

            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER_TEMP.PROC_CUSTOMER_DELETE_ALL_TEMP", "P_USED_USER_ID", GlobalVariables.V_UserID, "Məlumatlar temp cədvəldən silinmədi.");
            GlobalProcedures.DeleteAllFilesInDirectory(CustomerImagePath);
            this.RefreshCustomersDataGridView(RegistrationCodeText.Text.Trim());
        }

        private void LoadWorkDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                   ID,
                                   PLACE_NAME,
                                   POSITION,
                                   START_DATE,
                                   END_DATE,
                                   NOTE
                              FROM CRS_USER_TEMP.CUSTOMER_WORKPLACE_TEMP
                             WHERE IS_CHANGE <> 2 AND CUSTOMER_ID = {CustomerID}";
            WorkGridControl.DataSource = GlobalFunctions.GenerateDataTable(s);
            EditWorkBarButton.Enabled = DeleteWorkBarButton.Enabled = (WorkGridView.RowCount > 0);
        }

        void RefreshWork()
        {
            LoadWorkDataGridView();
        }

        public void LoadFWorkAddEdit(string transaction, string CustomerID, string WorkID)
        {
            FWorkAddEdit fp = new FWorkAddEdit();
            fp.TransactionName = transaction;
            fp.WorkID = WorkID;
            fp.CustomerID = CustomerID;
            fp.RefresWorkPlaceDataGridView += new FWorkAddEdit.DoEvent(RefreshWork);
            fp.ShowDialog();
        }

        private void NewWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFWorkAddEdit("INSERT", CustomerID, null);
        }

        private void WorkGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = WorkGridView.GetFocusedDataRow();
            if (row != null)
                WorkID = row["ID"].ToString();
        }

        private void EditWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFWorkAddEdit("EDIT", CustomerID, WorkID);
        }

        private void WorkGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditWorkBarButton.Enabled) && (WorkStandaloneBarDockControl.Enabled))
                LoadFWorkAddEdit("EDIT", CustomerID, WorkID);
        }

        private void OtherInfoTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            switch (OtherInfoTabControl.SelectedTabPageIndex)
            {
                case 1:
                    {
                        LoadPhoneDataGridView();
                        LoadMailDataGridView();
                    }
                    break;
                case 2:
                    LoadWorkDataGridView();
                    break;
            }
        }

        private void DeleteWork()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş iş yerini silmək istəyirsiniz?", "Müştərinin iş yerinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CUSTOMER_WORKPLACE_TEMP SET IS_CHANGE = 2 WHERE ID = {WorkID}",
                                                "Müştərinin iş yeri üçün olan temp məlumatlar silinmədi.",
                                                this.Name + "/DeleteWork");
            }
        }

        private void DeleteWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteWork();
            LoadWorkDataGridView();
        }

        private void RefreshWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadWorkDataGridView();
        }

        private bool ControlCustomerDetails()
        {
            bool b = false;

            if (RegistrationCodeText.Text.Length == 0)
            {
                RegistrationCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin qeydiyyat nömrəsi daxil edilməyib.");
                RegistrationCodeText.Focus();
                RegistrationCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SurnameText.Text.Length == 0)
            {
                SurnameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin soyadı daxil edilməyib.");
                SurnameText.Focus();
                SurnameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PatronymicText.Text.Length == 0)
            {
                PatronymicText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin atasının adı daxil edilməyib.");
                PatronymicText.Focus();
                PatronymicText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SexLookUp.EditValue == null)
            {
                SexLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin cinsi daxil edilməyib.");
                SexLookUp.Focus();
                SexLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.ChangeStringToDate(BirthdayDate.Text, "ddmmyyyy") >= DateTime.Today)
            {
                BirthdayDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin doğum tarixi cari tarixdən kiçik olmalıdır.");
                BirthdayDate.Focus();
                BirthdayDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (BirthplaceLookUp.EditValue == null)
            {
                BirthplaceLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin doğum yeri daxil edilməyib.");
                BirthplaceLookUp.Focus();
                BirthplaceLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length > 0 && !GlobalFunctions.Regexp("[0-9]{9}2", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Fiziki şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>2</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (CardsGridView.RowCount == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 0;
                CardsGridControl.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin şəxsiyyətini təsdiq edən sənəd daxil daxil edilməyib.");
                CardsGridControl.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PHONES_TEMP WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'C' AND OWNER_ID = {CustomerID} AND USED_USER_ID = {GlobalVariables.V_UserID}") == 0)
            {
                OtherInfoTabControl.SelectedTabPageIndex = 1;
                PhoneGridControl.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin ən azı bir telefon nömrəsi daxil edilməlidir.");
                PhoneGridControl.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.V_CUSTOMERS WHERE CODE = '" + RegistrationCodeText.Text + "'") > 0)
            {
                RegistrationCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(RegistrationCodeText.Text + " nömrəli müştəri artıq bazaya daxil edilib.");
                RegistrationCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CUSTOMERS WHERE SURNAME = '" + GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim()) + "' AND NAME = '" + GlobalFunctions.FirstCharToUpper(NameText.Text.Trim()) + "' AND (PATRONYMIC = '" + GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim()) + "' OR PATRONYMIC LIKE '%" + GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim()) + "%') AND BIRTHDAY = TO_DATE('" + BirthdayDate.Text + "','DD/MM/YYYY') AND BIRTHPLACE_ID = " + birthplace_id) > 0)
            {
                SurnameText.BackColor =
                    NameText.BackColor =
                    PatronymicText.BackColor =
                    BirthdayDate.BackColor =
                    BirthplaceLookUp.BackColor = Color.Red;

                DialogResult dialogResult = XtraMessageBox.Show(SurnameText.Text.Trim() + " " + NameText.Text.Trim() + " " + PatronymicText.Text.Trim() + " artıq bazaya daxil edilib. Bu müştərinin məlumatlarını təkrar olaraq bazaya daxil etmək istəyirsiniz?.", "Təkrar müştəri", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    b = true;
                else
                {
                    SurnameText.Focus();
                    SurnameText.BackColor =
                        NameText.BackColor =
                        PatronymicText.BackColor =
                        BirthdayDate.BackColor =
                        BirthplaceLookUp.BackColor = GlobalFunctions.ElementColor();
                    return false;
                }
            }
            else
                b = true;
            return b;
        }

        private void InsertCustomer()
        {
            int max_code_number = GlobalFunctions.GetID("SELECT NVL(MAX(TO_NUMBER(CODE)),0) FROM CRS_USER.V_CUSTOMERS") + 1;
            string code = null, sql = null;
            if (changecode)
                code = RegistrationCodeText.Text;
            else
                code = max_code_number.ToString().PadLeft(5, '0').Trim();

            sql = $@"INSERT INTO CRS_USER.CUSTOMERS(
                                                        ID,
                                                        CODE,
                                                        SURNAME, 
                                                        NAME, 
                                                        PATRONYMIC, 
                                                        SEX_ID, 
                                                        NOTE,
                                                        BIRTHDAY,
                                                        BIRTHPLACE_ID,
                                                        VOEN
                                                    )
                        VALUES(
                                {CustomerID},
                                '{code}',
                                '{GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim())}',
                                '{GlobalFunctions.FirstCharToUpper(NameText.Text.Trim())}',
                                '{GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim())}',
                                {sex_id},
                                '{NoteText.Text.Trim()}',
                                TO_DATE('{BirthdayDate.Text}','DD/MM/YYYY'),
                                {birthplace_id},
                                '{VoenText.Text}')";

            GlobalFunctions.ExecuteQuery(sql, "Müştərinin məlumatları sistemə daxil edilmədi.", this.Name + "/InsertCustomer");
        }

        private void InsertCustomerImage()
        {
            GlobalFunctions.ExecuteQueryWithBlob($@"INSERT INTO CRS_USER.CUSTOMER_IMAGE(CUSTOMER_ID,IMAGE) VALUES({CustomerID},:BlobFile)", CustomerImage, "Müştərinin şəkli bazaya daxil edilmədi.");
        }

        private void UpdateCustomer()
        {
            string sql = $@"UPDATE CRS_USER.CUSTOMERS SET 
                                                SURNAME = '{GlobalFunctions.FirstCharToUpper(SurnameText.Text.Trim())}',
                                                NAME = '{GlobalFunctions.FirstCharToUpper(NameText.Text.Trim())}',
                                                PATRONYMIC = '{GlobalFunctions.FirstCharToUpper(PatronymicText.Text.Trim())}',
                                                SEX_ID = {sex_id},
                                                NOTE = '{NoteText.Text.Trim()}',
                                                BIRTHDAY = TO_DATE('{BirthdayDate.Text}','DD/MM/YYYY'),
                                                BIRTHPLACE_ID = {birthplace_id},
                                                VOEN = '{VoenText.Text}'
                                WHERE ID = {CustomerID}";
            GlobalProcedures.ExecuteQuery(sql, "Müştərinin məlumatları sistemdə dəyişdirilmədi.", this.Name + "/UpdateCustomer");
        }

        private void UpdateCustomerImage()
        {
            if (existsImage)
            {
                if (String.IsNullOrWhiteSpace(CustomerImage))
                    GlobalProcedures.ExecuteQuery($@"DELETE CRS_USER.CUSTOMER_IMAGE WHERE CUSTOMER_ID = {CustomerID}", "Müştərinin şəkli sistemdən silinmədi.", this.Name + "/UpdateCustomerImage");
                else
                    GlobalFunctions.ExecuteQueryWithBlob($@"UPDATE CRS_USER.CUSTOMER_IMAGE SET IMAGE = :BlobFile WHERE CUSTOMER_ID = {CustomerID}", CustomerImage, "Müştərinin şəkli sistemdə dəyişdirilmədi.");
            }
            else
                GlobalFunctions.ExecuteQueryWithBlob($@"INSERT INTO CRS_USER.CUSTOMER_IMAGE(CUSTOMER_ID,IMAGE) VALUES({CustomerID},:BlobFile)", CustomerImage, "Müştərinin şəkli bazaya daxil edilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCustomerDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCustomerSaveWait));
                if (TransactionName == "INSERT")
                {
                    InsertCustomer();
                    InsertCustomerImage();
                }
                else
                {
                    UpdateCustomer();
                    UpdateCustomerImage();
                }
                UpdatePhoneSendSms();
                UpdateMailSend();
                InsertCustomerDetails();
                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }

        private void BirthdayDate_EditValueChanged(object sender, EventArgs e)
        {
            AgeLabel.Text = GlobalFunctions.CalculationAgeWithYear(BirthdayDate.DateTime, DateTime.Today);
        }

        private void LoadCustomerDetails()
        {
            string s = $@"SELECT C.CODE,
                       C.SURNAME,
                       C.NAME,
                       C.PATRONYMIC,
                       S.NAME SEX_NAME,
                       C.BIRTHDAY,
                       B.NAME BIRTHPLACE_NAME,
                       C.NOTE,
                       CI.IMAGE,
                       C.USED_USER_ID,
                       C.VOEN
                  FROM CRS_USER.CUSTOMERS C,
                       CRS_USER.BIRTHPLACE B,
                       CRS_USER.SEX S,
                       CRS_USER.CUSTOMER_IMAGE CI
                 WHERE     C.BIRTHPLACE_ID = B.ID
                       AND C.SEX_ID = S.ID
                       AND C.ID = CI.CUSTOMER_ID(+)
                       AND C.ID = {CustomerID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCustomerDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    RegistrationCodeText.Text = dr["CODE"].ToString();
                    SurnameText.Text = dr["SURNAME"].ToString();
                    NameText.Text = dr["NAME"].ToString();
                    PatronymicText.Text = dr["PATRONYMIC"].ToString();
                    SexLookUp.EditValue = SexLookUp.Properties.GetKeyValueByDisplayText(dr["SEX_NAME"].ToString());
                    BirthdayDate.EditValue = DateTime.Parse(dr["BIRTHDAY"].ToString());
                    BirthplaceLookUp.EditValue = BirthplaceLookUp.Properties.GetKeyValueByDisplayText(dr["BIRTHPLACE_NAME"].ToString());
                    NoteText.Text = dr["NOTE"].ToString();
                    VoenText.Text = dr["VOEN"].ToString();
                    CustomerUsedUserID = Convert.ToInt32(dr["USED_USER_ID"]);

                    if (!DBNull.Value.Equals(dr["IMAGE"]))
                    {
                        existsImage = true;
                        Byte[] BLOBData = (byte[])dr["IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        CustomerPictureBox.Image = Image.FromStream(stmBLOBData);

                        if (!Directory.Exists(CustomerImagePath))
                            Directory.CreateDirectory(CustomerImagePath);

                        GlobalProcedures.DeleteFile(CustomerImagePath + "\\C_" + RegistrationCodeText.Text + ".jpeg");
                        FileStream fs = new FileStream(CustomerImagePath + "\\C_" + RegistrationCodeText.Text + ".jpeg", FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(fs);
                        fs.Close();
                        stmBLOBData.Close();
                        CustomerImage = CustomerImagePath + "\\C_" + RegistrationCodeText.Text + ".jpeg";
                        BLoadPicture.Text = "Dəyiş";
                        BDeletePicture.Enabled = true;
                    }
                    else
                    {
                        BLoadPicture.Text = "Yüklə";
                        BDeletePicture.Enabled = false;
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Müştərinin parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void CardsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CardsGridView, CardPopupMenu, e);
        }

        private void LoadCardsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                 CC.ID,
                                 CS.NAME || ': ' || CS.SERIES || ' - ' || CC.CARD_NUMBER CARD,
                                 CC.ISSUE_DATE,
                                 CI.NAME ISSUE_NAME,
                                 CC.RELIABLE_DATE,
                                 CC.ADDRESS,
                                 CC.REGISTRATION_ADDRESS
                            FROM CRS_USER_TEMP.CUSTOMER_CARDS_TEMP CC,
                                 CRS_USER.CARD_SERIES CS,
                                 CRS_USER.CARD_ISSUING CI
                           WHERE     CC.CARD_SERIES_ID = CS.ID
                                 AND CC.CARD_ISSUING_ID = CI.ID
                                 AND CC.IS_CHANGE <> 2
                                 AND CC.CUSTOMER_ID = {CustomerID}
                        ORDER BY CC.ID";
            CardsGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCardsDataGridView");
            EditCardBarButton.Enabled = DeleteCardBarButton.Enabled = (CardsGridView.RowCount > 0);
        }

        void RefreshCards()
        {
            LoadCardsDataGridView();
        }

        public void LoadFCardAddEdit(string transaction, string CustomerID, string CardID)
        {
            FCardAddEdit fp = new FCardAddEdit();
            fp.TransactionName = transaction;
            fp.CardID = CardID;
            fp.OwnerID = CustomerID;
            fp.OwnerType = "C";
            fp.RefreshCardsDataGridView += new FCardAddEdit.DoEvent(RefreshCards);
            fp.ShowDialog();
        }

        private void NewCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardAddEdit("INSERT", CustomerID, null);
        }

        private void RefreshCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCardsDataGridView();
        }

        private void CardsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CardsGridView.GetFocusedDataRow();
            if (row != null)
                CardID = row["ID"].ToString();
        }

        private void EditCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardAddEdit("EDIT", CustomerID, CardID);
        }

        private void CardsGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditCardBarButton.Enabled) && (CardStandaloneBarDockControl.Enabled))
                LoadFCardAddEdit("EDIT", CustomerID, CardID);
        }

        private void DeleteCard()
        {
            if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE CUSTOMER_CARDS_ID = " + CardID) == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sənədi silmək istəyirsiniz?", "Şəxsiyyəti təsdiq edən sənədin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.CUSTOMER_CARDS_TEMP SET IS_CHANGE = 2 WHERE ID = " + CardID, "Şəxsiyyəti təsdiq edən sənəd silinmədi.");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş sənəd bazada müqavilələrdə istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCard();
            LoadCardsDataGridView();
        }

        void RefreshCode(string code, bool close)
        {
            changecode = close;
            if (close)
                RegistrationCodeText.Text = code;
        }

        private void BChangeCode_Click(object sender, EventArgs e)
        {
            FChangeCode fcc = new FChangeCode();
            fcc.type = 1;
            fcc.RefreshCode += new FChangeCode.DoEvent(RefreshCode);
            fcc.ShowDialog();
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled && PhoneStandaloneBarDockControl.Enabled)
                LoadFPhoneAddEdit("EDIT", CustomerID, "C", PhoneID);
        }

        void SelectionImage(string a, int count)
        {
            if (!String.IsNullOrEmpty(a) && File.Exists(a))
            {
                CustomerPictureBox.Image = Image.FromFile(a);
                CustomerImage = a;
                BDeletePicture.Enabled = true;
            }
            crop_image_count = count;
        }

        private void CustomerPictureBox_DoubleClick(object sender, EventArgs e)
        {
            CustomerPictureBox.Image = null;
            FImageCrop crop = new FImageCrop();
            crop.PictureOwner = "C" + CustomerID.ToString();
            crop.count = crop_image_count;
            crop.SelectionImage += new FImageCrop.DoEvent(SelectionImage);
            crop.ShowDialog();
        }

        private void PhoneGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = (PhoneGridView.RowCount > 0);
        }

        private void BirthplaceLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 4);
        }

        private void BirthplaceLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (BirthplaceLookUp.EditValue == null)
                return;

            birthplace_id = Convert.ToInt32(BirthplaceLookUp.EditValue);
        }

        private void VoenText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(VoenText, VoenLengthLabel);
        }

        private void PatronymicText_EditValueChanged(object sender, EventArgs e)
        {
            if (PatronymicText.Text.IndexOf("oğlu") > -1)
                GlobalProcedures.LookUpEditValue(SexLookUp, "Kişi");
            else if (PatronymicText.Text.IndexOf("qızı") > -1)
                GlobalProcedures.LookUpEditValue(SexLookUp, "Qadın");
        }

        private void RegistrationCodeText_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void SexLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SexLookUp.EditValue == null)
                return;

            sex_id = Convert.ToInt32(SexLookUp.EditValue);
        }

        private void MailGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditMailBarButton.Enabled = DeleteMailBarButton.Enabled = (MailGridView.RowCount > 0);
        }

        private void WorkGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditWorkBarButton.Enabled = DeleteWorkBarButton.Enabled = (WorkGridView.RowCount > 0);
        }

        private void CardsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditCardBarButton.Enabled = DeleteCardBarButton.Enabled = (CardsGridView.RowCount > 0);
        }

        private void ExportCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(CardsGridControl, "xls");
        }

        private void PrintCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(CardsGridControl);
        }

        private void PrintMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(MailGridControl);
        }

        private void ExportMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(MailGridControl, "xls");
        }

        private void PrintWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(WorkGridControl);
        }

        private void ExportWorkBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(WorkGridControl, "xls");
        }

        private void PrintPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(PhoneGridControl);
        }

        private void ExportPhoneBarButton_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(PhoneGridControl, "xls");
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