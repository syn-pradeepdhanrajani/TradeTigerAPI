using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoadScripts.Business
{
    public class ImportExcel
    {
        public static IEnumerable<K> Parse<K>(string fileName, string workSheetName) where K : class
        {
            IEnumerable<K> list = new List<K>();
            string connectionString = string.Format("provider=Microsoft.Jet.OLEDB.4.0; data source={0};Extended Properties=Excel 8.0;", fileName);

            //get sheet name 

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null || (dt != null  && dt.Rows.Count <= 0))
                {
                    return null;
                }

                workSheetName= dt.Rows[0]["TABLE_NAME"].ToString();
                //String[] excelSheets = new String[dt.Rows.Count];
                //int i = 0;

                //// Add the sheet name to the string array.
                //foreach (DataRow row in dt.Rows)
                //{
                //    excelSheets[i] = row["TABLE_NAME"].ToString();
                //    i++;
                //}

            }


            string query = string.Format("SELECT * FROM [{0}]", workSheetName);

            DataSet data = new DataSet();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                adapter.Fill(data);
                list = PopulateData<K>(data);
            }

            return list;
        }

        private static List<T> PopulateData<T>(DataSet data) where T : class
        {
            List<T> dtos = new List<T>();

            foreach (DataRow row in data.Tables[0].Rows)
            {
                T dto = Activator.CreateInstance<T>();

                PopulateFieldsFromDataRows(row, dto);
                dtos.Add(dto);
            }
            return dtos;
        }

        private static void PopulateFieldsFromDataRows(DataRow row, object o)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.DeclaredOnly;
            foreach (DataColumn col in row.Table.Columns)
            {

                string name = col.ColumnName;
                System.Reflection.FieldInfo field = o.GetType().GetField(name, flags);
                if (field == null)
                {
                    PropertyInfo prop = o.GetType().GetProperty(name);
                    if (prop == null)
                    {
                        prop = ManualMatchingProperty(o, name);
                    }

                    if (prop != null)
                    {
                        if (prop.CanWrite)
                        {
                            if (prop.PropertyType.Equals(typeof(DateTime)))
                            {
                                DateTime d = (DateTime)row[name];
                                if (d.Equals(new DateTime(1900, 1, 1)))
                                {
                                    d = DateTime.MinValue;
                                }

                                prop.SetValue(o, d, null);
                            }
                            else if (prop.PropertyType.Equals(typeof(int)))
                            {
                                prop.SetValue(o, Convert.ToInt32(row[name]), null);
                            }
                            else if (prop.PropertyType.Equals(typeof(decimal)))
                            {
                                prop.SetValue(o, Convert.ToDecimal(row[name]), null);
                            }
                            else
                            {
                                string value = row[name].ToString();
                                prop.SetValue(o, value, null);
                            }
                        }
                    }
                }
                else
                {
                    if (field.FieldType.Equals(typeof(DateTime)))
                    {
                        DateTime d = (DateTime)row[name];
                        if (d.Equals(new DateTime(1900, 1, 1)))
                        {
                            d = DateTime.MinValue;
                        }

                        field.SetValue(o, d);
                    }
                    else if (field.FieldType.Equals(typeof(int)))
                    {
                        field.SetValue(o, Convert.ToInt32(row[name]));
                    }
                    else if (field.FieldType.Equals(typeof(decimal)))
                    {
                        field.SetValue(o, Convert.ToDecimal(row[name]));
                    }
                    else if (field.FieldType.Equals(typeof(bool)))
                    {
                        if (row[name] == DBNull.Value)
                        {
                            field.SetValue(o, false);
                        }
                        else
                        {
                            field.SetValue(o, Convert.ToBoolean(row[name]));
                        }
                    }
                    else
                    {
                        field.SetValue(o, row[name]);
                    }
                }
            }
        }

        private static PropertyInfo ManualMatchingProperty(object o, string name)
        {
            PropertyInfo propInfo = null;
            switch (name)
            {
                case "Scrip Name":
                    propInfo = o.GetType().GetProperty("ScripName");
                    break;
                case "% Change":
                    propInfo = o.GetType().GetProperty("C37Change");
                    break;
                case "Last Traded Qty":
                    propInfo = o.GetType().GetProperty("LastTradedQty");
                    break;
                case "Bid Qty":
                    propInfo = o.GetType().GetProperty("BidQty");
                    break;
                case "Bid Price":
                    propInfo = o.GetType().GetProperty("BidPrice");
                    break;
                case "Offer Price":
                    propInfo = o.GetType().GetProperty("OfferPrice");
                    break;
                case "Offer Qty":
                    propInfo = o.GetType().GetProperty("OfferQty");
                    break;
                case "Last Updated Time":
                    propInfo = o.GetType().GetProperty("LastUpdatedTime");
                    break;
                case "Last Traded Time":
                    propInfo = o.GetType().GetProperty("LastTradedTime");
                    break;
                case "Last Traded Date":
                    propInfo = o.GetType().GetProperty("LastTradedDate");
                    break;
                case "Total Buy Qty":
                    propInfo = o.GetType().GetProperty("TotalBuyQty");
                    break;
                case "Scrip Code":
                    propInfo = o.GetType().GetProperty("ScripCode");
                    break;
                case "Total Sell Qty":
                    propInfo = o.GetType().GetProperty("TotalSellQty");
                    break;
                case "OI Difference":
                    propInfo = o.GetType().GetProperty("OiDifference");
                    break;
                case "OI Difference Percentage":
                    propInfo = o.GetType().GetProperty("OiDifferencePercentage");
                    break;
                case "Company Name":
                    propInfo = o.GetType().GetProperty("CompanyName");
                    break;
                case "P#Open":
                    propInfo = o.GetType().GetProperty("P35Open");
                    break;
                case "P#High":
                    propInfo = o.GetType().GetProperty("P35High");
                    break;
                case "P#Low":
                    propInfo = o.GetType().GetProperty("P35Low");
                    break;
                case "P#Close":
                    propInfo = o.GetType().GetProperty("P35Close");
                    break;
                case "P#Quantity":
                    propInfo = o.GetType().GetProperty("P35Quantity");
                    break;
                case "Pivot Res 3":
                    propInfo = o.GetType().GetProperty("PivotRes3");
                    break;
                case "Pivot Res 2":
                    propInfo = o.GetType().GetProperty("PivotRes2");
                    break;
                case "Pivot Res 1":
                    propInfo = o.GetType().GetProperty("PivotRes1");
                    break;
                case "Pivot Sup 1":
                    propInfo = o.GetType().GetProperty("PivotSup1");
                    break;
                case "Pivot Sup 2":
                    propInfo = o.GetType().GetProperty("PivotSup2");
                    break;
                case "Pivot Sup 3":
                    propInfo = o.GetType().GetProperty("PivotSup3");
                    break;
                default:
                    break;
            }

            return propInfo;
        }

        public static List<string> LoadCsvFile(string filePath)
        {
            var reader = new StreamReader(File.OpenRead(filePath));
            List<string> searchList = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                searchList.Add(line);
            }
            return searchList;
        }
    }
}
