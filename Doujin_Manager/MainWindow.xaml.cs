using Doujin_Manager.Controls;
using System;
using System.IO;
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
        CentralViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void doujinPanel_Loaded(object sender, RoutedEventArgs e)
        {
            AsyncPopulateDoujinPanel();
        }

        private void AsyncPopulateDoujinPanel()
        {
            DoujinScrubber ds = new DoujinScrubber();

            Thread newThread = new Thread(() => ds.SearchAll(viewModel));
            newThread.Start();
        }

        private void ChooseDoujinRootDirection()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = this.DataContext as CentralViewModel;

            // If appdata folder doesn't exist, create it and the thumbnail folder
            if (!Directory.Exists(DirectoryInfo.appdataDir))
                Directory.CreateDirectory(DirectoryInfo.thumbnailDir);

            // Open folder select dialog if no folder location is saved
            if (!Directory.Exists(Properties.Settings.Default.DoujinDirectory))
            {
                ChooseDoujinRootDirection();
            }
        }

        private void btnChangeDirectory_Click(object sender, RoutedEventArgs e)
        {
            ChooseDoujinRootDirection();
            viewModel.DoujinsViewModel.Doujins.Clear();
            AsyncPopulateDoujinPanel();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DoujinsViewModel.Doujins.Clear();
            AsyncPopulateDoujinPanel();
        }

        // TODO: causes an exception because images are not released after list is cleared
        private void btnCache_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < viewModel.DoujinsViewModel.Doujins.Count; i++)
            {
                viewModel.DoujinsViewModel.Doujins[i] = null;
            }

            viewModel.DoujinsViewModel.Doujins.Clear();
            GC.Collect();
            string[] thumbsnails = Directory.GetFiles(DirectoryInfo.thumbnailDir);

            foreach (string thumbnail in thumbsnails)
            {
                File.Delete(thumbnail);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}