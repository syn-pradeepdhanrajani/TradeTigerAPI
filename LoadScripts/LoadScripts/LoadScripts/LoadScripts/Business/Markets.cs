using AutoMapper;
using LoadScripts.Common;
using LoadScripts.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadScripts.Business
{
    public class Markets
    {
        private MyDbContext context = new MyDbContext();
        private List<MarketData> scriptTempList;
        private List<ScriptPriceView> scriptPriceView;
        private List<Script> scriptList;
        MapperConfiguration mapperConfig;

        public Markets()
        {
            scriptPriceView = new List<ScriptPriceView>();
            scriptList = new List<Script>();

            //mapperConfig = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<ScriptPriceView, ScriptPriceModel>()
            //    .ForMember(dst => dst.ScriptId, opt => opt.MapFrom(src => src.ScriptId))
            //    //.ForMember(dst => dst.ScriptCode, opt => opt.MapFrom(src => src.ScriptCode))
            //    .ForMember(dst => dst.HighPrice, opt => opt.MapFrom(src => src.DayHigh))
            //    .ForMember(dst => dst.LowPrice, opt => opt.MapFrom(src => src.DayLow))
            //    .ForMember(dst => dst.ClosePrice, opt => opt.MapFrom(src => src.ClosingPrice))
            //    .ForMember(dst => dst.OpenPrice, opt => opt.MapFrom(src => src.DayOpen))
            //    .ForMember(dst => dst.Quantity, opt => opt.MapFrom(src => src.DayVolume))
            //    .ForMember(dst => dst.TradeDate, opt => opt.MapFrom(src => src.TradeDate))
            //    .ForMember(dst => dst.Average, opt => opt.Ignore())
            //    .ForMember(dst => dst.ScriptTrend, opt => opt.Ignore())
            //    .ForMember(dst => dst.PriceModelList, opt => opt.Ignore())
            //    .ForMember(dst=> dst.DirectionChangeSignal, opt => opt.Ignore());
            //});

            //mapperConfig.AssertConfigurationIsValid();
        }

        public void LoadScripts()
        {
            scriptTempList = context.MarketDatas.ToList();
            int exchangeCode = 0;
            foreach (MarketData scriptItem in scriptTempList)
            {
                if (scriptItem.Exchange.Equals("NSE")) exchangeCode = (int)ExchangeCode.NC;
                if (scriptItem.Exchange.Equals("NSEFO")) exchangeCode = (int)ExchangeCode.NF;

                if (!context.Scripts.Any(s => s.ScriptCode.Trim().Equals(scriptItem.ScripCode.Trim()) && s.ScriptMarketExchangeId == exchangeCode))
                {
                    Script newScript = new Script()
                    {
                        ScriptCode = scriptItem.ScripCode,
                        ScriptName = scriptItem.ScripName,
                        CompanyName = scriptItem.CompanyName,
                        ScriptMarketExchangeId = exchangeCode
                    };

                    context.Scripts.Add(newScript);
                    context.SaveChanges();
                    
                }
                else {
                    //Customer c = (from x in dataBase.Customers
                    //              where x.Name == "Test"
                    //              select x).First();
                    //c.Name = "New Name";
                    //dataBase.SaveChanges();

                }
            }
        }

        public void ApplyJesseTradingKey(Script scriptItem)
        {
            //Last recorded price in trading system
            var tradingMasterKey = (from j in context.JesseTradingMasterKeys
                              orderby j.TradeDate descending
                              where j.ScriptId == scriptItem.ScriptId
                                select j).FirstOrDefault();

            //get all active pivots 
            var tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                              where j.ScriptId == scriptItem.ScriptId &&
                                    j.IsPivot == true
                              select j).ToList();

            //Last trade recorded in trading master key
            var lastTradeDate = tradingMasterKey.TradeDate;

            if (tradingMasterKey != null)
            {
                //Get all the prices after last recorded price in trading system
                var scriptPrices = (from s in context.ScriptPrices
                                    orderby s.TradeDate
                                    where s.ScriptId == scriptItem.ScriptId &&
                                           s.TradeDate > tradingMasterKey.TradeDate
                                    select s).ToList();
                int pos = 0;
                foreach (ScriptPrice scriptPriceItem in scriptPrices)
                {
                    pos++;
                    if (tradingMasterKey.SecondaryRallyPrice != null || tradingMasterKey.NaturalRallyPrice != null || tradingMasterKey.UptrendPrice != null)
                    {
                        //Set script price from downtrending columns
                        double? tradingSystemtPrice = 0.0;
                        if (tradingMasterKey.UptrendPrice != null)
                            tradingSystemtPrice = tradingMasterKey.UptrendPrice;
                        if (tradingMasterKey.NaturalRallyPrice != null)
                            tradingSystemtPrice = tradingMasterKey.NaturalRallyPrice;
                        if (tradingMasterKey.SecondaryRallyPrice != null)
                            tradingSystemtPrice = tradingMasterKey.SecondaryRallyPrice;

                        //check if price is higher then last recorded price on downside columns...then ignore further recording
                        if (tradingMasterKey.UptrendPrice != null && scriptPriceItem.ClosingPrice > tradingMasterKey.UptrendPrice)
                        {   
                            tradingSystemtPrice = tradingMasterKey.UptrendPrice;
                            //Record this entry to trading master key
                            
                            //Create entry to trading Master Key
                            JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                            {
                                ScriptId = scriptItem.ScriptId,
                                UptrendPrice = scriptPriceItem.ClosingPrice,
                                TradeDate = scriptPriceItem.TradeDate
                            };

                            context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                            context.SaveChanges();


                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below
                            var naturalReactionPivotEntry = tradingKeyPivot.Where(p => p.NaturalReactionPrice != null || p.DowntrendPrice != null).FirstOrDefault();
                            if (naturalReactionPivotEntry != null)
                            {
                                //Update pivot entry
                                var naturalReactionPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.NaturalReactionPrice != null);
                                naturalReactionPivot.IsPivot = false;
                                context.SaveChanges();

                                //get all active pivots 
                                tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                   where j.ScriptId == scriptItem.ScriptId &&
                                                         j.IsPivot == true
                                                   select j).ToList();
                            }
                            

                            //Refetch all of the below 
                            tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                orderby j.TradeDate descending
                                                where j.ScriptId == scriptItem.ScriptId
                                                select j).FirstOrDefault();

                        }
                        else if (tradingMasterKey.NaturalRallyPrice != null && scriptPriceItem.ClosingPrice > tradingMasterKey.NaturalRallyPrice)
                        {
                            tradingSystemtPrice = tradingMasterKey.NaturalRallyPrice;
                            //Record this entry to trading master key 

                            //Check if there is natural entry pivot and if it crosses 5% record entry to uptrend
                            var naturalRallyPivotEntry = tradingKeyPivot.Where(p => p.NaturalRallyPrice != null && p.NaturalRallyPrice> 0).FirstOrDefault();
                            if (naturalRallyPivotEntry != null && (scriptPriceItem.ClosingPrice > (naturalRallyPivotEntry.NaturalRallyPrice + (naturalRallyPivotEntry.NaturalRallyPrice * 0.05))))
                            {
                                //Create entry to trading Master Key
                                JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                {
                                    ScriptId = scriptItem.ScriptId,
                                    UptrendPrice = scriptPriceItem.ClosingPrice,
                                    TradeDate = scriptPriceItem.TradeDate
                                };

                                context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                context.SaveChanges();
                                
                                //Update pivot entry
                                var naturalRallyPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.NaturalRallyPrice != null);
                                naturalRallyPivot.IsPivot = false;
                                context.SaveChanges();

                                //get all active pivots 
                                tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                   where j.ScriptId == scriptItem.ScriptId &&
                                                         j.IsPivot == true
                                                   select j).ToList();


                            }
                            else //Continue adding to natural rally
                            {

                                //even if there is no pivot in natural rally column... check if there is pivot in uptrend.. add entry to uptrend and remove pivot
                                var uptrendPivotEntry = tradingKeyPivot.Where(p => p.UptrendPrice != null && p.UptrendPrice > 0).FirstOrDefault();
                                if (uptrendPivotEntry != null && (scriptPriceItem.ClosingPrice > naturalRallyPivotEntry.UptrendPrice))
                                {
                                    //Create entry to trading Master Key
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        UptrendPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();

                                    //remove uptrend pivot
                                    var uptrendPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.UptrendPrice != null);
                                    uptrendPivot.IsPivot = false;
                                    context.SaveChanges();

                                    //get all active pivots 
                                    tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                       where j.ScriptId == scriptItem.ScriptId &&
                                                             j.IsPivot == true
                                                       select j).ToList();
                                }
                                else
                                {
                                    //Create entry to trading Master Key
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        NaturalRallyPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();
                                }
                            }

                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below
                            var naturalReactionPivotEntry = tradingKeyPivot.Where(p => p.NaturalReactionPrice != null && p.NaturalReactionPrice > 0).FirstOrDefault();
                            if (naturalReactionPivotEntry != null && (scriptPriceItem.ClosingPrice > (naturalReactionPivotEntry.NaturalReactionPrice + (naturalReactionPivotEntry.NaturalReactionPrice * 0.2))))
                            {
                                //Update pivot entry
                                var naturalReactionPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.NaturalReactionPrice != null);
                                naturalReactionPivot.IsPivot = false;
                                context.SaveChanges();

                                //get all active pivots 
                                tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                   where j.ScriptId == scriptItem.ScriptId &&
                                                         j.IsPivot == true
                                                   select j).ToList();
                            }
                            else //Check if there is downtrend pivot 
                            {
                                var downtrendPivotEntry = tradingKeyPivot.Where(p => p.DowntrendPrice != null && p.DowntrendPrice > 0).FirstOrDefault();
                                if (downtrendPivotEntry != null && (scriptPriceItem.ClosingPrice > (downtrendPivotEntry.DowntrendPrice + (downtrendPivotEntry.DowntrendPrice * 0.2))))
                                {
                                    //Update pivot entry
                                    var downtrendPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.DowntrendPrice != null);
                                    downtrendPivot.IsPivot = false;
                                    context.SaveChanges();

                                    //get all active pivots 
                                    tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                       where j.ScriptId == scriptItem.ScriptId &&
                                                             j.IsPivot == true
                                                       select j).ToList();
                                }
                            }

                            //Refetch all of the below 
                            tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                orderby j.TradeDate descending
                                                where j.ScriptId == scriptItem.ScriptId
                                                select j).FirstOrDefault();

                        }
                        else if (tradingMasterKey.SecondaryRallyPrice != null && scriptPriceItem.ClosingPrice > tradingMasterKey.SecondaryRallyPrice)
                        {
                            JesseTradingMasterKeyPivot naturalRallyPivotEntry = null;
                            if (tradingKeyPivot != null)
                            {
                                naturalRallyPivotEntry = tradingKeyPivot.Where(p => p.NaturalRallyPrice != null && p.NaturalRallyPrice > 0).FirstOrDefault();
                                //Reversing trend...check if there is any pivot in natural rally or uptrend...
                                if (naturalRallyPivotEntry != null && naturalRallyPivotEntry.NaturalRallyPrice > 0 && scriptPriceItem.ClosingPrice > naturalRallyPivotEntry.NaturalRallyPrice)
                                {
                                    tradingSystemtPrice = tradingMasterKey.SecondaryRallyPrice;
                                    //Record this entry to trading master key 
                                    //make an entry to secondary rally column as natural rally pivot exists..
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        NaturalRallyPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();
                                }
                                else {
                                    tradingSystemtPrice = tradingMasterKey.SecondaryRallyPrice;
                                    //Record this entry to trading master key 
                                    //make an entry to secondary rally column as natural rally pivot exists..
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        SecondaryRallyPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();
                                }
                            }

                            tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                orderby j.TradeDate descending
                                                where j.ScriptId == scriptItem.ScriptId
                                                select j).FirstOrDefault();

                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below
                        }
                        //check if the price trend has reversed..reversed more than 10%
                        else if (scriptPriceItem.ClosingPrice < (tradingSystemtPrice - (tradingSystemtPrice * 0.1))) 
                        {
                            //Create pivot in trading system, before that check if there is active pivot
                            JesseTradingMasterKeyPivot naturalReactionPivotEntry = null;
                            if (tradingKeyPivot != null)
                            {
                                naturalReactionPivotEntry = tradingKeyPivot.Where(p => p.NaturalReactionPrice != null && p.NaturalReactionPrice > 0).FirstOrDefault();
                                //Reversing trend...check if there is any pivot in natural rally or uptrend...
                                if (naturalReactionPivotEntry == null) //&& naturalRallyPivotEntry.NaturalRallyPrice > 0 && naturalRallyPivotEntry.NaturalRallyPrice < 0
                                {

                                    if (tradingMasterKey.NaturalRallyPrice != null)
                                    {
                                        //check if there is already any pivot before adding new...this is only true if pivot was removed by 20% and 5% price change
                                        //this is only for natural rally or natural reaction 
                                        var naturalRallyPivot = tradingKeyPivot.Where(p => p.NaturalRallyPrice != null && p.NaturalRallyPrice > 0).FirstOrDefault();
                                        if (naturalRallyPivot == null)
                                        {
                                            //Create pivot entry
                                            JesseTradingMasterKeyPivot newPivot = new JesseTradingMasterKeyPivot()
                                            {
                                                ScriptId = scriptItem.ScriptId,
                                                IsPivot = true,
                                                NaturalRallyPrice = tradingMasterKey.NaturalRallyPrice,
                                            };

                                            context.JesseTradingMasterKeyPivots.Add(newPivot);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (tradingMasterKey.UptrendPrice != null)
                                    {
                                        //Create pivot entry
                                        JesseTradingMasterKeyPivot newPivot = new JesseTradingMasterKeyPivot()
                                        {
                                            ScriptId = scriptItem.ScriptId,
                                            IsPivot = true,
                                            UptrendPrice = tradingMasterKey.UptrendPrice,
                                        };

                                        context.JesseTradingMasterKeyPivots.Add(newPivot);
                                        context.SaveChanges();

                                    }
                                  

                                    //Create entry to trading Master Key
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        NaturalReactionPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();


                                }
                                else // There is pivot entry in natural reaction entry ... then enter it in secondary reaction 
                                {
                                    //check the closing price below natural reaction pivot, then enter in natural reaction or else in secondary reaction
                                    if (scriptPriceItem.ClosingPrice < naturalReactionPivotEntry.NaturalReactionPrice)
                                    {
                                        //Create entry to trading Master Key
                                        JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                        {
                                            ScriptId = scriptItem.ScriptId,
                                            NaturalReactionPrice = scriptPriceItem.ClosingPrice,
                                            TradeDate = scriptPriceItem.TradeDate
                                        };

                                        context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        //Create entry to trading Master Key
                                        JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                        {
                                            ScriptId = scriptItem.ScriptId,
                                            SecondaryReactionPrice = scriptPriceItem.ClosingPrice,
                                            TradeDate = scriptPriceItem.TradeDate
                                        };

                                        context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                        context.SaveChanges();
                                    }
                                }

                                //Refetch all of the below 
                                scriptPrices = (from s in context.ScriptPrices
                                                orderby s.TradeDate
                                                where s.ScriptId == scriptItem.ScriptId &&
                                                       s.TradeDate > scriptPriceItem.TradeDate
                                                select s).ToList();

                                tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                    orderby j.TradeDate descending
                                                    where j.ScriptId == scriptItem.ScriptId
                                                    select j).FirstOrDefault();

                                //get all active pivots 
                                tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                   where j.ScriptId == scriptItem.ScriptId &&
                                                         j.IsPivot == true
                                                   select j).ToList();
                            }

                        }

                    }
                    else if (tradingMasterKey.SecondaryReactionPrice != null || tradingMasterKey.NaturalReactionPrice != null || tradingMasterKey.DowntrendPrice != null)
                    {
                        //Set script price from downtrending columns
                        double? tradingSystemtPrice = 0.0;
                        if (tradingMasterKey.DowntrendPrice != null)
                            tradingSystemtPrice = tradingMasterKey.DowntrendPrice;
                        if (tradingMasterKey.NaturalReactionPrice != null)
                            tradingSystemtPrice = tradingMasterKey.NaturalReactionPrice;
                        if (tradingMasterKey.SecondaryReactionPrice != null)
                            tradingSystemtPrice = tradingMasterKey.SecondaryReactionPrice;

                        //check if price is higher then last recorded price on downside columns...then ignore further recording
                        if (tradingMasterKey.DowntrendPrice != null && scriptPriceItem.ClosingPrice < tradingMasterKey.DowntrendPrice)
                        {
                            tradingSystemtPrice = tradingMasterKey.DowntrendPrice;
                            //Record this entry to trading master key

                            //Create entry to trading Master Key
                            JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                            {
                                ScriptId = scriptItem.ScriptId,
                                DowntrendPrice = scriptPriceItem.ClosingPrice,
                                TradeDate = scriptPriceItem.TradeDate
                            };

                            context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                            context.SaveChanges();

                            tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                orderby j.TradeDate descending
                                                where j.ScriptId == scriptItem.ScriptId
                                                select j).FirstOrDefault();

                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below
                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below
                            var naturalRallyPivotEntry = tradingKeyPivot.Where(p => p.NaturalRallyPrice != null || p.UptrendPrice != null).FirstOrDefault();
                            if (naturalRallyPivotEntry != null)
                            {
                                //Update pivot entry
                                var naturalReactionPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.NaturalRallyPrice != null);
                                naturalReactionPivot.IsPivot = false;
                                context.SaveChanges();

                                //get all active pivots 
                                tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                   where j.ScriptId == scriptItem.ScriptId &&
                                                         j.IsPivot == true
                                                   select j).ToList();
                            }
                        }
                        else if (tradingMasterKey.NaturalReactionPrice != null && scriptPriceItem.ClosingPrice < tradingMasterKey.NaturalReactionPrice)
                        {
                            tradingSystemtPrice = tradingMasterKey.NaturalReactionPrice;
                            //Record this entry to trading master key 
                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below

                            var naturalReactionPivotEntry = tradingKeyPivot.Where(p => p.NaturalReactionPrice != null && p.NaturalReactionPrice> 0).FirstOrDefault();
                            if (naturalReactionPivotEntry != null && (scriptPriceItem.ClosingPrice < (naturalReactionPivotEntry.NaturalReactionPrice - (naturalReactionPivotEntry.NaturalReactionPrice * 0.05))))
                            {
                                //Create entry to trading Master Key
                                JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                {
                                    ScriptId = scriptItem.ScriptId,
                                    DowntrendPrice = scriptPriceItem.ClosingPrice,
                                    TradeDate = scriptPriceItem.TradeDate
                                };

                                context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                context.SaveChanges();

                                //Update pivot entry
                                var naturalReactionPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.NaturalReactionPrice != null);
                                naturalReactionPivot.IsPivot = false;
                                context.SaveChanges();
                            }
                            else //Continue adding to natural reaction
                            {

                                //even if there is no pivot in natural rally column... check if there is pivot in uptrend.. add entry to uptrend and remove pivot
                                var downtrendPivotEntry = tradingKeyPivot.Where(p => p.DowntrendPrice!= null && p.DowntrendPrice> 0).FirstOrDefault();
                                if (downtrendPivotEntry != null && (scriptPriceItem.ClosingPrice < downtrendPivotEntry.DowntrendPrice))
                                {
                                    //Create entry to trading Master Key
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        DowntrendPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();

                                    //remove uptrend pivot
                                    var downtrendPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.DowntrendPrice!= null);
                                    downtrendPivot.IsPivot = false;
                                    context.SaveChanges();

                                    //get all active pivots 
                                    tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                       where j.ScriptId == scriptItem.ScriptId &&
                                                             j.IsPivot == true
                                                       select j).ToList();
                                }
                                else
                                {

                                    //Create entry to trading Master Key
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        NaturalReactionPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();
                                }
                            }

                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below
                            var naturalRallyPivotEntry = tradingKeyPivot.Where(p => p.NaturalRallyPrice != null && p.NaturalRallyPrice > 0).FirstOrDefault();
                            if (naturalRallyPivotEntry != null && (scriptPriceItem.ClosingPrice < (naturalRallyPivotEntry.NaturalRallyPrice - (naturalRallyPivotEntry.NaturalRallyPrice * 0.2))))
                            {
                                //Update pivot entry
                                var naturalRallyPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.NaturalRallyPrice != null);
                                naturalRallyPivot.IsPivot = false;
                                context.SaveChanges();

                                //get all active pivots 
                                tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                   where j.ScriptId == scriptItem.ScriptId &&
                                                         j.IsPivot == true
                                                   select j).ToList();
                            }
                            else //Check uptrend pivot
                            {
                                var uptrendPivotEntry = tradingKeyPivot.Where(p => p.UptrendPrice != null && p.UptrendPrice > 0).FirstOrDefault();
                                if (uptrendPivotEntry != null && (scriptPriceItem.ClosingPrice < (uptrendPivotEntry.UptrendPrice - (uptrendPivotEntry.UptrendPrice * 0.2))))
                                {
                                    //Update pivot entry
                                    var uptrendPivot = context.JesseTradingMasterKeyPivots.SingleOrDefault(s => s.ScriptId == scriptItem.ScriptId && s.IsPivot && s.UptrendPrice != null);
                                    uptrendPivot.IsPivot = false;
                                    context.SaveChanges();

                                    //get all active pivots 
                                    tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                       where j.ScriptId == scriptItem.ScriptId &&
                                                             j.IsPivot == true
                                                       select j).ToList();
                                }
                            }

                            tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                orderby j.TradeDate descending
                                                where j.ScriptId == scriptItem.ScriptId
                                                select j).FirstOrDefault();
                        }
                        else if (tradingMasterKey.SecondaryReactionPrice != null && scriptPriceItem.ClosingPrice < tradingMasterKey.SecondaryReactionPrice)
                        {
                            tradingSystemtPrice = tradingMasterKey.SecondaryReactionPrice;
                            //Record this entry to trading master key 
                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below

                            JesseTradingMasterKeyPivot naturalReactionPivotEntry = null;
                            if (tradingKeyPivot != null)
                            {
                                naturalReactionPivotEntry = tradingKeyPivot.Where(p => p.NaturalReactionPrice != null && p.NaturalReactionPrice> 0).FirstOrDefault();
                                //Reversing trend...check if there is any pivot in natural rally or uptrend...
                                if (naturalReactionPivotEntry != null && naturalReactionPivotEntry.NaturalReactionPrice > 0 && scriptPriceItem.ClosingPrice < naturalReactionPivotEntry.NaturalReactionPrice)
                                {
                                    tradingSystemtPrice = tradingMasterKey.SecondaryRallyPrice;
                                    //Record this entry to trading master key 
                                    //make an entry to secondary rally column as natural rally pivot exists..
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        NaturalReactionPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    tradingSystemtPrice = tradingMasterKey.SecondaryRallyPrice;
                                    //Record this entry to trading master key 
                                    //make an entry to secondary rally column as natural rally pivot exists..
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        SecondaryRallyPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();
                                }
                            }

                            tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                orderby j.TradeDate descending
                                                where j.ScriptId == scriptItem.ScriptId
                                                select j).FirstOrDefault();

                            //Check if there is pivot entry in opposite trend, remove the pivot if the price is 20% below

                        }
                        else if (scriptPriceItem.ClosingPrice > (tradingSystemtPrice + (tradingSystemtPrice * 0.1))) //check if the price trend has reversed..reversed more than 10%
                        {
                            //Create pivot in trading system, before that check if there is active pivot
                            JesseTradingMasterKeyPivot naturalRallyPivotEntry = null;
                            if (tradingKeyPivot != null)
                            {
                                naturalRallyPivotEntry = tradingKeyPivot.Where(p => p.NaturalRallyPrice != null && p.NaturalRallyPrice > 0).FirstOrDefault();
                                //Reversing trend...check if there is any pivot in natural rally or uptrend...
                                if (naturalRallyPivotEntry == null)
                                {

                                    if (tradingMasterKey.NaturalReactionPrice != null)
                                    {
                                        //check if there is already any pivot before adding new...this is only true if pivot was removed by 20% and 5% price change
                                        //this is only for natural rally or natural reaction 
                                        var naturalReactionPivot = tradingKeyPivot.Where(p => p.NaturalReactionPrice != null && p.NaturalReactionPrice > 0).FirstOrDefault();
                                        if (naturalReactionPivot == null)
                                        {
                                            //Create pivot entry
                                            JesseTradingMasterKeyPivot newPivot = new JesseTradingMasterKeyPivot()
                                            {
                                                ScriptId = scriptItem.ScriptId,
                                                IsPivot = true,
                                                NaturalReactionPrice = tradingMasterKey.NaturalReactionPrice,
                                            };

                                            context.JesseTradingMasterKeyPivots.Add(newPivot);
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (tradingMasterKey.DowntrendPrice != null)
                                    {
                                        //Create pivot entry
                                        JesseTradingMasterKeyPivot newPivot = new JesseTradingMasterKeyPivot()
                                        {
                                            ScriptId = scriptItem.ScriptId,
                                            IsPivot = true,
                                            DowntrendPrice = tradingMasterKey.DowntrendPrice,
                                        };

                                        context.JesseTradingMasterKeyPivots.Add(newPivot);
                                        context.SaveChanges();

                                    }

                                    //Create entry to trading Master Key
                                    JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                    {
                                        ScriptId = scriptItem.ScriptId,
                                        NaturalRallyPrice = scriptPriceItem.ClosingPrice,
                                        TradeDate = scriptPriceItem.TradeDate
                                    };

                                    context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                    context.SaveChanges();

                                    scriptPrices = (from s in context.ScriptPrices
                                                    orderby s.TradeDate
                                                    where s.ScriptId == scriptItem.ScriptId &&
                                                           s.TradeDate > scriptPriceItem.TradeDate
                                                    select s).ToList();

                                    tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                        orderby j.TradeDate descending
                                                        where j.ScriptId == scriptItem.ScriptId
                                                        select j).FirstOrDefault();

                                    //get all active pivots 
                                    tradingKeyPivot = (from j in context.JesseTradingMasterKeyPivots
                                                       where j.ScriptId == scriptItem.ScriptId &&
                                                             j.IsPivot == true
                                                       select j).ToList();
                                }
                                else //Natural pivot entry exists
                                {
                                    if (scriptPriceItem.ClosingPrice > naturalRallyPivotEntry.NaturalRallyPrice)
                                    {
                                        //Create entry to trading Master Key
                                        JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                        {
                                            ScriptId = scriptItem.ScriptId,
                                            NaturalRallyPrice = scriptPriceItem.ClosingPrice,
                                            TradeDate = scriptPriceItem.TradeDate
                                        };

                                        context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        //make an entry to secondary rally column as natural rally pivot exists..
                                        JesseTradingMasterKey newTradingMasterKey = new JesseTradingMasterKey()
                                        {
                                            ScriptId = scriptItem.ScriptId,
                                            SecondaryRallyPrice = scriptPriceItem.ClosingPrice,
                                            TradeDate = scriptPriceItem.TradeDate
                                        };

                                        context.JesseTradingMasterKeys.Add(newTradingMasterKey);
                                        context.SaveChanges();
                                    }

                                    tradingMasterKey = (from j in context.JesseTradingMasterKeys
                                                        orderby j.TradeDate descending
                                                        where j.ScriptId == scriptItem.ScriptId
                                                        select j).FirstOrDefault();
                                }

                            }

                        }
                    }
                }
            }

        }

        public void LoadScriptPricesFromExcel(Script scriptItem, ScriptPriceType scriptPriceType)
        {
            bool isDaily, isWeekly, isMonthly, isQuarterly;
            isDaily = isWeekly  = isMonthly = isQuarterly = false;
            var priceList = GetPricesFromExcel();
            //update the database table.
            if (priceList != null && priceList.Count > 0)
            {
                priceList = GetPricesByPriceType(scriptPriceType, priceList);

                if (scriptPriceType == ScriptPriceType.IsDailyPrice) isDaily = true;
                else if (scriptPriceType == ScriptPriceType.IsWeeklyPrice) isWeekly = true;
                else if (scriptPriceType == ScriptPriceType.IsMonthlyPrice) isMonthly = true;
                else if (scriptPriceType == ScriptPriceType.IsQuarterlyPrice) isQuarterly = true;

                    foreach (ScriptPriceModel scriptPriceItem in priceList)
                    {
                        if (!context.ScriptPrices.Any(s => s.ScriptId == scriptItem.ScriptId 
                                                            && s.TradeDate == scriptPriceItem.TradeDate
                                                            && ((isDaily &&  s.IsDailyPrice) ||
                                                                (isWeekly &&  s.IsWeeklyPrice) ||
                                                                (isMonthly &&  s.IsMonthlyPrice) ||
                                                                (isQuarterly &&  s.IsQuarterlyPrice))
                                                    ))
                        {
                            ScriptPrice newScriptPrice = new ScriptPrice()
                            {
                                ScriptId = scriptItem.ScriptId,
                                ClosingPrice = Convert.ToDouble(scriptPriceItem.ClosePrice),
                                DayOpen = Convert.ToDouble(scriptPriceItem.OpenPrice),
                                DayHigh = Convert.ToDouble(scriptPriceItem.HighPrice),
                                DayLow = Convert.ToDouble(scriptPriceItem.LowPrice),
                                TradeDate = scriptPriceItem.TradeDate,
                                DayVolume = Convert.ToDecimal(scriptPriceItem.Quantity),
                                IsDailyPrice = isDaily ? true : false,
                                IsMonthlyPrice = isMonthly ? true : false,
                                IsQuarterlyPrice = isQuarterly ? true : false,
                                IsWeeklyPrice = isWeekly ? true : false
                            };

                            context.ScriptPrices.Add(newScriptPrice);
                            context.SaveChanges();
                        }
                    }
            }
        }

        private static IList<ScriptPriceModel> GetPricesByPriceType(ScriptPriceType scriptPriceType, IList<ScriptPriceModel> priceList)
        {
            if (scriptPriceType == ScriptPriceType.IsDailyPrice)
                priceList = priceList.OrderByDescending(s => s.TradeDate).Take(210).ToList();
            else if (scriptPriceType == ScriptPriceType.IsWeeklyPrice)
                priceList = priceList.OrderByDescending(s => s.TradeDate).Take(30).ToList();
            else if (scriptPriceType == ScriptPriceType.IsMonthlyPrice)
                priceList = priceList.OrderByDescending(s => s.TradeDate).Take(7).ToList();
            else if (scriptPriceType == ScriptPriceType.IsQuarterlyPrice)
                priceList = priceList.OrderByDescending(s => s.TradeDate).Take(4).ToList();
            return priceList;
        }

        public IList<ScriptPriceModel> GetPricesFromExcel()
        {
            //Check file path
            string excelPath = ExcelReader.CheckPath(ConfigurationManager.AppSettings["ExcelFilePath"]);

            //Get product list from the spreadsheet
            IList<ScriptPriceModel> dataList = ExcelReader.GetDataToList(excelPath, GetPrices);

            //Return resulted product list as formated string. 
            return dataList;
        }

        //Function for mapping and entering data into Product object.
        private ScriptPriceModel GetPrices(IList<string> rowData, IList<string> columnNames)
        {
            var scriptPrice = new ScriptPriceModel()
            {
                TradeDate = ParseToDate(rowData[columnNames.IndexFor("lasttradeddate")]),
                OpenPrice = rowData[columnNames.IndexFor("open")].ToDoubleNullable(),
                HighPrice = rowData[columnNames.IndexFor("high")].ToDoubleNullable(),
                LowPrice = rowData[columnNames.IndexFor("low")].ToDoubleNullable(),
                ClosePrice = rowData[columnNames.IndexFor("close")].ToDoubleNullable(),
                Quantity = rowData[columnNames.IndexFor("qty")].ToDecimalNullable()
            };
            return scriptPrice;
        }

        private DateTime ParseToDate(string dateField)
        {
            //double d = double.Parse(dateField);
            DateTime conv = DateTime.Parse(dateField);
            return conv;
        }

        public void UpdatePrices()
        {
            //First get the list of scripts from master table 
            List<Script>  scriptList= context.Scripts.Where(s=>s.Active).ToList();

            //Loop through and check get the closing price for the day
            foreach (Script scriptItem in scriptList)
            {
                UpdatePriceFromMaketData(scriptItem);

            }
        }

        public Script UpdatePrices(string scriptName, string[] scriptPrice)
        {
            //Script
            //Get Day closing prices
            Script scriptItem = (from s in context.Scripts
                                 where s.ScriptName.Equals(scriptName, StringComparison.OrdinalIgnoreCase)
                                 select s).FirstOrDefault();

            //Add prices to ScriptPrice table
            if (scriptPrice != null && scriptPrice.Length > 0)
            {
                //string[] dateString = scriptPrice[0].Split('-'); // .LastTradedDate //format - mm/dd/yyyy
                DateTime lastTradedDate = DateTime.ParseExact(scriptPrice[0], "dd-MMM-yyyy", CultureInfo.InvariantCulture); //new DateTime(Convert.ToInt16(dateString[2]), Convert.ToInt16(dateString[0]), Convert.ToInt16(dateString[1]));

                if (!context.ScriptPrices.Any(s => s.ScriptId == scriptItem.ScriptId && s.TradeDate == lastTradedDate))
                {
                    ScriptPrice newScriptPrice = new ScriptPrice()
                    {
                        ScriptId = scriptItem.ScriptId,
                        ClosingPrice = Convert.ToDouble(scriptPrice[5]),
                        DayOpen = Convert.ToDouble(scriptPrice[2]),
                        DayHigh = Convert.ToDouble(scriptPrice[3]),
                        DayLow = Convert.ToDouble(scriptPrice[4]),
                        TradeDate = lastTradedDate,
                        DayVolume = Convert.ToDecimal(scriptPrice[6]),
                    };

                    context.ScriptPrices.Add(newScriptPrice);
                    context.SaveChanges();
                }
                
            }
            return scriptItem;
        }

        private void UpdatePriceFromMaketData(Script scriptItem)
        {
            string scriptItemCode = Convert.ToString(scriptItem.ScriptCode);

            //Get Day closing prices
            var scriptPriceItem = (from s in context.MarketDatas
                                   where s.ScripCode == scriptItemCode
                                   select s).FirstOrDefault();

            //Add prices to ScriptPrice table
            if (scriptPriceItem != null)
            {
                string[] dateString = scriptPriceItem.LastTradedDate.Split('/'); //format - mm/dd/yyyy
                DateTime lastTradedDate = new DateTime(Convert.ToInt16(dateString[2]), Convert.ToInt16(dateString[0]), Convert.ToInt16(dateString[1]));

                if (!context.ScriptPrices.Any(s => s.ScriptId == scriptItem.ScriptId && s.TradeDate == lastTradedDate))
                {
                    ScriptPrice newScriptPrice = new ScriptPrice()
                    {
                        ScriptId = scriptItem.ScriptId,
                        ClosingPrice = Convert.ToDouble(scriptPriceItem.Current),
                        DayOpen = Convert.ToDouble(scriptPriceItem.Open),
                        DayHigh = Convert.ToDouble(scriptPriceItem.High),
                        DayLow = Convert.ToDouble(scriptPriceItem.Low),
                        TradeDate = lastTradedDate,
                        DayVolume = Convert.ToDecimal(scriptPriceItem.Qty),
                        OpenInterestPercentage = !string.IsNullOrEmpty(scriptPriceItem.OiDifferencePercentage) ? Convert.ToDouble(scriptPriceItem.OiDifferencePercentage) : 0,
                        OpenInterestDifference = !string.IsNullOrEmpty(scriptPriceItem.OiDifference) ? Convert.ToDecimal(scriptPriceItem.OiDifference) : 0
                    };

                    context.ScriptPrices.Add(newScriptPrice);
                    context.SaveChanges();
                }
            }
        }

        public void ProcessData(ScriptPriceType scriptPriceType)
        {
            LoadScriptPrices(); 
            ProcessPrices();
        }

        public void LoadScriptPrices()
        {
            scriptList = context.Scripts.Where(s => s.Active).ToList();
        }

        private void ProcessPrices()
        {
            foreach (Script scriptItem in scriptList)
            {
                var scriptPriceItem = context.ScriptPriceViews.Where(s => s.ScriptId == scriptItem.ScriptId /*&& s.Active*/).ToList();
                ProcessPrices(scriptPriceItem);
            }
        }

        private void ProcessPrices(List<ScriptPriceView> scriptPriceViewItem)
        {
            var scriptPrices = scriptPriceViewItem.OrderByDescending(o => o.TradeDate).ToList();
            
            //first get the recent highs and lows of a script 
            //weekly data process with at least 30 weeks 
            //near weekly prices closing 
            CheckPriceTrend(scriptPrices);

        }

        public bool CheckPriceTrend(List<ScriptPriceView> scriptPriceViewItem)
        {
            double highPrice, lowPrice;
            highPrice = lowPrice = 0;
            ScriptPriceModel scriptPriceModel = null; //= new ScriptPriceModel();
            var mapper = mapperConfig.CreateMapper();
            LinkedList<ScriptPriceModel> priceModelList = new LinkedList<ScriptPriceModel>();

            foreach (ScriptPriceView priceItem in scriptPriceViewItem)
            {
                scriptPriceModel = mapper.Map<ScriptPriceModel>(priceItem);
                priceModelList.AddLast(scriptPriceModel);
                if (priceItem.DayHigh > highPrice) highPrice = priceItem.DayHigh;
                if (lowPrice == 0 || priceItem.DayLow < lowPrice) lowPrice = priceItem.DayLow;
                
            }

            if (scriptPriceModel != null)
            {
                scriptPriceModel.PriceModelList = priceModelList;

                if (scriptPriceModel.PriceModelList != null && scriptPriceModel.PriceModelList.Count > 0)
                    //Trend check, compare if previous price bars....
                    CheckTrend(scriptPriceModel, highPrice, lowPrice);

            }
            return true;
        }

        private bool CheckTrend(ScriptPriceModel priceModelList, double highPrice, double lowPrice)
        {
            var node = priceModelList.PriceModelList.First;
            ScriptPriceModel lastPriceModel = node.Value; //Last recorded price i.e. 
            while (node != null)
            {
                ScriptPriceModel scriptPriceModel = node.Value;

                priceModelList.DirectionChangeSignal = DirectionSignal.None;
                //the latest price high bar is new high and new low... no trend
                if (scriptPriceModel.HighPrice == highPrice && scriptPriceModel.LowPrice == lowPrice)
                { 
                    priceModelList.ScriptTrend = Trend.Mixed;
                    return true;
                }
                //if the current high price is the highest .... nothing can be done
                else if (scriptPriceModel.HighPrice == highPrice)
                { 
                    priceModelList.ScriptTrend = Trend.Uptrend;
                    return true;
                }
                else if (scriptPriceModel.LowPrice == lowPrice)
                {
                    priceModelList.ScriptTrend = Trend.Downtrend;
                    return true;
                }
                else {
                    //continue processing with the prices

                }

                node = node.Next;
            }

            return true;
        }

        public void ProcessMarketData()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Stocks].[dbo].[ScriptTracking]");
            //Get Day closing prices
            var scriptPriceItem = (from s in context.MarketDatas
                                   orderby s.CompanyName
                                   select s).ToList();

            foreach (MarketData marketDataItem in scriptPriceItem)
            {
                if (!string.IsNullOrWhiteSpace(marketDataItem.Current)  && Convert.ToDouble(marketDataItem.Current) <= 0.00) continue;
                int exchangeCode = 0;
                if (marketDataItem.Exchange.Equals("NSE")) exchangeCode = (int)ExchangeCode.NC;
                if (marketDataItem.Exchange.Equals("NSEFO")) exchangeCode = (int)ExchangeCode.NF;

                int scriptId = 0;
                if (context.Scripts.Any(s => s.ScriptCode.Trim().Equals(marketDataItem.ScripCode.Trim()) && s.ScriptMarketExchangeId == exchangeCode))
                {
                    ScriptTrackingStatus trackingStatus = ScriptTrackingStatus.None;
                    scriptId = context.Scripts.Where(s => s.ScriptCode.Trim().Equals(marketDataItem.ScripCode.Trim()) && s.ScriptMarketExchangeId == exchangeCode).First().ScriptId;
                    trackingStatus = CheckIfNearDaysLow(marketDataItem);

                    //if (either of the cases is true, update the tracking table)
                    if (trackingStatus != ScriptTrackingStatus.None && trackingStatus != ScriptTrackingStatus.CrossedPreviousLow)
                    {
                        AddNewScriptTrackingStatus(scriptId, trackingStatus, DateTime.ParseExact(marketDataItem.LastTradedDate, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(marketDataItem.Current), isOpenDayLowSamePriceStatus: CheckOpenDayLowSamePrice(marketDataItem));
                    }
                    else
                    {
                        trackingStatus = CheckIfNearDaysHigh(marketDataItem);
                        if (trackingStatus != ScriptTrackingStatus.None && trackingStatus != ScriptTrackingStatus.CrossedPreviousHigh)
                        {
                            AddNewScriptTrackingStatus(scriptId, trackingStatus, DateTime.ParseExact(marketDataItem.LastTradedDate, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(marketDataItem.Current), isOpenDayHighSamePriceStatus: CheckOpenDayHighSamePrice(marketDataItem));
                        }
                    }
                }

            }
        }

        private void AddNewScriptTrackingStatus(int scriptId, ScriptTrackingStatus trackingStatus, DateTime tradeDate, double closingPrice, bool isOpenDayLowSamePriceStatus=false, bool isOpenDayHighSamePriceStatus=false)
        {
            bool scriptExist = context.ScriptTrackings.Any(s => s.ScriptId == scriptId && s.IsDailyTrackingStatus);
            if (!scriptExist)
            {
                //Add
                ScriptTracking scriptTrackingItem = new ScriptTracking()
                {
                    ScriptId = scriptId,
                    IsDailyTrackingStatus = true,
                    IsOpenLowSamePrice = isOpenDayLowSamePriceStatus,
                    IsOpenHighSamePrice = isOpenDayHighSamePriceStatus,
                    ScriptTrackingStatus = (int)trackingStatus,
                    TradeDate = tradeDate,
                    ClosingPrice = closingPrice
                };
                context.ScriptTrackings.Add(scriptTrackingItem);
                context.SaveChanges();
            }
        }

        private ScriptTrackingStatus CheckIfNearDaysLow(MarketData marketDataItem)
        {

            bool isAnyConditionTrue = false;
            double dayLowPrice = Convert.ToDouble(marketDataItem.Low);
            double dayHighPrice = Convert.ToDouble(marketDataItem.High);
            double dayOpenPrice = Convert.ToDouble(marketDataItem.Open);
            double dayClosePrice = Convert.ToDouble(marketDataItem.Current);
            double previousLowPrice = Convert.ToDouble(marketDataItem.P35Low);
            double previousHighPrice = Convert.ToDouble(marketDataItem.P35High);
            double previousOpenPrice = Convert.ToDouble(marketDataItem.P35Open);
            double previousClosePrice = Convert.ToDouble(marketDataItem.P35Close);
            double lowPriceRadar = previousLowPrice + ((previousLowPrice * 0.1) / 100);

            //
            if (dayLowPrice < previousLowPrice && dayHighPrice > previousHighPrice)
            {
                return ScriptTrackingStatus.CrossedPreviousHighAndLow;
            }

            //Stock price has gone below previous days low, mark this stock to be avoided for trade
            if (dayLowPrice < previousLowPrice)
            {
                return ScriptTrackingStatus.CrossedPreviousLow;
            }

            //Stock price is near previous days low and has not gone below previous low, mark this stock to be watched for going long, day high is also below radar price
            if (previousLowPrice < dayLowPrice && dayLowPrice < lowPriceRadar)
            {
                isAnyConditionTrue = true;

                if (dayClosePrice > dayOpenPrice)
                {
                    return ScriptTrackingStatus.NearPreviousLowClosingAboveOpenPrice;
                }
                return ScriptTrackingStatus.NearPreviousLow;
                //selectedScript.TradedCondition = "near previous days low";
            }
            else //(previousLowPrice < dayLowPrice ..............check condition for not broken previous lows.. then where is it and what is interesting that it is doing...
            {

            }

            return ScriptTrackingStatus.None;

        }
        
        private ScriptTrackingStatus CheckIfNearDaysHigh(MarketData marketDataItem)
        {
            bool isAnyConditionTrue = false;
            double dayLowPrice = Convert.ToDouble(marketDataItem.Low);
            double dayHighPrice = Convert.ToDouble(marketDataItem.High);
            double dayOpenPrice = Convert.ToDouble(marketDataItem.Open);
            double dayClosePrice = Convert.ToDouble(marketDataItem.Current);
            double previousLowPrice = Convert.ToDouble(marketDataItem.P35Low);
            double previousHighPrice = Convert.ToDouble(marketDataItem.P35High);
            double previousOpenPrice = Convert.ToDouble(marketDataItem.P35Open);
            double previousClosePrice = Convert.ToDouble(marketDataItem.P35Close);
            double highPriceRadar = previousHighPrice - ((previousHighPrice * 0.1) / 100);

            if (dayLowPrice < previousLowPrice && dayHighPrice > previousHighPrice)
            {
                return ScriptTrackingStatus.CrossedPreviousHighAndLow;
            }

            //Stock price has gone below previous days low, mark this stock to be avoided for trade
            if (dayHighPrice > previousHighPrice)
            {
                return ScriptTrackingStatus.CrossedPreviousHigh;
            }

            //Stock price is near previous days low and has not gone below previous low, mark this stock to be watched for going long, day high is also below radar price
            if (previousHighPrice >= dayHighPrice && dayHighPrice > highPriceRadar)
            {
                isAnyConditionTrue = true;
                if (dayClosePrice < dayOpenPrice)
                    return ScriptTrackingStatus.NearPreviousHighClosingBelowOpenPrice;
                return ScriptTrackingStatus.NearPreviousHigh;
                //selectedScript.TradedCondition = "near previous days highs";
            }


            return ScriptTrackingStatus.None;

        }

        private bool CheckOpenDayLowSamePrice(MarketData marketDataItem)
        {
            double dayLowPrice = Convert.ToDouble(marketDataItem.Low);
            double dayHighPrice = Convert.ToDouble(marketDataItem.High);
            double dayOpenPrice = Convert.ToDouble(marketDataItem.Open);
            
            return (dayLowPrice == dayOpenPrice);

        }

        private bool CheckOpenDayHighSamePrice(MarketData marketDataItem)
        {
            double dayLowPrice = Convert.ToDouble(marketDataItem.Low);
            double dayHighPrice = Convert.ToDouble(marketDataItem.High);
            double dayOpenPrice = Convert.ToDouble(marketDataItem.Open);

            return (dayHighPrice == dayOpenPrice);
        }

        public bool LoadMarketDataFromExcel(List<MarketData> excelData)
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Stocks].[dbo].[MarketData]");

            foreach (MarketData marketDataItem in excelData)
            {
                var newMarketData = new MarketData()
                {
                    BidPrice = marketDataItem.BidPrice,
                    BidQty = marketDataItem.BidQty,
                    C37Change = marketDataItem.C37Change,
                    Close = marketDataItem.Close,
                    CompanyName = marketDataItem.CompanyName,
                    CreateDate = marketDataItem.CreateDate,
                    Current = marketDataItem.Current,
                    Exchange = marketDataItem.Exchange,
                    High = marketDataItem.High,
                    LastTradedDate = marketDataItem.LastTradedDate,
                    LastTradedQty = marketDataItem.LastTradedQty,
                    LastTradedTime = marketDataItem.LastTradedTime,
                    LastUpdatedTime = marketDataItem.LastUpdatedTime,
                    Low = marketDataItem.Low,
                    OfferPrice = marketDataItem.OfferPrice,
                    OfferQty = marketDataItem.OfferQty,
                    OiDifference = marketDataItem.OiDifference,
                    OiDifferencePercentage = marketDataItem.OiDifferencePercentage,
                    Open = marketDataItem.Open,
                    P35Close = marketDataItem.P35Close,
                    P35High = marketDataItem.P35High,
                    P35Low = marketDataItem.P35Low,
                    P35Open = marketDataItem.P35Open,
                    P35Quantity = marketDataItem.P35Quantity,
                    Pivot = marketDataItem.Pivot,
                    PivotRes1 = marketDataItem.PivotRes1,
                    PivotRes2 = marketDataItem.PivotRes2,
                    PivotRes3 = marketDataItem.PivotRes3,
                    PivotSup1 = marketDataItem.PivotSup1,
                    PivotSup2 = marketDataItem.PivotSup2,
                    PivotSup3 = marketDataItem.PivotSup3,
                    Qty = marketDataItem.Qty,
                    ScripCode = marketDataItem.ScripCode,
                    ScripName = marketDataItem.ScripName,
                    TotalBuyQty = marketDataItem.TotalBuyQty,
                    TotalSellQty = marketDataItem.TotalSellQty,
                    StockId = marketDataItem.StockId
                };
                context.Entry(newMarketData).State = System.Data.Entity.EntityState.Added;
                context.MarketDatas.Add(newMarketData);

                context.SaveChanges();

                //string newMarketDataStmt = "INSERT INTO [dbo].[Nifty] ([Exchange] ,[Scrip Name] ,[% Change], [Current]";
                //newMarketDataStmt = newMarketDataStmt + ",[Last Traded Qty],[Bid Qty],[Bid Price] ,[Offer Price],[Offer Qty],[Open],[High],[Low],[Close]";
                //newMarketDataStmt = newMarketDataStmt + ",[Last Updated Time],[Last Traded Time],[Last Traded Date],[Qty],[Total Buy Qty],[Scrip Code],[Total Sell Qty],[OI Difference]";
                //newMarketDataStmt = newMarketDataStmt + ",[OI Difference Percentage],[Company Name],[P#Open],[P#High],[P#Low],[P#Close],[P#Quantity],[Pivot Res 3],[Pivot Res 2]";
                //newMarketDataStmt = newMarketDataStmt + ",[Pivot Res 1],[Pivot],[Pivot Sup 1],[Pivot Sup 2],[Pivot Sup 3])";

                //newMarketDataStmt = newMarketDataStmt + String.Format(" VALUES('{0}','{1}','{2}','{3}','{4}'", marketDataItem.Exchange, marketDataItem.ScripName, marketDataItem.C37Change, marketDataItem.Current, marketDataItem.LastTradedQty);
                //newMarketDataStmt = newMarketDataStmt + String.Format(",'{0}','{1}','{2}','{3}','{4}'", marketDataItem.BidQty, marketDataItem.BidPrice, marketDataItem.OfferPrice, marketDataItem.OfferQty, marketDataItem.Open);
                //newMarketDataStmt = newMarketDataStmt + String.Format(" ,'{0}','{1}','{2}','{3}' ,'{4}' ", marketDataItem.High, marketDataItem.Low, marketDataItem.Close, marketDataItem.LastUpdatedTime, marketDataItem.LastTradedTime);
                //newMarketDataStmt = newMarketDataStmt + String.Format(" ,'{0}','{1}','{2}','{3}','{4}'", marketDataItem.LastTradedDate, marketDataItem.Qty, marketDataItem.TotalBuyQty, marketDataItem.ScripCode, marketDataItem.TotalSellQty);
                //newMarketDataStmt = newMarketDataStmt + String.Format("  ,'{0}','{1}' ,'{2}','{3}'", marketDataItem.OiDifference, marketDataItem.OiDifferencePercentage, marketDataItem.CompanyName.Replace("'", ""), marketDataItem.P35Open);
                //newMarketDataStmt = newMarketDataStmt + String.Format(",'{0}','{1}','{2}','{3}','{4}'", marketDataItem.P35High, marketDataItem.P35Low, marketDataItem.P35Close, marketDataItem.P35Quantity, marketDataItem.PivotRes3);
                //newMarketDataStmt = newMarketDataStmt + String.Format(",'{0}','{1}','{2}','{3}','{4}'", marketDataItem.PivotRes2, marketDataItem.PivotRes1, marketDataItem.Pivot, marketDataItem.PivotSup1, marketDataItem.PivotSup2);
                //newMarketDataStmt = newMarketDataStmt + String.Format(",'{0}')", marketDataItem.PivotSup3);

                //context.Database.ExecuteSqlCommand(newMarketDataStmt);

                //if (context.NiftyPivots.Any(s => s.ScripCode.Trim().Equals(marketDataItem.ScripCode.Trim())))
                //{

                //}
                //else
                //{

                //}

            }
            return true;
        }

        public List<ScriptTrackingView> GetScriptTrackingViewData()
        {
          return context.ScriptTrackingViews.ToList();
        } 

        //public bool CheckPriceTrend(List<ScriptPriceView> scriptPriceViewItem)
        //{
        //    ScriptPriceView previousPriceItem = null;
        //    ScriptPriceView currentPriceItem = null;
        //    bool trendStatusRecorded = false;

        //    foreach (ScriptPriceView priceItem in scriptPriceViewItem)
        //    {
        //        if (currentPriceItem == null)
        //        {
        //            currentPriceItem = priceItem;
        //            continue;
        //        }

        //        if (previousPriceItem == null)
        //        {
        //            previousPriceItem = priceItem;
        //        }

        //        //First check if both high and lows are crossed
        //        if (currentPriceItem.DayLow < previousPriceItem.DayLow && currentPriceItem.DayHigh > previousPriceItem.DayHigh)
        //        {
        //            //update the status to database of both previous high low cross

        //            //check for near high price or low price to decide the direction
        //        }


        //        if (currentPriceItem.DayLow < previousPriceItem.DayLow)
        //        { 
        //            //trend is down..insert the status to database
        //            //TODO:

        //            //Check if it was near previous price item 
        //            if (previousPriceItem.DayHigh < currentPriceItem.DayHigh) { }

        //        }


        //        currentPriceItem = previousPriceItem;
        //        previousPriceItem = null;
        //        trendStatusRecorded = true;
        //    }

        //    return true;
        //}

    }
}
