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
        public int yOffset = 0;
        public int xOffset = 0;
        public int xModOffset = 0;
        public int yModOffset = 0;
        public float angle = 0;
        private int actualX;
        private int actualY;
        private int parX;
        public Dictionary<string, float> mods;
        private Chart chart;
        private float jumpAmount;
        private float beatTime;
        //static value thats nice
        public static float movementAmount = 100;
        
        //this needs to be changed
        public enum direction
        {
            down,
            up,
            left,
            right
        }
        public direction dir = direction.down;
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
            visual.z = 2;
            

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

            //visual.y = visual.y - 100;
            //movementAmount = 100;
            //movementAmount = 100;
            components.Add(visual);
            aimY = visual.y;
            actualX = visual.x;
            actualY = visual.y;
            float percent = noteInfo.time - chart.beat;
            visual.y = (int)(aimY - Math.Round((percent / chart.chartInfo.bpm) * 60 * movementAmount));
            actualY = visual.y;
            visual.x = actualX + xOffset;
            visual.y = actualY + yOffset;
        }

        public override void Update(double time, Game game)
        {
            /*
            float percent = (chart.beat - beatTime) / (noteInfo.time - beatTime);
            if(percent >= 0.5)
            {

                        visual.y = aimY + (int)Math.Round(percent * movementAmount);


            }
            actualY = visual.y;
            visual.x = actualX + xOffset;
            visual.y = actualY + yOffset; */
            
            float percent = noteInfo.time - chart.beat;

            //set x and ymodoffset
            xModOffset = 0;
            yModOffset = 0;
            //bumpy!
            xModOffset += (int)(mods["bumpy"] * Math.Sin(percent * 2 * Math.PI * 1));
            yModOffset += (int)(mods["wave"] * 3 * Math.Cos(percent * 2 * Math.PI * 2));
            
            actualY = (int)(aimY - Math.Cos(angle * 2 * Math.PI) * (Math.Round((percent / chart.chartInfo.bpm) * 60 * movementAmount)));
            //angle calculation done by Nytlaz because I Can Not Math
            visual.x = actualX + xOffset - (int)(Math.Sin(angle * 2 * Math.PI) * (Math.Round((percent / chart.chartInfo.bpm) * 60 * movementAmount))) + xModOffset;
            visual.y = actualY + yOffset + yModOffset;
        }
    }
}
