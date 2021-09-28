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
using Bytescout.Document;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CRS.Forms.Total
{
    public partial class FWarningLetter : DevExpress.XtraEditors.XtraForm
    {
        public FWarningLetter()
        {
            InitializeComponent();
        }
        public string CustomerName, ContractDate, ContractNumber, ContractAmount, PaymentDay, CarNumber, BrandAndModel, Delayes, ContractID, HostageName;
        public int CreditNameID;
       
        private void BOK_Click(object sender, EventArgs e)
        {
            if (ControlFileDetails())
            {
                GenerateFile();
                this.Close();
            }
        }

        private void GenerateFile()
        {
            string conNumber = Regex.Replace(ContractNumber, "[^0-9A-Z]", "");

            if (CreditNameID == 1)//avtomobil
            {
                if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Xəbərdarlıq.docx"))
                {
                    GlobalVariables.WordDocumentUsed = false;
                    XtraMessageBox.Show("Xəbərdarlıq məktubunun şablon faylı tapılmadı.");
                    return;
                }                

                try
                {
                    Document document = new Document();
                    document.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\Xəbərdarlıq.docx");
                    document.ReplaceText("[$code]", LetterCodeText.Text);
                    document.ReplaceText("[$date]", DateValue.Text);
                    document.ReplaceText("[$customername]", CustomerName);
                    document.ReplaceText("[$contractnumber]", ContractNumber);
                    document.ReplaceText("[$contractdate]", ContractDate);
                    document.ReplaceText("[$contractamount]", ContractAmount);
                    document.ReplaceText("[$paymentday]", PaymentDay);
                    document.ReplaceText("[$brend]", HostageName);
                    document.ReplaceText("[$carnumber]", CarNumber);
                    document.ReplaceText("[$delays]", Delayes);

                    if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx"))
                        File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx");

                    document.Save(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx");

                    string insert = $@"INSERT INTO CRS_USER.WARNING_LETTERS(CONTRACT_ID,LETTER_CODE,LETTER_DATE,LETTER_FILE,INSERT_USER)
                                   VALUES({ContractID},'{LetterCodeText.Text.Trim()}',TO_DATE('{DateValue.Text}','DD.MM.YYYY'),:BlobFile,{GlobalVariables.V_UserID})";
                    GlobalFunctions.ExecuteQueryWithBlob(insert, GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx", "Xəbərdarlıq məktubu bazada saxlanılmadı.", this.Name + "/GenerateFile");
                    Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx");
                }
                catch (Exception exx)
                {
                    XtraMessageBox.Show(conNumber + "_Xəbərdarlıq.docx faylı yaradılmadı.\r\n" + exx.Message);
                }
            }
            else //dasinmaz emlak
            {
                if (!File.Exists(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mənzil üçün xəbərdarlıq.docx"))
                {
                    GlobalVariables.WordDocumentUsed = false;
                    XtraMessageBox.Show("Xəbərdarlıq məktubunun şablon faylı tapılmadı.");
                    return;
                }

                try
                {
                    Document document = new Document();
                    document.Open(GlobalVariables.V_ExecutingFolder + "\\Documents\\Mənzil üçün xəbərdarlıq.docx");
                    document.ReplaceText("[$code]", LetterCodeText.Text);
                    document.ReplaceText("[$date]", DateValue.Text);
                    document.ReplaceText("[$customername]", CustomerName);
                    document.ReplaceText("[$contractnumber]", ContractNumber);
                    document.ReplaceText("[$contractdate]", ContractDate);
                    document.ReplaceText("[$contractamount]", ContractAmount);
                    document.ReplaceText("[$paymentday]", PaymentDay);
                    document.ReplaceText("[$apartment]", HostageName);
                    document.ReplaceText("[$delays]", Delayes);

                    if (File.Exists(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx"))
                        File.Delete(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx");

                    document.Save(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx");

                    string insert = $@"INSERT INTO CRS_USER.WARNING_LETTERS(CONTRACT_ID,LETTER_CODE,LETTER_DATE,LETTER_FILE,INSERT_USER)
                                   VALUES({ContractID},'{LetterCodeText.Text.Trim()}',TO_DATE('{DateValue.Text}','DD.MM.YYYY'),:BlobFile,{GlobalVariables.V_UserID})";
                    GlobalFunctions.ExecuteQueryWithBlob(insert, GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx", "Xəbərdarlıq məktubu bazada saxlanılmadı.", this.Name + "/GenerateFile");
                    Process.Start(GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + conNumber + "_Xəbərdarlıq.docx");
                }
                catch (Exception exx)
                {
                    XtraMessageBox.Show(conNumber + "_Xəbərdarlıq.docx faylı yaradılmadı.\r\n" + exx.Message);
                }
            }
        }

        private void FWarningLetter_Load(object sender, EventArgs e)
        {
            DateValue.DateTime = DateTime.Today;
        }

        private bool ControlFileDetails()
        {
            bool b = false;

            if (String.IsNullOrEmpty(LetterCodeText.Text))
            {
                LetterCodeText.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məktubun nömrəsi daxil edilməyib.");
                LetterCodeText.Focus();
                LetterCodeText.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            if (String.IsNullOrEmpty(DateValue.Text))
            {
                DateValue.BackColor = Color.Red;
                GlobalProcedures.ShowErrorMessage("Məktubun tarixi daxil edilməyib.");
                DateValue.Focus();
                DateValue.BackColor = GlobalFunctions.ElementColor();
                return false;
            }
            else
                b = true;

            return b;
        }
    }
}