using ApplicationCore.ClientAddress;
using ApplicationCore.Clients;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    public class ClientContext : DBConnection, IClientContext
    {
        public ClientContext(string connectionString) : base(connectionString)
        {

        }
        public Client Create(Client client)
        {
            using (var sqlConnnection = CreateSqlConnection())
            {
                OpenConnection(sqlConnnection);
                var command = sqlConnnection.CreateCommand();

                SqlParameter ClientId = new SqlParameter("@ClientId", client.Id);
                SqlParameter Title = new SqlParameter("@Title", client.Title);
                SqlParameter FirstName = new SqlParameter("@FirstName", client.FirstName);
                SqlParameter MiddleName = new SqlParameter("@MiddleName", client.MiddleName);
                SqlParameter LastName = new SqlParameter("@LastName", client.LastName);
                SqlParameter Suffix = new SqlParameter("@Suffix", "");
                SqlParameter Gender = new SqlParameter("@Gender", client.Gender);
                SqlParameter ModifiedDate = new SqlParameter("@ModifiedDate", DateTime.UtcNow);

                command.Parameters.Add(ClientId);
                command.Parameters.Add(Title);
                command.Parameters.Add(FirstName);
                command.Parameters.Add(MiddleName);
                command.Parameters.Add(LastName);
                command.Parameters.Add(Suffix);
                command.Parameters.Add(Gender);
                command.Parameters.Add(ModifiedDate);
                command.CommandText = @"INSERT INTO [dbo].[v_clients]
                                               ([Title] 
                                               ,[FirstName] 
                                               ,[MiddleName] 
                                               ,[LastName] 
                                               ,[Suffix] 
                                               ,[Gender] 
                                               ,[ModifiedDate])
                                        VALUES
                                               (@Title
                                               ,@FirstName
                                               ,@MiddleName
                                               ,@LastName
                                               ,@Suffix
                                               ,@Gender
                                               ,@ModifiedDate)";
                command.CommandType = CommandType.Text;
                using (var trans = sqlConnnection.BeginTransaction())
                {
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                    var getIDcommand = sqlConnnection.CreateCommand();
                    getIDcommand.Connection = sqlConnnection;
                    getIDcommand.CommandText = "SELECT MAX(Id) AS Id FROM [dbo].[v_clients]";
                    getIDcommand.Transaction = trans;
                    using (var reader = getIDcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            client.Id = (long)reader["Id"];
                        }
                    }
                    InsertAddress(client.Id, client.ClientAddresses, sqlConnnection, trans);
                    trans.Commit();
                }
            }
            return client;
        }
        public void Delete(Client client)
        {
            using (var sqlConnnection = CreateSqlConnection())
            {
                OpenConnection(sqlConnnection);

                var clientAddressCommand = sqlConnnection.CreateCommand();

                SqlParameter ClientId = new SqlParameter("@ClientId", client.Id);
                clientAddressCommand.CommandText = $"DELETE FROM [dbo].[v_client_address] WHERE ClientId = @ClientId";
                clientAddressCommand.CommandType = CommandType.Text;
                clientAddressCommand.Parameters.Add(ClientId);

                using (var trans = sqlConnnection.BeginTransaction())
                {
                    clientAddressCommand.Transaction = trans;
                    var rows = clientAddressCommand.ExecuteNonQuery();

                    foreach (var address in client.ClientAddresses)
                    {
                        var addressCommand = sqlConnnection.CreateCommand();

                        SqlParameter AddressId = new SqlParameter("@AddressId", address.Id);
                        addressCommand.CommandText = $"DELETE FROM [dbo].[v_address] WHERE Id = @AddressId";
                        addressCommand.CommandType = CommandType.Text;
                        addressCommand.Parameters.Add(AddressId);
                        addressCommand.Transaction = trans;
                        rows = addressCommand.ExecuteNonQuery();
                    }

                    var clientCommand = sqlConnnection.CreateCommand();

                    SqlParameter Id = new SqlParameter("@ClientId", client.Id);
                    clientCommand.CommandText = $"DELETE FROM [dbo].[v_clients] WHERE Id = @ClientId";
                    clientCommand.CommandType = CommandType.Text;
                    clientCommand.Parameters.Add(Id);
                    clientCommand.Transaction = trans;
                    rows = clientCommand.ExecuteNonQuery();
                    trans.Commit();
                }
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAll()
        {
            var result = new List<Client>();
            using (var sqlConnnection = CreateSqlConnection())
            {
                OpenConnection(sqlConnnection);

                var command = sqlConnnection.CreateCommand();

                command.CommandText = $"SELECT * FROM  [dbo].[v_clients]";
                command.CommandType = CommandType.Text;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var client = new Client
                        {
                            Id = (long)reader["id"],
                            Title = (string)reader["Title"],
                            FirstName = (string)reader["FirstName"],
                            MiddleName = (string)reader["MiddleName"],
                            LastName = (string)reader["LastName"],
                            Suffix = (string)reader["Suffix"],
                            Gender = (byte)reader["Gender"],
                            ModifiedDate = (DateTime)reader["ModifiedDate"]
                        };
                        result.Add(client);
                    }
                }
            }
            return result;
        }

        public Client GetById(long Id)
        {
            var result = new Client();
            if (Id > 0)
            {
                using (var sqlConnnection = CreateSqlConnection())
                {
                    OpenConnection(sqlConnnection);
                    var command = sqlConnnection.CreateCommand();
                    SqlParameter IdParam = new SqlParameter("@Id", Id);
                    command.Parameters.Add(IdParam);
                    command.CommandText = $"SELECT * FROM  [dbo].[v_clients] WHERE Id = @Id";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new Client { Id = (long)reader["Id"], Title = (string)reader["Title"], FirstName = (string)reader["FirstName"], MiddleName = (string)reader["MiddleName"], LastName = (string)reader["LastName"], Suffix = (string)reader["Suffix"], Gender = (byte)reader["Gender"], ModifiedDate = (DateTime)reader["ModifiedDate"] };
                        }
                    }

                }
            }
            result.ClientAddresses = GetClientAddress(Id);
            return result;
        }
        public void Update(Client client)
        {
            using (var sqlConnnection = CreateSqlConnection())
            {
                OpenConnection(sqlConnnection);

                var command = sqlConnnection.CreateCommand();

                SqlParameter ClientId = new SqlParameter("@ClientId", client.Id);
                SqlParameter Title = new SqlParameter("@Title", client.Title);
                SqlParameter FirstName = new SqlParameter("@FirstName", client.FirstName);
                SqlParameter MiddleName = new SqlParameter("@MiddleName", client.MiddleName);
                SqlParameter LastName = new SqlParameter("@LastName", client.LastName);
                SqlParameter Suffix = new SqlParameter("@Suffix", client.Suffix);
                SqlParameter Gender = new SqlParameter("@Gender", client.Gender);
                SqlParameter ModifiedDate = new SqlParameter("@ModifiedDate", DateTime.UtcNow);

                command.Parameters.Add(ClientId);
                command.Parameters.Add(Title);
                command.Parameters.Add(FirstName);
                command.Parameters.Add(MiddleName);
                command.Parameters.Add(LastName);
                command.Parameters.Add(Suffix);
                command.Parameters.Add(Gender);
                command.Parameters.Add(ModifiedDate);
                command.CommandText = @"UPDATE [dbo].[v_clients]
                                            SET [Title] = @Title
                                               ,[FirstName] = @FirstName
                                               ,[MiddleName] = @MiddleName
                                               ,[LastName] = @LastName
                                               ,[Suffix] = @Suffix
                                               ,[Gender] = @Gender
                                               ,[ModifiedDate] = @ModifiedDate
                                                WHERE Id = @ClientId";
                command.CommandType = CommandType.Text;
                using (var trans = sqlConnnection.BeginTransaction())
                {
                    command.Transaction = trans;
                    var rows = command.ExecuteNonQuery();
                    UpdateAddress(client.ClientAddresses, sqlConnnection, trans);
                    trans.Commit();
                }

            }
        }
        private IList<Address> GetClientAddress(long ClientId)
        {
            var result = new List<Address>();

            if (ClientId > 0)
            {
                using (var sqlConnnection = CreateSqlConnection())
                {
                    OpenConnection(sqlConnnection);
                    var command = sqlConnnection.CreateCommand();
                    SqlParameter IdParam = new SqlParameter("@Id", ClientId);
                    command.Parameters.Add(IdParam);
                    command.CommandText = $"SELECT * FROM [dbo].[v_client_address_details] WHERE ClientId = @Id";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var address = new Address
                            {
                                Id = (long)reader["AddressId"],
                                AddressLine1 = (string)reader["AddressLine1"],
                                AddressLine2 = (string)reader["AddressLine2"],
                                CellPhoneNumber = (string)reader["CellPhoneNumber"],
                                ResidentialPhoneNumber = (string)reader["ResidentialPhoneNumber"],
                                BusinessPhoneNumber = (string)reader["BusinessPhoneNumber"],
                                Email = (string)reader["Email"],
                                City = (string)reader["City"],
                                StateProvince = (int)reader["StateProvince"],
                                AddressTypeId = (int)reader["AddressTypeId"],
                                PostalCode = (string)reader["PostalCode"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"],
                                ClientId = (long)reader["ClientId"]
                            };
                            result.Add(address);
                        }
                    }
                }
            }

            if (result.Count == 0)
            {
                for (int i = 1; i <= 3; i++)
                {
                    var address = new Address
                    {
                        Id = 0,
                        AddressLine1 = "",
                        AddressLine2 = "",
                        CellPhoneNumber = "",
                        ResidentialPhoneNumber = "",
                        BusinessPhoneNumber = "",
                        Email = "",
                        City = "",
                        StateProvince = 0,
                        AddressTypeId = i,
                        PostalCode = "",
                        ModifiedDate = DateTime.UtcNow,
                        ClientId = ClientId
                    };
                    result.Add(address);
                }
            }

            return result;
        }
        private void UpdateAddress(IList<Address> ClientAddress, SqlConnection sqlConnnection, SqlTransaction transaction)
        {
            foreach (var address in ClientAddress)
            {
                var command = sqlConnnection.CreateCommand();
                SqlParameter Id = new SqlParameter("@Id", address.Id);
                SqlParameter AddressLine1 = new SqlParameter("@AddressLine1", address.AddressLine1);
                SqlParameter AddressLine2 = new SqlParameter("@AddressLine2", address.AddressLine2);
                SqlParameter CellPhoneNumber = new SqlParameter("@CellPhoneNumber", address.CellPhoneNumber);
                SqlParameter ResidentialPhoneNumber = new SqlParameter("@ResidentialPhoneNumber", address.ResidentialPhoneNumber);
                SqlParameter BusinessPhoneNumber = new SqlParameter("@BusinessPhoneNumber", address.BusinessPhoneNumber);
                SqlParameter Email = new SqlParameter("@Email", address.Email);
                SqlParameter City = new SqlParameter("@City", address.City);
                SqlParameter StateProvince = new SqlParameter("@StateProvince", address.StateProvince);
                SqlParameter AddressTypeId = new SqlParameter("@AddressTypeId", address.AddressTypeId);
                SqlParameter PostalCode = new SqlParameter("@PostalCode", address.PostalCode);
                SqlParameter ModifiedDate = new SqlParameter("@ModifiedDate", DateTime.UtcNow);

                command.Parameters.Add(Id);
                command.Parameters.Add(AddressLine1);
                command.Parameters.Add(AddressLine2);
                command.Parameters.Add(CellPhoneNumber);
                command.Parameters.Add(ResidentialPhoneNumber);
                command.Parameters.Add(BusinessPhoneNumber);
                command.Parameters.Add(Email);
                command.Parameters.Add(City);
                command.Parameters.Add(StateProvince);
                command.Parameters.Add(AddressTypeId);
                command.Parameters.Add(PostalCode);
                command.Parameters.Add(ModifiedDate);

                command.CommandText = @"UPDATE [dbo].[v_address]
                                       SET [AddressLine1] = @AddressLine1
                                          ,[AddressLine2] = @AddressLine2
                                          ,[CellPhoneNumber] = @CellPhoneNumber 
                                          ,[ResidentialPhoneNumber] = @ResidentialPhoneNumber 
                                          ,[BusinessPhoneNumber] = @BusinessPhoneNumber  
                                          ,[Email] = @Email 
                                          ,[City] = @City 
                                          ,[StateProvince] = @StateProvince 
                                          ,[AddressTypeId] = @AddressTypeId 
                                          ,[PostalCode] = @PostalCode 
                                          ,[ModifiedDate] = @ModifiedDate 
                                     WHERE Id = @Id";

                command.CommandType = CommandType.Text;
                command.Transaction = transaction;
                var rows = command.ExecuteNonQuery();
            }
        }
        private void InsertAddress(long clientId, IList<Address> ClientAddress, SqlConnection sqlConnnection, SqlTransaction transaction)
        {
            foreach (var address in ClientAddress)
            {
                var command = sqlConnnection.CreateCommand();

                SqlParameter Id = new SqlParameter("@Id", address.Id);
                SqlParameter AddressLine1 = new SqlParameter("@AddressLine1", address.AddressLine1);
                SqlParameter AddressLine2 = new SqlParameter("@AddressLine2", address.AddressLine2);
                SqlParameter CellPhoneNumber = new SqlParameter("@CellPhoneNumber", address.CellPhoneNumber);
                SqlParameter ResidentialPhoneNumber = new SqlParameter("@ResidentialPhoneNumber", address.ResidentialPhoneNumber);
                SqlParameter BusinessPhoneNumber = new SqlParameter("@BusinessPhoneNumber", address.BusinessPhoneNumber);
                SqlParameter Email = new SqlParameter("@Email", address.Email);
                SqlParameter City = new SqlParameter("@City", address.City);
                SqlParameter StateProvince = new SqlParameter("@StateProvince", address.StateProvince);
                SqlParameter AddressTypeId = new SqlParameter("@AddressTypeId", address.AddressTypeId);
                SqlParameter PostalCode = new SqlParameter("@PostalCode", address.PostalCode);
                SqlParameter ModifiedDate = new SqlParameter("@ModifiedDate", DateTime.UtcNow);

                command.Parameters.Add(Id);
                command.Parameters.Add(AddressLine1);
                command.Parameters.Add(AddressLine2);
                command.Parameters.Add(CellPhoneNumber);
                command.Parameters.Add(ResidentialPhoneNumber);
                command.Parameters.Add(BusinessPhoneNumber);
                command.Parameters.Add(Email);
                command.Parameters.Add(City);
                command.Parameters.Add(StateProvince);
                command.Parameters.Add(AddressTypeId);
                command.Parameters.Add(PostalCode);
                command.Parameters.Add(ModifiedDate);

                command.CommandText = @"INSERT INTO [dbo].[v_address]
                                           ([AddressLine1] 
                                          ,[AddressLine2]
                                          ,[CellPhoneNumber] 
                                          ,[ResidentialPhoneNumber]  
                                          ,[BusinessPhoneNumber]
                                          ,[Email]
                                          ,[City]
                                          ,[StateProvince] 
                                          ,[AddressTypeId]
                                          ,[PostalCode] 
                                          ,[ModifiedDate])
                                    VALUES
                                        (@AddressLine1
                                        ,@AddressLine2
                                        ,@CellPhoneNumber
                                        ,@ResidentialPhoneNumber
                                        ,@BusinessPhoneNumber
                                        ,@Email
                                        ,@City
                                        ,@StateProvince
                                        ,@AddressTypeId
                                        ,@PostalCode
                                        ,@ModifiedDate)";
                command.CommandType = CommandType.Text;
                command.Transaction = transaction;
                var rows = command.ExecuteNonQuery();

                var getIDcommand = sqlConnnection.CreateCommand();

                getIDcommand.Connection = sqlConnnection;
                getIDcommand.CommandText = "SELECT MAX(Id) AS Id FROM [dbo].[v_address]";
                getIDcommand.Transaction = transaction;
                using (var reader = getIDcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        address.Id = (long)reader["Id"];
                    }
                }

                var linkTablecommand = sqlConnnection.CreateCommand();

                SqlParameter AddressId = new SqlParameter("@AddressId", address.Id);
                SqlParameter ClientId = new SqlParameter("@ClientId", clientId);
                ModifiedDate = new SqlParameter("@ModifiedDate", DateTime.UtcNow);
                linkTablecommand.Parameters.Add(AddressId);
                linkTablecommand.Parameters.Add(ClientId);
                linkTablecommand.Parameters.Add(ModifiedDate);
                linkTablecommand.Connection = sqlConnnection;
                linkTablecommand.CommandText = @"INSERT INTO [dbo].[v_client_address] (ClientId, AddressId, ModifiedDate)
                                                 VALUES
                                                 (@ClientId, @AddressId , @ModifiedDate)";
                linkTablecommand.Transaction = transaction;
                rows = linkTablecommand.ExecuteNonQuery();
            }
        }
    }
}
