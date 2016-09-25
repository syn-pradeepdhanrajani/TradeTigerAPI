using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using Sharekhan.RawSocket;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Windows;
using TradeTigerAPI;
using System.Xml;
using System.Xml.Linq;

namespace TradeTigerAPI
{
    public class CTAPClient
    {
        #region Initial Declaration
        public Socket socket;
        StateObject stateObj = null;
        StringBuilder sbOutput = new StringBuilder();
        private string Exchange;
        public string RemoteIP;
        public int RemotePort;
        public static Dictionary<string, CStructures.NFScripMaster> dicTokenvsNF;
        public static Dictionary<string, CStructures.NCScripMaster> dicNcScripMaster;
        public static Dictionary<string, CStructures.NCScripMaster> dicBcScripMaster;
        public static Dictionary<string, CStructures.NFScripMaster> dicNfScripMaster;
        public static Dictionary<string, CStructures.RNScripMaster> dicRNScripMaster;
        public static Dictionary<string, CStructures.RNScripMaster> dicRMScripMaster;
        public static Dictionary<string, CStructures.CommodityMaster> dicNXScripMaster;
        public static Dictionary<string, CStructures.CommodityMaster> dicMXScripMaster;

        public static Dictionary<string, CStructures.ExchangeTradeConfirmation> dicTradeConfirm = new Dictionary<string, CStructures.ExchangeTradeConfirmation>(); // tradeconfirmation dictionary

        public double CTCLSequenceNumber;
        public DateTime LoginDateTime;
        public Int32 LogTime;
        public Int32 LatestTime;
        public Int32 LastAckRecTime;
        public string LastDataRecTime;
        int bytesToRemove = 0;

        //InitialFlagVariable
        /// <summary>
        /// set true when Connected to Tap
        /// </summary>
        bool receiveData = false;//
        /// <summary>
        /// Onconnected to tap Set to 1
        /// </summary>
        public int TAPSequenceNumber = 0;

        /// <summary>
        /// Flag For processing
        /// </summary>
        public bool ProcessData = false;

        //int TAPSequenceNumber;
        public int DataReceived;


        /// <summary>
        /// to track No of Order Rejected 
        /// </summary>
        public int NewOrderRejected;
        public int ModifyOrderRejected;
        public int CancelOrderRejected;
        public int OrderAckReceived;
        public int SpreadAckReceived;
        public int TradeReceived;


        public FileStream FSData;
        public StreamWriter stwData;

        //Dictionary Used in :
        //Dictionary<int, CStructures.TokenAndEligibility> SecurityDetail;
        public int InvitationNo = 0;

        private bool bolLogin;
        public bool IsLogin
        {
            get { return bolLogin; }
            set { bolLogin = value; }
        }

        private bool bolConnected;
        public bool IsConnected
        {
            get { return bolConnected; }
            set { bolConnected = value; }
        }

        // system_info_request Instance
        // public CStructures.systemInfoResponse SystemInfo = new CStructures.systemInfoResponse(0);

        // Queue declaration
        public CQueue<byte[]> InData = new CQueue<byte[]>();//byte Queue
        public CQueue<byte[]> SendData = new CQueue<byte[]>();
        public CQueue<byte[]> LoginSendData = new CQueue<byte[]>();
        CQueue<int> lastAckTimeData = new CQueue<int>();
        CQueue<byte[]> tempSendData = new CQueue<byte[]>();

        Thread ThreadListenSocket = null; // Listen Socket Connection Status.
        // Threads For Indata and senddata
        Thread ThreadProcessInData = null;// Receive data 
        Thread ThreadProcessSendData = null;//Send Data 
        Thread ThreadProcessSendLoginData = null;// Request
        Thread ThreadDisconnect = null; // Disconnect
        //Thread ThreadLastAckSerialiser = null;// Serialisaion of Delta Acknowledge
        Thread ThreadUpdateUI = null;// update User Interface
        Thread ThreadUpdAckTime = null;

        Thread ThreadProcessXml = null;     // process Xml File .
        public static CQueue<string> QueueTrade = new CQueue<string>();


        // Manual Reset For Asynchronous Socket.
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent disconnectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        // Stream no 
        public static string StreamNo = null;

        public bool isConnection = false;
        public bool IsConnectionAvailable
        {
            get { return isConnection; }
            set { isConnection = value; }
        }

        // public CObject ConfObj;
        public bool processFalg = false;
        #endregion

        Action notifyDisconnectStatus;

        #region Asynchronous Socket Eveents -Connect,receive,Send,Disconnect

        public CTAPClient(Action socketTerminated)
        {
            notifyDisconnectStatus = socketTerminated;
        }

        public bool Connect()
        {
            try
            {
                IPAddress ipAddress = null;
                IPEndPoint remoteEP = null;
                ipAddress = IPAddress.Parse(CConstants.TapIp);
                remoteEP = new IPEndPoint(ipAddress, 8000);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveBufferSize = 2048;
                socket.SendBufferSize = 2048;
                //  socket.Ttl = 128;           
                socket.ReceiveTimeout = 0;
                socket.SendTimeout = 0;

                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1);
                LogFile.Reference.WriteLogFile("Connection ", " Connection started On  IP : " + ipAddress + "  Port: " + 8000 + "");
                try
                {
                    Connect(remoteEP);
                }
                catch (Exception ex)
                {
                    LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                return false;
            }
            return true;
        }

        public void ListenSocket()
        {
            // s.Poll returns true if 
            // connection is closed, reset, terminated or pending (meaning no active connection)
            // connection is active and there is data available for reading

            // s.Available returns number of bytes available for reading
            // if both are true: 
            // there is no data available to read so connection is not active

            while (true)
            {
                try
                {
                    if (socket != null)
                    {
                        bool ConnectionAvail = socket.Poll(1000, SelectMode.SelectRead);
                        if (socket != null && ConnectionAvail)
                        {
                            bool DataAvailable = (socket.Available == 0);
                            if (!socket.Connected & ConnectionAvail & DataAvailable)
                                isConnection = false;
                            else
                                isConnection = true;
                        }
                        else
                            isConnection = false;
                    }
                    else
                        isConnection = false;
                }
                catch (Exception ex)
                {
                    LogFile.Reference.WriteLogFile("" + ex.StackTrace.ToString(), ex.Message.ToString());
                }
            }
        }

