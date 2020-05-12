using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System.Runtime.InteropServices;

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
        private ConsoleColor noteColor;
        private ConsoleColor noteBGColor;
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

            // Check the beat timing, set to red for quarter notes, blue for eighth notes, green for sixteenth notes, purple for triplets, yellow for anything else
            if (noteInfo.time % 1 == 0)
            {
                noteColor = ConsoleColor.Red;
            }
            else if (noteInfo.time % 1 == 0.5)
            {
                noteColor = ConsoleColor.Blue;
            }
            else if (noteInfo.time % 1 == 0.25 || noteInfo.time % 1 == 0.75)
            {
                noteColor = ConsoleColor.Green;
            }
            else if ((float)Math.Round(noteInfo.time % 1, 3) == 0.333f || (float)Math.Round(noteInfo.time % 1, 3) == 0.666f)
            {
                noteColor = ConsoleColor.DarkMagenta;
            }
            else
            {
                noteColor = ConsoleColor.Yellow;
            }

            noteBGColor = noteColor;

            switch (collumn)
            {
                case Chart.collumn.Left:
                    for (int i = 3; i > -2; i--)
                    {
                        visual.localPositions.Add(new Coords(i, 0, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(i, 1, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(i, -1, ' ', noteColor, noteBGColor));
                    }
                    for (int i = -2; i > -8; i--)
                    {
                        for (int x = -5 + ((-i) - 2); x < 5 - ((-i) - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(i, x, ' ', noteColor, noteBGColor));
                        }
                    }
                    break;
                case Chart.collumn.Down:
                    visual.x = 42;
                    visual.y = 44;
                    for (int i = 3; i > -2; i--)
                    {
                        visual.localPositions.Add(new Coords(0, i, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(1, i, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(-1, i, ' ', noteColor, noteBGColor));
                    }
                    for (int i = -2; i > -8; i--)
                    {
                        for (int x = -5 + ((-i) - 2); x < 5 - ((-i) - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(x, i, ' ', noteColor, noteBGColor));
                        }
                    }
                    break;
                case Chart.collumn.Up:
                    visual.x = 56;
                    visual.y = 40;
                    for (int i = -3; i < 2; i++)
                    {
                        visual.localPositions.Add(new Coords(0, i, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(1, i, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(-1, i, ' ', noteColor, noteBGColor));
                    }
                    //visual.localPositions.Add(new Coords(8, 0, 'h', ConsoleColor.DarkGreen, ConsoleColor.DarkCyan));
                    for (int i = 2; i < 8; i++)
                    {
                        for (int x = (-5) + i - 2; x < 5 - (i - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(x, i, ' ', noteColor, noteBGColor));
                        }
                    }
                    break;
                case Chart.collumn.Right:
                    visual.x = 68;
                    for (int i = -3; i < 2; i++)
                    {
                        visual.localPositions.Add(new Coords(i, 0, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(i, 1, ' ', noteColor, noteBGColor));
                        visual.localPositions.Add(new Coords(i, -1, ' ', noteColor, noteBGColor));
                    }
                    //visual.localPositions.Add(new Coords(8, 0, 'h', ConsoleColor.DarkGreen, ConsoleColor.DarkCyan));
                    for (int i = 2; i < 8; i++)
                    {
                        for (int x = (-5) + i - 2; x < 5 - (i - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(i, x, ' ', noteColor, noteBGColor));
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
            visual.y = (int)(aimY - Math.Round((percent / chart.firstBPM) * 60 * movementAmount));
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

            //beat! Mod code ported from OpenITG
            float beatModAccelTime = 0.2f;
            float beatModTotalTime = 0.5f;
            // Slow it down if the song is too fast, otherwise things look wrong
            float beatModDivRate = Math.Max(1.0f, (float)(Math.Truncate(chart.chartInfo.bpm / 150.0f)));

            // Speed up the time the beat occurs over, otherwise it starts to overlap
            beatModAccelTime /= beatModDivRate;
            beatModTotalTime /= beatModDivRate;

            float beatModBeat = chart.beat;
            beatModBeat /= beatModDivRate;

            // False if the beat is even, true if beat is odd
            bool beatModEvenBeat = ((int)(beatModBeat) % 2) != 0;

            // Only start if we're slightly past the start of the beat
            if (beatModBeat >= 0)
            {   
                // Get the fractional component of the beat, and take absolute value
                beatModBeat -= (int)Math.Truncate(beatModBeat);
                beatModBeat += 1;
                beatModBeat -= (int)Math.Truncate(beatModBeat);

                // Check to make sure we haven't finished the mod calculation for this beat yet
                if (beatModBeat < beatModTotalTime)
                {
                    float beatModAmount;

                    // If we haven't finished the startup acceleration, do that scaling first
                    if (beatModBeat < beatModAccelTime)
                    {
                        // Scale the amount to the time we accelerate outwards
                        beatModAmount = beatModBeat / beatModAccelTime;
                        beatModAmount *= beatModAmount;
                    } else
                    {
                        // Scale the amount to the time we accelerate backwards
                        beatModAmount = ((beatModBeat - beatModAccelTime) * (0.0f - 1.0f) / (beatModTotalTime - beatModAccelTime)) + 1.0f;
                        // Invert and square beatmodamount
                        beatModAmount = 1 - (1 - beatModAmount) * (1 - beatModAmount);
                    }

                    if (beatModEvenBeat)
                    {
                        // Go the other way on even beats
                        beatModAmount *= -1;
                    }

                    // Use the amount to scale a fast sin wave, so things beat 
                    // back and forth differently depending on the kind of note.
                    float beatModShift = 10.0f * beatModAmount * (float)Math.Sin(percent * 2 * Math.PI + Math.PI / 2.0f);

                    // We're done!
                    xModOffset += (int)(mods["beat"] * beatModShift);
                }
            }
            
            actualY = (int)(aimY - Math.Cos(angle * 2 * Math.PI) * (Math.Round((percent / chart.firstBPM) * 60 * movementAmount)));
            //angle calculation done by Nytlaz because I Can Not Math
            visual.x = actualX + xOffset - (int)(Math.Sin(angle * 2 * Math.PI) * (Math.Round((percent / chart.firstBPM) * 60 * movementAmount))) + xModOffset;
            visual.y = actualY + yOffset + yModOffset;
        }
    }
}
