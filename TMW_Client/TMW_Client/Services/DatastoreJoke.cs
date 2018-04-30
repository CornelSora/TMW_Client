using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMW_Client.Models;
using SQLite;
using System.IO;

namespace TMW_Client.Services
{
    public class DatastoreJoke
    {
        private static SQLiteConnection db = null;

        public DatastoreJoke()
        {
            if (db == null)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "ormdemo.db3");
                db = new SQLiteConnection(dbPath);
            }
        }

        public void Add(Joke item)
        {
            db.CreateTable<Joke>();
            db.Insert(item);
        }

        public void AddAll(List<Joke> items)
        {
            db.CreateTable<Joke>();
            var jokeTable = db.Table<Joke>();
            jokeTable.Delete(x => x.Id > 0);
            db.InsertAll(items);
        }

        public Joke Get(string id)
        {
            var joke = db.Get<Joke>(id); // primary key id of 5
            return joke;
        }

        public IEnumerable<Joke> GetAll()
        {
            return db.Table<Joke>();
        }

        public void Update(Joke joke)
        {
            db.Update(joke);
        }

        public List<Joke> GetMyJokes(int UserID)
        {
            var jokes = new List<Joke>();
            var tableQuery = db.Table<Joke>().Where(x => x.UserID == UserID);
            foreach (var item in tableQuery)
            {
                jokes.Add(item);
            }
            return jokes;
        }

    }
}
