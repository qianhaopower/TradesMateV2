using System.Collections.Generic;

namespace EF.Data
{
   
    public class CompanyService : BaseEntity
    {

        public CompanyService()
        {       
        }
        public int CompanyId { get; set; }

        public TradeType Type { get; set; }
      
        public virtual Company Company { get; set; }

    }
}
