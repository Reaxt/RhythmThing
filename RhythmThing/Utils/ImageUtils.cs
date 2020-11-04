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

        public static Coords[] BMPToCoords(string pathToBMP)
        {

            Bitmap bitmap = (Bitmap)Image.FromFile(pathToBMP);
            Coords[] coords = new Coords[bitmap.Width*bitmap.Height];
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                    coords.Append(new Coords(x, -y, ' ', consoleColor, consoleColor));
                }
            }
            return coords;

        }
        public static void BMPToBinary(string pathToFolder, string pathToWrite)
        {
            Console.WriteLine("Converting BMP to Binary file..");
            IFormatter formatter = new BinaryFormatter();
            string[] files = Directory.GetFiles(pathToFolder);
            List<Coords>[] data = new List<Coords>[files.Length];
            FileStream fileStream = new FileStream(pathToWrite, FileMode.Create);
            for (int i = 0; i < files.Length; i++)
            {
                Bitmap bitmap = (Bitmap)Image.FromFile(files[i]);
                //Coords[] tempCoords = new Coords[bitmap.Width*bitmap.Height];
                /*
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        Color pixel = bitmap.GetPixel(x, y);
                        ConsoleColor consoleColor = NearestConsoleColor.ClosestConsoleColor(pixel.R, pixel.G, pixel.B);
                        tempCoords.Append(new Coords(x, -y, ' ', consoleColor, consoleColor));
                    }
                }*/
                formatter.Serialize(fileStream, bitmap);
            }
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
