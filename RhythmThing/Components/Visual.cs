using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.System_Stuff;
using RhythmThing.Utils;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace RhythmThing.Components
{
    public class Visual : Component
    {


        public int x;
        public int y;
        private int _savedX;
        private int _savedY;
        public int z;
        public int randAmount = 0;
        public bool randBreak = false;
        public ConsoleColor overrideback;
        public ConsoleColor overridefront;
        public bool overrideColor = false;
        public int randSeed = 1337;
        private Random random = new Random();
        //what the object should interact with (local space)
        public List<Coords> localPositions = new List<Coords>();
        //what the visual class wil work with (world space)
        public List<Coords> renderPositions = new List<Coords>();
        //animation list
        private List<VisualAnimation> _animations = new List<VisualAnimation>();

        private int bigx = int.MinValue;
        private int bigy = int.MinValue;
        private int smallx = int.MaxValue;
        private int smally = int.MaxValue;

        public void writeText(int startingX, int startingY, string text, ConsoleColor front, ConsoleColor back)
        {
            char[] textArray = text.ToCharArray();
            for (int i = 0; i < textArray.Length; i++)
            {
                this.localPositions.Add(new Coords(startingX + i, startingY, textArray[i], front, back));
            }
        }

        public void Animate(int[] startPoint, int[] endPoint, string easing, float duration, bool saveCoords = true)
        {
            _animations.Add(new VisualAnimation(startPoint, endPoint, easing, duration, new int[] { x, y }, saveCoords));
        }

        public void LoadBMP(string path, int[] LBcorner)
        {

            Bitmap bitmap = (Bitmap)Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "!Content", path));
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    if (pixel.A == 0)
                    {

                    }
                    else
                    {
                        ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                        localPositions.Add(new Coords(LBcorner[0] + x, LBcorner[1] + (-y + bitmap.Height), ' ', consoleColor, consoleColor));

                    }
                }
            }

        }
        public void LoadBMP(string path)
        {

            Bitmap bitmap = (Bitmap)Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "!Content", path));
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    if (pixel.A == 0)
                    {

                    }
                    else
                    {
                        ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                        localPositions.Add(new Coords(x, (-y + bitmap.Height), ' ', consoleColor, consoleColor));

                    }
                }
            }

        }

        public void LoadCVidFrame(byte[,] frame)
        {
            for (int x = 0; x < frame.GetLength(0); x++)
            {
                for (int y = 0; y < frame.GetLength(1); y++)
                {

                    localPositions.Add(new Coords(x, y, ' ', (ConsoleColor)frame[x, y], (ConsoleColor)frame[x, y]));
                }
            }
        }
        public void LoadCVidFrame(byte[,] frame, int[] lbcorner)
        {
            for (int x = 0; x < frame.GetLength(0); x++)
            {
                for (int y = 0; y < frame.GetLength(1); y++)
                {

                    localPositions.Add(new Coords(x + lbcorner[0], y + lbcorner[1], ' ', (ConsoleColor)frame[x, y], (ConsoleColor)frame[x, y]));
                }
            }
        }
        public void LoadBMP(Bitmap bitmap, int[] LBcorner)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                    localPositions.Add(new Coords(LBcorner[0] + x, LBcorner[1] + (-y + bitmap.Height), ' ', consoleColor, consoleColor));
                }
            }
        }
        public void LoadBMP(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                    localPositions.Add(new Coords(x, (-y + bitmap.Height), ' ', consoleColor, consoleColor));
                }
            }
        }
        public void ClearAnims()
        {
            foreach (VisualAnimation animation in _animations)
            {
                animation.Live = false;
            }
        }
        public void AddAnimObject(VisualAnimation anim)
        {
            _animations.Add(anim);
        }
        public override void Update(double time)
        {
            //move the visual positions with any animations
            _savedX = x;
            _savedY = y;
            //anims only go on x and y rn...
            foreach (VisualAnimation animation in _animations.ToArray())
            {
                int[] offset = animation.UpdateAnim(time);
                if (!animation.Live)
                {
                    if (animation.SaveCoords)
                    {
                        x += offset[0];
                        y += offset[1];
                        _savedX += offset[0];
                        _savedY += offset[1];
                    }
                    _animations.Remove(animation);
                }
                else
                {
                    x += offset[0];
                    y += offset[1];

                }
            }



            //KISS for now
            renderPositions = new List<Coords>();
            random = new Random(randSeed);
            foreach (Coords coord in localPositions)
            {
                //only works with the one way rn
                int coordX = coord.x;
                int coordY = coord.y;

                if (randBreak)
                {
                    coordX += random.Next(-randAmount, randAmount);
                    coordY += random.Next(-randAmount, randAmount);
                }
                if (overrideColor)
                {
                    renderPositions.Add(new Coords(x + coordX, y + coordY, coord.character, overrideback, overridefront));
                }
                else
                {
                    renderPositions.Add(new Coords(x + coordX, y + coordY, coord.character, coord.foreColor, coord.backColor));

                }


            }
            //Restore the positions for any calculations an stuff
            x = _savedX;
            y = _savedY;

        }
    }
    public class VisualAnimation
    {
        public bool Live;
        private float _timePassed;
        private float _duration;
        private int[] _startPoint;
        private int[] _endPoint;
        private int[] _offset = { 0, 0 };
        public bool SaveCoords;
        private Func<float, float> _easeFunction;
        public VisualAnimation(int[] startPoint, int[] endPoint, string easing, float duration, int[] initialPoint, bool saveCoords)
        {
            this._startPoint = startPoint;
            this._endPoint = endPoint;
            this.SaveCoords = saveCoords;
            _easeFunction = Ease.byName[easing];
            _duration = duration;
            _timePassed = 0;

            _offset = new int[] { _startPoint[0] - initialPoint[0], _startPoint[1] - initialPoint[1] };

            Live = true;

        }

        public int[] UpdateAnim(double time)
        {
            if (_timePassed > _duration)
            {
                Live = false;
                return new int[] { _endPoint[0] - _startPoint[0] + _offset[0], _endPoint[1] - _startPoint[1] + _offset[1] };
            }
            int resX = (int)Math.Ceiling(((float)_endPoint[0] - (float)_startPoint[0]) * (_easeFunction(_timePassed / _duration))) + _offset[0];
            int resY = (int)Math.Ceiling(((float)_endPoint[1] - (float)_startPoint[1]) * (_easeFunction(_timePassed / _duration))) + _offset[1];
            _timePassed += (float)time;
            return new int[] { resX, resY };


        }
    }

    [Serializable()]
    public struct Coords : ISerializable
    {
        //all relative to 0,0 on the visual
        public int x;
        public int y;
        public char character;
        public ConsoleColor foreColor;
        public ConsoleColor backColor;
        public Coords(int xx, int yy, char charac, ConsoleColor forecol, ConsoleColor backcol)
        {
            this.x = xx;
            this.y = yy;
            this.character = charac;
            this.foreColor = forecol;
            this.backColor = backcol;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", this.x, typeof(int));
            info.AddValue("y", this.y, typeof(int));
            info.AddValue("char", this.character, typeof(char));
            info.AddValue("foreCol", this.foreColor, typeof(ConsoleColor));
            info.AddValue("backCol", this.backColor, typeof(ConsoleColor));

        }
    }

}
