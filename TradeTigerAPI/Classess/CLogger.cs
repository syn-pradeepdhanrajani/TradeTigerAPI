using System;
using System.Collections.Generic;
using System.Text;
using NSpring.Logging;
using System.Threading;
using System.Linq;

namespace TradeTigerAPI
{
     class CLogger
    {
        //public List<Thread> ListOfThreads;
        //public int NoThread = 0;
        //public CQueue<String> LoggerQ;

        private static CLogger Objlog = new CLogger();
        private static CLogger objDataLog = new CLogger();
        private Logger log;

        /// <summary>
        /// Defalut Constructor
        /// </summary>
        private CLogger() { }

        /// <summary>
        /// Returns Single Static Object
        /// </summary>
        public static CLogger Reference
        {
            get { return Objlog; }
        }

        public static CLogger ReferenceDataLog
        {
            get { return objDataLog; }
        }


        //public static CLogger BFReferance
        //{
        //    get { return BFLog; }
        //}

        /// <summary>
        /// Initialise Log File With Given Buffer Size.
        /// </summary>
        /// <param name="FileName">Log File Name</param>
        /// <param name="BufferSize">Buffer Size</param>
        public void LogInitilize(string FileName, int BufferSize)
        {
            //NoThread = 1;
            //LoggerQ = new CQueue<string>();
            //ListOfThreads = new List<Thread>();
            log = Logger.CreateFileLogger(FileName);
            log.IsBufferingEnabled = true;
            log.BufferSize = BufferSize;
            log.Open();
            //StartListOfThread();
        }

        /// <summary>
        /// Closing Log File.
        /// </summary>
        public void LogClose()
        {
            log.Close();
        }

        /// <summary>
        /// Debugging Log.
        /// </summary>
        /// <param name="sMessage">Debug Message</param>
        public void LogDebug(object sMessage)
        {
            log.Log(Level.Debug, sMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sMessage">Message</param>
        public void LogVerbose(object sMessage)
        {
            log.Log(Level.Verbose, sMessage);
        }

        /// <summary>
        /// Creates Config Level
        /// </summary>
        /// <param name="sMessage">Config Message</param>
        public void LogConfig(object sMessage)
        {
            log.Log(Level.Config, sMessage);
        }

        /// <summary>
        /// Informative Log.
        /// </summary>
        /// <param name="sMessage">String Message</param>
        public void LogInfo(object sMessage)
        {
            log.Log(Level.Info, sMessage);
        }

        /// <summary>
        /// Warning Log
        /// </summary>
        /// <param name="sMessage">String Message</param>
        public void LogWarning(object sMessage)
        {
            log.Log(Level.Warning, sMessage);
        }

        /// <summary>
        /// Exception Level
        /// </summary>
        /// <param name="sMessage">Message Object</param>
        public void LogException(object sMessage)
        {
            log.Log(Level.Exception, sMessage);
        }

        /// <summary>
        /// Exception Log
        /// </summary>
        /// <param name="Ex">Exception Instance</param>
        public void LogException(Exception Ex)
        {
            LogException(" Exception " + Ex.Message + " Trace " + Ex.StackTrace);
            LogException(Environment.NewLine);
        }


        #region Threads
        //public void StartListOfThread()
        //{
        //    try
        //    {

        //        if (ListOfThreads.Count > 0)
        //        {
        //            StopListOfthread(ref ListOfThreads);
        //            ListOfThreads.Clear();
        //        }

        //        for (Int32 i = 0; i < NoThread; i++)
        //        {
        //            Thread tProcess = new Thread(new ThreadStart(ThreadLog));
        //            if (tProcess != null)
        //            {
        //                ListOfThreads.Add(tProcess);
        //                tProcess.Name = "ThreadSend" + i.ToString();
        //                tProcess.IsBackground = true;
        //                tProcess.Start();
        //            }
        //        }
        //    }
        //    catch (ThreadStateException ex)
        //    {
        //        LogException("ThreadStateException in StartThread.);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        LogException("InvalidOperationException in StartThread.);
        //    }
        //    catch (System.Security.SecurityException ex)
        //    {
        //        LogException("SecurityException in StartThread. );
        //    }
        //    catch (OutOfMemoryException ex)
        //    {
        //        LogException("OutOfMemoryException in StartThread.);
        //    }
        //    catch (ThreadAbortException ex)
        //    {
        //        LogException("ThreadAbortException in StartThread.);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogException("Exception in StartThread.);
        //    }
        //}

        //public void StopListOfthread(ref List<Thread> threadlist)
        //{
        //    foreach (Thread tProcess in threadlist)
        //    {
        //        if (tProcess == null)
        //            return;
        //        try
        //        {
        //            tProcess.Abort();
        //        }
        //        catch (System.Security.SecurityException ex)
        //        {
        //            LogException("SecurityException in ThreadsStop);
        //        }
        //        catch (ThreadStateException ex)
        //        {
        //            LogException("ThreadStateException in ThreadsStop);
        //        }
        //        catch (ThreadAbortException ex)
        //        {
        //            LogException("ThreadAbortException in ThreadsStop);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogException("Exception in ThreadsStop);
        //        }
        //        //finally
        //        //{
        //        //    tProcess = null;
        //        //}
        //    }
        //}


        //public void ThreadLog()
        //{
        //    string data = string.Empty;
        //    string logdata = string.Empty;

        //    while (true)
        //    {
        //        try
        //        {
        //            logdata = LoggerQ.DeQueue();
        //            data = logdata.Substring(2);

        //            switch (logdata.Substring(0, 1))
        //            {
        //                case "I":
        //                    Program.LogData.EnQueue("I" + CConstants.LogSeparator+data);
        //                    break;
        //                case "E":
        //                   Program.LogData.EnQueue("E" + CConstants.LogSeparator+data);
        //                    break;
        //                case "W":
        //                    CLogger.Reference.LogWarning(data);
        //                    break;
        //                case "D":
        //                    CLogger.Reference.LogDebug(data);
        //                    break;
        //                case "V":
        //                    CLogger.Reference.LogVerbose(data);
        //                    break;
        //                case "C":
        //                    CLogger.Reference.LogConfig(data);
        //                    break;
        //            }
        //        }
        //        catch (ArgumentOutOfRangeException ex)
        //        {
        //            LogException(Thread.CurrentThread.Name + " ArgumentOutOfRangeException in ThreadSendData " + ex.Message + " Trace :- " + ex.StackTrace);
        //        }
        //        catch (ArgumentNullException ex)
        //        {
        //            LogException(Thread.CurrentThread.Name + " ArgumentNullException in ThreadSendData " + ex.Message + " Trace :- " + ex.StackTrace);
        //        }
        //        catch (ArgumentException ex)
        //        {
        //           Program.LogData.EnQueue("E" + CConstants.LogSeparator+Thread.CurrentThread.Name + " ArgumentException in ThreadSendData " + ex.Message + " Trace :- " + ex.StackTrace);
        //        }
        //        catch (ThreadAbortException ex)
        //        {
        //            LogInfo("Thread :- " + Thread.CurrentThread.Name + " is Stopped " + ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogException(Thread.CurrentThread.Name + " Exception in ThreadSendData " + ex.Message + " Trace :- " + ex.StackTrace);
        //        }
        //    }
        //}
        #endregion

       
    }
}
