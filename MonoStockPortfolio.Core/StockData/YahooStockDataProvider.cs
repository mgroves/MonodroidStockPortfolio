using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Android.Util;
using FileHelpers;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.StockData
{
    public class YahooStockDataProvider : IStockDataProvider
    {
        private const string LAST_TRADE_PRICE_ONLY = "l1";
        private const string NAME = "n";
        private const string VOLUME = "v";
        private const string TICKER_SYMBOL = "s";
        private const string CHANGE = "c1";
        private const string LAST_TRADE_TIME = "t1";
        private const string REAL_TIME_LAST_TRADE_WITH_TIME = "k1";
        private const string REAL_TIME_CHANGE = "c6";

        // http://www.gummy-stuff.org/Yahoo-data.htm
        // http://finance.yahoo.com/d/quotes.csv?s= a BUNCH of 
        // STOCK SYMBOLS separated by "+" &f=a bunch of special tags
        public IEnumerable<StockQuote> GetStockQuotes(IEnumerable<string> tickers)
        {
            string url = "http://finance.yahoo.com/d/quotes.csv?s=";
            url += string.Join("+", tickers.ToArray());
            url += "&f=";
            url += TICKER_SYMBOL;
            url += LAST_TRADE_PRICE_ONLY;
            url += NAME;
            url += VOLUME;
            url += CHANGE;
            url += LAST_TRADE_TIME;
            url += REAL_TIME_LAST_TRADE_WITH_TIME;
            url += REAL_TIME_CHANGE;

            string resultCsv = ScrapeUrl(url);

            var yahooQuoteData = ParseCsvIntoStockQuotes(resultCsv);

            return yahooQuoteData.Select(MapYahooData);
        }

        private static StockQuote MapYahooData(YahooFinanceStockData data)
        {
            if (data == null)
            {
                return null;
            }
            var stock = new StockQuote();
            stock.Name = data.Name;
            stock.LastTradePrice = data.LastTradePrice;
            stock.Ticker = data.Ticker;
            stock.Volume = data.Volume;
            stock.Change = data.Change;
            stock.LastTradeTime = data.LastTradeTime;
            stock.RealTimeLastTradePrice = decimal.Parse(data.RealTimeLastTradeWithTime
                .Replace("<b>", "")
                .Replace("</b>", "")
                .Replace("N/A -", "")
                .Trim()
            );
            stock.ChangeRealTime = data.ChangeRealTime;
            return stock;
        }

        private static IList<YahooFinanceStockData> ParseCsvIntoStockQuotes(string csv)
        {
            var engine = new FileHelperEngine(typeof(YahooFinanceStockData));
            var stockQuotes = engine.ReadString(csv) as YahooFinanceStockData[];
            stockQuotes.ToList().ForEach(q => Log.Error("ParseCSV",q.Volume));
            if (stockQuotes == null)
            {
                throw new ArgumentException("Could not parse CSV input");
            }
            return stockQuotes;
        }

        private static string ScrapeUrl(string url)
        {
            try
            {
                string resultCsv;
                var req = WebRequest.Create(url);
                var resp = req.GetResponse();
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    resultCsv = sr.ReadToEnd();
                    sr.Close();
                }
                return resultCsv;
            }
            catch (Exception ex)
            {
                Log.Error("ScrapeUrlException", ex.ToString());
                throw;
            }
        }
    }
}