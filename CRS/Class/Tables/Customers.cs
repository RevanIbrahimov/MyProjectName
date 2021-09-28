﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.Class.Tables
{
    public class Customers
    {
        public int ID { get; set; }
        public string CODE { get; set; }
        public int CODE_VALUE { get; set; }
        public string SURNAME { get; set; }
        public string NAME { get; set; }
        public string PATRONYMIC{ get; set; }
        public int SEX_ID { get; set; }
        public int BIRTHPLACE_ID { get; set; }
        public DateTime BIRTHDAY { get; set; }
        public string NOTE { get; set; }
        public int USED_USER_ID { get; set; }
    }
}
