using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites.EditPositionScreen
{
    [Activity(Label = "Add Position", MainLauncher = false, Name = "monostockportfolio.activites.EditPositionActivity")]
    public class EditPositionActivity : Activity, IEditPositionView
    {
        [IoC] IEditPositionPresenter _presenter;

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

        #region IEditPositionView implementation
        
        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void PopulateForm(Position position)
        {
            this.TickerTextBox.Text = position.Ticker;
            this.PriceTextBox.Text = position.PricePerShare.ToString();
            this.SharesTextBox.Text = position.Shares.ToString();
        }

        public void GoBackToMainActivity()
        {
            this.EndActivity();
        }

        public void ShowErrorMessages(IList<string> errorMessages)
        {
            var errorMessage = string.Empty;
            foreach (var error in errorMessages)
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

            SetContentView(Resource.Layout.addposition);

            var portfolioId = Intent.GetLongExtra(PORTFOLIOIDEXTRA, -1);
            var positionId = Intent.GetLongExtra(POSITIONIDEXTRA, -1);
            if (positionId != -1)
            {
                _presenter.Initialize(this, portfolioId, positionId);
            }
            else
            {
                _presenter.Initialize(this, portfolioId);
            }

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            SaveButton.Click += saveButton_Click;
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            _presenter.Save(GetPositionInputModel());
        }

        private PositionInputModel GetPositionInputModel()
        {
            var model = new PositionInputModel();
            model.TickerText = this.TickerTextBox.Text;
            model.PriceText = this.PriceTextBox.Text;
            model.SharesText = this.SharesTextBox.Text;
            return model;
        }
    }
}