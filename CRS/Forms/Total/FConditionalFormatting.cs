using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using CRS.Class;

namespace CRS.Forms.Total
{
    public partial class FConditionalFormatting : DevExpress.XtraEditors.XtraForm
    {
        public FConditionalFormatting()
        {
            InitializeComponent();
        }
        string FontID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        private void FormatGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GlobalProcedures.GridCustomColumnDisplayText(e);
        }
        
        private void FormatGridView_MouseUp(object sender, MouseEventArgs e)
        {
            GlobalProcedures.GridMouseUpForPopupMenu(FontGridView, PopupMenu, e);
        }

        void RefreshFonts()
        {
            LoadFontGridView();
        }

        private void LoadFConditionalFormattingAddEdit(string transaction, string fontid)
        {
            FConditionalFormattingAddEdit fc = new FConditionalFormattingAddEdit();
            fc.TransactionName = transaction;
            fc.FontID = fontid;
            fc.RefreshFontDataGridView += new FConditionalFormattingAddEdit.DoEvent(RefreshFonts);
            fc.ShowDialog();
        }

        private void NewBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFConditionalFormattingAddEdit("INSERT", null);
        }

        private void LoadFontGridView()
        {
            string s = "SELECT 1 SS,START_VALUE||' % -'||END_VALUE||' %',FONTNAME||', '||FONTSIZE||', '||FONTSTYLE FONT,NULL BACKCOLOR,NULL FORECOLOR,BACKCOLOR_A,BACKCOLOR_B,BACKCOLOR_G,BACKCOLOR_R,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_B,FONTCOLOR_G,FONTCOLOR_R,FONTCOLOR_TYPE,FONTCOLOR_NAME,USED_USER_ID,ID FROM CRS_USER.PORTFEL_FONTS ORDER BY START_VALUE,END_VALUE";
            try
            {   
                FontGridControl.DataSource = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFontGridView");
                FontGridView.PopulateColumns();
                FontGridView.Columns[0].Caption = "S/s";
                FontGridView.Columns[1].Caption = "Faiz intervalı";
                FontGridView.Columns[2].Caption = "Font";
                FontGridView.Columns[3].Caption = "Arxa fonun rəngi";
                FontGridView.Columns[4].Caption = "Şriftin rəngi";
                FontGridView.Columns[3].Visible = false;
                FontGridView.Columns[4].Visible = false;
                FontGridView.Columns[5].Visible = false;
                FontGridView.Columns[6].Visible = false;
                FontGridView.Columns[7].Visible = false;
                FontGridView.Columns[8].Visible = false;
                FontGridView.Columns[9].Visible = false;
                FontGridView.Columns[10].Visible = false;
                FontGridView.Columns[11].Visible = false;
                FontGridView.Columns[12].Visible = false;
                FontGridView.Columns[13].Visible = false;
                FontGridView.Columns[14].Visible = false;
                FontGridView.Columns[15].Visible = false;
                FontGridView.Columns[16].Visible = false;
                FontGridView.Columns[17].Visible = false;
                FontGridView.Columns[18].Visible = false;

                //TextAligment
                FontGridView.Columns[0].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                FontGridView.Columns[0].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                FontGridView.Columns[1].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                FontGridView.Columns[1].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                //HeaderAligment
                for (int i = 0; i < FontGridView.Columns.Count; i++)
                {
                    FontGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    FontGridView.Columns[i].AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                }

                EditBarButton.Enabled = DeleteBarButton.Enabled = (FontGridView.RowCount > 0);
                
                FontGridView.BestFitColumns();
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Şərtlər cədvələ yüklənmədi.", s, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void FConditionalFormatting_Load(object sender, EventArgs e)
        {
            LoadFontGridView();
        }

        private void FontGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        { 
            GlobalProcedures.GridRowCellStyleForBlock(FontGridView, e);
        }

        private void RefreshBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFontGridView();
        }

        private void DeleteBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Seçilmiş şərti silmək istəyirsiniz?", "Şərtin silinməsi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER.PORTFEL_FONTS WHERE ID = {FontID}", "Şərt silinmədi.", this.Name + "/DeleteBarButton_ItemClick");
            LoadFontGridView();
        }

        private void FontGridView_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow row = FontGridView.GetFocusedDataRow();
            if (row != null)
                FontID = row["ID"].ToString();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadFConditionalFormattingAddEdit("EDIT", FontID);
        }

        private void FontGridView_DoubleClick(object sender, EventArgs e)
        {
            if (EditBarButton.Enabled)
                LoadFConditionalFormattingAddEdit("EDIT", FontID);
        }

        private void FontGridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            string BA, BB, BG, BR, BT, BN, FA, FB, FG, FR, FT, FN;
            BA = currentView.GetRowCellDisplayText(e.RowHandle, "BACKCOLOR_A").ToString();
            BB = currentView.GetRowCellDisplayText(e.RowHandle, "BACKCOLOR_B").ToString();
            BG = currentView.GetRowCellDisplayText(e.RowHandle, "BACKCOLOR_G").ToString();
            BR = currentView.GetRowCellDisplayText(e.RowHandle, "BACKCOLOR_R").ToString();
            BT = currentView.GetRowCellDisplayText(e.RowHandle, "BACKCOLOR_TYPE").ToString();
            BN = currentView.GetRowCellDisplayText(e.RowHandle, "BACKCOLOR_NAME").ToString();


            FA = currentView.GetRowCellDisplayText(e.RowHandle, "FONTCOLOR_A").ToString();
            FB = currentView.GetRowCellDisplayText(e.RowHandle, "FONTCOLOR_B").ToString();
            FG = currentView.GetRowCellDisplayText(e.RowHandle, "FONTCOLOR_G").ToString();
            FR = currentView.GetRowCellDisplayText(e.RowHandle, "FONTCOLOR_R").ToString();
            FT = currentView.GetRowCellDisplayText(e.RowHandle, "FONTCOLOR_TYPE").ToString();
            FN = currentView.GetRowCellDisplayText(e.RowHandle, "FONTCOLOR_NAME").ToString();

            e.Appearance.BackColor = GlobalFunctions.CreateColor(BA, BR, BG, BB, BT, BN);
            e.Appearance.ForeColor = GlobalFunctions.CreateColor(FA, FR, FG, FB, FT, FN);
        }

        private void FontGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
            EditBarButton.Enabled = DeleteBarButton.Enabled = (FontGridView.RowCount > 0);            
        }

        private void FConditionalFormatting_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshDataGridView();
        }
    }
}