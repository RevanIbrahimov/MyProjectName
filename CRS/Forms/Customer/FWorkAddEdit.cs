using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CRS.Forms.Customer
{
    public partial class FWorkAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FWorkAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, WorkID, CustomerID;        

        public delegate void DoEvent();
        public event DoEvent RefresWorkPlaceDataGridView;

        private void FWorkAddEdit_Load(object sender, EventArgs e)
        {  
            if (TransactionName == "EDIT")
                LoadWorkDetails();
        }

        private void LoadWorkDetails()
        {
            string s = "SELECT PLACE_NAME,POSITION,START_DATE,END_DATE,NOTE FROM CRS_USER_TEMP.CUSTOMER_WORKPLACE_TEMP WHERE ID = " + WorkID;
            try
            {
                DataTable dt = Class.GlobalFunctions.GenerateDataTable(s);

                foreach (DataRow dr in dt.Rows)
                {
                    OfficeText.Text = dr[0].ToString();
                    PositionText.Text = dr[1].ToString();
                    StartDate.EditValue = DateTime.Parse(dr[2].ToString());
                    EndDate.EditValue = DateTime.Parse(dr[3].ToString());
                    NoteText.Text = dr[4].ToString();
                }  
            }
            catch (Exception exx)
            {
                Class.GlobalProcedures.LogWrite("İş yerinin detalları tapılmadı.", s, Class.GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }

        private void InsertWork()
        {           
            Class.GlobalProcedures.ExecuteQuery("INSERT INTO CRS_USER_TEMP.CUSTOMER_WORKPLACE_TEMP(ID,CUSTOMER_ID,PLACE_NAME,POSITION,START_DATE,END_DATE,NOTE,USED_USER_ID,IS_CHANGE) VALUES(CUSTOMER_WORKPLACE_SEQUENCE.NEXTVAL," + CustomerID + ",'" + OfficeText.Text.Trim() + "','" + PositionText.Text.Trim() + "',TO_DATE('" + StartDate.Text + "','DD/MM/YYYY'),TO_DATE('" + EndDate.Text + "','DD/MM/YYYY'),'" + NoteText.Text.Trim() + "'," + Class.GlobalVariables.V_UserID + ",1)", "İş yeri daxil edilmədi.");
        }

        private void UpdateWork()
        {
            Class.GlobalProcedures.ExecuteQuery("UPDATE CRS_USER_TEMP.CUSTOMER_WORKPLACE_TEMP SET IS_CHANGE = 1, PLACE_NAME = '" + OfficeText.Text.Trim() + "',POSITION = '" + PositionText.Text.Trim() + "',START_DATE = TO_DATE('" + StartDate.Text + "','DD/MM/YYYY'),END_DATE = TO_DATE('" + EndDate.Text + "','DD/MM/YYYY'),NOTE = '" + NoteText.Text.Trim() + "' WHERE ID = " + WorkID, "İş yeri dəyişdirilmədi.");
        }

        private bool ControlWorkDetails()
        {
            bool b = false;

            if (OfficeText.Text.Length == 0)
            {
                OfficeText.BackColor = Color.Red;
                Class.GlobalProcedures.ShowErrorMessage("İş yerinin adı daxil edilməyib.");               
                OfficeText.Focus();
                OfficeText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (PositionText.Text.Length == 0)
            {
                PositionText.BackColor = Color.Red;
                Class.GlobalProcedures.ShowErrorMessage("Vəzifə daxil edilməyib.");                
                PositionText.Focus();
                PositionText.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (StartDate.Text.Length == 0)
            {
                StartDate.BackColor = Color.Red;
                Class.GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");                
                StartDate.Focus();
                StartDate.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (EndDate.Text.Length == 0)
            {
                EndDate.BackColor = Color.Red;
                Class.GlobalProcedures.ShowErrorMessage("Tarix daxil edilməyib.");                
                EndDate.Focus();
                EndDate.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (Class.GlobalFunctions.ChangeStringToDate(StartDate.Text, "ddmmyyyy") == Class.GlobalFunctions.ChangeStringToDate(EndDate.Text, "ddmmyyyy"))
            {
                StartDate.BackColor = Color.Red;
                EndDate.BackColor = Color.Red;
                Class.GlobalProcedures.ShowErrorMessage("Başlanğıc tarixi ilə son tarix eyni ola bilməz.");                
                StartDate.Focus();
                StartDate.BackColor = Class.GlobalFunctions.ElementColor();
                EndDate.BackColor = Class.GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FWorkAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefresWorkPlaceDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if(ControlWorkDetails())
            {
                if (TransactionName == "INSERT")
                    InsertWork();
                else
                    UpdateWork();
                this.Close();
            }
        }
    }
}