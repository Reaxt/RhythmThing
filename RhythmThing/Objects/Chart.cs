using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using Newtonsoft.Json;
using System.IO;
using CSCore;
using System.Reflection;
using System.Security.Cryptography;

namespace RhythmThing.Objects
{
    public class Chart : GameObject
    {

        // ---ENUMS N STRUCTS FOR THE CHART---
        public enum collumn
        {
            Left,
            Down,
            Up,
            Right
        }
        //only one difficulty for this game, i cant chart all that
        public struct JsonChart
        {
            public string songPath;
            public float bpm;
            public float offset;
            public string songName;
            public string songAuthor;
            public string chartAuthor;
            //might not implement that one honestly
            public float preview;
            public float previewLength;
            public NoteInfo[] notes;
            public EventInfo[] events;
            public int difficulty;
            public videoInfo video;

        }
        public struct videoInfo
        {
            public int framerate;
            public string videoPath;
            public int frames;
            public int[] startPoint;

        }
        public struct NoteInfo
        {
            public float time;
            public collumn collumn;
        }
        //TODO: Make an enum to define event types
        public struct EventInfo
        {
            //these will be dealt with accordingly depending on the event
            public float time;
            public string name;
            public string data;

        }
        // ---END OF STRUCTS AND ENUMS---
        public static Chart instance;
        private string folderName;
        public string chartPath;
        public JsonChart chartInfo;
        private AudioTrack song;
        private List<NoteInfo> msNotes;
        public string hash;

        private Receiver leftReceiver;
        private Receiver downReceiver;
        private Receiver upReceiver;
        private Receiver rightReceiver;
        public Receiver[] receivers;
        public ScoreHandler scoreHandler;
        private ChartEventHandler chartEventHandler;
        public float beat;
        public float vBeat;
        public float approachBeat;
        public float scoreTime;
        public float missTime;
        public float firstBPM;
        private float lastBPMChangeBeat = 0;
        private double mstoSUB = 0;
        private float beatstoADD = 0;
        
        public Chart(string path)
        {
            folderName = path;
            chartPath = Path.Combine(Directory.GetCurrentDirectory(), "!Content/!Songs", path);
           // Console.WriteLine(chartPath);
            chartInfo = JsonConvert.DeserializeObject<JsonChart>(File.ReadAllText(Path.Combine(chartPath, "ChartInfo.json")));
            if(chartInfo.previewLength == 0)
            {
                chartInfo.previewLength = 3;
            }
            hash = Convert.ToBase64String(Program.mD5.ComputeHash(File.ReadAllBytes(Path.Combine(chartPath, "ChartInfo.json"))));
        }

        public override void End()
        {
            
        }

