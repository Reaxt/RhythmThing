using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace RhythmThing.Objects.Test_Objects
{
    class BMPTest : GameObject
    {
        private Visual visual;
        int index = 0;
        List<Bitmap> frames;
        Bitmap[] frameArray;
        string[] files;
        int[] startPoint = { 0, 0 };
        bool tog = false;
        public override void End()
        {

        }


        public override void Start(Game game)
        {
            frames = new List<Bitmap>();
            this.components = new List<Component>();
            visual = new Visual();
            visual.active = true;
            visual.x = 15;
            visual.z = -2;
            //visual.y = 25;
            visual.localPositions.Add(new Coords(0, 0, ' ', ConsoleColor.Red, ConsoleColor.Red));
            files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "!Content", "bmpvideotest", "bitmaps"));
            for (int i = 0; i < 5000; i++)
            {
                frames.Add((Bitmap)Image.FromFile(files[i]));
            }
            components.Add(visual);
        }

        public override void Update(double time, Game game)
        {
            visual.localPositions.Clear();
            visual.LoadBMP(files[index], startPoint);
            index++;
        }
    }
}
