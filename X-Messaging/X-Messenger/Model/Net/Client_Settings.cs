using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace X_Messenger.Model.Net;

internal partial class Client
{
    private readonly ColorSettings colorSettings = new();

    public ColorSettings ColorSettings => colorSettings;

    #region Themes

    private readonly Theme StandartTheme = new();

    private readonly Theme DarkTheme1 = new("Темная 1",
                                            new(Color.FromRgb(0x29, 0x29, 0x29)),
                                            new(Color.FromRgb(0x1c, 0x1c, 0x1c)),
                                            new(Color.FromRgb(0x2b, 0x2b, 0x2b)),
                                            new(Color.FromRgb(0x1e, 0x1e, 0x1e)),
                                            new(Color.FromRgb(0x5e, 0x5e, 0x5e)),
                                            new(Color.FromRgb(0x44, 0x44, 0x44)),
                                            new(Color.FromRgb(0xaa, 0x2c, 0x2c)),
                                            Color.FromRgb(0xaa, 0x2c, 0x2c),
                                            Color.FromRgb(0x5e, 0x5e, 0x5e));

    private readonly Theme DarkTheme2 = new("Темная 2",
                                            new(Color.FromRgb(0x12, 0x12, 0x12)),
                                            new(Color.FromRgb(0x1F, 0x1F, 0x1F)),
                                            new(Color.FromRgb(0x30, 0x30, 0x30)),
                                            new(Color.FromRgb(0x25, 0x25, 0x25)),
                                            new(Color.FromRgb(0x78, 0x78, 0x78)),
                                            new(Color.FromRgb(0x3C, 0x3C, 0x3C)),
                                            new(Color.FromRgb(0x9C, 0x27, 0xB0)),
                                            Color.FromRgb(0x9C, 0x27, 0xB0),
                                            Color.FromRgb(0x78, 0x78, 0x78)
                                            );

    private readonly Theme SunsetTheme = new("Закат",
                                             new SolidColorBrush(Color.FromRgb(0x22, 0x11, 0x33)),
                                             new SolidColorBrush(Color.FromRgb(0x44, 0x22, 0x44)),
                                             new SolidColorBrush(Color.FromRgb(0x33, 0x11, 0x33)),
                                             new SolidColorBrush(Color.FromRgb(0x66, 0x33, 0x55)),
                                             new SolidColorBrush(Color.FromRgb(0x99, 0x44, 0x77)),
                                             new SolidColorBrush(Color.FromRgb(0x77, 0x11, 0x66)),
                                             new SolidColorBrush(Color.FromRgb(0xDD, 0x55, 0x99)),
                                             Color.FromRgb(0xDD, 0x55, 0x99),
                                             Color.FromRgb(0x99, 0x44, 0x77)
                                            );

    private readonly Theme NeonTheme = new("Неоновая",
                                            new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00)),
                                            new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x1E)),
                                            new SolidColorBrush(Color.FromRgb(0x12, 0x12, 0x12)),
                                            new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                                            new SolidColorBrush(Color.FromRgb(0xFF, 0x2E, 0x8C)),
                                            new SolidColorBrush(Color.FromRgb(0xFF, 0x55, 0xC2)),
                                            new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0xB0)),
                                            Color.FromRgb(0x00, 0xFF, 0xB0),
                                            Color.FromRgb(0xFF, 0x2E, 0x8C)
                                            );

    private readonly Theme SwampTheme = new("Болотистая",
                                        new(Color.FromRgb(0x0F, 0x33, 0x25)),
                                        new(Color.FromRgb(0x18, 0x50, 0x40)),
                                        new(Color.FromRgb(0x2B, 0x3A, 0x2A)),
                                        new(Color.FromRgb(0x1F, 0x37, 0x2A)),
                                        new(Color.FromRgb(0x4A, 0x9D, 0x6D)),
                                        new(Color.FromRgb(0x40, 0x8D, 0x60)),
                                        new(Color.FromRgb(0x79, 0xCF, 0xA0)),
                                        Color.FromRgb(0x79, 0xCF, 0xA0),
                                        Color.FromRgb(0x4A, 0x9D, 0x6D)
                                       );


    private readonly Theme SpaceTheme = new(name: "Космическая",
                                            windowColor: new SolidColorBrush(Color.FromArgb(0xff, 0x0E, 0x1A, 0x23)),
                                            backgroundMainHeaderColor: new SolidColorBrush(Color.FromArgb(0xff, 0x0A, 0x13, 0x1C)),
                                            backgroundSecondColor: new SolidColorBrush(Color.FromArgb(0xff, 0x0D, 0x1B, 0x24)),
                                            peopleWrapperColor: new SolidColorBrush(Color.FromArgb(0xff, 0x14, 0x20, 0x2B)),
                                            searchColor: new SolidColorBrush(Color.FromArgb(0xff, 0x2F, 0x4D, 0x5A)),
                                            secondMainColor: new SolidColorBrush(Color.FromArgb(0xff, 0x3F, 0x69, 0x76)),
                                            thirdMainColor: new SolidColorBrush(Color.FromArgb(0xff, 0x84, 0xC1, 0xD9)),
                                            colorThirdMain: Color.FromArgb(0xff, 0x84, 0xC1, 0xD9),
                                            colorSecond: Color.FromArgb(0xff, 0x3F, 0x69, 0x76)
                                            );

    private readonly Theme LavaTheme = new("Лавовая",
                                           new(Color.FromRgb(0x6B, 0x00, 0x00)),
                                           new(Color.FromRgb(0x8C, 0x00, 0x00)),
                                           new(Color.FromRgb(0xB3, 0x00, 0x00)),
                                           new(Color.FromRgb(0x99, 0x00, 0x00)),
                                           new(Color.FromRgb(0xFF, 0x4D, 0x4D)),
                                           new(Color.FromRgb(0xFF, 0x33, 0x33)),
                                           new(Color.FromRgb(0xFF, 0xB3, 0xB3)),
                                           Color.FromRgb(0xFF, 0xB3, 0xB3),
                                           Color.FromRgb(0xFF, 0x4D, 0x4D)
                                           );

    private readonly Theme ColorMadnessTheme = new (
                                                   "Цветовое безумие",
                                                   new SolidColorBrush(Color.FromRgb(0xFF, 0xC1, 0x07)),
                                                   new SolidColorBrush(Color.FromRgb(0x8B, 0xC3, 0x4A)),
                                                   new SolidColorBrush(Color.FromRgb(0xFF, 0xEB, 0x3B)),
                                                   new SolidColorBrush(Color.FromRgb(0xCD, 0xDC, 0x39)),
                                                   new SolidColorBrush(Color.FromRgb(0xFF, 0x57, 0x22)),
                                                   new SolidColorBrush(Color.FromRgb(0x9C, 0x27, 0xB0)),
                                                   new SolidColorBrush(Color.FromRgb(0x03, 0xA9, 0xF4)),
                                                   Color.FromRgb(0x03, 0xA9, 0xF4),
                                                   Color.FromRgb(0xFF, 0x57, 0x22)
                                                   );

    private readonly Theme MonochromeTheme = new("Монохром",
                                            new(Color.FromRgb(0x0E, 0x0E, 0x0E)),
                                            new(Color.FromRgb(0x03, 0x03, 0x03)),
                                            new(Color.FromRgb(0x1E, 0x1E, 0x1E)),
                                            new(Color.FromRgb(0x28, 0x28, 0x28)),
                                            new(Color.FromRgb(0x44, 0x44, 0x44)),
                                            new(Color.FromRgb(0x2E, 0x2E, 0x2E)),
                                            new(Color.FromRgb(0x8F, 0x8F, 0x8F)),
                                            Color.FromRgb(0x8F, 0x8F, 0x8F),
                                            Color.FromRgb(0x44, 0x44, 0x44)
                                            );


    #endregion
}

