using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows;

namespace TradeTigerAPI
{
    public class CStructures
    {
        public struct MessageHeader : IStruct
        {
            /* 
               
                LONG MessageLength         4
             *  SHORT TransactionCode      2  6
             *  
             * TOTAL :                     6
             */


            /// <summary>
            /// SHORT MessageLength  :Size 4
            /// This field is set to the length of the entire message
            /// /// </summary>
            public Int32 cMessageLength;
            public Int32 Prop01MessageLength
            {
                get { return cMessageLength; }
                set { cMessageLength = value; }

            }

            /// <summary> 
            /// SHORT TransactionCode  :Size 2
            /// /// </summary>
            public Int16 cTransactionCode;
            public Int16 Prop02TransactionCode
            {
                get { return cTransactionCode; }
                set { cTransactionCode = value; }
            }

            #region Constructor
            /// <summary>
            /// Login Header
            /// </summary>
            /// <param name="trans">cconstant.transcode</param>
            /// <param name="MsgLen">Total Messsage Length</param>
            ///            
            public MessageHeader(Int16 tranCode)
            {
                cTransactionCode = 0;
                cMessageLength = 0;
            }
            # endregion

            public byte[] StructToByte()
            {
                byte[] HeadStruct = new byte[CConstants.MsgHeaderSize];
                try
                {
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(cMessageLength), 0, HeadStruct, 0, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(cTransactionCode), 0, HeadStruct, 4, 2);

                    //Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(cMessageLength)), 0, HeadStruct, 0, 4);
                    //Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(cTransactionCode)), 0, HeadStruct, 4, 2);

                    return HeadStruct;
                }
                catch (Exception ex)
                {
                    // FnoCtclLib.Reference.LogException(ex, "Message Header Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return HeadStruct;

                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01MessageLength = BitConverter.ToInt32(ByteStructure, 0);
                    Prop02TransactionCode = BitConverter.ToInt16(ByteStructure, 4);
                }
                catch (Exception ex)
                {
                    // FnoCtclLib.Reference.LogException(ex, "Message Header Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            public override string ToString()
            {
                try
                {
                    return ("MessageLength = " + Prop01MessageLength.ToString() + "|" +
                       "TransactionCode = " + Prop02TransactionCode.ToString());
                }
                catch (Exception ex)
                {

                    //  FnoCtclLib.Reference.LogException(ex, ex.Source.ToString() + ex.Message + "Trace : " + ex.StackTrace);
                    return null;
                }
            }
        }

        #region Login Request
        public struct SignOn : IStruct
        {
            /*
            *  Message header                            6       6
            *  CHAR LoginId[30]                         30      36
               CHAR MemberPassword [20]                 20      56
               CHAR TradingPassword [20]                20      76  
             *  CHAR IP [20]                            20      96
               CHAR Reserved [100]                      100     196
                                             
            * ================================================
            * Total                                    196
            * ================================================
            */


            /// <summary>
            /// MessageHeader : Size 6 (Location:0-5)
            /// </summary>
            public MessageHeader cHeader;
            /// <summary>
            /// MessageHeader : Size 6 (Location:0-5)
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }

            /// <summary>
            /// CHAR LoginId : Size(30) (Location:6-35)
            /// </summary>
            public char[] cLoginId;
            /// <summary>
            /// CHAR LoginId : Size(30) (Location:6-35)
            /// </summary>
            public string Prop02LoginId
            {
                get { return new string(cLoginId); }
                set { cLoginId = CUtility.GetPreciseArrayForString(value, cLoginId.Length); }
            }

            /// <summary>
            /// CHAR MemberPassword [20] :Size(20) Location:36-55
            /// </summary>
            public Char[] cMemberPassword;
            /// <summary>
            /// CHAR MemberPassword [20] :Size(20) Location:36-55
            /// </summary>
            public string Prop03MemberPassword
            {
                get { return new string(cMemberPassword); }
                set { cMemberPassword = CUtility.GetPreciseArrayForString(value, cMemberPassword.Length); }
            }


            /// <summary>
            /// CHAR TradingPassword [20] : Size(20) Location:56-75
            /// </summary>
            public Char[] cTradingPassword;
            /// <summary>
            /// CHAR TradingPassword [20] : Size(20) Location:56-75
            /// </summary>
            public string Prop04TradingPassword
            {
                get { return new string(cTradingPassword); }
                set { cTradingPassword = CUtility.GetPreciseArrayForString(value, cTradingPassword.Length); }
            }

            /// <summary>
            /// CHAR IP [20] : Size(20) Location:76-95
            /// </summary>
            public Char[] cIP;
            /// <summary>
            /// CHAR IP [20] : Size(20) Location:76-95
            /// </summary>
            public string Prop05IP
            {
                get { return new string(cIP); }
                set { cIP = CUtility.GetPreciseArrayForString(value, cIP.Length); }

            }

            /// <summary>
            /// CHAR Reserved [100] : Size(100) Location:96-195
            /// </summary>
            public Char[] cReserved;
            /// <summary>
            /// CHAR Reserved [100] : Size(100) Location:96-195
            /// </summary>
            public string Prop06Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }

            }

            public SignOn(Int16 tranCode)
            {
                cHeader = new MessageHeader(tranCode);
                cLoginId = new char[30];
                cMemberPassword = new char[20];
                cTradingPassword = new char[20];
                cIP = new char[20];
                cReserved = new char[100];
            }

            #region Implemented Methods

