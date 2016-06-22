//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EF.Data
//{
//    public class SubSectionTemplate :BaseEntity
//    {
//        public SubSectionTemplate() {
//            WorkItemTemplateList = new List<WorkItemTemplate>();
//            SubSectionList = new List<SubSection>();
//        }


//        public string Name { get; set; }
//        public string Description { get; set; }
//        public virtual ICollection<WorkItemTemplate> WorkItemTemplateList { get; set; }
//        public virtual ICollection<SubSection> SubSectionList { get; set; }

//        public int SectionTemplateId { get; set; }
//        public virtual SectionTemplate SectionTemplate { get; set; }
//    }
//}
