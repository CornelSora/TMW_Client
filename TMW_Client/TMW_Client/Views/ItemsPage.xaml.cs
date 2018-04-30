using Plugin.Connectivity;
using System;
using System.Diagnostics;
using TMW_Client.Models;
using TMW_Client.Services;
using TMW_Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMW_Client.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        private int UserID = -1;

        public ItemsPage()
        {
            InitializeComponent();

            ToolbarItems.RemoveAt(1);
            BindingContext = viewModel = new ItemsViewModel();
        }

        public ItemsPage(int userID)
        {
            InitializeComponent();
            ToolbarItems.RemoveAt(0);
            BindingContext = viewModel = new ItemsViewModel(userID);
            this.UserID = userID;
        }


        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
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
            } else
            {
                await Navigation.PushAsync(new ItemDetailPage(new JokeDetailViewModel(item), UserID));
            }

            // Manually deselect item
            ItemsListView.SelectedItem = null;
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

            if (viewModel.Jokes.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
