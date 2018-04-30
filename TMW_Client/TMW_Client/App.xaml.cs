using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using TMW_Client.Helpers;
using TMW_Client.Models;
using TMW_Client.Services;
using TMW_Client.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TMW_Client
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static User user { get; set; }
        static LoginService loginService = new LoginService();

        public App()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //SetNewMainPage();
            user = loginService.UserLoggedIn();
            if (user != null)
            {
                IsUserLoggedIn = true;
            }
            InitializeComponent();
            SetMainPage();
            new Thread(delegate ()
            {
                InitializeDatabase();
            }).Start();
            sw.Stop();
            var time = sw.ElapsedMilliseconds;
        }

        public static void SetNewMainPage()
        {
            Current.MainPage = new LoginPage()
            {
                Title = "Login page"
            };
        }

        public static void SetMainPage()
        {
            if (user != null && user.UserID != 0)
            {
                Current.MainPage = new TabbedPage
                {
                    Children =
                    {
                        new NavigationPage(new ItemsPage())
                        {
                            Title = "All jokes",
                            Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                        },
                        new NavigationPage(new ItemsPage(user.UserID))
                        {
                            Title = "My jokes",
                            Icon = Device.OnPlatform<string>("tab_login.png", null, null)
                        },
                        new NavigationPage(new AboutPage())
                        {
                            Title = "About",
                            Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                        },
                    }
                };
            }
            else
            {
                Current.MainPage = new TabbedPage
                {
                    Children =
                    {
                        new NavigationPage(new ItemsPage())
                        {
                            Title = "All jokes",
                            Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                        },
                        new NavigationPage(new LoginPage())
                        {
                            Title = "Login",
                            Icon = Device.OnPlatform<string>("tab_login.png", null, null)
                        },
                        new NavigationPage(new AboutPage())
                        {
                            Title = "About",
                            Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                        }
                    }
                };
            }

        }

        public static void Logout()
        {
            App.IsUserLoggedIn = false;
            loginService.Logout();
        }

        public async static void InitializeDatabase()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var DBJoke = new DatastoreJoke();
                string URL = Utils.PublicURL + "/api/jokes";

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));
                var webClient = new WebClient();
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    var jokeList = JsonConvert.DeserializeObject<List<Joke>>(content);
                    DBJoke.AddAll(jokeList);
                }
            }
        }
    }
}
