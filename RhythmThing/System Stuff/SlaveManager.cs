using System;
using System.Collections.Generic;
using System.Diagnostics;
using RhythmThing.Components;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.IO;
namespace RhythmThing.System_Stuff
{
    public class SlaveManager
    {
        public static List<SlaveManager> aliveWindows = new List<SlaveManager>();
        public struct SlaveData
        {
            public ConsoleColor[,] foreColors;
            public ConsoleColor[,] backColors;
            public char[,] characters;

        }
        public bool alive = true;
        Process slaveProcess;
        public SlaveData displayData;
        MemoryMappedFile mappedFile;
        private NamedPipeServerStream pipe;
        private StreamWriter writer;
        MemoryMappedViewAccessor accessor;
        long length;
        private int x;
        private int y;
        public List<Visual> visuals;
        public static long getLength(int x, int y)
        {
            return ((x * y) * 4) * 3;
        }
        public void UpdateVisuals()
        {
            displayData.foreColors = new ConsoleColor[x, y];
            displayData.backColors = new ConsoleColor[x, y];
            displayData.characters = new char[x, y];
            visuals.Sort((x, y) => x.z.CompareTo(y.z));
            visuals.ForEach(visual =>
            {
                visual.localPositions.ForEach(coord =>
                {
                    int locX = visual.x + coord.x;
                    int locY = visual.y + coord.y;
                    if(!((locX >= x) ||(locX < 0 ) || (locY >= y) || (locY < 0)))
                    {
                        displayData.foreColors[locX, locY] = coord.foreColor;
                        displayData.backColors[locX, locY] = coord.backColor;
                        displayData.characters[locX, locY] = coord.character;

                    }
                });
            });
            Draw();
        }
        public void Draw()
        {
            int curWrite = 0;
            for (int lX = 0; lX < x; lX += 1)
            {
                for (int lY = 0; lY < y; lY += 1)
                {
                    accessor.Write(curWrite, (int)displayData.foreColors[lX, lY]);
                    curWrite += 4;
                    accessor.Write(curWrite, (int)displayData.backColors[lX, lY]);
                    curWrite += 4;
                    accessor.Write(curWrite, displayData.characters[lX, lY]);
                    curWrite += 2;
                }
            }
        }
        public void CloseWindow()
        {
            if(pipe.IsConnected)
            {
                

                writer.AutoFlush = true;
                writer.WriteAsync("close");
                pipe.Disconnect();
                accessor.Dispose();
                mappedFile.Dispose();
                aliveWindows.Remove(this);
                this.alive = false;
                
            }
        }
        public static void CloseAll()
        {
            SlaveManager[] alive = aliveWindows.ToArray();
            for (int i = 0; i < alive.Length; i++)
            {
                alive[i].CloseWindow();
            }
        }

        public void MoveWindow(float x, float y)
        {
            x = (float)(x / 100) * 0.5f + 0.75f;
            y = (y / 100) * 0.5f + 0.75f;
            writer.Write($"SetWindowPos|{x}|{y}|");
            
            
        }
        public void MoveWindowEase(float startX, float startY, float endX, float endY, float duration, string easing)
        {
            startX = (float)(startX / 100) * 0.5f + 0.75f;
            startY = (startY / 100) * 0.5f + 0.75f;
            endX = (float)(endX / 100) * 0.5f + 0.75f;
            endY = (endY / 100) * 0.5f + 0.75f;
            writer.Write($"SetWindowEase|{startX}|{startY}|{endX}|{endY}|{duration}|{easing}|");
        }
        //if for one reason or another the ease MUST end
        public void EndEase() {
            writer.Write($"StopEase|");
        }
        public SlaveManager(string arg, int x, int y)
        {
            visuals = new List<Visual>();
            this.x = x;
            this.y = y;
            length = getLength(x, y);
            mappedFile = MemoryMappedFile.CreateNew(arg, length);
            displayData = new SlaveData();
            displayData.foreColors = new ConsoleColor[x, y];
            displayData.backColors = new ConsoleColor[x, y];
            displayData.characters = new char[x, y];
            //just for now
            for (int xl = 0; xl < 10; xl++)
            {
                for (int yl = 0; yl < 10; yl++)
                {
                    displayData.foreColors[xl, yl] = ConsoleColor.Red;
                    displayData.backColors[xl, yl] = ConsoleColor.Red;
                    displayData.characters[xl, yl] = arg.ToCharArray()[0];

                }
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "RhythmThing.exe",
                CreateNoWindow = false,
                UseShellExecute = true,
                Arguments = $"{arg} {x} {y}"

            };
            accessor = mappedFile.CreateViewAccessor();

            pipe = new NamedPipeServerStream(arg);

            slaveProcess = Process.Start(processStartInfo);
            pipe.WaitForConnection();
            writer = new StreamWriter(pipe);
            writer.AutoFlush = true;
            aliveWindows.Add(this);
        }

    }
}
