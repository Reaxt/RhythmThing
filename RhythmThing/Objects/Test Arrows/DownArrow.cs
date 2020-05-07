using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.System_Stuff;
using RhythmThing.Components;

namespace RhythmThing.Objects
{
    class DownArrow : GameObject
    {
        private double currenttime = 0;
        private Visual visual;
        public override void End()
        {
        }

        public override void Start(Game game)
        {
            //this block defines it as visual
            this.components = new List<Component>();
            this.type = objType.visual;
            this.visual = new Visual();
            visual.active = true;
            visual.x = 85;
            visual.y = 44;
            visual.z = 0;

            //holy fuck never do that again


            for (int i = 3; i > -2; i--)
            {
                visual.localPositions.Add(new Coords(0, i, 'h', ConsoleColor.Blue, ConsoleColor.Blue));
                visual.localPositions.Add(new Coords(1, i, 'h', ConsoleColor.Blue, ConsoleColor.Blue));
                visual.localPositions.Add(new Coords(-1, i, 'h', ConsoleColor.Blue, ConsoleColor.Blue));
            }
            for (int i = -2; i > -8; i--)
            {
                for (int x = -5 + ((-i) - 2); x < 5 - ((-i) - 3); x++)
                {
                    visual.localPositions.Add(new Coords(x, i, 'h', ConsoleColor.Blue, ConsoleColor.Blue));
                }
            }
            components.Add(visual);

        }

        public override void Update(double time, Game game)
        {
            currenttime = currenttime + time;
            if (currenttime > 0.2)
            {
               // visual.x++;
               // visual.y++;
                //currenttime = 0; 
            }
        }
    }
}
