using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;
using RhythmThing.Components;
using RhythmThing.Objects;
namespace RhythmThing.Objects
{
    public class Receiver : GameObject
    {
        private double currenttime = 0;
        private Visual visual;
        public Chart.collumn collumn;
        private Chart chart;
        private List<Chart.NoteInfo> notes;
        private List<Chart.NoteInfo> spawnedNotes;
        private List<Arrow> deadNotes;
        private List<Arrow> arrows;
        private bool nMiss = false;
        public override void End()
        {
        }
        public Receiver(Chart.collumn col, List<Chart.NoteInfo> noteInfos, Chart chart)
        {
            this.chart = chart;
            collumn = col;
            notes = new List<Chart.NoteInfo>(noteInfos);
            spawnedNotes = new List<Chart.NoteInfo>();
            deadNotes = new List<Arrow>();
            arrows = new List<Arrow>();


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
            visual.z = -1;

            //holy fuck never do that again
            //rumour has it there was once a disgusting block of code here
            //this is still kinda shit...

            switch (collumn)
            {
                case Chart.collumn.Left:
                    for (int i = 2; i > -2; i--)
                    {
                        visual.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(i, -1, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                    }
                    for (int i = -2; i > -8; i--)
                    {
                        for (int x = -5 + ((-i) - 2); x < 5 - ((-i) - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(i, x, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        }
                    }
                    break;
                case Chart.collumn.Down:
                    visual.x = 42;
                    visual.y = 44;
                    for (int i = 3; i > -2; i--)
                    {
                        visual.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(-1, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                    }
                    for (int i = -2; i > -8; i--)
                    {
                        for (int x = -5 + ((-i) - 2); x < 5 - ((-i) - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(x, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        }
                    }
                    break;
                case Chart.collumn.Up:
                    visual.x = 56;
                    visual.y = 40;
                    for (int i = -3; i < 2; i++)
                    {
                        visual.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(-1, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                    }
                    //visual.localPositions.Add(new Coords(8, 0, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkCyan));
                    for (int i = 2; i < 8; i++)
                    {
                        for (int x = (-5) + i - 2; x < 5 - (i - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(x, i, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        }
                    }
                    break;
                case Chart.collumn.Right:
                    visual.x = 68;
                    for (int i = -3; i < 2; i++)
                    {
                        visual.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        visual.localPositions.Add(new Coords(i, -1, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                    }
                    //visual.localPositions.Add(new Coords(8, 0, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkCyan));
                    for (int i = 2; i < 8; i++)
                    {
                        for (int x = (-5) + i - 2; x < 5 - (i - 3); x++)
                        {
                            visual.localPositions.Add(new Coords(i, x, ' ', ConsoleColor.Gray, ConsoleColor.Gray));
                        }
                    }
                    break;
                default:
                    break;
            }

            components.Add(visual);


        }

        public override void Update(double time, Game game)
        {
            nMiss = false;
            foreach (var note in notes)
            {
                if(chart.beat >= note.time-chart.approachBeat)
                {
                    //Console.WriteLine(collumn);
                    //spawn the note 
                    Arrow temparrow = new Arrow(collumn, note, this.chart);
                    game.addGameObject(temparrow);
                    arrows.Add(temparrow);
                    spawnedNotes.Add(note);
                }
            }
            foreach (var note in spawnedNotes)
            {
                notes.Remove(note);
            }
            spawnedNotes.Clear();
            //more to come
            //here it come
            foreach (var item in arrows)
            {

                if(chart.beat >= (item.noteInfo.time-chart.scoreTime) && chart.beat <= (item.noteInfo.time + chart.scoreTime))
                {
                    //time to HITE
                    //have to write this after
                    //Console.WriteLine("HIT");
                    bool hit = false;
                    switch (collumn)
                    {
                        case Chart.collumn.Left:
                            if (Input.leftKey == Input.buttonState.press)
                                hit = true;
                            
                            break;
                        case Chart.collumn.Down:
                            if (Input.downKey == Input.buttonState.press)
                                hit = true;
                            break;
                        case Chart.collumn.Up:
                            if (Input.upKey == Input.buttonState.press)
                                hit = true;
                            break;
                        case Chart.collumn.Right:
                            if (Input.rightKey == Input.buttonState.press)
                                hit = true;
                            break;
                        default:
                            hit = false;
                            break;
                    }
                    if(hit)
                    {
                        nMiss = true;
                        chart.scoreHandler.Hit();
                        deadNotes.Add(item);
                    }
                } else if(chart.beat <= (item.noteInfo.time + chart.missTime) && (item.noteInfo.time - chart.missTime) <= chart.beat)
                {
                    bool miss = false;
                    switch (collumn)
                    {
                        case Chart.collumn.Left:
                            if (Input.leftKey == Input.buttonState.press)
                                miss = true;

                            break;
                        case Chart.collumn.Down:
                            if (Input.downKey == Input.buttonState.press)
                                miss = true;
                            break;
                        case Chart.collumn.Up:
                            if (Input.upKey == Input.buttonState.press)
                                miss = true;
                            break;
                        case Chart.collumn.Right:
                            if (Input.rightKey == Input.buttonState.press)
                                miss = true;
                            break;
                        default:
                            miss = false;
                            break;
                    }
                    if(miss && !nMiss)
                    {
                        nMiss = true;
                        chart.scoreHandler.Miss(true);
                        deadNotes.Add(item);
                    }
                }
                if((item.noteInfo.time+chart.scoreTime) <= chart.beat)
                {
                    //darn issa miss
                    //Console.WriteLine("miss");
                    chart.scoreHandler.Miss(false);
                    deadNotes.Add(item);
                }

            }
            foreach (var item in deadNotes)
            {
                arrows.Remove(item);
                item.alive = false;
            }
            
        }
    }
}
