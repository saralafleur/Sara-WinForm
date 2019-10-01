using System.Threading;

namespace Sara.WinForm.ControlsNS
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
