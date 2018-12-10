using Doujin_Manager.Controls;
using Doujin_Manager.Model;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class DoujinsViewModel : INotifyPropertyChanged
    {
        public SortModel SortModel { get; set; }

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

        private Visibility _searchBoxVisibility = Visibility.Collapsed;
        public Visibility SearchBoxVisibility
        {
            get { return _searchBoxVisibility; }
            set { this._searchBoxVisibility = value; NotifyPropertyChanged("SearchBoxVisibility"); }
        }

        public DoujinsViewModel()
        {
            _filteredDoujinsView = (ListCollectionView)DoujinsModel.FilteredDoujinsView;
            SortModel = new SortModel(ref _filteredDoujinsView);
        }

        public ICommand EditCommand
        { get { return new RelayCommand(param => EditDoujin());} }

        public ICommand OpenCommand
        { get { return new RelayCommand(param => OpenDoujinDirectory());} }

        public ICommand OpenBrowserCommand
        { get { return new RelayCommand(param => OpenDoujinInBrowser());} }

        public ICommand DeleteCommand
        { get { return new RelayCommand(param => DeleteDoujin());} }

        public ICommand ToggleSearchCommand
        { get { return new RelayCommand(param => ToggleSearchVisibility()); } }

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
            if (SelectedDoujin != null)
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
                DoujinsModel.Doujins.Remove(SelectedDoujin);
            }
        }

        private void ToggleSearchVisibility()
        {
            switch (SearchBoxVisibility)
            {
                case Visibility.Visible:
                    SearchBoxVisibility = Visibility.Collapsed;
                    break;
                case Visibility.Collapsed:
                    SearchBoxVisibility = Visibility.Visible;
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
