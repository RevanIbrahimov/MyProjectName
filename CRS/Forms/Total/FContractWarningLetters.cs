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
    public partial class FContractWarningLetters : DevExpress.XtraEditors.XtraForm
    {
        public FContractWarningLetters()
        {
            InitializeComponent();
        }
        int letterID;

        private void FContractWarningLetters_Load(object sender, EventArgs e)
        {
            LoadLetters();
        }

        private void LoadLetters()
        {
            string sql = $@"SELECT 1 SS,
                                   WL.ID,
                                   CON.CONTRACT_CODE,
                                   WL.LETTER_CODE,
                                   WL.LETTER_DATE,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = WL.INSERT_USER)
                                      INSERT_USER,
                                   INSERT_DATE,
                                   (SELECT USER_FULLNAME
                                      FROM CRS_USER.V_USERS
                                     WHERE ID = WL.UPDATE_USER)
                                      UPDATE_USER,
                                   UPDATE_DATE
                              FROM CRS_USER.WARNING_LETTERS WL, CRS_USER.V_CONTRACTS CON
                             WHERE WL.CONTRACT_ID = CON.CONTRACT_ID
                            ORDER BY CON.CONTRACT_CODE";

            LettersGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql, this.Name + "/LoadLetters", "Xəbərdarlıq məktublarının siyahısı açılmadı.");

            if (GlobalVariables.V_UserID == 0)
                DeleteBarButton.Enabled = (LettersGridView.RowCount > 0);
            else
                DeleteBarButton.Enabled = (LettersGridView.RowCount > 0)? GlobalVariables.DeleteWarningLetter : false;
            ShowFileBarButton.Enabled = (LettersGridView.RowCount > 0);
        }

        private void LettersGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (GlobalVariables.V_UserID == 0)
                DeleteBarButton.Enabled = (LettersGridView.RowCount > 0);
            else
                DeleteBarButton.Enabled = (LettersGridView.RowCount > 0) ? GlobalVariables.DeleteWarningLetter : false;
            ShowFileBarButton.Enabled = (LettersGridView.RowCount > 0);
        }

        private void PrintBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.ShowGridPreview(LettersGridControl);
        }

        private void ExportBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalProcedures.GridExportToFile(LettersGridControl, "xls");
        }

        private void ShowFileBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowFile();
        }

        private void ShowFile()
        {
            GlobalProcedures.ShowWordFileFromDB($@"SELECT T.LETTER_FILE FROM CRS_USER.WARNING_LETTERS T WHERE T.ID = {letterID}",
                                                GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\XəbərdarlıqMəktubu_" + letterID + ".docx",
                                                "LETTER_FILE");
        }

        private void LettersGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void LettersGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(LettersGridView, PopupMenu, e);
        }

        private void LettersGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = LettersGridView.GetFocusedDataRow();
            if (row != null)
                letterID = int.Parse(row["ID"].ToString());
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş məktubu silmək istəyirsiniz?", "Məktubun silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.WARNING_LETTERS WHERE ID = {letterID}", "Xəbərdarlıq məktubu bazadan silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
            }
            LoadLetters();
        }

        private void LettersGridView_DoubleClick(object sender, EventArgs e)
        {
            ShowFile();
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLetters();
        }
    }
}