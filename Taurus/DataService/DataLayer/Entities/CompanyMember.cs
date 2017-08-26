using System;
using System.Collections.Generic;
using System.Linq;
namespace EF.Data
{
    //join table between company and member. 
    //Do not use the EF generated join table as we need add more property to it.
    public class CompanyMember : BaseEntity
    {

        public CompanyMember()
        {
            //Companies = new List<Company>();
            PropertyAllocations = new List<PropertyAllocation>();

        }


        //public virtual ICollection<Member> Members { get; set; }
        //public virtual ICollection<Company> Companies { get; set; }
        public int MemberId { get; set; }
        public int CompanyId { get; set; }

        public CompanyRole Role { get; set; }

        //incase the company ask the member to join and need confirm
        public bool Confirmed { get; set; }

        public string AllowedTradeTypesInternal { get; set; }

        public IEnumerable<TradeType> AllowedTradeTypes
        {
            get
            {
                if (string.IsNullOrEmpty(AllowedTradeTypesInternal))
                {
                    return new List<TradeType>();
                }

                return AllowedTradeTypesInternal.Split(',').ToList().Select(p => (TradeType)Enum.Parse(typeof(TradeType), p)).ToList();
            }
            set
            {
                if (value.Any())
                    AllowedTradeTypesInternal = value.Select(p => p.ToString()).Aggregate((x, y) => x.ToString() + "," + y.ToString());
                else
                    AllowedTradeTypesInternal = null;

            }
        }


        public virtual Member Member { get; set; }
        public virtual Company Company { get; set; }


        public virtual ICollection<PropertyAllocation> PropertyAllocations { get; set; }




    }
}
