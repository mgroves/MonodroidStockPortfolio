using System;
using System.Linq;
using Android.Runtime;
using Android.Util;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.EditPositionScreen
{
    [Preserve(AllMembers = true)]
    public class EditPositionPresenter : IEditPositionPresenter
    {
        private IEditPositionView _currentView;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockDataProvider _stockService;
        private long? _positionId;
        private long _portfolioId;

        public EditPositionPresenter(IPortfolioRepository portfolioRepository, IStockDataProvider stockService)
        {
            _portfolioRepository = portfolioRepository;
            _stockService = stockService;
        }

        public void Initialize(IEditPositionView editPositionView, long portfolioId, long? positionId = null)
        {
            _currentView = editPositionView;
            _positionId = positionId;
            _portfolioId = portfolioId;

            if (positionId != null)
            {
                _currentView.SetTitle("Edit Position");
                var position = _portfolioRepository.GetPositionById(positionId ?? -1);
                _currentView.PopulateForm(position);
            }
            else
            {
                _currentView.SetTitle("Add Position");
            }
        }

        public void Save(PositionInputModel positionInputModel)
        {
            var validator = new FormValidator();
            validator.AddRequired(() => positionInputModel.TickerText, "Please enter a ticker");
            validator.AddValidPositiveDecimal(() => positionInputModel.SharesText, "Please enter a valid, positive number of shares");
            validator.AddValidPositiveDecimal(() => positionInputModel.PriceText, "Please enter a valid, positive price per share");
            validator.AddValidation(() => ValidateTicker(positionInputModel.TickerText));
            validator.AddValidation(() => ValidateNotRepeatTicker(positionInputModel.TickerText));

            var errorMessages = validator.Apply();
            if (!errorMessages.Any())
            {
                _portfolioRepository.SavePosition(GetPosition(positionInputModel));
                _currentView.GoBackToPortfolioActivity();
            }
            else
            {
                _currentView.ShowErrorMessages(errorMessages.ToList());
            }
        }

        private Position GetPosition(PositionInputModel positionInputModel)
        {
            Position positionToSave;
            if (_positionId != null)
            {
                positionToSave = new Position(_positionId ?? -1);
            }
            else
            {
                positionToSave = new Position();
            }

            positionToSave.Shares = decimal.Parse(positionInputModel.SharesText);
            positionToSave.PricePerShare = decimal.Parse(positionInputModel.PriceText);
            positionToSave.Ticker = positionInputModel.TickerText.ToUpper();
            positionToSave.ContainingPortfolioID = _portfolioId;
            return positionToSave;
        }

        private string ValidateTicker(string ticker)
        {
            try
            {
                if (_stockService.IsValidTicker(ticker))
                {
                    return string.Empty;
                }
                return "Invalid Ticker Name";
            }
            catch (Exception ex)
            {
                return "Unable to load stock information from the web";
            }
        }

        private string ValidateNotRepeatTicker(string ticker)
        {
            if(_portfolioRepository.IsTickerAlreadyBeingTracked(ticker, _portfolioId))
            {
                return "You are already tracking that ticker in this portfolio";
            }
            return string.Empty;
        }
    }
}