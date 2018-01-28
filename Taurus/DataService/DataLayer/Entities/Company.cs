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
            //Properties = new List<Property>();
            PropertyCompanies = new List<PropertyCompany>();
            CompanyMembers = new List<CompanyMember>();
            Messages = new List<Message>();
            CompanyServices = new List<CompanyService>(); 
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public string CreditCard { get; set; }
        public string ABN { get; set; }
        public string Website { get; set; }
        public string AddressString { get; set; }

        public ICollection<WorkItemTemplate> WorkItemTemplateList {get;set;}


        public ICollection<PropertyCompany> PropertyCompanies { get; set; }

        public ICollection<CompanyMember> CompanyMembers { get; set; }

        public ICollection<CompanyService> CompanyServices { get; set; }

        // public virtual ICollection<ClientCompany> ClientCompanies { get; set; }

        public int? AddressId { get; set; }
        public virtual Address Address {get;set;}

        public virtual ICollection<Message> Messages { get; set; }
    }
}
