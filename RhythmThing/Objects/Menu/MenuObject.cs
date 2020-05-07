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
            
            
        }

        public override void Update(double time, Game game)
        {
            if(Input.downKey == Input.buttonState.press)
            {
                if (selected < count)
                {
                    selected++;
                    selector.y = selector.y - 5;

                } else
                {
                    selector.y = 45;
                    selected = 0;
                }
                chartInfoVisual.UpdateChart(songs[selected].chart.chartInfo);
            }
            if (Input.upKey == Input.buttonState.press)
            {
                if (selected > 0)
                {
                    selected--;
                    selector.y = selector.y + 5;

                }
                else
                {
                    selector.y = 45 + (count * -5);
                    selected = count;
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
