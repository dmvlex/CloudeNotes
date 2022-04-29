using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CloudNotes
{
    internal static class Technical
    {
        private static char[] forbiddenFolderNameSymbols = new char[] {'*','\"','\\','/','<','>',':','|','?'}; //символы, запрещенные названии фалов

        /// <summary>
        /// Очищает куки браузера Internet Explorer. Работает только на Windows
        /// </summary>
        public static void ClearIECookie()
        {
            Process coockieClean = new Process();
            coockieClean.StartInfo.UseShellExecute = false;
            coockieClean.StartInfo.FileName = "cmd.exe";
            coockieClean.StartInfo.Arguments = @"/C " + @"RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2";
            coockieClean.StartInfo.CreateNoWindow = true;
            coockieClean.Start();
        }

        /// <summary>
        /// Метод проверяет переданую строку на содержание запрещенных символов, 
        /// запрещенных для использования в названиях файлов
        /// </summary>
        /// <param name="stringToCheck">Строка для проверки</param>
        /// <returns>Возвращает true, если символы найдены. Иначе - false</returns>
        public static bool ForbiddenSymbolsCheck(string stringToCheck)
        {
            bool result = false;

            foreach (var symbol in forbiddenFolderNameSymbols)
            {
                if (stringToCheck.Contains(symbol))
                {
                    result = true;
                }
            }

            return result;
        }

    }
}
