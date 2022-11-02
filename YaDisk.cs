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
using System.Threading;
using YandexDisk.Client;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

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
            { return cloudFolderName; }

            set
            {

                if ((value == "") || (value == " "))
                {
                    MessageBox.Show("Имя папки не может быть пустым!", "Ошибка");
                }
                else if (Technical.ForbiddenSymbolsCheck(value))
                {
                    MessageBox.Show("В названии содержаться запрещенные символы!\n(<,>,?,*,\",\\,|,:)", "Ошибка");
                }
                else
                {
                    Settings.Default.CloudFolderName = value;
                    Settings.Default.Save();
                    cloudFolderName = Settings.Default.CloudFolderName;
                    MessageBox.Show("Папка на облаке переименована", "Успех");
                    CreateCloudFolder();
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
                var uploadLink = await diskApi.Files.GetUploadLinkAsync("/" + CloudFolderName + "/" + Path.GetFileName(file), overwrite: true);
                using (var fileStream = File.OpenRead(file))
                {
                    await diskApi.Files.UploadAsync(uploadLink, fileStream);
                }

            }

        }
        public static async Task UploadFilesOnCloud(string filePath, string LocalFolderName)
        {
            var uploadLink = await diskApi.Files.GetUploadLinkAsync(filePath.Substring(filePath.IndexOf($"{LocalFolderName}") - 1).Replace("\\", "/"), overwrite: true);
            using (var fileStream = File.OpenRead(filePath))
            {
                await diskApi.Files.UploadAsync(uploadLink, fileStream);
            }
        }

        public static async Task GetAllFoldersInFolder(string startPath) //вот этого ебучего франкинштейна я сшил. Осталось придумать, как загружать файлы
        {
            string currentShitFuck = startPath;

            Resource fooResourceDescription = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
            {
                Path = currentShitFuck, //Folder on Yandex Disk
            }, CancellationToken.None);

            IEnumerable<Resource> allFoldersInFolder =
               fooResourceDescription.Embedded.Items.Where(item => item.Type == ResourceType.Dir);

            string fuck = "";

            foreach (var dir in allFoldersInFolder)
            {
                fuck += $"{dir.Name}\n";
                IEnumerable<Resource> allFiles;

                Directory.CreateDirectory($"{LocalFiles.LocalFolderFullPath}\\{currentShitFuck}\\{dir.Name}");
                try
                {
                    Resource nextfooResourceDescription = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
                    {
                        Path = $"{currentShitFuck}/{dir.Name}", //Folder on Yandex Disk
                    }, CancellationToken.None);

                     allFiles = nextfooResourceDescription.Embedded.Items.Where(item => item.Type == ResourceType.File);
                }
                catch 
                {
                    allFiles = null;
                }

                    

                if (allFiles != null)
                {
                    foreach (var filein in allFiles)
                    {
                        await diskApi.Files.DownloadFileAsync(path: filein.Path,
                                                              localFile: $"{LocalFiles.LocalFolderFullPath}\\{currentShitFuck}\\{dir.Name}\\{filein.Name}");
                    }
                }
                await GetAllFoldersInFolder(currentShitFuck + "/" + dir.Name);
            }
        }


       /* public static async Task DownloadAllFilesInFolder()
        {
            //Getting information about folder /foo and all files in it
            Resource fooResourceDescription = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
            {
                Path = "/Saved Files", //Folder on Yandex Disk
            }, CancellationToken.None);





            IEnumerable<Task> downloadingTasks =
                allFilesInFolder.Select(file =>
                  diskApi.Files.DownloadFileAsync(path: file.Path,
                                                  localFile: System.IO.Path.Combine(localFolder, file.Name)));

            //Wait all done
            await Task.WhenAll(downloadingTasks);


            //Getting all files from response
            IEnumerable<Resource> allFilesInFolder =
                fooResourceDescription.Embedded.Items.Where(item => item.Type == ResourceType.File);

            //Path to local folder for downloading files
            string localFolder = @"C:\foo";

            //Run all downloadings in parallel. DiskApi is thread safe.
            
        }*/

        public static async Task UploadLocalFolderOnDiskAsync(string pathToLocalDirectory)
        {
            UploadLocalFolderOnDiskAsync(new DirectoryInfo(pathToLocalDirectory));
        }

        /// <summary>
        /// Полностью копирует директорию на Я.Диск
        /// </summary>
        /// <param name="sourceFolderDirectoryInfo">DirectoryInfo</param>
        /// <returns></returns>
        private static async Task UploadLocalFolderOnDiskAsync(DirectoryInfo sourceFolderDirectoryInfo)
        {
            string LocalFolderName = new DirectoryInfo(LocalFiles.LocalFolderFullPath).Name;
            foreach (DirectoryInfo currentSubfolderDirectory in sourceFolderDirectoryInfo.GetDirectories())
            {
                await CreateCloudFolder(currentSubfolderDirectory.FullName.Substring(currentSubfolderDirectory.FullName.IndexOf($"{LocalFolderName}") - 1).Replace("\\", "/"));
                foreach (FileInfo file in currentSubfolderDirectory.GetFiles())
                {
                    await UploadFilesOnCloud(file.FullName, LocalFolderName);
                }
                UploadLocalFolderOnDiskAsync(currentSubfolderDirectory);
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
                string localPathToFile = Path.Combine(LocalFiles.LocalFolderFullPath, file.Name);

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
            }
            catch (Exception e)
            {
                MessageBox.Show($"Произошла ошибка в процессе создания папки на облаке:\n{e.Message}", "Ошибка");
            }
        }
        public static async Task CreateCloudFolder(string folderName)
        {
            try
            {
                var rootFolderData = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" }); // данные о корневой папке

                if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(folderName))) //если нет такой папки
                {
                    await diskApi.Commands.CreateDictionaryAsync(folderName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Произошла ошибка в процессе создания папки на облаке:\n{e.Message}", "Ошибка");
            }
        }

    }
}
