using System;
using System.Collections.Generic;
using System.Text;
using nsoftware.IPWorks;
using System.Threading;
using System.Security.Cryptography;
using Sharekhan.RawSocket;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace FNOCTCL
{
    public class CTAPClient
    {
        #region Initial Declaration
        Socket socket = null;

        private string Exchange;
        public string RemoteIP;
        public int RemotePort;

        public double CTCLSequenceNumber;
        public DateTime LoginDateTime;
        public Int32 LogTime;
        public Int32 LatestTime;
        public Int32 LastAckRecTime;
        public string LastDataRecTime;

        //InitialFlagVariable
        bool receiveData = false;//set true when Connected to Tap
        public int TAPSequenceNumber = 0;// Onconnected to tap Set to 1
        public bool ProcessData = false;//Flag For processing
        //int TAPSequenceNumber;
        public int DataReceived;
        // to track No of Order Rejected 
        public int NewOrderRejected;
        public int ModifyOrderRejected;
        public int CancelOrderRejected;
        public int OrderAckReceived;
        public int SpreadAckReceived;
        public int TradeReceived;


        public FileStream FSData;
        public StreamWriter stwData;

        //Dictionary Used in :
        Dictionary<int, CStructures.TokenAndEligibility> SecurityDetail;
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
        public CStructures.systemInfoResponse SystemInfo = new CStructures.systemInfoResponse(0);

        // Queue declaration
        public CQueue<byte[]> InData = new CQueue<byte[]>();//byte Queue
        public CQueue<byte[]> SendData = new CQueue<byte[]>();
        public CQueue<byte[]> LoginSendData = new CQueue<byte[]>();
        CQueue<int> lastAckTimeData = new CQueue<int>();
        CQueue<byte[]> tempSendData = new CQueue<byte[]>(); 

        // Threads For Indata and senddata
        Thread ThreadProcessInData = null;// Receive data 
        Thread ThreadProcessSendData = null;//Send Data 
        Thread ThreadProcessSendLoginData = null;// Request
        Thread ThreadDisconnect = null; // Disconnect
       //Thread ThreadLastAckSerialiser = null;// Serialisaion of Delta Acknowledge
        Thread ThreadUpdateUI = null;// update User Interface
        Thread ThreadUpdAckTime = null;

        // Manual Reset For Asynchronous Socket.
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent disconnectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        // Stream no 
        public static string StreamNo = null;

        public CObject ConfObj;

        #endregion

        //Constructor
        public CTAPClient(string sExchange)
        {
            Exchange = sExchange;
        }

        #region Asynchronous Socket Eveents -Connect,receive,Send,Disconnect

        public void Connect()
        {
            try
            {
                IPAddress ipAddress = null;
                IPEndPoint remoteEP = null;
                ipAddress = IPAddress.Parse(RemoteIP);
                remoteEP = new IPEndPoint(ipAddress, RemotePort);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveBufferSize = 2048;
                socket.SendBufferSize = 2048;
                socket.Ttl = 128;
                socket.ReceiveTimeout = 0;
                socket.SendTimeout = 0;
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1);
                try
                {
                    Connect(remoteEP, socket);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Error Connecting Tap.");
                }
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Error Connecting Tap.Msg:- ");
            }

        }

        public void Connect(EndPoint remoteEP, Socket client)
        {
            try
            {
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
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

                FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "  " + "Connected To TAP ");//+ client.RemoteEndPoint.ToString());
                connectDone.Set();

                receiveData = true;
                OnConnected("", "");

                Receive(socket);
            }
            catch (SocketException ex)
            {
                OnConnected(ex.SocketErrorCode.ToString(), "SocketException " + ex.NativeErrorCode.ToString() + ex.Message);
                connectDone.Set();
            }
            catch (Exception ex)
            {
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
                bolLogin = false;
                bolConnected = false;

                StartThreads();
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception in tap's OnConnected()");
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                receiveDone.WaitOne();
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Method Receive(Socket client)");
            }

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                int bytesRead = socket.EndReceive(ar);
                // check if more data is there 
                if (bytesRead > 0)
                {
                    byte[] bdata = new byte[bytesRead];
                    bdata = CUtility.GetByteArrayFromStructure(state.buffer, 0, bytesRead);
                    InData.EnQueue(bdata);
                    InData.AddCounter(1);
                    Interlocked.Increment(ref DataReceived);
                    socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    FnoCtclLib.Reference.LogInfo("Abrupt Disconnection");
                    receiveData = false;
                    receiveDone.Set();
                    Disconnect();
                }
            }
            catch (SocketException ex)
            {

                FnoCtclLib.Reference.LogInfo("Abrupt Disconnection.");
                FnoCtclLib.Reference.LogException(ex, "Abrupt Disconnection. SocketException in ReceiveCallback() :- " + ex.SocketErrorCode + " Message :- " + ex.Message);
                receiveData = false;
                receiveDone.Set();
                Disconnect();
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogInfo("Abrupt Disconnection.");
                FnoCtclLib.Reference.LogException(ex, "Abrupt Disconnection. Exception in ReceiveCallback() Message :- " + ex.Message);
                receiveData = false;
                receiveDone.Set();
                Disconnect();
            }
        }

        private void Send(Socket client, byte[] byteData)
        {
            try
            {
                client.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), client);
            }
            catch (ArgumentNullException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ArgumentNullException in " + Exchange + " Send() " + ex.StackTrace);
            }
            catch (ArgumentException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ArgumentException in " + Exchange + " Send() " + ex.StackTrace);
            }
            catch (SocketException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "SocketException in " + Exchange + " Send() " + ex.StackTrace);
            }
            catch (ObjectDisposedException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ObjectDisposedException in " + Exchange + " Send() " + ex.StackTrace);
            }
            catch (InvalidOperationException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "InvalidOperationException in " + Exchange + " Send() " + ex.StackTrace);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ArgumentNullException in " + Exchange + " Send() " + ex.StackTrace);
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
                FnoCtclLib.Reference.LogException(ex, "ArgumentNullException in " + Exchange + " SendCallback() " + ex.StackTrace);
            }
            catch (ArgumentException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ArgumentException in " + Exchange + " SendCallback() " + ex.StackTrace);
            }
            catch (SocketException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "SocketException in " + Exchange + " SendCallback() " + ex.StackTrace);
            }
            catch (ObjectDisposedException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ObjectDisposedException in " + Exchange + " SendCallback() " + ex.StackTrace);
            }
            catch (InvalidOperationException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "InvalidOperationException in " + Exchange + " SendCallback() " + ex.StackTrace);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ArgumentNullException in " + Exchange + " SendCallback() " + ex.StackTrace);
            }
        }

        public void Disconnect()
        {
            try
            {
                receiveData = false;
                Disconnect(socket);
            }
            catch (NullReferenceException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "NullReferenceException in Disconnect()" + ex.Message);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception in Disconnect()" + ex.Message);
            }
            finally
            {
                // Start Disconnect Thread.
                if (ThreadDisconnect != null)
                    StopThread(ref ThreadDisconnect);
                StartThread(ref ThreadDisconnect, "Disconnect");
                socket = null;
            }
        }

        private void Disconnect(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;
                client.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), state);
                disconnectDone.WaitOne();
            }
            catch (SocketException ex)
            {
                OnDisconnected(ex.SocketErrorCode.ToString(), "SocketException " + ex.NativeErrorCode.ToString() + ex.Message);
                disconnectDone.Set();
                FnoCtclLib.Reference.Reconnect = true;
            }
            catch (Exception ex)
            {
                OnDisconnected(ex.Data.GetHashCode().ToString(), "Exception " + ex.Data.GetHashCode().ToString() + ex.Message);
                disconnectDone.Set();
                FnoCtclLib.Reference.Reconnect = true;
            }
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                socket.EndDisconnect(ar);

                FnoCtclLib.Reference.LogInfo("Socket disconnected.");
                disconnectDone.Set();
                socket.Close(1);// Timeout 1 second
                OnDisconnected("", "");
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
            finally
            {
                FnoCtclLib.Reference.Reconnect = true;
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
                    InData.PacketsCanSend = 1;
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

                    if (SendData.GetSize() > 0)
                    {
                        try
                        {
                            lock (SendData)
                            {
                                tempSendData.PacketsCanSend = 0;

                                byte[] bdata = null;
                                bdata = SendData.DeQueue(true);
                                while (bdata != null && bdata.Length > 0)
                                {
                                    tempSendData.EnQueue(bdata);
                                    if (SendData.GetSize() > 0)
                                        bdata = SendData.DeQueue(true);
                                    else
                                        bdata = null;
                                }
                                SendData.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            FnoCtclLib.Reference.ConnectedFlag = -1;
                            FnoCtclLib.Reference.LogException(ex, "Exception in OnDisconnected(Copy the Q Data to TempQ).");
                        }
                    }
                    StopThreads();
                    //SerializeLastAckReceived();
                    FnoCtclLib.Reference.LogInfo("Disconnected Successfull");
                }
                else
                {
                    FnoCtclLib.Reference.ConnectedFlag = -1;
                    FnoCtclLib.Reference.LogInfo("Disconnected. Error :- " + status + ". Discription :- " + discription);
                }
                FnoCtclLib.Reference.Reconnect = true;
            }
            catch (ThreadAbortException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Thread :- " + Thread.CurrentThread.Name + " is Stopped. Msg. :- " + ex.Message);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Error Connecting Tap");
            }
        }

        #endregion

        #region Threads Methods

        void StartThreads()
        {
            try
            {
                if (ThreadProcessInData != null)
                    StopThread(ref ThreadProcessInData);
                if (ThreadProcessSendData != null)
                    StopThread(ref ThreadProcessSendData);
                if (ThreadProcessSendLoginData != null)
                    StopThread(ref ThreadProcessSendLoginData);
                //if (ThreadLastAckSerialiser != null)
                //    StopThread(ref ThreadLastAckSerialiser);
                if (ThreadUpdateUI != null)
                    StopThread(ref ThreadUpdateUI);
                if (ThreadUpdAckTime != null)
                    StopThread(ref ThreadUpdAckTime);


                StartThread(ref ThreadProcessInData, "InData");
                StartThread(ref ThreadProcessSendData, "SendData");
                StartThread(ref ThreadProcessSendLoginData, "SendLoginData");
                //StartThread(ref ThreadLastAckSerialiser, "AckTimeSerialise");
                StartThread(ref ThreadUpdateUI, "UpdateUI");
                StartThread(ref ThreadUpdAckTime, "UpdAck");

            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Method StartThreads()");
            }
        }

        void StopThreads()
        {
            try
            {
                StopThread(ref ThreadProcessInData);
                StopThread(ref ThreadProcessSendData);
                StopThread(ref ThreadProcessSendLoginData);
                //StopThread(ref ThreadLastAckSerialiser);
                StopThread(ref ThreadUpdateUI);
                StopThread(ref ThreadUpdAckTime);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Method StopThreads()");
            }
        }

        void StartThread(ref Thread tProcess, string threadName)
        {
            try
            {
                switch (threadName)
                {
                    case "InData":
                        tProcess = new Thread(new ThreadStart(ThreadParseData));
                        break;
                    case "SendData":
                        tProcess = new Thread(new ThreadStart(ThreadSendData));
                        break;
                    case "SendLoginData":
                        tProcess = new Thread(new ThreadStart(ThreadSendLoginData));
                        break;
                    //case "AckTimeSerialise":
                    //    tProcess = new Thread(new ThreadStart(ThreadLastAckSerialise));
                    //    break;
                    case "Disconnect":
                        tProcess = new Thread(new ThreadStart(ThreadCallDisconnect));
                        break;
                    case "UpdateUI":
                        tProcess = new Thread(new ThreadStart(FnoCtclLib.Reference.UpdateUI));
                        break;
                    case "UpdAck":
                        tProcess = new Thread(new ThreadStart(UpdAckTextfile));
                        break;
                }

                tProcess.Name = threadName;
                tProcess.IsBackground = true;
                tProcess.Start();
            }
            catch (ThreadStateException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ThreadStateException in StartThread. ThreadName :- " + threadName);
            }
            catch (InvalidOperationException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "InvalidOperationException in StartThread. ThreadName :- " + threadName);
            }
            catch (System.Security.SecurityException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "SecurityException in StartThread. ThreadName :- " + threadName);
            }
            catch (OutOfMemoryException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "OutOfMemoryException in StartThread. ThreadName :- " + threadName);
            }
            catch (ThreadAbortException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ThreadAbortException in StartThread. ThreadName :- " + threadName);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception in StartThread. ThreadName :- " + threadName);
            }
        }

        void StopThread(ref Thread tProcess)
        {
            if (tProcess == null)
                return;

            try
            {
                tProcess.Abort();
            }
            catch (System.Security.SecurityException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "SecurityException in ThreadsStop");
            }
            catch (ThreadStateException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ThreadStateException in ThreadsStop");
            }
            catch (ThreadAbortException ex)
            {
                FnoCtclLib.Reference.LogException(ex, "ThreadAbortException in ThreadsStop");
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception in ThreadsStop");
            }
            finally
            {
                tProcess = null;
            }
        }

        /// <summary>
        /// get Error Response and other 
        /// </summary>
        private void ThreadParseData()
        {
            byte[] TotalBuffer = new byte[0];
            byte[] PartialBuffer = new byte[0];
            int BytesToRemove = 0;
            byte[] data;

            while (ProcessData)
            {
                try
                {

                    // ParseData(data);
                    if (BytesToRemove > 0 || PartialBuffer.Length > 0)
                    {
                        Array.Resize(ref TotalBuffer, PartialBuffer.Length);
                        Array.Copy(PartialBuffer, 0, TotalBuffer, 0, PartialBuffer.Length);
                    }

                    // Check For Complete Packet
                    if (CheckCompletePacket(TotalBuffer) == false)
                    {
                        data = InData.DeQueue(true);// Overloaded Method which check ipacketCount.
                        Array.Resize(ref TotalBuffer, data.Length + PartialBuffer.Length);
                        Array.Copy(data, 0, TotalBuffer, PartialBuffer.Length, data.Length);
                    }

                    BytesToRemove = 0;
                    Array.Resize(ref PartialBuffer, 0);

                    if (TotalBuffer.Length >= CConstants.MinPacketSize)
                    {
                        byte[] bTapHeader = new byte[CConstants.TAPHeaderSize];
                        Array.ConstrainedCopy(TotalBuffer, 0, bTapHeader, 0, CConstants.TAPHeaderSize);

                        CStructures.TapHeader TapHeader = new CStructures.TapHeader(0);
                        TapHeader.ByteToStruct(bTapHeader);

                        byte[] ActualData = new byte[TapHeader.Prop01PacketLength - CConstants.TAPHeaderSize];

                        if (TapHeader.Prop01PacketLength <= TotalBuffer.Length)
                        {
                            //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "TapHeader Sequence Number :- " + TapHeader.Prop02SequenceNumber.ToString() + " TapHeader Packet Length :- " + TapHeader.Prop01PacketLength.ToString());
                            // FnoCtclLib.Reference.LogInfo("TapHeader Packet Length :- " + TapHeader.Prop01PacketLength.ToString());
                            Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, ActualData, 0, TapHeader.Prop01PacketLength - CConstants.TAPHeaderSize);

                            MD5 md5 = new MD5CryptoServiceProvider();
                            byte[] ActualCheckSum = md5.ComputeHash(ActualData);

                            byte[] MessageHeader = new byte[CConstants.MsgHeaderSize];
                            Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, MessageHeader, 0, CConstants.MsgHeaderSize);

                            if (CUtility.CheckPacket(TapHeader.Prop03MD5CheckSum, ActualCheckSum) == true)
                            {
                                CStructures.MessageHeader Header = new CStructures.MessageHeader(0);
                                Header.ByteToStruct(MessageHeader);
                                if (Header.Prop03LogTime != 0)
                                    LatestTime = Header.Prop03LogTime;

                                if (LastAckRecTime < Header.Prop03LogTime && bolLogin == true)
                                {
                                    LastAckRecTime = Header.Prop03LogTime;
                                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Last Ack. Rec. Time :- " + Header.Prop03LogTime.ToString());

                                    //string txt = FnoCtclLib.Reference.stwReader.ReadLine();
                                    //FnoCtclLib.Reference.stwData.Write(txt.Replace(txt, LastAckRecTime.ToString()));
                                    //FnoCtclLib.Reference.stwData.Write(LastAckRecTime);
                                    lastAckTimeData.EnQueue(LastAckRecTime);
                                    lastAckTimeData.AddCounter(1);
                                    // FnoCtclLib.Reference.stwData.Flush();
                                    //SerializeLastAckReceived();
                                }
                                // In Case Error Occurs
                                if (Header.Prop06ErrorCode > 0)
                                {
                                    int TransCode = Header.Prop05TransactionCode;
                                    if (TransCode == 8224) break;//FOR SIMULATOR
                                    switch (TransCode)
                                    {
                                        #region LogOffResponse          Transcode:2321
                                        case (int)CConstants.TranCode.LogOffResponse:
                                            bolLogin = false;
                                            byte[] bErrorResponse = new byte[CConstants.ErrorResponseSize];
                                            Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bErrorResponse, 0, CConstants.ErrorResponseSize);

                                            CStructures.ErrorResponse errorResponse = new CStructures.ErrorResponse(0);
                                            errorResponse.Prop01Header = Header;
                                            errorResponse.ByteToStruct(bErrorResponse);
                                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Log Time - " + Header.Prop03LogTime.ToString() + " Key :-" + errorResponse.Prop02Key.ToString() + ". Error Msg. - " + errorResponse.Prop03ErrorMessage);

                                            BytesToRemove = CConstants.TAPHeaderSize + CConstants.SignOff;//22+185=205
                                            break;
                                        #endregion

                                        #region Order Rejected             Transcode:2231
                                        case (int)CConstants.TranCode.NewOrderRejected:
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                //FnoCtclLib.Reference.LogInfo("Order Rejected");

                                                byte[] bOrderError = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bOrderError, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bOrderError);
                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());
                                                Interlocked.Increment(ref OrderAckReceived);

                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AllQueue.EnQueue(
                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());

                                                Interlocked.Increment(ref NewOrderRejected);
                                                FnoCtclLib.Reference.LogInfo("Reason Code: " + cOrderRequest.Prop08ReasonCode.ToString());

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;//22+236=258
                                            }
                                            break;
                                        #endregion

                                        #region Modify Order Rejected                Transcode:2042
                                        case (int)CConstants.TranCode.ModifyOrderRejected:
                                            //FnoCtclLib.Reference.LogInfo("Modify Order Rejected");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bModifyOrderRejected = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bModifyOrderRejected, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bModifyOrderRejected);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());

                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                                                Interlocked.Increment(ref OrderAckReceived);

                                                // To Do
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);
                                                Interlocked.Increment(ref ModifyOrderRejected);
                                                //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Reason Code: " + cOrderRequest.Prop08ReasonCode.ToString());
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;
                                            }
                                            break;
                                        #endregion

                                        #region Cancel Order Rejected                   Transcode:2072,2127
                                        case (int)CConstants.TranCode.CancelOrderRejected:
                                        //case (int)CConstants.TranCode.SpreadOrderCancelRejected:
                                            //FnoCtclLib.Reference.LogInfo("Cancel Order Rejected");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bCancelOrderRejected = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bCancelOrderRejected, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bCancelOrderRejected);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());

                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                                                Interlocked.Increment(ref OrderAckReceived);

                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);
                                                Interlocked.Increment(ref CancelOrderRejected);
                                                //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Reason Code: " + cOrderRequest.Prop08ReasonCode.ToString());
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;
                                            }
                                            break;
                                        #endregion

                                        #region Trade Modification Rejected         Transcode:2223
                                        case (int)CConstants.TranCode.TradeError:
                                            // FnoCtclLib.Reference.LogInfo("Cancel Order Rejected");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bTradeModificationRejected = new byte[CConstants.TradeModificationsize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bTradeModificationRejected, 0, CConstants.TradeModificationsize);

                                                CStructures.TradeModificationRequest cTradeModifyRequest = new CStructures.TradeModificationRequest(0);
                                                cTradeModifyRequest.Prop01Header = Header;
                                                cTradeModifyRequest.ByteToStruct(bTradeModificationRejected);
                                                //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Trade Modification Rejected" + cTradeModifyRequest.ToString());


                                                //CRMSStructure.TradeDetails TradeResponse = new CRMSStructure.TradeDetails(string.Empty);
                                                //TradeResponse.TradeConfirm = cTradeModifyRequest;
                                                //TradeResponse.ParseStruct();
                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(TradeResponse.Indata);
                                                //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Reason Code: " + cOrderRequest.Prop08ReasonCode.ToString());
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;//150+22=172
                                            }
                                            break;
                                        #endregion

                                        #region Spread Order Rejected             Transcode:2154,2155,2156,2127
                                        case (int)CConstants.TranCode.SpreadOrderErrorResponse:
                                        case (int)CConstants.TranCode.OrderError2L:
                                        case (int)CConstants.TranCode.OrderError3L:
                                        case(int)CConstants.TranCode.SpreadOrderCancelRejected:
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Spread Order Rejected");

                                                byte[] bSpreadOrderError = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bSpreadOrderError, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bSpreadOrderError);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cSpreadOrderRequest.ToString());
                                                //CClientOrder ClientOrder = new CClientOrder();
                                                //ClientOrder.OrderRequest = cOrderRequest;
                                                //ClientOrder.ParseStruct();
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                                                
                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref SpreadAckReceived);
                                                //FnoCtclLib.Order2L3L.SendQueue2L3L.EnQueue(cSpreadOrderRequest);
                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.InData);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest;//22+400=422
                                            }
                                            break;
                                        #endregion

                                        #region Spread Order Cancel Acknowledgement         Transcode:2031,2131,2132
                                        case (int)CConstants.TranCode.SpreadOrderConfirmCancellationResponse:
                                        case (int)CConstants.TranCode.OrderCancelConfirm2L:
                                        case (int)CConstants.TranCode.OrderCancelConfirm3L:
                                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Cancel Spread Order Acknowledgement");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bCancelSpreadOrderAck = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bCancelSpreadOrderAck, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bCancelSpreadOrderAck);
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cSpreadOrderRequest.ToString());
                                                Interlocked.Increment(ref SpreadAckReceived);
                                                //TODO: Fetch Important Info
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                                               
                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest;// 258
                                            }
                                            break;
                                        #endregion

                                        #region Cancel Order Confirmation            Transcode:2075,2132
                                        case (int)CConstants.TranCode.CancelOrderConfirmation:
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bCancelOrderConfirm = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bCancelOrderConfirm, 0, CConstants.OrderRequestSize);
                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bCancelOrderConfirm);
                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                                              
                                                Interlocked.Increment(ref OrderAckReceived);
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;//258
                                            }
                                            break;
                                        #endregion

                                        #region Exercise and Position Liquidation    Transcode:4002,4007,4010
                                        case (int)CConstants.TranCode.ExerciseCancelConfirm:
                                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Exercise Confirmation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bExercisePosition = new byte[CConstants.ExcercisePositionRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bExercisePosition, 0, CConstants.ExcercisePositionRequest);

                                                CStructures.LiquidationEntryReq cPositLiqRequest = new CStructures.LiquidationEntryReq(4000, "CA");
                                                cPositLiqRequest.Prop01Header = Header;
                                                cPositLiqRequest.ByteToStruct(bExercisePosition);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cPositLiqRequest.ToString());
                                                FnoCtclLib.Reference.ExcerciseQueue.EnQueue(cPositLiqRequest);
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.ExcercisePositionRequest;// 258
                                            }
                                            break;
                                        #endregion

                                        #region ErrorResponse
                                        default:
                                            byte[] ErrorResponse = new byte[CConstants.ErrorResponseSize];
                                            Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, ErrorResponse, 0, CConstants.ErrorResponseSize);

                                            CStructures.ErrorResponse Errorresponse = new CStructures.ErrorResponse(0);
                                            Errorresponse.Prop01Header = Header;
                                            Errorresponse.ByteToStruct(ErrorResponse);

                                            switch (Header.Prop06ErrorCode.ToString().Trim())
                                            {
                                                case "16053": // Password Expired. Change Password.
                                                    {
                                                        string pwd = CConstants.Password;
                                                        List<char> characters = new List<char>();
                                                        characters.AddRange(pwd.ToCharArray());
                                                        characters.Reverse();
                                                        string reversed = new string(characters.ToArray());
                                                        CConstants.NewPassword = reversed;
                                                        //FnoCtclLib.Reference.SetConrolText(FnoCtclLib.Reference.txtNewPassword, reversed);
                                                        FnoCtclLib.Reference.LogInfo("Message :- " + Errorresponse.Prop03ErrorMessage);
                                                        FnoCtclLib.Reference.LoggedInFlag = -1;
                                                        FnoCtclLib.Reference.PasswordChangeFlag = 1;
                                                    }
                                                    break;
                                                case "16004": // User Already Logged In.
                                                case "16006": // Invalid Signon Login Parameters May be Wrong.
                                                case "16134": // Dealer Disabled. Call Exchange.                                                    
                                                    {
                                                        FnoCtclLib.Reference.LogInfo("Error Code - " + Header.Prop06ErrorCode.ToString() + ". Error Msg. - " + Errorresponse.Prop03ErrorMessage);
                                                        if (CConstants.AutoRestart == true)
                                                        {
                                                            CConstants.AutoRestart = false;
                                                            FnoCtclLib.Reference.LoggedInFlag = -1;
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Transaction Code:" + Header.Prop05TransactionCode.ToString() + "Error Code - " + Header.Prop06ErrorCode.ToString() + ". Error Msg. - " + Errorresponse.Prop03ErrorMessage);
                                                    break;
                                            }
                                            BytesToRemove = CConstants.TAPHeaderSize + CConstants.ErrorResponseSize;
                                            break;
                                        #endregion ErrorResponse
                                    }
                                }
                                else
                                {
                                    #region Response
                                    int TransCode = Header.Prop05TransactionCode;

                                    switch (TransCode)
                                    {
                                        #region Invitation          Transcode:15000
                                        case (int)CConstants.TranCode.Invitation:
                                            FnoCtclLib.Reference.LogInfo("Incoming Invitation Count");
                                            //FnoCtclLib.Reference.LogInfo("Incoming Invitation Count");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] InvitationPacket = new byte[CConstants.TAPInvitationSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, InvitationPacket, 0, CConstants.TAPInvitationSize);

                                                CStructures.TapInvitation TapInvitation = new CStructures.TapInvitation(0);
                                                TapInvitation.ByteToStruct(InvitationPacket);
                                                Int16 InvitationPacketCount = TapInvitation.Prop02Count;
                                                if (bolLogin)
                                                    SendData.AddCounter(InvitationPacketCount);
                                                else
                                                    LoginSendData.AddCounter(InvitationPacketCount);
                                                // Connected to Tap 
                                                bolConnected = true;
                                                Interlocked.Increment(ref InvitationNo);

                                                // FnoCtclLib.Reference.LogInfo("Invitation Count:" + InvitationPacketCount.ToString());
                                                BytesToRemove = 64;// 22+38+2=62
                                            }
                                            break;
                                        #endregion

                                        #region Stop Loss Triggered         Transcode:2212
                                        case (int)CConstants.TranCode.StopLossTriggered:
                                            // FnoCtclLib.Reference.LogInfo("Stop Loss Triggered");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)//62
                                            {
                                                byte[] bTradeConfirmation = new byte[CConstants.TradeConfirmSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bTradeConfirmation, 0, CConstants.TradeConfirmSize);

                                                CStructures.TradeConfirm cTradeConfirm = new CStructures.TradeConfirm(0);
                                                cTradeConfirm.Prop01Header = Header;
                                                cTradeConfirm.ByteToStruct(bTradeConfirmation);

                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueTradeConfirmation.EnQueue(cTradeConfirm);
                                                //FnoCtclLib.Reference.TradeQueue.EnQueue(cTradeConfirm);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Trade;
                                                ConfObj.Trade = cTradeConfirm;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                                                Interlocked.Increment(ref TradeReceived);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cTradeConfirm.ToString());


                                                //CRMSStructure.TradeDetails TradeResponse = new CRMSStructure.TradeDetails(string.Empty);
                                                //TradeResponse.TradeConfirm = cTradeConfirm;
                                                //TradeResponse.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(TradeResponse.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.TradeConfirmSize;//205+22=227
                                            }
                                            break;
                                        #endregion Stop Loss Triggered

                                        #region Trade Confirmation          Transcode:2222
                                        case (int)CConstants.TranCode.TradeConfirmation:
                                            //FnoCtclLib.Reference.LogInfo("Trade Confirmation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bTradeConfirmation = new byte[CConstants.TradeConfirmSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bTradeConfirmation, 0, CConstants.TradeConfirmSize);

                                                CStructures.TradeConfirm cTradeConfirm = new CStructures.TradeConfirm(0);
                                                cTradeConfirm.Prop01Header = Header;
                                                cTradeConfirm.ByteToStruct(bTradeConfirmation);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cTradeConfirm.ToString());

                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Trade;
                                                ConfObj.Trade=cTradeConfirm;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueTradeConfirmation.EnQueue(cTradeConfirm);
                                                //FnoCtclLib.Reference.TradeQueue.EnQueue(cTradeConfirm);                                               

                                                FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref TradeReceived);
                                                //CRMSStructure.TradeDetails TradeResponse = new CRMSStructure.TradeDetails(string.Empty);
                                                //TradeResponse.TradeConfirm = cTradeConfirm;
                                                //TradeResponse.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(TradeResponse.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.TradeConfirmSize;//22+205=227
                                            }
                                            break;
                                        #endregion Trade Confirmation

                                        #region Batch Order Cancellation        Transcode:9002
                                        case (int)CConstants.TranCode.BatchOrderCancel:
                                            FnoCtclLib.Reference.LogInfo("Batch Order Cancellation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bCancelOrderConfirm = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bCancelOrderConfirm, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bCancelOrderConfirm);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());


                                                // Added For Sidharth Application
                                                // FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref OrderAckReceived);
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;//258
                                            }
                                            break;
                                        #endregion Batch Order Cancellation

                                        #region Order Freeze                Transcode:2170
                                        /*  Reason code ‘18’ denotes Quantity freeze and 
                                         * reason code ‘17’ denotes Price freeze.       */
                                        case (int)CConstants.TranCode.OrderFreeze:
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                //FnoCtclLib.Reference.LogInfo("Order Freeze");

                                                byte[] bOrderFreeze = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bOrderFreeze, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bOrderFreeze);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());

                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);

                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                               // FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref OrderAckReceived);
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();
                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;//22+236=258
                                            }
                                            break;
                                        #endregion

                                        #region New Order Confirmation      Transcode:2073
                                        /*  Reason code ‘17’ or ‘18’ denotes freeze approved/rejected.
                                                Reason code ‘0’ denotes normal confirmation             */
                                        case (int)CConstants.TranCode.NewOrderConfirmation:
                                        case (int)CConstants.TranCode.PriceConfirmation:

                                            //FnoCtclLib.Reference.LogInfo((TransCode == 2012) ? "Price Confirmation" : "New Order Confirmation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bNewOrderResponse = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bNewOrderResponse, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bNewOrderResponse);
                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());

                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);

                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref OrderAckReceived);
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;//22+236=258
                                            }
                                            break;
                                        #endregion

                                        #region Modify Order Acknowledgement      Transcode:2041
                                        case (int)CConstants.TranCode.ModifyOrderAck:
                                            //FnoCtclLib.Reference.LogInfo("Modify Order Acknowledgement");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bModifyOrderAck = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bModifyOrderAck, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bModifyOrderAck);

                                                //TODO: 
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();
                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;
                                            }
                                            break;
                                        #endregion

                                        #region Modify Order Confirmation            Transcode:2074
                                        case (int)CConstants.TranCode.ModifyOrderConfirmation:
                                            //FnoCtclLib.Reference.LogInfo("Modify Order Confirmation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bModifyOrderResponse = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bModifyOrderResponse, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bModifyOrderResponse);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());


                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                Interlocked.Increment(ref OrderAckReceived);
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();
                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize; //258
                                            }
                                            break;
                                        #endregion

                                        #region Cancel Order Acknowledgement        Transcode:2071,2132

                                        case (int)CConstants.TranCode.CancelOrderAck:
                                        case (int)CConstants.TranCode.SpreadOrderCancelAck:
                                            //FnoCtclLib.Reference.LogInfo("Cancel Order Acknowledgement");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bCancelOrderAck = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bCancelOrderAck, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bCancelOrderAck);

                                                //TODO: Fetch Important Info
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();
                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);
                                                

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;// 258
                                            }
                                            break;
                                        #endregion

                                        #region Cancel Order Confirmation            Transcode:2075,2132
                                        case (int)CConstants.TranCode.CancelOrderConfirmation:
                                            //FnoCtclLib.Reference.LogInfo("Cancel Order Confirmation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bCancelOrderConfirm = new byte[CConstants.OrderRequestSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bCancelOrderConfirm, 0, CConstants.OrderRequestSize);

                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;
                                                cOrderRequest.ByteToStruct(bCancelOrderConfirm);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cOrderRequest.ToString());


                                                // Added For Sidharth Application
                                                //FnoCtclLib.Order2L3L.SendQueueOrderConfirmation.EnQueue(cOrderRequest);
                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                Interlocked.Increment(ref OrderAckReceived);
                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                // FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.OrderRequestSize;//258
                                            }
                                            break;
                                        #endregion

                                        #region SysInfoResponse        Transcode:1601
                                        case (int)CConstants.TranCode.SysInfoPartialResponse:
                                        case (int)CConstants.TranCode.SysInfoResponse:
                                            {
                                                // for simulator
                                                //if (TransCode == 7321) break;
                                                FnoCtclLib.Reference.LogInfo("Sys Info Response");
                                                if (FnoCtclLib.LiveEnvTrueSimFalse == true)
                                                {
                                                    byte[] mkt = new byte[8];
                                                    Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize + CConstants.MsgHeaderSize, mkt, 0, 8);
                                                    CStructures.MarketStatus m1 = new CStructures.MarketStatus(0);
                                                    m1.ByteToStruct(mkt);

                                                    byte[] mkt1 = new byte[8];
                                                    Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize + CConstants.MsgHeaderSize + 8, mkt1, 0, 8);
                                                    CStructures.MarketStatus m2 = new CStructures.MarketStatus(0);
                                                    m2.ByteToStruct(mkt1);

                                                    byte[] mkt2 = new byte[8];
                                                    Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize + CConstants.MsgHeaderSize + 16, mkt2, 0, 8);
                                                    CStructures.MarketStatus m3 = new CStructures.MarketStatus(0);
                                                    m3.ByteToStruct(mkt2);
                                                    byte[] bSysInfoRes = new byte[CConstants.SysInfoResponse];
                                                    Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bSysInfoRes, 0, CConstants.SysInfoResponse);
                                                    CStructures.systemInfoResponse cSystemInfo = new CStructures.systemInfoResponse(0);
                                                    cSystemInfo.Prop01Header = Header;
                                                    cSystemInfo.Prop02InfoMarketResponse = m1;
                                                    cSystemInfo.Prop02aInfoMarketResponse = m2;// m2;
                                                    cSystemInfo.Prop02bnfoMarketResponse = m3;// m3;
                                                    cSystemInfo.ByteToStruct(bSysInfoRes);
                                                    SystemInfo = cSystemInfo;
                                                    // Stream no in alpha char field
                                                    FnoCtclLib.Reference.LogInfo("StreamNo:" + bSysInfoRes[8].ToString());
                                                    StreamNo = bSysInfoRes[8].ToString();
                                                    LastAckRecTime = Header.Prop03LogTime;
                                                    //lastAckTimeData.EnQueue(Header.Prop03LogTime);
                                                    //lastAckTimeData.AddCounter(1);


                                                }
                                                else if (FnoCtclLib.LiveEnvTrueSimFalse == false)
                                                {
                                                    byte[] mktsim = new byte[8];
                                                    Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize + CConstants.MsgHeaderSize, mktsim, 0, 8);
                                                    CStructures.MarketStatus m1sim = new CStructures.MarketStatus(0);
                                                    m1sim.Prop01Normal = 1;
                                                    m1sim.Prop02OddLot = 1;
                                                    m1sim.Prop03Spot = 1;
                                                    m1sim.Prop04Auction = 1;
                                                    byte[] bSysInfoRes = new byte[CConstants.SysInfoResponse];
                                                    Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bSysInfoRes, 0, CConstants.SysInfoResponse);
                                                    CStructures.systemInfoResponse cSystemInfo = new CStructures.systemInfoResponse(0);
                                                    cSystemInfo.Prop01Header = Header;
                                                    cSystemInfo.Prop02InfoMarketResponse = m1sim;
                                                    cSystemInfo.Prop02aInfoMarketResponse = m1sim;// m2;
                                                    cSystemInfo.Prop02bnfoMarketResponse = m1sim;// m3;
                                                    cSystemInfo.ByteToStruct(bSysInfoRes);
                                                    SystemInfo = cSystemInfo;
                                                    LastAckRecTime = Header.Prop03LogTime;
                                                    //lastAckTimeData.EnQueue(Header.Prop03LogTime);
                                                    //lastAckTimeData.AddCounter(1);

                                                    //string txt = FnoCtclLib.Reference.stwReader.ReadLine();
                                                    //FnoCtclLib.Reference.stwData.Write(txt.Replace(txt, LastAckRecTime.ToString()));
                                                    //FnoCtclLib.Reference.stwData.Flush();
                                                }

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SysInfoResponse;
                                                SendLocalDBUpdate();

                                            }
                                            break;
                                        #endregion

                                        #region LocalDBUpdHeader        Transcode:7307
                                        case (int)CConstants.TranCode.LocalDBUpdHeader:
                                            {
                                                FnoCtclLib.Reference.LogInfo("Local DB Download Started");

                                                byte[] bDBUpdateHeader = new byte[CConstants.LocalDBUpdateHeaderSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bDBUpdateHeader, 0, CConstants.LocalDBUpdateHeaderSize);

                                                CStructures.DownloadDbHeader cUpdateDBHeader = new CStructures.DownloadDbHeader(0);
                                                cUpdateDBHeader.Prop01Header = Header;
                                                cUpdateDBHeader.ByteToStruct(bDBUpdateHeader);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.LocalDBUpdateHeaderSize;
                                            }
                                            break;
                                        #endregion

                                        #region LocalDBUpdData     Transcode:7304
                                        case (int)CConstants.TranCode.LocalDBUpdData:
                                            {
                                                FnoCtclLib.Reference.LogInfo("Local DB Data Recieving");
                                                byte[] bDBUpdateData = new byte[Header.Prop10MessageLength];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bDBUpdateData, 0, bDBUpdateData.Length);

                                                CStructures.DownloadDbData cUpdateDBdata = new CStructures.DownloadDbData(0);
                                                cUpdateDBdata.Prop01Header = Header;
                                                cUpdateDBdata.ByteToStruct(bDBUpdateData);

                                                if (cUpdateDBdata.Prop01Header.Prop10MessageLength > (CConstants.MsgHeaderSize * 2))
                                                    ParseInData(cUpdateDBdata.Prop02Data);
                                                else
                                                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Problem in DB Download");

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.LocalDBData;
                                            }
                                            break;
                                        #endregion

                                        #region LocalDBUpdTrailer   Transcode:7308
                                        case (int)CConstants.TranCode.LocalDBUpdTrailer:
                                            {
                                                FnoCtclLib.Reference.LogInfo("Local DB Download Ended");
                                                byte[] bDBUpdateHeader = new byte[Header.Prop10MessageLength];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bDBUpdateHeader, 0, bDBUpdateHeader.Length);//CConstants.LocalDBUpdateHeaderSize);

                                                CStructures.DownloadDbHeader cUpdateDBHeader = new CStructures.DownloadDbHeader(0);
                                                cUpdateDBHeader.Prop01Header = Header;
                                                cUpdateDBHeader.ByteToStruct(bDBUpdateHeader);
                                                //FnoCtclLib.Reference.LogInfo("Scrip Count Received :- " + SecurityDetail.Count.ToString());
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.LocalDBUpdateHeaderSize;
                                                //SendDeltaDownloadReq(true);
                                                SendDeltaDownloadReqStreamwise(true);
                                            }
                                            break;
                                        #endregion

                                        #region LoginResponse       Transcode:2301
                                        case (int)CConstants.TranCode.LoginResponse:
                                            //FnoCtclLib.Reference.LogInfo("Incoming Logon Response");

                                            //Code for Password Change and update is remaining
                                            if (CConstants.NewPassword.Trim().Equals("") == false)
                                            {
                                                string newPwd = CConstants.NewPassword.Trim();
                                                CConstants.Password = newPwd;

                                                CConstants.NewPassword = "";

                                                FnoCtclLib.Reference.dt.Rows[0]["Password"] = newPwd;
                                                FnoCtclLib.Reference.WriteXmlFile();
                                                FnoCtclLib.Reference.LogInfo("Password changed Successfully.");
                                            }



                                            CConstants.LogOnTime = CConstants.BaseDateTime.AddSeconds(Header.Prop03LogTime);
                                            LoginDateTime = CConstants.BaseDateTime.AddSeconds(Header.Prop03LogTime);
                                            LogTime = Header.Prop03LogTime;
                                            LastDataRecTime = Header.Prop08TimeStamp1;

                                            FnoCtclLib.Reference.LogInfo("Logon Time :- " + CConstants.LogOnTime.ToString("dd/MM/yyyy HH:mm:ss"));
                                            FnoCtclLib.Reference.LogInfo("Last Data Rec. Time :- " + LastDataRecTime);
                                            byte[] ResponsePacket = new byte[CConstants.LoginReqResSize];
                                            Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, ResponsePacket, 0, CConstants.LoginReqResSize);

                                            CStructures.SignOn cLoginResponse = new CStructures.SignOn(0);
                                            cLoginResponse.Prop01Header = Header;
                                            cLoginResponse.ByteToStruct(ResponsePacket);
                                            // FnoCtclLib.Reference.LogInfo(cLoginResponse.ToString());
                                            BytesToRemove = CConstants.TAPHeaderSize + CConstants.LoginReqResSize;//22+220=242
                                            SendSysInfoReq();
                                            break;
                                        #endregion

                                        #region Delta Download Header  Transcode:7011
                                        case (int)CConstants.TranCode.DeltaDownloadHeader:
                                            {
                                                FnoCtclLib.Reference.LogInfo("Local Delta Download Started");
                                                //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Local Delta Download Started");

                                                byte[] bDeltaHeader = new byte[CConstants.MsgHeaderSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bDeltaHeader, 0, CConstants.MsgHeaderSize);
                                                CStructures.MessageHeader cDeltaHeader = new CStructures.MessageHeader(0);
                                                cDeltaHeader.ByteToStruct(bDeltaHeader);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.MsgHeaderSize;
                                            }
                                            break;
                                        #endregion

                                        #region Delta Download Record   Transcode:7021
                                        case (int)CConstants.TranCode.DeltaDownloadRecord:
                                            {
                                                FnoCtclLib.Reference.LogInfo("Local Delta Download in Process");

                                                byte[] bDeltaDownLoad = new byte[Header.Prop10MessageLength];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bDeltaDownLoad, 0, Header.Prop10MessageLength);

                                                CStructures.ResponseData cDeltaData = new CStructures.ResponseData(0);
                                                cDeltaData.Prop01OuterHeader = Header;
                                                cDeltaData.ByteToStruct(bDeltaDownLoad);

                                                if (cDeltaData.Prop01OuterHeader.Prop10MessageLength >= (CConstants.MsgHeaderSize * 2))
                                                    ParseInData(cDeltaData.Prop02Data);
                                                else
                                                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Problem in Delta Download");

                                                BytesToRemove = TapHeader.Prop01PacketLength;
                                                break;
                                            }
                                        #endregion

                                        #region Delta Download Trailer      Transcode:7031
                                        case (int)CConstants.TranCode.DeltaDownloadTrailer:
                                            {
                                                FnoCtclLib.Reference.LogInfo("Local Delta Download Ended");
                                                byte[] bDeltaTrailer = new byte[CConstants.MsgHeaderSize];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bDeltaTrailer, 0, CConstants.MsgHeaderSize);

                                                CStructures.MessageHeader cDeltaTrailer = new CStructures.MessageHeader(0);
                                                cDeltaTrailer.ByteToStruct(bDeltaTrailer);
                                                // Send Delta to Rms Server 
                                                CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                                                cOrderRequest.Prop01Header = Header;

                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.Order;
                                                ConfObj.Order = cOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                Interlocked.Increment(ref OrderAckReceived);
                                                if (bolLogin)
                                                    FnoCtclLib.Reference.LogInfo("Local Delta Download Ended");
                                                else
                                                {
                                                    bolLogin = true;
                                                    if (LoginSendData.PacketsCanSend > 0)
                                                    {
                                                        SendData.AddCounter(LoginSendData.PacketsCanSend);
                                                        LoginSendData.PacketsCanSend = 0;
                                                    }
                                                    LastAckRecTime = Header.Prop03LogTime;
                                                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Last Ack. Rec. Time :- " + Header.Prop03LogTime.ToString());
                                                    //lastAckTimeData.EnQueue(LastAckRecTime);
                                                    //lastAckTimeData.AddCounter(1);
                                                }
                                                if (bolLogin)
                                                {
                                                    if (tempSendData.GetSize() > 0)
                                                    {
                                                        lock (SendData)
                                                        {
                                                            byte[] bdata = null;
                                                            bdata = tempSendData.DeQueue(true);

                                                            while (bdata != null && bdata.Length > 0)
                                                            {
                                                                SendData.EnQueue(bdata);
                                                                if (tempSendData.GetSize() > 0)
                                                                    bdata = tempSendData.DeQueue(true);
                                                                else
                                                                    bdata = null;
                                                            }
                                                            tempSendData.Clear();
                                                        }
                                                    }
                                                }

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.MsgHeaderSize;
                                            }
                                            break;
                                        #endregion

                                        #region Exchange PortFolio Response      Transcode:1776
                                        case (int)CConstants.TranCode.ExchportfolioResponse:
                                            {
                                                FnoCtclLib.Reference.LogInfo("Exchange PortFolio Response Length:" + TotalBuffer.Length.ToString());
                                                byte[] bPortFolioResp = new byte[CConstants.ExchPortfolioResponse];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bPortFolioResp, 0, CConstants.ExchPortfolioResponse);

                                                CStructures.ExchangePortfolioResponse cPortFolioResp = new CStructures.ExchangePortfolioResponse(0);
                                                cPortFolioResp.Prop01Header = Header;
                                                cPortFolioResp.ByteToStruct(bPortFolioResp);

                                                BytesToRemove = TapHeader.Prop01PacketLength;

                                            }
                                            break;
                                        #endregion

                                        #region New Spread Order Confirmation      Transcode:2124
                                        case (int)CConstants.TranCode.SpreadOrderConfirmationResponse:
                                        case (int)CConstants.TranCode.OrderConfirmation2L:
                                        case (int)CConstants.TranCode.OrderConfirmation3L:
                                            //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Spread Order Confirmation Response");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bSpreadOrderResponse = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bSpreadOrderResponse, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bSpreadOrderResponse);

                                                //FnoCtclLib.Order2L3L.SendQueue2L3L.EnQueue(cSpreadOrderRequest);
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref SpreadAckReceived);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cSpreadOrderRequest.ToString());


                                                // FnoCtclLib.CTCLServer.SendQueue.EnQueue(cSpreadOrderRequest.ToString());

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest;//22+400=422
                                            }
                                            break;
                                        #endregion

                                        #region Modify spread Order Acknowledgement      Transcode:2119
                                        case (int)CConstants.TranCode.SpreadOrderModAck:
                                            //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Modify Spread Order Acknowledgement");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bModifySpreadOrderAck = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bModifySpreadOrderAck, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bModifySpreadOrderAck);
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref SpreadAckReceived);
                                                //TODO: 

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest;//400+22=422
                                            }
                                            break;
                                        #endregion

                                        #region Modify Spread Order Confirmation            Transcode:2136
                                        case (int)CConstants.TranCode.SpreadOrderModConfirmResponse:
                                            //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Modify Spread Order Confirmation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bModifySpreadOrderResponse = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bModifySpreadOrderResponse, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bModifySpreadOrderResponse);
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref SpreadAckReceived);
                                                //TO DO
                                                //CClientOrder ClientOrder = new CClientOrder();
                                                //ClientOrder.OrderRequest = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.InData);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest; //258
                                            }
                                            break;
                                        #endregion

                                        #region Modify Spread Order Rejected                Transcode:2133
                                        case (int)CConstants.TranCode.SpreadOrderModRejected:
                                            //  FnoCtclLib.Reference.LogInfo( "Modify Spread Order Rejected");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bModifySpreadOrderRejected = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bModifySpreadOrderRejected, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bModifySpreadOrderRejected);
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                Interlocked.Increment(ref SpreadAckReceived);
                                                // To Do

                                                //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                                                //ClientOrder.OrderEntry = cOrderRequest;
                                                //ClientOrder.ParseStruct();

                                                //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.InData);

                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest;
                                            }
                                            break;
                                        #endregion

                                        #region Spread Order Rejected             Transcode:2154,2155,2156
                                        case (int)CConstants.TranCode.SpreadOrderErrorResponse:
                                        case (int)CConstants.TranCode.OrderError2L:
                                        case (int)CConstants.TranCode.OrderError3L:
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Spread Order Rejected");

                                                byte[] bSpreadOrderError = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bSpreadOrderError, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bSpreadOrderError);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cSpreadOrderRequest.ToString());

                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                Interlocked.Increment(ref SpreadAckReceived);
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest;//22+400=422
                                            }
                                            break;
                                        #endregion

                                        #region Spread Order Cancel Acknowledgement         Transcode:2031,2131,2132
                                        case (int)CConstants.TranCode.SpreadOrderConfirmCancellationResponse:
                                        case (int)CConstants.TranCode.OrderCancelConfirm2L:
                                        case (int)CConstants.TranCode.OrderCancelConfirm3L:
                                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Cancel Spread Order Acknowledgement");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bCancelSpreadOrderAck = new byte[CConstants.SpreadOrderEntryRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bCancelSpreadOrderAck, 0, CConstants.SpreadOrderEntryRequest);

                                                CStructures.SpreadOrderRequest cSpreadOrderRequest = new CStructures.SpreadOrderRequest(0);
                                                cSpreadOrderRequest.prop01Header = Header;
                                                cSpreadOrderRequest.ByteToStruct(bCancelSpreadOrderAck);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cSpreadOrderRequest.ToString());
                                                Interlocked.Increment(ref SpreadAckReceived);
                                                //FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cSpreadOrderRequest);
                                                ConfObj = new CObject();
                                                ConfObj.type = CConstants.objecttype.SpreadOrder;
                                                ConfObj.SpreadOrder = cSpreadOrderRequest;
                                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);

                                                //FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.SpreadOrderEntryRequest;// 258
                                            }
                                            break;
                                        #endregion

                                        #region Exercise and Position Liquidation    Transcode:4002,4007,4010
                                        case (int)CConstants.TranCode.ExcerPoistionConfirm:
                                        case (int)CConstants.TranCode.ExcerciseModificationConfirm:
                                        case (int)CConstants.TranCode.ExerciseCancelConfirm:
                                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Exercise Confirmation");
                                            if (TotalBuffer.Length >= CConstants.TAPInvitationSize + CConstants.TAPHeaderSize)
                                            {
                                                byte[] bExercisePosition = new byte[CConstants.ExcercisePositionRequest];
                                                Array.ConstrainedCopy(TotalBuffer, CConstants.TAPHeaderSize, bExercisePosition, 0, CConstants.ExcercisePositionRequest);

                                                CStructures.LiquidationEntryReq cPositLiqRequest = new CStructures.LiquidationEntryReq(4000, "CA");
                                                cPositLiqRequest.Prop01Header = Header;
                                                cPositLiqRequest.ByteToStruct(bExercisePosition);

                                                if (CConstants.LogData == true)
                                                    FnoCtclLib.Reference.LogData(cPositLiqRequest.ToString());
                                                FnoCtclLib.Reference.ExcerciseQueue.EnQueue(cPositLiqRequest);
                                                BytesToRemove = CConstants.TAPHeaderSize + CConstants.ExcercisePositionRequest;// 258
                                            }
                                            break;
                                        #endregion
                                                                                        
                                    }
                                    #endregion Response
                                }

                                BytesToRemove = TapHeader.Prop01PacketLength;
                            }
                            else
                            {
                                FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Invalid Packet Received");
                                BytesToRemove = 0;
                            }
                        }
                        else
                            BytesToRemove = 0;
                    }
                    else
                        BytesToRemove = 0;

                    if (TotalBuffer.Length - BytesToRemove > 0)
                    {
                        Array.Resize(ref PartialBuffer, TotalBuffer.Length - BytesToRemove);
                        Array.Copy(TotalBuffer, BytesToRemove, PartialBuffer, 0, PartialBuffer.Length);
                        BytesToRemove = PartialBuffer.Length;
                    }
                    else
                        Array.Resize(ref PartialBuffer, 0);

                    Array.Resize(ref TotalBuffer, 0);
                }
                catch (ThreadAbortException ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Thread :- " + Thread.CurrentThread.Name + " is Stopped " + ex.Message);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Exception In ThreadParseData " + ex.Message.ToString() + " Stack Trace :- " + ex.StackTrace.ToString());
                }
            }
        }

        private void ParseInData(byte[] pData)
        {
            byte[] MessageHeader = new byte[CConstants.MsgHeaderSize];
            Array.ConstrainedCopy(pData, 0, MessageHeader, 0, CConstants.MsgHeaderSize);

            CStructures.MessageHeader cMsgHeader = new CStructures.MessageHeader(0);
            cMsgHeader.ByteToStruct(pData);

            if (cMsgHeader.Prop06ErrorCode > 0)
            {
                #region ErrorResponse
                byte[] bErrorResponse = new byte[CConstants.ErrorResponseSize];
                Array.ConstrainedCopy(pData, CConstants.TAPHeaderSize, bErrorResponse, 0, CConstants.ErrorResponseSize);

                CStructures.ErrorResponse errorResponse = new CStructures.ErrorResponse(0);
                errorResponse.ByteToStruct(bErrorResponse);

                FnoCtclLib.Reference.LogInfo("Error Code - " + cMsgHeader.Prop06ErrorCode.ToString());
                //FnoCtclLib.Reference.LogInfo("Error - " + errorResponse.ToString());
                FnoCtclLib.Reference.LogInfo("Error Msg. - " + errorResponse.Prop03ErrorMessage.ToString());
                #endregion ErrorResponse
            }
            else
            {
                int TransCode = cMsgHeader.Prop05TransactionCode;
                switch (TransCode)
                {
                    #region Security Status
                    case (int)CConstants.TranCode.SecurityStatus:
                        {
                            byte[] bData = new byte[cMsgHeader.Prop10MessageLength];
                            Array.ConstrainedCopy(pData, 0, bData, 0, cMsgHeader.Prop10MessageLength);

                            CStructures.SecurityStatus cSecurityStatus = new CStructures.SecurityStatus(0);
                            cSecurityStatus.Prop01Header = cMsgHeader;
                            cSecurityStatus.ByteToStruct(bData);

                            if (SecurityDetail == null)
                                SecurityDetail = new Dictionary<int, CStructures.TokenAndEligibility>();

                            foreach (KeyValuePair<int, CStructures.TokenAndEligibility> kvp in cSecurityStatus.Prop03TokenEligibility)
                            {
                                if (SecurityDetail.ContainsKey(kvp.Key) == false)
                                    SecurityDetail.Add(kvp.Key, kvp.Value);
                                else
                                    SecurityDetail[kvp.Key] = kvp.Value;
                            }
                        }
                        break;
                    #endregion Security Status

                    #region IndexMap
                    case (int)CConstants.TranCode.IndexMapTable:
                        {
                            byte[] bData = new byte[cMsgHeader.Prop10MessageLength];
                            Array.ConstrainedCopy(pData, 0, bData, 0, cMsgHeader.Prop10MessageLength);

                            CStructures.IndexMapTable cIndexMap = new CStructures.IndexMapTable(0);
                            cIndexMap.Prop01Header = cMsgHeader;
                            cIndexMap.ByteToStruct(bData);
                            //FnoCtclLib.Reference.LogInfo(cIndexMap.ToString());
                        }
                        break;
                    #endregion Security Status

                    #region Delta Download
                    case (int)CConstants.TranCode.LoginResponse:
                    case (int)CConstants.TranCode.LogOffResponse:
                        {
                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Delta Download Login Response.");
                        }
                        break;

                    case (int)CConstants.TranCode.MarketOpenMessage:
                    case (int)CConstants.TranCode.MarketCloseMessage:
                    case (int)CConstants.TranCode.MarketPreOpenShutDownMsg:
                    case (int)CConstants.TranCode.NormalMarketPreOpenEnd:
                        FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Delta Download Market Open Close Msg.");
                        break;

                    case (int)CConstants.TranCode.NewOrderConfirmation:
                    case (int)CConstants.TranCode.ModifyOrderConfirmation:
                    case (int)CConstants.TranCode.CancelOrderConfirmation:
                    case (int)CConstants.TranCode.NewOrderRejected:
                    case (int)CConstants.TranCode.ModifyOrderRejected:
                    case (int)CConstants.TranCode.CancelOrderRejected:
                        {
                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Delta Download Order Msg.");
                            CStructures.OrderEntryRequest cOrderRequest = new CStructures.OrderEntryRequest(0);
                            cOrderRequest.ByteToStruct(pData);
                            if (bolLogin)
                            {
                                ConfObj = new CObject();
                                ConfObj.type = CConstants.objecttype.Order;
                                ConfObj.Order = cOrderRequest;
                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                            }

                            //FnoCtclLib.Reference.AckQueue.EnQueue(cOrderRequest);

                            //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                            //ClientOrder.OrderEntry = cOrderRequest;
                            //ClientOrder.ParseStruct();

                            //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);
                        }
                        break;

                    case (int)CConstants.TranCode.OrderCancelConfirm2L:
                    case (int)CConstants.TranCode.OrderCancelConfirm3L:
                    case (int)CConstants.TranCode.OrderConfirmation2L:
                    case (int)CConstants.TranCode.OrderConfirmation3L:
                    case (int)CConstants.TranCode.OrderEntryAck2L:
                    case (int)CConstants.TranCode.OrderEntryAcl3L:
                    case (int)CConstants.TranCode.OrderError2L:
                    case (int)CConstants.TranCode.OrderError3L:
                    case (int)CConstants.TranCode.SpreadOrderCancelAck:                    
                    case (int)CConstants.TranCode.SpreadOrderCancelRejected:
                    case (int)CConstants.TranCode.SpreadOrderConfirmationResponse:
                    case (int)CConstants.TranCode.SpreadOrderErrorResponse:
                    case (int)CConstants.TranCode.SpreadOrderModAck:
                    case (int)CConstants.TranCode.SpreadOrderModConfirmResponse:
                    case (int)CConstants.TranCode.SpreadOrderModRejected:
                    case (int)CConstants.TranCode.SpreadOrderRequestedResponse:
                        {
                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Delta Download Order Msg.");
                            CStructures.SpreadOrderRequest cOrderRequest = new CStructures.SpreadOrderRequest(0);
                            cOrderRequest.ByteToStruct(pData);

                            ConfObj = new CObject();
                            ConfObj.type = CConstants.objecttype.SpreadOrder;
                            ConfObj.SpreadOrder = cOrderRequest;
                            FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                            FnoCtclLib.Reference.AckSpreadQueue.EnQueue(cOrderRequest);

                            // if (bolLogin)                           

                            //CRMSStructure.CTCLOrderEntry ClientOrder = new CRMSStructure.CTCLOrderEntry(string.Empty);
                            //ClientOrder.OrderEntry = cOrderRequest;
                            //ClientOrder.ParseStruct();

                            //CurrCtclLib.CTCLServer.SendQueue.EnQueue(ClientOrder.Indata);
                        }
                        break;

                    case (int)CConstants.TranCode.TradeConfirmation:
                    case (int)CConstants.TranCode.StopLossTriggered:
                        {
                            FnoCtclLib.Reference.LogInfo("Delta Download Trade/SL Msg.");
                            CStructures.TradeConfirm cTradeConfirm = new CStructures.TradeConfirm(0);
                            cTradeConfirm.ByteToStruct(pData);

                            if (bolLogin)
                            {
                                ConfObj = new CObject();
                                ConfObj.type = CConstants.objecttype.Trade;
                                ConfObj.Trade = cTradeConfirm;
                                FnoCtclLib.Reference.AllQueue.EnQueue(ConfObj);
                            }
                                
                                //FnoCtclLib.Reference.TradeQueue.EnQueue(cTradeConfirm);                            F
                           // FnoCtclLib.Reference.LogInfo("Queue Count: " + FnoCtclLib.Reference.AllQueue.GetSize().ToString());
                            //CServerResponse ServerResponse = new CServerResponse();
                            //ServerResponse. TradeConfirm = cTradeConfirm;
                            //ServerResponse.ParseStruct();

                            //FnoCtclLib.CTCLServer.SendQueue.EnQueue(ServerResponse.InData);
                        }
                        break;                  
                    #endregion Delta Download

                    default:
                        FnoCtclLib.Reference.LogInfo("UnKnown Trans Code :- " + TransCode.ToString() + " Length: " + cMsgHeader.Prop10MessageLength.ToString());
                        FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "UnKnown Trans Code :- " + TransCode.ToString());
                        break;
                }
            }
        }

        void ThreadCallDisconnect()
        {
            try
            {
                Thread.Sleep(10);
                OnDisconnected("", "");
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception in ThreadCallDisconnect()" + ex.Message);
            }
            finally
            {
                ThreadDisconnect = null;
                FnoCtclLib.Reference.Reconnect = true;
            }
        }

        /// <summary>
        ///Check packet and MD5 validation
        /// </summary>
        /// <param name="bTotalBuffer"></param>
        /// <returns></returns>
        private bool CheckCompletePacket(byte[] bTotalBuffer)
        {

            if (bTotalBuffer.Length >= CConstants.MinPacketSize)//>(2+4+16+38=60)
            {
                byte[] bTapHeader = new byte[CConstants.TAPHeaderSize];
                Array.ConstrainedCopy(bTotalBuffer, 0, bTapHeader, 0, CConstants.TAPHeaderSize);

                CStructures.TapHeader TapHeader = new CStructures.TapHeader();
                TapHeader.ByteToStruct(bTapHeader);

                byte[] ActualData = new byte[TapHeader.Prop01PacketLength - CConstants.TAPHeaderSize];

                if (TapHeader.Prop01PacketLength <= bTotalBuffer.Length)
                {
                    Array.ConstrainedCopy(bTotalBuffer, CConstants.TAPHeaderSize, ActualData, 0, TapHeader.Prop01PacketLength - CConstants.TAPHeaderSize);

                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] ActualCheckSum = md5.ComputeHash(ActualData);

                    byte[] MessageHeader = new byte[CConstants.MsgHeaderSize];
                    Array.ConstrainedCopy(bTotalBuffer, CConstants.TAPHeaderSize, MessageHeader, 0, CConstants.MsgHeaderSize);

                    if (CUtility.CheckPacket(TapHeader.Prop03MD5CheckSum, ActualCheckSum) == true)
                    {
                        CStructures.MessageHeader Header = new CStructures.MessageHeader(0);
                        Header.ByteToStruct(MessageHeader);
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// Send data to Tap Server 
        /// </summary>
        private void ThreadSendData()
        {
            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "ProcessData :" + ProcessData.ToString());
            while (ProcessData)
            {
                try
                {

                    byte[] data = SendData.DeQueue(true);// Overloaded Method Implemented
                    Send(socket, data);
                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Data Send to Tap Server.Length: " + data.Length.ToString());
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Exception In ThreadSendData " + ex.Message);
                }
            }
        }

        //private void ThreadLastAckSerialise()
        //{
        //    //int lastAckTime = 0;
        //    //try
        //    //{
        //    //    while (true)
        //    //    {
        //    //        try
        //    //        {
        //    //            lastAckTime = lastAckTimeData.DeQueue();
        //    //            SerializeLastAckReceived(lastAckTime);
        //    //        }
        //    //        catch (ThreadAbortException ex)
        //    //        {
        //    //            FnoCtclLib.Reference.LogException(ex,"Thread :- " + Thread.CurrentThread.Name + ex.Message);
        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            FnoCtclLib.Reference.LogException(ex,"Exception in ThreadLastAckSerilize().while() " + ex.Message + " Trace :- " + ex.StackTrace);
        //    //        }
        //    //    }
        //    //}
        //    //catch (ThreadAbortException ex)
        //    //{
        //    //    FnoCtclLib.Reference.LogException(ex,"Thread :- " + Thread.CurrentThread.Name + ex.Message);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    FnoCtclLib.Reference.LogException(ex,"Exception in ThreadLastAckSerilize() " + ex.Message + " Trace :- " + ex.StackTrace);
        //    //}
        //}

        private void ThreadSendLoginData()
        {
            while (ProcessData)
            {
                try
                {
                    byte[] data = LoginSendData.DeQueue(true);
                    Send(socket, data); // socket.Send(data);
                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Send Login Data Size :- " + data.Length);
                    //Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Exception In ThreadSendLoginData " + ex.Message);
                }
            }
        }

        #endregion

        #region Request
        /// <summary>
        /// System Info Request
        /// </summary>
        internal void SendSysInfoReq()
        {
            CStructures.SystemInfoRequest SysInfoReq;
            try
            {
                SysInfoReq = new CStructures.SystemInfoRequest(0);
                CStructures.MessageHeader Header = new CStructures.MessageHeader(1600);
                Header.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                Header.Prop03LogTime = 0;
                Header.Prop04AlphaChar = string.Empty;
                Header.Prop05TransactionCode = (int)CConstants.TranCode.SysInfoRequest;
                Header.Prop06ErrorCode = 0;
                Header.Prop07TimeStamp = "0";
                Header.Prop08TimeStamp1 = "0";
                Header.Prop09TimeStamp2 = "0";
                Header.Prop10MessageLength = (Int16)CConstants.SysInfoReq;
                SysInfoReq.Prop01Header = Header;
                SysInfoReq.Prop02LastPortfolioUpd = LogTime;//0;
                SendRequest(SysInfoReq.StructToByte(), !bolLogin);
                //FnoCtclLib.Reference.LogInfo(SysInfoReq.ToString());
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception In FormLogOffRequest. Msg:- " + ex.Message + " Trace :- " + ex.StackTrace);
            }
        }
        /// <summary>
        /// Login Request 
        /// </summary>
        internal bool SendLoginRequest()
        {
            try
            {

                CStructures.SignOn cLoginRequest = new CStructures.SignOn(2300);
                CStructures.MessageHeader Header = new CStructures.MessageHeader(2300);
                Header.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                Header.Prop03LogTime = 0;
                Header.Prop04AlphaChar = " ";
                Header.Prop05TransactionCode = (int)CConstants.TranCode.LoginRequest;
                Header.Prop06ErrorCode = 0;
                Header.Prop07TimeStamp = "0";
                Header.Prop08TimeStamp1 = "0";
                Header.Prop09TimeStamp2 = "0";
                Header.Prop10MessageLength = CConstants.LoginReqResSize;

                cLoginRequest.Prop01Header = Header;
                cLoginRequest.Prop02UserId = CUtility.ConvertToInt16(CConstants.LoginId);
                cLoginRequest.Prop03Password = CConstants.Password;
                cLoginRequest.Prop04NewPassword = CConstants.NewPassword;
                cLoginRequest.Prop07Brokerid = CConstants.BrokerId;
                cLoginRequest.Prop09Branchid = CUtility.ConvertToInt16(CConstants.BranchId);
                cLoginRequest.Prop10VersionNumber = CConstants.TapVersion;//90100;
                cLoginRequest.Prop17WSClassName = CConstants.WsClassName;//"1727410       ";
                cLoginRequest.Prop23BrokerName = CConstants.BrokerName;//"Sharekhan                 ";

                SendRequest(cLoginRequest.StructToByte(), !bolLogin);
                //Program.LogData.EnQueue("I" + CConstants.LogSeparator + "Login Data :" + cLoginRequest.ToString());
                return (true);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception In FormLogOffRequest. Msg:- " + ex.Message + " Trace :- " + ex.StackTrace);
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
                CStructures.MessageHeader LogOffReq = new CStructures.MessageHeader((Int32)CConstants.TranCode.LogOffRequest);
                LogOffReq.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                LogOffReq.Prop03LogTime = LogTime;
                LogOffReq.Prop04AlphaChar = " ";
                LogOffReq.Prop05TransactionCode = (int)CConstants.TranCode.LogOffRequest;
                LogOffReq.Prop06ErrorCode = 0;
                LogOffReq.Prop07TimeStamp = "0";
                LogOffReq.Prop08TimeStamp1 = "0";
                LogOffReq.Prop09TimeStamp2 = "0";
                LogOffReq.Prop10MessageLength = (Int16)CConstants.MsgHeaderSize;
                SendRequest(LogOffReq.StructToByte(), false);
                return (true);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception In FormLogOffRequest. Msg:- " + ex.Message + " Trace :- " + ex.StackTrace);
                return (false);
            }
        }
        /// <summary>
        /// Local Database Update Request
        /// </summary>
        internal bool SendLocalDBUpdate()
        {
            try
            {
                CStructures.UpdateLocalDB cUpdateDBRequest = new CStructures.UpdateLocalDB(7300);
                CStructures.MessageHeader Header = new CStructures.MessageHeader(7300);
                Header.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                Header.Prop03LogTime = 0;
                Header.Prop04AlphaChar = string.Empty;
                Header.Prop05TransactionCode = (int)CConstants.TranCode.LocalDBUpdRequest;
                Header.Prop06ErrorCode = 0;
                Header.Prop07TimeStamp = "0";
                Header.Prop08TimeStamp1 = "0";
                Header.Prop09TimeStamp2 = "0";
                Header.Prop10MessageLength = (Int16)CConstants.LocalDBUpdateReqSize;

                cUpdateDBRequest.Prop01Header = Header;
                cUpdateDBRequest.Prop02LastUpdSecurityTime = LatestTime;
                cUpdateDBRequest.Prop03LastParticipantTime = LatestTime; //0;
                cUpdateDBRequest.Prop04LastUpdInstrumentTime = LatestTime;// 0;
                cUpdateDBRequest.Prop05LastUpdIndexTime = LatestTime;//0;
                cUpdateDBRequest.Prop06RequestForOpenOrder = "G";
                cUpdateDBRequest.Prop08LocalDbStatus = SystemInfo.Prop02InfoMarketResponse;
                cUpdateDBRequest.Prop09LocalDbExStatus = SystemInfo.Prop02aInfoMarketResponse;// new CStructures.MarketStatus(0);//
                cUpdateDBRequest.Prop10LocalDbPlStatus = SystemInfo.Prop02bnfoMarketResponse;

                SendRequest(cUpdateDBRequest.StructToByte(), !bolLogin);
                //FnoCtclLib.Reference.LogInfo(cUpdateDBRequest.ToString());
                return (true);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception In FormLogOffRequest. Msg:- " + ex.Message + " Trace :- " + ex.StackTrace);
                return (false);
            }
        }
        /// <summary>
        /// MessageDownloadRequest:46 byte
        /// Overloaded Method for Delta Download
        /// one Rms Server and other For login Process 
        /// </summary>
        internal bool SendDeltaDownloadReq(bool islogin)
        {
            try
            {
                CStructures.MessageDownloadRequest cMsgDownloadRequest = new CStructures.MessageDownloadRequest(0);
                CStructures.MessageHeader Header = new CStructures.MessageHeader(7000);
                Header.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                Header.Prop03LogTime = LogTime;
                Header.Prop04AlphaChar = string.Empty;
                Header.Prop05TransactionCode = (int)CConstants.TranCode.DeltaDownloadReq;
                Header.Prop06ErrorCode = 0;
                Header.Prop07TimeStamp = "0";
                Header.Prop08TimeStamp1 = "0";
                Header.Prop09TimeStamp2 = "0";
                Header.Prop10MessageLength = (Int16)CConstants.MsgDownloadReqSize;

                if (FnoCtclLib.LiveEnvTrueSimFalse == true)
                {
                    if (int.Parse(StreamNo) > 0)
                        Header.Prop04AlphaChar = "1 "; //StreamNo;
                }
                else
                    Header.Prop04AlphaChar = string.Empty;

                cMsgDownloadRequest.Prop01Header = Header;
                //Delta download for Current Time Stamp
                if (LastAckRecTime > 0)
                {
                    TimeSpan ts = DateTime.Now.Subtract(CConstants.BaseDateTime);
                    cMsgDownloadRequest.iSequenceNo = Math.Round(ts.TotalSeconds);
                }
                else
                {
                    // TimeSpan ts = CConstants.TodayDate.Subtract(CConstants.BaseDateTime);
                    TimeSpan ts = DateTime.Now.Subtract(CConstants.BaseDateTime);
                    cMsgDownloadRequest.iSequenceNo = Math.Round(ts.TotalSeconds);
                }
                SendRequest(cMsgDownloadRequest.StructToByte(), islogin);
                return (true);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception In SendDeltaDownloadReq.Msg:- " + ex.Message + " Trace :- " + ex.StackTrace);
                return (false);
            }
        }

        internal void SendDeltaDownloadReq(string sParam, bool isLogin)
        {
            try
            {
                Int64 iParam = Int64.Parse(sParam.Trim());
                CStructures.MessageDownloadRequest cMsgDownloadRequest = new CStructures.MessageDownloadRequest(0);
                CStructures.MessageHeader Header = new CStructures.MessageHeader(7000);
                if (iParam > 0)
                {
                    double lasttime = 0;
                    double.TryParse(iParam.ToString(), out lasttime);
                    cMsgDownloadRequest.iSequenceNo = Math.Round(lasttime);
                }
                else
                {
                    TimeSpan ts = DateTime.Now.Subtract(CConstants.BaseDateTime);
                    cMsgDownloadRequest.iSequenceNo = Math.Round(ts.TotalSeconds);
                }
                Header.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                Header.Prop03LogTime = LogTime;
                Header.Prop04AlphaChar = string.Empty;
                Header.Prop05TransactionCode = (int)CConstants.TranCode.DeltaDownloadReq;
                Header.Prop06ErrorCode = 0;
                Header.Prop07TimeStamp = "0";
                Header.Prop08TimeStamp1 = "0";
                Header.Prop09TimeStamp2 = "0";
                Header.Prop10MessageLength = (Int16)CConstants.MsgDownloadReqSize;
                if (StreamNo != string.Empty)
                {
                    if (int.Parse(StreamNo) > 0)
                    {
                        int stream = int.Parse(StreamNo);
                        for (int i = 1; i <= stream; i++)
                        {
                            Header.Prop04AlphaChar = i.ToString();
                            cMsgDownloadRequest.Prop01Header = Header;
                            SendRequest(cMsgDownloadRequest.StructToByte(), false);
                            FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + " Local Delta Download Request for Stream No: " + i.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception In FormLogOffRequest.Msg:- " + ex.Message + " Trace :- " + ex.StackTrace);
            }
        }


        public void SendDeltaDownloadReqStreamwise(bool isLogin)
        {
            try
            {
                string sParam = string.Empty;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Data.txt"))
                {
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Data.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader str = new StreamReader(fs);
                    sParam = str.ReadLine();
                }
                Int64 iParam = Int64.Parse(sParam.Trim());
                CStructures.MessageDownloadRequest cMsgDownloadRequest = new CStructures.MessageDownloadRequest(0);
                CStructures.MessageHeader Header = new CStructures.MessageHeader(7000);
                if (iParam > 0 && FNOCTCL.CConstants.BaseDateTime.AddSeconds(iParam).ToString("dd-MM-yyyy") == DateTime.Now.ToString("dd-MM-yyyy"))
                {
                    double lasttime = 0;
                    double.TryParse(iParam.ToString(), out lasttime);
                    cMsgDownloadRequest.iSequenceNo = Math.Round(lasttime);
                }
                else
                {
                    TimeSpan ts = DateTime.Now.Subtract(CConstants.BaseDateTime);
                    cMsgDownloadRequest.iSequenceNo = 0;// Math.Round(ts.TotalSeconds);
                }

                Header.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                Header.Prop03LogTime = LogTime;
                Header.Prop04AlphaChar = string.Empty;
                Header.Prop05TransactionCode = (int)CConstants.TranCode.DeltaDownloadReq;
                Header.Prop06ErrorCode = 0;
                Header.Prop07TimeStamp = "0";
                Header.Prop08TimeStamp1 = "0";
                Header.Prop09TimeStamp2 = "0";
                Header.Prop10MessageLength = (Int16)CConstants.MsgDownloadReqSize;

                //Header.Prop04AlphaChar = i.ToString();
                //cMsgDownloadRequest.Prop01Header = Header;
                //SendRequest(cMsgDownloadRequest.StructToByte(), isLogin);
                //FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + " Local Delta Download Request for Stream No: ");

                if (FnoCtclLib.LiveEnvTrueSimFalse == true)
                {
                    if (StreamNo != string.Empty)
                    {
                        if (int.Parse(StreamNo) > 0)
                        {
                            int stream = int.Parse(StreamNo);
                            for (int i = 1; i <= stream; i++)
                            {
                                Header.Prop04AlphaChar = i.ToString();
                                cMsgDownloadRequest.Prop01Header = Header;
                                SendRequest(cMsgDownloadRequest.StructToByte(), isLogin);
                                FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + " Local Delta Download Request for Stream No: " + i.ToString());
                            }
                        }
                    }
                }
                else
                {
                    Header.Prop04AlphaChar = string.Empty;
                    cMsgDownloadRequest.Prop01Header = Header;
                    SendRequest(cMsgDownloadRequest.StructToByte(), isLogin);
                }

            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception In SendDeltaDownloadReqStreamwise.Msg:- " + ex.Message + " Trace :- " + ex.StackTrace);
            }
        }


        /// <summary>
        /// SendRequest
        /// </summary>
        /// <param name="sendData">pass Byte[]</param>
        public void SendRequest(byte[] sendData, bool islogin)
        {
            try
            {

                MD5 md5 = new MD5CryptoServiceProvider();
                CStructures.TapHeader cTapHeader = new CStructures.TapHeader(TAPSequenceNumber);
                cTapHeader.Prop01PacketLength = (Int16)(CConstants.TAPHeaderSize + sendData.Length);
                cTapHeader.Prop02SequenceNumber = TAPSequenceNumber;
                cTapHeader.Prop03MD5CheckSum = md5.ComputeHash(sendData);

                if (bolLogin == false)
                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "TAP Sequence Number (Sending) :- " + TAPSequenceNumber.ToString() + " Packet Can Be Send :- " + LoginSendData.PacketsCanSend.ToString() + " Q Size :- " + LoginSendData.GetSize().ToString());
                else
                    FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "TAP Sequence Number (Sending) :- " + TAPSequenceNumber.ToString() + " Packet Can Be Send :- " + SendData.PacketsCanSend.ToString() + " Q Size :- " + SendData.GetSize().ToString());

                Interlocked.Increment(ref TAPSequenceNumber);
                byte[] bToSend = new byte[CConstants.TAPHeaderSize + sendData.Length];
                Array.ConstrainedCopy(cTapHeader.StructToByte(), 0, bToSend, 0, CConstants.TAPHeaderSize);
                Array.ConstrainedCopy(sendData, 0, bToSend, CConstants.TAPHeaderSize, sendData.Length);
                //printHex(bToSend);
                if (islogin == true)
                    LoginSendData.EnQueue(bToSend);
                else
                    SendData.EnQueue(bToSend);
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Exception in SendRequest(sendData).");
            }
        }
        /// <summary>
        /// New Order Request 
        /// </summary>
        public void SendNewOrderRequest()
        {
            try
            {

                Int16 Userid = CUtility.ConvertToInt16(CConstants.LoginId);
                String Brokerid = CConstants.BrokerId;
                Int16 Branchid = CUtility.ConvertToInt16(CConstants.BranchId);
                CStructures.OrderEntryRequest OrderReq = new CStructures.OrderEntryRequest((Int16)CConstants.TranCode.NewOrderRequest);
                //FnoCtclLib.Reference.LogInfo(OrderReq.ToString());
                OrderReq.StructToByte();
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, ex.Source + ex.Message + " Trace :- " + ex.StackTrace);
            }
        }

        public void SendExchangePortfolioRequest()
        {
            try
            {
                CStructures.MessageHeader Header = new CStructures.MessageHeader(1775);
                Header.Prop01Reserved = "";
                //Header.Prop02Apifunction="";
                Header.Prop03LogTime = 0;
                Header.Prop04AlphaChar = string.Empty;
                Header.Prop05TransactionCode = (int)CConstants.TranCode.ExchPortfolioReq;
                Header.Prop06ErrorCode = 0;
                Header.Prop07TimeStamp = "0";
                Header.Prop08TimeStamp1 = "0";
                Header.Prop09TimeStamp2 = "0";
                Header.Prop10MessageLength = (Int16)CConstants.ExchPortfolioRequest;

                CStructures.ExchangePortfolioRequest PortfolioReq = new CStructures.ExchangePortfolioRequest(0);
                PortfolioReq.Prop01Header = Header;
                PortfolioReq.Prop02LastUpdateDtTime = 0;
                SendRequest(PortfolioReq.StructToByte(), false);
                FnoCtclLib.Reference.LogInfo(DateTime.Now.ToString("HH:mm:ss ffff") + "$" + "Exchange Portfolio Requested");
            }
            catch (Exception ex)
            {
                FnoCtclLib.Reference.LogException(ex, "Message :" + ex.Source + ex.Message + " Trace :- " + ex.StackTrace);
            }
        }

        #endregion

        #region StateClass

        /// <summary>
        /// State Object class is created to preserve values during Asynochronous Receive From Server.
        /// </summary>
        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
        }

        #endregion

        public void printHex(byte[] a)
        {
            string temp = "";
            for (int i = 0; i < a.Length; i++)
            {
                temp += a[i].ToString("X2") + " ";
            }
            FnoCtclLib.Reference.LogInfo("Bytestruct  " + temp);
            FnoCtclLib.Reference.LogInfo(temp);
        }

        public void UpdAckTextfile()
        {
            while (true)
            {
                int Result = lastAckTimeData.DeQueue();
                try
                {
                    FSData = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Data.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    stwData = new StreamWriter(FSData);
                    stwData.BaseStream.Seek(0, SeekOrigin.Begin);
                    stwData.Write(Result.ToString());
                    stwData.Flush();
                }
                catch (Exception ex)
                {
                    FnoCtclLib.Reference.LogException(ex, "Exception In UpdAckTextfile()" + ex.Message);
                }
                finally
                {
                    stwData.Close();
                    FSData.Close();
                }


            }
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