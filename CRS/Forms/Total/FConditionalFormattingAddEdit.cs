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

namespace CRS.Forms.Total
{
    public partial class FConditionalFormattingAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FConditionalFormattingAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, FontID;
        bool CurrentStatus = false, FontUsed = false;
        int FontUsedUserID = -1;

        public delegate void DoEvent();
        public event DoEvent RefreshFontDataGridView;
        
        private void FConditionalFormattingAddEdit_Load(object sender, EventArgs e)
        {
            if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.PORTFEL_FONTS") > 0)
            {
                StartPercentValue.Value = (decimal)GlobalFunctions.GetAmount("SELECT END_VALUE + 0.01 FROM CRS_USER.PORTFEL_FONTS WHERE ID = (SELECT MAX(ID) FROM CRS_USER.PORTFEL_FONTS)");
                StartPercentValue.Enabled = false;
            }
            else
                StartPercentValue.Enabled = true;
            
            GlobalProcedures.LoadFontStyleComboBox(StyleComboBox);
            if (TransactionName == "INSERT")
                FontID = GlobalFunctions.GetOracleSequenceValue("PORTFEL_FONT_SEQUENCE").ToString();
            else
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PORTFEL_FONTS", GlobalVariables.V_UserID, "WHERE ID = " + FontID + " AND USED_USER_ID = -1");
                FontUsedUserID = GlobalFunctions.GetID($@"SELECT USED_USER_ID FROM CRS_USER.PORTFEL_FONTS WHERE ID = {FontID}");
                FontUsed = (FontUsedUserID > 0);                

                if (FontUsed)
                {
                    if (GlobalVariables.V_UserID != FontUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == FontUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş şərt hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş şərtin hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadFontDetails();
            }
        }

        private void ComponentEnabled(bool status)
        {
            FontNameText.Enabled = !status;
            StyleComboBox.Enabled = !status;
            FontSizeValue.Enabled = !status;
            ForeColorValue.Enabled = !status;
            BackColorValue.Enabled = !status;
            BFont.Enabled = !status;
            if (GlobalFunctions.GetCount("SELECT COUNT(*) FROM CRS_USER.PORTFEL_FONTS WHERE ID > " + FontID) > 0)
            {
                StartPercentValue.Enabled = false;
                EndPercentValue.Enabled = false;
            }
            else
            {
                //StartPercentValue.Enabled = !status;
                EndPercentValue.Enabled = !status;
            }
            BOK.Visible = !status;
        }

        private void LoadFontDetails()
        {
            string s = $@"SELECT START_VALUE,END_VALUE,FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_B,BACKCOLOR_G,BACKCOLOR_R,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_B,FONTCOLOR_G,FONTCOLOR_R,FONTCOLOR_TYPE,FONTCOLOR_NAME FROM CRS_USER.PORTFEL_FONTS WHERE ID = {FontID}";
            try
            {                
                DataTable dt = GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    StartPercentValue.Value = Convert.ToDecimal(dr[0].ToString());
                    EndPercentValue.Value = Convert.ToDecimal(dr[1].ToString());
                    FontNameText.Text = dr[2].ToString();
                    FontSizeValue.Value = Convert.ToInt32(dr[3].ToString());
                    StyleComboBox.EditValue = dr[4].ToString();
                    BackColorValue.Color = GlobalFunctions.CreateColor(dr[5].ToString(), dr[8].ToString(), dr[7].ToString(), dr[6].ToString(), dr[9].ToString(), dr[10].ToString());
                    ForeColorValue.Color = GlobalFunctions.CreateColor(dr[11].ToString(), dr[14].ToString(), dr[13].ToString(), dr[12].ToString(), dr[15].ToString(), dr[16].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Font açılmadı.", s, GlobalVariables.V_UserName, "Class", System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FConditionalFormattingAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.PORTFEL_FONTS", -1, "WHERE ID = " + FontID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshFontDataGridView();
        }

        private void BFont_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog(); 
            if (dlg.ShowDialog() == DialogResult.OK)
            {                
                FontNameText.Text = dlg.Font.Name;
                FontSizeValue.Value = Math.Round((decimal)dlg.Font.Size,0);
                StyleComboBox.EditValue = dlg.Font.Style.ToString();                
            }
        }

        private void ForeColorValue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ForeColorValue.Color = Color.FromArgb(0, 0, 0, 0);
        }

        private void BackColorValue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            BackColorValue.Color = Color.FromArgb(0, 0, 0, 0);
        }