        public override void Start(Game game)
        {
            /*test plugin loading

            Assembly testAssembly = Utils.PluginLoader.LoadPlugin(Path.Combine(chartPath, "script.dll"));
            SongScript testScript = null;
            foreach(Type type in testAssembly.GetTypes())
            {
                if(typeof(SongScript).IsAssignableFrom(type))
                {
                    testScript = Activator.CreateInstance(type) as SongScript;

                }
            }
            testScript.runScript();
            */
            
            instance = this;
            Arrow.movementAmount = 75; //static amount
            this.components = new List<Component>();
            type = objType.nonvisual;

            approachBeat = (float)Game.approachSpeed * ((float)(chartInfo.bpm) / 60000);
            scoreTime = (float)Game.scoringTime * ((float)(chartInfo.bpm) / 60000);
            missTime = (float)Game.missTime * ((float)(chartInfo.bpm) / 60000);
            //Console.WriteLine(Path.Combine(chartPath, chartInfo.songPath));
            List<NoteInfo> tempL = new List<NoteInfo>();
            List<NoteInfo> tempR = new List<NoteInfo>();
            List<NoteInfo> tempU = new List<NoteInfo>();
            List<NoteInfo> tempD = new List<NoteInfo>();
            foreach (var item in chartInfo.notes)
            {
                switch (item.collumn)
                {
                    case collumn.Left:
                        tempL.Add(item);
                        break;
                    case collumn.Down:
                        tempD.Add(item);
                        break;
                    case collumn.Up:
                        tempU.Add(item);
                        break;
                    case collumn.Right:
                        tempR.Add(item);
                        break;
                    default:
                        break;
                }
            }
            Receiver[] receiverss = new Receiver[4];
            receiverss[0] = new Receiver(collumn.Left, tempL, this);
            receiverss[3] = new Receiver(collumn.Right, tempR, this);
            receiverss[2] = new Receiver(collumn.Up, tempU, this);
            receiverss[1] = new Receiver(collumn.Down, tempD, this);
            

            game.addGameObject(receiverss[0]);
            game.addGameObject(receiverss[1]);
            game.addGameObject(receiverss[2]);
            game.addGameObject(receiverss[3]);

            chartEventHandler = new ChartEventHandler(this, receiverss, new List<EventInfo>(chartInfo.events));

            foreach(NoteInfo noteI in chartInfo.notes)
            {
                //float notems = (float)Math.Ceiling(noteI.time / ((float)chartInfo.bpm / 60000));
                //msNotes.Add()
            }

            //check if theres a video
            if(chartInfo.video.videoPath != null)
            {
                game.addGameObject(new VideoPlayer(Path.Combine(chartPath, chartInfo.video.videoPath),chartPath, Path.Combine(chartPath, "ChartInfo.json"), chartInfo.video, this));
            }

            scoreHandler = new ScoreHandler(this, chartInfo.notes.Length);
            game.addGameObject(scoreHandler);
            game.addGameObject(chartEventHandler);
            firstBPM = chartInfo.bpm;
            song = game.audioManager.addTrack(Path.Combine(chartPath, chartInfo.songPath));
            double tempbeat = beatstoADD + (((TimeConverterFactory.Instance.GetTimeConverterForSource(song.sampleSource).ToTimeSpan(song.sampleSource.WaveFormat, song.sampleSource.Length).TotalMilliseconds) * ((float)(firstBPM) / 60000)));
            VideoPlayer.LastBeat = (float)Math.Round(tempbeat, 2);
            //debug obj
            game.addGameObject(new ChartDebug(this));



            //song.sampleSource.SetPosition(TimeSpan.FromMilliseconds(startBeat / ((float)(chartInfo.bpm) / 60000)));

        }
        public void changeBPM(float newBPM, float beatChanged)
        {
            float beatsPassed = beatChanged - lastBPMChangeBeat;
            lastBPMChangeBeat = beatChanged;
            beatstoADD += beatsPassed;
            double oldMillisecondsPassed = ((double)(beatsPassed) / (double)(chartInfo.bpm)) * 60000.0;
            chartInfo.bpm = newBPM;
            //beatstoSUB = (float)Math.Round(newBeatsPassed, 2);
            mstoSUB += oldMillisecondsPassed;
            approachBeat = (float)Game.approachSpeed * ((float)(chartInfo.bpm) / 60000);
            scoreTime = (float)Game.scoringTime * ((float)(chartInfo.bpm) / 60000);
            missTime = (float)Game.missTime * ((float)(chartInfo.bpm) / 60000);
        }
        public override void Update(double time, Game game)
        {
            //calculate the current "beat"
            double tempbeat = beatstoADD + (((TimeConverterFactory.Instance.GetTimeConverterForSource(song.sampleSource).ToTimeSpan(song.sampleSource.WaveFormat, song.sampleSource.Position).TotalMilliseconds + (chartInfo.offset * 1000) - mstoSUB) * ((float)(chartInfo.bpm) / 60000)));
            beat = (float)Math.Round(tempbeat, 2);

            double tempbeat2 = (((TimeConverterFactory.Instance.GetTimeConverterForSource(song.sampleSource).ToTimeSpan(song.sampleSource.WaveFormat, song.sampleSource.Position).TotalMilliseconds) * ((float)(firstBPM) / 60000)));
            vBeat = (float)Math.Round(tempbeat2, 2);

            if (song.sampleSource.GetLength().TotalMilliseconds <= song.sampleSource.GetPosition().TotalMilliseconds)
            {
                //we are done.
                game.exitViaEsc = false;
                game.notesHit = scoreHandler.hits;
                game.totalNotes = scoreHandler.notes;
                game.songName = this.chartInfo.songName;
                game.songHash = this.hash;
                game.audioManager.removeTrack(song);
                game.sceneManager.loadScene(2);
                
            }
            if(game.input.ButtonStates[Input.ButtonKind.Cancel] == Input.ButtonState.Press)
            {
                game.exitViaEsc = true;
                game.notesHit = scoreHandler.hits;
                game.totalNotes = scoreHandler.notes;
                game.songName = this.chartInfo.songName;
                game.songHash = this.hash;
                game.audioManager.removeTrack(song);
                game.sceneManager.loadScene(2);

            }
            
        }
    }
}
