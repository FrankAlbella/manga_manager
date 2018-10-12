using Doujin_Manager.Util;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
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

        public void CreateAndSetCoverImage(string coverImagePath, bool shouldCompress = false)
        {
            this.CoverImage = CreateBitmapImageFromPath(coverImagePath, shouldCompress);
        }

        private BitmapImage CreateBitmapImageFromPath(string path, bool shouldCompress = false)
        {
            if (path != null && !File.Exists(path.Replace("file:///", "")))
                path = DoujinScrubber.GetDefaultCoverPath(Directory);

            else if (path != null && shouldCompress)
                path = ImageCompressor.CompressImage(path, 40);

            if (path == null)
                return null;

            BitmapImage newCoverImage = new BitmapImage();

            newCoverImage.BeginInit();
            newCoverImage.UriSource = new Uri(path, UriKind.Absolute);
            newCoverImage.CacheOption = BitmapCacheOption.None;
            newCoverImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            newCoverImage.DecodePixelWidth = 140;
            newCoverImage.EndInit();

            newCoverImage.Freeze();

            return newCoverImage;
        }

        [JsonConstructor]
        public Doujin(string CoverImage, string Title, string Author, string Tags, string Directory, string ID)
        {
            this.Title = Title;
            this.Author = Author;
            this.Tags = Tags;
            this.Directory = Directory;
            this.CoverImage = CreateBitmapImageFromPath(CoverImage);
            this.ID = ID;
        }

        public Doujin() { }

        public void OpenDirectory()
        {
            if (System.IO.Directory.Exists(this.Directory))
                System.Diagnostics.Process.Start(this.Directory);
        }

        public override bool Equals(object obj)
        {
            if (obj is Doujin doujin)
                return doujin.Directory == this.Directory;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Directory.GetHashCode();
        }
    }
}
