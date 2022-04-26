using System;
using CloudNotes.Properties;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CloudNotes
{
    internal static class CloudToken
    {
        public static string AppToken { get; set; } = Settings.Default.Token;

        public static bool Exist()
        {
            if (AppToken != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

        public static void ClearIECookie()
        {
            Process coockieClean = new Process();
            coockieClean.StartInfo.UseShellExecute = false;
            coockieClean.StartInfo.FileName = "cmd.exe";
            coockieClean.StartInfo.Arguments = @"/C " + @"RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2";
            coockieClean.StartInfo.CreateNoWindow = true;
            coockieClean.Start();
        }

        public static void UpdateToken()
        {
            AppToken = Settings.Default.Token;
            YaDisk.YaToken = AppToken;
        }

        public static void ClearToken()
        {
            Settings.Default.Token = "";
            Settings.Default.Save();
            UpdateToken();

        }
    }
}
