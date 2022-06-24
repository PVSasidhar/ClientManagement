namespace ApplicationCore.ClientAddress;
public interface IAddressContext : IDataContract<Address>
{
    public IEnumerable<Address> GetByIds(long [] Ids);
}

