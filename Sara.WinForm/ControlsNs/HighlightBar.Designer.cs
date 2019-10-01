namespace Sara.WinForm.ControlsNS
{
    partial class HighlightBar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHighlight = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlHighlight
            // 
            this.pnlHighlight.BackColor = System.Drawing.Color.White;
            this.pnlHighlight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHighlight.Location = new System.Drawing.Point(1, 1);
            this.pnlHighlight.Name = "pnlHighlight";
            this.pnlHighlight.Size = new System.Drawing.Size(23, 23);
            this.pnlHighlight.TabIndex = 0;
            // 
            // HighlightBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.pnlHighlight);
            this.Name = "HighlightBar";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(25, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHighlight;
    }
}
