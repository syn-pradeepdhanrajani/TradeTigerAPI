using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace TradeTigerAPI
{
    public class CConstants
    {
        public static char Seperator = '|';
        public static int SendTrail = 5;
        public static int MaxThreadWaitTime = 50;
        public static int TrailCountMain = 3;		/// No of times to try send data Main 
        public static long BytesOut = 0;

        public static string LogSeparator = "@";




        public static int TAPHeaderSize = 22;
        public static int MinPacketSize = 60;
        public static int MinBcastpacket = 50;



        public static int SignOff = 190;//185;
        public static int Contract = 28;
        public static int LegInfo = 80;

        public static string TapIp;
        public static string TapPort;
        public static string LoginId;
        public static string MemberPassword;
        public static string TradingPassword;
        public static int MsgHeaderSize = 6;
        public static int LoginRequestSize = 196;
        public static int LoginResponseSize = 433;
        public static int LogOffRequestSize = 136;
        //public static int OrderRequestSize = 357;
        public static int OrderItemSize = 227;
        public static int FeedRequestSize = 120;
        public static int ScripMasterRequest = 108;
        public static int ReportRequestSize = 181;
        public static int MarketDepthRequestSize = 121;
        public static int CommodityMasterSize = 235;
        public static int CurrencycripMasterSize = 329;
        public static int CashcripMasterSize = 184;
        public static int OrderRequestSize = 357;
        public static int DerivativeMasterItemSize = 217;

        public static string NFExCode = "NF";
        public static string NCExcode = "NC";
        public static string BCExcode = "BC";
        public static string NXExcode = "NX";
        public static string MXExcode = "MX";       
        public static string RNExCode = "RN";
        public static string RMExcode = "RM";      

        public enum TranCode : uint
        {

            LoginRequest = 1,
            LogOffRequest = 2,
            ScripMasterRequest = 21,
            Invitation = 15000,
            OrderRequest = 11,
            FeedRequest = 22,
            DepthRequest = 24,

            SysInfoRequest = 1600,
            SysInfoResponse = 1601,
            PriceConfirmation = 2012,

            #region New Order
            NewOrderRequest = 2000,
            NewOrderConfirmation = 2073,
            OrderFreeze = 2170,
            NewOrderRejected = 2231,
            #endregion

            #region Spread Order
            SpreadOrderRequest = 2100,
            SpreadOrderRequestedResponse = 2101,
            SpreadOrderConfirmationResponse = 2124,
            SpreadOrderErrorResponse = 2154,
            SpreadOrderConfirmCancellationResponse = 2130,
            SpreadOrderModRequest = 2118,
            SpreadOrderModAck = 2119,
            SpreadOrderModConfirmResponse = 2136,
            SpreadOrderModRejected = 2133,
            SpreadOrderCancelRejected = 2127,
            SpreadOrderCancelAck = 2107,
            SpreadOrderCancelConfirmation = 2132,
            #endregion

            #region Modify Order
            ModifyOrderRequest = 2040,
            ModifyOrderAck = 2041,
            ModifyOrderRejected = 2042,
            ModifyOrderConfirmation = 2074,
            #endregion

            #region Cancel Order
            CancelOrderRequest = 2070,
            CancelOrderAck = 2071,
            CancelOrderRejected = 2072,
            CancelOrderConfirmation = 2075,
            #endregion

            ExchPortfolioReq = 1775,
            ExchportfolioResponse = 1776,

            StopLossTriggered = 2212,
            TradeConfirmation = 2222,
            TradeError = 2223,
            TradeCancellationRequest = 5440,
            TradeCancellationRejected = 5441,
            TradeModificationRequest = 5445,

            MarketOpenMessage = 6511,
            MarketCloseMessage = 6521,
            MarketPreOpenShutDownMsg = 6531,
            NormalMarketPreOpenEnd = 6571,

            DeltaDownloadReq = 7000,
            DeltaDownloadHeader = 7011,   // Message Download Starts
            DeltaDownloadRecord = 7021,   // Message Download Data Received
            DeltaDownloadTrailer = 7031,  // Message Download End

            LocalDBUpdRequest = 7300,
            LocalDBUpdData = 7304,      // Local DB Data Received
            LocalDBUpdHeader = 7307,    // Local DB Download Starts
            LocalDBUpdTrailer = 7308,   // Local DB Download End

            SecurityStatus = 7320,
            SysInfoPartialResponse = 7321,

            BatchOrderCancel = 9002,
            CtrlMsgToTrader = 5295,
            TradeModifyConfirm = 2287,
            TradeModifyReject = 2288,
            TradeCancelConfirm = 2282,
            TradeCancellationReject = 2286,

            ExerPositionRequest = 4000,
            ExcerPositionAck = 4001,
            ExcerPoistionConfirm = 4002,
            ExcerciseModification = 4005,
            ExcerciseModificationConfirm = 4007,
            ExerciseCancellationReq = 4008,
            ExerciseCancelConfirm = 4010,

            IndexMapTable = 7326,



            #region 2L and 3L Orders
            OrderEntryRequest2L = 2102,
            OrderEntryRequest3L = 2104,
            OrderEntryAck2L = 2103,
            OrderEntryAcl3L = 2105,
            OrderConfirmation2L = 2125,
            OrderConfirmation3L = 2126,
            OrderError2L = 2155,
            OrderError3L = 2156,
            OrderCancelConfirm2L = 2131,
            OrderCancelConfirm3L = 2132,
            #endregion

            #region BroadCastTranscode

            BCastMessage = 6501,
            BCastSecurityMstchg = 7305,
            BCastParticipantMstChg = 7306,
            BCastSecurityStatusChg = 7320,
            BCastSecurityStatusChgPreOpen = 7210,
            BCastTurnoverExceeded = 9010,
            BCastBrokerReactivated = 9011,
            BCastAuctionInqOut = 6582,
            BCastAuctionStatusChg = 6581,
            BCastOpenMsg = 6511,
            BCastCloseMsg = 6521,
            BCastPreOpenShutDownMsg = 6531,
            BCastNormalMktPreopenEnded = 6571,
            BCastTickerandMktIndex = 7202,
            BcastMBOMBPUpdate = 7200,
            BCastOnlyMBP = 7208,
            BCastMWPoundRobin = 7201,
            SecurityOpenPrice = 6013,
            BCastCktCheck = 6541,
            BCastIndices = 7207,
            BCastIndustryIndices = 7203,
            BCastBuyBack = 7211

            #endregion

        }

       
    }
}