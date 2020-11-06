using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmThing.Utils
{
    //this is a class sourced from https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor , a simple thing thats not necesarily worth remaking.
    class NearestConsoleColor
    {
        public static ConsoleColor ClosestConsoleColor(byte r, byte g, byte b)
        {
            ConsoleColor ret = 0;
            double rr = r, gg = g, bb = b, delta = double.MaxValue;

            foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
            {
                var n = Enum.GetName(typeof(ConsoleColor), cc);
                var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
                if (t == 0.0)
                    return cc;
                if (t < delta)
                {
                    delta = t;
                    ret = cc;
                }
            }
            //bad apple only, or youtube compressed. need to think about more
            /*
            if(ret == ConsoleColor.Green || ret == ConsoleColor.DarkGreen)
            {
                ret = ConsoleColor.Black;
            }*/
            return ret;
        }
    }
}
