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
namespace RhythmThing.Objects
{
    public class SpriteWindow : GameObject
    {
        private Form _form = new Form();
        private Graphics _graphics;
        Visual visual;
        Screen screen;
        //TODO: implement easings!
        //TODO: window movement but good
        public override void End()
        {
            _form.Dispose();
        }
        public SpriteWindow(float startX, float startY, float width, float height)
        {
            //consider for 16:9??
            screen = Screen.PrimaryScreen;
            int realX = (int)Math.Round((startX/100)* screen.WorkingArea.Width);
            int realY = (int)Math.Round(((100-startY) / 100) * screen.WorkingArea.Height);
            int realWidth = (int)Math.Round((width) * ((double)screen.WorkingArea.Width/(double)WindowManager.DISPLAY_CALIBRATED_WIDTH));
            int realHeight = (int)Math.Round((height) * ((double)screen.WorkingArea.Height/(double)WindowManager.DISPLAY_CALIBRATED_HEIGHT));
            _form.AllowTransparency = true;
            _form.BackColor = Color.Black;
            
            _form.FormBorderStyle = FormBorderStyle.None;
            _form.Bounds = new Rectangle(realX,realY,(int)realWidth,(int)realHeight);
            _form.TopLevel = true;
            _form.TopMost = true;
            _form.TransparencyKey = Color.Black;
            _form.CreateControl();
            _graphics = _form.TopLevelControl.CreateGraphics();
            _form.Show();
           
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
        }
        public void DrawSprite(Image image,int x, int y, int width, int height,bool refresh)
        {
            if (refresh) _graphics.Clear(Color.Black);
            _graphics.DrawImage(image, x,y,width,height);
        }
        //draw a sprite along the whole window
        public void DrawSpriteToWindow(Image image, bool refresh)
        {
            if (refresh) _graphics.Clear(Color.Black);
            _graphics.DrawImage(image, _form.DisplayRectangle);
        }
        public override void Start(Game game)
        {
            ShowWindow();
            DrawSprite(Image.FromFile("./untitled.png"), 0, 0, 500 / 2, 1000 / 2, true);
            KillWindow();


        }

        public override void Update(double time, Game game)
        {

        }
    }
}
