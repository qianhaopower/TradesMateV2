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

        public string UserIdFrom { get; set; }

        public string UserIdTo { get; set; }

        //public bool HasResponse { get; set; }

        public bool IsWaitingForResponse { get; set; }// indicates if the message is waiting for response/

        public bool IsRead { get; set; }//indicates if the message has been open, once the Get MessageDetail happens we assume this message is read. 

        //public ResponseAction ResponseAction { get; set; }

        //public string ResponseText { get; set; }


        //all possible parameter for different message
        public int? CompanyId { get; set; }
        public int? MemberId { get; set; }
        public int? ClientId { get; set; }
        public int? PropertyId { get; set; }


        #region for work request
        //put these field in the message table for now. If Work request need more property we need consider add a workrequest info table

        public string PropertyAddress { get; set; } // only used if no propertyId is provided, i.e. new property
        public TradeType ServiceType { get; set; }
        public string Section { get; set; }// bedroom, bathroom etc.

        #endregion



        // public int? MessageResponseId { get; set; }


        public CompanyRole Role { get; set; }

        public virtual Company Company {get;set;}
        public virtual Member Member { get; set; }

        public virtual MessageResponse MessageResponse { get; set; }
        public virtual Client Client { get; set; }
        public virtual Property Property { get; set; }
    }
}
