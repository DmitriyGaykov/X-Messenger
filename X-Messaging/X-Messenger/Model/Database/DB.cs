using Lib.Assets.Patterns;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio.CoreAudioApi;

namespace X_Messenger.Model.Database;

internal class DB : IDisposable
{
    #region Fields

    public static string StringConnection { get; set; } = string.Empty;

    private readonly SqlConnection connection = new(StringConnection);

    #endregion

    #region Ctors

    public DB()
    {
        while (StringConnection == string.Empty) ;

        byte count = 0;

        while (!Open())
        {
            if (count++ == 10)
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
        try
        {
            connection.Open();
        }
        catch
        {

        }
        return connection.State is System.Data.ConnectionState.Open;
    }

    public async Task<int> UpdateObjectAsync<T>(T obj) where T : IDBTable
    {
        return await Task.Run(() => UpdateObject(obj));
    }

    public int UpdateObject<T>(T obj) where T : IDBTable
    {
        bool isAll = false;
        do
        {
            try
            {
                var sqlCommand = obj.UpdateCommand;
                sqlCommand.Connection = connection;

                SqlParameter returnParam = sqlCommand.Parameters.Add("returnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                sqlCommand.ExecuteNonQuery();

                return (int)returnParam.Value;
            }
            catch
            {
                View.Modals.MessageBox.Show("Проблема в методе UpdateObject");
            }
        }
        while (!isAll);
        return 0;
    }

    public int InsertObject<T>(T obj) where T : IDBTable
    {
        bool isAll = false;

        do
        {
            try
            {
                var sqlCommand = obj.InsertCommand;
                sqlCommand.Connection = connection;

                SqlParameter returnParam = sqlCommand.Parameters.Add("returnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                sqlCommand.ExecuteNonQuery();

                return (int)returnParam.Value;
            }
            catch
            {
                View.Modals.MessageBox.Show("Проблема в методе InsertObject");
            }
        }
        while (!isAll);
        return 0;
    }

    public int EqualsObject<T>(T obj) where T : IDBTable
    {
        bool isAll = false;
        do
        {
            try
            {
                var sqlCommand = obj.EqualsCommand;
                sqlCommand.Connection = connection;

                SqlParameter returnParam = sqlCommand.Parameters.Add("returnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;

                sqlCommand.ExecuteNonQuery();

                return (int)returnParam.Value;
            }
            catch
            {
                View.Modals.MessageBox.Show("Проблема в методе EqualsObject");
            }
        } while (!isAll);
        return 0;
    }

    public async Task DeleteObjectAsync<T>(T obj = null) where T : class, IDBTable, new()
    {
        await Task.Run(() => DeleteObject(obj));
    }

    public void DeleteObject<T>(T obj = null) where T : class, IDBTable, new()
    {
        while (true)
        {
            try
            {
                obj ??= new();
                var sqlCommand = obj.DeleteCommand;
                sqlCommand.Connection = connection;
                sqlCommand.ExecuteNonQuery();
                return;
            }
            catch
            {
                View.Modals.MessageBox.Show("Проблема в методе DeleteObject");
            }
        }
    }

    public async Task<IList<T>> GetDatasAsync<T>(T el = null) where T : class, IDBTable, new() => await Task.Run(() => GetDatas<T>(el));

    public IList<T> GetDatas<T>(T el = null) where T : class, IDBTable, new()
    {
        while (true)
        {
            try
            {
                IList<T> list = new List<T>();

                el ??= new();

                var sqlCommand = el.RecvDatasCommand;
                sqlCommand.Connection = connection;
                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    list.Add((T)el.GetObjectFrom(reader));
                }

                return list;
            }
            catch
            {
                View.Modals.MessageBox.Show("Проблема в методе GetDatas");
            }
        }
        return new List<T>();
    }

    public void HaveFilled<T>(ref T el) where T : IDBTable, new()
    {
        var sqlCommand = el.RecvDatasCommand;
        sqlCommand.Connection = connection;
        var reader = sqlCommand.ExecuteReader();

        if(reader.Read())
        {
            el = (T)el.GetObjectFrom(reader);
        }
    }

    public async Task ExecuteCommandAsync(SqlCommand command)
    {
        command.Connection = connection;
        await command.ExecuteNonQueryAsync();
    }
    
    public SqlDataReader GetDataSetByQuery(string query)
    {
        SqlCommand command = new(query, connection);
        return command.ExecuteReader();
    }

    public SqlDataReader GetDataSetFrom<T>(T el = null) where T : class, IDBTable, new()
    {
        el ??= new();

        var sqlCommand = el.RecvDatasCommand;
        sqlCommand.Connection = connection;

        return sqlCommand.ExecuteReader();
    }

    public async Task<SqlDataReader> GetDataSetFromAsync<T>(T el = null) where T : class, IDBTable, new()
    {
        return await Task.Run(() =>  GetDataSetFrom<T>(el));
    }

    #endregion

}