﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sara.WinForm.ControlsNS
{
    public class BufferedTreeView : TreeView {
        protected override void OnHandleCreated(EventArgs e) {
            SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }
        // Pinvoke:
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        public void Invoke(Action method)
        {
            base.Invoke(method);
        }
    }
}