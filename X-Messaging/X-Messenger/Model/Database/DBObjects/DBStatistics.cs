using Lib.Assets.Patterns;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Messenger.Model.Database.DBObjects;

internal class DBStatistics : Objects.Statistics, IDBTable
{
    public string TableName => throw new NotImplementedException();

    public SqlCommand InsertCommand => throw new NotImplementedException();

    public SqlCommand EqualsCommand => throw new NotImplementedException();

    public SqlCommand UpdateCommand => throw new NotImplementedException();

    public SqlCommand DeleteCommand => throw new NotImplementedException();

    public SqlCommand RecvDatasCommand
    {
        get
        {
            SqlCommand command = new("select * from getStats()");
            return command;
        }
    }

    public object GetObjectFrom(SqlDataReader set)
    {
        DBStatistics stats = new();

        stats.CountUsers = set.GetInt32(0);
        stats.CountAdmins = set.GetInt32(1);
        stats.CountNewUsersForDay = set.GetInt32(2);
        stats.CountNewUsersForWeek = set.GetInt32(3);
        stats.CountStickers = set.GetInt32(4);

        return stats;
    }
}
