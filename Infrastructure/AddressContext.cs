using ApplicationCore;
using ApplicationCore.ClientAddress;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class AddressContext :IAddressContext
    {
        private readonly IDBConnectionService _connService;

        public AddressContext(IDBConnectionService connService)  
        {
           _connService = connService;
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
