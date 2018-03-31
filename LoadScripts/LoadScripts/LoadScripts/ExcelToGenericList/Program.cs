using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;

namespace ExcelToGenericList
{
    class Program
    {               
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                                                
            Console.WriteLine("Starting Retrieving data from Excel...");
            Console.WriteLine("");
                        
            var productString = GetAllProductString();
            Console.WriteLine(productString);
           
            Console.WriteLine("");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static string GetAllProductString()
        {
            //Check file path
            string excelPath = ExcelReader.CheckPath(ConfigurationManager.AppSettings["ExcelFilePath"]);
            
            //Get product list from the spreadsheet
            IList<Product> dataList = ExcelReader.GetDataToList(excelPath, AddProductData);
                
            //Return resulted product list as formated string. 
            return dataList.ToString<Product>();            
        }       

        //Function for mapping and entering data into Product object.
        private static Product AddProductData(IList<string> rowData, IList<string> columnNames)
        {
            var product = new Product()
            {
                ProductID = rowData[columnNames.IndexFor("ProductID")].ToInt32(),
                ProductName = rowData[columnNames.IndexFor("ProductName")],
                CategoryID = rowData[columnNames.IndexFor("CategoryID")].ToInt32Nullable(),
                UnitPrice = rowData[columnNames.IndexFor("UnitPrice")].ToDecimalNullable(),
                OutOfStock = rowData[columnNames.IndexFor("OutOfStock")].ToBoolean(),
                StockDate = rowData[columnNames.IndexFor("StockDate")].ToDateTimeNullable()                
            };            
            return product;
        }

        //Merge files specified as resources into one output executable or dll
        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            var obj = new Object();
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(obj.GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }
    }
}
