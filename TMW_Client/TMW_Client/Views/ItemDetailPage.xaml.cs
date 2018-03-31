
using TMW_Client.ViewModels;

using Xamarin.Forms;

namespace TMW_Client.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		ItemDetailViewModel viewModel;
        JokeDetailViewModel jokeModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public ItemDetailPage(JokeDetailViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = this.jokeModel = viewModel;
           
		}

	}
}
