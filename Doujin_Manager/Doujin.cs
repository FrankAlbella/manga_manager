using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Doujin_Manager
{
    public class Doujin : INotifyPropertyChanged
    {
        private BitmapImage _coverImage;
        public BitmapImage CoverImage
        {
            get { return _coverImage; }
            set { this._coverImage = value; NotifyPropertyChanged("CoverImage"); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { this._title = value; NotifyPropertyChanged("Title"); }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set { this._author = value; NotifyPropertyChanged("Author"); }
        }

        private string _tags;
        public string Tags
        {
            get { return _tags; }
            set { this._tags = value; NotifyPropertyChanged("Tags"); }
        }

        public string Directory { get; set; }

        public string ID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void CreateAndSetCoverImage(string coverImagePath)
        {
            BitmapImage newCoverImage = new BitmapImage();

            newCoverImage.BeginInit();
                newCoverImage.UriSource = new Uri(coverImagePath, UriKind.Absolute);
                newCoverImage.CacheOption = BitmapCacheOption.None;
                newCoverImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                newCoverImage.DecodePixelWidth = 140;
            newCoverImage.EndInit();

            newCoverImage.Freeze();

            this.CoverImage = newCoverImage;
        }
    }
}
