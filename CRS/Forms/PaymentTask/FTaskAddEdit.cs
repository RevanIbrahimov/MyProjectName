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
using DevExpress.XtraReports.UI;
using CRS.Class;
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.PaymentTask
{
    public partial class FTaskAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FTaskAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, TaskID, TaskType, Reason = null;
        public decimal Amount;

        int type_id = 0,
            old_type_id = 0,
            currency_id,
            paying_bank_id,
            acceptor_id = 0,
            acceptor_bank_id = 0,
            task_number = 1,
            TaskUsedUserID = -1;

        string paying_bank_name,
            paying_bank_voen,
            paying_bank_swift,
            paying_bank_code,
            paying_account,
            paying_bank_cbar_account,
            acceptor_bank_name,
            acceptor_bank_name_vat,
            acceptor_bank_voen,
            acceptor_bank_voen_vat,
            acceptor_bank_swift,
            acceptor_bank_swift_vat,
            acceptor_bank_code,
            acceptor_bank_code_vat,
            acceptor_account,
            acceptor_account_vat,
            acceptor_bank_cbar_account,
            acceptor_bank_cbar_account_vat,
            type_code,
            acceptor_voen;

        bool CurrentStatus = false,
            TaskUsed = false,
            changecode = false,
            isInternal = false,
            clickOK = false;

        List<Banks> lstBanks = null;

        public delegate void DoEvent(int taskID, DateTime taskDate, bool clickOK);
        public event DoEvent RefreshDataGridView;

        private void FTaskAddEdit_Load(object sender, EventArgs e)
        {
            DateValue.EditValue = DateTime.Today;
            PayingNameText.Text = GlobalFunctions.ReadSetting("Company");
            RefreshDictionaries(7);
            CurrencyLookUp.EditValue = CurrencyLookUp.Properties.GetKeyValueByDisplayText("AZN");
            GlobalProcedures.FillLookUpEdit(PayingBankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
            RefreshType();
            lstBanks = BanksDAL.SelectBankByID(null).ToList<Banks>();
            RefreshTemplate();

            if (TransactionName == "INSERT")
            {
                TaskID = GlobalFunctions.GetOracleSequenceValue("PAYMENT_TASKS_SEQUENCE").ToString();
                AmountValue.EditValue = Amount;
                ReasonText.Text = ReasonText.Text + Reason;
                GlobalProcedures.LookUpEditValue(TypeLookUp, TaskType);
            }
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PAYMENT_TASKS", GlobalVariables.V_UserID, "WHERE ID = " + TaskID + " AND USED_USER_ID = -1");
                LoadTaskDetails();
                TaskUsed = (TaskUsedUserID >= 0);

                if (TaskUsed)
                {
                    if (GlobalVariables.V_UserID != TaskUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == TaskUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş ödəniş tapşırığının məlumatları hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş tapşırığın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnable(CurrentStatus);
            }
        }

        public void ComponentEnable(bool status)
        {
            PropertiesGroupControl.Enabled =
                PayingGroupControl.Enabled =
                AcceptorGroupControl.Enabled =
                TemplateCheck.Enabled =
                BOK.Visible = !status;
        }

        private void LoadTaskDetails()
        {
            string sql = $@"SELECT TT.TYPE_NAME,
                                   PT.CLASSIFICATION_CODE,
                                   PT.LEVEL_CODE,
                                   PT.CODE,
                                   PT.TASK_NUMBER,
                                   PT.TDATE,
                                   PT.AMOUNT,
                                   C.CODE CURRENCY_CODE,
                                   PT.EDV,
                                   PT.REASON,
                                   B.LONG_NAME BANK_FULLNAME,
                                   AB.ACCEPTOR_NAME,
                                   AB.ACCEPTOR_BANK_NAME,
                                   PT.TYPE_ID,
                                   PT.USED_USER_ID,
                                   (SELECT COUNT(*) FROM CRS_USER.INSURANCE_TRANSFER WHERE PAYMENT_TASK_ID = PT.ID) INSURANCE_TRANSFER_COUNT
                              FROM CRS_USER.PAYMENT_TASKS PT,
                                   CRS_USER.TASK_TYPE TT,
                                   CRS_USER.CURRENCY C,
                                   CRS_USER.BANKS B,
                                   CRS_USER.V_ACCEPTOR_BANK AB
                             WHERE     PT.CURRENCY_ID = C.ID
                                   AND PT.TYPE_ID = TT.ID
                                   AND PT.PAYING_BANK_ID = B.ID
                                   AND PT.ACCEPTOR_ID = AB.ACCEPTOR_ID
                                   AND PT.ACCEPTOR_BANK_ID = AB.ACCEPTOR_BANK_ID
                                   AND PT.ID = {TaskID}";
            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                old_type_id = int.Parse(dr["TYPE_ID"].ToString());
                TypeLookUp.EditValue = TypeLookUp.Properties.GetKeyValueByDisplayText(dr["TYPE_NAME"].ToString());
                ClassificationCodeText.Text = dr["CLASSIFICATION_CODE"].ToString();
                LevelCodeText.Text = dr["LEVEL_CODE"].ToString();
                CodeText.Text = dr["CODE"].ToString();
                task_number = int.Parse(dr["TASK_NUMBER"].ToString());
                DateValue.EditValue = DateTime.Parse(dr["TDATE"].ToString());
                AmountValue.Value = Convert.ToDecimal(dr["AMOUNT"]);
                CurrencyLookUp.EditValue = CurrencyLookUp.Properties.GetKeyValueByDisplayText(dr["CURRENCY_CODE"].ToString());
                EDVCheck.Checked = (Convert.ToDouble(dr["EDV"]) > 0);
                ReasonText.Text = dr["REASON"].ToString();
                PayingBankLookUp.EditValue = PayingBankLookUp.Properties.GetKeyValueByDisplayText(dr["BANK_FULLNAME"].ToString());

                if (!isInternal)
                {
                    AcceptorLookUp.EditValue = AcceptorLookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_NAME"].ToString());
                    AcceptorBankLookUp.EditValue = AcceptorBankLookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_BANK_NAME"].ToString());
                }
                else
                    AcceptorBank2LookUp.EditValue = AcceptorBank2LookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_BANK_NAME"].ToString());
                TaskUsedUserID = Convert.ToInt16(dr["USED_USER_ID"]);

                AmountValue.Enabled = TypeLookUp.Enabled = CurrencyLookUp.Enabled = Convert.ToInt16(dr["INSURANCE_TRANSFER_COUNT"]) == 0;
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void RefreshType()
        {
            GlobalProcedures.FillLookUpEdit(TypeLookUp, "TASK_TYPE", "ID", "TYPE_NAME", "1 = 1 ORDER BY ORDER_ID");
        }

        void RefreshCode(string code, int number, bool close)
        {
            changecode = close;
            task_number = number;
            if (close)
                CodeText.Text = code;
        }

        private void TemplateCheck_CheckedChanged(object sender, EventArgs e)
        {
            TemplateLookUp.Enabled = (TemplateCheck.Checked);

            if (!TemplateCheck.Checked)
            {
                TypeLookUp.EditValue =
                    PayingBankLookUp.EditValue =
                    AcceptorLookUp.EditValue =
                    AcceptorBankLookUp.EditValue =
                    TemplateLookUp.EditValue = null;
                ClassificationCodeText.Text = null;
                LevelCodeText.Text = "1";
            }

            TypeLookUp.Enabled =
                PayingBankLookUp.Enabled =
                AcceptorLookUp.Enabled =
                AcceptorBankLookUp.Enabled = !(TemplateCheck.Checked);
        }

        void RefreshTemplate()
        {
            GlobalProcedures.FillLookUpEdit(TemplateLookUp, "PAYMENT_TASK_TEMPLATES", "ID", "TEMPLATE_NAME", "1 = 1 ORDER BY TEMPLATE_NAME");
            RefreshDictionaries(11);
            RefreshType();
            RefreshAcceptor();
            LoadTemlateDetails();
        }

        private void TemplateLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FTemplates ft = new FTemplates();
                ft.RefreshData += new FTemplates.DoEvent(RefreshTemplate);
                ft.ShowDialog();
            }
        }

        private void TemplateLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (TemplateLookUp.EditValue == null)
                return;

            LoadTemlateDetails();
        }

        private void LoadTemlateDetails()
        {
            string sql = $@"SELECT TT.TYPE_NAME,
                                   PT.REASON,
                                   PT.CLASSIFICATION_CODE,
                                   PT.LEVEL_CODE,
                                   B.LONG_NAME PAYING_BANK,
                                   AB.ACCEPTOR_NAME,
                                   AB.ACCEPTOR_BANK_NAME ACCEPTOR_BANK,
                                   AB.ACCEPTOR_ACCOUNT
                              FROM CRS_USER.PAYMENT_TASK_TEMPLATES PT,
                                   CRS_USER.TASK_TYPE TT,
                                   CRS_USER.V_ACCEPTOR_BANK AB,
                                   CRS_USER.BANKS B
                             WHERE     PT.TYPE_ID = TT.ID
                                   AND PT.PAYING_BANK_ID = B.ID
                                   AND PT.ACCEPTOR_ID = AB.ACCEPTOR_ID
                                   AND PT.ACCEPTOR_BANK_ID = AB.ACCEPTOR_BANK_ID
                                   AND PT.ID = {Convert.ToInt32(TemplateLookUp.EditValue)}";
            foreach (DataRow dr in GlobalFunctions.GenerateDataTable(sql).Rows)
            {
                TypeLookUp.EditValue = TypeLookUp.Properties.GetKeyValueByDisplayText(dr["TYPE_NAME"].ToString());
                ReasonText.Text = dr["REASON"].ToString() + " " + Reason;
                ClassificationCodeText.Text = dr["CLASSIFICATION_CODE"].ToString();
                LevelCodeText.Text = dr["LEVEL_CODE"].ToString();
                PayingBankLookUp.EditValue = PayingBankLookUp.Properties.GetKeyValueByDisplayText(dr["PAYING_BANK"].ToString());

                if (!isInternal)
                {
                    AcceptorLookUp.EditValue = AcceptorLookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_NAME"].ToString());
                    AcceptorBankLookUp.EditValue = AcceptorBankLookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_BANK"].ToString());
                }
                else
                    AcceptorBank2LookUp.EditValue = AcceptorBank2LookUp.Properties.GetKeyValueByDisplayText(dr["ACCEPTOR_BANK"].ToString());
            }
        }

        private void PayingBankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (PayingBankLookUp.EditValue == null)
                return;

            paying_bank_id = Convert.ToInt32(PayingBankLookUp.EditValue);
            LoadPayingBankDetails();
        }

        private void PayingBankLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 11);
        }

        private void LoadAcceptorBank2Details()
        {
            if (acceptor_bank_id == 0)
                return;

            var acceptor_bank = lstBanks.Find(b => b.ID == acceptor_bank_id);

            acceptor_bank_name = acceptor_bank.LONG_NAME;
            acceptor_bank_code = acceptor_bank.CODE;
            acceptor_bank_swift = acceptor_bank.SWIFT;
            acceptor_bank_voen = acceptor_bank.VOEN;
            acceptor_bank_cbar_account = acceptor_bank.CBAR_ACCOUNT;
            acceptor_account = acceptor_bank.ACCOUNT;
        }

        private void AcceptorBank2LookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AcceptorBank2LookUp.EditValue == null)
                return;

            acceptor_bank_id = Convert.ToInt32(AcceptorBank2LookUp.EditValue);
            LoadAcceptorBank2Details();
        }

        private void LoadPayingBankDetails()
        {
            if (paying_bank_id == 0)
                return;

            var bank = lstBanks.Find(b => b.ID == paying_bank_id);
            paying_bank_name = bank.LONG_NAME;
            paying_bank_swift = bank.SWIFT;
            paying_bank_voen = bank.VOEN;
            paying_bank_cbar_account = bank.CBAR_ACCOUNT;
            paying_account = bank.ACCOUNT;
            paying_bank_code = bank.CODE;
        }

        private void CodeText_EditValueChanged(object sender, EventArgs e)
        {
            EDVCodeText.Text = CodeText.Text.Trim().Substring(0, 1) + "V" + CodeText.Text.Trim().Substring(1);
        }

        private void AcceptorBankLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AcceptorBankLookUp.EditValue == null)
                return;

            acceptor_bank_id = Convert.ToInt32(AcceptorBankLookUp.EditValue);
            LoadAcceptorBankDetails();
        }

        private void LoadAcceptorBankDetails()
        {
            if (acceptor_bank_id == 0)
                return;

            List<AcceptorBank> lstAcceptorBanks = AcceptorBankDAL.SelectBankByID(acceptor_bank_id).ToList<AcceptorBank>();
            var acceptor_bank = lstAcceptorBanks.First();

            acceptor_bank_name = acceptor_bank.NAME;
            acceptor_bank_code = acceptor_bank.CODE;
            acceptor_bank_swift = acceptor_bank.SWIFT;
            acceptor_bank_voen = acceptor_bank.VOEN;
            acceptor_bank_cbar_account = acceptor_bank.CBAR_ACCOUNT;
            acceptor_account = acceptor_bank.ACCEPTOR_ACCOUNT;
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

        private void AcceptorLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (AcceptorLookUp.EditValue == null)
                return;

            acceptor_id = Convert.ToInt32(AcceptorLookUp.EditValue);
            LoadAcceptorDetails();

            GlobalProcedures.FillLookUpEdit(AcceptorBankLookUp, "TASK_ACCEPTOR_BANKS", "ID", "NAME", "ACCEPTOR_ID = " + acceptor_id + " ORDER BY ORDER_ID");
            AcceptorBankLookUp.ItemIndex = 0;
        }

        private void LoadAcceptorDetails()
        {
            if (acceptor_id == 0)
                return;

            List<Acceptor> lstAcceptor = AcceptorDAL.SelectAcceptorByID(acceptor_id).ToList<Acceptor>();
            var acceptor = lstAcceptor.First();
            acceptor_voen = acceptor.ACCEPTOR_VOEN;
            acceptor_account_vat = acceptor.VAT_ACCOUNT;
        }

        private void DateValue_EditValueChanged(object sender, EventArgs e)
        {
            if (old_type_id != type_id)
                CodeText.Text = InsertTaskCodeTemp();
            EDVCodeLabel.Visible = EDVCodeText.Visible = (EDVCheck.Checked);
        }

        private void CurrencyLookUp_EditValueChanged(object sender, EventArgs e)
        {
            currency_id = Convert.ToInt32(CurrencyLookUp.EditValue);
        }

        private void CurrencyLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 7);
        }

        private void TypeLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FTaskType ftt = new FTaskType();
                ftt.RefreshTypeDataGridView += new FTaskType.DoEvent(RefreshType);
                ftt.ShowDialog();
            }
        }

        private void TypeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (TypeLookUp.EditValue == null)
                return;

            type_id = Convert.ToInt32(TypeLookUp.EditValue);
            List<TaskType> lstType = TaskTypeDAL.SelectTypeByID(type_id).ToList<TaskType>();
            type_code = lstType.First().CODE;
            isInternal = (lstType.First().IS_INTERNAL == 1);
            if (old_type_id != type_id)
                CodeText.Text = InsertTaskCodeTemp();

            BChangeCode.Visible = (type_id > 0 && old_type_id != type_id);

            if (isInternal)
            {
                acceptor_id = 0;
                acceptor_voen = GlobalFunctions.ReadSetting("CompanyVoen");
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
                GlobalProcedures.FillLookUpEdit(AcceptorLookUp, "TASK_ACCEPTOR", "ID", "ACCEPTOR_NAME", "1 = 1 ORDER BY 1");
                GlobalProcedures.FillLookUpEdit(AcceptorBankLookUp, "TASK_ACCEPTOR_BANKS", "ID", "NAME", "ACCEPTOR_ID = " + acceptor_id + " ORDER BY ORDER_ID");
            }
        }

        private void BChangeCode_Click(object sender, EventArgs e)
        {
            FChangeCode fc = new FChangeCode();
            fc.TypeName = TypeLookUp.Text;
            fc.TypeID = type_id;
            fc.TypeCode = type_code;
            fc.TaskDate = DateValue.DateTime;
            fc.RefreshCode += new FChangeCode.DoEvent(RefreshCode);
            fc.ShowDialog();
        }

        private void FTaskAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PAYMENT_TASKS", -1, "WHERE ID = " + TaskID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.TASK_CODE_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID}", "Tapşırığın nömrəsi temp cədvəldən silinmədi.");
            this.RefreshDataGridView(int.Parse(TaskID), DateValue.DateTime, clickOK);
        }

        void RefreshAcceptor()
        {
            if (type_id == 0)
                return;

            GlobalProcedures.FillLookUpEdit(AcceptorLookUp, "TASK_ACCEPTOR", "ID", "ACCEPTOR_NAME", "1 = 1 ORDER BY 1");
            LoadAcceptorDetails();
            GlobalProcedures.FillLookUpEdit(AcceptorBankLookUp, "TASK_ACCEPTOR_BANKS", "ID", "NAME", "ACCEPTOR_ID = " + acceptor_id + " ORDER BY ORDER_ID");
            LoadAcceptorBankDetails();
        }

        private bool InsertTask()
        {
            decimal edv = 0;

            if (EDVCheck.Checked)
                edv = Math.Round((AmountValue.Value * 18) / 100, 2);

            int max_task_number = 1;
            string code = null;
            if (changecode)
                code = CodeText.Text;
            else
            {
                max_task_number = GlobalFunctions.GetMax($@"SELECT NVL(MAX(TASK_NUMBER),0) FROM CRS_USER.PAYMENT_TASKS WHERE EXTRACT(YEAR FROM TDATE) = EXTRACT(YEAR FROM SYSDATE) AND TYPE_ID = {type_id}") + 1;
                code = type_code + max_task_number + "-" + DateTime.Today.Year.ToString().Substring(2, 2);
                task_number = max_task_number;
            }

            string sql = $@"INSERT INTO CRS_USER.PAYMENT_TASKS (ID,
                                                                TYPE_ID,
                                                                CODE,
                                                                TASK_NUMBER,
                                                                TDATE,
                                                                AMOUNT,
                                                                CURRENCY_ID,
                                                                EDV,
                                                                REASON,
                                                                PAYING_BANK_ID,                                                                
                                                                ACCEPTOR_ID,
                                                                ACCEPTOR_BANK_ID,
                                                                CLASSIFICATION_CODE,
                                                                LEVEL_CODE,
                                                                INSERT_USER)
                            VALUES (
                                        {TaskID},
                                        {type_id},
                                        '{code}',
                                        {task_number},
                                        TO_DATE('{DateValue.Text}','DD.MM.YYYY'),
                                        {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                        {currency_id},
                                        {edv.ToString(GlobalVariables.V_CultureInfoEN)},
                                        '{ReasonText.Text.Trim()}',
                                        {paying_bank_id}, 
                                        {acceptor_id},
                                        {acceptor_bank_id},                                        
                                        '{ClassificationCodeText.Text.Trim()}',
                                        '{LevelCodeText.Text.Trim()}',
                                        {GlobalVariables.V_UserID}
                                   )";

            return (GlobalFunctions.ExecuteQuery(sql, "Ödəniş tapşırığı bazaya daxil edilmədi.") > 0);
        }

        private bool UpdateTask()
        {
            decimal edv = 0;

            if (EDVCheck.Checked)
                edv = Math.Round((AmountValue.Value * 18) / 100, 2);

            string sql = $@"UPDATE CRS_USER.PAYMENT_TASKS SET
                                TYPE_ID = {type_id},
                                CODE = '{CodeText.Text.Trim()}',
                                TASK_NUMBER = {task_number},
                                TDATE = TO_DATE('{DateValue.Text}','DD.MM.YYYY'),
                                AMOUNT = {AmountValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                CURRENCY_ID = {currency_id},
                                EDV = {edv.ToString(GlobalVariables.V_CultureInfoEN)},
                                REASON = '{ReasonText.Text.Trim()}',
                                PAYING_BANK_ID = {paying_bank_id},                               
                                ACCEPTOR_ID = {acceptor_id},  
                                ACCEPTOR_BANK_ID = {acceptor_bank_id},  
                                CLASSIFICATION_CODE = '{ClassificationCodeText.Text.Trim()}',
                                LEVEL_CODE = '{LevelCodeText.Text.Trim()}',
                                UPDATE_USER = {GlobalVariables.V_UserID},
                                UPDATE_DATE = SYSDATE
                            WHERE ID = {TaskID}",
                     sqlInsuranceTransfer = $@"UPDATE CRS_USER.INSURANCE_TRANSFER SET TRANSFER_DATE = TO_DATE('{DateValue.Text}','DD.MM.YYYY') WHERE PAYMENT_TASK_ID = {TaskID}";
            return (GlobalFunctions.ExecuteTwoQuery(sql, sqlInsuranceTransfer, "Ödəniş tapşırığı dəyişdirilmədi.") > 0);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetail())
            {
                GlobalProcedures.SplashScreenShow(this, typeof(WaitForms.FWait));                
                bool execute = false;
                if (TransactionName == "INSERT")
                    execute = InsertTask();
                else
                    execute = UpdateTask();
                GlobalProcedures.SplashScreenClose();
                if (execute)
                {
                    if (!EDVCheck.Checked)
                        CreatePrintDocument();
                    else
                        CreatePrintDocumentWithEDV();
                    clickOK = true;
                    this.Close();
                }                
            }
        }

        private void CreatePrintDocument()
        {
            string amount_with_string, date = GlobalFunctions.DateWithDayMonthYear(DateValue.DateTime);

            amount_with_string = GlobalFunctions.AmountInWritining((double)AmountValue.Value, currency_id, true);

            Task report = new Task();
            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
            report.ShowPrintMarginsWarning = false;
            report.RequestParameters = false;

            report.Parameters["PSs"].Value = "Ödəniş tapşırığı № " + CodeText.Text.Trim();
            report.Parameters["PCode"].Value = ClassificationCodeText.Text.Trim();
            report.Parameters["PCode2"].Value = LevelCodeText.Text.Trim();
            report.Parameters["PDate"].Value = date;
            report.Parameters["PAmount"].Value = AmountValue.Value.ToString("N2");
            report.Parameters["PAmountWithString"].Value = amount_with_string;
            report.Parameters["PReason"].Value = ReasonText.Text;
            report.Parameters["PCurrency"].Value = CurrencyLookUp.Text;
            report.Parameters["PPayingCustomer"].Value = GlobalFunctions.ReadSetting("Company");
            report.Parameters["PPayingAccount"].Value = paying_account;
            report.Parameters["PPayingVoen"].Value = GlobalFunctions.ReadSetting("CompanyVoen");
            report.Parameters["PAcceptor"].Value = AcceptorLookUp.Text;
            report.Parameters["PAcceptorAccount"].Value = acceptor_account;
            report.Parameters["PAcceptorVoen"].Value = acceptor_voen;
            report.Parameters["PPayingBank"].Value = paying_bank_name;
            report.Parameters["PAcceptorBank"].Value = acceptor_bank_name;
            report.Parameters["PPayingBankCode"].Value = paying_bank_code;
            report.Parameters["PAcceptorBankCode"].Value = acceptor_bank_code;
            report.Parameters["PPayingBankVoen"].Value = paying_bank_voen;
            report.Parameters["PAcceptorBankVoen"].Value = acceptor_bank_voen;
            report.Parameters["PPayingBankSwift"].Value = paying_bank_swift;
            report.Parameters["PAcceptorBankSwift"].Value = acceptor_bank_swift;
            report.Parameters["PPayingBankAccount"].Value = paying_bank_cbar_account;
            report.Parameters["PAcceptorBankAccount"].Value = acceptor_bank_cbar_account;
            new ReportPrintTool(report).ShowPreview();
            report.PrintingSystem.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.Parameters, new object[] { true });
        }

        private void CreatePrintDocumentWithEDV()
        {
            string amount_with_string, amount_edv_with_string,
                    date = GlobalFunctions.DateWithDayMonthYear(DateValue.DateTime);

            decimal edv = (EDVCheck.Checked) ? Math.Round((AmountValue.Value * 18) / 100, 2) : 0;

            amount_with_string = GlobalFunctions.AmountInWritining((double)AmountValue.Value, currency_id, true);
            amount_edv_with_string = GlobalFunctions.AmountInWritining((double)edv, currency_id, true);
            LoadVatBankDetails();

            TaskWithEDV report = new TaskWithEDV();
            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
            report.ShowPrintMarginsWarning = false;
            report.RequestParameters = false;

            report.Parameters["PSs"].Value = "Ödəniş tapşırığı № " + CodeText.Text.Trim();
            report.Parameters["PSsV"].Value = "Ödəniş tapşırığı № " + EDVCodeText.Text.Trim();
            report.Parameters["PCode"].Value = ClassificationCodeText.Text.Trim();
            report.Parameters["PCode2"].Value = LevelCodeText.Text.Trim();
            report.Parameters["PDate"].Value = date;
            report.Parameters["PAmount"].Value = AmountValue.Value.ToString("N2");
            report.Parameters["PAmountWithString"].Value = amount_with_string;
            report.Parameters["PAmountEDV"].Value = edv.ToString("N2");
            report.Parameters["PAmountEDVWithString"].Value = amount_edv_with_string;
            report.Parameters["PReason"].Value = ReasonText.Text;
            report.Parameters["PCurrency"].Value = CurrencyLookUp.Text;
            report.Parameters["PPayingCustomer"].Value = GlobalFunctions.ReadSetting("Company");
            report.Parameters["PPayingAccount"].Value = paying_account;
            report.Parameters["PPayingVoen"].Value = GlobalFunctions.ReadSetting("CompanyVoen");
            report.Parameters["PAcceptor"].Value = AcceptorLookUp.Text;
            report.Parameters["PAcceptorAccount"].Value = acceptor_account;
            report.Parameters["PAcceptorAccountVat"].Value = acceptor_account_vat;
            report.Parameters["PAcceptorVoen"].Value = acceptor_voen;
            report.Parameters["PPayingBank"].Value = paying_bank_name;
            report.Parameters["PAcceptorBank"].Value = acceptor_bank_name;
            report.Parameters["PAcceptorBankVat"].Value = acceptor_bank_name_vat;
            report.Parameters["PPayingBankCode"].Value = paying_bank_code;
            report.Parameters["PAcceptorBankCode"].Value = acceptor_bank_code;
            report.Parameters["PAcceptorBankCodeVat"].Value = acceptor_bank_code_vat;
            report.Parameters["PPayingBankVoen"].Value = paying_bank_voen;
            report.Parameters["PAcceptorBankVoen"].Value = acceptor_bank_voen;
            report.Parameters["PAcceptorBankVoenVat"].Value = acceptor_bank_voen_vat;
            report.Parameters["PPayingBankSwift"].Value = paying_bank_swift;
            report.Parameters["PAcceptorBankSwift"].Value = acceptor_bank_swift;
            report.Parameters["PAcceptorBankSwiftVat"].Value = acceptor_bank_swift_vat;
            report.Parameters["PPayingBankAccount"].Value = paying_bank_cbar_account;
            report.Parameters["PAcceptorBankAccount"].Value = acceptor_bank_cbar_account;
            report.Parameters["PAcceptorBankAccountVat"].Value = acceptor_bank_cbar_account_vat;
            new ReportPrintTool(report).ShowPreview();
            report.PrintingSystem.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.Parameters, new object[] { true });
        }

        private void LoadVatBankDetails()
        {
            string s = $@"SELECT B.NAME,   
                                   B.CODE,                                
                                   B.CBAR_ACCOUNT,
                                   B.SWIFT,
                                   B.VOEN                                  
                              FROM CRS_USER.TASK_VAT_BANKS B";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, "LoadVatBankDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    acceptor_bank_name_vat = dr["NAME"].ToString();
                    acceptor_bank_code_vat = dr["CODE"].ToString();
                    acceptor_bank_cbar_account_vat = dr["CBAR_ACCOUNT"].ToString();
                    acceptor_bank_swift_vat = dr["SWIFT"].ToString();
                    acceptor_bank_voen_vat = dr["VOEN"].ToString();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("ƏDV üçün bank açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private string InsertTaskCodeTemp()
        {
            if (type_id == 0)
                return null;

            int max_code_number = 0;
            string code = null;

            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.TASK_CODE_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID}", "Tapşırığın kodu temp cədvəldən silinmədi.");

            List<TaskCodeTemp> lstTaskCode = TaskCodeTempDAL.SelectTaskCode(type_id).ToList<TaskCodeTemp>();

            if (lstTaskCode.Count == 0)
                max_code_number = GlobalFunctions.GetMax($@"SELECT NVL(MAX(TASK_NUMBER),0) FROM CRS_USER.PAYMENT_TASKS WHERE EXTRACT(YEAR FROM TDATE) = EXTRACT(YEAR FROM SYSDATE) AND TYPE_ID = {type_id}");
            else
                max_code_number = lstTaskCode.Max(t => t.TASK_NUMBER);

            task_number = max_code_number + 1;
            code = type_code + task_number.ToString().PadLeft(2, '0') + "-" + DateValue.DateTime.Year.ToString().Substring(2, 2);
            if (GlobalFunctions.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.TASK_CODE_TEMP(USED_USER_ID,TASK_ID,TASK_TYPE_ID,TASK_NUMBER,CODE)VALUES({GlobalVariables.V_UserID},{TaskID},{type_id},{task_number},'{code}')",
                                                    "Tapşırığın maksimum nömrəsi temp cədvələ daxil edilmədi.") > 0)
                return code;
            else
                return null;
        }

        private bool ControlDetail()
        {
            bool b = false;

            if (TemplateCheck.Checked && TemplateLookUp.EditValue == null)
            {
                TemplateLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şablon forma daxil edilməyib.");
                TemplateLookUp.Focus();
                TemplateLookUp.BackColor = GlobalFunctions.ElementColor();
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

            if (CodeText.Text.Length == 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tapşırığın nömrəsi daxil edilməyib.");
                CodeText.Focus();
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (changecode && GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.PAYMENT_TASKS WHERE CODE = '{CodeText.Text.Trim()}' AND TYPE_ID = {type_id}") > 0)
            {
                CodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(CodeText.Text + " nömrəli tapşırıq artıq bazaya daxil edilib.");
                CodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(DateValue.Text))
            {
                DateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                DateValue.Focus();
                DateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (AmountValue.Value <= 0)
            {
                AmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məbləğ daxil edilməyib.");
                AmountValue.Focus();
                AmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CurrencyLookUp.EditValue == null)
            {
                CurrencyLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Valyuta daxil edilməyib.");
                CurrencyLookUp.Focus();
                CurrencyLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(ReasonText.Text))
            {
                ReasonText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Təyinat daxil edilməyib.");
                ReasonText.Focus();
                ReasonText.BackColor = GlobalFunctions.ElementColor();
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

            if(EDVCheck.Checked && acceptor_account_vat == null)
            {
                AcceptorLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Alan tərəfin ƏDV-ni hesabı daxil edilməyib. Ödəniş tapşırığını yaratmaq üçün əvvəlcə ödənişi alan tərəflərinin siyahısını açın və seçdiyiniz müştəri üçün ƏDV hesabını daxil edin.");
                AcceptorLookUp.Focus();
                AcceptorLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 7:
                    GlobalProcedures.FillLookUpEdit(CurrencyLookUp, "CURRENCY", "ID", "CODE", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 11:
                    {

                        lstBanks = BanksDAL.SelectBankByID(null).ToList<Banks>();
                        GlobalProcedures.FillLookUpEdit(PayingBankLookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                        LoadPayingBankDetails();
                        if (isInternal)
                        {
                            GlobalProcedures.FillLookUpEdit(AcceptorBank2LookUp, "BANKS", "ID", "LONG_NAME", "STATUS_ID = 7 ORDER BY ORDER_ID");
                            LoadAcceptorBank2Details();
                        }
                    }
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
    }
}