using System;
using System.Collections;
using System.ComponentModel;

namespace Doujin_Manager.Comparator
{
    class SortByAuthor : IComparer
    {
        private readonly ListSortDirection sortDirection;

        public SortByAuthor(ListSortDirection sortDirection)
        {
            this.sortDirection = sortDirection;
        }

        public int Compare(object doujin1, object doujin2)
        {
            if ((Doujin)doujin1 == null && (Doujin)doujin2 == null)
                throw new ArgumentException("Only Doujin objects can be sorted by SortByAuthor");

            if (this.sortDirection == ListSortDirection.Ascending)
                return string.Compare(((Doujin)doujin1).Author, ((Doujin)doujin2).Author);
            else
                return string.Compare(((Doujin)doujin2).Author, ((Doujin)doujin1).Author);
        }
    }
}
