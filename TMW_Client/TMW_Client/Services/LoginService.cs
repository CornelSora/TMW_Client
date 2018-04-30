using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TMW_Client.Helpers;
using TMW_Client.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(TMW_Client.Services.LoginService))]
namespace TMW_Client.Services
{
    public class LoginService : IAccount<User>
    {
        private static DatastoreUser db = null;

        public LoginService()
        {
            if (db == null)
            {
                db = new DatastoreUser();
            }
        }

        public async Task<int> LoginAsync(User user)
        {
            string URL = Utils.PublicURL + "/api/Account/Login";
            if (!CrossConnectivity.Current.IsConnected)
            {
                return -2;
            }

            HttpClient httpClient = new HttpClient();
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", user.Username),
                new KeyValuePair<string, string>("password", user.Password)
            });
            //formContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var id = 0;
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(URL, formContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Int32.TryParse(content, out id);
                    user.UserID = id;
                    user.IsLoggedIn = true;
                    db.Add(user);
                    if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        MessagingCenter.Send(new MessagingCenterAlert
                        {
                            Title = "Register successful",
                            Message = "The account was added in database",
                            Cancel = "OK"
                        }, "message");
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>-2 if no internet; 1 if success; 0 if user exists</returns>
        public async Task<int> RegisterAsync(User user)
        {
            string URL = Utils.PublicURL + "/api/Account/Register";
            if (!CrossConnectivity.Current.IsConnected)
            {
                return -2;
            }

            HttpClient httpClient = new HttpClient();
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", user.Username),
                new KeyValuePair<string, string>("password", user.Password)
            });
            //formContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(URL, formContent);
                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(0);
        }


        public User UserLoggedIn()
        {
            return db.GetAvailable();
        }

        public void Logout()
        {
            var user = App.user;
            user.IsLoggedIn = false;
            db.Update(user);
        }

        public Task<bool> Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}
