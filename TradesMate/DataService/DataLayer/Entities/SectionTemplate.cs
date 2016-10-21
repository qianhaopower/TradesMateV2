//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EF.Data
//{
    
//    public class SectionTemplate : BaseEntity
//    {
//        public SectionTemplate()
//        {
//            SubSectionTemplateList = new List<SubSectionTemplate>();
//            SectionList = new List<Section>();
//        }

//        public string Name { get; set; }
//        public string Description { get; set; }

//        public virtual ICollection<SubSectionTemplate> SubSectionTemplateList { get; set; }
//        public virtual ICollection<Section> SectionList { get; set; }


//        public int CompanyId { get; set;}
//        public virtual Company Company { get; set; }


//    }
//}
