using System;
using Android.App;
using Android.OS;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Framework;

namespace MonoStockPortfolio.Activites
{
    [Activity(Label = "Add Position", MainLauncher = false)]
    public partial class EditPositionActivity : Activity
    {
        [IoC] private IPortfolioRepository _repo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.addposition);

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
                return validator;
            }
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