        public void Connect(EndPoint remoteEP)
        {
            try
            {
                socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), socket);
                connectDone.WaitOne();
            }
            catch (SocketException ex)
            {
                OnConnected(ex.SocketErrorCode.ToString(), "SocketException " + ex.NativeErrorCode.ToString() + ex.Message);
            }
            catch (Exception ex)
            {
                OnConnected(ex.Data.GetHashCode().ToString(), "Exception " + ex.Data.GetHashCode().ToString() + ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);


                MainWindow.logOutputQueue.EnQueue("TT Connected...");
                connectDone.Set();

                receiveData = true;
                OnConnected("", "");
                if (client.Connected == true)
                    Receive();
            }
            catch (SocketException ex)
            {
                MainWindow.logOutputQueue.EnQueue("Unable to Connect...");
                OnConnected(ex.SocketErrorCode.ToString(), "SocketException " + ex.NativeErrorCode.ToString() + ex.Message);
                connectDone.Set();
            }
            catch (Exception ex)
            {
                MainWindow.logOutputQueue.EnQueue("Unable to Connect...");
                OnConnected(ex.Data.GetHashCode().ToString(), "Exception " + ex.Data.GetHashCode().ToString() + ex.Message);
                connectDone.Set();
            }
        }

        private void OnConnected(string status, string discription)
        {
            try
            {

                TAPSequenceNumber = 1;
                InData.PacketsCanSend = 1;
                SendData.PacketsCanSend = 0;
                LoginSendData.PacketsCanSend = 0;
                DataReceived = 0;
                NewOrderRejected = 0;
                ModifyOrderRejected = 0;
                CancelOrderRejected = 0;
                OrderAckReceived = 0;
                SpreadAckReceived = 0;
                TradeReceived = 0;
                ProcessData = true;
                bolLogin = true;
                bolConnected = false;

                StartThreads();
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        private void Receive()
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = socket;
                if (socket.Connected == true)
                    socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                // receiveDone.WaitOne();
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                stateObj = (StateObject)ar.AsyncState;
                int bytesRead = stateObj.workSocket.EndReceive(ar);
                // check if more data is there 
                if (bytesRead > 0)
                {
                    byte[] data = new byte[bytesRead];
                    data = CUtility.GetByteArrayFromStructure(stateObj.buffer, 0, bytesRead);

                    InData.EnQueue(data);
                    InData.AddCounter(1);
                    Interlocked.Increment(ref DataReceived);
                    socket.BeginReceive(stateObj.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), stateObj);
                    // receiveDone.WaitOne();
                }
                else
                {

                    receiveData = false;
                    receiveDone.Set();
                    Disconnect();
                }
            }
            catch (SocketException ex)
            {

                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                receiveData = false;
                receiveDone.Set();
                Disconnect();
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                receiveData = false;
                receiveDone.Set();
                Disconnect();
            }
        }

        private void Send(Socket client, byte[] byteData)
        {
            try
            {

                processFalg = true;
                socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                processFalg = false;
            }
            catch (ArgumentNullException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (SocketException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (ObjectDisposedException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (InvalidOperationException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                //Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                sendDone.Set();
            }
            catch (ArgumentNullException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (ArgumentException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (SocketException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (ObjectDisposedException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (InvalidOperationException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        public void Disconnect()
        {
            try
            {
                LogFile.Reference.WriteLogFile("Disconnect()", "In Disconnect()");
                receiveData = false;
                Disconnect(socket);
            }
            catch (NullReferenceException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            finally
            {

                if (ThreadDisconnect != null)
                    StopThread(ref ThreadDisconnect);
                StartThread(ref ThreadDisconnect, "Disconnect");
                socket = null;
            }

            notifyDisconnectStatus();
        }

        private void Disconnect(Socket client)
        {
            try
            {
                if (client.Connected)
                {
                    StateObject state = new StateObject();
                    state.workSocket = client;
                    client.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), state);
                    // disconnectDone.WaitOne(); 
                }
            }
            catch (SocketException ex)
            {
                OnDisconnected(ex.SocketErrorCode.ToString(), "SocketException " + ex.NativeErrorCode.ToString() + ex.Message);
                disconnectDone.Set();

            }
            catch (Exception ex)
            {
                OnDisconnected(ex.Data.GetHashCode().ToString(), "Exception " + ex.Data.GetHashCode().ToString() + ex.Message);
                disconnectDone.Set();
            }
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                state.workSocket.EndDisconnect(ar);
                //socket.EndDisconnect(ar);

                // FnoCtclLib.Reference.LogInfo("Socket disconnected.");
                disconnectDone.Set();
                socket.Close();// Timeout 1 second

            }
            catch (SocketException ex)
            {
                OnDisconnected(ex.SocketErrorCode.ToString(), "SocketException " + ex.NativeErrorCode.ToString() + ex.Message);
                disconnectDone.Set();
                LogFile.Reference.WriteLogFile("Disconnected :" + ex.StackTrace.ToString(), ex.Message.ToString());

            }
            catch (Exception ex)
            {
                OnDisconnected(ex.Data.GetHashCode().ToString(), "Exception " + ex.Data.GetHashCode().ToString() + ex.Message);
                disconnectDone.Set();
                LogFile.Reference.WriteLogFile("Disconnected :" + ex.StackTrace.ToString(), ex.Message.ToString());
            }
            finally
            {
                //FnoCtclLib.Reference.Reconnect = true;
            }
        }

        void OnDisconnected(string status, string discription)
        {
            try
            {
                if (status.Equals(""))
                {

                    ProcessData = false;
                    bolLogin = false;
                    bolConnected = false;

                    TAPSequenceNumber = 1;
                    //InData.PacketsCanSend = 1;
                    SendData.PacketsCanSend = 0;
                    LoginSendData.PacketsCanSend = 0;
                    // Reset UI Flags 
                    DataReceived = 0;
                    NewOrderRejected = 0;//No of Rejected Order 
                    ModifyOrderRejected = 0;
                    CancelOrderRejected = 0;
                    OrderAckReceived = 0;
                    SpreadAckReceived = 0;
                    TradeReceived = 0;

                    LogFile.Reference.WriteLogFile(" Thread Disconnected Call. ", "Disconnected Connection !!!!!");
                    StopThreads();
                    
                    //Connect();
                }
                else
                {

                    LogFile.Reference.WriteLogFile("OnDisconnected ", "Disconnected Connection ..........");
                    
                   // Connect();
                }

            }
            catch (ThreadAbortException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        #endregion

        #region Threads Methods

        void StartThreads()
        {
            try
            {
                if (ThreadListenSocket != null)
                    StopThread(ref ThreadListenSocket);

                if (ThreadProcessInData != null)
                    StopThread(ref ThreadProcessInData);
                if (ThreadProcessSendData != null)
                    StopThread(ref ThreadProcessSendData);
                if (ThreadProcessSendLoginData != null)
                    StopThread(ref ThreadProcessSendLoginData);

                if (ThreadUpdateUI != null)
                    StopThread(ref ThreadUpdateUI);
                if (ThreadUpdAckTime != null)
                    StopThread(ref ThreadUpdAckTime);

                StartThread(ref ThreadListenSocket, "ListenSocket");
                StartThread(ref ThreadProcessInData, "InData");
                StartThread(ref ThreadProcessSendData, "SendData");
                StartThread(ref ThreadProcessSendLoginData, "SendLoginData");
                //TODO: Pradeep - Commenting to check on error... //StartThread(ref ThreadProcessXml, "ProcessXml");

            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        void StopThreads()
        {
            try
            {
                LogFile.Reference.WriteLogFile("Stop thread called ", "");
                StopThread(ref ThreadProcessInData);
                StopThread(ref ThreadProcessSendData);
                StopThread(ref ThreadProcessSendLoginData);
                StopThread(ref ThreadProcessXml);
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        void StartThread(ref Thread tProcess, string threadName)
        {
            try
            {
                switch (threadName)
                {
                    case "ListenSocket":
                        tProcess = new Thread(new ThreadStart(ListenSocket));
                        break;
                    case "InData":
                        tProcess = new Thread(new ThreadStart(ThreadParseData));
                        break;
                    case "SendData":
                        tProcess = new Thread(new ThreadStart(ThreadSendData));
                        break;
                    case "SendLoginData":
                        tProcess = new Thread(new ThreadStart(ThreadSendLoginData));
                        break;
                    case "Disconnect":
                        tProcess = new Thread(new ThreadStart(ThreadCallDisconnect));
                        break;

                    case "ProcessXml":
                        tProcess = new Thread(new ThreadStart(ProcessXml));
                        break;
                }

                tProcess.Name = threadName;
                tProcess.IsBackground = true;
                tProcess.Start();
            }
            catch (ThreadStateException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (InvalidOperationException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (System.Security.SecurityException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (OutOfMemoryException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (ThreadAbortException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        void StopThread(ref Thread tProcess)
        {
            if (tProcess == null)
                return;

            try
            {
                tProcess.Abort();
                tProcess.Join();
            }
            catch (System.Security.SecurityException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (ThreadStateException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (ThreadAbortException ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            finally
            {
                tProcess = null;
            }
        }

        void ThreadCallDisconnect()
        {
            try
            {
                Thread.Sleep(10);
                OnDisconnected("", "");
                LogFile.Reference.WriteLogFile("ThreadCallDisconnect", " Thread Disconnected...");
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
            finally
            {
                ThreadDisconnect = null;
            }
        }

        /// <summary>
        /// Send data to Tap Server 
        /// </summary>
        private void ThreadSendData()
        {
            while (ProcessData)
            {
                try
                {
                    if (processFalg == false && SendData.GetSize() > 0)
                    {
                        byte[] data = SendData.DeQueue(true);// Overloaded Method Implemented

                        Send(socket, data);
                        // FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Data Send to Tap Server.Length: " + data.Length.ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                }
            }
        }

        private void ThreadSendLoginData()
        {
            while (ProcessData)
            {
                try
                {
                    byte[] data = LoginSendData.DeQueue(true);
                    Send(socket, data); // socket.Send(data);                   
                }
                catch (Exception ex)
                {
                    LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                }
            }
        }

        private void ThreadParseData()
        {
            byte[] data;
            byte[] PartialBuffer = new byte[0]; //To store partial data
            byte[] TotalBuffer = new byte[0];   // To store complete data
            try
            {
                while (ProcessData)
                {


                    if (bytesToRemove > 0 || PartialBuffer.Length > 0)
                    {
                        Array.Resize(ref TotalBuffer, PartialBuffer.Length);
                        Array.Copy(PartialBuffer, 0, TotalBuffer, 0, PartialBuffer.Length);
                        bytesToRemove = 0;
                        Array.Resize(ref PartialBuffer, 0);
                    }
                    //check data before computing 
                    if (CheckCompletePacket(TotalBuffer) == false)
                    {
                        data = InData.DeQueue();
                        int destinationIndex = TotalBuffer.Length;
                        Array.Resize(ref TotalBuffer, TotalBuffer.Length + data.Length);
                        Array.Copy(data, 0, TotalBuffer, destinationIndex, data.Length);
                    }
                    else
                    {
                        byte[] MessageHeader = new byte[CConstants.MsgHeaderSize];
                        Array.ConstrainedCopy(TotalBuffer, 0, MessageHeader, 0, CConstants.MsgHeaderSize);

                        CStructures.MessageHeader header = new CStructures.MessageHeader();
                        header.ByteToStruct(MessageHeader);

                        switch (header.Prop02TransactionCode)
                        {
                            case 1:
                                CStructures.LoginResponse loginResponse = new CStructures.LoginResponse(1);
                                loginResponse.ByteToStruct(TotalBuffer);
                                if (loginResponse.Prop02StatusCode == 0)
                                    MainWindow.isSuccess = true;
                                else
                                    MainWindow.isSuccess = false;

                                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + loginResponse.ToString()));

                                break;
                            case 2:
                                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "LogOff done Successfully"));
                                break;
                            case 21:
                                CStructures.ScripMasterResponse scripMasterResponse = new CStructures.ScripMasterResponse(1);
                                scripMasterResponse.ByteToStruct(TotalBuffer);
                                int numberOfScrips;
                                int sourceIndex = 0;

                                #region Scripmaster download
                                switch (scripMasterResponse.Prop02ExchangeCode)
                                {
                                    case "NF":
                                        byte[] nfData = new byte[header.Prop01MessageLength - 8];
                                        Array.ConstrainedCopy(TotalBuffer, 8, nfData, 0, nfData.Length);

                                        CStructures.NFScripMaster nfScripMaster;
                                        numberOfScrips = nfData.Length / CConstants.DerivativeMasterItemSize;
                                        dicNfScripMaster = new Dictionary<string, CStructures.NFScripMaster>();
                                        dicTokenvsNF = new Dictionary<string, CStructures.NFScripMaster>();
                                        for (int i = 0; i < numberOfScrips; i++)
                                        {
                                            nfScripMaster = new CStructures.NFScripMaster(true);
                                            byte[] nfScripData = new byte[CConstants.DerivativeMasterItemSize];
                                            Array.ConstrainedCopy(nfData, sourceIndex, nfScripData, 0, CConstants.DerivativeMasterItemSize);
                                            nfScripMaster.ByteToStruct(nfScripData);
                                            if (!dicNfScripMaster.ContainsKey(nfScripMaster.Prop04ScripShortName))
                                            {
                                                dicNfScripMaster.Add(nfScripMaster.Prop04ScripShortName.Replace("\0", "").Trim(), nfScripMaster);
                                            }
                                            sourceIndex += CConstants.DerivativeMasterItemSize;
                                        }
                                        MainWindow.LoadScripmaster(dicNfScripMaster, scripMasterResponse.Prop02ExchangeCode);
                                        MainWindow.logOutputQueue.EnQueue(string.Format("\n" + scripMasterResponse.ToString() + "|" + dicNfScripMaster.FirstOrDefault().Value.ToString()));
                                        break;
                                    case "NC":
                                    case "BC":
                                        byte[] ncData = new byte[header.Prop01MessageLength - 8];
                                        Array.ConstrainedCopy(TotalBuffer, 8, ncData, 0, ncData.Length);

                                        CStructures.NCScripMaster ncScripMaster;
                                        numberOfScrips = ncData.Length / CConstants.CashcripMasterSize;
                                        sourceIndex = 0;

                                        dicNcScripMaster = new Dictionary<string, CStructures.NCScripMaster>();
                                        dicBcScripMaster = new Dictionary<string, CStructures.NCScripMaster>();
                                        for (int i = 0; i < numberOfScrips; i++)
                                        {
                                            ncScripMaster = new CStructures.NCScripMaster(true);
                                            byte[] ncScripData = new byte[CConstants.CashcripMasterSize];
                                            Array.ConstrainedCopy(ncData, sourceIndex, ncScripData, 0, CConstants.CashcripMasterSize);
                                            ncScripMaster.ByteToStruct(ncScripData);

                                            if (scripMasterResponse.Prop02ExchangeCode == "NC")
                                            {
                                                if (!dicNcScripMaster.ContainsKey(ncScripMaster.Prop04ScripShortName))
                                                    dicNcScripMaster.Add(ncScripMaster.Prop04ScripShortName, ncScripMaster);
                                            }
                                            else
                                            {
                                                if (!dicBcScripMaster.ContainsKey(ncScripMaster.Prop04ScripShortName))
                                                    dicBcScripMaster.Add(ncScripMaster.Prop04ScripShortName, ncScripMaster);
                                            }
                                            sourceIndex += CConstants.CashcripMasterSize;

                                        }
                                        if (scripMasterResponse.Prop02ExchangeCode == "NC")
                                        {
                                            MainWindow.LoadScripmaster(dicNcScripMaster, scripMasterResponse.Prop02ExchangeCode);
                                            MainWindow.logOutputQueue.EnQueue(string.Format("\n" + scripMasterResponse.ToString() + "|" + dicNcScripMaster.FirstOrDefault().Value.ToString()));
                                        }
                                        else
                                        {
                                            MainWindow.LoadScripmaster(dicBcScripMaster, scripMasterResponse.Prop02ExchangeCode);
                                            MainWindow.logOutputQueue.EnQueue(string.Format("\n" + scripMasterResponse.ToString() + "|" + dicBcScripMaster.FirstOrDefault().Value.ToString()));
                                        }
                                        break;

                                    case "RN":
                                    case "RM":
                                        byte[] rnData = new byte[header.Prop01MessageLength - 8];
                                        Array.ConstrainedCopy(TotalBuffer, 8, rnData, 0, rnData.Length);

                                        CStructures.RNScripMaster currencycripMaster;
                                        numberOfScrips = rnData.Length / CConstants.CurrencycripMasterSize;
                                        sourceIndex = 0;

                                        dicRNScripMaster = new Dictionary<string, CStructures.RNScripMaster>();
                                        dicRMScripMaster = new Dictionary<string, CStructures.RNScripMaster>();
                                        for (int i = 0; i < numberOfScrips; i++)
                                        {
                                            currencycripMaster = new CStructures.RNScripMaster(true);
                                            byte[] scripData = new byte[CConstants.CurrencycripMasterSize];
                                            Array.ConstrainedCopy(rnData, sourceIndex, scripData, 0, CConstants.CurrencycripMasterSize);
                                            currencycripMaster.ByteToStruct(scripData);
                                            if (scripMasterResponse.Prop02ExchangeCode == "RN")
                                            {
                                                if (!dicRNScripMaster.ContainsKey(currencycripMaster.Prop04ScripShortName))
                                                    dicRNScripMaster.Add(currencycripMaster.Prop04ScripShortName, currencycripMaster);
                                            }
                                            else
                                            {
                                                if (!dicRMScripMaster.ContainsKey(currencycripMaster.Prop04ScripShortName))
                                                    dicRMScripMaster.Add(currencycripMaster.Prop04ScripShortName, currencycripMaster);
                                            }
                                            sourceIndex += CConstants.CurrencycripMasterSize;
                                        }
                                        if (scripMasterResponse.Prop02ExchangeCode == "RN")
                                        {
                                            MainWindow.LoadScripmaster(dicRNScripMaster, scripMasterResponse.Prop02ExchangeCode);
                                            MainWindow.logOutputQueue.EnQueue(string.Format("\n" + scripMasterResponse.ToString() + "|" + dicRNScripMaster.FirstOrDefault().Value.ToString()));
                                        }
                                        else
                                        {
                                            MainWindow.LoadScripmaster(dicRMScripMaster, scripMasterResponse.Prop02ExchangeCode);
                                            MainWindow.logOutputQueue.EnQueue(string.Format("\n" + scripMasterResponse.ToString() + "|" + dicRMScripMaster.FirstOrDefault().Value.ToString()));
                                        }

                                        break;
                                    case "NX":
                                    case "MX":

                                        byte[] commodityData = new byte[header.Prop01MessageLength - 8];
                                        Array.ConstrainedCopy(TotalBuffer, 8, commodityData, 0, commodityData.Length);

                                        CStructures.CommodityMaster commoScripMaster;
                                        numberOfScrips = commodityData.Length / CConstants.CommodityMasterSize;
                                        sourceIndex = 0;

                                        dicNXScripMaster = new Dictionary<string, CStructures.CommodityMaster>();
                                        dicMXScripMaster = new Dictionary<string, CStructures.CommodityMaster>();
                                        for (int i = 0; i < numberOfScrips; i++)
                                        {
                                            commoScripMaster = new CStructures.CommodityMaster(true);
                                            byte[] nxScripData = new byte[CConstants.CommodityMasterSize];
                                            Array.ConstrainedCopy(commodityData, sourceIndex, nxScripData, 0, CConstants.CommodityMasterSize);
                                            commoScripMaster.ByteToStruct(nxScripData);
                                            if (scripMasterResponse.Prop02ExchangeCode == "NX")
                                            {
                                                if (!dicNXScripMaster.ContainsKey(commoScripMaster.Prop03ScripShortName))
                                                    dicNXScripMaster.Add(commoScripMaster.Prop03ScripShortName, commoScripMaster);
                                            }
                                            else
                                            {
                                                if (!dicMXScripMaster.ContainsKey(commoScripMaster.Prop03ScripShortName))
                                                    dicMXScripMaster.Add(commoScripMaster.Prop03ScripShortName, commoScripMaster);
                                            }
                                            sourceIndex += CConstants.CommodityMasterSize;
                                        }
                                        if (scripMasterResponse.Prop02ExchangeCode == "NX")
                                        {
                                            MainWindow.LoadScripmaster(dicNXScripMaster, scripMasterResponse.Prop02ExchangeCode);
                                            MainWindow.logOutputQueue.EnQueue(string.Format("\n" + scripMasterResponse.ToString() + "|" + dicMXScripMaster.FirstOrDefault().Value.ToString()));
                                        }
                                        else
                                        {
                                            MainWindow.LoadScripmaster(dicMXScripMaster, scripMasterResponse.Prop02ExchangeCode);
                                            MainWindow.logOutputQueue.EnQueue(string.Format("\n" + scripMasterResponse.ToString() + "|" + dicNXScripMaster.FirstOrDefault().Value.ToString()));
                                        }

                                        break;
                                }
                                #endregion
                                break;
                            case 22:
                                CStructures.FeedResponse feedResponse = new CStructures.FeedResponse(true);
                                feedResponse.ByteToStruct(TotalBuffer);
                                string token = feedResponse.Prop03ScripToken;



                                if (!MainWindow.DicFeedsRespone.ContainsKey(Convert.ToInt32(feedResponse.Prop03ScripToken.Replace("\0", ""))))
                                {
                                    MainWindow.DicFeedsRespone.Add(Convert.ToInt32(feedResponse.Prop03ScripToken.Replace("\0", "")), feedResponse);
                                    try
                                    {
                                        MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Feed Response :" + feedResponse.ToString().Replace("\0", "").Replace("SegmentId = }7\t|","").Replace("SegmentId = {\u001d\u0001|","")));
                                    }
                                    catch (Exception ex)
                                    {
                                        LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), feedResponse.ToString());
                                    }
                                }
                                break;
                            case 26:
                                CStructures.MarketDepthResponse depthResponse = new CStructures.MarketDepthResponse(true);
                                depthResponse.ByteToStruct(TotalBuffer);

                                if (!MainWindow.DicMarketDepthResponse.ContainsKey(Convert.ToInt32(depthResponse.Prop05ScripCode)))
                                {
                                    MainWindow.DicMarketDepthResponse.Add(Convert.ToInt32(depthResponse.Prop05ScripCode), depthResponse);
                                    MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Depth Response :" + depthResponse.ToString()));
                                }
                                break;
                            case 11:
                                CStructures.SharekhanOrderConfirmation soc = new CStructures.SharekhanOrderConfirmation(true);
                                soc.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "SharekhanOrderConfirmation : " + soc.ToString()));

                                if (!MainWindow.DicSharekhanOrderResponse.ContainsKey(soc.Prop02RequestID))
                                    MainWindow.DicSharekhanOrderResponse.Add(soc.Prop02RequestID, soc);

                                if (MainWindow.dicReqIDTrack.ContainsKey(soc.Prop02RequestID.Replace("\0", "").Trim()))
                                {
                                    lock(MainWindow.dicReqIDTrack[soc.Prop02RequestID.Replace("\0", "").Trim()])
                                    {
                                    MainWindow.orders order = MainWindow.dicReqIDTrack[soc.Prop02RequestID.Replace("\0", "").Trim()];


                                    List<CStructures.OrderConfirmationItem> objConfirmation = soc.Prop05OrderConfirmationItems;
                                    order.SharuID = objConfirmation[0].Prop04SharekhanOrderID.Replace("\0", "").Trim();


                                    order.ConfrmType = "SharekhanConfirmation";


                                    MainWindow.dicSharekhanIDvsAPIReqID.Add(order.SharuID, order.APIReqID.Replace("\0", "").Trim());
                                    // MainWindow.QueueAmbiOrd.EnQueue(objSharAmbi.SharekhanID);
                                    LogFile.Reference.WriteLogFile("SharekhanConfirm : ", order.ToString());
                                    // TradeTigerAPI.LogFile.Reference.WriteLogFile();
                                  }
                                }

                                break;
                            case 13:
                                CStructures.ExchangeTradeConfirmation orderConfirmation = new CStructures.ExchangeTradeConfirmation(true);
                                orderConfirmation.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "ExchangeOrderConfirmation : " + orderConfirmation.ToString()));

                                if (MainWindow.dicSharekhanIDvsAPIReqID.ContainsKey(orderConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim()))
                                {
                                    string APIReqID = MainWindow.dicSharekhanIDvsAPIReqID[orderConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim()];
                                    lock (MainWindow.dicReqIDTrack[APIReqID])
                                    {
                                        MainWindow.orders order = MainWindow.dicReqIDTrack[APIReqID];
                                        order.ExchangeOrdID = orderConfirmation.Prop06ExchangeOrderId.Replace("\0", "").Trim();
                                        order.ExchangeSignal = orderConfirmation.Prop11BuySell;

                                        order.ConfrmType = "ExchangeConfirmation";
                                        LogFile.Reference.WriteLogFile("ExchangeConfirmation : ", order.ToString());
                                    }
                                    
                                }
                                break;
                            case 14:
                                CStructures.ExchangeTradeConfirmation tradeConfirmation = new CStructures.ExchangeTradeConfirmation(true);
                                tradeConfirmation.ByteToStruct(TotalBuffer);

                                lock (dicTradeConfirm)
                                {
                                    if (!dicTradeConfirm.ContainsKey(tradeConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim()))
                                        dicTradeConfirm.Add(tradeConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim(), tradeConfirmation);
                                    else
                                        dicTradeConfirm[tradeConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim()] = tradeConfirmation;

                                    QueueTrade.EnQueue(tradeConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim());
                                }

                                break;


                            case 31:
                                CStructures.ReportResponse objTransCode31 = new CStructures.ReportResponse(true);
                                objTransCode31.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode31.ToString());

                                for (int i = 0; i < objTransCode31.Prop02RecordCount; i++)
                                {
                                    MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                    CStructures.EquityOrderReportItem ReportResponse = new CStructures.EquityOrderReportItem(true);
                                    ReportResponse.ByteToStruct(TotalBuffer);
                                    MainWindow.logOutputQueue.EnQueue(ReportResponse.ToString());
                                }
                                //string token = feedResponse.Prop03ScripToken;
                                //if (!MainWindow.DicFeedsRespone.ContainsKey(Convert.ToInt32(feedResponse.Prop03ScripToken)))
                                //{
                                //    MainWindow.DicFeedsRespone.Add(Convert.ToInt32(feedResponse.Prop03ScripToken), feedResponse);

                                //}
                                break;
                            case 32:
                                CStructures.ReportResponse objTransCode32 = new CStructures.ReportResponse(true);
                                objTransCode32.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode32.ToString());

                                for (int i = 0; i < objTransCode32.Prop02RecordCount; i++)
                                {
                                    MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                    CStructures.DPSRReportItem DPSRReport = new CStructures.DPSRReportItem(true);
                                    DPSRReport.ByteToStruct(TotalBuffer);
                                    MainWindow.logOutputQueue.EnQueue(DPSRReport.ToString());
                                }
                                break;
                            case 33:
                                #region  CashOrderDetailsReportItem
                                CStructures.ReportResponse objTransCode33 = new CStructures.ReportResponse(true);
                                objTransCode33.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode33.ToString());

                                int cashOrderStart = 0;
                                int cashOrderEnd = 0;
                                if (objTransCode33.Prop02RecordCount > 0)
                                {
                                    for (int i = 0; i < objTransCode33.Prop02RecordCount; i++)
                                    {
                                        int FixSize = 461;  // Cash Order Detail Size
                                        if (i == 0)
                                        {
                                            cashOrderStart = 10;
                                            cashOrderEnd = 10 + FixSize;
                                        }
                                        else
                                        {
                                            cashOrderStart = cashOrderEnd;
                                            cashOrderEnd = cashOrderStart + FixSize;
                                        }
                                        byte[] Report = new byte[FixSize]; //To store partial data

                                        Array.Copy(TotalBuffer, cashOrderStart, Report, 0, FixSize);


                                        MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                        CStructures.CashOrderDetailsReportItem CashOrderDetailsReportItemResponse = new CStructures.CashOrderDetailsReportItem(true);
                                        CashOrderDetailsReportItemResponse.ByteToStruct(Report);
                                        MainWindow.logOutputQueue.EnQueue(CashOrderDetailsReportItemResponse.ToString());
                                    }
                                }
                                #endregion
                                break;
                            case 34:
                                #region CashTradeDetailsReportItem
                                CStructures.ReportResponse objTransCode34 = new CStructures.ReportResponse(true);
                                objTransCode34.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode34.ToString());

                                int cashtradeStart = 0;
                                int cashtradeEnd = 0;
                                if (objTransCode34.Prop02RecordCount > 0)
                                {
                                    for (int i = 0; i < objTransCode34.Prop02RecordCount; i++)
                                    {

                                        int FixSize = 294;  // Cash Order Detail Size
                                        if (i == 0)
                                        {
                                            cashtradeStart = 10;
                                            cashtradeEnd = 10 + FixSize;
                                        }
                                        else
                                        {
                                            cashtradeStart = cashtradeEnd + 100;
                                            cashOrderEnd = cashtradeStart + FixSize + 100;
                                        }
                                        byte[] Report = new byte[FixSize]; //To store partial data

                                        Array.Copy(TotalBuffer, cashtradeStart, Report, 0, FixSize);


                                        MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                        CStructures.CashTradeDetailsReportItem CashTradeDetailsReport = new CStructures.CashTradeDetailsReportItem(true);
                                        CashTradeDetailsReport.ByteToStruct(Report);
                                        MainWindow.logOutputQueue.EnQueue(CashTradeDetailsReport.ToString());
                                    }
                                }
                                #endregion
                                break;
                            case 35:
                                CStructures.ReportResponse objTransCode35 = new CStructures.ReportResponse(true);
                                objTransCode35.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode35.ToString());

                                for (int i = 0; i < objTransCode35.Prop02RecordCount; i++)
                                {
                                    MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                    CStructures.CashLimitReportItem CashLimitReportItem = new CStructures.CashLimitReportItem(true);
                                    CashLimitReportItem.ByteToStruct(TotalBuffer);
                                    MainWindow.logOutputQueue.EnQueue(CashLimitReportItem.ToString());
                                }
                                break;
                            case 36:
                                CStructures.ReportResponse objTransCode36 = new CStructures.ReportResponse(true);
                                objTransCode36.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode36.ToString());

                                for (int i = 0; i < objTransCode36.Prop02RecordCount; i++)
                                {
                                    MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                    CStructures.CashNetPositionReportItem CashNetPositionReportItem = new CStructures.CashNetPositionReportItem(true);
                                    CashNetPositionReportItem.ByteToStruct(TotalBuffer);
                                    MainWindow.logOutputQueue.EnQueue(CashNetPositionReportItem.ToString());
                                }
                                break;
                            case 41:

                                CStructures.ReportResponse objTransCode41 = new CStructures.ReportResponse(true);
                                objTransCode41.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode41.ToString());
                                for (int i = 0; i < objTransCode41.Prop02RecordCount; i++)
                                {
                                    MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                    CStructures.DerivativeOrderReportItem DerivativeOrderReportItem = new CStructures.DerivativeOrderReportItem(true);
                                    DerivativeOrderReportItem.ByteToStruct(TotalBuffer);
                                    MainWindow.logOutputQueue.EnQueue(DerivativeOrderReportItem.ToString());
                                }
                                break;
                            case 42:
                                CStructures.ReportResponse objTransCode42 = new CStructures.ReportResponse(true);
                                objTransCode42.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode42.ToString());

                                for (int i = 0; i < objTransCode42.Prop02RecordCount; i++)
                                {
                                    MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                    CStructures.TurnOverReportItem obj = new CStructures.TurnOverReportItem(true);
                                    obj.ByteToStruct(TotalBuffer);
                                    MainWindow.logOutputQueue.EnQueue(obj.ToString());
                                }
                                break;
                            case 43:
                                #region  DerivativeOrderDetailReportItem
                                CStructures.ReportResponse objTransCode43 = new CStructures.ReportResponse(true);
                                objTransCode43.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode43.ToString());


                                int Reportstartind = 0;
                                int Reortendind = 0;
                                if (objTransCode43.Prop02RecordCount > 0)
                                {
                                    for (int i = 0; i < objTransCode43.Prop02RecordCount; i++)
                                    {
                                        int FixSize = 764;
                                        if (i == 0)
                                        {
                                            Reportstartind = 10;
                                            Reortendind = 10 + FixSize;
                                        }
                                        else
                                        {
                                            Reportstartind = Reortendind;
                                            Reortendind = Reortendind + FixSize;
                                        }
                                        byte[] Report = new byte[FixSize]; //To store partial data

                                        Array.Copy(TotalBuffer, Reportstartind, Report, 0, FixSize);

                                        MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                        CStructures.DerivativeOrderDetailReportItem objDerOrdDetailReport = new CStructures.DerivativeOrderDetailReportItem(true);
                                        objDerOrdDetailReport.ByteToStruct(Report);
                                        MainWindow.logOutputQueue.EnQueue(objDerOrdDetailReport.ToString());
                                    }
                                }
                                #endregion
                                break;
                            case 44:
                                #region   DerivativeTradeDetailsReportItem
                                CStructures.ReportResponse objTransCode44 = new CStructures.ReportResponse(true);
                                objTransCode44.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode44.ToString());

                                int fnotradeStart = 0;
                                int fnotradeEnd = 0;
                                if (objTransCode44.Prop02RecordCount > 0)
                                {
                                    for (int i = 0; i < objTransCode44.Prop02RecordCount; i++)
                                    {

                                        int FixSize = 364;  // FNO Trade Detail Size
                                        if (i == 0)
                                        {
                                            fnotradeStart = 10;
                                            fnotradeEnd = 10 + FixSize;
                                        }
                                        else
                                        {
                                            fnotradeStart = fnotradeEnd + 100;
                                            fnotradeEnd = fnotradeStart + FixSize + 100;
                                        }
                                        byte[] Report = new byte[FixSize]; //To store partial data

                                        Array.Copy(TotalBuffer, fnotradeStart, Report, 0, FixSize);

                                        MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                        CStructures.DerivativeTradeDetailsReportItem objDerTradeDetail = new CStructures.DerivativeTradeDetailsReportItem(true);
                                        objDerTradeDetail.ByteToStruct(Report);
                                        MainWindow.logOutputQueue.EnQueue(objDerTradeDetail.ToString());
                                    }
                                }
                                #endregion
                                break;
                            case 49:
                            case 54:
                                CStructures.ReportResponse objTransCode49 = new CStructures.ReportResponse(true);
                                objTransCode49.ByteToStruct(TotalBuffer);
                                MainWindow.logOutputQueue.EnQueue(objTransCode49.ToString());

                                for (int i = 0; i < objTransCode49.Prop02RecordCount; i++)
                                {
                                    MainWindow.logOutputQueue.EnQueue("Record Number : " + i.ToString() + "\n");
                                    CStructures.CommodityLimitReportItem objcmdLimit = new CStructures.CommodityLimitReportItem(true);
                                    objcmdLimit.ByteToStruct(TotalBuffer);
                                    MainWindow.logOutputQueue.EnQueue(objcmdLimit.ToString());
                                }
                                break;

                        }
                        if (header.Prop01MessageLength < TotalBuffer.Length)
                        {
                            bytesToRemove = header.cMessageLength;
                            Array.Resize(ref PartialBuffer, TotalBuffer.Length - header.Prop01MessageLength);
                            Array.Copy(TotalBuffer, bytesToRemove, PartialBuffer, 0, PartialBuffer.Length);
                        }

                        Array.Resize(ref TotalBuffer, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        public void ProcessXml()
        {
            while (true)
            {
                try
                {
                    string Tradeconfirm = QueueTrade.DeQueue();
                    MainWindow.logOutputQueue.EnQueue("Queue xml: " + Tradeconfirm);
                    if (MainWindow.dicReqIDTrack.ContainsKey(Tradeconfirm))
                    {
                        lock (MainWindow.dicReqIDTrack[Tradeconfirm])
                        {
                            MainWindow.orders obj = MainWindow.dicReqIDTrack[Tradeconfirm];
                            MainWindow.InsertUpdateOrdertable(MainWindow.OrderXmlFileName, obj);
                        }
                    }


                    if (dicTradeConfirm.ContainsKey(Tradeconfirm))
                    {
                        lock (dicTradeConfirm)
                        {
                            CStructures.ExchangeTradeConfirmation tradeConfirmation = dicTradeConfirm[Tradeconfirm];
                            lock (tradeConfirmation.Prop05SharekhanOrderID)
                            {
                                if (MainWindow.dicSharekhanIDvsAPIReqID.ContainsKey(tradeConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim()))
                                {
                                    string APIReqID = MainWindow.dicSharekhanIDvsAPIReqID[tradeConfirmation.Prop05SharekhanOrderID.Replace("\0", "").Trim()];

                                    MainWindow.orders ord = MainWindow.dicReqIDTrack[APIReqID];

                                    ord.ExchangeOrdID = tradeConfirmation.Prop06ExchangeOrderId;
                                    ord.ExcPrice = tradeConfirmation.Prop19TradePrice;
                                    ord.ExcQty = tradeConfirmation.Prop14TradeQty;
                                    ord.ExcDateTime = tradeConfirmation.Prop07ExchangeDateTime.Replace("\0", "").Trim();
                                    ord.AvgPrice = Convert.ToDouble(tradeConfirmation.Prop19TradePrice) / 100;
                                    ord.ConfrmType = "TradeConfirmation";


                                    MainWindow.MainOrderPosition obj = new MainWindow.MainOrderPosition();
                                    obj.SCRIPTINFO = ord.ScriptName + ord.SettingType;
                                    obj.XMLFILENAME = MainWindow.OrderXmlFileName;
                                    obj.API_ID = Convert.ToInt16(ord.APIReqID);
                                    obj.SCRIPT = ord.ScriptName;
                                    obj.OPEN = ord.open;
                                    obj.DATETIME = ord.datetime;
                                    obj.FORMULA = ord.FormulaName;
                                    obj.SETTING = ord.SettingType;

                                    if (!MainWindow.dicMainPosition.ContainsKey(obj.SCRIPTINFO))
                                    {
                                        #region Insert New
                                        if (MainWindow.dicMainPosition.Count == 0)
                                        {
                                            obj.XmlType = 3;
                                        }
                                        else
                                        {
                                            obj.XmlType = 0;
                                        }


                                        obj.LastNETPOSITION = 0;
                                        obj.CurrentNETPOSITION = ord.NetPosition;
                                        obj.LastSIGNAL = "START";
                                        obj.CurrentSIGNAL = MainWindow.dicReqIDTrack[APIReqID].Signal;// tradeConfirmation.Prop11BuySell.Replace("\0", "").Trim();

                                        if (obj.CurrentSIGNAL == "BUY" || obj.CurrentSIGNAL == "COVER")
                                        {
                                            obj.BuyAvgPrice = Convert.ToDouble(ord.ExcPrice) / 100;
                                            obj.TotalBuyQTY = ord.ExcQty;
                                            obj.SellAvgPrice = 0;
                                            obj.TotalSellQTY = 0;
                                            obj.BuyTotalPrice = obj.BuyTotalPrice + (obj.BuyAvgPrice * obj.TotalBuyQTY);
                                            obj.SellTotalPrice = 0;
                                        }
                                        else
                                        {

                                            obj.SellAvgPrice = Convert.ToDouble(ord.ExcPrice) / 100;
                                            obj.TotalSellQTY = ord.ExcQty;
                                            obj.BuyAvgPrice = 0;
                                            obj.TotalBuyQTY = 0;
                                            obj.SellTotalPrice = obj.SellTotalPrice + (obj.SellAvgPrice * obj.TotalSellQTY);
                                            obj.BuyTotalPrice = 0;
                                        }

                                        #endregion

                                        MainWindow.InsertupdateMaintable(MainWindow.MainXMlFileName, obj);
                                        MainWindow.dicMainPosition.Add(obj.SCRIPTINFO, obj);
                                    }
                                    else
                                    {
                                        lock (MainWindow.dicMainPosition[obj.SCRIPTINFO])
                                        {

                                            obj.XmlType = 1;
                                            obj.LastSIGNAL = MainWindow.dicMainPosition[obj.SCRIPTINFO].CurrentSIGNAL;
                                            obj.CurrentSIGNAL = MainWindow.dicReqIDTrack[APIReqID].Signal; ;//tradeConfirmation.Prop11BuySell.Replace("\0", "").Trim();

                                            obj.LastNETPOSITION = MainWindow.dicMainPosition[obj.SCRIPTINFO].CurrentNETPOSITION;
                                            obj.CurrentNETPOSITION = ord.NetPosition;

                                            string LastSIGNAL = "";
                                            string CurrentSIGNAL = "";

                                            if (obj.LastSIGNAL == "BUY" || obj.LastSIGNAL == "COVER")
                                                LastSIGNAL = "B";
                                            else
                                                LastSIGNAL = "S";

                                            if (obj.CurrentSIGNAL == "BUY" || obj.CurrentSIGNAL == "COVER")
                                                CurrentSIGNAL = "B";
                                            else
                                                CurrentSIGNAL = "S";


                                            if (LastSIGNAL == CurrentSIGNAL)
                                            {
                                                #region Same Signal Last and Current
                                                if (CurrentSIGNAL == "B")
                                                {
                                                    if (obj.CurrentNETPOSITION == 0)
                                                    {
                                                        obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + (MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice - MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice);
                                                        // obj.BuyAvgPrice = Convert.ToDouble(ord.ExcPrice) / 100;
                                                        obj.TotalBuyQTY = 0;
                                                        obj.SellAvgPrice = 0;
                                                        obj.TotalSellQTY = 0;
                                                        obj.BuyTotalPrice = 0;
                                                        obj.SellTotalPrice = 0;

                                                    }
                                                    else
                                                    {

                                                        obj.BuyAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY) + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty)) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + ord.ExcQty);
                                                        obj.TotalBuyQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + ord.ExcQty;
                                                        if (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY == 0)
                                                        {
                                                            obj.SellAvgPrice = 0;
                                                            obj.TotalSellQTY = 0;

                                                            obj.SellTotalPrice = 0;

                                                        }
                                                        else
                                                        {
                                                           obj.SellAvgPrice = (MainWindow.dicMainPosition[obj.SCRIPTINFO].SellAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + 0) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + 0);
                                                           
                                                            obj.TotalSellQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY -  obj.TotalBuyQTY;               
                                                            
                                                            obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice;
                                                            obj.SellTotalPrice = (MainWindow.dicMainPosition[obj.SCRIPTINFO]).SellTotalPrice;

                                                            obj.BuyTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);
                                                        }
                                                       
                                                        
                                                    }

                                                }
                                                else
                                                {

                                                    if (obj.CurrentNETPOSITION == 0)
                                                    {

                                                        obj.SellAvgPrice = 0;
                                                        obj.TotalSellQTY = 0;
                                                        obj.BuyAvgPrice = 0;
                                                        obj.TotalBuyQTY = 0;

                                                        obj.BuyTotalPrice = 0;
                                                        obj.SellTotalPrice = 0;
                                                        obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + (MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice - MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice);
                                                    }
                                                    else
                                                    {
                                                        obj.SellAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].SellAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY) + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty)) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + ord.ExcQty);
                                                        obj.TotalSellQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + ord.ExcQty;
                                                        if (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY == 0)
                                                        {
                                                            obj.BuyAvgPrice = 0;
                                                            obj.TotalBuyQTY = 0;

                                                            obj.BuyTotalPrice = 0;

                                                        }
                                                        else
                                                        {
                                                            obj.BuyTotalPrice = (MainWindow.dicMainPosition[obj.SCRIPTINFO]).BuyTotalPrice;

                                                            obj.BuyAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY) + 0) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + 0);
                                                            obj.TotalBuyQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + 0;
                                                        }
                                                        obj.SellTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);
                                                        obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + 0;
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Sqoff and Calculate PNL

                                                if (CurrentSIGNAL == "B")
                                                {
                                                    if (obj.CurrentNETPOSITION == 0)
                                                    {
                                                        obj.BuyTotalPrice = (ord.ExcQty * (Convert.ToDouble(ord.ExcPrice) / 100));
                                                        obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + (MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice - obj.BuyTotalPrice);
                                                        // obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + (MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice - MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice) + (ord.ExcQty * (Convert.ToDouble(ord.ExcPrice) / 100));
                                                        obj.BuyTotalPrice = 0;
                                                        obj.SellTotalPrice = 0;
                                                        obj.BuyAvgPrice = 0;
                                                        obj.TotalBuyQTY = 0;
                                                        obj.SellAvgPrice = 0;
                                                        obj.TotalSellQTY = 0;

                                                    }
                                                    else
                                                    {
                                                        if (obj.LastNETPOSITION == 0)
                                                        {
                                                            obj.BuyAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY) + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty)) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + ord.ExcQty);
                                                            obj.TotalBuyQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + ord.ExcQty;
                                                            obj.SellAvgPrice = 0;
                                                            obj.TotalSellQTY = 0;
                                                            obj.BuyTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);
                                                            obj.SellTotalPrice = 0;
                                                            obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + 0;
                                                        }
                                                        else
                                                        {
                                                            obj.BuyAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY) + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty)) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + ord.ExcQty);
                                                            obj.TotalBuyQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + ord.ExcQty;
                                                            obj.SellAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].SellAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY) + 0) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + 0);
                                                            obj.TotalSellQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY - ord.ExcQty;

                                                            obj.BuyTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);
                                                            obj.SellTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice - ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);

                                                            obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (obj.CurrentNETPOSITION == 0)
                                                    {
                                                        obj.SellAvgPrice = (ord.ExcQty * (Convert.ToDouble(ord.ExcPrice) / 100));
                                                        obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + (MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice - obj.SellTotalPrice);
                                                        // obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + (MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice - MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice) - (ord.ExcQty * (Convert.ToDouble(ord.ExcPrice) / 100));
                                                        obj.BuyAvgPrice = 0;
                                                        obj.TotalBuyQTY = 0;
                                                        obj.SellAvgPrice = 0;
                                                        obj.TotalSellQTY = 0;

                                                        obj.BuyTotalPrice = 0;
                                                        obj.SellTotalPrice = 0;
                                                    }
                                                    else
                                                    {
                                                        if (obj.LastNETPOSITION == 0)
                                                        {
                                                            obj.BuyAvgPrice = 0;
                                                            obj.TotalBuyQTY = 0;
                                                            obj.SellAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].SellAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY) + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty)) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + ord.ExcQty);
                                                            obj.TotalSellQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + ord.ExcQty; ;

                                                            obj.BuyTotalPrice = 0;
                                                            obj.SellTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);

                                                            obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + 0;
                                                        }
                                                        else
                                                        {
                                                            obj.SellAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].SellAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY) + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty)) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + ord.ExcQty);
                                                            obj.TotalSellQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalSellQTY + ord.ExcQty;
                                                            obj.BuyAvgPrice = ((MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyAvgPrice * MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY) + 0) / (MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY + 0);
                                                            obj.TotalBuyQTY = MainWindow.dicMainPosition[obj.SCRIPTINFO].TotalBuyQTY - ord.ExcQty;

                                                            obj.BuyTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].BuyTotalPrice - ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);
                                                            obj.SellTotalPrice = MainWindow.dicMainPosition[obj.SCRIPTINFO].SellTotalPrice + ((Convert.ToDouble(ord.ExcPrice) / 100) * ord.ExcQty);

                                                            obj.MainPNL = MainWindow.dicMainPosition[obj.SCRIPTINFO].MainPNL + 0;
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }

                                        }
                                        MainWindow.InsertupdateMaintable(MainWindow.MainXMlFileName, obj);
                                        MainWindow.dicMainPosition[obj.SCRIPTINFO] = obj;
                                    }

                                    MainWindow.InsertUpdateOrdertable(MainWindow.OrderXmlFileName, ord);
                                    MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "ExchangeTradeConfirmation : " + tradeConfirmation.ToString()));
                                    LogFile.Reference.WriteLogFile("Trade : ", ord.ToString());

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.Reference.WriteLogFile("Xml Updation Exception : " + ex.ToString(), ex.ToString());
                    MainWindow.logOutputQueue.EnQueue(string.Format("\n" + "Exception in ProcessXML()  : " + ex.ToString()));
                }

            }

        }
        /// <summary>
        /// Checks whether data recieved completely or not
        /// </summary>
        /// <param name="TotalBuffer"></param>
        /// <returns></returns>
        private bool CheckCompletePacket(byte[] TotalBuffer)
        {
            try
            {
                if (TotalBuffer.Length > 0)
                {
                    byte[] MessageHeader = new byte[CConstants.MsgHeaderSize];
                    Array.ConstrainedCopy(TotalBuffer, 0, MessageHeader, 0, CConstants.MsgHeaderSize);

                    CStructures.MessageHeader header = new CStructures.MessageHeader();
                    header.ByteToStruct(MessageHeader);
                    if (header.cTransactionCode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        if (header.Prop01MessageLength <= TotalBuffer.Length)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                return false;
            }
        }
        #endregion

        internal bool SendLoginRequest()
        {
            try
            {
                CStructures.MessageHeader Header = new CStructures.MessageHeader((Int16)CConstants.TranCode.LoginRequest)
                {
                    Prop01MessageLength = CConstants.LoginRequestSize,
                    Prop02TransactionCode = (Int16)CConstants.TranCode.LoginRequest
                };

                CStructures.SignOn cLoginRequest = new CStructures.SignOn((Int16)CConstants.TranCode.LoginRequest)
                {
                    Prop01Header = Header,
                    Prop02LoginId = CConstants.LoginId,
                    Prop03MemberPassword = CConstants.MemberPassword,
                    Prop04TradingPassword = CConstants.TradingPassword,
                    Prop05IP = CConstants.TapIp,
                    Prop06Reserved = ""
                };
                SendRequest(cLoginRequest.StructToByte());
                MainWindow.requestSentQueue.EnQueue(cLoginRequest.ToString());

                return (true);
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                return (false);
            }
        }
        /// <summary>
        /// LogOff Request
        /// </summary>
        internal bool SendLogOffRequest()
        {
            try
            {
                CStructures.MessageHeader LogOffReqHeader = new CStructures.MessageHeader((Int16)CConstants.TranCode.LogOffRequest)
                {
                    Prop01MessageLength = CConstants.LogOffRequestSize,
                    Prop02TransactionCode = (Int16)CConstants.TranCode.LogOffRequest
                };

                CStructures.LogOffRequest LogOffReq = new CStructures.LogOffRequest((Int16)CConstants.TranCode.LogOffRequest)
                {
                    Prop01Header = LogOffReqHeader,
                    Prop02LoginId = CConstants.LoginId,
                    Prop03Reserved = ""
                };
                MainWindow.requestSentQueue.EnQueue(LogOffReq.ToString());
                SendRequest(LogOffReq.StructToByte());
                Disconnect();
                return (true);
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                return (false);
            }
        }

        internal bool SendScripMasterDownload()
        {
            try
            {
                CStructures.MessageHeader Ncheader = new CStructures.MessageHeader((Int16)CConstants.TranCode.ScripMasterRequest)
                       {
                           Prop01MessageLength = CConstants.ScripMasterRequest,
                           Prop02TransactionCode = (Int16)CConstants.TranCode.ScripMasterRequest
                       };

                #region NSE FO ScripMaster Request

                CStructures.ScripMasterRequest NFScripMaster = new CStructures.ScripMasterRequest(true)
                {
                    Prop01Header = Ncheader,
                    Prop02ExchangeCode = CConstants.NFExCode
                };
                MainWindow.requestSentQueue.EnQueue(NFScripMaster.ToString());
                SendRequest(NFScripMaster.StructToByte());

                #endregion

                #region NSE Cash ScripMaster Request

                CStructures.ScripMasterRequest NCScripMaster = new CStructures.ScripMasterRequest(true)
                {
                    Prop01Header = Ncheader,
                    Prop02ExchangeCode = CConstants.NCExcode
                };
                MainWindow.requestSentQueue.EnQueue(NCScripMaster.ToString());
                SendRequest(NCScripMaster.StructToByte());

                #endregion

                #region BSE Cash ScripMaster Request

                CStructures.ScripMasterRequest BCScripMaster = new CStructures.ScripMasterRequest(true)
                {
                    Prop01Header = Ncheader,
                    Prop02ExchangeCode = CConstants.BCExcode
                };
                MainWindow.requestSentQueue.EnQueue(BCScripMaster.ToString());
                SendRequest(BCScripMaster.StructToByte());

                #endregion

                #region NSE Currency ScripMaster Request

                CStructures.ScripMasterRequest RNScripMaster = new CStructures.ScripMasterRequest(true)
                {
                    Prop01Header = Ncheader,
                    Prop02ExchangeCode = CConstants.RNExCode
                };
                MainWindow.requestSentQueue.EnQueue(RNScripMaster.ToString());
                SendRequest(RNScripMaster.StructToByte());

                #endregion

                #region MCX Currency ScripMaster Request

                CStructures.ScripMasterRequest RMScripMaster = new CStructures.ScripMasterRequest(true)
                {
                    Prop01Header = Ncheader,
                    Prop02ExchangeCode = CConstants.RMExcode
                };
                MainWindow.requestSentQueue.EnQueue(RMScripMaster.ToString());
                SendRequest(RMScripMaster.StructToByte());

                #endregion

                #region MCX Commodity ScripMaster Request

                CStructures.ScripMasterRequest MXScripMaster = new CStructures.ScripMasterRequest(true)
                {
                    Prop01Header = Ncheader,
                    Prop02ExchangeCode = CConstants.MXExcode
                };
                SendRequest(MXScripMaster.StructToByte());
                MainWindow.requestSentQueue.EnQueue(MXScripMaster.ToString());
                #endregion

                #region NCDEX Commodity ScripMaster Request

                CStructures.ScripMasterRequest NXScripMaster = new CStructures.ScripMasterRequest(true)
                {
                    Prop01Header = Ncheader,
                    Prop02ExchangeCode = CConstants.NXExcode
                };
                SendRequest(NXScripMaster.StructToByte());
                MainWindow.requestSentQueue.EnQueue(NXScripMaster.ToString());
                #endregion

                return (true);
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                return (false);
            }
        }

        /// <summary>
        /// SendRequest
        /// </summary>
        /// <param name="sendData">pass Byte[]</param>
        public void SendRequest(byte[] sendData)
        {
            try
            {
                SendData.EnQueue(sendData);
            }
            catch (Exception ex)
            {
                LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
            }
        }

        #region StateClass

        /// <summary>
        /// State Object class is created to preserve values during Asynochronous Receive From Server.
        /// </summary>
        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024 * 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder data = new StringBuilder();
        }

        #endregion

        internal void SubscribeforFeeds(byte[] request)
        {
            SendRequest(request);
        }

        internal void SendOrderReportRequest(byte[] p)
        {
            SendRequest(p);
        }
    }
}
#region Commented
#region RawSocket Event
//void rawSocketClient_OnLog(object sender, string s)
//{

