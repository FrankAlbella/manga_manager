using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Manager
{
    public class DoujinInfoViewModel : INotifyPropertyChanged
    {
        private string _title = "Title: ";
        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        private string _author = "Author: ";
        public string Author
        {
            get { return _author; }   
            set { _author = value; NotifyPropertyChanged("Author"); }
        }

        private string _tags = "Tags: ";
        public string Tags
        {
            get { return _tags; }
            set { _tags = value; NotifyPropertyChanged("Tags"); }
        }

        private string _count = "Count: ";
        public string Count
        {
            get { return _count; }
            set { _count = value; NotifyPropertyChanged("Count"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
