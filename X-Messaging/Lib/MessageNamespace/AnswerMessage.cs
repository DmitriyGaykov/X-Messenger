using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.MessageNamespace;

public class AnswerMessage : Message
{
    public Status MessageStatus { get; set; } = Status.Default;

    public AnswerMessage() : base(TypeOfMessage.AnswerMessage)
    {
        NeedResnd = false;
    }

    public AnswerMessage(Message msg) : base(msg)
    {
        this.MessageType = TypeOfMessage.AnswerMessage;
        NeedResnd = false;
    }

    public AnswerMessage(Status messageStatus) : this()
    {
        MessageStatus = messageStatus;
    }

    public enum Status
    {
        Success,
        Unsuccess,
        Default
    }
}
