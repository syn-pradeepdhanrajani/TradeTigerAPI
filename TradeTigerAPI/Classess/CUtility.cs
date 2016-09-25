using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;



namespace TradeTigerAPI
{
    static class CUtility
    {


        public static byte[] HostToNetworkDouble(double dNumber)
        {
            byte[] NumberToByte = BitConverter.GetBytes(dNumber);
            Array.Reverse(NumberToByte);
            return NumberToByte;
        }

        public static double NetworkToHostDouble(byte[] bNumber)
        {
            double ByteToNumber;
            Array.Reverse(bNumber);
            ByteToNumber = BitConverter.ToDouble(bNumber, 0);
            return ByteToNumber;
        }

        public static double NetworkToHostLong(byte[] bNumber)
        {
            double ByteToNumber;
            Array.Reverse(bNumber);
            ByteToNumber = (double)BitConverter.ToInt64(bNumber, 0);
            return ByteToNumber;
        }

        public static byte[] HostToNetworkLongold(double dNumber)
        {
            long lNumber = (long)dNumber;
            byte[] NumberToByte = BitConverter.GetBytes(lNumber);
            Array.Reverse(NumberToByte);
            return NumberToByte;
        }

        public static byte[] HostToNetworkLong(double dNumber)
        {
            long lNumber = (long)dNumber * 256 * 256; // Shift the Value to 2 Bytes
            byte[] NumberToByte = BitConverter.GetBytes(lNumber);
            Array.Reverse(NumberToByte);
            return NumberToByte;
        }

        public static byte[] GetHeaderFromStructure(byte[] CompleteStructure)
        {
            byte[] temp = new byte[40];
            for (int ArrayCount = 0; ArrayCount < 40; ArrayCount++)
            {
                temp[ArrayCount] = CompleteStructure[ArrayCount];
            }
            return temp;
        }

        public static byte[] GetLoginHeaderFromStructure(byte[] CompleteStructure)
        {
            byte[] temp = new byte[6];
            for (int ArrayCount = 0; ArrayCount < 6; ArrayCount++)
            {
                temp[ArrayCount] = CompleteStructure[ArrayCount];
            }
            return temp;
        }

        public static byte[] GetByteArrayFromStructure(byte[] CompleteStructure, int StartPosition, int length)
        {
            byte[] temp = new byte[length];
            for (int ArrayCount = 0; ArrayCount < length; ArrayCount++)
            {
                temp[ArrayCount] = CompleteStructure[StartPosition + ArrayCount];
            }
            return temp;
        }

        public static char[] GetPreciseArrayForString(string Str, int ArraySize)
        {         
            string temp = new string(' ', ArraySize);
            temp = Str + temp;
            temp = temp.Substring(0, ArraySize);
            temp = temp.Replace(' ', '\0');
            return temp.ToCharArray();       
        }

        public static byte[] GetBytesFromChar(char[] CharArray)
        {
            Byte[] temp = new byte[CharArray.Length];

            for (int ArrayCount = 0; ArrayCount < CharArray.Length; ArrayCount++)
            {               
                    temp[ArrayCount] = Convert.ToByte(CharArray[ArrayCount]);                
            }
            return temp;
        }

        public static byte[] GetBytesFromNumbers(Int16 Number)
        {
            byte[] temp = BitConverter.GetBytes(Number);
            return temp;
        }

        public static byte[] GetBytesFromNumbers(Int32 Number)
        {
            byte[] temp = BitConverter.GetBytes(Number);
            return temp;
        }

        public static byte[] GetBytesFromNumbers(Int64 Number)
        {
            byte[] temp = BitConverter.GetBytes(Number);
            return temp;
        }

        public static byte[] PutBytesInPosition(byte[] Databyte, int StartPositionFrom0, int ByteArrayLength, byte[] FinalByteStructure)
        {
            int AbsoluteStartPosition = StartPositionFrom0;
            for (int ByteArrayCount = 0; ByteArrayCount < ByteArrayLength; ByteArrayCount++, AbsoluteStartPosition++)
            {
                FinalByteStructure[AbsoluteStartPosition] = Databyte[ByteArrayCount];
            }
            return FinalByteStructure;
        }

      

    
        public static bool CheckPacket(byte[] bMD5CheckSum, byte[] bActualCheckSum)
        {
            bool bSame = false;

            if (bActualCheckSum.Length == bMD5CheckSum.Length)
            {
                int iIdx = 0;
                do
                {
                    if (bActualCheckSum[iIdx] != bMD5CheckSum[iIdx])
                    {
                        bSame = false;
                        break;
                    }
                    else
                        bSame = true;
                    iIdx++;
                }
                while (iIdx < bActualCheckSum.Length);
            }
            else
                bSame = false;

            return bSame;
        }

        public static int ConvertToInt(string sValue)
        {
            int iReturn;
            int.TryParse(sValue, out iReturn);
            return iReturn;
        }

        public static Int32 ConvertToInt32(string sValue)
        {
            Int32 iReturn;
            Int32.TryParse(sValue, out iReturn);
            return iReturn;
        }

        public static Int16 ConvertToInt16(string sValue)
        {
            Int16 iReturn;
            Int16.TryParse(sValue, out iReturn);
            return iReturn;
        }

        public static Int64 ConvertToInt64(string sValue)
        {
            Int64 iReturn;
            Int64.TryParse(sValue, out iReturn);
            return iReturn;
        }
        public static double ConvertToDouble(string sValue)
        {
            double dReturn;
            double.TryParse(sValue, out dReturn);
            return dReturn;
        }

      
        public static string DoubleToString(double value)
        {
            string s = "0"; //int sLen = value.ToString("0").Length;
            try
            {
                long lng = (long)value;
                s = lng.ToString();

             }
            catch (ArgumentException ex)
            {
                //FnoCtclLib.Reference.LogException(ex,"ArgumentException in DoubleToString().");
                s = value.ToString();
            }
            catch (Exception ex)
            {
              //  FnoCtclLib.Reference.LogException(ex,"Exception in DoubleToString().");
                s = value.ToString();
            }
            return s;
        }


    }
}
