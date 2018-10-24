using System;
using System.Collections;
using System.ComponentModel;

namespace Doujin_Manager.Comparator
{
    class SortByTitle : IComparer
    {
        private readonly ListSortDirection sortDirection;

        public SortByTitle(ListSortDirection sortDirection)
        {
            this.sortDirection = sortDirection;
        }

        public int Compare(object doujin1, object doujin2)
        {
            if ((Doujin)doujin1 == null && (Doujin)doujin2 == null)
                throw new ArgumentException("Only Doujin objects can be sorted by SortByTitle");

            if (this.sortDirection == ListSortDirection.Ascending)
                return string.Compare(((Doujin)doujin1).Title, ((Doujin)doujin2).Title);
            else
                return string.Compare(((Doujin)doujin2).Title, ((Doujin)doujin1).Title);
        }
    }
}
