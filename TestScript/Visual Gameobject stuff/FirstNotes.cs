using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Utils;
using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using TestScript.Shaders;

namespace TestScript.Visual_Gameobject_stuff
{
    class FirstNotes : GameObject
    {
        private Chart chart;
        private BoxBuffer boxBuffer = new BoxBuffer();
        private int lastBeat = 60;
        private bool lrtog = false;
        private bool udtog = false;
        private bool lrtog2 = false;
        private int moveamount = 30;
        private int currenty = 0;
        private bool[] hits = new bool[3];
        private float timetopass = 0;
        private double passed = 0;
        int go = 0;

        private Random random = new Random();
        public FirstNotes(Chart chart)
        {
            this.chart = chart;
        }
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            boxBuffer.boxPoint = new int[] { 23, 15 };
            boxBuffer.boxDimensions = new int[] { 53, 20 };
        }

        public override void Update(double time, Game game)
        {
            if(chart.beat >= 91.5)
            {
                if (!hits[2])
                {
                    hits[2] = !hits[2];
                    lastBeat = 92;
                    game.display.ActivateFilter(boxBuffer);
                }
                passed += time;
                /*if(passed>= timetopass)
                {
                    boxBuffer.boxPoint[1]++;
                    passed = 0;
                    if(boxBuffer.boxPoint[1] >= 50)
                    {
                        boxBuffer.boxPoint[1] = -20;
                    }
                }*/
                if(chart.beat >= lastBeat)
                {

                    if(chart.beat - lastBeat <= 1)
                    {
                        if (chart.beat - lastBeat <= 0.5)
                        {
                            boxBuffer.boxPoint[0] = (int)(23 + (23 * Ease.Sinusoidal.In((chart.beat - (float)lastBeat)*2)));

                        } else
                        {
                            boxBuffer.boxPoint[0] = (int)(0 + (23 * Ease.Sinusoidal.Out((float)(chart.beat - (float)lastBeat-0.5)*2)));

                        }
                    } else
                    {
                        lastBeat += 2;
                    }
                }
            }
            if (chart.beat >= 84 && !hits[1])
            {
                hits[1] = true;
                for (int i = 0; i < 4; i++)
                {
                    chart.chartEventHandler.moveCollumnEase(i, 0, currenty, 0, 0, chart.beat, 0.5f, "easeOutExpo");
                    chart.chartEventHandler.setCollumnAngleEase(i, 180, 0, chart.beat, 0.5f, "easeOutExpo");
                    
                }
            }

            if (chart.beat >= lastBeat && chart.beat <= 84)
            {
                if (chart.beat - lastBeat <= 1)
                {
                    

                    for (int i = 0; i < 4; i++)
                    {
                        go = lrtog ? -moveamount : moveamount;

                        go = (int)((float)go * Ease.Sinusoidal.Out(chart.beat - (float)lastBeat));
                        chart.chartEventHandler.moveCollumn(i, go, currenty);
                    }
                    lrtog = !lrtog;

                }
                else
                {
                    if (chart.beat >= 75 && !hits[0])
                    {
                        hits[0] = true;
                        for (int i = 0; i < 4; i++)
                        {
                            go = lrtog2 ? -moveamount : moveamount;
                            chart.chartEventHandler.moveCollumnEase(i, go, currenty, 0, -40, chart.beat, 0.5f, "easeOutExpo");
                            chart.chartEventHandler.setCollumnAngleEase(i, 0, 180, chart.beat, 0.5f, "easeOutExpo");
                            lrtog2 = !lrtog2;
                        }
                        currenty = -40;
                    } else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            go = lrtog2 ? -moveamount : moveamount;
                            chart.chartEventHandler.moveCollumnEase(i, go, currenty, 0, currenty, chart.beat, 0.5f, "easeOutExpo");
                            lrtog2 = !lrtog2;
                        }
                    }
                    lastBeat += 2;

                }
            }

        }
    }
}
