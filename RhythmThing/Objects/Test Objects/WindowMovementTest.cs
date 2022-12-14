using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Objects.Test_Objects
{
    class WindowMovementTest : GameObject
    {
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            game.DisplayInstance.windowManager.MoveWindowRelativeToMonitor(100, 0);
            game.addGameObject(new SpriteWindowTest());
        }

        public override void Update(double time, Game game)
        {

        }
    }
}
