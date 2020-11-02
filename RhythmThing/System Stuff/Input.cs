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

namespace RhythmThing.System_Stuff
{
    public class Input
    {
        //TODO: HMMM might not be needed with the existing system class around
        //conclusion: will be handled by objects ingame
        //conlcusion 2: it will not!!!

        const int leftCode = 75;
        const int upCode = 72;
        const int downCode = 80;
        const int rightCode = 77;
        const int enterCode = 28;
        const int escCode = 1;

        private static Input INSTANCE;
        public static Input Instance {
            get {
                if(INSTANCE == null)
                    INSTANCE = new Input();
                return INSTANCE;
            } 
        }

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

            _buttonBindings = new Dictionary<int, ButtonKind>()
            {
                { leftCode, ButtonKind.Left },
                { upCode, ButtonKind.Up },
                { downCode, ButtonKind.Down },
                { rightCode, ButtonKind.Right },
                { enterCode, ButtonKind.Confirm },
                { escCode, ButtonKind.Cancel },
            };


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
            


            anythingIsHeld = false;
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
                    Thread.Yield();
                }



                if(_buttonBindings.ContainsKey(rawInputState.Code))
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
