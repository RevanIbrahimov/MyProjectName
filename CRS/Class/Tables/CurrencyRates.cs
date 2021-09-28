using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class CurrencyRates
    {        
        public int CURRENCY_ID { get; set; }
        public string CURRENCY_CODE { get; set; }
        public DateTime RATE_DATE { get; set; }        
        public decimal AMOUNT { get; set; }
        public int VALUE { get; set; }
    }
}
