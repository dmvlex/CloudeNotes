﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using CloudNotes.Properties;

namespace CloudNotes
{
    public partial class WebBrowserWindow : Window
    {
        //RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2 очистка куки IE

        public bool IsTokenFinded { get; set; }

        private string clientID = "a7d8d0875cf447c0a34d437a74fa2d67";
        private Regex regToken = new Regex("access_token=(?<token>[^&]+)",RegexOptions.Compiled);

        public WebBrowserWindow()
        {
            InitializeComponent();
        }

        private void LoginLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith("https://localhost:7834/callback"))
            {
                var matchToken = regToken.Match(e.Uri.AbsoluteUri);

                if (matchToken.Success)
                {
                    Settings.Default.Token = matchToken.Groups["token"].Value;
                    Settings.Default.Save();
                    CloudToken.UpdateToken();
                    IsTokenFinded = true;
                    this.Close();
                }
            }
        }

        private void LoginLoad(object sender, RoutedEventArgs e)
        {
            //ссылка на получение токена
            LoginWebBrowser.Navigate($"https://oauth.yandex.ru/authorize?response_type=token&client_id={clientID}");
        }


        private void WebWindowIsClosed(object sender, EventArgs e)
        {
            
        }
    }
}
