using System;
using System.Threading;
using System.Windows.Forms;

namespace Sara.NETFramework.WinForm.Threading
{
    public static class ThreadHelper
    {
        public static Thread GetControlOwnerThread(Control ctrl)
        {
            if (!ctrl.InvokeRequired) return Thread.CurrentThread;

            var o = ctrl.Invoke(
                new Action<Control>(control => GetControlOwnerThread(control)),
                new object[] { ctrl });
            return o as Thread;
        }
    }
}
