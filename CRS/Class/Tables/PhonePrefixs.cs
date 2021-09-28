using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.Class.Tables
{
    public class PhonePrefixs
    {
        public int ID { get; set; }
        public string PHONE_DESCRIPTION_ID { get; set; }
        public string PREFIX { get; set; }        
        public string NOTE { get; set; }        
        public DateTime ETL_DT_TM { get; set; }
    }
}
