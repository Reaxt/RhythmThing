﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.System_Stuff;
using RhythmThing.Components;
using RhythmThing.Objects;
using RhythmThing.Utils;
namespace RhythmThing.Objects
{
    public class Receiver : GameObject
    {
        private double currenttime = 0;
        //sorry,
        public int xOffset = 0;
        public int yOffset = 0;
        public int xModOffset = 0;
        public int yModOffset = 0;
        public float rot = 0f;
        private int actualX;
        private int actualY;
        private Visual visual;
        private Visual pressedVisual;
        public Chart.collumn collumn;
        private Chart chart;
        private List<Chart.NoteInfo> notes;
        private List<Chart.NoteInfo> spawnedNotes;
        private List<Arrow> deadNotes;
        private List<Arrow> arrows;
        private bool nMiss = false;
        public Arrow.direction direction = Arrow.direction.down;
        public Dictionary<string, float> mods;
        public bool frozen = false;
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
            //dictionary representing mods
            this.mods = ModEffects.getNewModsDictionary();


            
        }
        public override void Start(Game game)
        {
            //this block defines it as visual
            this.components = new List<Component>();
            this.type = objType.visual;
            this.visual = new Visual();
            this.pressedVisual = new Visual();
            visual.active = true;
            visual.x = 30;
            visual.y = 42;
            visual.z = -1;
            pressedVisual.z = -2;
            
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
                    for (int i = 2; i > -2; i--)
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
                    for (int i = -2; i < 2; i++)
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
                    for (int i = -2; i < 2; i++)
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
            
            foreach (var item in visual.localPositions)
            {
                pressedVisual.localPositions.Add(new Coords(item.x, item.y, item.character, ConsoleColor.DarkGray, ConsoleColor.DarkGray));
            }
            components.Add(visual);
            components.Add(pressedVisual);
            pressedVisual.active = true;
            pressedVisual.x = visual.x;
            pressedVisual.y = visual.y;
            pressedVisual.z = -2;
            actualX = visual.x;
            actualY = visual.y;
        }

        public override void Update(double time, Game game)
        {
            xModOffset = 0;
            yModOffset = 0;
            //how to do mods!
            xModOffset = ModEffects.CalculateReceiverX(mods);
            yModOffset = ModEffects.CalculateReceiverY(mods);



            visual.x = xOffset + actualX + xModOffset;
            visual.y = yOffset + actualY + yModOffset;
            pressedVisual.x = visual.x;
            pressedVisual.y = visual.y;
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
                    temparrow.mods = this.mods;
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
                item.xOffset = this.xOffset;
                item.yOffset = this.yOffset;
                item.dir = this.direction;
                item.angle = this.rot;
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
                //REALLLLY FEELS LIKE A BAD IDEA DOING THIS LIKE THIS
                item.freeze(this.frozen);
            }
            foreach (var item in deadNotes)
            {
                arrows.Remove(item);
                item.alive = false;
            }

            bool pressed = false;
            switch (collumn)
            {
                case Chart.collumn.Left:
                    if (Input.leftKey == Input.buttonState.held)
                        pressed = true;

                    break;
                case Chart.collumn.Down:
                    if (Input.downKey == Input.buttonState.held)
                        pressed = true;
                    break;
                case Chart.collumn.Up:
                    if (Input.upKey == Input.buttonState.held)
                        pressed = true;
                    break;
                case Chart.collumn.Right:
                    if (Input.rightKey == Input.buttonState.held)
                        pressed = true;
                    break;
                default:
                    pressed = false;
                    break;
            }
            if(pressed)
            {
                
                pressedVisual.z = 0;
            } else
            {
                pressedVisual.z = -2;
            }

        }
    }
}
