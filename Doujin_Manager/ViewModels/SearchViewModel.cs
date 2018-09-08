using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {
        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
          get { return _visibility; }
          set { this._visibility = value; NotifyPropertyChanged("Visibility"); }
        }

        private ICommand _toggleSearchCommand;
        public ICommand ToggleSearchCommand
        {
            get { _toggleSearchCommand = new RelayCommand(param => ToggleSearchVisibility()); return _toggleSearchCommand; }
        }

        private void ToggleSearchVisibility()
        {
            switch (Visibility)
            {
                case Visibility.Visible:
                    Visibility = Visibility.Collapsed;
                    break;
                case Visibility.Collapsed:
                    Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
