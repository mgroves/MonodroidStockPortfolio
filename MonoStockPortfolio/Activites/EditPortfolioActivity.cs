using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Add Portfolio", MainLauncher = false)]
    public partial class EditPortfolioActivity : Activity
    {
        [IoC] private IPortfolioRepository _repo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.addportfolio);

            WireUpEvents();

            var portfolioId = Intent.GetLongExtra(Extra_PortfolioID, -1);
            if(portfolioId != -1)
            {
                this.Title = "Edit Portfolio";
                PopulateForm(portfolioId);
            }
        }

        private void PopulateForm(long portfolioId)
        {
            var portfolio = _repo.GetPortfolioById(portfolioId);
            PortfolioName.Text = portfolio.Name;
        }

        private void WireUpEvents()
        {
            SaveButton.Click += saveButton_Click;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Portfolio portfolioToSave = GetPortfolioToSave();

            if (Validate(portfolioToSave))
            {
                _repo.SavePortfolio(portfolioToSave);

                Toast.MakeText(this, "You saved: " + PortfolioName.Text, ToastLength.Short).Show();

                var intent = new Intent();
                SetResult(Result.Ok, intent);
                Finish();
            }
        }

        private bool Validate(Portfolio portfolioToSave)
        {
            var validator = new FormValidator();
            validator.AddRequired(PortfolioName, "Please enter a portfolio name");
            validator.AddValidation(PortfolioName, () => IsDuplicateName(portfolioToSave));

            var result = validator.Apply();
            if(result != string.Empty)
            {
                Toast.MakeText(this, result, ToastLength.Long).Show();
                return false;
            }
            return true;
        }

        private string IsDuplicateName(Portfolio portfolioToSave)
        {
            var portfolio = _repo.GetPortfolioByName(portfolioToSave.Name);
            if(portfolio != null && portfolio.ID != portfolioToSave.ID)
            {
                return "Portfolio name is already taken";
            }
            return string.Empty;
        }

        private Portfolio GetPortfolioToSave()
        {
            Portfolio portfolioToSave;
            var portfolioId = Intent.GetLongExtra(Extra_PortfolioID, -1);
            if (portfolioId != -1)
            {
                portfolioToSave = new Portfolio(portfolioId);
            }
            else
            {
                portfolioToSave = new Portfolio();
            }
            portfolioToSave.Name =  PortfolioName.Text.ToString();
            return portfolioToSave;
        }
    }
}