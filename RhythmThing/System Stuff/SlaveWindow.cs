using System;
using System.Collections.Generic;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;
using System.IO.Pipes;
using RhythmThing.Utils;
using System.Threading;

namespace RhythmThing.System_Stuff
{
    public class SlaveWindow
    {
        private SlaveDisplay display;
        public bool alive = true;
        MemoryMappedFile file;
        private NamedPipeClientStream pipe;
        private SlaveManager.SlaveData data;
        Stopwatch stopwatch = new Stopwatch();
        public double deltaTime;


        //ease variables
        float timepassed;
        float duration;
        float startX;
        float startY;
        float endX;
        float endY;
        string easing;
        bool easeGo;
        //

        public SlaveWindow(string[] arg)
        {
            deltaTime = 0;

            //just dont die for now
            Program.ScreenX = int.Parse(arg[1]);
            Program.ScreenY = int.Parse(arg[2]);
            file = MemoryMappedFile.OpenExisting(arg[0]);
            pipe = new NamedPipeClientStream(arg[0]);
            pipe.Connect();
            
            data = new SlaveManager.SlaveData();
            data.foreColors = new ConsoleColor[Program.ScreenX, Program.ScreenY];
            data.backColors = new ConsoleColor[Program.ScreenX, Program.ScreenY];
            data.characters = new char[Program.ScreenX, Program.ScreenY];

            display = new SlaveDisplay();
            new Thread(() =>
            {
                while (alive)
                {
                    if(!pipe.IsConnected)
                    {
                        alive = false;
                    }
                    Console.WriteLine("a");
                    byte[] messageBuffer = new byte[520];
                    pipe.Read(messageBuffer, 0, 520);
                    string input = Encoding.UTF8.GetString(messageBuffer);
                    if (input.StartsWith("close"))
                    {
                        this.alive = false;
                    }
                    string[] args = input.Split("|");
                    switch (args[0])
                    {
                        case "SetWindowPos":
                            display.windowManager.moveWindow(float.Parse(args[1]), float.Parse(args[2]));
                            break;
                        case "SetWindowEase":
                            startX = float.Parse(args[1]);
                            startY = float.Parse(args[2]);
                            endX = float.Parse(args[3]);
                            endY = float.Parse(args[4]);
                            duration = float.Parse(args[5]);
                            easing = args[6];
                            timepassed = 0;
                            easeGo = true;
                            break;
                        case "StopEase":
                            timepassed = duration;

                            break;
                        default:
                            break;
                    }

                }
            }).Start();
            while (alive)
            {

                
                stopwatch.Start();


                var stream = file.CreateViewStream();
                int curOffset = 0;
                for (int x = 0; x < Program.ScreenX; x++)
                {
                    for (int y = 0; y < Program.ScreenY; y++)
                    {
                        byte[] buffer = new byte[4];
                        stream.Read(buffer, 0, 4);
                        data.foreColors[x, y] = (ConsoleColor)BitConverter.ToInt32(buffer);
                        curOffset += 4;
                        
                        stream.Read(buffer, 0, 4);
                        data.backColors[x, y] = (ConsoleColor)BitConverter.ToInt32(buffer);
                        curOffset += 4;

                        buffer = new byte[2];
                        stream.Read(buffer, 0, 2);
                        data.characters[x, y] = BitConverter.ToChar(buffer);
                        curOffset += 2;
                    }
                }
                string temp;
                //read.ReadLine();
                display.DrawFrame(data);
                //
                if (easeGo)
                {
                    if(timepassed >= duration)
                    {
                        display.windowManager.moveWindow(endX, endY);
                    } else
                    {
                        float x = (startX) + ((endX-startX) * Ease.byName[easing](timepassed / duration));
                        float y = (startY) + ((endY - startY) * Ease.byName[easing](timepassed / duration));
                        display.windowManager.moveWindow(x, y);
                    }
                    timepassed += (float)deltaTime;
                }

                Thread.Sleep(1); //just in case
                stopwatch.Stop();
                deltaTime = stopwatch.ElapsedMilliseconds * 0.001;
                stopwatch.Reset();
            }



        }
        }
    }

