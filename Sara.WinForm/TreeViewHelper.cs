using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sara.WinForm
{
    public static class TreeViewHelper
    {
        public static void Clone(this TreeView target, TreeView source)
        {
            target.SuspendLayout();
            target.BeginUpdate();
            try
            {
                target.Nodes.Clear();
                foreach (TreeNode node in source.Nodes)
                {
                    if (node.Clone() is TreeNode c) target.Nodes.Add(c);
                }
            }
            finally
            {
                target.EndUpdate();
                target.ResumeLayout();
            }
        }

        private const int WM_SCROLL = 276; // Horizontal scroll
        private const int WM_VSCROLL = 277; // Vertical scroll
        private const int SB_LINEUP = 0; // Scrolls one line up
        private const int SB_LINELEFT = 0;// Scrolls one cell left
        private const int SB_LINEDOWN = 1; // Scrolls one line down
        private const int SB_LINERIGHT = 1;// Scrolls one cell right
        private const int SB_PAGEUP = 2; // Scrolls one page up
        private const int SB_PAGELEFT = 2;// Scrolls one page left
        private const int SB_PAGEDOWN = 3; // Scrolls one page down
        private const int SB_PAGERIGTH = 3; // Scrolls one page right
        private const int SB_PAGETOP = 6; // Scrolls to the upper left
        private const int SB_LEFT = 6; // Scrolls to the left
        private const int SB_PAGEBOTTOM = 7; // Scrolls to the upper right
        private const int SB_RIGHT = 7; // Scrolls to the right
        private const int SB_ENDSCROLL = 8; // Ends scroll


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public static void ScrollDown(this TreeView source)
        {
            SendMessage(source.Handle, WM_VSCROLL, (IntPtr)SB_LINEDOWN, IntPtr.Zero);
        }

        public static void ScrollUp(this TreeView source)
        {
            SendMessage(source.Handle, WM_VSCROLL, (IntPtr)SB_LINEUP, IntPtr.Zero);
        }
    }
}