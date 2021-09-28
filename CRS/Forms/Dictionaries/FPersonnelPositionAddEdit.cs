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

namespace CRS.Forms.Dictionaries
{
    public partial class FPersonnelPositionAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FPersonnelPositionAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName;
        public int PersonnelID;
        public int? ID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int positionID = 0;

        private void FPersonnelPositionAddEdit_Load(object sender, EventArgs e)
        {
            RefreshDictionaries(16);
            if (TransactionName == "EDIT")
                LodaDetails();
            else
            {
                DateTime lastStartDate = GlobalFunctions.GetMaxDate($@"SELECT NVL(MAX(START_DATE),TRUNC(SYSDATE)) FROM CRS_USER_TEMP.PERSONNEL_POSITIONS_TEMP WHERE PERSONNEL_ID = {PersonnelID}", this.Name + "/FPersonnelPositionAddEdit_Load");
                if (lastStartDate == DateTime.Today)
                    StartDateValue.DateTime = DateTime.Today;
                else
                    StartDateValue.DateTime = lastStartDate.AddDays(1);
            }
        }

        private void LodaDetails()
        {
            string sql = $@"SELECT P.NAME POSITION_NAME,
                                   PP.START_DATE,
                                   PP.SALARY,
                                   PP.NOTE
                              FROM CRS_USER_TEMP.PERSONNEL_POSITIONS_TEMP PP, CRS_USER.POSITIONS P
                             WHERE PP.POSITION_ID = P.ID AND PP.ID = {ID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LodaDetails", "Vəzifə açılmadı.");

            if (dt.Rows.Count > 0)
            {
                GlobalProcedures.LookUpEditValue(PositionLookUp, dt.Rows[0]["POSITION_NAME"].ToString());
                StartDateValue.EditValue = dt.Rows[0]["START_DATE"];
                SalaryValue.EditValue = dt.Rows[0]["SALARY"];
                NoteText.Text = dt.Rows[0]["NOTE"].ToString();
            }
        }

        void RefreshDictionaries(int index)
        {
            switch (index)
            {
                case 16:
                    GlobalProcedures.FillLookUpEdit(PositionLookUp, "POSITIONS", "ID", "NAME", "1 = 1 ORDER BY ORDER_ID");
                    break;
            }
        }

        private void LoadDictionaries(string transaction, int index, string where)
        {
            FDictionaries fc = new FDictionaries();
            fc.TransactionType = transaction;
            fc.ViewSelectedTabIndex = index;
            fc.StatusWhere = where;
            fc.RefreshList += new FDictionaries.DoEvent(RefreshDictionaries);
            fc.ShowDialog();
        }

        private void PositionLookUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
                LoadDictionaries("E", 16, null);
        }

        private void PositionLookUp_EditValueChanged(object sender, EventArgs e)
        {
            positionID = GlobalFunctions.GetLookUpValueMember(PositionLookUp);
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (positionID == 0)
            {
                PositionLookUp.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Vəzifə seçilməyib.");
                PositionLookUp.Focus();
                PositionLookUp.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (StartDateValue.Text.Length == 0)
            {
                StartDateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Tarix seçilməyib.");
                StartDateValue.Focus();
                StartDateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SalaryValue.Value <= 0)
            {
                SalaryValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Əmək haqqı sıfırdan böyük olmalıdır.");
                SalaryValue.Focus();
                SalaryValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void Insert()
        {
            string sql = $@"INSERT INTO CRS_USER_TEMP.PERSONNEL_POSITIONS_TEMP (ID,
                                                                                  PERSONNEL_ID,
                                                                                  POSITION_ID,
                                                                                  START_DATE,
                                                                                  SALARY,
                                                                                  NOTE,
                                                                                  IS_CHANGE,
                                                                                  USED_USER_ID)
                            VALUES(CRS_USER.PERSONNEL_POSITIONS_SEQUENCE.NEXTVAL,
                                   {PersonnelID},
                                   {positionID},
                                   TO_DATE('{StartDateValue.Text}','DD.MM.YYYY'),
                                   {Math.Round(SalaryValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                   '{NoteText.Text.Trim()}',
                                   1,
                                   {GlobalVariables.V_UserID})";
            GlobalProcedures.ExecuteQuery(sql, "Vəzifə temp cədvələ daxil edilmədi.", this.Name + "/Insert");
        }

        private void Update()
        {
            string sql = $@"UPDATE CRS_USER_TEMP.PERSONNEL_POSITIONS_TEMP SET POSITION_ID = {positionID},
                                                                                  START_DATE = TO_DATE('{StartDateValue.Text}','DD.MM.YYYY'),
                                                                                  SALARY = {Math.Round(SalaryValue.Value, 2).ToString(GlobalVariables.V_CultureInfoEN)},
                                                                                  NOTE = '{NoteText.Text.Trim()}',
                                                                                  IS_CHANGE = 1
                            WHERE ID = {ID}";
            GlobalProcedures.ExecuteQuery(sql, "Vəzifə temp cədvələ daxil edilmədi.", this.Name + "/Insert");
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

        private void FPersonnelPositionAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }
    }
}