using RhythmThing.Components;
using RhythmThing.System_Stuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace RhythmThing.Objects.Menu.MenuMusic
{
    class HVVideo : GameObject
    {
        private Visual videoPlayer;
        private IFormatter formatter = new BinaryFormatter();
        private FileStream readStream;
        private string _path = Path.Combine(Program.contentPath, "MenuMusic", "HVMusic", "vidR.cvid");
        private double timePassed = 0;
        private double timePerFrame;
        private int[] _startPoint = new int[] { 0, 0 };
        public override void End()
        {
            readStream.Close();
        }

        public override void Start(Game game)
        {
            type = objType.visual;
            videoPlayer = new Visual();
            videoPlayer.z = -10;
            videoPlayer.active = true;
            readStream = new FileStream(_path, FileMode.Open);
            timePerFrame = (double)1 / (double)30;
            components.Add(videoPlayer);

        }

        public override void Update(double time, Game game)
        {
            if (readStream.Length <= readStream.Position)
            {
                readStream.Position = 0;
            }
            timePassed += time;
            if (timePassed >= timePerFrame)
            {
                timePassed = 0;
                byte[,] toLoad = null;
                videoPlayer.localPositions.Clear();


                toLoad = (byte[,])formatter.Deserialize(readStream);

                videoPlayer.LoadCVidFrame(toLoad, _startPoint);
            }

        }
    }
}
