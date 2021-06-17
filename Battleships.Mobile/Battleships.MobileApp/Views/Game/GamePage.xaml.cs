using Battleships.MobileApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Battleships.MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            (BindingContext as ViewModelBase).InitializeAsync();

            base.OnAppearing();
        }
    }
}