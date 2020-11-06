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
        private Chart _chart;
        public static float LastBeat;
        IFormatter formatter = new BinaryFormatter();
        FileStream readStream;
        public VideoPlayer(string path, string chartPath, string ChartInfoPath, Chart.videoInfo videoinfo, Chart chart)
        {
            //under current logic, this means that it is a bitmap folder. we will proceed to conver to .cvid
            this._path = path;
            this._fps = videoinfo.framerate;
            this._frames = videoinfo.frames;
            this._startPoint = videoinfo.startPoint;
            this._chart = chart;
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
            readStream.Close();
        }


        public override void Start(Game game)
        {
            frames = new List<Bitmap>();
            this.components = new List<Component>();
            visual = new Visual();
            visual.active = true;
            visual.x = 0;
            visual.z = -5;
            //visual.y = -25;
            visual.localPositions.Add(new Coords(0, 0, ' ', ConsoleColor.Red, ConsoleColor.Red));

            components.Add(visual);
            _playing = true;
            _timePerFrame = LastBeat / _frames;
        }
        public void play()
        {
            _playing = true;
        }

        public override void Update(double time, Game game)
        {
            if (_playing)
            {
                //_timePassed += time;
                if(_chart.vBeat >= _timePerFrame * _currentFrame)
                {
                    byte[,] toLoad = null;
                    visual.localPositions.Clear();

                    while (_chart.vBeat >= _timePerFrame*(_currentFrame) && (_currentFrame != _frames))
                    {
                        
                        toLoad = (byte[,])formatter.Deserialize(readStream);

                        _currentFrame++;
                    }
                    visual.LoadCVidFrame(toLoad, _startPoint);
                    
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
