using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.EditPortfolioScreen
{
    [Activity(Label = "Add Portfolio", MainLauncher = false, Name = "monostockportfolio.activites.EditPortfolioActivity")]
    public class EditPortfolioActivity : Activity, IEditPortfolioView
    {
        [IoC] private IEditPortfolioPresenter _presenter;

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

        #region IEditPortfolioView members

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void PopulateForm(Portfolio portfolio)
        {
            PortfolioName.Text = portfolio.Name;
        }

        public void ShowSaveSuccessMessage(string message)
        {
            this.LongToast(message);
        }

        public void GoBackToMainActivity()
        {
            this.EndActivity();
        }

        public void ShowValidationErrors(IEnumerable<string> errors)
        {
            var errorMessage = string.Empty;
            foreach (var error in errors)
            {
                errorMessage += error + "\n";
            }
            errorMessage = errorMessage.Trim('\n');
            this.LongToast(errorMessage);
        }

        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.addportfolio);

            var portfolioId = Intent.GetLongExtra(PORTFOLIOIDEXTRA, -1);
            if (portfolioId != -1)
            {
                _presenter.Initialize(this, portfolioId);
            }
            else
            {
                _presenter.Initialize(this);
            }

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            SaveButton.Click += saveButton_Click;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            _presenter.SavePortfolio(GetPortfolioToSave());
        }

        private Portfolio GetPortfolioToSave()
        {
            Portfolio portfolioToSave;
            var portfolioId = Intent.GetLongExtra(PORTFOLIOIDEXTRA, -1);
            if (portfolioId != -1)
            {
                portfolioToSave = new Portfolio(portfolioId);
            }
            else
            {
                portfolioToSave = new Portfolio();
            }
            portfolioToSave.Name =  PortfolioName.Text;
            return portfolioToSave;
        }
    }
}