using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Messenger.Model.Assets.Messages.TextMessages;

internal abstract class TextMessage : Message
{
    private string text;

    public string Text 
    { 
        get => text;
        set
        {
            text = value;
            this.Buffer = Encoding.UTF8.GetBytes(text);
        }
    }

    public TextMessage(Message msg) : base(msg)
    {
        this.Text = Encoding.UTF8.GetString(Buffer);
    }
}
