using System;
using System.ComponentModel;

namespace Doujin_Manager.ViewModels
{
    public class DoujinInfoViewModel : INotifyPropertyChanged
    {
        private string _title = "Title: ";
        public string Title
        {
            get { return _title; }
            set { _title = "Title: " + value; NotifyPropertyChanged("Title"); }
        }

        private string _author = "Author: ";
        public string Author
        {
            get { return _author; }   
            set { _author = "Author: " + value; NotifyPropertyChanged("Author"); }
        }

        private string _tags = "Tags: ";
        public string Tags
        {
            get { return _tags; }
            set { _tags = "Tags: " + value; NotifyPropertyChanged("Tags"); }
        }

        private string _count = "Count: ";
        public string Count
        {
            get { return _count; }
            set { _count = "Count: " + value; NotifyPropertyChanged("Count"); }
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
