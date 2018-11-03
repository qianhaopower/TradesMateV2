using System.Collections.Generic;

namespace EF.Data
{
    public class Property : BaseEntity
    {
        public Property()
        {
            SectionList = new List<Section>();
            PropertyCompanies = new List<PropertyCompany>();
            PropertyAllocations = new List<PropertyAllocation>();
            ClientProperties = new List<ClientProperty>();
            Messages = new List<Message>();

        }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Condition { get; set; }

        public string Narrative { get; set; }
        public string Comment { get; set; }


        public int? AddressId { get; set; }

        public int? SystemPropertyCompanyId { get; set; }
        public virtual Address Address { get; set; }

        //public int ClientId { get; set; }


        //public virtual Client Client { get; set; }


        public virtual ICollection<Section> SectionList { get; set; }

        public virtual ICollection<PropertyCompany> PropertyCompanies { get; set; }
        public virtual ICollection<PropertyAllocation> PropertyAllocations { get; set; }

        public virtual ICollection<ClientProperty> ClientProperties { get; set; }

        public virtual ICollection<Message> Messages { get; set; }


        public string AddressDisplay
        {
            get
            {
                if (Address != null)
                    return string.Format("{0} {1} {2} {3} {4} {5}",
             Address.Line1,
             Address.Line2,
             Address.Line3,
              Address.Suburb,
              Address.State,
             Address.PostCode);
                else
                    return string.Empty;
            }
            set { }
            
        }
    }
}
