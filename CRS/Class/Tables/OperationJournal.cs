using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.Class.Tables
{
    public class OperationJournal
    {
        public int ID { get; set; }
        public DateTime OPERATION_DATE { get; set; }
        public int YR_MNTH_DY { get; set; }
        public int CREATED_USER_ID { get; set; }
        public string DEBIT_ACCOUNT { get; set; }
        public string CREDIT_ACCOUNT { get; set; }
        public double CURRENCY_RATE { get; set; }
        public decimal AMOUNT_CUR { get; set; }
        public decimal AMOUNT_AZN { get; set; }
        public string APPOINTMENT { get; set; }
        public int CREATECONTRACT_IDD_USER_ID { get; set; }
        public int CUSTOMER_PAYMENT_ID { get; set; }
        public int ACCOUNT_OPERATION_TYPE_ID { get; set; }
        public int IS_MANUAL { get; set; }        
        public int USED_USER_ID { get; set; }
        public DateTime ETL_DT_TM { get; set; }
    }
}
