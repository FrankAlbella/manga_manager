using Doujin_Manager.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class DoujinsViewModel
    {
        public ObservableCollection<Doujin> Doujins { get; set; } = new ObservableCollection<Doujin>();
        public Doujin SelectedDoujin { get; set; }

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
    }
}
