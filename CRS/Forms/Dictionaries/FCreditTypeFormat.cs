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
    public partial class FCreditTypeFormat : DevExpress.XtraEditors.XtraForm
    {
        public FCreditTypeFormat()
        {
            InitializeComponent();
        }
        public string TypeID;

        public delegate void DoEvent();
        public event DoEvent RefreshFont;

        private void FCreditTypeFormat_Load(object sender, EventArgs e)
        {
            GlobalProcedures.LoadFontStyleComboBox(StyleComboBox);
            LoadFontDetails();
        }

        private void LoadFontDetails()
        {
            string s = $@"SELECT FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_B,BACKCOLOR_G,BACKCOLOR_R,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_B,FONTCOLOR_G,FONTCOLOR_R,FONTCOLOR_TYPE,FONTCOLOR_NAME FROM CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP WHERE CREDIT_TYPE_ID = {TypeID}";
            try
            {
                DataTable dt = GlobalFunctions.GenerateDataTable(s, this.Name + "/LoadFontDetails");

                foreach (DataRow dr in dt.Rows)
                {
                    FontNameText.Text = dr[0].ToString();
                    StyleComboBox.EditValue = dr[2].ToString();
                    BackColorValue.Color = GlobalFunctions.CreateColor(dr[3].ToString(), dr[6].ToString(), dr[5].ToString(), dr[4].ToString(), dr[7].ToString(), dr[8].ToString());
                    ForeColorValue.Color = GlobalFunctions.CreateColor(dr[9].ToString(), dr[12].ToString(), dr[11].ToString(), dr[10].ToString(), dr[13].ToString(), dr[14].ToString());
                }
            }
            catch (Exception exx)
            {
                GlobalProcedures.LogWrite("Font açılmadı.", s, GlobalVariables.V_UserName, this.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCreditTypeFormat_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshFont();
        }

        private void ChangeFont()
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

            int count = GlobalFunctions.GetCount($@"SELECT COUNT(*) FROM CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP WHERE CREDIT_TYPE_ID = {TypeID}");
            if (count == 0)
                GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP(ID,CREDIT_TYPE_ID,FONTNAME,FONTSIZE,FONTSTYLE,BACKCOLOR_A,BACKCOLOR_B,BACKCOLOR_G,BACKCOLOR_R,BACKCOLOR_TYPE,BACKCOLOR_NAME,FONTCOLOR_A,FONTCOLOR_B,FONTCOLOR_G,FONTCOLOR_R,FONTCOLOR_TYPE,FONTCOLOR_NAME,USED_USER_ID,IS_CHANGE)VALUES(CREDIT_TYPE_FONT_SEQUENCE.NEXTVAL," + TypeID + ",'" + FontNameText.Text.Trim() + "',8,'" + StyleComboBox.Text + "'," + BackColorValue.Color.A + "," + BackColorValue.Color.B + "," + BackColorValue.Color.G + "," + BackColorValue.Color.R + ",'" + bankcolor_type + "','" + BackColorValue.Color.Name + "'," + ForeColorValue.Color.A + "," + ForeColorValue.Color.B + "," + ForeColorValue.Color.G + "," + ForeColorValue.Color.R + ",'" + fontcolor_type + "','" + ForeColorValue.Color.Name + "'," + GlobalVariables.V_UserID + ",1)",
                                                        "Font daxil edilmədi.",
                                                        this.Name + "/ChangeFont");
            else
                GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.CREDIT_TYPE_FONTS_TEMP SET FONTNAME = '{FontNameText.Text.Trim()}',
                                                                                            FONTSIZE = 8,
                                                                                            FONTSTYLE = '{StyleComboBox.Text}',
                                                                                            BACKCOLOR_A = {BackColorValue.Color.A},
                                                                                            BACKCOLOR_B = {BackColorValue.Color.B},
                                                                                            BACKCOLOR_G = {BackColorValue.Color.G},
                                                                                            BACKCOLOR_R = {BackColorValue.Color.R},
                                                                                            BACKCOLOR_TYPE = '{bankcolor_type}',
                                                                                            BACKCOLOR_NAME = '{BackColorValue.Color.Name}',
                                                                                            FONTCOLOR_A = {ForeColorValue.Color.A},
                                                                                            FONTCOLOR_B = {ForeColorValue.Color.B},
                                                                                            FONTCOLOR_G = {ForeColorValue.Color.G },
                                                                                            FONTCOLOR_R = {ForeColorValue.Color.R},
                                                                                            FONTCOLOR_TYPE = '{fontcolor_type}',
                                                                                            FONTCOLOR_NAME = '{ForeColorValue.Color.Name}',
                                                                                            IS_CHANGE = 1 
                                                WHERE CREDIT_TYPE_ID = {TypeID} AND USED_USER_ID = {GlobalVariables.V_UserID}",
                                                        "Font dəyişdirilmədi.",
                                                        this.Name + "/ChangeFont");
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            ChangeFont();
            this.Close();
        }

        private void ForeColorValue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ForeColorValue.Color = Color.FromArgb(0, 0, 0, 0);
        }

        private void BackColorValue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            BackColorValue.Color = Color.FromArgb(0, 0, 0, 0);
        }
    }
}