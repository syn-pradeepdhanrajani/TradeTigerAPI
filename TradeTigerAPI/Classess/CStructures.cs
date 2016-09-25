using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Linq;

namespace TradeTigerAPI
{
    public class CStructuresold
    {

        #region Message Header Structure
        /// <summary>
        /// MESSAGEHEADER    (Size :=  38) (Location := 0 = 37)
        /// </summary>
        public struct MessageHeader : IStruct
        {

            /*  CHAR iapitcode              1       User id
                CHAR iapifuncid             1       Trader id
                LONG Logtime                4       0
                CHAR AlphaChar [2]          2       Blank
                SHORT TransactionCode       2       Transaction Code    
                SHORT ErrorCode             2       0
                CHAR Timestamp  [8]         8       0
                CHAR TimeStamp1 [8]         8       0
                CHAR TimeStamp2 [8]         8
                SHORT MessageLength         2
             */

            /// <summary>
            /// CHAR iApiTcode[1]  :Size 1  (Location :0=1)
            /// </summary>
            public char[] ciApiTCode;
            /// <summary>
            /// CHAR iApiTcode[1]  :Size 1  (Location :0=1)
            /// This field should contain the user ID.
            /// </summary>
            public string Prop01ApiTCode
            {
                get { return new string(ciApiTCode); }
                set { ciApiTCode = CUtility.GetPreciseArrayForString(value.ToUpper(), ciApiTCode.Length); }
            }

            /// <summary>
            /// CHAR iApiTcode[1]  :Size 1  (Location :1=2)
            /// </summary>
            public char[] ciApiFuncId;
            /// <summary>
            /// CHAR iApiFuncId[1]  :Size 1  (Location :1=2)
            /// This field should contain the trader ID.
            /// </summary>
            public string Prop02Apifunction
            {
                get { return new string(ciApiFuncId); }
                set { ciApiFuncId = CUtility.GetPreciseArrayForString(value.ToUpper(), ciApiFuncId.Length); }
            }

            /// <summary>
            /// LONG LogTime  :Size 4  (Location :2=5)
            /// </summary>
            public Int32 iLogTime;
            /// <summary>
            /// LONG LogTime  :Size 4 (Location :2=5)
            /// set to zero while sending messages to the host.
            /// </summary>
            public Int32 Prop03LogTime
            {
                get { return iLogTime; }
                set { iLogTime = value; }

            }

            /// <summary>
            /// CHAR AlphaChar[2]  :Size 2 (Location :6=7)
            /// </summary>
            public char[] cAlphaChar;
            /// <summary>
            /// CHAR AlphaChar[2]  :Size 2 (Location :6=7)
            /// </summary>
            public string Prop04AlphaChar
            {

                get { return new string(cAlphaChar); }
                set { cAlphaChar = CUtility.GetPreciseArrayForString(value.ToUpper(), cAlphaChar.Length); }

            }

            /// <summary>
            /// SHORT TransactionCode   :Size 2  (Location:8=9)
            /// </summary>
            public Int16 iTransactionCode;
            /// <summary>
            /// SHORT TransactionCode   :Size 2  (Location:8=9)
            /// This field should contain the transaction message number.
            /// </summary>
            public Int16 Prop05TransactionCode
            {
                get { return iTransactionCode; }
                set { iTransactionCode = value; }

            }

            /// <summary>
            /// SHORT ErrorCode   :Size 2  (Location:10=11)
            /// </summary>
            public Int16 iErrorCode;
            /// <summary>
            /// SHORT ErrorCode   :Size 2  (Location:10=11)
            /// </summary>
            public Int16 Prop06ErrorCode
            {
                get { return iErrorCode; }
                set { iErrorCode = value; }

            }

            /// <summary>
            /// CHAR Timestamp [8]  :Size 8(Location:12=19)
            /// </summary>
            public char[] cTimeStamp;
            /// <summary>
            /// CHAR Timestamp [8]  :Size 8(Location:12=19)
            /// </summary>
            public string Prop07TimeStamp
            {
                get { return new string(cTimeStamp); }
                set { cTimeStamp = CUtility.GetPreciseArrayForString(value.ToUpper(), cTimeStamp.Length); }
            }

            /// <summary>
            /// CHAR TimeStamp1 [8]  :Size 8(Location:20=27)
            /// /// </summary>
            public char[] cTimeStamp1;
            /// <summary>
            /// CHAR TimeStamp1 [8]  :Size 8(Location:20=27)
            /// /// </summary>
            public string Prop08TimeStamp1
            {
                get { return new string(cTimeStamp1); }
                set { cTimeStamp1 = CUtility.GetPreciseArrayForString(value.ToUpper(), cTimeStamp1.Length); }
            }

            /// <summary>
            /// CHAR TimeStamp2 [8]  :Size 8(Location:28=35)
            /// /// </summary>
            public char[] cTimeStamp2;
            /// <summary>
            /// CHAR TimeStamp2 [8]  :Size 8(Location:28=35)
            /// /// </summary>
            public string Prop09TimeStamp2
            {
                get { return new string(cTimeStamp2); }
                set { cTimeStamp2 = CUtility.GetPreciseArrayForString(value.ToUpper(), cTimeStamp2.Length); }
            }

            /// <summary>
            /// SHORT MessageLength  :Size 2(Location:36=37)
            /// /// </summary>
            public Int16 iMessageLength;
            /// <summary>
            /// SHORT MessageLength  :Size 2(Location:36=37)
            /// This field is set to the length of the entire message
            /// /// </summary>
            public Int16 Prop10MessageLength
            {
                get { return iMessageLength; }
                set { iMessageLength = value; }

            }

            #region Constructor
            /// <summary>
            /// 
            /// </summary>
            /// <param name="trans">cconstant.transcode</param>
            /// <param name="MsgLen">Total Messsage Length</param>
            public MessageHeader(Int16 Transcode)
            {

                ciApiTCode = new char[1];
                ciApiFuncId = new char[1];
                cAlphaChar = new char[2];
                cTimeStamp = new char[8];
                cTimeStamp1 = new char[8];
                cTimeStamp2 = new char[8];
                iLogTime = 0;
                iTransactionCode = 0;
                iErrorCode = 0;
                iMessageLength = 0;
                //cTimeStamp = "0       ".ToCharArray();
                //cTimeStamp1 = "0       ".ToCharArray();
                //cTimeStamp2 = "0 ".ToCharArray();
                //cAlphaChar = "  ".ToCharArray(); //"  ".ToCharArray();

            }
            # endregion

            public byte[] StructToByte()
            {
                byte[] HeadStruct = new byte[CConstants.MsgHeaderSize];
                try
                {

                    // Reverse the byte

                    byte[] Exchguserid = new byte[ciApiTCode.Length];
                    Exchguserid = CUtility.ConvertCharFrom16to8bit(ciApiTCode);

                    byte[] Exchgtraderid = new byte[ciApiFuncId.Length];
                    Exchgtraderid = CUtility.ConvertCharFrom16to8bit(ciApiFuncId);

                    byte[] ExchgLogTime;
                    ExchgLogTime = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(iLogTime));
                    // convert char to 1byte 
                    byte[] ExchgAlphaChar = new byte[cAlphaChar.Length];
                    ExchgAlphaChar = CUtility.ConvertCharFrom16to8bit(cAlphaChar);

                    byte[] ExchgTransactionCode;
                    ExchgTransactionCode = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(iTransactionCode));

                    byte[] ExchgErrorCode;
                    ExchgErrorCode = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(iErrorCode));

                    byte[] ExchgTimeStamp = new byte[cTimeStamp.Length];
                    ExchgTimeStamp = CUtility.ConvertCharFrom16to8bit(cTimeStamp);

                    byte[] ExchgTimeStamp1 = new byte[cTimeStamp1.Length];
                    ExchgTimeStamp1 = CUtility.ConvertCharFrom16to8bit(cTimeStamp1);

                    byte[] ExchgTimeStamp2 = new byte[cTimeStamp2.Length];
                    ExchgTimeStamp2 = CUtility.ConvertCharFrom16to8bit(cTimeStamp2);

                    byte[] ExchgMessageLength;
                    ExchgMessageLength = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(iMessageLength));

