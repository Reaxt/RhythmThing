using RhythmThing.Components;
using RhythmThing.Objects;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Text;
using TestScript.Shaders;
using RhythmThing.Utils;

namespace TestScript.Visual_Gameobject_stuff
{
    class LastNotes : GameObject
    {
        private Chart chart;
        private BoxBuffer boxBuffer = new BoxBuffer();
        private Visual flash;
        private bool flashStatus;
        private float lastFlashBeat = 0;
        private Random random = new Random();
        private bool[] hits = new bool[5];
        private int lastBeat = 60;
        private int lastBeat1 = 94;
        public LastNotes(Chart chart)
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
                    flash.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.White, ConsoleColor.White));
                }
            }
            flash.active = false;
            flash.z = 5;
            components.Add(flash);
            boxBuffer.boxPoint = new int[] { 23, 0 };
            boxBuffer.boxDimensions = new int[] { 53, 60 };
        }

        public override void Update(double time, Game game)
        {
            if(chart.beat >= 225 && !hits[0])
            {
                hits[0] = true;
                boxBuffer.boxPoint = new int[] { 23, -60 };
                game.display.ActivateFilter(boxBuffer);
            }
            if(chart.beat >= 225 && hits[0] && !hits[1])
            {

                float dur = 2;
                boxBuffer.boxPoint[1] = (-60)+ (int)Math.Ceiling(60* Ease.Sinusoidal.Out((chart.beat - 225) / dur));
                if((chart.beat-225) >= dur)
                {
                    hits[1] = true;
                    boxBuffer.boxPoint[1] = 0;
                }
            }

            if (chart.beat >= 259 && !hits[3])
            {
                if (chart.beat >= lastFlashBeat + (0.25/2))
                {
                    flash.active = !flash.active;
                    flashStatus = flash.active;
                    lastFlashBeat = chart.beat;
                }
                if (chart.beat >= 260)
                {
                    flash.active = false;
                    flashStatus = flash.active;
                    hits[3] = true;
                }
                //flash.active = true;
            }
            if (chart.beat >= 228 && (chart.beat <= 258 || hits[3]) && chart.beat < 292)
            {

                if (!hits[2])
                {
                    chart.chartEventHandler.setModPercent("beat", 2);
                    chart.chartEventHandler.setModPercent("bumpy", 2);
                    Arrow.movementAmount = 75;
                }
                if (chart.beat >= lastBeat)
                {

                    if (chart.beat - lastBeat <= 1)
                    {
                        if (chart.beat - lastBeat <= 0.5)
                        {
                            boxBuffer.boxPoint[0] = (int)(23 + (100 * Ease.Sinusoidal.In((chart.beat - (float)lastBeat) * 2)));

                        }
                        else
                        {
                            boxBuffer.boxPoint[0] = (int)(-100 + (((100) + 23) * Ease.Sinusoidal.Out((float)((chart.beat - (float)lastBeat) - 0.5) * 2)));

                        }
                    }
                    else
                    {
                        lastBeat += 4;
                    }
                }
                if (chart.beat >= lastBeat1)
                {
                    if (chart.beat - lastBeat1 <= 2)
                    {
                        if (chart.beat - lastBeat1 <= 1)
                        {
                            boxBuffer.boxDimensions[1] = (int)(60 - (40 * Ease.Back.Out((chart.beat - (float)lastBeat1))));
                            //boxBuffer.boxPoint[1] = (int)(-17 + (40 * Ease.Back.Out((chart.beat - (float)lastBeat1) )));

                        }
                        else
                        {
                            boxBuffer.boxDimensions[1] = (int)(20 + (40 * Ease.Back.Out((chart.beat - (float)lastBeat1) - 1)));
                            //boxBuffer.boxPoint[1] = (int)(23 - (40 * Ease.Back.Out((chart.beat - (float)lastBeat1)-1)));

                        }
                    }
                    else
                    {
                        lastBeat1 += 4;
                    }

                }
            }
            if(chart.beat >= 291.5)
            {
                //flash.active = true;
            }
            if(chart.beat >= 292)
            {
                chart.chartEventHandler.setModPercent("bumpy", 0);
                chart.chartEventHandler.setModPercent("beat", 0);
                game.display.DisableFilter();
                this.alive = false;
            }
        }
    }
}
