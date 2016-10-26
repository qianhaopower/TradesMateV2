using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public class Message : BaseEntity
    {
        public Message()
        { 
        }

        public MessageType MessageType { get; set; }

        public string MessageText { get; set; }

        public int UserIdFrom { get; set; }

        public int UserIdTo { get; set; }

        public bool Pending { get; set; }

        public ResponseAction ResponseAction { get; set; }

        public string ResponseText { get; set; }


        //all possible parameter for different message
        public int? CompanyId { get; set; }
        public int? MemberId { get; set; }
        public int? ClientId { get; set; }
        public int? PropertyId { get; set; }

        public string RoleName { get; set; }

        public virtual Company Company {get;set;}
        public virtual Member Member { get; set; }
        public virtual Client Client { get; set; }
        public virtual Property Property { get; set; }
    }
}
