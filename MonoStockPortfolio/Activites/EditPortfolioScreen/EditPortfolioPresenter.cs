using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.EditPortfolioScreen
{
    [Preserve(AllMembers = true)]
    public class EditPortfolioPresenter : IEditPortfolioPresenter
    {
        private IEditPortfolioView _currentView;
        private long? _portfolioId;
        private readonly IPortfolioRepository _porfolioRepository;

        public EditPortfolioPresenter(IPortfolioRepository portfolioRepository)
        {
            _porfolioRepository = portfolioRepository;
        }

        public void Initialize(IEditPortfolioView editPortfolioView, long? portfolioId = null)
        {
            _portfolioId = portfolioId;
            _currentView = editPortfolioView;

            SetTitle();

            PrepopulateForm();
        }

        public void SavePortfolio(Portfolio portfolioToSave)
        {
            var errors = Validate((portfolioToSave));
            if (!errors.Any())
            {
                _porfolioRepository.SavePortfolio(portfolioToSave);

                _currentView.ShowSaveSuccessMessage("You saved: " + portfolioToSave.Name);

                _currentView.GoBackToMainActivity();
            }
            else
            {
                _currentView.ShowValidationErrors(errors);
            }
        }

        private IEnumerable<string> Validate(Portfolio portfolioToSave)
        {
            var validator = new FormValidator();
            validator.AddRequired(() => portfolioToSave.Name, "Please enter a portfolio name");
            validator.AddValidation(() => IsDuplicateName(portfolioToSave));

            return validator.Apply().ToList();
        }

        private string IsDuplicateName(Portfolio portfolioToSave)
        {
            var portfolio = _porfolioRepository.GetPortfolioByName(portfolioToSave.Name);
            if (portfolio != null && portfolio.ID != portfolioToSave.ID)
            {
                return "Portfolio name is already taken";
            }
            return string.Empty;
        }

        private void PrepopulateForm()
        {
            if (_portfolioId != null)
            {
                var portfolio = _porfolioRepository.GetPortfolioById(_portfolioId ?? -1);
                _currentView.PopulateForm(portfolio);
            }
        }

        private void SetTitle()
        {
            _currentView.SetTitle(_portfolioId == null ? "Add New Portfolio" : "Edit Portfolio");
        }
    }
}