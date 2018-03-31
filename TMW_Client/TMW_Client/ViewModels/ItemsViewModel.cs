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
		public ObservableRangeCollection<Item> Items { get; set; }
        public ObservableRangeCollection<Joke> Jokes { get; set; }
        public Command LoadItemsCommand { get; set; }

		public ItemsViewModel()
		{
			Title = "Browse";
			Items = new ObservableRangeCollection<Item>();
            Jokes = new ObservableRangeCollection<Joke>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

			MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
			{
				var _item = item as Item;
				Items.Add(_item);
				await DataStore.AddItemAsync(_item);
			});

            MessagingCenter.Subscribe<NewItemPage, Joke>(this, "AddJoke", async (obj, item) =>
            {
                var _item = item as Joke;
                Jokes.Add(_item);
                await JokeDataStore.AddItemAsync(_item);
            });
        }

		async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				Items.Clear();
				var items = await DataStore.GetItemsAsync(true);
				Items.ReplaceRange(items);

                Jokes.Clear();
                var jokes = await JokeDataStore.GetItemsAsync(true);
                Jokes.ReplaceRange(jokes);

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