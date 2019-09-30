using System.Linq;
using System.Windows.Forms;

namespace Sara.WinForm
{
    public static class DataGridViewHelper
    {
        /// <summary>
        /// Sets all columns to AutoSize and the last column to Fill
        /// </summary>
        public static void AutoSizeGrid(DataGridView dgv)
        {
            for (var i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].AutoSizeMode = i == dgv.ColumnCount - 1 ? DataGridViewAutoSizeColumnMode.Fill : 
                                                                         DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        public static void AutoSizeGridCell(DataGridView dgv)
        {
            for (var i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        /// <summary>
        /// Allows for a User to click on a Column and sort it.
        /// </summary>
        /// <param name="dgv"></param>
        public static void MakeSortable(DataGridView dgv)
        {
            foreach (var dataGridViewColumn in dgv.Columns.Cast<DataGridViewColumn>()
                .Select(column => dgv.Columns[column.Name])
                .Where(dataGridViewColumn => dataGridViewColumn != null))
            {
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        public static void SimpleReadOnly(DataGridView dgv)
        {
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersVisible = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
        }
    }
}
