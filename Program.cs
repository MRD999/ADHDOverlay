using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace ADHDOverlay
{
    internal class Program
    {
        // P/Invoke declarations.
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
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
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);
        //Get Screen size
        public static int screenWidth { get; set; }
        public static int screenHeight { get; set; }

        static void Main(string[] args)
        {
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            Console.WriteLine("Screen Width: " + screenWidth);
            Console.WriteLine("Screen Height: " + screenHeight);
           WebPages();
        }
        static void WebPages()
        {
            Random rnd = new Random();
            //OPening windows
            //Process.Start("notepad", "readme.txt");
            //Thread.Sleep(1000);
            Process.Start("chrome.exe", "https://bouncingdvdlogo.com");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "--new-window https://puginarug.com");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "--new-window https://floatingqrcode.com");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "--new-window https://optical.toys/spinning-duck/");
            Thread.Sleep(1000);
            // 
            // Enumerate all Chrome windows
            List<IntPtr> chromeWindows = new List<IntPtr>();
            EnumWindows((hWnd, lParam) =>
            {
                StringBuilder sb = new StringBuilder(256);
                GetClassName(hWnd, sb, sb.Capacity);

                if (sb.ToString() == "Chrome_WidgetWin_1")
                {
                    uint processId;
                    GetWindowThreadProcessId(hWnd, out processId);
                    Process proc = Process.GetProcessById((int)processId);

                    if (proc.ProcessName == "chrome" && IsWindowVisible(hWnd))
                    {
                        chromeWindows.Add(hWnd);
                    }
                }

                return true;
            }, IntPtr.Zero);
            if (chromeWindows.Count >= 4)
            {
                IntPtr[] windowCount = chromeWindows.ToArray();
                int[] windowX = { rnd.Next(screenWidth), rnd.Next(screenWidth), rnd.Next(screenWidth), rnd.Next(screenWidth)};
                int[] windowY = { rnd.Next(screenHeight), rnd.Next(screenHeight), rnd.Next(screenHeight), rnd.Next(screenHeight)};
                int[] changeX = { 1, 1, 1, 1 };
                int[] changeY = { 1, 1, 1, 1 };
                int[] appWidth = { 100, 100, 100, 400};
                int[] appHeight = { 600, 600, 600, 900 };
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
                            changeX[count] = rnd.Next(1,5) *-1;
                        }
                        if (rect.left <= 0)
                        {
                            changeX[count] = rnd.Next(1,5);
                        }
                        if (rect.bottom >= screenHeight)
                        {
                            changeY[count] = rnd.Next(1, 5) * -1;
                        }
                        if (rect.top <= 0)
                        {
                            changeY[count] = rnd.Next(1, 5);
                        }
                        windowX[count] += changeX[count];
                        windowY[count] += changeY[count];
                        Debug.WriteLine("Webpage: " + i + " rect left: " + rect.left + " rect right: " + rect.right+ " rect top: " + rect.top+ " rect bottom: " + rect.bottom);
                        Debug.WriteLine("windowX: "+ windowX[count] +" windowY: "+ windowY[count]);
                        Debug.WriteLine("windowX: " + changeX[count] + " windowY: " + changeY[count]);
                        count++;
                    }
                    Thread.Sleep(1);
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
