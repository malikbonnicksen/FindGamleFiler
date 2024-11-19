using System;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;

namespace FindGamleFiler
{
    public class ListViewItemComparer : IComparer
    {
        private int col;
        private SortOrder order;

        public ListViewItemComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }

        public ListViewItemComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }

        public int Compare(object x, object y)
        {
            int returnVal = -1;
            ListViewItem itemX = (ListViewItem)x;
            ListViewItem itemY = (ListViewItem)y;

            if (col == 1) // Assuming the date is in the second column (index 1)
            {
                DateTime dateX = DateTime.Parse(itemX.SubItems[col].Text, CultureInfo.GetCultureInfo("da-DK"));
                DateTime dateY = DateTime.Parse(itemY.SubItems[col].Text, CultureInfo.GetCultureInfo("da-DK"));
                returnVal = DateTime.Compare(dateX, dateY);
            }
            else
            {
                returnVal = String.Compare(itemX.SubItems[col].Text, itemY.SubItems[col].Text);
            }

            if (order == SortOrder.Descending)
                returnVal *= -1;

            return returnVal;
        }

        public int Column
        {
            get { return col; }
        }
    }
}
