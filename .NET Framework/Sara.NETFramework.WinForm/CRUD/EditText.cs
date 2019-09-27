using System.Windows.Forms;
using System;
using Sara.NETFramework.WinForm.ColorScheme.ControlNS;
using Sara.NETFramework.WinForm.ColorScheme.Modal;
using Sara.NETFramework.WinForm.Common;

namespace Sara.NETFramework.WinForm.CRUD
{
    public partial class EditText : Form, IColorSchemeControl
    {
        public EditText()
        {
            InitializeComponent();
            ColorService.Setup(this);
        }

        public string Textvalue
        {
            get => tbText.Text;
            set
            {
                tbText.Text = value;
                tbText.GoToEnd();
            }
        }

        public void ApplyColorScheme()
        {
            panel1.BackColor = ColorService.ColorScheme.Current.StatusBorderColor;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
