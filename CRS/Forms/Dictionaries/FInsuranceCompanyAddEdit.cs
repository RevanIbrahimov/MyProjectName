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

namespace CRS.Forms.Dictionaries
{
    public partial class FInsuranceCompanyAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FInsuranceCompanyAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CompanyID;
        bool CurrentStatus = false, CompanyUsed = false;
        int CompanyUsedUserID = -1;
        string LogoImage, LogoImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Images";

        public delegate void DoEvent();
        public event DoEvent RefreshInsuranceCompanyDataGridView;


        private void FInsuranceCompanyAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")  
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.INSURANCE_COMPANY", GlobalVariables.V_UserID, "WHERE ID = " + CompanyID + " AND USED_USER_ID = -1");
                CompanyUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.INSURANCE_COMPANY WHERE ID = {CompanyID}");
                CompanyUsed = (CompanyUsedUserID > 0);
                
                if (CompanyUsed)
                {
                    if (GlobalVariables.V_UserID != CompanyUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == CompanyUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş şirkət hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş şirkətin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadCompanyDetails();
            }
        }

        private void LoadCompanyDetails()
        {
            string s = "SELECT NAME,NOTE,ADDRESS,ADDRESS_DESCRIPTION,PHONE_NUMBER,FAX,LOGO FROM CRS_USER.INSURANCE_COMPANY WHERE ID = " + CompanyID;
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCompanyDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    NameText.Text = dr[0].ToString();
                    NoteText.Text = dr[1].ToString();
                    AddressText.Text = dr[2].ToString();
                    AddressDescriptionText.Text = dr[3].ToString();
                    PhoneText.Text = dr[4].ToString();
                    FaxText.Text = dr[5].ToString();

                    if (!DBNull.Value.Equals(dr["LOGO"]))
                    {
                        Byte[] BLOBData = (byte[])dr["LOGO"];
                        MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                        LogoPictureBox.Image = Image.FromStream(stmBLOBData);


                        if (!Directory.Exists(LogoImagePath))
                        {
                            Directory.CreateDirectory(LogoImagePath);
                        }
                        GlobalProcedures.DeleteFile(LogoImagePath + "\\IC_" + CompanyID + ".jpeg");
                        FileStream fs = new FileStream(LogoImagePath + "\\IC_" + CompanyID + ".jpeg", FileMode.Create, FileAccess.Write);
                        stmBLOBData.WriteTo(fs);
                        fs.Close();
                        stmBLOBData.Close();
                        LogoImage = LogoImagePath + "\\IC_" + CompanyID + ".jpeg";
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
                                LogoPictureBox.Properties.NullText = "Логотип компании";
                                break;
                            case "EN":
                                LogoPictureBox.Properties.NullText = "Company logo";
                                break;
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şirkətin məlumatları tapılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
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
                dlg.Title = "Müştərinin şəkilini seçin";
                dlg.Filter = "All files (*.jpeg;*.jpg;*.bmp;*.png)|*.jpeg;*.jpg;*.bmp;*.png|Image files (*.jpeg;*.jpg)|*.jpeg;*.jpg|Bmp files (*.bmp)|*.bmp|Png files (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LogoPictureBox.Image = new Bitmap(dlg.FileName);
                    LogoImage = dlg.FileName;
                    BDeletePicture.Enabled = true;
                }
                dlg.Dispose();
            }
        }

        private bool ControlCompanyDetails()
        {
            bool b = false;

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şirkətin adı daxil edilməyib.");               
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AddressText.Text.Length == 0)
            {
                AddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şirkətin ünvanı daxil edilməyib.");                
                AddressText.Focus();
                AddressText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PhoneText.Text.Length == 0)
            {
                PhoneText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şirkətin telefon nömrəsi daxil edilməyib.");                
                PhoneText.Focus();
                PhoneText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCompany()
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
                    if (LogoImage != null)
                    {
                        FileStream fls = new FileStream(LogoImage, FileMode.Open, FileAccess.Read);
                        byte[] blob = new byte[fls.Length];
                        fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                        fls.Close();
                        command.CommandText = "INSERT INTO CRS_USER.INSURANCE_COMPANY(NAME,ADDRESS,ADDRESS_DESCRIPTION,PHONE_NUMBER,FAX,NOTE,"
                                                                             + "LOGO) VALUES('" + NameText.Text.Trim() + "','" + AddressText.Text.Trim() + "','" + AddressDescriptionText.Text.Trim() + "','"
                                                                            + PhoneText.Text.Trim() + "','" + FaxText.Text.Trim() + "','" + NoteText.Text.Trim() + "',:BlobImage)";
                        OracleParameter blobParameter = new OracleParameter();
                        blobParameter.OracleDbType = OracleDbType.Blob;
                        blobParameter.ParameterName = "BlobImage";
                        blobParameter.Value = blob;
                        command.Parameters.Add(blobParameter);
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO CRS_USER.INSURANCE_COMPANY(NAME,ADDRESS,ADDRESS_DESCRIPTION,PHONE_NUMBER,FAX,NOTE) VALUES('" + NameText.Text.Trim() + "','" + AddressText.Text.Trim() + "','" + AddressDescriptionText.Text.Trim() + "','"
                                                                            + PhoneText.Text.Trim() + "','" + FaxText.Text.Trim() + "','" + NoteText.Text.Trim() + "')";
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Şirkətin məlumatları sistemə daxil edilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                    
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void UpdateCompany()
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
                    if (LogoImage != null)
                    {
                        FileStream fls = new FileStream(LogoImage, FileMode.Open, FileAccess.Read);
                        byte[] blob = new byte[fls.Length];
                        fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
                        fls.Close();
                        command.CommandText = "UPDATE CRS_USER.INSURANCE_COMPANY SET NAME = '" + NameText.Text.Trim() + "',ADDRESS = '" + AddressText.Text.Trim() + "',ADDRESS_DESCRIPTION = '" + AddressDescriptionText.Text.Trim() + "',PHONE_NUMBER = '" + PhoneText.Text.Trim() + "',FAX = '" + FaxText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',LOGO = :BlobImage WHERE ID = " + CompanyID;
                        OracleParameter blobParameter = new OracleParameter();
                        blobParameter.OracleDbType = OracleDbType.Blob;
                        blobParameter.ParameterName = "BlobImage";
                        blobParameter.Value = blob;
                        command.Parameters.Add(blobParameter);
                    }
                    else
                    {
                        command.CommandText = "UPDATE CRS_USER.INSURANCE_COMPANY SET NAME = '" + NameText.Text.Trim() + "',ADDRESS = '" + AddressText.Text.Trim() + "',ADDRESS_DESCRIPTION = '" + AddressDescriptionText.Text.Trim() + "',PHONE_NUMBER = '" + PhoneText.Text.Trim() + "',FAX = '" + FaxText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "',LOGO = NULL WHERE ID = " + CompanyID;
                    }
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception exx)
                {
                    transaction.Rollback();
                    GlobalProcedures.LogWrite("Şirkətin məlumatları dəyişdirilmədi.", command.CommandText, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                    
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        private void BDeletePicture_Click(object sender, EventArgs e)
        {
            LogoPictureBox.Image = null;
            LogoImage = null;
            BLoadPicture.Text = "Yüklə";
            BDeletePicture.Enabled = false;
        }

        private void FInsuranceCompanyAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.INSURANCE_COMPANY", -1, "WHERE ID = " + CompanyID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshInsuranceCompanyDataGridView();
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled = !status;
            AddressText.Enabled = !status;
            AddressDescriptionText.Enabled = !status;
            PhoneText.Enabled = !status;
            FaxText.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCompanyDetails())
            {
                if (TransactionName == "INSERT")
                    InsertCompany();
                else
                    UpdateCompany();
                this.Close();
            }
        }
    }
}