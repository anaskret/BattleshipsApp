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
    public partial class PopupInfo : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopupInfo()
        {
            InitializeComponent();
        }
    }
}