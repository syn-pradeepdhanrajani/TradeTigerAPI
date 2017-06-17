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
        #region Declarations

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
        ILogger logger;

        #endregion

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
        public Nifty Script81 { get; set; }
        public Nifty Script82 { get; set; }
        public Nifty Script83 { get; set; }
        public Nifty Script84 { get; set; }
        public Nifty Script85 { get; set; }
        public Nifty Script86 { get; set; }
        public Nifty Script87 { get; set; }
        public Nifty Script88 { get; set; }
        public Nifty Script89 { get; set; }
        public Nifty Script90 { get; set; }
        public Nifty Script91 { get; set; }
        public Nifty Script92 { get; set; }
        public Nifty Script93 { get; set; }
        public Nifty Script94 { get; set; }
        public Nifty Script95 { get; set; }
        public Nifty Script96 { get; set; }
        public Nifty Script97 { get; set; }
        public Nifty Script98 { get; set; }
        public Nifty Script99 { get; set; }
        public Nifty Script100 { get; set; }
        public Nifty Script101 { get; set; }
        public Nifty Script102 { get; set; }
        public Nifty Script103 { get; set; }

        public Nifty Script104 { get; set; }
        public Nifty Script105 { get; set; }
        public Nifty Script106 { get; set; }
        public Nifty Script107 { get; set; }
        public Nifty Script108 { get; set; }
        public Nifty Script109 { get; set; }
        public Nifty Script110 { get; set; }
        public Nifty Script111 { get; set; }
        public Nifty Script112 { get; set; }
        public Nifty Script113 { get; set; }
        public Nifty Script114 { get; set; }
        public Nifty Script115 { get; set; }
        public Nifty Script116 { get; set; }
        public Nifty Script117 { get; set; }
        public Nifty Script118 { get; set; }
        public Nifty Script119 { get; set; }
        public Nifty Script120 { get; set; }
        public Nifty Script121 { get; set; }
        public Nifty Script122 { get; set; }
        public Nifty Script123 { get; set; }
        public Nifty Script124 { get; set; }
        public Nifty Script125 { get; set; }
        public Nifty Script126 { get; set; }
        public Nifty Script127 { get; set; }
        public Nifty Script128 { get; set; }
        public Nifty Script129 { get; set; }
        public Nifty Script130 { get; set; }
        public Nifty Script131 { get; set; }
        public Nifty Script132 { get; set; }
        public Nifty Script133 { get; set; }
        public Nifty Script134 { get; set; }
        public Nifty Script135 { get; set; }
        public Nifty Script136 { get; set; }
        public Nifty Script137 { get; set; }
        public Nifty Script138 { get; set; }
        public Nifty Script139 { get; set; }
        public Nifty Script140 { get; set; }
        public Nifty Script141 { get; set; }
        public Nifty Script142 { get; set; }
        public Nifty Script143 { get; set; }
        public Nifty Script144 { get; set; }
        public Nifty Script145 { get; set; }
        public Nifty Script146 { get; set; }
        public Nifty Script147 { get; set; }
        
        #endregion

        public Stocks(ILogger logManager)
        {
            logger = logManager;
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
                    client.BaseAddress = new Uri("http://jeenakamat1999-001-site1.atempurl.com/");
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
                    client.BaseAddress = new Uri("http://jeenakamat1999-001-site1.atempurl.com/");
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
                    client.BaseAddress = new Uri("http://jeenakamat1999-001-site1.atempurl.com/");
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
                    client.BaseAddress = new Uri("http://jeenakamat1999-001-site1.atempurl.com/");
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
                Script81 = stockScripts[80] as Nifty;
                Script82 = stockScripts[81] as Nifty;
                Script83 = stockScripts[82] as Nifty;
                Script84 = stockScripts[83] as Nifty;
                Script85 = stockScripts[84] as Nifty;
                Script86 = stockScripts[85] as Nifty;
                Script87 = stockScripts[86] as Nifty;
                Script88 = stockScripts[87] as Nifty;
                Script89 = stockScripts[88] as Nifty;
                Script90 = stockScripts[89] as Nifty;
                Script91 = stockScripts[90] as Nifty;
                Script92 = stockScripts[91] as Nifty;
                Script93 = stockScripts[92] as Nifty;
                Script94 = stockScripts[93] as Nifty;
                Script95 = stockScripts[94] as Nifty;
                Script96 = stockScripts[95] as Nifty;
                Script97 = stockScripts[96] as Nifty;
                Script98 = stockScripts[97] as Nifty;
                Script99 = stockScripts[98] as Nifty;
                Script100 = stockScripts[99] as Nifty;
                Script101 = stockScripts[100] as Nifty;
                Script102 = stockScripts[101] as Nifty;
                Script103 = stockScripts[102] as Nifty;
                Script104 = stockScripts[103] as Nifty;
                Script105 = stockScripts[104] as Nifty;
                Script106 = stockScripts[105] as Nifty;
                Script107 = stockScripts[106] as Nifty;
                Script108 = stockScripts[107] as Nifty;
                Script109 = stockScripts[108] as Nifty;
                Script110 = stockScripts[109] as Nifty;
                Script111 = stockScripts[110] as Nifty;
                Script112 = stockScripts[111] as Nifty;
                Script113 = stockScripts[112] as Nifty;
                Script114 = stockScripts[113] as Nifty;
                Script115 = stockScripts[114] as Nifty;
                Script116 = stockScripts[115] as Nifty;
                Script117 = stockScripts[116] as Nifty;
                Script118 = stockScripts[117] as Nifty;
                Script119 = stockScripts[118] as Nifty;
                Script120 = stockScripts[119] as Nifty;
                Script121 = stockScripts[120] as Nifty;
                Script122 = stockScripts[121] as Nifty;
                Script123 = stockScripts[122] as Nifty;
                Script124 = stockScripts[123] as Nifty;
                Script125 = stockScripts[124] as Nifty;
                Script126 = stockScripts[125] as Nifty;
                Script127 = stockScripts[126] as Nifty;
                Script128 = stockScripts[127] as Nifty;
                Script129 = stockScripts[128] as Nifty;
                Script130 = stockScripts[129] as Nifty;
                Script131 = stockScripts[130] as Nifty;
                Script132 = stockScripts[131] as Nifty;
                Script133 = stockScripts[132] as Nifty;
                Script134 = stockScripts[133] as Nifty;
                Script135 = stockScripts[134] as Nifty;
                Script136 = stockScripts[135] as Nifty;
                Script137 = stockScripts[136] as Nifty;
                Script138 = stockScripts[137] as Nifty;
                Script139 = stockScripts[138] as Nifty;
                Script140 = stockScripts[139] as Nifty;
                Script141 = stockScripts[140] as Nifty;
                Script142 = stockScripts[141] as Nifty;
                Script143 = stockScripts[142] as Nifty;
                Script144 = stockScripts[143] as Nifty;
                Script145 = stockScripts[144] as Nifty;
                Script146 = stockScripts[145] as Nifty;
                //Script147 = stockScripts[146] as Nifty;


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
                if (TrackScripts(feedData, Script52)) return Script52;
                if (TrackScripts(feedData, Script53)) return Script53;
                if (TrackScripts(feedData, Script54)) return Script54;
                if (TrackScripts(feedData, Script55)) return Script55;
                if (TrackScripts(feedData, Script56)) return Script56;
                if (TrackScripts(feedData, Script57)) return Script57;
                if (TrackScripts(feedData, Script58)) return Script58;
                if (TrackScripts(feedData, Script59)) return Script59;
                if (TrackScripts(feedData, Script60)) return Script60;
                if (TrackScripts(feedData, Script61)) return Script61;
                if (TrackScripts(feedData, Script62)) return Script62;
                if (TrackScripts(feedData, Script63)) return Script63;
                if (TrackScripts(feedData, Script64)) return Script64;
                if (TrackScripts(feedData, Script65)) return Script65;
                if (TrackScripts(feedData, Script66)) return Script66;
                if (TrackScripts(feedData, Script67)) return Script67;
                if (TrackScripts(feedData, Script68)) return Script68;
                if (TrackScripts(feedData, Script69)) return Script69;
                if (TrackScripts(feedData, Script70)) return Script70;
                if (TrackScripts(feedData, Script71)) return Script71;
                if (TrackScripts(feedData, Script72)) return Script72;
                if (TrackScripts(feedData, Script73)) return Script73;
                if (TrackScripts(feedData, Script74)) return Script74;
                if (TrackScripts(feedData, Script75)) return Script75;
                if (TrackScripts(feedData, Script76)) return Script76;
                if (TrackScripts(feedData, Script77)) return Script77;
                if (TrackScripts(feedData, Script78)) return Script78;
                if (TrackScripts(feedData, Script79)) return Script79;
                if (TrackScripts(feedData, Script80)) return Script80;
                if (TrackScripts(feedData, Script81)) return Script81;
                if (TrackScripts(feedData, Script82)) return Script82;
                if (TrackScripts(feedData, Script83)) return Script83;
                if (TrackScripts(feedData, Script84)) return Script84;
                if (TrackScripts(feedData, Script85)) return Script85;
                if (TrackScripts(feedData, Script86)) return Script86;
                if (TrackScripts(feedData, Script87)) return Script87;
                if (TrackScripts(feedData, Script88)) return Script88;
                if (TrackScripts(feedData, Script89)) return Script89;
                if (TrackScripts(feedData, Script90)) return Script90;
                if (TrackScripts(feedData, Script91)) return Script91;
                if (TrackScripts(feedData, Script92)) return Script92;
                if (TrackScripts(feedData, Script93)) return Script93;
                if (TrackScripts(feedData, Script94)) return Script94;
                if (TrackScripts(feedData, Script95)) return Script95;
                if (TrackScripts(feedData, Script96)) return Script96;
                if (TrackScripts(feedData, Script97)) return Script97;
                if (TrackScripts(feedData, Script98)) return Script98;
                if (TrackScripts(feedData, Script99)) return Script99;
                if (TrackScripts(feedData, Script100)) return Script100;
                if (TrackScripts(feedData, Script101)) return Script101;
                if (TrackScripts(feedData, Script102)) return Script102;
                if (TrackScripts(feedData, Script103)) return Script103;
                if (TrackScripts(feedData, Script104)) return Script104;
                if (TrackScripts(feedData, Script105)) return Script105;
                if (TrackScripts(feedData, Script106)) return Script106;
                if (TrackScripts(feedData, Script107)) return Script107;
                if (TrackScripts(feedData, Script108)) return Script108;
                if (TrackScripts(feedData, Script109)) return Script109;
                if (TrackScripts(feedData, Script110)) return Script110;
                if (TrackScripts(feedData, Script111)) return Script111;
                if (TrackScripts(feedData, Script112)) return Script112;
                if (TrackScripts(feedData, Script113)) return Script113;
                if (TrackScripts(feedData, Script114)) return Script114;
                if (TrackScripts(feedData, Script115)) return Script115;
                if (TrackScripts(feedData, Script116)) return Script116;
                if (TrackScripts(feedData, Script117)) return Script117;
                if (TrackScripts(feedData, Script118)) return Script118;
                if (TrackScripts(feedData, Script119)) return Script119;
                if (TrackScripts(feedData, Script120)) return Script120;
                if (TrackScripts(feedData, Script121)) return Script121;
                if (TrackScripts(feedData, Script122)) return Script122;
                if (TrackScripts(feedData, Script123)) return Script123;
                if (TrackScripts(feedData, Script124)) return Script124;
                if (TrackScripts(feedData, Script125)) return Script125;
                if (TrackScripts(feedData, Script126)) return Script126;
                if (TrackScripts(feedData, Script127)) return Script127;
                if (TrackScripts(feedData, Script128)) return Script128;
                if (TrackScripts(feedData, Script129)) return Script129;
                if (TrackScripts(feedData, Script130)) return Script130;
                if (TrackScripts(feedData, Script131)) return Script131;
                if (TrackScripts(feedData, Script132)) return Script132;
                if (TrackScripts(feedData, Script133)) return Script133;
                if (TrackScripts(feedData, Script134)) return Script134;
                if (TrackScripts(feedData, Script135)) return Script135;
                if (TrackScripts(feedData, Script136)) return Script136;
                if (TrackScripts(feedData, Script137)) return Script137;
                if (TrackScripts(feedData, Script138)) return Script138;
                if (TrackScripts(feedData, Script139)) return Script139;
                if (TrackScripts(feedData, Script140)) return Script140;
                if (TrackScripts(feedData, Script141)) return Script141;
                if (TrackScripts(feedData, Script142)) return Script142;
                if (TrackScripts(feedData, Script143)) return Script143;
                if (TrackScripts(feedData, Script144)) return Script144;
                if (TrackScripts(feedData, Script145)) return Script145;
                if (TrackScripts(feedData, Script146)) return Script146;
                //if (TrackScripts(feedData, Script147)) return Script123;


            }
            return new Nifty();
        }

        public bool TrackScripts(string feedData, Nifty trackScript)
        {
            if (feedData.Contains("ScripToken = " +trackScript.ScripCode + "|"))
            {
                trackScript.TradedCondition = "";
                string[] stockFeedData = feedData.Split('|');
                if ((!trackScript.PreviousLowCrossed) && (!trackScript.IsInHighPriceRadar && !trackScript.IsScriptShortCandidate)) CheckIfNearDaysLow(stockFeedData, trackScript);
                if ((!trackScript.PreviousHighCrossed) && (!trackScript.IsInLowPriceRadar && !trackScript.IsScriptBuyCandidate)) CheckIfNearDaysHigh(stockFeedData, trackScript);

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
            bool isPriceAbovePreviousHighAndLow = false;
            double ltPrice = GetLastTradedPrice(feedData);
            selectedScript.LastTradedPriceFromFeed = ltPrice;
            selectedScript.LastTradedTimeFromFeed = GetLastTradedDateTime(feedData).Trim().Substring(10);
            selectedScript.LastTradedVolumeFromFeed = GetVolumeFromFeed(feedData);
            double dayLowPrice = GetDayLowPriceFromFeedData(feedData);
            double dayHighPrice = GetDayHighPriceFromFeedData(feedData);
            double dayOpenPrice = GetDayOpenPriceFromFeedData(feedData);
            double previousLow = Convert.ToDouble(selectedScript.Low);
            double previousHigh = Convert.ToDouble(selectedScript.High);
            double lowPriceRadar = previousLow + ((previousLow * 0.5) / 100);

            //Price crosssed previous days high and low
            if (dayLowPrice < previousLow && dayHighPrice > previousHigh) isPriceAbovePreviousHighAndLow = true;
            //Stock price has gone below previous days low, mark this stock to be avoided for trade
            else if (dayLowPrice < previousLow)
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
                selectedScript.BuyRadarCounter++;
            }
            else if (previousLow <= dayLowPrice && dayLowPrice <= lowPriceRadar && ltPrice > lowPriceRadar && /*dayHighPrice <= (lowPriceRadar + lowPriceRadar * 0.01) && ltPrice <= (lowPriceRadar + lowPriceRadar * 0.002) &&*/ (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.IsScriptBuyCandidate = false; //this condition is to check if the last trade price is above radar price after being close to previous lows.
                selectedScript.TradedCondition = "near prev low and above radar and near days high";// + (previousLow + " <= " + dayLowPrice + " && " + dayLowPrice + "<=" + lowPriceRadar + " && " + ltPrice + ">" + lowPriceRadar + "&&" + dayHighPrice + "<=" + (lowPriceRadar + lowPriceRadar * 0.01) + "&&" + ltPrice + "<=" + (lowPriceRadar + lowPriceRadar * 0.002) + "&&" + (ltPrice + (ltPrice * 0.001) + ">" + dayHighPrice));
                selectedScript.BuyRadarCounter++;
            }

            //Day near low and open prices are same, best candidate for trade.
            if (previousLow <= dayLowPrice && (dayOpenPrice == dayLowPrice && ltPrice > dayOpenPrice) && (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptBuyCandidate = false;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.TradedCondition = "open & day low same price";
                selectedScript.BuyRadarCounter++;
            }
            //Below condition the prices have not gone very much below from the opening and it is trying to cover back and is above day open prices
            else if (previousLow <= dayLowPrice && (dayLowPrice >= (dayOpenPrice - (dayOpenPrice * 0.05)) && ltPrice >= dayOpenPrice) && (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {   
                isAnyConditionTrue = true;
                selectedScript.IsScriptBuyCandidate = false;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.TradedCondition = "above open price";
                if (((ltPrice + (ltPrice * 0.001)) >= dayOpenPrice) && ((dayOpenPrice + dayOpenPrice*0.001) > dayHighPrice))
                {
                    //SystemSounds.Exclamation.Play();
                    selectedScript.IsScriptBuyCandidate = true;
                    selectedScript.IsInLowPriceRadar = false;
                }
                else
                    selectedScript.BuyRadarCounter++;
            }
            //after all above conditions even if the stock is not radar, check if it was near previous days low price i.e. below radar price
            else if (previousLow <= dayLowPrice && dayLowPrice <= lowPriceRadar && ltPrice > lowPriceRadar && (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.IsScriptBuyCandidate = false; //this condition is to check if the last trade price is above radar price after being close to previous lows.
                selectedScript.TradedCondition = "was near previous low & above radar price now";// + (previousLow + " <= " + dayLowPrice + " && " + dayLowPrice + "<=" + lowPriceRadar + " && " + ltPrice + ">" + lowPriceRadar + "&&" + dayHighPrice + "<=" + (lowPriceRadar + lowPriceRadar * 0.01) + "&&" + ltPrice + "<=" + (lowPriceRadar + lowPriceRadar * 0.002) + "&&" + (ltPrice + (ltPrice * 0.001) + ">" + dayHighPrice));
                selectedScript.BuyRadarCounter++;
            }

            //Special condition
            if (isPriceAbovePreviousHighAndLow && ltPrice > dayOpenPrice && (ltPrice + (ltPrice * 0.001) > dayHighPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsInLowPriceRadar = true;
                selectedScript.IsScriptBuyCandidate = false; 
                selectedScript.TradedCondition = "crossed previous high low";
                selectedScript.BuyRadarCounter++;
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
            bool isPriceAbovePreviousHighAndLow = false;
            double ltPrice = GetLastTradedPrice(feedData);
            selectedScript.LastTradedPriceFromFeed = ltPrice;
            selectedScript.LastTradedTimeFromFeed = GetLastTradedDateTime(feedData).Trim().Substring(10);
            selectedScript.LastTradedVolumeFromFeed = GetVolumeFromFeed(feedData);
            double dayLowPrice = GetDayLowPriceFromFeedData(feedData);
            double dayHighPrice = GetDayHighPriceFromFeedData(feedData);
            double dayOpenPrice = GetDayOpenPriceFromFeedData(feedData);
            double previousHigh = Convert.ToDouble(selectedScript.High);
            double previousLow = Convert.ToDouble(selectedScript.Low);
            double highPriceRadar = previousHigh - ((previousHigh * 0.5) / 100);

            if (dayHighPrice > previousHigh && dayLowPrice < previousLow) isPriceAbovePreviousHighAndLow = true;
            //Stock price has gone below previous days low, mark this stock to be avoided for trade
            else if (dayHighPrice > previousHigh)
            {
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
                selectedScript.ShortRadarCounter++;
            }
            else if (previousHigh >= dayHighPrice && dayHighPrice >= highPriceRadar && ltPrice <= highPriceRadar && /* dayLowPrice >= (highPriceRadar - highPriceRadar * 0.01) && ltPrice >= (highPriceRadar - highPriceRadar * 0.002) && */ (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.TradedCondition = "was near previous high & below radar price now";//+ previousHigh +">="+ dayHighPrice +"&&"+ dayHighPrice +">="+ highPriceRadar +"&&"+ ltPrice +"<="+ highPriceRadar +"&&"+ dayLowPrice +">="+ (highPriceRadar - highPriceRadar * 0.01) +"&&"+ ltPrice +">="+ (highPriceRadar - highPriceRadar * 0.002) +"&&"+ (ltPrice - (ltPrice * 0.001) < dayLowPrice);
                selectedScript.ShortRadarCounter++;
            }

            //Day near low and open prices are same, best candidate for trade.
            if (previousHigh >= dayHighPrice && (dayOpenPrice == dayHighPrice && ltPrice < dayHighPrice) && (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.TradedCondition = "open & day high same price";
                selectedScript.ShortRadarCounter++;
            }
            else if (previousHigh >= dayHighPrice && ((dayHighPrice <= (dayOpenPrice + dayOpenPrice*0.05)) && ltPrice < dayHighPrice) && (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.TradedCondition = "below open price";
                if ((ltPrice - (ltPrice * 0.001)) <= dayOpenPrice && ((dayOpenPrice - dayOpenPrice * 0.001) < dayLowPrice))
                {
                    //SystemSounds.Exclamation.Play();
                    selectedScript.IsScriptShortCandidate = true;
                    selectedScript.IsInHighPriceRadar = false;
                }
                else selectedScript.ShortRadarCounter++;
            }
            //after all above conditions even if the stock is not radar, check if it was near previous days high price i.e. above radar price
            else if (previousHigh >= dayHighPrice && dayHighPrice >= highPriceRadar && ltPrice <= highPriceRadar && (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.TradedCondition = "near prev high and near days low below radar";//+ previousHigh +">="+ dayHighPrice +"&&"+ dayHighPrice +">="+ highPriceRadar +"&&"+ ltPrice +"<="+ highPriceRadar +"&&"+ dayLowPrice +">="+ (highPriceRadar - highPriceRadar * 0.01) +"&&"+ ltPrice +">="+ (highPriceRadar - highPriceRadar * 0.002) +"&&"+ (ltPrice - (ltPrice * 0.001) < dayLowPrice);
                selectedScript.ShortRadarCounter++;
            }

            //Special condition
            if (isPriceAbovePreviousHighAndLow && ltPrice < dayOpenPrice && (ltPrice - (ltPrice * 0.001) < dayLowPrice))
            {
                isAnyConditionTrue = true;
                selectedScript.IsScriptShortCandidate = false;
                selectedScript.IsInHighPriceRadar = true;
                selectedScript.TradedCondition = "crossed previous high low";
                selectedScript.ShortRadarCounter++;
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

        private int GetVolumeFromFeed(string[] feedData)
        {
            int ltVolume = 0;
            string[] stockFeedData = feedData;
            if (stockFeedData.Length > 0)
            {
                string[] feedDataVolume = stockFeedData[5].Split('=');
                if (feedDataVolume.Length > 0)
                {
                    ltVolume = Convert.ToInt32(feedDataVolume[1]);
                }
            }
            return ltVolume;
        }

        private async void AddToTrackingCollection(Nifty stockScript)
        {   

            if (stockScript.IsInHighPriceRadar)
            {
                if (!scriptsInShortRadar.ContainsKey(stockScript.ScripCode.Trim())) 
                { 
                    scriptsInShortRadar.Add(stockScript.ScripCode.Trim(), stockScript); 
                    ScriptsInShortRadarCollection.Add(stockScript); 
                    PostShortsRadarData(ScriptsInShortRadarCollection);
                    logger.LogInfoMessage(stockScript.LastTradedTimeFromFeed + ",ShortRadar," + stockScript.ScripName + "," + stockScript.LastTradedPriceFromFeed + "," + stockScript.LastTradedVolumeFromFeed.ToString() + "," + DateTime.Now.ToString("dd/MM/yyyy") + "," + stockScript.TradedCondition + "," + stockScript.ShortRadarCounter);
                }
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
                if (!scriptsInBuyRadar.ContainsKey(stockScript.ScripCode.Trim())) { 
                    scriptsInBuyRadar.Add(stockScript.ScripCode.Trim(), stockScript); 
                    ScriptsInBuyRadarCollection.Add(stockScript); 
                    PostBuyRadarData(ScriptsInBuyRadarCollection);
                    logger.LogInfoMessage(stockScript.LastTradedTimeFromFeed + ",BuyRadar," + stockScript.ScripName + "," + stockScript.LastTradedPriceFromFeed + "," + stockScript.LastTradedVolumeFromFeed.ToString() + "," + DateTime.Now.ToString("dd/MM/yyyy") + "," + stockScript.TradedCondition + "," + stockScript.BuyRadarCounter);

                };
                //else scriptsInBuyRadar[stockScript.ScripCode] = stockScript;
            }

            if (stockScript.IsScriptBuyCandidate)
            {
                if (!scriptsBuy.ContainsKey(stockScript.ScripCode.Trim())) 
                { 
                    scriptsBuy.Add(stockScript.ScripCode.Trim(), stockScript); 
                    ScriptsBuyCollection.Add(stockScript); 
                    PostBuyData(ScriptsBuyCollection);
                };
                // else scriptsBuy[stockScript.ScripCode] = stockScript;
            }

            if (stockScript.IsScriptShortCandidate)
            {
                if (!scriptsShort.ContainsKey(stockScript.ScripCode.Trim())) 
                { 
                    scriptsShort.Add(stockScript.ScripCode.Trim(), stockScript); 
                    ScriptsShortCollection.Add(stockScript); 
                    PostShortsData(ScriptsShortCollection);
                };
                //else scriptsShort[stockScript.ScripCode] = stockScript;
            }

            RemoveFromTracking(stockScript);
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
                else  if (!stockScript.IsInLowPriceRadar && scriptsInBuyRadar.ContainsKey(stockScript.ScripCode.Trim()))
                {
                    scriptsInBuyRadar.Remove(stockScript.ScripCode.Trim());
                    ScriptsInBuyRadarCollection.Remove(stockScript);
                    PostBuyRadarData(ScriptsInBuyRadarCollection);
                }
                else  if (!stockScript.IsScriptBuyCandidate && scriptsBuy.ContainsKey(stockScript.ScripCode.Trim()))
                {
                    scriptsBuy.Remove(stockScript.ScripCode.Trim());
                    ScriptsBuyCollection.Remove(stockScript);
                    PostBuyData(ScriptsBuyCollection);
                }
                else if (!stockScript.IsScriptShortCandidate && scriptsShort.ContainsKey(stockScript.ScripCode.Trim()))
                {
                    scriptsShort.Remove(stockScript.ScripCode.Trim());
                    ScriptsShortCollection.Remove(stockScript);
                    PostShortsData(ScriptsShortCollection);
                }

            }
        }

        public void ClearServerCache()
        {
            Scripts<Nifty> niftyList = new Scripts<Nifty>();
            PostShortsRadarData(niftyList);
            PostBuyRadarData(niftyList);
            PostBuyData(niftyList);
            PostShortsData(niftyList);
        }
    }
}
