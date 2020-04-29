using System;
using System.Collections.Generic;
using System.Text;

namespace hello
{
    public class MonthData
    {
        public string MonthCode { get; set; }
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
        public double HighestPrice { get; set; }
        public double LowestPrice { get; set; }
        public double ValueCostRatio { get; set; }
        public MonthData(string monthCode, double startPrice, double highestPrice, double lowestPrice, double endPrice)
        {
            MonthCode = monthCode;
            StartPrice = startPrice;
            HighestPrice = highestPrice;
            LowestPrice = lowestPrice;
            EndPrice = endPrice;
            ValueCostRatio = -9999;
        }
    }
}
