using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

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
            //Get Screen size
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            Console.WriteLine("Screen Width: " + screenWidth);
            Console.WriteLine("Screen Height: " + screenHeight);
            //open notepad
            Process notePadProcess = Process.Start("notepad", "readme.txt");
            Thread.Sleep(1000);
            // Find (the first-in-Z-order) Notepad window.
            IntPtr hWnd = FindWindow("Notepad", null);

            // If found, position it.
            if (hWnd != IntPtr.Zero)
            {
                int windowX = 0;
                int windowY = 0;
                int changeX = 1;
                int changeY = 1;
                int appWidth = 150;
                int appHeight = 150;
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, appWidth, appHeight, 0x0002 | 0x0004);
                Thread.Sleep(1000);
                while (true)
                {
                    SetWindowPos(hWnd, IntPtr.Zero, windowX, windowY, 0, 0, 0x0001 | 0x0004);
                    if(windowX+appWidth >= screenWidth)
                    {
                        changeX = -1;
                    }
                    if (windowX <= 0)
                    {
                        changeX = 1;
                    }
                    if (windowY + appHeight >= screenHeight)
                    {
                        changeY = -1;
                    }
                    if (windowY <= 0)
                    {
                        changeY = 1;
                    }
                    windowX += changeX;
                    windowY += changeY;
                    Console.WriteLine(windowX +" "+ windowY);
                    Thread.Sleep(25);
                }
            }
            else
            {
                Console.WriteLine("Error window not found");
            }

        }
    }
}
