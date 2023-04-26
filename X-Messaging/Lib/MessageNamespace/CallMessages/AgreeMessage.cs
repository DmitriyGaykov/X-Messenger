namespace Lib.MessageNamespace.CallMessages;
public class AgreeMessage : CallMessage
{
    public bool Agree { get; set; } = false;

    public AgreeMessage() : base(TypeOfCallMessage.AgreeMessage)
    {
    }

    public AgreeMessage(bool agree) : this()
    {
        Agree = agree;
    }

    public AgreeMessage(Message msg) : base(msg)
    {
        if(msg is AgreeMessage am)
        {
            this.Agree = am.Agree;
        }
    }
}
