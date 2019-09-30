using System;
using System.Drawing;
using System.Windows.Forms;
using Sara.WinForm.ColorScheme.ControlNS;

namespace Sara.WinForm.ControlsNs
{
    public class BorderPanel : Panel, IColorSchemeControl
    {

        public event EventHandler BorderDoubleClick;

        public BorderPanel()
        {
            InitializeComponent();

            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(1);
        }

        private bool _shown;
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (_shown) return;

            _shown = true;
            ApplyMouse(this);
        }

        private void ApplyMouse(Control obj)
        {
            obj.MouseEnter += Border_MouseEnter;
            obj.MouseLeave += Border_MouseLeave;

            foreach (var item in obj.Controls)
            {
                if (item is Control c)
                {
                    c.MouseEnter += Border_MouseEnter;
                    c.MouseLeave += Border_MouseLeave;
                    c.MouseDoubleClick += Item_MouseDoubleClick;
                    ApplyMouse(c);
                }
            }
        }

        private void Item_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BorderDoubleClick?.Invoke(sender, e);
        }

        private void Border_MouseLeave(object sender, EventArgs e)
        {
            var g = CreateGraphics();
            ControlPaint.DrawBorder(g, ClientRectangle, BackColor, ButtonBorderStyle.Solid);
        }

        private void Border_MouseEnter(object sender, EventArgs e)
        {
            var g = CreateGraphics();
            ControlPaint.DrawBorder(g, ClientRectangle, FocusedBorderColor, ButtonBorderStyle.Solid);
        }

        public void ApplyColorScheme()
        {
            BorderStyle = BorderStyle.None;
        }

        public Color FocusedBorderColor { get; set; }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // BorderPanel
            // 
            Margin = new Padding(10);
            Name = "BorderPanel";
            Padding = new Padding(10);
            ResumeLayout(false);
        }
    }
}