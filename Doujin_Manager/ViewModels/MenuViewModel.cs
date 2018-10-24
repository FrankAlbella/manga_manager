using Doujin_Manager.Controls;
using System.Diagnostics;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class MenuViewModel
    {
        public ICommand OpenRepositoryCommand
        {
            get { return new RelayCommand(param => Process.Start("https://github.com/frankstar10/Doujin_Manager"));}
        }

        public ICommand AboutCommand
        {
            get { return new RelayCommand(param => OpenAboutWindow());}
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
