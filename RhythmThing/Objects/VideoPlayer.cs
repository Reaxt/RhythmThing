using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace RhythmThing.Objects
{
    public class VideoPlayer : GameObject
    {
        private Visual visual;
        int index = 0;
        List<Bitmap> frames;
        Bitmap[] frameArray;
        string[] files;
        private string _path;
        private bool _playing = false;
        private float _timePassed = 0;
        private float _timePerFrame = 0;
        int[] startPoint = { 0, 0 };
        bool tog = false;
        private int _fps = 0;
        public VideoPlayer(string path, int fps)
        {
            this._path = path;
            this._fps = fps;

            files = Directory.GetFiles(path);
            Bitmap firstMap = (Bitmap)Image.FromFile(files[0]);
            startPoint[0] = Program.ScreenX - (firstMap.Width / 2);
            _timePerFrame = 1 / (float)fps;
        }

        public override void End()
        {

        }


        public override void Start(Game game)
        {
            frames = new List<Bitmap>();
            this.components = new List<Component>();
            visual = new Visual();
            visual.active = true;
            visual.x = 0;
            visual.z = -2;
            //visual.y = 25;
            visual.localPositions.Add(new Coords(0, 0, ' ', ConsoleColor.Red, ConsoleColor.Red));

            components.Add(visual);
            _playing = true;

        }
        public void play()
        {
            _playing = true;
        }

        public override void Update(double time, Game game)
        {
            if (_playing)
            {
                if (_timePassed >= _timePerFrame)
                {
                    _timePassed = 0;
                    visual.localPositions.Clear();
                    visual.LoadBMP(files[index], startPoint);
                }
                else
                {
                    _timePassed += (float)time;
                }

            }
        }
    }
}
