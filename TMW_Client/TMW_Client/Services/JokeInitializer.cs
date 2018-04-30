using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TMW_Client.Helpers;
using TMW_Client.Models;
using Xamarin.Forms;
using Plugin.Connectivity;

[assembly: Dependency(typeof(TMW_Client.Services.JokeInitializer))]
namespace TMW_Client.Services
{
	public class JokeInitializer : IDataStore<Joke>
	{
        public bool isBusy { get; set; }
        public DatastoreJoke DBJoke { get; set; }
        public static List<Joke> Jokes;
        public JokeService _jokeService;
        public int UserID { get; set; }


        public async Task<bool> AddItemAsync(Joke Joke)
		{
			await InitializeAsync();

            JokeService jokeService = new JokeService();
            await jokeService.AddAsyncJokeAsync(Joke);

			Jokes.Add(Joke);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(Joke Joke)
		{
			await InitializeAsync();

			var _Joke = Jokes.Where((Joke arg) => arg.JokeID == Joke.JokeID).FirstOrDefault();
			Jokes.Remove(_Joke);
			Jokes.Add(Joke);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(Joke Joke)
		{
			await InitializeAsync();

			var _Joke = Jokes.Where((Joke arg) => arg.JokeID == Joke.JokeID).FirstOrDefault();
			Jokes.Remove(_Joke);

			return await Task.FromResult(true);
		}

		public async Task<Joke> GetItemAsync(string id)
		{
			await InitializeAsync();

			return await Task.FromResult(Jokes.FirstOrDefault(s => s.JokeID == id));
		}

		public async Task<IEnumerable<Joke>> GetItemsAsync(bool forceRefresh = false, int UserID = -1)
		{
            this.UserID = UserID;
            await InitializeAsync();
			return await Task.FromResult(Jokes);
		}

		public Task<bool> PullLatestAsync()
		{
			return Task.FromResult(true);
		}


		public Task<bool> SyncAsync()
		{
			return Task.FromResult(true);
		}

		public async Task InitializeAsync()
		{
            if (isBusy)
                return;
            isBusy = true;
            Jokes = new List<Joke>();
            _jokeService = new JokeService();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DBJoke = new DatastoreJoke();
                if (UserID > 0)
                {
                    Jokes = DBJoke.GetMyJokes(UserID);
                }
                else
                {
                    Jokes = DBJoke.GetAll().ToList();
                }
                isBusy = false;
                return;
            }

            Jokes.Clear();
            Jokes = await _jokeService.GetAsyncJokes(UserID);
            isBusy = false;
        }
    }
}