//}

//void rawSocketClient_OnError(object sender, SocketErrorEventArgs e, string s)
//{

//}

//void rawSocketClient_OnDisconnected(object sender, DisconnectedEventArgs e)
//{
//    FnoCtclLib.Reference.LogInfo("Disconnected.");
//    ProcessData = false;

//    FnoCtclLib.Reference.SetConrolText(FnoCtclLib.Reference.btntapconnect, "&Connect");
//    // FnoCtclLib.Reference.SetColor(FnoCtclLib.Reference.btnConnect, Color.Green);

//    FnoCtclLib.Reference.SetConrolText(FnoCtclLib.Reference.btnlogin, "&Login");
//    //FnoCtclLib.Reference.SetColor(FnoCtclLib.Reference.btnLogin, Color.Green);
//}

//void rawSocketClient_OnDataIn(object sender, DataInEventArgs e)
//{
//    byte[] bInData = e.TextA;
//    InData.EnQueue(bInData); 
//}

//void rawSocketClient_OnConnected(object sender, ConnectedEventArgs e)
//{
//    FnoCtclLib.Reference.LogInfo("Connected.");
//    ProcessData = true;
//    TAPSequenceNumber = 1;// Sequence Number has to be Generated
//    InData.PacketsCanSend = 1;
//    SendData.PacketsCanSend = 0;
//    StartThreads();
//    FnoCtclLib.Reference.SetConrolText(FnoCtclLib.Reference.btntapconnect, "&Disconnect");
//    FnoCtclLib.Reference.SetColor(FnoCtclLib.Reference.btntapconnect, Color.Red);
//}

