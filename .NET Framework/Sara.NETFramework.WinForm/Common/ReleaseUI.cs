using System;
using System.Reflection;
using System.Windows.Forms;
using Sara.NETStandard.Common.Extension;
using Sara.NETStandard.Logging;

namespace Sara.NETFramework.WinForm.Common
{
    public static class ReleaseOutput
    {
        public static Action<string> Log { get; set; }
    }
    /// <summary>
    /// Release can be used on a long running process on the Main UI Thread.
    /// </summary>
    public class ReleaseUi
    {
        private int _iteration;
        private readonly string _name;
        private readonly DateTime _start;
        private DateTime _lastStart;
        private DateTime _lastStop;
        private readonly bool _skipLogging;
        public ReleaseUi(string name = "RELEASE", bool skipLogging = false)
        {
            _start = DateTime.Now;
            _lastStart = DateTime.Now;
            _name = name;
            _iteration = 0;
            _skipLogging = skipLogging;
        }
        public void DoEvents(string comment = null)
        {
            ProcessDoEvents(true, comment);
        }

        public void Stop()
        {
            ProcessDoEvents(false);
        }
        private void ProcessDoEvents(bool doEvent, string comment = null)
        {
            _lastStop = DateTime.Now;
            var duration = _lastStart.Difference(_lastStop);
            var fullDuration = _start.Difference(_lastStop);
            _lastStart = DateTime.Now;
            _iteration++;
            if (doEvent) Application.DoEvents();
            var m = doEvent ? "" : "Stop ";
            var c = comment == null ? "" : $" {comment}";
            var message = $"{_name} DoEvents({_iteration}{c}) {m}- {duration.ToShortReadableString()} | {fullDuration.ToShortReadableString()}";
            Log.Write(message, typeof(ReleaseUi).FullName, MethodBase.GetCurrentMethod().Name);
            if (!_skipLogging) ReleaseOutput.Log?.Invoke(message);
        }

    }
}
