using System;
using System.Collections.Generic;
using System.Text;
using RhythmThing.Components;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace RhythmThing.Utils
{
    public class MathTools
    {
        public static Coords[] Rotate(Coords[] pointsToRotate, float angleInDegrees)
        {
            Matrix matrix = new Matrix();
            matrix.Rotate(angleInDegrees);
            PointF[] points = new PointF[pointsToRotate.Length];
            for (int i = 0; i < pointsToRotate.Length; i++)
            {
                points[i] = new PointF(pointsToRotate[i].x, pointsToRotate[i].y);
            }
            matrix.TransformPoints(points);
            Coords[] coords = new Coords[pointsToRotate.Length];
            for (int i = 0; i < points.Length; i++)
            {
                coords[i] = new Coords((int)Math.Round(points[i].X), (int)Math.Round(points[i].Y), pointsToRotate[i].character, pointsToRotate[i].foreColor, pointsToRotate[i].backColor);
                
            }
            return coords;

            
        }
    }
}
