using System;
using System.Collections.Generic;
using System.Text;

namespace TMW_Client.Models
{
    public class Joke : BaseDataObject
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string JokeID { get; set; }
        public Nullable<int> NoLikes { get; set; }
        public Nullable<int> NoUnlikes { get; set; }
    }
}
