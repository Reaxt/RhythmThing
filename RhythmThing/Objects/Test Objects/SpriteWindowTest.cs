using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Utils;
namespace RhythmThing.Objects.Test_Objects
{
    class SpriteWindowTest : GameObject
    {
        SpriteWindow testWindow;
        float curTime = 0;
        float goalTime = 10;
        float[] pos1 = new float[] { 40, 150 };
        float[] pos2 = new float[] { 40, 90 };

        public override void End()
        {

        }

        public override void Start(Game game)
        {
            testWindow = new SpriteWindow(50, 50, 500, 1000);
            testWindow.DrawSpriteToWindow(System.Drawing.Image.FromFile("./untitled.png"),false);
            testWindow.ShowWindow();
        }

        public override void Update(double time, Game game)
        {
            if(curTime <= goalTime)
            {
                float ease = Ease.Bounce.Out(curTime / goalTime);
                testWindow.MoveWindow(Utils.Ease.Lerp(pos1[0], pos2[0], ease), Utils.Ease.Lerp(pos1[1],pos2[1], ease));
                curTime += (float)time;
            }
        }
    }
}
