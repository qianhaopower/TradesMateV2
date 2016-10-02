using System.Collections.Generic;

namespace EF.Data
{
    public class ClientCompany : BaseEntity
    {

        public ClientCompany()
        {
            Companies = new List<Company>();
            Clients = new List<Client>();
        }


        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public int ClientId { get; set; }
        public int CompanyId { get; set; }


        //public virtual Client Client { get; set; }
        //public virtual Property Property { get; set; }



    }
}
