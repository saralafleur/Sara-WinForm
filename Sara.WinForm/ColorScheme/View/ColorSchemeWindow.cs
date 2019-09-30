using System;
using System.Windows.Forms;
using Sara.WinForm.ColorScheme.Modal;
using Sara.WinForm.ColorScheme.ViewModel;
using Sara.WinForm.Notification;

namespace Sara.NETFramework.WinForm.ColorScheme.View
{
    public partial class ColorSchemeWindow : Form
    {
        public ColorSchemeWindow()
        {
            InitializeComponent();
        }

        private void ColorSchemeWindow_Load(object sender, EventArgs e)
        {
            Render();
        }

        public void Render()
        {
            var model = ColorSchemeViewModel.GetModel();

            lbColorScheme.Items.Clear();
            foreach (var item in model.Collection)
                lbColorScheme.Items.Add(item);
            if (lbColorScheme.Items.Count > 0)
            {
                lbColorScheme.SelectedIndex = 0;
                pgColorScheme.SelectedObject = Convert.ChangeType(lbColorScheme.SelectedItem, ColorService.ColorSchemeType);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ColorSchemeViewModel.Add();
            DialogResult = DialogResult.None;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            StatusPanel.StatusUpdate(StatusModel.Update("Saving"));
            ColorSchemeViewModel.Save();
            StatusPanel.StatusUpdate(StatusModel.Completed);
            Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ColorSchemeViewModel.Edit(lbColorScheme.SelectedItem);
            DialogResult = DialogResult.None;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ColorSchemeViewModel.Delete(lbColorScheme.SelectedItem);
        }

        private void lbColorScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbColorScheme.SelectedItem == null)
                return;
            pgColorScheme.SelectedObject = lbColorScheme.SelectedItem;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (pgColorScheme.SelectedObject == null)
                MessageBox.Show("You must select a Scheme to Reset.");

            if (pgColorScheme.SelectedObject is ColorSchemeBaseModal cs)
            {
                cs.SetDarkDefault();
                pgColorScheme.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pgColorScheme.SelectedObject == null)
                MessageBox.Show("You must select a Scheme to Reset.");

            if (pgColorScheme.SelectedObject is ColorSchemeBaseModal cs)
            {
                cs.SetLightDefault();
                pgColorScheme.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorService.ResetAll();
            Render();
        }

        private void ColorSchemeWindow_Shown(object sender, EventArgs e)
        {
            ColorService.Setup(this);
        }
    }
}
