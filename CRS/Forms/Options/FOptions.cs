using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml;
using CRS.Class;

namespace CRS.Forms.Options
{
    public partial class FOptions : DevExpress.XtraEditors.XtraForm
    {
        public FOptions()
        {
            InitializeComponent();
        }

        private UCStart StartProgram = new UCStart();
        private UCColors Colors = new UCColors();
        private UCTemplateFiles TemplateFiles = new UCTemplateFiles();

        private void FOptions_Load(object sender, EventArgs e)
        {
            OptionsNavBarControl.SelectedLink = OptionsNavBarControl.Groups[0].ItemLinks[0];
            GlobalProcedures.ShowUserControl(OptionSplitContainerControl, StartProgram);
        }
        
        private void StartNavBar_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GlobalProcedures.ShowUserControl(OptionSplitContainerControl, StartProgram);
        }

        private void ColorsNavBar_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GlobalProcedures.ShowUserControl(OptionSplitContainerControl, Colors);
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void BOK_Click(object sender, EventArgs e)
        {
            UpdateColor();            
            this.Close();
        }

        private void UpdateColor()
        {
            GlobalProcedures.ExecuteProcedureWithParametr("CRS_USER.PROC_UPDATE_LRS_COLORS", "P_USED_USER_ID", GlobalVariables.V_UserID, "Rənglər əsas cədvəldə dəyişdirilmədi.");
            GlobalProcedures.LoadLrsColor();
        }
        
        private void TemplateFileNavBar_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GlobalProcedures.ShowUserControl(OptionSplitContainerControl, TemplateFiles);
        }

        private void FOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.ExecuteQuery($@"DELETE FROM CRS_USER_TEMP.LRS_COLORS_TEMP WHERE USED_USER_ID = {GlobalVariables.V_UserID}", 
                                            "Rənglər temp cədvəldən silinmədi.",
                                            this.Name + "/FOptions_FormClosing");
        }
    }
}