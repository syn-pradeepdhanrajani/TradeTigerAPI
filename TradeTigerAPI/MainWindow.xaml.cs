using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using Microsoft.Win32;
using TradeTigerAPI.Business;
using System.Threading.Tasks;
using TradeTigerAPI.Utilities;
using System.Timers;
//using System.Data.Sql;


namespace TradeTigerAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Initialisation

        CTAPClient tapClient;
        Thread threadResponseReceived;
        Thread threadRequestSend;
        public static bool isSuccess = false;
        int count = 0;
        public static CQueue<string> logOutputQueue = new CQueue<string>();
        public static CQueue<string> requestSentQueue = new CQueue<string>();
        public static CQueue<string> excedptionQ = new CQueue<string>();

        static List<string> lstExchange = new List<string>();
        static List<string> lstFeedType = new List<string>();
        public static List<string> lstExpiry = new List<string>();
        public static List<string> lstInstrumnent = new List<string>();

        public List<string> lstFeedScrips = new List<string>();
        public static List<string> lstDepthScrips = new List<string>();
        public Dictionary<string, CStructures.OrderRequest> DicOrdersNamevOrderRequest = new Dictionary<string, CStructures.OrderRequest>();

        static List<string> lstMScripType = new List<string>();
        static List<string> lstNScripType = new List<string>();
        static List<string> lstBCScripType = new List<string>();
        static List<string> lstNXScripType = new List<string>();
        List<string> lstfeedOrOrder = new List<string>();

        public static Dictionary<int, CStructures.FeedResponse> DicFeedsRespone = new Dictionary<int, CStructures.FeedResponse>();
        public static Dictionary<int, CStructures.MarketDepthResponse> DicMarketDepthResponse = new Dictionary<int, CStructures.MarketDepthResponse>();
        public static Dictionary<string, CStructures.SharekhanOrderConfirmation> DicSharekhanOrderResponse = new Dictionary<string, CStructures.SharekhanOrderConfirmation>();

        public static Dictionary<string, CStructures.NFScripMaster> nfScripMaster = new Dictionary<string, CStructures.NFScripMaster>();
        public static Dictionary<string, CStructures.NCScripMaster> ncScripMaster = new Dictionary<string, CStructures.NCScripMaster>();
        public static Dictionary<string, CStructures.RNScripMaster> rnScripMaster = new Dictionary<string, CStructures.RNScripMaster>();
        public static Dictionary<string, CStructures.NCScripMaster> bcScripMaster = new Dictionary<string, CStructures.NCScripMaster>();
        public static Dictionary<string, CStructures.CommodityMaster> nxScripMaster = new Dictionary<string, CStructures.CommodityMaster>();
        public static Dictionary<string, CStructures.CommodityMaster> mxScripMaster = new Dictionary<string, CStructures.CommodityMaster>();
        public static Dictionary<string, CStructures.RNScripMaster> rmScripMaster = new Dictionary<string, CStructures.RNScripMaster>();

        #endregion

        public static int NiftyFUTCount = 0;
        public static int BankNiftyFUTCount = 0;
        public static int RelienceFUTCount = 0;
        public static int INFoFUTCount = 0;
        public static int TCSFUTCount = 0;

        public static Thread thProcessAmbiOrders = null;
        public static Thread thAmbiOrd = null;
        public static CQueue<orders> queueAmbi = new CQueue<orders>();
        public static CQueue<string> queueProcessAmbi = new CQueue<string>();

        public static int IncrementCounter = 0;
        public static bool OrderSendflag = false;

        public static string OrderXmlFileName { get; set; }
        public static string MainXMlFileName { get; set; }

        Stocks stk;
        List<Nifty> niftyList;
        Task<List<Nifty>> niftyStocks;
        ILogger logger = new Logger(typeof(MainWindow));
        Action socketTerminated;

        //System.Timers.Timer aTimer = new System.Timers.Timer();
        System.Timers.Timer bTimer = new System.Timers.Timer();
        DateTime lastReceivedPrice = DateTime.Now;
        //bool bTimerFlag = false; 

        //   (AppDomain.CurrentDomain.BaseDirectory + @"Log\TradeTigerAPI_" + DateTime.Now.ToString("ddMMyyyy") + ".Log", 256);//By Mahendar
        public MainWindow()
        {
            logger.LogInfoMessage("LastUpdated, Trend, Script, Price, Volume, LastUpdatedDate, TradedCondition, RadarCounter, Traded Amount, above 2 lacs");
            //aTimer.Elapsed += new ElapsedEventHandler(ReConnect);
            //aTimer.Interval = 5000;

            bTimer.Elapsed += new ElapsedEventHandler(DisconnectAndReconnect);
            bTimer.Interval = 60000;
            bTimer.Enabled = false;

            socketTerminated = new Action(ConnectionTerminated);
            stk = new Stocks(logger);
            //logger.LogError("Error while downloading data");
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            niftyStocks = stk.GetNiftyData();
            chkboxOrderFlag.IsChecked = true;
            txtLoginId.Text = ConfigurationSettings.AppSettings.Get("LoginID");
            txtMemberPassword.Password = ConfigurationSettings.AppSettings.Get("MembershipPWD");
            txtTradingPassword.Password = ConfigurationSettings.AppSettings.Get("TradingPWD");
            txtIP.Text = GetIPAddress();//ConfigurationSettings.AppSettings.Get("IP");

            tapClient = new CTAPClient(socketTerminated);
            threadResponseReceived = new Thread(new ThreadStart(ParseResponseQ));
            threadResponseReceived.IsBackground = true;
            threadResponseReceived.Start();

            threadRequestSend = new Thread(new ThreadStart(ParseSendRequestQ));
            threadRequestSend.IsBackground = true;
            threadRequestSend.Start();

            thProcessAmbiOrders = new Thread(new ThreadStart(sendorder));
            thProcessAmbiOrders.IsBackground = true;
            thProcessAmbiOrders.Start();

            thAmbiOrd = new Thread(new ThreadStart(ProcessAmbiOrdes));
            thAmbiOrd.IsBackground = true;
            thAmbiOrd.Start();

            lstExchange.Add("Select Exchange");
            lstExchange.Add("NSE");
            lstExchange.Add("BSE");
            lstExchange.Add("MCX");
            lstExchange.Add("NCDEX");
            cmbExchange.ItemsSource = lstExchange;

            lstNScripType.Add("Select ExchangeCode");
            lstNScripType.Add("NC");
            lstNScripType.Add("NF");
            lstNScripType.Add("RN");

            lstMScripType.Add("Select ExchangeCode");
            lstMScripType.Add("MX");
            lstMScripType.Add("RM");

            lstBCScripType.Add("Select ExchangeCode");
            lstBCScripType.Add("BC");

            lstNXScripType.Add("Select ExchangeCode");
            lstNXScripType.Add("NX");

            lstFeedType.Add("Select RequestType");
            lstFeedType.Add("Feed");
            lstFeedType.Add("Depth");
            cmbFeedOrDepth.ItemsSource = lstFeedType;

            lstfeedOrOrder.Add("Select");
            lstfeedOrOrder.Add("Get Feeds");
            lstfeedOrOrder.Add("Place Order");
            cmbFeedorOrder.ItemsSource = lstfeedOrOrder;

            cmbOrderReport.ItemsSource = Enum.GetValues(typeof(CStructures.OrderReport));// dicReportName;

            OrderXmlFileName = Directory.GetCurrentDirectory() + "\\ORDERS\\" + DateTime.Now.ToString("dd-MM-yyyy") + "Order.xml";
            MainXMlFileName = Directory.GetCurrentDirectory() + "\\ORDERS\\" + "MainXML.xml";


            LoadDictionary(OrderXmlFileName, MainXMlFileName);

            if (File.Exists(MainXMlFileName))
            {
                XDocument xdocument = XDocument.Load(MainXMlFileName);
                var orderdatalist = (from order in xdocument.Descendants("Table1")
                                     select order);
                foreach (System.Xml.Linq.XElement orderdata in xdocument.Descendants("Table1"))
                {
                    if (orderdata.Element("SCRIPTINFO").Value != "0")
                    {
                        StockwiseInfo obj = new StockwiseInfo();
                        obj.ScriptInfo = orderdata.Element("SCRIPTINFO").Value;
                        //     obj. = orderdata.Element("API_ID").Value;

                        // if (orderdata.Element("CurrentSIGNAL").Value == "B")
                        obj.LastSignal = orderdata.Element("CurrentSIGNAL").Value;
                        //else
                        //   obj.LastSignal = "SELL";

                        obj.NetPosition = Convert.ToInt16(orderdata.Element("CurrentNETPOSITION").Value);
                        dicStockinfoVsOrderDetail.Add(obj.ScriptInfo, obj);
                    }
                }
                //  IncrementCounter = Convert.ToInt16(orderdatalist.FirstOrDefault().Element("API_ID").Value) + 5;
            }
            else
            {

                CreateMainTable(MainXMlFileName);

            }

            if (!File.Exists(OrderXmlFileName))
            {
                CreateOrderTable(OrderXmlFileName);
            }




            RegistryKey registryKey = Registry.CurrentUser;
            RegistryKey r1 = registryKey.OpenSubKey("Software");
            RegistryKey r2 = r1.OpenSubKey("MICROSOFT");

            RegistryKey r3 = r2.OpenSubKey("mwor");
            if (r3 != null)
                Read(r3);

            RegistryKey reg = Registry.LocalMachine;
            RegistryKey r4 = reg.OpenSubKey("Software");
            RegistryKey r5 = r4.OpenSubKey("MICROSOFT");

            RegistryKey r6 = r5.OpenSubKey("mwor");
            if (r6 != null)
                Read(r6);

        }

        public string GetIPAddress()
        {
            string IPAddress="";

            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;

        }

        public void ConnectionTerminated()
        {
            //aTimer.Enabled = true;
        }

        private void ReConnect(object source, ElapsedEventArgs e)
        {
            tapClient = new CTAPClient(socketTerminated);
            CConstants.LoginId = ConfigurationSettings.AppSettings.Get("LoginID");
            CConstants.MemberPassword = ConfigurationSettings.AppSettings.Get("MembershipPWD");
            CConstants.TradingPassword = ConfigurationSettings.AppSettings.Get("TradingPWD");
            CConstants.TapIp = GetIPAddress();
            tapClient.Connect(); // connect to trade tiger

            tapClient.SendLoginRequest(); // To login authentication     

            //aTimer.Enabled = false;
        }

        private void DisconnectAndReconnect(object source, ElapsedEventArgs e)
        {
            if (lastReceivedPrice.AddMinutes(5) > DateTime.Now) return;
            try
            {
                bTimer.Enabled = false;
                tapClient.Disconnect();
                tapClient = null;
                ReConnect(null,null);
            }
            catch { }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public static void Read(RegistryKey root)
        {
            try
            {


                foreach (var child in root.GetSubKeyNames())
                {
                    using (var childKey = root.OpenSubKey(child))
                    {
                        Read(childKey);
                    }
                }

                foreach (var value in root.GetValueNames())
                {

                    string log = " Key :" + string.Format("{0}\\{1}", root, value) + " Type : " + root.GetValueKind(value) + "  value: " + (root.GetValue(value) ?? "").ToString();
                    LogFile.Reference.WriteLogFile("laod last in  : ", log);
                }
            }
            catch (Exception)
            {


            }
        }

        public void LoadDictionary(string orderxmlfile, string mainxmlfile)
        {
            try
            {
                if (File.Exists(orderxmlfile))
                {
                    using (XmlReader xmlFile = XmlReader.Create(orderxmlfile, new XmlReaderSettings()))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            ds.ReadXml(orderxmlfile);

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                orders obj = new orders();
                                obj.APIReqID = row["API_ID"].ToString();
                                obj.ScriptName = row["SCRIPT"].ToString();
                                obj.open = row["SIGNAL"].ToString();
                                obj.datetime = row["OPEN"].ToString();
                                obj.datetime = row["DATETIME"].ToString();
                                obj.FormulaName = row["FORMULA"].ToString();
                                obj.SettingType = row["SETTING"].ToString();
                                obj.NetPosition = Convert.ToInt32(row["NETPOSITION"].ToString());
                                obj.OrdDateTime = row["OrdDATETIME"].ToString();
                                obj.Quantity = Convert.ToInt32(row["QUANTITY"].ToString());
                                obj.ExcPrice = Convert.ToInt32(row["ExcPrice"].ToString());
                                obj.ExcQty = Convert.ToInt32(row["ExcQUANTITY"].ToString());
                                obj.ExcDateTime = row["ExcDATETIME"].ToString();

                                dicReqIDTrack.Add(obj.APIReqID, obj);
                            }

                        }
                    }
                }

                if (File.Exists(mainxmlfile))
                {
                    using (XmlReader xmlFile = XmlReader.Create(mainxmlfile, new XmlReaderSettings()))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            ds.ReadXml(mainxmlfile);

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                MainOrderPosition obj = new MainOrderPosition();
                                obj.SCRIPTINFO = row["SCRIPTINFO"].ToString();
                                obj.API_ID = Convert.ToInt32(row["API_ID"].ToString());
                                obj.SCRIPT = row["SCRIPT"].ToString();
                                obj.OPEN = row["OPEN"].ToString();
                                obj.DATETIME = row["DATETIME"].ToString();
                                obj.FORMULA = row["FORMULA"].ToString();
                                obj.LastSIGNAL = row["LastSIGNAL"].ToString();
                                obj.CurrentSIGNAL = row["CurrentSIGNAL"].ToString();
                                obj.SETTING = row["SETTING"].ToString();
                                obj.LastNETPOSITION = Convert.ToInt32(row["LastNETPOSITION"].ToString());
                                obj.CurrentNETPOSITION = Convert.ToInt32(row["CurrentNETPOSITION"].ToString());
                                obj.BuyAvgPrice = Convert.ToDouble(row["BuyAvgPrice"].ToString());
                                obj.SellAvgPrice = Convert.ToDouble(row["SellAvgPrice"].ToString());
                                obj.TotalBuyQTY = Convert.ToInt32(row["TotalBuyQTY"].ToString());
                                obj.TotalSellQTY = Convert.ToInt32(row["TotalSellQTY"].ToString());
                                obj.BuyTotalPrice = Convert.ToDouble(row["BuyTotalPrice"].ToString());
                                obj.SellTotalPrice = Convert.ToDouble(row["SellTotalPrice"].ToString());
                                obj.MainPNL = Convert.ToDouble(row["MainPNL"].ToString());
                                obj.XMLFILENAME = row["XMLFILENAME"].ToString();

                                dicMainPosition.Add(obj.SCRIPTINFO, obj);
                            }

                        }
                    }
                }

                if (dicMainPosition.Count > 0)
                {
                    var dicmain = dicMainPosition.Where(p => p.Key != "0");
                    if (dicmain.Count() > 0)
                    {
                        IncrementCounter = dicMainPosition[dicmain.FirstOrDefault().Key].API_ID + 1;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region Thread To log response and request
        public void ParseResponseQ()
        {
            try
            {
                while (true)
                {
                    string outputtext = logOutputQueue.DeQueue(true);
                    lastReceivedPrice = DateTime.Now;
                    bool IsOrderResponse = CheckFeedDataIsOrderResponse(outputtext.Replace("\0", "")); //Check type of response received : Order Response or Feed Data
                    DicFeedsRespone.Clear();
                    if (IsOrderResponse) continue;
                    Nifty scriptTracked = stk.TrackScripts(outputtext.Replace("\0", ""));
                    txtRecieved.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
                                 {   
                                     if (txtRecieved.Text.Length > 1000)
                                         txtRecieved.Text = "";

                                     if (txtSend.Text.Length > 1000)
                                         txtSend.Text = "";

                                     if (scriptTracked.IsInLowPriceRadar)
                                         txtRecieved.Text = string.Format("Buy Radar -" + scriptTracked.ScripName + "=>" + DateTime.Now.ToShortTimeString() + "\n\n" + txtRecieved.Text);

                                     if (scriptTracked.IsScriptBuyCandidate)
                                         txtRecieved.Text = string.Format("Buy -" + scriptTracked.ScripName + "=>" + DateTime.Now.ToShortTimeString() + "\n" + txtRecieved.Text);

                                     if (scriptTracked.IsInHighPriceRadar)
                                         txtSend.Text = string.Format("Short Radar - " + scriptTracked.ScripName + "=>" + DateTime.Now.ToShortTimeString() + "\n\n" + txtSend.Text);

                                     if (scriptTracked.IsScriptShortCandidate)
                                         txtSend.Text = string.Format("Short -" + scriptTracked.ScripName + "=>" + DateTime.Now.ToShortTimeString() + "\n\n" + txtSend.Text);

                                     //txtRecieved.Text = string.Format(outputtext.Replace("\0", "") + "\n" + txtRecieved.Text);
                                     //if (!bTimer.Enabled) bTimer.Enabled = true;
                                     lastReceivedPrice = DateTime.Now;
                                     stockTrackingForm.RefreshData();
                                     //if (scriptTracked != null && !string.IsNullOrEmpty(scriptTracked.ScripCode) && !scriptTracked.IsOrderPlaced && scriptTracked.IsScriptBuyCandidate && scriptTracked.LastTradedPriceFromFeed > 0)
                                     //{
                                     //    PlaceOrder(scriptTracked, "BM");
                                     //    scriptTracked.IsOrderPlaced = true;
                                     //}

                                     //if (scriptTracked != null && !string.IsNullOrEmpty(scriptTracked.ScripCode) && !scriptTracked.IsOrderPlaced && scriptTracked.IsScriptShortCandidate && scriptTracked.LastTradedPriceFromFeed > 0)
                                     //{
                                     //    PlaceOrder(scriptTracked, "SM");
                                     //    scriptTracked.IsOrderPlaced = true;
                                     //}


                                 });



                    if (isSuccess == true)
                    {
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
                    {

                        tbcontrolMain.SelectedIndex = 1;
                        btnLogoff.Visibility = Visibility.Visible;
                        btnScripMasterDownload.Visibility = Visibility.Visible;
                        tabFeedRequest.Visibility = Visibility.Visible;
                        tabOrderReport.Visibility = Visibility.Visible;
                        tabLogin.Visibility = Visibility.Collapsed;
                        isSuccess = false;
                        niftyList = niftyStocks.Result;
                        stk.InitializeStockScripts(niftyList);
                        
                        RegisterStocksForFeeds();                        
                        stockTrackingForm.InitializeUI(stk);
                        bTimer.Enabled = true;
                        lastReceivedPrice = DateTime.Now;

                    });
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        private bool CheckFeedDataIsOrderResponse(string feedData)
        {
            if (feedData.Contains("SharekhanOrderID = "))
            {
                return true;
            }
            return false;
        }

        public void ParseSendRequestQ()
        {
            try
            {
                while (true)
                {
                    string requestQ = requestSentQueue.DeQueue(true);
                    txtRecieved.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
                    {
                        txtSend.Text = string.Format(requestQ.Replace("\0", "") + "\n\n" + txtSend.Text);
                    });
                }
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        #endregion

        /// <summary>
        /// Login Code which takes user inputs and do authentication process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtLoginId.Text != "" && txtMemberPassword.Password != "" && txtTradingPassword.Password != "")
                {
                    btnSync_Click(null, null);
                    CConstants.LoginId = txtLoginId.Text;
                    CConstants.MemberPassword = txtMemberPassword.Password;
                    CConstants.TradingPassword = txtTradingPassword.Password;
                    //CConstants.TapIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(); // To get host address
                    CConstants.TapIp = txtIP.Text;
                    tapClient.Connect(); // connect to trade tiger

                    if (tapClient.SendLoginRequest()) // To login authentication   
                    {
                        stockTrackingForm = new StockScriptsTracking();
                        stockTrackingForm.Show();
                    }              

                }
                else
                {
                    //MessageBox.Show("Please provide complete details...");
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public void Authenticated(bool isSuccess)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (System.Threading.ThreadStart)delegate
                                 {
                                     if (isSuccess == true)
                                     {
                                         tbcontrolMain.SelectedIndex = 1;
                                         btnSync.Visibility = Visibility.Visible;
                                         btnLogoff.Visibility = Visibility.Visible;
                                         btnScripMasterDownload.Visibility = Visibility.Visible;
                                         tabFeedRequest.Visibility = Visibility.Visible;
                                         tabOrderReport.Visibility = Visibility.Visible;
                                         tabLogin.Visibility = Visibility.Collapsed;
                                         RegisterStocksForFeeds();

                                     }
                                 });
        }

        /// <summary>
        /// To log off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogoff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isLogoff = tapClient.SendLogOffRequest();
                if (isLogoff == true)
                {
                    logOutputQueue.EnQueue("LogOff done successfully");
                    tabLogin.Visibility = Visibility.Visible;
                    btnLogoff.Visibility = Visibility.Collapsed;
                    btnScripMasterDownload.Visibility = Visibility.Collapsed;
                    tabFeedRequest.Visibility = Visibility.Collapsed;
                    tabOrderReport.Visibility = Visibility.Collapsed;
                    tbcontrolMain.TabIndex = 1;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Downloads the NC, NF, RN, BC, NX, MX, RM scripmasters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScripMasterDownload_Click(object sender, RoutedEventArgs e)
        {


            tapClient.SendScripMasterDownload();
            StartRead = true;
        }

        /// <summary>
        /// On selecting any exchange it will fetch respective Echangecodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbExchange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbExchange.SelectedValue != null)
            {
                cmbScripType.ItemsSource = null;
                switch (cmbExchange.SelectedValue.ToString())
                {
                    case "NSE":
                        cmbScripType.ItemsSource = lstNScripType;
                        break;
                    case "BSE":
                        cmbScripType.ItemsSource = lstBCScripType;
                        break;
                    case "MCX":
                        cmbScripType.ItemsSource = lstMScripType;
                        break;
                    case "NCDEX":
                        cmbScripType.ItemsSource = lstNXScripType;
                        break;
                }
            }

        }

        public static int GetCount()
        {
            return Interlocked.Increment(ref IncrementCounter);
        }

        #region Stores Scripmasters in respective dictionaries

        public static void LoadScripmaster(object scripmaster, string exchangeCode)
        {
            if (scripmaster as Dictionary<string, CStructures.NCScripMaster> != null && exchangeCode == CConstants.NCExcode)
            {
                ncScripMaster = scripmaster as Dictionary<string, CStructures.NCScripMaster>;
            }
            else if (scripmaster as Dictionary<string, CStructures.NFScripMaster> != null)
            {
                nfScripMaster = scripmaster as Dictionary<string, CStructures.NFScripMaster>;
            }
            else if (scripmaster as Dictionary<string, CStructures.RNScripMaster> != null && exchangeCode == CConstants.RNExCode)
            {
                rnScripMaster = scripmaster as Dictionary<string, CStructures.RNScripMaster>;
            }
            else if (scripmaster as Dictionary<string, CStructures.RNScripMaster> != null && exchangeCode == CConstants.RMExcode)
            {
                rmScripMaster = scripmaster as Dictionary<string, CStructures.RNScripMaster>;
            }
            else if (scripmaster as Dictionary<string, CStructures.NCScripMaster> != null && exchangeCode == CConstants.BCExcode)
            {
                bcScripMaster = scripmaster as Dictionary<string, CStructures.NCScripMaster>;
            }
            else if (scripmaster as Dictionary<string, CStructures.CommodityMaster> != null && exchangeCode == CConstants.NXExcode)
            {
                nxScripMaster = scripmaster as Dictionary<string, CStructures.CommodityMaster>;
            }
            else if (scripmaster as Dictionary<string, CStructures.CommodityMaster> != null && exchangeCode == CConstants.MXExcode)
            {
                mxScripMaster = scripmaster as Dictionary<string, CStructures.CommodityMaster>;
            }

        }

        #endregion

        /// <summary>
        /// delete scrip from feeds list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_feed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lbfeedRequest.SelectedIndex == -1) return;

                string element = lbfeedRequest.SelectedValue.ToString();

                if (lstFeedScrips.Contains(element))
                {
                    lstFeedScrips.Remove(element);
                    logOutputQueue.EnQueue(element + " removed");
                }
                lbfeedRequest.ItemsSource = null;
                lbfeedRequest.ItemsSource = lstFeedScrips;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// delete scrip from depthlst
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_depth_Click(object sender, RoutedEventArgs e)
        {
            if (lbMarketDepth.SelectedIndex == -1) return;

            string element = lbMarketDepth.SelectedValue.ToString();

            if (lstDepthScrips.Contains(element))
            {
                lstDepthScrips.Remove(element);
                logOutputQueue.EnQueue(element + " removed");
            }
            lbMarketDepth.ItemsSource = null;
            lbMarketDepth.ItemsSource = lstDepthScrips;
        }

        /// <summary>
        /// On selecting exchange code it will fetch scrips in scrips list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbScripType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            try
            {
                if (cmbScripType.SelectedValue != null)
                {
                    cmbScrips.ItemsSource = null;
                    cmbExpiry.ItemsSource = null;
                    cmbInstrument.ItemsSource = null;
                    switch (cmbExchange.SelectedValue.ToString())
                    {
                        case "NSE":
                            switch (cmbScripType.SelectedValue.ToString())
                            {
                                case "NC":
                                    if (ncScripMaster != null)
                                        cmbScrips.ItemsSource = niftyList;//ncScripMaster.Values.Select(s => s.Prop04ScripShortName.ToString().Replace("\0", "")).Distinct().ToList();
                                    cmbExpiry.Visibility = Visibility.Collapsed;
                                    cmbInstrument.Visibility = Visibility.Collapsed;
                                    cmbStrike.Visibility = Visibility.Collapsed;
                                    lblExpiry.Visibility = Visibility.Collapsed;
                                    lblInstrument.Visibility = Visibility.Collapsed;
                                    lblStrike.Visibility = Visibility.Collapsed;
                                    break;
                                case "NF":
                                    if (nfScripMaster != null)
                                        cmbScrips.ItemsSource = nfScripMaster.Values.Select(s => s.Prop04ScripShortName.Substring(0, s.Prop04ScripShortName.IndexOf("\0")).Replace("\0", "")).Distinct().ToList();
                                    cmbExpiry.Visibility = Visibility.Visible;
                                    cmbInstrument.Visibility = Visibility.Visible;
                                    cmbStrike.Visibility = Visibility.Visible;
                                    cmbExpiry.ItemsSource = lstExpiry;
                                    cmbInstrument.ItemsSource = lstInstrumnent;
                                    lblExpiry.Visibility = Visibility.Visible;
                                    lblInstrument.Visibility = Visibility.Visible;
                                    lblStrike.Visibility = Visibility.Visible;
                                    break;
                                case "RN":
                                    if (rnScripMaster != null)
                                    {
                                        cmbScrips.ItemsSource = rnScripMaster.Select(s => s.Key.Substring(0, s.Key.IndexOf("\0")).Replace("\0", "")).Distinct().ToList();
                                        cmbExpiry.ItemsSource = rnScripMaster.Values.Select(s => s.Prop05ExpiryDate.Replace("\0", "")).Distinct().ToList();
                                        cmbInstrument.ItemsSource = rnScripMaster.Values.Select(s => s.Prop06FutOption.ToString().Replace("\0", "")).Distinct().ToList();
                                    }
                                    cmbExpiry.Visibility = Visibility.Visible;
                                    cmbInstrument.Visibility = Visibility.Visible;
                                    cmbStrike.Visibility = Visibility.Visible;
                                    lblExpiry.Visibility = Visibility.Visible;
                                    lblInstrument.Visibility = Visibility.Visible;
                                    lblStrike.Visibility = Visibility.Visible;
                                    break;
                            }
                            break;
                        case "BSE":
                            if (CTAPClient.dicBcScripMaster != null)
                                cmbScrips.ItemsSource = CTAPClient.dicBcScripMaster.Values.Select(s => s.Prop04ScripShortName.ToString().Replace("\0", "")).Distinct().ToList();
                            cmbExpiry.Visibility = Visibility.Collapsed;
                            cmbInstrument.Visibility = Visibility.Collapsed;
                            cmbStrike.Visibility = Visibility.Collapsed;
                            lblExpiry.Visibility = Visibility.Collapsed;
                            lblInstrument.Visibility = Visibility.Collapsed;
                            lblStrike.Visibility = Visibility.Collapsed;
                            break;
                        case "MCX":
                            switch (cmbScripType.SelectedValue.ToString())
                            {
                                case "MX":
                                    if (mxScripMaster != null)
                                        cmbScrips.ItemsSource = mxScripMaster.Values.Select(s => s.Prop03ScripShortName.ToString().Replace("\0", "")).Distinct().ToList();
                                    cmbExpiry.Visibility = Visibility.Collapsed;
                                    cmbInstrument.Visibility = Visibility.Collapsed;
                                    cmbStrike.Visibility = Visibility.Collapsed;
                                    lblExpiry.Visibility = Visibility.Collapsed;
                                    lblInstrument.Visibility = Visibility.Collapsed;
                                    lblStrike.Visibility = Visibility.Collapsed;
                                    break;
                                case "RM":
                                    if (rmScripMaster != null)
                                        cmbScrips.ItemsSource = rmScripMaster.Values.Select(s => s.Prop04ScripShortName.ToString().Replace("\0", "")).Distinct().ToList();
                                    cmbExpiry.Visibility = Visibility.Collapsed;
                                    cmbInstrument.Visibility = Visibility.Collapsed;
                                    cmbStrike.Visibility = Visibility.Collapsed;
                                    lblExpiry.Visibility = Visibility.Collapsed;
                                    lblInstrument.Visibility = Visibility.Collapsed;
                                    lblStrike.Visibility = Visibility.Collapsed;
                                    break;
                            }
                            break;
                        case "NCDEX":
                            if (nxScripMaster != null)
                                cmbScrips.ItemsSource = nxScripMaster.Values.Select(s => s.Prop03ScripShortName.ToString().Replace("\0", "")).Distinct().ToList();

                            cmbExpiry.Visibility = Visibility.Collapsed;
                            cmbInstrument.Visibility = Visibility.Collapsed;
                            cmbStrike.Visibility = Visibility.Collapsed;
                            lblExpiry.Visibility = Visibility.Collapsed;
                            lblInstrument.Visibility = Visibility.Collapsed;
                            lblStrike.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// on selecting scrip it will fetch its strike prices in strike combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbScrips_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbScrips.SelectedValue != null && cmbScripType.SelectedValue != null)
                {
                    cmbStrike.ItemsSource = null;
                    switch (cmbScripType.SelectedValue.ToString())
                    {
                        case "NF":
                            if (nfScripMaster.Count > 0)
                            {
                                cmbStrike.ItemsSource = nfScripMaster.Values.OrderBy(o => o.Prop07StrikePrice).Where(s => s.Prop04ScripShortName.Substring(0, s.Prop04ScripShortName.IndexOf("\0")) == cmbScrips.SelectedValue.ToString()).Select(s => s.Prop07StrikePrice / 100).Distinct().ToList();
                                cmbExpiry.ItemsSource = nfScripMaster.Values.Select(s => s.Prop05ExpiryDate.ToString().Replace("\0", "")).Distinct().ToList();
                                cmbInstrument.ItemsSource = nfScripMaster.Values.Select(s => s.Prop06FutOption.ToString().Replace("\0", "")).Distinct().ToList();

                            }

                            break;
                        case "RN":
                            if (rnScripMaster.Count > 0)
                            {
                                cmbStrike.ItemsSource = rnScripMaster.Values.OrderBy(o => o.Prop07StrikePrice).Where(s => s.Prop04ScripShortName.Substring(0, s.Prop04ScripShortName.IndexOf("\0")) == cmbScrips.SelectedValue.ToString()).Select(s => Convert.ToDouble(s.Prop07StrikePrice) / 100).Distinct().ToList();
                                cmbExpiry.ItemsSource = rnScripMaster.Values.Select(s => s.Prop05ExpiryDate.ToString().Replace("\0", "")).Distinct().ToList();
                                cmbInstrument.ItemsSource = rnScripMaster.Values.Select(s => s.Prop06FutOption.ToString().Replace("\0", "")).Distinct().ToList();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// select feed or depth to request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFeedorOrder.SelectedValue != null)
            {
                if (cmbFeedorOrder.SelectedValue.ToString() == "Get Feeds")
                {
                    if (cmbExchange.SelectedValue != null && cmbFeedOrDepth.SelectedValue != null && cmbScripType.SelectedValue != null && cmbScrips.SelectedValue != null)
                    {
                        try
                        {
                            Nifty nfstock = cmbScrips.SelectedValue as Nifty;
                            CStructures.MessageHeader header;
                            string scripName = string.Empty;
                            switch (cmbFeedOrDepth.SelectedValue.ToString())
                            {
                                case "Feed":
                                    header = new CStructures.MessageHeader(1);
                                    header.Prop01MessageLength = CConstants.FeedRequestSize;
                                    header.Prop02TransactionCode = Convert.ToInt16(CConstants.TranCode.FeedRequest);
                                    CStructures.FeedRequest feedreq = new CStructures.FeedRequest(true);
                                    feedreq.Prop01Header = header;
                                    feedreq.Prop02Count = 1;

                                    switch (cmbScripType.SelectedValue.ToString())
                                    {
                                        case "NC":
                                            feedreq.Prop03ScripList = cmbScripType.SelectedValue.ToString() + nfstock.ScripCode;// cmbScripType.SelectedValue.ToString() + ncScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                                            break;
                                        case "BC":
                                            feedreq.Prop03ScripList = cmbScripType.SelectedValue.ToString() + bcScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                                            break;
                                        case "NF":
                                            if (cmbInstrument.SelectedValue.ToString() != "FUT")
                                                scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                                            else
                                                scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString();

                                            feedreq.Prop03ScripList = cmbScripType.SelectedValue.ToString() + nfScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "").Trim() == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();

                                            break;
                                        case "RN":
                                            scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                                            feedreq.Prop03ScripList = cmbScripType.SelectedValue.ToString() + rnScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                                            break;
                                        case "MX":
                                            feedreq.Prop03ScripList = cmbScripType.SelectedValue.ToString() + mxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                                            break;
                                        case "RM":
                                            feedreq.Prop03ScripList = cmbScripType.SelectedValue.ToString() + rmScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                                            break;
                                        case "NX":
                                            feedreq.Prop03ScripList = cmbScripType.SelectedValue.ToString() + nxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                                            break;
                                    }


                                    requestSentQueue.EnQueue(feedreq.ToString());
                                    tapClient.SubscribeforFeeds(feedreq.StructToByte());


                                    if (scripName.Equals(string.Empty))
                                        scripName = nfstock.ScripName;

                                    if (!lstFeedScrips.Contains(scripName))
                                        lstFeedScrips.Add(scripName);
                                    lbfeedRequest.ItemsSource = null;
                                    lbfeedRequest.ItemsSource = lstFeedScrips;
                                    break;
                                case "Depth":
                                    header = new CStructures.MessageHeader(1);
                                    header.Prop01MessageLength = CConstants.MarketDepthRequestSize;
                                    header.Prop02TransactionCode = Convert.ToInt16(CConstants.TranCode.DepthRequest);
                                    CStructures.MarketDepthRequest depthRequest = new CStructures.MarketDepthRequest(true);
                                    depthRequest.Prop01Header = header;

                                    switch (cmbScripType.SelectedValue.ToString())
                                    {
                                        case "NC":
                                            depthRequest.Prop03ScripCode = nfstock.ScripCode;
                                            break;
                                        case "BC":
                                            depthRequest.Prop03ScripCode = bcScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                            break;
                                        case "NF":
                                            if (cmbInstrument.SelectedValue.ToString() != "FUT")
                                                scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                                            else
                                                scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString();

                                            depthRequest.Prop03ScripCode = nfScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "").Trim() == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                                            break;
                                        case "RN":
                                            scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                                            depthRequest.Prop03ScripCode = rnScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                            break;
                                        case "MX":
                                            depthRequest.Prop03ScripCode = mxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                                            break;
                                        case "RM":
                                            depthRequest.Prop03ScripCode = rmScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                            break;
                                        case "NX":
                                            depthRequest.Prop03ScripCode = nxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                                            break;
                                    }

                                    depthRequest.Prop02ExchangeCode = cmbScripType.SelectedValue.ToString();

                                    requestSentQueue.EnQueue(depthRequest.ToString());
                                    tapClient.SubscribeforFeeds(depthRequest.StructToByte());

                                    if (scripName.Equals(string.Empty))
                                        scripName = cmbScrips.SelectedValue.ToString();

                                    if (!lstDepthScrips.Contains(scripName))
                                        lstDepthScrips.Add(scripName);
                                    lbMarketDepth.ItemsSource = null;
                                    lbMarketDepth.ItemsSource = lstDepthScrips;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        txtPrice.Visibility = Visibility.Collapsed;
                        txtQty.Visibility = Visibility.Collapsed;
                        btnPlaceOrder.Visibility = Visibility.Collapsed;
                        lblQty.Visibility = Visibility.Collapsed;
                        lblPrice.Visibility = Visibility.Collapsed;
                        cmbFeedorOrder.SelectedIndex = 0;
                    }
                }
                else
                {
                    txtPrice.Visibility = Visibility.Visible;
                    txtQty.Visibility = Visibility.Visible;
                    btnPlaceOrder.Visibility = Visibility.Visible;
                    lblQty.Visibility = Visibility.Visible;
                    lblPrice.Visibility = Visibility.Visible;
                }
            }
        }


        public void RegisterStocksForFeeds()
        {
            int icount = 1;
            foreach (Nifty nfstk in niftyList)
            {
                try
                {
                    CStructures.MessageHeader header;
                    string scripName = string.Empty;
                    header = new CStructures.MessageHeader(1);
                    header.Prop01MessageLength = CConstants.FeedRequestSize;
                    header.Prop02TransactionCode = Convert.ToInt16(CConstants.TranCode.FeedRequest);
                    CStructures.FeedRequest feedreq = new CStructures.FeedRequest(true);
                    feedreq.Prop01Header = header;
                    feedreq.Prop02Count = 1;

                    feedreq.Prop03ScripList = "NC" + nfstk.ScripCode;// cmbScripType.SelectedValue.ToString() + ncScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().Replace("\0", "").ToString();
                    requestSentQueue.EnQueue(feedreq.ToString());
                    tapClient.SubscribeforFeeds(feedreq.StructToByte());

                    scripName = nfstk.ScripName;

                    if (!lstFeedScrips.Contains(scripName))
                        lstFeedScrips.Add(scripName);
                    lbfeedRequest.ItemsSource = lstFeedScrips;

                    //if (icount > 0) break;
                    icount++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// click to place order after providing price and quantity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (cmbExchange.SelectedIndex != -1 && cmbScripType.SelectedValue != null && txtPrice.Text != string.Empty && txtQty.Text != string.Empty)
            {
                CStructures.MessageHeader header = new CStructures.MessageHeader(1);
                header.Prop01MessageLength = CConstants.OrderRequestSize; ;
                header.Prop02TransactionCode = Convert.ToInt16(CConstants.TranCode.OrderRequest);

                CStructures.OrderItem orderItem = new CStructures.OrderItem(true);
                orderItem.Prop01DataLength = CConstants.OrderItemSize;
                orderItem.Prop02OrderID = "";
                string scripName = string.Empty;

                switch (cmbScripType.SelectedValue.ToString())
                {
                    case "NC":
                        orderItem.Prop05ScripToken = ncScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "BC":
                        orderItem.Prop05ScripToken = bcScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "NF":
                        scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                        if (cmbInstrument.SelectedValue.ToString() == "FUT")
                            scripName = scripName.Substring(0, scripName.Length - 4);

                        var aa = nfScripMaster.Where(p => p.Value.Prop05ExpiryDate.Replace("\0", "") == cmbExpiry.SelectedValue.ToString() && p.Value.Prop02DerivativeType.Replace("\0", "") == "FI");
                        orderItem.Prop05ScripToken = nfScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "RN":
                        scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                        orderItem.Prop05ScripToken = rnScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "MX":
                        orderItem.Prop05ScripToken = mxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "RM":
                        orderItem.Prop05ScripToken = rmScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "NX":
                        orderItem.Prop05ScripToken = nxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                }

                orderItem.Prop03CustomerID = "103956";
                orderItem.Prop04S2KID = "";
                orderItem.Prop06BuySell = "B";
                orderItem.Prop07OrderQty = Convert.ToInt32(txtQty.Text);
                orderItem.Prop08OrderPrice = Convert.ToInt32(txtPrice.Text);
                orderItem.Prop09TriggerPrice = 0;
                orderItem.Prop10DisclosedQty = 0;
                orderItem.Prop11ExecutedQty = 0;
                orderItem.Prop12RMSCode = "";
                orderItem.Prop13ExecutedPrice = 0;
                orderItem.Prop14AfterHour = "N";
                orderItem.Prop15GTDFlag = "IOC"; //"GFD";
                orderItem.Prop16GTD = "";
                orderItem.Prop17Reserved = "";


                int AutoInc = GetCount();
                CStructures.OrderRequest orderRequest = new CStructures.OrderRequest(true);
                orderRequest.Prop01Header = header;

                orderRequest.Prop03OrderCount = Convert.ToInt16(AutoInc);
                orderRequest.Prop04ExchangeCode = cmbScripType.SelectedValue.ToString();
                orderRequest.Prop05OrderType1 = "NEW";
                orderRequest.Prop02RequestID = AutoInc.ToString(); ;


                List<CStructures.OrderItem> itemList = new List<CStructures.OrderItem>();
                itemList.Add(orderItem);
                orderRequest.Prop06OrderItems = itemList;
                orderRequest.Prop07Reserved = "";
                tapClient.SubscribeforFeeds(orderRequest.StructToByte());
                requestSentQueue.EnQueue(orderRequest.ToString());


                if (scripName.Equals(string.Empty))
                {
                    scripName = cmbScrips.SelectedValue.ToString();
                }

                if (!DicOrdersNamevOrderRequest.ContainsKey(scripName))
                    DicOrdersNamevOrderRequest.Add(scripName, orderRequest);

                lbOrdersSend.ItemsSource = null;
                lbOrdersSend.ItemsSource = DicOrdersNamevOrderRequest.Keys;
            }
            else
            {
                MessageBox.Show("Please provide complete details...");
            }
        }

        private bool PlaceOrder(Nifty stockItem, string OrderType)
        {

            CStructures.MessageHeader header = new CStructures.MessageHeader(1);
            header.Prop01MessageLength = CConstants.OrderRequestSize; ;
            header.Prop02TransactionCode = Convert.ToInt16(CConstants.TranCode.OrderRequest);

            CStructures.OrderItem orderItem = new CStructures.OrderItem(true);
            orderItem.Prop01DataLength = CConstants.OrderItemSize;
            orderItem.Prop02OrderID = "";
            string scripName = string.Empty;

            orderItem.Prop05ScripToken = stockItem.ScripCode;
            orderItem.Prop03CustomerID = "1765035"; //"1524410";//"1765035";
            orderItem.Prop04S2KID = "1765035"; //"1524410";// "1765035";
            orderItem.Prop06BuySell = OrderType;//"B";
            if (stockItem.LastTradedPriceFromFeed > 0)
                orderItem.Prop07OrderQty = (int)Math.Round(300000 / stockItem.LastTradedPriceFromFeed);
            else
                return false;
            orderItem.Prop08OrderPrice = (int)Math.Round((stockItem.LastTradedPriceFromFeed + stockItem.LastTradedPriceFromFeed * 0.001)) * 100;
            if (OrderType.ToUpper().Equals("BM") || OrderType.ToUpper().Equals("B"))
                orderItem.Prop09TriggerPrice = (int)Math.Round(Convert.ToDouble(stockItem.Low)) * 100;
            else
                orderItem.Prop09TriggerPrice = 0;
            orderItem.Prop10DisclosedQty = 0;
            orderItem.Prop11ExecutedQty = 0;
            orderItem.Prop12RMSCode = "";
            orderItem.Prop13ExecutedPrice = 0;
            orderItem.Prop14AfterHour = "N";
            orderItem.Prop15GTDFlag = "GFD";//"IOC"; 
            orderItem.Prop16GTD = "";
            orderItem.Prop17Reserved = "";


            int AutoInc = GetCount();
            CStructures.OrderRequest orderRequest = new CStructures.OrderRequest(true);
            orderRequest.Prop01Header = header;

            orderRequest.Prop03OrderCount = Convert.ToInt16(AutoInc);
            orderRequest.Prop04ExchangeCode = "NC";
            orderRequest.Prop05OrderType1 = "NEW";
            orderRequest.Prop02RequestID = AutoInc.ToString();


            List<CStructures.OrderItem> itemList = new List<CStructures.OrderItem>();
            itemList.Add(orderItem);
            orderRequest.Prop06OrderItems = itemList;
            orderRequest.Prop07Reserved = "";
            tapClient.SubscribeforFeeds(orderRequest.StructToByte());
            requestSentQueue.EnQueue(orderRequest.ToString());

            scripName = stockItem.ScripName;

            if (!DicOrdersNamevOrderRequest.ContainsKey(scripName))
                DicOrdersNamevOrderRequest.Add(scripName, orderRequest);

            lbOrdersSend.ItemsSource = null;
            lbOrdersSend.ItemsSource = DicOrdersNamevOrderRequest.Keys;

            return true;
        }

        /// <summary>
        /// To modify and cancel Orders from orders list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOrdersModify_Click(object sender, RoutedEventArgs e)
        {

            if (lbOrdersSend.SelectedIndex == -1) return;

            string element = lbOrdersSend.SelectedValue.ToString();



            if (DicOrdersNamevOrderRequest.ContainsKey(element))
            {
                CStructures.OrderRequest request = DicOrdersNamevOrderRequest[element];

                CStructures.MessageHeader header = new CStructures.MessageHeader(1);
                header.Prop01MessageLength = CConstants.OrderRequestSize; ;
                header.Prop02TransactionCode = Convert.ToInt16(CConstants.TranCode.OrderRequest);

                CStructures.OrderItem orderItem = new CStructures.OrderItem(true);
                orderItem.Prop01DataLength = CConstants.OrderItemSize;
                if (DicSharekhanOrderResponse.ContainsKey(request.Prop02RequestID))
                {
                    orderItem.Prop02OrderID = DicSharekhanOrderResponse[request.Prop02RequestID].Prop05OrderConfirmationItems[0].Prop04SharekhanOrderID;
                    orderItem.Prop12RMSCode = DicSharekhanOrderResponse[request.Prop02RequestID].Prop05OrderConfirmationItems[0].Prop06RMSCode;
                }
                string scripName = string.Empty;

                switch (request.Prop04ExchangeCode)
                {
                    case "NC":
                        orderItem.Prop05ScripToken = ncScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "BC":
                        orderItem.Prop05ScripToken = bcScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "NF":
                        scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                        orderItem.Prop05ScripToken = nfScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "RN":
                        scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                        orderItem.Prop05ScripToken = rnScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "MX":
                        orderItem.Prop05ScripToken = mxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "RM":
                        orderItem.Prop05ScripToken = rmScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                    case "NX":
                        orderItem.Prop05ScripToken = nxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                        break;
                }

                orderItem.Prop03CustomerID = "103956";
                orderItem.Prop04S2KID = "";
                orderItem.Prop06BuySell = "B";
                orderItem.Prop07OrderQty = Convert.ToInt32(txtQty.Text);
                orderItem.Prop08OrderPrice = Convert.ToInt32(txtPrice.Text);
                orderItem.Prop09TriggerPrice = 0;
                orderItem.Prop10DisclosedQty = 0;
                orderItem.Prop11ExecutedQty = 0;

                orderItem.Prop13ExecutedPrice = 0;
                orderItem.Prop14AfterHour = "Y";
                orderItem.Prop15GTDFlag = "GFD";
                orderItem.Prop16GTD = "";
                orderItem.Prop17Reserved = "";

                CStructures.OrderRequest orderRequest = new CStructures.OrderRequest(true);
                orderRequest.Prop01Header = header;
                orderRequest.Prop02RequestID = "";
                orderRequest.Prop03OrderCount = 1;
                orderRequest.Prop04ExchangeCode = cmbScripType.SelectedValue.ToString();
                MenuItem item = sender as MenuItem;
                orderRequest.Prop05OrderType1 = item.Header.ToString().ToUpper();
                orderRequest.Prop02RequestID = request.Prop02RequestID;

                List<CStructures.OrderItem> itemList = new List<CStructures.OrderItem>();
                itemList.Add(orderItem);
                orderRequest.Prop06OrderItems = itemList;
                orderRequest.Prop07Reserved = "";
                tapClient.SubscribeforFeeds(orderRequest.StructToByte());
                requestSentQueue.EnQueue(orderRequest.ToString());
            }
        }

        /// <summary>
        /// To get any Report 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOrderReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOrderReport.SelectedItem.ToString() != "")
            {

                CStructures.MessageHeader header;
                header = new CStructures.MessageHeader(1);
                header.Prop01MessageLength = 181;
                int TransCode = 0;
                switch (cmbOrderReport.SelectedValue.ToString())
                {

                    case "EquityOrderReportItem":
                        TransCode = 31;
                        break;
                    case "DPSRReportItem":
                        TransCode = 32;
                        break;
                    case "CashOrderDetailsReportItem":
                        TransCode = 33;
                        break;
                    case "CashTradeDetailsReportItem":
                        TransCode = 34;
                        break;
                    case "CashLimitReportItem":
                        TransCode = 35;
                        break;
                    case "CashNetPositionReportItem":
                        TransCode = 36;
                        break;
                    case "DerivativeOrderReportItem":
                        TransCode = 41;
                        break;
                    case "TurnOverReportItem":
                        TransCode = 42;
                        break;
                    case "DerivativeOrderDetailReportItem":
                        TransCode = 43;
                        break;
                    case "DerivativeTradeDetailsReportItem":
                        TransCode = 44;
                        break;
                    case "CurrencyLimitReport":
                        TransCode = 54;
                        break;
                    case "CommodityLimitReportItem":
                        TransCode = 49;
                        break;
                    default:
                        break;
                }
                header.Prop02TransactionCode = Convert.ToInt16(TransCode);

                CStructures.ReportRequest OrderReportReq = new CStructures.ReportRequest(true);
                OrderReportReq.Prop01Header = header;
                OrderReportReq.Prop02LoginID = "sr_shah";
                OrderReportReq.Prop03CustomerID = "103956";
                OrderReportReq.Prop04DateTime = "";
                OrderReportReq.Prop05ScripCode = "";
                OrderReportReq.Prop06OrderId = "243638563";    //   For Report put your sharekhanorder ID here  to see Order Report
                //   like example cash order : 243638552      FNOOrder:  58459357 
                OrderReportReq.Prop07Reserved = "";
                tapClient.SendOrderReportRequest(OrderReportReq.StructToByte());
                requestSentQueue.EnQueue(OrderReportReq.ToString());
            }
        }

        private void txtQty_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public static StockScriptsTracking stockTrackingForm;
        public void btnSync_Click(object sender, RoutedEventArgs e)
        {
            //frm  = new AmbiPos();
            //AmbiPos.Refe.Show();
            //processFile();
        }


        public static Dictionary<string, AmbiOrders> dicStringvsValid = new Dictionary<string, AmbiOrders>();
        public static Dictionary<string, bool> dicStringvsStatus = new Dictionary<string, bool>();

        public static Dictionary<string, bool> dicReadFile = new Dictionary<string, bool>();    // 

        public static Dictionary<string, StockwiseInfo> dicStockinfoVsOrderDetail = new Dictionary<string, StockwiseInfo>();
        public static Dictionary<string, int> dicStockinfoVsTrackID = new Dictionary<string, int>();

        public static Dictionary<string, orders> dicReqIDTrack = new Dictionary<string, orders>();

        public static Dictionary<string, string> dicSharekhanIDvsAPIReqID = new Dictionary<string, string>();

        public static Dictionary<string, MainOrderPosition> dicMainPosition = new Dictionary<string, MainOrderPosition>();


        public static CQueue<string> QueueAmbiOrd = new CQueue<string>();
        public static System.IO.FileSystemWatcher FileWatcher;
        static string FileName = "";
        public static List<string> ScriptNameList = new List<string>();
        public static System.Timers.Timer timer;

        public static bool StartRead = false;
        public static string Filepath = "";


        public void processFile()
        {
            try
            {
                Filepath = ConfigurationSettings.AppSettings.Get("Filename");
                string directorypath = System.IO.Path.GetDirectoryName(Filepath);
                string[] arrpath = Filepath.Split('/');
                string filename = arrpath[arrpath.Length - 1];

                FileWatcher = new System.IO.FileSystemWatcher();
                FileWatcher.Path = directorypath; //"C:\\";
                FileWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
                FileWatcher.Filter = filename; //"test.txt";
                FileWatcher.Changed += new System.IO.FileSystemEventHandler(FileWatcher_Changed);
                FileWatcher.EnableRaisingEvents = true;

                List<string> FileList = File.ReadLines(Filepath).ToList<string>();
                ConfigurationSettings.AppSettings.Set("LineNo", FileList.Count().ToString());
                //timer = new System.Timers.Timer();
                //timer.Interval = 2000;
                //timer.Start();
                //timer.Elapsed +=new System.Timers.ElapsedEventHandler(timer_Elapsed);

            }
            catch (Exception ex)
            {

            }

        }

        public static void timer_Elapsed(object obj, EventArgs e)
        {
            if (StartRead == true)
            {
                List<string> FileList = File.ReadLines(ConfigurationSettings.AppSettings.Get("Filename")).ToList<string>();

                int lineNO = Convert.ToInt16(ConfigurationSettings.AppSettings.Get("LineNo"));


                if (lineNO == 0)
                {
                    ConfigurationSettings.AppSettings.Set("LineNo", FileList.Count().ToString());
                }
                else
                {
                    if (FileList.Count > lineNO)
                    {

                        ConfigurationSettings.AppSettings.Set("LineNo", FileList.Count().ToString());
                        string oneline = FileList[FileList.Count - 1].ToString();
                        queueProcessAmbi.EnQueue(oneline);


                        string[] AmbiorderArr = oneline.Split('|').ToArray();

                        if (AmbiorderArr.Length == 6)
                        {
                            if (!ScriptNameList.Contains(AmbiorderArr[4].ToString()))
                            {
                                if (AmbiorderArr[4].ToString() != "")
                                {

                                    ScriptNameList.Add(AmbiorderArr[4].ToString());
                                    ScriptDownloadDLL.ScriptDownload.Main(AmbiorderArr[4].ToString());
                                }
                            }
                        }

                    }
                }
            }
        }

        private static void FileWatcher_Changed(object source, System.IO.FileSystemEventArgs e)
        {

            try
            {
                FileWatcher.EnableRaisingEvents = false;
                int lastcount = Convert.ToInt16(ConfigurationSettings.AppSettings.Get("LineNo"));
                List<string> FileList = File.ReadLines(Filepath).ToList<string>();
                int diff = 0;
                int count = 0;
                count = diff = FileList.Count - lastcount;
                if (count != 1)
                {
                    LogFile.Reference.WriteLogFile("Number of Line : ", count.ToString());
                }
                for (int i = 0; i < diff;)
                {

                    string oneline = FileList[FileList.Count - count].ToString();

                    queueProcessAmbi.EnQueue(oneline);
                    count = count - 1;
                    i++;
                }
                ConfigurationSettings.AppSettings.Set("LineNo", FileList.Count().ToString());

            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile("FileWatcher_Changed Exception : ", ex.ToString());
            }
            finally
            {
                FileWatcher.EnableRaisingEvents = true;
            }
        }

        public void ProcessAmbiOrdes()
        {

            while (true)
            {
                try
                {


                    if (OrderSendflag == true)
                    {

                        string line = queueProcessAmbi.DeQueue();

                        if (line != "")
                        {

                            string[] Ambi = line.Split('|').ToArray();

                            LogFile.Reference.WriteLogFile("ReadString: ", line);

                            if (!dicReadFile.ContainsKey(line))
                            {

                                if (Ambi[1] == "BUY" || Ambi[1] == "SELL" || Ambi[1] == "COVER" || Ambi[1] == "SHORT")
                                {
                                    LogFile.Reference.WriteLogFile("OrderString: ", line);



                                    if (!dicReadFile.ContainsKey(line))
                                    {
                                        dicReadFile.Add(line, true);

                                        if (!dicStringvsValid.ContainsKey(line))
                                        {
                                            string CurrentSignal = "";
                                            int QtyMultiplier = 0;
                                            AmbiOrders objord = new AmbiOrders();
                                            objord.ambistring = line;
                                            objord.Exchange = "NSE";
                                            objord.ExchangeCode = "NF";

                                            if (!Ambi[0].Contains("_"))
                                            {
                                                objord.ScriptName = Ambi[0].ToString();

                                                objord.Expiry = ConfigurationSettings.AppSettings.Get("NearExpiry");
                                            }
                                            else
                                            {
                                                if (Ambi[0].Contains("-"))
                                                {
                                                    string[] NameExp = Ambi[0].Split('-').ToArray();
                                                    if (NameExp[1] == "1")
                                                    {
                                                        objord.ScriptName = NameExp[0].ToString();
                                                        objord.Expiry = ConfigurationSettings.AppSettings.Get("NextExpiry");
                                                    }
                                                }
                                                else
                                                {
                                                    string[] NameExpDate = Ambi[0].Split('_').ToArray();

                                                    if (NameExpDate[1] == "F1")
                                                    {
                                                        objord.ScriptName = NameExpDate[0].ToString();
                                                        objord.Expiry = ConfigurationSettings.AppSettings.Get("NearExpiry");
                                                    }

                                                    if (NameExpDate[1] == "F2")
                                                    {
                                                        objord.ScriptName = NameExpDate[0].ToString();
                                                        objord.Expiry = ConfigurationSettings.AppSettings.Get("NextExpiry");
                                                    }
                                                }
                                            }

                                            objord.Signal = Ambi[1].ToString();
                                            CurrentSignal = objord.Signal;
                                            objord.open = Ambi[2].ToString();
                                            objord.datetime = Ambi[3].ToString();
                                            objord.FormulaName = Ambi[4].ToString();

                                            objord.InstrumentName = "FUT";
                                            objord.Strikeprice = "0";
                                            objord.Quantity = 25;
                                            objord.price = 0;


                                            orders ordinfrm = new orders();
                                            ordinfrm.SettingType = Ambi[5].ToString();
                                            ordinfrm.StockIndexInfo = objord.ScriptName + objord.Expiry + " " + ordinfrm.SettingType;
                                            ordinfrm.ScriptName = objord.ScriptName;

                                            ordinfrm.OrdDateTime = DateTime.Now.ToString();

                                            ordinfrm.open = objord.open;
                                            ordinfrm.datetime = objord.datetime;
                                            ordinfrm.FormulaName = objord.FormulaName;

                                            ordinfrm.Exchange = objord.Exchange;
                                            ordinfrm.ExchangeCode = objord.ExchangeCode;
                                            ordinfrm.InstrumentName = objord.InstrumentName;
                                            ordinfrm.Strikeprice = objord.Strikeprice;
                                            ordinfrm.Expiry = objord.Expiry;
                                            ordinfrm.price = objord.price;

                                            ordinfrm.Signal = objord.Signal;

                                            // dicStringvsStatus.Add(objord.ambistring, true);

                                            if (!dicStockinfoVsOrderDetail.ContainsKey(ordinfrm.ScriptName + ordinfrm.SettingType))
                                            {
                                                if (CurrentSignal == "BUY" || CurrentSignal == "SHORT")
                                                {

                                                    StockwiseInfo objStock = new StockwiseInfo();
                                                    objStock.ScriptInfo = ordinfrm.ScriptName + ordinfrm.SettingType;
                                                    objStock.LastSignal = CurrentSignal;

                                                    if (ordinfrm.Signal == "BUY")
                                                    {
                                                        objStock.NetPosition = 1;
                                                        ordinfrm.NetPosition = 1;
                                                    }
                                                    if (ordinfrm.Signal == "SHORT")
                                                    {
                                                        objStock.NetPosition = -1;
                                                        ordinfrm.NetPosition = -1;
                                                    }


                                                    QtyMultiplier = 1;
                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                    ordinfrm.Quantity = objord.Quantity;
                                                    objStock.Quantity = objord.Quantity;

                                                    dicStockinfoVsOrderDetail.Add(objStock.ScriptInfo, objStock);
                                                    queueAmbi.EnQueue(ordinfrm);
                                                }

                                            }
                                            else
                                            {

                                                lock (dicStockinfoVsOrderDetail[ordinfrm.ScriptName + ordinfrm.SettingType])
                                                {
                                                    StockwiseInfo obj = dicStockinfoVsOrderDetail[ordinfrm.ScriptName + ordinfrm.SettingType];

                                                    switch (obj.LastSignal)
                                                    {
                                                        case "BUY":
                                                            #region BUY Signal
                                                            switch (CurrentSignal)
                                                            {
                                                                case "SELL":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition - 1;

                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    ordinfrm.Quantity = objord.Quantity;

                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;

                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "SHORT":
                                                                    QtyMultiplier = 2;
                                                                    obj.NetPosition = obj.NetPosition - 2;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;

                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "BUY":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition + 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;
                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;

                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "COVER":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition + 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;
                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;

                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;


                                                                default:
                                                                    break;
                                                            }
                                                            #endregion
                                                            break;

                                                        case "SELL":
                                                            #region SELL Signal
                                                            switch (CurrentSignal)
                                                            {
                                                                case "SELL":
                                                                    if (obj.NetPosition > 0)
                                                                    {
                                                                        QtyMultiplier = 1;
                                                                        obj.NetPosition = obj.NetPosition - 1;

                                                                        ordinfrm.NetPosition = obj.NetPosition;

                                                                        objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                        ordinfrm.Quantity = objord.Quantity;

                                                                        obj.LastSignal = CurrentSignal;
                                                                        obj.Quantity = ordinfrm.Quantity;
                                                                        queueAmbi.EnQueue(ordinfrm);
                                                                    }
                                                                    break;

                                                                case "SHORT":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition - 1;

                                                                    ordinfrm.NetPosition = obj.NetPosition;

                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;

                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "BUY":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition + 1;

                                                                    ordinfrm.NetPosition = obj.NetPosition;

                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;

                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "COVER":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition + 1;

                                                                    ordinfrm.NetPosition = obj.NetPosition;

                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;

                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                default:
                                                                    break;
                                                            }
                                                            #endregion
                                                            break;

                                                        case "COVER":
                                                            #region COVER Signal
                                                            switch (CurrentSignal)
                                                            {
                                                                case "COVER":
                                                                    if (obj.NetPosition < 0)
                                                                    {
                                                                        QtyMultiplier = 1;
                                                                        obj.NetPosition = obj.NetPosition + 1;
                                                                        ordinfrm.NetPosition = obj.NetPosition;
                                                                        objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                        ordinfrm.Quantity = objord.Quantity;

                                                                        obj.LastSignal = CurrentSignal;
                                                                        obj.Quantity = ordinfrm.Quantity;
                                                                        queueAmbi.EnQueue(ordinfrm);
                                                                    }
                                                                    break;

                                                                case "SHORT":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition - 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;
                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "BUY":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition + 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;

                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "SELL":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition - 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;
                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                default:
                                                                    break;
                                                            }
                                                            #endregion
                                                            break;

                                                        case "SHORT":
                                                            #region SHORT Signal
                                                            switch (CurrentSignal)
                                                            {
                                                                case "BUY":
                                                                    QtyMultiplier = 2;
                                                                    obj.NetPosition = obj.NetPosition + 2;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;

                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "COVER":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition + 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;
                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;


                                                                case "SHORT":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition - 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;
                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;

                                                                case "SELL":
                                                                    QtyMultiplier = 1;
                                                                    obj.NetPosition = obj.NetPosition - 1;
                                                                    ordinfrm.NetPosition = obj.NetPosition;
                                                                    objord.Quantity = objord.Quantity * QtyMultiplier;
                                                                    ordinfrm.Quantity = objord.Quantity;
                                                                    obj.LastSignal = CurrentSignal;
                                                                    obj.Quantity = ordinfrm.Quantity;
                                                                    queueAmbi.EnQueue(ordinfrm);
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            #endregion
                                                            break;

                                                        default:
                                                            break;
                                                    }

                                                }
                                                dicStringvsValid.Add(objord.ambistring, objord);

                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        string line = queueProcessAmbi.DeQueue();
                        line = "order not send Check order send flag true : " + line;
                        MessageBox.Show(line, "AmbiPosition", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                        LogFile.Reference.WriteLogFile("ReadString: ", line);
                    }

                }

                catch (Exception ex)
                {

                    LogFile.Reference.WriteLogFile("Exception in ProcessAmbiOrdes() : ", ex.ToString());
                    MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Exception in ProcessAmbiOrdes()  : " + ex.ToString()));
                }
            }
        }

        public void sendorder()
        {

            while (true)
            {
                try
                {
                    orders objAmbi = queueAmbi.DeQueue();

                    try
                    {
                        ScriptNameList.Add(objAmbi.ToString());
                        ScriptDownloadDLL.ScriptDownload.Main(objAmbi.ToString());

                    }
                    catch (Exception ex)
                    {
                        LogFile.Reference.WriteLogFile("Exception in send : ", ex.ToString());
                    }

                    lock (objAmbi)
                    {

                        CStructures.MessageHeader header = new CStructures.MessageHeader(1);
                        header.Prop01MessageLength = CConstants.OrderRequestSize; ;
                        header.Prop02TransactionCode = Convert.ToInt16(CConstants.TranCode.OrderRequest);

                        CStructures.OrderItem orderItem = new CStructures.OrderItem(true);
                        orderItem.Prop01DataLength = CConstants.OrderItemSize;
                        orderItem.Prop02OrderID = "";
                        string scripName = string.Empty;

                        switch (objAmbi.ExchangeCode)
                        {
                            case "NC":
                                //    orderItem.Prop05ScripToken = ncScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                break;
                            case "BC":
                                //  orderItem.Prop05ScripToken = bcScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                break;
                            case "NF":
                                scripName = objAmbi.ScriptName + objAmbi.Expiry;
                                // orderItem.Prop05ScripToken = nfScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                orderItem.Prop05ScripToken = nfScripMaster[scripName].Prop03ScripCode.Replace("\0", "");
                                break;
                            case "RN":
                                //   scripName = cmbScrips.SelectedValue.ToString() + cmbExpiry.SelectedValue.ToString() + cmbInstrument.SelectedValue.ToString() + cmbStrike.SelectedValue.ToString();
                                //   orderItem.Prop05ScripToken = rnScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == scripName).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                break;
                            case "MX":
                                //   orderItem.Prop05ScripToken = mxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                                break;
                            case "RM":
                                //   orderItem.Prop05ScripToken = rmScripMaster.Where(w => w.Value.Prop04ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop03ScripCode.Replace("\0", "")).Single().ToString();
                                break;
                            case "NX":
                                //   orderItem.Prop05ScripToken = nxScripMaster.Where(w => w.Value.Prop03ScripShortName.Replace("\0", "") == cmbScrips.SelectedValue.ToString()).Select(s => s.Value.Prop02ScripCode.Replace("\0", "")).Single().ToString();
                                break;
                        }

                        orderItem.Prop03CustomerID = "348147";//"740790"; //"103956" sidharth sir; // "348147" vinod sir; // "1318424" - bhushan thakkar
                        orderItem.Prop04S2KID = "";

                        if (objAmbi.Signal == "BUY" || objAmbi.Signal == "COVER")
                            orderItem.Prop06BuySell = "B";
                        else
                            orderItem.Prop06BuySell = "S";

                        orderItem.Prop07OrderQty = objAmbi.Quantity;
                        orderItem.Prop08OrderPrice = Convert.ToInt16(objAmbi.price);
                        orderItem.Prop09TriggerPrice = 0;
                        orderItem.Prop10DisclosedQty = 0;
                        orderItem.Prop11ExecutedQty = 0;
                        orderItem.Prop12RMSCode = "";
                        orderItem.Prop13ExecutedPrice = 0;
                        orderItem.Prop14AfterHour = "N";
                        orderItem.Prop15GTDFlag = "GFD";
                        orderItem.Prop16GTD = "";
                        orderItem.Prop17Reserved = "";

                        int AutoInc = GetCount();
                        CStructures.OrderRequest orderRequest = new CStructures.OrderRequest(true);
                        orderRequest.Prop01Header = header;
                        orderRequest.Prop02RequestID = AutoInc.ToString();
                        objAmbi.APIReqID = AutoInc.ToString();
                        orderRequest.Prop03OrderCount = Convert.ToInt16(AutoInc);
                        orderRequest.Prop04ExchangeCode = "NF";
                        orderRequest.Prop05OrderType1 = "NEW";



                        List<CStructures.OrderItem> itemList = new List<CStructures.OrderItem>();
                        itemList.Add(orderItem);
                        orderRequest.Prop06OrderItems = itemList;
                        orderRequest.Prop07Reserved = "";

                        lock (dicReqIDTrack)
                            dicReqIDTrack.Add(orderRequest.Prop02RequestID.Replace("\0", "").Trim(), objAmbi);

                        CTAPClient.QueueTrade.EnQueue(orderRequest.Prop02RequestID.Replace("\0", "").Trim());
                        //CreateUpdateXMlFile(objAmbi);  // xml file in write.

                        LogFile.Reference.WriteLogFile("APIConfirmation : ", objAmbi.ToString());

                        tapClient.SubscribeforFeeds(orderRequest.StructToByte());
                        requestSentQueue.EnQueue(orderRequest.ToString());



                    }
                }
                catch (Exception ex)
                {
                    LogFile.Reference.WriteLogFile("Exception sendorder : ", ex.ToString());
                    MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Exception in sendorder()  : " + ex.ToString()));
                }
            }
        }

        public void CreateOrderTable(string ordxmlpath)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(Directory.GetCurrentDirectory() + "\\ORDERS");
                if (!exists)
                    System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\ORDERS");

                using (DataSet ds = new DataSet())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("API_ID", Type.GetType("System.String"));
                    dt.Columns.Add("SCRIPT", Type.GetType("System.String"));
                    dt.Columns.Add("SIGNAL", Type.GetType("System.String"));
                    dt.Columns.Add("OPEN", Type.GetType("System.String"));
                    dt.Columns.Add("DATETIME", Type.GetType("System.String"));
                    dt.Columns.Add("FORMULA", Type.GetType("System.String"));
                    dt.Columns.Add("SETTING", Type.GetType("System.String"));
                    dt.Columns.Add("NETPOSITION", Type.GetType("System.String"));
                    dt.Columns.Add("OrdDATETIME", Type.GetType("System.String"));
                    dt.Columns.Add("QUANTITY", Type.GetType("System.String"));
                    dt.Columns.Add("ExcPrice", Type.GetType("System.String"));
                    dt.Columns.Add("ExcQUANTITY", Type.GetType("System.String"));
                    dt.Columns.Add("ExcDATETIME", Type.GetType("System.String"));

                    dt.Rows.Add(new object[]{ "0", "0", "0"
               , "0", "0", "0", "0", "0","0" ,"0","0","0","0"});
                    ds.Tables.Add(dt);
                    ds.WriteXml(ordxmlpath);
                };
            }
            catch (Exception ex)
            {

                LogFile.Reference.WriteLogFile("Exception CreateOrderTable : ", ex.ToString());
                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Exception in CreateOrderTable()  : " + ex.ToString()));
            }
        }

        public void CreateMainTable(string Mainxmlpath)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(Directory.GetCurrentDirectory() + "\\ORDERS");
                if (!exists)
                    System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\ORDERS");

                using (DataSet ds = new DataSet())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("SCRIPTINFO", Type.GetType("System.String"));
                    dt.Columns.Add("API_ID", Type.GetType("System.String"));
                    dt.Columns.Add("SCRIPT", Type.GetType("System.String"));
                    dt.Columns.Add("OPEN", Type.GetType("System.String"));
                    dt.Columns.Add("DATETIME", Type.GetType("System.String"));
                    dt.Columns.Add("FORMULA", Type.GetType("System.String"));
                    dt.Columns.Add("LastSIGNAL", Type.GetType("System.String"));
                    dt.Columns.Add("CurrentSIGNAL", Type.GetType("System.String"));
                    dt.Columns.Add("SETTING", Type.GetType("System.String"));
                    dt.Columns.Add("LastNETPOSITION", Type.GetType("System.String"));
                    dt.Columns.Add("CurrentNETPOSITION", Type.GetType("System.String"));
                    dt.Columns.Add("BuyAvgPrice", Type.GetType("System.String"));
                    dt.Columns.Add("SellAvgPrice", Type.GetType("System.String"));
                    dt.Columns.Add("TotalBuyQTY", Type.GetType("System.String"));
                    dt.Columns.Add("TotalSellQTY", Type.GetType("System.String"));

                    dt.Columns.Add("BuyTotalPrice", Type.GetType("System.String"));
                    dt.Columns.Add("SellTotalPrice", Type.GetType("System.String"));

                    dt.Columns.Add("MainPNL", Type.GetType("System.String"));
                    dt.Columns.Add("XMLFILENAME", Type.GetType("System.String"));
                    dt.Rows.Add(new object[]{ "0", "0", "0", "0", "0",
                                                     "0", "0", "0","0" ,"0",
                                                     "0","0","0","0", "0", "0","0" ,"0","0"});
                    ds.Tables.Add(dt);
                    ds.WriteXml(Mainxmlpath);
                }
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile("Exception CreateMainTable : ", ex.ToString());
                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Exception in CreateMainTable()  : " + ex.ToString()));

            }

        }

        public static void InsertUpdateOrdertable(string orderxmlpath, orders obj)
        {
            try
            {
                DataSet ds_OldOrd = new DataSet();
                ds_OldOrd.ReadXml(orderxmlpath);
                DataTable dt_oldOrd = new DataTable();
                dt_oldOrd = ds_OldOrd.Tables[0];


                DataRow[] rowArr = dt_oldOrd.Select("API_ID = '" + obj.APIReqID.ToString() + "'");
                if (rowArr.Length != 0)
                {
                    rowArr[0][0] = obj.APIReqID.ToString();
                    rowArr[0][1] = obj.ScriptName.ToString();
                    rowArr[0][2] = obj.Signal.ToString();
                    rowArr[0][3] = obj.open.ToString();
                    rowArr[0][4] = obj.datetime.ToString();
                    rowArr[0][5] = obj.FormulaName.ToString();
                    rowArr[0][6] = obj.SettingType.ToString();
                    rowArr[0][7] = obj.NetPosition.ToString();
                    rowArr[0][8] = obj.OrdDateTime.ToString();
                    rowArr[0][9] = obj.Quantity.ToString();

                    rowArr[0][10] = obj.ExcPrice.ToString(); //obj.ExcQty.ToString();
                    rowArr[0][11] = obj.ExcQty.ToString();  //obj.ExcPrice.ToString();

                    rowArr[0][12] = obj.ExcDateTime == null ? "NULL" : obj.ExcDateTime.ToString();

                }
                else
                {
                    dt_oldOrd.Rows.Add(new object[]{ obj.APIReqID.ToString(), obj.ScriptName.ToString(), obj.Signal.ToString()
               , obj.open.ToString(), obj.datetime.ToString(), obj.FormulaName.ToString(), obj.SettingType.ToString(), obj.NetPosition.ToString(),
                obj.OrdDateTime.ToString() ,obj.Quantity.ToString(),"0","0","0"});
                }

                DataTable dt_newOrd = new DataTable();
                dt_newOrd = dt_oldOrd.Copy();
                DataSet ds_NewOrd = new DataSet();
                ds_NewOrd.Tables.Add(dt_newOrd);
                ds_NewOrd.WriteXml(orderxmlpath);
            }
            catch (Exception ex)
            {

                LogFile.Reference.WriteLogFile("Exception InsertUpdateOrdertable() : ", ex.ToString());
                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Exception in InsertUpdateOrdertable()  : " + ex.ToString()));
            }
        }

        public static void InsertupdateMaintable(string mainxmlpath, MainOrderPosition obj)
        {

            try
            {
                DataSet ds_OldMain = new DataSet();
                ds_OldMain.ReadXml(mainxmlpath);
                DataTable dt_oldMain = new DataTable();
                dt_oldMain = ds_OldMain.Tables[0];

                var rowArr = dt_oldMain.Select("SCRIPTINFO = '" + obj.SCRIPTINFO.ToString() + "'"); ;
                if (rowArr.Length != 0)
                {

                    rowArr[0][0] = obj.SCRIPTINFO.ToString();
                    rowArr[0][1] = obj.API_ID.ToString();
                    rowArr[0][2] = obj.SCRIPT.ToString();
                    rowArr[0][3] = obj.OPEN.ToString();
                    rowArr[0][4] = obj.DATETIME.ToString();
                    rowArr[0][5] = obj.FORMULA.ToString();
                    rowArr[0][6] = obj.LastSIGNAL.ToString();
                    rowArr[0][7] = obj.CurrentSIGNAL.ToString();
                    rowArr[0][8] = obj.SETTING.ToString();
                    rowArr[0][9] = obj.LastNETPOSITION.ToString();
                    rowArr[0][10] = obj.CurrentNETPOSITION.ToString();
                    rowArr[0][11] = obj.BuyAvgPrice;
                    rowArr[0][12] = obj.SellAvgPrice;
                    rowArr[0][13] = obj.TotalBuyQTY.ToString();
                    rowArr[0][14] = obj.TotalSellQTY.ToString();
                    rowArr[0][15] = obj.BuyTotalPrice.ToString();
                    rowArr[0][16] = obj.SellTotalPrice.ToString();
                    rowArr[0][17] = obj.MainPNL.ToString();
                    rowArr[0][18] = obj.XMLFILENAME.ToString();
                }
                else
                {

                    dt_oldMain.Rows.Add(new object[]{ obj.SCRIPTINFO,obj.API_ID.ToString(),obj.SCRIPT.ToString(),obj.OPEN.ToString(),obj.DATETIME.ToString(),obj.FORMULA.ToString(),
                       obj.LastSIGNAL.ToString(),obj.CurrentSIGNAL.ToString(),obj.SETTING.ToString(),obj.LastNETPOSITION.ToString(),obj.CurrentNETPOSITION.ToString()
                       ,obj.BuyAvgPrice.ToString(),obj.SellAvgPrice.ToString(),obj.TotalBuyQTY.ToString(),obj.TotalSellQTY.ToString(),
                       obj.BuyTotalPrice.ToString(),obj.SellTotalPrice.ToString(),obj.MainPNL.ToString(),obj.XMLFILENAME.ToString()});
                }

                DataTable dt_newMain = new DataTable();
                dt_newMain = dt_oldMain.Copy();
                DataSet ds_NewMain = new DataSet();
                ds_NewMain.Tables.Add(dt_newMain);
                ds_NewMain.WriteXml(mainxmlpath);
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile("Exception InsertupdateMaintable() : ", ex.ToString());
                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Exception in InsertupdateMaintable()  : " + ex.ToString()));
            }
        }

        public class MainOrderPosition
        {

            public string SCRIPTINFO { get; set; }
            public string XMLFILENAME { get; set; }
            public int API_ID { get; set; }

            public string SCRIPT { get; set; }

            public string LastSIGNAL { get; set; }
            public string CurrentSIGNAL { get; set; }

            public string OPEN { get; set; }
            public string DATETIME { get; set; }
            public string FORMULA { get; set; }
            public string SETTING { get; set; }

            public int LastNETPOSITION { get; set; }
            public int CurrentNETPOSITION { get; set; }


            public double BuyAvgPrice { get; set; }
            public double SellAvgPrice { get; set; }

            public int TotalBuyQTY { get; set; }
            public int TotalSellQTY { get; set; }

            public double BuyTotalPrice { get; set; }
            public double SellTotalPrice { get; set; }

            public double MainPNL { get; set; }

            public int XmlType { get; set; }

        }

        public class StockwiseInfo
        {
            public string ScriptInfo { get; set; }
            public string LastSignal { get; set; }
            public int NetPosition { get; set; }
            public int Quantity { get; set; }
        }

        public class orders
        {

            public string StockIndexInfo { get; set; }
            public string ScriptName { get; set; }
            public string Signal { get; set; }       // BUY  SELL  COVER SHORT   
            public int NetPosition { get; set; }
            public string open { get; set; }
            public string datetime { get; set; }
            public string FormulaName { get; set; }
            public string SettingType { get; set; }     // Setting :   SET1, SET2,SET3,SET4,SET5
            public double AvgPrice { get; set; }

            public string Exchange { get; set; }
            public string ExchangeCode { get; set; }
            public string InstrumentName { get; set; }
            public string Strikeprice { get; set; }
            public string Expiry { get; set; }
            public int Quantity { get; set; }
            public int price { get; set; }
            public string APIReqID { get; set; }
            public string SharuID { get; set; }
            public string ExchangeOrdID { get; set; }

            public string ConfrmType { get; set; }
            public int ExcQty { get; set; }
            public int ExcPrice { get; set; }

            public string OrdDateTime { get; set; }
            public string ExcDateTime { get; set; }

            public string ExchangeSignal { get; set; }

            //    Buy	Sell	1	-1	      BUY_SELL        
            //    Buy	Short	1	-2	      BUY_SHORT	

            //   Sell	Buy	   -1	 1		  SELL_BUY
            //   Sell	Short  -1	-1	      SELL_SHORT  

            //   Short	Buy	   -1	2         SHORT_BUY 
            //	Short	Cover   -1	1	      SHORT_COVER 

            //  Cover	Short	1	-1        COVER_SHORT
            //   Cover	Buy	    1	1         COVER_BUY

            public override string ToString()
            {

                return "APIReqID : " + APIReqID + " SharekhanID : " + SharuID + "  ExchangeID : " + ExchangeOrdID + "Signal : " + Signal + " ExchangeSignal : " + ExchangeSignal + " NetPosition :" + NetPosition + "  Quantity : " + Quantity + " Price : " + price + " ConfirmationType : " + ConfrmType + " ExecuteQty : " + ExcQty + " ExecutePrice : " + ExcPrice;
            }

        }

        public class AmbiOrders
        {

            public string Exchange { get; set; }
            public string ExchangeCode { get; set; }
            public string InstrumentName { get; set; }
            public string Strikeprice { get; set; }
            public string Expiry { get; set; }

            public string ambistring { get; set; }
            public string ScriptName { get; set; }
            public string open { get; set; }
            public string datetime { get; set; }
            public string FormulaName { get; set; }

            public string Signal { get; set; }
            public int Quantity { get; set; }
            public int price { get; set; }
            public string APIReqID { get; set; }
        }

        private void chkboxOrderFlag_Checked(object sender, RoutedEventArgs e)
        {
            SetOrderSendflag();
        }

        private void chkboxOrderFlag_Click(object sender, RoutedEventArgs e)
        {
            SetOrderSendflag();
        }
        private void SetOrderSendflag()
        {
            OrderSendflag = chkboxOrderFlag.IsChecked == true ? true : false;
        }

    }
}
