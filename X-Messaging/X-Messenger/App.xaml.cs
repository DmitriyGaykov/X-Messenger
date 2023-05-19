using Lib.Assets.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using X_Messenger.Model.Net;

namespace X_Messenger;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private bool isExit = false;
    public App()
    {
        this.Exit += (s, e) =>
        {
            isExit = true;
            Client.GetClient().AppSettings.Save();
        };
    }
}
