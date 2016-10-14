using System.Collections.Generic;

namespace EF.Data
{
    public class PropertyCompany : BaseEntity
    {

        public PropertyCompany()
        {
           
        }


    
        public int PropertyId { get; set; }
        public int CompanyId { get; set; }


        public virtual Company Company { get; set; }
        public virtual Property Property { get; set; }



    }
}
