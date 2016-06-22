using System.Collections.Generic;

namespace EF.Data
{
    public class Client : BaseEntity
    {

        public Client()
        {
            Properties = new List<Property>();
        }
        public string FirstName { get; set; }
        public string  MiddleName{ get; set; }
        public string SurName { get; set; }

        public string Description { get; set; }

        public string MobileNumber { get; set; }
        public string Email { get; set; }
       
        public virtual ICollection<Property> Properties { get; set; }


        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}
