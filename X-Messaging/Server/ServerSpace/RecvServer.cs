using Lib.Converters;
using Lib.MessageNamespace;
using Lib.MessageNamespace.ViewMessages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.ServerSpace;

public partial class Server
{
    private const int MAXBYTESIZE = 4092;

    public async void StartReceiveAsync()
    {
        if (socket is null) throw new ArgumentNullException("StartReceiveAsync");

        await Task.Run(() => StartReceive());
    }

    private void StartReceive()
    {
        logger.Info("Сервер начал принимать сообщения");

        int size;
        byte[] buffer = new byte[MAXBYTESIZE];
        EndPoint clnt;
        StringBuilder req = new(string.Empty);
        Message msg;

        while(!isExit)
        {
            clnt = new IPEndPoint(IPAddress.Any, 8282);
            req.Clear();

            size = socket.ReceiveFrom(buffer, ref clnt);

            logger.Info(
                $"""
                Сервер получил сообщение.
                Информация о клиенте: 
                IP: { ((IPEndPoint)clnt).Address?.ToString() }
                """);

            req.Append(Encoding.UTF8.GetString(buffer, 0, size));

            WorkWithGettedMessageAsync(req.ToString(), clnt);
        }

        logger.Info("Сервер перестал принимать сообщения");
    }

    private async void WorkWithGettedMessageAsync(string json, EndPoint clnt)
    {
        await Task.Run(() => WorkWithGettedMessage(json, clnt));
    }

    private void WorkWithGettedMessage(string json, EndPoint clnt)
    {
        logger.MicroInfo($"Работа над сообщением от {((IPEndPoint)clnt).Address} начата");
        Message? msg;

        msg = MyJsonConverter.FromJson<Message>(json);

        SendResponceAsync(msg, clnt, msg is not null);

        if (msg is not null)
        {
            msg = DefineMessage(msg, json.ToString());

            semaphore.WaitOne();

            this.AddOnlineUserAsync(msg.IDUserFrom, clnt);
            messages.Add(msg);

            semaphore.Release();
        }
        logger.MicroInfo($"Работа над сообщением от {((IPEndPoint)clnt).Address} закончена");
    }

    private async void SendResponceAsync(Message msg, EndPoint endPoint, bool success)
    {
        if (msg is not null && msg.MessageType is (Message.TypeOfMessage.AnswerMessage or Message.TypeOfMessage.OnlineMessage)) return;

        await Task.Run(() =>
        {
            msg = new AnswerMessage(msg);
            ((AnswerMessage)msg).MessageStatus = success ? AnswerMessage.Status.Success : AnswerMessage.Status.Unsuccess;

            byte[] buffer = Encoding.UTF8.GetBytes(MyJsonConverter.ToJson(msg));
            socket.SendToAsync(buffer, endPoint);

            if (success)
            {
                logger.MicroInfo($"Успешно принятое сообщение от {((IPEndPoint)endPoint).Address}");
            }
            else
            {
                logger.Warning($"Неуспешно принятое сообщение от {((IPEndPoint)endPoint).Address}");
            }
        });
    }

    private static Message DefineMessage(Message msg, string json)
    {
        Message? answer = null;

        switch(msg.MessageType)
        {
            case Message.TypeOfMessage.ViewMessage: // Видимые сообщения
                {
                    var temp = MyJsonConverter.FromJson<ViewMessage>(json);

                    switch (temp?.ViewMessageType)
                    {
                        case ViewMessage.TypeOfViewMessage.TextMessage:
                            answer = MyJsonConverter.FromJson<TextMessage>(json);
                            break;
                        case ViewMessage.TypeOfViewMessage.VoiceMessage:
                            answer = MyJsonConverter.FromJson<VoiceMessage>(json);
                            break;
                    }
                }
                break;
            case Message.TypeOfMessage.AnswerMessage:
                answer = MyJsonConverter.FromJson<AnswerMessage>(json);
                break;
            case Message.TypeOfMessage.OnlineMessage:
                answer = MyJsonConverter.FromJson<OnlineMessage>(json);
                break;
        }

        return answer;
    }
}
