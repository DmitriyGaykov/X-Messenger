using Lib.Assets.Patterns;
using System;
using System.Data.SqlClient;
using System.Windows;
using X_Messenger.Model.Assets.Converters;
using X_Messenger.Model.Assets.Messages;

namespace X_Messenger.Model.Database.DBObjects;

internal class DBMessage : Message, IDBTable
{
    public string TableName => throw new NotImplementedException();
    public bool DelAll { get; set; } = true;

    public SqlCommand InsertCommand
    {
        get
        {
            SqlCommand command = new("sendMessage");
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@messageId", this.IdMessage);
            command.Parameters.AddWithValue("@idFrom", this.IdFrom);
            command.Parameters.AddWithValue("@idTo", this.IdTo);
            command.Parameters.AddWithValue("@buffer", this.Buffer);
            command.Parameters.AddWithValue("@type", this.MessageType switch
            {
                TypeOfMessage.Text => "TEXT",
                TypeOfMessage.Sticker => "STICKER",
                TypeOfMessage.Voice => "VOICE"
            });

            return command;
        }
    }

    public SqlCommand EqualsCommand => throw new NotImplementedException();

    public SqlCommand RecvDatasCommand => throw new NotImplementedException();

    public SqlCommand UpdateCommand
    {
        get
        {
            SqlCommand command = new("editMsg");
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", this.IdMessage);
            command.Parameters.AddWithValue("@buffer", this.Buffer);

            return command;
        }
    }

    public SqlCommand DeleteCommand
    {
        get
        {
            if (DelAll)
            {
                SqlCommand command = new("deleteAllMessages");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("idFrom", this.IdFrom);
                command.Parameters.AddWithValue("idTo", this.IdTo);
                return command;
            }
            else
            {
                SqlCommand command = new("delMsg");

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", this.IdMessage);

                return command;
            }
        }
    }

    public object GetObjectFrom(SqlDataReader set)
    {
        Message msg = new();

        msg.IdMessage = new(set.GetString(0));
        msg.IdFrom = set.GetInt32(1);
        msg.IdTo = set.GetInt32(2);
        msg.Buffer = set.GetSqlBinary(3).Value;

        string msgType = set.GetString(4);

        msg.MessageType = msgType switch
        {
            "STICKER" => Message.TypeOfMessage.Sticker,
            "VOICE" => Message.TypeOfMessage.Voice,
            "TEXT" => Message.TypeOfMessage.Text,
            _ => Message.TypeOfMessage.Text
        };

        msg.Date = set.GetDateTime(5);

        var image = set.GetValue(6) is DBNull ? null : set.GetSqlBinary(6).Value;

        if (image is null)
        {
            Application.Current.Dispatcher.Invoke(() => msg.Image = null);
        }
        else
        {
            Application.Current.Dispatcher.Invoke(() => msg.Image = ImageConverter.ToImageSource(image));
        }

        return msg;
    }

    public DBMessage()
    {

    }

    public DBMessage(Message msg)
    {
        this.IdMessage = msg.IdMessage;
        this.IdFrom = msg.IdFrom;
        this.IdTo = msg.IdTo;
        this.Buffer = msg.Buffer;
        this.Date = msg.Date;
        this.MessageType = msg.MessageType;
    }
}
