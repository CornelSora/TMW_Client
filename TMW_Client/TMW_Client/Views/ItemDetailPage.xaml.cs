
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using TMW_Client.Helpers;
using TMW_Client.Models;
using TMW_Client.Services;
using TMW_Client.ViewModels;

using Xamarin.Forms;

namespace TMW_Client.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        class TimerExampleState
        {
            public int counter = 0;
            public Timer tmr;
        }

        JokeDetailViewModel jokeModel;
        private int UserID = -1;
        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        //public ItemDetailPage()
        //{
        //    InitializeComponent();
        //}

        public class BindablePropertyChangedEventArgs
        {
            public object NewValue { get; set; }
            public object OldValue { get; set; }
            public BindableProperty Property { get; set; }
        }

        public event EventHandler<BindablePropertyChangedEventArgs> BindingContextChanged;

        public ItemDetailPage(JokeDetailViewModel viewModel, int UserID)
        {
            InitializeComponent();

            BindingContext = this.jokeModel = viewModel;

            this.UserID = UserID;

            btnDelete.IsVisible = true;

            DoAgain();
        }

        public void DoAgain()
        {
            TimerExampleState s = new TimerExampleState();

            // Create the delegate that invokes methods for the timer.
            TimerCallback timerDelegate = new TimerCallback(CheckStatus);

            // Create a timer that waits one second, then invokes every second.
            Timer timer = new Timer(timerDelegate, s, 1000, 3000);

            // Keep a handle to the timer, so it can be disposed.
            s.tmr = timer;

            // The main thread does nothing until the timer is disposed.
            Console.WriteLine("Timer example done.");
        }

        async void CheckStatus(Object state)
        {
            TimerExampleState s = (TimerExampleState)state;
            s.counter++;
            Console.WriteLine("{0} Checking Status {1}.", DateTime.Now.TimeOfDay, s.counter);

            var jokeid = jokeModel.Item.JokeID;
            var _jokeService = new JokeService();
            var joke = await _jokeService.GetJoke(jokeid, true);
            this.jokeModel.Item.NoLikes = joke.NoLikes;
            this.jokeModel.Item.NoUnlikes = joke.NoUnlikes;
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
                //MessagingCenter.Send(this, "LikeJoke", jokeid);
                var joke = await _jokeService.GetJoke(jokeid, true);
                this.jokeModel.Item.NoLikes = joke.NoLikes;
                this.jokeModel.Item.NoUnlikes = joke.NoUnlikes;
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
        public void changeStatus(Joke joke)
        {
            lblDislike.Text = joke.NoUnlikes + "";
            lblLike.Text = joke.NoLikes + "";
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var joke = jokeModel.Item as Joke;
            btnDelete.IsEnabled = false;
            if (UserID > 0)
            {
                bool accepted = await DisplayAlert("Delete Context Action", joke.Title + " delete context action", "OK", "Cancel");
                if (accepted)
                {
                    var _jokeService = new JokeService();
                    int result = await _jokeService.DeleteAsync(joke.JokeID);
                    if (result == -2)
                    {
                        await DisplayAlert("No internet connection", "You must be online!", "Ok");
                    }
                    else if (result == 0)
                    {
                        await DisplayAlert("Error", "We are sorry! An error has occured!", "Ok");
                    }
                    else
                    {
                        MessagingCenter.Send(this, "DeleteJoke", joke);
                        //await Navigation.PopToRootAsync();
                        Navigation.InsertPageBefore(new ItemsPage(UserID), this);
                        await Navigation.PopAsync();
                    }
                }
            }
            btnDelete.IsEnabled = true;
        }
    }
}
