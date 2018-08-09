using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
        [STAThread]
        private void doujinPanel_Loaded(object sender, RoutedEventArgs e)
        {
            DoujinScrubber ds = new DoujinScrubber();
            DoujinViewModel viewModel = this.DataContext as DoujinViewModel;

            SynchronizationContext uiContext = SynchronizationContext.Current;

            Thread newThread = new Thread(() => ds.SearchAll(uiContext, viewModel));
            newThread.Start();
        }
    }
}