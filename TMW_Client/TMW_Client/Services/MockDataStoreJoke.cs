using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TMW_Client.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(TMW_Client.Services.MockDataStoreJoke))]
namespace TMW_Client.Services
{
	public class MockDataStoreJoke : IDataStore<Joke>
	{
		bool isInitialized;
        public bool isBusy { get; set; }

        List<Joke> Jokes;

		public async Task<bool> AddItemAsync(Joke Joke)
		{
			await InitializeAsync();

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

		public async Task<IEnumerable<Joke>> GetItemsAsync(bool forceRefresh = false)
		{
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
			//if (isInitialized)
			//	return;
            if (isBusy)
                return;

            isBusy = true;

			Jokes = new List<Joke>();

            string URL = "http://172.24.193.225/api/jokes";

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));
            var webClient = new WebClient();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jokeList = JsonConvert.DeserializeObject<List<Joke>>(content);
                ////Databind the list
                //lstWeather.JokesSource = weatherList;
                foreach (var joke in jokeList)
                {
                    Jokes.Add(joke);
                }
                
            }

            isInitialized = true;
            isBusy = false;
		}
	}
}
