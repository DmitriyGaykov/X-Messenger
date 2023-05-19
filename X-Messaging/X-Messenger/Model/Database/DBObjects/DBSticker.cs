using Lib.Assets.Patterns;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using X_Messenger.Model.Assets.Converters;
using X_Messenger.Model.Messages;

namespace X_Messenger.Model.Database.DBObjects;

internal class DBSticker : Sticker, IDBTable
{
    public DBSticker(Sticker sticker)
    {
        this.Id = sticker.Id;
        this.Source = sticker.Source;
    }

    public DBSticker()
    {
        
    }

    public string TableName => throw new NotImplementedException();

    public SqlCommand InsertCommand
    {
        get
        {
            SqlCommand command = new("addSticker");
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@buffer", ImageConverter.ImageToBytes(this.Source));
            return command;
        }
    }

    public SqlCommand EqualsCommand => throw new NotImplementedException();

    public SqlCommand UpdateCommand => throw new NotImplementedException();

    public SqlCommand RecvDatasCommand
    {
        get
        {
            SqlCommand command = new("select * from getStickers()");
            return command;
        }
    }

    public SqlCommand DeleteCommand
    {
        get
        {
            SqlCommand command = new("removeSticker");

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", this.Id);

            return command;
        }
    }

    public object GetObjectFrom(SqlDataReader set)
    {
        Sticker sticker = new();

        sticker.Id = set.GetInt32(0);
        sticker.Source = ImageConverter.ToImageSource((byte[])set.GetValue(1));

        return sticker;
    }
}
