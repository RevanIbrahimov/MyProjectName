using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class Penalty
    {
        public int ID { get; set; }
        public int CONTRACT_ID { get; set; }
        public DateTime CALC_DATE { get; set; }
        public decimal INTEREST { get; set; }
        public int IS_DEFAULT { get; set; }
        public string NOTE { get; set; }
    }
}
