using System;
using System.Collections.Generic;
using System.Text;

namespace hello
{
    public class DividendData
    {
        public string Year { get; set; }
        public double CashDividend { get; set; }
        public double ShareDividend { get; set; }
        public DividendData(string year, double cashDividend, double shareDividend)
        {
            Year = year;
            CashDividend = cashDividend;
            ShareDividend = shareDividend;
        }
    }
}
