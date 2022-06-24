using ApplicationCore;
using ApplicationCore.ClientAddress;

namespace Infrastructure
{
    public class AddressContext : DBConnection, IAddressContext
    {
        public AddressContext(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Address> GetByIds(long[] Ids)
        {
            throw new NotImplementedException();
        }

        Address IDataContract<Address>.Create(Address _object)
        {
            throw new NotImplementedException();
        }

        void IDataContract<Address>.Delete(Address _object)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Address> IDataContract<Address>.GetAll()
        {
            throw new NotImplementedException();
        }

        Address IDataContract<Address>.GetById(long Id)
        {
            throw new NotImplementedException();
        }

        void IDataContract<Address>.Update(Address _object)
        {
            throw new NotImplementedException();
        }
    }
}
