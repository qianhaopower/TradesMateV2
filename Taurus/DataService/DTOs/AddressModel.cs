

using EF.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataService.Models
{
  

    public class AddressModel
    {
        [Required]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Suburb { get; set; }
        public string City { get; set; }

        public string AddressDisplay => $"{Line1} {Line2} {Line3} {Suburb} {PostCode}";
    }


}