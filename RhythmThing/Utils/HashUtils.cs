using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RhythmThing.Utils
{
    public class HashUtils
    {
        public static string GetHash(string path)
        {
            return Convert.ToBase64String(Program.mD5.ComputeHash(File.ReadAllBytes(path)));
        }
    }
}
