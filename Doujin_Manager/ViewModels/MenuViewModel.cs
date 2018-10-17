using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Doujin_Manager.ViewModels
{
    class MenuViewModel : INotifyPropertyChanged 
    {
        private ICommand _openDirectoryCommand;
        public ICommand OpenDirectoryCommand
        {
            get { _openDirectoryCommand = new RelayCommand(param => Process.Start("https://github.com/frankstar10/Doujin_Manager")); return _openDirectoryCommand; }
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
