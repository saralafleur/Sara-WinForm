using System.Windows.Forms;
using Sara.WinForm.ColorScheme;
using Sara.WinForm.ColorScheme.Modal;

namespace Sara.WinForm.ControlsNS
{
    public partial class TitleLine : UserControl, IColorSchemeControl
    {
        public TitleLine()
        {
            InitializeComponent();

            ColorService.Setup(this);
        }

        public string Title { get { return lblTitle.Text; } set { lblTitle.Text = value; } }

        public void ApplyColorScheme()
        {
            // When the color scheme changes the font, it will change the width of the line, which is a label!
            lblLine.Width = Width - 10;
            lblLine.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
