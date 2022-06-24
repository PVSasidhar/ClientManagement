using System.Data.SqlClient;
namespace Infrastructure;

public class DBConnection
{
    protected string _connectionString { get; }
    public DBConnection(string connectionString)
    {
        _connectionString = connectionString;
    }
    public SqlConnection CreateSqlConnection()
    {
        return new SqlConnection(_connectionString);
    }
    protected void OpenConnection(SqlConnection connection)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }
    }
    protected void CloseConnection(SqlConnection connection)
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }

}
