using Doujin_Manager.Controls;
using Doujin_Manager.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {
        private SearchWindow searchWindow;

        public ObservableCollection<string> TitleKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> AuthorKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> ParodyKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> CharacterKeywords { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> TagKeywords { get; set; } = new ObservableCollection<string>();

        public ICommand ShowAdvancedSearch
        { get { return new RelayCommand(param => ShowAdvancedSearchWindow()); } }

        public ICommand ApplyAdvancedSearchCommand
        { get { return new RelayCommand(param => ApplyAdvancedSearch()); } }

        public ICommand ClearKeywordsCommand
        { get { return new RelayCommand(param => ClearKeywords()); } }

        private string _textSearch;
        public string TextSearch
        {
            get { return _textSearch; }
            set
            {
                _textSearch = value;
                NotifyPropertyChanged("TextSearch");

                if (String.IsNullOrEmpty(value))
                    DoujinsModel.FilteredDoujinsView.Filter = null;
                else
                    DoujinsModel.FilteredDoujinsView.Filter = new Predicate<object>(o =>
                    {
                        Doujin doujin = ((Doujin)o);
                        return doujin.Tags.ToLower().Contains(value.ToLower())
                            || doujin.Title.ToLower().Contains(value.ToLower())
                            || doujin.Parodies.ToLower().Contains(value.ToLower())
                            || doujin.Characters.ToLower().Contains(value.ToLower());
                    });
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

        private void ApplyAdvancedSearch()
        {
            DoujinsModel.FilteredDoujinsView.Filter = new Predicate<object>(o =>
            {
                Doujin doujin = ((Doujin)o);

                return TitleKeywords.All(doujin.Title.ToLower().Contains)
                    && AuthorKeywords.All(doujin.Author.ToLower().Contains)
                    && ParodyKeywords.All(doujin.Parodies.ToLower().Contains)
                    && CharacterKeywords.All(doujin.Characters.ToLower().Contains)
                    && TagKeywords.All(doujin.Tags.ToLower().Contains);
            });
        }

        private void ClearKeywords()
        {
            TitleKeywords.Clear();
            AuthorKeywords.Clear();
            ParodyKeywords.Clear();
            CharacterKeywords.Clear();
            TagKeywords.Clear();
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
