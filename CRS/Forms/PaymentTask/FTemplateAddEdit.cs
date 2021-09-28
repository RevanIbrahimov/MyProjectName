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
using CRS.Class;
using CRS.Class.DataAccess;
using CRS.Class.Tables;

namespace CRS.Forms.PaymentTask
{
    public partial class FTemplateAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FTemplateAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, TemplateID;

        bool CurrentStatus = false,
            TemplateUsed = false,
            isInternal = false;

        int type_id = 0,
            paying_bank_id = 0,
            acceptor_id = 0,
            acceptor_bank_id = 0,
            TemplateUsedUserID = -1;

        List<Banks> lstBanks = null;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void AcceptorLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AcceptorLookUp.EditValue == null)
                return;

            acceptor_id = Convert.ToInt32(AcceptorLookUp.EditValue);
            //List<Acceptor> lstAcceptor = AcceptorDAL.SelectAcceptorByID(acceptor_id).ToList<Acceptor>();            

            GlobalProcedures.FillLookUpEdit(AcceptorBankLookUp, "TASK_ACCEPTOR_BANKS", "ID", "NAME", "ACCEPTOR_ID = " + acceptor_id + " ORDER BY ORDER_ID");
            AcceptorBankLookUp.ItemIndex = 0;

        }

        private void TypeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (TypeLookUp.EditValue == null)
                return;

            type_id = Convert.ToInt32(TypeLookUp.EditValue);
            List<TaskType> lstType = TaskTypeDAL.SelectTypeByID(type_id).ToList<TaskType>();
            isInternal = (lstType.First().IS_INTERNAL == 1);

            if (isInternal)
            {
                acceptor_id = 0;

                AcceptorBankLookUp.Visible = false;
                AcceptorBank2LookUp.Visible = true;
                AcceptorLookUp.EditValue = null;
                AcceptorLookUp.Properties.ReadOnly = true;
                AcceptorLookUp.Properties.NullText = GlobalFunctions.ReadSetting("Company");
                AcceptorLookUp.Properties.Buttons[0].Visible =
                    AcceptorLookUp.Properties.Buttons[1].Visible = false;
                GlobalProcedures.FillLookUpEdit(AcceptorBank2LookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                AcceptorBank2LookUp.Location = new Point(108, 58);
            }
            else
            {
                AcceptorBankLookUp.Visible = true;
                AcceptorBank2LookUp.Visible = false;
                AcceptorLookUp.Properties.ReadOnly = false;
                AcceptorLookUp.Properties.Buttons[0].Visible =
                    AcceptorLookUp.Properties.Buttons[1].Visible = true;
                RefreshAcceptor();
                //GlobalProcedures.FillLookUpEdit(AcceptorLookUp, "TASK_ACCEPTOR", "ID", "ACCEPTOR_NAME", "1 = 1 ORDER BY 1");
                //GlobalProcedures.FillLookUpEdit(AcceptorBankLookUp, "TASK_ACCEPTOR_BANKS", "ID", "NAME", "ACCEPTOR_ID = " + acceptor_id + " ORDER BY ORDER_ID");
            }
        }

        private void FTemplateAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PAYMENT_TASK_TEMPLATES", -1, "WHERE ID = " + TemplateID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        void RefreshType()
        {
            GlobalProcedures.FillLookUpEdit(TypeLookUp, "TASK_TYPE", "ID", "TYPE_NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        private void TypeLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FTaskType ftt = new FTaskType();
            ftt.RefreshTypeDataGridView += new FTaskType.DoEvent(RefreshType);
            ftt.ShowDialog();
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 11:
                    {
                        lstBanks = BanksDAL.SelectBankByID(null).ToList<Banks>();
                        GlobalProcedures.FillLookUpEdit(PayingBankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                        if (isInternal)
                            GlobalProcedures.FillLookUpEdit(AcceptorBank2LookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                    }
                    break;
            }
        }

        private bool ControlDetail()
        {
            bool b = false;

            if (String.IsNullOrWhiteSpace(TemplateNameText.Text))
            {
                TemplateNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şablonun adı daxil edilməyib.");
                TemplateNameText.Focus();
                TemplateNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TransactionName == "INSERT" && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAYMENT_TASK_TEMPLATES WHERE TEMPLATE_NAME = '{TemplateNameText.Text.Trim()}'") > 0)
            {
                TemplateNameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bu şablon adı artıq bazaya daxil edilib. Zəhmət olmasa digər şablon adı daxil edin.");
                TemplateNameText.Focus();
                TemplateNameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TypeLookUp.EditValue == null)
            {
                TypeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tapşırığın növü daxil edilməyib.");
                TypeLookUp.Focus();
                TypeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PayingBankLookUp.EditValue == null)
            {
                PayingBankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Ödəyən bank daxil edilməyib.");
                PayingBankLookUp.Focus();
                PayingBankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!isInternal && AcceptorLookUp.EditValue == null)
            {
                AcceptorLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Alan tərəf daxil edilməyib.");
                AcceptorLookUp.Focus();
                AcceptorLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!isInternal && AcceptorBankLookUp.EditValue == null)
            {
                AcceptorBankLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Alan tərəfin bankı daxil edilməyib.");
                AcceptorBankLookUp.Focus();
                AcceptorBankLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (isInternal && AcceptorBank2LookUp.EditValue == null)
            {
                AcceptorBank2LookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Alan tərəfin bankı daxil edilməyib.");
                AcceptorBank2LookUp.Focus();
                AcceptorBank2LookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertTaskTemplate()
        {
            string sql = $@"INSERT INTO CRS_USER.PAYMENT_TASK_TEMPLATES (TEMPLATE_NAME,
                                                                            TYPE_ID,                                                                
                                                                            PAYING_BANK_ID,  
                                                                            ACCEPTOR_ID,   
                                                                            ACCEPTOR_BANK_ID, 
                                                                            REASON,                                                                        
                                                                            CLASSIFICATION_CODE,
                                                                            LEVEL_CODE,
                                                                            INSERT_USER)
                            VALUES ('{TemplateNameText.Text.Trim()}',
                                        {type_id},                                                                         
                                        {paying_bank_id},   
                                        {acceptor_id},   
                                        {acceptor_bank_id},  
                                        '{ReasonText.Text.Trim()}',                                   
                                        '{ClassificationCodeText.Text.Trim()}',
                                        '{LevelCodeText.Text.Trim()}',
                                        {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Ödəniş tapşırığının şablon forması bazaya daxil edilmədi.");
        }

        private void UpdateTaskTemplate()
        {
            string sql = $@"UPDATE CRS_USER.PAYMENT_TASK_TEMPLATES SET
                                TYPE_ID = {type_id},                                
                                TEMPLATE_NAME = '{TemplateNameText.Text.Trim()}',
                                PAYING_BANK_ID = {paying_bank_id},  
                                ACCEPTOR_ID = {acceptor_id},        
                                ACCEPTOR_BANK_ID = {acceptor_bank_id},  
                                REASON = '{ReasonText.Text.Trim()}',                      
                                CLASSIFICATION_CODE = '{ClassificationCodeText.Text.Trim()}',
                                LEVEL_CODE = '{LevelCodeText.Text.Trim()}',
                                UPDATE_USER = {GlobalVariables.V_UserID},
                                UPDATE_DATE = SYSDATE
                            WHERE ID = {TemplateID}";
            GlobalFunctions.ExecuteQuery(sql, "Ödəniş tapşırığının şablon forması dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetail())
            {
                if (TransactionName == "INSERT")
                    InsertTaskTemplate();
                else
                    UpdateTaskTemplate();
                this.Close();
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

        private void PayingBranchLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        void RefreshAcceptor()
        {
            GlobalProcedures.FillLookUpEdit(AcceptorLookUp, "TASK_ACCEPTOR", "ID", "ACCEPTOR_NAME", "1 = 1 ORDER BY 1");
        }

        private void AcceptorBank2LookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AcceptorBank2LookUp.EditValue == null)
                return;

            acceptor_bank_id = Convert.ToInt32(AcceptorBank2LookUp.EditValue);
        }

        private void AcceptorLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FAcceptors fa = new FAcceptors();
                fa.RefreshAcceptorDataGridView += new FAcceptors.DoEvent(RefreshAcceptor);
                fa.ShowDialog();
            }
        }

        private void PayingBankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (PayingBankLookUp.EditValue == null)
                return;

            paying_bank_id = Convert.ToInt32(PayingBankLookUp.EditValue);
            var bank = lstBanks.Find(b => b.ID == paying_bank_id);

            PayingAccountText.Text = bank.ACCOUNT;
        }

        private void AcceptorBankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AcceptorBankLookUp.EditValue == null)
                return;

            acceptor_bank_id = Convert.ToInt32(AcceptorBankLookUp.EditValue);
        }

        private void FTemplateAddEdit_Load(object sender, EventArgs e)
        {
            PayingNameText.Text = GlobalFunctions.ReadSetting("Company");
            RefreshType();
            RefreshDictionaries(11);

            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PAYMENT_TASK_TEMPLATES", GlobalVariables.V_UserID, "WHERE ID = " + TemplateID + " AND USED_USER_ID = -1");
                TemplateUsedUserID = GlobalFunctions.GetID("SELECT USED_USER_ID FROM CRS_USER.PAYMENT_TASK_TEMPLATES WHERE ID = " + TemplateID);
                TemplateUsed = (TemplateUsedUserID >= 0);

                if (TemplateUsed)
                {
                    if (GlobalVariables.V_UserID != TemplateUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == TemplateUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş şablon forması hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş şablon formanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnable(CurrentStatus);
                LoadTemplateDetails();
            }
        }

        public void ComponentEnable(bool status)
        {
            PropertiesGroupControl.Enabled =
                PayingGroupControl.Enabled =
                AcceptorGroupControl.Enabled =
                TemplateNameText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadTemplateDetails()
        {
            string sql = $@"SELECT PT.TEMPLATE_NAME,
                                           TT.TYPE_NAME,
                                           PT.REASON,
                                           PT.CLASSIFICATION_CODE,
                                           PT.LEVEL_CODE,
                                           B.LONG_NAME PAYING_BANK,
                                           B.ACCOUNT PAYING_ACCOUNT,
                                           AB.ACCEPTOR_NAME,
                                           AB.ACCEPTOR_BANK_NAME ACCEPTOR_BANK
                                      FROM CRS_USER.PAYMENT_TASK_TEMPLATES PT,
                                           CRS_USER.TASK_TYPE TT,
                                           CRS_USER.V_ACCEPTOR_BANK AB,
                                           CRS_USER.BANKS B
                                     WHERE     PT.TYPE_ID = TT.ID
                                           AND PT.PAYING_BANK_ID = B.ID
                                           AND PT.ACCEPTOR_ID = AB.ACCEPTOR_ID
                                           AND PT.ACCEPTOR_BANK_ID = AB.ACCEPTOR_BANK_ID
                                       AND PT.ID = {TemplateID}";
            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                TemplateNameText.Text = dr["TEMPLATE_NAME"].ToString();
                TypeLookUp.EditValue = TypeLookUp.Properties.GetKeyValueByDisplayText(dr["TYPE_NAME"].ToString());
                ReasonText.Text = dr["REASON"].ToString();
                ClassificationCodeText.Text = dr["CLASSIFICATION_CODE"].ToString();
                LevelCodeText.Text = dr["LEVEL_CODE"].ToString();
                PayingBankLookUp.EditValue = PayingBankLookUp.Properties.GetKeyValueByDisplayText(dr["PAYING_BANK"].ToString());
                PayingAccountText.Text = dr["PAYING_ACCOUNT"].ToString();

                if (!isInternal)
                {
                    AcceptorLookUp.EditValue = AcceptorLookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_NAME"].ToString());
                    AcceptorBankLookUp.EditValue = AcceptorBankLookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_BANK"].ToString());
                }
                else
                    AcceptorBank2LookUp.EditValue = AcceptorBank2LookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_BANK"].ToString());
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}