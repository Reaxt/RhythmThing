﻿using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using RhythmThing.Utils;
using System.Runtime.InteropServices;


namespace RhythmThing.Objects.Test_Objects
{
    class SpriteWindowTest : GameObject
    {
        SpriteWindow testWindow;
        SpriteWindow topLeft;
        SpriteWindow topRight;
        SpriteWindow bottomRight;
        SpriteWindow bottomLeft;

        float curTime = ((90 * 60) / 4)-1;
        float goalTime = (90*60)/4;
        int curI = 0;
        bool done = false;
        private float[][] positions = new float[4][];
        
        //Bitmap test = new Bitmap("./test.gif");

        public override void End()
        {

        }

        public override void Start(Game game)
        {

 
        }
        private void OnFrameChanged(object o, EventArgs e)
        {

            //Force a call to the Paint event handler.
            //testWindow.DrawSpriteToWindow(test, true);

        }
        public override void Update(double time, Game game)
        {
            
            if(curTime >= goalTime)
            {

                /*
                topLeft.MoveWindowEase(positions[curI % 4], new float[] {-50,150 }, moveEase, easeDur);
                topRight.MoveWindowEase(positions[(curI + 1) % 4], positions[(curI + 2) % 4 % 4], moveEase, easeDur);
                bottomRight.MoveWindowEase(positions[(curI + 2) % 4], positions[(curI + 3) % 4], moveEase, easeDur);
                bottomLeft.MoveWindowEase(positions[(curI + 3) % 4], positions[(curI + 4) % 4], moveEase, easeDur);
                done = true;*/
            }
            

            curTime += (float)time;
            //ImageAnimator.UpdateFrames();
        }
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow());
        }
        public static Image CaptureDesktopArea(Rectangle area)
        {
            var result = new Bitmap(area.Width, area.Height);
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(area.Left, area.Top), Point.Empty, area.Size);
            }

            return result;
        }
        public static Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }

        public static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }
    }
}
