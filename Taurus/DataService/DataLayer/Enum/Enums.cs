using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public enum TradeType
    {
        Electrician = 0,
        Handyman = 1,
        Plumber =2,
        Builder =3,
        AirConditioning =4,
    }

    public enum SectionType
    {
        Bedroom,
        LivingRoom,
        Bathroom,
        Kitchen,
        LaundryRoom,
        HallWay,
        Deck,
        Basement,
        Garden,
        Garage,
    }    
    
    public enum WorkItemStatus
    {
        NotStarted,
        InProgress,
        Pending,
        Completed,
        Canceled
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
        InviteJoinCompanyRequest = 3,
        WorkRequest = 4,
        AddPropertyCoOwner = 5,
        InviteClientToCompany =6,
    }

    public enum ResponseAction
    {
        Accept = 0,
        Reject = 1,
    }


    public enum AttachmentEntityType {
        Property  = 0,
        WorkItem  =1 ,
        CompanyLogo = 2,
        WorkItemTemplate =3,
    }

    public enum AttachmentType
    {
        Image = 0,
        Document = 1,
    }
}
