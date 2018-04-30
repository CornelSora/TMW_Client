using System;
using SQLite;

namespace TMW_Client.Models
{
    [Table("Jokes")]
    public class Joke : BaseDataObject
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string JokeID { get; set; }
        public Nullable<int> NoLikes { get; set; }
        public Nullable<int> NoUnlikes { get; set; }
    }
}
