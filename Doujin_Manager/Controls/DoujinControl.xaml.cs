using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Doujin_Manager.Controls
{
    /// <summary>
    /// Interaction logic for DoujinControl.xaml
    /// </summary>
    public partial class DoujinControl : UserControl
    {
        Doujin doujin;
        public DoujinControl(Doujin doujin)
        {
            InitializeComponent();

            this.CoverImage.Source = doujin.CoverImage;
            this.doujin = doujin;
        }

        private void CoverImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DoujinInfoViewModel viewModel = this.DataContext as DoujinInfoViewModel;
            viewModel.Author = "Author: " + this.doujin.Author;
            viewModel.Title = "Title: " + this.doujin.Title;
            viewModel.Tags = "Tags: idk probably some furry shit";
        }

        private void OpenDirectoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(System.IO.Directory.Exists(this.doujin.Directory))
                System.Diagnostics.Process.Start("explorer.exe", this.doujin.Directory);
        }

        private void EditDoujinMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow(ref this.doujin);
            editWindow.Show();
        }
    }
}
