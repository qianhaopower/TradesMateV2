using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public class Company : BaseEntity
    {
        public Company()
        {
            WorkItemTemplateList = new List<WorkItemTemplate>();
            Properties = new List<Property>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public string CreditCard { get; set; }

        public ICollection<WorkItemTemplate> WorkItemTemplateList {get;set;}


        public ICollection<Property> Properties { get; set; }

        // public virtual ICollection<ClientCompany> ClientCompanies { get; set; }

        public int? AddressId { get; set; }
        public virtual Address Address {get;set;}
    }
}
