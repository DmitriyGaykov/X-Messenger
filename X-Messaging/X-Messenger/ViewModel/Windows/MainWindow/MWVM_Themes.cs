using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Net;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel
{
    private ICollection<string> themes = new ObservableCollection<string>();

    public ICollection<string> Themes
    {
        get => themes;
        set
        {
            themes = value;
            OnPropertyChanged("Themes");
        }
    }

    public ICommand OnThemeCommand { get; private init; }

    public async void LoadThemesAsync()
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            Themes = Client.GetClient().ColorSettings.Themes;
        });
    }

    public void OnTheme(object param)
    {
        string name = param as string;
        Client.GetClient().ColorSettings.PermiteTheme(name);
        Client.GetClient().AppSettings.NameOfTheme = name;
        Client.GetClient().AppSettings.Save();
    }
}
