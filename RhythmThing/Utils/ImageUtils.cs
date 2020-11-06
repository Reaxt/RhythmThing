using Newtonsoft.Json;
using RhythmThing.Components;
using RhythmThing.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace RhythmThing.Utils
{
    public static class ImageUtils
    {
        //TODO: IMPLEMENT TRANSPARENCY INTO CFRAMES
        public static Coords[] BMPToCoords(string pathToBMP)
        {

            Bitmap bitmap = (Bitmap)Image.FromFile(pathToBMP);
            Coords[] coords = new Coords[bitmap.Width * bitmap.Height];
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    if(pixel.A == 1)
                    {
                        //nah
                    } else
                    {

                    ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                    coords.Append(new Coords(x, y, ' ', consoleColor, consoleColor));
                    }
                }
            }
            return coords;

        }
        public static void visualToBMP(Visual visual, string path)
        {
            //THIS FUNCTION IS NOT OPTIMIZED. IT SHOULD NOT BE USED FOR ANYTHING OTHER THAN DEV.
            //first make whats basically a cframe
            //find out the bounds

            int smallestX = int.MaxValue;
            int biggestX = int.MinValue;
            int smallestY = int.MaxValue;
            int biggestY = int.MinValue;

            visual.renderPositions.ForEach(coord =>
            {
                if (smallestX > coord.x) smallestX = coord.x;
                if (smallestY > coord.y) smallestY = coord.y;
                if (biggestX < coord.x) biggestX = coord.x;
                if (biggestY < coord.y) biggestY = coord.y;
            });
            //smallestX and smallestY are now OFFSETS. always subtract by these numbers.
            Bitmap bitmap = new Bitmap((biggestX - smallestX) + 1, (biggestY - smallestY) + 1);
            visual.renderPositions.ForEach(coord =>
            {
                Color color = Color.FromName(coord.backColor.ToString());
                bitmap.SetPixel(coord.x - smallestX, (bitmap.Height-1) - (coord.y - smallestY), color);
            });
            bitmap.Save(Path.Combine(Program.contentPath, path));
        }
        public static void visualToCframe(Visual visual, string path)
        {

            //THIS FUNCTION IS NOT OPTIMIZED. IT SHOULD NOT BE USED FOR ANYTHING OTHER THAN DEV.
            //first make whats basically a cframe
            //find out the bounds
            IFormatter formatter = new BinaryFormatter();

            int smallestX = int.MaxValue;
            int biggestX = int.MinValue;
            int smallestY = int.MaxValue;
            int biggestY = int.MinValue;
            FileStream fileStream = new FileStream(Path.Combine(Program.contentPath, path), FileMode.Create);
            visual.renderPositions.ForEach(coord =>
            {
                if (smallestX > coord.x) smallestX = coord.x;
                if (smallestY > coord.y) smallestY = coord.y;
                if (biggestX < coord.x) biggestX = coord.x;
                if (biggestY < coord.y) biggestY = coord.y;
            });
            //smallestX and smallestY are now OFFSETS. always subtract by these numbers.
            int[,] tempCoords = new int[(biggestX - smallestX) + 1, (biggestY - smallestY) + 1];

            visual.renderPositions.ForEach(coord =>
            {
                tempCoords[coord.x - smallestX, biggestY - (coord.y - smallestY)] = (int)coord.backColor;
            });
            formatter.Serialize(fileStream, tempCoords);


        }
        public static void BMPToBinary(string pathToFolder, string pathToWrite)
        {
            Console.WriteLine("Converting BMP files to Binary file(CVID)..");
            IFormatter formatter = new BinaryFormatter();
            string[] files = Directory.GetFiles(pathToFolder);
            List<Coords>[] data = new List<Coords>[files.Length];
            FileStream fileStream = new FileStream(pathToWrite, FileMode.Create);
            for (int i = 0; i < files.Length; i++)
            {
                Bitmap bitmap = (Bitmap)Image.FromFile(files[i]);

                byte[,] tempCoords = new byte[bitmap.Width, bitmap.Height];

                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        Color pixel = bitmap.GetPixel(x, y);
                        if(pixel.A == 1)
                        {
                            //hm
                        }
                        ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                        tempCoords[x, (bitmap.Height - y) - 1] = (byte)consoleColor;
                    }
                }
                formatter.Serialize(fileStream, tempCoords);
            }
            fileStream.Close();

        }

        public static void BMPToCframe(string pathToBMP, string pathToWrite)
        {
            IFormatter formatter = new BinaryFormatter();
            Bitmap bitmap = (Bitmap)Image.FromFile(pathToBMP);
            byte[,] tempCoords = new byte[bitmap.Width, bitmap.Height];
            FileStream fileStream = new FileStream(pathToWrite, FileMode.Create);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                    tempCoords[x, (bitmap.Height - y) - 1] = (byte)consoleColor;
                }
            }
            formatter.Serialize(fileStream, tempCoords);
            fileStream.Close();

        }



        public static void BMPToCVID(string pathToFolder, string ChartInfoPath, Chart.videoInfo videoInfo, out string path, out int[] startPoint, out int frames)
        {
            string[] files = Directory.GetFiles(pathToFolder);
            Bitmap firstMap = (Bitmap)Image.FromFile(files[0]);
            startPoint = new int[] { 0, 0 };
            startPoint[0] = 50 - (firstMap.Width / 2);
            string path1 = Path.Combine(Directory.GetParent(ChartInfoPath).FullName, "video.cvid");
            BMPToBinary(pathToFolder, path1);
            path = path1;
            Chart.JsonChart chartInfo = JsonConvert.DeserializeObject<Chart.JsonChart>(File.ReadAllText(ChartInfoPath));
            chartInfo.video.frames = files.Length;
            chartInfo.video.startPoint = startPoint;
            chartInfo.video.videoPath = Path.GetFileName(path);
            frames = files.Length;
            File.WriteAllText(ChartInfoPath, JsonConvert.SerializeObject(chartInfo));
        }
    }
}
