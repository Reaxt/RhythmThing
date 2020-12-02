using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
namespace TestScript.Shaders
{
    class Yoinkers : ScreenFilter
    {
        public bool Yoink = false;
        private DisplayData heckdata;
        public Visual[] cols;
        private int firstX = 20;
        public Yoinkers(Visual heck)
        {
            
            this.heckdata = new DisplayData();
            heckdata.backColors = new ConsoleColor[100, 50];
            heckdata.foreColors = new ConsoleColor[100, 50];
            heckdata.characters = new char[100, 50];
            heck.localPositions.ForEach(coords =>
            {
                if(coords.x < 100 && coords.y < 50)
                {
                    heckdata.backColors[coords.x, coords.y] = coords.backColor;
                    heckdata.foreColors[coords.x, coords.y] = coords.foreColor;
                    heckdata.characters[coords.x, coords.y] = coords.character;

                }

            });
            cols = new Visual[4];
            for (int i = 0; i < 4; i++)
            {
                cols[i] = new Visual();
                cols[i].active = true;

            }
        }
        public override DisplayData RunFilt(ConsoleColor[,] foreColors, ConsoleColor[,] backColors, char[,] characters)
        {

            for (int i = 0; i < 4; i++)
            {
                int xOff = 15 * i;
                cols[i].localPositions.Clear();
                for (int x = 0; x < 15; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        int xRef = firstX + xOff + x;

                        cols[i].localPositions.Add(new Coords(x, y, characters[xRef, y], foreColors[xRef, y], backColors[xRef, y]));
                    }
                }

            }
            if(Yoink)
            {
                return heckdata;
            } else
            {
                return new DisplayData(foreColors, backColors, characters);

            }
        }
    }
}
