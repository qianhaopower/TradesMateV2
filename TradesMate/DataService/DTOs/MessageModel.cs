

using EF.Data;
using System;

namespace DataService.Models
{
    //public class MessageModel
    //{
    //    public int Id { get; set; }
    //    public int MessageType { get; set; }

    //    public string MessageText { get; set; }

    //    public bool IsWaitingForResponse { get; set; }


    //    //display unread if
    //    /// <summary>
    //    /// 1. message if for the user and message is unread.
    //    /// 2. message response if for the user and message is unread
    //    /// </summary>
    //    public bool ShouldDisplayUnread { get; set; }

    //    public DateTime CreateTime { get; set; }

    //    public bool HasResponse { get; set; }

    //}

    public class MessageDetailModel
    {
        public int Id { get; set; }
        public int MessageType { get; set; }

        public string MessageText { get; set; }

        public bool IsWaitingForResponse { get; set; }
        public bool IsRead { get; set; }

        public string UserIdTo { get; set; }
        public string Mobile { get; set; }


        public string Role { get; set; }

        public string CompanyName { get; set; }
        public string MemberName { get; set; }

        public string PropertyAddress { get; set; } 
        public TradeType ServiceType { get; set; }
        public string Section { get; set; }

        public int? PropertyId { get; set; }
        public int? ClientId { get; set; }


        public int? CompanyId { get; set; }

        public DateTime CreateTime { get; set; }

        public MessageResponseModel MessageResponse { get; set; }
        public string ClientName { get; set; }
        public string PropertyName { get; set; }
        public bool HasResponse { get; internal set; }
        public bool ShouldDisplayUnread { get; internal set; }
        public string Title { get; internal set; }
    }


}