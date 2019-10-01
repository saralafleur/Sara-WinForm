using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Sara.WinForm.ControlsNS
{
    public class FadeControl : UserControl
    {

        public FadeControl()
        {
            _pictureBox = new PictureBox();
            _pictureBox.BorderStyle = BorderStyle.None;
            _pictureBox.Paint += PictureBoxPaint;
            _fadeTimer = new Timer();
            _fadeTimer.Interval = 5000;
            _fadeTimer.Tick += fadeTimer_Tick;
        }

        public bool Faded
        {
            get { return _blend < 0.5f; }
        }
        public void FadeIn()
        {
            StopFade(false);
            CreateBitmaps();
            StartFade(1);
        }
        public void FadeOut(bool disposeWhenDone)
        {
            StopFade(false);
            CreateBitmaps();
            _disposeOnComplete = disposeWhenDone;
            StartFade(-1);
        }

        private void CreateBitmaps()
        {
            _bmpBack = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (var gr = Graphics.FromImage(_bmpBack)) gr.Clear(Parent.BackColor);
            _bmpFore = new Bitmap(_bmpBack.Width, _bmpBack.Height);
            DrawToBitmap(_bmpFore, ClientRectangle);
        }
        void fadeTimer_Tick(object sender, EventArgs e)
        {
            _blend += _blendDir * 0.02F;   // tweakable
            var done = false;
            if (_blend < 0) { done = true; _blend = 0; }
            if (_blend > 1) { done = true; _blend = 1; }
            if (done) StopFade(true);
            else _pictureBox.Invalidate();
        }
        void PictureBoxPaint(object sender, PaintEventArgs e)
        {
            var rc = new Rectangle(0, 0, _pictureBox.Width, _pictureBox.Height);
            var cm = new ColorMatrix();
            var ia = new ImageAttributes();
            cm.Matrix33 = _blend;
            ia.SetColorMatrix(cm);
            e.Graphics.DrawImage(_bmpFore, rc, 0, 0, _bmpFore.Width, _bmpFore.Height, GraphicsUnit.Pixel, ia);
            cm.Matrix33 = 1F - _blend;
            ia.SetColorMatrix(cm);
            e.Graphics.DrawImage(_bmpBack, rc, 0, 0, _bmpBack.Width, _bmpBack.Height, GraphicsUnit.Pixel, ia);
        }

        private void StopFade(bool complete)
        {
            _fadeTimer.Enabled = false;
            if (complete)
            {
                if (!Faded) Controls.Remove(_pictureBox);
                else if (_disposeOnComplete) Dispose();
            }
            if (_bmpBack != null) { _bmpBack.Dispose(); _bmpBack = null; }
            if (_bmpFore != null) { _bmpFore.Dispose(); _bmpFore = null; }
            Visible = false;
        }
        private void StartFade(int dir)
        {
            Controls.Add(_pictureBox);
            Controls.SetChildIndex(_pictureBox, 0);
            _blendDir = dir;
            _fadeTimer.Enabled = true;
            fadeTimer_Tick(this, EventArgs.Empty);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode) FadeIn();
        }
        protected override void OnResize(EventArgs args)
        {
            _pictureBox.Size = ClientSize;
            base.OnResize(args);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopFade(false);
                _pictureBox.Dispose();
                _fadeTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        private readonly PictureBox _pictureBox;
        private readonly Timer _fadeTimer;
        private Bitmap _bmpBack, _bmpFore;
        private float _blend;
        private int _blendDir = 1;
        private bool _disposeOnComplete;
    }
}