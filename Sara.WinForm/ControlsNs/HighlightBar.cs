using System.Drawing;
using System.Windows.Forms;

namespace Sara.NETFramework.WinForm.ControlsNS
{
    public partial class HighlightBar : UserControl
    {
        public HighlightBar()
        {
            InitializeComponent();
        }
        public Color HighlightColor
        {
            get => pnlHighlight.BackColor;
            set => pnlHighlight.BackColor = value;
        }
    }
}
