using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RhythmThing.System_Stuff;
using System.Windows.Forms;
using System.Drawing;
using RhythmThing.Components;
using RhythmThing.Utils;
namespace RhythmThing.Objects
{
    public class SpriteWindow : GameObject
    {
        private struct SimpleEase
        {
            public string Easing;
            public float EaseDuration;
            public float TimePassed;
            public float[] Point1;
            public float[] Point2;
            //to remove
            public bool ongoing;
        }
        private struct ChartSyncEase
        {
            public string Easing;
            public float StartBeat;
            public float EndBeat;
            public float[] Point1;
            public float[] Point2;
            //to remove
            public bool ongoing;
        }
        private Form _form = new Form();
        private Graphics _graphics;
        Visual visual;
        Screen screen;
        private bool _animation = false;
        private Bitmap gifBitmap;
        private int _realX;
        private int _realY;
        private int _realWidth;
        private int _realHeight;
        private bool _initForced = false;
        //TODO: multiple eases??????
        private SimpleEase _simpleEase;
        private ChartSyncEase chartSyncEase;
        
        public bool TopLevel { get
            {
                if(_form != null)
                {
                    return _form.TopMost;
                } else
                {
                    return false;
                }
            }
            set
            {
                if(_form != null)
                {
                    _form.TopMost = value;
                }
            }
        }

        //TODO: implement easings!
        //TODO: window movement but good
        public override void End()
        {
            _form.Close();
            _form.Dispose();
        }
        public SpriteWindow(float startX, float startY, float width, float height)
        {
            //consider for 16:9??
            screen = Screen.PrimaryScreen;
            _realX = (int)Math.Round((startX/100)* screen.WorkingArea.Width);
            _realY = (int)Math.Round(((100-startY) / 100) * screen.WorkingArea.Height);
            _realWidth = (int)Math.Round((width) * ((double)screen.WorkingArea.Width/(double)WindowManager.DISPLAY_CALIBRATED_WIDTH));
            _realHeight = (int)Math.Round((height) * ((double)screen.WorkingArea.Height/(double)WindowManager.DISPLAY_CALIBRATED_HEIGHT));
            _form.TopLevel = false;
           
        }
        public void ShowWindow()
        {
            _form.Show();
        }
        public void MoveWindow(float x, float y)
        {
            screen = Screen.PrimaryScreen;
            int realX = (int)Math.Round((x / 100) * screen.WorkingArea.Width);
            int realY = (int)Math.Round(((100 - y) / 100) * screen.WorkingArea.Height);
            _form.Bounds = new Rectangle(realX, realY, _form.Bounds.Width, _form.Bounds.Height);

        }
        public void HideWindow()
        {
            _form.Hide();
        }
        public void KillWindow()
        {
            this.alive = false;
            if (_animation) ImageAnimator.StopAnimate(gifBitmap, new EventHandler(this.OnFrameChanged));
        }
        //not sure how to do this rn, may deprecate
        public void DrawSprite(Image image,int x, int y, int width, int height,bool refresh)
        {
            if (refresh) _graphics.Clear(Color.Black);
            _graphics.DrawImage(image, x,y,width,height);
            if (_animation)
            {
                ImageAnimator.StopAnimate(gifBitmap, new EventHandler(this.OnFrameChanged));
                _animation = false;
            }
        }
        /// <summary>
        /// Draw a sprite to the window, stretched to fit
        /// </summary>
        /// <param name="image">The image to draw</param>
        /// <param name="refresh">Whether or not youd like to clear the last drawn image</param>
        public void DrawSpriteToWindow(Image image, bool refresh)
        {
            if (refresh) _graphics.Clear(Color.Black);
            _graphics.DrawImage(image, _form.DisplayRectangle);
            if (_animation)
            {
                ImageAnimator.StopAnimate(gifBitmap, new EventHandler(this.OnFrameChanged));
                _animation = false;
            }
        }
        /// <summary>
        /// given a path, draw and animate a gif to the window
        /// </summary>
        /// <param name="path">path to gif</param>
        public void DrawGifToWindow(string path)
        {
            _graphics.Clear(Color.Black);
            _animation = true;
            gifBitmap = new Bitmap(path);
            //begin animation
            ImageAnimator.Animate(gifBitmap, new EventHandler(this.OnFrameChanged));
        }
        private void OnFrameChanged(object o, EventArgs e)
        {
            //TODO: why is it flickering?
            _graphics.Clear(Color.Black);
            _graphics.DrawImage(gifBitmap, _form.DisplayRectangle);
        }
        public override void Start(Game game)
        {

            if (!_initForced)
            {
                _form.AllowTransparency = true;
                _form.BackColor = Color.Black;

                _form.FormBorderStyle = FormBorderStyle.None;
                _form.Bounds = new Rectangle(_realX, _realY, (int)_realWidth, (int)_realHeight);
                //_form.TopLevel = true;
                //_form.TopMost = true;
                _form.TransparencyKey = Color.Black;
                _form.CreateControl();
                _graphics = _form.TopLevelControl.CreateGraphics();
                _form.Show();
            }
        }
        /// <summary>
        /// Forcefully initiate the object ahead of time
        /// </summary>
        public void ForceInit()
        {
            _initForced = true;
            _form.AllowTransparency = true;
            _form.BackColor = Color.Black;

            _form.FormBorderStyle = FormBorderStyle.None;
            _form.Bounds = new Rectangle(_realX, _realY, (int)_realWidth, (int)_realHeight);
            _form.TopLevel = true;
            //_form.TopMost = true;
            _form.TransparencyKey = Color.Black;
            _form.CreateControl();
            _graphics = _form.TopLevelControl.CreateGraphics();
            _form.Show();
        }

        public void MoveWindowEase(float[] pos1, float[]pos2, string easing, float duration)
        {
            _simpleEase = new SimpleEase();
            _simpleEase.Easing = easing;
            _simpleEase.EaseDuration = duration;
            _simpleEase.Point1 = pos1;
            _simpleEase.Point2 = pos2;
            _simpleEase.TimePassed = 0;
            _simpleEase.ongoing = true;
        }

        public override void Update(double time, Game game)
        {
            if (_animation)
            {
                ImageAnimator.UpdateFrames();
            }
            if(_simpleEase.ongoing)
            {
                if (_simpleEase.TimePassed > _simpleEase.EaseDuration)
                {
                    _simpleEase.ongoing = false;
                    MoveWindow(_simpleEase.Point2[0], _simpleEase.Point2[1]);
                } else
                {
                    float[] curpos = Ease.Lerp(_simpleEase.Point1, _simpleEase.Point2, Ease.byName[_simpleEase.Easing](_simpleEase.TimePassed/_simpleEase.EaseDuration));
                    MoveWindow(curpos[0], curpos[1]);
                    _simpleEase.TimePassed += (float)time;
                }
            }
        }
    }
}
