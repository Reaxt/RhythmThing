using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.Objects.Menu
{
    //goal of this class, it will draw everything relating to containers.
    public class ContainerHandler : GameObject
    {
        private int count = 11;
        private int sep = 5;
        private int initY = 25;
        private int initX = 9;
        private ConsoleColor _normalFront = ConsoleColor.Black;
        private ConsoleColor _normalBack = ConsoleColor.White;
        private int lastSelected = 0;
        private string AnimEasing = "easeOutSine";
        private float AnimDur = 0.125f;
        private List<SongContainer> _containers;
        private Visual[] visuals;

        public ContainerHandler(List<SongContainer> containers)
        {
            _containers = containers;
            visuals = new Visual[count];
            //lazy!!!
            for (int i = 0; i < count; i++)
            {
                visuals[i] = new Visual();

            }

            
        }
        public override void End()
        {

        }

        private void DrawAll(int selected)
        {
            //draw down,
            
            for (int i = 0; i < count; i++)
            {
                visuals[i].localPositions.Clear();

                if (i <= (count / 2))
                {
                    visuals[i].y = initY - (i * sep);

                }
                else
                {
                    visuals[i].y = (initY + (((count / 2) * sep)) + sep) - ((i - (count / 2)) * sep);
                }
                visuals[i].x = initX;
                visuals[i].z = 10;
                //sorry for the hardcoding..
                if (i > 1 && i <= count / 2)
                {
                    visuals[i].x -= i - 1;
                }
                else if ((i < count - 1) && i > 1)
                {
                    visuals[i].x -= (count - i);
                }
                visuals[i].active = true;
                for (int x = 0; x < 31; x++)
                {
                    visuals[i].localPositions.Add(new Coords(x, 1, ' ', _normalFront, _normalBack));
                    visuals[i].localPositions.Add(new Coords(x, 0, ' ', _normalFront, _normalBack));
                    visuals[i].localPositions.Add(new Coords(x, -1, ' ', _normalFront, _normalBack));
                }
                visuals[i].localPositions.Add(new Coords(0, 0, i.ToString().ToCharArray()[0], ConsoleColor.Black, ConsoleColor.White));
                visuals[i].localPositions.Add(new Coords(0, -1, i.ToString().ToCharArray()[0], ConsoleColor.Black, ConsoleColor.White));
                visuals[i].localPositions.Add(new Coords(0, 1, i.ToString().ToCharArray()[0], ConsoleColor.Black, ConsoleColor.White));
                //not gonna use sprites here I dont think,
            }
            //go from selected
            int offset = 0;
            for (int i = 0; i <= count/2; i++)
            {

                if((_containers.Count-1) < i + selected + offset)
                {
                    offset -= (_containers.Count);
                }
                int goalIndex = i + selected + offset;
                visuals[i].writeText(2, 0, _containers[goalIndex].chart.chartInfo.songName, _normalFront, _normalBack);
                ConsoleColor difficulty = getDiffColor(_containers[goalIndex].chart.chartInfo.difficulty);
                visuals[i].localPositions.Add(new Coords(0, 1, ' ', difficulty, difficulty));
                visuals[i].localPositions.Add(new Coords(0, 0, ' ', difficulty, difficulty));
                visuals[i].localPositions.Add(new Coords(0, -1, ' ', difficulty, difficulty));

            }
            offset = 0;
            int otherCount = 0;
            for (int i = count-1; i > (count/2); i--)
            {
                otherCount--;
                int goalIndex = (selected + (otherCount));
                while (goalIndex <0)
                {
                    goalIndex += _containers.Count;
                }
                visuals[i].writeText(2, 0, _containers[goalIndex].chart.chartInfo.songName, _normalFront, _normalBack);
                ConsoleColor difficulty = getDiffColor(_containers[goalIndex].chart.chartInfo.difficulty);
                visuals[i].localPositions.Add(new Coords(0, 1, ' ', difficulty, difficulty));
                visuals[i].localPositions.Add(new Coords(0, 0, ' ', difficulty, difficulty));
                visuals[i].localPositions.Add(new Coords(0, -1, ' ', difficulty, difficulty));
            }
        }
        public void AnimUp()
        {
            DrawAll(MenuObject.selected);
            for (int i = 0; i < visuals.Length; i++)
            {
                int copyTarg = i+1;
                if (copyTarg >= visuals.Length)
                {
                    copyTarg = 0;
                }

                int pastX = visuals[copyTarg].x;
                int pastY = visuals[copyTarg].y;
                if (count / 2 == i)
                {
                    pastX = visuals[i].x - 1;
                    pastY = -sep;
                }
                visuals[i].Animate(new int[] { pastX, pastY }, new int[] { visuals[i].x, visuals[i].y }, AnimEasing, AnimDur);
            }
        }

        public void AnimDown()
        {
            DrawAll(MenuObject.selected);
            for (int i = visuals.Length-1; i >= 0; i--)
            {
                int copyTarg = i - 1;
                if (copyTarg < 0)
                {
                    copyTarg = visuals.Length-1;
                }

                int pastX = visuals[copyTarg].x;
                int pastY = visuals[copyTarg].y;
                if ((count / 2)+1 == i)
                {
                    pastX = visuals[i].x - 1;
                    pastY = Program.ScreenY+sep;
                }
                visuals[i].Animate(new int[] { pastX, pastY }, new int[] { visuals[i].x, visuals[i].y }, AnimEasing, AnimDur);
            }
        }

        public override void Start(Game game)
        {

            DrawAll(MenuObject.selected);
            for (int i = 0; i < visuals.Length; i++)
            {
                components.Add(visuals[i]);
                visuals[i].Animate(new int[] { visuals[i].x - 20, visuals[i].y }, new int[] { visuals[i].x, visuals[i].y }, "easeOutBack", 0.125f + (i * 0.05f));
            }

        }
        
        public static ConsoleColor getDiffColor(int diff)
        {
            ConsoleColor temp = ConsoleColor.White;
            switch (diff)
            {
                case 0:
                    temp = ConsoleColor.Gray;
                    break;
                case 1:
                    temp = ConsoleColor.Blue;
                    break;
                case 2:
                    temp = ConsoleColor.Green;
                    break;
                case 3:
                    temp = ConsoleColor.Yellow;
                    break;
                case 4:
                    temp = ConsoleColor.Red;
                    break;
                case 5:
                    temp = ConsoleColor.Magenta;
                    break;
                default:
                    break;
            }
            return temp;
        }

        public override void Update(double time, Game game)
        {
            //lets see
            

        }
    }
}
