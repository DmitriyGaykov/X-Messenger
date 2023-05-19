using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Assets.Patterns;

public interface IDBTable
{
    string TableName { get; }
    public SqlCommand InsertCommand { get; }
    public SqlCommand EqualsCommand { get; }
    public SqlCommand UpdateCommand { get; }
    public SqlCommand DeleteCommand { get; }
    public SqlCommand RecvDatasCommand { get; }
    public object GetObjectFrom(SqlDataReader set) => new object();
}
