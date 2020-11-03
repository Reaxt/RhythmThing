using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Objects.ScoreScreen
{
    public class ScoreScreenHandler : GameObject
    {
        /*
         * <60 = F
         * 60-66 = D
         * 67-72 = C
         * 73-79 = B
         * 80-86 = A
         * 87-90 = S
         * 90-99 = SS
         * 100 = SSS
         */
        private Visual visual;
        private Visual letterGrade;
        private Visual backDrop;
        private Visual clearGuage;
        private Visual gradeCover;
        private float percent;
        private float guagePercent = -1;
        private float timePassed = 0;
        private float guageTick = 0.04f;
        private float coverWait = 0.05f;
        private float coverTick = 0.025f;
        private bool coverSlide = false;
        string grade;
        public override void End()
        {

        }

        public override void Start(Game game)
        {
            components = new List<Component>();
            percent = (float)game.notesHit / (float)game.totalNotes;
            //just write it out for now
            visual = new Visual();
            letterGrade = new Visual();
            backDrop = new Visual();
            clearGuage = new Visual();
            gradeCover = new Visual();
            gradeCover.z = 2;
            clearGuage.y = 25;
            letterGrade.z = 1;
            letterGrade.x = 60;
            visual.y = 49;
            percent = (float)Math.Floor(percent * 100);
            char[] songName = ("Song: " + (game.songName)).ToCharArray();
            char[] percentText = ("Percent: " + percent + "%").ToCharArray();
            char[] grade;
            
            
            //This esentially draws the letter sprites. Its a bit of a mess, but it works fine.
            if (percent == 100)
            {
                grade = "SSS".ToCharArray();
                //first s
                for (int i = 0; i < 10; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(10, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(9, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(1, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(0, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
                for (int i = 0; i < 11; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 11, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 20, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 21, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
                for (int i = 0; i < 10; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i+12, 1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i+12, 0, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(10+12, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(9+12, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(1+12, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(0+12, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));

                    letterGrade.localPositions.Add(new Coords(i + 24, 1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 24, 0, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(10 +24, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(9 + 24, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(1 + 24, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(0 + 24, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
                for (int i = 0; i < 11; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i+12, 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i+12, 11, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i+12, 20, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i+12, 21, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));

                    letterGrade.localPositions.Add(new Coords(i + 24, 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 24, 11, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 24, 20, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 24, 21, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }


            } else if(percent >= 90)
            {
                letterGrade.x = 65;
                grade = "SS".ToCharArray();
                for (int i = 0; i < 10; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(10, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(9, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(1, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(0, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
                for (int i = 0; i < 11; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 11, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 20, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 21, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
                for (int i = 0; i < 10; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i + 12, 1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 12, 0, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(10 + 12, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(9 + 12, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(1 + 12, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(0 + 12, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
                for (int i = 0; i < 11; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i + 12, 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 12, 11, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 12, 20, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i + 12, 21, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }


            } else if(percent >=87)
            {
                letterGrade.x = 70;
                grade = "S".ToCharArray();
                for (int i = 0; i < 10; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(10, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(9, i, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(1, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(0, i + 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }
                for (int i = 0; i < 11; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 10, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 11, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 20, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                    letterGrade.localPositions.Add(new Coords(i, 21, ' ', ConsoleColor.Yellow, ConsoleColor.Yellow));
                }

            } else if(percent >= 80)
            {
                letterGrade.x = 69;
                grade = "A".ToCharArray();
                for (int i = 0; i < 21; i++)
                {
                    letterGrade.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.Green, ConsoleColor.Green));
                    letterGrade.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.Green, ConsoleColor.Green));
                    letterGrade.localPositions.Add(new Coords(15, i, ' ', ConsoleColor.Green, ConsoleColor.Green));
                    letterGrade.localPositions.Add(new Coords(16, i, ' ', ConsoleColor.Green, ConsoleColor.Green));
                }
                for (int i = 0; i < 15; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 20, ' ', ConsoleColor.Green, ConsoleColor.Green));
                    letterGrade.localPositions.Add(new Coords(i, 19, ' ', ConsoleColor.Green, ConsoleColor.Green));
                    letterGrade.localPositions.Add(new Coords(i, 13, ' ', ConsoleColor.Green, ConsoleColor.Green));
                    letterGrade.localPositions.Add(new Coords(i, 12, ' ', ConsoleColor.Green, ConsoleColor.Green));
                }
            } else if(percent >= 73)
            {
                letterGrade.x = 69;
                grade = "B".ToCharArray();

                for (int i = 0; i < 21; i++)
                {
                    letterGrade.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(15, i, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(16, i, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                }
                for (int i = 0; i < 15; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 20, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(i, 19, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(i, 11, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(i, 10, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(i, 9, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                    letterGrade.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Green, ConsoleColor.Blue));
                }
                letterGrade.localPositions.Add(new Coords(15, 20, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(16, 20, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(16, 19, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(16, 0, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(15, 0, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(16, 1, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(16, 10, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(15, 10, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(16, 11, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
                letterGrade.localPositions.Add(new Coords(16, 9, ' ', ConsoleColor.Black, ConsoleColor.DarkGray));
            } else if(percent >= 67)
            {
                letterGrade.x = 65;
                grade = "C".ToCharArray();

                for (int i = 0; i < 22; i++)
                {
                    letterGrade.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.DarkYellow, ConsoleColor.DarkYellow));
                    letterGrade.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.DarkYellow, ConsoleColor.DarkYellow));

                }
                for (int i = 0; i < 15; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.DarkYellow, ConsoleColor.DarkYellow));
                    letterGrade.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.DarkYellow, ConsoleColor.DarkYellow));
                    letterGrade.localPositions.Add(new Coords(i, 21, ' ', ConsoleColor.DarkYellow, ConsoleColor.DarkYellow));
                    letterGrade.localPositions.Add(new Coords(i, 22, ' ', ConsoleColor.DarkYellow, ConsoleColor.DarkYellow));
                }
            } else if(percent >= 60)
            {
                letterGrade.x = 65;
                for (int i = 0; i < 22; i++)
                {
                    letterGrade.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));
                    letterGrade.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));

                }
                for (int i = 0; i < 19; i++)
                {
                    letterGrade.localPositions.Add(new Coords(14, i+2, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));
                    letterGrade.localPositions.Add(new Coords(15, i+2, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));
                }
                for (int i = 0; i < 14; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));
                    letterGrade.localPositions.Add(new Coords(i, 1, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));
                    letterGrade.localPositions.Add(new Coords(i, 22, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));
                    letterGrade.localPositions.Add(new Coords(i, 21, ' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen));
                }
                grade = "D".ToCharArray();
            } else
            {
                letterGrade.x = 67;
                for (int i = 0; i < 22; i++)
                {
                    letterGrade.localPositions.Add(new Coords(0, i, ' ', ConsoleColor.Red, ConsoleColor.Red));
                    letterGrade.localPositions.Add(new Coords(1, i, ' ', ConsoleColor.Red, ConsoleColor.Red));

                }
                for (int i = 0; i < 15; i++)
                {
                    letterGrade.localPositions.Add(new Coords(i, 22, ' ', ConsoleColor.Red, ConsoleColor.Red));
                    letterGrade.localPositions.Add(new Coords(i, 21, ' ', ConsoleColor.Red, ConsoleColor.Red));
                    letterGrade.localPositions.Add(new Coords(i, 14, ' ', ConsoleColor.Red, ConsoleColor.Red));
                    letterGrade.localPositions.Add(new Coords(i, 15, ' ', ConsoleColor.Red, ConsoleColor.Red));
                }
                grade = "F".ToCharArray();
            }
            for (int i = 0; i < 101; i++)
            {
                visual.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                visual.localPositions.Add(new Coords(i, -1, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                visual.localPositions.Add(new Coords(i, -2, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
            }
            for (int i = 0; i < songName.Length; i++)
            {
                visual.localPositions.Add(new Coords((100/2)-(songName.Length/2)+i, -1, songName[i], ConsoleColor.White, ConsoleColor.DarkGray));
            }
            for (int i = 0; i < percentText.Length; i++)
            {
                visual.localPositions.Add(new Coords(i+13, -30, percentText[i], ConsoleColor.White, ConsoleColor.DarkGray));
                
            }
            char[] enter = "Press enter to return to the main menu".ToCharArray();
            for (int i = 0; i < enter.Length; i++)
            {
                visual.localPositions.Add(new Coords(i, -40, enter[i], ConsoleColor.White, ConsoleColor.DarkGray));

            }

            //draw backdrop and grade cover
            for (int x = 60; x < 100; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    backDrop.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                    backDrop.localPositions.Add(new Coords(x-60, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));

                    gradeCover.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                    gradeCover.localPositions.Add(new Coords(x - 60, y, ' ', ConsoleColor.Magenta, ConsoleColor.DarkGray));
                }
            }
            //draw "clear line"
            for (int x = 40; x < 60; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    backDrop.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Red, ConsoleColor.Red));

                }
            }
            for (int i = 25; i < 100; i++)
            {
                backDrop.localPositions.Add(new Coords(59, i, ' ', ConsoleColor.Red, ConsoleColor.Red));
            }
            char[] GradeText = "Grade".ToCharArray();
            for (int i = 0; i < GradeText.Length; i++)
            {
                backDrop.localPositions.Add(new Coords(i+75, 24, GradeText[i], ConsoleColor.White, ConsoleColor.Gray));
            }
            //draw clear guage
            
            for (int i = 0; i < percent; i++)
            {
                //clearGuage.localPositions.Add(new Coords(i, 0, ' ', ConsoleColor.Green, ConsoleColor.Green));
            }

            visual.active = true;
            letterGrade.active = true;
            backDrop.active = true;
            clearGuage.active = true;
            gradeCover.active = true;
            components.Add(backDrop);
            components.Add(visual);
            components.Add(letterGrade);
            components.Add(clearGuage);
            components.Add(gradeCover);
            
        }

        public override void Update(double time, Game game)
        {
            //draw guage
            //timePassed = 20;
            if (timePassed >= guageTick)
            {
                if(guagePercent <= percent)
                {
                    if(guagePercent < 60)
                    {
                        for (int y = 0; y < 22; y++)
                        {
                            clearGuage.localPositions.Add(new Coords((int)guagePercent-1, y, ' ', ConsoleColor.Red, ConsoleColor.Red));
                        }
                        game.audioManager.playForget("syssd_score.mp3");
                        guagePercent++;
                    } else if(guagePercent == 60)
                    {
                        clearGuage.localPositions.Clear();
                        for (int x = 0; x < 60; x++)
                        {
                            for (int y = 0; y < 22; y++)
                            {
                                clearGuage.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Green, ConsoleColor.Green));
                            }
                        }
                        for (int x = 40; x < 60; x++)
                        {
                            for (int y = 0; y < 25; y++)
                            {
                                backDrop.localPositions.Add(new Coords(x, y, ' ', ConsoleColor.Green, ConsoleColor.Green));

                            }
                        }
                        game.audioManager.playForget("syssd_res_2.mp3");
                        guagePercent++;
                    } else
                    {
                        for (int y = 0; y < 22; y++)
                        {
                            clearGuage.localPositions.Add(new Coords((int)guagePercent - 1, y, ' ', ConsoleColor.Green, ConsoleColor.Green));
                        }
                        game.audioManager.playForget("syssd_score.mp3");
                        guagePercent++;
                    }
                    timePassed = 0; 
                } else 
                {
                    
                }
            }
            if(guagePercent >= percent)
            {
                if(coverSlide)
                {
                    if(timePassed >= coverTick)
                    {
                        gradeCover.y--;
                        timePassed = 0;
                    }
                } else if(timePassed >= coverWait)
                {
                    timePassed = 0;
                    coverSlide = true;
                }
            }
            timePassed = timePassed + (float)time;
            if(game.input.ButtonStates[Input.ButtonKind.Confirm] == Input.ButtonState.Press || game.input.ButtonStates[Input.ButtonKind.Cancel] == Input.ButtonState.Press)
            {
                game.sceneManager.loadScene(0);
            }
        }
    }
}
