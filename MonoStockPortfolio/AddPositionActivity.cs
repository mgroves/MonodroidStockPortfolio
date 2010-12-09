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
        public static string ClassName { get { return "monostockportfolio.AddPositionActivity"; } }
        public static string Extra_PortfolioID { get { return "monoStockPortfolio.AddPositionActivity.PortfolioID"; } }

        private IPortfolioRepository _repo;

        private EditText TickerTextBox { get { return FindViewById<EditText>(Resource.id.addPositionTicker); } }
        private EditText PriceTextBox { get { return FindViewById<EditText>(Resource.id.addPositionPrice); } }
        private EditText SharesTextBox { get { return FindViewById<EditText>(Resource.id.addPositionShares); } }
        private Button SaveButton { get { return FindViewById<Button>(Resource.id.addPositionSaveButton); } }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.addposition);

            _repo = new AndroidSqlitePortfolioRepository(this);

            SaveButton.Click += saveButton_Click;
        }

        void saveButton_Click(object sender, EventArgs e)
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
            var result = ValidationRules.Apply();

            if (result == string.Empty)
            {
                position.Shares = decimal.Parse(SharesTextBox.Text.ToString());
                position.PricePerShare = decimal.Parse(PriceTextBox.Text.ToString());
                position.Ticker = TickerTextBox.Text.ToString();
                position.ContainingPortfolioID = Intent.GetLongExtra(Extra_PortfolioID, -1);
                return true;
            }

            Toast.MakeText(this, result, ToastLength.Long).Show();
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
    }
}