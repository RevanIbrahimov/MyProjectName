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
using CRS.Class.Tables;
using CRS.Class.DataAccess;

namespace CRS.Forms.Dictionaries
{
    public partial class FCardSeriesAddEdit : DevExpress.XtraEditors.XtraForm
    {
        public FCardSeriesAddEdit()
        {
            InitializeComponent();
        }
        public string TransactionName, SeriesID;
        bool CurrentStatus = false, SeriesUsed = false;
        int SeriesUsedUserID = -1;
        string old_name = null,
              old_serie = null;

        List<CardSeries> lstSeries = null;

        public delegate void DoEvent();
        public event DoEvent RefreshCardSeriesDataGridView;

        private void FCardSeriesAddEdit_Load(object sender, EventArgs e)
        {
            if (TransactionName == "EDIT")
            {
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CARD_SERIES", GlobalVariables.V_UserID, "WHERE ID = " + SeriesID + " AND USED_USER_ID = -1");
                lstSeries = CardSeriesDAL.SelectSerieByID(int.Parse(SeriesID)).ToList<CardSeries>();
                SeriesUsedUserID = lstSeries.First().USED_USER_ID;
                SeriesUsed = (SeriesUsedUserID > 0);
                
                if (SeriesUsed)
                {
                    if (GlobalVariables.V_UserID != SeriesUsedUserID)
                    {
                        string used_user_name = GlobalVariables.lstUsers.Find(u => u.ID == SeriesUsedUserID).FULLNAME;
                        XtraMessageBox.Show("Seçilmiş seriya hal-hazırda " + used_user_name + " tərəfindən istifadə edilir. Onun məlumatları dəyişdirilə bilməz. Siz yalnız məlumatlara baxa bilərsiniz.", "Seçilmiş seriyanın hal-hazırkı statusu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CurrentStatus = true;
                    }
                    else
                        CurrentStatus = false;
                }
                else
                    CurrentStatus = false;
                ComponentEnabled(CurrentStatus);
                LoadSeriesDetails();
            }
        }

        private void ComponentEnabled(bool status)
        {
            NameText.Enabled =
                SeriesText.Enabled =
                NoteText.Enabled =
                BOK.Visible = !status;
        }

        private void LoadSeriesDetails()
        {
            var serie = lstSeries.First();
            NameText.Text = serie.NAME;
            NoteText.Text = serie.NOTE;
            SeriesText.EditValue = serie.SERIES;
            old_name = serie.NAME;
            old_serie = serie.SERIES;
        }

        private bool ControlSeriesDetails()
        {
            bool b = false;
            if (TransactionName == "INSERT" || old_name != NameText.Text.Trim() || old_serie != SeriesText.Text.Trim())
                lstSeries = CardSeriesDAL.SelectSerieByID(null).ToList<CardSeries>();

            if (NameText.Text.Length == 0)
            {
                NameText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin adı daxil edilməyib.");
                NameText.Focus();
                NameText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (SeriesText.Text.Length == 0)
            {
                SeriesText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Şəxsiyyəti təsdiq edən sənədin seriyası daxil edilməyib.");
                SeriesText.Focus();
                SeriesText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            bool b1 = old_serie != SeriesText.Text.Trim();
            bool b2 = lstSeries.Exists(item => item.SERIES == SeriesText.Text.Trim());
            bool b3 = b1 && b2;

            if (b3)
            {
                SeriesText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Bu seriya artıq digər sənədlər üçün daxil edilib. Zəhmət olmasa digər seriya daxil edin.");
                SeriesText.Focus();
                SeriesText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }

        private void InsertSeries()
        {
            int order = GlobalFunctions.GetMax("SELECT NVL(MAX(ORDER_ID),0) FROM CRS_USER.CARD_SERIES") + 1;
            GlobalProcedures.ExecuteQuery($@"INSERT INTO CRS_USER.CARD_SERIES(ID,
                                                                                NAME,
                                                                                SERIES,
                                                                                NOTE,
                                                                                ORDER_ID) 
                                            VALUES(CARD_SERIES_SEQUENCE.NEXTVAL,
                                                    '{NameText.Text.Trim()}',
                                                    '{SeriesText.Text.Trim()}',
                                                    '{NoteText.Text.Trim()}',
                                                    {order})",
                                            "Seriya daxil edilmədi.");
        }

        private void UpdateSeries()
        {
            GlobalProcedures.ExecuteQuery($@"UPDATE CRS_USER.CARD_SERIES SET NAME ='{NameText.Text.Trim()}',
                                                                            NOTE ='{NoteText.Text.Trim()}',
                                                                            SERIES = '{SeriesText.Text.Trim()}' 
                                                       WHERE ID = {SeriesID}",
                                                "Seriya dəyişdirilmədi.");
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCardSeriesAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TransactionName == "EDIT")
                GlobalProcedures.Lock_or_UnLock_UserID("CRS_USER.CARD_SERIES", -1, "WHERE ID = " + SeriesID + " AND USED_USER_ID = " + GlobalVariables.V_UserID);
            this.RefreshCardSeriesDataGridView();
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlSeriesDetails())
            {
                if (TransactionName == "INSERT")
                    InsertSeries();
                else
                    UpdateSeries();
                this.Close();
            }
        }
    }
}