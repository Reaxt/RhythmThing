using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Objects.Test_Objects
{
    class SpriteWindowTest : GameObject
    {
        SpriteWindow testWindow;
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            testWindow = new SpriteWindow(50, 50, 15, 15*2);
            testWindow.DrawSpriteToWindow(System.Drawing.Image.FromFile("./untitled.png"),false);
            testWindow.ShowWindow();
        }

        public override void Update(double time, Game game)
        {

        }
    }
}
