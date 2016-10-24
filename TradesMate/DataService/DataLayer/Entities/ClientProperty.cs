using System.Collections.Generic;

namespace EF.Data
{
    public class ClientProperty : BaseEntity
    {

        public ClientProperty()
        {
           
        }


    
        public int PropertyId { get; set; }
        public int ClientId { get; set; }


        public virtual Client Client { get; set; }
        public virtual Property Property { get; set; }

        public bool Confirmed { get; set; }

        public ClientRole Role { get; set; }



    }
}
