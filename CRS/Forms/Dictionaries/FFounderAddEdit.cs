using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using System.Collections;
using CRS.Class;

namespace CRS.Forms.Dictionaries
{
    public partial class FFounderAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FFounderAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, FounderID;
        bool CurrentStatus = false, FounderUsed = false;
        int FounderUsedUserID = -1;
        string CardID, PhoneID, PhoneNumber, MailID;

        public delegate void DoEvent();
        public event DoEvent RefreshFoundersDataGridView;

        private void FFounderAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
                FounderID = GlobalFunctions.GetOracleSequenceValue("FOUNDER_SEQUENCE").ToString();
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", GlobalVariables.V_UserID, "WHERE ID = " + FounderID + " AND USED_USER_ID = -1");
                LoadFounderDetails();
                FounderUsed = (FounderUsedUserID >= 0);
                
                if (FounderUsed)
                {
                    if (GlobalVariables.V_UserID != FounderUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == FounderUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş təsisçi hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş təsisçinin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                InsertDetailsTemp();                
                LoadCardsDataGridView();   
            }
        }

        private void ComponentEnabled(bool status)
        {
            FounderFullNameText.Enabled = 
                NoteText.Enabled = 
                CardStandaloneBarDockControl.Enabled = 
                PhoneStandaloneBarDockControl.Enabled = 
                MailStandaloneBarDockControl.Enabled = 
                BOK.Visible = !status;

            if (status)
            {
                IDCardPopupMenu.Manager = null;
                PhonePopupMenu.Manager = null;
                MailPopupMenu.Manager = null;
            }
            else
            {
                IDCardPopupMenu.Manager = IDCardBarManager;
                PhonePopupMenu.Manager = PhoneBarManager;
                MailPopupMenu.Manager = MailBarManager;
            }            
        }

        private void LoadFounderDetails()
        {
            string s = $@"SELECT FULLNAME,NOTE,USED_USER_ID FROM CRS_USER.FOUNDERS WHERE ID = {FounderID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFounderDetails", "Təsisçinin parametrləri açılmadı.");

