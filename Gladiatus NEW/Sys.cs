﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Gladiatus_NEW
{
    class Sys
    {
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
        public static void Handle_exception(Exception ex)
        {
            string path = @"settings\exceptions.txt";
            string content = "=======START=======" + Environment.NewLine;
            content += "Error Message: " + ex.Message + Environment.NewLine;
            content += "Stack Trace: " + ex.StackTrace + Environment.NewLine;
            content += "Date: " + DateTime.Now + Environment.NewLine;
            content += "=======END=======" + Environment.NewLine;
            File.AppendAllText(path, content);
        }

        public static void Sleep()
        {
            SetSuspendState(false, true, false);
        }
    }
}
