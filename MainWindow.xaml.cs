using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.IO;

namespace CloudNotes
{
    enum DropBoxStyle
    {
        Default,
        Enter
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string[] droppedFilesPaths; //массив всех дропнутых путей файлов
        private string droppedFilesPathsString; //строка со всеми путями дропнутых файлов

        public MainWindow()
        {
            InitializeComponent();
            InstallDropBoxStyle(DropBoxStyle.Default);
            CloudFiles.MakeLocalDirectory();
            YaDisk.CreateCloudFolder();
        }

        private void InstallDropBoxStyle(DropBoxStyle dropBoxStyle)
        {
            switch (dropBoxStyle)
            {
                case DropBoxStyle.Default:
                    DropBoxBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(100, 169, 169, 169));
                    DropBox.Text = "Файл(-ы) бросать сюда";
                    AllowDrop = true;
                    break;
                case DropBoxStyle.Enter:
                    DropBoxBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
                    DropBox.Text = "Отпускай";
                    break;
                default:
                    break;
            }
            
        }

        


        //Эвенты дроп бокса
        private void DropBoxDrop(object sender, DragEventArgs e)
        {
            AllowDrop = false;
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                droppedFilesPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                StringBuilder pathsStringBuilder = new StringBuilder();

                foreach (var path in droppedFilesPaths)
                {
                    pathsStringBuilder.Append($"{path}\n");
                }
                droppedFilesPathsString = pathsStringBuilder.ToString();
            }

            CloudFiles.MoveToLocalDirectory(droppedFilesPaths);
            InstallDropBoxStyle(DropBoxStyle.Default);
        }

        private void DropBoxDragEnter(object sender, DragEventArgs e)
        {
            InstallDropBoxStyle(DropBoxStyle.Enter);
        }

        private void DropBoxDragLeave(object sender, DragEventArgs e)
        {
            InstallDropBoxStyle(DropBoxStyle.Default);
        }

        //вкладка настроек
        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.Show();
        }

        private void SynchronizationButtonClick(object sender, RoutedEventArgs e)
        {
            var filesPath = CloudFiles.GetFilesFromLocalDirectory();

            try
            {
                YaDisk.UploadFiles(filesPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
