using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MonoStockPortfolio.Core.PortfolioRepositories;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio
{
    [Activity(Label = "Add Portfolio", MainLauncher = false)]
    public class AddPortfolioActivity : Activity
    {
        public AddPortfolioActivity(IntPtr handle) : base(handle)
        {
            _repo = new AndroidSqlitePortfolioRepository(this);
        }

        public static string ClassName { get { return "monoStockPortfolio.AddPortfolioActivity"; } }
        private IPortfolioRepository _repo; 

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.layout.addportfolio);

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            var saveButton = FindViewById<Button>(Resource.id.btnSave);
            saveButton.Click += saveButton_Click;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var portfolioName = FindViewById<EditText>(Resource.id.portfolioName);

            _repo.SavePortfolio(new Portfolio {Name = portfolioName.Text.ToString()});

            Toast.MakeText(this, "You saved: " + portfolioName.Text, ToastLength.Short).Show();

            var intent = new Intent();
            SetResult(Result.Ok, intent);
            Finish();
        }
    }
}