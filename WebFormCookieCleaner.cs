using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CloudNotes
{
    internal static class WebFormCookieCleaner
    {
        /// <summary>
        /// Очищает куки браузера Internet Explorer. Работает только на Windows
        /// </summary>
        public static void Clear()
        {
            Process coockieClean = new Process();
            coockieClean.StartInfo.UseShellExecute = false;
            coockieClean.StartInfo.FileName = "cmd.exe";
            coockieClean.StartInfo.Arguments = @"/C " + @"RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2";
            coockieClean.StartInfo.CreateNoWindow = true;
            coockieClean.Start();
        }
    }
}
