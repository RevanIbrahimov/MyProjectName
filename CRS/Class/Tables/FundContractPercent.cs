using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class FundContractPercent
    {
        public int ID { get; set; }
        public int FUNDS_CONTRACTS_ID { get; set; }        
        public DateTime PDATE { get; set; }        
        public Decimal PERCENT_VALUE { get; set; }        
        public string NOTE { get; set; }        
    }
}
