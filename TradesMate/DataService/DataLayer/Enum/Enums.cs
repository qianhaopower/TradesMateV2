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

    public enum MessageType
    {
        AssignDefaultRole = 0,
        AssignDefaultRoleRequest = 1,
        AssignContractorRole = 2,
        nviteJoinCompanyRequest = 3,
        WorkRequest = 4,
        AddPropertyCoOwner = 5,
    }

    public enum ResponseAction
    {
        Accept = 0,
        Reject = 1,
    }

}
