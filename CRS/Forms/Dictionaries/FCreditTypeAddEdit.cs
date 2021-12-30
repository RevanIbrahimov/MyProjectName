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

namespace CRS.Forms.Dictionaries
{
    public partial class FCreditTypeAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCreditTypeAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, TypeID;
        bool CurrentStatus = false, TypeUsed = false;
        int TypeUsedUserID = -1, nameID, categoryID;

        public delegate void DoEvent();
        public event DoEvent RefreshCreditTypeDataGridView;

        private void FCreditTypeAddEdit_Load(object sender, EventArgs e)
        {
            GlobalProcedures.FillLookUpEdit(CreditCategoryLookUp, "CREDIT_CATEGORY", "ID", "NAME", "1 = 1 ORDER BY ID");
            CalcDate.EditValue = DateTime.Today;            

            if (TransactionName == "INSERT")            
                TypeID = GlobalFunctions.GetOracleSequenceValue("CREDIT_TYPE_SEQUENCE").ToString();   
            else
            {
                CalcDate.Enabled = false;
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", GlobalVariables.V_UserID, "WHERE ID = " + TypeID + " AND USED_USER_ID = -1");
                LoadTypeDetails();                
                TypeUsed = (TypeUsedUserID >= 0);
                
                if (TypeUsed)
                {
                    if (GlobalVariables.V_UserID != TypeUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == TypeUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş parametr hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş parametrin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);

                int a = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER.CONTRACTS WHERE CREDIT_TYPE_ID = {TypeID}");
                if (a > 0)
                {
                    XtraMessageBox.Show("Seçilmiş parametr müqavilələrdə istifadə olunduğu üçün dəyişdirilə bilməz.", "Xəbərdarlıq", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CreditNameLookUp.Enabled = 
                        CalcDate.Enabled =                
                        TermValue.Enabled = 
                        InterestValue.Enabled = 
                        CreditCategoryLookUp.Enabled =
                        PenaltyPercentValue.Enabled = 
                        CommissionPercentValue.Enabled =                        
                        NoteText.Enabled = false;

                    BFont.Visible = 
                        BOK.Visible = 
                        CodeText.Properties.ReadOnly = true;
                }
                
                
                InsertCreditTypeFontsTemp();
                RefreshFont();
            }
        }

        private void ComponentEnabled(bool status)
        {            
            CreditNameLookUp.Enabled = 
                CalcDate.Enabled =            
                CodeText.Enabled = 
                TermValue.Enabled = 
                InterestValue.Enabled = 
                NoteText.Enabled = 
                BFont.Visible = 
                CreditCategoryLookUp.Enabled =
                PenaltyPercentValue.Enabled =
                CommissionPercentValue.Enabled =
                BOK.Visible = !status;
        }

        private void LoadTypeDetails()
        {
            string s = $@"SELECT CN.NAME,
                                   CN.NAME_EN,
                                   CN.NAME_RU,
                                   CT.NOTE,
                                   CT.CALC_DATE,
                                   CT.TERM,
                                   CT.INTEREST,
                                   CT.USED_USER_ID,
                                   CC.NAME CATEGORY_NAME,
                                   CT.PENALTY_PERCENT,
                                   CT.COMMISSION
                              FROM CRS_USER.CREDIT_TYPE CT, CRS_USER.CREDIT_NAMES CN, CRS_USER.CREDIT_CATEGORY CC
                             WHERE CT.NAME_ID = CN.ID AND CN.CREDIT_CATEGORY_ID = CC.ID AND CT.ID = {TypeID}";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadTypeDetails", "Kreditin növü açılmadı.");
            if(dt.Rows.Count > 0)
            {
                CreditCategoryLookUp.EditValue = CreditCategoryLookUp.Properties.GetKeyValueByDisplayText(dt.Rows[0]["CATEGORY_NAME"].ToString());
                CreditNameLookUp.EditValue = CreditNameLookUp.Properties.GetKeyValueByDisplayText(dt.Rows[0]["NAME"].ToString());
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                CalcDate.EditValue = DateTime.Parse(dt.Rows[0]["CALC_DATE"].ToString());
                TermValue.Value = Convert.ToInt32(dt.Rows[0]["TERM"]);
                InterestValue.Value = Convert.ToDecimal(dt.Rows[0]["INTEREST"]);
                PenaltyPercentValue.Value = Convert.ToDecimal(dt.Rows[0]["PENALTY_PERCENT"]);
                CommissionPercentValue.Value = Convert.ToDecimal(dt.Rows[0]["COMMISSION"]);
                TypeUsedUserID = Convert.ToInt32(dt.Rows[0]["USED_USER_ID"]);
            }            
        }

        private void InsertCreditTypeFontsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithUser("CRS_USER_TEMP.PROC_INS_CREDIT_TYPE_FONT_TEMP", "P_TYPE_ID", int.Parse(TypeID), "Kodun fontları temp cədvələ daxil edilmədi.");            
        }

