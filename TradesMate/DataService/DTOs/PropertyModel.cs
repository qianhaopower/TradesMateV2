

using EF.Data;
using System;

namespace DataService.Models
{
  

    public class PropertyModel
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Condition { get; set; }

        public string Narrative { get; set; }
        public string Comment { get; set; }

        public string AddressDisplay { get; set; }
      
    }


}