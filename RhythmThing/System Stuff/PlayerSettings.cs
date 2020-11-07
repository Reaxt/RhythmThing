using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RhythmThing.Utils;

namespace RhythmThing.System_Stuff
{
    public struct ChartScore
    {
        public string hashString;
        public string letter;
        public float percent;
        public ChartScore(string hash, string inLetter, float inPercent)
        {
            this.letter = inLetter;
            this.percent = inPercent;
            this.hashString = hash;
        }
    }
    public class PlayerSettings
    {
        
        private static PlayerSettings _instance;
        public static PlayerSettings Instance
        {
            get
            {
                
                if (_instance == null)
                    _instance = new PlayerSettings();
                return _instance;
            }
        }
        public PlayerSettings()
        {
            if(this.chartScores == null)
            {
                this.chartScores = new Dictionary<string, ChartScore>();
            }
        }
        //default settings
        /*
        public int LeftBind = 75;
        public int RightBind = 77;
        public int UpBind = 72;
        public int DownBind = 80;
        public int ConfirmBind = 28;
        public int CancelBind = 1;
        */

        public Dictionary<int, Input.ButtonKind> ButtonBindings;
        public Dictionary<string, ChartScore> chartScores;
        public float offset = 0;//ithink
        public void WriteSettings()
        {
            string json = JsonConvert.SerializeObject(Instance);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "!Content","PlayerSettings.json"), json);
            ReadSettings();
        }
        public void ReadSettings()
        {
            
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "!Content", "PlayerSettings.json"))) {
                _instance = JsonConvert.DeserializeObject<PlayerSettings>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "!Content", "PlayerSettings.json")));

            } else
            {
                WriteDefaultSettings();
            }
            LoadSums();

        }
        public void SaveScore(string hash, string letter, float percent)
        {
            Instance.chartScores[hash] = new ChartScore(hash, letter, percent);
            WriteSettings();
        }
        private void LoadSums()
        {
            string[] songPaths = Directory.GetDirectories(Path.Combine(Program.contentPath, "!Songs"));
            bool anyNew = false;
            for (int i = 0; i < songPaths.Length; i++)
            {
                if (File.Exists(Path.Combine(songPaths[i], "ChartInfo.json"))){
                    bool alreadyIn = false;
                    string hash = HashUtils.GetHash(Path.Combine(songPaths[i], "ChartInfo.json"));

                    foreach (var item in Instance.chartScores)
                    {
                        if(item.Key == hash)
                        {
                            alreadyIn = true;
                        }
                    }
                    if (!alreadyIn)
                    {
                        ChartScore newScore = new ChartScore(hash, "NA", 0);
                        Instance.chartScores.Add(hash, newScore);
                        anyNew = true;
                    }
                    
                }
            }
            if (anyNew)
            {
                WriteSettings();
            }
        }
        public void WriteDefaultSettings()
        {
            ButtonBindings = Input.Instance.GenDefaultBindings();
            offset = 0;
            string json = JsonConvert.SerializeObject(_instance);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "!Content", "PlayerSettings.json"), json);
            ReadSettings();
            Input.Instance.SetBindingsToConfig();
        }
    }
    static class SerializableOptions
    {

    }
}
