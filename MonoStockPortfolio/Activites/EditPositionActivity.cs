using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.StockData;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Add Position", MainLauncher = false, Name = "monostockportfolio.activites.EditPositionActivity")]
    public class EditPositionActivity : Activity
    {
        [IoC] private IPortfolioRepository _repo;
        [IoC] private IStockDataProvider _svc;

        [LazyView(Resource.Id.addPositionTicker)] protected EditText TickerTextBox;
        [LazyView(Resource.Id.addPositionPrice)] protected EditText PriceTextBox;
        [LazyView(Resource.Id.addPositionShares)] protected EditText SharesTextBox;
        [LazyView(Resource.Id.addPositionSaveButton)] protected Button SaveButton;

        private const string POSITIONIDEXTRA = "monoStockPortfolio.EditPositionActivity.PositionID";
        private const string PORTFOLIOIDEXTRA = "monoStockPortfolio.EditPositionActivity.PortfolioID";

        public static Intent AddIntent(Context context, long portfolioId)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<EditPositionActivity>());
            intent.PutExtra(PORTFOLIOIDEXTRA, portfolioId);
            return intent;
        }
        public static Intent EditIntent(Context context, long positionId, long portfolioId)
        {
            var intent = new Intent();
            intent.SetClassName(context, ManifestNames.GetName<EditPositionActivity>());
            intent.PutExtra(POSITIONIDEXTRA, positionId);
            intent.PutExtra(PORTFOLIOIDEXTRA, portfolioId);
            return intent;
        }
        public long ThisPortfolioId { get { return Intent.GetLongExtra(PORTFOLIOIDEXTRA, -1); } }
        public long ThisPositionId { get { return Intent.GetLongExtra(POSITIONIDEXTRA, -1); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.addposition);

            var positionId = ThisPositionId;
            if (positionId != -1)
            {
                this.Title = "Edit Position";
                PopulateForm(positionId);
            }

            WireUpEvents();
        }

        private void PopulateForm(long positionId)
        {
            var position = _repo.GetPositionById(positionId);
            this.TickerTextBox.Text = position.Ticker;
            this.PriceTextBox.Text = position.PricePerShare.ToString();
            this.SharesTextBox.Text = position.Shares.ToString();
        }

        private void WireUpEvents()
        {
            SaveButton.Click += saveButton_Click;
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            if(Validate())
            {
                var positionToSave = GetPositionToSave();
                _repo.SavePosition(positionToSave);

                this.EndActivity();
            }
        }

        private bool Validate()
        {
            var result = ValidationRules.Apply();

            if (result == string.Empty)
            {
                return true;
            }

            this.LongToast(result);
            return false;
        }

        private FormValidator ValidationRules
        {
            get
            {
                var validator = new FormValidator();
                validator.AddRequired(TickerTextBox, "Please enter a ticker");
                validator.AddValidPositiveDecimal(SharesTextBox, "Please enter a valid, positive number of shares");
                validator.AddValidPositiveDecimal(PriceTextBox, "Please enter a valid, positive price per share");
                validator.AddValidation(TickerTextBox, () => ValidateTicker(TickerTextBox.Text));
                return validator;
            }
        }

        private string ValidateTicker(string ticker)
        {
            if(_svc.IsValidTicker(ticker))
            {
                return string.Empty;
            }
            return "Invalid Ticker Name";
        }

        private Position GetPositionToSave()
        {
            Position positionToSave;
            var positionId = ThisPositionId;
            if (positionId != -1)
            {
                positionToSave = new Position(positionId);
            }
            else
            {
                positionToSave = new Position();
            }

            positionToSave.Shares = decimal.Parse(SharesTextBox.Text.ToString());
            positionToSave.PricePerShare = decimal.Parse(PriceTextBox.Text.ToString());
            positionToSave.Ticker = TickerTextBox.Text.ToString();
            positionToSave.ContainingPortfolioID = ThisPortfolioId;
            return positionToSave;
        }
    }
}