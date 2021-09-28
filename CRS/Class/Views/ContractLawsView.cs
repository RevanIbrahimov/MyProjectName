using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Views
{
    public class ContractLawsView
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int ContractID { get; set; }
        public string DefandantName { get; set; }
        public int LawID { get; set; }
        public string LawName { get; set; }
        public string JudgeName { get; set; }
        public int LawStatusID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastDate { get; set; }
        public DateTime NextDate { get; set; }
        public string Note { get; set; }
        public string CreatedUserName { get; set; }
        public int IsActive { get; set; }
        public int UsedUserID { get; set; }
    }
}