        private void InsertCreditTypeFonts()
        {
            GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER.CREDIT_TYPE_FONTS WHERE ID IN (SELECT ID FROM CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP WHERE IS_CHANGE = 1 AND CREDIT_TYPE_ID = {TypeID})",
                                            $@"INSERT INTO CRS_USER.CREDIT_TYPE_FONTS(ID,CREDIT_TYPE_ID,FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_B,BACKCOLOR_G,BACKCOLOR_R,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_B,FONTCOLOR_G,FONTCOLOR_R,FONTCOLOR_TYPE,FONTCOLOR_NAME)SELECT ID,CREDIT_TYPE_ID,FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_B,BACKCOLOR_G,BACKCOLOR_R,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_B,FONTCOLOR_G,FONTCOLOR_R,FONTCOLOR_TYPE,FONTCOLOR_NAME FROM CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP WHERE IS_CHANGE = 1 AND CREDIT_TYPE_ID = {TypeID}",
                                                    "Kodun fontları temp cədvələ daxil edilmədi.",
                                                    this.Name + "/InsertCreditTypeFonts");
        }

        private bool ControlTypeDetails()
        {
            bool b = false;

            if (CreditCategoryLookUp.EditValue == null)
            {
                CreditCategoryLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kateqoriya seçilməyib.");
                CreditCategoryLookUp.Focus();
                CreditCategoryLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CreditNameLookUp.EditValue == null)
            {
                CreditNameLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Kreditin növü seçilməyib.");               
                CreditNameLookUp.Focus();
                CreditNameLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(CalcDate.Text))
            {
                CalcDate.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");                
                CalcDate.Focus();
                CalcDate.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (TermValue.Value <= 0)
            {
                TermValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Müddət sıfırdan böyük olmalıdır.");                
                TermValue.Focus();
                TermValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (InterestValue.Value < 0)
            {
                InterestValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("İllik faiz dərəcəsi mənfi ədəd ola bilməz.");               
                InterestValue.Focus();
                InterestValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PenaltyPercentValue.Value < 0)
            {
                PenaltyPercentValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Cərimə faizi mənfi ədəd ola bilməz.");
                PenaltyPercentValue.Focus();
                PenaltyPercentValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertType()
        {           
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CREDIT_TYPE(ID,
                                                                                NAME_ID,
                                                                                NOTE,
                                                                                CALC_DATE,
                                                                                TERM,
                                                                                INTEREST,
                                                                                PENALTY_PERCENT,
                                                                                COMMISSION,
                                                                                INSERT_USER)
                                             VALUES({TypeID},
                                                    {nameID},
                                                    '{NoteText.Text.Trim()}',
                                                    TO_DATE('{CalcDate.Text}','DD/MM/YYYY'),
                                                    {TermValue.Value},
                                                    {InterestValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                    {PenaltyPercentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                    {CommissionPercentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                    {GlobalVariables.V_UserID})",
                                              "Kreditin növü daxil edilmədi.",
                                              this.Name + "/InsertType");
        }

        private void UpdateType()
        {                    
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CREDIT_TYPE SET NAME_ID = {nameID},
                                                                                NOTE = '{NoteText.Text.Trim()}',
                                                                                CALC_DATE = TO_DATE('{CalcDate.Text}','DD/MM/YYYY'),
                                                                                TERM = {TermValue.Value},
                                                                                INTEREST = {InterestValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                PENALTY_PERCENT = {PenaltyPercentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                COMMISSION = {CommissionPercentValue.Value.ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                UPDATE_USER = {GlobalVariables.V_UserID},
                                                                                UPDATE_DATE = SYSDATE
                                                    WHERE ID = {TypeID}",
                                                "Kreditin növü dəyişdirilmədi.",
                                                this.Name + "/UpdateType");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCreditTypeAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CREDIT_TYPE", -1, "WHERE ID = " + TypeID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID} AND CREDIT_TYPE_ID = {TypeID}",
                                                "Kodun fontları temp cədvəldən silinmədi.",
                                                this.Name + "/FCreditTypeAddEdit_FormClosing");
            this.RefreshCreditTypeDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlTypeDetails())
            {
                if (TransactionName == "INSERT")
                    InsertType();
                else
                    UpdateType();
                InsertCreditTypeFonts();
                this.Close();
            }
        }

        void RefreshDictionaries(int index)
        {
            GlobalProcedures.FillLookUpEdit(CreditNameLookUp, "CREDIT_NAMES", "ID", "NAME", $@"CREDIT_CATEGORY_ID = {categoryID} ORDER BY ID");
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void CreditCategoryLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CreditCategoryLookUp.EditValue == null)
                return;

            categoryID = Convert.ToInt32(CreditCategoryLookUp.EditValue);
            RefreshDictionaries(6);
        }

        private void CreditNameLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if (CreditNameLookUp.EditValue == null)
                return;

            nameID = Convert.ToInt32(CreditNameLookUp.EditValue);
                        
            if (TransactionName == "INSERT" && !String.IsNullOrEmpty(TypeID))
            {                
                GlobalProcedures.ExecuteTwoQuery($@"DELETE FROM CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP
                                                        WHERE CREDIT_TYPE_ID = {TypeID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                 $@"INSERT INTO CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP (ID,
                                                                                                          CREDIT_TYPE_ID,
                                                                                                          FONTNAME,
                                                                                                          FONTSIZE,
                                                                                                          FONTSTYLE,
                                                                                                          BACKCOLOR_A,
                                                                                                          BACKCOLOR_B,
                                                                                                          BACKCOLOR_G,
                                                                                                          BACKCOLOR_R,
                                                                                                          BACKCOLOR_TYPE,
                                                                                                          BACKCOLOR_NAME,
                                                                                                          FONTCOLOR_A,
                                                                                                          FONTCOLOR_B,
                                                                                                          FONTCOLOR_G,
                                                                                                          FONTCOLOR_R,
                                                                                                          FONTCOLOR_TYPE,
                                                                                                          FONTCOLOR_NAME,
                                                                                                          USED_USER_ID,
                                                                                                          IS_CHANGE)
                                                           SELECT CREDIT_TYPE_FONT_SEQUENCE.NEXTVAL,
                                                                  {TypeID},
                                                                  FONTNAME,
                                                                  FONTSIZE,
                                                                  FONTSTYLE,
                                                                  BACKCOLOR_A,
                                                                  BACKCOLOR_B,
                                                                  BACKCOLOR_G,
                                                                  BACKCOLOR_R,
                                                                  BACKCOLOR_TYPE,
                                                                  BACKCOLOR_NAME,
                                                                  FONTCOLOR_A,
                                                                  FONTCOLOR_B,
                                                                  FONTCOLOR_G,
                                                                  FONTCOLOR_R,
                                                                  FONTCOLOR_TYPE,
                                                                  FONTCOLOR_NAME,
                                                                  {GlobalVariables.V_UserID} UED_USER_ID,
                                                                  1 IS_CHANGE
                                                             FROM CRS_USER.CREDIT_TYPE_FONTS
                                                            WHERE CREDIT_TYPE_ID = (SELECT NVL (MAX (ID), 0)
                                                                                      FROM CRS_USER.CREDIT_TYPE
                                                                                     WHERE NAME_ID = {nameID})",                                                 
                                                        "Kodun fontları temp cədvələ daxil edilmədi.",
                                                        this.Name + "/CreditNameLookUp_EditValueChanged");
                RefreshFont();
            }
        }

        private void CreditNameLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 6);
        }

        void RefreshFont()
        {
            string s = $@"SELECT FONTNAME,
                                   FONTSIZE,
                                   FONTSTYLE,
                                   BACKCOLOR_A,
                                   BACKCOLOR_R,
                                   BACKCOLOR_G,
                                   BACKCOLOR_B,
                                   BACKCOLOR_TYPE,
                                   BACKCOLOR_NAME,
                                   FONTCOLOR_A,
                                   FONTCOLOR_R,
                                   FONTCOLOR_G,
                                   FONTCOLOR_B,
                                   FONTCOLOR_TYPE,
                                   FONTCOLOR_NAME
                              FROM CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP
                             WHERE CREDIT_TYPE_ID = {TypeID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/RefreshFont");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        switch (dr[2].ToString())
                        {
                            case "Regular": CodeText.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Regular);
                                break;
                            case "Bold": CodeText.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Bold);
                                break;
                            case "Italic": CodeText.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Italic);
                                break;
                            case "Underline": CodeText.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Underline);
                                break;
                            case "Strikeout": CodeText.Font = new Font(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), FontStyle.Strikeout);
                                break;
                        }
                        if (Convert.ToInt32(dr[3].ToString()) != 0 || Convert.ToInt32(dr[4].ToString()) != 0 || Convert.ToInt32(dr[5].ToString()) != 0 || Convert.ToInt32(dr[6].ToString()) != 0)
                            CodeText.BackColor = GlobalFunctions.CreateColor(dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                        else
                            CodeText.BackColor = GlobalFunctions.ElementColor();
                        if (Convert.ToInt32(dr[9].ToString()) != 0 || Convert.ToInt32(dr[10].ToString()) != 0 || Convert.ToInt32(dr[11].ToString()) != 0 || Convert.ToInt32(dr[12].ToString()) != 0)
                            CodeText.ForeColor = GlobalFunctions.CreateColor(dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), dr[13].ToString(), dr[14].ToString());
                        else
                            CodeText.ForeColor = GlobalFunctions.ElementColor();
                    }
                }
                else
                {
                    CodeText.Font = new Font("Tahoma", 8, FontStyle.Regular);
                    CodeText.BackColor = GlobalFunctions.ElementColor();
                    CodeText.ForeColor = GlobalFunctions.ElementColor();
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Kodun fontları açılmadı.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void BFont_Click(object sender, EventArgs e)
        {
            FCreditTypeFormat fctf = new FCreditTypeFormat();
            fctf.TypeID = TypeID;
            fctf.RefreshFont += new FCreditTypeFormat.DoEvent(RefreshFont);
            fctf.ShowDialog();
        }
    }
}