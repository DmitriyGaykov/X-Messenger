using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.MessageNamespace.ViewMessages;

public class ViewMessage : Message
{
    public TypeOfViewMessage ViewMessageType { get; private init; }

    public ViewMessage(TypeOfViewMessage type) : base(Message.TypeOfMessage.ViewMessage)
    {
        this.ViewMessageType = type;
    }

    public enum TypeOfViewMessage
    {
        TextMessage,
        VoiceMessage
    }
}
