using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class InsuranceTransfer
    {
        public int insuranceID { set; get; }
        public decimal amount { set; get; }
        public decimal compensation { set; get; }
        public string note { set; get; }
    }
}
