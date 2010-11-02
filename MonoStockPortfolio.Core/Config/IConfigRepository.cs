namespace MonoStockPortfolio.Core.Config
{
    public interface IConfigRepository
    {
        ConfigManager.Config LoadOrCreateConfig();
        void SaveConfig(ConfigManager.Config config);
    }
}