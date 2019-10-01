using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Sara.Common.Extension;
using Sara.Logging;
using Sara.WinForm.ColorScheme;
using Sara.WinForm.ColorScheme.Modal;
using Sara.WinForm.Notification;
using Timer = System.Threading.Timer;

namespace Sara.WinForm.ControlsNS
{
    public partial class StatusPanel : UserControl, IColorSchemeControl
    {
        #region Public Properties
        [Description("When True the Status Panel will calculate the Time Remaining based on Current and Total."), Category("StatusPanel")]
        public bool SP_DisplayRemainingTime { get; set; }
        /// <summary>
        /// When True the Control will become the Top Control and Dock will be set to Fill
        /// </summary>
        [Description("When True the Status Panel will be Full Screen."), Category("StatusPanel")]
        private bool _fullscreen;
        public bool SP_FullScreen
        {
            get => _fullscreen;
            set
            {
                _fullscreen = value;
                _hasInitialize = false;
                Initialize();
            }
        }
        /// <summary>
        /// The Default Size is used to ensure each Status Panel is the same size.
        /// </summary>
        [Description("When True the Status Panel will be set to a Default Size."), Category("StatusPanel")]
        public bool SP_DefaultStatusSize { get; set; }
        [Description("When False the Status Panel will not display anything on the screen.  Used when an update takes less then 250ms."), Category("StatusPanel")]
        public bool SP_Enabled { get; set; }

