using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class InsuranceRate
    {
        public int ID { get; set; }        
        public decimal Rate { get; set; }        
        public decimal Amount { get; set; }        
        public string Note { get; set; }        
        public int UsedUserID { get; set; }
    }
}
