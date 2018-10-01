using Doujin_Manager.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class DoujinsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Doujin> Doujins { get; set; } = new ObservableCollection<Doujin>();
        
        private Doujin _selectedDouin;
        public Doujin SelectedDoujin
        {
            get { return _selectedDouin; }
            set { this._selectedDouin = value; NotifyPropertyChanged("SelectedDoujin"); }
        }

        public DoujinsViewModel()
        {
            _filteredDoujinsView = new ListCollectionView(Doujins);
        }

        private ListCollectionView _filteredDoujinsView;
        public ICollectionView FilteredDoujinsView
        {
            get { return this._filteredDoujinsView; }
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
                    FilteredDoujinsView.Filter = new Predicate<object>(o => ((Doujin)o).Tags.Contains(value));
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

        private void EditDoujin()
        {
            EditWindow editWindow = new EditWindow(SelectedDoujin);
            editWindow.Show();
        }

        private void OpenDoujinDirectory()
        {
            if (System.IO.Directory.Exists(SelectedDoujin.Directory))
                System.Diagnostics.Process.Start("explorer.exe", SelectedDoujin.Directory);
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
