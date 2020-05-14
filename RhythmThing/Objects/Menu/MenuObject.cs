using RhythmThing.System_Stuff;
using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RhythmThing.Objects.Menu
{
    class MenuObject : GameObject
    {
        List<SongContainer> songs;
        private Visual selector;
        private ChartInfoVisual chartInfoVisual;
        private int selected = 0;
        private int count = -1;
        private int drawAmount = 8;
        private int currentHighestSelect = 7;//must be same as drawAmount-1
        private int currentLowestSelect = 0; 
        public override void End()
        {
            //throw new NotImplementedException();
        }

        public override void Start(Game game)
        {
            components = new List<Component>();

            string[] chartNames = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "!Content/!Songs"));
            
            songs = new List<SongContainer>();
            for (int i = 0; i < chartNames.Length; i++)
            {
                SongContainer tempcon = new SongContainer(chartNames[i], i);
                game.addGameObject(tempcon);
                songs.Add(tempcon);
                count++;
            }
            selector = new Visual();
            selector.active = true;

            selector.x = 3;
            selector.y = 45;
            selector.localPositions.Add(new Coords(0, 0, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(0, 1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(0, -1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(32, 0, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(32, 1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            selector.localPositions.Add(new Coords(32, -1, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            for (int i = 0; i < 33; i++)
            {
                selector.localPositions.Add(new Coords(i, 2, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
                selector.localPositions.Add(new Coords(i, -2, ' ', ConsoleColor.Cyan, ConsoleColor.Cyan));
            }
            components.Add(selector);
            chartInfoVisual = new ChartInfoVisual(songs[selected].chart.chartInfo);
            game.addGameObject(chartInfoVisual);

            DrawFromPoint(0);
        }

        //should only ever be called from a safe index. otherwise I fucked up elsewhere lo
        void DrawFromPoint(int topmost)
        {
            //reset
            foreach(SongContainer songContainer in songs)
            {
                songContainer.visual.active = false;
            }
            //draw starting from point, draw drawamount or limit
            if(songs.Count >= drawAmount)
            {
                for (int i = topmost; i < drawAmount+topmost; i++)
                {
                    songs[i].visual.active = true;
                    songs[i].visual.y = (45 + ((i-topmost) * -5));
                }
            } else
            {
                for (int i = 0; i < songs.Count; i++)
                {
                    songs[i].visual.active = true;
                }
            }

        }

        public override void Update(double time, Game game)
        {
            if(Input.downKey == Input.buttonState.press)
            {
                if (selected < count)
                {

                    selected++;
                    if (selected > currentHighestSelect)
                    {
                        currentLowestSelect++;
                        currentHighestSelect++;
                        DrawFromPoint(currentLowestSelect);
                    } else
                    {
                        selector.y = selector.y - 5;

                    }
                    

                } else
                {
                    selector.y = 45;
                    selected = 0;
                    DrawFromPoint(0);
                    currentLowestSelect = 0;
                    currentHighestSelect = drawAmount-1;
                }
                chartInfoVisual.UpdateChart(songs[selected].chart.chartInfo);
            }
            if (Input.upKey == Input.buttonState.press)
            {
                if (selected > 0)
                {
                    selected--;
                    if(selected < currentLowestSelect)
                    {
                        currentLowestSelect--;
                        currentHighestSelect--;
                        DrawFromPoint(currentLowestSelect);
                    } else
                    {
                        selector.y = selector.y + 5;

                    }

                }
                else
                {
                    if(songs.Count > drawAmount)
                    {
                    selector.y = 45 + ((drawAmount-1) * -5);

                    } else
                    {
                        selector.y = 45 + ((count) * -5);
                    }
                    selected = count;
                    DrawFromPoint(count - (drawAmount-1));
                    currentHighestSelect = count;
                    currentLowestSelect = (count - (drawAmount-1));
                }
                chartInfoVisual.UpdateChart(songs[selected].chart.chartInfo);
            }
            if(Input.enterKey == Input.buttonState.press)
            {
                game.ChartToLoad = songs[selected].chartName;
                game.sceneManager.loadScene(1);
            }
        }
    }
}
