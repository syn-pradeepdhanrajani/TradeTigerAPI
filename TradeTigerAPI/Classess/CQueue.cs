using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;

namespace TradeTigerAPI
{
   public class CQueue<T>
    {
        private Queue<T> DataQ;
        

        #region Packet
        private int iPackets;

        public int PacketsCanSend
        {
            get { return iPackets; }
            set { iPackets = value; }
        }

        public void IncrementCounter()
        {
            Interlocked.Increment(ref iPackets);
            if (iPackets > 0)
                lock (DataQ)
                {
                    Monitor.PulseAll(DataQ);
                }
            
        }

        public void DecrementCounter()
        {
            if (iPackets > 0)
                Interlocked.Decrement(ref iPackets);
            else
                return;
        }

        public void AddCounter(int iValue)
        {
            Interlocked.Add(ref iPackets, iValue);
            if (iPackets > 0)
                lock (DataQ)
                {
                    Monitor.PulseAll(DataQ);
                }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CQueue()
        {
        DataQ=new Queue<T>();
        }

        /// <summary>
        /// To Populate the value in Queue
        /// </summary>
        public void EnQueue(T data)
        {
            lock (DataQ)
            {
                DataQ.Enqueue(data);
                try
                {
                    Monitor.PulseAll(DataQ);
                }
                catch (Exception ex)
                {
                    LogFile.Reference.WriteLogFile(ex.StackTrace.ToString(), ex.Message.ToString());
                }
            }		
         }

        /// <summary>
         /// To Check the data is avail in Queue or Not
         /// </summary>
        private bool IsDataPresent()
        {
            while (true)
            {
                if (DataQ.Count == 0)
                {
                    Monitor.TryEnter(DataQ);
                    try
                    {
                        Monitor.Wait(DataQ, TimeSpan.FromSeconds(10));
                    }
                    catch (ThreadAbortException ex)
                    {
                        //FnoCtclLib.Reference.LogException(ex,"Thread :- " + Thread.CurrentThread.Name + " is Stopped ");
                    }
                    catch (Exception ex)
                    {
                      //  FnoCtclLib.Reference.LogException(ex, "Exception in IsDataPresent().");
                    }
                    finally
                    {
                        Monitor.Exit(DataQ);
                    }
                }
                else
                {
                    return true;
                }
            }
            // return true;
        }
        
        /// <summary>
        /// To Reterive the value from DeQueue
        /// </summary>
        /// 
        public T DeQueue()
        {
            lock (DataQ)
            {
                if (IsDataPresent())
                {
                    try
                    {
                        T data = DataQ.Dequeue();
                        return data;
                    }
                    catch (InvalidOperationException ex)
                    {

                       // FnoCtclLib.Reference.LogException(ex, "InvalidOperationException in DeQueue (MyStringQ) Msg. :- ");
                        return default(T);
                    }
                    catch (Exception ex)
                    {
                       // FnoCtclLib.Reference.LogException(ex,"Exception in DeQueue (MyStringQ).");
                        return default(T);
                    }
                }
                return default(T);
            }
        }
        public int GetSize()
         {
             return DataQ.Count;
         }
        public void Clear()
         {
             lock (DataQ)
             {
                 DataQ.Clear();
             }
         }

         public T DeQueue(bool value)
         {
             lock (DataQ)
             {
                 if (IsDataPresent(true))
                 {
                     try
                     {
                         T data = DataQ.Dequeue();
                         try
                         {
                            // if (iPackets > 0) Interlocked.Decrement(ref iPackets);
                         }
                         catch (ArgumentNullException ex)
                         {
                           //  FnoCtclLib.Reference.LogException(ex,"ArgumentNullException in Decrement the Packet to Send Msg. :- " + ex.Message + " Trace :- " + ex.StackTrace);
                         }
                         catch (Exception ex)
                         {
                            // FnoCtclLib.Reference.LogException(ex,"Exception Decrement the Packet to Send (MyStreamQ) Msg. :- " + ex.Message + " Trace :- " + ex.StackTrace);
                         }
                         return data;
                     }
                     catch (InvalidOperationException ex)
                     {
                         //FnoCtclLib.Reference.LogException(ex,"InvalidOperationException in DeQueue (MyStringQ) Msg. :- " + ex.Message);
                         return default(T);
                     }
                     catch (Exception ex)
                     {
                        // FnoCtclLib.Reference.LogException(ex,"Exception in DeQueue (MyStringQ) Msg. :- " + ex.Message + " Trace :- " + ex.StackTrace);
                         return default(T);
                     }
                 }
                 return default(T);
             }
         }

         private bool IsDataPresent(bool val)
         {
             while (true)
             {
                 if (DataQ.Count == 0)
                 {
                     Monitor.TryEnter(DataQ);
                     try
                     {
                         Monitor.Wait(DataQ, TimeSpan.FromSeconds(10));
                     }
                     catch (ThreadAbortException ex)
                     {
                        // FnoCtclLib.Reference.LogException(ex,"Thread " + Thread.CurrentThread.Name + " is Stopped. Msg. :- " + ex.Message);
                     }
                     catch (Exception ex)
                     {
                        // FnoCtclLib.Reference.LogException(ex,"Exception in IsDataPresent() .Reason:- " + ex.Message + "Trace :" + ex.StackTrace);
                     }
                     finally
                     {
                         Monitor.Exit(DataQ);
                     }
                 }
                 else
                 {
                     return true;
                 }
             }
         }
        
    }
}



