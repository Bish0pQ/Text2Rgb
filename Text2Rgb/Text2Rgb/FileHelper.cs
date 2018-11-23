﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text2Rgb
{
    public class FileHelper
    {
        public static void LogError(Exception ex, string customMessage = "")
        {
            string errorPath = $"{Environment.CurrentDirectory}\\logs\\error.txt";
            string prefix = $"[{DateTime.Now.ToShortDateString()}] at [{DateTime.Now.ToShortTimeString()}]";

            if (!File.Exists(errorPath))
            {
                File.Create(errorPath);
            }

            //Log the error
            using (StreamWriter sw = File.AppendText(errorPath))
            {
                sw.WriteLine($"New error logged on {prefix}\r\nError Message{ex.Message}\r\nException Details:\r\n{ex.InnerException}");
                if (!string.IsNullOrWhiteSpace(customMessage))
                {
                    sw.WriteLine($"Custom developer message: {customMessage}\r\n");
                }
            }
        }
    }
}
