using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TMW_Client.Models;

namespace TMW_Client.Services
{
    public class DatastoreUser 
    {
        private static SQLiteConnection db = null;

        public DatastoreUser()
        {
            if (db == null)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "ormdemo.db3");
                db = new SQLiteConnection(dbPath);
            }
        }

        public void Add(User item)
        {
            db.CreateTable<User>();
            var user = db.Table<User>().Where(x => x.Username == item.Username).FirstOrDefault();
            if (user == null)
            {
                db.Insert(item);
            }
            else
            {
                db.Update(item);
            }
        }

        public void AddAll(List<User> items)
        {
            db.CreateTable<User>();
            var userTable = db.Table<User>();
            userTable.Delete(x => x.UserID > 0);
            db.InsertAll(items);
        }

        public User GetAvailable()
        {
            db.CreateTable<User>();
            var user = db.Table<User>().Where(x => x.IsLoggedIn == true).FirstOrDefault(); 
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Table<User>();
        }

        public void Update(User user)
        {
            user.IsLoggedIn = false;
            db.Update(user);
        }

        public User Login(User user)
        {
            var _user = db.Table<User>().Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
            return _user;
        }

        public string GetPassword(string username)
        {
            var user = db.Table<User>().Where(x => x.Username == username).FirstOrDefault();
            return user != null ? user.Password : "";
        }
    }
}
