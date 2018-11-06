using Doujin_Manager.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {
        private SearchWindow searchWindow;

        private Visibility _visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
          get { return _visibility; }
          set { this._visibility = value; NotifyPropertyChanged("Visibility"); }
        }

        public ObservableCollection<string> TitleKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> AuthorKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> ParodyKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> CharacterKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> TagKeywords { get; set; } = new ObservableCollection<string>();

        public ICommand ToggleSearchCommand
        { get { return new RelayCommand(param => ToggleSearchVisibility()); } }

        public ICommand ShowAdvancedSearch
        { get { return new RelayCommand(param => ShowAdvancedSearchWindow()); } }

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
            }
        }

        private void ShowAdvancedSearchWindow()
        {
            if(searchWindow == null
                || !searchWindow.IsVisible)
                searchWindow = new SearchWindow();

            searchWindow.Show();
            searchWindow.Activate();
            searchWindow.Focus();
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
