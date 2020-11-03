using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace RhythmThing.System_Stuff
{
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
        public float offset = 0;//ithink
        public void WriteSettings()
        {
            string json = JsonConvert.SerializeObject(_instance);
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
