using System;
using System.Windows;
using System.Windows.Input;
using X_Messenger.Model.Assets.Messages;
using X_Messenger.Model.Assets.Messages.TextMessages;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel
{
    public ICommand OnMessageCommand { get; init; }
    public ICommand EditMsgCommand { get; init; }
    public ICommand DelMsgCommand { get; init; }

    private TextMessage editingMsg = null;
    private DateTime last = DateTime.Now;

    private void OnMessage(object param)
    {
        var dur = DateTime.Now - last;

        last = DateTime.Now;
        if (dur.TotalMilliseconds > 500)
        {
            return;
        }

        Message msg = param as Message;

        if (msg is not null)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                msg.ShowEDMenu = !msg.ShowEDMenu;
                MessagesList = new(MessagesList);
                OnPropertyChanged("MessagesList");
            }));
        }
    }

    private void DelMsg(object param)
    {
        Message msg = param as Message;
        DBMessage dbMsg = new DBMessage(msg);
        dbMsg.DelAll = false;

        DellInDB(dbMsg);

        MessagesList.Remove(msg);
        OnPropertyChanged("MessagesList");
    }

    private async void DellInDB(DBMessage msg)
    {
        try
        {
            using DB db = new();
            await db.DeleteObjectAsync(msg);
        }
        catch
        {
            View.Modals.MessageBox.Show("Ошибка. Удаление не прошло");
        }
    }

    private void EditMsg(object param)
    {
        TextMessage msg = param as TextMessage;

        if (msg is not null)
        {
            editingMsg = msg;
            SendedMessage = msg.Text;
            OnPropertyChanged("SendedMessage");
        }
    }

    private bool Edit(string text)
    {
        if (editingMsg is null) return false;

        editingMsg.Text = text;

        EditInDB(editingMsg);
        editingMsg.ShowEDMenu = false;

        editingMsg = null;

        MessagesList = new(MessagesList);
        OnPropertyChanged("MessagesList");

        return true;
    }

    private async void EditInDB(Message msg)
    {
        try
        {
            using DB db = new();
            await db.UpdateObjectAsync(new DBMessage(msg));
        }
        catch
        {
            View.Modals.MessageBox.Show("Ошибка. Данные в БД не обновились");
        }
    }
}
