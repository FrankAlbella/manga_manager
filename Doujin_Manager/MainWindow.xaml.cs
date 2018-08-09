using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

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
            DoujinScrubber ds = new DoujinScrubber();
            DoujinViewModel viewModel = this.DataContext as DoujinViewModel;

            Thread newThread = new Thread(() => ds.SearchAll(viewModel));
            newThread.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.DoujinDirectory == string.Empty)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Properties.Settings.Default.DoujinDirectory = fbd.SelectedPath;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }
    }
}