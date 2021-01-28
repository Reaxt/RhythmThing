using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Components;
using RhythmThing.System_Stuff;

namespace RhythmThing.Objects
{
    public class ScoreHandler : GameObject
    {
        private Visual visual;
        private Chart chart;
        private int combo;
        public int hits;
        private Coords[] hit;
        private Coords[] miss;
        private Coords[] early;
        private Coords[] late;
        private bool lastHit = false;
        private bool lastMiss = false;
        public int notes;
        public ScoreHandler(Chart chart, int notes)
        {
            this.chart = chart;
            this.notes = notes;
        }

        public override void End()
        {

        }

        public override void Start(Game game)
        {
            this.GameObjectType= objType.visual;
            Components = new List<Component>();
            visual = new Visual();
            visual.x = 48;
            visual.y = 20;
            visual.Active = true;
            hit = new Coords[3];
            hit[0] = new Coords(0, 0, 'H', ConsoleColor.Green, ConsoleColor.Black);
            hit[1] = new Coords(1, 0, 'I', ConsoleColor.Green, ConsoleColor.Black);
            hit[2] = new Coords(2, 0, 'T', ConsoleColor.Green, ConsoleColor.Black);
            miss = new Coords[4];
            miss[0] = new Coords(0, 0, 'M', ConsoleColor.Red, ConsoleColor.Black);
            miss[1] = new Coords(1, 0, 'I', ConsoleColor.Red, ConsoleColor.Black);
            miss[2] = new Coords(2, 0, 'S', ConsoleColor.Red, ConsoleColor.Black);
            miss[3] = new Coords(3, 0, 'S', ConsoleColor.Red, ConsoleColor.Black);
            early = new Coords[5];
            early[0] = new Coords(-1, 1, 'e', ConsoleColor.Yellow, ConsoleColor.Black);
            early[1] = new Coords(0, 1, 'a', ConsoleColor.Yellow, ConsoleColor.Black);
            early[2] = new Coords(1, 1, 'r', ConsoleColor.Yellow, ConsoleColor.Black);
            early[3] = new Coords(2, 1, 'l', ConsoleColor.Yellow, ConsoleColor.Black);
            early[4] = new Coords(3, 1, 'y', ConsoleColor.Yellow, ConsoleColor.Black);
            late = new Coords[4];
            late[0] = new Coords(0, 1, 'l', ConsoleColor.Yellow, ConsoleColor.Black);
            late[1] = new Coords(1, 1, 'a', ConsoleColor.Yellow, ConsoleColor.Black);
            late[2] = new Coords(2, 1, 't', ConsoleColor.Yellow, ConsoleColor.Black);
            late[3] = new Coords(3, 1, 'e', ConsoleColor.Yellow, ConsoleColor.Black);
            Components.Add(visual);
            
            combo = 0;
            hits = 0;
        }

        public void Hit()
        {
            if(lastMiss)
            {
                for (int i = 0; i < miss.Length; i++)
                {
                    visual.localPositions.Remove(miss[i]);
                }

            }
            if(!lastHit)
            {
            for (int i = 0; i < hit.Length; i++)
            {
                visual.localPositions.Add(hit[i]);
            }
            lastMiss = false;
            lastHit = true;

            }
            combo++;
            hits++;
            //draw combo
            string combostr = combo.ToString();
            char[] comboar = combostr.ToCharArray();
            visual.localPositions.RemoveAll(p => p.y == 1);
            for (int i = 0; i < comboar.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, 1, comboar[i], ConsoleColor.White, ConsoleColor.Black));
            }
        }

        public void Miss(bool isEarly)
        {
            visual.localPositions.RemoveAll(p => p.y == 1);
            
            if(lastHit)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    visual.localPositions.Remove(hit[i]);

                }

            }
            if(!lastMiss)
            {

                for (int i = 0; i < miss.Length; i++)
                {
                    visual.localPositions.Add(miss[i]);
                lastHit = false;
                lastMiss = true;
                combo = 0;
                }
            }
            if(isEarly)
            {
                for (int i = 0; i < early.Length; i++)
                {
                    visual.localPositions.Add(early[i]);
                }
            } else
            {
                for (int i = 0; i < late.Length; i++)
                {
                    visual.localPositions.Add(late[i]);
                }
            }
        }

        public override void Update(double time, Game game)
        {
        }
    }
}
