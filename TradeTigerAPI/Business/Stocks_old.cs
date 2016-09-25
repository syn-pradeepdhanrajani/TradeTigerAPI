using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TradeTigerAPI.Utilities;
using System.Media;
using System.Threading;
using System.Net.Http.Formatting;

namespace TradeTigerAPI.Business
{
    public class Stocks 
    {

        public Dictionary<string, Nifty> scriptsInBuyRadar = new Dictionary<string, Nifty>();
        public Dictionary<string, Nifty> scriptsBuy = new Dictionary<string, Nifty>();
        public Dictionary<string, Nifty> scriptsInShortRadar = new Dictionary<string, Nifty>();
        public Dictionary<string, Nifty> scriptsShort = new Dictionary<string, Nifty>();

        public Scripts<Nifty> ScriptsInBuyRadarCollection { get; set; }
        public Scripts<Nifty> ScriptsBuyCollection { get; set; }
        public Scripts<Nifty> ScriptsInShortRadarCollection { get; set; }
        public Scripts<Nifty> ScriptsShortCollection { get; set; }

        IEnumerable<Nifty> niftyStocks = null;
        public bool IsScriptsInitialized = false;
        public static Nifty TrackingObject = new Nifty();

        #region Stock Scripts
        public Nifty Script1 { get; set; }
        public Nifty Script2 { get; set; }
        public Nifty Script3 { get; set; }
        public Nifty Script4 { get; set; }
        public Nifty Script5 { get; set; }
        public Nifty Script6 { get; set; }
        public Nifty Script7 { get; set; }
        public Nifty Script8 { get; set; }
        public Nifty Script9 { get; set; }
        public Nifty Script10 { get; set; }
        public Nifty Script11 { get; set; }
        public Nifty Script12 { get; set; }
        public Nifty Script13 { get; set; }
        public Nifty Script14 { get; set; }
        public Nifty Script15 { get; set; }
        public Nifty Script16 { get; set; }
        public Nifty Script17 { get; set; }
        public Nifty Script18 { get; set; }
        public Nifty Script19 { get; set; }
        public Nifty Script20 { get; set; }
        public Nifty Script21 { get; set; }
        public Nifty Script22 { get; set; }
        public Nifty Script23 { get; set; }
        public Nifty Script24 { get; set; }
        public Nifty Script25 { get; set; }
        public Nifty Script26 { get; set; }
        public Nifty Script27 { get; set; }
        public Nifty Script28 { get; set; }
        public Nifty Script29 { get; set; }
        public Nifty Script30 { get; set; }
        public Nifty Script31 { get; set; }
        public Nifty Script32 { get; set; }
        public Nifty Script33 { get; set; }
        public Nifty Script34 { get; set; }
        public Nifty Script35 { get; set; }
        public Nifty Script36 { get; set; }
        public Nifty Script37 { get; set; }
        public Nifty Script38 { get; set; }
        public Nifty Script39 { get; set; }
        public Nifty Script40 { get; set; }
        public Nifty Script41 { get; set; }
        public Nifty Script42 { get; set; }
        public Nifty Script43 { get; set; }
        public Nifty Script44 { get; set; }
        public Nifty Script45 { get; set; }
        public Nifty Script46 { get; set; }
        public Nifty Script47 { get; set; }
        public Nifty Script48 { get; set; }
        public Nifty Script49 { get; set; }
        public Nifty Script50 { get; set; }
        public Nifty Script51 { get; set; }
        public Nifty Script52 { get; set; }
        public Nifty Script53 { get; set; }
        public Nifty Script54 { get; set; }
        public Nifty Script55 { get; set; }
        public Nifty Script56 { get; set; }
        public Nifty Script57 { get; set; }
        public Nifty Script58 { get; set; }
        public Nifty Script59 { get; set; }
        public Nifty Script60 { get; set; }
        public Nifty Script61 { get; set; }
        public Nifty Script62 { get; set; }
        public Nifty Script63 { get; set; }
        public Nifty Script64 { get; set; }
        public Nifty Script65 { get; set; }
        public Nifty Script66 { get; set; }
        public Nifty Script67 { get; set; }
        public Nifty Script68 { get; set; }
        public Nifty Script69 { get; set; }
        public Nifty Script70 { get; set; }
        public Nifty Script71 { get; set; }
        public Nifty Script72 { get; set; }
        public Nifty Script73 { get; set; }
        public Nifty Script74 { get; set; }
        public Nifty Script75 { get; set; }
        public Nifty Script76 { get; set; }
        public Nifty Script77 { get; set; }
        public Nifty Script78 { get; set; }
        public Nifty Script79 { get; set; }
        public Nifty Script80 { get; set; }


