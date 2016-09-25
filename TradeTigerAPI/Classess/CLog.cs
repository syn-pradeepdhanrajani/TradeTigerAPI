using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
namespace TradeTigerAPI
{
    public  class LogFile
    {
        public static LogFile file = new LogFile();

        public static LogFile Reference
        {
            get { return file; }
        }

        private static string FILE_NAME = DateTime.Now.ToString("ddMMyyyy") + "LogTextFile.txt";
        private static string ConfigFilePath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + @"\Logs\"  + FILE_NAME; }
        }
        public void WriteLogFile(string methodName, string message)
        {
            //String FolderPath = Environment.ExpandEnvironmentVariables("C:\\User\\LogFile.txt");
            FileStream fs = null;
            if (!File.Exists(ConfigFilePath))
            {
                using (fs = File.Create(ConfigFilePath))
                {
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    using (FileStream file = new FileStream(ConfigFilePath, FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(file);
                        streamWriter.WriteLine("----------------------------------------------------------------------------------------------------------------");                      
                        streamWriter.WriteLine((((System.DateTime.Now + " - ") + " - ") + methodName + " - ") + message + "\r");                      
                        streamWriter.Close();
                    }
                }
            }
            catch
            {
            }
        }
    }
}
