
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using TMW_Client.Helpers;
using TMW_Client.Models;
using TMW_Client.Services;
using TMW_Client.ViewModels;

using Xamarin.Forms;

namespace TMW_Client.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        JokeDetailViewModel jokeModel;
        private int UserID = -1;
        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public ItemDetailPage(JokeDetailViewModel viewModel, int UserID)
        {
            InitializeComponent();

            BindingContext = this.jokeModel = viewModel;

            this.UserID = UserID;
            btnDelete.IsVisible = true;
        }


        public ItemDetailPage(JokeDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.jokeModel = viewModel;

        }

        protected async void CallMethodForLike(bool isLike)
        {
            var jokeid = jokeModel.Item.JokeID;
            var _jokeService = new JokeService();
            var result = await _jokeService.LikeOrUnlike(isLike, jokeid);
            if (result)
            {
                var joke = await _jokeService.GetJoke(jokeid, true);
                lblDislike.Text = joke.NoUnlikes + "";
                lblLike.Text = joke.NoLikes + "";
            }
        }

        private void OnLikeButtonClicked(object sender, System.EventArgs e)
        {
            CallMethodForLike(true);
        }

        private void OnDislikeButtonClicked(object sender, System.EventArgs e)
        {
            CallMethodForLike(false);
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var joke = jokeModel.Item as Joke;

            if (UserID > 0)
            {
                DisplayAlert("Delete Context Action", joke.Title + " delete context action", "OK");
            }
        }
    }
}
