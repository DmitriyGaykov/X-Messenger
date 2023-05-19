using Lib.MessageNamespace;
using Lib.MessageNamespace.CallMessages;
using Lib.MessageNamespace.RequestMessages;
using Lib.MessageNamespace.ViewMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Converters;

public static class MessagesDefiner
{
    public static Message DefineMessage(Message msg, string json)
    {
        if (msg is null) return null;

        Message? answer = null;

        switch (msg.MessageType)
        {
            case Message.TypeOfMessage.ViewMessage: // Видимые сообщения
                answer = MyJsonConverter.FromJson<ViewMessage>(json);
                break;
            case Message.TypeOfMessage.AnswerMessage:
                answer = MyJsonConverter.FromJson<AnswerMessage>(json);
                break;
            case Message.TypeOfMessage.OnlineMessage:
                answer = MyJsonConverter.FromJson<OnlineMessage>(json);
                break;
            case Message.TypeOfMessage.CallMessage:
                {
                    var temp = MyJsonConverter.FromJson<CallMessage>(json);

                    switch (temp?.MessageCallMessageType)
                    {
                        case CallMessage.TypeOfCallMessage.StartCall:
                            answer = MyJsonConverter.FromJson<StartCallMessage>(json);
                            break;
                        case CallMessage.TypeOfCallMessage.ByteCallMessage:
                            answer = MyJsonConverter.FromJson<ByteCallMessage>(json);
                            break;
                        case CallMessage.TypeOfCallMessage.EndCallMessage:
                            answer = MyJsonConverter.FromJson<EndCallMessage>(json);
                            break;
                    }
                }
                break;
            case Message.TypeOfMessage.RequestMessage:
                {
                    var temp = MyJsonConverter.FromJson<RequestMessage>(json);
                    switch(temp?.RequestMessageType)
                    {
                        case RequestMessage.TypeOfRequestMessage.RegisterMessage:
                            answer = MyJsonConverter.FromJson<RegisterMessage>(json);
                            break;
                    }
                }
                break;
        }

        return answer;
      }
}
