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
    [Activity(Label = "Add Portfolio", MainLauncher = false, Name = "monostockportfolio.activites.EditPortfolioActivity")]
    public class EditPortfolioActivity : Activity
    {
        [IoC] private IPortfolioRepository _repo;

        [LazyView(Resource.Id.btnSave)] protected Button SaveButton;
        [LazyView(Resource.Id.portfolioName)] protected EditText PortfolioName;

        private const string PORTFOLIOIDEXTRA = "monoStockPortfolio.EditPortfolioActivity.PortfolioID";

        public static Intent AddIntent(Context context)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<EditPortfolioActivity>());
            return intent;
        }
        public static Intent EditIntent(Context context, long portfolioId)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<EditPortfolioActivity>());
            intent.PutExtra(PORTFOLIOIDEXTRA, portfolioId);
            return intent;
        }
        public long ThisPortfolioId { get { return Intent.GetLongExtra(PORTFOLIOIDEXTRA, -1); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.addportfolio);

            WireUpEvents();

            var portfolioId = ThisPortfolioId;
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

                this.LongToast("You saved: " + PortfolioName.Text);

                this.EndActivity();
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
                this.LongToast(result);
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
            var portfolioId = ThisPortfolioId;
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