using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Messenger.Model.Assets.Messages.TextMessages;

internal class FriendTextMessage : TextMessage
{
    public FriendTextMessage(Message msg) : base(msg)
    {
    }
}
