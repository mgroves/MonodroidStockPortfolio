using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portRepo;
        private readonly IStockDataProvider _stockRepo;

        public PortfolioService(Context context) : this(new AndroidSqlitePortfolioRepository(context),
            new YahooStockDataProvider())
        {
        }

        public PortfolioService(IPortfolioRepository portfolioRepository,
                                IStockDataProvider stockDataProvider)
        {
            _portRepo = portfolioRepository;
            _stockRepo = stockDataProvider;
        }

        public IList<Portfolio> GetAllPortfolios()
        {
            return _portRepo.GetAllPortfolios();
        }

        public IEnumerable<IDictionary<StockDataItem, string>> GetDetailedItems(long portfolioID, IEnumerable<StockDataItem> items)
        {
            IDictionary<StockDataItem, string> dict = new Dictionary<StockDataItem, string>();
            dict.Add(StockDataItem.Ticker, "DENN");
            dict.Add(StockDataItem.LastTradePrice, "4.99");
            dict.Add(StockDataItem.Time, "3:59pm");

            IDictionary<StockDataItem, string> dict2 = new Dictionary<StockDataItem, string>();
            dict2.Add(StockDataItem.Ticker, "XIN");
            dict2.Add(StockDataItem.LastTradePrice, "3.02");
            dict2.Add(StockDataItem.Time, "4:00pm");

            var list = new List<IDictionary<StockDataItem, string>>();
            list.Add(dict);
            list.Add(dict2);
            return list;

//            var positions = _portRepo.GetAllPositions(portfolioID);
//            var tickers = positions.Select(p => p.Ticker);
//            var stockData = _stockRepo.GetStockQuotes(tickers);
//
//            foreach (var position in positions)
//            {
//                var ticker = position.Ticker;
//                var tickerStockData = stockData.Single(stock => stock.Ticker == ticker);
//                var stockItems = GetStockItems(items, tickerStockData);
//                var remainingItemsToGet = items.Except(stockItems.Keys);
//                stockItems.AddRange(CalculateItems(remainingItemsToGet, position, tickerStockData));
//                
//                yield return stockItems;
//            }
        }

        public Portfolio GetPortolioById(long portfolioId)
        {
            return _portRepo.GetPortfolioById(portfolioId);
        }

        public IEnumerable<IDictionary<StockDataItem, string>> GetDetailedItems(Portfolio portfolio, IEnumerable<StockDataItem> items)
        {
//            var tickers = portfolio.Positions.Select(p => p.Ticker);
//            var stockData = _stockRepo.GetStockQuotes(tickers);
//
//            foreach (var position in portfolio.Positions)
//            {
//                var ticker = position.Ticker;
//                var tickerStockData = stockData.Single(stock => stock.Ticker == ticker);
//                var stockItems = GetStockItems(items, tickerStockData);
//                var remainingItemsToGet = items.Except(stockItems.Keys);
//                stockItems.AddRange(CalculateItems(remainingItemsToGet, position, tickerStockData));
//
//                yield return stockItems;
//            }
            throw new NotImplementedException();
        }

        private IDictionary<StockDataItem, string> GetStockItems(IEnumerable<StockDataItem> items, StockQuote quote)
        {
            var dict = new Dictionary<StockDataItem, string>();
            foreach (var item in items)
            {
                switch (item)
                {
                    case StockDataItem.Change:
                        dict.Add(item, quote.Change.ToString());
                        break;
                    case StockDataItem.Ticker:
                        dict.Add(item, quote.Ticker);
                        break;
                    case StockDataItem.Time:
                        dict.Add(item, quote.LastTradeTime);
                        break;
                    case StockDataItem.Volume:
                        dict.Add(item, quote.Volume);
                        break;
                    case StockDataItem.LastTradePrice:
                        dict.Add(item, quote.LastTradePrice.ToString());
                        break;
                    case StockDataItem.RealTimeLastTradeWithTime:
                        dict.Add(item, quote.RealTimeLastTradePrice.ToString());
                        break;
                    case StockDataItem.ChangeRealTime:
                        dict.Add(item, quote.ChangeRealTime);
                        break;
                }
            }
            return dict;
        }

        private IDictionary<StockDataItem,string> CalculateItems(IEnumerable<StockDataItem> items, Position position, StockQuote quote)
        {
            var dict = new Dictionary<StockDataItem, string>();
            foreach (var item in items)
            {
                switch (item)
                {
                    case StockDataItem.GainLoss:
                        dict.Add(item, CalculateGainLoss(quote, position).ToString());
                        break;
                    case StockDataItem.GainLossRealTime:
                        dict.Add(item, CalculateGainLossRealTime(quote, position).ToString());
                        break;
                    default:
                        throw new ArgumentException("That StockDataItem type cannot be calculated");
                }
            }
            return dict;
        }

        private static decimal CalculateGainLossRealTime(StockQuote quote, Position position)
        {
            var moneyISpent = position.PricePerShare * position.Shares;
            var moneyItsWorth = position.Shares * quote.RealTimeLastTradePrice;
            return moneyItsWorth - moneyISpent;
        }

        private static decimal CalculateGainLoss(StockQuote quote, Position position)
        {
            var moneyISpent = position.PricePerShare*position.Shares;
            var moneyItsWorth = position.Shares*quote.LastTradePrice;
            return moneyItsWorth - moneyISpent;
        }
    }
}