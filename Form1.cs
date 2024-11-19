using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FindGamleFiler
{
    public partial class Form1 : Form
    {
        private SortOrder sortOrder = SortOrder.Ascending;

        public Form1()
        {
            InitializeComponent();
            lstFiles.ColumnClick += new ColumnClickEventHandler(lstFiles_ColumnClick);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFolderPath.Text) || !DateTime.TryParse(txtDate.Text, out DateTime givenDate))
            {
                MessageBox.Show("Indtast venligst en gyldig mappe sti og dato.");
                return;
            }

            var files = Directory.GetFiles(txtFolderPath.Text, "*.*", SearchOption.AllDirectories)
                                 .Where(file => File.GetLastWriteTime(file) < givenDate)
                                 .Select(file => new { Path = file, Date = File.GetLastWriteTime(file) })
                                 .ToList();

            lstFiles.Items.Clear();
            foreach (var file in files)
            {
                var listViewItem = new ListViewItem(new[] { file.Path, file.Date.ToString() });
                listViewItem.Checked = true;
                lstFiles.Items.Add(listViewItem);
            }

            if (files.Count == 0)
            {
                MessageBox.Show("Ingen filer fundet ældre end den angivne dato.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstFiles.CheckedItems)
            {
                string filePath = item.SubItems[0].Text;
                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                        lstFiles.Items.Remove(item);
                        MessageBox.Show($"Slettet: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fejl ved sletning af {filePath}: {ex.Message}");
                    }
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstFiles.Items)
            {
                item.Checked = true;
            }
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstFiles.Items)
            {
                item.Checked = false;
            }
        }

        private void lstFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lstFiles.ListViewItemSorter is ListViewItemComparer sorter && sorter.Column == e.Column)
            {
                sortOrder = sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                sortOrder = SortOrder.Ascending;
            }

            lstFiles.ListViewItemSorter = new ListViewItemComparer(e.Column, sortOrder);
        }
    }
}
