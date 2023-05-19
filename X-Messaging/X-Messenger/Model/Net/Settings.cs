using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlzEx.Standard;
using Lib.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace X_Messenger.Model.Net;

internal class Settings
{
    private readonly string settfile = "settings.json";

    private EventHandler<bool> loaded;

    private bool isLoaded = false;

    [JsonProperty("database")]
    public string DBString { get; set; } = "Data Source=DIMA;Initial Catalog=X_Messenger;Integrated Security=True";

    [JsonProperty("last theme")]
    public string NameOfTheme { get; set; } = "Стандартная";

    [JsonIgnore]
    public bool IsLoaded 
    { 
        get => isLoaded; 
        private set
        {
            isLoaded = value;
        }
    }

    public event EventHandler<bool> Loaded 
    { 
        add => loaded += value; 
        remove
        {
            if(loaded is not null && value is not null) 
            {
                loaded -= value;
            }
        }
    }

    public void Save()
    {
        string json = MyJsonConverter.ToJson(this);
        using StreamWriter sw = new(settfile);
        sw.Write(json);
    }

    private async Task RecvFromFile()
    {
        using StreamReader sw = new(settfile);

        string json = await sw.ReadToEndAsync();
        Settings? settings = MyJsonConverter.FromJson<Settings>(json);

        if (settings is null) throw new Exception("Файл с настройками(settings.json) не создан или поврежден. Будут взяты найстройки по умолчанию");

        this.DBString = settings.DBString;
        this.NameOfTheme = settings.NameOfTheme;
    }

    public async void LoadSettingsAsync()
    {
        try
        {
            try
            {
                await RecvFromFile();
            }
            catch (Exception e)
            {
                View.Modals.MessageBox.Show(e.Message);
                throw new Exception(e.Message);
            }
        }
        catch (Exception ex)
        {
            using StreamWriter sw = new(settfile);
            await sw.WriteAsync(JsonConvert.SerializeObject(this));
        }
        IsLoaded = true;
        loaded?.Invoke(this, IsLoaded);
    }
}
