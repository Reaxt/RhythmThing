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
            this.Components = new List<Component>();
            visual = new Visual();
            visual.Active = true;
            visual.y = -1;
            
            visual.LoadBMP(Path.Combine("Untitled.bmp"), new int[] { 0,0});
            Components.Add(visual);
        }

        public override void Update(double time, Game game)
        {

        }
    }
}
