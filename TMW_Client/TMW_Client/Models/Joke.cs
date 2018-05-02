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
        int noLikes = 0;
        public int NoLikes
        {
            get { return noLikes; }
            set { SetProperty(ref noLikes, value); }
        }
        int noDislikes = 0;
        public int NoUnlikes
        {
            get { return noDislikes; }
            set { SetProperty(ref noDislikes, value); }
        }
    }
}
