using System.Windows;
using CloudNotes.Properties;
using System.Diagnostics;
using WinForms = System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System;

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

            LocalPathOutput.Text = LocalFiles.LocalFolderFullPath;
            ShowTokenStatus();

        }

        private void ChangeLocalFolder(object sender, RoutedEventArgs e)
        {
            var dialogFileChoise = new WinForms.FolderBrowserDialog();
            var dialogResult = dialogFileChoise.ShowDialog();

            if (dialogResult == WinForms.DialogResult.OK)
            {
                LocalFiles.LocalFolderFullPath = dialogFileChoise.SelectedPath;
                LocalPathOutput.Text = LocalFiles.LocalFolderFullPath;
            }

        } //Нажатие на кнопку смены локального пути 
        private void OpenRegistration(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenRegistrationWindow();
        } //нажатие на кнопку смены аккаунта
        private void CleanCookies(object sender, RoutedEventArgs e)
        {
            WebFormCookieCleaner.Clear();
        } //Нажатие на кнопку очищения куки 
        private void OpenLocalFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo openFolderInfo = new ProcessStartInfo();
                openFolderInfo.Arguments = LocalFiles.LocalFolderFullPath;
                openFolderInfo.FileName = "explorer.exe";

                Process.Start(openFolderInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Во время открытия локальной папки произошла ошибка:\n{ex.Message}", "Ошибка");
            }
        } //Нажатие на открытие локальной папки

        /// <summary>
        /// Выводит получен ли токен, в строке в окне настроек
        /// </summary>
        private void ShowTokenStatus()
        {
            if (CloudToken.IsTokenEmpty)
            {
                TokenStatus.Content = "токен не получен";
                TokenStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 214, 69, 69));
            }
            else
            {
                TokenStatus.Content = "токен получен";
                TokenStatus.Foreground = new SolidColorBrush(Color.FromArgb(255, 146, 214, 69));
            }
        }

        private void SettingsWindowClosed(object sender, System.EventArgs e)
        {
            MainWindow.IsSettingsWindowOpen = false;
        }
    }
}
