

namespace DataService.Models
{
    
    public class SectionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int PropertyId { get; set; }  
    }
}