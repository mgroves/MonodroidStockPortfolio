using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;
using MonoStockPortfolio.Validation;

namespace MonoStockPortfolio
{
    [Activity(Label = "Add Position", MainLauncher = false)]
    public class AddPositionActivity : Activity
    {
        public AddPositionActivity(IntPtr handle) : base(handle)
        {
            _repo = new AndroidSqlitePortfolioRepository(this);
        }

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

            var validator = new FormValidator();
            validator.AddRequired(tickerTextBox, "Please enter a ticker");
            validator.AddValidPositiveDecimal(sharesTextBox, "Please enter a valid, positive number of shares");
            validator.AddValidPositiveDecimal(priceTextBox, "Please enter a valid, positive price per share");

            var result = validator.Validate();

            if (result == string.Empty)
            {
                position.Shares = decimal.Parse(sharesTextBox.Text.ToString());
                position.PricePerShare = decimal.Parse(priceTextBox.Text.ToString());
                position.Ticker = tickerTextBox.Text.ToString();
                position.ContainingPortfolioID = Intent.GetLongExtra(Extra_PortfolioID, -1);
                return true;
            }

            Toast.MakeText(this, result, ToastLength.Long).Show();
            return false;
        }
    }
}