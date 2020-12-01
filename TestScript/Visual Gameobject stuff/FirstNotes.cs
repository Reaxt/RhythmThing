using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Utils;
using RhythmThing.System_Stuff;
using RhythmThing.Objects;
using RhythmThing.Components;
using TestScript.Shaders;

namespace TestScript.Visual_Gameobject_stuff
{
    class FirstNotes : GameObject
    {
        private Chart chart;
        private BoxBuffer boxBuffer = new BoxBuffer();
        private int lastBeat = 60;
        private int lastBeat1 = 94;
        private bool lrtog = false;
        private bool udtog = false;
        private bool lrtog2 = false;
        private Visual flash;
        private int moveamount = 30;
        private int[,] receiverxy = new int[4, 2];
        private int[,] receiverxyt = new int[4, 2];
        private int currenty = 0;
        private bool[] hits = new bool[6];
        private bool[] hits99 = new bool[8];
        private float timetopass = 0;
        private double passed = 0;
        int targRot = 720;
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
            flash = new Visual();
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    flash.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Black, ConsoleColor.Black));
                }
            }
            flash.active = false;
            flash.z = 5;
            components.Add(flash);
            boxBuffer.boxPoint = new int[] { 23, 0 };
            boxBuffer.boxDimensions = new int[] { 53, 20 };
        }

        public override void Update(double time, Game game)
        {
            if(chart.beat >=1)
            if(chart.beat >= 99)
            {
                float stat = chart.beat - 99;
                float interval = 1f / 8f;
                if ((stat >= interval) && !hits99[0])
                {
                    flash.active = true;
                    chart.chartEventHandler.moveCollumn(0, 0, 0);
                    hits99[0] = true;
                }
                if (stat >= interval *2 && !hits99[1])
                {
                    flash.active = false;
                    hits99[1] = true;
                }
                if ((stat >= interval*3) && !hits99[2])
                {
                    flash.active = true;
                    chart.chartEventHandler.moveCollumn(3, 0, 0);
                    hits99[2] = true;
                }
                if (stat >= interval * 4 && !hits99[3])
                {
                    flash.active = false;
                    hits99[3] = true;
                }
                if ((stat >= interval * 5) && !hits99[4])
                {
                    flash.active = true;
                    chart.chartEventHandler.moveCollumn(2, 0, 0);
                    hits99[4] = true;
                }
                if (stat >= interval * 6 && !hits99[5])
                {
                    flash.active = false;
                    hits99[5] = true;
                }
                if ((stat >= interval * 7) && !hits99[6])
                {
                    flash.active = true;
                    chart.chartEventHandler.moveCollumn(1, 0, 0);
                    hits99[6] = true;
                }
                if (stat >= interval * 8 && !hits99[7])
                {
                    flash.active = false;
                    hits99[7] = true;
                }
            }
            if(chart.beat >= 91 && !hits[3])
            {
                for (int i = 0; i < 4; i++)
                {
                    receiverxy[i, 0] = chart.receivers[i].visual.x;
                    receiverxy[i,1] = chart.receivers[i].visual.y;
                    receiverxyt[i, 0] =  random.Next(-30, 30);
                    receiverxyt[i, 1] =  random.Next(-40, -20);
                    chart.receivers[i].visual.useMatrix = true;
                    chart.receivers[i].pressedVisual.useMatrix = true;

                }
                hits[3] = true;
            }
            if (chart.beat >= 91 && !hits[4])
            {
                for (int i = 0; i < 4; i++)
                {

                    int rx = (int)Math.Ceiling( (receiverxyt[i, 0] * Ease.Exponential.Out((chart.beat - 91) / 6)));
                    int ry = (int)Math.Ceiling( (receiverxyt[i, 1] * Ease.Exponential.Out((chart.beat - 91) / 6)));
                    chart.receivers[i].visual.rotation = (int)Math.Ceiling(targRot * Ease.Exponential.Out((chart.beat - 91) / 6));
                    chart.receivers[i].pressedVisual.rotation = (int)Math.Ceiling(targRot * Ease.Exponential.Out((chart.beat - 91) / 6));

                    chart.chartEventHandler.moveCollumn(i, rx, ry);

                }
                if (chart.beat >= 98)
                {
                    hits[4] = true;
                }
            }
            if(chart.beat >= 132.25 && hits[5])
            {
                flash.active = false;
                chart.chartEventHandler.setModPercent("beat", 0);
                chart.chartEventHandler.setModPercent("bumpy", 3);

                chart.chartEventHandler.setMovementAmount(50);
            }
            if (chart.beat >= 91.5 && !hits[5])
            {
                if (chart.beat >= 132)
                {
                    hits[5] = true;
                    game.display.DisableFilter();
                    flash.overrideColor = true;
                    flash.overrideback = ConsoleColor.White;
                    flash.overridefront = ConsoleColor.White;

                    flash.active = true;
                }
                if (!hits[2] && chart.beat >= 100)
                {
                    hits[2] = !hits[2];
                    lastBeat = 92;
                    chart.chartEventHandler.setModPercent("bumpy", 0);
                    chart.chartEventHandler.setModPercent("beat", 2);

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
                            boxBuffer.boxPoint[0] = (int)(23 + (100 * Ease.Sinusoidal.In((chart.beat - (float)lastBeat)*2)));

                        } else
                        {
                            boxBuffer.boxPoint[0] = (int)(-100 + (((100)+23) * Ease.Sinusoidal.Out((float)((chart.beat - (float)lastBeat)-0.5)*2)));

                        }
                    } else
                    {
                        lastBeat += 4;
                    }
                }
                if(chart.beat>= lastBeat1 && !hits[5])
                {
                    if(chart.beat - lastBeat1 <= 2)
                    {
                        if(chart.beat - lastBeat1 <= 1)
                        {
                            boxBuffer.boxDimensions[1] = (int)(60 - (40 * Ease.Back.Out((chart.beat - (float)lastBeat1) )));
                            //boxBuffer.boxPoint[1] = (int)(-17 + (40 * Ease.Back.Out((chart.beat - (float)lastBeat1) )));

                        } else
                        {
                            boxBuffer.boxDimensions[1] = (int)(20 + (40 * Ease.Back.Out((chart.beat - (float)lastBeat1)-1)));
                            //boxBuffer.boxPoint[1] = (int)(23 - (40 * Ease.Back.Out((chart.beat - (float)lastBeat1)-1)));

                        }
                    } else
                    {
                        lastBeat1 += 4;
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
