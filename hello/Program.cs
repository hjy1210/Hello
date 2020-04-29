using System;

namespace hello
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //MyTest.SayHello();
            //testOneToNine();
            testMyScraper();
        }
        static void testOneToNine()
        {
            OneToNine obj = new OneToNine();
            obj.Solve();
        }
        static void testMyScraper()
        {
            string[] codes = new string[] {"1101","1104","1301","1326","1434","1504","1802","2002","2308",
                "2357","2330","2412","2454","2498","2501","2892","2911","4938","8131"};

            MyScraper.SaveMonthData(codes);
            MyScraper.SaveStockDividendPolicy(codes);
            MyScraper.ComputeProfit(codes);
        }
    }
}
