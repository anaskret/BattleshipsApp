using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Battleships.MobileApp.Models
{
    public class TileModel : ObservableProperty
    {
        public int X { get; set; }
        public int Y { get; set; }

        private bool _isShip;
        public bool IsShip{ get => _isShip; set => SetProperty(ref _isShip, value); }

        private bool _isShot = false;
        public bool IsShot { get => _isShot; set => SetProperty(ref _isShot, value); }

        private bool _isSunk = false;
        public bool IsSunk { get => _isSunk; set => SetProperty(ref _isSunk, value); }

        public ShipModel Ship { get; set; }
    }

    public class ObservableProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
