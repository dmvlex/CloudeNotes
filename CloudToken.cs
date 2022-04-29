using System;
using CloudNotes.Properties;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CloudNotes
{
    internal static class CloudToken
    {

        /// <summary>
        /// Токен пользователя для подключения к Yandex API
        /// </summary>
        public static string UserToken
        {
            get { return userToken; }
            set
            {
                if (value != "")
                {
                    isTokenEmpty = false;
                    Settings.Default.Token = value;
                    Settings.Default.Save();
                    userToken = Settings.Default.Token;
                }
                else
                {
                    isTokenEmpty = true;
                    Settings.Default.Token = value;
                    Settings.Default.Save();
                    userToken = Settings.Default.Token;
                    MessageBox.Show("Токен был очищен","Внимание!");
                }
            }
        }

        /// <summary>
        /// Пустой ли токен на данный момент?
        /// </summary>
        public static bool IsTokenEmpty
        {
            get {return isTokenEmpty;}
        }

        private static string userToken = Settings.Default.Token; 
        private static bool isTokenEmpty = true;

        /// <summary>
        /// Очищает значение токена пользователя
        /// </summary>
        public static void ClearToken()
        {
            UserToken = "";
        }
    }
}
