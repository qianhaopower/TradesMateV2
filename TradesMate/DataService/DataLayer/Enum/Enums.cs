using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
   public  enum TradeType
    {
        Electrician,
        Handyman,
        Plumber,
    }

    public enum UserType
    {
        Client = 0,
        Trade = 1
    }

    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    }


    public enum CompanyRole
    {
       Default = 0 ,
       Admin = 1,
       Contractor = 2, 
    }

    public enum ClientRole
    {
        Owner = 0,// owner can accept coowner's request, so the co owner can have the same access to a property
        CoOwner  =1,//
    }

}