        #endregion

        public Stocks()
        {
            ScriptsInBuyRadarCollection = new Scripts<Nifty>();
            ScriptsBuyCollection = new Scripts<Nifty>();
            ScriptsInShortRadarCollection = new Scripts<Nifty>();
            ScriptsShortCollection = new Scripts<Nifty>();
            ClearServerCache();
        }

        public async System.Threading.Tasks.Task<List<Nifty>> GetNiftyData()
        {
            
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://localhost:9192/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // New code:
                    HttpResponseMessage response = await client.GetAsync("api/stocks/");
                    if (response.IsSuccessStatusCode)
                    {
                        niftyStocks = await response.Content.ReadAsAsync<IEnumerable<Nifty>>();
                    }
                }
                catch { }
            }

            return niftyStocks.ToList();

        }

        #region Update Web APIs

        public async void PostBuyRadarData(Scripts<Nifty> niftyList)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    client.BaseAddress = new Uri("http://ensembleframework-api-dev.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new ObjectContent(typeof(List<Nifty>), niftyList.ToList(), new JsonMediaTypeFormatter());
                    content.LoadIntoBufferAsync().Wait();
                    // New code:
                    await client.PostAsync("api/stocks/PostBuyRadarScripts", content, source.Token);
                }
                catch { }
                
            }

        }

        public async void PostBuyData(Scripts<Nifty> niftyList)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    client.BaseAddress = new Uri("http://ensembleframework-api-dev.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new ObjectContent(typeof(List<Nifty>), niftyList.ToList(), new JsonMediaTypeFormatter());
                    content.LoadIntoBufferAsync().Wait();
                    // New code:
                    await client.PostAsync("api/stocks/PostBuyScripts", content, source.Token);

                }
                catch { }

            }
            
            
        }

        public async void PostShortsRadarData(Scripts<Nifty> niftyList)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    client.BaseAddress = new Uri("http://ensembleframework-api-dev.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new ObjectContent(typeof(List<Nifty>), niftyList.ToList(), new JsonMediaTypeFormatter());
                    content.LoadIntoBufferAsync().Wait();
                    // New code:
                    await client.PostAsync("api/stocks/PostShortsRadarScripts", content, source.Token);
                }
                catch { }

            }

        }

        public async void PostShortsData(Scripts<Nifty> niftyList)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    client.BaseAddress = new Uri("http://ensembleframework-api-dev.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new ObjectContent(typeof(List<Nifty>), niftyList.ToList(), new JsonMediaTypeFormatter());
                    content.LoadIntoBufferAsync().Wait();
                    // New code:
                    await client.PostAsync("api/stocks/PostShortsScripts", content, source.Token);
                }
                catch { }
            }

        }
        #endregion

        public void InitializeStockScripts(List<Nifty> stockScripts)
        {
            if (stockScripts != null && stockScripts.Count > 0)
            {
                IsScriptsInitialized = true;
                Script1 = stockScripts[0] as Nifty;
                Script2 = stockScripts[1] as Nifty;
                Script3 = stockScripts[2] as Nifty;
                Script4 = stockScripts[3] as Nifty;
                Script5 = stockScripts[4] as Nifty;
                Script6 = stockScripts[5] as Nifty;
                Script7 = stockScripts[6] as Nifty;
                Script8 = stockScripts[7] as Nifty;
                Script9 = stockScripts[8] as Nifty;
                Script10 = stockScripts[9] as Nifty;
                Script11 = stockScripts[10] as Nifty;
                Script12 = stockScripts[11] as Nifty;
                Script13 = stockScripts[12] as Nifty;
                Script14 = stockScripts[13] as Nifty;
                Script15 = stockScripts[14] as Nifty;
                Script16 = stockScripts[15] as Nifty;
                Script17 = stockScripts[16] as Nifty;
                Script18 = stockScripts[17] as Nifty;
                Script19 = stockScripts[18] as Nifty;
                Script20 = stockScripts[19] as Nifty;
                Script21 = stockScripts[20] as Nifty;
                Script22 = stockScripts[21] as Nifty;
                Script23 = stockScripts[22] as Nifty;
                Script24 = stockScripts[23] as Nifty;
                Script25 = stockScripts[24] as Nifty;
                Script26 = stockScripts[25] as Nifty;
                Script27 = stockScripts[26] as Nifty;
                Script28 = stockScripts[27] as Nifty;
                Script29 = stockScripts[28] as Nifty;
                Script30 = stockScripts[29] as Nifty;
                Script31 = stockScripts[30] as Nifty;
                Script32 = stockScripts[31] as Nifty;
                Script33 = stockScripts[32] as Nifty;
                Script34 = stockScripts[33] as Nifty;
                Script35 = stockScripts[34] as Nifty;
                Script36 = stockScripts[35] as Nifty;
                Script37 = stockScripts[36] as Nifty;
                Script38 = stockScripts[37] as Nifty;
                Script39 = stockScripts[38] as Nifty;
                Script40 = stockScripts[39] as Nifty;
                Script41 = stockScripts[40] as Nifty;
                Script42 = stockScripts[41] as Nifty;
                Script43 = stockScripts[42] as Nifty;
                Script44 = stockScripts[43] as Nifty;
                Script45 = stockScripts[44] as Nifty;
                Script46 = stockScripts[45] as Nifty;
                Script47 = stockScripts[46] as Nifty;
                Script48 = stockScripts[47] as Nifty;
                Script49 = stockScripts[48] as Nifty;
                Script50 = stockScripts[49] as Nifty;
                Script51 = stockScripts[50] as Nifty;
                Script52 = stockScripts[51] as Nifty;
                Script53 = stockScripts[52] as Nifty;
                Script54 = stockScripts[53] as Nifty;
                Script55 = stockScripts[54] as Nifty;
                Script56 = stockScripts[55] as Nifty;
                Script57 = stockScripts[56] as Nifty;
                Script58 = stockScripts[57] as Nifty;
                Script59 = stockScripts[58] as Nifty;
                Script60 = stockScripts[59] as Nifty;
                Script61 = stockScripts[60] as Nifty;
                Script62 = stockScripts[61] as Nifty;
                Script63 = stockScripts[62] as Nifty;
                Script64 = stockScripts[63] as Nifty;
                Script65 = stockScripts[64] as Nifty;
                Script66 = stockScripts[65] as Nifty;
                Script67 = stockScripts[66] as Nifty;
                Script68 = stockScripts[67] as Nifty;
                Script69 = stockScripts[68] as Nifty;
                Script70 = stockScripts[69] as Nifty;
                Script71 = stockScripts[70] as Nifty;
                Script72 = stockScripts[71] as Nifty;
                Script73 = stockScripts[72] as Nifty;
                Script74 = stockScripts[73] as Nifty;
                Script75 = stockScripts[74] as Nifty;
                Script76 = stockScripts[75] as Nifty;
                Script77 = stockScripts[76] as Nifty;
                Script78 = stockScripts[77] as Nifty;
                Script79 = stockScripts[78] as Nifty;
                Script80 = stockScripts[79] as Nifty;

            }
        }

        public Nifty TrackScripts(string feedData)
        {
            if (IsScriptsInitialized)
            {
                if (TrackScripts(feedData, Script1)) return Script1;
                if (TrackScripts(feedData, Script2)) return Script2;
                if (TrackScripts(feedData, Script3)) return Script3;
                if (TrackScripts(feedData, Script4)) return Script4;
                if (TrackScripts(feedData, Script5)) return Script5;
                if (TrackScripts(feedData, Script6)) return Script6;
                if (TrackScripts(feedData, Script7)) return Script7;
                if (TrackScripts(feedData, Script8)) return Script8;
                if (TrackScripts(feedData, Script9)) return Script9;
                if (TrackScripts(feedData, Script10)) return Script10;
                if (TrackScripts(feedData, Script11)) return Script11;
                if (TrackScripts(feedData, Script12)) return Script12;
                if (TrackScripts(feedData, Script13)) return Script13;
                if (TrackScripts(feedData, Script14)) return Script14;
                if (TrackScripts(feedData, Script15)) return Script15;
                if (TrackScripts(feedData, Script16)) return Script16;
                if (TrackScripts(feedData, Script17)) return Script17;
                if (TrackScripts(feedData, Script18)) return Script18;
                if (TrackScripts(feedData, Script19)) return Script19;
                if (TrackScripts(feedData, Script20)) return Script20;
                if (TrackScripts(feedData, Script21)) return Script21;
                if (TrackScripts(feedData, Script22)) return Script22;
                if (TrackScripts(feedData, Script23)) return Script23;
                if (TrackScripts(feedData, Script24)) return Script24;
                if (TrackScripts(feedData, Script25)) return Script25;
                if (TrackScripts(feedData, Script26)) return Script26;
                if (TrackScripts(feedData, Script27)) return Script27;
                if (TrackScripts(feedData, Script28)) return Script28;
                if (TrackScripts(feedData, Script29)) return Script29;
                if (TrackScripts(feedData, Script30)) return Script30;
                if (TrackScripts(feedData, Script31)) return Script31;
                if (TrackScripts(feedData, Script32)) return Script32;
                if (TrackScripts(feedData, Script33)) return Script33;
                if (TrackScripts(feedData, Script34)) return Script34;
                if (TrackScripts(feedData, Script35)) return Script35;
                if (TrackScripts(feedData, Script36)) return Script36;
                if (TrackScripts(feedData, Script37)) return Script37;
                if (TrackScripts(feedData, Script38)) return Script38;
                if (TrackScripts(feedData, Script39)) return Script39;
                if (TrackScripts(feedData, Script40)) return Script40;
                if (TrackScripts(feedData, Script41)) return Script41;
                if (TrackScripts(feedData, Script42)) return Script42;
                if (TrackScripts(feedData, Script43)) return Script43;
                if (TrackScripts(feedData, Script44)) return Script44;
                if (TrackScripts(feedData, Script45)) return Script45;
                if (TrackScripts(feedData, Script46)) return Script46;
                if (TrackScripts(feedData, Script47)) return Script47;
                if (TrackScripts(feedData, Script48)) return Script48;
                if (TrackScripts(feedData, Script49)) return Script49;
                if (TrackScripts(feedData, Script50)) return Script50;
                if (TrackScripts(feedData, Script51)) return Script51;
                
            }
            return new Nifty();
        }

        public bool TrackScripts(string feedData, Nifty trackScript)
        {
            if (feedData.Contains("ScripToken = " +trackScript.ScripCode))
            {
                trackScript.TradedCondition = "";
                string[] stockFeedData = feedData.Split('|');
                if ((!trackScript.PreviousLowCrossed) && (!trackScript.IsInHighPriceRadar || !trackScript.IsScriptShortCandidate)) CheckIfNearDaysLow(stockFeedData, trackScript);
                if ((!trackScript.PreviousHighCrossed) && (!trackScript.IsInLowPriceRadar || !trackScript.IsScriptBuyCandidate)) CheckIfNearDaysHigh(stockFeedData, trackScript);

                Task.Run(() =>
                {
                    App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                    {
                        AddToTrackingCollection(trackScript);
                    });
                });

                return true;
            }
            return false;
        }

        public bool CheckIfNearDaysLow(string[] feedData, Nifty selectedScript)
        {
            bool isAnyConditionTrue = false;
            double ltPrice = GetLastTradedPrice(feedData);
            selectedScript.LastTradedPriceFromFeed = ltPrice;
            selectedScript.LastTradedTimeFromFeed = GetLastTradedDateTime(feedData).Trim().Substring(10);
            double dayLowPrice = GetDayLowPriceFromFeedData(feedData);
            double dayHighPrice = GetDayHighPriceFromFeedData(feedData);
            double dayOpenPrice = GetDayOpenPriceFromFeedData(feedData);
            double previousLow = Convert.ToDouble(selectedScript.Low);
            double lowPriceRadar = previousLow + ((previousLow * 0.5) / 100);

            //Stock price has gone below previous days low, mark this stock to be avoided for trade
            if (dayLowPrice < previousLow)
            {
                selectedScript.PreviousLowCrossed = true;
                selectedScript.IsInLowPriceRadar = false;
                selectedScript.IsScriptBuyCandidate = false;
                return true;
            }

            //Stock price is near previous days low and has not gone below previous low, mark this stock to be watched for going long, day high is also below radar price
            if (previousLow <= dayLowPrice && ltPrice < lowPriceRadar && dayHighPrice <= lowPriceRadar)
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptBuyCandidate = false;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.TradedCondition = "near previous days low";
            }
            else if (previousLow <= dayLowPrice && dayLowPrice <= lowPriceRadar && ltPrice > lowPriceRadar && dayHighPrice <= (lowPriceRadar + lowPriceRadar * 0.01) && ltPrice <= (lowPriceRadar + lowPriceRadar * 0.003) && (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.IsScriptBuyCandidate = false; //this condition is to check if the last trade price is above radar price after being close to previous lows.
                selectedScript.TradedCondition = "was near previous low & above radar price now";// + (previousLow + " <= " + dayLowPrice + " && " + dayLowPrice + "<=" + lowPriceRadar + " && " + ltPrice + ">" + lowPriceRadar + "&&" + dayHighPrice + "<=" + (lowPriceRadar + lowPriceRadar * 0.01) + "&&" + ltPrice + "<=" + (lowPriceRadar + lowPriceRadar * 0.003) + "&&" + (ltPrice + (ltPrice * 0.001) + ">" + dayHighPrice));
            }

            //Day near low and open prices are same, best candidate for trade.
            if (previousLow <= dayLowPrice && (dayOpenPrice == dayLowPrice && ltPrice > dayOpenPrice) && (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptBuyCandidate = false;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.TradedCondition = "open & day low same price";
            }
            //Below condition the prices have not gone very much below from the opening and it is trying to cover back and is above day open prices
            else if (previousLow <= dayLowPrice && (dayLowPrice >= (dayOpenPrice - (dayOpenPrice * 0.05)) && ltPrice >= dayOpenPrice) && (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {   
                isAnyConditionTrue = true;
                selectedScript.IsScriptBuyCandidate = false;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.TradedCondition = "above open price";
                if (((ltPrice + (ltPrice * 0.001)) >= dayOpenPrice) && ((dayOpenPrice + dayOpenPrice*0.002) > dayHighPrice))
                {
                    SystemSounds.Exclamation.Play();
                    selectedScript.IsScriptBuyCandidate = true;
                    selectedScript.IsInLowPriceRadar = false;
                }
            }

            if (!isAnyConditionTrue)
            {
                selectedScript.IsInLowPriceRadar = false;
                selectedScript.IsScriptBuyCandidate = false;
            }
            return true;
        }
        
        public bool CheckIfNearDaysHigh(string[] feedData, Nifty selectedScript)
        {
            bool isAnyConditionTrue = false;
            double ltPrice = GetLastTradedPrice(feedData);
            selectedScript.LastTradedPriceFromFeed = ltPrice;
            selectedScript.LastTradedTimeFromFeed = GetLastTradedDateTime(feedData).Trim().Substring(10);
            double dayLowPrice = GetDayLowPriceFromFeedData(feedData);
            double dayHighPrice = GetDayHighPriceFromFeedData(feedData);
            double dayOpenPrice = GetDayOpenPriceFromFeedData(feedData);
            double previousHigh = Convert.ToDouble(selectedScript.High);
            double highPriceRadar = previousHigh - ((previousHigh * 0.5) / 100);

            //Stock price has gone below previous days low, mark this stock to be avoided for trade
            if (dayHighPrice > previousHigh) {
                selectedScript.PreviousHighCrossed = true;
                selectedScript.IsInHighPriceRadar = false;
                selectedScript.IsScriptShortCandidate = false;
                return true;
            } 

            //Stock price is near previous days low and has not gone below previous low, mark this stock to be watched for going long
            if (previousHigh >= dayHighPrice && ltPrice > highPriceRadar && dayLowPrice >= highPriceRadar)
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.TradedCondition = "near previous days high";
            }
            else if (previousHigh >= dayHighPrice && dayHighPrice >= highPriceRadar && ltPrice <= highPriceRadar && dayLowPrice >= (highPriceRadar - highPriceRadar * 0.01) && ltPrice >= (highPriceRadar - highPriceRadar * 0.003) && (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.TradedCondition = "was near previous high & below radar price now";//+ previousHigh +">="+ dayHighPrice +"&&"+ dayHighPrice +">="+ highPriceRadar +"&&"+ ltPrice +"<="+ highPriceRadar +"&&"+ dayLowPrice +">="+ (highPriceRadar - highPriceRadar * 0.01) +"&&"+ ltPrice +">="+ (highPriceRadar - highPriceRadar * 0.003) +"&&"+ (ltPrice - (ltPrice * 0.001) < dayLowPrice);
            }

            //Day near low and open prices are same, best candidate for trade.
            if (previousHigh >= dayHighPrice && (dayOpenPrice == dayHighPrice && ltPrice < dayHighPrice) && (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.TradedCondition = "open & day high same price";
            }
            else if (previousHigh >= dayHighPrice && ((dayHighPrice <= (dayOpenPrice + dayOpenPrice*0.05)) && ltPrice < dayHighPrice) && (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.TradedCondition = "below open price";
                if ((ltPrice - (ltPrice * 0.001)) <= dayOpenPrice && ((dayOpenPrice - dayOpenPrice * 0.002) < dayLowPrice))
                {
                    SystemSounds.Exclamation.Play();
                    selectedScript.IsScriptShortCandidate = true;
                    selectedScript.IsInHighPriceRadar = false;
                }
            }

            if (!isAnyConditionTrue)
            {
                selectedScript.IsInHighPriceRadar = false;
                selectedScript.IsScriptShortCandidate = false;
            }

            return true;
        }
        
        private double GetLastTradedPrice(string[] feedData)
        {
            double ltPrice = 0.0;
            string[] stockFeedData = feedData;
            if (stockFeedData.Length > 0)
            {
                string[] feedDataLtPrice = stockFeedData[4].Split('=');
                if (feedDataLtPrice.Length > 0)
                {
                    ltPrice = Convert.ToDouble(feedDataLtPrice[1]) / 100;
                }
            }
            return ltPrice;
        }

        private string GetLastTradedDateTime(string[] feedData)
        {
            string ltDateTime = "";
            string[] stockFeedData = feedData;
            if (stockFeedData.Length > 0)
            {
                string[] feedDataLtPrice = stockFeedData[6].Split('=');
                if (feedDataLtPrice.Length > 0)
                {
                    ltDateTime = feedDataLtPrice[1];
                }
            }
            return ltDateTime;
        }

        private double GetDayLowPriceFromFeedData(string[] feedData)
        {
            double ltPrice = 0.0;
            string[] stockFeedData = feedData;
            if (stockFeedData.Length > 0)
            {
                string[] feedDataPrice = stockFeedData[16].Split('='); 
                if (feedDataPrice.Length > 0)
                {
                    ltPrice = Convert.ToDouble(feedDataPrice[1]) / 100;
                }
            }
            return ltPrice;
        }

        private double GetDayHighPriceFromFeedData(string[] feedData)
        {
            double ltPrice = 0.0;
            string[] stockFeedData = feedData;
            if (stockFeedData.Length > 0)
            {
                string[] feedDataPrice = stockFeedData[15].Split('='); 
                if (feedDataPrice.Length > 0)
                {
                    ltPrice = Convert.ToDouble(feedDataPrice[1]) / 100;
                }
            }
            return ltPrice;
        }

        private double GetDayOpenPriceFromFeedData(string[] feedData)
        {
            double ltPrice = 0.0;
            string[] stockFeedData = feedData;
            if (stockFeedData.Length > 0)
            {
                string[] feedDataPrice = stockFeedData[14].Split('=');
                if (feedDataPrice.Length > 0)
                {
                    ltPrice = Convert.ToDouble(feedDataPrice[1]) / 100;
                }
            }
            return ltPrice;
        }

        private async void AddToTrackingCollection(Nifty stockScript)
        {
            RemoveFromTracking(stockScript);

            if (stockScript.IsInHighPriceRadar)
            {
                if (!scriptsInShortRadar.ContainsKey(stockScript.ScripCode.Trim())) { scriptsInShortRadar.Add(stockScript.ScripCode.Trim(), stockScript); ScriptsInShortRadarCollection.Add(stockScript); PostShortsRadarData(ScriptsInShortRadarCollection); }
                //else { 

                //// ... define after getting the List/Enumerable/whatever
                //    var dict = ScriptsInShortRadarCollection.ToDictionary(x => x);
                ////// ... somewhere in code
                //    Nifty found;
                //    if (dict.TryGetValue(stockScript.ScripCode,q out found)) found = newValue;
                //}
            }

            if (stockScript.IsInLowPriceRadar)
            {
                if (!scriptsInBuyRadar.ContainsKey(stockScript.ScripCode.Trim())) { scriptsInBuyRadar.Add(stockScript.ScripCode.Trim(), stockScript); ScriptsInBuyRadarCollection.Add(stockScript); PostBuyRadarData(ScriptsInBuyRadarCollection); };
                //else scriptsInBuyRadar[stockScript.ScripCode] = stockScript;
            }

            if (stockScript.IsScriptBuyCandidate)
            {
                if (!scriptsBuy.ContainsKey(stockScript.ScripCode.Trim())) { scriptsBuy.Add(stockScript.ScripCode.Trim(), stockScript); ScriptsBuyCollection.Add(stockScript); PostBuyData(ScriptsBuyCollection); };
                // else scriptsBuy[stockScript.ScripCode] = stockScript;
            }

            if (stockScript.IsScriptShortCandidate)
            {
                if (!scriptsShort.ContainsKey(stockScript.ScripCode.Trim())) { scriptsShort.Add(stockScript.ScripCode.Trim(), stockScript); ScriptsShortCollection.Add(stockScript); PostShortsData(ScriptsShortCollection); };
                //else scriptsShort[stockScript.ScripCode] = stockScript;
            }

        }

        private void RemoveFromTracking(Nifty stockScript)
        {
            lock (TrackingObject)
            {
                if (!stockScript.IsInHighPriceRadar && scriptsInShortRadar.ContainsKey(stockScript.ScripCode.Trim()))
                {
                    scriptsInShortRadar.Remove(stockScript.ScripCode);
                    ScriptsInShortRadarCollection.Remove(stockScript);
                    PostShortsRadarData(ScriptsInShortRadarCollection);
                }
                if (!stockScript.IsInLowPriceRadar && scriptsInBuyRadar.ContainsKey(stockScript.ScripCode.Trim())) { scriptsInBuyRadar.Remove(stockScript.ScripCode.Trim()); ScriptsInBuyRadarCollection.Remove(stockScript); PostBuyRadarData(ScriptsInBuyRadarCollection); }
                if (!stockScript.IsScriptBuyCandidate && scriptsBuy.ContainsKey(stockScript.ScripCode.Trim())) { scriptsBuy.Remove(stockScript.ScripCode.Trim()); ScriptsBuyCollection.Remove(stockScript); PostBuyData(ScriptsBuyCollection); }
                if (!stockScript.IsScriptShortCandidate && scriptsShort.ContainsKey(stockScript.ScripCode.Trim())) { scriptsShort.Remove(stockScript.ScripCode.Trim()); ScriptsShortCollection.Remove(stockScript); PostShortsData(ScriptsShortCollection); }
            }
        }

        private void ClearServerCache()
        {
            Scripts<Nifty> niftyList = new Scripts<Nifty>();
            PostShortsRadarData(niftyList);
            PostBuyRadarData(niftyList);
            PostBuyData(niftyList);
            PostShortsData(niftyList);
        }
    }
}
