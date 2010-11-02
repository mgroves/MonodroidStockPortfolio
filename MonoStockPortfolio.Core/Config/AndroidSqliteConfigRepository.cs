using System;

namespace MonoStockPortfolio.Core.Config
{
    public class AndroidSqliteConfigRepository : IConfigRepository
    {
        public ConfigManager.Config LoadOrCreateConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveConfig(ConfigManager.Config config)
        {
            throw new NotImplementedException();
        }
    }
}