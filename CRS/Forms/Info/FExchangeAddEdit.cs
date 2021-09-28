using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms.Info
{
    public partial class FExchangeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FExchangeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, RateID;
        bool CurrentStatus = false, RateUsed = false;

        int cur, RateUsedUserID;

        public delegate void DoEvent();
        public event DoEvent RefreshExchangesDataGridView;

        private void FExchangeAddEdit_Load(object sender, EventArgs e)
        {                      
            RateDate.EditValue = DateTime.Today;
            RateDate.Properties.MaxValue = DateTime.Today;
            GlobalProcedures.FillComboBoxEditWithSqlText(CurrencyComboBox, "SELECT VALUE||' '||CODE||' ('||NAME||')' FROM CRS_USER.CURRENCY WHERE ID <> 1 ORDER BY ORDER_ID");
            if (TransactionName == "EDIT")            
            {
                RateDate.Enabled = false;
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CURRENCY_RATES", GlobalVariables.V_UserID, "WHERE ID = " + RateID + " AND USED_USER_ID = -1");
                RateUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.CURRENCY_RATES WHERE ID = {RateID}");
                RateUsed = (RateUsedUserID > 0);
                
                if (RateUsed)
                {
                    if (GlobalVariables.V_UserID != RateUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == RateUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş məzənnə " + used_user_name + " tərəfindən bloklanıb. Siz yalnız baxa bilərsiniz.", "Seçilmiş məzənnənin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadExchangeDetail();
            }
        }

        private void ComponentEnabled(bool status)
        {
            CurrencyComboBox.Enabled = !status;
            AmountValue.Enabled = !status;
            NoteText.Enabled = !status;
            BOK.Visible = !status;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FExchangeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CURRENCY_RATES", -1, "WHERE ID = " + RateID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshExchangesDataGridView();
        }

        private bool ControlExchangeDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(RateDate.Text))
            {
                RateDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");
                RateDate.Focus();
                RateDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if ((AmountValue.Value == 0) || (String.IsNullOrEmpty(AmountValue.Text)))
            {
                AmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məzənnə daxil edilməyib.");
                AmountValue.Focus();
                AmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;                       

            int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CURRENCY_RATES WHERE CURRENCY_ID = {cur} AND RATE_DATE = TO_DATE('{RateDate.Text}','DD/MM/YYYY')");
            if ((a > 0) && (TransactionName == "INSERT"))
            {
                CurrencyComboBox.BackColor = Color.Red;
                RateDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage(RateDate.Text + " tarixinə " + CurrencyComboBox.Text + " valyutasının AZN qarşılığı artıq daxil edilib.");
                CurrencyComboBox.Focus();
                RateDate.BackColor = Color.White;
                CurrencyComboBox.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertExchange()
        {            
            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.CURRENCY_RATES(ID,CURRENCY_ID,RATE_DATE,AMOUNT,NOTE) VALUES(CURRENCY_RATE_SEQUENCE.NEXTVAL," + cur + ",TO_DATE('" + RateDate.Text + "','DD/MM/YYYY')," + Math.Round(AmountValue.Value,4).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + NoteText.Text.Trim() + "')",
                                                "Məzənnə bazaya daxil olmadı.",
                                                this.Name + "/InsertExchange");
        }

        private void UpdateExchange()
        {
            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.CURRENCY_RATES SET CURRENCY_ID = " + cur + ",AMOUNT = " + Math.Round(AmountValue.Value, 4).ToString(Class.GlobalVariables.V_CultureInfoEN) + ",NOTE = '" + NoteText.Text.Trim() + "' WHERE ID = " + RateID,
                                                "Məzənnə bazada dəyişdirilmədi.",
                                                this.Name + "/UpdateExchange");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlExchangeDetails())
            {
                if (TransactionName == "INSERT")
                    InsertExchange();
                else
                    UpdateExchange();
                this.Close();
            }
        }

        private void CurrencyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            cur = GlobalFunctions.FindComboBoxSelectedValue("CURRENCY", "VALUE||' '||CODE||' ('||NAME||')'", "ID", CurrencyComboBox.Text);
        }

        private void LoadExchangeDetail()
        {
            string s = $@"SELECT TO_CHAR(CR.RATE_DATE,'DD/MM/YYYY'),C.VALUE||' '||C.CODE||' ('||C.NAME||')' CUR,CR.AMOUNT,CR.NOTE FROM CRS_USER.CURRENCY_RATES CR,CRS_USER.CURRENCY C WHERE CR.CURRENCY_ID = C.ID AND CR.ID = {RateID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadExchangeDetail");

                foreach (DataRow dr in dt.Rows)
                {
                    RateDate.EditValue = GlobalFunctions.ChangeStringToDate(dr[0].ToString(), "ddmmyyyy");  
                    CurrencyComboBox.EditValue = dr[1].ToString();                                      
                    AmountValue.Value = Convert.ToDecimal(dr[2].ToString());
                    if (!String.IsNullOrEmpty(dr[3].ToString()))
                        NoteText.Text = dr[3].ToString();
                    else
                        NoteText.Text = null;
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Məzənnənin parametrləri açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        void RefreshPersonType(int index)
        {
            GlobalProcedures.FillComboBoxEditWithSqlText(CurrencyComboBox, "SELECT VALUE||' '||CODE||' ('||NAME||')' FROM CRS_USER.CURRENCY ORDER BY ORDER_ID");            
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshPersonType);
            fc.ShowDialog();
        }

        private void CurrencyComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 7);
        }
    }
}