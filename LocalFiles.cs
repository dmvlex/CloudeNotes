using CloudNotes.Properties;
using System;
using System.IO;
using System.Windows;

namespace CloudNotes
{
    internal static class LocalFiles
    {
        /// <summary>
        /// Путь к локальной папке, откуда файлы попадают в папку на диск.
        /// И в которую попадают из Drag'n'Drop.
        /// </summary>
        public static string LocalFolderFullPath
        {
            get
            {return localFolderFullPath;}
            set
            {
                if (value != "")
                {
                    Settings.Default.LocalPath = value;
                    Settings.Default.Save();
                    localFolderFullPath = Path.GetFullPath(Settings.Default.LocalPath);
                }
                else
                {
                    MessageBox.Show("Путь к локальной папке не может быть пустым", "Ошибка");
                }
            }
        }   

        /// <summary>
        /// Пути ко всем файлам из локальной папки.
        /// </summary>
        public static string[] FilesFromLocalFolder
        {
            get { return Directory.GetFiles(localFolderFullPath);}
        }

        private static string localFolderFullPath = Path.GetFullPath(Settings.Default.LocalPath);

        /// <summary>
        /// Метод создает локальную папку, если таковой нет.
        /// </summary>
        public static void MakeLocalDirectory()
        {
            if (!Directory.Exists(LocalFolderFullPath))
            {
                Directory.CreateDirectory(LocalFolderFullPath);
            }
        }

        /// <summary>
        /// Метод переносит файлы в указанную локальную папку.
        /// </summary>
        /// <param name="filesToMove">Пути к файлам, которые нужно перенести</param>
        public static void MoveToLocalPath(string[] filesToMove)
        {

            foreach (var path in filesToMove)
            {
                try
                {
                    File.Move(path, Path.Combine(LocalFolderFullPath, Path.GetFileName(path)));
                }catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
        }
    }
}
