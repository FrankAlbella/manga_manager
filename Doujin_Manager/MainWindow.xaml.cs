using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Doujin_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void doujinPanel_Loaded(object sender, RoutedEventArgs e)
        {
            int count=0;
            DoujinScrubber ds = new DoujinScrubber();
            ds.SearchAll(this.doujinPanel, ref count);

            DoujinInfoViewModel viewModel = this.DataContext as DoujinInfoViewModel;
            viewModel.Count = "Count: " + count;

        }
    }
}