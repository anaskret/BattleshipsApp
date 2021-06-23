using Battleships.MobileApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Battleships.MobileApp.Models
{
    public class GridModel : ObservableProperty
    {
        public int X { get; set; }
        public int Y { get; set; }

        private bool _isShip;
        public bool IsShip{ get => _isShip; set => SetProperty(ref _isShip, value); }

        private GridStatusEnum _gridStatus = GridStatusEnum.NotHit;
        public GridStatusEnum GridStatus { get => _gridStatus; set => SetProperty(ref _gridStatus, value); }

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
