using System.Windows;
using CloudNotes.Properties;
using System.Diagnostics;
using WinForms = System.Windows.Forms;
using System.IO;
using System.Windows.Media;

namespace CloudNotes
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            PathInput.Text = CloudFiles.LocalFilesFullPath;
            CloudPathInput.Text = YaDisk.CloudFolderName;
            CheckTokenStatus();
            
        }

        //костыль.ОЧЕНЬ,ОЧЕНЬ,ОЧЕНЬ КОСТЫЛЬНЫЙ КОСТЫЛЬ
        private void CloudSettingsUpdate()
        {
            YaDisk.CloudFolderName = Settings.Default["CloudFolderName"].ToString();
            YaDisk.CreateCloudFolder();
        }

        private void CheckTokenStatus()
        {
            if (CloudToken.Exist())
            {
                TokenStatus.Content = "токен получен";
                TokenStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 146, 214, 69));
            }
            else
            {
                TokenStatus.Content = "токен не получен";
                TokenStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 214, 69, 69));
            }
        }

        private void LocalFolderSettingsUpdate()
        {
            CloudFiles.LocalFilesPath = Settings.Default["LocalPath"].ToString();
            CloudFiles.LocalFilesFullPath = Path.GetFullPath(CloudFiles.LocalFilesPath);
            CloudFiles.MakeLocalDirectory();
        }

        private void ChangeLocalFolderButtonClick(object sender, RoutedEventArgs e)
        {
            var dialogFileChoise = new WinForms.FolderBrowserDialog();
            var dialogResult = dialogFileChoise.ShowDialog();

            if (dialogResult == WinForms.DialogResult.OK)
            {
                Settings.Default.LocalPath = dialogFileChoise.SelectedPath;
                Settings.Default.Save();
                LocalFolderSettingsUpdate();
                PathInput.Text = CloudFiles.LocalFilesFullPath;
            }
 
        }

        private void ChangeCloudFolderNameClick(object sender, RoutedEventArgs e)
        {
            if (CloudPathInput.Text != "")
            {
                Settings.Default.CloudFolderName = CloudPathInput.Text;
                Settings.Default.Save();
                CloudSettingsUpdate();


                MessageBox.Show("Имя папки на облаке успешно изменено", "Успех");
            }
            else
            {
                MessageBox.Show("Имя папки не может быть пустым", "Ошибка!");
            }
            
        }

        private void WebButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenWebWindow();
        }


        private void CookieCLeanClick(object sender, RoutedEventArgs e)
        {
            CloudToken.ClearIECookie();
        }

       
    }
}
