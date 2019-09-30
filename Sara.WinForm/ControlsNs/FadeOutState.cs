using System.Threading;

namespace Sara.WinForm.ControlsNs
{
    public class FadeOutState
    {
        public Timer FadeOutTimer;
        public FadeOutState()
        {
            Canceled = false;
        }
        public bool Canceled;
    }
}
