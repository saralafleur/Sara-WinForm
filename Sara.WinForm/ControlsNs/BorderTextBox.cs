using System;
using System.Drawing;
using System.Windows.Forms;
using Sara.WinForm.ColorScheme.ControlNS;

namespace Sara.WinForm.ControlsNs
{
    public sealed class BorderedTextBox : UserControl, IColorSchemeControl
    {
        public TextBox Item;

        public event EventHandler DoubleClickTextBox;

        public BorderedTextBox()
        {
            Item = new TextBox()
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true
            };
            Item.MouseEnter += TextBox_MouseEnter;
            Item.MouseLeave += TextBox_MouseLeave;
            Item.MouseDoubleClick += Item_MouseDoubleClick;
            Controls.Add(Item);

            BorderStyle = BorderStyle.FixedSingle;

            DefaultBorderColor = SystemColors.ControlDark;
            FocusedBorderColor = Color.Red;
            BackColor = DefaultBorderColor;
            Padding = new Padding(-10);
            Size = Item.Size;
        }

        private void Item_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DoubleClickTextBox?.Invoke(sender, e);
        }

        private void TextBox_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
            BackColor = DefaultBorderColor;
        }

        private void TextBox_MouseEnter(object sender, EventArgs e)
        {
            BackColor = FocusedBorderColor;
            OnMouseEnter(e);
        }

        public void ApplyColorScheme()
        {
            Item.BorderStyle = BorderStyle.None;
            BorderStyle = BorderStyle.None;
        }

        public Color DefaultBorderColor { get; set; }
        public Color FocusedBorderColor { get; set; }

        public override string Text
        {
            get { return Item.Text; }
            set { Item.Text = value; }
        }
    }
}