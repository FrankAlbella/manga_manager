using Doujin_Manager.Util;
using Microsoft.Win32;
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
            tBoxID.Text = this.doujin.ID;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(this.doujin.CoverImage.UriSource.AbsolutePath != tBoxCoverDir.Text
                && File.Exists(tBoxCoverDir.Text))
                this.doujin.CreateAndSetCoverImage(tBoxCoverDir.Text);
                
            if (this.doujin.ID != tBoxID.Text)
            {
                TagScrubber tagScrubber = new TagScrubber();
                tagScrubber.GatherDoujinDetails(tBoxID.Text, TagScrubber.SearchMode.ID);

                if (tagScrubber.HasValues)
                {
                    this.doujin.ID = tagScrubber.ID;
                    this.doujin.Title = tagScrubber.Title;
                    this.doujin.Author = tagScrubber.Author;
                    this.doujin.Tags = tagScrubber.Tags;
                }
            }
            else
            {
                this.doujin.Title = tBoxTitle.Text;
                this.doujin.Author = tBoxAuthor.Text;
                this.doujin.Tags = tBoxTags.Text;
            }
            this.Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
                tBoxCoverDir.Text = ofd.FileName;
        }

        private void tBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                btnSave_Click(this, e);
        }
    }
}
