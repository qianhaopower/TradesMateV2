

using System;

namespace DataService.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public int MessageType { get; set; }

        public string MessageText { get; set; }

        public bool IsWaitingForResponse { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreateTime { get; set; }

        public bool HasResponse { get; set; }

    }

    public class MessageDetailModel
    {
        public int Id { get; set; }
        public int MessageType { get; set; }

        public string MessageText { get; set; }

        public bool IsWaitingForResponse { get; set; }
        public bool IsRead { get; set; }


        public string Role { get; set; }

        public string CompanyName { get; set; }
        public string MemberName { get; set; }

        public DateTime CreateTime { get; set; }

        public MessageResponseModel MessageResponse { get; set; }
        public string ClientName { get; set; }
        public string PropertyName { get; set; }

    }


}