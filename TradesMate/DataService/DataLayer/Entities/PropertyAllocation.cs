using System.Collections.Generic;

namespace EF.Data
{
    //join table between CompanyMember and Property used for property allocation. 
    //Do not use the EF generated join table as we need add more property to it.
    public class PropertyAllocation : BaseEntity
    {

        public PropertyAllocation()
        {
           
        }


       
        public int CompanyMemberId { get; set; }
        public int PropertyId { get; set; }

      


        public virtual CompanyMember CompanyMember { get; set; }
        public virtual Property Property { get; set; }



    }
}
