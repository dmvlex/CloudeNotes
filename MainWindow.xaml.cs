using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Linq;

namespace CloudNotes
{
    enum DropBoxStyle
    {
        Default,
        Enter
    }

    public enum ButtonStatus
    {
        Enabled,
        Disabled
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
        }

        private void MainWindowRendered(object sender, EventArgs e)
        {

            CloudFiles.MakeLocalDirectory();
            if (CloudToken.Exist())
            {
                YaDisk.CreateCloudFolder();
            }
            else
            {
                OpenWebWindow();
            }
        }

        private void InstallDropBoxStyle(DropBoxStyle dropBoxStyle)
        {
            switch (dropBoxStyle)
            {
                case DropBoxStyle.Default:
                    DropBoxBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 17, 110, 214));
                    DropBox.Text = "Файл(-ы) бросать сюда";
                    AllowDrop = true;
                    break;
                case DropBoxStyle.Enter:
                    DropBoxBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 104, 166, 236));
                    DropBox.Text = "Отпускай";
                    break;
                default:
                    break;
            }
            
        }
        
        private void DoYouWannaRegistrate()
        {
            var result = MessageBox.Show("Нет доступа к Я.Диску.\nХотите зарегистрироваться?","Регистрация",MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                OpenWebWindow();
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
            if (!CloudToken.Exist())
            {
                DoYouWannaRegistrate();
            }
            else
            {
                try
                {
                    YaDisk.DownloadFilesFromCloud();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UploadToCloudClick(object sender, RoutedEventArgs e)
        {
            if (!CloudToken.Exist())
            {
                DoYouWannaRegistrate();
            }
            else
            {
                var filesPath = CloudFiles.GetFilesFromLocalDirectory();

                try
                {
                    YaDisk.UploadFilesOnCloud(filesPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //веб браузер
        public static void OpenWebWindow()
        {
            if (!CloudToken.Exist())
            {
                CloudToken.ClearIECookie();
                WebBrowserWindow webBrowserWindow = new WebBrowserWindow();
                webBrowserWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                webBrowserWindow.Show();
            }
            else
            {
                var messageBoxResult = MessageBox.Show("Вы уже зарегистрировались.\nВы уверены, что хотите сменить аккаунт?", "Управление аккаунтом", MessageBoxButton.OKCancel);

                if (MessageBoxResult.OK == messageBoxResult)
                {
                    CloudToken.ClearIECookie();
                    WebBrowserWindow webBrowserWindow = new WebBrowserWindow();
                    webBrowserWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    webBrowserWindow.Show();
                }

            }
        }

       
    }
}
