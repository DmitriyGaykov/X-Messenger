global using System.Collections.Concurrent;
using Lib.Converters;
using Lib.MessageNamespace.CallMessages;
using Lib.MessageNamespace.RequestMessages;
using Lib.MessageNamespace.ViewMessages;
using Server.Assets.Database;
using Server.Assets.Database.DBObjects;
using System.Text;

namespace Server.ServerSpace;

public partial class Server
{
    private readonly ConcurrentBag<Message> messages = new();

    public async void MessageControllerStartAsync()
    {
        await Task.Run(() => MessageControllerStart());
    }
    private void MessageControllerStart()
    {
        logger.IInfo("Контролер сообщений начал работать");

        Message msg = new();
        while(!isExit)
        {
            if(!messages.IsEmpty)
            {
                while(!messages.TryTake(out msg));

                MessageHandlerAsync(msg);
            }
        }

        logger.IInfo("Контролер сообщений прекратил работу");
    }

    private async void MessageHandlerAsync(Message msg)
    {
        await Task.Run(() => MessageHandler(msg));
    }

    private void MessageHandler(Message msg)
    {
        logger.Info($"Контроллер сообщений начал работать с сообщением(ID: {msg.Id}, Type: ${ msg.MessageType })");
        EndPoint? to;

        if (msg.NeedResnd)
        {
            switch(msg.MessageType)
            {
                case Message.TypeOfMessage.ViewMessage:
                    {
                        onlineUsers.TryGetValue(msg.IDUserTo, out to);

                        ToDBAsync((ViewMessage)msg); /////////////////////////////

                        if (to is not null)
                            SendAsync(msg, to);
                        else
                            logger.MicroInfo($"Сообщение не отправленно. Человека с ID {msg.IDUserTo} нет в сети");
                    }
                    break;

                case Message.TypeOfMessage.CallMessage:
                    {
                        WorkWithCallMessageAsync((CallMessage)msg);
                    }
                    break;
                case Message.TypeOfMessage.RequestMessage:
                    {
                        var rmsg = (RequestMessage)msg;
                        switch(rmsg.RequestMessageType)
                        {
                            case RequestMessage.TypeOfRequestMessage.RegisterMessage:
                                WorkWithRegistr((RegisterMessage)rmsg);
                                break;
                        }
                    }
                    break;
            }
        }
            
        logger.Info($"Контроллер сообщений закончил работать с сообщением(ID {msg.Id})");
    }

    private async void SendAsync(Message msg, EndPoint to)
    {
        await Task.Run(() => Send(msg, to));
    }
    private void Send(Message msg, EndPoint to)
    {
        var json = MyJsonConverter.ToJson(msg);
        var bytes = Encoding.UTF8.GetBytes(json);

        socket.SendToAsync(bytes, to);

        AddOnWaiting(msg, to);

        logger.MicroInfo($"Контролер сообщений отправил сообщение пользователю с ID {msg.IDUserTo}");
    }
    private async void ToDBAsync(ViewMessage msg)
    {
        // Когда-нибудь реализовать
    }

    private async void WorkWithRegistr(RegisterMessage registerMessage, EndPoint to = null)
    {
        logger.MicroInfo($"Пришел запрос на регистрацию пользователя с именем {registerMessage.User.Name}");
        await Task.Run(() =>
        {
            if (to is null)
            {
                onlineUsers.TryGetValue(registerMessage.IDUserFrom, out to);

                if (to is null)
                    return;
            }

            DBUser user = new(registerMessage.User);
            bool res;

            using(var db = new DB())
            {
                res = db.InsertObject(user);
            }

            RegisterMessage rmsg = new(user, res);
            SendAsync(rmsg, to);

            if(res)
            {
                logger.MicroInfo($"Регистрация успешно произошла! Пользователь { user.Name }");
            }
            else
            {
                logger.MicroWarning($"Регистрация неуспешна! Пользователь {user.Name}");
            }
        });
    }
}
