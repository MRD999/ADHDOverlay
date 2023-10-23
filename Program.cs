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

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        //global varable
        //Get Screen size
        public static int screenWidth { get; set; }
        public static int screenHeight { get; set; }

        static void Main(string[] args)
        {
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            Console.WriteLine("Screen Width: " + screenWidth);
            Console.WriteLine("Screen Height: " + screenHeight);
            //open notepad
            /*try
            {
                Parallel.Invoke(() =>
                {
                    Notepad();
                },
                () =>
                {
                    WebPage();
                });
            }
            catch ( AggregateException e )
            {
                Console.WriteLine("An action has thrown an exception. THIS WAS UNEXPECTED.\n{0}", e.InnerException.ToString());
            }
           // Notepad();
           //
            */
           WebPage();
        }
        static void Notepad()
        {
            Process notePadProcess = Process.Start("notepad", "readme.txt");
            Thread.Sleep(1000);
            // Find (the first-in-Z-order) Notepad window.
            IntPtr hWnd = FindWindow("Notepad", null);

            // If found, position it.
            if (hWnd != IntPtr.Zero)
            {
                int windowX = 2000;
                int windowY = 500;
                int changeX = 1;
                int changeY = 1;
                int appWidth = 150;
                int appHeight = 150;
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, appWidth, appHeight, 0x0002 | 0x0004);
                Thread.Sleep(1000);
                while (true)
                {
                    SetWindowPos(hWnd, IntPtr.Zero, windowX, windowY, 0, 0, 0x0001 | 0x0004);
                    if (windowX + appWidth+170 >= screenWidth)
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
                    Debug.WriteLine("Notepad: " + windowX + " " + windowY);
                    Thread.Sleep(10);
                }
            }
            else
            {
                Debug.WriteLine("Error window not found");
            }

        }
        static void WebPage()
        {
            Process.Start("chrome.exe", "https://bouncingdvdlogo.com");
            Thread.Sleep(1000);
            Process.Start("chrome.exe", "--new-window https://www.youtube.com/watch?v=eRXE8Aebp7s");
            Thread.Sleep(1000);
            // 
            IntPtr window0 = FindWindow("Chrome_WidgetWin_1", null);
            IntPtr window1 = FindWindowEx(IntPtr.Zero, window0, "Chrome_WidgetWin_1", null);
            Debug.WriteLine(window0);
            Debug.WriteLine(window1);

            // If found, position it.
            if (window0 != IntPtr.Zero && window1 != IntPtr.Zero)
            {
                IntPtr[] windowCount = { window0, window1 };
                int[] windowX = { 0,1000 };
                int[] windowY = { 0, 0 };
                int[] changeX = { 1, 0 };
                int[] changeY = { 0, 1 };
                int[] appWidth = { 100, 100 };
                int[] appHeight = { 600, 600 };
                //starting width
                SetWindowPos(window0, IntPtr.Zero, 0, 0, appWidth[0], appHeight[0], 0x0002 | 0x0004);
                SetWindowPos(window1, IntPtr.Zero, 0, 0, appWidth[1], appHeight[1], 0x0002 | 0x0004);
                Thread.Sleep(1000);
                while (true)
                {
                    int count = 0;
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
