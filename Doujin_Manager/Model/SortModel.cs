using Doujin_Manager.Comparator;
using Doujin_Manager.ViewModels;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Doujin_Manager.Model
{
    class SortModel
    {
        public ListSortDirection sortDirection;
        public ListCollectionView filteredDoujinsView;
        public ICommand selectedCommand;

        public SortModel(ref ListCollectionView listCollectionView)
        {
            sortDirection = Properties.Settings.Default.DoujinListboxSortOrder;
            selectedCommand = SortByTitleCommand;

            this.filteredDoujinsView = listCollectionView;
        }

        public ICommand SortByTitleCommand
        { get { return new RelayCommand(param => SortByTitle()); } }

        public ICommand SortByAuthorCommand
        { get { return new RelayCommand(param => SortByAuthor()); } }

        public ICommand SortByAscendingCommand
        { get { return new RelayCommand(param => ChangeSortDirection(ListSortDirection.Ascending)); } }

        public ICommand SortByDescendingCommand
        { get { return new RelayCommand(param => ChangeSortDirection(ListSortDirection.Descending)); } }

        private void SortByTitle()
        {
            filteredDoujinsView.CustomSort = new SortByTitle(this.sortDirection);
            selectedCommand = SortByTitleCommand;
        }

        private void SortByAuthor()
        {
            filteredDoujinsView.CustomSort = new SortByAuthor(this.sortDirection);
            selectedCommand = SortByAuthorCommand;
        }

        private void ChangeSortDirection(ListSortDirection direction)
        {
            if (direction == this.sortDirection)
                return;

            this.sortDirection = direction;

            Properties.Settings.Default.DoujinListboxSortOrder = direction;

            selectedCommand.Execute(null);
        }
    }
}
