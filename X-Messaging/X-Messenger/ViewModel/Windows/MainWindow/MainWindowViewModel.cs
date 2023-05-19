using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Net;
using X_Messenger.View.Assets.Commands;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public MouseButtonEventHandler VoiceEvent { get; set; }

    public ICommand OnSettingsCommand { get; private init; }
    public ICommand OnUpdateCommand { get; private init; }

    public RoutedEventHandler OnSticker { get; init; }

    public bool ShowStickersMenu { get; set; } = false;
    public bool ShowSettings { get; set; } = false;

    public MainWindowViewModel()
    {
        VoiceEvent = new(StartVoice);
        OnSticker += OnStickerEvent;

        ShowAdminPanel = Client.GetClient().CurrentUser.AdminLvl > 0 ? Visibility.Visible : Visibility.Hidden;

        OnSettingsCommand = new RelayCommand(OnSettings, o => true);
        SendMessageCommand = new RelayCommand(SendTextMessage, o => true);
        VoiceMessageCommand = new RelayCommand(OnVoiceMessage, o => true);
        OnEditCommand = new RelayCommand(OnEdit, o => true);
        OnExitCommand = new RelayCommand(OnExit, o => true);
        OnStickerImageCommand = new RelayCommand(OnStickerImage, o => true);
        DeleteChatCommand = new RelayCommand(DeleteChat, o => true);
        OnMessageCommand = new RelayCommand(OnMessage, o => true);
        EditMsgCommand = new RelayCommand(EditMsg, o => true);
        DelMsgCommand = new RelayCommand(DelMsg, o => true);
        OnCrossProfileCommand = ShowProfileCommand = new RelayCommand(ShowProfile, o => true);
        OnAdminPanelCommand = new RelayCommand(OnAdminPanel, o => true);
        OnUpdateCommand = new RelayCommand(OnUpdate, o => true);
        OnThemeCommand = new RelayCommand(OnTheme, o => true);

        recorder.OnStopped += (s, e) => OnStopped(null, null);

        LoadThemesAsync();
        OnUpdate();
        //ForeverOnUpdateAsync();
    }

    private async void ForeverOnUpdateAsync()
    {
        await Task.Run(() =>
        {
            while (true)
            {
                Thread.Sleep(3000);
                OnUpdate();
            }
        });
    }

    public void OnStickerEvent(object sender, RoutedEventArgs e)
    {
        ShowStickersMenu = !ShowStickersMenu;
        OnPropertyChanged("ShowStickersMenu");
    }

    public void OnUpdate(object param = null)
    {
        GetPeopleListAsync();
        GetStickersAsync();
    }
    public void OnSettings(object sender)
    {
        ShowSettings = !ShowSettings;
        OnPropertyChanged("ShowSettings");
    }

    private void OnPropertyChanged(string pName)
    {
        if (PropertyChanged is not null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(pName));
        }
    }
}
