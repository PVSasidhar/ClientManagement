namespace ApplicationCore.ClientAddress;
public class Address
{
    public long Id { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string CellPhoneNumber { get; set; }
    public string ResidentialPhoneNumber { get; set; }
    public string BusinessPhoneNumber { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public int StateProvince { get; set; }
    public int AddressTypeId { get; set; }
    public string PostalCode { get; set; }
    public DateTime ModifiedDate { get; set; }
    public long ClientId { get; set; }
}

