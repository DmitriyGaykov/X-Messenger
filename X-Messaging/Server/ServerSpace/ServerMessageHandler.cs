global using System.Collections.Concurrent;
using Lib.Converters;
using Lib.MessageNamespace.ViewMessages;
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

        if (msg.MessageType is not Message.TypeOfMessage.OnlineMessage or Message.TypeOfMessage.AnswerMessage)
        {
            switch(msg.MessageType)
            {
                case Message.TypeOfMessage.ViewMessage:
                    onlineUsers.TryGetValue(msg.IDUserTo, out to);

                    ToDBAsync((ViewMessage)msg);

                    if (to is not null)
                        SendAsync(msg, to);
                    else
                        logger.MicroInfo($"Сообщение не отправленно. Человека с ID { msg.IDUserTo } нет в сети");
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

        logger.MicroInfo($"Контролер сообщений отправил сообщение пользователю с ID {msg.IDUserTo}");
    }
    private async void ToDBAsync(ViewMessage msg)
    {
        // Когда-нибудь реализовать
    }
}
