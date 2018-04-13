using LoadScripts.Business;
using LoadScripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoadScripts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Markets mkts = new Markets();
        ScriptMaster scriptMaster = new ScriptMaster();
        public Script SelectedScript { get; set; }
        public int? selectedScriptPriceType { get; set; }
        public string ScriptName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DisplayScriptMaster();
        }

        public void DisplayScriptMaster()
        {
            cbScriptMaster.ItemsSource = scriptMaster.LoadScriptsFromDB();
            cbScriptMaster.DisplayMemberPath = "ScriptName";
            cbScriptMaster.SelectedValuePath = "ScriptId";
        }

        private void LoadPricesClick(object sender, RoutedEventArgs e)
        {          
            mkts.LoadScripts();
        }

        private void LoadPricesFromExcel(object sender, RoutedEventArgs e)
        {
            selectedScriptPriceType = ((System.Windows.Controls.ComboBoxItem)cbScriptPriceType.SelectedItem).Tag.ToString().ToInt32Nullable();
            SelectedScript = cbScriptMaster.SelectedItem as Script;
            mkts.LoadScriptPricesFromExcel(SelectedScript, (ScriptPriceType)selectedScriptPriceType);
        }

        private void UpdatePricesClick(object sender, RoutedEventArgs e)
        {
            mkts.UpdatePrices();
        }

        private void ProcessDataClick(object sender, RoutedEventArgs e)
        {
            selectedScriptPriceType = ((System.Windows.Controls.ComboBoxItem)cbScriptPriceType.SelectedItem).Tag.ToString().ToInt32Nullable();
            mkts.ProcessData((ScriptPriceType)selectedScriptPriceType);
        }

        private void ProcessMarketData(object sender, RoutedEventArgs e)
        {
            //mkts.ProcessMarketData();
            //ScriptTrackingWindow scriptTrackingWindow = new ScriptTrackingWindow();
            //scriptTrackingWindow.Show();


            //Process as jesse livermore principals
            //Loop through all the CSVs and Import it to database
            var CsvData = ImportExcel.LoadCsvFile(string.Format(@"C:\Users\pradeepd\Desktop\Personal_Project\{0}.csv", ((Script)cbScriptMaster.Items[cbScriptMaster.SelectedIndex]).ScriptName));

            //Remove Header
            if (CsvData != null && CsvData.Count > 0)
            {
                CsvData.RemoveAt(0);
                Script scriptItem = null;
                //Loop through the CVS data
                foreach (string csvDataItem in CsvData)
                {
                    string[] csvDataItemArr = csvDataItem.Split(',');
                    //Pass the string array to Markets component for saving it to database
                    scriptItem = mkts.UpdatePrices(((Script)cbScriptMaster.Items[cbScriptMaster.SelectedIndex]).ScriptName, csvDataItemArr);
                }

                //ApplyJesseTradingKey
                if (scriptItem != null)
                    mkts.ApplyJesseTradingKey(scriptItem);
            }


        }
            
        private void LoadMarketDataFromExcel(object sender, RoutedEventArgs e)
        {
            var importData = new List<MarketData>(ImportExcel.Parse<MarketData>(@"C:\Users\pradeepd\Desktop\Personal_Project\MW.xls", "NIFTY"));
            if (importData.Count > 0)
            {
                mkts.LoadMarketDataFromExcel(importData);
            }
            MessageBox.Show("Loading Market Data Done");

        }

        private void cbScripts_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
