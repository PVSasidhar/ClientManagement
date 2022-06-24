using ApplicationCore.Core;
using System.Data;
using System.Text;

namespace Infrastructure
{
    public class FileExporter : DBConnection, IFileExporter
    {
        public FileExporter(string connectionString) : base(connectionString)
        {
        }
        public async Task<byte[]> GetCsv()
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            using (var sqlConnnection = CreateSqlConnection())
            {
                OpenConnection(sqlConnnection);

                var seperator = ",";
                var command = sqlConnnection.CreateCommand();

                command.CommandText = $"SELECT * FROM [dbo].[v_client_address_download_details]";
                command.CommandType = CommandType.Text;

                var data = string.Join(seperator, "Title", "First Name", "Last Name", "Gender", "Residential Address Line1", "Residential Address Line2", "Residential Email", "Residential City", "Work Address Line1", "Work Address Line2", "Work Email", "Work City", "Postal Address Line1", "Postal Address Line2", "Postal Email", "Postal City");
                await writer.WriteLineAsync(data);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data = string.Join(seperator,
                                            (string)reader["Title"],
                                            (string)reader["FirstName"],
                                            (string)reader["LastName"],
                                            (string)reader["Gender"],
                                            (string)reader["ResidentialAddressLine1"],
                                            (string)reader["ResidentialAddressLine2"],
                                            (string)reader["ResidentialEmail"],
                                            (string)reader["ResidentialCity"],
                                            (string)reader["WorkAddressLine1"],
                                            (string)reader["WorkAddressLine2"],
                                            (string)reader["WorkEmail"],
                                            (string)reader["WorkCity"],
                                            (string)reader["PostalAddressLine1"],
                                            (string)reader["PostalAddressLine2"],
                                            (string)reader["PostalEmail"],
                                            (string)reader["PostalCity"]
                                            );

                        await writer.WriteLineAsync(data);
                    }

                    await writer.FlushAsync();
                }

            }
            return stream.ToArray();
        }
    }
}
