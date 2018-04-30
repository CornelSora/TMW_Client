using TMW_Client.Helpers;
using TMW_Client.Models;
using TMW_Client.Services;

using Xamarin.Forms;

namespace TMW_Client.ViewModels
{
	public class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Get the azure service instance
		/// </summary>
        public IDataStore<Joke> JokeInitializer => DependencyService.Get<IDataStore<Joke>>();
        public IAccount<User> LoginService => DependencyService.Get<IAccount<User>>();

        bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set { SetProperty(ref isBusy, value); }
		}
		/// <summary>
		/// Private backing field to hold the title
		/// </summary>
		string title = string.Empty;
		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}
	}
}

