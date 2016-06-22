//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EF.Data
//{
//    public class SubSection : BaseEntity
//    {
//        public SubSection() {
//            this.WorkItemList = new List<WorkItem>();
//        }

//        public string Name { get; set; }
//        public string Description { get; set; }

//        public virtual ICollection<WorkItem> WorkItemList { get; set; }


//        public int SubSectionTemplateId { get; set; }
//        public virtual SubSectionTemplate TemplateRecord { get; }

//        public int SectionId { get; set; }
//        public virtual Section Section { get; set; }
//    }
//}
