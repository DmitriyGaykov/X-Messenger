namespace X_Messenger.Model.Assets.Messages.VoiceMessages;

internal abstract class VoiceMessage : Message
{
    private byte[] voice;

    public byte[] Voice
    {
        get => voice; 
        set => this.Buffer = voice = value;
    }

    public VoiceMessage(Message msg) : base(msg)
    {
        this.MessageType = TypeOfMessage.Voice;
        Voice = Buffer;
    }
}
