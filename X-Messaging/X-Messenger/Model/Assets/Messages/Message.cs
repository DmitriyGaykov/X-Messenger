using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace X_Messenger.Model.Assets.Messages;

internal class Message
{
    public Guid IdMessage { get; set; }
    public int IdFrom { get; set; }
    public int IdTo { get; set; }
    public byte[] Buffer { get; set; }
    public TypeOfMessage MessageType { get; set; }
    public DateTime Date { get; set; }
    public BitmapSource Image { get; set; }

    public bool ShowEDMenu { get; set; } = false;

    public Message()
    {
        IdMessage = Guid.NewGuid();
    }

    public Message(Message msg) : this()
    {
        this.IdMessage = msg.IdMessage;
        this.IdFrom = msg.IdFrom;
        this.IdTo = msg.IdTo;
        this.Buffer = msg.Buffer;
        this.Date = msg.Date;
        this.MessageType = msg.MessageType;
        this.Image = msg.Image;
    }

    public enum TypeOfMessage
    {
        Text,
        Voice,
        Sticker
    }
}
