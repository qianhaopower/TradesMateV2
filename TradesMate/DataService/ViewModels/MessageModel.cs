﻿

using System;

namespace DataService.Models
{
    public class MessageModel
    {
        public int MessageType { get; set; }

        public string MessageText { get; set; }

        public bool Pending { get; set; }

        public string Role { get; set; }

        public string CompanyName { get; set; }
        public string MemberName { get; set; }

        public DateTime CreatTime { get; set; }

        public MessageResponseModel MessageResponse { get; set; }
        public string ClientName { get; set; }
        public string PropertyName { get; set; }

    }


}