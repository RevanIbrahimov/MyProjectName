using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.Class.Tables
{
    public class Countries
    {
        public int ID { get; set; }       
        public string NAME { get; set; }
        public string NAME_EN { get; set; }
        public string NAME_RU { get; set; }
        public int CODE { get; set; }
        public int ORDER_ID { get; set; }       
        public string NOTE { get; set; }
        public int USED_USER_ID { get; set; }
        public DateTime ETL_DT_TM { get; set; }        
    }
}
