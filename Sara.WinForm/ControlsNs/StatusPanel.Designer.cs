namespace Sara.WinForm.ControlsNS
{
    partial class StatusPanel
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
            this.components = new System.ComponentModel.Container();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblStatusDetail = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.RemaingTimer = new System.Windows.Forms.Timer(this.components);
            this.CountdownTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContent.Controls.Add(this.lblStatusDetail);
            this.pnlContent.Controls.Add(this.lblStatus);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(400, 100);
            this.pnlContent.TabIndex = 15;
            // 
            // lblStatusDetail
            // 
            this.lblStatusDetail.AutoEllipsis = true;
            this.lblStatusDetail.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblStatusDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusDetail.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatusDetail.Location = new System.Drawing.Point(0, 28);
            this.lblStatusDetail.Name = "lblStatusDetail";
            this.lblStatusDetail.Size = new System.Drawing.Size(400, 72);
            this.lblStatusDetail.TabIndex = 7;
            this.lblStatusDetail.Text = "Detail Message";
            this.lblStatusDetail.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(400, 28);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Main Message";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // RemaingTimer
            // 
            this.RemaingTimer.Interval = 1000;
            // 
            // CountdownTimer
            // 
            this.CountdownTimer.Interval = 1000;
            // 
            // StatusPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cyan;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlContent);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StatusPanel";
            this.Size = new System.Drawing.Size(400, 100);
            this.VisibleChanged += new System.EventHandler(this.StatusPanel_VisibleChanged);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Timer RemaingTimer;
        private System.Windows.Forms.Timer CountdownTimer;
        private System.Windows.Forms.Label lblStatusDetail;
        private System.Windows.Forms.Label lblStatus;
    }
}
