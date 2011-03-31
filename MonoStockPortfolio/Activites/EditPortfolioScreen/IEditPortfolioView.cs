using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.EditPortfolioScreen
{
    public interface IEditPortfolioView
    {
        void SetTitle(string title);
        void PopulateForm(Portfolio portfolio);
        void ShowSaveSuccessMessage(string message);
        void GoBackToMainActivity();
        void ShowValidationErrors(IEnumerable<string> errors);
    }
}