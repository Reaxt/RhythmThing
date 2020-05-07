using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.System_Stuff;
namespace RhythmThing.Components
{
    public class Visual : Component
    {

        
        public int x;
        public int y;
        public int z;
        //privated so I DONT USE IT
        private int scale;
        public ConsoleColor overrideback;
        public ConsoleColor overridefront;
        public bool overrideColor = false;
        //what the object should interact with (local space)
        public List<Coords> localPositions = new List<Coords>();
        //what the visual class wil work with (world space)
        public List<Coords> renderPositions = new List<Coords>();

        private int bigx = int.MinValue;
        private int bigy = int.MinValue;
        private int smallx = int.MaxValue;
        private int smally = int.MaxValue;
        
        public override void Update(double time)
        {
            //KISS for now
            renderPositions = new List<Coords>();

            foreach (Coords coord in localPositions)
            {
                //only works with the one way rn
                
                if(overrideColor)
                {
                   renderPositions.Add(new Coords(x + coord.x, y + coord.y, coord.character, overrideback, overridefront));
                } else
                {
                   renderPositions.Add(new Coords(x + coord.x, y+coord.y, coord.character, coord.foreColor, coord.backColor));

                }
                if(scale != 0)
                {
                    //TODO: FIX THIS MAYBE
                    //check current "bounds"
                    if(bigx < coord.x)
                    {
                        bigx = coord.x;

                    }
                    if(bigy < coord.y)
                    {
                        bigy = coord.y;
                    }
                    if(smallx > coord.x)
                    {
                        smallx = coord.x;
                    }
                    if(smally > coord.y)
                    {
                        smally = coord.y;                               
                    }
                }


            }
            //calculate scale
            if(scale < 0)
            {
                int newX = bigx;
                int newY = bigy;
                int subY = 0;
                int subX = 0;
                Coords[,] currentWorkArray;
                //center
                /*
                 *-2 4 -> 7
                 * 3 9 -> 7
                 */
                newX = ((bigx - smallx)+1);
                newY = ((bigy - smally)+1);
                //fill new 2d array
                Coords[,] firstArray = new Coords[newX,newY];
                Coords[,] lastArray;
                for (int x = 0; x < newX; x++)
                {
                    for (int y = 0; y < newY; y++)
                    {
                        Coords tempCoord = localPositions.Find(c => c.x == (x + smallx) && c.y == (y + smally));
                        firstArray[x, y] = tempCoord;   
                    }
                }
                int scaledX = newX + scale;
                int scaledY = newY + scale;
                int curX = newX;
                int curY = newY;
                
                for (int i = scale; i < 0; i++)
                {
                    currentWorkArray = new Coords[curX-1, curY-1];
                    for (int y = 0; y < curY-1; y++)
                    {
                        for (int x = 0; x < curX-1; x++)
                        {
                            ConsoleColor[] foreColors = new ConsoleColor[4];
                            ConsoleColor[] backColors = new ConsoleColor[4];
                            char[] chars = new char[4];
                            char charLead = ' ';
                            ConsoleColor foreLead = ConsoleColor.Black;
                            ConsoleColor backLead = ConsoleColor.Black;
                            for (int l = 0; l < 4; l++)
                            {
                                int chX = 0;
                                int chY = 0;
                                if (l == 0) { chX = x + 0; chY = y + 0; }
                                if (l == 1) { chX = x + 1; chY = y + 0; }
                                if (l == 2) { chX = x + 0; chY = y + 1; }
                                if (l == 3) { chX = x + 1; chY = y + 1; }
                                if(firstArray[chX, chY] != null)
                                {
                                    foreColors[l] = firstArray[chX, chY].foreColor;
                                    backColors[l] = firstArray[chX, chY].backColor;
                                    chars[l] = firstArray[chX, chY].character;

                                }
                            }
                            //find most common foreground
                            int leadingAmount = 0;
                            foreach (var item in foreColors)
                            {
                                int count = 0;
                                foreach (var item2 in foreColors)
                                {
                                    if (item == item2) count++;
                                }
                                if(count > leadingAmount)
                                {
                                    foreLead = item;
                                    leadingAmount = count;
                                }
                            }
                            //most common background
                            leadingAmount = 0;
                            foreach (var item in backColors)
                            {
                                int count = 0;
                                foreach (var item2 in backColors)
                                {
                                    if (item == item2) count++;
                                }
                                if (count > leadingAmount)
                                {
                                    backLead = item;
                                    leadingAmount = count;
                                }
                            }
                            //find most common char
                            leadingAmount = 0;
                            foreach (var item in chars)
                            {
                                int count = 0;
                                foreach (var item2 in chars)
                                {
                                    if (item == item2) count++;
                                }
                                if (count > leadingAmount)
                                {
                                    charLead = item;
                                    leadingAmount = count;
                                }
                            }
                            currentWorkArray[x, y] = new Coords(x, y, charLead, foreLead, backLead);
                        }
                    }
                    firstArray = new Coords[currentWorkArray.GetLength(0), currentWorkArray.GetLength(1)];
                    for (int copyX = 0; copyX < firstArray.GetLength(0); copyX++)
                    {
                        for (int copyY = 0; copyY < firstArray.GetLength(1); copyY++)
                        {
                            firstArray[copyX, copyY] = currentWorkArray[copyX, copyY];
                        }
                    }
                    curX = curX - 1;
                    curY = curY - 1;
                }
                //drawwww that shit
                for (int drX = 0; drX < firstArray.GetLength(0); drX++)
                {
                    for (int drY = 0; drY < firstArray.GetLength(1); drY++)
                    {
                        //just a simple way for now to see if this actually works lol
                        Coords tempCoord = firstArray[drX, drY];
                        renderPositions.Add(new Coords((drX + smallx) + x, (drY + smally) + y, tempCoord.character, tempCoord.foreColor, tempCoord.backColor));
                    }
                }
                
            }
        }
    }
    public class Coords
    {
        //all relative to 0,0 on the visual
        public int x;
        public int y;
        public char character;
        public ConsoleColor foreColor;
        public ConsoleColor backColor;
        public Coords(int xx, int yy, char charac, ConsoleColor forecol, ConsoleColor backcol)
        {
            this.x = xx;
            this.y = yy;
            this.character = charac;
            this.foreColor = forecol;
            this.backColor = backcol;
        }

    }

}
