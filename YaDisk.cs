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

namespace CloudNotes
{
    internal static class YaDisk
    {
        //https://disk.yandex.ru/client/disk/#access_token=AQAAAABeHUu8AAfYYO68oVeDn02BoRK3rVk-Pjc&token_type=bearer&expires_in=31536000

        public static string CloudFolderName = Settings.Default["CloudFolderName"].ToString(); // Позже стоит сделать выбор имени папки через настройки!!!
        public static string YaToken { get; set; } = CloudToken.AppToken;
        private static DiskHttpApi yaApi = new DiskHttpApi(YaToken);//подключение к api яндекса через токен

        public static async void UploadFilesOnCloud(string[] filesPaths)
        {
            foreach (var file in filesPaths)
            {
                var uploadLink = await yaApi.Files.GetUploadLinkAsync("/"+CloudFolderName+"/"+Path.GetFileName(file),overwrite:true);
                using(var fileStream = File.OpenRead(file))
                {
                    await yaApi.Files.UploadAsync(uploadLink,fileStream);
                }
                
            }

        }

        public static async void DownloadFilesFromCloud()
        {
            CloudFiles.MakeLocalDirectory();

            var cloudFolderData = await yaApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" + CloudFolderName });

            foreach (var file in cloudFolderData.Embedded.Items)
            {
                string localPathToFile = Path.Combine(CloudFiles.LocalFilesFullPath , file.Name);

                if (!File.Exists(localPathToFile))
                {
                    await yaApi.Files.DownloadFileAsync(path: file.Path, localPathToFile);
                }
                else
                {

                    File.Delete(localPathToFile);
                    await yaApi.Files.DownloadFileAsync(path: file.Path, localPathToFile);
                }

                
            }


        }

        public static async void CreateCloudFolder()
        {
            var rootFolderData = await yaApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" }); // данные о корневой папке

            if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(CloudFolderName))) //если нет такой папки
            {
               await yaApi.Commands.CreateDictionaryAsync("/" + CloudFolderName); 
            }

        }


    }
}
