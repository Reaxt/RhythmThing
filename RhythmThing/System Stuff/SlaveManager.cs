using System;
using System.Collections.Generic;
using System.Diagnostics;
using RhythmThing.Components;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.IO;
using System.Threading.Tasks;

namespace RhythmThing.System_Stuff
{
    public class SlaveManager
    {
        private static List<SlaveManager> _aliveWindows = new List<SlaveManager>();
        public struct SlaveData
        {
            public ConsoleColor[,] foreColors;
            public ConsoleColor[,] backColors;
            public char[,] characters;

        }
        public bool Alive = true;
        Process _slaveProcess;
        public SlaveData displayData;
        MemoryMappedFile mappedFile;
        private NamedPipeServerStream pipe;
        private StreamWriter _writer;
        private MemoryMappedViewAccessor _accessor;
        long length;
        private int _x;
        private int _y;
        private bool _youCanUpdate = true;
        public List<Visual> visuals;
        
        public static long getLength(int x, int y)
        {
            return ((x * y) * 4) * 3;
        }
        public void UpdateVisualsAsync()
        {
            if (_youCanUpdate)
            {
                Task.Run(() => {
                    _youCanUpdate = false;
                    displayData.foreColors = new ConsoleColor[_x, _y];
                    displayData.backColors = new ConsoleColor[_x, _y];
                    displayData.characters = new char[_x, _y];
                    visuals.Sort((x, y) => x.z.CompareTo(y.z));
                    Visual[] h = visuals.ToArray();
                    for (int i = 0; i < h.Length; i++)
                    {
                        Visual visual = h[i];
                        if (visual.Active)
                        {
                            Coords[] coordArray = visual.localPositions.ToArray();
                            for (int z = 0; z < coordArray.Length; z++)
                            {
                                Coords coord = coordArray[z];
                                int locX = visual.x + coord.x;
                                int locY = visual.y + coord.y;
                                if (!((locX >= _x) || (locX < 0) || (locY >= _y) || (locY < 0)))
                                {
                                    displayData.foreColors[locX, locY] = coord.foreColor;
                                    displayData.backColors[locX, locY] = coord.backColor;
                                    displayData.characters[locX, locY] = coord.character;

                                }
                            }


                        }
                    }
                    Draw();
                    _youCanUpdate = true;
                });
            }


        }
        public void DrawAsync()
        {
            int curWrite = 0;
            for (int lX = 0; lX < _x; lX += 1)
            {
                for (int lY = 0; lY < _y; lY += 1)
                {
                    _accessor.Write(curWrite, (int)displayData.foreColors[lX, lY]);
                    curWrite += 4;
                    _accessor.Write(curWrite, (int)displayData.backColors[lX, lY]);
                    curWrite += 4;
                    _accessor.Write(curWrite, displayData.characters[lX, lY]);
                    curWrite += 2;
                }
            }
        }
        public void Draw()
        {
            int curWrite = 0;
            for (int lX = 0; lX < _x; lX += 1)
            {
                for (int lY = 0; lY < _y; lY += 1)
                {
                    _accessor.Write(curWrite, (int)displayData.foreColors[lX, lY]);
                    curWrite += 4;
                    _accessor.Write(curWrite, (int)displayData.backColors[lX, lY]);
                    curWrite += 4;
                    _accessor.Write(curWrite, displayData.characters[lX, lY]);
                    curWrite += 2;
                }
            }
        }
        public void CloseWindow()
        {
            if(pipe.IsConnected)
            {
                

                _writer.AutoFlush = true;
                _writer.WriteAsync("close");
                pipe.Disconnect();
                pipe.Dispose();
                _accessor.Dispose();
                mappedFile.Dispose();
                _aliveWindows.Remove(this);
                this.Alive = false;
                
            }
        }
        public static void CloseAll()
        {
            SlaveManager[] alive = _aliveWindows.ToArray();
            for (int i = 0; i < alive.Length; i++)
            {
                alive[i].CloseWindow();
            }
        }

        public void MoveWindow(float x, float y)
        {
            x = (float)(x / 100) * 0.5f + 0.75f;
            y = (y / 100) * 0.5f + 0.75f;
            Task.Run(() =>
            {
                _writer.Write($"SetWindowPos|{x}|{y}|");

            });
            
            
        }
        public void SetWindowTitle(string title)
        {
            _writer.Write($"SetTitle|{title}|");
        }
        public void MoveWindowEase(float startX, float startY, float endX, float endY, float duration, string easing)
        {
            startX = (float)(startX / 100) * 0.5f + 0.75f;
            startY = (startY / 100) * 0.5f + 0.75f;
            endX = (float)(endX / 100) * 0.5f + 0.75f;
            endY = (endY / 100) * 0.5f + 0.75f;
            Task.Run(() =>
            {
                _writer.Write($"SetWindowEase|{startX}|{startY}|{endX}|{endY}|{duration}|{easing}|");

            });
        }
        //if for one reason or another the ease MUST end
        public void EndEase() {
            _writer.Write($"StopEase|");
        }
        public SlaveManager(string arg, int x, int y)
        {
            visuals = new List<Visual>();
            this._x = x;
            this._y = y;
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
            _accessor = mappedFile.CreateViewAccessor();

            pipe = new NamedPipeServerStream(arg);

            _slaveProcess = Process.Start(processStartInfo);
            pipe.WaitForConnection();
            _writer = new StreamWriter(pipe);
            _writer.AutoFlush = true;
            _aliveWindows.Add(this);
        }

    }
}
