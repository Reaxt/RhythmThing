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
        private bool _alive = true;
        private MemoryMappedFile _file;
        private NamedPipeClientStream _pipe;
        private SlaveManager.SlaveData _data;
        private Stopwatch _stopwatch = new Stopwatch();
        public double deltaTime;


        //ease variables
        private float _timepassed;
        private float _duration;
        private float _startX;
        private float _startY;
        private float _endX;
        private float _endY;
        private string _easing;
        private bool _easeGo;
        //

        public SlaveWindow(string[] arg)
        {
            deltaTime = 0;

            //just dont die for now
            Program.ScreenX = int.Parse(arg[1]);
            Program.ScreenY = int.Parse(arg[2]);
            _file = MemoryMappedFile.OpenExisting(arg[0]);
            _pipe = new NamedPipeClientStream(arg[0]);
            _pipe.Connect();
            
            _data = new SlaveManager.SlaveData();
            _data.foreColors = new ConsoleColor[Program.ScreenX, Program.ScreenY];
            _data.backColors = new ConsoleColor[Program.ScreenX, Program.ScreenY];
            _data.characters = new char[Program.ScreenX, Program.ScreenY];

            display = new SlaveDisplay();
            new Thread(() =>
            {
                while (_alive)
                {
                    if(!_pipe.IsConnected)
                    {
                        _alive = false;
                    }
                    byte[] messageBuffer = new byte[520];
                    _pipe.Read(messageBuffer, 0, 520);
                    string input = Encoding.UTF8.GetString(messageBuffer);
                    if (input.StartsWith("close"))
                    {
                        this._alive = false;
                    }
                    string[] args = input.Split("|");
                    switch (args[0])
                    {
                        case "SetWindowPos":
                            display.windowManager.MoveWindowLegacy(float.Parse(args[1]), float.Parse(args[2]));
                            break;
                        case "SetWindowEase":
                            if(_easeGo)
                            {
                                display.windowManager.MoveWindowLegacy(_endX, _endY);
                                _timepassed = 0;
                            }
                            _startX = float.Parse(args[1]);
                            _startY = float.Parse(args[2]);
                            _endX = float.Parse(args[3]);
                            _endY = float.Parse(args[4]);
                            _duration = float.Parse(args[5]);
                            _easing = args[6];
                            _timepassed = 0;
                            _easeGo = true;
                            break;
                        case "StopEase":
                            _timepassed = _duration;
                            break;
                        case "SetTitle":
                            Console.Title = args[1];
                            break;
                        default:
                            break;
                    }

                }
            }).Start();
            while (_alive)
            {

                
                _stopwatch.Start();


                var stream = _file.CreateViewStream();
                int curOffset = 0;
                for (int x = 0; x < Program.ScreenX; x++)
                {
                    for (int y = 0; y < Program.ScreenY; y++)
                    {
                        byte[] buffer = new byte[4];
                        stream.Read(buffer, 0, 4);
                        _data.foreColors[x, y] = (ConsoleColor)BitConverter.ToInt32(buffer);
                        curOffset += 4;
                        
                        stream.Read(buffer, 0, 4);
                        _data.backColors[x, y] = (ConsoleColor)BitConverter.ToInt32(buffer);
                        curOffset += 4;

                        buffer = new byte[2];
                        stream.Read(buffer, 0, 2);
                        _data.characters[x, y] = BitConverter.ToChar(buffer);
                        curOffset += 2;
                    }
                }
                display.DrawFrame(_data);
                //
                if (_easeGo)
                {
                    if(_timepassed >= _duration)
                    {
                        display.windowManager.MoveWindowLegacy(_endX, _endY);
                        _easeGo = false;
                    } else
                    {
                        float x = (_startX) + ((_endX-_startX) * Ease.byName[_easing](_timepassed / _duration));
                        float y = (_startY) + ((_endY - _startY) * Ease.byName[_easing](_timepassed / _duration));
                        display.windowManager.MoveWindowLegacy(x, y);
                    }
                    _timepassed += (float)deltaTime;
                }

                Thread.Sleep(1); //just in case
                _stopwatch.Stop();
                deltaTime = _stopwatch.ElapsedMilliseconds * 0.001;
                _stopwatch.Reset();
            }



        }
        }
    }

