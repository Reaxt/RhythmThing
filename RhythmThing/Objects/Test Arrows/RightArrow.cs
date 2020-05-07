using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
namespace RhythmThing.Objects
{
    public class RightArrow : GameObject
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
            visual.x = 130;
            visual.y = 42;
            visual.z = 0;

            //holy fuck never do that again

          
            for (int i = -5; i < 2; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, 'h', ConsoleColor.Yellow, ConsoleColor.Yellow));
                visual.localPositions.Add(new Coords(i, 1, 'h', ConsoleColor.Yellow, ConsoleColor.Yellow));
                visual.localPositions.Add(new Coords(i, -1, 'h', ConsoleColor.Yellow, ConsoleColor.Yellow));
            }
            //visual.localPositions.Add(new Coords(8, 0, 'h', ConsoleColor.DarkGreen, ConsoleColor.DarkCyan));
            for (int i = 2; i < 8; i++)
            {
                for (int x = (-5) + i-2; x < 5-(i-3); x++)
                {
                    visual.localPositions.Add(new Coords(i, x, 'h', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
            }

            components.Add(visual);

        }

        public override void Update(double time, Game game)
        {
            currenttime = currenttime + time;
            if(currenttime > 0.3)
            { 
                //visual.x++;
                //visual.y++;
                //currenttime = 0; 
            }
        }
    }
}
