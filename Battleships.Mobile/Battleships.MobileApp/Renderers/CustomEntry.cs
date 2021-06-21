using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Battleships.MobileApp.Renderers
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty IsPasswordFlagProperty =
        BindableProperty.Create("IsPasswordFlag", typeof(bool), typeof(CustomEntry), defaultBindingMode: BindingMode.OneWay);
        public bool IsPasswordFlag
        {
            get { return (bool)GetValue(IsPasswordFlagProperty); }
            set { SetValue(IsPasswordFlagProperty, value); }
        }
    }
}
