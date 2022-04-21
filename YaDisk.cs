using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace CloudNotes
{
    internal static class YaDisk
    {
        //https://disk.yandex.ru/client/disk/#access_token=AQAAAABeHUu8AAfYYO68oVeDn02BoRK3rVk-Pjc&token_type=bearer&expires_in=31536000

        private static string cloudFolderName = "SavedFromPC"; // Позже стоит сделать выбор имени папки через настройки!!!

        private static string yaToken = "AQAAAABeHUu8AAfYYO68oVeDn02BoRK3rVk-Pjc";
        private static DiskHttpApi yaApi = new DiskHttpApi(yaToken);//подключение к api яндекса через токен

        public static async void UploadFiles(string[] filesPaths)
        {
            foreach (var file in filesPaths)
            {
                var uploadLink = await yaApi.Files.GetUploadLinkAsync("/"+cloudFolderName+"/"+Path.GetFileName(file),overwrite:true);
                using(var fileStream = File.OpenRead(file))
                {
                    await yaApi.Files.UploadAsync(uploadLink,fileStream);
                }
                
            }

        }

        public static async void CreateCloudFolder()
        {
            var rootFolderData = await yaApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" }); // данные о корневой папке

            if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(cloudFolderName))) //если нет такой папки
            {
               await yaApi.Commands.CreateDictionaryAsync("/" + cloudFolderName); 
            }

        }
    }
}
