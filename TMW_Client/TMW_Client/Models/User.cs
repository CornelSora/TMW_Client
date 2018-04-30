using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMW_Client.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
