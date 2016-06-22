using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public class WorkItem : BaseEntity
    {
        public int Qauntity { get; set; }
        public string Description { get; set;  }

        public string Name { get; set; }

        public TradeType TradeWorkType { get; set; }

        public int WorkItemTemplateId { get; set; }
        public virtual WorkItemTemplate TemplateRecord { get; set; }

        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
    }
}
