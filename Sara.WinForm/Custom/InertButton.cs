using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Sara.WinForm.Custom
{
    internal class InertButton : Button
    {
        private enum RepeatClickStatus
        {
            Disabled,
            Started,
            Repeating,
            Stopped
        }

        private class RepeatClickEventArgs : EventArgs
        {
            static RepeatClickEventArgs()
            {
                Empty = new RepeatClickEventArgs();
            }

            public new static RepeatClickEventArgs Empty { get; }
        }

        private readonly IContainer components = new Container();
        private int _mBorderWidth = 1;
        private bool _mMouseOver;
        private bool _mMouseCapture;
        private bool _mIsPopup;
        private Image _mImageEnabled;
        private Image _mImageDisabled;
        private int _mImageIndexEnabled = -1;
        private int _mImageIndexDisabled = -1;
        private bool _mMonochrome = true;
        private ToolTip _mToolTip;
        private string _mToolTipText = "";
        private Color _mBorderColor = Color.Empty;

        public InertButton()
        {
            InternalConstruct(null, null);
        }

        public InertButton(Image imageEnabled)
        {
            InternalConstruct(imageEnabled, null);
        }

        public InertButton(Image imageEnabled, Image imageDisabled)
        {
            InternalConstruct(imageEnabled, imageDisabled);
        }
        
        private void InternalConstruct(Image imageEnabled, Image imageDisabled)
        {
            // Remember parameters
            ImageEnabled = imageEnabled;
            ImageDisabled = imageDisabled;

            // Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Prevent base class from trying to generate double click events and
            // so testing clicks against the double click time and rectangle. Getting
            // rid of this allows the user to press then release button very quickly.
            //SetStyle(ControlStyles.StandardDoubleClick, false);

            // Should not be allowed to select this control
            SetStyle(ControlStyles.Selectable, false);

            Timer = new Timer();
            Timer.Enabled = false;
            Timer.Tick += Timer_Tick;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        public Color BorderColor
        {
            get    {    return _mBorderColor;    }
            set
            {
                if (_mBorderColor != value)
                {
                    _mBorderColor = value;
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializeBorderColor()
        {
            return (_mBorderColor != Color.Empty);
        }

        public int BorderWidth
        {
            get { return _mBorderWidth; }

            set
            {
                if (value < 1)
                    value = 1;
                if (_mBorderWidth != value)
                {
                    _mBorderWidth = value;
                    Invalidate();
                }
            }
        }

        public Image ImageEnabled
        {
            get
            { 
                if (_mImageEnabled != null)
                    return _mImageEnabled;

                try
                {
                    if (ImageList == null || ImageIndexEnabled == -1)
                        return null;
                    else
                        return ImageList.Images[_mImageIndexEnabled];
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                if (_mImageEnabled != value)
                {
                    _mImageEnabled = value;
                    Invalidate();
                }
            }
        }

        private bool ShouldSerializeImageEnabled()
        {
            return (_mImageEnabled != null);
        }

        public Image ImageDisabled
        {
            get
            {
                if (_mImageDisabled != null)
                    return _mImageDisabled;

                try
                {
                    if (ImageList == null || ImageIndexDisabled == -1)
                        return null;
                    else
                        return ImageList.Images[_mImageIndexDisabled];
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                if (_mImageDisabled != value)
                {
                    _mImageDisabled = value;
                    Invalidate();
                }
            }
        }

        public int ImageIndexEnabled
        {
            get    {    return _mImageIndexEnabled;    }
            set
            {
                if (_mImageIndexEnabled != value)
                {
                    _mImageIndexEnabled = value;
                    Invalidate();
                }
            }
        }

        public int ImageIndexDisabled
        {
            get    {    return _mImageIndexDisabled;    }
            set
            {
                if (_mImageIndexDisabled != value)
                {
                    _mImageIndexDisabled = value;
                    Invalidate();
                }
            }
        }

        public bool IsPopup
        {
            get { return _mIsPopup; }

            set
            {
                if (_mIsPopup != value)
                {
                    _mIsPopup = value;
                    Invalidate();
                }
            }
        }

        public bool Monochrome
        {
            get    {    return _mMonochrome;    }
            set
            {
                if (value != _mMonochrome)
                {
                    _mMonochrome = value;
                    Invalidate();
                }
            }
        }

        public bool RepeatClick
        {
            get => (ClickStatus != RepeatClickStatus.Disabled);
            set => ClickStatus = RepeatClickStatus.Stopped;
        }

        private RepeatClickStatus _mClickStatus = RepeatClickStatus.Disabled;
        private RepeatClickStatus ClickStatus
        {
            get => _mClickStatus;
            set
            {
                if (_mClickStatus == value)
                    return;

                _mClickStatus = value;
                if (ClickStatus == RepeatClickStatus.Started)
                {
                    Timer.Interval = RepeatClickDelay;
                    Timer.Enabled = true;
                }
                else if (ClickStatus == RepeatClickStatus.Repeating)
                    Timer.Interval = RepeatClickInterval;
                else
                    Timer.Enabled = false;
            }
        }

        public int RepeatClickDelay { get; set; } = 500;

        public int RepeatClickInterval { get; set; } = 100;

        private Timer Timer { get; set; }

        public string ToolTipText
        {
            get => _mToolTipText;
            set
            {
                if (_mToolTipText == value) return;
                if (_mToolTip == null)
                    _mToolTip = new ToolTip(components);
                _mToolTipText = value;
                _mToolTip.SetToolTip(this, value);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_mMouseCapture && _mMouseOver)
                OnClick(RepeatClickEventArgs.Empty);
            if (ClickStatus == RepeatClickStatus.Started)
                ClickStatus = RepeatClickStatus.Repeating;
        }

        /// <exclude/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (_mMouseCapture == false || _mMouseOver == false)
            {
                _mMouseCapture = true;
                _mMouseOver = true;

                //Redraw to show button state
                Invalidate();
            }

            if (RepeatClick)
            {
                OnClick(RepeatClickEventArgs.Empty);
                ClickStatus = RepeatClickStatus.Started;
            }
        }

        /// <exclude/>
        protected override void OnClick(EventArgs e)
        {
            if (RepeatClick && !(e is RepeatClickEventArgs))
                return;

            base.OnClick (e);
        }

        /// <exclude/>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (_mMouseOver || _mMouseCapture)
            {
                _mMouseOver = false;
                _mMouseCapture = false;

                // Redraw to show button state
                Invalidate();
            }

            if (RepeatClick)
                ClickStatus = RepeatClickStatus.Stopped;
        }

        /// <exclude/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Is mouse point inside our client rectangle
            var over = ClientRectangle.Contains(new Point(e.X, e.Y));

            // If entering the button area or leaving the button area...
            if (over != _mMouseOver)
            {
                // Update state
                _mMouseOver = over;

                // Redraw to show button state
                Invalidate();
            }
        }

        /// <exclude/>
        protected override void OnMouseEnter(EventArgs e)
        {
            // Update state to reflect mouse over the button area
            if (!_mMouseOver)
            {
                _mMouseOver = true;

                // Redraw to show button state
                Invalidate();
            }

            base.OnMouseEnter(e);
        }

        /// <exclude/>
        protected override void OnMouseLeave(EventArgs e)
        {
            // Update state to reflect mouse not over the button area
            if (_mMouseOver)
            {
                _mMouseOver = false;

                // Redraw to show button state
                Invalidate();
            }

            base.OnMouseLeave(e);
        }

        /// <exclude/>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBackground(e.Graphics);
            DrawImage(e.Graphics);
            DrawText(e.Graphics);
            DrawBorder(e.Graphics);
        }

        private void DrawBackground(Graphics g)
        {
            using (var brush = new SolidBrush(BackColor))
            {
                g.FillRectangle(brush, ClientRectangle);
            }
        }

        private void DrawImage(Graphics g)
        {
            var image = Enabled ? ImageEnabled : ((ImageDisabled != null) ? ImageDisabled : ImageEnabled);
            ImageAttributes imageAttr = null;

            if (null == image)
                return;

            if (_mMonochrome)
            {
                imageAttr = new ImageAttributes();

                // transform the monochrom image
                // white -> BackColor
                // black -> ForeColor
                var colorMap = new ColorMap[2];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = Color.White;
                colorMap[0].NewColor = BackColor;
                colorMap[1] = new ColorMap();
                colorMap[1].OldColor = Color.Black;
                colorMap[1].NewColor = ForeColor;
                imageAttr.SetRemapTable(colorMap);
            }

            var rect = new Rectangle(0, 0, image.Width, image.Height);

            if ((!Enabled) && (null == ImageDisabled))
            {
                using (var bitmapMono = new Bitmap(image, ClientRectangle.Size))
                {
                    if (imageAttr != null)
                    {
                        using (var gMono = Graphics.FromImage(bitmapMono))
                        {
                            gMono.DrawImage(image, new[] { new Point(0, 0), new Point(image.Width - 1, 0), new Point(0, image.Height - 1) }, rect, GraphicsUnit.Pixel, imageAttr);
                        }
                    }
                    ControlPaint.DrawImageDisabled(g, bitmapMono, 0, 0, BackColor);
                }
            }
            else
            {
                // Three points provided are upper-left, upper-right and 
                // lower-left of the destination parallelogram. 
                var pts = new Point[3];
                pts[0].X = (Enabled && _mMouseOver && _mMouseCapture) ? 1 : 0;
                pts[0].Y = (Enabled && _mMouseOver && _mMouseCapture) ? 1 : 0;
                pts[1].X = pts[0].X + ClientRectangle.Width;
                pts[1].Y = pts[0].Y;
                pts[2].X = pts[0].X;
                pts[2].Y = pts[1].Y + ClientRectangle.Height;

                if (imageAttr == null)
                    g.DrawImage(image, pts, rect, GraphicsUnit.Pixel);
                else
                    g.DrawImage(image, pts, rect, GraphicsUnit.Pixel, imageAttr);
            }
        }    

        private void DrawText(Graphics g)
        {
            if (Text == string.Empty)
                return;

            var rect = ClientRectangle;

            rect.X += BorderWidth;
            rect.Y += BorderWidth;
            rect.Width -= 2 * BorderWidth;
            rect.Height -= 2 * BorderWidth;

            var stringFormat = new StringFormat();

            if (TextAlign == ContentAlignment.TopLeft)
            {
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
            }
            else if (TextAlign == ContentAlignment.TopCenter)
            {
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Near;
            }
            else if (TextAlign == ContentAlignment.TopRight)
            {
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Near;
            }
            else if (TextAlign == ContentAlignment.MiddleLeft)
            {
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Center;
            }
            else if (TextAlign == ContentAlignment.MiddleCenter)
            {
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
            }
            else if (TextAlign == ContentAlignment.MiddleRight)
            {
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Center;
            }
            else if (TextAlign == ContentAlignment.BottomLeft)
            {
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Far;
            }
            else if (TextAlign == ContentAlignment.BottomCenter)
            {
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Far;
            }
            else if (TextAlign == ContentAlignment.BottomRight)
            {
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Far;
            }

            using (Brush brush = new SolidBrush(ForeColor))
            {
                g.DrawString(Text, Font, brush, rect, stringFormat);
            }
        }

        private void DrawBorder(Graphics g)
        {
            ButtonBorderStyle bs;

            // Decide on the type of border to draw around image
            if (!Enabled)
                bs = IsPopup ? ButtonBorderStyle.Outset : ButtonBorderStyle.Solid;
            else if (_mMouseOver && _mMouseCapture)
                bs = ButtonBorderStyle.Inset;
            else if (IsPopup || _mMouseOver)
                bs = ButtonBorderStyle.Outset;
            else
                bs = ButtonBorderStyle.Solid;

            Color colorLeftTop;
            Color colorRightBottom;
            if (bs == ButtonBorderStyle.Solid)
            {
                colorLeftTop = BackColor;
                colorRightBottom = BackColor;
            }
            else if (bs == ButtonBorderStyle.Outset)
            {
                colorLeftTop = _mBorderColor.IsEmpty ? BackColor : _mBorderColor;
                colorRightBottom = BackColor;
            }
            else
            {
                colorLeftTop = BackColor;
                colorRightBottom = _mBorderColor.IsEmpty ? BackColor : _mBorderColor;
            }
            ControlPaint.DrawBorder(g, ClientRectangle,
                colorLeftTop, _mBorderWidth, bs,
                colorLeftTop, _mBorderWidth, bs,
                colorRightBottom, _mBorderWidth, bs,
                colorRightBottom, _mBorderWidth, bs);
        }

        /// <exclude/>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (Enabled == false)
            {
                _mMouseOver = false;
                _mMouseCapture = false;
                if (RepeatClick && ClickStatus != RepeatClickStatus.Stopped)
                    ClickStatus = RepeatClickStatus.Stopped;
            }
            Invalidate();
        }
    }
}