using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Sara.WinForm.CRUD;
using TextBox = System.Windows.Forms.TextBox;

namespace Sara.WinForm.Common

{
    public static class UICommon
    {
        /// <summary>
        /// Forces a ListBox to re-evaluate the ToString of it's Items
        /// </summary>
        public static void RefreshItemsToString(this ListBox lb)
        {
            lb.SuspendLayout();
            try
            {
                for (var i = 0; i < lb.Items.Count; i++)
                    lb.Items[i] = lb.Items[i];
            }
            finally
            {
                lb.ResumeLayout();
            }
        }
        public static Point GetLocation(Control c, int xoffset = 0, int yoffset = 0)
        {
            return c.PointToScreen(new Point(xoffset, yoffset));
        }

        public static bool ConfirmDelete(Control c, int yoffset = 0)
        {
            var window = new ConfirmDelete()
            {
                Location = GetLocation(c, yoffset)
            };
            return window.ShowDialog() == DialogResult.Yes;
        }
        /// <summary>
        /// Positions the cursor at the end of the Textbox
        /// </summary>
        /// <param name="item"></param>
        public static void GoToEnd(this TextBox item)
        {
            item.SelectionStart = item.Text.Length;
            item.SelectionLength = 0;
        }
        public static bool EditText(ref string value)
        {
            var window = new EditText()
            {
                Textvalue = value
            };

            if (window.ShowDialog() == DialogResult.OK)
            {
                value = window.Textvalue;
                return true;
            }

            value = null;
            return false;
        }


        /// <summary>
        ///  Add a highlight border to the control
        /// </summary>
        /// <param name="c"></param>
        public static void HighlightBorder(this Control c)
        {
            var bp = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Red,
                Padding = new Padding(1),
                Tag = "Highlight"
            };
            var bpInner = new Panel()
            {
                Dock = DockStyle.Fill
            };

            if (c is Panel pnl && pnl.AutoScroll)
            {
                bpInner.AutoScroll = true;
            }

            bp.Controls.Add(bpInner);

            while (c.Controls.Count > 0)
                c.Controls[0].Parent = bpInner;

            c.Controls.Add(bp);
            bp.SendToBack();
        }

        public const string SelectAll = "(Select All)";
        public static bool WhereCheck(CheckBox ckb, bool value)
        {
            return ckb.CheckState == CheckState.Indeterminate ||
                   value == ckb.Checked;
        }
        public static bool WhereText(TextBox tb, string text)
        {
            return tb.Text == string.Empty ||
                   text.Contains(tb.Text);
        }
        public static bool WhereComboBox(ComboBox cb, string value)
        {
            return cb.SelectedIndex == 0 ||
                   value.Contains((string)cb.SelectedItem);
        }
        public static bool WhereCheckListBox<T>(CheckedListBox clb, T value)
        {
            if ((string)clb.CheckedItems[0] == SelectAll)
                return true;

            if (value == null)
                return false;

            if (clb.CheckedItems.Count == 0)
                return false;

            return clb.CheckedItems.Cast<object>().Any(item => value.ToString() == (string) item);
        }
        public static bool WhereCheckListBox<T>(CheckedListBox clb, List<T> values)
        {
            if (clb.CheckedItems.Count == 0)
                return false;

            return (string) clb.CheckedItems[0] == SelectAll || (from object item in clb.CheckedItems
                       select values.Any(v => v.ToString() == (string) item)).All(
                       found => found);
        }
        /// <summary>
        /// Based on Google, there is a bug in nested controls and the use of 'DesignMode', in that it will alwasy return false.
        /// This is the common work around that was recommended, though not very pretty! - Sara
        /// </summary>
        /// <returns></returns>
        public static bool InDesignMode
        {
            get
            {
                try
                {
                    return System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv";
                }
                catch
                {
                    return false;
                }
            }
        }
        public static void SetDoubleBuffered(Control c)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            var aProp =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            if (aProp != null) aProp.SetValue(c, true, null);
        }
        public static void ApplyLabelStyle(TextBox control)
        {
            control.ReadOnly = true;
            control.BorderStyle = 0;
            control.BackColor = control.Parent.BackColor;
            control.TabStop = false;
        }
        public static void ConfirmDelete(Action action)
        {
            var confirmResult = MessageBox.Show(@"Are you sure you want to delete this item?", @"Confirm Delete!",
                                MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                action?.Invoke();
            }
        }
        public static void AutoColumnWidth(CheckedListBox clb)
        {
            var result = 0;
            foreach (var str in clb.Items)
            {
                var width = TextRenderer.MeasureText(str.ToString(), clb.Font).Width;
                if (width > result)
                    result = width;
            }
            clb.ColumnWidth = result + 20;
        }
        public static void ClearCheckListBox(CheckedListBox clb)
        {
            for (var i = 0; i < clb.Items.Count; i++)
            {
                clb.SetItemChecked(i, false);
            }
        }

        public static void AddWindowBorder(PaintEventArgs e, Form parent)
        {
            // Do nothing for now - Sara
            //parent.Padding = new Padding(1);
            //e.Graphics.DrawRectangle(System.Drawing.Pens.Red, new System.Drawing.Rectangle(0, 0, parent.Width-1, parent.Height-1));
        }
    }
}
