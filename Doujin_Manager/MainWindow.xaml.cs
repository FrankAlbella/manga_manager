using System.Windows;

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