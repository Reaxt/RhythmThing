using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using TestScript.Shaders;

namespace TestScript.Visual_Gameobject_stuff
{
    class CollumnWindows : GameObject
    {
        private Chart chart;
        private SlaveManager[] slaveWindows;
        private Yoinkers yoinked;
        bool go = false;
        int[] xValues = new int[4];
        bool[] hits = new bool[10];
        int[,] positions = new int[4, 2];
        bool tog = false;
        float num = 200;
        int lastBeat = 0;
        int circStep = 16;

        public override void End()
        {
            
        }
        public CollumnWindows(SlaveManager[] slaves, Chart chart)
        {
            this.chart = chart;
            slaveWindows = slaves;
            Visual beep = new Visual();
            beep.LoadBMPPath("!Songs/Broadcast/broadcast.bmp");
            yoinked = new Yoinkers(beep);
            for (int i = 0; i < 4; i++)
            {
                int startX = 600;

                xValues[i] = startX + (i * (TestScript.colX * (-23)));
                slaveWindows[i].visuals.Add(yoinked.cols[i]);

            }
            Game.mainInstance.display.ActivateFilter(yoinked);
        }
        public override void Start(Game game)
        {
            positions[0, 0] = xValues[0] + (int)num;
            positions[0, 1] = 50;
            positions[1, 0] = xValues[1] + (int)num/2;
            positions[1, 1] = 50;
            positions[2, 0] = xValues[2] - (int)num/2;
            positions[2, 1] = 50;
            positions[3, 0] = xValues[3] - (int)num;
            positions[3, 1] = 50;


        }

