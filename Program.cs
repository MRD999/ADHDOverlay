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
            try
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
           //WebPage();

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
                Console.WriteLine("Error window not found");
            }

        }
        static void WebPage()
        {
            Process.Start("chrome.exe", "https://bouncingdvdlogo.com");
            Thread.Sleep(1000);
            // 
            IntPtr hWnd = FindWindow("Chrome_WidgetWin_1", null);
            Console.WriteLine(hWnd);

            // If found, position it.
            if (hWnd != IntPtr.Zero)
            {
                int windowX = 0;
                int windowY = 0;
                int changeX = 1;
                int changeY = 0;
                int appWidth = 100;
                int appHeight = 600;
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, appWidth, appHeight, 0x0002 | 0x0004);
                Thread.Sleep(1000);
                while (true)
                {
                    SetWindowPos(hWnd, IntPtr.Zero, windowX, windowY, 0, 0, 0x0001 | 0x0004);
                    if (windowX + appWidth+400 >= screenWidth)
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
                    Debug.WriteLine("Webpage: "+windowX + " " + windowY);
                    Thread.Sleep(5);
                }
            }
            else
            {
                Console.WriteLine("Error window not found");
            }

        }
    }
}
