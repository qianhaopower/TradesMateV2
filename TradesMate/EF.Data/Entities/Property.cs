using System.Collections.Generic;

namespace EF.Data
{
    public class Property : BaseEntity
    {
        public Property()
        {
            SectionList = new List<Section>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
      
        public string Condition { get; set; }

        public string Narrative { get; set; }
        public string Comment { get; set; }


        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }

        public int ClientId { get; set; }


        public virtual Client Client { get; set; }


        public virtual ICollection<Section> SectionList {get;set;}
    }
}