            public byte[] StructToByte()
            {
                byte[] LoginRequest = new byte[CConstants.LoginRequestSize];
                try
                {
                    Buffer.BlockCopy(cHeader.StructToByte(), 0, LoginRequest, 0, CConstants.MsgHeaderSize);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cLoginId), 0, LoginRequest, CConstants.MsgHeaderSize, cLoginId.Length);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cMemberPassword), 0, LoginRequest, CConstants.MsgHeaderSize + cLoginId.Length, cMemberPassword.Length);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cTradingPassword), 0, LoginRequest, CConstants.MsgHeaderSize + cLoginId.Length + cMemberPassword.Length, cTradingPassword.Length);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cIP), 0, LoginRequest, CConstants.MsgHeaderSize + cLoginId.Length + cMemberPassword.Length + cTradingPassword.Length, cIP.Length);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, LoginRequest, CConstants.MsgHeaderSize + cLoginId.Length + cMemberPassword.Length + cTradingPassword.Length + cIP.Length, cReserved.Length);
                    return LoginRequest;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return LoginRequest;
                }

            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    Prop01Header.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));
                    Prop02LoginId = Encoding.ASCII.GetString(ByteStructure, 6, 30);
                    Prop03MemberPassword = Encoding.ASCII.GetString(ByteStructure, 36, 20);
                    Prop04TradingPassword = Encoding.ASCII.GetString(ByteStructure, 56, 20);
                    Prop05IP = Encoding.ASCII.GetString(ByteStructure, 76, 20);
                    Prop06Reserved = Encoding.ASCII.GetString(ByteStructure, 96, 100);
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SignOn Struture's ByteToStruct.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                }
            }

            public override string ToString()
            {
                return (Prop01Header.ToString() + "|" + "LoginId = " + Prop02LoginId + "|" + "MemberPassword = " + Prop03MemberPassword + "|" + "TradingPassword = " + Prop04TradingPassword + "|" + "IP = " + Prop05IP);
            }

            #endregion
        }
        #endregion

        #region Login Response
        public struct LoginResponse : IStruct
        {
            /*
                       *  Message header                             6      6
                       *  Short StatusCode[2]                        2      8
                          CHAR Message [250]                       250    258
                          CHAR Client Info List[N * 75]             75    333                         
                          CHAR Reserved [100]                      100    433
                                             
                       * ================================================
                       * Total                                     433
                       * ================================================
                       */

            /// <summary>
            /// Login Header 6 bytes
            /// </summary>
            public MessageHeader cHeader;

            /// <summary>
            /// Login Header 6 bytes
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }

            /// <summary>
            /// 0 – Success, 1- Failure
            /// </summary>
            public Int16 cStatusCode;

            /// <summary>
            /// 0 – Success, 1- Failure
            /// </summary>
            public Int16 Prop02StatusCode
            {
                get { return cStatusCode; }
                set { cStatusCode = value; }
            }

            /// <summary>
            /// Actual Message of the Authentication Process
            /// </summary>
            public char[] cMessage;
            /// <summary>
            /// Actual Message of the Authentication Process
            /// </summary>
            public string Prop03Message
            {
                get { return new string(cMessage); }
                set { cMessage = CUtility.GetPreciseArrayForString(value, cMessage.Length); }
            }

            /// <summary>
            /// Client Name, Customer ID and S2KID separated by Comma delimiter. 
            /// Single Client Info Length is 75. N represents client count
            /// </summary>
            public char[] cClientInfoList;
            /// <summary>
            /// Client Name, Customer ID and S2KID separated by Comma delimiter. 
            /// Single Client Info Length is 75. N represents client count
            /// </summary>
            public string Prop04ClientInfoList
            {
                get { return new string(cClientInfoList); }
                set { cClientInfoList = CUtility.GetPreciseArrayForString(value.ToUpper(), cClientInfoList.Length); }
            }

            /// <summary>
            /// Default blank
            /// </summary>
            public char[] cReserved;
            /// <summary>
            /// Default blank
            /// </summary>
            public string Prop05Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToUpper(), cReserved.Length); }
            }

            public LoginResponse(Int16 tranCode)
            {
                cHeader = new MessageHeader(tranCode);
                cStatusCode = 1;
                cMessage = new char[247];
                cClientInfoList = new char[75];
                cReserved = new char[100];
            }

            #region Interface Methods

            public byte[] StructToByte()
            {
                byte[] LoginResponse = new byte[CConstants.LoginResponseSize];
                try
                {
                    Buffer.BlockCopy(cHeader.StructToByte(), 0, LoginResponse, 0, 6);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(cStatusCode), 0, LoginResponse, 6, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cMessage), 0, LoginResponse, 8, 250);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cClientInfoList), 0, LoginResponse, 258, 75);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, LoginResponse, 333, 100);
                    return LoginResponse;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return LoginResponse;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    cHeader = new MessageHeader(1);
                    cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                    Prop01Header = cHeader;
                    Prop02StatusCode = BitConverter.ToInt16(ByteStructure, 6);
                    Prop03Message = Encoding.ASCII.GetString(ByteStructure, 8, 247);
                    Prop04ClientInfoList = Encoding.ASCII.GetString(ByteStructure, 258, 75);
                    // Prop05Reserved = Encoding.ASCII.GetString(ByteStructure, 333, 100);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            #endregion

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "StatusCode = " + Prop02StatusCode + "|" + "Message = " + Prop03Message + "|" + "ClientInfoList = " + Prop04ClientInfoList + "|" + "Reserved = " + Prop05Reserved;
            }
        }
        #endregion

        #region Logoff Request

        public struct LogOffRequest : IStruct
        {
            /*
                      *  Message header                           6      6
                      *  Char LoginId[30]                        30     36                                          
                         CHAR Reserved [100]                    100    136
                                             
                      * ================================================
                      * Total                                   136
                      * ================================================
                      */
            /// <summary>
            /// Login Header Size 6
            /// </summary>
            public MessageHeader header;
            /// <summary>
            /// Login Header Size 6
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return header; }
                set { header = value; }
            }
            /// <summary>
            /// This field should contain User ID of the user/broker 
            /// </summary>
            public Char[] cLoginId;
            /// <summary>
            /// This field should contain User ID of the user/broker 
            /// </summary>
            public string Prop02LoginId
            {
                get { return new string(cLoginId); }
                set { cLoginId = CUtility.GetPreciseArrayForString(value.ToUpper(), cLoginId.Length); }
            }
            /// <summary>
            /// Reserved for future, Default is blank
            /// </summary>
            public char[] cReserved;
            /// <summary>
            /// Reserved for future, Default is blank
            /// </summary>
            public string Prop03Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToUpper(), cLoginId.Length); }
            }

            public LogOffRequest(Int16 tranCode)
            {
                header = new MessageHeader(tranCode);
                cLoginId = new char[30];
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                byte[] LogOfRequest = new byte[CConstants.LogOffRequestSize];
                try
                {
                    Buffer.BlockCopy(header.StructToByte(), 0, LogOfRequest, 0, 6);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cLoginId), 0, LogOfRequest, 6, 30);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, LogOfRequest, 36, 100);
                    return LogOfRequest;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return LogOfRequest;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01Header.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));
                Prop02LoginId = Encoding.ASCII.GetString(ByteStructure, 6, 30);
                Prop03Reserved = Encoding.ASCII.GetString(ByteStructure, 36, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "LoginId = " + Prop02LoginId + "|" + "Reserved = " + Prop03Reserved;
            }
        }

        #endregion

        #region Request ScripMaster

        public struct ScripMasterRequest : IStruct
        {
            /*
                                *  Message header                           6      6
                                *  Char ExchangeCode[2]                     2      8
                                   CHAR Reserved [100]                    100    108
                                             
                                * ================================================
                                * Total                                   108
                                * ================================================
                                */

            private MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            /// <summary>
            /// Code for each exchange
            /// </summary>
            private Char[] cExchangeCode;
            /// <summary>
            ///Code for each exchange
            /// </summary>
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }
            /// <summary>
            /// Reserved for future
            /// </summary>
            private Char[] cReserved;
            /// <summary>
            /// Reserved for future
            /// </summary>
            public string Prop03Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public ScripMasterRequest(bool b)
            {
                cHeader = new MessageHeader(1);
                cExchangeCode = new char[2];
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                byte[] SripMasterRequest = new byte[CConstants.ScripMasterRequest];
                try
                {
                    Buffer.BlockCopy(cHeader.StructToByte(), 0, SripMasterRequest, 0, 6);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cExchangeCode), 0, SripMasterRequest, 6, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, SripMasterRequest, 8, 100);
                    return SripMasterRequest;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return SripMasterRequest;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01Header.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 6, 2);
                Prop03Reserved = Encoding.ASCII.GetString(ByteStructure, 8, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "ExchangeCode = " + Prop02ExchangeCode + "|" + "Reserved = " + Prop03Reserved;
            }
        }

        #endregion

        #region ScripMaster response

        public struct ScripMasterResponse : IStruct
        {
            /*
                                          *  Message header                           6      6
                                          *  Char ExchangeCode[2]                     2      8
                                          *  Scrip Master List                       
                                             CHAR Reserved [100]                    100    108
                                             
                                          * ================================================
                                          * Total                                   108
                                          * ================================================
                                          */
            private MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            /// <summary>
            /// Code for each exchange
            /// </summary>
            private Char[] cExchangeCode;
            /// <summary>
            ///Code for each exchange
            /// </summary>
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }

            /// <summary>
            /// Reserved for future
            /// </summary>
            private Char[] cReserved;
            /// <summary>
            /// Reserved for future
            /// </summary>
            public string Prop03Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public ScripMasterResponse(Int16 transCode)
            {
                cHeader = new MessageHeader(transCode);
                cExchangeCode = new char[2];
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                cHeader = new MessageHeader(1);
                cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                Prop01Header = cHeader;
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 6, 2);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "ExchangeCode = " + Prop02ExchangeCode + "|" + "Reserved = " + Prop03Reserved;
            }
        }

        public struct NCScripMaster : IStruct
        {
            /*
                                                     Long DataLength                              4      4
                                                  *  Char Segment[10]                            10     14
                                                  *  Char ScripCode [10]                         10     24
                                                     CHAR ScripShortName [60]                    60     84                                                   
            *                                       Char Reserved[100]                          100    184
                                                  * ================================================
                                                  * Total                                       184
                                                  * ================================================
                                                  */
            public Int32 cDataLength;
            public Int32 Prop01DataLength
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            public Char[] cSegment;
            public string Prop02Segment
            {
                get { return new string(cSegment); }
                set { cSegment = CUtility.GetPreciseArrayForString(value, cSegment.Length); }
            }

            public Char[] cScripCode;
            public string Prop03ScripCode
            {
                get { return new string(cScripCode); }
                set { cScripCode = CUtility.GetPreciseArrayForString(value, cScripCode.Length); }
            }

            public Char[] cScripShortName;
            public string Prop04ScripShortName
            {
                get { return new string(cScripShortName); }
                set { cScripShortName = CUtility.GetPreciseArrayForString(value, cScripShortName.Length); }
            }

            public Char[] cReserved;
            public string Prop05Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public NCScripMaster(bool a)
            {
                cDataLength = 0;
                cSegment = new Char[10];
                cScripCode = new Char[10];
                cScripShortName = new Char[60];
                cReserved = new Char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLength = BitConverter.ToInt32(ByteStructure, 0);
                Prop02Segment = Encoding.ASCII.GetString(ByteStructure, 4, 10);
                Prop03ScripCode = Encoding.ASCII.GetString(ByteStructure, 14, 10);
                Prop04ScripShortName = Encoding.ASCII.GetString(ByteStructure, 24, 60);
                Prop05Reserved = Encoding.ASCII.GetString(ByteStructure, 84, 100);
            }

            public override string ToString()
            {
                return "DataLength = " + Prop01DataLength + "|" + "Segment = " + Prop02Segment + "|" + "ScripCode = " + Prop03ScripCode + "|" + "ScripShortName = " + Prop04ScripShortName + "|" + "Reserved = " + Prop05Reserved;
            }
        }

        public struct NFScripMaster : IStruct
        {
            /*
                                                      Long DataLength                             4      4
                                                   *  Char DerivativeType[10]                     10     14
                                                   *  Char ScripCode [10]                         10     24
                                                      CHAR ScripShortName [60]                    60     84
                                                     CHAR ExpiryDate[15]                          15     99
             *                                       Char FutOption[10]                           10    109
             *                                       Long StrikePrice                              4    113
             *                                       Long LotSize                                  4    117
             *                                       Char Reserved[100]                          100    217
                                                   * ================================================
                                                   * Total                                       217
                                                   * ================================================
                                                   */

            private Int32 cDataLength;
            public Int32 Prop01DataLength
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private Char[] cDerivativeType;
            public string Prop02DerivativeType
            {
                get { return new string(cDerivativeType); }
                set { cDerivativeType = CUtility.GetPreciseArrayForString(value, cDerivativeType.Length); }
            }

            private Char[] cScripCode;
            public string Prop03ScripCode
            {
                get { return new string(cScripCode); }
                set { cScripCode = CUtility.GetPreciseArrayForString(value, cScripCode.Length); }
            }

            private Char[] cScripShortName;
            public string Prop04ScripShortName
            {
                get { return new string(cScripShortName); }
                set { cScripShortName = CUtility.GetPreciseArrayForString(value, cScripShortName.Length); }
            }

            private Char[] cExpiryDate;
            public string Prop05ExpiryDate
            {
                get { return new string(cExpiryDate); }
                set { cExpiryDate = CUtility.GetPreciseArrayForString(value, cExpiryDate.Length); }
            }

            private Char[] cFutOption;
            public string Prop06FutOption
            {
                get { return new string(cFutOption); }
                set { cFutOption = CUtility.GetPreciseArrayForString(value, cFutOption.Length); }
            }

            private Int32 cStrikePrice;
            public Int32 Prop07StrikePrice
            {
                get { return cStrikePrice; }
                set { cStrikePrice = value; }
            }

            private Int32 cLotSize;
            public Int32 Prop08LotSize
            {
                get { return cLotSize; }
                set { cLotSize = value; }
            }
            private Char[] cReserved;
            public string Prop09Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public NFScripMaster(bool b)
            {
                cDataLength = 0;
                cDerivativeType = new Char[10];
                cScripCode = new Char[10];
                cScripShortName = new Char[60];
                cExpiryDate = new Char[15];
                cFutOption = new Char[10];
                cStrikePrice = 0;
                cLotSize = 0;
                cReserved = new Char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLength = BitConverter.ToInt32(ByteStructure, 0);
                Prop02DerivativeType = Encoding.ASCII.GetString(ByteStructure, 4, 10);
                Prop03ScripCode = Encoding.ASCII.GetString(ByteStructure, 14, 10);
                Prop04ScripShortName = Encoding.ASCII.GetString(ByteStructure, 24, 60);
                Prop05ExpiryDate = Encoding.ASCII.GetString(ByteStructure, 84, 15);
                Prop06FutOption = Encoding.ASCII.GetString(ByteStructure, 99, 10);
                Prop07StrikePrice = BitConverter.ToInt32(ByteStructure, 109);
                Prop08LotSize = BitConverter.ToInt32(ByteStructure, 113);
                Prop09Reserved = Encoding.ASCII.GetString(ByteStructure, 117, 100);
            }

            public override string ToString()
            {
                return "DisplayLotSize = " + Prop01DataLength + "|" + "DerivativeType = " + Prop02DerivativeType + "|" + "ScripCode = " + Prop03ScripCode + "|" + "ScripShortName = " + Prop04ScripShortName + "|" + "ExpiryDate = " + Prop05ExpiryDate + "|" + "FutOption = " + Prop06FutOption + "|" + "StrikePrice = " + Prop07StrikePrice + "|" + "LotSize = " + Prop08LotSize + "|" + "Reserved = " + Prop09Reserved;
            }
        }

        public struct RNScripMaster : IStruct
        {
            /*
                                                     Long DataLength                             4      4
                                                  *  Char CurrencyType[10]                      10    14
                                                  *  Char ScripCode [10]                         10    24
                                                     CHAR ScripShortName [60]                    60    84
                                                    CHAR ExpiryDate[15]                          15    99
            *                                       Char FutOption[10]                           10   109
            *                                       Long StrikePrice                              4   113
            *                                       Long LotSize                                  4   117
             *                                      Long DisplayLotSize                           4   121
             *                                      char LotType[25]                             25   146   
             *                                      char DisplayLotType[35]                      35   181
             *                                      char OFType[15]                              15   196
             *                                      Long MinimumTradeQty                          4   200
             *                                      char PriceTick[25]                           25   225
             *                                      Long Multipler                                4   229
            *                                       Char Reserved[100]                          100   329
                                                  * ================================================
                                                  * Total                                       329
                                                  * ================================================
             
             * 
             */

            private Int32 cDataLength;
            public Int32 Prop01DataLength
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private Char[] cCurrencyType;
            public string Prop02CurrencyType
            {
                get { return new string(cCurrencyType); }
                set { cCurrencyType = CUtility.GetPreciseArrayForString(value, cCurrencyType.Length); }
            }

            private Char[] cScripCode;
            public string Prop03ScripCode
            {
                get { return new string(cScripCode); }
                set { cScripCode = CUtility.GetPreciseArrayForString(value, cScripCode.Length); }
            }

            private Char[] cScripShortName;
            public string Prop04ScripShortName
            {
                get { return new string(cScripShortName); }
                set { cScripShortName = CUtility.GetPreciseArrayForString(value, cScripShortName.Length); }
            }

            private Char[] cExpiryDate;
            public string Prop05ExpiryDate
            {
                get { return new string(cExpiryDate); }
                set { cExpiryDate = CUtility.GetPreciseArrayForString(value, cExpiryDate.Length); }
            }

            private Char[] cFutOption;
            public string Prop06FutOption
            {
                get { return new string(cFutOption); }
                set { cFutOption = CUtility.GetPreciseArrayForString(value, cFutOption.Length); }
            }

            private Int32 cStrikePrice;
            public Int32 Prop07StrikePrice
            {
                get { return cStrikePrice; }
                set { cStrikePrice = value; }
            }

            private Int32 cLotSize;
            public Int32 Prop08LotSize
            {
                get { return cLotSize; }
                set { cLotSize = value; }
            }

            private Int32 cDisplayLotSize;
            public Int32 Prop09DisplayLotSize
            {
                get { return cDisplayLotSize; }
                set { cDisplayLotSize = value; }
            }

            private Char[] cLotType;
            public string Prop10LotType
            {
                get { return new string(cLotType); }
                set { cLotType = CUtility.GetPreciseArrayForString(value, cLotType.Length); }
            }

            private Char[] cDisplayLotType;
            public string Prop11DisplayLotType
            {
                get { return new string(cDisplayLotType); }
                set { cDisplayLotType = CUtility.GetPreciseArrayForString(value, cDisplayLotType.Length); }
            }

            private Char[] cOFType;
            public string Prop12OFType
            {
                get { return new string(cOFType); }
                set { cOFType = CUtility.GetPreciseArrayForString(value, cOFType.Length); }
            }

            private Int32 cMinimumTradeQty;
            public Int32 Prop13MinimumTradeQty
            {
                get { return cMinimumTradeQty; }
                set { cMinimumTradeQty = value; }
            }

            private Char[] cPriceTick;
            public string Prop14PriceTick
            {
                get { return new string(cPriceTick); }
                set { cPriceTick = CUtility.GetPreciseArrayForString(value, cPriceTick.Length); }
            }

            private Int32 cMultipler;
            public Int32 Prop15Multipler
            {
                get { return cMultipler; }
                set { cMultipler = value; }
            }

            private Char[] cReserved;
            public string Prop16Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public RNScripMaster(bool b)
            {
                cDataLength = 0;
                cCurrencyType = new Char[10];
                cScripCode = new Char[10];
                cScripShortName = new Char[60];
                cExpiryDate = new Char[15];
                cFutOption = new Char[10];
                cStrikePrice = 0;
                cLotSize = 0;
                cDisplayLotSize = 0;
                cLotType = new char[25];
                cDisplayLotType = new char[35];
                cOFType = new char[15];
                cMinimumTradeQty = 0;
                cPriceTick = new char[25];
                cMultipler = 0;
                cReserved = new Char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLength = BitConverter.ToInt32(ByteStructure, 0);
                Prop02CurrencyType = Encoding.ASCII.GetString(ByteStructure, 4, 10);
                Prop03ScripCode = Encoding.ASCII.GetString(ByteStructure, 14, 10);
                Prop04ScripShortName = Encoding.ASCII.GetString(ByteStructure, 24, 60);
                Prop05ExpiryDate = Encoding.ASCII.GetString(ByteStructure, 84, 15);
                Prop06FutOption = Encoding.ASCII.GetString(ByteStructure, 99, 10);
                Prop07StrikePrice = BitConverter.ToInt32(ByteStructure, 109);
                Prop08LotSize = BitConverter.ToInt32(ByteStructure, 113);
                Prop09DisplayLotSize = BitConverter.ToInt32(ByteStructure, 117);
                Prop10LotType = Encoding.ASCII.GetString(ByteStructure, 121, 25);
                Prop11DisplayLotType = Encoding.ASCII.GetString(ByteStructure, 146, 35);
                Prop12OFType = Encoding.ASCII.GetString(ByteStructure, 181, 15);
                Prop13MinimumTradeQty = BitConverter.ToInt32(ByteStructure, 196);
                Prop14PriceTick = Encoding.ASCII.GetString(ByteStructure, 200, 25);
                Prop15Multipler = BitConverter.ToInt32(ByteStructure, 225);
                Prop16Reserved = Encoding.ASCII.GetString(ByteStructure, 229, 100);
            }

            public override string ToString()
            {
                return "DataLength = " + Prop01DataLength + "|" + "CurrencyType = " + Prop02CurrencyType + "|" + "ScripCode = " + Prop03ScripCode + "|" + "ScripShortName = " + Prop04ScripShortName + "|" + "ExpiryDate = " + Prop05ExpiryDate + "|" + "FutOption = " + Prop06FutOption + "|" + "StrikePrice = " + Prop07StrikePrice + "|" + "LotSize = " + Prop08LotSize
                    + "|" + "DisplayLotSize = " + Prop09DisplayLotSize + "|" + "LotType = " + Prop10LotType + "|" + "DisplayLotType = " + Prop11DisplayLotType + "|" + "OFType = "
                    + Prop12OFType + "|" + "MinimumTradeQty = " + Prop13MinimumTradeQty + "|" + "PriceTick = " + Prop14PriceTick + "|" + "Multipler = " + Prop15Multipler + "|" + "Reserved = " + Prop16Reserved;
            }
        }

        public struct NFScripMasterResponse : IStruct
        {
            /*
                                         *  Message header                           6      6
                                         *  Char ExchangeCode[2]                     2      8
                                         *  List<NFScripMaster>                    217    225
                                            CHAR Reserved [100]                    100    325
                                             
                                         * ================================================
                                         * Total                                   325
                                         * ================================================
                                         */
            private MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            /// <summary>
            /// Code for each exchange
            /// </summary>
            private Char[] cExchangeCode;
            /// <summary>
            ///Code for each exchange
            /// </summary>
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }

            private static Dictionary<string, NFScripMaster> dicNFScripMaster;
            public static Dictionary<string, NFScripMaster> DicNFScripMaster
            {
                get { return dicNFScripMaster; }
                set { dicNFScripMaster = value; }
            }

            /// <summary>
            /// Reserved for future
            /// </summary>
            private Char[] cReserved;
            /// <summary>
            /// Reserved for future
            /// </summary>
            public string Prop03Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public NFScripMasterResponse(Int16 TransCode)
            {
                cHeader = new MessageHeader(TransCode);
                cExchangeCode = new Char[2];
                cReserved = new Char[100];
                dicNFScripMaster = new Dictionary<string, NFScripMaster>();
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                //Prop01Header.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));
                //Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 6, 2);
                //Dictionary<string, NFScripMaster> dicScripMaster = new Dictionary<string, NFScripMaster>();
                NFScripMaster nfScripMaster;
                int numberOfScrips = ByteStructure.Length / 217;
                int sourceIndex = 0;
                int destinationIndex = 216;
                for (int i = 0; i < numberOfScrips; i++)
                {
                    nfScripMaster = new NFScripMaster();
                    byte[] nfScripData = new byte[217];
                    Array.ConstrainedCopy(ByteStructure, sourceIndex, nfScripData, 0, 217);
                    nfScripMaster.ByteToStruct(nfScripData);
                    DicNFScripMaster.Add(nfScripMaster.Prop04ScripShortName, nfScripMaster);
                    sourceIndex += 217;
                    destinationIndex += 217;
                }

                Prop03Reserved = Encoding.ASCII.GetString(ByteStructure, 100, 108);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "ExchangeCode = " + Prop02ExchangeCode + "|" + "Reserved = " + Prop03Reserved;
            }
        }

        public struct CommodityMaster : IStruct
        {
            /*
                                                     Long DataLength                             4      4                                                  * 
                                                  *  Char ScripCode [10]                         10    14
                                                     CHAR ScripShortName [60]                    60    74
             *                                       Long DisplayLotSize                          4    78
             *                                       char DisplayLotType[20]                     20    98             * 
                                                     CHAR ExpiryDate[15]                         15   113
             *                                       Long LotSize                                 4   117             *                                      
             *                                       char LotType[25]                            10   127   
            *                                        Long Multipler                               4   131
             *                                       char PriceTick[25]                           4   135             *                                      
            *                                        Char Reserved[100]                          100  235
                                                  * ================================================
                                                  * Total                                       235
                                                  * ================================================            
             * 
             */

            private Int32 cDataLength;
            public Int32 Prop01DataLength
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private Char[] cScripCode;
            public string Prop02ScripCode
            {
                get { return new string(cScripCode); }
                set { cScripCode = CUtility.GetPreciseArrayForString(value, cScripCode.Length); }
            }

            private Char[] cScripShortName;
            public string Prop03ScripShortName
            {
                get { return new string(cScripShortName); }
                set { cScripShortName = CUtility.GetPreciseArrayForString(value, cScripShortName.Length); }
            }
            private Int32 cDisplayLotSize;
            public Int32 Prop04DisplayLotSize
            {
                get { return cDisplayLotSize; }
                set { cDisplayLotSize = value; }
            }

            private Char[] cDisplayLotType;
            public string Prop05DisplayLotType
            {
                get { return new string(cDisplayLotType); }
                set { cDisplayLotType = CUtility.GetPreciseArrayForString(value, cDisplayLotType.Length); }
            }

            private Char[] cExpiryDate;
            public string Prop06ExpiryDate
            {
                get { return new string(cExpiryDate); }
                set { cExpiryDate = CUtility.GetPreciseArrayForString(value, cExpiryDate.Length); }
            }

            private Int32 cLotSize;
            public Int32 Prop07LotSize
            {
                get { return cLotSize; }
                set { cLotSize = value; }
            }

            private Char[] cLotType;
            public string Prop08LotType
            {
                get { return new string(cLotType); }
                set { cLotType = CUtility.GetPreciseArrayForString(value, cLotType.Length); }
            }

            private Int32 cMultipler;
            public Int32 Prop09Multipler
            {
                get { return cMultipler; }
                set { cMultipler = value; }
            }

            private Int32 cPriceTick;
            public Int32 Prop10PriceTick
            {
                get { return cPriceTick; }
                set { cPriceTick = value; }
            }

            private Char[] cReserved;
            public string Prop11Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public CommodityMaster(bool b)
            {
                cDataLength = 0;
                cScripCode = new Char[10];
                cScripShortName = new Char[60];
                cDisplayLotSize = 0;
                cDisplayLotType = new char[20];
                cExpiryDate = new Char[15];
                cLotSize = 0;
                cLotType = new char[10];
                cPriceTick = 0;
                cMultipler = 0;
                cReserved = new Char[100];
            }

            public byte[] StructToByte()
            {
                return new byte[2];
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLength = BitConverter.ToInt32(ByteStructure, 0);
                Prop02ScripCode = Encoding.ASCII.GetString(ByteStructure, 4, 10);
                Prop03ScripShortName = Encoding.ASCII.GetString(ByteStructure, 14, 60);
                Prop04DisplayLotSize = BitConverter.ToInt32(ByteStructure, 74);
                Prop05DisplayLotType = Encoding.ASCII.GetString(ByteStructure, 78, 20);
                Prop06ExpiryDate = Encoding.ASCII.GetString(ByteStructure, 98, 15);
                Prop07LotSize = BitConverter.ToInt32(ByteStructure, 113);
                Prop08LotType = Encoding.ASCII.GetString(ByteStructure, 117, 10);
                Prop09Multipler = BitConverter.ToInt32(ByteStructure, 127);
                Prop10PriceTick = BitConverter.ToInt32(ByteStructure, 131);
                Prop11Reserved = Encoding.ASCII.GetString(ByteStructure, 135, 100);
            }

            public override string ToString()
            {
                return Prop01DataLength + "|" + "ScripCode = " + Prop02ScripCode + "|" + "ScripShortName = " + Prop03ScripShortName + "|" + "DisplayLotSize = " + Prop04DisplayLotSize + "|" + "LotType = " + Prop05DisplayLotType + "|" + "ExpiryDate = " + Prop06ExpiryDate + "|" + "LotSize = " + Prop07LotSize + "|" + "LotType = " + Prop08LotType
                    + "|" + "Multipler = " + Prop09Multipler + "|" + "PriceTick = " + Prop10PriceTick + "|" + "Reserved = " + Prop11Reserved;
            }
        }

        #endregion

        #region Feed Request & Response

        public struct FeedRequest : IStruct
        {
            /*
             *          MessageHeader               6       6
                        Int16 Count                 4       10
                        Char[][12] ScripList        12      22 
                        Reserved                    100     122
             * 
             */

            private MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            private Int16 cCount;
            public Int16 Prop02Count
            {
                get { return cCount; }
                set { cCount = value; }
            }
            private char[] cScripList;
            public string Prop03ScripList
            {
                get { return new string(cScripList); }
                set { cScripList = CUtility.GetPreciseArrayForString(value.ToString(), cScripList.Length); }
            }
            private char[] cReserved;
            public string Prop04Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public FeedRequest(bool b)
            {
                cHeader = new MessageHeader(1);
                cCount = 0;
                cScripList = new char[12];
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                byte[] FeedRequest = new byte[CConstants.FeedRequestSize];
                try
                {
                    Buffer.BlockCopy(cHeader.StructToByte(), 0, FeedRequest, 0, 6);
                    Buffer.BlockCopy(CUtility.GetBytesFromNumbers(cCount), 0, FeedRequest, 6, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cScripList), 0, FeedRequest, 8, 12);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, FeedRequest, 20, 100);
                    return FeedRequest;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    MessageBox.Show(ex.ToString());
                    return FeedRequest;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "Count = " + Prop02Count + "|" + "ScripList = " + Prop03ScripList + "|" + "Reserved = " + Prop04Reserved;
            }
        }

        public struct FeedResponse : IStruct
        {
            /*
             *  MessageHeader    
Exchange                                                                               CHAR[5]
ScripToken                                                                             CHAR[10]
LTPrice             Last Traded Price Default value is ‘0’                             LONG
LTQuantity          Last Traded Quantity                                               LONG
LTDate              Last Traded Data Time Format MM/dd/yyyy HH:mm:ss                   CHAR[25]
BidPrice            Price offered by Buyer Default ‘0’                                 LONG
BidQuantity         Buying Quantity                                                    LONG
OfferPrice          Seller’s Offer price Default ‘0’                                   LONG
OfferQuantity       Seller Quantity                                                    LONG
TotalTradedQty      Sum of Traded Quantity                                             LONG
TradedQuantity      Total traded quantity                                              LONG
AverageTradePrice   Average of Traded price Default ‘0’                                LONG
Open                Today’s Open price Default ‘0’                                     LONG
High                Today’s High Default ‘0’                                           LONG
Low                 Today’s Low Default ‘0’                                            LONG
Close               Yesterday Close Price Default ‘0’                                  LONG
PerChange           Percentage change of LTPrice and close Default multiplied by ‘100’  LONG
TurnOver            Total traded amount in Thousands                                   LONG
YearlyHigh          52 week High price  Default ‘0’                                    LONG
YearlyLow           52 week Low price Default ‘0’                                      LONG
UpperCkt            Upper limit for today’s trading Default ‘0’                        LONG
LowerCkt            Lower limit for today’s trading Default ‘0’                        LONG
Difference          Difference between LTPrice and Close Price                         LONG
CostofCarry1        Cost of Carry1                                                     LONG
CostOfCarry2        Cost Of Carry2                                                     LONG
ChangeIndicator     Upward/Downward Indicator +/-                                      CHAR[10]
SpotPrice           Spot Price of the future contract Default ‘0’                      LONG
OITime              Open Interest Time                                                 CHAR[20]
OI                  Open Interest Default ‘0’                                           LONG
OIHigh              Open Interest High Default ‘0’                                     LONG
OILow               Open Interest Low Default ‘0’                                      LONG
TotalTrades         Total Trades                                                       LONG
TradeValueFlag      TradeValueFlag                                                     CHAR[10]
Trend               Trend                                                              CHAR[10]
SunFlag             SunFlag                                                             CHAR[10]
AllnoneFlag         Flag of All/None Default Blank                                     CHAR[10]
Tender              Tender Default ‘0’                                                   LONG
PriceQuotation      PriceQuotation (Only for Commodity) in terms of Kg/Grams/ltr etc..,  CHAR[20]
TotalBuyQty         Total Buy Quantity                                                   LONG
TotalSellQty        Total Sell Quantity                                                  LONG
SegmentId           Ignore this field                                                    CHAR[20]
OIDifference        OI Difference Default ‘0’                                            LONG
OIDiffPercentage    OI Difference Percentage Default ‘0’                                 LONG
Reserved            Reserved for future Default  blank                                   CHAR[100]


             */

            private MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            private char[] cExchange;
            public string Prop02Exchange
            {
                get { return new string(cExchange); }
                set { cExchange = CUtility.GetPreciseArrayForString(value, cExchange.Length); }
            }
            private char[] cScripToken;
            public string Prop03ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value, cScripToken.Length); }
            }
            private Int32 cLTPrice;
            public Int32 Prop04LTPrice
            {
                get { return cLTPrice; }
                set { cLTPrice = value; }
            }
            private Int32 cLTQuantity;
            public Int32 Prop05LTQuantity
            {
                get { return cLTQuantity; }
                set { cLTQuantity = value; }
            }
            private char[] cLTDate;
            public string Prop06LTDate
            {
                get { return new string(cLTDate); }
                set { cLTDate = CUtility.GetPreciseArrayForString(value, cLTDate.Length); }
            }
            private Int32 cBidPrice;
            public Int32 Prop07BidPrice
            {
                get { return cBidPrice; }
                set { cBidPrice = value; }
            }
            private Int32 cBidQuantity;
            public Int32 Prop08BidQuantity
            {
                get { return cBidQuantity; }
                set { cBidQuantity = value; }
            }
            private Int32 cOfferPrice;
            public Int32 Prop09OfferPrice
            {
                get { return cOfferPrice; }
                set { cOfferPrice = value; }
            }
            private Int32 cOfferQuantity;
            public Int32 Prop10OfferQuantity
            {
                get { return cOfferQuantity; }
                set { cOfferQuantity = value; }
            }
            private Int32 cTotalTradedQty;
            public Int32 Prop11TotalTradedQty
            {
                get { return cTotalTradedQty; }
                set { cTotalTradedQty = value; }
            }
            private Int32 cTradedQuantity;
            public Int32 Prop12TradedQuantity
            {
                get { return cTradedQuantity; }
                set { cTradedQuantity = value; }
            }
            private Int32 cAverageTradePrice;
            public Int32 Prop13AverageTradePrice
            {
                get { return cAverageTradePrice; }
                set { cAverageTradePrice = value; }
            }
            private Int32 cOpen;
            public Int32 Prop14Open
            {
                get { return cOpen; }
                set { cOpen = value; }
            }
            private Int32 cHigh;
            public Int32 Prop15High
            {
                get { return cHigh; }
                set { cHigh = value; }
            }
            private Int32 cLow;
            public Int32 Prop16Low
            {
                get { return cLow; }
                set { cLow = value; }
            }
            private Int32 cClose;
            public Int32 Prop17Close
            {
                get { return cClose; }
                set { cClose = value; }
            }
            private Int32 cPerChange;
            public Int32 Prop18PerChange
            {
                get { return cPerChange; }
                set { cPerChange = value; }
            }
            private Int32 cTurnOver;
            public Int32 Prop19TurnOver
            {
                get { return cTurnOver; }
                set { cTurnOver = value; }
            }
            private Int32 cYearlyHigh;
            public Int32 Prop20YearlyHigh
            {
                get { return cYearlyHigh; }
                set { cYearlyHigh = value; }
            }
            private Int32 cYearlyLow;
            public Int32 Prop21YearlyLow
            {
                get { return cYearlyLow; }
                set { cYearlyLow = value; }
            }
            private Int32 cUpperCkt;
            public Int32 Prop22UpperCkt
            {
                get { return cUpperCkt; }
                set { cUpperCkt = value; }
            }
            private Int32 cLowerCkt;
            public Int32 Prop23LowerCkt
            {
                get { return cLowerCkt; }
                set { cLowerCkt = value; }
            }
            private Int32 cDifference;
            public Int32 Prop24Difference
            {
                get { return cDifference; }
                set { cDifference = value; }
            }
            private Int32 cCostofCarry1;
            public Int32 Prop25CostofCarry1
            {
                get { return cCostofCarry1; }
                set { cCostofCarry1 = value; }
            }
            private Int32 cCostOfCarry2;
            public Int32 Prop26CostOfCarry2
            {
                get { return cCostOfCarry2; }
                set { cCostOfCarry2 = value; }
            }
            private char[] cChangeIndicator;
            public string Prop27ChangeIndicator
            {
                get { return new string(cChangeIndicator); }
                set { cChangeIndicator = CUtility.GetPreciseArrayForString(value, cChangeIndicator.Length); }
            }
            private Int32 cSpotPrice;
            public Int32 Prop28SpotPrice
            {
                get { return cSpotPrice; }
                set { cSpotPrice = value; }
            }
            private char[] cOITime;
            public string Prop29OITime
            {
                get { return new string(cOITime); }
                set { cOITime = CUtility.GetPreciseArrayForString(value, cOITime.Length); }
            }
            private Int32 cOI;
            public Int32 Prop30OI
            {
                get { return cOI; }
                set { cOI = value; }
            }
            private Int32 cOIHigh;
            public Int32 Prop31OIHigh
            {
                get { return cOIHigh; }
                set { cOIHigh = value; }
            }
            private Int32 cOILow;
            public Int32 Prop32OILow
            {
                get { return cOILow; }
                set { cOILow = value; }
            }
            private Int32 cTotalTrades;
            public Int32 Prop33TotalTrades
            {
                get { return cTotalTrades; }
                set { cTotalTrades = value; }
            }
            private char[] cTradeValueFlag;
            public string Prop34TradeValueFlag
            {
                get { return new string(cTradeValueFlag); }
                set { cTradeValueFlag = CUtility.GetPreciseArrayForString(value, cTradeValueFlag.Length); }
            }
            private char[] cTrend;
            public string Prop35Trend
            {
                get { return new string(cTrend); }
                set { cTrend = CUtility.GetPreciseArrayForString(value, cTrend.Length); }
            }
            private char[] cSunFlag;
            public string Prop36SunFlag
            {
                get { return new string(cSunFlag); }
                set { cSunFlag = CUtility.GetPreciseArrayForString(value, cSunFlag.Length); }
            }
            private char[] cAllnoneFlag;
            public string Prop37AllnoneFlag
            {
                get { return new string(cAllnoneFlag); }
                set { cAllnoneFlag = CUtility.GetPreciseArrayForString(value, cAllnoneFlag.Length); }
            }
            private Int32 cTender;
            public Int32 Prop38Tender
            {
                get { return cTender; }
                set { cTender = value; }
            }
            private char[] cPriceQuotation;
            public string Prop39PriceQuotation
            {
                get { return new string(cPriceQuotation); }
                set { cPriceQuotation = CUtility.GetPreciseArrayForString(value, cPriceQuotation.Length); }
            }
            private Int32 cTotalBuyQty;
            public Int32 Prop40TotalBuyQty
            {
                get { return cTotalBuyQty; }
                set { cTotalBuyQty = value; }
            }
            private Int32 cTotalSellQty;
            public Int32 Prop41TotalSellQty
            {
                get { return cTotalSellQty; }
                set { cTotalSellQty = value; }
            }
            private char[] cSegmentId;
            public string Prop42SegmentId
            {
                get { return new string(cSegmentId); }
                set { cSegmentId = CUtility.GetPreciseArrayForString(value, cSegmentId.Length); }
            }
            private Int32 cOIDifference;
            public Int32 Prop43OIDifference
            {
                get { return cOIDifference; }
                set { cOIDifference = value; }
            }
            private Int32 cOIDiffPercentage;
            public Int32 Prop44OIDiffPercentage
            {
                get { return cOIDiffPercentage; }
                set { cOIDiffPercentage = value; }
            }
            private char[] cReserved;
            public string Prop45Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public FeedResponse(bool b)
            {
                cHeader = new MessageHeader(1);
                cExchange = new char[2];
                cScripToken = new char[10];
                cLTPrice = 0;
                cLTQuantity = 0;
                cLTDate = cScripToken = new char[25];
                cBidPrice = 0;
                cBidQuantity = 0;
                cOfferPrice = 0;
                cOfferQuantity = 0;
                cTotalTradedQty = 0;
                cTradedQuantity = 0;
                cAverageTradePrice = 0;
                cOpen = 0;
                cHigh = 0;
                cLow = 0;
                cClose = 0;
                cPerChange = 0;
                cTurnOver = 0;
                cYearlyHigh = 0;
                cYearlyLow = 0;
                cUpperCkt = 0;
                cLowerCkt = 0;
                cDifference = 0;
                cCostofCarry1 = 0;
                cCostOfCarry2 = 0;
                cChangeIndicator = cScripToken = new char[10];
                cSpotPrice = 0;
                cOITime = cScripToken = new char[20];
                cOI = 0;
                cOIHigh = 0;
                cOILow = 0;
                cTotalTrades = 0;
                cTradeValueFlag = cScripToken = new char[10];
                cTrend = cScripToken = new char[10];
                cSunFlag = cScripToken = new char[10];
                cAllnoneFlag = cScripToken = new char[10];
                cTender = 0;
                cPriceQuotation = cScripToken = new char[20];
                cTotalBuyQty = 0;
                cTotalSellQty = 0;
                cSegmentId = cScripToken = new char[20];
                cOIDifference = 0;
                cOIDiffPercentage = 0;
                cReserved = cScripToken = new char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                cHeader = new MessageHeader(1);
                cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                Prop01Header = cHeader;
                Prop02Exchange = Encoding.ASCII.GetString(ByteStructure, 6, 5);
                Prop03ScripToken = Encoding.ASCII.GetString(ByteStructure, 11, 10);
                Prop04LTPrice = BitConverter.ToInt32(ByteStructure, 21);
                Prop05LTQuantity = BitConverter.ToInt32(ByteStructure, 25);
                Prop06LTDate = Encoding.ASCII.GetString(ByteStructure, 29, 25);
                Prop07BidPrice = BitConverter.ToInt32(ByteStructure, 54);
                Prop08BidQuantity = BitConverter.ToInt32(ByteStructure, 58);
                Prop09OfferPrice = BitConverter.ToInt32(ByteStructure, 62);
                Prop10OfferQuantity = BitConverter.ToInt32(ByteStructure, 66); //Added missing field
                Prop11TotalTradedQty = BitConverter.ToInt32(ByteStructure, 70);
                Prop12TradedQuantity = BitConverter.ToInt32(ByteStructure, 74);
                Prop13AverageTradePrice = BitConverter.ToInt32(ByteStructure, 78);
                Prop14Open = BitConverter.ToInt32(ByteStructure, 82);
                Prop15High = BitConverter.ToInt32(ByteStructure, 86);
                Prop16Low = BitConverter.ToInt32(ByteStructure, 90);
                Prop17Close = BitConverter.ToInt32(ByteStructure, 94);
                Prop18PerChange = BitConverter.ToInt32(ByteStructure, 98);
                Prop19TurnOver = BitConverter.ToInt32(ByteStructure, 102);
                Prop20YearlyHigh = BitConverter.ToInt32(ByteStructure, 106);
                Prop21YearlyLow = BitConverter.ToInt32(ByteStructure, 110);
                Prop22UpperCkt = BitConverter.ToInt32(ByteStructure, 114);
                Prop23LowerCkt = BitConverter.ToInt32(ByteStructure, 118);
                Prop24Difference = BitConverter.ToInt32(ByteStructure, 122);
                Prop25CostofCarry1 = BitConverter.ToInt32(ByteStructure, 126);
                Prop26CostOfCarry2 = BitConverter.ToInt32(ByteStructure, 130);
                Prop27ChangeIndicator = Encoding.ASCII.GetString(ByteStructure, 134, 10);
                Prop28SpotPrice = BitConverter.ToInt32(ByteStructure, 144);
                Prop29OITime = Encoding.ASCII.GetString(ByteStructure, 148, 20);
                Prop30OI = BitConverter.ToInt32(ByteStructure, 172);
                Prop31OIHigh = BitConverter.ToInt32(ByteStructure, 176);
                Prop32OILow = BitConverter.ToInt32(ByteStructure, 180);
                Prop33TotalTrades = BitConverter.ToInt32(ByteStructure, 184);
                Prop34TradeValueFlag = Encoding.ASCII.GetString(ByteStructure, 188, 10);
                Prop35Trend = Encoding.ASCII.GetString(ByteStructure, 192, 10);
                Prop36SunFlag = Encoding.ASCII.GetString(ByteStructure, 204, 10);
                Prop37AllnoneFlag = Encoding.ASCII.GetString(ByteStructure, 214, 10);
                Prop38Tender = BitConverter.ToInt32(ByteStructure, 224);
                Prop39PriceQuotation = Encoding.ASCII.GetString(ByteStructure, 228, 20);
                Prop40TotalBuyQty = BitConverter.ToInt32(ByteStructure, 248);
                Prop41TotalSellQty = BitConverter.ToInt32(ByteStructure, 252);
                Prop42SegmentId = Encoding.ASCII.GetString(ByteStructure, 256, 20);
                Prop43OIDifference = BitConverter.ToInt32(ByteStructure, 260);
                Prop44OIDiffPercentage = BitConverter.ToInt32(ByteStructure, 264);
                Prop45Reserved = Encoding.ASCII.GetString(ByteStructure, 284, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "Exchange = " + Prop02Exchange + "|" + "ScripToken = " + Prop03ScripToken + "|" + "LTPrice  = " + Prop04LTPrice + "|" + "LTQuantity = " + Prop05LTQuantity + "|" + "LTDate = " + Prop06LTDate + "|" + "BidPrice = " + Prop07BidPrice + "|" + "BidQuantity = " + Prop08BidQuantity
                    + "|" + "OfferPrice = " + Prop09OfferPrice + "|" + "OfferQuantity = " + Prop10OfferQuantity + "|" + "TotalTradedQty = " + Prop11TotalTradedQty + "|" + "TradedQuantity = "
                    + Prop12TradedQuantity + "|" + "AverageTradePrice = " + Prop13AverageTradePrice + "|" + "Open = " + Prop14Open + "|" + "High = " + Prop15High + "|" + "Low = " + Prop16Low + "|" + "Close = " + Prop17Close + "|" + "PerChange = " + Prop18PerChange + "|" + "TurnOver = " + Prop19TurnOver + "|" + "YearlyHigh = " + Prop20YearlyHigh
                     + "|" + "YearlyLow  = " + Prop21YearlyLow + "|" + "UpperCkt = " + Prop22UpperCkt + "|" + "LowerCkt = " + Prop23LowerCkt + "|" + "Difference  = " + Prop24Difference + "|" + "CostofCarry1 = " + Prop25CostofCarry1 + "|" + "CostOfCarry2 = " + Prop26CostOfCarry2 + "|" + "ChangeIndicator = " + Prop27ChangeIndicator + "|" + "SpotPrice = " + Prop28SpotPrice + "|" + "OITime = " + Prop29OITime + "|" + "OI = " + Prop30OI
                      + "|" + "High  = " + Prop31OIHigh + "|" + "OILow = " + Prop32OILow + "|" + "TotalTrades = " + Prop33TotalTrades + "|" + "TradeValueFlag = " + Prop34TradeValueFlag + "|" + "Trend = " + Prop35Trend
                    + "|" + "SunFlag  = " + Prop36SunFlag + "|" + "AllnoneFlag = " + Prop37AllnoneFlag + "|" + "Tender = " + Prop38Tender + "|" + "PriceQuotation = " + Prop39PriceQuotation + "|" + "TotalBuyQty = " + Prop40TotalBuyQty
                   + "|" + "SellQty  = " + Prop41TotalSellQty + "|" + "SegmentId = " + Prop42SegmentId + "|" + "OIDifference = " + Prop43OIDifference + "|" + "DiffPercentage = " + Prop44OIDiffPercentage + "|" + "Reserved = " + Prop45Reserved;
            }
        }

        public struct BidOffer : IStruct
        {
            /*
             *  MessageHeader 
             *  Exchange        Code for each exchange  CHAR[5]
                ScripToken      unique token number for each scrip                CHAR[10]
                BidPrice        Price offered by Buyer Default ‘0’                Long
                LONG            BidQuantity Buying Quantity                       LONG
                OfferPrice      Seller’s Offer price Default ‘0’                  LONG
                OfferQuantity   Seller Quantity                                   LONG
                TotalBuyQty     Total Buy Quantity                                LONG
                TotalSellQty    Total Sell Quantity                               LONG
                Reserved        Reserved for futured default  blank               CHAR[100]

             * 
             */

            MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            char[] cExchange;
            public string Prop02Exchange
            {
                get { return new string(cExchange); }
                set { cExchange = CUtility.GetPreciseArrayForString(value, cExchange.Length); }
            }
            char[] cScripToken;
            public string Prop03ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value, cScripToken.Length); }
            }
            Int32 cBidPrice;
            public Int32 Prop04BidPrice
            {
                get { return cBidPrice; }
                set { cBidPrice = value; }
            }
            Int32 cBidQuantity;
            public Int32 Prop05BidQuantity
            {
                get { return cBidQuantity; }
                set { cBidQuantity = value; }
            }
            Int32 cOfferPrice;
            public Int32 Prop06OfferPrice
            {
                get { return cOfferPrice; }
                set { cOfferPrice = value; }
            }
            Int32 cOfferQuantity;
            public Int32 Prop07OfferQuantity
            {
                get { return cOfferQuantity; }
                set { cOfferQuantity = value; }
            }
            Int32 cTotalBuyQty;
            public Int32 Prop08TotalBuyQty
            {
                get { return cTotalBuyQty; }
                set { cTotalBuyQty = value; }
            }
            Int32 cTotalSellQty;
            public Int32 Prop09TotalSellQty
            {
                get { return cTotalSellQty; }
                set { cTotalSellQty = value; }
            }
            char[] cReserved;
            public string Prop10Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public BidOffer(bool b)
            {
                cHeader = new MessageHeader(1);
                cExchange = new char[5];
                cScripToken = new char[10];
                cBidPrice = 0;
                cBidQuantity = 0;
                cOfferPrice = 0;
                cOfferQuantity = 0;
                cTotalBuyQty = 0;
                cTotalSellQty = 0;
                cReserved = new char[100];

            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                cHeader = new MessageHeader(1);
                cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                Prop01Header = cHeader;
                Prop02Exchange = Encoding.ASCII.GetString(ByteStructure, 6, 5);
                Prop03ScripToken = Encoding.ASCII.GetString(ByteStructure, 11, 10);
                Prop04BidPrice = BitConverter.ToInt32(ByteStructure, 21);
                Prop05BidQuantity = BitConverter.ToInt32(ByteStructure, 25);
                Prop06OfferPrice = BitConverter.ToInt32(ByteStructure, 29);
                Prop07OfferQuantity = BitConverter.ToInt32(ByteStructure, 33);
                Prop08TotalBuyQty = BitConverter.ToInt32(ByteStructure, 37);
                Prop09TotalSellQty = BitConverter.ToInt32(ByteStructure, 41);
                Prop10Reserved = Encoding.ASCII.GetString(ByteStructure, 45, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "Exchange = " + Prop02Exchange + "|" + "ScripToken = " + Prop03ScripToken + "|" + "BidPrice = " + Prop04BidPrice + "|" + "BidQuantity = " + Prop05BidQuantity + "|" + "OfferPrice  = " + Prop06OfferPrice + "|" + "OfferQuantity = " + Prop07OfferQuantity + "|" + "TotalBuyQty = " + Prop08TotalBuyQty
                    + "|" + "TotalSellQty = " + Prop09TotalSellQty + "|" + "Reserved = " + Prop10Reserved;
            }
        }

        public struct MarketDepthRequest : IStruct
        {
            /*  MessageHeader
                Exchange Code       Code for each exchange Refer Exchange Code      CHAR[5]
                Scrip Code          unique token number for each scrip              CHAR[10]
                Reserved            Reserved for future Default blank               CHAR[100]

             */

            MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }
            char[] cScripCode;
            public string Prop03ScripCode
            {
                get { return new string(cScripCode); }
                set { cScripCode = CUtility.GetPreciseArrayForString(value, cScripCode.Length); }
            }
            char[] cReserved;
            public string Prop04Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public MarketDepthRequest(bool b)
            {
                cHeader = new MessageHeader(1);
                cExchangeCode = new char[5];
                cScripCode = new char[10];
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                byte[] DepthRequest = new byte[CConstants.MarketDepthRequestSize];
                try
                {
                    Buffer.BlockCopy(cHeader.StructToByte(), 0, DepthRequest, 0, 6);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cExchangeCode), 0, DepthRequest, 6, 5);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cScripCode), 0, DepthRequest, 11, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, DepthRequest, 21, 100);
                    return DepthRequest;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return DepthRequest;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                cHeader = new MessageHeader(1);
                cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                Prop01Header = cHeader;
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 6, 5);
                Prop03ScripCode = Encoding.ASCII.GetString(ByteStructure, 11, 10);
                Prop04Reserved = Encoding.ASCII.GetString(ByteStructure, 21, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "ExchangeCode = " + Prop02ExchangeCode + "|" + "ScripCode = " + Prop03ScripCode + "|" + "Reserved = " + Prop04Reserved;
            }
        }

        public struct MarketDepthResponse : IStruct
        {
            /*MessageHeader
                Exchange Code       Code for each exchange Refer Exchange Code          CHAR[5]
                Exchange            Name Of The Exchange                                CHAR[10]
                LastTradedTime      Last Traded Time                                    CHAR[25]
                Scrip Code          unique token number for each scrip                  CHAR[10]
                TotalBuyQuantity    Total Buy Quantity Default 0                        LONG
                TotSellQuantity     Total Sell Quantity Default 0                       LONG
                BuyPrice1           Buy Price Default 0                                 LONG
                BuyQuantity1        Buy Quantity Default 0                              LONG
                BuyNumberOfOrder1   Total Number Of orders Default 0                    LONG
                BuyPrice2           Buy Price Default 0                                 LONG
                BuyQuantity2        Buy Quantity Default 0                              LONG
                BuyNumberOfOrder2   Total Number Of orders Default 0                    LONG
                BuyPrice3           Buy Price Default 0                                 LONG
                BuyQuantity3        Buy Quantity Default 0                              LONG
                BuyNumberOfOrder3   Total Number Of orders Default 0                    LONG
                BuyPrice4           Buy Price Default 0                                 LONG
                BuyQuantity4        Buy Quantity default 0                              LONG
                BuyNumberOfOrder4   Total Number Of orders Default 0                    LONG
                BuyPrice5           Buy Price Default 0                                 LONG
                BuyQuantity5        Buy Quantity Default 0                              LONG
                BuyNumberOfOrder5   Total Number Of orders Default 0                    LONG
                SellPrice1          Sell Price Default 0                                LONG
                SellQuantity1       Sell Quantity Default 0                             LONG
                SellNumberOfOrder1  Total Number Of orders Default 0                    LONG
                SellPrice2          Sell Price Default 0                                LONG
                SellQuantity2       Sell Quantity Default 0                             LONG
                SellNumberOfOrder2  Total Number Of orders  Default 0                   LONG
                SellPrice3          Sell Price Default 0                                LONG
                SellQuantity3       Sell Quantity Default 0                             LONG
                SellNumberOfOrder3  Total Number Of orders Default 0                    LONG
                SellPrice4          sell Price Default 0                                LONG
                SellQuantity4       Sell Quantity Default 0                             LONG
                SellNumberOfOrder4  Total Number Of orders Default 0                    LONG
                SellPrice5          Sell Price Default 0                                LONG
                SellQuantity5       Sell Quantity Default 0                             LONG
                SellNumberOfOrder5  Total Number Of orders Default 0                    LONG
              * Reserved            Reserved for future Default blank                   CHAR[100]

             * 
             */

            MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }
            char[] cExchange;
            public string Prop03Exchange
            {
                get { return new string(cExchange); }
                set { cExchange = CUtility.GetPreciseArrayForString(value, cExchange.Length); }
            }
            char[] cLastTradedTime;
            public string Prop04LastTradedTime
            {
                get { return new string(cLastTradedTime); }
                set { cLastTradedTime = CUtility.GetPreciseArrayForString(value, cLastTradedTime.Length); }
            }
            char[] cScripCode;
            public string Prop05ScripCode
            {
                get { return new string(cScripCode); }
                set { cScripCode = CUtility.GetPreciseArrayForString(value, cScripCode.Length); }
            }
            Int32 cTotalBuyQuantity;
            public Int32 Prop06TotalBuyQuantity
            {
                get { return cTotalBuyQuantity; }
                set { cTotalBuyQuantity = value; }
            }
            Int32 cTotSellQuantity;
            public Int32 Prop07TotSellQuantity
            {
                get { return cTotSellQuantity; }
                set { cTotSellQuantity = value; }
            }
            Int32 cBuyPrice1;
            public Int32 Prop08BuyPrice1
            {
                get { return cBuyPrice1; }
                set { cBuyPrice1 = value; }
            }
            Int32 cBuyQuantity1;
            public Int32 Prop09BuyQuantity1
            {
                get { return cBuyQuantity1; }
                set { cBuyQuantity1 = value; }
            }
            Int32 cBuyNumberOfOrder1;
            public Int32 Prop10BuyNumberOfOrder1
            {
                get { return cBuyNumberOfOrder1; }
                set { cBuyNumberOfOrder1 = value; }
            }
            Int32 cBuyPrice2;
            public Int32 Prop11BuyPrice2
            {
                get { return cBuyPrice2; }
                set { cBuyPrice2 = value; }
            }
            Int32 cBuyQuantity2;
            public Int32 Prop12BuyQuantity2
            {
                get { return cBuyQuantity2; }
                set { cBuyQuantity2 = value; }
            }
            Int32 cBuyNumberOfOrder2;
            public Int32 Prop13BuyNumberOfOrder2
            {
                get { return cBuyNumberOfOrder2; }
                set { cBuyNumberOfOrder2 = value; }
            }
            Int32 cBuyPrice3;
            public Int32 Prop14BuyPrice3
            {
                get { return cBuyPrice3; }
                set { cBuyPrice3 = value; }
            }
            Int32 cBuyQuantity3;
            public Int32 Prop15BuyQuantity3
            {
                get { return cBuyQuantity3; }
                set { cBuyQuantity3 = value; }
            }
            Int32 cBuyNumberOfOrder3;
            public Int32 Prop16BuyNumberOfOrder3
            {
                get { return cBuyNumberOfOrder3; }
                set { cBuyNumberOfOrder3 = value; }
            }
            Int32 cBuyPrice4;
            public Int32 Prop17BuyPrice4
            {
                get { return cBuyPrice4; }
                set { cBuyPrice4 = value; }
            }
            Int32 cBuyQuantity4;
            public Int32 Prop18BuyQuantity4
            {
                get { return cBuyQuantity4; }
                set { cBuyQuantity4 = value; }
            }
            Int32 cBuyNumberOfOrder4;
            public Int32 Prop19BuyNumberOfOrder4
            {
                get { return cBuyNumberOfOrder4; }
                set { cBuyNumberOfOrder4 = value; }
            }
            Int32 cBuyPrice5;
            public Int32 Prop20BuyPrice5
            {
                get { return cBuyPrice5; }
                set { cBuyPrice5 = value; }
            }
            Int32 cBuyQuantity5;
            public Int32 Prop21BuyQuantity5
            {
                get { return cBuyQuantity5; }
                set { cBuyQuantity5 = value; }
            }
            Int32 cBuyNumberOfOrder5;
            public Int32 Prop22BuyNumberOfOrder5
            {
                get { return cBuyNumberOfOrder5; }
                set { cBuyNumberOfOrder5 = value; }
            }
            Int32 cSellPrice1;
            public Int32 Prop23SellPrice1
            {
                get { return cSellPrice1; }
                set { cSellPrice1 = value; }
            }
            Int32 cSellQuantity1;
            public Int32 Prop24SellQuantity1
            {
                get { return cSellQuantity1; }
                set { cSellQuantity1 = value; }
            }
            Int32 cSellNumberOfOrder1;
            public Int32 Prop25SellNumberOfOrder1
            {
                get { return cSellNumberOfOrder1; }
                set { cSellNumberOfOrder1 = value; }
            }
            Int32 cSellPrice2;
            public Int32 Prop26SellPrice2
            {
                get { return cSellPrice2; }
                set { cSellPrice2 = value; }
            }
            Int32 cSellQuantity2;
            public Int32 Prop27SellQuantity2
            {
                get { return cSellQuantity2; }
                set { cSellQuantity2 = value; }
            }
            Int32 cSellNumberOfOrder2;
            public Int32 Prop28SellNumberOfOrder2
            {
                get { return cSellNumberOfOrder2; }
                set { cSellNumberOfOrder2 = value; }
            }
            Int32 cSellPrice3;
            public Int32 Prop29SellPrice3
            {
                get { return cSellPrice3; }
                set { cSellPrice3 = value; }
            }
            Int32 cSellQuantity3;
            public Int32 Prop30SellQuantity3
            {
                get { return cSellQuantity3; }
                set { cSellQuantity3 = value; }
            }
            Int32 cSellNumberOfOrder3;
            public Int32 Prop31SellNumberOfOrder3
            {
                get { return cSellNumberOfOrder3; }
                set { cSellNumberOfOrder3 = value; }
            }
            Int32 cSellPrice4;
            public Int32 Prop32SellPrice4
            {
                get { return cSellPrice4; }
                set { cSellPrice4 = value; }
            }
            Int32 cSellQuantity4;
            public Int32 Prop33SellQuantity4
            {
                get { return cSellQuantity4; }
                set { cSellQuantity4 = value; }
            }
            Int32 cSellNumberOfOrder4;
            public Int32 Prop34SellNumberOfOrder4
            {
                get { return cSellNumberOfOrder4; }
                set { cSellNumberOfOrder4 = value; }
            }
            Int32 cSellPrice5;
            public Int32 Prop35SellPrice5
            {
                get { return cSellPrice5; }
                set { cSellPrice5 = value; }
            }
            Int32 cSellQuantity5;
            public Int32 Prop36SellQuantity5
            {
                get { return cSellQuantity5; }
                set { cSellQuantity5 = value; }

            }
            Int32 cSellNumberOfOrder5;
            public Int32 Prop37SellNumberOfOrder5
            {
                get { return cSellNumberOfOrder5; }
                set { cSellNumberOfOrder5 = value; }
            }
            char[] cReserved;
            public string Prop38Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public MarketDepthResponse(bool b)
            {
                cHeader = new MessageHeader(0);
                cExchangeCode = new char[5];
                cExchange = new char[10];
                cLastTradedTime = new char[25];
                cScripCode = new char[10];
                cTotalBuyQuantity = 0;
                cTotSellQuantity = 0;
                cBuyPrice1 = 0;
                cBuyQuantity1 = 0;
                cBuyNumberOfOrder1 = 0;
                cBuyPrice2 = 0;
                cBuyQuantity2 = 0;
                cBuyNumberOfOrder2 = 0;
                cBuyPrice3 = 0;
                cBuyQuantity3 = 0;
                cBuyNumberOfOrder3 = 0;
                cBuyPrice4 = 0;
                cBuyQuantity4 = 0;
                cBuyNumberOfOrder4 = 0;
                cBuyPrice5 = 0;
                cBuyQuantity5 = 0;
                cBuyNumberOfOrder5 = 0;
                cSellPrice1 = 0;
                cSellQuantity1 = 0;
                cSellNumberOfOrder1 = 0;
                cSellPrice2 = 0;
                cSellQuantity2 = 0;
                cSellNumberOfOrder2 = 0;
                cSellPrice3 = 0;
                cSellQuantity3 = 0;
                cSellNumberOfOrder3 = 0;
                cSellPrice4 = 0;
                cSellQuantity4 = 0;
                cSellNumberOfOrder4 = 0;
                cSellPrice5 = 0;
                cSellQuantity5 = 0;
                cSellNumberOfOrder5 = 0;
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                cHeader = new MessageHeader(1);
                cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                Prop01Header = cHeader;
                Prop01Header.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 6, 5);
                Prop03Exchange = Encoding.ASCII.GetString(ByteStructure, 11, 10);
                Prop04LastTradedTime = Encoding.ASCII.GetString(ByteStructure, 21, 25);
                Prop05ScripCode = Encoding.ASCII.GetString(ByteStructure, 46, 10);
                Prop06TotalBuyQuantity = BitConverter.ToInt32(ByteStructure, 56);
                Prop07TotSellQuantity = BitConverter.ToInt32(ByteStructure, 60);
                Prop08BuyPrice1 = BitConverter.ToInt32(ByteStructure, 64);
                Prop09BuyQuantity1 = BitConverter.ToInt32(ByteStructure, 68);
                Prop10BuyNumberOfOrder1 = BitConverter.ToInt32(ByteStructure, 72);
                Prop11BuyPrice2 = BitConverter.ToInt32(ByteStructure, 76);
                Prop12BuyQuantity2 = BitConverter.ToInt32(ByteStructure, 80);
                Prop13BuyNumberOfOrder2 = BitConverter.ToInt32(ByteStructure, 84);
                Prop14BuyPrice3 = BitConverter.ToInt32(ByteStructure, 88);
                Prop15BuyQuantity3 = BitConverter.ToInt32(ByteStructure, 92);
                Prop16BuyNumberOfOrder3 = BitConverter.ToInt32(ByteStructure, 96);
                Prop17BuyPrice4 = BitConverter.ToInt32(ByteStructure, 100);
                Prop18BuyQuantity4 = BitConverter.ToInt32(ByteStructure, 104);
                Prop19BuyNumberOfOrder4 = BitConverter.ToInt32(ByteStructure, 108);
                Prop20BuyPrice5 = BitConverter.ToInt32(ByteStructure, 112);
                Prop21BuyQuantity5 = BitConverter.ToInt32(ByteStructure, 116);
                Prop22BuyNumberOfOrder5 = BitConverter.ToInt32(ByteStructure, 120);
                Prop23SellPrice1 = BitConverter.ToInt32(ByteStructure, 124);
                Prop24SellQuantity1 = BitConverter.ToInt32(ByteStructure, 128);
                Prop25SellNumberOfOrder1 = BitConverter.ToInt32(ByteStructure, 132);
                Prop26SellPrice2 = BitConverter.ToInt32(ByteStructure, 136);
                Prop27SellQuantity2 = BitConverter.ToInt32(ByteStructure, 140);
                Prop28SellNumberOfOrder2 = BitConverter.ToInt32(ByteStructure, 144);
                Prop29SellPrice3 = BitConverter.ToInt32(ByteStructure, 148);
                Prop30SellQuantity3 = BitConverter.ToInt32(ByteStructure, 152);
                Prop31SellNumberOfOrder3 = BitConverter.ToInt32(ByteStructure, 156);
                Prop32SellPrice4 = BitConverter.ToInt32(ByteStructure, 160);
                Prop33SellQuantity4 = BitConverter.ToInt32(ByteStructure, 164);
                Prop34SellNumberOfOrder4 = BitConverter.ToInt32(ByteStructure, 168);
                Prop35SellPrice5 = BitConverter.ToInt32(ByteStructure, 172);
                Prop36SellQuantity5 = BitConverter.ToInt32(ByteStructure, 176);
                Prop37SellNumberOfOrder5 = BitConverter.ToInt32(ByteStructure, 180);
                Prop38Reserved = Encoding.ASCII.GetString(ByteStructure, 184, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "ExchangeCode = " + Prop02ExchangeCode + "|" + "Exchange = " + Prop03Exchange + "|" + "LastTradedTime = " + Prop04LastTradedTime + "|" + "ScripCode = " + Prop05ScripCode + "|" + "TotalBuyQuantity  = " + Prop06TotalBuyQuantity + "|" + "TotSellQuantity = " + Prop07TotSellQuantity + "|" + "BuyPrice1 = " + Prop08BuyPrice1
                    + "|" + "BuyQuantity1 = " + Prop09BuyQuantity1 + "|" + "BuyNumberOfOrder1 = " + Prop10BuyNumberOfOrder1 + "|" + "Reserved = " + Prop11BuyPrice2 + "|" + "BuyQuantity2 = "
                    + Prop12BuyQuantity2 + "|" + "BuyNumberOfOrder2 = " + Prop13BuyNumberOfOrder2 + "|" + "BuyPrice3 = " + Prop14BuyPrice3 + "|" + "BuyQuantity3 = " + Prop15BuyQuantity3 + "|" + "BuyNumberOfOrder3 = " + Prop16BuyNumberOfOrder3 + "|" + "BuyPrice4 = " + Prop17BuyPrice4 + "|" + "BuyQuantity4 = " + Prop18BuyQuantity4 + "|" + "BuyNumberOfOrder4 = " + Prop19BuyNumberOfOrder4 + "|" + "BuyPrice5 = " + Prop20BuyPrice5 + "|" + "BuyQuantity5 = " + Prop21BuyQuantity5
                    + "|" + "BuyNumberOfOrder5 = " + Prop22BuyNumberOfOrder5 + "|" + "SellPrice1 = " + Prop23SellPrice1 + "|" + "SellQuantity1 = " + Prop24SellQuantity1 + "|" + "SellNumberOfOrder1 = " + Prop25SellNumberOfOrder1 + Prop26SellPrice2 + "|" + "SellQuantity2 = " + Prop27SellQuantity2 + "|" + "NumberOfOrder2 = " + Prop28SellNumberOfOrder2 + "|" + "SellPrice3  = " + Prop29SellPrice3 + "|" + "Quantity3 = " + Prop30SellQuantity3
                    + "|" + "SellNumberOfOrder3 = " + Prop31SellNumberOfOrder3 + "|" + "SellPrice4 = " + Prop32SellPrice4 + "|" + "SellQuantity4 = " + Prop33SellQuantity4 + "|" + "SellNumberOfOrder4 = " + Prop34SellNumberOfOrder4 + "|" + "SellPrice5 = " + Prop35SellPrice5
                    + "|" + "SellQuantity5 = " + Prop36SellQuantity5 + "|" + "SellNumberOfOrder5 = " + Prop37SellNumberOfOrder5 + "|" + "Reserved = " + Prop38Reserved;
            }
        }
        #endregion

        #region Order Request & Response

        public struct OrderItem : IStruct
        {
            /*
                * DataLength                                                                                    LONG
                OrderID         Represents unique order id generated for each order generated by sharekhan.
                                Default Blank for New Order                                                  CHAR[20]
                CustomerID      An Id of customer who has currently logged in and placed bulk order.         CHAR[10]
                S2KID           Unique sharekhan Id generated by sharekhan for each customer                 CHAR[10]
                ScripToken      unique token number for each scrip                                           CHAR[10]
                BuySell         User needs to specify whether the buying/selling order needs to be placed
                                Ex: B, S, SAM, BM, SM.                                                       CHAR[3]
                OrderQty        Represents number of shares entered by customer of particular company while 
                                placing order For Modification of Partly Executed Orders Quantity has to be 
                                (OrderQuantity – Executed Qty) for Partly Executed Orders.                   LONG
                OrderPrice      If order type is market then price = 0 otherwise price=user defined price.   LONG
                TriggerPrice    Price at which order will get triggered and ready for Execution.
                                [Optional] Default Blank.                                                    LONG
                DisclosedQty    Represents quantity that is to be disclosed to public for that order
                                [Optional] Default 0.                                                        LONG
                ExecutedQty     Represents quantity at which actual transaction got executed. 
                                Required for Modification and cancellation of Partly Executed orders.
                                [Optional] Default 0.                                                        LONG
                    RMSCode        Type of RMS Code                                                             CHAR[15]
                ExecutedPrice   Average Price of Partly Executed Quantity. 
                                Required for Modification and cancellation of Partly Executed orders.
                                    [Optional] Default 0.                                                       LONG

                AfterHour       After market hours Y-After Hour, N-Normal                                    CHAR[1]
                GTDFlag         Represents the status of Order Validation in duration.
                                Default: Compulsory for All Exchanges Ex: IOC/GFD/GTD/GTC                    CHAR[5]
                GTD             Default: Blank for Cash and FNO compulsory for Commodity
                                Ex: DD/MM/YYYY	                                                             CHAR[25]
                Reserved        Reserved for future Default blank                                            CHAR[100]

                * 
                */

            Int32 cDataLength;
            public Int32 Prop01DataLength
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }
            char[] cOrderID;
            public string Prop02OrderID
            {
                get { return new string(cOrderID); }
                set { cOrderID = CUtility.GetPreciseArrayForString(value, cOrderID.Length); }
            }

            char[] cCustomerID;
            public string Prop03CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value, cCustomerID.Length); }
            }

            char[] cS2KID;
            public string Prop04S2KID
            {
                get { return new string(cS2KID); }
                set { cS2KID = CUtility.GetPreciseArrayForString(value, cS2KID.Length); }
            }

            char[] cScripToken;
            public string Prop05ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value, cScripToken.Length); }
            }

            char[] cBuySell;
            public string Prop06BuySell
            {
                get { return new string(cBuySell); }
                set { cBuySell = CUtility.GetPreciseArrayForString(value, cBuySell.Length); }
            }

            Int32 cOrderQty;
            public Int32 Prop07OrderQty
            {
                get { return cOrderQty; }
                set { cOrderQty = value; }
            }

            Int32 cOrderPrice;
            public Int32 Prop08OrderPrice
            {
                get { return cOrderPrice; }
                set { cOrderPrice = value; }
            }

            Int32 cTriggerPrice;
            public Int32 Prop09TriggerPrice
            {
                get { return cTriggerPrice; }
                set { cTriggerPrice = value; }
            }

            Int32 cDisclosedQty;
            public Int32 Prop10DisclosedQty
            {
                get { return cDisclosedQty; }
                set { cDisclosedQty = value; }
            }

            Int32 cExecutedQty;
            public Int32 Prop11ExecutedQty
            {
                get { return cExecutedQty; }
                set { cExecutedQty = value; }
            }

            char[] cRMSCode;
            public string Prop12RMSCode
            {
                get { return new string(cRMSCode); }
                set { cRMSCode = CUtility.GetPreciseArrayForString(value, cRMSCode.Length); }
            }


            Int32 cExecutedPrice;
            public Int32 Prop13ExecutedPrice
            {
                get { return cExecutedPrice; }
                set { cExecutedPrice = value; }
            }

            char[] cAfterHour;
            public string Prop14AfterHour
            {
                get { return new string(cAfterHour); }
                set { cAfterHour = CUtility.GetPreciseArrayForString(value, cAfterHour.Length); }
            }

            char[] cGTDFlag;
            public string Prop15GTDFlag
            {
                get { return new string(cGTDFlag); }
                set { cGTDFlag = CUtility.GetPreciseArrayForString(value, cGTDFlag.Length); }
            }
            char[] cGTD;
            public string Prop16GTD
            {
                get { return new string(cGTD); }
                set { cGTD = CUtility.GetPreciseArrayForString(value, cGTD.Length); }
            }
            char[] cReserved;
            public string Prop17Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public OrderItem(bool b)
            {
                cDataLength = 0;
                cOrderID = new char[20];
                cCustomerID = new char[10];
                cS2KID = new char[10];
                cScripToken = new char[10];
                cBuySell = new char[3];
                cOrderQty = 0;
                cOrderPrice = 0;
                cTriggerPrice = 0;
                cDisclosedQty = 0;
                cExecutedQty = 0;
                cRMSCode = new char[15];
                cExecutedPrice = 0;
                cAfterHour = new char[1];
                cGTDFlag = new char[5];
                cGTD = new char[25];
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                byte[] orderItem = new byte[CConstants.OrderItemSize];
                try
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(cDataLength), 0, orderItem, 0, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cOrderID), 0, orderItem, 4, 20);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cCustomerID), 0, orderItem, 24, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cS2KID), 0, orderItem, 34, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cScripToken), 0, orderItem, 44, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cBuySell), 0, orderItem, 54, 3);
                    Buffer.BlockCopy(BitConverter.GetBytes(cOrderQty), 0, orderItem, 57, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(cOrderPrice), 0, orderItem, 61, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(cTriggerPrice), 0, orderItem, 65, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(cDisclosedQty), 0, orderItem, 69, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(cExecutedQty), 0, orderItem, 73, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cRMSCode), 0, orderItem, 77, 15);
                    Buffer.BlockCopy(BitConverter.GetBytes(cExecutedPrice), 0, orderItem, 92, 4);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cAfterHour), 0, orderItem, 96, 1);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cGTDFlag), 0, orderItem, 97, 5);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cGTD), 0, orderItem, 102, 25);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, orderItem, 127, 100);
                    return orderItem;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return orderItem;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return Prop01DataLength + "|" + "OrderID = " + Prop02OrderID + "|" + "CustomerID = " + Prop03CustomerID + "|" + "S2KID = " + Prop04S2KID + "|" + "scripToken = " + Prop05ScripToken + "|" + "BuySell = " + Prop06BuySell + "|" + "OrderQty = " + Prop07OrderQty + "|" + "OrderPrice = " + Prop08OrderPrice
                    + "|" + "TriggerPrice  = " + Prop09TriggerPrice + "|" + "DisclosedQty = " + Prop10DisclosedQty + "|" + "ExecutedQty = " + Prop11ExecutedQty + "|" + "RMSCode = "
                    + Prop12RMSCode + "|" + "ExecutedPrice = " + Prop13ExecutedPrice + "|" + "AfterHour = " + Prop14AfterHour + "|" + "GTDFlag = " + Prop15GTDFlag + "|" + "GTD = " + Prop16GTD + "|" + "Reserved = " + Prop17Reserved;
            }
        }

        public struct OrderRequest : IStruct
        {
            /*
             * MessageHeader
                RequestID       User defined Id for Each Request. 
                                This can be used to identify each order request separately by the client.   CHAR[10]
                OrderCount      Represents Number of orders in the request                                  SHORT
                ExchangeCode    Code for each exchange Refer Exchange Code                                  CHAR[2]
                OrderType1      OrderTypes specifies the type of orders Ex: NEW/ MODIFY/ CANCEL             CHAR[10]
                OrderItems      Contains the list of order items Refer OrderItems                           structure given below
                                 Structure. Refer Respective Structure given below                          List<OrderItem>
                Reserved Reserved for future Default blank                                                  CHAR[100]

             * 
             */

            MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }

            char[] cRequestID;
            public string Prop02RequestID
            {
                get { return new string(cRequestID); }
                set { cRequestID = CUtility.GetPreciseArrayForString(value, cRequestID.Length); }
            }

            Int16 cOrderCount;
            public Int16 Prop03OrderCount
            {
                get { return cOrderCount; }
                set { cOrderCount = value; }
            }

            char[] cExchangeCode;
            public string Prop04ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }
            char[] cOrderType1;
            public string Prop05OrderType1
            {
                get { return new string(cOrderType1); }
                set { cOrderType1 = CUtility.GetPreciseArrayForString(value, cOrderType1.Length); }
            }

            List<OrderItem> clstOrderItems;
            public List<OrderItem> Prop06OrderItems
            {
                get { return clstOrderItems; }
                set { clstOrderItems = value; }
            }

            char[] cReserved;
            public string Prop07Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public OrderRequest(bool b)
            {
                cHeader = new MessageHeader(1);
                cRequestID = new char[10];
                cOrderCount = 0;
                cExchangeCode = new char[2];
                cOrderType1 = new char[10];
                clstOrderItems = new List<OrderItem>();
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                byte[] OrderRequest = new byte[357];
                try
                {
                    Buffer.BlockCopy(cHeader.StructToByte(), 0, OrderRequest, 0, 6);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cRequestID), 0, OrderRequest, 6, 10);
                    Buffer.BlockCopy(BitConverter.GetBytes(cOrderCount), 0, OrderRequest, 16, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cExchangeCode), 0, OrderRequest, 18, 2);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cOrderType1), 0, OrderRequest, 20, 10);

                    int startIndex = 30;
                    foreach (OrderItem item in clstOrderItems)
                    {
                        Buffer.BlockCopy(item.StructToByte(), 0, OrderRequest, startIndex, 227);
                        startIndex += 227;
                    }

                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, OrderRequest, startIndex, 100);
                    return OrderRequest;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return OrderRequest;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "RequestID = " + Prop02RequestID + "|" + "OrderCount = " + Prop03OrderCount + "|" + "ExchangeCode = " + Prop04ExchangeCode + "|" + "OrderType1 = " + Prop05OrderType1 + "|" + "OrderItems = " + Prop06OrderItems + "|" + "Reserved = " + Prop07Reserved;
            }
        }

        #region Order Confirmation

        public struct SharekhanOrderConfirmation : IStruct
        {
            /*
             * MessageHeader
                RequestID                User defined Id for Each Request.                  CHAR[10]
                ExchangeCode             Code for each exchange Refer Exchange Code         CHAR[2]
                Count                    No of Orders placed                                SHORT
                OrderConfirmationItems  It consists list of order response                  List<OrderConfirmationItem>
                Reserved                Reserved for future Default is blank                CHAR[100]

             */

            public MessageHeader cHeader;
            /// <summary>
            /// Message Header of 6 bytes
            /// </summary>
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            char[] cRequestID;
            /// <summary>
            /// User defined Id for Each Request.
            /// </summary>
            public string Prop02RequestID
            {
                get { return new string(cRequestID); }
                set { cRequestID = CUtility.GetPreciseArrayForString(value, cRequestID.Length); }
            }
            char[] cExchangeCode;
            /// <summary>
            /// Code for each exchange Refer Exchange Code  
            /// </summary>
            public string Prop03ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }

            Int16 cCount;
            /// <summary>
            /// No of Orders placed       
            /// </summary>
            public Int16 Prop04Count
            {
                get { return cCount; }
                set { cCount = value; }
            }
            List<OrderConfirmationItem> cOrderConfirmationItems;
            /// <summary>
            /// It consists list of order respons
            /// </summary>
            public List<OrderConfirmationItem> Prop05OrderConfirmationItems
            {
                get { return cOrderConfirmationItems; }
                set { cOrderConfirmationItems = value; }
            }
            char[] cReserved;
            /// <summary>
            /// Reserved for future Default is blank  
            /// </summary>
            public string Prop06Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public SharekhanOrderConfirmation(bool b)
            {
                cHeader = new MessageHeader(1);
                cRequestID = new char[10];
                cExchangeCode = new char[2];
                cCount = 0;
                cOrderConfirmationItems = new List<OrderConfirmationItem>();
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                cHeader = new MessageHeader(1);
                cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                Prop01Header = cHeader;
                Prop02RequestID = Encoding.ASCII.GetString(ByteStructure, 6, 10);
                Prop03ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 16, 2);
                Prop04Count = BitConverter.ToInt16(ByteStructure, 18);
                List<OrderConfirmationItem> items = new List<OrderConfirmationItem>();
                int startIndex = 20;
                OrderConfirmationItem item;
                for (int i = 0; i < Prop04Count; i++)
                {
                    item = new OrderConfirmationItem(true);
                    byte[] orderConfirmation = new byte[459];
                    Array.ConstrainedCopy(ByteStructure, startIndex, orderConfirmation, 0, 459);
                    item.ByteToStruct(orderConfirmation);
                    startIndex += 459;
                    items.Add(item);
                }
                Prop05OrderConfirmationItems = items;
                Prop06Reserved = Encoding.ASCII.GetString(ByteStructure, 56, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "RequestID = " + Prop02RequestID + "|" + "ExchangeCode = " + Prop03ExchangeCode + "|" + "Count = " + Prop04Count + "|" + "OrderConfirmationItems = " + Prop05OrderConfirmationItems[0].ToString() + "|" + "Reserved = " + Prop06Reserved;
            }
        }

        public struct OrderConfirmationItem : IStruct
        {
            /*DataLength                                                                                         LONG
                StatusCode          Status Code representing whether order request successfully processed or not
                                        contains “ERROR” in case of error occurs while sending 
                                        or processing Bulk order request True – Success  False - Failure         CHAR[25]
                Message             Contains an error message                                                    CHAR[250]
                SharekhanOrderID    Represents unique order id generated for each order                          CHAR[20]
                OrderDateTime       Time at which sent response Format: Cash: DD/MM/YYYY HH:mm:ss
                                        Der: YYYY-MM-DD HH:mm:ss.n                                               CHAR[25]
                RMSCode             RMS server Code under which the order place
                                        Will  be used for modification and cancellation of orders.               CHAR[15]
                CoverOrderID        Child order ID in Advanced Order. Default Blank                              CHAR[20]
                Reserved            Reserved for future  Default blank                                           CHAR[100]

                * 
                */

            Int32 cDataLength;
            /// <summary>
            /// 
            /// </summary>
            public Int32 Prop01DataLength
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            char[] cStatusCode;
            /// <summary>
            /// Status Code representing whether order request successfully processed or not
            ///    contains “ERROR” in case of error occurs while sending 
            ///    or processing Bulk order request True – Success  False - Failure 
            /// </summary>
            public string Prop02StatusCode
            {
                get { return new string(cStatusCode); }
                set { cStatusCode = CUtility.GetPreciseArrayForString(value, cStatusCode.Length); }
            }
            char[] cMessage;
            /// <summary>
            /// Contains an error message     
            /// </summary>
            public string Prop03Message
            {
                get { return new string(cMessage); }
                set { cMessage = CUtility.GetPreciseArrayForString(value, cMessage.Length); }
            }
            char[] cSharekhanOrderID;
            /// <summary>
            /// Represents unique order id generated for each order  
            /// </summary>
            public string Prop04SharekhanOrderID
            {
                get { return new string(cSharekhanOrderID); }
                set { cSharekhanOrderID = CUtility.GetPreciseArrayForString(value, cSharekhanOrderID.Length); }
            }
            char[] cOrderDateTime;
            /// <summary>
            ///  Time at which sent response Format: Cash: DD/MM/YYYY HH:mm:ss
            ///  Der: YYYY-MM-DD HH:mm:ss.n 
            /// </summary>
            public string Prop05OrderDateTime
            {
                get { return new string(cOrderDateTime); }
                set { cOrderDateTime = CUtility.GetPreciseArrayForString(value, cOrderDateTime.Length); }
            }

            char[] cRMSCode;
            /// <summary>
            /// RMS server Code under which the order place
            ///     Will  be used for modification and cancellation of orders.
            /// </summary>
            public string Prop06RMSCode
            {
                get { return new string(cRMSCode); }
                set { cRMSCode = CUtility.GetPreciseArrayForString(value, cRMSCode.Length); }
            }
            char[] cCoverOrderID;
            /// <summary>
            /// Child order ID in Advanced Order. Default Blank  
            /// </summary>
            public string Prop07CoverOrderID
            {
                get { return new string(cCoverOrderID); }
                set { cCoverOrderID = CUtility.GetPreciseArrayForString(value, cCoverOrderID.Length); }
            }
            char[] cReserved;
            /// <summary>
            /// Reserved for future  Default blank   
            /// </summary>
            public string Prop08Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public OrderConfirmationItem(bool b)
            {
                cDataLength = 0;
                cStatusCode = new char[25];
                cMessage = new char[250];
                cSharekhanOrderID = new char[20];
                cOrderDateTime = new char[25];
                cRMSCode = new char[15];
                cCoverOrderID = new char[20];
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLength = BitConverter.ToInt16(ByteStructure, 0);
                Prop02StatusCode = Encoding.ASCII.GetString(ByteStructure, 2, 25);
                Prop03Message = Encoding.ASCII.GetString(ByteStructure, 27, 250);
                Prop04SharekhanOrderID = Encoding.ASCII.GetString(ByteStructure, 277, 20);
                Prop05OrderDateTime = Encoding.ASCII.GetString(ByteStructure, 297, 25);
                Prop06RMSCode = Encoding.ASCII.GetString(ByteStructure, 322, 15);
                Prop07CoverOrderID = Encoding.ASCII.GetString(ByteStructure, 337, 20);
                Prop08Reserved = Encoding.ASCII.GetString(ByteStructure, 357, 100);
            }

            public override string ToString()
            {
                return Prop01DataLength + "|" + "StatusCode  = " + Prop02StatusCode + "|" + "Message = " + Prop03Message + "|" + "SharekhanOrderID = " + Prop04SharekhanOrderID + "|" + "OrderDateTime = " + Prop05OrderDateTime + "|" + "RMSCode = " + Prop06RMSCode + "|" + "CoverOrderID = " + Prop07CoverOrderID + "|" + "Reserved = " + Prop08Reserved;
            }
        }

        public struct ExchangeTradeConfirmation : IStruct
        {
            /*MessageHeader
ExchangeCode        Code for each exchange Refer Exchange Code                          CHAR[2]

AckCode            This is the code number which specifies the exchange confirmation
                    such as pending, rejection, modification, cancel.Refer Appendix     SHORT  
MsgLength           This specifies the data length                                      SHORT 

SharekhanOrderID    Represents unique order id generated for each order                 CHAR[20]
ExchangeOrderId     Unique id for confirmation of the order.                            CHAR[20]

ExchangeDateTime    The date and time of confirmation / execution in the exchange
                    Ex: Cash : DD/MM/YYYY HH:mm:ss                                      CHAR[25]

TradeID             This is the exchange trade id for executed orders.                  CHAR[20]

CustomerId          It’s an identification number for the customer whose order is placed.CHAR[10 ]

ScripToken          unique token number for each scrip                                   CHAR[10]

BuySell             Type of order.B, S, BM, SM And SAM. Refer Abbreviation               CHAR[10]

OrderQty            Quantity for which the order was placed.                             LONG
RemainingQty        The balance of the quantity which is remaining in the order
                    which is not being executed.                                         LONG

TradeQty            The quantity which has been executed.                                LONG

DisclosedQty        Represents quantity that is to be disclosed to public for that order LONG

DisclosedRemainingQty The balance amount of  the quantity which is 
                     remaining in the disclose quantity                                  LONG

OrderPrice          If order type is market then price = 0 
                     otherwise price=user defined price.                                 LONG

TriggerPrice        Price at which order will get triggered and ready for Execution.     LONG

TradePrice          The price at which the trade occurs.                                 LONG 

ExchangeGTD         Flag indicating types of order Ex: IOC/GFD/GTD/GTC                   CHAR[5]


ExchangeGTDDate     If the order is a GTD order then it will contain GTD date
                    Default : Blank (or) Ex: DD/MM/YYYY HH:mm:ss.	                     CHAR[25]

ChannelCode         Unique Code of the terminal, in which the order was placed           CHAR[10]

Channel User        Login ID/ Admin ID who has placed order                              CHAR[30]

ErrorMessage        Contains the error message.                                          CHAR[250]

OrderTrailingPrice  Used for Advanced orders. It mentions the trailing price for the 
                    order Default 0 for normal Orders.                                   LONG

OrderTargetPrice    Used for Advanced orders.
                    It mentions the target price for trailing stoploss order
                     Default 0 for normal Orders.                                        LONG

UpperPrice          Used for Advanced Bracket Order. 
                   It denotes the upper price for target.Default 0 for normal Orders.    LONG

ChildSLPrice       Used for Advanced Bracket Order.
                   It denotes the stoploss price of the second leg.
                   Default 0 for normal Orders.                                          LONG

LowerPrice          Used for Advanced Bracket Order. It denotes the lower price for target.
                    Default 0 for normal Orders.                                         LONG             
Reserved            Reserved for future Default  blank                                   CHAR[100]


             * 
             */

            MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value, cExchangeCode.Length); }
            }

            Int16 cAckCode;
            public Int16 Prop03AckCode
            {
                get { return cAckCode; }
                set { cAckCode = value; }
            }

            Int16 cMsgLength;
            public Int16 Prop04MsgLength
            {
                get { return cMsgLength; }
                set { cMsgLength = value; }
            }
            char[] cSharekhanOrderID;
            public string Prop05SharekhanOrderID
            {
                get { return new string(cSharekhanOrderID); }
                set { cSharekhanOrderID = CUtility.GetPreciseArrayForString(value, cSharekhanOrderID.Length); }
            }
            char[] cExchangeOrderId;
            public string Prop06ExchangeOrderId
            {
                get { return new string(cExchangeOrderId); }
                set { cExchangeOrderId = CUtility.GetPreciseArrayForString(value, cExchangeOrderId.Length); }
            }
            char[] cExchangeDateTime;
            public string Prop07ExchangeDateTime
            {
                get { return new string(cExchangeDateTime); }
                set { cExchangeDateTime = CUtility.GetPreciseArrayForString(value, cExchangeDateTime.Length); }
            }


            char[] cTradeID;
            public string Prop08TradeID
            {
                get { return new string(cTradeID); }
                set { cTradeID = CUtility.GetPreciseArrayForString(value, cTradeID.Length); }
            }
            char[] cCustomerId;
            public string Prop09CustomerId
            {
                get { return new string(cCustomerId); }
                set { cCustomerId = CUtility.GetPreciseArrayForString(value, cCustomerId.Length); }
            }
            char[] cScripToken;
            public string Prop10ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value, cScripToken.Length); }
            }

            char[] cBuySell;
            public string Prop11BuySell
            {
                get { return new string(cBuySell); }
                set { cBuySell = CUtility.GetPreciseArrayForString(value, cBuySell.Length); }
            }

            Int32 cOrderQty;
            public Int32 Prop12OrderQty
            {
                get { return cOrderQty; }
                set { cOrderQty = value; }
            }

            Int32 cRemainingQty;
            public Int32 Prop13RemainingQty
            {
                get { return cRemainingQty; }
                set { cRemainingQty = value; }
            }

            Int32 cTradeQty;
            public Int32 Prop14TradeQty
            {
                get { return cTradeQty; }
                set { cTradeQty = value; }
            }
            Int32 cDisclosedQty;
            public Int32 Prop15DisclosedQty
            {
                get { return cDisclosedQty; }
                set { cDisclosedQty = value; }
            }
            Int32 cDisclosedRemainingQty;
            public Int32 Prop16DisclosedRemainingQty
            {
                get { return cDisclosedRemainingQty; }
                set { cDisclosedRemainingQty = value; }
            }
            Int32 cOrderPrice;
            public Int32 Prop17OrderPrice
            {
                get { return cOrderPrice; }
                set { cOrderPrice = value; }
            }


            Int32 cTriggerPrice;
            public Int32 Prop18TriggerPrice
            {
                get { return cTriggerPrice; }
                set { cTriggerPrice = value; }
            }
            Int32 cTradePrice;
            public Int32 Prop19TradePrice
            {
                get { return cTradePrice; }
                set { cTradePrice = value; }
            }

            char[] cExchangeGTD;
            public string Prop20ExchangeGTD
            {
                get { return new string(cExchangeGTD); }
                set { cExchangeGTD = CUtility.GetPreciseArrayForString(value, cExchangeGTD.Length); }
            }


            char[] cExchangeGTDDate;
            public string Prop21ExchangeGTDDate
            {
                get { return new string(cExchangeGTDDate); }
                set { cExchangeGTDDate = CUtility.GetPreciseArrayForString(value, cExchangeGTDDate.Length); }
            }


            char[] cChannelCode;
            public string Prop22ChannelCode
            {
                get { return new string(cChannelCode); }
                set { cChannelCode = CUtility.GetPreciseArrayForString(value, cChannelCode.Length); }
            }


            char[] cChannelUser;
            public string Prop23ChannelUser
            {
                get { return new string(cChannelUser); }
                set { cChannelUser = CUtility.GetPreciseArrayForString(value, cChannelUser.Length); }
            }
            char[] cErrorMessage;
            public string Prop24ErrorMessage
            {
                get { return new string(cErrorMessage); }
                set { cErrorMessage = CUtility.GetPreciseArrayForString(value, cErrorMessage.Length); }

            }

            Int32 cOrderTrailingPrice;
            public Int32 Prop25OrderTrailingPrice
            {
                get { return cOrderTrailingPrice; }
                set { cOrderTrailingPrice = value; }
            }


            Int32 cOrderTargetPrice;
            public Int32 Prop26OrderTargetPrice
            {
                get { return cOrderTargetPrice; }
                set { cOrderTargetPrice = value; }
            }

            Int32 cUpperPrice;
            public Int32 Prop27UpperPrice
            {
                get { return cUpperPrice; }
                set { cUpperPrice = value; }
            }

            Int32 cChildSLPrice;
            public Int32 Prop28ChildSLPrice
            {
                get { return cChildSLPrice; }
                set { cChildSLPrice = value; }
            }

            Int32 cLowerPrice;
            public Int32 Prop29LowerPrice
            {
                get { return cLowerPrice; }
                set { cLowerPrice = value; }
            }

            char[] cReserved;
            public string Prop30Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public ExchangeTradeConfirmation(bool b)
            {
                cHeader = new MessageHeader(1);
                cExchangeCode = new char[2];
                cAckCode = 0;
                cMsgLength = 0;
                cSharekhanOrderID = new char[20];
                cExchangeOrderId = new char[20];
                cExchangeDateTime = new char[25];
                cTradeID = new char[20];
                cCustomerId = new char[10];
                cScripToken = new char[10];
                cBuySell = new char[10];
                cOrderQty = 0;
                cRemainingQty = 0;
                cTradeQty = 0;
                cDisclosedQty = 0;
                cDisclosedRemainingQty = 0;
                cOrderPrice = 0;
                cTriggerPrice = 0;
                cTradePrice = 0;
                cExchangeGTD = new char[5];
                cExchangeGTDDate = new char[25];
                cChannelCode = new char[10];
                cChannelUser = new char[30];
                cErrorMessage = new char[250];
                cOrderTrailingPrice = 0;
                cOrderTargetPrice = 0;
                cUpperPrice = 0;
                cChildSLPrice = 0;
                cLowerPrice = 0;
                cReserved = new char[100];
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                cHeader = new MessageHeader(1);
                cHeader.ByteToStruct(CUtility.GetLoginHeaderFromStructure(ByteStructure));

                Prop01Header = cHeader;
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 6, 2);
                Prop03AckCode = BitConverter.ToInt16(ByteStructure, 8);
                Prop04MsgLength = BitConverter.ToInt16(ByteStructure, 10);
                Prop05SharekhanOrderID = Encoding.ASCII.GetString(ByteStructure, 12, 20);
                Prop06ExchangeOrderId = Encoding.ASCII.GetString(ByteStructure, 32, 20);
                Prop07ExchangeDateTime = Encoding.ASCII.GetString(ByteStructure, 52, 25);
                Prop08TradeID = Encoding.ASCII.GetString(ByteStructure, 77, 20);
                Prop09CustomerId = Encoding.ASCII.GetString(ByteStructure, 97, 10);
                Prop10ScripToken = Encoding.ASCII.GetString(ByteStructure, 107, 10);
                Prop11BuySell = Encoding.ASCII.GetString(ByteStructure, 117, 10);
                Prop12OrderQty = BitConverter.ToInt32(ByteStructure, 127);
                Prop13RemainingQty = BitConverter.ToInt32(ByteStructure, 131);
                Prop14TradeQty = BitConverter.ToInt32(ByteStructure, 135);
                Prop15DisclosedQty = BitConverter.ToInt32(ByteStructure, 139);
                Prop16DisclosedRemainingQty = BitConverter.ToInt32(ByteStructure, 143);
                Prop17OrderPrice = BitConverter.ToInt32(ByteStructure, 147);
                Prop18TriggerPrice = BitConverter.ToInt32(ByteStructure, 151);
                Prop19TradePrice = BitConverter.ToInt32(ByteStructure, 155);
                Prop20ExchangeGTD = Encoding.ASCII.GetString(ByteStructure, 159, 5);
                Prop21ExchangeGTDDate = Encoding.ASCII.GetString(ByteStructure, 164, 25);
                Prop22ChannelCode = Encoding.ASCII.GetString(ByteStructure, 189, 10);
                Prop23ChannelUser = Encoding.ASCII.GetString(ByteStructure, 199, 30);
                Prop24ErrorMessage = Encoding.ASCII.GetString(ByteStructure, 229, 250);
                Prop25OrderTrailingPrice = BitConverter.ToInt32(ByteStructure, 479);
                Prop26OrderTargetPrice = BitConverter.ToInt32(ByteStructure, 483);
                Prop27UpperPrice = BitConverter.ToInt32(ByteStructure, 487);
                Prop28ChildSLPrice = BitConverter.ToInt32(ByteStructure, 491);
                Prop29LowerPrice = BitConverter.ToInt32(ByteStructure, 495);
                Prop30Reserved = Encoding.ASCII.GetString(ByteStructure, 499, 100);
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "ExchangeCode  = " + Prop02ExchangeCode + "|" + "AckCode   = " + Prop03AckCode + "|" + "MsgLength  = " + Prop04MsgLength + "|" + "SharekhanOrderID = " + Prop05SharekhanOrderID + "|" + "ExchangeOrderId  = " + Prop06ExchangeOrderId + "|" + "ExchangeDateTime = " + Prop07ExchangeDateTime + "|" + "TradeID  = " + Prop08TradeID
                    + "|" + "CustomerId  = " + Prop09CustomerId + "|" + "ScripToken  = " + Prop10ScripToken + "|" + "BuySell   = " + Prop11BuySell + "|" + "OrderQty  = "
                    + Prop12OrderQty + "|" + "RemainingQty  = " + Prop13RemainingQty + "|" + "TradeQty  = " + Prop14TradeQty + "|" + "DisclosedQty  = " + Prop15DisclosedQty + "|" + "DisclosedRemainingQty  = " + Prop16DisclosedRemainingQty + "|" + "OrderPrice  = " + Prop17OrderPrice + "|" + "TriggerPrice  = " + Prop18TriggerPrice + "|" + "TradePrice  = " + Prop19TradePrice + "|" + "ExchangeGTD  = " + Prop20ExchangeGTD + "|" + "ExchangeGTDDate  = " + Prop21ExchangeGTDDate + "|" + "ChannelCode = "
                    + Prop22ChannelCode + "|" + "ChannelUser  = " + Prop23ChannelUser + "|" + "ErrorMessage  = " + Prop24ErrorMessage + "|" + "OrderTrailingPrice  = " + Prop25OrderTrailingPrice + "|" + "OrderTargetPrice  = " + Prop26OrderTargetPrice + "UpperPrice = " + Prop27UpperPrice + "|" + "ChildSLPrice  = " + Prop28ChildSLPrice + "|" + "LowerPrice  = " + Prop29LowerPrice + "|" + "Reserved  = " + Prop30Reserved;
            }
        }

        #endregion

        #endregion       

        public enum OrderReport : int
        {
            Select = 0,
            EquityOrderReportItem = 31,
            DPSRReportItem = 32,
            CashOrderDetailsReportItem = 33,
            CashTradeDetailsReportItem = 34,
            CashLimitReportItem = 35,
            CashNetPositionReportItem = 36,
            DerivativeOrderReportItem = 41,
            TurnOverReportItem = 42,
            DerivativeOrderDetailReportItem = 43,
            DerivativeTradeDetailsReportItem = 44,
            CommodityLimitReportItem = 49,
            CurrencyLimitReport = 54
        };

        #region Reports

        public struct ReportRequest : IStruct
        {
            /*
             *  MessageHeader
                Login ID        This field should contain User ID of the user/broker        CHAR[20]
                Customer ID     Customer Id of the user.                                    CHAR[10]
                Date Time       User has to select the specific date to get the order 
                                status of that day.                                         CHAR[25]
                Scrip Code      unique token number for each scrip                          CHAR[10]
                OrderId         Is Used to get the details of the specific order            CHAR[10]
                Reserved        Reserved for future                                         CHAR[100]
 
             */

            private MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            private char[] cLoginID;
            public string Prop02LoginID
            {
                get { return new string(cLoginID); }
                set { cLoginID = CUtility.GetPreciseArrayForString(value.ToString(), cLoginID.Length); }
            }

            private char[] cCustomerID;
            public string Prop03CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }

            private char[] cDateTime;
            public string Prop04DateTime
            {
                get { return new string(cDateTime); }
                set { cDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cDateTime.Length); }
            }
            private char[] cScripCode;
            public string Prop05ScripCode
            {
                get { return new string(cScripCode); }
                set { cScripCode = CUtility.GetPreciseArrayForString(value.ToString(), cScripCode.Length); }
            }
            private char[] cOrderId;
            public string Prop06OrderId
            {
                get { return new string(cOrderId); }
                set { cOrderId = CUtility.GetPreciseArrayForString(value, cOrderId.Length); }
            }
            private char[] cReserved;
            public string Prop07Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public ReportRequest(bool b)
            {
                cHeader = new MessageHeader(1);
                cLoginID = new char[20];
                cCustomerID = new char[10];
                cDateTime = new char[25];
                cScripCode = new char[10];
                cOrderId = new char[10];
                cReserved = new char[100];
            }


            public byte[] StructToByte()
            {
                byte[] ReportRequest = new byte[CConstants.ReportRequestSize];
                try
                {
                    Buffer.BlockCopy(cHeader.StructToByte(), 0, ReportRequest, 0, 6);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cLoginID), 0, ReportRequest, 6, 20);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cCustomerID), 0, ReportRequest, 26, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cDateTime), 0, ReportRequest, 36, 25);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cScripCode), 0, ReportRequest, 61, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cOrderId), 0, ReportRequest, 71, 10);
                    Buffer.BlockCopy(CUtility.GetBytesFromChar(cReserved), 0, ReportRequest, 81, 100);
                    return ReportRequest;
                }
                catch (Exception ex)
                {
                    //FnoCtclLib.Reference.LogException(ex, "SigOn Struture's StructToByte.Reason : " + ex.Message + "Trace : " + ex.StackTrace);
                    return ReportRequest;
                }
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return Prop01Header.ToString() + "|" + "LoginID = " + Prop02LoginID + "|" + "CustomerID = " + Prop03CustomerID + "|" + "DateTime = " + Prop04DateTime + "|" + "ScripCode = " + Prop05ScripCode + "|" + "OrderId = " + Prop06OrderId + "|" + "Reserved = " + Prop07Reserved;
            }
        }

        public struct ReportResponse : IStruct
        {

            /*
             *  MessageHeader
                Record Count     Number of records                                                          LONG
                ReportItems     It consists list of report response items such as
                                Equity Order Report Item, Derivative Order Report Item, 
                                DPSR Report Item,CashNetPositionReportItem, 
                                TurnOver Report Item, Cash Order Details Report Item,
                                Cash Trade Details report Item,Derivative Order Details Report Item,
                                Derivative Trade Details report Item,Cash Limit Report Item,
                                Commodity Limit report Item                                                 Structure
                Reserved        Reserved for future                                                         CHAR[100]
                
 
             */


            private MessageHeader cHeader;
            public MessageHeader Prop01Header
            {
                get { return cHeader; }
                set { cHeader = value; }
            }
            private Int32 cRecordCount;
            public Int32 Prop02RecordCount
            {
                get { return cRecordCount; }
                set { cRecordCount = value; }
            }

            //private ReportItem cReportItem;
            //public ReportItem Prop03ReportItem
            //{
            //    get { return cReportItem; }
            //    set { cReportItem = value; }
            //}


            private char[] cReserved;
            public string Prop04Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }


            public ReportResponse(bool value)
            {
                cHeader = new MessageHeader(0);
                cRecordCount = new Int32();
                //    cReportItem = new ReportItem();
                cReserved = new char[100];
            }

            #region Interface Methods

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                try
                {
                    byte[] TotalBuffer = new byte[0];

                    Prop01Header.ByteToStruct(CUtility.GetHeaderFromStructure(ByteStructure));
                    Prop02RecordCount = BitConverter.ToInt32(ByteStructure, 6);
                    //if (Prop01Header.Prop02TransactionCode == 41)
                    //{
                    //    DerivativeOrderReportItem DerivativeOrderReportItem = new DerivativeOrderReportItem(true);
                    //    DerivativeOrderReportItem.ByteToStruct(TotalBuffer);
                    //    MainWindow.logOutputQueue.EnQueue(DerivativeOrderReportItem.ToString());
                    //}
                    //Prop03ReportItem = Encoding.ASCII.GetString(ByteStructure, 10,462);
                    Prop04Reserved = Encoding.ASCII.GetString(ByteStructure, 10, 100);



                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            #endregion

            public override string ToString()
            {
                return " Header :" + Prop01Header.ToString() + "|" + "Record Count : " + Prop02RecordCount + "|" + "ReportItem  " + " " + "|" + "Reserved : + " + Prop04Reserved + "\n";
            }
        }

        public struct EquityOrderReportItem : IStruct
        {

            /*
             DataLength           Length of the Data                                   LONG
             Exchange Code        Code for each exchange                               CHAR[2]
             Order Status         It contain status of the order                       CHAR[20]
                                    (I.e. order is pending or fully executed).
             Order ID             Represents unique order id generated for             CHAR[20]
                                    each order by sharekhan.
             Exchange Order ID    Unique order id of exchange order confirmation.      CHAR[20]
             ExchangeAckDateTime    Date of that day exchange order will confirm.        CHAR[25]

             Customer ID                                                                 CHAR[10]
             S2KID                  Unique user’s id generated by share khan.            CHAR[10]
             Scrip Token            unique token number for each scrip                   CHAR[10]
             BuySell                It contain the type of  order (i.e. buy/sell)
                                    B,S,BM,SM,SS                                         CHAR[2]

            OrderQty                This field contains the number of shares entered
                                    by customer of particular company.                   LONG
            Disclosed Quantity      Indicates how many quantity users has disclosed.     LONG
            Executed Quantity       It contain the number of orders get executed.        LONG
            Order Price             If order type is market then price = 0 
                                    otherwise price=user defined price.                  LONG 
            Executed Price          From this field get the price of the 
                                    executed orders.                                     LONG
            Trigger Price           Price at which order will get triggered              LONG
            RequestStatus           It contains the status of request. 
                                    (i.e. request is new/cancel/modify).                CHAR[15]
            Date Time               Ex: YYYY-MM-DD HH:mm:ss.n                           CHAR[25]
            AfterHour               After market hours  Y , N                           CHAR[1]
            RMS code                RMS server code under which the order placed.       CHAR[15]
            GoodTill                Represents the status of GTD,GTC.
                                    Ex: IOC/GFD/GTD/GTC                                 CHAR[5]
            GoodTillDate            Ex: YYYY-MM-DD HH:mm:ss.n                           CHAR[25]
            Channel Code            It contains the type of channel.
                                    Like TT, Web, TT API.                               CHAR[10]
            Channel User            It contain the user id who has currently 
                                    logged in.                                          CHAR[20]
            OrderTrailingPrice      Used for Advanced orders. It mentions the 
                                    trailing price for the order                        LONG
            OrderTargetPrice        Used for Advanced orders. It mentions the 
                                    target price for trailing stoploss order            LONG
            UpperPrice              Used for Advanced Bracket Order. It denotes 
                                    the upper price for target.                         LONG
            LowerPrice              Used for Advanced Bracket Order. It denotes 
                                    the lower price for target.                         LONG
            BracketSLPrice          Used for Advanced Bracket Order. It 
                                    denotes the stoploss price of the second leg.       LONG
            Order_Type              It is advanced order type 
                                    i.e Bracket or TSL.                                 CHAR[25]
            TrailingStatus          It can contain any one value out of the 
                                    following Track_inprocess:order is not executed 
                                    because the price is not match with upper bound 
                                    or lower bound of the order price.
                                    Track_ Completed: price is reached any one of the  
                                    bound then status change to the track completed.     CHAR[25]
            CoverOrderID            Using this id we can identify the child order.       CHAR[25]
            UpperLowerFlag          It can contain two values:
                                    0: represents the upper bound is reached.
                                    1: Represents the lower bound is reached.            CHAR[1]
            Reserved                Reserved for future Default blank                   CHAR[100]

                
 
            */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeCode.Length); }
            }

            private char[] cOrderStatus;
            public string Prop03OrderStatus
            {
                get { return new string(cOrderStatus); }
                set { cOrderStatus = CUtility.GetPreciseArrayForString(value.ToString(), cOrderStatus.Length); }
            }

            private char[] cOrderID;
            public string Prop04OrderID
            {
                get { return new string(cOrderID); }
                set { cOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cOrderID.Length); }
            }

            private char[] cExchangeOrderID;
            public string Prop05ExchangeOrderID
            {
                get { return new string(cExchangeOrderID); }
                set { cExchangeOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeOrderID.Length); }
            }


            private char[] cExchangeAckDateTime;
            public string Prop06ExchangeAckDateTime
            {
                get { return new string(cExchangeAckDateTime); }
                set { cExchangeAckDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeAckDateTime.Length); }
            }

            private char[] cCustomerID;
            public string Prop07CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }

            private char[] cS2KID;
            public string Prop08S2KID
            {
                get { return new string(cS2KID); }
                set { cS2KID = CUtility.GetPreciseArrayForString(value.ToString(), cS2KID.Length); }
            }


            private char[] cScripToken;
            public string Prop09ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }

            private char[] cBuySell;
            public string Prop10BuySell
            {
                get { return new string(cBuySell); }
                set { cBuySell = CUtility.GetPreciseArrayForString(value.ToString(), cBuySell.Length); }
            }

            private Int32 cOrderQty;
            public Int32 Prop11OrderQty
            {
                get { return cOrderQty; }
                set { cOrderQty = value; }
            }

            private Int32 cDisclosedQuantity;
            public Int32 Prop12DisclosedQuantity
            {
                get { return cDisclosedQuantity; }
                set { cDisclosedQuantity = value; }
            }

            private Int32 cExecutedQuantity;
            public Int32 Prop13ExecutedQuantity
            {
                get { return cExecutedQuantity; }
                set { cExecutedQuantity = value; }
            }

            private Int32 cOrderPrice;
            public Int32 Prop14OrderPrice
            {
                get { return cOrderPrice; }
                set { cOrderPrice = value; }
            }

            private Int32 cExecutedPrice;
            public Int32 Prop15ExecutedPrice
            {
                get { return cExecutedPrice; }
                set { cExecutedPrice = value; }
            }

            private Int32 cTriggerPrice;
            public Int32 Prop16TriggerPrice
            {
                get { return cTriggerPrice; }
                set { cTriggerPrice = value; }
            }

            private char[] cRequestStatus;
            public string Prop17RequestStatus
            {
                get { return new string(cRequestStatus); }
                set { cRequestStatus = CUtility.GetPreciseArrayForString(value.ToString(), cRequestStatus.Length); }
            }

            private char[] cDateTime;
            public string Prop18DateTime
            {
                get { return new string(cDateTime); }
                set { cDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cDateTime.Length); }
            }

            private char[] cAfterHour;
            public string Prop19AfterHour
            {
                get { return new string(cAfterHour); }
                set { cAfterHour = CUtility.GetPreciseArrayForString(value.ToString(), cAfterHour.Length); }
            }

            private char[] cRMScode;
            public string Prop20RMScode
            {
                get { return new string(cRMScode); }
                set { cRMScode = CUtility.GetPreciseArrayForString(value.ToString(), cRMScode.Length); }
            }


            private char[] cGoodTill;
            public string Prop21GoodTill
            {
                get { return new string(cGoodTill); }
                set { cGoodTill = CUtility.GetPreciseArrayForString(value.ToString(), cGoodTill.Length); }
            }

            private char[] cGoodTillDate;
            public string Prop22GoodTillDate
            {
                get { return new string(cGoodTillDate); }
                set { cGoodTillDate = CUtility.GetPreciseArrayForString(value.ToString(), cGoodTillDate.Length); }
            }

            private char[] cChannelCode;
            public string Prop23ChannelCode
            {
                get { return new string(cChannelCode); }
                set { cChannelCode = CUtility.GetPreciseArrayForString(value.ToString(), cChannelCode.Length); }
            }

            private char[] cChannelUser;
            public string Prop24ChannelUser
            {
                get { return new string(cChannelCode); }
                set { cChannelUser = CUtility.GetPreciseArrayForString(value.ToString(), cChannelUser.Length); }
            }

            private Int32 cOrderTrailingPrice;
            public Int32 Prop25OrderTrailingPrice
            {
                get { return cOrderTrailingPrice; }
                set { cOrderTrailingPrice = value; }
            }


            private Int32 cOrderTargetPrice;
            public Int32 Prop26OrderTargetPrice
            {
                get { return cOrderTargetPrice; }
                set { cOrderTargetPrice = value; }
            }

            private Int32 cUpperPrice;
            public Int32 Prop27UpperPrice
            {
                get { return cUpperPrice; }
                set { cUpperPrice = value; }
            }

            private Int32 cLowerPrice;
            public Int32 Prop28LowerPrice
            {
                get { return cLowerPrice; }
                set { cLowerPrice = value; }
            }

            private Int32 cBracketSLPrice;
            public Int32 Prop29BracketSLPrice
            {
                get { return cBracketSLPrice; }
                set { cBracketSLPrice = value; }
            }

            private char[] cOrder_Type;
            public string Prop30Order_Type
            {
                get { return new string(cOrder_Type); }
                set { cOrder_Type = CUtility.GetPreciseArrayForString(value.ToString(), cOrder_Type.Length); }
            }

            private char[] cTrailingStatus;
            public string Prop31TrailingStatus
            {
                get { return new string(cTrailingStatus); }
                set { cTrailingStatus = CUtility.GetPreciseArrayForString(value.ToString(), cTrailingStatus.Length); }
            }

            private char[] cCoverOrderID;
            public string Prop32CoverOrderID
            {
                get { return new string(cCoverOrderID); }
                set { cCoverOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cCoverOrderID.Length); }
            }

            private char[] cUpperLowerFlag;
            public string Prop33UpperLowerFlag
            {
                get { return new string(cUpperLowerFlag); }
                set { cUpperLowerFlag = CUtility.GetPreciseArrayForString(value.ToString(), cUpperLowerFlag.Length); }
            }

            private char[] cReserved;
            public string Prop34Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToString(), cReserved.Length); }
            }


            public EquityOrderReportItem(bool value)
            {
                cDataLength = 0;
                cExchangeCode = new char[2];
                cOrderStatus = new char[20];
                cOrderID = new char[20];
                cExchangeOrderID = new char[20];
                cExchangeAckDateTime = new char[25];
                cCustomerID = new char[10];
                cS2KID = new char[10];
                cScripToken = new char[10];
                cBuySell = new char[2];
                cOrderQty = 0;
                cDisclosedQuantity = 0;
                cExecutedQuantity = 0;
                cOrderPrice = 0;
                cExecutedPrice = 0;
                cTriggerPrice = 0;
                cRequestStatus = new char[15];
                cDateTime = new char[25];
                cAfterHour = new char[1];
                cRMScode = new char[15];
                cGoodTill = new char[5];
                cGoodTillDate = new char[25];
                cChannelCode = new char[10];
                cChannelUser = new char[20];
                cOrderTrailingPrice = 0;
                cOrderTargetPrice = 0;
                cUpperPrice = 0;
                cLowerPrice = 0;
                cBracketSLPrice = 0;
                cOrder_Type = new char[25];
                cTrailingStatus = new char[25];
                cCoverOrderID = new char[25];
                cUpperLowerFlag = new char[1];
                cReserved = new char[100];


            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return Prop01DataLenght + "|" + Prop02ExchangeCode + "|" + Prop03OrderStatus + "|" + Prop04OrderID + "|" + Prop05ExchangeOrderID + "|" + Prop06ExchangeAckDateTime + "|" + Prop07CustomerID + "|" + Prop08S2KID + "|" + Prop09ScripToken + "|" + Prop10BuySell + "|" + Prop11OrderQty + "|" + Prop12DisclosedQuantity + "|" + Prop13ExecutedQuantity
                    + "|" + Prop14OrderPrice + "|" + Prop15ExecutedPrice + "|" + Prop16TriggerPrice + "|" + Prop17RequestStatus + "|" + Prop18DateTime + "|" + Prop19AfterHour + "|" + Prop20RMScode + "|" + Prop21GoodTill + "|" + Prop22GoodTillDate + "|" + Prop23ChannelCode + "|" + Prop24ChannelUser + "|" + Prop25OrderTrailingPrice + "|" + Prop26OrderTargetPrice
                    + "|" + Prop28LowerPrice + "|" + Prop29BracketSLPrice + "|" + Prop30Order_Type + "|" + Prop31TrailingStatus + "|" + Prop32CoverOrderID + "|" + Prop33UpperLowerFlag + "|" + Prop34Reserved;

            }
            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 4);
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03OrderStatus = Encoding.ASCII.GetString(ByteStructure, 6, 20);
                Prop04OrderID = Encoding.ASCII.GetString(ByteStructure, 26, 20);
                Prop05ExchangeOrderID = Encoding.ASCII.GetString(ByteStructure, 46, 20);
                Prop06ExchangeAckDateTime = Encoding.ASCII.GetString(ByteStructure, 66, 25);
                Prop07CustomerID = Encoding.ASCII.GetString(ByteStructure, 91, 10);
                Prop08S2KID = Encoding.ASCII.GetString(ByteStructure, 101, 10);
                Prop09ScripToken = Encoding.ASCII.GetString(ByteStructure, 111, 10);
                Prop10BuySell = Encoding.ASCII.GetString(ByteStructure, 121, 2);
                Prop11OrderQty = BitConverter.ToInt32(ByteStructure, 123);
                Prop12DisclosedQuantity = BitConverter.ToInt32(ByteStructure, 127);
                Prop13ExecutedQuantity = BitConverter.ToInt32(ByteStructure, 131);
                Prop14OrderPrice = BitConverter.ToInt32(ByteStructure, 135);
                Prop15ExecutedPrice = BitConverter.ToInt32(ByteStructure, 139);
                Prop16TriggerPrice = BitConverter.ToInt32(ByteStructure, 144);
                Prop17RequestStatus = Encoding.ASCII.GetString(ByteStructure, 149, 15);
                Prop18DateTime = Encoding.ASCII.GetString(ByteStructure, 164, 25);
                Prop19AfterHour = Encoding.ASCII.GetString(ByteStructure, 189, 1);
                Prop20RMScode = Encoding.ASCII.GetString(ByteStructure, 190, 15);
                Prop21GoodTill = Encoding.ASCII.GetString(ByteStructure, 205, 5);
                Prop22GoodTillDate = Encoding.ASCII.GetString(ByteStructure, 210, 25);
                Prop23ChannelCode = Encoding.ASCII.GetString(ByteStructure, 235, 10);
                Prop24ChannelUser = Encoding.ASCII.GetString(ByteStructure, 245, 20);
                Prop25OrderTrailingPrice = BitConverter.ToInt32(ByteStructure, 265);
                Prop26OrderTargetPrice = BitConverter.ToInt32(ByteStructure, 269);
                Prop27UpperPrice = BitConverter.ToInt32(ByteStructure, 274);
                Prop28LowerPrice = BitConverter.ToInt32(ByteStructure, 278);
                Prop29BracketSLPrice = BitConverter.ToInt32(ByteStructure, 282);
                Prop30Order_Type = Encoding.ASCII.GetString(ByteStructure, 286, 25);
                Prop31TrailingStatus = Encoding.ASCII.GetString(ByteStructure, 311, 25);
                Prop32CoverOrderID = Encoding.ASCII.GetString(ByteStructure, 336, 25);
                Prop33UpperLowerFlag = Encoding.ASCII.GetString(ByteStructure, 361, 1);
                Prop34Reserved = Encoding.ASCII.GetString(ByteStructure, 362, 100);

            }
        }

        public struct DerivativeOrderReportItem : IStruct
        {
            /*
             * 
            DataLength           Length of the Data                         LONG
            Exchange Code       Code for each exchange
                                Refer Exchange Code                         CHAR[2]
            Order Status        It contain status of the order 
                                (I.e. order is pending or fully executed).  CHAR[20]
            Order ID            Represents unique order id generated 
                                for each order by sharekhan.                CHAR[20]
            ExchangeOrderID     Unique order id of exchange order 
                                confirmation.                               CHAR[25]
            Customer ID                                                     CHAR[10]
            S2KID               Unique user’s id.                           CHAR[25]
            ScripToken          unique token number for each scrip          CHAR[10]
            OrderType           It can two types of orders
                                i.e Market or Limit                         CHAR[10]
            BuySell             It contain the type of  order 
                                 (i.e. buy/sell)
                                B,S,BM,SM,SS.                               CHAR[2]

           OrderQuantity        This field contains the number of 
                                shares entered by customer of particular 
                                company in multiple of minimum quantity 
                                for that particular script.
                                Total Quantity(No. Of Lot  X Lot Size )     LONG
           ExecutedQuantity     Order executed quantity                     LONG
           Order Price          If order type is market then 
                                price = 0 otherwise price=user 
                                defined price.                              LONG
          Average Price         From this field get the price of 
                                the executed orders.                        LONG
          DateTime             Ex: YYYY-MM-DD HH:mm:ss.n                    CHAR[25]
          RequestStatus         It contain status of request
                                (i.e request is new/cancel/modify).         CHAR[15]
         ChannelCode            It contains the type of channel.
                                Like TT,Web,TT API.                         CHAR[10]
        ChannelUser             It contain the user id who has 
                                currently logged in.                        CHAR[20]
        LastModTime             It represents the date-time at which
                                the order details where last modified.
                                Ex: YYYY-MM-DD HH:mm:ss.n                   CHAR[25]
        OpenQty                 It represents pending order quantity.       LONG.
        POI                     Ignore this field                           CHAR[25]
        DisclosedQuantity                                                   LONG
        MIF                     Ignore this field                           CHAR[50]
        TriggerPrice            Default 0                                   LONG
        RMSCode                 RMS server code under which 
                                the order placed.                           CHAR[15]
        AfterHour               After market hours
                                Y , N                                       CHAR[1]
        GoodTill                Represents the status of IOC, 
                                GFD, GTD and GTC.                           CHAR[5]
        GoodTillDate            Format “yyyy-MM-DD HH:mm:ss.n               CHAR[25]
        UpdateDate              It represents the date-time at which 
                                the order details where last modified.
                                Format “yyyy-MM-DD HH:mm:ss.n”              CHAR[25]
       UpdateUser               It contain our system status 
                                Like NOR=order is pending.,TC= order 
                                    executed.,COC= canceled order.          CHAR[25]
       CALevel                  Ignore this field                           CHAR[15]
       AON                      Order will get executed  either for 
                                all quantity or it will not get 
                                executed entirely.
                                All or None                                 CHAR[25]
        OPOC                    Ignore this field                           CHAR[25]
        FnoOrderType            It represents the normal order type.        CHAR[25]
        BuySellFlag             Ignore this field 
                                Y - buy.
                                N - sell.                                   CHAR[1]
        Reserved                Reserved for future
                                Default blank                               CHAR[100]
*/




            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeCode.Length); }
            }

            private char[] cOrderStatus;
            public string Prop03OrderStatus
            {
                get { return new string(cOrderStatus); }
                set { cOrderStatus = CUtility.GetPreciseArrayForString(value.ToString(), cOrderStatus.Length); }
            }

            private char[] cOrderID;
            public string Prop04OrderID
            {
                get { return new string(cOrderID); }
                set { cOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cOrderID.Length); }
            }

            private char[] cExchangeOrderID;
            public string Prop05ExchangeOrderID
            {
                get { return new string(cExchangeOrderID); }
                set { cExchangeOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeOrderID.Length); }
            }




            private char[] cCustomerID;
            public string Prop06CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }

            private char[] cS2KID;
            public string Prop07S2KID
            {
                get { return new string(cS2KID); }
                set { cS2KID = CUtility.GetPreciseArrayForString(value.ToString(), cS2KID.Length); }
            }


            private char[] cScripToken;
            public string Prop08ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }


            private char[] cOrderType;
            public string Prop09OrderType
            {
                get { return new string(cOrderType); }
                set { cOrderType = CUtility.GetPreciseArrayForString(value.ToString(), cOrderType.Length); }
            }


            private char[] cBuySell;
            public string Prop10BuySell
            {
                get { return new string(cBuySell); }
                set { cBuySell = CUtility.GetPreciseArrayForString(value.ToString(), cBuySell.Length); }
            }

            private Int32 cOrderQty;
            public Int32 Prop11OrderQty
            {
                get { return cOrderQty; }
                set { cOrderQty = value; }
            }



            private Int32 cExecutedQuantity;
            public Int32 Prop12ExecutedQuantity
            {
                get { return cExecutedQuantity; }
                set { cExecutedQuantity = value; }
            }



            private Int32 cOrderPrice;
            public Int32 Prop13OrderPrice
            {
                get { return cOrderPrice; }
                set { cOrderPrice = value; }
            }

            private Int32 cAveragePrice;
            public Int32 Prop14AveragePrice
            {
                get { return cAveragePrice; }
                set { cAveragePrice = value; }
            }


            private char[] cDateTime;
            public string Prop15DateTime
            {
                get { return new string(cDateTime); }
                set { cDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cDateTime.Length); }
            }

            private char[] cRequestStatus;
            public string Prop16RequestStatus
            {
                get { return new string(cRequestStatus); }
                set { cRequestStatus = CUtility.GetPreciseArrayForString(value.ToString(), cRequestStatus.Length); }
            }

            private char[] cChannelCode;
            public string Prop17ChannelCode
            {
                get { return new string(cChannelCode); }
                set { cChannelCode = CUtility.GetPreciseArrayForString(value.ToString(), cChannelCode.Length); }
            }

            private char[] cChannelUser;
            public string Prop18ChannelUser
            {
                get { return new string(cChannelCode); }
                set { cChannelUser = CUtility.GetPreciseArrayForString(value.ToString(), cChannelUser.Length); }
            }

            private char[] cLastModTime;
            public string Prop19LastModTime
            {
                get { return new string(cLastModTime); }
                set { cLastModTime = CUtility.GetPreciseArrayForString(value.ToString(), cLastModTime.Length); }
            }

            private Int32 cOpenQty;
            public Int32 Prop20OpenQty
            {
                get { return cOpenQty; }
                set { cOpenQty = value; }
            }

            private char[] cPOI;
            public string Prop21POI
            {
                get { return new string(cPOI); }
                set { cPOI = CUtility.GetPreciseArrayForString(value.ToString(), cPOI.Length); }
            }

            private Int32 cDisclosedQuantity;
            public Int32 Prop22DisclosedQuantity
            {
                get { return cDisclosedQuantity; }
                set { cDisclosedQuantity = value; }
            }



            private char[] cMIF;
            public string Prop23MIF
            {
                get { return new string(cMIF); }
                set { cMIF = CUtility.GetPreciseArrayForString(value.ToString(), cMIF.Length); }
            }

            private Int32 cTriggerPrice;
            public Int32 Prop24TriggerPrice
            {
                get { return cTriggerPrice; }
                set { cTriggerPrice = value; }
            }

            private char[] cRMScode;
            public string Prop25RMScode
            {
                get { return new string(cRMScode); }
                set { cRMScode = CUtility.GetPreciseArrayForString(value.ToString(), cRMScode.Length); }
            }


            private char[] cAfterHour;
            public string Prop26AfterHour
            {
                get { return new string(cAfterHour); }
                set { cAfterHour = CUtility.GetPreciseArrayForString(value.ToString(), cAfterHour.Length); }
            }



            private char[] cGoodTill;
            public string Prop27GoodTill
            {
                get { return new string(cGoodTill); }
                set { cGoodTill = CUtility.GetPreciseArrayForString(value.ToString(), cGoodTill.Length); }
            }

            private char[] cGoodTillDate;
            public string Prop28GoodTillDate
            {
                get { return new string(cGoodTillDate); }
                set { cGoodTillDate = CUtility.GetPreciseArrayForString(value.ToString(), cGoodTillDate.Length); }
            }


            private char[] cUpdateDate;
            public string Prop29UpdateDate
            {
                get { return new string(cUpdateDate); }
                set { cUpdateDate = CUtility.GetPreciseArrayForString(value.ToString(), cUpdateDate.Length); }
            }

            private char[] cUpdateUser;
            public string Prop30UpdateUser
            {
                get { return new string(cUpdateUser); }
                set { cUpdateUser = CUtility.GetPreciseArrayForString(value.ToString(), cUpdateUser.Length); }
            }

            private char[] cCALevel;
            public string Prop31CALevel
            {
                get { return new string(cCALevel); }
                set { cCALevel = CUtility.GetPreciseArrayForString(value.ToString(), cCALevel.Length); }
            }



            private char[] cAON;
            public string Prop32AON
            {
                get { return new string(cAON); }
                set { cAON = CUtility.GetPreciseArrayForString(value.ToString(), cAON.Length); }
            }

            private char[] cOPOC;
            public string Prop33OPOC
            {
                get { return new string(cOPOC); }
                set { cOPOC = CUtility.GetPreciseArrayForString(value.ToString(), cOPOC.Length); }
            }

            private char[] cFnoOrderType;
            public string Prop34FnoOrderType
            {
                get { return new string(cFnoOrderType); }
                set { cFnoOrderType = CUtility.GetPreciseArrayForString(value.ToString(), cFnoOrderType.Length); }
            }

            private char[] cBuySellFlag;
            public string Prop35BuySellFlag
            {
                get { return new string(cBuySellFlag); }
                set { cBuySellFlag = CUtility.GetPreciseArrayForString(value.ToString(), cBuySellFlag.Length); }
            }

            private char[] cReserved;
            public string Prop36Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToString(), cReserved.Length); }
            }


            public DerivativeOrderReportItem(bool value)
            {

                cDataLength = 0;
                cExchangeCode = new char[2];
                cOrderStatus = new char[20];
                cOrderID = new char[20];
                cExchangeOrderID = new char[25];
                cCustomerID = new char[10];
                cS2KID = new char[25];
                cScripToken = new char[10];
                cOrderType = new char[10];
                cBuySell = new char[2];
                cOrderQty = 0;
                cExecutedQuantity = 0;
                cOrderPrice = 0;
                cAveragePrice = 0;
                cDateTime = new char[25];
                cRequestStatus = new char[15];
                cChannelCode = new char[10];
                cChannelUser = new char[20];
                cLastModTime = new char[25];
                cOpenQty = 0;
                cPOI = new char[25]; ;
                cDisclosedQuantity = 0;
                cMIF = new char[50];
                cTriggerPrice = 0;
                cRMScode = new char[15];
                cAfterHour = new char[1];
                cGoodTill = new char[5];
                cGoodTillDate = new char[25];
                cUpdateDate = new char[25];
                cUpdateUser = new char[25];

                cCALevel = new char[15];

                cAON = new char[25];

                cOPOC = new char[25];

                cFnoOrderType = new char[25];

                cBuySellFlag = new char[1];

                cReserved = new char[100];

            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {

                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 4);
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03OrderStatus = Encoding.ASCII.GetString(ByteStructure, 6, 20);
                Prop04OrderID = Encoding.ASCII.GetString(ByteStructure, 26, 20);
                Prop05ExchangeOrderID = Encoding.ASCII.GetString(ByteStructure, 46, 25);
                Prop06CustomerID = Encoding.ASCII.GetString(ByteStructure, 71, 10);
                Prop07S2KID = Encoding.ASCII.GetString(ByteStructure, 81, 25);
                Prop08ScripToken = Encoding.ASCII.GetString(ByteStructure, 106, 10);
                Prop09OrderType = Encoding.ASCII.GetString(ByteStructure, 116, 10);
                Prop10BuySell = Encoding.ASCII.GetString(ByteStructure, 126, 2);
                Prop11OrderQty = BitConverter.ToInt32(ByteStructure, 128);
                Prop12ExecutedQuantity = BitConverter.ToInt32(ByteStructure, 132);
                Prop13OrderPrice = BitConverter.ToInt32(ByteStructure, 136);
                Prop14AveragePrice = BitConverter.ToInt32(ByteStructure, 140);
                Prop15DateTime = Encoding.ASCII.GetString(ByteStructure, 144, 25);
                Prop16RequestStatus = Encoding.ASCII.GetString(ByteStructure, 169, 15);
                Prop17ChannelCode = Encoding.ASCII.GetString(ByteStructure, 184, 10);
                Prop18ChannelUser = Encoding.ASCII.GetString(ByteStructure, 194, 20);
                Prop19LastModTime = Encoding.ASCII.GetString(ByteStructure, 214, 25);
                Prop20OpenQty = BitConverter.ToInt32(ByteStructure, 239);
                Prop21POI = Encoding.ASCII.GetString(ByteStructure, 243, 25);
                Prop22DisclosedQuantity = BitConverter.ToInt32(ByteStructure, 268);
                Prop23MIF = Encoding.ASCII.GetString(ByteStructure, 272, 50);
                Prop24TriggerPrice = BitConverter.ToInt32(ByteStructure, 322);
                Prop25RMScode = Encoding.ASCII.GetString(ByteStructure, 326, 15);
                Prop26AfterHour = Encoding.ASCII.GetString(ByteStructure, 341, 1);
                Prop27GoodTill = Encoding.ASCII.GetString(ByteStructure, 342, 5);
                Prop28GoodTillDate = Encoding.ASCII.GetString(ByteStructure, 347, 25);
                Prop29UpdateDate = Encoding.ASCII.GetString(ByteStructure, 372, 25); ;
                Prop30UpdateUser = Encoding.ASCII.GetString(ByteStructure, 397, 25);
                Prop31CALevel = Encoding.ASCII.GetString(ByteStructure, 422, 15);
                Prop32AON = Encoding.ASCII.GetString(ByteStructure, 437, 25);
                Prop33OPOC = Encoding.ASCII.GetString(ByteStructure, 462, 25);
                Prop34FnoOrderType = Encoding.ASCII.GetString(ByteStructure, 487, 25);
                Prop35BuySellFlag = Encoding.ASCII.GetString(ByteStructure, 512, 1);
                Prop36Reserved = Encoding.ASCII.GetString(ByteStructure, 513, 100);
            }

            public override string ToString()
            {

                return Prop01DataLenght + "|" + Prop02ExchangeCode + "|" + Prop03OrderStatus + "|" + Prop04OrderID + "|" + Prop05ExchangeOrderID + "|" + Prop06CustomerID + "|" + Prop07S2KID + "|" + Prop08ScripToken + "|" + Prop09OrderType + "|" + Prop10BuySell
                    + "|" + Prop11OrderQty + "|" + Prop12ExecutedQuantity + "|" + Prop13OrderPrice + "|" + Prop14AveragePrice + "|" + Prop15DateTime + "|" + Prop16RequestStatus + "|" + Prop17ChannelCode + "|" + Prop18ChannelUser + "|" + Prop19LastModTime
                + "|" + Prop20OpenQty + "|" + Prop21POI + "|" + Prop22DisclosedQuantity + "|" + Prop23MIF + "|" + Prop24TriggerPrice + "|" + Prop25RMScode + "|" + Prop26AfterHour + "|" + Prop27GoodTill + "|" + Prop28GoodTillDate + "|" + Prop29UpdateDate + "|" + Prop30UpdateUser
                + "|" + Prop31CALevel + "|" + Prop32AON + "|" + Prop33OPOC + "|" + Prop34FnoOrderType + "|" + Prop35BuySellFlag + "|" + Prop36Reserved + "\n";
            }



        }

        public struct DPSRReportItem : IStruct
        {
            /*
             DataLength        Length of the Data                        LONG    
             Exchange          NC,BC                                     CHAR[2]
             CustomerID        An id of customer who has 
                               currently logged in and placed order.     CHAR[10]
             S2KID             Unique user’s id generated by share khan. CHAR[10]
             ScripToken        It contains the name of the company of 
                               which shares are brought or sell.         CHAR[10]
             Receivable                                                  LONG
             DpMarginQty       Quantities which are moved from DP 
                               to Margin                                 LONG
             DP                                                          LONG
             Pool                                                        LONG
             MF                                                          LONG
             Pledge                                                      LONG
             InvstQty                                                    LONG
             MarginQty           MaxTrade Quantity                       LONG
             AvailableQty                                                LONG
             HoldPrice                                                   LONG
             MktPrice                                                    LONG
             MktValue                                                    LONG
             DpMarginValue                                               LONG
             Reserved           Reserved for future Default blank        CHAR[100]


             
             */


            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private char[] cExchange;
            public string Prop02Exchange
            {
                get { return new string(cExchange); }
                set { cExchange = CUtility.GetPreciseArrayForString(value.ToString(), cExchange.Length); }
            }

            private char[] cCustomerID;
            public string Prop03CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }

            private char[] cS2KID;
            public string Prop04S2KID
            {
                get { return new string(cS2KID); }
                set { cS2KID = CUtility.GetPreciseArrayForString(value.ToString(), cS2KID.Length); }
            }

            private char[] cScripToken;
            public string Prop05ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }





            private Int32 cReceivable;
            public Int32 Prop06Receivable
            {
                get { return cReceivable; }
                set { cReceivable = value; }
            }

            private Int32 cDpMarginQty;
            public Int32 Prop07DpMarginQty
            {
                get { return cDpMarginQty; }
                set { cDpMarginQty = value; }
            }

            private Int32 cDP;
            public Int32 Prop08DP
            {
                get { return cDP; }
                set { cDP = value; }
            }

            private Int32 cPool;
            public Int32 Prop09Pool
            {
                get { return cPool; }
                set { cPool = value; }
            }

            private Int32 cMF;
            public Int32 Prop10cMF
            {
                get { return cMF; }
                set { cMF = value; }
            }

            private Int32 cPledge;
            public Int32 Prop11Pledge
            {
                get { return cPledge; }
                set { cPledge = value; }
            }

            private Int32 cInvstQty;
            public Int32 Prop12InvstQty
            {
                get { return cInvstQty; }
                set { cInvstQty = value; }
            }

            private Int32 cMarginQty;
            public Int32 Prop13MarginQty
            {
                get { return cMarginQty; }
                set { cMarginQty = value; }
            }

            private Int32 cAvailableQty;
            public Int32 Prop14AvailableQty
            {
                get { return cAvailableQty; }
                set { cAvailableQty = value; }
            }

            private Int32 cHoldPrice;
            public Int32 Prop15HoldPrice
            {
                get { return cHoldPrice; }
                set { cHoldPrice = value; }
            }




            private Int32 cMktPrice;
            public Int32 Prop16MktPrice
            {
                get { return cMktPrice; }
                set { cMktPrice = value; }
            }


            private Int32 cMktValue;
            public Int32 Prop17MktValue
            {
                get { return cMktValue; }
                set { cMktValue = value; }
            }


            private Int32 cDpMarginValue;
            public Int32 Prop18cDpMarginValue
            {
                get { return cDpMarginValue; }
                set { cDpMarginValue = value; }
            }


            private char[] cReserved;
            public string Prop19Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public DPSRReportItem(bool value)
            {


                cDataLength = 0;

                cExchange = new char[2];

                cCustomerID = new char[10];

                cS2KID = new char[10];

                cScripToken = new char[10];

                cReceivable = 0;

                cDpMarginQty = 0;

                cDP = 0;

                cPool = 0;

                cMF = 0;

                cPledge = 0;

                cInvstQty = 0;

                cMarginQty = 0;

                cAvailableQty = 0;

                cHoldPrice = 0;

                cMktPrice = 0;

                cMktValue = 0;

                cDpMarginValue = 0;

                cReserved = new char[100];




            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 4);
                Prop02Exchange = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03CustomerID = Encoding.ASCII.GetString(ByteStructure, 6, 10);
                Prop04S2KID = Encoding.ASCII.GetString(ByteStructure, 16, 10);
                Prop05ScripToken = Encoding.ASCII.GetString(ByteStructure, 26, 10);
                Prop06Receivable = BitConverter.ToInt32(ByteStructure, 36);
                Prop07DpMarginQty = BitConverter.ToInt32(ByteStructure, 40);
                Prop08DP = BitConverter.ToInt32(ByteStructure, 44);
                Prop09Pool = BitConverter.ToInt32(ByteStructure, 48);
                Prop10cMF = BitConverter.ToInt32(ByteStructure, 52);
                Prop11Pledge = BitConverter.ToInt32(ByteStructure, 56);
                Prop12InvstQty = BitConverter.ToInt32(ByteStructure, 60);
                Prop13MarginQty = BitConverter.ToInt32(ByteStructure, 64);
                Prop14AvailableQty = BitConverter.ToInt32(ByteStructure, 68);
                Prop15HoldPrice = BitConverter.ToInt32(ByteStructure, 72);
                Prop16MktPrice = BitConverter.ToInt32(ByteStructure, 76);
                Prop17MktValue = BitConverter.ToInt32(ByteStructure, 80);
                Prop18cDpMarginValue = BitConverter.ToInt32(ByteStructure, 84);
                Prop19Reserved = Encoding.ASCII.GetString(ByteStructure, 88, 100);
            }

        }

        public struct CashNetPositionReportItem : IStruct
        {
            /*
             * 
            DataLength          Length of the Data                      LONG
            Exchange            Names of exchange:-NF,MX,NX
                                Refer Exchange Code                     CHAR[2]
            ScripName           This field contains name of 
                                the Company.                            CHAR[100]
            ScripToken          unique token number for each scrip      CHAR[10]
            Segment                                                     CHAR[20]
            ProductType                                                 CHAR[20]
            NetPosition                                                 LONG
            AVGRate                                                     LONG
            MKTRate                                                     LONG
            MTMP                Notional profit.                        LONG
            BookedPL            Realized profit.                        LONG
            BuyQty              It represents quantity bought today.    LONG
            AVGBuyRate                                                  LONG
            BuyValue            It is average of purchase price.        LONG
            SellQty             It represents quantity sold today.      LONG
            AVGSellRate                                                 LONG
            SellValue           It is average of sale price.            LONG
            DPQty                                                       LONG
            Reserved            Reserved for future
                                Default blank                           CHAR[100]

             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }


            private char[] cExchange;
            public string Prop02Exchange
            {
                get { return new string(cExchange); }
                set { cExchange = CUtility.GetPreciseArrayForString(value.ToString(), cExchange.Length); }
            }

            private char[] cScripName;
            public string Prop03ScripName
            {
                get { return new string(cScripName); }
                set { cScripName = CUtility.GetPreciseArrayForString(value.ToString(), cScripName.Length); }
            }

            private char[] cScripToken;
            public string Prop04ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }

            private char[] cSegment;
            public string Prop05Segment
            {
                get { return new string(cSegment); }
                set { cSegment = CUtility.GetPreciseArrayForString(value.ToString(), cSegment.Length); }
            }


            private char[] cProductType;
            public string Prop06ProductType
            {
                get { return new string(cProductType); }
                set { cProductType = CUtility.GetPreciseArrayForString(value.ToString(), cProductType.Length); }
            }


            private Int32 cNetPosition;
            public Int32 Prop07NetPosition
            {
                get { return cNetPosition; }
                set { cNetPosition = value; }
            }

            private Int32 cAVGRate;
            public Int32 Prop08AVGRate
            {
                get { return cAVGRate; }
                set { cAVGRate = value; }
            }

            private Int32 cMKTRate;
            public Int32 Prop09MKTRate
            {
                get { return cMKTRate; }
                set { cMKTRate = value; }
            }

            private Int32 cMTMP;
            public Int32 Prop10MTMP
            {
                get { return cMTMP; }
                set { cMTMP = value; }
            }

            private Int32 cBookedPL;
            public Int32 Prop11BookedPL
            {
                get { return cBookedPL; }
                set { cBookedPL = value; }
            }

            private Int32 cBuyQty;
            public Int32 Prop12BuyQty
            {
                get { return cBuyQty; }
                set { cBuyQty = value; }
            }

            private Int32 cAVGBuyRate;
            public Int32 Prop13AVGBuyRate
            {
                get { return cAVGBuyRate; }
                set { cAVGBuyRate = value; }
            }

            private Int32 cBuyValue;
            public Int32 Prop14BuyValue
            {
                get { return cBuyValue; }
                set { cBuyValue = value; }
            }

            private Int32 cSellQty;
            public Int32 Prop15SellQty
            {
                get { return cSellQty; }
                set { cSellQty = value; }
            }

            private Int32 cAVGSellRate;
            public Int32 Prop16AVGSellRate
            {
                get { return cAVGSellRate; }
                set { cAVGSellRate = value; }
            }

            private Int32 cSellValue;
            public Int32 Prop17SellValue
            {
                get { return cSellValue; }
                set { cSellValue = value; }
            }

            private Int32 cDPQty;
            public Int32 Prop18DPQty
            {
                get { return cDPQty; }
                set { cDPQty = value; }
            }

            private char[] cReserved;
            public string Prop19Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }


            public CashNetPositionReportItem(bool value)
            {
                cDataLength = 0;
                cExchange = new char[2];
                cScripName = new char[100];
                cScripToken = new char[10];
                cSegment = new char[20];
                cProductType = new char[20];
                cNetPosition = 0;
                cAVGRate = 0;
                cMKTRate = 0;
                cMTMP = 0;
                cBookedPL = 0;
                cBuyQty = 0;
                cAVGBuyRate = 0;
                cBuyValue = 0;
                cSellQty = 0;
                cAVGSellRate = 0;
                cSellValue = 0;
                cDPQty = 0;
                cReserved = new char[100];

            }


            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 4);
                Prop02Exchange = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03ScripName = Encoding.ASCII.GetString(ByteStructure, 6, 100);
                Prop04ScripToken = Encoding.ASCII.GetString(ByteStructure, 106, 10);
                Prop05Segment = Encoding.ASCII.GetString(ByteStructure, 116, 20);
                Prop06ProductType = Encoding.ASCII.GetString(ByteStructure, 136, 20);
                Prop07NetPosition = BitConverter.ToInt32(ByteStructure, 156);
                Prop08AVGRate = BitConverter.ToInt32(ByteStructure, 160);
                Prop09MKTRate = BitConverter.ToInt32(ByteStructure, 164);
                Prop10MTMP = BitConverter.ToInt32(ByteStructure, 168);
                Prop11BookedPL = BitConverter.ToInt32(ByteStructure, 172);
                Prop13AVGBuyRate = BitConverter.ToInt32(ByteStructure, 176);
                Prop14BuyValue = BitConverter.ToInt32(ByteStructure, 180);
                Prop15SellQty = BitConverter.ToInt32(ByteStructure, 184);
                Prop16AVGSellRate = BitConverter.ToInt32(ByteStructure, 188);
                Prop17SellValue = BitConverter.ToInt32(ByteStructure, 192);
                Prop18DPQty = BitConverter.ToInt32(ByteStructure, 196);
                Prop19Reserved = Encoding.ASCII.GetString(ByteStructure, 200, 100);
            }

            public override string ToString()
            {
                return Prop01DataLenght + "|" + Prop02Exchange + "|" + Prop03ScripName + "|" + Prop04ScripToken + "|" + Prop05Segment + "|" + Prop06ProductType + "|" + Prop07NetPosition
                    + "|" + Prop08AVGRate + "|" + Prop09MKTRate + "|" + Prop10MTMP + "|" + Prop11BookedPL + "|" + Prop12BuyQty + "|" + Prop12BuyQty + "|" + Prop13AVGBuyRate
                    + "|" + Prop14BuyValue + "|" + Prop15SellQty + "|" + Prop16AVGSellRate + "|" + Prop18DPQty + "|" + Prop19Reserved;
            }

        }

        public struct TurnOverReportItem : IStruct
        {
            /*
             * 
             DataLength         Length of the Data                          LONG
             Exchange           Names of exchange:-NF,MX,NX
                                Refer Exchange Code                         CHAR[2]
            CustomerID          An id of customer who has 
                                currently logged in and placed order.       CHAR[10]
            ScripToken          unique token number for each scrip          CHAR[10]
            S2KID               Unique user’s id generated by 
                                share khan.                                 CHAR[10]
            OpenQty             It represents the previous order 
                                quantity.                                   LONG

            BuyQty              It represents quantity bought today.        LONG
            SellQty             It represents quantity sold today.          LONG
            NetQty              NetQty= OpenQty + BuyQty- SellQty.          LONG
            OpeningRate         It represents the previous order 
                                quantity rate.                              LONG
            BuyRate             It is average of purchase price.            LONG
            SaleRate            It is average of sale price.                LONG
            NetRate                                                         LONG
            IntradayRate                                                    LONG
            IntradayQty                                                     LONG
            SqOffQty            SqOffQty= OpenQty+ BuyQty- SellQty          LONG
            PrevClose           Last day closing price.                     LONG
            MktPrice            It is current price.                        LONG
            MTM                 Notional profit.                            LONG
            Bpl                 Realized profit.                            LONG
            StatementDate                                                   CHAR[25]
            OpenSettMTM                                                     LONG
            NetSettledMTM                                                   LONG
            BookedSettledMTM                                                LONG
            TotalMTM            TotalMTM= MTM + OpenSettMTM                 LONG
            TotalBpl            TotalBpl= Bpl+ BookedSettledMTM             LONG
            InvstType           It is of two types:-Invest =trader 
                                will invest its own money.
                                MaxTrade=trader will invest some amount 
                                and remaining amount invested by sharekhan. CHAR[15]
            Reserved            Reserved for future. Default blank          CHAR[100]

             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }


            private char[] cExchange;
            public string Prop02Exchange
            {
                get { return new string(cExchange); }
                set { cExchange = CUtility.GetPreciseArrayForString(value.ToString(), cExchange.Length); }
            }

            private char[] cCustomerID;
            public string Prop03CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }



            private char[] cScripToken;
            public string Prop04ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }

            private char[] cS2KID;
            public string Prop05S2KID
            {
                get { return new string(cS2KID); }
                set { cS2KID = CUtility.GetPreciseArrayForString(value.ToString(), cS2KID.Length); }
            }


            private Int32 cOpenQty;
            public Int32 Prop06OpenQty
            {
                get { return cOpenQty; }
                set { cOpenQty = value; }
            }


            private Int32 cBuyQty;
            public Int32 Prop07BuyQty
            {
                get { return cBuyQty; }
                set { cBuyQty = value; }
            }

            private Int32 cSellQty;
            public Int32 Prop08SellQty
            {
                get { return cSellQty; }
                set { cSellQty = value; }
            }

            private Int32 cNetQty;
            public Int32 Prop09NetQty
            {
                get { return cNetQty; }
                set { cNetQty = value; }
            }


            private Int32 cOpeningRate;
            public Int32 Prop10OpeningRate
            {
                get { return cOpeningRate; }
                set { cOpeningRate = value; }
            }

            private Int32 cBuyRate;
            public Int32 Prop11BuyRate
            {
                get { return cBuyRate; }
                set { cBuyRate = value; }
            }

            private Int32 cSellRate;
            public Int32 Prop12SellRate
            {
                get { return cSellRate; }
                set { cSellRate = value; }
            }

            private Int32 cNetRate;
            public Int32 Prop13NetRate
            {
                get { return cNetRate; }
                set { cNetRate = value; }
            }
            private Int32 cIntradayRate;
            public Int32 Prop14IntradayRate
            {
                get { return cIntradayRate; }
                set { cIntradayRate = value; }
            }

            private Int32 cIntradayQty;
            public Int32 Prop15IntradayQty
            {
                get { return cIntradayQty; }
                set { cIntradayQty = value; }
            }



            private Int32 cSqOffQty;
            public Int32 Prop16SqOffQty
            {
                get { return cSqOffQty; }
                set { cSqOffQty = value; }
            }

            private Int32 cPrevClose;
            public Int32 Prop17PrevClose
            {
                get { return cPrevClose; }
                set { cPrevClose = value; }
            }



            private Int32 cMktPrice;
            public Int32 Prop18MktPrice
            {
                get { return cMktPrice; }
                set { cMktPrice = value; }
            }

            private Int32 cMTM;
            public Int32 Prop19MTM
            {
                get { return cMTM; }
                set { cMTM = value; }
            }

            private Int32 cBpl;
            public Int32 Prop20Bpl
            {
                get { return cBpl; }
                set { cBpl = value; }
            }

            private char[] cStatementDate;
            public string Prop21StatementDate
            {
                get { return new string(cStatementDate); }
                set { cStatementDate = CUtility.GetPreciseArrayForString(value, cStatementDate.Length); }
            }


            private Int32 cOpenSettMTM;
            public Int32 Prop22OpenSettMTM
            {
                get { return cOpenSettMTM; }
                set { cOpenSettMTM = value; }
            }

            private Int32 cNetSettledMTM;
            public Int32 Prop23NetSettledMTM
            {
                get { return cNetSettledMTM; }
                set { cNetSettledMTM = value; }
            }

            private Int32 cBookedSettledMTM;
            public Int32 Prop24BookedSettledMTM
            {
                get { return cBookedSettledMTM; }
                set { cBookedSettledMTM = value; }
            }

            private Int32 cTotalMTM;
            public Int32 Prop25TotalMTM
            {
                get { return cTotalMTM; }
                set { cTotalMTM = value; }
            }

            private Int32 cTotalBpl;
            public Int32 Prop26TotalBpl
            {
                get { return cTotalBpl; }
                set { cTotalBpl = value; }
            }

            private char[] cInvstType;
            public string Prop27InvstType
            {
                get { return new string(cInvstType); }
                set { cInvstType = CUtility.GetPreciseArrayForString(value, cInvstType.Length); }
            }


            private char[] cReserved;
            public string Prop28Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public TurnOverReportItem(bool value)
            {
                cDataLength = 0;
                cExchange = new char[2];
                cCustomerID = new char[10];
                cScripToken = new char[10];
                cS2KID = new char[10];
                cOpenQty = 0;
                cBuyQty = 0;
                cSellQty = 0;
                cNetQty = 0;
                cOpeningRate = 0;
                cBuyRate = 0;
                cSellRate = 0;
                cNetRate = 0;
                cIntradayRate = 0;
                cIntradayQty = 0;
                cSqOffQty = 0;
                cPrevClose = 0;
                cMktPrice = 0;
                cMTM = 0;
                cBpl = 0;
                cStatementDate = new char[25];
                cOpenSettMTM = 0;
                cNetSettledMTM = 0;
                cBookedSettledMTM = 0;
                cTotalMTM = 0;
                cTotalBpl = 0;
                cInvstType = new char[15];
                cReserved = new char[100];


            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 4);
                Prop02Exchange = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03CustomerID = Encoding.ASCII.GetString(ByteStructure, 6, 10);
                Prop04ScripToken = Encoding.ASCII.GetString(ByteStructure, 16, 10);
                Prop05S2KID = Encoding.ASCII.GetString(ByteStructure, 26, 10);
                Prop06OpenQty = BitConverter.ToInt32(ByteStructure, 36);
                Prop07BuyQty = BitConverter.ToInt32(ByteStructure, 40);
                Prop08SellQty = BitConverter.ToInt32(ByteStructure, 44);
                Prop09NetQty = BitConverter.ToInt32(ByteStructure, 48);
                Prop10OpeningRate = BitConverter.ToInt32(ByteStructure, 52);
                Prop11BuyRate = BitConverter.ToInt32(ByteStructure, 56);
                Prop12SellRate = BitConverter.ToInt32(ByteStructure, 60);
                Prop13NetRate = BitConverter.ToInt32(ByteStructure, 64);
                Prop14IntradayRate = BitConverter.ToInt32(ByteStructure, 68);
                Prop15IntradayQty = BitConverter.ToInt32(ByteStructure, 72);
                Prop16SqOffQty = BitConverter.ToInt32(ByteStructure, 76);
                Prop17PrevClose = BitConverter.ToInt32(ByteStructure, 80);
                Prop18MktPrice = BitConverter.ToInt32(ByteStructure, 84);
                Prop19MTM = BitConverter.ToInt32(ByteStructure, 88);
                Prop20Bpl = BitConverter.ToInt32(ByteStructure, 92);
                Prop21StatementDate = Encoding.ASCII.GetString(ByteStructure, 96, 25);
                Prop22OpenSettMTM = BitConverter.ToInt32(ByteStructure, 121);
                Prop23NetSettledMTM = BitConverter.ToInt32(ByteStructure, 125);
                Prop24BookedSettledMTM = BitConverter.ToInt32(ByteStructure, 129);
                Prop25TotalMTM = BitConverter.ToInt32(ByteStructure, 133);
                Prop26TotalBpl = BitConverter.ToInt32(ByteStructure, 137);
                Prop27InvstType = Encoding.ASCII.GetString(ByteStructure, 141, 15);
                Prop28Reserved = Encoding.ASCII.GetString(ByteStructure, 145, 100);

            }

            public override string ToString()
            {
                return Prop01DataLenght + "|" + Prop02Exchange + "|" + Prop03CustomerID + "|" + Prop04ScripToken + "|" + Prop05S2KID + "|" + Prop06OpenQty + "|" + Prop07BuyQty + "|" + Prop08SellQty + "|" + Prop09NetQty + "|" + Prop10OpeningRate
                    + "|" + Prop11BuyRate + "|" + Prop12SellRate + "|" + Prop13NetRate + "|" + Prop14IntradayRate + "|" + Prop15IntradayQty + "|" + Prop16SqOffQty + "|" + Prop17PrevClose + "|" + Prop18MktPrice + "|" + Prop19MTM + "|" + Prop20Bpl
                    + "|" + Prop21StatementDate + "|" + Prop22OpenSettMTM + "|" + Prop23NetSettledMTM + "|" + Prop24BookedSettledMTM + "|" + Prop25TotalMTM + "|" + Prop26TotalBpl + "|" + Prop27InvstType + "|" + Prop28Reserved;
            }
        }

        public struct CashOrderDetailsReportItem : IStruct
        {
            /*
             * 
             DataLength             Length of the Data                      LONG
             OrderDisplayStatus                                             CHAR[15]
             OrderID                Represents unique order id 
                                    generated for each order by sharekhan.  CHAR[20]
            ExchAckDateTime         Date of that day exchange order 
                                    will confirm. 
                                    Ex: YYYY-MM-DD HH:mm:ss.n               CHAR[25]
            OrderQty                This field contains the number of 
                                    shares entered by customer of 
                                    particular company.                     LONG
            OrderPrice              If order type is market then 
                                    price = 0 otherwise price=user 
                                    defined price.                          LONG
            OrderTriggerPrice       Price at which order will get 
                                    triggered   Default 0                   LONG
            RequestStatus           It contain status of request
                                    (i.e request is new/cancel/modify).     CHAR[15]
            OrderTrailingPrice      Used for Advanced orders. It mentions 
                                    the trailing price for the order
                                    Default 0                               LONG
            OrderTargetPrice        Used for Advanced orders. It mentions 
                                    the target price for trailing 
                                    stoploss order Default 0                LONG
            UpperPrice              Used for Advanced Bracket Order. 
                                    It denotes the upper price for target.
                                    Default 0                               LONG
            ChildSLPrice            Default 0                               LONG
            LowerPrice              Used for Advanced Bracket Order. 
                                    It denotes the lower price for target.
                                    Default 0                               LONG
            ErrorMsg                                                        CHAR[250]
            Reserved            Reserved for future Default blank           CHAR[100]

             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }


            private char[] cOrderDisplayStatus;
            public string Prop02OrderDisplayStatus
            {
                get { return new string(cOrderDisplayStatus); }
                set { cOrderDisplayStatus = CUtility.GetPreciseArrayForString(value.ToString(), cOrderDisplayStatus.Length); }
            }

            private char[] cOrderID;
            public string Prop03OrderID
            {
                get { return new string(cOrderID); }
                set { cOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cOrderID.Length); }
            }

            private char[] cExchAckDateTime;
            public string Prop04ExchAckDateTime
            {
                get { return new string(cExchAckDateTime); }
                set { cExchAckDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cExchAckDateTime.Length); }
            }



            private Int32 cOrderQty;
            public Int32 Prop05OrderQty
            {
                get { return cOrderQty; }
                set { cOrderQty = value; }
            }

            private Int32 cOrderPrice;
            public Int32 Prop06OrderPrice
            {
                get { return cOrderPrice; }
                set { cOrderPrice = value; }
            }

            private Int32 cOrderTriggerPrice;
            public Int32 Prop07OrderTriggerPrice
            {
                get { return cOrderTriggerPrice; }
                set { cOrderTriggerPrice = value; }
            }

            private char[] cRequestStatus;
            public string Prop08RequestStatus
            {
                get { return new string(cRequestStatus); }
                set { cRequestStatus = CUtility.GetPreciseArrayForString(value.ToString(), cRequestStatus.Length); }
            }

            private Int32 cOrderTrailingPrice;
            public Int32 Prop09OrderTrailingPrice
            {
                get { return cOrderTrailingPrice; }
                set { cOrderTrailingPrice = value; }
            }

            private Int32 cOrderTargetPrice;
            public Int32 Prop10OrderTargetPrice
            {
                get { return cOrderTargetPrice; }
                set { cOrderTargetPrice = value; }
            }

            private Int32 cUpperPrice;
            public Int32 Prop11UpperPrice
            {
                get { return cUpperPrice; }
                set { cUpperPrice = value; }
            }

            private Int32 cChildSLPrice;
            public Int32 Prop12ChildSLPrice
            {
                get { return cChildSLPrice; }
                set { cChildSLPrice = value; }
            }

            private Int32 cLowerPrice;
            public Int32 Prop13LowerPrice
            {
                get { return cLowerPrice; }
                set { cLowerPrice = value; }
            }



            private char[] cErrorMsg;
            public string Prop14ErrorMsg
            {
                get { return new string(cErrorMsg); }
                set { cErrorMsg = CUtility.GetPreciseArrayForString(value.ToString(), cErrorMsg.Length); }
            }

            private char[] cReserved;
            public string Prop15Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public CashOrderDetailsReportItem(bool value)
            {
                cDataLength = 0;
                cOrderDisplayStatus = new char[15];
                cOrderID = new char[20];
                cExchAckDateTime = new char[25];
                cOrderQty = 0;
                cOrderPrice = 0;
                cOrderTriggerPrice = 0;
                cRequestStatus = new char[15];
                cOrderTrailingPrice = 0;
                cOrderTargetPrice = 0;
                cUpperPrice = 0;
                cChildSLPrice = 0;
                cLowerPrice = 0;
                cErrorMsg = new char[250];
                cReserved = new char[100];

            }


            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 0);
                Prop02OrderDisplayStatus = Encoding.ASCII.GetString(ByteStructure, 4, 15);
                Prop03OrderID = Encoding.ASCII.GetString(ByteStructure, 19, 20);
                Prop04ExchAckDateTime = Encoding.ASCII.GetString(ByteStructure, 39, 25);
                Prop05OrderQty = BitConverter.ToInt32(ByteStructure, 64);
                Prop06OrderPrice = BitConverter.ToInt32(ByteStructure, 68);
                Prop07OrderTriggerPrice = BitConverter.ToInt32(ByteStructure, 72);
                Prop08RequestStatus = Encoding.ASCII.GetString(ByteStructure, 76, 15);
                Prop09OrderTrailingPrice = BitConverter.ToInt32(ByteStructure, 91);
                Prop10OrderTargetPrice = BitConverter.ToInt32(ByteStructure, 95);
                Prop11UpperPrice = BitConverter.ToInt32(ByteStructure, 99);
                Prop12ChildSLPrice = BitConverter.ToInt32(ByteStructure, 103);
                Prop13LowerPrice = BitConverter.ToInt32(ByteStructure, 107);
                Prop14ErrorMsg = Encoding.ASCII.GetString(ByteStructure, 111, 250);
                Prop15Reserved = Encoding.ASCII.GetString(ByteStructure, 261, 100);
            }

            public override string ToString()
            {
                return "DatatLength = " + Prop01DataLenght + "|" + "OrderDisplayStatus = " + Prop02OrderDisplayStatus + "|" + "OrderID = " + Prop03OrderID + "|" + "OrderDisplayStatus = " + Prop04ExchAckDateTime + "|" + "OrderQty =" + Prop05OrderQty + "|" + "OrderPrice = " + Prop06OrderPrice + "|" +
                    Prop07OrderTriggerPrice + "|" + "RequestStatus = " + Prop08RequestStatus + "|" + "OrderTrailingPrice = " + Prop09OrderTrailingPrice + "|" + "OrderTargetPrice = " + Prop10OrderTargetPrice + "|" + "UpperPrice = " + Prop11UpperPrice + "|" + "ChildSLPrice = " + Prop12ChildSLPrice
                    + "|" + "LowerPrice = " + Prop13LowerPrice + "|" + "ErrorMsg = " + Prop14ErrorMsg + "|" + "Reserved = " + Prop15Reserved;
            }

        }

        public struct CashTradeDetailsReportItem : IStruct
        {
            /*
             * 
         
             DataLength         Length of the Data                          LONG
            ExchangeCode        Code for each exchange
                                Refer Exchange Code                         CHAR[2]
            OrderID             Represents unique order id 
                                generated for each order by sharekhan.      CHAR[20]
            ExchOrderID         Unique order id of exchange order 
                                confirmation.                               CHAR[25]
            ExchAckDateTime     Date of that day exchange order 
                                will confirm. 
                                Ex: YYYY-MM-DD HH:mm:ss.n                   CHAR[25]
            TradeDateTime                                                   CHAR[25]
            InternalTradeID                                                 CHAR[15]
            TradeID                                                         CHAR[20]
            CustomerID                                                      CHAR[10]
            ScripToken          It contains the name of the company 
                                of which shares are brought or sell.        CHAR[10]
            BuySell             It contain the type of  order 
                                (i.e. buy/sell)
                                B,S,BM,SM,SS                                CHAR[2]
           TradeQty         This field contains the number of shares 
                            traded  by customer of particular company.      LONG
           TradePrice                                                       LONG
           TradeAmount                                                      LONG
           TotalTradeAmount                                                 LONG
           ChannelCode          It contains the type of channel.
                                Like TT,Web,TT API.                         CHAR[10]
           OrsExchangeMarketCode                                            CHAR[10]
           Reserved             Reserved for future
                                Default blank                               CHAR[100]

             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }


            private char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeCode.Length); }
            }

            private char[] cOrderID;
            public string Prop03OrderID
            {
                get { return new string(cOrderID); }
                set { cOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cOrderID.Length); }
            }

            private char[] cExchOrderID;
            public string Prop04ExchOrderID
            {
                get { return new string(cExchOrderID); }
                set { cExchOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cExchOrderID.Length); }
            }


            private char[] cExchAckDateTime;
            public string Prop05ExchAckDateTime
            {
                get { return new string(cExchAckDateTime); }
                set { cExchAckDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cExchAckDateTime.Length); }
            }


            private char[] cTradeDateTime;
            public string Prop06TradeDateTime
            {
                get { return new string(cTradeDateTime); }
                set { cTradeDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cTradeDateTime.Length); }
            }


            private char[] cInternalTradeID;
            public string Prop07InternalTradeID
            {
                get { return new string(cInternalTradeID); }
                set { cInternalTradeID = CUtility.GetPreciseArrayForString(value.ToString(), cInternalTradeID.Length); }
            }



            private char[] cTradeID;
            public string Prop08TradeID
            {
                get { return new string(cTradeID); }
                set { cTradeID = CUtility.GetPreciseArrayForString(value.ToString(), cTradeID.Length); }
            }

            private char[] cCustomerID;
            public string Prop09CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }

            private char[] cScripToken;
            public string Prop10ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }

            private char[] cBuySell;
            public string Prop11BuySell
            {
                get { return new string(cBuySell); }
                set { cBuySell = CUtility.GetPreciseArrayForString(value.ToString(), cBuySell.Length); }
            }


            private Int32 cTradeQty;
            public Int32 Prop12TradeQty
            {
                get { return cTradeQty; }
                set { cTradeQty = value; }
            }

            private Int32 cTradePrice;
            public Int32 Prop13TradePrice
            {
                get { return cTradePrice; }
                set { cTradePrice = value; }
            }

            private Int32 cTradeAmount;
            public Int32 Prop14TradeAmount
            {
                get { return cTradeAmount; }
                set { cTradeAmount = value; }
            }

            private Int32 cTotalTradeAmount;
            public Int32 Prop15TradeAmount
            {
                get { return cTotalTradeAmount; }
                set { cTotalTradeAmount = value; }

            }

            private char[] cChannelCode;
            public string Prop16ChannelCode
            {
                get { return new string(cChannelCode); }
                set { cChannelCode = CUtility.GetPreciseArrayForString(value.ToString(), cChannelCode.Length); }
            }

            private char[] cOrsExchangeMarketCode;
            public string Prop17OrsExchangeMarketCode
            {
                get { return new string(cOrsExchangeMarketCode); }
                set { cOrsExchangeMarketCode = CUtility.GetPreciseArrayForString(value.ToString(), cOrsExchangeMarketCode.Length); }
            }

            private char[] cReserved;
            public string Prop18Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }


            public CashTradeDetailsReportItem(bool value)
            {
                cDataLength = 0;
                cExchangeCode = new char[2];
                cOrderID = new char[20];
                cExchOrderID = new char[25];
                cExchAckDateTime = new char[25];
                cTradeDateTime = new char[25];
                cInternalTradeID = new char[15];
                cTradeID = new char[20];
                cCustomerID = new char[10];
                cScripToken = new char[10];
                cBuySell = new char[2];
                cTradeQty = 0;
                cTradePrice = 0;
                cTradeAmount = 0;
                cTotalTradeAmount = 0;
                cChannelCode = new char[10];
                cOrsExchangeMarketCode = new char[10];
                cReserved = new char[100];

            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 0);
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03OrderID = Encoding.ASCII.GetString(ByteStructure, 6, 20);
                Prop04ExchOrderID = Encoding.ASCII.GetString(ByteStructure, 26, 25);
                Prop05ExchAckDateTime = Encoding.ASCII.GetString(ByteStructure, 51, 25);
                Prop06TradeDateTime = Encoding.ASCII.GetString(ByteStructure, 76, 25);
                Prop07InternalTradeID = Encoding.ASCII.GetString(ByteStructure, 101, 15);
                Prop08TradeID = Encoding.ASCII.GetString(ByteStructure, 116, 20);
                Prop09CustomerID = Encoding.ASCII.GetString(ByteStructure, 136, 10);
                Prop10ScripToken = Encoding.ASCII.GetString(ByteStructure, 146, 10);
                Prop11BuySell = Encoding.ASCII.GetString(ByteStructure, 156, 2);
                Prop12TradeQty = BitConverter.ToInt32(ByteStructure, 158);
                Prop13TradePrice = BitConverter.ToInt32(ByteStructure, 162);
                Prop14TradeAmount = BitConverter.ToInt32(ByteStructure, 166);
                Prop15TradeAmount = BitConverter.ToInt32(ByteStructure, 170);
                Prop16ChannelCode = Encoding.ASCII.GetString(ByteStructure, 174, 10);
                Prop17OrsExchangeMarketCode = Encoding.ASCII.GetString(ByteStructure, 184, 10);
                Prop18Reserved = Encoding.ASCII.GetString(ByteStructure, 194, 100);
            }

            public override string ToString()
            {
                return "DataLenght = " + Prop01DataLenght + "|" + "ExchangeCode = " + Prop02ExchangeCode + "|" + "LowerPrice = " + Prop03OrderID + "|" + "ExchOrderID = " + Prop04ExchOrderID + "|" + "ExchAckDateTime = " + Prop05ExchAckDateTime + "|" + "TradeDateTime = " + Prop06TradeDateTime + "|" + "InternalTradeID = " + Prop07InternalTradeID + "|" + "TradeID = " + Prop08TradeID
                    + "|" + "CustomerID  = " + Prop09CustomerID + "|" + "ScripToken = " + Prop10ScripToken + "|" + "BuySell = " + Prop11BuySell + "|" + "TradeQty = " + Prop12TradeQty + "|" + "TradePrice = " + Prop13TradePrice + "|" + "TradeAmount = " + Prop14TradeAmount + "|" + "TradeAmount = " + Prop15TradeAmount + "|" + "ChannelCode = " + Prop16ChannelCode
                    + "|" + "ChannelCode  = " + Prop16ChannelCode + "|" + "OrsExchangeMarketCode = " + Prop17OrsExchangeMarketCode + "|" + "Reserved = " + Prop18Reserved;
            }
        }

        public struct DerivativeOrderDetailReportItem : IStruct
        {
            /*
             *
              
             
            DataLength          Length of the Data                  LONG
            ExchangeCode        Code for each exchange
                                Refer Exchange Code                 CHAR[2]
            OrderStatus         It contain status of the order 
                                (I.e. order is pending or fully 
                                executed).
                                Refer Appendix                      CHAR[20]
            OrderID             Represents unique order id 
                                generated for each order by 
                                sharekhan.                          CHAR[20]
            ExchangeOrderID     Unique order id of exchange 
                                order confirmation.                 CHAR[20]
            OrderDateTime       Ex: YYYY-MM-DD HH:mm:ss.n           CHAR[25]
            CustomerID                                              CHAR[10]
            DpClientId                                              CHAR[25]
            OrsOrderID                                              CHAR[10]
            ScripToken          unique token number for 
                                each scrip                          CHAR[10]
            OrderType           It can two types of orders
                                i.e Market or Limit                 CHAR[10]
            BuySell             It contain the type of  
                                order (i.e. buy/sell)
                                B,S,BM,SM,SS.                       CHAR[2]
            OrderQty            This field contains the number of 
                                shares entered by customer of 
                                particular company in multiple of 
                                minimum quantity for that particular 
                                script.
                                Total Quantity(No. Of Lot  X Lot 
                                Size)                               LONG
            OrderExecutedQty    Order executed quantity             LONG
            OrderDisclosedQty                                       LONG
            OrderMIFQty                                             LONG
            OrderPrice          If order type is market then 
                                price = 0 otherwise price=user 
                                defined price.                      LONG
            OrderTriggerPrice   Price at which order will 
                                get triggered                       LONG
            RMSCode             RMS server code under which the 
                                order placed.                       CHAR[15]
            AfterHour           After market hours Y , N            CHAR[1]
            BranchTraderID                                          CHAR[15]
            AveragePrice                                            LONG
            RequestStatus       It contain status of request
                                (i.e request is new/cancel/modify). CHAR[15]
            GoodTill            Represents the status of IOC, GFD, 
                                GTD and GTC.                        CHAR[5]
            GoodTillDate        Format “yyyy-MM-DD HH:mm:ss.n”      CHAR[25]
            DpId                                                    CHAR[10]
            OrsExchangeMktCode                                      CHAR[10]
            ChannelCode         It contains the type of channel.
                                Like TT,Web,TT API.                 CHAR[10]
            ChannelUser         It contain the user id who has 
                                currently logged in.                CHAR[20]
            LastModDateTime     It represents the date-time at 
                                which the order details where 
                                last modified.
                                Ex: YYYY-MM-DD HH:mm:ss.n           CHAR[25]
            OpenQty             It represents pending order 
                                quantity.                           LONG.
            PvtOrderInd                                             LONG
            ClientAccount                                           CHAR[20]
            ClientGroup                                             CHAR[20]
            OhEntryDateTime                                         CHAR[25]
            WebResponseTime                                         CHAR[25]
            FohExitDateTime                                         CHAR[25]
            ExchangeAckDateTime                                     CHAR[25]
            Brokerage                                               LONG
            ParticipantCode                                         CHAR[10]
            UpdateDate          It represents the date-time at 
                                which the order details where 
                                last modified.
                                Format “yyyy-MM-DD HH:mm:ss.n”      CHAR[25]
           UpdateUser           It contain our system status 
                                Like NOR=order is pending.,TC= 
                                order executed.,COC= canceled 
                                order.                              CHAR[25]
            CALevel             Ignore this field                   CHAR[15]
            AllOrNone           Order will get executed  either 
                                for all quantity or it will not 
                                get executed entirely.
                                All or None                         CHAR[25]
            OpenOrClose         Ignore this field                   CHAR[25]
            FnoOrderType        It represents the normal order 
                                type.                               CHAR[25]
            FnoSquareOff                                            CHAR[25]
            Reserved            Reserved for future Default blank   CHAR[100]

         
             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }


            private char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeCode.Length); }
            }

            private char[] cOrderStatus;
            public string Prop03OrderStatus
            {
                get { return new string(cOrderStatus); }
                set { cOrderStatus = CUtility.GetPreciseArrayForString(value.ToString(), cOrderStatus.Length); }
            }

            private char[] cOrderID;
            public string Prop04OrderID
            {
                get { return new string(cOrderID); }
                set { cOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cOrderID.Length); }
            }

            private char[] cExchangeOrderID;
            public string Prop05ExchangeOrderID
            {
                get { return new string(cExchangeOrderID); }
                set { cExchangeOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeOrderID.Length); }
            }


            private char[] cOrderDateTime;
            public string Prop06OrderDateTime
            {
                get { return new string(cOrderDateTime); }
                set { cOrderDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cOrderDateTime.Length); }
            }

            private char[] cCustomerID;
            public string Prop07CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }

            private char[] cDpClientId;
            public string Prop08DpClientId
            {
                get { return new string(cDpClientId); }
                set { cDpClientId = CUtility.GetPreciseArrayForString(value.ToString(), cDpClientId.Length); }
            }

            private char[] cOrsOrderID;
            public string Prop09OrsOrderID
            {
                get { return new string(cOrsOrderID); }
                set { cOrsOrderID = CUtility.GetPreciseArrayForString(value.ToString(), cOrsOrderID.Length); }
            }

            private char[] cScripToken;
            public string Prop10ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }

            private char[] cOrderType;
            public string Prop11OrderType
            {
                get { return new string(cOrderType); }
                set { cOrderType = CUtility.GetPreciseArrayForString(value.ToString(), cOrderType.Length); }
            }

            private char[] cBuySell;
            public string Prop12BuySell
            {
                get { return new string(cBuySell); }
                set { cBuySell = CUtility.GetPreciseArrayForString(value.ToString(), cBuySell.Length); }
            }

            private Int32 cOrderQty;
            public Int32 Prop13OrderQty
            {
                get { return cOrderQty; }
                set { cOrderQty = value; }
            }



            private Int32 cOrderExecutedQty;
            public Int32 Prop14OrderExecutedQty
            {
                get { return cOrderExecutedQty; }
                set { cOrderExecutedQty = value; }
            }

            private Int32 cOrderDisclosedQty;
            public Int32 Prop15OrderDisclosedQty
            {
                get { return cOrderDisclosedQty; }
                set { cOrderDisclosedQty = value; }
            }

            private Int32 cOrderMIFQty;
            public Int32 Prop16OrderMIFQty
            {
                get { return cOrderMIFQty; }
                set { cOrderMIFQty = value; }
            }

            private Int32 cOrderPrice;
            public Int32 Prop17OrderPrice
            {
                get { return cOrderPrice; }
                set { cOrderPrice = value; }
            }

            private Int32 cOrderTriggerPrice;
            public Int32 Prop18OrderTriggerPrice
            {
                get { return cOrderTriggerPrice; }
                set { cOrderTriggerPrice = value; }
            }

            private char[] cRMSCode;
            public string Prop19RMSCode
            {
                get { return new string(cRMSCode); }
                set { cRMSCode = CUtility.GetPreciseArrayForString(value, cRMSCode.Length); }
            }

            private char[] cAfterHour;
            public string Prop20AfterHour
            {
                get { return new string(cAfterHour); }
                set { cAfterHour = CUtility.GetPreciseArrayForString(value, cAfterHour.Length); }
            }

            private char[] cBranchTraderID;
            public string Prop21BranchTraderID
            {
                get { return new string(cBranchTraderID); }
                set { cBranchTraderID = CUtility.GetPreciseArrayForString(value, cBranchTraderID.Length); }
            }

            private Int32 cAveragePrice;
            public Int32 Prop22AveragePrice
            {
                get { return cAveragePrice; }
                set { cAveragePrice = value; }
            }

            private char[] cRequestStatus;
            public string Prop23RequestStatus
            {
                get { return new string(cRequestStatus); }
                set { cRequestStatus = CUtility.GetPreciseArrayForString(value, cRequestStatus.Length); }
            }

            private char[] cGoodTill;
            public string Prop24GoodTill
            {
                get { return new string(cGoodTill); }
                set { cGoodTill = CUtility.GetPreciseArrayForString(value, cGoodTill.Length); }
            }

            private char[] cGoodTillDate;
            public string Prop25GoodTillDate
            {
                get { return new string(cGoodTillDate); }
                set { cGoodTillDate = CUtility.GetPreciseArrayForString(value, cGoodTillDate.Length); }
            }

            private char[] cDpId;
            public string Prop26DpId
            {
                get { return new string(cDpId); }
                set { cDpId = CUtility.GetPreciseArrayForString(value, cDpId.Length); }
            }

            private char[] cOrsExchangeMktCode;
            public string Prop27OrsExchangeMktCode
            {
                get { return new string(cOrsExchangeMktCode); }
                set { cOrsExchangeMktCode = CUtility.GetPreciseArrayForString(value, cOrsExchangeMktCode.Length); }
            }

            private char[] cChannelCode;
            public string Prop28ChannelCode
            {
                get { return new string(cChannelCode); }
                set { cChannelCode = CUtility.GetPreciseArrayForString(value, cChannelCode.Length); }
            }

            private char[] cChannelUser;
            public string Prop29ChannelUser
            {
                get { return new string(cChannelUser); }
                set { cChannelUser = CUtility.GetPreciseArrayForString(value, cChannelUser.Length); }
            }

            private char[] cLastModDateTime;
            public string Prop30LastModDateTime
            {
                get { return new string(cLastModDateTime); }
                set { cLastModDateTime = CUtility.GetPreciseArrayForString(value, cLastModDateTime.Length); }
            }
            private Int32 cOpenQty;
            public Int32 Prop31OpenQty
            {
                get { return cOpenQty; }
                set { cOpenQty = value; }
            }

            private Int32 cPvtOrderInd;
            public Int32 Prop32PvtOrderInd
            {
                get { return cPvtOrderInd; }
                set { cPvtOrderInd = value; }
            }

            private char[] cClientAccount;
            public string Prop33ClientAccount
            {
                get { return new string(cClientAccount); }
                set { cClientAccount = CUtility.GetPreciseArrayForString(value, cClientAccount.Length); }
            }

            private char[] cClientGroup;
            public string Prop34ClientGroup
            {
                get { return new string(cClientGroup); }
                set { cClientGroup = CUtility.GetPreciseArrayForString(value, cClientGroup.Length); }
            }

            private char[] cOhEntryDateTime;
            public string Prop35OhEntryDateTime
            {
                get { return new string(cOhEntryDateTime); }
                set { cOhEntryDateTime = CUtility.GetPreciseArrayForString(value, cOhEntryDateTime.Length); }
            }

            private char[] cWebResponseTime;
            public string Prop36WebResponseTime
            {
                get { return new string(cWebResponseTime); }
                set { cWebResponseTime = CUtility.GetPreciseArrayForString(value, cWebResponseTime.Length); }
            }

            private char[] cFohExitDateTime;
            public string Prop37FohExitDateTime
            {
                get { return new string(cFohExitDateTime); }
                set { cFohExitDateTime = CUtility.GetPreciseArrayForString(value, cFohExitDateTime.Length); }
            }

            private char[] cExchangeAckDateTime;
            public string Prop38ExchangeAckDateTime
            {
                get { return new string(cExchangeAckDateTime); }
                set { cExchangeAckDateTime = CUtility.GetPreciseArrayForString(value, cExchangeAckDateTime.Length); }
            }

            private Int32 cBrokerage;
            public Int32 Prop39Brokerage
            {
                get { return cBrokerage; }
                set { cBrokerage = value; }
            }

            private char[] cParticipantCode;
            public string Prop40ParticipantCode
            {
                get { return new string(cParticipantCode); }
                set { cParticipantCode = CUtility.GetPreciseArrayForString(value, cParticipantCode.Length); }
            }

            private char[] cUpdateDate;
            public string Prop41UpdateDate
            {
                get { return new string(cUpdateDate); }
                set { cUpdateDate = CUtility.GetPreciseArrayForString(value, cUpdateDate.Length); }
            }

            private char[] cUpdateUser;
            public string Prop42UpdateUser
            {
                get { return new string(cUpdateUser); }
                set { cUpdateUser = CUtility.GetPreciseArrayForString(value, cUpdateUser.Length); }
            }

            private char[] cCALevel;
            public string Prop43CALevel
            {
                get { return new string(cCALevel); }
                set { cCALevel = CUtility.GetPreciseArrayForString(value, cCALevel.Length); }
            }

            private char[] cAllOrNone;
            public string Prop44AllOrNone
            {
                get { return new string(cAllOrNone); }
                set { cAllOrNone = CUtility.GetPreciseArrayForString(value, cAllOrNone.Length); }
            }

            private char[] cOpenOrClose;
            public string Prop45OpenOrClose
            {
                get { return new string(cOpenOrClose); }
                set { cOpenOrClose = CUtility.GetPreciseArrayForString(value, cOpenOrClose.Length); }
            }

            private char[] cFnoOrderType;
            public string Prop46FnoOrderType
            {
                get { return new string(cFnoOrderType); }
                set { cFnoOrderType = CUtility.GetPreciseArrayForString(value, cFnoOrderType.Length); }
            }
            private char[] cFnoSquareOff;
            public string Prop47FnoSquareOff
            {
                get { return new string(cFnoSquareOff); }
                set { cFnoSquareOff = CUtility.GetPreciseArrayForString(value, cFnoSquareOff.Length); }
            }

            public DerivativeOrderDetailReportItem(bool value)
            {
                cDataLength = 0;
                cExchangeCode = new char[2];
                cOrderStatus = new char[20];
                cOrderID = new char[20];
                cExchangeOrderID = new char[20];
                cOrderDateTime = new char[25];
                cCustomerID = new char[10];
                cDpClientId = new char[25];
                cOrsOrderID = new char[10];
                cScripToken = new char[10];
                cOrderType = new char[10];
                cBuySell = new char[2];
                cOrderQty = 0;
                cOrderExecutedQty = 0;
                cOrderDisclosedQty = 0;
                cOrderMIFQty = 0;
                cOrderPrice = 0;
                cOrderTriggerPrice = 0;
                cRMSCode = new char[15];
                cAfterHour = new char[1];
                cBranchTraderID = new char[15];
                cAveragePrice = 0;
                cRequestStatus = new char[15];
                cGoodTill = new char[5];
                cGoodTillDate = new char[25];
                cDpId = new char[10];
                cOrsExchangeMktCode = new char[10];
                cChannelCode = new char[10];
                cChannelUser = new char[20];
                cLastModDateTime = new char[25];
                cOpenQty = 0;
                cPvtOrderInd = 0;
                cClientAccount = new char[20];
                cClientGroup = new char[20];
                cOhEntryDateTime = new char[25];
                cWebResponseTime = new char[25];
                cFohExitDateTime = new char[25];
                cExchangeAckDateTime = new char[25];
                cBrokerage = 0;
                cParticipantCode = new char[10];
                cUpdateDate = new char[25];
                cUpdateUser = new char[25];
                cCALevel = new char[15];
                cAllOrNone = new char[25];
                cOpenOrClose = new char[25];
                cFnoOrderType = new char[25];
                cFnoSquareOff = new char[25];
                cReserved = new char[100];


            }


            private char[] cReserved;
            public string Prop48Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 0);
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03OrderStatus = Encoding.ASCII.GetString(ByteStructure, 6, 20);
                Prop04OrderID = Encoding.ASCII.GetString(ByteStructure, 26, 20);
                Prop05ExchangeOrderID = Encoding.ASCII.GetString(ByteStructure, 46, 20);
                Prop06OrderDateTime = Encoding.ASCII.GetString(ByteStructure, 66, 25);
                Prop07CustomerID = Encoding.ASCII.GetString(ByteStructure, 91, 10);
                Prop08DpClientId = Encoding.ASCII.GetString(ByteStructure, 101, 25);
                Prop09OrsOrderID = Encoding.ASCII.GetString(ByteStructure, 126, 10);
                Prop10ScripToken = Encoding.ASCII.GetString(ByteStructure, 136, 10);
                Prop11OrderType = Encoding.ASCII.GetString(ByteStructure, 146, 10);
                Prop12BuySell = Encoding.ASCII.GetString(ByteStructure, 156, 2);
                Prop13OrderQty = BitConverter.ToInt32(ByteStructure, 158);
                Prop14OrderExecutedQty = BitConverter.ToInt32(ByteStructure, 162);
                Prop15OrderDisclosedQty = BitConverter.ToInt32(ByteStructure, 166);
                Prop16OrderMIFQty = BitConverter.ToInt32(ByteStructure, 170);
                Prop17OrderPrice = BitConverter.ToInt32(ByteStructure, 174);
                Prop18OrderTriggerPrice = BitConverter.ToInt32(ByteStructure, 178);
                Prop19RMSCode = Encoding.ASCII.GetString(ByteStructure, 182, 15);
                Prop20AfterHour = Encoding.ASCII.GetString(ByteStructure, 197, 1);
                Prop21BranchTraderID = Encoding.ASCII.GetString(ByteStructure, 198, 15);
                Prop22AveragePrice = BitConverter.ToInt32(ByteStructure, 213);
                Prop23RequestStatus = Encoding.ASCII.GetString(ByteStructure, 217, 15);
                Prop24GoodTill = Encoding.ASCII.GetString(ByteStructure, 232, 5);
                Prop25GoodTillDate = Encoding.ASCII.GetString(ByteStructure, 237, 25);
                Prop26DpId = Encoding.ASCII.GetString(ByteStructure, 262, 10);
                Prop27OrsExchangeMktCode = Encoding.ASCII.GetString(ByteStructure, 272, 10);
                Prop28ChannelCode = Encoding.ASCII.GetString(ByteStructure, 282, 10);
                Prop29ChannelUser = Encoding.ASCII.GetString(ByteStructure, 292, 20);
                Prop30LastModDateTime = Encoding.ASCII.GetString(ByteStructure, 312, 25);
                Prop31OpenQty = BitConverter.ToInt32(ByteStructure, 337);
                Prop32PvtOrderInd = BitConverter.ToInt32(ByteStructure, 441);
                Prop33ClientAccount = Encoding.ASCII.GetString(ByteStructure, 445, 20);
                Prop34ClientGroup = Encoding.ASCII.GetString(ByteStructure, 465, 20);
                Prop35OhEntryDateTime = Encoding.ASCII.GetString(ByteStructure, 485, 25);
                Prop36WebResponseTime = Encoding.ASCII.GetString(ByteStructure, 510, 25);
                Prop37FohExitDateTime = Encoding.ASCII.GetString(ByteStructure, 535, 25);
                Prop38ExchangeAckDateTime = Encoding.ASCII.GetString(ByteStructure, 560, 25);
                Prop39Brokerage = BitConverter.ToInt32(ByteStructure, 585);
                Prop40ParticipantCode = Encoding.ASCII.GetString(ByteStructure, 589, 10);
                Prop41UpdateDate = Encoding.ASCII.GetString(ByteStructure, 599, 25);
                Prop42UpdateUser = Encoding.ASCII.GetString(ByteStructure, 624, 25);
                Prop43CALevel = Encoding.ASCII.GetString(ByteStructure, 649, 15);
                Prop44AllOrNone = Encoding.ASCII.GetString(ByteStructure, 664, 25);
                Prop45OpenOrClose = Encoding.ASCII.GetString(ByteStructure, 689, 25);
                Prop46FnoOrderType = Encoding.ASCII.GetString(ByteStructure, 714, 25);
                Prop47FnoSquareOff = Encoding.ASCII.GetString(ByteStructure, 739, 25);
              //  Prop48Reserved = Encoding.ASCII.GetString(ByteStructure, 764, 100);
            }

            public override string ToString()
            {
                return Prop01DataLenght + "|" + Prop02ExchangeCode + "|" + Prop03OrderStatus + "|" + Prop04OrderID + "|" + Prop04OrderID + "|" + Prop05ExchangeOrderID + "|" + Prop06OrderDateTime + "|" + Prop07CustomerID + "|" + Prop08DpClientId + "|" + Prop09OrsOrderID + "|" + Prop10ScripToken
                    + "|" + Prop11OrderType + "|" + Prop12BuySell + "|" + Prop13OrderQty + "|" + Prop14OrderExecutedQty + "|" + Prop15OrderDisclosedQty + "|" + Prop16OrderMIFQty + "|" + Prop17OrderPrice + "|" + Prop18OrderTriggerPrice + "|" + Prop19RMSCode + "|" + Prop20AfterHour
                    + "|" + Prop21BranchTraderID + "|" + Prop22AveragePrice + "|" + Prop23RequestStatus + "|" + Prop24GoodTill + "|" + Prop25GoodTillDate + "|" + Prop26DpId + "|" + Prop27OrsExchangeMktCode + "|" + Prop28ChannelCode + "|" + Prop29ChannelUser + "|" + Prop30LastModDateTime
                    + "|" + Prop31OpenQty + "|" + Prop32PvtOrderInd + "|" + Prop33ClientAccount + "|" + Prop34ClientGroup + Prop35OhEntryDateTime + "|" + Prop36WebResponseTime + "|" + Prop37FohExitDateTime + "|" + Prop38ExchangeAckDateTime + "|" + Prop39Brokerage + "|" + Prop40ParticipantCode
                    + "|" + Prop41UpdateDate + "|" + Prop42UpdateUser + "|" + Prop43CALevel + "|" + Prop44AllOrNone + "|" + Prop45OpenOrClose + "|" + Prop46FnoOrderType + "|" + Prop47FnoSquareOff + "|" + Prop48Reserved;
            }
        }

        public struct DerivativeTradeDetailsReportItem : IStruct
        {
            /*
             * 
            DataLength          Length of the Data                      LONG
            ExchangeCode        Code for each exchange   
                                Refer Exchange Code                     CHAR[2]
            InternalTradeId                                             CHAR[15]
            TradeId             Represents unique trade id 
                                generated for each order by 
                                sharekhan.                              CHAR[20]
            ChannelCode         It contains the type of channel.
                                Like TT,Web,TT API.                     CHAR[10]
            ChannelUser         It contain the user id who has 
                                currently logged in.                    CHAR[20]
            OrderId             Represents unique order id 
                                generated for each order by sharekhan.  CHAR[20]
            CustomerId                                                  CHAR[10]
            BuySell             It contain the type of  order 
                                (i.e. buy/sell)
                                B,S,BM,SM,SS.                           CHAR[2]
            OrsExchMktCode                                              CHAR[10]
            ScripToken          unique token number for each scrip      CHAR[10]
            TradeQty            This field contains the number of 
                                shares traded  by customer of 
                                particular company.                      LONG
            TradePrice          Executed Price                           LONG
            TradeAmount         Total Executed Amount                    LONG
            TradeDateTime       Executed Date Time                       CHAR[25]
            ExchAckDateTime                                              CHAR[25]
            Brokerage           Descibes the brokerage details           CHAR[10]
            TotalTradeAmount    Total Executed Amount                    LONG
            UpdateDate          It represents the date-time at which 
                                the order details where last modified.    CHAR[25]
            UpdateUser          It contain our system status 
                                Like NOR=order is pending.,TC= order 
                                executed.,COC= canceled order.          CHAR[25]
            CALevel             Ignore this field                       CHAR[15]
            Reserved            Reserved for future Default blank       CHAR[100]

             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }


            private char[] cExchangeCode;
            public string Prop02ExchangeCode
            {
                get { return new string(cExchangeCode); }
                set { cExchangeCode = CUtility.GetPreciseArrayForString(value.ToString(), cExchangeCode.Length); }
            }

            private char[] cInternalTradeId;
            public string Prop03InternalTradeId
            {
                get { return new string(cInternalTradeId); }
                set { cInternalTradeId = CUtility.GetPreciseArrayForString(value.ToString(), cInternalTradeId.Length); }
            }

            private char[] cTradeId;
            public string Prop04TradeId
            {
                get { return new string(cTradeId); }
                set { cTradeId = CUtility.GetPreciseArrayForString(value.ToString(), cTradeId.Length); }
            }


            private char[] cChannelCode;
            public string Prop05ChannelCode
            {
                get { return new string(cChannelCode); }
                set { cChannelCode = CUtility.GetPreciseArrayForString(value.ToString(), cChannelCode.Length); }
            }


            private char[] cChannelUser;
            public string Prop06ChannelUser
            {
                get { return new string(cChannelUser); }
                set { cChannelUser = CUtility.GetPreciseArrayForString(value.ToString(), cChannelUser.Length); }
            }


            private char[] cOrderId;
            public string Prop07OrderId
            {
                get { return new string(cOrderId); }
                set { cOrderId = CUtility.GetPreciseArrayForString(value.ToString(), cOrderId.Length); }
            }


            private char[] cCustomerID;
            public string Prop08CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }

            private char[] cBuySell;
            public string Prop09BuySell
            {
                get { return new string(cBuySell); }
                set { cBuySell = CUtility.GetPreciseArrayForString(value.ToString(), cBuySell.Length); }
            }

            private char[] cOrsExchMktCode;
            public string Prop10OrsExchMktCode
            {
                get { return new string(cOrsExchMktCode); }
                set { cOrsExchMktCode = CUtility.GetPreciseArrayForString(value.ToString(), cOrsExchMktCode.Length); }
            }



            private char[] cScripToken;
            public string Prop11ScripToken
            {
                get { return new string(cScripToken); }
                set { cScripToken = CUtility.GetPreciseArrayForString(value.ToString(), cScripToken.Length); }
            }


            private Int32 cTradeQty;
            public Int32 Prop12TradeQty
            {
                get { return cTradeQty; }
                set { cTradeQty = value; }
            }

            private Int32 cTradePrice;
            public Int32 Prop13TradePrice
            {
                get { return cTradePrice; }
                set { cTradePrice = value; }
            }

            private Int32 cTradeAmount;
            public Int32 Prop14TradeAmount
            {
                get { return cTradeAmount; }
                set { cTradeAmount = value; }
            }

            private char[] cTradeDateTime;
            public string Prop15TradeDateTime
            {
                get { return new string(cTradeDateTime); }
                set { cTradeDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cTradeDateTime.Length); }
            }



            private char[] cExchAckDateTime;
            public string Prop16ExchAckDateTime
            {
                get { return new string(cExchAckDateTime); }
                set { cExchAckDateTime = CUtility.GetPreciseArrayForString(value.ToString(), cExchAckDateTime.Length); }
            }

            private char[] cBrokerage;
            public string Prop17Brokerage
            {
                get { return new string(cBrokerage); }
                set { cBrokerage = CUtility.GetPreciseArrayForString(value.ToString(), cBrokerage.Length); }

            }

            private Int32 cTotalTradeAmount;
            public Int32 Prop18TotalTradeAmount
            {
                get { return cTotalTradeAmount; }
                set { cTotalTradeAmount = value; }
            }

            private char[] cUpdateDate;
            public string Prop19UpdateDate
            {
                get { return new string(cUpdateDate); }
                set { cUpdateDate = CUtility.GetPreciseArrayForString(value, cUpdateDate.Length); }
            }

            private char[] cUpdateUser;
            public string Prop20UpdateUser
            {
                get { return new string(cUpdateUser); }
                set { cUpdateUser = CUtility.GetPreciseArrayForString(value, cUpdateUser.Length); }
            }

            private char[] cCALevel;
            public string Prop21CALevel
            {
                get { return new string(cCALevel); }
                set { cCALevel = CUtility.GetPreciseArrayForString(value, cCALevel.Length); }
            }


            private char[] cReserved;
            public string Prop22Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value, cReserved.Length); }
            }

            public DerivativeTradeDetailsReportItem(bool value)
            {
                cDataLength = 0;
                cExchangeCode = new char[2];
                cInternalTradeId = new char[15];
                cTradeId = new char[20];
                cChannelCode = new char[10];
                cChannelUser = new char[20];
                cOrderId = new char[20];
                cCustomerID = new char[10];
                cBuySell = new char[2];
                cOrsExchMktCode = new char[10];
                cScripToken = new char[10];
                cTradeQty = 0;
                cTradePrice = 0;
                cTradeAmount = 0;
                cTradeDateTime = new char[25];
                cExchAckDateTime = new char[25];
                cBrokerage = new char[10];
                cTotalTradeAmount = 0;
                cUpdateDate = new char[25];
                cUpdateUser = new char[25];
                cCALevel = new char[15];
                cReserved = new char[100];

            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 0);
                Prop02ExchangeCode = Encoding.ASCII.GetString(ByteStructure, 4, 2);
                Prop03InternalTradeId = Encoding.ASCII.GetString(ByteStructure, 6, 15);
                Prop04TradeId = Encoding.ASCII.GetString(ByteStructure, 21, 20);
                Prop05ChannelCode = Encoding.ASCII.GetString(ByteStructure, 41, 10);
                Prop06ChannelUser = Encoding.ASCII.GetString(ByteStructure, 51, 20);
                Prop07OrderId = Encoding.ASCII.GetString(ByteStructure, 71, 20);
                Prop08CustomerID = Encoding.ASCII.GetString(ByteStructure, 91, 10);
                Prop09BuySell = Encoding.ASCII.GetString(ByteStructure, 101, 2);
                Prop10OrsExchMktCode = Encoding.ASCII.GetString(ByteStructure, 103, 10);
                Prop11ScripToken = Encoding.ASCII.GetString(ByteStructure, 113, 10);
                Prop12TradeQty = BitConverter.ToInt32(ByteStructure, 123);
                Prop13TradePrice = BitConverter.ToInt32(ByteStructure, 127);
                Prop14TradeAmount = BitConverter.ToInt32(ByteStructure, 131);
                Prop15TradeDateTime = Encoding.ASCII.GetString(ByteStructure, 135, 25);
                Prop16ExchAckDateTime = Encoding.ASCII.GetString(ByteStructure, 160, 25);
                Prop17Brokerage = Encoding.ASCII.GetString(ByteStructure, 185, 10);
                Prop18TotalTradeAmount = BitConverter.ToInt32(ByteStructure, 195);
                Prop19UpdateDate = Encoding.ASCII.GetString(ByteStructure, 199, 25);
                Prop20UpdateUser = Encoding.ASCII.GetString(ByteStructure, 224, 25);
                Prop21CALevel = Encoding.ASCII.GetString(ByteStructure, 249, 10);
                Prop22Reserved = Encoding.ASCII.GetString(ByteStructure, 259, 100);

            }

            public override string ToString()
            {
                return Prop01DataLenght + "|" + Prop02ExchangeCode + "|" + Prop03InternalTradeId + "|" + Prop04TradeId + "|" + Prop05ChannelCode + "|" + Prop06ChannelUser + "|" + Prop07OrderId + "|" + Prop08CustomerID + "|" + Prop09BuySell + "|" + Prop10OrsExchMktCode
                    + "|" + Prop11ScripToken + "|" + Prop12TradeQty + "|" + Prop13TradePrice + "|" + Prop14TradeAmount + "|" + Prop15TradeDateTime + "|" + Prop16ExchAckDateTime + "|" + Prop17Brokerage + "|" + Prop18TotalTradeAmount + "|" + Prop19UpdateDate + "|" + Prop20UpdateUser
                    + "|" + Prop21CALevel + "|" + Prop22Reserved;
            }

        }

        public struct CashLimitReportItem : IStruct
        {
            /*
             * 
             DataLength                     Length of the Data                      LONG
            CustomerID                                                              CHAR[10]
            CurrentCashBalance              Opening limit                           LONG
            PendingWithdrawalRequest        The client has withdrawn the amount     LONG
            NonCashLimit                    Tradable not actual amount              LONG
            CashBpl                         The client has booked the Profit Loss   LONG
            CashMTM                         Notional Profit Loss Cash               LONG
            LimitAgainstShares                                                      LONG
            CashPreviousSettlementExposure  Last two days settlement balance        LONG
            IntradayMarginCash              Today’s utilized margin                 LONG
            FnoMTM                          Notional Profit Loss FNO                LONG
            FnoPremium                                                              LONG
            FnoBpl                          FNO’s booked ProfitLoss                 LONG
            IntradayMarginFno                                                       LONG
            IntradayMarginComm                                                      LONG
            HoldFunds                                                               LONG
            Total                                                                   LONG
            Reserved                        Reserved for future Default blank       CHAR[100]

             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private char[] cCustomerID;
            public string Prop02CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }


            private Int32 cCurrentCashBalance;
            public Int32 Prop03CurrentCashBalance
            {
                get { return cCurrentCashBalance; }
                set { cCurrentCashBalance = value; }
            }

            private Int32 cPendingWithdrawalRequest;
            public Int32 Prop04PendingWithdrawalRequest
            {
                get { return cPendingWithdrawalRequest; }
                set { cPendingWithdrawalRequest = value; }
            }



            private Int32 cNonCashLimit;
            public Int32 Prop05NonCashLimit
            {
                get { return cNonCashLimit; }
                set { cNonCashLimit = value; }
            }

            private Int32 cCashBpl;
            public Int32 Prop06CashBpl
            {
                get { return cCashBpl; }
                set { cCashBpl = value; }
            }

            private Int32 cCashMTM;
            public Int32 Prop07CashMTM
            {
                get { return cCashMTM; }
                set { cCashMTM = value; }
            }

            private Int32 cLimitAgainstShares;
            public Int32 Prop08LimitAgainstShares
            {
                get { return cLimitAgainstShares; }
                set { cLimitAgainstShares = value; }
            }


            private Int32 cCashPreviousSettlementExposure;
            public Int32 Prop09CashPreviousSettlementExposure
            {
                get { return cCashPreviousSettlementExposure; }
                set { cCashPreviousSettlementExposure = value; }
            }


            private Int32 cIntradayMarginCash;
            public Int32 Prop10IntradayMarginCash
            {
                get { return cIntradayMarginCash; }
                set { cIntradayMarginCash = value; }
            }

            private Int32 cFnoMTM;
            public Int32 Prop11FnoMTM
            {
                get { return cFnoMTM; }
                set { cFnoMTM = value; }
            }
            private Int32 cFnoPremium;
            public Int32 Prop12FnoPremium
            {
                get { return cFnoPremium; }
                set { cFnoPremium = value; }
            }

            private Int32 cFnoBpl;
            public Int32 Prop13FnoBpl
            {
                get { return cFnoBpl; }
                set { cFnoBpl = value; }
            }

            private Int32 cIntradayMarginFno;
            public Int32 Prop14IntradayMarginFno
            {
                get { return cIntradayMarginFno; }
                set { cIntradayMarginFno = value; }
            }

            private Int32 cIntradayMarginComm;
            public Int32 Prop15IntradayMarginComm
            {
                get { return cIntradayMarginComm; }
                set { cIntradayMarginComm = value; }
            }

            private Int32 cHoldFunds;
            public Int32 Prop16HoldFunds
            {
                get { return cHoldFunds; }
                set { cHoldFunds = value; }
            }

            private Int32 cTotal;
            public Int32 Prop17Total
            {
                get { return cTotal; }
                set { cTotal = value; }
            }

            private char[] cReserved;
            public string Prop18Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToString(), cReserved.Length); }
            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public CashLimitReportItem(bool value)
            {

                cDataLength = 0;
                cCustomerID = new char[10];
                cCurrentCashBalance = 0;
                cPendingWithdrawalRequest = 0;
                cNonCashLimit = 0;
                cCashBpl = 0;
                cCashMTM = 0;
                cLimitAgainstShares = 0;
                cCashPreviousSettlementExposure = 0;
                cIntradayMarginCash = 0;
                cFnoMTM = 0;
                cFnoPremium = 0;
                cFnoBpl = 0;
                cIntradayMarginFno = 0;
                cIntradayMarginComm = 0;
                cHoldFunds = 0;
                cTotal = 0;
                cReserved = new char[100];


            }

            public void ByteToStruct(byte[] ByteStructure)
            {
                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 4);
                Prop02CustomerID = Encoding.ASCII.GetString(ByteStructure, 4, 10);
                Prop03CurrentCashBalance = BitConverter.ToInt32(ByteStructure, 14); ;
                Prop04PendingWithdrawalRequest = BitConverter.ToInt32(ByteStructure, 18);
                Prop05NonCashLimit = BitConverter.ToInt32(ByteStructure, 22);
                Prop06CashBpl = BitConverter.ToInt32(ByteStructure, 26);
                Prop07CashMTM = BitConverter.ToInt32(ByteStructure, 30);
                Prop08LimitAgainstShares = BitConverter.ToInt32(ByteStructure, 34);
                Prop09CashPreviousSettlementExposure = BitConverter.ToInt32(ByteStructure, 38);
                Prop10IntradayMarginCash = BitConverter.ToInt32(ByteStructure, 42);
                Prop11FnoMTM = BitConverter.ToInt32(ByteStructure, 46);
                Prop12FnoPremium = BitConverter.ToInt32(ByteStructure, 50);
                Prop13FnoBpl = BitConverter.ToInt32(ByteStructure, 54);
                Prop14IntradayMarginFno = BitConverter.ToInt32(ByteStructure, 58);
                Prop15IntradayMarginComm = BitConverter.ToInt32(ByteStructure, 62);
                Prop16HoldFunds = BitConverter.ToInt32(ByteStructure, 66);
                Prop17Total = BitConverter.ToInt32(ByteStructure, 70);
                Prop18Reserved = Encoding.ASCII.GetString(ByteStructure, 74, 100);
            }

            public override string ToString()
            {
                return Prop01DataLenght + "|" + Prop02CustomerID + "|" + Prop03CurrentCashBalance + "|" + Prop04PendingWithdrawalRequest + "|" + Prop05NonCashLimit + "|" + Prop06CashBpl
                    + "|" + Prop07CashMTM + "|" + Prop08LimitAgainstShares + "|" + Prop09CashPreviousSettlementExposure + "|" + Prop10IntradayMarginCash + "|" + Prop11FnoMTM + "|" + Prop12FnoPremium
                    + "|" + Prop13FnoBpl + "|" + Prop14IntradayMarginFno + "|" + Prop15IntradayMarginComm;
            }
        }

        public struct CommodityLimitReportItem : IStruct
        {
            /*
             * 
             DataLength         Length of the Data                  LONG
            CustomerID                                              CHAR[10]
            CCB                                                     LONG
            WithDrawn                                               LONG
            NCL                                                     LONG
            MarginforComm                                           LONG
            MMTMLoss                                                LONG
            Bpl                                                     LONG
            HoldFunds                                               LONG
            NseWithdrawlBal                                         LONG
            PremiumForCurrency  Applicable only for Currency        LONG
            Reserved            Reserved for future
                                Default blank                       CHAR[100]

             */

            private Int32 cDataLength;
            public Int32 Prop01DataLenght
            {
                get { return cDataLength; }
                set { cDataLength = value; }
            }

            private char[] cCustomerID;
            public string Prop02CustomerID
            {
                get { return new string(cCustomerID); }
                set { cCustomerID = CUtility.GetPreciseArrayForString(value.ToString(), cCustomerID.Length); }
            }


            private Int32 cCCB;
            public Int32 Prop03CCB
            {
                get { return cCCB; }
                set { cCCB = value; }
            }

            private Int32 cWithDrawn;
            public Int32 Prop04WithDrawn
            {
                get { return cWithDrawn; }
                set { cWithDrawn = value; }
            }



            private Int32 cNCL;
            public Int32 Prop05NCL
            {
                get { return cNCL; }
                set { cNCL = value; }
            }

            private Int32 cMarginforComm;
            public Int32 Prop06MarginforComm
            {
                get { return cMarginforComm; }
                set { cMarginforComm = value; }
            }



            private Int32 cMMTMLoss;
            public Int32 Prop07MMTMLoss
            {
                get { return cMMTMLoss; }
                set { cMMTMLoss = value; }
            }


            private Int32 cBpl;
            public Int32 Prop08Bpl
            {
                get { return cBpl; }
                set { cBpl = value; }
            }


            private Int32 cHoldFunds;
            public Int32 Prop09HoldFunds
            {
                get { return cHoldFunds; }
                set { cHoldFunds = value; }
            }

            private Int32 cNseWithdrawlBal;
            public Int32 Prop10NseWithdrawlBal
            {
                get { return cNseWithdrawlBal; }
                set { cNseWithdrawlBal = value; }
            }
            private Int32 cPremiumForCurrency;
            public Int32 Prop11PremiumForCurrency
            {
                get { return cPremiumForCurrency; }
                set { cPremiumForCurrency = value; }
            }


            private char[] cReserved;
            public string Prop12Reserved
            {
                get { return new string(cReserved); }
                set { cReserved = CUtility.GetPreciseArrayForString(value.ToString(), cReserved.Length); }
            }

            public CommodityLimitReportItem(bool value)
            {
                cDataLength = 0;
                cCustomerID = new char[10];
                cCCB = 0;
                cWithDrawn = 0;
                cNCL = 0;
                cMarginforComm = 0;
                cMMTMLoss = 0;
                cBpl = 0;
                cHoldFunds = 0;
                cNseWithdrawlBal = 0;
                cPremiumForCurrency = 0;
                cReserved = new char[100];

            }

            public byte[] StructToByte()
            {
                throw new NotImplementedException();
            }

            public void ByteToStruct(byte[] ByteStructure)
            {

                Prop01DataLenght = BitConverter.ToInt32(ByteStructure, 4);
                Prop02CustomerID = Encoding.ASCII.GetString(ByteStructure, 4, 10);
                Prop03CCB = BitConverter.ToInt32(ByteStructure, 14); ;
                Prop04WithDrawn = BitConverter.ToInt32(ByteStructure, 18);
                Prop05NCL = BitConverter.ToInt32(ByteStructure, 22);
                Prop06MarginforComm = BitConverter.ToInt32(ByteStructure, 26);
                Prop07MMTMLoss = BitConverter.ToInt32(ByteStructure, 30);
                Prop08Bpl = BitConverter.ToInt32(ByteStructure, 34);
                Prop09HoldFunds = BitConverter.ToInt32(ByteStructure, 38);
                Prop10NseWithdrawlBal = BitConverter.ToInt32(ByteStructure, 42);
                Prop11PremiumForCurrency = BitConverter.ToInt32(ByteStructure, 46);
                Prop12Reserved = Encoding.ASCII.GetString(ByteStructure, 50, 100);

            }

            public override string ToString()
            {
                return Prop01DataLenght + "|" + Prop02CustomerID + "|" + Prop03CCB + "|" + Prop04WithDrawn + "|" + Prop05NCL + "|" + Prop06MarginforComm + "|" + Prop07MMTMLoss + "|" + Prop08Bpl + "|" + Prop09HoldFunds
                    + "|" + Prop10NseWithdrawlBal + "|" + Prop11PremiumForCurrency + "|" + Prop12Reserved;
            }
        }



        #endregion
      
    }
}
