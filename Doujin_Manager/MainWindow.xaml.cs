using Doujin_Manager.Controls;
using Doujin_Manager.ViewModels;
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
        CentralViewModel dataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AsyncPopulateDoujinPanel()
        {
            DoujinScrubber ds = new DoujinScrubber();

            Thread newThread = new Thread(() => ds.SearchAll(dataContext));
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
            dataContext = this.DataContext as CentralViewModel;

            // If appdata folder doesn't exist, create it and the thumbnail folder
            if (!Directory.Exists(DirectoryInfo.appdataDir))
                Directory.CreateDirectory(DirectoryInfo.thumbnailDir);

            // Open folder select dialog if no folder location is saved
            if (!Directory.Exists(Properties.Settings.Default.DoujinDirectory))
            {
                ChooseDoujinRootDirection();
            }
        }

        private void doujinListView_Loaded(object sender, RoutedEventArgs e)
        {
            AsyncPopulateDoujinPanel();
        }

        private void btnChangeDirectory_Click(object sender, RoutedEventArgs e)
        {
            ChooseDoujinRootDirection();
            dataContext.DoujinsViewModel.Doujins.Clear();
            AsyncPopulateDoujinPanel();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dataContext.DoujinsViewModel.Doujins.Clear();
            AsyncPopulateDoujinPanel();
        }

        // TODO: causes an exception because images are not released after list is cleared
        private void btnCache_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dataContext.DoujinsViewModel.Doujins.Count; i++)
            {
                dataContext.DoujinsViewModel.Doujins[i] = null;
            }

            dataContext.DoujinsViewModel.Doujins.Clear();
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