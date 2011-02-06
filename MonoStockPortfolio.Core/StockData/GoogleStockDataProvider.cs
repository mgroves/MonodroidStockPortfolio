using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.StockData
{
    public class GoogleStockDataProvider : IStockDataProvider
    {
        private const string BASE_URL = "http://www.google.com/finance/info?infotype=infoquoteall&q=";
        /*
        http://code.google.com/p/qsb-mac-plugins/source/browse/trunk/stock-quoter/trunk/StockQuoter.py?r=4
         The Google Finance feed can return some or all of the following keys:

  avvo    * Average volume (float with multiplier, like '3.54M')
  beta    * Beta (float)
  c       * Amount of change while open (float)
  ccol    * (unknown) (chars)
  cl        Last perc. change
  cp      * Change perc. while open (float)
  e       * Exchange (text, like 'NASDAQ')
  ec      * After hours last change from close (float)
  eccol   * (unknown) (chars)
  ecp     * After hours last chage perc. from close (float)
  el      * After. hours last quote (float)
  el_cur  * (unknown) (float)
  elt       After hours last quote time (unknown)
  eo      * Exchange Open (0 or 1)
  eps     * Earnings per share (float)
  fwpe      Forward PE ratio (float)
  hi      * Price high (float)
  hi52    * 52 weeks high (float)
  id      * Company id (identifying number)
  l       * Last value while open (float)
  l_cur   * Last value at close (like 'l')
  lo      * Price low (float)
  lo52    * 52 weeks low (float)
  lt        Last value date/time
  ltt       Last trade time (Same as "lt" without the data)
  mc      * Market cap. (float with multiplier, like '123.45B')
  name    * Company name (text)
  op      * Open price (float)
  pe      * PE ratio (float)
  t       * Ticker (text)
  type    * Type (i.e. 'Company')
  vo      * Volume (float with multiplier, like '3.54M')
         */

        public IEnumerable<StockQuote> GetStockQuotes(IEnumerable<string> tickers)
        {
            var tickerCsv = string.Join(",", tickers.ToArray());
            var url = BASE_URL + tickerCsv;
            var jsonResults = ScrapeUrl(url).Split('}');

            return jsonResults.Select(MapJsonToStockitems);
        }

        protected StockQuote MapJsonToStockitems(string jsonResults)
        {
            using(var sr = new StringReader(jsonResults))
            {
                var sq = new StockQuote();
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    if(line.StartsWith(",\"t\""))
                    {
                        sq.Ticker = line.Replace(",\"t\" : ", "").Trim().Trim('"');
                        continue;
                    }
                    if(line.StartsWith(",\"c\""))
                    {
                        sq.Change = decimal.Parse(line.Replace(",\"c\" : ", "").Trim().Trim('"'));
                        continue;
                    }
                    if(line.StartsWith(",\"l\""))
                    {
                        sq.LastTradePrice = decimal.Parse(line.Replace(",\"l\" : ", "").Trim().Trim('"'));
                        continue;
                    }
                    if(line.StartsWith(",\"ltt\""))
                    {
                        sq.LastTradeTime = line.Replace(",\"ltt\":", "").Trim().Trim('"').Replace("EST","").Trim();
                        continue;
                    }
                }
                return sq;
            }
        }

        private static string ScrapeUrl(string url)
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
    }
}