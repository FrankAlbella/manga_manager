using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Doujin_Manager.Model
{
    static class DoujinsModel
    {
        public static ObservableCollection<Doujin> Doujins { get; set; } = new ObservableCollection<Doujin>();

        private static ListCollectionView _filteredDoujinsView = new ListCollectionView(Doujins);
        public static ICollectionView FilteredDoujinsView
        {
            get { return _filteredDoujinsView; }
        }
    }
}
