﻿
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using SQLite;


namespace MultiCurrencyWallet
{
    public class DatabaseOps
    {

        public SQLiteConnection database;
        public static object locker = new object();

        public DatabaseOps()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Currency>();
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            lock (locker)
            {
                return (from i in database.Table<Currency>() select i).ToList();
            }
        }

        public IEnumerable<Currency> GetItemsNotDone()
        {
            lock (locker)
            {
                return database.Query<Currency>("SELECT * FROM Currency WHERE rate = 1");
            }
        }

        public Currency GetCurrency(string code)
        {
            lock (locker)
            {
                return database.Table<Currency>().FirstOrDefault(x => x.code == code);
            }
        }

        public int DeleteCurrency(int id)
        {
            lock (locker)
            {
                return database.Delete<Currency>(id);
            }
        }


        public void InsertCurrency(Currency c)
        {
            lock (locker)
            {
                database.Insert(c);
            }
        }

        /**
         * Updates item, or inserts if it doesn't exist.
         */
        public void UpdateCurrency(Currency c)
        {
            lock (locker)
            {
                if (database.Update(c) == 0)
                    database.Insert(c);
            }
        }
    }
}
