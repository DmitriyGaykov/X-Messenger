global using X_Messenger.View.Windows.AdminPanel.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using X_Messenger.Model.Net;
using X_Messenger.Model.Objects;
using X_Messenger.View.Assets.Commands;

namespace X_Messenger.ViewModel.Windows.AdminPanel;

internal partial class AdminPanelViewModel : INotifyPropertyChanged
{
    private User currentAdmin;

    public event PropertyChangedEventHandler? PropertyChanged;
    public User CurrentAdmin
    {
        get => currentAdmin;
        set
        {
            currentAdmin = value;
            OnPropertyChanged("CurrentAdmin");
        }
    }

    public AdminPanelViewModel(Frame navigator)
    {
        CurrentAdmin = Client.GetClient().CurrentUser;
        this.navigator = navigator;
        ToUsers(new object());

        ToUsersCommand = new RelayCommand(ToUsers, o => true);
        ToStatisticsCommand = new RelayCommand(ToStatistics, o => true);
        ToStickersCommand = new RelayCommand(ToStickers, o => true);

        Users = new ObservableCollection<User>();

        OnSearchCommand = new RelayCommand(OnSearchAsync, o => true);
        OnRemoveAccountCommand = new RelayCommand(OnRemoveAccountAsync, o => true);
        OnStickerCommand = new RelayCommand(OnSticker, o => true);
        OnRemoveStickerCommand = new RelayCommand(OnRemoveStickerAsync, o => true);
    }



    private void OnPropertyChanged(string pName)
    {
        if (PropertyChanged is not null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(pName));
        }
    }
}
