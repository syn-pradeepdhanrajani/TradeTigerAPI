using LoadScripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadScripts.Models
{
    public class ScriptPriceModel
    {
        public int? ScriptId { get; set; } 
        public string ScriptCode { get; set; } 

        public DateTime TradeDate { get; set; }
        public double? OpenPrice { get; set; }
        public double? HighPrice { get; set; }
        public double? LowPrice { get; set; }
        public double? ClosePrice { get; set; }
        public decimal? Quantity { get; set; }
        public double? Average { get; set; }

        public LinkedList<ScriptPriceModel> PriceModelList { get; set; }

        public Trend? ScriptTrend { get; set; }
        public DirectionSignal? DirectionChangeSignal { get; set; }

    }
}
