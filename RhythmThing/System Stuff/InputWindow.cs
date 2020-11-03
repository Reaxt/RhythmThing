using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using RhythmThing.Utils;

namespace RhythmThing.System_Stuff
{
    class InputWindow : NativeWindow
    {
        public event EventHandler<RawInputEventArgs> Input;

        public InputWindow()
        {
            CreateHandle(new CreateParams
            {
                X = 0,
                Y = 0,
                Width = 0,
                Height = 0,
                Style = 0x800000,
                Caption = "RhythmThing input because windows is dumb"
            });
           
        }

        protected override void WndProc(ref Message m)
        {
            Logger.DebugLog(m.Msg.ToString());
            const int WM_INPUT = 0x00FF;
            if(m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);
                Input?.Invoke(this, new RawInputEventArgs(data));
            }

            base.WndProc(ref m);
        }
    }

    class RawInputEventArgs : EventArgs
    {
        public RawInputEventArgs(RawInputData data)
        {
            Data = data;
        }

        public RawInputData Data { get; }
    }
}
