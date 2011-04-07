using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database.Sqlite;
using Android.Runtime;
using Android.Util;
using MonoStockPortfolio.Entities;

namespace MonoStockPortfolio.Core.PortfolioRepositories
{
    [Preserve(AllMembers = true)]
    public class AndroidSqlitePortfolioRepository : AndroidSqliteBase, IPortfolioRepository
    {
        public AndroidSqlitePortfolioRepository(Context context)
            : base(context)
        { }

        public IList<Portfolio> GetAllPortfolios()
        {
            var list = new List<Portfolio>();
            var cursor = Db.Query(PORTFOLIO_TABLE_NAME, new[] { "id", "Name" }, null, null, null, null, null);
            if (cursor.Count > 0)
            {
                while (cursor.MoveToNext())
                {
                    var portfolio = new Portfolio(cursor.GetInt(0));
                    portfolio.Name = cursor.GetString(1);
                    list.Add(portfolio);
                }
            }
            if (!cursor.IsClosed) cursor.Close();
            return list;
        }

        public Portfolio GetPortfolioById(long portfolioId)
        {
            var cursor = Db.Query(PORTFOLIO_TABLE_NAME, new[] { "id", "Name" }, " ID = " + portfolioId, null, null, null, null);
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
            if (position.ID == null)
            {
                InsertNewPosition(position);
            }
            else
            {
                UpdateExistingPosition(position);
            }
        }

        public void DeletePortfolioById(int portfolioId)
        {
            Db.BeginTransaction();
            try
            {
                Db.Delete(PORTFOLIO_TABLE_NAME, "id = " + portfolioId, null);
                Db.Delete(POSITION_TABLE_NAME, "ContainingPortfolioID = " + portfolioId, null);
                Db.SetTransactionSuccessful();
            }
            catch (SQLiteException)
            {
                Log.Error("DeletePortfolio", "SQLiteException => Id = " + portfolioId);
            }
            finally
            {
                Db.EndTransaction();
            }
        }

        public Portfolio GetPortfolioByName(string portfolioName)
        {
            var cursor = Db.RawQuery("SELECT id, Name FROM " + PORTFOLIO_TABLE_NAME + " WHERE Name = ?", new[] {portfolioName} );
            if (cursor.Count > 0)
            {
                cursor.MoveToNext();
                var portfolio = new Portfolio(cursor.GetLong(0));
                portfolio.Name = cursor.GetString(1);
                if (!cursor.IsClosed) cursor.Close();
                return portfolio;
            }
            return null;
        }

        public void DeletePositionById(long positionId)
        {
            Db.Delete(POSITION_TABLE_NAME, "id = " + positionId, null);
        }

        public Position GetPositionById(long positionId)
        {
            Position position = null;

            var cursor = Db.Query(POSITION_TABLE_NAME, new[] { "id", "Ticker", "Shares", "PricePerShare" }, " id = " + positionId, null, null, null, null);
            if (cursor.Count > 0)
            {
                while (cursor.MoveToNext())
                {
                    position = new Position(cursor.GetInt(0));
                    position.Ticker = cursor.GetString(1);
                    position.Shares = Convert.ToDecimal(cursor.GetFloat(2));
                    position.PricePerShare = Convert.ToDecimal(cursor.GetFloat(3));
                }
            }
            if (!cursor.IsClosed) cursor.Close();
            return position;
        }

        public IList<Position> GetAllPositions(long portfolioId)
        {
            var list = new List<Position>();

            var cursor = Db.Query(POSITION_TABLE_NAME, new[] { "id", "Ticker", "Shares", "PricePerShare" }, " ContainingPortfolioID = " + portfolioId, null, null, null, null);
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
            Log.Error("UpdateExistingPortfolio", "Portfolios updated: " + Db.Update(PORTFOLIO_TABLE_NAME, GetPortfolioContentValues(portfolio), "id = " + portfolioID, null));
        }

        private void InsertNewPortfolio(Portfolio portfolio)
        {
            Log.Error("InsertNewPortfolio", "Portfolios inserted: " + Db.Insert(PORTFOLIO_TABLE_NAME, null, GetPortfolioContentValues(portfolio)));
        }

        private void UpdateExistingPosition(Position position)
        {
            var positionID = position.ID ?? -1;
            Log.Error("UpdateExistingPosition", "Positions updated: " + Db.Update(POSITION_TABLE_NAME, GetPositionContentValues(position), "id = " + positionID, null));
        }

        private void InsertNewPosition(Position position)
        {
            Log.Error("InsertNewPosition", "Positions inserted: " + Db.Insert(POSITION_TABLE_NAME, null, GetPositionContentValues(position)));
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
    }
}