using Doujin_Manager.Model;
using Doujin_Manager.Util;
using Doujin_Manager.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Doujin_Manager
{
    public partial class MainWindow : Window
    {
        DoujinsViewModel dataContext;
        Thread populateThread;

        public MainWindow()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine(System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        }

        private void AsyncPopulateDoujinPanel()
        {
            DoujinScrubber ds = new DoujinScrubber();

            if (populateThread != null && populateThread.IsAlive)
                populateThread.Abort();

            populateThread = new Thread(() => ds.PopulateDoujins());
            populateThread.Name = "DoujinScrubber Thread";
            populateThread.IsBackground = true;
            populateThread.Start();
        }

        private void ChooseDoujinRootDirection()
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    SettingsUtil.DoujinDirectory = fbd.SelectedPath;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataContext = (DoujinsViewModel)this.DataContext;

            // If appdata folder doesn't exist, create it and the thumbnail folder
            if (!Directory.Exists(PathUtil.appdataDir))
                Directory.CreateDirectory(PathUtil.thumbnailDir);

            // Open folder select dialog if no folder location is saved
            if (!Directory.Exists(SettingsUtil.DoujinDirectory))
                ChooseDoujinRootDirection();
        }

        private void doujinListView_Loaded(object sender, RoutedEventArgs e)
        {
            AsyncPopulateDoujinPanel();
        }

        private void btnChangeDirectory_Click(object sender, RoutedEventArgs e)
        {
            ChooseDoujinRootDirection();
            DoujinsModel.Doujins.Clear();
            AsyncPopulateDoujinPanel();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DoujinsModel.Doujins.Clear();
            AsyncPopulateDoujinPanel();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cache cache = new Cache();
            cache.Save(new List<Doujin>(DoujinsModel.Doujins));
        }

        private void searchBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((TextBox)sender).Visibility == Visibility.Visible)
                ((TextBox)sender).Focus();
        }

        private void doujinListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(dataContext.SelectedDoujin != null)
                dataContext.SelectedDoujin.OpenDirectory();
        }

        private void SortByMenuItems_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                RadioButton radioButton = (RadioButton)menuItem.Icon;
                if (radioButton != null)
                {
                    // Check if menuitem that fired event is a Ascending or Descending button
                    // if it isn't, it will uncheck all other buttons before checking the sender
                    // if it is, then it unchecks the Ascending and Descending buttons before checking the sender
                    if (menuItem != (MenuItem)SortMenu.Items.GetItemAt(SortMenu.Items.Count-1)
                        && menuItem != SortMenu.Items.GetItemAt(SortMenu.Items.Count - 2))
                        SortMenu.Items.OfType<MenuItem>().Take(3).ToList().ForEach(x => ((RadioButton)x.Icon).IsChecked = false);
                    else
                        SortMenu.Items.OfType<MenuItem>().Skip(3).Take(2).ToList().ForEach(x => ((RadioButton)x.Icon).IsChecked = false);

                    radioButton.IsChecked = true;
                }
                    
            }
        }
    }
}