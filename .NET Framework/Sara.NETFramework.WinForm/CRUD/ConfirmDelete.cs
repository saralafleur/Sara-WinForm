using System.Windows.Forms;
using Sara.NETFramework.WinForm.ColorScheme.ControlNS;
using Sara.NETFramework.WinForm.ColorScheme.Modal;

namespace Sara.NETFramework.WinForm.CRUD
{
    public partial class ConfirmDelete : Form, IColorSchemeControl
    {
        public ConfirmDelete()
        {
            InitializeComponent();

            ColorService.Setup(this);
        }
        public void ApplyColorScheme()
        {
            panel1.BackColor = ColorService.ColorScheme.Current.StatusBorderColor;
        }

        private void btnYes_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}
