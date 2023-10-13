using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace ADHDOverlay
{
    internal class Program
    {
        // P/Invoke declarations.

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static void Main(string[] args)
        {
            //Process.Start("notepad", "readme.txt");
            Thread.Sleep(6000);
            // Find (the first-in-Z-order) Notepad window.
            IntPtr hWnd = FindWindow("Notepad", null);

            // If found, position it.
            if (hWnd != IntPtr.Zero)
            {
                // Move the window to (0,0) without changing its size or position
                // in the Z order.
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, 0x0001 | 0x0004);
            }

        }
    }
}
