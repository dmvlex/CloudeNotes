using System;
using System.Collections.Generic;
using System.Linq;
using CloudNotes.Properties;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using YandexDisk.Client.Clients;
using System.Windows;

namespace CloudNotes
{
    internal static class YaDisk
    {
        /// <summary>
        /// Имя папки, создаваемой на облаке
        /// </summary>
        public static string CloudFolderName
        {
            get
            { return cloudFolderName;}

            set
            {
                
                if ((value == "") || (value == " "))
                {
                    MessageBox.Show("Имя папки не может быть пустым!", "Ошибка");
                }
                else if(Technical.ForbiddenSymbolsCheck(value))
                {
                    MessageBox.Show("В названии содержаться запрещенные символы!\n(<,>,?,*,\",\\,|,:)", "Ошибка");
                }
                else
                {
                    Settings.Default.CloudFolderName = value;
                    Settings.Default.Save();
                    cloudFolderName = Settings.Default.CloudFolderName;
                    MessageBox.Show("Папка на облаке переименована", "Успех");
                }
            }

        }

        private static string cloudFolderName = Settings.Default.CloudFolderName;

        private static DiskHttpApi diskApi = new DiskHttpApi(CloudToken.UserToken); //Поле подключения к API диска

        /// <summary>
        /// Метод загружает файлы в папку, на подключенный диск
        /// </summary>
        /// <param name="filesPaths">Пути к файлам, для загрузки</param>
        public static async void UploadFilesOnCloud(string[] filesPaths)
        {
            foreach (var file in filesPaths)
            {
                var uploadLink = await diskApi.Files.GetUploadLinkAsync("/"+CloudFolderName+"/"+Path.GetFileName(file),overwrite:true);
                using(var fileStream = File.OpenRead(file))
                {
                    await diskApi.Files.UploadAsync(uploadLink,fileStream);
                }
                
            }

        }

        /// <summary>
        /// Метод загружает файлы из папки на диске, в локальную папку
        /// </summary>
        public static async void DownloadFilesFromCloud()
        {
            LocalFiles.MakeLocalDirectory();
            
            var cloudFolderData = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" + CloudFolderName });

            foreach (var file in cloudFolderData.Embedded.Items)
            {
                string localPathToFile = Path.Combine(LocalFiles.LocalFolderFullPath , file.Name);

                if (!File.Exists(localPathToFile))
                {
                    await diskApi.Files.DownloadFileAsync(path: file.Path, localPathToFile);
                }
                else
                {

                    File.Delete(localPathToFile);
                    await diskApi.Files.DownloadFileAsync(path: file.Path, localPathToFile);
                }

                
            }


        }

        /// <summary>
        /// Метод создает папку, для сохранения файлов на диске
        /// </summary>
        public static async void CreateCloudFolder()
        {
            try
            {
                var rootFolderData = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" }); // данные о корневой папке

                if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(CloudFolderName))) //если нет такой папки
                {
                    await diskApi.Commands.CreateDictionaryAsync("/" + CloudFolderName);
                }
            }catch(Exception e)
            {
                MessageBox.Show($"Произошла ошибка в процессе создания папки на облаке:\n{e.Message}", "Ошибка");
            }
        }
    }
}
