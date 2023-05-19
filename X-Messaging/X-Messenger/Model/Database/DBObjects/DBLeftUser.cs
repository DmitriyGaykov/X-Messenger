using Lib.Assets.Patterns;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Assets.Converters;
using X_Messenger.Model.Messages;
using X_Messenger.Model.Objects;

namespace X_Messenger.Model.Database.DBObjects;

internal class DBLeftUser : LeftUser, IDBTable
{
    public DBLeftUser(User user) : base(user)
    {
    }

    public DBLeftUser() : this(new User())
    {

    }

    public string TableName => throw new NotImplementedException();

    public SqlCommand InsertCommand => throw new NotImplementedException();

    public SqlCommand EqualsCommand => throw new NotImplementedException();

    public SqlCommand RecvDatasCommand
    {
        get
        {
            string query = $"select * from getPeople({this.Id})";
            SqlCommand command = new(query);
            return command;
        }
    }

    public SqlCommand UpdateCommand => throw new NotImplementedException();

    public SqlCommand DeleteCommand => throw new NotImplementedException();

    public object GetObjectFrom(SqlDataReader set)
    {
        DBLeftUser user = new();

        user.Id = set.GetInt32(0);
        user.Name = set.GetString(1);
        var buffer = set.GetValue(2);

        if(buffer is DBNull)
        {
            BitmapImage src = new(new("../../../View/Assets/Images/DefaultUser.png", UriKind.RelativeOrAbsolute));
            Application.Current.Dispatcher.Invoke(() => user.ImageSource = src);
        }
        else
        {
            Application.Current.Dispatcher.Invoke(() =>  user.ImageSource = Assets.Converters.ImageConverter.ToImageSource(buffer as byte[]));
        }

        var glastmsg = set.GetValue(3);
        user.MsgType = set.GetValue(4) is DBNull ? null : set.GetString(4);

        if (glastmsg is DBNull)
        {
            user.LastMessage = "Start chating...";
        }
        else
        {
            if(user.MsgType is "TEXT")
            {
                user.LastMessage = Encoding.UTF8.GetString((byte[])glastmsg);
                if(user.LastMessage.Length > 16)
                {
                    user.LastMessage = $"{user.LastMessage.Substring(0, 16)}...";
                }
            }
            else if(user.MsgType is "VOICE")
            {
                user.LastMessage = "Voice message";
            }
            else
            {
                user.LastMessage = "Sticker ☺";
            }
        }

        var gdate = set.GetValue(5);

        try 
        {
            var date = (DateTime)gdate;

            if (date.Date != DateTime.Now.Date)
            {
                user.TimeOfLastMessage = "недавно";
            }
            else
                user.TimeOfLastMessage = $"{(date.Hour < 10 ? ("0" + date.Hour) : date.Hour)}:{(date.Minute < 10 ? ("0" + date.Minute) : date.Minute)}";
        }
        catch
        {
            user.TimeOfLastMessage = "00:00";
        }

        return user;
    }
}
