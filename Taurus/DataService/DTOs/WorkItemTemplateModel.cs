

using EF.Data;
using System;
using System.Collections.Generic;

namespace DataService.Models
{
    
    public class WorkItemTemplateModel
	{
		public int Id { get; set; }
		public string Description { get; set; }

		public string Name { get; set; }

		public TradeType TradeWorkType { get; set; }//user will have this type as well
		public string TemplateType { get; set; }//public, private

		public int CompanyId { get; set; }
		
		public DateTime ModifiedDateTime { get; set; }

	}


}