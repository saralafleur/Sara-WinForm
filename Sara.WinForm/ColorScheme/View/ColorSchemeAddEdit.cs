using System;
using System.Windows.Forms;
using Sara.WinForm.ColorScheme.Modal;

namespace Sara.WinForm.ColorScheme.View
{
    public partial class ColorSchemeAddEdit : Form
    {
        public ColorSchemeAddEdit()
        {
            InitializeComponent();
        }

        public string ColorSchemeName { get { return tbColorScheme.Text; } set { tbColorScheme.Text = value; } }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ColorSchemeAddEdit_Shown(object sender, EventArgs e)
        {
            ColorService.Setup(this);
        }
    }
}
