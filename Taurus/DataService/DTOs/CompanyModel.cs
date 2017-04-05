using EF.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataService.Models
{
    public class CompanyModel
    {
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }


   
        [Display(Name = "Description")]
        public string Description { get; set; }

   
        [Display(Name = "Credit Card Number")]
        public string CreditCard { get; set; }


      
        public int CompanyId { get; set; }

        public List<TradeType> TradeTypes { get; set; }





    }


}