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

namespace CRS.Forms.Total
{
    public partial class FContractImages : DevExpress.XtraEditors.XtraForm
    {
        public FContractImages()
        {
            InitializeComponent();
        }
        public string ContractCode;
        public int ContractID;

        string ContractImagePath = GlobalVariables.V_ExecutingFolder + "\\TEMP\\ContractImages";

        private void FContractImages_Load(object sender, EventArgs e)
        {
            this.Name = ContractCode + " saylı lizinq müqaviləsinə aid olan şəkillər.";
            LoadContractImages();
        }

        private void LoadContractImages()
        {
            DataTable dt = GlobalFunctions.GenerateDataTable($@"SELECT T.IMAGE FROM CRS_USER.CONTRACT_IMAGES T WHERE T.CONTRACT_ID = {ContractID}", this.Name + "/LoadContractImages");            

            foreach (DataRow dr in dt.Rows)
            {
                if (!DBNull.Value.Equals(dr["IMAGE"]))
                {
                    Byte[] BLOBData = (byte[])dr["IMAGE"];
                    MemoryStream stmBLOBData = new MemoryStream(BLOBData);
                    ContractImageSlider.Images.Add(Image.FromStream(stmBLOBData));

                    if (!Directory.Exists(ContractImagePath))
                    {
                        Directory.CreateDirectory(ContractImagePath);
                    }
                    GlobalProcedures.DeleteFile(ContractImagePath + "\\" + ContractImageSlider.Images.Count + "_" + ContractCode + ".jpeg");
                    FileStream fs = new FileStream(ContractImagePath + "\\" + ContractImageSlider.Images.Count + "_" + ContractCode + ".jpeg", FileMode.Create, FileAccess.Write);
                    
                    stmBLOBData.WriteTo(fs);
                    fs.Close();
                    stmBLOBData.Close();
                }
            }
        }

        private void FContractImages_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalProcedures.DeleteAllFilesInDirectory(ContractImagePath);
        }
    }
}