using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ServerSpace;

public partial class Server
{
    private readonly HashSet<Guid> answerMessages = new(); // ответы от клиентов
    private readonly HashSet<Guid> expectAnswer = new(); // сервер ожидает ответ

    private const byte RESENDAFTERTIME = 4;

    private readonly Dictionary<Guid, DateTime> resendAfter = new(); // переотправить сообщение через
    private readonly Dictionary<Guid, WaitMsg> reMessage = new(); // айди и сообщение, чтобы отправить заного

    public void StartResponseServerAsync()
    {
        logger.IInfo("Response Server начал работу");
        AnswerGetter();
        Resender();
    }

    private async void AnswerGetter()
    {
        await Task.Run(() =>
        {
            Guid id;
            while (!isExit)
            {
                if (answerMessages.Count() > 0)
                {
                    id = answerMessages.First();

                    answerMessages.Remove(id);
                    expectAnswer.Remove(id);
                    resendAfter.Remove(id);
                    reMessage.Remove(id);
                }
            }
        });
    }
    private async void Resender()
    {
        await Task.Run(() =>
        {
            while(!isExit)
            {
                foreach(var el in resendAfter)
                {
                    if((DateTime.Now - el.Value).Seconds >= RESENDAFTERTIME)
                    {
                        logger.MicroInfo($"ResponseServer | Сообщение {el.Key} отправлено заново");
                        SendAsync(reMessage[el.Key].Msg, reMessage[el.Key].To);
                    }
                }
            }
        });
        logger.IInfo("Response Server прекратил работу");
    }

    private void AddOnWaiting(Message msg, EndPoint to)
    {
        expectAnswer.Add(msg.Id);
        resendAfter.Add(msg.Id, DateTime.Now.AddSeconds(RESENDAFTERTIME));
        reMessage.Add(msg.Id, new(msg, to));
    }

    private record WaitMsg(Message Msg, EndPoint To);
}
