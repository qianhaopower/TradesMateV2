using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public class WorkItemTemplate: BaseEntity
    {
        public WorkItemTemplate()
        {
            WorkItemList = new List<WorkItem>();
        }

        public string Description { get; set; }

        public string Name { get; set; }

        public TradeType TradeWorkType { get; set; }//user will have this type as well
        public string TemplateType { get; set; }//public, private
            

        public virtual ICollection<WorkItem> WorkItemList { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
