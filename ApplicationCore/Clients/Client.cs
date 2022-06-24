using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.ClientAddress;
 
namespace ApplicationCore.Clients
{
    public class Client 
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public short Gender { get; set; }
        public DateTime ModifiedDate { get; set; }
        public IList<Address> ClientAddresses { get; set; }
    }
}
