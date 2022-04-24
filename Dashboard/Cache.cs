using System;

namespace Dashboard
{
    internal class Cache
    {
        public static void Put(string json)
        {
            string filename = GetFilename();
            File.WriteAllText(filename, json);
        }

        public static string GetFilename()
        {
            return $"elpris{DateOnly.FromDateTime(DateTime.Now):MM-dd-yy}.json";
        }

        public static string? Get()
        {
            var filename = GetFilename();
            if (!File.Exists(filename))
            {
                return null;
            }

            return File.ReadAllText(filename);
        }
    }
}
