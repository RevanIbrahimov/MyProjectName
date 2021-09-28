using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class Branches
    {
        public int ID { get; set; }
        public int BANK_ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string NOTE { get; set; }
        public int STATUS_ID { get; set; }        
    }
}
