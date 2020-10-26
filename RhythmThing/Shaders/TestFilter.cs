using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhythmThing.Components;
namespace RhythmThing.Shaders
{
    class TestFilter : ScreenFilter
    {
        Random random = new Random();
        public override DisplayData RunFilt(ConsoleColor[,] foreColors, ConsoleColor[,] backColors, char[,] characters)
        {

            //lets just set all the colors to invert?
            //naaah
            for (int x = 0; x < foreColors.GetLength(0); x++)
            {
                for (int y = 0; y < foreColors.GetLength(1); y++)
                {
                    if((int)foreColors[x,y] < 15)
                    {
                        foreColors[x, y] = foreColors[x, y] + 1;


                    }
                    else
                    {
                        foreColors[x, y] = ConsoleColor.Black;
                    }

                    if ((int)foreColors[x, y] < 15)
                    {
                        backColors[x, y] = backColors[x, y] + 1;


                    }
                    else
                    {
                        backColors[x, y] = ConsoleColor.Black;
                    }
                }
            }
            return (new DisplayData(foreColors,backColors,characters));
        }
    }
}
