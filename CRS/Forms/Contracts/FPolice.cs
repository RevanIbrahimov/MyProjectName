using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CRS.Forms.Contracts
{
    public partial class FPolice : DevExpress.XtraEditors.XtraForm
    {
        public FPolice()
        {
            InitializeComponent();
        }
        public string ContractCode, Customer, CarNumber, Car, Card, DateAndContract, CarWihtoutBan, PObject, ObjectCount, Type, BanType, Year, Ban, PColor, Brand,
            EngineNumber, ChassisNumber;

        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FPolice_Load(object sender, EventArgs e)
        {
            Class.GlobalProcedures.DateEditFormat(CreatedDate);            
            CreatedDate.EditValue = DateTime.Today;
            Class.GlobalProcedures.DateEditFormat(PeriodValidityDate);            
            PeriodValidityDate.EditValue = DateTime.Today;
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            string day = String.Format("{0:dd}", Class.GlobalFunctions.ChangeStringToDate(CreatedDate.Text, "ddmmyyyy")),
                month = Class.GlobalFunctions.FindMonth(Class.GlobalFunctions.ChangeStringToDate(CreatedDate.Text, "ddmmyyyy").Month),
                year = Class.GlobalFunctions.FindYear(Class.GlobalFunctions.ChangeStringToDate(CreatedDate.Text, "ddmmyyyy"));
            int code_number = int.Parse(Regex.Replace(ContractCode, "[^0-9]", ""));
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document aDoc = null;
            object fileName = Path.Combine(Class.GlobalVariables.V_ExecutingFolder + "\\Documents\\Polisə çıxarış.docx"), 
                   saveAs;

            try
            {
                if (File.Exists((string)fileName))
                {
                    object readOnly = false;
                    object isVisible = false;
                    wordApp.Visible = false;

                    aDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                    saveAs = Path.Combine(Class.GlobalVariables.V_ExecutingFolder + "\\TEMP\\Documents\\" + code_number + "_Polisə çıxarış.docx");                    
                    aDoc.Activate();

                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$currentdate]", day + " " + month + " " + year);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$customername]", Customer);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$carnumber]", CarNumber);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$car]", Car);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$type]", Type);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$ban]", Ban);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$card]", Card);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$lastdate]", PeriodValidityDate.Text);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$dateandcontract]", DateAndContract);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$object]", PObject);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$objectcount]", ObjectCount);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$contractcode]", ContractCode);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$bantype]", BanType);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$year]", Year);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$color]", PColor);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$brand]", Brand);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$enginenumber]", EngineNumber);
                    Class.GlobalProcedures.FindAndReplace(wordApp, "[$chassisnumber]", ChassisNumber);
                    Class.GlobalVariables.WordDocumentUsed = true;    
                    aDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    aDoc.Close(ref missing, ref missing, ref missing);
                    if (File.Exists((string)saveAs))
                        Process.Start((string)saveAs);
                    this.Close();
                }
                else
                {
                    XtraMessageBox.Show("Polisə çıxarışın çap faylı yaradılmayıb.");
                    return;
                }
            }
            catch (Exception exx)
            {
                Class.GlobalProcedures.LogWrite(code_number + "_Polise çıxarış.docx faylı açıq olduğu üçün bu faylı yenidən yaratmaq olmaz.", null, Class.GlobalVariables.V_UserName, this.Name, this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, exx);                
            }
        }
    }
}