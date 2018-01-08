

using System;

namespace DataService.Models
{
    public class MessageResponseModel
    {

        //public int MessageId { get; set; }
        public string ResponseAction { get; set; }

        public string ResponseText { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsRead { get; set; }
        public string UserIdTo { get; set; }

    }


}