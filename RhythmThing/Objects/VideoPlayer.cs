using RhythmThing.Components;
using RhythmThing.System_Stuff;
using RhythmThing.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace RhythmThing.Objects
{
    public class VideoPlayer : GameObject
    {
        private Visual visual;
        int index = 30;
        List<Bitmap> frames;
        Bitmap[] frameArray;
        string[] files;
        private string _path;
        private bool _playing = false;
        private double _timePassed = 0;
        private double _timePerFrame = 0;
        private int[] _startPoint = { 0, 0 };
        bool tog = false;
        private int _fps = 0;
        private int _frames;
        private int _currentFrame = 0;
        IFormatter formatter = new BinaryFormatter();
        FileStream readStream;
        public VideoPlayer(string path, string chartPath, string ChartInfoPath, Chart.videoInfo videoinfo)
        {
            //under current logic, this means that it is a bitmap folder. we will proceed to conver to .cvid
            this._path = path;
            this._fps = videoinfo.framerate;
            this._frames = videoinfo.frames;
            this._startPoint = videoinfo.startPoint;
            if (!path.EndsWith(".cvid"))
            {
                ImageUtils.BMPToCVID(path, ChartInfoPath, videoinfo, out this._path, out this._startPoint, out this._frames);
            
            }


            
            _timePerFrame = 1 / (double)_fps;

            readStream = new FileStream(_path, FileMode.Open);
            //ImageUtils.BMPToBinary(path, Path.Combine(Directory.GetCurrentDirectory(), "!Content", "testVid.cvid"));
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
            visual.z = -5;
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
                _timePassed += time;
                if(_timePassed > _timePerFrame * _currentFrame)
                {
                    Bitmap toLoad = null;
                    visual.localPositions.Clear();

                    while (_timePassed > _timePerFrame*_currentFrame && (_currentFrame != _frames))
                    {
                        
                        toLoad = (Bitmap)formatter.Deserialize(readStream);

                        _currentFrame++;
                    }
                    visual.LoadBMP(toLoad, _startPoint);

                }
                if (_currentFrame == _frames)
                {
                    _playing = false;
                    return;
                }


            }
        }
    }
}
