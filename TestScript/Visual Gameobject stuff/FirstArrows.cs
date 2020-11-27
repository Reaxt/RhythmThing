using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing;
using RhythmThing.System_Stuff;
using RhythmThing.Components;
using RhythmThing.Objects;
namespace TestScript.Visual_Gameobject_stuff
{

    class FirstArrows : GameObject
    {
        private List<Visual> arrowVisuals;
        private Chart chart;
        private Visual textvisual;
        private Random random = new Random();
        private bool[] hits = new bool[8];
        private float textStep = 0.10f;
        private float timePassed = 0f;
        private int textIndex = 0;
        private bool circBump = false;
        Visual beep = new Visual();
        public override void End()
        {

        }
        public FirstArrows(Chart chart)
        {
            this.chart = chart;
            textvisual = new Visual();
            textvisual.active = true;
            textvisual.x = 30;
            textvisual.y = 25;
            textvisual.z = -2;
            components.Add(textvisual);
            beep.LoadBMPPath("!Songs/Broadcast/broadcast.bmp");
            beep.z = 0;
            beep.y = -1;
            beep.z = -1;
            components.Add(beep);
            arrowVisuals = new List<Visual>();
            int amount = 20;
            for (int i = 0; i < amount; i++)
            {
                Visual newVis = new Visual();
                newVis.LoadBMPPath("Sprites/UpReceiver.bmp", new int[] { -5, -6 });
                newVis.active = true;
                newVis.z = -(i+3);
                newVis.y = -20;
                newVis.x = (int)(((float)i / (float)amount) * 100f);
                int tx = random.Next(0, 100);
                int ty = random.Next(10, 50);
                newVis.overrideColor = true;

                newVis.overridefront = (ConsoleColor)random.Next(0, 12);
                newVis.rotation = random.Next(0, 360);
                newVis.Animate(new int[] { newVis.x, newVis.y }, new int[] { tx, ty }, "easeOutExpo", 5f);
                arrowVisuals.Add(newVis);
                this.components.Add(newVis);
            }
        }
        public override void Start(Game game)
        {

        }

