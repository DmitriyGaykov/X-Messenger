
namespace Lib.MessageNamespace.CallMessages;

public class ByteCallMessage : CallMessage
{
    public byte[] Buffer { get; set; }

    public ByteCallMessage() : base(TypeOfCallMessage.ByteCallMessage)
    {
        NeedResnd = false;
    }
    public ByteCallMessage(byte[] buffer) : this()
    {
        Buffer = buffer;
    }
}
