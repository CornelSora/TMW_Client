﻿using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMW_Client.Models;
using TMW_Client.Services;
using TMW_Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMW_Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyJokes : ContentPage
	{
        ItemsViewModel firstViewModel;
        ItemsViewModel secondViewModel;

        private int UserID = -1;

        public MyJokes(int userID)
        {
            InitializeComponent();
            ToolbarItems.RemoveAt(0);
            BindingContext = secondViewModel = new ItemsViewModel(userID);
            this.UserID = userID;
        }


        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                var item = args.SelectedItem as Joke;
                if (item == null)
                    return;

                if (CrossConnectivity.Current.IsConnected)
                {
                    var _jokeService = new JokeService();
                    item = await _jokeService.GetJoke(item.JokeID, false);
                }
                if (this.UserID < 0)
                {
                    await Navigation.PushAsync(new ItemDetailPage(new JokeDetailViewModel(item)));
                }
                else
                {
                    await Navigation.PushAsync(new ItemDetailPage(new JokeDetailViewModel(item), UserID));
                }

                // Manually deselect item
                ItemsListView.SelectedItem = null;
            }
            catch (Exception)
            {
                Navigation.InsertPageBefore(new ItemsPage(), this);
                await Navigation.PopAsync();
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new NewItemPage());
            }
        }

        async void Logout_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                App.Logout();
                var tab = App.Current.MainPage as TabbedPage;
                tab.Children[1].Title = "Login";
                await Navigation.PushAsync(new LoginPage());
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (firstViewModel != null && firstViewModel.Jokes.Count == 0)
                firstViewModel.LoadItemsCommand.Execute(null);
            if (secondViewModel != null && secondViewModel.Jokes.Count == 0)
                secondViewModel.LoadItemsCommand.Execute(null);
        }
    }
}