        public void ApplyColorScheme()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ApplyColorScheme));
                return;
            }

            var cs = ColorService.ColorScheme.Current;

            BackColor = cs.StatusBorderColor;
            Padding = new Padding(cs.StatusBorderSize);
            lblStatus.BackColor = cs.StatusBackColor;
            lblStatus.ForeColor = cs.StatusForeColor;
            pnlContent.BackColor = cs.StatusBackColor;
            lblStatusDetail.BackColor = cs.StatusBackColor;
            lblStatusDetail.ForeColor = cs.StatusForeColor;
        }
        #endregion

        #region Private Properties
        private int BackGroundThreads { get; set; }
        private int Total { get; set; }
        private int Current { get; set; }
        private DateTime? Started { get; set; }
        private TimeSpan TimeRemaining { get; set; }
        private FadeOutState _fadeOutState;
        private string _lastStatus = string.Empty;
        private string _detailMessage;
        private string _persistentDetail = string.Empty;
        private int DefaultHeight { get; set; }
        private int DefaultWidth { get; set; }
        private string StopWatchValue { get; set; }
        private string CountdownValue { get; set; }
        private bool _hasInitialize;
        #endregion

        #region Setup
        public StatusPanel()
        {
            InitializeComponent();
            Started = null;
            SP_DisplayRemainingTime = false;
            _fadeOutState = new FadeOutState { Canceled = true };
            Visible = false;
            SP_DefaultStatusSize = true;
            SP_FullScreen = true; // Default
            BackGroundThreads = 0;
            SP_Enabled = true;

            if (!DesignMode)
                ColorService.Setup(this);
        }
        /// <summary>
        /// Prepare the Status Panel for display
        /// Centers the Status Panel if the Dock is None
        /// </summary>
        private void Initialize()
        {
            if (_hasInitialize)
                return;

            if (DesignMode)
                return;

            if (TopLevelControl == null)
                return;

            Parent.SuspendLayout();
            try
            {
                DefaultWidth = 400;
                DefaultHeight = 100;

                Parent.Resize -= ParentResize;
                Parent.Resize += ParentResize;

                Dock = _fullscreen ? DockStyle.Fill : Dock = DockStyle.None;

                if (Dock != DockStyle.Fill)
                    BringToFront();

                AdjustTextSize();

                _hasInitialize = true;
            }
            finally
            {
                Parent.ResumeLayout();
            }
        }
        private void AdjustTextSize()
        {
            var extra = (lblStatusDetail.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Count() - 1) * lblStatusDetail.Font.Height;

            lblStatusDetail.Height = (pnlContent.Height / 2) + extra / 2;

            if (Dock != DockStyle.None) return;

            Width = (Parent.Width < DefaultWidth) ? Parent.Width - 10 : DefaultWidth;
            Height = (Parent.Height < DefaultHeight) ? Parent.Height - 2 : DefaultHeight;
            Left = (Parent.Width - Width) / 2;
            Top = (Parent.Height - Height) / 2;

        }
        #endregion

        #region Events
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible && !Disposing) Initialize();
        }
        public void StatusUpdate(IStatusModel model)
        {
            if (!SP_Enabled)
                return;

            try
            {
                if (IsDisposed) return;
                // I was receiving an 'Cannot access a disposed object.' error.  When I looked at the parent of the Status Panel, I noticed that it was not 'Created' yet.  
                // Thus I have added the following check to avoid this error. - Sara
                if (!Parent.Created) return;
                if (InvokeRequired)
                {
                    if (IsDisposed) return;
                    Invoke(new Action<IStatusModel>(StatusUpdate), model);
                    return;
                }

                switch (model.Action)
                {
                    case StatusAction.ClearPersistentDetail:
                        _persistentDetail = string.Empty;
                        return;
                    case StatusAction.AddPersistentDetail:
                        _persistentDetail += model.StatusDetail + Environment.NewLine;
                        return;
                    case StatusAction.ShowEstimatedTime:
                        SP_DisplayRemainingTime = true;
                        return;
                    case StatusAction.HideEstimatedTime:
                        SP_DisplayRemainingTime = false;
                        SetStatusDetail("");
                        return;
                    case StatusAction.StartStopWatch:
                        RemaingTimer.Enabled = true;
                        StopWatchValue = "";
                        return;
                    case StatusAction.StopStopWatch:
                        StopStopWatch();
                        return;

                    #region Countdown
                    case StatusAction.StartCountdown:
                        CountdownTimer.Enabled = true;
                        StopWatchValue = "";
                        return;
                    case StatusAction.StopCountdown:
                        StopCountdown();
                        return;
                    #endregion Countdown

                    case StatusAction.Updating:
                        // If a prior timer is running then Cancel it so that we don't hide the StatusPanel - Sara
                        lock (this)
                        {
                            if (_fadeOutState.Canceled == false)
                                _fadeOutState.Canceled = true;
                        }
                        Visible = true;
                        break;
                    case StatusAction.FadeOutUpdate:
                        // If a prior timer is running then Cancel it so that we don't hide the StatusPanel - Sara
                        lock (this)
                        {
                            if (_fadeOutState.Canceled == false)
                                _fadeOutState.Canceled = true;
                        }
                        SetStatusDetail("");
                        Visible = true;
                        model.Action = StatusAction.Updating;
                        FadeOut(1000);
                        break;
                    case StatusAction.FullScreenOn:
                        SP_FullScreen = true;
                        return;
                    case StatusAction.FullScreenOff:
                        SP_FullScreen = false;
                        return;
                    case StatusAction.Complete:
                        if (BackGroundThreads != 0)
                            return;
                        StopStopWatch();
                        SetStatusDetail("Complete");
                        FadeOut(0);
                        return;
                    case StatusAction.StartBackground:
                        BackGroundThreads++;
                        //Log.Write(typeof(StatusPanel).FullName, MethodBase.GetCurrentMethod().Name, LogEntryType.Warning, "++BackGroundThreads=={0}", BackGroundThreads);
                        return;
                    case StatusAction.EndBackground:
                        BackGroundThreads--;
                        //Log.Write(typeof(StatusPanel).FullName, MethodBase.GetCurrentMethod().Name, LogEntryType.Warning, "--BackGroundThreads=={0}", BackGroundThreads);
                        if (BackGroundThreads == 0)
                        {
                            BackGroundThreads = 0;
                            //Log.Write(typeof(StatusPanel).FullName, MethodBase.GetCurrentMethod().Name, LogEntryType.Warning, "BackGroundThreads-- below 0!");
                        }
                        return;
                    case StatusAction.Error:

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                UpdateTimeRemaining(model);

                if (model.Status != null)
                {
                    SetStatus(model.Status);
                }

                if (model.StatusDetail == null && !SP_DisplayRemainingTime) return;

                if (model.StatusDetail == null)
                    model.StatusDetail = string.Empty;


                _detailMessage = _persistentDetail == string.Empty ? model.StatusDetail : _persistentDetail + "-----" + Environment.NewLine + model.StatusDetail;
                if (SP_DisplayRemainingTime)
                    _detailMessage += Environment.NewLine + string.Format("Estimated Time Remaining {0}", TimeRemaining.ToReadableString());

                SetStatusDetail(_detailMessage);

                Refresh();
            }
            catch (Exception ex)
            {
                Log.WriteError(typeof(StatusPanel).FullName, MethodBase.GetCurrentMethod().Name,ex);
            }
        }
        private void ParentResize(object sender, EventArgs e)
        {
            AdjustTextSize();
        }
        public void DoAction(string status, Action action)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                StatusUpdate(StatusModel.Update(status));
                action?.Invoke();
            }
            finally
            {
                StatusUpdate(StatusModel.Completed);
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method will force the Window Control to Double Buffer so that flicker is reduced...
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        private void FadeOut(int delay)
        {
            lock (this)
            {
                lblStatus.Text = @"...";
                _fadeOutState = new FadeOutState();
                _fadeOutState.FadeOutTimer = new Timer(FadeOutComplete, _fadeOutState, delay, Timeout.Infinite);
            }
        }
        public void FadeOutComplete(Object o)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object>(FadeOutComplete), o);
                return;
            }

            lock (this)
            {
                var state = o as FadeOutState;
                if (state == null)
                    throw new Exception("State must be of type FadeOutState!");

                state.FadeOutTimer.Dispose();

                if (state.Canceled)
                    return;

                Visible = false;
            }
        }
        private void StopStopWatch()
        {
            RemaingTimer.Enabled = false;
            StopWatchValue = "";
        }
        private void StopCountdown()
        {
            CountdownTimer.Enabled = false;
            CountdownValue = "";
        }
        private void SetStatus(string status)
        {
            // Prevent the display from updating with the same status, causes flickers - Sara
            if (status == _lastStatus)
            {
                _lastStatus = status;
                return;
            }
            lblStatus.Text = status;
            AdjustTextSize();
        }
        private void SetStatusDetail(string value)
        {
            value = $"{value}{StopWatchValue}{CountdownValue}";

            if (value == lblStatusDetail.Text)
                return;

            lblStatusDetail.Text = value;
            if ((value != string.Empty) != lblStatusDetail.Visible)
                AdjustTextSize();
            lblStatusDetail.Visible = (value != string.Empty);
            if (value == string.Empty)
                lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            else
                lblStatus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
        }
        private void UpdateTimeRemaining(IStatusModel model)
        {
            try
            {
                if (model.Total > 0)
                    Total = model.Total;
                if (model.Current > 0)
                    Current = model.Current;

                switch (model.Action)
                {
                    case StatusAction.Updating:
                        if (Started == null)
                            Started = DateTime.Now;
                        break;
                    case StatusAction.Complete:
                        Started = null;
                        return;
                    case StatusAction.StartBackground:
                    case StatusAction.EndBackground:
                    case StatusAction.Error:
                        // Do Nothing
                        return;
                    default:
                        throw new ArgumentOutOfRangeException($"Action: {model.Action} is undefined");
                }
                if (Total == 0)
                    return;

                var timeTaken = DateTime.Now - Started;
                var itemsLeft = Total - Current;
                if (itemsLeft <= 0)
                {
                    TimeRemaining = new TimeSpan(0);
                    return;
                }
                var secondsRemaining = Current == 0 ? 0 : (timeTaken.Value.TotalSeconds / Current) * itemsLeft;
                TimeRemaining = new TimeSpan(0, 0, Convert.ToInt32(secondsRemaining));
            }
            catch (Exception ex)
            {
                Log.WriteError(typeof(StatusPanel).FullName, MethodBase.GetCurrentMethod().Name, ex);
                // Do nothing on an error in calculating Time Remaining
            }
        }
        #endregion
        private bool _shown;
        private void StatusPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!_shown)
            {
                _shown = true;
                ApplyColorScheme();
            }
            // We are getting many UI elements that are not repaiting at times.  To avoid this I'm going to repaint the parent when the status panel is hidden. - Sara
            if (!Visible)
            {
                if (Parent != null)
                    Parent.Refresh();
            }
        }
    }

}
