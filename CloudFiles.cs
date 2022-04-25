using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudNotes.Properties;
using System.IO;
using System.Windows;

namespace CloudNotes
{
    internal static class CloudFiles
    {
        public static string LocalFilesPath { get; set; } = Settings.Default["LocalPath"].ToString();
        public static string LocalFilesFullPath { get; set; } = Path.GetFullPath(LocalFilesPath);

        public static void MakeLocalDirectory()
        {
            if (!Directory.Exists(LocalFilesFullPath))
            {
                Directory.CreateDirectory(LocalFilesFullPath);
            }
        }

        public static void MoveToLocalDirectory(string[] pathsToMove)
        {

            foreach (var path in pathsToMove)
            {
                try
                {
                    File.Move(path, Path.Combine(LocalFilesFullPath, Path.GetFileName(path)));
                }catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }

        public static string[] GetFilesFromLocalDirectory()
        {

            return Directory.GetFiles(LocalFilesFullPath);
        }
    }
}
