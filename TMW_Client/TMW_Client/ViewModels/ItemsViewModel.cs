using System;
using System.Diagnostics;
using System.Threading.Tasks;

using TMW_Client.Helpers;
using TMW_Client.Models;
using TMW_Client.Views;

using Xamarin.Forms;

namespace TMW_Client.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Joke> Jokes { get; set; }
        public Command LoadItemsCommand { get; set; }
        public int UserID { get; set; }

        public ItemsViewModel()
        {
            UserID = -1;
            Title = "Browse";
            Jokes = new ObservableRangeCollection<Joke>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Joke>(this, "AddJoke", async (obj, item) =>
            {
                var _item = item as Joke;
                Jokes.Add(_item);
                await JokeInitializer.AddItemAsync(_item);
            });
        }

        public ItemsViewModel(int userID)
        {
            Title = "Browse";
            Jokes = new ObservableRangeCollection<Joke>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            UserID = userID;
            MessagingCenter.Subscribe<NewItemPage, Joke>(this, "AddJoke", async (obj, item) =>
            {
                var _item = item as Joke;
                Jokes.Add(_item);
                await JokeInitializer.AddItemAsync(_item);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Jokes.Clear();
                if (UserID > 0)
                {
                    var jokes = await JokeInitializer.GetItemsAsync(true, UserID);
                    Jokes.ReplaceRange(jokes);
                }
                else
                {
                    var jokes = await JokeInitializer.GetItemsAsync(true);
                    Jokes.ReplaceRange(jokes);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}