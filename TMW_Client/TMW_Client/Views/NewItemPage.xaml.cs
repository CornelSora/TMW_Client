using System;

using TMW_Client.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMW_Client.Views
{
	public partial class NewItemPage : ContentPage
	{
		public Item Item { get; set; }
        public Joke Joke { get; set; }

        public NewItemPage()
		{
			InitializeComponent();

            //Item = new Item
            //{
            //	Text = "Item name",
            //	Description = "This is a nice description"
            //};

            Joke = new Joke
            {
                Text = ""
            };

			BindingContext = this;
		}

		async void Save_Clicked(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "AddJoke", Joke);
			await Navigation.PopToRootAsync();
		}
	}
}