using DataService.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Data
{
    public class EmailHistory : BaseEntity
    {

        public EmailHistory()
        {
        }

        public string From { get; set; }
        public string ToEmailAddress { get; set; }
        public int ToUserId { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
      

    }
}
