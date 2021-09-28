using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.Class.Tables
{
    public class Contract
    {
        public int ID { get; set; }
        public string CODE { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int SELLER_ID { get; set; }
        public int CREDIT_TYPE_ID { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public int GRACE_PERIOD { get; set; }
        public decimal AMOUNT { get; set; }
        public int CURRENCY_ID { get; set; }
        public string CUSTOMER_ACCOUNT { get; set; }
        public string LEASING_ACCOUNT { get; set; }
        public string LEASING_INTEREST_ACCOUNT { get; set; }
        public int IS_COMMIT { get; set; }
        public int COMMITED_USER_ID { get; set; }
        public int USED_USER_ID { get; set; }
        public int CHECK_END_DATE { get; set; }
        public int CUSTOMER_CARDS_ID { get; set; }
        public decimal MONTHLY_AMOUNT { get; set; }
        public int CHECK_PERIOD { get; set; }
        public int PAYMENT_TYPE { get; set; }
        public int STATUS_ID { get; set; }
        public int COUNT { get; set; }
        public string NOTE { get; set; }
        public string NOTE_CHANGE_USER { get; set; }
        public DateTime NOTE_CHANGE_DATE { get; set; }
        public int CHECK_INTEREST { get; set; }
        public DateTime ETL_DT_TM { get; set; }
        public string SELLER_ACCOUNT { get; set; }
        public string BANK_CASH { get; set; }
        public int  IS_EXPENSES { get; set; }
        public int LIQUID_TYPE { get; set; }
        public decimal CURRENCY_RATE { get; set; }
        public int SELLER_TYPE_ID { get; set; }
        public int CUSTOMER_TYPE_ID { get; set; }
        public int? PARENT_ID { get; set; }
        public int INTEREST { get; set; }
        public int PERIOD { get; set; }
        public int IS_SPECIAL_ATTENTION { get; set; }
        public int CONTRACT_IMAGE_COUNT { get; set; }
    }
}
