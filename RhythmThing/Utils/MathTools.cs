using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;

namespace RhythmThing.Utils
{
    public class MathTools
    {
        public static Coords[] Rotate(Coords[] pointsToRotate, float angleInDegrees)
        {
            Coords[] newCoords = new Coords[pointsToRotate.Length];
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            int midx = 0;
            int midy = 0;
            //h
            for (int i = 0; i < pointsToRotate.Length; i++)
            {
                //newCoords[i] = pointsToRotate[i];
                var tempcoord = pointsToRotate[i];
                tempcoord.x = (int)Math.Round((cosTheta * (pointsToRotate[i].x - midx) - sinTheta * (pointsToRotate[i].y - midy) + midx));
                tempcoord.y = (int)Math.Round((sinTheta * (pointsToRotate[i].x - midx) + cosTheta * (pointsToRotate[i].y - midy) + midy));

                newCoords[i] = tempcoord;
            }

            return newCoords;
        }
    }
}