        private void InsertFont()
        {
            string bankcolor_type, fontcolor_type;
            if (BackColorValue.Color.IsNamedColor)
                bankcolor_type = "Name";
            else
                bankcolor_type = "Number";

            if (ForeColorValue.Color.IsNamedColor)
                fontcolor_type = "Name";
            else
                fontcolor_type = "Number";

            GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER.PORTFEL_FONTS(ID,START_VALUE,END_VALUE,FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_B,BACKCOLOR_G,BACKCOLOR_R,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_B,FONTCOLOR_G,FONTCOLOR_R,FONTCOLOR_TYPE,FONTCOLOR_NAME)VALUES(" + FontID + "," + StartPercentValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + "," + EndPercentValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",'" + FontNameText.Text.Trim() + "'," + FontSizeValue.Value + ",'" + StyleComboBox.Text + "'," + BackColorValue.Color.A + "," + BackColorValue.Color.B + "," + BackColorValue.Color.G + "," + BackColorValue.Color.R + ",'" + bankcolor_type + "','" + BackColorValue.Color.Name + "'," + ForeColorValue.Color.A + "," + ForeColorValue.Color.B + "," + ForeColorValue.Color.G + "," + ForeColorValue.Color.R + ",'" + fontcolor_type + "','" + ForeColorValue.Color.Name + "')",
                                                "Font daxil edilmədi.");
        }

        private void UpdateFont()
        {
            string bankcolor_type, fontcolor_type;
            if (BackColorValue.Color.IsNamedColor)
                bankcolor_type = "Name";
            else
                bankcolor_type = "Number";

            if (ForeColorValue.Color.IsNamedColor)
                fontcolor_type = "Name";
            else
                fontcolor_type = "Number";

            GlobalProcedures.ExecuteQuery("UPDATE CRS_USER.PORTFEL_FONTS SET START_VALUE = " + StartPercentValue.Value.ToString(GlobalVariables.V_CultureInfoEN) + ",END_VALUE = " + EndPercentValue.Value.ToString(Class.GlobalVariables.V_CultureInfoEN) + ",FONTNAME = '" + FontNameText.Text.Trim() + "',FONTSIZE = " + FontSizeValue.Value + ",FONTSTYLE = '" + StyleComboBox.Text + "',BACKCOLOR_A = " + BackColorValue.Color.A + ",BACKCOLOR_B = " + BackColorValue.Color.B + ",BACKCOLOR_G = " + BackColorValue.Color.G + ",BACKCOLOR_R = " + BackColorValue.Color.R + ",BACKCOLOR_TYPE = '" + bankcolor_type + "',BACKCOLOR_NAME = '" + BackColorValue.Color.Name + "',FONTCOLOR_A = " + ForeColorValue.Color.A + ",FONTCOLOR_B = " + ForeColorValue.Color.B + ",FONTCOLOR_G = " + ForeColorValue.Color.G + ",FONTCOLOR_R = " + ForeColorValue.Color.R + ",FONTCOLOR_TYPE = '" + fontcolor_type + "',FONTCOLOR_NAME = '" + ForeColorValue.Color.Name + "' WHERE ID = " + FontID,
                                                "Font dəyişdirilmədi.");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (TransactionName == "INSERT")
                InsertFont();
            else
                UpdateFont();
            this.Close();
        }
    }
}