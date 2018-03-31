using System;

[DisplayTable("nifty")]
public class Nifty
{
    [DisplayColumn("Exchange", 0)]
    public System.String Exchange { get; set; }
    [DisplayColumn("Scrip Name", 1)]
    public System.String ScripName { get; set; }
    [DisplayColumn("% Change", 2)]
    public System.String %Change { get; set; }
    [DisplayColumn("Current", 3)]
    public System.String Current { get; set; }
    [DisplayColumn("Bid Qty", 4)]
    public System.String BidQty { get; set; }
    [DisplayColumn("Bid Price", 5)]
    public System.String BidPrice { get; set; }
    [DisplayColumn("Offer Price", 6)]
    public System.String OfferPrice { get; set; }
    [DisplayColumn("Offer Qty", 7)]
    public System.String OfferQty { get; set; }
    [DisplayColumn("Open", 8)]
    public System.String Open { get; set; }
    [DisplayColumn("High", 9)]
    public System.String High { get; set; }
    [DisplayColumn("Low", 10)]
    public System.String Low { get; set; }
    [DisplayColumn("Close", 11)]
    public System.String Close { get; set; }
    [DisplayColumn("Scrip Code", 12)]
    public System.String ScripCode { get; set; }
    [DisplayColumn("Last Updated Time", 13)]
    public System.String LastUpdatedTime { get; set; }
    [DisplayColumn("Last Traded Time", 14)]
    public System.String LastTradedTime { get; set; }
    [DisplayColumn("Last Traded Date", 15)]
    public System.String LastTradedDate { get; set; }
    [DisplayColumn("Qty", 16)]
    public System.String Qty { get; set; }
    [DisplayColumn("Total Buy Qty", 17)]
    public System.String TotalBuyQty { get; set; }
    [DisplayColumn("Total Sell Qty", 18)]
    public System.String TotalSellQty { get; set; }
    [DisplayColumn("OI Difference", 19)]
    public System.String OiDifference { get; set; }
    [DisplayColumn("OI Difference Percentage", 20)]
    public System.String OiDifferencePercentage { get; set; }
    [DisplayColumn("Company Name", 21)]
    public System.String CompanyName { get; set; }
    [DisplayColumn("P.Open", 22)]
    public System.String P.Open { get; set; }
    [DisplayColumn("P.High", 23)]
    public System.String P.High { get; set; }
    [DisplayColumn("P.Low", 24)]
    public System.String P.Low { get; set; }
    [DisplayColumn("P.Close", 25)]
    public System.String P.Close { get; set; }
    [DisplayColumn("P.Quantity", 26)]
    public System.String P.Quantity { get; set; }
    [DisplayColumn("Pivot Res 3", 27)]
    public System.String PivotRes3 { get; set; }
    [DisplayColumn("Pivot Res 2", 28)]
    public System.String PivotRes2 { get; set; }
    [DisplayColumn("Pivot Res 1", 29)]
    public System.String PivotRes1 { get; set; }
    [DisplayColumn("Pivot", 30)]
    public System.String Pivot { get; set; }
    [DisplayColumn("Pivot Sup 1", 31)]
    public System.String PivotSup1 { get; set; }
    [DisplayColumn("Pivot Sup 2", 32)]
    public System.String PivotSup2 { get; set; }
    [DisplayColumn("Pivot Sup 3", 33)]
    public System.String PivotSup3 { get; set; }
}