#endregion

#region Events of IPPort
// On DataIn push to Queue
//void IPPort_OnDataIn(object sender, IpportDataInEventArgs e)
//{
//    byte[] bInData = e.TextB;
//    InData.EnQueue(bInData);
//}

//void IPPort_OnDisconnected(object sender, IpportDisconnectedEventArgs e)
//{
//    FnoCtclLib.Reference.LogInfo("Disconnected.");
//    ProcessData = false;

//    FnoCtclLib.Reference.SetConrolText(FnoCtclLib.Reference.btntapconnect, "&Connect");
//    // FnoCtclLib.Reference.SetColor(FnoCtclLib.Reference.btnConnect, Color.Green);

//    FnoCtclLib.Reference.SetConrolText(FnoCtclLib.Reference.btnlogin, "&Login");
//    //FnoCtclLib.Reference.SetColor(FnoCtclLib.Reference.btnLogin, Color.Green);
//}

//void IPPort_OnConnected(object sender, IpportConnectedEventArgs e)
//{
//    FnoCtclLib.Reference.LogInfo("Connected.");
//    ProcessData = true;
//    TAPSequenceNumber = 1;// Sequence Number has to be Generated
//    InData.PacketsCanSend = 1;
//    SendData.PacketsCanSend = 0;
//    StartThreads();
//    FnoCtclLib.Reference.SetConrolText(FnoCtclLib.Reference.btntapconnect, "&Disconnect");
//    FnoCtclLib.Reference.SetColor(FnoCtclLib.Reference.btntapconnect, Color.Red);

//}

#endregion
#endregion