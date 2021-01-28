using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using RhythmThing.System_Stuff;

namespace RhythmThing.Objects
{
    class LeftArrow : GameObject
    {
        private double currenttime = 0;
        private Visual visual;
        public override void End()
        {
        }

        public override void Start(Game game)
        {
            //this block defines it as visual
            this.Components = new List<Component>();
            this.GameObjectType = objType.visual;
            this.visual = new Visual();
            visual.Active = true;
            visual.x = 0;
            visual.y = 0;
            visual.z = 0;

            //holy fuck never do that again

            /*
            for (int i = 6; i > -2; i--)
            {
                visual.localPositions.Add(new Coords(i, 0, 'h', ConsoleColor.Green, ConsoleColor.Green));
                visual.localPositions.Add(new Coords(i, 1, 'h', ConsoleColor.Green, ConsoleColor.Green));
                visual.localPositions.Add(new Coords(i, -1, 'h', ConsoleColor.Green, ConsoleColor.Green));
            }
            for (int i = -2; i > -8; i--)
            {
                for (int x = -5 +((-i)-2); x < 5 - ((-i)-3); x++)
                {
                    visual.localPositions.Add(new Coords(i, x, 'h', ConsoleColor.Green, ConsoleColor.Green));
                }
            } */
            visual.localPositions.Add(new Coords(0, 0, 'h', ConsoleColor.Green, ConsoleColor.Green));
            Components.Add(visual);
        }

        public override void Update(double time, Game game)
        {
            currenttime = currenttime + time;
            if (currenttime > 1)
            {
                visual.x++;
                visual.y++;
                currenttime = 0; 
            }
        }
    }
}
