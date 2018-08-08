using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
