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
    public partial class LoginPage : ContentPage
    {
        public User User { get; set; }
        public LoginViewModel LoginViewModel { get; set; }

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = this.LoginViewModel = new LoginViewModel();
        }

        async Task OnLoginButtonClicked(object sender, EventArgs e)
        {
            //MessagingCenter.Send(this, "Login", LoginViewModel.User);
            var _user = LoginViewModel.User as User;
            LoginService LoginService = new LoginService();
            var id = await LoginService.LoginAsync(_user);
            if (id != 0 && id != -2)
            {
                LoginMethod(_user, id);
            }
            else
            {
                if (id == -2)
                {
                    errorLabel.Text = "You don't have an internet connection";
                }
                else
                {
                    errorLabel.Text = "Login not successful";
                }
            }
        }

        protected void Register_Clicked(object sender, EventArgs e)
        {
            LoginLayout.IsVisible = false;
            RegisterLayout.IsVisible = true;
            Login.Text = "Login";
            Register.Text = "";
        }
        protected void Login_Clicked(object sender, EventArgs e)
        {
            LoginLayout.IsVisible = true;
            RegisterLayout.IsVisible = false;
            Login.Text = "";
            Register.Text = "Sign up";
        }

        protected async void LoginMethod(User _user, int id)
        {
            App.IsUserLoggedIn = true;
            _user.UserID = id;
            App.user = _user;
            var test = App.Current.MainPage as TabbedPage;
            test.Children[1].Title = "My jokes";
            Navigation.InsertPageBefore(new ItemsPage(_user.UserID), this);
            await Navigation.PopAsync();
        }

        async Task OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var _user = LoginViewModel.User as User;
            LoginService LoginService = new LoginService();
            if (_user.Username == null || _user.Password == null ||
                _user.Username == "" || _user.Password == "")
            {
                errorLabel.Text = "You must enter the username and the password";
            }

            if (_user.Password.Length < 4 || _user.Username.Length < 3)
            {
                errorLabel.Text = "The password must have at least 4 elements";
            }
            var id = await LoginService.RegisterAsync(_user);

            if (id != 0 && id != -2)
            {
                LoginMethod(_user, id);
            }
            else
            {
                if (id == -2)
                {
                    errorLabel.Text = "You don't have an internet connection";
                }
                else
                {
                    errorLabel.Text = "The user already exists.";
                }
            }
        }

        private void usernameEntry_TextChanged(object sender, EventArgs e)
        {
            var _userDB = new DatastoreUser();
            var username = usernameEntry.Text;
            if (username.Length > 3)
                passwordEntry.Text = _userDB.GetPassword(username);
        }
    }
}