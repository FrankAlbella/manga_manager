using Doujin_Manager.Controls;
using System.Diagnostics;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class MenuViewModel
    {
        private ICommand _openRepositoryCommand;
        public ICommand OpenRepositoryCommand
        {
            get { _openRepositoryCommand = new RelayCommand(param => Process.Start("https://github.com/frankstar10/Doujin_Manager")); return _openRepositoryCommand; }
        }

        private ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get { _aboutCommand = new RelayCommand(param => OpenAboutWindow()); return _aboutCommand; }
        }

        private void OpenAboutWindow()
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
            aboutWindow.Activate();
            aboutWindow.Focus();
            aboutWindow.Topmost = true;
        }
    }
}
