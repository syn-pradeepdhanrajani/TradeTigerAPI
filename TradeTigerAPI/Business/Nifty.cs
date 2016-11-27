using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTigerAPI.Business
{
    public class Nifty : ICloneable
    {
        #region Stock Properties

        public int StockId { get; set; } // StockId
        public string Exchange { get; set; } // Exchange (length: 255)
        public string ScripName { get; set; } // Scrip Name (length: 255)
        public string C37Change { get; set; } // % Change (length: 255)
        public string Current { get; set; } // Current (length: 255)
        public string LastTradedQty { get; set; } // Last Traded Qty (length: 255)
        public string BidQty { get; set; } // Bid Qty (length: 255)
        public string BidPrice { get; set; } // Bid Price (length: 255)
        public string OfferPrice { get; set; } // Offer Price (length: 255)
        public string OfferQty { get; set; } // Offer Qty (length: 255)
        public string Open { get; set; } // Open (length: 255)
        public string High { get; set; } // High (length: 255)
        public string Low { get; set; } // Low (length: 255)
        public string Close { get; set; } // Close (length: 255)
        public string LastUpdatedTime { get; set; } // Last Updated Time (length: 255)
        public string LastTradedTime { get; set; } // Last Traded Time (length: 255)
        public string LastTradedDate { get; set; } // Last Traded Date (length: 255)
        public string Qty { get; set; } // Qty (length: 255)
        public string TotalBuyQty { get; set; } // Total Buy Qty (length: 255)
        public string ScripCode { get; set; } // Scrip Code (length: 255)
        public string TotalSellQty { get; set; } // Total Sell Qty (length: 255)
        public string OiDifference { get; set; } // OI Difference (length: 255)
        public string OiDifferencePercentage { get; set; } // OI Difference Percentage (length: 255)
        public string CompanyName { get; set; } // Company Name (length: 255)
        public string P35Open { get; set; } // P#Open (length: 255)
        public string P35High { get; set; } // P#High (length: 255)
        public string P35Low { get; set; } // P#Low (length: 255)
        public string P35Close { get; set; } // P#Close (length: 255)
        public string P35Quantity { get; set; } // P#Quantity (length: 255)
        public string PivotRes3 { get; set; } // Pivot Res 3 (length: 255)
        public string PivotRes2 { get; set; } // Pivot Res 2 (length: 255)
        public string PivotRes1 { get; set; } // Pivot Res 1 (length: 255)
        public string Pivot { get; set; } // Pivot (length: 255)
        public string PivotSup1 { get; set; } // Pivot Sup 1 (length: 255)
        public string PivotSup2 { get; set; } // Pivot Sup 2 (length: 255)
        public string PivotSup3 { get; set; } // Pivot Sup 3 (length: 255)
        public System.DateTime CreateDate { get; set; } // CreateDate

        #endregion

        #region Additional Properties based on the calculations & feeds
        private bool isInLowPriceRadar;
        public bool IsInLowPriceRadar
        {
            get
            {
                return this.isInLowPriceRadar;
            }
            set
            {
                this.isInLowPriceRadar = value;
            }
        }

        private bool isInHighPriceRadar;
        public bool IsInHighPriceRadar
        {
            get
            {
                return this.isInHighPriceRadar;
            }
            set
            {
                this.isInHighPriceRadar = value;
            }
        }

        private bool isScriptShortCandidate = false;
        public bool IsScriptShortCandidate
        {
            get
            {
                return this.isScriptShortCandidate;
            }
            set
            {
                this.isScriptShortCandidate = value;
            }
        }

        private bool isScriptBuyCandidate = false;
        public bool IsScriptBuyCandidate
        {
            get
            {
                return this.isScriptBuyCandidate;
            }
            set
            {
                this.isScriptBuyCandidate = value;
            }
        }

        private bool previousLowCrossed = false;
        public bool PreviousLowCrossed
        {
            get
            {
                return this.previousLowCrossed;
            }
            set
            {
                this.previousLowCrossed = value;
            }
        }

        private bool previousHighCrossed = false;
        public bool PreviousHighCrossed
        {
            get
            {
                return this.previousHighCrossed;
            }
            set
            {
                this.previousHighCrossed = value;
            }
        }


        /// <summary>
        /// //Any trade done for the day for this stock, only trade allowed for the day for a particular stock.
        /// </summary>
        public bool IsOrderPlaced { get; set; }
        /// <summary>
        /// Last traded price from the feed data
        /// </summary>
        public double LastTradedPriceFromFeed { get; set; }
        /// <summary>
        /// Last traded time from the feed data
        /// </summary>
        public string LastTradedTimeFromFeed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LastTradedVolumeFromFeed { get; set; }
        /// <summary>
        /// Log the condition for the trade taken
        /// </summary>
        public string TradedCondition { get; set; }
        /// <summary>
        /// Feed Data String
        /// </summary>
        public string FeedData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BuyRadarCounter { get; set; }

        public int ShortRadarCounter { get; set; }

        #endregion

        public Nifty()
        {
            CreateDate = System.DateTime.Now;
            BuyRadarCounter = 0;
            ShortRadarCounter = 0;
        }

        public object Clone()
        {
            return new Nifty()
            {
                StockId = this.StockId, // StockId
                Exchange = this.Exchange,// Exchange (length: 255)
                ScripName = this.ScripName,// Scrip Name (length: 255)
                C37Change = this.C37Change,// % Change (length: 255)
                Current = this.Current,// Current (length: 255)
                LastTradedQty = this.LastTradedQty, // Last Traded Qty (length: 255)
                BidQty = this.BidQty, // Bid Qty (length: 255)
                BidPrice = this.BidPrice,  // Bid Price (length: 255)
                OfferPrice = this.OfferPrice, // Offer Price (length: 255)
                OfferQty = this.OfferQty,// Offer Qty (length: 255)
                Open = this.Open,// Open (length: 255)
                High = this.High,// High (length: 255)
                Low = this.Low, // Low (length: 255)
                Close = this.Close, // Close (length: 255)
                LastUpdatedTime = this.LastUpdatedTime,// Last Updated Time (length: 255)
                LastTradedTime = this.LastTradedTime,// Last Traded Time (length: 255)
                LastTradedDate = this.LastTradedDate,// Last Traded Date (length: 255)
                Qty = this.Qty,// Qty (length: 255)
                TotalBuyQty = this.TotalBuyQty, // Total Buy Qty (length: 255)
                ScripCode = this.ScripCode, // Scrip Code (length: 255)
                TotalSellQty = this.TotalSellQty, // Total Sell Qty (length: 255)
                OiDifference = this.OiDifference, // OI Difference (length: 255)
                OiDifferencePercentage = this.OiDifferencePercentage,// OI Difference Percentage (length: 255)
                CompanyName = this.CompanyName,// Company Name (length: 255)
                P35Open = this.P35Open,// P#Open (length: 255)
                P35High = this.P35High,// P#High (length: 255)
                P35Low = this.P35Low,// P#Low (length: 255)
                P35Close = this.P35Close,// P#Close (length: 255)
                P35Quantity = this.P35Quantity,// P#Quantity (length: 255)
                PivotRes3 = this.PivotRes3,// Pivot Res 3 (length: 255)
                PivotRes2 = this.PivotRes2,// Pivot Res 2 (length: 255)
                PivotRes1 = this.PivotRes1,// Pivot Res 1 (length: 255)
                Pivot = this.Pivot,// Pivot (length: 255)
                PivotSup1 = this.PivotSup1, // Pivot Sup 1 (length: 255)
                PivotSup2 = this.PivotSup2, // Pivot Sup 2 (length: 255)
                PivotSup3 = this.PivotSup3,// Pivot Sup 3 (length: 255)
                CreateDate = this.CreateDate,// CreateDate
                IsOrderPlaced = this.IsOrderPlaced,
                LastTradedPriceFromFeed = this.LastTradedPriceFromFeed,
                LastTradedTimeFromFeed = this.LastTradedTimeFromFeed,
                TradedCondition = this.TradedCondition,
                FeedData = this.FeedData
            };
        }

    }
}
