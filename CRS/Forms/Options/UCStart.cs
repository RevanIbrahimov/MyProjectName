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
    public partial class UCStart : DevExpress.XtraEditors.XtraUserControl
    {
        public UCStart()
        {
            InitializeComponent();
        }

        private void MenuRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalVariables.V_DefaultMenu = MenuRadioGroup.SelectedIndex;
        }

        private void LanguageRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalVariables.V_DefaultLanguage = LanguageRadioGroup.SelectedIndex;
        }

        private void UCStart_Load(object sender, EventArgs e)
        {
            MenuRadioGroup.SelectedIndex = GlobalVariables.V_DefaultMenu;
            LanguageRadioGroup.SelectedIndex = GlobalVariables.V_DefaultLanguage;
            DateSortRadioGroup.SelectedIndex = GlobalVariables.V_DefaultDateSort;
        }

        private void GroupControl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DateSortRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalVariables.V_DefaultDateSort = DateSortRadioGroup.SelectedIndex;
        }
    }
}
