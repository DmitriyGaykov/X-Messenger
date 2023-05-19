using Lib.Assets.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.MessageNamespace.RequestMessages;

public class RegisterMessage : RequestMessage
{
    public User User { get; set; }
    public bool Success { get; private set; }

    public RegisterMessage() : base(TypeOfRequestMessage.RegisterMessage) 
    {
        
    }

    public RegisterMessage(User user) : this()
    {
        User = user;
    }

    public RegisterMessage(bool success) : this()
    {
        Success = success;
    }

    public RegisterMessage(User user, bool success) : this(user)
    {
        Success = success;
    }   
}
