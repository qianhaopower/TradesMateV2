using DataService.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Data
{
    public class Client : BaseEntity
    {

        public Client()
        {
            Properties = new List<Property>();
           // ClientCompanies = new List<ClientCompany>();
        }
        public string FirstName { get; set; }
        public string  MiddleName{ get; set; }
        public string SurName { get; set; }

        public string Description { get; set; }

        public string MobileNumber { get; set; }
        public string Email { get; set; }
       
        public virtual ICollection<Property> Properties { get; set; }



        //[ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }

      //  public virtual ICollection<ClientCompany> ClientCompanies { get; set; }
    }
}
