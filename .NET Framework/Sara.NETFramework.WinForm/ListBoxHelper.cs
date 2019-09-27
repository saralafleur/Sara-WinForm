using System.Windows.Forms;

namespace Sara.NETFramework.WinForm
{
    public static class ListBoxHelper
    {
        /// <summary>
        /// Forces the Listbox to re-evaluate the ToString of each Item
        /// </summary>
        /// <param name="listBox"></param>
        public static void UpdateToString(ListBox listBox)
        {
            var count = listBox.Items.Count;
            listBox.SuspendLayout();
            for (var i = 0; i < count; i++)
            {
                listBox.Items[i] = listBox.Items[i];
            }
            listBox.ResumeLayout();
        }
    }
}
