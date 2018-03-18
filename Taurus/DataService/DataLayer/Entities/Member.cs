using DataService.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Data
{
    public class Member : BaseEntity
    {

        public Member()
        {
            //Properties = new List<Property>();
            // ClientCompanies = new List<ClientCompany>();
            CompanyMembers = new List<CompanyMember>();
            Messages = new List<Message>();
        }
        public string FullName => FirstName + ' ' + LastName;
        public string FirstName { get; set; }
        public string  MiddleName{ get; set; }
        public string LastName { get; set; }

        public string Description { get; set; }

        public string MobileNumber { get; set; }
        public string Email { get; set; }

        //public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<CompanyMember> CompanyMembers { get; set; }



        //[ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Message> Messages { get; set; }



        //  public virtual ICollection<ClientCompany> ClientCompanies { get; set; }
    }

    public class MemberInfo
    {
        public Member Member { get; set; }

        public Company Company { get; set; }

        public CompanyMember CompanyMember { get; set; }

        public ApplicationUser User { get; set; }
    }

}
