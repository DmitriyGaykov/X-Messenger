using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.MessageNamespace.CallMessages;

public class StartCallMessage : CallMessage
{
    public StartCallMessage() : base(TypeOfCallMessage.StartCall)
    {

    }
}
