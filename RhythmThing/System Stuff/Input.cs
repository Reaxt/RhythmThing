using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
namespace RhythmThing.System_Stuff
{
    public class Input
    {
        //TODO: HMMM might not be needed with the existing system class around
        //conclusion: will be handled by objects ingame
        //conlcusion 2: it will not!!!
        public enum buttonState
        {
            off,
            press,
            held,
            lifted
        }
        //should this be with a method?
        public static Key left = Key.Left;
        public static Key right = Key.Right;
        public static Key up = Key.Up;
        public static Key down = Key.Down;

        //should these be rebound?
        public static Key enter = Key.Enter;
        public static Key esc = Key.Escape;

        public static buttonState leftKey;
        public static buttonState upKey;
        public static buttonState downKey;
        public static buttonState rightKey;
        public static buttonState enterKey;
        public static buttonState escKey;
        
        //this wont work with a joystick sorry
        public Input()
        {
            leftKey = buttonState.off;
            upKey = buttonState.off;
            downKey = buttonState.off;
            rightKey = buttonState.off;
            enterKey = buttonState.off;
            escKey = buttonState.off;

        }
        
        public static void UpdateInput()
        {
            if (!WindowManager.isFocused())
            {
                return;
            }
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                if (escKey == buttonState.off)
                {
                    escKey = buttonState.press;
                }
                else if (escKey == buttonState.press)
                {
                    escKey = buttonState.held;
                }
            }
            else
            {
                if (escKey == buttonState.held || escKey == buttonState.press)
                {
                    escKey = buttonState.lifted;
                }
                else
                {
                    escKey = buttonState.off;
                }
            }

            if (Keyboard.IsKeyDown(Key.Left))
            {
                if (leftKey == buttonState.off)
                {
                    leftKey = buttonState.press;
                } else if (leftKey == buttonState.press)
                {
                    leftKey = buttonState.held;
                }
            } else
            {
                if (leftKey == buttonState.held || leftKey == buttonState.press)
                {
                    leftKey = buttonState.lifted;
                } else
                {
                    leftKey = buttonState.off;
                }
            }



            if (Keyboard.IsKeyDown(Key.Up))
            {
                if (upKey == buttonState.off)
                {
                    upKey = buttonState.press;
                }
                else if (upKey == buttonState.press)
                {
                    upKey = buttonState.held;
                }
            }
            else
            {
                if (upKey == buttonState.held || upKey == buttonState.press)
                {
                    upKey = buttonState.lifted;
                }
                else
                {
                    upKey = buttonState.off;
                }
            }


            if (Keyboard.IsKeyDown(Key.Right))
            {
                if (rightKey == buttonState.off)
                {
                    rightKey = buttonState.press;
                }
                else if (rightKey == buttonState.press)
                {
                    rightKey = buttonState.held;
                }
            }
            else
            {
                if (rightKey == buttonState.held || rightKey == buttonState.press)
                {
                    rightKey = buttonState.lifted;
                }
                else
                {
                    rightKey = buttonState.off;
                }
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                if (downKey == buttonState.off)
                {
                    downKey = buttonState.press;
                }
                else if (downKey == buttonState.press)
                {
                    downKey = buttonState.held;
                }
            }
            else
            {
                if (downKey == buttonState.held || downKey == buttonState.press)
                {
                    downKey = buttonState.lifted;
                }
                else
                {
                    downKey = buttonState.off;
                }
            }


            if (Keyboard.IsKeyDown(Key.Enter))
            {
                if (enterKey == buttonState.off)
                {
                    enterKey = buttonState.press;
                }
                else if (enterKey == buttonState.press)
                {
                    enterKey = buttonState.held;
                }
            }
            else
            {
                if (enterKey == buttonState.held || enterKey == buttonState.press)
                {
                    enterKey = buttonState.lifted;
                }
                else
                {
                    enterKey = buttonState.off;
                }
            }

        }
        
    }
}
