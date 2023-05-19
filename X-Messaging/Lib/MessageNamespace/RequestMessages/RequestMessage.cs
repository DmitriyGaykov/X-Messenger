namespace Lib.MessageNamespace.RequestMessages;

public class RequestMessage : Message
{
    public TypeOfRequestMessage RequestMessageType { get; set; }

    public RequestMessage(TypeOfRequestMessage requestMessageType) : base(TypeOfMessage.RequestMessage)
    {
        RequestMessageType = requestMessageType;
    }

    public enum TypeOfRequestMessage
    {
        RegisterMessage
    }
}
