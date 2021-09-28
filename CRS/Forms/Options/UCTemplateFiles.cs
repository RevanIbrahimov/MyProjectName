using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;
using System.IO;
using System.Diagnostics;

namespace CRS.Forms.Options
{
    public partial class UCTemplateFiles : DevExpress.XtraEditors.XtraUserControl
    {
        public UCTemplateFiles()
        {
            InitializeComponent();
        }
        string ID, Name;
        int old_row_num, topindex;

        private void GroupControl_Paint(object sender, PaintEventArgs e)
        {
            LoadFilesGridView();
        }

        private void LoadFilesGridView()
        {
            string sql = $@"SELECT 1 SS,
                                    F.ID,
                                     F.NAME,
                                     F.NOTE,
                                     F.INSERT_DATE,
                                     F.CHANGE_DATE,
                                     F.USED_USER_ID
                                FROM CRS_USER.TEMPLATE_FILES F
                            ORDER BY F.ID";

            FilesGridControl.DataSource = GlobalFunctions.GenerateDataTable(sql);

            EditBarButton.Enabled = (FilesGridView.RowCount > 0) ? GlobalVariables.EditTemplateFile : false;
        }

        private void FilesGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFilesGridView();
        }

        private void FilesGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(FilesGridView, PopupMenu, e);
        }

        private void FilesGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = FilesGridView.GetFocusedDataRow();
            if (row != null)
            {
                ID = row["ID"].ToString();
                Name = row["NAME"].ToString();
            }
        }

        void RefreshFiles()
        {
            LoadFilesGridView();
        }

        private void LoadFEditTemplateFile(string template_name, string id)
        {
            old_row_num = FilesGridView.FocusedRowHandle;
            topindex = FilesGridView.TopRowIndex;

            int UsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.TEMPLATE_FILES WHERE ID = {ID}");
            bool FileUsed = (UsedUserID >= 0), CurrentStatus = false;

            if (FileUsed)
            {
                if (GlobalVariables.V_UserID != UsedUserID)
                {
                    string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == UsedUserID).FULLNAME;
                    XtraMessageBox.Show("Seçilmiş şablon fayl hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz.", "Seçilmiş faylın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CurrentStatus = true;
                }
                else
                    CurrentStatus = false;
            }
            else
                CurrentStatus = false;

            if (!CurrentStatus)
            {
                FEditTemplateFile fe = new FEditTemplateFile();
                fe.TemplateName = template_name;
                fe.TemplateID = id;
                fe.RefreshDataGridView += new FEditTemplateFile.DoEvent(RefreshFiles);
                fe.ShowDialog();
            }

            FilesGridView.FocusedRowHandle = old_row_num;
            FilesGridView.TopRowIndex = topindex;
        }

        private void LoadFile()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.TEMPLATE_FILE,T.NAME FROM CRS_USER.TEMPLATE_FILES T WHERE T.ID = {ID}");

            if (dt == null)
                return;

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["TEMPLATE_FILE"]))
                {
                    Byte[] BLOBData = (byte[])dr["TEMPLATE_FILE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    string filePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + dr["NAME"] + ".docx";
                    GlobalProcedures.DeleteFile(filePath);
                    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                    Process.Start(filePath);
                }
            }
        }

        private void ShowBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFile();
        }

        private void FilesGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GlobalProcedures.GridRowCellStyleForBlock(FilesGridView, e);
        }

        private void FilesGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFEditTemplateFile(Name, ID);
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFEditTemplateFile(Name, ID);
        }
    }
}
