using System;
using System.Collections.Generic;
using System.Text;
using TMW_Client.Helpers;
using TMW_Client.Models;
using TMW_Client.Views;
using Xamarin.Forms;

namespace TMW_Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ObservableRangeCollection<User> Users { get; set; }
        public Command LoginCommand { get; set; }
        public User User { get; set; }

        public LoginViewModel()
        {
            Title = "Browse";
            //LoginCommand = new Command(async () => await ExecuteLoadItemsCommand());
            User = new User()
            {
                Password = "",
                Username = ""
            };

            MessagingCenter.Subscribe<LoginPage, User>(this, "Login", async (obj, user) =>
            {
                var _user = user as User;
                var id = await LoginService.LoginAsync(_user);
                if (id != 0)
                {
                    
                }
                else
                {
                    
                }
            });
        }
    }
}
