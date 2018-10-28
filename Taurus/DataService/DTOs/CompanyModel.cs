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
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string CreditCard { get; set; }
        public int CompanyId { get; set; }
        public List<TradeType> TradeTypes { get; set; }
        public string ABN { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string ElectricianLicense { get; set; }

        public string PlumberLicense { get; set; }

        public string HandymanLicense { get; set; }

        public string AirconditioningLicense { get; set; }

        public string BuilderLicense { get; set; }
    }
   

}