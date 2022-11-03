using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using CloudNotes.Properties;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        /// <summary>
        /// Открыто ли в данный момент окно настроек?
        /// </summary>
        public static bool IsSettingsWindowOpen { get; set; } = false;

        /// <summary>
        /// Открыто ли в данный момент окно регистрации?
        /// </summary>
        public static bool IsRegistrationWindowOpen { get; set; } = false;

        private static string[] droppedFilesPaths; //массив всех дропнутых путей файлов
        private string droppedFilesPathsString; //строка со всеми путями дропнутых файлов


        public MainWindow()
        {
            InitializeComponent();
            InstallDropBoxStyle(DropBoxStyle.Default);
            CloudToken.UserToken = Settings.Default.Token;
        }

        /// <summary>
        /// Открывает окно регистрации в Я.Диске
        /// </summary>
        public static void OpenRegistrationWindow()
        {
            if (CloudToken.IsTokenEmpty)
            {
                OpenRegWindow();
            }
            else
            {
                var messageBoxResult = MessageBox.Show("Вы уже зарегистрировались.\nВы уверены, что хотите сменить аккаунт?", "Управление аккаунтом", MessageBoxButton.OKCancel);
                if (MessageBoxResult.OK == messageBoxResult)
                {
                    OpenRegWindow();
                }

            }
        }

        /// <summary>
        /// Метод запрашивает у пользователя разрешение на открытие окна регистрации.
        /// </summary>
        public static void RegistrationRequest()
        {
            var result = MessageBox.Show("Похоже, вы еще не дали приложению доступа к Я.Диску.\nХотите провести регистрацию прямо сейчас?", "Регистрация", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                OpenRegistrationWindow();
            }
        }
        private void MainWindowRendered(object sender, EventArgs e)
        {
            LocalFiles.MakeLocalDirectory();

            if (CloudToken.IsTokenEmpty)
            {
                RegistrationRequest();
            }
        } 

        private async void DropBoxDrop(object sender, DragEventArgs e)
        {
            AllowDrop = false;
            
            //Логика получения файлов через Drag'n'Drop
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
            
            await Task.Run(()=> { LocalFiles.MoveToLocalPath(droppedFilesPaths); }); //Сразу перемещаем файлы в локальную папку
            InstallDropBoxStyle(DropBoxStyle.Default);
        } 
        
        private void OpenSettingsWindow(object sender, RoutedEventArgs e)
        {
            if (!IsSettingsWindowOpen)
            {
                SettingsWindow settingsWindow = new SettingsWindow();
                settingsWindow.Owner = this;
                settingsWindow.Show();
                IsSettingsWindowOpen = true;
            }

        } 

        private async void DownloadFiles(object sender, RoutedEventArgs e)
        {
            DownloadFilesButton.IsEnabled = false;
            DownloadFilesButton.Content = "Cкачивание...";
            if (CloudToken.IsTokenEmpty)
            {
                RegistrationRequest();
            }
            else
            {
                try
                {
                    await YaDiskClient.DownloadFolderAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Во время скачивания файлов из облака произошла ошибка:\n{ex.Message}", "Ошибка");
                }
            }
            DownloadFilesButton.Content = "Загрузить файлы из облака";
            DownloadFilesButton.IsEnabled = true;
        } //Нажатие кнопки загрузки из облака в локальную папку

        private async void UploadToCloud(object sender, RoutedEventArgs e)
        {
            UploadToCloudButton.IsEnabled = false;
            UploadToCloudButton.Content = "Загрузка...";
            if (CloudToken.IsTokenEmpty)
            {
                RegistrationRequest();
            }
            else
            {
                try
                {
                    await YaDiskClient.UploadLocalDirectoryAsync(LocalFiles.LocalFolderFullPath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Во время загрузки файлов в облако произошла ошибка:\n{ex.Message}", "Ошибка");
                }
            }
            UploadToCloudButton.Content = "Загрузить файлы в облако";
            UploadToCloudButton.IsEnabled = true;
        } //Нажатие кнопки загрузки в облако из локальной папки

        private static void OpenRegWindow()
        {
            if (!IsRegistrationWindowOpen)
            {
                WebFormCookieCleaner.Clear();
                RegistartionWindow webBrowserWindow = new RegistartionWindow();
                //присваем во владельцы окна - активное окно
                webBrowserWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                webBrowserWindow.Show();
                IsRegistrationWindowOpen = true;
            }
            
        } //метод, вызывающий окно регистрации


        //Работа со стилями контрола Drag'n'Drop 

        /// <summary>
        /// Метод устнавливает стиль состояния для элемента DropBox'a 
        /// </summary>
        /// <param name="dropBoxStyle">Требуемое состояние</param>
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

        private void DropBoxDragEnter(object sender, DragEventArgs e)
        {
            InstallDropBoxStyle(DropBoxStyle.Enter);
        }

        private void DropBoxDragLeave(object sender, DragEventArgs e)
        {
            InstallDropBoxStyle(DropBoxStyle.Default);
        }
    }
}
