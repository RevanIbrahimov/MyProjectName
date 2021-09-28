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

namespace CRS.Forms.Total
{
    public partial class FAttentionAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FAttentionAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, ContractID, NoteID;

        public delegate void DoEvent();
        public event DoEvent RefreshAttentionDataGridView;

        private void NoteText_EditValueChanged(object sender, EventArgs e)
        {
            if (NoteText.Text.Length <= 1400)
                DescriptionCharCountLabel.Text = "Daxil ediləcək simvolların sayı - " + (1400 - NoteText.Text.Length).ToString();
        }

        private void FAttentionAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
                LoadNoteDetils();
        }

        private void LoadNoteDetils()
        {
            string sql = $@"SELECT NOTE FROM CRS_USER.PAYMENT_NOTES WHERE ID = {NoteID}";

            foreach(DataRow dr in GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadNoteDetils").Rows)
            {
                NoteText.Text = dr["NOTE"].ToString();
            }
        }

        private void InsertNote()
        {
            string sql = $@"INSERT INTO CRS_USER.PAYMENT_NOTES(CONTRACT_ID,NOTE,INSERT_USER_ID)VALUES({ContractID},'{NoteText.Text.Trim()}',{GlobalVariables.V_UserID})";
            GlobalProcedures.ExecuteQuery(sql, "Qeyd bazaya daxil edilmədi.", this.Name + "/InsertNote");
        }

        private void EditNote()
        {
            string sql = $@"UPDATE CRS_USER.PAYMENT_NOTES SET NOTE = '{NoteText.Text.Trim()}',CHANGE_DATE = SYSDATE WHERE ID = {NoteID}";
            GlobalProcedures.ExecuteQuery(sql, "Qeyd bazada dəyişdirilmədi.", this.Name + "/EditNote");
        }

        private bool ControlDetails()
        {
            bool b = false;

            if (NoteText.Text == null)
            {
                NoteText.BackColor = Color.Red;
                XtraMessageBox.Show("Qeyd daxil edilməyib.");
                NoteText.Focus();
                NoteText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NoteText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                BOK_Click(sender, EventArgs.Empty);
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlDetails())
            {
                if (TransactionName == "INSERT")
                    InsertNote();
                else
                    EditNote();
                this.Close();
            }
        }

        private void FAttentionAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshAttentionDataGridView();
        }
    }
}