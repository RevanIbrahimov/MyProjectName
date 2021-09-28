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
using Oracle.ManagedDataAccess.Client;
using CRS.Class;

namespace CRS.Forms.Customer
{
    public partial class FCardAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCardAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, OwnerID, CardID, OwnerType;

        string ImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\IDCardImages";
        int card_series_id, card_issuing_id;
        bool CurrentStatus = false;

        public delegate void DoEvent();
        public event DoEvent RefreshCardsDataGridView;

        private void FCardAddEdit_Load(object sender, EventArgs e)
        {
            DateOfIssueDate.Properties.MaxValue = DateTime.Today;
            ReliableDate.Properties.MinValue = DateTime.Today;
            GlobalProcedures.FillLookUpEdit(SeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");                      
            GlobalProcedures.FillLookUpEdit(IssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
            if (TransactionName == "INSERT")
            {
                CardID = GlobalFunctions.GetOracleSequenceValue("CUSTOMER_CARD_SEQUENCE").ToString();
                SeriesLookUp.EditValue = SeriesLookUp.Properties.GetKeyValueByDisplayText("AZE");
            }
            else
            {
                int a = 0;
                if (OwnerType == "C")
                    a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE CUSTOMER_CARDS_ID = " + CardID);
                else if (OwnerType == "F")
                    a = GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_CARD_ID = " + CardID);
                //else if (OwnerType == "P")
                //    a = Class.GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.FOUNDER_CONTRACTS WHERE FOUNDER_CARD_ID = " + CardID);

                if (a == 0)
                    CurrentStatus = false;
                else
                {
                    CurrentStatus = true;
                    XtraMessageBox.Show("Seçilmiş sənəd müqavilələrdə istifadə olunduğu üçün dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Sənədin hal-hazırki statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                ComponentEnable(CurrentStatus);
                LoadCardDetails();
            }
        }

        public void ComponentEnable(bool status)
        {
            SeriesLookUp.Enabled = 
                NumberText.Enabled = 
                PinCodeText.Enabled = 
                DateOfIssueDate.Enabled = 
                ReliableDate.Enabled = 
                IssuingLookUp.Enabled = 
                AddressText.Enabled = 
                RegistrationAddressText.Enabled = 
                NoteText.Enabled = 
                FrontFaceButtonEdit.Properties.Buttons[0].Enabled = 
                FrontFaceButtonEdit.Properties.Buttons[1].Enabled = 
                RearFaceButtonEdit.Properties.Buttons[0].Enabled = 
                RearFaceButtonEdit.Properties.Buttons[1].Enabled = 
                BOK.Visible = !status;
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 2:
                    GlobalProcedures.FillLookUpEdit(SeriesLookUp, "CARD_SERIES", "ID", "SERIES", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 3:
                    GlobalProcedures.FillLookUpEdit(IssuingLookUp, "CARD_ISSUING", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }
        
        private void FrontFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Sənədin ön üzünü seçin";
                    dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png;*.pdf)|*.jpeg;*.jpg;*.bmp;*.png;*.pdf|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Pdf files (*.pdf)|*.pdf";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        FrontFaceButtonEdit.Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                FrontFaceButtonEdit.Text = null;
            else
            {
                try
                {
                    if (FrontFaceButtonEdit.Text.Length != 0)
                        System.Diagnostics.Process.Start(FrontFaceButtonEdit.Text);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin ön üzünün skan formasının ünvanı düz deyil.", FrontFaceButtonEdit.Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void RearFaceButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Sənədin arxa üzünü seçin";
                    dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png;*.pdf)|*.jpeg;*.jpg;*.bmp;*.png;*.pdf|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png|Pdf files (*.pdf)|*.pdf";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        RearFaceButtonEdit.Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                RearFaceButtonEdit.Text = null;
            else
            {
                try
                {
                    if (RearFaceButtonEdit.Text.Length != 0)
                        System.Diagnostics.Process.Start(RearFaceButtonEdit.Text);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin arxa üzünün skan formasının ünvanı düz deyil.", RearFaceButtonEdit.Text, Class.GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void FCardAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.DeleteAllFilesInDirectory(ImagePath);
            this.RefreshCardsDataGridView();
        }

        private bool ControlCardDetails()
        {
            bool b = false;

            if (SeriesLookUp.EditValue == null)
            {
                SeriesLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriyası daxil edilməyib.");
                SeriesLookUp.Focus();
                SeriesLookUp.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(NumberText.Text.Trim()))
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi daxil edilməyib.");               
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if ((card_series_id == 2) && (NumberText.Text.Length != 8))
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriya nömrəsi 8 simvol olmalıdır.");                
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if ((card_series_id == 2) && (PinCodeText.Text.Length != 7) && (!String.IsNullOrEmpty(PinCodeText.Text.Trim())))
            {
                PinCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin pin kodu 7 simvol olmalıdır.");               
                PinCodeText.Focus();
                PinCodeText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(DateOfIssueDate.Text))
            {
                DateOfIssueDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi daxil edilməyib.");                
                DateOfIssueDate.Focus();
                DateOfIssueDate.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else if (String.IsNullOrEmpty(ReliableDate.Text))
            {
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin etibarlı olma tarixi daxil edilməyib.");
                ReliableDate.Focus();
                ReliableDate.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else if (GlobalFunctions.ChangeStringToDate(DateOfIssueDate.Text, "ddmmyyyy") == GlobalFunctions.ChangeStringToDate(ReliableDate.Text, "ddmmyyyy"))
            {
                DateOfIssueDate.BackColor = Color.Red;
                ReliableDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Sənədin verilmə tarixi ilə etibarlı olma tarixi eyni ola bilməz.");                
                DateOfIssueDate.Focus();
                DateOfIssueDate.BackColor = GlobalFunctions.ElementColor(); ;
                ReliableDate.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (IssuingLookUp.EditValue == null)
            {
                IssuingLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədi verən orqanın adı daxil edilməyib.");
                IssuingLookUp.Focus();
                IssuingLookUp.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(RegistrationAddressText.Text))
            {
                RegistrationAddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qeydiyyatda olduğu ünvan daxil edilməyib.");               
                RegistrationAddressText.Focus();
                RegistrationAddressText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            int card_count = 0;
            if (OwnerType == "C")
                card_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER_TEMP.CUSTOMER_CARDS_TEMP UNION ALL SELECT CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER.CUSTOMER_CARDS) WHERE CARD_NUMBER = '" + NumberText.Text.Trim() + "' AND CARD_SERIES_ID = " + card_series_id);
            else if (OwnerType == "F")
                card_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER_TEMP.FOUNDER_CARDS_TEMP UNION ALL SELECT CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER.FOUNDER_CARDS) WHERE CARD_NUMBER = '" + NumberText.Text.Trim() + "' AND CARD_SERIES_ID = " + card_series_id);
            else if (OwnerType == "P")
                card_count = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER_TEMP.PERSONNEL_CARDS_TEMP UNION ALL SELECT CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER.PERSONNEL_CARDS) WHERE CARD_NUMBER = '" + NumberText.Text.Trim() + "' AND CARD_SERIES_ID = " + card_series_id);

            if (card_count > 0 && TransactionName == "INSERT")
            {
                NumberText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Daxil etdiyiniz nömrə artıq bazaya daxil edilib.");                
                NumberText.Focus();
                NumberText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            int card_code = 0;
            if (OwnerType == "C")
                card_code = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT PINCODE,CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER_TEMP.CUSTOMER_CARDS_TEMP UNION ALL SELECT PINCODE,CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER.CUSTOMER_CARDS) WHERE PINCODE = '" + PinCodeText.Text.Trim() + "' AND CARD_NUMBER = '" + NumberText.Text.Trim() + "' AND CARD_SERIES_ID = " + card_series_id);
            else if (OwnerType == "F")
                card_code = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT PINCODE,CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER_TEMP.FOUNDER_CARDS_TEMP UNION ALL SELECT PINCODE,CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER.FOUNDER_CARDS) WHERE PINCODE = '" + PinCodeText.Text.Trim() + "' AND CARD_NUMBER = '" + NumberText.Text.Trim() + "' AND CARD_SERIES_ID = " + card_series_id);
            else if (OwnerType == "P")
                card_code = GlobalFunctions.GetCount("SELECT COUNT(*) FROM (SELECT PINCODE,CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER_TEMP.PERSONNEL_CARDS_TEMP UNION ALL SELECT PINCODE,CARD_NUMBER,CARD_SERIES_ID FROM CRS_USER.PERSONNEL_CARDS) WHERE PINCODE = '" + PinCodeText.Text.Trim() + "' AND CARD_NUMBER = '" + NumberText.Text.Trim() + "' AND CARD_SERIES_ID = " + card_series_id);

            if (card_code > 0 && TransactionName == "INSERT")
            {
                PinCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Daxil etdiyiniz fin kod artıq bazaya daxil edilib.");                
                PinCodeText.Focus();
                PinCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void UpdateCardFrontFace()//senedlerin skan formasinin daxil etmek
        {
            string front_format = "jpeg";
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
                    if (!String.IsNullOrEmpty(FrontFaceButtonEdit.Text))
                    {
                        FileStream front_fls = new FileStream(FrontFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                        byte[] front_blob = new byte[front_fls.Length];
                        front_fls.Read(front_blob, 0, System.Convert.ToInt32(front_fls.Length));
                        front_format = Path.GetExtension(FrontFaceButtonEdit.Text);
                        front_fls.Close();
                        if (OwnerType == "C")
                            command.CommandText = "UPDATE CRS_USER_TEMP.CUSTOMER_CARDS_TEMP SET FRONT_FACE_IMAGE = :BlobFrontImage, FRONT_FACE_IMAGE_FORMAT = '" + front_format + "' WHERE ID = " + CardID;
                        else if (OwnerType == "F")
                            command.CommandText = "UPDATE CRS_USER_TEMP.FOUNDER_CARDS_TEMP SET FRONT_FACE_IMAGE = :BlobFrontImage, FRONT_FACE_IMAGE_FORMAT = '" + front_format + "' WHERE ID = " + CardID;
                        else if (OwnerType == "P")
                            command.CommandText = "UPDATE CRS_USER_TEMP.PERSONNEL_CARDS_TEMP SET FRONT_FACE_IMAGE = :BlobFrontImage, FRONT_FACE_IMAGE_FORMAT = '" + front_format + "' WHERE ID = " + CardID;
                        OracleParameter front_blobParameter = new OracleParameter();
                        front_blobParameter.OracleDbType = OracleDbType.Blob;
                        front_blobParameter.ParameterName = "BlobFrontImage";
                        front_blobParameter.Value = front_blob;
                        command.Parameters.Add(front_blobParameter);
                    }
                    else
                    {
                        if (OwnerType == "C")
                            command.CommandText = "UPDATE CRS_USER_TEMP.CUSTOMER_CARDS_TEMP SET FRONT_FACE_IMAGE = null WHERE ID = " + CardID;
                        else if (OwnerType == "F")
                            command.CommandText = "UPDATE CRS_USER_TEMP.FOUNDER_CARDS_TEMP SET FRONT_FACE_IMAGE = null WHERE ID = " + CardID;
                        else if (OwnerType == "P")
                            command.CommandText = "UPDATE CRS_USER_TEMP.PERSONNEL_CARDS_TEMP SET FRONT_FACE_IMAGE = null WHERE ID = " + CardID;
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin ön üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void UpdateCardRearFace()//senedlerin skan formasinin daxil etmek
        {
            string rear_format = "jpeg";
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
                    if (!String.IsNullOrEmpty(RearFaceButtonEdit.Text))
                    {
                        FileStream rear_fls = new FileStream(RearFaceButtonEdit.Text, FileMode.Open, FileAccess.Read);
                        byte[] rear_blob = new byte[rear_fls.Length];
                        rear_fls.Read(rear_blob, 0, System.Convert.ToInt32(rear_fls.Length));
                        rear_format = Path.GetExtension(RearFaceButtonEdit.Text);
                        rear_fls.Close();
                        if (OwnerType == "C")
                            command.CommandText = "UPDATE CRS_USER_TEMP.CUSTOMER_CARDS_TEMP SET REAR_FACE_IMAGE = :BlobRearImage, REAR_FACE_IMAGE_FORMAT = '" + rear_format + "' WHERE ID = " + CardID;
                        else if (OwnerType == "F")
                            command.CommandText = "UPDATE CRS_USER_TEMP.FOUNDER_CARDS_TEMP SET REAR_FACE_IMAGE = :BlobRearImage, REAR_FACE_IMAGE_FORMAT = '" + rear_format + "' WHERE ID = " + CardID;
                        else if (OwnerType == "P")
                            command.CommandText = "UPDATE CRS_USER_TEMP.PERSONNEL_CARDS_TEMP SET REAR_FACE_IMAGE = :BlobRearImage, REAR_FACE_IMAGE_FORMAT = '" + rear_format + "' WHERE ID = " + CardID;
                        OracleParameter rear_blobParameter = new OracleParameter();
                        rear_blobParameter.OracleDbType = OracleDbType.Blob;
                        rear_blobParameter.ParameterName = "BlobRearImage";
                        rear_blobParameter.Value = rear_blob;
                        command.Parameters.Add(rear_blobParameter);
                    }
                    else
                    {
                        if (OwnerType == "C")
                            command.CommandText = "UPDATE CRS_USER_TEMP.CUSTOMER_CARDS_TEMP SET REAR_FACE_IMAGE = null WHERE ID = " + CardID;
                        else if (OwnerType == "F")
                            command.CommandText = "UPDATE CRS_USER_TEMP.FOUNDER_CARDS_TEMP SET REAR_FACE_IMAGE = null WHERE ID = " + CardID;
                        else if (OwnerType == "P")
                            command.CommandText = "UPDATE CRS_USER_TEMP.PERSONNEL_CARDS_TEMP SET REAR_FACE_IMAGE = null WHERE ID = " + CardID;
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənədin arxa üzünün skan forması sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }
        
        

        private void InsertCard()
        {
            if (OwnerType == "C")
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.CUSTOMER_CARDS_TEMP(ID,CUSTOMER_ID,CARD_SERIES_ID,CARD_NUMBER,PINCODE,ISSUE_DATE,RELIABLE_DATE,CARD_ISSUING_ID,ADDRESS,REGISTRATION_ADDRESS,NOTE,USED_USER_ID,IS_CHANGE)VALUES(" + CardID + "," + OwnerID + "," + card_series_id + ",'" + NumberText.Text.Trim() + "','" + PinCodeText.Text.Trim() + "',TO_DATE('" + DateOfIssueDate.Text + "','DD/MM/YYYY'),TO_DATE('" + ReliableDate.Text + "','DD/MM/YYYY')," + card_issuing_id + ",'" + AddressText.Text.Trim() + "','" + RegistrationAddressText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + GlobalVariables.V_UserID + ",1)",
                                                "Şəxsiyyəti təsdiq edən sənəd temp cədvələ daxil edilmədi.",
                                                this.Name + "/InsertCard");
            else if (OwnerType == "F")
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.FOUNDER_CARDS_TEMP(ID,FOUNDER_ID,CARD_SERIES_ID,CARD_NUMBER,PINCODE,ISSUE_DATE,RELIABLE_DATE,CARD_ISSUING_ID,ADDRESS,REGISTRATION_ADDRESS,NOTE,USED_USER_ID,IS_CHANGE)VALUES(" + CardID + "," + OwnerID + "," + card_series_id + ",'" + NumberText.Text.Trim() + "','" + PinCodeText.Text.Trim() + "',TO_DATE('" + DateOfIssueDate.Text + "','DD/MM/YYYY'),TO_DATE('" + ReliableDate.Text + "','DD/MM/YYYY')," + card_issuing_id + ",'" + AddressText.Text.Trim() + "','" + RegistrationAddressText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + GlobalVariables.V_UserID + ",1)",
                                                "Şəxsiyyəti təsdiq edən sənəd temp cədvələ daxil edilmədi.",
                                                this.Name + "/InsertCard");
            else if (OwnerType == "P")
                GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.PERSONNEL_CARDS_TEMP(ID,PERSONNEL_ID,CARD_SERIES_ID,CARD_NUMBER,PINCODE,ISSUE_DATE,RELIABLE_DATE,CARD_ISSUING_ID,ADDRESS,REGISTRATION_ADDRESS,NOTE,USED_USER_ID,IS_CHANGE)VALUES(" + CardID + "," + OwnerID + "," + card_series_id + ",'" + NumberText.Text.Trim() + "','" + PinCodeText.Text.Trim() + "',TO_DATE('" + DateOfIssueDate.Text + "','DD/MM/YYYY'),TO_DATE('" + ReliableDate.Text + "','DD/MM/YYYY')," + card_issuing_id + ",'" + AddressText.Text.Trim() + "','" + RegistrationAddressText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + GlobalVariables.V_UserID + ",1)",
                                                "Şəxsiyyəti təsdiq edən sənəd temp cədvələ daxil edilmədi.",
                                                this.Name + "/InsertCard");
        }

        private void UpdateCard()
        {
            if (OwnerType == "C")
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.CUSTOMER_CARDS_TEMP SET CARD_SERIES_ID = " + card_series_id + ",CARD_NUMBER = '" + NumberText.Text.Trim() + "',PINCODE = '" + PinCodeText.Text.Trim() + "',ISSUE_DATE = TO_DATE('" + DateOfIssueDate.Text + "','DD/MM/YYYY'),RELIABLE_DATE = TO_DATE('" + ReliableDate.Text + "','DD/MM/YYYY'),CARD_ISSUING_ID = " + card_issuing_id + ",ADDRESS = '" + AddressText.Text.Trim() + "',REGISTRATION_ADDRESS = '" + RegistrationAddressText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',IS_CHANGE = 1 WHERE ID = " + CardID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                "Şəxsiyyəti təsdiq edən sənəd temp cədvəldə dəyişdirilmədi.",
                                              this.Name + "/UpdateCard");
            else if (OwnerType == "F")
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.FOUNDER_CARDS_TEMP SET CARD_SERIES_ID = " + card_series_id + ",CARD_NUMBER = '" + NumberText.Text.Trim() + "',PINCODE = '" + PinCodeText.Text.Trim() + "',ISSUE_DATE = TO_DATE('" + DateOfIssueDate.Text + "','DD/MM/YYYY'),RELIABLE_DATE = TO_DATE('" + ReliableDate.Text + "','DD/MM/YYYY'),CARD_ISSUING_ID = " + card_issuing_id + ",ADDRESS = '" + AddressText.Text.Trim() + "',REGISTRATION_ADDRESS = '" + RegistrationAddressText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',IS_CHANGE = 1 WHERE ID = " + CardID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                "Şəxsiyyəti təsdiq edən sənəd temp cədvəldə dəyişdirilmədi.",
                                                this.Name + "/UpdateCard");
            else if (OwnerType == "P")
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.PERSONNEL_CARDS_TEMP SET CARD_SERIES_ID = " + card_series_id + ",CARD_NUMBER = '" + NumberText.Text.Trim() + "',PINCODE = '" + PinCodeText.Text.Trim() + "',ISSUE_DATE = TO_DATE('" + DateOfIssueDate.Text + "','DD/MM/YYYY'),RELIABLE_DATE = TO_DATE('" + ReliableDate.Text + "','DD/MM/YYYY'),CARD_ISSUING_ID = " + card_issuing_id + ",ADDRESS = '" + AddressText.Text.Trim() + "',REGISTRATION_ADDRESS = '" + RegistrationAddressText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',IS_CHANGE = 1 WHERE ID = " + CardID + " AND USED_USER_ID = " + Class.GlobalVariables.V_UserID,
                                                "Şəxsiyyəti təsdiq edən sənəd temp cədvəldə dəyişdirilmədi.",
                                                this.Name + "/UpdateCard");
        }

        private void IssuingLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (IssuingLookUp.EditValue == null)
                return;

            card_issuing_id = Convert.ToInt32(IssuingLookUp.EditValue);
        }

        private void IssuingLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 3);
        }

        private void SeriesLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 2);
        }

        private void SeriesLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (SeriesLookUp.EditValue == null)
                return;

            card_series_id = Convert.ToInt32(SeriesLookUp.EditValue);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCardDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCard();
                else
                    UpdateCard();
                UpdateCardFrontFace();
                UpdateCardRearFace();
                this.Close();
            }
        }

        private void LoadCardDetails()
        {
            string s = null;
            if (OwnerType == "C")
                s = "SELECT CS.SERIES,CC.CARD_NUMBER,CC.PINCODE,CC.ISSUE_DATE,CI.NAME,CC.RELIABLE_DATE,CC.ADDRESS,CC.REGISTRATION_ADDRESS,CC.NOTE,FRONT_FACE_IMAGE,REAR_FACE_IMAGE,FRONT_FACE_IMAGE_FORMAT,REAR_FACE_IMAGE_FORMAT,CUSTOMER_ID OWNER_ID FROM CRS_USER_TEMP.CUSTOMER_CARDS_TEMP CC, CRS_USER.CARD_SERIES CS, CRS_USER.CARD_ISSUING CI WHERE CC.CARD_SERIES_ID = CS.ID AND CC.CARD_ISSUING_ID = CI.ID AND CC.ID = " + CardID;
            else if (OwnerType == "F")
                s = "SELECT CS.SERIES,CC.CARD_NUMBER,CC.PINCODE,CC.ISSUE_DATE,CI.NAME,CC.RELIABLE_DATE,CC.ADDRESS,CC.REGISTRATION_ADDRESS,CC.NOTE,FRONT_FACE_IMAGE,REAR_FACE_IMAGE,FRONT_FACE_IMAGE_FORMAT,REAR_FACE_IMAGE_FORMAT,FOUNDER_ID OWNER_ID FROM CRS_USER_TEMP.FOUNDER_CARDS_TEMP CC, CRS_USER.CARD_SERIES CS, CRS_USER.CARD_ISSUING CI WHERE CC.CARD_SERIES_ID = CS.ID AND CC.CARD_ISSUING_ID = CI.ID AND CC.ID = " + CardID;
            else if (OwnerType == "P")
                s = "SELECT CS.SERIES,CC.CARD_NUMBER,CC.PINCODE,CC.ISSUE_DATE,CI.NAME,CC.RELIABLE_DATE,CC.ADDRESS,CC.REGISTRATION_ADDRESS,CC.NOTE,FRONT_FACE_IMAGE,REAR_FACE_IMAGE,FRONT_FACE_IMAGE_FORMAT,REAR_FACE_IMAGE_FORMAT,PERSONNEL_ID OWNER_ID FROM CRS_USER_TEMP.PERSONNEL_CARDS_TEMP CC, CRS_USER.CARD_SERIES CS, CRS_USER.CARD_ISSUING CI WHERE CC.CARD_SERIES_ID = CS.ID AND CC.CARD_ISSUING_ID = CI.ID AND CC.ID = " + CardID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCardDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    SeriesLookUp.EditValue = SeriesLookUp.Properties.GetKeyValueByDisplayText(dr[0].ToString());
                    NumberText.Text = dr[1].ToString();
                    PinCodeText.Text = dr[2].ToString();
                    DateOfIssueDate.EditValue = DateTime.Parse(dr[3].ToString());
                    IssuingLookUp.EditValue = IssuingLookUp.Properties.GetKeyValueByDisplayText(dr[4].ToString());
                    ReliableDate.EditValue = DateTime.Parse(dr[5].ToString());
                    AddressText.Text = dr[6].ToString();
                    RegistrationAddressText.Text = dr[7].ToString();
                    NoteText.Text = dr[8].ToString();

                    if (!DBNull.Value.Equals(dr["FRONT_FACE_IMAGE"]))
                    {
                        Byte[] front_BLOBData = (byte[])dr["FRONT_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(front_BLOBData);

                        if (!Directory.Exists(ImagePath))
                        {
                            Directory.CreateDirectory(ImagePath);
                        }
                        FileStream front_fs = new FileStream(ImagePath + "\\" + OwnerType + "_Front_" + dr["OWNER_ID"] + dr["FRONT_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(front_fs);
                        front_fs.Close();
                        stmBLOBData.Close();
                        FrontFaceButtonEdit.Text = ImagePath + "\\" + OwnerType + "_Front_" + dr["OWNER_ID"] + dr["FRONT_FACE_IMAGE_FORMAT"];
                    }

                    if (!DBNull.Value.Equals(dr["REAR_FACE_IMAGE"]))
                    {
                        Byte[] rear_BLOBData = (byte[])dr["REAR_FACE_IMAGE"];
                        MemoryStream stmBLOBData = new MemoryStream(rear_BLOBData);

                        if (!Directory.Exists(ImagePath))
                        {
                            Directory.CreateDirectory(ImagePath);
                        }
                        FileStream front_fs = new FileStream(ImagePath + "\\" + OwnerType + "_Rear_" + dr["OWNER_ID"] + dr["REAR_FACE_IMAGE_FORMAT"], FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(front_fs);
                        front_fs.Close();
                        stmBLOBData.Close();
                        RearFaceButtonEdit.Text = ImagePath + "\\" + OwnerType + "_Rear_" + dr["OWNER_ID"] + dr["REAR_FACE_IMAGE_FORMAT"];
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şəxsiyyəti təsdiq edən sənəd tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void NumberText_EditValueChanged(object sender, EventArgs e)
        {
            if (NumberText.Text.Trim().Length == 0)
                NumberLengthLabel.Visible = false;
            else
            {
                NumberLengthLabel.Visible = true;
                NumberLengthLabel.Text = NumberText.Text.Trim().Length.ToString();
            }
        }

        private void PinCodeText_EditValueChanged(object sender, EventArgs e)
        {
            if (PinCodeText.Text.Trim().Length == 0)
                PinCodeLengthLabel.Visible = false;
            else
            {
                PinCodeLengthLabel.Visible = true;
                PinCodeLengthLabel.Text = PinCodeText.Text.Trim().Length.ToString();
            }
        }
    }
}