        public override void Update(double time, Game game)
        {
            bool tog = false;
            foreach (Visual visual in arrowVisuals)
            {
                visual.useMatrix = true;
                if (tog)
                {
                    visual.rotation++;

                } else
                {
                    visual.rotation--;
                }
                tog = !tog;
            }
            if(chart.beat >= 11)
            {
                if (!circBump)
                {
                    float[,] circ = RhythmThing.Utils.MathTools.circle(30, 20);
                    int xbase = 50;
                    int ybase = 25;
                    for (int i = 0; i < 20; i++)
                    {
                        if (i == 19)
                        {
                            int[] point = new int[] { (int)(circ[i, 0] + xbase), (int)(circ[i, 1] + ybase) };
                            arrowVisuals[0].Animate(new int[] { arrowVisuals[0].x, arrowVisuals[0].y }, point, "easeOutExpo", 5);
                        }
                        else
                        {
                            int[] point = new int[] { (int)(circ[i, 0] + xbase), (int)(circ[i, 1] + ybase) };
                            arrowVisuals[i + 1].Animate(new int[] { arrowVisuals[i+1].x, arrowVisuals[i+1].y }, point, "easeOutExpo", 5);
                        }

                    }
                    circBump = true;

                }
                if (!hits[0])
                {
                    char[] text = "if anyone is out there".ToCharArray();
                    timePassed += (float)time;
                    if(timePassed >= textStep)
                    {
                        int baseX = 0;
                        int baseY = 0;
                        timePassed = 0;
                        textvisual.localPositions.Add(new Coords(baseX + textIndex, baseY, text[textIndex], ConsoleColor.Black, ConsoleColor.White));
                        textIndex++;
                        if(textIndex == text.Length)
                        {
                            hits[0] = true;
                            textIndex = 0;

                        }
                    }
                    
                }
            }
            if(chart.beat >= 21)
            {
                if (!hits[1])
                {
                    textStep = 0.20f;
                    char[] text = "please".ToCharArray();
                    timePassed += (float)time;
                    if (timePassed >= textStep)
                    {
                        int baseX = 0;
                        int baseY = -1;
                        timePassed = 0;
                        textvisual.localPositions.Add(new Coords(baseX + textIndex, baseY, text[textIndex], ConsoleColor.Black, ConsoleColor.White));
                        textIndex++;
                        if (textIndex == text.Length)
                        {
                            hits[1] = true;
                            textIndex = 0;

                        }
                    }

                }
            }
            if (chart.beat >= 26)
            {
                if (!hits[2])
                {
                    textStep = 0.10f;
                    char[] text = "save us".ToCharArray();
                    timePassed += (float)time;
                    if (timePassed >= textStep)
                    {
                        int baseX = 0;
                        int baseY = -2;
                        timePassed = 0;
                        textvisual.localPositions.Add(new Coords(baseX + textIndex, baseY, text[textIndex], ConsoleColor.Black, ConsoleColor.White));
                        textIndex++;
                        if (textIndex == text.Length)
                        {
                            hits[2] = true;
                            textIndex = 0;

                        }
                    }

                }
            }
            if (chart.beat >= 31)
            {
                if (!hits[3])
                {
                    textStep = 0.10f;
                    char[] text = "we have nothing".ToCharArray();
                    timePassed += (float)time;
                    if (timePassed >= textStep)
                    {
                        int baseX = 0;
                        int baseY = -3;
                        timePassed = 0;
                        textvisual.localPositions.Add(new Coords(baseX + textIndex, baseY, text[textIndex], ConsoleColor.Black, ConsoleColor.White));
                        textIndex++;
                        if (textIndex == text.Length)
                        {
                            hits[3] = true;
                            textIndex = 0;

                        }
                    }

                }
            }
            if (chart.beat >= 38)
            {
                if (!hits[4])
                {
                    textStep = 0.08f;
                    char[] text = "dont leave us with the things that lurk".ToCharArray();
                    timePassed += (float)time;
                    if (timePassed >= textStep)
                    {
                        int baseX = 0;
                        int baseY = -4;
                        timePassed = 0;
                        textvisual.localPositions.Add(new Coords(baseX + textIndex, baseY, text[textIndex], ConsoleColor.Black, ConsoleColor.White));
                        textIndex++;
                        if (textIndex == text.Length)
                        {
                            hits[4] = true;
                            textIndex = 0;

                        }
                    }

                }
            }
            if (chart.beat >= 47)
            {
                if (!hits[5])
                {
                    textStep = 0.08f;
                    char[] text = "in the darkness".ToCharArray();
                    timePassed += (float)time;
                    if (timePassed >= textStep)
                    {
                        int baseX = 0;
                        int baseY = -5;
                        timePassed = 0;
                        textvisual.localPositions.Add(new Coords(baseX + textIndex, baseY, text[textIndex], ConsoleColor.White, ConsoleColor.Black));
                        textIndex++;
                        if (textIndex == text.Length)
                        {
                            hits[5] = true;
                            textIndex = 0;

                        }
                    }

                }
            }
            if(chart.beat >= 53)
            {
                if(textIndex >= textvisual.localPositions.Count)
                {
                    textIndex = 0;
                }
                textvisual.localPositions[textIndex] = new Coords(textvisual.localPositions[textIndex].x, textvisual.localPositions[textIndex].y, (char)random.Next(0, 100), textvisual.localPositions[textIndex].foreColor, textvisual.localPositions[textIndex].backColor);

                textIndex++;

                if (!hits[6])
                {
                    float[,] circ = RhythmThing.Utils.MathTools.circle(-70, 20);
                    int xbase = 50;
                    int ybase = 25;
                    for (int i = 0; i < 20; i++)
                    {
                        int[] point = new int[] { (int)(circ[i, 0] + xbase), (int)(circ[i, 1] + ybase) };
                        arrowVisuals[i].Animate(new int[] { arrowVisuals[i].x, arrowVisuals[i].y }, point, "easeInQuad", 2);
                        circBump = true;
                    }
                    hits[6] = true;
                }
            }
            if(chart.beat >= 59)
            {
                if (!hits[7])
                {
                    for (int i = 0; i < 4; i++)
                    {
                        chart.chartEventHandler.moveCollumn(i, 0, 0);
                        chart.receivers[i].visual.overrideColor = true;
                        chart.receivers[i].visual.overridefront = ConsoleColor.White;
                        chart.receivers[i].visual.overrideback = ConsoleColor.White;

                    }
                    hits[7] = true;
                    beep.active = true;

                }
            }
            if(chart.beat >= 60)
            {
                for (int i = 0; i < 4; i++)
                {
                    chart.receivers[i].visual.overrideColor = false;

                }
                chart.chartEventHandler.setModPercent("beat", 1);
                this.alive = false;
            }
        }
    }
}
