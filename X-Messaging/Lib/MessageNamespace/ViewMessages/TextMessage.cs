using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.MessageNamespace.ViewMessages;

public class TextMessage : ViewMessage
{
    public string Text { get; set; }
    public TextMessage() : base(TypeOfViewMessage.TextMessage)
    {
    }

    public TextMessage(string text) : this()
    {
        this.Text = text;
    }
}
