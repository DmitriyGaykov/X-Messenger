using Lib.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using X_Messenger.Model.Assets.Messages;
using X_Messenger.Model.Assets.Messages.StickerMessages;
using X_Messenger.Model.Assets.Messages.TextMessages;
using X_Messenger.Model.Assets.Messages.VoiceMessages;
using X_Messenger.Model.Database;
using X_Messenger.Model.Database.DBObjects;
using X_Messenger.Model.Net;
using X_Messenger.View.Assets.Commands;

namespace X_Messenger.ViewModel.Windows.MainWindow;

internal partial class MainWindowViewModel
{
    public ObservableCollection<Message> MessagesList { get; set; } = new();

    public ListBox ObjectMessages { get; set; }

    public string SendedMessage { get; set; } = "Enter message...";
    public bool VoiceMessageActive { get; set; } = false;

    public ICommand SendMessageCommand { get; private init; }
    public ICommand VoiceMessageCommand { get; private init; }

    private readonly VoiceRecorder recorder = new();

    public async void GetMessagesAsync(int idFrom, int idTo)
    {
        MessagesList.Clear();

        await Task.Run(async () =>
        {
            SqlDataReader set;
            using (DB db = new())
            {
                set = db.GetDataSetByQuery($"select * from getMessages({idFrom}, {idTo})");
                DBMessage dbMsg = new();
                Message msg;

                while (await set.ReadAsync())
                {
                    msg = dbMsg.GetObjectFrom(set) as Message;
                    msg = DefineMessage(msg, msg.IdFrom == idFrom);
                    AddToMessageList(msg);
                }
            }
        });
    }

    public void AddToMessageList(IEnumerable<Message> list)
    {
        foreach(var el in list)
        {
            AddToMessageList(el);
        }
    }

    public void AddToMessageList(Message msg, bool append = true)
    {
        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            if (append)
                MessagesList = new(MessagesList.Prepend(msg));
            else
                MessagesList.Add(msg);
            OnPropertyChanged("MessagesList");
            if(ObjectMessages.Items.Count > 2)
            {
                Application.Current.Dispatcher.Invoke(() => ObjectMessages.ScrollIntoView(ObjectMessages.Items[ObjectMessages.Items.Count - 1]));
            }
        }));
    }


    public void SendTextMessage(object param)
    {
        if (!string.IsNullOrWhiteSpace(SendedMessage) && !Edit(SendedMessage))
        {
            Message msg = new();

            if(SendedMessage.Length > 200)
            {
                SendedMessage = SendedMessage.Substring(0, 200);
                View.Modals.MessageBox.Show("Сообщение обрезано. Максимальная длина сообщения 200 символов");
            }

            msg.IdFrom = Client.GetClient().CurrentUser?.Id.Value ?? 0;
            msg.IdTo = CurrentUser?.Id.Value ?? 0;
            msg.Buffer = Encoding.UTF8.GetBytes(SendedMessage);
            msg.MessageType = Message.TypeOfMessage.Text;

            SendMessageAsync(msg);

            msg = new MyTextMessage(msg);
            AddToMessageList(msg, false);
        }

        SendedMessage = "Enter message...";
        OnPropertyChanged("SendedMessage");
    }

    public void SendMessage(Message msg)
    {
        using DB db = new();
        db.InsertObject(new DBMessage(msg));
    }

    public async Task SendMessageAsync(Message msg) => await Task.Run(() => SendMessage(msg));

    private static Message DefineMessage(Message msg, bool isMine)
    {
        switch (msg.MessageType)
        {
            case Message.TypeOfMessage.Text:
                msg = isMine ? new MyTextMessage(msg) : new FriendTextMessage(msg);
                break;
            case Message.TypeOfMessage.Voice:
                msg = isMine ? new MyVoiceMessage(msg) : new FriendVoiceMessage(msg);
                break;
            case Message.TypeOfMessage.Sticker:
                msg = isMine ? new MyStickerMessage(msg) : new FriendStickerMessage(msg);
                break;
        }
        return msg;
    }
}
