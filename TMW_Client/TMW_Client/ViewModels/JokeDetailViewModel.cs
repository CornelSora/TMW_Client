using System;
using System.Collections.Generic;
using System.Text;
using TMW_Client.Models;

namespace TMW_Client.ViewModels
{
    public class JokeDetailViewModel: BaseViewModel
    {
        public Joke Item { get; set; }

        public JokeDetailViewModel(Joke item = null)
        {
            Title = item?.Title;
            Item = item;
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}
