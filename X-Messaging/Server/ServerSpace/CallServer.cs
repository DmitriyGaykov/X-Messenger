using Lib.Converters;
using Lib.MessageNamespace.CallMessages;
using System.Net;
using System.Text;

namespace Server.ServerSpace;

public partial class Server
{
    private readonly Socket callSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private readonly Semaphore csem = new Semaphore(5, 10);
    private readonly int cport;
    private readonly EndPoint cEndPoint;

    private readonly ConcurrentBag<int> waitingUsers = new();

    private async void WorkWithCallMessageAsync(CallMessage message) => await Task.Run(() => WorkWithCallMessage(message));
    private void WorkWithCallMessage(CallMessage message)
    {
        if(message is StartCallMessage scmsg)
        {
            if(!IsInNetwork(scmsg.IDUserTo))
            {
                var agreeMsg = new AgreeMessage(scmsg);
                agreeMsg.Agree = false;

                SendToAsync(socket, agreeMsg, onlineUsers[scmsg.IDUserFrom]);
                return;
            }

            SendToAsync(socket, scmsg, onlineUsers[scmsg.IDUserTo]);
        }
    }

    /// <summary>
    /// Запуск сервера звонков
    /// </summary>
    public async void StartCallServerAsync()
    {
        logger.IInfo("Сервер звонков запущен");

        logger.IInfo(
            $"""
            Сервер звонков связан с параметрами:
            IP      : {ip}
            PORT    : {port}
            """
            );

        await Task.Run(() => StartCallServer());

        logger.IInfo("Сервер звонков прекратил работу");
    }

    private void StartCallServer()
    {
        byte[] buffer = new byte[MAXBYTESIZE];
        EndPoint clnt;
        int size;

        while(!isExit)
        {
            clnt = new IPEndPoint(IPAddress.Any, 0);
            size = callSocket.ReceiveFrom(buffer, ref clnt);

            SendCallAnswerAsync(buffer, clnt, size);
        }

        callSocket.Close();
    }

    public async void SendCallAnswerAsync(byte[] buffer, EndPoint clnt, int size)
    {
        Message msg;
        CallMessage cmsg;
        ByteCallMessage bcmsg;
        string text;

        await Task.Run(() =>
        {
            csem.WaitOne();
            text = Encoding.UTF8.GetString(buffer, 0, size);

            if (string.IsNullOrWhiteSpace(text))
            {
                csem.Release();
                return;
            }

            msg = MyJsonConverter.FromJson<Message>(text);
            msg = DefineMessage(msg, text);

            if (msg is null || msg.MessageType is not Message.TypeOfMessage.CallMessage)
            {
                csem.Release();
                return;
            }
            cmsg = (CallMessage)msg;

            if (cmsg.MessageCallMessageType is not CallMessage.TypeOfCallMessage.ByteCallMessage)
            {
                csem.Release();
                return;
            }
            bcmsg = (ByteCallMessage)cmsg;


            if (!IsInNetwork(bcmsg.IDUserTo))
            {
                SendToAsync(callSocket, new EndCallMessage(), clnt);
                logger.MicroWarning(
                    $"""
                    Человек с ID {bcmsg.IDUserTo} вышел во время звонка.
                    Звонок между пользователями #{bcmsg.IDUserFrom} и #{bcmsg.IDUserTo} завершен пренудительно.
                    """);
            }
            else
            {
                SendToAsync(callSocket, msg, onlineUsers[bcmsg.IDUserTo]);
            }
        });
    }
    private async void SendToAsync(Socket socket, Message msg, EndPoint clnt)
    {
        await Task.Run(() =>
        {
            string json = MyJsonConverter.ToJson(msg);
            var buffer = Encoding.UTF8.GetBytes(json);
            socket.SendToAsync(buffer, clnt);

            csem.Release();
        });
    }
}
