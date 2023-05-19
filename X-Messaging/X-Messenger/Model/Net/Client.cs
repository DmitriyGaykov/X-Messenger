using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using X_Messenger.Model.Database;
using X_Messenger.Model.Objects;

namespace X_Messenger.Model.Net;

internal partial class Client
{
    private static readonly Client client = new();
    private User currentUser;

    private readonly Settings settings = new();

    private Client()
    {
        AppSettings.Loaded += OnLoad;
        AppSettings.LoadSettingsAsync();
    }

    private void OnLoad(object? sender, bool res)
    {
        colorSettings.AddTheme(StandartTheme, DarkTheme1, DarkTheme2, SunsetTheme, NeonTheme, SwampTheme, SpaceTheme, LavaTheme, ColorMadnessTheme, MonochromeTheme);
        colorSettings.PermiteTheme(settings.NameOfTheme);
        DB.StringConnection = settings.DBString;
    }

    public User? CurrentUser
    {
        get => currentUser;
        set
        {
            currentUser = value ??
                          new User();

            StartIamOnlineAsync();
        }
    }

    public bool NeedAutoConnect { get; set; } = true;

    public Settings AppSettings => settings;

    public static Client GetClient() => client;

    public static void NavigateTo<T>() where T : Window, new()
    {
        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            Window window = new T();
            var oldWindow = Application.Current.MainWindow;
            window?.Show();
            StayMain(in window);
            oldWindow?.Close();
        }));
    }

    public static void StayMain(in Window window)
    {
        Application.Current.MainWindow = window;
    }

    public static void DenyMain(in Window window)
    {
        Application.Current.MainWindow = null;
    }

    public async void StartIamOnlineAsync()
    {
        await Task.Run(async () =>
        {
            DB db;
            SqlCommand command = new("UpdateOnline");
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", currentUser.Id);

            while (true)
            {
                using (db = new DB())
                {
                    await db.ExecuteCommandAsync(command);
                }
                Thread.Sleep(20_000);
            }
        });
    }
}
