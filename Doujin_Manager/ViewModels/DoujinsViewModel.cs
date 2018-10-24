using Doujin_Manager.Comparator;
using Doujin_Manager.Controls;
using Doujin_Manager.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class DoujinsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Doujin> Doujins { get; set; } = new ObservableCollection<Doujin>();

        public SortModel _sortModel;
        public SortModel SortModel
        {
            get { return this._sortModel; }
            set { this._sortModel = value; }
        }

        private Doujin _selectedDouin;
        public Doujin SelectedDoujin
        {
            get { return _selectedDouin; }
            set { this._selectedDouin = value; NotifyPropertyChanged("SelectedDoujin"); }
        }

        private ListCollectionView _filteredDoujinsView;
        public ICollectionView FilteredDoujinsView
        {
            get { return this._filteredDoujinsView; }
        }

        public DoujinsViewModel()
        {
            _filteredDoujinsView = new ListCollectionView(Doujins);
            SortModel = new SortModel(ref _filteredDoujinsView);
        }

        private string _textSearch;
        public string TextSearch
        {
            get { return _textSearch; }
            set
            {
                _textSearch = value;
                NotifyPropertyChanged("TextSearch");

                if (String.IsNullOrEmpty(value))
                    FilteredDoujinsView.Filter = null;
                else
                    FilteredDoujinsView.Filter = new Predicate<object>(o => 
                    {
                        Doujin doujin = ((Doujin)o);
                        return doujin.Tags.ToLower().Contains(value.ToLower())
                        || doujin.Title.ToLower().Contains(value.ToLower())
                        || doujin.Parodies.ToLower().Contains(value.ToLower())
                        || doujin.Characters.ToLower().Contains(value.ToLower());
                    });
            }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get { _editCommand = new RelayCommand(param => EditDoujin()); return _editCommand; }
        }

        private ICommand _openCommand;
        public ICommand OpenCommand
        {
            get { _openCommand = new RelayCommand(param => OpenDoujinDirectory()); return _openCommand; }
        }

        private ICommand _openBrowserCommand;
        public ICommand OpenBrowserCommand
        {
            get { _openBrowserCommand = new RelayCommand(param => OpenDoujinInBrowser()); return _openBrowserCommand; }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get { _deleteCommand = new RelayCommand(param => DeleteDoujin()); return _deleteCommand; }
        }

        private void EditDoujin()
        {
            EditWindow editWindow = new EditWindow(SelectedDoujin);
            editWindow.ShowDialog();
            editWindow.Activate();
            editWindow.Focus();
            editWindow.Topmost = true;
        }

        private void OpenDoujinDirectory()
        {
            SelectedDoujin.OpenDirectory();
        }

        private void OpenDoujinInBrowser()
        {
            string nHentaiUrl = @"https://nhentai.net/";

            if (SelectedDoujin.ID != "000000")
                nHentaiUrl += "g/" + SelectedDoujin.ID;

            System.Diagnostics.Process.Start(nHentaiUrl);
        }

        private void DeleteDoujin()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this doujin?\nThis will delete all subfolders and files",
                "Delete Confirmation",
                System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == System.Windows.MessageBoxResult.Yes)
            {
                Directory.Delete(SelectedDoujin.Directory, true);
                Doujins.Remove(SelectedDoujin);
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
