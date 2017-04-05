using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class AllocationModel
    {
     
        public string PropertyName   { get; set; }

        public string Description { get; set; }

        public int PropertyId { get; set; }

        public bool Allocated { get; set; }

    }


}