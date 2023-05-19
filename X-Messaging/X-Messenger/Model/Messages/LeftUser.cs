using Lib.Assets.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Objects;

namespace X_Messenger.Model.Messages;

internal class LeftUser : User
{
    public string LastMessage { get; set; }
    public string TimeOfLastMessage { get; set; }
    public string MsgType { get; set; }
    public BitmapImage ImageSource { get; set; }
    public bool IsCurrent { get; set; } = false;

    public LeftUser(User user)
    {
        this.Name = user.Name;
        this.Id = user.Id;
    }
}
