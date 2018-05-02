using System;
using System.Collections.Generic;
using System.Text;
using TMW_Client.Models;
using TMW_Client.Services;
using TMW_Client.Views;
using Xamarin.Forms;

namespace TMW_Client.ViewModels
{
    public class JokeDetailViewModel: BaseViewModel
    {
        public Joke Item { get; set; }

        public JokeDetailViewModel(Joke item = null)
        {
            Title = item?.Title;
            Item = item;

            MessagingCenter.Subscribe<ItemDetailPage, string>(this, "LikeJoke", async (obj, jokeid) =>
            {
                var _jokeService = new JokeService();
                var joke = await _jokeService.GetJoke(jokeid, true);
                item.NoLikes = joke.NoLikes;
                item.NoUnlikes = joke.NoUnlikes;
            });

        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}
