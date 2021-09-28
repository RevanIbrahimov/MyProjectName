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

namespace CRS.Forms.Options
{
    public partial class FColorEdit : DevExpress.XtraEditors.XtraForm
    {
        public FColorEdit()
        {
            InitializeComponent();
        }
        public int ColorID;

        public delegate void DoEvent();
        public event DoEvent RefreshColorsDataGridView;

        private void FColorEdit_Load(object sender, EventArgs e)
        {
            LoadColor();
        }

        private void LoadColor()
        {
            string sql = $@"SELECT LCT.NAME,
                                    LC.COLOR_VALUE_1,
                                    LC.COLOR_VALUE_2
                                  FROM CRS_USER_TEMP.LRS_COLORS_TEMP LC, CRS_USER.LRS_COLOR_TYPE LCT
                                 WHERE LC.COLOR_TYPE_ID = LCT.ID AND LC.ID = {ColorID}";

            DataTable dt = GlobalFunctions.GenerateDataTable(sql);

            foreach(DataRow dr in dt.Rows)
            {
                ColorTypeLabel.Text = dr["NAME"].ToString();
                Color1.Color = Color.FromArgb(Convert.ToInt32(dr["COLOR_VALUE_1"].ToString()));
                Color2.Color = Color.FromArgb(Convert.ToInt32(dr["COLOR_VALUE_2"].ToString()));
                ColorControl.Appearance.BackColor = Color1.Color;
                ColorControl.Appearance.BackColor2 = Color2.Color;
            }
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FColorEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshColorsDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            int color1 = Color1.Color.ToArgb(), color2 = Color2.Color.ToArgb();
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER_TEMP.LRS_COLORS_TEMP SET COLOR_VALUE_1 = {color1}, COLOR_VALUE_2 = {color2} WHERE ID = {ColorID}", "Rəng dəyişdirilmədi.");
            this.Close();
        }

        private void Color1_EditValueChanged(object sender, EventArgs e)
        {
            ColorControl.Appearance.BackColor = Color1.Color;
            ColorControl.Appearance.BackColor2 = Color2.Color;
        }
    }
}