using System.Data.SqlClient;
namespace Infrastructure.Interfaces;


public interface IDBConnectionService
{
    public SqlConnection CreateSqlConnection();
    public void OpenConnection(SqlConnection connection);
    public void CloseConnection(SqlConnection connection);
}
