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
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Contracts
{
    public partial class FPawnAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPawnAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID, Currency;
        public int? ID;
        public decimal CreditAmount;

        int pawnTypeID = 0, goldTypeID = 0, eyebrowsTypeID = 0, isDeduction = 0;
        decimal coefficient = 1, goldAmount = 0;
        bool FormStatus = false;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FPawnAddEdit_Load(object sender, EventArgs e)
        {
            RefreshList(6);
            RefreshList(8);
            RefreshList(13);
            
            CreditAmountValue.EditValue = CreditAmount;
            CreditAmountCurrencyLabel.Text = Currency;

            if (TransactionName == "EDIT")            
                LoadDetails();            
            else
            {
                FormStatus = true;
                GlobalProcedures.LookUpEditValue(EyebrowsTypeLookUp, "Brilyant");
            }

            LomAutomaticCheck_CheckedChanged(sender, EventArgs.Empty);

            if (TransactionName == "EDIT")
                FormStatus = true;
        }

        private void LoadDetails()
        {
            string sql = $@"SELECT PT.NAME PAWN_TYPE_NAME,
                                   P.COUNT,
                                   GT.NAME GOLD_TYPE_NAME,
                                   P.WEIGHT,
                                   P.CWEIGHT,
                                   P.EYEBROWS_WEIGHT,
                                   P.EYEBROWS_AMOUNT,
                                   ET.NAME EYEBROWS_TYPE_NAME,                                   
                                   P.NOTE,
                                   P.IS_LOM_AMOUNT_AUTOMATIC,
                                   P.LOM_AMOUNT_COEFFICIENT,
                                   P.AMOUNT
                              FROM CRS_USER_TEMP.PAWN_TEMP P,
                                   CRS_USER.PAWN_TYPE PT,
                                   CRS_USER.GOLD_TYPE GT,
                                   CRS_USER.EYEBROWS_TYPE ET
                             WHERE     P.GOLD_TYPE_ID = GT.ID
                                   AND P.PAWN_TYPE_ID = PT.ID
                                   AND P.EYEBROWS_TYPE_ID = ET.ID
                                   AND P.USED_USER_ID = {GlobalVariables.V_UserID}
                                   AND P.ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadDetails", "Girovun məlumatları açılmadı.");

            if (dt.Rows.Count > 0)
            {
                GlobalProcedures.LookUpEditValue(PawnTypeLookUp, dt.Rows[0]["PAWN_TYPE_NAME"].ToString());
                CountValue.EditValue = dt.Rows[0]["COUNT"];
                GlobalProcedures.LookUpEditValue(CreditClassLookUp, dt.Rows[0]["GOLD_TYPE_NAME"].ToString());
                WeightValue.EditValue = dt.Rows[0]["WEIGHT"];
                CWeightValue.EditValue = dt.Rows[0]["CWEIGHT"];
                BirilliantWeightValue.EditValue = dt.Rows[0]["EYEBROWS_WEIGHT"];
                BirilliantAmountValue.EditValue = dt.Rows[0]["EYEBROWS_AMOUNT"];
                GlobalProcedures.LookUpEditValue(EyebrowsTypeLookUp, dt.Rows[0]["EYEBROWS_TYPE_NAME"].ToString());
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
                LomAutomaticCheck.Checked = Convert.ToInt16(dt.Rows[0]["IS_LOM_AMOUNT_AUTOMATIC"]) == 1? true : false;
                LomPriceValue.EditValue = Convert.ToDecimal(dt.Rows[0]["LOM_AMOUNT_COEFFICIENT"]);
                LomAmountValue.EditValue = Convert.ToDecimal(dt.Rows[0]["AMOUNT"]);                
            }
        }

        private void CalcGoldAmount()
        {
            if (!FormStatus || CWeightValue.Value <= 0 || LomPriceValue.Value <= 0)
                return;

            if (LomAutomaticCheck.Checked)
                LomAmountValue.EditValue = Math.Round((CreditAmountValue.Value / LomPriceValue.Value) / CWeightValue.Value, 2);            
        }

        void RefreshList(int index)
        {
            switch (index)
            {
                case 6:
                    GlobalProcedures.FillLookUpEdit(PawnTypeLookUp, "PAWN_TYPE", "ID", "NAME", "1 = 1 ORDER BY NAME");
                    break;
                case 8:
                    GlobalProcedures.FillLookUpEdit(CreditClassLookUp, "GOLD_TYPE", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
                case 13:
                    GlobalProcedures.FillLookUpEdit(EyebrowsTypeLookUp, "EYEBROWS_TYPE", "ID", "NAME", "1 = 1 ORDER BY ID");
                    break;
            }
        }

        private void PawnTypeLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 6);
        }

        private void CreditClassLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 8);
        }

        private void PawnTypeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            pawnTypeID = Convert.ToInt32((sender as LookUpEdit).EditValue);
        }

        private void CreditClassLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            goldTypeID = Convert.ToInt32((sender as LookUpEdit).EditValue);
            List<CreditClass> lstCreditClass = CreditClassDAL.SelectCreditClassByID(goldTypeID).ToList<CreditClass>();
            if (lstCreditClass.Count > 0)
            {
                //coefficient = lstCreditClass.FirstOrDefault().COEFFICIENT;
                //goldAmount = lstCreditClass.FirstOrDefault().AMOUNT;
            }
            CoefficientValue.EditValue = coefficient;
            LomAmountValue.EditValue = goldAmount;
            ConvertCreditClass();
        }

        private void ConvertCreditClass()
        {
            CWeightValue.EditValue = (WeightValue.Value - (isDeduction == 1? BirilliantWeightValue.Value : 0)) * coefficient;
        }

        private void LoadDictionaries(string transaction, int index)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshList);
            fc.ShowDialog();
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (pawnTypeID == 0)
            {
                PawnTypeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əşya seçilməyib.");
                PawnTypeLookUp.Focus();
                PawnTypeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (CountValue.Value <= 0)
            {
                CountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əşyanın sayı ən azı 1 ədəd olmalıdır.");
                CountValue.Focus();
                CountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (goldTypeID == 0)
            {
                CreditClassLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əşyanın əyyarı seçilməyib.");
                CreditClassLookUp.Focus();
                CreditClassLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (WeightValue.Value <= 0)
            {
                WeightValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əşyanın çəkisi sıfırdan böyük olmalıdır.");
                WeightValue.Focus();
                WeightValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (!LomAutomaticCheck.Checked && LomAmountValue.Value <= 0)
            {
                LomAmountValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qızılın lom qiyməti sıfırdan böyük olmalıdır.");
                LomAmountValue.Focus();
                LomAmountValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (eyebrowsTypeID == 0)
            {
                EyebrowsTypeLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Qaşın növü seçilməyib.");
                EyebrowsTypeLookUp.Focus();
                EyebrowsTypeLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void Insert()
        {
            decimal weight = CWeightValue.Value,
                    eyebrowsAmount = isDeduction == 1? -Math.Round(BirilliantAmountValue.Value, 2) : Math.Round(BirilliantAmountValue.Value, 2);

            string sql = $@"INSERT INTO CRS_USER_TEMP.PAWN_TEMP(ID,
                                                                CONTRACT_ID,
                                                                PAWN_TYPE_ID,
                                                                COUNT,
                                                                GOLD_TYPE_ID,
                                                                WEIGHT,
                                                                CWEIGHT,                                                                
                                                                EYEBROWS_WEIGHT,
                                                                EYEBROWS_AMOUNT,
                                                                EYEBROWS_TYPE_ID,
                                                                NOTE,
                                                                IS_CHANGE,
                                                                AMOUNT,
                                                                GOLD_AMOUNT, 
                                                                TOTAL_AMOUNT,
                                                                IS_LOM_AMOUNT_AUTOMATIC,
                                                                LOM_AMOUNT_COEFFICIENT,
                                                                USED_USER_ID)
                            VALUES(CRS_USER.PAWN_SEQUENCE.NEXTVAL,
                                   {ContractID},
                                   {pawnTypeID},
                                   {CountValue.Value},
                                   {goldTypeID},
                                   {Math.Round(WeightValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                   {Math.Round(CWeightValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                                   
                                   {Math.Round(BirilliantWeightValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                   {Math.Round(BirilliantAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},   
                                   {eyebrowsTypeID},
                                   '{NoteText.Text.Trim()}',
                                   1,
                                   {Math.Round(LomAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                   {(Math.Round(weight, 2) * Math.Round(LomAmountValue.Value, 2)).ToString(GlobalVariables.V_CultureInfoEN)}, 
                                   {(Math.Round(weight, 2) * Math.Round(LomAmountValue.Value, 2) + eyebrowsAmount).ToString(GlobalVariables.V_CultureInfoEN)}, 
                                   {(LomAutomaticCheck.Checked? 1 : 0)},
                                   {Math.Round(LomPriceValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                   {GlobalVariables.V_UserID})";

            GlobalProcedures.ExecuteQuery(sql, "Girov temp cədvələ daxil edilmədi.", this.Name + "/Insert");
        }

        private void EyebrowsTypeLookUp_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as LookUpEdit).EditValue == null)
                return;

            eyebrowsTypeID = Convert.ToInt32((sender as LookUpEdit).EditValue);

            List<Eyebrow> lstEyebrow = EyebrowDAL.SelectEyebrowByID(eyebrowsTypeID).ToList<Eyebrow>();
            if (lstEyebrow.Count > 0)
                isDeduction = lstEyebrow.LastOrDefault().IS_DEDUCTION;
        }

        private void EyebrowsTypeLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 13);
        }

        private void LomAutomaticCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (LomAutomaticCheck.Checked)
            {
                LomPriceValue.TabStop = true;
                LomPriceValue.ReadOnly = false;
                LomPriceValue.Focus();
                CalcGoldAmount();
            }
            else
            {
                LomPriceValue.ReadOnly = true;
                LomPriceValue.TabStop = false;
                LomAmountValue.EditValue = goldAmount;
            }
        }

        private void BirilliantWeightValue_EditValueChanged(object sender, EventArgs e)
        {
            if (BirilliantWeightValue.EditorContainsFocus)
                ConvertCreditClass();
        }

        private void CWeightValue_EditValueChanged(object sender, EventArgs e)
        {            
            CalcGoldAmount();
        }

        private void LomPriceValue_EditValueChanged(object sender, EventArgs e)
        {
            if (LomPriceValue.EditorContainsFocus)
                CalcGoldAmount();
        }

        private void WeightValue_EditValueChanged(object sender, EventArgs e)
        {
            ConvertCreditClass();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    Insert();
                else
                    Update();
                this.Close();
            }
        }

        private void FPawnAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }

        private void Update()
        {
            decimal weight = CWeightValue.Value,
                    eyebrowsAmount = isDeduction == 1 ? -Math.Round(BirilliantAmountValue.Value, 2) : Math.Round(BirilliantAmountValue.Value, 2);

            string sql = $@"UPDATE CRS_USER_TEMP.PAWN_TEMP SET PAWN_TYPE_ID = {pawnTypeID},
                                                                COUNT = {CountValue.Value},
                                                                GOLD_TYPE_ID = {goldTypeID},
                                                                WEIGHT = {Math.Round(WeightValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                CWEIGHT = {Math.Round(CWeightValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},                                                                
                                                                EYEBROWS_WEIGHT = {Math.Round(BirilliantWeightValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                EYEBROWS_AMOUNT = {Math.Round(BirilliantAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},   
                                                                EYEBROWS_TYPE_ID = {eyebrowsTypeID},
                                                                NOTE = '{NoteText.Text.Trim()}',
                                                                AMOUNT = {Math.Round(LomAmountValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                GOLD_AMOUNT = {(Math.Round(weight, 2) * Math.Round(LomAmountValue.Value, 2)).ToString(GlobalVariables.V_CultureInfoEN)}, 
                                                                TOTAL_AMOUNT = {(Math.Round(weight, 2) * Math.Round(LomAmountValue.Value, 2) + eyebrowsAmount).ToString(GlobalVariables.V_CultureInfoEN)}, 
                                                                IS_LOM_AMOUNT_AUTOMATIC = {(LomAutomaticCheck.Checked ? 1 : 0)},
                                                                LOM_AMOUNT_COEFFICIENT = {Math.Round(LomPriceValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                IS_CHANGE = 1
                           WHERE ID = {ID}";

            GlobalProcedures.ExecuteQuery(sql, "Girov temp cədvələ daxil edilmədi.", this.Name + "/Insert");
        }
    }
}