        public override void Update(double time, Game game)
        {

            if(chart.beat >= 132 && !hits[0])
            {
                slaveWindows[0].MoveWindowEase(xValues[0], -450, xValues[0], -300, 0.5f, "easeOutExpo");
                slaveWindows[1].MoveWindowEase(xValues[1], -450, xValues[1], -300, 0.5f, "easeOutExpo");
                slaveWindows[2].MoveWindowEase(xValues[2], -450, xValues[2], -300, 0.5f, "easeOutExpo");
                slaveWindows[3].MoveWindowEase(xValues[3], -450, xValues[3], -300, 0.5f, "easeOutExpo");

                hits[0] = true;
            }
            if (chart.beat >= 134 && !hits[1])
            {
                slaveWindows[0].MoveWindowEase(xValues[0], -300, xValues[0], -250, 0.25f, "easeOutExpo");
                slaveWindows[1].MoveWindowEase(xValues[1], -300, xValues[1], -250, 0.25f, "easeOutExpo");
                slaveWindows[2].MoveWindowEase(xValues[2], -300, xValues[2], -250, 0.25f, "easeOutExpo");
                slaveWindows[3].MoveWindowEase(xValues[3], -300, xValues[3], -250, 0.25f, "easeOutExpo");
                hits[1] = true;
            }
            if (chart.beat >= 136 && !hits[2])
            {
                slaveWindows[0].MoveWindowEase(xValues[0], -250, xValues[0], -200, 0.25f, "easeOutExpo");
                slaveWindows[1].MoveWindowEase(xValues[1], -250, xValues[1], -200, 0.25f, "easeOutExpo");
                slaveWindows[2].MoveWindowEase(xValues[2], -250, xValues[2], -200, 0.25f, "easeOutExpo");
                slaveWindows[3].MoveWindowEase(xValues[3], -250, xValues[3], -200, 0.25f, "easeOutExpo");
                hits[2] = true;
            }
            if (chart.beat >= 144 && !hits[3])
            {
                slaveWindows[0].MoveWindowEase(xValues[0], -200, xValues[0], -450, 0.25f, "easeInQuad");
                slaveWindows[1].MoveWindowEase(xValues[1], -200, xValues[1], -450, 0.5f, "easeInQuad");
                slaveWindows[2].MoveWindowEase(xValues[2], -200, xValues[2], -450, 0.75f, "easeInQuad");
                slaveWindows[3].MoveWindowEase(xValues[3], -200, xValues[3], -450, 1, "easeInQuad");
                hits[3] = true;
            }
            if (chart.beat >= 148 && !hits[4])
            {
                slaveWindows[0].MoveWindowEase(1000+xValues[0], 50-300, xValues[0], 50, 0.5f, "easeOutExpo");
                hits[4] = true;
            }
            if(chart.beat >= 152 && !hits[5])
            {
                slaveWindows[3].MoveWindowEase(xValues[3] - 1000, 50 + 300, xValues[3], 50, 0.5f, "easeOutExpo");
                hits[5] = true;

            }
            if (chart.beat >= 156 && !hits[6])
            {
                slaveWindows[1].MoveWindowEase(xValues[1], 500, xValues[1], 50, 0.5f, "easeOutQuad");
                slaveWindows[2].MoveWindowEase(xValues[2], -500, xValues[2], 50, 0.5f, "easeOutQuad");
                hits[6] = true;

            }
            if (chart.beat >= 162 && !hits[7])
            {

                slaveWindows[0].MoveWindowEase(xValues[0], 50, xValues[0] + (num), 50, 0.5f, "easeOutExpo");
                slaveWindows[1].MoveWindowEase(xValues[1], 50, xValues[1] + (num/2), 100, 0.5f, "easeOutExpo");
                slaveWindows[2].MoveWindowEase(xValues[2], 50, xValues[2] - (num/2), 0, 0.5f, "easeOutExpo");
                slaveWindows[3].MoveWindowEase(xValues[3], 50, xValues[3] - (num), 50, 0.5f, "easeOutExpo");
                //chart.chartEventHandler.moveWindowEase(50, 50, 50, -500, chart.beat, 3, "easeInBack");
                //Arrow.movementAmount = 100;
                chart.chartEventHandler.setModPercent("bumpy", 0);
                hits[7] = true;

            }
            if (chart.beat >= 163 && !hits[8])
            {

                slaveWindows[1].MoveWindowEase(xValues[1] + (num / 2), 50, xValues[1]+(num/2), 50, 0.5f, "easeOutExpo");
                slaveWindows[2].MoveWindowEase(xValues[2] - (250), 50, xValues[2]-(num/2), 50, 0.5f, "easeOutExpo");
                yoinked.Yoink = true;


                hits[8] = true;

            }
            if(chart.beat >= 164)
            {
                if(chart.beat > lastBeat)
                {
                    for (int i = 0; i < 4; i++)
                    {

                        int gogo = (i + circStep) % 4;
                        int last = (gogo - 1);
                        if(last == -1)
                        {
                            last = 3;
                        }
                        //slaveWindows[i].MoveWindowEase(positions[last, 0], positions[last, 1], positions[gogo, 0], positions[gogo, 1], 0.25f, "easeInOutSine");
                    }
                    float dur = 0.125f;
                    string easing = "easeInOutExpo";
                    if (tog)
                    {
                        slaveWindows[0].MoveWindowEase(positions[0, 0], positions[0, 1], positions[3, 0], positions[3, 1], dur, easing);
                        slaveWindows[1].MoveWindowEase(positions[1, 0], positions[1, 1], positions[2, 0], positions[2, 1], dur, easing);
                        slaveWindows[2].MoveWindowEase(positions[2, 0], positions[2, 1], positions[1, 0], positions[1, 1], dur, easing);
                        slaveWindows[3].MoveWindowEase(positions[3, 0], positions[3, 1], positions[0, 0], positions[0, 1], dur, easing);
                    } else
                    {
                        slaveWindows[3].MoveWindowEase(positions[0, 0], positions[0, 1], positions[3, 0], positions[3, 1], dur, easing);
                        slaveWindows[2].MoveWindowEase(positions[1, 0], positions[1, 1], positions[2, 0], positions[2, 1], dur, easing);
                        slaveWindows[1].MoveWindowEase(positions[2, 0], positions[2, 1], positions[1, 0], positions[1, 1], dur, easing);
                        slaveWindows[0].MoveWindowEase(positions[3, 0], positions[3, 1], positions[0, 0], positions[0, 1], dur, easing);
                    }
                    tog = !tog;
                    circStep++;
                    lastBeat = (int)Math.Ceiling(chart.beat);
                }
            }

            /*
            if(chart.beat >= 15 && !go)
            {
                for (int i = 0; i < 4; i++)
                {
                    int startX = 600;

                    slaveWindows[i].MoveWindow(startX + (i * (TestScript.colX * (-23))), 50);
                    slaveWindows[i].visuals.Add(yoinked.cols[i]);
                }
                go = true;
            }*/
            for (int i = 0; i < 4; i++)
            {
                slaveWindows[i].UpdateVisualsAsync();
            }
        }
    }
}
