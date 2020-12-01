using RhythmThing.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestScript.Shaders
{
    public class BoxBuffer : ScreenFilter
    {
        public int[] boxPoint = { 3, 0 };
        public int[] boxDimensions = { 5, 10 };
        DisplayData lastSavedCoords;
        int screenX;
        int screenY;
        bool firstRun = true;

        public override DisplayData RunFilt(ConsoleColor[,] foreColors, ConsoleColor[,] backColors, char[,] characters)
        {
            if (firstRun)
            {
                lastSavedCoords = new DisplayData(foreColors, backColors, characters);
                screenX = characters.GetLength(0);
                screenY = characters.GetLength(1);
                firstRun = false;
            }
            for (int x = 0; x < screenX; x++)
            {
                for (int y = 0; y < screenY; y++)
                {
                    if(x >= boxPoint[0] && x <= (boxPoint[0]+boxDimensions[0]) && y >= boxPoint[1] && y <= (boxPoint[1] + boxDimensions[1]))
                    {

                    } else
                    {
                        foreColors[x, y] = lastSavedCoords.foreColors[x, y];
                        backColors[x, y] = lastSavedCoords.backColors[x, y];
                        characters[x, y] = lastSavedCoords.characters[x, y];
                    }
                }
            }

            lastSavedCoords = new DisplayData(foreColors, backColors, characters);
            ConsoleColor[,] tempF = foreColors.Clone() as ConsoleColor[,];
            ConsoleColor[,] tempB = backColors.Clone() as ConsoleColor[,];
            char[,] tempC = characters.Clone() as char[,];

            for (int x = 0; x < screenX; x++)
            {
                for (int y = 0; y < screenY; y++)
                {
                    if (((x == boxPoint[0] || x == boxPoint[0] + boxDimensions[0]) && y >= boxPoint[1] && y <= boxPoint[1] + boxDimensions[1]) || ((y == boxPoint[1] || y == boxPoint[1] + boxDimensions[1]) && x >= boxPoint[0] && x <= boxPoint[0] + boxDimensions[0]))
                    {
                        tempF[x, y] = ConsoleColor.Red;
                        tempB[x, y] = ConsoleColor.Red;
                        tempC[x, y] = ' ';
                    }
                }
            }

            
            return new DisplayData(tempF, tempB, tempC);
        }
    }
}
