using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TradeTigerAPI
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        public OrderControl()
        {
            InitializeComponent();
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            CStructures.MessageHeader header = new CStructures.MessageHeader(1);
            header.Prop01MessageLength = 357;
            header.Prop02TransactionCode = 11;

            CStructures.OrderItem orderItem = new CStructures.OrderItem();
            orderItem.Prop01DataLength = 227;
            orderItem.Prop02OrderID = "1";
            orderItem.Prop03CustomerID = CConstants.LoginId;
            orderItem.Prop04S2KID = "1";
            orderItem.Prop05ScripToken = "1491";
            orderItem.Prop06BuySell = "B";
            orderItem.Prop07OrderQty = 1;
            orderItem.Prop08OrderPrice = 1;
            orderItem.Prop09TriggerPrice =1;
            orderItem.Prop10DisclosedQty = 1;
            orderItem.Prop11ExecutedQty = 1;
            orderItem.Prop12RMSCode = "";
            orderItem.Prop13ExecutedPrice = 1;
            orderItem.Prop14AfterHour = "N";
            orderItem.Prop15GTDFlag = "GFD";
            orderItem.Prop16GTD = "";
            orderItem.Prop17Reserved = "";

            CStructures.OrderRequest orderRequest = new CStructures.OrderRequest(true);
            orderRequest.Prop01Header = header;
            orderRequest.Prop02RequestID = "1";
            orderRequest.Prop03OrderCount = 1;
            orderRequest.Prop04ExchangeCode = "NC";
            orderRequest.Prop05OrderType1 = "NEW";
            List<CStructures.OrderItem> itemList = new List<CStructures.OrderItem>();
            itemList.Add(orderItem);
            orderRequest.Prop06OrderItems = itemList;
            orderRequest.Prop07Reserved = "";


        }
    }
}
