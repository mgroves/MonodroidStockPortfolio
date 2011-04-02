using System.Collections.Generic;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Activites.EditPositionScreen
{
    public interface IEditPositionView
    {
        void SetTitle(string title);
        void PopulateForm(Position position);
        void GoBackToPortfolioActivity();
        void ShowErrorMessages(IList<string> errorMessages);
    }
}