using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class CardSeries
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string SERIES { get; set; }   
        public string NOTE { get; set; }
        public int ORDER_ID { get; set; }
        public int USED_USER_ID { get; set; }
        public DateTime ETL_DT_TM { get; set; }
    }
}
