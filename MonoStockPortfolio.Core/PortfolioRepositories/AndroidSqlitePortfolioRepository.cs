using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database.Sqlite;
using Android.Util;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.PortfolioRepositories
{
    public class AndroidSqlitePortfolioRepository : IPortfolioRepository
    {
        private OpenHelper _dbHelper;
        private SQLiteDatabase _db;
        private const string PORTFOLIO_TABLE_NAME = "Portfolios";
        private const string DATABASE_NAME = "stockportfolio.db";
        private const int DATABASE_VERSION = 1;
        private const string POSITION_TABLE_NAME = "Positions";

        public AndroidSqlitePortfolioRepository(Context context)
        {
            _dbHelper = new OpenHelper(context, DATABASE_NAME, null, DATABASE_VERSION);
            _db = _dbHelper.WritableDatabase;
        }

        public IList<Portfolio> GetAllPortfolios()
        {
            var list = new List<Portfolio>();
            var cursor = _db.Query(PORTFOLIO_TABLE_NAME, new[] {"id", "Name"}, null, null, null, null, null);
            if(cursor.Count > 0)
            {
                while(cursor.MoveToNext())
                {
                    var portfolio = new Portfolio(cursor.GetInt(0));
                    portfolio.Name = cursor.GetString(1);
                    list.Add(portfolio);
                }
            }
            if(!cursor.IsClosed) cursor.Close();
            return list;
        }

        public Portfolio GetPortfolioById(long portfolioId)
        {
            var cursor = _db.Query(PORTFOLIO_TABLE_NAME, new[] { "id", "Name" }, null, null, null, null, null);
            if (cursor.Count > 0)
            {
                cursor.MoveToNext();
                var portfolio = new Portfolio(cursor.GetInt(0));
                portfolio.Name = cursor.GetString(1);
                if (!cursor.IsClosed) cursor.Close();
                return portfolio;
            }
            return null;
        }

        public void SavePosition(Position position)
        {
            _db.Insert(POSITION_TABLE_NAME, null, GetPositionContentValues(position));
        }


        public void DeletePortfolio(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public IList<Position> GetAllPositions(long portfolioId)
        {
            var list = new List<Position>();

            var cursor = _db.Query(POSITION_TABLE_NAME, new[] { "id", "Ticker", "Shares", "PricePerShare" }, " ContainingPortfolioID = " + portfolioId, null, null, null, null);
            if (cursor.Count > 0)
            {
                while (cursor.MoveToNext())
                {
                    var position = new Position(cursor.GetInt(0));
                    position.Ticker = cursor.GetString(1);
                    position.Shares = Convert.ToDecimal(cursor.GetFloat(2));
                    position.PricePerShare = Convert.ToDecimal(cursor.GetFloat(3));
                    list.Add(position);
                }
            }
            if (!cursor.IsClosed) cursor.Close();
            return list;
        }

        public void SavePortfolio(Portfolio portfolio)
        {
            if (portfolio.ID == null)
            {
                InsertNewPortfolio(portfolio);
            }
            else
            {
                UpdateExistingPortfolio(portfolio);
            }
        }

        private void UpdateExistingPortfolio(Portfolio portfolio)
        {
            var portfolioID = portfolio.ID ?? -1;
            _db.Update(PORTFOLIO_TABLE_NAME, GetPortfolioContentValues(portfolio), " WHERE id = " + portfolioID, null);
        }

        private void InsertNewPortfolio(Portfolio portfolio)
        {
            _db.Insert(PORTFOLIO_TABLE_NAME, null, GetPortfolioContentValues(portfolio));
        }

        private static ContentValues GetPortfolioContentValues(Portfolio portfolio)
        {
            var contentValues = new ContentValues();
            contentValues.Put("Name", portfolio.Name);
            return contentValues;
        }

        private ContentValues GetPositionContentValues(Position position)
        {
            var positionValues = new ContentValues();
            positionValues.Put("PricePerShare", (double)position.PricePerShare);
            positionValues.Put("Ticker", position.Ticker);
            positionValues.Put("Shares", (double)position.Shares);
            positionValues.Put("ContainingPortfolioID", position.ContainingPortfolioID);
            return positionValues;
        }

        private class OpenHelper : SQLiteOpenHelper
        {
            public OpenHelper(Context context, string name, SQLiteDatabase.ICursorFactory factory, int version)
                : base(context, name, factory, version)
            {
            }

            public override void OnCreate(SQLiteDatabase db)
            {
                db.ExecSQL("CREATE TABLE " + PORTFOLIO_TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT)");
                db.ExecSQL("CREATE TABLE " + POSITION_TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, Ticker TEXT, Shares REAL, PricePerShare REAL, ContainingPortfolioID INT)");
            }

            public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
            {
                Log.W("Upgrade", "Nothing to upgrade");
            }
        }
    }


}