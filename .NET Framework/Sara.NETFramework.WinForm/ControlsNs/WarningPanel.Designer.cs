namespace Sara.NETFramework.WinForm.ControlsNS
{
    partial class WarningPanel
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlWarning = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlWarning.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Transparent;
            this.pnlTop.Controls.Add(this.pnlWarning);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(10);
            this.pnlTop.Size = new System.Drawing.Size(363, 297);
            this.pnlTop.TabIndex = 14;
            // 
            // pnlWarning
            // 
            this.pnlWarning.BackColor = System.Drawing.Color.Black;
            this.pnlWarning.Controls.Add(this.lblWarning);
            this.pnlWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWarning.Location = new System.Drawing.Point(10, 10);
            this.pnlWarning.Name = "pnlWarning";
            this.pnlWarning.Padding = new System.Windows.Forms.Padding(1);
            this.pnlWarning.Size = new System.Drawing.Size(343, 277);
            this.pnlWarning.TabIndex = 11;
            // 
            // lblWarning
            // 
            this.lblWarning.AutoEllipsis = true;
            this.lblWarning.BackColor = System.Drawing.Color.Khaki;
            this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblWarning.ForeColor = System.Drawing.Color.DimGray;
            this.lblWarning.Location = new System.Drawing.Point(1, 1);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(341, 275);
            this.lblWarning.TabIndex = 6;
            this.lblWarning.Text = "Test Message...";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WarningPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pnlTop);
            this.Name = "WarningPanel";
            this.Size = new System.Drawing.Size(363, 297);
            this.pnlTop.ResumeLayout(false);
            this.pnlWarning.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlWarning;
        private System.Windows.Forms.Label lblWarning;
    }
}
