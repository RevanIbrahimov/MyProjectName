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
using System.IO;

namespace CRS.Forms.Options
{
    public partial class FEditTemplateFile : DevExpress.XtraEditors.XtraForm
    {
        public FEditTemplateFile()
        {
            InitializeComponent();
        }
        public string TemplateName, TemplateID;

        public delegate void DoEvent();
        public event DoEvent RefreshDataGridView;

        int UsedUserID = -1;
        bool FileUsed = false, CurrentStatus = false;

        private void FEditTemplateFile_Load(object sender, EventArgs e)
        {
            TemplateNameText.Text = TemplateName;
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TEMPLATE_FILES", GlobalVariables.V_UserID, "WHERE ID = " + TemplateID + " AND USED_USER_ID = -1");            
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ControlFileDetails()
        {
            bool b = false;

            
            if (String.IsNullOrEmpty(URLButtonEdit.Text.Trim()))
            {
                URLButtonEdit.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şablon fayl daxil edilməyib.");
                URLButtonEdit.Focus();
                URLButtonEdit.BackColor = GlobalFunctions.ElementColor(); ;
                return false;
            }
            else
                b = true;

            return b;
        }

        private void UpdateFile()
        {
            string sql = $@"UPDATE CRS_USER.TEMPLATE_FILES SET NOTE = '{NoteText.Text.Trim()}', TEMPLATE_FILE = :BlobFile WHERE ID = {TemplateID}";
            GlobalFunctions.ExecuteQueryWithBlob(sql, URLButtonEdit.Text.Trim(), "Şablon fayl bazada dəyişdirilmədi.");

            if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\" + TemplateName + ".docx"))
                File.Copy(URLButtonEdit.Text.Trim(), GlobalVariables.V_ExecutingFolder + "\\Documents\\" + TemplateName + ".docx");
        }

        private void FEditTemplateFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.TEMPLATE_FILES", -1, "WHERE ID = " + TemplateID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshDataGridView();
        }

        private void URLButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Şablon faylı seçin";
                    dlg.Filter = "All files (*.doc;*.docx)|*.doc;*.docx";

                    if (dlg.ShowDialog() == DialogResult.OK)
                        URLButtonEdit.Text = dlg.FileName;
                    dlg.Dispose();
                }
            }
            else if (e.Button.Index == 1)
                URLButtonEdit.Text = null;
            else
            {
                try
                {
                    if (URLButtonEdit.Text.Length != 0)
                        System.Diagnostics.Process.Start(URLButtonEdit.Text);
                }
                catch (Exception exx)
                {
                    GlobalProcedures.LogWrite("Faylın ünvanı düz deyil.", URLButtonEdit.Text, GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "/" + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);
                }
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlFileDetails())
            {
                UpdateFile();
                this.Close();
            }
        }
    }
}