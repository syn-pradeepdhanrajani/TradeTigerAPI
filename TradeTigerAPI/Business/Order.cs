using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTigerAPI.Business
{
    public class Order
    {
        public string OrderFeedData { get; set; }
        public string RequestID { get; set; }
        public string ExchangeCode { get; set; }
        public string Count { get; set; }
        public string SharekhanOrderID { get; set; }

        public string ExchangeOrderId { get; set; }

    }
}
