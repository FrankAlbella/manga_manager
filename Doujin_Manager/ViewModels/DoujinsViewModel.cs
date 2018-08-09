using Doujin_Manager.Controls;
using System.Collections.ObjectModel;

namespace Doujin_Manager
{
    class DoujinsViewModel
    {
        public ObservableCollection<DoujinControl> Doujins { get; set; } = new ObservableCollection<DoujinControl>();
    }
}
