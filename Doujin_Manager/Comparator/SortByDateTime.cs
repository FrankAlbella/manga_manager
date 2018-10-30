using System;
using System.Collections;
using System.ComponentModel;

namespace Doujin_Manager.Comparator
{
    class SortByDateTime : IComparer
    {
        private readonly ListSortDirection sortDirection;

        public SortByDateTime(ListSortDirection sortDirection)
        {
            this.sortDirection = sortDirection;
        }

        public int Compare(object doujin1, object doujin2)
        {
            if ((Doujin)doujin1 == null && (Doujin)doujin2 == null)
                throw new ArgumentException("Only Doujin objects can be sorted by SortByDateAdded");

            if (this.sortDirection == ListSortDirection.Ascending)
                return DateTime.Compare(((Doujin)doujin1).DateAdded, ((Doujin)doujin2).DateAdded);
            else
                return DateTime.Compare(((Doujin)doujin2).DateAdded, ((Doujin)doujin1).DateAdded);
        }
    }
}
