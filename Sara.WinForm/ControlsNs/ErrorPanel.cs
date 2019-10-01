using System;
using System.Windows.Forms;

namespace Sara.WinForm.ControlsNS
{
    public partial class ErrorPanel : UserControl
    {
        private Action CloseCallback { get; set; }
        public static void ShowError(Control parent, string error, Action closeCallback)
        {
            var panel = new ErrorPanel
            {
                CloseCallback = closeCallback,
                Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right))),
                tbError =
                    {
                        Text = error

                    },
                Dock = DockStyle.Fill
            };
            parent.Controls.Add(panel);
            panel.BringToFront();
        }
        public ErrorPanel()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Visible = false;
            Parent.Controls.Remove(this);
            // At this point there should be no reference to this control and it should be freed by the garbage collector. - Sara

            if (CloseCallback != null)
                CloseCallback();
        }
    }
}
