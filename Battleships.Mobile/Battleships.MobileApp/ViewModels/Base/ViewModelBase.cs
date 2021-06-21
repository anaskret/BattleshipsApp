using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.ViewModels.Base
{
    public class ViewModelBase : BaseViewModel
    {
        public virtual async void InitializeAsync()
        {
            await Task.FromResult(false);
        }
    }
}
