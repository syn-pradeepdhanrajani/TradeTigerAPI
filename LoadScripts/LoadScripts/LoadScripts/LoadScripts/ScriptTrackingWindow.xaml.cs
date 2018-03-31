using LoadScripts.Business;
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

namespace LoadScripts
{
    /// <summary>
    /// Interaction logic for ScriptTrackingWindow.xaml
    /// </summary>
    public partial class ScriptTrackingWindow : Window
    {
        List<ScriptTrackingView> scriptTrackingList;

        public ScriptTrackingWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            InitializeUI();
        }

        private void InitializeUI()
        {
            Markets mkts = new Markets();
            scriptTrackingList = mkts.GetScriptTrackingViewData();
            ScriptTrackingGrid.DataContext = scriptTrackingList;
        }

    }
}
