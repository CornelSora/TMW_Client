using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMW_Client.Helpers;
using TMW_Client.Models;

namespace TMW_Client.Services
{
    public class JokeService
    {
        public async Task<bool> AddAsyncJokeAsync(Joke joke)
        {
            var id = App.user?.UserID;
            var secret = "secret" + id;
            string URL = Utils.PublicURL + "/api/jokes?secret=" + secret;
            HttpClient httpClient = new HttpClient();
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", joke.Text),
            });
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(URL, formContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(true);
        }

        public async Task<List<Joke>> GetAsyncJokes(int UserID)
        {
            string URL = "";
            if (UserID > 0)
            {
                URL = Utils.PublicURL + "/api/jokes/" + UserID;
            }
            else
            {
                URL = Utils.PublicURL + "/api/jokes";
            }

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));
            var webClient = new WebClient();
            var jokeList = new List<Joke>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                jokeList = JsonConvert.DeserializeObject<List<Joke>>(content);
            }
            return await Task.FromResult<List<Joke>>(jokeList);
        }

        public async Task<bool> LikeOrUnlike(bool isLike, string jokeid)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }
            if (App.user != null)
            {
                var userid = App.user.UserID;
                string URL = Utils.PublicURL + "/api/jokes/like?UserID=" + userid + "&JokeID=" + jokeid + "&isLike=" + isLike;

                HttpClient httpClient = new HttpClient();
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("", "")
                });
                HttpResponseMessage response = await httpClient.PutAsync(new Uri(URL), formContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// returns -2 if not internet connection; 1 if ok; 0 if not
        /// </summary>
        /// <param name="jokeID"></param>
        /// <returns>-2 if not internet connection; 1 if ok; 0 if not</returns>
        public async Task<int> DeleteAsync(string jokeID)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return -2;
            }
            var userID = App.user.UserID;
            var URL = Utils.PublicURL + "/api/joke/delete?UserID=" + userID + "&JokeID=" + jokeID;

            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(new Uri(URL));
            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            return 0;
        }

        public async Task<Joke> GetJoke(string jokeid, bool likeSuccess)
        {
            Joke joke = new Joke();
            if (!CrossConnectivity.Current.IsConnected)
            {
                return joke;
            }
            var URL = Utils.PublicURL + "/api/joke?JokeID=" + jokeid;
            var _jokeDS = new DatastoreJoke();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(new Uri(URL));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                joke = JsonConvert.DeserializeObject<Joke>(content);
            }

            if (likeSuccess)
            {
                _jokeDS.Update(joke);
            }

            return await Task.FromResult<Joke>(joke);

        }
    }
}
