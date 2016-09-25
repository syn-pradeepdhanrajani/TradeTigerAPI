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
using System.Windows.Shapes;
using TradeTigerAPI.Business;
using System.Collections.ObjectModel;

namespace TradeTigerAPI
{
    /// <summary>
    /// Interaction logic for StockScriptsTracking.xaml
    /// </summary>
    public partial class StockScriptsTracking : Window
    {
        Stocks stockInstance;
        

        public StockScriptsTracking()
        {
            InitializeComponent();
        }

        public void InitializeUI(Stocks stks)
        {
            stockInstance = stks;
        }

        public void RefreshData()
        {
            InBuyRadarLabel.Content = "In Buy Radar                                         Last Updated :" + DateTime.Now;
            if (stockInstance != null && this.InBuyRadarGrid.DataContext == null)
            {

                //var scriptsInBuyRadarList = stockInstance.scriptsInBuyRadar.Select(d => d.Value).ToList();
                this.InBuyRadarGrid.DataContext = stockInstance.ScriptsInBuyRadarCollection;

                //var scriptsInShortRadarList = stockInstance.scriptsInShortRadar.Select(d => d.Value).ToList();
                this.InShortRadarGrid.DataContext = stockInstance.ScriptsInShortRadarCollection;

               // var scriptsBuyList = stockInstance.scriptsBuy.Select(d => d.Value).ToList();
                this.BuyGrid.DataContext = stockInstance.ScriptsBuyCollection;

                //var scriptsShortList = stockInstance.scriptsShort.Select(d => d.Value).ToList();
                this.ShortGrid.DataContext = stockInstance.ScriptsShortCollection;
                                
            }
        }
    }
}
