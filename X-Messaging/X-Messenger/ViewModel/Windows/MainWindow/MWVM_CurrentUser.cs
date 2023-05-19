using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Assets.Messages;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Messages;
using X_Messenger.Model.Net;
using X_Messenger.Model.Objects;
using X_Messenger.View.Windows.Authorization;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel : INotifyPropertyChanged
{
    private User user = null;

    public string CurrentUserId { get; set; }
    public string CurrentUserName { get; set; }
    public string CurrentUserDescr { get; set; }
    public BitmapImage CurrentUserImage { get; set; }
    public string LastVisit { get; set; }

    public ICommand ClickOnUserCommand { get; private init; }
    public ICommand OnExitCommand { get; private init; }
    public ICommand DeleteChatCommand { get; private init; }

    public int SelectedIndex { get; set; } = 0;

    public User CurrentUser
    {
        get => user;
        set
        {
            user = value;

            string date;
            if (value.LastVisit.Value.Date == DateTime.Now.Date)
                date = $"в {(user.LastVisit.Value.Hour < 10 ? ("0" + user.LastVisit.Value.Hour) : user.LastVisit.Value.Hour)}:{(user.LastVisit.Value.Minute < 10 ? ("0" + user.LastVisit.Value.Minute) : user.LastVisit.Value.Minute)}";
            else
                date = "недавно";
            CurrentUserName = user.Name;
            LastVisit = $"последний раз был в сети {date}";
            CurrentUserId = user.Id.ToString();
            CurrentUserDescr = user.Description;
            CurrentUserImage = user.Source;

            OnPropertyChanged("CurrentUserId");
            OnPropertyChanged("CurrentUserDescr");
            OnPropertyChanged("CurrentUserImage");
            OnPropertyChanged("CurrentUserName");
            OnPropertyChanged("LastVisit");
        }
    }

    public void ClickOnUser(int id)
    {
        SetCurrentUserAsync(id);
    }

    public async Task SetCurrentUserAsync(int id)
    {
        editingMsg = null;
        StopVoiceMessage();
        EndRecord();
        await Task.Run(async () =>
        {
            using DB db = new();
            DBUser user = new();
            user.Id = id;

            db.HaveFilled(ref user);

            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                CurrentUser = user;

                PeopleList = new(PeopleList.Select(el => { el.IsCurrent = el.Id == user.Id; return el; }));
                OnPropertyChanged("PeopleList");
            });
        });
        GetMessagesAsync(Client.GetClient().CurrentUser.Id.Value, id);
    }

    public void OnExit(object param)
    {
        Client.GetClient().CurrentUser = null;
        Window window = new Autorization();
        Client.NavigateTo<Autorization>();
    }

    public async void DeleteChat(object param)
    {
        try
        {
            LeftUser user = param as LeftUser;
            Message msg = new();
            msg.IdFrom = Client.GetClient().CurrentUser.Id.Value;
            msg.IdTo = user.Id.Value;

            using DB db = new();
            await db.DeleteObjectAsync<DBMessage>(new DBMessage(msg));
            View.Modals.MessageBox.Show("Чат удален!");
            this.GetMessagesAsync(msg.IdFrom, msg.IdTo);
        }
        catch
        {
            View.Modals.MessageBox.Show("Удалить чат не удалось");
        }
    }
}
