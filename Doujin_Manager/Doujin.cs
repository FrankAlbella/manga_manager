using System.Windows.Media.Imaging;

namespace Doujin_Manager
{
    public class Doujin
    {
        public BitmapImage CoverImage { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public TagList Tags { get; set; }
        public string Directory { get; set; }
    }
}