internal record ColorSettings
{
    private readonly Dictionary<string, Theme> themes = new();

    public void AddTheme(params Theme[] themes)
    {
        foreach (var theme in themes)
        {
            this.themes[theme.Name] = theme;
        }
    }

    public void PermiteTheme(string tname)
    {
        Theme theme;
        if (themes.TryGetValue(tname, out theme))
        {
            theme.Permite();
        }
    }

    public ICollection<string> Themes => new ObservableCollection<string>(themes.Keys);
}
internal record Theme
{
    public string Name { get; private init; }

    public SolidColorBrush WindowColor { get; private init; }
    public SolidColorBrush BackgroundMainHeaderColor { get; private init; }
    public SolidColorBrush BackgroundSecondColor { get; private init; }
    public SolidColorBrush PeopleWrapperColor { get; private init; }
    public SolidColorBrush SearchColor { get; private init; }
    public SolidColorBrush SecondMainColor { get; private init; }
    public SolidColorBrush ThirdMainColor { get; private init; }
    public Color ColorThirdMain { get; private init; }
    public Color ColorSecond { get; private init; }

    public Theme()
    {
        Name = "Стандартная";

        WindowColor = new SolidColorBrush(Color.FromArgb(0xff, 0x26, 0x13, 0x32));
        BackgroundMainHeaderColor = new SolidColorBrush(Color.FromArgb(0xff, 0x3e, 0x10, 0x56));
        BackgroundSecondColor = new SolidColorBrush(Color.FromArgb(0xff, 0x2e, 0x0d, 0x40));
        PeopleWrapperColor = new SolidColorBrush(Color.FromArgb(0xff, 0x33, 0x11, 0x45));
        SearchColor = new SolidColorBrush(Color.FromArgb(0xff, 0x50, 0x20, 0x74));
        SecondMainColor = new SolidColorBrush(Color.FromArgb(0xff, 0x70, 0x40, 0x94));
        ThirdMainColor = new SolidColorBrush(Color.FromArgb(0xff, 0xd3, 0x13, 0xa9));
        ColorThirdMain = Color.FromArgb(0xff, 0xd3, 0x13, 0xa9);
        ColorSecond = Color.FromArgb(0xff, 0x70, 0x40, 0x94);
    }

    public Theme(string name,
                 SolidColorBrush windowColor,
                 SolidColorBrush backgroundMainHeaderColor,
                 SolidColorBrush backgroundSecondColor,
                 SolidColorBrush peopleWrapperColor,
                 SolidColorBrush searchColor,
                 SolidColorBrush secondMainColor,
                 SolidColorBrush thirdMainColor,
                 Color colorThirdMain,
                 Color colorSecond)
    {
        Name = name;

        WindowColor = windowColor;
        BackgroundMainHeaderColor = backgroundMainHeaderColor;
        BackgroundSecondColor = backgroundSecondColor;
        PeopleWrapperColor = peopleWrapperColor;
        SearchColor = searchColor;
        SecondMainColor = secondMainColor;
        ThirdMainColor = thirdMainColor;
        ColorThirdMain = colorThirdMain;
        ColorSecond = colorSecond;
    }

    public async void Permite()
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            foreach (var prop in this.GetType().GetProperties())
            {
                Application.Current.Resources[prop.Name] = prop.GetValue(this);
            }
        });
    }
}