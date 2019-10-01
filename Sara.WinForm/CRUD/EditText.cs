using System.Windows.Forms;
using System;
using Sara.WinForm.ColorScheme;
using Sara.WinForm.ColorScheme.Modal;
using Sara.WinForm.Common;

namespace Sara.WinForm.CRUD
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