            if(dt.Rows.Count > 0)
            {
                FounderFullNameText.Text = dt.Rows[0]["FULLNAME"].ToString();
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                FounderUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }
        }

        private void CardsGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(CardsGridView, IDCardPopupMenu, e);
        }

        private void PhoneGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(PhoneGridView, PhonePopupMenu, e);
        }

        private void MailGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(MailGridView, MailPopupMenu, e);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ControlFounderDetails()
        {
            bool b = false;

            if (FounderFullNameText.Text.Length == 0)
            {
                FounderFullNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təsisçinin tam adı daxil edilməyib.");
                FounderFullNameText.Focus();
                FounderFullNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CardsGridView.RowCount == 0)
            {
                OtherTabControl.SelectedTabPageIndex = 0;
                GlobalProcedures.ShowErrorMessage("Təsisçinin şəxsiyyətini təsdiq edən sənəd daxil edilməyib.");
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertFounder()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.FOUNDERS") + 1;
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.FOUNDERS(ID,FULLNAME,NOTE,ORDER_ID) VALUES(" + FounderID + ",'" + FounderFullNameText.Text.Trim() + "','" + NoteText.Text.Trim() + "'," + order + ")",
                                                "Təsisçi daxil edilmədi.");
        }

        private void UpdateFounder()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.FOUNDERS SET FULLNAME ='" + FounderFullNameText.Text.Trim() + "',NOTE = '" + NoteText.Text.Trim() + "' WHERE ID = " + FounderID,
                                                "Təsisçi dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlFounderDetails())
            {
                if (TransactionName == "INSERT")
                    InsertFounder();
                else
                    UpdateFounder();
                UpdatePhoneSendSms();
                UpdateMailSend();
                InsertDetails();
                this.Close();
            }
        }

        private void LoadCardsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                                     CC.ID,
                                     CS.NAME || ': ' || CS.SERIES || ' - ' || CC.CARD_NUMBER,
                                     TO_CHAR(CC.ISSUE_DATE, 'DD.MM.YYYY'),
                                     CI.NAME,
                                     TO_CHAR(CC.RELIABLE_DATE, 'DD.MM.YYYY'),
                                     CC.ADDRESS,
                                     CC.REGISTRATION_ADDRESS
                                FROM CRS_USER_TEMP.FOUNDER_CARDS_TEMP CC,
                                     CRS_USER.CARD_SERIES CS,
                                     CRS_USER.CARD_ISSUING CI
                               WHERE CC.CARD_SERIES_ID = CS.ID
                                     AND CC.CARD_ISSUING_ID = CI.ID
                                     AND CC.FOUNDER_ID = {FounderID}
                            ORDER BY CC.ID";
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
                                
                if (CardsGridView.RowCount > 0)
                {
                    EditCardBarButton.Enabled = DeleteCardBarButton.Enabled = true;
                    CardsGridView.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "{0:n0}");
                }
                else
                {
                    EditCardBarButton.Enabled = DeleteCardBarButton.Enabled = false;
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

        public void LoadFCardAddEdit(string transaction, string FounderID, string CardID)
        {
            Customer.FCardAddEdit fp = new Customer.FCardAddEdit();
            fp.TransactionName = transaction;
            fp.CardID = CardID;
            fp.OwnerID = FounderID;
            fp.OwnerType = "F";
            fp.RefreshCardsDataGridView += new Customer.FCardAddEdit.DoEvent(RefreshCards);
            fp.ShowDialog();
        }

        private void NewCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardAddEdit("INSERT", FounderID, null);
        }

        private void CardsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = CardsGridView.GetFocusedDataRow();
            if (row != null)
                CardID = row["ID"].ToString();
        }

        private void CardsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
        
        private void EditCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFCardAddEdit("EDIT", FounderID, CardID);
        }

        private void CardsGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditCardBarButton.Enabled) && (CardStandaloneBarDockControl.Enabled))
                LoadFCardAddEdit("EDIT", FounderID, CardID);
        }

        private void InsertDetailsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INSERT_FOUNDER_DATA_TEMP", "P_FOUNDER_ID", int.Parse(FounderID), "Təsisçinin məlumatları temp cədvələ daxil edilmədi.");            
        }

        private void InsertDetails()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER.PROC_INSERT_FOUNDER_DETAILS", "P_FOUNDER_ID", int.Parse(FounderID), "Təsisçinin məlumatları əsəs cədvələ daxil edilmədi.");            
        }
        
        private void DeleteCard()
        {
            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM (SELECT FOUNDER_CARD_ID FROM CRS_USER.FOUNDER_CONTRACTS UNION SELECT FOUNDER_CARD_ID FROM CRS_USER.CASH_FOUNDER) WHERE FOUNDER_CARD_ID = {CardID}", this.Name + "/DeleteCard");
            if (a == 0)
            {
                DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş sənədi silmək istəyirsiniz?", "Şəxsiyyəti təsdiq edən sənədin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.FOUNDER_CARDS_TEMP WHERE ID = {CardID}", "Şəxsiyyəti təsdiq edən sənəd silinmədi.", this.Name + "/DeleteCard");
                }
            }
            else
                XtraMessageBox.Show("Seçilmiş sənəd bazada digər məlumatlarda istifadə olunduğu üçün silinə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteCard();
            LoadCardsDataGridView();
        }

        private void PhoneGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = PhoneGridView.GetFocusedDataRow();
            if (row != null)
            {
                PhoneID = row["ID"].ToString();
                PhoneNumber = row["PHONENUMBER"].ToString();
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
                                 AND P.OWNER_TYPE = 'F'
                                 AND P.OWNER_ID = {FounderID}
                        ORDER BY P.ORDER_ID";
            try
            {
                PhoneGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadPhoneDataGridView");
                if (PhoneGridView.RowCount > 0)
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled = true;
                else
                    EditPhoneBarButton.Enabled =
                        DeletePhoneBarButton.Enabled = false;
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
                             WHERE IS_CHANGE IN (0, 1) AND OWNER_TYPE = 'F' AND OWNER_ID = {FounderID}";
            try
            {
                MailGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadMailDataGridView", "Maillər cədvələ yüklənmədi.");
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
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_SEND_SMS = 0 WHERE IS_SEND_SMS = 1 AND OWNER_TYPE = 'F' AND OWNER_ID = {FounderID} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
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

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'F' AND OWNER_ID = {FounderID} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
                                                "Telefon nömrələri dəyişdirilmədi.",
                                            this.Name + "/UpdatePhoneSendSms");
        }

        private void UpdateMailSend()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_SEND = 0 WHERE IS_SEND = 1 AND OWNER_TYPE = 'F' AND OWNER_ID = {FounderID} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
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

            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 1 WHERE IS_CHANGE <> 2 AND OWNER_TYPE = 'F' AND OWNER_ID = {FounderID} AND USED_USER_ID = {GlobalVariables.V_UserID}", 
                                                "Maillər dəyişdirilmədi.",
                                                this.Name + "/UpdateMailSend");
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
            LoadFPhoneAddEdit("INSERT", FounderID, "F", null);
        }

        private void EditPhoneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFPhoneAddEdit("EDIT", FounderID, "F", PhoneID);
        }

        private void DeletePhone()
        {
            DialogResult dialogResult = XtraMessageBox.Show(PhoneNumber + " nömrəsini silmək istəyirsiniz?", "Telefon nömrəsinin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.PHONES_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'F' AND OWNER_ID = " + FounderID + " AND ID = " + PhoneID, "Telefon nömrəsi temp cədvəldən silinmədi.");
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
            FMailAddEdit fp = new Forms.FMailAddEdit();
            fp.TransactionName = transaction;
            fp.OwnerID = OwnerID;
            fp.OwnerType = OwnerType;
            fp.MailID = MailID;
            fp.RefreshEmailDataGridView += new FMailAddEdit.DoEvent(RefreshMail);
            fp.ShowDialog();
        }

        private void NewMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("INSERT", FounderID, "F", null);
        }

        private void EditMailBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFMailAddEdit("EDIT", FounderID, "F", MailID);
        }

        private void MailGridView_DoubleClick(object sender, EventArgs e)
        {
            if ((EditMailBarButton.Enabled) && (MailStandaloneBarDockControl.Enabled))
                LoadFMailAddEdit("EDIT", FounderID, "F", MailID);
        }

        private void DeleteMail()
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş elektron ünvanları silmək istəyirsiniz?", "Elektron ünvanların silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.MAILS_TEMP SET IS_CHANGE = 2 WHERE OWNER_TYPE = 'F' AND OWNER_ID = " + FounderID + " AND ID = " + MailID, "Elektron ünvanlar temp cədvəldən silinmədi.");
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

        private void DeleteAllTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_FOUNDER_TEMP_DELETE", "P_FOUNDER_ID", int.Parse(FounderID), "Təsisçinin məlumatları temp cədvəllərdən silinmədi.");            
        }

        private void PhoneGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditPhoneBarButton.Enabled && PhoneStandaloneBarDockControl.Enabled)
                LoadFPhoneAddEdit("EDIT", FounderID, "F", PhoneID);
        }

        private void FFounderAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.FOUNDERS", -1, "WHERE ID = " + FounderID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            DeleteAllTemp();
            this.RefreshFoundersDataGridView();
        }

        private void OtherTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            LoadPhoneDataGridView();
            LoadMailDataGridView();
        }

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 400)
                FounderDescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (400 - NoteText.Text.Length).ToString();
        }

        private void RefreshCardBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCardsDataGridView();
        }

        private void CardsGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditCardBarButton.Enabled = DeleteCardBarButton.Enabled = (CardsGridView.RowCount > 0);
        }

        private void PhoneGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditPhoneBarButton.Enabled = DeletePhoneBarButton.Enabled = (PhoneGridView.RowCount > 0);
        }

        private void MailGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditMailBarButton.Enabled = DeleteMailBarButton.Enabled = (MailGridView.RowCount > 0);
        }
    }
}