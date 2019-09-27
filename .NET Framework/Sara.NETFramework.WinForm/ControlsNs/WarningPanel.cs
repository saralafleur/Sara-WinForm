using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sara.NETFramework.WinForm.ControlsNS
{
    public partial class WarningPanel : UserControl
    {
        public WarningPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Used when showing and hiding the message
        /// </summary>
        [Browsable(true)]
        public Panel Content { get; set; }

        private string _warningText;
        [Browsable(true)]
        public string WarningText
        {
            get { return _warningText; }
            set
            {
                lblWarning.Text = value;
                _warningText = value;
            }
        }
        public void ShowWarning()
        {
            if (Content == null)
                throw new Exception("Warning Panel Content must never be null if you are calling this method!");

            Visible = true;
            Content.Visible = false;
            Dock = DockStyle.Fill;
        }
        public void HideWarning()
        {
            if (Content == null)
                throw new Exception("Warning Panel Content must never be null if you are calling this method!");

            Visible = false;
            Content.Visible = true;
        }
    }
}
