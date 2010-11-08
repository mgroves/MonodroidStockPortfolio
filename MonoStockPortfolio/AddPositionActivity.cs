using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Core.Services;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio
{
    [Activity(Label = "Add Position", MainLauncher = false)]
    public class AddPositionActivity : Activity
    {
        public AddPositionActivity(IntPtr handle) : base(handle) { }
        public static string ClassName { get { return "monoStockPortfolio.AddPositionActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.AddPositionActivity.PortfolioID"; } }
        private IPortfolioRepository _repo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.addposition);

            var saveButton = FindViewById<Button>(Resource.id.addPositionSaveButton);
            saveButton.Click += saveButton_Click;
        }

        void saveButton_Click(object sender, System.EventArgs e)
        {
            var position = new Position();
            if(Validate(position))
            {
                _repo = new AndroidSqlitePortfolioRepository(this);
                _repo.SavePosition(position);

                var intent = new Intent();
                SetResult(Result.Ok, intent);
                Finish();
            }
        }

        private bool Validate(Position position)
        {
            var tickerTextBox = FindViewById<EditText>(Resource.id.addPositionTicker);
            var priceTextBox = FindViewById<EditText>(Resource.id.addPositionPrice);
            var sharesTextBox = FindViewById<EditText>(Resource.id.addPositionShares);

            string errorMessage = string.Empty;

            if (string.IsNullOrEmpty(tickerTextBox.Text.ToString()))
            {
                errorMessage += "Please enter a ticker\n";
            }

            decimal dummy;
            if (string.IsNullOrEmpty(sharesTextBox.Text.ToString()))
            {
                errorMessage += "Please enter the number of shares you bought\n";
            }
            else if (!decimal.TryParse(sharesTextBox.Text.ToString(), out dummy) ||
                     decimal.Parse(sharesTextBox.Text.ToString()) <= 0)
            {
                errorMessage += "Please enter a valid number of shares";
            }

            if (string.IsNullOrEmpty(priceTextBox.Text.ToString()))
            {
                errorMessage += "Please enter the price you bought these shares at";
            }
            else if (!decimal.TryParse(priceTextBox.Text.ToString(), out dummy) ||
                     decimal.Parse(priceTextBox.Text.ToString()) <= 0)
            {
                errorMessage += "Please enter a valid price";
            }

            if (errorMessage == string.Empty)
            {
                position.Shares = decimal.Parse(sharesTextBox.Text.ToString());
                position.PricePerShare = decimal.Parse(priceTextBox.Text.ToString());
                position.Ticker = tickerTextBox.Text.ToString();
                position.ContainingPortfolioID = Intent.GetLongExtra(Extra_PortfolioID, -1);
                return true;
            }

            Toast.MakeText(this, errorMessage, ToastLength.Long).Show();
            return false;
        }
    }
}