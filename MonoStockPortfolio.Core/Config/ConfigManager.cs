using System.Collections.Generic;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Core.Config;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.Config
{
    public class ConfigManager
    {
        public class Config
        {
            public Config()
            {
                StockDetailItems = new List<StockDataItem>();
            }
            public IList<StockDataItem> StockDetailItems { get; set; }
            public bool AgreedToTermsOfService { get; set; }
        }

        private IConfigRepository _repo;
        private Config _config;

        public ConfigManager() : this(new AndroidSqliteConfigRepository()) {}

        public ConfigManager(IConfigRepository repository)
        {
            _repo = repository;
            _config = _repo.LoadOrCreateConfig();
        }

        public bool AgreedToTos
        {
            get {
                return _config.AgreedToTermsOfService;
            }
            set
            {
                _config.AgreedToTermsOfService = value;
                _repo.SaveConfig(_config);
            }
        }

        public void UpdateStockDetailItems(IList<StockDataItem> newStockDetailItems)
        {
            _config.StockDetailItems = newStockDetailItems;
            _repo.SaveConfig(_config);
        }

        public IEnumerable<StockDataItem> GetStockDetailItems()
        {
            return _config.StockDetailItems;
        }
    }
}
