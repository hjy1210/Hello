using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace hello
{
    public class MyScraper
    {
        public static void SaveStockDividendPolicy(string[] codes)
        {
            foreach (var code in codes)
                StockDividendPolicy(code);
        }
        public static void SaveMonthData(string[] codes)
        {
            foreach (var code in codes)
                MonthGraph(code);
        }

        public static void ComputeProfit(string[] codes)
        {
            for (int i = 0; i < codes.Length; i++)
            {
                Dictionary<string, DividendData> dicDividendData = GetDividendData("股利-" + codes[i] + ".csv");
                List<MonthData> monthDatas = GetMonthData("月線-" + codes[i] + ".csv");
                monthDatas = (from md in monthDatas orderby md.MonthCode select md).ToList();
                UpdateMonthData(monthDatas, dicDividendData);
                monthDatas = (from md in monthDatas orderby md.MonthCode descending select md).ToList();
                int monthCount = 0;
                double multipiler = 1;
                for (int j = 1; j <= 120 && j < monthDatas.Count; j++)
                {
                    if (!IsMissing(monthDatas[j].ValueCostRatio))
                    {
                        monthCount++;
                        multipiler *= monthDatas[j].ValueCostRatio;
                    }
                    else
                    {
                        break;
                    }
                }
                double profit = 1200 * (Math.Exp(Math.Log(multipiler) / monthCount) - 1);
                Console.WriteLine($"{codes[i]} : {monthCount} 個月幾何平均年息 {profit}%");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static void StockDividendPolicy(string code)
        {
            ScrapingBrowser browser = new ScrapingBrowser();

            //set UseDefaultCookiesParser as false if a website returns invalid cookies format
            //browser.UseDefaultCookiesParser = false;
            string addr = "https://goodinfo.tw/StockInfo/StockDividendPolicy.asp?STOCK_ID=" + code;
            WebPage homePage = browser.NavigateToPage(new Uri(addr));

            //PageWebForm form = homePage.FindFormById("sb_form");
            //form["q"] = "scrapysharp";
            //form.Method = HttpVerb.Get;
            //WebPage resultsPage = form.Submit();

            HtmlNode[] trs = homePage.Html.CssSelect("div#divDetail tr").ToArray(); // div#divDetail tbody tr 不行
            StringBuilder sb = new StringBuilder();
            string header = "股利年度,現金盈餘,現金公積,現金合計,股票盈餘,股票公積,股票合計,股利合計,現金億元,股票千張,填息日數,填權日數," +
                "股價年度,最高,最低,年均,現金殖利率,股票殖利率,殖利率,股利所屬年度," +
                "EPS元,配息率,配股率,配利率";
            sb.AppendLine(header);
            foreach (var tr in trs)
            {
                var tds = tr.SelectNodes("td");
                if (tds.Count != 24 || !Char.IsDigit(tds[0].InnerText[0]))
                    continue;
                string data = "";
                for (int i = 0; i < tds.Count; i++)
                {
                    data += $"{tds[i].InnerText}";
                    data += i < tds.Count - 1 ? "," : "";
                }
                sb.AppendLine(data);
            }
            //File.WriteAllText("股利-"+code+".csv", sb.ToString());
            using (StreamWriter sw = new StreamWriter("股利-" + code + ".csv", false, Encoding.UTF8))
            {
                sw.Write(sb.ToString());
            }
            Console.WriteLine("股利-" + code + ".csv saved");
        }
        static void MonthGraph(string code)
        {
            ScrapingBrowser browser = new ScrapingBrowser();

            //set UseDefaultCookiesParser as false if a website returns invalid cookies format
            //browser.UseDefaultCookiesParser = false;
            string addr = "https://goodinfo.tw/StockInfo/ShowK_Chart.asp?STOCK_ID=" + code + "&CHT_CAT2=MONTH";
            WebPage homePage = browser.NavigateToPage(new Uri(addr));

            HtmlNode[] trs = homePage.Html.CssSelect("div#divPriceDetail tr").ToArray(); // div#divDetail tbody tr 不行
            StringBuilder sb = new StringBuilder();
            string header = "交易月份,交易日數,開盤,最高,最低,收盤,漲跌,漲跌%,震幅%,成交千張,日均千張,成交億元," +
                "日均億元,外資,投信,自營,合計,外資持股%,融資增減,融資餘額," +
                "融卷增減,融卷餘額,卷資比%";
            sb.AppendLine(header);
            foreach (var tr in trs)
            {
                var tds = tr.SelectNodes("td");
                if (tds.Count != 23 || !Char.IsDigit(tds[0].InnerText[0]))
                    continue;
                string data = "";
                for (int i = 0; i < tds.Count; i++)
                {
                    data += $"{tds[i].InnerText}";
                    data += i < tds.Count - 1 ? "," : "";
                }
                sb.AppendLine(data);
            }
            using (StreamWriter sw = new StreamWriter("月線-" + code + ".csv", false, Encoding.UTF8))
            {
                sw.Write(sb.ToString());
            }
            Console.WriteLine("月線-" + code + ".csv saved");
        }
        static List<MonthData> GetMonthData(string path)
        {
            List<MonthData> monthDatas = new List<MonthData>();
            string[] lines = new string[] { };
            using (StreamReader sr = new StreamReader(path, true))
            {
                lines = sr.ReadToEnd().Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string MonthCode = tokens[0];
                double StartPrice = DoubleParse(tokens[2]);
                double HighestPrice = DoubleParse(tokens[3]);
                double LowestPrice = DoubleParse(tokens[4]);
                double EndPrice = DoubleParse(tokens[5]);
                monthDatas.Add(new MonthData(MonthCode, StartPrice, HighestPrice, LowestPrice, EndPrice));
            }
            return monthDatas;
        }
        static Dictionary<string, DividendData> GetDividendData(string path)
        {
            Dictionary<string, DividendData> dicDividendData = new Dictionary<string, DividendData>();
            string[] lines = new string[] { };
            using (StreamReader sr = new StreamReader(path, true))
            {
                lines = sr.ReadToEnd().Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string Year = tokens[0];
                double CashDividend = DoubleParse(tokens[3]);
                double ShareDividend = DoubleParse(tokens[6]);
                dicDividendData[Year] = new DividendData(Year, CashDividend, ShareDividend);
            }
            return dicDividendData;
        }
        static void UpdateMonthData(List<MonthData> monthDatas, Dictionary<string, DividendData> dicDividend)
        {
            for (int i = 0; i < monthDatas.Count - 1; i++)
            {
                string monthCode = monthDatas[i].MonthCode;
                string month = monthCode.Substring(3);
                if (month != "08") // 假設9月第一個交易日將現金股息以當日開盤價買入
                {
                    monthDatas[i].ValueCostRatio = monthDatas[i + 1].StartPrice / monthDatas[i].StartPrice;
                }
                else
                {
                    int year = int.Parse(monthCode.Substring(0, 2));
                    year += year < 50 ? 2000 : 1900;
                    if (dicDividend.Keys.Contains(year.ToString()))
                    {
                        DividendData dividendData = dicDividend[year.ToString()];
                        double newShare = dividendData.ShareDividend / 10; //配股股數
                        double newCash = dividendData.CashDividend / monthDatas[i + 1].StartPrice; //配息買得股數
                        monthDatas[i].ValueCostRatio = (monthDatas[i + 1].StartPrice / monthDatas[i].StartPrice) * (1 + newShare + newCash);
                    }
                }
            }
        }
        static bool IsMissing(double v)
        {
            return v == -9999;
        }
        static double DoubleParse(string token)
        {
            try
            {
                return double.Parse(token);
            }
            catch (Exception)
            {
                return -9999;
            }
        }

    }
}
