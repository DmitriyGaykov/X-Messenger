using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.MessageNamespace.CallMessages;
public class CallMessage : Message
{
    public TypeOfCallMessage MessageCallMessageType { get; set; }

    public CallMessage(TypeOfCallMessage messageCallMessageType) : base(TypeOfMessage.CallMessage)
    {
        MessageCallMessageType = messageCallMessageType;
    }

    public CallMessage(Message msg) : base(msg)
    {

    }

    public enum TypeOfCallMessage
    {
        StartCall,
        ByteCallMessage,
        EndCallMessage,
        AgreeMessage
    }
}
