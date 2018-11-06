using Doujin_Manager.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Doujin_Manager.Controls
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        SearchViewModel dataContext;

        Key[] newTagKeys = new Key[] { Key.Enter };

        public SearchWindow()
        {
            InitializeComponent();

            dataContext = (SearchViewModel)this.DataContext;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (newTagKeys.Contains(e.Key))
            {
                TextBox textBox = (TextBox)sender;
                string newKeyword = textBox.Text.Trim().ToLower();

                ListView relatedListView = FindRelatedListView(textBox);
                ObservableCollection<string> Keywords = GetListViewItemSource(relatedListView);

                if (!Keywords.Contains(newKeyword)
                    && !string.IsNullOrWhiteSpace(newKeyword))
                    Keywords.Add(newKeyword);

                textBox.Text = "";
            }
        }

        private ObservableCollection<string> GetListViewItemSource(ListView listView)
        {
            return (ObservableCollection<string>)listView.ItemsSource;
        }

        private ListView FindRelatedListView(TextBox textBox)
        {
            string listViewName = textBox.Name.Replace("TextBox", "ListView");

            return (ListView)this.FindName(listViewName);
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem button = (ListViewItem)sender;
            ListView parent = (ListView)ItemsControl.ItemsControlFromItemContainer(button);
            ObservableCollection<string> keywords = (ObservableCollection<string>)parent.ItemsSource;

            string keyword = button.Content.ToString();

            keywords.Remove(keyword);
        }
    }
}