                    Buffer.BlockCopy(Exchguserid, 0, HeadStruct, 0, 1);
                    Buffer.BlockCopy(Exchgtraderid, 0, HeadStruct, 1, 1);
                    Buffer.BlockCopy(ExchgLogTime, 0, HeadStruct, 2, 4);
                    Buffer.BlockCopy(ExchgAlphaChar, 0, HeadStruct, 6, 2);
                    Buffer.BlockCopy(ExchgTransactionCode, 0, HeadStruct, 8, 2);
                    Buffer.BlockCopy(ExchgErrorCode, 0, HeadStruct, 10, 2);
                    Buffer.BlockCopy(ExchgTimeStamp1, 0, HeadStruct, 20, 8);
                    Buffer.BlockCopy(ExchgTimeStamp2, 0, HeadStruct, 28, 8);
                    Buffer.BlockCopy(ExchgMessageLength, 0, HeadStruct, 36, 2);
                    return HeadStruct;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Message Header Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return HeadStruct;

                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01ApiTCode = Encoding.ASCII.GetString(ByteStructure, 0, 1);
                    Prop02Apifunction = Encoding.ASCII.GetString(ByteStructure, 1, 1);
                    Prop03LogTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 2));
                    Prop04AlphaChar = Encoding.ASCII.GetString(ByteStructure, 6, 2);
                    Prop05TransactionCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 8));
                    Prop06ErrorCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 10));
                    Prop07TimeStamp = Encoding.ASCII.GetString(ByteStructure, 12, 8);
                    Prop08TimeStamp1 = Encoding.ASCII.GetString(ByteStructure, 20, 8);
                    Prop09TimeStamp2 = Encoding.ASCII.GetString(ByteStructure, 28, 8);
                    Prop10MessageLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 36));

                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Message Header Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }


            }

            public override string ToString()
            {
                try
                {
                    return (Prop01ApiTCode.ToString() + CConstants.Seperator +
                        Prop02Apifunction.ToString() + CConstants.Seperator +
                        Prop03LogTime.ToString() + CConstants.Seperator +
                        Prop04AlphaChar + CConstants.Seperator +
                        Prop05TransactionCode.ToString() + CConstants.Seperator +
                        Prop06ErrorCode.ToString() + CConstants.Seperator +
                        Prop07TimeStamp.ToString() + CConstants.Seperator +
                        Prop07TimeStamp.ToString() + CConstants.Seperator +
                        Prop08TimeStamp1.ToString() + CConstants.Seperator +
                        Prop09TimeStamp2.ToString() + CConstants.Seperator +
                        Prop10MessageLength.ToString());
                }
                catch (Exception ex)
                {

                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }

        # endregion

        #region Error Response
        /// <summary>
        /// MSERRORRESPONSE (Size:180) (Location:0=179)
        /// </summary>
        public struct ErrorResponse : IStruct
        {
            /*
             *          Message Header       38
             * Char     key                  14
             * Char     Error Message        128
             * ==================================
             * Total                         180        Tap Header(22)+Error Response(180)=202
             * ==================================
             * */
            /// <summary>
            /// MESSAGEHEADER  Size(38) Location:0=37
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// MESSAGEHEADER :Size(38) Location:0=37
            /// Transaction Code 
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            /// <summary>
            /// CHAR Key [14] :Size (14) (Location:38=51)
            /// </summary>
            public char[] ckey;
            /// <summary>
            /// CHAR Key [14] :Size (14) (Location:38=51)
            /// </summary>
            public String Prop02Key
            {
                get { return new string(ckey); }
                set { ckey = CUtility.GetPreciseArrayForString(value.ToUpper(), ckey.Length); }
            }
            /// <summary>
            /// CHAR ErrorMessage [128] :Size (128) (Location:52=179)
            /// </summary>
            public char[] cErrorMessage;
            /// <summary>
            /// CHAR ErrorMessage [128] :Size (128) (Location:52=179)
            /// </summary>
            public String Prop03ErrorMessage
            {
                get { return new string(cErrorMessage); }
                set { cErrorMessage = CUtility.GetPreciseArrayForString(value.ToUpper(), cErrorMessage.Length); }
            }
            /// <summary>
            /// Error Response Code
            /// </summary>
            /// <param name="transcode">Transcode Received</param>
            public ErrorResponse(Int16 transcode)
            {
                strHeader = new MessageHeader(transcode);
                ckey = new char[14];
                cErrorMessage = new char[128];
                //ckey = "    ".ToCharArray();
                //cErrorMessage = "   ".ToCharArray();

            }

            public byte[] StructToByte()
            {
                try
                {
                    byte[] Errorresponse = new byte[CConstants.ErrorResponseSize];
                    Errorresponse = CUtility.PutBytesInPosition(strHeader.StructToByte(), 0, 38, Errorresponse);
                    Errorresponse = CUtility.PutBytesInPosition(CUtility.ConvertCharFrom16to8bit(ckey), 38, 14, Errorresponse);
                    Errorresponse = CUtility.PutBytesInPosition(CUtility.ConvertCharFrom16to8bit(cErrorMessage), 52, 128, Errorresponse);
                    return Errorresponse;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Error Response Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02Key = Encoding.ASCII.GetString(ByteStructure, 38, 14);
                    Prop03ErrorMessage = Encoding.ASCII.GetString(ByteStructure, 52, 128);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Error Response Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }

            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() +
                        Prop02Key.ToString() +
                        Prop03ErrorMessage.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Error Response Struture's ToString.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }


            }
        }
        #endregion

        #region Broker Eligibility
        public struct BrokerEiligibilityPerMarket : IStruct
        {
            /*      Reserved :           4 bits
                    Auction market:      1 bit
                    Spot market :        1 bit
             *      Oddlot market :      1 bit
                    Normal market:       1 bit
                    Reserved :           8 bits
             * ==============================================
             * Total                     16 bits(2 Bytes)
             * ============================================
             */

            /// <summary>
            /// Auction market: 1 bit Size(1 bit)
            /// </summary>
            public bool bolAuctionMarket;
            /// <summary>
            /// Auction market: 1 bit Size(1 bit)
            /// </summary>
            public bool Prop01AuctionMarket
            {
                get { return bolAuctionMarket; }
                set { bolAuctionMarket = value; }
            }

            /// <summary>
            /// Spot market : 1 bit
            /// </summary>
            public bool bolSpotMarket;
            /// <summary>
            /// Spot market : 1 bit
            /// </summary>
            public bool Prop02SpotMarket
            {
                get { return bolSpotMarket; }
                set { bolSpotMarket = value; }
            }

            /// <summary>
            /// Oddlot market : 1 bit
            /// </summary>
            public bool bolOddlotMarket;
            /// <summary>
            /// Oddlot market : 1 bit
            /// </summary>
            public bool Prop03OddlotMarket
            {
                get { return bolOddlotMarket; }
                set { bolOddlotMarket = value; }
            }

            /// <summary>
            /// Normal market: 1 bit
            /// </summary>
            public bool bolNormalMarket;
            /// <summary>
            /// Normal market: 1 bit
            /// </summary>
            public bool Prop04NormalMarket
            {
                get { return bolNormalMarket; }
                set { bolNormalMarket = value; }
            }

            public byte[] StructToByte()
            {
                try
                {
                    byte[] BrokerEligibility = new byte[2];
                    byte bfill = Convert.ToByte(0);
                    byte breserve = Convert.ToByte(0);
                    bfill = CUtility.ConvertByteStructureToExchangeBitFormat
                        (false, false,
                         false, false,
                         false, false,
                         false, false,
                         bolAuctionMarket, true,
                         bolSpotMarket, true,
                         bolOddlotMarket, true,
                         bolNormalMarket, true, bfill);
                    BrokerEligibility[0] = bfill;
                    BrokerEligibility[1] = breserve;
                    return BrokerEligibility;

                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "BrokerEiligibilityPerMarket Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

            public BrokerEiligibilityPerMarket(bool autionmkt, bool spotmkt, bool oddmkt, bool normalmkt)
            {

                bolAuctionMarket = autionmkt;
                bolSpotMarket = spotmkt;
                bolOddlotMarket = oddmkt;
                bolNormalMarket = normalmkt;

            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    byte bfill = Convert.ToByte(0);
                    byte breserve = Convert.ToByte(0);
                    bfill = ByteStructure[0];
                    breserve = ByteStructure[1];
                    bool[] temp = CUtility.ConvertExchangeBitFormatToStructure(bfill);
                    Prop01AuctionMarket = temp[3];
                    Prop02SpotMarket = temp[2];
                    Prop03OddlotMarket = temp[1];
                    Prop04NormalMarket = temp[0];
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "BrokerEiligibilityPerMarket Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public override string ToString()
            {
                try
                {
                    return (Prop01AuctionMarket.ToString() +
                       Prop02SpotMarket.ToString() +
                       Prop03OddlotMarket.ToString() +
                       Prop04NormalMarket.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "BrokerEiligibilityPerMarket Struture's Tostring.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }
        }
        #endregion

        #region Sign On
        /// <summary>
        /// SIGNON Size(210) Location(0=209)
        /// </summary>
        public struct SignOn : IStruct
        {

            /*
             *  Message header                          38      38
             *  SHORT UserId                             2      40
                CHAR Password [8]                        8      48
                CHAR NewPassword [8]                     8      56
                CHAR TraderName [26]                    26      82
                LONG LastPasswordChangeDate              4      86
                CHAR BrokerId [5]                        5      91  
                CHAR Reserved                            1      92
                SHORT BranchId                           2      94
                LONG VersionNumber                       4      98
                LONG Batch2StartTime                     4      102
                CHAR HostSwitchContext                   1      103
                CHAR Colour [50]                        50      153
                CHAR Reserved                            1      154
                SHORT UserType                           2      156
                DOUBLE SequenceNumber                    8      164
                CHAR WsClassName [14]                   14      178
                CHAR BrokerStatus                        1      179
                CHAR ShowIndex                           1      180
                struct STBROKERELIGIBILITYPERMKT     2      182
                SHORT MemberType                         2      184
                CHAR ClearingStatus                      1      185
                CHAR BrokerName [25]                    25      210              
             * ================================================
             * Total                                    210
             * ================================================
             */
            /// <summary>
            /// MessageHeader : Size 38 (Location:0=37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// MessageHeader : Size 38 (Location:0=37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }

            /// <summary>
            /// SHORT UserId : Size(2) (Location=38=39)
            /// </summary>
            Int16 iUserId;
            /// <summary>
            /// SHORT UserId : Size(2) (Location=38=39)
            /// </summary>
            public Int16 Prop02UserId
            {
                get { return iUserId; }
                set { iUserId = value; }
            }

            /// <summary>
            /// CHAR Password [8] :Size(8) Location:40=47
            /// </summary>
            public Char[] cPassword;
            /// <summary>
            /// CHAR Password [8] :Size(8) Location:40=47
            /// </summary>
            public string Prop03Password
            {
                get { return new string(cPassword); }
                set { cPassword = CUtility.GetPreciseArrayForString(value.ToUpper(), cPassword.Length); }
            }


            /// <summary>
            /// CHAR NewPassword [8] : Size(8) Location:48=55
            /// </summary>
            public Char[] cNewPassword;
            /// <summary>
            /// CHAR NewPassword [8] : Size(8) Location:48=55
            /// </summary>
            public string Prop04NewPassword
            {
                get { return new string(cNewPassword); }
                set { cNewPassword = CUtility.GetPreciseArrayForString(value.ToUpper(), cNewPassword.Length); }
            }

            /// <summary>
            /// CHAR TraderName [26] :Size(26) Location(56=81)
            /// </summary>
            Char[] cTraderName;
            /// <summary>
            /// CHAR TraderName [26] :Size(26) Location(56=81)
            /// </summary>
            public string Prop05Tradername
            {
                get { return new string(cTraderName); }
                set { cTraderName = CUtility.GetPreciseArrayForString(value.ToUpper(), cTraderName.Length); }
            }

            /// <summary>
            /// LONG LastPasswordChangeDate :size(4) Location:82=85
            /// </summary>
            Int32 iLastPwdChangeDate;
            /// <summary>
            /// LONG LastPasswordChangeDate :size(4) Location:82=85
            /// </summary>
            public Int32 Prop06LastPwdChangeDate
            {
                get { return iLastPwdChangeDate; }
                set { iLastPwdChangeDate = value; }
            }

            /// <summary>
            /// CHAR BrokerId [5] : size(5) Location(86=90)
            /// </summary>
            Char[] cBrokerId;
            /// <summary>
            /// CHAR BrokerId [5] : size(5) Location(86=90)
            /// </summary>
            public string Prop07Brokerid
            {
                get { return new string(cBrokerId); }
                set { cBrokerId = CUtility.GetPreciseArrayForString(value.ToUpper(), cBrokerId.Length); }
            }

            /// <summary>
            /// CHAR Reserved :size(1)  Location(91)
            /// </summary>
            Char[] cReserved;
            /// <summary>
            /// CHAR Reserved :size(1)  Location(91)
            /// </summary>
            public string Prop08Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToUpper(), cReserved.Length); }
            }

            /// <summary>
            /// SHORT BranchId :Size(2) Location(92=93)
            /// </summary>
            Int16 iBranchId;
            /// <summary>
            /// SHORT BranchId :Size(2) Location(92=93)
            /// </summary>
            public Int16 Prop09Branchid
            {
                get { return iBranchId; }
                set { iBranchId = value; }
            }

            /// <summary>
            /// LONG VersionNumber : Size(4) Location(94=97)
            /// </summary>
            Int32 iVersionNumber;
            /// <summary>
            /// LONG VersionNumber : Size(4) Location(94=97)
            /// </summary>
            public Int32 Prop10VersionNumber
            {
                get { return iVersionNumber; }
                set { iVersionNumber = value; }
            }

            /// <summary>
            /// LONG Batch2StartTime :Size(4) Location:98=101
            /// </summary>
            Int32 iBatchToStartTime;
            /// <summary>
            /// LONG Batch2StartTime :Size(4) Location:98=101
            /// </summary>
            public Int32 Prop11BatchToStartTime
            {
                get { return iBatchToStartTime; }
                set { iBatchToStartTime = value; }
            }

            /// <summary>
            /// CHAR HostSwitchContext : size(1) Location(102)
            /// </summary>
            Char[] cHostSwitchContext;
            /// <summary>
            /// CHAR HostSwitchContext : size(1) Location(102)
            /// </summary>
            public string Prop12HostSwitchContext
            {
                get { return new string(cHostSwitchContext); }
                set { cHostSwitchContext = CUtility.GetPreciseArrayForString(value.ToUpper(), cHostSwitchContext.Length); }
            }

            /// <summary>
            /// CHAR Colour [50] : Size(50) Location(103=152)
            /// </summary>
            public Char[] cColour;
            /// <summary>
            /// CHAR Colour [50] : Size(50) Location(103=152)
            /// </summary>
            public string Prop13Colour
            {
                get { return new string(cColour); }
                set { cColour = CUtility.GetPreciseArrayForString(value.ToUpper(), cColour.Length); }
            }

            /// <summary>
            /// CHAR Reserved : Size(1) Location(153)
            /// </summary>
            Char[] cReserve1;
            /// <summary>
            /// CHAR Reserved : Size(1) Location(153)
            /// </summary>
            public string Prop14Reserved1
            {
                get { return new string(cReserve1); }
                set { cReserve1 = CUtility.GetPreciseArrayForString(value.ToUpper(), cReserve1.Length); }
            }

            /// <summary>
            /// SHORT UserType :size(2) Location(154=155))
            /// • ‘0’ denotes Dealer
            /// •  ‘4’ denotes Corporate Manager
            /// •  ‘5’ denotes Branch Manager
            /// </summary>
            Int16 iUserType;
            public Int16 Prop15Usertype
            {
                get { return iUserType; }
                set { iUserType = value; }
            }

            /// <summary>
            /// DOUBLE SequenceNumber :Size(8) Location(156=163)
            /// </summary>
            public Double iSequenceNumber;
            /// <summary>
            /// SHORT UserType :size(2) Location(154=155))
            /// </summary>
            public Double Prop16Sequenceno
            {
                get { return iSequenceNumber; }
                set { iSequenceNumber = value; }
            }

            /// <summary>
            /// CHAR WsClassName [14] :Size(14) Location(164=177)
            /// </summary>
            Char[] cWSClassName;
            /// <summary>
            /// CHAR WsClassName [14] :Size(14) Location(164=177)
            /// </summary>
            public string Prop17WSClassName
            {
                get { return new string(cWSClassName); }
                set { cWSClassName = CUtility.GetPreciseArrayForString(value.ToUpper(), cWSClassName.Length); }
            }

            /// <summary>
            /// CHAR BrokerStatus :size(1) Location(178)
            /// S=Suspended 
            /// A=Active
            /// D=DeActive
            /// C=Close out
            /// </summary>
            Char[] cBrokerStatus;
            /// <summary>
            /// CHAR BrokerStatus :size(1) Location(178)
            /// </summary>
            public string Prop18Brokerstatus
            {
                get { return new string(cBrokerStatus); }
                set { cBrokerStatus = CUtility.GetPreciseArrayForString(value.ToUpper(), cBrokerStatus.Length); }
            }

            /// <summary>
            /// CHAR ShowIndex : Size(1) Location(179)
            /// </summary>
            Char[] cShowIndex;
            /// <summary>
            /// CHAR ShowIndex : Size(1) Location(179)
            /// </summary>
            public string Prop19ShowIndex
            {
                get { return new string(cShowIndex); }
                set { cShowIndex = CUtility.GetPreciseArrayForString(value.ToUpper(), cShowIndex.Length); }
            }

            /// <summary>
            ///  BROKERELIGIBILITYPERMKT : Size(2) Location(180=181)
            /// </summary>
            BrokerEiligibilityPerMarket strEligibilty;
            /// <summary>
            ///  BROKERELIGIBILITYPERMKT : Size(2) Location(180=181)
            /// </summary>
            public BrokerEiligibilityPerMarket Prop20Eligibilty
            {
                get { return strEligibilty; }
                set { strEligibilty = value; }
            }

            /// <summary>
            /// SHORT MemberType : Size(2) Location(182=183)
            /// 1= Trading Member
            /// 2= Trading and Clearing Member
            /// 3= Clearing Member 
            /// </summary>
            Int16 iMemberType;
            /// <summary>
            /// SHORT MemberType : Size(2) Location(182=183)
            /// </summary>
            public Int16 Prop21MemberType
            {
                get { return iMemberType; }
                set { iMemberType = value; }
            }

            /// <summary>
            /// CHAR ClearingStatus :Size(1) Location(184)
            /// A=Active
            /// S=Suspended
            /// D=DeActivated
            /// </summary>
            Char[] cClearingStatus;
            /// <summary>
            /// CHAR ClearingStatus :Size(1) Location(184)
            /// </summary>
            public string Prop22ClearStatus
            {
                get { return new string(cClearingStatus); }
                set { cClearingStatus = CUtility.GetPreciseArrayForString(value.ToUpper(), cClearingStatus.Length); }
            }

            /// <summary>
            /// CHAR BrokerName [25]  size(25) Location(185=209)
            /// </summary>
            Char[] cBrokerName;
            /// <summary>
            /// CHAR BrokerName [25]  size(25) Location(185=209)
            /// </summary>
            public string Prop23BrokerName
            {
                get { return new string(cBrokerName); }
                set { cBrokerName = CUtility.GetPreciseArrayForString(value.ToUpper(), cBrokerName.Length); }
            }

            #region Implemented Methods
            public byte[] StructToByte()
            {
                byte[] LoginRequest = new byte[CConstants.LoginReqResSize];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, LoginRequest, 0, 38);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iUserId), 0, LoginRequest, 38, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cPassword), 0, LoginRequest, 40, 8);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cNewPassword), 0, LoginRequest, 48, 8);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cTraderName), 0, LoginRequest, 56, 26);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastPwdChangeDate), 0, LoginRequest, 82, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBrokerId), 0, LoginRequest, 86, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserved), 0, LoginRequest, 91, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBranchId), 0, LoginRequest, 92, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVersionNumber), 0, LoginRequest, 94, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBatchToStartTime), 0, LoginRequest, 98, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cHostSwitchContext), 0, LoginRequest, 102, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cColour), 0, LoginRequest, 103, 50);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve1), 0, LoginRequest, 153, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iUserType), 0, LoginRequest, 154, 2);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iSequenceNumber), 0, LoginRequest, 156, 8);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cWSClassName), 0, LoginRequest, 164, 14);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBrokerStatus), 0, LoginRequest, 178, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cShowIndex), 0, LoginRequest, 179, 1);
                    Buffer.BlockCopy(strEligibilty.StructToByte(), 0, LoginRequest, 180, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iMemberType), 0, LoginRequest, 182, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cClearingStatus), 0, LoginRequest, 184, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBrokerName), 0, LoginRequest, 185, 25);
                    return LoginRequest;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return LoginRequest;
                }

            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02UserId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                    Prop03Password = Encoding.ASCII.GetString(ByteStructure, 40, 8);
                    Prop04NewPassword = Encoding.ASCII.GetString(ByteStructure, 48, 8);
                    Prop05Tradername = Encoding.ASCII.GetString(ByteStructure, 56, 26);
                    Prop06LastPwdChangeDate = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 82));
                    Prop07Brokerid = Encoding.ASCII.GetString(ByteStructure, 86, 5);
                    Prop08Reserved = Encoding.ASCII.GetString(ByteStructure, 91, 1);
                    Prop09Branchid = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 92));
                    Prop10VersionNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 94));
                    Prop11BatchToStartTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 98));
                    Prop12HostSwitchContext = Encoding.ASCII.GetString(ByteStructure, 102, 1);
                    Prop13Colour = Encoding.ASCII.GetString(ByteStructure, 103, 50);
                    Prop14Reserved1 = Encoding.ASCII.GetString(ByteStructure, 153, 1);
                    Prop15Usertype = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 154));
                    Prop16Sequenceno = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 156, 8));// IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 156));
                    Prop17WSClassName = Encoding.ASCII.GetString(ByteStructure, 164, 14);
                    Prop18Brokerstatus = Encoding.ASCII.GetString(ByteStructure, 178, 1);
                    Prop19ShowIndex = Encoding.ASCII.GetString(ByteStructure, 179, 1);
                    Prop20Eligibilty.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 180, 2));
                    Prop21MemberType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 182));
                    Prop22ClearStatus = Encoding.ASCII.GetString(ByteStructure, 184, 1);
                    Prop23BrokerName = Encoding.ASCII.GetString(ByteStructure, 185, 25);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "SignOn Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            #endregion

            #region Constructor
            public SignOn(Int16 transcode)
            {

                strHeader = new MessageHeader(2300);
                strEligibilty = new BrokerEiligibilityPerMarket(false, false, false, false);
                iUserId = 0;
                iUserType = 0;
                iVersionNumber = 0;
                iSequenceNumber = 0;
                iMemberType = 0;
                iBranchId = 0;
                iLastPwdChangeDate = 0;
                iBatchToStartTime = 0;
                cTraderName = new char[26];
                // cTraderName = " ".ToCharArray();
                cPassword = new char[8];
                // cPassword = " ".ToCharArray();
                cNewPassword = new char[8];
                // cNewPassword = " ".ToCharArray();
                cBrokerId = new char[5];
                // cBrokerId = " ".ToCharArray();
                cReserved = new char[1];
                //cReserved = " ".ToCharArray();
                cHostSwitchContext = new char[1];
                //cHostSwitchContext = " ".ToCharArray();
                cColour = new char[50];
                // cColour = " ".ToCharArray();
                cReserve1 = new char[1];
                // cReserve1 = " ".ToCharArray();
                cWSClassName = new char[14];
                // cWSClassName = " ".ToCharArray();
                cBrokerStatus = new char[1];
                // cBrokerStatus = " ".ToCharArray();
                cShowIndex = new char[1];
                // cShowIndex = " ".ToCharArray();
                cClearingStatus = new char[1];
                //  cClearingStatus = " ".ToCharArray();
                cBrokerName = new char[25];
                // cBrokerName = " ".ToCharArray();
            }
            #endregion

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + CConstants.Seperator +
                        Prop02UserId.ToString() + CConstants.Seperator +
                        Prop03Password + CConstants.Seperator +
                        Prop04NewPassword + CConstants.Seperator +
                        Prop05Tradername + CConstants.Seperator +
                        Prop06LastPwdChangeDate.ToString() + CConstants.Seperator +
                    Prop07Brokerid + CConstants.Seperator +
                    Prop08Reserved + CConstants.Seperator +
                    Prop09Branchid.ToString() + CConstants.Seperator +
                    Prop10VersionNumber.ToString() + CConstants.Seperator +
                    Prop11BatchToStartTime.ToString() + CConstants.Seperator +
                    Prop12HostSwitchContext + Prop13Colour + CConstants.Seperator +
                    Prop14Reserved1 + CConstants.Seperator +
                    Prop15Usertype.ToString() + CConstants.Seperator +
                    Prop16Sequenceno.ToString() + CConstants.Seperator +
                    Prop17WSClassName.ToString() + CConstants.Seperator +
                    Prop18Brokerstatus + CConstants.Seperator +
                    Prop19ShowIndex + CConstants.Seperator +
                    Prop20Eligibilty.ToString() + CConstants.Seperator +
                    Prop21MemberType.ToString() + CConstants.Seperator +
                    Prop22ClearStatus + CConstants.Seperator +
                    Prop23BrokerName + CConstants.Seperator);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "SignOn Struture's ToString.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }
        #endregion

        #region System Info
        /// <summary>
        /// System Info Request:     Size(42)   Location(0-41)
        /// /// </summary>
        public struct SystemInfoRequest : IStruct
        {
            /*
             *       Messageheader            38
             *       lastupdatedPortfolio      4
             * =============================================
             *       Total                     42
             * =============================================
             * */
            /// <summary>
            /// Message Header : size(0-38) Location(0-37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// Message Header : size(0-38) Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }

            }
            /// <summary>
            /// Message Header : size(4) Location(38-41)
            /// </summary>
            public Int32 iLastPortFolioUpd;
            /// <summary>
            /// Message Header : size(4) Location(38-41)
            /// </summary>
            public Int32 Prop02LastPortfolioUpd
            {
                get { return iLastPortFolioUpd; }
                set { iLastPortFolioUpd = value; }

            }

            public byte[] StructToByte()
            {
                byte[] systemInfo = new byte[CConstants.SysInfoReq];
                Buffer.BlockCopy(Prop01Header.StructToByte(), 0, systemInfo, 0, 38);
                Buffer.BlockCopy(CUtility.GetBytesFromNumbers(Prop02LastPortfolioUpd), 0, systemInfo, 38, 4);
                return systemInfo;

            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                Prop02LastPortfolioUpd = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 38));
            }
            public SystemInfoRequest(Int32 lastpotupd)
            {
                this.strHeader = new MessageHeader(0);
                strHeader.Prop05TransactionCode = (Int16)CConstants.TranCode.SysInfoRequest;
                strHeader.Prop10MessageLength = (Int16)CConstants.SysInfoReq;
                this.iLastPortFolioUpd = 0;
            }
            public override string ToString()
            {
                return (Prop01Header.ToString() + CConstants.Seperator + Prop02LastPortfolioUpd.ToString() + CConstants.Seperator);

            }

        }
        #endregion

        #region Market Status
        /// <summary>
        /// Market Status : Size(8) location(0-7)
        /// 0=PreOpen (Normal Market Only)
        /// 1=Open
        /// 2=Closed
        /// 3=PreOpened Closed
        /// 4=Post Close
        /// </summary>
        public struct MarketStatus : IStruct
        {

            /// <summary>
            /// SHORT Normal :size(2) Location(0-1)
            /// </summary>
            public Int16 iNormal;
            /// <summary>
            /// SHORT Normal :size(2) Location(0-1)
            /// </summary>
            public Int16 Prop01Normal
            {
                get { return iNormal; }
                set { iNormal = value; }
            }

            /// <summary>
            /// SHORT Oddlot :Size(2) Location(2-3)
            /// </summary>
            public Int16 iOddLot;
            /// <summary>
            /// SHORT Oddlot :Size(2) Location(2-3)
            /// </summary>
            public Int16 Prop02OddLot
            {
                get { return iOddLot; }
                set { iOddLot = value; }
            }

            /// <summary>
            /// SHORT Spot :size(2) Location (4-5)
            /// </summary>
            public Int16 iSpot;
            /// <summary>
            /// SHORT Spot :size(2) Location (4-5)
            /// </summary>
            public Int16 Prop03Spot
            {
                get { return iSpot; }
                set { iSpot = value; }
            }

            /// <summary>
            /// SHORT Auction :Size(2) Location(6-7)
            /// </summary>
            public Int16 iAuction;
            /// <summary>
            /// SHORT Auction :Size(2) Location(6-7)
            /// </summary>
            public Int16 Prop04Auction
            {
                get { return iAuction; }
                set { iAuction = value; }
            }


            public byte[] StructToByte()
            {
                byte[] MktStatus = new byte[8];
                Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iNormal), 0, MktStatus, 0, 2);
                Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iOddLot), 0, MktStatus, 2, 2);
                Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iSpot), 0, MktStatus, 4, 2);
                Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iAuction), 0, MktStatus, 6, 2);
                return MktStatus;
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Normal = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 0));
                    Prop02OddLot = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 2));
                    Prop03Spot = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 4));
                    Prop04Auction = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 6));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "MarketStatus Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            /// <summary>
            /// Market Status
            /// </summary>
            /// <param name="normal">0</param>
            /// <param name="Odd">0</param>
            /// <param name="spot">0</param>
            /// <param name="auction">0</param>
            public MarketStatus(Int16 normal)
            {

                iNormal = 0;
                iSpot = 0;
                iOddLot = 0;
                iAuction = 0;
            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Normal.ToString() +
                        Prop02OddLot.ToString() +
                        Prop03Spot.ToString() +
                        Prop04Auction.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "MarketStatus Struture's ToString.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }

        }
        #endregion

        #region Stock Eligibilty
        /// <summary>
        /// Stockeligibleindicator : Size(2) Location(0-1) Bitwise 
        /// /// </summary>
        public struct StockEligibleIndicator : IStruct
        {

            /*
             *      Reserved :          5 bits
                    Books Merged :      1 bit
                    Minimum Fill :      1 bit
                    AON :               1 bit
                    Reserved :          8 bits
             * =================================================
             *      Total               16 bits (2 byte)
             * =================================================
             * 
             */
            /// <summary>
            /// Books Merged : 1 bit location(00000100)
            /// </summary>
            public bool bolBookMerged;
            /// <summary>
            /// Books Merged : 1 bit location(00000100)
            /// </summary>
            public bool Prop01BookMerged
            {
                get { return bolBookMerged; }
                set { bolBookMerged = value; }
            }

            /// <summary>
            /// Minimum Fill : 1 bit location(00000010)
            /// </summary>
            public bool bolMinimumFill;
            /// <summary>
            ///Minimum Fill : 1 bit location(00000010)
            /// </summary>
            public bool Prop02MinimumFill
            {
                get { return bolMinimumFill; }
                set { bolMinimumFill = value; }
            }

            /// <summary>
            /// AON : 1 bit location(00000001)
            /// </summary>
            public bool bolAON;
            /// <summary>
            /// AON : 1 bit location(00000001)
            /// </summary>
            public bool Prop03AON
            {
                get { return bolAON; }
                set { bolAON = value; }
            }
            /// <summary>
            /// StockEligibility
            /// </summary>
            /// <param name="bookMerged"></param>
            /// <param name="minFill"></param>
            /// <param name="Aon"></param>
            public StockEligibleIndicator(bool bookMerged, bool minFill, bool Aon)
            {

                bolBookMerged = bookMerged;
                bolMinimumFill = minFill;
                bolAON = Aon;


            }

            public byte[] StructToByte()
            {

                byte[] StockEligibilty = new byte[2];
                try
                {
                    byte bfill = Convert.ToByte(0);
                    byte breserve = Convert.ToByte(0);
                    bfill = CUtility.ConvertByteStructureToExchangeBitFormat
                        (false, false,
                         false, false,
                         false, false,
                         false, false,
                         false, false,
                         Prop01BookMerged, true,
                         Prop02MinimumFill, true,
                         Prop03AON, true,
                         bfill);
                    StockEligibilty[0] = bfill;
                    StockEligibilty[1] = breserve;

                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "StockEligibleIndicator Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
                return StockEligibilty;

            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    byte bfill = Convert.ToByte(0);
                    byte breserve = Convert.ToByte(0);
                    bfill = ByteStructure[0];
                    breserve = ByteStructure[1];
                    bool[] temp = CUtility.ConvertExchangeBitFormatToStructure(bfill);
                    Prop01BookMerged = temp[2];
                    Prop02MinimumFill = temp[1];
                    Prop03AON = temp[0];
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "StockEligibleIndicator Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }

            }

            public override string ToString()
            {
                return (Prop01BookMerged.ToString() +
                    Prop02MinimumFill.ToString() +
                    Prop03AON.ToString());

            }
        }
        #endregion

        #region System Response
        /// <summary>
        /// System Info Response Size(104) Location(0-103) 
        /// </summary>
        public struct systemInfoResponse : IStruct
        {
            /*
             * Messageheader                    38  38  0
             * Marketstatus                     8   46  38
             * ExMarketstatus                   8   54  46
             * PLMarketstatus                   8   62  54
             * UpdatePortfolio                  1   63  62
             * MarketIndex                      4   67  63
             * DefaultSettlementPeriod          2   69  67  NormalMkt
             * DefaultSettlementPeriod          2   71  69  spot
             * DefaultSettlementPeriod          2   73  71  Auction
             * CompetitorPeriod                 2   75  73
             * SolicitorPeriod                  2   77  75
             * WarningPercent                   2   79  77
             * VolumeFreezePercent              2   81  79
             * SnapQuoteTime                    2   83  81
             * Reserved                         2   85  83
             * BoardLotQuantity                 4   89  85
             * TickSize                         4   93  89
             * MaximumGtcDays                   2   95  93
             * Stockeligibleindicator           2   97  95
             * DisclosedQuantityPercentAllowed  2   99  97
             * RiskFreeInterestRate             4   103 99
             */

            /// <summary>
            /// Message Header : Size(38) Location(0-37)
            /// </summary> 
            MessageHeader strHeader;
            /// <summary>
            /// Message Header : Size(38) Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            /// <summary>
            /// Marketstatus : Size(8) Location(38-45) 
            /// </summary>
            public MarketStatus strMarketStatus;
            /// <summary>
            /// Property Marketstatus : Size(8)*3 Location(38-61) 
            /// </summary>
            public MarketStatus Prop02InfoMarketResponse
            {
                get { return strMarketStatus; }
                set { strMarketStatus = value; }
            }

            /// <summary>
            /// Marketstatus : Size(8) Location(38-45) 
            /// </summary>
            public MarketStatus strMarketStatusa;
            /// <summary>
            /// Property Marketstatus : Size(8)*3 Location(38-61) 
            /// </summary>
            public MarketStatus Prop02aInfoMarketResponse
            {
                get { return strMarketStatusa; }
                set { strMarketStatusa = value; }
            }

            /// <summary>
            /// Marketstatus : Size(8) Location(38-45) 
            /// </summary>
            public MarketStatus strMarketStatusb;
            /// <summary>
            /// Property Marketstatus : Size(8)*3 Location(38-61) 
            /// </summary>
            public MarketStatus Prop02bnfoMarketResponse
            {
                get { return strMarketStatusb; }
                set { strMarketStatusb = value; }
            }

            /// <summary>
            /// CHAR UpdatePortfolio :size(1) Location(62)
            /// </summary>
            public char[] cUpdPortFolio;
            /// <summary>
            /// CHAR UpdatePortfolio :size(1) Location(62)
            /// </summary>
            public String Prop03UpdatePortfolio
            {
                get { return new String(cUpdPortFolio); }
                set { cUpdPortFolio = CUtility.GetPreciseArrayForString(value.ToUpper(), cUpdPortFolio.Length); }
            }

            /// <summary>
            /// LONG MarketIndex : size(4) Location(63-66) 
            /// </summary>
            public Int32 iMarketIndex;
            /// <summary>
            /// LONG MarketIndex : size(4) Location(63-66) 
            /// </summary>
            public Int32 Prop04MarketIndex
            {
                get { return iMarketIndex; }
                set { iMarketIndex = value; }
            }

            /// <summary>
            /// SHORT DefaultSettlementPeriod(Normal) : size (2) Location(67-68)
            /// </summary>
            public Int16 iSettleNormal;
            /// <summary>
            /// SHORT DefaultSettlementPeriod(Normal) : size (2) Location(67-68)
            /// </summary>
            public Int16 Prop05SettlementNormal
            {
                get { return iSettleNormal; }
                set { iSettleNormal = value; }
            }

            /// <summary>
            /// SHORT DefaultSettlementPeriod (Spot): size(2) Location(69-70)
            /// </summary>
            public Int16 iSettleSpot;
            /// <summary>
            /// SHORT DefaultSettlementPeriod (Spot): size(2) Location(69-70)
            /// </summary>
            public Int16 Prop06SettlementSpot
            {
                get { return iSettleSpot; }
                set { iSettleSpot = value; }
            }

            /// <summary>
            /// SHORT DefaultSettlementPeriod (Auction) :size(2) Location(71-72)
            /// </summary>
            public Int16 iSettleAuction;
            /// <summary>
            /// SHORT DefaultSettlementPeriod (Auction) :size(2) Location(71-72)
            /// </summary>
            public Int16 Prop07SettlementAuction
            {
                get { return iSettleAuction; }
                set { iSettleAuction = value; }
            }

            /// <summary>
            /// SHORT CompetitorPeriod :size(2) Location(73-74)
            /// </summary>
            public Int16 iCompetitorPeriod;
            /// <summary>
            /// SHORT CompetitorPeriod ::size(2) Location(73-74)
            /// </summary>
            public Int16 Prop08CompetitorPeriod
            {
                get { return iCompetitorPeriod; }
                set { iCompetitorPeriod = value; }
            }

            /// <summary>
            /// SHORT SolicitorPeriod :Size(2) Location(75-76)
            /// </summary>
            public Int16 iSolicatorPeriod;
            /// <summary>
            /// SHORT SolicitorPeriod :Size(2) Location(75-76)
            /// </summary>
            public Int16 Prop09SolicatorPeriod
            {
                get { return iSolicatorPeriod; }
                set { iSolicatorPeriod = value; }
            }

            /// <summary>
            /// SHORT WarningPercent : Size(2) Location(77-78)
            /// </summary>
            public Int16 iWarningPercent;
            /// <summary>
            /// SHORT WarningPercent : Size(2) Location(77-78)
            /// </summary>
            public Int16 Prop10WarningPercent
            {
                get { return iWarningPercent; }
                set { iWarningPercent = value; }
            }

            /// <summary>
            /// SHORT VolumeFreezePercent: size(2) Location(79-80)
            /// </summary>
            public Int16 iVolumneFreezePercent;
            /// <summary>
            /// SHORT VolumeFreezePercent: size(2) Location(79-80)
            /// </summary>
            public Int16 Prop11VolumneFreezePercent
            {
                get { return iVolumneFreezePercent; }
                set { iVolumneFreezePercent = value; }
            }

            /// <summary>
            /// SHORT SnapQuoteTime :size(2) Location(81-82)
            /// </summary>
            public Int16 iSnapQuoteTime;
            public Int16 Prop12SnapQuoteTime
            {
                get { return iSnapQuoteTime; }
                set { iSnapQuoteTime = value; }
            }

            /// <summary>
            /// SHORT Reserved :size(2) Loaction(83-84)
            /// </summary>
            public Int16 iReserve;
            /// <summary>
            /// SHORT Reserved :size(2) Loaction(83-84)
            /// </summary>
            public Int16 Prop13Reserved
            {
                get { return iReserve; }
                set { iReserve = value; }
            }

            /// <summary>
            /// LONG BoardLotQuantity :size(4) Loaction(85-88)
            /// </summary>
            public Int32 iBroadLotQty;
            /// <summary>
            /// LONG BoardLotQuantity :size(4) Loaction(85-88)
            /// </summary>
            public Int32 Prop14BroadLotQty
            {
                get { return iBroadLotQty; }
                set { iBroadLotQty = value; }
            }

            /// <summary>
            /// LONG TickSize : Size(4) Location (89-92)
            /// </summary>
            public Int32 iTickSize;
            /// <summary>
            /// LONG TickSize : Size(4) Location (89-92)
            /// </summary>
            public Int32 Prop15TickSize
            {
                get { return iTickSize; }
                set { iTickSize = value; }
            }

            /// <summary>
            ///  SHORT MaximumGtcDays : Size(2) Location(93-94)
            /// </summary>
            public Int32 iMaxGtcDate;
            /// <summary>
            /// SHORT MaximumGtcDays : Size(2) Location(93-94)
            /// </summary>
            public Int32 Prop16MaxGtcDate
            {
                get { return iMaxGtcDate; }
                set { iMaxGtcDate = value; }
            }

            /// <summary>
            /// Struct STSTOCKELIGIBLEINDICATORS : size(2) Location (95-96)
            /// </summary>
            public StockEligibleIndicator strStockEligibilty;
            /// <summary>
            /// Struct STSTOCKELIGIBLEINDICATORS : size(2) Location (95-96)
            /// </summary>
            public StockEligibleIndicator Prop17StockEligibilty
            {
                get { return strStockEligibilty; }
                set { strStockEligibilty = value; }
            }

            /// <summary>
            /// SHORT DisclosedQuantityPercentAllowed : size(2) Location(97-98)
            /// </summary>
            public Int16 iDisclosedQtyPerAllowed;
            /// <summary>
            /// SHORT DisclosedQuantityPercentAllowed : size(2) Location(97-98)
            /// </summary>
            public Int16 Prop18DisClosedQtyPerAllowed
            {
                get { return iDisclosedQtyPerAllowed; }
                set { iDisclosedQtyPerAllowed = value; }
            }

            /// <summary>
            /// LONG RiskFreeInterestRate : Size(4) Location(99-103)
            /// </summary>
            public Int32 iRiskFreeIntRate;
            /// <summary>
            /// LONG RiskFreeInterestRate : size(4) Location()
            /// </summary>
            public Int32 Prop19RiskFreeIntRate
            {
                get { return iRiskFreeIntRate; }
                set { iRiskFreeIntRate = value; }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02InfoMarketResponse.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 38, 8));
                    Prop02aInfoMarketResponse.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 46, 8));
                    Prop02bnfoMarketResponse.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 54, 8));
                    Prop03UpdatePortfolio = Encoding.ASCII.GetString(ByteStructure, 62, 1);
                    Prop04MarketIndex = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 63));
                    Prop05SettlementNormal = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 67));
                    Prop06SettlementSpot = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 69));
                    Prop07SettlementAuction = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 71));
                    Prop08CompetitorPeriod = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 73));
                    Prop09SolicatorPeriod = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 75));
                    Prop10WarningPercent = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 77));
                    Prop11VolumneFreezePercent = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 79));
                    Prop12SnapQuoteTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 81));
                    Prop13Reserved = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 83));
                    Prop14BroadLotQty = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 85));
                    Prop15TickSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 89));
                    Prop16MaxGtcDate = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 93));
                    Prop17StockEligibilty.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 95, 2));
                    Prop18DisClosedQtyPerAllowed = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 97));
                    Prop19RiskFreeIntRate = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 99));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            public byte[] StructToByte()
            {
                return null;

            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + CConstants.Seperator +
                        Prop02InfoMarketResponse.ToString() + CConstants.Seperator +
                        Prop02aInfoMarketResponse.ToString() + CConstants.Seperator +
                        Prop02bnfoMarketResponse.ToString() + CConstants.Seperator +
                        Prop03UpdatePortfolio + CConstants.Seperator +
                        Prop04MarketIndex.ToString() + CConstants.Seperator +
                        Prop05SettlementNormal.ToString() + CConstants.Seperator +
                        Prop06SettlementSpot.ToString() + CConstants.Seperator +
                                       Prop07SettlementAuction.ToString() + CConstants.Seperator +
                                       Prop08CompetitorPeriod.ToString() + CConstants.Seperator +
                                       Prop09SolicatorPeriod.ToString() + CConstants.Seperator +
                                       Prop10WarningPercent.ToString() + CConstants.Seperator +
                                       Prop11VolumneFreezePercent.ToString() + CConstants.Seperator +
                                       Prop12SnapQuoteTime.ToString() + CConstants.Seperator +
                                       Prop13Reserved.ToString() + CConstants.Seperator +
                                       Prop14BroadLotQty.ToString() + CConstants.Seperator +
                                       Prop15TickSize.ToString() + CConstants.Seperator +
                                       Prop16MaxGtcDate.ToString() + CConstants.Seperator +
                                       Prop17StockEligibilty.ToString() + CConstants.Seperator +
                                       Prop18DisClosedQtyPerAllowed.ToString() + CConstants.Seperator +
                                       Prop19RiskFreeIntRate.ToString() + CConstants.Seperator);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

            public systemInfoResponse(Int16 transcode)
            {
                strHeader = new MessageHeader(transcode);
                strMarketStatus = new MarketStatus(0);
                strMarketStatusa = new MarketStatus(0);
                strMarketStatusb = new MarketStatus(0);
                cUpdPortFolio = new char[1];
                iMarketIndex = 0;
                iSettleNormal = 0;
                iSettleSpot = 0;
                iSettleAuction = 0;
                iCompetitorPeriod = 0;
                iSolicatorPeriod = 0;
                iWarningPercent = 0;
                iVolumneFreezePercent = 0;
                iSnapQuoteTime = 0;
                iReserve = 0;
                iBroadLotQty = 0;
                iTickSize = 0;
                iMaxGtcDate = 0;
                strStockEligibilty = new StockEligibleIndicator(false, false, false);
                iDisclosedQtyPerAllowed = 0;
                iRiskFreeIntRate = 0;
            }


        }

        #endregion

        #region Local Database Download Request

        /// <summary>
        /// Update Local Database Request : size(80) Location(0-79)
        /// </summary>
        public struct UpdateLocalDB : IStruct
        {
            /*
             * Messagestructure             38  38
             * LastUpdateSecurityTime       4   42  
             * LastUpdateParticipantTime    4   46  
             * LastUpdateInstrumentTime     4   50  
             * LastUpdateIndexTime          4   54
             * RequestForOpenOrders         1   55
             * Reserved                     1   56
             * STRUCT STMARKETSTATUS        8   64
             * STRUCT STEXMARKETSTATUS      8   72
             * STRUCT STPLMARKETSTATUS      8   80
             * ============================================
             * Total                        80
             * ============================================
             */
            /// <summary>
            /// STRUCT MESSAGEHEADER : Size(38) Location(0-37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// STRUCT MESSAGEHEADER : Size(38) Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }

            /// <summary>
            /// LONG LastUpdateSecurityTime : Size(4) Location(38-41)
            /// =0 For Complete Download 
            /// </summary>
            public Int32 iLastUpdSecurityTime;
            /// <summary>
            /// LONG LastUpdateSecurityTime : Size(4) Location(38-41)
            /// </summary>
            public Int32 Prop02LastUpdSecurityTime
            {
                get { return iLastUpdSecurityTime; }
                set { iLastUpdSecurityTime = value; }
            }

            /// <summary>
            /// LONG LastUpdateParticipantTime : Size(4) Location(42-45)
            /// =0 For Complete Download 
            /// </summary>
            public Int32 iLastUpdParticipantTime;
            /// <summary>
            ///  LONG LastUpdateParticipantTime : Size(4) Location(42-45)
            /// </summary>
            public Int32 Prop03LastParticipantTime
            {
                get { return iLastUpdParticipantTime; }
                set { iLastUpdParticipantTime = value; }
            }

            /// <summary>
            ///  LONG LastUpdateInstrumentTime : Size(4) Location(46-49)
            /// </summary>
            public Int32 iLastUpdInstrumentTime;
            /// <summary>
            ///  LONG LastUpdateParticipantTime : Size(4) Location(46-49)
            /// </summary>
            public Int32 Prop04LastUpdInstrumentTime
            {
                get { return iLastUpdInstrumentTime; }
                set { iLastUpdInstrumentTime = value; }
            }

            /// <summary>
            ///  LONG LastUpdateIndexTime : Size(4) Location(50-53)
            /// </summary>
            public Int32 iLastUpdIndextime;
            /// <summary>
            ///  LONG LastUpdateIndexTime: Size(4) Location(50-53)
            /// </summary>
            public Int32 Prop05LastUpdIndexTime
            {
                get { return iLastUpdIndextime; }
                set { iLastUpdIndextime = value; }
            }

            /// <summary>
            /// CHAR RequestForOpenOrders : size(1)  Location(54)
            /// = G For GTC and GTD and =N Otherwise
            /// </summary>
            public char[] cRequestForOpenOrder;
            /// <summary>
            /// CHAR RequestForOpenOrders : size(1)  Location(54)
            /// </summary>
            public String Prop06RequestForOpenOrder
            {
                get { return new string(cRequestForOpenOrder); }
                set { cRequestForOpenOrder = CUtility.GetPreciseArrayForString(value.ToUpper(), cRequestForOpenOrder.Length); }
            }

            /// <summary>
            /// CHAR Reserved : size(1) Location(55)
            /// </summary>
            public char[] cReserved;
            /// <summary>
            /// CHAR Reserved : size(1) Location(55)
            /// </summary>
            public String Prop07Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToUpper(), cReserved.Length); }
            }

            /// <summary>
            /// STRUCT STMARKETSTATUS : size(8) Location(56-63)
            /// </summary>
            public MarketStatus strLocalDbStatus;
            /// <summary>
            /// STRUCT STMARKETSTATUS : size(8) Location(56-63)
            /// </summary>
            public MarketStatus Prop08LocalDbStatus
            {
                get { return strLocalDbStatus; }
                set { strLocalDbStatus = value; }
            }

            /// <summary>
            /// STRUCT STEXMARKETSTATUS : size(8) Location(64-71)
            /// </summary>
            public MarketStatus strLocalDbExStatus;
            /// <summary>
            /// STRUCT STEXMARKETSTATUS : size(8) Location(64-71)
            /// </summary>
            public MarketStatus Prop09LocalDbExStatus
            {
                get { return strLocalDbExStatus; }
                set { strLocalDbExStatus = value; }
            }

            /// <summary>
            /// STRUCT STPLMARKETSTATUS : size(8) Location(72-79)
            /// </summary>
            public MarketStatus strLocalDbPlStatus;
            /// <summary>
            /// STRUCT STPLMARKETSTATUS : size(8) Location(72-79)
            /// </summary>
            public MarketStatus Prop10LocalDbPlStatus
            {
                get { return strLocalDbPlStatus; }
                set { strLocalDbPlStatus = value; }
            }

            public byte[] StructToByte()
            {
                byte[] DbHeader = new byte[CConstants.LocalDBUpdateReqSize];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, DbHeader, 0, 38);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastUpdSecurityTime), 0, DbHeader, 38, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastUpdParticipantTime), 0, DbHeader, 42, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastUpdInstrumentTime), 0, DbHeader, 46, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastUpdIndextime), 0, DbHeader, 50, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cRequestForOpenOrder), 0, DbHeader, 54, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserved), 0, DbHeader, 55, 1);
                    Buffer.BlockCopy(strLocalDbStatus.StructToByte(), 0, DbHeader, 56, 8);
                    Buffer.BlockCopy(strLocalDbExStatus.StructToByte(), 0, DbHeader, 64, 8);
                    Buffer.BlockCopy(strLocalDbPlStatus.StructToByte(), 0, DbHeader, 72, 8);

                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
                return DbHeader;

            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02LastUpdSecurityTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 38));
                    Prop03LastParticipantTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 42));
                    Prop04LastUpdInstrumentTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 46));
                    Prop05LastUpdIndexTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 50));
                    Prop06RequestForOpenOrder = Encoding.ASCII.GetString(ByteStructure, 54, 1);
                    Prop07Reserved = Encoding.ASCII.GetString(ByteStructure, 55, 1);
                    Prop08LocalDbStatus.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 56, 8));
                    Prop09LocalDbExStatus.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 64, 8));
                    Prop10LocalDbPlStatus.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 72, 8));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }

            public UpdateLocalDB(Int16 Transcode)
            {

                strHeader = new MessageHeader(7300);
                strHeader.Prop05TransactionCode = (Int16)CConstants.TranCode.LocalDBUpdRequest;
                strHeader.Prop10MessageLength = (Int16)CConstants.LocalDBUpdateReqSize;
                iLastUpdSecurityTime = 0;
                iLastUpdInstrumentTime = 0;
                iLastUpdParticipantTime = 0;
                iLastUpdIndextime = 0;
                cRequestForOpenOrder = new char[1];
                cRequestForOpenOrder = "G".ToCharArray();
                cReserved = new char[1];
                cReserved = " ".ToCharArray();
                strLocalDbStatus = new MarketStatus(0);
                strLocalDbPlStatus = new MarketStatus(0);
                strLocalDbExStatus = new MarketStatus(0);
            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + CConstants.Seperator +
                            Prop02LastUpdSecurityTime.ToString() + CConstants.Seperator +
                            Prop03LastParticipantTime.ToString() + CConstants.Seperator +
                            Prop04LastUpdInstrumentTime.ToString() + CConstants.Seperator +
                            Prop05LastUpdIndexTime.ToString() + CConstants.Seperator +
                            Prop06RequestForOpenOrder + CConstants.Seperator +
                            Prop07Reserved + CConstants.Seperator +
                            Prop08LocalDbStatus.ToString() + CConstants.Seperator +
                            Prop09LocalDbExStatus.ToString() + CConstants.Seperator +
                            Prop10LocalDbPlStatus.ToString() + CConstants.Seperator);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }


        }
        #endregion

        #region Local Database Header
        /// <summary>
        /// Local Database header : size(40) Location (0-39)
        /// </summary>
        /*
         * MESSAGEHEADER       :        38
           CHAR Reserved [2]    :        2
         * -----------------------------------------
         *  Total               :       40
         * -----------------------------------------
         */
        public struct DownloadDbHeader : IStruct
        {
            /// <summary>
            ///  Message Header : size(38) location(0-37)
            /// </summary>
            MessageHeader strHeader;
            /// <summary>
            /// Message Header : size(38) locayion(0-37)
            /// Transaction Code :7307
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }

            }

            /// <summary>
            /// CHAR Reserved [2] :Size(2) Location(38-39)
            /// </summary>
            char[] cReserve;
            /// <summary>
            /// CHAR Reserved [2] :Size(2) Location(38-39)
            /// </summary>
            public String Prop02Reserve
            {
                get { return new string(cReserve); }
                set { cReserve = CUtility.GetPreciseArrayForString(value.ToString(), cReserve.Length); }
            }

            public byte[] StructToByte()
            {
                byte[] dloadheader = new byte[CConstants.LocalDBUpdateHeaderSize];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, dloadheader, 0, 38);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve), 0, dloadheader, 38, 2);
                    return dloadheader;

                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02Reserve = Encoding.ASCII.GetString(ByteStructure, 38, 2);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }
            public DownloadDbHeader(Int16 transcode)
            {

                strHeader = new MessageHeader(7307);
                cReserve = new char[2];
            }
            public override string ToString()
            {

                return (Prop01Header.ToString() + Prop02Reserve);

            }

        }
        #endregion

        #region Local Database Data
        /// <summary>
        /// Local Database Data : size(512) Location (0-511)
        /// </summary>
        /*
         * MESSAGEHEADER             :        38
           CHAR Reserved [436+38]    :        474
         * -----------------------------------------
         *  Total               :              512
         * -----------------------------------------
         */
        public struct DownloadDbData : IStruct
        {
            /// <summary>
            ///  Message Header : size(38) location(0-37)
            /// </summary>
            MessageHeader strHeader;
            /// <summary>
            /// Message Header : size(38) location(0-37)
            /// Transaction Code :7304
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }

            }

            /// <summary>
            /// CHAR Reserved [2] :Size(2) Location(38-39)
            /// </summary>
            Byte[] bData;
            /// <summary>
            /// CHAR Data [474] :Size(474) Location(38-711)
            /// </summary>
            public Byte[] Prop02Data
            {
                get { return bData; }
                set { bData = value; }
            }
            public byte[] StructToByte()
            {
                byte[] dloaddata = new byte[CConstants.LocalDBData];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, dloaddata, 0, 38);
                    Buffer.BlockCopy(bData, 0, dloaddata, CConstants.MsgHeaderSize, bData.Length);
                    return dloaddata;

                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Array.Resize(ref bData, ByteStructure.Length - CConstants.MsgHeaderSize);
                    Array.Copy(ByteStructure, CConstants.MsgHeaderSize, bData, 0, ByteStructure.Length - CConstants.MsgHeaderSize);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }
            public DownloadDbData(Int16 transcode)
            {
                strHeader = new MessageHeader(7304);
                bData = new byte[474];

            }
            public override string ToString()
            {
                return (Prop01Header.ToString() + CConstants.Seperator + Encoding.ASCII.GetString(Prop02Data) + CConstants.Seperator);
            }
        }

        #region Message Download request

        /// <summary>
        /// Message Download Request : size(46) location(0-45)
        /// </summary>
        public struct MessageDownloadRequest : IStruct
        {
            /// <summary>
            /// Message header : size(38) Location(0-37)
            /// </summary>
            MessageHeader strHeader;
            /// <summary>
            /// Message header : size(38) Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }

            }

            /// <summary>
            /// Double Sequence Number : size(8) location(38-45)
            /// </summary>
            public Double iSequenceNo;
            /// <summary>
            /// Double Sequence Number : size(8) location(38-45)
            /// To receive Message from begining of trading Day Set sequnceNo=0 or SeqNo from Login Response
            /// </summary>
            public Double Prop02SequenceNo
            {
                get { return iSequenceNo; }
                set { iSequenceNo = value; }
            }
            public byte[] StructToByte()
            {
                byte[] msgdloadheader = new byte[CConstants.MsgDownloadReqSize];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, msgdloadheader, 0, 38);
                    Buffer.BlockCopy(CUtility.HostToNetworkLong(iSequenceNo), 0, msgdloadheader, 38, 8);
                    return msgdloadheader;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02SequenceNo = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 38, 8));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public MessageDownloadRequest(Int64 seqno)
            {

                strHeader = new MessageHeader(0);
                strHeader.Prop05TransactionCode = (Int16)CConstants.TranCode.DeltaDownloadReq;
                strHeader.Prop10MessageLength = (Int16)CConstants.MsgDownloadReqSize;
                iSequenceNo = 0;
            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() +
                        Prop02SequenceNo.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

        }

        #endregion

        #region Download Index    TransactionCode=7325
        public struct IndexDetails : IStruct
        {
            public char[] cIndexName;
            public string Prop01IndexName
            {
                get { return new string(cIndexName); }
                set { cIndexName = CUtility.GetPreciseArrayForString(value.ToUpper(), cIndexName.Length); }
            }

            public Int32 iToken;
            public Int32 Prop02Token
            {
                get { return iToken; }
                set { iToken = value; }
            }

            public Int32 iLastUpdateDateTime;
            public Int32 Prop03LastUpdDateTime
            {
                get { return iLastUpdateDateTime; }
                set { iLastUpdateDateTime = value; }

            }

            public byte[] StructToByte()
            { return null; }
            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01IndexName = Encoding.ASCII.GetString(ByteStructure, 0, 16);
                Prop02Token = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 16));
                Prop03LastUpdDateTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 20));
            }
            public override string ToString()
            {
                return (Prop01IndexName +
                    Prop02Token.ToString() +
                    Prop03LastUpdDateTime.ToString()
                    );
            }
            public IndexDetails(Int16 indexLen)
            {
                cIndexName = new char[16];
                iToken = 0;
                iLastUpdateDateTime = 0;
            }

        }
        public struct DownloadIndex : IStruct
        {
            public MessageHeader strHeader;
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            public Int16 iNoofRecord;
            public Int16 Prop02NoOfRecords
            {
                get { return iNoofRecord; }
                set { iNoofRecord = value; }
            }
            public Dictionary<Int32, IndexDetails> dicIndexDet;
            public Dictionary<Int32, IndexDetails> Prop03IndexDetails
            {
                get { return dicIndexDet; }
                set { dicIndexDet = value; }
            }

            public DownloadIndex(Int16 Transcode)
            {
                strHeader = new MessageHeader(0);
                iNoofRecord = 0;
                dicIndexDet = new Dictionary<int, IndexDetails>();
            }
            public byte[] StructToByte()
            { return null; }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02NoOfRecords = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                    for (int idx = 0; idx < iNoofRecord; idx++)
                    {
                        IndexDetails IndexData = new IndexDetails(0);
                        IndexData.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 40 + (idx * 24), 24));
                        Prop03IndexDetails.Add(IndexData.Prop02Token, IndexData);
                    }
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }
            public override string ToString()
            {
                try
                {
                    string str = "";
                    foreach (KeyValuePair<Int32, IndexDetails> kvp in dicIndexDet)
                    {
                        str = str + kvp.Value.ToString();
                    }
                    return (str);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }

        }
        #endregion



        #endregion

        #region Exchange Portfolio Request
        /// <summary>
        /// Exchange Portfolio Request : size(42) Location (0-41)
        /// </summary>
        /*
         * MESSAGEHEADER       :        38
           LastUpdateDtTime    :        4
         * -----------------------------------------
         *  Total               :       42
         * -----------------------------------------
         */
        public struct ExchangePortfolioRequest : IStruct
        {
            /// <summary>
            ///  Message Header : size(38) location(0-37)
            /// </summary>
            MessageHeader strHeader;
            /// <summary>
            /// Message Header : size(38) locayion(0-37)
            /// Transaction Code :7307
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }

            }

            /// <summary>
            /// LastUpdateDtTime        : Size(4)   Location(38-41)
            /// </summary>
            Int32 iLastUpdateDtTime;
            /// <summary>
            /// LastUpdateDtTime        : Size(4)   Location(38-41)
            /// </summary>
            public Int32 Prop02LastUpdateDtTime
            {
                get { return iLastUpdateDtTime; }
                set { iLastUpdateDtTime = value; }
            }


            public byte[] StructToByte()
            {

                byte[] bExchPortfolioReq = new byte[CConstants.ExchPortfolioRequest];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, bExchPortfolioReq, 0, 38);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastUpdateDtTime), 0, bExchPortfolioReq, 38, 4);
                    return bExchPortfolioReq;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02LastUpdateDtTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 38));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }
            public ExchangePortfolioRequest(Int16 Transcode)
            {

                strHeader = new MessageHeader(0);
                iLastUpdateDtTime = 0;
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + Prop02LastUpdateDtTime.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

        }
        #endregion

        #region Exchange Portfolio Response
        /// <summary>
        /// Exchange Portfolio Response : size(327) Location (0-326)
        ///                               42+285=327
        /// </summary>
        /*
         * MESSAGEHEADER            :        38     38
           SHORT NoOfRecords        :        2      40
         * CHAR MoreRecords         :        1      41
         * CHAR Filler              :        1      42
         * struct PORTFOLIO_DATA    :        25 * NoOfRecords
         * -----------------------------------------
         *  Total                   :        327
         * -----------------------------------------
         */


        #region PortFolioData
        /*
         *  CHAR Portfolio [10]     :   10
            LONG Token              :   4
            LONG c                  :   4    
            CHAR DeleteFlag         :   1
         * =======================================
         *  Total                   :   (25)    
         * =======================================
         * 
         */
        public struct PortFolioData : IStruct
        {
            /// <summary>
            ///CHAR Portfolio [10]     :   size(10) Location(0-9)
            /// </summary>
            public char[] cPortfolio;
            /// <summary>
            ///CHAR Portfolio [10]     :   size(10) Location(0-9)
            /// </summary>
            public string Prop01Portfolio
            {
                get { return new string(cPortfolio); }
                set { cPortfolio = CUtility.GetPreciseArrayForString(value.ToString(), cPortfolio.Length); }
            }

            /// <summary>
            /// LONG Token              :   size(4)   Location(10-13)
            /// </summary>
            public Int32 iToken;
            /// <summary>
            /// LONG Token              :   size(4)   Location(10-13)
            /// </summary>
            public Int32 Prop02Token
            {
                get { return iToken; }
                set { iToken = value; }
            }

            /// <summary>
            ///  LONG LastUpdtDtTime     : Size(4    ) Location(14-17)
            /// </summary>
            public Int32 iLastUpdtDtTime;
            /// <summary>
            ///  LONG LastUpdtDtTime     : Size(4    ) Location(14-17)
            /// </summary>
            public Int32 Prop03LastUpdtDtTime
            {
                get { return iLastUpdtDtTime; }
                set { iLastUpdtDtTime = value; }
            }

            /// <summary>
            ///CHAR DeleteFlag [1]     :   size(1) Location(18)
            /// </summary>
            public char[] cDeleteFlag;
            /// <summary>
            ///CHAR DeleteFlag [1]     :   size(1) Location(18)
            /// </summary>
            public string Prop04DeleteFlag
            {
                get { return new string(cDeleteFlag); }
                set { cDeleteFlag = CUtility.GetPreciseArrayForString(value.ToString(), cDeleteFlag.Length); }
            }

            public byte[] StructToByte()
            {
                byte[] bPortfoliodata = new byte[CConstants.PortFolioData];
                try
                {
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cPortfolio), 0, bPortfoliodata, 0, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iToken), 0, bPortfoliodata, 10, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastUpdtDtTime), 0, bPortfoliodata, 14, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cDeleteFlag), 0, bPortfoliodata, 18, 1);
                    return bPortfoliodata;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }


            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Portfolio = Encoding.ASCII.GetString(ByteStructure, 0, 10);
                    Prop02Token = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 10));
                    Prop03LastUpdtDtTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 14));
                    Prop04DeleteFlag = Encoding.ASCII.GetString(ByteStructure, 18, 1);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public PortFolioData(Int16 Transcode)
            {

                cPortfolio = new char[10];
                iToken = 0;
                iLastUpdtDtTime = 0;
                cDeleteFlag = new char[1];

            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Portfolio + Prop02Token.ToString() + Prop03LastUpdtDtTime.ToString() + Prop04DeleteFlag);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

        }
        #endregion

        public struct ExchangePortfolioResponse : IStruct
        {
            /// <summary>
            ///  Message Header : size(38) location(0-37)
            /// </summary>
            MessageHeader strHeader;
            /// <summary>
            /// Message Header : size(38) locayion(0-37)
            /// Transaction Code :1776
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }

            }

            /// <summary>
            ///  SHORT NoOfRecords        : size(2)   Location(38-39)
            /// </summary>
            Int16 iNoOfRecords;
            /// <summary>
            ///SHORT NoOfRecords        : size(2)   Location(38-39)
            /// </summary>
            public Int16 Prop02NoOfRecords
            {
                get { return iNoOfRecords; }
                set { iNoOfRecords = value; }
            }

            /// <summary>
            /// CHAR MoreRecords         :size(1) Location(40)
            /// </summary>
            public char[] CMoreRecords;
            /// <summary>
            /// CHAR MoreRecords         :size(1) Location(40)
            /// </summary>
            public String Prop03MoreRecords
            {
                get { return new string(CMoreRecords); }
                set { CMoreRecords = CUtility.GetPreciseArrayForString(value.ToString(), CMoreRecords.Length); }
            }

            /// <summary>
            /// CHAR Filler              : size(1) Location(41)
            /// </summary>
            public char[] CFiller;
            /// <summary>
            /// CHAR Filler              : size(1) Location(41)
            /// </summary>
            public String Prop04Filler
            {
                get { return new string(CFiller); }
                set { CFiller = CUtility.GetPreciseArrayForString(value.ToString(), CFiller.Length); }
            }

            public Dictionary<Int32, PortFolioData> dPortFolioData;
            public Dictionary<Int32, PortFolioData> Prop05PortFolioData
            {
                get { return dPortFolioData; }
                set { dPortFolioData = value; }
            }

            public byte[] StructToByte()
            {
                byte[] ExchangeStruct = new byte[0];
                try
                {
                    return ExchangeStruct;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }


            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02NoOfRecords = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                    Prop03MoreRecords = Encoding.ASCII.GetString(ByteStructure, 40, 1);
                    Prop04Filler = Encoding.ASCII.GetString(ByteStructure, 41, 1);
                    for (int idx = 0; idx < Prop02NoOfRecords; idx++)
                    {
                        PortFolioData strportfoliodata = new PortFolioData(0);
                        strportfoliodata.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 42 + (idx * 19), 19));
                        Prop05PortFolioData.Add(strportfoliodata.Prop02Token, strportfoliodata);
                        //strportfoliodata.Prop01Portfolio = Encoding.ASCII.GetString(ByteStructure, 42 + (idx * 25), 10);
                        //strportfoliodata.Prop02Token = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 52 + (idx * 25)));
                        //strportfoliodata.Prop03LastUpdtDtTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 56 + (idx * 25)));
                        //strportfoliodata.Prop01Portfolio = Encoding.ASCII.GetString(ByteStructure, 60 + (idx * 25), 1);

                    }
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public ExchangePortfolioResponse(Int16 Transcode)
            {
                strHeader = new MessageHeader(0);
                iNoOfRecords = 0;
                CMoreRecords = new char[1];
                CFiller = new char[1];
                dPortFolioData = new Dictionary<Int32, PortFolioData>();

            }

            public override string ToString()
            {
                return (Prop01Header.ToString() + Prop02NoOfRecords.ToString() + Prop03MoreRecords + Prop04Filler + Prop05PortFolioData.ToString());

            }

        }
        #endregion

        #region Message Download data
        /// <summary>
        /// Message download data : size(max=512 Min=76)
        /// 
        /// </summary>
        public struct ResponseData : IStruct
        {
            /// <summary>
            /// Message Header :size(38) Location(0-37)
            /// </summary>
            MessageHeader strHeaderOuter;
            /// <summary>
            /// Message Header :size(38) Location(0-37)
            /// Transaction Code:7021
            /// </summary>
            public MessageHeader Prop01OuterHeader
            {
                get { return strHeaderOuter; }
                set { strHeaderOuter = value; }

            }
            /// <summary>
            /// Message Header :size(38) Location(38-75)
            /// </summary>
            public byte[] bdata;
            /// <summary>
            /// Data :size(max=474) Location(38-511)
            /// </summary>
            public Byte[] Prop02Data
            {
                get { return bdata; }
                set { bdata = value; }

            }

            public byte[] StructToByte()
            { return null; }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01OuterHeader.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Array.Resize(ref bdata, ByteStructure.Length - CConstants.MsgHeaderSize);
                    Array.Copy(ByteStructure, 38, bdata, 0, ByteStructure.Length - CConstants.MsgHeaderSize);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public override string ToString()
            {

                return (Prop01OuterHeader.ToString() + Prop02Data.ToString());
            }
            public ResponseData(int transcode)
            {
                strHeaderOuter = new MessageHeader(0);
                bdata = new byte[0];
            }

        }

        #endregion

        #region SignOff Confirmation Response
        /// <summary>
        /// Signoff Out: Size(185) Location(0-184)
        /// </summary>
        public struct SignOffOut : IStruct
        {
            /*
             * MESSAGEHEADER       : 38    0
             * SHORT UserId         : 2    38
             * Reserved             : 145   40
             * ---------------------------------------------
             * Total                  185
             * ---------------------------------------------
             */


            /// <summary>
            /// Message header: Size(38) Location(0-37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// Message header: Size(38) Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }

            }

            /// <summary>
            /// UserId: Size(2) Location(38-39)
            /// </summary>
            public Int16 iUserId;
            /// <summary>
            /// UserId: Size(2) Location(38-39)
            /// </summary>
            public Int16 Prop02UserId
            {
                get { return iUserId; }
                set { iUserId = value; }
            }

            /// <summary>
            /// Reserve: Size(145) Location(40-184)
            /// </summary>
            public Char[] cReserve;
            /// <summary>
            /// Reserve: Size(145) Location(40-184)
            /// </summary>
            public string Prop03Reserve
            {
                get { return new String(cReserve); }
                set { cReserve = CUtility.GetPreciseArrayForString(value.ToString(), cReserve.Length); }

            }
            public byte[] StructToByte()
            {
                byte[] Lofoff = new byte[CConstants.SignOff];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, Lofoff, 0, 38);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iUserId), 0, Lofoff, 38, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve), 0, Lofoff, 40, 145);
                    return Lofoff;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02UserId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                    Prop03Reserve = Encoding.ASCII.GetString(ByteStructure, 40, 145);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            public SignOffOut(int Transcode)
            {
                strHeader = new MessageHeader(0);
                iUserId = 0;
                cReserve = new char[145];
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + Prop02UserId.ToString() + Prop03Reserve);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }
        }

        #endregion

        #region All Order Structures

        #region Contract Description
        /// <summary>
        /// Contract description : Size(28) Location(0-27)
        /// </summary>
        public struct CONTRACTDESC : IStruct
        {
            /*
             *  CHAR    InstrumentName [6]      6       0
                CHAR    Symbol [10]             10      6
                LONG    ExpiryDate              4       16
                LONG    StrikePrice             4       20
                CHAR    OptionType [2]          2       24
                SHORT   CALevel                 2       26
             * ============================================== 
             * Total                            28
             * ==============================================
             */
            /// <summary>
            /// CHAR    InstrumentName [6] :size(6) Location(0-5)
            /// </summary>
            char[] cInstrumentName;
            /// <summary>
            /// CHAR    InstrumentName [6] :size(6) Location(0-5)
            /// </summary>
            public String Prop01InstrumentName
            {
                get { return new String(cInstrumentName); }
                set { cInstrumentName = CUtility.GetPreciseArrayForString(value.ToString(), cInstrumentName.Length); }
            }
            /// <summary>
            /// CHAR    Symbol [10] : size(10) Location(6-15)
            /// </summary>
            char[] cSymbol;
            /// <summary>
            /// CHAR    Symbol [10] : size(10) Location(6-15)
            /// </summary>
            public String Prop02Symbol
            {
                get { return new String(cSymbol); }
                set { cSymbol = CUtility.GetPreciseArrayForString(value.ToString(), cSymbol.Length); }

            }

            /// <summary>
            /// LONG    ExpiryDate : size(4) location(16-19)
            /// </summary>
            public Int32 iExpiryDate;
            ///    /// <summary>
            /// LONG    ExpiryDate : size(4) location(16-19)
            /// </summary>
            public Int32 Prop03ExpiryDate
            {
                get { return iExpiryDate; }
                set { iExpiryDate = value; }
            }

            /// <summary>
            /// LONG    StrikePrice: size(4)  Location(20-23)  
            /// </summary>
            public Int32 iStrikePrice;
            /// <summary>
            /// LONG    StrikePrice: size(4)  Location(20-23)  
            /// </summary>
            public Int32 Prop04StrikePrice
            {
                get { return iStrikePrice; }
                set { iStrikePrice = value; }
            }

            /// <summary>
            ///  CHAR    OptionType [2]: Size(2)   Location(24-25) 
            /// </summary>
            public char[] cOptionType;
            /// <summary>
            ///  CHAR    OptionType [2]: Size(2)   Location(24-25) 
            /// </summary>
            public String Prop05OptionType
            {
                get { return new String(cOptionType); }
                set { cOptionType = CUtility.GetPreciseArrayForString(value.ToString(), cOptionType.Length); }

            }

            /// <summary>
            /// SHORT   CALevel  : size(2) Location (26-27)
            /// </summary>
            public Int16 iCALevel;
            /// <summary>
            /// SHORT   CALevel  : size(2) Location (26-27)
            /// </summary>
            public Int16 prop06CALevel
            {
                get { return iCALevel; }
                set { iCALevel = value; }
            }

            public byte[] StructToByte()
            {
                byte[] contract = new byte[CConstants.Contract];
                try
                {
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cInstrumentName), 0, contract, 0, 6);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSymbol), 0, contract, 6, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iExpiryDate), 0, contract, 16, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iStrikePrice), 0, contract, 20, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOptionType), 0, contract, 24, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iCALevel), 0, contract, 26, 2);
                    return contract;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01InstrumentName = Encoding.ASCII.GetString(ByteStructure, 0, 6);
                    Prop02Symbol = Encoding.ASCII.GetString(ByteStructure, 6, 10);
                    Prop03ExpiryDate = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 16));
                    Prop04StrikePrice = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 20));
                    Prop05OptionType = Encoding.ASCII.GetString(ByteStructure, 24, 2);
                    prop06CALevel = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 26));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            public CONTRACTDESC(Int16 CALevel)
            {

                cInstrumentName = new char[6];
                cSymbol = new char[10];
                iExpiryDate = 0;
                iStrikePrice = 0;
                cOptionType = new char[2];
                iCALevel = 0;
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01InstrumentName +
                        Prop02Symbol +
                        Prop03ExpiryDate.ToString() +
                        Prop04StrikePrice.ToString() +
                        Prop05OptionType +
                        prop06CALevel.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

        }
        #endregion

        #region OrderFlag
        public struct OrderFlag
        {
            /*
             *  AON         : 1 bit
                IOC         : 1 bit
                GTC         : 1 bit
                Day         : 1 bit
                MIT         : 1 bit
                SL          : 1 bit
                Market      : 1 bit
                ATO         : 1 bit
                Reserved    : 3 bits
                Frozen      : 1 bit
                Modified    : 1 bit
                Traded      : 1 bit
                MatchedInd  : 1 bit
                MF          : 1 bit
             * ===============================
             * Total        : 16 bits(2 bytes)
             * ===============================
             * 
             */

            public bool bolAon, bolIoc, bolGtc, bolDay, bolMit, bolSl, bolMarket, bolAto, bolFrozen, bolModified, boltraded, bolMatchind, bolMf;
            public byte[] StructToByte()
            {
                byte[] Orderflag = new byte[2];
                try
                {
                    byte bfill = Convert.ToByte(0);
                    byte bfill2 = Convert.ToByte(0);
                    bfill = CUtility.ConvertByteStructureToExchangeBitFormat
                        (bolAon, true,
                         bolIoc, true,
                         bolGtc, true,
                         bolDay, true,
                         bolMit, true,
                         bolSl, true,
                         bolMarket, true,
                         bolAto, true,
                         bfill);
                    bfill2 = CUtility.ConvertByteStructureToExchangeBitFormat(false, false,
                        false, false,
                        false, false,
                        bolFrozen, true,
                        bolModified, true,
                        boltraded, true,
                        bolMatchind, true,
                        bolMf, true, bfill2);

                    Orderflag[0] = bfill;
                    Orderflag[1] = bfill2;
                    return Orderflag;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }


            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    byte bfill = Convert.ToByte(0);
                    byte bfill2 = Convert.ToByte(0);
                    bfill = ByteStructure[0];
                    bfill2 = ByteStructure[1];
                    bool[] temp1 = CUtility.ConvertExchangeBitFormatToStructure(bfill);
                    bool[] temp2 = CUtility.ConvertExchangeBitFormatToStructure(bfill2);
                    bolAon = temp1[7];
                    bolIoc = temp1[6];
                    bolGtc = temp1[5];
                    bolDay = temp1[4];
                    bolMit = temp1[3];
                    bolSl = temp1[2];
                    bolMarket = temp1[1];
                    bolAto = temp1[0];
                    bolFrozen = temp2[4];
                    bolModified = temp2[3];
                    boltraded = temp2[2];
                    bolMatchind = temp2[1];
                    bolMf = temp2[1];
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            public override string ToString()
            {
                try
                {
                    return (Convert.ToInt16(bolAon).ToString() +
                        Convert.ToInt16(bolIoc).ToString() +
                        Convert.ToInt16(bolGtc).ToString() +
                        Convert.ToInt16(bolDay).ToString() +
                           Convert.ToInt16(bolMit).ToString() +
                           Convert.ToInt16(bolSl).ToString() +
                           Convert.ToInt16(bolMarket).ToString() +
                           Convert.ToInt16(bolAto).ToString() +
                           Convert.ToInt16(bolFrozen).ToString() +
                           Convert.ToInt16(bolModified).ToString() +
                           Convert.ToInt16(boltraded).ToString() +
                           Convert.ToInt16(bolMatchind).ToString() +
                           Convert.ToInt16(bolMf).ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }

            public OrderFlag(int code)
            {
                bolAon = false;
                bolIoc = false;
                bolGtc = false;
                bolDay = false;
                bolMit = false;
                bolSl = false;
                bolMarket = false;
                bolAto = false;
                bolFrozen = false;
                bolModified = false;
                boltraded = false;
                bolMatchind = false;
                bolMf = false;
            }
        }
        #endregion

        #region Order Fillers
        /// <summary>
        /// Order filler : size(2) location(0-1)
        /// </summary>
        public struct OrderFiller
        {
            /*
             *  USHORT filler1  :1
                USHORT filler2  :1
                USHORT filler3  :1
                USHORT filler4  :1
                USHORT filler5  :1
                USHORT filler6  :1
                USHORT filler7  :1
                USHORT filler8  :1
                USHORT filler9  :1
                USHORT filler10 :1
                USHORT filler11 :1
                USHORT filler12 :1
                USHORT filler13 :1
                USHORT filler14 :1
                USHORT filler15 :1
                USHORT filler16 :1
             * ==============================
             * Total            16(bit) 2 byte
             * ==============================
             */
            bool bolfiller1;
            public bool Prop01Filler1
            {
                get { return bolfiller1; }
                set { bolfiller1 = value; }
            }
            bool bolfiller2;
            public bool Prop02Filler2
            {
                get { return bolfiller2; }
                set { bolfiller2 = value; }
            }
            bool bolfiller3;
            public bool Prop03Filler3
            {
                get { return bolfiller3; }
                set { bolfiller3 = value; }
            }
            bool bolfiller4;
            public bool Prop04Filler4
            {
                get { return bolfiller4; }
                set { bolfiller4 = value; }
            }
            bool bolfiller5;
            public bool Prop05Filler5
            {
                get { return bolfiller5; }
                set { bolfiller5 = value; }
            }
            bool bolfiller6;
            public bool Prop06Filler6
            {
                get { return bolfiller6; }
                set { bolfiller6 = value; }
            }
            bool bolfiller7;
            public bool Prop07Filler7
            {
                get { return bolfiller7; }
                set { bolfiller7 = value; }
            }
            bool bolfiller8;
            public bool Prop08Filler8
            {
                get { return bolfiller8; }
                set { bolfiller8 = value; }
            }
            bool bolfiller9;
            public bool Prop09Filler9
            {
                get { return bolfiller9; }
                set { bolfiller9 = value; }
            }
            bool bolfiller10;
            public bool Prop10Filler10
            {
                get { return bolfiller10; }
                set { bolfiller10 = value; }
            }
            bool bolfiller11;
            public bool Prop11Filler11
            {
                get { return bolfiller11; }
                set { bolfiller11 = value; }
            }
            bool bolfiller12;
            public bool Prop12Filler12
            {
                get { return bolfiller12; }
                set { bolfiller12 = value; }
            }
            bool bolfiller13;
            public bool Prop13Filler13
            {
                get { return bolfiller13; }
                set { bolfiller13 = value; }
            }
            bool bolfiller14;
            public bool Prop14Filler14
            {
                get { return bolfiller14; }
                set { bolfiller14 = value; }
            }
            bool bolfiller15;
            public bool Prop15Filler15
            {
                get { return bolfiller15; }
                set { bolfiller15 = value; }
            }
            bool bolfiller16;
            public bool Prop16Filler16
            {
                get { return bolfiller16; }
                set { bolfiller16 = value; }
            }

            public byte[] StructToByte()
            {
                byte[] Orderfiller = new byte[2];
                try
                {
                    byte bfill1 = Convert.ToByte(0);
                    byte bfill2 = Convert.ToByte(0);
                    bfill1 = CUtility.ConvertByteStructureToExchangeBitFormat
                        (bolfiller1, true,
                         bolfiller2, true,
                         bolfiller3, true,
                         bolfiller4, true,
                         bolfiller5, true,
                         bolfiller6, true,
                         bolfiller7, true,
                         bolfiller8, true,
                         bfill1);
                    bfill2 = CUtility.ConvertByteStructureToExchangeBitFormat(
                        bolfiller9, true,
                        bolfiller10, true,
                        bolfiller11, true,
                        bolfiller12, true,
                        bolfiller13, true,
                        bolfiller14, true,
                        bolfiller15, true,
                        bolfiller16, true, bfill2);

                    Orderfiller[0] = bfill1;
                    Orderfiller[1] = bfill2;
                    return Orderfiller;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    byte bfill1 = Convert.ToByte(0);
                    byte bfill2 = Convert.ToByte(0);
                    bfill1 = ByteStructure[0];
                    bfill2 = ByteStructure[1];
                    bool[] temp1 = CUtility.ConvertExchangeBitFormatToStructure(bfill1);
                    bool[] temp2 = CUtility.ConvertExchangeBitFormatToStructure(bfill2);
                    Prop01Filler1 = temp1[7];
                    Prop02Filler2 = temp1[6];
                    Prop03Filler3 = temp1[5];
                    Prop04Filler4 = temp1[4];
                    Prop05Filler5 = temp1[3];
                    Prop06Filler6 = temp1[2];
                    Prop07Filler7 = temp1[1];
                    Prop08Filler8 = temp1[0];
                    Prop09Filler9 = temp2[7];
                    Prop10Filler10 = temp2[6];
                    Prop11Filler11 = temp2[5];
                    Prop12Filler12 = temp2[4];
                    Prop13Filler13 = temp1[3];
                    Prop14Filler14 = temp1[2];
                    Prop15Filler15 = temp1[1];
                    Prop16Filler16 = temp1[0];
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }

            public override string ToString()
            {
                try
                {
                    return (Convert.ToInt16(Prop01Filler1).ToString() + Convert.ToInt16(Prop02Filler2).ToString() + Convert.ToInt16(Prop03Filler3).ToString() + Convert.ToInt16(Prop04Filler4).ToString() + Convert.ToInt16(Prop05Filler5).ToString() +
               Convert.ToInt16(Prop06Filler6).ToString() + Convert.ToInt16(Prop07Filler7).ToString() + Convert.ToInt16(Prop08Filler8).ToString() + Convert.ToInt16(Prop09Filler9).ToString() + Convert.ToInt16(Prop10Filler10).ToString() +
               Convert.ToInt16(Prop11Filler11).ToString() + Convert.ToInt16(Prop12Filler12).ToString() + Convert.ToInt16(Prop13Filler13).ToString() + Convert.ToInt16(Prop14Filler14).ToString() + Convert.ToInt16(Prop15Filler15).ToString() + Convert.ToInt16(Prop16Filler16).ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }

        }
        #endregion

        #region Order Entry Request
        /// <summary>
        /// Order Entry Request : Size(236) Location (0-235)
        /// </summary>
        public struct OrderEntryRequest : IStruct
        {
            /*
             *  MESSAGEHEADER                      38      38
                CHAR ParticipantType                1       39
                CHAR Reserved                       1       40  
                SHORT CompetitorPeriod              2       42
                SHORT SolicitorPeriod               2       44
                CHAR Modified / CancelledBy         1       45
                CHAR Reserved                       1       46
                SHORT ReasonCode                    2       48
                CHAR Reserved [4]                   4       52
                LONG TokenNo                        4       56
                STRUCT CONTRACTDESC                28      84
                CHAR CounterPartyBrokerId [5]       5       89
                CHAR Reserved                       1       90
                CHAR Reserved [2]                   2       92  
                CHAR CloseoutFlag                   1       93
                CHAR Reserved                       1       94
                SHORT OrderType                     2       96
                DOUBLE OrderNumber                  8       104
                CHAR AccountNumber [10]             10      114
                SHORT BookType                      2       116
                SHORT Buy / SellIndicator           2       118
                LONG DisclosedVolume                4       122
                LONG DisclosedVolumeRemaining       4       126
                LONG TotalVolumeRemaining           4       130
                LONG Volume                         4       134
                LONG VolumeFilledToday              4       138
                LONG Price                          4       142
                LONG TriggerPrice                   4       146
                LONG GoodTillDate                   4       150 
                LONG EntryDateTime                  4       154 
                LONG MinimumFill / AONVolume        4       158
                LONG LastModified                   4       162
                STORDERFLAGS                        2       164
                SHORT BranchId                      2       166
                SHORT TraderId                      2       168
                CHAR BrokerId [5]                   5       173
                CHAR Remarks [24]                   24      197
                CHAR Open/Close                     1       198
                CHAR Settlor [12]                   12      210
                SHORT Pro / ClientIndicator         2       212
                SHORT SettlementPeriod              2       214
                CHAR Cover/Uncover                  1       215
                CHAR GiveupFlag                     1       216
                USHORT filler1 :1
                USHORT filler2 :1
                USHORT filler3 :1
                USHORT filler4 :1
                USHORT filler5 :1
                USHORT filler6 :1
                USHORT filler7 :1
                USHORT filler8 :1
                USHORT filler9 :1
                USHORT filler10 :1
                USHORT filler11 :1
                USHORT filler12 :1
                USHORT filler13 :1
                USHORT filler14 :1
                USHORT filler15 :1
                USHORT filler16 :1                  2       218
                CHAR filler17                       1       219
                CHAR filler18                       1       220
                DOUBLE NnfField                     8       228
                DOUBLE MktReplay                    8       236
             * =====================================================
             * Total                                236     236
             * =====================================================
            */
            /// <summary>
            /// MESSAGEHEADER : Size(38) Location(0-37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            ///  Property 01 Message Header : Size(38) Loaction(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }

            /// <summary>
            /// CHAR ParticipantType : size(1) Location(38)
            /// </summary>
            public char[] cParticipantType;
            /// <summary>
            ///  CHAR ParticipantType : size(1) Location(38)    
            /// </summary>
            public string Prop02ParticipantType
            {
                get { return new string(cParticipantType); }
                set { cParticipantType = CUtility.GetPreciseArrayForString(value.ToString(), cParticipantType.Length); }
            }

            /// <summary>
            /// CHAR Reserved : size(1) Location(39)
            /// </summary>
            public char[] cReserve;
            /// <summary>
            /// CHAR Reserved : size(1) Location(39)
            /// </summary>
            public string Prop03Reserve
            {
                get { return new string(cReserve); }
                set { cReserve = CUtility.GetPreciseArrayForString(value.ToString(), cReserve.Length); }
            }

            /// <summary>
            /// SHORT CompetitorPeriod : Size(2) Loaction(40-41)
            /// </summary>
            Int16 iCompetitorPeriod;
            /// <summary>
            /// SHORT CompetitorPeriod : Size(2) Loaction(40-41)
            /// </summary>
            public Int16 Prop04CompetitionPeriod
            {
                get { return iCompetitorPeriod; }
                set { iCompetitorPeriod = value; }
            }

            /// <summary>
            /// SHORT SolicitorPeriod :size(2) Loaction(42-43)
            /// </summary>
            public Int16 iSolicitorPeriod;
            /// <summary>
            /// SHORT SolicitorPeriod: size(2) Loaction(42-43)
            /// </summary>
            public Int16 Prop05SolicitorPeriod
            {
                get { return iSolicitorPeriod; }
                set { iSolicitorPeriod = value; }
            }

            /// <summary>
            /// CHAR Modified / CancelledBy : size(1) Location(44)
            /// </summary>
            public char[] cModified;
            /// <summary>
            /// CHAR Modified / CancelledBy : size(1) Location(44)
            /// </summary>
            public string Prop06Modified
            {
                get { return new string(cModified); }
                set { cModified = CUtility.GetPreciseArrayForString(value.ToString(), cModified.Length); }
            }

            /// <summary>
            /// CHAR Reserved :size(1) Location(45)
            /// </summary>
            public char[] cReserve1;
            /// <summary>
            /// CHAR Reserved :size(1) Location(45)
            /// </summary>
            public string Prop07Reserve1
            {
                get { return new string(cReserve1); }
                set { cReserve1 = CUtility.GetPreciseArrayForString(value.ToString(), cReserve1.Length); }
            }

            /// <summary>
            /// SHORT ReasonCode :Size(2) Loaction(46-47)
            /// </summary>
            public Int16 iReasonCode;
            /// <summary>
            /// SHORT ReasonCode :Size(2) Loaction(46-47)
            /// </summary>
            public Int16 Prop08ReasonCode
            {
                get { return iReasonCode; }
                set { iReasonCode = value; }
            }

            /// <summary>
            /// CHAR Reserved [4] :Size(4) Location(48-51)
            /// </summary>
            public char[] cReserve2;
            /// <summary>
            /// CHAR Reserved [4] :Size(4) Location(48-51)
            /// </summary>
            public string Prop09Reserve2
            {
                get { return new string(cReserve2); }
                set { cReserve2 = CUtility.GetPreciseArrayForString(value.ToString(), cReserve2.Length); }
            }

            /// <summary>
            /// LONG TokenNo : Size(4) Loaction(52-55)
            /// </summary>
            public Int32 iTokenNo;
            /// <summary>
            /// LONG TokenNo : Size(4) Loaction(52-55)
            /// </summary>
            public Int32 Prop10TokenNo
            {
                get { return iTokenNo; }
                set { iTokenNo = value; }
            }

            /// <summary>
            /// CONTRACTDESC :Size(28) Location(83)
            /// </summary>
            public CONTRACTDESC strContractDesc;
            /// <summary>
            /// CONTRACTDESC :Size(28) Location(83)
            /// </summary>
            public CONTRACTDESC Prop11ContractDesc
            {
                get { return strContractDesc; }
                set { strContractDesc = value; }
            }

            /// <summary>
            /// CHAR CounterPartyBrokerId [5] : Size(5) Location(84-88)
            /// </summary>
            public char[] cCounterBrokerid;
            /// <summary>
            /// CHAR CounterPartyBrokerId [5] : Size(5) Location(84-88)
            /// </summary>
            public string Prop12CounterBrokerid
            {
                get { return new string(cCounterBrokerid); }
                set { cCounterBrokerid = CUtility.GetPreciseArrayForString(value.ToString(), cCounterBrokerid.Length); }
            }

            /// <summary>
            /// CHAR Reserved : Size(1) Location(89)
            /// </summary>
            public char[] cReserve3;
            /// <summary>
            /// CHAR Reserved : Size(1) Location(89)
            /// </summary>
            public string Prop13Reserve3
            {
                get { return new string(cReserve3); }
                set { cReserve3 = CUtility.GetPreciseArrayForString(value.ToString(), cReserve3.Length); }
            }

            /// <summary>
            /// CHAR Reserved [2] : size(2) Location(90-91)
            /// </summary>
            public char[] cReserve4;
            /// <summary>
            /// CHAR Reserved [2] : size(2) Location(90-91)
            /// </summary>
            public string Prop14Reserve4
            {
                get { return new string(cReserve4); }
                set { cReserve4 = CUtility.GetPreciseArrayForString(value.ToString(), cReserve4.Length); }
            }

            /// <summary>
            /// CHAR CloseoutFlag :Size(1) Location(92)
            /// </summary>
            public char[] cClosedOutFlag;
            /// <summary>
            /// CHAR CloseoutFlag :Size(1) Location(92)
            /// </summary>
            public string Prop15ClosedoutFlag
            {
                get { return new string(cClosedOutFlag); }
                set { cClosedOutFlag = CUtility.GetPreciseArrayForString(value.ToString(), cClosedOutFlag.Length); }
            }

            /// <summary>
            /// CHAR Reserved : size(1) Location(93)
            /// </summary>
            public char[] cReserve5;
            /// <summary>
            /// CHAR Reserved : size(1) Location(93)
            /// </summary>
            public string Prop16Reserve5
            {
                get { return new string(cReserve5); }
                set { cReserve5 = CUtility.GetPreciseArrayForString(value.ToString(), cReserve5.Length); }
            }

            /// <summary>
            ///  SHORT OrderType : size(2) Loaction(94-95)
            /// </summary>
            public Int16 iOrderType;
            /// <summary>
            /// SHORT OrderType : size(2) Loaction(94-95)
            /// </summary>
            public Int16 Prop17OrderType
            {
                get { return iOrderType; }
                set { iOrderType = value; }
            }

            /// <summary>
            /// DOUBLE OrderNumber :size(8) Location(96-103)
            /// </summary>
            public Double iOrderNumber;
            /// <summary>
            /// DOUBLE OrderNumber :size(8) Location(96-103)
            /// </summary>
            public Double Prop18OrderNumber
            {
                get { return iOrderNumber; }
                set { iOrderNumber = value; }
            }

            /// <summary>
            /// CHAR AccountNumber [10]: size(10) Location(104-113)
            /// </summary>
            public char[] cAccountNo;
            /// <summary>
            /// CHAR AccountNumber [10]: size(10) Location(104-113)
            /// </summary>
            public string Prop19AccountNo
            {
                get { return new string(cAccountNo); }
                set { cAccountNo = CUtility.GetPreciseArrayForString(value.ToString(), cAccountNo.Length); }
            }
            /// <summary>
            /// SHORT BookType: size(2) Location(114-115)
            /// </summary>
            public Int16 ibookType;
            /// <summary>
            /// SHORT BookType: size(2) Location(114-115)
            /// </summary>
            public Int16 Prop20BookType
            {
                get { return ibookType; }
                set { ibookType = value; }
            }

            /// <summary>
            /// SHORT Buy / SellIndicator: size(2) Location(116-117)
            /// </summary>
            public Int16 iIndicator;
            /// <summary>
            /// SHORT Buy / SellIndicator: size(2) Location(116-117)
            /// </summary>
            public Int16 Prop21Indicator
            {
                get { return iIndicator; }
                set { iIndicator = value; }
            }

            /// <summary>
            /// LONG DisclosedVolume : size(4) Location(118-121)
            /// </summary>
            public Int32 iDisclosedVolume;
            /// <summary>
            /// LONG DisclosedVolume : size(4) Location(118-121)
            /// </summary>
            public Int32 Prop22DisclosedVolume
            {
                get { return iDisclosedVolume; }
                set { iDisclosedVolume = value; }
            }

            /// <summary>
            /// LONG DisclosedVolumeRemaining : size(4) location(122-125)
            /// </summary>
            public Int32 iDisclosedVolRemain;
            /// <summary>
            /// LONG DisclosedVolumeRemaining : size(4) location(122-125)
            /// </summary>
            public Int32 Prop23DisclosedVolRemain
            {
                get { return iDisclosedVolRemain; }
                set { iDisclosedVolRemain = value; }
            }
            /// <summary>
            /// LONG TotalVolumeRemaining : size(4) location(126-129)
            /// </summary>
            public Int32 iTotalVolumeRem;
            /// <summary>
            /// LONG TotalVolumeRemaining : size(4) location(126-129)
            /// </summary>
            public Int32 Prop24TotalVolRemaining
            {
                get { return iTotalVolumeRem; }
                set { iTotalVolumeRem = value; }
            }

            /// <summary>
            /// LONG Volume : size(4) location(130-133)
            /// </summary>
            public Int32 iVolume;
            /// <summary>
            /// LONG Volume : size(4) location(130-133)
            /// </summary>
            public Int32 Prop25Volume
            {
                get { return iVolume; }
                set { iVolume = value; }
            }

            /// <summary>
            /// LONG VolumeFilledToday : size(4) Location(134-137)
            /// </summary>
            public Int32 iVolumeFillToday;
            /// <summary>
            /// LONG VolumeFilledToday : size(4) Location(134-137)
            /// </summary>
            public Int32 Prop26VolumeFillToday
            {
                get { return iVolumeFillToday; }
                set { iVolumeFillToday = value; }
            }
            /// <summary>
            /// LONG Price :size(4) Location(138-141)
            /// </summary>
            public Int32 iPrice;
            /// <summary>
            /// LONG Price :size(4) Location(138-141)
            /// </summary>
            public Int32 Prop27Price
            {
                get { return iPrice; }
                set { iPrice = value; }
            }

            /// <summary>
            /// LONG TriggerPrice: Size(4) Location(142-145)
            /// </summary>
            public Int32 iTriggerPrice;
            /// <summary>
            /// LONG TriggerPrice: Size(4) Location(142-145)
            /// </summary>
            public Int32 Prop28TriggerPrice
            {
                get { return iTriggerPrice; }
                set { iTriggerPrice = value; }
            }

            /// <summary>
            /// LONG GoodTillDate : Size(4) Location(146-149)
            /// </summary>
            public Int32 iGoodTillDate;
            /// <summary>
            /// LONG GoodTillDate : Size(4) Location(146-149)
            /// </summary>
            public Int32 Prop29Goodtilldate
            {
                get { return iGoodTillDate; }
                set { iGoodTillDate = value; }
            }
            /// <summary>
            /// LONG EntryDateTime: :Size(4) Location(150-153)
            /// </summary>
            public Int32 iEntryDateTime;
            /// <summary>
            /// LONG EntryDateTime: :Size(4) Location(150-153)
            /// </summary>
            public Int32 Prop30EntryDatetime
            {
                get { return iEntryDateTime; }
                set { iEntryDateTime = value; }
            }
            /// <summary>
            /// LONG MinimumFill / AONVolume: size(4) Loaction(154-157)
            /// </summary>
            public Int32 iMinFillVol;
            /// <summary>
            /// LONG MinimumFill / AONVolume: size(4) Loaction(154-157)
            /// </summary>
            public Int32 Prop31MinimumfillVol
            {
                get { return iMinFillVol; }
                set { iMinFillVol = value; }
            }
            /// <summary>
            /// LONG LastModified : size(4) location(158-161)
            /// </summary>
            public Int32 iLastModified;
            /// <summary>
            /// LONG LastModified : size(4) location(158-161)
            /// </summary>
            public Int32 Prop32LastModify
            {
                get { return iLastModified; }
                set { iLastModified = value; }
            }

            public Int16 iflag;
            public Int16 Prop33OrderFlag
            {
                get { return iflag; }
                set { iflag = value; }
            }

            /// <summary>
            /// SHORT BranchId : size(2) Location(164-165)
            /// </summary>
            public Int16 iBranchId;
            /// <summary>
            /// SHORT BranchId : size(2) Location(164-165)
            /// </summary>
            public Int16 Prop34BranchId
            {
                get { return iBranchId; }
                set { iBranchId = value; }
            }

            /// <summary>
            /// SHORT TraderId: Size(2) Location(166-167)
            /// </summary>
            public Int16 iTraderId;
            /// <summary>
            /// SHORT TraderId: Size(2) Location(166-167)
            /// </summary>
            public Int16 Prop35TraderId
            {
                get { return iTraderId; }
                set { iTraderId = value; }
            }

            /// <summary>
            /// CHAR BrokerId [5] : size(5) Location(168-172)
            /// </summary>
            public char[] cBrokerId;
            /// <summary>
            /// CHAR BrokerId [5] : size(5) Location(168-172)
            /// </summary>
            public string Prop36BrokerId
            {
                get { return new string(cBrokerId); }
                set { cBrokerId = CUtility.GetPreciseArrayForString(value.ToString(), cBrokerId.Length); }
            }

            /// <summary>
            /// CHAR Remarks [24] : Size(24) Location (173-196)
            /// </summary>
            public char[] cRemark;
            /// <summary>
            /// CHAR Remarks [24] : Size(24) Location (173-196) 
            /// </summary>
            public string Prop37Remark
            {
                get { return new string(cRemark); }
                set { cRemark = CUtility.GetPreciseArrayForString(value.ToString(), cRemark.Length); }
            }
            /// <summary>
            /// CHAR Open/Close : size(1) Location(197)
            /// </summary>
            public char[] cOpenClose;
            /// <summary>
            /// CHAR Open/Close : size(1) Location(197)
            /// </summary>
            public string Prop38OpenClose
            {
                get { return new string(cOpenClose); }
                set { cOpenClose = CUtility.GetPreciseArrayForString(value.ToString(), cOpenClose.Length); }
            }

            /// <summary>
            /// CHAR Settlor [12] : size(12) location(198-209)
            /// </summary>
            public char[] cSettlor;
            /// <summary>
            /// CHAR Settlor [12] : size(12) location(197-209)
            /// </summary>
            public string Prop39Settlor
            {
                get { return new string(cSettlor); }
                set { cSettlor = CUtility.GetPreciseArrayForString(value.ToString(), cSettlor.Length); }
            }

            /// <summary>
            /// SHORT Pro / ClientIndicator : size(2) Location(210-211)
            /// </summary>
            public Int16 iProIndicator;
            /// <summary>
            /// SHORT Pro / ClientIndicator : size(2) Location(210-211)
            /// </summary>
            public Int16 Prop40ProIndicator
            {
                get { return iProIndicator; }
                set { iProIndicator = value; }
            }

            /// <summary>
            /// SHORT SettlementPeriod : size(2) Location(212-213)
            /// </summary>
            public Int16 iSettlePeriod;
            /// <summary>
            /// SHORT SettlementPeriod : size(2) Location(212-213)
            /// </summary>
            public Int16 Prop41SettlementPeriod
            {
                get { return iSettlePeriod; }
                set { iSettlePeriod = value; }
            }

            /// <summary>
            /// CHAR Cover/Uncover : size(1) Location(214)
            /// </summary>
            public char[] cCoverUncover;
            /// <summary>
            /// CHAR Cover/Uncover : size(1) Location(214)
            /// </summary>
            public string Prop42CoverUncover
            {
                get { return new string(cCoverUncover); }
                set { cCoverUncover = CUtility.GetPreciseArrayForString(value.ToString(), cCoverUncover.Length); }
            }

            /// <summary>
            /// CHAR GiveupFlag : size(1) Location(215)
            /// </summary>
            public char[] cGiveUpFlag;
            /// <summary>
            /// CHAR GiveupFlag : size(1) Location(215)
            /// </summary>
            public string Prop43GiveupFlag
            {
                get { return new string(cGiveUpFlag); }
                set { cGiveUpFlag = CUtility.GetPreciseArrayForString(value.ToString(), cGiveUpFlag.Length); }
            }

            public Int32 iOrderId;
            public Int32 PropOrderId
            {
                get { return iOrderId; }
                set { iOrderId = value; }
            }

            /// <summary>
            /// DOUBLE NnfField : size(8) Location(220-227)
            /// </summary>
            public Double iNNFField;
            /// <summary>
            /// DOUBLE NnfField : size(8) Location(220-227)
            /// </summary>
            public Double Prop47NNFField
            {
                get { return iNNFField; }
                set { iNNFField = value; }
            }
            /// <summary>
            /// DOUBLE MktReplay : size(8) Location(228-235)
            /// </summary>
            public Double iMarketReplay;
            /// <summary>
            /// DOUBLE MktReplay : size(8) Location(228-235)
            /// </summary>
            public Double Prop48MktReplay
            {
                get { return iMarketReplay; }
                set { iMarketReplay = value; }
            }




            ///// <summary>
            ///// STRUCT STORDERFLAGS : size(2) Location (162-163)
            ///// </summary>
            //public OrderFlag strOrderflag;
            ///// <summary>
            ///// STRUCT STORDERFLAGS : size(2) Location (162-163)
            ///// </summary>
            //public OrderFlag Prop33OrderFlag
            //{
            //    get { return strOrderflag; }
            //    set { strOrderflag = value; }

            //}
            ///// <summary>
            ///// struct OrderFiller: size(2) Location(216-217)
            ///// </summary>
            //public OrderFiller strOrderFiller;
            ///// <summary>
            ///// struct OrderFiller: size(2) Location(216-217)
            ///// </summary>
            //public OrderFiller Prop44OrderFiller
            //{
            //    get { return strOrderFiller; }
            //    set { strOrderFiller = value; }

            //}

            ///// <summary>
            ///// CHAR filler17 : Size(1) Location(218)
            ///// </summary>
            //public char[] cFiller1;
            ///// <summary>
            ///// CHAR filler17 : Size(1) Location(218)
            ///// </summary>
            //public string Prop45Filler1
            //{
            //    get { return new string(cFiller1); }
            //    set { cFiller1 = CUtility.GetPreciseArrayForString(value.ToString(), cFiller1.Length); }
            //}
            ///// <summary>
            ///// CHAR filler18 : Size(1) Location(219)
            ///// </summary>
            //public char[] cFiller2;
            ///// <summary>
            ///// CHAR filler18 : Size(1) Location(219)
            ///// </summary>
            //public string Prop46Filler2
            //{
            //    get { return new string(cFiller2); }
            //    set { cFiller2 = CUtility.GetPreciseArrayForString(value.ToString(), cFiller2.Length); }
            //}




            public byte[] StructToByte()
            {
                byte[] OrderRequest = new byte[CConstants.OrderRequestSize];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, OrderRequest, 0, 38);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cParticipantType), 0, OrderRequest, 38, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve), 0, OrderRequest, 39, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iCompetitorPeriod), 0, OrderRequest, 40, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iSolicitorPeriod), 0, OrderRequest, 42, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cModified), 0, OrderRequest, 44, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve1), 0, OrderRequest, 45, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iReasonCode), 0, OrderRequest, 46, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve2), 0, OrderRequest, 48, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTokenNo), 0, OrderRequest, 52, 4);
                    Buffer.BlockCopy(strContractDesc.StructToByte(), 0, OrderRequest, 56, 28);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cCounterBrokerid), 0, OrderRequest, 84, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve3), 0, OrderRequest, 89, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve4), 0, OrderRequest, 90, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cClosedOutFlag), 0, OrderRequest, 92, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserve5), 0, OrderRequest, 93, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iOrderType), 0, OrderRequest, 94, 2);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iOrderNumber), 0, OrderRequest, 96, 8);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cAccountNo), 0, OrderRequest, 104, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(ibookType), 0, OrderRequest, 114, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iIndicator), 0, OrderRequest, 116, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVolume), 0, OrderRequest, 118, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVolRemain), 0, OrderRequest, 122, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTotalVolumeRem), 0, OrderRequest, 126, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVolume), 0, OrderRequest, 130, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVolumeFillToday), 0, OrderRequest, 134, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iPrice), 0, OrderRequest, 138, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTriggerPrice), 0, OrderRequest, 142, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iGoodTillDate), 0, OrderRequest, 146, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iEntryDateTime), 0, OrderRequest, 150, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iMinFillVol), 0, OrderRequest, 154, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastModified), 0, OrderRequest, 158, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iflag), 0, OrderRequest, 162, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBranchId), 0, OrderRequest, 164, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTraderId), 0, OrderRequest, 166, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBrokerId), 0, OrderRequest, 168, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cRemark), 0, OrderRequest, 173, 24);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOpenClose), 0, OrderRequest, 197, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSettlor), 0, OrderRequest, 198, 12);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iProIndicator), 0, OrderRequest, 210, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iSettlePeriod), 0, OrderRequest, 212, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cCoverUncover), 0, OrderRequest, 214, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cGiveUpFlag), 0, OrderRequest, 215, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iOrderId), 0, OrderRequest, 216, 4);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iNNFField), 0, OrderRequest, 220, 8);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iMarketReplay), 0, OrderRequest, 228, 8);
                    return OrderRequest;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
                //Buffer.BlockCopy(strOrderFiller.StructToByte(), 0, OrderRequest, 216, 2);
                //Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFiller1), 0, OrderRequest, 218, 1);
                //Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFiller2), 0, OrderRequest, 219, 1);

            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                strContractDesc = new CONTRACTDESC(11);

                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02ParticipantType = Encoding.ASCII.GetString(ByteStructure, 38, 1);
                    Prop03Reserve = Encoding.ASCII.GetString(ByteStructure, 39, 1);
                    Prop04CompetitionPeriod = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 40));
                    Prop05SolicitorPeriod = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 42));
                    Prop06Modified = Encoding.ASCII.GetString(ByteStructure, 44, 1);
                    Prop07Reserve1 = Encoding.ASCII.GetString(ByteStructure, 45, 1);
                    Prop08ReasonCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 46));
                    Prop09Reserve2 = Encoding.ASCII.GetString(ByteStructure, 48, 4);
                    Prop10TokenNo = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 52));
                    strContractDesc.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 56, 28));
                    Prop11ContractDesc = strContractDesc;//.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 56, 28));
                    Prop12CounterBrokerid = Encoding.ASCII.GetString(ByteStructure, 84, 5);
                    Prop13Reserve3 = Encoding.ASCII.GetString(ByteStructure, 89, 1);
                    Prop14Reserve4 = Encoding.ASCII.GetString(ByteStructure, 90, 2);
                    Prop15ClosedoutFlag = Encoding.ASCII.GetString(ByteStructure, 92, 1);
                    Prop16Reserve5 = Encoding.ASCII.GetString(ByteStructure, 93, 1);
                    Prop17OrderType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 94));
                    Prop18OrderNumber = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 96, 8));//IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 96));
                    Prop19AccountNo = Encoding.ASCII.GetString(ByteStructure, 104, 10);
                    Prop20BookType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 114));
                    Prop21Indicator = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 116));
                    Prop22DisclosedVolume = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 118));
                    Prop23DisclosedVolRemain = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 122));
                    Prop24TotalVolRemaining = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 126));
                    Prop25Volume = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 130));
                    Prop26VolumeFillToday = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 134));
                    Prop27Price = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 138));
                    Prop28TriggerPrice = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 142));
                    Prop29Goodtilldate = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 146));
                    Prop30EntryDatetime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 150));
                    Prop31MinimumfillVol = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 154));
                    Prop32LastModify = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 158));
                    Prop33OrderFlag = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 162));
                    Prop34BranchId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 164));
                    Prop35TraderId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 166));
                    Prop36BrokerId = Encoding.ASCII.GetString(ByteStructure, 168, 5);
                    Prop37Remark = Encoding.ASCII.GetString(ByteStructure, 173, 24);
                    Prop38OpenClose = Encoding.ASCII.GetString(ByteStructure, 197, 1);
                    Prop39Settlor = Encoding.ASCII.GetString(ByteStructure, 198, 12);
                    Prop40ProIndicator = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 210));
                    Prop41SettlementPeriod = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 212));
                    Prop42CoverUncover = Encoding.ASCII.GetString(ByteStructure, 214, 1);
                    Prop43GiveupFlag = Encoding.ASCII.GetString(ByteStructure, 215, 1);
                    PropOrderId = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 216));
                    Prop47NNFField = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 220, 8));//IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 220));
                    Prop48MktReplay = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 228, 8));//IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 228));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }


            /// <summary>
            /// New Order Initial parameters 
            /// </summary>
            /// <param name="Transcode">2000</param>
            /// <param name="token">0</param>
            /// <param name="indicator">0</param>
            /// <param name="price">0</param>
            /// <param name="Brchid">Branchid else 0</param>
            /// <param name="Traderid">Trader id Else 0</param>
            /// <param name="ProIndicator">0</param>
            public OrderEntryRequest(Int16 Transcode)
            {

                strHeader = new MessageHeader(Transcode);
                cParticipantType = new char[1];
                cReserve = new char[1];
                iCompetitorPeriod = 0;
                iSolicitorPeriod = 0;
                cModified = new char[1];
                cReserve1 = new char[1];
                iReasonCode = 0;
                cReserve2 = new char[4];
                iTokenNo = 0;
                strContractDesc = new CONTRACTDESC(11);
                cCounterBrokerid = new char[5];
                cReserve3 = new char[1];
                cReserve4 = new char[2];
                cClosedOutFlag = new char[1];
                cReserve5 = new char[1];
                iOrderType = 0;
                iOrderNumber = 0;
                cAccountNo = new char[10];
                ibookType = 0;
                iIndicator = 0;
                iDisclosedVolume = 0;
                iDisclosedVolRemain = 0;
                iTotalVolumeRem = 0;
                iVolume = 0;
                iVolumeFillToday = 0;
                iPrice = 0;
                iTriggerPrice = 0;
                iGoodTillDate = 0;
                iEntryDateTime = 0;
                iMinFillVol = 0;
                iLastModified = 0;
                // strOrderflag = new OrderFlag();
                iBranchId = 0;
                iTraderId = 0;
                cBrokerId = new char[5];
                cRemark = new char[24];
                cOpenClose = new char[1];
                cSettlor = new char[12];
                iProIndicator = 0;
                iSettlePeriod = 0;
                cCoverUncover = new char[1];
                cGiveUpFlag = new char[1];
                //strOrderFiller = new OrderFiller();
                //cFiller1 = new char[1];
                //cFiller2 = new char[1];
                iNNFField = 0;
                iMarketReplay = 0;
                iflag = 0;
                iOrderId = 0;
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + CConstants.Seperator +
                        Prop02ParticipantType + CConstants.Seperator +
                        Prop03Reserve + CConstants.Seperator +
                        Prop04CompetitionPeriod.ToString() + CConstants.Seperator +
                        Prop05SolicitorPeriod.ToString() + CConstants.Seperator +
                            Prop06Modified + CConstants.Seperator +
                            Prop07Reserve1 + CConstants.Seperator +
                            Prop08ReasonCode.ToString() + CConstants.Seperator +
                            Prop09Reserve2 + CConstants.Seperator +
                            Prop10TokenNo.ToString() + CConstants.Seperator +
                            Prop11ContractDesc.ToString() + CConstants.Seperator +
                            Prop12CounterBrokerid + CConstants.Seperator +
                            Prop13Reserve3 + CConstants.Seperator +
                            Prop14Reserve4 + CConstants.Seperator +
                            Prop15ClosedoutFlag + CConstants.Seperator +
                            Prop16Reserve5 + CConstants.Seperator +
                            Prop17OrderType.ToString() + CConstants.Seperator +
                            Prop18OrderNumber.ToString() + CConstants.Seperator +
                            Prop19AccountNo + CConstants.Seperator +
                            Prop20BookType.ToString() + CConstants.Seperator +
                            Prop21Indicator.ToString() + CConstants.Seperator +
                            Prop22DisclosedVolume.ToString() + CConstants.Seperator +
                            Prop23DisclosedVolRemain.ToString() + CConstants.Seperator +
                            Prop24TotalVolRemaining.ToString() + CConstants.Seperator +
                            Prop25Volume.ToString() + CConstants.Seperator +
                            Prop26VolumeFillToday.ToString() + CConstants.Seperator +
                            Prop27Price.ToString() + CConstants.Seperator +
                            Prop28TriggerPrice.ToString() + CConstants.Seperator +
                            Prop29Goodtilldate.ToString() + CConstants.Seperator +
                            Prop30EntryDatetime.ToString() + CConstants.Seperator +
                            Prop31MinimumfillVol.ToString() + CConstants.Seperator +
                            Prop32LastModify.ToString() + CConstants.Seperator +
                            Prop33OrderFlag.ToString() + CConstants.Seperator +
                            Prop34BranchId.ToString() + CConstants.Seperator +
                            Prop35TraderId.ToString() + CConstants.Seperator +
                            Prop36BrokerId.ToString() + CConstants.Seperator +
                            Prop37Remark + CConstants.Seperator +
                            Prop38OpenClose + CConstants.Seperator +
                            Prop39Settlor + CConstants.Seperator +
                            Prop40ProIndicator.ToString() + CConstants.Seperator +
                            Prop41SettlementPeriod.ToString() + CConstants.Seperator +
                            Prop42CoverUncover + Prop43GiveupFlag + CConstants.Seperator +
                            PropOrderId.ToString() + CConstants.Seperator +
                            Prop47NNFField.ToString() + CConstants.Seperator +
                            Prop48MktReplay.ToString() + CConstants.Seperator);

                    //Prop44OrderFiller.ToString() + CConstants.Seperator +
                    // Prop45Filler1.ToString() + CConstants.Seperator +
                    //Prop46Filler2.ToString() + CConstants.Seperator +
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }

        #endregion

        #endregion

        #region Enumeration
        public enum EBookID
        {
            RegularLotOrderNormalMarket = 1,
            SpecialTermOrderNormalMarket = 2,
            StopLossKITOrderNormalMarket = 3,
            NegotiatedOrderNormalMarket = 4,
            OddLotOrderOddMarket = 5,
            SpotOrderSpotMarket = 6,
            AuctionOrderauctionMarket = 7


        }

        public enum EMarketStatus
        {
            PreOpen = 0,
            Open = 1,
            Closed = 2,
            PreOpenEnded = 3,
            PostClose = 4

        }

        public enum ESecurityStatus
        {
            PreOpen = 1,
            Open = 2,
            Suspended = 3,
            PreOpenExtended = 4,
            OpenWithMarket = 5
        }

        public enum EUsertype : uint
        {
            Dealer = 0,
            CorporateManager = 4,
            BranchManager = 5
        }

        #endregion

        #region Trade Modification Request
        /// <summary>
        /// Trade Modification Request : size(150) Location (0-149) 
        /// </summary>
        public struct TradeModificationRequest : IStruct
        {
            /*
                         *  MESSAGEHEADER                      38          38  
                            LONG TokenNo                        4           42
                            STRUCT CONTRACTDESC                28          70
                            LONG FillNumber                     4           74
                            LONG FillQuantity                   4           78
                            LONG FillPrice                      4           82
                            CHAR MktType                        1           83
                            CHAR BuyOpenClose                   1           84
                            LONG NewVolume                      4           88
                            CHAR BuyBrokerId [5]                5           93
                            CHAR SellBrokerId [5]               5           98
                            SHORT TraderId                      2           100
                            CHAR RequestedBy                    1           101                
                            CHAR SellOpenClose                  1           102
                            CHAR BuyAccountNumber [10]          10          112
                            CHAR SellAccountNumber [10]         10          122
                            CHAR BuyParticipant [12]            12          134
                            CHAR SellParticipant [12]           12          146
                            CHAR BuyCoverUncover                1           147
                            CHAR SellCoverUncover               1           148
                            CHAR BuyGiveupFlag                  1           149
                            CHAR SellGiveupFlag                 1           150
             *              =====================================================
             *              Total                                           150    
             *              =====================================================
             */
            /// <summary>
            /// MESSAGEHEADER          Size(38)    Location(0-37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// MESSAGEHEADER          Size(38)    Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }

            /// <summary>
            /// LONG TokenNo            size(4)     Location(38-41) 
            /// </summary>
            Int32 iTokenNo;
            /// <summary>
            /// LONG TokenNo            size(4)     Location(38-41) 
            /// </summary>
            public Int32 Prop02TokenNumber
            {
                get { return iTokenNo; }
                set { iTokenNo = value; }

            }

            /// <summary>
            /// STRUCT CONTRACTDESC    Size(28)    Location(42-69)
            /// </summary>
            public CONTRACTDESC strContractDesc;
            /// <summary>
            /// STRUCT CONTRACTDESC    Size(28)    Location(42-69)
            /// </summary>
            public CONTRACTDESC Prop03ContractDesc
            {
                get { return strContractDesc; }
                set { strContractDesc = value; }

            }

            /// <summary>
            /// LONG FillNumber         size(4)     Location(70-73)
            /// </summary>
            public Int32 iFillNo;
            /// <summary>
            /// LONG FillNumber         size(4)     Location(70-73)
            /// </summary>
            public Int32 Prop04FillNumber
            {
                get { return iFillNo; }
                set { iFillNo = value; }

            }

            /// <summary>
            /// LONG FillQuantity         size(4)     Location(74-77)
            /// </summary>
            public Int32 iFillQty;
            /// <summary>
            /// LONG FillQuantity         size(4)     Location(74-77)
            /// </summary>
            public Int32 Prop05FillQuantity
            {
                get { return iFillQty; }
                set { iFillQty = value; }

            }

            /// <summary>
            /// LONG FillPrice      size(4)     Location(78-81)
            /// Should Be In multiples of 100
            /// </summary>
            public Int32 iFillPrice;
            /// <summary>
            /// LONG FillPrice      size(4)     Location(78-81)
            /// should be in Multiple of 100
            /// </summary>
            public Int32 Prop06FillPrice
            {
                get { return iFillPrice; }
                set { iFillPrice = value; }

            }

            /// <summary>
            /// CHAR MktType        Size(1) location(82)
            /// </summary>
            public Char[] cMarketType;
            /// <summary>
            /// CHAR MktType        Size(1) location(82)
            /// </summary>
            public string Prop07MktType
            {
                get { return new string(cMarketType); }
                set { cMarketType = CUtility.GetPreciseArrayForString(value.ToString(), cMarketType.Length); }
            }

            /// <summary>
            /// CHAR BuyOpenClose   Size(1) Location(83)
            /// C=Closed and O=Open
            /// </summary>
            public Char[] cBuyOpenClose;
            /// <summary>
            /// CHAR BuyOpenClose   Size(1) Location(83)
            /// C=Closed and O=Open
            /// </summary>
            public string Prop08BuyOpenClose
            {
                get { return new string(cBuyOpenClose); }
                set { cBuyOpenClose = CUtility.GetPreciseArrayForString(value.ToString(), cBuyOpenClose.Length); }
            }

            /// <summary>
            /// LONG NewVolume      Size(4)  Location(84-87)
            /// </summary>
            public Int32 iNewVolume;
            /// <summary>
            /// LONG NewVolume      Size(4)  Location(84-87)
            /// </summary>
            public Int32 Prop09NewVolume
            {
                get { return iNewVolume; }
                set { iNewVolume = value; }

            }

            /// <summary>
            /// CHAR BuyBrokerId [5]    size(5) Location(88-92)
            /// </summary>
            public Char[] cBuyBrokerid;
            /// <summary>
            /// CHAR BuyBrokerId [5]    size(5) Location(88-92)
            /// </summary>
            public string Prop10BuyBrokerId
            {
                get { return new string(cBuyBrokerid); }
                set { cBuyBrokerid = CUtility.GetPreciseArrayForString(value.ToString(), cBuyBrokerid.Length); }
            }

            /// <summary>
            /// CHAR SellBrokerId [5]   size(5)   Location(93-97)
            /// </summary>
            public Char[] cSellBrokerid;
            /// <summary>
            /// CHAR SellBrokerId [5]   size(5)   Location(93-97)
            /// </summary>
            public string Prop11SellBrokerId
            {
                get { return new string(cSellBrokerid); }
                set { cSellBrokerid = CUtility.GetPreciseArrayForString(value.ToString(), cSellBrokerid.Length); }
            }

            /// <summary>
            /// SHORT TraderId      size(2)     Location(98-99)
            /// </summary>
            public Int16 iTraderId;
            /// <summary>
            /// SHORT TraderId      size(2)     Location(98-99)
            /// </summary>
            public Int16 Prop12TraderId
            {
                get { return iTraderId; }
                set { iTraderId = value; }

            }

            /// <summary>
            /// CHAR RequestedBy    Size(1)     Location(100)
            /// • ‘1’ (BUY) if the buy side is requesting
            ///• ‘2’ (SELL) if the sell side is requesting
            ///• ‘3’ if BM/CM of any one side requesting or when both theparties requesting modification.
            /// </summary>
            public Char[] cRequestedBy;
            /// <summary>
            /// CHAR RequestedBy    Size(1)     Location(100)
            /// • ‘1’ (BUY) if the buy side is requesting
            /// • ‘2’ (SELL) if the sell side is requesting
            /// • ‘3’ if BM/CM of any one side requesting or when both theparties requesting modification.
            /// </summary>
            public string Prop13RequestedBy
            {
                get { return new string(cRequestedBy); }
                set { cRequestedBy = CUtility.GetPreciseArrayForString(value.ToString(), cRequestedBy.Length); }
            }

            /// <summary>
            /// CHAR SellOpenClose  size(1)     Location(101)
            /// </summary>
            public Char[] cSellOpenClose;
            /// <summary>
            /// CHAR SellOpenClose  size(1)     Location(101)
            /// </summary>
            public string Prop14SellOpenClose
            {
                get { return new string(cSellOpenClose); }
                set { cSellOpenClose = CUtility.GetPreciseArrayForString(value.ToString(), cSellOpenClose.Length); }
            }

            /// <summary>
            /// CHAR BuyAccountNumber [10]  Size(102-111)
            /// </summary>
            public Char[] cBuyAccountNo;
            /// <summary>
            /// CHAR BuyAccountNumber [10]  Size(102-111)
            /// </summary>
            public string Prop15BuyAccountNo
            {
                get { return new string(cBuyAccountNo); }
                set { cBuyAccountNo = CUtility.GetPreciseArrayForString(value.ToString(), cBuyAccountNo.Length); }
            }

            /// <summary>
            /// CHAR SellAccountNumber [10]     size(112-121)
            /// </summary>
            public Char[] cSellAccountNo;
            /// <summary>
            /// CHAR SellAccountNumber [10]     size(112-121)
            /// </summary>
            public string Prop16SellAccountNo
            {
                get { return new string(cSellAccountNo); }
                set { cSellAccountNo = CUtility.GetPreciseArrayForString(value.ToString(), cSellAccountNo.Length); }
            }

            /// <summary>
            /// CHAR BuyParticipant [12]    Size(12)    location(122-133)
            /// </summary>
            public Char[] cBuyParticipant;
            /// <summary>
            /// CHAR BuyParticipant [12]    Size(12)    location(122-133)
            /// </summary>
            public string Prop17BuyParticipant
            {
                get { return new string(cBuyParticipant); }
                set { cBuyParticipant = CUtility.GetPreciseArrayForString(value.ToString(), cBuyParticipant.Length); }
            }

            /// <summary>
            /// CHAR SellParticipant [12]       Size(12)    Location(134-145)
            /// </summary>
            public Char[] cSellParticipant;
            /// <summary>
            /// CHAR SellParticipant [12]       Size(12)    Location(134-145)
            /// </summary>
            public string Prop18SellParticipant
            {
                get { return new string(cSellParticipant); }
                set { cSellParticipant = CUtility.GetPreciseArrayForString(value.ToString(), cSellParticipant.Length); }
            }

            /// <summary>
            /// CHAR BuyCoverUncover    Size(1) Location(146)
            /// </summary>
            public Char[] cBuyCovUncov;
            /// <summary>
            /// CHAR BuyCoverUncover    Size(1) Location(146)
            /// </summary>
            public string Prop19BuyCoverUncover
            {
                get { return new string(cBuyCovUncov); }
                set { cBuyCovUncov = CUtility.GetPreciseArrayForString(value.ToString(), cBuyCovUncov.Length); }
            }

            /// <summary>
            /// CHAR SellCoverUncover       Size(1)     Location(147)
            /// </summary>
            public Char[] cSellCovUncov;
            /// <summary>
            /// CHAR SellCoverUncover       Size(1)     Location(147)
            /// </summary>
            public string Prop20SellCoverUncover
            {
                get { return new string(cSellCovUncov); }
                set { cSellCovUncov = CUtility.GetPreciseArrayForString(value.ToString(), cSellCovUncov.Length); }
            }

            /// <summary>
            /// CHAR BuyGiveupFlag          Size(1)     Location(148)
            /// </summary>
            public Char[] cBuyGiveup;
            /// <summary>
            /// CHAR BuyGiveupFlag          Size(1)     Location(148)
            /// </summary>
            public string Prop21BuyGiveUpFlag
            {
                get { return new string(cBuyGiveup); }
                set { cBuyGiveup = CUtility.GetPreciseArrayForString(value.ToString(), cBuyGiveup.Length); }
            }

            /// <summary>
            /// CHAR SellGiveupFlag         Size(1)     Location(149)
            /// </summary>
            public Char[] cSellGiveup;
            /// <summary>
            /// CHAR SellGiveupFlag         Size(1)     Location(149)
            /// </summary>
            public string Prop22SellGiveUpFlag
            {
                get { return new string(cSellGiveup); }
                set { cSellGiveup = CUtility.GetPreciseArrayForString(value.ToString(), cSellGiveup.Length); }
            }

            public byte[] StructToByte()
            {
                byte[] Tradeinqdata = new byte[CConstants.TradeModificationsize];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, Tradeinqdata, 0, 38);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTokenNo), 0, Tradeinqdata, 38, 4);
                    Buffer.BlockCopy(strContractDesc.StructToByte(), 0, Tradeinqdata, 42, 28);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iFillNo), 0, Tradeinqdata, 70, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iFillQty), 0, Tradeinqdata, 74, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iFillPrice), 0, Tradeinqdata, 78, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cMarketType), 0, Tradeinqdata, 82, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBuyOpenClose), 0, Tradeinqdata, 83, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iNewVolume), 0, Tradeinqdata, 84, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBuyBrokerid), 0, Tradeinqdata, 88, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSellBrokerid), 0, Tradeinqdata, 93, 5);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTraderId), 0, Tradeinqdata, 98, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cRequestedBy), 0, Tradeinqdata, 100, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSellOpenClose), 0, Tradeinqdata, 101, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBuyAccountNo), 0, Tradeinqdata, 102, 10);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSellAccountNo), 0, Tradeinqdata, 112, 10);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBuyParticipant), 0, Tradeinqdata, 122, 12);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSellParticipant), 0, Tradeinqdata, 134, 12);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBuyCovUncov), 0, Tradeinqdata, 146, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSellCovUncov), 0, Tradeinqdata, 147, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBuyGiveup), 0, Tradeinqdata, 148, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSellGiveup), 0, Tradeinqdata, 149, 1);
                    return Tradeinqdata;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                strContractDesc = new CONTRACTDESC(11);
                try
                {

                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02TokenNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 38));
                    Prop03ContractDesc.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 42, 28));
                    Prop04FillNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 70));
                    Prop05FillQuantity = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 74));
                    Prop06FillPrice = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 78));
                    Prop07MktType = Encoding.ASCII.GetString(ByteStructure, 82, 1);
                    Prop08BuyOpenClose = Encoding.ASCII.GetString(ByteStructure, 83, 1);
                    Prop09NewVolume = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 84));
                    Prop10BuyBrokerId = Encoding.ASCII.GetString(ByteStructure, 88, 5);
                    Prop11SellBrokerId = Encoding.ASCII.GetString(ByteStructure, 93, 5);
                    Prop12TraderId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 98));
                    Prop13RequestedBy = Encoding.ASCII.GetString(ByteStructure, 100, 1);
                    Prop14SellOpenClose = Encoding.ASCII.GetString(ByteStructure, 101, 1);
                    Prop15BuyAccountNo = Encoding.ASCII.GetString(ByteStructure, 102, 10);
                    Prop16SellAccountNo = Encoding.ASCII.GetString(ByteStructure, 112, 10); ;
                    Prop17BuyParticipant = Encoding.ASCII.GetString(ByteStructure, 122, 12);
                    Prop18SellParticipant = Encoding.ASCII.GetString(ByteStructure, 134, 12);
                    Prop19BuyCoverUncover = Encoding.ASCII.GetString(ByteStructure, 146, 1);
                    Prop20SellCoverUncover = Encoding.ASCII.GetString(ByteStructure, 147, 1);
                    Prop21BuyGiveUpFlag = Encoding.ASCII.GetString(ByteStructure, 148, 1);
                    Prop22SellGiveUpFlag = Encoding.ASCII.GetString(ByteStructure, 149, 1);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public TradeModificationRequest(Int16 TransCode)
            {

                strHeader = new MessageHeader(0);
                iTokenNo = 0;
                strContractDesc = new CONTRACTDESC(0);
                iFillNo = 0;
                iFillQty = 0;
                iFillPrice = 0;
                cMarketType = new char[1];
                cBuyOpenClose = new char[1];
                iNewVolume = 0;
                cBuyBrokerid = new char[5];
                cSellBrokerid = new char[5];
                iTraderId = 0;
                cRequestedBy = new char[1];
                cSellOpenClose = new char[1];
                cBuyAccountNo = new char[10];
                cSellAccountNo = new char[10];
                cBuyParticipant = new char[12];
                cSellParticipant = new char[12];
                cBuyCovUncov = new char[1];
                cSellCovUncov = new char[1];
                cBuyGiveup = new char[1];
                cSellGiveup = new char[1];
                Prop01Header = new MessageHeader(TransCode);
                Prop13RequestedBy = "";

            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + Prop02TokenNumber.ToString() + Prop03ContractDesc.ToString() +
                        Prop04FillNumber.ToString() + Prop06FillPrice.ToString() + Prop07MktType + Prop08BuyOpenClose + Prop09NewVolume.ToString() +
                        Prop10BuyBrokerId.ToString() + Prop11SellBrokerId + Prop12TraderId.ToString() + Prop13RequestedBy + Prop14SellOpenClose + Prop15BuyAccountNo +
                        Prop16SellAccountNo + Prop17BuyParticipant + Prop18SellParticipant + Prop19BuyCoverUncover + Prop20SellCoverUncover + Prop21BuyGiveUpFlag + Prop22SellGiveUpFlag);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

        }
        #endregion

        #region All Spread Order Structures

        #region LEG INFO
        /// <summary>
        /// MSSPDLEGINFO : size(80) Location(0-79)
        /// </summary>
        public struct LEGINFO : IStruct
        {
            /*
             *      LONG Token2                 4      4
                    struct CONTRACTDESC        28      32
                    CHAR OpBrokerId2 [5]        5      37
                    CHAR Fillerx2               1      38
                    SHORT OrderType2            2      40
                    SHORT BuySell2              2      42
                    LONG DisclosedVol2          4      46
                    LONG DisclosedVolRemaining2 4      50
                    LONG TotalVolRemaining2     4      54
                    LONG Volume2                4      58   
                    LONG VolumeFilledToday2     4      62
                    LONG Price2                 4      66
                    LONG TriggerPrice2          4      70
             *      LONG MinFillAon2            4      74
             *      struct STORDERFLAGS         2      76
             *      CHAR OpenClose2             1      77
                    CHAR CoverUncover2          1      78
                    CHAR GiveupFlag2            1      79
                    CHAR FillerY                1      80
             * ==============================================
             *      Total                       80
             * ==============================================
             */
            /// <summary>
            /// LONG Token2    : Size(4) Location(0-3)
            /// </summary>
            public Int32 iToken2;
            /// <summary>
            /// LONG Token2    : Size(4) Location(0-3)
            /// </summary>
            public Int32 Prop01Token2
            {
                get { return iToken2; }
                set { iToken2 = value; }

            }

            /// <summary>
            /// struct CONTRACTDESC       Size(28)     Location(4-31)
            /// </summary>
            public CONTRACTDESC strContractDesc;
            /// <summary>
            /// struct CONTRACTDESC       Size(28)     Location(4-31)
            /// </summary>
            public CONTRACTDESC Prop02ContractDesc
            {
                get { return strContractDesc; }
                set { strContractDesc = value; }

            }

            /// <summary>
            /// CHAR OpBrokerId2 [5]        size=5     location(32-36)
            /// </summary>
            public char[] cOpBrokerId2;
            /// <summary>
            ///  CHAR OpBrokerId2 [5]        size=5     location(32-36)
            /// </summary>
            public String Prop03OpBrokerId2
            {
                get { return new string(cOpBrokerId2); }
                set { cOpBrokerId2 = CUtility.GetPreciseArrayForString(value.ToString(), cOpBrokerId2.Length); }
            }

            /// <summary>
            /// CHAR Fillerx2               Size=1      Location(37)
            /// </summary>
            public char[] cFillerx2;
            /// <summary>
            ///  CHAR Fillerx2               Size=1      Location(37)
            /// </summary>
            public String Prop04Fillerx2
            {
                get { return new string(cFillerx2); }
                set { cFillerx2 = CUtility.GetPreciseArrayForString(value.ToString(), cFillerx2.Length); }
            }

            /// <summary>
            /// SHORT OrderType2            Size=2      Location=(38-39)
            /// </summary>
            public Int16 iOrderType2;
            /// <summary>
            /// SHORT OrderType2            Size=2      Location=(38-39)
            /// </summary>
            public Int16 Prop05OrderType2
            {
                get { return iOrderType2; }
                set { iOrderType2 = value; }
            }

            /// <summary>
            ///  SHORT BuySell2              Size=2      Location(40-41)
            /// </summary>
            public Int16 iBuySell2;
            /// <summary>
            ///  SHORT BuySell2              Size=2      Location(40-41)
            /// </summary>
            public Int16 Prop06BuySell2
            {
                get { return iBuySell2; }
                set { iBuySell2 = value; }
            }

            /// <summary>
            /// LONG DisclosedVol2          Size=4      Location(42-45) 
            /// </summary>
            public Int32 iDisclosedVol2;
            /// <summary>
            /// LONG DisclosedVol2          Size=4      Location(42-45)
            /// </summary>
            public Int32 Prop07DisclosedVol2
            {
                get { return iDisclosedVol2; }
                set { iDisclosedVol2 = value; }
            }

            /// <summary>
            /// LONG DisclosedVolRemaining2 Size=4      Location(46-49)
            /// </summary>
            public Int32 iDisclosedVolRemaining2;
            /// <summary>
            /// LONG DisclosedVolRemaining2 Size=4      Location(46-49)
            /// </summary>
            public Int32 Prop08DisclosedVolRemaining2
            {
                get { return iDisclosedVolRemaining2; }
                set { iDisclosedVolRemaining2 = value; }
            }

            /// <summary>
            ///  LONG TotalVolRemaining2     Size=4      Location(50-53)
            /// </summary>
            public Int32 iTotalVolRemaining2;
            /// <summary>
            ///  LONG TotalVolRemaining2     Size=4      Location(50-53)
            /// </summary>
            public Int32 Prop09TotalVolRemaining2
            {
                get { return iTotalVolRemaining2; }
                set { iTotalVolRemaining2 = value; }
            }

            /// <summary>
            ///  LONG Volume2                Size=4      Location(54-57)   
            /// </summary>
            public Int32 iVolume2;
            /// <summary>
            /// LONG Volume2                Size=4      Location(54-57)  
            /// </summary>
            public Int32 Prop10Volume2
            {
                get { return iVolume2; }
                set { iVolume2 = value; }
            }

            /// <summary>
            /// LONG VolumeFilledToday2    Size=4      Location(58-61)
            /// </summary>
            public Int32 iVolumeFilledToday2;
            /// <summary>
            /// LONG VolumeFilledToday2    Size=4      Location(58-61)
            /// </summary>
            public Int32 Prop11VolumeFilledToday2
            {
                get { return iVolumeFilledToday2; }
                set { iVolumeFilledToday2 = value; }
            }

            /// <summary>
            ///  LONG Price2                 Size=4      Location(62-65)
            /// </summary>
            public Int32 iPrice2;
            /// <summary>
            ///  LONG Price2                 Size=4      Location(62-65)
            /// </summary>
            public Int32 Prop12Price2
            {
                get { return iPrice2; }
                set { iPrice2 = value; }
            }

            /// <summary>
            /// LONG TriggerPrice2          Size=4      Location(66-69)
            /// </summary>
            public Int32 iTriggerPrice2;
            /// <summary>
            /// LONG TriggerPrice2          Size=4      Location(66-69)
            /// </summary>
            public Int32 Prop13TriggerPrice2
            {
                get { return iTriggerPrice2; }
                set { iTriggerPrice2 = value; }
            }

            /// <summary>
            ///  LONG MinFillAon2            Size=4      Location(70-73)
            /// </summary>
            public Int32 iMinFillAon2;
            /// <summary>
            ///  LONG MinFillAon2            Size=4      Location(70-73)
            /// </summary>
            public Int32 Prop14MinFillAon2
            {
                get { return iMinFillAon2; }
                set { iMinFillAon2 = value; }
            }

            ///// <summary>
            ///// struct STORDERFLAGS      Size=2      Location=(74-75)
            ///// </summary>
            //public OrderFlag strOrderFlag;
            ///// <summary>
            ///// struct STORDERFLAGS      Size=2      Location=(74-75)
            ///// </summary>
            //public OrderFlag Prop15OrderFlag
            //{
            //    get { return strOrderFlag; }
            //    set { strOrderFlag = value; }

            //}

            public Int16 iflag;
            public Int16 Prop15OrderFlag
            {
                get { return iflag; }
                set { iflag = value; }
            }

            /// <summary>
            /// CHAR OpenClose2             Size=1      Location(76)
            /// </summary>
            public char[] cOpenClose2;
            /// <summary>
            /// CHAR OpenClose2             Size=1      Location(76)
            /// </summary>
            public String Prop16OpenClose2
            {
                get { return new string(cOpenClose2); }
                set { cOpenClose2 = CUtility.GetPreciseArrayForString(value.ToString(), cOpenClose2.Length); }
            }

            /// <summary>
            /// CHAR CoverUncover2          Size=1      Location(77)
            /// </summary>
            public char[] cCoverUncover2;
            /// <summary>
            /// 
            /// </summary>
            public String Prop17CoverUncover2
            {
                get { return new string(cCoverUncover2); }
                set { cCoverUncover2 = CUtility.GetPreciseArrayForString(value.ToString(), cCoverUncover2.Length); }
            }

            /// <summary>
            ///  CHAR GiveupFlag2           Size=1      Location(78)
            /// </summary>
            public char[] cGiveupFlag2;
            /// <summary>
            /// CHAR GiveupFlag2           Size=1      Location(78)
            /// </summary>
            public String Prop18GiveupFlag2
            {
                get { return new string(cGiveupFlag2); }
                set { cGiveupFlag2 = CUtility.GetPreciseArrayForString(value.ToString(), cGiveupFlag2.Length); }
            }

            /// <summary>
            /// CHAR FillerY                Size=1      Location(79)
            /// </summary>
            public char[] cFillerY;
            /// <summary>
            /// CHAR FillerY                Size=1      Location(79)
            /// </summary>
            public String Prop19FillerY
            {
                get { return new string(cFillerY); }
                set { cFillerY = CUtility.GetPreciseArrayForString(value.ToString(), cFillerY.Length); }
            }
            public byte[] StructToByte()
            {
                byte[] LegInfo = new byte[CConstants.LegInfo];
                try
                {
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iToken2), 0, LegInfo, 0, 4);
                    Buffer.BlockCopy(strContractDesc.StructToByte(), 0, LegInfo, 4, 28);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOpBrokerId2), 0, LegInfo, 32, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFillerx2), 0, LegInfo, 37, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iOrderType2), 0, LegInfo, 38, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBuySell2), 0, LegInfo, 40, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVol2), 0, LegInfo, 42, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVolRemaining2), 0, LegInfo, 46, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTotalVolRemaining2), 0, LegInfo, 50, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVolume2), 0, LegInfo, 54, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVolumeFilledToday2), 0, LegInfo, 58, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iPrice2), 0, LegInfo, 62, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTriggerPrice2), 0, LegInfo, 66, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iMinFillAon2), 0, LegInfo, 70, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iflag), 0, LegInfo, 74, 2);
                    //Buffer.BlockCopy(strOrderFlag.StructToByte(), 0, LegInfo, 74, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOpenClose2), 0, LegInfo, 76, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cCoverUncover2), 0, LegInfo, 77, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cGiveupFlag2), 0, LegInfo, 78, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFillerY), 0, LegInfo, 79, 1);
                    return LegInfo;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    strContractDesc = new CONTRACTDESC(0);
                    Prop01Token2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 0));
                    strContractDesc.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 4, 28));
                    Prop02ContractDesc = strContractDesc;
                    Prop03OpBrokerId2 = Encoding.ASCII.GetString(ByteStructure, 32, 5);
                    Prop04Fillerx2 = Encoding.ASCII.GetString(ByteStructure, 37, 1);
                    Prop05OrderType2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                    Prop06BuySell2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 40));
                    Prop07DisclosedVol2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 42));
                    Prop08DisclosedVolRemaining2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 46));
                    Prop09TotalVolRemaining2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 50));
                    Prop10Volume2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 54));
                    Prop11VolumeFilledToday2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 58));
                    Prop12Price2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 62));
                    Prop13TriggerPrice2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 66));
                    Prop14MinFillAon2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 70));
                    Prop15OrderFlag = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 74));
                    //Prop15OrderFlag.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 74, 2));
                    Prop16OpenClose2 = Encoding.ASCII.GetString(ByteStructure, 76, 1);
                    Prop17CoverUncover2 = Encoding.ASCII.GetString(ByteStructure, 77, 1);
                    Prop18GiveupFlag2 = Encoding.ASCII.GetString(ByteStructure, 78, 1);
                    Prop19FillerY = Encoding.ASCII.GetString(ByteStructure, 79, 1);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Token2.ToString() + CConstants.Seperator +
                        Prop02ContractDesc.ToString() + CConstants.Seperator +
                        Prop03OpBrokerId2 + +CConstants.Seperator +
                        Prop04Fillerx2 + CConstants.Seperator +
                        Prop05OrderType2.ToString() + CConstants.Seperator +
                        Prop06BuySell2.ToString() + CConstants.Seperator +
                        Prop07DisclosedVol2.ToString() + CConstants.Seperator +
                        Prop08DisclosedVolRemaining2.ToString() + CConstants.Seperator +
                        Prop09TotalVolRemaining2.ToString() + CConstants.Seperator +
                        Prop10Volume2.ToString() + CConstants.Seperator +
                        Prop11VolumeFilledToday2.ToString() + CConstants.Seperator +
                        Prop12Price2.ToString() + CConstants.Seperator +
                        Prop13TriggerPrice2.ToString() + CConstants.Seperator +
                        Prop14MinFillAon2.ToString() + CConstants.Seperator +
                        Prop15OrderFlag.ToString() + CConstants.Seperator +
                        Prop16OpenClose2 + CConstants.Seperator +
                        Prop17CoverUncover2 + CConstants.Seperator +
                        Prop18GiveupFlag2 + CConstants.Seperator + Prop19FillerY + CConstants.Seperator);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public LEGINFO(int trancode)
            {
                iToken2 = 0;
                strContractDesc = new CONTRACTDESC(0);
                cOpBrokerId2 = new char[5];
                cFillerx2 = new char[1];
                iOrderType2 = 0;
                iBuySell2 = 0;
                iDisclosedVol2 = 0;
                iDisclosedVolRemaining2 = 0;
                iTotalVolRemaining2 = 0;
                iVolume2 = 0;
                iVolumeFilledToday2 = 0;
                iPrice2 = 0;
                iTriggerPrice2 = 0;
                iMinFillAon2 = 0;
                iflag = 0;
                //strOrderFlag = new OrderFlag(0);
                cOpenClose2 = new char[1];
                cCoverUncover2 = new char[1];
                cGiveupFlag2 = new char[1];
                cFillerY = new char[1];

            }
        }
        #endregion

        #region Spread Orders
        public struct SpreadOrderRequest : IStruct
        {
            /*
                    struct MESSAGEHEADER Header1           38  38
                    CHAR ParticipantType1                   1   39
                    CHAR Filler1                            1   40
                    SHORT CompetitorPeriod1                 2   42
                    SHORT SolicitorPeriod1                  2   44
                    CHAR ModCxlBy1                          1   45
                    CHAR Filler9                            1   46    
                    SHORT ReasonCode1                       2   48
                    CHAR StartAlpha1[2]                     2   50
                    CHAR EndAlpha1[2]                       2   52
                    LONG Token1                             4   56    
                    struct CONTRACTDESC                    28  84
                    CHAR OpBrokerId1 [5]                    5   89 
                    CHAR Fillerx1                           1   90
                    CHAR FillerOptions1[3]                  3   93
                    CHAR Fillery1                           1   94
                    SHORT OrderType1                        2   96
                    DOUBLE OrderNumber1                     8   104
                    CHAR AccountNumber1 [10]                10  114
                    SHORT BookType1                         2   116
                    SHORT BuySell1                          2   118
                    LONG DisclosedVol1                      4   122
                    LONG DisclosedVolRemaining1             4   126
                    LONG TotalVolRemaining1                 4   130
                    LONG Volume1                            4   134
                    LONG VolumeFilledToday1                 4   138
                    LONG Price1                             4   142 
                    LONG TriggerPrice1                      4   146
                    LONG GoodTillDate1                      4   150
                    LONG EntryDateTime1                     4   154
                    LONG MinFillAon1                        4   158
                    LONG LastModified1                      4   162
             *      struct STORDERFLAGS                     2   164
                    SHORT BranchId1                         2   166
                    SHORT TraderId1                         2   168
                    CHAR BrokerId1 [5]                      5   173
                    CHAR OERemarks1 [24]                    24  197
                    CHAR OpenClose1                         1   199
                    CHAR Settlor1 [12]                      12  210
                    SHORT ProClient1                        2   212
                    SHORT SettlementPeriod1                 2   214
                    CHAR CoverUncover1                      1   215
                    CHAR GiveupFlag1                        1   216
                    OrderFiller                             2   218
                    CHAR filler17                           1   219
                    CHAR filler18                           1   220
                    DOUBLE NnfField                         8   228
                    DOUBLE MktReplay                        8   236
                    LONG PriceDiff                          4   240
                 *  struct LegInfo                          80  320
                 *  struct LegInfo                          80  400
                 * =======================================================
                 *  Total                                   400 400
                 *======================================================== 
               
                */
            /// <summary>
            ///struct MESSAGEHEADER Header1      size(38) Location(0-37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// struct MESSAGEHEADER Header1      size(38) Location(0-37)
            /// </summary>
            public MessageHeader prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            /// <summary>
            /// CHAR ParticipantType1:Size(1)  Location(38)
            /// </summary>
            char[] cParticipantType1;
            /// <summary>
            /// CHAR ParticipantType1:Size(1)  Location(38)
            /// </summary>
            public String prop02ParticipantType1
            {
                get { return new string(cParticipantType1); }
                set { cParticipantType1 = CUtility.GetPreciseArrayForString(value.ToString(), cParticipantType1.Length); }
            }

            /// <summary>
            /// CHAR Filler1  : Size(1) Location(39)
            /// </summary>
            public char[] cFiller1;
            /// <summary>
            /// CHAR Filler1  : Size(1) Location(39)
            /// </summary>
            public String Prop03Filler1
            {
                get { return new string(cFiller1); }
                set { cFiller1 = CUtility.GetPreciseArrayForString(value.ToString(), cFiller1.Length); }

            }

            /// <summary>
            /// SHORT CompetitorPeriod1 :size(2) Location(40-41)
            /// </summary>
            public Int16 iCompetitorPeriod1;
            /// <summary>
            /// SHORT CompetitorPeriod1 :size(2) Location(40-41)
            /// </summary>
            public Int16 Prop04CompetitorPeriod1
            {
                get { return iCompetitorPeriod1; }
                set { iCompetitorPeriod1 = value; }
            }
            /// <summary>
            /// SHORT SolicitorPeriod :size(2) location(42-43)
            /// </summary>
            public Int16 iSolicitorPeriod1;
            /// <summary>
            /// SHORT SolicitorPeriod: size(2) location(42-43)
            /// </summary>
            public Int16 Prop05SolicitorPeriod
            {
                get { return iSolicitorPeriod1; }
                set { iSolicitorPeriod1 = value; }
            }

            /// <summary>
            /// CHAR ModCxlBy1: size(1) Location(44)
            /// </summary>
            public char[] cModCxlBy1;
            /// <summary>
            /// CHAR ModCxlBy1 : size(1) Location(44)
            /// </summary>
            public string Prop06ModCxlBy1
            {
                get { return new string(cModCxlBy1); }
                set { cModCxlBy1 = CUtility.GetPreciseArrayForString(value.ToString(), cModCxlBy1.Length); }
            }

            /// <summary>
            /// CHAR Filler9 Size(1) Location(45)
            /// </summary>
            public char[] cFiller9;
            /// <summary>
            /// CHAR Filler9 Size(1) Location(45)
            /// </summary>
            public string Prop07Filler9
            {
                get { return new string(cFiller9); }
                set { cFiller9 = CUtility.GetPreciseArrayForString(value.ToString(), cFiller9.Length); }
            }

            /// <summary>
            /// SHORT ReasonCode1 Size(2) Location(46-47)
            /// </summary>
            public Int16 iReasonCode1;
            /// <summary>
            /// SHORT ReasonCode1 Size(2) Location(46-47)
            /// </summary>
            public Int16 Prop08ReasonCode1
            {
                get { return iReasonCode1; }
                set { iReasonCode1 = value; }
            }

            /// <summary>
            /// CHAR StartAlpha1[2] Size(2) Location(48-49)
            /// </summary>
            public char[] cStartAlpha1;
            /// <summary>
            /// CHAR StartAlpha1[2] Size(2) Location(48-49)
            /// </summary>
            public String Prop09StartAlpha1
            {
                get { return new string(cStartAlpha1); }
                set { cStartAlpha1 = CUtility.GetPreciseArrayForString(value.ToString(), cStartAlpha1.Length); }
            }

            /// <summary>
            /// CHAR EndAlpha1[2] : Size(2) Location(50-51)
            /// </summary>
            public char[] cEndAlpha1;
            /// <summary>
            /// CHAR EndAlpha1[2] : Size(2) Location(50-51)
            /// </summary>
            public String Prop10EndAlpha1
            {
                get { return new string(cEndAlpha1); }
                set { cEndAlpha1 = CUtility.GetPreciseArrayForString(value.ToString(), cEndAlpha1.Length); }
            }

            /// <summary>
            /// LONG Token1 Size(4) Location(52-55)
            /// </summary>
            public Int32 iToken1;
            /// <summary>
            /// LONG Token1 Size(4) Location(52-55)
            /// </summary>
            public Int32 Prop11Token1
            {
                get { return iToken1; }
                set { iToken1 = value; }
            }

            /// <summary>
            /// struct CONTRACTDESC Size(28) Location(56-83)
            /// </summary>
            public CONTRACTDESC strContractDesc;
            /// <summary>
            /// struct CONTRACTDESC Size(28) Location(56-83)
            /// </summary>
            public CONTRACTDESC Prop12ContractDesc
            {
                get { return strContractDesc; }
                set { strContractDesc = value; }
            }
            /// <summary>
            /// CHAR OpBrokerId1 [5] Size(5) Location(84-88)
            /// </summary>
            public char[] cOpBrokerId1;
            /// <summary>
            /// CHAR OpBrokerId1 [5] Size(5) Location(84-88)
            /// </summary>
            public String Prop13OpBrokerId1
            {
                get { return new string(cOpBrokerId1); }
                set { cOpBrokerId1 = CUtility.GetPreciseArrayForString(value.ToString(), cOpBrokerId1.Length); }
            }

            /// <summary>
            /// CHAR Fillerx1 : Size(1) Location(89)
            /// </summary>
            public char[] cFillerx1;
            /// <summary>
            /// CHAR Fillerx1 : Size(1) Location(89)
            /// </summary>
            public String Prop14Fillerx1
            {
                get { return new string(cFillerx1); }
                set { cFillerx1 = CUtility.GetPreciseArrayForString(value.ToString(), cFillerx1.Length); }
            }

            /// <summary>
            /// CHAR FillerOptions1[3] :Size(3) Location(90-92)
            /// </summary>
            public char[] cFillerOptions1;
            /// <summary>
            /// CHAR FillerOptions1[3] :Size(3) Location(90-92)
            /// </summary>
            public String Prop15FillerOptions1
            {
                get { return new string(cFillerOptions1); }
                set { cFillerOptions1 = CUtility.GetPreciseArrayForString(value.ToString(), cFillerOptions1.Length); }
            }

            /// <summary>
            /// CHAR Fillery1 : Size(1) Location(93)
            /// </summary>
            public char[] cFillery1;
            /// <summary>
            /// CHAR Fillery1 : Size(1) Location(93)
            /// </summary>
            public String Prop16Fillery1
            {
                get { return new string(cFillery1); }
                set { cFillery1 = CUtility.GetPreciseArrayForString(value.ToString(), cFillery1.Length); }
            }

            /// <summary>
            /// SHORT OrderType1 : size(2) Location(94-95)
            /// </summary>
            public Int16 iOrderType1;
            /// <summary>
            ///  SHORT OrderType1 : size(2) Location(94-95)
            /// </summary>
            public Int16 Prop17OrderType1
            {
                get { return iOrderType1; }
                set { iOrderType1 = value; }
            }

            /// <summary>
            /// DOUBLE OrderNumber1 : Size(8) Location(96-103)
            /// </summary>
            public Double iOrderNumber1;
            /// <summary>
            /// DOUBLE OrderNumber1 : Size(8) Location(96-103)
            /// </summary>
            public Double Prop18OrderNumber1
            {
                get { return iOrderNumber1; }
                set { iOrderNumber1 = value; }
            }

            /// <summary>
            /// CHAR AccountNumber1 [10]: size(10) Location(104-113)
            /// </summary>
            public char[] cAccountNumber1;
            /// <summary>
            /// CHAR AccountNumber1 [10]: size(10) Location(104-113)
            /// </summary>
            public String Prop19AccountNumber1
            {
                get { return new string(cAccountNumber1); }
                set { cAccountNumber1 = CUtility.GetPreciseArrayForString(value.ToString(), cAccountNumber1.Length); }
            }

            /// <summary>
            /// SHORT BookType1: size(2) Location(114-115)
            /// </summary>
            public Int16 iBookType1;
            /// <summary>
            /// SHORT BookType1: size(2) Location(114-115)
            /// </summary>
            public Int16 Prop20BookType1
            {
                get { return iBookType1; }
                set { iBookType1 = value; }
            }

            /// <summary>
            /// SHORT BuySell1 Size(2) Location(116-117)
            /// </summary>
            public Int16 iBuySell1;
            /// <summary>
            /// SHORT BuySell1 Size(2) Location(116-117)
            /// </summary>
            public Int16 Prop21BuySell1
            {
                get { return iBuySell1; }
                set { iBuySell1 = value; }
            }

            /// <summary>
            /// LONG DisclosedVol1 size(4) Location(118-121)
            /// </summary>
            public Int32 iDisclosedVol1;
            /// <summary>
            /// LONG DisclosedVol1 size(4) Location(118-121)
            /// </summary>
            public Int32 Prop22DisclosedVol1
            {
                get { return iDisclosedVol1; }
                set { iDisclosedVol1 = value; }
            }

            /// <summary>
            /// LONG DisclosedVolRemaining1 size(4) Location(122-125)
            /// </summary>
            public Int32 iDisclosedVolRemaining1;
            /// <summary>
            /// LONG DisclosedVolRemaining1 size(4) Location(122-125)
            /// </summary>
            public Int32 Prop23DisclosedVolRemaining
            {
                get { return iDisclosedVolRemaining1; }
                set { iDisclosedVolRemaining1 = value; }
            }

            /// <summary>
            /// LONG TotalVolRemaining1 Size(4) Location(126-129)
            /// </summary>
            public Int32 iTotalVolRemaining1;
            /// <summary>
            /// LONG TotalVolRemaining1 Size(4) Location(126-129)
            /// </summary>
            public Int32 Prop24TotalVolRemaining1
            {
                get { return iTotalVolRemaining1; }
                set { iTotalVolRemaining1 = value; }
            }

            /// <summary>
            /// LONG Volume1 Size(4) Location(130-133)
            /// </summary>
            public Int32 iVolume1;
            /// <summary>
            /// LONG Volume1 Size(4) Location(130-133)
            /// </summary>
            public Int32 Prop25Volume1
            {
                get { return iVolume1; }
                set { iVolume1 = value; }
            }

            /// <summary>
            /// LONG VolumeFilledToday1 : Size(4) Location(134-137)
            /// </summary>
            public Int32 iVolumeFilledToday1;
            /// <summary>
            /// LONG VolumeFilledToday1 : Size(4) Location(134-137)
            /// </summary>
            public Int32 Prop26VolumeFilledToday1
            {
                get { return iVolumeFilledToday1; }
                set { iVolumeFilledToday1 = value; }
            }

            /// <summary>
            /// LONG Price1 : Size(4) Location(138-141)
            /// </summary>
            public Int32 iPrice1;
            /// <summary>
            /// LONG Price1 : Size(4) Location(138-141)
            /// </summary>
            public Int32 Prop27Price1
            {
                get { return iPrice1; }
                set { iPrice1 = value; }
            }

            /// <summary>
            /// LONG TriggerPrice1 : size(4) Location(142-145)
            /// </summary>
            public Int32 iTriggerPrice1;
            /// <summary>
            /// LONG TriggerPrice1 : size(4) Location(142-145)
            /// </summary>
            public Int32 Prop28TriggerPrice1
            {
                get { return iTriggerPrice1; }
                set { iTriggerPrice1 = value; }
            }

            /// <summary>
            /// LONG GoodTillDate1 Size(4) Location(147-150)
            /// </summary>
            public Int32 iGoodTillDate1;
            /// <summary>
            /// LONG GoodTillDate1 Size(4) Location(147-150)
            /// </summary>
            public Int32 Prop29GoodTillDate1
            {
                get { return iGoodTillDate1; }
                set { iGoodTillDate1 = value; }
            }

            /// <summary>
            /// LONG EntryDateTime1 Size(4) Location(151-154)
            /// </summary>
            public Int32 iEntryDateTime1;
            /// <summary>
            /// LONG EntryDateTime1 Size(4) Location(151-154)
            /// </summary>
            public Int32 Prop30EntryDateTime1
            {
                get { return iEntryDateTime1; }
                set { iEntryDateTime1 = value; }
            }

            /// <summary>
            /// LONG MinFillAon1 : size(4) location(155-158))
            /// </summary>
            public Int32 iMinFillAon1;
            /// <summary>
            /// LONG MinFillAon1 : size(4) location(155-158))
            /// </summary>
            public Int32 Prop31MinFillAon1
            {
                get { return iMinFillAon1; }
                set { iMinFillAon1 = value; }
            }

            /// <summary>
            /// LONG LastModified1 : Size(4) Location(159-162)
            /// </summary>
            public Int32 iLastModified1;
            /// <summary>
            /// LONG LastModified1 : Size(4) Location(159-162)
            /// </summary>
            public Int32 Prop32LastModified1
            {
                get { return iLastModified1; }
                set { iLastModified1 = value; }
            }

            ///// <summary>
            ///// struct STORDERFLAGS                   Size=2   Location(163-164)
            ///// </summary>
            //public OrderFlag strOrderFlag;
            ///// <summary>
            ///// struct STORDERFLAGS                   Size=2   Location(163-164)
            ///// </summary>
            //public OrderFlag Prop33OrderFlag
            //{
            //    get { return strOrderFlag; }
            //    set { strOrderFlag = value; }

            //}
            public Int16 iflag;
            public Int16 Prop33OrderFlag
            {
                get { return iflag; }
                set { iflag = value; }
            }

            /// <summary>
            /// SHORT BranchId1 : Size(2) Location(165-166)
            /// </summary>
            public Int16 iBranchId1;
            /// <summary>
            /// SHORT BranchId1 : Size(2) Location(165-166)
            /// </summary>
            public Int16 Prop34BranchId1
            {
                get { return iBranchId1; }
                set { iBranchId1 = value; }
            }

            /// <summary>
            /// SHORT TraderId1 : Size(2) location(167-168)
            /// </summary>
            public Int16 iTraderId1;
            /// <summary>
            /// SHORT TraderId1 : Size(2) location(167-168)
            /// </summary>
            public Int16 Prop35TraderId1
            {
                get { return iTraderId1; }
                set { iTraderId1 = value; }
            }

            /// <summary>
            /// CHAR BrokerId1 [5] : size(5) Location(169-173)
            /// </summary>
            public char[] cBrokerId1;
            /// <summary>
            /// CHAR BrokerId1 [5] : size(5) Location(169-173)
            /// </summary>
            public String Prop36BrokerId1
            {
                get { return new string(cBrokerId1); }
                set { cBrokerId1 = CUtility.GetPreciseArrayForString(value.ToString(), cBrokerId1.Length); }
            }

            /// <summary>
            /// CHAR OERemarks1 [24]: size(24) Location(174-197)
            /// </summary>
            public char[] cOERemarks1;
            /// <summary>
            /// CHAR OERemarks1 [24]: size(24) Location(174-197)
            /// </summary>
            public String Prop37OERemarks1
            {
                get { return new string(cOERemarks1); }
                set { cOERemarks1 = CUtility.GetPreciseArrayForString(value.ToString(), cOERemarks1.Length); }
            }

            /// <summary>
            /// CHAR OpenClose1 : Size(1) Location(198)
            /// </summary>
            public char[] cOpenClose1;
            /// <summary>
            /// CHAR OpenClose1 : Size(1) Location(198)
            /// </summary>
            public String Prop38OpenClose1
            {
                get { return new string(cOpenClose1); }
                set { cOpenClose1 = CUtility.GetPreciseArrayForString(value.ToString(), cOpenClose1.Length); }
            }

            /// <summary>
            /// CHAR Settlor1 [12] : Size(12) Location(199-210)
            /// </summary>
            public char[] cSettlor1;
            /// <summary>
            /// CHAR Settlor1 [12] : Size(12) Location(199-210)
            /// </summary>
            public String Prop39Settlor1
            {
                get { return new string(cSettlor1); }
                set { cSettlor1 = CUtility.GetPreciseArrayForString(value.ToString(), cSettlor1.Length); }
            }

            /// <summary>
            /// SHORT ProClient1 : size(2) Location(211-212)
            /// </summary>
            public Int16 iProClient1;
            /// <summary>
            /// SHORT ProClient1 : size(2) Location(211-212)
            /// </summary>
            public Int16 Prop40ProClient1
            {
                get { return iProClient1; }
                set { iProClient1 = value; }
            }

            /// <summary>
            /// SHORT SettlementPeriod1 : size(2) Location(213-314)
            /// </summary>
            public Int16 iSettlementPeriod1;
            /// <summary>
            /// SHORT SettlementPeriod1 : size(2) Location(213-314)
            /// </summary>
            public Int16 Prop41SettlementPeriod1
            {
                get { return iSettlementPeriod1; }
                set { iSettlementPeriod1 = value; }
            }
            /// <summary>
            /// CHAR CoverUncover1 : size(1) Location(215)
            /// </summary>
            public char[] cCoverUncover1;
            /// <summary>
            /// CHAR CoverUncover1 : size(1) Location(215)
            /// </summary>
            public String Prop42CoverUncover1
            {
                get { return new string(cCoverUncover1); }
                set { cCoverUncover1 = CUtility.GetPreciseArrayForString(value.ToString(), cCoverUncover1.Length); }
            }

            /// <summary>
            /// CHAR GiveupFlag1 : size(1) Location(216)
            /// </summary>
            public char[] cGiveupFlag1;
            /// <summary>
            /// CHAR GiveupFlag1 : size(1) Location(216)
            /// </summary>
            public String Prop43GiveupFlag1
            {
                get { return new string(cGiveupFlag1); }
                set { cGiveupFlag1 = CUtility.GetPreciseArrayForString(value.ToString(), cGiveupFlag1.Length); }
            }

            ///// <summary>
            ///// Order Filler : Size(2)  Location(217-218)
            ///// </summary>
            //public OrderFiller strOrderFiller;
            ///// <summary>
            ///// Order Filler : Size(2)  Location(217-218)
            ///// </summary>
            //public OrderFiller Prop44OrderFiller
            //{
            //    get { return strOrderFiller; }
            //    set { strOrderFiller = value; }
            //}

            public Int32 iOrderId;
            public Int32 PropOrderId
            {
                get { return iOrderId; }
                set { iOrderId = value; }
            }

            /// <summary>
            /// CHAR filler17 : size(1) Location(219)
            /// </summary>
            //public char[] cFiller17;
            ///// <summary>
            ///// CHAR filler17 : size(1) Location(219)
            ///// </summary>
            //public String Prop45Filler17
            //{
            //    get { return new string(cFiller17); }
            //    set { cFiller17 = CUtility.GetPreciseArrayForString(value.ToString(), cFiller17.Length); }
            //}
            ///// <summary>
            ///// CHAR filler18 : size(1) Location(220)
            ///// </summary>
            //public char[] cFiller18;
            ///// <summary>
            ///// CHAR filler18 : size(1) Location(220)
            ///// </summary>
            //public String Prop46Filler18
            //{
            //    get { return new string(cFiller18); }
            //    set { cFiller18 = CUtility.GetPreciseArrayForString(value.ToString(), cFiller18.Length); }
            //}

            /// <summary>
            /// DOUBLE NnfField : size(8) Location(221-228)
            /// </summary>
            public Double iNnfField;
            /// <summary>
            /// DOUBLE NnfField : size(8) Location(221-228)
            /// First four digits will bear a code to identify program (0011)/ system(0000) generated orders.
            /// Next five digits will bear the unique approved person code.
            /// Next six digits will contain the location code (pin code).
            /// </summary>
            public Double Prop47NnfField
            {
                get { return iNnfField; }
                set { iNnfField = value; }
            }

            /// <summary>
            /// DOUBLE MktReplay: size(8) Location(229-236)
            /// </summary>
            public Double iMarketReplay;
            /// <summary>
            /// DOUBLE MktReplay: size(8) Location(229-236)
            /// </summary>
            public Double Prop48MarketReplay
            {
                get { return iMarketReplay; }
                set { iMarketReplay = value; }
            }

            /// <summary>
            /// LONG PriceDiff size(4) Location(237-240)
            /// </summary>
            public Int32 iPriceDiff;
            /// <summary>
            /// LONG PriceDiff size(4) Location(237-240)
            /// This is the difference between the prices at which leg2 and leg1 
            /// should trade and it should be less than 9999999.9
            /// </summary>
            public Int32 Prop49PriceDiff
            {
                get { return iPriceDiff; }
                set { iPriceDiff = value; }
            }

            /// <summary>
            /// LEGINFO : size(80) Location(241-319)
            /// </summary>
            public LEGINFO strLeg2;
            /// <summary>
            /// LEGINFO : size(80) Location(241-319)
            /// </summary>
            public LEGINFO Prop50Leg2
            {
                get { return strLeg2; }
                set { strLeg2 = value; }

            }
            /// <summary>
            /// LEGINFO : size(80) Location(320-399)
            /// </summary>
            public LEGINFO strLeg3;
            /// <summary>
            /// LEGINFO : size(80) Location(320-399)
            /// </summary>
            public LEGINFO Prop51Leg3
            {
                get { return strLeg3; }
                set { strLeg3 = value; }

            }

            public byte[] StructToByte()
            {
                byte[] SOrder = new byte[CConstants.SpreadOrderEntryRequest];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, SOrder, 0, 38);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cParticipantType1), 0, SOrder, 38, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFiller1), 0, SOrder, 39, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iCompetitorPeriod1), 0, SOrder, 40, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iSolicitorPeriod1), 0, SOrder, 42, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cModCxlBy1), 0, SOrder, 44, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFiller9), 0, SOrder, 45, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iReasonCode1), 0, SOrder, 46, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cStartAlpha1), 0, SOrder, 48, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cEndAlpha1), 0, SOrder, 50, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iToken1), 0, SOrder, 52, 4);
                    Buffer.BlockCopy(strContractDesc.StructToByte(), 0, SOrder, 56, 28);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOpBrokerId1), 0, SOrder, 84, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFillerx1), 0, SOrder, 89, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFillerOptions1), 0, SOrder, 90, 3);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFillery1), 0, SOrder, 93, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iOrderType1), 0, SOrder, 94, 2);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iOrderNumber1), 0, SOrder, 96, 8);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cAccountNumber1), 0, SOrder, 104, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBookType1), 0, SOrder, 114, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBuySell1), 0, SOrder, 116, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVol1), 0, SOrder, 118, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVolRemaining1), 0, SOrder, 122, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTotalVolRemaining1), 0, SOrder, 126, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVolume1), 0, SOrder, 130, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVolumeFilledToday1), 0, SOrder, 134, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iPrice1), 0, SOrder, 138, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTriggerPrice1), 0, SOrder, 142, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iGoodTillDate1), 0, SOrder, 146, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iEntryDateTime1), 0, SOrder, 150, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iMinFillAon1), 0, SOrder, 154, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iLastModified1), 0, SOrder, 158, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iflag), 0, SOrder, 162, 2);
                    //Buffer.BlockCopy(strOrderFlag.StructToByte(), 0, SOrder, 162, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBranchId1), 0, SOrder, 164, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTraderId1), 0, SOrder, 166, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBrokerId1), 0, SOrder, 168, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOERemarks1), 0, SOrder, 173, 24);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOpenClose1), 0, SOrder, 197, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSettlor1), 0, SOrder, 198, 12);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iProClient1), 0, SOrder, 210, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iSettlementPeriod1), 0, SOrder, 212, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cCoverUncover1), 0, SOrder, 214, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cGiveupFlag1), 0, SOrder, 215, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iOrderId), 0, SOrder, 216, 4);
                    //Buffer.BlockCopy(strOrderFiller.StructToByte(), 0, SOrder, 217, 2);
                    //Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFiller17), 0, SOrder, 219, 1);
                    //Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cFiller18), 0, SOrder, 220, 1);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iNnfField), 0, SOrder, 220, 8);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iMarketReplay), 0, SOrder, 228, 8);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iPriceDiff), 0, SOrder, 236, 4);
                    Buffer.BlockCopy(strLeg2.StructToByte(), 0, SOrder, 240, 80);
                    Buffer.BlockCopy(strLeg3.StructToByte(), 0, SOrder, 320, 80);
                    return SOrder;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    strContractDesc = new CONTRACTDESC(0);
                    strLeg2 = new LEGINFO(0);
                    strLeg3 = new LEGINFO(1);
                    prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    prop02ParticipantType1 = Encoding.ASCII.GetString(ByteStructure, 38, 1);
                    Prop03Filler1 = Encoding.ASCII.GetString(ByteStructure, 39, 1);
                    Prop04CompetitorPeriod1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 40));
                    Prop05SolicitorPeriod = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 42));
                    Prop06ModCxlBy1 = Encoding.ASCII.GetString(ByteStructure, 44, 1);
                    Prop07Filler9 = Encoding.ASCII.GetString(ByteStructure, 45, 1);
                    Prop08ReasonCode1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 46));
                    Prop09StartAlpha1 = Encoding.ASCII.GetString(ByteStructure, 48, 2);
                    Prop10EndAlpha1 = Encoding.ASCII.GetString(ByteStructure, 50, 2);
                    Prop11Token1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 52));
                    strContractDesc.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 56, 28));
                    Prop12ContractDesc = strContractDesc;
                    Prop13OpBrokerId1 = Encoding.ASCII.GetString(ByteStructure, 84, 5);
                    Prop14Fillerx1 = Encoding.ASCII.GetString(ByteStructure, 89, 1);
                    Prop15FillerOptions1 = Encoding.ASCII.GetString(ByteStructure, 90, 3);
                    Prop16Fillery1 = Encoding.ASCII.GetString(ByteStructure, 93, 1);
                    Prop17OrderType1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 94));
                    Prop18OrderNumber1 = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 96, 8));//IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 96));
                    Prop19AccountNumber1 = Encoding.ASCII.GetString(ByteStructure, 104, 10);
                    Prop20BookType1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 114));
                    Prop21BuySell1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 116));
                    Prop22DisclosedVol1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 118));
                    Prop23DisclosedVolRemaining = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 122));
                    Prop24TotalVolRemaining1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 126));
                    Prop25Volume1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 130));
                    Prop26VolumeFilledToday1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 134));
                    Prop27Price1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 138));
                    Prop28TriggerPrice1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 142));
                    Prop29GoodTillDate1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 146));
                    Prop30EntryDateTime1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 150));
                    Prop31MinFillAon1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 154));
                    Prop32LastModified1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 158));
                    //Prop33OrderFlag.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 163, 2));
                    Prop33OrderFlag = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 162));
                    Prop34BranchId1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 164));
                    Prop35TraderId1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 166));
                    Prop36BrokerId1 = Encoding.ASCII.GetString(ByteStructure, 168, 5);
                    Prop37OERemarks1 = Encoding.ASCII.GetString(ByteStructure, 173, 24);
                    Prop38OpenClose1 = Encoding.ASCII.GetString(ByteStructure, 197, 1);
                    Prop39Settlor1 = Encoding.ASCII.GetString(ByteStructure, 198, 12);
                    Prop40ProClient1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 210));
                    Prop41SettlementPeriod1 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 212));
                    Prop42CoverUncover1 = Encoding.ASCII.GetString(ByteStructure, 214, 1);
                    Prop43GiveupFlag1 = Encoding.ASCII.GetString(ByteStructure, 215, 1);
                    PropOrderId = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 216));
                    //Prop44OrderFiller.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 217, 2));
                    //Prop45Filler17 = Encoding.ASCII.GetString(ByteStructure, 219, 1);
                    //Prop46Filler18 = Encoding.ASCII.GetString(ByteStructure, 220, 1);
                    Prop47NnfField = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 220, 8)); //IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 221));
                    Prop48MarketReplay = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 228, 8));//IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 229));
                    Prop49PriceDiff = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 236));
                    strLeg2.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 240, 80));
                    Prop50Leg2 = strLeg2;
                    strLeg3.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 320, 80));
                    Prop51Leg3 = strLeg3;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public SpreadOrderRequest(Int16 Transcode)
            {

                strHeader = new MessageHeader(0);
                cParticipantType1 = new char[1];
                cFiller1 = new char[1];
                iCompetitorPeriod1 = 0;
                iSolicitorPeriod1 = 0;
                cModCxlBy1 = new char[1];
                cFiller9 = new char[1];
                iReasonCode1 = 0;
                cStartAlpha1 = new char[2];
                cEndAlpha1 = new char[2];
                iToken1 = 0;
                strContractDesc = new CONTRACTDESC(0);
                cOpBrokerId1 = new char[5];
                cFillerx1 = new char[1];
                cFillerOptions1 = new char[3];
                cFillery1 = new char[1];
                iOrderType1 = 0;
                iOrderNumber1 = 0;
                cAccountNumber1 = new char[10];
                iBookType1 = 1;//‘1’ – Regular lot order, ‘2’ – Special terms order
                iBuySell1 = 1; //• ‘1’ denotes Buy order,• ‘2’ denotes Sell order
                iDisclosedVol1 = 0;
                iDisclosedVolRemaining1 = 0;
                iTotalVolRemaining1 = 0;
                iVolume1 = 0;
                iVolumeFilledToday1 = 0;
                iPrice1 = 0;
                iTriggerPrice1 = 0;
                iGoodTillDate1 = 0;
                iEntryDateTime1 = 0;
                iMinFillAon1 = 0;
                iLastModified1 = 0;
                iflag = 0;
                //strOrderFlag = new OrderFlag();
                iBranchId1 = 0;
                iTraderId1 = 0;
                cBrokerId1 = new char[5];
                cOERemarks1 = new char[24];
                cOpenClose1 = new char[1];
                cSettlor1 = new char[12];
                iProClient1 = 0;
                iSettlementPeriod1 = 0;
                cCoverUncover1 = new char[1];
                cGiveupFlag1 = new char[1];
                //strOrderFiller = new OrderFiller();
                //cFiller17 = new char[1];
                //cFiller18 = new char[1];
                iOrderId = 0;
                iNnfField = 0;
                iMarketReplay = 0;
                iPriceDiff = 0;
                strLeg2 = new LEGINFO(0);
                strLeg3 = new LEGINFO(0);
                //Prop38OpenClose1 = "O";//• ‘O’ denotes Open ,• ‘C’ denotes Close
                //Prop40ProClient1 = 2;//• ‘1’ represents the client’s order.• ‘2’ represents a broker’s order.
                //Prop41SettlementPeriod1 = 10;
                //Prop42CoverUncover1 = "U";//• ‘U’ for Uncovered.• ‘V’ for Covered.
            }
            public override string ToString()
            {
                try
                {
                    return (prop01Header.ToString() + CConstants.Seperator +
                        prop02ParticipantType1 + CConstants.Seperator +
                        Prop03Filler1 + Prop04CompetitorPeriod1.ToString() + CConstants.Seperator +
                        Prop05SolicitorPeriod.ToString() + CConstants.Seperator +
                        Prop05SolicitorPeriod.ToString() + CConstants.Seperator + Prop06ModCxlBy1 + CConstants.Seperator + Prop07Filler9 + Prop08ReasonCode1.ToString() + CConstants.Seperator +
                        Prop09StartAlpha1 + Prop10EndAlpha1 + CConstants.Seperator +
                        Prop11Token1.ToString() + CConstants.Seperator +
                        Prop12ContractDesc.ToString() + CConstants.Seperator +
                        Prop13OpBrokerId1 + Prop14Fillerx1 + Prop15FillerOptions1 + Prop16Fillery1 + Prop17OrderType1.ToString() + CConstants.Seperator +
                        Prop18OrderNumber1.ToString() + CConstants.Seperator + Prop19AccountNumber1 + CConstants.Seperator + Prop20BookType1.ToString() + CConstants.Seperator + Prop21BuySell1.ToString() + CConstants.Seperator + Prop22DisclosedVol1.ToString() + CConstants.Seperator + Prop23DisclosedVolRemaining.ToString() + CConstants.Seperator +
                        Prop24TotalVolRemaining1.ToString() + CConstants.Seperator +
                        Prop25Volume1.ToString() + CConstants.Seperator +
                        Prop26VolumeFilledToday1.ToString() + CConstants.Seperator + Prop27Price1.ToString() + CConstants.Seperator + Prop28TriggerPrice1.ToString() + CConstants.Seperator + Prop29GoodTillDate1.ToString() + CConstants.Seperator +
                        Prop30EntryDateTime1.ToString() + CConstants.Seperator + Prop31MinFillAon1.ToString() + Prop32LastModified1.ToString() + CConstants.Seperator +
                        Prop33OrderFlag.ToString() + CConstants.Seperator + Prop34BranchId1.ToString() + CConstants.Seperator + Prop35TraderId1.ToString() + CConstants.Seperator + Prop36BrokerId1 + CConstants.Seperator + Prop37OERemarks1 + CConstants.Seperator + Prop38OpenClose1 + CConstants.Seperator + Prop39Settlor1 + CConstants.Seperator +
                        Prop40ProClient1.ToString() + CConstants.Seperator + Prop41SettlementPeriod1.ToString() + CConstants.Seperator + Prop42CoverUncover1 + CConstants.Seperator + Prop43GiveupFlag1 + CConstants.Seperator + PropOrderId.ToString() + CConstants.Seperator +
                        Prop47NnfField.ToString() + CConstants.Seperator + Prop48MarketReplay.ToString() + CConstants.Seperator + Prop49PriceDiff.ToString() + CConstants.Seperator + Prop50Leg2.ToString() + CConstants.Seperator + Prop51Leg3.ToString() + CConstants.Seperator);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }
        #endregion

        #endregion

        #region Tap Header
        /// <summary>
        /// Tap Header Structure : Size(Min =60 and max=1024) Location (0-59,0-1023)
        /// </summary>
        public struct TapHeader : IStruct
        {
            /* Length           2 bytes
             * SequenceNumber   4 Bytes
             * Md5 Checksum     16 bytes
             * Data             
             * Minimum size     22 bytes + 38 Bytes(msg header)=60
             * Maximum Size     1024(22+1002(data))
             * Invitation       22 Bytes + 38 bytes(header) + 2bytes(invitation count)=62 
             */

            //public MessageHeader strHeader;
            //public MessageHeader Prop01Header
            //{
            //    get { return strHeader; }
            //    set { strHeader = value; }

            //}

            /// <summary>
            ///  Length :size(2) Location(0-1)
            /// </summary>
            public Int16 iTapHeaderLength;
            /// <summary>
            /// Length :size(2) Location(0-1)
            /// </summary>
            public Int16 Prop01PacketLength
            {
                get { return iTapHeaderLength; }
                set { iTapHeaderLength = value; }
            }

            /// <summary>
            /// SequenceNumber : Size(4) Location(2-5)
            /// </summary>
            public Int32 iSeqenceNo;
            /// <summary>
            /// SequenceNumber : Size(4) Location(2-5)
            /// </summary>
            public Int32 Prop02SequenceNumber
            {
                get { return iSeqenceNo; }
                set { iSeqenceNo = value; }
            }

            /// <summary>
            /// Md5 Checksum  : Size(16) Location(6-21)
            /// </summary>
            public byte[] bTapMD5CheckSum;
            /// <summary>
            /// Md5 Checksum  : Size(16) Location(6-21)
            /// </summary>
            public byte[] Prop03MD5CheckSum
            {
                get { return bTapMD5CheckSum; }
                set { bTapMD5CheckSum = value; }
            }

            public TapHeader(Int32 seqno)
            {
                iTapHeaderLength = 0;
                iSeqenceNo = seqno;
                bTapMD5CheckSum = new byte[16];
            }

            public byte[] StructToByte()
            {
                byte[] tapheader = new byte[CConstants.TAPHeaderSize];
                try
                {

                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(Prop01PacketLength), 0, tapheader, 0, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(Prop02SequenceNumber), 0, tapheader, 2, 4);
                    Buffer.BlockCopy(Prop03MD5CheckSum, 0, tapheader, 6, 16);
                    return tapheader;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {

                    Prop01PacketLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 0));
                    Prop02SequenceNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 2));
                    Prop03MD5CheckSum = CUtility.GetByteArrayFromStructure(ByteStructure, 6, 16);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01PacketLength.ToString() +
                        Prop02SequenceNumber.ToString() +
                        Prop03MD5CheckSum.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }
        #endregion

        #region Tap Invitation
        /// <summary>
        /// Tap Invitaion : 
        /// </summary>
        public struct TapInvitation : IStruct
        {

            public MessageHeader strHeader;
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            public Int16 iInvitationCount;
            public Int16 Prop02Count
            {
                get { return iInvitationCount; }
                set { iInvitationCount = value; }
            }
            public TapInvitation(Int16 transcode)
            {

                strHeader = new MessageHeader((Int16)CConstants.TranCode.Invitation);
                iInvitationCount = 0;

            }
            public byte[] StructToByte()
            {
                byte[] TapInvitation = new byte[CConstants.TAPInvitationSize];
                try
                {
                    TapInvitation = CUtility.PutBytesInPosition(Prop01Header.StructToByte(), 0, 38, TapInvitation);
                    TapInvitation = CUtility.PutBytesInPosition(CUtility.GetBytesFromNumbers(Prop02Count), 38, 2, TapInvitation);
                    return TapInvitation;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02Count = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() +
                        Prop02Count.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }
        #endregion

        #region UnSolicited Message

        #region Trade Confirm
        /// <summary>
        /// Structure Name: MSTRADECONFIRM
        /// Packet Size: 205 bytes
        /// Transaction Code: TRADECONFIRMATION (2222)
        /// Transaction code : Stop Loss Triggering and Market if Touch(2212)
        /// </summary>
        public struct TradeConfirm : IStruct
        {
            /*
        MESSAGEHEADER                           38  38
        DOUBLE ResponseOrderNumber              8   46
        CHAR BrokerId [5]                       5   51
        CHAR Reserved                           1   52
        SHORT TraderNumber                      2   54
        CHAR AccountNumber [10]                 10  64
        SHORT Buy/SellIndicator                 2   66
        LONG OriginalVolume                     4   70
        LONG DisclosedVolume                    4   74
        LONG RemainingVolume                    4   78
        LONG DisclosedVolume Remaining          4   82
        LONG Price                              4   86
        STRUCT STORDER FLAGS                    2   88
        LONG GoodTillDate                       4   92  
        LONG FillNumber                         4   96  
        LONG FillQuantity                       4   100
        LONG FillPrice                          4   104
        LONG VolumeFilledToday                  4   108
        CHAR ActivityType [2]                   2   110
        LONG ActivityTime                       4   114
        DOUBLE CounterTraderOrderNumber         8   122
        CHAR CounterBrokerId [5]                5   127
        LONG Token                              4   131
        STRUCT CONTRACTDESC                     28  159
        CHAR OpenClose                          1   160
        CHAR OldOpenClose                       1   161
        CHAR BookType                           1   162
        LONG NewVolume                          4   166
        CHAR OldAccountNumber [10]              10  176
        CHAR Participant [12]                   12  188
        CHAR OldParticipant [12]                12  200
        CHAR CoverUncover                       1   201
        CHAR OldCoverUncover                    1   202 
        CHAR GiveUpTrade                        1   203
            */
            /// <summary>
            /// MESSAGEHEADER :size(38) Location(0-37)
            /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// MESSAGEHEADER :size(38) Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }

            /// <summary>
            /// DOUBLE ResponseOrderNumber : size(8) Location(38-45)
            /// </summary>
            public Double iResponseOrderNumber;
            /// <summary>
            /// DOUBLE ResponseOrderNumber : size(8) Location(38-45)
            /// </summary>
            public Double prop02ResponseOrderNumber
            {
                get { return iResponseOrderNumber; }
                set { iResponseOrderNumber = value; }
            }
            /// <summary>
            /// CHAR BrokerId [5]       Size(5) Location(46-50)
            /// </summary>
            public char[] cBrokerId;
            /// <summary>
            /// CHAR BrokerId [5]       Size(5) Location(46-50)
            /// </summary>
            public string prop03BrokerId
            {
                get { return new string(cBrokerId); }
                set { cBrokerId = CUtility.GetPreciseArrayForString(value.ToString(), cBrokerId.Length); }
            }
            /// <summary>
            /// CHAR Reserved           Size(1) Location(51)
            /// </summary>
            public char[] cReserved;
            /// <summary>
            /// CHAR Reserved           Size(1) Location(51)
            /// </summary>
            public string Prop04Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToString(), cReserved.Length); }

            }

            /// <summary>
            /// SHORT TraderNumber      Size(2) Location(52-53)
            /// </summary>
            public Int16 iTraderNumber;
            /// <summary>
            /// SHORT TraderNumber      Size(2) Location(52-53)
            /// </summary>
            public Int16 prop05TraderNumber
            {
                get { return iTraderNumber; }
                set { iTraderNumber = value; }
            }

            /// <summary>
            /// CHAR AccountNumber [10]   Size(10) Location(54-63)
            /// </summary>
            public char[] cAccountNumber;
            /// <summary>
            /// CHAR AccountNumber [10]   Size(10) Location(54-63)
            /// </summary>
            public string Prop06AccountNumber
            {
                get { return new string(cAccountNumber); }
                set { cAccountNumber = CUtility.GetPreciseArrayForString(value.ToString(), cAccountNumber.Length); }

            }

            /// <summary>
            /// SHORT Buy/SellIndicator     size(2) Location(64-65)
            /// </summary>
            public Int16 iBuySellIndicator;
            /// <summary>
            /// SHORT Buy/SellIndicator     size(2) Location(64-65)
            /// </summary>
            public Int16 Prop07BuySellIndicator
            {
                get { return iBuySellIndicator; }
                set { iBuySellIndicator = value; }
            }

            /// <summary>
            /// LONG OriginalVolume : Size(4)   Location(66-69)
            /// </summary>
            public Int32 iOriginalVolume;
            /// <summary>
            /// LONG OriginalVolume : Size(4)   Location(66-69)
            /// </summary>
            public Int32 Prop08OriginalVolume
            {
                get { return iOriginalVolume; }
                set { iOriginalVolume = value; }
            }
            /// <summary>
            /// LONG DisclosedVolume    Size(4) Location(70-73)
            /// </summary>
            public Int32 iDisclosedVolume;
            /// <summary>
            /// LONG DisclosedVolume    Size(4) Location(70-73)
            /// </summary>
            public Int32 Prop09DisclosedVolume
            {
                get { return iDisclosedVolume; }
                set { iDisclosedVolume = value; }
            }

            /// <summary>
            /// LONG RemainingVolume        Size(4) Location(74-77)
            /// </summary>
            public Int32 iRemainingVolume;
            /// <summary>
            /// LONG RemainingVolume        Size(4) Location(74-77)
            /// </summary>
            public Int32 Prop10RemainingVolume
            {
                get { return iRemainingVolume; }
                set { iRemainingVolume = value; }
            }

            /// <summary>
            /// LONG DisclosedVolume Remaining      size(4) Location(78-81)
            /// </summary>
            public Int32 iDisclosedVolumeRemaining;
            /// <summary>
            /// LONG DisclosedVolume Remaining      size(4) Location(78-81)
            /// </summary>
            public Int32 Prop11DisclosedVolumeRemaining
            {
                get { return iDisclosedVolumeRemaining; }
                set { iDisclosedVolumeRemaining = value; }
            }

            /// <summary>
            /// LONG Price          size(4)     Location(82-85)
            /// </summary>
            public Int32 iPrice;
            /// <summary>
            /// LONG Price          size(4)     Location(82-85)
            /// </summary>
            public Int32 Prop12Price
            {
                get { return iPrice; }
                set { iPrice = value; }
            }


            public Int16 iflag;
            public Int16 Prop13OrderFlag
            {
                get { return iflag; }
                set { iflag = value; }
            }

            /// <summary>
            /// LONG GoodTillDate       : Size(4) Location(88-91)
            /// </summary>
            public Int32 iGoodTillDate;
            /// <summary>
            /// LONG GoodTillDate       : Size(4) Location(88-91)
            /// </summary>
            public Int32 Prop14GoodTillDate
            {
                get { return iGoodTillDate; }
                set { iGoodTillDate = value; }
            }

            /// <summary>
            /// LONG FillNumber     Size(4) Location(92-95)
            /// </summary>
            public Int32 iFillNumber;
            /// <summary>
            /// LONG FillNumber     Size(4) Location(92-95)
            /// </summary>
            public Int32 Prop15FillNumber
            {
                get { return iFillNumber; }
                set { iFillNumber = value; }
            }

            /// <summary>
            /// LONG FillQuantity       Size(4) Location(96-99)
            /// </summary>
            public Int32 iFillQuantity;
            /// <summary>
            /// LONG FillQuantity       Size(4) Location(96-99)
            /// </summary>
            public Int32 Prop16FillQuantity
            {
                get { return iFillQuantity; }
                set { iFillQuantity = value; }
            }

            /// <summary>
            /// LONG FillPrice          Size(4) location(100-103)
            /// </summary>
            public Int32 iFillPrice;
            /// <summary>
            /// LONG FillPrice          Size(4) location(100-103)
            /// </summary>
            public Int32 Prop17FillPrice
            {
                get { return iFillPrice; }
                set { iFillPrice = value; }
            }

            /// <summary>
            /// LONG VolumeFilledToday  Size(4) Location(104-107)
            /// </summary>
            public Int32 iVolumeFilledToday;
            /// <summary>
            /// LONG VolumeFilledToday  Size(4) Location(104-107)
            /// </summary>
            public Int32 Prop18VolumeFilledToday
            {
                get { return iVolumeFilledToday; }
                set { iVolumeFilledToday = value; }
            }

            /// <summary>
            /// CHAR ActivityType [2]       Size(2) location(108-109)
            /// </summary>
            public char[] cActivityType;
            /// <summary>
            /// CHAR ActivityType [2]       Size(2) location(108-109)
            /// </summary>
            public string prop19ActivityType
            {
                get { return new string(cActivityType); }
                set { cActivityType = CUtility.GetPreciseArrayForString(value.ToString(), cActivityType.Length); }
            }

            /// <summary>
            /// LONG ActivityTime       Size(4) Location(110-113)
            /// </summary>
            public Int32 iActivityTime;
            /// <summary>
            /// LONG ActivityTime       Size(4) Location(110-113)
            /// </summary>
            public Int32 Prop20ActivityTime
            {
                get { return iActivityTime; }
                set { iActivityTime = value; }
            }

            /// <summary>
            /// DOUBLE CounterTraderOrderNumber :Size(8) location(114-121)
            /// </summary>
            public Double iCounterTraderOrderNumber;
            /// <summary>
            /// DOUBLE CounterTraderOrderNumber :Size(8) location(114-121)
            /// </summary>
            public Double Prop21CounterTraderOrderNumber
            {
                get { return iCounterTraderOrderNumber; }
                set { iCounterTraderOrderNumber = value; }
            }

            /// <summary>
            /// CHAR CounterBrokerId [5]        Size(5) Location(122-126)
            /// </summary>
            public char[] cCounterBrokerId;
            /// <summary>
            /// CHAR CounterBrokerId [5]        Size(5) Location(122-126)
            /// </summary>
            public string Prop22CounterBrokerId
            {
                get { return new string(cCounterBrokerId); }
                set { cCounterBrokerId = CUtility.GetPreciseArrayForString(value.ToString(), cCounterBrokerId.Length); }

            }

            /// <summary>
            /// LONG Token  Size(4)     Location(127-130)
            /// </summary>
            public Int32 iToken;

            /// <summary>
            /// LONG Token  Size(4)     Location(127-130)
            /// </summary>
            public Int32 Prop23Token
            {
                get { return iToken; }
                set { iToken = value; }
            }

            /// <summary>
            /// STRUCT CONTRACTDESC    Size(28) Location(131-158)
            /// </summary>
            public CONTRACTDESC strContractDesc;
            /// <summary>
            /// STRUCT CONTRACTDESC    Size(28) Location(131-158)
            /// </summary>
            public CONTRACTDESC Prop24ContractDesc
            {
                get { return strContractDesc; }
                set { strContractDesc = value; }

            }

            /// <summary>
            /// CHAR OpenClose      size(1) Location(159)
            /// This field contains either ‘O’ for Open or ‘C’ for Close.
            /// </summary>
            public char[] cOpenClose;
            /// <summary>
            /// CHAR OpenClose      size(1) Location(159)
            /// </summary>
            public string Prop25OpenClose
            {
                get { return new string(cOpenClose); }
                set { cOpenClose = CUtility.GetPreciseArrayForString(value.ToString(), cOpenClose.Length); }

            }

            /// <summary>
            /// CHAR OldOpenClose   Size(1) Location(160)
            /// </summary>
            public char[] cOldOpenClose;
            /// <summary>
            /// CHAR OldOpenClose   Size(1) Location(160)
            /// </summary>
            public string Prop26OldOpenClose
            {
                get { return new string(cOldOpenClose); }
                set { cOldOpenClose = CUtility.GetPreciseArrayForString(value.ToString(), cOldOpenClose.Length); }

            }

            /// <summary>
            /// CHAR BookType       Size(1) Location(161)
            /// This field contains the book type—RL/ ST/ SL/NT/ OL/ SP/ AU
            /// 
            /// </summary>
            public char[] cBookType;
            /// <summary>
            /// CHAR BookType       Size(1) Location(161)
            /// </summary>
            public string Prop27BookType
            {
                get { return new string(cBookType); }
                set { cBookType = CUtility.GetPreciseArrayForString(value.ToString(), cBookType.Length); }

            }

            /// <summary>
            /// LONG NewVolume      size(4)     Location(162-165)
            /// </summary>
            public Int32 iNewVolume;
            /// <summary>
            /// LONG NewVolume      size(4)     Location(162-165)
            /// </summary>
            public Int32 Prop28NewVolume
            {
                get { return iNewVolume; }
                set { iNewVolume = value; }
            }

            /// <summary>
            /// CHAR OldAccountNumber [10]      size(10) location(166-175)
            /// </summary>
            public char[] cOldAccountNumber;
            /// <summary>
            /// CHAR OldAccountNumber [10]      size(10) location(166-175)
            /// </summary>
            public string Prop29OldAccountNumber
            {
                get { return new string(cOldAccountNumber); }
                set { cOldAccountNumber = CUtility.GetPreciseArrayForString(value.ToString(), cOldAccountNumber.Length); }

            }

            /// <summary>
            /// CHAR Participant [12]   Size(12) location(176-187)
            /// </summary>
            public char[] cParticipant;
            /// <summary>
            /// CHAR Participant [12]   Size(12) location(176-187)
            /// </summary>
            public string Prop30Participant
            {
                get { return new string(cParticipant); }
                set { cParticipant = CUtility.GetPreciseArrayForString(value.ToString(), cParticipant.Length); }

            }

            /// <summary>
            /// CHAR OldParticipant [12]        size(12)    Location(188-199)
            /// </summary>
            public char[] cOldParticipant;
            /// <summary>
            /// CHAR OldParticipant [12]        size(12)    Location(188-199)
            /// </summary>
            public string Prop31OldParticipant
            {
                get { return new string(cOldParticipant); }
                set { cOldParticipant = CUtility.GetPreciseArrayForString(value.ToString(), cOldParticipant.Length); }

            }
            /// <summary>
            /// CHAR CoverUncover   size(1) Location(200)
            /// </summary>
            public char[] cCoverUncover;
            /// <summary>
            /// CHAR CoverUncover   size(1) Location(200)
            /// </summary>
            public string Prop32CoverUncover
            {
                get { return new string(cCoverUncover); }
                set { cCoverUncover = CUtility.GetPreciseArrayForString(value.ToString(), cCoverUncover.Length); }

            }
            /// <summary>
            /// CHAR OldCoverUncover        size(1)     Location(201)
            /// </summary>
            public char[] cOldCoverUncover;
            /// <summary>
            /// CHAR OldCoverUncover        size(1)     Location(201)
            /// </summary>
            public string Prop33OldCoverUncover
            {
                get { return new string(cOldCoverUncover); }
                set { cOldCoverUncover = CUtility.GetPreciseArrayForString(value.ToString(), cOldCoverUncover.Length); }

            }
            /// <summary>
            /// CHAR GiveUpTrade:       Size(1)     Location(202)
            /// </summary>
            public char[] cGiveUpTrade;
            /// <summary>
            /// CHAR GiveUpTrade:       Size(1)     Location(202)
            /// </summary>
            public string Prop34GiveUpTrade
            {
                get { return new string(cGiveUpTrade); }
                set { cGiveUpTrade = CUtility.GetPreciseArrayForString(value.ToString(), cGiveUpTrade.Length); }

            }

            ///// <summary>
            ///// STRUCT STORDER FLAGS  : size(2) Location(86-87)
            ///// </summary>
            //public OrderFlag strOrderFlag;
            ///// <summary>
            ///// STRUCT STORDER FLAGS  : size(2) Location(86-87)
            ///// </summary>
            //public OrderFlag Prop13OrderFlag
            //{
            //    get { return strOrderFlag; }
            //    set { strOrderFlag = value; }
            //}


            public TradeConfirm(Int16 transcode)
            {

                strHeader = new MessageHeader(transcode);
                iResponseOrderNumber = 0;
                cBrokerId = new char[5];
                cReserved = new char[1];
                iTraderNumber = 0;
                cAccountNumber = new char[10];
                iBuySellIndicator = 1;//1=buy;2=sell
                iOriginalVolume = 0;
                iDisclosedVolume = 0;
                iRemainingVolume = 0;
                iDisclosedVolumeRemaining = 0;
                iPrice = 0;
                //strOrderFlag = new OrderFlag();
                iGoodTillDate = 0;
                iFillNumber = 0;
                iFillQuantity = 0;
                iFillPrice = 0;
                iVolumeFilledToday = 0;
                cActivityType = new char[2];
                iActivityTime = 0;
                iCounterTraderOrderNumber = 0;
                cCounterBrokerId = new char[5];
                iToken = 0;
                strContractDesc = new CONTRACTDESC(01);
                cOpenClose = new char[1];
                cOldOpenClose = new char[1];
                cBookType = new char[1];
                iNewVolume = 0;
                cOldAccountNumber = new char[10];
                cParticipant = new char[12];
                cOldParticipant = new char[12];
                cOldCoverUncover = new char[1];
                cOldParticipant = new char[1];
                cGiveUpTrade = new char[1];
                cCoverUncover = new char[1];
                iflag = 0;

            }
            public byte[] StructToByte()
            {
                byte[] Tradecon = new byte[CConstants.TradeConfirmSize];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, Tradecon, 0, 38);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iResponseOrderNumber), 0, Tradecon, 38, 8);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBrokerId), 0, Tradecon, 46, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserved), 0, Tradecon, 51, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTraderNumber), 0, Tradecon, 52, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cAccountNumber), 0, Tradecon, 54, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBuySellIndicator), 0, Tradecon, 64, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iOriginalVolume), 0, Tradecon, 66, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVolume), 0, Tradecon, 70, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iRemainingVolume), 0, Tradecon, 74, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iDisclosedVolumeRemaining), 0, Tradecon, 78, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iPrice), 0, Tradecon, 82, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iflag), 0, Tradecon, 86, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iGoodTillDate), 0, Tradecon, 88, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iFillNumber), 0, Tradecon, 92, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iFillQuantity), 0, Tradecon, 96, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iFillPrice), 0, Tradecon, 100, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iVolumeFilledToday), 0, Tradecon, 104, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cActivityType), 0, Tradecon, 108, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iActivityTime), 0, Tradecon, 110, 4);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iCounterTraderOrderNumber), 0, Tradecon, 114, 8);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cCounterBrokerId), 0, Tradecon, 122, 5);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iToken), 0, Tradecon, 127, 4);
                    Buffer.BlockCopy(strContractDesc.StructToByte(), 0, Tradecon, 131, 28);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOpenClose), 0, Tradecon, 159, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOldOpenClose), 0, Tradecon, 160, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBookType), 0, Tradecon, 161, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iNewVolume), 0, Tradecon, 162, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOldAccountNumber), 0, Tradecon, 166, 10);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cParticipant), 0, Tradecon, 176, 12);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOldParticipant), 0, Tradecon, 188, 12);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cCoverUncover), 0, Tradecon, 200, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOldCoverUncover), 0, Tradecon, 201, 1);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cGiveUpTrade), 0, Tradecon, 202, 1);
                    return Tradecon;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                strContractDesc = new CONTRACTDESC(11);
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    prop02ResponseOrderNumber = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 38, 8));//IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 38));
                    prop03BrokerId = Encoding.ASCII.GetString(ByteStructure, 46, 5);
                    Prop04Reserved = Encoding.ASCII.GetString(ByteStructure, 51, 1);
                    prop05TraderNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 52));
                    Prop06AccountNumber = Encoding.ASCII.GetString(ByteStructure, 54, 10);
                    Prop07BuySellIndicator = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 64));
                    Prop08OriginalVolume = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 66));
                    Prop09DisclosedVolume = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 70));
                    Prop10RemainingVolume = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 74));
                    Prop11DisclosedVolumeRemaining = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 78));
                    Prop12Price = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 82));
                    Prop13OrderFlag = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 86));
                    Prop14GoodTillDate = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 88));
                    Prop15FillNumber = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 92));
                    Prop16FillQuantity = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 96));
                    Prop17FillPrice = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 100));
                    Prop18VolumeFilledToday = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 104));
                    prop19ActivityType = Encoding.ASCII.GetString(ByteStructure, 108, 2);
                    Prop20ActivityTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 110));
                    Prop21CounterTraderOrderNumber = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 114, 8));//IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ByteStructure, 114));
                    Prop22CounterBrokerId = Encoding.ASCII.GetString(ByteStructure, 122, 5);
                    Prop23Token = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 128));
                    strContractDesc.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 132, 28));
                    Prop24ContractDesc = strContractDesc;
                    Prop25OpenClose = Encoding.ASCII.GetString(ByteStructure, 160, 1);
                    Prop26OldOpenClose = Encoding.ASCII.GetString(ByteStructure, 161, 1);
                    Prop27BookType = Encoding.ASCII.GetString(ByteStructure, 162, 1);
                    Prop28NewVolume = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 163));
                    Prop29OldAccountNumber = Encoding.ASCII.GetString(ByteStructure, 167, 10);
                    Prop30Participant = Encoding.ASCII.GetString(ByteStructure, 177, 12);
                    Prop31OldParticipant = Encoding.ASCII.GetString(ByteStructure, 190, 12);
                    Prop32CoverUncover = Encoding.ASCII.GetString(ByteStructure, 202, 1);
                    Prop33OldCoverUncover = Encoding.ASCII.GetString(ByteStructure, 203, 1);
                    Prop34GiveUpTrade = Encoding.ASCII.GetString(ByteStructure, 204, 1);

                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + CConstants.Seperator +
                        prop02ResponseOrderNumber.ToString() + CConstants.Seperator +
                        prop03BrokerId + Prop04Reserved + CConstants.Seperator +
                        prop05TraderNumber.ToString() + CConstants.Seperator +
                        Prop06AccountNumber + CConstants.Seperator +
                        Prop07BuySellIndicator.ToString() + CConstants.Seperator +
                        Prop08OriginalVolume.ToString() + CConstants.Seperator +
                        Prop09DisclosedVolume.ToString() + CConstants.Seperator +
                        Prop10RemainingVolume.ToString() + CConstants.Seperator +
                        Prop22CounterBrokerId + CConstants.Seperator +
                        Prop23Token.ToString() + CConstants.Seperator +
                        Prop24ContractDesc.ToString() + CConstants.Seperator +
                        Prop25OpenClose + CConstants.Seperator +
                        Prop26OldOpenClose + CConstants.Seperator +
                        Prop27BookType + CConstants.Seperator +
                        Prop28NewVolume.ToString() + CConstants.Seperator +
                        Prop29OldAccountNumber + CConstants.Seperator +
                        Prop30Participant + CConstants.Seperator +
                        Prop31OldParticipant + CConstants.Seperator +
                        Prop32CoverUncover + CConstants.Seperator +
                        Prop33OldCoverUncover + CConstants.Seperator +
                        Prop34GiveUpTrade + CConstants.Seperator);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }


        }
        #endregion

        #region Trader Interactive Message
        /*
         *  MESSAGEHEADER                  38      38
            SHORT TraderId                  2       40
            CHAR Reserved [3]               3       43
            CHAR Reserved                   1       44
            SHORT BroadCastMessage Length   2       46
            CHAR BroadCastMessage [239]     239     285
         * ================================================
         *  Total                           285     285
         * ================================================
         */
        public struct TraderInteractivemsg : IStruct
        {

            public MessageHeader strHeader;
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }

            public Int16 iTraderId;
            public Int16 Prop02TraderId
            {
                get { return iTraderId; }
                set { iTraderId = value; }
            }

            public char[] cReserved;
            public string Prop03Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToString(), cReserved.Length); }

            }

            public char[] cReserved1;
            public string Prop04Reserved1
            {
                get { return new string(cReserved1); }
                set { cReserved1 = CUtility.GetPreciseArrayForString(value.ToString(), cReserved1.Length); }

            }

            public Int16 iBroadCastMessageLength;
            public Int16 Prop05BroadCastMessageLength
            {
                get { return iBroadCastMessageLength; }
                set { iBroadCastMessageLength = value; }
            }

            public char[] cBroadCastMessage;
            public string Prop06BroadCastMessage
            {
                get { return new string(cBroadCastMessage); }
                set { cBroadCastMessage = CUtility.GetPreciseArrayForString(value.ToString(), cBroadCastMessage.Length); }

            }

            public byte[] StructToByte()
            {
                byte[] IntTrader = new byte[CConstants.InteractiveTrader];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, IntTrader, 0, 38);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTraderId), 0, IntTrader, 38, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserved), 0, IntTrader, 40, 3);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cReserved1), 0, IntTrader, 43, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBroadCastMessageLength), 0, IntTrader, 44, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBroadCastMessage), 0, IntTrader, 46, 239);
                    return IntTrader;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02TraderId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                    Prop03Reserved = Encoding.ASCII.GetString(ByteStructure, 40, 3);
                    Prop04Reserved1 = Encoding.ASCII.GetString(ByteStructure, 43, 1);
                    Prop05BroadCastMessageLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 44));
                    Prop06BroadCastMessage = Encoding.ASCII.GetString(ByteStructure, 46, 239);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }
            }
            public TraderInteractivemsg(Int16 transcode)
            {

                strHeader = new MessageHeader(01);
                iTraderId = 0;
                cReserved = new char[3];
                cReserved1 = new char[1];
                iBroadCastMessageLength = 0;
                cBroadCastMessage = new char[239];

            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + Prop02TraderId.ToString() + Prop03Reserved + Prop04Reserved1 + Prop05BroadCastMessageLength.ToString() + Prop06BroadCastMessage);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

        }
        #endregion

        #endregion

        #region Postion and Excercise Liquidation
        public struct PositionExcerInfo : IStruct
        {
            /* 
             *  LONG Token                       4       4
                CHAR InstrumentName [6]         6       10
                CHAR Symbol [10]                10      20
                LONG ExpiryDate                 4       24  
                LONG StrikePrice                4       28
                CHAR OptionType [2]             2       30  
                SHORT CALevel                   2       32
                SHORT ExplFlag                  2       34  
                DOUBLE ExplNumber               8       42
                SHORT MarketType                2       44
                CHAR AccountNumber [10]         10      54
                LONG Quantity                   4       58
                SHORT ProCLi                    2       60
                SHORT ExerciseType              2       62
                LONG EntryDateTime              4       66
                SHORT BranchId                  2       68
                SHORT TraderId                  2       70
                CHAR BrokerId [5]               5       75
                CHAR Remarks [30]               30      105
                CHAR ParticipantId [12]         12      117
         * ====================================================
         *      Total                           117     117
         * ====================================================
         */
            /// <summary>
            ///  LONG Token                      Size=4     Location(0-3)
            /// </summary>
            public Int32 iToken;
            /// <summary>
            ///  LONG Token                      Size=4     Location(0-3)
            /// </summary>
            public Int32 Prop01Token
            {
                get { return iToken; }
                set { iToken = value; }
            }
            /// <summary>
            /// CHAR InstrumentName [6]         Size=6      Location(4-9) 
            /// </summary>
            public Char[] cInstrumentName;
            /// <summary>
            /// CHAR InstrumentName [6]         Size=6      Location(4-9) 
            /// </summary>
            public String Prop02InstrumentName
            {
                get { return new string(cInstrumentName); }
                set { cInstrumentName = CUtility.GetPreciseArrayForString(value.ToString(), cInstrumentName.Length); }
            }
            /// <summary>
            /// CHAR Symbol [10]                Size=10      Location(10-19)
            /// </summary>
            public Char[] cSymbol;
            /// <summary>
            /// CHAR Symbol [10]                Size=10      Location(10-19)
            /// </summary>
            public String Prop03Symbol
            {
                get { return new string(cSymbol); }
                set { cSymbol = CUtility.GetPreciseArrayForString(value.ToString(), cSymbol.Length); }
            }
            /// <summary>
            /// LONG ExpiryDate                 size=4       Location(20-23)
            /// </summary>
            public Int32 iExpiryDate;
            /// <summary>
            /// LONG ExpiryDate                 size=4       Location(20-23)
            /// </summary>
            public Int32 Prop04ExpiryDate
            {
                get { return iExpiryDate; }
                set { iExpiryDate = value; }
            }
            /// <summary>
            /// CHAR OptionType [2]            Size=2       Location(24-27) 
            /// </summary>
            public Int32 iStrikePrice;
            /// <summary>
            /// LONG StrikePrice                size=4       Location(24-27) 
            /// </summary>
            public Int32 Prop05StrikePrice
            {
                get { return iStrikePrice; }
                set { iStrikePrice = value; }
            }

            /// <summary>
            ///   LONG StrikePrice                size=4       Location(24-27) 
            /// • CA/PA for American• CE/PE for European
            /// </summary>
            public Char[] cOptionType;
            /// <summary>
            ///    CHAR OptionType [2]             size=2     Location(28-29)
            /// • CA/PA for American• CE/PE for European
            /// </summary>
            public String Prop06OptionType
            {
                get { return new string(cOptionType); }
                set { cOptionType = CUtility.GetPreciseArrayForString(value.ToString(), cOptionType.Length); }
            }

            /// <summary>
            /// SHORT CALevel                   size=2       Location(30-31)
            /// </summary>
            public Int16 iCALevel;
            /// <summary>
            /// SHORT CALevel                   size=2       Location(30-31)
            /// </summary>
            public Int16 Prop07CALevel
            {
                get { return iCALevel; }
                set { iCALevel = value; }
            }
            /// <summary>
            ///  SHORT ExplFlag                  Size=2       Location(32-33) 
            /// ‘1’ it is an Exercise request. • ‘2’ it is a PL request.
            /// </summary>
            public Int16 iExplFlag;
            /// <summary>
            ///  SHORT ExplFlag                  Size=2       Location(32-33) 
            /// ‘1’ it is an Exercise request. • ‘2’ it is a PL request.
            /// </summary>
            public Int16 Prop08ExplFlag
            {
                get { return iExplFlag; }
                set { iExplFlag = value; }
            }
            /// <summary>
            /// DOUBLE ExplNumber               Size=8       Location(34-41)
            /// </summary>
            public Double iExplNumber;
            /// <summary>
            /// DOUBLE ExplNumber               Size=8       Location(34-41)
            /// </summary>
            public Double Prop09ExplNumber
            {
                get { return iExplNumber; }
                set { iExplNumber = value; }
            }

            /// <summary>
            ///  SHORT MarketType                size=2       location(42-43)
            /// ‘1’ - Normal Market • ‘2’ - Odd Lot Market • ‘3’ - Spot Market • ‘4’ - Auction Market
            /// </summary>
            public Int16 iMarketType;
            /// <summary>
            ///  SHORT MarketType                size=2       location(42-43)
            /// ‘1’ - Normal Market • ‘2’ - Odd Lot Market • ‘3’ - Spot Market • ‘4’ - Auction Market
            /// </summary>
            public Int16 Prop10MarketType
            {
                get { return iMarketType; }
                set { iMarketType = value; }
            }

            /// <summary>
            /// CHAR AccountNumber [10]         Size=10      Location(44-53)
            /// </summary>
            public Char[] cAccountNumber;
            /// <summary>
            /// CHAR AccountNumber [10]         Size=10      Location(44-53)
            /// </summary>
            public String Prop11AccountNumber
            {
                get { return new string(cAccountNumber); }
                set { cAccountNumber = CUtility.GetPreciseArrayForString(value.ToString(), cAccountNumber.Length); }
            }
            /// <summary>
            /// LONG Quantity                   Size=4       Location(54-57)
            /// </summary>
            public Int32 iQuantity;
            /// <summary>
            /// LONG Quantity                   Size=4       Location(54-57)
            /// </summary>
            public Int32 Prop12Quantity
            {
                get { return iQuantity; }
                set { iQuantity = value; }
            }

            /// <summary>
            ///   SHORT ProCLi                    Size=2       Location(58-59)
            /// ‘1’ for Client• ‘2’ for Proprietary
            /// </summary>
            public Int16 iProCLi;
            /// <summary>
            ///   SHORT ProCLi                    Size=2       Location(58-59)
            /// ‘1’ for Client• ‘2’ for Proprietary
            /// </summary>
            public Int16 Prop13ProCLi
            {
                get { return iProCLi; }
                set { iProCLi = value; }
            }
            /// <summary>
            /// SHORT ExerciseType              Size=2       Loaction(60-61)
            /// </summary>
            public Int16 iExerciseType;
            /// <summary>
            /// SHORT ExerciseType              Size=2       Loaction(60-61)
            /// ‘1’ for Do Exercise • ‘0’ for Don’t Exercise
            /// </summary>
            public Int16 Prop14ExerciseType
            {
                get { return iExerciseType; }
                set { iExerciseType = value; }
            }
            /// <summary>
            /// LONG EntryDateTime              Size=4       Location(62-65)
            /// </summary>
            public Int32 iEntryDateTime;
            /// <summary>
            /// LONG EntryDateTime              Size=4       Location(62-65)
            /// </summary>
            public Int32 Prop15EntryDateTime
            {
                get { return iEntryDateTime; }
                set { iEntryDateTime = value; }
            }

            /// <summary>
            ///  SHORT BranchId                 Size=2       Location(66-67)
            /// </summary>
            public Int16 iBranchId;
            /// <summary>
            ///  SHORT BranchId                 Size=2       Location(66-67)
            /// </summary>
            public Int16 Prop16BranchId
            {
                get { return iBranchId; }
                set { iBranchId = value; }
            }
            /// <summary>
            ///  SHORT TraderId                  Size=2       Location(68-69)
            /// </summary>
            public Int16 iTraderId;
            /// <summary>
            ///  SHORT TraderId                  Size=2       Location(68-69)
            /// </summary>
            public Int16 Prop17TraderId
            {
                get { return iTraderId; }
                set { iTraderId = value; }
            }
            /// <summary>
            ///  CHAR BrokerId [5]               Size=5       Location(70-74)
            /// </summary>
            public Char[] cBrokerId;
            /// <summary>
            ///  CHAR BrokerId [5]               Size=5       Location(70-74)
            /// </summary>
            public String Prop18BrokerId
            {
                get { return new string(cBrokerId); }
                set { cBrokerId = CUtility.GetPreciseArrayForString(value.ToString(), cBrokerId.Length); }
            }
            /// <summary>
            /// CHAR Remarks [30]               Size=30      Location(75-104)
            /// </summary>
            public Char[] cRemarks;
            /// <summary>
            /// CHAR Remarks [30]               Size=30      Location(75-104)
            /// </summary>
            public String Prop19Remarks
            {
                get { return new string(cRemarks); }
                set { cRemarks = CUtility.GetPreciseArrayForString(value.ToString(), cRemarks.Length); }
            }
            /// <summary>
            ///  CHAR ParticipantId [12]         Size=12      Location(105-116)
            /// </summary>
            public Char[] cParticipantId;
            /// <summary>
            ///  CHAR ParticipantId [12]         Size=12      Location(105-116)
            /// </summary>
            public String Prop20ParticipantId
            {
                get { return new string(cParticipantId); }
                set { cParticipantId = CUtility.GetPreciseArrayForString(value.ToString(), cParticipantId.Length); }
            }

            public byte[] StructToByte()
            {

                byte[] Info = new byte[CConstants.ExcercisePositioninfo];
                try
                {
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iToken), 0, Info, 0, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cInstrumentName), 0, Info, 4, 6);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cSymbol), 0, Info, 10, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iExpiryDate), 0, Info, 20, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iStrikePrice), 0, Info, 24, 4);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cOptionType), 0, Info, 28, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iCALevel), 0, Info, 30, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iExplFlag), 0, Info, 32, 2);
                    Buffer.BlockCopy(CUtility.HostToNetworkDouble(iExplNumber), 0, Info, 34, 8);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iMarketType), 0, Info, 42, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cAccountNumber), 0, Info, 44, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iQuantity), 0, Info, 54, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iProCLi), 0, Info, 58, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iExerciseType), 0, Info, 60, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iEntryDateTime), 0, Info, 62, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iBranchId), 0, Info, 66, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iTraderId), 0, Info, 68, 2);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cBrokerId), 0, Info, 70, 5);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cRemarks), 0, Info, 75, 30);
                    Buffer.BlockCopy(CUtility.ConvertCharFrom16to8bit(cParticipantId), 0, Info, 105, 12);
                    return Info;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Token = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 0));
                    Prop02InstrumentName = Encoding.ASCII.GetString(ByteStructure, 4, 6);
                    Prop03Symbol = Encoding.ASCII.GetString(ByteStructure, 10, 10);
                    Prop04ExpiryDate = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 20));
                    Prop05StrikePrice = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 24));
                    Prop06OptionType = Encoding.ASCII.GetString(ByteStructure, 28, 2);
                    Prop07CALevel = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 30));
                    Prop08ExplFlag = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 32));
                    Prop09ExplNumber = CUtility.NetworkToHostDouble(CUtility.GetByteArrayFromStructure(ByteStructure, 34, 8));
                    Prop10MarketType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 42));
                    Prop11AccountNumber = Encoding.ASCII.GetString(ByteStructure, 44, 10);
                    Prop12Quantity = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 54));
                    Prop13ProCLi = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 58));
                    Prop14ExerciseType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 60));
                    Prop15EntryDateTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 62));
                    Prop16BranchId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 66));
                    Prop17TraderId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 68));
                    Prop18BrokerId = Encoding.ASCII.GetString(ByteStructure, 70, 5);
                    Prop19Remarks = Encoding.ASCII.GetString(ByteStructure, 75, 20);
                    Prop20ParticipantId = Encoding.ASCII.GetString(ByteStructure, 105, 12);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }


            }

            public PositionExcerInfo(String sOptionType)
            {

                iToken = 0;
                cInstrumentName = new char[6];
                cSymbol = new char[10];
                iExpiryDate = 0;
                iStrikePrice = 0;
                cOptionType = new char[2];
                iCALevel = 0;
                iExplFlag = 0;
                iExplNumber = 0;
                iMarketType = 0;
                cAccountNumber = new char[10];
                iQuantity = 0;
                iProCLi = 0;
                iExerciseType = 0;
                iEntryDateTime = 0;
                iBranchId = 0;
                iTraderId = 0;
                cBrokerId = new char[5];
                cRemarks = new char[30];
                cParticipantId = new char[12];
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Token.ToString() + CConstants.Seperator +
                         Prop02InstrumentName + CConstants.Seperator +
                         Prop03Symbol + CConstants.Seperator +
                         Prop04ExpiryDate.ToString() + CConstants.Seperator +
                         Prop05StrikePrice.ToString() + CConstants.Seperator +
                         Prop06OptionType + CConstants.Seperator +
                         Prop07CALevel.ToString() + CConstants.Seperator +
                         Prop08ExplFlag.ToString() + CConstants.Seperator +
                         Prop09ExplNumber.ToString() + CConstants.Seperator +
                         Prop10MarketType.ToString() + CConstants.Seperator +
                         Prop11AccountNumber + CConstants.Seperator +
                         Prop12Quantity.ToString() + CConstants.Seperator +
                         Prop13ProCLi.ToString() + CConstants.Seperator +
                        Prop14ExerciseType.ToString() + CConstants.Seperator +
                        Prop15EntryDateTime.ToString() + CConstants.Seperator +
                        Prop16BranchId.ToString() + CConstants.Seperator +
                        Prop17TraderId.ToString() + CConstants.Seperator +
                        Prop18BrokerId + CConstants.Seperator +
                        Prop19Remarks + CConstants.Seperator +
                        Prop20ParticipantId + CConstants.Seperator);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }

        }

        /// <summary>
        /// LiquidationEntryReq Size(157) Location(0-156)
        /// EX_PL_ENTRY_OUT:4001,EX_PL_CONFIRMATION (4002).,EX_PL_MOD_IN (4005).,EX_PL_MOD_CONFIRMATION (4007).,EX_PL_CXL_IN(4008).
        /// EX_PL_CXL_OUT (4009).EX_PL_CXL_CONFIRMATION (4010).
        /// </summary>
        public struct LiquidationEntryReq : IStruct
        {
            /*
             * STRUCT MESSAGE_HEADER        38      38
             * SHORT ReasonCode             2       40
             * STRUCT EX_PL_INFO            117     157
             * ==============================================
             * Toatl                        157     157
             * ==============================================
             */
            /// <summary>
            /// STRUCT MESSAGE_HEADER        size=38      Location(0-37)
            /// /// </summary>
            public MessageHeader strHeader;
            /// <summary>
            /// STRUCT MESSAGE_HEADER        size=38      Location(0-37)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            /// <summary>
            /// SHORT ReasonCode             size=2       Location(38-39)
            /// </summary>
            public Int16 iReasonCode;
            /// <summary>
            /// SHORT ReasonCode             size=2       Location(38-39)
            /// </summary>
            public Int16 Prop02ReasonCode
            {
                get { return iReasonCode; }
                set { iReasonCode = value; }
            }

            /// <summary>
            /// STRUCT EX_PL_INFO            Size=117     Location(40-156)
            /// </summary>
            public PositionExcerInfo strInfo;
            /// <summary>
            /// STRUCT EX_PL_INFO            Size=117     Location(40-156)
            /// </summary>
            public PositionExcerInfo Prop03strInfo
            {
                get { return strInfo; }
                set { strInfo = value; }
            }

            public byte[] StructToByte()
            {
                byte[] PosExcerReq = new byte[CConstants.ExcercisePositionRequest];
                try
                {
                    Buffer.BlockCopy(strHeader.StructToByte(), 0, PosExcerReq, 0, 38);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(iReasonCode), 0, PosExcerReq, 38, 2);
                    Buffer.BlockCopy(strInfo.StructToByte(), 0, PosExcerReq, 40, 117);
                    return PosExcerReq;
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02ReasonCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 40));
                    Prop03strInfo.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 40, 117));
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }
            public LiquidationEntryReq(Int16 transcode, String OptionType)
            {

                strHeader = new MessageHeader((Int16)CConstants.TranCode.ExerPositionRequest);
                iReasonCode = 0;
                strInfo = new PositionExcerInfo(OptionType);
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + CConstants.Seperator +
                        Prop02ReasonCode.ToString() + CConstants.Seperator + Prop03strInfo.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }
        #endregion

        #region Change of Security Status       Transaction Code:7320,7210      size:472
        #region Token Eligibility
        public struct TokenAndEligibility : IStruct
        {
            /*
             *  Long Token                         4       4                
                SECURITY STATUS PER MARKET [4]      4*2     8
             * ==============================================
             *  Total                               12      12
             * ==============================================
             */


            public struct securitystatusperMkt
            {
                /*    ‘1’ - Pre-open
                    • ‘2’ - Open
                    • ‘3’ - Suspended
                    • ‘4’ - Pre-open extended
                */
                public Int16 iStatus;
                public Int16 Prop01Status
                {
                    get { return iStatus; }
                    set { iStatus = value; }
                }
                public byte[] StructToByte()
                {
                    return null;
                }
                public void ByteToStruct(byte[] ByteStructure)
                {
                    Prop01Status = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 0));
                }
                public securitystatusperMkt(int code)
                {
                    iStatus = 0;
                }

            }
            public Int32 iToken;
            public Int32 Prop01Token
            {
                get { return iToken; }
                set { iToken = value; }
            }
            public Dictionary<Int32, securitystatusperMkt> dSecurityStatusPerMkt;
            public Dictionary<Int32, securitystatusperMkt> Prop02SecurityStatusPerMkt
            {
                get { return dSecurityStatusPerMkt; }
                set { dSecurityStatusPerMkt = value; }
            }


            public byte[] StructToByte()
            {
                return null;
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01Token = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 0));
                for (int idx = 0; idx < 4; idx++)
                {
                    securitystatusperMkt status = new securitystatusperMkt(0);
                    status.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 2 + (idx * 2), 2));
                    if (Prop02SecurityStatusPerMkt.ContainsKey(idx) == true)
                        Prop02SecurityStatusPerMkt[idx] = status;
                    else
                        Prop02SecurityStatusPerMkt.Add(idx, status);

                }
                //Prop02Status.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 2, 4));
            }

            public TokenAndEligibility(int code)
            {
                iToken = 0;
                dSecurityStatusPerMkt = new Dictionary<int, securitystatusperMkt>();
            }

            public override string ToString()
            {
                return (Prop01Token.ToString() + Prop02SecurityStatusPerMkt.ToString());
            }
        }
        #endregion
        /*
         * STRUCT MESSAGE_HEADER            38              0-37
         * SHORT NumberOfRecords            2               38-39
         * STRUCT TOKEN AND ELIGIBILITY     35*No of Records    
         * ===================================================
         * Total                            460(40+ 35*12=460) 
         * ===================================================
         */
        public struct SecurityStatus : IStruct
        {
            public MessageHeader strHeader;
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            public Int16 iNoOfRecords;
            public Int16 Prop02NoOfRecords
            {
                get { return iNoOfRecords; }
                set { iNoOfRecords = value; }
            }
            public Dictionary<Int32, TokenAndEligibility> dTokenEligibility;
            public Dictionary<Int32, TokenAndEligibility> Prop03TokenEligibility
            {
                get { return dTokenEligibility; }
                set { dTokenEligibility = value; }
            }

            public byte[] StructToByte()
            {
                return null;
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02NoOfRecords = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                    for (int idx = 0; idx < Prop02NoOfRecords; idx++)
                    {
                        TokenAndEligibility Tokeneligibility = new TokenAndEligibility(0);
                        Tokeneligibility.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 40 + (idx * 12), 12));
                        if (Prop03TokenEligibility.ContainsKey(Tokeneligibility.Prop01Token) == true)
                            Prop03TokenEligibility[Tokeneligibility.Prop01Token] = Tokeneligibility;
                        else
                            Prop03TokenEligibility.Add(Tokeneligibility.Prop01Token, Tokeneligibility);
                    }
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                }

            }

            public SecurityStatus(int Transcode)
            {
                strHeader = new MessageHeader(0);
                iNoOfRecords = 0;
                dTokenEligibility = new Dictionary<int, TokenAndEligibility>();
            }

            public override string ToString()
            {
                try
                {
                    return (Prop01Header.ToString() + Prop02NoOfRecords.ToString() + Prop03TokenEligibility.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }

            }
        }
        #endregion

        #region Instrument Master Change       Transaction Code:7324       size:76
        /// <summary>
        /// ParticipantUpdateInfo size(81) Location(0-80)
        /// </summary>
        public struct InstrumentUpdateInfo
        {
            /*
             *  MESSAGE HEADER                     38   38
             *  InstrumentId                        2   40
                CHAR InstrumentName [6]             6   46
                CHAR InstrumentDescription         25   71
                LONG InstrumentUpdateDateTime       4   75
                CHAR DeleteFlag                     1   76
             * ==============================================
             * Total                            :      76
             * ==============================================
             * 
             */
            /// <summary>
            /// MESSAGE HEADER                     38   0-37
            /// </summary>
            private MessageHeader strHeader;
            /// <summary>
            /// MESSAGE HEADER                      38   0-37
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }
            /// <summary>
            /// InstrumentId                        2   38-39
            /// </summary>
            private Int16 iInstrumentId;
            /// <summary>
            /// InstrumentId                        2   38-39
            /// </summary>
            public Int16 Prop02InstrumentId
            {
                get { return iInstrumentId; }
                set { iInstrumentId = value; }
            }

            /// <summary>
            ///  CHAR InstrumentName [6]          6     40-45
            /// </summary>
            private char[] cInstrumentName;
            /// <summary>
            ///  CHAR InstrumentName [6]          6     40-45
            /// </summary>
            public string Prop03PInstrumentName
            {
                get { return new string(cInstrumentName); }
                set { cInstrumentName = CUtility.GetPreciseArrayForString(value.ToUpper(), cInstrumentName.Length); }
            }

            /// <summary>
            ///  CHAR InstrumentDescription         25   46-51
            /// </summary>
            private char[] cInstrumentDescription;
            /// <summary>
            ///  CHAR InstrumentDescription         25   46-51
            /// </summary>
            public string Prop04InstrumentDescription
            {
                get { return new string(cInstrumentDescription); }
                set { cInstrumentDescription = CUtility.GetPreciseArrayForString(value.ToUpper(), cInstrumentDescription.Length); }
            }

            /// <summary>
            ///  LONG InstrumentUpdateDateTime       4   75
            /// </summary>
            private Int32 iInstrumentUpdateDateTime;
            /// <summary>
            ///  LONG InstrumentUpdateDateTime       4   75
            /// </summary>
            public Int32 Prop05InstrumentUpdateDateTime
            {
                get { return iInstrumentUpdateDateTime; }
                set { iInstrumentUpdateDateTime = value; }
            }

            /// <summary>
            ///  CHAR DeleteFlag                     1   76
            /// • ‘Y’ means deleted • ‘N’ means not deleted
            /// </summary>
            private char[] cDeleteFlag;
            /// <summary>
            /// CHAR DeleteFlag                     1   76
            /// </summary>
            public string Prop06DeleteFlag
            {
                get { return new string(cDeleteFlag); }
                set { cDeleteFlag = CUtility.GetPreciseArrayForString(value.ToUpper(), cDeleteFlag.Length); }
            }

            public byte[] StructToByte()
            {
                return null;
            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                Prop02InstrumentId = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                Prop03PInstrumentName = Encoding.ASCII.GetString(ByteStructure, 40, 6);
                Prop04InstrumentDescription = Encoding.ASCII.GetString(ByteStructure, 46, 25);
                Prop05InstrumentUpdateDateTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 71));
                Prop06DeleteFlag = Encoding.ASCII.GetString(ByteStructure, 76, 1);

            }
            public override string ToString()
            {
                return (Prop01Header.ToString() + Prop02InstrumentId.ToString() + Prop03PInstrumentName + Prop04InstrumentDescription + Convert.ToInt32(Prop05InstrumentUpdateDateTime).ToString() + Prop06DeleteFlag);
            }
            public InstrumentUpdateInfo(Int16 trancode)
            {
                strHeader = new MessageHeader(0);
                iInstrumentId = 0;
                cInstrumentName = new char[6];
                cInstrumentDescription = new char[25];
                iInstrumentUpdateDateTime = 0;
                cDeleteFlag = new char[1];
            }


        }
        #endregion

        #region Index Map Table         Transaction Code:7326      size:460

        public struct IndexMapDetails : IStruct
        {

            #region Properties

            /// <summary>
            /// 
            /// </summary>
            private char[] cBcastName;
            /// <summary>
            ///  CHAR InstrumentName [26]          26     0-25
            /// </summary>
            public string Prop01BcastName
            {
                get { return new string(cBcastName); }
                set { cBcastName = CUtility.GetPreciseArrayForString(value.ToUpper(), cBcastName.Length); }
            }

            /// <summary>
            /// 
            /// </summary>
            private char[] cChangedName;
            /// <summary>
            ///  CHAR InstrumentName [26]          10     26-35
            /// </summary>
            public string Prop02ChangedName
            {
                get { return new string(cChangedName); }
                set { cChangedName = CUtility.GetPreciseArrayForString(value.ToUpper(), cChangedName.Length); }
            }

            /// <summary>
            /// 
            /// </summary>
            private char[] cDeleteFlag;
            /// <summary>
            ///  CHAR InstrumentName [26]         1   36
            /// </summary>
            public string Prop03DeleteFlag
            {
                get { return new string(cDeleteFlag); }
                set { cDeleteFlag = CUtility.GetPreciseArrayForString(value.ToUpper(), cDeleteFlag.Length); }
            }

            /// <summary>
            /// 
            /// </summary>
            private Int32 iLastUpdateDateTime;
            /// <summary>
            /// Int32 iLastUpdateDateTime       4   37-40
            /// </summary>
            public Int32 Prop04LastUpdateDateTime
            {
                get { return iLastUpdateDateTime; }
                set { iLastUpdateDateTime = value; }
            }
            #endregion

            public IndexMapDetails(int Transcode)
            {

                cBcastName = new char[26];
                cChangedName = new char[10];
                cDeleteFlag = new char[1];
                iLastUpdateDateTime = 0;
            }

            public override string ToString()
            {
                return (Prop01BcastName + CConstants.Seperator +
                    Prop02ChangedName + CConstants.Seperator +
                    Prop03DeleteFlag + CConstants.Seperator +
                    Prop04LastUpdateDateTime.ToString() + CConstants.Seperator);
            }

            #region IStruct Members

            public byte[] StructToByte()
            {
                return null;
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01BcastName = Encoding.ASCII.GetString(ByteStructure, 0, 26);
                Prop02ChangedName = Encoding.ASCII.GetString(ByteStructure, 26, 10);
                Prop03DeleteFlag = Encoding.ASCII.GetString(ByteStructure, 36, 1);
                Prop04LastUpdateDateTime = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ByteStructure, 37));
            }

            #endregion
        }

        public struct IndexMapTable : IStruct
        {

            #region Properties
            public MessageHeader strHeader;
            public MessageHeader Prop01Header
            {
                get { return strHeader; }
                set { strHeader = value; }
            }

            public Int16 iNoOfRecords;
            public Int16 Prop02NoOfRecords
            {
                get { return iNoOfRecords; }
                set { iNoOfRecords = value; }
            }

            private char[] cIndexMap;
            public String Prop03IndexMap
            {
                get { return new string(cIndexMap); }
                set { cIndexMap = CUtility.GetPreciseArrayForString(value, cIndexMap.Length); }
            }


            #endregion

            #region IStruct Members

            public byte[] StructToByte()
            {
                return null;
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                Prop02NoOfRecords = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(ByteStructure, 38));
                StringBuilder sr = new StringBuilder();
                for (int idx = 0; idx < Prop02NoOfRecords; idx++)
                {
                    IndexMapDetails indexdet = new IndexMapDetails(7326);
                    indexdet.ByteToStruct(CUtility.GetByteArrayFromStructure(ByteStructure, 40 + (idx * 41), 41));
                    sr.Append(indexdet.ToString());
                }
                cIndexMap = new char[sr.Length];
                Prop03IndexMap = sr.ToString();
            }

            #endregion

            public override string ToString()
            {
                return (Prop01Header.ToString() + CConstants.Seperator +
                    Prop02NoOfRecords.ToString() + CConstants.Seperator +
                    Prop03IndexMap + CConstants.Seperator);

            }

            public IndexMapTable(int transcode)
            {
                strHeader = new MessageHeader(0);
                iNoOfRecords = 0;
                cIndexMap = new char[410];
            }
        }

        #endregion
    }
}
