using Android.Content;
using Android.Database.Sqlite;
using Android.Util;

namespace MonoStockPortfolio.Core
{
    public abstract class AndroidSqliteBase : SQLiteOpenHelper
    {
        public const string PORTFOLIO_TABLE_NAME = "Portfolios";
        public const string POSITION_TABLE_NAME = "Positions";
        public const string CONFIG_TABLE_NAME = "Config";

        public const string DATABASE_NAME = "stockportfolio.db";
        public const int DATABASE_VERSION = 1;

        protected AndroidSqliteBase(Context context)
            : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {
        }

        protected SQLiteDatabase Db { get { return WritableDatabase; } }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL("CREATE TABLE " + PORTFOLIO_TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT)");
            db.ExecSQL("CREATE TABLE " + POSITION_TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, Ticker TEXT, Shares REAL, PricePerShare REAL, ContainingPortfolioID INT)");
            db.ExecSQL("CREATE TABLE " + CONFIG_TABLE_NAME + " (StockItems TEXT)");

            db.ExecSQL("INSERT INTO " + CONFIG_TABLE_NAME + " (StockItems) VALUES ('2,0,1,3')");
            db.ExecSQL("INSERT INTO " + PORTFOLIO_TABLE_NAME + " (Name) VALUES ('Sample portfolio')");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('GOOG', '500', '593.97', 1)");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('AMZN', '500', '180.00', 1)");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('AAPL', '500', '322.56', 1)");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('MSFT', '500', '27.91', 1)");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('NOVL', '500', '5.92', 1)");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('S', '500', '4.23', 1)");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('VZ', '500', '35.78', 1)");
            db.ExecSQL("INSERT INTO " + POSITION_TABLE_NAME + " (Ticker, Shares, PricePerShare, ContainingPortfolioID) VALUES ('T', '500', '29.38', 1)");
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            Log.W("Upgrade", "Nothing to upgrade");
        }
    }
}