namespace MonoStockPortfolio.Activites.EditPositionScreen
{
    public interface IEditPositionPresenter
    {
        void Initialize(IEditPositionView editPositionActivity, long portfolioId, long? positionId = null);
        void Save(PositionInputModel getPositionInputModel);
    }
}