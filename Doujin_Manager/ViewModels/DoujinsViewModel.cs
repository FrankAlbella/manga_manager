using Doujin_Manager.Controls;
using System.Collections.ObjectModel;

namespace Doujin_Manager
{
    class DoujinsViewModel
    {
        public ObservableCollection<Doujin> Doujins { get; set; } = new ObservableCollection<Doujin>();
    }
}
