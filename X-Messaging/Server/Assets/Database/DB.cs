using Lib.Assets.Patterns;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Server.Assets.Database;

internal class DB : IDisposable
{
    #region Fields

    private const string stringConnection = "Data Source=DIMA;Initial Catalog=X_Messenger;Integrated Security=True";

    private readonly SqlConnection connection = new(stringConnection);

    #endregion

    #region Ctors

    public DB()
    {
        byte count = 0;

        while(!Open())
        {
            if(count++ == 10)
            {
                throw new Exception("Не удалось подключится к БД");
            }
        }
    }

    #endregion

    #region Props

    #endregion

    #region Methods

    public void Dispose() => connection.Dispose();

    public bool Open()
    {
        connection.Open();
        return connection.State is System.Data.ConnectionState.Open;
    }

    public bool InsertObject<T>(T obj) where T : IDBTable
    {
        var sqlCommand = obj.InsertCommand;
        sqlCommand.Connection = connection;

        int res = sqlCommand.ExecuteNonQuery();

        return res > 0;
    }

    #endregion

}
