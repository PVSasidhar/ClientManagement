using Infrastructure.Interfaces;
using System.Data.SqlClient;
namespace Infrastructure;

public class DBConnectionService: IDBConnectionService, IDisposable 
{
    private readonly string _connectionString ;
     
    public DBConnectionService(string connectionString) 
    {
        _connectionString = connectionString;
    }
    public SqlConnection CreateSqlConnection()
    {
        return new SqlConnection(_connectionString);
    }
    public void OpenConnection(SqlConnection connection)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }
    }
    public void CloseConnection(SqlConnection connection)
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public void Dispose()
    {
    }
}
