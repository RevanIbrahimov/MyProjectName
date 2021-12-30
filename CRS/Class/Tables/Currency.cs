using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class Currency
    {
        public int ID { get; set; }
        public string CODE { get; set; }
        public int VALUE { get; set; }
        public string NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public string SMALL_NAME { get; set; }
        public string NOTE { get; set; }

        public string ALPHA3CODE { get; set; }
    }
}
