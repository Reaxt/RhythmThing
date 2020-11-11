using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing
{
    public class WindowManager
    {

        //todo: comment everything and explain it.
        //TODO: slight refactoring. I think theres some repetition
        //this was written with massive help from a good friend of mine, ikeiwa. https://github.com/ikeiwa 
        //Lesson to be learned is that interop is hard.

        

        IntPtr wHnd;
        int wwidth = 100;
        int wheight = 50;



        public int wwidth1 = 100;
        public int wheight1 = 50;

        short pwidth = 15;
        short pheight = 15;

        string wtitle = "Rhythm Thing";

        CHAR_INFO[,] buffer;

        #region native consts
        const int STD_INPUT_HANDLE = -10;
        const int STD_OUTPUT_HANDLE = -11;
        const int STD_ERROR_HANDLE = -12;

        const int GWL_STYLE = -16;

        const int WS_MAXIMIZEBOX = 0x00010000;
        const int WS_SIZEBOX = 0x00040000;

        const int LF_FACESIZE = 32;
        #endregion

        #region native structs

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CONSOLE_FONT_INFOEX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
            public string FaceName;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CHAR_INFO
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public char AsciiChar;
            [FieldOffset(2)]
            public ushort Attributes;
        }

        ushort None = 0x0000;
        ushort FOREGROUND_BLUE = 0x0001;
        ushort FOREGROUND_GREEN = 0x0002;
        ushort FOREGROUND_RED = 0x0004;
        ushort FOREGROUND_INTENSITY = 0x0008;
        ushort BACKGROUND_BLUE = 0x0010;
        ushort BACKGROUND_GREEN = 0x0020;
        ushort BACKGROUND_RED = 0x0040;
        ushort BACKGROUND_INTENSITY = 0x0080;
        ushort COMMON_LVB_LEADING_BYTE = 0x0100;
        ushort COMMON_LVB_TRAILING_BYTE = 0x0200;
        ushort COMMON_LVB_GRID_HORIZONTAL = 0x0400;
        ushort COMMON_LVB_GRID_LVERTICAL = 0x0800;
        ushort COMMON_LVB_GRID_RVERTICAL = 0x1000;
        ushort COMMON_LVB_REVERSE_VIDEO = 0x4000;
        ushort COMMON_LVB_UNDERSCORE = 0x8000;



        #endregion

        #region native methods
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int width, int height, bool repaint);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        internal extern static int SetWindowLong(IntPtr hwnd, int index, int value);

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        const int MONITOR_DEFAULTTOPRIMARY = 1;
        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [StructLayout(LayoutKind.Sequential)]
        struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
            public static MONITORINFO Default
            {
                get { var inst = new MONITORINFO(); inst.cbSize = (uint)Marshal.SizeOf(inst); return inst; }
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")]
        internal extern static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, ref CONSOLE_FONT_INFOEX consoleCurrentFontEx);

        [DllImport("kernel32.dll", EntryPoint = "WriteConsoleOutputW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool WriteConsoleOutput(
        IntPtr hConsoleOutput,
        [MarshalAs(UnmanagedType.LPArray), In] CHAR_INFO[] lpBuffer,
        COORD dwBufferSize,
        COORD dwBufferCoord,
        ref SMALL_RECT lpWriteRegion);

        #endregion

        public WindowManager()
        {
            wHnd = GetStdHandle(STD_OUTPUT_HANDLE);
            //buffer = new CHAR_INFO[];

            for (int x = 0; x < wwidth; ++x)
            {
                for (int y = 0; y < wheight; ++y)
                {
                    //buffer[x, y] = new CHAR_INFO { UnicodeChar = 'A', Attributes = (ushort)(BACKGROUND_BLUE | FOREGROUND_GREEN | BACKGROUND_INTENSITY) };
                }
            }
        }

        public void InitWindow()
        {
            IntPtr hwnd = GetConsoleWindow();
            int value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, value & ~WS_MAXIMIZEBOX & ~WS_SIZEBOX);

            CONSOLE_FONT_INFOEX cfi = new CONSOLE_FONT_INFOEX
            {
                nFont = 0,
                dwFontSize = new COORD(pwidth, pheight),
                FontFamily = (1 << 4),
                FontWeight = 400,
                FaceName = "Consolas"

            };
            cfi.cbSize = (uint)Marshal.SizeOf(cfi);
            SetCurrentConsoleFontEx(wHnd, false, ref cfi);
            wwidth = Program.ScreenX;
            wheight = Program.ScreenY;
            wwidth1 = Program.ScreenX;
            wheight1 = Program.ScreenY;
            Console.SetWindowSize(wwidth, wheight);
            Console.SetBufferSize(wwidth, wheight);
            Console.Title = wtitle;
            Console.CursorVisible = false;


            //RenderBuffer(buffer);
        }

        public void RenderBuffer(CHAR_INFO[] buffer)
        {
            SMALL_RECT writeArea = new SMALL_RECT { Left = 0, Top = 0, Right = (short)(wwidth1 - 1), Bottom = (short)(wheight1) };
            WriteConsoleOutput(wHnd, buffer, new COORD { X = (short)wwidth1, Y = (short)wheight1 }, new COORD { X = 0, Y = 0 }, ref writeArea);
        }
        //move relative to window or something
        public void moveWindow(float x, float y)
        {
            //OK BOYS HERES THE PLAN
            //-1 = left window all the way left
            //1=right window all the way right
            RECT existingRect;
            IntPtr hwnd = GetConsoleWindow();
            var mi = MONITORINFO.Default;
            GetMonitorInfo(MonitorFromWindow(hwnd, MONITOR_DEFAULTTOPRIMARY), ref mi);

            GetWindowRect(new HandleRef(this, hwnd), out existingRect);
            //-1 = mi.rcWork.Left
            int resx = 0;
            int resy = 0;
            int denominator1 = (mi.rcWork.Right / 2) - ((existingRect.Right - existingRect.Left) / 2);

            float newx = x;
            //im presuming left is 0. this could be a mistake. belchhh
            resx = (int)((float)(mi.rcWork.Right / 2) - ((existingRect.Right - existingRect.Left) / 2) * newx);
            resy = (int)((float)(mi.rcWork.Bottom / 2) - ((existingRect.Bottom - existingRect.Top) / 2) * y);

            //1 = mi.rcWork.Right-(existingRect.Right - existingRect.Left)
            MoveWindow(hwnd, resx, resy, existingRect.Right - existingRect.Left, existingRect.Bottom - existingRect.Top, false);

        }
        public void CenterWindow()
        {
            RECT existingRect;
            IntPtr hwnd = GetConsoleWindow();
            var mi = MONITORINFO.Default;
            GetMonitorInfo(MonitorFromWindow(hwnd, MONITOR_DEFAULTTOPRIMARY), ref mi);

            GetWindowRect(new HandleRef(this, hwnd), out existingRect);
            MoveWindow(hwnd, (mi.rcWork.Right / 2) - ((existingRect.Right - existingRect.Left) / 2), (mi.rcWork.Bottom / 2) - ((existingRect.Bottom - existingRect.Top)/2), existingRect.Right - existingRect.Left, existingRect.Bottom - existingRect.Top, false);
        }
        public static bool isFocused()
        {
            var activatedHandle = GetForegroundWindow();
            var hwnd = GetConsoleWindow();
            if(activatedHandle == hwnd)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
