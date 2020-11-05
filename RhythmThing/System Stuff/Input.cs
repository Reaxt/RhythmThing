using Linearstar.Windows.RawInput;
using RhythmThing.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Linearstar.Windows.RawInput.Native;
using System.Linq;
using Newtonsoft.Json;

namespace RhythmThing.System_Stuff
{
    public class Input
    {
        //TODO: HMMM might not be needed with the existing system class around
        //conclusion: will be handled by objects ingame
        //conlcusion 2: it will not!!!

        /*
        private int _leftCode = 75;
        private int _upCode = 72;
        private int _downCode = 80;
        private int _rightCode = 77;
        private int _enterCode = 28;
        private int _escCode = 1;
        */


        private static Input _instance;
        public static Input Instance {
            get {
                if(_instance == null)
                    _instance = new Input();
                return _instance;
            } 
        }

        public bool RebindStatus { get; private set; }
        private ButtonKind _keyToRebind;

        public enum ButtonState
        {
            Off,
            Press,
            Held,
            Lifted
        }

        public enum ButtonKind{
            Up,
            Down,
            Left,
            Right,
            Confirm,
            Cancel
        }

        //should this be with a method?
        public Key left = Key.Left;
        public Key right = Key.Right;
        public Key up = Key.Up;
        public Key down = Key.Down;

        //should these be rebound?
        public Key enter = Key.Enter;
        public Key esc = Key.Escape;



        public Dictionary <ButtonKind, ButtonState> ButtonStates{get; private set;}
        
        private Dictionary<int, ButtonKind> _buttonBindings;


        public bool anythingIsHeld = false;


        private InputWindow window;

        struct RawInputState
        {
            public int Code;
            public bool State;
        }


        private ConcurrentQueue<RawInputState> _inputQueue;

        public void RebindKey(ButtonKind button)
        {
            _keyToRebind = button;
            RebindStatus = true;
        }

        private Input()
        {
            ButtonStates = new Dictionary<ButtonKind, ButtonState>()
            {
                { ButtonKind.Left, ButtonState.Off },
                { ButtonKind.Up, ButtonState.Off },
                { ButtonKind.Down, ButtonState.Off },
                { ButtonKind.Right, ButtonState.Off },
                { ButtonKind.Confirm, ButtonState.Off },
                { ButtonKind.Cancel, ButtonState.Off },
            };

            //GenBindingDictionary();
            _buttonBindings = PlayerSettings.Instance.ButtonBindings;

            var devices = RawInputDevice.GetDevices();
            foreach (var device in devices)
            {
                Logger.DebugLog($"{device.DeviceType}");
            }

            _inputQueue = new ConcurrentQueue<RawInputState>();
            new Thread(() =>
            {
                //create input window
                window = new InputWindow();
                //subscribe to it
                window.Input += (object sender, RawInputEventArgs e) =>
                {
                    switch(e.Data){
                        case RawInputKeyboardData kb_data:
                            Logger.DebugLog(kb_data.Keyboard.ScanCode.ToString());
                            _inputQueue.Enqueue(new RawInputState()
                            {
                                Code = kb_data.Keyboard.ScanCode,
                                State = !kb_data.Keyboard.Flags.HasFlag(RawKeyboardFlags.Up)
                            });
                            break;
                        case RawInputMouseData m_data:
                            break;
                        case RawInputHidData h_data:
                            
                            break;
                        default:
                            break;

                    }
                    
                };

                //inputReceiver;
                //run the thing,,,heck
                RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.ExInputSink | RawInputDeviceFlags.NoLegacy, window.Handle);
                Application.Run();
            }).Start();
            RebindStatus = false;
        }

        public Dictionary<int, ButtonKind> GenDefaultBindings()
        {

            return new Dictionary<int, ButtonKind>()
            {
                { 75, ButtonKind.Left },
                { 72, ButtonKind.Up },
                { 80, ButtonKind.Down },
                { 77, ButtonKind.Right },
                { 28, ButtonKind.Confirm },
                { 1, ButtonKind.Cancel },
            };
        }

        public void SaveCurrentBindings()
        {
            PlayerSettings.Instance.ButtonBindings = this._buttonBindings;
            PlayerSettings.Instance.WriteSettings();
        }
        public void SetBindingsToConfig()
        {
            this._buttonBindings = PlayerSettings.Instance.ButtonBindings;
        }
        public void UpdateInput()
        {


            //checking with held
            foreach (var button in ButtonStates.ToArray())
            {
                if(button.Value == ButtonState.Press)
                {
                    ButtonStates[button.Key] = ButtonState.Held;
                } else if(button.Value == ButtonState.Lifted)
                {
                    ButtonStates[button.Key] = ButtonState.Off;
                }
            }
            


            
            if (!WindowManager.isFocused())
            {
                return;
            }

            int queueSize = _inputQueue.Count;
            for (int i = 0; i < queueSize; i++)
            {
                RawInputState rawInputState;
                while (!_inputQueue.TryDequeue(out rawInputState))
                {
                    Console.WriteLine("");
                    Thread.Yield();
                }

                //for rebinding
                if (RebindStatus)
                {
                    if (rawInputState.State)
                    {
                        //thas the one
                        //this code, is bad.
                        foreach (var item in _buttonBindings.ToArray())
                        {
                            if(item.Value == _keyToRebind)
                            {
                                _buttonBindings.Remove(item.Key);
                                _buttonBindings.Add(rawInputState.Code, _keyToRebind);

                                RebindStatus = false;
                                Logger.DebugLog(JsonConvert.SerializeObject(_buttonBindings));
                            }
                        }
                    }
                    return;
                }


                if (_buttonBindings.ContainsKey(rawInputState.Code))
                {
                    ButtonKind buttonKind = _buttonBindings[rawInputState.Code];
                    if(rawInputState.State && (ButtonStates[buttonKind] == ButtonState.Off || ButtonStates[buttonKind] == ButtonState.Lifted))
                    {
                        ButtonStates[buttonKind] = ButtonState.Press;
                    } else if(!rawInputState.State && (ButtonStates[buttonKind] == ButtonState.Press || ButtonStates[buttonKind] == ButtonState.Held) )
                    {
                        ButtonStates[buttonKind] = ButtonState.Lifted;
                    }
                       
                }



            }


        }


    }
}
