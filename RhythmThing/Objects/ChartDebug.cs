using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Objects
{
    public class ChartDebug : GameObject
    {
        Visual visual;
        Chart chart;
        string beat = "Beat: ";
        ConsoleColor fore = ConsoleColor.White;
        ConsoleColor back = ConsoleColor.Black;
        public ChartDebug(Chart chart)
        {
            this.chart = chart;
        }
        public override void End()
        {
            //throw new NotImplementedException();
        }

        public override void Start(Game game)
        {
            Components = new List<Component>();
            visual = new Visual();

            visual.y = 49;
            visual.z = 5;
            visual.Active = true;
            Components.Add(visual);
            //throw new NotImplementedException();
        }

        public override void Update(double time, Game game)
        {
            //throw new NotImplementedException();
            string theThing = beat + chart.beat.ToString();
            visual.localPositions.Clear();
            for (int i = 0; i < theThing.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, theThing[i], fore, back));
            }
        }
    }
}
