using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace RhythmThing.System_Stuff
{
    public class Display
    {

        ushort None = 0x0000;
        ushort FOREGROUND_BLUE = 0x0001;
        ushort FOREGROUND_GREEN = 0x0002;
        ushort FOREGROUND_RED = 0x0004;
        ushort FOREGROUND_INTENSITY = 0x0008;
        ushort BACKGROUND_BLUE = 0x0010;
        ushort BACKGROUND_GREEN = 0x0020;
        ushort BACKGROUND_RED = 0x0040;
        ushort BACKGROUND_INTENSITY = 0x0080;
        ushort COMMON_LVB_LEADING_BYTE = 0x0100;
        ushort COMMON_LVB_TRAILING_BYTE = 0x0200;
        ushort COMMON_LVB_GRID_HORIZONTAL = 0x0400;
        ushort COMMON_LVB_GRID_LVERTICAL = 0x0800;
        ushort COMMON_LVB_GRID_RVERTICAL = 0x1000;
        ushort COMMON_LVB_REVERSE_VIDEO = 0x4000;
        ushort COMMON_LVB_UNDERSCORE = 0x8000;

        private List<Visual> Objects;
        private List<Visual> ObjectsToRemove;
        private List<Visual> ObjectsToAdd;
        private ConcurrentQueue<Visual> _objectsToAdd;
        private ConcurrentQueue<Visual> _objectsToRemove;
        private ScreenFilter screenFilter;
        private bool filterActive;
        ConsoleColor[,] currentForeColors;
        ConsoleColor[,] currentBackColors;
        char[,] currentFinalChars;
        public WindowManager windowManager;
        //consoleb
        //private ConsoleColor[,] finalPixels; //ech terminology consistency what is that
        private double TimePassed;
        public Display()
        {
            Objects = new List<Visual>();
            ObjectsToRemove = new List<Visual>();
            ObjectsToAdd = new List<Visual>();
            TimePassed = 0;

            currentForeColors = new ConsoleColor[Program.ScreenX, Program.ScreenY];
            currentBackColors = new ConsoleColor[Program.ScreenX, Program.ScreenY]; 
            currentFinalChars = new char[Program.ScreenX, Program.ScreenY];
            windowManager = new WindowManager();
            windowManager.InitWindow();
            windowManager.CenterWindow();
            //windowManager.moveWindow(0.75f, 0.75f);
        }

        public void AddObject(GameObject obj)
        {
            //TODO: TURN THIS INTO A ENUM oh hey I did
            if (obj.type == objType.visual)
            {
                foreach (Component component in obj.components)
                {
                    if (component is Visual)
                    {
                        if (component != null)
                        {

                            //_objectsToAdd.Enqueue((Visual)component);
                            ObjectsToAdd.Add((Visual)component);

                        }
                    }
                }
            }
        }

        public void RemoveObject(GameObject obj)
        {
            if (obj.type == objType.visual)
            {
                foreach (Component component in obj.components)
                {
                    if (component != null)
                    {
                        if (component is Visual)
                        {
                            //_objectsToRemove.Enqueue((Visual)component);
                            ObjectsToRemove.Add((Visual)component);
                        }
                    }
                }
            }
        }

        public void AddObject(Visual visual)
        {
            //_objectsToAdd.Enqueue(visual);
            ObjectsToRemove.Add(visual);
        }

        public void ActivateFilter(ScreenFilter filter)
        {
            screenFilter = filter;
            filterActive = true;

        }

        public void DisableFilter()
        {
            filterActive = false;
        }

        public void DrawFrame(double time)
        {

            //remove objects from main list that need to be

            //add objects to main list that need to be
            if (ObjectsToAdd.Count > 0)
            {
                for (int i = 0; i < ObjectsToAdd.Count; i++)
                {
                    try
                    {
                        Objects.Add(ObjectsToAdd[i]);
                    }
                    catch
                    {

                        Console.WriteLine("Tried to add object to the list but failed");
                    }
                }
            }
            ObjectsToAdd.Clear();
            if (ObjectsToRemove.Count > 0)
            {

                for (int i = 0; i < ObjectsToRemove.Count; i++)
                {
                    try
                    {
                        Objects.Remove(ObjectsToRemove[i]);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Tried to remove object from list but failed!");
                    }

                }
            }
            ObjectsToRemove.Clear();
            ConsoleColor[,] finalForeColors = new ConsoleColor[Program.ScreenX, Program.ScreenY];
            ConsoleColor[,] finalBackColors = new ConsoleColor[Program.ScreenX, Program.ScreenY];
            char[,] finalChars = new char[Program.ScreenX, Program.ScreenY];
            //sosorry
            for (int x = 0; x < Program.ScreenX; x++)
            {
                for (int y = 0; y < Program.ScreenY; y++)
                {
                    finalChars[x, y] = ' ';
                }
            }
            //Sort
            Objects.Sort((x, y) => x.z.CompareTo(y.z));
            for (int i = 0; i < Objects.Count; i++)
            {
                if(Objects[i].active)
                {

                    //I should already be going in order I think..

                    //fuck i need to make it work with singles
                    //blech

                    foreach(Coords coord in Objects[i].renderPositions)
                    {
                        if (coord.x < Program.ScreenX && coord.y < Program.ScreenY && coord.x > -1 && coord.y > -1)
                        {
                            finalForeColors[coord.x, coord.y] = coord.foreColor;
                            finalBackColors[coord.x, coord.y] = coord.backColor;
                            finalChars[coord.x, coord.y] = coord.character;

                        }
                        else
                        {
                            //Console.WriteLine("Coord our of bounds!");
                        }
                    }
                }
            }

            if (filterActive)
            {
                DisplayData newData = screenFilter.RunFilt(finalForeColors, finalBackColors, finalChars);
                finalForeColors = newData.foreColors;
                finalBackColors = newData.backColors;
                finalChars = newData.characters;
            }
            //not gonna try drawing this way anymore...
            /*
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < Program.ScreenY; y++)
            {
                for (int x = 0; x < Program.ScreenX; x++)
                {
                    //only draw changes
                    if(currentBackColors[x, y] != finalBackColors[x,y] || currentForeColors[x,y] != finalForeColors[x,y] || currentFinalChars[x,y] != finalChars[x,y])
                    {
                        Console.SetCursorPosition(x, Program.ScreenY - y);
                        Console.ForegroundColor = finalForeColors[x, y];
                        Console.BackgroundColor = finalBackColors[x, y];
                        Console.Write(finalChars[x, y]);
                    }
                    
                }
            }
            */
            //here we go oh boy.
            WindowManager.CHAR_INFO[] buffer = new WindowManager.CHAR_INFO[Program.ScreenX * Program.ScreenY];

            for (int y = 0; y < Program.ScreenY; y++)
            {
                for (int x = 0; x < Program.ScreenX; x++)
                {
                    ushort attributes = None;
                    switch (finalForeColors[x, Program.ScreenY - (y + 1)])
                    {
                        case ConsoleColor.Black:
                            attributes = None;
                            break;
                        case ConsoleColor.DarkBlue:
                            attributes = FOREGROUND_BLUE;
                            break;
                        case ConsoleColor.DarkGreen:
                            attributes = FOREGROUND_GREEN;
                            break;
                        case ConsoleColor.DarkCyan:
                            attributes = (ushort)(FOREGROUND_GREEN | FOREGROUND_BLUE);
                            break;
                        case ConsoleColor.DarkRed:
                            attributes = FOREGROUND_RED;
                            break;
                        case ConsoleColor.DarkMagenta:
                            attributes = (ushort)(FOREGROUND_RED | FOREGROUND_BLUE);
                            break;
                        case ConsoleColor.DarkYellow:
                            attributes = (ushort)(FOREGROUND_RED | FOREGROUND_GREEN);
                            break;
                        case ConsoleColor.Gray:
                            attributes = (ushort)(FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_RED);
                            break;
                        case ConsoleColor.DarkGray:
                            attributes = FOREGROUND_INTENSITY;
                            break;
                        case ConsoleColor.Blue:
                            attributes = (ushort)(FOREGROUND_BLUE | FOREGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Green:
                            attributes = (ushort)(FOREGROUND_GREEN | FOREGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Cyan:
                            attributes = (ushort)(FOREGROUND_BLUE | FOREGROUND_BLUE | FOREGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Red:
                            attributes = (ushort)(FOREGROUND_RED | FOREGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Magenta:
                            attributes = (ushort)(FOREGROUND_BLUE | FOREGROUND_RED | FOREGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Yellow:
                            attributes = (ushort)(FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_INTENSITY);
                            break;
                        case ConsoleColor.White:
                            attributes = (ushort)(FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE | FOREGROUND_INTENSITY);
                            break;
                        default:
                            break;
                    }

                    ushort backattribute = None;
                    switch (finalBackColors[x, Program.ScreenY - (y + 1)])
                    {
                        case ConsoleColor.Black:
                            backattribute = None;
                            break;
                        case ConsoleColor.DarkBlue:
                            backattribute = BACKGROUND_BLUE;
                            break;
                        case ConsoleColor.DarkGreen:
                            backattribute = BACKGROUND_GREEN;
                            break;
                        case ConsoleColor.DarkCyan:
                            backattribute = (ushort)(BACKGROUND_GREEN | BACKGROUND_BLUE);
                            break;
                        case ConsoleColor.DarkRed:
                            backattribute = BACKGROUND_RED;
                            break;
                        case ConsoleColor.DarkMagenta:
                            backattribute = (ushort)(BACKGROUND_RED | BACKGROUND_BLUE);
                            break;
                        case ConsoleColor.DarkYellow:
                            backattribute = (ushort)(BACKGROUND_RED | BACKGROUND_GREEN);
                            break;
                        case ConsoleColor.Gray:
                            backattribute = (ushort)(BACKGROUND_BLUE | BACKGROUND_GREEN | BACKGROUND_RED);
                            break;
                        case ConsoleColor.DarkGray:
                            backattribute = BACKGROUND_INTENSITY;
                            break;
                        case ConsoleColor.Blue:
                            backattribute = (ushort)(BACKGROUND_BLUE | BACKGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Green:
                            backattribute = (ushort)(BACKGROUND_GREEN | BACKGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Cyan:
                            backattribute = (ushort)(BACKGROUND_BLUE | BACKGROUND_BLUE | BACKGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Red:
                            backattribute = (ushort)(BACKGROUND_RED | BACKGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Magenta:
                            backattribute = (ushort)(BACKGROUND_BLUE | BACKGROUND_RED | BACKGROUND_INTENSITY);
                            break;
                        case ConsoleColor.Yellow:
                            backattribute = (ushort)(BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_INTENSITY);
                            break;
                        case ConsoleColor.White:
                            backattribute = (ushort)(BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE | BACKGROUND_INTENSITY);
                            break;
                        default:
                            break;
                    }

                    //im so sorry for these switch cases interop is not nice
                    attributes = (ushort)(backattribute | attributes);

                    buffer[y*Program.ScreenX + x] = new WindowManager.CHAR_INFO {UnicodeChar = finalChars[x, Program.ScreenY- (y+1)], Attributes = attributes };
                }
            }
            //buffer[1 + 1] = new WindowManager.CHAR_INFO { UnicodeChar = 'h', Attributes = (ushort)(BACKGROUND_BLUE | FOREGROUND_GREEN ) };
            windowManager.RenderBuffer(buffer);
            //save changes
            currentBackColors = finalBackColors;
            currentForeColors = finalForeColors;
            currentFinalChars = finalChars;
            
        }
        }
    }
