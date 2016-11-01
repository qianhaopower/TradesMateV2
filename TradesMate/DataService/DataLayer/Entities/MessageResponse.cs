using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public class MessageResponse : BaseEntity
    {
        public MessageResponse()
        { 
        }

    

        public string UserIdFrom { get; set; }

        public string UserIdTo { get; set; }

        //public int MessageId { get; set; }
        public ResponseAction ResponseAction { get; set; }

        public string ResponseText { get; set; }

        public bool IsRead { get; set; }

        //public int MessageId { get; set; }


        public virtual Message Message {get;set;}
       
    }
}
