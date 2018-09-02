using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Doujin_Manager.Controls
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        Doujin doujin;
        public EditWindow(Doujin doujin)
        {
            InitializeComponent();

            this.doujin = doujin;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            tBoxCoverDir.Text = this.doujin.CoverImage.UriSource.AbsolutePath;
            tBoxTitle.Text = this.doujin.Title;
            tBoxAuthor.Text = this.doujin.Author;
            tBoxTags.Text = this.doujin.Tags;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(File.Exists(tBoxCoverDir.Text))
                this.doujin.CoverImage = new BitmapImage(new Uri(tBoxCoverDir.Text));
            this.doujin.Title = tBoxTitle.Text;
            this.doujin.Author = tBoxAuthor.Text;
            this.doujin.Tags = tBoxTags.Text;

            this.Close();
        }
    }
}
