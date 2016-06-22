using System.Collections.Generic;

namespace EF.Data
{
    public class Address : BaseEntity
    {

         public Address()
        {
            ClientList = new List<Client>();
            CompanyList = new List<Company>();
            PropertyList = new List<Property>();
        }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        public string PostCode { get; set; }
        
        public string State { get; set; }

        public string Suburb { get; set; }
        public string City { get; set; }

        public virtual ICollection<Client> ClientList { get; set; }

        public virtual ICollection<Company> CompanyList { get; set; }

        public virtual ICollection<Property> PropertyList { get; set; }



    }
}
