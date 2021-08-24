using RhythmThing.System_Stuff;
using System;
using RhythmThing.Objects;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using RhythmThing.Utils;
namespace TestScript.Visual_Gameobject_stuff
{
    class Finale : GameObject
    {
        private Chart chart;
        private static int count = 40;
        Random random = new Random();
        private List<Visual> arrowVisuals;
        float[,] circ = RhythmThing.Utils.MathTools.circle(5, count);
        float[,] circ2 = RhythmThing.Utils.MathTools.circle(60, count);
        private Visual textVisual;
        private double timePassed = 0;
        private float timePerLetter = 0.25f;
        private int textIndex;
        private bool[] hits = new bool[2];
        public Finale(Chart chart)
        {
            
            this.chart = chart;
            arrowVisuals = new List<Visual>();
            for (int i = 0; i < count; i++)
            {
                Visual newVis = new Visual();
                newVis.LoadBMPPath("Sprites/UpReceiver.bmp", new int[] { -5, -6 });
                newVis.Active = true;
                newVis.z = -(i + 3);
                int xbase = 50;
                int ybase = 25;
                int[] point = new int[] { (int)(circ[i, 0] + xbase), (int)(circ[i, 1] + ybase) };
                int[] point2 = new int[] { (int)(circ2[i, 0] + xbase), (int)(circ2[i, 1] + ybase) };

                newVis.y = point2[1];
                newVis.x = point2[0];
                int tx = random.Next(0, 100);
                int ty = random.Next(10, 50);
                newVis.overrideColor = true;

                newVis.overridefront = (ConsoleColor)random.Next(0, 12);
                newVis.rotation = random.Next(0, 360);
                newVis.useMatrix = true;
                for (int h = 0; h < 4; h++)
                {

                    chart.chartEventHandler.moveCollumnEase(h, 0,0,0,20,chart.beat,0.5f,"easeInSine");
                }

                
                newVis.Animate(new int[] { newVis.x, newVis.y }, new int[] { point[0], point[1] }, "easeOutExpo", 10f);
                arrowVisuals.Add(newVis);
                this.Components.Add(newVis);
            }
            textVisual = new Visual();
            textVisual.z = 10000;
            textVisual.x = 40;
            textVisual.y = 25;
            textVisual.Active = true;
            Components.Add(textVisual);
        }
        public override void End()
        {

        }

        public override void Start(Game game)
        {

        }

        public override void Update(double time, Game game)
        {
            bool tog = false;
            if (!hits[1])
            {
                foreach (Visual visual in arrowVisuals)
                {
                    if (tog)
                    {
                        visual.rotation++;

                    }
                    else
                    {
                        visual.rotation--;
                    }
                    tog = !tog;
                }

            }
            if (!hits[0])
            {
                timePassed += time;
                if(timePassed >= timePerLetter)
                {
                    timePassed = 0;
                    char[] charArray = "If anyone is out there".ToCharArray();
                    if(charArray.Length > textIndex)
                    {
                        textVisual.localPositions.Add(new Coords(textIndex, 0, charArray[textIndex], ConsoleColor.Black, ConsoleColor.White));
                        textIndex++;
                    } else
                    {
                        hits[0] = true;
                    }
                }
            }
            if(!hits[1])
            {

                if(textIndex > 5)
                {
                    textVisual.localPositions[3] = new Coords(3, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);
                }
                if (textIndex > 7)
                {
                    textVisual.localPositions[5] = new Coords(5, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);
                }
                if (textIndex > 9)
                {
                    textVisual.localPositions[1] = new Coords(1, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);
                    textVisual.localPositions[4] = new Coords(4, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);

                }
                if (textIndex > 8)
                {
                    textVisual.localPositions[7] = new Coords(7, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);
                }
                if (textIndex > 12)
                {
                    textVisual.localPositions[11] = new Coords(11, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);
                }
                if (textIndex > 13)
                {
                    textVisual.localPositions[10] = new Coords(10, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);
                    textVisual.localPositions[9] = new Coords(9, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);

                }
                if (textIndex > 15)
                {
                    textVisual.localPositions[13] = new Coords(13, 0, (char)random.Next(0, 100), ConsoleColor.Black, ConsoleColor.White);
                }
            }
            if(chart.beat > 308.2 && !hits[1])
            {
                textVisual.localPositions.Clear();
                textVisual.writeText(-35, -5, "_ host [127.0.0.1] has sent a potentially hazardous data stream... closing connection...//", ConsoleColor.Red, ConsoleColor.Black);
                hits[1] = true;
            }
        }
        
    }
}
