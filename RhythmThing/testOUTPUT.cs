using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing
{
   public class testOUTPUT
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "WriteConsoleOutputA", CharSet = CharSet.Unicode)]
        static extern bool WriteConsoleOutputW(SafeFileHandle hConsoleOutput, CharInfo[] lpBuffer, Coord dwBufferSize, Coord dwBufferCoord, ref SmallRect lpWriteRegion);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] uint fileAccess, [MarshalAs(UnmanagedType.U4)] uint fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] int flags, IntPtr template);
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "SetConsoleOutputCP")]
        static extern bool SetConsoleOutputCP(uint wCodePageID);

        private static readonly SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        public static void RegionWrite(string s, int x, int y, int width, int height)
        {
            if (!h.IsInvalid)
            {
                int length = width * height;

                // Pad any extra space we have
                string fill = s + new string(' ', length - s.Length);

                // Grab the background and foreground as integers
                int bg = (int)Console.BackgroundColor;
                int fg = (int)Console.ForegroundColor;

                // Make background and foreground into attribute value
                short attr = (short)(fg | (bg << 4));

                CharInfo[] buf = fill.Select(c =>
                {
                    CharInfo info = new CharInfo();

                    // Give it our character to write
                    info.Char.UnicodeChar = c;

                    // Use our attributes
                    info.Attributes = attr;

                    // Return info for this character
                    return info;

                }).ToArray();

                // Make everything short so we don't have to cast all the time
                short sx = (short)x;
                short sy = (short)y;
                short swidth = (short)width;
                short sheight = (short)height;

                // Make a buffer size out our dimensions
                Coord bufferSize = new Coord(swidth, sheight);

                // Not really sure what this is but its probably important
                Coord pos = new Coord(0, 0);

                // Where do we place this?
                SmallRect rect = new SmallRect() { Left = sx, Top = sy, Right = (short)(sx + swidth), Bottom = (short)(sy + sheight) };

                bool b = WriteConsoleOutputW(h, buf, bufferSize, pos, ref rect);
            }
            else
            {
                throw new Exception("Console handle is invalid.");
            }

        }
        public void Setup()
        {
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            Console.OutputEncoding = System.Text.Encoding.UTF8;




            SetConsoleOutputCP(65001);

            //DefaultColor();
            Console.Clear();

            Console.ReadLine();

            RegionWrite("┬┬┬fffffffffffffffffffffffffffffffffffffffffff┬", 4, 4, 10, 10);

            //Console.WriteLine("┬┬┬┬");

            Console.ReadLine();
        }
    }
}
