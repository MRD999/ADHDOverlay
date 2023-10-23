using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ADHDOverlay
{
    internal class Program
    {
        // P/Invoke declarations.

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        /* uFlags
         * 0x0001-Retains the current size (ignores the cx and cy parameters)
         * 0x0002-Retains the current position (ignores the X and Y parameters)
         * 0x0003-It retains both the current size and position of the window
         * 0x0004-Retains the current Z order (ignores the hWndInsertAfter parameter)
         */
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        //Get Screen size
        public static int screenWidth { get; set; }
        public static int screenHeight { get; set; }

        static void Main(string[] args)
        {
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            Console.WriteLine("Screen Width: " + screenWidth);
            Console.WriteLine("Screen Height: " + screenHeight);
           openWindows();
        }
        static void openWindows()
        {
            Random rnd = new Random();
            //OPening windows
            Process.Start("notepad", "readme.txt");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "https://bouncingdvdlogo.com");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "--new-window https://www.youtube.com/shorts/0t4ClCzI1sA");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "--new-window https://www.youtube.com/watch?v=dvjy6V4vLlI");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "--new-window https://optical.toys/spinning-duck/");
            Thread.Sleep(1000);
            // 
            IntPtr notePad = FindWindow("Notepad", null);
            IntPtr window0 = FindWindow("Chrome_WidgetWin_1", null);
            IntPtr window1 = FindWindowEx(IntPtr.Zero, window0, "Chrome_WidgetWin_1", null);
            IntPtr window2 = FindWindowEx(IntPtr.Zero, window1, "Chrome_WidgetWin_1", null);
            IntPtr window3 = FindWindowEx(IntPtr.Zero, window2, "Chrome_WidgetWin_1", null);
            if (window0 != IntPtr.Zero && window1 != IntPtr.Zero)
            {
                IntPtr[] windowCount = { notePad, window0, window1, window2, window3 };
                int[] windowX = { rnd.Next(screenWidth), rnd.Next(screenWidth), rnd.Next(screenWidth), rnd.Next(screenWidth), rnd.Next(screenWidth) };
                int[] windowY = { rnd.Next(screenHeight), rnd.Next(screenHeight), rnd.Next(screenHeight), rnd.Next(screenHeight), rnd.Next(screenHeight) };
                int[] changeX = { 1, 1, 0, 1, 0 };
                int[] changeY = { 1, 0, 1, 1, 1 };
                int[] appWidth = { 150,600, 70, 100, 100};
                int[] appHeight = { 150,600, 700, 600, 600 };
                //starting 
                int count = 0;
                foreach (var i in windowCount)
                {
                    Console.WriteLine(i);
                    Console.WriteLine(count);
                    Console.WriteLine(windowCount[count]);
                    SetWindowPos(i, IntPtr.Zero, 0, 0, appWidth[count], appHeight[count], 0x0002 | 0x0004);
                    SetWindowPos(i, new IntPtr(-1), 0, 0, 0, 0, 0x0003);
                    count++;
                }
                Thread.Sleep(1000);
                while (true)
                {
                    count = 0;
                    foreach (var i in windowCount)
                    {
                        // Get the rectangle of the current window
                        RECT rect;
                        GetWindowRect(i, out rect);
                        SetWindowPos(i, IntPtr.Zero, windowX[count], windowY[count], 0, 0, 0x0001 | 0x0004);
                        //check for hitting screen bounds
                        if (rect.right >= screenWidth)
                        {
                            changeX[count] = -1;
                        }
                        if (rect.left <= 0)
                        {
                            changeX[count] = 1;
                        }
                        if (rect.bottom >= screenHeight)
                        {
                            changeY[count] = -1;
                        }
                        if (rect.top <= 0)
                        {
                            changeY[count] = 1;
                        }
                        windowX[count] += changeX[count];
                        windowY[count] += changeY[count];
                        Debug.WriteLine("Webpage: " + i + " rect left: " + rect.left + " rect right: " + rect.right+ " rect top: " + rect.top+ " rect bottom: " + rect.bottom);
                        Debug.WriteLine("windowX: "+ windowX[count] +" windowY: "+ windowY[count]);
                        Debug.WriteLine("windowX: " + changeX[count] + " windowY: " + changeY[count]);
                        count++;
                    }
                    Thread.Sleep(2);
                }
            }
            else
            {
                Debug.WriteLine("Error window not found");
            }

        }

        public struct RECT
        {
            public int left;        // x position of upper-left corner
            public int top;         // y position of upper-left corner
            public int right;       // x position of lower-right corner
            public int bottom;      // y position of lower-right corner
        }
    }
}
