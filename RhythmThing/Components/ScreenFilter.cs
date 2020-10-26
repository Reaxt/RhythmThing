using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmThing.Components
{
    //this may be a bad name...
    public struct DisplayData
    {
        public ConsoleColor[,] foreColors;
        public ConsoleColor[,] backColors;
        public char[,] characters;
        public DisplayData(ConsoleColor[,] foreCol, ConsoleColor[,] backCol, char[,] chars)
        {
            foreColors = foreCol;
            backColors = backCol;
            characters = chars;
        }
    }
    public abstract class ScreenFilter
    {
        public abstract DisplayData RunFilt(ConsoleColor[,] foreColors, ConsoleColor[,] backColors, char[,] characters);
    }
}
