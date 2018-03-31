using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMW_Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginMaster : ContentPage
    {
        public ListView ListView;

        public LoginMaster()
        {
            InitializeComponent();

            BindingContext = new LoginMasterViewModel();
            ListView = MenuItemsListView;
        }

        class LoginMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<LoginMenuItem> MenuItems { get; set; }
            
            public LoginMasterViewModel()
            {
                MenuItems = new ObservableCollection<LoginMenuItem>(new[]
                {
                    new LoginMenuItem { Id = 0, Title = "Page 1" },
                    new LoginMenuItem { Id = 1, Title = "Page 2" },
                    new LoginMenuItem { Id = 2, Title = "Page 3" },
                    new LoginMenuItem { Id = 3, Title = "Page 4" },
                    new LoginMenuItem { Id = 4, Title = "Page 5" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}