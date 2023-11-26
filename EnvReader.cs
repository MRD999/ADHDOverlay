using System;
using System.IO;

namespace ADHDOverlay
{
    internal class EnvReader
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("OpenAI Key env file not found");
                return;
            }
            else
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    char[] separators = new char[] { '=' };
                    var parts = line.Split(separators,StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                        continue;
                    Environment.SetEnvironmentVariable(parts[0], parts[1]);
                }
            }
        }
    }
}