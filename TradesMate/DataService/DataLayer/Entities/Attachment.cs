using DataService.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Data
{
    public class Attachment : BaseEntity
    {

        public Attachment()
        {
          

        }

        public string Name { get; set; }
        public string  Url{ get; set; }
        public long SizeInBytes { get; set; }
        public AttachmentEntityType EntityType { get; set; }
        public AttachmentType Type { get; set; }

    }
}
