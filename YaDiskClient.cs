using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using File = System.IO.File;

namespace CloudNotes
{
    internal static class YaDiskClient
    {
        private static DiskHttpApi diskApi = new DiskHttpApi(CloudToken.UserToken); //Поле подключения к API диска



        /// <summary>
        /// Загружает локальную директорию с Я.Диска.(Глубокое копирование)
        /// </summary>
        /// <returns></returns>
        public static async Task DownloadFolderAsync()
        {
            await DownloadFolderAsync($"/{new DirectoryInfo(LocalFiles.LocalFolderFullPath).Name}");

            MessageBox.Show("Все файлы успешно сохранены","Успех");
        }

        /// <summary>
        /// Загружает переданную директорию на Я.Диск.(Глубокое копирование)
        /// </summary>
        /// <param name="pathToLocalDirectory">Путь к директории</param>
        /// <returns></returns>
        public static async Task UploadLocalDirectoryAsync(string pathToLocalDirectory)
        {
            await UploadLocalDirectoryAsync(new DirectoryInfo(pathToLocalDirectory));
            MessageBox.Show("Файлы загружены", "Успех");
        }

        /// <summary>
        /// Создает папку на Я.Диске по указанному пути
        /// </summary>
        /// <param name="folderPath">Путь к создаваемой папке</param>
        /// <returns></returns>
        public static async Task CreateFolderAsync(string folderPath)
        {
            try
            {
                var rootFolderData = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = "/" }); // данные о корневой папке

                if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(folderPath))) //если нет такой папки
                {
                    await diskApi.Commands.CreateDictionaryAsync(folderPath);
                }
            }
            catch (YandexApiException yaEx)
            {
                /*Здесь игнорируется исключение о попытке создать существующую папку.
                  Костыль? Возможно. Но для функциональности никакого вреда не несет, ибо если папка уже есть, значит
                  вторую создавать нам не нужно*/
                if (!(yaEx.Error.Error == "DiskPathPointsToExistentDirectoryError"))
                {
                    MessageBox.Show($"Произошла ошибка в процессе создания папки на облак2222е:\n{yaEx.Message}", "Ошибка");
                }
            }
            catch (Exception e)
            {

                MessageBox.Show($"Произошла ошибка в процессе создания папки на облаке:\n{e.Message}", "Ошибка");
            }
        }


        /// <summary>
        /// Загружает директорию с Я.Диска.(Глубокое копирование)
        /// </summary>
        /// <param name="pathToFolder">Путь к папке на диске</param>
        /// <returns></returns>
        private static async Task DownloadFolderAsync(string pathToFolder)
        {
            /*В данном методе в основе лежит также рекурсивный алгоритм
              копирования директории*/

            string curentFolderPath = pathToFolder;
            IEnumerable<Resource> allFiles;
            string localSideOfPath = CutLocalPath(LocalFiles.LocalFolderFullPath);

            Resource startFolderResources = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest { Path = curentFolderPath });

            IEnumerable<Resource> allFoldersInFolder =
               startFolderResources.Embedded.Items.Where(item => item.Type == ResourceType.Dir); //получаем все субдиректории в папке

            /*Загрузка файлов из текущей папки. Костыль*/
            allFiles = startFolderResources.Embedded.Items.Where(item => item.Type == ResourceType.File);

            foreach (var filein in allFiles)
            {
                string filePath = $"{localSideOfPath}\\{curentFolderPath}\\{filein.Name}";

                if (File.Exists(filePath))//удаляет файл, что бы перезаписать.
                {
                    File.Delete(filePath);
                }
                await diskApi.Files.DownloadFileAsync(path: filein.Path,
                                                  localFile: filePath);
            }

            foreach (var dir in allFoldersInFolder)
            {

                Directory.CreateDirectory($"{localSideOfPath}\\{curentFolderPath}\\{dir.Name}");
                try
                {
                    Resource currentFolderOnDiskResources = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
                    {
                        Path = $"{curentFolderPath}/{dir.Name}",
                    }, CancellationToken.None);

                    allFiles = currentFolderOnDiskResources.Embedded.Items.Where(item => item.Type == ResourceType.File); // получаем все файлы в субдиректории
                }
                catch (YandexApiException yaEx)
                {
                    allFiles = null;
                    MessageBox.Show($"Произошла ошибка при получении файлов с Диска:\n{yaEx.Message}", "Ошибка");
                }

                if (allFiles != null)
                {

                    foreach (var filein in allFiles)
                    {
                        string filePath = $"{localSideOfPath}\\{curentFolderPath}\\{dir.Name}\\{filein.Name}";

                        if (File.Exists(filePath))//удаляет файл, что бы перезаписать.
                        {
                            File.Delete(filePath);
                        }
                            await diskApi.Files.DownloadFileAsync(path: filein.Path,
                                                              localFile: filePath);
                    }
                }
                await DownloadFolderAsync(curentFolderPath + "/" + dir.Name);
            }
        }

        /// <summary>
        /// Возвращает локальный путь без имени самой локальной папке. Костыль.
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        private static string CutLocalPath(string localPath)
        {
            DirectoryInfo dir = new DirectoryInfo(localPath);

            return localPath.Substring(0, localPath.IndexOf(dir.Name));
        }

        /// <summary>
        /// Загружает переданную директорию на Я.Диск.(Глубокое копирование)
        /// </summary>
        /// <param name="sourceFolderDirectoryInfo">Информация о копируемой директории</param>
        /// <returns></returns>
        private static async Task UploadLocalDirectoryAsync(DirectoryInfo sourceFolderDirectoryInfo)
        {
            /*В данном методе используется рекурсивный метод копирования директории.
              На диск загружаются все файлы, а также вложенные директории и вложенные в них файлы.*/

            string LocalFolderName = new DirectoryInfo(LocalFiles.LocalFolderFullPath).Name;
            await CreateFolderAsync($"/{LocalFolderName}"); //Сначала создается сама передаваемая папка.

            foreach (DirectoryInfo currentSubfolderDirectory in sourceFolderDirectoryInfo.GetDirectories())
            {
                /*Далее проходимся по всем субдиректориям и создаем их.*/

                //Преобразование пути субдиректории к пути, воспринимаемым облаком. Костыль? Да.
                string currentSubDirectoryCloudPath = currentSubfolderDirectory.FullName.Substring(currentSubfolderDirectory.FullName.IndexOf($"{LocalFolderName}") - 1).Replace("\\", "/");

                await CreateFolderAsync(currentSubDirectoryCloudPath);

                /*Пробегаемся по всем файлам в нынешней директории. 
                  Не самый эффективный способ. Но без этого не будут грузится файлы из передаваемой папки*/
                foreach (FileInfo file in sourceFolderDirectoryInfo.GetFiles())
                {
                    await UploadFilesOnCloud(file.FullName, LocalFolderName);
                }

                //Пробегаемся по всем файлам в субдиректории и загружаем на диск
                foreach (FileInfo file in currentSubfolderDirectory.GetFiles())
                {
                    await UploadFilesOnCloud(file.FullName, LocalFolderName);
                }
                UploadLocalDirectoryAsync(currentSubfolderDirectory);
            }
        }
        private static async Task UploadFilesOnCloud(string filePath, string localFolderName)
        {
            var uploadLink = await diskApi.Files.GetUploadLinkAsync(filePath.Substring(filePath.IndexOf($"{localFolderName}") - 1).Replace("\\", "/"), overwrite: true);
            using (var fileStream = File.OpenRead(filePath))
            {
                await diskApi.Files.UploadAsync(uploadLink, fileStream);
            }
        }
    }
}
