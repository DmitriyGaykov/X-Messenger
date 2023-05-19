using Lib.Assets.Patterns;
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows;
using System.Windows.Media.Imaging;
using X_Messenger.Model.Assets.Converters;
using X_Messenger.Model.Objects;

namespace X_Messenger.Model.Database.DBObjects;

internal class DBUser : X_Messenger.Model.Objects.User, IDBTable
{
    public string TableName => "Users";

    public DBUser() : base()
    {

    }

    public DBUser(User user)
    {
        this.Id = user.Id;
        this.name = user.Name;
        this.password = user.Password;
        this.login = user.Login;
        Application.Current.Dispatcher.Invoke(() => this.Source = user.Source);
        this.Description = user.Description;
    }

    public SqlCommand InsertCommand
    {
        get
        {
            SqlCommand command = new("Registration");

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@NICKNAME", name);
            command.Parameters.AddWithValue("@LOGIN", login);
            command.Parameters.AddWithValue("@PASS", password);

            return command;
        }
    }

    public SqlCommand EqualsCommand
    {
        get
        {
            SqlCommand command = new("Authorization");

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@LOGIN", login);
            command.Parameters.AddWithValue("@PASS", password);

            return command;
        }
    }

    public SqlCommand RecvDatasCommand => new($"select * from getUserById({id})");

    public SqlCommand UpdateCommand
    {
        get
        {
            SqlCommand command = new("editUser");
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@login", login ?? null);
            command.Parameters.AddWithValue("@pass", password ?? null);
            command.Parameters.AddWithValue("@image", image is null ? null : ImageConverter.ImageToBytes(image));
            command.Parameters.AddWithValue("@descr", descr ?? null);

            return command;
        }
    }

    public SqlCommand DeleteCommand
    {
        get
        {
            SqlCommand command = new("removeAccount");
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@userId", this.Id);

            return command;
        }
    }

    public object GetObjectFrom(SqlDataReader set)
    {
        DBUser user = new();

        user.Id = set.GetInt32(0);
        user.Name = set.GetString(1);

        var gimage = set.GetValue(2);

        try
        {
            Application.Current.Dispatcher.Invoke(() => user.Source = Assets.Converters.ImageConverter.ToImageSource(gimage as byte[]));

        }
        catch
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                BitmapImage image = new(new("../../../View/Assets/Images/DefaultUser.png", System.UriKind.Relative));
                user.Source = image;
            });
        }

        var gdescr = set.GetValue(3);

        if (gdescr is DBNull)
        {
            user.Description = null;
        }
        else
        {
            user.Description = (string)gdescr;
        }

        var glastVisit = set.GetValue(4);

        if(glastVisit is DBNull)
        {
            user.LastVisit = null; 
        }
        else
        {
            user.LastVisit = (DateTime)glastVisit;
        }

        user.AdminLvl = set.GetInt32(5);

        return user;
    }
}
