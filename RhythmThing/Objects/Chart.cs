﻿using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using RhythmThing.System_Stuff;
using Newtonsoft.Json;
using System.IO;
using CSCore;

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
            public string songName;
            public string songAuthor;
            public string chartAuthor;
            //might not implement that one honestly
            public float preview;
            public NoteInfo[] notes;
            public EventInfo[] events;
            public int difficulty;

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
        private string folderName;
        private string chartPath;
        public JsonChart chartInfo;
        private AudioTrack song;
        private List<NoteInfo> msNotes;

        private Receiver leftReceiver;
        private Receiver downReceiver;
        private Receiver upReceiver;
        private Receiver rightReceiver;
        public Receiver[] receivers;
        public ScoreHandler scoreHandler;
        private ChartEventHandler chartEventHandler;
        public float beat;
        public float approachBeat;
        public float scoreTime;
        public float missTime;
        public Chart(string path)
        {
            folderName = path;
            chartPath = Path.Combine(Directory.GetCurrentDirectory(), "!Content/!Songs", path);
           // Console.WriteLine(chartPath);
            chartInfo = JsonConvert.DeserializeObject<JsonChart>(File.ReadAllText(Path.Combine(chartPath, "ChartInfo.json")));
        
        }

        public override void End()
        {
            
        }

        public override void Start(Game game)
        {
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

            scoreHandler = new ScoreHandler(this, chartInfo.notes.Length);
            game.addGameObject(scoreHandler);
            game.addGameObject(chartEventHandler);
            song = game.audioManager.addTrack(Path.Combine(chartPath, chartInfo.songPath));
            //debug obj
            game.addGameObject(new ChartDebug(this));
            float startBeat = 275;
            //song.sampleSource.SetPosition(TimeSpan.FromMilliseconds(startBeat / ((float)(chartInfo.bpm) / 60000)));

        }

        public override void Update(double time, Game game)
        {
            //calculate the current "beat"
            double tempbeat = ((TimeConverterFactory.Instance.GetTimeConverterForSource(song.sampleSource).ToTimeSpan(song.sampleSource.WaveFormat, song.sampleSource.Position).TotalMilliseconds) * ((float)(chartInfo.bpm) / 60000));
            beat = (float)Math.Round(tempbeat, 2);

            if (song.sampleSource.GetLength().TotalMilliseconds <= song.sampleSource.GetPosition().TotalMilliseconds)
            {
                //we are done.
                game.notesHit = scoreHandler.hits;
                game.totalNotes = scoreHandler.notes;
                game.songName = this.chartInfo.songName;
                game.audioManager.removeTrack(song);
                game.sceneManager.loadScene(2);
                
            }
            if(Input.escKey == Input.buttonState.press)
            {
                game.notesHit = scoreHandler.hits;
                game.totalNotes = scoreHandler.notes;
                game.songName = this.chartInfo.songName;
                game.audioManager.removeTrack(song);
                game.sceneManager.loadScene(2);

            }
            
        }
    }
}