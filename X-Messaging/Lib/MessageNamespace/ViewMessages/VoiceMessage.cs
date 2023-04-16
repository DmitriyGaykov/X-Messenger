namespace Lib.MessageNamespace.ViewMessages;

public class VoiceMessage : ViewMessage
{
    public byte[] Voice { get; set; }
    public VoiceMessage() : base(TypeOfViewMessage.VoiceMessage)
    {

    }

    public VoiceMessage(byte[] voice) : this()
    {
        Voice = voice;
    }
}
