using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRS.Class;

namespace CRS.Forms.Options
{
    public partial class UCColors : DevExpress.XtraEditors.XtraUserControl
    {
        public UCColors()
        {
            InitializeComponent();
        }
        int colorID;


        private void UCColors_Load(object sender, EventArgs e)
        {
            InsertColorsTemp();
            LoadColorsDataGridView();
        }

        private void LoadColorsDataGridView()
        {
            string s = $@"SELECT 1 SS,
                               LC.ID,
                               LCT.NAME,
                               LC.COLOR_VALUE_1,
                               LC.COLOR_VALUE_2
                          FROM CRS_USER_TEMP.LRS_COLORS_TEMP LC, CRS_USER.LRS_COLOR_TYPE LCT
                         WHERE LC.COLOR_TYPE_ID = LCT.ID AND LC.USER_ID = {GlobalVariables.V_UserID}
                        ORDER BY LC.COLOR_TYPE_ID";
            DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadColorsDataGridView", "Rənglər cədvələ yüklənmədi.");

            ColorsGridControl.DataSource = dt;
            EditBarButton.Enabled = (ColorsGridView.RowCount > 0);
        }

        private void InsertColorsTemp()
        {
            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_LRS_COLORS_TEMP", "P_USED_USER_ID", GlobalVariables.V_UserID, GlobalVariables.V_UserName + " istifadəçisinin sistem rəngləri temp cədvələ daxil olmadı");
        }

        private void ColorsGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }

        private void ColorsGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            int value1, value2;

            if (e.Column.Name == "Color1")
            {
                value1 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_1"));
                e.Appearance.BackColor = Color.FromArgb(value1);
            }

            if (e.Column.Name == "Color2")
            {
                value2 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_2"));
                e.Appearance.BackColor = Color.FromArgb(value2);
            }

            if (e.Column.Name == "Color3")
            {
                value1 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_1"));
                value2 = Convert.ToInt32(ColorsGridView.GetRowCellValue(e.RowHandle, "COLOR_VALUE_2"));
                e.Appearance.BackColor = Color.FromArgb(value1);
                e.Appearance.BackColor2 = Color.FromArgb(value2);
            }
        }

        private void ColorsGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = ColorsGridView.GetFocusedDataRow();
            if (row != null)
                colorID = Convert.ToInt32(row["ID"]);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadColorsDataGridView();
        }

        void RefreshColors()
        {
            LoadColorsDataGridView();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFColorEdit();
        }

        private void LoadFColorEdit()
        {
            FColorEdit fc = new FColorEdit();
            fc.ColorID = colorID;
            fc.RefreshColorsDataGridView += new FColorEdit.DoEvent(RefreshColors);
            fc.ShowDialog();
        }

        private void ColorsGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFColorEdit();
        }

        private void OtherBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FOtherUserColors fo = new FOtherUserColors();
            fo.RefreshColorsDataGridView += new FOtherUserColors.DoEvent(RefreshColors);
            fo.ShowDialog();
        }
    }
}
