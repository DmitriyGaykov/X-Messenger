using Lib.Assets.Objects;
using Lib.Assets.Patterns;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Assets.Database.DBObjects;

internal class DBUser : IDBTable
{
    public string TableName => "Users";

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

    public SqlCommand EqualsCommand => throw new NotImplementedException();

    public DBUser(User user)
    {
        this.name = user.Name;
        this.login = user.Login;
        this.password = user.Password;
    }
}
