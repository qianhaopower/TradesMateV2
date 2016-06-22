using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public class Section : BaseEntity
    {

        public Section()
        {
            //SubSectionList = new List<SubSection>();
            this.WorkItemList = new List<WorkItem>();
        }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<WorkItem> WorkItemList { get; set; }


        //public virtual ICollection<SubSection> SubSectionList { get; set; }

        //public int SectionTemplateId { get; set; }
        //public virtual SectionTemplate TemplateRecord { get; set; }

        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

    }
}