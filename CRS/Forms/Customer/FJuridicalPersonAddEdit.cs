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
using System.Collections;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Customer
{
    public partial class FJuridicalPersonAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FJuridicalPersonAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, CustomerID;

        int CustomerUsedUserID = -1;
        bool CurrentStatus = false, CustomerUsed = false, changecode = false;
        string PhoneID, PhoneNumber, MailID;

        public delegate void DoEvent(string a);
        public event DoEvent RefreshCustomersDataGridView;

        private void FJuridicalPersonAddEdit_Load(object sender, EventArgs e)
        {
            //permission
            if (Class.GlobalVariables.V_UserID > 0)
            {
                if (TransactionName == "INSERT")
                    BChangeCode.Visible = Class.GlobalVariables.EditCustomerCode;
            }
            if (TransactionName == "EDIT")
            {
                CodeDescriptionLabel.Visible = false;
                Class.GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.JURIDICAL_PERSONS", Class.GlobalVariables.V_UserID, "WHERE ID = " + CustomerID + " AND USED_USER_ID = -1");
                List<JuridicalPersons> lstJuridicalPersons = JuridicalPersonsDAL.SelectJuridicalPerson(int.Parse(CustomerID)).ToList<JuridicalPersons>();
                CustomerUsedUserID = lstJuridicalPersons.First().USED_USER_ID;
                CustomerUsed = (CustomerUsedUserID >= 0);

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
                ComponentEnable(CurrentStatus);
                InsertTemps();
                //InsertPhonesTemp();
                //InsertMailTemp();
                LoadCustomerDetails();
            }
            else
            {
                CustomerID = GlobalFunctions.GetOracleSequenceValue("JURIDICAL_PERSONS_SEQUENCE").ToString();
                CodeDescriptionLabel.Visible = !(GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER_TEMP.CUSTOMER_CODE_TEMP") == 0);
                RegistrationCodeText.Text = GlobalFunctions.GenerateCustomerCode();
            }
            LoadPhoneDataGridView();
            LoadMailDataGridView();
        }

        private void LoadCustomerDetails()
        {
            string s = $@"SELECT CODE,
                                   NAME,
                                   LEADING_NAME,
                                   ADDRESS,
                                   VOEN,
                                   ACCOUNT_NUMBER,
                                   NOTE
                              FROM CRS_USER.JURIDICAL_PERSONS
                             WHERE IS_BUYER = 1 AND ID = {CustomerID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadCustomerDetails", "Müştərinin parametrləri açılmadı.");

            foreach (DataRow dr in dt.Rows)
            {
                RegistrationCodeText.Text = dr["CODE"].ToString();
                FullNameText.Text = dr["NAME"].ToString();
                LeadingPersonNameText.Text = dr["LEADING_NAME"].ToString();
                AddressText.Text = dr["ADDRESS"].ToString();
                VoenText.Text = dr["VOEN"].ToString();
                AccountText.Text = dr["ACCOUNT_NUMBER"].ToString();
                NoteText.Text = dr["NOTE"].ToString();
            }
        }

        public void ComponentEnable(bool status)
        {
            PersonalDetailsGroupBox.Enabled =
                PhoneStandaloneBarDockControl.Enabled =
                MailStandaloneBarDockControl.Enabled = !status;
            BOK.Visible = !status;

            if (status == false)
            {
                PhonePopupMenu.Manager = PhoneBarManager;
                MailPopupMenu.Manager = MailBarManager;
            }
            else
            {
                PhonePopupMenu.Manager = null;
                MailPopupMenu.Manager = null;
            }
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
                                 AND P.OWNER_TYPE = 'JP'
                                 AND P.OWNER_ID = {CustomerID}
                        ORDER BY P.ORDER_ID";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");
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
                             WHERE IS_CHANGE IN (0, 1) AND OWNER_TYPE = 'JP' AND OWNER_ID = {CustomerID}";
            try
            {
                MailGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadMailDataGridView");
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
                GlobalProcedures.LogWrite("Maillər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void UpdatePhoneSendSms()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0, IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'JP' AND USED_USER_ID = {GlobalVariables.V_UserID}",
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
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'JP' AND ID IN ({listID}) AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                                this.Name + "/UpdatePhoneSendSms");
        }

        private void UpdateMailSend()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 0, IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'JP' AND USED_USER_ID = {GlobalVariables.V_UserID}",
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
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'JP' AND ID IN ({listID}) AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                    "Telefon nömrələrinə sms göndərmək üçün olan seçimlər yadda saxlanmadı.",
                                              this.Name + "/UpdateMailSend");
        }

        private void InsertTemps()
        {
            if (TransactionName == "INSERT")
                return;

            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_JP_TEMP", "P_CUSTOMER_ID", CustomerID, "Müştərinin məlumatları temp cədvələ daxil edilmədi.");
        }

        private void InsertCustomerDetails()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_JP_DETAILS", "P_CUSTOMER_ID", CustomerID, "Müştərinin məlumatları temp cədvələ daxil edilmədi.");
        }

        private void RefreshPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPhoneDataGridView();
        }

        private void RefreshMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadMailDataGridView();
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
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'JP' AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = " + PhoneID, "Telefon nömrəsi temp cədvəldən silinmədi.");
            }
        }

        private void DeletePhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeletePhone();
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
            LoadFMailAddEdit("INSERT", CustomerID, "JP", null);
        }

        private void EditMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("EDIT", CustomerID, "JP", MailID);
        }

        private void MailGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditMailBarButton.Enabled) && (MailStandaloneBarDockControl.Enabled))
                LoadFMailAddEdit("EDIT", CustomerID, "JP", MailID);
        }

        private void DeleteMail()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş elektron ünvanları silmək istəyirsiniz?", "Elektron ünvanların silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'JP' AND USED_USER_ID = {GlobalVariables.V_UserID} AND ID = " + MailID, "Elektron ünvanlar temp cədvəldən silinmədi.");
            }
        }

        private void DeleteMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteMail();
            LoadMailDataGridView();
        }

        private bool ControlCustomerDetails()
        {
            bool b = false;

            if (RegistrationCodeText.Text.Length == 0)
            {
                RegistrationCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin qeydiyyat nömrəsi daxil edilməyib.");
                RegistrationCodeText.Focus();
                RegistrationCodeText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (FullNameText.Text.Length == 0)
            {
                FullNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin tam adı daxil edilməyib.");
                FullNameText.Focus();
                FullNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (LeadingPersonNameText.Text.Length == 0)
            {
                LeadingPersonNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Rəhbər şəxsin adı daxil edilməyib.");
                LeadingPersonNameText.Focus();
                LeadingPersonNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AddressText.Text.Length == 0)
            {
                AddressText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ünvan daxil edilməyib.");
                AddressText.Focus();
                AddressText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length == 0)
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Vöen daxil edilməyib.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (VoenText.Text.Length > 0 && !GlobalFunctions.Regexp("[0-9]{9}1", VoenText.Text.Trim()))
            {
                VoenText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Hüquqi şəxsin vöen-i yalnız rəqəmlərdən ibarət olmalıdır, ümumi uzunluğu 10 simvol olmalıdır və sonuncu simvol <b><color=104,0,0>1</color></b> olmalıdır.");
                VoenText.Focus();
                VoenText.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.V_CUSTOMERS WHERE CODE = '{RegistrationCodeText.Text}'") > 0)
            {
                RegistrationCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(RegistrationCodeText.Text + " nömrəli müştəri artıq bazaya daxil edilib.");
                RegistrationCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.PHONES_TEMP WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'JP' AND OWNER_ID = {CustomerID} AND USED_USER_ID = {GlobalVariables.V_UserID}") == 0)
            {
                PhoneGridControl.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müştərinin ən azı bir telefon nömrəsi daxil edilməlidir.");
                PhoneGridControl.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertCustomer()
        {
            int max_code_number = GlobalFunctions.GetMax("SELECT NVL(MAX(TO_NUMBER(CODE)),0) FROM CRS_USER.V_CUSTOMERS") + 1;
            string code = null, sql = null;
            if (changecode)
                code = RegistrationCodeText.Text;
            else
                code = max_code_number.ToString().PadLeft(4, '0').Trim();

            sql = $@"INSERT INTO CRS_USER.JURIDICAL_PERSONS(ID,
                                                                CODE,                                                       
                                                                NAME, 
                                                                LEADING_NAME, 
                                                                ADDRESS, 
                                                                VOEN,
                                                                ACCOUNT_NUMBER,
                                                                NOTE,
                                                                IS_BUYER)
                        VALUES({CustomerID},
                                '{code}',
                                '{FullNameText.Text.Trim()}',
                                '{LeadingPersonNameText.Text.Trim()}',
                                '{AddressText.Text.Trim()}',
                                '{VoenText.Text.Trim()}',       
                                '{AccountText.Text.Trim()}',
                                '{NoteText.Text.Trim()}',
                                {1}
                              )
                        RETURNING ID INTO :CustomerID";

            CustomerID = GlobalFunctions.InsertQuery(sql, "Müştərinin məlumatları sistemə daxil edilmədi.", "CustomerID", this.Name + "/InsertCustomer").ToString();
        }

        private void UpdateCustomer()
        {
            string sql = $@"UPDATE CRS_USER.JURIDICAL_PERSONS SET 
                                                NAME = '{FullNameText.Text.Trim()}',
                                                ADDRESS = '{AddressText.Text.Trim()}',
                                                LEADING_NAME = '{LeadingPersonNameText.Text.Trim()}',
                                                VOEN = '{VoenText.Text.Trim()}',
                                                ACCOUNT_NUMBER = '{AccountText.Text.Trim()}'
                                WHERE IS_BUYER = 1 AND ID = {CustomerID} RETURNING ID INTO :CustomerID";
            CustomerID = GlobalFunctions.InsertQuery(sql, "Müştərinin məlumatları sistemdə dəyişdirilmədi.", "CustomerID", this.Name + "/UpdateCustomer").ToString();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlCustomerDetails())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FCustomerSaveWait));
                if (TransactionName == "INSERT")
                    InsertCustomer();
                else
                    UpdateCustomer();

                UpdatePhoneSendSms();
                UpdateMailSend();
                InsertCustomerDetails();
                GlobalProcedures.SplashScreenClose();
                this.Close();
            }
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled && PhoneStandaloneBarDockControl.Enabled)
                LoadFPhoneAddEdit("EDIT", "JP", PhoneID);
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

        private void FJuridicalPersonAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.JURIDICAL_PERSONS", -1, "WHERE ID = " + CustomerID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER_TEMP.PROC_CUSTOMER_DELETE_ALL_TEMP", "P_USED_USER_ID", GlobalVariables.V_UserID, "Məlumatlar temp cədvəldən silinmədi.");
            this.RefreshCustomersDataGridView(RegistrationCodeText.Text.Trim());
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PhonePopupMenu, e);
        }

        private void MailGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(MailGridView, MailPopupMenu, e);
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

        private void VoenText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(VoenText, VoenLengthLabel);
        }

        private void AddressText_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void AccountText_EditValueChanged(object sender, EventArgs e)
        {
            GlobalProcedures.TextEditCharCount(AccountText, AccountLengthLabel);
        }

        private void NewPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("INSERT", "JP", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", "JP", PhoneID);
        }

        void RefreshPhone()
        {
            LoadPhoneDataGridView();
        }

        public void LoadFPhoneAddEdit(string transaction, string OwnerType, string PhoneID)
        {
            FPhoneAddEdit fp = new FPhoneAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerType = OwnerType;
            fp.OwnerID = CustomerID;
            fp.PhoneID = PhoneID;
            fp.RefreshPhonesDataGridView += new FPhoneAddEdit.DoEvent(RefreshPhone);
            fp.ShowDialog();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}