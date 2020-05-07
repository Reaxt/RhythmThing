using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;
using RhythmThing.Components;
namespace RhythmThing.Objects
{
    public class Arrow : GameObject
    {
        private Visual visual;
        public Chart.collumn collumn;
        public Chart.NoteInfo noteInfo;
        private Chart chart;
        private float jumpAmount;
        private float beatTime;
        private float movementAmount;
        //this needs to be changed
        private int aimY;
        public override void End()
        {
        }
        public Arrow(Chart.collumn col, Chart.NoteInfo note, Chart chart)
        {

            collumn = col;
            noteInfo = note;
            this.chart = chart;
        }
        public override void Start(Game game)
        {
            //this block defines it as visual
            this.components = new List<Component>();
            this.type = objType.visual;
            this.visual = new Visual();
            visual.active = true;
            visual.x = 30;
            visual.y = 42;
            visual.z = 0;
            

            //holy fuck never do that again
            //rumour has it there was once a disgusting block of code here
            //this is still kinda shit...

            switch (collumn)
            {
                case Chart.collumn.Left:
                    for (int i = 3; i > -2; i--)
                    {
                        visual.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Green, ConsoleColor.Green));
                        visual.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Green, ConsoleColor.Green));
                        visual.localPositions.Add(new Coords(i, -1, ' ', ConsoleColor.Green, ConsoleColor.Green));
                    }
                    for (int i = -2; i > -8; i--)
                    {
                        for (int x = -5 + ((-i) - 2); x < 5 - ((-i) - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(i, x, ' ', ConsoleColor.Green, ConsoleColor.Green));
                        }
                    }
                    break;
                case Chart.collumn.Down:
                    visual.x = 42;
                    visual.y = 44;
                    for (int i = 3; i > -2; i--)
                    {
                        visual.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.Blue, ConsoleColor.Blue));
                        visual.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.Blue, ConsoleColor.Blue));
                        visual.localPositions.Add(new Coords(-1, i, ' ', ConsoleColor.Blue, ConsoleColor.Blue));
                    }
                    for (int i = -2; i > -8; i--)
                    {
                        for (int x = -5 + ((-i) - 2); x < 5 - ((-i) - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(x, i, ' ', ConsoleColor.Blue, ConsoleColor.Blue));
                        }
                    }
                    break;
                case Chart.collumn.Up:
                    visual.x = 56;
                    visual.y = 40;
                    for (int i = -3; i < 2; i++)
                    {
                        visual.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.Red, ConsoleColor.Red));
                        visual.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.Red, ConsoleColor.Red));
                        visual.localPositions.Add(new Coords(-1, i, ' ', ConsoleColor.Red, ConsoleColor.Red));
                    }
                    //visual.localPositions.Add(new Coords(8, 0, 'h', ConsoleColor.DarkGreen, ConsoleColor.DarkCyan));
                    for (int i = 2; i < 8; i++)
                    {
                        for (int x = (-5) + i - 2; x < 5 - (i - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(x, i, ' ', ConsoleColor.Red, ConsoleColor.Red));
                        }
                    }
                    break;
                case Chart.collumn.Right:
                    visual.x = 68;
                    for (int i = -3; i < 2; i++)
                    {
                        visual.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                        visual.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                        visual.localPositions.Add(new Coords(i, -1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    }
                    //visual.localPositions.Add(new Coords(8, 0, 'h', ConsoleColor.DarkGreen, ConsoleColor.DarkCyan));
                    for (int i = 2; i < 8; i++)
                    {
                        for (int x = (-5) + i - 2; x < 5 - (i - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(i, x, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                        }
                    }
                    break;
                default:
                    break;
            }
            beatTime = chart.beat;
            jumpAmount = chart.approachBeat / 47;

            visual.y = visual.y - 100;
            movementAmount = 100;
            components.Add(visual);
            aimY = visual.y;

        }

        public override void Update(double time, Game game)
        {
            float percent = (chart.beat - beatTime) / (noteInfo.time - beatTime);
            if(percent >= 0.5)
            {
                //Console.WriteLine("h");
                visual.y = aimY + (int)Math.Round(percent * movementAmount);
            }
        }
    }
}
